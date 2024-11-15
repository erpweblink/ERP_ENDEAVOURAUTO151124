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

public partial class Admin_UserMasterList : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
          
            GridView();
            if (Request.QueryString["Id"] != null)
            {
                string id = Decrypt(Request.QueryString["Id"].ToString());
                         
                hidden.Value = id;
            }
            else
            {

            }
        }
    }

  
   

    protected void lnkBtnsearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(txtSearch.Text))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Search User Name');", true);

            }
            if (string.IsNullOrEmpty(txtSearch.Text))
            {
                GridView();
            }
            else
            {

                DataTable dt = new DataTable();

                SqlDataAdapter sad = new SqlDataAdapter("SELECT [Id],[name],[pass],[ConfamPass],[MobileNumber],[role],[IsActive],[Email],[CreatedBy],[CreatedDate],[updatedBy],[updatedDate] from LogIn  where [name]='" + txtSearch.Text + "' AND isdeleted='0'", con);
                sad.Fill(dt);
                gv_user.EmptyDataText = "Not Records Found";
                gv_user.DataSource = dt;
                gv_user.DataBind();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    void GridView()
    {
        try
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlDataAdapter sad = new SqlDataAdapter("SELECT [Id],[name],[pass],[ConfamPass],[MobileNumber],[role],roleId,[IsActive],[Email],[CreatedBy],[CreatedDate],[updatedBy],[updatedDate] from LogIn where isdeleted='0' ORDER BY CreatedDate", con);
            sad.Fill(dt);
            gv_user.EmptyDataText = "Not Records Found";
            gv_user.DataSource = dt;
            gv_user.DataBind();

            con.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    SqlDataAdapter sad;
    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {

        try
        {
            DataTable dt = new DataTable();

            if (ddlStatus.Text == "All")
            {
                sad = new SqlDataAdapter("SELECT [Id],[name],[pass],[ConfamPass],[MobileNumber],[role],roleId,[IsActive],[Email],[CreatedBy],[CreatedDate],[updatedBy],[updatedDate] from LogIn where isdeleted='0' ", con);
            }
            else
            {
                sad = new SqlDataAdapter("SELECT [Id],[name],[pass],[ConfamPass],[MobileNumber],[role],roleId,[IsActive],[Email],[CreatedBy],[CreatedDate],[updatedBy],[updatedDate] from LogIn where [IsActive]='" + ddlStatus.SelectedValue + "' AND isdeleted='0' ", con);
            }
            sad.Fill(dt);
            gv_user.EmptyDataText = "Not Records Found";
            gv_user.DataSource = dt;
            gv_user.DataBind();
        }
        catch (Exception)
        {

            throw;
        }

    }

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetUserList(string prefixText, int count)
    {
        return AutoFillUserlist(prefixText);
    }

    public static List<string> AutoFillUserlist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT name from LogIn where " + "name like @Search + '%' AND isdeleted='0'";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> name = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        name.Add(sdr["name"].ToString());
                    }
                }
                con.Close();
                return name;
            }

        }
    }

   

    protected void gv_user_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "RowEdit")
        {
            Response.Redirect("UserMaster.aspx?Id=" + encrypt(e.CommandArgument.ToString()) + "");
        }
        if (e.CommandName == "RowDelete")
        {
            SqlCommand cmddelete = new SqlCommand("update LogIn set isdeleted='1' where Id=@Id", con);
            cmddelete.Parameters.AddWithValue("@Id", Convert.ToInt32(e.CommandArgument.ToString()));
            cmddelete.Parameters.AddWithValue("@isdeleted", '1');
            con.Open();
            cmddelete.ExecuteNonQuery();
            con.Close();
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Delete Sucessfully');", true);

            //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Delete sucessfully!!');window.location ='CustomerList.aspx';", true);

            GridView();

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

    protected void gv_user_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblisstatu = (Label)e.Row.FindControl("lblIsActive") as Label;
            if (lblisstatu.Text == "True")

            {
                lblisstatu.Text = "Active";
            }

            else
            {
                lblisstatu.Text = "DeActive";

            }
        }
    }


    protected void CheckBoxconpass_CheckedChanged(object sender, EventArgs e)
    {
        //txtconpassword.TextMode.
    }

    protected void lnkrefresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("UserMasterList.aspx");
    }

    protected void LnkBtnsearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(txtSearch.Text))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Search User Name');", true);

            }
            if (string.IsNullOrEmpty(txtSearch.Text))
            {

                GridView();

            }
            else
            {

                DataTable dt = new DataTable();

                SqlDataAdapter sad = new SqlDataAdapter("SELECT [Id],[name],[pass],[ConfamPass],[MobileNumber],[role],[IsActive],[Email],[CreatedBy],[CreatedDate],[updatedBy],[updatedDate] from LogIn  where [name]='" + txtSearch.Text + "' AND isdeleted='0'", con);
                sad.Fill(dt);
                gv_user.EmptyDataText = "Not Records Found";
                gv_user.DataSource = dt;
                gv_user.DataBind();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("UserMasterList.aspx");
    }
    protected void gv_user_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_user.PageIndex = e.NewPageIndex;
        GridView();
    }

    protected void btncreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("UserMaster.aspx");
    }
}