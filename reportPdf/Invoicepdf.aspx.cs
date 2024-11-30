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
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Invoicepdf : System.Web.UI.Page
{
    SqlConnection Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
    string id;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["Id"] != null)
            {
                id = Decrypt(Request.QueryString["Id"].ToString());
                //id = Session["PDFID"].ToString();// Decrypt(Request.QueryString["Id"].ToString());
                //Pdf("Original");
            }
        }

        //if (!IsPostBack)
        //{
        //    if (Request.QueryString["Id"] != null)
        //    {
        //        id = Decrypt(Request.QueryString["Id"].ToString());
        //        Pdf(id);
        //    }
        //}
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

    private void Pdf(string flg)
    {
        //string id = Session["PDFID"].ToString();

        DataTable Dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter("SELECT tbl_Invoice_both_hdr.Id,tbl_Invoice_both_hdr.InvoiceNo,InvoiceDate,PoNo,PoDate,CustName,ChallanNo,ChallanDate,PayTerm," +
            "Delivery,KindAtt,CompanyAddress,CompanyGstNo,CompanyPanNo, ComapyRegType, CompanyStateCode, " +
            "CustomerShippingAddress, CustomerGstNo, CustomerPanNo, CustomerRegType,CustomerStateCode, CGST, SGST,IGST," +
            " AllTotalAmount, GrandTotal, TotalInWord, Description,printdescription, Hsn, TaxPercentage, Quntity, Unit, Rate, DiscountPercentage," +
            " Total,CreatedOn FROM tbl_Invoice_both_hdr INNER JOIN tbl_Invoice_both_Dtls ON tbl_Invoice_both_Dtls.InvoiceId = tbl_Invoice_both_hdr.Id WHERE tbl_Invoice_both_hdr.Id='" + id + "'", Conn);
        da.Fill(Dt);
        //GvPurchaseOrderList.DataSource = Dt;
        // GvPurchaseOrderList.DataBind();

        StringWriter sw = new StringWriter();
        StringReader sr = new StringReader(sw.ToString());

        Document doc = new Document(PageSize.A4, 10f, 10f, 55f, 0f);

        PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(Server.MapPath("~/Files/") + "TaxInvoice.pdf", FileMode.Create));
        //PdfWriter writer = PdfWriter.GetInstance(doc, Response.OutputStream);
        iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, sr);

        doc.Open();

        string imageURL = Server.MapPath("~") + "/image/AA.png";

        iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(imageURL);

        //Resize image depend upon your need

        png.ScaleToFit(70, 100);

        //For Image Position
        png.SetAbsolutePosition(40, 745);
        //var document = new Document();

        //Give space before image
        //png.ScaleToFit(document.PageSize.Width - (document.RightMargin * 100), 50);
        png.SpacingBefore = 50f;

        //Give some space after the image

        png.SpacingAfter = 1f;

        png.Alignment = Element.ALIGN_LEFT;

        //paragraphimage.Add(png);
        //doc.Add(paragraphimage);
        doc.Add(png);

        PdfContentByte cb = writer.DirectContent;
        cb.Rectangle(17f, 735f, 560f, 60f);
        cb.Stroke();
        // Header 
        cb.BeginText();
        cb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 20);
        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "ENDEAVOUR AUTOMATION", 180, 773, 0);
        cb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 11);
        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "S.N.9/2/A1,Sakpal Wasti,Behind Genesis furniture,Nr.HP Petrol Pump,Ravet Road", 145, 755, 0);
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

        //-------------------
        PdfContentByte cdd = writer.DirectContent;
        cdd.Rectangle(17f, 660f, 560f, 25f);
        cdd.Stroke();
        cdd.BeginText();

        if (flg == "Original")
        {
            cb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "ORIGINAL FOR BUYER", 480, 800, 0);
        }
        else if (flg == "Duplicate")
        {
            cb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "DUPLICATE FOR TRANSPORTER", 450, 800, 0);
        }
        else if (flg == "Triplicate")
        {
            cb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "TRIPLICATE FOR SUPPLIER", 470, 800, 0);
        }
        else if (flg == "Extra")
        {
            cb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "EXTRA COPY", 480, 800, 0);
        }
        cdd.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 17);
        cdd.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Tax Invoice", 260, 667, 0);
        cdd.EndText();
        //--------------


        // Header 
        //cd.BeginText();
        //cd.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 17);
        //cd.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Tax Invoice", 260, 667, 0);
        //cd.EndText();

        if (Dt.Rows.Count > 0)
        {
            var CreateDate = DateTime.Now.ToString("dd-MM-yyyy");
            string CustName = Dt.Rows[0]["CustName"].ToString();
            string InvoiceNo = Dt.Rows[0]["InvoiceNo"].ToString();
            string PoDate = Dt.Rows[0]["PoDate"].ToString().TrimEnd("0:0".ToCharArray());
            string PoNumber = Dt.Rows[0]["PoNo"].ToString();
            string CompanyAddress = Dt.Rows[0]["CompanyAddress"].ToString();
            string Address = Dt.Rows[0]["CustomerShippingAddress"].ToString();
            string InvoiceDate = Dt.Rows[0]["InvoiceDate"].ToString().TrimEnd("0:0".ToCharArray());
            string ChallanNo = Dt.Rows[0]["ChallanNo"].ToString();
            string GSTNo = Dt.Rows[0]["CustomerGstNo"].ToString();
            string KindAtt = Dt.Rows[0]["KindAtt"].ToString();
            string TotalInWord = Dt.Rows[0]["TotalInWord"].ToString();
            string GrandTotal = Dt.Rows[0]["GrandTotal"].ToString();
            string CGST = Dt.Rows[0]["CGST"].ToString();
            string SGST = Dt.Rows[0]["SGST"].ToString();
            //string IGST = Dt.Rows[0]["IGST"].ToString();
            string Total = Dt.Rows[0]["AllTotalAmount"].ToString();
            string CompanyStateCode = Dt.Rows[0]["CompanyStateCode"].ToString();


            decimal IGST = Convert.ToDecimal(Dt.Rows[0]["IGST"]);
            decimal GSTAmt = IGST / 2;



            Paragraph paragraphTable1 = new Paragraph();
            paragraphTable1.SpacingBefore = 120f;
            paragraphTable1.SpacingAfter = 10f;

            PdfPTable table = new PdfPTable(4);

            float[] widths2 = new float[] { 100, 180, 100, 180 };
            table.SetWidths(widths2);
            table.TotalWidth = 560f;
            table.LockedWidth = true;

            DateTime ffff1 = Convert.ToDateTime(Dt.Rows[0]["InvoiceDate"].ToString());
            string datee = ffff1.ToString("dd-MM-yyyy");

            DateTime ffff2 = Convert.ToDateTime(Dt.Rows[0]["PoDate"].ToString());
            string podate = ffff1.ToString("dd-MM-yyyy");


            table.AddCell(new Phrase("Buyer : ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(CustName, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("Consignee :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(CustName, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("Shipping Address : ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(CompanyAddress, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("Billing Address : ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(Address, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("Invoice Number : ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(InvoiceNo, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("Invoice Date :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(datee, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            //table.AddCell(new Phrase("Address", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            //table.AddCell(new Phrase(Address, FontFactory.GetFont("Arial", 9, Font.BOLD)));


            table.AddCell(new Phrase("P.O. Number :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(PoNumber, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("P.O. Date :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(podate, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("Challan No. :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(ChallanNo, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("GST No. :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
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
                    table.AddCell(new Phrase(dr["printdescription"].ToString(), FontFactory.GetFont("Arial", 9)));
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
                table.AddCell(new Phrase("  \n\n\n\n\n\n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                //doc.Add(table);
            }
            else if (Dt.Rows.Count >= 7 && Dt.Rows.Count <= 9)
            {
                table.AddCell(new Phrase("  \n\n\n\n\n\n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));

            }
            else if (Dt.Rows.Count >= 4 && Dt.Rows.Count <= 6)
            {
                table.AddCell(new Phrase("  \n\n\n\n\n\n\n\n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));

            }
            else if (Dt.Rows.Count < 4)
            {

                table.AddCell(new Phrase("  \n\n\n\n\n\n\n\n\n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));

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
            table.AddCell(paragraph);
            PdfPCell cell = new PdfPCell(new Phrase("Sub Total", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cell);
            PdfPCell cell11 = new PdfPCell(new Phrase(Total, FontFactory.GetFont("Arial", 10, Font.BOLD)));
            cell11.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cell11);


            doc.Add(table);
            //add total row end


            if (CompanyStateCode == "27 MAHARASHTRA")
            {
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

                var Gstamt = Convert.ToDecimal(IGST) / 2;

                table.SetWidths(new float[] { 0f, 76f, 12f });
                table.AddCell(paragraph);
                PdfPCell cell2 = new PdfPCell(new Phrase("CGST 9%", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                cell2.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell2);
                PdfPCell cell3 = new PdfPCell(new Phrase(CGST.ToString(), FontFactory.GetFont("Arial", 10, Font.BOLD)));
                //PdfPCell cell3 = new PdfPCell(new Phrase(Gstamt.ToString(), FontFactory.GetFont("Arial", 10, Font.BOLD)));
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
                PdfPCell cell22 = new PdfPCell(new Phrase("SGST 9%", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                cell22.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell22);
                PdfPCell cell33 = new PdfPCell(new Phrase(SGST.ToString(), FontFactory.GetFont("Arial", 10, Font.BOLD)));
                //PdfPCell cell33 = new PdfPCell(new Phrase(Gstamt.ToString(), FontFactory.GetFont("Arial", 10, Font.BOLD)));
                cell33.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell33);

                doc.Add(table);
                //SGST 9% Row End
            }
            else
            {
                //new for i gst
                table = new PdfPTable(3);
                table.TotalWidth = 560f;
                table.LockedWidth = true;

                table.SetWidths(new float[] { 0f, 76f, 12f });
                table.AddCell(paragraph);
                PdfPCell cell222 = new PdfPCell(new Phrase("IGST 18%", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                cell222.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell222);
                PdfPCell cell333 = new PdfPCell(new Phrase(IGST.ToString(), FontFactory.GetFont("Arial", 10, Font.BOLD)));
                //PdfPCell cell33 = new PdfPCell(new Phrase(Gstamt.ToString(), FontFactory.GetFont("Arial", 10, Font.BOLD)));
                cell333.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell333);

                doc.Add(table);
                //new for i gst
            }

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


            table.SetWidths(new float[] { 0f, 76f, 12f });
            table.AddCell(paragraph);
            PdfPCell cell44 = new PdfPCell(new Phrase("Grand Total", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            cell44.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cell44);

            PdfPCell cell55 = new PdfPCell(new Phrase(GrandTotal, FontFactory.GetFont("Arial", 10, Font.BOLD)));
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
            cn.Rectangle(17f, 150f, 560f, 75f);
            cn.Stroke();

            // Header 
            cn.BeginText();

            cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibri.ttf", "Identity-H", BaseFont.EMBEDDED), 9);
            cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "I / we certify that our registration certficate under the GST Act, 2017 is in force on the date on which the supply of goods specified In This Tax Invoice", 22, 216, 0);
            cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "is made by me / us & the transaction of supply covered by this Tax Invoice had been effected by me / us & it shall be accounted for in the turnover of", 22, 204, 0);
            cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "supplies while filling of return &the due tax if any payable on the supplies has been paid or shall be paid. Further certified that the particulars given", 22, 193, 0);
            cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "above are true and correct & the amount indicated represents the prices actually charged and that there is no flow additional consideration directly or ", 22, 181, 0);
            cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "indirectly from the buyer. Interest @ 18% p.a. charged on all outstanding more than one month after invoice has been rendered", 22, 170, 0);

            cn.EndText();

            //---start
            PdfContentByte cnn = writer.DirectContent;
            cnn.Rectangle(17f, 15f, 560f, 135f);
            cnn.Stroke();

            // Header 
            cnn.BeginText();

            cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 12);
            cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Terms and Conditions :", 30, 138, 0);

            cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
            cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "* Subject To Pune Jurisdiction Only * ", 30, 125, 0);

            cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibri.ttf", "Identity-H", BaseFont.EMBEDDED), 9);
            cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Payment to be made by A/c. Payee Cheque Only.", 30, 110, 0);
            cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Interest @18 % will be charged on bill not paid within due date.", 30, 98, 0);

            cnn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 14);
            cnn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Bank Details :", 30, 83, 0);
            cnn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibri.ttf", "Identity-H", BaseFont.EMBEDDED), 12);
            cnn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Bank Name : BANK OF BARODA", 30, 69, 0);
            cnn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibri.ttf", "Identity-H", BaseFont.EMBEDDED), 12);
            cnn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Account Name : ENDEAVOUR AUTOMATION", 30, 57, 0);
            cnn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibri.ttf", "Identity-H", BaseFont.EMBEDDED), 12);
            cnn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Branch : KALEWADI", 30, 45, 0);
            cnn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibri.ttf", "Identity-H", BaseFont.EMBEDDED), 12);

            cnn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "A/c No : 46180200000214", 30, 33, 0);
            cnn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibri.ttf", "Identity-H", BaseFont.EMBEDDED), 12);
            cnn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "IFSC/Neft Code :BARB0KALEWA", 30, 20, 0);

            cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 12);
            cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "For", 392, 125, 0);
            cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 13);
            cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "ENDEAVOUR AUTOMATION", 413, 125, 0);
            cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 13);
            cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Authorised Signatory", 443, 40, 0);

            //cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 11);
            //cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " Subject To Pune Jurisdiction Only " , 30, 215, 0);

            //cnn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 14);
            //cnn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Bank Details :", 30, 230, 0);
            //cnn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibri.ttf", "Identity-H", BaseFont.EMBEDDED), 12);
            //cnn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Bank Name : BANK OF BARODA", 30, 200, 0);
            //cnn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibri.ttf", "Identity-H", BaseFont.EMBEDDED), 12);
            //cnn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Account Name : ENDEAVOUR AUTOMATION", 30, 190, 0);
            //cnn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibri.ttf", "Identity-H", BaseFont.EMBEDDED), 12);
            //cnn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Branch : KALEWADI", 30, 177, 0);
            //cnn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibri.ttf", "Identity-H", BaseFont.EMBEDDED), 12);
            //cnn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "A/c No : 46180200000214", 30, 164, 0);
            //cnn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibri.ttf", "Identity-H", BaseFont.EMBEDDED), 12);
            //cnn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "IFSC/Neft Code :BARB0KALEWA", 30, 148, 0);
            //cnn
            //cnncn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibrii.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
            //cnncn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "We Thank you for your enquiry", 440, 256, 0);
            //cnn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 12);
            //cnn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "For", 392, 230, 0);
            //cnn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 13);
            //cnn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "ENDEAVOUR AUTOMATION", 413, 230, 0);
            //cnn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 13);
            //cnn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Authorised Signatory", 443, 180, 0);
            cnn.EndText();
            //---end



            Paragraph paragraphTable4 = new Paragraph();

            paragraphTable4.SpacingBefore = 10f;

            table = new PdfPTable(2);
            table.TotalWidth = 560f;

            float[] widths = new float[] { 160f, 400f };
            table.SetWidths(widths);
            table.LockedWidth = true;

            doc.Close();

            //---

            //-----


            Byte[] FileBuffer = File.ReadAllBytes(Server.MapPath("~/Files/") + "TaxInvoice.pdf");
            //string empFilename = "TaxInvoice" + DateTime.Now.ToShortDateString() + ".pdf";

            //if (FileBuffer != null)
            //{
            //    Response.ContentType = "application/pdf";
            //    Response.AddHeader("content-length", FileBuffer.Length.ToString());
            //    Response.BinaryWrite(FileBuffer);
            //    Response.AddHeader("Content-Disposition", "attachment;filename=" + empFilename);
            //}

        }
        doc.Close();
        ifrRight6.Attributes["src"] = @"../Files/" + "TaxInvoice.pdf";
    }

    protected void btnprint_Click(object sender, EventArgs e)
    {
        try
        {
            id = Decrypt(Request.QueryString["Id"].ToString());
            DataTable Dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("SELECT tbl_Invoice_both_hdr.Id,tbl_Invoice_both_hdr.InvoiceNo,InvoiceDate,PoNo,PoDate,CustName,ChallanNo,ChallanDate,PayTerm," +
                "Delivery,KindAtt,CompanyAddress,CompanyGstNo,CompanyPanNo, ComapyRegType, CompanyStateCode, " +
                "CustomerShippingAddress, CustomerGstNo, CustomerPanNo, CustomerRegType,CustomerStateCode, CGST, SGST,IGST," +
                " AllTotalAmount, GrandTotal, TotalInWord, Description,printdescription, Hsn, TaxPercentage, Quntity, Unit, Rate, DiscountPercentage," +
                " Total,Term_Condition_1,Term_Condition_2,Term_Condition_3,Term_Condition_4,Term_Condition_5,Term_Condition_6,CreatedOn FROM tbl_Invoice_both_hdr INNER JOIN tbl_Invoice_both_Dtls ON tbl_Invoice_both_Dtls.InvoiceId = tbl_Invoice_both_hdr.Id WHERE tbl_Invoice_both_hdr.Id='" + id + "'", Conn);
            da.Fill(Dt);
            StringWriter sw = new StringWriter();
            StringReader sr = new StringReader(sw.ToString());

            Document doc = new Document(PageSize.A4, 10f, 10f, 55f, 0f);

            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(Server.MapPath("~/Files/") + "TaxInvoice.pdf", FileMode.Create));
            //PdfWriter writer = PdfWriter.GetInstance(doc, Response.OutputStream);
            iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, sr);
            string imageURL = Server.MapPath("~") + "/image/AA.png";

            iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(imageURL);

            //Resize image depend upon your need
            doc.Open();
            png.ScaleToFit(70, 100);

            //For Image Position
            png.SetAbsolutePosition(40, 745);
            //var document = new Document();

            //Give space before image
            //png.ScaleToFit(document.PageSize.Width - (document.RightMargin * 100), 50);
            png.SpacingBefore = 50f;

            //Give some space after the image

            png.SpacingAfter = 1f;

            png.Alignment = Element.ALIGN_LEFT;

            //paragraphimage.Add(png);
            //doc.Add(paragraphimage);
            doc.Add(png);


            if (Chk_ORIGINAL.Checked == true)
            {
                #region  Pdf For Oroginal


                PdfContentByte cb = writer.DirectContent;
                cb.Rectangle(17f, 735f, 560f, 60f);
                cb.Stroke();
                // Header 
                cb.BeginText();
                cb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 20);
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "ENDEAVOUR AUTOMATION", 180, 773, 0);
                cb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 11);
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "S.N.9/2/A1,Sakpal Wasti,Behind Genesis furniture,Nr.HP Petrol Pump,Ravet Road", 145, 755, 0);
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

                //-------------------
                PdfContentByte cdd = writer.DirectContent;
                cdd.Rectangle(17f, 660f, 560f, 25f);
                cdd.Stroke();
                cdd.BeginText();

                cb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "ORIGINAL FOR BUYER", 480, 800, 0);

                cdd.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 17);
                cdd.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Tax Invoice", 260, 667, 0);
                cdd.EndText();
                //--------------


                if (Dt.Rows.Count > 0)
                {
                    var CreateDate = DateTime.Now.ToString("dd-MM-yyyy");
                    string CustName = Dt.Rows[0]["CustName"].ToString();
                    string InvoiceNo = Dt.Rows[0]["InvoiceNo"].ToString();
                    string PoDate = Dt.Rows[0]["PoDate"].ToString().TrimEnd("0:0".ToCharArray());
                    string PoNumber = Dt.Rows[0]["PoNo"].ToString();
                    string CompanyAddress = Dt.Rows[0]["CompanyAddress"].ToString();
                    string Address = Dt.Rows[0]["CustomerShippingAddress"].ToString();
                    string InvoiceDate = Dt.Rows[0]["InvoiceDate"].ToString().TrimEnd("0:0".ToCharArray());
                    string ChallanNo = Dt.Rows[0]["ChallanNo"].ToString();
                    string GSTNo = Dt.Rows[0]["CustomerGstNo"].ToString();
                    string KindAtt = Dt.Rows[0]["KindAtt"].ToString();
                    string TotalInWord = Dt.Rows[0]["TotalInWord"].ToString();
                    string GrandTotal = Dt.Rows[0]["GrandTotal"].ToString();
                    string CGST = Dt.Rows[0]["CGST"].ToString();
                    string SGST = Dt.Rows[0]["SGST"].ToString();
                    //string IGST = Dt.Rows[0]["IGST"].ToString();
                    string Total = Dt.Rows[0]["AllTotalAmount"].ToString();
                    string CompanyStateCode = Dt.Rows[0]["CompanyStateCode"].ToString();


                    string str = Dt.Rows[0]["Term_Condition_1"].ToString();
                    string str1 = Dt.Rows[0]["Term_Condition_2"].ToString();
                    string str2 = Dt.Rows[0]["Term_Condition_3"].ToString();
                    string str3 = Dt.Rows[0]["Term_Condition_4"].ToString();
                    string str4 = Dt.Rows[0]["Term_Condition_5"].ToString();
                    string str5 = Dt.Rows[0]["Term_Condition_6"].ToString();
                    string[] arrstr = str.ToString().Split('-');
                    string[] arrstr1 = str1.ToString().Split('-');
                    string[] arrstr2 = str2.ToString().Split('-');
                    string[] arrstr3 = str3.ToString().Split('-');
                    string[] arrstr4 = str4.ToString().Split('-');
                    string[] arrstr5 = str5.ToString().Split('-');


                    decimal IGST = Convert.ToDecimal(Dt.Rows[0]["IGST"]);
                    decimal GSTAmt = IGST / 2;



                    Paragraph paragraphTable1 = new Paragraph();
                    paragraphTable1.SpacingBefore = 120f;
                    paragraphTable1.SpacingAfter = 10f;

                    PdfPTable table = new PdfPTable(4);

                    float[] widths2 = new float[] { 100, 180, 100, 180 };
                    table.SetWidths(widths2);
                    table.TotalWidth = 560f;
                    table.LockedWidth = true;

                    DateTime ffff1 = Convert.ToDateTime(Dt.Rows[0]["InvoiceDate"].ToString());
                    string datee = ffff1.ToString("dd-MM-yyyy");

                    DateTime ffff2 = Convert.ToDateTime(Dt.Rows[0]["PoDate"].ToString());
                    string podate = ffff1.ToString("dd-MM-yyyy");


                    table.AddCell(new Phrase("Buyer : ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(CustName, FontFactory.GetFont("Arial", 9, Font.BOLD)));

                    table.AddCell(new Phrase("Consignee :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(CustName, FontFactory.GetFont("Arial", 9, Font.BOLD)));

                    table.AddCell(new Phrase("Shipping Address : ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(CompanyAddress, FontFactory.GetFont("Arial", 9, Font.BOLD)));

                    table.AddCell(new Phrase("Billing Address : ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(Address, FontFactory.GetFont("Arial", 9, Font.BOLD)));

                    table.AddCell(new Phrase("Invoice Number : ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(InvoiceNo, FontFactory.GetFont("Arial", 9, Font.BOLD)));

                    table.AddCell(new Phrase("Invoice Date :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(datee, FontFactory.GetFont("Arial", 9, Font.BOLD)));

                    //table.AddCell(new Phrase("Address", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    //table.AddCell(new Phrase(Address, FontFactory.GetFont("Arial", 9, Font.BOLD)));


                    table.AddCell(new Phrase("P.O. Number :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(PoNumber, FontFactory.GetFont("Arial", 9, Font.BOLD)));

                    table.AddCell(new Phrase("P.O. Date :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(podate, FontFactory.GetFont("Arial", 9, Font.BOLD)));

                    table.AddCell(new Phrase("Challan No. :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(ChallanNo, FontFactory.GetFont("Arial", 9, Font.BOLD)));

                    table.AddCell(new Phrase("GST No. :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
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
                            table.AddCell(new Phrase(dr["printdescription"].ToString(), FontFactory.GetFont("Arial", 9)));
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

                    if (Dt.Rows.Count >= 10)
                    {
                        table.AddCell(new Phrase("  \n\n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                        //doc.Add(table);
                    }
                    else if (Dt.Rows.Count >= 7 && Dt.Rows.Count <= 9)
                    {
                        table.AddCell(new Phrase("  \n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));

                    }
                    else if (Dt.Rows.Count >= 4 && Dt.Rows.Count <= 6)
                    {
                        table.AddCell(new Phrase("  \n\n\n\n\n\n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));

                    }
                    else if (Dt.Rows.Count < 4)
                    {

                        table.AddCell(new Phrase("  \n\n\n\n\n\n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));

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
                    table.AddCell(paragraph);
                    PdfPCell cell = new PdfPCell(new Phrase("Sub Total", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(cell);
                    PdfPCell cell11 = new PdfPCell(new Phrase(Total, FontFactory.GetFont("Arial", 10, Font.BOLD)));
                    cell11.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(cell11);


                    doc.Add(table);
                    //add total row end


                    if (CompanyStateCode == "27 MAHARASHTRA")
                    {
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

                        var Gstamt = Convert.ToDecimal(IGST) / 2;

                        table.SetWidths(new float[] { 0f, 76f, 12f });
                        table.AddCell(paragraph);
                        PdfPCell cell2 = new PdfPCell(new Phrase("CGST 9%", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                        cell2.HorizontalAlignment = Element.ALIGN_RIGHT;
                        table.AddCell(cell2);
                        PdfPCell cell3 = new PdfPCell(new Phrase(CGST.ToString(), FontFactory.GetFont("Arial", 10, Font.BOLD)));
                        //PdfPCell cell3 = new PdfPCell(new Phrase(Gstamt.ToString(), FontFactory.GetFont("Arial", 10, Font.BOLD)));
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
                        PdfPCell cell22 = new PdfPCell(new Phrase("SGST 9%", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                        cell22.HorizontalAlignment = Element.ALIGN_RIGHT;
                        table.AddCell(cell22);
                        PdfPCell cell33 = new PdfPCell(new Phrase(SGST.ToString(), FontFactory.GetFont("Arial", 10, Font.BOLD)));
                        //PdfPCell cell33 = new PdfPCell(new Phrase(Gstamt.ToString(), FontFactory.GetFont("Arial", 10, Font.BOLD)));
                        cell33.HorizontalAlignment = Element.ALIGN_RIGHT;
                        table.AddCell(cell33);

                        doc.Add(table);
                        //SGST 9% Row End
                    }
                    else
                    {
                        //new for i gst
                        table = new PdfPTable(3);
                        table.TotalWidth = 560f;
                        table.LockedWidth = true;

                        table.SetWidths(new float[] { 0f, 76f, 12f });
                        table.AddCell(paragraph);
                        PdfPCell cell222 = new PdfPCell(new Phrase("IGST 18%", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                        cell222.HorizontalAlignment = Element.ALIGN_RIGHT;
                        table.AddCell(cell222);
                        PdfPCell cell333 = new PdfPCell(new Phrase(IGST.ToString(), FontFactory.GetFont("Arial", 10, Font.BOLD)));
                        //PdfPCell cell33 = new PdfPCell(new Phrase(Gstamt.ToString(), FontFactory.GetFont("Arial", 10, Font.BOLD)));
                        cell333.HorizontalAlignment = Element.ALIGN_RIGHT;
                        table.AddCell(cell333);

                        doc.Add(table);
                        //new for i gst
                    }

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


                    table.SetWidths(new float[] { 0f, 76f, 12f });
                    table.AddCell(paragraph);
                    PdfPCell cell44 = new PdfPCell(new Phrase("Grand Total", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                    cell44.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(cell44);

                    PdfPCell cell55 = new PdfPCell(new Phrase(GrandTotal, FontFactory.GetFont("Arial", 10, Font.BOLD)));
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


                    ////new code start 22-08-02024

                    string[] items90termm = { "Dear Sir/Madam,\b", "We acknowledge with thanks the receipt of your above enquiry and have great pleasure in submitting our lowest quotation,\b", "subject to the conditions printed below.\n" };

                    Font font12112termm = FontFactory.GetFont("Arial", 8);
                    Font font10111termm = FontFactory.GetFont("Arial", 8);
                    Font fontWithSize = FontFactory.GetFont(FontFactory.HELVETICA, 10);
                    // Paragraph paragraph110term = new Paragraph("\bTerms & Conditions:", font12112term);


                    //Paragraph paragraph110termm = new Paragraph("I / we certify that our registration certficate under the GST Act, 2017 is in force on the date on which the supply of goods specified In This Tax Invoice made by me / us & the transaction of supply covered by this Tax Invoice had been effected by me / us & it shall be accounted for in the turnover ofsupplies while filling of return &the due tax if any payable on the supplies has been paid or shall be paid.Further certified that the particulars givenabove are true and correct &the amount indicated represents the prices actually charged and that there is no flow additional consideration directly orindirectly from the buyer. Interest @ 18 % p.a.charged on all outstanding more than one month after invoice has been rendered", fontWithSize);

                    // Create the full paragraph
                    Paragraph paragraph110termm = new Paragraph("I / we certify that our registration certificate under the GST Act, 2017 is in force on the date on which the supply of goods specified in this tax invoice made by me / us & the transaction of supply covered by this tax invoice had been effected by me / us & it shall be accounted for in the turnover of supplies while filling of return & the due tax if any payable on the supplies has been paid or shall be paid. Further certified that the particulars given above are true and correct & the amount indicated represents the prices actually charged and that there is no flow of additional consideration directly or indirectly from the buyer. ", fontWithSize);

                    // Create a Chunk with the text to be bolded and apply bold font
                    Font boldFont = new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.BOLD);
                    Chunk boldChunk = new Chunk("Interest @ 18% p.a. charged on all outstanding more than one month after invoice has been rendered.", boldFont);

                    // Add the bold chunk to the paragraph
                    paragraph110termm.Add(boldChunk);

                    for (int i = 0; i < items90termm.Length; i++)
                    {
                        paragraph110termm.Add(new Phrase(" ", fontWithSize));
                    }

                    table = new PdfPTable(1);
                    table.TotalWidth = 560f;
                    table.LockedWidth = true;
                    table.SetWidths(new float[] { 150f });

                    table.AddCell(paragraph110termm);
                    table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10)));
                    table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 11, Font.BOLD)));

                    doc.Add(table);

                    ///////////term And Condition

                    string[] items90term = { "Dear Sir/Madam,\b", "We acknowledge with thanks the receipt of your above enquiry and have great pleasure in submitting our lowest quotation,\b", "subject to the conditions printed below.\n" };

                    Font font12112term = FontFactory.GetFont("Arial", 12, Font.BOLD);
                    Font font10111term = FontFactory.GetFont("Arial", 10);
                    Paragraph paragraph110term = new Paragraph("\bTerms & Conditions:", font12112term);

                    for (int i = 0; i < items90term.Length; i++)
                    {
                        paragraph110term.Add(new Phrase(" ", font10111term));
                    }

                    table = new PdfPTable(1);
                    table.TotalWidth = 560f;
                    table.LockedWidth = true;
                    table.SetWidths(new float[] { 150f });

                    table.AddCell(paragraph110term);
                    //table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10)));
                    //table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 11, Font.BOLD)));

                    doc.Add(table);

                    /////////////////////////////////////////////////term condition in Database

                    string[] items90termdata = { "Dear Sir/Madam,\b", "We acknowledge with thanks the receipt of your above enquiry and have great pleasure in submitting our lowest quotation,\b", "subject to the conditions printed below.\n" };

                    Font font12112termdata = FontFactory.GetFont("Arial", 12, Font.BOLD);
                    Font font10111termdata = FontFactory.GetFont("Arial", 10);
                    Paragraph paragraph110termdata = new Paragraph("", font12112termdata);

                    for (int i = 0; i < items90termdata.Length; i++)
                    {
                        paragraph110termdata.Add(new Phrase(" ", font10111termdata));
                    }

                    table = new PdfPTable(2);
                    table.TotalWidth = 560f;
                    table.LockedWidth = true;
                    table.SetWidths(new float[] { 150f, 350f });

                    table.AddCell(new Phrase(arrstr[0].ToString(), FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(arrstr[1].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(arrstr1[0].ToString(), FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(arrstr1[1].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(arrstr2[0].ToString(), FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(arrstr2[1].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(arrstr3[0].ToString(), FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(arrstr3[1].ToString(), FontFactory.GetFont("Arial", 9)));

                    table.AddCell(new Phrase(arrstr4[0].ToString(), FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(arrstr4[1].ToString(), FontFactory.GetFont("Arial", 9)));

                    table.AddCell(new Phrase(arrstr5[0].ToString(), FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(arrstr5[1].ToString(), FontFactory.GetFont("Arial", 9)));

                    doc.Add(table);

                    ///////////Add Bank details Titles

                    string[] items90 = { "Dear Sir/Madam,\b", "We acknowledge with thanks the receipt of your above enquiry and have great pleasure in submitting our lowest quotation,\b", "subject to the conditions printed below.\n" };

                    Font font12112 = FontFactory.GetFont("Arial", 12, Font.BOLD);
                    Font font10111 = FontFactory.GetFont("Arial", 10);
                    Paragraph paragraph110 = new Paragraph("\bBank Account Details", font12112);

                    for (int i = 0; i < items90.Length; i++)
                    {
                        paragraph110.Add(new Phrase(" ", font10111));
                    }

                    table = new PdfPTable(1);
                    table.TotalWidth = 560f;
                    table.LockedWidth = true;
                    table.SetWidths(new float[] { 310f });

                    table.AddCell(paragraph110);
                    doc.Add(table);
                    /////////////////////////////////////////////////
                    string[] items9 = { "Dear Sir/Madam,\b", "We acknowledge with thanks the receipt of your above enquiry and have great pleasure in submitting our lowest quotation,\b", "subject to the conditions printed below.\n" };

                    Font font1211 = FontFactory.GetFont("Arial", 12, Font.BOLD);
                    Font font1011 = FontFactory.GetFont("Arial", 10);
                    Paragraph paragraph11 = new Paragraph("", font1211);

                    for (int i = 0; i < items9.Length; i++)
                    {
                        paragraph11.Add(new Phrase(" ", font1011));
                    }

                    table = new PdfPTable(2);
                    table.TotalWidth = 560f;
                    table.LockedWidth = true;
                    table.SetWidths(new float[] { 310f, 250f });

                    //table.AddCell(paragraph11);
                    table.AddCell(new Phrase("\bBank Name : BANK OF BARODA\n\n\bAccount Name : ENDEAVOUR AUTOMATION\n\n\bBranch : KALEWADI\n\n\bA/c No. : 46180200000214\n\n\bIFSC/Neft Code :BARB0KALEWA\n", FontFactory.GetFont("Arial", 10)));
                    table.AddCell(new Phrase("\b      For,\n\n\b\b\b\b\b\b\b\b\b            ENDEAVOUR AUTOMATION\n\n\n\n\n                                       Authorised Signatory", FontFactory.GetFont("Arial", 11, Font.BOLD)));

                    doc.Add(table);
                    /////////////////////////////
                    PdfPCell celll = null;
                    Paragraph paragraphTable4 = new Paragraph();
                    paragraphTable4.SpacingBefore = 10f;
                    table = new PdfPTable(1);
                    table.TotalWidth = 560f;
                    float[] widths = new float[] { 560f };
                    table.SetWidths(widths);
                    table.LockedWidth = true;
                    cell = new PdfPCell(new Phrase("                                                                                           *Subject to Pune Jurisdiction Only*", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    cell.Border = Rectangle.NO_BORDER;
                    table.AddCell(cell);
                    doc.Add(table);
                    // doc.Close();
                    /////////new code //////////14 End
                    doc.NewPage();
                }

                #endregion
            }

            if (Chk_DUPLICATE.Checked == true)
            {
                #region  Pdf For Duplicate
                //string id = Session["PDFID"].ToString();

                png.ScaleToFit(70, 100);

                //For Image Position
                png.SetAbsolutePosition(40, 745);
                //var document = new Document();

                //Give space before image
                //png.ScaleToFit(document.PageSize.Width - (document.RightMargin * 100), 50);
                png.SpacingBefore = 50f;

                //Give some space after the image

                png.SpacingAfter = 1f;

                png.Alignment = Element.ALIGN_LEFT;

                //paragraphimage.Add(png);
                //doc.Add(paragraphimage);
                doc.Add(png);

                PdfContentByte cb1 = writer.DirectContent;
                cb1.Rectangle(17f, 735f, 560f, 60f);
                cb1.Stroke();
                // Header 
                cb1.BeginText();
                cb1.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 20);
                cb1.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "ENDEAVOUR AUTOMATION", 180, 773, 0);
                cb1.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 11);
                cb1.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "S.N.9/2/A1,Sakpal Wasti,Behind Genesis furniture,Nr.HP Petrol Pump,Ravet Road", 145, 755, 0);
                cb1.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 11);
                cb1.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Tathawade, Pune-411033", 227, 740, 0);
                cb1.EndText();

                PdfContentByte cbb1 = writer.DirectContent;
                cbb1.Rectangle(17f, 710f, 560f, 25f);
                cbb1.Stroke();
                // Header 
                cbb1.BeginText();
                cbb1.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
                cbb1.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " Mob: 9860502108    Email: endeavour.automations@gmail.com ", 153, 722, 0);
                cbb1.EndText();

                PdfContentByte cbbb1 = writer.DirectContent;
                cbbb1.Rectangle(17f, 685f, 560f, 25f);
                cbbb1.Stroke();
                // Header 
                cbbb1.BeginText();
                cbbb1.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
                cbbb1.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "GSTIN :  27AFYPJ3488G1ZQ ", 30, 695, 0);
                cbbb1.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
                cbbb1.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "W.E.F. :  01/07/2017", 185, 695, 0);
                cbbb1.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
                cbbb1.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "PAN No. :  AFYPJ3488G", 310, 695, 0);
                cbbb1.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
                cbbb1.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "State Code :  27 Maharashtra", 440, 695, 0);
                cbbb1.EndText();

                //-------------------
                PdfContentByte cdd1 = writer.DirectContent;
                cdd1.Rectangle(17f, 660f, 560f, 25f);
                cdd1.Stroke();
                cdd1.BeginText();

                cb1.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
                cb1.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "DUPLICATE FOR BUYER", 480, 800, 0);

                cdd1.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 17);
                cdd1.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Tax Invoice", 260, 667, 0);
                cdd1.EndText();
                //--------------



                if (Dt.Rows.Count > 0)
                {
                    var CreateDate = DateTime.Now.ToString("dd-MM-yyyy");
                    string CustName = Dt.Rows[0]["CustName"].ToString();
                    string InvoiceNo = Dt.Rows[0]["InvoiceNo"].ToString();
                    string PoDate = Dt.Rows[0]["PoDate"].ToString().TrimEnd("0:0".ToCharArray());
                    string PoNumber = Dt.Rows[0]["PoNo"].ToString();
                    string CompanyAddress = Dt.Rows[0]["CompanyAddress"].ToString();
                    string Address = Dt.Rows[0]["CustomerShippingAddress"].ToString();
                    string InvoiceDate = Dt.Rows[0]["InvoiceDate"].ToString().TrimEnd("0:0".ToCharArray());
                    string ChallanNo = Dt.Rows[0]["ChallanNo"].ToString();
                    string GSTNo = Dt.Rows[0]["CustomerGstNo"].ToString();
                    string KindAtt = Dt.Rows[0]["KindAtt"].ToString();
                    string TotalInWord = Dt.Rows[0]["TotalInWord"].ToString();
                    string GrandTotal = Dt.Rows[0]["GrandTotal"].ToString();
                    string CGST = Dt.Rows[0]["CGST"].ToString();
                    string SGST = Dt.Rows[0]["SGST"].ToString();
                    //string IGST = Dt.Rows[0]["IGST"].ToString();
                    string Total = Dt.Rows[0]["AllTotalAmount"].ToString();
                    string CompanyStateCode = Dt.Rows[0]["CompanyStateCode"].ToString();


                    decimal IGST = Convert.ToDecimal(Dt.Rows[0]["IGST"]);
                    decimal GSTAmt = IGST / 2;


                    string str = Dt.Rows[0]["Term_Condition_1"].ToString();
                    string str1 = Dt.Rows[0]["Term_Condition_2"].ToString();
                    string str2 = Dt.Rows[0]["Term_Condition_3"].ToString();
                    string str3 = Dt.Rows[0]["Term_Condition_4"].ToString();
                    string str4 = Dt.Rows[0]["Term_Condition_5"].ToString();
                    string str5 = Dt.Rows[0]["Term_Condition_6"].ToString();
                    string[] arrstr = str.ToString().Split('-');
                    string[] arrstr1 = str1.ToString().Split('-');
                    string[] arrstr2 = str2.ToString().Split('-');
                    string[] arrstr3 = str3.ToString().Split('-');
                    string[] arrstr4 = str4.ToString().Split('-');
                    string[] arrstr5 = str5.ToString().Split('-');



                    Paragraph paragraphTable1 = new Paragraph();
                    paragraphTable1.SpacingBefore = 120f;
                    paragraphTable1.SpacingAfter = 10f;

                    PdfPTable table = new PdfPTable(4);

                    float[] widths2 = new float[] { 100, 180, 100, 180 };
                    table.SetWidths(widths2);
                    table.TotalWidth = 560f;
                    table.LockedWidth = true;

                    DateTime ffff1 = Convert.ToDateTime(Dt.Rows[0]["InvoiceDate"].ToString());
                    string datee = ffff1.ToString("dd-MM-yyyy");

                    DateTime ffff2 = Convert.ToDateTime(Dt.Rows[0]["PoDate"].ToString());
                    string podate = ffff1.ToString("dd-MM-yyyy");


                    table.AddCell(new Phrase("Buyer : ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(CustName, FontFactory.GetFont("Arial", 9, Font.BOLD)));

                    table.AddCell(new Phrase("Consignee :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(CustName, FontFactory.GetFont("Arial", 9, Font.BOLD)));

                    table.AddCell(new Phrase("Shipping Address : ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(CompanyAddress, FontFactory.GetFont("Arial", 9, Font.BOLD)));

                    table.AddCell(new Phrase("Billing Address : ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(Address, FontFactory.GetFont("Arial", 9, Font.BOLD)));

                    table.AddCell(new Phrase("Invoice Number : ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(InvoiceNo, FontFactory.GetFont("Arial", 9, Font.BOLD)));

                    table.AddCell(new Phrase("Invoice Date :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(datee, FontFactory.GetFont("Arial", 9, Font.BOLD)));

                    //table.AddCell(new Phrase("Address", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    //table.AddCell(new Phrase(Address, FontFactory.GetFont("Arial", 9, Font.BOLD)));


                    table.AddCell(new Phrase("P.O. Number :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(PoNumber, FontFactory.GetFont("Arial", 9, Font.BOLD)));

                    table.AddCell(new Phrase("P.O. Date :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(podate, FontFactory.GetFont("Arial", 9, Font.BOLD)));

                    table.AddCell(new Phrase("Challan No. :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(ChallanNo, FontFactory.GetFont("Arial", 9, Font.BOLD)));

                    table.AddCell(new Phrase("GST No. :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
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
                            table.AddCell(new Phrase(dr["printdescription"].ToString(), FontFactory.GetFont("Arial", 9)));
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
                        //doc.Add(table);
                    }
                    else if (Dt.Rows.Count >= 7 && Dt.Rows.Count <= 9)
                    {
                        table.AddCell(new Phrase("  \n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));

                    }
                    else if (Dt.Rows.Count >= 4 && Dt.Rows.Count <= 6)
                    {
                        table.AddCell(new Phrase("  \n\n\n\n\n\n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));

                    }
                    else if (Dt.Rows.Count < 4)
                    {

                        table.AddCell(new Phrase("  \n\n\n\n\n\n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));

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
                    table.AddCell(paragraph);
                    PdfPCell cell = new PdfPCell(new Phrase("Sub Total", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(cell);
                    PdfPCell cell11 = new PdfPCell(new Phrase(Total, FontFactory.GetFont("Arial", 10, Font.BOLD)));
                    cell11.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(cell11);


                    doc.Add(table);
                    //add total row end


                    if (CompanyStateCode == "27 MAHARASHTRA")
                    {
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

                        var Gstamt = Convert.ToDecimal(IGST) / 2;

                        table.SetWidths(new float[] { 0f, 76f, 12f });
                        table.AddCell(paragraph);
                        PdfPCell cell2 = new PdfPCell(new Phrase("CGST 9%", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                        cell2.HorizontalAlignment = Element.ALIGN_RIGHT;
                        table.AddCell(cell2);
                        PdfPCell cell3 = new PdfPCell(new Phrase(CGST.ToString(), FontFactory.GetFont("Arial", 10, Font.BOLD)));
                        //PdfPCell cell3 = new PdfPCell(new Phrase(Gstamt.ToString(), FontFactory.GetFont("Arial", 10, Font.BOLD)));
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
                        PdfPCell cell22 = new PdfPCell(new Phrase("SGST 9%", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                        cell22.HorizontalAlignment = Element.ALIGN_RIGHT;
                        table.AddCell(cell22);
                        PdfPCell cell33 = new PdfPCell(new Phrase(SGST.ToString(), FontFactory.GetFont("Arial", 10, Font.BOLD)));
                        //PdfPCell cell33 = new PdfPCell(new Phrase(Gstamt.ToString(), FontFactory.GetFont("Arial", 10, Font.BOLD)));
                        cell33.HorizontalAlignment = Element.ALIGN_RIGHT;
                        table.AddCell(cell33);

                        doc.Add(table);
                        //SGST 9% Row End
                    }
                    else
                    {
                        //new for i gst
                        table = new PdfPTable(3);
                        table.TotalWidth = 560f;
                        table.LockedWidth = true;

                        table.SetWidths(new float[] { 0f, 76f, 12f });
                        table.AddCell(paragraph);
                        PdfPCell cell222 = new PdfPCell(new Phrase("IGST 18%", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                        cell222.HorizontalAlignment = Element.ALIGN_RIGHT;
                        table.AddCell(cell222);
                        PdfPCell cell333 = new PdfPCell(new Phrase(IGST.ToString(), FontFactory.GetFont("Arial", 10, Font.BOLD)));
                        //PdfPCell cell33 = new PdfPCell(new Phrase(Gstamt.ToString(), FontFactory.GetFont("Arial", 10, Font.BOLD)));
                        cell333.HorizontalAlignment = Element.ALIGN_RIGHT;
                        table.AddCell(cell333);

                        doc.Add(table);
                        //new for i gst
                    }

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


                    table.SetWidths(new float[] { 0f, 76f, 12f });
                    table.AddCell(paragraph);
                    PdfPCell cell44 = new PdfPCell(new Phrase("Grand Total", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                    cell44.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(cell44);

                    PdfPCell cell55 = new PdfPCell(new Phrase(GrandTotal, FontFactory.GetFont("Arial", 10, Font.BOLD)));
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


                    ////new code start 22-08-02024

                    string[] items90termm = { "Dear Sir/Madam,\b", "We acknowledge with thanks the receipt of your above enquiry and have great pleasure in submitting our lowest quotation,\b", "subject to the conditions printed below.\n" };

                    Font font12112termm = FontFactory.GetFont("Arial", 8);
                    Font font10111termm = FontFactory.GetFont("Arial", 8);
                    Font fontWithSize = FontFactory.GetFont(FontFactory.HELVETICA, 10);
                    // Paragraph paragraph110term = new Paragraph("\bTerms & Conditions:", font12112term);


                    //Paragraph paragraph110termm = new Paragraph("I / we certify that our registration certficate under the GST Act, 2017 is in force on the date on which the supply of goods specified In This Tax Invoice made by me / us & the transaction of supply covered by this Tax Invoice had been effected by me / us & it shall be accounted for in the turnover ofsupplies while filling of return &the due tax if any payable on the supplies has been paid or shall be paid.Further certified that the particulars givenabove are true and correct &the amount indicated represents the prices actually charged and that there is no flow additional consideration directly orindirectly from the buyer. Interest @ 18 % p.a.charged on all outstanding more than one month after invoice has been rendered", fontWithSize);

                    // Create the full paragraph
                    Paragraph paragraph110termm = new Paragraph("I / we certify that our registration certificate under the GST Act, 2017 is in force on the date on which the supply of goods specified in this tax invoice made by me / us & the transaction of supply covered by this tax invoice had been effected by me / us & it shall be accounted for in the turnover of supplies while filling of return & the due tax if any payable on the supplies has been paid or shall be paid. Further certified that the particulars given above are true and correct & the amount indicated represents the prices actually charged and that there is no flow of additional consideration directly or indirectly from the buyer. ", fontWithSize);

                    // Create a Chunk with the text to be bolded and apply bold font
                    Font boldFont = new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.BOLD);
                    Chunk boldChunk = new Chunk("Interest @ 18% p.a. charged on all outstanding more than one month after invoice has been rendered.", boldFont);

                    // Add the bold chunk to the paragraph
                    paragraph110termm.Add(boldChunk);

                    for (int i = 0; i < items90termm.Length; i++)
                    {
                        paragraph110termm.Add(new Phrase(" ", fontWithSize));
                    }

                    table = new PdfPTable(1);
                    table.TotalWidth = 560f;
                    table.LockedWidth = true;
                    table.SetWidths(new float[] { 150f });

                    table.AddCell(paragraph110termm);
                    table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10)));
                    table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 11, Font.BOLD)));

                    doc.Add(table);

                    ///////////term And Condition

                    string[] items90term = { "Dear Sir/Madam,\b", "We acknowledge with thanks the receipt of your above enquiry and have great pleasure in submitting our lowest quotation,\b", "subject to the conditions printed below.\n" };

                    Font font12112term = FontFactory.GetFont("Arial", 12, Font.BOLD);
                    Font font10111term = FontFactory.GetFont("Arial", 10);
                    Paragraph paragraph110term = new Paragraph("\bTerms & Conditions:", font12112term);

                    for (int i = 0; i < items90term.Length; i++)
                    {
                        paragraph110term.Add(new Phrase(" ", font10111term));
                    }

                    table = new PdfPTable(1);
                    table.TotalWidth = 560f;
                    table.LockedWidth = true;
                    table.SetWidths(new float[] { 150f });

                    table.AddCell(paragraph110term);
                    //table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10)));
                    //table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 11, Font.BOLD)));

                    doc.Add(table);

                    /////////////////////////////////////////////////term condition in Database

                    string[] items90termdata = { "Dear Sir/Madam,\b", "We acknowledge with thanks the receipt of your above enquiry and have great pleasure in submitting our lowest quotation,\b", "subject to the conditions printed below.\n" };

                    Font font12112termdata = FontFactory.GetFont("Arial", 12, Font.BOLD);
                    Font font10111termdata = FontFactory.GetFont("Arial", 10);
                    Paragraph paragraph110termdata = new Paragraph("", font12112termdata);

                    for (int i = 0; i < items90termdata.Length; i++)
                    {
                        paragraph110termdata.Add(new Phrase(" ", font10111termdata));
                    }

                    table = new PdfPTable(2);
                    table.TotalWidth = 560f;
                    table.LockedWidth = true;
                    table.SetWidths(new float[] { 150f, 350f });

                    table.AddCell(new Phrase(arrstr[0].ToString(), FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(arrstr[1].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(arrstr1[0].ToString(), FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(arrstr1[1].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(arrstr2[0].ToString(), FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(arrstr2[1].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(arrstr3[0].ToString(), FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(arrstr3[1].ToString(), FontFactory.GetFont("Arial", 9)));

                    table.AddCell(new Phrase(arrstr4[0].ToString(), FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(arrstr4[1].ToString(), FontFactory.GetFont("Arial", 9)));

                    table.AddCell(new Phrase(arrstr5[0].ToString(), FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(arrstr5[1].ToString(), FontFactory.GetFont("Arial", 9)));

                    doc.Add(table);

                    ///////////Add Bank details Titles

                    string[] items90 = { "Dear Sir/Madam,\b", "We acknowledge with thanks the receipt of your above enquiry and have great pleasure in submitting our lowest quotation,\b", "subject to the conditions printed below.\n" };

                    Font font12112 = FontFactory.GetFont("Arial", 12, Font.BOLD);
                    Font font10111 = FontFactory.GetFont("Arial", 10);
                    Paragraph paragraph110 = new Paragraph("\bBank Account Details", font12112);

                    for (int i = 0; i < items90.Length; i++)
                    {
                        paragraph110.Add(new Phrase(" ", font10111));
                    }

                    table = new PdfPTable(1);
                    table.TotalWidth = 560f;
                    table.LockedWidth = true;
                    table.SetWidths(new float[] { 310f });

                    table.AddCell(paragraph110);
                    doc.Add(table);
                    /////////////////////////////////////////////////
                    string[] items9 = { "Dear Sir/Madam,\b", "We acknowledge with thanks the receipt of your above enquiry and have great pleasure in submitting our lowest quotation,\b", "subject to the conditions printed below.\n" };

                    Font font1211 = FontFactory.GetFont("Arial", 12, Font.BOLD);
                    Font font1011 = FontFactory.GetFont("Arial", 10);
                    Paragraph paragraph11 = new Paragraph("", font1211);

                    for (int i = 0; i < items9.Length; i++)
                    {
                        paragraph11.Add(new Phrase(" ", font1011));
                    }

                    table = new PdfPTable(2);
                    table.TotalWidth = 560f;
                    table.LockedWidth = true;
                    table.SetWidths(new float[] { 310f, 250f });

                    //table.AddCell(paragraph11);
                    table.AddCell(new Phrase("\bBank Name : BANK OF BARODA\n\n\bAccount Name : ENDEAVOUR AUTOMATION\n\n\bBranch : KALEWADI\n\n\bA/c No. : 46180200000214\n\n\bIFSC/Neft Code :BARB0KALEWA\n", FontFactory.GetFont("Arial", 10)));
                    table.AddCell(new Phrase("\b      For,\n\n\b\b\b\b\b\b\b\b\b            ENDEAVOUR AUTOMATION\n\n\n\n\n                                       Authorised Signatory", FontFactory.GetFont("Arial", 11, Font.BOLD)));

                    doc.Add(table);
                    /////////////////////////////
                    PdfPCell celll = null;
                    Paragraph paragraphTable4 = new Paragraph();
                    paragraphTable4.SpacingBefore = 10f;
                    table = new PdfPTable(1);
                    table.TotalWidth = 560f;
                    float[] widths = new float[] { 560f };
                    table.SetWidths(widths);
                    table.LockedWidth = true;
                    cell = new PdfPCell(new Phrase("                                                                                           *Subject to Pune Jurisdiction Only*", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    cell.Border = Rectangle.NO_BORDER;
                    table.AddCell(cell);
                    doc.Add(table);
                    // doc.Close();
                    /////////new code //////////14 End

                    doc.NewPage();


                }
                //doc.Close();
                //ifrRight6.Attributes["src"] = @"../Files/" + "TaxInvoice.pdf";
                #endregion
            }

            if (Chk_TRIPLICATE.Checked == true)
            {
                #region  Pdf For Triplicate
                //string id = Session["PDFID"].ToString();

                png.ScaleToFit(70, 100);

                //For Image Position
                png.SetAbsolutePosition(40, 745);
                //var document = new Document();

                //Give space before image
                //png.ScaleToFit(document.PageSize.Width - (document.RightMargin * 100), 50);
                png.SpacingBefore = 50f;

                //Give some space after the image

                png.SpacingAfter = 1f;

                png.Alignment = Element.ALIGN_LEFT;

                //paragraphimage.Add(png);
                //doc.Add(paragraphimage);
                doc.Add(png);

                PdfContentByte cb2 = writer.DirectContent;
                cb2.Rectangle(17f, 735f, 560f, 60f);
                cb2.Stroke();
                // Header 
                cb2.BeginText();
                cb2.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 20);
                cb2.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "ENDEAVOUR AUTOMATION", 180, 773, 0);
                cb2.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 11);
                cb2.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "S.N.9/2/A1,Sakpal Wasti,Behind Genesis furniture,Nr.HP Petrol Pump,Ravet Road", 145, 755, 0);
                cb2.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 11);
                cb2.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Tathawade, Pune-411033", 227, 740, 0);
                cb2.EndText();

                PdfContentByte cbb2 = writer.DirectContent;
                cbb2.Rectangle(17f, 710f, 560f, 25f);
                cbb2.Stroke();
                // Header 
                cbb2.BeginText();
                cbb2.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
                cbb2.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " Mob: 9860502108    Email: endeavour.automations@gmail.com ", 153, 722, 0);
                cbb2.EndText();

                PdfContentByte cbbb2 = writer.DirectContent;
                cbbb2.Rectangle(17f, 685f, 560f, 25f);
                cbbb2.Stroke();
                // Header 
                cbbb2.BeginText();
                cbbb2.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
                cbbb2.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "GSTIN :  27AFYPJ3488G1ZQ ", 30, 695, 0);
                cbbb2.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
                cbbb2.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "W.E.F. :  01/07/2017", 185, 695, 0);
                cbbb2.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
                cbbb2.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "PAN No. :  AFYPJ3488G", 310, 695, 0);
                cbbb2.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
                cbbb2.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "State Code :  27 Maharashtra", 440, 695, 0);
                cbbb2.EndText();

                //-------------------
                PdfContentByte cdd2 = writer.DirectContent;
                cdd2.Rectangle(17f, 660f, 560f, 25f);
                cdd2.Stroke();
                cdd2.BeginText();

                cb2.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
                cb2.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "TRIPLICATE FOR BUYER", 480, 800, 0);

                cdd2.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 17);
                cdd2.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Tax Invoice", 260, 667, 0);
                cdd2.EndText();
                //--------------

                if (Dt.Rows.Count > 0)
                {
                    var CreateDate = DateTime.Now.ToString("dd-MM-yyyy");
                    string CustName = Dt.Rows[0]["CustName"].ToString();
                    string InvoiceNo = Dt.Rows[0]["InvoiceNo"].ToString();
                    string PoDate = Dt.Rows[0]["PoDate"].ToString().TrimEnd("0:0".ToCharArray());
                    string PoNumber = Dt.Rows[0]["PoNo"].ToString();
                    string CompanyAddress = Dt.Rows[0]["CompanyAddress"].ToString();
                    string Address = Dt.Rows[0]["CustomerShippingAddress"].ToString();
                    string InvoiceDate = Dt.Rows[0]["InvoiceDate"].ToString().TrimEnd("0:0".ToCharArray());
                    string ChallanNo = Dt.Rows[0]["ChallanNo"].ToString();
                    string GSTNo = Dt.Rows[0]["CustomerGstNo"].ToString();
                    string KindAtt = Dt.Rows[0]["KindAtt"].ToString();
                    string TotalInWord = Dt.Rows[0]["TotalInWord"].ToString();
                    string GrandTotal = Dt.Rows[0]["GrandTotal"].ToString();
                    string CGST = Dt.Rows[0]["CGST"].ToString();
                    string SGST = Dt.Rows[0]["SGST"].ToString();
                    //string IGST = Dt.Rows[0]["IGST"].ToString();
                    string Total = Dt.Rows[0]["AllTotalAmount"].ToString();
                    string CompanyStateCode = Dt.Rows[0]["CompanyStateCode"].ToString();


                    decimal IGST = Convert.ToDecimal(Dt.Rows[0]["IGST"]);
                    decimal GSTAmt = IGST / 2;


                    string str = Dt.Rows[0]["Term_Condition_1"].ToString();
                    string str1 = Dt.Rows[0]["Term_Condition_2"].ToString();
                    string str2 = Dt.Rows[0]["Term_Condition_3"].ToString();
                    string str3 = Dt.Rows[0]["Term_Condition_4"].ToString();
                    string str4 = Dt.Rows[0]["Term_Condition_5"].ToString();
                    string str5 = Dt.Rows[0]["Term_Condition_6"].ToString();
                    string[] arrstr = str.ToString().Split('-');
                    string[] arrstr1 = str1.ToString().Split('-');
                    string[] arrstr2 = str2.ToString().Split('-');
                    string[] arrstr3 = str3.ToString().Split('-');
                    string[] arrstr4 = str4.ToString().Split('-');
                    string[] arrstr5 = str5.ToString().Split('-');

                    Paragraph paragraphTable1 = new Paragraph();
                    paragraphTable1.SpacingBefore = 120f;
                    paragraphTable1.SpacingAfter = 10f;

                    PdfPTable table = new PdfPTable(4);

                    float[] widths2 = new float[] { 100, 180, 100, 180 };
                    table.SetWidths(widths2);
                    table.TotalWidth = 560f;
                    table.LockedWidth = true;

                    DateTime ffff1 = Convert.ToDateTime(Dt.Rows[0]["InvoiceDate"].ToString());
                    string datee = ffff1.ToString("dd-MM-yyyy");

                    DateTime ffff2 = Convert.ToDateTime(Dt.Rows[0]["PoDate"].ToString());
                    string podate = ffff1.ToString("dd-MM-yyyy");


                    table.AddCell(new Phrase("Buyer : ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(CustName, FontFactory.GetFont("Arial", 9, Font.BOLD)));

                    table.AddCell(new Phrase("Consignee :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(CustName, FontFactory.GetFont("Arial", 9, Font.BOLD)));

                    table.AddCell(new Phrase("Shipping Address : ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(CompanyAddress, FontFactory.GetFont("Arial", 9, Font.BOLD)));

                    table.AddCell(new Phrase("Billing Address : ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(Address, FontFactory.GetFont("Arial", 9, Font.BOLD)));

                    table.AddCell(new Phrase("Invoice Number : ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(InvoiceNo, FontFactory.GetFont("Arial", 9, Font.BOLD)));

                    table.AddCell(new Phrase("Invoice Date :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(datee, FontFactory.GetFont("Arial", 9, Font.BOLD)));

                    //table.AddCell(new Phrase("Address", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    //table.AddCell(new Phrase(Address, FontFactory.GetFont("Arial", 9, Font.BOLD)));


                    table.AddCell(new Phrase("P.O. Number :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(PoNumber, FontFactory.GetFont("Arial", 9, Font.BOLD)));

                    table.AddCell(new Phrase("P.O. Date :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(podate, FontFactory.GetFont("Arial", 9, Font.BOLD)));

                    table.AddCell(new Phrase("Challan No. :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(ChallanNo, FontFactory.GetFont("Arial", 9, Font.BOLD)));

                    table.AddCell(new Phrase("GST No. :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
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
                            table.AddCell(new Phrase(dr["printdescription"].ToString(), FontFactory.GetFont("Arial", 9)));
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
                        //doc.Add(table);
                    }
                    else if (Dt.Rows.Count >= 7 && Dt.Rows.Count <= 9)
                    {
                        table.AddCell(new Phrase("  \n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));

                    }
                    else if (Dt.Rows.Count >= 4 && Dt.Rows.Count <= 6)
                    {
                        table.AddCell(new Phrase("  \n\n\n\n\n\n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));

                    }
                    else if (Dt.Rows.Count < 4)
                    {

                        table.AddCell(new Phrase("  \n\n\n\n\n\n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));

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
                    table.AddCell(paragraph);
                    PdfPCell cell = new PdfPCell(new Phrase("Sub Total", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(cell);
                    PdfPCell cell11 = new PdfPCell(new Phrase(Total, FontFactory.GetFont("Arial", 10, Font.BOLD)));
                    cell11.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(cell11);


                    doc.Add(table);
                    //add total row end


                    if (CompanyStateCode == "27 MAHARASHTRA")
                    {
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

                        var Gstamt = Convert.ToDecimal(IGST) / 2;

                        table.SetWidths(new float[] { 0f, 76f, 12f });
                        table.AddCell(paragraph);
                        PdfPCell cell2 = new PdfPCell(new Phrase("CGST 9%", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                        cell2.HorizontalAlignment = Element.ALIGN_RIGHT;
                        table.AddCell(cell2);
                        PdfPCell cell3 = new PdfPCell(new Phrase(CGST.ToString(), FontFactory.GetFont("Arial", 10, Font.BOLD)));
                        //PdfPCell cell3 = new PdfPCell(new Phrase(Gstamt.ToString(), FontFactory.GetFont("Arial", 10, Font.BOLD)));
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
                        PdfPCell cell22 = new PdfPCell(new Phrase("SGST 9%", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                        cell22.HorizontalAlignment = Element.ALIGN_RIGHT;
                        table.AddCell(cell22);
                        PdfPCell cell33 = new PdfPCell(new Phrase(SGST.ToString(), FontFactory.GetFont("Arial", 10, Font.BOLD)));
                        //PdfPCell cell33 = new PdfPCell(new Phrase(Gstamt.ToString(), FontFactory.GetFont("Arial", 10, Font.BOLD)));
                        cell33.HorizontalAlignment = Element.ALIGN_RIGHT;
                        table.AddCell(cell33);

                        doc.Add(table);
                        //SGST 9% Row End
                    }
                    else
                    {
                        //new for i gst
                        table = new PdfPTable(3);
                        table.TotalWidth = 560f;
                        table.LockedWidth = true;

                        table.SetWidths(new float[] { 0f, 76f, 12f });
                        table.AddCell(paragraph);
                        PdfPCell cell222 = new PdfPCell(new Phrase("IGST 18%", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                        cell222.HorizontalAlignment = Element.ALIGN_RIGHT;
                        table.AddCell(cell222);
                        PdfPCell cell333 = new PdfPCell(new Phrase(IGST.ToString(), FontFactory.GetFont("Arial", 10, Font.BOLD)));
                        //PdfPCell cell33 = new PdfPCell(new Phrase(Gstamt.ToString(), FontFactory.GetFont("Arial", 10, Font.BOLD)));
                        cell333.HorizontalAlignment = Element.ALIGN_RIGHT;
                        table.AddCell(cell333);

                        doc.Add(table);
                        //new for i gst
                    }

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


                    table.SetWidths(new float[] { 0f, 76f, 12f });
                    table.AddCell(paragraph);
                    PdfPCell cell44 = new PdfPCell(new Phrase("Grand Total", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                    cell44.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(cell44);

                    PdfPCell cell55 = new PdfPCell(new Phrase(GrandTotal, FontFactory.GetFont("Arial", 10, Font.BOLD)));
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


                    ////new code start 22-08-02024

                    string[] items90termm = { "Dear Sir/Madam,\b", "We acknowledge with thanks the receipt of your above enquiry and have great pleasure in submitting our lowest quotation,\b", "subject to the conditions printed below.\n" };

                    Font font12112termm = FontFactory.GetFont("Arial", 8);
                    Font font10111termm = FontFactory.GetFont("Arial", 8);
                    Font fontWithSize = FontFactory.GetFont(FontFactory.HELVETICA, 10);
                    // Paragraph paragraph110term = new Paragraph("\bTerms & Conditions:", font12112term);


                    //Paragraph paragraph110termm = new Paragraph("I / we certify that our registration certficate under the GST Act, 2017 is in force on the date on which the supply of goods specified In This Tax Invoice made by me / us & the transaction of supply covered by this Tax Invoice had been effected by me / us & it shall be accounted for in the turnover ofsupplies while filling of return &the due tax if any payable on the supplies has been paid or shall be paid.Further certified that the particulars givenabove are true and correct &the amount indicated represents the prices actually charged and that there is no flow additional consideration directly orindirectly from the buyer. Interest @ 18 % p.a.charged on all outstanding more than one month after invoice has been rendered", fontWithSize);

                    // Create the full paragraph
                    Paragraph paragraph110termm = new Paragraph("I / we certify that our registration certificate under the GST Act, 2017 is in force on the date on which the supply of goods specified in this tax invoice made by me / us & the transaction of supply covered by this tax invoice had been effected by me / us & it shall be accounted for in the turnover of supplies while filling of return & the due tax if any payable on the supplies has been paid or shall be paid. Further certified that the particulars given above are true and correct & the amount indicated represents the prices actually charged and that there is no flow of additional consideration directly or indirectly from the buyer. ", fontWithSize);

                    // Create a Chunk with the text to be bolded and apply bold font
                    Font boldFont = new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.BOLD);
                    Chunk boldChunk = new Chunk("Interest @ 18% p.a. charged on all outstanding more than one month after invoice has been rendered.", boldFont);

                    // Add the bold chunk to the paragraph
                    paragraph110termm.Add(boldChunk);

                    for (int i = 0; i < items90termm.Length; i++)
                    {
                        paragraph110termm.Add(new Phrase(" ", fontWithSize));
                    }

                    table = new PdfPTable(1);
                    table.TotalWidth = 560f;
                    table.LockedWidth = true;
                    table.SetWidths(new float[] { 150f });

                    table.AddCell(paragraph110termm);
                    table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10)));
                    table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 11, Font.BOLD)));

                    doc.Add(table);

                    ///////////term And Condition

                    string[] items90term = { "Dear Sir/Madam,\b", "We acknowledge with thanks the receipt of your above enquiry and have great pleasure in submitting our lowest quotation,\b", "subject to the conditions printed below.\n" };

                    Font font12112term = FontFactory.GetFont("Arial", 12, Font.BOLD);
                    Font font10111term = FontFactory.GetFont("Arial", 10);
                    Paragraph paragraph110term = new Paragraph("\bTerms & Conditions:", font12112term);

                    for (int i = 0; i < items90term.Length; i++)
                    {
                        paragraph110term.Add(new Phrase(" ", font10111term));
                    }

                    table = new PdfPTable(1);
                    table.TotalWidth = 560f;
                    table.LockedWidth = true;
                    table.SetWidths(new float[] { 150f });

                    table.AddCell(paragraph110term);
                    //table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10)));
                    //table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 11, Font.BOLD)));

                    doc.Add(table);

                    /////////////////////////////////////////////////term condition in Database

                    string[] items90termdata = { "Dear Sir/Madam,\b", "We acknowledge with thanks the receipt of your above enquiry and have great pleasure in submitting our lowest quotation,\b", "subject to the conditions printed below.\n" };

                    Font font12112termdata = FontFactory.GetFont("Arial", 12, Font.BOLD);
                    Font font10111termdata = FontFactory.GetFont("Arial", 10);
                    Paragraph paragraph110termdata = new Paragraph("", font12112termdata);

                    for (int i = 0; i < items90termdata.Length; i++)
                    {
                        paragraph110termdata.Add(new Phrase(" ", font10111termdata));
                    }

                    table = new PdfPTable(2);
                    table.TotalWidth = 560f;
                    table.LockedWidth = true;
                    table.SetWidths(new float[] { 150f, 350f });

                    table.AddCell(new Phrase(arrstr[0].ToString(), FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(arrstr[1].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(arrstr1[0].ToString(), FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(arrstr1[1].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(arrstr2[0].ToString(), FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(arrstr2[1].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(arrstr3[0].ToString(), FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(arrstr3[1].ToString(), FontFactory.GetFont("Arial", 9)));

                    table.AddCell(new Phrase(arrstr4[0].ToString(), FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(arrstr4[1].ToString(), FontFactory.GetFont("Arial", 9)));

                    table.AddCell(new Phrase(arrstr5[0].ToString(), FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(arrstr5[1].ToString(), FontFactory.GetFont("Arial", 9)));

                    doc.Add(table);

                    ///////////Add Bank details Titles

                    string[] items90 = { "Dear Sir/Madam,\b", "We acknowledge with thanks the receipt of your above enquiry and have great pleasure in submitting our lowest quotation,\b", "subject to the conditions printed below.\n" };

                    Font font12112 = FontFactory.GetFont("Arial", 12, Font.BOLD);
                    Font font10111 = FontFactory.GetFont("Arial", 10);
                    Paragraph paragraph110 = new Paragraph("\bBank Account Details", font12112);

                    for (int i = 0; i < items90.Length; i++)
                    {
                        paragraph110.Add(new Phrase(" ", font10111));
                    }

                    table = new PdfPTable(1);
                    table.TotalWidth = 560f;
                    table.LockedWidth = true;
                    table.SetWidths(new float[] { 310f });

                    table.AddCell(paragraph110);
                    doc.Add(table);
                    /////////////////////////////////////////////////
                    string[] items9 = { "Dear Sir/Madam,\b", "We acknowledge with thanks the receipt of your above enquiry and have great pleasure in submitting our lowest quotation,\b", "subject to the conditions printed below.\n" };

                    Font font1211 = FontFactory.GetFont("Arial", 12, Font.BOLD);
                    Font font1011 = FontFactory.GetFont("Arial", 10);
                    Paragraph paragraph11 = new Paragraph("", font1211);

                    for (int i = 0; i < items9.Length; i++)
                    {
                        paragraph11.Add(new Phrase(" ", font1011));
                    }

                    table = new PdfPTable(2);
                    table.TotalWidth = 560f;
                    table.LockedWidth = true;
                    table.SetWidths(new float[] { 310f, 250f });

                    //table.AddCell(paragraph11);
                    table.AddCell(new Phrase("\bBank Name : BANK OF BARODA\n\n\bAccount Name : ENDEAVOUR AUTOMATION\n\n\bBranch : KALEWADI\n\n\bA/c No. : 46180200000214\n\n\bIFSC/Neft Code :BARB0KALEWA\n", FontFactory.GetFont("Arial", 10)));
                    table.AddCell(new Phrase("\b      For,\n\n\b\b\b\b\b\b\b\b\b            ENDEAVOUR AUTOMATION\n\n\n\n\n                                       Authorised Signatory", FontFactory.GetFont("Arial", 11, Font.BOLD)));

                    doc.Add(table);
                    /////////////////////////////
                    PdfPCell celll = null;
                    Paragraph paragraphTable4 = new Paragraph();
                    paragraphTable4.SpacingBefore = 10f;
                    table = new PdfPTable(1);
                    table.TotalWidth = 560f;
                    float[] widths = new float[] { 560f };
                    table.SetWidths(widths);
                    table.LockedWidth = true;
                    cell = new PdfPCell(new Phrase("                                                                                           *Subject to Pune Jurisdiction Only*", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    cell.Border = Rectangle.NO_BORDER;
                    table.AddCell(cell);
                    doc.Add(table);
                    // doc.Close();
                    /////////new code //////////14 End

                    doc.NewPage();

                }

                #endregion
            }

            if (Chk_EXTRA.Checked == true)
            {
                #region  Pdf For Extra Copy


                png.ScaleToFit(70, 100);

                //For Image Position
                png.SetAbsolutePosition(40, 745);

                png.SpacingBefore = 50f;


                png.SpacingAfter = 1f;

                png.Alignment = Element.ALIGN_LEFT;

                doc.Add(png);

                PdfContentByte cb3 = writer.DirectContent;
                cb3.Rectangle(17f, 735f, 560f, 60f);
                cb3.Stroke();
                // Header 
                cb3.BeginText();
                cb3.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 20);
                cb3.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "ENDEAVOUR AUTOMATION", 180, 773, 0);
                cb3.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 11);
                cb3.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "S.N.9/2/A1,Sakpal Wasti,Behind Genesis furniture,Nr.HP Petrol Pump,Ravet Road", 145, 755, 0);
                cb3.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 11);
                cb3.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Tathawade, Pune-411033", 227, 740, 0);
                cb3.EndText();

                PdfContentByte cbb3 = writer.DirectContent;
                cbb3.Rectangle(17f, 710f, 560f, 25f);
                cbb3.Stroke();
                // Header 
                cbb3.BeginText();
                cbb3.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
                cbb3.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " Mob: 9860502108    Email: endeavour.automations@gmail.com ", 153, 722, 0);
                cbb3.EndText();

                PdfContentByte cbbb3 = writer.DirectContent;
                cbbb3.Rectangle(17f, 685f, 560f, 25f);
                cbbb3.Stroke();
                // Header 
                cbbb3.BeginText();
                cbbb3.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
                cbbb3.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "GSTIN :  27AFYPJ3488G1ZQ ", 30, 695, 0);
                cbbb3.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
                cbbb3.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "W.E.F. :  01/07/2017", 185, 695, 0);
                cbbb3.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
                cbbb3.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "PAN No. :  AFYPJ3488G", 310, 695, 0);
                cbbb3.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
                cbbb3.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "State Code :  27 Maharashtra", 440, 695, 0);
                cbbb3.EndText();

                //-------------------
                PdfContentByte cdd3 = writer.DirectContent;
                cdd3.Rectangle(17f, 660f, 560f, 25f);
                cdd3.Stroke();
                cdd3.BeginText();

                cb3.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
                cb3.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "EXTRA COPY", 480, 800, 0);

                cdd3.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 17);
                cdd3.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Tax Invoice", 260, 667, 0);
                cdd3.EndText();
                //--------------



                if (Dt.Rows.Count > 0)
                {
                    var CreateDate = DateTime.Now.ToString("dd-MM-yyyy");
                    string CustName = Dt.Rows[0]["CustName"].ToString();
                    string InvoiceNo = Dt.Rows[0]["InvoiceNo"].ToString();
                    string PoDate = Dt.Rows[0]["PoDate"].ToString().TrimEnd("0:0".ToCharArray());
                    string PoNumber = Dt.Rows[0]["PoNo"].ToString();
                    string CompanyAddress = Dt.Rows[0]["CompanyAddress"].ToString();
                    string Address = Dt.Rows[0]["CustomerShippingAddress"].ToString();
                    string InvoiceDate = Dt.Rows[0]["InvoiceDate"].ToString().TrimEnd("0:0".ToCharArray());
                    string ChallanNo = Dt.Rows[0]["ChallanNo"].ToString();
                    string GSTNo = Dt.Rows[0]["CustomerGstNo"].ToString();
                    string KindAtt = Dt.Rows[0]["KindAtt"].ToString();
                    string TotalInWord = Dt.Rows[0]["TotalInWord"].ToString();
                    string GrandTotal = Dt.Rows[0]["GrandTotal"].ToString();
                    string CGST = Dt.Rows[0]["CGST"].ToString();
                    string SGST = Dt.Rows[0]["SGST"].ToString();
                    //string IGST = Dt.Rows[0]["IGST"].ToString();
                    string Total = Dt.Rows[0]["AllTotalAmount"].ToString();
                    string CompanyStateCode = Dt.Rows[0]["CompanyStateCode"].ToString();


                    decimal IGST = Convert.ToDecimal(Dt.Rows[0]["IGST"]);
                    decimal GSTAmt = IGST / 2;


                    string str = Dt.Rows[0]["Term_Condition_1"].ToString();
                    string str1 = Dt.Rows[0]["Term_Condition_2"].ToString();
                    string str2 = Dt.Rows[0]["Term_Condition_3"].ToString();
                    string str3 = Dt.Rows[0]["Term_Condition_4"].ToString();
                    string str4 = Dt.Rows[0]["Term_Condition_5"].ToString();
                    string str5 = Dt.Rows[0]["Term_Condition_6"].ToString();
                    string[] arrstr = str.ToString().Split('-');
                    string[] arrstr1 = str1.ToString().Split('-');
                    string[] arrstr2 = str2.ToString().Split('-');
                    string[] arrstr3 = str3.ToString().Split('-');
                    string[] arrstr4 = str4.ToString().Split('-');
                    string[] arrstr5 = str5.ToString().Split('-');

                    Paragraph paragraphTable1 = new Paragraph();
                    paragraphTable1.SpacingBefore = 120f;
                    paragraphTable1.SpacingAfter = 10f;

                    PdfPTable table = new PdfPTable(4);

                    float[] widths2 = new float[] { 100, 180, 100, 180 };
                    table.SetWidths(widths2);
                    table.TotalWidth = 560f;
                    table.LockedWidth = true;

                    DateTime ffff1 = Convert.ToDateTime(Dt.Rows[0]["InvoiceDate"].ToString());
                    string datee = ffff1.ToString("dd-MM-yyyy");

                    DateTime ffff2 = Convert.ToDateTime(Dt.Rows[0]["PoDate"].ToString());
                    string podate = ffff1.ToString("dd-MM-yyyy");

                    table.AddCell(new Phrase("Buyer : ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(CustName, FontFactory.GetFont("Arial", 9, Font.BOLD)));

                    table.AddCell(new Phrase("Consignee :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(CustName, FontFactory.GetFont("Arial", 9, Font.BOLD)));

                    table.AddCell(new Phrase("Shipping Address : ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(CompanyAddress, FontFactory.GetFont("Arial", 9, Font.BOLD)));

                    table.AddCell(new Phrase("Billing Address : ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(Address, FontFactory.GetFont("Arial", 9, Font.BOLD)));

                    table.AddCell(new Phrase("Invoice Number : ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(InvoiceNo, FontFactory.GetFont("Arial", 9, Font.BOLD)));

                    table.AddCell(new Phrase("Invoice Date :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(datee, FontFactory.GetFont("Arial", 9, Font.BOLD)));

                    //table.AddCell(new Phrase("Address", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    //table.AddCell(new Phrase(Address, FontFactory.GetFont("Arial", 9, Font.BOLD)));

                    table.AddCell(new Phrase("P.O. Number :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(PoNumber, FontFactory.GetFont("Arial", 9, Font.BOLD)));

                    table.AddCell(new Phrase("P.O. Date :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(podate, FontFactory.GetFont("Arial", 9, Font.BOLD)));

                    table.AddCell(new Phrase("Challan No. :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(ChallanNo, FontFactory.GetFont("Arial", 9, Font.BOLD)));

                    table.AddCell(new Phrase("GST No. :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
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
                            table.AddCell(new Phrase(dr["printdescription"].ToString(), FontFactory.GetFont("Arial", 9)));
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
                        //doc.Add(table);
                    }
                    else if (Dt.Rows.Count >= 7 && Dt.Rows.Count <= 9)
                    {
                        table.AddCell(new Phrase("  \n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));

                    }
                    else if (Dt.Rows.Count >= 4 && Dt.Rows.Count <= 6)
                    {
                        table.AddCell(new Phrase("  \n\n\n\n\n\n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));

                    }
                    else if (Dt.Rows.Count < 4)
                    {

                        table.AddCell(new Phrase("  \n\n\n\n\n\n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));

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
                    table.AddCell(paragraph);
                    PdfPCell cell = new PdfPCell(new Phrase("Sub Total", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(cell);
                    PdfPCell cell11 = new PdfPCell(new Phrase(Total, FontFactory.GetFont("Arial", 10, Font.BOLD)));
                    cell11.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(cell11);


                    doc.Add(table);
                    //add total row end


                    if (CompanyStateCode == "27 MAHARASHTRA")
                    {
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

                        var Gstamt = Convert.ToDecimal(IGST) / 2;

                        table.SetWidths(new float[] { 0f, 76f, 12f });
                        table.AddCell(paragraph);
                        PdfPCell cell2 = new PdfPCell(new Phrase("CGST 9%", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                        cell2.HorizontalAlignment = Element.ALIGN_RIGHT;
                        table.AddCell(cell2);
                        PdfPCell cell3 = new PdfPCell(new Phrase(CGST.ToString(), FontFactory.GetFont("Arial", 10, Font.BOLD)));
                        //PdfPCell cell3 = new PdfPCell(new Phrase(Gstamt.ToString(), FontFactory.GetFont("Arial", 10, Font.BOLD)));
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
                        PdfPCell cell22 = new PdfPCell(new Phrase("SGST 9%", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                        cell22.HorizontalAlignment = Element.ALIGN_RIGHT;
                        table.AddCell(cell22);
                        PdfPCell cell33 = new PdfPCell(new Phrase(SGST.ToString(), FontFactory.GetFont("Arial", 10, Font.BOLD)));
                        //PdfPCell cell33 = new PdfPCell(new Phrase(Gstamt.ToString(), FontFactory.GetFont("Arial", 10, Font.BOLD)));
                        cell33.HorizontalAlignment = Element.ALIGN_RIGHT;
                        table.AddCell(cell33);

                        doc.Add(table);
                        //SGST 9% Row End
                    }
                    else
                    {
                        //new for i gst
                        table = new PdfPTable(3);
                        table.TotalWidth = 560f;
                        table.LockedWidth = true;

                        table.SetWidths(new float[] { 0f, 76f, 12f });
                        table.AddCell(paragraph);
                        PdfPCell cell222 = new PdfPCell(new Phrase("IGST 18%", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                        cell222.HorizontalAlignment = Element.ALIGN_RIGHT;
                        table.AddCell(cell222);
                        PdfPCell cell333 = new PdfPCell(new Phrase(IGST.ToString(), FontFactory.GetFont("Arial", 10, Font.BOLD)));
                        //PdfPCell cell33 = new PdfPCell(new Phrase(Gstamt.ToString(), FontFactory.GetFont("Arial", 10, Font.BOLD)));
                        cell333.HorizontalAlignment = Element.ALIGN_RIGHT;
                        table.AddCell(cell333);

                        doc.Add(table);
                        //new for i gst
                    }

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


                    table.SetWidths(new float[] { 0f, 76f, 12f });
                    table.AddCell(paragraph);
                    PdfPCell cell44 = new PdfPCell(new Phrase("Grand Total", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                    cell44.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(cell44);

                    PdfPCell cell55 = new PdfPCell(new Phrase(GrandTotal, FontFactory.GetFont("Arial", 10, Font.BOLD)));
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


                    ////new code start 22-08-02024

                    string[] items90termm = { "Dear Sir/Madam,\b", "We acknowledge with thanks the receipt of your above enquiry and have great pleasure in submitting our lowest quotation,\b", "subject to the conditions printed below.\n" };

                    Font font12112termm = FontFactory.GetFont("Arial", 8);
                    Font font10111termm = FontFactory.GetFont("Arial", 8);
                    Font fontWithSize = FontFactory.GetFont(FontFactory.HELVETICA, 10);
                    // Paragraph paragraph110term = new Paragraph("\bTerms & Conditions:", font12112term);


                    //Paragraph paragraph110termm = new Paragraph("I / we certify that our registration certficate under the GST Act, 2017 is in force on the date on which the supply of goods specified In This Tax Invoice made by me / us & the transaction of supply covered by this Tax Invoice had been effected by me / us & it shall be accounted for in the turnover ofsupplies while filling of return &the due tax if any payable on the supplies has been paid or shall be paid.Further certified that the particulars givenabove are true and correct &the amount indicated represents the prices actually charged and that there is no flow additional consideration directly orindirectly from the buyer. Interest @ 18 % p.a.charged on all outstanding more than one month after invoice has been rendered", fontWithSize);

                    // Create the full paragraph
                    Paragraph paragraph110termm = new Paragraph("I / we certify that our registration certificate under the GST Act, 2017 is in force on the date on which the supply of goods specified in this tax invoice made by me / us & the transaction of supply covered by this tax invoice had been effected by me / us & it shall be accounted for in the turnover of supplies while filling of return & the due tax if any payable on the supplies has been paid or shall be paid. Further certified that the particulars given above are true and correct & the amount indicated represents the prices actually charged and that there is no flow of additional consideration directly or indirectly from the buyer. ", fontWithSize);

                    // Create a Chunk with the text to be bolded and apply bold font
                    Font boldFont = new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.BOLD);
                    Chunk boldChunk = new Chunk("Interest @ 18% p.a. charged on all outstanding more than one month after invoice has been rendered.", boldFont);

                    // Add the bold chunk to the paragraph
                    paragraph110termm.Add(boldChunk);

                    for (int i = 0; i < items90termm.Length; i++)
                    {
                        paragraph110termm.Add(new Phrase(" ", fontWithSize));
                    }

                    table = new PdfPTable(1);
                    table.TotalWidth = 560f;
                    table.LockedWidth = true;
                    table.SetWidths(new float[] { 150f });

                    table.AddCell(paragraph110termm);
                    table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10)));
                    table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 11, Font.BOLD)));

                    doc.Add(table);

                    ///////////term And Condition

                    string[] items90term = { "Dear Sir/Madam,\b", "We acknowledge with thanks the receipt of your above enquiry and have great pleasure in submitting our lowest quotation,\b", "subject to the conditions printed below.\n" };

                    Font font12112term = FontFactory.GetFont("Arial", 12, Font.BOLD);
                    Font font10111term = FontFactory.GetFont("Arial", 10);
                    Paragraph paragraph110term = new Paragraph("\bTerms & Conditions:", font12112term);

                    for (int i = 0; i < items90term.Length; i++)
                    {
                        paragraph110term.Add(new Phrase(" ", font10111term));
                    }

                    table = new PdfPTable(1);
                    table.TotalWidth = 560f;
                    table.LockedWidth = true;
                    table.SetWidths(new float[] { 150f });

                    table.AddCell(paragraph110term);
                    //table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10)));
                    //table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 11, Font.BOLD)));

                    doc.Add(table);

                    /////////////////////////////////////////////////term condition in Database

                    string[] items90termdata = { "Dear Sir/Madam,\b", "We acknowledge with thanks the receipt of your above enquiry and have great pleasure in submitting our lowest quotation,\b", "subject to the conditions printed below.\n" };

                    Font font12112termdata = FontFactory.GetFont("Arial", 12, Font.BOLD);
                    Font font10111termdata = FontFactory.GetFont("Arial", 10);
                    Paragraph paragraph110termdata = new Paragraph("", font12112termdata);

                    for (int i = 0; i < items90termdata.Length; i++)
                    {
                        paragraph110termdata.Add(new Phrase(" ", font10111termdata));
                    }

                    table = new PdfPTable(2);
                    table.TotalWidth = 560f;
                    table.LockedWidth = true;
                    table.SetWidths(new float[] { 150f, 350f });

                    table.AddCell(new Phrase(arrstr[0].ToString(), FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(arrstr[1].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(arrstr1[0].ToString(), FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(arrstr1[1].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(arrstr2[0].ToString(), FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(arrstr2[1].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(arrstr3[0].ToString(), FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(arrstr3[1].ToString(), FontFactory.GetFont("Arial", 9)));

                    table.AddCell(new Phrase(arrstr4[0].ToString(), FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(arrstr4[1].ToString(), FontFactory.GetFont("Arial", 9)));

                    table.AddCell(new Phrase(arrstr5[0].ToString(), FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(arrstr5[1].ToString(), FontFactory.GetFont("Arial", 9)));

                    doc.Add(table);

                    ///////////Add Bank details Titles

                    string[] items90 = { "Dear Sir/Madam,\b", "We acknowledge with thanks the receipt of your above enquiry and have great pleasure in submitting our lowest quotation,\b", "subject to the conditions printed below.\n" };

                    Font font12112 = FontFactory.GetFont("Arial", 12, Font.BOLD);
                    Font font10111 = FontFactory.GetFont("Arial", 10);
                    Paragraph paragraph110 = new Paragraph("\bBank Account Details", font12112);

                    for (int i = 0; i < items90.Length; i++)
                    {
                        paragraph110.Add(new Phrase(" ", font10111));
                    }

                    table = new PdfPTable(1);
                    table.TotalWidth = 560f;
                    table.LockedWidth = true;
                    table.SetWidths(new float[] { 310f });

                    table.AddCell(paragraph110);
                    doc.Add(table);
                    /////////////////////////////////////////////////
                    string[] items9 = { "Dear Sir/Madam,\b", "We acknowledge with thanks the receipt of your above enquiry and have great pleasure in submitting our lowest quotation,\b", "subject to the conditions printed below.\n" };

                    Font font1211 = FontFactory.GetFont("Arial", 12, Font.BOLD);
                    Font font1011 = FontFactory.GetFont("Arial", 10);
                    Paragraph paragraph11 = new Paragraph("", font1211);

                    for (int i = 0; i < items9.Length; i++)
                    {
                        paragraph11.Add(new Phrase(" ", font1011));
                    }

                    table = new PdfPTable(2);
                    table.TotalWidth = 560f;
                    table.LockedWidth = true;
                    table.SetWidths(new float[] { 310f, 250f });

                    //table.AddCell(paragraph11);
                    table.AddCell(new Phrase("\bBank Name : BANK OF BARODA\n\n\bAccount Name : ENDEAVOUR AUTOMATION\n\n\bBranch : KALEWADI\n\n\bA/c No. : 46180200000214\n\n\bIFSC/Neft Code :BARB0KALEWA\n", FontFactory.GetFont("Arial", 10)));
                    table.AddCell(new Phrase("\b      For,\n\n\b\b\b\b\b\b\b\b\b            ENDEAVOUR AUTOMATION\n\n\n\n\n                                       Authorised Signatory", FontFactory.GetFont("Arial", 11, Font.BOLD)));

                    doc.Add(table);
                    /////////////////////////////
                    PdfPCell celll = null;
                    Paragraph paragraphTable4 = new Paragraph();
                    paragraphTable4.SpacingBefore = 10f;
                    table = new PdfPTable(1);
                    table.TotalWidth = 560f;
                    float[] widths = new float[] { 560f };
                    table.SetWidths(widths);
                    table.LockedWidth = true;
                    cell = new PdfPCell(new Phrase("                                                                                           *Subject to Pune Jurisdiction Only*", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    cell.Border = Rectangle.NO_BORDER;
                    table.AddCell(cell);
                    doc.Add(table);
                    // doc.Close();
                    /////////new code //////////14 End

                    doc.Close();

                }
             
                #endregion
            }

          
            doc.Close();
            ifrRight6.Attributes["src"] = @"../Files/" + "TaxInvoice.pdf";
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}