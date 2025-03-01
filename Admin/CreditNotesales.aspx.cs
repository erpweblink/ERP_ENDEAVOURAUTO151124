﻿using iTextSharp.text;
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


public partial class Admin_CreditNotesales : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
    DataTable Dt_Items = new DataTable();
    string ID;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.txtvochardate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            this.txtvochardate.TextMode = TextBoxMode.Date;
            this.txtorderdate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            this.txtorderdate.TextMode = TextBoxMode.Date;
            GenerateChallan();

            ////purchasedescription grid view temp table
            ViewState["RowNo"] = 0;
            Dt_Items.Columns.AddRange(new DataColumn[9] { new DataColumn("ID"), new DataColumn("Description"), new DataColumn("HSN/SAC"), new DataColumn("Tax"), new DataColumn("Quntity"), new DataColumn("Unit"), new DataColumn("Rate"), new DataColumn("Discount"), new DataColumn("TotalAmount") });
            ViewState["Invoice"] = Dt_Items;

            if (Request.QueryString["Voc_no"] != null)
            {
                ID = Decrypt(Request.QueryString["Voc_no"].ToString());
                loadData(ID);
                btn_save.Text = "Update";

            }
        }
    }
    protected void loadData(string id)
    {
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblCreditNoteSalesHdrs where Voc_no='" + ID + "'", con);
        sad.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            txtJobNo.ReadOnly = true;
            txtVocNo.Text = dt.Rows[0]["Voc_no"].ToString();
            txtrefNo.Text = dt.Rows[0]["Ref_No"].ToString();
            txtorderno.Text = dt.Rows[0]["OrderNo"].ToString();
            txtnarration.Text = dt.Rows[0]["Narration"].ToString();
            txtCompName.Text = dt.Rows[0]["CustomerName"].ToString();
            txt_CompanyGSTno.Text = dt.Rows[0]["GSTNo"].ToString();
            txtStateCode.Text = dt.Rows[0]["StatrCode"].ToString();
            txt_CompanyAddress.Text = dt.Rows[0]["Address"].ToString();
            txtmobileno.Text = dt.Rows[0]["MobileNo"].ToString();
            txtJobNo.Text = dt.Rows[0]["JobNo"].ToString();
            ddldropdownlist.Text = dt.Rows[0]["Type"].ToString();
            txt_cgst_amt.Text = dt.Rows[0]["CGST"].ToString();
            txt_sgst_amt.Text = dt.Rows[0]["SGST"].ToString();
            txt_igst_amt.Text = dt.Rows[0]["IGST"].ToString();
            txt_grand_total.Text = dt.Rows[0]["GrandTotal"].ToString();
            lbl_Amount_In_Word.Text = dt.Rows[0]["InwordAmount"].ToString();
            DateTime ffff2 = Convert.ToDateTime(dt.Rows[0]["VocDate"].ToString());
            txtvochardate.Text = ffff2.ToString("yyyy-MM-dd");
            DateTime ffff3 = Convert.ToDateTime(dt.Rows[0]["orderDate"].ToString());
            txtorderdate.Text = ffff3.ToString("yyyy-MM-dd");
        }
        ////load data in details table
        DataTable Dt_Product = new DataTable();
        SqlDataAdapter daa = new SqlDataAdapter("SELECT  Description,Hsn,Tax,Quntity,Unit,Rate,Discount,TotalAmt FROM tblCreditNoteSalesDtls WHERE Voc_No='" + txtVocNo.Text + "'", con);
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
                Dt_Items.Rows.Add(Count, Dt_Product.Rows[i]["Description"].ToString(), Dt_Product.Rows[i]["Hsn"].ToString(), Dt_Product.Rows[i]["Tax"].ToString(), Dt_Product.Rows[i]["Quntity"].ToString(), Dt_Product.Rows[i]["Unit"].ToString(), Dt_Product.Rows[i]["Rate"].ToString(), Dt_Product.Rows[i]["Discount"].ToString(), Dt_Product.Rows[i]["TotalAmt"].ToString());
                Count = Count + 1;
            }
        }
        gvPurchaseRecord.DataSource = Dt_Items;
        gvPurchaseRecord.DataBind();

        ////Email Load 
        SqlDataAdapter Sda = new SqlDataAdapter("SELECT * FROM tblCreditSalesEmail WHERE VocNo='" + txtVocNo.Text + "'", con);
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
    protected void GenerateChallan()
    {
        SqlDataAdapter ad = new SqlDataAdapter("SELECT max([Id]) as maxid FROM [tblCreditNoteSalesHdrs]", con);
        DataTable dt = new DataTable();
        ad.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            int maxid = dt.Rows[0]["maxid"].ToString() == "" ? 0 : Convert.ToInt32(dt.Rows[0]["maxid"].ToString());
            txtVocNo.Text = "VOC-" + (maxid + 1).ToString();
        }
        else
        {
            txtVocNo.Text = string.Empty;
        }
    }

    protected void txtCompName_TextChanged(object sender, EventArgs e)
    {
        try
        {
            ///customer name wise data fetch automatically
            DataTable dt = new DataTable();
            SqlDataAdapter sad = new SqlDataAdapter("select * from tblCustomer where CustomerName='" + txtCompName.Text + "'", con);
            sad.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                txt_CompanyGSTno.Text = dt.Rows[0]["GSTNo"].ToString();
                txt_CompanyAddress.Text = dt.Rows[0]["AddresLine1"].ToString();
                txtmobileno.Text = dt.Rows[0]["MobNo"].ToString();
                txtStateCode.Text = dt.Rows[0]["StateCode"].ToString();

            }

            /////Email fetch into customer name
            DataTable dt2 = new DataTable();
            SqlDataAdapter sad2 = new SqlDataAdapter("select * from tblCustomerContactPerson where CustName='" + txtCompName.Text + "'", con);
            sad2.Fill(dt2);
            Grd_MAIL.DataSource = dt2;
            Grd_MAIL.DataBind();
            Grd_MAIL.EmptyDataText = "Record Not Found";
        }
        catch (Exception)
        {

            throw;
        }
    }
    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetCustomerList(string prefixText, int count)
    {
        return AutoFillcustomerlist(prefixText);
    }

    public static List<string> AutoFillcustomerlist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT CustomerName from tblCustomer where " + "CustomerName like @Search + '%' AND isdeleted='0'";

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

    protected void txt_discription_TextChanged(object sender, EventArgs e)
    {
        try
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
        catch (Exception)
        {

            throw;
        }

    }

    protected void txt_rate_TextChanged(object sender, EventArgs e)
    {
        if (txt_quntity.Text == "")
        {

            txt_tax.Text = "0";
            txt_discount.Text = "0";
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
        if (txt_discription.Text == "" || txt_hsn.Text == "" || txt_rate.Text == "" || txt_quntity.Text == "")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Please fill Component Information  !!!');", true);
            //txt_quntity.Focus();
        }
        else
        {
            ShowGrid();
        }
        taxcalculation();
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
    protected void taxcalculation()
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
        if (txtStateCode.Text == "27 MAHARASHTRA")
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

    protected void gvPurchaseRecord_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvPurchaseRecord.EditIndex = e.NewEditIndex;
        gvPurchaseRecord.DataSource = (DataTable)ViewState["Invoice"];
        gvPurchaseRecord.DataBind();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Success", "scrollToElement();", true);
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
        taxcalculation();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Success", "scrollToElement();", true);
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

    protected void txt_rate_grd_TextChanged(object sender, EventArgs e)
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


    protected void txt_quntity_grd_TextChanged(object sender, EventArgs e)
    {
        GridViewRow row = (sender as TextBox).NamingContainer as GridViewRow;

        GriCalculation(row);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Success", "scrollToElement();", true);
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
    }

    protected void btn_save_Click(object sender, EventArgs e)
    {

        SaveRecord();

    }
    protected void SaveRecord()
    {
        string createdby = Session["adminname"].ToString();
        try
        {

            if (btn_save.Text == "Submit")
            {

                SqlCommand Cmd = new SqlCommand("SP_CreditNoteSales", con);
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.AddWithValue("@Voc_No", txtVocNo.Text);
                Cmd.Parameters.AddWithValue("@VocDate", txtvochardate.Text);
                Cmd.Parameters.AddWithValue("@Ref_No", txtrefNo.Text);
                Cmd.Parameters.AddWithValue("@OrderNo", txtorderno.Text);
                Cmd.Parameters.AddWithValue("@orderDate", txtorderdate.Text);
                Cmd.Parameters.AddWithValue("@Narration", txtnarration.Text);
                Cmd.Parameters.AddWithValue("@CustomerName", txtCompName.Text);
                Cmd.Parameters.AddWithValue("@GSTNo", txt_CompanyGSTno.Text);
                Cmd.Parameters.AddWithValue("@StatrCode", txtStateCode.Text);

                Cmd.Parameters.AddWithValue("@Address", txt_CompanyAddress.Text);
                Cmd.Parameters.AddWithValue("@MobileNo", txtmobileno.Text);
                Cmd.Parameters.AddWithValue("@JobNo", txtJobNo.Text);
                Cmd.Parameters.AddWithValue("@Type", ddldropdownlist.Text);
                Cmd.Parameters.AddWithValue("@CGST", txt_cgst_amt.Text);
                Cmd.Parameters.AddWithValue("@SGST", txt_sgst_amt.Text);
                Cmd.Parameters.AddWithValue("@IGST", txt_igst_amt.Text);
                Cmd.Parameters.AddWithValue("@CreatedBy", createdby);
                Cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                Cmd.Parameters.AddWithValue("@GrandTotal", txt_grand_total.Text);
                Cmd.Parameters.AddWithValue("@isdeleted", '0');
                Cmd.Parameters.AddWithValue("@InwordAmount", lbl_Amount_In_Word.Text);
                Cmd.Parameters.AddWithValue("@Action", "Insert");
                con.Open();
                Cmd.ExecuteNonQuery();
                con.Close();

                ////Insert description in details tables
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

                    SqlCommand Cmd1 = new SqlCommand("SP_CreditNotesSalesDtls", con);
                    Cmd1.CommandType = CommandType.StoredProcedure;
                    Cmd1.Parameters.AddWithValue("@Voc_No", txtVocNo.Text);
                    //Cmd1.Parameters.AddWithValue("@JobNo", txtJobNo.Text);
                    Cmd1.Parameters.AddWithValue("@Description", Discription);
                    Cmd1.Parameters.AddWithValue("@Hsn", HSN);
                    Cmd1.Parameters.AddWithValue("@Rate", Rate);
                    Cmd1.Parameters.AddWithValue("@Unit", Unit);
                    Cmd1.Parameters.AddWithValue("@Quntity", Quntity);
                    Cmd1.Parameters.AddWithValue("@Tax", Tax);
                    Cmd1.Parameters.AddWithValue("@Discount", Discount);
                    Cmd1.Parameters.AddWithValue("@TotalAmt", Total_Amount);
                    Cmd1.Parameters.AddWithValue("@Action", "Insert");
                    con.Open();
                    Cmd1.ExecuteNonQuery();
                    con.Close();
                }
                ////insert mail in mail table
                foreach (GridViewRow g1 in Grd_MAIL.Rows)
                {
                    string MAIL = (g1.FindControl("lblmultMail") as Label).Text;
                    // bool chkmail = (g1.FindControl("chkmail") as CheckBox).Checked;

                    SqlCommand cmdtable = new SqlCommand("insert into tblCreditSalesEmail(VocNo,Email) values(@VocNo,@Email)", con);
                    cmdtable.Parameters.AddWithValue("@VocNo", txtVocNo.Text);
                    cmdtable.Parameters.AddWithValue("@Email", MAIL);
                    // cmdtable.Parameters.AddWithValue("@chkMail", chkmail);

                    // cmdtable.Parameters.Add("@Id", Id);

                    con.Open();
                    cmdtable.ExecuteNonQuery();
                    con.Close();
                }
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data saved Sucessfully');", true);
            }
            if (btn_save.Text == "Update")
            {

                SqlCommand Cmd = new SqlCommand("SP_CreditNoteSales", con);
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.AddWithValue("@Voc_No", txtVocNo.Text);
                Cmd.Parameters.AddWithValue("@VocDate", txtvochardate.Text);
                Cmd.Parameters.AddWithValue("@Ref_No", txtrefNo.Text);
                Cmd.Parameters.AddWithValue("@OrderNo", txtorderno.Text);
                Cmd.Parameters.AddWithValue("@orderDate", txtorderdate.Text);
                Cmd.Parameters.AddWithValue("@Narration", txtnarration.Text);
                Cmd.Parameters.AddWithValue("@CustomerName", txtCompName.Text);
                Cmd.Parameters.AddWithValue("@GSTNo", txt_CompanyGSTno.Text);
                Cmd.Parameters.AddWithValue("@StatrCode", txtStateCode.Text);

                Cmd.Parameters.AddWithValue("@Address", txt_CompanyAddress.Text);
                Cmd.Parameters.AddWithValue("@MobileNo", txtmobileno.Text);
                Cmd.Parameters.AddWithValue("@JobNo", txtJobNo.Text);
                Cmd.Parameters.AddWithValue("@Type", ddldropdownlist.Text);
                Cmd.Parameters.AddWithValue("@CGST", txt_cgst_amt.Text);
                Cmd.Parameters.AddWithValue("@SGST", txt_sgst_amt.Text);
                Cmd.Parameters.AddWithValue("@IGST", txt_igst_amt.Text);
                Cmd.Parameters.AddWithValue("@UpdatedBy", createdby);
                Cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                Cmd.Parameters.AddWithValue("@GrandTotal", txt_grand_total.Text);
                Cmd.Parameters.AddWithValue("@isdeleted", '0');
                Cmd.Parameters.AddWithValue("@InwordAmount", lbl_Amount_In_Word.Text);
                Cmd.Parameters.AddWithValue("@Action", "Update");
                con.Open();
                Cmd.ExecuteNonQuery();
                con.Close();
                ///update details table
                DataTable dtdetails = new DataTable();
                SqlDataAdapter sada = new SqlDataAdapter("select * from tblCreditNoteSalesDtls where Voc_No='" + txtVocNo.Text + "'", con);
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

                        SqlCommand Cmd1 = new SqlCommand("SP_CreditNotesSalesDtls", con);
                        Cmd1.CommandType = CommandType.StoredProcedure;
                        Cmd1.Parameters.AddWithValue("@Voc_No", txtVocNo.Text);
                        //Cmd1.Parameters.AddWithValue("@JobNo", txtJobNo.Text);
                        Cmd1.Parameters.AddWithValue("@Description", Discription);
                        Cmd1.Parameters.AddWithValue("@Hsn", HSN);
                        Cmd1.Parameters.AddWithValue("@Rate", Rate);
                        Cmd1.Parameters.AddWithValue("@Unit", Unit);
                        Cmd1.Parameters.AddWithValue("@Quntity", Quntity);
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
                    SqlCommand CmdDelete = new SqlCommand("DELETE FROM tblCreditNoteSalesDtls WHERE Voc_No=@Voc_No", con);
                    CmdDelete.Parameters.AddWithValue("@Voc_No", txtVocNo.Text);
                    con.Open();
                    CmdDelete.ExecuteNonQuery();
                    con.Close();
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

                        SqlCommand Cmd1 = new SqlCommand("SP_CreditNotesSalesDtls", con);
                        Cmd1.CommandType = CommandType.StoredProcedure;
                        Cmd1.Parameters.AddWithValue("@Voc_No", txtVocNo.Text);
                        //Cmd1.Parameters.AddWithValue("@JobNo", txtJobNo.Text);
                        Cmd1.Parameters.AddWithValue("@Description", Discription);
                        Cmd1.Parameters.AddWithValue("@Hsn", HSN);
                        Cmd1.Parameters.AddWithValue("@Rate", Rate);
                        Cmd1.Parameters.AddWithValue("@Unit", Unit);
                        Cmd1.Parameters.AddWithValue("@Quntity", Quntity);
                        Cmd1.Parameters.AddWithValue("@Tax", Tax);
                        Cmd1.Parameters.AddWithValue("@Discount", Discount);
                        Cmd1.Parameters.AddWithValue("@TotalAmt", Total_Amount);
                        Cmd1.Parameters.AddWithValue("@Action", "Insert");
                        con.Open();
                        Cmd1.ExecuteNonQuery();
                        con.Close();

                    }
                }

                ///email update
                SqlDataAdapter Sda = new SqlDataAdapter("SELECT * FROM tblCreditSalesEmail WHERE VocNo='" + txtVocNo.Text + "'", con);
                DataTable DTMAIL = new DataTable();
                Sda.Fill(DTMAIL);

                foreach (GridViewRow g1 in Grd_MAIL.Rows)
                {

                    string MAIL = (g1.FindControl("lblmultMail") as Label).Text;
                    // bool chkmail = (g1.FindControl("chkmail") as CheckBox).Checked;

                    SqlCommand cmdtable = new SqlCommand("UPDATE tblCreditSalesEmail SET VocNo=@VocNo,Email=@Email WHERE VocNo=@VocNo AND Email=@Email", con);
                    cmdtable.Parameters.AddWithValue("@VocNo", txtVocNo.Text);
                    cmdtable.Parameters.AddWithValue("@Email", MAIL);
                    // cmdtable.Parameters.AddWithValue("@chkMail", chkmail);
                    con.Open();
                    cmdtable.ExecuteNonQuery();
                    con.Close();
                }

                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Updated Sucessfully');", true);
            }
        }
        catch (Exception)
        {

            throw;
        }
    }
    protected void btn_Cancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("");
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
}
