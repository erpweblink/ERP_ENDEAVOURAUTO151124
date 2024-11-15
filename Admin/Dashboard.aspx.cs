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
using System.Collections;

public partial class Admin_Dashboard : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["adminname"] == null)
        {
            Response.Redirect("../LoginPage.aspx");
        }
        else
        {
            if (!IsPostBack)
            {
                var Role = Session["adminname"].ToString();
                if (Role == "Admin")
                {
                    DivAdminDashboard.Visible = true;
                    CustomerCount(); VendorCount(); ProductCount(); ComponantCount(); testinglisttoday();
                    totalpendingtestunglist(); todaycreateQuatation(); QuationlistTop20(); todaycreateCustomerPO();
                    TotalCustomerPOlist(); todayVendorPOLIst(); TotalVendorPOlist(); TodayInvoicelist(); TodayInvoicelist(); Totalinvoicelist();
                }
                else if (Role == "Reception")
                {
                    DivReceptionDashboard.Visible = true;
                    receptiontestinglisttoday();
                    Receptiontotalpendingtestunglist();
                }
                else if (Role == "Technical")
                {
                    DivTechnicalDashboard.Visible = true;
                    Technicaltestinglisttoday();
                    Technicaltotalpendingtestunglist();
                }
                else if (Role == "Customer Support")
                {
                    DivCustomerSupportDashboard.Visible = true;
                    Customersupporttestinglisttoday();
                    Customersupporttotalpendingtestunglist();
                }
                else if (Role == "SubCustomer")
                {
                    DivSchneiderDashboard.Visible = true;
                    subcustomertodaycreateQuatation();
                    SubcustomerPendingQuationlistTop20();
                    subcustomertodaycreateCustomerPO();
                    subcustomerTotalCustomerPOlist();
                    subcustomerTodayInvoicelist();
                    subcustomerTotalinvoicelist();

                }
                else if (Role == "Accounts")
                {
                    
                }
                //else
                //{
                //    Divcountblock.Visible = false;
                //    Divcountblock1.Visible = false;
                //    Divcountblock2.Visible = false;
                //    Divcountblock3.Visible = false;

                //    QuationlistTop20(); todaycreateCustomerPO(); TotalCustomerPOlist(); todayVendorPOLIst(); TotalVendorPOlist();
                //    TodayInvoicelist(); Totalinvoicelist(); testinglisttoday(); totalpendingtestunglist(); todaycreateQuatation();
                //}
            }
        }
    }

    protected void CustomerCount()
    {
        int count = 0;
        con.Open();
        SqlCommand cmd = new SqlCommand("SELECT Count(*) FROM tblCustomer where [isdeleted]='0' AND IsStatus='1'", con);
        count = Convert.ToInt16(cmd.ExecuteScalar());
        lblcustomercount.Text = count.ToString();

        con.Close();
    }
    protected void VendorCount()
    {
        int count = 0;
        con.Open();
        SqlCommand cmd = new SqlCommand("SELECT Count(*) FROM tblVendor where [isdeleted]='0' AND IsStatus='1'", con);
        count = Convert.ToInt16(cmd.ExecuteScalar());
        lblvendorcount.Text = count.ToString();
        con.Close();
    }

    protected void ProductCount()
    {
        int count = 0;
        con.Open();
        SqlCommand cmd = new SqlCommand("SELECT Count(*) FROM tblProduct where [isdeleted]='0' AND IsStatus='1' ", con);
        count = Convert.ToInt16(cmd.ExecuteScalar());
        lblproductcount.Text = count.ToString();
        con.Close();
    }

    protected void ComponantCount()
    {
        int count = 0;
        con.Open();
        SqlCommand cmd = new SqlCommand("SELECT Count(*) FROM tblComponent where [isdeleted]='0' AND IsStatus='1' ", con);
        count = Convert.ToInt16(cmd.ExecuteScalar());
        lblcompcount.Text = count.ToString();
        con.Close();
    }

    protected void testinglisttoday()
    {
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * FROM [tblTestingProduct] where Convert(char(10), CreatedDate, 120) = Convert(date, getdate()) AND isCompleted = '1' AND isdeleted = '0' ", con);
        sad.Fill(dt);
        gv_todaytestingprod.EmptyDataText = " Records Not Found ";
        gv_todaytestingprod.DataSource = dt;
        gv_todaytestingprod.DataBind();
        con.Close();
    }


    protected void gv_todaytestingprod_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_todaytestingprod.PageIndex = e.NewPageIndex;
        testinglisttoday();
    }

    protected void gv_pendingtesting_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_pendingtesting.PageIndex = e.NewPageIndex;
        totalpendingtestunglist();
    }
    protected void totalpendingtestunglist()
    {
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * FROM [tblTestingProduct] where  isCompleted is NULL AND isdeleted = '0' ", con);
        sad.Fill(dt);
        gv_pendingtesting.EmptyDataText = " Records Not Found";
        gv_pendingtesting.DataSource = dt;
        gv_pendingtesting.DataBind();
    }
    protected void todaycreateQuatation()
    {
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * FROM tbl_Quotation_Hdr where Convert(char(10), CreatedOn, 120) = Convert(date, getdate()) AND  isdeleted = '0' AND (isCreateQuata='1' OR mnQuatation='1')  ", con);
        sad.Fill(dt);
        gv_Quot_List.EmptyDataText = "Records Not Found";
        gv_Quot_List.DataSource = dt;
        gv_Quot_List.DataBind();
    }

    protected void gv_Quot_List_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_Quot_List.PageIndex = e.NewPageIndex;
        todaycreateQuatation();
    }

    protected void Gv_totalQuatationList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Gv_totalQuatationList.PageIndex = e.NewPageIndex;
        QuationlistTop20();
    }
    protected void QuationlistTop20()
    {
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * FROM tbl_Quotation_Hdr where isCompleted='1'  AND isdeleted = '0' AND isCreateQuata is NULL AND  mnQuatation is NULL  ", con);
        sad.Fill(dt);
        Gv_totalQuatationList.EmptyDataText = "Records Not Found";
        Gv_totalQuatationList.DataSource = dt;
        Gv_totalQuatationList.DataBind();
    }

    protected void todaycreateCustomerPO()
    {
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * FROM CustomerPO_Hdr where Convert(char(10), CreatedOn, 120) = Convert(date, getdate()) AND  Is_Deleted = '0' ", con);
        sad.Fill(dt);
        GvCustomerpoListtoday.EmptyDataText = "Records Not Found";
        GvCustomerpoListtoday.DataSource = dt;
        GvCustomerpoListtoday.DataBind();
    }

    protected void GvCustomerpoListtoday_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvCustomerpoListtoday.PageIndex = e.NewPageIndex;
        todaycreateCustomerPO();
    }

    protected void gv_totalCustomerPOList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_totalCustomerPOList.PageIndex = e.NewPageIndex;
        TotalCustomerPOlist();
    }
    protected void TotalCustomerPOlist()
    {
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select top 20 * FROM CustomerPO_Hdr where   Is_Deleted = '0'  ", con);
        sad.Fill(dt);
        gv_totalCustomerPOList.EmptyDataText = "Records Not Found";
        gv_totalCustomerPOList.DataSource = dt;
        gv_totalCustomerPOList.DataBind();
    }

    protected void todayVendorPOLIst()
    {
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * FROM tblPurchaseOrderHdr where Convert(char(10), CreatedOn, 120) = Convert(date, getdate()) AND  Is_Deleted = '0' ", con);
        sad.Fill(dt);
        GvPurchaseOrderList.EmptyDataText = "Records Not Found";
        GvPurchaseOrderList.DataSource = dt;
        GvPurchaseOrderList.DataBind();
    }

    protected void GvPurchaseOrderList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvPurchaseOrderList.PageIndex = e.NewPageIndex;
        todayVendorPOLIst();
    }

    protected void TotalVendorPOlist()
    {
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select top 20 * FROM tblPurchaseOrderHdr where   Is_Deleted = '0' ", con);
        sad.Fill(dt);
        gv_totalvendorlist.EmptyDataText = "Records Not Found";
        gv_totalvendorlist.DataSource = dt;
        gv_totalvendorlist.DataBind();
    }

    protected void gv_totalvendorlist_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_totalvendorlist.PageIndex = e.NewPageIndex;
        TotalVendorPOlist();
    }

    protected void TodayInvoicelist()
    {
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter(" select * FROM tblInvoiceHdr where Convert(char(10), CreatedOn, 120) = Convert(date, getdate()) AND  Is_Deleted = '0' ", con);
        sad.Fill(dt);
        gv_todayInvoiceList.EmptyDataText = "Records Not Found";
        gv_todayInvoiceList.DataSource = dt;
        gv_todayInvoiceList.DataBind();
    }

    protected void gv_todayInvoiceList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_todayInvoiceList.PageIndex = e.NewPageIndex;
        TodayInvoicelist();
    }

    protected void Totalinvoicelist()
    {
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select top 20 * FROM tblInvoiceHdr where   Is_Deleted = '0'  ", con);
        sad.Fill(dt);
        gv_totalinvoicelist.EmptyDataText = "Records Not Found";
        gv_totalinvoicelist.DataSource = dt;
        gv_totalinvoicelist.DataBind();
    }

    protected void gv_totalinvoicelist_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_totalinvoicelist.PageIndex = e.NewPageIndex;
        Totalinvoicelist();
    }

    protected void gv_todaytestingprod_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        gv_todaytestingprod.Columns[3].Visible = true;
    }

    protected void gv_pendingtesting_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        gv_pendingtesting.Columns[3].Visible = true;
    }

    //Reception Dashboard start

    protected void receptiontestinglisttoday()
    {
        try
        {
            DataTable dt = new DataTable();
            DateTime date = DateTime.Today;
            DateTime ffff1 = Convert.ToDateTime(date);
            string dd = ffff1.ToString("yyyy-MM-dd");
            con.Open();
            SqlDataAdapter sad = new SqlDataAdapter("select * FROM [tblTestingProduct] where Convert(char(10), UpdatedDate, 120) = Convert(date, '" + dd + "') AND isCompleted = '1' AND isdeleted = '0'", con);
            sad.Fill(dt);

            GridReception.EmptyDataText = "Not Records Found";
            GridReception.DataSource = dt;
            GridReception.DataBind();

            con.Close();

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void Receptiontotalpendingtestunglist()
    {
        try
        {
            DataTable dt = new DataTable();
            DateTime date = DateTime.Today;
            DateTime ffff1 = Convert.ToDateTime(date);
            string dd = ffff1.ToString("yyyy-MM-dd");
            con.Open();
            SqlDataAdapter sad = new SqlDataAdapter("select * FROM [tblTestingProduct] where  isCompleted is NULL AND isdeleted = '0'", con);
            sad.Fill(dt);

            GridReceptionpendingestinglist.EmptyDataText = "Not Records Found";
            GridReceptionpendingestinglist.DataSource = dt;
            GridReceptionpendingestinglist.DataBind();

            con.Close();

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void GridReception_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridReception.PageIndex = e.NewPageIndex;
        receptiontestinglisttoday();
    }

    protected void GridReception_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (Session["adminname"].ToString() == "Admin")
        {
            GridReception.Columns[3].Visible = true;

        }
        else
        {
            GridReception.Columns[3].Visible = true;
        }
    }

    protected void GridReceptionpendingestinglist_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridReceptionpendingestinglist.PageIndex = e.NewPageIndex;
        Receptiontotalpendingtestunglist();
    }

    protected void GridReceptionpendingestinglist_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (Session["adminname"].ToString() == "Admin")
        {
            GridReceptionpendingestinglist.Columns[3].Visible = true;
            // cust.Visible = true;
            // cust1.Visible = true;
        }
        else
        {
            // cust.Visible = false;
            //cust1.Visible = false;
            GridReceptionpendingestinglist.Columns[3].Visible = true;
        }
    }

    //Reception Dashboard End


    //Technical Dashboard start

    protected void Technicaltestinglisttoday()
    {
        try
        {
            DataTable dt = new DataTable();
            DateTime date = DateTime.Today;
            DateTime ffff1 = Convert.ToDateTime(date);
            string dd = ffff1.ToString("yyyy-MM-dd");
            con.Open();
            SqlDataAdapter sad = new SqlDataAdapter("select * FROM [tblTestingProduct] where Convert(char(10), UpdatedDate, 120) = Convert(date, '" + dd + "') AND isCompleted = '1' AND isdeleted = '0'", con);
            sad.Fill(dt);

            GridTechnicaltodaytestinglist.EmptyDataText = "Not Records Found";
            GridTechnicaltodaytestinglist.DataSource = dt;
            GridTechnicaltodaytestinglist.DataBind();

            con.Close();

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void Technicaltotalpendingtestunglist()
    {
        try
        {
            DataTable dt = new DataTable();
            DateTime date = DateTime.Today;
            DateTime ffff1 = Convert.ToDateTime(date);
            string dd = ffff1.ToString("yyyy-MM-dd");
            con.Open();
            SqlDataAdapter sad = new SqlDataAdapter("select * FROM [tblTestingProduct] where  isCompleted is NULL AND isdeleted = '0'", con);
            sad.Fill(dt);

            Gridtechnicalpendingtestinglist.EmptyDataText = "Not Records Found";
            Gridtechnicalpendingtestinglist.DataSource = dt;
            Gridtechnicalpendingtestinglist.DataBind();

            con.Close();

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void GridTechnicaltodaytestinglist_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridTechnicaltodaytestinglist.PageIndex = e.NewPageIndex;
        Technicaltestinglisttoday();
    }

    protected void GridTechnicaltodaytestinglist_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (Session["adminname"].ToString() == "Admin")
        {
            GridTechnicaltodaytestinglist.Columns[3].Visible = true;
        }
        else
        {
            GridTechnicaltodaytestinglist.Columns[3].Visible = true;
        }
    }

    protected void Gridtechnicalpendingtestinglist_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Gridtechnicalpendingtestinglist.PageIndex = e.NewPageIndex;
        Technicaltotalpendingtestunglist();
    }

    protected void Gridtechnicalpendingtestinglist_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (Session["adminname"].ToString() == "Admin")
        {
            Gridtechnicalpendingtestinglist.Columns[3].Visible = true;
        }
        else
        {
            Gridtechnicalpendingtestinglist.Columns[3].Visible = true;
        }
    }

    //Technical Dashboard End   


    //Customer Support Start

    protected void Customersupporttestinglisttoday()
    {
        try
        {
            DataTable dt = new DataTable();
            DateTime date = DateTime.Today;
            DateTime ffff1 = Convert.ToDateTime(date);
            string dd = ffff1.ToString("yyyy-MM-dd");
            con.Open();
            SqlDataAdapter sad = new SqlDataAdapter("select * FROM [tblTestingProduct] where Convert(char(10), UpdatedDate, 120) = Convert(date, '" + dd + "') AND isCompleted = '1' AND isdeleted = '0'", con);
            sad.Fill(dt);

            GridCustomerSupportTodayTesting.EmptyDataText = "Not Records Found";
            GridCustomerSupportTodayTesting.DataSource = dt;
            GridCustomerSupportTodayTesting.DataBind();

            con.Close();

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void Customersupporttotalpendingtestunglist()
    {
        try
        {
            DataTable dt = new DataTable();
            DateTime date = DateTime.Today;
            DateTime ffff1 = Convert.ToDateTime(date);
            string dd = ffff1.ToString("yyyy-MM-dd");
            con.Open();
            SqlDataAdapter sad = new SqlDataAdapter("select * FROM [tblTestingProduct] where  isCompleted is NULL AND isdeleted = '0'", con);
            sad.Fill(dt);

            GridCustomerSupportPendingTestingList.EmptyDataText = "Not Records Found";
            GridCustomerSupportPendingTestingList.DataSource = dt;
            GridCustomerSupportPendingTestingList.DataBind();

            con.Close();

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void GridCustomerSupportTodayTesting_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridCustomerSupportTodayTesting.PageIndex = e.NewPageIndex;
        Customersupporttestinglisttoday();
    }

    protected void GridCustomerSupportTodayTesting_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (Session["adminname"].ToString() == "Admin")
        {
            GridCustomerSupportTodayTesting.Columns[3].Visible = true;
        }
        else
        {
            GridCustomerSupportTodayTesting.Columns[3].Visible = true;
        }
    }

    protected void GridCustomerSupportPendingTestingList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridCustomerSupportPendingTestingList.PageIndex = e.NewPageIndex;
        Customersupporttotalpendingtestunglist();
    }

    protected void GridCustomerSupportPendingTestingList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (Session["adminname"].ToString() == "Admin")
        {
            GridCustomerSupportPendingTestingList.Columns[3].Visible = true;
        }
        else
        {
            GridCustomerSupportPendingTestingList.Columns[3].Visible = true;
        }
    }

    //Customer Support End


    //SubCustomer SCHNEIDER Start

    protected void GridSubCustomerTodayCreatedQuotation_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridSubCustomerTodayCreatedQuotation.PageIndex = e.NewPageIndex;
        subcustomertodaycreateQuatation();
    }

    protected void subcustomertodaycreateQuatation()
    {
        try
        {
            DataTable dt = new DataTable();
            DateTime date = DateTime.Today;
            DateTime ffff1 = Convert.ToDateTime(date);
            string dd = ffff1.ToString("yyyy-MM-dd");
            con.Open();

            SqlDataAdapter sdd = new SqlDataAdapter("select * FROM tbl_Quotation_Hdr where Convert(char(10), CreatedOn, 120) = Convert(date, getdate()) AND  isdeleted = '0' AND (isCreateQuata='1' OR mnQuatation='1') AND Customer_Name='Schneider Electric India Pvt.Ltd.' ", con);
            sdd.Fill(dt);
            GridSubCustomerTodayCreatedQuotation.EmptyDataText = "Records Not Found";
            GridSubCustomerTodayCreatedQuotation.DataSource = dt;
            GridSubCustomerTodayCreatedQuotation.DataBind();

            con.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void SubcustomerPendingQuationlistTop20()
    {
        try
        {
            DataTable dt = new DataTable();
            DateTime date = DateTime.Today;
            DateTime ffff1 = Convert.ToDateTime(date);
            string dd = ffff1.ToString("yyyy-MM-dd");
            con.Open();

            SqlDataAdapter sadd = new SqlDataAdapter("select * FROM tbl_Quotation_Hdr where isCompleted='1'  AND isdeleted = '0' AND isCreateQuata is NULL AND  mnQuatation is NULL  AND Customer_Name='Schneider Electric India Pvt.Ltd.'", con);
            sadd.Fill(dt);
            GridSubcustomerpendingquotationlist.EmptyDataText = "Records Not Found";
            GridSubcustomerpendingquotationlist.DataSource = dt;
            GridSubcustomerpendingquotationlist.DataBind();


            con.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void GridSubcustomerpendingquotationlist_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridSubcustomerpendingquotationlist.PageIndex = e.NewPageIndex;
        SubcustomerPendingQuationlistTop20();
    }

    protected void subcustomertodaycreateCustomerPO()
    {
        try
        {
            DataTable dt = new DataTable();
            DateTime date = DateTime.Today;
            DateTime ffff1 = Convert.ToDateTime(date);
            string dd = ffff1.ToString("yyyy-MM-dd");
            con.Open();

            SqlDataAdapter sadd = new SqlDataAdapter("select * FROM CustomerPO_Hdr where Convert(char(10), CreatedOn, 120) = Convert(date, getdate()) AND  Is_Deleted = '0' AND CustomerName ='Schneider Electric India Pvt.Ltd.' ", con);
            sadd.Fill(dt);
            GridSubcustomertodaycustomerpolist.EmptyDataText = "Records Not Found";
            GridSubcustomertodaycustomerpolist.DataSource = dt;
            GridSubcustomertodaycustomerpolist.DataBind();


            con.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void GridSubcustomertodaycustomerpolist_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvCustomerpoListtoday.PageIndex = e.NewPageIndex;
        subcustomertodaycreateCustomerPO();
    }

    protected void GridSubcustomertotalcustomer_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridSubcustomertotalcustomer.PageIndex = e.NewPageIndex;
        subcustomerTotalCustomerPOlist();
    }

    protected void subcustomerTotalCustomerPOlist()
    {
        try
        {
            DataTable dt = new DataTable();
            DateTime date = DateTime.Today;
            DateTime ffff1 = Convert.ToDateTime(date);
            string dd = ffff1.ToString("yyyy-MM-dd");
            con.Open();

            SqlDataAdapter sdd = new SqlDataAdapter("select top 20 * FROM CustomerPO_Hdr where   Is_Deleted = '0' AND CustomerName='Schneider Electric India Pvt.Ltd.' ", con);
            sdd.Fill(dt);
            GridSubcustomertotalcustomer.EmptyDataText = "Records Not Found";
            GridSubcustomertotalcustomer.DataSource = dt;
            GridSubcustomertotalcustomer.DataBind();

            con.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void subcustomerTodayInvoicelist()
    {
        try
        {
            DataTable dt = new DataTable();
            DateTime date = DateTime.Today;
            DateTime ffff1 = Convert.ToDateTime(date);
            string dd = ffff1.ToString("yyyy-MM-dd");
            con.Open();

          
            SqlDataAdapter sdd = new SqlDataAdapter(" select * FROM tblInvoiceHdr where Convert(char(10), CreatedOn, 120) = Convert(date, getdate()) AND  Is_Deleted = '0' AND CompName='Schneider Electric India Pvt.Ltd.' ", con);
            sdd.Fill(dt);
            Gridsubcustomertodayinvoicelist.EmptyDataText = "Records Not Found";
            Gridsubcustomertodayinvoicelist.DataSource = dt;
            Gridsubcustomertodayinvoicelist.DataBind();
           

            con.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void Gridsubcustomertodayinvoicelist_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Gridsubcustomertodayinvoicelist.PageIndex = e.NewPageIndex;
        subcustomerTodayInvoicelist();
    }

    protected void Gridsubcustomertotalinvoicelist_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Gridsubcustomertotalinvoicelist.PageIndex = e.NewPageIndex;
        subcustomerTotalinvoicelist();
    }

    protected void subcustomerTotalinvoicelist()
    {
        try
        {
            DataTable dt = new DataTable();
            DateTime date = DateTime.Today;
            DateTime ffff1 = Convert.ToDateTime(date);
            string dd = ffff1.ToString("yyyy-MM-dd");
            con.Open();
           
                SqlDataAdapter sdd = new SqlDataAdapter("select top 20 * FROM tblInvoiceHdr where   Is_Deleted = '0' AND CompName='Schneider Electric India Pvt.Ltd.' ", con);
                sdd.Fill(dt);
            Gridsubcustomertotalinvoicelist.EmptyDataText = "Records Not Found";
            Gridsubcustomertotalinvoicelist.DataSource = dt;
            Gridsubcustomertotalinvoicelist.DataBind();
      

            con.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    //SubCustomer SCHNEIDER End
}


