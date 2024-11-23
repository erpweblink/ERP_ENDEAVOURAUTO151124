using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Drawing.Printing;

public partial class QuatationPdf : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
    string ID = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["Quotation_no"] != null)
        {
            ID = Decrypt(Request.QueryString["Quotation_no"].ToString());
            PDF(ID);
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

    public void PDF(string Quo_NO)
    {

        //SqlDataAdapter Da = new SqlDataAdapter("SELECT * FROM vw_Quot_pdf WHERE Quotation_no='" + Quo_NO + "'  ", con);
        SqlDataAdapter Da = new SqlDataAdapter("SELECT * FROM vw_Two_Quot_pdf_sales WHERE Quotation_no='" + Quo_NO + "'  ", con);
        DataTable Dt = new DataTable();
        Da.Fill(Dt);

        StringWriter sw = new StringWriter();
        StringReader sr = new StringReader(sw.ToString());

        Document doc = new Document(PageSize.A4, 10f, 10f, 55f, 0f);

        PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(Server.MapPath("~/Files/") + "QuotationInvoice.pdf", FileMode.Create));
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
        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "ENDEAVOUR AUTOMATION", 185, 773, 0);
        cb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 11);
        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "S.N.9/2/A1,Sakpal Wasti,Behind Genesis furniture,Nr.HP Petrol Pump,Ravet Road,", 130, 755, 0);
        cb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 11);
        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Tathawade, Pune-411033", 232, 740, 0);
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
        cd.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "QUOTATION", 260, 667, 0);
        cd.EndText();

        Paragraph paragraphTable1 = new Paragraph();
        paragraphTable1.SpacingBefore = 120f;
        paragraphTable1.SpacingAfter = 10f;

        PdfPTable table = new PdfPTable(4);

        float[] widths2 = new float[] { 100, 180, 100, 180 };
        table.SetWidths(widths2);
        table.TotalWidth = 560f;
        table.LockedWidth = true;

        if (Dt.Rows.Count > 0)
        {
            var date = DateTime.Now.ToString("dd-MM-yyyy");

            string compname = Dt.Rows[0]["Customer_Name"].ToString();
            string Quo_No = Dt.Rows[0]["Quotation_no"].ToString();
            string Address = Dt.Rows[0]["Address"].ToString();
            //string Quo_Dt = Dt.Rows[0]["Quotation_Date"].ToString();
            string state = Dt.Rows[0]["State_Code"].ToString();
            string GST_NO = Dt.Rows[0]["GST_No"].ToString();
            string kindAtt = Dt.Rows[0]["kind_Att"].ToString();
            string amt_in_Word = Dt.Rows[0]["Total_in_word"].ToString();
            string cgstper = Dt.Rows[0]["CGST"].ToString();
            string igstper = Dt.Rows[0]["IGST"].ToString();
            string Ftotall = Dt.Rows[0]["total"].ToString();
            string sgstper = Dt.Rows[0]["SGST"].ToString();
            string Tax = Dt.Rows[0]["Tax"].ToString();
            string TagNo = Dt.Rows[0]["JobNo"].ToString();
            string RefNo = Dt.Rows[0]["ID"].ToString();
            DateTime ffff1 = Convert.ToDateTime(Dt.Rows[0]["Quotation_Date"].ToString());
            string datee = ffff1.ToString("dd-MM-yyyy");
            DateTime dateexp = Convert.ToDateTime(Dt.Rows[0]["ExpiryDate"].ToString());
            string expdate = dateexp.ToString("dd-MM-yyyy");
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


            table.AddCell(new Phrase("  Company Name : ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("   " + compname, FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("  Quotation No. :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("   " + Quo_No, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("  Address :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("   " + Address, FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("  Quotation Date :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("   " + datee, FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("  State :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("   " + state, FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("  GST No. :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("   " + GST_NO, FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("  Ref.No. :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("   " + RefNo, FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("  Ref. Date :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("   " + date, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("  Tag No.:", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("   " + TagNo, FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("  Expiry Date", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("   " + expdate, FontFactory.GetFont("Arial", 9, Font.BOLD)));



            paragraphTable1.Add(table);
            doc.Add(paragraphTable1);

            //////////////////////Kintt Atte start.

            string[] items1 = { "Dear Sir/Madam,\b", "We acknowledge with thanks the receipt of your above enquiry and have great pleasure in submitting our lowest ,\b", "quotation subject to the conditions printed below.\n" };

            Font font121 = FontFactory.GetFont("Arial", 11, Font.BOLD);
            Font font101 = FontFactory.GetFont("Arial", 9);
            Paragraph paragraph1 = new Paragraph(" Kind Att:  " + kindAtt + "\n\n", font121);

            for (int i = 0; i < items1.Length; i++)
            {
                paragraph1.Add(new Phrase(" " + items1[i] + "\n", font101));
            }

            table = new PdfPTable(3);
            table.TotalWidth = 560f;
            table.LockedWidth = true;
            table.SetWidths(new float[] { 310f, 0f, 0f });

            table.AddCell(paragraph1);
            table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));

            doc.Add(table);

            /////////////////////kind Atte End.

            ///////////Add product details Titles

            string[] items90product = { "Dear Sir/Madam,\b", "We acknowledge with thanks the receipt of your above enquiry and have great pleasure in submitting our lowest quotation,\b", "subject to the conditions printed below.\n" };

            Font font121121 = FontFactory.GetFont("Arial", 12, Font.BOLD);
            Font font101111 = FontFactory.GetFont("Arial", 10);
            Paragraph paragraph1101 = new Paragraph("\b                                                                      Product Details", font121121);

            for (int i = 0; i < items90product.Length; i++)
            {
                paragraph1101.Add(new Phrase(" ", font101111));
            }

            table = new PdfPTable(1);
            table.TotalWidth = 560f;
            table.LockedWidth = true;
            table.SetWidths(new float[] { 310f });

            table.AddCell(paragraph1101);

            //table.AddCell(new Phrase("\bBank Name : BANK OF BARODA\n\n\bAccount Name : ENDEAVOUR AUTOMATION\n\n\bBranch : KALEWADI\n\n\bA/c No : 46180200000214\n\n\bIFSC/Neft Code :BARB0KALEWA\n", FontFactory.GetFont("Arial", 10)));
            //table.AddCell(new Phrase("\n\n\b\b\b\b\b\b\b\b\b           For ENDEAVOUR AUTOMATION\n\n\n\n\n                                       Authorised Signatory", FontFactory.GetFont("Arial", 11, Font.BOLD)));

            doc.Add(table);


            /////////////////////////////////////////////////


            Paragraph paragraphTable2 = new Paragraph();
            paragraphTable2.SpacingAfter = 0f;
            table = new PdfPTable(9);
            float[] widths3 = new float[] { 4f, 12f, 10f, 8f, 8f, 11f, 8f, 8f, 12f };
            table.SetWidths(widths3);

            double Ttotal_price = 0;
            //DataTable Dt = Read_Table("SELECT * FROM vw_Quotation_Invoice");

            if (Dt.Rows.Count > 0)
            {
                table.TotalWidth = 560f;
                table.LockedWidth = true;
                table.AddCell(new Phrase("  SN.", FontFactory.GetFont("Arial", 10, Font.BOLD)));
               // table.AddCell(new Phrase("Product", FontFactory.GetFont("Arial", 10, Font.BOLD)));
               // table.AddCell(new Phrase("Component", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("   Description", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("   Hsn / Sac", FontFactory.GetFont("Arial", 10, Font.BOLD)));
               
                table.AddCell(new Phrase("  Quantity", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("     Unit", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("         Rate", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("   Tax(%)", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("  Disc.(%)", FontFactory.GetFont("Arial", 10, Font.BOLD))); 
                table.AddCell(new Phrase("        Total", FontFactory.GetFont("Arial", 10, Font.BOLD)));

                int rowid = 1;
                foreach (DataRow dr in Dt.Rows)
                {
                    table.TotalWidth = 560f;
                    table.LockedWidth = true;

                    double Ftotal = Convert.ToDouble(dr["total"].ToString());
                    string _ftotal = Ftotal.ToString("##.00");
                    table.AddCell(new Phrase(rowid.ToString(), FontFactory.GetFont("Arial", 9)));
                   // table.AddCell(new Phrase(dr["product"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(dr["Description"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase("        " +dr["HSN"].ToString(), FontFactory.GetFont("Arial", 9)));                 
                    table.AddCell(new Phrase("        " + dr["Qty"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase("      " + dr["Units"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase("    " + dr["Rate"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase("      " + dr["Tax"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase("      " + dr["Disc_per"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(_ftotal, FontFactory.GetFont("Arial", 9)));
                    rowid++;

                    Ttotal_price += Convert.ToDouble("    " + dr["total"].ToString());
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
            table.SetWidths(new float[] { 4f, 12f, 10f, 8f, 8f, 11f, 8f, 8f, 12f });
            table.AddCell(paragraph);
            table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));


            //doc.Add(table);
            if (Dt.Rows.Count >= 10)
            {
                table.AddCell(new Phrase("  \n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            }
            else if (Dt.Rows.Count >= 7 && Dt.Rows.Count <= 9)
            {
                table.AddCell(new Phrase("  \n\n\n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            }
            else if (Dt.Rows.Count >= 4 && Dt.Rows.Count <= 6)
            {
                table.AddCell(new Phrase("   \n\n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            }
            else if (Dt.Rows.Count < 4)
            {
                table.AddCell(new Phrase("  \n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            }
            doc.Add(table);
            //Add Total Row start
            Paragraph paragraphTable5 = new Paragraph();

            //paragraphTable5.SpacingAfter = 10f;   

            string[] itemsss = { "Goods once sold will not be taken back or exchange. \b",
                         "Interest at the rate of 18% will be charged if bill is'nt paid within 30 days.\b",
                         "Our risk and responsibility ceases the moment goods leaves out godown. \n",
                         };

            Font font13 = FontFactory.GetFont("Arial", 12, Font.BOLD);
            Font font11 = FontFactory.GetFont("Arial", 10, Font.BOLD);
            Paragraph paragraphh = new Paragraph("", font12);

            //paragraphh.SpacingAfter = 10f;

            for (int i = 0; i < items.Length; i++)
            {
                paragraph.Add(new Phrase("", font10));
            }

            table = new PdfPTable(3);
            table.TotalWidth = 560f;
            table.LockedWidth = true;

            table.SetWidths(new float[] { 0f, 81f, 12f });
            table.AddCell(paragraph);
            PdfPCell cell2t = new PdfPCell(new Phrase("Subtotal", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            cell2t.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cell2t);
            PdfPCell cell3t = new PdfPCell(new Phrase(Ttotal_price.ToString("#.00"), FontFactory.GetFont("Arial", 10, Font.BOLD)));
            cell3t.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cell3t);
            doc.Add(table);
            //add total row end




            if (state == "27 MAHARASHTRA")
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

                //paragraphh.SpacingAfter = 10f;

                for (int i = 0; i < items.Length; i++)
                {
                    paragraph.Add(new Phrase("", font10));
                }

                table = new PdfPTable(3);
                table.TotalWidth = 560f;
                table.LockedWidth = true;

                var Cgst_9 = Convert.ToDecimal(Tax) / 2;

                table.SetWidths(new float[] { 0f, 81f, 12f });
                table.AddCell(paragraph);
                PdfPCell cell2 = new PdfPCell(new Phrase("CGST-" + Cgst_9 + "%", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                cell2.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell2);
                PdfPCell cell3 = new PdfPCell(new Phrase(cgstper, FontFactory.GetFont("Arial", 10, Font.BOLD)));
                cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell3);

                doc.Add(table);
                //CGST 9% Row End

                //SGST 9% Row STart
                Paragraph paragraphTable16 = new Paragraph();
                paragraphTable5.SpacingAfter = 10f;

                //paragraphTable5.SpacingAfter = 10f;

                string[] item = { "Goods once sold will not be taken back or exchange. \b",
                         "Interest at the rate of 18% will be charged if bill is'nt paid within 30 days.\b",
                         "Our risk and responsibility ceases the moment goods leaves out godown. \n",
                         };

                Font font14 = FontFactory.GetFont("Arial", 12, Font.BOLD);
                Font font15 = FontFactory.GetFont("Arial", 10, Font.BOLD);
                Paragraph paragraphhhh = new Paragraph("", font12);

                //paragraphh.SpacingAfter = 10f;

                for (int i = 0; i < items.Length; i++)
                {
                    paragraph.Add(new Phrase("", font10));
                }

                table = new PdfPTable(3);
                table.TotalWidth = 560f;
                table.LockedWidth = true;

                var Sgst_9 = Convert.ToDecimal(Tax) / 2;


                table.SetWidths(new float[] { 0f, 81f, 12f });
                table.AddCell(paragraph);
                PdfPCell cell22 = new PdfPCell(new Phrase("SGST-" + Sgst_9 + "%", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                cell22.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell22);
                PdfPCell cell33 = new PdfPCell(new Phrase(sgstper, FontFactory.GetFont("Arial", 10, Font.BOLD)));
                cell33.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell33);
                doc.Add(table);
                //SGST 9% Row End
            }
            else
            {
                //IGST 9% Row STart
                Paragraph paragraphTable16igst = new Paragraph();
                paragraphTable5.SpacingAfter = 10f;

                //paragraphTable5.SpacingAfter = 10f;

                string[] itemigst = { "Goods once sold will not be taken back or exchange. \b",
                         "Interest at the rate of 18% will be charged if bill is'nt paid within 30 days.\b",
                         "Our risk and responsibility ceases the moment goods leaves out godown. \n",
                         };

                Font font14igst = FontFactory.GetFont("Arial", 12, Font.BOLD);
                Font font15igst = FontFactory.GetFont("Arial", 10, Font.BOLD);
                Paragraph paragraphhhhigst = new Paragraph("", font14igst);

                //paragraphh.SpacingAfter = 10f;

                for (int i = 0; i < itemigst.Length; i++)
                {
                    paragraph.Add(new Phrase("", font15igst));
                }

                table = new PdfPTable(3);
                table.TotalWidth = 560f;
                table.LockedWidth = true;

                var igst_9 = Convert.ToDecimal(Tax);

                table.SetWidths(new float[] { 0f, 81f, 12f });
                table.AddCell(paragraph);
                PdfPCell cell22igst = new PdfPCell(new Phrase("IGST-" + igst_9 + "%", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                cell22igst.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell22igst);
                PdfPCell cell33igst = new PdfPCell(new Phrase(igstper, FontFactory.GetFont("Arial", 10, Font.BOLD)));
                cell33igst.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell33igst);
                doc.Add(table);
                //IGST 9% Row End
            }

            //Grand total Row STart
            Paragraph paragraphTable17 = new Paragraph();
            paragraphTable5.SpacingAfter = 10f;

            //paragraphTable5.SpacingAfter = 10f;

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

            var Grndttl = Convert.ToDecimal(Ttotal_price) + Convert.ToDecimal(cgstper) + Convert.ToDecimal(sgstper) + Convert.ToDecimal(igstper);

            table.SetWidths(new float[] { 0f, 81f, 12f });
            table.AddCell(paragraph);
            PdfPCell cell44 = new PdfPCell(new Phrase("Grand Total", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            cell44.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cell44);
            PdfPCell cell55 = new PdfPCell(new Phrase(Grndttl.ToString("0.00"), FontFactory.GetFont("Arial", 10, Font.BOLD)));
            cell55.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cell55);
            doc.Add(table);
            //Grand total Row End

            //Grand total in word Row STart
            Paragraph paragraphTable18 = new Paragraph();
            paragraphTable18.SpacingAfter = 50f;

            //paragraphTable5.SpacingAfter = 10f;

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
            PdfPCell cell77 = new PdfPCell(new Phrase(amt_in_Word, FontFactory.GetFont("Arial", 10, Font.BOLD)));
            cell77.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell77);

            doc.Add(table);
            //Grand total in word Row End

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
            ////////term condition in Database end

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
            //table.AddCell(new Phrase("\bBank Name : BANK OF BARODA\n\n\bAccount Name : ENDEAVOUR AUTOMATION\n\n\bBranch : KALEWADI\n\n\bA/c No : 46180200000214\n\n\bIFSC/Neft Code :BARB0KALEWA\n", FontFactory.GetFont("Arial", 10)));
            //table.AddCell(new Phrase("\n\n\b\b\b\b\b\b\b\b\b           For ENDEAVOUR AUTOMATION\n\n\n\n\n                                       Authorised Signatory", FontFactory.GetFont("Arial", 11, Font.BOLD)));

            doc.Add(table);


            /////////////////////////////////////////////////
            string[] items9 = { "Dear Sir/Madam,\b", "We acknowledge with thanks the receipt of your above enquiry and have great pleasure in submitting our lowest quotation,\b", "subject to the conditions printed below.\n" };

            Font font1211 = FontFactory.GetFont("Arial", 12, Font.BOLD);
            Font font1011 = FontFactory.GetFont("Arial", 10);
            Paragraph paragraph11 = new Paragraph("", font1211);

            for (int i = 0; i < items9.Length; i++)
            {
                paragraph11.Add(new Phrase(" ", font101));
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
            PdfPCell cell = null;
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
            doc.Close();


            Byte[] FileBuffer = File.ReadAllBytes(Server.MapPath("~/Files/") + "QuotationInvoice.pdf");

            doc.Close();
        }
        ifrRight6.Attributes["src"] = @"../Files/" + "QuotationInvoice.pdf";
    }
}