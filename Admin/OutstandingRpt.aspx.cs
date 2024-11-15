using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_OutstandingRpt : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
    private object memoryStream;
    private string query;
    private SqlCommand com;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DivRoot1.Visible = true;
            btn.Visible = true;
            bindOutstandingData();
        }
    }

    protected void btnresetfilter_Click(object sender, EventArgs e)
    {
        Response.Redirect("OutstandingRpt.aspx");
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

                com.CommandText = "select DISTINCT CustName from tblInvoiceHdr where " + "CustName like @Search + '%'  ";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> CustName = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        CustName.Add(sdr["CustName"].ToString());
                    }
                }
                con.Close();
                return CustName;
            }

        }
    }
    //int count = 0;
    //protected void ddltype_TextChanged(object sender, EventArgs e)
    //{
    //    if (ddltype.Text == "Sales")
    //    {
    //        AutoCompleteExtender1.Enabled = true;
    //        txtPartyName.Text = string.Empty;
    //        GetCustomerList(txtPartyName.Text, count);
    //        //AutoCompleteExtender3.Enabled = false;

    //    }

    //    else if (ddltype.Text == "Purchase")
    //    {
    //        //AutoCompleteExtender3.Enabled = true;
    //        txtPartyName.Text = string.Empty;
    //        GetSupplierList(txtPartyName.Text, count);
    //        AutoCompleteExtender1.Enabled = false;

    //    }
    //}
    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetSupplierList(string prefixText, int count)
    {

        return AutoFillSupplierlist(prefixText);
    }

    public static List<string> AutoFillSupplierlist(string prefixText)
    {

        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {

                com.CommandText = "select DISTINCT CustName from tblInvoiceHdr where " + "CustName like @Search + '%'  ";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> CustName = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        CustName.Add(sdr["CustName"].ToString());
                    }
                }
                con.Close();
                return CustName;
            }

        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        //required to avoid the runtime error "  
        //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
    }

    protected void bindOutstandingData()
    {
        dgvOutstanding.DataSource = null;//GetData("SP_OutstandingR");
        dgvOutstanding.DataBind();
    }

    private static DataTable GetData(string SP)
    {
        string strConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(strConnString))
        {
            using (SqlCommand cmd = new SqlCommand(SP, con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", "SALE");
                cmd.Parameters.AddWithValue("@PartyName", "Weblink Services");
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataSet ds = new DataSet())
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        return dt;
                    }
                }
            }
        }
    }

    decimal Balance = 0;
    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string invoiceno = e.Row.Cells[1].Text;
            string payble = e.Row.Cells[4].Text;
            string Recevd = e.Row.Cells[5].Text;
            Label lblbalance = (Label)e.Row.FindControl("lblbalance");
            Label lblCum_Balance = (Label)e.Row.FindControl("lblCum_Balance");
        }

    }

    protected void btnpdf_Click(object sender, EventArgs e)
    {

        Pdf();
    }

    protected void btnsearch_Click(object sender, EventArgs e)
    {
        try
        {


        }
        catch (Exception)
        {

            throw;
        }

    }

    private void Pdf()
    {
        DataTable Dt = new DataTable();
        SqlDataAdapter Da;
        if (txtPartyName.Text == "")
        {
            Da = new SqlDataAdapter("select distinct(CustName),CustomerShippingAddress from tblInvoiceHdr", con);
        }
        else
        {
            Da = new SqlDataAdapter("select distinct(CustName),CustomerShippingAddress from tblInvoiceHdr where CustName='" + txtPartyName.Text + "'", con);
        }

        Da.Fill(Dt);

        StringWriter sw = new StringWriter();
        StringReader sr = new StringReader(sw.ToString());

        Document doc = new Document(PageSize.A4, 10f, 10f, 55f, 0f);
        string Customernameqqq = Dt.Rows[0]["CustName"].ToString();
        //string ShiftAddress = Dt.Rows[0]["ShiftAddress"].ToString();
        PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(Server.MapPath("~/Files/") + "OutstandingReport.pdf", FileMode.Create));
        //PdfWriter writer = PdfWriter.GetInstance(doc, Response.OutputStream);
        //XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, sr);
        doc.Open();


        string imageURL = Server.MapPath("~") + "/image/AA.png";

        iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(imageURL);
        //Resize image depend upon your need
        png.ScaleToFit(70, 100);

        //For Image Position
        png.SetAbsolutePosition(45, 770);
        //var document = new Document();

        //Give space before image
        //png.ScaleToFit(document.PageSize.Width - (document.RightMargin * 100), 50);
        png.SpacingBefore = 40f;

        //Give some space after the image

        png.SpacingAfter = 1f;

        png.Alignment = Element.ALIGN_RIGHT;

        //paragraphimage.Add(png);
        //doc.Add(paragraphimage);
        doc.Add(png);

        if (Dt.Rows.Count > 0)
        {
            string BillingCustomer = Dt.Rows[0]["CustName"].ToString();
            string ShippingAddress = Dt.Rows[0]["CustomerShippingAddress"].ToString();
           // string Paid = Dt.Rows[0]["ContactNo"].ToString();
        }

        PdfContentByte cbb = writer.DirectContent;
        cbb.Rectangle(17f, 725f, 560f, 25f);
        cbb.Stroke();

        // Header 
        cbb.BeginText();
        cbb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 14);
        cbb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Outstanding Report", 270, 735, 0);
        cbb.EndText();


        PdfContentByte cb = writer.DirectContent;
        cb.Rectangle(17f, 750f, 560f, 80f);
        cb.Stroke();

        // Header 
        cb.BeginText();

        cb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 22);
        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "ENDEAVOUR AUTOMATION", 220, 795, 0);
        cb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 11);
        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Survey No. 27, Nilambkar Nagar, Near Raghunandan Karyalay,", 190, 775, 0);
        cb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 11);
        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Tathawade, Pune-411033", 270, 760, 0);
        cb.EndText();

        Paragraph paragraphTable1 = new Paragraph();
        paragraphTable1.SpacingBefore = 50f;
        paragraphTable1.SpacingAfter = 0f;
        PdfPTable table = new PdfPTable(1);

        float[] widths2 = new float[] { 100 };
        table.SetWidths(widths2);
        table.TotalWidth = 560f;
        table.LockedWidth = true;

        table = new PdfPTable(1);
        float[] widths3 = new float[] { 100f };
        table.SetWidths(widths2);
        table.TotalWidth = 560f;
        table.LockedWidth = true;
        PdfPCell cell = null;

        string to = txttodate.Text.ToString();
        string from = txtfromdate.Text.ToString();

        cell = new PdfPCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
        cell.HorizontalAlignment = 1;
        table.AddCell(cell);

        cell = new PdfPCell(new Phrase(Customernameqqq, FontFactory.GetFont("Arial", 13, Font.BOLD)));
        cell.Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER;
        cell.HorizontalAlignment = 1;
        table.AddCell(cell);

        cell = new PdfPCell(new Phrase(from + "  To  " + to, FontFactory.GetFont("Arial", 10, Font.NORMAL)));
        cell.Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER;
        cell.HorizontalAlignment = 1;
        table.AddCell(cell);

        paragraphTable1.Add(table);
        doc.Add(paragraphTable1);

        Paragraph paragraphTable2 = new Paragraph();
        paragraphTable2.SpacingAfter = 0f;
        paragraphTable2.SpacingBefore = 0f;
        table = new PdfPTable(6);
        float[] widths33 = new float[] { 10f, 10f, 10f, 10f, 10f, 10f };
        PdfPCell celll = null;

        table.SetWidths(widths33);
        if (Dt.Rows.Count > 0)
        {
            DataTable Dtt = new DataTable();
            DataTable DttOpening = new DataTable();
            string fdate;
            string tdate;
            string ft = txtfromdate.Text;
            string tt = txttodate.Text;
            if (ft == "")
            {
                fdate = "";
            }
            else
            {
                var fttime = ft;
                fdate = fttime.ToString();
            }

            if (tt == "")
            {
                tdate = "";
            }
            else
            {
                var tttime = tt;
                tdate = tttime.ToString();
            }

           // Dtt = GetData("SP_OutstandingRpt", Dt.Rows[0]["CustName"].ToString(), fdate, tdate);

            if (Dtt.Rows.Count > 0)
            {
                //double Ttotal_price = 0;
                if (Dt.Rows.Count > 0)
                {
                    table.TotalWidth = 560f;
                    table.LockedWidth = true;

                    celll = new PdfPCell(new Phrase("Invoice No", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                    celll.HorizontalAlignment = 1;
                    table.AddCell(celll);

                    celll = new PdfPCell(new Phrase("Invoice Date", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                    celll.HorizontalAlignment = 1;
                    table.AddCell(celll);

                    celll = new PdfPCell(new Phrase("Payable Amount", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                    celll.HorizontalAlignment = 1;
                    table.AddCell(celll);

                    celll = new PdfPCell(new Phrase("Recived Amount", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                    celll.HorizontalAlignment = 1;
                    table.AddCell(celll);

                    celll = new PdfPCell(new Phrase("Pending Amount ", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                    celll.HorizontalAlignment = 1;
                    table.AddCell(celll);

                    celll = new PdfPCell(new Phrase("Days", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                    celll.HorizontalAlignment = 1;
                    table.AddCell(celll);

                    int rowid = 1;
                    decimal Balance = 0;
                    decimal SumOfDebit = 0;
                    decimal SumOfCredit = 0;
                    decimal SumOfbal = 0;
                    string OpeningBalance = "";
                    if (OpeningBalance == "")
                    {
                        Balance = 0;
                    }
                    else
                    {
                        Balance = Convert.ToDecimal(OpeningBalance);
                    }

                    foreach (DataRow dr in Dtt.Rows)
                    {
                        table.TotalWidth = 560f;
                        table.LockedWidth = true;
                        celll = new PdfPCell(new Phrase(dr["INVOICENO"].ToString(), FontFactory.GetFont("Arial", 9)));
                        celll.HorizontalAlignment = 1;
                        table.AddCell(celll);

                        celll = new PdfPCell(new Phrase(dr["INVOICEDATE"].ToString().TrimEnd("0:0".ToCharArray()), FontFactory.GetFont("Arial", 9)));
                        celll.HorizontalAlignment = 1;
                        table.AddCell(celll);

                        celll = new PdfPCell(new Phrase(dr["PayableAmt"].ToString(), FontFactory.GetFont("Arial", 9)));
                        celll.HorizontalAlignment = 1;
                        table.AddCell(celll);

                        celll = new PdfPCell(new Phrase(dr["RecivedAmt"].ToString(), FontFactory.GetFont("Arial", 9)));
                        celll.HorizontalAlignment = 1;
                        table.AddCell(celll);

                        celll = new PdfPCell(new Phrase(dr["PendingAmt"].ToString(), FontFactory.GetFont("Arial", 9)));
                        celll.HorizontalAlignment = 1;
                        table.AddCell(celll);

                        celll = new PdfPCell(new Phrase(dr["Days"].ToString(), FontFactory.GetFont("Arial", 9)));
                        celll.HorizontalAlignment = 1;
                        table.AddCell(celll);
                        rowid++;
                        SumOfbal = Balance;
                    }
                }
                paragraphTable2.Add(table);
                doc.Add(paragraphTable2);

                //Space
                Paragraph paragraphTable3 = new Paragraph();

                string[] items = { "Goods once sold will not be taken back or exchange. \b",
                        "Interest at the rate of 18% will be charged if bill is'nt paid within 30 days.\b",
                        "Our risk and responsibility ceases the moment goods leaves out godown. \n",
                        };

                Font font12 = FontFactory.GetFont("Arial", 12, Font.BOLD);
                Font font10 = FontFactory.GetFont("Arial", 10, Font.BOLD);
                Paragraph paragraph = new Paragraph("", font12);

                for (int i = 0; i < items.Length; i++)
                {
                    paragraph.Add(new Phrase("", font10));
                }

                for (int i = 0; i < 1; i++)
                {
                    table = new PdfPTable(6);
                    table.TotalWidth = 560f;
                    table.LockedWidth = true;
                    table.SetWidths(new float[] { 10f, 10f, 10f, 10f, 10f, 10f });
                    table.AddCell(paragraph);
                    table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                    table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                    table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                    table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                    table.AddCell(new Phrase("\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                    doc.Add(table);
                }
                paragraphTable3.Add(table);
                doc.Add(paragraphTable3);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Record Not Found !!!');", true);
            }
        }
        table = new PdfPTable(2);
        table.TotalWidth = 560f;

        float[] widths = new float[] { 160f, 400f };
        table.SetWidths(widths);
        table.LockedWidth = true;
        doc.Close();

        //Byte[] FileBuffer = File.ReadAllBytes(Server.MapPath("~/Files/") + "TaxInvoice.pdf");
        //string empFilename = "" + CustomerName + "" + DateTime.Now.ToShortDateString() + ".pdf";

        //if (FileBuffer != null)
        //{
        //    Response.ContentType = "application/pdf";
        //    Response.AddHeader("content-length", FileBuffer.Length.ToString());
        //    Response.BinaryWrite(FileBuffer);
        //    Response.AddHeader("Content-Disposition", "attachment;filename=" + empFilename);
        //}
        //string FilecontentType = "application/pdf";

        doc.Close();
        ifrRight6.Attributes["src"] = @"../Files/OutstandingReport.pdf";
    }

    //protected void Pdf()
    //{
    //    DataTable Dt = new DataTable();
    //    SqlDataAdapter Da;
    //    if (txtPartyName.Text == "")
    //    {
    //        Da = new SqlDataAdapter("SELECT * FROM VW_Outstanding", con);
    //    }
    //    else
    //    {
    //        Da = new SqlDataAdapter("SELECT * FROM VW_Outstanding WHERE CustName='" + txtPartyName.Text + "'", con);
    //    }

    //    Da.Fill(Dt);


    //    //DataTable Dt = new DataTable();
    //    //SqlDataAdapter Da = new SqlDataAdapter("SELECT * FROM VW_Outstanding WHERE CustName='" + txtcustomername.Text + "'", con);
    //    //Da.Fill(Dt);

    //    StringWriter sw = new StringWriter();
    //    StringReader sr = new StringReader(sw.ToString());

    //    Document doc = new Document(PageSize.A4, 10f, 10f, 55f, 0f);
    //    string Customernameqqq = Dt.Rows[0]["CustName"].ToString();
    //    //string ShiftAddress = Dt.Rows[0]["ShiftAddress"].ToString();
    //    PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(Server.MapPath("~/Files/") + "OutstandingReport.pdf", FileMode.Create));
    //    //PdfWriter writer = PdfWriter.GetInstance(doc, Response.OutputStream);
    //    //XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, sr);

    //    doc.Open();

    //    string imageURL = Server.MapPath("~") + "/image/AA.png";

    //    iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(imageURL);

    //    //Resize image depend upon your need

    //    png.ScaleToFit(70, 100);

    //    //For Image Position
    //    png.SetAbsolutePosition(45, 770);
    //    //var document = new Document();

    //    //Give space before image
    //    //png.ScaleToFit(document.PageSize.Width - (document.RightMargin * 100), 50);
    //    png.SpacingBefore = 40f;

    //    //Give some space after the image

    //    png.SpacingAfter = 1f;

    //    png.Alignment = Element.ALIGN_RIGHT;

    //    //paragraphimage.Add(png);
    //    //doc.Add(paragraphimage);
    //    doc.Add(png);


    //    PdfContentByte cb = writer.DirectContent;
    //    cb.Rectangle(17f, 750f, 560f, 80f);
    //    cb.Stroke();
    //    // Header 
    //    cb.BeginText();

    //    cb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 22);
    //    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "ENDEAVOUR AUTOMATION", 220, 795, 0);
    //    cb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 11);
    //    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Survey No. 27, Nilambkar Nagar, Near Raghunandan Karyalay,", 190, 775, 0);
    //    cb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 11);
    //    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Tathawade, Pune-411033", 270, 760, 0);
    //    cb.EndText();

    //    PdfContentByte cbb = writer.DirectContent;
    //    cbb.Rectangle(17f, 725f, 560f, 25f);
    //    cbb.Stroke();
    //    // Header 
    //    cbb.BeginText();
    //    cbb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 14);
    //    cbb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Outstanding Report", 270, 735, 0);
    //    cbb.EndText();


    //    Paragraph paragraphTable1 = new Paragraph();
    //    paragraphTable1.SpacingBefore = 50f;
    //    paragraphTable1.SpacingAfter = 0f;
    //    PdfPTable table = new PdfPTable(1);

    //    float[] widths2 = new float[] { 100 };
    //    table.SetWidths(widths2);
    //    table.TotalWidth = 560f;
    //    table.LockedWidth = true;

    //    table = new PdfPTable(1);
    //    float[] widths3 = new float[] { 100f };
    //    table.SetWidths(widths2);
    //    table.TotalWidth = 560f;
    //    table.LockedWidth = true;
    //    PdfPCell cell = null;

    //    string to = txttodate.Text.ToString();
    //    string from = txtfromdate.Text.ToString();

    //    cell = new PdfPCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //    cell.HorizontalAlignment = 1;
    //    table.AddCell(cell);

    //    cell = new PdfPCell(new Phrase(Customernameqqq, FontFactory.GetFont("Arial", 13, Font.BOLD)));
    //    cell.Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER;
    //    cell.HorizontalAlignment = 1;
    //    table.AddCell(cell);

    //    cell = new PdfPCell(new Phrase(from + "  To  " + to, FontFactory.GetFont("Arial", 10, Font.NORMAL)));
    //    cell.Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER;
    //    cell.HorizontalAlignment = 1;
    //    table.AddCell(cell);

    //    paragraphTable1.Add(table);
    //    doc.Add(paragraphTable1);

    //    Paragraph paragraphTable2 = new Paragraph();
    //    paragraphTable2.SpacingAfter = 0f;
    //    paragraphTable2.SpacingBefore = 0f;
    //    table = new PdfPTable(6);
    //    float[] widths33 = new float[] { 10f, 10f, 10f, 10f, 10f, 10f };
    //    PdfPCell celll = null;

    //    table.SetWidths(widths33);
    //    if (Dt.Rows.Count > 0)
    //    {
    //        DataTable Dtt = new DataTable();
    //        DataTable DttOpening = new DataTable();
    //        string fdate;
    //        string tdate;
    //        string ft = txtfromdate.Text;
    //        string tt = txttodate.Text;
    //        if (ft == "")
    //        {
    //            fdate = "";
    //        }
    //        else
    //        {
    //            var fttime = ft;
    //            fdate = fttime.ToString();
    //        }

    //        if (tt == "")
    //        {
    //            tdate = "";
    //        }
    //        else
    //        {
    //            var tttime = tt;
    //            tdate = tttime.ToString();
    //        }


    //        Dtt = GetData("SP_OutstandingRpt", Dt.Rows[0]["CustName"].ToString(), fdate, tdate, ddltype.Text);

    //        //Dtt = GetData("Sp_OutstandingRpt", Dt.Rows[0]["CustName"].ToString(), fdate, tdate);

    //        if (Dtt.Rows.Count > 0)
    //        {
    //            //double Ttotal_price = 0;
    //            if (Dt.Rows.Count > 0)
    //            {
    //                table.TotalWidth = 560f;
    //                table.LockedWidth = true;

    //                celll = new PdfPCell(new Phrase("Invoice No", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //                celll.HorizontalAlignment = 1;
    //                table.AddCell(celll);

    //                celll = new PdfPCell(new Phrase("Invoice Date", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //                celll.HorizontalAlignment = 1;
    //                table.AddCell(celll);

    //                celll = new PdfPCell(new Phrase("Payable Amount", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //                celll.HorizontalAlignment = 1;
    //                table.AddCell(celll);

    //                celll = new PdfPCell(new Phrase("Recived Amount", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //                celll.HorizontalAlignment = 1;
    //                table.AddCell(celll);

    //                celll = new PdfPCell(new Phrase("Pending Amount ", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //                celll.HorizontalAlignment = 1;
    //                table.AddCell(celll);

    //                celll = new PdfPCell(new Phrase("Days", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //                celll.HorizontalAlignment = 1;
    //                table.AddCell(celll);

    //                int rowid = 1;
    //                decimal Balance = 0;
    //                decimal SumOfDebit = 0;
    //                decimal SumOfCredit = 0;
    //                decimal SumOfbal = 0;
    //                string OpeningBalance = "";
    //                if (OpeningBalance == "")
    //                {
    //                    Balance = 0;
    //                }
    //                else
    //                {
    //                    Balance = Convert.ToDecimal(OpeningBalance);
    //                }

    //                foreach (DataRow dr in Dtt.Rows)
    //                {
    //                    table.TotalWidth = 560f;
    //                    table.LockedWidth = true;
    //                    celll = new PdfPCell(new Phrase(dr["INVOICENO"].ToString(), FontFactory.GetFont("Arial", 9)));
    //                    celll.HorizontalAlignment = 1;
    //                    table.AddCell(celll);

    //                    celll = new PdfPCell(new Phrase(dr["INVOICEDATE"].ToString().TrimEnd("0:0".ToCharArray()), FontFactory.GetFont("Arial", 9)));
    //                    celll.HorizontalAlignment = 1;
    //                    table.AddCell(celll);

    //                    celll = new PdfPCell(new Phrase(dr["PayableAmt"].ToString(), FontFactory.GetFont("Arial", 9)));
    //                    celll.HorizontalAlignment = 1;
    //                    table.AddCell(celll);

    //                    celll = new PdfPCell(new Phrase(dr["RecivedAmt"].ToString(), FontFactory.GetFont("Arial", 9)));
    //                    celll.HorizontalAlignment = 1;
    //                    table.AddCell(celll);

    //                    celll = new PdfPCell(new Phrase(dr["PendingAmt"].ToString(), FontFactory.GetFont("Arial", 9)));
    //                    celll.HorizontalAlignment = 1;
    //                    table.AddCell(celll);

    //                    celll = new PdfPCell(new Phrase(dr["Days"].ToString(), FontFactory.GetFont("Arial", 9)));
    //                    celll.HorizontalAlignment = 1;
    //                    table.AddCell(celll);
    //                    rowid++;
    //                    SumOfbal = Balance;
    //                }
    //            }
    //            paragraphTable2.Add(table);
    //            doc.Add(paragraphTable2);

    //            //Space
    //            Paragraph paragraphTable3 = new Paragraph();

    //            string[] items = { "Goods once sold will not be taken back or exchange. \b",
    //                    "Interest at the rate of 18% will be charged if bill is'nt paid within 30 days.\b",
    //                    "Our risk and responsibility ceases the moment goods leaves out godown. \n",
    //                    };

    //            Font font12 = FontFactory.GetFont("Arial", 12, Font.BOLD);
    //            Font font10 = FontFactory.GetFont("Arial", 10, Font.BOLD);
    //            Paragraph paragraph = new Paragraph("", font12);

    //            for (int i = 0; i < items.Length; i++)
    //            {
    //                paragraph.Add(new Phrase("", font10));
    //            }

    //            for (int i = 0; i < 1; i++)
    //            {
    //                table = new PdfPTable(6);
    //                table.TotalWidth = 560f;
    //                table.LockedWidth = true;
    //                table.SetWidths(new float[] { 10f, 10f, 10f, 10f, 10f, 10f });
    //                table.AddCell(paragraph);
    //                table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //                table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //                table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //                table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //                table.AddCell(new Phrase("\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //                doc.Add(table);
    //            }
    //            paragraphTable3.Add(table);
    //            doc.Add(paragraphTable3);
    //        }
    //        else
    //        {
    //            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Record Not Found !!!');", true);
    //        }
    //    }
    //    table = new PdfPTable(2);
    //    table.TotalWidth = 560f;

    //    float[] widths = new float[] { 160f, 400f };
    //    table.SetWidths(widths);
    //    table.LockedWidth = true;
    //    doc.Close();

    //    //Byte[] FileBuffer = File.ReadAllBytes(Server.MapPath("~/Files/") + "TaxInvoice.pdf");
    //    //string empFilename = "" + CustomerName + "" + DateTime.Now.ToShortDateString() + ".pdf";

    //    //if (FileBuffer != null)
    //    //{
    //    //    Response.ContentType = "application/pdf";
    //    //    Response.AddHeader("content-length", FileBuffer.Length.ToString());
    //    //    Response.BinaryWrite(FileBuffer);
    //    //    Response.AddHeader("Content-Disposition", "attachment;filename=" + empFilename);
    //    //}
    //    //string FilecontentType = "application/pdf";

    //    doc.Close();
    //    ifrRight6.Attributes["src"] = @"../Files/OutstandingReport.pdf";
    //}


    private static DataTable GetData(string SP, string PartyName, string FromDate, string ToDate, string Type)
    {
        string strConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(strConnString))
        {
            using (SqlCommand cmd = new SqlCommand(SP, con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", Type);
                cmd.Parameters.AddWithValue("@PartyName", PartyName);
                if (FromDate == "")
                    cmd.Parameters.AddWithValue("@FromDate", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@FromDate", FromDate);
                if (ToDate == "")
                    cmd.Parameters.AddWithValue("@ToDate", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@ToDate", ToDate);
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataSet ds = new DataSet())
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        return dt;
                    }
                }
            }
        }
    }
}