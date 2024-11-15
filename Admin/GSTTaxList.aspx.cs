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

public partial class Admin_GSTTaxList : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            gridrecord();
            if (Request.QueryString["Id"] != null)
            {
                string id = Decrypt(Request.QueryString["Id"].ToString());
               
               

            }
        }
    }

   
    protected void gridrecord()
    {
        try
        {
            DataTable dt = new DataTable();
            SqlDataAdapter sad = new SqlDataAdapter("select * from tblGSTTaxMaster where isdeleted='0' ORDER BY CreatedDate Desc", con);
            sad.Fill(dt);
            gv_gsttax.DataSource = dt;
            gv_gsttax.DataBind();
            gv_gsttax.EmptyDataText = "Record Not Found";
        }
        catch (Exception)
        {

            throw;
        }
    }

    protected void gv_gsttax_RowDataBound(object sender, GridViewRowEventArgs e)
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

    protected void gv_gsttax_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "RowEdit")
        {
            Response.Redirect("GstTaxMaster.aspx?Id=" + encrypt(e.CommandArgument.ToString()) + "");
        }

        if (e.CommandName == "RowDelete")
        {

            SqlCommand cmddelete = new SqlCommand("update tblGSTTaxMaster set isdeleted='1' where Id=@Id", con);
            cmddelete.Parameters.AddWithValue("@Id", Convert.ToInt32(e.CommandArgument.ToString()));
            cmddelete.Parameters.AddWithValue("@isdeleted", '1');
            con.Open();
            cmddelete.ExecuteNonQuery();
            con.Close();
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Delete Sucessfully');", true);

            //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Delete sucessfully!!');window.location ='CustomerList.aspx';", true);

            //GridView();
        }
    }
    SqlDataAdapter sad;
    protected void ddlStatus_TextChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();

            if (ddlStatus.Text == "All")
            {
                sad = new SqlDataAdapter("SELECT  * from tblGSTTaxMaster where isdeleted='0'", con);
            }
            else
            {
                sad = new SqlDataAdapter("SELECT  * from tblGSTTaxMaster where isactive='" + ddlStatus.SelectedValue + "' AND  isdeleted='0'", con);
            }
            sad.Fill(dt);
            gv_gsttax.EmptyDataText = "Not Records Found";
            gv_gsttax.DataSource = dt;
            gv_gsttax.DataBind();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Success", "scrollToElement();", true);

        }
        catch (Exception)
        {

            throw;
        }
    }

    protected void lnkBtnsearch_Click(object sender, EventArgs e)
    {
        DataTable dt1 = new DataTable();
        SqlDataAdapter sad1 = new SqlDataAdapter("select * from tblGSTTaxMaster where TaxName='" + txtname.Text + "'", con);
        sad1.Fill(dt1);
        gv_gsttax.DataSource = dt1;
        gv_gsttax.DataBind();
        gv_gsttax.EmptyDataText = "Record Not Found";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Success", "scrollToElement();", true);

    }

    protected void lnkrefresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("GstTaxMaster.aspx");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Success", "scrollToElement();", true);

    }

    protected void gv_gsttax_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_gsttax.PageIndex = e.NewPageIndex;
        gridrecord();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Success", "scrollToElement();", true);

    }

    protected void btncreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("GstTaxMaster.aspx");
    }
}