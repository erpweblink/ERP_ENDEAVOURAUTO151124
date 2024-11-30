using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Org.BouncyCastle.Asn1.Cmp;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
//using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_TaxInvoiceList : System.Web.UI.Page
{
    SqlConnection Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
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
                Load_Record();

                //UserCompany = Session["adminname"].ToString();
                //if (UserCompany != "Admin")
                //{
                //    gvbind_Company();
                //    btncreate.Visible = false;
                //    this.GvPurchaseOrderList.Columns[9].Visible = false;
                //}



                //else
                //{
                //    Load_Record();                
                //}            
            }

            // New Code to show  the count 
            int jobCount = GetJobCount();
            lblcount.Text = jobCount.ToString();
            if (Convert.ToInt32(lblcount.Text) > 0)
            {
                lnkshow.Attributes.Add("class", "bell-bounce");
            }
            else
            {
                lnkshow.Attributes.Remove("class");
            }
        }
    }

    void gvbind_Company()
    {
        try
        {
            string UserCompany = Session["name"].ToString();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_Invoice_both_hdr WHERE Is_Deleted='0' AND CustName='" + UserCompany + "' ", Conn);
            //SqlDataAdapter sad = new SqlDataAdapter("SELECT ID,Quotation_no,Quotation_Date,JobNo,Customer_Name,Address,Mobile_No,Phone_No,GST_No,State_Code,kind_Att,CGST,SGST,AllTotal_price,Total_in_word,IsDeleted,CreatedBy,CreatedOn,DATEDIFF(DAY, Quotation_Date, getdate()) AS days FROM tbl_Quotation_Hdr WHERE IsDeleted='0' AND isCompleted='1'", con);
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
        //SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_Invoice_both_hdr WHERE Is_Deleted='0' ORDER BY CreatedOn Desc ", Conn);
        SqlDataAdapter da = new SqlDataAdapter(@"WITH DistinctInvoices AS (
        SELECT DISTINCT
            IH.Id,
            IH.InvoiceNo,
            IH.PoNo,
            IH.CompName,
            IH.ChallanNo,
            IH.PayTerm,
            IH.InvoiceDate,
            IH.CreatedBy,
            IH.CreatedOn,
            IH.CGST,
            IH.SGST,
            IH.IGST,
            IH.AllTotalAmount,
            IH.GrandTotal,
            IH.Type
        FROM
            tbl_Invoice_both_hdr AS IH
        WHERE
            IH.Is_Deleted = '0' AND Type = 'JobNo'
    )
    SELECT
        DI.Id,
        DI.InvoiceNo,
        DI.PoNo,
        DI.CompName,
        DI.ChallanNo,
        DI.PayTerm,
        DI.InvoiceDate,
        DI.CreatedBy,
        DI.CreatedOn,
        DI.CGST,
        DI.SGST,
        DI.IGST,
        DI.Type,
        DI.AllTotalAmount,
        DI.GrandTotal,
        (
            SELECT STUFF((
                SELECT ',' + CAST(dtls.JobNo AS VARCHAR(10))
                FROM tbl_Invoice_both_Dtls AS dtls
                WHERE dtls.InvoiceNo = DI.InvoiceNo
                FOR XML PATH(''), TYPE
            ).value('.', 'VARCHAR(MAX)'), 1, 1, '')
        ) AS JobNo
    FROM
        DistinctInvoices AS DI
    ORDER BY
        DI.CreatedOn DESC;", Conn);
        da.Fill(Dt);
        GvPurchaseOrderList.EmptyDataText = "Records Not Found";
        GvPurchaseOrderList.DataSource = Dt;
        GvPurchaseOrderList.DataBind();
    }

    private void GridExport()
    {
        DataTable Dt = new DataTable();
        //SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_Invoice_both_hdr WHERE Is_Deleted='0' ORDER BY CreatedOn Desc ", Conn);
        SqlDataAdapter da = new SqlDataAdapter(@"WITH DistinctInvoices AS (
        SELECT DISTINCT
            IH.Id,
            IH.InvoiceNo,
            IH.PoNo,
            IH.CompName,
            IH.ChallanNo,
            IH.PayTerm,
            IH.InvoiceDate,
            IH.CreatedBy,
            IH.CreatedOn,
            IH.CGST,
            IH.SGST,
            IH.IGST,
            IH.AllTotalAmount,
            IH.GrandTotal,
            IH.Type
        FROM
            tbl_Invoice_both_hdr AS IH
        WHERE
            IH.Is_Deleted = '0'
    )
    SELECT
        DI.Id,
        DI.InvoiceNo,
        DI.PoNo,
        DI.CompName,
        DI.ChallanNo,
        DI.PayTerm,
        DI.InvoiceDate,
        DI.CreatedBy,
        DI.CreatedOn,
        DI.CGST,
        DI.SGST,
        DI.IGST,
        DI.AllTotalAmount,
        DI.GrandTotal,
        DI.Type
        (
            SELECT STUFF((
                SELECT ',' + CAST(dtls.JobNo AS VARCHAR(10))
                FROM tbl_Invoice_both_Dtls AS dtls
                WHERE dtls.InvoiceNo = DI.InvoiceNo
                FOR XML PATH(''), TYPE
            ).value('.', 'VARCHAR(MAX)'), 1, 1, '')
        ) AS JobNo
    FROM
        DistinctInvoices AS DI
    ORDER BY
        DI.CreatedOn DESC;", Conn);
        da.Fill(Dt);
        GridExportExcel.EmptyDataText = "Records Not Found";
        GridExportExcel.DataSource = Dt;
        GridExportExcel.DataBind();
    }


    protected void gv_Tax_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //Added New for Count by Shubham Patil
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            decimal totalAmount = 0;

            if (Gvsorted.Rows.Count > 0)
            {
                foreach (GridViewRow row in Gvsorted.Rows)
                {

                    Label lblIGST = row.FindControl("lblIGST") as Label;
                    if (lblIGST != null)
                    {
                        if (decimal.TryParse(lblIGST.Text, out decimal rowAmount)) 
                        {
                            totalAmount += rowAmount;
                        }
                    }
                }
            }
            else
            {
                foreach (GridViewRow row in GvPurchaseOrderList.Rows)
                {
                    Label lblIGST = row.FindControl("lblIGST") as Label;
                    if (lblIGST != null)
                    {
                        if (decimal.TryParse(lblIGST.Text, out decimal rowAmount))
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


    protected void GvPurchaseOrderList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "RowDelete")
        {
            Conn.Open();
            SqlCommand Cmd = new SqlCommand("UPDATE tbl_Invoice_both_hdr SET Is_Deleted='1' WHERE Id=@Id", Conn);

            Cmd.Parameters.AddWithValue("Id", Convert.ToInt32(e.CommandArgument.ToString()));
            Cmd.Parameters.AddWithValue("Is_Deleted", '1');

            Cmd.ExecuteNonQuery();
            //ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Record Deleted Sucessfully');", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Record Deleted Sucessfully');", true);
            Response.Redirect("TaxInvoiceList.aspx");

        }

        if (e.CommandName == "RowPrint")
        {
            Response.Write("<script>window.open ('../reportPdf/Invoicepdf.aspx?Id=" + encrypt(e.CommandArgument.ToString()) + "','_blank');</script>");

            //string ID = e.CommandArgument.ToString();
            //Pdf(ID);
        }
        if (e.CommandName == "RowEdit")
        {
            Response.Redirect("TaxInvoice.aspx?Id=" + encrypt(e.CommandArgument.ToString()) + "");
        }
        if (e.CommandName == "SendINV")
        {
            int id = Convert.ToInt32(e.CommandArgument);
            string QuatationNo = GvPurchaseOrderList.DataKeys[id]["Quotationno"].ToString();
            Response.Redirect("TaxInvoice.aspx?QuatationNo=" + Server.UrlEncode(QuatationNo));
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

    //private void Pdf(string ID)
    //{
    //    DataTable Dt = new DataTable();
    //    SqlDataAdapter da = new SqlDataAdapter("SELECT tbl_Invoice_both_hdr.Id,InvoiceNo,InvoiceDate,PoNo,PoDate,ChallanNo,ChallanDate,PayTerm," +
    //        "Delivery,KindAtt,CompanyAddress,CompanyGstNo,CompanyPanNo, ComapyRegType, CompanyStateCode, " +
    //        "CustomerShippingAddress, CustomerGstNo, CustomerPanNo, CustomerRegType,CustomerStateCode, CGST, SGST," +
    //        " AllTotalAmount, GrandTotal, TotalInWord, Description, Hsn, TaxPercentage, Quntity, Unit, Rate, DiscountPercentage," +
    //        " Total,CreatedOn FROM tbl_Invoice_both_hdr INNER JOIN tbl_Invoice_both_Dtls ON tbl_Invoice_both_Dtls.InvoiceId = tbl_Invoice_both_hdr.Id WHERE tbl_Invoice_both_hdr.Id='" + ID + "'", Conn);
    //    da.Fill(Dt);
    //    GvPurchaseOrderList.DataSource = Dt;
    //    GvPurchaseOrderList.DataBind();

    //    StringWriter sw = new StringWriter();
    //    StringReader sr = new StringReader(sw.ToString());

    //    Document doc = new Document(PageSize.A4, 10f, 10f, 55f, 0f);

    //    PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(Server.MapPath("~/Files/") + "TaxInvoice.pdf", FileMode.Create));
    //    //PdfWriter writer = PdfWriter.GetInstance(doc, Response.OutputStream);
    //    iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, sr);

    //    doc.Open();

    //    string imageURL = Server.MapPath("~") + "/image/AA.png";

    //    iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(imageURL);

    //    //Resize image depend upon your need

    //    png.ScaleToFit(70, 100);

    //    //For Image Position
    //    png.SetAbsolutePosition(40, 745);
    //    //var document = new Document();

    //    //Give space before image
    //    //png.ScaleToFit(document.PageSize.Width - (document.RightMargin * 100), 50);
    //    png.SpacingBefore = 50f;

    //    //Give some space after the image

    //    png.SpacingAfter = 1f;

    //    png.Alignment = Element.ALIGN_LEFT;

    //    //paragraphimage.Add(png);
    //    //doc.Add(paragraphimage);
    //    doc.Add(png);


    //    PdfContentByte cb = writer.DirectContent;
    //    cb.Rectangle(17f, 735f, 560f, 60f);
    //    cb.Stroke();
    //    // Header 
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
    //    // Header 
    //    cbb.BeginText();
    //    cbb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
    //    cbb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " Mob: 9860502108    Email: endeavour.automations@gmail.com ", 153, 722, 0);
    //    cbb.EndText();

    //    PdfContentByte cbbb = writer.DirectContent;
    //    cbbb.Rectangle(17f, 685f, 560f, 25f);
    //    cbbb.Stroke();
    //    // Header 
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
    //    // Header 
    //    cd.BeginText();
    //    cd.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 17);
    //    cd.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Tax Invoice", 260, 667, 0);
    //    cd.EndText();

    //    if (Dt.Rows.Count > 0)
    //    {
    //        var CreateDate = DateTime.Now.ToString("yyyy-MM-dd");
    //        string InvoiceNo = Dt.Rows[0]["InvoiceNo"].ToString();
    //        string PoNumber = Dt.Rows[0]["PoNo"].ToString();
    //        string Address = Dt.Rows[0]["CustomerShippingAddress"].ToString();
    //        string InvoiceDate = Dt.Rows[0]["InvoiceDate"].ToString().TrimEnd("0:0".ToCharArray());
    //        string ChallanNo = Dt.Rows[0]["ChallanNo"].ToString();
    //        string GSTNo = Dt.Rows[0]["CustomerGstNo"].ToString();
    //        string KindAtt = Dt.Rows[0]["KindAtt"].ToString();
    //        string TotalInWord = Dt.Rows[0]["TotalInWord"].ToString();
    //        string GrandTotal = Dt.Rows[0]["GrandTotal"].ToString();
    //        string CGST = Dt.Rows[0]["CGST"].ToString();
    //        string SGST = Dt.Rows[0]["SGST"].ToString();
    //        string Total = Dt.Rows[0]["AllTotalAmount"].ToString();

    //        Paragraph paragraphTable1 = new Paragraph();
    //        paragraphTable1.SpacingBefore = 120f;
    //        paragraphTable1.SpacingAfter = 10f;

    //        PdfPTable table = new PdfPTable(4);

    //        float[] widths2 = new float[] { 100, 180, 100, 180 };
    //        table.SetWidths(widths2);
    //        table.TotalWidth = 560f;
    //        table.LockedWidth = true;

    //        DateTime ffff1 = Convert.ToDateTime(Dt.Rows[0]["InvoiceDate"].ToString());
    //        string datee = ffff1.ToString("yyyy-MM-dd");


    //        table.AddCell(new Phrase("Invoice Number : ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
    //        table.AddCell(new Phrase(InvoiceNo, FontFactory.GetFont("Arial", 9, Font.BOLD)));

    //        table.AddCell(new Phrase("PO Number :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
    //        table.AddCell(new Phrase(PoNumber, FontFactory.GetFont("Arial", 9, Font.BOLD)));

    //        table.AddCell(new Phrase("Address", FontFactory.GetFont("Arial", 9, Font.BOLD)));
    //        table.AddCell(new Phrase(Address, FontFactory.GetFont("Arial", 9, Font.BOLD)));

    //        table.AddCell(new Phrase("Invoice Date :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
    //        table.AddCell(new Phrase(datee, FontFactory.GetFont("Arial", 9, Font.BOLD)));

    //        table.AddCell(new Phrase("Challan No :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
    //        table.AddCell(new Phrase(ChallanNo, FontFactory.GetFont("Arial", 9, Font.BOLD)));

    //        table.AddCell(new Phrase("GST No :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
    //        table.AddCell(new Phrase(GSTNo, FontFactory.GetFont("Arial", 9, Font.BOLD)));

    //        table.AddCell(new Phrase("Created Date :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
    //        table.AddCell(new Phrase(CreateDate, FontFactory.GetFont("Arial", 9, Font.BOLD)));

    //        table.AddCell(new Phrase("Kind Attn. :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
    //        table.AddCell(new Phrase(KindAtt, FontFactory.GetFont("Arial", 9, Font.BOLD)));

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

    //                double Ftotal = Convert.ToDouble(dr["Total"].ToString());
    //                string _ftotal = Ftotal.ToString("##.00");
    //                table.AddCell(new Phrase(rowid.ToString(), FontFactory.GetFont("Arial", 9)));
    //                table.AddCell(new Phrase(dr["Description"].ToString(), FontFactory.GetFont("Arial", 9)));
    //                table.AddCell(new Phrase(dr["Hsn"].ToString(), FontFactory.GetFont("Arial", 9)));
    //                table.AddCell(new Phrase(dr["TaxPercentage"].ToString(), FontFactory.GetFont("Arial", 9)));
    //                table.AddCell(new Phrase(dr["Quntity"].ToString(), FontFactory.GetFont("Arial", 9)));
    //                table.AddCell(new Phrase(dr["Unit"].ToString(), FontFactory.GetFont("Arial", 9)));
    //                table.AddCell(new Phrase(dr["Rate"].ToString(), FontFactory.GetFont("Arial", 9)));
    //                table.AddCell(new Phrase(dr["DiscountPercentage"].ToString(), FontFactory.GetFont("Arial", 9)));
    //                table.AddCell(new Phrase(_ftotal, FontFactory.GetFont("Arial", 9)));
    //                rowid++;

    //                Ttotal_price += Convert.ToDouble(dr["Total"].ToString());
    //            }

    //        }
    //        paragraphTable2.Add(table);
    //        doc.Add(paragraphTable2);

    //        //Space
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
    //        //table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //        table.AddCell(new Phrase("  \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));

    //        doc.Add(table);

    //        //Add Total Row start
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
    //        //add total row end

    //        //CGST 9% Row STart
    //        Paragraph paragraphTable15 = new Paragraph();
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
    //        PdfPCell cell2 = new PdfPCell(new Phrase("CGST 9%", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //        cell2.HorizontalAlignment = Element.ALIGN_RIGHT;
    //        table.AddCell(cell2);
    //        PdfPCell cell3 = new PdfPCell(new Phrase(CGST, FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //        cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
    //        table.AddCell(cell3);

    //        doc.Add(table);
    //        //CGST 9% Row End

    //        //SGST 9% Row STart
    //        Paragraph paragraphTable16 = new Paragraph();
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
    //        PdfPCell cell22 = new PdfPCell(new Phrase("SGST 9%", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //        cell22.HorizontalAlignment = Element.ALIGN_RIGHT;
    //        table.AddCell(cell22);
    //        PdfPCell cell33 = new PdfPCell(new Phrase(SGST, FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //        cell33.HorizontalAlignment = Element.ALIGN_RIGHT;
    //        table.AddCell(cell33);


    //        doc.Add(table);
    //        //SGST 9% Row End

    //        //Grand total Row STart
    //        Paragraph paragraphTable17 = new Paragraph();
    //        paragraphTable5.SpacingAfter = 10f;

    //        string[] itemm = { "Goods once sold will not be taken back or exchange. \b",
    //                    "Interest at the rate of 18% will be charged if bill is'nt paid within 30 days.\b",
    //                    "Our risk and responsibility ceases the moment goods leaves out godown. \n",
    //                    };

    //        Font font16 = FontFactory.GetFont("Arial", 12, Font.BOLD);
    //        Font font17 = FontFactory.GetFont("Arial", 10, Font.BOLD);
    //        Paragraph paragraphhhhh = new Paragraph("", font12);

    //        //paragraphh.SpacingAfter = 10f;

    //        for (int i = 0; i < items.Length; i++)
    //        {
    //            paragraph.Add(new Phrase("", font10));
    //        }

    //        table = new PdfPTable(3);
    //        table.TotalWidth = 560f;
    //        table.LockedWidth = true;

    //        //var Grndttl = Convert.ToDecimal(Ttotal_price) + Convert.ToDecimal(Cgst_9) + Convert.ToDecimal(Sgst_9);

    //        table.SetWidths(new float[] { 0f, 76f, 12f });
    //        table.AddCell(paragraph);
    //        PdfPCell cell44 = new PdfPCell(new Phrase("Grand Total", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //        cell44.HorizontalAlignment = Element.ALIGN_RIGHT;
    //        table.AddCell(cell44);
    //        PdfPCell cell55 = new PdfPCell(new Phrase(GrandTotal, FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //        cell55.HorizontalAlignment = Element.ALIGN_RIGHT;
    //        table.AddCell(cell55);


    //        doc.Add(table);
    //        //Grand total Row End

    //        //Grand total in word Row STart
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
    //        // Header 
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


    //        Byte[] FileBuffer = File.ReadAllBytes(Server.MapPath("~/Files/") + "TaxInvoice.pdf");
    //        string empFilename = "TaxInvoice" + DateTime.Now.ToShortDateString() + ".pdf";

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
    void AddPageNumber(string fileIn, string fileOut)
    {
        byte[] bytes = File.ReadAllBytes(fileIn);
        Font blackFont = FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK);
        using (MemoryStream stream = new MemoryStream())
        {
            PdfReader reader = new PdfReader(bytes);
            using (PdfStamper stamper = new PdfStamper(reader, stream))
            {
                int pages = reader.NumberOfPages;
                for (int i = 1; i <= pages; i++)
                {
                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_RIGHT, new Phrase(i.ToString(), blackFont), 568f, 15f, 0);
                }
            }
            bytes = stream.ToArray();
        }
        File.WriteAllBytes(fileOut, bytes);
    }


    private void Pdf(string ID)
    {
        DataTable Dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter("SELECT tbl_Invoice_both_hdr.Id,InvoiceNo,InvoiceDate,PoNo,PoDate,ChallanNo,ChallanDate,PayTerm," +
            "Delivery,KindAtt,CompanyAddress,CompanyGstNo,CompanyPanNo, ComapyRegType, CompanyStateCode, " +
            "CustomerShippingAddress, CustomerGstNo, CustomerPanNo, CustomerRegType,CustomerStateCode, CGST, SGST," +
            " AllTotalAmount, GrandTotal, TotalInWord, Description, Hsn, TaxPercentage, Quntity, Unit, Rate, DiscountPercentage," +
            " Total,CreatedOn FROM tbl_Invoice_both_hdr INNER JOIN tbl_Invoice_both_Dtls ON tbl_Invoice_both_Dtls.InvoiceId = tbl_Invoice_both_hdr.Id WHERE tbl_Invoice_both_hdr.Id='" + ID + "'", Conn);
        da.Fill(Dt);
        GvPurchaseOrderList.DataSource = Dt;
        GvPurchaseOrderList.DataBind();

        StringWriter sw = new StringWriter();
        StringReader sr = new StringReader(sw.ToString());

        Document doc = new Document(PageSize.A4, 10f, 10f, 55f, 0f);


        PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(Server.MapPath("~/Files/") + "TaxInvoice.pdf", FileMode.Create));

        iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, sr);

        doc.Open();

        string imageURL = Server.MapPath("~") + "/image/AA.png";


        iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(imageURL);
        png.ScaleToFit(70, 100);
        png.SetAbsolutePosition(40, 745);
        png.SpacingBefore = 50f;
        png.SpacingAfter = 1f;
        png.Alignment = Element.ALIGN_LEFT;
        doc.Add(png);

        PdfContentByte cb = writer.DirectContent;
        cb.Rectangle(17f, 735f, 560f, 60f);
        cb.Stroke();
        // Header 
        cb.BeginText();
        cb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 20);
        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "ENDEAVOUR AUTOMATION", 180, 773, 0);
        cb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 11);
        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Survey No. 27, Nilambkar Nagar, Near Raghunandan Karyalay,", 145, 755, 0);
        cb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 11);
        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Tathawade, Pune-411033", 227, 740, 0);
        cb.EndText();

        PdfContentByte cbb = writer.DirectContent;
        cbb.Rectangle(17f, 710f, 560f, 25f);
        cbb.Stroke();
        // Header 
        cbb.BeginText();
        cbb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
        cbb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " Mob: 9860502108    Email: endeavour.automations@gmail.com ", 153, 722, 0);
        cbb.EndText();

        PdfContentByte cbbb = writer.DirectContent;
        cbbb.Rectangle(17f, 685f, 560f, 25f);
        cbbb.Stroke();
        // Header 
        cbbb.BeginText();
        cbbb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
        cbbb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "GSTIN :  27AFYPJ3488G1ZQ ", 30, 695, 0);
        cbbb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
        cbbb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "W.E.F. :  01/07/2017", 185, 695, 0);
        cbbb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
        cbbb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "PAN No. :  AFYPJ3488G", 310, 695, 0);
        cbbb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
        cbbb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "State Code :  27 Maharashtra", 440, 695, 0);
        cbbb.EndText();

        PdfContentByte cd = writer.DirectContent;
        cd.Rectangle(17f, 660f, 560f, 25f);
        cd.Stroke();
        // Header 
        cd.BeginText();
        cd.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 17);
        cd.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Tax Invoice", 260, 667, 0);
        cd.EndText();


        if (Dt.Rows.Count > 0)
        {
            var CreateDate = DateTime.Now.ToString("yyyy-MM-dd");
            string InvoiceNo = Dt.Rows[0]["InvoiceNo"].ToString();
            string PoNumber = Dt.Rows[0]["PoNo"].ToString();
            string Address = Dt.Rows[0]["CustomerShippingAddress"].ToString();
            string InvoiceDate = Dt.Rows[0]["InvoiceDate"].ToString().TrimEnd("0:0".ToCharArray());
            string ChallanNo = Dt.Rows[0]["ChallanNo"].ToString();
            string GSTNo = Dt.Rows[0]["CustomerGstNo"].ToString();
            string KindAtt = Dt.Rows[0]["KindAtt"].ToString();
            string TotalInWord = Dt.Rows[0]["TotalInWord"].ToString();
            string GrandTotal = Dt.Rows[0]["GrandTotal"].ToString();
            string CGST = Dt.Rows[0]["CGST"].ToString();
            string SGST = Dt.Rows[0]["SGST"].ToString();
            string Total = Dt.Rows[0]["AllTotalAmount"].ToString();

            Paragraph paragraphTable1 = new Paragraph();
            paragraphTable1.SpacingBefore = 120f;
            paragraphTable1.SpacingAfter = 10f;

            PdfPTable table = new PdfPTable(4);

            float[] widths2 = new float[] { 100, 180, 100, 180 };
            table.SetWidths(widths2);
            table.TotalWidth = 560f;
            table.LockedWidth = true;

            DateTime ffff1 = Convert.ToDateTime(Dt.Rows[0]["InvoiceDate"].ToString());
            string datee = ffff1.ToString("yyyy-MM-dd");


            table.AddCell(new Phrase("Invoice Number : ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(InvoiceNo, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("PO Number :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(PoNumber, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("Address", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(Address, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("Invoice Date :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(datee, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("Challan No :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(ChallanNo, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("GST No :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(GSTNo, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("Created Date :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(CreateDate, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("Kind Attn. :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(KindAtt, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            paragraphTable1.Add(table);
            doc.Add(paragraphTable1);

            Paragraph paragraphTable2 = new Paragraph();
            paragraphTable2.SpacingAfter = 0f;
            table = new PdfPTable(9);
            float[] widths3 = new float[] { 4f, 19f, 10f, 8f, 8f, 8f, 11f, 8f, 12f };
            table.SetWidths(widths3);

            double Ttotal_price = 0;
            if (Dt.Rows.Count > 0)
            {
                table.TotalWidth = 560f;
                table.LockedWidth = true;
                table.AddCell(new Phrase("SN.", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("Description", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("Hsn / Sac", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("Tax %", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("Quantity", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("Unit", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("Rate", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("Disc %", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("Total", FontFactory.GetFont("Arial", 10, Font.BOLD)));

                int rowid = 1;
                foreach (DataRow dr in Dt.Rows)
                {
                    table.TotalWidth = 560f;
                    table.LockedWidth = true;

                    double Ftotal = Convert.ToDouble(dr["Total"].ToString());
                    string _ftotal = Ftotal.ToString("##.00");
                    table.AddCell(new Phrase(rowid.ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(dr["Description"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(dr["Hsn"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(dr["TaxPercentage"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(dr["Quntity"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(dr["Unit"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(dr["Rate"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(dr["DiscountPercentage"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(_ftotal, FontFactory.GetFont("Arial", 9)));
                    rowid++;

                    Ttotal_price += Convert.ToDouble(dr["Total"].ToString());
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

            table = new PdfPTable(9);
            table.TotalWidth = 560f;
            table.LockedWidth = true;
            table.SetWidths(new float[] { 4f, 19f, 10f, 8f, 8f, 8f, 11f, 8f, 12f });
            table.AddCell(paragraph);
            table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            //table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            //table.AddCell(new Phrase("  \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            if (Dt.Rows.Count >= 10)
            {
                table.AddCell(new Phrase("  \n\n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));

            }
            else if (Dt.Rows.Count >= 7 && Dt.Rows.Count <= 9)
            {
                table.AddCell(new Phrase("  \n\n\n\n\n\n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));

            }
            else if (Dt.Rows.Count >= 4 && Dt.Rows.Count <= 6)
            {
                table.AddCell(new Phrase("  \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));

            }
            else if (Dt.Rows.Count < 4)
            {

                table.AddCell(new Phrase("  \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));

            }
            doc.Add(table);

            //Add Total Row start
            Paragraph paragraphTable5 = new Paragraph();



            string[] itemsss = { "Goods once sold will not be taken back or exchange. \b",
                        "Interest at the rate of 18% will be charged if bill is'nt paid within 30 days.\b",
                        "Our risk and responsibility ceases the moment goods leaves out godown. \n",
                        };

            Font font13 = FontFactory.GetFont("Arial", 12, Font.BOLD);
            Font font11 = FontFactory.GetFont("Arial", 10, Font.BOLD);
            Paragraph paragraphh = new Paragraph("", font12);



            for (int i = 0; i < items.Length; i++)
            {
                paragraph.Add(new Phrase("", font10));
            }

            table = new PdfPTable(3);
            table.TotalWidth = 560f;
            table.LockedWidth = true;

            paragraph.Alignment = Element.ALIGN_RIGHT;

            table.SetWidths(new float[] { 0f, 76f, 12f });
            //table.AddCell(paragraph);
            //PdfPCell cell = new PdfPCell(new Phrase("Sub Total", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //table.AddCell(cell);
            //PdfPCell cell11 = new PdfPCell(new Phrase(Total, FontFactory.GetFont("Arial", 10, Font.BOLD)));
            //cell11.HorizontalAlignment = Element.ALIGN_RIGHT;
            //table.AddCell(cell11);


            doc.Add(table);
            //add total row end

            //CGST 9% Row STart
            Paragraph paragraphTable15 = new Paragraph();
            paragraphTable5.SpacingAfter = 0f;


            string[] itemss = { "Goods once sold will not be taken back or exchange. \b",
                        "Interest at the rate of 18% will be charged if bill is'nt paid within 30 days.\b",
                        "Our risk and responsibility ceases the moment goods leaves out godown. \n",
                        };

            Font font1 = FontFactory.GetFont("Arial", 12, Font.BOLD);
            Font font2 = FontFactory.GetFont("Arial", 10, Font.BOLD);
            Paragraph paragraphhh = new Paragraph("", font12);



            for (int i = 0; i < items.Length; i++)
            {
                paragraph.Add(new Phrase("", font10));
            }

            table = new PdfPTable(3);
            table.TotalWidth = 560f;
            table.LockedWidth = true;

            table.SetWidths(new float[] { 0f, 76f, 12f });
            table.AddCell(paragraph);
            PdfPCell cell2 = new PdfPCell(new Phrase("CGST %", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            cell2.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cell2);
            PdfPCell cell3 = new PdfPCell(new Phrase(CGST, FontFactory.GetFont("Arial", 10, Font.BOLD)));
            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cell3);

            doc.Add(table);
            //CGST 9% Row End

            //SGST 9% Row STart
            Paragraph paragraphTable16 = new Paragraph();
            paragraphTable5.SpacingAfter = 10f;


            string[] item = { "Goods once sold will not be taken back or exchange. \b",
                        "Interest at the rate of 18% will be charged if bill is'nt paid within 30 days.\b",
                        "Our risk and responsibility ceases the moment goods leaves out godown. \n",
                        };

            Font font14 = FontFactory.GetFont("Arial", 12, Font.BOLD);
            Font font15 = FontFactory.GetFont("Arial", 10, Font.BOLD);
            Paragraph paragraphhhh = new Paragraph("", font12);



            for (int i = 0; i < items.Length; i++)
            {
                paragraph.Add(new Phrase("", font10));
            }

            table = new PdfPTable(3);
            table.TotalWidth = 560f;
            table.LockedWidth = true;

            table.SetWidths(new float[] { 0f, 76f, 12f });
            table.AddCell(paragraph);
            PdfPCell cell22 = new PdfPCell(new Phrase("SGST %", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            cell22.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cell22);
            PdfPCell cell33 = new PdfPCell(new Phrase(SGST, FontFactory.GetFont("Arial", 10, Font.BOLD)));
            cell33.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cell33);


            doc.Add(table);
            //SGST 9% Row End

            //Grand total Row STart
            Paragraph paragraphTable17 = new Paragraph();
            paragraphTable5.SpacingAfter = 10f;

            string[] itemm = { "Goods once sold will not be taken back or exchange. \b",
                        "Interest at the rate of 18% will be charged if bill is'nt paid within 30 days.\b",
                        "Our risk and responsibility ceases the moment goods leaves out godown. \n",
                        };

            Font font16 = FontFactory.GetFont("Arial", 12, Font.BOLD);
            Font font17 = FontFactory.GetFont("Arial", 10, Font.BOLD);
            Paragraph paragraphhhhh = new Paragraph("", font12);

            //paragraphh.SpacingAfter = 10f;

            for (int i = 0; i < items.Length; i++)
            {
                paragraph.Add(new Phrase("", font10));
            }

            table = new PdfPTable(3);
            table.TotalWidth = 560f;
            table.LockedWidth = true;

            //var Grndttl = Convert.ToDecimal(Ttotal_price) + Convert.ToDecimal(Cgst_9) + Convert.ToDecimal(Sgst_9);

            table.SetWidths(new float[] { 0f, 76f, 12f });
            table.AddCell(paragraph);
            PdfPCell cell44 = new PdfPCell(new Phrase("Grand Total", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            cell44.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cell44);
            PdfPCell cell55 = new PdfPCell(new Phrase(Total, FontFactory.GetFont("Arial", 10, Font.BOLD)));
            cell55.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cell55);


            doc.Add(table);
            //Grand total Row End

            //Grand total in word Row STart
            Paragraph paragraphTable18 = new Paragraph();
            paragraphTable18.SpacingAfter = 50f;

            string[] itemmm = { "Goods once sold will not be taken back or exchange. \b",
                        "Interest at the rate of 18% will be charged if bill is'nt paid within 30 days.\b",
                        "Our risk and responsibility ceases the moment goods leaves out godown. \n",
                        };

            Font font18 = FontFactory.GetFont("Arial", 12, Font.BOLD);
            Font font19 = FontFactory.GetFont("Arial", 10, Font.BOLD);
            Paragraph paragraphhmhhh = new Paragraph("", font12);



            for (int i = 0; i < items.Length; i++)
            {
                paragraph.Add(new Phrase("", font10));
            }

            table = new PdfPTable(3);
            table.TotalWidth = 560f;
            table.LockedWidth = true;

            table.SetWidths(new float[] { 0f, 25f, 63f });
            table.AddCell(paragraph);
            PdfPCell cell66 = new PdfPCell(new Phrase("Amount In Words Rs. ", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            cell66.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell66);
            PdfPCell cell77 = new PdfPCell(new Phrase(TotalInWord, FontFactory.GetFont("Arial", 10, Font.BOLD)));
            cell77.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell77);
            doc.Add(table);


            PdfContentByte cn = writer.DirectContent;
            cn.Rectangle(17f, 160f, 560f, 115f);
            cn.Stroke();
            // Header 
            cn.BeginText();
            cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 14);
            cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Bank Details :", 30, 258, 0);
            cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibri.ttf", "Identity-H", BaseFont.EMBEDDED), 12);
            cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Bank Name : BANK OF BARODA", 30, 240, 0);
            cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibri.ttf", "Identity-H", BaseFont.EMBEDDED), 12);
            cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Account Name : ENDEAVOUR AUTOMATION", 30, 225, 0);
            cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibri.ttf", "Identity-H", BaseFont.EMBEDDED), 12);
            cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Branch : KALEWADI", 30, 210, 0);
            cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibri.ttf", "Identity-H", BaseFont.EMBEDDED), 12);
            cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "A/c No : 46180200000214", 30, 195, 0);
            cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibri.ttf", "Identity-H", BaseFont.EMBEDDED), 12);
            cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "IFSC/Neft Code :BARB0KALEWA", 30, 180, 0);

            //cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibrii.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
            //cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "We Thank you for your enquiry", 440, 256, 0);
            cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 12);
            cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "For", 392, 230, 0);
            cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 13);
            cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "ENDEAVOUR AUTOMATION", 413, 230, 0);
            cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 13);
            cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Authorised Signatory", 443, 180, 0);
            cn.EndText();


            Paragraph paragraphTable4 = new Paragraph();

            paragraphTable4.SpacingBefore = 10f;

            table = new PdfPTable(2);
            table.TotalWidth = 560f;

            float[] widths = new float[] { 160f, 400f };
            table.SetWidths(widths);
            table.LockedWidth = true;

            doc.Close();

            Byte[] FileBuffer = File.ReadAllBytes(Server.MapPath("~/Files/") + "TaxInvoice.pdf");
            Font blackFont = FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK);
            using (MemoryStream stream = new MemoryStream())
            {
                PdfReader reader = new PdfReader(FileBuffer);
                using (PdfStamper stamper = new PdfStamper(reader, stream))
                {
                    int pages = reader.NumberOfPages;
                    for (int i = 1; i <= pages; i++)
                    {
                        if (i == 1)
                        {

                        }
                        else
                        {
                            var pdfbyte = stamper.GetOverContent(i);
                            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imageURL);
                            image.ScaleToFit(70, 100);
                            image.SetAbsolutePosition(40, 792);
                            image.SpacingBefore = 50f;
                            image.SpacingAfter = 1f;
                            image.Alignment = Element.ALIGN_LEFT;
                            pdfbyte.AddImage(image);
                        }
                        var PageName = "Page No " + i.ToString();
                        ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_RIGHT, new Phrase(PageName, blackFont), 568f, 15f, 0);
                    }
                }
                FileBuffer = stream.ToArray();
            }
            //File.WriteAllBytes(@"D:\PDFs\Test_1.pdf", FileBuffer);
            string empFilename = "TaxInvoice" + DateTime.Now.ToShortDateString() + ".pdf";

            if (FileBuffer != null)
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-length", FileBuffer.Length.ToString());
                Response.BinaryWrite(FileBuffer);
                Response.AddHeader("Content-Disposition", "attachment;filename=" + empFilename);
            }

        }
        doc.Close();

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
                com.CommandText = "select DISTINCT JobNo from tblTestingProduct where " + "JobNo like @Search + '%' AND isCompleted='1'  ";

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
    public static List<string> GetCustomerList(string prefixText, int count)
    {
        return AutoFillCUSTOMERlist(prefixText);
    }
    public static List<string> AutoFillCUSTOMERlist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT CompName from tbl_Invoice_both_hdr where " + "CompName like @Search + '%'";

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

    protected void btn_search_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txt_Invoice_search.Text) && string.IsNullOrEmpty(txtJobno.Text) && string.IsNullOrEmpty(txt_form_podate_search.Text))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Please Enter Invoice No  !!!');", true);
            Load_Record();
        }
        else
        {
            if (!string.IsNullOrEmpty(txt_Invoice_search.Text) && string.IsNullOrEmpty(txtJobno.Text) && string.IsNullOrEmpty(txt_form_podate_search.Text))
            {
                ViewState["Excell"] = "GetsortedInvoiceno";
                GetsortedInvoiceno();
                //DataTable Dt = new DataTable();
                //SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_Invoice_both_hdr WHERE InvoiceNo = '" + txt_Invoice_search.Text + "' AND Is_Deleted='0' ", Conn);
                //da.Fill(Dt);
                //GvPurchaseOrderList.DataSource = Dt;
                //GvPurchaseOrderList.DataBind();
                //GvPurchaseOrderList.EmptyDataText = "Not Records Found";
            }
            else if (string.IsNullOrEmpty(txt_Invoice_search.Text) && !string.IsNullOrEmpty(txtJobno.Text) && string.IsNullOrEmpty(txt_form_podate_search.Text))
            {
                ViewState["Excell"] = "Getsortedjobno";
                Getsortedjobno();
                //DataTable Dt = new DataTable();
                //SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_Invoice_both_hdr WHERE CompName LIKE '%" + txtJobno.Text + "%' AND Is_Deleted='0' ", Conn);
                //da.Fill(Dt);
                //GvPurchaseOrderList.DataSource = Dt;
                //GvPurchaseOrderList.DataBind();
                //GvPurchaseOrderList.EmptyDataText = "Not Records Found";
            }
            else if (string.IsNullOrEmpty(txt_Invoice_search.Text) && !string.IsNullOrEmpty(txtJobno.Text) && !string.IsNullOrEmpty(txt_form_podate_search.Text))
            {
                ViewState["Excell"] = "GetDatewisecutomer";
                GetDatewisecutomer();
                //GetsortedDatewise();
                //DateTime date = Convert.ToDateTime(txt_form_podate_search.Text.ToString());

                //DataTable Dt = new DataTable();
                //txt_form_podate_search.Text = date.ToString("yyyy-MM-dd");

                //SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_Invoice_both_hdr WHERE InvoiceDate= '" + txt_form_podate_search.Text + "' AND Is_Deleted='0' ", Conn);
                //da.Fill(Dt);
                //GvPurchaseOrderList.DataSource = Dt;
                //GvPurchaseOrderList.DataBind();
                //GvPurchaseOrderList.EmptyDataText = "Not Records Found";
            }
            else if (!string.IsNullOrEmpty(txt_Invoice_search.Text) && !string.IsNullOrEmpty(txtJobno.Text) && !string.IsNullOrEmpty(txt_form_podate_search.Text))
            {
                ViewState["Excell"] = "GetsortedIJPD";
                GetsortedIJPD();
                //DateTime date = Convert.ToDateTime(txt_form_podate_search.Text.ToString());
                //txt_form_podate_search.Text = date.ToString("yyyy-MM-dd");

                //DataTable Dt = new DataTable();
                //SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_Invoice_both_hdr WHERE InvoiceNo LIKE '%" + txt_Invoice_search.Text + "%'AND CompName LIKE '%" + txtJobno.Text + "%' AND  InvoiceDate = '" + txt_form_podate_search.Text + "' AND Is_Deleted='0' ", Conn);
                //da.Fill(Dt);
                //GvPurchaseOrderList.DataSource = Dt;
                //GvPurchaseOrderList.DataBind();
                //GvPurchaseOrderList.EmptyDataText = "Not Records Found";
            }
            else if (!string.IsNullOrEmpty(txt_Invoice_search.Text) && !string.IsNullOrEmpty(txtJobno.Text))
            {

                ViewState["Excell"] = "GetsortedInvoicejob";
                GetsortedInvoicejob();
                //DataTable Dt = new DataTable();
                //SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_Invoice_both_hdr WHERE InvoiceNo LIKE '%" + txt_Invoice_search.Text + "%' AND CompName LIKE '%" + txtJobno.Text + "%' AND Is_Deleted='0' ", Conn);
                //da.Fill(Dt);
                //GvPurchaseOrderList.DataSource = Dt;
                //GvPurchaseOrderList.DataBind();
                //GvPurchaseOrderList.EmptyDataText = "Not Records Found";
            }
            else if (!string.IsNullOrEmpty(txt_Invoice_search.Text) && !string.IsNullOrEmpty(txt_form_podate_search.Text))
            {
                ViewState["Excell"] = "GetsortedInvoiceDatewise";
                GetsortedInvoiceDatewise();
                //DateTime date = Convert.ToDateTime(txt_form_podate_search.Text.ToString());
                //txt_form_podate_search.Text = date.ToString("yyyy-MM-dd");

                //DataTable Dt = new DataTable();
                //SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_Invoice_both_hdr WHERE InvoiceNo LIKE '%" + txt_Invoice_search.Text + "%' AND  InvoiceDate = '" + txt_form_podate_search.Text + "' AND Is_Deleted='0' ", Conn);
                //da.Fill(Dt);
                //GvPurchaseOrderList.DataSource = Dt;
                //GvPurchaseOrderList.DataBind();
                //GvPurchaseOrderList.EmptyDataText = "Not Records Found";
            }
            else if (!string.IsNullOrEmpty(txtJobno.Text) && !string.IsNullOrEmpty(txt_form_podate_search.Text))
            {
                ViewState["Excell"] = "GetSortedjobnodatewise";
                GetSortedjobnodatewise();
                //DateTime date = Convert.ToDateTime(txt_form_podate_search.Text.ToString());
                //txt_form_podate_search.Text = date.ToString("yyyy-MM-dd");

                //DataTable Dt = new DataTable();
                //SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_Invoice_both_hdr WHERE CompName LIKE '%" + txtJobno.Text + "%' AND   InvoiceDate = '" + txt_form_podate_search.Text + "' AND Is_Deleted='0' ", Conn);
                //da.Fill(Dt);
                //GvPurchaseOrderList.DataSource = Dt;
                //GvPurchaseOrderList.DataBind();
            }

            //From date to date filter
            else if (!string.IsNullOrEmpty(txt_form_podate_search.Text) && !string.IsNullOrEmpty(txt_to_podate_search.Text))
            {
                ViewState["Excell"] = "GetsortedPODatewise";
                GetsortedPODatewise();
                //DateTime date = Convert.ToDateTime(txt_form_podate_search.Text.ToString());
                //txt_form_podate_search.Text = date.ToString("yyyy-MM-dd");

                //DateTime datee = Convert.ToDateTime(txt_to_podate_search.Text.ToString());
                //txt_to_podate_search.Text = datee.ToString("yyyy-MM-dd");

                //DataTable Dt = new DataTable();
                //SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_Invoice_both_hdr WHERE  InvoiceDate between '" + txt_form_podate_search.Text + "'  AND '" + txt_to_podate_search.Text + "' AND Is_Deleted='0' ", Conn);
                //da.Fill(Dt);
                //GvPurchaseOrderList.DataSource = Dt;
                //GvPurchaseOrderList.DataBind();
                //GvPurchaseOrderList.EmptyDataText = "Not Records Found";
            }
            else if (!string.IsNullOrEmpty(txt_form_podate_search.Text) && string.IsNullOrEmpty(txt_to_podate_search.Text))
            {
                ViewState["Excell"] = "GetsortedInvoicedatewise";
                GetsortedInvoicedatewise();
                //DateTime date = Convert.ToDateTime(txt_form_podate_search.Text.ToString());
                //txt_form_podate_search.Text = date.ToString("yyyy-MM-dd");


                //DataTable Dt = new DataTable();
                //SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_Invoice_both_hdr WHERE  InvoiceDate= '" + txt_form_podate_search.Text + "'   AND Is_Deleted='0' ", Conn);
                //da.Fill(Dt);
                //GvPurchaseOrderList.DataSource = Dt;
                //GvPurchaseOrderList.DataBind();
                //GvPurchaseOrderList.EmptyDataText = "Not Records Found";
            }
        }
    }

    protected void ddlservicetype_TextChanged(object sender, EventArgs e)
    {
        GvPurchaseOrderList.Visible = false;
        DataTable dtt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM [tbl_Invoice_both_hdr] where  ServiceType ='" + ddlservicetype.Text + "' AND Is_Deleted = '0'", Conn);
        //SqlDataAdapter sad = new SqlDataAdapter("select  Id,CustomerName,SubCustomer,JobNo,Pono,PoDate,RefNo,Mobileno,Quotationno,CreatedBy,CreatedOn,DATEDIFF(DAY, PoDate, getdate()) AS days from CustomerPO_Hdr where  ServiceType ='" + ddlservicetype.Text + "' AND Is_Deleted = '0'", con);

        sad.Fill(dtt);
        Gvsorted.EmptyDataText = "Records Not Found";
        Gvsorted.DataSource = dtt;
        Gvsorted.DataBind();
    }

    protected void btn_refresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("TaxInvoiceList.aspx");
    }

    protected void btncreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("TaxInvoice.aspx");
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
                com.CommandText = "select DISTINCT InvoiceNo from tbl_Invoice_both_hdr where " + "InvoiceNo like @Search + '%' AND Is_Deleted='0' ";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> InvoiceNo = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        InvoiceNo.Add(sdr["InvoiceNo"].ToString());
                    }
                }
                con.Close();
                return InvoiceNo;
            }

        }
    }
    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetponoList(string prefixText, int count)
    {
        return AutoFillponolist(prefixText);
    }

    public static List<string> AutoFillponolist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT PoNo from tbl_Invoice_both_hdr where " + "PoNo like @Search + '%' AND Is_Deleted='0' ";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> PoNo = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        PoNo.Add(sdr["PoNo"].ToString());
                    }
                }
                con.Close();
                return PoNo;
            }

        }
    }

    protected void GvPurchaseOrderList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvPurchaseOrderList.PageIndex = e.NewPageIndex;
        Load_Record();

    }
    //sorted Grid start
    protected void Gvsorted_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

        if (ViewState["Record"].ToString() == "Invoice")
        {
            Gvsorted.PageIndex = e.NewPageIndex;
            GetsortedInvoicenoGrid();
        }
        if (ViewState["Record"].ToString() == "Job")
        {
            Gvsorted.PageIndex = e.NewPageIndex;
            Getsortedjobnogrid();
        }

        if (ViewState["Record"].ToString() == "Date")
        {
            Gvsorted.PageIndex = e.NewPageIndex;
            GetsortedDatewisegrid();
        }

        if (ViewState["Record"].ToString() == "IJPD")
        {
            Gvsorted.PageIndex = e.NewPageIndex;
            GetsortedIJPDGrid();
        }

        if (ViewState["Record"].ToString() == "Inoce&Job")
        {
            Gvsorted.PageIndex = e.NewPageIndex;
            GetsortedInvoicejobgrid();
        }

        if (ViewState["Record"].ToString() == "Inoce&Date")
        {
            Gvsorted.PageIndex = e.NewPageIndex;
            GetsortedInvoiceDatewisegrid();
        }
        if (ViewState["Record"].ToString() == "Job&Date")
        {
            Gvsorted.PageIndex = e.NewPageIndex;
            GetSortedjobnodatewiseGird();
        }

        if (ViewState["Record"].ToString() == "PODate")
        {
            Gvsorted.PageIndex = e.NewPageIndex;
            GetsortedPODatewisegrid();
        }

        if (ViewState["Record"].ToString() == "InvoiceDate")
        {
            Gvsorted.PageIndex = e.NewPageIndex;
            GetsortedInvoicedatewisegrid();
        }
    }
    public void GetsortedInvoiceno()
    {
        GvPurchaseOrderList.Visible = false;
        ViewState["Record"] = "Invoice";
        DataTable Dt = new DataTable();
        //SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_Invoice_both_hdr WHERE InvoiceNo = '" + txt_Invoice_search.Text + "' AND Is_Deleted='0' ", Conn);
        SqlDataAdapter da = new SqlDataAdapter(@"WITH DistinctInvoices AS (
    SELECT DISTINCT
        IH.Id,
        IH.InvoiceNo,
        IH.PoNo,
        IH.CompName,
        IH.GrandTotal,
        IH.ChallanNo,
        IH.PayTerm,
        IH.InvoiceDate,
        IH.CreatedBy,
        IH.CreatedOn,
        IH.CGST,
        IH.SGST,
        IH.IGST,
        IH.Is_Deleted
    FROM
        tbl_Invoice_both_hdr AS IH
    WHERE
        IH.Is_Deleted = '0'
)
SELECT
    DI.Id,
    DI.InvoiceNo,
    DI.PoNo,
    DI.CompName,
    DI.GrandTotal,
    DI.ChallanNo,
    DI.PayTerm,
    DI.InvoiceDate,
    DI.CreatedBy,
    DI.CreatedOn,
    DI.CGST,
    DI.SGST,
    DI.IGST,
    DI.Is_Deleted,
    (
        SELECT STUFF((
            SELECT ',' + CAST(dtls.JobNo AS VARCHAR(10))
            FROM tbl_Invoice_both_Dtls AS dtls
            WHERE dtls.InvoiceNo = DI.InvoiceNo
            FOR XML PATH(''), TYPE
        ).value('.', 'VARCHAR(MAX)'), 1, 1, '')
    ) AS JobNo
FROM
    DistinctInvoices AS DI
WHERE
    DI.InvoiceNo = '" + txt_Invoice_search.Text + "' AND DI.Is_Deleted = '0' ORDER BY DI.CreatedOn DESC; ", Conn);



        da.Fill(Dt);
        Gvsorted.DataSource = Dt;
        Gvsorted.DataBind();
        Gvsorted.EmptyDataText = "Not Records Found";
    }
    public void GetsortedInvoicenoGrid()
    {
        GvPurchaseOrderList.Visible = false;
        DataTable Dt = new DataTable();
        //SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_Invoice_both_hdr WHERE InvoiceNo = '" + txt_Invoice_search.Text + "' AND Is_Deleted='0' ", Conn);
        SqlDataAdapter da = new SqlDataAdapter(@"WITH DistinctInvoices AS (
    SELECT DISTINCT
        IH.Id,
        IH.InvoiceNo,
        IH.PoNo,
        IH.CompName,
        IH.ChallanNo,
        IH.PayTerm,
        IH.InvoiceDate,
        IH.CreatedBy,
        IH.CreatedOn,
        IH.CGST,
        IH.SGST,
        IH.IGST,
        IH.Is_Deleted
    FROM
        tbl_Invoice_both_hdr AS IH
    WHERE
        IH.Is_Deleted = '0'
)
SELECT
    DI.Id,
    DI.InvoiceNo,
    DI.PoNo,
    DI.CompName,
    DI.ChallanNo,
    DI.PayTerm,
    DI.InvoiceDate,
    DI.CreatedBy,
    DI.CreatedOn,
    DI.CGST,
    DI.SGST,
    DI.IGST,
    DI.Is_Deleted,
    (
        SELECT STUFF((
            SELECT ',' + CAST(dtls.JobNo AS VARCHAR(10))
            FROM tbl_Invoice_both_Dtls AS dtls
            WHERE dtls.InvoiceNo = DI.InvoiceNo
            FOR XML PATH(''), TYPE
        ).value('.', 'VARCHAR(MAX)'), 1, 1, '')
    ) AS JobNo
FROM
    DistinctInvoices AS DI
WHERE
    DI.InvoiceNo = '" + txt_Invoice_search.Text + "' AND DI.Is_Deleted = '0' ORDER BY DI.CreatedOn DESC; ", Conn);

        da.Fill(Dt);
        Gvsorted.DataSource = Dt;
        Gvsorted.DataBind();
        Gvsorted.EmptyDataText = "Not Records Found";
    }
    public void Getsortedjobno()
    {

        GvPurchaseOrderList.Visible = false;
        ViewState["Record"] = "Job";
        DataTable Dt = new DataTable();
        // SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_Invoice_both_hdr WHERE CompName LIKE '%" + txtJobno.Text + "%' AND Is_Deleted='0' ", Conn);
        SqlDataAdapter da = new SqlDataAdapter(@"WITH DistinctInvoices AS (
    SELECT DISTINCT
        IH.Id,
        IH.InvoiceNo,
        IH.PoNo,
        IH.CompName,
        IH.GrandTotal,
        IH.ChallanNo,
        IH.PayTerm,
        IH.InvoiceDate,
        IH.CreatedBy,
        IH.CreatedOn,
        IH.CGST,
        IH.SGST,
        IH.IGST,
        IH.Is_Deleted
    FROM
        tbl_Invoice_both_hdr AS IH
    WHERE
        IH.Is_Deleted = '0'
)
SELECT
    DI.Id,
    DI.InvoiceNo,
    DI.PoNo,
    DI.CompName,
    DI.GrandTotal,
    DI.ChallanNo,
    DI.PayTerm,
    DI.InvoiceDate,
    DI.CreatedBy,
    DI.CreatedOn,
    DI.CGST,
    DI.SGST,
    DI.IGST,
    DI.Is_Deleted,
    (
        SELECT STUFF((
            SELECT ',' + CAST(dtls.JobNo AS VARCHAR(10))
            FROM tbl_Invoice_both_Dtls AS dtls
            WHERE dtls.InvoiceNo = DI.InvoiceNo
            FOR XML PATH(''), TYPE
        ).value('.', 'VARCHAR(MAX)'), 1, 1, '')
    ) AS JobNo
FROM
    DistinctInvoices AS DI
WHERE
    DI.CompName LIKE '%" + txtJobno.Text + "%'  AND DI.Is_Deleted = '0' ORDER BY DI.CreatedOn DESC; ", Conn);

        da.Fill(Dt);
        Gvsorted.DataSource = Dt;
        Gvsorted.DataBind();
        Gvsorted.EmptyDataText = "Not Records Found";
    }
    public void Getsortedjobnogrid()
    {

        GvPurchaseOrderList.Visible = false;
        DataTable Dt = new DataTable();
        //SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_Invoice_both_hdr WHERE CompName LIKE '%" + txtJobno.Text + "%' AND Is_Deleted='0' ", Conn);
        SqlDataAdapter da = new SqlDataAdapter(@"WITH DistinctInvoices AS (
    SELECT DISTINCT
        IH.Id,
        IH.InvoiceNo,
        IH.PoNo,
        IH.CompName,
        IH.ChallanNo,
        IH.PayTerm,
        IH.InvoiceDate,
        IH.CreatedBy,
        IH.CreatedOn,
        IH.CGST,
        IH.SGST,
        IH.IGST,
        IH.Is_Deleted
    FROM
        tbl_Invoice_both_hdr AS IH
    WHERE
        IH.Is_Deleted = '0'
)
SELECT
    DI.Id,
    DI.InvoiceNo,
    DI.PoNo,
    DI.CompName,
    DI.ChallanNo,
    DI.PayTerm,
    DI.InvoiceDate,
    DI.CreatedBy,
    DI.CreatedOn,
    DI.CGST,
    DI.SGST,
    DI.IGST,
    DI.Is_Deleted,
    (
        SELECT STUFF((
            SELECT ',' + CAST(dtls.JobNo AS VARCHAR(10))
            FROM tbl_Invoice_both_Dtls AS dtls
            WHERE dtls.InvoiceNo = DI.InvoiceNo
            FOR XML PATH(''), TYPE
        ).value('.', 'VARCHAR(MAX)'), 1, 1, '')
    ) AS JobNo
FROM
    DistinctInvoices AS DI
WHERE
    DI.CompName LIKE '%" + txtJobno.Text + "%'  AND DI.Is_Deleted = '0' ORDER BY DI.CreatedOn DESC; ", Conn);




        da.Fill(Dt);
        Gvsorted.DataSource = Dt;
        Gvsorted.DataBind();
        Gvsorted.EmptyDataText = "Not Records Found";
    }
    public void GetsortedDatewise()
    {
        GvPurchaseOrderList.Visible = false;
        ViewState["Record"] = "Date";
        DateTime date = Convert.ToDateTime(txt_form_podate_search.Text.ToString());
        DataTable Dt = new DataTable();
        txt_form_podate_search.Text = date.ToString("yyyy-MM-dd");
        //SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_Invoice_both_hdr WHERE InvoiceDate= '" + txt_form_podate_search.Text + "' AND Is_Deleted='0' ", Conn);
        SqlDataAdapter da = new SqlDataAdapter(@"WITH DistinctInvoices AS (
    SELECT DISTINCT
        IH.Id,
        IH.InvoiceNo,
        IH.PoNo,
        IH.CompName,
        IH.ChallanNo,
        IH.PayTerm,
        IH.InvoiceDate,
        IH.CreatedBy,
        IH.CreatedOn,
        IH.CGST,
        IH.SGST,
        IH.IGST,
        IH.Is_Deleted
    FROM
        tbl_Invoice_both_hdr AS IH
    WHERE
        IH.Is_Deleted = '0'
)
SELECT
    DI.Id,
    DI.InvoiceNo,
    DI.PoNo,
    DI.CompName,
    DI.ChallanNo,
    DI.PayTerm,
    DI.InvoiceDate,
    DI.CreatedBy,
    DI.CreatedOn,
    DI.CGST,
    DI.SGST,
    DI.IGST,
    DI.Is_Deleted,
    (
        SELECT STUFF((
            SELECT ',' + CAST(dtls.JobNo AS VARCHAR(10))
            FROM tbl_Invoice_both_Dtls AS dtls
            WHERE dtls.InvoiceNo = DI.InvoiceNo
            FOR XML PATH(''), TYPE
        ).value('.', 'VARCHAR(MAX)'), 1, 1, '')
    ) AS JobNo
FROM
    DistinctInvoices AS DI
WHERE
    DI.InvoiceDate= '" + txt_form_podate_search.Text + "'  AND DI.Is_Deleted = '0' ORDER BY DI.CreatedOn DESC; ", Conn);



        da.Fill(Dt);
        Gvsorted.DataSource = Dt;
        Gvsorted.DataBind();
        Gvsorted.EmptyDataText = "Not Records Found";
    }
    public void GetsortedDatewisegrid()
    {

        GvPurchaseOrderList.Visible = false;
        DateTime date = Convert.ToDateTime(txt_form_podate_search.Text.ToString());
        DataTable Dt = new DataTable();
        txt_form_podate_search.Text = date.ToString("yyyy-MM-dd");
        //SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_Invoice_both_hdr WHERE InvoiceDate= '" + txt_form_podate_search.Text + "' AND Is_Deleted='0' ", Conn);
        SqlDataAdapter da = new SqlDataAdapter(@"WITH DistinctInvoices AS (
    SELECT DISTINCT
        IH.Id,
        IH.InvoiceNo,
        IH.PoNo,
        IH.CompName,
        IH.ChallanNo,
        IH.PayTerm,
        IH.InvoiceDate,
        IH.CreatedBy,
        IH.CreatedOn,
        IH.CGST,
        IH.SGST,
        IH.IGST,
        IH.Is_Deleted
    FROM
        tbl_Invoice_both_hdr AS IH
    WHERE
        IH.Is_Deleted = '0'
)
SELECT
    DI.Id,
    DI.InvoiceNo,
    DI.PoNo,
    DI.CompName,
    DI.ChallanNo,
    DI.PayTerm,
    DI.InvoiceDate,
    DI.CreatedBy,
    DI.CreatedOn,
    DI.CGST,
    DI.SGST,
    DI.IGST,
    DI.Is_Deleted,
    (
        SELECT STUFF((
            SELECT ',' + CAST(dtls.JobNo AS VARCHAR(10))
            FROM tbl_Invoice_both_Dtls AS dtls
            WHERE dtls.InvoiceNo = DI.InvoiceNo
            FOR XML PATH(''), TYPE
        ).value('.', 'VARCHAR(MAX)'), 1, 1, '')
    ) AS JobNo
FROM
    DistinctInvoices AS DI
WHERE
    DI.InvoiceDate= '" + txt_form_podate_search.Text + "'  AND DI.Is_Deleted = '0' ORDER BY DI.CreatedOn DESC; ", Conn);

        da.Fill(Dt);
        Gvsorted.DataSource = Dt;
        Gvsorted.DataBind();
        Gvsorted.EmptyDataText = "Not Records Found";
    }
    public void GetsortedIJPD()
    {
        GvPurchaseOrderList.Visible = false;
        ViewState["Record"] = "IJPD";
        DateTime date = Convert.ToDateTime(txt_form_podate_search.Text.ToString());
        txt_form_podate_search.Text = date.ToString("yyyy-MM-dd");

        DataTable Dt = new DataTable();
        //SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_Invoice_both_hdr WHERE InvoiceNo LIKE '%" + txt_Invoice_search.Text + "%'AND CompName LIKE '%" + txtJobno.Text + "%' AND  InvoiceDate = '" + txt_form_podate_search.Text + "' AND Is_Deleted='0' ", Conn);
        SqlDataAdapter da = new SqlDataAdapter(@"WITH DistinctInvoices AS (
    SELECT DISTINCT
        IH.Id,
        IH.InvoiceNo,
        IH.PoNo,
        IH.CompName,
        IH.GrandTotal,
        IH.ChallanNo,
        IH.PayTerm,
        IH.InvoiceDate,
        IH.CreatedBy,
        IH.CreatedOn,
        IH.CGST,
        IH.SGST,
        IH.IGST,
        IH.Is_Deleted
    FROM
        tbl_Invoice_both_hdr AS IH
    WHERE
        IH.Is_Deleted = '0'
)
SELECT
    DI.Id,
    DI.InvoiceNo,
    DI.PoNo,
    DI.CompName,
    DI.GrandTotal,
    DI.ChallanNo,
    DI.PayTerm,
    DI.InvoiceDate,
    DI.CreatedBy,
    DI.CreatedOn,
    DI.CGST,
    DI.SGST,
    DI.IGST,
    DI.Is_Deleted,
    (
        SELECT STUFF((
            SELECT ',' + CAST(dtls.JobNo AS VARCHAR(10))
            FROM tbl_Invoice_both_Dtls AS dtls
            WHERE dtls.InvoiceNo = DI.InvoiceNo
            FOR XML PATH(''), TYPE
        ).value('.', 'VARCHAR(MAX)'), 1, 1, '')
    ) AS JobNo
FROM
    DistinctInvoices AS DI
WHERE
   DI.InvoiceNo LIKE '%" + txt_Invoice_search.Text + "%'AND DI.CompName LIKE '%" + txtJobno.Text + "%' AND  DI.InvoiceDate = '" + txt_form_podate_search.Text + "'  AND DI.Is_Deleted = '0' ORDER BY DI.CreatedOn DESC; ", Conn);

        da.Fill(Dt);
        Gvsorted.DataSource = Dt;
        Gvsorted.DataBind();
        Gvsorted.EmptyDataText = "Not Records Found";
    }
    public void GetsortedIJPDGrid()
    {
        GvPurchaseOrderList.Visible = false;
        DateTime date = Convert.ToDateTime(txt_form_podate_search.Text.ToString());
        txt_form_podate_search.Text = date.ToString("yyyy-MM-dd");
        DataTable Dt = new DataTable();
        // SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_Invoice_both_hdr WHERE InvoiceNo LIKE '%" + txt_Invoice_search.Text + "%'AND CompName LIKE '%" + txtJobno.Text + "%' AND  InvoiceDate = '" + txt_form_podate_search.Text + "' AND Is_Deleted='0' ", Conn);
        SqlDataAdapter da = new SqlDataAdapter(@"WITH DistinctInvoices AS (
    SELECT DISTINCT
        IH.Id,
        IH.InvoiceNo,
        IH.PoNo,
        IH.CompName,
        IH.ChallanNo,
        IH.PayTerm,
        IH.InvoiceDate,
        IH.CreatedBy,
        IH.CreatedOn,
        IH.CGST,
        IH.SGST,
        IH.IGST,
        IH.Is_Deleted
    FROM
        tbl_Invoice_both_hdr AS IH
    WHERE
        IH.Is_Deleted = '0'
)
SELECT
    DI.Id,
    DI.InvoiceNo,
    DI.PoNo,
    DI.CompName,
    DI.ChallanNo,
    DI.PayTerm,
    DI.InvoiceDate,
    DI.CreatedBy,
    DI.CreatedOn,
    DI.CGST,
    DI.SGST,
    DI.IGST,
    DI.Is_Deleted,
    (
        SELECT STUFF((
            SELECT ',' + CAST(dtls.JobNo AS VARCHAR(10))
            FROM tbl_Invoice_both_Dtls AS dtls
            WHERE dtls.InvoiceNo = DI.InvoiceNo
            FOR XML PATH(''), TYPE
        ).value('.', 'VARCHAR(MAX)'), 1, 1, '')
    ) AS JobNo
FROM
    DistinctInvoices AS DI
WHERE
   DI.InvoiceNo LIKE '%" + txt_Invoice_search.Text + "%'AND DI.CompName LIKE '%" + txtJobno.Text + "%' AND  DI.InvoiceDate = '" + txt_form_podate_search.Text + "'  AND DI.Is_Deleted = '0' ORDER BY DI.CreatedOn DESC; ", Conn);

        da.Fill(Dt);
        Gvsorted.DataSource = Dt;
        Gvsorted.DataBind();
        Gvsorted.EmptyDataText = "Not Records Found";
    }
    public void GetsortedInvoicejob()
    {
        GvPurchaseOrderList.Visible = false;
        ViewState["Record"] = "Inoce&Job";
        DataTable Dt = new DataTable();
        //SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_Invoice_both_hdr WHERE InvoiceNo LIKE '%" + txt_Invoice_search.Text + "%' AND CompName LIKE '%" + txtJobno.Text + "%' AND Is_Deleted='0' ", Conn);
        SqlDataAdapter da = new SqlDataAdapter(@"WITH DistinctInvoices AS (
    SELECT DISTINCT
        IH.Id,
        IH.InvoiceNo,
        IH.PoNo,
        IH.CompName,
        IH.GrandTotal,
        IH.ChallanNo,
        IH.PayTerm,
        IH.InvoiceDate,
        IH.CreatedBy,
        IH.CreatedOn,
        IH.CGST,
        IH.SGST,
        IH.IGST,
        IH.Is_Deleted
    FROM
        tbl_Invoice_both_hdr AS IH
    WHERE
        IH.Is_Deleted = '0'
)
SELECT
    DI.Id,
    DI.InvoiceNo,
    DI.PoNo,
    DI.CompName,
    DI.GrandTotal,
    DI.ChallanNo,
    DI.PayTerm,
    DI.InvoiceDate,
    DI.CreatedBy,
    DI.CreatedOn,
    DI.CGST,
    DI.SGST,
    DI.IGST,
    DI.Is_Deleted,
    (
        SELECT STUFF((
            SELECT ',' + CAST(dtls.JobNo AS VARCHAR(10))
            FROM tbl_Invoice_both_Dtls AS dtls
            WHERE dtls.InvoiceNo = DI.InvoiceNo
            FOR XML PATH(''), TYPE
        ).value('.', 'VARCHAR(MAX)'), 1, 1, '')
    ) AS JobNo
FROM
    DistinctInvoices AS DI
WHERE
   DI.InvoiceNo LIKE '%" + txt_Invoice_search.Text + "%'AND DI.CompName LIKE '%" + txtJobno.Text + "%'  AND DI.Is_Deleted = '0' ORDER BY DI.CreatedOn DESC; ", Conn);

        da.Fill(Dt);
        Gvsorted.DataSource = Dt;
        Gvsorted.DataBind();
        Gvsorted.EmptyDataText = "Not Records Found";
    }
    public void GetsortedInvoicejobgrid()
    {
        GvPurchaseOrderList.Visible = false;
        DataTable Dt = new DataTable();
        //SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_Invoice_both_hdr WHERE InvoiceNo LIKE '%" + txt_Invoice_search.Text + "%' AND CompName LIKE '%" + txtJobno.Text + "%' AND Is_Deleted='0' ", Conn);


        SqlDataAdapter da = new SqlDataAdapter(@"WITH DistinctInvoices AS (
    SELECT DISTINCT
        IH.Id,
        IH.InvoiceNo,
        IH.PoNo,
        IH.CompName,
        IH.ChallanNo,
        IH.PayTerm,
        IH.InvoiceDate,
        IH.CreatedBy,
        IH.CreatedOn,
        IH.CGST,
        IH.SGST,
        IH.IGST,
        IH.Is_Deleted
    FROM
        tbl_Invoice_both_hdr AS IH
    WHERE
        IH.Is_Deleted = '0'
)
SELECT
    DI.Id,
    DI.InvoiceNo,
    DI.PoNo,
    DI.CompName,
    DI.ChallanNo,
    DI.PayTerm,
    DI.InvoiceDate,
    DI.CreatedBy,
    DI.CreatedOn,
    DI.CGST,
    DI.SGST,
    DI.IGST,
    DI.Is_Deleted,
    (
        SELECT STUFF((
            SELECT ',' + CAST(dtls.JobNo AS VARCHAR(10))
            FROM tbl_Invoice_both_Dtls AS dtls
            WHERE dtls.InvoiceNo = DI.InvoiceNo
            FOR XML PATH(''), TYPE
        ).value('.', 'VARCHAR(MAX)'), 1, 1, '')
    ) AS JobNo
FROM
    DistinctInvoices AS DI
WHERE
   DI.InvoiceNo LIKE '%" + txt_Invoice_search.Text + "%'AND DI.CompName LIKE '%" + txtJobno.Text + "%'  AND DI.Is_Deleted = '0' ORDER BY DI.CreatedOn DESC; ", Conn);




        da.Fill(Dt);
        Gvsorted.DataSource = Dt;
        Gvsorted.DataBind();
        Gvsorted.EmptyDataText = "Not Records Found";
    }
    public void GetsortedInvoiceDatewise()
    {
        GvPurchaseOrderList.Visible = false;
        ViewState["Record"] = "Inoce&Date";
        DateTime date = Convert.ToDateTime(txt_form_podate_search.Text.ToString());
        txt_form_podate_search.Text = date.ToString("yyyy-MM-dd");
        DataTable Dt = new DataTable();
        //SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_Invoice_both_hdr WHERE InvoiceDate BETWEEN '" + txt_form_podate_search.Text + "' AND '" + txt_to_podate_search.Text + "' AND InvoiceNo LIKE '%" + txt_Invoice_search.Text + "%' AND Is_Deleted = '0'", Conn);
        //SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_Invoice_both_hdr WHERE InvoiceNo LIKE '%" + txt_Invoice_search.Text + "%' AND  InvoiceDate = '" + txt_form_podate_search.Text + "' AND Is_Deleted='0' ", Conn);

        SqlDataAdapter da = new SqlDataAdapter(@"WITH DistinctInvoices AS (
    SELECT DISTINCT
        IH.Id,
        IH.InvoiceNo,
        IH.PoNo,
        IH.CompName,
        IH.GrandTotal,
        IH.ChallanNo,
        IH.PayTerm,
        IH.InvoiceDate,
        IH.CreatedBy,
        IH.CreatedOn,
        IH.CGST,
        IH.SGST,
        IH.IGST,
        IH.Is_Deleted
    FROM
        tbl_Invoice_both_hdr AS IH
    WHERE
        IH.Is_Deleted = '0'
)
SELECT
    DI.Id,
    DI.InvoiceNo,
    DI.PoNo,
    DI.CompName,
    DI.GrandTotal,
    DI.ChallanNo,
    DI.PayTerm,
    DI.InvoiceDate,
    DI.CreatedBy,
    DI.CreatedOn,
    DI.CGST,
    DI.SGST,
    DI.IGST,
    DI.Is_Deleted,
    (
        SELECT STUFF((
            SELECT ',' + CAST(dtls.JobNo AS VARCHAR(10))
            FROM tbl_Invoice_both_Dtls AS dtls
            WHERE dtls.InvoiceNo = DI.InvoiceNo
            FOR XML PATH(''), TYPE
        ).value('.', 'VARCHAR(MAX)'), 1, 1, '')
    ) AS JobNo
FROM
    DistinctInvoices AS DI

 WHERE DI.InvoiceDate BETWEEN '" + txt_form_podate_search.Text + "' AND '" + txt_to_podate_search.Text + "' AND DI.InvoiceNo LIKE '%" + txt_Invoice_search.Text + "%'  AND DI.Is_Deleted = '0' ORDER BY DI.CreatedOn DESC; ", Conn);





        da.Fill(Dt);
        Gvsorted.DataSource = Dt;
        Gvsorted.DataBind();
        Gvsorted.EmptyDataText = "Not Records Found";
    }
    public void GetsortedInvoiceDatewisegrid()
    {
        GvPurchaseOrderList.Visible = false;
        DateTime date = Convert.ToDateTime(txt_form_podate_search.Text.ToString());
        txt_form_podate_search.Text = date.ToString("yyyy-MM-dd");
        DataTable Dt = new DataTable();
        // SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_Invoice_both_hdr WHERE InvoiceDate BETWEEN '" + txt_form_podate_search.Text + "' AND '" + txt_to_podate_search.Text + "' AND InvoiceNo LIKE '%" + txt_Invoice_search.Text + "%' AND Is_Deleted = '0'", Conn);
        //SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_Invoice_both_hdr WHERE InvoiceNo LIKE '%" + txt_Invoice_search.Text + "%' AND  InvoiceDate = '" + txt_form_podate_search.Text + "' AND Is_Deleted='0' ", Conn);

        SqlDataAdapter da = new SqlDataAdapter(@"WITH DistinctInvoices AS (
    SELECT DISTINCT
        IH.Id,
        IH.InvoiceNo,
        IH.PoNo,
        IH.CompName,
        IH.ChallanNo,
        IH.PayTerm,
        IH.InvoiceDate,
        IH.CreatedBy,
        IH.CreatedOn,
        IH.CGST,
        IH.SGST,
        IH.IGST,
        IH.Is_Deleted
    FROM
        tbl_Invoice_both_hdr AS IH
    WHERE
        IH.Is_Deleted = '0'
)
SELECT
    DI.Id,
    DI.InvoiceNo,
    DI.PoNo,
    DI.CompName,
    DI.ChallanNo,
    DI.PayTerm,
    DI.InvoiceDate,
    DI.CreatedBy,
    DI.CreatedOn,
    DI.CGST,
    DI.SGST,
    DI.IGST,
    DI.Is_Deleted,
    (
        SELECT STUFF((
            SELECT ',' + CAST(dtls.JobNo AS VARCHAR(10))
            FROM tbl_Invoice_both_Dtls AS dtls
            WHERE dtls.InvoiceNo = DI.InvoiceNo
            FOR XML PATH(''), TYPE
        ).value('.', 'VARCHAR(MAX)'), 1, 1, '')
    ) AS JobNo
FROM
    DistinctInvoices AS DI

 WHERE DI.InvoiceDate BETWEEN '" + txt_form_podate_search.Text + "' AND '" + txt_to_podate_search.Text + "' AND DI.InvoiceNo LIKE '%" + txt_Invoice_search.Text + "%'  AND DI.Is_Deleted = '0' ORDER BY DI.CreatedOn DESC; ", Conn);

        da.Fill(Dt);
        Gvsorted.DataSource = Dt;
        Gvsorted.DataBind();
        Gvsorted.EmptyDataText = "Not Records Found";
    }
    public void GetSortedjobnodatewise()
    {
        GvPurchaseOrderList.Visible = false;
        ViewState["Record"] = "Job&Date";
        DateTime date = Convert.ToDateTime(txt_form_podate_search.Text.ToString());
        txt_form_podate_search.Text = date.ToString("yyyy-MM-dd");

        DataTable Dt = new DataTable();
        // SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_Invoice_both_hdr WHERE CompName LIKE '%" + txtJobno.Text + "%' AND   InvoiceDate = '" + txt_form_podate_search.Text + "' AND Is_Deleted='0' ", Conn);
        SqlDataAdapter da = new SqlDataAdapter(@"WITH DistinctInvoices AS (
    SELECT DISTINCT
        IH.Id,
        IH.InvoiceNo,
        IH.PoNo,
        IH.CompName,
        IH.GrandTotal,
        IH.ChallanNo,
        IH.PayTerm,
        IH.InvoiceDate,
        IH.CreatedBy,
        IH.CreatedOn,
        IH.CGST,
        IH.SGST,
        IH.IGST,
        IH.Is_Deleted
    FROM
        tbl_Invoice_both_hdr AS IH
    WHERE
        IH.Is_Deleted = '0'
)
SELECT
    DI.Id,
    DI.InvoiceNo,
    DI.PoNo,
    DI.CompName,
    DI.GrandTotal,
    DI.ChallanNo,
    DI.PayTerm,
    DI.InvoiceDate,
    DI.CreatedBy,
    DI.CreatedOn,
    DI.CGST,
    DI.SGST,
    DI.IGST,
    DI.Is_Deleted,
    (
        SELECT STUFF((
            SELECT ',' + CAST(dtls.JobNo AS VARCHAR(10))
            FROM tbl_Invoice_both_Dtls AS dtls
            WHERE dtls.InvoiceNo = DI.InvoiceNo
            FOR XML PATH(''), TYPE
        ).value('.', 'VARCHAR(MAX)'), 1, 1, '')
    ) AS JobNo
FROM
    DistinctInvoices AS DI

 WHERE DI.CompName LIKE '%" + txtJobno.Text + "%' AND   DI.InvoiceDate = '" + txt_form_podate_search.Text + "'  AND DI.Is_Deleted = '0' ORDER BY DI.CreatedOn DESC; ", Conn);

        da.Fill(Dt);
        Gvsorted.DataSource = Dt;
        Gvsorted.DataBind();
    }
    public void GetSortedjobnodatewiseGird()
    {
        GvPurchaseOrderList.Visible = false;
        DateTime date = Convert.ToDateTime(txt_form_podate_search.Text.ToString());
        txt_form_podate_search.Text = date.ToString("yyyy-MM-dd");
        DataTable Dt = new DataTable();
        // SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_Invoice_both_hdr WHERE CompName LIKE '%" + txtJobno.Text + "%' AND   InvoiceDate = '" + txt_form_podate_search.Text + "' AND Is_Deleted='0' ", Conn);
        SqlDataAdapter da = new SqlDataAdapter(@"WITH DistinctInvoices AS (
    SELECT DISTINCT
        IH.Id,
        IH.InvoiceNo,
        IH.PoNo,
        IH.CompName,
        IH.ChallanNo,
        IH.PayTerm,
        IH.InvoiceDate,
        IH.CreatedBy,
        IH.CreatedOn,
        IH.CGST,
        IH.SGST,
        IH.IGST,
        IH.Is_Deleted
    FROM
        tbl_Invoice_both_hdr AS IH
    WHERE
        IH.Is_Deleted = '0'
)
SELECT
    DI.Id,
    DI.InvoiceNo,
    DI.PoNo,
    DI.CompName,
    DI.ChallanNo,
    DI.PayTerm,
    DI.InvoiceDate,
    DI.CreatedBy,
    DI.CreatedOn,
    DI.CGST,
    DI.SGST,
    DI.IGST,
    DI.Is_Deleted,
    (
        SELECT STUFF((
            SELECT ',' + CAST(dtls.JobNo AS VARCHAR(10))
            FROM tbl_Invoice_both_Dtls AS dtls
            WHERE dtls.InvoiceNo = DI.InvoiceNo
            FOR XML PATH(''), TYPE
        ).value('.', 'VARCHAR(MAX)'), 1, 1, '')
    ) AS JobNo
FROM
    DistinctInvoices AS DI

 WHERE DI.CompName LIKE '%" + txtJobno.Text + "%' AND   DI.InvoiceDate = '" + txt_form_podate_search.Text + "'  AND DI.Is_Deleted = '0' ORDER BY DI.CreatedOn DESC; ", Conn);

        da.Fill(Dt);
        Gvsorted.DataSource = Dt;
        Gvsorted.DataBind();
    }
    public void GetsortedPODatewise()
    {
        GvPurchaseOrderList.Visible = false;
        ViewState["Record"] = "PODate";
        DateTime date = Convert.ToDateTime(txt_form_podate_search.Text.ToString());
        txt_form_podate_search.Text = date.ToString("yyyy-MM-dd");
        DateTime datee = Convert.ToDateTime(txt_to_podate_search.Text.ToString());
        txt_to_podate_search.Text = datee.ToString("yyyy-MM-dd");
        DataTable Dt = new DataTable();
        //SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_Invoice_both_hdr WHERE  InvoiceDate between '" + txt_form_podate_search.Text + "'  AND '" + txt_to_podate_search.Text + "' AND Is_Deleted='0' ", Conn);
        SqlDataAdapter da = new SqlDataAdapter(@"WITH DistinctInvoices AS (
    SELECT DISTINCT
        IH.Id,
        IH.InvoiceNo,
        IH.PoNo,
        IH.CompName,
        IH.GrandTotal,
        IH.ChallanNo,
        IH.PayTerm,
        IH.InvoiceDate,
        IH.CreatedBy,
        IH.CreatedOn,
        IH.CGST,
        IH.SGST,
        IH.IGST,
        IH.Is_Deleted
    FROM
        tbl_Invoice_both_hdr AS IH
    WHERE
        IH.Is_Deleted = '0'
)
SELECT
    DI.Id,
    DI.InvoiceNo,
    DI.PoNo,
    DI.CompName,
    DI.GrandTotal,
    DI.ChallanNo,
    DI.PayTerm,
    DI.InvoiceDate,
    DI.CreatedBy,
    DI.CreatedOn,
    DI.CGST,
    DI.SGST,
    DI.IGST,
    DI.Is_Deleted,
    (
        SELECT STUFF((
            SELECT ',' + CAST(dtls.JobNo AS VARCHAR(10))
            FROM tbl_Invoice_both_Dtls AS dtls
            WHERE dtls.InvoiceNo = DI.InvoiceNo
            FOR XML PATH(''), TYPE
        ).value('.', 'VARCHAR(MAX)'), 1, 1, '')
    ) AS JobNo
FROM
    DistinctInvoices AS DI

 WHERE DI.InvoiceDate between '" + txt_form_podate_search.Text + "'  AND '" + txt_to_podate_search.Text + "'  AND DI.Is_Deleted = '0' ORDER BY DI.CreatedOn DESC; ", Conn);





        da.Fill(Dt);
        Gvsorted.DataSource = Dt;
        Gvsorted.DataBind();
        Gvsorted.EmptyDataText = "Not Records Found";
    }
    public void GetsortedPODatewisegrid()
    {
        GvPurchaseOrderList.Visible = false;
        DateTime date = Convert.ToDateTime(txt_form_podate_search.Text.ToString());
        txt_form_podate_search.Text = date.ToString("yyyy-MM-dd");
        DateTime datee = Convert.ToDateTime(txt_to_podate_search.Text.ToString());
        txt_to_podate_search.Text = datee.ToString("yyyy-MM-dd");
        DataTable Dt = new DataTable();
        // SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_Invoice_both_hdr WHERE  InvoiceDate between '" + txt_form_podate_search.Text + "'  AND '" + txt_to_podate_search.Text + "' AND Is_Deleted='0' ", Conn);
        SqlDataAdapter da = new SqlDataAdapter(@"WITH DistinctInvoices AS (
    SELECT DISTINCT
        IH.Id,
        IH.InvoiceNo,
        IH.PoNo,
        IH.CompName,
        IH.ChallanNo,
        IH.PayTerm,
        IH.InvoiceDate,
        IH.CreatedBy,
        IH.CreatedOn,
        IH.CGST,
        IH.SGST,
        IH.IGST,
        IH.Is_Deleted
    FROM
        tbl_Invoice_both_hdr AS IH
    WHERE
        IH.Is_Deleted = '0'
)
SELECT
    DI.Id,
    DI.InvoiceNo,
    DI.PoNo,
    DI.CompName,
    DI.ChallanNo,
    DI.PayTerm,
    DI.InvoiceDate,
    DI.CreatedBy,
    DI.CreatedOn,
    DI.CGST,
    DI.SGST,
    DI.IGST,
    DI.Is_Deleted,
    (
        SELECT STUFF((
            SELECT ',' + CAST(dtls.JobNo AS VARCHAR(10))
            FROM tbl_Invoice_both_Dtls AS dtls
            WHERE dtls.InvoiceNo = DI.InvoiceNo
            FOR XML PATH(''), TYPE
        ).value('.', 'VARCHAR(MAX)'), 1, 1, '')
    ) AS JobNo
FROM
    DistinctInvoices AS DI

 WHERE DI.InvoiceDate between '" + txt_form_podate_search.Text + "'  AND '" + txt_to_podate_search.Text + "'  AND DI.Is_Deleted = '0' ORDER BY DI.CreatedOn DESC; ", Conn);


        da.Fill(Dt);
        Gvsorted.DataSource = Dt;
        Gvsorted.DataBind();
        Gvsorted.EmptyDataText = "Not Records Found";
    }
    public void GetsortedInvoicedatewise()
    {
        GvPurchaseOrderList.Visible = false;
        ViewState["Record"] = "InvoiceDate";
        DateTime date = Convert.ToDateTime(txt_form_podate_search.Text.ToString());
        txt_form_podate_search.Text = date.ToString("yyyy-MM-dd");
        DataTable Dt = new DataTable();
        // SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_Invoice_both_hdr WHERE  InvoiceDate= '" + txt_form_podate_search.Text + "'   AND Is_Deleted='0' ", Conn);
        SqlDataAdapter da = new SqlDataAdapter(@"WITH DistinctInvoices AS (
    SELECT DISTINCT
        IH.Id,
        IH.InvoiceNo,
        IH.PoNo,
        IH.CompName,
        IH.GrandTotal,
        IH.ChallanNo,
        IH.PayTerm,
        IH.InvoiceDate,
        IH.CreatedBy,
        IH.CreatedOn,
        IH.CGST,
        IH.SGST,
        IH.IGST,
        IH.Is_Deleted
    FROM
        tbl_Invoice_both_hdr AS IH
    WHERE
        IH.Is_Deleted = '0'
)
SELECT
    DI.Id,
    DI.InvoiceNo,
    DI.PoNo,
    DI.CompName,
    DI.GrandTotal,
    DI.ChallanNo,
    DI.PayTerm,
    DI.InvoiceDate,
    DI.CreatedBy,
    DI.CreatedOn,
    DI.CGST,
    DI.SGST,
    DI.IGST,
    DI.Is_Deleted,
    (
        SELECT STUFF((
            SELECT ',' + CAST(dtls.JobNo AS VARCHAR(10))
            FROM tbl_Invoice_both_Dtls AS dtls
            WHERE dtls.InvoiceNo = DI.InvoiceNo
            FOR XML PATH(''), TYPE
        ).value('.', 'VARCHAR(MAX)'), 1, 1, '')
    ) AS JobNo
FROM
    DistinctInvoices AS DI

 WHERE DI.InvoiceDate= '" + txt_form_podate_search.Text + "'   AND DI.Is_Deleted = '0' ORDER BY DI.CreatedOn DESC; ", Conn);


        da.Fill(Dt);
        Gvsorted.DataSource = Dt;
        Gvsorted.DataBind();
        Gvsorted.EmptyDataText = "Not Records Found";
    }
    public void GetsortedInvoicedatewisegrid()
    {
        GvPurchaseOrderList.Visible = false;
        DateTime date = Convert.ToDateTime(txt_form_podate_search.Text.ToString());
        txt_form_podate_search.Text = date.ToString("yyyy-MM-dd");
        DataTable Dt = new DataTable();
        //SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_Invoice_both_hdr WHERE  InvoiceDate= '" + txt_form_podate_search.Text + "'   AND Is_Deleted='0' ", Conn);
        SqlDataAdapter da = new SqlDataAdapter(@"WITH DistinctInvoices AS (
    SELECT DISTINCT
        IH.Id,
        IH.InvoiceNo,
        IH.PoNo,
        IH.CompName,
        IH.ChallanNo,
        IH.PayTerm,
        IH.InvoiceDate,
        IH.CreatedBy,
        IH.CreatedOn,
        IH.CGST,
        IH.SGST,
        IH.IGST,
        IH.Is_Deleted
    FROM
        tbl_Invoice_both_hdr AS IH
    WHERE
        IH.Is_Deleted = '0'
)
SELECT
    DI.Id,
    DI.InvoiceNo,
    DI.PoNo,
    DI.CompName,
    DI.ChallanNo,
    DI.PayTerm,
    DI.InvoiceDate,
    DI.CreatedBy,
    DI.CreatedOn,
    DI.CGST,
    DI.SGST,
    DI.IGST,
    DI.Is_Deleted,
    (
        SELECT STUFF((
            SELECT ',' + CAST(dtls.JobNo AS VARCHAR(10))
            FROM tbl_Invoice_both_Dtls AS dtls
            WHERE dtls.InvoiceNo = DI.InvoiceNo
            FOR XML PATH(''), TYPE
        ).value('.', 'VARCHAR(MAX)'), 1, 1, '')
    ) AS JobNo
FROM
    DistinctInvoices AS DI

 WHERE DI.InvoiceDate= '" + txt_form_podate_search.Text + "'   AND DI.Is_Deleted = '0' ORDER BY DI.CreatedOn DESC; ", Conn);

        da.Fill(Dt);
        Gvsorted.DataSource = Dt;
        Gvsorted.DataBind();
        Gvsorted.EmptyDataText = "Not Records Found";
    }
    public void GetDatewisecutomer()
    {
        GvPurchaseOrderList.Visible = false;
        ViewState["Record"] = "DatewiseCustomer";
        DateTime date = Convert.ToDateTime(txt_form_podate_search.Text.ToString());
        DataTable Dt = new DataTable();
        txt_form_podate_search.Text = date.ToString("yyyy-MM-dd");
        // SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_Invoice_both_hdr WHERE InvoiceDate BETWEEN '" + txt_form_podate_search.Text + "' AND '" + txt_to_podate_search.Text + "' AND CustName LIKE '%" + txtJobno.Text + "%' AND Is_Deleted = '0'", Conn);

        SqlDataAdapter da = new SqlDataAdapter(@"WITH DistinctInvoices AS (
    SELECT DISTINCT
        IH.Id,
        IH.InvoiceNo,
        IH.PoNo,
        IH.CompName,
        IH.GrandTotal,
        IH.ChallanNo,
        IH.PayTerm,
        IH.InvoiceDate,
        IH.CreatedBy,
        IH.CreatedOn,
        IH.CGST,
        IH.SGST,
        IH.IGST,
        IH.Is_Deleted
    FROM
        tbl_Invoice_both_hdr AS IH
    WHERE
        IH.Is_Deleted = '0'
)
SELECT
    DI.Id,
    DI.InvoiceNo,
    DI.PoNo,
    DI.CompName,
    DI.GrandTotal,
    DI.ChallanNo,
    DI.PayTerm,
    DI.InvoiceDate,
    DI.CreatedBy,
    DI.CreatedOn,
    DI.CGST,
    DI.SGST,
    DI.IGST,
    DI.Is_Deleted,
    (
        SELECT STUFF((
            SELECT ',' + CAST(dtls.JobNo AS VARCHAR(10))
            FROM tbl_Invoice_both_Dtls AS dtls
            WHERE dtls.InvoiceNo = DI.InvoiceNo
            FOR XML PATH(''), TYPE
        ).value('.', 'VARCHAR(MAX)'), 1, 1, '')
    ) AS JobNo
FROM
    DistinctInvoices AS DI

 WHERE DI.InvoiceDate BETWEEN '" + txt_form_podate_search.Text + "' AND '" + txt_to_podate_search.Text + "' AND DI.CompName LIKE '%" + txtJobno.Text + "%'   AND DI.Is_Deleted = '0' ORDER BY DI.CreatedOn DESC; ", Conn);




        da.Fill(Dt);
        Gvsorted.DataSource = Dt;
        Gvsorted.DataBind();
        Gvsorted.EmptyDataText = "Not Records Found";
    }
    public void GetDatewisecutomergrid()
    {
        GvPurchaseOrderList.Visible = false;
        DateTime date = Convert.ToDateTime(txt_form_podate_search.Text.ToString());
        DataTable Dt = new DataTable();
        txt_form_podate_search.Text = date.ToString("yyyy-MM-dd");
        // SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_Invoice_both_hdr WHERE InvoiceDate BETWEEN '" + txt_form_podate_search.Text + "' AND '" + txt_to_podate_search.Text + "' AND CustName LIKE '%" + txtJobno.Text + "%' AND Is_Deleted = '0'", Conn);

        SqlDataAdapter da = new SqlDataAdapter(@"WITH DistinctInvoices AS (
    SELECT DISTINCT
        IH.Id,
        IH.InvoiceNo,
        IH.PoNo,
        IH.CompName,
        IH.ChallanNo,
        IH.PayTerm,
        IH.InvoiceDate,
        IH.CreatedBy,
        IH.CreatedOn,
        IH.CGST,
        IH.SGST,
        IH.IGST,
        IH.Is_Deleted
    FROM
        tbl_Invoice_both_hdr AS IH
    WHERE
        IH.Is_Deleted = '0'
)
SELECT
    DI.Id,
    DI.InvoiceNo,
    DI.PoNo,
    DI.CompName,
    DI.ChallanNo,
    DI.PayTerm,
    DI.InvoiceDate,
    DI.CreatedBy,
    DI.CreatedOn,
    DI.CGST,
    DI.SGST,
    DI.IGST,
    DI.Is_Deleted,
    (
        SELECT STUFF((
            SELECT ',' + CAST(dtls.JobNo AS VARCHAR(10))
            FROM tbl_Invoice_both_Dtls AS dtls
            WHERE dtls.InvoiceNo = DI.InvoiceNo
            FOR XML PATH(''), TYPE
        ).value('.', 'VARCHAR(MAX)'), 1, 1, '')
    ) AS JobNo
FROM
    DistinctInvoices AS DI

 WHERE DI.InvoiceDate BETWEEN '" + txt_form_podate_search.Text + "' AND '" + txt_to_podate_search.Text + "' AND DI.CompName LIKE '%" + txtJobno.Text + "%'   AND DI.Is_Deleted = '0' ORDER BY DI.CreatedOn DESC; ", Conn);



        da.Fill(Dt);
        Gvsorted.DataSource = Dt;
        Gvsorted.DataBind();
        Gvsorted.EmptyDataText = "Not Records Found";
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        //required to avoid the run time error "  
        //Control 'GridView1' of type 'Grid View' must be placed inside a form tag with runat=server."  
    }
    protected void btnexporttoexcel_Click(object sender, EventArgs e)
    {
        ExportGridToExcel();
    }

    private void ExportGridToExcel()
    {
        if (ViewState["Excell"] != null)
        {
            string Method = ViewState["Excell"].ToString();
            if (Method == "GetsortedInvoiceno")
            {
                GetsortedInvoicenoForExcell();
            }
            if (Method == "Getsortedjobno")
            {
                GetsortedjobnoForExcell();
            }
            if (Method == "GetDatewisecutomer")
            {
                GetDatewisecutomerForExcell();
            }
            if (Method == "GetsortedInvoiceno")
            {
                GetsortedInvoicenoForExcell();
            }
            if (Method == "GetsortedIJPD")
            {
                GetsortedIJPDForExcell();
            }
            if (Method == "GetsortedInvoiceDatewise")
            {
                GetsortedInvoiceDatewiseForExcell();

            }
            if (Method == "GetSortedjobnodatewise")
            {
                GetSortedjobnodatewiseForExcell();

            }
            if (Method == "GetsortedPODatewise")
            {
                GetsortedPODatewiseForExcell();

            }
            if (Method == "GetsortedInvoicedatewise")
            {
                GetsortedInvoicedatewiseForExcell();

            }
        }

        else
        {
            GridExport();
        }


        Response.Clear();
        Response.Buffer = true;
        Response.ClearContent();
        Response.ClearHeaders();
        Response.Charset = "";
        string FileName = "Tax_Invoice_List_" + DateTime.Now + ".xls";
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
    public void GetsortedInvoicenoForExcell()
    {
        GvPurchaseOrderList.Visible = false;
        ViewState["Record"] = "Invoice";
        DataTable Dt = new DataTable();
        // SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_Invoice_both_hdr WHERE InvoiceNo = '" + txt_Invoice_search.Text + "' AND Is_Deleted='0' ", Conn);
        SqlDataAdapter da = new SqlDataAdapter(@"WITH DistinctInvoices AS (
    SELECT DISTINCT
        IH.Id,
        IH.InvoiceNo,
        IH.PoNo,
        IH.CompName,
        IH.ChallanNo,
        IH.PayTerm,
        IH.InvoiceDate,
        IH.CreatedBy,
        IH.CreatedOn,
        IH.CGST,
        IH.SGST,
        IH.IGST,
        IH.Is_Deleted
    FROM
        tbl_Invoice_both_hdr AS IH
    WHERE
        IH.Is_Deleted = '0'
)
SELECT
    DI.Id,
    DI.InvoiceNo,
    DI.PoNo,
    DI.CompName,
    DI.ChallanNo,
    DI.PayTerm,
    DI.InvoiceDate,
    DI.CreatedBy,
    DI.CreatedOn,
    DI.CGST,
    DI.SGST,
    DI.IGST,
    DI.Is_Deleted,
    (
        SELECT STUFF((
            SELECT ',' + CAST(dtls.JobNo AS VARCHAR(10))
            FROM tbl_Invoice_both_Dtls AS dtls
            WHERE dtls.InvoiceNo = DI.InvoiceNo
            FOR XML PATH(''), TYPE
        ).value('.', 'VARCHAR(MAX)'), 1, 1, '')
    ) AS JobNo
FROM
    DistinctInvoices AS DI
WHERE
    DI.InvoiceNo = '" + txt_Invoice_search.Text + "' AND DI.Is_Deleted = '0' ORDER BY DI.CreatedOn DESC; ", Conn);

        da.Fill(Dt);
        GridExportExcel.DataSource = Dt;
        GridExportExcel.DataBind();
        GridExportExcel.EmptyDataText = "Not Records Found";
    }
    public void GetsortedjobnoForExcell()
    {

        GvPurchaseOrderList.Visible = false;
        ViewState["Record"] = "Job";
        DataTable Dt = new DataTable();
        // SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_Invoice_both_hdr WHERE CompName LIKE '%" + txtJobno.Text + "%' AND Is_Deleted='0' ", Conn);
        SqlDataAdapter da = new SqlDataAdapter(@"WITH DistinctInvoices AS (
    SELECT DISTINCT
        IH.Id,
        IH.InvoiceNo,
        IH.PoNo,
        IH.CompName,
        IH.ChallanNo,
        IH.PayTerm,
        IH.InvoiceDate,
        IH.CreatedBy,
        IH.CreatedOn,
        IH.CGST,
        IH.SGST,
        IH.IGST,
        IH.Is_Deleted
    FROM
        tbl_Invoice_both_hdr AS IH
    WHERE
        IH.Is_Deleted = '0'
)
SELECT
    DI.Id,
    DI.InvoiceNo,
    DI.PoNo,
    DI.CompName,
    DI.ChallanNo,
    DI.PayTerm,
    DI.InvoiceDate,
    DI.CreatedBy,
    DI.CreatedOn,
    DI.CGST,
    DI.SGST,
    DI.IGST,
    DI.Is_Deleted,
    (
        SELECT STUFF((
            SELECT ',' + CAST(dtls.JobNo AS VARCHAR(10))
            FROM tbl_Invoice_both_Dtls AS dtls
            WHERE dtls.InvoiceNo = DI.InvoiceNo
            FOR XML PATH(''), TYPE
        ).value('.', 'VARCHAR(MAX)'), 1, 1, '')
    ) AS JobNo
FROM
    DistinctInvoices AS DI
WHERE
    DI.CompName LIKE '%" + txtJobno.Text + "%'  AND DI.Is_Deleted = '0' ORDER BY DI.CreatedOn DESC; ", Conn);
        da.Fill(Dt);
        GridExportExcel.DataSource = Dt;
        GridExportExcel.DataBind();
        GridExportExcel.EmptyDataText = "Not Records Found";
    }
    public void GetDatewisecutomerForExcell()
    {
        GvPurchaseOrderList.Visible = false;
        ViewState["Record"] = "DatewiseCustomer";
        DateTime date = Convert.ToDateTime(txt_form_podate_search.Text.ToString());
        DataTable Dt = new DataTable();
        txt_form_podate_search.Text = date.ToString("yyyy-MM-dd");
        // SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_Invoice_both_hdr WHERE InvoiceDate BETWEEN '" + txt_form_podate_search.Text + "' AND '" + txt_to_podate_search.Text + "' AND CustName LIKE '%" + txtJobno.Text + "%' AND Is_Deleted = '0'", Conn);
        SqlDataAdapter da = new SqlDataAdapter(@"WITH DistinctInvoices AS (
    SELECT DISTINCT
        IH.Id,
        IH.InvoiceNo,
        IH.PoNo,
        IH.CompName,
        IH.ChallanNo,
        IH.PayTerm,
        IH.InvoiceDate,
        IH.CreatedBy,
        IH.CreatedOn,
        IH.CGST,
        IH.SGST,
        IH.IGST,
        IH.Is_Deleted
    FROM
        tbl_Invoice_both_hdr AS IH
    WHERE
        IH.Is_Deleted = '0'
)
SELECT
    DI.Id,
    DI.InvoiceNo,
    DI.PoNo,
    DI.CompName,
    DI.ChallanNo,
    DI.PayTerm,
    DI.InvoiceDate,
    DI.CreatedBy,
    DI.CreatedOn,
    DI.CGST,
    DI.SGST,
    DI.IGST,
    DI.Is_Deleted,
    (
        SELECT STUFF((
            SELECT ',' + CAST(dtls.JobNo AS VARCHAR(10))
            FROM tbl_Invoice_both_Dtls AS dtls
            WHERE dtls.InvoiceNo = DI.InvoiceNo
            FOR XML PATH(''), TYPE
        ).value('.', 'VARCHAR(MAX)'), 1, 1, '')
    ) AS JobNo
FROM
    DistinctInvoices AS DI
WHERE
    DI.InvoiceDate= '" + txt_form_podate_search.Text + "'  AND DI.Is_Deleted = '0' ORDER BY DI.CreatedOn DESC; ", Conn);

        da.Fill(Dt);
        GridExportExcel.DataSource = Dt;
        GridExportExcel.DataBind();
        GridExportExcel.EmptyDataText = "Not Records Found";
    }
    public void GetsortedIJPDForExcell()
    {
        GvPurchaseOrderList.Visible = false;
        ViewState["Record"] = "IJPD";
        DateTime date = Convert.ToDateTime(txt_form_podate_search.Text.ToString());
        txt_form_podate_search.Text = date.ToString("yyyy-MM-dd");

        DataTable Dt = new DataTable();
        //SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_Invoice_both_hdr WHERE InvoiceNo LIKE '%" + txt_Invoice_search.Text + "%'AND CompName LIKE '%" + txtJobno.Text + "%' AND  InvoiceDate = '" + txt_form_podate_search.Text + "' AND Is_Deleted='0' ", Conn);
        SqlDataAdapter da = new SqlDataAdapter(@"WITH DistinctInvoices AS (
    SELECT DISTINCT
        IH.Id,
        IH.InvoiceNo,
        IH.PoNo,
        IH.CompName,
        IH.ChallanNo,
        IH.PayTerm,
        IH.InvoiceDate,
        IH.CreatedBy,
        IH.CreatedOn,
        IH.CGST,
        IH.SGST,
        IH.IGST,
        IH.Is_Deleted
    FROM
        tbl_Invoice_both_hdr AS IH
    WHERE
        IH.Is_Deleted = '0'
)
SELECT
    DI.Id,
    DI.InvoiceNo,
    DI.PoNo,
    DI.CompName,
    DI.ChallanNo,
    DI.PayTerm,
    DI.InvoiceDate,
    DI.CreatedBy,
    DI.CreatedOn,
    DI.CGST,
    DI.SGST,
    DI.IGST,
    DI.Is_Deleted,
    (
        SELECT STUFF((
            SELECT ',' + CAST(dtls.JobNo AS VARCHAR(10))
            FROM tbl_Invoice_both_Dtls AS dtls
            WHERE dtls.InvoiceNo = DI.InvoiceNo
            FOR XML PATH(''), TYPE
        ).value('.', 'VARCHAR(MAX)'), 1, 1, '')
    ) AS JobNo
FROM
    DistinctInvoices AS DI
WHERE
   DI.InvoiceNo LIKE '%" + txt_Invoice_search.Text + "%'AND DI.CompName LIKE '%" + txtJobno.Text + "%' AND  DI.InvoiceDate = '" + txt_form_podate_search.Text + "'  AND DI.Is_Deleted = '0' ORDER BY DI.CreatedOn DESC; ", Conn);

        da.Fill(Dt);
        GridExportExcel.DataSource = Dt;
        GridExportExcel.DataBind();
        GridExportExcel.EmptyDataText = "Not Records Found";
    }
    public void GetsortedInvoicejobForExcell()
    {
        GvPurchaseOrderList.Visible = false;
        ViewState["Record"] = "Inoce&Job";
        DataTable Dt = new DataTable();
        //SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_Invoice_both_hdr WHERE InvoiceNo LIKE '%" + txt_Invoice_search.Text + "%' AND CompName LIKE '%" + txtJobno.Text + "%' AND Is_Deleted='0' ", Conn);
        SqlDataAdapter da = new SqlDataAdapter(@"WITH DistinctInvoices AS (
    SELECT DISTINCT
        IH.Id,
        IH.InvoiceNo,
        IH.PoNo,
        IH.CompName,
        IH.ChallanNo,
        IH.PayTerm,
        IH.InvoiceDate,
        IH.CreatedBy,
        IH.CreatedOn,
        IH.CGST,
        IH.SGST,
        IH.IGST,
        IH.Is_Deleted
    FROM
        tbl_Invoice_both_hdr AS IH
    WHERE
        IH.Is_Deleted = '0'
)
SELECT
    DI.Id,
    DI.InvoiceNo,
    DI.PoNo,
    DI.CompName,
    DI.ChallanNo,
    DI.PayTerm,
    DI.InvoiceDate,
    DI.CreatedBy,
    DI.CreatedOn,
    DI.CGST,
    DI.SGST,
    DI.IGST,
    DI.Is_Deleted,
    (
        SELECT STUFF((
            SELECT ',' + CAST(dtls.JobNo AS VARCHAR(10))
            FROM tbl_Invoice_both_Dtls AS dtls
            WHERE dtls.InvoiceNo = DI.InvoiceNo
            FOR XML PATH(''), TYPE
        ).value('.', 'VARCHAR(MAX)'), 1, 1, '')
    ) AS JobNo
FROM
    DistinctInvoices AS DI
WHERE
   DI.InvoiceNo LIKE '%" + txt_Invoice_search.Text + "%'AND DI.CompName LIKE '%" + txtJobno.Text + "%'  AND DI.Is_Deleted = '0' ORDER BY DI.CreatedOn DESC; ", Conn);

        da.Fill(Dt);
        GridExportExcel.DataSource = Dt;
        GridExportExcel.DataBind();
        GridExportExcel.EmptyDataText = "Not Records Found";
    }
    public void GetsortedInvoiceDatewiseForExcell()
    {
        GvPurchaseOrderList.Visible = false;
        ViewState["Record"] = "Inoce&Date";
        DateTime date = Convert.ToDateTime(txt_form_podate_search.Text.ToString());
        txt_form_podate_search.Text = date.ToString("yyyy-MM-dd");
        DataTable Dt = new DataTable();
        //SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_Invoice_both_hdr WHERE InvoiceDate BETWEEN '" + txt_form_podate_search.Text + "' AND '" + txt_to_podate_search.Text + "' AND InvoiceNo LIKE '%" + txt_Invoice_search.Text + "%' AND Is_Deleted = '0'", Conn);
        //SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_Invoice_both_hdr WHERE InvoiceNo LIKE '%" + txt_Invoice_search.Text + "%' AND  InvoiceDate = '" + txt_form_podate_search.Text + "' AND Is_Deleted='0' ", Conn);

        SqlDataAdapter da = new SqlDataAdapter(@"WITH DistinctInvoices AS (
    SELECT DISTINCT
        IH.Id,
        IH.InvoiceNo,
        IH.PoNo,
        IH.CompName,
        IH.ChallanNo,
        IH.PayTerm,
        IH.InvoiceDate,
        IH.CreatedBy,
        IH.CreatedOn,
        IH.CGST,
        IH.SGST,
        IH.IGST,
        IH.Is_Deleted
    FROM
        tbl_Invoice_both_hdr AS IH
    WHERE
        IH.Is_Deleted = '0'
)
SELECT
    DI.Id,
    DI.InvoiceNo,
    DI.PoNo,
    DI.CompName,
    DI.ChallanNo,
    DI.PayTerm,
    DI.InvoiceDate,
    DI.CreatedBy,
    DI.CreatedOn,
    DI.CGST,
    DI.SGST,
    DI.IGST,
    DI.Is_Deleted,
    (
        SELECT STUFF((
            SELECT ',' + CAST(dtls.JobNo AS VARCHAR(10))
            FROM tbl_Invoice_both_Dtls AS dtls
            WHERE dtls.InvoiceNo = DI.InvoiceNo
            FOR XML PATH(''), TYPE
        ).value('.', 'VARCHAR(MAX)'), 1, 1, '')
    ) AS JobNo
FROM
    DistinctInvoices AS DI

 WHERE DI.InvoiceDate BETWEEN '" + txt_form_podate_search.Text + "' AND '" + txt_to_podate_search.Text + "' AND DI.InvoiceNo LIKE '%" + txt_Invoice_search.Text + "%'  AND DI.Is_Deleted = '0' ORDER BY DI.CreatedOn DESC; ", Conn);


        da.Fill(Dt);
        GridExportExcel.DataSource = Dt;
        GridExportExcel.DataBind();
        GridExportExcel.EmptyDataText = "Not Records Found";
    }

    public void GetSortedjobnodatewiseForExcell()
    {
        GvPurchaseOrderList.Visible = false;
        ViewState["Record"] = "Job&Date";
        DateTime date = Convert.ToDateTime(txt_form_podate_search.Text.ToString());
        txt_form_podate_search.Text = date.ToString("yyyy-MM-dd");

        DataTable Dt = new DataTable();
        // SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_Invoice_both_hdr WHERE CompName LIKE '%" + txtJobno.Text + "%' AND   InvoiceDate = '" + txt_form_podate_search.Text + "' AND Is_Deleted='0' ", Conn);
        SqlDataAdapter da = new SqlDataAdapter(@"WITH DistinctInvoices AS (
    SELECT DISTINCT
        IH.Id,
        IH.InvoiceNo,
        IH.PoNo,
        IH.CompName,
        IH.ChallanNo,
        IH.PayTerm,
        IH.InvoiceDate,
        IH.CreatedBy,
        IH.CreatedOn,
        IH.CGST,
        IH.SGST,
        IH.IGST,
        IH.Is_Deleted
    FROM
        tbl_Invoice_both_hdr AS IH
    WHERE
        IH.Is_Deleted = '0'
)
SELECT
    DI.Id,
    DI.InvoiceNo,
    DI.PoNo,
    DI.CompName,
    DI.ChallanNo,
    DI.PayTerm,
    DI.InvoiceDate,
    DI.CreatedBy,
    DI.CreatedOn,
    DI.CGST,
    DI.SGST,
    DI.IGST,
    DI.Is_Deleted,
    (
        SELECT STUFF((
            SELECT ',' + CAST(dtls.JobNo AS VARCHAR(10))
            FROM tbl_Invoice_both_Dtls AS dtls
            WHERE dtls.InvoiceNo = DI.InvoiceNo
            FOR XML PATH(''), TYPE
        ).value('.', 'VARCHAR(MAX)'), 1, 1, '')
    ) AS JobNo
FROM
    DistinctInvoices AS DI

 WHERE DI.CompName LIKE '%" + txtJobno.Text + "%' AND   DI.InvoiceDate = '" + txt_form_podate_search.Text + "'  AND DI.Is_Deleted = '0' ORDER BY DI.CreatedOn DESC; ", Conn);

        da.Fill(Dt);
        GridExportExcel.DataSource = Dt;
        GridExportExcel.DataBind();
    }
    public void GetsortedPODatewiseForExcell()
    {
        GvPurchaseOrderList.Visible = false;
        ViewState["Record"] = "PODate";
        DateTime date = Convert.ToDateTime(txt_form_podate_search.Text.ToString());
        txt_form_podate_search.Text = date.ToString("yyyy-MM-dd");
        DateTime datee = Convert.ToDateTime(txt_to_podate_search.Text.ToString());
        txt_to_podate_search.Text = datee.ToString("yyyy-MM-dd");
        DataTable Dt = new DataTable();
        //SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_Invoice_both_hdr WHERE  InvoiceDate between '" + txt_form_podate_search.Text + "'  AND '" + txt_to_podate_search.Text + "' AND Is_Deleted='0' ", Conn);
        SqlDataAdapter da = new SqlDataAdapter(@"WITH DistinctInvoices AS (
    SELECT DISTINCT
        IH.Id,
        IH.InvoiceNo,
        IH.PoNo,
        IH.CompName,
        IH.ChallanNo,
        IH.PayTerm,
        IH.InvoiceDate,
        IH.CreatedBy,
        IH.CreatedOn,
        IH.CGST,
        IH.SGST,
        IH.IGST,
        IH.Is_Deleted
    FROM
        tbl_Invoice_both_hdr AS IH
    WHERE
        IH.Is_Deleted = '0'
)
SELECT
    DI.Id,
    DI.InvoiceNo,
    DI.PoNo,
    DI.CompName,
    DI.ChallanNo,
    DI.PayTerm,
    DI.InvoiceDate,
    DI.CreatedBy,
    DI.CreatedOn,
    DI.CGST,
    DI.SGST,
    DI.IGST,
    DI.Is_Deleted,
    (
        SELECT STUFF((
            SELECT ',' + CAST(dtls.JobNo AS VARCHAR(10))
            FROM tbl_Invoice_both_Dtls AS dtls
            WHERE dtls.InvoiceNo = DI.InvoiceNo
            FOR XML PATH(''), TYPE
        ).value('.', 'VARCHAR(MAX)'), 1, 1, '')
    ) AS JobNo
FROM
    DistinctInvoices AS DI

 WHERE DI.InvoiceDate between '" + txt_form_podate_search.Text + "'  AND '" + txt_to_podate_search.Text + "'  AND DI.Is_Deleted = '0' ORDER BY DI.CreatedOn DESC; ", Conn);

        da.Fill(Dt);
        GridExportExcel.DataSource = Dt;
        GridExportExcel.DataBind();
        GridExportExcel.EmptyDataText = "Not Records Found";
    }
    public void GetsortedInvoicedatewiseForExcell()
    {
        GvPurchaseOrderList.Visible = false;
        ViewState["Record"] = "InvoiceDate";
        DateTime date = Convert.ToDateTime(txt_form_podate_search.Text.ToString());
        txt_form_podate_search.Text = date.ToString("yyyy-MM-dd");
        DataTable Dt = new DataTable();
        //SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_Invoice_both_hdr WHERE  InvoiceDate= '" + txt_form_podate_search.Text + "'   AND Is_Deleted='0' ", Conn);
        SqlDataAdapter da = new SqlDataAdapter(@"WITH DistinctInvoices AS (
    SELECT DISTINCT
        IH.Id,
        IH.InvoiceNo,
        IH.PoNo,
        IH.CompName,
        IH.ChallanNo,
        IH.PayTerm,
        IH.InvoiceDate,
        IH.CreatedBy,
        IH.CreatedOn,
        IH.CGST,
        IH.SGST,
        IH.IGST,
        IH.Is_Deleted
    FROM
        tbl_Invoice_both_hdr AS IH
    WHERE
        IH.Is_Deleted = '0'
)
SELECT
    DI.Id,
    DI.InvoiceNo,
    DI.PoNo,
    DI.CompName,
    DI.ChallanNo,
    DI.PayTerm,
    DI.InvoiceDate,
    DI.CreatedBy,
    DI.CreatedOn,
    DI.CGST,
    DI.SGST,
    DI.IGST,
    DI.Is_Deleted,
    (
        SELECT STUFF((
            SELECT ',' + CAST(dtls.JobNo AS VARCHAR(10))
            FROM tbl_Invoice_both_Dtls AS dtls
            WHERE dtls.InvoiceNo = DI.InvoiceNo
            FOR XML PATH(''), TYPE
        ).value('.', 'VARCHAR(MAX)'), 1, 1, '')
    ) AS JobNo
FROM
    DistinctInvoices AS DI

 WHERE DI.InvoiceDate= '" + txt_form_podate_search.Text + "'   AND DI.Is_Deleted = '0' ORDER BY DI.CreatedOn DESC; ", Conn);

        da.Fill(Dt);
        GridExportExcel.DataSource = Dt;
        GridExportExcel.DataBind();
        GridExportExcel.EmptyDataText = "Not Records Found";
    }

    // Nikhil Code for Pending Quotations

    protected void gv_Quot_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            Label lbl_Quo_no = (Label)e.Row.FindControl("lblQuNo");
            Label lblcompanyname = (Label)e.Row.FindControl("lblCompName");
            Label lblsubcustomer = (Label)e.Row.FindControl("lblsubcustomer");
        
            string Id = gv_Quot_List.DataKeys[e.Row.RowIndex].Value.ToString();
            GridView gvDetails = e.Row.FindControl("gvDetails") as GridView;

            SqlDataAdapter Daaa = new SqlDataAdapter(" SELECT * FROM [CustomerPO_Dtls_Both] WHERE [Quotationno]='" + Id + "' AND [JobNo] IS NOT NULL", Conn);
            DataTable Dttt = new DataTable();
            Daaa.Fill(Dttt);
            foreach (DataRow row in Dttt.Rows)
            {
                
                SqlDataAdapter Da = new SqlDataAdapter("SELECT CreatedOn FROM CustomerPO_Hdr_Both WHERE id ='" + row["PurchaseId"] + "'", Conn);
                DataTable Dt = new DataTable();
                Da.Fill(Dt);

               
                if (Dt.Rows.Count > 0)
                {
                    DateTime createdOn = Convert.ToDateTime(Dt.Rows[0]["CreatedOn"]);

                    if (!Dttt.Columns.Contains("CreatedOn"))
                    {
                        Dttt.Columns.Add("CreatedOn", typeof(DateTime)); 
                    }

                    row["CreatedOn"] = createdOn;  

                    if (row["JobDaysCount"] == DBNull.Value || Convert.ToInt32(row["JobDaysCount"]) == 0)
                    {
                        DateTime createdOnDateOnly = createdOn.Date;
                        int jobDaysCount = (DateTime.Now.Date - createdOnDateOnly).Days;
                        row["JobDaysCount"] = jobDaysCount; 
                    }
                }
            }
            gvDetails.DataSource = Dttt;
            gvDetails.DataBind();


        }
    }

    protected void lnkshow_Click(object sender, EventArgs e)
    {

        try
        {
            DateTime startDate = DateTime.Now.Date.AddDays(-30);
            //DateTime endDate = DateTime.Now.Date;
            DateTime endDate = DateTime.Now.Date.AddDays(1);

            string formattedStartDate = startDate.ToString("yyyy-MM-dd");
            string formattedEndDate = endDate.ToString("yyyy-MM-dd");

            string role = Session["adminname"].ToString();
            if (role == "SubCustomer")
            {

                DataTable Dt = new DataTable();

                string query = @"
             SELECT * FROM CustomerPO_Hdr_Both H
          LEFT JOIN (
              SELECT Quotationno, COUNT(JobNo) AS JobCount
              FROM CustomerPO_Dtls_Both J
              WHERE JobNo IS NOT NULL AND JobStatus ='Pending' 
              GROUP BY Quotationno
          ) J ON H.Quotationno = J.Quotationno 
          WHERE H.Status = 'Pending' AND H.Is_deleted = '0' AND H.CreatedOn >= @StartDate AND H.CreatedOn <= @EndDate 
          AND H.Customer_Name = 'Schneider Electric India Pvt.Ltd.' AND (J.JobCount > 0)
          ORDER BY H.CreatedOn DESC;";

                SqlDataAdapter Da = new SqlDataAdapter(query, Conn);
                Da.SelectCommand.Parameters.AddWithValue("@StartDate", formattedStartDate);
                Da.SelectCommand.Parameters.AddWithValue("@EndDate", formattedEndDate);

                Da.Fill(Dt);
                gv_Quot_List.DataSource = Dt;
                gv_Quot_List.DataBind();
                modelprofile.Show();
            }
            else
            {
                DataTable Dt = new DataTable();

                string query = @"
                SELECT *,                     
              ISNULL(J.JobCount, 0) AS JobCount
          FROM CustomerPO_Hdr_Both H
          LEFT JOIN (
              SELECT Quotationno, COUNT(JobNo) AS JobCount
              FROM CustomerPO_Dtls_Both J
              WHERE JobNo IS NOT NULL AND JobStatus ='Pending' 
              GROUP BY Quotationno
          ) J ON H.Quotationno = J.Quotationno
          WHERE H.Status = 'Pending' AND H.Is_deleted = '0' AND H.CreatedOn >= @StartDate AND H.CreatedOn <= @EndDate AND (J.JobCount > 0)
          ORDER BY H.CreatedOn DESC;";

                SqlDataAdapter Da = new SqlDataAdapter(query, Conn);
                Da.SelectCommand.Parameters.AddWithValue("@StartDate", formattedStartDate);
                Da.SelectCommand.Parameters.AddWithValue("@EndDate", formattedEndDate);

                Da.Fill(Dt);
                gv_Quot_List.DataSource = Dt;
                gv_Quot_List.DataBind();
                modelprofile.Show();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static int GetJobCount()
    {
        int jobCount = 0;
        DateTime startDate = DateTime.Now.Date.AddDays(-30);
        //DateTime endDate = DateTime.Now.Date;
        DateTime endDate = DateTime.Now.Date.AddDays(1);

        string formattedStartDate = startDate.ToString("yyyy-MM-dd");
        string formattedEndDate = endDate.ToString("yyyy-MM-dd");
        string connString = System.Configuration.ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlDataAdapter Da = new SqlDataAdapter("SELECT Id FROM CustomerPO_Hdr_Both WHERE Is_deleted = '0' AND CreatedOn >= '" + formattedStartDate + "' AND CreatedOn <= '" + formattedEndDate + "'", con);
            DataTable Dt = new DataTable();
            Da.Fill(Dt);

            foreach (DataRow row in Dt.Rows)
            {
                string purchaseId = row["Id"].ToString();
                
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM CustomerPO_Dtls_Both WHERE JobNo IS NOT NULL AND JobStatus = 'Pending' AND PurchaseId = @PurchaseId", con);
                cmd.Parameters.AddWithValue("@PurchaseId", purchaseId); 

                con.Open();
                jobCount += (int)cmd.ExecuteScalar();
                con.Close();
            }
        }

        return jobCount;
    }

    protected void lnkbtnCorrect_Click(object sender, EventArgs e)
    {
        LinkButton clickedButton = (LinkButton)sender;
        string QuoNo = clickedButton.CommandArgument;

        Response.Redirect("TaxInvoice.aspx?QuoNo=" + encrypt(QuoNo) + "");
    }

    // Delete functionality 

    [System.Web.Services.WebMethod]
    public static bool ProcessJobNos(List<string> jobNos)
    {
        try
        {
            foreach (var jobNo in jobNos)
            {
                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(connString))
                {                    
                        SqlCommand cmd = new SqlCommand("UPDATE CustomerPO_Dtls_Both SET JobStatus = 'Closed'," +
                        " JobDaysCount = DATEDIFF(DAY, CreatedOn, GETDATE())" +
                        "  WHERE JobNo = '" + jobNo + "'", con);
                        con.Open();
                        //cmd.ExecuteScalar();                   
                }
            }
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }



}


