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

public partial class Admin_Quotation_Sales : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
    string id = "";
    string ID = "";
    string Idd = "";
    string chkupdate;
    DataTable Dt_Mail = new DataTable();
    DataTable Dt_Component = new DataTable();
    DataTable Dt_jobno = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                GenerateCode();
                jobnogrid();
                FillTerms();

                taxPanel1.Visible = false;
                taxpanel2.Visible = false;
                txtpanel3.Visible = false;

                Txt_Quo_Date.Attributes["max"] = DateTime.Now.ToString("yyyy-MM-dd");

                ViewState["RowNo"] = 0;

                //07-04-23
                Dt_Component.Columns.AddRange(new DataColumn[11] { new DataColumn("id"), new DataColumn("MateName"), new DataColumn("AddDescription"), new DataColumn("HSN/SAC"), new DataColumn("Rate"), new DataColumn("Unit"), new DataColumn("Quantity"), new DataColumn("Tax"), new DataColumn("Discount"), new DataColumn("Total"), new DataColumn("Total Amount") });
                ViewState["QuotationComp"] = Dt_Component;

                ViewState["RowNo"] = 0;
                Dt_Mail.Columns.AddRange(new DataColumn[3] { new DataColumn("mailid"), new DataColumn("designation"), new DataColumn("mailtext") });
                ViewState["MULTMail"] = Dt_Mail;

                ViewState["RowNo"] = 0;
                Dt_jobno.Columns.AddRange(new DataColumn[3] { new DataColumn("Jobno"), new DataColumn("designation"), new DataColumn("mailtext") });
                ViewState["jobno"] = Dt_jobno;

                ////Create Quotation
                if (Request.QueryString["JobNo"] != null)
                {
                    id = Decrypt(Request.QueryString["JobNo"].ToString());
                    btnSubmit.Text = "Submit";
                    hidden.Value = id;
                    ViewData();
                    showCustDtl();
                }
                else
                {

                }

                //Edit Quotation
                if (Request.QueryString["Quotation_no"] != null)
                {
                    ID = Decrypt(Request.QueryString["Quotation_no"].ToString());
                    txt_Comp_name.ReadOnly = true;
                    btnSubmit.Text = "Update";

                    ShowHeaderEdit();
                    ShowDtlEdit();
                    hidden.Value = ID;
                }

                if (Request.QueryString["ID"] != null)
                {
                    Idd = Decrypt(Request.QueryString["ID"].ToString());
                    ReportLoadData();
                    reportresdonly();
                }
            }
        }
        catch (Exception ex)
        {
            string errorMsg = "An error occurred : " + ex.Message;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + errorMsg + "') ", true);
        }
    }

    protected void ReportLoadData()
    {
        try
        {
            SqlDataAdapter Da = new SqlDataAdapter("SELECT JobNo,Quotation_no,IGST,Customer_Name,SubCustomer,Quotation_Date,Address,Mobile_No,Phone_No,GST_No,State_Code,kind_Att,CGST,SGST,AllTotal_price,Total_in_word,[Term_Condition_1],[Term_Condition_2],[Term_Condition_3],[Term_Condition_4] FROM tbl_Quotation_two_Hdr WHERE ID='" + Idd + "'", con);
            DataTable Dt = new DataTable();
            Da.Fill(Dt);
            if (Dt.Rows.Count > 0)
            {
                txt_Quo_No.Text = Dt.Rows[0]["Quotation_no"].ToString();
                txt_Comp_name.Text = Dt.Rows[0]["Customer_Name"].ToString();
                ddlsubcustomerbind();
                ddlsubcustomer.SelectedItem.Text = Dt.Rows[0]["SubCustomer"].ToString();
                txt_Quo_No.Text = Dt.Rows[0]["Quotation_no"].ToString();

                DateTime ffff1 = Convert.ToDateTime(Dt.Rows[0]["Quotation_Date"].ToString());
                Txt_Quo_Date.Text = ffff1.ToString("yyyy-MM-dd");

                txt_Address.Text = Dt.Rows[0]["Address"].ToString();
                txt_Mobile.Text = Dt.Rows[0]["Mobile_No"].ToString();
                txt_Phoneno.Text = Dt.Rows[0]["Phone_No"].ToString();
                txt_GST.Text = Dt.Rows[0]["GST_No"].ToString();
                txt_state.Text = Dt.Rows[0]["State_Code"].ToString();
                txt_cgst9.Text = Dt.Rows[0]["CGST"].ToString();
                txt_sgst9.Text = Dt.Rows[0]["SGST"].ToString();
                txtigst.Text = Dt.Rows[0]["IGST"].ToString();
                txt_grandTotal.Text = Dt.Rows[0]["AllTotal_price"].ToString();
                lbl_total_amt_Value.Text = Dt.Rows[0]["Total_in_word"].ToString();

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
                    txt_condition_5.Text = arrstr4[0].ToString();
                }

                if (arrstr5.Length > 0)
                {
                    txt_term_6.Text = arrstr5[0].ToString();
                    txt_condition_6.Text = arrstr5[0].ToString();
                }
            }
            SqlDataAdapter Sda = new SqlDataAdapter("SELECT * FROM [tblCustomerContactPerson] WHERE CustName='" + txt_Comp_name.Text + "'", con);
            DataTable Sdt = new DataTable();
            Sda.Fill(Sdt);
            Grd_MAIL.DataSource = Sdt;
            Grd_MAIL.DataBind();
            ShowDtlEdit();
            txt_kind_att.Text = Dt.Rows[0]["kind_Att"].ToString();
        }
        catch (Exception ex)
        {
            string errorMsg = "An error occurred : " + ex.Message;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + errorMsg + "') ", true);
        }
    }

    protected void jobnogrid()
    {
        try
        {
            SqlDataAdapter Sda = new SqlDataAdapter("SELECT JobNo,MateName FROM [tblInwardEntry] WHERE CustName = '" + txt_Comp_name.Text + "' ", con);
            DataTable Sdt = new DataTable();
            Sda.Fill(Sdt);
            grdjobno.DataSource = Sdt;
            grdjobno.DataBind();
        }
        catch (Exception ex)
        {
            string errorMsg = "An error occurred : " + ex.Message;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + errorMsg + "') ", true);
        }

    }

    protected void ddlsubcustomerbind()
    {
        try
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlDataAdapter sad = new SqlDataAdapter("SELECT DISTINCT Subcustomer FROM [tblInwardEntry] WHERE CustName = '" + txt_Comp_name.Text + "' ", con);
            sad.Fill(dt);

            ddlsubcustomer.DataSource = dt;
            ddlsubcustomer.DataTextField = "Subcustomer";
            ddlsubcustomer.DataBind();
            ddlsubcustomer.Items.Insert(0, "NA");
            con.Close();
        }
        catch (Exception ex)
        {
            string errorMsg = "An error occurred : " + ex.Message;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + errorMsg + "') ", true);
        }
    }

    protected void reportresdonly()
    {
        try
        {
            componantdetails.Visible = false;
            btnCancel.Text = "Close";
            btnSubmit.Visible = false;
            mailcheck.Visible = false;
            Grd_MAIL.Columns[2].Visible = false;
            dgvProductDtl.Columns[10].Visible = false;
            Txt_Quo_Date.ReadOnly = true;
            txt_Comp_name.ReadOnly = true;
            txt_Address.ReadOnly = true;
            txt_Mobile.ReadOnly = true;
            txt_Phoneno.ReadOnly = true;
            txt_GST.ReadOnly = true;
            txt_state.ReadOnly = true;
            txt_term_1.ReadOnly = true;
            txt_term_2.ReadOnly = true;
            txt_term_3.ReadOnly = true;
            txt_term_4.ReadOnly = true;
            txt_term_5.ReadOnly = true;
            txt_term_6.ReadOnly = true;
            txt_condition_1.ReadOnly = true;
            txt_condition_2.ReadOnly = true;
            txt_condition_3.ReadOnly = true;
            txt_condition_4.ReadOnly = true;
            txt_condition_5.ReadOnly = true;
            txt_condition_6.ReadOnly = true;
            txt_kind_att.Enabled = true;
            headerreport.InnerText = "Quatation Report";
        }
        catch (Exception ex)
        {
            string errorMsg = "An error occurred : " + ex.Message;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + errorMsg + "') ", true);
        }


    }

    protected void ViewData()
    {
        try
        {
            SqlDataAdapter Da = new SqlDataAdapter("SELECT JobNo,CustomerName,Quotation_no FROM tblTestingProduct WHERE JobNo='" + id + "'  ", con);
            DataTable Dt = new DataTable();
            Da.Fill(Dt);
            if (Dt.Rows.Count > 0)
            {
                txt_Quo_No.Text = Dt.Rows[0]["Quotation_no"].ToString();
                txt_Comp_name.Text = Dt.Rows[0]["CustomerName"].ToString();
            }
        }
        catch (Exception ex)
        {
            string errorMsg = "An error occurred : " + ex.Message;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + errorMsg + "') ", true);
        }
    }

    protected void ShowHeaderEdit()
    {
        try
        {
            SqlDataAdapter Da = new SqlDataAdapter("SELECT JobNo,Quotation_no,Customer_Name,SubCustomer,Quotation_Date,ExpiryDate,Address,Mobile_No,Phone_No,GST_No,State_Code,kind_Att,CGST,SGST,AllTotal_price,Total_in_word,[Term_Condition_1],[Term_Condition_2],[Term_Condition_3],[Term_Condition_4],[Term_Condition_5],[Term_Condition_6],IGST FROM tbl_Quotation_two_Hdr WHERE Quotation_no='" + ID + "'", con);
            DataTable Dt = new DataTable();
            Da.Fill(Dt);
            if (Dt.Rows.Count > 0)
            {
                txt_Quo_No.Text = Dt.Rows[0]["Quotation_no"].ToString();
                txt_Comp_name.Text = Dt.Rows[0]["Customer_Name"].ToString();
                ddlsubcustomerbind();
                ddlsubcustomer.SelectedItem.Text = Dt.Rows[0]["SubCustomer"].ToString();
                txt_Quo_No.Text = Dt.Rows[0]["Quotation_no"].ToString();

                DateTime ffff1 = Convert.ToDateTime(Dt.Rows[0]["Quotation_Date"].ToString());
                DateTime expdate = Convert.ToDateTime(Dt.Rows[0]["ExpiryDate"].ToString());
                Txt_Quo_Date.Text = ffff1.ToString("yyyy-MM-dd");
                txtexpirydate.Text = expdate.ToString("yyyy-MM-dd");
                txt_Address.Text = Dt.Rows[0]["Address"].ToString();
                txt_Mobile.Text = Dt.Rows[0]["Mobile_No"].ToString();
                txt_Phoneno.Text = Dt.Rows[0]["Phone_No"].ToString();
                txt_GST.Text = Dt.Rows[0]["GST_No"].ToString();
                txt_state.Text = Dt.Rows[0]["State_Code"].ToString();
                txt_cgst9.Text = Dt.Rows[0]["CGST"].ToString();
                txt_sgst9.Text = Dt.Rows[0]["SGST"].ToString();
                txtigst.Text = Dt.Rows[0]["IGST"].ToString();
                txt_grandTotal.Text = Dt.Rows[0]["AllTotal_price"].ToString();
                lbl_total_amt_Value.Text = Dt.Rows[0]["Total_in_word"].ToString();

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

            SqlDataAdapter Sda = new SqlDataAdapter("SELECT * FROM tblCustomerContactPerson WHERE CustName='" + txt_Comp_name.Text + "'", con);
            DataTable Sdt = new DataTable();
            Sda.Fill(Sdt);
            Grd_MAIL.DataSource = Sdt;
            Grd_MAIL.DataBind();

            SqlDataAdapter Saa = new SqlDataAdapter("SELECT id, Jobno FROM [tbl_Quotationjobno] WHERE Quotation_no='" + txt_Quo_No.Text + "'", con);
            DataTable dttt = new DataTable();
            Saa.Fill(dttt);
            grdjobno.DataSource = dttt;
            grdjobno.DataBind();
        }
        catch (Exception ex)
        {
            string errorMsg = "An error occurred : " + ex.Message;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + errorMsg + "') ", true);
        }
    }

    protected void ShowDtlEdit()
    {
        try
        {
            // ddljobnobind();
            SqlDataAdapter Da = new SqlDataAdapter("SELECT * FROM tbl_Quotation_two_Dtls WHERE Quotation_no='" + txt_Quo_No.Text + "'", con);
            DataTable DTCOMP = new DataTable();
            Da.Fill(DTCOMP);

            int count = 1;
            if (DTCOMP.Rows.Count > 0)
            {
                if (Dt_Component.Columns.Count < 1)
                {
                    Show_Grid();
                }

                for (int i = 0; i < DTCOMP.Rows.Count; i++)
                {
                    Dt_Component.Rows.Add(count, DTCOMP.Rows[i]["product"].ToString(), DTCOMP.Rows[i]["Description"].ToString(), DTCOMP.Rows[i]["HSN"].ToString(), DTCOMP.Rows[i]["Rate"].ToString(), DTCOMP.Rows[i]["Units"].ToString(), DTCOMP.Rows[i]["Qty"].ToString(), DTCOMP.Rows[i]["Tax"].ToString(), DTCOMP.Rows[i]["Disc_per"].ToString(), DTCOMP.Rows[i]["total"].ToString(), DTCOMP.Rows[i]["FTotal"].ToString());
                    count = count + 1;
                }
            }

            dgvProductDtl.EmptyDataText = "No Data Found";
            dgvProductDtl.DataSource = Dt_Component;
            dgvProductDtl.DataBind();

            SqlDataAdapter Sda = new SqlDataAdapter("SELECT * FROM tbl_Quotation_two_Hdr WHERE Quotation_no='" + txt_Quo_No.Text + "'", con);
            DataTable Sdt = new DataTable();
            Sda.Fill(Sdt);
            if (Sdt.Rows.Count > 0)
            {
                txt_cgst9.Text = Sdt.Rows[0]["CGST"].ToString();
                txt_sgst9.Text = Sdt.Rows[0]["SGST"].ToString();
                txt_grandTotal.Text = Sdt.Rows[0]["AllTotal_price"].ToString();
                lbl_total_amt_Value.Text = Sdt.Rows[0]["Total_in_word"].ToString();
            }
        }
        catch (Exception ex)
        {
            string errorMsg = "An error occurred : " + ex.Message;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + errorMsg + "') ", true);
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

    protected void showCustDtl()
    {
        SqlDataAdapter Da = new SqlDataAdapter("SELECT CustomerName,GSTNo,StateCode,AddresLine1,Email,MobNo,ContactPerName1,Custid FROM tblCustomer WHERE CustomerName='" + txt_Comp_name.Text + "'", con);
        DataTable Dt = new DataTable();
        Da.Fill(Dt);
        if (Dt.Rows.Count > 0)
        {
            txt_GST.Text = Dt.Rows[0]["GSTNo"].ToString();
            txt_state.Text = Dt.Rows[0]["StateCode"].ToString();
            txt_Address.Text = Dt.Rows[0]["AddresLine1"].ToString();
            txt_Mobile.Text = Dt.Rows[0]["MobNo"].ToString();
            string customerID = Dt.Rows[0]["Custid"].ToString();
            hiddencustomerID.Value = customerID.ToString();
        }

        SqlDataAdapter Sda = new SqlDataAdapter("SELECT * FROM tblCustomerContactPerson WHERE CustName='" + txt_Comp_name.Text + "'", con);
        DataTable Sdt = new DataTable();
        Sda.Fill(Sdt);
        Grd_MAIL.DataSource = Sdt;
        Grd_MAIL.DataBind();
    }

    public void Calculations(GridViewRow row)
    {
        TextBox Rate = (TextBox)row.FindControl("txtRate");
        TextBox Qty = (TextBox)row.FindControl("txtQuantity");
        Label Total = (Label)row.FindControl("txtTotal");
        TextBox Tax_per = (TextBox)row.FindControl("txtTax");
        TextBox Disc_Per = (TextBox)row.FindControl("txt_Discount");
        Label GrossTotal = (Label)row.FindControl("lblTotalPrice");
        var total = Convert.ToDecimal(Rate.Text) * Convert.ToDecimal(Qty.Text);
        Total.Text = string.Format("{0:0.00}", total);

        decimal tax_amt;
        if (string.IsNullOrEmpty(Tax_per.Text))
        {
            tax_amt = 0;
        }
        else
        {
            tax_amt = Convert.ToDecimal(total.ToString()) * Convert.ToDecimal(Tax_per.Text) / 100;
        }

        var totalWithTax = Convert.ToDecimal(total.ToString()) + Convert.ToDecimal(tax_amt.ToString());
        decimal disc_amt;
        if (string.IsNullOrEmpty(Disc_Per.Text))
        {
            disc_amt = 0;
        }
        else
        {
            disc_amt = Convert.ToDecimal(total.ToString()) * Convert.ToDecimal(Disc_Per.Text) / 100;
        }

        var Grossamt = Convert.ToDecimal(total.ToString()) - Convert.ToDecimal(disc_amt.ToString());
        Total.Text = string.Format("{0:0.00}", Grossamt);

        ScriptManager.RegisterStartupScript(this, this.GetType(), "Success", "scrollToElement();", true);
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

    protected void txtQuantity_TextChanged(object sender, EventArgs e)
    {
        GridViewRow row = (sender as TextBox).NamingContainer as GridViewRow;
        Calculations(row);
    }

    protected void txtTax_TextChanged(object sender, EventArgs e)
    {
        GridViewRow row = (sender as TextBox).NamingContainer as GridViewRow;
        Calculations(row);
    }

    protected void GenerateCode()
    {
        SqlDataAdapter ad = new SqlDataAdapter("SELECT max([ID]) as maxid FROM [tbl_Quotation_two_Hdr]", con);
        DataTable dt = new DataTable();
        ad.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            int maxid = dt.Rows[0]["maxid"].ToString() == "" ? 0 : Convert.ToInt32(dt.Rows[0]["maxid"].ToString());
            txt_Quo_No.Text = "QN" + (maxid + 1).ToString();
        }
        else
        {
            txt_Quo_No.Text = string.Empty;
        }
    }

    protected void FillTerms()
    {
        txt_term_1.Text = "Delivery";
        txt_term_2.Text = "Freight";
        txt_term_3.Text = "Installation";
        txt_term_4.Text = "Payment Terms";
        txt_term_5.Text = "Warranty";
    }

    protected void GenerateManualyJobCode()
    {
        string str;
        SqlCommand com;
        int count;
        str = "select count(mnQuatation) from tbl_Quotation_two_Hdr";
        com = new SqlCommand(str, con);
        con.Open();

        count = Convert.ToInt16(com.ExecuteScalar()) + 1;

        con.Close();
        string str1;
        SqlCommand com1;
        int count1;
        str1 = "select count(mnQuatation) from tbl_Quotation_two_Hdr";
        com1 = new SqlCommand(str1, con);
        con.Open();
        count1 = Convert.ToInt16(com.ExecuteScalar()) + 1;
        txt_Quo_No.Text = "MN_QN" + count.ToString();

        con.Close();

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["ID"] != null)
        {
            Response.Redirect("QuatationReport.aspx");
        }
        else
        {
            //Response.Redirect("QuotationList.aspx"); 
            Response.Redirect("Quotation_ListSales.aspx");
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (btnSubmit.Text == "Submit")
        {
            if (ChkSendQuotation.Checked == true)
            {
                Save_Record();
                Send_Mail();
            }
            else
            {
                Save_Record();
            }
        }

        if (btnSubmit.Text == "Update")
        {
            if (ChkSendQuotation.Checked == true)
            {
                Save_Record();
                Send_Mail();
            }
            else
            {
                Save_Record();
            }
        }
    }

    protected void Save_Record()
    {
        string createdby = Session["adminname"].ToString();
        try
        {
            DataTable Dt = new DataTable("SELECT * FROM [tbl_Quotation_two_Hdr] WHERE Quotation_no = '" + txt_Quo_No.Text + "'");
            if (btnSubmit.Text == "Submit")
            {
                if (Dt.Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Quotation Alredy Created..!!')", true);
                }
                else
                {
                    string[] subs = txt_Quo_No.Text.Split('_');
                    DateTime Date = DateTime.Now;
                    SqlCommand cmd = new SqlCommand("SP_Quotation_Hdr_two", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Quotation_no", txt_Quo_No.Text);
                    cmd.Parameters.AddWithValue("@Quotation_Date", Txt_Quo_Date.Text);
                    cmd.Parameters.AddWithValue("@ExpiryDate", txtexpirydate.Text);
                    cmd.Parameters.AddWithValue("@Customer_Name", txt_Comp_name.Text);
                    cmd.Parameters.AddWithValue("@SubCustomer", ddlsubcustomer.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@Address", txt_Address.Text);
                    cmd.Parameters.AddWithValue("@Mobile_No", txt_Mobile.Text);
                    cmd.Parameters.AddWithValue("@Phone_No", txt_Phoneno.Text);
                    cmd.Parameters.AddWithValue("@GST_No", txt_GST.Text);
                    cmd.Parameters.AddWithValue("@State_Code", txt_state.Text);
                    cmd.Parameters.AddWithValue("@kind_Att", txt_kind_att.Text);
                    cmd.Parameters.AddWithValue("@CGST", txt_cgst9.Text);
                    cmd.Parameters.AddWithValue("@SGST", txt_sgst9.Text);
                    cmd.Parameters.AddWithValue("@IGST", txtigst.Text);
                    cmd.Parameters.AddWithValue("@ServiceType", ddlservicetype.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@Againstby", ddlagainstby.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@AllTotal_price", txt_grandTotal.Text);
                    cmd.Parameters.AddWithValue("@Total_in_word", lbl_total_amt_Value.Text);
                    cmd.Parameters.AddWithValue("@IsDeleted", '0');
                    cmd.Parameters.AddWithValue("@isCompleted", '1');
                    cmd.Parameters.AddWithValue("@mnQuatation", '1');
                    cmd.Parameters.AddWithValue("@Term_Condition_1", txt_term_1.Text + "-" + txt_condition_1.Text);
                    cmd.Parameters.AddWithValue("@Term_Condition_2", txt_term_2.Text + "-" + txt_condition_2.Text);
                    cmd.Parameters.AddWithValue("@Term_Condition_3", txt_term_3.Text + "-" + txt_condition_3.Text);
                    cmd.Parameters.AddWithValue("@Term_Condition_4", txt_term_4.Text + "-" + txt_condition_4.Text);
                    cmd.Parameters.AddWithValue("@Term_Condition_5", txt_term_5.Text + "-" + txt_condition_5.Text);
                    cmd.Parameters.AddWithValue("@Term_Condition_6", txt_term_6.Text + "-" + txt_condition_6.Text);
                    cmd.Parameters.AddWithValue("@CreatedBy", createdby);
                    cmd.Parameters.AddWithValue("@CreatedOn", Date);
                    cmd.Parameters.AddWithValue("@Action", "Insert");
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    foreach (GridViewRow grd1 in dgvProductDtl.Rows)
                    {
                        string product = (grd1.FindControl("lblproduct") as Label).Text;
                        string AddDescription = (grd1.FindControl("lbl_AddDescription") as Label).Text;
                        string HSN_grd = (grd1.FindControl("lblhsnsac") as Label).Text;
                        string Rate_grd = (grd1.FindControl("lbl_Rate") as Label).Text;
                        string unit_grd = (grd1.FindControl("lblunit") as Label).Text;
                        string Qty_grd = (grd1.FindControl("lbl_quntity_grd") as Label).Text;
                        string TaxPer_grd = (grd1.FindControl("lbl_Tax") as Label).Text;
                        string total_grd = (grd1.FindControl("txtTotal") as Label).Text;
                        string DiscPer_grd = (grd1.FindControl("lbl_Discount") as Label).Text;
                        string Total_grd = (grd1.FindControl("lblTotalPrice") as Label).Text;

                        SqlCommand cmdd = new SqlCommand("INSERT INTO tbl_Quotation_two_Dtls (Quotation_no,HSN,Tax,Qty,Units,total,Rate,Disc_per,FTotal,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,Description,product) VALUES(@Quotation_no,@HSN,@Tax,@Qty,@Units,@total,@Rate,@Disc_per,@FTotal,@CreatedBy,@CreatedOn,@UpdatedBy,@UpdatedOn,@Description,@product)", con);
                        cmdd.Parameters.AddWithValue("@Quotation_no", txt_Quo_No.Text);
                        cmdd.Parameters.AddWithValue("@HSN", HSN_grd);
                        cmdd.Parameters.AddWithValue("@Tax", TaxPer_grd);
                        cmdd.Parameters.AddWithValue("@Qty", Qty_grd);
                        cmdd.Parameters.AddWithValue("@Units", unit_grd);
                        cmdd.Parameters.AddWithValue("@total", total_grd);
                        cmdd.Parameters.AddWithValue("@Rate", Rate_grd);
                        cmdd.Parameters.AddWithValue("@Disc_per", DiscPer_grd);
                        cmdd.Parameters.AddWithValue("@FTotal", Total_grd);
                        cmdd.Parameters.AddWithValue("@CreatedBy", createdby);
                        cmdd.Parameters.AddWithValue("@CreatedOn", Date);
                        cmdd.Parameters.AddWithValue("@UpdatedBy", createdby);
                        cmdd.Parameters.AddWithValue("@UpdatedOn", Date);
                        cmdd.Parameters.AddWithValue("@Description", AddDescription);
                        cmdd.Parameters.AddWithValue("@product", product);
                        con.Open();
                        cmdd.ExecuteNonQuery();
                        con.Close();
                    }

                    foreach (GridViewRow g1 in Grd_MAIL.Rows)
                    {
                        string MAIL = (g1.FindControl("lblmultMail") as Label).Text;
                        bool chkmail = (g1.FindControl("chkmail") as CheckBox).Checked;

                        con.Open();
                        SqlCommand cmdtable = new SqlCommand("insert into QuotationMail(Quotation_no,Email,chkmail,CreatedBy,CreatedOn) values(@Quotation_no,@Email,@chkmail,@CreatedBy,@CreatedOn)", con);
                        cmdtable.Parameters.AddWithValue("@Quotation_no", txt_Quo_No.Text);
                        cmdtable.Parameters.AddWithValue("@Email", MAIL);
                        cmdtable.Parameters.AddWithValue("@chkmail", chkmail);
                        cmdtable.Parameters.AddWithValue("@CreatedBy", createdby);
                        cmdtable.Parameters.AddWithValue("@CreatedOn", Date);
                        cmdtable.ExecuteNonQuery();
                        con.Close();
                    }
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Saved Sucessfully');", true);
                }
            }

            else if (btnSubmit.Text == "Update")
            {
                DateTime Date = DateTime.Now;
                con.Open();

                SqlCommand cmd = new SqlCommand("SP_Quotation_Hdr_two", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Quotation_no", txt_Quo_No.Text);
                cmd.Parameters.AddWithValue("@Quotation_Date", Txt_Quo_Date.Text);
                cmd.Parameters.AddWithValue("@ExpiryDate", txtexpirydate.Text);
                cmd.Parameters.AddWithValue("@Customer_Name", txt_Comp_name.Text);
                cmd.Parameters.AddWithValue("@SubCustomer", ddlsubcustomer.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@Address", txt_Address.Text);
                cmd.Parameters.AddWithValue("@Mobile_No", txt_Mobile.Text);
                cmd.Parameters.AddWithValue("@Phone_No", txt_Phoneno.Text);
                cmd.Parameters.AddWithValue("@GST_No", txt_GST.Text);
                cmd.Parameters.AddWithValue("@State_Code", txt_state.Text);
                cmd.Parameters.AddWithValue("@kind_Att", txt_kind_att.Text);
                cmd.Parameters.AddWithValue("@CGST", txt_cgst9.Text);
                cmd.Parameters.AddWithValue("@SGST", txt_sgst9.Text);
                cmd.Parameters.AddWithValue("@IGST", txtigst.Text);
                cmd.Parameters.AddWithValue("@ServiceType", ddlservicetype.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@Againstby", ddlagainstby.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@AllTotal_price", txt_grandTotal.Text);
                cmd.Parameters.AddWithValue("@Total_in_word", lbl_total_amt_Value.Text);
                cmd.Parameters.AddWithValue("@IsDeleted", '0');
                cmd.Parameters.AddWithValue("@CreatedBy", createdby);
                cmd.Parameters.AddWithValue("@CreatedOn", Date);
                cmd.Parameters.AddWithValue("@UpdatedBy", createdby);
                cmd.Parameters.AddWithValue("@Term_Condition_1", txt_term_1.Text + "-" + txt_condition_1.Text);
                cmd.Parameters.AddWithValue("@Term_Condition_2", txt_term_2.Text + "-" + txt_condition_2.Text);
                cmd.Parameters.AddWithValue("@Term_Condition_3", txt_term_3.Text + "-" + txt_condition_3.Text);
                cmd.Parameters.AddWithValue("@Term_Condition_4", txt_term_4.Text + "-" + txt_condition_4.Text);
                cmd.Parameters.AddWithValue("@Term_Condition_5", txt_term_5.Text + "-" + txt_condition_5.Text);
                cmd.Parameters.AddWithValue("@Term_Condition_6", txt_term_6.Text + "-" + txt_condition_6.Text);
                cmd.Parameters.AddWithValue("@UpdatedOn", Date);
                cmd.Parameters.AddWithValue("@isCreateQuata", '1');
                cmd.Parameters.AddWithValue("@Action", "Update");
                cmd.ExecuteNonQuery();
                con.Close();

                SqlCommand cmddelete = new SqlCommand("DELETE FROM tbl_Quotation_two_Dtls WHERE Quotation_no=@Quotation_no", con);
                cmddelete.Parameters.AddWithValue("@Quotation_no", txt_Quo_No.Text);
                con.Open();
                cmddelete.ExecuteNonQuery();
                con.Close();

                foreach (GridViewRow grd1 in dgvProductDtl.Rows)
                {
                    con.Open();

                    string product = (grd1.FindControl("lblproduct") as Label).Text;
                    string AddDescription = (grd1.FindControl("lbl_AddDescription") as Label).Text;
                    string HSN_grd = (grd1.FindControl("lblhsnsac") as Label).Text;
                    string Rate_grd = (grd1.FindControl("lbl_Rate") as Label).Text;
                    string unit_grd = (grd1.FindControl("lblunit") as Label).Text;
                    string Qty_grd = (grd1.FindControl("lbl_quntity_grd") as Label).Text;
                    string TaxPer_grd = (grd1.FindControl("lbl_Tax") as Label).Text;
                    string total_grd = (grd1.FindControl("txtTotal") as Label).Text;
                    string DiscPer_grd = (grd1.FindControl("lbl_Discount") as Label).Text;
                    string Total_grd = (grd1.FindControl("lblTotalPrice") as Label).Text;

                    SqlCommand cmdd = new SqlCommand("INSERT INTO tbl_Quotation_two_Dtls (Quotation_no,HSN,Tax,Qty,Units,total,Rate,Disc_per,FTotal,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,Description,product) VALUES(@Quotation_no,@HSN,@Tax,@Qty,@Units,@total,@Rate,@Disc_per,@FTotal,@CreatedBy,@CreatedOn,@UpdatedBy,@UpdatedOn,@Description,@product)", con);
                    cmdd.Parameters.AddWithValue("@Quotation_no", txt_Quo_No.Text);
                    cmdd.Parameters.AddWithValue("@HSN", HSN_grd);
                    cmdd.Parameters.AddWithValue("@Tax", TaxPer_grd);
                    cmdd.Parameters.AddWithValue("@Qty", Qty_grd);
                    cmdd.Parameters.AddWithValue("@Units", unit_grd);
                    cmdd.Parameters.AddWithValue("@total", total_grd);
                    cmdd.Parameters.AddWithValue("@Rate", Rate_grd);
                    cmdd.Parameters.AddWithValue("@Disc_per", DiscPer_grd);
                    cmdd.Parameters.AddWithValue("@FTotal", Total_grd);
                    cmdd.Parameters.AddWithValue("@CreatedBy", createdby);
                    cmdd.Parameters.AddWithValue("@CreatedOn", Date);
                    cmdd.Parameters.AddWithValue("@UpdatedBy", createdby);
                    cmdd.Parameters.AddWithValue("@UpdatedOn", Date);
                    cmdd.Parameters.AddWithValue("@Description", AddDescription);
                    cmdd.Parameters.AddWithValue("@product", product);
                    cmdd.ExecuteNonQuery();
                    con.Close();
                }

                SqlDataAdapter Sda = new SqlDataAdapter("SELECT * FROM QuotationMail WHERE Quotation_no='" + txt_Quo_No.Text + "'", con);
                DataTable DTMAIL = new DataTable();
                Sda.Fill(DTMAIL);
                foreach (GridViewRow g1 in Grd_MAIL.Rows)
                {
                    string MAIL = (g1.FindControl("lblmultMail") as Label).Text;
                    bool chkmail = (g1.FindControl("chkmail") as CheckBox).Checked;
                    con.Open();
                    SqlCommand cmdtable = new SqlCommand("UPDATE QuotationMail SET Email=@Email, UpdatedBy=@UpdatedBy, UpdatedOn=@UpdatedOn,chkmail=@chkmail WHERE Quotation_no=@Quotation_no AND Email=@Email", con);
                    cmdtable.Parameters.AddWithValue("@Quotation_no", txt_Quo_No.Text);
                    cmdtable.Parameters.AddWithValue("@Email", MAIL);
                    cmdtable.Parameters.AddWithValue("@chkmail", chkmail);
                    cmdtable.Parameters.AddWithValue("@UpdatedBy", createdby);
                    cmdtable.Parameters.AddWithValue("@UpdatedOn", Date);
                    cmdtable.ExecuteNonQuery();
                    con.Close();
                }
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Updated Successfully');", true);
            }

            ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Updated Successfully');", true);
        }
        catch (Exception ex)
        {
            string errorMsg = "An error occurred : " + ex.Message;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + errorMsg + "') ", true);
        }
    }
    protected void Send_Mail()
    {
        try
        {
            DataTable dt = new DataTable();
            SqlDataAdapter sad = new SqlDataAdapter("Select * from tbl_Quotation_two_Hdr where Quotation_no='" + txt_Quo_No.Text + "'", con);
            sad.Fill(dt);

            string strMessage =

                        "<strong>Dear Sir," + "<br/><br/>" +

                        "Greetings from " + "<strong>Endeavour  Automation...!" + "<br/><br/>" +

                          "Please find the attached offer for your reference. " + "Quotation No. - " + txt_Quo_No.Text.Trim() + ".pdf" + "<br/><br/>" +

                         "We hope that we will receive a Purchase Order Soon." + " <br/><br/>" +

                         "Please do not hesitate to contact us for any query or clarification." + " <br/><br/>";


            MemoryStream file = new MemoryStream(PDF("This is pdf file text", Server.MapPath("~/Files/")).ToArray());
            string pdfname = "Quotation - " + txt_Quo_No.Text.Trim() + "/" + Txt_Quo_Date.Text.Trim() + ".pdf";
            MailMessage message = new MailMessage();

            string BCC = "sales.endeavourautomations@gmail.com";

            foreach (GridViewRow g1 in Grd_MAIL.Rows)
            {
                string MAIL = (g1.FindControl("lblmultMail") as Label).Text;
                bool chkmail = (g1.FindControl("chkmail") as CheckBox).Checked;
                if (chkmail == true)
                {
                    message.To.Add(MAIL);// Email-ID of Receiver  
                }
            }

            message.CC.Add(BCC);
            file.Seek(0, SeekOrigin.Begin);
            Attachment data = new Attachment(file, pdfname, "application/pdf");
            ContentDisposition disposition = data.ContentDisposition;
            disposition.CreationDate = System.DateTime.Now;
            disposition.ModificationDate = System.DateTime.Now;
            disposition.DispositionType = DispositionTypeNames.Attachment;
            message.Attachments.Add(data);//Attach the file

            // message.Body = strMessage.ToString();

            string body = strMessage + GetEmailSignature();
            message.Body = body;

            message.Subject = "Quotation PDF";// Subject of Email  
            message.From = new MailAddress("enquiry@weblinkservices.net", "sales.endeavourautomations@gmail.com");
            message.IsBodyHtml = true;
            // Set the "Reply-To" header to indicate the desired display address
            message.ReplyToList.Add(new MailAddress("sales.endeavourautomations@gmail.com"));
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
        catch (Exception ex)
        {
            string errorMsg = "An error occurred : " + ex.Message;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + errorMsg + "') ", true);
        }
    }

    private string GetEmailSignature()
    {
        return @"
                  <br>
                  <strong>Thanks & Regards.</strong><br>
                  <img src='https://www.endeavours.in/images/logo.png' alt='Company Logo' width='100' height='100'><br>
                  <strong>Divya Sutar</strong><br>
                  Tellecaller <br>
                  Contact No: 8767236105<br>
                  <a href='mailto:sales.endeavourautomations@gmail.com'>sales.endeavourautomations@gmail.com</a><br>
                  <a href='https://www.endeavours.in/'>www.endeavours.in</a><br><br>
                  <strong>Office Address:</strong><br>
                  S.N. 9/2/A1, Sapkal Wasti,<br>
                  Ravet Road, Nr. HP Petrol Pump,<br>
                  Behind Genesis Furniture,<br>
                  Tathawade, PUNE- 411033,<br>
                  Maharashtra, INDIA
                  ";
    }



    public MemoryStream PDF(string Quo_NO, string message)
    {
        try
        {

        }
        catch (Exception ex)
        {
            string errorMsg = "An error occurred : " + ex.Message;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + errorMsg + "') ", true);
        }

        MemoryStream pdf = new MemoryStream();
        SqlDataAdapter Da = new SqlDataAdapter("SELECT * FROM vw_QuotMailPdf_Sales WHERE Quotation_no='" + txt_Quo_No.Text + "' ", con);
        DataTable Dt = new DataTable();
        Da.Fill(Dt);
        StringWriter sw = new StringWriter();
        StringReader sr = new StringReader(sw.ToString());
        Document doc = new Document(PageSize.A4, 10f, 10f, 55f, 0f);
        PdfWriter pdfWriter = PdfWriter.GetInstance(doc, pdf);
        PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(Server.MapPath("~/Files/") + "QuotationInvoice.pdf", FileMode.Create));
        XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, sr);
        doc.Open();
        string imageURL = Server.MapPath("~") + "/image/AA.png";
        iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(imageURL);
        //Resize image depend upon your need
        png.ScaleToFit(70, 100);
        //For Image Position
        png.SetAbsolutePosition(40, 745);

        png.SpacingBefore = 50f;

        //Give some space after the image

        png.SpacingAfter = 1f;

        png.Alignment = Element.ALIGN_LEFT;

        doc.Add(png);

        PdfContentByte cb = pdfWriter.DirectContent;
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
        cd.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Quotation", 260, 667, 0);
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

            table.AddCell(new Phrase("Company Name : ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(compname, FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("Quotation No. :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(Quo_No, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("Address :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(Address, FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("Quotation Date :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(datee, FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("State :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(state, FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("GST No. :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(GST_NO, FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("Ref.No. :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(RefNo, FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("Ref. Date :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(date, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("  Expiry Date", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("   " + expdate, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 9, Font.BOLD)));

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
                table.AddCell(new Phrase("Quantity", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("Unit", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("Rate", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("Tax(%)", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("Disc.(%)", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("Total", FontFactory.GetFont("Arial", 10, Font.BOLD)));

                int rowid = 1;
                foreach (DataRow dr in Dt.Rows)
                {
                    table.TotalWidth = 560f;
                    table.LockedWidth = true;

                    double Ftotal = Convert.ToDouble(dr["total"].ToString());
                    string _ftotal = Ftotal.ToString("##.00");
                    table.AddCell(new Phrase(rowid.ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(dr["Description"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(dr["HSN"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(dr["Qty"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(dr["Units"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(dr["Rate"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(dr["Tax"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(dr["Disc_per"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(_ftotal, FontFactory.GetFont("Arial", 9)));
                    rowid++;

                    Ttotal_price += Convert.ToDouble(dr["total"].ToString());
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

            if (Dt.Rows.Count >= 10)
            {
                table.AddCell(new Phrase("  \n\n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));

            }
            else if (Dt.Rows.Count >= 7 && Dt.Rows.Count <= 9)
            {
                table.AddCell(new Phrase("  \n\n\n\n\n\n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));

            }
            else if (Dt.Rows.Count >= 4 && Dt.Rows.Count <= 6)
            {
                table.AddCell(new Phrase("  \n\n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));

            }
            else if (Dt.Rows.Count < 4)
            {
                table.AddCell(new Phrase("  \n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));
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

                table.SetWidths(new float[] { 0f, 76f, 12f });
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


                table.SetWidths(new float[] { 0f, 76f, 12f });
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


                table.SetWidths(new float[] { 0f, 76f, 12f });
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

            table.SetWidths(new float[] { 0f, 76f, 12f });
            table.AddCell(paragraph);
            PdfPCell cell44 = new PdfPCell(new Phrase("Grand Total", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            cell44.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cell44);
            PdfPCell cell55 = new PdfPCell(new Phrase(Grndttl.ToString("##.00"), FontFactory.GetFont("Arial", 10, Font.BOLD)));
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
            Paragraph paragraph110term = new Paragraph("\bTerms & Condition:", font12112term);

            for (int i = 0; i < items90term.Length; i++)
            {
                paragraph110term.Add(new Phrase(" ", font10111term));
            }

            table = new PdfPTable(1);
            table.TotalWidth = 560f;
            table.LockedWidth = true;
            table.SetWidths(new float[] { 150f });

            table.AddCell(paragraph110term);

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
        }
        //}
        //}

        return pdf;
    }

    private decimal Total;

    protected void gandtotal()
    {

        decimal grd_total;
        decimal val1 = Convert.ToDecimal(txt_Subtotal.Text);
        grd_total = (val1);


        txt_Subtotal.Text = grd_total.ToString();

    }

    protected void txt_Discountt_TextChanged(object sender, EventArgs e)
    {
        GridViewRow row = (sender as TextBox).NamingContainer as GridViewRow;
        Calculations(row);


    }

    decimal totaltax = 0;
    decimal Totaltax18, Totaltax12, Total11177, Totaltx28 = 0, Qty = 0, rate111111 = 0, totalrateqty = 0;
    string lbljobno, lblproduct, lbl_Description, lbladddescription, lblhsnsac, lbl_Rate, lbl_quntity_grd, lblunit, lbltotaltax, lbl_Discount, lblrate, lblqty, Amount, LblID, LblQuoNo;
    protected void dgvProductDtl_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //cast the current row to a datarowview
            DataRowView row = e.Row.DataItem as DataRowView;
            Total += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Total"));
            txt_Subtotal.Text = Total.ToString("0.00");
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnkedit = e.Row.FindControl("btn_edit") as LinkButton;
            if (lnkedit == null)
            {
                lbladddescription = (e.Row.FindControl("txt_AddDescription") as TextBox).Text;
                lbltotaltax = (e.Row.FindControl("txtTax") as TextBox).Text;
                lblrate = (e.Row.FindControl("txtRate") as TextBox).Text;
                lblqty = (e.Row.FindControl("txtQuantity") as TextBox).Text;

                totaltax = Convert.ToDecimal(lbltotaltax);
                rate111111 = Convert.ToDecimal(lblrate);
                Qty = Convert.ToDecimal(lblqty);
                Total11177 = (Convert.ToDecimal(txt_Subtotal.Text) * totaltax) / 100;
                totalrateqty = Total11177;

            }
            else
            {

                lblproduct = (e.Row.FindControl("lblproduct") as Label).Text;
                lbladddescription = (e.Row.FindControl("lbl_AddDescription") as Label).Text;
                lblhsnsac = (e.Row.FindControl("lblhsnsac") as Label).Text;
                lbl_Rate = (e.Row.FindControl("lbl_Rate") as Label).Text;
                lbl_quntity_grd = (e.Row.FindControl("lbl_quntity_grd") as Label).Text;
                lblunit = (e.Row.FindControl("lblunit") as Label).Text;
                lbltotaltax = (e.Row.FindControl("lbl_Tax") as Label).Text;
                lbl_Discount = (e.Row.FindControl("lbl_Discount") as Label).Text;


                Amount = (e.Row.FindControl("txtTotal") as Label).Text;
                totaltax = Convert.ToDecimal(lbltotaltax);
                rate111111 = Convert.ToDecimal(lblrate);
                Qty = Convert.ToDecimal(lblqty);
                Total11177 = (Convert.ToDecimal(txt_Subtotal.Text) * totaltax) / 100;
                totalrateqty = Total11177;
            }
            if (lbltotaltax == "12")
            {
                taxpanel2.Visible = true;
                Totaltax12 += Convert.ToDecimal(Amount);
                if (txt_state.Text == "27 MAHARASHTRA")
                {
                    Label2.Text = ((Convert.ToDecimal(Totaltax12) * Convert.ToDecimal(lbltotaltax) / 100) / 2).ToString("0.00");
                    Label4.Text = ((Convert.ToDecimal(Totaltax12) * Convert.ToDecimal(lbltotaltax) / 100) / 2).ToString("0.00");
                    Label6.Text = Convert.ToDecimal(00).ToString("0.00");
                }
                else
                {
                    Label6.Text = ((Convert.ToDecimal(Totaltax12) * Convert.ToDecimal(lbltotaltax) / 100)).ToString("0.00");
                    Label2.Text = Convert.ToDecimal(00).ToString("0.00");
                    Label4.Text = Convert.ToDecimal(00).ToString("0.00");
                }
            }
            if (lbltotaltax == "18")
            {
                taxPanel1.Visible = true;
                Totaltax18 += Convert.ToDecimal(Amount);
                if (txt_state.Text == "27 MAHARASHTRA")
                {
                    txt_cgst9.Text = ((Convert.ToDecimal(Totaltax18) * Convert.ToDecimal(lbltotaltax) / 100) / 2).ToString("0.00");
                    txt_sgst9.Text = ((Convert.ToDecimal(Totaltax18) * Convert.ToDecimal(lbltotaltax) / 100) / 2).ToString("0.00");
                    txtigst.Text = Convert.ToDecimal(00).ToString("0.00");
                }
                else
                {
                    txtigst.Text = ((Convert.ToDecimal(Totaltax18) * Convert.ToDecimal(lbltotaltax) / 100)).ToString("0.00");
                    txt_cgst9.Text = Convert.ToDecimal(00).ToString("0.00");
                    txt_sgst9.Text = Convert.ToDecimal(00).ToString("0.00");
                }
            }
            if (lbltotaltax == "28")
            {
                txtpanel3.Visible = true;
                Totaltx28 += Convert.ToDecimal(Amount);
                if (txt_state.Text == "27 MAHARASHTRA")
                {
                    Label8.Text = ((Convert.ToDecimal(Totaltx28) * Convert.ToDecimal(lbltotaltax) / 100) / 2).ToString("0.00");
                    Label10.Text = ((Convert.ToDecimal(Totaltx28) * Convert.ToDecimal(lbltotaltax) / 100) / 2).ToString("0.00");
                    Label12.Text = Convert.ToDecimal(00).ToString("0.00");
                }
                else
                {
                    Label12.Text = ((Convert.ToDecimal(Totaltx28) * Convert.ToDecimal(lbltotaltax) / 100)).ToString("0.00");
                    Label8.Text = Convert.ToDecimal(00).ToString("0.00");
                    Label10.Text = Convert.ToDecimal(00).ToString("0.00");
                }
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            decimal grnd_TOTAL;
            if (string.IsNullOrEmpty(txt_Subtotal.Text))
            {
                grnd_TOTAL = 0;
            }
            else
            {
                decimal val1 = Convert.ToDecimal(txt_Subtotal.Text);
                decimal val2 = Convert.ToDecimal(txt_cgst9.Text);
                decimal val3 = Convert.ToDecimal(txt_sgst9.Text);
                decimal val4 = Convert.ToDecimal(txtigst.Text);
                decimal val5 = Convert.ToDecimal(Label2.Text);
                decimal val6 = Convert.ToDecimal(Label4.Text);
                decimal val7 = Convert.ToDecimal(Label6.Text);
                decimal val8 = Convert.ToDecimal(Label8.Text);
                decimal val9 = Convert.ToDecimal(Label10.Text);
                decimal val10 = Convert.ToDecimal(Label12.Text);

                grnd_TOTAL = (val1 + val2 + val3 + val4 + val5 + val6 + val7 + val8 + val9 + val10);
            }
            txt_grandTotal.Text = grnd_TOTAL.ToString("##.00");


            string isNegative = "";

            try
            {
                string number = txt_grandTotal.Text;

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
    }

    private void Show_Grid()
    {
        ViewState["RowNo"] = (int)ViewState["RowNo"] + 1;
        DataTable Dt = (DataTable)ViewState["QuotationComp"];
        Dt.Rows.Add(ViewState["RowNo"], txtpoduct.Text, txtadddescription.Text.Trim(), txt_hsn_Tbl.Text, txt_rate_Tbl.Text, txt_unit_Tbl.Text, txt_quntity_Tbl.Text, txt_tax_Tbl.Text,
        txt_discount_Tbl.Text, txt_Total_Tbl.Text, txt_total_amount_Tbl.Text);
        ViewState["QuotationComp"] = Dt;
        txt_hsn_Tbl.Text = string.Empty;
        txt_tax_Tbl.Text = string.Empty;
        txt_quntity_Tbl.Text = string.Empty;
        txt_unit_Tbl.Text = string.Empty;
        txt_Total_Tbl.Text = string.Empty;
        txt_rate_Tbl.Text = string.Empty;
        txt_discount_Tbl.Text = string.Empty;
        txt_total_amount_Tbl.Text = string.Empty;
        txtadddescription.Text = string.Empty;
        txtpoduct.Text = string.Empty;
        dgvProductDtl.DataSource = (DataTable)ViewState["QuotationComp"];
        dgvProductDtl.DataBind();
    }

    private void Table_Calculation()
    {
        decimal Qty;
        if (string.IsNullOrEmpty(txt_quntity_Tbl.Text))
        {
            Qty = 0;
        }
        else
        {
            var totalamt = Convert.ToDecimal(txt_quntity_Tbl.Text.Trim()) * Convert.ToDecimal(txt_rate_Tbl.Text.Trim());
            txt_Total_Tbl.Text = totalamt.ToString();
            txt_total_amount_Tbl.Text = totalamt.ToString();
        }

        decimal DiscountAmt;
        if (string.IsNullOrEmpty(txt_discount_Tbl.Text))
        {
            DiscountAmt = 0;
        }
        else
        {
            DiscountAmt = Convert.ToDecimal(txt_Total_Tbl.Text) * Convert.ToDecimal(txt_discount_Tbl.Text) / 100;
        }
        var GrossAmt = Convert.ToDecimal(txt_Total_Tbl.Text) - (DiscountAmt);
        txt_Total_Tbl.Text = GrossAmt.ToString("##.00");
    }

    protected void txt_discount_Tbl_TextChanged(object sender, EventArgs e)
    {
        Table_Calculation();
    }

    protected void txt_tax_Tbl_TextChanged(object sender, EventArgs e)
    {
        Table_Calculation();
    }

    protected void txt_quntity_Tbl_TextChanged(object sender, EventArgs e)
    {
        Table_Calculation();
    }

    protected void txt_rate_Tbl_TextChanged(object sender, EventArgs e)
    {
        if (txt_quntity_Tbl.Text == "")
        {
            txt_tax_Tbl.Text = "0";
            txt_discount_Tbl.Text = "0";
            txt_tax_Tbl.Text = "18";
        }
        else
        {
            Table_Calculation();
        }
    }

    protected void dgvProductDtl_RowEditing(object sender, GridViewEditEventArgs e)
    {
        dgvProductDtl.EditIndex = e.NewEditIndex;
        dgvProductDtl.DataSource = (DataTable)ViewState["QuotationComp"];
        dgvProductDtl.DataBind();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Success", "scrollToElement();", true);
    }

    protected void dgvProductDtl_RowUpdated(object sender, GridViewUpdatedEventArgs e)
    {

    }

    protected void gv_update_Click(object sender, EventArgs e)
    {
        GridViewRow row = (sender as LinkButton).NamingContainer as GridViewRow;

        string Product = ((TextBox)row.FindControl("txtmatename")).Text;
        string AddDescription = ((TextBox)row.FindControl("txt_AddDescription")).Text;
        string Tax = ((TextBox)row.FindControl("txtTax")).Text;
        string Quntity = ((TextBox)row.FindControl("txtQuantity")).Text;
        string Unit = ((TextBox)row.FindControl("txtunit")).Text;
        string Rate = ((TextBox)row.FindControl("txtRate")).Text;
        string HSN = ((TextBox)row.FindControl("txt_HSN_SAC")).Text;
        string Discount = ((TextBox)row.FindControl("txt_Discount")).Text;
        string TotalAmount = ((Label)row.FindControl("txtTotal")).Text;

        DataTable Dt = ViewState["QuotationComp"] as DataTable;

        Dt.Rows[row.RowIndex]["MateName"] = Product;
        Dt.Rows[row.RowIndex]["AddDescription"] = AddDescription;
        Dt.Rows[row.RowIndex]["Tax"] = Tax;
        Dt.Rows[row.RowIndex]["Quantity"] = Quntity;
        Dt.Rows[row.RowIndex]["Unit"] = Unit;
        Dt.Rows[row.RowIndex]["Rate"] = Rate;
        Dt.Rows[row.RowIndex]["HSN/SAC"] = HSN;
        Dt.Rows[row.RowIndex]["Discount"] = Discount;
        Dt.Rows[row.RowIndex]["Total"] = TotalAmount;

        Dt.AcceptChanges();

        ViewState["QuotationComp"] = Dt;
        dgvProductDtl.EditIndex = -1;

        dgvProductDtl.DataSource = (DataTable)ViewState["QuotationComp"];
        dgvProductDtl.DataBind();

        ScriptManager.RegisterStartupScript(this, this.GetType(), "Success", "scrollToElement();", true);
    }

    protected void gv_cancel_Click(object sender, EventArgs e)
    {
        GridViewRow row = (sender as LinkButton).NamingContainer as GridViewRow;


        string AddDescription = ((TextBox)row.FindControl("txt_AddDescription")).Text;
        string Tax = ((TextBox)row.FindControl("txtTax")).Text;
        string Quntity = ((TextBox)row.FindControl("txtQuantity")).Text;
        string Rate = ((TextBox)row.FindControl("txtRate")).Text;
        string Discount = ((TextBox)row.FindControl("txt_Discount")).Text;
        string TotalAmount = ((Label)row.FindControl("lblTotalPrice")).Text;

        DataTable Dt = ViewState["QuotationComp"] as DataTable;
        dgvProductDtl.EditIndex = -1;
        ViewState["QuotationComp"] = Dt;
        dgvProductDtl.EditIndex = -1;

        dgvProductDtl.DataSource = (DataTable)ViewState["QuotationComp"];
        dgvProductDtl.DataBind();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Success", "scrollToElement();", true);
    }

    protected void btn_add_more_Tbl_Click1(object sender, EventArgs e)
    {
        if (txt_hsn_Tbl.Text == "" || txt_rate_Tbl.Text == "" || txt_quntity_Tbl.Text == "" || txtadddescription.Text == "")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Please fill Component Information  !!!');", true);
        }
        else
        {
            Show_Grid();
        }
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


    protected void txt_Comp_name_TextChanged(object sender, EventArgs e)
    {
        jobnogrid();
        ddlsubcustomerbind();
        showCustDtl();
    }

    protected void txtRate_TextChanged(object sender, EventArgs e)
    {
        GridViewRow row = (sender as TextBox).NamingContainer as GridViewRow;
        Calculations(row);
    }

    protected void Grd_MAIL_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (btnSubmit.Text == "Update")
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                con.Open();
                int id = Convert.ToInt32(Grd_MAIL.DataKeys[e.Row.RowIndex].Values[0]);
                CheckBox chkmailupdate = (CheckBox)e.Row.FindControl("chkmail");
                Label mail = (Label)e.Row.FindControl("lblmultMail");
                SqlCommand cmd = new SqlCommand("select chkmail from QuotationMail where Quotation_no='" + txt_Quo_No.Text + "' AND id='" + id + "'", con);

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {

                    chkupdate = dr["chkmail"].ToString();
                    //con.Close();
                }
                chkmailupdate.Checked = chkupdate == "True" ? true : false;
                con.Close();
            }
        }
    }

    protected void lnkbtnAdd_Click(object sender, EventArgs e)
    {

        showCustDtl();
        Response.Redirect("Customer.aspx?Custid=" + encrypt(hiddencustomerID.Value) + "");
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

    protected void lnkbtnDelete_Click(object sender, EventArgs e)
    {
        GridViewRow row = (sender as LinkButton).NamingContainer as GridViewRow;
        DataTable dt = ViewState["QuotationComp"] as DataTable;
        dt.Rows.Remove(dt.Rows[row.RowIndex]);
        ViewState["QuotationComp"] = dt;
        dgvProductDtl.DataSource = (DataTable)ViewState["QuotationComp"];
        dgvProductDtl.DataBind();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Component Delete Succesfully !!!');", true);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Success", "scrollToElement();", true);
    }

    protected void grdjobno_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdjobno.PageIndex = e.NewPageIndex;
        jobnogrid();
    }

    protected void grdjobno_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (btnSubmit.Text == "Update")
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                con.Open();
                int Id = Convert.ToInt32(grdjobno.DataKeys[e.Row.RowIndex].Values[0]);
                CheckBox chkjobnoupdate = (CheckBox)e.Row.FindControl("chkjobno");
                Label jobno = (Label)e.Row.FindControl("lbljobno");
                SqlCommand cmd = new SqlCommand("select chkjobno from tbl_Quotationjobno where Jobno='" + Id + "' ", con);

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    chkupdate = dr["chkjobno"].ToString();
                    chkjobnoupdate.Checked = chkupdate == "True" ? true : false;
                }

                con.Close();
            }
        }
    }
}