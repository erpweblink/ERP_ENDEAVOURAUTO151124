using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.Security.Cryptography;
using System.IO;

public partial class Reception_Evalution : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
    DataTable dt11 = new DataTable();
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
                //GridView();
                //gv_Evalution.DataSource = dt11;
                //gv_Evalution.DataBind();
                //this.gv_Evalution.Columns[9].Visible = true;

                string UserCompany = Session["adminname"].ToString();
                if (UserCompany != "Admin")
                {
                   
                    gv_Evalution.DataSource = dt11;
                    gv_Evalution.DataBind();
                    this.gv_Evalution.Columns[9].Visible = true;
                    gv_Evalution.Columns[2].Visible = false;
                    gv_Evalution.Columns[3].Visible = false;
                    GridView();
                }
                else
                {
                    GridView();
                    //gv_Evalution.DataSource = dt11;
                    //gv_Evalution.DataBind();
                    //this.gv_Evalution.Columns[9].Visible = true;
                }
            }
            // GridView();
            //gv_Evalution.DataSource = dtdaycount;
            //gv_Evalution.DataBind();
        }
    }

    DataTable dtdaycount = new DataTable();

    void gvbind_Company()
    {
        try
        {
            string UserCompany = Session["UserCompany"].ToString();
            DataTable dt = new DataTable();
            SqlDataAdapter sad = new SqlDataAdapter("select [id],[JobNo],[CustomerName],[ModelNo],[SerialNo]," +
                                       "[EngiName],[TestingDate],[Status],[Remark],[EntryDate],[isCompleted],[isdeleted],[isCancel]," +
                                       "[CreatedBy],[CreatedDate],[UpdateBy],[UpdatedDate],ProductName,Status,DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where  isdeleted='0' AND CustomerName='" + UserCompany + "' ORDER BY [JobNo] Desc ", con);
            sad.Fill(dt);
            gv_Evalution.EmptyDataText = "Not Records Found";
            gv_Evalution.DataSource = dt;
            gv_Evalution.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    void GridView()
    {
        try
        {
            DataTable dt = new DataTable();
            SqlDataAdapter sad = new SqlDataAdapter("select [id],[JobNo],[CustomerName],[Subcustomer],[ModelNo],[SerialNo]," +
                            "[EngiName],[TestingDate],[Status],[Remark],[EntryDate],[isCompleted],[isdeleted],[isCancel]," +
                            "[CreatedBy],[CreatedDate],[UpdateBy],[UpdatedDate],ProductName,Status,DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where  isdeleted='0' ORDER BY [CreatedDate] Desc ", con);
            sad.Fill(dt);
            gv_Evalution.EmptyDataText = "Not Records Found";
            gv_Evalution.DataSource = dt;
            gv_Evalution.DataBind();
           // SortGvEvaluations.Visible = false;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    void ExportExcelGrid()
    {
        try
        {
            DataTable dt = new DataTable();
            SqlDataAdapter sad = new SqlDataAdapter("select [id],[JobNo],[CustomerName],[Subcustomer],[ModelNo],[SerialNo]," +
                            "[EngiName],[TestingDate],[Status],[Remark],[EntryDate],[isCompleted],[isdeleted],[isCancel]," +
                            "[CreatedBy],[CreatedDate],[UpdateBy],[UpdatedDate],ProductName,Status,DATEDIFF(DAY, EntryDate, getdate()) AS days from tblTestingProduct where  isdeleted='0' ORDER BY [CreatedDate] Desc ", con);
            sad.Fill(dt);
            ExportToExcelGrid.EmptyDataText = "Not Records Found";
            ExportToExcelGrid.DataSource = dt;
            ExportToExcelGrid.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    void TestingFilter()
    {
        try
        {
            DataTable dt = new DataTable();
            sad = new SqlDataAdapter("select id, JobNo, CustomerName,Subcustomer, EntryDate, ProductName,EngiName, ModelNo, SerialNo, Status, Remark, isCompleted, CreatedBy, DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where isCompleted = '1'", con);
            //sad = new SqlDataAdapter("select id,JobNo,CustomerName,ProductName,ModelNo,SerialNo,EngiName,TestingDate,Status,Remark,EntryDate,isCompleted,CreatedDate,CreatedBy,Quotation_no,inwardEntrystatus,ReportedTO, DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where JobNo= '" + jobno + "'", con);
            sad.Fill(dt);
            Grid_Filter.EmptyDataText = "Not Records Found";
            Grid_Filter.DataSource = dt;
            Grid_Filter.DataBind();


            //DataTable dt = new DataTable();
            //SqlDataAdapter sad = new SqlDataAdapter("select [id],[JobNo],[CustomerName],[ModelNo],[SerialNo]," +
            //                "[EngiName],[TestingDate],[Status],[Remark],[EntryDate],[isCompleted],[isdeleted],[isCancel]," +
            //                "[CreatedBy],[CreatedDate],[UpdateBy],[UpdatedDate],ProductName,Status,DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where  isdeleted='0' ORDER BY [CreatedDate] Desc ", con);
            //sad.Fill(dt);
            //Grid_Filter.EmptyDataText = "Not Records Found";
            //Grid_Filter.DataSource = dt;
            //Grid_Filter.DataBind();

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetJOBNOList(string prefixText, int count)
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
                com.CommandText = "select DISTINCT JobNo from tblTestingProduct where " + "JobNo like @Search + '%' ";

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

    protected void gv_Evalution_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName == "selectdata")
        {

            Response.Redirect("GoTesting.aspx?JobNo=" + encrypt(e.CommandArgument.ToString()));
        }
        if (e.CommandName == "RowEdit")
        {
            Response.Redirect("GoTesting.aspx?id=" + encrypt(e.CommandArgument.ToString()) + "");
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

    protected void gv_Evalution_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_Evalution.PageIndex = e.NewPageIndex;
        GridView();
    }

    protected void btn_search_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtJobNo.Text) && string.IsNullOrEmpty(txtcustomername.Text) && string.IsNullOrEmpty(txtDateSearchfrom.Text) && string.IsNullOrEmpty(txtDateSearchto.Text) && string.IsNullOrEmpty(txtproduct.Text) && string.IsNullOrEmpty(txtstatus.Text)&& string.IsNullOrEmpty(ddlEvalution.SelectedItem.Text))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Please Enter Job No. OR Customer Name !!!');", true);
            GridView();
        }
        else
        {
            if (!string.IsNullOrEmpty(txtJobNo.Text))
            {

                ViewState["Excell"] = "getsortedjobno";
                getsortedjobno();
                //string jobno = txtJobNo.Text;

                //DataTable dt = new DataTable();
                //sad = new SqlDataAdapter("select id,JobNo,CustomerName,ProductName,Subcustomer,ModelNo,SerialNo,EngiName,TestingDate,Status,Remark,EntryDate,isCompleted,CreatedDate,CreatedBy,Quotation_no,inwardEntrystatus,ReportedTO, DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where JobNo= '" + jobno + "'", con);
                //sad.Fill(dt);
                //gv_Evalution.EmptyDataText = "Not Records Found";
                //gv_Evalution.DataSource = dt;
                //gv_Evalution.DataBind();
            }

            if (!string.IsNullOrEmpty(txtstatus.Text) && string.IsNullOrEmpty(txtDateSearchfrom.Text))
            {
                ViewState["Excell"] = "Getsortedstatus";
                Getsortedstatus();
                //DataTable dt = new DataTable();
                //SqlDataAdapter sad = new SqlDataAdapter(" select [id],[JobNo],[CustomerName],Subcustomer,[ProductName],[ModelNo],[SerialNo],[EngiName],[TestingDate],[Status],[Remark],[EntryDate],[isCompleted],[isdeleted],[isCancel],[CreatedBy],[CreatedDate] ,UpdateBy,UpdatedDate,Quotation_no,inwardEntrystatus,ReportedTO,DATEDIFF(DAY, CreatedDate, getdate()) AS days FROM tblTestingProduct Where Status = '" + txtstatus.Text + "' ", con);

                //sad.Fill(dt);
                //gv_Evalution.EmptyDataText = "Not Records Found";
                //gv_Evalution.DataSource = dt;
                //gv_Evalution.DataBind();
            }

            if (!string.IsNullOrEmpty(txtproduct.Text) && string.IsNullOrEmpty(txtDateSearchfrom.Text))
            {
                ViewState["Excell"] = "Getsortedproduct";
                Getsortedproduct();
                //DataTable dt = new DataTable();
                //SqlDataAdapter sad = new SqlDataAdapter("select  id,JobNo,CustomerName,Subcustomer,EntryDate,ProductName,EngiName,ModelNo,SerialNo,Status,Remark,isCompleted,CreatedBy,DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where  ProductName LIKE '%" + txtproduct.Text + "%'", con);

                //// SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],[FinalStatus],[TestBy],[ModelNo],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, CreatedDate, getdate()) AS days FROM [tblInwardEntry] Where MateName='" + txtproduct.Text + "' AND isdeleted='0'", con);
                //sad.Fill(dt);
                //gv_Evalution.EmptyDataText = "Not Records Found";
                //gv_Evalution.DataSource = dt;
                //gv_Evalution.DataBind();
            }

            //From Date To ToDate
            else if (!string.IsNullOrEmpty(txtDateSearchfrom.Text) && !string.IsNullOrEmpty(txtDateSearchto.Text) && string.IsNullOrEmpty(txtDateSearchfrom.Text) && string.IsNullOrEmpty(txtDateSearchfrom.Text))
            {
                ViewState["Excell"] = "GetsortedDatewise";
                GetsortedDatewise();
                //DataTable dt = new DataTable();
                //SqlDataAdapter sad = new SqlDataAdapter("select  id,JobNo,CustomerName,Subcustomer,EntryDate,ProductName,EngiName,ModelNo,SerialNo,Status,Remark,isCompleted,CreatedBy,DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where  EntryDate between '" + txtDateSearchfrom.Text + "' AND  '" + txtDateSearchto.Text + "' ", con);

                //sad.Fill(dt);
                //gv_Evalution.EmptyDataText = "Not Records Found";
                //gv_Evalution.DataSource = dt;
                //gv_Evalution.DataBind();
            }

            if (!string.IsNullOrEmpty(txtcustomername.Text) && string.IsNullOrEmpty(txtDateSearchfrom.Text))
            {
                ViewState["Excell"] = "Getsortedcustomer";
                Getsortedcustomer();
                //DataTable dt = new DataTable();

                //SqlDataAdapter sad = new SqlDataAdapter("select  id,JobNo,CustomerName,Subcustomer,EntryDate,ProductName,EngiName,ModelNo,SerialNo,Status,Remark,isCompleted,CreatedBy,DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where  CustomerName LIKE '%" + txtcustomername.Text + "%'", con);
                ////SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],[FinalStatus],[TestBy],[ModelNo],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, CreatedDate, getdate()) AS days FROM [tblInwardEntry] Where CustName='" + txtSearch.Text + "' AND isdeleted='0'", con);
                //sad.Fill(dt);
                //gv_Evalution.EmptyDataText = "Not Records Found";
                //gv_Evalution.DataSource = dt;
                //gv_Evalution.DataBind();
            }
            else if (!string.IsNullOrEmpty(txtDateSearchfrom.Text) && !string.IsNullOrEmpty(txtDateSearchto.Text) && !string.IsNullOrEmpty(txtcustomername.Text))
            {
                ViewState["Excell"] = "Getdatwisecustomer";
                Getdatwisecustomer();
            }

            else if (!string.IsNullOrEmpty(txtDateSearchfrom.Text) && !string.IsNullOrEmpty(txtDateSearchto.Text) && !string.IsNullOrEmpty(txtproduct.Text))
            {
                ViewState["Excell"] = "Getdatewiseproduct";
                Getdatewiseproduct();
            }
            else if (!string.IsNullOrEmpty(txtDateSearchfrom.Text) && !string.IsNullOrEmpty(txtDateSearchto.Text) && !string.IsNullOrEmpty(txtstatus.Text))
            {
                ViewState["Excell"] = "Getdatewisestatus";
                Getdatewisestatus();
            }

            else if (!string.IsNullOrEmpty(txtDateSearchfrom.Text) && !string.IsNullOrEmpty(txtDateSearchto.Text) && !string.IsNullOrEmpty(ddlEvalution.SelectedItem.Text))
            {
                ViewState["Excell"] = "DatewiseList";
                Getdatewisesortedlist();
            }
            else if (!string.IsNullOrEmpty(ddlEvalution.SelectedItem.Text))
            {
                if (ddlEvalution.SelectedItem.Text != "Select")
                {
                    ViewState["Excell"] = "ListWiseSearch";
                    GetListwisesearch();
                }
            }

        }
    }

    protected void lnkBtn_rfresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("Evalution.aspx");
    }
    SqlDataAdapter sadday;
    string status;
    string update = "";

    protected void gv_Evalution_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                LinkButton Lnk_Edit = (LinkButton)e.Row.FindControl("lnkbtnEdit");
                LinkButton Lnk_testing = (LinkButton)e.Row.FindControl("lnkbtnTotesting");
                Label jobno = (Label)e.Row.FindControl("lblJobno");
                Label iscompleted = (Label)e.Row.FindControl("lbliscompleted");

                if (string.IsNullOrWhiteSpace(iscompleted.Text))
                {
                    Lnk_testing.Enabled = true;
                    Lnk_Edit.Enabled = false;
                    // gv_Evalution.Columns[2].Visible = false;
                    // gv_Evalution.Columns[3].Visible = false;
                    Lnk_testing.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    Lnk_Edit.Enabled = true;
                    Lnk_testing.Enabled = false;
                    Lnk_Edit.ForeColor = System.Drawing.Color.Green;
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                con.Open();
                Label jobnono = (Label)e.Row.FindControl("lblJobno");
                string jobno = jobnono.Text;

                SqlCommand cmdquatationreader = new SqlCommand("select q.isCreateQuata, q.CreatedOn from tblTestingProduct t inner join tbl_Quotation_Hdr q on t.JobNo=q.JobNo where t.JobNo='" + jobno + "'", con);
                SqlDataReader reader = cmdquatationreader.ExecuteReader();

                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        status = reader["isCreateQuata"].ToString();

                        if (status == "True")
                        {
                            DateTime quattiondate = Convert.ToDateTime(reader["CreatedOn"].ToString());
                            update = quattiondate.ToString("yyyy-MM-dd");
                            con.Close();
                            sadday = new SqlDataAdapter("select  id,JobNo,CustomerName,Subcustomer,EntryDate,ProductName,EngiName,ModelNo,SerialNo,Status,Remark,isCompleted,CreatedBy,CreatedDate,DATEDIFF(DAY, CreatedDate, '" + update + "') AS days from tblTestingProduct where JobNo='" + jobno + "'", con);
                            sadday.Fill(dtdaycount);
                        }
                        else
                        {
                            con.Close();
                            sadday = new SqlDataAdapter("select  id,JobNo,CustomerName,Subcustomer,EntryDate,ProductName,EngiName,ModelNo,SerialNo,Status,Remark,isCompleted,CreatedBy,DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where JobNo='" + jobno + "'", con);
                            sadday.Fill(dtdaycount);
                            e.Row.Cells[9].ForeColor = System.Drawing.Color.Red;
                        }
                    }
                }
                else
                {
                    con.Close();
                    sadday = new SqlDataAdapter("select  id,JobNo,CustomerName,Subcustomer,EntryDate,ProductName,EngiName,ModelNo,SerialNo,Status,Remark,isCompleted,CreatedBy,DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where JobNo='" + jobno + "'", con);
                    sadday.Fill(dtdaycount);
                    e.Row.Cells[9].ForeColor = System.Drawing.Color.Red;
                }
            }
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
        return AutoFillCustomerlist(prefixText);
    }

    public static List<string> AutoFillCustomerlist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT CustomerName from tblTestingProduct where " + "CustomerName like @Search + '%' AND isdeleted = '0'";


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

    //----product 
    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetProductList(string prefixText, int count)
    {
        return AutoFillGetProductList(prefixText);
    }

    public static List<string> AutoFillGetProductList(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {

                //com.CommandText = "select DISTINCT ProductName from tblTestingProduct where " + "ProductName like @Search + '%' AND isCompleted IS NULL AND isdeleted='0'";
                com.CommandText = "select DISTINCT ProductName from tblTestingProduct where " + "ProductName like @Search + '%' AND isdeleted = '0'";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> ProductName = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        ProductName.Add(sdr["ProductName"].ToString());
                    }
                }
                con.Close();
                return ProductName;
            }

        }
    }

    //[System.Web.Script.Services.ScriptMethod()]
    //[System.Web.Services.WebMethod]
    //public static List<string> GetProductList(string prefixText, int count)
    //{
    //    return AutoFillproductlist(prefixText);
    //}

    //public static List<string> AutoFillproductlist(string prefixText)
    //{
    //    using (SqlConnection con = new SqlConnection())
    //    {
    //        con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

    //        using (SqlCommand com = new SqlCommand())
    //        {
    //            com.CommandText = "select DISTINCT ProductName from tblTestingProduct where " + "ProductName like @Search + '%' AND isCompleted IS NULL AND isdeleted='0'";

    //            com.Parameters.AddWithValue("@Search", prefixText);
    //            com.Connection = con;
    //            con.Open();
    //            List<string> ProductName = new List<string>();
    //            using (SqlDataReader sdr = com.ExecuteReader())
    //            {
    //                while (sdr.Read())
    //                {
    //                    ProductName.Add(sdr["ProductName"].ToString());
    //                }
    //            }
    //            con.Close();
    //            return ProductName;
    //        }
    //    }
    //}

    //----Status 
    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetStatusList(string prefixText, int count)
    {
        return AutoFillStatuslist(prefixText);
    }

    public static List<string> AutoFillStatuslist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {


                com.CommandText = "select DISTINCT Status from tblTestingProduct where " + "Status like @Search + '%' AND  isdeleted='0'";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> Status = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        Status.Add(sdr["Status"].ToString());
                    }
                }
                con.Close();
                return Status;
            }

        }
    }

    //-------end product-------

    SqlDataAdapter sad;
    //protected void ddlEvalution_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        GridView();
    //        dgvgridview.Visible = false;
    //        DataTable dt = new DataTable();

    //        if (ddlEvalution.SelectedItem.Text == "All")
    //        {
    //            foreach (GridViewRow g1 in gv_Evalution.Rows)
    //            {
    //                con.Open();
    //                Label jobnono = (Label)g1.FindControl("lblJobno");
    //                string jobno = jobnono.Text;
    //                Label lbldaycount = (Label)g1.FindControl("lbldaycount");
    //                SqlCommand cmdquatationreader = new SqlCommand("select q.isCreateQuata, q.CreatedOn from tblTestingProduct t inner join tbl_Quotation_Hdr q on t.JobNo=q.JobNo where t.JobNo='" + jobno + "'", con);
    //                SqlDataReader reader = cmdquatationreader.ExecuteReader();

    //                if (reader.HasRows)
    //                {
    //                    if (reader.Read())
    //                    {
    //                        status = reader["isCreateQuata"].ToString();

    //                        if (status == "True")
    //                        {
    //                            DateTime quattiondate = Convert.ToDateTime(reader["CreatedOn"].ToString());
    //                            update = quattiondate.ToString("yyyy-MM-dd");
    //                            con.Close();
    //                            sad = new SqlDataAdapter("select  id,JobNo,CustomerName,Subcustomer,EntryDate,ProductName,EngiName,ModelNo,SerialNo,Status,Remark,isCompleted,CreatedBy,CreatedDate,DATEDIFF(DAY, CreatedDate, '" + update + "') AS days from tblTestingProduct where JobNo='" + jobno + "'", con);
    //                            sad.Fill(dt);

    //                        }
    //                        else
    //                        {
    //                            con.Close();
    //                            sad = new SqlDataAdapter("select  id,JobNo,CustomerName,Subcustomer,EntryDate,ProductName,EngiName,ModelNo,SerialNo,Status,Remark,isCompleted,CreatedBy,DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where JobNo='" + jobno + "'", con);
    //                            sad.Fill(dt);
    //                            lbldaycount.ForeColor = System.Drawing.Color.Red;
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    con.Close();
    //                    sad = new SqlDataAdapter("select  id,JobNo,CustomerName,Subcustomer,EntryDate,ProductName,EngiName,ModelNo,SerialNo,Status,Remark,isCompleted,CreatedBy,DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where JobNo='" + jobno + "'", con);
    //                    sad.Fill(dt);
    //                    lbldaycount.ForeColor = System.Drawing.Color.Red;
    //                }
    //            }
    //        }
    //        else if (ddlEvalution.SelectedItem.Text == "Tested List")
    //        {

    //            sad = new SqlDataAdapter("select id, JobNo, CustomerName,Subcustomer, EntryDate, ProductName,EngiName, ModelNo, SerialNo, Status, Remark, isCompleted, CreatedBy, DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where isCompleted = '1'", con);
    //            //sad = new SqlDataAdapter("select id,JobNo,CustomerName,ProductName,ModelNo,SerialNo,EngiName,TestingDate,Status,Remark,EntryDate,isCompleted,CreatedDate,CreatedBy,Quotation_no,inwardEntrystatus,ReportedTO, DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where JobNo= '" + jobno + "'", con);
    //            sad.Fill(dt);
    //            SortGvEvaluations.EmptyDataText = "Not Records Found";
    //            SortGvEvaluations.DataSource = dt;
    //            SortGvEvaluations.DataBind();


    //            //foreach (GridViewRow g1 in gv_Evalution.Rows)
    //            //{
    //            //    con.Open();
    //            //    Label jobnono = (Label)g1.FindControl("lblJobno");
    //            //    string jobno = jobnono.Text;
    //            //    Label lbldaycount = (Label)g1.FindControl("lbldaycount");
    //            //    SqlCommand cmdquatationreader = new SqlCommand("select q.isCreateQuata, q.CreatedOn from tblTestingProduct t inner join tbl_Quotation_Hdr q on t.JobNo=q.JobNo where t.JobNo='" + jobno + "'", con);
    //            //    SqlDataReader reader = cmdquatationreader.ExecuteReader();

    //            //    if (reader.HasRows)
    //            //    {
    //            //        if (reader.Read())
    //            //        {
    //            //            status = reader["isCreateQuata"].ToString();

    //            //            if (status == "True")
    //            //            {
    //            //                DateTime quattiondate = Convert.ToDateTime(reader["CreatedOn"].ToString());
    //            //                update = quattiondate.ToString("yyyy-MM-dd");
    //            //                con.Close();
    //            //                sad = new SqlDataAdapter("select  id,JobNo,CustomerName,EntryDate,ProductName,ModelNo,SerialNo,Status,Remark,isCompleted,CreatedBy,CreatedDate,DATEDIFF(DAY, CreatedDate, '" + update + "') AS days from tblTestingProduct where JobNo='" + jobno + "' AND  isCompleted='1'", con);
    //            //                sad.Fill(dt);

    //            //            }
    //            //            else
    //            //            {
    //            //                con.Close();
    //            //                //sad = new SqlDataAdapter("select  id,JobNo,CustomerName,EntryDate,ProductName,ModelNo,SerialNo,Status,Remark,isCompleted,CreatedBy,DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where JobNo='" + jobno + "' AND  isCompleted='1'", con);
    //            //                sad = new SqlDataAdapter("select id, JobNo, CustomerName, EntryDate, ProductName, ModelNo, SerialNo, Status, Remark, isCompleted, CreatedBy, DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where isCompleted = '1'", con);
    //            //                sad.Fill(dt);
    //            //                lbldaycount.ForeColor = System.Drawing.Color.Red;
    //            //            }
    //            //        }
    //            //    }
    //            //    else
    //            //    {
    //            //        con.Close();
    //            //        sad = new SqlDataAdapter("select  id,JobNo,CustomerName,EntryDate,ProductName,ModelNo,SerialNo,Status,Remark,isCompleted,CreatedBy,DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where JobNo='" + jobno + "' AND  isCompleted='1'", con);
    //            //        sad.Fill(dt);
    //            //        lbldaycount.ForeColor = System.Drawing.Color.Red;
    //            //    }
    //            //}
    //        }
    //        else if (ddlEvalution.SelectedItem.Text == "Pending List")
    //        {

    //            //sad = new SqlDataAdapter("select id, JobNo, CustomerName, EntryDate, ProductName, ModelNo, SerialNo, Status, Remark, isCompleted, CreatedBy, DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where isCompleted = '1'", con);
    //            sad = new SqlDataAdapter("select  id,JobNo,CustomerName,Subcustomer,EntryDate,ProductName,EngiName,ModelNo,SerialNo,Status,Remark,isCompleted,CreatedBy,DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where isCompleted is Null", con);

    //            sad.Fill(dt);
    //            SortGvEvaluations.EmptyDataText = "Not Records Found";
    //            SortGvEvaluations.DataSource = dt;
    //            SortGvEvaluations.DataBind();

    //            //foreach (GridViewRow g1 in gv_Evalution.Rows)
    //            //{
    //            //    con.Open();
    //            //    Label jobnono = (Label)g1.FindControl("lblJobno");
    //            //    string jobno = jobnono.Text;
    //            //    Label lbldaycount = (Label)g1.FindControl("lbldaycount");
    //            //    SqlCommand cmdquatationreader = new SqlCommand("select q.isCreateQuata, q.CreatedOn from tblTestingProduct t inner join tbl_Quotation_Hdr q on t.JobNo=q.JobNo where t.JobNo='" + jobno + "'", con);
    //            //    SqlDataReader reader = cmdquatationreader.ExecuteReader();

    //            //    if (reader.HasRows)
    //            //    {
    //            //        if (reader.Read())
    //            //        {
    //            //            status = reader["isCreateQuata"].ToString();

    //            //            if (status == "True")
    //            //            {
    //            //                DateTime quattiondate = Convert.ToDateTime(reader["CreatedOn"].ToString());
    //            //                update = quattiondate.ToString("yyyy-MM-dd");
    //            //                con.Close();
    //            //                sad = new SqlDataAdapter("select  id,JobNo,CustomerName,EntryDate,ProductName,ModelNo,SerialNo,Status,Remark,isCompleted,CreatedBy,CreatedDate,DATEDIFF(DAY, CreatedDate, '" + update + "') AS days from tblTestingProduct where JobNo='" + jobno + "' AND   isCompleted is Null", con);
    //            //                sad.Fill(dt);

    //            //            }
    //            //            else
    //            //            {
    //            //                con.Close();
    //            //                sad = new SqlDataAdapter("select  id,JobNo,CustomerName,EntryDate,ProductName,ModelNo,SerialNo,Status,Remark,isCompleted,CreatedBy,DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where JobNo='" + jobno + "' AND   isCompleted is Null", con);
    //            //                sad.Fill(dt);
    //            //                lbldaycount.ForeColor = System.Drawing.Color.Red;
    //            //            }
    //            //        }
    //            //    }
    //            //    else
    //            //    {
    //            //        con.Close();
    //            //        sad = new SqlDataAdapter("select  id,JobNo,CustomerName,EntryDate,ProductName,ModelNo,SerialNo,Status,Remark,isCompleted,CreatedBy,DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where JobNo='" + jobno + "' AND   isCompleted is Null", con);
    //            //        sad.Fill(dt);
    //            //        lbldaycount.ForeColor = System.Drawing.Color.Red;
    //            //    }
    //            //}
    //        }
    //        SortGvEvaluations.EmptyDataText = "Not Records Found";
    //        SortGvEvaluations.DataSource = dt;
    //        SortGvEvaluations.DataBind();

    //        ViewState["Record"] = "List";
    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }
    //}

    protected void Grid_Filter_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "selectdata")
        {

            Response.Redirect("GoTesting.aspx?JobNo=" + encrypt(e.CommandArgument.ToString()));
        }
        if (e.CommandName == "RowEdit")
        {
            Response.Redirect("GoTesting.aspx?id=" + encrypt(e.CommandArgument.ToString()) + "");
        }
    }

    protected void Grid_Filter_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Filter.PageIndex = e.NewPageIndex;
        TestingFilter();
        dgvgridview.Visible = false;
    }

    //Sorted functionlity start 
    protected void SortGvEvaluations_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

        if (ViewState["Record"].ToString() == "Job")
        {
            SortGvEvaluations.PageIndex = e.NewPageIndex;
            getsortedjobnoGrid();
        }
        if (ViewState["Record"].ToString() == "status")
        {
            SortGvEvaluations.PageIndex = e.NewPageIndex;
            GetsortedstatusGrid();
        }
        if (ViewState["Record"].ToString() == "Product")
        {
            SortGvEvaluations.PageIndex = e.NewPageIndex;
            GetsortedproductGrid();
        }

        if (ViewState["Record"].ToString() == "Date")
        {
            SortGvEvaluations.PageIndex = e.NewPageIndex;
            GetsortedDatewiseGrid();
        }

        if (ViewState["Record"].ToString() == "Customer")
        {
            SortGvEvaluations.PageIndex = e.NewPageIndex;
            GetsortedcustomerGrid();
        }
        if (ViewState["Record"].ToString() == "datewiseCustomer")
        {
            SortGvEvaluations.PageIndex = e.NewPageIndex;
            GetsortedcustomerGrid();
        }

        if (ViewState["Record"].ToString() == "datewiseproduct")
        {
            SortGvEvaluations.PageIndex = e.NewPageIndex;
            Getdatewiseproductgrid();
        }

        if (ViewState["Record"].ToString() == "Datewisestatus")
        {
            SortGvEvaluations.PageIndex = e.NewPageIndex;
            GetsortedstatusGrid();
        }

        if (ViewState["Record"].ToString() == "List")
        {
            SortGvEvaluations.PageIndex = e.NewPageIndex;
            Getsortedlist();
        }


        //if (ViewState["Record"].ToString() == "List")
        //{
        //    SortGvEvaluations.PageIndex = e.NewPageIndex;
        //    Getsortedlist();
        //}

        if (ViewState["Record"].ToString() == "DatewiseList")
        {
            SortGvEvaluations.PageIndex = e.NewPageIndex;
            Getdatewisesortedlist();
        }
        if (ViewState["Record"].ToString() == "ListWiseSearch")
        {
            SortGvEvaluations.PageIndex = e.NewPageIndex;
            GetListwisesearch();
        }

    }
    public void getsortedjobno()
    {
        gv_Evalution.Visible = false;
        ViewState["Record"] = "Job";
        string jobno = txtJobNo.Text;
        DataTable dt = new DataTable();
        sad = new SqlDataAdapter("select id,JobNo,CustomerName,ProductName,Subcustomer,ModelNo,SerialNo,EngiName,TestingDate,Status,Remark,EntryDate,isCompleted,CreatedDate,CreatedBy,Quotation_no,inwardEntrystatus,ReportedTO, DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where JobNo= '" + jobno + "'", con);
        sad.Fill(dt);
        SortGvEvaluations.DataSource = dt;
        SortGvEvaluations.DataBind();

    }
    public void getsortedjobnoGrid()
    {
        string jobno = txtJobNo.Text;

        DataTable dt = new DataTable();
        sad = new SqlDataAdapter("select id,JobNo,CustomerName,ProductName,Subcustomer,ModelNo,SerialNo,EngiName,TestingDate,Status,Remark,EntryDate,isCompleted,CreatedDate,CreatedBy,Quotation_no,inwardEntrystatus,ReportedTO, DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where JobNo= '" + jobno + "'", con);
        sad.Fill(dt);
        SortGvEvaluations.DataSource = dt;
        SortGvEvaluations.DataBind();

    }
    public void Getsortedstatus()
    {
        gv_Evalution.Visible = false;
        ViewState["Record"] = "status";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter(" select [id],[JobNo],[CustomerName],Subcustomer,[ProductName],[ModelNo],[SerialNo],[EngiName],[TestingDate],[Status],[Remark],[EntryDate],[isCompleted],[isdeleted],[isCancel],[CreatedBy],[CreatedDate] ,UpdateBy,UpdatedDate,Quotation_no,inwardEntrystatus,ReportedTO,DATEDIFF(DAY, CreatedDate, getdate()) AS days FROM tblTestingProduct Where Status = '" + txtstatus.Text + "' ", con);
        sad.Fill(dt);
        SortGvEvaluations.EmptyDataText = "Not Records Found";
        SortGvEvaluations.DataSource = dt;
        SortGvEvaluations.DataBind();
    }
    public void GetsortedstatusGrid()
    {
        gv_Evalution.Visible = false;
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter(" select [id],[JobNo],[CustomerName],Subcustomer,[ProductName],[ModelNo],[SerialNo],[EngiName],[TestingDate],[Status],[Remark],[EntryDate],[isCompleted],[isdeleted],[isCancel],[CreatedBy],[CreatedDate] ,UpdateBy,UpdatedDate,Quotation_no,inwardEntrystatus,ReportedTO,DATEDIFF(DAY, CreatedDate, getdate()) AS days FROM tblTestingProduct Where Status = '" + txtstatus.Text + "' ", con);
        sad.Fill(dt);
        SortGvEvaluations.EmptyDataText = "Not Records Found";
        SortGvEvaluations.DataSource = dt;
        SortGvEvaluations.DataBind();
    }
    public void Getsortedproduct()
    {
        gv_Evalution.Visible = false;
        ViewState["Record"] = "Product";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select  id,JobNo,CustomerName,Subcustomer,EntryDate,ProductName,EngiName,ModelNo,SerialNo,Status,Remark,isCompleted,CreatedBy,DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where  ProductName LIKE '%" + txtproduct.Text + "%'", con);
        sad.Fill(dt);
        SortGvEvaluations.EmptyDataText = "Not Records Found";
        SortGvEvaluations.DataSource = dt;
        SortGvEvaluations.DataBind();
    }
    public void GetsortedproductGrid()
    {
        gv_Evalution.Visible = false;
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select  id,JobNo,CustomerName,Subcustomer,EntryDate,ProductName,EngiName,ModelNo,SerialNo,Status,Remark,isCompleted,CreatedBy,DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where  ProductName LIKE '%" + txtproduct.Text + "%'", con);
        sad.Fill(dt);
        SortGvEvaluations.EmptyDataText = "Not Records Found";
        SortGvEvaluations.DataSource = dt;
        SortGvEvaluations.DataBind();
    }
    public void GetsortedDatewise()
    {
        gv_Evalution.Visible = false;
        ViewState["Record"] = "Date";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select  id,JobNo,CustomerName,Subcustomer,EntryDate,ProductName,EngiName,ModelNo,SerialNo,Status,Remark,isCompleted,CreatedBy,DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where  EntryDate between '" + txtDateSearchfrom.Text + "' AND  '" + txtDateSearchto.Text + "' ", con);
        sad.Fill(dt);
        SortGvEvaluations.EmptyDataText = "Not Records Found";
        SortGvEvaluations.DataSource = dt;
        SortGvEvaluations.DataBind();
    }
    public void GetsortedDatewiseGrid()
    {
        gv_Evalution.Visible = false;
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select  id,JobNo,CustomerName,Subcustomer,EntryDate,ProductName,EngiName,ModelNo,SerialNo,Status,Remark,isCompleted,CreatedBy,DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where  EntryDate between '" + txtDateSearchfrom.Text + "' AND  '" + txtDateSearchto.Text + "' ", con);
        sad.Fill(dt);
        SortGvEvaluations.EmptyDataText = "Not Records Found";
        SortGvEvaluations.DataSource = dt;
        SortGvEvaluations.DataBind();
    }
    public void Getsortedcustomer()
    {
        gv_Evalution.Visible = false;
        ViewState["Record"] = "Customer";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select  id,JobNo,CustomerName,Subcustomer,EntryDate,ProductName,EngiName,ModelNo,SerialNo,Status,Remark,isCompleted,CreatedBy,DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where  CustomerName LIKE '%" + txtcustomername.Text + "%'", con);
        sad.Fill(dt);
        SortGvEvaluations.EmptyDataText = "Not Records Found";
        SortGvEvaluations.DataSource = dt;
        SortGvEvaluations.DataBind();
    }
    public void GetsortedcustomerGrid()
    {
        gv_Evalution.Visible = false;
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select  id,JobNo,CustomerName,Subcustomer,EntryDate,ProductName,EngiName,ModelNo,SerialNo,Status,Remark,isCompleted,CreatedBy,DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where  CustomerName LIKE '%" + txtcustomername.Text + "%'", con);
        sad.Fill(dt);
        SortGvEvaluations.EmptyDataText = "Not Records Found";
        SortGvEvaluations.DataSource = dt;
        SortGvEvaluations.DataBind();
    }
    public void GetDatewisecustomergrid()
    {
        gv_Evalution.Visible = false;
        ViewState["Record"] = "datewiseCustomer";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT id, JobNo, CustomerName, Subcustomer, EntryDate, ProductName, EngiName, ModelNo, SerialNo, Status, Remark, isCompleted, CreatedBy, DATEDIFF(DAY, CreatedDate, GETDATE()) AS days FROM tblTestingProduct WHERE EntryDate BETWEEN '" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "' AND CustomerName LIKE '%" + txtcustomername.Text + "%'", con);
        sad.Fill(dt);
        SortGvEvaluations.EmptyDataText = "Not Records Found";
        SortGvEvaluations.DataSource = dt;
        SortGvEvaluations.DataBind();
    }
    public void Getdatwisecustomer()
    {
        gv_Evalution.Visible = false;
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT id, JobNo, CustomerName, Subcustomer, EntryDate, ProductName, EngiName, ModelNo, SerialNo, Status, Remark, isCompleted, CreatedBy, DATEDIFF(DAY, CreatedDate, GETDATE()) AS days FROM tblTestingProduct WHERE EntryDate BETWEEN '" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "' AND CustomerName LIKE '%" + txtcustomername.Text + "%'", con);
        sad.Fill(dt);
        SortGvEvaluations.EmptyDataText = "Not Records Found";
        SortGvEvaluations.DataSource = dt;
        SortGvEvaluations.DataBind();
    }
    public void Getdatewiseproduct()
    {
        gv_Evalution.Visible = false;
        ViewState["Record"] = "datewiseproduct";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT id, JobNo, CustomerName, Subcustomer, EntryDate, ProductName, EngiName, ModelNo, SerialNo, Status, Remark, isCompleted, CreatedBy, DATEDIFF(DAY, CreatedDate, GETDATE()) AS days FROM tblTestingProduct WHERE EntryDate BETWEEN '" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "' AND ProductName LIKE '%" + txtproduct.Text + "%'", con);
        sad.Fill(dt);
        SortGvEvaluations.EmptyDataText = "Not Records Found";
        SortGvEvaluations.DataSource = dt;
        SortGvEvaluations.DataBind();
    }
    public void Getdatewiseproductgrid()
    {
        gv_Evalution.Visible = false;
        ViewState["Record"] = "datewiseproduct";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT id, JobNo, CustomerName, Subcustomer, EntryDate, ProductName, EngiName, ModelNo, SerialNo, Status, Remark, isCompleted, CreatedBy, DATEDIFF(DAY, CreatedDate, GETDATE()) AS days FROM tblTestingProduct WHERE EntryDate BETWEEN '" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "' AND ProductName LIKE '%" + txtproduct.Text + "%'", con);
        sad.Fill(dt);
        SortGvEvaluations.EmptyDataText = "Not Records Found";
        SortGvEvaluations.DataSource = dt;
        SortGvEvaluations.DataBind();
    }
    public void Getdatewisestatus()
    {
        gv_Evalution.Visible = false;
        ViewState["Record"] = "Datewisestatus";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT id, JobNo, CustomerName, Subcustomer, EntryDate, ProductName, EngiName, ModelNo, SerialNo, Status, Remark, isCompleted, CreatedBy, DATEDIFF(DAY, CreatedDate, GETDATE()) AS days FROM tblTestingProduct WHERE EntryDate BETWEEN '" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "' AND Status = '" + txtstatus.Text + "'", con);
        sad.Fill(dt);
        SortGvEvaluations.EmptyDataText = "Not Records Found";
        SortGvEvaluations.DataSource = dt;
        SortGvEvaluations.DataBind();
    }
    public void Getdatewisestatusgrid()
    {
        gv_Evalution.Visible = false;
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT id, JobNo, CustomerName, Subcustomer, EntryDate, ProductName, EngiName, ModelNo, SerialNo, Status, Remark, isCompleted, CreatedBy, DATEDIFF(DAY, CreatedDate, GETDATE()) AS days FROM tblTestingProduct WHERE EntryDate BETWEEN '" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "' AND Status = '" + txtstatus.Text + "'", con);
        sad.Fill(dt);
        SortGvEvaluations.EmptyDataText = "Not Records Found";
        SortGvEvaluations.DataSource = dt;
        SortGvEvaluations.DataBind();
    }
    public void Getsortedlist()
    {
        gv_Evalution.Visible = false;
        dgvgridview.Visible = false;
        DataTable dt = new DataTable();

        if (ddlEvalution.SelectedItem.Text == "All")
        {
            foreach (GridViewRow g1 in gv_Evalution.Rows)
            {
                con.Open();
                Label jobnono = (Label)g1.FindControl("lblJobno");
                string jobno = jobnono.Text;
                Label lbldaycount = (Label)g1.FindControl("lbldaycount");
                SqlCommand cmdquatationreader = new SqlCommand("select q.isCreateQuata, q.CreatedOn from tblTestingProduct t inner join tbl_Quotation_Hdr q on t.JobNo=q.JobNo where t.JobNo='" + jobno + "'", con);
                SqlDataReader reader = cmdquatationreader.ExecuteReader();

                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        status = reader["isCreateQuata"].ToString();

                        if (status == "True")
                        {
                            DateTime quattiondate = Convert.ToDateTime(reader["CreatedOn"].ToString());
                            update = quattiondate.ToString("yyyy-MM-dd");
                            con.Close();
                            sad = new SqlDataAdapter("select  id,JobNo,CustomerName,Subcustomer,EntryDate,ProductName,EngiName,ModelNo,SerialNo,Status,Remark,isCompleted,CreatedBy,CreatedDate,DATEDIFF(DAY, CreatedDate, '" + update + "') AS days from tblTestingProduct where JobNo='" + jobno + "'", con);
                            sad.Fill(dt);

                        }
                        else
                        {
                            con.Close();
                            sad = new SqlDataAdapter("select  id,JobNo,CustomerName,Subcustomer,EntryDate,ProductName,EngiName,ModelNo,SerialNo,Status,Remark,isCompleted,CreatedBy,DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where JobNo='" + jobno + "'", con);
                            sad.Fill(dt);
                            lbldaycount.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                }
                else
                {
                    con.Close();
                    sad = new SqlDataAdapter("select  id,JobNo,CustomerName,Subcustomer,EntryDate,ProductName,EngiName,ModelNo,SerialNo,Status,Remark,isCompleted,CreatedBy,DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where JobNo='" + jobno + "'", con);
                    sad.Fill(dt);
                    lbldaycount.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
        else if (ddlEvalution.SelectedItem.Text == "Tested List")
        {

            sad = new SqlDataAdapter("select id, JobNo, CustomerName,Subcustomer, EntryDate, ProductName,EngiName, ModelNo, SerialNo, Status, Remark, isCompleted, CreatedBy, DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where isCompleted = '1'", con);
            //sad = new SqlDataAdapter("select id,JobNo,CustomerName,ProductName,ModelNo,SerialNo,EngiName,TestingDate,Status,Remark,EntryDate,isCompleted,CreatedDate,CreatedBy,Quotation_no,inwardEntrystatus,ReportedTO, DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where JobNo= '" + jobno + "'", con);
            sad.Fill(dt);
            SortGvEvaluations.EmptyDataText = "Not Records Found";
            SortGvEvaluations.DataSource = dt;
            SortGvEvaluations.DataBind();


        }
        else if (ddlEvalution.SelectedItem.Text == "Pending List")
        {

            //sad = new SqlDataAdapter("select id, JobNo, CustomerName, EntryDate, ProductName, ModelNo, SerialNo, Status, Remark, isCompleted, CreatedBy, DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where isCompleted = '1'", con);
            sad = new SqlDataAdapter("select  id,JobNo,CustomerName,Subcustomer,EntryDate,ProductName,EngiName,ModelNo,SerialNo,Status,Remark,isCompleted,CreatedBy,DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where isCompleted is Null", con);

            sad.Fill(dt);
            SortGvEvaluations.EmptyDataText = "Not Records Found";
            SortGvEvaluations.DataSource = dt;
            SortGvEvaluations.DataBind();


        }
   

    }
    public void Getdatewisesortedlist()
    {
        ViewState["Record"] = "DatewiseList";
        gv_Evalution.Visible = false;
        dgvgridview.Visible = false;
       
        DataTable dt = new DataTable();

        if (ddlEvalution.SelectedItem.Text == "All")
        {
            foreach (GridViewRow g1 in gv_Evalution.Rows)
            {
                con.Open();
                Label jobnono = (Label)g1.FindControl("lblJobno");
                string jobno = jobnono.Text;
                Label lbldaycount = (Label)g1.FindControl("lbldaycount");
                SqlCommand cmdquatationreader = new SqlCommand("SELECT q.isCreateQuata, q.CreatedOn FROM tblTestingProduct t INNER JOIN tbl_Quotation_Hdr q ON t.JobNo = q.JobNo WHERE Quotation_Date BETWEEN '" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "' AND t.JobNo = '" + jobno + "'", con);
                SqlDataReader reader = cmdquatationreader.ExecuteReader();

                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        status = reader["isCreateQuata"].ToString();

                        if (status == "True")
                        {
                            DateTime quattiondate = Convert.ToDateTime(reader["CreatedOn"].ToString());
                            update = quattiondate.ToString("yyyy-MM-dd");
                            con.Close();
                            sad = new SqlDataAdapter("select  id,JobNo,CustomerName,Subcustomer,EntryDate,ProductName,EngiName,ModelNo,SerialNo,Status,Remark,isCompleted,CreatedBy,CreatedDate,DATEDIFF(DAY, CreatedDate, '" + update + "') AS days from tblTestingProduct where JobNo='" + jobno + "'", con);
                            sad.Fill(dt);

                        }
                        else
                        {
                            con.Close();
                            sad = new SqlDataAdapter("select  id,JobNo,CustomerName,Subcustomer,EntryDate,ProductName,EngiName,ModelNo,SerialNo,Status,Remark,isCompleted,CreatedBy,DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where JobNo='" + jobno + "'", con);
                            sad.Fill(dt);
                            lbldaycount.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                }
                else
                {
                    con.Close();
                    sad = new SqlDataAdapter("select  id,JobNo,CustomerName,Subcustomer,EntryDate,ProductName,EngiName,ModelNo,SerialNo,Status,Remark,isCompleted,CreatedBy,DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where JobNo='" + jobno + "'", con);
                    sad.Fill(dt);
                    lbldaycount.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
        else if (ddlEvalution.SelectedItem.Text == "Tested List")
        {

            SqlDataAdapter sad = new SqlDataAdapter("SELECT id, JobNo, CustomerName, Subcustomer, EntryDate, ProductName, EngiName, ModelNo, SerialNo, Status, Remark, isCompleted, CreatedBy, DATEDIFF(DAY, CreatedDate, GETDATE()) AS days FROM tblTestingProduct WHERE isCompleted = '1' AND EntryDate BETWEEN '" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "'", con);
            //sad = new SqlDataAdapter("select id,JobNo,CustomerName,ProductName,ModelNo,SerialNo,EngiName,TestingDate,Status,Remark,EntryDate,isCompleted,CreatedDate,CreatedBy,Quotation_no,inwardEntrystatus,ReportedTO, DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where JobNo= '" + jobno + "'", con);
            sad.Fill(dt);
            SortGvEvaluations.EmptyDataText = "Not Records Found";
            SortGvEvaluations.DataSource = dt;
            SortGvEvaluations.DataBind();


        }
        else if (ddlEvalution.SelectedItem.Text == "Pending List")
        {

            //sad = new SqlDataAdapter("select id, JobNo, CustomerName, EntryDate, ProductName, ModelNo, SerialNo, Status, Remark, isCompleted, CreatedBy, DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where isCompleted = '1'", con);
            sad = new SqlDataAdapter("select  id,JobNo,CustomerName,Subcustomer,EntryDate,ProductName,EngiName,ModelNo,SerialNo,Status,Remark,isCompleted,CreatedBy,DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where isCompleted is Null AND EntryDate BETWEEN '" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "'", con);

            sad.Fill(dt);
            SortGvEvaluations.EmptyDataText = "Not Records Found";
            SortGvEvaluations.DataSource = dt;
            SortGvEvaluations.DataBind();


        }
 


    }

    public void GetListwisesearch()
    {
       
        try
        {
       
            dgvgridview.Visible = false;
            DataTable dt = new DataTable();

            if (ddlEvalution.SelectedItem.Text == "All")
            {
                foreach (GridViewRow g1 in SortGvEvaluations.Rows)
                {
                    con.Open();
                    Label jobnono = (Label)g1.FindControl("lblJobno");
                    string jobno = jobnono.Text;
                    Label lbldaycount = (Label)g1.FindControl("lbldaycount");
                    SqlCommand cmdquatationreader = new SqlCommand("select q.isCreateQuata, q.CreatedOn from tblTestingProduct t inner join tbl_Quotation_Hdr q on t.JobNo=q.JobNo where t.JobNo='" + jobno + "'", con);
                    SqlDataReader reader = cmdquatationreader.ExecuteReader();

                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            status = reader["isCreateQuata"].ToString();

                            if (status == "True")
                            {
                                DateTime quattiondate = Convert.ToDateTime(reader["CreatedOn"].ToString());
                                update = quattiondate.ToString("yyyy-MM-dd");
                                con.Close();
                                sad = new SqlDataAdapter("select  id,JobNo,CustomerName,Subcustomer,EntryDate,ProductName,EngiName,ModelNo,SerialNo,Status,Remark,isCompleted,CreatedBy,CreatedDate,DATEDIFF(DAY, CreatedDate, '" + update + "') AS days from tblTestingProduct where JobNo='" + jobno + "'", con);
                                sad.Fill(dt);

                            }
                            else
                            {
                                con.Close();
                                sad = new SqlDataAdapter("select  id,JobNo,CustomerName,Subcustomer,EntryDate,ProductName,EngiName,ModelNo,SerialNo,Status,Remark,isCompleted,CreatedBy,DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where JobNo='" + jobno + "'", con);
                                sad.Fill(dt);
                                lbldaycount.ForeColor = System.Drawing.Color.Red;
                            }
                        }
                    }
                    else
                    {
                        con.Close();
                        sad = new SqlDataAdapter("select  id,JobNo,CustomerName,Subcustomer,EntryDate,ProductName,EngiName,ModelNo,SerialNo,Status,Remark,isCompleted,CreatedBy,DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where JobNo='" + jobno + "'", con);
                        sad.Fill(dt);
                        lbldaycount.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
            else if (ddlEvalution.SelectedItem.Text == "Tested List")
            {

                sad = new SqlDataAdapter("select id, JobNo, CustomerName,Subcustomer, EntryDate, ProductName,EngiName, ModelNo, SerialNo, Status, Remark, isCompleted, CreatedBy, DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where isCompleted = '1'", con);
                //sad = new SqlDataAdapter("select id,JobNo,CustomerName,ProductName,ModelNo,SerialNo,EngiName,TestingDate,Status,Remark,EntryDate,isCompleted,CreatedDate,CreatedBy,Quotation_no,inwardEntrystatus,ReportedTO, DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where JobNo= '" + jobno + "'", con);
                sad.Fill(dt);
                SortGvEvaluations.EmptyDataText = "Not Records Found";
                SortGvEvaluations.DataSource = dt;
                SortGvEvaluations.DataBind();

                
            }
            else if (ddlEvalution.SelectedItem.Text == "Pending List")
            {

                //sad = new SqlDataAdapter("select id, JobNo, CustomerName, EntryDate, ProductName, ModelNo, SerialNo, Status, Remark, isCompleted, CreatedBy, DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where isCompleted = '1'", con);
                sad = new SqlDataAdapter("select  id,JobNo,CustomerName,Subcustomer,EntryDate,ProductName,EngiName,ModelNo,SerialNo,Status,Remark,isCompleted,CreatedBy,DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where isCompleted is Null", con);

                sad.Fill(dt);
                SortGvEvaluations.EmptyDataText = "Not Records Found";
                SortGvEvaluations.DataSource = dt;
                SortGvEvaluations.DataBind();

               
            }

            sad.Fill(dt);
            SortGvEvaluations.EmptyDataText = "Not Records Found";
            SortGvEvaluations.DataSource = dt;
            SortGvEvaluations.DataBind();


            ViewState["Record"] = "List";
        }
        catch (Exception)
        {
            throw;
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        //required to avoid the run time error "  
        //Control 'GridView1' of type 'Grid View' must be placed inside a form tag with runat=server."  
    }
    protected void lnkbtn_ExportExcel_Click(object sender, EventArgs e)
    {
        ExportGridToExcel();
    }
    private void ExportGridToExcel()
    {
        if (ViewState["Excell"] != null)
        {
            string Method = ViewState["Excell"].ToString();
            if (Method == "getsortedjobno")
            {
                getsortedjobnoForExcell();
            }
            if (Method == "Getsortedstatus")
            {
                GetsortedstatusForExcell();
            }
            if (Method == "Getsortedproduct")
            {
                GetsortedproductForExcell();
            }
            if (Method == "GetsortedDatewise")
            {
                GetsortedDatewiseForExcell();
            }
            if (Method == "Getsortedcustomer")
            {
                GetsortedcustomerForExcell();
            }
            if (Method == "Getdatwisecustomer")
            {
                GetsortedcustomerForExcell();
            }

            if (Method == "Getdatewiseproduct")
            {
                GetdatewiseproductForExcell();
            }
            if (Method == "Getdatewisestatus")
            {
                GetdatewisestatusForExcell();
            }
            if (Method == "DatewiseList")
            {
                GetdatewisesortedlistForExcell();
            }
            if (Method == "ListWiseSearch")
            {
                GetListwisesearchForExcell();
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
        string FileName = "Evalution_List_" + DateTime.Now + ".xls";
        StringWriter strwritter = new StringWriter();
        HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
        ExportToExcelGrid.GridLines = GridLines.Both;
        ExportToExcelGrid.HeaderStyle.Font.Bold = true;
        ExportToExcelGrid.RenderControl(htmltextwrtter);
        Response.Write(strwritter.ToString());
        Response.End();
    }
    public void getsortedjobnoForExcell()
    {
        gv_Evalution.Visible = false;
        ViewState["Record"] = "Job";
        string jobno = txtJobNo.Text;
        DataTable dt = new DataTable();
        sad = new SqlDataAdapter("select id,JobNo,CustomerName,ProductName,Subcustomer,ModelNo,SerialNo,EngiName,TestingDate,Status,Remark,EntryDate,isCompleted,CreatedDate,CreatedBy,Quotation_no,inwardEntrystatus,ReportedTO, DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where JobNo= '" + jobno + "'", con);
        sad.Fill(dt);
        ExportToExcelGrid.DataSource = dt;
        ExportToExcelGrid.DataBind();

    }
    public void GetsortedstatusForExcell()
    {
        gv_Evalution.Visible = false;
        ViewState["Record"] = "status";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter(" select [id],[JobNo],[CustomerName],Subcustomer,[ProductName],[ModelNo],[SerialNo],[EngiName],[TestingDate],[Status],[Remark],[EntryDate],[isCompleted],[isdeleted],[isCancel],[CreatedBy],[CreatedDate] ,UpdateBy,UpdatedDate,Quotation_no,inwardEntrystatus,ReportedTO,DATEDIFF(DAY, CreatedDate, getdate()) AS days FROM tblTestingProduct Where Status = '" + txtstatus.Text + "' ", con);
        sad.Fill(dt);
        ExportToExcelGrid.EmptyDataText = "Not Records Found";
        ExportToExcelGrid.DataSource = dt;
        ExportToExcelGrid.DataBind();
    }
    public void GetsortedproductForExcell()
    {
        gv_Evalution.Visible = false;
        ViewState["Record"] = "Product";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select  id,JobNo,CustomerName,Subcustomer,EntryDate,ProductName,EngiName,ModelNo,SerialNo,Status,Remark,isCompleted,CreatedBy,DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where  ProductName LIKE '%" + txtproduct.Text + "%'", con);
        sad.Fill(dt);
        ExportToExcelGrid.EmptyDataText = "Not Records Found";
        ExportToExcelGrid.DataSource = dt;
        ExportToExcelGrid.DataBind();
    }
    public void GetsortedDatewiseForExcell()
    {
        gv_Evalution.Visible = false;
        ViewState["Record"] = "Date";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select  id,JobNo,CustomerName,Subcustomer,EntryDate,ProductName,EngiName,ModelNo,SerialNo,Status,Remark,isCompleted,CreatedBy,DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where  EntryDate between '" + txtDateSearchfrom.Text + "' AND  '" + txtDateSearchto.Text + "' ", con);
        sad.Fill(dt);
        ExportToExcelGrid.EmptyDataText = "Not Records Found";
        ExportToExcelGrid.DataSource = dt;
        ExportToExcelGrid.DataBind();
    }
    public void GetsortedcustomerForExcell()
    {
        gv_Evalution.Visible = false;
        ViewState["Record"] = "Customer";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select  id,JobNo,CustomerName,Subcustomer,EntryDate,ProductName,EngiName,ModelNo,SerialNo,Status,Remark,isCompleted,CreatedBy,DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where  CustomerName LIKE '%" + txtcustomername.Text + "%'", con);
        sad.Fill(dt);
        ExportToExcelGrid.EmptyDataText = "Not Records Found";
        ExportToExcelGrid.DataSource = dt;
        ExportToExcelGrid.DataBind();
    }
    public void GetdatwisecustomerForExcell()
    {
        gv_Evalution.Visible = false;
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT id, JobNo, CustomerName, Subcustomer, EntryDate, ProductName, EngiName, ModelNo, SerialNo, Status, Remark, isCompleted, CreatedBy, DATEDIFF(DAY, CreatedDate, GETDATE()) AS days FROM tblTestingProduct WHERE EntryDate BETWEEN '" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "' AND CustomerName LIKE '%" + txtcustomername.Text + "%'", con);
        sad.Fill(dt);
        ExportToExcelGrid.EmptyDataText = "Not Records Found";
        ExportToExcelGrid.DataSource = dt;
        ExportToExcelGrid.DataBind();
    }
    public void GetdatewiseproductForExcell()
    {
        gv_Evalution.Visible = false;
        ViewState["Record"] = "datewiseproduct";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT id, JobNo, CustomerName, Subcustomer, EntryDate, ProductName, EngiName, ModelNo, SerialNo, Status, Remark, isCompleted, CreatedBy, DATEDIFF(DAY, CreatedDate, GETDATE()) AS days FROM tblTestingProduct WHERE EntryDate BETWEEN '" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "' AND ProductName LIKE '%" + txtproduct.Text + "%'", con);
        sad.Fill(dt);
        ExportToExcelGrid.EmptyDataText = "Not Records Found";
        ExportToExcelGrid.DataSource = dt;
        ExportToExcelGrid.DataBind();
    }
    public void GetdatewisestatusForExcell()
    {
        gv_Evalution.Visible = false;
        ViewState["Record"] = "Datewisestatus";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT id, JobNo, CustomerName, Subcustomer, EntryDate, ProductName, EngiName, ModelNo, SerialNo, Status, Remark, isCompleted, CreatedBy, DATEDIFF(DAY, CreatedDate, GETDATE()) AS days FROM tblTestingProduct WHERE EntryDate BETWEEN '" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "' AND Status = '" + txtstatus.Text + "'", con);
        sad.Fill(dt);
        ExportToExcelGrid.EmptyDataText = "Not Records Found";
        ExportToExcelGrid.DataSource = dt;
        ExportToExcelGrid.DataBind();
    }
    public void GetdatewisesortedlistForExcell()
    {
        ViewState["Record"] = "DatewiseList";
        gv_Evalution.Visible = false;
        dgvgridview.Visible = false;
        SortGvEvaluations.Visible = false;
        DataTable dt = new DataTable();

        if (ddlEvalution.SelectedItem.Text == "All")
        {
            foreach (GridViewRow g1 in gv_Evalution.Rows)
            {
                con.Open();
                Label jobnono = (Label)g1.FindControl("lblJobno");
                string jobno = jobnono.Text;
                Label lbldaycount = (Label)g1.FindControl("lbldaycount");
                SqlCommand cmdquatationreader = new SqlCommand("SELECT q.isCreateQuata, q.CreatedOn FROM tblTestingProduct t INNER JOIN tbl_Quotation_Hdr q ON t.JobNo = q.JobNo WHERE Quotation_Date BETWEEN '" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "' AND t.JobNo = '" + jobno + "'", con);
                SqlDataReader reader = cmdquatationreader.ExecuteReader();

                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        status = reader["isCreateQuata"].ToString();

                        if (status == "True")
                        {
                            DateTime quattiondate = Convert.ToDateTime(reader["CreatedOn"].ToString());
                            update = quattiondate.ToString("yyyy-MM-dd");
                            con.Close();
                            sad = new SqlDataAdapter("select  id,JobNo,CustomerName,Subcustomer,EntryDate,ProductName,EngiName,ModelNo,SerialNo,Status,Remark,isCompleted,CreatedBy,CreatedDate,DATEDIFF(DAY, CreatedDate, '" + update + "') AS days from tblTestingProduct where JobNo='" + jobno + "'", con);
                            sad.Fill(dt);

                        }
                        else
                        {
                            con.Close();
                            sad = new SqlDataAdapter("select  id,JobNo,CustomerName,Subcustomer,EntryDate,ProductName,EngiName,ModelNo,SerialNo,Status,Remark,isCompleted,CreatedBy,DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where JobNo='" + jobno + "'", con);
                            sad.Fill(dt);
                            lbldaycount.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                }
                else
                {
                    con.Close();
                    sad = new SqlDataAdapter("select  id,JobNo,CustomerName,Subcustomer,EntryDate,ProductName,EngiName,ModelNo,SerialNo,Status,Remark,isCompleted,CreatedBy,DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where JobNo='" + jobno + "'", con);
                    sad.Fill(dt);
                    lbldaycount.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
        else if (ddlEvalution.SelectedItem.Text == "Tested List")
        {

            SqlDataAdapter sad = new SqlDataAdapter("SELECT id, JobNo, CustomerName, Subcustomer, EntryDate, ProductName, EngiName, ModelNo, SerialNo, Status, Remark, isCompleted, CreatedBy, DATEDIFF(DAY, CreatedDate, GETDATE()) AS days FROM tblTestingProduct WHERE isCompleted = '1' AND EntryDate BETWEEN '" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "'", con);
            //sad = new SqlDataAdapter("select id,JobNo,CustomerName,ProductName,ModelNo,SerialNo,EngiName,TestingDate,Status,Remark,EntryDate,isCompleted,CreatedDate,CreatedBy,Quotation_no,inwardEntrystatus,ReportedTO, DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where JobNo= '" + jobno + "'", con);
            sad.Fill(dt);
            ExportToExcelGrid.EmptyDataText = "Not Records Found";
            ExportToExcelGrid.DataSource = dt;
            ExportToExcelGrid.DataBind();


        }
        else if (ddlEvalution.SelectedItem.Text == "Pending List")
        {

            //sad = new SqlDataAdapter("select id, JobNo, CustomerName, EntryDate, ProductName, ModelNo, SerialNo, Status, Remark, isCompleted, CreatedBy, DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where isCompleted = '1'", con);
            sad = new SqlDataAdapter("select  id,JobNo,CustomerName,Subcustomer,EntryDate,ProductName,EngiName,ModelNo,SerialNo,Status,Remark,isCompleted,CreatedBy,DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where isCompleted is Null AND EntryDate BETWEEN '" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "'", con);

            sad.Fill(dt);
            ExportToExcelGrid.EmptyDataText = "Not Records Found";
            ExportToExcelGrid.DataSource = dt;
            ExportToExcelGrid.DataBind();


        }
        ExportToExcelGrid.EmptyDataText = "Not Records Found";
        ExportToExcelGrid.DataSource = dt;
        ExportToExcelGrid.DataBind();


    }


    public void GetListwisesearchForExcell()
    {
        try
        {
            dgvgridview.Visible = false;
            DataTable dt = new DataTable();

            if (ddlEvalution.SelectedItem.Text == "All")
            {
                foreach (GridViewRow g1 in gv_Evalution.Rows)
                {
                    con.Open();
                    Label jobnono = (Label)g1.FindControl("lblJobno");
                    string jobno = jobnono.Text;
                    Label lbldaycount = (Label)g1.FindControl("lbldaycount");
                    SqlCommand cmdquatationreader = new SqlCommand("select q.isCreateQuata, q.CreatedOn from tblTestingProduct t inner join tbl_Quotation_Hdr q on t.JobNo=q.JobNo where t.JobNo='" + jobno + "'", con);
                    SqlDataReader reader = cmdquatationreader.ExecuteReader();

                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            status = reader["isCreateQuata"].ToString();

                            if (status == "True")
                            {
                                DateTime quattiondate = Convert.ToDateTime(reader["CreatedOn"].ToString());
                                update = quattiondate.ToString("yyyy-MM-dd");
                                con.Close();
                                sad = new SqlDataAdapter("select  id,JobNo,CustomerName,Subcustomer,EntryDate,ProductName,EngiName,ModelNo,SerialNo,Status,Remark,isCompleted,CreatedBy,CreatedDate,DATEDIFF(DAY, CreatedDate, '" + update + "') AS days from tblTestingProduct where JobNo='" + jobno + "'", con);
                                sad.Fill(dt);

                            }
                            else
                            {
                                con.Close();
                                sad = new SqlDataAdapter("select  id,JobNo,CustomerName,Subcustomer,EntryDate,ProductName,EngiName,ModelNo,SerialNo,Status,Remark,isCompleted,CreatedBy,DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where JobNo='" + jobno + "'", con);
                                sad.Fill(dt);
                                lbldaycount.ForeColor = System.Drawing.Color.Red;
                            }
                        }
                    }
                    else
                    {
                        con.Close();
                        sad = new SqlDataAdapter("select  id,JobNo,CustomerName,Subcustomer,EntryDate,ProductName,EngiName,ModelNo,SerialNo,Status,Remark,isCompleted,CreatedBy,DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where JobNo='" + jobno + "'", con);
                        sad.Fill(dt);
                        lbldaycount.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
            else if (ddlEvalution.SelectedItem.Text == "Tested List")
            {

                sad = new SqlDataAdapter("select id, JobNo, CustomerName,Subcustomer, EntryDate, ProductName,EngiName, ModelNo, SerialNo, Status, Remark, isCompleted, CreatedBy, DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where isCompleted = '1'", con);
                //sad = new SqlDataAdapter("select id,JobNo,CustomerName,ProductName,ModelNo,SerialNo,EngiName,TestingDate,Status,Remark,EntryDate,isCompleted,CreatedDate,CreatedBy,Quotation_no,inwardEntrystatus,ReportedTO, DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where JobNo= '" + jobno + "'", con);
                sad.Fill(dt);
                ExportToExcelGrid.EmptyDataText = "Not Records Found";
                ExportToExcelGrid.DataSource = dt;
                ExportToExcelGrid.DataBind();


            }
            else if (ddlEvalution.SelectedItem.Text == "Pending List")
            {

                //sad = new SqlDataAdapter("select id, JobNo, CustomerName, EntryDate, ProductName, ModelNo, SerialNo, Status, Remark, isCompleted, CreatedBy, DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where isCompleted = '1'", con);
                sad = new SqlDataAdapter("select  id,JobNo,CustomerName,Subcustomer,EntryDate,ProductName,EngiName,ModelNo,SerialNo,Status,Remark,isCompleted,CreatedBy,DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblTestingProduct where isCompleted is Null", con);

                sad.Fill(dt);
                ExportToExcelGrid.EmptyDataText = "Not Records Found";
                ExportToExcelGrid.DataSource = dt;
                ExportToExcelGrid.DataBind();


            }
       

            ViewState["Record"] = "List";
        }
        catch (Exception)
        {
            throw;
        }
    }




    protected void ddlEvalution_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlEvalution.SelectedItem.Text != "Select")
        {
            ViewState["Excell"] = "ListWiseSearch";
            GetListwisesearch();
        }
    }
}