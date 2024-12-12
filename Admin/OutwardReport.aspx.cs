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


public partial class Admin_OutwardReport : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //ddlsearchfltr();
            OutwardGridView();
        }
    }


    //protected void ddlsearchfltr()
    //{
    //    try
    //    {

    //        DataTable dt = new DataTable();

    //        con.Open();
    //        SqlDataAdapter sad = new SqlDataAdapter("select [Id],[Status] from RepairingStatusOutward", con);
    //        sad.Fill(dt);
    //        ddlsearch.DataValueField = "Id";
    //        ddlsearch.DataTextField = "Status";

    //        ddlsearch.DataSource = dt;
    //        ddlsearch.DataBind();

    //        con.Close();




    //    }
    //    catch (Exception)
    //    {

    //        throw;
    //    }
    //}
    void OutwardGridView()
    {
        try
        {
            DataTable dt = new DataTable();

            con.Open();
            SqlDataAdapter sad = new SqlDataAdapter("select [Id],[JobNo],[DateOut],[CustName],[MateName],[ModelNo],[SerialNo],[JobWorkby],[DateReturn],[ReturnRepair],[CreateBy],[CreatedDate],[UpdateBy],[UpdateDate],[againstby] from [tblOutwardEntry] where isdeleted='0' ORDER BY [DateOut] Desc", con);
            sad.Fill(dt);

            gv_Outward.EmptyDataText = "Not Records Found";
            gv_Outward.DataSource = dt;
            gv_Outward.DataBind();

            con.Close();

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void lnkBtnsearch_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtjob.Text) && string.IsNullOrEmpty(txtSearchCust.Text) && string.IsNullOrEmpty(txtsearchworkby.Text) && string.IsNullOrEmpty(txtDateSearchto.Text) && string.IsNullOrEmpty(txtDateSearchfrom.Text) && string.IsNullOrEmpty(txttested.Text))
        {
            OutwardGridView();
        }

        else if (!string.IsNullOrEmpty(txtsearchworkby.Text) && !string.IsNullOrEmpty(txtjob.Text))
        {
            ViewState["Record"] = "Getsortedcustomer";
            DataTable dt = new DataTable();
            SqlDataAdapter sad = new SqlDataAdapter("select * from tblOutwardEntry where [JobWorkby]='" + txtsearchworkby.Text + "' AND JobNo='" + txtjob.Text + "' AND isdeleted='0'", con);
            sad.Fill(dt);
            gv_Outward.EmptyDataText = "Record Not Found";
            gv_Outward.DataSource = dt;
            gv_Outward.DataBind();
        }
        else if (!string.IsNullOrEmpty(txtSearchCust.Text) && !string.IsNullOrEmpty(txtjob.Text))
        {
            ViewState["Record"] = "Getsortedcustomer1";
            DataTable dt = new DataTable();
            SqlDataAdapter sad = new SqlDataAdapter("select * from tblOutwardEntry where [CustName]='" + txtSearchCust.Text + "' AND JobNo='" + txtjob.Text + "' AND isdeleted='0'", con);
            sad.Fill(dt);
            gv_Outward.EmptyDataText = "Record Not Found";
            gv_Outward.DataSource = dt;
            gv_Outward.DataBind();
        }

        else if (!string.IsNullOrEmpty(txtDateSearchto.Text) && !string.IsNullOrEmpty(txtDateSearchfrom.Text) && !string.IsNullOrEmpty(txtSearchCust.Text))
        {
            ViewState["Excell"] = "custanddate";
            custanddate();

            //ViewState["Record"] = "Getsortedcustomer2";
            //DataTable dt = new DataTable();
            //SqlDataAdapter sad = new SqlDataAdapter("select * from tblOutwardEntry where [DateOut] between '" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "' AND isdeleted='0'", con);
            //sad.Fill(dt);
            //gv_Outward.EmptyDataText = "Record Not Found";
            //gv_Outward.DataSource = dt;
            //gv_Outward.DataBind();
        }
        else if (!string.IsNullOrEmpty(txtDateSearchto.Text) && !string.IsNullOrEmpty(txtDateSearchfrom.Text))
        {
            ViewState["Excell"] = "Getsortedcustomer2";
            Getsortedcustomer2();

            //ViewState["Record"] = "Getsortedcustomer2";
            //DataTable dt = new DataTable();
            //SqlDataAdapter sad = new SqlDataAdapter("select * from tblOutwardEntry where [DateOut] between '" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "' AND isdeleted='0'", con);
            //sad.Fill(dt);
            //gv_Outward.EmptyDataText = "Record Not Found";
            //gv_Outward.DataSource = dt;
            //gv_Outward.DataBind();
        }
        else if (string.IsNullOrEmpty(txtDateSearchto.Text) && !string.IsNullOrEmpty(txtDateSearchfrom.Text))
        {
            ViewState["Record"] = "Getsortedcustomer3";
            DataTable dt = new DataTable();
            SqlDataAdapter sad = new SqlDataAdapter("select * from tblOutwardEntry where [DateOut] ='" + txtDateSearchfrom.Text + "' AND isdeleted='0'", con);
            sad.Fill(dt);
            gv_Outward.EmptyDataText = "Record Not Found";
            gv_Outward.DataSource = dt;
            gv_Outward.DataBind();
        }

        else if (!string.IsNullOrEmpty(txtDateSearchto.Text) && string.IsNullOrEmpty(txtDateSearchfrom.Text))
        {

            ViewState["Record"] = "Getsortedcustomer4";
            DataTable dt = new DataTable();
            SqlDataAdapter sad = new SqlDataAdapter("select * from tblOutwardEntry where [DateOut]" +
                " ='" + txtDateSearchto.Text + "' AND isdeleted='0'", con);
            sad.Fill(dt);
            gv_Outward.EmptyDataText = "Record Not Found";
            gv_Outward.DataSource = dt;
            gv_Outward.DataBind();
        }
        else if (!string.IsNullOrEmpty(txtjob.Text))
        {
            ViewState["Excell"] = "Getsortedcustomer5";
            Getsortedcustomer5();
            //ViewState["Record"] = "Getsortedcustomer5";
            //DataTable dt = new DataTable();
            //SqlDataAdapter sad = new SqlDataAdapter("select * from tblOutwardEntry where JobNo='" + txtjob.Text + "' AND isdeleted='0'  ", con);
            //sad.Fill(dt);
            //gv_Outward.EmptyDataText = "Record Not Found";
            //gv_Outward.DataSource = dt;
            //gv_Outward.DataBind();
        }
        else if (!string.IsNullOrEmpty(txtSearchCust.Text))
        {
            ViewState["Excell"] = "Getsortedcustomer6";
            Getsortedcustomer6();

            //ViewState["Record"] = "Getsortedcustomer6";
            //DataTable dt = new DataTable();
            //SqlDataAdapter sad = new SqlDataAdapter("select * from tblOutwardEntry where [CustName]='" + txtSearchCust.Text + "' AND isdeleted='0'", con);
            //sad.Fill(dt);
            //gv_Outward.EmptyDataText = "Record Not Found";
            //gv_Outward.DataSource = dt;
            //gv_Outward.DataBind();
        }
        else if (!string.IsNullOrEmpty(txtsearchworkby.Text))
        {
            ViewState["Excell"] = "Getsortedcustomer7";
            Getsortedcustomer7();

            //ViewState["Record"] = "Getsortedcustomer7";
            //DataTable dt = new DataTable();
            //SqlDataAdapter sad = new SqlDataAdapter("select * from tblOutwardEntry where JobWorkby='" + txtsearchworkby.Text + "' AND isdeleted='0'", con);
            //sad.Fill(dt);
            //gv_Outward.EmptyDataText = "Record Not Found";
            //gv_Outward.DataSource = dt;
            //gv_Outward.DataBind();
        }
        else if (!string.IsNullOrEmpty(txttested.Text))
        {
            ViewState["Excell"] = "Getsortedcustomer8";
            Getsortedcustomer8();
        }

        else if (!string.IsNullOrEmpty(txtDateSearchto.Text) && !string.IsNullOrEmpty(txtDateSearchfrom.Text))
        {
            GetDatweisestatus();
        }
    }

    //Search product type
    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetProductList(string prefixText, int count)
    {
        return AutoFillProductlist(prefixText);
    }

    public static List<string> AutoFillProductlist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT ReturnRepair from tblOutwardEntry where " + "ReturnRepair like @Search + '%' AND isdeleted='0' ";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> ReturnRepair = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        ReturnRepair.Add(sdr["ReturnRepair"].ToString());
                    }
                }
                con.Close();
                return ReturnRepair;
            }

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
                com.CommandText = "select DISTINCT CustName from [tblOutwardEntry] where CustName like @Search + '%' AND isdeleted='0' ";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> CustName = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        CustName.Add(sdr["CustName"].ToString());
                    }
                }
                con.Close();
                return CustName;
            }

        }
    }

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> AutoFilljobworklist(string prefixText, int count)
    {
        return AutoFilljobworklist(prefixText);
    }

    public static List<string> AutoFilljobworklist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT [JobWorkby] from [tblOutwardEntry] where " + "[JobWorkby] like @Search + '%' AND isdeleted='0' ";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> JobWorkby = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        JobWorkby.Add(sdr["JobWorkby"].ToString());
                    }
                }
                con.Close();
                return JobWorkby;
            }

        }
    }
    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetJobList(string prefixText, int count)
    {
        return AutoFillJoblist(prefixText);
    }

    public static List<string> AutoFillJoblist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT JobNo from [tblOutwardEntry] where " + "JobNo like @Search + '%' AND isdeleted='0'" +
                    "";

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


    protected void lnkrefresh_Click(object sender, EventArgs e)
    {
        txtjob.Text = ""; txtSearchCust.Text = ""; txtsearchworkby.Text = ""; txtDateSearchfrom.Text = ""; txtDateSearchto.Text = "";
        OutwardGridView();
    }

    protected void lnkshow_Click(object sender, EventArgs e)
    {
        int rowIndex = Convert.ToInt32(((sender as LinkButton).NamingContainer as GridViewRow).RowIndex);
        GridViewRow row = gv_Outward.Rows[rowIndex];

        lbljobshow.Text = (row.FindControl("lblJobno") as Label).Text;
        lblmatenameshow.Text = (row.FindControl("lblMatename") as Label).Text;
        lblSerialNoshow.Text = (row.FindControl("lblSriNo") as Label).Text;
        lblDateouthow.Text = (row.FindControl("lblDateOut") as Label).Text;
        lblcustomershow.Text = (row.FindControl("lblCustname") as Label).Text;
        lblJobWorkbyshow.Text = (row.FindControl("lbljobworkby") as Label).Text;
        lblmodelNoshow.Text = (row.FindControl("lblModelNo") as Label).Text;
        //lblCustStatusshow.Text = (row.FindControl("lblCustStatus") as Label).Text;
        lblDateReturnshow.Text = (row.FindControl("lblreturnDate") as Label).Text;
        // lblReturnRepairshow.Text = (row.FindControl("lblrepairstatus") as Label).Text;


        modelprofile.Show();
    }

    //protected void ddlsearch_TextChanged(object sender, EventArgs e)
    //{
    //    if (ddlsearch.SelectedItem.Text == "Select Status")
    //    {
    //        OutwardGridView();
    //    }
    //    else
    //    {


    //    SqlDataAdapter Da = new SqlDataAdapter("Select * From tblOutwardEntry WHERE isdeleted='0' AND ReturnRepair='" + ddlsearch.SelectedItem.Text + "'", con);
    //    DataTable Dt = new DataTable();
    //    Da.Fill(Dt);


    //    gv_Outward.DataSource = Dt;
    //    gv_Outward.DataBind();
    //    gv_Outward.EmptyDataText = "Not Records Found";
    //    }
    //}

    public void GetDatweisestatus()
    {
        SqlDataAdapter Da = new SqlDataAdapter("SELECT * FROM tblOutwardEntry WHERE isdeleted='0' AND [DateOut] BETWEEN '" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "' AND isdeleted = '0'", con);
        DataTable Dt = new DataTable();
        Da.Fill(Dt);
        gv_Outward.DataSource = Dt;
        gv_Outward.DataBind();
        gv_Outward.EmptyDataText = "Not Records Found";
    }


    public void Getsortedcustomer()
    {
        ViewState["Record"] = "Getsortedcustomer";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblOutwardEntry where [JobWorkby]='" + txtsearchworkby.Text + "' AND JobNo='" + txtjob.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        gv_Outward.EmptyDataText = "Record Not Found";
        gv_Outward.DataSource = dt;
        gv_Outward.DataBind();
    }


    public void Getsortedcustomer1()
    {
        ViewState["Record"] = "Getsortedcustomer1";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblOutwardEntry where [CustName]='" + txtSearchCust.Text + "' AND JobNo='" + txtjob.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        gv_Outward.EmptyDataText = "Record Not Found";
        gv_Outward.DataSource = dt;
        gv_Outward.DataBind();
    }

    public void Getsortedcustomer2()
    {
        ViewState["Excell"] = "Getsortedcustomer2";
        ViewState["Record"] = "Getsortedcustomer2";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblOutwardEntry where [DateOut] between '" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        gv_Outward.EmptyDataText = "Record Not Found";
        gv_Outward.DataSource = dt;
        gv_Outward.DataBind();
    }

    public void custanddate()
    {
        ViewState["Excell"] = "custanddate";
        ViewState["Record"] = "custanddate";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblOutwardEntry where [DateOut] between '" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "' AND CustName='" + txtSearchCust.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        gv_Outward.EmptyDataText = "Record Not Found";
        gv_Outward.DataSource = dt;
        gv_Outward.DataBind();
    }

    public void Getsortedcustomer2Excel()
    {
        ViewState["Excell"] = "Getsortedcustomer2";
        ViewState["Record"] = "Getsortedcustomer2";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblOutwardEntry where [DateOut] between '" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        sortedgv.EmptyDataText = "Record Not Found";
        sortedgv.DataSource = dt;
        sortedgv.DataBind();
    }


    public void Getsortedcustomer3()
    {
        ViewState["Record"] = "Getsortedcustomer3";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblOutwardEntry where [DateOut] ='" + txtDateSearchfrom.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        gv_Outward.EmptyDataText = "Record Not Found";
        gv_Outward.DataSource = dt;
        gv_Outward.DataBind();
    }

    public void Getsortedcustomer4()
    {
        ViewState["Record"] = "Getsortedcustomer4";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblOutwardEntry where [DateOut]" +
            " ='" + txtDateSearchto.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        gv_Outward.EmptyDataText = "Record Not Found";
        gv_Outward.DataSource = dt;
        gv_Outward.DataBind();
    }


    public void Getsortedcustomer5()
    {
        ViewState["Excell"] = "Getsortedcustomer5";
        ViewState["Record"] = "Getsortedcustomer5";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblOutwardEntry where JobNo='" + txtjob.Text + "' AND isdeleted='0'  ", con);
        sad.Fill(dt);
        gv_Outward.EmptyDataText = "Record Not Found";
        gv_Outward.DataSource = dt;
        gv_Outward.DataBind();
    }

    public void Getsortedcustomer5Excel()
    {
        ViewState["Excell"] = "Getsortedcustomer5";
        ViewState["Record"] = "Getsortedcustomer5";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblOutwardEntry where JobNo='" + txtjob.Text + "' AND isdeleted='0'  ", con);
        sad.Fill(dt);
        sortedgv.EmptyDataText = "Record Not Found";
        sortedgv.DataSource = dt;
        sortedgv.DataBind();
    }

    public void Getsortedcustomer6()
    {
        ViewState["Excell"] = "Getsortedcustomer6";
        ViewState["Record"] = "Getsortedcustomer6";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblOutwardEntry where [CustName]='" + txtSearchCust.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        gv_Outward.EmptyDataText = "Record Not Found";
        gv_Outward.DataSource = dt;
        gv_Outward.DataBind();
    }

    public void Getsortedcustomer6Excel()
    {
        ViewState["Excell"] = "Getsortedcustomer6";
        ViewState["Record"] = "Getsortedcustomer6";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblOutwardEntry where [CustName]='" + txtSearchCust.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        sortedgv.EmptyDataText = "Record Not Found";
        sortedgv.DataSource = dt;
        sortedgv.DataBind();
    }

    public void Getsortedcustomer7()
    {
        ViewState["Excell"] = "Getsortedcustomer7";
        ViewState["Record"] = "Getsortedcustomer7";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblOutwardEntry where JobWorkby='" + txtsearchworkby.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        gv_Outward.EmptyDataText = "Record Not Found";
        gv_Outward.DataSource = dt;
        gv_Outward.DataBind();
    }


    public void Getsortedcustomer8()
    {
        ViewState["Excell"] = "Getsortedcustomer8";
        ViewState["Record"] = "Getsortedcustomer8";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblOutwardEntry where ReturnRepair='" + txttested.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        gv_Outward.EmptyDataText = "Record Not Found";
        gv_Outward.DataSource = dt;
        gv_Outward.DataBind();
    }

    public void Getsortedcustomer7Excel()
    {
        ViewState["Excell"] = "Getsortedcustomer7";
        ViewState["Record"] = "Getsortedcustomer7";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblOutwardEntry where JobWorkby='" + txtsearchworkby.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        sortedgv.EmptyDataText = "Record Not Found";
        sortedgv.DataSource = dt;
        sortedgv.DataBind();
    }

    public void Getsortedcustomer8Excel()
    {
        ViewState["Excell"] = "Getsortedcustomer8";
        ViewState["Record"] = "Getsortedcustomer8";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblOutwardEntry where ReturnRepair='" + txttested.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        sortedgv.EmptyDataText = "Record Not Found";
        sortedgv.DataSource = dt;
        sortedgv.DataBind();
    }

    public void custanddateExcel()
    {
        ViewState["Excell"] = "custanddate";
        ViewState["Record"] = "custanddate";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblOutwardEntry where [DateOut] between '" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "' AND CustName='" + txtSearchCust.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        sortedgv.EmptyDataText = "Record Not Found";
        sortedgv.DataSource = dt;
        sortedgv.DataBind();
    }

    protected void gv_Outward_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {



        if (ViewState["Record"] != null)
        {

            if (ViewState["Record"].ToString() == "Getsortedcustomer")
            {
                gv_Outward.PageIndex = e.NewPageIndex;
                Getsortedcustomer();
            }


            if (ViewState["Record"].ToString() == "Getsortedcustomer1")
            {
                gv_Outward.PageIndex = e.NewPageIndex;
                Getsortedcustomer1();
            }

            if (ViewState["Record"].ToString() == "Getsortedcustomer2")
            {
                gv_Outward.PageIndex = e.NewPageIndex;
                Getsortedcustomer2();
            }

            if (ViewState["Record"].ToString() == "Getsortedcustomer3")
            {
                gv_Outward.PageIndex = e.NewPageIndex;

                Getsortedcustomer3();
            }
            if (ViewState["Record"].ToString() == "Getsortedcustomer4")
            {
                gv_Outward.PageIndex = e.NewPageIndex;

                Getsortedcustomer4();
            }

            if (ViewState["Record"].ToString() == "Getsortedcustomer5")
            {
                gv_Outward.PageIndex = e.NewPageIndex;

                Getsortedcustomer5();
            }

            if (ViewState["Record"].ToString() == "Getsortedcustomer6")
            {
                gv_Outward.PageIndex = e.NewPageIndex;

                Getsortedcustomer6();
            }
            if (ViewState["Record"].ToString() == "Getsortedcustomer7")
            {
                gv_Outward.PageIndex = e.NewPageIndex;

                Getsortedcustomer7();
            }
            if (ViewState["Record"].ToString() == "Getsortedcustomer8")
            {
                gv_Outward.PageIndex = e.NewPageIndex;

                Getsortedcustomer8();
            }
            if (ViewState["Record"].ToString() == "custanddate")
            {
                gv_Outward.PageIndex = e.NewPageIndex;

                custanddate();
            }
            if (ViewState["Record"].ToString() == "custanddate")
            {
                gv_Outward.PageIndex = e.NewPageIndex;

                custanddateExcel();
            }
         
        }
        else
        {
            gv_Outward.PageIndex = e.NewPageIndex;
            OutwardGridView();
        }
    }


    public override void VerifyRenderingInServerForm(Control control)
    {
        //required to avoid the run time error "
        //Control 'GridView1' of type 'Grid View' must be placed inside a form tag with runat=server."
    }

    protected void btnExportExcel_Click(object sender, EventArgs e)
    {
        ExportGridToExcel();
    }

    private void ExportGridToExcel()
    {

        if (ViewState["Excell"] != null)
        {
            gv_Outward.Visible = false;
            string Method = ViewState["Excell"].ToString();

            if (Method == "Getsortedcustomer5")
            {
                Getsortedcustomer5Excel();
            }
            if (Method == "Getsortedcustomer6")
            {
                Getsortedcustomer6Excel();
            }
            if (Method == "Getsortedcustomer7")
            {
                Getsortedcustomer7Excel();
            }
            if (Method == "Getsortedcustomer2")
            {
                Getsortedcustomer2Excel();
            }
            if (Method == "Getsortedcustomer8")
            {
                Getsortedcustomer8Excel();
            }
            if (Method == "custanddate")
            {
                custanddateExcel();
            }

        }
        else
        {
            GridExportExcel();
        }

        Response.Clear();
        Response.Buffer = true;
        Response.ClearContent();
        Response.ClearHeaders();
        Response.Charset = "";
        string FileName = "Outward_Reprt_List_" + DateTime.Now + ".xls";
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

    public void GridExportExcel()
    {
        DataTable dt = new DataTable();

        con.Open();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblOutwardEntry where isdeleted='0'", con);
        sad.Fill(dt);
        sortedgv.EmptyDataText = "Not Records Found";
        sortedgv.DataSource = dt;
        sortedgv.DataBind();

        con.Close();
    }
}