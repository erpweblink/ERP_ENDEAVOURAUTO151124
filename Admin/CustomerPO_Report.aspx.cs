using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections;
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

public partial class Admin_CustomerPO_Report : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);

    protected void Page_Load(object sender, EventArgs e)
    {
        SHOWPOGrid();
    }
    protected void SHOWPOGrid()
    {
        try
        {
            DataTable Dt = new DataTable();
            con.Open();
           // SqlDataAdapter Da = new SqlDataAdapter("SELECT * FROM CustomerPO_Hdr WHERE Is_Deleted='0' ", con);
            SqlDataAdapter Da = new SqlDataAdapter("SELECT * FROM CustomerPO_Hdr_Both WHERE Is_Deleted='0' ORDER BY PoDate DESC ", con);
            Da.Fill(Dt);
            gv_PO_List.DataSource = Dt;
            gv_PO_List.DataBind();
            con.Close();
        }
        catch (Exception)
        {
            throw;
        }
    }
    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetPONOList(string prefixText, int count)
    {
        return AutoFillPONOlist(prefixText);
    }
    public static List<string> AutoFillPONOlist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT Pono from CustomerPO_Hdr_Both where " + "Pono like @Search + '%' AND Is_Deleted='0'";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> Pono = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        Pono.Add(sdr["Pono"].ToString());
                    }
                }
                con.Close();
                return Pono;
            }

        }
    }
    protected void lnkBtnsearch_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtSearchCust.Text) && string.IsNullOrEmpty(txtPONo.Text) && string.IsNullOrEmpty(txtDateSearchfrom.Text))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Search Customer Name');", true);

            SHOWPOGrid();
        }
        else
        {
            if (!string.IsNullOrEmpty(txtSearchCust.Text) && string.IsNullOrEmpty(txtPONo.Text) && string.IsNullOrEmpty(txtDateSearchfrom.Text))
            {
                ViewState["Excell"] = "Getsortedcustomer";
                getSortedcustomer();
        
                //ViewState["Record"] = "Getsortedcustomer";
                //DataTable Dt = new DataTable();
                //SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM CustomerPO_Hdr WHERE CustomerName LIKE '%" + txtSearchCust.Text + "%' AND Is_Deleted='0' ", con);
                //da.Fill(Dt);
                //gv_PO_List.DataSource = Dt;
                //gv_PO_List.DataBind();
            }
            else if (string.IsNullOrEmpty(txtSearchCust.Text) && !string.IsNullOrEmpty(txtPONo.Text) && string.IsNullOrEmpty(txtDateSearchfrom.Text))
            {
                ViewState["Excell"] = "GetsortedPono";
                GetsortedPono();

                //ViewState["Record"] = "GetsortedPono";
                //DataTable Dt = new DataTable();
                //SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM CustomerPO_Hdr WHERE Pono LIKE '%" + txtPONo.Text + "%' AND Is_Deleted='0' ", con);
                //da.Fill(Dt);
                //gv_PO_List.DataSource = Dt;
                //gv_PO_List.DataBind();
            }
            else if (string.IsNullOrEmpty(txtSearchCust.Text) && !string.IsNullOrEmpty(txtPONo.Text) && !string.IsNullOrEmpty(txtDateSearchfrom.Text))
            {
                ViewState["Record"] = "GetsortedPoDatesearch";
                DateTime date = Convert.ToDateTime(txtDateSearchfrom.Text.ToString());

                DataTable Dt = new DataTable();
                txtDateSearchfrom.Text = date.ToString("yyyy-MM-dd");

                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM CustomerPO_Hdr_Both WHERE PoDate= '" + txtDateSearchfrom.Text + "' AND Is_Deleted='0' ", con);
                da.Fill(Dt);
                gv_PO_List.DataSource = Dt;
                gv_PO_List.DataBind();
                gv_PO_List.EmptyDataText = "Not Records Found";
            }
            else if (!string.IsNullOrEmpty(txtSearchCust.Text) && !string.IsNullOrEmpty(txtPONo.Text) && !string.IsNullOrEmpty(txtDateSearchfrom.Text))
            {
                ViewState["Record"] = "GetsortedPonocustDate";
                DateTime date = Convert.ToDateTime(txtDateSearchfrom.Text.ToString());
                txtDateSearchfrom.Text = date.ToString("yyyy-MM-dd");

                DataTable Dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM CustomerPO_Hdr_Both WHERE CustomerName LIKE '%" + txtSearchCust.Text + "%'AND Pono LIKE '%" + txtPONo.Text + "%' AND  PoDate = '" + txtDateSearchfrom.Text + "' AND Is_Deleted='0' ", con);
                da.Fill(Dt);
                gv_PO_List.DataSource = Dt;
                gv_PO_List.DataBind();
            }
            else if (!string.IsNullOrEmpty(txtSearchCust.Text) && !string.IsNullOrEmpty(txtPONo.Text))
            {
                ViewState["Record"] = "GetsortedCustomernamePo";
                DataTable Dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM CustomerPO_Hdr_Both WHERE CustomerName LIKE '%" + txtSearchCust.Text + "%' AND Pono LIKE '%" + txtPONo.Text + "%' AND Is_Deleted='0' ", con);
                da.Fill(Dt);
                gv_PO_List.DataSource = Dt;
                gv_PO_List.DataBind();
            }
            else if (!string.IsNullOrEmpty(txtSearchCust.Text) && !string.IsNullOrEmpty(txtDateSearchfrom.Text) && string.IsNullOrEmpty(txtSearchCust.Text))
            {
                ViewState["Record"] = "GetsortedCustForms";
                DateTime date = Convert.ToDateTime(txtDateSearchfrom.Text.ToString());
                txtDateSearchfrom.Text = date.ToString("yyyy-MM-dd");

                DataTable Dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM CustomerPO_Hdr_Both WHERE CustomerName LIKE '%" + txtSearchCust.Text + "%' AND  PoDate = '" + txtDateSearchfrom.Text + "' AND Is_Deleted='0' ", con);
                da.Fill(Dt);
                gv_PO_List.DataSource = Dt;
                gv_PO_List.DataBind();
            }
            else if (!string.IsNullOrEmpty(txtPONo.Text) && !string.IsNullOrEmpty(txtDateSearchfrom.Text))
            {
                ViewState["Record"] = "GetsortedpodatePonumber";
                DateTime date = Convert.ToDateTime(txtDateSearchfrom.Text.ToString());
                txtDateSearchfrom.Text = date.ToString("yyyy-MM-dd");

                DataTable Dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM CustomerPO_Hdr_Both WHERE Pono LIKE '%" + txtPONo.Text + "%' AND   PoDate = '" + txtDateSearchfrom.Text + "' AND Is_Deleted='0' ", con);
                da.Fill(Dt);
                gv_PO_List.DataSource = Dt;
                gv_PO_List.DataBind();
            }
            else if (!string.IsNullOrEmpty(txtDateSearchfrom.Text) && !string.IsNullOrEmpty(txtDateSearchto.Text) && string.IsNullOrEmpty(txtSearchCust.Text))
            {
                ViewState["Excell"] = "Getsortedpodatewise";
                Getsortedpodatewise();



                //ViewState["Record"] = "Getsortedpodatewise";
                //DateTime date = Convert.ToDateTime(txtDateSearchfrom.Text.ToString());
                //txtDateSearchfrom.Text = date.ToString("yyyy-MM-dd");

                //DateTime datee = Convert.ToDateTime(txtDateSearchto.Text.ToString());
                //txtDateSearchto.Text = datee.ToString("yyyy-MM-dd");

                //DataTable Dt = new DataTable();
                //SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM CustomerPO_Hdr WHERE  PoDate between '" + txtDateSearchfrom.Text + "'  AND '" + txtDateSearchto.Text + "' AND Is_Deleted='0' ", con);
                //da.Fill(Dt);
                //gv_PO_List.DataSource = Dt;
                //gv_PO_List.DataBind();
            }

            else if (!string.IsNullOrEmpty(txtDateSearchfrom.Text) && (string.IsNullOrEmpty(txtSearchCust.Text)))
            {
                ViewState["Record"] = "Getsortedpodateserach";
                DataTable Dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM CustomerPO_Hdr_Both WHERE PoDate = '" + txtDateSearchfrom.Text + "' AND Is_Deleted='0' ", con);
                da.Fill(Dt);
                gv_PO_List.DataSource = Dt;
                gv_PO_List.DataBind();
            }



            else if (!string.IsNullOrEmpty(txtDateSearchfrom.Text) && !string.IsNullOrEmpty(txtDateSearchto.Text) && !string.IsNullOrEmpty(txtSearchCust.Text))
            {

                ViewState["Record"] = "Getsortedcustomerwisepodate";
                Getsortedcustomerwisepodate();

                //DateTime date = Convert.ToDateTime(txtDateSearchfrom.Text.ToString());
                //txtDateSearchfrom.Text = date.ToString("yyyy-MM-dd");

                //DateTime datee = Convert.ToDateTime(txtDateSearchto.Text.ToString());
                //txtDateSearchto.Text = datee.ToString("yyyy-MM-dd");

                //DataTable Dt = new DataTable();
                //SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM CustomerPO_Hdr WHERE PoDate BETWEEN '" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "' AND CustomerName = '" + txtSearchCust.Text + "' AND Is_Deleted = '0'", con);

                //da.Fill(Dt);
                //gv_PO_List.DataSource = Dt;
                //gv_PO_List.DataBind();
            }
        }
    }

    protected void lnkrefresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("CustomerPO_Report.aspx");
    }
    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetCustomerList(string prefixText, int count)
    {
        return AutoFillCustlist(prefixText);
    }
    public static List<string> AutoFillCustlist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT CustomerName from CustomerPO_Hdr_Both where " + "CustomerName like @Search + '%' AND Is_Deleted='0'";

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
    protected void gv_PO_List_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName == "RowView")
        {
            Response.Write("<script>window.open ('../reportPdf/customerPOpdf.aspx?Id=" + encrypt(e.CommandArgument.ToString()) + "','_blank');</script>");

            //string id = e.CommandArgument.ToString();

            //Pdf(id);
        }
        if (e.CommandName == "ShowReport")
        {
            Response.Redirect("Customer_PO.aspx?Pono=" + encrypt(e.CommandArgument.ToString()) + "");


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
    private void Pdf(string id)
    {
        DataTable Dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter("SELECT CustomerPO_Hdr_Both.Id,CustomerName,Description,Pono,PoDate,RefNo,Mobileno,KindAtt,DeliveryAddress,EmailId,GstNo,VehicelNo,PayTerm,Cgst,Sgst,Igst,AllTotalPrice,TotalInWord, RoundOff, GrandTotal, Term_Condition_1, Term_Condition_2, Term_Condition_3, Term_Condition_4, Description, Hsn_Sac,CreatedOn, TaxPercenteage, Quantity, Unit, Rate, DiscountPercentage, Total from CustomerPO_Hdr_Both INNER JOIN CustomerPO_Dtls ON CustomerPO_Hdr_Both.Id = CustomerPO_Dtls.PurchaseId WHERE CustomerPO_Hdr_Both.Id='" + id + "'", con);
        da.Fill(Dt);
        gv_PO_List.DataSource = Dt;
        gv_PO_List.DataBind();

        StringWriter sw = new StringWriter();
        StringReader sr = new StringReader(sw.ToString());

        Document doc = new Document(PageSize.A4, 10f, 10f, 55f, 0f);

        PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(Server.MapPath("~/Files/") + "CustomerPO.pdf", FileMode.Create));
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
        cd.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Purchase Order", 260, 667, 0);
        cd.EndText();

        if (Dt.Rows.Count > 0)
        {
            var CreateDate = DateTime.Now.ToString("yyyy-MM-dd");
            string CustomerName = Dt.Rows[0]["CustomerName"].ToString();
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
            string Total = Dt.Rows[0]["AllTotalPrice"].ToString();

            Paragraph paragraphTable1 = new Paragraph();
            paragraphTable1.SpacingBefore = 120f;
            paragraphTable1.SpacingAfter = 10f;

            PdfPTable table = new PdfPTable(4);

            float[] widths2 = new float[] { 100, 180, 100, 180 };
            table.SetWidths(widths2);
            table.TotalWidth = 560f;
            table.LockedWidth = true;

            var date = DateTime.Now.ToString("yyyy-MM-dd");


            table.AddCell(new Phrase("Vender Name : ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(CustomerName, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("PO Number :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(PoNumber, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("Address", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(Address, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("PO Date :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(PODate, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("Mobile No :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(MobileNo, FontFactory.GetFont("Arial", 9, Font.BOLD)));

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
                    table.AddCell(new Phrase(dr["Hsn_Sac"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(dr["TaxPercenteage"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(dr["Quantity"].ToString(), FontFactory.GetFont("Arial", 9)));
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
            cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "", 30, 258, 0);
            cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibri.ttf", "Identity-H", BaseFont.EMBEDDED), 12);
            cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "", 30, 240, 0);
            cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibri.ttf", "Identity-H", BaseFont.EMBEDDED), 12);
            cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "", 30, 225, 0);
            cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibri.ttf", "Identity-H", BaseFont.EMBEDDED), 12);
            cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "", 30, 210, 0);
            cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibri.ttf", "Identity-H", BaseFont.EMBEDDED), 12);
            cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "", 30, 195, 0);
            cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibri.ttf", "Identity-H", BaseFont.EMBEDDED), 12);
            cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "", 30, 180, 0);

            cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibrii.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
            cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "We Thank you for your enquiry", 440, 256, 0);
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


            Byte[] FileBuffer = File.ReadAllBytes(Server.MapPath("~/Files/") + "CustomerPO.pdf");
            string empFilename = "CustomerPO" + DateTime.Now.ToShortDateString() + ".pdf";

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

    protected void gv_PO_List_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (ViewState["Record"] != null)
        {
            if (ViewState["Record"].ToString() == "Getsortedcustomer")
            {
                gv_PO_List.PageIndex = e.NewPageIndex;
                getSortedcustomer();
            }

            if (ViewState["Record"].ToString() == "GetsortedPono")
            {
                gv_PO_List.PageIndex = e.NewPageIndex;
                GetsortedPono();

            }
            if (ViewState["Record"].ToString() == "GetsortedPoDatesearch")
            {
                gv_PO_List.PageIndex = e.NewPageIndex;
                GetsortedPoDatesearch();

            }
            if (ViewState["Record"].ToString() == "GetsortedPonocustDate")
            {
                gv_PO_List.PageIndex = e.NewPageIndex;
                GetsortedPonocustDate();

            }

            if (ViewState["Record"].ToString() == "GetsortedCustomernamePo")
            {
                gv_PO_List.PageIndex = e.NewPageIndex;
                GetsortedCustomernamePo();

            }

            if (ViewState["Record"].ToString() == "GetsortedCustForms")
            {
                gv_PO_List.PageIndex = e.NewPageIndex;
                GetsortedCustForms();

            }

            if (ViewState["Record"].ToString() == "GetsortedpodatePonumber")
            {
                gv_PO_List.PageIndex = e.NewPageIndex;

                GetsortedpodatePonumber();
            }

            if (ViewState["Record"].ToString() == "Getsortedpodatewise")
            {
                gv_PO_List.PageIndex = e.NewPageIndex;

                Getsortedpodatewise();
            }

            if (ViewState["Record"].ToString() == "Getsortedpodateserach")
            {
                gv_PO_List.PageIndex = e.NewPageIndex;

                Getsortedpodateserach();
            }

            if (ViewState["Record"].ToString() == "Getsortedcustomerwisepodate")
            {
                gv_PO_List.PageIndex = e.NewPageIndex;

                Getsortedcustomerwisepodate();
            }
        }
        else
        {
            gv_PO_List.PageIndex = e.NewPageIndex;

        }
    }

    public void getSortedcustomer()
    {
        ViewState["Excell"] = "Getsortedcustomer";
        ViewState["Record"] = "Getsortedcustomer";
        DataTable Dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM CustomerPO_Hdr_Both WHERE CustomerName LIKE '%" + txtSearchCust.Text + "%' AND Is_Deleted='0' ", con);
        da.Fill(Dt);
        gv_PO_List.DataSource = Dt;
        gv_PO_List.DataBind();
    }

    public void GetsortedPono()
    {
        ViewState["Excell"] = "GetsortedPono";
        ViewState["Record"] = "GetsortedPono";
        DataTable Dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM CustomerPO_Hdr_Both WHERE Pono LIKE '%" + txtPONo.Text + "%' AND Is_Deleted='0' ", con);
        da.Fill(Dt);
        gv_PO_List.DataSource = Dt;
        gv_PO_List.DataBind();
    }

    public void GetsortedPoDatesearch()
    {
        ViewState["Record"] = "GetsortedPoDatesearch";
        DateTime date = Convert.ToDateTime(txtDateSearchfrom.Text.ToString());

        DataTable Dt = new DataTable();
        txtDateSearchfrom.Text = date.ToString("yyyy-MM-dd");

        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM CustomerPO_Hdr_Both WHERE PoDate= '" + txtDateSearchfrom.Text + "' AND Is_Deleted='0' ", con);
        da.Fill(Dt);
        gv_PO_List.DataSource = Dt;
        gv_PO_List.DataBind();
        gv_PO_List.EmptyDataText = "Not Records Found";
    }


    public void GetsortedPonocustDate()
    {
        ViewState["Record"] = "GetsortedPonocustDate";
        DateTime date = Convert.ToDateTime(txtDateSearchfrom.Text.ToString());
        txtDateSearchfrom.Text = date.ToString("yyyy-MM-dd");

        DataTable Dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM CustomerPO_Hdr_Both WHERE CustomerName LIKE '%" + txtSearchCust.Text + "%'AND Pono LIKE '%" + txtPONo.Text + "%' AND  PoDate = '" + txtDateSearchfrom.Text + "' AND Is_Deleted='0' ", con);
        da.Fill(Dt);
        gv_PO_List.DataSource = Dt;
        gv_PO_List.DataBind();
    }

    public void GetsortedCustomernamePo()
    {
        ViewState["Record"] = "GetsortedCustomernamePo";
        DataTable Dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM CustomerPO_Hdr_Both WHERE CustomerName LIKE '%" + txtSearchCust.Text + "%' AND Pono LIKE '%" + txtPONo.Text + "%' AND Is_Deleted='0' ", con);
        da.Fill(Dt);
        gv_PO_List.DataSource = Dt;
        gv_PO_List.DataBind();
    }

    public void GetsortedCustForms()
    {
        ViewState["Record"] = "GetsortedCustForms";
        DateTime date = Convert.ToDateTime(txtDateSearchfrom.Text.ToString());
        txtDateSearchfrom.Text = date.ToString("yyyy-MM-dd");

        DataTable Dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM CustomerPO_Hdr_Both WHERE CustomerName LIKE '%" + txtSearchCust.Text + "%' AND  PoDate = '" + txtDateSearchfrom.Text + "' AND Is_Deleted='0' ", con);
        da.Fill(Dt);
        gv_PO_List.DataSource = Dt;
        gv_PO_List.DataBind();
    }

    public void GetsortedpodatePonumber()
    {
        ViewState["Record"] = "GetsortedpodatePonumber";
        DateTime date = Convert.ToDateTime(txtDateSearchfrom.Text.ToString());
        txtDateSearchfrom.Text = date.ToString("yyyy-MM-dd");

        DataTable Dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM CustomerPO_Hdr_Both WHERE Pono LIKE '%" + txtPONo.Text + "%' AND   PoDate = '" + txtDateSearchfrom.Text + "' AND Is_Deleted='0' ", con);
        da.Fill(Dt);
        gv_PO_List.DataSource = Dt;
        gv_PO_List.DataBind();
    }

    public void Getsortedpodatewise()
    {

        ViewState["Excell"] = "Getsortedpodatewise";
        ViewState["Record"] = "Getsortedpodatewise";
        DateTime date = Convert.ToDateTime(txtDateSearchfrom.Text.ToString());
        txtDateSearchfrom.Text = date.ToString("yyyy-MM-dd");

        DateTime datee = Convert.ToDateTime(txtDateSearchto.Text.ToString());
        txtDateSearchto.Text = datee.ToString("yyyy-MM-dd");

        DataTable Dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM CustomerPO_Hdr_Both WHERE  PoDate between '" + txtDateSearchfrom.Text + "'  AND '" + txtDateSearchto.Text + "' AND Is_Deleted='0' ", con);
        da.Fill(Dt);
        gv_PO_List.DataSource = Dt;
        gv_PO_List.DataBind();
    }

    public void Getsortedpodateserach()
    {
        ViewState["Record"] = "Getsortedpodateserach";
        DataTable Dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM CustomerPO_Hdr_Both WHERE PoDate = '" + txtDateSearchfrom.Text + "' AND Is_Deleted='0' ", con);
        da.Fill(Dt);
        gv_PO_List.DataSource = Dt;
        gv_PO_List.DataBind();
    }


    public void Getsortedcustomerwisepodate()
    {
        ViewState["Excell"] = "Getsortedcustomerwisepodate";
        ViewState["Record"] = "Getsortedcustomerwisepodate";
        DateTime date = Convert.ToDateTime(txtDateSearchfrom.Text.ToString());
        txtDateSearchfrom.Text = date.ToString("yyyy-MM-dd");

        DateTime datee = Convert.ToDateTime(txtDateSearchto.Text.ToString());
        txtDateSearchto.Text = datee.ToString("yyyy-MM-dd");

        DataTable Dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM CustomerPO_Hdr_Both WHERE PoDate BETWEEN '" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "' AND CustomerName = '" + txtSearchCust.Text + "' AND Is_Deleted = '0'", con);

        da.Fill(Dt);
        gv_PO_List.DataSource = Dt;
        gv_PO_List.DataBind();
    }

    public void GetsortedPonoExcel()
    {
        ViewState["Excell"] = "GetsortedPono";
        ViewState["Record"] = "GetsortedPono";
        DataTable Dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM CustomerPO_Hdr_Both WHERE Pono LIKE '%" + txtPONo.Text + "%' AND Is_Deleted='0' ", con);
        da.Fill(Dt);
        sortedgv.DataSource = Dt;
        sortedgv.DataBind();
    }

    public void getSortedcustomerExecl()
    {
        ViewState["Excell"] = "Getsortedcustomer";
        ViewState["Record"] = "Getsortedcustomer";
        DataTable Dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM CustomerPO_Hdr_Both WHERE CustomerName LIKE '%" + txtSearchCust.Text + "%' AND Is_Deleted='0' ", con);
        da.Fill(Dt);
        sortedgv.DataSource = Dt;
        sortedgv.DataBind();
    }



    public void GetsortedpodatewiseExcel()
    {

        ViewState["Excell"] = "Getsortedpodatewise";
        ViewState["Record"] = "Getsortedpodatewise";
        DateTime date = Convert.ToDateTime(txtDateSearchfrom.Text.ToString());
        txtDateSearchfrom.Text = date.ToString("yyyy-MM-dd");

        DateTime datee = Convert.ToDateTime(txtDateSearchto.Text.ToString());
        txtDateSearchto.Text = datee.ToString("yyyy-MM-dd");

        DataTable Dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM CustomerPO_Hdr_Both WHERE  PoDate between '" + txtDateSearchfrom.Text + "'  AND '" + txtDateSearchto.Text + "' AND Is_Deleted='0' ", con);
        da.Fill(Dt);
        sortedgv.DataSource = Dt;
        sortedgv.DataBind();
    }

    public void GetsortedcustomerwisepodateExcel()
    {
        ViewState["Excell"] = "Getsortedcustomerwisepodate";
        ViewState["Record"] = "Getsortedcustomerwisepodate";
        DateTime date = Convert.ToDateTime(txtDateSearchfrom.Text.ToString());
        txtDateSearchfrom.Text = date.ToString("yyyy-MM-dd");

        DateTime datee = Convert.ToDateTime(txtDateSearchto.Text.ToString());
        txtDateSearchto.Text = datee.ToString("yyyy-MM-dd");

        DataTable Dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM CustomerPO_Hdr_Both WHERE PoDate BETWEEN '" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "' AND CustomerName = '" + txtSearchCust.Text + "' AND Is_Deleted = '0'", con);

        da.Fill(Dt);
        sortedgv.DataSource = Dt;
        sortedgv.DataBind();
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

        if (ViewState["Excell"] != null)
        {
            gv_PO_List.Visible = false;
            string Method = ViewState["Excell"].ToString();

            if (Method == "GetsortedPono")
            {
                GetsortedPonoExcel();
            }
            if (Method == "Getsortedcustomer")
            {
                getSortedcustomerExecl();
            }
            if (Method == "Getsortedpodatewise")
            {
                GetsortedpodatewiseExcel();
            }
            if (Method == "Getsortedcustomerwisepodate")
            {
                GetsortedcustomerwisepodateExcel();
            }


            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";
            string FileName = "Cust_PO_Report" + DateTime.Now + ".xls";
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