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

public partial class Admin_DeliveryChallanReport : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Gridrecord();
        }
    }
    protected void Gridrecord()
    {
        try
        {
            DataTable dt = new DataTable();
            //SqlDataAdapter sad = new SqlDataAdapter("select * from tblChallanHdr where isdeleted='0'", con);
            SqlDataAdapter sad = new SqlDataAdapter("select * from tblChallanHdr where isdeleted='0' ORDER BY PoDate DESC", con);
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

    protected void gv_Del_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //Added New for Count by Shubham Patil
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            decimal totalAmount = 0;

            if (sortedgv.Rows.Count > 0)
            {
                foreach (GridViewRow row in sortedgv.Rows)
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
                ViewState["Record"] = "Gedatwisecustomer";
                DataTable dt = new DataTable();
                SqlDataAdapter sad = new SqlDataAdapter("select * from tblChallanHdr where CustomerName='" + txt_Customername_search.Text + "' AND ChallanNo='" + txtchallanNo.Text + "' AND isdeleted='0'", con);
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
        else if (!string.IsNullOrEmpty(txt_formsearch.Text) && !string.IsNullOrEmpty(txt_Tosearch.Text) && string.IsNullOrEmpty(txt_Customername_search.Text))
        {
            ViewState["Excell"] = "Gedatwisecustomer1";
            Gedatwisecustomer1();

            //try
            //{
            //    ViewState["Record"] = "Gedatwisecustomer1";
            //    DateTime date = Convert.ToDateTime(txt_formsearch.Text.ToString());
            //    txt_Tosearch.Text = date.ToString("yyyy-MM-dd");

            //    DateTime datee = Convert.ToDateTime(txt_Tosearch.Text.ToString());
            //    txt_Tosearch.Text = datee.ToString("yyyy-MM-dd");
            //    DataTable dt = new DataTable();
            //    SqlDataAdapter sad = new SqlDataAdapter("select * from tblChallanHdr where ChallanDate BETWEEN '" + txt_formsearch.Text + "' AND '" + txt_Tosearch.Text + "' AND isdeleted='0'", con);
            //    sad.Fill(dt);
            //    gv_Deliverychallan.DataSource = dt;
            //    gv_Deliverychallan.DataBind();
            //    gv_Deliverychallan.EmptyDataText = "Record Not Found";
            //}
            //catch (Exception ex)
            //{

            //    throw;
            //}

        }
        else if (!string.IsNullOrEmpty(txt_Customername_search.Text))
        {
            ViewState["Excell"] = "Gedatwisecustomer2";
            Gedatwisecustomer2();
            //try
            //{
            //    ViewState["Record"] = "Gedatwisecustomer2";
            //    DataTable dt = new DataTable();
            //    SqlDataAdapter sad = new SqlDataAdapter("select * from tblChallanHdr where CustomerName='" + txt_Customername_search.Text + "'  AND isdeleted='0'", con);
            //    sad.Fill(dt);
            //    gv_Deliverychallan.DataSource = dt;
            //    gv_Deliverychallan.DataBind();
            //    gv_Deliverychallan.EmptyDataText = "Record Not Found";
            //}
            //catch (Exception)
            //{

            //    throw;
            //}

        }
        else if (!string.IsNullOrEmpty(txtchallanNo.Text))
        {
            ViewState["Excell"] = "Gedatwisecustomer3";
            Gedatwisecustomer3();

            //try
            //{
            //    ViewState["Record"] = "Gedatwisecustomer3";
            //    DataTable dt = new DataTable();
            //    SqlDataAdapter sad = new SqlDataAdapter("select * from tblChallanHdr where  ChallanNo='" + txtchallanNo.Text + "' AND isdeleted='0'", con);
            //    sad.Fill(dt);
            //    gv_Deliverychallan.DataSource = dt;
            //    gv_Deliverychallan.DataBind();
            //    gv_Deliverychallan.EmptyDataText = "Record Not Found";
            //}
            //catch (Exception)
            //{

            //    throw;
            //}

        }
        else if (!string.IsNullOrEmpty(txt_formsearch.Text))
        {
            try
            {
                ViewState["Record"] = "Gedatwisecustomer4";
                DataTable dt = new DataTable();
                SqlDataAdapter sad = new SqlDataAdapter("select * from tblChallanHdr where ChallanDate ='" + txt_formsearch.Text + "'  AND isdeleted='0'", con);
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
        else if (!string.IsNullOrEmpty(txt_Tosearch.Text))
        {
            try
            {
                ViewState["Record"] = "Gedatwisecustomer5";
                DataTable dt = new DataTable();
                SqlDataAdapter sad = new SqlDataAdapter("select * from tblChallanHdr where ChallanDate ='" + txt_Tosearch.Text + "'  AND isdeleted='0'", con);
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
        else if (!string.IsNullOrEmpty(txt_formsearch.Text) && !string.IsNullOrEmpty(txt_Tosearch.Text) && !string.IsNullOrEmpty(txt_Customername_search.Text))
        {
            try
            {
                ViewState["Record"] = "Gedatwisecustomer6";
                DateTime date = Convert.ToDateTime(txt_formsearch.Text.ToString());
                txt_Tosearch.Text = date.ToString("yyyy-MM-dd");

                DateTime datee = Convert.ToDateTime(txt_Tosearch.Text.ToString());
                txt_Tosearch.Text = datee.ToString("yyyy-MM-dd");
                DataTable dt = new DataTable();
                SqlDataAdapter sad = new SqlDataAdapter("select * from tblChallanHdr where ChallanDate BETWEEN '" + txt_formsearch.Text + "' AND '" + txt_Tosearch.Text + "'  AND  CustomerName= '" + txt_Customername_search.Text + "' AND isdeleted='0'", con);
                sad.Fill(dt);
                gv_Deliverychallan.DataSource = dt;
                gv_Deliverychallan.DataBind();
                gv_Deliverychallan.EmptyDataText = "Record Not Found";
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
    //private void Pdf(string id)
    //{
    //    DataTable Dt = new DataTable();
    //    SqlDataAdapter da = new SqlDataAdapter("select * from vw_DeliveryChallan where ChallanNo='" + id + "'", con);
    //    da.Fill(Dt);
    //    gv_Deliverychallan.DataSource = Dt;
    //    gv_Deliverychallan.DataBind();

    //    StringWriter sw = new StringWriter();
    //    StringReader sr = new StringReader(sw.ToString());

    //    Document doc = new Document(PageSize.A4, 10f, 10f, 55f, 0f);

    //    PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(Server.MapPath("~/Files/") + "DeliveryChallan.pdf", FileMode.Create));
    //    PdfWriter writer = PdfWriter.GetInstance(doc, Response.OutputStream);
    //    XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, sr);

    //    doc.Open();

    //    string imageURL = Server.MapPath("~") + "/image/AA.png";

    //    iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(imageURL);

    //    Resize image depend upon your need

    //    png.ScaleToFit(70, 100);

    //    For Image Position
    //    png.SetAbsolutePosition(40, 745);
    //    var document = new Document();

    //    Give space before image
    //    png.ScaleToFit(document.PageSize.Width - (document.RightMargin * 100), 50);
    //    png.SpacingBefore = 50f;

    //    Give some space after the image

    //    png.SpacingAfter = 1f;

    //    png.Alignment = Element.ALIGN_LEFT;

    //    paragraphimage.Add(png);
    //    doc.Add(paragraphimage);
    //    doc.Add(png);


    //    PdfContentByte cb = writer.DirectContent;
    //    cb.Rectangle(17f, 735f, 560f, 60f);
    //    cb.Stroke();
    //    Header
    //    cb.BeginText();
    //    cb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 20);
    //    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "ENDEAVOUR AUTOMATION", 180, 773, 0);
    //    cb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 11);
    //    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Survey No. 27, Nilambkar Nagar, Near Raghunandan Karyalay,", 145, 755, 0);
    //    cb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 11);
    //    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Tathawade, Pune-411033", 227, 740, 0);
    //    cb.EndText();

    //    PdfContentByte cbb = writer.DirectContent;
    //    cbb.Rectangle(17f, 710f, 560f, 25f);
    //    cbb.Stroke();
    //    Header
    //    cbb.BeginText();
    //    cbb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
    //    cbb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " Mob: 9860502108    Email: endeavour.automations@gmail.com ", 153, 722, 0);
    //    cbb.EndText();

    //    PdfContentByte cbbb = writer.DirectContent;
    //    cbbb.Rectangle(17f, 685f, 560f, 25f);
    //    cbbb.Stroke();
    //    Header
    //    cbbb.BeginText();
    //    cbbb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
    //    cbbb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "GSTIN :  27AFYPJ3488G1ZQ ", 30, 695, 0);
    //    cbbb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
    //    cbbb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "W.E.F. :  01/07/2017", 185, 695, 0);
    //    cbbb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
    //    cbbb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "PAN No. :  AFYPJ3488G", 310, 695, 0);
    //    cbbb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
    //    cbbb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "State Code :  27 Maharashtra", 440, 695, 0);
    //    cbbb.EndText();

    //    PdfContentByte cd = writer.DirectContent;
    //    cd.Rectangle(17f, 660f, 560f, 25f);
    //    cd.Stroke();
    //    Header
    //    cd.BeginText();
    //    cd.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 17);
    //    cd.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Delivery Challan", 260, 667, 0);
    //    cd.EndText();

    //    if (Dt.Rows.Count > 0)
    //    {
    //        var CreateDate = DateTime.Now.ToString("yyyy-MM-dd");
    //        string CustomerName = Dt.Rows[0]["CustomerName"].ToString();
    //        string PoNumber = Dt.Rows[0]["PONo"].ToString();
    //        string Address = Dt.Rows[0]["Address"].ToString();
    //        string PODate = Dt.Rows[0]["PODate"].ToString().TrimEnd("0:0".ToCharArray());
    //        string MobileNo = Dt.Rows[0]["MobileNo"].ToString();
    //        string GSTNo = Dt.Rows[0]["GstNo"].ToString();
    //        string KindAtt = Dt.Rows[0]["KindAtt"].ToString();
    //        string TotalInWord = Dt.Rows[0]["TotalInWord"].ToString();
    //        string GrandTotal = Dt.Rows[0]["GrandTotal"].ToString();
    //        string CGST = Dt.Rows[0]["Cgst"].ToString();
    //        string SGST = Dt.Rows[0]["Sgst"].ToString();
    //        string Total = Dt.Rows[0]["GrandTotal"].ToString();
    //        string challaNo = Dt.Rows[0]["ChallanNo"].ToString();
    //        string challanDate = Dt.Rows[0]["ChallanDate"].ToString().TrimEnd("0:0".ToCharArray()); ;

    //        Paragraph paragraphTable1 = new Paragraph();
    //        paragraphTable1.SpacingBefore = 120f;
    //        paragraphTable1.SpacingAfter = 10f;

    //        PdfPTable table = new PdfPTable(4);

    //        float[] widths2 = new float[] { 100, 180, 100, 180 };
    //        table.SetWidths(widths2);
    //        table.TotalWidth = 560f;
    //        table.LockedWidth = true;

    //        DateTime ffff1 = Convert.ToDateTime(Dt.Rows[0]["PODate"].ToString());
    //        string datee = ffff1.ToString("yyyy-MM-dd");
    //        DateTime ffff2 = Convert.ToDateTime(Dt.Rows[0]["ChallanDate"].ToString());
    //        string Dateeee = ffff1.ToString("yyyy-MM-dd");


    //        table.AddCell(new Phrase("Customer Name : ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
    //        table.AddCell(new Phrase(CustomerName, FontFactory.GetFont("Arial", 9, Font.BOLD)));

    //        table.AddCell(new Phrase("Challan No :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
    //        table.AddCell(new Phrase(challaNo, FontFactory.GetFont("Arial", 9, Font.BOLD)));

    //        table.AddCell(new Phrase("Address:", FontFactory.GetFont("Arial", 9, Font.BOLD)));
    //        table.AddCell(new Phrase(Address, FontFactory.GetFont("Arial", 9, Font.BOLD)));

    //        table.AddCell(new Phrase("Date :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
    //        table.AddCell(new Phrase(challanDate, FontFactory.GetFont("Arial", 9, Font.BOLD)));

    //        table.AddCell(new Phrase("GSt No :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
    //        table.AddCell(new Phrase(GSTNo, FontFactory.GetFont("Arial", 9, Font.BOLD)));

    //        table.AddCell(new Phrase("PO No :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
    //        table.AddCell(new Phrase(PoNumber, FontFactory.GetFont("Arial", 9, Font.BOLD)));

    //        table.AddCell(new Phrase("KinD Att :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
    //        table.AddCell(new Phrase(KindAtt, FontFactory.GetFont("Arial", 9, Font.BOLD)));

    //        table.AddCell(new Phrase("Date :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
    //        table.AddCell(new Phrase(PODate, FontFactory.GetFont("Arial", 9, Font.BOLD)));

    //        paragraphTable1.Add(table);
    //        doc.Add(paragraphTable1);

    //        Paragraph paragraphTable2 = new Paragraph();
    //        paragraphTable2.SpacingAfter = 0f;
    //        table = new PdfPTable(9);
    //        float[] widths3 = new float[] { 4f, 19f, 10f, 8f, 8f, 8f, 11f, 8f, 12f };
    //        table.SetWidths(widths3);

    //        double Ttotal_price = 0;
    //        if (Dt.Rows.Count > 0)
    //        {
    //            table.TotalWidth = 560f;
    //            table.LockedWidth = true;
    //            table.AddCell(new Phrase("SN.", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //            table.AddCell(new Phrase("Description", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //            table.AddCell(new Phrase("Hsn / Sac", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //            table.AddCell(new Phrase("Tax %", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //            table.AddCell(new Phrase("Quantity", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //            table.AddCell(new Phrase("Unit", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //            table.AddCell(new Phrase("Rate", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //            table.AddCell(new Phrase("Disc %", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //            table.AddCell(new Phrase("Total", FontFactory.GetFont("Arial", 10, Font.BOLD)));

    //            int rowid = 1;
    //            foreach (DataRow dr in Dt.Rows)
    //            {
    //                table.TotalWidth = 560f;
    //                table.LockedWidth = true;

    //                double Ftotal = Convert.ToDouble(dr["GrandTotal"].ToString());
    //                string _ftotal = Ftotal.ToString("##.00");
    //                table.AddCell(new Phrase(rowid.ToString(), FontFactory.GetFont("Arial", 9)));
    //                table.AddCell(new Phrase(dr["Description"].ToString(), FontFactory.GetFont("Arial", 9)));
    //                table.AddCell(new Phrase(dr["HSN_code"].ToString(), FontFactory.GetFont("Arial", 9)));
    //                table.AddCell(new Phrase(dr["Tax"].ToString(), FontFactory.GetFont("Arial", 9)));
    //                table.AddCell(new Phrase(dr["Qty"].ToString(), FontFactory.GetFont("Arial", 9)));
    //                table.AddCell(new Phrase(dr["Units"].ToString(), FontFactory.GetFont("Arial", 9)));
    //                table.AddCell(new Phrase(dr["Rate"].ToString(), FontFactory.GetFont("Arial", 9)));
    //                table.AddCell(new Phrase(dr["Discount"].ToString(), FontFactory.GetFont("Arial", 9)));
    //                table.AddCell(new Phrase(_ftotal, FontFactory.GetFont("Arial", 9)));
    //                rowid++;

    //                Ttotal_price += Convert.ToDouble(dr["GrandTotal"].ToString());
    //            }

    //        }
    //        paragraphTable2.Add(table);
    //        doc.Add(paragraphTable2);

    //        Space
    //        Paragraph paragraphTable3 = new Paragraph();

    //        string[] items = { "Goods once sold will not be taken back or exchange. \b",
    //                    "Interest at the rate of 18% will be charged if bill is'nt paid within 30 days.\b",
    //                    "Our risk and responsibility ceases the moment goods leaves out godown. \n",
    //                    };

    //        Font font12 = FontFactory.GetFont("Arial", 12, Font.BOLD);
    //        Font font10 = FontFactory.GetFont("Arial", 10, Font.BOLD);
    //        Paragraph paragraph = new Paragraph("", font12);

    //        for (int i = 0; i < items.Length; i++)
    //        {
    //            paragraph.Add(new Phrase("", font10));
    //        }

    //        table = new PdfPTable(9);
    //        table.TotalWidth = 560f;
    //        table.LockedWidth = true;
    //        table.SetWidths(new float[] { 4f, 19f, 10f, 8f, 8f, 8f, 11f, 8f, 12f });
    //        table.AddCell(paragraph);
    //        table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //        table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //        table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //        table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //        table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //        table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //        table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //        table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //        table.AddCell(new Phrase("  \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //        if (Dt.Rows.Count >= 10)
    //        {
    //            table.AddCell(new Phrase("  \n\n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //            doc.Add(table);
    //        }
    //        else if (Dt.Rows.Count >= 7 && Dt.Rows.Count <= 9)
    //        {
    //            table.AddCell(new Phrase("  \n\n\n\n\n\n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));

    //        }
    //        else if (Dt.Rows.Count >= 4 && Dt.Rows.Count <= 6)
    //        {
    //            table.AddCell(new Phrase("  \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));

    //        }
    //        else if (Dt.Rows.Count < 4)
    //        {

    //            table.AddCell(new Phrase("  \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));

    //        }
    //        doc.Add(table);

    //        Add Total Row start
    //        Paragraph paragraphTable5 = new Paragraph();



    //        string[] itemsss = { "Goods once sold will not be taken back or exchange. \b",
    //                    "Interest at the rate of 18% will be charged if bill is'nt paid within 30 days.\b",
    //                    "Our risk and responsibility ceases the moment goods leaves out godown. \n",
    //                    };

    //        Font font13 = FontFactory.GetFont("Arial", 12, Font.BOLD);
    //        Font font11 = FontFactory.GetFont("Arial", 10, Font.BOLD);
    //        Paragraph paragraphh = new Paragraph("", font12);



    //        for (int i = 0; i < items.Length; i++)
    //        {
    //            paragraph.Add(new Phrase("", font10));
    //        }

    //        table = new PdfPTable(3);
    //        table.TotalWidth = 560f;
    //        table.LockedWidth = true;

    //        paragraph.Alignment = Element.ALIGN_RIGHT;

    //        table.SetWidths(new float[] { 0f, 76f, 12f });
    //        table.AddCell(paragraph);
    //        PdfPCell cell = new PdfPCell(new Phrase("Sub Total", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
    //        table.AddCell(cell);
    //        PdfPCell cell11 = new PdfPCell(new Phrase(Total, FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //        cell11.HorizontalAlignment = Element.ALIGN_RIGHT;
    //        table.AddCell(cell11);


    //        doc.Add(table);
    //        add total row end

    //        CGST 9 % Row STart
    //         Paragraph paragraphTable15 = new Paragraph();
    //        paragraphTable5.SpacingAfter = 0f;


    //        string[] itemss = { "Goods once sold will not be taken back or exchange. \b",
    //                    "Interest at the rate of 18% will be charged if bill is'nt paid within 30 days.\b",
    //                    "Our risk and responsibility ceases the moment goods leaves out godown. \n",
    //                    };

    //        Font font1 = FontFactory.GetFont("Arial", 12, Font.BOLD);
    //        Font font2 = FontFactory.GetFont("Arial", 10, Font.BOLD);
    //        Paragraph paragraphhh = new Paragraph("", font12);



    //        for (int i = 0; i < items.Length; i++)
    //        {
    //            paragraph.Add(new Phrase("", font10));
    //        }

    //        table = new PdfPTable(3);
    //        table.TotalWidth = 560f;
    //        table.LockedWidth = true;

    //        table.SetWidths(new float[] { 0f, 76f, 12f });
    //        table.AddCell(paragraph);
    //        PdfPCell cell2 = new PdfPCell(new Phrase("CGST %", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //        cell2.HorizontalAlignment = Element.ALIGN_RIGHT;
    //        table.AddCell(cell2);
    //        PdfPCell cell3 = new PdfPCell(new Phrase(CGST, FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //        cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
    //        table.AddCell(cell3);

    //        doc.Add(table);
    //        CGST 9 % Row End

    //         SGST 9 % Row STart
    //          Paragraph paragraphTable16 = new Paragraph();
    //        paragraphTable5.SpacingAfter = 10f;


    //        string[] item = { "Goods once sold will not be taken back or exchange. \b",
    //                    "Interest at the rate of 18% will be charged if bill is'nt paid within 30 days.\b",
    //                    "Our risk and responsibility ceases the moment goods leaves out godown. \n",
    //                    };

    //        Font font14 = FontFactory.GetFont("Arial", 12, Font.BOLD);
    //        Font font15 = FontFactory.GetFont("Arial", 10, Font.BOLD);
    //        Paragraph paragraphhhh = new Paragraph("", font12);



    //        for (int i = 0; i < items.Length; i++)
    //        {
    //            paragraph.Add(new Phrase("", font10));
    //        }

    //        table = new PdfPTable(3);
    //        table.TotalWidth = 560f;
    //        table.LockedWidth = true;

    //        table.SetWidths(new float[] { 0f, 76f, 12f });
    //        table.AddCell(paragraph);
    //        PdfPCell cell22 = new PdfPCell(new Phrase("SGST %", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //        cell22.HorizontalAlignment = Element.ALIGN_RIGHT;
    //        table.AddCell(cell22);
    //        PdfPCell cell33 = new PdfPCell(new Phrase(SGST, FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //        cell33.HorizontalAlignment = Element.ALIGN_RIGHT;
    //        table.AddCell(cell33);


    //        doc.Add(table);
    //        SGST 9 % Row End

    //         Grand total Row STart
    //         Paragraph paragraphTable17 = new Paragraph();
    //        paragraphTable5.SpacingAfter = 10f;

    //        string[] itemm = { "Goods once sold will not be taken back or exchange. \b",
    //                    "Interest at the rate of 18% will be charged if bill is'nt paid within 30 days.\b",
    //                    "Our risk and responsibility ceases the moment goods leaves out godown. \n",
    //                    };

    //        Font font16 = FontFactory.GetFont("Arial", 12, Font.BOLD);
    //        Font font17 = FontFactory.GetFont("Arial", 10, Font.BOLD);
    //        Paragraph paragraphhhhh = new Paragraph("", font12);

    //        paragraphh.SpacingAfter = 10f;

    //        for (int i = 0; i < items.Length; i++)
    //        {
    //            paragraph.Add(new Phrase("", font10));
    //        }

    //        table = new PdfPTable(3);
    //        table.TotalWidth = 560f;
    //        table.LockedWidth = true;

    //        var Grndttl = Convert.ToDecimal(Ttotal_price) + Convert.ToDecimal(Cgst_9) + Convert.ToDecimal(Sgst_9);

    //        table.SetWidths(new float[] { 0f, 76f, 12f });
    //        table.AddCell(paragraph);
    //        PdfPCell cell44 = new PdfPCell(new Phrase("Grand Total", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //        cell44.HorizontalAlignment = Element.ALIGN_RIGHT;
    //        table.AddCell(cell44);
    //        PdfPCell cell55 = new PdfPCell(new Phrase(Total, FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //        cell55.HorizontalAlignment = Element.ALIGN_RIGHT;
    //        table.AddCell(cell55);


    //        doc.Add(table);
    //        Grand total Row End

    //        Grand total in word Row STart
    //        Paragraph paragraphTable18 = new Paragraph();
    //        paragraphTable18.SpacingAfter = 50f;

    //        string[] itemmm = { "Goods once sold will not be taken back or exchange. \b",
    //                    "Interest at the rate of 18% will be charged if bill is'nt paid within 30 days.\b",
    //                    "Our risk and responsibility ceases the moment goods leaves out godown. \n",
    //                    };

    //        Font font18 = FontFactory.GetFont("Arial", 12, Font.BOLD);
    //        Font font19 = FontFactory.GetFont("Arial", 10, Font.BOLD);
    //        Paragraph paragraphhmhhh = new Paragraph("", font12);



    //        for (int i = 0; i < items.Length; i++)
    //        {
    //            paragraph.Add(new Phrase("", font10));
    //        }

    //        table = new PdfPTable(3);
    //        table.TotalWidth = 560f;
    //        table.LockedWidth = true;

    //        table.SetWidths(new float[] { 0f, 25f, 63f });
    //        table.AddCell(paragraph);
    //        PdfPCell cell66 = new PdfPCell(new Phrase("Amount In Words Rs. ", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //        cell66.HorizontalAlignment = Element.ALIGN_CENTER;
    //        table.AddCell(cell66);
    //        PdfPCell cell77 = new PdfPCell(new Phrase(TotalInWord, FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //        cell77.HorizontalAlignment = Element.ALIGN_CENTER;
    //        table.AddCell(cell77);
    //        doc.Add(table);


    //        PdfContentByte cn = writer.DirectContent;
    //        cn.Rectangle(17f, 160f, 560f, 115f);
    //        cn.Stroke();
    //        Header
    //        cn.BeginText();
    //        cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 14);
    //        cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Bank Details :", 30, 258, 0);
    //        cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibri.ttf", "Identity-H", BaseFont.EMBEDDED), 12);
    //        cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Bank Name : BANK OF BARODA", 30, 240, 0);
    //        cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibri.ttf", "Identity-H", BaseFont.EMBEDDED), 12);
    //        cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Account Name : ENDEAVOUR AUTOMATION", 30, 225, 0);
    //        cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibri.ttf", "Identity-H", BaseFont.EMBEDDED), 12);
    //        cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Branch : KALEWADI", 30, 210, 0);
    //        cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibri.ttf", "Identity-H", BaseFont.EMBEDDED), 12);
    //        cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "A/c No : 46180200000214", 30, 195, 0);
    //        cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibri.ttf", "Identity-H", BaseFont.EMBEDDED), 12);
    //        cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "IFSC/Neft Code :BARB0KALEWA", 30, 180, 0);

    //        cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibrii.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
    //        cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "We Thank you for your enquiry", 440, 256, 0);
    //        cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 12);
    //        cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "For", 392, 230, 0);
    //        cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 13);
    //        cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "ENDEAVOUR AUTOMATION", 413, 230, 0);
    //        cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 13);
    //        cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Authorised Signatory", 443, 180, 0);
    //        cn.EndText();


    //        Paragraph paragraphTable4 = new Paragraph();

    //        paragraphTable4.SpacingBefore = 10f;

    //        table = new PdfPTable(2);
    //        table.TotalWidth = 560f;

    //        float[] widths = new float[] { 160f, 400f };
    //        table.SetWidths(widths);
    //        table.LockedWidth = true;

    //        doc.Close();


    //        Byte[] FileBuffer = File.ReadAllBytes(Server.MapPath("~/Files/") + "DeliveryChallan.pdf");
    //        string empFilename = "DeliveryChallan" + DateTime.Now.ToShortDateString() + ".pdf";

    //        if (FileBuffer != null)
    //        {
    //            Response.ContentType = "application/pdf";
    //            Response.AddHeader("content-length", FileBuffer.Length.ToString());
    //            Response.BinaryWrite(FileBuffer);
    //            Response.AddHeader("Content-Disposition", "attachment;filename=" + empFilename);
    //        }

    //    }
    //    doc.Close();

    //}
    protected void lnkBtn_rfresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("DeliveryChallanReport.aspx");
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

    protected void gv_Deliverychallan_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName == "RowView")
        {
            Response.Write("<script>window.open ('../reportPdf/DeliverychallanPdf.aspx?ChallanNo=" + encrypt(e.CommandArgument.ToString()) + "','_blank');</script>");

            //string id = e.CommandArgument.ToString();

            //Pdf(id);
        }
        if (e.CommandName == "RowShow")
        {
            Response.Redirect("DeliveryChallan.aspx?Id=" + encrypt(e.CommandArgument.ToString()) + "");
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
    protected void gv_Deliverychallan_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
       
        if (ViewState["Record"] != null)
        {

            if (ViewState["Record"].ToString() == "Gedatwisecustomer")
            {
                gv_Deliverychallan.PageIndex = e.NewPageIndex;
                Gedatwisecustomer();
            }

            if (ViewState["Record"].ToString() == "  Gedatwisecustomer1")
            {
                gv_Deliverychallan.PageIndex = e.NewPageIndex;
                Gedatwisecustomer1();


            }
            if (ViewState["Record"].ToString() == "Gedatwisecustomer2")
            {
                gv_Deliverychallan.PageIndex = e.NewPageIndex;
                Gedatwisecustomer2();
            }
            if (ViewState["Record"].ToString() == "Gedatwisecustomer3")
            {
                gv_Deliverychallan.PageIndex = e.NewPageIndex;
                Gedatwisecustomer3();
            }
            if (ViewState["Record"].ToString() == "Gedatwisecustomer4")
            {
                gv_Deliverychallan.PageIndex = e.NewPageIndex;
                Gedatwisecustomer4();
            }
            if (ViewState["Record"].ToString() == "Gedatwisecustomer5")
            {
                gv_Deliverychallan.PageIndex = e.NewPageIndex;
                Gedatwisecustomer5();
            }
            if (ViewState["Record"].ToString() == "Gedatwisecustomer6")
            {
                gv_Deliverychallan.PageIndex = e.NewPageIndex;
                Gedatwisecustomer6();
            }
        }
        else
        {
            gv_Deliverychallan.PageIndex = e.NewPageIndex;
            Gridrecord();
        }
    }

    public void Gedatwisecustomer()

    {
        ViewState["Record"] = "Gedatwisecustomer";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblChallanHdr where CustomerName='" + txt_Customername_search.Text + "' AND ChallanNo='" + txtchallanNo.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        gv_Deliverychallan.DataSource = dt;
        gv_Deliverychallan.DataBind();
        gv_Deliverychallan.EmptyDataText = "Record Not Found";
    }

    public void Gedatwisecustomer1()
    {
        ViewState["Excell"] = "Gedatwisecustomer1";
        ViewState["Record"] = "Gedatwisecustomer1";
        DateTime date = Convert.ToDateTime(txt_formsearch.Text.ToString());
        txt_formsearch.Text = date.ToString("yyyy-MM-dd");

        DateTime datee = Convert.ToDateTime(txt_Tosearch.Text.ToString());
        txt_Tosearch.Text = datee.ToString("yyyy-MM-dd");
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblChallanHdr where ChallanDate BETWEEN '" + txt_formsearch.Text + "' AND '" + txt_Tosearch.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        gv_Deliverychallan.DataSource = dt;
        gv_Deliverychallan.DataBind();
        gv_Deliverychallan.EmptyDataText = "Record Not Found";
    }


    public void Gedatwisecustomer1Excel()
    {
        ViewState["Excell"] = "Gedatwisecustomer1";
        ViewState["Record"] = "Gedatwisecustomer1";
        DateTime date = Convert.ToDateTime(txt_formsearch.Text.ToString());
        txt_formsearch.Text = date.ToString("yyyy-MM-dd");

        DateTime datee = Convert.ToDateTime(txt_Tosearch.Text.ToString());
        txt_Tosearch.Text = datee.ToString("yyyy-MM-dd");
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblChallanHdr where ChallanDate BETWEEN '" + txt_formsearch.Text + "' AND '" + txt_Tosearch.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        sortedgv.DataSource = dt;
        sortedgv.DataBind();
        sortedgv.EmptyDataText = "Record Not Found";
    }


    public void Gedatwisecustomer2()
    {
        ViewState["Excell"] = "Gedatwisecustomer2";
        ViewState["Record"] = "Gedatwisecustomer2";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblChallanHdr where CustomerName='" + txt_Customername_search.Text + "'  AND isdeleted='0'", con);
        sad.Fill(dt);
        gv_Deliverychallan.DataSource = dt;
        gv_Deliverychallan.DataBind();
        gv_Deliverychallan.EmptyDataText = "Record Not Found";
    }

    public void Gedatwisecustomer2Excel()
    {
        ViewState["Excell"] = "Gedatwisecustomer2";
        ViewState["Record"] = "Gedatwisecustomer2";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblChallanHdr where CustomerName='" + txt_Customername_search.Text + "'  AND isdeleted='0'", con);
        sad.Fill(dt);
        sortedgv.DataSource = dt;
        sortedgv.DataBind();
        sortedgv.EmptyDataText = "Record Not Found";
    }

    public void Gedatwisecustomer3()
    {     
        ViewState["Excell"] = "Gedatwisecustomer3";
        ViewState["Record"] = "Gedatwisecustomer3";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblChallanHdr where  ChallanNo='" + txtchallanNo.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        gv_Deliverychallan.DataSource = dt;
        gv_Deliverychallan.DataBind();
        gv_Deliverychallan.EmptyDataText = "Record Not Found";
    }

    public void Gedatwisecustomer3Excel()
    {

        ViewState["Excell"] = "Gedatwisecustomer3";
        ViewState["Record"] = "Gedatwisecustomer3";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblChallanHdr where  ChallanNo='" + txtchallanNo.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        sortedgv.DataSource = dt;
        sortedgv.DataBind();
        sortedgv.EmptyDataText = "Record Not Found";
    }

    public void Gedatwisecustomer4()

    {
        ViewState["Record"] = "Gedatwisecustomer4";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblChallanHdr where ChallanDate ='" + txt_formsearch.Text + "'  AND isdeleted='0'", con);
        sad.Fill(dt);
        gv_Deliverychallan.DataSource = dt;
        gv_Deliverychallan.DataBind();
        gv_Deliverychallan.EmptyDataText = "Record Not Found";
    }

    public void Gedatwisecustomer5()
    {
        ViewState["Record"] = "Gedatwisecustomer5";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblChallanHdr where ChallanDate ='" + txt_Tosearch.Text + "'  AND isdeleted='0'", con);
        sad.Fill(dt);
        gv_Deliverychallan.DataSource = dt;
        gv_Deliverychallan.DataBind();
        gv_Deliverychallan.EmptyDataText = "Record Not Found";
    }

    public void Gedatwisecustomer6()

    {
        ViewState["Record"] = "Gedatwisecustomer6";
        DateTime date = Convert.ToDateTime(txt_formsearch.Text.ToString());
        txt_Tosearch.Text = date.ToString("yyyy-MM-dd");

        DateTime datee = Convert.ToDateTime(txt_Tosearch.Text.ToString());
        txt_Tosearch.Text = datee.ToString("yyyy-MM-dd");
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblChallanHdr where ChallanDate BETWEEN '" + txt_formsearch.Text + "' AND '" + txt_Tosearch.Text + "'  AND  CustomerName= '" + txt_Customername_search.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        gv_Deliverychallan.DataSource = dt;
        gv_Deliverychallan.DataBind();
        gv_Deliverychallan.EmptyDataText = "Record Not Found";
    }

    protected void btnExportExcel_Click(object sender, EventArgs e)
    {
        ExportGridToExcel();
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        //required to avoid the run time error "
        //Control 'GridView1' of type 'Grid View' must be placed inside a form tag with runat=server."
    }

    private void ExportGridToExcel()
    {

        if (ViewState["Excell"] != null)
        {
            gv_Deliverychallan.Visible = false;
            string Method = ViewState["Excell"].ToString();

            if (Method == "Gedatwisecustomer3")
            {
                Gedatwisecustomer3Excel();
            }
            if (Method == "Gedatwisecustomer2")
            {
                Gedatwisecustomer2Excel();
            }
            if (Method == "Gedatwisecustomer1")
            {
                Gedatwisecustomer1Excel();
            }
            

            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";
            string FileName = "Inward_Entry_List_" + DateTime.Now + ".xls";
            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            sortedgv.GridLines = GridLines.Both;
            sortedgv.HeaderStyle.Font.Bold = true;
            sortedgv.RenderControl(htmltextwrtter);
            Response.Write(strwritter.ToString());
            Response.End();
        }
    }
}