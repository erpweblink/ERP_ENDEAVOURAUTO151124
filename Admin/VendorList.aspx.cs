using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.Security.Cryptography;
using System.IO;

public partial class Reception_VendorList : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);

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
            SqlDataAdapter sad = new SqlDataAdapter("SELECT [VendorId],[VendorName],[GSTNo],[StateCode],[PanNo],[AddreLine1],[AddreLine2],[AddreLine3],[Area],[Email],[City],[Country],[MobNo],[PostalCode],[Contactper1],[ContactNumb1],[Contactper2],[ContactNumb2],[IsStatus],[CreatedBy],[CreatedDate],[UpdatedBy],[UpdatedDate] FROM [tblVendor] where isdeleted='0' ORDER BY CreatedDate Desc", con);
            sad.Fill(dt);
            gv_Vendor.EmptyDataText = "Not Records Found";
            gv_Vendor.DataSource = dt;
            gv_Vendor.DataBind();

            con.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void gv_Vendor_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName == "RowEdit")
        {
            Response.Redirect("Vendor.aspx?VendorId=" + encrypt(e.CommandArgument.ToString()) + "");
        }

        if (e.CommandName == "RowDelete")
        {
            SqlCommand cmddelete = new SqlCommand("update tblVendor set isdeleted='1' where VendorId=@vendorId", con);
            cmddelete.Parameters.AddWithValue("@vendorId", Convert.ToInt32(e.CommandArgument.ToString()));
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

    protected void btncreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("Vendor.aspx");
    }

    protected void gv_Vendor_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_Vendor.PageIndex = e.NewPageIndex;
        GridView();
    }
    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetVendorList(string prefixText, int count)
    {
        return AutoFillVendorlist(prefixText);
    }

    public static List<string> AutoFillVendorlist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT VendorName from tblVendor where " + "VendorName like @Search + '%' AND isdeleted='0'";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> VendorName = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        VendorName.Add(sdr["VendorName"].ToString());
                    }
                }
                con.Close();
                return VendorName;
            }

        }
    }

    protected void lnkBtnsearch_Click(object sender, EventArgs e)
    {
       try
        {
            if (string.IsNullOrEmpty(txtSearch.Text))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Search Vendor Name');", true);

            }
            if (string.IsNullOrEmpty(txtSearch.Text))
            {
                GridView();

            }
            else
            {
                
                DataTable dt = new DataTable();

                SqlDataAdapter sad = new SqlDataAdapter("SELECT [VendorId],[VendorName],[GSTNo],[StateCode],[PanNo],[AddreLine1],[AddreLine2],[AddreLine3],[Area],[Email],[City],[Country],[MobNo],[PostalCode],[Contactper1],[ContactNumb1],[Contactper2],[ContactNumb2],[IsStatus],[CreatedBy],[CreatedDate],[UpdatedBy],[UpdatedDate] FROM [tblVendor] where VendorName='" + txtSearch.Text+ "' AND isdeleted='0'", con);
                sad.Fill(dt);
                gv_Vendor.EmptyDataText = "Not Records Found";
                gv_Vendor.DataSource = dt;
                gv_Vendor.DataBind();
               
            }


            //GridView();
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void gv_Vendor_RowDataBound(object sender, GridViewRowEventArgs e)
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
    SqlDataAdapter sad;
    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();

            if (ddlStatus.Text == "All")
            {
                sad = new SqlDataAdapter("SELECT [VendorId],[VendorName],[GSTNo],[StateCode],[PanNo],[AddreLine1],[AddreLine2],[AddreLine3],[Area],[Email],[City],[Country],[MobNo],[PostalCode],[Contactper1],[ContactNumb1],[Contactper2],[ContactNumb2],[IsStatus],[CreatedBy],[CreatedDate],[UpdatedBy],[UpdatedDate] FROM [tblVendor] where isdeleted='0' ", con);
            }
            else
            {
            sad = new SqlDataAdapter("SELECT [VendorId],[VendorName],[GSTNo],[StateCode],[PanNo],[AddreLine1],[AddreLine2],[AddreLine3],[Area],[Email],[City],[Country],[MobNo],[PostalCode],[Contactper1],[ContactNumb1],[Contactper2],[ContactNumb2],[IsStatus],[CreatedBy],[CreatedDate],[UpdatedBy],[UpdatedDate] FROM [tblVendor] where IsStatus='" + ddlStatus.Text + "' AND isdeleted='0'", con);
            }
            sad.Fill(dt);
            gv_Vendor.EmptyDataText = "Not Records Found";
            gv_Vendor.DataSource = dt;
            gv_Vendor.DataBind();
        }
        catch (Exception)
        {

            throw;
        }
    }

    protected void lnkrefresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("VendorList.aspx");
    }
}