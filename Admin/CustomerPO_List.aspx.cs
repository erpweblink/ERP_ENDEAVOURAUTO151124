using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_CustomerPO_List : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
    string id;
    string Id = "";
    DataTable dt11 = new DataTable();
    string UserCompany = "";

    protected void Page_Load(object sender, EventArgs e)
    {
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

        /////////////////////////////////////
       
        if (Session["adminname"] == null)
        {
            Response.Redirect("../LoginPage.aspx");
        }
        else
        {
            ViewData();
            // GvCustomerpoList.DataSource = dt11;
            GvCustomerpoList.DataBind();

            //UserCompany = Session["adminname"].ToString();
            //if (UserCompany != "Admin")
            //{
            //    gvbind_Company();
            //    btncreate.Visible = false;
            //}
            //else
            //{
            //    ViewData();
            //    GvCustomerpoList.DataSource = dt11;
            //    GvCustomerpoList.DataBind();
            //}
            //this.txtDateSearch.Text = DateTime.Now.ToString("yyyy-MM-dd");
            //this.txtDateSearch.TextMode = TextBoxMode.Date;
        }
    }
    void gvbind_Company()
    {
        try
        {
            string UserCompany = Session["name"].ToString();
            DataTable dt = new DataTable();
            SqlDataAdapter sad = new SqlDataAdapter("SELECT Id,CustomerName,SubCustomer,JobNo,Pono,PoDate,RefNo,Mobileno,GrandTotal,CreatedBy,CreatedOn,DATEDIFF(DAY, PoDate, getdate()) AS days FROM CustomerPO_Hdr_Both WHERE Is_Deleted='0' AND CustomerName='" + UserCompany + "'", con);
            sad.Fill(dt);
            GvCustomerpoList.EmptyDataText = "Not Records Found";
            GvCustomerpoList.DataSource = dt;
            GvCustomerpoList.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void ViewData()
    {
        //DataTable Dt = new DataTable();
        //SqlDataAdapter da = new SqlDataAdapter("SELECT Id,CustomerName,SubCustomer,JobNo,Pono,PoDate,RefNo,Mobileno,CreatedBy,CreatedOn,DATEDIFF(DAY, PoDate, getdate()) AS days FROM CustomerPO_Hdr_Both WHERE Is_Deleted='0' ORDER BY CreatedOn DESC", con);
        //da.Fill(Dt);
        //GvCustomerpoList.EmptyDataText = "Records Not Found";
        //GvCustomerpoList.DataSource = Dt;
        //GvCustomerpoList.DataBind();


        DataTable Dt = new DataTable();

        //SqlDataAdapter da = new SqlDataAdapter("SELECT Id,jobNo,Quotationno,CustomerName,SubCustomer,Pono,PoDate,RefNo,Mobileno," +
        //    "GrandTotal,CreatedBy,CreatedOn,DATEDIFF(DAY, PoDate, getdate()) AS days, Type FROM CustomerPO_Hdr_Both WHERE Type = 'JobNo' " +
        //    "AND Is_Deleted='0' ORDER BY CreatedOn DESC", con);

        SqlDataAdapter da = new SqlDataAdapter(
                     "SELECT C.Id, C.Quotationno, C.CustomerName," +
                     "C.SubCustomer, C.SubCustomer, C.Pono, C.PoDate, C.RefNo, *," +
                     "CASE " +
                         "WHEN NOT EXISTS (SELECT 1 FROM CustomerPO_Dtls_Both D WHERE D.PurchaseId = C.Id AND D.JobStatus != 'Completed') " +
                         "THEN (SELECT MAX(D.JobDaysCount) FROM CustomerPO_Dtls_Both D WHERE D.PurchaseId = C.Id) " +
                         "ELSE DATEDIFF(DAY, C.CreatedOn, GETDATE()) " +
                     "END AS CountDays " +
                     "FROM CustomerPO_Hdr_Both C " +
                     "WHERE C.Type = 'JobNo' " +
                     "ORDER BY C.CreatedOn DESC;", con);

        //SqlDataAdapter da = new SqlDataAdapter("SELECT Id,jobNo,Quotationno,CustomerName,SubCustomer,Pono,PoDate,RefNo,Mobileno,GrandTotal,Type,CreatedBy,CreatedOn,DATEDIFF(DAY, PoDate, getdate()) AS days FROM CustomerPO_Hdr_Both WHERE Is_Deleted='0' ORDER BY CreatedOn DESC", con);

        da.Fill(Dt);
        GvCustomerpoList.EmptyDataText = "Records Not Found";
        GvCustomerpoList.DataSource = Dt;
        GvCustomerpoList.DataBind();
    }

    private void GridExport()
    {
        DataTable Dt = new DataTable();

        SqlDataAdapter da = new SqlDataAdapter("SELECT C.Id, C.Quotationno, C.CustomerName," +
                     "C.SubCustomer, C.SubCustomer, C.Pono, C.PoDate, C.RefNo, *," +
                     "CASE " +
                         "WHEN NOT EXISTS (SELECT 1 FROM CustomerPO_Dtls_Both D WHERE D.PurchaseId = C.Id AND D.JobStatus != 'Completed') " +
                         "THEN (SELECT MAX(D.JobDaysCount) FROM CustomerPO_Dtls_Both D WHERE D.PurchaseId = C.Id) " +
                         "ELSE DATEDIFF(DAY, C.CreatedOn, GETDATE()) " +
                     "END AS CountDays " +
                     "FROM CustomerPO_Hdr_Both C " +
                     "WHERE C.Type = 'JobNo' " +
                     "ORDER BY C.CreatedOn DESC;", con);

        da.Fill(Dt);
        GridExportExcel.EmptyDataText = "Records Not Found";
        GridExportExcel.DataSource = Dt;
        GridExportExcel.DataBind();
    }


    protected void btncreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("Customer_PO.aspx");
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

                com.CommandText = "select DISTINCT CustomerName from CustomerPO_Hdr_Both where " + "CustomerName like @Search + '%' AND Type='JobNo' ANd Is_Deleted='0'";


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
    public static List<string> GetJobNOList(string prefixText, int count)
    {
        return AutoFilljobnorlist(prefixText);
    }

    public static List<string> AutoFilljobnorlist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {

                com.CommandText = "select DISTINCT JobNo from CustomerPO_Hdr_Both where " + "JobNo like @Search + '%' AND Type='JobNo' AND Is_Deleted='0'";


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
    public static List<string> GetPOList(string prefixText, int count)
    {
        return AutoFillPOlist(prefixText);
    }

    public static List<string> AutoFillPOlist(string prefixText)
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

    SqlDataAdapter sadquatation11;
    protected void btn_search_Click(object sender, EventArgs e)
    {



        DataTable dt = new DataTable();
        if (string.IsNullOrEmpty(txtJobno.Text) && string.IsNullOrEmpty(txt_pono_search.Text) && string.IsNullOrEmpty(txt_form_podate_search.Text) && (string.IsNullOrEmpty(txtjob.Text)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Please Search Customer Name !!!');", true);
            ViewData();
        }

        //From Date To ToDate
        else if (!string.IsNullOrEmpty(txt_form_podate_search.Text) && !string.IsNullOrEmpty(txt_to_podate_search.Text) && string.IsNullOrEmpty(txtJobno.Text))
        {
            ViewState["Excell"] = "GetSortedDatewisedata";
            GetSortedDatewisedata();
            //DataTable dtt = new DataTable();
            //SqlDataAdapter sad = new SqlDataAdapter("select  Id,CustomerName,SubCustomer,JobNo,Pono,PoDate,RefNo,Mobileno,Quotationno,CreatedBy,CreatedOn,DATEDIFF(DAY, PoDate, getdate()) AS days from CustomerPO_Hdr_Both where  PoDate between '" + txt_form_podate_search.Text + "'  AND '" + txt_to_podate_search.Text + "' AND Is_Deleted='0'", con);
            //sad.Fill(dtt);
            //GvCustomerpoList.EmptyDataText = "Records Not Found";
            //GvCustomerpoList.DataSource = dtt;
            //GvCustomerpoList.DataBind();
        }

        //Quotation no search
        else if (!string.IsNullOrEmpty(txt_pono_search.Text) && string.IsNullOrEmpty(txt_form_podate_search.Text))
        {
            ViewState["Excell"] = "Getsortedquation";
            Getsortedquation();
            //DataTable dtt = new DataTable();
            //SqlDataAdapter sad = new SqlDataAdapter("select  Id,CustomerName,SubCustomer,JobNo,Pono,PoDate,RefNo,Mobileno,Quotationno,CreatedBy,CreatedOn,DATEDIFF(DAY, PoDate, getdate()) AS days from CustomerPO_Hdr_Both where  Pono = '" + txt_pono_search.Text + "' AND Is_Deleted='0'", con);
            //sad.Fill(dtt);
            //GvCustomerpoList.EmptyDataText = "Records Not Found";
            //GvCustomerpoList.DataSource = dtt;
            //GvCustomerpoList.DataBind();
        }

        //Company name search
        else if (!string.IsNullOrEmpty(txtJobno.Text) && string.IsNullOrEmpty(txt_form_podate_search.Text) && string.IsNullOrEmpty(txtjob.Text))
        {
            ViewState["Excell"] = "Sortedcompony";

            Sortedcompony();
            //DataTable dtt = new DataTable();
            //SqlDataAdapter sad = new SqlDataAdapter("select  Id,CustomerName,SubCustomer,JobNo,Pono,PoDate,RefNo,Mobileno,Quotationno,CreatedBy,CreatedOn,DATEDIFF(DAY, PoDate, getdate()) AS days from CustomerPO_Hdr_Both where CustomerName = '" + txtJobno.Text + "' AND  Is_Deleted='0'", con);
            //sad.Fill(dtt);
            //GvCustomerpoList.EmptyDataText = "Records Not Found";
            //GvCustomerpoList.DataSource = dtt;
            //GvCustomerpoList.DataBind();
        }

        else if (!string.IsNullOrEmpty(txtJobno.Text) && !string.IsNullOrEmpty(txt_form_podate_search.Text) && string.IsNullOrEmpty(txt_form_podate_search.Text))
        {
            ViewState["Excell"] = "Getdatewisecustomer";
            Getdatewisecustomer();

        }

        else if (!string.IsNullOrEmpty(txt_pono_search.Text) && !string.IsNullOrEmpty(txt_form_podate_search.Text))
        {
            ViewState["Excell"] = "GetDatwisePo";
            GetDatwisePo();

        }
        //Date wise search
        //else if (!string.IsNullOrEmpty(txtDateSearch.Text))
        //{
        //    DataTable dtt = new DataTable();
        //    SqlDataAdapter sad = new SqlDataAdapter("select ID,Quotation_no,Quotation_Date,ExpiryDate,JobNo,Customer_Name,SubCustomer,Address,Mobile_No,Phone_No,GST_No,State_Code,kind_Att,CGST,SGST,AllTotal_price,Total_in_word,IsDeleted,CreatedBy,CreatedOn, DATEDIFF(DAY, Quotation_Date, getdate()) AS days from tbl_Quotation_Hdr where Quotation_Date='" + txtDateSearch.Text + "'", con);
        //    sad.Fill(dtt);
        //    gv_Quot_List.EmptyDataText = "Records Not Found";
        //    gv_Quot_List.DataSource = dtt;
        //    gv_Quot_List.DataBind();
        //}


        else if (!string.IsNullOrEmpty(txtjob.Text) && (string.IsNullOrEmpty(txtJobno.Text)))
        {
            ViewState["Excell"] = "GetJobNowise";
            GetJobNowise();
        }


        else if (!string.IsNullOrEmpty(txtjob.Text) && (!string.IsNullOrEmpty(txtJobno.Text)))
        {
            ViewState["Excell"] = "GetCustomerowisejob";
            GetCustomerowisejob();
        }


    }

    protected void lnkBtn_rfresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("CustomerPO_List.aspx");
    }

    protected void GvCustomerpoList_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName == "RowDelete")
        {
            con.Open();
            SqlCommand Cmd = new SqlCommand("UPDATE CustomerPO_Hdr_Both SET IsDeleted='1' WHERE Id=@Id", con);
            Cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(e.CommandArgument.ToString()));
            Cmd.Parameters.AddWithValue("@IsDeleted", '1');

            Cmd.ExecuteNonQuery();

            ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Record Deleted Sucessfully');", true);

        }
        if (e.CommandName == "RowPrint")
        {
            Response.Write("<script>window.open ('../reportPdf/customerPOpdf.aspx?Id=" + encrypt(e.CommandArgument.ToString()) + "','_blank');</script>");

            //string id = e.CommandArgument.ToString();

            //Pdf(id);
        }
        if (e.CommandName == "Rowedit")
        {
            Response.Redirect("Customer_PO.aspx?Id=" + encrypt(e.CommandArgument.ToString()) + "");
        }
        if (e.CommandName == "SendINV")
        {
            int id = Convert.ToInt32(e.CommandArgument);
            string QuatationNo = GvCustomerpoList.DataKeys[id]["Quotationno"].ToString();
            Response.Redirect("TaxInvoice.aspx?QuatationNo=" + Server.UrlEncode(QuatationNo));

            //string encryptedQuatationNo = Encrypt(QuatationNo);
            //Response.Redirect($"TaxInvoice.aspx?QuatationNo={Server.UrlEncode(encryptedQuatationNo)}");

        }

    }

    //private string Encrypt(string text)
    //{
    //    byte[] data = System.Text.Encoding.UTF8.GetBytes(text);
    //    byte[] encryptedData = System.Web.Security.MachineKey.Protect(data);
    //    return Convert.ToBase64String(encryptedData);
    //}

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
        GvCustomerpoList.DataSource = Dt;
        GvCustomerpoList.DataBind();

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
            string SubCustomer = Dt.Rows[0]["SubCustomer"].ToString();
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

            DateTime ffff1 = Convert.ToDateTime(Dt.Rows[0]["PODate"].ToString());
            string datee = ffff1.ToString("yyyy-MM-dd");


            table.AddCell(new Phrase("Vender Name : ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(CustomerName, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("PO Number :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(PoNumber, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("Address", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(Address, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("PO Date :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(datee, FontFactory.GetFont("Arial", 9, Font.BOLD)));

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

    protected void GvCustomerpoList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvCustomerpoList.PageIndex = e.NewPageIndex;
        ViewData();
    }

    string status;
    SqlDataAdapter sadquatation;
    protected void GvCustomerpoList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //Added New for Count by Shubham Patil
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            decimal totalAmount = 0;

            if (GvSorted.Rows.Count > 0)
            {
                foreach (GridViewRow row in GvSorted.Rows)
                {

                    Label lblgrandtotal = row.FindControl("lblgrandtotal") as Label;
                    if (lblgrandtotal != null)
                    {
                        if (decimal.TryParse(lblgrandtotal.Text, out decimal rowAmount)) 
                        {
                            totalAmount += rowAmount;
                        }
                    }
                }
            }
            else
            {
                foreach (GridViewRow row in GvCustomerpoList.Rows)
                {
                    Label lblgrandtotal = row.FindControl("lblgrandtotal") as Label;
                    if (lblgrandtotal != null)
                    {
                        if (decimal.TryParse(lblgrandtotal.Text, out decimal rowAmount))
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

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string Id = GvCustomerpoList.DataKeys[e.Row.RowIndex].Value.ToString();
            GridView gvDetailss = e.Row.FindControl("gvDetailss") as GridView;



            string QuatationNo = GvCustomerpoList.DataKeys[e.Row.RowIndex]["Quotationno"].ToString();

            if (txtjob.Text == "")
            {
                string query1 = string.Empty;

                query1 = @"SELECT [JobNo],[JobStatus],[JobDaysCount],[PurchaseId] FROM [CustomerPO_Dtls_Both] WHERE Quotationno ='" + Id + "'";

                SqlDataAdapter ad = new SqlDataAdapter(query1, con);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    if (row["JobDaysCount"] == DBNull.Value || Convert.ToInt32(row["JobDaysCount"]) == 0 && row["JobStatus"].ToString() == "Pending")
                    {
                        con.Open();
                        SqlCommand cmdss = new SqlCommand("SELECT CreatedOn FROM CustomerPO_Hdr_Both WHERE Id = '" + row["PurchaseId"] + "'", con);
                        object result = cmdss.ExecuteScalar();

                        if (result != DBNull.Value)
                        {
                            DateTime createdOn = Convert.ToDateTime(result);
                            DateTime createdOnDateOnly = createdOn.Date;
                            int jobDaysCount = (DateTime.Now.Date - createdOnDateOnly).Days;
                            row["JobDaysCount"] = jobDaysCount;
                        }
                        con.Close();
                    }
                }
                gvDetailss.DataSource = dt;
                gvDetailss.DataBind();
            }
            else
            {
                string query1 = string.Empty;

                query1 = @"SELECT Top 1 [JobNo], * FROM [CustomerPO_Dtls_Both] WHERE JobNo ='" + txtjob.Text + "'";

                SqlDataAdapter ad = new SqlDataAdapter(query1, con);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                gvDetailss.DataSource = dt;
                gvDetailss.DataBind();
            }


        }
    }

    //sorted Grid section start

    protected void GvSorted_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

        if (ViewState["Record"].ToString() == "Date")
        {
            GvSorted.PageIndex = e.NewPageIndex;
            GetSortedDatewisedataGrid();
        }
        if (ViewState["Record"].ToString() == "Quatation")
        {
            GvSorted.PageIndex = e.NewPageIndex;
            GetsortedquationGrid();
        }

        if (ViewState["Record"].ToString() == "company")
        {
            GvSorted.PageIndex = e.NewPageIndex;
            Sortedcomponygrid();
        }
        if (ViewState["Record"].ToString() == "Datewisecustyomer")
        {
            GvSorted.PageIndex = e.NewPageIndex;
            Getdatewisecustomergrid();
        }

        if (ViewState["Record"].ToString() == "DatewisePO")
        {
            GvSorted.PageIndex = e.NewPageIndex;

        }
        if (ViewState["Record"].ToString() == "Jobwise")
        {
            GvSorted.PageIndex = e.NewPageIndex;
            GetJobNowise();
        }

        if (ViewState["Record"].ToString() == "CustomerwiseJob")
        {
            GvSorted.PageIndex = e.NewPageIndex;
            GetCustomerowisejob();
        }



    }


    public void GetSortedDatewisedata()
    {
        GvCustomerpoList.Visible = false;
        ViewState["Record"] = "Date";
        DataTable dtt = new DataTable();
       // SqlDataAdapter sad = new SqlDataAdapter("select  Id,CustomerName,SubCustomer,JobNo,Pono,PoDate,RefNo,Mobileno,Quotationno,CreatedBy,CreatedOn,DATEDIFF(DAY, PoDate, getdate()) AS days from CustomerPO_Hdr_Both where  PoDate between '" + txt_form_podate_search.Text + "'  AND '" + txt_to_podate_search.Text + "' AND Is_Deleted='0'", con);

        SqlDataAdapter sad = new SqlDataAdapter(
                     "SELECT C.Id, C.Quotationno, C.CustomerName," +
                     "C.SubCustomer, C.SubCustomer, C.Pono, C.PoDate, C.RefNo, *," +
                     "CASE " +
                         "WHEN NOT EXISTS (SELECT 1 FROM CustomerPO_Dtls_Both D WHERE D.PurchaseId = C.Id AND D.JobStatus != 'Completed') " +
                         "THEN (SELECT MAX(D.JobDaysCount) FROM CustomerPO_Dtls_Both D WHERE D.PurchaseId = C.Id) " +
                         "ELSE DATEDIFF(DAY, C.CreatedOn, GETDATE()) " +
                     "END AS CountDays " +
                     "FROM CustomerPO_Hdr_Both C " +
                     "WHERE C.Type = 'JobNo' AND C.PoDate between '" + txt_form_podate_search.Text + "'  AND '" + txt_to_podate_search.Text + "'" +
                     "ORDER BY C.CreatedOn DESC;", con);

        sad.Fill(dtt);
        GvSorted.EmptyDataText = "Records Not Found";
        GvSorted.DataSource = dtt;
        GvSorted.DataBind();
    }



    public void GetSortedDatewisedataGrid()
    {
        DataTable dtt = new DataTable();

       // SqlDataAdapter sad = new SqlDataAdapter("select  Id,CustomerName,SubCustomer,JobNo,Pono,PoDate,RefNo,Mobileno,Quotationno,CreatedBy,CreatedOn,DATEDIFF(DAY, PoDate, getdate()) AS days from CustomerPO_Hdr_Both where  PoDate between '" + txt_form_podate_search.Text + "'  AND '" + txt_to_podate_search.Text + "' AND Is_Deleted='0'", con);
        SqlDataAdapter sad = new SqlDataAdapter(
                    "SELECT C.Id, C.Quotationno, C.CustomerName," +
                    "C.SubCustomer, C.SubCustomer, C.Pono, C.PoDate, C.RefNo, *," +
                    "CASE " +
                        "WHEN NOT EXISTS (SELECT 1 FROM CustomerPO_Dtls_Both D WHERE D.PurchaseId = C.Id AND D.JobStatus != 'Completed') " +
                        "THEN (SELECT MAX(D.JobDaysCount) FROM CustomerPO_Dtls_Both D WHERE D.PurchaseId = C.Id) " +
                        "ELSE DATEDIFF(DAY, C.CreatedOn, GETDATE()) " +
                    "END AS CountDays " +
                    "FROM CustomerPO_Hdr_Both C " +
                    "WHERE C.Type = 'JobNo' AND C.PoDate between '" + txt_form_podate_search.Text + "'  AND '" + txt_to_podate_search.Text + "'" +
                    "ORDER BY C.CreatedOn DESC;", con);

        sad.Fill(dtt);
        GvSorted.EmptyDataText = "Records Not Found";
        GvSorted.DataSource = dtt;
        GvSorted.DataBind();
    }
    public void Getsortedquation()
    {
        GvCustomerpoList.Visible = false;
        ViewState["Record"] = "Quatation";
        DataTable dtt = new DataTable();

        //SqlDataAdapter sad = new SqlDataAdapter("select  Id,CustomerName,SubCustomer,JobNo,Pono,PoDate,RefNo,Mobileno,Quotationno,CreatedBy,CreatedOn,DATEDIFF(DAY, PoDate, getdate()) AS days from CustomerPO_Hdr_Both where  Pono = '" + txt_pono_search.Text + "' AND Is_Deleted='0'", con);
        SqlDataAdapter sad = new SqlDataAdapter(
                   "SELECT C.Id, C.Quotationno, C.CustomerName," +
                   "C.SubCustomer, C.SubCustomer, C.Pono, C.PoDate, C.RefNo, *," +
                   "CASE " +
                       "WHEN NOT EXISTS (SELECT 1 FROM CustomerPO_Dtls_Both D WHERE D.PurchaseId = C.Id AND D.JobStatus != 'Completed') " +
                       "THEN (SELECT MAX(D.JobDaysCount) FROM CustomerPO_Dtls_Both D WHERE D.PurchaseId = C.Id) " +
                       "ELSE DATEDIFF(DAY, C.CreatedOn, GETDATE()) " +
                   "END AS CountDays " +
                   "FROM CustomerPO_Hdr_Both C " +
                   "WHERE C.Type = 'JobNo' AND C. Pono = '" + txt_pono_search.Text + "'" +
                   "ORDER BY C.CreatedOn DESC;", con);
        sad.Fill(dtt);
        GvSorted.EmptyDataText = "Records Not Found";
        GvSorted.DataSource = dtt;
        GvSorted.DataBind();
    }
    public void GetsortedquationGrid()
    {
        GvCustomerpoList.Visible = false;
        DataTable dtt = new DataTable();

        //SqlDataAdapter sad = new SqlDataAdapter("select  Id,CustomerName,SubCustomer,JobNo,Pono,PoDate,RefNo,Mobileno,Quotationno,CreatedBy,CreatedOn,DATEDIFF(DAY, PoDate, getdate()) AS days from CustomerPO_Hdr_Both where  Pono = '" + txt_pono_search.Text + "' AND Is_Deleted='0'", con);

        SqlDataAdapter sad = new SqlDataAdapter(
                  "SELECT C.Id, C.Quotationno, C.CustomerName," +
                  "C.SubCustomer, C.SubCustomer, C.Pono, C.PoDate, C.RefNo, *," +
                  "CASE " +
                      "WHEN NOT EXISTS (SELECT 1 FROM CustomerPO_Dtls_Both D WHERE D.PurchaseId = C.Id AND D.JobStatus != 'Completed') " +
                      "THEN (SELECT MAX(D.JobDaysCount) FROM CustomerPO_Dtls_Both D WHERE D.PurchaseId = C.Id) " +
                      "ELSE DATEDIFF(DAY, C.CreatedOn, GETDATE()) " +
                  "END AS CountDays " +
                  "FROM CustomerPO_Hdr_Both C " +
                  "WHERE C.Type = 'JobNo' AND C.Pono = '" + txt_pono_search.Text + "'" +
                  "ORDER BY C.CreatedOn DESC;", con);

        sad.Fill(dtt);
        GvSorted.EmptyDataText = "Records Not Found";
        GvSorted.DataSource = dtt;
        GvSorted.DataBind();
    }
    public void Sortedcompony()
    {
        GvCustomerpoList.Visible = false;
        ViewState["Record"] = "company";
        DataTable dtt = new DataTable();

        //SqlDataAdapter sad = new SqlDataAdapter("select  Id,CustomerName,SubCustomer,JobNo,Pono,PoDate,RefNo,Mobileno,Quotationno,CreatedBy,CreatedOn,DATEDIFF(DAY, PoDate, getdate()) AS days from CustomerPO_Hdr_Both where CustomerName = '" + txtJobno.Text + "' AND  Is_Deleted='0'", con);
        SqlDataAdapter sad = new SqlDataAdapter(
                     "SELECT C.Id, C.Quotationno, C.CustomerName," +
                     "C.SubCustomer, C.SubCustomer, C.Pono, C.PoDate, C.RefNo, *," +
                     "CASE " +
                         "WHEN NOT EXISTS (SELECT 1 FROM CustomerPO_Dtls_Both D WHERE D.PurchaseId = C.Id AND D.JobStatus != 'Completed') " +
                         "THEN (SELECT MAX(D.JobDaysCount) FROM CustomerPO_Dtls_Both D WHERE D.PurchaseId = C.Id) " +
                         "ELSE DATEDIFF(DAY, C.CreatedOn, GETDATE()) " +
                     "END AS CountDays " +
                     "FROM CustomerPO_Hdr_Both C " +
                     "WHERE C.Type = 'JobNo' AND C.CustomerName = '" + txtJobno.Text + "'" +
                     "ORDER BY C.CreatedOn DESC;", con);


        sad.Fill(dtt);
        GvSorted.EmptyDataText = "Records Not Found";
        GvSorted.DataSource = dtt;
        GvSorted.DataBind();
    }
    public void Sortedcomponygrid()
    {
        GvCustomerpoList.Visible = false;
        DataTable dtt = new DataTable();

        //SqlDataAdapter sad = new SqlDataAdapter("select  Id,CustomerName,SubCustomer,JobNo,Pono,PoDate,RefNo,Mobileno,Quotationno,CreatedBy,CreatedOn,DATEDIFF(DAY, PoDate, getdate()) AS days from CustomerPO_Hdr_Both where CustomerName = '" + txtJobno.Text + "' AND  Is_Deleted='0'", con);
        SqlDataAdapter sad = new SqlDataAdapter(
                    "SELECT C.Id, C.Quotationno, C.CustomerName," +
                    "C.SubCustomer, C.SubCustomer, C.Pono, C.PoDate, C.RefNo, *," +
                    "CASE " +
                        "WHEN NOT EXISTS (SELECT 1 FROM CustomerPO_Dtls_Both D WHERE D.PurchaseId = C.Id AND D.JobStatus != 'Completed') " +
                        "THEN (SELECT MAX(D.JobDaysCount) FROM CustomerPO_Dtls_Both D WHERE D.PurchaseId = C.Id) " +
                        "ELSE DATEDIFF(DAY, C.CreatedOn, GETDATE()) " +
                    "END AS CountDays " +
                    "FROM CustomerPO_Hdr_Both C " +
                    "WHERE C.Type = 'JobNo' AND C.CustomerName = '" + txtJobno.Text + "'" +
                    "ORDER BY C.CreatedOn DESC;", con);


        sad.Fill(dtt);
        GvSorted.EmptyDataText = "Records Not Found";
        GvSorted.DataSource = dtt;
        GvSorted.DataBind();
    }
    public void Getdatewisecustomer()
    {
        GvCustomerpoList.Visible = false;
        ViewState["Record"] = "Datewisecustyomer";
        DataTable dtt = new DataTable();

       // SqlDataAdapter sad = new SqlDataAdapter("SELECT Id, CustomerName, SubCustomer, JobNo, Pono, PoDate, RefNo, Mobileno, Quotationno, CreatedBy, CreatedOn, DATEDIFF(DAY, PoDate, GETDATE()) AS days FROM CustomerPO_Hdr_Both WHERE PoDate BETWEEN '" + txt_form_podate_search.Text + "' AND '" + txt_to_podate_search.Text + "' AND CustomerName = '" + txtJobno.Text + "' AND Is_Deleted = '0'", con);


        SqlDataAdapter sad = new SqlDataAdapter(
           "SELECT C.Id, C.Quotationno, C.CustomerName," +
           "C.SubCustomer, C.SubCustomer, C.Pono, C.PoDate, C.RefNo, *," +
           "CASE " +
               "WHEN NOT EXISTS (SELECT 1 FROM CustomerPO_Dtls_Both D WHERE D.PurchaseId = C.Id AND D.JobStatus != 'Completed') " +
               "THEN (SELECT MAX(D.JobDaysCount) FROM CustomerPO_Dtls_Both D WHERE D.PurchaseId = C.Id) " +
               "ELSE DATEDIFF(DAY, C.CreatedOn, GETDATE()) " +
           "END AS CountDays " +
           "FROM CustomerPO_Hdr_Both C " +
           "WHERE C.Type = 'JobNo' AND C.PoDate BETWEEN '" + txt_form_podate_search.Text + "' AND '" + txt_to_podate_search.Text + "' AND C.CustomerName = '" + txtJobno.Text + "'" +
           "ORDER BY C.CreatedOn DESC;", con);
        sad.Fill(dtt);
        GvSorted.EmptyDataText = "Records Not Found";
        GvSorted.DataSource = dtt;
        GvSorted.DataBind();
    }
    public void Getdatewisecustomergrid()
    {
        GvCustomerpoList.Visible = false;
        DataTable dtt = new DataTable();

        //SqlDataAdapter sad = new SqlDataAdapter("SELECT Id, CustomerName, SubCustomer, JobNo, Pono, PoDate, RefNo, Mobileno, Quotationno, CreatedBy, CreatedOn, DATEDIFF(DAY, PoDate, GETDATE()) AS days FROM CustomerPO_Hdr_Both WHERE PoDate BETWEEN '" + txt_form_podate_search.Text + "' AND '" + txt_to_podate_search.Text + "' AND CustomerName = '" + txtJobno.Text + "' AND Is_Deleted = '0'", con);

        SqlDataAdapter sad = new SqlDataAdapter(
           "SELECT C.Id, C.Quotationno, C.CustomerName," +
           "C.SubCustomer, C.SubCustomer, C.Pono, C.PoDate, C.RefNo, *," +
           "CASE " +
               "WHEN NOT EXISTS (SELECT 1 FROM CustomerPO_Dtls_Both D WHERE D.PurchaseId = C.Id AND D.JobStatus != 'Completed') " +
               "THEN (SELECT MAX(D.JobDaysCount) FROM CustomerPO_Dtls_Both D WHERE D.PurchaseId = C.Id) " +
               "ELSE DATEDIFF(DAY, C.CreatedOn, GETDATE()) " +
           "END AS CountDays " +
           "FROM CustomerPO_Hdr_Both C " +
           "WHERE C.Type = 'JobNo' AND C.PoDate BETWEEN '" + txt_form_podate_search.Text + "' AND '" + txt_to_podate_search.Text + "' AND C.CustomerName = '" + txtJobno.Text + "'" +
           "ORDER BY C.CreatedOn DESC;", con);

        sad.Fill(dtt);
        GvSorted.EmptyDataText = "Records Not Found";
        GvSorted.DataSource = dtt;
        GvSorted.DataBind();
    }
    public void GetDatwisePo()
    {
        GvCustomerpoList.Visible = false;
        ViewState["Record"] = "DatewisePO";
        DataTable dtt = new DataTable();

        //SqlDataAdapter sad = new SqlDataAdapter("SELECT Id, CustomerName, SubCustomer, JobNo, Pono, PoDate, RefNo, Mobileno, Quotationno, CreatedBy, CreatedOn, DATEDIFF(DAY, PoDate, GETDATE()) AS days FROM CustomerPO_Hdr_Both WHERE PoDate BETWEEN '" + txt_form_podate_search.Text + "' AND '" + txt_to_podate_search.Text + "' AND  Pono = '" + txt_pono_search.Text + "' AND Is_Deleted = '0'", con);
        SqlDataAdapter sad = new SqlDataAdapter(
           "SELECT C.Id, C.Quotationno, C.CustomerName," +
           "C.SubCustomer, C.SubCustomer, C.Pono, C.PoDate, C.RefNo, *," +
           "CASE " +
               "WHEN NOT EXISTS (SELECT 1 FROM CustomerPO_Dtls_Both D WHERE D.PurchaseId = C.Id AND D.JobStatus != 'Completed') " +
               "THEN (SELECT MAX(D.JobDaysCount) FROM CustomerPO_Dtls_Both D WHERE D.PurchaseId = C.Id) " +
               "ELSE DATEDIFF(DAY, C.CreatedOn, GETDATE()) " +
           "END AS CountDays " +
           "FROM CustomerPO_Hdr_Both C " +
           "WHERE C.Type = 'JobNo' AND C.PoDate BETWEEN '" + txt_form_podate_search.Text + "' AND '" + txt_to_podate_search.Text + "' AND  C.Pono = '" + txt_pono_search.Text + "'" +
           "ORDER BY C.CreatedOn DESC;", con);

        sad.Fill(dtt);
        GvSorted.EmptyDataText = "Records Not Found";
        GvSorted.DataSource = dtt;
        GvSorted.DataBind();
    }
    public void GetDatwisePogrid()
    {
        GvCustomerpoList.Visible = false;
        DataTable dtt = new DataTable();

       // SqlDataAdapter sad = new SqlDataAdapter("SELECT Id, CustomerName, SubCustomer, JobNo, Pono, PoDate, RefNo, Mobileno, Quotationno, CreatedBy, CreatedOn, DATEDIFF(DAY, PoDate, GETDATE()) AS days FROM CustomerPO_Hdr_Both WHERE PoDate BETWEEN '" + txt_form_podate_search.Text + "' AND '" + txt_to_podate_search.Text + "' AND  Pono = '" + txt_pono_search.Text + "' AND Is_Deleted = '0'", con);
        SqlDataAdapter sad = new SqlDataAdapter(
          "SELECT C.Id, C.Quotationno, C.CustomerName," +
          "C.SubCustomer, C.SubCustomer, C.Pono, C.PoDate, C.RefNo, *," +
          "CASE " +
              "WHEN NOT EXISTS (SELECT 1 FROM CustomerPO_Dtls_Both D WHERE D.PurchaseId = C.Id AND D.JobStatus != 'Completed') " +
              "THEN (SELECT MAX(D.JobDaysCount) FROM CustomerPO_Dtls_Both D WHERE D.PurchaseId = C.Id) " +
              "ELSE DATEDIFF(DAY, C.CreatedOn, GETDATE()) " +
          "END AS CountDays " +
          "FROM CustomerPO_Hdr_Both C " +
          "WHERE C.Type = 'JobNo' AND C.PoDate BETWEEN '" + txt_form_podate_search.Text + "' AND '" + txt_to_podate_search.Text + "' AND  C.Pono = '" + txt_pono_search.Text + "'" +
          "ORDER BY C.CreatedOn DESC;", con);

        sad.Fill(dtt);
        GvSorted.EmptyDataText = "Records Not Found";
        GvSorted.DataSource = dtt;
        GvSorted.DataBind();
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
            if (Method == "GetSortedDatewisedata")
            {
                GetSortedDatewisedataForExcell();
            }
            if (Method == "Getsortedquation")
            {
                GetsortedquationForExcell();
            }
            if (Method == "Sortedcompony")
            {
                SortedcomponyForExcell();
            }
            if (Method == "Getdatewisecustomer")
            {
                GetdatewisecustomerForExcell();
            }
            if (Method == "GetDatwisePo")
            {
                GetDatwisePoForExccell();
            }
            if (Method == "GetJobNowise")
            {
                GetJobNowiseForExcell();
            }
            if (Method == "GetCustomerowisejob")
            {
                GetCustomerowisejobForExcell();
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
        string FileName = "CustomerPO_List_" + DateTime.Now + ".xls";
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

    public void GetSortedDatewisedataForExcell()
    {
        GvCustomerpoList.Visible = false;
        ViewState["Record"] = "Date";
        DataTable dtt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select  Id,CustomerName,SubCustomer,JobNo,Pono,PoDate,RefNo,Mobileno,Quotationno,CreatedBy,CreatedOn,DATEDIFF(DAY, PoDate, getdate()) AS days from CustomerPO_Hdr_Both where  PoDate between '" + txt_form_podate_search.Text + "'  AND '" + txt_to_podate_search.Text + "' AND Is_Deleted='0'", con);
        sad.Fill(dtt);
        GridExportExcel.EmptyDataText = "Records Not Found";
        GridExportExcel.DataSource = dtt;
        GridExportExcel.DataBind();
    }

    public void GetsortedquationForExcell()
    {
        GvCustomerpoList.Visible = false;
        ViewState["Record"] = "Quatation";
        DataTable dtt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select  Id,CustomerName,SubCustomer,JobNo,Pono,PoDate,RefNo,Mobileno,Quotationno,CreatedBy,CreatedOn,DATEDIFF(DAY, PoDate, getdate()) AS days from CustomerPO_Hdr_Both where  Pono = '" + txt_pono_search.Text + "' AND Is_Deleted='0'", con);
        sad.Fill(dtt);
        GridExportExcel.EmptyDataText = "Records Not Found";
        GridExportExcel.DataSource = dtt;
        GridExportExcel.DataBind();
    }

    public void SortedcomponyForExcell()
    {
        GvCustomerpoList.Visible = false;
        ViewState["Record"] = "company";
        DataTable dtt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select  Id,CustomerName,SubCustomer,JobNo,Pono,PoDate,RefNo,Mobileno,Quotationno,CreatedBy,CreatedOn,DATEDIFF(DAY, PoDate, getdate()) AS days from CustomerPO_Hdr_Both where CustomerName = '" + txtJobno.Text + "' AND  Is_Deleted='0'", con);
        sad.Fill(dtt);
        GridExportExcel.EmptyDataText = "Records Not Found";
        GridExportExcel.DataSource = dtt;
        GridExportExcel.DataBind();
    }

    public void GetdatewisecustomerForExcell()
    {
        GvCustomerpoList.Visible = false;
        ViewState["Record"] = "Datewisecustyomer";
        DataTable dtt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT Id, CustomerName, SubCustomer, JobNo, Pono, PoDate, RefNo, Mobileno, Quotationno, CreatedBy, CreatedOn, DATEDIFF(DAY, PoDate, GETDATE()) AS days FROM CustomerPO_Hdr_Both WHERE PoDate BETWEEN '" + txt_form_podate_search.Text + "' AND '" + txt_to_podate_search.Text + "' AND CustomerName = '" + txtJobno.Text + "' AND Is_Deleted = '0'", con);

        sad.Fill(dtt);
        GridExportExcel.EmptyDataText = "Records Not Found";
        GridExportExcel.DataSource = dtt;
        GridExportExcel.DataBind();
    }

    public void GetDatwisePoForExccell()
    {
        GvCustomerpoList.Visible = false;
        ViewState["Record"] = "DatewisePO";
        DataTable dtt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT Id, CustomerName, SubCustomer, JobNo, Pono, PoDate, RefNo, Mobileno, Quotationno, CreatedBy, CreatedOn, DATEDIFF(DAY, PoDate, GETDATE()) AS days FROM CustomerPO_Hdr_Both WHERE PoDate BETWEEN '" + txt_form_podate_search.Text + "' AND '" + txt_to_podate_search.Text + "' AND  Pono = '" + txt_pono_search.Text + "' AND Is_Deleted = '0'", con);
        sad.Fill(dtt);
        GridExportExcel.EmptyDataText = "Records Not Found";
        GridExportExcel.DataSource = dtt;
        GridExportExcel.DataBind();
    }

    public void GetJobNowise()
    {
        DataTable dtt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT hdr.Id,CustomerName,SubCustomer,DTLS.JobNo,Pono,PoDate,RefNo,Mobileno,hdr.Quotationno,CreatedBy,CreatedOn,DATEDIFF(DAY, PoDate, getdate()) AS days  FROM [DB_EndeAuto].[EndeavourAuto].[CustomerPO_Dtls] As DTLS Inner join [DB_EndeAuto].[EndeavourAuto].[CustomerPO_Hdr_Both] AS hdr on hdr.Quotationno = DTLS.Quotationno where DTLS.JobNo = '" + txtjob.Text + "' AND  hdr.Is_Deleted='0'", con);
        sad.Fill(dtt);
        ViewState["Record"] = "Jobwise";
        GvCustomerpoList.EmptyDataText = "Records Not Found";
        GvCustomerpoList.DataSource = dtt;
        GvCustomerpoList.DataBind();
    }



    public void GetJobNowiseForExcell()
    {
        DataTable dtt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT hdr.Id,CustomerName,SubCustomer,DTLS.JobNo,Pono,PoDate,RefNo,Mobileno,hdr.Quotationno,CreatedBy,CreatedOn,DATEDIFF(DAY, PoDate, getdate()) AS days  FROM [DB_EndeAuto].[EndeavourAuto].[CustomerPO_Dtls] As DTLS Inner join [DB_EndeAuto].[EndeavourAuto].[CustomerPO_Hdr_Both] AS hdr on hdr.Quotationno = DTLS.Quotationno where DTLS.JobNo = '" + txtjob.Text + "' AND  hdr.Is_Deleted='0'", con);
        sad.Fill(dtt);
        ViewState["Record"] = "Jobwise";
        GridExportExcel.EmptyDataText = "Records Not Found";
        GridExportExcel.DataSource = dtt;
        GridExportExcel.DataBind();
    }



    public void GetCustomerowisejob()
    {
        DataTable dtt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT hdr.Id, CustomerName, SubCustomer, DTLS.JobNo, Pono, PoDate, RefNo, Mobileno, hdr.Quotationno, CreatedBy, CreatedOn, DATEDIFF(DAY, PoDate, GETDATE()) AS days FROM [DB_EndeAuto].[EndeavourAuto].[CustomerPO_Dtls] AS DTLS INNER JOIN [DB_EndeAuto].[EndeavourAuto].[CustomerPO_Hdr_Both] AS hdr ON hdr.Quotationno = DTLS.Quotationno WHERE DTLS.JobNo = '" + txtjob.Text + "' AND hdr.CustomerName = '" + txtJobno.Text + "' AND hdr.Is_Deleted = '0'", con);

        sad.Fill(dtt);
        ViewState["Record"] = "CustomerwiseJob";
        GvCustomerpoList.EmptyDataText = "Records Not Found";
        GvCustomerpoList.DataSource = dtt;
        GvCustomerpoList.DataBind();
    }

    public void GetCustomerowisejobForExcell()
    {
        DataTable dtt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT hdr.Id, CustomerName, SubCustomer, DTLS.JobNo, Pono, PoDate, RefNo, Mobileno, hdr.Quotationno, CreatedBy, CreatedOn, DATEDIFF(DAY, PoDate, GETDATE()) AS days FROM [DB_EndeAuto].[EndeavourAuto].[CustomerPO_Dtls] AS DTLS INNER JOIN [DB_EndeAuto].[EndeavourAuto].[CustomerPO_Hdr_Both] AS hdr ON hdr.Quotationno = DTLS.Quotationno WHERE DTLS.JobNo = '" + txtjob.Text + "' AND hdr.CustomerName = '" + txtJobno.Text + "' AND hdr.Is_Deleted = '0'", con);

        sad.Fill(dtt);
        ViewState["Record"] = "CustomerwiseJob";
        GridExportExcel.EmptyDataText = "Records Not Found";
        GridExportExcel.DataSource = dtt;
        GridExportExcel.DataBind();
    }

    protected void ddlservicetype_TextChanged(object sender, EventArgs e)
    {
        GvCustomerpoList.Visible = false;
        DataTable dtt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select  *,CASE WHEN NOT EXISTS (SELECT 1 FROM CustomerPO_Dtls_Both D WHERE D.PurchaseId = C.Id AND D.JobStatus != 'Completed') THEN (SELECT MAX(D.JobDaysCount) FROM CustomerPO_Dtls_Both D WHERE D.PurchaseId = C.Id) ELSE DATEDIFF(DAY, C.CreatedOn, GETDATE()) END AS CountDays ,Id,CustomerName,SubCustomer,JobNo,Pono,PoDate,RefNo,Mobileno,Quotationno,CreatedBy,CreatedOn,DATEDIFF(DAY, PoDate, getdate()) AS days from CustomerPO_Hdr_Both C where  ServiceType ='" + ddlservicetype.Text + "' AND Is_Deleted = '0'", con);

        // SqlDataAdapter sad = new SqlDataAdapter("select ID,Quotation_no,Quotation_Date,ExpiryDate,JobNo,Customer_Name,SubCustomer,Address,Mobile_No,Phone_No,GST_No,State_Code,kind_Att,CGST,SGST,AllTotal_price,Total_in_word,IsDeleted,CreatedBy,CreatedOn, DATEDIFF(DAY, Quotation_Date, getdate()) AS days from tbl_Quotation_Hdr where ServiceType ='" + ddlservicetype.Text + "' AND isdeleted = '0'", con);
        sad.Fill(dtt);
        GvSorted.EmptyDataText = "Records Not Found";
        GvSorted.DataSource = dtt;
        GvSorted.DataBind();

    }





    // Nikhil Code for Pending Quotations
     
    protected void gv_Quot_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton Lnk_Edit = (LinkButton)e.Row.FindControl("lnkbtnEdit");
            LinkButton Lnk_Create = (LinkButton)e.Row.FindControl("lnkCreateQua");
            LinkButton Lnk_vw_Quo = (LinkButton)e.Row.FindControl("lnkBtn_View");
            Label lbl_Quo_no = (Label)e.Row.FindControl("lblQuNo");
            Label lblcompanyname = (Label)e.Row.FindControl("lblCompName");
            Label lblsubcustomer = (Label)e.Row.FindControl("lblsubcustomer");

            SqlDataAdapter Da = new SqlDataAdapter("SELECT Quotation_no From tbl_Quotation_two_Hdr WHERE Quotation_no='" + lbl_Quo_no + "'", con);
            DataTable Dt = new DataTable();
            Da.Fill(Dt);
           
            string Id = gv_Quot_List.DataKeys[e.Row.RowIndex].Value.ToString();
            GridView gvDetails = e.Row.FindControl("gvDetails") as GridView;

            SqlDataAdapter Daaa = new SqlDataAdapter(" SELECT * FROM [tbl_Quotation_two_Dtls] WHERE [Quotation_no]='" + Id + "'", con);
            DataTable Dttt = new DataTable();
            Daaa.Fill(Dttt);
            foreach (DataRow row in Dttt.Rows)
            {
                if (row["JobDaysCount"] == DBNull.Value || Convert.ToInt32(row["JobDaysCount"]) == 0)
                {
                    DateTime createdOn = Convert.ToDateTime(row["CreatedOn"]);
                   
                    
                    DateTime createdOnDateOnly = createdOn.Date;

                    
                    int jobDaysCount = (DateTime.Now.Date - createdOnDateOnly).Days;
                    row["JobDaysCount"] = jobDaysCount;
                }                
            }
            gvDetails.DataSource = Dttt;
            gvDetails.DataBind();

            foreach (GridViewRow gvRow in gvDetails.Rows)
            {
                // Find the checkbox control in the current row of gvDetails
                CheckBox chkSelect = (CheckBox)gvRow.FindControl("chkSelect");

                // Find the JobStatus label control in the current row of gvDetails
                Label lblJobStatus = (Label)gvRow.FindControl("lblJobStatus");

                // If JobStatus is "Completed", disable the checkbox
                if (lblJobStatus != null && lblJobStatus.Text == "Completed" && chkSelect != null)
                {
                    chkSelect.Enabled = false; // Disable the checkbox
                }
            }
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
                SELECT  H.ID,  H.Quotation_no, H.Quotation_Date, H.ExpiryDate, H.CreatedOn AS QuotationCreatedOn, H.Customer_Name, 
                    H.SubCustomer, H.Address, H.Mobile_No, H.Phone_No, H.GST_No, H.State_Code, H.kind_Att,  H.CGST, H.SGST, 
                    H.AllTotal_price, H.Total_in_word, H.IsDeleted, H.CreatedBy, H.CreatedOn, H.Againstby,                     
                    ISNULL(J.JobCount, 0) AS JobCount
                FROM tbl_Quotation_two_Hdr H
                LEFT JOIN (
                    SELECT Quotation_no, COUNT(JobNo) AS JobCount
                    FROM tbl_Quotation_two_Dtls
                    WHERE JobStatus ='Pending' 
                    GROUP BY Quotation_no
                ) J ON H.Quotation_no = J.Quotation_no
                WHERE H.Againstby = 'JobNo'  AND H.Customer_Name = 'Schneider Electric India Pvt.Ltd.' AND H.IsDeleted = '0'
                AND H.CreatedOn >= @StartDate AND H.CreatedOn <= @EndDate AND H.JobNoCount != 0 AND Status = 'Pending'
                ORDER BY H.CreatedOn DESC;";

                SqlDataAdapter Da = new SqlDataAdapter(query, con);
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
                SELECT  H.ID,  H.Quotation_no, H.Quotation_Date, H.ExpiryDate, H.CreatedOn AS QuotationCreatedOn, H.Customer_Name, 
                    H.SubCustomer, H.Address, H.Mobile_No, H.Phone_No, H.GST_No, H.State_Code, H.kind_Att,  H.CGST, H.SGST, 
                    H.AllTotal_price, H.Total_in_word, H.IsDeleted, H.CreatedBy, H.CreatedOn, H.Againstby,                     
                    ISNULL(J.JobCount, 0) AS JobCount
                FROM tbl_Quotation_two_Hdr H
                LEFT JOIN (
                    SELECT Quotation_no, COUNT(JobNo) AS JobCount
                    FROM tbl_Quotation_two_Dtls
                    WHERE JobStatus ='Pending'
                    GROUP BY Quotation_no
                ) J ON H.Quotation_no = J.Quotation_no 
                WHERE H.Againstby = 'JobNo' AND H.IsDeleted = '0' AND Status = 'Pending' 
                AND H.CreatedOn >= @StartDate AND H.CreatedOn <= @EndDate AND H.JobNoCount != 0
                ORDER BY H.CreatedOn DESC;";

                SqlDataAdapter Da = new SqlDataAdapter(query, con);
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
           
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM tbl_Quotation_two_Dtls WHERE JobNo IS NOT NULL AND JobStatus = 'Pending' AND CreatedOn >= '" + formattedStartDate + "' AND CreatedOn <= '" + formattedEndDate + "'", con);
            con.Open();
            jobCount = (int)cmd.ExecuteScalar();
        }

        return jobCount;
    }

    protected void lnkbtnCorrect_Click(object sender, EventArgs e)
    {
        LinkButton clickedButton = (LinkButton)sender;
        string QoutationNO = clickedButton.CommandArgument;

        Response.Redirect("Customer_PO.aspx?QuoNo=" + encrypt(QoutationNO) + "");
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
                                   
                    SqlCommand cmd = new SqlCommand("UPDATE tbl_Quotation_two_Dtls SET JobStatus = 'Closed'," +
                        " JobDaysCount = DATEDIFF(DAY, CreatedOn, GETDATE())" +
                        "  WHERE JobNo = '" + jobNo + "'", con);
                    con.Open();
                    cmd.ExecuteScalar();
                }
            }
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }


    protected void GvSorted_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            decimal totalAmount = 0;

            if (GvSorted.Rows.Count > 0)
            {
                foreach (GridViewRow row in GvSorted.Rows)
                {

                    Label lblgrandtotal = row.FindControl("lblgrandtotal") as Label;
                    if (lblgrandtotal != null)
                    {
                        if (decimal.TryParse(lblgrandtotal.Text, out decimal rowAmount))
                        {
                            totalAmount += rowAmount;
                        }
                    }
                }
            }
            else
            {
                foreach (GridViewRow row in GvCustomerpoList.Rows)
                {
                    Label lblgrandtotal = row.FindControl("lblgrandtotal") as Label;
                    if (lblgrandtotal != null)
                    {
                        if (decimal.TryParse(lblgrandtotal.Text, out decimal rowAmount))
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

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string Id = GvSorted.DataKeys[e.Row.RowIndex].Value.ToString();
            GridView gvDetailss = e.Row.FindControl("gvDetailss") as GridView;



            string QuatationNo = GvSorted.DataKeys[e.Row.RowIndex]["Quotationno"].ToString();

            if (txtjob.Text == "")
            {
                string query1 = string.Empty;
                query1 = @"SELECT [JobNo],[JobStatus],[JobDaysCount],[PurchaseId] FROM [CustomerPO_Dtls_Both] WHERE Quotationno ='" + Id + "'";
                SqlDataAdapter ad = new SqlDataAdapter(query1, con);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    if (row["JobDaysCount"] == DBNull.Value || Convert.ToInt32(row["JobDaysCount"]) == 0 && row["JobStatus"].ToString() == "Pending")
                    {
                        con.Open();
                        SqlCommand cmdss = new SqlCommand("SELECT CreatedOn FROM CustomerPO_Hdr_Both WHERE Id = '" + row["PurchaseId"] + "'", con);
                        object result = cmdss.ExecuteScalar();

                        if (result != DBNull.Value)
                        {
                            DateTime createdOn = Convert.ToDateTime(result);
                            DateTime createdOnDateOnly = createdOn.Date;
                            int jobDaysCount = (DateTime.Now.Date - createdOnDateOnly).Days;
                            row["JobDaysCount"] = jobDaysCount;
                        }
                        con.Close();
                    }
                }
                gvDetailss.DataSource = dt;
                gvDetailss.DataBind();
            }
            else
            {
                string query1 = string.Empty;
                query1 = @"SELECT Top 1 [JobNo], * FROM [CustomerPO_Dtls_Both] WHERE JobNo ='" + txtjob.Text + "'";
                SqlDataAdapter ad = new SqlDataAdapter(query1, con);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                gvDetailss.DataSource = dt;
                gvDetailss.DataBind();
            }


        }
    }
}