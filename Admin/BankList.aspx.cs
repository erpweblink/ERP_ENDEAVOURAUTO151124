using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_BankList : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GridView();
        }

      //  GridView();
    }

    void GridView()
    {
        try
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM [tbl_Bank] ", con);
            sad.Fill(dt);
            GVBank.EmptyDataText = "Not Records Found";
            GVBank.DataSource = dt;
            GVBank.DataBind();

            con.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //SqlDataAdapter sad;
    //protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        DataTable dt = new DataTable();

    //        if (ddlStatus.Text == "All")
    //        {
    //            sad = new SqlDataAdapter("SELECT * FROM [tbl_Bank]  ", con);
    //        }
    //        else
    //        {
    //            sad = new SqlDataAdapter("SELECT * FROM [tbl_Bank]", con);
    //        }
    //        sad.Fill(dt);
    //        GVBank.EmptyDataText = "Not Records Found";
    //        GVBank.DataSource = dt;
    //        GVBank.DataBind();
    //    }
    //    catch (Exception)
    //    {

    //        throw;
    //    }
    //}

    protected void lnkBtnsearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(txtSearch.Text))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Search Bank Name');", true);
            }
            if (string.IsNullOrEmpty(txtSearch.Text))
            {
                GridView();
            }
            else
            {

                DataTable dt = new DataTable();

                SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM [tbl_Bank] where [BankName]='" + txtSearch.Text + "'  ", con);
                sad.Fill(dt);
                GVBank.EmptyDataText = "Not Records Found";
                GVBank.DataSource = dt;
                GVBank.DataBind();
            }


            //GridView();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetBankList(string prefixText, int count)
    {
        return AutoFillBanklist(prefixText);
    }

    public static List<string> AutoFillBanklist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                //com.CommandText = "select DISTINCT BankName from tbl_Bank where " + "BankName like @Search + '%' AND isdeleted='0'";
                com.CommandText = "select DISTINCT BankName from [EndeavourAuto].[tbl_Bank] where " + "BankName like @Search + '%' ";


                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> BankName = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        BankName.Add(sdr["BankName"].ToString());
                    }
                }
                con.Close();
                return BankName;
            }

        }
    }

    protected void GVBank_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            LinkButton lnkbtnEdit = e.Row.FindControl("lnkbtnEdit") as LinkButton;
            LinkButton lnkbtnDelete = e.Row.FindControl("lnkbtnDelete") as LinkButton;


            string Id = Session["Id"].ToString();


            DataTable Dtt = new DataTable();
            SqlDataAdapter Sdd = new SqlDataAdapter("Select * FROM tblUserRoleAuthorization where UserID = '" + Id + "' AND PageName = 'BankList.aspx' AND PagesView = '1'", con);
            Sdd.Fill(Dtt);
            if (Dtt.Rows.Count > 0)
            {
                //btnAddDelChallan.Visible = false;
                GVBank.Columns[6].Visible = false;
                lnkbtnEdit.Visible = false;
                lnkbtnDelete.Visible = false;
                btncreate.Visible = false;
            }
        }
    }

    protected void btncreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("AddBank.aspx");
    }

    protected void GVBank_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "RowEdit")
        {
            Response.Redirect("AddBank.aspx?id=" + encrypt(e.CommandArgument.ToString()) + "");
        }

        if (e.CommandName == "RowDelete")
        {
            SqlCommand cmddelete = new SqlCommand("DELETE FROM tbl_Bank  where id=@id", con);
            cmddelete.Parameters.AddWithValue("@id", Convert.ToInt32(e.CommandArgument.ToString()));
            //cmddelete.Parameters.AddWithValue("@isdeleted", '1');
            con.Open();
            cmddelete.ExecuteNonQuery();
            con.Close();
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Delete Sucessfully');", true);

            Response.Redirect("BankList.aspx");
            //GridView();
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
}