using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

public partial class Admin_CategoryMaster : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
    DataTable dt11 = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {


            //DropDownListUnits.Items.Clear();
            //DropDownListUnits.Items.Add("Nos.");
            //DropDownListUnits.Items.Add("Unit.");
            //DropDownListUnits.Items.Add("Pieces.");
            //GridView();

            if (Request.QueryString["ID"] != null)
            {
                string id = Decrypt(Request.QueryString["ID"].ToString());
                loadData(id);
                btnSubmit.Text = "Update";
                hidden.Value = id;

            }

        }

    }


    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string createdby = Session["adminname"].ToString();
        string updatedby = Session["adminname"].ToString();


        try
        {
            if (btnSubmit.Text == "Submit")
            {
                int id;

                con.Open();
                SqlCommand cmd1 = new SqlCommand("SELECT [ID],[CategoryName],[CreateBy],[CreateDate],[UpdateBy],[UpdateDate],[IsStatus] FROM [tbl_Category] WHERE CategoryName=@CategoryName AND isdeleted='0'", con);
                cmd1.Parameters.AddWithValue("@CategoryName", txtCategoryName.Text);
                SqlDataReader reader = cmd1.ExecuteReader();

                if (reader.Read())
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Data Already Exist.');window.location ='CategoryMaster.aspx';", true);
                }
                else
                {
                    con.Close();
                    DateTime Date = DateTime.Now;
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SP_Category", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CategoryName", txtCategoryName.Text);
                    cmd.Parameters.AddWithValue("@CreateBy", createdby);
                    cmd.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@isdeleted", '0');
                    bool isactive = DropDownListisActive.Text == "Yes";
                    cmd.Parameters.AddWithValue("@IsStatus", isactive);
                    cmd.Parameters.AddWithValue("@Action", "Insert");
                    cmd.ExecuteNonQuery();
                    con.Close();

                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Saved Successfully');", true);
                }
            }
            else if (btnSubmit.Text == "Update")
            {
                con.Open();
                SqlCommand cmdCheck = new SqlCommand("SELECT [ID] FROM [tbl_Category] WHERE CategoryName=@CategoryName AND isdeleted='0' AND ID <> @ID", con);
                cmdCheck.Parameters.AddWithValue("@CategoryName", txtCategoryName.Text);
                cmdCheck.Parameters.AddWithValue("@ID", Convert.ToInt32(hidden.Value));
                SqlDataReader readerCheck = cmdCheck.ExecuteReader();

                if (readerCheck.Read())
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Data Already Exist.');window.location ='CategoryMaster.aspx';", true);
                    con.Close();
                    return;
                }
                con.Close();

                DateTime Date = DateTime.Now;
                con.Open();
                SqlCommand cmd = new SqlCommand("SP_Category", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CategoryName", txtCategoryName.Text);
                cmd.Parameters.AddWithValue("@UpdateBy", updatedby);
                cmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@ID", Convert.ToInt32(hidden.Value));
                bool isactive = DropDownListisActive.Text == "Yes";
                cmd.Parameters.AddWithValue("@IsStatus", isactive);
                cmd.Parameters.AddWithValue("@Action", "Update");
                cmd.ExecuteNonQuery();
                con.Close();

                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Updated Successfully');", true);
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }



    protected void btnCancel_Click(object sender, EventArgs e)
    {
        txtCategoryName.Text = "";
        Response.Redirect("CategoryList.aspx");
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

    protected void loadData(string id)
    {
        try
        {
            SqlDataAdapter sad = new SqlDataAdapter("SELECT [ID],[CategoryName],[CreateBy],[CreateDate],[UpdateBy],[UpdateDate],[IsStatus] FROM [tbl_Category] where ID='" + id + "' ", con);

            DataTable dt = new DataTable();
            sad.Fill(dt);

            if (dt.Rows.Count > 0)
            {

                txtCategoryName.Text = dt.Rows[0]["CategoryName"].ToString();
                string flgStatus = "";
                if (dt.Rows[0]["IsStatus"].ToString() == "False")
                {
                    flgStatus = "No";
                }
                else
                {
                    flgStatus = "Yes";
                }
                DropDownListisActive.Text = flgStatus;
            }
            else
            {

            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public string Decrypt(string cipherText)
    {
        string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        cipherText = cipherText.Replace(" ", "+");
        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                }
                cipherText = Encoding.Unicode.GetString(ms.ToArray());
            }
        }
        return cipherText;
    }

    protected void gv_Comp_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblisstatus = (Label)e.Row.FindControl("lblisstatus") as Label;

            if (lblisstatus.Text == "True")

            {
                lblisstatus.Text = "Active";
            }

            else

            {

                lblisstatus.Text = "DeActive";

            }

        }

    }

    

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetCompList(string prefixText, int count)
    {
        return AutoFillComplist(prefixText);
    }

    public static List<string> AutoFillComplist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT CategoryName from tbl_Category where " + "CategoryName like @Search + '%' AND isdeleted='0'";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> CategoryName = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        CategoryName.Add(sdr["CategoryName"].ToString());
                    }
                }
                con.Close();
                return CategoryName;
            }

        }
    }
    

    protected void lnkrefresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("CategoryMaster.aspx");
    }
}
