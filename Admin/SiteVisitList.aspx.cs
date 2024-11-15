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

public partial class Admin_SiteVisitList : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
    DataTable dt11 = new DataTable();
    UserAuthorization objUserAuthorization = new UserAuthorization();
    string create1, Delete1, Update1, view1, Report1;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GridView();
        }
    }

    void GridView()
    {
        try
        {
            DataTable dt = new DataTable();
            con.Open();
           // SqlDataAdapter sad = new SqlDataAdapter("SELECT [Prodid],[ProdName],[TechSpeci],[ModelNo],SerialNo,[IsStatus],[CreateBy],[CreateDate],[updateBy],[updateDate]FROM [tblProduct] where isdeleted='0' ORDER BY CreateDate", con);
            SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM tbl_sitevisit where isdeleted='0' ORDER BY CreateDate", con);
            sad.Fill(dt);
            gv_Prod.EmptyDataText = "Not Records Found";
            gv_Prod.DataSource = dt;
            gv_Prod.DataBind();
            con.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> Getsite(string prefixText, int count)
    {
        return AutoFillGetsitelist(prefixText);
    }

    public static List<string> AutoFillGetsitelist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT Custname from tbl_sitevisit where " + "Custname like @Search + '%' AND isdeleted='0'";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> CustomerName = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        CustomerName.Add(sdr["Custname"].ToString());
                    }
                }
                con.Close();
                return CustomerName;
            }

        }
    }


    protected void lnkrefresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("SiteVisitList.aspx");
    }

    SqlDataAdapter sadd;
    

    protected void gv_Prod_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_Prod.PageIndex = e.NewPageIndex;
        GridView();
    }

    protected void btnceate_Click(object sender, EventArgs e)
    {
        Response.Redirect("Sitevisitnew.aspx");
    }

    protected void gv_Prod_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "RowEdit")
        {
            string ash = e.CommandArgument.ToString();
            Response.Redirect("Sitevisitnew.aspx?ID=" + encrypt(e.CommandArgument.ToString()) + "");
        }

        if (e.CommandName == "RowDelete")
        {
            SqlCommand cmddelete = new SqlCommand("UPDATE tblProduct set isdeleted='1' where Prodid=@Prodid", con);
            cmddelete.Parameters.AddWithValue("@Prodid", Convert.ToInt32(e.CommandArgument.ToString()));
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

    protected void txtSearch_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(txtSearch.Text))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Search Site Name');", true);
            }
            if (string.IsNullOrEmpty(txtSearch.Text))
            {
                GridView();
            }
            else
            {
                DataTable dt = new DataTable();

               // SqlDataAdapter sad = new SqlDataAdapter("SELECT [Custid],[CustomerName],[GSTNo],[StateCode],[PanNo],[AddresLine1],[AddresLine2],[AddresLine3],[Area],[Email],[City],[Country],[MobNo],[PostalCode],[ContactPerName1],[ContactPerNo1],[ContactPerName2],[ContactPerNo2],[IsStatus],[CreatedBy],[Createddate],[UpdatedBy],[UpdatedDate] FROM [tblCustomer] where [CustomerName]='" + txtSearch.Text + "' AND isdeleted='0' ", con);
                SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM [tbl_sitevisit] WHERE [Custname]='" + txtSearch.Text + "' AND isdeleted ='0' ", con);
                sad.Fill(dt);
                gv_Prod.EmptyDataText = "Not Records Found";
                gv_Prod.DataSource = dt;
                gv_Prod.DataBind();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}