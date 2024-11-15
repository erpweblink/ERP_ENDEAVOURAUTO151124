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

public partial class Admin_EngineerList : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
    string id;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            RoleGrid();
        }
    }

    protected void RoleGrid()
    {
        try
        {
            DataTable dt = new DataTable();
            SqlDataAdapter sad = new SqlDataAdapter("SELECT  * FROM [tbl_Engineer] ", con);
            sad.Fill(dt);
            gv_Role.EmptyDataText = "No Record Found";
            gv_Role.DataSource = dt;
            gv_Role.DataBind();
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void gv_Role_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "RowEdit")
        {
            Response.Redirect("EngineerMaster.aspx?Id=" + encrypt(e.CommandArgument.ToString()) + "");

            //string id = e.CommandArgument.ToString();
            //loadData(id);
        }
        if (e.CommandName == "RowDelete")
        {
            //SqlCommand cmddelete = new SqlCommand("update tbl_Engineer set isdeleted='1' where Id=@Id", con);
            //cmddelete.Parameters.AddWithValue("@Id", Convert.ToInt32(e.CommandArgument.ToString()));
            //cmddelete.Parameters.AddWithValue("@isdeleted", '1');
            //con.Open();
            //cmddelete.ExecuteNonQuery();
            //con.Close();
            //ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Deleted Sucessfully');", true);
            //RoleGrid();
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
    protected void gv_Role_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    Label lblisstatu = (Label)e.Row.FindControl("lblIsActive") as Label;
        //    if (lblisstatu.Text == "True")

        //    {
        //        lblisstatu.Text = "Active";
        //    }

        //    else

        //    {

        //        lblisstatu.Text = "DeActive";

        //    }
        //}
    }
    //protected void lnkBtnsearch_Click(object sender, EventArgs e)
    //{
    //    if (string.IsNullOrEmpty(txtSearch.Text))
    //    {
    //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Search Engineer Name');", true);

    //    }
    //    if (string.IsNullOrEmpty(txtSearch.Text))
    //    {
    //        RoleGrid();
    //    }
    //    else
    //    {

    //        DataTable dt = new DataTable();

    //        SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM [tblRole] where isdeleted='0' AND Role='" + txtSearch.Text + "'", con);
    //        sad.Fill(dt);
    //        gv_Role.EmptyDataText = "Not Records Found";
    //        gv_Role.DataSource = dt;
    //        gv_Role.DataBind();
    //    }
    //}

    protected void lnkrefresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("RoleList.aspx");
    }
    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetRoleList(string prefixText, int count)
    {
        return AutoFillRolelist(prefixText);
    }

    public static List<string> AutoFillRolelist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT Role from [tblRole] where " + "Role like @Search + '%' AND isdeleted='0'";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> Role = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        Role.Add(sdr["Role"].ToString());
                    }
                }
                con.Close();
                return Role;
            }

        }
    }

    //protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        SqlDataAdapter sad;
    //        DataTable dt = new DataTable();

    //        if (ddlStatus.Text == "All")
    //        {
    //            sad = new SqlDataAdapter("SELECT  * FROM [tbl_RoleMaster] where isdeleted='0'", con);
    //        }
    //        else
    //        {
    //            sad = new SqlDataAdapter("SELECT * FROM [tbl_RoleMaster] where isdeleted='0' AND IsActive='" + ddlStatus.SelectedValue + "'", con);
    //        }
    //        sad.Fill(dt);
    //        gv_Role.EmptyDataText = "Not Records Found";
    //        gv_Role.DataSource = dt;
    //        gv_Role.DataBind();
    //    }
    //    catch (Exception)
    //    {

    //        throw;
    //    }
    //}

    protected void gv_Role_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_Role.PageIndex = e.NewPageIndex;
        RoleGrid();
    }

    protected void btncreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("EngineerMaster.aspx");
    }
}