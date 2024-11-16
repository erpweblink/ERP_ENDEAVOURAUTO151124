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

public partial class Admin_Sitevisitnew : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
    DataTable dt11 = new DataTable();
    DataTable dtpers = new DataTable();
    UserAuthorization objUserAuthorization = new UserAuthorization();
    string create1, Delete1, Update1, view1, Report1;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillEngName();

            if (Request.QueryString["ID"] != null)
            {
                string id = Decrypt(Request.QueryString["ID"].ToString());
                loadData(id);
                btnSubmit.Text = "Update";
                hidden.Value = id;
            }
        }
    }

    void gvbind_Company()
    {
        try
        {
            string UserCompany = Session["UserCompany"].ToString();
            DataTable dt = new DataTable();
            SqlDataAdapter sad = new SqlDataAdapter("SELECT [Prodid],[ProdName],[TechSpeci],[ModelNo],SerialNo,[IsStatus],[CreateBy],[CreateDate],[updateBy],[updateDate]FROM [tblProduct] where isdeleted='0'", con);
            sad.Fill(dt);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FillEngName()
    {
        SqlDataAdapter ad = new SqlDataAdapter("SELECT * FROM [tbl_Engineer]", con);
        DataTable dt = new DataTable();
        ad.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            txtengineername.DataSource = dt;
            // ddlcompnay.DataValueField = "ID";
            txtengineername.DataTextField = "EngineerName";
            txtengineername.DataBind();
            txtengineername.Items.Insert(0, " --  Select Engineer Name -- ");
        }
    }

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetCust(string prefixText, int count)
    {
        return AutoFillGetCustlist(prefixText);
    }

    public static List<string> AutoFillGetCustlist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT CustomerName from tblCustomer where " + "CustomerName like @Search + '%' AND isdeleted='0'";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> CustomerName = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        CustomerName.Add(sdr["CustomerName"].ToString());
                    }
                }
                con.Close();
                return CustomerName;
            }

        }
    }

    void GridView()
    {
        try
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlDataAdapter sad = new SqlDataAdapter("SELECT [Prodid],[ProdName],[TechSpeci],[ModelNo],SerialNo,[IsStatus],[CreateBy],[CreateDate],[updateBy],[updateDate]FROM [tblProduct] where isdeleted='0'", con);
            sad.Fill(dt);
            con.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string createdby = Session["adminname"].ToString();
        try
        {
            if (btnSubmit.Text == "Submit")
            {
                DateTime Date = DateTime.Now;
                con.Open();
                string selectedEngineers = hiddenSelectedEngineers.Value; // New add because dropdown

                SqlCommand cmd = new SqlCommand("SP_SiteVisit", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Custname", txtsitename.Text);
                cmd.Parameters.AddWithValue("@location", txtlocation.Text);
                cmd.Parameters.AddWithValue("@Servicetype", ddlservicetype.SelectedValue);
                //cmd.Parameters.AddWithValue("@Engineername", txtengineername.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@EngineerName", selectedEngineers);

                cmd.Parameters.AddWithValue("@Product", txtproduct.Text);
                cmd.Parameters.AddWithValue("@Date", txtvisitdate.Text);
                cmd.Parameters.AddWithValue("@CreateBy", createdby);
                cmd.Parameters.AddWithValue("@CreateDate", Date);
                cmd.Parameters.AddWithValue("@updateBy", null);
                cmd.Parameters.AddWithValue("@updateDate", Date);
                cmd.Parameters.AddWithValue("@isdeleted", '0');
                cmd.Parameters.AddWithValue("@Action", "Insert");
                cmd.ExecuteNonQuery();
                con.Close();
                if (Request.QueryString["Name"] != null)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Saved Successfully','0');", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Saved Successfully','1');", true);
                }

            }
            else if (btnSubmit.Text == "Update")
            {
                DateTime Date = DateTime.Now;
                con.Open();
                SqlCommand cmd = new SqlCommand("SP_SiteVisit", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Custname", txtsitename.Text);
                cmd.Parameters.AddWithValue("@location", txtlocation.Text);
                cmd.Parameters.AddWithValue("@Servicetype", ddlservicetype.SelectedValue);
                //cmd.Parameters.AddWithValue("@Engineername", txtengineername.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@EngineerName", hiddenSelectedEngineers.Value);

                cmd.Parameters.AddWithValue("@Product", txtproduct.Text);
                cmd.Parameters.AddWithValue("@Date", txtvisitdate.Text);
                cmd.Parameters.AddWithValue("@CreateBy", createdby);
                cmd.Parameters.AddWithValue("@CreateDate", Date);
                cmd.Parameters.AddWithValue("@updateBy", null);
                cmd.Parameters.AddWithValue("@updateDate", Date);
                cmd.Parameters.AddWithValue("@isdeleted", '0');
                cmd.Parameters.AddWithValue("@ID", Convert.ToInt32(hidden.Value));
                cmd.Parameters.AddWithValue("@Action", "Update");
                cmd.ExecuteNonQuery();
                con.Close();
                if (Request.QueryString["Name"] != null)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Updated Successfully','0');", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Updated Successfully','1');", true);
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
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

    protected void loadData(string id)
    {
        try
        {
            SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM [tbl_sitevisit] where ID='" + id + "' ", con);

            DataTable dt = new DataTable();
            sad.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                txtsitename.Text = dt.Rows[0]["Custname"].ToString();
                txtlocation.Text = dt.Rows[0]["location"].ToString();
                ddlservicetype.SelectedValue = dt.Rows[0]["Servicetype"].ToString();
                //txtengineername.SelectedItem.Text = dt.Rows[0]["Engineername"].ToString();
                hiddenSelectedEngineers.Value = dt.Rows[0]["Engineername"].ToString();
                txtproduct.Text = dt.Rows[0]["Product"].ToString();
                DateTime ffff1 = Convert.ToDateTime(dt.Rows[0]["Date"].ToString());
                txtvisitdate.Text = ffff1.ToString("yyyy-MM-dd");
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

    protected void gv_Prod_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "RowEdit")
        {
            Response.Redirect("Product.aspx?Prodid=" + encrypt(e.CommandArgument.ToString()) + "");
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
            GridView();
        }
    }

    protected void gv_Prod_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblisstatus = (Label)e.Row.FindControl("lblIsStatus") as Label;

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
    public static List<string> GetProdList(string prefixText, int count)
    {
        return AutoFillProdlist(prefixText);
    }

    public static List<string> AutoFillProdlist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT ProdName from tblProduct where " + "ProdName like @Search + '%' AND isdeleted='0'";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> ProdName = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        ProdName.Add(sdr["ProdName"].ToString());
                    }
                }
                con.Close();
                return ProdName;
            }
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("SiteVisitList.aspx");
    }

    protected void lnkrefresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("Sitevisitnew.aspx");
    }
}