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

public partial class Admin_SalesProformaList : System.Web.UI.Page
{
    SqlConnection Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
    string UserCompany = "";
    DataTable dt11 = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["name"] == null)
            {
                Response.Redirect("../LoginPage.aspx");
            }
            else
            {
                UserCompany = Session["name"].ToString();
                if (UserCompany != "Admin")
                {
                    gvbind_Company();
                    btncreate.Visible = false;


                    this.GvPurchaseOrderList.Columns[7].Visible = false;

                }
                else
                {
                    Load_Record();
                    //GvPurchaseOrderList.DataSource = dt11;
                    //GvPurchaseOrderList.DataBind();
                    //this.GvPurchaseOrderList.Columns[7].Visible = true;
                }
                //this.txtDateSearch.Text = DateTime.Now.ToString("yyyy-MM-dd");
                //this.txtDateSearch.TextMode = TextBoxMode.Date;
            }
        }
    }

    void gvbind_Company()
    {
        try
        {
            string UserCompany = Session["Admin"].ToString();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM [tbl_Proforma_Hdr] WHERE Is_Deleted='0' AND CompName='" + UserCompany + "' ", Conn);
            da.Fill(dt);
            GvPurchaseOrderList.EmptyDataText = "Not Records Found";
            GvPurchaseOrderList.DataSource = dt;
            GvPurchaseOrderList.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    private void Load_Record()
    {
        DataTable Dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM [tbl_Proforma_Hdr] WHERE Is_Deleted='0' ORDER BY CreatedOn Desc", Conn);
        da.Fill(Dt);
        GvPurchaseOrderList.EmptyDataText = "Records Not Found";
        GvPurchaseOrderList.DataSource = Dt;
        GvPurchaseOrderList.DataBind();
    }

    protected void btncreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("Proforma.aspx");
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
                com.CommandText = "select DISTINCT JobNo from [tbl_Proforma_Hdr] where " + "JobNo like @Search + '%' AND Is_Deleted='0'";

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
    public static List<string> GetinvoicenoList(string prefixText, int count)
    {
        return AutoFillinvoicelist(prefixText);
    }

    public static List<string> AutoFillinvoicelist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT InvNo from [tbl_Proforma_Hdr] where " + "InvNo like @Search + '%' AND Is_Deleted='0' ";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> InvNo = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        InvNo.Add(sdr["InvNo"].ToString());
                    }
                }
                con.Close();
                return InvNo;
            }

        }
    }
    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetcomapnyList(string prefixText, int count)
    {
        return AutoFillcompanylist(prefixText);
    }

    public static List<string> AutoFillcompanylist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT CompName from [tbl_Proforma_Hdr] where " + "CompName like @Search + '%' AND Is_Deleted='0' ";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> CompName = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        CompName.Add(sdr["CompName"].ToString());
                    }
                }
                con.Close();
                return CompName;
            }

        }
    }

    protected void GvPurchaseOrderList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "RowPrint")
        {
            Response.Write("<script>window.open ('../reportPdf/salesProformapdf.aspx?Id=" + encrypt(e.CommandArgument.ToString()) + "','_blank');</script>");
        }
        if (e.CommandName == "RowEdit")
        {
            Response.Redirect("Proforma.aspx?Id=" + encrypt(e.CommandArgument.ToString()) + "");
        }
        if (e.CommandName == "RowDelete")
        {
            Conn.Open();
            SqlCommand Cmd = new SqlCommand("UPDATE [tbl_Proforma_Hdr] SET Is_Deleted='1' WHERE Id=@Id", Conn);

            Cmd.Parameters.AddWithValue("Id", Convert.ToInt32(e.CommandArgument.ToString()));
            Cmd.Parameters.AddWithValue("Is_Deleted", '1');

            Cmd.ExecuteNonQuery();

            ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Record Deleted Sucessfully');", true);

        }
    }

    protected void GvPurchaseOrderList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvPurchaseOrderList.PageIndex = e.NewPageIndex;
        Load_Record();
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


    protected void btn_search_Click(object sender, EventArgs e)
    {
        try
        {
            if(!string.IsNullOrEmpty(txt_Invoice_search.Text) && !string.IsNullOrEmpty(txtcompany.Text))
            {
                DataTable dt = new DataTable();
                SqlDataAdapter sad = new SqlDataAdapter("select * from [tbl_Proforma_Hdr] where InvNo='"+txt_Invoice_search.Text+ "' AND CompName='"+txtcompany.Text+ "' AND Is_Deleted='0'", Conn);
                sad.Fill(dt);
                GvPurchaseOrderList.DataSource = dt;
                GvPurchaseOrderList.DataBind();
                GvPurchaseOrderList.EmptyDataText = "Record Not Found";
            }
           else if (!string.IsNullOrEmpty(txtJobno.Text) && !string.IsNullOrEmpty(txtcompany.Text))
            {
                DataTable dt = new DataTable();
                SqlDataAdapter sad = new SqlDataAdapter("select * from [tbl_Proforma_Hdr] where JobNo='" + txtJobno.Text + "' AND CompName='" + txtcompany.Text + "'  AND Is_Deleted='0'", Conn);
                sad.Fill(dt);
                GvPurchaseOrderList.DataSource = dt;
                GvPurchaseOrderList.DataBind();
                GvPurchaseOrderList.EmptyDataText = "Record Not Found";
            }
          else  if (!string.IsNullOrEmpty(txt_Invoice_search.Text) && !string.IsNullOrEmpty(txt_to_podate_search.Text))
            {
                DataTable dt = new DataTable();
                SqlDataAdapter sad = new SqlDataAdapter("select * from [tbl_Proforma_Hdr] where InvNo='" + txt_Invoice_search.Text + "' AND invoiceDate='" + txt_Invoice_search.Text + "'  AND Is_Deleted='0'", Conn);
                sad.Fill(dt);
                GvPurchaseOrderList.DataSource = dt;
                GvPurchaseOrderList.DataBind();
                GvPurchaseOrderList.EmptyDataText = "Record Not Found";
            }
            else if (!string.IsNullOrEmpty(txtJobno.Text) && !string.IsNullOrEmpty(txt_to_podate_search.Text))
            {
                DataTable dt = new DataTable();
                SqlDataAdapter sad = new SqlDataAdapter("select * from [tbl_Proforma_Hdr] where JobNo='" + txtJobno.Text + "' AND invoiceDate='" + txt_Invoice_search.Text + "'  AND Is_Deleted='0'", Conn);
                sad.Fill(dt);
                GvPurchaseOrderList.DataSource = dt;
                GvPurchaseOrderList.DataBind();
                GvPurchaseOrderList.EmptyDataText = "Record Not Found";
            }
            else if (!string.IsNullOrEmpty(txtJobno.Text) )
            {
                DataTable dt = new DataTable();
                SqlDataAdapter sad = new SqlDataAdapter("select * from [tbl_Proforma_Hdr] where JobNo='" + txtJobno.Text + "'  AND Is_Deleted='0' ", Conn);
                sad.Fill(dt);
                GvPurchaseOrderList.DataSource = dt;
                GvPurchaseOrderList.DataBind();
                GvPurchaseOrderList.EmptyDataText = "Record Not Found";
            }
            else if (!string.IsNullOrEmpty(txt_Invoice_search.Text))
            {
                DataTable dt = new DataTable();
                SqlDataAdapter sad = new SqlDataAdapter("select * from [tbl_Proforma_Hdr] where InvNo='" + txt_Invoice_search.Text + "'  AND Is_Deleted='0'", Conn);
                sad.Fill(dt);
                GvPurchaseOrderList.DataSource = dt;
                GvPurchaseOrderList.DataBind();
                GvPurchaseOrderList.EmptyDataText = "Record Not Found";
            }
            else if (!string.IsNullOrEmpty(txtcompany.Text))
            {
                DataTable dt = new DataTable();
                SqlDataAdapter sad = new SqlDataAdapter("select * from [tbl_Proforma_Hdr] where CompName='" + txtcompany.Text + "'  AND Is_Deleted='0' ", Conn);
                sad.Fill(dt);
                GvPurchaseOrderList.DataSource = dt;
                GvPurchaseOrderList.DataBind();
                GvPurchaseOrderList.EmptyDataText = "Record Not Found";
            }
            //else if (!string.IsNullOrEmpty(txtJobno.Text))
            else 
            {
                DataTable dt = new DataTable();
                SqlDataAdapter sad = new SqlDataAdapter("select * from [tbl_Proforma_Hdr] where invoiceDate='" + txt_to_podate_search.Text + "'  AND Is_Deleted='0'", Conn);
                sad.Fill(dt);
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
        Response.Redirect("SalesProformaList.aspx");
    } 

}