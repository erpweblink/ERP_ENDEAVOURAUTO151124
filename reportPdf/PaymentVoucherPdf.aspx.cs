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

public partial class reportPdf_PaymentVoucherPdf : System.Web.UI.Page
{
    string id = "";
    SqlConnection Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["Id"] != null)
        {
            id = Decrypt(Request.QueryString["Id"].ToString());
            Pdf(id);
        }
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

    private void Pdf(string id)
    {
        DataTable Dt = new DataTable();
         SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM [TblPaymentVoucher] where Id='" + id + "'", Conn);
        //SqlDataAdapter da = new SqlDataAdapter(" SELECT TblReceiptVoucherDtls.Balance,TblReceiptVoucherHdr.CustomerName,Date,ReceiptNo,BankCashDr,PaymentMode,ClearedOnDate,ChequeTransIdNo,ChequeTransDate,SlipNo,BankName,Branch,Address,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate, TblReceiptVoucherDtls.Balance  FROM TblReceiptVoucherHdr  RIGHT JOIN TblReceiptVoucherDtls  ON TblReceiptVoucherHdr.Id = TblReceiptVoucherDtls.ReceiptVoucherId WHERE TblReceiptVoucherHdr.Id='" + id + "'", Conn);



        //SqlDataAdapter da = new SqlDataAdapter("SELECT tblPurchaseOrderHdr.Id,VendorName,Description,Pono,PoDate,RefNo,Mobileno,KindAtt,DeliveryAddress,EmailId,GstNo,VehicelNo,PayTerm,Cgst,Sgst,Igst,AllTotalPrice,TotalInWord, RoundOff, GrandTotal, Term_Condition_1, Term_Condition_2, Term_Condition_3, Term_Condition_4, Description, Hsn_Sac,CreatedOn, TaxPercenteage, Quantity, Unit, Rate, DiscountPercentage, Total,RefNo from tblPurchaseOrderHdr INNER JOIN tblPurchaseOrderDtls ON tblPurchaseOrderHdr.Id = tblPurchaseOrderDtls.PurchaseId WHERE tblPurchaseOrderHdr.Id='" + id + "'", Conn);
        da.Fill(Dt);
        //GvPurchaseOrderList.DataSource = Dt;
        //GvPurchaseOrderList.DataBind();

        StringWriter sw = new StringWriter();
        StringReader sr = new StringReader(sw.ToString());

        Document doc = new Document(PageSize.A4, 10f, 10f, 55f, 0f);

        PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(Server.MapPath("~/Files/") + "PaymentVoucher.pdf", FileMode.Create));
        //PdfWriter writer = PdfWriter.GetInstance(doc, Response.OutputStream);


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
        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Survey No. 27, Nimbalkar Nagar, Near Raghunandan Karyalay,", 145, 755, 0);
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

        PdfContentByte cd = writer.DirectContent;
        cd.Rectangle(17f, 680f, 560f, 25f);
        cd.Stroke();
        // Header 
        cd.BeginText();
        cd.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 17);
        cd.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "PAYMENT VOUCHER", 225, 687, 0);
        cd.EndText();

        if (Dt.Rows.Count > 0)
        {
            var CreateDate = DateTime.Now.ToString("dd-MM-yyyy");
            string VendorName = Dt.Rows[0]["Vendorname"].ToString();
            string PaymentVoucherNo = Dt.Rows[0]["PaymentVoucherNo"].ToString();
            string PaymentDate = Dt.Rows[0]["PaymentDate"].ToString().TrimEnd("0:0".ToCharArray());
            string Address = Dt.Rows[0]["Address"].ToString();
            string PaymentMode = Dt.Rows[0]["PaymentMode"].ToString();
            string ChequeTransNo = Dt.Rows[0]["ChequeTransNo"].ToString();
            string DrownOnBankCash = Dt.Rows[0]["DrownOnBankCash"].ToString();
            string CreatedDate = Dt.Rows[0]["CreatedDate"].ToString().TrimEnd("0:0".ToCharArray());
            string TotalAmount = Dt.Rows[0]["TotalAmount"].ToString();
            string AmountInWord = Dt.Rows[0]["TotalInWord"].ToString();

            Paragraph paragraphTable1 = new Paragraph();
            paragraphTable1.SpacingBefore = 100f;
            paragraphTable1.SpacingAfter = 10f;

            PdfPTable table = new PdfPTable(4);

            float[] widths2 = new float[] { 100, 180, 100, 180 };
            table.SetWidths(widths2);
            table.TotalWidth = 560f;
            table.LockedWidth = true;

            DateTime ffff1 = Convert.ToDateTime(Dt.Rows[0]["PaymentDate"].ToString());
            string datee = ffff1.ToString("dd-MM-yyyy");

            DateTime ffff2 = Convert.ToDateTime(Dt.Rows[0]["CreatedDate"].ToString());
            string dateee = ffff2.ToString("dd-MM-yyyy");

            table.AddCell(new Phrase("Vendor Name : ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(VendorName, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("Voucher No : ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(PaymentVoucherNo, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("Address : ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(Address, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("Payment Date : ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(PaymentDate, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("Payment Mode : ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(PaymentMode, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("Cheq. Tran. No. : ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(ChequeTransNo, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("Drawn On Bank / Cash : ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(DrownOnBankCash, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("Create Date ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(CreatedDate, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            paragraphTable1.Add(table);
            doc.Add(paragraphTable1);

            Paragraph paragraphTable2 = new Paragraph();
            paragraphTable2.SpacingAfter = 0f;
            table = new PdfPTable(4);
            float[] widths3 = new float[] { 4f, 19f, 10f, 8f };
            table.SetWidths(widths3);

            //------------HEADER END-----------------


            if (Dt.Rows.Count > 0)
            {
                table.TotalWidth = 560f;
                table.LockedWidth = true;
                table.AddCell(new Phrase("    Sr.No.", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("                                 Voucher No.", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("            Voucher Date", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("           Amount", FontFactory.GetFont("Arial", 10, Font.BOLD)));

                int rowid = 1;
                foreach (DataRow dr in Dt.Rows)
                {
                    table.TotalWidth = 560f;
                    table.LockedWidth = true;

                    table.AddCell(new Phrase(rowid.ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(dr["PaymentVoucherNo"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(dr["PaymentDate"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(dr["TotalAmount"].ToString(), FontFactory.GetFont("Arial", 9)));

                }

            }
            paragraphTable2.Add(table);
            doc.Add(paragraphTable2);

            Font font12 = FontFactory.GetFont("Arial", 12, Font.BOLD);
            Font font10 = FontFactory.GetFont("Arial", 10, Font.BOLD);
            Paragraph paragraph = new Paragraph("", font12);

            table = new PdfPTable(4);
            table.TotalWidth = 560f;
            table.LockedWidth = true;
            table.SetWidths(new float[] { 4f, 19f, 10f, 8f });
            table.AddCell(paragraph);
            table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 20, Font.BOLD)));
            table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 20, Font.BOLD)));
            table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));


            table.AddCell(new Phrase("  \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));

            //  }
            doc.Add(table);

            //Add Total Row start
            Paragraph paragraphTable5 = new Paragraph();



            table = new PdfPTable(3);
            table.TotalWidth = 560f;
            table.LockedWidth = true;

            table.SetWidths(new float[] { 0f, 49.50f, 12f });
            table.AddCell(paragraph);
            PdfPCell cell2t = new PdfPCell(new Phrase("Total", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            cell2t.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cell2t);
            PdfPCell cell3t = new PdfPCell(new Phrase(TotalAmount.ToString(), FontFactory.GetFont("Arial", 10, Font.BOLD)));
            cell3t.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cell3t);
            doc.Add(table);

            table = new PdfPTable(3);
            table.TotalWidth = 560f;
            table.LockedWidth = true;
            table.SetWidths(new float[] { 0f, 25f, 63f });
            table.AddCell(paragraph);
            PdfPCell cell66 = new PdfPCell(new Phrase("Amount In Words Rs. ", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            cell66.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell66);
            PdfPCell cell77 = new PdfPCell(new Phrase(AmountInWord, FontFactory.GetFont("Arial", 10, Font.BOLD)));
            cell77.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell77);
            doc.Add(table);

            table = new PdfPTable(2);
            table.TotalWidth = 560f;
            table.LockedWidth = true;
            table.SetWidths(new float[] { 310f, 250f });

            //table.AddCell(paragraph11);
            table.AddCell(new Phrase("\b\n\n\b\n\n\b\n\n\b\n\n\b                                      Receiver's Signature ", FontFactory.GetFont("Arial", 10)));
            table.AddCell(new Phrase("\b      For,\n\n\b\b\b\b\b\b\b\b\b         ENDEAVOUR AUTOMATION\n\n\n\n\n                          Authorised Signatory", FontFactory.GetFont("Arial", 11, Font.BOLD)));

            doc.Add(table);
        }
        doc.Close();
        ifrRight6.Attributes["src"] = @"../Files/" + "PaymentVoucher.pdf";
    }
}