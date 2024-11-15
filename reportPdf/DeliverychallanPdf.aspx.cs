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

public partial class DeliverychallanPdf : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
    string id;
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            if (Request.QueryString["ChallanNo"] != null)
            {
                id = Decrypt(Request.QueryString["ChallanNo"].ToString());
                Pdf(id);
            }
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

    private void Pdfwithtotal(string id)
    {
        DataTable Dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter("select * from vw_DeliveryChallan where ChallanNo='" + id + "'", con);
        da.Fill(Dt);
        //gv_Deliverychallan.DataSource = Dt;
        //gv_Deliverychallan.DataBind();

        StringWriter sw = new StringWriter();
        StringReader sr = new StringReader(sw.ToString());

        Document doc = new Document(PageSize.A4, 10f, 10f, 55f, 0f);

        PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(Server.MapPath("~/Files/") + "DeliveryChallan.pdf", FileMode.Create));
        //PdfWriter writer = PdfWriter.GetInstance(doc, Response.OutputStream);
        XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, sr);

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
        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "S.N.9/2/A1,Sakpal Wasti,Behind Genesis furniture,Nr.HP Petrol Pump,Ravet Road,", 130, 755, 0);
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
        cd.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Delivery Challan", 245, 667, 0);
        cd.EndText();

        if (Dt.Rows.Count > 0)
        {
            var CreateDate = DateTime.Now.ToString("yyyy-MM-dd");
            string CustomerName = Dt.Rows[0]["CustomerName"].ToString();
            string PoNumber = Dt.Rows[0]["PONo"].ToString();
            string Address = Dt.Rows[0]["Address"].ToString();
            string Delivery = Dt.Rows[0]["Delivery"].ToString();
            string PODate = Dt.Rows[0]["PODate"].ToString().TrimEnd("0:0".ToCharArray());
            string MobileNo = Dt.Rows[0]["MobileNo"].ToString();
            string GSTNo = Dt.Rows[0]["GstNo"].ToString();
            string KindAtt = Dt.Rows[0]["KindAtt"].ToString();
            string TotalInWord = Dt.Rows[0]["TotalInWord"].ToString();
            string GrandTotal = Dt.Rows[0]["GrandTotal"].ToString();
            string CGST = Dt.Rows[0]["Cgst"].ToString();
            string SGST = Dt.Rows[0]["Sgst"].ToString();
            string IGST = Dt.Rows[0]["Igst"].ToString();
            string Total = Dt.Rows[0]["GrandTotal"].ToString();
            string statecode = Dt.Rows[0]["statecode"].ToString();
            string challaNo = Dt.Rows[0]["ChallanNo"].ToString();
            string challanDate = Dt.Rows[0]["ChallanDate"].ToString().TrimEnd("0:0".ToCharArray()); ;
            string Basic = Dt.Rows[0]["Ftotal"].ToString();

            Paragraph paragraphTable1 = new Paragraph();
            paragraphTable1.SpacingBefore = 120f;
            paragraphTable1.SpacingAfter = 10f;

            PdfPTable table = new PdfPTable(4);

            float[] widths2 = new float[] { 100, 180, 100, 180 };
            table.SetWidths(widths2);
            table.TotalWidth = 560f;
            table.LockedWidth = true;

            DateTime ffff1 = Convert.ToDateTime(Dt.Rows[0]["PODate"].ToString());
            string datee = ffff1.ToString("yyyy-MM-dd");
            DateTime ffff2 = Convert.ToDateTime(Dt.Rows[0]["ChallanDate"].ToString());
            string Dateeee = ffff1.ToString("yyyy-MM-dd");

            table.AddCell(new Phrase("   Customer Name : ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("   " + CustomerName, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("   Challan No :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("   " + challaNo, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("   Address:", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("   " + Address, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("  Delivery Address:", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("   " + Delivery, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("   Date :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("   " + challanDate, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("   GSt No :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("   " + GSTNo, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("   PO No :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("   " + PoNumber, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("   KinD Att :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("   " + KindAtt, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 9, Font.BOLD)));

            paragraphTable1.Add(table);
            doc.Add(paragraphTable1);

            Paragraph paragraphTable2 = new Paragraph();
            paragraphTable2.SpacingAfter = 0f;
            table = new PdfPTable(9);
            float[] widths3 = new float[] { 4f, 19f, 10f, 8f, 8f, 8f, 8f, 8f, 8f };
            table.SetWidths(widths3);

            double Ttotal_price = 0;
            if (Dt.Rows.Count > 0)
            {
                table.TotalWidth = 560f;
                table.LockedWidth = true;
                table.AddCell(new Phrase("SN.", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("             Description", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("    Hsn / Sac", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("    Tax %", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase(" Quantity", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("     Unit", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("   Rate", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("  Disc %", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("  Total", FontFactory.GetFont("Arial", 10, Font.BOLD)));

                int rowid = 1;
                foreach (DataRow dr in Dt.Rows)
                {
                    table.TotalWidth = 560f;
                    table.LockedWidth = true;

                   // double Ftotal = Convert.ToDouble(dr["GrandTotal"].ToString());
                    double Ftotal = Convert.ToDouble(dr["Ftotal"].ToString());
                    string _ftotal = Ftotal.ToString("##.00");
                    table.AddCell(new Phrase("     " + rowid.ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(" " + dr["Description"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase("           " + dr["HSN_code"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase("           " + dr["Tax"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase("            " + dr["Qty"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase("         " + dr["Units"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(dr["Rate"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(dr["Discount"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(_ftotal, FontFactory.GetFont("Arial", 9)));
                    rowid++;
                
                   // Ttotal_price += Convert.ToDouble(dr["GrandTotal"].ToString());
                    Ttotal_price += Convert.ToDouble(dr["Ftotal"].ToString());
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
            table.SetWidths(new float[] { 4f, 19f, 10f, 8f, 8f, 8f, 8f, 8f, 8f});
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
                table.AddCell(new Phrase("  \n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                //doc.Add(table);
            }
            else if (Dt.Rows.Count >= 7 && Dt.Rows.Count <= 9)
            {
                table.AddCell(new Phrase("  \n\n\n\n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));

            }
            else if (Dt.Rows.Count >= 4 && Dt.Rows.Count <= 6)
            {
                table.AddCell(new Phrase("  \n\n\n\n\n\n\n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));

            }
            else if (Dt.Rows.Count < 4)
            {
                table.AddCell(new Phrase("  \n\n\n\n\n\n\n\n\n\n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));
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
            PdfPCell cell11 = new PdfPCell(new Phrase(Ttotal_price.ToString(), FontFactory.GetFont("Arial", 10, Font.BOLD)));
            cell11.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cell11);

            doc.Add(table);
            //add total row end

            if (statecode == "27 MAHARASHTRA")
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

                //IGST 9% Row STart
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
            }
            else
            {
                //SGST 9% Row STart
                Paragraph paragraphTable166 = new Paragraph();
                paragraphTable5.SpacingAfter = 10f;

                string[] itemm = { "Goods once sold will not be taken back or exchange. \b",
                        "Interest at the rate of 18% will be charged if bill is'nt paid within 30 days.\b",
                        "Our risk and responsibility ceases the moment goods leaves out godown. \n",
                        };

                Font font144 = FontFactory.GetFont("Arial", 12, Font.BOLD);
                Font font155 = FontFactory.GetFont("Arial", 10, Font.BOLD);
                Paragraph paragraphhhhh = new Paragraph("", font12);

                for (int i = 0; i < items.Length; i++)
                {
                    paragraph.Add(new Phrase("", font10));
                }

                table = new PdfPTable(3);
                table.TotalWidth = 560f;
                table.LockedWidth = true;

                table.SetWidths(new float[] { 0f, 76f, 12f });
                table.AddCell(paragraph);
                PdfPCell cell222 = new PdfPCell(new Phrase("IGST %", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                cell222.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell222);
                PdfPCell cell333 = new PdfPCell(new Phrase(IGST, FontFactory.GetFont("Arial", 10, Font.BOLD)));
                cell333.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell333);

                doc.Add(table);
                //IGST 9% Row End
            }

            //Grand total Row STart
            Paragraph paragraphTable17 = new Paragraph();
            paragraphTable5.SpacingAfter = 10f;

            string[] itemmm = { "Goods once sold will not be taken back or exchange. \b",
                        "Interest at the rate of 18% will be charged if bill is'nt paid within 30 days.\b",
                        "Our risk and responsibility ceases the moment goods leaves out godown. \n",
                        };

            Font font16 = FontFactory.GetFont("Arial", 12, Font.BOLD);
            Font font17 = FontFactory.GetFont("Arial", 10, Font.BOLD);
            Paragraph paragraphhhhhh = new Paragraph("", font12);

            //paragraphh.SpacingAfter = 10f;

            for (int i = 0; i < items.Length; i++)
            {
                paragraph.Add(new Phrase("", font10));
            }

            table = new PdfPTable(3);
            table.TotalWidth = 560f;
            table.LockedWidth = true;

            var Grndttl = Convert.ToDecimal(Ttotal_price) + Convert.ToDecimal(CGST) + Convert.ToDecimal(SGST);

            table.SetWidths(new float[] { 0f, 76f, 12f });
            table.AddCell(paragraph);
            PdfPCell cell44 = new PdfPCell(new Phrase("Grand Total", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            cell44.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cell44);
            PdfPCell cell55 = new PdfPCell(new Phrase(Total.ToString(), FontFactory.GetFont("Arial", 10, Font.BOLD)));
            cell55.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cell55);

            doc.Add(table);
            //Grand total Row End

            //Grand total in word Row STart
            Paragraph paragraphTable18 = new Paragraph();
            paragraphTable18.SpacingAfter = 50f;

            string[] itemmhm = { "Goods once sold will not be taken back or exchange. \b",
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


            table = new PdfPTable(1);
            table.TotalWidth = 560f;
            table.LockedWidth = true;

            table.SetWidths(new float[] { 560f });
            table.AddCell(paragraph);
            PdfPCell cell666 = new PdfPCell(new Phrase("                                                                                                                                    For ENDEAVOUR AUTOMATION\n\n\n\n\n\n\n\n\n                                                                                                                                             Authorized Signature", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            cell666.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell666);
            PdfPCell cell777 = new PdfPCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            cell777.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell777);
            doc.Add(table);

            //PdfContentByte cn = writer.DirectContent;
            //cn.Rectangle(17f, 225f, 560f, 115f);
            //cn.Stroke();
            //// Header 
            //cn.BeginText();
            //cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 14);
            //cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Delivery Address :", 30, 325, 0);
            //cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibri.ttf", "Identity-H", BaseFont.EMBEDDED), 11);
            //cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "6609-NOT REPAIRED / 6610-REPAIRED / 6611 - REPAIRED( REPAIRED VFD PO NOT RECD SO DISPACHED CHALLAN BASIS)", 30, 306, 0);
            //cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibri.ttf", "Identity-H", BaseFont.EMBEDDED), 12);
            //cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Subject To Pune Jurisdiction Only", 30, 293, 0);
            //cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibri.ttf", "Identity-H", BaseFont.EMBEDDED), 12);
            //cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Received above mterial duly checked.", 30, 255, 0);
            //cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibri.ttf", "Identity-H", BaseFont.EMBEDDED), 12);
            //cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Customer's Signature & Stamp", 30, 239, 0);
            ////cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibri.ttf", "Identity-H", BaseFont.EMBEDDED), 12);
            ////cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "IFSC/Neft Code :BARB0KALEWA", 30, 180, 0);

            ////cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibrii.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
            ////cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "We Thank you for your enquiry", 440, 256, 0);
            ////cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 12);
            ////cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "For", 392, 230, 0);
            //cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 13);
            //cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "For ENDEAVOUR AUTOMATION", 400, 290, 0);
            //cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 13);
            //cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Authorised Signatory", 443, 230, 0);
            //cn.EndText();

            Paragraph paragraphTable4 = new Paragraph();

            paragraphTable4.SpacingBefore = 10f;

            table = new PdfPTable(2);
            table.TotalWidth = 560f;

            float[] widths = new float[] { 160f, 400f };
            table.SetWidths(widths);
            table.LockedWidth = true;

            doc.Close();

            Byte[] FileBuffer = File.ReadAllBytes(Server.MapPath("~/Files/") + "DeliveryChallan.pdf");
            //string empFilename = "DeliveryChallan" + DateTime.Now.ToShortDateString() + ".pdf";

            //if (FileBuffer != null)
            //{
            //    Response.ContentType = "application/pdf";
            //    Response.AddHeader("content-length", FileBuffer.Length.ToString());
            //    Response.BinaryWrite(FileBuffer);
            //    Response.AddHeader("Content-Disposition", "attachment;filename=" + empFilename);
            //}

        }
        doc.Close();
        ifrRight6.Attributes["src"] = @"../Files/" + "DeliveryChallan.pdf";
    }



    private void Pdf(string id)
    {
        DataTable Dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter("select * from vw_DeliveryChallan where ChallanNo='" + id + "'", con);
        da.Fill(Dt);
        //gv_Deliverychallan.DataSource = Dt;
        //gv_Deliverychallan.DataBind();

        StringWriter sw = new StringWriter();
        StringReader sr = new StringReader(sw.ToString());

        Document doc = new Document(PageSize.A4, 10f, 10f, 55f, 0f);

        PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(Server.MapPath("~/Files/") + "DeliveryChallan.pdf", FileMode.Create));
        //PdfWriter writer = PdfWriter.GetInstance(doc, Response.OutputStream);
        XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, sr);

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
        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "S.N.9/2/A1,Sakpal Wasti,Behind Genesis furniture,Nr.HP Petrol Pump,Ravet Road,", 130, 755, 0);
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
        cd.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Delivery Challan", 245, 667, 0);
        cd.EndText();

        if (Dt.Rows.Count > 0)
        {
            var CreateDate = DateTime.Now.ToString("yyyy-MM-dd");
            string CustomerName = Dt.Rows[0]["CustomerName"].ToString();
            string PoNumber = Dt.Rows[0]["PONo"].ToString();
            string Address = Dt.Rows[0]["Address"].ToString();
            string Delivery = Dt.Rows[0]["Delivery"].ToString();
            string PODate = Dt.Rows[0]["PODate"].ToString().TrimEnd("0:0".ToCharArray());
            string MobileNo = Dt.Rows[0]["MobileNo"].ToString();
            string GSTNo = Dt.Rows[0]["GstNo"].ToString();
            string KindAtt = Dt.Rows[0]["KindAtt"].ToString();
            string TotalInWord = Dt.Rows[0]["TotalInWord"].ToString();
            string GrandTotal = Dt.Rows[0]["GrandTotal"].ToString();
            string CGST = Dt.Rows[0]["Cgst"].ToString();
            string SGST = Dt.Rows[0]["Sgst"].ToString();
            string Total = Dt.Rows[0]["GrandTotal"].ToString();
            string challaNo = Dt.Rows[0]["ChallanNo"].ToString();
            string challanDate = Dt.Rows[0]["ChallanDate"].ToString().TrimEnd("0:0".ToCharArray()); ;

            Paragraph paragraphTable1 = new Paragraph();
            paragraphTable1.SpacingBefore = 120f;
            paragraphTable1.SpacingAfter = 10f;

            PdfPTable table = new PdfPTable(4);

            float[] widths2 = new float[] { 100, 180, 100, 180 };
            table.SetWidths(widths2);
            table.TotalWidth = 560f;
            table.LockedWidth = true;

            DateTime ffff1 = Convert.ToDateTime(Dt.Rows[0]["PODate"].ToString());
            string datee = ffff1.ToString("yyyy-MM-dd");
            DateTime ffff2 = Convert.ToDateTime(Dt.Rows[0]["ChallanDate"].ToString());
            string Dateeee = ffff1.ToString("yyyy-MM-dd");

            table.AddCell(new Phrase("   Customer Name : ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("   " + CustomerName, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("   Challan No :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("   " + challaNo, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("   Address:", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("   " + Address, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("  Delivery Address:", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("   " + Delivery, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("   Date :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("   " + challanDate, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("   GSt No :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("   " + GSTNo, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("   PO No :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("   " + PoNumber, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("   KinD Att :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("   " + KindAtt, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 9, Font.BOLD)));

            paragraphTable1.Add(table);
            doc.Add(paragraphTable1);

            Paragraph paragraphTable2 = new Paragraph();
            paragraphTable2.SpacingAfter = 0f;
            table = new PdfPTable(6);
            float[] widths3 = new float[] { 4f, 19f, 10f, 8f, 8f, 8f};
            table.SetWidths(widths3);

            double Ttotal_price = 0;
            if (Dt.Rows.Count > 0)
            {
                table.TotalWidth = 560f;
                table.LockedWidth = true;
                table.AddCell(new Phrase("   SN.", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("                     Description", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("        Hsn / Sac", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("        Tax %", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("      Quantity", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("        Unit", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                //table.AddCell(new Phrase("Rate", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                //table.AddCell(new Phrase("Disc %", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                //table.AddCell(new Phrase("Total", FontFactory.GetFont("Arial", 10, Font.BOLD)));

                int rowid = 1;
                foreach (DataRow dr in Dt.Rows)
                {
                    table.TotalWidth = 560f;
                    table.LockedWidth = true;

                    double Ftotal = Convert.ToDouble(dr["GrandTotal"].ToString());
                    string _ftotal = Ftotal.ToString("##.00");
                    table.AddCell(new Phrase("     " + rowid.ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(" " + dr["Description"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase("           " +dr["HSN_code"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase("           " +dr["Tax"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase("            " +dr["Qty"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase("         " + dr["Units"].ToString(), FontFactory.GetFont("Arial", 9)));
                    //table.AddCell(new Phrase(dr["Rate"].ToString(), FontFactory.GetFont("Arial", 9)));
                    //table.AddCell(new Phrase(dr["Discount"].ToString(), FontFactory.GetFont("Arial", 9)));
                    //table.AddCell(new Phrase(_ftotal, FontFactory.GetFont("Arial", 9)));
                    rowid++;

                    Ttotal_price += Convert.ToDouble(dr["GrandTotal"].ToString());
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

            table = new PdfPTable(6);
            table.TotalWidth = 560f;
            table.LockedWidth = true;
            table.SetWidths(new float[] { 4f, 19f, 10f, 8f, 8f, 8f });
            table.AddCell(paragraph);
            table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            //table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            //table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            //table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            //table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            //table.AddCell(new Phrase("  \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            if (Dt.Rows.Count >= 10)
            {
                table.AddCell(new Phrase("  \n\n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                //doc.Add(table);
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
                table.AddCell(new Phrase("  \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));
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

            //table.SetWidths(new float[] { 0f, 76f, 12f });
            //table.AddCell(paragraph);
            //PdfPCell cell2 = new PdfPCell(new Phrase("CGST %", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            //cell2.HorizontalAlignment = Element.ALIGN_RIGHT;
            //table.AddCell(cell2);
            //PdfPCell cell3 = new PdfPCell(new Phrase(CGST, FontFactory.GetFont("Arial", 10, Font.BOLD)));
            //cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
            //table.AddCell(cell3);

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

            //table.SetWidths(new float[] { 0f, 76f, 12f });
            //table.AddCell(paragraph);
            //PdfPCell cell22 = new PdfPCell(new Phrase("SGST %", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            //cell22.HorizontalAlignment = Element.ALIGN_RIGHT;
            //table.AddCell(cell22);
            //PdfPCell cell33 = new PdfPCell(new Phrase(SGST, FontFactory.GetFont("Arial", 10, Font.BOLD)));
            //cell33.HorizontalAlignment = Element.ALIGN_RIGHT;
            //table.AddCell(cell33);

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

            //var Grndttl = Convert.ToDecimal(Ttotal_price) + Convert.ToDecimal(CGST) + Convert.ToDecimal(SGST);

            //table.SetWidths(new float[] { 0f, 76f, 12f });
            //table.AddCell(paragraph);
            //PdfPCell cell44 = new PdfPCell(new Phrase("Grand Total", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            //cell44.HorizontalAlignment = Element.ALIGN_RIGHT;
            //table.AddCell(cell44);
            //PdfPCell cell55 = new PdfPCell(new Phrase(Grndttl.ToString(), FontFactory.GetFont("Arial", 10, Font.BOLD)));
            //cell55.HorizontalAlignment = Element.ALIGN_RIGHT;
            //table.AddCell(cell55);

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

            table = new PdfPTable(1);
            table.TotalWidth = 560f;
            table.LockedWidth = true;

            table.SetWidths(new float[] { 560f });
            table.AddCell(paragraph);
            PdfPCell cell66 = new PdfPCell(new Phrase("                                                                                                                                    For ENDEAVOUR AUTOMATION\n\n\n\n\n\n\n\n\n                                                                                                                                             Authorized Signature", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            cell66.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell66);
            PdfPCell cell77 = new PdfPCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            cell77.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell77);
            doc.Add(table);

            //PdfContentByte cn = writer.DirectContent;
            //cn.Rectangle(17f, 225f, 560f, 115f); 
            //cn.Stroke();
            //// Header 
            //cn.BeginText();
            //cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 14);
            //cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Delivery Address :", 30, 325, 0);
            //cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibri.ttf", "Identity-H", BaseFont.EMBEDDED), 11);
            //cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "6609-NOT REPAIRED / 6610-REPAIRED / 6611 - REPAIRED( REPAIRED VFD PO NOT RECD SO DISPACHED CHALLAN BASIS)", 30, 306, 0);
            //cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibri.ttf", "Identity-H", BaseFont.EMBEDDED), 12);
            //cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Subject To Pune Jurisdiction Only", 30, 293, 0);
            //cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibri.ttf", "Identity-H", BaseFont.EMBEDDED), 12);
            //cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Received above mterial duly checked.", 30, 255, 0);
            //cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibri.ttf", "Identity-H", BaseFont.EMBEDDED), 12);
            //cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Customer's Signature & Stamp", 30, 239, 0);
            ////cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibri.ttf", "Identity-H", BaseFont.EMBEDDED), 12);
            ////cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "IFSC/Neft Code :BARB0KALEWA", 30, 180, 0);

            ////cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibrii.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
            ////cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "We Thank you for your enquiry", 440, 256, 0);
            ////cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 12);
            ////cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "For", 392, 230, 0);
            //cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 13);
            //cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "For ENDEAVOUR AUTOMATION", 400, 290, 0);
            //cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 13);
            //cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Authorized Signature", 443, 230, 0);
            //cn.EndText();

            Paragraph paragraphTable4 = new Paragraph();

            paragraphTable4.SpacingBefore = 10f;

            table = new PdfPTable(2);
            table.TotalWidth = 560f;

            float[] widths = new float[] { 160f, 400f };
            table.SetWidths(widths);
            table.LockedWidth = true;

            doc.Close();

            Byte[] FileBuffer = File.ReadAllBytes(Server.MapPath("~/Files/") + "DeliveryChallan.pdf");
            //string empFilename = "DeliveryChallan" + DateTime.Now.ToShortDateString() + ".pdf";

            //if (FileBuffer != null)
            //{
            //    Response.ContentType = "application/pdf";
            //    Response.AddHeader("content-length", FileBuffer.Length.ToString());
            //    Response.BinaryWrite(FileBuffer);
            //    Response.AddHeader("Content-Disposition", "attachment;filename=" + empFilename);
            //}

        }
        doc.Close();
        ifrRight6.Attributes["src"] = @"../Files/" + "DeliveryChallan.pdf";
    }

    protected void btn_withtotal_pdf_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["ChallanNo"] != null)
        {
            id = Decrypt(Request.QueryString["ChallanNo"].ToString());
            Pdfwithtotal(id);
        }
    }

    protected void btn_pdf_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["ChallanNo"] != null)
        {
            id = Decrypt(Request.QueryString["ChallanNo"].ToString());
            Pdf(id);
        }
    }
}