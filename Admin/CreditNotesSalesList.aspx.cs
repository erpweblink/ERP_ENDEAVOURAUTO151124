using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using SelectPdf;
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

public partial class Admin_CreditNotesSalesList : System.Web.UI.Page
{
    SqlConnection Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            gridviewdata();
        }
    }
    protected void gridviewdata()
    {
        try
        {
            DataTable dtt = new DataTable();
            SqlDataAdapter sad1 = new SqlDataAdapter("select * from tblCreditNoteSalesHdrs where isdeleted='0' ORDER BY CreatedDate Desc", Conn);
            sad1.Fill(dtt);
            GvPurchaseOrderList.DataSource = dtt;
            GvPurchaseOrderList.DataBind();
            GvPurchaseOrderList.EmptyDataText = "Record Not Found";

        }
        catch (Exception)
        {

            throw;
        }
    }

    protected void GvPurchaseOrderList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "RowDelete")
        {
            Conn.Open();
            SqlCommand Cmd = new SqlCommand("UPDATE tblCreditNoteSalesHdrs SET isdeleted='1' WHERE Id=@Id", Conn);

            Cmd.Parameters.AddWithValue("Id", Convert.ToInt32(e.CommandArgument.ToString()));
            Cmd.Parameters.AddWithValue("isdeleted", '1');

            Cmd.ExecuteNonQuery();

            ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Record Deleted Sucessfully');", true);

        }
        if (e.CommandName == "RowEdit")
        {
            Response.Redirect("CreditNotesales.aspx?Voc_no=" + encrypt(e.CommandArgument.ToString()) + "");
        }
    }

    protected void GvPurchaseOrderList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvPurchaseOrderList.PageIndex = e.NewPageIndex;
        gridviewdata();
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
        Response.Redirect("CreditNotesales.aspx");
      
    }

    protected void btn_search_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtcustomer.Text) && string.IsNullOrEmpty(txtJobno.Text) && string.IsNullOrEmpty(txtvocno.Text) && string.IsNullOrEmpty(txt_Vocdate.Text))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Please Enter customer name !!!');", true);
            gridviewdata();
        }
        else if(!string.IsNullOrEmpty(txtvocno.Text) && !string.IsNullOrEmpty(txt_Vocdate.Text))
        {
            DataTable dt = new DataTable();
            SqlDataAdapter sad1 = new SqlDataAdapter("select * from tblCreditNoteSalesHdrs where Voc_no='" + txtvocno.Text + "' AND VocDate='" + txt_Vocdate.Text + "' AND isdeleted='0' ", Conn);
            sad1.Fill(dt);
            GvPurchaseOrderList.DataSource = dt;
            GvPurchaseOrderList.DataBind();
            GvPurchaseOrderList.EmptyDataText = "Record Not Found";

        }
        else if (!string.IsNullOrEmpty(txtJobno.Text) && !string.IsNullOrEmpty(txt_Vocdate.Text))
        {
            DataTable dt = new DataTable();
            SqlDataAdapter sad1 = new SqlDataAdapter("select * from tblCreditNoteSalesHdrs where JobNo='" + txtJobno.Text + "' AND VocDate='" + txt_Vocdate.Text + "' AND isdeleted='0' ", Conn);
            sad1.Fill(dt);
            GvPurchaseOrderList.DataSource = dt;
            GvPurchaseOrderList.DataBind();
            GvPurchaseOrderList.EmptyDataText = "Record Not Found";

        }
        else if (!string.IsNullOrEmpty(txtcustomer.Text) && !string.IsNullOrEmpty(txt_Vocdate.Text))
        {
            DataTable dt = new DataTable();
            SqlDataAdapter sad1 = new SqlDataAdapter("select * from tblCreditNoteSalesHdrs where CustomerName='" + txtcustomer.Text + "' AND VocDate='" + txt_Vocdate.Text + "' AND isdeleted='0' ", Conn);
            sad1.Fill(dt);
            GvPurchaseOrderList.DataSource = dt;
            GvPurchaseOrderList.DataBind();
            GvPurchaseOrderList.EmptyDataText = "Record Not Found";

        }
        else if (!string.IsNullOrEmpty(txtcustomer.Text))
        {
            DataTable dt = new DataTable();
            SqlDataAdapter sad1 = new SqlDataAdapter("select * from tblCreditNoteSalesHdrs where CustomerName='" + txtcustomer.Text + "'  AND isdeleted='0' ", Conn);
            sad1.Fill(dt);
            GvPurchaseOrderList.DataSource = dt;
            GvPurchaseOrderList.DataBind();
            GvPurchaseOrderList.EmptyDataText = "Record Not Found";

        }
        else if (!string.IsNullOrEmpty(txtJobno.Text) )
        {
            DataTable dt = new DataTable();
            SqlDataAdapter sad1 = new SqlDataAdapter("select * from tblCreditNoteSalesHdrs where JobNo='" + txtJobno.Text + "' AND isdeleted='0' ", Conn);
            sad1.Fill(dt);
            GvPurchaseOrderList.DataSource = dt;
            GvPurchaseOrderList.DataBind();
            GvPurchaseOrderList.EmptyDataText = "Record Not Found";

        }
        else if (!string.IsNullOrEmpty(txtvocno.Text) )
        {
            DataTable dt = new DataTable();
            SqlDataAdapter sad1 = new SqlDataAdapter("select * from tblCreditNoteSalesHdrs where Voc_no='" + txtvocno.Text + "'  AND isdeleted='0' ", Conn);
            sad1.Fill(dt);
            GvPurchaseOrderList.DataSource = dt;
            GvPurchaseOrderList.DataBind();
            GvPurchaseOrderList.EmptyDataText = "Record Not Found";

        }
        else if ( !string.IsNullOrEmpty(txt_Vocdate.Text))
        {
            DataTable dt = new DataTable();
            SqlDataAdapter sad1 = new SqlDataAdapter("select * from tblCreditNoteSalesHdrs where  VocDate='" + txt_Vocdate.Text + "' AND isdeleted='0' ", Conn);
            sad1.Fill(dt);
            GvPurchaseOrderList.DataSource = dt;
            GvPurchaseOrderList.DataBind();
            GvPurchaseOrderList.EmptyDataText = "Record Not Found";

        }
    }

    protected void btn_refresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("CreditNotesSalesList.aspx");
    }

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetvocList(string prefixText, int count)
    {
        return AutoFillvocelist(prefixText);
    }

    public static List<string> AutoFillvocelist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT Voc_no from tblCreditNoteSalesHdrs where " + "Voc_no like @Search + '%' AND isdeleted='0' ";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> Voc_no = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        Voc_no.Add(sdr["Voc_no"].ToString());
                    }
                }
                con.Close();
                return Voc_no;
            }

        }
    }

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetJobNOList(string prefixText, int count)
    {
        return AutoFilljoblist(prefixText);
    }

    public static List<string> AutoFilljoblist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT JobNo from tblCreditNoteSalesHdrs where " + "JobNo like @Search + '%' AND isdeleted='0' ";

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
    public static List<string> GetcustomerList(string prefixText, int count)
    {
        return AutoFillcustomerlist(prefixText);
    }

    public static List<string> AutoFillcustomerlist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT CustomerName from tblCreditNoteSalesHdrs where " + "CustomerName like @Search + '%' AND isdeleted='0' ";

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
}