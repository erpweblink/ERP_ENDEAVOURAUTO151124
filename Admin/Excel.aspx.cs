using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Admin_Excel : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
    private object memoryStream;
    private string query;
    private SqlCommand com;

    protected void Page_Load(object sender, EventArgs e)
    {
       // OutstandingGridView();
    }

    void OutstandingGridView()
    {
        try
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM VW_Outstanding WHERE CustName='" + txtcustomername.Text + "'", con);
            sad.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                DataTable Dtt = new DataTable();
                DataTable DttOpening = new DataTable();
                string fdate;
                string tdate;
                string ft = txtFromDt.Text;
                string tt = txttoDt.Text;
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

                Dtt = GetData("SP_OutstandingReport", dt.Rows[0]["CustName"].ToString(), fdate, tdate);
                GVOSTRPT.EmptyDataText = "Not Records Found";
                GVOSTRPT.DataSource = Dtt;
                GVOSTRPT.DataBind();
            }
            con.Close();

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetCustomerlist(string prefixText, int count)
    {
        return AutoFillCust(prefixText);
    }
    public static List<string> AutoFillCust(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select CustName from tblInvoiceHdr" +
                    " where CustName like '%'+ @Search + '%'";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> productsCode = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        productsCode.Add(sdr["CustName"].ToString());
                    }
                }
                con.Close();
                return productsCode;
            }
        }
    }

    public static string ConvertNumbertoWords(int number)
    {
        if (number == 0)
            return "ZERO";
        if (number < 0)
            return "minus " + ConvertNumbertoWords(Math.Abs(number));
        string words = "";
        if ((number / 1000000) > 0)
        {
            words += ConvertNumbertoWords(number / 1000000) + " MILLION ";
            number %= 1000000;
        }
        if ((number / 1000) > 0)
        {
            words += ConvertNumbertoWords(number / 1000) + " THOUSAND ";
            number %= 1000;
        }
        if ((number / 100) > 0)
        {
            words += ConvertNumbertoWords(number / 100) + " HUNDRED ";
            number %= 100;
        }
        if (number > 0)
        {
            if (words != "")
                words += "AND ";
            var unitsMap = new[] { "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN" };
            var tensMap = new[] { "ZERO", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY" };

            if (number < 20)
                words += unitsMap[number];
            else
            {
                words += tensMap[number / 10];
                if ((number % 10) > 0)
                    words += " " + unitsMap[number % 10];
            }
        }
        return words;
    }

    private static DataTable GetData(string SP, string PartyName, string FromDate, string ToDate)
    {
        string strConnString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(strConnString))
        {
            using (SqlCommand cmd = new SqlCommand(SP, con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@Type", Type);
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

    //private void Pdf()
    //{
    //    DataTable Dt = new DataTable();
    //    SqlDataAdapter Da = new SqlDataAdapter("SELECT * FROM VW_Outstanding WHERE CustName='" + txtcustomername.Text + "'", con);
    //    Da.Fill(Dt);

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

    //    string to = txttoDt.Text.ToString();
    //    string from = txtFromDt.Text.ToString();

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
    //        string ft = txtFromDt.Text;
    //        string tt = txttoDt.Text;
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

    //        Dtt = GetData("SP_OutstandingReport", Dt.Rows[0]["CustName"].ToString(), fdate, tdate);

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

    public override void VerifyRenderingInServerForm(Control control)
    {

    }


    private void ExportGridToExcel()
    {
        Response.Clear();
        Response.Buffer = true;
        Response.ClearContent();
        Response.ClearHeaders();
        Response.Charset = "";
        string FileName = "Vithal" + DateTime.Now + ".xls";
        StringWriter strwritter = new StringWriter();
        HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
        GVOSTRPT.GridLines = GridLines.Both;
        GVOSTRPT.HeaderStyle.Font.Bold = true;
        GVOSTRPT.RenderControl(htmltextwrtter);
        Response.Write(strwritter.ToString());
        Response.End();
    }

    private void Export_Excel()
    {
        string Export = "";

        DataTable dt = new DataTable();
        dt.Columns.AddRange(new DataColumn[6]
        {
            new DataColumn("INVOICENO"),
            new DataColumn("INVOICEDATE"),
            new DataColumn("PayableAmt"),
            new DataColumn("RecivedAmt"),
            new DataColumn("PendingAmt"),
            new DataColumn("Days"),

          });
        foreach (GridViewRow row in GVOSTRPT.Rows)
        {

            LinkButton lstText = (LinkButton)row.FindControl("linkcname");

            string Invoiceno = (row.Cells[1].FindControl("lblinvoiceno") as Label).Text;
            string Invoicedate = (row.Cells[1].FindControl("lblinvoicedate") as Label).Text;
            string Paybleamount = (row.Cells[1].FindControl("lblpaybleamt") as Label).Text;
            string Recivedamount = (row.Cells[1].FindControl("lblrecivedamt") as Label).Text;
            string Pendingamount = (row.Cells[1].FindControl("lblpendingamt") as Label).Text;
            string Days = (row.Cells[1].FindControl("lbldays") as Label).Text;
            //string GrandTotal = (row.Cells[1].FindControl("lblGrandToatal") as Label).Text;
            //string TotalWeight = (row.Cells[1].FindControl("lbltotalweight") as Label).Text;

            dt.Rows.Add(Invoiceno, Invoicedate, Paybleamount, Recivedamount, Pendingamount, Days);

        }
        //Create a dummy GridView
        GridView GridView1 = new GridView();
        GridView1.AllowPaging = false;
        GridView1.DataSource = dt;
        GridView1.DataBind();
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=OutstandingGridView(" + Export + ").xls");
        Response.Charset = "";
        Response.ContentType = "application/ms-excel";

        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);

        for (int i = 0; i < GridView1.Rows.Count; i++)
        {
            //Apply text style to each Row
            GridView1.Rows[i].Attributes.Add("class", "textmode");
        }

        GridView1.RenderControl(hw);

        //style to format numbers to string
        string style = @"<style> .textmode { mso-number-format:\@; } </style>";
        Response.Write(style);
        Response.Output.Write(sw.ToString());
        Response.Flush();
        Response.End();
    }


    //protected void btnDownloadpdf_Click(object sender, EventArgs e)
    //{
    //    if (string.IsNullOrWhiteSpace(txtcustomername.Text))
    //    {
    //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Kindly Enter The Customer First..!!');", true);
    //    }
    //    else
    //    {
    //        Pdf();
    //    }
    //}


    protected void btnrefresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("OutstandingReport.aspx");
    }


    protected void btnDownloadExcel_Click(object sender, EventArgs e)
    {
        ExportGridToExcel();
    }

    protected void txttoDt_TextChanged(object sender, EventArgs e)
    {
        OutstandingGridView();
    }

    //private void excelexport()
    //{
    //    Response.Clear();
    //    Response.Buffer = true;
    //    Response.ClearContent();
    //    Response.ClearHeaders();
    //    Response.Charset = "";
    //    string FileName = "Vithal" + DateTime.Now + ".xls";
    //    StringWriter strwritter = new StringWriter();
    //    HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
    //    Response.Cache.SetCacheability(HttpCacheability.NoCache);
    //    Response.ContentType = "application/vnd.ms-excel";
    //    Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
    //    GVOSTRPT.GridLines = GridLines.Both;
    //    GVOSTRPT.HeaderStyle.Font.Bold = true;
    //    GVOSTRPT.RenderControl(htmltextwrtter);
    //    Response.Write(strwritter.ToString());
    //    Response.End();
    //}

    protected void Excel_Click(object sender, EventArgs e)
    {
        Export_Excel();
    }
}