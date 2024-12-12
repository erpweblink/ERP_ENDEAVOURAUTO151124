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

public partial class Admin_InwardReport : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InwardGridView();
        }
    }

    void InwardGridView()
    {
        try
        {
            DataTable dt = new DataTable();

            con.Open();
           // SqlDataAdapter sad = new SqlDataAdapter("select [Id],[JobNo],[DateIn],[CustName],[MateName],[SrNo],[MateStatus],[TestBy],[ModelNo],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] FROM [tblInwardEntry] where isdeleted='0' ", con);
            SqlDataAdapter sad = new SqlDataAdapter("select [Id],[JobNo],[DateIn],[CustName],[MateName],[SrNo],[MateStatus],[TestBy],[ModelNo],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] FROM [tblInwardEntry] where isdeleted='0' ORDER BY DateIn DESC ", con);
            sad.Fill(dt);

            gv_Inward.EmptyDataText = "Not Records Found";
            gv_Inward.DataSource = dt;
            gv_Inward.DataBind();

            con.Close();

        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void gv_Inward_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //gv_Inward.PageIndex = e.NewPageIndex;
        //InwardGridView();

        if (ViewState["Record"] != null)
        {
            if (ViewState["Record"].ToString() == "Datewise")
            {
                gv_Inward.PageIndex = e.NewPageIndex;
                GetFromdateto();
            }
            if (ViewState["Record"].ToString() == "JobNo")
            {
                gv_Inward.PageIndex = e.NewPageIndex;
                GetJobno();

            }
            if (ViewState["Record"].ToString() == "CustName")
            {
                gv_Inward.PageIndex = e.NewPageIndex;
                GetCustname();

            }
            if (ViewState["Record"].ToString() == "Tested")
            {
                gv_Inward.PageIndex = e.NewPageIndex;
                GetTested();
            }
            if (ViewState["Record"].ToString() == "GetCustanddate")
            {
                gv_Inward.PageIndex = e.NewPageIndex;
                GetCustanddate();
            }
        }
        else
        {
            gv_Inward.PageIndex = e.NewPageIndex;
            InwardGridView();
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
                com.CommandText = "select DISTINCT JobNo from tblInwardEntry where " + "JobNo like @Search + '%' AND isdeleted='0'" +
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
                com.CommandText = "select DISTINCT CustName from tblInwardEntry where " + "CustName like @Search + '%' AND isdeleted='0' ";

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
                com.CommandText = "select DISTINCT MateStatus from tblInwardEntry where " + "MateStatus like @Search + '%' AND isdeleted='0' ";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> MateStatus = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        MateStatus.Add(sdr["MateStatus"].ToString());
                    }
                }
                con.Close();
                return MateStatus;
            }

        }
    }


    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetEngineerList(string prefixText, int count)
    {
        return AutoFillEngineerlist(prefixText);
    }

    public static List<string> AutoFillEngineerlist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT TestBy from tblInwardEntry where " + "TestBy like @Search + '%' AND isdeleted='0' ";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> TestBy = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        TestBy.Add(sdr["TestBy"].ToString());
                    }
                }
                con.Close();
                return TestBy;
            }
        }
    }

    protected void lnkBtnsearch_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtjob.Text) && string.IsNullOrEmpty(txtSearchCust.Text) && string.IsNullOrEmpty(txtDateSearchto.Text) && string.IsNullOrEmpty(txtDateSearchfrom.Text) && string.IsNullOrEmpty(txttested.Text))
        {
            InwardGridView();
        }
        //else if (!string.IsNullOrEmpty(txtsearchtestedby.Text) && !string.IsNullOrEmpty(txtjob.Text))
        //{
        //    DataTable dt = new DataTable();
        //    SqlDataAdapter sad = new SqlDataAdapter("select * from tblInwardEntry where TestBy='" + txtsearchtestedby.Text + "' AND JobNo='"+txtjob.Text+ "' AND isdeleted='0'", con);
        //    sad.Fill(dt);
        //    gv_Inward.EmptyDataText = "Record Not Found";
        //    gv_Inward.DataSource = dt;
        //    gv_Inward.DataBind();
        //}
        else if (!string.IsNullOrEmpty(txtSearchCust.Text) && !string.IsNullOrEmpty(txtjob.Text))
        {
            DataTable dt = new DataTable();
            SqlDataAdapter sad = new SqlDataAdapter("select * from tblInwardEntry where CustName='" + txtSearchCust.Text + "' AND JobNo='" + txtjob.Text + "' AND isdeleted='0'", con);
            sad.Fill(dt);
            gv_Inward.EmptyDataText = "Record Not Found";
            gv_Inward.DataSource = dt;
            gv_Inward.DataBind();
        }
        else if (!string.IsNullOrEmpty(txtDateSearchto.Text) && !string.IsNullOrEmpty(txtDateSearchfrom.Text) && !string.IsNullOrEmpty(txtSearchCust.Text))
        {
            ViewState["Excell"] = "Datewise";
            GetFromdateto();
            //DataTable dt = new DataTable();
            //SqlDataAdapter sad = new SqlDataAdapter("select * from tblInwardEntry where DateIn between '" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "' AND isdeleted='0'", con);
            //sad.Fill(dt);
            //gv_Inward.EmptyDataText = "Record Not Found";
            //gv_Inward.DataSource = dt;
            //gv_Inward.DataBind();
        }
        else if (string.IsNullOrEmpty(txtDateSearchto.Text) && !string.IsNullOrEmpty(txtDateSearchfrom.Text))
        {
            DataTable dt = new DataTable();
            SqlDataAdapter sad = new SqlDataAdapter("select * from tblInwardEntry where DateIn ='" + txtDateSearchfrom.Text + "' AND isdeleted='0'", con);
            sad.Fill(dt);
            gv_Inward.EmptyDataText = "Record Not Found";
            gv_Inward.DataSource = dt;
            gv_Inward.DataBind();
        }
        else if (!string.IsNullOrEmpty(txtDateSearchto.Text) && string.IsNullOrEmpty(txtDateSearchfrom.Text))
        {
            DataTable dt = new DataTable();
            SqlDataAdapter sad = new SqlDataAdapter("select * from tblInwardEntry where DateIn ='" + txtDateSearchto.Text + "' AND isdeleted='0'", con);
            sad.Fill(dt);
            gv_Inward.EmptyDataText = "Record Not Found";
            gv_Inward.DataSource = dt;
            gv_Inward.DataBind();
        }
        else if (!string.IsNullOrEmpty(txtjob.Text))
        {
            ViewState["Excell"] = "JobNo";
            GetJobno();

            //DataTable dt = new DataTable();
            //SqlDataAdapter sad = new SqlDataAdapter("select * from tblInwardEntry where JobNo='" + txtjob.Text + "'", con);
            //sad.Fill(dt);
            //gv_Inward.EmptyDataText = "Record Not Found";
            //gv_Inward.DataSource = dt;
            //gv_Inward.DataBind();
        }
        else if (!string.IsNullOrEmpty(txtSearchCust.Text))
        {
            ViewState["Excell"] = "CustName";
            GetCustname();


            //DataTable dt = new DataTable();
            //SqlDataAdapter sad = new SqlDataAdapter("select * from tblInwardEntry where CustName='" + txtSearchCust.Text + "' AND isdeleted='0'", con);
            //sad.Fill(dt);
            //gv_Inward.EmptyDataText = "Record Not Found";
            //gv_Inward.DataSource = dt;
            //gv_Inward.DataBind();
        }
        else if (!string.IsNullOrEmpty(txttested.Text))
        {
            ViewState["Excell"] = "Tested";
            GetTested();


            //DataTable dt = new DataTable();
            //SqlDataAdapter sad = new SqlDataAdapter("select * from tblInwardEntry where CustName='" + txtSearchCust.Text + "' AND isdeleted='0'", con);
            //sad.Fill(dt);
            //gv_Inward.EmptyDataText = "Record Not Found";
            //gv_Inward.DataSource = dt;
            //gv_Inward.DataBind();
        }
        else if (!string.IsNullOrEmpty(txtDateSearchfrom.Text) && !string.IsNullOrEmpty(txtDateSearchto.Text))
        {
            ViewState["Excell"] = "GetCustanddate";
            GetCustanddate();

        }

        //else if (!string.IsNullOrEmpty(txtsearchtestedby.Text))
        //{
        //    DataTable dt = new DataTable();
        //    SqlDataAdapter sad = new SqlDataAdapter("select * from tblInwardEntry where TestBy='" + txtsearchtestedby.Text + "' AND isdeleted='0'", con);
        //    sad.Fill(dt);
        //    gv_Inward.EmptyDataText = "Record Not Found";
        //    gv_Inward.DataSource = dt;
        //    gv_Inward.DataBind();
        //}
    }

    private void GetJobno()
    {
        ViewState["Record"] = "JobNo";
        ViewState["Excell"] = "JobNo";
        DataTable dt = new DataTable();
        con.Open();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblInwardEntry where JobNo='" + txtjob.Text + "'", con);
        sad.Fill(dt);
        gv_Inward.EmptyDataText = "Record Not Found";
        gv_Inward.DataSource = dt;
        gv_Inward.DataBind();
        con.Close();
    }

    private void GetCustname()
    {
        ViewState["Record"] = "CustName";
        ViewState["Excell"] = "CustName";
        con.Open();
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblInwardEntry where CustName='" + txtSearchCust.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        gv_Inward.EmptyDataText = "Record Not Found";
        gv_Inward.DataSource = dt;
        gv_Inward.DataBind();
        con.Close();
    }

    private void GetCustnameExcel()
    {
        ViewState["Record"] = "CustName";
        ViewState["Excell"] = "CustName";
        con.Open();
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblInwardEntry where CustName='" + txtSearchCust.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        sortedgv.EmptyDataText = "Record Not Found";
        sortedgv.DataSource = dt;
        sortedgv.DataBind();
        con.Close();
    }


    private void GetFromdateto()
    {
        ViewState["Record"] = "Datewise";
        ViewState["Excell"] = "Datewise";
        con.Open();
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblInwardEntry where DateIn between '" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "' AND CustName='" + txtSearchCust.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        gv_Inward.EmptyDataText = "Record Not Found";
        gv_Inward.DataSource = dt;
        gv_Inward.DataBind();
        con.Close();
    }


    private void GetFromdatetoExcel()
    {
        ViewState["Record"] = "Datewise";
        ViewState["Excell"] = "Datewise";
        con.Open();
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblInwardEntry where DateIn between '" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "' AND CustName='" + txtSearchCust.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        sortedgv.EmptyDataText = "Record Not Found";
        sortedgv.DataSource = dt;
        sortedgv.DataBind();
        con.Close();
    }



    private void GetJobnoExcel()
    {
        ViewState["Record"] = "JobNo";
        ViewState["Excell"] = "JobNo";
        DataTable dt = new DataTable();
        con.Open();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblInwardEntry where JobNo='" + txtjob.Text + "'", con);
        sad.Fill(dt);
        sortedgv.EmptyDataText = "Record Not Found";
        sortedgv.DataSource = dt;
        sortedgv.DataBind();
        con.Close();
    }

    private void GetTested()
    {
        ViewState["Record"] = "Tested";
        ViewState["Excell"] = "Tested";
        con.Open();
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblInwardEntry where MateStatus='" + txttested.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        gv_Inward.EmptyDataText = "Record Not Found";
        gv_Inward.DataSource = dt;
        gv_Inward.DataBind();
        con.Close();
    }


    private void GetCustanddate()
    {
        ViewState["Record"] = "GetCustanddate";
        ViewState["Excell"] = "GetCustanddate";
        con.Open();
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblInwardEntry where DateIn between '" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        gv_Inward.EmptyDataText = "Record Not Found";
        gv_Inward.DataSource = dt;
        gv_Inward.DataBind();
        con.Close();
    }



    private void GetTestedExcel()
    {
        ViewState["Record"] = "Tested";
        ViewState["Excell"] = "Tested";
        con.Open();
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblInwardEntry where MateStatus='" + txttested.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        sortedgv.EmptyDataText = "Record Not Found";
        sortedgv.DataSource = dt;
        sortedgv.DataBind();
        con.Close();
    }

    private void GetCustanddateExcel()
    {
        ViewState["Record"] = "GetCustanddate";
        ViewState["Excell"] = "GetCustanddate";
        con.Open();
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblInwardEntry where DateIn between '" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        sortedgv.EmptyDataText = "Record Not Found";
        sortedgv.DataSource = dt;
        sortedgv.DataBind();
        con.Close();
    }

    protected void lnkrefresh_Click(object sender, EventArgs e)
    {
        txtjob.Text = ""; txtSearchCust.Text = ""; txtDateSearchfrom.Text = ""; txtDateSearchto.Text = "";
        InwardGridView();  //txtsearchtestedby.Text = "";
    }

    protected void lnkshow_Click(object sender, EventArgs e)
    {
        int rowIndex = Convert.ToInt32(((sender as LinkButton).NamingContainer as GridViewRow).RowIndex);
        GridViewRow row = gv_Inward.Rows[rowIndex];

        lbljobshow.Text = (row.FindControl("lblJobno") as Label).Text;
        lblmatenameshow.Text = (row.FindControl("lblMatename") as Label).Text;
        lblSerialNoshow.Text = (row.FindControl("lblSriNo") as Label).Text;
        lblDateInhow.Text = (row.FindControl("lblDateIn") as Label).Text;
        lblcustomershow.Text = (row.FindControl("lblCustname") as Label).Text;
        //lbltestbyshow.Text = (row.FindControl("lblTestBy") as Label).Text;
        lblmodelNoshow.Text = (row.FindControl("lblModelNo") as Label).Text;
        lblmaterialstatusshow.Text = (row.FindControl("lblMateStatus") as Label).Text;
        modelprofile.Show();
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
            gv_Inward.Visible = false;
            string Method = ViewState["Excell"].ToString();

            if (Method == "JobNo")
            {
                GetJobnoExcel();
            }
            if (Method == "CustName")
            {
                GetCustnameExcel();
            }
            if (Method == "Datewise")
            {
                GetFromdatetoExcel();
            }
            if (Method == "Tested")
            {
                GetTestedExcel();
            }
            if (Method == "GetCustanddate")
            {
                GetCustanddateExcel();
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
        string FileName = "Inward_Report_List_" + DateTime.Now + ".xls";
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
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblInwardEntry where isdeleted='0'", con);
        sad.Fill(dt);
        sortedgv.EmptyDataText = "Not Records Found";
        sortedgv.DataSource = dt;
        sortedgv.DataBind();

        con.Close();
    }

}