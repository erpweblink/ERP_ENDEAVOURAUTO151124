using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_ReceiptVoucher : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GenrateReceiptVoucherNo();
        }
    }

    private void BindGrid()
    {
        DataTable Dt = new DataTable();
        SqlDataAdapter Da = new SqlDataAdapter("SELECT * FROM tblInvoiceHdr", con);
        Da.Fill(Dt);

        GvReceiptVoucher.DataSource = Dt;
        GvReceiptVoucher.DataBind();
    }

    //Get Customer
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
                com.CommandText = "select DISTINCT CustName from tblInvoiceHdr where " + "CustName like @Search + '%'";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> CustomerName = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        CustomerName.Add(sdr["CustName"].ToString());
                    }
                }
                con.Close();
                return CustomerName;
            }

        }
    }

    protected void txtcustomername_TextChanged(object sender, EventArgs e)
    {
        DataTable Dt = new DataTable();
        SqlDataAdapter Da = new SqlDataAdapter("SELECT * FROM tblInvoiceHdr WHERE CustName='" + txtcustomername.Text + "'", con);
        Da.Fill(Dt);
        GvReceiptVoucher.DataSource = Dt;
        GvReceiptVoucher.DataBind();
    }

    private void FindBalance(GridViewRow row)
    {
        TextBox InvoiceAmount = (TextBox)row.FindControl("txtinvoiceamt");
        TextBox PaidAmount = (TextBox)row.FindControl("txtpaidamt");
        TextBox Balance = (TextBox)row.FindControl("txtbalance");
        TextBox AdjustAmount = (TextBox)row.FindControl("txtadjust");
        Label lblBalance = (Label)row.FindControl("lblbalance");

        var BalanceAmt = Convert.ToDecimal(InvoiceAmount.Text) - Convert.ToDecimal(PaidAmount.Text);
        Balance.Text = BalanceAmt.ToString("0.00", CultureInfo.InvariantCulture);
    }

    protected void txtpaidamt_TextChanged(object sender, EventArgs e)
    {
        GridViewRow row = (sender as TextBox).NamingContainer as GridViewRow;
        FindBalance(row);
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            SqlCommand Cmd = new SqlCommand("SP_ReceiptVoucher", con);
            Cmd.CommandType = CommandType.StoredProcedure;

            Cmd.Parameters.AddWithValue("@Action", "Save");
            Cmd.Parameters.AddWithValue("@CustomerName", txtcustomername.Text);
            Cmd.Parameters.AddWithValue("@Date", txtDate.Text);
            Cmd.Parameters.AddWithValue("@ReceiptNo", txtReceiptvoucherno.Text);
            Cmd.Parameters.AddWithValue("@BankCashDr", ddlbankcashdr.SelectedItem.Text);
            Cmd.Parameters.AddWithValue("@PaymentMode", ddlpaymentmode.SelectedItem.Text);
            Cmd.Parameters.AddWithValue("@ClearedOnDate", txtclearedOnDate.Text);
            Cmd.Parameters.AddWithValue("@ChequeTransIdNo", txtchequeTransfporedno.Text);
            Cmd.Parameters.AddWithValue("@ChequeTransDate", txtchequetransforeddate.Text);
            Cmd.Parameters.AddWithValue("@SlipNo", txtslipno.Text);
            Cmd.Parameters.AddWithValue("@BankName", txtbankname.Text);
            Cmd.Parameters.AddWithValue("@Branch", txtbranch.Text);
            Cmd.Parameters.AddWithValue("@Address", txtaddress.Text);
            Cmd.Parameters.AddWithValue("@TotalInWord", lbl_total_amt_Value.Text);
            Cmd.Parameters.AddWithValue("@CreatedBy", Session["adminname"].ToString());
            Cmd.Parameters.Add("@IDD", SqlDbType.Int).Direction = ParameterDirection.Output;

            con.Open();
            Cmd.ExecuteNonQuery();
            string Id = Cmd.Parameters["@IDD"].Value.ToString();
            con.Close();

            foreach (GridViewRow g1 in GvReceiptVoucher.Rows)
            {
                CheckBox Che = (g1.FindControl("chk") as CheckBox);
                string Invoiceno = (g1.FindControl("lblinvoiceno") as Label).Text;
                string InvoiceDate = (g1.FindControl("lblinvoicedate") as Label).Text;
                string InvoiceAmount = (g1.FindControl("txtinvoiceamt") as TextBox).Text;
                string PaidAmount = (g1.FindControl("txtpaidamt") as TextBox).Text;
                string Balance = (g1.FindControl("txtbalance") as TextBox).Text;
                string AdjustAmount = (g1.FindControl("txtadjust") as TextBox).Text;

                SqlCommand Cmd1 = new SqlCommand("INSERT INTO TblReceiptVoucherDtls (ReceiptVoucherId,InvoiceNo,InvoiceDate,InvoiceAmount,PaidAmount,Balance,AdjustAmount)" +
                    "VALUES('" + Id + "','" + Invoiceno + "','" + Convert.ToDateTime(InvoiceDate).ToString("yyyy-MM-dd") + "','" + InvoiceAmount + "','" + PaidAmount + "','" + Balance + "','" + AdjustAmount + "')", con);

                con.Open();
                if (Che.Checked == true)
                {
                    Cmd1.ExecuteNonQuery();
                }
                con.Close();
            }
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Saved Sucessfully');", true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void GvReceiptVoucher_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TextBox InvoiceAmount = (TextBox)e.Row.FindControl("txtinvoiceamt");
            TextBox PaidAmount = (TextBox)e.Row.FindControl("txtpaidamt");
            TextBox Balance = (TextBox)e.Row.FindControl("txtbalance");
            TextBox AdjustAmount = (TextBox)e.Row.FindControl("txtadjust");
            Label lblInvoiceNo = (Label)e.Row.FindControl("lblinvoiceno");
            Label lblBalance = (Label)e.Row.FindControl("lblbalance");
            Label lbl_total_amt_Value = (Label)e.Row.FindControl("TotalInWord");

            string invoiceNo = lblInvoiceNo.Text;
            string AdjustAmt = AdjustAmount.Text;
            SqlCommand Cmd = new SqlCommand("SELECT isnull (sum (cast (AdjustAmount as decimal)),0) FROM TblReceiptVoucherDtls WHERE InvoiceNo='" + invoiceNo + "'", con);
            con.Open();
            object TotalPaid = Cmd.ExecuteScalar();
            var Paid = TotalPaid == null ? "0.00" : TotalPaid.ToString();
            PaidAmount.Text = Paid.ToString();

            var BalanceAmt = Convert.ToDecimal(InvoiceAmount.Text) - Convert.ToDecimal(PaidAmount.Text);
            Balance.Text = BalanceAmt.ToString("0.00", CultureInfo.InvariantCulture);

            if (Balance.Text == "0.00")
            {
                e.Row.Visible = false;
            }
            con.Close();
        }

        string isNegative = " ";

        try
        {
            string number = txtgrandtotal.Text;

            number = Convert.ToDouble(number).ToString();

            if (number.Contains("-"))
            {
                isNegative = "Minus ";
                number = number.Substring(1, number.Length - 1);

            }
            else
            {
                lbl_total_amt_Value.Text = isNegative + ConvertToWords(number);
            }
        }
        catch (Exception)
        {

        }
    }                           

    protected void btncancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ReceiptVoucherDetails.aspx");
    }

    //Receipt Voucher No
    protected void GenrateReceiptVoucherNo()
    {
        SqlDataAdapter Da = new SqlDataAdapter("SELECT max([Id]) as maxid FROM [TblReceiptVoucherHdr]", con);
        DataTable Dt = new DataTable();
        Da.Fill(Dt);
        if (Dt.Rows.Count > 0)
        {
            int maxid = Dt.Rows[0]["maxid"].ToString() == "" ? 0 : Convert.ToInt32(Dt.Rows[0]["maxid"].ToString());
            txtReceiptvoucherno.Text = "RECP-" + (maxid + 1).ToString();
        }
        else
        {
            txtReceiptvoucherno.Text = string.Empty;
        }
    }

    protected void txtadjust_TextChanged(object sender, EventArgs e)
    {
        GridViewRow row = (sender as TextBox).NamingContainer as GridViewRow;
        PaidAmt(row);
    }

    private void PaidAmt(GridViewRow row)
    {
        TextBox Adjust = (TextBox)row.FindControl("txtadjust");
        TextBox PaidAmt = (TextBox)row.FindControl("txtpaidamt");
        TextBox Balance = (TextBox)row.FindControl("txtbalance");

        var Paid = Convert.ToDecimal(Adjust.Text) + Convert.ToDecimal(PaidAmt.Text);
        PaidAmt.Text = Paid.ToString();
    }


    //------------Converts Amount In Words--------------

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

    protected void chk_CheckedChanged(object sender, EventArgs e)
    {
        foreach (GridViewRow G1 in GvReceiptVoucher.Rows)
        {
            CheckBox Chk = (G1.FindControl("chk") as CheckBox);
            string Total = (G1.FindControl("txtpaidamt") as TextBox).Text;
            if (Chk.Checked == true)
            {
                txtgrandtotal.Text += Total.ToString();
            }
            else if (Chk.Checked == false)
            {
                txtgrandtotal.Text = "";
            }
        }
    }
}