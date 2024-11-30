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

public partial class Admin_Quotation_ListSales : System.Web.UI.Page
{
    string Id = "";
    DataTable dt11 = new DataTable();
    string UserCompany = "";
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["adminname"] == null)
            {
                Response.Redirect("../LoginPage.aspx");
            }
            else
            {
                ViewData();
                //gvbind_Company();
            }
        }
    }

    void gvbind_Company()
    {
        try
        {
            //string UserCompany = Session["name"].ToString();
            DataTable dt = new DataTable();
            SqlDataAdapter Da = new SqlDataAdapter("SELECT ID,Quotation_no,Quotation_Date,JobNo,Customer_Name,Address,Mobile_No,Phone_No,GST_No,State_Code,kind_Att,CGST,SGST,AllTotal_price,Total_in_word,IsDeleted,CreatedBy,CreatedOn,DATEDIFF(DAY, Quotation_Date, getdate()) AS days FROM tbl_Quotation_two_Hdr WHERE  IsDeleted='0' AND isCompleted='1'", con);
            Da.Fill(dt);
            gv_Quot_List.EmptyDataText = "Not Records Found";
            gv_Quot_List.DataSource = dt;
            gv_Quot_List.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void ViewData()
    {
        try
        {
            string role = Session["adminname"].ToString();
            if (role == "SubCustomer")
            {
                DataTable Dt = new DataTable();
                //original
                //SqlDataAdapter Da = new SqlDataAdapter("SELECT ID,Quotation_no,Quotation_Date,ExpiryDate,CreatedOn,JobNo,Customer_Name,SubCustomer,Address,Mobile_No,Phone_No,GST_No,State_Code,kind_Att,CGST,SGST,AllTotal_price,Total_in_word,IsDeleted,CreatedBy,CreatedOn,DATEDIFF(DAY, Quotation_Date, getdate()) AS days FROM tbl_Quotation_two_Hdr WHERE IsDeleted='0' AND isCompleted='1' ORDER BY Quotation_Date DESC ", con);
                //SqlDataAdapter Da = new SqlDataAdapter("SELECT ID,Quotation_no,Quotation_Date,ExpiryDate,CreatedOn,JobNo,Customer_Name,SubCustomer,Address,Mobile_No,Phone_No,GST_No,State_Code,kind_Att,CGST,SGST,AllTotal_price,Total_in_word,Againstby,IsDeleted,CreatedBy,CreatedOn,DATEDIFF(DAY, Quotation_Date, getdate()) AS days FROM tbl_Quotation_two_Hdr WHERE Againstby='Sales' AND Customer_Name='Schneider Electric India Pvt.Ltd.' AND IsDeleted='0'  ORDER BY Quotation_Date DESC  ", con);
                // SqlDataAdapter Da = new SqlDataAdapter("SELECT * FROM vw_Quotationjobno", con);

                SqlDataAdapter Da = new SqlDataAdapter(@"SELECT 
                    ID, 
                    Quotation_no, 
                    Quotation_Date, 
                    ExpiryDate, 
                    Q.CreatedOn AS QuotationCreatedOn, 
                    Customer_Name, 
                    SubCustomer, 
                    Address, 
                    Mobile_No, 
                    Phone_No, 
                    GST_No, 
                    State_Code, 
                    kind_Att, 
                    CGST, 
                    SGST, 
                    AllTotal_price, 
                    Total_in_word, 
                    Againstby, 
                    IsDeleted, 
                    Q.CreatedBy, 
                    Q.CreatedOn,
                    CASE 
                        WHEN Status = 'Pending' AND AgainstBy = 'Sales' THEN DATEDIFF(DAY, Quotation_Date, GETDATE())
                        WHEN Status = 'Completed' AND AgainstBy = 'Sales' THEN JobNoCount
                        ELSE NULL
                    END AS Counts
                    FROM 
                    tbl_Quotation_two_Hdr Q
                    WHERE 
                    AgainstBy = 'Sales' AND Customer_Name='Schneider Electric India Pvt.Ltd.'
                    AND IsDeleted = '0'
                    ORDER BY 
                    Q.CreatedOn DESC", con);

                Da.Fill(Dt);
                gv_Quot_List.DataSource = Dt;
                gv_Quot_List.DataBind();

            }
            else
            {
                DataTable Dt = new DataTable();
                // SqlDataAdapter Da = new SqlDataAdapter("SELECT ID,Quotation_no,Quotation_Date,ExpiryDate,CreatedOn,Customer_Name,SubCustomer,Address,Mobile_No,Phone_No,GST_No,State_Code,kind_Att,CGST,SGST,AllTotal_price,Total_in_word,IsDeleted,CreatedBy,CreatedOn,DATEDIFF(DAY, Quotation_Date, getdate()) AS days FROM tbl_Quotation_two_Hdr WHERE IsDeleted='0'  ORDER BY Quotation_Date DESC ", con);
                SqlDataAdapter Da = new SqlDataAdapter(@"SELECT 
                    ID, 
                    Quotation_no, 
                    Quotation_Date, 
                    ExpiryDate, 
                    Q.CreatedOn AS QuotationCreatedOn, 
                    Customer_Name, 
                    SubCustomer, 
                    Address, 
                    Mobile_No, 
                    Phone_No, 
                    GST_No, 
                    State_Code, 
                    kind_Att, 
                    CGST, 
                    SGST, 
                    AllTotal_price, 
                    Total_in_word, 
                    Againstby, 
                    IsDeleted, 
                    Q.CreatedBy, 
                    Q.CreatedOn,
                    CASE 
                        WHEN Status = 'Pending' AND AgainstBy = 'Sales' THEN DATEDIFF(DAY, Quotation_Date, GETDATE())
                        WHEN Status = 'Completed' AND AgainstBy = 'Sales' THEN JobNoCount
                        ELSE NULL
                    END AS Counts
                    FROM 
                    tbl_Quotation_two_Hdr Q
                    WHERE 
                    AgainstBy = 'Sales' 
                    AND IsDeleted = '0'
                    ORDER BY 
                    Q.CreatedOn DESC", con);

                Da.Fill(Dt);
                gv_Quot_List.DataSource = Dt;
                gv_Quot_List.DataBind();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void ExportExcelGrid()
    {
        try
        {
            string role = Session["adminname"].ToString();
            if (role == "SubCustomer")
            {
                DataTable Dt = new DataTable();
                //original
                //SqlDataAdapter Da = new SqlDataAdapter("SELECT ID,Quotation_no,Quotation_Date,ExpiryDate,CreatedOn,JobNo,Customer_Name,SubCustomer,Address,Mobile_No,Phone_No,GST_No,State_Code,kind_Att,CGST,SGST,AllTotal_price,Total_in_word,IsDeleted,CreatedBy,CreatedOn,DATEDIFF(DAY, Quotation_Date, getdate()) AS days FROM tbl_Quotation_two_Hdr WHERE IsDeleted='0' AND isCompleted='1' ORDER BY Quotation_Date DESC ", con);
                SqlDataAdapter Da = new SqlDataAdapter("SELECT ID,Quotation_no,Quotation_Date,ExpiryDate,CreatedOn,JobNo,Customer_Name,SubCustomer,Address,Mobile_No,Phone_No,GST_No,State_Code,kind_Att,CGST,SGST,AllTotal_price,Total_in_word,IsDeleted,CreatedBy,CreatedOn,DATEDIFF(DAY, Quotation_Date, getdate()) AS days FROM tbl_Quotation_two_Hdr WHERE Customer_Name='Schneider Electric India Pvt.Ltd.' AND IsDeleted='0' AND Againstby='Sales' ORDER BY Quotation_Date DESC ", con);
                // SqlDataAdapter Da = new SqlDataAdapter("SELECT * FROM vw_Quotationjobno", con);
                // SqlDataAdapter Da = new SqlDataAdapter("SELECT ID, Quotation_no, Quotation_Date, ExpiryDate,CreatedOn,JobNo,Customer_Name,SubCustomer,Address,Mobile_No,GST_No,State_Code,kind_Att, CGST,SGST,AllTotal_price,Total_in_word,IsDeleted,CreatedBy,CreatedOn,DATEDIFF(DAY, Quotation_Date, getdate()) AS days FROM tbl_Quotation_two_Hdr WHERE Customer_Name = "Schneder Electric India Pvt.Ltd.' AND IsDeleted='0' ORDER BY Quotation_Date DESC",con);

                Da.Fill(Dt);
                ExportGrid.DataSource = Dt;
                ExportGrid.DataBind();
            }
            else
            {
                DataTable Dt = new DataTable();
                //original
                //SqlDataAdapter Da = new SqlDataAdapter("SELECT ID,Quotation_no,Quotation_Date,ExpiryDate,CreatedOn,JobNo,Customer_Name,SubCustomer,Address,Mobile_No,Phone_No,GST_No,State_Code,kind_Att,CGST,SGST,AllTotal_price,Total_in_word,IsDeleted,CreatedBy,CreatedOn,DATEDIFF(DAY, Quotation_Date, getdate()) AS days FROM tbl_Quotation_two_Hdr WHERE IsDeleted='0' AND isCompleted='1' ORDER BY Quotation_Date DESC ", con);
                SqlDataAdapter Da = new SqlDataAdapter("SELECT ID,Quotation_no,Quotation_Date,ExpiryDate,CreatedOn,JobNo,Customer_Name,SubCustomer,Address,Mobile_No,Phone_No,GST_No,State_Code,kind_Att,CGST,SGST,AllTotal_price,Total_in_word,IsDeleted,CreatedBy,CreatedOn,DATEDIFF(DAY, Quotation_Date, getdate()) AS days FROM tbl_Quotation_two_Hdr WHERE IsDeleted='0' AND Againstby='Sales'  ORDER BY Quotation_Date DESC ", con);
                // SqlDataAdapter Da = new SqlDataAdapter("SELECT * FROM vw_Quotationjobno", con);

                Da.Fill(Dt);
                ExportGrid.DataSource = Dt;
                ExportGrid.DataBind();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }



    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetCustomerList(string prefixText, int count)
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
                com.CommandText = "select DISTINCT Customer_Name from tbl_Quotation_two_Hdr where " + "Customer_Name like @Search + '%'AND IsDeleted='0'AND Againstby = 'Sales'";

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
    public static List<string> GetJobList(string prefixText, int count)
    {
        return AutoFilljoblist(prefixText);
    }
    public static List<string> AutoFilljoblist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT Quotation_no from tbl_Quotation_two_Hdr where " + "Quotation_no like @Search + '%'AND IsDeleted='0' And Againstby = 'Sales'";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> Quotation_no = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        Quotation_no.Add(sdr["Quotation_no"].ToString());
                    }
                }
                con.Close();
                return Quotation_no;
            }
        }
    }

    SqlDataAdapter Da;
    protected void lnkBtnsearch_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            if (string.IsNullOrEmpty(txtSearch.Text) && string.IsNullOrEmpty(txtDateSearch.Text) && string.IsNullOrEmpty(txtDateSearchfrom.Text) && string.IsNullOrEmpty(txtDateSearchto.Text) && string.IsNullOrEmpty(txtquotation.Text))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Please Search Company Name !!!');", true);
                ViewData();
            }

            //From Date To ToDate
            else if (!string.IsNullOrEmpty(txtDateSearchfrom.Text) && !string.IsNullOrEmpty(txtDateSearchto.Text) && string.IsNullOrEmpty(txtSearch.Text))
            {
                ViewState["Excell"] = "GetsortedDatewise";
                GetsortedDatewise();
                //DataTable dtt = new DataTable();
                //SqlDataAdapter sad = new SqlDataAdapter("select ID,Quotation_no,Quotation_Date,ExpiryDate,JobNo,Customer_Name,SubCustomer,Address,Mobile_No,Phone_No,GST_No,State_Code,kind_Att,CGST,SGST,AllTotal_price,Total_in_word,IsDeleted,CreatedBy,CreatedOn, DATEDIFF(DAY, Quotation_Date, getdate()) AS days from tbl_Quotation_two_Hdr where IsDeleted='0' and Quotation_Date between '" + txtDateSearchfrom.Text + "' AND  '" + txtDateSearchto.Text + "' ", con);
                //sad.Fill(dtt);
                //gv_Quot_List.EmptyDataText = "Records Not Found";
                //gv_Quot_List.DataSource = dtt;
                //gv_Quot_List.DataBind();
            }

            //Quotation no search
            else if (!string.IsNullOrEmpty(txtquotation.Text))
            {
                ViewState["Excell"] = "GetsortedQuation";
                GetsortedQuation();
                //DataTable dtt = new DataTable();
                //SqlDataAdapter sad = new SqlDataAdapter("select ID,Quotation_no,Quotation_Date,ExpiryDate,JobNo,Customer_Name,SubCustomer,Address,Mobile_No,Phone_No,GST_No,State_Code,kind_Att,CGST,SGST,AllTotal_price,Total_in_word,IsDeleted,CreatedBy,CreatedOn, DATEDIFF(DAY, Quotation_Date, getdate()) AS days from tbl_Quotation_two_Hdr where Quotation_no='" + txtquotation.Text + "'", con);
                //sad.Fill(dtt);
                //gv_Quot_List.EmptyDataText = "Records Not Found";
                //gv_Quot_List.DataSource = dtt;
                //gv_Quot_List.DataBind();
            }

            //Company name search
            else if (!string.IsNullOrEmpty(txtSearch.Text) && string.IsNullOrEmpty(txtDateSearchfrom.Text))
            {
                ViewState["Excell"] = "GetsortedCsutomer";
                GetsortedCsutomer();
                //DataTable dtt = new DataTable();
                //SqlDataAdapter sad = new SqlDataAdapter("select ID,Quotation_no,Quotation_Date,ExpiryDate,JobNo,Customer_Name,SubCustomer,Address,Mobile_No,Phone_No,GST_No,State_Code,kind_Att,CGST,SGST,AllTotal_price,Total_in_word,IsDeleted,CreatedBy,CreatedOn, DATEDIFF(DAY, Quotation_Date, getdate()) AS days from tbl_Quotation_two_Hdr where  Customer_Name='" + txtSearch.Text + "'", con);
                //sad.Fill(dtt);
                //gv_Quot_List.EmptyDataText = "Records Not Found";
                //gv_Quot_List.DataSource = dtt;
                //gv_Quot_List.DataBind();
            }

            //Date wise search
            else if (!string.IsNullOrEmpty(txtDateSearch.Text))
            {
                ViewState["Excell"] = "DateSearch";
                DataTable dtt = new DataTable();
                SqlDataAdapter sad = new SqlDataAdapter("select ID,Quotation_no,Quotation_Date,ExpiryDate,JobNo,Customer_Name,SubCustomer,Address,Mobile_No,Phone_No,GST_No,State_Code,kind_Att,CGST,SGST,AllTotal_price,Total_in_word,IsDeleted,CreatedBy,CreatedOn, DATEDIFF(DAY, Quotation_Date, getdate()) AS days from tbl_Quotation_two_Hdr where Quotation_Date='" + txtDateSearch.Text + "' AND isdeleted = '0'", con);
                sad.Fill(dtt);
                gv_Quot_List.EmptyDataText = "Records Not Found";
                gv_Quot_List.DataSource = dtt;
                gv_Quot_List.DataBind();
            }


            else if (!string.IsNullOrEmpty(txtSearch.Text) && !string.IsNullOrEmpty(txtDateSearchfrom.Text))
            {
                GetDatewiseCsutomer();
            }

            //else
            //{
            //    if (!string.IsNullOrEmpty(txtSearch.Text) && string.IsNullOrEmpty(txtDateSearch.Text))
            //    {
            //        foreach (GridViewRow g1 in gv_Quot_List.Rows)
            //        {
            //            Label lbldaycount = (Label)g1.FindControl("lbldaycount");
            //            Label jobnono = (Label)g1.FindControl("lblJobNo");
            //            string jobno = jobnono.Text;
            //            con.Open();
            //            SqlCommand cmdquatationpo = new SqlCommand("select  c.CreatedOn from tbl_Quotation_two_Hdr q inner join CustomerPO_Hdr c on q.JobNo=c.JobNo where c.JobNo='" + jobno + "'", con);
            //            SqlDataReader reader = cmdquatationpo.ExecuteReader();
            //            if (reader.Read())
            //            {
            //                DateTime ffff1 = Convert.ToDateTime(reader["CreatedOn"].ToString());
            //                string update = ffff1.ToString("yyyy-MM-dd");

            //                con.Close();
            //                Da = new SqlDataAdapter("select ID,Quotation_no,Quotation_Date,ExpiryDate,JobNo,Customer_Name,SubCustomer,Address,Mobile_No,Phone_No,GST_No,State_Code,kind_Att,CGST,SGST,AllTotal_price,Total_in_word,IsDeleted,CreatedBy,CreatedOn, DATEDIFF(DAY, Quotation_Date, '" + update + "') AS days from tbl_Quotation_two_Hdr where JobNo='" + jobno + "' AND IsDeleted='0' AND isCompleted='1' AND Customer_Name='" + txtSearch.Text + "' ", con);
            //                Da.Fill(dt);
            //            }
            //            else
            //            {
            //                con.Close();
            //                Da = new SqlDataAdapter("select ID,Quotation_no,Quotation_Date,ExpiryDate,JobNo,Customer_Name,SubCustomer,Address,Mobile_No,Phone_No,GST_No,State_Code,kind_Att,CGST,SGST,AllTotal_price,Total_in_word,IsDeleted,CreatedBy,CreatedOn, DATEDIFF(DAY, Quotation_Date, getdate()) AS days from tbl_Quotation_two_Hdr where JobNo='" + jobno + "' AND IsDeleted='0' AND isCompleted='1' AND Customer_Name='" + txtSearch.Text + "'", con);
            //                Da.Fill(dt);
            //                lbldaycount.ForeColor = System.Drawing.Color.Red;
            //            }
            //        }
            //    }
            //    else if (string.IsNullOrEmpty(txtSearch.Text) && !string.IsNullOrEmpty(txtDateSearch.Text))
            //    {
            //        foreach (GridViewRow g1 in gv_Quot_List.Rows)
            //        {
            //            Label lbldaycount = (Label)g1.FindControl("lbldaycount");
            //            Label jobnono = (Label)g1.FindControl("lblJobNo");
            //            string jobno = jobnono.Text;
            //            con.Open();
            //            SqlCommand cmdquatationpo = new SqlCommand("select  c.CreatedOn from tbl_Quotation_two_Hdr q inner join CustomerPO_Hdr c on q.JobNo=c.JobNo where c.JobNo='" + jobno + "'", con);
            //            SqlDataReader reader = cmdquatationpo.ExecuteReader();
            //            if (reader.Read())
            //            {
            //                DateTime ffff1 = Convert.ToDateTime(reader["CreatedOn"].ToString());
            //                string update = ffff1.ToString("yyyy-MM-dd");

            //                con.Close();
            //                Da = new SqlDataAdapter("select ID,Quotation_no,Quotation_Date,ExpiryDate,JobNo,Customer_Name,SubCustomer,Address,Mobile_No,Phone_No,GST_No,State_Code,kind_Att,CGST,SGST,AllTotal_price,Total_in_word,IsDeleted,CreatedBy,CreatedOn, DATEDIFF(DAY, Quotation_Date, '" + update + "') AS days from tbl_Quotation_two_Hdr where JobNo='" + jobno + "' AND IsDeleted='0' AND isCompleted='1'  AND Quotation_Date='" + txtDateSearch.Text + "' ", con);
            //                Da.Fill(dt);
            //            }
            //            else
            //            {
            //                con.Close();
            //                Da = new SqlDataAdapter("select ID,Quotation_no,Quotation_Date,ExpiryDate,JobNo,Customer_Name,SubCustomer,Address,Mobile_No,Phone_No,GST_No,State_Code,kind_Att,CGST,SGST,AllTotal_price,Total_in_word,IsDeleted,CreatedBy,CreatedOn, DATEDIFF(DAY, Quotation_Date, getdate()) AS days from tbl_Quotation_two_Hdr where JobNo='" + jobno + "' AND IsDeleted='0' AND isCompleted='1' AND Quotation_Date='" + txtDateSearch.Text + "'", con);
            //                Da.Fill(dt);
            //                lbldaycount.ForeColor = System.Drawing.Color.Red;
            //            }
            //        }
            //    }
            //    else if (!string.IsNullOrEmpty(txtSearch.Text) && !string.IsNullOrEmpty(txtDateSearch.Text))
            //    {

            //        foreach (GridViewRow g1 in gv_Quot_List.Rows)
            //        {
            //            Label lbldaycount = (Label)g1.FindControl("lbldaycount");
            //            Label jobnono = (Label)g1.FindControl("lblJobNo");
            //            string jobno = jobnono.Text;
            //            con.Open();
            //            SqlCommand cmdquatationpo = new SqlCommand("select  c.CreatedOn from tbl_Quotation_two_Hdr q inner join CustomerPO_Hdr c on q.JobNo=c.JobNo where c.JobNo='" + jobno + "'", con);
            //            SqlDataReader reader = cmdquatationpo.ExecuteReader();
            //            if (reader.Read())
            //            {
            //                DateTime ffff1 = Convert.ToDateTime(reader["CreatedOn"].ToString());
            //                string update = ffff1.ToString("yyyy-MM-dd");

            //                con.Close();
            //                Da = new SqlDataAdapter("select ID,Quotation_no,Quotation_Date,ExpiryDate,JobNo,Customer_Name,SubCustomer,Address,Mobile_No,Phone_No,GST_No,State_Code,kind_Att,CGST,SGST,AllTotal_price,Total_in_word,IsDeleted,CreatedBy,CreatedOn, DATEDIFF(DAY, Quotation_Date, '" + update + "') AS days from tbl_Quotation_two_Hdr where JobNo='" + jobno + "' AND IsDeleted='0' AND isCompleted='1' AND  Quotation_Date='" + txtDateSearch.Text + "' AND Customer_Name='" + txtSearch.Text + "' ", con);
            //                Da.Fill(dt);
            //            }
            //            else
            //            {
            //                con.Close();
            //                Da = new SqlDataAdapter("select ID,Quotation_no,Quotation_Date,ExpiryDateJobNo,Customer_Name,SubCustomer,Address,Mobile_No,Phone_No,GST_No,State_Code,kind_Att,CGST,SGST,AllTotal_price,Total_in_word,IsDeleted,CreatedBy,CreatedOn, DATEDIFF(DAY, Quotation_Date, getdate()) AS days from tbl_Quotation_two_Hdr where JobNo='" + jobno + "' AND IsDeleted='0' AND isCompleted='1' AND Quotation_Date='" + txtDateSearch.Text + "' AND Customer_Name='" + txtSearch.Text + "'", con);
            //                Da.Fill(dt);
            //                lbldaycount.ForeColor = System.Drawing.Color.Red;
            //            }
            //        }
            //    }

            //    //------------------------------------------
            //    /// from date to date filter
            //    //else if (!string.IsNullOrEmpty(txtDateSearchfrom.Text) && !string.IsNullOrEmpty(txtDateSearchto.Text)) 
            //    //{

            //    //    foreach (GridViewRow g1 in gv_Quot_List.Rows)
            //    //    {
            //    //        Label lbldaycount = (Label)g1.FindControl("lbldaycount");
            //    //        Label jobnono = (Label)g1.FindControl("lblJobNo");
            //    //        string jobno = jobnono.Text;
            //    //        con.Open();
            //    //        SqlCommand cmdquatationpo = new SqlCommand("select  c.CreatedOn from tbl_Quotation_two_Hdr q inner join CustomerPO_Hdr c on q.JobNo=c.JobNo where c.JobNo='" + jobno + "'", con);
            //    //        SqlDataReader reader = cmdquatationpo.ExecuteReader();
            //    //        if (reader.Read())
            //    //        {
            //    //            DateTime ffff1 = Convert.ToDateTime(reader["CreatedOn"].ToString());
            //    //            string update = ffff1.ToString("yyyy-MM-dd");

            //    //            con.Close();
            //    //            Da = new SqlDataAdapter("select ID,Quotation_no,Quotation_Date,ExpiryDate,JobNo,Customer_Name,SubCustomer,Address,Mobile_No,Phone_No,GST_No,State_Code,kind_Att,CGST,SGST,AllTotal_price,Total_in_word,IsDeleted,CreatedBy,CreatedOn, DATEDIFF(DAY, Quotation_Date, '" + update + "') AS days from tbl_Quotation_two_Hdr where Quotation_Date between '" + txtDateSearchfrom.Text + "' AND  '" + txtDateSearchto.Text + "' ", con);
            //    //        }
            //    //        else
            //    //        {
            //    //            con.Close();
            //    //            Da = new SqlDataAdapter("SELECT * FROM [tbl_Quotation_two_Hdr] WHERE Quotation_Date between '" + txtDateSearchfrom.Text + "' AND  '" + txtDateSearchto.Text + "' ", con);
            //    //            // Da = new SqlDataAdapter("select ID,Quotation_no,Quotation_Date,ExpiryDate,JobNo,Customer_Name,SubCustomer,Address,Mobile_No,Phone_No,GST_No,State_Code,kind_Att,CGST,SGST,AllTotal_price,Total_in_word,IsDeleted,CreatedBy,CreatedOn, DATEDIFF(DAY, Quotation_Date, getdate()) AS days from tbl_Quotation_two_Hdr where Quotation_Date between '" + txtDateSearchfrom.Text + "' AND  '" + txtDateSearchto.Text + "' ", con);
            //    //            Da.Fill(dt);
            //    //            //lbldaycount.ForeColor = System.Drawing.Color.Red;
            //    //        }
            //    //    }
            //    //}




            //    //else if (!string.IsNullOrEmpty(txtDateSearchfrom.Text) && string.IsNullOrEmpty(txtDateSearchto.Text))
            //    //{
            //    //    //DataTable dtt = new DataTable();


            //    //    SqlDataAdapter sad = new SqlDataAdapter("SELECT *  AS days FROM [tbl_Quotation_two_Hdr]  Quotation_Date ='" + txtDateSearchfrom.Text + "' ", con);
            //    //    sad.Fill(dt);

            //    //    gv_Quot_List.EmptyDataText = "Not Records Found";
            //    //    gv_Quot_List.DataSource = dt;
            //    //    gv_Quot_List.DataBind();


            //    //    //Evalution();
            //    //    //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Enter To Date!!');", true);

            //    //}
            //    //else if (string.IsNullOrEmpty(txtDateSearchfrom.Text) && !string.IsNullOrEmpty(txtDateSearchto.Text))
            //    //{
            //    //    //DataTable dtt = new DataTable();


            //    //    SqlDataAdapter sad = new SqlDataAdapter("SELECT *  FROM [tbl_Quotation_two_Hdr]  Quotation_Date ='" + txtDateSearchto.Text + "' ", con);
            //    //    sad.Fill(dt);

            //    //    gv_Quot_List.EmptyDataText = "Not Records Found";
            //    //    gv_Quot_List.DataSource = dt;
            //    //    gv_Quot_List.DataBind();


            //    //    //Evalution();
            //    //    //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Enter From Date!!');", true);

            //    //}
            //    //else if (!string.IsNullOrEmpty(txtDateSearchfrom.Text) && !string.IsNullOrEmpty(txtDateSearchto.Text))
            //    //{
            //    //    //DataTable dtt = new DataTable();

            //    //    SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM [tbl_Quotation_two_Hdr] WHERE  Quotation_Date  between '" + txtDateSearchfrom.Text + "' AND  '" + txtDateSearchto.Text + "' ", con);
            //    //    sad.Fill(dt);

            //    //    gv_Quot_List.EmptyDataText = "Not Records Found";
            //    //    gv_Quot_List.DataSource = dt;
            //    //    gv_Quot_List.DataBind();
            //    //}

            //    //--------------------end----------------------

            //    //else if (!string.IsNullOrEmpty(txtquotation.Text))
            //    //{

            //    //    foreach (GridViewRow g1 in gv_Quot_List.Rows)
            //    //    {
            //    //        Label lbldaycount = (Label)g1.FindControl("lbldaycount");
            //    //        Label jobnono = (Label)g1.FindControl("lblJobNo");
            //    //        string Quotationno = txtquotation.Text;
            //    //        con.Open();
            //    //        SqlCommand cmdquatationpo = new SqlCommand("select  c.CreatedOn from tbl_Quotation_two_Hdr q inner join CustomerPO_Hdr c on q.JobNo=c.JobNo where c.Quotation_no='" + Quotationno + "'", con);
            //    //        SqlDataReader reader = cmdquatationpo.ExecuteReader();
            //    //        if (reader.Read())
            //    //        {
            //    //            DateTime ffff1 = Convert.ToDateTime(reader["CreatedOn"].ToString());
            //    //            string update = ffff1.ToString("yyyy-MM-dd");

            //    //            con.Close();
            //    //            Da = new SqlDataAdapter("select ID,Quotation_no,Quotation_Date,ExpiryDate,JobNo,Customer_Name,SubCustomer,Address,Mobile_No,Phone_No,GST_No,State_Code,kind_Att,CGST,SGST,AllTotal_price,Total_in_word,IsDeleted,CreatedBy,CreatedOn, DATEDIFF(DAY, Quotation_Date, '" + update + "') AS days from tbl_Quotation_two_Hdr where Quotation_no='" + Quotationno + "' AND IsDeleted='0' AND isCompleted='1' AND  Quotation_no='" + txtquotation.Text + "' ", con);
            //    //            Da.Fill(dt);
            //    //        }
            //    //        else
            //    //        {
            //    //            con.Close();
            //    //            Da = new SqlDataAdapter("select ID,Quotation_no,Quotation_Date,ExpiryDate,JobNo,Customer_Name,SubCustomer,Address,Mobile_No,Phone_No,GST_No,State_Code,kind_Att,CGST,SGST,AllTotal_price,Total_in_word,IsDeleted,CreatedBy,CreatedOn, DATEDIFF(DAY, Quotation_Date, getdate()) AS days from tbl_Quotation_two_Hdr where Quotation_no='" + Quotationno + "' AND IsDeleted='0' AND isCompleted='1' AND Quotation_no='" + txtquotation.Text + "'", con);
            //    //            Da.Fill(dt);
            //    //            lbldaycount.ForeColor = System.Drawing.Color.Red;
            //    //        }
            //    //    }
            //    //}
            //    gv_Quot_List.EmptyDataText = "Records Not Found";
            //    gv_Quot_List.DataSource = dt;
            //    gv_Quot_List.DataBind();
            //}
        }
        catch (Exception ex)
        {
            throw ex;
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

    protected void gv_Quot_List_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "RowQuotation")
            {
                Response.Redirect("Quotation_Sales.aspx?JobNo=" + encrypt(e.CommandArgument.ToString()) + "");
            }
            if (e.CommandName == "RowEdit")
            {
                Response.Redirect("Quotation_Sales.aspx?Quotation_no=" + encrypt(e.CommandArgument.ToString()) + "");
            }
            if (e.CommandName == "RowDelete")
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("UPDATE tbl_Quotation_two_Hdr SET IsDeleted='1' WHERE ID=@ID", con);
                cmd.Parameters.AddWithValue("@ID", Convert.ToInt32(e.CommandArgument.ToString()));
                cmd.Parameters.AddWithValue("@IsDeleted", '1');
                cmd.ExecuteNonQuery();
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Deleted Sucessfully');", true);
            }
            if (e.CommandName == "RowView")
            {
                string qno = e.CommandArgument.ToString();
                Response.Write("<script>window.open ('../reportPdf/QuatationPdfSales.aspx?Quotation_no=" + encrypt(e.CommandArgument.ToString()) + "','_blank');</script>");
            }
            if (e.CommandName == "SendPO")
            {
                Response.Redirect("Customer_PO_Sales.aspx?Quotation_no=" + encrypt(e.CommandArgument.ToString()) + "");
            }
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }
    string status;
    SqlDataAdapter sadquatation;
    protected void gv_Quot_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //Added New for Count by Shubham Patil
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            decimal totalAmount = 0;

            if (sortedgv.Rows.Count > 0)
            {
                foreach (GridViewRow row in sortedgv.Rows)
                {

                    Label lblTotalAmt = row.FindControl("lbltotalAmt") as Label;
                    if (lblTotalAmt != null)
                    {
                        if (decimal.TryParse(lblTotalAmt.Text, out decimal rowAmount))
                        {
                            totalAmount += rowAmount;
                        }
                    }
                }
            }
            else
            {
                foreach (GridViewRow row in gv_Quot_List.Rows)
                {
                    Label lblTotalAmt = row.FindControl("lbltotalAmt") as Label;
                    if (lblTotalAmt != null)
                    {
                        if (decimal.TryParse(lblTotalAmt.Text, out decimal rowAmount))
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
            LinkButton Lnk_Edit = (LinkButton)e.Row.FindControl("lnkbtnEdit");
            LinkButton Lnk_Create = (LinkButton)e.Row.FindControl("lnkCreateQua");
            LinkButton Lnk_vw_Quo = (LinkButton)e.Row.FindControl("lnkBtn_View");
            Label lbl_Quo_no = (Label)e.Row.FindControl("lblQuNo");
            Label lblcompanyname = (Label)e.Row.FindControl("lblCompName");
            Label lblsubcustomer = (Label)e.Row.FindControl("lblsubcustomer");

            SqlDataAdapter Da = new SqlDataAdapter("SELECT Quotation_no From tbl_Quotation_two_Hdr WHERE Quotation_no='" + lbl_Quo_no + "'", con);
            DataTable Dt = new DataTable();
            Da.Fill(Dt);
            if (!string.IsNullOrWhiteSpace(lblcompanyname.Text))
            {
                Lnk_Edit.Enabled = true;
                //  Lnk_Create.Enabled = false;
                Lnk_vw_Quo.Enabled = true;
            }
            else
            {
                Lnk_Edit.Enabled = false;
                // Lnk_Create.Enabled = true;
                Lnk_vw_Quo.Enabled = false;
            }

            string Id = gv_Quot_List.DataKeys[e.Row.RowIndex].Value.ToString();
            //GridView gvDetails = e.Row.FindControl("gvDetails") as GridView;

            //SqlDataAdapter Daaa = new SqlDataAdapter(" SELECT * FROM [tbl_Quotation_Dtl] WHERE [Quotation_no]='" + Id + "'", con);
            //DataTable Dttt = new DataTable();
            //Daaa.Fill(Dttt);
            //gvDetails.DataSource = Dttt;
            //gvDetails.DataBind();

        }
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    Label jobnono = (Label)e.Row.FindControl("lblJobNo");
        //    string jobno = jobnono.Text;
        //    con.Open();
        //    SqlCommand cmdquatationpo = new SqlCommand("select  c.CreatedOn from tbl_Quotation_two_Hdr q inner join CustomerPO_Hdr c on q.JobNo=c.JobNo where c.JobNo='" + jobno + "'", con);
        //    SqlDataReader reader = cmdquatationpo.ExecuteReader();
        //    if (reader.Read())
        //    {
        //        DateTime ffff1 = Convert.ToDateTime(reader["CreatedOn"].ToString());
        //        string update = ffff1.ToString("yyyy-MM-dd");
        //        con.Close();
        //        sadquatation = new SqlDataAdapter("select ID,Quotation_no,Quotation_Date,ExpiryDate,JobNo,Customer_Name,SubCustomer,Address,Mobile_No,Phone_No,GST_No,State_Code,kind_Att,CGST,SGST,AllTotal_price,Total_in_word,IsDeleted,CreatedBy,CreatedOn, DATEDIFF(DAY, Quotation_Date, '" + update + "') AS days from tbl_Quotation_two_Hdr where JobNo='" + jobno + "' AND IsDeleted='0' AND isCompleted='1' ", con);
        //        sadquatation.Fill(dt11);

        //    }
        //    else
        //    {
        //        con.Close();
        //        sadquatation = new SqlDataAdapter("select ID,Quotation_no,Quotation_Date,ExpiryDate,JobNo,Customer_Name,SubCustomer,Address,Mobile_No,Phone_No,GST_No,State_Code,kind_Att,CGST,SGST,AllTotal_price,Total_in_word,IsDeleted,CreatedBy,CreatedOn, DATEDIFF(DAY, Quotation_Date, getdate()) AS days from tbl_Quotation_two_Hdr where JobNo='" + jobno + "' AND IsDeleted='0' AND isCompleted='1'", con);
        //        sadquatation.Fill(dt11);
        //        e.Row.Cells[9].ForeColor = System.Drawing.Color.Red;
        //    }
        //}
    }

    protected void lnkBtn_rfresh_Click(object sender, EventArgs e)
    {
        //Response.Redirect("QuotationList.aspx");
        Response.Redirect("Quotation_ListSales.aspx");
    }

    public DataTable Read_Table(string Query)
    {
        DataTable Dt = new DataTable();
        SqlCommand cmd = new SqlCommand(Query, con);
        SqlDataAdapter Da = new SqlDataAdapter(cmd);
        Da.Fill(Dt);
        return Dt;
    }

    int iddd;
    string Email;
    //public void PDF(string Quo_NO)
    //{

    //    SqlDataAdapter Da = new SqlDataAdapter("SELECT * FROM vw_Quot_pdf WHERE Quotation_no='" + Quo_NO + "' ", con);
    //    DataTable Dt = new DataTable();
    //    Da.Fill(Dt);

    //    StringWriter sw = new StringWriter();
    //    StringReader sr = new StringReader(sw.ToString());



    //    Document doc = new Document(PageSize.A4, 10f, 10f, 55f, 0f);


    //    PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(Server.MapPath("~/Files/") + "QuotationInvoice.pdf", FileMode.Create));
    //    //PdfWriter writer = PdfWriter.GetInstance(doc, Response.OutputStream);
    //    XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, sr);

    //    doc.Open();

    //    string imageURL = Server.MapPath("~") + "/image/AA.png";

    //    iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(imageURL);

    //    //Resize image depend upon your need

    //    png.ScaleToFit(70, 100);

    //    //For Image Position
    //    png.SetAbsolutePosition(40, 745);
    //    //var document = new Document();

    //    //Give space before image
    //    //png.ScaleToFit(document.PageSize.Width - (document.RightMargin * 100), 50);
    //    png.SpacingBefore = 50f;

    //    //Give some space after the image

    //    png.SpacingAfter = 1f;

    //    png.Alignment = Element.ALIGN_LEFT;

    //    //paragraphimage.Add(png);
    //    //doc.Add(paragraphimage);
    //    doc.Add(png);


    //    PdfContentByte cb = writer.DirectContent;
    //    cb.Rectangle(17f, 735f, 560f, 60f);
    //    cb.Stroke();
    //    // Header 
    //    cb.BeginText();
    //    cb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 20);
    //    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "ENDEAVOUR AUTOMATION", 185, 773, 0);
    //    cb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 11);
    //    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Survey No. 27, Nilambkar Nagar, Near Raghunandan Karyalay,", 155, 755, 0);
    //    cb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 11);
    //    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Tathawade, Pune-411033", 232, 740, 0);
    //    cb.EndText();

    //    PdfContentByte cbb = writer.DirectContent;
    //    cbb.Rectangle(17f, 710f, 560f, 25f);
    //    cbb.Stroke();
    //    // Header 
    //    cbb.BeginText();
    //    cbb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
    //    cbb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " Mob: 9860502108    Email: endeavour.automations@gmail.com ", 153, 722, 0);
    //    cbb.EndText();

    //    PdfContentByte cbbb = writer.DirectContent;
    //    cbbb.Rectangle(17f, 685f, 560f, 25f);
    //    cbbb.Stroke();
    //    // Header 
    //    cbbb.BeginText();
    //    cbbb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
    //    cbbb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "GSTIN :  27AFYPJ3488G1ZQ ", 30, 695, 0);
    //    cbbb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
    //    cbbb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "W.E.F. :  01/07/2017", 185, 695, 0);
    //    cbbb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
    //    cbbb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "PAN No. :  AFYPJ3488G", 310, 695, 0);
    //    cbbb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
    //    cbbb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "State Code :  27 Maharashtra", 440, 695, 0);
    //    cbbb.EndText();

    //    PdfContentByte cd = writer.DirectContent;
    //    cd.Rectangle(17f, 660f, 560f, 25f);
    //    cd.Stroke();
    //    // Header 
    //    cd.BeginText();
    //    cd.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 17);
    //    cd.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Quotation", 260, 667, 0);
    //    cd.EndText();

    //    Paragraph paragraphTable1 = new Paragraph();
    //    paragraphTable1.SpacingBefore = 120f;
    //    paragraphTable1.SpacingAfter = 10f;

    //    PdfPTable table = new PdfPTable(4);

    //    float[] widths2 = new float[] { 100, 180, 100, 180 };
    //    table.SetWidths(widths2);
    //    table.TotalWidth = 560f;
    //    table.LockedWidth = true;




    //    if (Dt.Rows.Count > 0)
    //    {
    //        var date = DateTime.Now.ToString("yyyy-MM-dd");

    //        string compname = Dt.Rows[0]["Customer_Name"].ToString();
    //        string Quo_No = Dt.Rows[0]["Quotation_no"].ToString();
    //        string Address = Dt.Rows[0]["Address"].ToString();
    //        //string Quo_Dt = Dt.Rows[0]["Quotation_Date"].ToString();
    //        string state = Dt.Rows[0]["State_Code"].ToString();
    //        string GST_NO = Dt.Rows[0]["GST_No"].ToString();
    //        string kindAtt = Dt.Rows[0]["kind_Att"].ToString();
    //        string amt_in_Word = Dt.Rows[0]["Total_in_word"].ToString();
    //        string cgstper = Dt.Rows[0]["CGST"].ToString();
    //        string sgstper = Dt.Rows[0]["SGST"].ToString();
    //        DateTime ffff1 = Convert.ToDateTime(Dt.Rows[0]["Quotation_Date"].ToString());
    //        string datee = ffff1.ToString("yyyy-MM-dd");

    //        table.AddCell(new Phrase("Company Name : ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
    //        table.AddCell(new Phrase(compname, FontFactory.GetFont("Arial", 9, Font.BOLD)));
    //        table.AddCell(new Phrase("Quot. Number :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
    //        table.AddCell(new Phrase(Quo_No, FontFactory.GetFont("Arial", 9, Font.BOLD)));

    //        table.AddCell(new Phrase("Address :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
    //        table.AddCell(new Phrase(Address, FontFactory.GetFont("Arial", 9, Font.BOLD)));
    //        table.AddCell(new Phrase("Quot. Date :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
    //        table.AddCell(new Phrase(datee, FontFactory.GetFont("Arial", 9, Font.BOLD)));


    //        table.AddCell(new Phrase("State :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
    //        table.AddCell(new Phrase(state, FontFactory.GetFont("Arial", 9, Font.BOLD)));
    //        table.AddCell(new Phrase("GST No :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
    //        table.AddCell(new Phrase(GST_NO, FontFactory.GetFont("Arial", 9, Font.BOLD)));

    //        table.AddCell(new Phrase("Created Date :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
    //        table.AddCell(new Phrase(date, FontFactory.GetFont("Arial", 9, Font.BOLD)));
    //        table.AddCell(new Phrase("Kind Attn. :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
    //        table.AddCell(new Phrase(kindAtt, FontFactory.GetFont("Arial", 9, Font.BOLD)));


    //        paragraphTable1.Add(table);
    //        doc.Add(paragraphTable1);



    //        Paragraph paragraphTable2 = new Paragraph();
    //        paragraphTable2.SpacingAfter = 0f;
    //        table = new PdfPTable(9);
    //        float[] widths3 = new float[] { 4f, 19f, 10f, 8f, 8f, 8f, 11f, 8f, 12f };
    //        table.SetWidths(widths3);

    //        double Ttotal_price = 0;
    //        //DataTable Dt = Read_Table("SELECT * FROM vw_Quotation_Invoice");

    //        if (Dt.Rows.Count > 0)
    //        {
    //            table.TotalWidth = 560f;
    //            table.LockedWidth = true;
    //            table.AddCell(new Phrase("SN.", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //            table.AddCell(new Phrase("Description", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //            table.AddCell(new Phrase("Hsn / Sac", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //            table.AddCell(new Phrase("Tax %", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //            table.AddCell(new Phrase("Quantity", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //            table.AddCell(new Phrase("Unit", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //            table.AddCell(new Phrase("Rate", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //            table.AddCell(new Phrase("Disc %", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //            table.AddCell(new Phrase("Total", FontFactory.GetFont("Arial", 10, Font.BOLD)));

    //            int rowid = 1;
    //            foreach (DataRow dr in Dt.Rows)
    //            {
    //                table.TotalWidth = 560f;
    //                table.LockedWidth = true;

    //                double Ftotal = Convert.ToDouble(dr["FTotal"].ToString());
    //                string _ftotal = Ftotal.ToString("##.00");
    //                table.AddCell(new Phrase(rowid.ToString(), FontFactory.GetFont("Arial", 9)));
    //                table.AddCell(new Phrase(dr["CompName"].ToString(), FontFactory.GetFont("Arial", 9)));
    //                table.AddCell(new Phrase(dr["HSN"].ToString(), FontFactory.GetFont("Arial", 9)));
    //                table.AddCell(new Phrase(dr["Tax"].ToString(), FontFactory.GetFont("Arial", 9)));
    //                table.AddCell(new Phrase(dr["Qty"].ToString(), FontFactory.GetFont("Arial", 9)));
    //                table.AddCell(new Phrase(dr["Units"].ToString(), FontFactory.GetFont("Arial", 9)));
    //                table.AddCell(new Phrase(dr["Rate"].ToString(), FontFactory.GetFont("Arial", 9)));
    //                table.AddCell(new Phrase(dr["Disc_per"].ToString(), FontFactory.GetFont("Arial", 9)));
    //                table.AddCell(new Phrase(_ftotal, FontFactory.GetFont("Arial", 9)));
    //                rowid++;

    //                Ttotal_price += Convert.ToDouble(dr["FTotal"].ToString());
    //            }
    //        }
    //        paragraphTable2.Add(table);
    //        doc.Add(paragraphTable2);

    //        //Space
    //        Paragraph paragraphTable3 = new Paragraph();

    //        string[] items = { "Goods once sold will not be taken back or exchange. \b",
    //                    "Interest at the rate of 18% will be charged if bill is'nt paid within 30 days.\b",
    //                    "Our risk and responsibility ceases the moment goods leaves out godown. \n",
    //                    };

    //        Font font12 = FontFactory.GetFont("Arial", 12, Font.BOLD);
    //        Font font10 = FontFactory.GetFont("Arial", 10, Font.BOLD);
    //        Paragraph paragraph = new Paragraph("", font12);

    //        for (int i = 0; i < items.Length; i++)
    //        {
    //            paragraph.Add(new Phrase("", font10));
    //        }

    //        table = new PdfPTable(9);
    //        table.TotalWidth = 560f;
    //        table.LockedWidth = true;
    //        table.SetWidths(new float[] { 4f, 19f, 10f, 8f, 8f, 8f, 11f, 8f, 12f });
    //        table.AddCell(paragraph);
    //        table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //        table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //        table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //        table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //        table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //        table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //        table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //        //table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //        //table.AddCell(new Phrase("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n ", FontFactory.GetFont("Arial", 10, Font.BOLD)));

    //        //doc.Add(table);
    //        if (Dt.Rows.Count >= 10)
    //        {
    //            table.AddCell(new Phrase("  \n\n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));

    //        }
    //        else if (Dt.Rows.Count >= 7 && Dt.Rows.Count <= 9)
    //        {
    //            table.AddCell(new Phrase("  \n\n\n\n\n\n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));

    //        }
    //        else if (Dt.Rows.Count >= 4 && Dt.Rows.Count <= 6)
    //        {
    //            table.AddCell(new Phrase("  \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));

    //        }
    //        else if (Dt.Rows.Count < 4)
    //        {

    //            table.AddCell(new Phrase("  \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));

    //        }
    //        doc.Add(table);
    //        //Add Total Row start
    //        Paragraph paragraphTable5 = new Paragraph();

    //        //paragraphTable5.SpacingAfter = 10f;   

    //        string[] itemsss = { "Goods once sold will not be taken back or exchange. \b",
    //                    "Interest at the rate of 18% will be charged if bill is'nt paid within 30 days.\b",
    //                    "Our risk and responsibility ceases the moment goods leaves out godown. \n",
    //                    };

    //        Font font13 = FontFactory.GetFont("Arial", 12, Font.BOLD);
    //        Font font11 = FontFactory.GetFont("Arial", 10, Font.BOLD);
    //        Paragraph paragraphh = new Paragraph("", font12);

    //        //paragraphh.SpacingAfter = 10f;

    //        for (int i = 0; i < items.Length; i++)
    //        {
    //            paragraph.Add(new Phrase("", font10));
    //        }

    //        table = new PdfPTable(3);
    //        table.TotalWidth = 560f;
    //        table.LockedWidth = true;

    //        paragraph.Alignment = Element.ALIGN_RIGHT;

    //        table.SetWidths(new float[] { 0f, 76f, 12f });
    //        //table.AddCell(paragraph);
    //        //PdfPCell cell = new PdfPCell(new Phrase("Sub Total", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //        //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
    //        //table.AddCell(cell);
    //        //PdfPCell cell11 = new PdfPCell(new Phrase(Ttotal_price.ToString("##.00"), FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //        //cell11.HorizontalAlignment = Element.ALIGN_RIGHT;
    //        //table.AddCell(cell11);


    //        doc.Add(table);
    //        //add total row end

    //        //CGST 9% Row STart
    //        Paragraph paragraphTable15 = new Paragraph();
    //        paragraphTable5.SpacingAfter = 0f;
    //        //paragraphTable15
    //        //paragraphTable5.SpacingAfter = 10f;

    //        string[] itemss = { "Goods once sold will not be taken back or exchange. \b",
    //                    "Interest at the rate of 18% will be charged if bill is'nt paid within 30 days.\b",
    //                    "Our risk and responsibility ceases the moment goods leaves out godown. \n",
    //                    };

    //        Font font1 = FontFactory.GetFont("Arial", 12, Font.BOLD);
    //        Font font2 = FontFactory.GetFont("Arial", 10, Font.BOLD);
    //        Paragraph paragraphhh = new Paragraph("", font12);

    //        //paragraphh.SpacingAfter = 10f;

    //        for (int i = 0; i < items.Length; i++)
    //        {
    //            paragraph.Add(new Phrase("", font10));
    //        }

    //        table = new PdfPTable(3);
    //        table.TotalWidth = 560f;
    //        table.LockedWidth = true;

    //        var Cgst_9 = Convert.ToDecimal(Ttotal_price) * 9 / 100;

    //        table.SetWidths(new float[] { 0f, 76f, 12f });
    //        table.AddCell(paragraph);
    //        PdfPCell cell2 = new PdfPCell(new Phrase("C-Gst %", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //        cell2.HorizontalAlignment = Element.ALIGN_RIGHT;
    //        table.AddCell(cell2);
    //        PdfPCell cell3 = new PdfPCell(new Phrase(cgstper, FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //        cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
    //        table.AddCell(cell3);

    //        doc.Add(table);
    //        //CGST 9% Row End

    //        //SGST 9% Row STart
    //        Paragraph paragraphTable16 = new Paragraph();
    //        paragraphTable5.SpacingAfter = 10f;

    //        //paragraphTable5.SpacingAfter = 10f;

    //        string[] item = { "Goods once sold will not be taken back or exchange. \b",
    //                    "Interest at the rate of 18% will be charged if bill is'nt paid within 30 days.\b",
    //                    "Our risk and responsibility ceases the moment goods leaves out godown. \n",
    //                    };

    //        Font font14 = FontFactory.GetFont("Arial", 12, Font.BOLD);
    //        Font font15 = FontFactory.GetFont("Arial", 10, Font.BOLD);
    //        Paragraph paragraphhhh = new Paragraph("", font12);

    //        //paragraphh.SpacingAfter = 10f;

    //        for (int i = 0; i < items.Length; i++)
    //        {
    //            paragraph.Add(new Phrase("", font10));
    //        }

    //        table = new PdfPTable(3);
    //        table.TotalWidth = 560f;
    //        table.LockedWidth = true;

    //        var Sgst_9 = Convert.ToDecimal(Ttotal_price) * 9 / 100;


    //        table.SetWidths(new float[] { 0f, 76f, 12f });
    //        table.AddCell(paragraph);
    //        PdfPCell cell22 = new PdfPCell(new Phrase("S-Gst %", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //        cell22.HorizontalAlignment = Element.ALIGN_RIGHT;
    //        table.AddCell(cell22);
    //        PdfPCell cell33 = new PdfPCell(new Phrase(sgstper, FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //        cell33.HorizontalAlignment = Element.ALIGN_RIGHT;
    //        table.AddCell(cell33);


    //        doc.Add(table);
    //        //SGST 9% Row End

    //        //Grand total Row STart
    //        Paragraph paragraphTable17 = new Paragraph();
    //        paragraphTable5.SpacingAfter = 10f;

    //        //paragraphTable5.SpacingAfter = 10f;

    //        string[] itemm = { "Goods once sold will not be taken back or exchange. \b",
    //                    "Interest at the rate of 18% will be charged if bill is'nt paid within 30 days.\b",
    //                    "Our risk and responsibility ceases the moment goods leaves out godown. \n",
    //                    };

    //        Font font16 = FontFactory.GetFont("Arial", 12, Font.BOLD);
    //        Font font17 = FontFactory.GetFont("Arial", 10, Font.BOLD);
    //        Paragraph paragraphhhhh = new Paragraph("", font12);

    //        //paragraphh.SpacingAfter = 10f;

    //        for (int i = 0; i < items.Length; i++)
    //        {
    //            paragraph.Add(new Phrase("", font10));
    //        }

    //        table = new PdfPTable(3);
    //        table.TotalWidth = 560f;
    //        table.LockedWidth = true;

    //        var Grndttl = Convert.ToDecimal(Ttotal_price) + Convert.ToDecimal(Cgst_9) + Convert.ToDecimal(Sgst_9);

    //        table.SetWidths(new float[] { 0f, 76f, 12f });
    //        table.AddCell(paragraph);
    //        PdfPCell cell44 = new PdfPCell(new Phrase("Grand Total", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //        cell44.HorizontalAlignment = Element.ALIGN_RIGHT;
    //        table.AddCell(cell44);
    //        PdfPCell cell55 = new PdfPCell(new Phrase(Ttotal_price.ToString("##.00"), FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //        cell55.HorizontalAlignment = Element.ALIGN_RIGHT;
    //        table.AddCell(cell55);


    //        doc.Add(table);
    //        //Grand total Row End

    //        //Grand total in word Row STart
    //        Paragraph paragraphTable18 = new Paragraph();
    //        paragraphTable18.SpacingAfter = 50f;

    //        //paragraphTable5.SpacingAfter = 10f;

    //        string[] itemmm = { "Goods once sold will not be taken back or exchange. \b",
    //                    "Interest at the rate of 18% will be charged if bill is'nt paid within 30 days.\b",
    //                    "Our risk and responsibility ceases the moment goods leaves out godown. \n",
    //                    };

    //        Font font18 = FontFactory.GetFont("Arial", 12, Font.BOLD);
    //        Font font19 = FontFactory.GetFont("Arial", 10, Font.BOLD);
    //        Paragraph paragraphhmhhh = new Paragraph("", font12);



    //        for (int i = 0; i < items.Length; i++)
    //        {
    //            paragraph.Add(new Phrase("", font10));
    //        }

    //        table = new PdfPTable(3);
    //        table.TotalWidth = 560f;
    //        table.LockedWidth = true;



    //        table.SetWidths(new float[] { 0f, 25f, 63f });
    //        table.AddCell(paragraph);
    //        PdfPCell cell66 = new PdfPCell(new Phrase("Amount In Words Rs. ", FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //        cell66.HorizontalAlignment = Element.ALIGN_CENTER;
    //        table.AddCell(cell66);
    //        PdfPCell cell77 = new PdfPCell(new Phrase(amt_in_Word, FontFactory.GetFont("Arial", 10, Font.BOLD)));
    //        cell77.HorizontalAlignment = Element.ALIGN_CENTER;
    //        table.AddCell(cell77);

    //        doc.Add(table);
    //        //Grand total in word Row End


    //        PdfContentByte cn = writer.DirectContent;
    //        cn.Rectangle(17f, 160f, 560f, 115f);
    //        cn.Stroke();
    //        // Header 
    //        cn.BeginText();
    //        cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 14);
    //        cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Bank Details :", 30, 258, 0);
    //        cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibri.ttf", "Identity-H", BaseFont.EMBEDDED), 12);
    //        cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Bank Name : BANK OF BARODA", 30, 240, 0);
    //        cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibri.ttf", "Identity-H", BaseFont.EMBEDDED), 12);
    //        cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Account Name : ENDEAVOUR AUTOMATION", 30, 225, 0);
    //        cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibri.ttf", "Identity-H", BaseFont.EMBEDDED), 12);
    //        cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Branch : KALEWADI", 30, 210, 0);
    //        cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibri.ttf", "Identity-H", BaseFont.EMBEDDED), 12);
    //        cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "A/c No : 46180200000214", 30, 195, 0);
    //        cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibri.ttf", "Identity-H", BaseFont.EMBEDDED), 12);
    //        cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "IFSC/Neft Code :BARB0KALEWA", 30, 180, 0);

    //        cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\calibrii.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
    //        cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "We Thank you for your enquiry", 440, 256, 0);
    //        cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 12);
    //        cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "For", 392, 230, 0);
    //        cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 13);
    //        cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "ENDEAVOUR AUTOMATION", 413, 230, 0);
    //        cn.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 13);
    //        cn.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Authorised Signatory", 443, 180, 0);
    //        cn.EndText();



    //        Paragraph paragraphTable4 = new Paragraph();

    //        paragraphTable4.SpacingBefore = 10f;

    //        table = new PdfPTable(2);
    //        table.TotalWidth = 560f;

    //        float[] widths = new float[] { 160f, 400f };
    //        table.SetWidths(widths);
    //        table.LockedWidth = true;

    //        doc.Close();


    //        Byte[] FileBuffer = File.ReadAllBytes(Server.MapPath("~/Files/") + "QuotationInvoice.pdf");

    //        //if (FileBuffer != null)
    //        //{
    //        //    Response.ContentType = "application/pdf";
    //        //    Response.AddHeader("content-length", FileBuffer.Length.ToString());
    //        //    Response.BinaryWrite(FileBuffer);
    //        //    Response.AddHeader("Content-Disposition", "attachment;filename=Quotation.pdf");
    //        //}


    //        doc.Close();
    //    }
    //    ifrRight6.Visible =true;
    //    ifrRight6.Attributes["src"] = @"../Files/" + "QuotationInvoice.pdf";
    //}

    protected void btncreate_Click(object sender, EventArgs e)
    {

        Response.Redirect("Quotation_Sales.aspx");
    }

    protected void gv_Quot_List_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_Quot_List.PageIndex = e.NewPageIndex;
        ViewData();
    }

    SqlDataAdapter sad111;

    protected void sortedgv_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (ViewState["Record"].ToString() == "Vender")
        {
            gv_Quot_List.PageIndex = e.NewPageIndex;
            GetsortedDatewisegrid();

        }

        if (ViewState["Record"].ToString() == "Quation")
        {
            gv_Quot_List.PageIndex = e.NewPageIndex;
            GetsortedQuationgrid();

        }

        if (ViewState["Record"].ToString() == "Customer")
        {
            gv_Quot_List.PageIndex = e.NewPageIndex;
            GetsortedCsutomergrid();

        }

        if (ViewState["Record"].ToString() == "DateWise")
        {
            gv_Quot_List.PageIndex = e.NewPageIndex;
            GetsortedDatewisesearchGrid();

        }

        if (ViewState["Record"].ToString() == "Date")
        {
            gv_Quot_List.PageIndex = e.NewPageIndex;
            GetsortedDatewisegrid();

        }

    }

    public void GetsortedDatewise()
    {
        gv_Quot_List.Visible = false;
        ViewState["Record"] = "Date";
        DataTable dtt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select ID,Quotation_no,Quotation_Date,ExpiryDate,JobNo,Customer_Name,SubCustomer,Address,Mobile_No,Phone_No,GST_No,State_Code,kind_Att,CGST,SGST,AllTotal_price,Total_in_word,IsDeleted,CreatedBy,CreatedOn, DATEDIFF(DAY, Quotation_Date, getdate()) AS days from tbl_Quotation_two_Hdr where IsDeleted='0' and Quotation_Date between '" + txtDateSearchfrom.Text + "' AND  '" + txtDateSearchto.Text + "' ", con);
        sad.Fill(dtt);
        sortedgv.EmptyDataText = "Records Not Found";
        sortedgv.DataSource = dtt;
        sortedgv.DataBind();
    }

    public void GetsortedDatewisegrid()
    {
        gv_Quot_List.Visible = false;
        DataTable dtt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select ID,Quotation_no,Quotation_Date,ExpiryDate,JobNo,Customer_Name,SubCustomer,Address,Mobile_No,Phone_No,GST_No,State_Code,kind_Att,CGST,SGST,AllTotal_price,Total_in_word,IsDeleted,CreatedBy,CreatedOn, DATEDIFF(DAY, Quotation_Date, getdate()) AS days from tbl_Quotation_two_Hdr where IsDeleted='0' and Quotation_Date between '" + txtDateSearchfrom.Text + "' AND  '" + txtDateSearchto.Text + "' ", con);
        sad.Fill(dtt);
        sortedgv.EmptyDataText = "Records Not Found";
        sortedgv.DataSource = dtt;
        sortedgv.DataBind();
    }

    public void GetsortedQuation()
    {
        gv_Quot_List.Visible = false;
        ViewState["Record"] = "Quation";
        DataTable dtt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select ID,Quotation_no,Quotation_Date,ExpiryDate,JobNo,Customer_Name,SubCustomer,Address,Mobile_No,Phone_No,GST_No,State_Code,kind_Att,CGST,SGST,AllTotal_price,Total_in_word,IsDeleted,CreatedBy,CreatedOn, DATEDIFF(DAY, Quotation_Date, getdate()) AS days from tbl_Quotation_two_Hdr where Quotation_no='" + txtquotation.Text + "'", con);
        sad.Fill(dtt);
        sortedgv.EmptyDataText = "Records Not Found";
        sortedgv.DataSource = dtt;
        sortedgv.DataBind();
    }

    public void GetsortedQuationgrid()
    {
        gv_Quot_List.Visible = false;
        DataTable dtt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select ID,Quotation_no,Quotation_Date,ExpiryDate,JobNo,Customer_Name,SubCustomer,Address,Mobile_No,Phone_No,GST_No,State_Code,kind_Att,CGST,SGST,AllTotal_price,Total_in_word,IsDeleted,CreatedBy,CreatedOn, DATEDIFF(DAY, Quotation_Date, getdate()) AS days from tbl_Quotation_two_Hdr where Quotation_no='" + txtquotation.Text + "'", con);
        sad.Fill(dtt);
        sortedgv.EmptyDataText = "Records Not Found";
        sortedgv.DataSource = dtt;
        sortedgv.DataBind();
    }

    public void GetsortedCsutomer()
    {
        gv_Quot_List.Visible = false;
        ViewState["Record"] = "Customer";
        DataTable dtt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select ID,Quotation_no,Quotation_Date,ExpiryDate,JobNo,Customer_Name,SubCustomer,Address,Mobile_No,Phone_No,GST_No,State_Code,kind_Att,CGST,SGST,AllTotal_price,Total_in_word,IsDeleted,CreatedBy,CreatedOn, DATEDIFF(DAY, Quotation_Date, getdate()) AS days from tbl_Quotation_two_Hdr where  Customer_Name='" + txtSearch.Text + "'", con);
        sad.Fill(dtt);
        sortedgv.EmptyDataText = "Records Not Found";
        sortedgv.DataSource = dtt;
        sortedgv.DataBind();
    }


    public void GetsortedCsutomergrid()
    {
        gv_Quot_List.Visible = false;
        DataTable dtt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select ID,Quotation_no,Quotation_Date,ExpiryDate,JobNo,Customer_Name,SubCustomer,Address,Mobile_No,Phone_No,GST_No,State_Code,kind_Att,CGST,SGST,AllTotal_price,Total_in_word,IsDeleted,CreatedBy,CreatedOn, DATEDIFF(DAY, Quotation_Date, getdate()) AS days from tbl_Quotation_two_Hdr where  Customer_Name='" + txtSearch.Text + "'", con);
        sad.Fill(dtt);
        sortedgv.EmptyDataText = "Records Not Found";
        sortedgv.DataSource = dtt;
        sortedgv.DataBind();
    }

    public void GetsortedDatewisesearch()
    {
        gv_Quot_List.Visible = false;
        ViewState["Record"] = "DateWise";
        DataTable dtt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select ID,Quotation_no,Quotation_Date,ExpiryDate,JobNo,Customer_Name,SubCustomer,Address,Mobile_No,Phone_No,GST_No,State_Code,kind_Att,CGST,SGST,AllTotal_price,Total_in_word,IsDeleted,CreatedBy,CreatedOn, DATEDIFF(DAY, Quotation_Date, getdate()) AS days from tbl_Quotation_two_Hdr where Quotation_Date='" + txtDateSearch.Text + "' AND isdeleted = '0'", con);
        sad.Fill(dtt);
        sortedgv.EmptyDataText = "Records Not Found";
        sortedgv.DataSource = dtt;
        sortedgv.DataBind();
    }

    public void GetsortedDatewisesearchGrid()
    {
        DataTable dtt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select ID,Quotation_no,Quotation_Date,ExpiryDate,JobNo,Customer_Name,SubCustomer,Address,Mobile_No,Phone_No,GST_No,State_Code,kind_Att,CGST,SGST,AllTotal_price,Total_in_word,IsDeleted,CreatedBy,CreatedOn, DATEDIFF(DAY, Quotation_Date, getdate()) AS days from tbl_Quotation_two_Hdr where Quotation_Date='" + txtDateSearch.Text + "' AND isdeleted = '0'", con);
        sad.Fill(dtt);
        sortedgv.EmptyDataText = "Records Not Found";
        sortedgv.DataSource = dtt;
        sortedgv.DataBind();
    }

    public void GetDatewiseCsutomer()
    {
        gv_Quot_List.Visible = false;
        ViewState["Record"] = "DatwiseCustomer";
        DataTable dtt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT ID, Quotation_no, Quotation_Date, ExpiryDate, JobNo, Customer_Name, SubCustomer, Address, Mobile_No, Phone_No, GST_No, State_Code, kind_Att, CGST, SGST, AllTotal_price, Total_in_word, IsDeleted, CreatedBy, CreatedOn, DATEDIFF(DAY, Quotation_Date, GETDATE()) AS days FROM tbl_Quotation_two_Hdr WHERE Quotation_Date BETWEEN '" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "' AND Customer_Name = '" + txtSearch.Text + "'", con);

        sad.Fill(dtt);
        sortedgv.EmptyDataText = "Records Not Found";
        sortedgv.DataSource = dtt;
        sortedgv.DataBind();
    }

    public void GetDatewiseCsutomergrid()
    {
        gv_Quot_List.Visible = false;
        DataTable dtt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT ID, Quotation_no, Quotation_Date, ExpiryDate, JobNo, Customer_Name, SubCustomer, Address, Mobile_No, Phone_No, GST_No, State_Code, kind_Att, CGST, SGST, AllTotal_price, Total_in_word, IsDeleted, CreatedBy, CreatedOn, DATEDIFF(DAY, Quotation_Date, GETDATE()) AS days FROM tbl_Quotation_two_Hdr WHERE Quotation_Date BETWEEN '" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "' AND Customer_Name = '" + txtSearch.Text + "'", con);

        sad.Fill(dtt);
        sortedgv.EmptyDataText = "Records Not Found";
        sortedgv.DataSource = dtt;
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
            string Method = ViewState["Excell"].ToString();
            if (Method == "GetsortedDatewise")
            {
                GetsortedDatewiseForExcell();
            }

            if (Method == "GetsortedQuation")
            {
                GetsortedQuationForExcell();
            }
            if (Method == "GetsortedCsutomer")
            {
                GetsortedCsutomerForExcell();
            }
            if (Method == "DateSearch")
            {
                GetDateForexcell();
            }
        }
        else
        {
            ExportExcelGrid();
        }

        Response.Clear();
        Response.Buffer = true;
        Response.ClearContent();
        Response.ClearHeaders();
        Response.Charset = "";
        string FileName = "Quotation_List_" + DateTime.Now + ".xls";
        StringWriter strwritter = new StringWriter();
        HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
        ExportGrid.GridLines = GridLines.Both;
        ExportGrid.HeaderStyle.Font.Bold = true;
        ExportGrid.RenderControl(htmltextwrtter);
        Response.Write(strwritter.ToString());
        Response.End();
    }

    public void GetsortedDatewiseForExcell()
    {
        gv_Quot_List.Visible = false;
        ViewState["Record"] = "Date";
        DataTable dtt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select ID,Quotation_no,Quotation_Date,ExpiryDate,JobNo,Customer_Name,SubCustomer,Address,Mobile_No,Phone_No,GST_No,State_Code,kind_Att,CGST,SGST,AllTotal_price,Total_in_word,IsDeleted,CreatedBy,CreatedOn, DATEDIFF(DAY, Quotation_Date, getdate()) AS days from tbl_Quotation_two_Hdr where IsDeleted='0' and Quotation_Date between '" + txtDateSearchfrom.Text + "' AND  '" + txtDateSearchto.Text + "' ", con);
        sad.Fill(dtt);
        ExportGrid.EmptyDataText = "Records Not Found";
        ExportGrid.DataSource = dtt;
        ExportGrid.DataBind();
    }
    public void GetsortedQuationForExcell()
    {
        gv_Quot_List.Visible = false;
        ViewState["Record"] = "Quation";
        DataTable dtt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select ID,Quotation_no,Quotation_Date,ExpiryDate,JobNo,Customer_Name,SubCustomer,Address,Mobile_No,Phone_No,GST_No,State_Code,kind_Att,CGST,SGST,AllTotal_price,Total_in_word,IsDeleted,CreatedBy,CreatedOn, DATEDIFF(DAY, Quotation_Date, getdate()) AS days from tbl_Quotation_two_Hdr where Quotation_no='" + txtquotation.Text + "'", con);
        sad.Fill(dtt);
        ExportGrid.EmptyDataText = "Records Not Found";
        ExportGrid.DataSource = dtt;
        ExportGrid.DataBind();
    }
    public void GetsortedCsutomerForExcell()
    {
        gv_Quot_List.Visible = false;
        ViewState["Record"] = "Customer";
        DataTable dtt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select ID,Quotation_no,Quotation_Date,ExpiryDate,JobNo,Customer_Name,SubCustomer,Address,Mobile_No,Phone_No,GST_No,State_Code,kind_Att,CGST,SGST,AllTotal_price,Total_in_word,IsDeleted,CreatedBy,CreatedOn, DATEDIFF(DAY, Quotation_Date, getdate()) AS days from tbl_Quotation_two_Hdr where  Customer_Name='" + txtSearch.Text + "'", con);
        sad.Fill(dtt);
        ExportGrid.EmptyDataText = "Records Not Found";
        ExportGrid.DataSource = dtt;
        ExportGrid.DataBind();
    }
    public void GetDateForexcell()
    {
        DataTable dtt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select ID,Quotation_no,Quotation_Date,ExpiryDate,JobNo,Customer_Name,SubCustomer,Address,Mobile_No,Phone_No,GST_No,State_Code,kind_Att,CGST,SGST,AllTotal_price,Total_in_word,IsDeleted,CreatedBy,CreatedOn, DATEDIFF(DAY, Quotation_Date, getdate()) AS days from tbl_Quotation_two_Hdr where Quotation_Date='" + txtDateSearch.Text + "' AND isdeleted = '0'", con);
        sad.Fill(dtt);
        ExportGrid.EmptyDataText = "Records Not Found";
        ExportGrid.DataSource = dtt;
        ExportGrid.DataBind();

    }

    protected void ddlservicetype_TextChanged(object sender, EventArgs e)
    {
        DataTable dtt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select ID,Quotation_no,Quotation_Date,ExpiryDate,JobNo,Customer_Name,SubCustomer,Address,Mobile_No,Phone_No,GST_No,State_Code,kind_Att,CGST,SGST,AllTotal_price,Total_in_word,IsDeleted,CreatedBy,CreatedOn, DATEDIFF(DAY, Quotation_Date, getdate()) AS days from tbl_Quotation_two_Hdr where ServiceType ='" + ddlservicetype.Text + "' AND isdeleted = '0'", con);
        //  SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM [tbl_Quotation_two_Hdr] WHERE ServiceType ='" + ddlservicetype.Text + "' AND isdeleted = '0'", con);
        sad.Fill(dtt);
        gv_Quot_List.EmptyDataText = "Records Not Found";
        gv_Quot_List.DataSource = dtt;
        gv_Quot_List.DataBind();

    }
}