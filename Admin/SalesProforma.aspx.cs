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

public partial class Admin_SalesProforma : System.Web.UI.Page
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
            
            GenerateInvoice();
            ViewState["RowNo"] = 0;
            Dt_Items.Columns.AddRange(new DataColumn[9] { new DataColumn("ID"), new DataColumn("Description"), new DataColumn("HSN/SAC"), new DataColumn("Tax"), new DataColumn("Quntity"), new DataColumn("Unit"), new DataColumn("Rate"), new DataColumn("Discount"), new DataColumn("TotalAmount") });
            ViewState["Invoice"] = Dt_Items;

            if (Request.QueryString["Id"] != null)
            {
                ID = Decrypt(Request.QueryString["Id"].ToString());
                hdnID.Value = ID;
                btn_save.Text = "Update";
                Loadrecord();
               
            }
        }
    }
    protected void Loadrecord()
    {
        txtJobNo.ReadOnly = true;
        ////load record in header table
        DataTable dtheader = new DataTable();
        SqlDataAdapter sad1 = new SqlDataAdapter("select * from tblsalesProformaHdr where Id='" + ID + "'", con);
        sad1.Fill(dtheader);
        if (dtheader.Rows.Count > 0)
        {
            txt_InvoiceNo.Text = dtheader.Rows[0]["InvNo"].ToString();
            txtJobNo.Text = dtheader.Rows[0]["JobNo"].ToString();
            txtorderno.Text = dtheader.Rows[0]["orderNo"].ToString();
            txt_KindAtt.SelectedItem.Text = dtheader.Rows[0]["KindAtt"].ToString();
            txtCrdays.Text = dtheader.Rows[0]["Cr_Days"].ToString();
            txtCompName.Text = dtheader.Rows[0]["CompanyName"].ToString();
            txt_CompanyPanNo.Text = dtheader.Rows[0]["Comanypanno"].ToString();
            txt_CompanyAddress.Text = dtheader.Rows[0]["CompanyAddress"].ToString();
            drop_CompanyRegisterType.SelectedItem.Text = dtheader.Rows[0]["Companyregistetype"].ToString();
            txt_CompanyGSTno.Text = dtheader.Rows[0]["CompanyGstNo"].ToString();
            txt_CompanyStateCode.Text = dtheader.Rows[0]["CompanyStateCode"].ToString();
            txt_cgst_amt.Text = dtheader.Rows[0]["CGST"].ToString();
            txt_sgst_amt.Text = dtheader.Rows[0]["SGST"].ToString();
            txt_igst_amt.Text = dtheader.Rows[0]["IGST"].ToString();
            txt_grand_total.Text = dtheader.Rows[0]["GradTotal"].ToString();
            lbl_Amount_In_Word.Text = dtheader.Rows[0]["Inwordgrandtotal"].ToString();
            DateTime ffff1 = Convert.ToDateTime(dtheader.Rows[0]["invoiceDate"].ToString());
            txt_InvoiceDate.Text = ffff1.ToString("yyyy-MM-dd");
            DateTime ffff2 = Convert.ToDateTime(dtheader.Rows[0]["orderdate"].ToString());
            txtorederdate.Text = ffff2.ToString("yyyy-MM-dd");

            string str = dtheader.Rows[0]["TermCondition1"].ToString();
            string str1 = dtheader.Rows[0]["TermCondition2"].ToString();
            string str2 = dtheader.Rows[0]["TermCondition3"].ToString();
            string str3 = dtheader.Rows[0]["Termcondition4"].ToString();
            string[] arrstr = str.ToString().Split('-');
            string[] arrstr1 = str1.ToString().Split('-');
            string[] arrstr2 = str2.ToString().Split('-');
            string[] arrstr3 = str3.ToString().Split('-');

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


        }
        ////load data in details table
        DataTable Dt_Product = new DataTable();
        SqlDataAdapter daa = new SqlDataAdapter("SELECT  Description,Hsn,Tax,Quantity,Unit,Rate,Discount,TotalAmt FROM tblSalesProformDtls WHERE Invno='" + txt_InvoiceNo.Text + "'", con);
        daa.Fill(Dt_Product);

        int Count = 1;
        if (Dt_Product.Rows.Count > 0)
        {
            if (Dt_Items.Columns.Count < 1)
            {
                ShowGrid();
            }

            for (int i = 0; i < Dt_Product.Rows.Count; i++)
            {
                Dt_Items.Rows.Add(Count, Dt_Product.Rows[i]["Description"].ToString(), Dt_Product.Rows[i]["Hsn"].ToString(), Dt_Product.Rows[i]["Tax"].ToString(), Dt_Product.Rows[i]["Quantity"].ToString(), Dt_Product.Rows[i]["Unit"].ToString(), Dt_Product.Rows[i]["Rate"].ToString(), Dt_Product.Rows[i]["Discount"].ToString(), Dt_Product.Rows[i]["TotalAmt"].ToString());
                Count = Count + 1;
            }
        }
        gvPurchaseRecord.DataSource = Dt_Items;
        gvPurchaseRecord.DataBind();
        ////load email
        SqlDataAdapter Sda = new SqlDataAdapter("SELECT * FROM tblSalesProformaEmail WHERE InviNo='" + txt_InvoiceNo.Text + "'", con);
        DataTable Sdt = new DataTable();
        Sda.Fill(Sdt);
        Grd_MAIL.DataSource = Sdt;
        Grd_MAIL.DataBind();
        //bind kitt attence
        DataTable dt1 = new DataTable();
        SqlDataAdapter sad4 = new SqlDataAdapter("select * from tblCustomerContactPerson where CustName='" + txtCompName.Text + "'", con);
        sad4.Fill(dt1);
        txt_KindAtt.DataTextField = "ContactPerName";

        txt_KindAtt.DataSource = dt1;
        txt_KindAtt.DataBind();
        txt_KindAtt.SelectedItem.Text = dtheader.Rows[0]["KindAtt"].ToString();


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
    protected void GenerateInvoice()
    {
        SqlDataAdapter ad = new SqlDataAdapter("SELECT max([Id]) as maxid FROM [tblsalesProformaHdr]", con);
        DataTable dt = new DataTable();
        ad.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            int maxid = dt.Rows[0]["maxid"].ToString() == "" ? 0 : Convert.ToInt32(dt.Rows[0]["maxid"].ToString());
            txt_InvoiceNo.Text = "INV-" + (maxid + 1).ToString();
        }
        else
        {
            txt_InvoiceNo.Text = string.Empty;
        }
    }

    protected void txtJobNo_TextChanged(object sender, EventArgs e)
    {
        ////customer information 
        SqlDataAdapter Da = new SqlDataAdapter("SELECT * FROM CustomerPO_Hdr WHERE JobNo='" + txtJobNo.Text + "'  ", con);
        DataTable Dt = new DataTable();
        Da.Fill(Dt);
        if (Dt.Rows.Count > 0)
        {
            txtCompName.Text = Dt.Rows[0]["CustomerName"].ToString();

            DateTime ffff1 = Convert.ToDateTime(Dt.Rows[0]["PoDate"].ToString());

            txt_CompanyGSTno.Text = Dt.Rows[0]["GstNo"].ToString();
            txt_KindAtt.Text = Dt.Rows[0]["KindAtt"].ToString();
            txt_CompanyAddress.Text = Dt.Rows[0]["DeliveryAddress"].ToString();

        }
        ///pan number information in customer table
        SqlDataAdapter Daa = new SqlDataAdapter("SELECT * FROM tblCustomer WHERE CustomerName='" + txtCompName.Text + "'  ", con);
        DataTable Dtt = new DataTable();
        Daa.Fill(Dtt);
        if (Dtt.Rows.Count > 0)
        {
            txt_CompanyPanNo.Text = Dtt.Rows[0]["PanNo"].ToString();
            txt_CompanyStateCode.Text = Dtt.Rows[0]["StateCode"].ToString();
        }
        ///bind grid in automatic in customer po table.
        con.Open();
        DataTable SDt = new DataTable();
        SqlDataAdapter SDA = new SqlDataAdapter("SELECT [Description],[Hsn_Sac],[TaxPercenteage],[Quantity],[Unit],[Rate],[DiscountPercentage],[Total],JobNo  FROM vw_Tax_Invoice WHERE JobNo='" + txtJobNo.Text + "'", con);
        SDA.Fill(SDt);
        int count = 1;
        if (SDt.Rows.Count > 0)
        {
            ViewState["RowNo"] = 0;
            Dt_Itemsdetails.Columns.AddRange(new DataColumn[9] { new DataColumn("Id"),
                new DataColumn("Description"),  new DataColumn("Hsn_Sac"),
                new DataColumn("TaxPercenteage"), new DataColumn("Quantity"),
                new DataColumn("Unit"), new DataColumn("Rate"),
                new DataColumn("DiscountPercentage"), new DataColumn("Total")
              });

            ViewState["Invoicedetails"] = Dt_Itemsdetails;
            for (int i = 0; i < SDt.Rows.Count; i++)
            {
                Dt_Itemsdetails.Rows.Add(count, SDt.Rows[i]["Description"].ToString(), SDt.Rows[i]["Hsn_Sac"].ToString(), SDt.Rows[i]["TaxPercenteage"].ToString(), SDt.Rows[i]["Quantity"].ToString(), SDt.Rows[i]["Unit"].ToString(), SDt.Rows[i]["Rate"].ToString(), SDt.Rows[i]["DiscountPercentage"].ToString(), SDt.Rows[i]["Total"].ToString());

                count = count + 1;
            }
        }

        grd_getDTLS.DataSource = Dt_Itemsdetails;
        grd_getDTLS.DataBind();
        grd_getDTLS.EmptyDataText = "Not Records Found";
        con.Close();
        //kint attence bind
        DataTable dt1 = new DataTable();
        SqlDataAdapter sad1 = new SqlDataAdapter("select * from tblCustomerContactPerson where CustName='" + txtCompName.Text + "'", con);
        sad1.Fill(dt1);
        txt_KindAtt.DataTextField = "ContactPerName";

        txt_KindAtt.DataSource = dt1;
        txt_KindAtt.DataBind();

        //email gridview bind
        DataTable dt2 = new DataTable();
        SqlDataAdapter sad2 = new SqlDataAdapter("select * from tblCustomerContactPerson where CustName='" + txtCompName.Text + "'", con);
        sad2.Fill(dt2);
        Grd_MAIL.DataSource = dt2;
        Grd_MAIL.DataBind();
        Grd_MAIL.EmptyDataText = "Record Not Found";
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

    protected void btn_add_more_Click(object sender, EventArgs e)
    {
        if (txt_discription.Text == "" || txt_hsn.Text == "" || txt_rate.Text == "" || txt_quntity.Text == "")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Please fill Component Information  !!!');", true);
            //txt_quntity.Focus();
        }
        else
        {
            ShowGrid();
        }
        if (hdnPoProductTot.Value != "")
        {
            decimal totaltax = 0, totalgrdtax = 0;
            decimal Total11177 = 0, Qty = 0, rate111111 = 0, totalrateqty = 0;
            foreach (GridViewRow g1 in gvPurchaseRecord.Rows)
            {
                string lbltotaltax = (g1.FindControl("lbl_tax_grd") as Label).Text;
                string lblrate = (g1.FindControl("lbl_rate_grd") as Label).Text;
                string lblqty = (g1.FindControl("lbl_quntity_grd") as Label).Text;
                totaltax = Convert.ToDecimal(lbltotaltax);

                rate111111 = Convert.ToDecimal(lblrate);
                Qty = Convert.ToDecimal(lblqty);
                Total11177 = ((Qty * rate111111) * totaltax) / 100;
                totalrateqty += Total11177;
            }
            totalgrdtax = totalrateqty + Convert.ToDecimal(taxhidden.Value);
            if (txt_CompanyStateCode.Text == "27 MAHARASHTRA")
            {
                txt_cgst_amt.Text = Convert.ToDecimal(totalgrdtax / 2).ToString("##.00");
                txt_sgst_amt.Text = Convert.ToDecimal(totalgrdtax / 2).ToString("##.00");
                txt_igst_amt.Text = Convert.ToDecimal(00).ToString("##.00");
            }
            else
            {
                txt_cgst_amt.Text = Convert.ToDecimal(00).ToString("##.00");
                txt_sgst_amt.Text = Convert.ToDecimal(00).ToString("##.00");
                txt_igst_amt.Text = Convert.ToDecimal(totalgrdtax).ToString("##.00");
            }
            //decimal taxtotal = 0;
            //decimal totaltax1 = 0;
            //foreach (GridViewRow g1 in gvPurchaseRecord.Rows)
            //{
            //    string lbltotaltax = (g1.FindControl("lbl_tax_grd") as Label).Text;
            //    totaltax1 += Convert.ToDecimal(lbltotaltax);
            //}
            //taxtotal = totaltax1 + Convert.ToDecimal(taxhidden.Value);
            //txt_cgst_amt.Text = (taxtotal / 2).ToString();
            //txt_sgst_amt.Text = (taxtotal / 2).ToString();

        }
        else
        {
            decimal totaltax = 0;
            decimal Total11177 = 0, Qty = 0, rate111111 = 0, totalrateqty = 0;
            foreach (GridViewRow g1 in gvPurchaseRecord.Rows)
            {
                string lbltotaltax = (g1.FindControl("lbl_tax_grd") as Label).Text;
                string lblrate = (g1.FindControl("lbl_rate_grd") as Label).Text;
                string lblqty = (g1.FindControl("lbl_quntity_grd") as Label).Text;
                totaltax = Convert.ToDecimal(lbltotaltax);

                rate111111 = Convert.ToDecimal(lblrate);
                Qty = Convert.ToDecimal(lblqty);
                Total11177 = ((Qty * rate111111) * totaltax) / 100;
                totalrateqty += Total11177;
            }
            if (txt_CompanyStateCode.Text == "27 MAHARASHTRA")
            {
                txt_cgst_amt.Text = Convert.ToDecimal(totalrateqty / 2).ToString("##.00");
                txt_sgst_amt.Text = Convert.ToDecimal(totalrateqty / 2).ToString("##.00");
                txt_igst_amt.Text = Convert.ToDecimal(00).ToString("##.00");
            }
            else
            {
                txt_cgst_amt.Text = Convert.ToDecimal(00).ToString("##.00");
                txt_sgst_amt.Text = Convert.ToDecimal(00).ToString("##.00");
                txt_igst_amt.Text = Convert.ToDecimal(totalrateqty).ToString("##.00");
            }
        }

    }
    private void ShowGrid()
    {
        ViewState["RowNo"] = (int)ViewState["RowNo"] + 1;
        DataTable Dt = (DataTable)ViewState["Invoice"];

        Dt.Rows.Add(ViewState["RowNo"], txt_discription.Text.Trim(), txt_hsn.Text, txt_tax.Text, txt_quntity.Text, txt_unit.Text, txt_rate.Text, txt_discount.Text, txt_total_amount.Text);
        ViewState["Invoice"] = Dt;

        txt_discription.Text = string.Empty;
        txt_hsn.Text = string.Empty;
        txt_tax.Text = string.Empty;
        txt_quntity.Text = string.Empty;
        txt_unit.Text = string.Empty;
        txt_rate.Text = string.Empty;
        txt_discount.Text = string.Empty;
        txt_total_amount.Text = string.Empty;

        gvPurchaseRecord.DataSource = Dt;
        gvPurchaseRecord.DataBind();
    }

    protected void txt_discription_TextChanged(object sender, EventArgs e)
    {
        SqlDataAdapter Da = new SqlDataAdapter("SELECT * FROM tblComponent WHERE CompName='" + txt_discription.Text + "'", con);
        DataTable Dt = new DataTable();
        Da.Fill(Dt);
        if (Dt.Rows.Count > 0)
        {
            txt_total_amount.Text = "0";
            txt_discount.Text = "0";

            txt_hsn.Text = Dt.Rows[0]["HSN"].ToString();
            txt_rate.Text = Dt.Rows[0]["Rate"].ToString();
            txt_unit.Text = Dt.Rows[0]["Units"].ToString();
            txt_tax.Text = Dt.Rows[0]["Tax"].ToString();
        }
    }

    protected void txt_quntity_TextChanged(object sender, EventArgs e)
    {
        if (txt_quntity.Text == "" || txt_rate.Text == "")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Please fill Rate  !!!');", true);
            //txt_quntity.Focus();            
        }
        else
        {
            TableCalulation();

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
        decimal TaxAmt;
        if (string.IsNullOrEmpty(txt_tax.Text))
        {
            TaxAmt = 0;
        }
        else
        {

            decimal val1 = Convert.ToDecimal(txt_total_amount.Text);
            decimal val2 = Convert.ToDecimal(txt_tax.Text);

            TaxAmt = (val1 * val2 / 100);
        }

        var TotalWithTax = Convert.ToDecimal(txt_total_amount.Text) + (TaxAmt);
        txt_total_amount.Text = TotalWithTax.ToString();

        decimal DiscountAmt;
        if (string.IsNullOrEmpty(txt_discount.Text))
        {
            DiscountAmt = 0;
        }
        else
        {
            decimal val1 = Convert.ToDecimal(TotalWithTax);
            decimal val2 = Convert.ToDecimal(txt_discount.Text);

            DiscountAmt = (val1 * val2 / 100);
        }
        var GrossAmt = Convert.ToDecimal(TotalWithTax) - (DiscountAmt);
        txt_total_amount.Text = GrossAmt.ToString("##.00");
    }

    protected void txt_rate_TextChanged(object sender, EventArgs e)
    {
        if (txt_quntity.Text == "")
        {
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Please fill Quntity !!!');", true);
            txt_tax.Text = "0";
            txt_discount.Text = "0";
        }
        else
        {
            TableCalulation();
        }
    }
    public string ConvertNumbertoWords(int number)
    {
        if (number == 0)
            return "ZERO";
        if (number < 0)
            return "minus " + ConvertNumbertoWords(Math.Abs(number));
        string words = "";
        if ((number / 1000000) > 0)
        {
            words += ConvertNumbertoWords(number / 1000000) + " Million ";
            number %= 1000000;
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
                name = "Fourty";
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
            bool beginsZero = false;//tests for 0XX  
            bool isDone = false;//test if already translated  
            double dblAmt = (Convert.ToDouble(Number));
            //if ((dblAmt > 0) && number.StartsWith("0"))  
            if (dblAmt > 0)
            {//test for zero or digit zero in a nuemric  
                beginsZero = Number.StartsWith("0");

                int numDigits = Number.Length;
                int pos = 0;//store digit grouping  
                String place = "";//digit grouping name:hundres,thousand,etc...  
                switch (numDigits)
                {
                    case 1://ones' range  

                        word = ones(Number);
                        isDone = true;
                        break;
                    case 2://tens' range  
                        word = tens(Number);
                        isDone = true;
                        break;
                    case 3://hundreds' range  
                        pos = (numDigits % 3) + 1;
                        place = " Hundred ";
                        break;
                    case 4://thousands' range  
                    case 5:
                    case 6:
                        pos = (numDigits % 4) + 1;
                        place = " Thousand ";
                        break;
                    case 7://millions' range  
                    case 8:
                        pos = (numDigits % 6) + 1;
                        place = " Lac ";
                        break;
                    case 9:
                        pos = (numDigits % 8) + 1;
                        place = " Million ";
                        break;
                    case 10://Billions's range  
                    case 11:
                    case 12:

                        pos = (numDigits % 10) + 1;
                        place = " Billion ";
                        break;
                    //add extra case options for anything above Billion...  
                    default:
                        isDone = true;
                        break;
                }
                if (!isDone)
                {//if transalation is not done, continue...(Recursion comes in now!!)  
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

                    //check for trailing zeros  
                    //if (beginsZero) word = " and " + word.Trim();  
                }
                //ignore digit grouping names  
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
                    andStr = "and";// just to separate whole numbers from points/cents  
                    endStr = "Paisa " + endStr;//Cents  
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
    private decimal Total;
    decimal Alltotal;
    protected void gvPurchaseRecord_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // cast the current row to a datarowview
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
            txt_subtotal.Text = Alltotal.ToString("##.00");





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

                grd_total = (val1 + val2 + val3);

                //grd_total = (val1);

            }
            txt_grand_total.Text = grd_total.ToString();
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


            //txt_cgst_amt.Text = (totaltax / 2).ToString();
            //txt_sgst_amt.Text = (totaltax / 2).ToString();

        }
    }
    private decimal TTotal;
    protected void grd_getDTLS_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // cast the current row to a datarowview
            DataRowView row = e.Row.DataItem as DataRowView;
            TTotal += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Total"));
            hdnPoProductTot.Value = TTotal.ToString();
            txt_subtotal.Text = TTotal.ToString("##.00");

            //decimal Cgst_Total_Amt;
            //if (string.IsNullOrEmpty(txt_subtotal.Text))
            //{
            //    Cgst_Total_Amt = 0;

            //}
            //else
            //{
            //    decimal val1 = Convert.ToDecimal(txt_subtotal.Text);

            //    Cgst_Total_Amt = (val1 * 9 / 100);
            //}
            //txt_cgst_amt.Text = Cgst_Total_Amt.ToString("##.00");

            //decimal Sgst_Toatl_Amt;
            //if (string.IsNullOrEmpty(txt_subtotal.Text))
            //{
            //    Sgst_Toatl_Amt = 0;

            //}
            //else
            //{
            //    decimal val1 = Convert.ToDecimal(txt_subtotal.Text);

            //    Sgst_Toatl_Amt = (val1 * 9 / 100);
            //}
            //txt_sgst_amt.Text = Sgst_Toatl_Amt.ToString("##.00");

            decimal grd_total;
            if (!string.IsNullOrEmpty(txt_total_amount.Text))
            {
                grd_total = 0;
            }
            else
            {
                decimal val1 = Convert.ToDecimal(txt_subtotal.Text);
                //decimal val2 = Convert.ToDecimal(txt_sgst_amt.Text);
                //decimal val3 = Convert.ToDecimal(txt_cgst_amt.Text);


                grd_total = (val1);

            }
            txt_grand_total.Text = grd_total.ToString();
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
        }

        decimal totaltax = 0;
        decimal Total11177 = 0, Qty = 0, rate111111 = 0, totalrateqty = 0;
        foreach (GridViewRow g1 in grd_getDTLS.Rows)
        {
            string lbltotaltax = (g1.FindControl("lbl_tax_GET") as Label).Text;
            string lblrate = (g1.FindControl("lbl_rate_GET") as Label).Text;
            string lblqty = (g1.FindControl("lbl_quntity_GET") as Label).Text;
            totaltax = Convert.ToDecimal(lbltotaltax);

            rate111111 = Convert.ToDecimal(lblrate);
            Qty = Convert.ToDecimal(lblqty);
            Total11177 = ((Qty * rate111111) * totaltax) / 100;
            totalrateqty += Total11177;
        }
        if (txt_CompanyStateCode.Text == "27 MAHARASHTRA")
        {
            txt_cgst_amt.Text = Convert.ToDecimal(totalrateqty / 2).ToString("##.00");
            txt_sgst_amt.Text = Convert.ToDecimal(totalrateqty / 2).ToString("##.00");
            txt_igst_amt.Text = Convert.ToDecimal(00).ToString("##.00");
        }
        else
        {
            txt_cgst_amt.Text = Convert.ToDecimal(00).ToString("##.00");
            txt_sgst_amt.Text = Convert.ToDecimal(00).ToString("##.00");
            txt_igst_amt.Text = Convert.ToDecimal(totalrateqty).ToString("##.00");
        }
        taxhidden.Value = totalrateqty.ToString();
    }

    protected void btn_save_Click(object sender, EventArgs e)
    {
        if (ChkSendQuotation.Checked == true)
        {
            save_Records();
            string InvoiceNo = ToString();
            Send_Mail();
        }
        else
        {
            save_Records();
        }
       
    }
    protected void save_Records()
    {
        string createdby = Session["adminname"].ToString();
        if (btn_save.Text == "Submit")
        {
            con.Open();
            int Id;
            SqlCommand cmd1 = new SqlCommand("SELECT * FROM [tblsalesProformaHdr] where JobNo='" + txtJobNo.Text + "' AND isdeleted='0'", con);
            SqlDataReader reader = cmd1.ExecuteReader();

            if (reader.Read())
            {
                con.Close();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Data Already Exist.');window.location ='SalesProforma.aspx';", true);
            }
            else
            {
                con.Close();

                if (btn_save.Text == "Submit")
                {
                    //DateTime Date = DateTime.Now;

                    SqlCommand Cmd = new SqlCommand("SP_SalesProformaHdrs", con);
                    Cmd.CommandType = CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("@InvNo", txt_InvoiceNo.Text);
                    Cmd.Parameters.AddWithValue("@invoiceDate", txt_InvoiceDate.Text);
                    Cmd.Parameters.AddWithValue("@JobNo", txtJobNo.Text);
                    Cmd.Parameters.AddWithValue("@orderNo", txtorderno.Text);
                    Cmd.Parameters.AddWithValue("@orderdate", txtorederdate.Text);
                    Cmd.Parameters.AddWithValue("@KindAtt", txt_KindAtt.Text);

                    Cmd.Parameters.AddWithValue("@Cr_Days", txtCrdays.Text);
                    Cmd.Parameters.AddWithValue("@CompanyGstNo", txt_CompanyGSTno.Text);
                    Cmd.Parameters.AddWithValue("@CompanyName", txtCompName.Text);
                    Cmd.Parameters.AddWithValue("@Comanypanno", txt_CompanyPanNo.Text);
                    Cmd.Parameters.AddWithValue("@CompanyAddress", txt_CompanyAddress.Text);
                    Cmd.Parameters.AddWithValue("@Companyregistetype", drop_CompanyRegisterType.Text);
                    Cmd.Parameters.AddWithValue("@isdeleted", '0');
                    Cmd.Parameters.AddWithValue("@CompanyStateCode", txt_CompanyStateCode.Text);
                    Cmd.Parameters.AddWithValue("@CGST", txt_cgst_amt.Text);
                    Cmd.Parameters.AddWithValue("@SGST", txt_sgst_amt.Text);
                    Cmd.Parameters.AddWithValue("@IGST", txt_igst_amt.Text);
                    Cmd.Parameters.AddWithValue("@GradTotal", txt_grand_total.Text);
                    Cmd.Parameters.AddWithValue("@CreatedBy", createdby);
                    Cmd.Parameters.AddWithValue("@Inwordgrandtotal", lbl_Amount_In_Word.Text);
                    Cmd.Parameters.AddWithValue("@Createddate", DateTime.Now);
                    //Cmd.Parameters.AddWithValue("@Companyregistetype", );
                    Cmd.Parameters.AddWithValue("@TermCondition1", txt_term_1.Text + "-" + txt_condition_1.Text);
                    Cmd.Parameters.AddWithValue("@TermCondition2", txt_term_2.Text + "-" + txt_condition_2.Text);
                    Cmd.Parameters.AddWithValue("@TermCondition3", txt_term_3.Text + "-" + txt_condition_3.Text);
                    Cmd.Parameters.AddWithValue("@Termcondition4", txt_term_4.Text + "-" + txt_condition_4.Text);
                    Cmd.Parameters.AddWithValue("@Action", "Insert");
                    //Id = Convert.ToInt32(Cmd.Parameters["@Id"].Value);
                    con.Open();
                    Cmd.ExecuteNonQuery();
                    con.Close();



                    ////Details table insert 
                    foreach (GridViewRow G1 in gvPurchaseRecord.Rows)
                    {
                        string Discription = (G1.FindControl("txt_discription_grd") as Label).Text;
                        string HSN = (G1.FindControl("txt_hsn_grd") as Label).Text;
                        string Tax = (G1.FindControl("lbl_tax_grd") as Label).Text;
                        string Quntity = (G1.FindControl("lbl_quntity_grd") as Label).Text;
                        string Unit = (G1.FindControl("txt_unit_grd") as Label).Text;
                        string Rate = (G1.FindControl("lbl_rate_grd") as Label).Text;
                        string Discount = (G1.FindControl("lbl_discount_grd") as Label).Text;
                        string Total_Amount = (G1.FindControl("lbl_total_amount_grd") as Label).Text;

                        SqlCommand Cmd1 = new SqlCommand("SP_SalesProformaDtls", con);
                        Cmd1.CommandType = CommandType.StoredProcedure;
                        Cmd1.Parameters.AddWithValue("@Invno", txt_InvoiceNo.Text);
                        Cmd1.Parameters.AddWithValue("@JobNo", txtJobNo.Text);
                        Cmd1.Parameters.AddWithValue("@Description", Discription);
                        Cmd1.Parameters.AddWithValue("@Hsn", HSN);
                        Cmd1.Parameters.AddWithValue("@Rate", Rate);
                        Cmd1.Parameters.AddWithValue("@Unit", Unit);
                        Cmd1.Parameters.AddWithValue("@Quantity", Quntity);
                        Cmd1.Parameters.AddWithValue("@Tax", Tax);
                        Cmd1.Parameters.AddWithValue("@Discount", Discount);
                        Cmd1.Parameters.AddWithValue("@TotalAmt", Total_Amount);
                        Cmd1.Parameters.AddWithValue("@Action", "Insert");
                        con.Open();
                        Cmd1.ExecuteNonQuery();
                        con.Close();

                    }

                    foreach (GridViewRow G2 in grd_getDTLS.Rows)
                    {
                        string Discription_GET = (G2.FindControl("txt_discription_GET") as Label).Text;
                        string HSN_GET = (G2.FindControl("txt_hsn_GET") as Label).Text;
                        string Tax_GET = (G2.FindControl("lbl_tax_GET") as Label).Text;
                        string Quntity_GET = (G2.FindControl("lbl_quntity_GET") as Label).Text;
                        string Unit_GET = (G2.FindControl("txt_unit_GET") as Label).Text;
                        string Rate_GET = (G2.FindControl("lbl_rate_GET") as Label).Text;
                        string Discount_GET = (G2.FindControl("lbl_discount_GET") as Label).Text;
                        string Total_Amount_GET = (G2.FindControl("lbl_total_amount_GET") as Label).Text;

                        SqlCommand Cmd2 = new SqlCommand("SP_SalesProformaDtls", con);
                        Cmd2.CommandType = CommandType.StoredProcedure;
                        Cmd2.Parameters.AddWithValue("@Invno", txt_InvoiceNo.Text);
                        Cmd2.Parameters.AddWithValue("@JobNo", txtJobNo.Text);
                        Cmd2.Parameters.AddWithValue("@Description", Discription_GET);
                        Cmd2.Parameters.AddWithValue("@Hsn", HSN_GET);
                        Cmd2.Parameters.AddWithValue("@Rate", Rate_GET);
                        Cmd2.Parameters.AddWithValue("@Unit", Unit_GET);
                        Cmd2.Parameters.AddWithValue("@Quantity", Quntity_GET);
                        Cmd2.Parameters.AddWithValue("@Tax", Tax_GET);
                        Cmd2.Parameters.AddWithValue("@Discount", Discount_GET);
                        Cmd2.Parameters.AddWithValue("@TotalAmt", Total_Amount_GET);
                        Cmd2.Parameters.AddWithValue("@Action", "Insert");
                        con.Open();
                        Cmd2.ExecuteNonQuery();
                        con.Close();

                    }

                    //email insert

                    foreach (GridViewRow g1 in Grd_MAIL.Rows)
                    {
                        string MAIL = (g1.FindControl("lblmultMail") as Label).Text;
                        bool chkmail = (g1.FindControl("chkmail") as CheckBox).Checked;

                        SqlCommand cmdtable = new SqlCommand("insert into tblSalesProformaEmail(InviNo,Email,chkMail) values(@InviNo,@Email,@chkMail)", con);
                        cmdtable.Parameters.AddWithValue("@InviNo", txt_InvoiceNo.Text);
                        cmdtable.Parameters.AddWithValue("@Email", MAIL);
                        cmdtable.Parameters.AddWithValue("@chkMail", chkmail);

                        // cmdtable.Parameters.Add("@Id", Id);

                        con.Open();
                        cmdtable.ExecuteNonQuery();
                        con.Close();
                    }
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data saved Sucessfully');", true);


                }
            }
        }
        else if (btn_save.Text == "Update")
        {
            SqlCommand Cmd = new SqlCommand("SP_SalesProformaHdrs", con);
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.Parameters.AddWithValue("@InvNo", txt_InvoiceNo.Text);
            Cmd.Parameters.AddWithValue("@invoiceDate", txt_InvoiceDate.Text);
            Cmd.Parameters.AddWithValue("@JobNo", txtJobNo.Text);
            Cmd.Parameters.AddWithValue("@orderNo", txtorderno.Text);
            Cmd.Parameters.AddWithValue("@orderdate", txtorederdate.Text);
            Cmd.Parameters.AddWithValue("@KindAtt", txt_KindAtt.Text);
            Cmd.Parameters.AddWithValue("@Cr_Days", txtCrdays.Text);
            Cmd.Parameters.AddWithValue("@CompanyGstNo", txt_CompanyGSTno.Text);
            Cmd.Parameters.AddWithValue("@CompanyName", txtCompName.Text);
            Cmd.Parameters.AddWithValue("@Comanypanno", txt_CompanyPanNo.Text);
            Cmd.Parameters.AddWithValue("@CompanyAddress", txt_CompanyAddress.Text);
            Cmd.Parameters.AddWithValue("@Companyregistetype", drop_CompanyRegisterType.Text);
            Cmd.Parameters.AddWithValue("@isdeleted", '0');
            Cmd.Parameters.AddWithValue("@CompanyStateCode", txt_CompanyStateCode.Text);
            Cmd.Parameters.AddWithValue("@CGST", txt_cgst_amt.Text);
            Cmd.Parameters.AddWithValue("@SGST", txt_sgst_amt.Text);
            Cmd.Parameters.AddWithValue("@IGST", txt_igst_amt.Text);
            Cmd.Parameters.AddWithValue("@GradTotal", txt_grand_total.Text);
            Cmd.Parameters.AddWithValue("@updateby", createdby);
            Cmd.Parameters.AddWithValue("@Inwordgrandtotal", lbl_Amount_In_Word.Text);
            Cmd.Parameters.AddWithValue("@updatedate", DateTime.Now);
            //Cmd.Parameters.AddWithValue("@Companyregistetype", );
            Cmd.Parameters.AddWithValue("@TermCondition1", txt_term_1.Text + "-" + txt_condition_1.Text);
            Cmd.Parameters.AddWithValue("@TermCondition2", txt_term_2.Text + "-" + txt_condition_2.Text);
            Cmd.Parameters.AddWithValue("@TermCondition3", txt_term_3.Text + "-" + txt_condition_3.Text);
            Cmd.Parameters.AddWithValue("@Termcondition4", txt_term_4.Text + "-" + txt_condition_4.Text);
            Cmd.Parameters.AddWithValue("@Action", "Update");
            //Id = Convert.ToInt32(Cmd.Parameters["@Id"].Value);
            con.Open();
            Cmd.ExecuteNonQuery();
            con.Close();

            ///update details table
            DataTable dtdetails = new DataTable();
            SqlDataAdapter sada = new SqlDataAdapter("select * from tblSalesProformDtls where Invno='" + txt_InvoiceNo.Text + "'", con);
            sada.Fill(dtdetails);
            if (dtdetails.Rows.Count == gvPurchaseRecord.Rows.Count)
            {
                foreach (GridViewRow G1 in gvPurchaseRecord.Rows)
                {
                    string Discription = (G1.FindControl("txt_discription_grd") as Label).Text;
                    string HSN = (G1.FindControl("txt_hsn_grd") as Label).Text;
                    string Tax = (G1.FindControl("lbl_tax_grd") as Label).Text;
                    string Quntity = (G1.FindControl("lbl_quntity_grd") as Label).Text;
                    string Unit = (G1.FindControl("txt_unit_grd") as Label).Text;
                    string Rate = (G1.FindControl("lbl_rate_grd") as Label).Text;
                    string Discount = (G1.FindControl("lbl_discount_grd") as Label).Text;
                    string Total_Amount = (G1.FindControl("lbl_total_amount_grd") as Label).Text;

                    SqlCommand Cmd1 = new SqlCommand("SP_SalesProformaDtls", con);
                    Cmd1.CommandType = CommandType.StoredProcedure;
                    Cmd1.Parameters.AddWithValue("@Invno", txt_InvoiceNo.Text);
                    Cmd1.Parameters.AddWithValue("@JobNo", txtJobNo.Text);
                    Cmd1.Parameters.AddWithValue("@Description", Discription);
                    Cmd1.Parameters.AddWithValue("@Hsn", HSN);
                    Cmd1.Parameters.AddWithValue("@Rate", Rate);
                    Cmd1.Parameters.AddWithValue("@Unit", Unit);
                    Cmd1.Parameters.AddWithValue("@Quantity", Quntity);
                    Cmd1.Parameters.AddWithValue("@Tax", Tax);
                    Cmd1.Parameters.AddWithValue("@Discount", Discount);
                    Cmd1.Parameters.AddWithValue("@TotalAmt", Total_Amount);
                    Cmd1.Parameters.AddWithValue("@Action", "Update");
                    con.Open();
                    Cmd1.ExecuteNonQuery();
                    con.Close();
                }
            }
            else
            {
                //delete details table 
                SqlCommand CmdDelete = new SqlCommand("DELETE FROM tblSalesProformDtls WHERE Invno=@Invno", con);
                CmdDelete.Parameters.AddWithValue("@Invno", txt_InvoiceNo.Text);

                CmdDelete.ExecuteNonQuery();
                ////insert details table
                foreach (GridViewRow G1 in gvPurchaseRecord.Rows)
                {
                    string Discription = (G1.FindControl("txt_discription_grd") as Label).Text;
                    string HSN = (G1.FindControl("txt_hsn_grd") as Label).Text;
                    string Tax = (G1.FindControl("lbl_tax_grd") as Label).Text;
                    string Quntity = (G1.FindControl("lbl_quntity_grd") as Label).Text;
                    string Unit = (G1.FindControl("txt_unit_grd") as Label).Text;
                    string Rate = (G1.FindControl("lbl_rate_grd") as Label).Text;
                    string Discount = (G1.FindControl("lbl_discount_grd") as Label).Text;
                    string Total_Amount = (G1.FindControl("lbl_total_amount_grd") as Label).Text;

                    SqlCommand Cmd1 = new SqlCommand("SP_SalesProformaDtls", con);
                    Cmd1.CommandType = CommandType.StoredProcedure;
                    Cmd1.Parameters.AddWithValue("@Invno", txt_InvoiceNo.Text);
                    Cmd1.Parameters.AddWithValue("@JobNo", txtJobNo.Text);
                    Cmd1.Parameters.AddWithValue("@Description", Discription);
                    Cmd1.Parameters.AddWithValue("@Hsn", HSN);
                    Cmd1.Parameters.AddWithValue("@Rate", Rate);
                    Cmd1.Parameters.AddWithValue("@Unit", Unit);
                    Cmd1.Parameters.AddWithValue("@Quantity", Quntity);
                    Cmd1.Parameters.AddWithValue("@Tax", Tax);
                    Cmd1.Parameters.AddWithValue("@Discount", Discount);
                    Cmd1.Parameters.AddWithValue("@TotalAmt", Total_Amount);
                    Cmd1.Parameters.AddWithValue("@Action", "Insert");
                    con.Open();
                    Cmd1.ExecuteNonQuery();
                    con.Close();

                }
            }

            ///email updating
            SqlDataAdapter Sda = new SqlDataAdapter("SELECT * FROM tblSalesProformaEmail WHERE InviNo='" + txt_InvoiceNo.Text + "'", con);
            DataTable DTMAIL = new DataTable();
            Sda.Fill(DTMAIL);
           
            foreach (GridViewRow g1 in Grd_MAIL.Rows)
            {
                
                string MAIL = (g1.FindControl("lblmultMail") as Label).Text;
                bool chkmail = (g1.FindControl("chkmail") as CheckBox).Checked;

                SqlCommand cmdtable = new SqlCommand("UPDATE tblSalesProformaEmail SET InviNo=@InviNo,Email=@Email,chkMail=@chkMail WHERE InviNo=@InviNo AND Email=@Email", con);
                cmdtable.Parameters.AddWithValue("@InviNo", txt_InvoiceNo.Text);
                cmdtable.Parameters.AddWithValue("@Email", MAIL);
                cmdtable.Parameters.AddWithValue("@chkMail", chkmail);
                con.Open();
                cmdtable.ExecuteNonQuery();
                con.Close();
            }
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Updated Sucessfully');", true);
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

        decimal grd_total;
        if (!string.IsNullOrEmpty(txt_total_amount.Text))
        {
            grd_total = 0;
        }
        else
        {
            decimal val1 = Convert.ToDecimal(txt_subtotal.Text);
            //decimal val2 = Convert.ToDecimal(txt_sgst_amt.Text);
            //decimal val3 = Convert.ToDecimal(txt_cgst_amt.Text);


            grd_total = (val1);

            //var grd = grd_total - val5;
        }
        txt_grand_total.Text = grd_total.ToString();
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

        if (hdnPoProductTot.Value != "")
        {
            decimal totaltax = 0, totalgrdtax = 0;
            decimal Total11177 = 0, Qty = 0, rate111111 = 0, totalrateqty = 0;
            foreach (GridViewRow g1 in gvPurchaseRecord.Rows)
            {
                string lbltotaltax = (g1.FindControl("lbl_tax_grd") as Label).Text;
                string lblrate = (g1.FindControl("lbl_rate_grd") as Label).Text;
                string lblqty = (g1.FindControl("lbl_quntity_grd") as Label).Text;
                totaltax = Convert.ToDecimal(lbltotaltax);

                rate111111 = Convert.ToDecimal(lblrate);
                Qty = Convert.ToDecimal(lblqty);
                Total11177 = ((Qty * rate111111) * totaltax) / 100;
                totalrateqty += Total11177;
            }
            totalgrdtax = totalrateqty + Convert.ToDecimal(taxhidden.Value);
            if (txt_CompanyStateCode.Text == "27 MAHARASHTRA")
            {
                txt_cgst_amt.Text = Convert.ToDecimal(totalgrdtax / 2).ToString("##.00");
                txt_sgst_amt.Text = Convert.ToDecimal(totalgrdtax / 2).ToString("##.00");
                txt_igst_amt.Text = Convert.ToDecimal(00).ToString("##.00");
            }
            else
            {
                txt_cgst_amt.Text = Convert.ToDecimal(00).ToString("##.00");
                txt_sgst_amt.Text = Convert.ToDecimal(00).ToString("##.00");
                txt_igst_amt.Text = Convert.ToDecimal(totalgrdtax).ToString("##.00");
            }


        }
        else
        {
            decimal totaltax = 0;
            decimal Total11177 = 0, Qty = 0, rate111111 = 0, totalrateqty = 0;
            foreach (GridViewRow g1 in gvPurchaseRecord.Rows)
            {
                string lbltotaltax = (g1.FindControl("lbl_tax_grd") as Label).Text;
                string lblrate = (g1.FindControl("lbl_rate_grd") as Label).Text;
                string lblqty = (g1.FindControl("lbl_quntity_grd") as Label).Text;
                totaltax = Convert.ToDecimal(lbltotaltax);

                rate111111 = Convert.ToDecimal(lblrate);
                Qty = Convert.ToDecimal(lblqty);
                Total11177 = ((Qty * rate111111) * totaltax) / 100;
                totalrateqty += Total11177;
            }
            if (txt_CompanyStateCode.Text == "27 MAHARASHTRA")
            {
                txt_cgst_amt.Text = Convert.ToDecimal(totalrateqty / 2).ToString("##.00");
                txt_sgst_amt.Text = Convert.ToDecimal(totalrateqty / 2).ToString("##.00");
                txt_igst_amt.Text = Convert.ToDecimal(00).ToString("##.00");
            }
            else
            {
                txt_cgst_amt.Text = Convert.ToDecimal(00).ToString("##.00");
                txt_sgst_amt.Text = Convert.ToDecimal(00).ToString("##.00");
                txt_igst_amt.Text = Convert.ToDecimal(totalrateqty).ToString("##.00");
            }
        }

        ScriptManager.RegisterStartupScript(this, this.GetType(), "Success", "scrollToElement();", true);
    }

    protected void gv_cancel_Click(object sender, EventArgs e)
    {
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

    protected void gvPurchaseRecord_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvPurchaseRecord.EditIndex = e.NewEditIndex;
        gvPurchaseRecord.DataSource = (DataTable)ViewState["Invoice"];
        gvPurchaseRecord.DataBind();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Success", "scrollToElement();", true);
    }

    protected void txt_quntity_grd_TextChanged(object sender, EventArgs e)
    {
        GridViewRow row = (sender as TextBox).NamingContainer as GridViewRow;

        GriCalculation(row);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Success", "scrollToElement();", true);
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

        decimal TaxAmt;
        if (string.IsNullOrEmpty(Tax.Text))
        {
            TaxAmt = 0;
        }
        else
        {
            decimal val1 = Convert.ToDecimal(TotalAmount.Text);
            decimal val2 = Convert.ToDecimal(Tax.Text);

            TaxAmt = (val1 * val2 / 100);
        }

        //txt_tax.Text = TaxAmt.ToString();
        var TotalWithTax = Convert.ToDecimal(TotalAmount.Text) + (TaxAmt);
        TotalAmount.Text = TotalWithTax.ToString();

        decimal DiscountAmt;
        if (string.IsNullOrEmpty(Discount.Text))
        {
            DiscountAmt = 0;
        }
        else
        {
            decimal val1 = Convert.ToDecimal(TotalWithTax);
            decimal val2 = Convert.ToDecimal(Discount.Text);

            DiscountAmt = (val1 * val2 / 100);
        }
        var GrossAmt = Convert.ToDecimal(TotalWithTax) - (DiscountAmt);
        TotalAmount.Text = GrossAmt.ToString("##.00");
    }

    protected void txt_tax_grd_TextChanged(object sender, EventArgs e)
    {
        GridViewRow row = (sender as TextBox).NamingContainer as GridViewRow;

        GriCalculation(row);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Success", "scrollToElement();", true);
    }

    protected void txt_discount_grd_TextChanged(object sender, EventArgs e)
    {
        GridViewRow row = (sender as TextBox).NamingContainer as GridViewRow;

        GriCalculation(row);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Success", "scrollToElement();", true);

    }

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
                SqlCommand cmd = new SqlCommand("select chkMail from tblSalesProformaEmail where InviNo='" + txt_InvoiceNo.Text + "' AND Id='" + id + "'", con);

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {

                    chkupdate = dr["chkMail"].ToString();
                    con.Close();
                }
                chkmailupdate.Checked = chkupdate == "True" ? true : false;
            }
        }
    }

    protected void btn_Cancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("SalesProformaList.aspx");
    }

    private MemoryStream PDF(string InvoiceNo, string message)
    {
        MemoryStream pdf = new MemoryStream();



        foreach (GridViewRow g1 in Grd_MAIL.Rows)
        {
            string MAIL = (g1.FindControl("lblmultMail") as Label).Text;
            bool chkmail = (g1.FindControl("chkmail") as CheckBox).Checked;
            if (chkmail == true)
            {
                SqlDataAdapter Da = new SqlDataAdapter("SELECT * FROM vw_SalesProformaPDF WHERE InvNo='" + txt_InvoiceNo.Text + "' AND Email='" + MAIL + "'   ", con);


                DataTable Dt = new DataTable();
                Da.Fill(Dt);

                StringWriter sw = new StringWriter();
                StringReader sr = new StringReader(sw.ToString());



                Document doc = new Document(PageSize.A4, 10f, 10f, 55f, 0f);
                PdfWriter pdfWriter = PdfWriter.GetInstance(doc, pdf);

                PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(Server.MapPath("~/Files/") + "ProformaInvoice.pdf", FileMode.Create));
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
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Survey No. 27, Nilamba Nagar, Near Raghunandan Karyalay,", 155, 755, 0);
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
                    string Invoiceno = Dt.Rows[0]["InvNo"].ToString();
                    string CompanyName = Dt.Rows[0]["CompanyName"].ToString();
                    string Address = Dt.Rows[0]["CompanyAddress"].ToString();
                    string InvoiceDate = Dt.Rows[0]["invoiceDate"].ToString().TrimEnd("0:0".ToCharArray());

                    string orderNo = Dt.Rows[0]["orderNo"].ToString();
                    string GSTNo = Dt.Rows[0]["CompanyGstNo"].ToString();
                    string KindAtt = Dt.Rows[0]["KindAtt"].ToString();
                    string TotalInWord = Dt.Rows[0]["Inwordgrandtotal"].ToString();
                    string GrandTotal = Dt.Rows[0]["GradTotal"].ToString();
                    string CGST = Dt.Rows[0]["CGST"].ToString();
                    string SGST = Dt.Rows[0]["SGST"].ToString();
                    string IGST = Dt.Rows[0]["IGST"].ToString();
                    string Total = Dt.Rows[0]["TotalAmt"].ToString();

                    table.AddCell(new Phrase("Invoice Number : ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(Invoiceno, FontFactory.GetFont("Arial", 9, Font.BOLD)));

                    table.AddCell(new Phrase("Invoice Date :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(InvoiceDate, FontFactory.GetFont("Arial", 9, Font.BOLD)));

                    table.AddCell(new Phrase("Company Name :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(CompanyName, FontFactory.GetFont("Arial", 9, Font.BOLD)));

                    table.AddCell(new Phrase("Address", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(Address, FontFactory.GetFont("Arial", 9, Font.BOLD)));

                    table.AddCell(new Phrase("Order No :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase(orderNo, FontFactory.GetFont("Arial", 9, Font.BOLD)));

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

                            double Ftotal = Convert.ToDouble(dr["TotalAmt"].ToString());
                            string _ftotal = Ftotal.ToString("##.00");
                            table.AddCell(new Phrase(rowid.ToString(), FontFactory.GetFont("Arial", 9)));
                            table.AddCell(new Phrase(dr["Description"].ToString(), FontFactory.GetFont("Arial", 9)));
                            table.AddCell(new Phrase(dr["Hsn"].ToString(), FontFactory.GetFont("Arial", 9)));
                            table.AddCell(new Phrase(dr["Tax"].ToString(), FontFactory.GetFont("Arial", 9)));
                            table.AddCell(new Phrase(dr["Quantity"].ToString(), FontFactory.GetFont("Arial", 9)));
                            table.AddCell(new Phrase(dr["Unit"].ToString(), FontFactory.GetFont("Arial", 9)));
                            table.AddCell(new Phrase(dr["Rate"].ToString(), FontFactory.GetFont("Arial", 9)));
                            table.AddCell(new Phrase(dr["Discount"].ToString(), FontFactory.GetFont("Arial", 9)));
                            table.AddCell(new Phrase(_ftotal, FontFactory.GetFont("Arial", 9)));
                            rowid++;

                            Ttotal_price += Convert.ToDouble(dr["TotalAmt"].ToString());
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
                    //PdfPCell cell = new PdfPCell(new Phrase("Sub Total", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                    //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //table.AddCell(cell);
                    //PdfPCell cell11 = new PdfPCell(new Phrase(Ttotal_price.ToString("##.00"), FontFactory.GetFont("Arial", 10, Font.BOLD)));
                    //cell11.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //table.AddCell(cell11);


                    doc.Add(table);
                    //add total row end

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
                     
                    //var Sgst_9 = Convert.ToDecimal(Ttotal_price) * 9 / 100;


                    table.SetWidths(new float[] { 0f, 76f, 12f });
                    table.AddCell(paragraph);
                    PdfPCell cell22igst = new PdfPCell(new Phrase("IGST %", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                    cell22igst.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(cell22igst);
                    PdfPCell cell33igst = new PdfPCell(new Phrase(IGST, FontFactory.GetFont("Arial", 10, Font.BOLD)));
                    cell33igst.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(cell33igst);


                    doc.Add(table);
                    //IGST 9% Row End

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
                    PdfPCell cell55 = new PdfPCell(new Phrase(Total, FontFactory.GetFont("Arial", 10, Font.BOLD)));
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


                    PdfContentByte cn = pdfWriter.DirectContent;
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

                    //doc.Close();


                    //Byte[] FileBuffer = File.ReadAllBytes(Server.MapPath("~/Files/") + "QuotationInvoice.pdf");

                    //if (FileBuffer != null)
                    //{
                    //    Response.ContentType = "application/pdf";
                    //    Response.AddHeader("content-length", FileBuffer.Length.ToString());
                    //    Response.BinaryWrite(FileBuffer);
                    //    Response.AddHeader("Content-Disposition", "attachment;filename=myfilename.pdf");
                    //}

                    pdfWriter.CloseStream = false;
                    doc.Close();
                    pdf.Position = 0;
                }
            }
        }
        return pdf;

    }
    protected void Send_Mail()
    {
        string strMessage = "Hello " + txtCompName.Text.Trim() + "<br/>" +


                        "Greetings From " + "<strong>ENDEAVOUR AUTOMATION<strong>" + "<br/>" +
                        "We sent you an Tax Invoice." + "Tax - " + txt_InvoiceNo.Text.Trim() + "/" + txt_InvoiceDate.Text.Trim() + ".pdf" + "<br/>" +

                         "We Look Foward to Conducting Future Business with you." + "<br/>" +

                        "Kind Regards," + "<br/>" +
                        "<strong>ENDEAVOUR AUTOMATION<strong>";
        string pdfname = "Salesproforma - " + txt_InvoiceNo.Text.Trim() + "/" + txt_InvoiceDate.Text.Trim() + ".pdf";

        MailMessage message = new MailMessage();
        MailMessage msgendeaour = new MailMessage();
        MailMessage msgenaccount = new MailMessage();
        msgendeaour.To.Add("shwetawalunj98@gmail.com");
        msgenaccount.To.Add("shwetawalunj98@gmail.com");
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
        message.Subject = "Proforma Invoice";// Subject of Email  
        message.Body = strMessage;
        msgendeaour.Subject = "Proforma Invoice";// Subject of Email  
        msgendeaour.Body = strMessage;
        msgenaccount.Subject = "Proforma Invoice";// Subject of Email  
        msgenaccount.Body = strMessage;



        message.From = new System.Net.Mail.MailAddress("enquiry@weblinkservices.net");// Email-ID of Sender  
        message.IsBodyHtml = true;
        msgendeaour.From = new System.Net.Mail.MailAddress("enquiry@weblinkservices.net");// Email-ID of Sender  
        msgendeaour.IsBodyHtml = true;
        msgenaccount.From = new System.Net.Mail.MailAddress("enquiry@weblinkservices.net");// Email-ID of Sender  
        msgenaccount.IsBodyHtml = true;

        MemoryStream file = new MemoryStream(PDF("This is pdf file text", Server.MapPath("~/Files/")).ToArray());

        file.Seek(0, SeekOrigin.Begin);
        Attachment data = new Attachment(file, pdfname, "application/pdf");
        ContentDisposition disposition = data.ContentDisposition;
        disposition.CreationDate = System.DateTime.Now;
        disposition.ModificationDate = System.DateTime.Now;
        disposition.DispositionType = DispositionTypeNames.Attachment;
        message.Attachments.Add(data);//Attach the file  
        msgendeaour.Attachments.Add(data);//Attach the file
        msgenaccount.Attachments.Add(data);//Attach the file

        //message.Body = txtmessagebody.Text;
        SmtpClient SmtpMail = new SmtpClient();
        SmtpMail.Host = "smtpout.secureserver.net";//name or IP-Address of Host used for SMTP transactions  
        SmtpMail.Port = 587;//Port for sending the mail  
        SmtpMail.Credentials = new System.Net.NetworkCredential("enquiry@weblinkservices.net", "wlspl@123");//username/password of network, if apply  
        SmtpMail.DeliveryMethod = SmtpDeliveryMethod.Network;
        SmtpMail.EnableSsl = false;
        SmtpMail.ServicePoint.MaxIdleTime = 0;
        SmtpMail.ServicePoint.SetTcpKeepAlive(true, 2000, 2000);
        message.BodyEncoding = Encoding.Default;
        message.Priority = MailPriority.High;
        SmtpMail.Send(message); //Smtpclient to send the mail message  
        msgendeaour.BodyEncoding = Encoding.Default;
        msgendeaour.Priority = MailPriority.High;
        SmtpMail.Send(msgendeaour); //Smtpclient to send the mail message 
        msgenaccount.BodyEncoding = Encoding.Default;
        msgenaccount.Priority = MailPriority.High;
        SmtpMail.Send(msgenaccount); //Smtpclient to send the mail message 
        Response.Write("Email has been sent");

    } 

}