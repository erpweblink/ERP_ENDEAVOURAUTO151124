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

public partial class Admin_TaxInvoice : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

    DataTable Dt_Items = new DataTable();
    DataTable Dt_Mail = new DataTable();
    DataTable Dt_Itemsdetails = new DataTable();
    string ID;
    string chkupdate;
    string id = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TaxPanel3.Visible = false;
            TaxPanel2.Visible = false;
            TaxPanel1.Visible = false;

            this.txt_InvoiceDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            this.txt_InvoiceDate.TextMode = TextBoxMode.Date;
            this.txt_ChallanDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            this.txt_ChallanDate.TextMode = TextBoxMode.Date;

            ViewState["RowNo"] = 0;
            Dt_Items.Columns.AddRange(new DataColumn[10] { new DataColumn("Id"), new DataColumn("MateName"), new DataColumn("PrintDescription"), new DataColumn("HSN/SAC"), new DataColumn("Rate"), new DataColumn("Unit"), new DataColumn("Quntity"), new DataColumn("Tax"), new DataColumn("Discount"), new DataColumn("TotalAmount") });
            ViewState["Invoice"] = Dt_Items;

            //mail temprary table
            ViewState["RowNo"] = 0;
            Dt_Mail.Columns.AddRange(new DataColumn[2] { new DataColumn("mailid"), new DataColumn("mailtext") });
            ViewState["MULTMail"] = Dt_Mail;

            ///details gridview temprary table
            if (Request.QueryString["Id"] != null)
            {
                ID = Decrypt(Request.QueryString["Id"].ToString());
                hdnID.Value = ID;
                Load_Record();
                //GetJobNO();
                //DataTable dt1 = new DataTable();
                //SqlDataAdapter sad1 = new SqlDataAdapter("select * from tblCustomerContactPerson where CustName='" + txtCompName.Text + "'", con);
                //sad1.Fill(dt1);
                //txt_KindAtt.DataTextField = "ContactPerName";
                //txt_KindAtt.DataSource = dt1;
                //txt_KindAtt.DataBind();
                //GenerateInvoice();
                //GenerateChallan();
            }

            else
            {
                GenerateInvoice();
                GenerateChallan();
            }

            if (Request.QueryString["InvoiceNo"] != null)
            {
                id = Decrypt(Request.QueryString["InvoiceNo"].ToString());
                ReportDataLoad();
                ReadOnlyReport();
            }

            //NEW CODE FOR FETCH THE QUOTATION DATA START
            //Edit Quotation
            if (Request.QueryString["QuatationNo"] != null)
            {
                Server.UrlDecode(Request.QueryString["QuatationNo"]);
                //Added Decrypt 
                string ID = Decrypt(Server.UrlDecode(Request.QueryString["QuatationNo"]));

                //ID = Decrypt(Request.QueryString["QuatationNo"].ToString());
                ShowHeaderEdit(ID);
                ShowDtlEdit(ID);
                hidden.Value = ID;
            }

            if (Request.QueryString["ID"] != null)
            {
                //Idd = Decrypt(Request.QueryString["ID"].ToString());
                //ReportLoadData();
                //reportresdonly();
            }
            //NEW CODE FOR FETCH THE QUOTATION DATA END
        }
    }

    //protected void ddljobnobind()
    //{
    //    try
    //    {
    //        DataTable dt = new DataTable();
    //        con.Open();
    //        SqlDataAdapter sad = new SqlDataAdapter("SELECT Id,JobNo FROM [tblInwardEntry] WHERE CustName = '" + txtCompName.Text + "' ", con);
    //        sad.Fill(dt);
    //        txt_jobno.DataSource = dt;
    //        txt_jobno.DataTextField = "JobNo";
    //        txt_jobno.DataValueField = "Id";
    //        txt_jobno.DataBind();
    //        con.Close();
    //    }
    //    catch (Exception ex)
    //    {
    //        string errorMsg = "An error occurred : " + ex.Message;
    //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + errorMsg + "') ", true);
    //    }
    //}

    //NEW METHODS FOR QUOTATION DATA FETCH START
    protected void ShowHeaderEdit(string ID)


    {
        SqlDataAdapter Da = new SqlDataAdapter("SELECT *,DATEDIFF(DAY, CreatedOn, GETDATE()) AS Days_Completed FROM [CustomerPO_Hdr_Both] WHERE Quotationno='" + ID + "'", con);
        DataTable Dt = new DataTable();
        Da.Fill(Dt);
        if (Dt.Rows.Count > 0)
        {
            ddlagainst.Enabled = false;
            ddlagainstno.Enabled = false;
            ddlagainst.SelectedItem.Text = "Order";
            txtcountst.Text = Dt.Rows[0]["Days_Completed"].ToString(); //New added by Shubham Patil
            textquotationid.Value = Dt.Rows[0]["ID"].ToString();



            txtCompName.Text = Dt.Rows[0]["CustomerName"].ToString();
            // ddlagainstno.SelectedValue = Dt.Rows[0]["Pono"].ToString();
            txt_CompanyAddress.Text = Dt.Rows[0]["ShippingAddress"].ToString();
            txt_PoNo.Text = Dt.Rows[0]["Pono"].ToString();
          

            DateTime ffff18 = Convert.ToDateTime(Dt.Rows[0]["PoDate"].ToString());
            txt_poDate.Text = ffff18.ToString("yyyy-MM-dd");

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

            if (arrstr.Length > 0)
            {
                txt_term_1.Text = arrstr[0].ToString();
                txt_condition_1.Text = arrstr[1].ToString();
            }
            if (arrstr1.Length > 0)
            {
                txt_term_2.Text = arrstr1[0].ToString();
                txt_condition_2.Text = arrstr1[1].ToString();
            }
            if (arrstr2.Length > 0)
            {
                txt_term_3.Text = arrstr2[0].ToString();
                txt_condition_3.Text = arrstr2[1].ToString();
            }
            if (arrstr3.Length > 0)
            {
                txt_term_4.Text = arrstr3[0].ToString();
                txt_condition_4.Text = arrstr3[1].ToString();
            }

            if (arrstr4.Length > 0)
            {
                txt_term_5.Text = arrstr4[0].ToString();
                txt_condition_5.Text = arrstr4[1].ToString();
            }
            if (arrstr5.Length > 0)
            {
                txt_term_6.Text = arrstr5[0].ToString();
                txt_condition_6.Text = arrstr5[1].ToString();
            }
        }

        SqlDataAdapter Daa = new SqlDataAdapter("SELECT * FROM [tblCustomer] WHERE CustomerName ='" + txtCompName.Text + "'", con);
        DataTable Dtt = new DataTable();
        Daa.Fill(Dtt);
        if (Dtt.Rows.Count > 0)
        {
            txt_CompanyPanNo.Text = Dtt.Rows[0]["PanNo"].ToString();
            txt_CompanyGSTno.Text = Dtt.Rows[0]["GSTNo"].ToString();
            txt_CompanyStateCode.Text = Dtt.Rows[0]["StateCode"].ToString();
        }

        SqlDataAdapter Sda = new SqlDataAdapter("SELECT * FROM tblCustomerContactPerson WHERE CustName='" + txtCompName.Text + "'", con);
        DataTable Sdt = new DataTable();
        Sda.Fill(Sdt);
        Grd_MAIL.DataSource = Sdt;
        Grd_MAIL.DataBind();
    }

    protected void ShowDtlEdit(string ID)
    {
        ////Automatic description bind in job number from quaation details table
        DataTable dt3 = new DataTable();
        SqlDataAdapter sad3 = new SqlDataAdapter("select * from [CustomerPO_Dtls_Both] where Quotationno='" + ID + "'", con);
        sad3.Fill(dt3);
        int count = 1;
        if (dt3.Rows.Count > 0)
        {
            ViewState["RowNo"] = 0;
            Dt_Itemsdetails.Columns.AddRange(new DataColumn[10] { new DataColumn("Id"), new DataColumn("MateName"),
                   new DataColumn("PrintDescription"), new DataColumn("Hsn_Sac"),
                    new DataColumn("Rate"),  new DataColumn("Unit"),
                    new DataColumn("Quantity"),  new DataColumn("TaxPercenteage"),
                    new DataColumn("DiscountPercentage"), new DataColumn("Total")
                  });

            ViewState["Customerdetails"] = Dt_Itemsdetails;
            for (int i = 0; i < dt3.Rows.Count; i++)
            {
                Dt_Itemsdetails.Rows.Add(count, dt3.Rows[i]["MateName"].ToString(), dt3.Rows[i]["PrintDescription"].ToString(), dt3.Rows[i]["Hsn_Sac"].ToString(), dt3.Rows[i]["Rate"].ToString(), dt3.Rows[i]["Unit"].ToString(), dt3.Rows[i]["Quantity"].ToString(), dt3.Rows[i]["TaxPercenteage"].ToString(), dt3.Rows[i]["DiscountPercentage"].ToString(), dt3.Rows[i]["Total"].ToString());
                count = count + 1;
            }
            grd_getDTLS.DataSource = Dt_Itemsdetails;
            grd_getDTLS.DataBind();
            grd_getDTLS.EmptyDataText = "Not Records Found"; //      
        }


        SqlDataAdapter Sda = new SqlDataAdapter("SELECT * FROM tbl_Quotation_Hdr_Sales WHERE Quotation_no='" + ID + "'", con);
        DataTable Sdt = new DataTable();
        Sda.Fill(Sdt);
        if (Sdt.Rows.Count > 0)
        {
            txt_cgst_amt.Text = Sdt.Rows[0]["CGST"].ToString();
            txt_sgst_amt.Text = Sdt.Rows[0]["SGST"].ToString();
            // txt_igst_amt.Text = Sdt.Rows[0]["IGST"].ToString();
            txt_grand_total.Text = Sdt.Rows[0]["AllTotal_price"].ToString();
        }
    }

    //NEW METHODS FOR QUOTATION DATA FETCH END


    //Bind Against No
    private void Fillddlagainstnumber()
    {
        SqlDataAdapter ad = new SqlDataAdapter("SELECT DISTINCT [AgainstNo] FROM [tbl_Invoice_both_hdr] ", con);
        DataTable dt = new DataTable();
        ad.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            ddlagainstno.DataSource = dt;
            ddlagainstno.DataTextField = "AgainstNo";
            ddlagainstno.DataBind();
            ddlagainstno.Items.Insert(0, " ---  Select Type  --- ");
        }
    }

    protected void ReadOnlyReport()
    {
        tbl.Visible = false;
        btn_Cancel.Text = "Close";
        txt_InvoiceDate.ReadOnly = true;
        txt_InvoiceNo.ReadOnly = true;
        txt_ChallanDate.ReadOnly = true;
        txt_challanNo.ReadOnly = true;
        txt_poDate.ReadOnly = true;
        txt_PoNo.ReadOnly = true;
        txt_CompanyAddress.ReadOnly = true;
        txt_CustomerGstNo.ReadOnly = true;
        txt_CustomerPanNo.ReadOnly = true;
        txt_CustomerStateCode.ReadOnly = true;
        txt_CompanyStateCode.ReadOnly = true;
        txt_CompanyGSTno.ReadOnly = true;
        txt_CompanyPanNo.ReadOnly = true;
        txt_ShipingAdddesss.ReadOnly = true;
        txt_Payterm.ReadOnly = true;
        //txt_KindAtt.ReadOnly = true;
        //txt_Delivery.ReadOnly = true;
        drop_CompanyRegisterType.Enabled = false;
        drop_CustomerRagisterType.Enabled = false;
        check_address.Enabled = false;
        btn_save.Visible = false;
        //txt_gst_no.ReadOnly = true;
        //btn_save.Visible = false;
        gvPurchaseRecord.Columns[9].Visible = false;
        Grd_MAIL.Columns[2].Visible = false;
        headerreport.InnerText = " Invoice Report";
        mailcheck.Visible = false;
        //txtJobNo.ReadOnly = true;
        txtCompName.ReadOnly = true;
        txtCustName.ReadOnly = true;
        txt_term_1.ReadOnly = true;
        txt_term_2.ReadOnly = true;
        txt_term_3.ReadOnly = true;
        txt_term_4.ReadOnly = true;
        txt_condition_1.ReadOnly = true;
        txt_condition_2.ReadOnly = true;
        txt_condition_3.ReadOnly = true;
        txt_condition_4.ReadOnly = true;
        txt_condition_5.ReadOnly = true;
        txt_condition_6.ReadOnly = true;
    }

    protected void ReportDataLoad()
    {
        DataTable Dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter("select * from vw_InvoiceReport where InvoiceNo='" + id + "'", con);
        da.Fill(Dt);

        if (Dt.Rows.Count > 0)
        {
            //btn_save.Text = "Update";
            txt_InvoiceNo.Text = Dt.Rows[0]["InvoiceNo"].ToString();
            DateTime ffff1 = Convert.ToDateTime(Dt.Rows[0]["InvoiceDate"].ToString());
            txt_InvoiceDate.Text = ffff1.ToString("yyyy-MM-dd");
            txt_challanNo.Text = Dt.Rows[0]["ChallanNo"].ToString();
            DateTime ffff2 = Convert.ToDateTime(Dt.Rows[0]["ChallanDate"].ToString());
            txt_ChallanDate.Text = ffff1.ToString("yyyy-MM-dd");
            txt_PoNo.Text = Dt.Rows[0]["PoNo"].ToString();
            DateTime ffff3 = Convert.ToDateTime(Dt.Rows[0]["PoDate"].ToString());
            txt_poDate.Text = ffff1.ToString("yyyy-MM-dd");
            txt_CompanyAddress.Text = Dt.Rows[0]["CompanyAddress"].ToString();
            txt_ShipingAdddesss.Text = Dt.Rows[0]["CustomerShippingAddress"].ToString();
            txt_CompanyGSTno.Text = Dt.Rows[0]["CompanyGstNo"].ToString();
            txt_CustomerGstNo.Text = Dt.Rows[0]["CustomerGstNo"].ToString();
            txt_CompanyPanNo.Text = Dt.Rows[0]["CompanyPanNo"].ToString();
            txt_CustomerPanNo.Text = Dt.Rows[0]["CustomerPanNo"].ToString();
            txt_CompanyStateCode.Text = Dt.Rows[0]["CompanyStateCode"].ToString();
            txt_CustomerStateCode.Text = Dt.Rows[0]["CustomerStateCode"].ToString();
            txt_Payterm.Text = Dt.Rows[0]["PayTerm"].ToString();

            // txt_Delivery.Text = Dt.Rows[0]["Delivery"].ToString();
            lbl_Amount_In_Word.Text = Dt.Rows[0]["TotalInWord"].ToString();
            txt_grand_total.Text = Dt.Rows[0]["GrandTotal"].ToString();
            txt_sgst_amt.Text = Dt.Rows[0]["SGST"].ToString();
            txt_cgst_amt.Text = Dt.Rows[0]["CGST"].ToString();
            txt_igst_amt.Text = Dt.Rows[0]["IGST"].ToString();
            // txtJobNo.Text = Dt.Rows[0]["JobNo"].ToString();
            txtCompName.Text = Dt.Rows[0]["CompName"].ToString();
            txtCustName.Text = Dt.Rows[0]["CustName"].ToString();
            drop_CompanyRegisterType.Text = Dt.Rows[0]["ComapyRegType"].ToString();
            drop_CustomerRagisterType.Text = Dt.Rows[0]["CustomerRegType"].ToString();
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

            if (arrstr.Length > 0)
            {
                txt_term_1.Text = arrstr[0].ToString();
                txt_condition_1.Text = arrstr[0].ToString();
            }

            if (arrstr1.Length > 0)
            {
                txt_term_2.Text = arrstr1[0].ToString();
                txt_condition_2.Text = arrstr1[0].ToString();
            }

            if (arrstr2.Length > 0)
            {
                txt_term_3.Text = arrstr2[0].ToString();
                txt_condition_3.Text = arrstr2[0].ToString();
            }
            if (arrstr3.Length > 0)
            {
                txt_term_4.Text = arrstr3[0].ToString();
                txt_condition_4.Text = arrstr3[0].ToString();
            }
            if (arrstr4.Length > 0)
            {
                txt_term_5.Text = arrstr4[0].ToString();
                txt_condition_5.Text = arrstr3[0].ToString();
            }
            if (arrstr5.Length > 0)
            {
                txt_term_6.Text = arrstr5[0].ToString();
                txt_condition_6.Text = arrstr3[0].ToString();
            }

        }

        DataTable Dt_Product = new DataTable();
        SqlDataAdapter daa = new SqlDataAdapter("SELECT JobNo,Description,Hsn,Rate,Unit,Quntity,TaxPercentage,DiscountPercentage,Total FROM vw_InvoiceReport WHERE InvoiceNo='" + id + "'", con);
        da.Fill(Dt_Product);

        int Count = 1;
        if (Dt_Product.Rows.Count > 0)
        {
            if (Dt_Items.Columns.Count < 1)
            {
                ShowGrid();
            }

            for (int i = 0; i < Dt_Product.Rows.Count; i++)
            {
                Dt_Items.Rows.Add(Count, Dt_Product.Rows[i]["JobNo"].ToString(), Dt_Product.Rows[i]["Description"].ToString(), Dt_Product.Rows[i]["Hsn"].ToString(), Dt_Product.Rows[i]["Rate"].ToString(), Dt_Product.Rows[i]["Unit"].ToString(), Dt_Product.Rows[i]["Quntity"].ToString(), Dt_Product.Rows[i]["TaxPercentage"].ToString(), Dt_Product.Rows[i]["DiscountPercentage"].ToString(), Dt_Product.Rows[i]["Total"].ToString());
                Count = Count + 1;
            }
        }

        gvPurchaseRecord.DataSource = Dt_Items;
        gvPurchaseRecord.DataBind();

        SqlDataAdapter Sda = new SqlDataAdapter("SELECT * FROM InvoiceMail WHERE InvoiceNo='" + txt_InvoiceNo.Text + "'", con);
        DataTable Sdt = new DataTable();
        Sda.Fill(Sdt);
        Grd_MAIL.DataSource = Sdt;
        Grd_MAIL.DataBind();
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

    //protected void GenerateInvoice()
    //{
    //    // Get the current year
    //    int currentYear = DateTime.Now.Year;
    //    int lastTwoDigitsOfYear = currentYear % 100;

    //    // Construct the SQL query to get the maximum invoice ID for the current year
    //    string query = $"SELECT MAX([Id]) AS maxid FROM [tbl_Invoice_both_hdr] WHERE YEAR([InvoiceDate]) IN ({currentYear - 1}, {currentYear})";

    //    // Use parameterized query to prevent SQL injection
    //    using (SqlDataAdapter ad = new SqlDataAdapter(query, con))
    //    {
    //        DataTable dt = new DataTable();
    //        ad.Fill(dt);

    //        if (dt.Rows.Count > 0)
    //        {
    //            int maxid = dt.Rows[0]["maxid"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[0]["maxid"]);

    //            if (maxid > 0)
    //            {
    //                // If records found for the current or previous year, use the maximum ID + 1
    //                txt_InvoiceNo.Text = $"{currentYear - 1}-{lastTwoDigitsOfYear}/{maxid + 1:D4}";
    //            }
    //            else
    //            {
    //                // If no records found for the current or previous year, start with 1
    //                txt_InvoiceNo.Text = $"{currentYear - 1}-{lastTwoDigitsOfYear}/1";
    //            }
    //        }
    //        else
    //        {
    //            // If no records found for the current or previous year, start with 1
    //            txt_InvoiceNo.Text = $"{currentYear - 1}-{lastTwoDigitsOfYear}/1";
    //        }
    //    }
    //}

    protected void GenerateInvoice()
    {
        // Construct the SQL query to get the maximum invoice number
        string query = "SELECT MAX(CAST([InvoiceNo] AS INT)) AS maxid FROM [tbl_Invoice_both_hdr]";

        // Use parameterized query to prevent SQL injection
        using (SqlDataAdapter ad = new SqlDataAdapter(query, con))
        {
            DataTable dt = new DataTable();
            ad.Fill(dt);

            // Check if there is any record in the database
            if (dt.Rows.Count > 0)
            {
                int maxid = dt.Rows[0]["maxid"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[0]["maxid"]);

                // If a maxid was found, increment it, else start from 1
                if (maxid > 0)
                {
                    txt_InvoiceNo.Text = $"{(maxid + 1):D4}";  // Increment the invoice number and format it as 4 digits
                }
                else
                {
                    // If no records found, start with 0001
                    txt_InvoiceNo.Text = "0231";
                }
            }
            else
            {
                // If no records found, start with 0001
                txt_InvoiceNo.Text = "0001";
            }
        }
    }

    //protected void GenerateInvoice()
    //{
    //    // Get the current year
    //    int currentYear = DateTime.Now.Year;

    //    // Construct the SQL query to get the maximum invoice ID for the current year
    //    string query = $"SELECT MAX([Id]) AS maxid FROM [tbl_Invoice_both_hdr] WHERE YEAR([InvoiceDate]) = {currentYear}";

    //    // Use parameterized query to prevent SQL injection
    //    using (SqlDataAdapter ad = new SqlDataAdapter(query, con))
    //    {
    //        DataTable dt = new DataTable();
    //        ad.Fill(dt);

    //        if (dt.Rows.Count > 0)
    //        {
    //            int maxid = dt.Rows[0]["maxid"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[0]["maxid"]);
    //            txt_InvoiceNo.Text = $"INV-{currentYear}-{maxid + 1}";
    //        }
    //        else
    //        {
    //            // If no records found for the current year, start with 1
    //            txt_InvoiceNo.Text = $"INV-{currentYear}-1";
    //        }
    //    }
    //}


    //protected void GenerateInvoice()
    //{
    //    SqlDataAdapter ad = new SqlDataAdapter("SELECT max([Id]) as maxid FROM [tbl_Invoice_both_hdr]", con);
    //    DataTable dt = new DataTable();
    //    ad.Fill(dt);
    //    if (dt.Rows.Count > 0)
    //    {
    //        int maxid = dt.Rows[0]["maxid"].ToString() == "" ? 0 : Convert.ToInt32(dt.Rows[0]["maxid"].ToString());
    //        txt_InvoiceNo.Text = "INV-" + (maxid + 1).ToString();
    //    }
    //    else
    //    {
    //        txt_InvoiceNo.Text = string.Empty;
    //    }
    //}

    protected void GenerateChallan()
    {
        SqlDataAdapter ad = new SqlDataAdapter("SELECT max([Id]) as maxid FROM [tbl_Invoice_both_hdr]", con);
        DataTable dt = new DataTable();
        ad.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            int maxid = dt.Rows[0]["maxid"].ToString() == "" ? 0 : Convert.ToInt32(dt.Rows[0]["maxid"].ToString());
            txt_challanNo.Text = "CLN-" + (maxid + 1).ToString();
        }
        else
        {
            txt_challanNo.Text = string.Empty;
        }
    }

    private void Load_Record()
    {
        DataTable Dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter("SELECT tbl_Invoice_both_Dtls.JobNo, tbl_Invoice_both_Dtls.MateName,tbl_Invoice_both_Dtls.PrintDescription,tbl_Invoice_both_hdr.InvoiceNo,tbl_Invoice_both_hdr.InvoiceAgainst,tbl_Invoice_both_hdr.AgainstNo,InvoiceDate,tbl_Invoice_both_hdr.JobNo,tbl_Invoice_both_hdr.CompName,tbl_Invoice_both_hdr.CustName,PoNo,PoDate,ChallanNo,ChallanDate,PayTerm,Delivery,KindAtt,CompanyAddress,CompanyGstNo,CompanyPanNo, ComapyRegType, CompanyStateCode, CustomerShippingAddress, CustomerGstNo,Term_Condition_1,Term_Condition_2,Term_Condition_3,Term_Condition_4,Term_Condition_5,Term_Condition_6, CustomerPanNo, CustomerRegType,CustomerStateCode, CGST, SGST,IGST, AllTotalAmount, GrandTotal, TotalInWord, Description, Hsn, TaxPercentage, Quntity, Unit, Rate, DiscountPercentage, Total FROM tbl_Invoice_both_hdr INNER JOIN tbl_Invoice_both_Dtls ON tbl_Invoice_both_Dtls.InvoiceId = tbl_Invoice_both_hdr.Id WHERE tbl_Invoice_both_hdr.Id='" + hdnID.Value + "'", con);
        da.Fill(Dt);

        if (Dt.Rows.Count > 0)
        {
            ddlagainst.Enabled = false;
            ddlagainstno.Enabled = false;

            btn_save.Text = "Update";
            txt_InvoiceNo.Text = Dt.Rows[0]["InvoiceNo"].ToString();
            ddlagainst.SelectedItem.Text = Dt.Rows[0]["InvoiceAgainst"].ToString();
            Fillddlagainstnumber();
            ddlagainstno.SelectedItem.Text = Dt.Rows[0]["AgainstNo"].ToString();
            txtCompName.Text = Dt.Rows[0]["CompName"].ToString();

            txtCustName.Text = Dt.Rows[0]["CustName"].ToString();
            DateTime ffff1 = Convert.ToDateTime(Dt.Rows[0]["InvoiceDate"].ToString());
            txt_InvoiceDate.Text = ffff1.ToString("yyyy-MM-dd");
            txt_challanNo.Text = Dt.Rows[0]["ChallanNo"].ToString();
            DateTime ffff2 = Convert.ToDateTime(Dt.Rows[0]["ChallanDate"].ToString());
            txt_ChallanDate.Text = ffff1.ToString("yyyy-MM-dd");
            txt_PoNo.Text = Dt.Rows[0]["PoNo"].ToString();
            DateTime ffff3 = Convert.ToDateTime(Dt.Rows[0]["PoDate"].ToString());
            txt_poDate.Text = ffff1.ToString("yyyy-MM-dd");
            txt_CompanyAddress.Text = Dt.Rows[0]["CompanyAddress"].ToString();
            txt_ShipingAdddesss.Text = Dt.Rows[0]["CustomerShippingAddress"].ToString();
            txt_CompanyGSTno.Text = Dt.Rows[0]["CompanyGstNo"].ToString();
            txt_CustomerGstNo.Text = Dt.Rows[0]["CustomerGstNo"].ToString();
            txt_CompanyPanNo.Text = Dt.Rows[0]["CompanyPanNo"].ToString();
            txt_CustomerPanNo.Text = Dt.Rows[0]["CustomerPanNo"].ToString();
            txt_CompanyStateCode.Text = Dt.Rows[0]["CompanyStateCode"].ToString();
            txt_CustomerStateCode.Text = Dt.Rows[0]["CustomerStateCode"].ToString();
            txt_Payterm.Text = Dt.Rows[0]["PayTerm"].ToString();
            txt_KindAtt.Text = Dt.Rows[0]["KindAtt"].ToString();
            //  txt_Delivery.Text = Dt.Rows[0]["Delivery"].ToString();
            lbl_Amount_In_Word.Text = Dt.Rows[0]["TotalInWord"].ToString();
            txt_grand_total.Text = Dt.Rows[0]["GrandTotal"].ToString();
            txt_sgst_amt.Text = Dt.Rows[0]["SGST"].ToString();
            txt_cgst_amt.Text = Dt.Rows[0]["CGST"].ToString();
            txt_igst_amt.Text = Dt.Rows[0]["IGST"].ToString();

            drop_CompanyRegisterType.Text = Dt.Rows[0]["ComapyRegType"].ToString();
            drop_CustomerRagisterType.Text = Dt.Rows[0]["CustomerRegType"].ToString();

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
            if (arrstr.Length > 0)
            {
                txt_term_1.Text = arrstr[0].ToString();
                txt_condition_1.Text = arrstr[1].ToString();
            }

            if (arrstr1.Length > 0)
            {
                txt_term_2.Text = arrstr1[0].ToString();
                txt_condition_2.Text = arrstr1[1].ToString();
            }

            if (arrstr2.Length > 0)
            {
                txt_term_3.Text = arrstr2[0].ToString();
                txt_condition_3.Text = arrstr2[1].ToString();
            }

            if (arrstr3.Length > 0)
            {
                txt_term_4.Text = arrstr3[0].ToString();
                txt_condition_4.Text = arrstr3[1].ToString();
            }
            if (arrstr4.Length > 0)
            {
                txt_term_5.Text = arrstr4[0].ToString();
                txt_condition_5.Text = arrstr4[1].ToString();
            }
            if (arrstr5.Length > 0)
            {
                txt_term_6.Text = arrstr5[0].ToString();
                txt_condition_6.Text = arrstr5[1].ToString();
            }
        }

        // txtmatename.Text = Dt.Rows[0]["MateName"].ToString();

        DataTable Dt_Product = new DataTable();
        SqlDataAdapter daa = new SqlDataAdapter("SELECT MateName,Description,Hsn,Rate,Unit,Quntity,TaxPercentage,DiscountPercentage,Total FROM tbl_Invoice_both_Dtls WHERE tbl_Invoice_both_hdr.Id='" + hdnID.Value + "'", con);
        da.Fill(Dt_Product);

        int Count = 1;
        if (Dt_Product.Rows.Count > 0)
        {
            if (Dt_Items.Columns.Count < 1)
            {
                ShowGrid();
            }

            for (int i = 0; i < Dt_Product.Rows.Count; i++)
            {
                Dt_Items.Rows.Add(Count, Dt_Product.Rows[i]["MateName"].ToString(), Dt_Product.Rows[i]["PrintDescription"].ToString(), Dt_Product.Rows[i]["Hsn"].ToString(), Dt_Product.Rows[i]["Rate"].ToString(), Dt_Product.Rows[i]["Unit"].ToString(), Dt_Product.Rows[i]["Quntity"].ToString(), Dt_Product.Rows[i]["TaxPercentage"].ToString(), Dt_Product.Rows[i]["DiscountPercentage"].ToString(), Dt_Product.Rows[i]["Total"].ToString());
                Count = Count + 1;
            }
        }

        gvPurchaseRecord.DataSource = Dt_Items;
        gvPurchaseRecord.DataBind();

        SqlDataAdapter Sda = new SqlDataAdapter("SELECT * FROM [tblCustomerContactPerson] WHERE CustName='" + txtCustName.Text + "'", con);
        //SqlDataAdapter Sda = new SqlDataAdapter("SELECT * FROM InvoiceMail WHERE InvoiceNo='" + txt_InvoiceNo.Text + "'", con);
        DataTable Sdt = new DataTable();
        Sda.Fill(Sdt);
        Grd_MAIL.DataSource = Sdt;
        Grd_MAIL.DataBind();
        //int count = 1;
        //if (Sdt.Rows.Count > 0)
        //{
        //    if (Dt_Mail.Columns.Count < 1)
        //    {
        //        ShowMAILRecord();
        //    }

        //    for (int i = 0; i < Sdt.Rows.Count; i++)
        //    {
        //        Dt_Mail.Rows.Add(count, Sdt.Rows[i]["EmailID"].ToString());
        //        count = count + 1;
        //    }
        //}
    }

    private void SaveRecord()
    {
        //con.Open();
        int Id;
        string CretedBy = Session["adminname"].ToString();
        DateTime Date = DateTime.Now;
        if (btn_save.Text == "Update")
        {
            SqlCommand Cmd = new SqlCommand("UPDATE tbl_Invoice_both_hdr SET InvoiceNo=@InvoiceNo,InvoiceDate=@InvoiceDate,InvoiceAgainst=@InvoiceAgainst,AgainstNo=@AgainstNo,PoNo=@PoNo,PoDate=@PoDate," +
                "CompName=@CompName,CustName=@CustName,ChallanNo=@ChallanNo,ChallanDate=@ChallanDate,PayTerm=@PayTerm," +
                "KindAtt=@KindAtt,CompanyAddress=@CompanyAddress,CompanyGstNo=@CompanyGstNo,CompanyPanNo=@CompanyPanNo,ComapyRegType=@ComapyRegType," +
                "CompanyStateCode=@CompanyStateCode,CustomerShippingAddress=@CustomerShippingAddress,CustomerGstNo=@CustomerGstNo," +
                "CustomerPanNo=@CustomerPanNo,CustomerRegType=@CustomerRegType,CustomerStateCode=@CustomerStateCode,CGST=@CGST,SGST=@SGST," +
                "AllTotalAmount=@AllTotalAmount,GrandTotal=@GrandTotal,TotalInWord=@TotalInWord,Term_Condition_1=@Term_Condition_1," +
                "Term_Condition_2=@Term_Condition_2,Term_Condition_3=@Term_Condition_3,Term_Condition_4=@Term_Condition_4,Term_Condition_5=@Term_Condition_5,Term_Condition_6=@Term_Condition_6,ServiceType=@ServiceType,Type=@Type,IGST=@IGST,Is_Deleted=@Is_Deleted,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy " +
                "WHERE Id='" + hdnID.Value + "' ", con);  //Remove Delivery Col....

            Cmd.Parameters.AddWithValue("@InvoiceNo", txt_InvoiceNo.Text);
            Cmd.Parameters.AddWithValue("@InvoiceDate", txt_InvoiceDate.Text);
            Cmd.Parameters.AddWithValue("@InvoiceAgainst", ddlagainst.SelectedItem.Text);
            Cmd.Parameters.AddWithValue("@AgainstNo", ddlagainstno.SelectedItem.Text);
            Cmd.Parameters.AddWithValue("@PoNo", txt_PoNo.Text);
            Cmd.Parameters.AddWithValue("@PoDate", txt_poDate.Text);
            Cmd.Parameters.AddWithValue("@CompName", txtCompName.Text);
            Cmd.Parameters.AddWithValue("@CustName", txtCustName.Text);
            Cmd.Parameters.AddWithValue("@ChallanNo", txt_challanNo.Text);
            Cmd.Parameters.AddWithValue("@ChallanDate", txt_ChallanDate.Text);
            Cmd.Parameters.AddWithValue("@PayTerm", txt_Payterm.Text);
            Cmd.Parameters.AddWithValue("@KindAtt", txt_KindAtt.Text);
            Cmd.Parameters.AddWithValue("@CompanyAddress", txt_CompanyAddress.Text);
            Cmd.Parameters.AddWithValue("@CompanyGstNo", txt_CompanyGSTno.Text);
            Cmd.Parameters.AddWithValue("@CompanyPanNo", txt_CompanyPanNo.Text);
            Cmd.Parameters.AddWithValue("@ComapyRegType", drop_CompanyRegisterType.Text);
            Cmd.Parameters.AddWithValue("@CompanyStateCode", txt_CompanyStateCode.Text);
            Cmd.Parameters.AddWithValue("@CustomerShippingAddress", txt_ShipingAdddesss.Text);
            Cmd.Parameters.AddWithValue("@CustomerGstNo", txt_CustomerGstNo.Text);
            Cmd.Parameters.AddWithValue("@CustomerPanNo", txt_CustomerPanNo.Text);
            Cmd.Parameters.AddWithValue("@CustomerRegType", drop_CustomerRagisterType.Text);
            Cmd.Parameters.AddWithValue("@CustomerStateCode", txt_CustomerStateCode.Text);
            Cmd.Parameters.AddWithValue("@CGST", txt_cgst_amt.Text);
            Cmd.Parameters.AddWithValue("@SGST", txt_sgst_amt.Text);
            Cmd.Parameters.AddWithValue("@IGST", txt_igst_amt.Text);
            Cmd.Parameters.AddWithValue("@AllTotalAmount", txt_subtotal.Text);
            Cmd.Parameters.AddWithValue("@GrandTotal", txt_grand_total.Text);
            Cmd.Parameters.AddWithValue("@TotalInWord", lbl_Amount_In_Word.Text);
            Cmd.Parameters.AddWithValue("@Term_Condition_1", txt_term_1.Text + "-" + txt_condition_1.Text);
            Cmd.Parameters.AddWithValue("@Term_Condition_2", txt_term_2.Text + "-" + txt_condition_2.Text);
            Cmd.Parameters.AddWithValue("@Term_Condition_3", txt_term_3.Text + "-" + txt_condition_3.Text);
            Cmd.Parameters.AddWithValue("@Term_Condition_4", txt_term_4.Text + "-" + txt_condition_4.Text);
            Cmd.Parameters.AddWithValue("@Term_Condition_5", txt_term_5.Text + "-" + txt_condition_5.Text);
            Cmd.Parameters.AddWithValue("@Term_Condition_6", txt_term_6.Text + "-" + txt_condition_6.Text);
            Cmd.Parameters.AddWithValue("@ServiceType", ddlservicetype.SelectedItem.Text);
            Cmd.Parameters.AddWithValue("@Type", ddltype.SelectedItem.Text);
            Cmd.Parameters.AddWithValue("@Is_Deleted", '0');
            //Cmd.Parameters.AddWithValue("@status", txtstatus.Text);
            Cmd.Parameters.AddWithValue("@UpdatedBy", CretedBy);
            Cmd.Parameters.AddWithValue("@UpdatedOn", DateTime.Now);
            con.Open();
            Cmd.ExecuteNonQuery();
           

            DataTable Dt = new DataTable();
            SqlDataAdapter daa = new SqlDataAdapter("SELECT JobNo,Description,Hsn,TaxPercentage,Quntity,Unit,Rate," +
                "DiscountPercentage,Total FROM tbl_Invoice_both_Dtls WHERE Id='" + hdnID.Value + "'", con);
            daa.Fill(Dt);

            SqlCommand CmdDelete = new SqlCommand("DELETE FROM tbl_Invoice_both_Dtls WHERE InvoiceId=@InvoiceId", con);
            CmdDelete.Parameters.AddWithValue("@InvoiceId", hdnID.Value);

            CmdDelete.ExecuteNonQuery();

            foreach (GridViewRow g1 in gvPurchaseRecord.Rows)
            {
                //string JobNo = (g1.FindControl("txt_Jobno_grd") as Label).Text;
                //string Discription = (g1.FindControl("lblcomp") as Label).Text;
                string MateName = (g1.FindControl("lblproduct") as Label).Text;
                string HSN = (g1.FindControl("lblHsn") as Label).Text;
                string TAX = (g1.FindControl("lbl_tax_grd") as Label).Text;
                string Quntity = (g1.FindControl("lbl_quntity_grd") as Label).Text;
                string unit = (g1.FindControl("lblUnit") as Label).Text;
                string Rate = (g1.FindControl("lbl_rate_grd") as Label).Text;
                string Discount = (g1.FindControl("lbl_discount_grd") as Label).Text;
                string Total_Amount = (g1.FindControl("lbl_total_amount_grd") as Label).Text;
                string PrintDescription = (g1.FindControl("LblPrintdescription") as Label).Text;
                SqlCommand Cmd2 = new SqlCommand("INSERT INTO tbl_Invoice_both_Dtls (InvoiceId,Hsn,TaxPercentage,Quntity,Unit,Rate,DiscountPercentage, MateName, PrintDescription,Total) " +
                    "VALUES ('" + hdnID.Value + "','" + HSN + "','" + TAX + "','" + Quntity + "'," +
                    "'" + unit + "','" + Rate + "','" + Discount + "',  '" + MateName + "',  '" + PrintDescription + "','" + Total_Amount + "')", con);

                Cmd2.ExecuteNonQuery();
            }
            con.Close();
            SqlDataAdapter Sda = new SqlDataAdapter("SELECT * FROM InvoiceMail WHERE InvoiceNo='" + txt_InvoiceNo.Text + "'", con);
            DataTable DTMAIL = new DataTable();
            Sda.Fill(DTMAIL);

            foreach (GridViewRow g1 in Grd_MAIL.Rows)
            {
                //con.Close();
                string MAIL = (g1.FindControl("lblmultMail") as Label).Text;
                string lbldesignation = (g1.FindControl("lbldesignation") as Label).Text;
                bool chkmail = (g1.FindControl("chkmail") as CheckBox).Checked;

                SqlCommand cmdtable = new SqlCommand("UPDATE InvoiceMail SET Email=@Email, UpdatedBy=@UpdatedBy, UpdatedOn=@UpdatedOn,InvoiceId=@InvoiceId,chkEmail=@chkEmail,designation=@designation WHERE InvoiceNo=@InvoiceNo AND Email=@Email", con);
                cmdtable.Parameters.AddWithValue("@InvoiceNo", txt_InvoiceNo.Text);
                cmdtable.Parameters.AddWithValue("@Email", MAIL);
                cmdtable.Parameters.AddWithValue("@chkEmail", chkmail);
                cmdtable.Parameters.AddWithValue("@UpdatedBy", CretedBy);
                cmdtable.Parameters.Add("@InvoiceId", hdnID.Value);
                cmdtable.Parameters.Add("@designation", lbldesignation);
                cmdtable.Parameters.AddWithValue("@UpdatedOn", Date);
                con.Open();
                cmdtable.ExecuteNonQuery();
                con.Close();   
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Data Updated Sucessfully.');window.location ='/Admin/TaxInvoiceList_Sales.aspx';", true);
            // ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data saved Sucessfully');window.location ='/Admin/TaxInvoiceList_Sales.aspx';", true);
            //  ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Updated Sucessfully')", true);
        }
        else
        {
            //string CretedBy = "adminname";
            try
            {
                //con.Open();


                SqlCommand Cmd = new SqlCommand("SP_Invoice_both", con);
                Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                Cmd.Parameters.AddWithValue("@InvoiceNo", txt_InvoiceNo.Text);
                Cmd.Parameters.AddWithValue("@InvoiceDate", txt_InvoiceDate.Text);
                Cmd.Parameters.AddWithValue("@InvoiceAgainst", ddlagainst.SelectedItem.Text);
                if (ddlagainst.Text == "Direct")
                {
                    Cmd.Parameters.AddWithValue("@AgainstNo", "Direct");
                }
                if (ddlagainst.Text == "Order")
                {
                   // Cmd.Parameters.AddWithValue("@AgainstNo", ddlagainstno.SelectedItem.Text);
                }
                Cmd.Parameters.AddWithValue("@PoNo", txt_PoNo.Text);
                Cmd.Parameters.AddWithValue("@PoDate", txt_poDate.Text);
                Cmd.Parameters.AddWithValue("@CompName", txtCompName.Text);
                Cmd.Parameters.AddWithValue("@CustName", txtCustName.Text);
                Cmd.Parameters.AddWithValue("@ChallanNo", txt_challanNo.Text);
                Cmd.Parameters.AddWithValue("@ChallanDate", txt_ChallanDate.Text);
                Cmd.Parameters.AddWithValue("@PayTerm", txt_Payterm.Text);
                Cmd.Parameters.AddWithValue("@KindAtt", txt_KindAtt.Text);
                Cmd.Parameters.AddWithValue("@CompanyAddress", txt_CompanyAddress.Text);
                Cmd.Parameters.AddWithValue("@CompanyGstNo", txt_CompanyGSTno.Text);
                Cmd.Parameters.AddWithValue("@CompanyPanNo", txt_CompanyPanNo.Text);
                Cmd.Parameters.AddWithValue("@ComapyRegType", drop_CompanyRegisterType.Text);
                Cmd.Parameters.AddWithValue("@CompanyStateCode", txt_CompanyStateCode.Text);
                Cmd.Parameters.AddWithValue("@CustomerShippingAddress", txt_ShipingAdddesss.Text);
                Cmd.Parameters.AddWithValue("@CustomerGstNo", txt_CustomerGstNo.Text);
                Cmd.Parameters.AddWithValue("@CustomerPanNo", txt_CustomerPanNo.Text);
                Cmd.Parameters.AddWithValue("@CustomerRegType", drop_CustomerRagisterType.Text);
                Cmd.Parameters.AddWithValue("@CustomerStateCode", txt_CustomerStateCode.Text);
                Cmd.Parameters.AddWithValue("@CGST", txt_cgst_amt.Text);
                Cmd.Parameters.AddWithValue("@SGST", txt_sgst_amt.Text);
                Cmd.Parameters.AddWithValue("@IGST", txt_igst_amt.Text);
                //  Cmd.Parameters.AddWithValue("@InvoiceId", txt_sgst_amt.Text);
                Cmd.Parameters.AddWithValue("@AllTotalAmount", txt_subtotal.Text);
                Cmd.Parameters.AddWithValue("@GrandTotal", txt_grand_total.Text);
                Cmd.Parameters.AddWithValue("@TotalInWord", lbl_Amount_In_Word.Text);
                Cmd.Parameters.AddWithValue("@Term_Condition_1", txt_term_1.Text + "-" + txt_condition_1.Text);
                Cmd.Parameters.AddWithValue("@Term_Condition_2", txt_term_2.Text + "-" + txt_condition_2.Text);
                Cmd.Parameters.AddWithValue("@Term_Condition_3", txt_term_3.Text + "-" + txt_condition_3.Text);
                Cmd.Parameters.AddWithValue("@Term_Condition_4", txt_term_4.Text + "-" + txt_condition_4.Text);
                Cmd.Parameters.AddWithValue("@Term_Condition_5", txt_term_4.Text + "-" + txt_condition_5.Text);
                Cmd.Parameters.AddWithValue("@Term_Condition_6", txt_term_4.Text + "-" + txt_condition_6.Text);
                Cmd.Parameters.AddWithValue("@ServiceType", ddlservicetype.SelectedItem.Text);
                Cmd.Parameters.AddWithValue("@Type", ddltype.SelectedItem.Text);
                Cmd.Parameters.AddWithValue("@Is_Deleted", '0');
                Cmd.Parameters.AddWithValue("@CreatedOn", DateTime.Now);
                Cmd.Parameters.AddWithValue("@CreatedBy", CretedBy);
                //Cmd.Parameters.AddWithValue("@JobNo", );
                // Cmd.Parameters.AddWithValue("@status", txtstatus.Text);
                Cmd.Parameters.Add("@InvoiceId", SqlDbType.Int).Direction = ParameterDirection.Output;
                con.Open();
                Cmd.ExecuteNonQuery();
                con.Close();

                // New Code Shubham Patil
                int id = Convert.ToInt32(textquotationid.Value);
                con.Open();
                SqlCommand Cmds = new SqlCommand("UPDATE CustomerPO_Hdr_Both SET Status='Completed',JobNoCount='" + txtcountst.Text + "' WHERE ID ='" + id + "'", con);
                Cmds.ExecuteNonQuery();
                con.Close();
                // New Code End

                Id = Convert.ToInt32(Cmd.Parameters["@InvoiceId"].Value);

                foreach (GridViewRow G1 in gvPurchaseRecord.Rows)
                {
                    //string JobNo = (G1.FindControl("txt_Jobno_grd") as Label).Text;
                    //string Discription = (G1.FindControl("LblPrintdescription") as Label).Text;
                    string HSN = (G1.FindControl("lblHsn") as Label).Text;
                    string Tax = (G1.FindControl("lbl_tax_grd") as Label).Text;
                    string Quntity = (G1.FindControl("lbl_quntity_grd") as Label).Text;
                    string Unit = (G1.FindControl("lblUnit") as Label).Text;
                    string Rate = (G1.FindControl("lbl_rate_grd") as Label).Text;
                    string Discount = (G1.FindControl("lbl_discount_grd") as Label).Text;
                    string Total_Amount = (G1.FindControl("lbl_total_amount_grd") as Label).Text;
                    string InvoiceNo = txt_InvoiceNo.Text;
                    string MateName = (G1.FindControl("lblproduct") as Label).Text;
                    string PrintDescription = (G1.FindControl("LblPrintdescription") as Label).Text;
                    SqlCommand Cmd1 = new SqlCommand("INSERT INTO tbl_Invoice_both_Dtls (InvoiceId,Hsn,TaxPercentage,Quntity,Unit,Rate,DiscountPercentage,Total,MateName,PrintDescription ,InvoiceNo) " +
                         //"VALUES ('" + Id + "','" + JobNo + "','" + Discription + "','" + HSN + "','" + Tax + "','" + Quntity + "','" + Unit + "','" + Rate + "','" + Discount + "','" + Total_Amount + "' )", con);
                         "VALUES ('" + Id + "','" + HSN + "','" + Tax + "','" + Quntity + "','" + Unit + "','" + Rate + "','" + Discount + "','" + Total_Amount + "' ,'" + MateName + "','" + PrintDescription + "' ,'" + InvoiceNo + "' )", con);
                    con.Open();
                    Cmd1.ExecuteNonQuery();
                    con.Close();
                }

                foreach (GridViewRow G2 in grd_getDTLS.Rows)
                {
                    //string JobNo_GET = (G2.FindControl("txt_Jobno_grdd") as Label).Text;
                    // string Discription_GET = (G2.FindControl("txt_discription_GET") as Label).Text; 
                    string HSN_GET = (G2.FindControl("txt_hsn_GET") as Label).Text;
                    string Tax_GET = (G2.FindControl("lbl_tax_GET") as Label).Text;
                    string Quntity_GET = (G2.FindControl("lbl_quntity_GET") as Label).Text;
                    string Unit_GET = (G2.FindControl("txt_unit_GET") as Label).Text;
                    string Rate_GET = (G2.FindControl("lbl_rate_GET") as Label).Text;
                    string Discount_GET = (G2.FindControl("lbl_discount_GET") as Label).Text;
                    string Total_Amount_GET = (G2.FindControl("lbl_total_amount_GET") as Label).Text;
                    string MateName = (G2.FindControl("lblproduct") as Label).Text;
                    string PrintDescription = (G2.FindControl("Lblprintdescription_grd") as Label).Text;
                    string InvoiceNo = txt_InvoiceNo.Text;
                    SqlCommand Cmd1 = new SqlCommand("INSERT INTO tbl_Invoice_both_Dtls (InvoiceId,Hsn,TaxPercentage,Quntity,Unit,Rate,DiscountPercentage,Total, MateName, PrintDescription, InvoiceNo) " +
                    //"VALUES ('" + Id + "','" + JobNo_GET + "','" + Discription_GET + "','" + HSN_GET + "','" + Tax_GET + "','" + Quntity_GET + "','" + Unit_GET + "','" + Rate_GET + "','" + Discount_GET + "','" + Total_Amount_GET + "')", con);
                    "VALUES ('" + Id + "','" + HSN_GET + "','" + Tax_GET + "','" + Quntity_GET + "','" + Unit_GET + "','" + Rate_GET + "','" + Discount_GET + "','" + Total_Amount_GET + "'  ,'" + MateName + "', '" + PrintDescription + "','" + InvoiceNo + "' )", con);
                    con.Open();
                    Cmd1.ExecuteNonQuery();
                    con.Close();
                }

                foreach (GridViewRow g1 in Grd_MAIL.Rows)
                {
                    string MAIL = (g1.FindControl("lblmultMail") as Label).Text;
                    bool chkmail = (g1.FindControl("chkmail") as CheckBox).Checked;
                    string lbldesignation = (g1.FindControl("lbldesignation") as Label).Text;
                    SqlCommand cmdtable = new SqlCommand("insert into InvoiceMail(InvoiceNo,Email,CreatedBy,CreatedOn,InvoiceId,chkEmail,designation) values(@InvoiceNo,@Email,@CreatedBy,@CreatedOn,@InvoiceId,@chkEmail,@designation)", con);
                    cmdtable.Parameters.AddWithValue("@InvoiceNo", txt_InvoiceNo.Text);
                    cmdtable.Parameters.AddWithValue("@Email", MAIL);
                    cmdtable.Parameters.AddWithValue("@chkEmail", chkmail);
                    cmdtable.Parameters.AddWithValue("@CreatedBy", CretedBy);
                    cmdtable.Parameters.Add("@InvoiceId", Id);
                    cmdtable.Parameters.Add("@designation", lbldesignation);
                    cmdtable.Parameters.AddWithValue("@CreatedOn", Date);
                    con.Open();
                    cmdtable.ExecuteNonQuery();
                    con.Close();
                }

                //ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data saved Sucessfully');window.location ='/Admin/TaxInvoiceList_Sales.aspx';", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Data saved Sucessfully.');window.location ='/Admin/TaxInvoiceList_Sales.aspx';", true);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    private void TableCalulation()
    {
        decimal Qty;
        if (string.IsNullOrEmpty(txt_quntity.Text))
        {
            Qty = 0;
        }
        else
        {
            var totalamt = Convert.ToDecimal(txt_quntity.Text.Trim()) * Convert.ToDecimal(txt_rate.Text.Trim());
            txt_total_amount.Text = totalamt.ToString();

        }
        //decimal TaxAmt;
        //if (string.IsNullOrEmpty(txt_tax.Text))
        //{
        //    TaxAmt = 0;
        //}
        //else
        //{

        //    decimal val1 = Convert.ToDecimal(txt_total_amount.Text);
        //    decimal val2 = Convert.ToDecimal(txt_tax.Text);

        //    TaxAmt = (val1 * val2 / 100);
        //}

        //var TotalWithTax = Convert.ToDecimal(txt_total_amount.Text) + (TaxAmt);
        //txt_total_amount.Text = TotalWithTax.ToString();

        decimal DiscountAmt;
        if (string.IsNullOrEmpty(txt_discount.Text))
        {
            DiscountAmt = 0;
        }
        else
        {
            DiscountAmt = Convert.ToDecimal(txt_total_amount.Text) * Convert.ToDecimal(txt_discount.Text) / 100;
        }
        var GrossAmt = Convert.ToDecimal(txt_total_amount.Text) - (DiscountAmt);
        txt_total_amount.Text = GrossAmt.ToString("##.00");
    }

    private void ShowGrid()
    {
        ViewState["RowNo"] = (int)ViewState["RowNo"] + 1;
        DataTable Dt = (DataTable)ViewState["Invoice"];

        Dt.Rows.Add(ViewState["RowNo"], txt_hsn.Text, txt_rate.Text, txt_unit.Text, txt_quntity.Text, txt_tax.Text, txt_discount.Text, txt_total_amount.Text);
        ViewState["Invoice"] = Dt;

        //txt_jobno.Text = string.Empty;

        txt_hsn.Text = string.Empty;
        txt_rate.Text = string.Empty;
        txt_unit.Text = string.Empty;
        txt_quntity.Text = string.Empty;
        txt_tax.Text = string.Empty;
        txt_discount.Text = string.Empty;
        txt_total_amount.Text = string.Empty;

        gvPurchaseRecord.DataSource = Dt;
        gvPurchaseRecord.DataBind();
    }

    private void GriCalculation(GridViewRow row)
    {
        TextBox Tax = (TextBox)row.FindControl("txt_tax_grd");
        TextBox Quntity = (TextBox)row.FindControl("txt_quntity_grd");
        TextBox Rate = (TextBox)row.FindControl("txt_rate_grd");
        TextBox Discount = (TextBox)row.FindControl("txt_discount_grd");
        TextBox TotalAmount = (TextBox)row.FindControl("txt_total_amount_grd");


        decimal Qty;
        if (string.IsNullOrEmpty(Quntity.Text))
        {
            Qty = 0;
        }
        else
        {
            var totalamt = Convert.ToDecimal(Quntity.Text.Trim()) * Convert.ToDecimal(Rate.Text.Trim());
            TotalAmount.Text = totalamt.ToString();
        }
        decimal DiscountAmt;
        if (string.IsNullOrEmpty(Discount.Text))
        {
            DiscountAmt = 0;
        }
        else
        {
            DiscountAmt = Convert.ToDecimal(TotalAmount.Text) * Convert.ToDecimal(Discount.Text) / 100;
        }
        var GrossAmt = Convert.ToDecimal(TotalAmount.Text) - (DiscountAmt);
        TotalAmount.Text = GrossAmt.ToString("##.00");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Success", "scrollToElement();", true);
    }

    protected void txt_rate_TextChanged(object sender, EventArgs e)
    {
        if (txt_quntity.Text == "")
        {
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Please fill Quntity !!!');", true);
            txt_tax.Text = "0";
            txt_discount.Text = "0";
            txt_tax.Text = "18";
        }
        else
        {
            TableCalulation();
        }
    }

    protected void txt_quntity_TextChanged(object sender, EventArgs e)
    {
        if (txt_quntity.Text == "" || txt_rate.Text == "")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Please fill Rate  !!!');", true);
        }
        else
        {
            TableCalulation();

        }
    }

    protected void txt_tax_TextChanged(object sender, EventArgs e)
    {
        if (txt_quntity.Text == "")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Please fill Quntity !!!');", true);

        }
        else
        {
            TableCalulation();
        }
    }

    protected void txt_discount_TextChanged(object sender, EventArgs e)
    {
        TableCalulation();
    }

    protected void btn_add_more_Click(object sender, EventArgs e)
    {
        if (txt_hsn.Text == "" || txt_rate.Text == "" || txt_quntity.Text == "")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Please fill Component Information  !!!');", true);
            //txt_quntity.Focus();
        }
        else
        {
            //ShowGrid();
            Show_Grid1();
        }
        //if (hdnPoProductTot.Value != "")
        //{
        //    decimal totaltax = 0, totalgrdtax = 0;
        //    decimal Total11177 = 0, Qty = 0, rate111111 = 0, totalrateqty = 0;
        //    foreach (GridViewRow g1 in gvPurchaseRecord.Rows)
        //    {
        //        string lbltotaltax = (g1.FindControl("lbl_tax_grd") as Label).Text;
        //        string lblrate = (g1.FindControl("lbl_rate_grd") as Label).Text;
        //        string lblqty = (g1.FindControl("lbl_quntity_grd") as Label).Text;
        //        totaltax = Convert.ToDecimal(lbltotaltax);

        //        rate111111 = Convert.ToDecimal(lblrate);
        //        Qty = Convert.ToDecimal(lblqty);
        //        Total11177 = ((Qty * rate111111) * totaltax) / 100;
        //        totalrateqty += Total11177;
        //    }
        //    totalgrdtax = totalrateqty + Convert.ToDecimal(taxhidden.Value);
        //    if (txt_CompanyStateCode.Text == "27 MAHARASHTRA")
        //    {
        //        txt_cgst_amt.Text = Convert.ToDecimal(totalgrdtax / 2).ToString("##.00");
        //        txt_sgst_amt.Text = Convert.ToDecimal(totalgrdtax / 2).ToString("##.00");
        //        txt_igst_amt.Text = Convert.ToDecimal(00).ToString("##.00");
        //    }
        //    else
        //    {
        //        txt_cgst_amt.Text = Convert.ToDecimal(00).ToString("##.00");
        //        txt_sgst_amt.Text = Convert.ToDecimal(00).ToString("##.00");
        //        txt_igst_amt.Text = Convert.ToDecimal(totalgrdtax).ToString("##.00");
        //    }
        //    //decimal taxtotal = 0;
        //    //decimal totaltax1 = 0;
        //    //foreach (GridViewRow g1 in gvPurchaseRecord.Rows)
        //    //{
        //    //    string lbltotaltax = (g1.FindControl("lbl_tax_grd") as Label).Text;
        //    //    totaltax1 += Convert.ToDecimal(lbltotaltax);
        //    //}
        //    //taxtotal = totaltax1 + Convert.ToDecimal(taxhidden.Value);
        //    //txt_cgst_amt.Text = (taxtotal / 2).ToString();
        //    //txt_sgst_amt.Text = (taxtotal / 2).ToString();

        //}
        //else
        //{
        //    decimal totaltax = 0;
        //    decimal Total11177 = 0, Qty = 0, rate111111 = 0, totalrateqty = 0;
        //    foreach (GridViewRow g1 in gvPurchaseRecord.Rows)
        //    {
        //        string lbltotaltax = (g1.FindControl("lbl_tax_grd") as Label).Text;
        //        string lblrate = (g1.FindControl("lbl_rate_grd") as Label).Text;
        //        string lblqty = (g1.FindControl("lbl_quntity_grd") as Label).Text;
        //        totaltax = Convert.ToDecimal(lbltotaltax);

        //        rate111111 = Convert.ToDecimal(lblrate);
        //        Qty = Convert.ToDecimal(lblqty);
        //        Total11177 = ((Qty * rate111111) * totaltax) / 100;
        //        totalrateqty += Total11177;
        //    }
        //    if (txt_CompanyStateCode.Text == "27 MAHARASHTRA")
        //    {
        //        txt_cgst_amt.Text = Convert.ToDecimal(totalrateqty / 2).ToString("##.00");
        //        txt_sgst_amt.Text = Convert.ToDecimal(totalrateqty / 2).ToString("##.00");
        //        txt_igst_amt.Text = Convert.ToDecimal(00).ToString("##.00");
        //    }
        //    else
        //    {
        //        txt_cgst_amt.Text = Convert.ToDecimal(00).ToString("##.00");
        //        txt_sgst_amt.Text = Convert.ToDecimal(00).ToString("##.00");
        //        txt_igst_amt.Text = Convert.ToDecimal(totalrateqty).ToString("##.00");
        //    }
        //}
    }

    //---new code s

    public string ConvertNumbertoWords(int number)
    {
        if (number == 0)
            return "Zero";
        if (number < 0)
            return "Minus " + ConvertNumbertoWords(Math.Abs(number));
        string words = "";

        if ((number / 10000000) > 0)
        {
            words += ConvertNumbertoWords(number / 10000000) + " Crore ";
            number %= 10000000;
        }
        if ((number / 100000) > 0)
        {
            words += ConvertNumbertoWords(number / 100000) + " Lakh ";
            number %= 100000;
        }
        if ((number / 1000) > 0)
        {
            words += ConvertNumbertoWords(number / 1000) + " Thousand ";
            number %= 1000;
        }
        if ((number / 100) > 0)
        {
            words += ConvertNumbertoWords(number / 100) + " Hundred ";
            number %= 100;
        }
        if (number > 0)
        {
            if (words != "")
                words += "And ";
            var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
            var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

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

    private static String ones(String Number)
    {
        int _Number = Convert.ToInt32(Number);
        String name = "";
        switch (_Number)
        {
            case 1:
                name = "One";
                break;
            case 2:
                name = "Two";
                break;
            case 3:
                name = "Three";
                break;
            case 4:
                name = "Four";
                break;
            case 5:
                name = "Five";
                break;
            case 6:
                name = "Six";
                break;
            case 7:
                name = "Seven";
                break;
            case 8:
                name = "Eight";
                break;
            case 9:
                name = "Nine";
                break;
        }
        return name;
    }

    private static String tens(String Number)
    {
        int _Number = Convert.ToInt32(Number);
        String name = null;
        switch (_Number)
        {
            case 10:
                name = "Ten";
                break;
            case 11:
                name = "Eleven";
                break;
            case 12:
                name = "Twelve";
                break;
            case 13:
                name = "Thirteen";
                break;
            case 14:
                name = "Fourteen";
                break;
            case 15:
                name = "Fifteen";
                break;
            case 16:
                name = "Sixteen";
                break;
            case 17:
                name = "Seventeen";
                break;
            case 18:
                name = "Eighteen";
                break;
            case 19:
                name = "Nineteen";
                break;
            case 20:
                name = "Twenty";
                break;
            case 30:
                name = "Thirty";
                break;
            case 40:
                name = "Forty";
                break;
            case 50:
                name = "Fifty";
                break;
            case 60:
                name = "Sixty";
                break;
            case 70:
                name = "Seventy";
                break;
            case 80:
                name = "Eighty";
                break;
            case 90:
                name = "Ninety";
                break;
            default:
                if (_Number > 0)
                {
                    name = tens(Number.Substring(0, 1) + "0") + " " + ones(Number.Substring(1));
                }
                break;
        }
        return name;
    }

    private static String ConvertWholeNumber(String Number)
    {
        string word = "";
        try
        {
            bool beginsZero = false;
            bool isDone = false;
            double dblAmt = (Convert.ToDouble(Number));

            if (dblAmt > 0)
            {
                beginsZero = Number.StartsWith("0");
                int numDigits = Number.Length;
                int pos = 0;
                String place = "";

                switch (numDigits)
                {
                    case 1:
                        word = ones(Number);
                        isDone = true;
                        break;
                    case 2:
                        word = tens(Number);
                        isDone = true;
                        break;
                    case 3:
                        pos = (numDigits % 3) + 1;
                        place = " Hundred ";
                        break;
                    case 4:
                    case 5:
                        pos = (numDigits % 4) + 1;
                        place = " Thousand ";
                        break;
                    case 6:
                    case 7:
                        pos = (numDigits % 6) + 1;
                        place = " Lakh ";
                        break;
                    case 8:
                    case 9:
                        pos = (numDigits % 8) + 1;
                        place = " Crore ";
                        break;
                    default:
                        isDone = true;
                        break;
                }

                if (!isDone)
                {
                    if (Number.Substring(0, pos) != "0" && Number.Substring(pos) != "0")
                    {
                        try
                        {
                            word = ConvertWholeNumber(Number.Substring(0, pos)) + place + ConvertWholeNumber(Number.Substring(pos));
                        }
                        catch { }
                    }
                    else
                    {
                        word = ConvertWholeNumber(Number.Substring(0, pos)) + ConvertWholeNumber(Number.Substring(pos));
                    }
                }

                if (word.Trim().Equals(place.Trim())) word = "";
            }
        }
        catch { }
        return word.Trim();
    }

    private static String ConvertToWords(String numb)
    {
        String val = "", wholeNo = numb, points = "", andStr = "", pointStr = "";
        String endStr = "Only";
        try
        {
            int decimalPlace = numb.IndexOf(".");
            if (decimalPlace > 0)
            {
                wholeNo = numb.Substring(0, decimalPlace);
                points = numb.Substring(decimalPlace + 1);
                if (Convert.ToInt32(points) > 0)
                {
                    andStr = "and";
                    endStr = "Paisa " + endStr;
                    pointStr = ConvertDecimals(points);
                }
            }
            val = String.Format("{0} {1}{2} {3}", ConvertWholeNumber(wholeNo).Trim(), andStr, pointStr, endStr);
        }
        catch { }
        return val;
    }

    private static String ConvertDecimals(String number)
    {
        String cd = "", digit = "", engOne = "";
        for (int i = 0; i < number.Length; i++)
        {
            digit = number[i].ToString();
            if (digit.Equals("0"))
            {
                engOne = "Zero";
            }
            else
            {
                engOne = ones(digit);
            }
            cd += " " + engOne;
        }
        return cd;
    }

    protected void gvPurchaseRecord_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvPurchaseRecord.EditIndex = e.NewEditIndex;
        gvPurchaseRecord.DataSource = (DataTable)ViewState["Invoice"];
        gvPurchaseRecord.DataBind();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Success", "scrollToElement();", true);
    }

    private decimal Total;
    decimal Alltotal, totaAmount18 = 0, totaAmount12 = 0, totaAmount28 = 0;
    string lbltotaltax, lblrate, lblqty, Amount;
    protected void gvPurchaseRecord_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            // cast the current state row to a datarowview
            DataRowView row = e.Row.DataItem as DataRowView;
            Total += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TotalAmount"));
            if (hdnPoProductTot.Value != "")
            {
                Alltotal = Convert.ToDecimal(hdnPoProductTot.Value) + Total;
            }
            else
            {
                Alltotal = Total;
            }

            txt_subtotal.Text = Alltotal.ToString("0.00");
            LinkButton lnkedit = e.Row.FindControl("btn_edit") as LinkButton;
            if (lnkedit == null)
            {
                lbltotaltax = (e.Row.FindControl("txt_quntity_grd") as TextBox).Text;
                lblrate = (e.Row.FindControl("txt_rate_grd") as TextBox).Text;
                lblqty = (e.Row.FindControl("txt_quntity_grd") as TextBox).Text;
                Amount = (e.Row.FindControl("txt_total_amount_grd") as TextBox).Text;
            }
            else
            {
                lbltotaltax = (e.Row.FindControl("lbl_tax_grd") as Label).Text;
                lblrate = (e.Row.FindControl("lbl_rate_grd") as Label).Text;
                lblqty = (e.Row.FindControl("lbl_quntity_grd") as Label).Text;
                Amount = (e.Row.FindControl("lbl_total_amount_grd") as Label).Text;
                totaltax = Convert.ToDecimal(lbltotaltax);
            }
            if (lbltotaltax == "12")
            {
                TaxPanel2.Visible = true;
                string Valt1 = hdn12.Value;
                Totaltax12 += Convert.ToDecimal(Amount);
                if (hdn18.Value != "")
                {
                    totaAmount12 = Convert.ToDecimal(Valt1) + Convert.ToDecimal(Totaltax12);
                }
                else
                {
                    totaAmount12 = Convert.ToDecimal(Totaltax12);
                }

                if (txt_CompanyStateCode.Text == "27 MAHARASHTRA")
                {
                    Label.Text = ((Convert.ToDecimal(totaAmount12) * Convert.ToDecimal(lbltotaltax) / 100) / 2).ToString("0.00");
                    TextBox2.Text = ((Convert.ToDecimal(totaAmount12) * Convert.ToDecimal(lbltotaltax) / 100) / 2).ToString("0.00");
                    TextBox3.Text = Convert.ToDecimal(00).ToString("0.00");
                }
                else
                {
                    TextBox3.Text = ((Convert.ToDecimal(totaAmount12) * Convert.ToDecimal(lbltotaltax) / 100)).ToString("0.00");
                    Label.Text = Convert.ToDecimal(00).ToString("0.00");
                    TextBox2.Text = Convert.ToDecimal(00).ToString("0.00");
                }
            }
            if (lbltotaltax == "18")
            {
                TaxPanel1.Visible = true;
                Totaltax18 += Convert.ToDecimal(Amount);
                string Valt2 = hdn18.Value;
                if (hdn18.Value != "")
                {
                    totaAmount18 = Convert.ToDecimal(Valt2) + Convert.ToDecimal(Totaltax18);
                }
                else
                {
                    totaAmount18 = Convert.ToDecimal(Totaltax18);
                }

                if (txt_CompanyStateCode.Text == "27 MAHARASHTRA")
                {
                    txt_cgst_amt.Text = ((Convert.ToDecimal(totaAmount18) * Convert.ToDecimal(lbltotaltax) / 100) / 2).ToString("0.00");
                    txt_sgst_amt.Text = ((Convert.ToDecimal(totaAmount18) * Convert.ToDecimal(lbltotaltax) / 100) / 2).ToString("0.00");
                    txt_igst_amt.Text = Convert.ToDecimal(00).ToString("0.00");
                }
                else
                {
                    txt_igst_amt.Text = ((Convert.ToDecimal(totaAmount18) * Convert.ToDecimal(lbltotaltax) / 100)).ToString("0.00");
                    txt_cgst_amt.Text = Convert.ToDecimal(00).ToString("0.00");
                    txt_sgst_amt.Text = Convert.ToDecimal(00).ToString("0.00");
                }
            }
            if (lbltotaltax == "28")
            {
                TaxPanel3.Visible = true;
                Totaltx28 += Convert.ToDecimal(Amount);
                string Valt3 = hdn28.Value;
                if (hdn18.Value != "")
                {
                    totaAmount28 = Convert.ToDecimal(Valt3) + Convert.ToDecimal(Totaltx28);
                }
                else
                {
                    totaAmount18 = Convert.ToDecimal(Totaltx28);
                }

                if (txt_CompanyStateCode.Text == "27 MAHARASHTRA")
                {
                    TextBox4.Text = ((Convert.ToDecimal(Totaltx28) * Convert.ToDecimal(lbltotaltax) / 100) / 2).ToString("0.00");
                    TextBox5.Text = ((Convert.ToDecimal(Totaltx28) * Convert.ToDecimal(lbltotaltax) / 100) / 2).ToString("0.00");
                    TextBox6.Text = Convert.ToDecimal(00).ToString("0.00");
                }
                else
                {
                    TextBox6.Text = ((Convert.ToDecimal(Totaltx28) * Convert.ToDecimal(lbltotaltax) / 100)).ToString("0.00");
                    TextBox4.Text = Convert.ToDecimal(00).ToString("0.00");
                    TextBox5.Text = Convert.ToDecimal(00).ToString("0.00");
                }
            }

            decimal grd_total;
            if (!string.IsNullOrEmpty(txt_total_amount.Text))
            {
                grd_total = 0;
            }
            else
            {
                decimal val1 = Convert.ToDecimal(txt_subtotal.Text);
                decimal val2 = Convert.ToDecimal(txt_sgst_amt.Text);
                decimal val3 = Convert.ToDecimal(txt_cgst_amt.Text);
                decimal val4 = Convert.ToDecimal(txt_igst_amt.Text);
                decimal val5 = Convert.ToDecimal(Label.Text);
                decimal val6 = Convert.ToDecimal(TextBox2.Text);
                decimal val7 = Convert.ToDecimal(TextBox3.Text);
                decimal val8 = Convert.ToDecimal(TextBox4.Text);
                decimal val9 = Convert.ToDecimal(TextBox5.Text);
                decimal val10 = Convert.ToDecimal(TextBox6.Text);
                grd_total = (val1 + val2 + val3 + val4 + val5 + val6 + val7 + val8 + val9 + val10);


            }
            txt_grand_total.Text = grd_total.ToString("##.00");
            hfTotal.Value = txt_grand_total.Text;

            string isNegative = "";
            try
            {
                string number = hfTotal.Value;
                number = Convert.ToDouble(number).ToString();

                lbl_Amount_In_Word.Text = isNegative + ConvertToWords(number);
            }
            catch (Exception)
            {
                throw;
            }

            //Round Off
            var Totalamtfff = Convert.ToDouble(txt_grand_total.Text);
            var totalgrand = Math.Round(Totalamtfff);
            System.Globalization.CultureInfo info = System.Globalization.CultureInfo.GetCultureInfo("en-IN");
            string FinaleTotalamt = Totalamtfff.ToString("N2", info);
            double GetVal = 0;  // to know rounded value
            GetVal -= Convert.ToDouble(Totalamtfff) - Convert.ToDouble(totalgrand);
            Double roundoff = Math.Round(GetVal, 2);
            txtroundoff.Text = roundoff.ToString();
            txt_grand_total.Text = totalgrand.ToString("##.00");
        }
    }

    protected void gv_update_Click(object sender, EventArgs e)
    {
        GridViewRow row = (sender as LinkButton).NamingContainer as GridViewRow;

        string Tax = ((TextBox)row.FindControl("txt_tax_grd")).Text;
        string Quntity = ((TextBox)row.FindControl("txt_quntity_grd")).Text;
        string Rate = ((TextBox)row.FindControl("txt_rate_grd")).Text;
        string Discount = ((TextBox)row.FindControl("txt_discount_grd")).Text;
        string TotalAmount = ((TextBox)row.FindControl("txt_total_amount_grd")).Text;

        DataTable Dt = ViewState["Invoice"] as DataTable;

        Dt.Rows[row.RowIndex]["Tax"] = Tax;
        Dt.Rows[row.RowIndex]["Quntity"] = Quntity;
        Dt.Rows[row.RowIndex]["Rate"] = Rate;
        Dt.Rows[row.RowIndex]["Discount"] = Discount;
        Dt.Rows[row.RowIndex]["TotalAmount"] = TotalAmount;

        Dt.AcceptChanges();

        ViewState["Invoice"] = Dt;
        gvPurchaseRecord.EditIndex = -1;

        gvPurchaseRecord.DataSource = (DataTable)ViewState["Invoice"];
        gvPurchaseRecord.DataBind();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Success", "scrollToElement();", true);
    }

    protected void txt_rate_grd_TextChanged(object sender, EventArgs e)
    {
        GridViewRow row = (sender as TextBox).NamingContainer as GridViewRow;

        GriCalculation(row);
    }

    protected void txt_quntity_grd_TextChanged(object sender, EventArgs e)
    {
        GridViewRow row = (sender as TextBox).NamingContainer as GridViewRow;

        GriCalculation(row);
    }

    protected void txt_tax_grd_TextChanged(object sender, EventArgs e)
    {
        GridViewRow row = (sender as TextBox).NamingContainer as GridViewRow;

        GriCalculation(row);
    }

    protected void txt_discount_grd_TextChanged(object sender, EventArgs e)
    {
        GridViewRow row = (sender as TextBox).NamingContainer as GridViewRow;

        GriCalculation(row);
    }

    protected void btn_save_Click(object sender, EventArgs e)
    {
        if (btn_save.Text == "Save")
        {
            if (ChkSendQuotation.Checked == true)
            {
                SaveRecord();
                Send_Mail();
            }
            else
            {
                SaveRecord();
            }
        }
        if (btn_save.Text == "Update")
        {
            if (ChkSendQuotation.Checked == true)
            {
                SaveRecord();
                Send_Mail();
                // ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Updated Sucessfully')", true);
            }
            else
            {
                SaveRecord();
            }
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data saved Sucessfully.');window.location ='/Admin/TaxInvoiceList_Sales.aspx';", true);

        }
    }

    protected void Send_Mail()
    {
        string strMessage =

               "<strong>Dear Sir," + "<br/><br/>" +

                      "Greetings from " + "<strong>Endeavour  Automation...!" + "<br/><br/>" +

                       "Please find the attached offer for your reference." + " <br/><br/>" +

                        "Please find the attached offer for your reference. " + "Invoice No. - " + txt_InvoiceNo.Text.Trim() + ".pdf" + "<br/><br/>" +

                       "We hope that we will receive a Purchase Order Soon." + " <br/><br/>" +

                       "Please do not hesitate to contact us for any query or clarification." + " <br/><br/>" +

                      "Thanks & Regards." + "<br/>" +
                      "<strong>ENDEAVOUR AUTOMATION<strong>";

        string pdfname = "TaxInv - " + txt_InvoiceNo.Text.Trim() + "/" + txt_InvoiceDate.Text.Trim() + ".pdf";

        MailMessage message = new MailMessage();
        //MailMessage msgendeaour = new MailMessage();
        //MailMessage msgenaccount = new MailMessage();
        //msgendeaour.To.Add("erp@weblinkservices.net");
        //msgenaccount.To.Add("erp@weblinkservices.net");
        foreach (GridViewRow g1 in Grd_MAIL.Rows)
        {
            string MAIL = (g1.FindControl("lblmultMail") as Label).Text;
            bool chkmail = (g1.FindControl("chkmail") as CheckBox).Checked;
            if (chkmail == true)
            {
                message.To.Add(MAIL);// Email-ID of Receiver  
            }
        }
        //message.To.Add(txt_Mail.Text);// Email-ID of Receiver  
        message.Subject = "Tax Invoice";// Subject of Email  
        message.Body = strMessage;
        //msgendeaour.Subject = "Tax Invoice";// Subject of Email  
        //msgendeaour.Body = strMessage;
        //msgenaccount.Subject = "Tax Invoice";// Subject of Email  
        //msgenaccount.Body = strMessage;

        message.From = new MailAddress("enquiry@weblinkservices.net", "info@endeavours.in");
        //message.From = new System.Net.Mail.MailAddress("info@endeavours.in");// Email-ID of Sender  
        // message.From = new System.Net.Mail.MailAddress("enquiry@weblinkservices.net");// Email-ID of Sender  
        message.IsBodyHtml = true;
        //msgendeaour.From = new System.Net.Mail.MailAddress("enquiry@weblinkservices.net");// Email-ID of Sender  
        //msgendeaour.IsBodyHtml = true;
        //msgenaccount.From = new System.Net.Mail.MailAddress("enquiry@weblinkservices.net");// Email-ID of Sender  
        //msgenaccount.IsBodyHtml = true;

        MemoryStream file = new MemoryStream(PDF("This is pdf file text", Server.MapPath("~/Files/")).ToArray());

        file.Seek(0, SeekOrigin.Begin);
        Attachment data = new Attachment(file, pdfname, "application/pdf");
        ContentDisposition disposition = data.ContentDisposition;
        disposition.CreationDate = System.DateTime.Now;
        disposition.ModificationDate = System.DateTime.Now;
        disposition.DispositionType = DispositionTypeNames.Attachment;
        message.Attachments.Add(data);//Attach the file  
                                      //msgendeaour.Attachments.Add(data);//Attach the file
                                      //msgenaccount.Attachments.Add(data);//Attach the file


        //message.Body = txtmessagebody.Text;
        SmtpClient SmtpMail = new SmtpClient();
        SmtpMail.Host = "smtpout.secureserver.net"; // Name or IP-Address of Host used for SMTP transactions  
        SmtpMail.Port = 587; // Port for sending the mail  
        SmtpMail.Credentials = new System.Net.NetworkCredential("enquiry@weblinkservices.net", "wlspl@123"); // Username/password of network, if apply  
        SmtpMail.DeliveryMethod = SmtpDeliveryMethod.Network;
        SmtpMail.EnableSsl = false;

        SmtpMail.ServicePoint.MaxIdleTime = 0;
        SmtpMail.ServicePoint.SetTcpKeepAlive(true, 2000, 2000);
        message.BodyEncoding = Encoding.Default;
        message.Priority = MailPriority.High;
        SmtpMail.Send(message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Mail Send Successfully !!');", true);
        Response.Write("Email has been sent");
    }

    private MemoryStream PDF(string InvoiceNo, string message)
    {
        MemoryStream pdf = new MemoryStream();

        //foreach (GridViewRow g1 in Grd_MAIL.Rows)
        //{
        //    string MAIL = (g1.FindControl("lblmultMail") as Label).Text;
        //    bool chkmail = (g1.FindControl("chkmail") as CheckBox).Checked;
        //    if (chkmail == true)
        //    {
        //SqlDataAdapter Da = new SqlDataAdapter("SELECT * FROM vw_Taxinvoice_pdf WHERE InvoiceNo='" + txt_InvoiceNo.Text + "' AND Email='" + MAIL + "'   ", con);

        //Changes for Temparay
        SqlDataAdapter Da = new SqlDataAdapter("SELECT * FROM vw_Tax_Invoice_PDF_SALE_Mail As TP Inner join CustomerPO_Dtls_Both As TD on TP.Id= TD.InvoiceId WHERE TP.InvoiceNo='" + txt_InvoiceNo.Text + "' ", con);
        // SqlDataAdapter Da = new SqlDataAdapter("SELECT * FROM vw_Taxinvoice_pdf WHERE InvoiceNo='" + txt_InvoiceNo.Text + "' ", con);
        DataTable Dt = new DataTable();
        Da.Fill(Dt);
        StringWriter sw = new StringWriter();
        StringReader sr = new StringReader(sw.ToString());

        Document doc = new Document(PageSize.A4, 10f, 10f, 55f, 0f);
        PdfWriter pdfWriter = PdfWriter.GetInstance(doc, pdf);

        PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(Server.MapPath("~/Files/") + "TaxInvoice.pdf", FileMode.Create));
        //PdfWriter writer = PdfWriter.GetInstance(doc, Response.OutputStream);
        XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, sr);

        doc.Open();
        //byte[] file;
        //file = System.IO.File.ReadAllBytes(message);
        //iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(file);

        //jpg.ScaleToFit(550F, 200F);
        //doc.Add(jpg);

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

        PdfContentByte cb = pdfWriter.DirectContent;
        cb.Rectangle(17f, 735f, 560f, 60f);
        cb.Stroke();
        // Header 
        cb.BeginText();
        cb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 20);
        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "ENDEAVOUR AUTOMATION", 185, 773, 0);
        cb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 11);
        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Survey No. 27, Nilambkar Nagar, Near Raghunandan Karyalay,", 155, 755, 0);
        cb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 11);
        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Tathawade, Pune-411033", 232, 740, 0);
        cb.EndText();

        PdfContentByte cbb = pdfWriter.DirectContent;
        cbb.Rectangle(17f, 710f, 560f, 25f);
        cbb.Stroke();
        // Header 
        cbb.BeginText();
        cbb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
        cbb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " Mob: 9860502108    Email: endeavour.automations@gmail.com ", 153, 722, 0);
        cbb.EndText();

        PdfContentByte cbbb = pdfWriter.DirectContent;
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

        PdfContentByte cd = pdfWriter.DirectContent;
        cd.Rectangle(17f, 660f, 560f, 25f);
        cd.Stroke();
        // Header 
        cd.BeginText();
        cd.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 17);
        cd.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Tax Invoice", 260, 667, 0);
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
            var CreateDate = DateTime.Now.ToString("yyyy-MM-dd");
            string CustName = Dt.Rows[0]["CustName"].ToString();
            string Invoicen = Dt.Rows[0]["InvoiceNo"].ToString();
            string PoDate = Dt.Rows[0]["PoDate"].ToString().TrimEnd("0:0".ToCharArray());
            string PoNumber = Dt.Rows[0]["PoNo"].ToString();
            string CompanyAddress = Dt.Rows[0]["CompanyAddress"].ToString();
            string Address = Dt.Rows[0]["CustomerShippingAddress"].ToString();
            string InvoiceDate = Dt.Rows[0]["InvoiceDate"].ToString().TrimEnd("0:0".ToCharArray());
            string ChallanNo = Dt.Rows[0]["ChallanNo"].ToString();
            string GSTNo = Dt.Rows[0]["CustomerGstNo"].ToString();
            string StateCode = Dt.Rows[0]["CustomerStateCode"].ToString();
            string KindAtt = Dt.Rows[0]["KindAtt"].ToString();
            string TotalInWord = Dt.Rows[0]["TotalInWord"].ToString();
            string GrandTotal = Dt.Rows[0]["GrandTotal"].ToString();
            string CGST = Dt.Rows[0]["CGST"].ToString();
            string SGST = Dt.Rows[0]["SGST"].ToString();
            string IGST = Dt.Rows[0]["IGST"].ToString();
            string Total = Dt.Rows[0]["AllTotalAmount"].ToString();

            DateTime ffff1 = Convert.ToDateTime(Dt.Rows[0]["InvoiceDate"].ToString());
            string datee = ffff1.ToString("dd-MM-yyyy");

            DateTime ffff2 = Convert.ToDateTime(Dt.Rows[0]["PoDate"].ToString());
            string podate = ffff1.ToString("dd-MM-yyyy");

            table.AddCell(new Phrase("Buyer : ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(CustName, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("Consignee :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(CustName, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("Shipping Address : ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(Address, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("Billing Address : ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(CompanyAddress, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("Invoice Number : ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(Invoicen, FontFactory.GetFont("Arial", 9, Font.BOLD)));

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

            //table.AddCell(new Phrase("Invoice Number : ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            //table.AddCell(new Phrase(Invoicen, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            //table.AddCell(new Phrase("PO Number :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            //table.AddCell(new Phrase(PoNumber, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            //table.AddCell(new Phrase("Address", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            //table.AddCell(new Phrase(Address, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            //table.AddCell(new Phrase("Invoice Date :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            //table.AddCell(new Phrase(InvoiceDate, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            //table.AddCell(new Phrase("Challan No :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            //table.AddCell(new Phrase(ChallanNo, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            //table.AddCell(new Phrase("GST No :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            //table.AddCell(new Phrase(GSTNo, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            //table.AddCell(new Phrase("Created Date :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            //table.AddCell(new Phrase(CreateDate, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            //table.AddCell(new Phrase("Kind Attn. :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            //table.AddCell(new Phrase(KindAtt, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            paragraphTable1.Add(table);
            doc.Add(paragraphTable1);

            Paragraph paragraphTable2 = new Paragraph();
            paragraphTable2.SpacingAfter = 0f;
            table = new PdfPTable(9);
            float[] widths3 = new float[] { 4f, 19f, 10f, 8f, 8f, 8f, 11f, 8f, 12f };
            table.SetWidths(widths3);

            double Ttotal_price = 0;
            //DataTable Dt = Read_Table("SELECT * FROM vw_Quotation_Invoice");

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

                    //// double Ftotal = Convert.ToDouble(dr["Total"].ToString());
                    // string _ftotal = Ftotal.ToString("##.00");
                    // table.AddCell(new Phrase(rowid.ToString(), FontFactory.GetFont("Arial", 9)));
                    // table.AddCell(new Phrase(dr["Description"].ToString(), FontFactory.GetFont("Arial", 9)));
                    // table.AddCell(new Phrase(dr["Hsn"].ToString(), FontFactory.GetFont("Arial", 9)));
                    // table.AddCell(new Phrase(dr["TaxPercentage"].ToString(), FontFactory.GetFont("Arial", 9)));
                    // table.AddCell(new Phrase(dr["Quntity"].ToString(), FontFactory.GetFont("Arial", 9)));
                    // table.AddCell(new Phrase(dr["Unit"].ToString(), FontFactory.GetFont("Arial", 9)));
                    // table.AddCell(new Phrase(dr["Rate"].ToString(), FontFactory.GetFont("Arial", 9)));
                    // table.AddCell(new Phrase(dr["DiscountPercentage"].ToString(), FontFactory.GetFont("Arial", 9)));
                    // table.AddCell(new Phrase(_ftotal, FontFactory.GetFont("Arial", 9)));
                    // rowid++;

                    // //Ttotal_price += Convert.ToDouble(dr["Total"].ToString());

                    // //Ttotal_price += Convert.ToDouble(dr["AllTotalAmount"].ToString());


                    //Temp changes


                    // double Ftotal = Convert.ToDouble(dr["Total"].ToString());
                    double Ftotal = Convert.ToDouble(dr["Total1"].ToString());
                    //double Ftotal = Convert.ToDouble(dr["AllTotalAmount"].ToString());
                    string _ftotal = Ftotal.ToString("##.00");
                    table.AddCell(new Phrase(rowid.ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(dr["PrintDescription"].ToString(), FontFactory.GetFont("Arial", 9)));
                    //table.AddCell(new Phrase(dr["Description1"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(dr["Hsn1"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(dr["TaxPercentage1"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(dr["Quntity1"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(dr["Unit1"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(dr["Rate1"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(dr["DiscountPercentage1"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(_ftotal, FontFactory.GetFont("Arial", 9)));
                    rowid++;

                    //Ttotal_price += Convert.ToDouble(dr["Total"].ToString());
                    Ttotal_price += Convert.ToDouble(dr["Total1"].ToString());
                    //Ttotal_price += Convert.ToDouble(dr["AllTotalAmount"].ToString());
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
            //table.AddCell(new Phrase("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));

            //doc.Add(table);
            if (Dt.Rows.Count >= 10)
            {
                table.AddCell(new Phrase("  \n\n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                //doc.Add(table);
            }
            else if (Dt.Rows.Count >= 7 && Dt.Rows.Count <= 9)
            {
                table.AddCell(new Phrase("  \n\n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));

            }
            else if (Dt.Rows.Count >= 4 && Dt.Rows.Count <= 6)
            {
                table.AddCell(new Phrase("  \n\n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));

            }
            else if (Dt.Rows.Count < 4)
            {

                table.AddCell(new Phrase("  \n\n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));

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

            paragraph.Alignment = Element.ALIGN_RIGHT;

            table.SetWidths(new float[] { 0f, 76f, 12f });
            table.AddCell(paragraph);
            PdfPCell cell = new PdfPCell(new Phrase("Sub Total", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cell);
            PdfPCell cell11 = new PdfPCell(new Phrase(Ttotal_price.ToString("##.00"), FontFactory.GetFont("Arial", 10, Font.BOLD)));
            cell11.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cell11);
            doc.Add(table);
            // add total row end



            if (StateCode == "27 MAHARASHTRA")
            {
                //CGST 9% Row STart
                Paragraph paragraphTable15 = new Paragraph();
                paragraphTable5.SpacingAfter = 0f;
                //paragraphTable15
                //paragraphTable5.SpacingAfter = 10f;

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

                //var Cgst_9 = Convert.ToDecimal(Ttotal_price) * 9 / 100;

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



                //var Sgst_9 = Convert.ToDecimal(Ttotal_price) * 9 / 100;
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



                //var Sgst_9 = Convert.ToDecimal(Ttotal_price) * 9 / 100;
                table.SetWidths(new float[] { 0f, 76f, 12f });
                table.AddCell(paragraph);
                PdfPCell cell22 = new PdfPCell(new Phrase("IGST %", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                cell22.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell22);
                PdfPCell cell33 = new PdfPCell(new Phrase(IGST, FontFactory.GetFont("Arial", 10, Font.BOLD)));
                cell33.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell33);


                doc.Add(table);
                //SGST 9% Row End
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

                //var Grndttl = Convert.ToDecimal(Ttotal_price) + Convert.ToDecimal(Cgst_9) + Convert.ToDecimal(Sgst_9);

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
                PdfPCell cell77 = new PdfPCell(new Phrase(TotalInWord, FontFactory.GetFont("Arial", 10, Font.BOLD)));
                cell77.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell77);

                doc.Add(table);
            //Grand total in word Row End



            /////////new code //////////14 start
            ///  ///////////term And Condition

            string[] items90term = { "Dear Sir/Madam,\b", "We acknowledge with thanks the receipt of your above enquiry and have great pleasure in submitting our lowest quotation,\b", "subject to the conditions printed below.\n" };

            Font font12112term = FontFactory.GetFont("Arial", 8);
            Font font10111term = FontFactory.GetFont("Arial", 8);
            Font fontWithSize = FontFactory.GetFont(FontFactory.HELVETICA, 10);
            // Paragraph paragraph110term = new Paragraph("\bTerms & Conditions:", font12112term);
            Paragraph paragraph110term = new Paragraph("I / we certify that our registration certficate under the GST Act, 2017 is in force on the date on which the supply of goods specified In This Tax Invoiceis made by me / us & the transaction of supply covered by this Tax Invoice had been effected by me / us & it shall be accounted for in the turnover ofsupplies while filling of return &the due tax if any payable on the supplies has been paid or shall be paid.Further certified that the particulars givenabove are true and correct &the amount indicated represents the prices actually charged and that there is no flow additional consideration directly orindirectly from the buyer. Interest @ 18 % p.a.charged on all outstanding more than one month after invoice has been rendered", fontWithSize);

            for (int i = 0; i < items90term.Length; i++)
            {
                paragraph110term.Add(new Phrase(" ", fontWithSize));
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



            doc.Add(table);
            ////////term condition in Database end
            string[] items91 = { "Dear Sir/Madam,\b", "We acknowledge with thanks the receipt of your above enquiry and have great pleasure in submitting our lowest quotation,\b", "subject to the conditions printed below.\n" };

            Font font12111 = FontFactory.GetFont("Arial", 12, Font.BOLD);
            Font font101111 = FontFactory.GetFont("Arial", 10);
            Paragraph paragraph111 = new Paragraph("", font12111);

            for (int i = 0; i < items91.Length; i++)
            {
                paragraph111.Add(new Phrase(" ", font101111));
            }

            table = new PdfPTable(1);
            table.TotalWidth = 560f;
            table.LockedWidth = true;
            table.SetWidths(new float[] { 560f });

            table.AddCell(new Phrase("\b Subject To Pune Jurisdiction Only \n Payment to be made by A/c. Payee Cheque Only. \n Interest @18 % will be charged on bill not paid within due date", FontFactory.GetFont("Arial", 10)));

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
            doc.Close();
            Byte[] FileBuffer = File.ReadAllBytes(Server.MapPath("~/Files/") + "TaxInvoice.pdf");
            }
            //    }
            //}
            return pdf;
        }

        protected void check_address_CheckedChanged(object sender, EventArgs e)
        {
            if (check_address.Checked == true)
            {
                txtCustName.Text = txtCompName.Text;
                txt_ShipingAdddesss.Text = txt_CompanyAddress.Text;
                txt_CustomerGstNo.Text = txt_CompanyGSTno.Text;
                txt_CustomerPanNo.Text = txt_CompanyPanNo.Text;
                txt_CustomerStateCode.Text = txt_CompanyStateCode.Text;
                drop_CustomerRagisterType.Text = drop_CustomerRagisterType.Text;
            }
        }

        protected void btn_Cancel_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["InvoiceNo"] != null)
            {
                Response.Redirect("Taxinvoicereport.aspx");
            }
            else
            {
            //Response.Redirect("TaxInvoiceList.aspx");
            Response.Redirect("TaxInvoiceList_Sales.aspx");
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
                com.CommandText = "select DISTINCT Pono from CustomerPO_Hdr_Both where " + "Pono like @Search + '%' AND Is_Deleted='0' ";

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

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetCompanyList(string prefixText, int count)
    {
        return AutoFillCompanylist(prefixText);
    }
    public static List<string> AutoFillCompanylist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                // com.CommandText = "select DISTINCT JobNo from CustomerPO_Hdr_Sales where " + "JobNo like @Search + '%' AND Is_Deleted='0'  ";
                com.CommandText = "select DISTINCT Customer_Name from [tbl_Quotation_Hdr_Sales] where " + "Customer_Name like @Search + '%' AND IsDeleted='0'  ";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> Customer_Name = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        Customer_Name.Add(sdr["Customer_Name"].ToString());
                    }
                }
                con.Close();
                return Customer_Name;
            }
        }
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
                com.CommandText = "select DISTINCT JobNo from CustomerPO_Hdr_Both where " + "JobNo like @Search + '%' AND Is_Deleted='0'  ";

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
    public static List<string> GetDescriptionList(string prefixText, int count)
    {
        return AutoFillDescriptionlist(prefixText);
    }

    public static List<string> AutoFillDescriptionlist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT CompName from tblComponent where " + "CompName like @Search + '%' AND isdeleted='0'";

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

    //protected void txt_PoNo_TextChanged(object sender, EventArgs e)
    //{
    //try
    //{
    //    DataTable Dt = new DataTable();
    //    SqlDataAdapter Da = new SqlDataAdapter("SELECT * FROM CustomerPO_Hdr_Sales WHERE Pono='" + txt_PoNo.Text + "'", con);
    //    Da.Fill(Dt);
    //    if (Dt.Rows.Count > 0)
    //    {
    //        DateTime ffff1 = Convert.ToDateTime(Dt.Rows[0]["PoDate"].ToString());
    //        txt_poDate.Text = ffff1.ToString("yyyy-MM-dd");

    //        txt_CompanyGSTno.Text = Dt.Rows[0]["GstNo"].ToString();
    //        txt_KindAtt.Text = Dt.Rows[0]["KindAtt"].ToString();
    //        txt_CompanyAddress.Text = Dt.Rows[0]["DeliveryAddress"].ToString();
    //        txt_Payterm.Text = Dt.Rows[0]["PayTerm"].ToString();
    //        TXtMailtbl.Text = Dt.Rows[0]["EmailId"].ToString();
    //        //txt_subtotal.Text = Dt.Rows[0]["AllTotalPrice"].ToString();
    //        //txt_cgst_amt.Text = Dt.Rows[0]["Cgst"].ToString();
    //        //txt_sgst_amt.Text = Dt.Rows[0]["Sgst"].ToString();    
    //        //txt_grand_total.Text = Dt.Rows[0]["GrandTotal"].ToString();
    //    }

    //    con.Open();
    //    DataTable SDt = new DataTable();
    //    SqlDataAdapter SDA = new SqlDataAdapter("SELECT  * FROM vw_Tax_Invoice WHERE Pono='" + txt_PoNo.Text + "'", con);
    //    SDA.Fill(SDt);
    //    grd_getDTLS.EmptyDataText = "Not Records Found";
    //    grd_getDTLS.DataSource = SDt;
    //    grd_getDTLS.DataBind();
    //    con.Close();
    //}
    //catch (Exception)
    //{

    //    throw;
    //}

    //}

    //protected void txt_discription_TextChanged(object sender, EventArgs e)
    //{
    //    SqlDataAdapter Da = new SqlDataAdapter("SELECT * FROM tblComponent WHERE CompName='" + txt_discription.Text + "'", con);
    //    DataTable Dt = new DataTable();
    //    Da.Fill(Dt);
    //    if (Dt.Rows.Count > 0)
    //    {
    //        txt_total_amount.Text = "0";
    //        txt_discount.Text = "0";

    //        txt_hsn.Text = Dt.Rows[0]["HSN"].ToString();
    //        txt_rate.Text = Dt.Rows[0]["Rate"].ToString();
    //        txt_unit.Text = Dt.Rows[0]["Units"].ToString();
    //        txt_tax.Text = Dt.Rows[0]["Tax"].ToString();
    //    }
    //    txtprintdescription.Text = txt_discription.Text;
    //}
    private decimal TTotal;
    decimal totaltax = 0;

    decimal Total11177 = 0, Qty = 0, rate111111 = 0, totalrateqty = 0, Totaltax12 = 0, Totaltax18 = 0, Totaltx28 = 0;
    protected void grd_getDTLS_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // cast the current row to a datarowview
            DataRowView row = e.Row.DataItem as DataRowView;
            TTotal += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Total"));
            hdnPoProductTot.Value = TTotal.ToString();
            txt_subtotal.Text = TTotal.ToString("##.00");

            string lbltotaltax = (e.Row.FindControl("lbl_tax_GET") as Label).Text;
            string lblrate = (e.Row.FindControl("lbl_rate_GET") as Label).Text;
            string lblqty = (e.Row.FindControl("lbl_quntity_GET") as Label).Text;
            string lblAmount = (e.Row.FindControl("lbl_total_amount_GET") as Label).Text;
            totaltax = Convert.ToDecimal(lbltotaltax);
            if (lbltotaltax == "12")
            {
                TaxPanel2.Visible = true;
                Totaltax12 += Convert.ToDecimal(lblAmount);
                hdn12.Value = Totaltax12.ToString();
                if (txt_CompanyStateCode.Text == "27 MAHARASHTRA")
                {
                    Label.Text = ((Convert.ToDecimal(Totaltax12) * Convert.ToDecimal(lbltotaltax) / 100) / 2).ToString("0.00");
                    TextBox2.Text = ((Convert.ToDecimal(Totaltax12) * Convert.ToDecimal(lbltotaltax) / 100) / 2).ToString("0.00");
                    TextBox3.Text = Convert.ToDecimal(00).ToString("0.00");
                }
                else
                {
                    TextBox3.Text = ((Convert.ToDecimal(Totaltax12) * Convert.ToDecimal(lbltotaltax) / 100)).ToString("0.00");
                    TextBox2.Text = Convert.ToDecimal(00).ToString("0.00");
                    Label.Text = Convert.ToDecimal(00).ToString("0.00");
                }
            }
            if (lbltotaltax == "18")
            {
                TaxPanel1.Visible = true;
                Totaltax18 += Convert.ToDecimal(lblAmount);
                hdn18.Value = Totaltax18.ToString();
                if (txt_CompanyStateCode.Text == "27 MAHARASHTRA")
                {
                    txt_cgst_amt.Text = ((Convert.ToDecimal(Totaltax18) * Convert.ToDecimal(lbltotaltax) / 100) / 2).ToString("0.00");
                    txt_sgst_amt.Text = ((Convert.ToDecimal(Totaltax18) * Convert.ToDecimal(lbltotaltax) / 100) / 2).ToString("0.00");
                    txt_igst_amt.Text = Convert.ToDecimal(00).ToString("0.00");
                }
                else
                {
                    // txt_cgst_amt.Text = ((Convert.ToDecimal(Totaltax18) * Convert.ToDecimal(lbltotaltax) / 100) / 2).ToString("0.00");
                    //  txt_sgst_amt.Text = ((Convert.ToDecimal(Totaltax18) * Convert.ToDecimal(lbltotaltax) / 100) / 2).ToString("0.00");
                    txt_cgst_amt.Text = Convert.ToDecimal(00).ToString("0.00");
                    txt_sgst_amt.Text = Convert.ToDecimal(00).ToString("0.00");
                    txt_igst_amt.Text = ((Convert.ToDecimal(Totaltax18) * Convert.ToDecimal(lbltotaltax) / 100)).ToString("0.00");
                    //txt_igst_amt.Text = Convert.ToDecimal(00).ToString("0.00");
                }
            }
            if (lbltotaltax == "28")
            {
                TaxPanel3.Visible = true;
                Totaltx28 += Convert.ToDecimal(lblAmount);
                hdn28.Value = Totaltx28.ToString();
                if (txt_CompanyStateCode.Text == "27 MAHARASHTRA")
                {
                    TextBox4.Text = ((Convert.ToDecimal(Totaltx28) * Convert.ToDecimal(lbltotaltax) / 100) / 2).ToString("0.00");
                    TextBox5.Text = ((Convert.ToDecimal(Totaltx28) * Convert.ToDecimal(lbltotaltax) / 100) / 2).ToString("0.00");
                    TextBox6.Text = Convert.ToDecimal(00).ToString("0.00");
                }
                else
                {
                    TextBox6.Text = ((Convert.ToDecimal(Totaltx28) * Convert.ToDecimal(lbltotaltax) / 100)).ToString("0.00");
                    TextBox4.Text = Convert.ToDecimal(00).ToString("0.00");
                    TextBox5.Text = Convert.ToDecimal(00).ToString("0.00");
                }
            }

            taxhidden.Value = totalrateqty.ToString();

            decimal grd_total;
            if (!string.IsNullOrEmpty(txt_total_amount.Text))
            {
                grd_total = 0;
            }
            else
            {
                decimal val1 = Convert.ToDecimal(txt_subtotal.Text);
                decimal val2 = Convert.ToDecimal(txt_sgst_amt.Text);
                decimal val3 = Convert.ToDecimal(txt_cgst_amt.Text);
                decimal val4 = Convert.ToDecimal(Label.Text);
                decimal val5 = Convert.ToDecimal(TextBox2.Text);
                decimal val6 = Convert.ToDecimal(TextBox3.Text);
                decimal val7 = Convert.ToDecimal(TextBox4.Text);
                decimal val8 = Convert.ToDecimal(TextBox5.Text);
                decimal val9 = Convert.ToDecimal(TextBox6.Text);
                decimal val10 = Convert.ToDecimal(txt_igst_amt.Text);
                grd_total = (val1 + val2 + val3 + val4 + val5 + val6 + val7 + val8 + val9 + val10);
            }
            //txt_grand_total.Text = grd_total.ToString();
            txt_grand_total.Text = grd_total.ToString("##.00");


            hfTotal.Value = txt_grand_total.Text;
            string isNegative = "";
            try
            {
                string number = hfTotal.Value;
                number = Convert.ToDouble(number).ToString();

                lbl_Amount_In_Word.Text = isNegative + ConvertToWords(number);
            }
            catch (Exception)
            {

                throw;
            }

            //Round Off
            var Totalamtfff = Convert.ToDouble(txt_grand_total.Text);
            var totalgrand = Math.Round(Totalamtfff);
            System.Globalization.CultureInfo info = System.Globalization.CultureInfo.GetCultureInfo("en-IN");
            string FinaleTotalamt = Totalamtfff.ToString("N2", info);
            double GetVal = 0;  // to know rounded value
            GetVal -= Convert.ToDouble(Totalamtfff) - Convert.ToDouble(totalgrand);
            Double roundoff = Math.Round(GetVal, 2);
            txtroundoff.Text = roundoff.ToString();
            txt_grand_total.Text = totalgrand.ToString();
        }
    }

    protected void gv_cancel_Click(object sender, EventArgs e)
    {
        GridViewRow row = (sender as LinkButton).NamingContainer as GridViewRow;

        string Tax = ((TextBox)row.FindControl("txt_tax_grd")).Text;
        string Quntity = ((TextBox)row.FindControl("txt_quntity_grd")).Text;
        string Rate = ((TextBox)row.FindControl("txt_rate_grd")).Text;
        string Discount = ((TextBox)row.FindControl("txt_discount_grd")).Text;
        string TotalAmount = ((TextBox)row.FindControl("txt_total_amount_grd")).Text;

        DataTable Dt = ViewState["Invoice"] as DataTable;
        gvPurchaseRecord.EditIndex = -1;
        //Show_Grid();
        //gvPurchaseRecord.DataBind();
        ViewState["Invoice"] = Dt;
        gvPurchaseRecord.EditIndex = -1;

        gvPurchaseRecord.DataSource = (DataTable)ViewState["Invoice"];
        gvPurchaseRecord.DataBind();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Success", "scrollToElement();", true);
    }

    protected void gvPurchaseRecord_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        //gvPurchaseRecord.EditIndex = -1;
        //gvPurchaseRecord.DataSource = (DataTable)ViewState["Invoice"];
        //gvPurchaseRecord.DataBind();
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Success", "scrollToElement();", true);
    }

    protected void lnkMAILDelete_Click(object sender, EventArgs e)
    {
        GridViewRow row = (sender as LinkButton).NamingContainer as GridViewRow;

        DataTable dt = ViewState["MULTMail"] as DataTable;
        dt.Rows.Remove(dt.Rows[row.RowIndex]);
        ViewState["MULTMail"] = dt;
        Grd_MAIL.DataSource = (DataTable)ViewState["MULTMail"];
        Grd_MAIL.DataBind();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Mail ID Delete Succesfully !!!');", true);
    }

    protected void ShowMAILRecord()
    {
        ViewState["RowNo"] = (int)ViewState["RowNo"] + 1;
        DataTable Dt = (DataTable)ViewState["MULTMail"];
        Dt.Rows.Add(ViewState["RowNo"], TXtMailtbl.Text);
        ViewState["MULTMail"] = Dt;

        TXtMailtbl.Text = string.Empty;

        Grd_MAIL.DataSource = (DataTable)ViewState["MULTMail"];
        Grd_MAIL.DataBind();
    }

    protected void Lnkbtn_Addmail_Click(object sender, EventArgs e)
    {
        if (TXtMailtbl.Text == "")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Please fill Mail ID  !!!');", true);
        }
        else
        {
            ShowMAILRecord();
        }
    }

    //protected void txtJobNo_TextChanged(object sender, EventArgs e)
    //{
    //    ////customer information 
    //    SqlDataAdapter Da = new SqlDataAdapter("SELECT * FROM CustomerPO_Hdr_Sales WHERE JobNo='" + txtJobNo.Text + "'  ", con);
    //    DataTable Dt = new DataTable();
    //    Da.Fill(Dt);
    //    if (Dt.Rows.Count > 0)
    //    {
    //        txtCompName.Text = Dt.Rows[0]["CustomerName"].ToString();
    //        txt_PoNo.Text = Dt.Rows[0]["Pono"].ToString();
    //        DateTime ffff1 = Convert.ToDateTime(Dt.Rows[0]["PoDate"].ToString());
    //        txt_poDate.Text = ffff1.ToString("yyyy-MM-dd");
    //        txt_CompanyGSTno.Text = Dt.Rows[0]["GstNo"].ToString();
    //        txt_KindAtt.Text = Dt.Rows[0]["KindAtt"].ToString();
    //        txt_CompanyAddress.Text = Dt.Rows[0]["DeliveryAddress"].ToString();
    //        txt_Payterm.Text = Dt.Rows[0]["PayTerm"].ToString();
    //        TXtMailtbl.Text = Dt.Rows[0]["EmailId"].ToString();
    //    }
    //    ///pan number information in customer table
    //    SqlDataAdapter Daa = new SqlDataAdapter("SELECT * FROM tblCustomer WHERE CustomerName='" + txtCompName.Text + "'  ", con);
    //    DataTable Dtt = new DataTable();
    //    Daa.Fill(Dtt);
    //    if (Dtt.Rows.Count > 0)
    //    {
    //        txt_CompanyPanNo.Text = Dtt.Rows[0]["PanNo"].ToString();
    //        txt_CompanyStateCode.Text = Dtt.Rows[0]["StateCode"].ToString();
    //    }
    //    ///bind grid in automatic in customer po table.
    //    con.Open();
    //    DataTable SDt = new DataTable();
    //    SqlDataAdapter SDA = new SqlDataAdapter("SELECT [Description],[Hsn_Sac],[TaxPercenteage],[Quantity],[Unit],[Rate],[DiscountPercentage],[Total] FROM vw_Tax_Invoice WHERE Pono='" + txt_PoNo.Text + "'", con);
    //    SDA.Fill(SDt);
    //    int count = 1;
    //    if (SDt.Rows.Count > 0)
    //    {
    //        ViewState["RowNo"] = 0;
    //        Dt_Itemsdetails.Columns.AddRange(new DataColumn[9] { new DataColumn("Id"),
    //            new DataColumn("Description"),  new DataColumn("Hsn_Sac"),
    //            new DataColumn("TaxPercenteage"), new DataColumn("Quantity"),
    //            new DataColumn("Unit"), new DataColumn("Rate"),
    //            new DataColumn("DiscountPercentage"), new DataColumn("Total")
    //          });

    //        ViewState["Invoicedetails"] = Dt_Itemsdetails;
    //        for (int i = 0; i < SDt.Rows.Count; i++)
    //        {
    //            Dt_Itemsdetails.Rows.Add(count, SDt.Rows[i]["Description"].ToString(), SDt.Rows[i]["Hsn_Sac"].ToString(), SDt.Rows[i]["TaxPercenteage"].ToString(), SDt.Rows[i]["Quantity"].ToString(), SDt.Rows[i]["Unit"].ToString(), SDt.Rows[i]["Rate"].ToString(), SDt.Rows[i]["DiscountPercentage"].ToString(), SDt.Rows[i]["Total"].ToString());

    //            count = count + 1;
    //        }
    //    }

    //    grd_getDTLS.DataSource = Dt_Itemsdetails;
    //    grd_getDTLS.DataBind();
    //    grd_getDTLS.EmptyDataText = "Not Records Found";
    //    con.Close();
    //    //kint attence bind
    //    DataTable dt1 = new DataTable();
    //    SqlDataAdapter sad1 = new SqlDataAdapter("select * from tblCustomerContactPerson where CustName='" + txtCompName.Text + "'", con);
    //    sad1.Fill(dt1);
    //    if (dt1.Rows.Count>0)
    //    {
    //        txt_KindAtt.DataTextField = "ContactPerName";
    //        txt_KindAtt.DataSource = dt1;
    //        txt_KindAtt.DataBind();
    //        txt_KindAtt.Items.Insert(0, "-- Select Kind Att. --");
    //    }

    //    //email gridview bind
    //    DataTable dt2 = new DataTable();
    //    SqlDataAdapter sad2 = new SqlDataAdapter("select * from tblCustomerContactPerson where CustName='" + txtCompName.Text + "'", con);
    //    sad2.Fill(dt2);
    //    Grd_MAIL.DataSource = dt2;
    //    Grd_MAIL.DataBind();
    //    Grd_MAIL.EmptyDataText = "Record Not Found";

    //}

    protected void Grd_MAIL_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (btn_save.Text == "Update")
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                con.Open();
                int id = Convert.ToInt32(Grd_MAIL.DataKeys[e.Row.RowIndex].Values[0]);
                CheckBox chkmailupdate = (CheckBox)e.Row.FindControl("chkmail");
                Label mail = (Label)e.Row.FindControl("lblmultMail");
                SqlCommand cmd = new SqlCommand("select chkEmail from InvoiceMail where InvoiceNo='" + txt_InvoiceNo.Text + "' AND id='" + id + "'", con);

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {

                    chkupdate = dr["chkEmail"].ToString();
                    con.Close();
                }
                chkmailupdate.Checked = chkupdate == "True" ? true : false;
                con.Close();
            }
        }
    }

    protected void lnkbtnDelete_Click(object sender, EventArgs e)
    {
        GridViewRow row = (sender as LinkButton).NamingContainer as GridViewRow;

        DataTable dt = ViewState["Invoicedetails"] as DataTable;
        dt.Rows.Remove(dt.Rows[row.RowIndex]);
        ViewState["Invoicedetails"] = dt;
        grd_getDTLS.DataSource = (DataTable)ViewState["Invoicedetails"];
        grd_getDTLS.DataBind();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Data Delete Succesfully !!!');", true);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Success", "scrollToElement();", true);
    }

    protected void txtCompName_TextChanged(object sender, EventArgs e)
    {
        SqlDataAdapter Da = new SqlDataAdapter("SELECT * FROM [tbl_Quotation_Hdr_Sales] WHERE Customer_Name='" + txtCompName.Text + "'  ", con);
        DataTable Dt = new DataTable();
        Da.Fill(Dt);
        if (Dt.Rows.Count > 0)
        {
            //DateTime ffff1 = Convert.ToDateTime(Dt.Rows[0]["PoDate"].ToString());
            //txt_poDate.Text = ffff1.ToString("yyyy-MM-dd");
            txt_CompanyGSTno.Text = Dt.Rows[0]["GST_No"].ToString();
            txt_KindAtt.Text = Dt.Rows[0]["kind_Att"].ToString();
            txt_CompanyAddress.Text = Dt.Rows[0]["Address"].ToString();
            //txt_Payterm.Text = Dt.Rows[0]["PayTerm"].ToString();
        }

        ///pan number information in customer table
        SqlDataAdapter Daa = new SqlDataAdapter("SELECT * FROM tblCustomer WHERE CustomerName='" + txtCompName.Text + "'  ", con);
        DataTable Dtt = new DataTable();
        Daa.Fill(Dtt);
        if (Dtt.Rows.Count > 0)
        {
            txt_CompanyGSTno.Text = Dtt.Rows[0]["GSTNo"].ToString();
            // txt_KindAtt.Text = Dt.Rows[0]["kind_Att"].ToString();
            txt_CompanyAddress.Text = Dtt.Rows[0]["AddresLine1"].ToString();
            txt_CompanyPanNo.Text = Dtt.Rows[0]["PanNo"].ToString();
            txt_CompanyStateCode.Text = Dtt.Rows[0]["StateCode"].ToString();
        }

        SqlDataAdapter Daaa = new SqlDataAdapter("SELECT * FROM CustomerPO_Hdr_Both WHERE CustomerName='" + txtCompName.Text + "'  ", con);
        DataTable Dttt = new DataTable();
        Daaa.Fill(Dttt);
        if (Dttt.Rows.Count > 0)
        {

            //txt_PoNo.Text = Dttt.Rows[0]["Pono"].ToString();
            DateTime ffff1 = Convert.ToDateTime(Dttt.Rows[0]["PoDate"].ToString());
            //txt_poDate.Text = ffff1.ToString("yyyy-MM-dd");
            txt_Payterm.Text = Dttt.Rows[0]["PayTerm"].ToString();
            txt_KindAtt.Text = Dttt.Rows[0]["KindAtt"].ToString();
        }

        //email gridview bind
        DataTable dt2 = new DataTable();
        SqlDataAdapter sad2 = new SqlDataAdapter("select * from tblCustomerContactPerson where CustName='" + txtCompName.Text + "'", con);
        sad2.Fill(dt2);
        Grd_MAIL.DataSource = dt2;
        Grd_MAIL.DataBind();
        Grd_MAIL.EmptyDataText = "Record Not Found";
        //GetJobNO();

        //  ddljobnobind();

        //kint attence bind
        DataTable dt1 = new DataTable();
        SqlDataAdapter sad1 = new SqlDataAdapter("select * from tblCustomerContactPerson where CustName='" + txtCompName.Text + "'", con);
        sad1.Fill(dt1);
        if (dt1.Rows.Count > 0)
        {
            // txt_KindAtt.Text = dt1.Rows[0]["KindAtt"].ToString();
        }
    }

    protected void ddlagainst_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (!String.IsNullOrEmpty(txtCompName.Text))
            {
                if (ddlagainst.SelectedItem.Text == "Direct")
                {
                    ddlagainstno.Enabled = false;
                }
                else if (ddlagainst.Text == "Order")
                {
                    //TableDirect.Visible = false;
                    //TableOrder.Visible = true;

                    ddlagainstno.Enabled = true;
                    // SqlDataAdapter ad = new SqlDataAdapter("SELECT [Quotationno] FROM [tbl_QuotationHdr] WHERE Companyname = '" + txtCompName.Text + "'", con);
                    SqlDataAdapter ad = new SqlDataAdapter("SELECT [Pono] FROM [CustomerPO_Hdr_Both] WHERE CustomerName = '" + txtCompName.Text + "' AND  Is_Deleted = 0 ", con);
                    DataTable dt = new DataTable();
                    ad.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        ddlagainstno.DataSource = dt;
                        ddlagainstno.DataTextField = "Pono";
                        ddlagainstno.DataBind();
                        ddlagainstno.Items.Insert(0, "-- Select Product --");
                    }
                }
                else
                {
                    ddlagainst.SelectedValue = "0";
                }
            }
            else
            {
                // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Customer Name First');window.location.href='TaxInvoice.aspx';", true);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }


        //if (ddlagainst.SelectedItem.Text == "Order")
        //{
        //    ///bind grid in automatic in customer po table.
        //    con.Open();
        //    DataTable SDt = new DataTable();
        //    SqlDataAdapter SDA = new SqlDataAdapter("SELECT JobNo,[Description],[Hsn_Sac],[Rate],[Unit],[Quantity],[TaxPercenteage],[DiscountPercentage],[Total] FROM vw_Tax_Invoice WHERE Pono='" + txt_PoNo.Text + "'", con);
        //    SDA.Fill(SDt);
        //    int count = 1;
        //    if (SDt.Rows.Count > 0)
        //    {

        //        ViewState["RowNo"] = 0;
        //        Dt_Itemsdetails.Columns.AddRange(new DataColumn[10] { new DataColumn("Id"),new DataColumn("JobNo"),
        //        new DataColumn("Description"),  new DataColumn("Hsn_Sac"),
        //        new DataColumn("Rate"),  new DataColumn("Unit"),
        //        new DataColumn("Quantity"),  new DataColumn("TaxPercenteage"),
        //        new DataColumn("DiscountPercentage"),  new DataColumn("Total"),
        //      });

        //        ViewState["Invoicedetails"] = Dt_Itemsdetails;
        //        for (int i = 0; i < SDt.Rows.Count; i++)
        //        {
        //            Dt_Itemsdetails.Rows.Add(count, SDt.Rows[i]["JobNo"].ToString(), SDt.Rows[i]["Description"].ToString(), SDt.Rows[i]["Hsn_Sac"].ToString(), SDt.Rows[i]["Rate"].ToString(), SDt.Rows[i]["Unit"].ToString(), SDt.Rows[i]["Quantity"].ToString(), SDt.Rows[i]["TaxPercenteage"].ToString(), SDt.Rows[i]["DiscountPercentage"].ToString(), SDt.Rows[i]["Total"].ToString());

        //            count = count + 1;
        //        }
        //    }

        //    grd_getDTLS.DataSource = Dt_Itemsdetails;
        //    grd_getDTLS.DataBind();
        //    grd_getDTLS.EmptyDataText = "Not Records Found";
        //    con.Close();
        //}
    }

    protected void ddlagainstno_TextChanged(object sender, EventArgs e)
    {
        try
        {
            ///bind grid in automatic in customer po table.
            con.Open();
            DataTable SDt = new DataTable();
            SqlDataAdapter SDA = new SqlDataAdapter("SELECT JobNo, MateName, [Description], PrintDescription,[Hsn_Sac],[Rate],[Unit],[Quantity],[TaxPercenteage],[DiscountPercentage],[Total] FROM vw_Tax_Invoice_Sales WHERE Pono='" + ddlagainstno.Text + "'", con);
            SDA.Fill(SDt);
            int count = 1;
            if (SDt.Rows.Count > 0)
            {
                ViewState["RowNo"] = 0;
                Dt_Itemsdetails.Columns.AddRange(new DataColumn[12] { new DataColumn("Id"),new DataColumn("JobNo"), new DataColumn("MateName"),
                    new DataColumn("Description"), new DataColumn("PrintDescription"),  new DataColumn("Hsn_Sac"),
                    new DataColumn("Rate"),  new DataColumn("Unit"),
                    new DataColumn("Quantity"),  new DataColumn("TaxPercenteage"),
                    new DataColumn("DiscountPercentage"),  new DataColumn("Total"),
                  });

                ViewState["Invoicedetails"] = Dt_Itemsdetails;
                for (int i = 0; i < SDt.Rows.Count; i++)
                {
                    Dt_Itemsdetails.Rows.Add(count, SDt.Rows[i]["JobNo"].ToString(), SDt.Rows[i]["MateName"].ToString(), SDt.Rows[i]["Description"].ToString(), SDt.Rows[i]["PrintDescription"].ToString(), SDt.Rows[i]["Hsn_Sac"].ToString(), SDt.Rows[i]["Rate"].ToString(), SDt.Rows[i]["Unit"].ToString(), SDt.Rows[i]["Quantity"].ToString(), SDt.Rows[i]["TaxPercenteage"].ToString(), SDt.Rows[i]["DiscountPercentage"].ToString(), SDt.Rows[i]["Total"].ToString());

                    count = count + 1;
                }
            }

            grd_getDTLS.DataSource = Dt_Itemsdetails;
            grd_getDTLS.DataBind();
            grd_getDTLS.EmptyDataText = "Not Records Found";
            con.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        //txt_PoNo.Text = ddlagainstno.Text.Trim();
        GetPODate();
    }

    //protected void txt_jobno_TextChanged(object sender, EventArgs e)
    //{
    //    SqlDataAdapter Da = new SqlDataAdapter("SELECT Id,JobNo,MateName FROM tblInwardEntry WHERE JobNo ='" + txt_jobno.Text + "'", con);
    //    DataTable Dt = new DataTable();
    //    Da.Fill(Dt);
    //    txtpoduct.Text = Dt.Rows[0]["MateName"].ToString();
    //}

    //Job No Against Quation No For componnt details
    //public void GetJobNO()
    //{
    //    DataTable dt = new DataTable();
    //    SqlDataAdapter sd = new SqlDataAdapter("select JobNo from  tblInwardEntry  where CustName='" + txtCompName.Text + "'", con);
    //    sd.Fill(dt);

    //    if (dt.Rows.Count > 0)
    //    {
    //        txt_jobno.DataTextField = "JobNo";
    //        txt_jobno.DataSource = dt;
    //        txt_jobno.DataBind();
    //    }
    //    else
    //    {

    //    }
    //}

    private void Show_Grid1()
    {
        try
        {

            ViewState["RowNo"] = (int)ViewState["RowNo"] + 1;
            DataTable Dt = (DataTable)ViewState["Invoice"];

            Dt.Rows.Add(ViewState["RowNo"], txtpoduct.Text, txtprintdescription.Text, txt_hsn.Text, txt_rate.Text, txt_unit.Text, txt_quntity.Text, txt_tax.Text, txt_discount.Text, txt_total_amount.Text);
            ViewState["Invoice"] = Dt;

            //txt_jobno.Text = string.Empty;
            // txt_discription.Text = string.Empty;
            txt_hsn.Text = string.Empty;
            txtpoduct.Text = string.Empty;
            txtprintdescription.Text = string.Empty;
            txt_rate.Text = string.Empty;
            txt_unit.Text = string.Empty;
            txt_quntity.Text = string.Empty;
            txt_tax.Text = string.Empty;
            txt_discount.Text = string.Empty;
            txt_total_amount.Text = string.Empty;

            gvPurchaseRecord.DataSource = Dt;
            gvPurchaseRecord.DataBind();

        }
        catch (Exception ex)
        {
            //  Block of code to handle errors
        }


    }


    public void GetPODate()
    {

        SqlDataAdapter Daaa = new SqlDataAdapter("SELECT * FROM CustomerPO_Hdr_Both WHERE Pono ='" + ddlagainstno.Text + "'  ", con);
        DataTable Dttt = new DataTable();
        Daaa.Fill(Dttt);
        if (Dttt.Rows.Count > 0)
        {

            txt_PoNo.Text = Dttt.Rows[0]["Pono"].ToString();
            DateTime ffff1 = Convert.ToDateTime(Dttt.Rows[0]["PoDate"].ToString());
            txt_poDate.Text = ffff1.ToString("yyyy-MM-dd");

        }

    }
}