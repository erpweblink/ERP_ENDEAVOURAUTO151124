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

public partial class vendorPOpdf : System.Web.UI.Page
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
        SqlDataAdapter da = new SqlDataAdapter("SELECT tblPurchaseOrderHdr.Id,statecode,VendorName,Description,Pono,PoDate,RefNo,Mobileno,KindAtt,DeliveryAddress,EmailId,GstNo,VehicelNo,PayTerm,Cgst,Sgst,Igst,AllTotalPrice,TotalInWord, RoundOff, GrandTotal, Term_Condition_1, Term_Condition_2, Term_Condition_3, Term_Condition_4, Description, Hsn_Sac,CreatedOn, TaxPercenteage, Quantity, Unit, Rate, DiscountPercentage, Total,RefNo from tblPurchaseOrderHdr INNER JOIN tblPurchaseOrderDtls ON tblPurchaseOrderHdr.Id = tblPurchaseOrderDtls.PurchaseId WHERE tblPurchaseOrderHdr.Id='" + id + "'", Conn);
        da.Fill(Dt);
        //GvPurchaseOrderList.DataSource = Dt;
        //GvPurchaseOrderList.DataBind();

        StringWriter sw = new StringWriter();
        StringReader sr = new StringReader(sw.ToString());

        Document doc = new Document(PageSize.A4, 10f, 10f, 55f, 0f);

        PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(Server.MapPath("~/Files/") + "PurchaseOrder.pdf", FileMode.Create));
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
        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Tathawade, Pune-411033", 235, 740, 0);
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
        cd.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Purchase Order", 240, 667, 0);
        cd.EndText();

        if (Dt.Rows.Count > 0)
        {
            var CreateDate = DateTime.Now.ToString("dd-MM-yyyy");
            string statecode = Dt.Rows[0]["statecode"].ToString();
            string VendorName = Dt.Rows[0]["VendorName"].ToString();
            string PoNumber = Dt.Rows[0]["Pono"].ToString();
            string Address = Dt.Rows[0]["DeliveryAddress"].ToString();
            string PODate = Dt.Rows[0]["PoDate"].ToString().TrimEnd("0:0".ToCharArray());
            string MobileNo = Dt.Rows[0]["Mobileno"].ToString();
            string GSTNo = Dt.Rows[0]["GstNo"].ToString();
            string KindAtt = Dt.Rows[0]["KindAtt"].ToString();
            string TotalInWord = Dt.Rows[0]["TotalInWord"].ToString();
            string GrandTotal = Dt.Rows[0]["GrandTotal"].ToString();
            string CGST = Dt.Rows[0]["Cgst"].ToString();
            string SGST = Dt.Rows[0]["Sgst"].ToString();
            string IGST = Dt.Rows[0]["Igst"].ToString();
            string RoundOff = Dt.Rows[0]["RoundOff"].ToString();
            string Total = Dt.Rows[0]["AllTotalPrice"].ToString();
            string RefNo = Dt.Rows[0]["RefNo"].ToString();
            string PayTerms = Dt.Rows[0]["PayTerm"].ToString();
            string Tax = Dt.Rows[0]["TaxPercenteage"].ToString();
            string str = Dt.Rows[0]["Term_Condition_1"].ToString();
            string str1 = Dt.Rows[0]["Term_Condition_2"].ToString();
            string str2 = Dt.Rows[0]["Term_Condition_3"].ToString();
            string str3 = Dt.Rows[0]["Term_Condition_4"].ToString();
            string[] arrstr = str.ToString().Split('-');
            string[] arrstr1 = str1.ToString().Split('-');
            string[] arrstr2 = str2.ToString().Split('-');
            string[] arrstr3 = str3.ToString().Split('-');

            Paragraph paragraphTable1 = new Paragraph();
            paragraphTable1.SpacingBefore = 120f;
            paragraphTable1.SpacingAfter = 10f;

            PdfPTable table = new PdfPTable(4);

            float[] widths2 = new float[] { 100, 180, 100, 180 };
            table.SetWidths(widths2);
            table.TotalWidth = 560f;
            table.LockedWidth = true;

            DateTime ffff1 = Convert.ToDateTime(Dt.Rows[0]["PODate"].ToString());
            string datee = ffff1.ToString("dd-MM-yyyy");

            table.AddCell(new Phrase("      Vendor Name : ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("       " +VendorName, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("      PO No. :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("       " + PoNumber, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("      Address", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("       " + Address, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("      PO Date :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("       " + datee, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("      Mobile No. :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("       " + MobileNo, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("      GST No. :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("       " + GSTNo, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("      Ref. No.:", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("       " + RefNo, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("      Ref. Date:", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("       " + CreateDate, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("      Pay Terms:", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("       " + PayTerms, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("      Kind Att:", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("       " + KindAtt, FontFactory.GetFont("Arial", 9, Font.BOLD)));
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
                table.AddCell(new Phrase(" SN.", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("          Description", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("  Hsn / Sac", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("   Tax %", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase(" Quantity", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("    Unit", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("        Rate", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("   Disc %", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("        Total", FontFactory.GetFont("Arial", 10, Font.BOLD)));

                int rowid = 1;
                foreach (DataRow dr in Dt.Rows)
                {
                    table.TotalWidth = 560f;
                    table.LockedWidth = true;

                    double Ftotal = Convert.ToDouble(dr["Total"].ToString());
                    string _ftotal = Ftotal.ToString("##.00");
                    table.AddCell(new Phrase(rowid.ToString(), FontFactory.GetFont("Arial", 9)));   
                    table.AddCell(new Phrase("     " + dr["Description"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase("     " + dr["Hsn_Sac"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase("     " + dr["TaxPercenteage"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase("     " + dr["Quantity"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase("     " + dr["Unit"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase("     " + dr["Rate"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase("     " + dr["DiscountPercentage"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(_ftotal, FontFactory.GetFont("Arial", 9)));
                    rowid++;

                    Ttotal_price += Convert.ToDouble("     " + dr["Total"].ToString());
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
     
            table.AddCell(new Phrase("  \n\n\n\n\n\n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));

            //  }
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

            table.SetWidths(new float[] { 0f, 76f, 12f });
            table.AddCell(paragraph);
            PdfPCell cell2t = new PdfPCell(new Phrase("Subtotal", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            cell2t.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cell2t);
            PdfPCell cell3t = new PdfPCell(new Phrase(Ttotal_price.ToString("#.00"), FontFactory.GetFont("Arial", 10, Font.BOLD)));
            cell3t.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cell3t);
            doc.Add(table);
            //add total row end


            if (statecode != "27 MAHARASHTRA")
            {
                //IGST 9% Row STart
                Paragraph paragraphTable161 = new Paragraph();
                paragraphTable5.SpacingAfter = 10f;


                string[] item1 = { "Goods once sold will not be taken back or exchange. \b",
                        "Interest at the rate of 18% will be charged if bill is'nt paid within 30 days.\b",
                        "Our risk and responsibility ceases the moment goods leaves out godown. \n",
                        };

                Font font141 = FontFactory.GetFont("Arial", 12, Font.BOLD);
                Font font151 = FontFactory.GetFont("Arial", 10, Font.BOLD);
                Paragraph paragraphhhh1 = new Paragraph("", font12);

                for (int i = 0; i < items.Length; i++)
                {
                    paragraph.Add(new Phrase("", font10));
                }

                table = new PdfPTable(3);
                table.TotalWidth = 560f;
                table.LockedWidth = true;
                var IGSTPer = Convert.ToDecimal(Tax);
                table.SetWidths(new float[] { 0f, 76f, 12f });
                table.AddCell(paragraph);
                PdfPCell cell221 = new PdfPCell(new Phrase("IGST- 18%", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                cell221.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell221);
                PdfPCell cell313 = new PdfPCell(new Phrase(IGST, FontFactory.GetFont("Arial", 10, Font.BOLD)));
                cell313.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell313);
                doc.Add(table);
                //IGST 9% Row End
            }
            else
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

                var CGSTPer = Convert.ToDecimal(Tax) / 2;
                table.SetWidths(new float[] { 0f, 76f, 12f });
                table.AddCell(paragraph);
                // PdfPCell cell2 = new PdfPCell(new Phrase("CGST-" + CGSTPer + " %", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                PdfPCell cell2 = new PdfPCell(new Phrase("CGST- 9% ", FontFactory.GetFont("Arial", 10, Font.BOLD)));
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
                var SGSTPer = Convert.ToDecimal(Tax) / 2;
                table.SetWidths(new float[] { 0f, 76f, 12f });
                table.AddCell(paragraph);
                PdfPCell cell22 = new PdfPCell(new Phrase("SGST- 9%", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                cell22.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell22);
                PdfPCell cell33 = new PdfPCell(new Phrase(SGST, FontFactory.GetFont("Arial", 10, Font.BOLD)));
                cell33.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell33);
                doc.Add(table);
                //SGST 9% Row End
            }






           
            //Round Off
            //Paragraph paragraphTable1611 = new Paragraph();
            //paragraphTable5.SpacingAfter = 10f;
            //string[] item11 = { "Goods once sold will not be taken back or exchange. \b",
            //            "Interest at the rate of 18% will be charged if bill is'nt paid within 30 days.\b",
            //            "Our risk and responsibility ceases the moment goods leaves out godown. \n",
            //            };

            //Font font1411 = FontFactory.GetFont("Arial", 12, Font.BOLD);
            //Font font1511 = FontFactory.GetFont("Arial", 10, Font.BOLD);
            //Paragraph paragraphhhh11 = new Paragraph("", font12);
            //for (int i = 0; i < items.Length; i++)
            //{
            //    paragraph.Add(new Phrase("", font10));
            //}

            //table = new PdfPTable(3);
            //table.TotalWidth = 560f;
            //table.LockedWidth = true;
            //table.SetWidths(new float[] { 0f, 76f, 12f });
            //table.AddCell(paragraph);
            //PdfPCell cell2211 = new PdfPCell(new Phrase("Round Off", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            //cell2211.HorizontalAlignment = Element.ALIGN_RIGHT;
            //table.AddCell(cell2211);
            //PdfPCell cell3131 = new PdfPCell(new Phrase(RoundOff, FontFactory.GetFont("Arial", 10, Font.BOLD)));
            //cell3131.HorizontalAlignment = Element.ALIGN_RIGHT;
            //table.AddCell(cell3131);
            //doc.Add(table);
            //Round Off End

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
            //grand Total Row end

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
            ////grand total in word end



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

            doc.Add(table);
            ////////term condition in Database end
            /////////////////////////////////////////////////
            string[] items91 = { "Dear Sir/Madam,\b", "We acknowledge with thanks the receipt of your above enquiry and have great pleasure in submitting our lowest quotation,\b", "subject to the conditions printed below.\n" };

            Font font12111 = FontFactory.GetFont("Arial", 12, Font.BOLD);
            Font font101111 = FontFactory.GetFont("Arial", 10);
            Paragraph paragraph111 = new Paragraph("", font12111);

            for (int i = 0; i < items91.Length; i++)
            {
                paragraph111.Add(new Phrase(" ", font101111));
            }

            table = new PdfPTable(2);
            table.TotalWidth = 560f;
            table.LockedWidth = true;
            table.SetWidths(new float[] { 310f, 250f });

            //table.AddCell(paragraph11);
            table.AddCell(new Phrase("\bSpecial Instruction:\n\n1) Please Mention our P.O.No on your D.C. & Invoice.\n\n2) Send Delivery challan & Orignal Invoice (with Transporter's copy) \n\b along with the material.", FontFactory.GetFont("Arial", 10)));
            table.AddCell(new Phrase("\bRejection:\n\bIf material received is found faulty or not matching With \n required specifications,we will intimate you within 4 \n weeks From receiving the material.Such material will \n be rejected and Will be sent back at the cost and risk \n of supplier.Corresponding Deductions will apply to \n Supplier's A/C.", FontFactory.GetFont("Arial", 10)));

            doc.Add(table);
            /////////////////////////////

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

            Byte[] FileBuffer = File.ReadAllBytes(Server.MapPath("~/Files/") + "PurchaseOrder.pdf");
            //string empFilename = "PurchaseOrder" + DateTime.Now.ToShortDateString() + ".pdf";

            //if (FileBuffer != null)
            //{
            //    Response.ContentType = "application/pdf";
            //    Response.AddHeader("content-length", FileBuffer.Length.ToString());
            //    Response.BinaryWrite(FileBuffer);
            //    Response.AddHeader("Content-Disposition", "attachment;filename=" + empFilename);
            //}

        }
        doc.Close();
        ifrRight6.Attributes["src"] = @"../Files/" + "PurchaseOrder.pdf";

    }
}