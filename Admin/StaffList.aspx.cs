using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Security.Cryptography;
using System.IO;
using System.Text;


public partial class Admin_StaffList : System.Web.UI.Page

{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            gridRecord();
        }
    }

    protected void  gridRecord()
    {
        try
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlDataAdapter sad = new SqlDataAdapter("select * from tblStaffMaster where  isdeleted='0' ORDER BY Createddate Desc", con);
            sad.Fill(dt);
            gv_stafflist.EmptyDataText = "Not Records Found";
            gv_stafflist.DataSource = dt;
            gv_stafflist.DataBind();
             
            con.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void gv_stafflist_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if(e.CommandName=="RowEdit")
        {
            Response.Redirect("staffMaster.aspx?Id=" + encrypt(e.CommandArgument.ToString()) + "");
        }
        if (e.CommandName == "RowDelete")
        {
            SqlCommand cmddelete = new SqlCommand("update tblStaffMaster set isdeleted='1' where Id=@Id", con);
            cmddelete.Parameters.AddWithValue("@Id", Convert.ToInt32(e.CommandArgument.ToString()));
            cmddelete.Parameters.AddWithValue("@isdeleted", '1');
            con.Open();
            cmddelete.ExecuteNonQuery();
            con.Close();
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Delete Sucessfully');", true);

            //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Delete sucessfully!!');window.location ='CustomerList.aspx';", true);

            gridRecord();

        }
    }

    public string encrypt(string encryptString)
    {
        string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                encryptString = Convert.ToBase64String(ms.ToArray());
            }
        }
        return encryptString;
    }

    protected void btncreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("staffMaster.aspx");
    }

    protected void lnkBtnsearch_Click(object sender, EventArgs e)
    {
        try
        {
            
            if (string.IsNullOrEmpty(txtSearch.Text))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Search Employee Name');", true);
                gridRecord();
            }
            else
            {

                DataTable dt = new DataTable();

                SqlDataAdapter sad = new SqlDataAdapter("select * from tblStaffMaster where EmpName='"+txtSearch.Text+"'", con);
                sad.Fill(dt);
                gv_stafflist.EmptyDataText = "Not Records Found";
                gv_stafflist.DataSource = dt;
                gv_stafflist.DataBind();
            }


            //GridView();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void lnkrefresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("StaffList.aspx");
    }

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetEmployeeList(string prefixText, int count)
    {
        return AutoFillEmployeelist(prefixText);
    }

    public static List<string> AutoFillEmployeelist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT EmpName from tblStaffMaster where " + "EmpName like @Search + '%' AND isdeleted='0'";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> EmpName = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        EmpName.Add(sdr["EmpName"].ToString());
                    }
                }
                con.Close();
                return EmpName;
            }

        }
    }

    protected void gv_stafflist_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_stafflist.PageIndex = e.NewPageIndex;
        gridRecord();
       
    }
}