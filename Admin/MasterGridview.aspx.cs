﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_MasterGridview : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);

    protected void Page_Load(object sender, EventArgs e)
    {
        //Session["adminname"] = "Admin";

        string role = Session["adminname"].ToString();

        if (!IsPostBack)
        {

            if (role == "Technical")
            {
                Getstatusdata();
                GridView1();
            }
            else
            {
                Getstatusdata();
                GridView();
            }

            //Getstatusdata();
            //GridView();
        }
    }

    public void GridView()
    {

        string strConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(strConnString))
        {
            using (SqlCommand cmd = new SqlCommand("[dbo].[SP_Masterlist]", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                if (txtfromdate.Text == "" && txttodate.Text == "")
                {
                    cmd.Parameters.Add(new SqlParameter("@FromDate", DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@Todate", DBNull.Value));
                }
                else
                {
                    cmd.Parameters.Add(new SqlParameter("@FromDate", txtfromdate.Text));
                    cmd.Parameters.Add(new SqlParameter("@Todate", txttodate.Text));
                }

                if (txtSearch.Text == "")
                    cmd.Parameters.AddWithValue("@Customer", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@Customer", txtSearch.Text);

                if (txtEvolutionEnginName.Text != "")
                {
                    cmd.Parameters.AddWithValue("@Action", "GetMasterlistEvolutionEngiNamewise");
                    cmd.Parameters.AddWithValue("@EngineerName", txtEvolutionEnginName.Text.Trim());
                }

                if (ddlservicetype.SelectedItem.Text != "--Select--")
                {
                    cmd.Parameters.AddWithValue("@Action", "GetMasterlistTypeWise");
                    cmd.Parameters.AddWithValue("@Servicetype", ddlservicetype.SelectedItem.Text.Trim());
                }

                if (txtRepairingENGName.Text != "")
                {
                    cmd.Parameters.AddWithValue("@Action", "GetMasterListRepairingENGNameWise");
                    cmd.Parameters.AddWithValue("@RepairingENGName", txtRepairingENGName.Text.Trim());
                }

                if (ddlStatus.SelectedItem.Text != "--Select--")
                {
                    cmd.Parameters.AddWithValue("@Action", "GetMasterlistStatusWise");
                    cmd.Parameters.AddWithValue("@Status", ddlStatus.SelectedItem.Text.Trim());
                }

                if (txtEvolutionEnginName.Text == "" && ddlservicetype.SelectedItem.Text == "--Select--" && txtRepairingENGName.Text == "" && ddlStatus.SelectedItem.Text == "--Select--")
                {
                    cmd.Parameters.AddWithValue("@Action", "GetMasterList");
                }
                if (txtJobno.Text == "")
                    cmd.Parameters.AddWithValue("@JobNo", DBNull.Value);
                else

                    cmd.Parameters.AddWithValue("@JobNo", txtJobno.Text);

                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataSet ds = new DataSet())
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        Gvmasterlist.DataSource = dt;
                        Gvmasterlist.DataBind();

                    }
                }
            }
        }
    }


    public void GridView1()
    {

        string strConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(strConnString))
        {
            using (SqlCommand cmd = new SqlCommand("[dbo].[SP_Masterlist]", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                if (txtfromdate.Text == "" && txttodate.Text == "")
                {
                    cmd.Parameters.Add(new SqlParameter("@FromDate", DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@Todate", DBNull.Value));
                }
                else
                {
                    cmd.Parameters.Add(new SqlParameter("@FromDate", txtfromdate.Text));
                    cmd.Parameters.Add(new SqlParameter("@Todate", txttodate.Text));
                }

                if (txtSearch.Text == "")
                    cmd.Parameters.AddWithValue("@Customer", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@Customer", txtSearch.Text);

                if (txtEvolutionEnginName.Text != "")
                {
                    cmd.Parameters.AddWithValue("@Action", "GetMasterlistEvolutionEngiNamewise");
                    cmd.Parameters.AddWithValue("@EngineerName", txtEvolutionEnginName.Text.Trim());
                }

                if (ddlservicetype.SelectedItem.Text != "--Select--")
                {
                    cmd.Parameters.AddWithValue("@Action", "GetMasterlistTypeWise");
                    cmd.Parameters.AddWithValue("@Servicetype", ddlservicetype.SelectedItem.Text.Trim());
                }

                if (txtRepairingENGName.Text != "")
                {
                    cmd.Parameters.AddWithValue("@Action", "GetMasterListRepairingENGNameWise");
                    cmd.Parameters.AddWithValue("@RepairingENGName", txtRepairingENGName.Text.Trim());
                }

                if (ddlStatus.SelectedItem.Text != "--Select--")
                {
                    cmd.Parameters.AddWithValue("@Action", "GetMasterlistStatusWise");
                    cmd.Parameters.AddWithValue("@Status", ddlStatus.SelectedItem.Text.Trim());
                }

                if (txtEvolutionEnginName.Text == "" && ddlservicetype.SelectedItem.Text == "--Select--" && txtRepairingENGName.Text == "" && ddlStatus.SelectedItem.Text == "--Select--")
                {
                    cmd.Parameters.AddWithValue("@Action", "GetMasterList");
                }
                if (txtJobno.Text == "")
                    cmd.Parameters.AddWithValue("@JobNo", DBNull.Value);
                else

                    cmd.Parameters.AddWithValue("@JobNo", txtJobno.Text);

                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataSet ds = new DataSet())
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        GridTechnical.DataSource = dt;
                        GridTechnical.DataBind();

                    }
                }
            }
        }
    }

    public void Getstatusdata()
    {
        try
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlDataAdapter sad = new SqlDataAdapter(" select distinct status from [EndeavourAuto].tblJobcardHdr AS JH where isdeleted=0 AND status!= ''", con);
            sad.Fill(dt);
            ddlStatus.DataSource = dt;
            ddlStatus.DataTextField = "status";
            ddlStatus.DataValueField = "status";
            ddlStatus.DataBind();
            ddlStatus.Items.Insert(0, "--Select--");
            con.Close();
        }
        catch (Exception ex)
        {
            string errorMsg = "An error occurred : " + ex.Message;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + errorMsg + "') ", true);
        }
    }

    protected void Gvmasterlist_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Gvmasterlist.PageIndex = e.NewPageIndex;
        GridView();
    }

    protected void txtcname_TextChanged(object sender, EventArgs e)
    {
        GridView();
    }

    //Search Customer List
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
                com.CommandText = "select DISTINCT CustomerName from tblCustomer where " + "CustomerName like '%'+ @Search + '%' AND isdeleted='0'";

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

    protected void txtSearch_TextChanged(object sender, EventArgs e)
    {
        GridView();
    }

    //Search job no. list
    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetJobNoList(string prefixText, int count)
    {
        return AutoFillJobNo(prefixText);
    }

    public static List<string> AutoFillJobNo(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select distinct JobNo from tblInwardEntry where " + "JobNo like '%'+ @Search + '%'";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> countryNames = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        countryNames.Add(sdr["JobNo"].ToString());
                    }
                }
                con.Close();
                return countryNames;
            }
        }
    }

    protected void txtJobno_TextChanged(object sender, EventArgs e)
    {
        GridView();
    }



    protected void btnrefresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("MasterGridview.aspx", false);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {

        GridView();



    }

    //Search Evolution Engineer Name list
    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetEvolutionEnginNameList(string prefixText, int count)
    {
        return AutoFillEvalutionList(prefixText);
    }

    public static List<string> AutoFillEvalutionList(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())

            {
                com.CommandText = "select distinct  EngiName FROM [dbo].tblInwardEntry AS TI Left JOIN [DB_EndeAuto].[dbo].tblTestingProduct AS TP ON TI.JobNo = TP.JobNo Left JOIN [EndeavourAuto].tblEstimationHdr AS TEH ON TEH.JobNo = TP.JobNo Left JOIN [EndeavourAuto].tblJobcardHdr AS JH ON JH.JobCardNo = TI.JobNo Left JOIN [DB_EndeAuto].[dbo].tbl_Quotation_Dtl AS QD ON QD.JobNo = TI.JobNo Left JOIN tbl_Quotation_Hdr AS QH ON QH.Quotation_no = QD.Quotation_no Left JOIN [EndeavourAuto].CustomerPO_Hdr AS CPO ON CPO.Quotationno = QH.Quotation_no Left JOIN tblInvoiceHdr AS TIH ON TIH.PoNo = CPO.Pono Left  JOIN tblInvoiceDtls AS TIDH ON TIH.Id = TIDH.InvoiceId Left JOIN tblOutwardEntry AS TOE ON TOE.JobNo = TIDH.JobNo where " + "EngiName like '%'+ @Search + '%' AND TI.isdeleted='0' ";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> countryNames = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        countryNames.Add(sdr["EngiName"].ToString());
                    }
                }
                con.Close();
                return countryNames;
            }
        }
    }

    protected void ddlservicetype_TextChanged(object sender, EventArgs e)
    {
        //string strConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //using (SqlConnection con = new SqlConnection(strConnString))
        //{
        //    using (SqlCommand cmd = new SqlCommand("[dbo].[SP_Masterlist]", con))
        //    {
        //        cmd.CommandType = CommandType.StoredProcedure;          
        //        cmd.Parameters.AddWithValue("@Action", "GetMasterlist");
        //        cmd.Parameters.Add(new SqlParameter("@FromDate", txtfromdate.Text));
        //        cmd.Parameters.Add(new SqlParameter("@Todate", txttodate.Text));
        //        cmd.Parameters.Add(new SqlParameter("@Customer", txtSearch.Text));
        //        // cmd.Parameters.Add(new SqlParameter("@EngineerName", txtEvolutionEnginName.Text));    
        //         cmd.Parameters.Add(new SqlParameter("@Servicetype", ddlservicetype.SelectedItem.Text));    
        //        using (SqlDataAdapter sda = new SqlDataAdapter())
        //        {
        //            cmd.Connection = con;
        //            sda.SelectCommand = cmd;
        //            using (DataSet ds = new DataSet())
        //            {
        //                DataTable dt = new DataTable();
        //                sda.Fill(dt);
        //                Gvmasterlist.DataSource = dt;
        //                Gvmasterlist.DataBind();

        //            }
        //        }
        //    }
        //}
        GridView();

    }

    protected void txtEvolutionEnginName_TextChanged(object sender, EventArgs e)
    {
        //if(txtEvolutionEnginName.Text!="")
        //{
        //    GetrecordDatewise();
        //}
        GridView();
    }

    //Search Repairing Engineer Name list
    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetRepairingEnginNameList(string prefixText, int count)
    {
        return AutoFillRepairingEngList(prefixText);
    }

    public static List<string> AutoFillRepairingEngList(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())

            {
                com.CommandText = "select distinct  EngineerName FROM [dbo].tblInwardEntry AS TI Left JOIN [DB_EndeAuto].[dbo].tblTestingProduct AS TP ON TI.JobNo = TP.JobNo Left JOIN [EndeavourAuto].tblEstimationHdr AS TEH ON TEH.JobNo = TP.JobNo Left JOIN [EndeavourAuto].tblJobcardHdr AS JH ON JH.JobCardNo = TI.JobNo Left JOIN [DB_EndeAuto].[dbo].tbl_Quotation_Dtl AS QD ON QD.JobNo = TI.JobNo Left JOIN tbl_Quotation_Hdr AS QH ON QH.Quotation_no = QD.Quotation_no Left JOIN [EndeavourAuto].CustomerPO_Hdr AS CPO ON CPO.Quotationno = QH.Quotation_no Left JOIN tblInvoiceHdr AS TIH ON TIH.PoNo = CPO.Pono Left  JOIN tblInvoiceDtls AS TIDH ON TIH.Id = TIDH.InvoiceId Left JOIN tblOutwardEntry AS TOE ON TOE.JobNo = TIDH.JobNo where " + "EngineerName like '%'+ @Search + '%' AND TI.isdeleted='0' ";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> countryNames = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        countryNames.Add(sdr["EngineerName"].ToString());
                    }
                }
                con.Close();
                return countryNames;
            }
        }
    }
    protected void txtRepairingENGName_TextChanged(object sender, EventArgs e)
    {
        //if (txtEvolutionEnginName.Text != "")
        //{
        //    GetrecordDatewise();
        //}
        GridView();
    }

    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridView();
    }

    protected void Gvmasterlist_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblRepeatedDate = (Label)e.Row.FindControl("lblRepeatedDate");
            Label lblReturnDate = (Label)e.Row.FindControl("lblReturnDate");

            if (lblRepeatedDate != null && lblReturnDate != null)
            {
                string repeatedDate = lblRepeatedDate.Text;
                string returnDate = lblReturnDate.Text;

                if (!string.IsNullOrEmpty(repeatedDate) && !string.IsNullOrEmpty(returnDate))
                {
                    if (repeatedDate == returnDate)
                    {
                        e.Row.BackColor = System.Drawing.Color.Yellow; // or any other color you prefer
                    }
                }
            }
        }
    }
}
