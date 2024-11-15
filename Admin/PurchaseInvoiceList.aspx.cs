using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_PurchaseInvoiceList : System.Web.UI.Page
{
    SqlConnection Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GridViewData();

        }
    }

    protected void GvPurchaseOrderList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "RowEdit")
        {
            Response.Redirect("PurchaseInvoice.aspx?Id=" + encrypt(e.CommandArgument.ToString()) + "");
        }

        if (e.CommandName == "RowDelete")
        {
            Conn.Open();
            SqlCommand Cmd = new SqlCommand("UPDATE tblPurcahseInvoiceHdr SET isdeleted='1' WHERE Id=@Id", Conn);

            Cmd.Parameters.AddWithValue("Id", Convert.ToInt32(e.CommandArgument.ToString()));
            Cmd.Parameters.AddWithValue("isdeleted", '1');

            Cmd.ExecuteNonQuery();

            ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Record Deleted Sucessfully');", true);

        }
    }

    protected void GvPurchaseOrderList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvPurchaseOrderList.PageIndex = e.NewPageIndex;
        GridViewData();
    }
    protected void GridViewData()
    {
        try
        {
            DataTable dt = new DataTable();
            SqlDataAdapter sad = new SqlDataAdapter("select * from tblPurcahseInvoiceHdr where isdeleted='0' ORDER BY Createddate Desc", Conn);
            sad.Fill(dt);
            GvPurchaseOrderList.DataSource = dt;
            GvPurchaseOrderList.DataBind();
            GvPurchaseOrderList.EmptyDataText = "Record Not Found";
        }
        catch (Exception)
        {

            throw;
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
        Response.Redirect("PurchaseInvoice.aspx");
    }

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetJobNOList(string prefixText, int count)
    {
        return AutoFillJOBNOlist(prefixText);
    }
    public static List<string> AutoFillJOBNOlist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT JobNo from tblPurcahseInvoiceHdr where " + "JobNo like @Search + '%' AND isdeleted='0'";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> JobNo = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        JobNo.Add(sdr["JobNo"].ToString());
                    }
                }
                con.Close();
                return JobNo;
            }

        }
    }

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetvocList(string prefixText, int count)
    {
        return AutoFillvoclist(prefixText);
    }
    public static List<string> AutoFillvoclist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT Voc_No from tblPurcahseInvoiceHdr where " + "Voc_No like @Search + '%' AND isdeleted='0'";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> Voc_No = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        Voc_No.Add(sdr["Voc_No"].ToString());
                    }
                }
                con.Close();
                return Voc_No;
            }

        }
    }

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetvendorList(string prefixText, int count)
    {
        return AutoFillvendorlist(prefixText);
    }
    public static List<string> AutoFillvendorlist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT VendorName from tblPurcahseInvoiceHdr where " + "VendorName like @Search + '%' AND isdeleted='0'";

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

    protected void btn_search_Click(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(txtJobno.Text) && !string.IsNullOrEmpty(txt_Vocdate.Text))
            {
                DataTable dt = new DataTable();
                SqlDataAdapter sad1 = new SqlDataAdapter("select * from tblPurcahseInvoiceHdr where JobNo='"+txtJobno.Text+"' AND VocDate='"+txt_Vocdate.Text+ "' AND isdeleted='0'", Conn);
                sad1.Fill(dt);
                GvPurchaseOrderList.DataSource = dt;
                GvPurchaseOrderList.DataBind();
                GvPurchaseOrderList.EmptyDataText = "Record Not Found";
            }
           else if (!string.IsNullOrEmpty(txtvocno.Text) && !string.IsNullOrEmpty(txt_Vocdate.Text))
            {
                DataTable dt = new DataTable();
                SqlDataAdapter sad1 = new SqlDataAdapter("select * from tblPurcahseInvoiceHdr where Voc_No='" + txtvocno.Text + "' AND VocDate='" + txt_Vocdate.Text + "' AND isdeleted='0'", Conn);
                sad1.Fill(dt);
                GvPurchaseOrderList.DataSource = dt;
                GvPurchaseOrderList.DataBind();
                GvPurchaseOrderList.EmptyDataText = "Record Not Found";
            }
            else if (!string.IsNullOrEmpty(txtvocno.Text) && !string.IsNullOrEmpty(txt_Vocdate.Text))
            {
                DataTable dt = new DataTable();
                SqlDataAdapter sad1 = new SqlDataAdapter("select * from tblPurcahseInvoiceHdr where Voc_No='" + txtvocno.Text + "' AND VocDate='" + txt_Vocdate.Text + "' AND isdeleted='0'", Conn);
                sad1.Fill(dt);
                GvPurchaseOrderList.DataSource = dt;
                GvPurchaseOrderList.DataBind();
                GvPurchaseOrderList.EmptyDataText = "Record Not Found";
            }
            else if (!string.IsNullOrEmpty(txtvocno.Text))
            {
                DataTable dt = new DataTable();
                SqlDataAdapter sad1 = new SqlDataAdapter("select * from tblPurcahseInvoiceHdr where Voc_No='" + txtvocno.Text + "'  AND isdeleted='0'", Conn);
                sad1.Fill(dt);
                GvPurchaseOrderList.DataSource = dt;
                GvPurchaseOrderList.DataBind();
                GvPurchaseOrderList.EmptyDataText = "Record Not Found";
            }
            else if (!string.IsNullOrEmpty(txtJobno.Text))
            {
                DataTable dt = new DataTable();
                SqlDataAdapter sad1 = new SqlDataAdapter("select * from tblPurcahseInvoiceHdr where JobNo='" + txtJobno.Text + "'  AND isdeleted='0'", Conn);
                sad1.Fill(dt);
                GvPurchaseOrderList.DataSource = dt;
                GvPurchaseOrderList.DataBind();
                GvPurchaseOrderList.EmptyDataText = "Record Not Found";
            }
            else if (!string.IsNullOrEmpty(txtvendor.Text))
            {
                DataTable dt = new DataTable();
                SqlDataAdapter sad1 = new SqlDataAdapter("select * from tblPurcahseInvoiceHdr where VendorName='" + txtvendor.Text + "'  AND isdeleted='0'", Conn);
                sad1.Fill(dt);
                GvPurchaseOrderList.DataSource = dt;
                GvPurchaseOrderList.DataBind();
                GvPurchaseOrderList.EmptyDataText = "Record Not Found";
            }
            else if (!string.IsNullOrEmpty(txt_Vocdate.Text))
            {
                DataTable dt = new DataTable();
                SqlDataAdapter sad1 = new SqlDataAdapter("select * from tblPurcahseInvoiceHdr where VocDate='" +txt_Vocdate.Text + "'  AND isdeleted='0'", Conn);
                sad1.Fill(dt);
                GvPurchaseOrderList.DataSource = dt;
                GvPurchaseOrderList.DataBind();
                GvPurchaseOrderList.EmptyDataText = "Record Not Found";
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    protected void btn_refresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("PurchaseInvoiceList.aspx");
    }
}