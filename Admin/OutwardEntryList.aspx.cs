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

public partial class Reception_OutwardEntryList : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //this.txtDateSearch.Text = DateTime.Now.ToString("yyyy-MM-dd");
            //this.txtDateSearch.TextMode = TextBoxMode.Date;
            GridView();
            ddlsearchfltr();
        }
    }
    protected void ddlsearchfltr()
    {
        try
        {

            DataTable dt = new DataTable();

            con.Open();
            SqlDataAdapter sad = new SqlDataAdapter("select [Id],[Status] from RepairingStatusOutward", con);
            sad.Fill(dt);

            ddlsearch.DataValueField = "Id";
            ddlsearch.DataTextField = "Status";

            ddlsearch.DataSource = dt;
            ddlsearch.DataBind();

            con.Close();
        }
        catch (Exception)
        {

            throw;
        }
    }
    void GridView()
    {
        try
        {
            string role = Session["adminname"].ToString();
            if (role == "SubCustomer")
            {
                DataTable dt = new DataTable();

                con.Open();
                SqlDataAdapter sad = new SqlDataAdapter("select * from [tblOutwardEntry] where CustName='Schneider Electric India Pvt.Ltd.' AND isdeleted='0' ORDER BY [DateOut] Desc", con);
                sad.Fill(dt);

                gv_Outward.EmptyDataText = "Not Records Found";
                gv_Outward.DataSource = dt;
                gv_Outward.DataBind();
                con.Close();
            }
            else
            {
                DataTable dt = new DataTable();

                con.Open();
                SqlDataAdapter sad = new SqlDataAdapter("select * from [tblOutwardEntry] where isdeleted='0' ORDER BY [DateOut] Desc", con);
                sad.Fill(dt);

                gv_Outward.EmptyDataText = "Not Records Found";
                gv_Outward.DataSource = dt;
                gv_Outward.DataBind();

                con.Close();
            }
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
            string role = Session["adminname"].ToString();
            if (role == "SubCustomer")
            {
                DataTable dt = new DataTable();

                con.Open();
                SqlDataAdapter sad = new SqlDataAdapter("select * from [tblOutwardEntry] where CustName='Schneider Electric India Pvt.Ltd.' AND isdeleted='0' ORDER BY [DateOut] Desc", con);
                sad.Fill(dt);

                GridExportExcel.EmptyDataText = "Not Records Found";
                GridExportExcel.DataSource = dt;
                GridExportExcel.DataBind();
                con.Close();
            }
            else
            {
                DataTable dt = new DataTable();

                con.Open();
                SqlDataAdapter sad = new SqlDataAdapter("select * from [tblOutwardEntry] where isdeleted='0' ORDER BY [DateOut] Desc", con);
                sad.Fill(dt);

                GridExportExcel.EmptyDataText = "Not Records Found";
                GridExportExcel.DataSource = dt;
                GridExportExcel.DataBind();

                con.Close();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void gv_Outward_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "RowEdit")
        {
            Response.Redirect("OutwardEntry.aspx?Id=" + encrypt(e.CommandArgument.ToString()) + "");
        }

        if (e.CommandName == "RowDelete")
        {
            SqlCommand cmddelete = new SqlCommand("update tblOutwardEntry set isdeleted='1' where Id=@Id", con);
            cmddelete.Parameters.AddWithValue("@Id", Convert.ToInt32(e.CommandArgument.ToString()));
            cmddelete.Parameters.AddWithValue("@isdeleted", '1');
            con.Open();
            cmddelete.ExecuteNonQuery();
            con.Close();
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Delete Sucessfully');", true);

            //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Delete sucessfully!!');window.location ='CustomerList.aspx';", true);

            GridView();
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


    protected void btncreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("OutwardEntry.aspx");
    }

    protected void lnkBtnsearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(txtSearch.Text) && string.IsNullOrEmpty(txtDateSearch.Text) && string.IsNullOrEmpty(txtrepeatedno.Text) && string.IsNullOrEmpty(txtDateSearchfrom.Text) && string.IsNullOrEmpty(ddlsearch.SelectedItem.Text))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Please Search Record !!!');", true);
                GridView();

            }
            else
            {
                if (!string.IsNullOrEmpty(txtSearch.Text) && string.IsNullOrEmpty(txtDateSearch.Text) && string.IsNullOrEmpty(txtrepeatedno.Text) && string.IsNullOrEmpty(txtDateSearchfrom.Text))
                {
                    ViewState["Excell"] = "GetsortedCustomer";
                    GetsortedCustomer();

                    //DataTable dt = new DataTable();

                    //SqlDataAdapter sad = new SqlDataAdapter("select * from [tblOutwardEntry] where CustName='" + txtSearch.Text + "' AND isdeleted='0'", con);
                    //sad.Fill(dt);
                    //gv_Outward.EmptyDataText = "Not Records Found";
                    //gv_Outward.DataSource = dt;
                    //gv_Outward.DataBind();
                }
                else if (string.IsNullOrEmpty(txtSearch.Text) && !string.IsNullOrEmpty(txtDateSearch.Text) && string.IsNullOrEmpty(txtrepeatedno.Text))
                {
                    ViewState["Excell"] = "Getsortededatewise";
                    Getsortededatewise();
                    //DateTime date = Convert.ToDateTime(txtDateSearch.Text.ToString());
                    //DataTable dt = new DataTable();

                    //txtDateSearch.Text = date.ToString("yyyy-MM-dd");
                    //SqlDataAdapter sad = new SqlDataAdapter("select * from tblOutwardEntry where DateOut='" + txtDateSearch.Text + "' AND isdeleted='0'", con);
                    //sad.Fill(dt);
                    //gv_Outward.EmptyDataText = "Not Records Found";
                    //gv_Outward.DataSource = dt;
                    //gv_Outward.DataBind();
                }
                else if (string.IsNullOrEmpty(txtSearch.Text) && string.IsNullOrEmpty(txtDateSearch.Text) && !string.IsNullOrEmpty(txtrepeatedno.Text) && string.IsNullOrEmpty(txtDateSearchfrom.Text))
                {
                    ViewState["Excell"] = "GetsortedRepeatedNo";
                    GetsortedRepeatedNo();
                    ////DateTime date = Convert.ToDateTime(txtDateSearch.Text.ToString());
                    //DataTable dt = new DataTable();

                    ////txtDateSearch.Text = date.ToString("yyyy-MM-dd");
                    //SqlDataAdapter sad = new SqlDataAdapter("select * from tblOutwardEntry where repeatedNo='" + txtrepeatedno.Text + "' AND isdeleted='0'", con);
                    //sad.Fill(dt);
                    //gv_Outward.EmptyDataText = "Not Records Found";
                    //gv_Outward.DataSource = dt;
                    //gv_Outward.DataBind();
                }
                else if ((!string.IsNullOrEmpty(txtSearch.Text) && !string.IsNullOrEmpty(txtDateSearch.Text)))
                {
                    ViewState["Excell"] = "Getsortededatewise";
                    Getsortededatewise();
                    //DateTime date = Convert.ToDateTime(txtDateSearch.Text.ToString());
                    //DataTable dt = new DataTable();

                    //txtDateSearch.Text = date.ToString("yyyy-MM-dd");
                    //SqlDataAdapter sad = new SqlDataAdapter("select [Id],[JobNo],[DateOut],[CustName],[MateName],[ModelNo],[SerialNo],[JobWorkby],[Dispatchdate],[DateReturn],[ReturnRepair],Courier,[CreateBy],[CreatedDate],[UpdateBy],[UpdateDate] from [tblOutwardEntry] Where DateOut='" + txtDateSearch.Text + "' AND CustName='" + txtSearch.Text + "' AND isdeleted='0' ", con);
                    //sad.Fill(dt);
                    //gv_Outward.EmptyDataText = "Not Records Found";
                    //gv_Outward.DataSource = dt;
                    //gv_Outward.DataBind();
                }
                else if ((!string.IsNullOrEmpty(txtSearch.Text) && !string.IsNullOrEmpty(txtrepeatedno.Text)) && string.IsNullOrEmpty(txtDateSearchfrom.Text))
                {
                    ViewState["Excell"] = "Getsortedcustomerwierepetedno";
                    Getsortedcustomerwierepetedno();
                    ////DateTime date = Convert.ToDateTime(txtDateSearch.Text.ToString());
                    //DataTable dt = new DataTable();

                    ////txtDateSearch.Text = date.ToString("yyyy-MM-dd");
                    //SqlDataAdapter sad = new SqlDataAdapter("select * from [tblOutwardEntry] Where repeatedNo='" + txtrepeatedno.Text + "' AND CustName='" + txtSearch.Text + "' AND isdeleted='0' ", con);
                    //sad.Fill(dt);
                    //gv_Outward.EmptyDataText = "Not Records Found";
                    //gv_Outward.DataSource = dt;
                    //gv_Outward.DataBind();
                }
                else if ((!string.IsNullOrEmpty(txtDateSearchfrom.Text) && !string.IsNullOrEmpty(txtDateSearchto.Text) && string.IsNullOrEmpty(txtSearch.Text)) && string.IsNullOrEmpty(txtrepeatedno.Text) && string.IsNullOrEmpty(ddlsearch.Text))
                {
                    ViewState["Excell"] = "SortedDataFromdateToLastdate";
                    SortedDataFromdateToLastdate();
                }
                else if ((!string.IsNullOrEmpty(txtDateSearchfrom.Text) && !string.IsNullOrEmpty(txtDateSearchto.Text) && !string.IsNullOrEmpty(txtSearch.Text)))
                {
                    ViewState["Excell"] = "Getdatwisecustomer";
                    Getdatwisecustomer();
                }

                else if ((!string.IsNullOrEmpty(txtDateSearchfrom.Text) && !string.IsNullOrEmpty(txtDateSearchto.Text) && !string.IsNullOrEmpty(txtrepeatedno.Text)))
                {
                    ViewState["Excell"] = "GetDatewisesortedrepetedNO";
                    GetDatewisesortedrepetedNO();
                }
                else if ((!string.IsNullOrEmpty(txtDateSearchfrom.Text) && !string.IsNullOrEmpty(txtDateSearchto.Text) && !string.IsNullOrEmpty(ddlsearch.Text.Trim())))
                {
                    ViewState["Excell"] = "Getdatewisestatus";
                    Getdatewisestatus();
                }
                else if ( !string.IsNullOrEmpty(ddlsearch.Text.Trim()))
                {
                    ViewState["Excell"] = "sortedstatus";
                    Getsortedstatus();
                }
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
        return AutoFillCustomerlist(prefixText);
    }

    public static List<string> AutoFillCustomerlist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT CustName from tblOutwardEntry where " + "CustName like @Search + '%' AND isdeleted='0'";

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
    public static List<string> GetrepeatednoList(string prefixText, int count)
    {
        return AutoFillrepeatedlist(prefixText);
    }

    public static List<string> AutoFillrepeatedlist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT repeatedNo from tblOutwardEntry where " + "repeatedNo like @Search + '%' AND isdeleted='0'";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> repeatedNo = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        repeatedNo.Add(sdr["repeatedNo"].ToString());
                    }
                }
                con.Close();
                return repeatedNo;
            }

        }
    }
    protected void gv_Outward_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_Outward.PageIndex = e.NewPageIndex;
        GridView();
    }

    protected void lnkrefresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("OutwardEntryList.aspx");
    }

    //protected void ddlsearch_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (ddlsearch.SelectedItem.Text == "Select Status")
    //    {
    //        GridView();
    //    }
    //    else
    //    {


    //        SqlDataAdapter Da = new SqlDataAdapter("Select * From tblOutwardEntry WHERE isdeleted='0' AND ReturnRepair='" + ddlsearch.SelectedItem.Text + "'", con);
    //        DataTable Dt = new DataTable();
    //        Da.Fill(Dt);


    //        gv_Outward.DataSource = Dt;
    //        gv_Outward.DataBind();
    //        gv_Outward.EmptyDataText = "Not Records Found";

    //    }


        //if (ddlsearch.SelectedItem.Text == "Repaired")
        //{
        //    sad = new SqlDataAdapter("SELECT * FROM [tblOutwardEntry] where isdeleted='0' AND ReturnRepair='Repaired' ", con);
        //}
        //else if (ddlsearch.SelectedItem.Text == "Unrepaired")
        //{
        //    sad = new SqlDataAdapter("SELECT * FROM [tblOutwardEntry] where isdeleted='0' AND ReturnRepair='Unrepaired' ", con);
        //}
        //else if (ddlsearch.SelectedItem.Text == "completed")
        //{
        //    sad = new SqlDataAdapter("SELECT * FROM [tblOutwardEntry] where isdeleted='0' AND ReturnRepair='completed' ", con);
        //}
        //sad.Fill(Dt);
        //gv_Outward.EmptyDataText = "Not Records Found";
        //gv_Outward.DataSource = Dt;
        //gv_Outward.DataBind();
    

    protected void gv_Outward_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //Authorization
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            LinkButton lnkbtnEdit = e.Row.FindControl("lnkbtnEdit") as LinkButton;
            LinkButton lnkbtnDelete = e.Row.FindControl("lnkbtnDelete") as LinkButton;

            string Id = Session["Id"].ToString();

            DataTable Dtt = new DataTable();
            SqlDataAdapter Sdd = new SqlDataAdapter("Select * FROM tblUserRoleAuthorization where UserID = '" + Id + "' AND PageName = 'OutwardEntryList.aspx' AND PagesView = '1'", con);
            Sdd.Fill(Dtt);
            if (Dtt.Rows.Count > 0)
            {
                btncreate.Visible = false;
                //  gv_Outward.Columns[15].Visible = false;
                lnkbtnEdit.Visible = false;
                lnkbtnDelete.Visible = false;

            }
        }
    }

    //sorted Grid start
    protected void Gvsorted_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (ViewState["Record"].ToString() == "Customer")
        {
            Gvsorted.PageIndex = e.NewPageIndex;
            GetsortedCustomergrid();
        }
        if (ViewState["Record"].ToString() == "Date")
        {
            Gvsorted.PageIndex = e.NewPageIndex;
            Getsortededatewisegrid();
        }

        if (ViewState["Record"].ToString() == "Repeated")
        {
            Gvsorted.PageIndex = e.NewPageIndex;
            GetsortedRepeatedNogrid();
        }

        if (ViewState["Record"].ToString() == "Datewisecustomer")
        {
            Gvsorted.PageIndex = e.NewPageIndex;
            GetsortedDatwisecustomergrid();
        }

        if (ViewState["Record"].ToString() == "Customerwiserepeatedno")
        {
            Gvsorted.PageIndex = e.NewPageIndex;
            Getsortedcustomerwierepetednogrid();
        }

        if (ViewState["Record"].ToString() == "Datewise")
        {
            Gvsorted.PageIndex = e.NewPageIndex;
            SortedDataFromdateToLastdategrid();


        }
        if (ViewState["Record"].ToString() == "DatewiseRepeated")
        {
            Gvsorted.PageIndex = e.NewPageIndex;
            GetDatewisesortedrepetedNOgrid();
        }

        if (ViewState["Record"].ToString() == "Datewisestatus")
        {
            Gvsorted.PageIndex = e.NewPageIndex;
            GetDatewisestatusgrid();
        }
        if (ViewState["Record"].ToString() == "sortedstatus")
        {
            Gvsorted.PageIndex = e.NewPageIndex;
            Getsortedstatus();
        }

    }

    public void GetsortedCustomer()
    {
        gv_Outward.Visible = false;
        ViewState["Record"] = "Customer";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from [tblOutwardEntry] where CustName='" + txtSearch.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        Gvsorted.EmptyDataText = "Not Records Found";
        Gvsorted.DataSource = dt;
        Gvsorted.DataBind();
    }

    public void GetsortedCustomergrid()
    {
        gv_Outward.Visible = false;
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from [tblOutwardEntry] where CustName='" + txtSearch.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        Gvsorted.EmptyDataText = "Not Records Found";
        Gvsorted.DataSource = dt;
        Gvsorted.DataBind();
    }

    public void Getsortededatewise()
    {
        gv_Outward.Visible = false;
        ViewState["Record"] = "Date";
        DateTime date = Convert.ToDateTime(txtDateSearch.Text.ToString());
        DataTable dt = new DataTable();
        txtDateSearch.Text = date.ToString("yyyy-MM-dd");
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblOutwardEntry where DateOut='" + txtDateSearch.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        Gvsorted.EmptyDataText = "Not Records Found";
        Gvsorted.DataSource = dt;
        Gvsorted.DataBind();
    }

    public void Getsortededatewisegrid()
    {
        gv_Outward.Visible = false;
        DateTime date = Convert.ToDateTime(txtDateSearch.Text.ToString());
        DataTable dt = new DataTable();
        txtDateSearch.Text = date.ToString("yyyy-MM-dd");
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblOutwardEntry where DateOut='" + txtDateSearch.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        Gvsorted.EmptyDataText = "Not Records Found";
        Gvsorted.DataSource = dt;
        Gvsorted.DataBind();
    }

    public void GetsortedRepeatedNo()
    {
        gv_Outward.Visible = false;
        ViewState["Record"] = "Repeated";
        //DateTime date = Convert.ToDateTime(txtDateSearch.Text.ToString());
        DataTable dt = new DataTable();
        //txtDateSearch.Text = date.ToString("yyyy-MM-dd");
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblOutwardEntry where repeatedNo='" + txtrepeatedno.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        Gvsorted.EmptyDataText = "Not Records Found";
        Gvsorted.DataSource = dt;
        Gvsorted.DataBind();
    }

    public void GetsortedRepeatedNogrid()
    {
        gv_Outward.Visible = false;
        //DateTime date = Convert.ToDateTime(txtDateSearch.Text.ToString());
        DataTable dt = new DataTable();
        //txtDateSearch.Text = date.ToString("yyyy-MM-dd");
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblOutwardEntry where repeatedNo='" + txtrepeatedno.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        Gvsorted.EmptyDataText = "Not Records Found";
        Gvsorted.DataSource = dt;
        Gvsorted.DataBind();
    }

    public void GetsortedDatwisecustomer()
    {
        gv_Outward.Visible = false;
        ViewState["Record"] = "Datewisecustomer";
        DateTime date = Convert.ToDateTime(txtDateSearch.Text.ToString());
        DataTable dt = new DataTable();
        txtDateSearch.Text = date.ToString("yyyy-MM-dd");
        SqlDataAdapter sad = new SqlDataAdapter("select [Id],[JobNo],[DateOut],[CustName],[MateName],[ModelNo],[SerialNo],[JobWorkby],[Dispatchdate],[DateReturn],[ReturnRepair],Courier,[CreateBy],[CreatedDate],[UpdateBy],[UpdateDate] from [tblOutwardEntry] Where DateOut='" + txtDateSearch.Text + "' AND CustName='" + txtSearch.Text + "' AND isdeleted='0' ", con);
        sad.Fill(dt);
        Gvsorted.EmptyDataText = "Not Records Found";
        Gvsorted.DataSource = dt;
        Gvsorted.DataBind();
    }

    public void GetsortedDatwisecustomergrid()
    {
        gv_Outward.Visible = false;
        DateTime date = Convert.ToDateTime(txtDateSearch.Text.ToString());
        DataTable dt = new DataTable();
        txtDateSearch.Text = date.ToString("yyyy-MM-dd");
        SqlDataAdapter sad = new SqlDataAdapter("select [Id],[JobNo],[DateOut],[CustName],[MateName],[ModelNo],[SerialNo],[JobWorkby],[Dispatchdate],[DateReturn],[ReturnRepair],Courier,[CreateBy],[CreatedDate],[UpdateBy],[UpdateDate] from [tblOutwardEntry] Where DateOut='" + txtDateSearch.Text + "' AND CustName='" + txtSearch.Text + "' AND isdeleted='0' ", con);
        sad.Fill(dt);
        Gvsorted.EmptyDataText = "Not Records Found";
        Gvsorted.DataSource = dt;
        Gvsorted.DataBind();
    }

    public void Getsortedcustomerwierepetedno()
    {
        gv_Outward.Visible = false;
        ViewState["Record"] = "Customerwiserepeatedno";
        //DateTime date = Convert.ToDateTime(txtDateSearch.Text.ToString());
        DataTable dt = new DataTable();
        //txtDateSearch.Text = date.ToString("yyyy-MM-dd");
        SqlDataAdapter sad = new SqlDataAdapter("select * from [tblOutwardEntry] Where repeatedNo='" + txtrepeatedno.Text + "' AND CustName='" + txtSearch.Text + "' AND isdeleted='0' ", con);
        sad.Fill(dt);
        Gvsorted.EmptyDataText = "Not Records Found";
        Gvsorted.DataSource = dt;
        Gvsorted.DataBind();
    }

    public void Getsortedcustomerwierepetednogrid()
    {
        gv_Outward.Visible = false;
        //DateTime date = Convert.ToDateTime(txtDateSearch.Text.ToString());
        DataTable dt = new DataTable();
        //txtDateSearch.Text = date.ToString("yyyy-MM-dd");
        SqlDataAdapter sad = new SqlDataAdapter("select * from [tblOutwardEntry] Where repeatedNo='" + txtrepeatedno.Text + "' AND CustName='" + txtSearch.Text + "' AND isdeleted='0' ", con);
        sad.Fill(dt);
        Gvsorted.EmptyDataText = "Not Records Found";
        Gvsorted.DataSource = dt;
        Gvsorted.DataBind();
    }

    public void SortedDataFromdateToLastdate()
    {
        ViewState["Record"] = "Datewise";
        gv_Outward.Visible = false;
        DataTable dtt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select  *  from  [tblOutwardEntry]  where  DateOut between '" + txtDateSearchfrom.Text + "'  AND '" + txtDateSearchto.Text + "'", con);
        sad.Fill(dtt);
        Gvsorted.DataSource = dtt;
        Gvsorted.DataBind();
    }

    public void SortedDataFromdateToLastdategrid()
    {
        gv_Outward.Visible = false;
        DataTable dtt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select  *  from  [tblOutwardEntry]  where  DateOut between '" + txtDateSearchfrom.Text + "'  AND '" + txtDateSearchto.Text + "'", con);
        sad.Fill(dtt);
        Gvsorted.DataSource = dtt;
        Gvsorted.DataBind();
    }

    public void Getdatwisecustomer()
    {
        gv_Outward.Visible = false;
        ViewState["Record"] = "DatewiseCustomer";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM [tblOutwardEntry] WHERE DateOut BETWEEN '" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "' AND CustName = '" + txtSearch.Text + "' AND isdeleted = '0'", con);
        //SqlDataAdapter sad = new SqlDataAdapter("select * from [tblOutwardEntry] where CustName='" + txtSearch.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        Gvsorted.EmptyDataText = "Not Records Found";
        Gvsorted.DataSource = dt;
        Gvsorted.DataBind();
    }

    public void Getdatwisecustomergrid()
    {
        gv_Outward.Visible = false;
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM [tblOutwardEntry] WHERE DateOut BETWEEN '" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "' AND CustName = '" + txtSearch.Text + "' AND isdeleted = '0'", con);
        sad.Fill(dt);
        Gvsorted.EmptyDataText = "Not Records Found";
        Gvsorted.DataSource = dt;
        Gvsorted.DataBind();
    }

    public void GetDatewisesortedrepetedNO()
    {
        gv_Outward.Visible = false;
        ViewState["Record"] = "DatewiseRepeated";
        //DateTime date = Convert.ToDateTime(txtDateSearch.Text.ToString());
        DataTable dt = new DataTable();
        //txtDateSearch.Text = date.ToString("yyyy-MM-dd");
        SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM [tblOutwardEntry] WHERE DateOut BETWEEN '" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "' AND repeatedNo = '" + txtrepeatedno.Text + "' AND isdeleted = '0'", con);
        sad.Fill(dt);
        Gvsorted.EmptyDataText = "Not Records Found";
        Gvsorted.DataSource = dt;
        Gvsorted.DataBind();
    }

    public void GetDatewisesortedrepetedNOgrid()
    {
        gv_Outward.Visible = false;
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM [tblOutwardEntry] WHERE DateOut BETWEEN '" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "' AND repeatedNo = '" + txtrepeatedno.Text + "' AND isdeleted = '0'", con);
        sad.Fill(dt);
        Gvsorted.EmptyDataText = "Not Records Found";
        Gvsorted.DataSource = dt;
        Gvsorted.DataBind();
    }

    public void Getdatewisestatus()
    {
        ViewState["ID"] = ddlsearch.Text.Trim();
        ddlsearchflt1();
        string Status = ViewState["status"].ToString();
        gv_Outward.Visible = false;
        ViewState["Record"] = "Datewisestatus";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM [tblOutwardEntry] WHERE DateOut BETWEEN '" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "' AND ReturnRepair = '" + Status + "' AND isdeleted = '0'", con);
        sad.Fill(dt);
        Gvsorted.EmptyDataText = "Not Records Found";
        Gvsorted.DataSource = dt;
        Gvsorted.DataBind();
    }

    public void GetDatewisestatusgrid()
    {
        gv_Outward.Visible = false;
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM [tblOutwardEntry] WHERE DateOut BETWEEN '" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "' AND ReturnRepair = '" + ddlsearch.Text + "' AND isdeleted = '0'", con);
        sad.Fill(dt);
        Gvsorted.EmptyDataText = "Not Records Found";
        Gvsorted.DataSource = dt;
        Gvsorted.DataBind();
    }



    protected void ddlsearchflt1()
    {
        try
        {
            string ID = ViewState["ID"].ToString();

            DataTable dt = new DataTable();
            con.Open();
            SqlDataAdapter sad = new SqlDataAdapter("SELECT [Id], [Status] FROM RepairingStatusOutward WHERE Id = " + ID, con);
            sad.Fill(dt);

            ddlsearch.DataValueField = "Id";
            ddlsearch.DataTextField = "Status";

            ddlsearch.DataSource = dt;
            ddlsearch.DataBind();
            ViewState["status"] = dt.Rows[0]["Status"].ToString();
            con.Close();
        }
        catch (Exception ex)
        {

            throw;
        }
    }


    public void Getsortedstatus()
    {
        if (ddlsearch.SelectedItem.Text != "Select Status")
        {
            gv_Outward.Visible = false;
            ViewState["Record"] = "sortedstatus";
            SqlDataAdapter Da = new SqlDataAdapter("Select * From tblOutwardEntry WHERE isdeleted='0' AND ReturnRepair='" + ddlsearch.SelectedItem.Text + "'", con);
            DataTable Dt = new DataTable();
            Da.Fill(Dt);
            Gvsorted.DataSource = Dt;
            Gvsorted.DataBind();
            Gvsorted.EmptyDataText = "Not Records Found";

        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        //required to avoid the run time error "  
        //Control 'GridView1' of type 'Grid View' must be placed inside a form tag with runat=server."  
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        ExportGridToExcel();
    }

    private void ExportGridToExcel()
    {

        if (ViewState["Excell"] != null)
        {
            string Method = ViewState["Excell"].ToString();
            if (Method == "GetsortedCustomer")
            {
                GetsortedCustomerForExcell();
            }
            if (Method == "Getsortededatewise")
            {
                GetsortededatewiseForExcell();
            }

            if (Method == "GetsortedRepeatedNo")
            {
                GetsortedRepeatedNoForExcell();
            }
            if (Method == "Getsortededatewise")
            {
                GetsortededatewiseFoExecll();
            }
            if (Method == "SortedDataFromdateToLastdate")
            {
                SortedDataFromdateToLastdateForExcell();
            }
            if (Method == "Getdatwisecustomer")
            {
                GetdatwisecustomerForExcell();
            }
            if (Method == "GetDatewisesortedrepetedNO")
            {
                GetDatewisesortedrepetedNOForExcell();
            }
            if (Method == "Getdatewisestatus")
            {
                Getdatewisestatus();
            }
            if (Method == "sortedstatus")
            {
                GetsortedstatusForExcell();
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
        string FileName = "OutwardEntry_List_" + DateTime.Now + ".xls";
        StringWriter strwritter = new StringWriter();
        HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
        GridExportExcel.GridLines = GridLines.Both;
        GridExportExcel.HeaderStyle.Font.Bold = true;
        GridExportExcel.RenderControl(htmltextwrtter);
        Response.Write(strwritter.ToString());
        Response.End();
    }




    public void GetsortedCustomerForExcell()
    {
        gv_Outward.Visible = false;
        ViewState["Record"] = "Customer";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from [tblOutwardEntry] where CustName='" + txtSearch.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        GridExportExcel.EmptyDataText = "Not Records Found";
        GridExportExcel.DataSource = dt;
        GridExportExcel.DataBind();
    }

    public void GetsortededatewiseForExcell()
    {
        gv_Outward.Visible = false;
        ViewState["Record"] = "Date";
        DateTime date = Convert.ToDateTime(txtDateSearch.Text.ToString());
        DataTable dt = new DataTable();
        txtDateSearch.Text = date.ToString("yyyy-MM-dd");
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblOutwardEntry where DateOut='" + txtDateSearch.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        GridExportExcel.EmptyDataText = "Not Records Found";
        GridExportExcel.DataSource = dt;
        GridExportExcel.DataBind();
    }

    public void GetsortedRepeatedNoForExcell()
    {
        gv_Outward.Visible = false;
        ViewState["Record"] = "Repeated";
        //DateTime date = Convert.ToDateTime(txtDateSearch.Text.ToString());
        DataTable dt = new DataTable();
        //txtDateSearch.Text = date.ToString("yyyy-MM-dd");
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblOutwardEntry where repeatedNo='" + txtrepeatedno.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        GridExportExcel.EmptyDataText = "Not Records Found";
        GridExportExcel.DataSource = dt;
        GridExportExcel.DataBind();
    }

    public void GetsortededatewiseFoExecll()
    {
        gv_Outward.Visible = false;
        ViewState["Record"] = "Date";
        DateTime date = Convert.ToDateTime(txtDateSearch.Text.ToString());
        DataTable dt = new DataTable();
        txtDateSearch.Text = date.ToString("yyyy-MM-dd");
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblOutwardEntry where DateOut='" + txtDateSearch.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        GridExportExcel.EmptyDataText = "Not Records Found";
        GridExportExcel.DataSource = dt;
        GridExportExcel.DataBind();
    }

    public void Getsortedcustomerwierepetednoforexcell()
    {
        gv_Outward.Visible = false;
        ViewState["Record"] = "Customerwiserepeatedno";
        //DateTime date = Convert.ToDateTime(txtDateSearch.Text.ToString());
        DataTable dt = new DataTable();
        //txtDateSearch.Text = date.ToString("yyyy-MM-dd");
        SqlDataAdapter sad = new SqlDataAdapter("select * from [tblOutwardEntry] Where repeatedNo='" + txtrepeatedno.Text + "' AND CustName='" + txtSearch.Text + "' AND isdeleted='0' ", con);
        sad.Fill(dt);
        GridExportExcel.EmptyDataText = "Not Records Found";
        GridExportExcel.DataSource = dt;
        GridExportExcel.DataBind();
    }

    public void SortedDataFromdateToLastdateForExcell()
    {
        ViewState["Record"] = "Datewise";
        gv_Outward.Visible = false;
        DataTable dtt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select  *  from  [tblOutwardEntry]  where  DateOut between '" + txtDateSearchfrom.Text + "'  AND '" + txtDateSearchto.Text + "'", con);
        sad.Fill(dtt);
        GridExportExcel.DataSource = dtt;
        GridExportExcel.DataBind();
    }

    public void GetdatwisecustomerForExcell()
    {
        gv_Outward.Visible = false;
        ViewState["Record"] = "DatewiseCustomer";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM [tblOutwardEntry] WHERE DateOut BETWEEN '" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "' AND CustName = '" + txtSearch.Text + "' AND isdeleted = '0'", con);
        //SqlDataAdapter sad = new SqlDataAdapter("select * from [tblOutwardEntry] where CustName='" + txtSearch.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        GridExportExcel.EmptyDataText = "Not Records Found";
        GridExportExcel.DataSource = dt;
        GridExportExcel.DataBind();
    }

    public void GetDatewisesortedrepetedNOForExcell()
    {
        gv_Outward.Visible = false;
        ViewState["Record"] = "DatewiseRepeated";
        //DateTime date = Convert.ToDateTime(txtDateSearch.Text.ToString());
        DataTable dt = new DataTable();
        //txtDateSearch.Text = date.ToString("yyyy-MM-dd");
        SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM [tblOutwardEntry] WHERE DateOut BETWEEN '" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "' AND repeatedNo = '" + txtrepeatedno.Text + "' AND isdeleted = '0'", con);
        sad.Fill(dt);
        GridExportExcel.EmptyDataText = "Not Records Found";
        GridExportExcel.DataSource = dt;
        GridExportExcel.DataBind();
    }


    public void GetdatewisestatusForExcell()
    {
        ViewState["ID"] = ddlsearch.Text.Trim();
        ddlsearchflt1();
        string Status = ViewState["status"].ToString();
        gv_Outward.Visible = false;
        ViewState["Record"] = "Datewisestatus";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM [tblOutwardEntry] WHERE DateOut BETWEEN '" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "' AND ReturnRepair = '" + Status + "' AND isdeleted = '0'", con);
        sad.Fill(dt);
        GridExportExcel.EmptyDataText = "Not Records Found";
        GridExportExcel.DataSource = dt;
        GridExportExcel.DataBind();
    }


    public void GetsortedstatusForExcell()
    {
        if (ddlsearch.SelectedItem.Text != "Select Status")
        {
            gv_Outward.Visible = false;
            ViewState["Record"] = "sortedstatus";
            SqlDataAdapter Da = new SqlDataAdapter("Select * From tblOutwardEntry WHERE isdeleted='0' AND ReturnRepair='" + ddlsearch.SelectedItem.Text + "'", con);
            DataTable Dt = new DataTable();
            Da.Fill(Dt);
            GridExportExcel.DataSource = Dt;
            GridExportExcel.DataBind();
            GridExportExcel.EmptyDataText = "Not Records Found";

        }
    }

}
