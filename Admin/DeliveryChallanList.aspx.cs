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
using System.Xml.Linq;

public partial class Admin_DeliveryChallanList : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
    string UserCompany = "";
    DataTable dt11 = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["adminname"] == null)
            {
                Response.Redirect("../LoginPage.aspx");
            }
            else
            {
                Gridrecord();

                //UserCompany = Session["adminname"].ToString();
                //if (UserCompany != "Admin")
                //{
                //    gvbind_Company();
                //    btncreate.Visible = false;
                //}
                //else
                //{
                //    Gridrecord();
                //    //gv_Deliverychallan.DataSource = dt11;
                //    //gv_Deliverychallan.DataBind();
                //}
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
            SqlDataAdapter sad = new SqlDataAdapter("select * from tblChallanHdr where isdeleted='0' AND CustomerName='" + UserCompany + "'", con);
            sad.Fill(dt);
            gv_Deliverychallan.EmptyDataText = "Not Records Found";
            gv_Deliverychallan.DataSource = dt;
            gv_Deliverychallan.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void Gridrecord()
    {
        try
        {
            DataTable dt = new DataTable();
            SqlDataAdapter sad = new SqlDataAdapter("select * from tblChallanHdr where isdeleted='0' ORDER BY Createddate Desc", con);
            sad.Fill(dt);
            gv_Deliverychallan.DataSource = dt;
            gv_Deliverychallan.DataBind();
            gv_Deliverychallan.EmptyDataText = "Record Not Found";
        }
        catch (Exception)
        {

            throw;
        }

    }

    private void GridExport()
    {
        try
        {
            DataTable dt = new DataTable();
            SqlDataAdapter sad = new SqlDataAdapter("select * from tblChallanHdr where isdeleted='0' ORDER BY Createddate Desc", con);
            sad.Fill(dt);
            GridExportExcel.DataSource = dt;
            GridExportExcel.DataBind();
            GridExportExcel.EmptyDataText = "Record Not Found";
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void btncreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("DeliveryChallan.aspx");
    }

    protected void gv_Del_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //Added New for Count by Shubham Patil
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            decimal totalAmount = 0;

            if (Sortedgv.Rows.Count > 0)
            {
                foreach (GridViewRow row in Sortedgv.Rows)
                {

                    Label lblGrandTotal = row.FindControl("lblGrandTotal") as Label;
                    if (lblGrandTotal != null)
                    {
                        if (decimal.TryParse(lblGrandTotal.Text, out decimal rowAmount)) 
                        {
                            totalAmount += rowAmount;
                        }
                    }
                }
            }
            else
            {
                foreach (GridViewRow row in gv_Deliverychallan.Rows)
                {
                    Label lblGrandTotal = row.FindControl("lblGrandTotal") as Label;
                    if (lblGrandTotal != null)
                    {
                        if (decimal.TryParse(lblGrandTotal.Text, out decimal rowAmount))
                        {
                            totalAmount += rowAmount;
                        }
                    }
                }
            }

            Label lblFooterTotalAmt = (Label)e.Row.FindControl("lblFooterTotalAmt");
            if (lblFooterTotalAmt != null)
            {
                lblFooterTotalAmt.Text = "Total Amt: ₹" + totalAmount.ToString("N2");
            }
        }
        //End
    }

    protected void gv_Deliverychallan_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "RowEdit")
        {
            Response.Redirect("DeliveryChallan.aspx?ChallanNo=" + encrypt(e.CommandArgument.ToString()) + "");
        }
        if (e.CommandName == "RowDelete")
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("UPDATE tblChallanHdr SET IsDeleted='1' WHERE Id=@Id ", con);
            cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(e.CommandArgument.ToString()));
            cmd.Parameters.AddWithValue("@isdeleted", '1');
            cmd.ExecuteNonQuery();

            ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Deleted Sucessfully');", true);
        }
        if (e.CommandName == "RowView")
        {
            Response.Write("<script>window.open ('../reportPdf/DeliverychallanPdf.aspx?ChallanNo=" + encrypt(e.CommandArgument.ToString()) + "','_blank');</script>");

            //string id = e.CommandArgument.ToString();

            //Pdf(id);
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

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetJobList(string prefixText, int count)
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
                com.CommandText = "select DISTINCT JobNo from tblChallanHdr where " + "JobNo like @Search + '%' AND isdeleted='0'";

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
    public static List<string> GetchallanList(string prefixText, int count)
    {
        return AutoFillchallanlist(prefixText);
    }

    public static List<string> AutoFillchallanlist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT ChallanNo from tblChallanHdr where " + "ChallanNo like @Search + '%' AND isdeleted='0'";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> ChallanNo = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        ChallanNo.Add(sdr["ChallanNo"].ToString());
                    }
                }
                con.Close();
                return ChallanNo;
            }

        }
    }

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetCustomerList(string prefixText, int count)
    {
        return AutoFillCustomerlist(prefixText);
    }

    public static List<string> AutoFillCustomerlist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT CustomerName from tblChallanHdr where " + "CustomerName like @Search + '%' AND isdeleted='0'";

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

    protected void lnkBtn_rfresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("DeliveryChallanList.aspx");
    }

    protected void btn_search_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txt_Customername_search.Text) && string.IsNullOrEmpty(txtchallanNo.Text) && string.IsNullOrEmpty(txt_formsearch.Text) && string.IsNullOrEmpty(txt_Tosearch.Text))
        {
            Gridrecord();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Pleaseuse Any filter to search Name !!!');", true);

        }

        else if (!string.IsNullOrEmpty(txt_Customername_search.Text) && !string.IsNullOrEmpty(txtchallanNo.Text))
        {
            try
            {
                GetsortedCustomerwithchallanNo();
                //DataTable dt = new DataTable();
                //SqlDataAdapter sad = new SqlDataAdapter("select * from tblChallanHdr where CustomerName='" + txt_Customername_search.Text + "' AND ChallanNo='" + txtchallanNo.Text + "' AND isdeleted='0'", con);
                //sad.Fill(dt);
                //gv_Deliverychallan.DataSource = dt;
                //gv_Deliverychallan.DataBind();
                //gv_Deliverychallan.EmptyDataText = "Record Not Found";
            }
            catch (Exception)
            {

                throw;
            }

        }
        else if (!string.IsNullOrEmpty(txt_formsearch.Text) && !string.IsNullOrEmpty(txt_Tosearch.Text) && string.IsNullOrEmpty(txtchallanNo.Text) && string.IsNullOrEmpty(txt_Customername_search.Text))
        {
            try
            {
                GetsortedDatewise();
                //DataTable dt = new DataTable();
                //SqlDataAdapter sad = new SqlDataAdapter("select * from tblChallanHdr where ChallanDate BETWEEN '" + txt_formsearch.Text + "' AND '" + txt_Tosearch.Text + "' AND isdeleted='0'", con);
                //sad.Fill(dt);
                //gv_Deliverychallan.DataSource = dt;
                //gv_Deliverychallan.DataBind();
                //gv_Deliverychallan.EmptyDataText = "Record Not Found";
            }
            catch (Exception)
            {

                throw;
            }

        }
        else if (!string.IsNullOrEmpty(txt_Customername_search.Text) && string.IsNullOrEmpty(txt_formsearch.Text))
        {
            try
            {
                GetsortedCustomer();
                //DataTable dt = new DataTable();
                //SqlDataAdapter sad = new SqlDataAdapter("select * from tblChallanHdr where CustomerName='" + txt_Customername_search.Text + "'  AND isdeleted='0'", con);
                //sad.Fill(dt);
                //gv_Deliverychallan.DataSource = dt;
                //gv_Deliverychallan.DataBind();
                //gv_Deliverychallan.EmptyDataText = "Record Not Found";
            }
            catch (Exception)
            {

                throw;
            }

        }
        else if (!string.IsNullOrEmpty(txtchallanNo.Text))
        {
            try
            {
                //Getdatewisechallan();
                GetsortedChallan();
                //DataTable dt = new DataTable();
                //SqlDataAdapter sad = new SqlDataAdapter("select * from tblChallanHdr where  ChallanNo='" + txtchallanNo.Text + "' AND isdeleted='0'", con);
                //sad.Fill(dt);
                //gv_Deliverychallan.DataSource = dt;
                //gv_Deliverychallan.DataBind();
                //gv_Deliverychallan.EmptyDataText = "Record Not Found";
            }
            catch (Exception)
            {

                throw;
            }

        }
        else if (!string.IsNullOrEmpty(txt_formsearch.Text) && string.IsNullOrEmpty(txtchallanNo.Text) && string.IsNullOrEmpty(txt_Tosearch.Text))
        {
            try
            {
                GetsortedDatewisechallan();
                //DataTable dt = new DataTable();
                //SqlDataAdapter sad = new SqlDataAdapter("select * from tblChallanHdr where ChallanDate ='" + txt_formsearch.Text + "'  AND isdeleted='0'", con);
                //sad.Fill(dt);
                //gv_Deliverychallan.DataSource = dt;
                //gv_Deliverychallan.DataBind();
                //gv_Deliverychallan.EmptyDataText = "Record Not Found";
            }
            catch (Exception)
            {

                throw;
            }

        }
        else if (!string.IsNullOrEmpty(txt_Tosearch.Text) && string.IsNullOrEmpty(txt_Tosearch.Text))
        {
            try
            {
                GetsortedTodatewise();
                //DataTable dt = new DataTable();
                //SqlDataAdapter sad = new SqlDataAdapter("select * from tblChallanHdr where ChallanDate ='" + txt_Tosearch.Text + "'  AND isdeleted='0'", con);
                //sad.Fill(dt);
                //gv_Deliverychallan.DataSource = dt;
                //gv_Deliverychallan.DataBind();
                //gv_Deliverychallan.EmptyDataText = "Record Not Found";
            }
            catch (Exception)
            {

                throw;
            }
        }
        else if (!string.IsNullOrEmpty(txtchallanNo.Text) && string.IsNullOrEmpty(txt_formsearch.Text))
        {
            Getdatewisechallan();

        }

        else if (!string.IsNullOrEmpty(txt_Customername_search.Text) && !string.IsNullOrEmpty(txt_formsearch.Text))
        {
            GetDatwisecustoner();

        }

    }




    protected void gv_Deliverychallan_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_Deliverychallan.PageIndex = e.NewPageIndex;
        Gridrecord();
    }


    //sorted gridview started
    protected void Sortedgv_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (ViewState["Record"].ToString() == "Customer&Chaalan")
        {
            Sortedgv.PageIndex = e.NewPageIndex;
            GetsortedCustomerwithchallanNogrid();
        }

        if (ViewState["Record"].ToString() == "Date")
        {
            Sortedgv.PageIndex = e.NewPageIndex;
            GetsortedDatewiseGrid();
        }

        if (ViewState["Record"].ToString() == "Customer")
        {
            Sortedgv.PageIndex = e.NewPageIndex;
            GetsortedCustomergrid();
        }

        if (ViewState["Record"].ToString() == "Challan")
        {
            Sortedgv.PageIndex = e.NewPageIndex;
            GetsortedChallangrid();
        }

        if (ViewState["Record"].ToString() == "DatewiseChallan")
        {
            Sortedgv.PageIndex = e.NewPageIndex;
            GetsortedDatewisechallangrid();
        }

        if (ViewState["Record"].ToString() == "TODate")
        {
            Sortedgv.PageIndex = e.NewPageIndex;
            GetsortedTodatewisegrid();
        }

        if (ViewState["Record"].ToString() == "datewisechallan")
        {
            Sortedgv.PageIndex = e.NewPageIndex;
            Getdatewisechallan();
        }

        if (ViewState["Record"].ToString() == "DatwiseCustomer")
        {
            Sortedgv.PageIndex = e.NewPageIndex;
            GetDatwisecustoner();
        }


    }

    public void GetsortedCustomerwithchallanNo()
    {
        gv_Deliverychallan.Visible = false;
        ViewState["Record"] = "Customer&Chaalan";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblChallanHdr where CustomerName='" + txt_Customername_search.Text + "' AND ChallanNo='" + txtchallanNo.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        Sortedgv.DataSource = dt;
        Sortedgv.DataBind();
        Sortedgv.EmptyDataText = "Record Not Found";
    }

    public void GetsortedCustomerwithchallanNogrid()
    {
        gv_Deliverychallan.Visible = false;
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblChallanHdr where CustomerName='" + txt_Customername_search.Text + "' AND ChallanNo='" + txtchallanNo.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        Sortedgv.DataSource = dt;
        Sortedgv.DataBind();
        Sortedgv.EmptyDataText = "Record Not Found";
    }

    public void GetsortedDatewise()
    {
        gv_Deliverychallan.Visible = false;
        ViewState["Record"] = "Date";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblChallanHdr where ChallanDate BETWEEN '" + txt_formsearch.Text + "' AND '" + txt_Tosearch.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        Sortedgv.DataSource = dt;
        Sortedgv.DataBind();
        Sortedgv.EmptyDataText = "Record Not Found";
    }

    public void GetsortedDatewiseGrid()
    {
        gv_Deliverychallan.Visible = false;
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblChallanHdr where ChallanDate BETWEEN '" + txt_formsearch.Text + "' AND '" + txt_Tosearch.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        Sortedgv.DataSource = dt;
        Sortedgv.DataBind();
        Sortedgv.EmptyDataText = "Record Not Found";
    }

    public void GetsortedCustomer()
    {
        gv_Deliverychallan.Visible = false;
        ViewState["Record"] = "Customer";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblChallanHdr where CustomerName='" + txt_Customername_search.Text + "'  AND isdeleted='0'", con);
        sad.Fill(dt);
        Sortedgv.DataSource = dt;
        Sortedgv.DataBind();
        Sortedgv.EmptyDataText = "Record Not Found";
    }

    public void GetsortedCustomergrid()
    {
        gv_Deliverychallan.Visible = false;
        ViewState["Record"] = "Customer";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblChallanHdr where CustomerName='" + txt_Customername_search.Text + "'  AND isdeleted='0'", con);
        sad.Fill(dt);
        Sortedgv.DataSource = dt;
        Sortedgv.DataBind();
        Sortedgv.EmptyDataText = "Record Not Found";
    }

    public void GetsortedChallan()
    {
        gv_Deliverychallan.Visible = false;
        ViewState["Record"] = "Challan";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblChallanHdr where  ChallanNo='" + txtchallanNo.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        Sortedgv.DataSource = dt;
        Sortedgv.DataBind();
        Sortedgv.EmptyDataText = "Record Not Found";
    }
    public void GetsortedChallangrid()
    {
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblChallanHdr where  ChallanNo='" + txtchallanNo.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        Sortedgv.DataSource = dt;
        Sortedgv.DataBind();
        Sortedgv.EmptyDataText = "Record Not Found";
    }

    public void GetsortedDatewisechallan()
    {
        gv_Deliverychallan.Visible = false;
        ViewState["Record"] = "DatewiseChallan";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblChallanHdr where ChallanDate ='" + txt_formsearch.Text + "'  AND isdeleted='0'", con);
        sad.Fill(dt);
        Sortedgv.DataSource = dt;
        Sortedgv.DataBind();
        Sortedgv.EmptyDataText = "Record Not Found";
    }

    public void GetsortedDatewisechallangrid()
    {
        gv_Deliverychallan.Visible = false;
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblChallanHdr where ChallanDate ='" + txt_formsearch.Text + "'  AND isdeleted='0'", con);
        sad.Fill(dt);
        Sortedgv.DataSource = dt;
        Sortedgv.DataBind();
        Sortedgv.EmptyDataText = "Record Not Found";
    }

    public void GetsortedTodatewise()
    {
        gv_Deliverychallan.Visible = false;
        ViewState["Record"] = "TODate";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblChallanHdr where ChallanDate ='" + txt_Tosearch.Text + "'  AND isdeleted='0'", con);
        sad.Fill(dt);
        Sortedgv.DataSource = dt;
        Sortedgv.DataBind();
        Sortedgv.EmptyDataText = "Record Not Found";
    }

    public void GetsortedTodatewisegrid()
    {
        gv_Deliverychallan.Visible = false;
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblChallanHdr where ChallanDate ='" + txt_Tosearch.Text + "'  AND isdeleted='0'", con);
        sad.Fill(dt);
        Sortedgv.DataSource = dt;
        Sortedgv.DataBind();
        Sortedgv.EmptyDataText = "Record Not Found";
    }


    public void Getdatewisechallan()
    {
        gv_Deliverychallan.Visible = false;
        ViewState["Record"] = "datewisechallan";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM tblChallanHdr WHERE ChallanDate = '" + txt_Tosearch.Text + "' AND ChallanNo = '" + txtchallanNo.Text + "' AND isdeleted = '0'", con);

        sad.Fill(dt);
        Sortedgv.DataSource = dt;
        Sortedgv.DataBind();
        Sortedgv.EmptyDataText = "Record Not Found";
    }


    public void Getdatewisechallangrid()
    {
        gv_Deliverychallan.Visible = false;
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM tblChallanHdr WHERE ChallanDate = '" + txt_Tosearch.Text + "' AND ChallanNo = '" + txtchallanNo.Text + "' AND isdeleted = '0'", con);
        sad.Fill(dt);
        Sortedgv.DataSource = dt;
        Sortedgv.DataBind();
        Sortedgv.EmptyDataText = "Record Not Found";
    }

    public void GetDatwisecustoner()
    {

        gv_Deliverychallan.Visible = false;
        ViewState["Record"] = "DatwiseCustomer";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM tblChallanHdr WHERE ChallanDate BETWEEN '" + txt_formsearch.Text + "' AND '" + txt_Tosearch.Text + "' AND CustomerName = '" + txt_Customername_search.Text + "'", con);
        //SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM tblChallanHdr WHERE ChallanDate = '" + txt_Tosearch.Text + "' AND CustomerName = '" + txt_Customername_search.Text + "' AND isdeleted = '0'", con);
        sad.Fill(dt);
        Sortedgv.DataSource = dt;
        Sortedgv.DataBind();
        Sortedgv.EmptyDataText = "Record Not Found";

    }

    public void GetDatwisecustonergrid()
    {
        gv_Deliverychallan.Visible = false;
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM tblChallanHdr WHERE ChallanDate BETWEEN '" + txt_formsearch.Text + "' AND '" + txt_Tosearch.Text + "' AND CustomerName = '" + txt_Customername_search.Text + "'", con);
        sad.Fill(dt);
        Sortedgv.DataSource = dt;
        Sortedgv.DataBind();
        Sortedgv.EmptyDataText = "Record Not Found";

    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        //required to avoid the run time error "  
        //Control 'GridView1' of type 'Grid View' must be placed inside a form tag with runat=server."  
    }

    protected void btnexportexcel_Click(object sender, EventArgs e)
    {
        ExportGridToExcel();
    }

    private void ExportGridToExcel()
    {
        GridExport();

        Response.Clear();
        Response.Buffer = true;
        Response.ClearContent();
        Response.ClearHeaders();
        Response.Charset = "";
        string FileName = "Delivery_Challan_List_" + DateTime.Now + ".xls";
        StringWriter strwritter = new StringWriter();
        HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
        GridExportExcel.GridLines = GridLines.Both;
        GridExportExcel.HeaderStyle.Font.Bold = true;
        GridExportExcel.RenderControl(htmltextwrtter);
        Response.Write(strwritter.ToString());
        Response.End();
    }

    protected void txtstatus_TextChanged(object sender, EventArgs e)
    {

        if (!string.IsNullOrEmpty(txtstatus.SelectedValue))
        {
            try
            {
                gv_Deliverychallan.Visible = false;
                DataTable dt = new DataTable();
                SqlDataAdapter sad = new SqlDataAdapter("select * from tblChallanHdr where status='" + txtstatus.SelectedValue + "'", con);
                sad.Fill(dt);
                Sortedgv.DataSource = dt;
                Sortedgv.DataBind();
                Sortedgv.EmptyDataText = "Record Not Found";
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}