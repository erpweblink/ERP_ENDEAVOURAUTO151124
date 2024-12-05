using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Text;
using System.Security.Cryptography;
using System.IO;

public partial class Admin_JOBcardList : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GridRecord();
        }
    }

    //protected void GridRecord()
    //{
    //    try
    //    {
    //        DataTable dt = new DataTable();
    //        con.Open();
    //        //SqlDataAdapter sad = new SqlDataAdapter("SELECT  [Id],[JobCardNo],[RepeatedNo],[ItemDesc],[ModelNo],[InwardDate],[outwardDate],[CreatedBy],[CreatedDate],[updatedby],[Updateddate],[isdeleted],status,EngineerName from tblJobcardHdr where isdeleted='0' ORDER BY CreatedDate", con);
    //        SqlDataAdapter sad = new SqlDataAdapter("SELECT  [Id],[JobCardNo],[RepeatedNo],[ItemDesc],[ModelNo],[InwardDate],[outwardDate],[CreatedBy],[CreatedDate],[updatedby],[Updateddate],[isdeleted],status,EngineerName,EngineerName2,EngineerName3,EngineerName4,Reparingdate from tblJobcardHdr where isdeleted='0' ORDER BY CreatedDate Desc", con);
    //        sad.Fill(dt);
    //        gv_JOBCARD.EmptyDataText = "Not Records Found";
    //        gv_JOBCARD.DataSource = dt;
    //        gv_JOBCARD.DataBind();

    //        con.Close();
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    protected void GridRecord()
    {
        try
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlDataAdapter sad = new SqlDataAdapter("SELECT [Id],[JobCardNo],[RepeatedNo],[ItemDesc],[ModelNo],[InwardDate],[outwardDate],[CreatedBy],[CreatedDate],[updatedby],[Updateddate],[isdeleted],status,EngineerName,EngineerName2,EngineerName3,EngineerName4,Reparingdate FROM tblJobcardHdr WHERE isdeleted='0' ORDER BY CreatedDate DESC", con);
            sad.Fill(dt);
            foreach (DataRow row in dt.Rows)
            {
                if (row["Reparingdate"] != DBNull.Value && Convert.ToDateTime(row["Reparingdate"]).ToString("yyyy-MM-dd") == "1900-01-01")
                {
                    row["Reparingdate"] = DBNull.Value; 
                }
            }

            gv_JOBCARD.EmptyDataText = "No Records Found";
            gv_JOBCARD.DataSource = dt;
            gv_JOBCARD.DataBind();

            con.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    private void GridExport()
    {
        try
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlDataAdapter sad = new SqlDataAdapter("SELECT  [Id],[JobCardNo],[RepeatedNo],[ItemDesc],[ModelNo],[InwardDate],[Reparingdate],[outwardDate],[CreatedBy],[CreatedDate],[updatedby],[Updateddate],[isdeleted],status,EngineerName from tblJobcardHdr where isdeleted='0' ORDER BY CreatedDate", con);
            sad.Fill(dt);
            foreach (DataRow row in dt.Rows)
            {
                if (row["Reparingdate"] != DBNull.Value && Convert.ToDateTime(row["Reparingdate"]).ToString("yyyy-MM-dd") == "1900-01-01")
                {
                    row["Reparingdate"] = DBNull.Value;
                }
            }
            GridExportExcel.EmptyDataText = "Not Records Found";
            GridExportExcel.DataSource = dt;
            GridExportExcel.DataBind();

            con.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void gv_JOBCARD_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_JOBCARD.PageIndex = e.NewPageIndex;
        GridRecord();
    }


    protected void gv_JOBCARD_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "RowEdit")
        {
            Response.Redirect("JOBCard.aspx?JobCardNo=" + encrypt(e.CommandArgument.ToString()) + "");
        }
        if (e.CommandName == "RowDelete")
        {
            SqlCommand cmddelete = new SqlCommand("update tblJobcardHdr set isdeleted='1' where Id=@Id", con);
            cmddelete.Parameters.AddWithValue("@Id", Convert.ToInt32(e.CommandArgument.ToString()));
            cmddelete.Parameters.AddWithValue("@isdeleted", '1');
            con.Open();
            cmddelete.ExecuteNonQuery();
            con.Close();
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Deleted Sucessfully');", true);
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
        Response.Redirect("JOBCard.aspx");
    }

    protected void lnkBtnsearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(txtjob.Text) && (string.IsNullOrEmpty(txtitemdesc.Text)) && (string.IsNullOrEmpty(txtrepetedno.Text)) && (string.IsNullOrEmpty(txtengineername.Text)) && (string.IsNullOrEmpty(txtstatus.Text)) && (string.IsNullOrEmpty(txt_form_podate_search.Text)) && (string.IsNullOrEmpty(txt_to_podate_search.Text)))
            {
                GridRecord();
            }
            //Job No
            else if (!string.IsNullOrEmpty(txtjob.Text) && (string.IsNullOrEmpty(txtitemdesc.Text)))
            {
                ViewState["Excell"] = "GetsortedJobno";
                GetsortedJobno();

                //DataTable dt = new DataTable();
                //SqlDataAdapter sad = new SqlDataAdapter("select * from tblJobcardHdr where JobCardNo='" + txtjob.Text + "' AND isdeleted='0'", con);
                //sad.Fill(dt);
                //gv_JOBCARD.DataSource = dt;
                //gv_JOBCARD.DataBind();
            }
            //repeted no
            else if (!string.IsNullOrEmpty(txtrepetedno.Text) && string.IsNullOrEmpty(txt_form_podate_search.Text))
            /*if (!string.IsNullOrEmpty(txtrepetedno.Text) && (string.IsNullOrEmpty(txt_form_podate_search.Text)))*/
            {
                ViewState["Excell"] = "Getsorepetedno";
                Getsorepetedno();

                //DataTable dt = new DataTable();
                //SqlDataAdapter sad = new SqlDataAdapter("select * from tblJobcardHdr where RepeatedNo='" + txtrepetedno.Text + "' AND isdeleted='0'", con);
                //sad.Fill(dt);
                //gv_JOBCARD.DataSource = dt;
                //gv_JOBCARD.DataBind();
            }
            //Engineer Name
            else if (!string.IsNullOrEmpty(txtengineername.Text) && string.IsNullOrEmpty(txt_form_podate_search.Text))
            {
                ViewState["Excell"] = "GetsortedData";
                GetsortedData();
                //    string searchCriteria = txtengineername.Text;

                //    DataTable dt = new DataTable();
                //    SqlDataAdapter sad = new SqlDataAdapter("select * from tblJobcardHdr where EngineerName='" + txtengineername.Text + "'", con);
                //    sad.Fill(dt);
                //    gv_JOBCARD.DataSource = dt;
                //    gv_JOBCARD.DataBind();
            }

            //Status 
            else if (!string.IsNullOrEmpty(txtstatus.Text) && string.IsNullOrEmpty(txt_form_podate_search.Text))
            {
                ViewState["Excell"] = "GetSortedstatus";
                GetSortedstatus();
                //DataTable dt = new DataTable();
                //SqlDataAdapter sad = new SqlDataAdapter("select * from tblJobcardHdr where status='" + txtstatus.Text + "'", con);
                //sad.Fill(dt);
                //gv_JOBCARD.DataSource = dt;
                //gv_JOBCARD.DataBind();
            }

            else if (string.IsNullOrEmpty(txtjob.Text) && (!string.IsNullOrEmpty(txtitemdesc.Text)) && (string.IsNullOrEmpty(txt_form_podate_search.Text)))
            {
                ViewState["Excell"] = "Getsorteddiscriptions";
                Getsorteddiscriptions();
                //DataTable dt = new DataTable();
                //SqlDataAdapter sad = new SqlDataAdapter("select * from tblJobcardHdr where ItemDesc='" + txtitemdesc.Text + "' AND isdeleted='0'", con);
                //sad.Fill(dt);
                //gv_JOBCARD.DataSource = dt;
                //gv_JOBCARD.DataBind();
            }
            else if (!string.IsNullOrEmpty(txtjob.Text) && (!string.IsNullOrEmpty(txtitemdesc.Text)))
            {
                ViewState["Excell"] = "Getsortdatadeswithjob";
                Getsortdatadeswithjob();
                //DataTable dt = new DataTable();
                //SqlDataAdapter sad = new SqlDataAdapter("select * from tblJobcardHdr where ItemDesc='" + txtitemdesc.Text + "' AND JobCardNo='" + txtjob.Text + "' AND isdeleted='0'", con);
                //sad.Fill(dt);
                //gv_JOBCARD.DataSource = dt;
                //gv_JOBCARD.DataBind();
            }
            //From Date To ToDate
            else if (!string.IsNullOrEmpty(txt_form_podate_search.Text) && !string.IsNullOrEmpty(txt_to_podate_search.Text) && string.IsNullOrEmpty(txtrepetedno.Text) && string.IsNullOrEmpty(txtjob.Text) && string.IsNullOrEmpty(txtengineername.Text) && string.IsNullOrEmpty(txtitemdesc.Text) && string.IsNullOrEmpty(txtstatus.Text))
            {
                ViewState["Excell"] = "SortedDataFromdateToLastdate";
                SortedDataFromdateToLastdate();
            }

            else if (!string.IsNullOrEmpty(txt_form_podate_search.Text) && !string.IsNullOrEmpty(txt_to_podate_search.Text) && !string.IsNullOrEmpty(txtjob.Text))
            {
                ViewState["Excell"] = "DatwiseJobNo";
                DatwiseJobNo();
            }
            else if (!string.IsNullOrEmpty(txt_form_podate_search.Text) && !string.IsNullOrEmpty(txt_to_podate_search.Text) && !string.IsNullOrEmpty(txtengineername.Text) && !string.IsNullOrEmpty(txtstatus.Text))
            {
                ViewState["Excell"] = "GetDatweiseEngineer";
                GetDatweiseEngineer();
            }

            else if (!string.IsNullOrEmpty(txt_form_podate_search.Text) && !string.IsNullOrEmpty(txt_to_podate_search.Text) && !string.IsNullOrEmpty(txtitemdesc.Text))
            {
                ViewState["Excell"] = "GetDatewisediscriptions";
                GetDatewisediscriptions();
            }

            else if (!string.IsNullOrEmpty(txt_form_podate_search.Text) && !string.IsNullOrEmpty(txt_to_podate_search.Text) && !string.IsNullOrEmpty(txtstatus.Text))
            {
                ViewState["Excell"] = "GetDatewisestatus";
                GetDatewisestatus();
            }

            else if (!string.IsNullOrEmpty(txtrepetedno.Text) && !string.IsNullOrEmpty(txt_form_podate_search.Text))
            {
                ViewState["Excell"] = "Getdatwiserepetedno";
                Getdatwiserepetedno();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void lnkrefresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("JOBcardList.aspx");
    }
    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetjobList(string prefixText, int count)
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
                com.CommandText = "select DISTINCT JobCardNo from tblJobcardHdr where " + "JobCardNo like @Search + '%' AND isdeleted='0'";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> JobCardNo = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        JobCardNo.Add(sdr["JobCardNo"].ToString());
                    }
                }
                con.Close();
                return JobCardNo;
            }

        }
    }

    //Get Repetead no list
    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetrepeteList(string prefixText, int count)
    {
        return AutoFillrepetedlist(prefixText);
    }

    public static List<string> AutoFillrepetedlist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT RepeatedNo from tblJobcardHdr where " + "RepeatedNo like @Search + '%' AND isdeleted='0'";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> RepeatedNo = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        RepeatedNo.Add(sdr["RepeatedNo"].ToString());
                    }
                }
                con.Close();
                return RepeatedNo;
            }

        }
    }

    //Get Engineer Name list
    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetEnginameList(string prefixText, int count)
    {
        return AutoFillEnginamelist(prefixText);
    }

    public static List<string> AutoFillEnginamelist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT EngineerName from tblJobcardHdr where " + "EngineerName like @Search + '%' AND isdeleted='0'";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> EngineerName = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        EngineerName.Add(sdr["EngineerName"].ToString());
                    }
                }
                con.Close();
                return EngineerName;
            }

        }
    }

    //Get Status list
    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetstatusList(string prefixText, int count)
    {
        return AutoFillstatusList(prefixText);
    }

    public static List<string> AutoFillstatusList(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT status from tblJobcardHdr where " + "status like @Search + '%' AND isdeleted='0'";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> status = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        status.Add(sdr["status"].ToString());
                    }
                }
                con.Close();
                return status;
            }
        }
    }

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> Getitemdesc(string prefixText, int count)
    {
        return AutoFillitemlist(prefixText);
    }

    public static List<string> AutoFillitemlist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT ItemDesc from tblJobcardHdr where " + "ItemDesc like @Search + '%' AND isdeleted='0'";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> ItemDesc = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        ItemDesc.Add(sdr["ItemDesc"].ToString());
                    }
                }
                con.Close();
                return ItemDesc;
            }

        }
    }

    protected void gv_JOBCARD_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (Session["name"].ToString() == "Admin")
        {
            gv_JOBCARD.Columns[9].Visible = true;
            // cust.Visible = true;
            // cust1.Visible = true;
        }
        else
        {
            // cust.Visible = false;
            //cust1.Visible = false;
            gv_JOBCARD.Columns[9].Visible = false;
        }
    }
    //Er section
    public void GetsortedData()
    {
        ViewState["Record"] = "ER";
          gv_JOBCARD.Visible = false;
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblJobcardHdr where EngineerName='" + txtengineername.Text + "'", con);
        sad.Fill(dt);
        foreach (DataRow row in dt.Rows)
        {
            if (row["Reparingdate"] != DBNull.Value && Convert.ToDateTime(row["Reparingdate"]).ToString("yyyy-MM-dd") == "1900-01-01")
            {
                row["Reparingdate"] = DBNull.Value;
            }
        }
        sortedgv_JOBCARD.DataSource = dt;
        sortedgv_JOBCARD.DataBind();
    }

    protected void GridRecord1()
    {
        try
        {
            DataTable dt = new DataTable();
            con.Open();
            // SqlDataAdapter sad = new SqlDataAdapter("SELECT  [Id],[JobCardNo],[RepeatedNo],[ItemDesc],[ModelNo],[InwardDate],[outwardDate],[CreatedBy],[CreatedDate],[updatedby],[Updateddate],[isdeleted],status,EngineerName from tblJobcardHdr where isdeleted='0' ORDER BY CreatedDate", con);
            SqlDataAdapter sad = new SqlDataAdapter("select * from tblJobcardHdr where EngineerName='" + txtengineername.Text + "'", con);
            sad.Fill(dt);
            sortedgv_JOBCARD.EmptyDataText = "Not Records Found";
            sortedgv_JOBCARD.DataSource = dt;
            sortedgv_JOBCARD.DataBind();

            con.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    //End Er
    //Job no section
    public void GetsortedJobno()
    {
        ViewState["Record"] = "jobno";

        gv_JOBCARD.Visible = false;
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblJobcardHdr where JobCardNo='" + txtjob.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        foreach (DataRow row in dt.Rows)
        {
            if (row["Reparingdate"] != DBNull.Value && Convert.ToDateTime(row["Reparingdate"]).ToString("yyyy-MM-dd") == "1900-01-01")
            {
                row["Reparingdate"] = DBNull.Value;
            }
        }
        sortedgv_JOBCARD.DataSource = dt;
        sortedgv_JOBCARD.DataBind();
    }

    public void sortedjobGrid()
    {
        sortedgv_JOBCARD.Visible = true;
        try
        {
            gv_JOBCARD.Visible = false;
            DataTable dt = new DataTable();
            con.Open();
            SqlDataAdapter sad = new SqlDataAdapter("select * from tblJobcardHdr where JobCardNo='" + txtjob.Text + "' AND isdeleted='0'", con);
            sad.Fill(dt);
            foreach (DataRow row in dt.Rows)
            {
                if (row["Reparingdate"] != DBNull.Value && Convert.ToDateTime(row["Reparingdate"]).ToString("yyyy-MM-dd") == "1900-01-01")
                {
                    row["Reparingdate"] = DBNull.Value;
                }
            }
            sortedgv_JOBCARD.EmptyDataText = "Not Records Found";
            sortedgv_JOBCARD.DataSource = dt;
            sortedgv_JOBCARD.DataBind();

            con.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    //End

    //repeted No
    public void Getsorepetedno()
    {
        ViewState["Record"] = "repeted";

        gv_JOBCARD.Visible = false;
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblJobcardHdr where RepeatedNo='" + txtrepetedno.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        foreach (DataRow row in dt.Rows)
        {
            if (row["Reparingdate"] != DBNull.Value && Convert.ToDateTime(row["Reparingdate"]).ToString("yyyy-MM-dd") == "1900-01-01")
            {
                row["Reparingdate"] = DBNull.Value;
            }
        }
        sortedgv_JOBCARD.DataSource = dt;
        sortedgv_JOBCARD.DataBind();
    }

    public void Sortedrepetednogrid()
    {

        sortedgv_JOBCARD.Visible = true;
        gv_JOBCARD.Visible = false;
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblJobcardHdr where RepeatedNo='" + txtrepetedno.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        foreach (DataRow row in dt.Rows)
        {
            if (row["Reparingdate"] != DBNull.Value && Convert.ToDateTime(row["Reparingdate"]).ToString("yyyy-MM-dd") == "1900-01-01")
            {
                row["Reparingdate"] = DBNull.Value;
            }
        }
        sortedgv_JOBCARD.DataSource = dt;
        sortedgv_JOBCARD.DataBind();
    }

    //status 

    public void GetSortedstatus()
    {
        gv_JOBCARD.Visible = false;
        ViewState["Record"] = "status";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblJobcardHdr where status='" + txtstatus.Text + "'", con);
        sad.Fill(dt);
        foreach (DataRow row in dt.Rows)
        {
            if (row["Reparingdate"] != DBNull.Value && Convert.ToDateTime(row["Reparingdate"]).ToString("yyyy-MM-dd") == "1900-01-01")
            {
                row["Reparingdate"] = DBNull.Value;
            }
        }
        sortedgv_JOBCARD.DataSource = dt;
        sortedgv_JOBCARD.DataBind();
    }

    public void GetSortedstatusGrid()
    {
        gv_JOBCARD.Visible = false;
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblJobcardHdr where status='" + txtstatus.Text + "'", con);
        sad.Fill(dt);
        foreach (DataRow row in dt.Rows)
        {
            if (row["Reparingdate"] != DBNull.Value && Convert.ToDateTime(row["Reparingdate"]).ToString("yyyy-MM-dd") == "1900-01-01")
            {
                row["Reparingdate"] = DBNull.Value;
            }
        }
        sortedgv_JOBCARD.DataSource = dt;
        sortedgv_JOBCARD.DataBind();
    }

    public void Getsorteddiscriptions()
    {
        try
        {
            ViewState["Record"] = "Disc";
            gv_JOBCARD.Visible = false;
            DataTable dt = new DataTable();
            SqlDataAdapter sad = new SqlDataAdapter("select * from tblJobcardHdr where ItemDesc='" + txtitemdesc.Text + "' AND isdeleted='0'", con);
            sad.Fill(dt);
            foreach (DataRow row in dt.Rows)
            {
                if (row["Reparingdate"] != DBNull.Value && Convert.ToDateTime(row["Reparingdate"]).ToString("yyyy-MM-dd") == "1900-01-01")
                {
                    row["Reparingdate"] = DBNull.Value;
                }
            }
            sortedgv_JOBCARD.DataSource = dt;
            sortedgv_JOBCARD.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public void GetsorteddiscriptionsGrid()
    {

        gv_JOBCARD.Visible = false;
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblJobcardHdr where ItemDesc='" + txtitemdesc.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        foreach (DataRow row in dt.Rows)
        {
            if (row["Reparingdate"] != DBNull.Value && Convert.ToDateTime(row["Reparingdate"]).ToString("yyyy-MM-dd") == "1900-01-01")
            {
                row["Reparingdate"] = DBNull.Value;
            }
        }
        sortedgv_JOBCARD.DataSource = dt;
        sortedgv_JOBCARD.DataBind();
    }


    public void Getsortdatadeswithjob()
    {
        gv_JOBCARD.Visible = false;
        ViewState["Record"] = "Disc&Job";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblJobcardHdr where ItemDesc='" + txtitemdesc.Text + "' AND JobCardNo='" + txtjob.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        foreach (DataRow row in dt.Rows)
        {
            if (row["Reparingdate"] != DBNull.Value && Convert.ToDateTime(row["Reparingdate"]).ToString("yyyy-MM-dd") == "1900-01-01")
            {
                row["Reparingdate"] = DBNull.Value;
            }
        }
        sortedgv_JOBCARD.DataSource = dt;
        sortedgv_JOBCARD.DataBind();
    }


    public void GetsortdatadeswithjobGrid()
    {
        gv_JOBCARD.Visible = false;
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblJobcardHdr where ItemDesc='" + txtitemdesc.Text + "' AND JobCardNo='" + txtjob.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        foreach (DataRow row in dt.Rows)
        {
            if (row["Reparingdate"] != DBNull.Value && Convert.ToDateTime(row["Reparingdate"]).ToString("yyyy-MM-dd") == "1900-01-01")
            {
                row["Reparingdate"] = DBNull.Value;
            }
        }
        sortedgv_JOBCARD.DataSource = dt;
        sortedgv_JOBCARD.DataBind();
    }


    public void SortedDataFromdateToLastdate()
    {
        ViewState["Record"] = "Date";
        gv_JOBCARD.Visible = false;
        DataTable dtt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblJobcardHdr where InwardDate between '" + txt_form_podate_search.Text + "' AND '" + txt_to_podate_search.Text + "' AND isdeleted='0'", con);

        //SqlDataAdapter sad = new SqlDataAdapter("select  *  from tblJobcardHdr where  InwardDate between '" + txt_form_podate_search.Text + "'  AND '" + txt_to_podate_search.Text + "' AND isdeleted='0' '", con);
        sad.Fill(dtt);
        foreach (DataRow row in dtt.Rows)
        {
            if (row["Reparingdate"] != DBNull.Value && Convert.ToDateTime(row["Reparingdate"]).ToString("yyyy-MM-dd") == "1900-01-01")
            {
                row["Reparingdate"] = DBNull.Value;
            }
        }
        sortedgv_JOBCARD.DataSource = dtt;
        sortedgv_JOBCARD.DataBind();
    }


    public void FromdateToastdategrid()
    {
        gv_JOBCARD.Visible = false;
        DataTable dtt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblJobcardHdr where InwardDate between '" + txt_form_podate_search.Text + "' AND '" + txt_to_podate_search.Text + "' AND isdeleted='0'", con);

        //SqlDataAdapter sad = new SqlDataAdapter("select  *  from tblJobcardHdr where  InwardDate between '" + txt_form_podate_search.Text + "'  AND '" + txt_to_podate_search.Text + "' AND isdeleted='0' '", con);
        sad.Fill(dtt);
        foreach (DataRow row in dtt.Rows)
        {
            if (row["Reparingdate"] != DBNull.Value && Convert.ToDateTime(row["Reparingdate"]).ToString("yyyy-MM-dd") == "1900-01-01")
            {
                row["Reparingdate"] = DBNull.Value;
            }
        }
        sortedgv_JOBCARD.DataSource = dtt;
        sortedgv_JOBCARD.DataBind();

    }


    public void DatwiseJobNo()
    {
        ViewState["Record"] = "Datewisejob";
        gv_JOBCARD.Visible = false;
        DataTable dtt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM tblJobcardHdr WHERE InwardDate BETWEEN '" + txt_form_podate_search.Text + "' AND '" + txt_to_podate_search.Text + "' AND JobCardNo='" + txtjob.Text + "' AND isdeleted='0' '", con);
        sad.Fill(dtt);
        foreach (DataRow row in dtt.Rows)
        {
            if (row["Reparingdate"] != DBNull.Value && Convert.ToDateTime(row["Reparingdate"]).ToString("yyyy-MM-dd") == "1900-01-01")
            {
                row["Reparingdate"] = DBNull.Value;
            }
        }
        sortedgv_JOBCARD.DataSource = dtt;
        sortedgv_JOBCARD.DataBind();

    }


    public void DatwiseJobNogrid()
    {

        gv_JOBCARD.Visible = false;
        DataTable dtt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM tblJobcardHdr WHERE InwardDate BETWEEN '" + txt_form_podate_search.Text + "' AND '" + txt_to_podate_search.Text + "' AND JobCardNo='" + txtjob.Text + "' AND isdeleted='0' '", con);
        sad.Fill(dtt);
        foreach (DataRow row in dtt.Rows)
        {
            if (row["Reparingdate"] != DBNull.Value && Convert.ToDateTime(row["Reparingdate"]).ToString("yyyy-MM-dd") == "1900-01-01")
            {
                row["Reparingdate"] = DBNull.Value;
            }
        }
        sortedgv_JOBCARD.DataSource = dtt;
        sortedgv_JOBCARD.DataBind();

    }


    public void GetDatwiseRepetedNO()
    {
        ViewState["Record"] = "datewiserepeted";
        gv_JOBCARD.Visible = false;
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM tblJobcardHdr WHERE InwardDate BETWEEN '" + txt_form_podate_search.Text + "' AND '" + txt_to_podate_search.Text + "' AND RepeatedNo='" + txtrepetedno.Text + "' AND isdeleted='0' '", con);
        sad.Fill(dt);
        foreach (DataRow row in dt.Rows)
        {
            if (row["Reparingdate"] != DBNull.Value && Convert.ToDateTime(row["Reparingdate"]).ToString("yyyy-MM-dd") == "1900-01-01")
            {
                row["Reparingdate"] = DBNull.Value;
            }
        }
        sortedgv_JOBCARD.DataSource = dt;
        sortedgv_JOBCARD.DataBind();
    }
    public void GetDatwiseRepetedNOgrid()
    {
        gv_JOBCARD.Visible = false;
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM tblJobcardHdr WHERE InwardDate BETWEEN '" + txt_form_podate_search.Text + "' AND '" + txt_to_podate_search.Text + "' AND RepeatedNo='" + txtrepetedno.Text + "' AND isdeleted='0' '", con);
        sad.Fill(dt);
        foreach (DataRow row in dt.Rows)
        {
            if (row["Reparingdate"] != DBNull.Value && Convert.ToDateTime(row["Reparingdate"]).ToString("yyyy-MM-dd") == "1900-01-01")
            {
                row["Reparingdate"] = DBNull.Value;
            }
        }
        sortedgv_JOBCARD.DataSource = dt;
        sortedgv_JOBCARD.DataBind();
    }

    //Eng Name status date 
    public void GetDatweiseEngineer()
    {
        gv_JOBCARD.Visible = false;
        ViewState["Record"] = "DatewiseER";
        
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM tblJobcardHdr WHERE InwardDate BETWEEN '" + txt_form_podate_search.Text + "' AND '" + txt_to_podate_search.Text + "' AND EngineerName='" + txtengineername.Text + "' AND status='" + txtstatus.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        foreach (DataRow row in dt.Rows)
        {
            if (row["Reparingdate"] != DBNull.Value && Convert.ToDateTime(row["Reparingdate"]).ToString("yyyy-MM-dd") == "1900-01-01")
            {
                row["Reparingdate"] = DBNull.Value;
            }
        }
        sortedgv_JOBCARD.DataSource = dt;
        sortedgv_JOBCARD.DataBind();

    }


    public void GetDatweiseEngineergrid()
    {

        gv_JOBCARD.Visible = false;
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM tblJobcardHdr WHERE InwardDate BETWEEN '" + txt_form_podate_search.Text + "' AND '" + txt_to_podate_search.Text + "' AND EngineerName='" + txtengineername.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        foreach (DataRow row in dt.Rows)
        {
            if (row["Reparingdate"] != DBNull.Value && Convert.ToDateTime(row["Reparingdate"]).ToString("yyyy-MM-dd") == "1900-01-01")
            {
                row["Reparingdate"] = DBNull.Value;
            }
        }
        sortedgv_JOBCARD.DataSource = dt;
        sortedgv_JOBCARD.DataBind();

    }

    public void GetDatewisediscriptions()
    {
        try
        {
            ViewState["Record"] = "DatwiseDisc";
            gv_JOBCARD.Visible = false;
            DataTable dt = new DataTable();
            SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM tblJobcardHdr WHERE InwardDate BETWEEN '" + txt_form_podate_search.Text + "' AND '" + txt_to_podate_search.Text + "' AND ItemDesc='" + txtitemdesc.Text + "' AND isdeleted='0'", con);
            sad.Fill(dt);
            foreach (DataRow row in dt.Rows)
            {
                if (row["Reparingdate"] != DBNull.Value && Convert.ToDateTime(row["Reparingdate"]).ToString("yyyy-MM-dd") == "1900-01-01")
                {
                    row["Reparingdate"] = DBNull.Value;
                }
            }
            sortedgv_JOBCARD.DataSource = dt;
            sortedgv_JOBCARD.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }



    public void GetDatewisediscriptionsgrid()
    {
        try
        {
            gv_JOBCARD.Visible = false;
            DataTable dt = new DataTable();
            SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM tblJobcardHdr WHERE InwardDate BETWEEN '" + txt_form_podate_search.Text + "' AND '" + txt_to_podate_search.Text + "' AND ItemDesc='" + txtitemdesc.Text + "' AND isdeleted='0'", con);
            sad.Fill(dt);
            foreach (DataRow row in dt.Rows)
            {
                if (row["Reparingdate"] != DBNull.Value && Convert.ToDateTime(row["Reparingdate"]).ToString("yyyy-MM-dd") == "1900-01-01")
                {
                    row["Reparingdate"] = DBNull.Value;
                }
            }
            sortedgv_JOBCARD.DataSource = dt;
            sortedgv_JOBCARD.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public void GetDatewisestatus()
    {
        gv_JOBCARD.Visible = false;
        ViewState["Record"] = "Datewisestatus";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM tblJobcardHdr WHERE Reparingdate BETWEEN '" + txt_form_podate_search.Text + "' AND '" + txt_to_podate_search.Text + "' AND status='" + txtstatus.Text + "' AND isdeleted='0'", con);

        //SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM tblJobcardHdr WHERE InwardDate BETWEEN '" + txt_form_podate_search.Text + "' AND '" + txt_to_podate_search.Text + "' AND status='" + txtstatus.Text + "' AND isdeleted='0' '", con);
        sad.Fill(dt);
        foreach (DataRow row in dt.Rows)
        {
            if (row["Reparingdate"] != DBNull.Value && Convert.ToDateTime(row["Reparingdate"]).ToString("yyyy-MM-dd") == "1900-01-01")
            {
                row["Reparingdate"] = DBNull.Value;
            }
        }
        sortedgv_JOBCARD.DataSource = dt;
        sortedgv_JOBCARD.DataBind();
    }

    public void GetDatewisestatusgrid()
    {
        gv_JOBCARD.Visible = false;
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM tblJobcardHdr WHERE InwardDate BETWEEN '" + txt_form_podate_search.Text + "' AND '" + txt_to_podate_search.Text + "' AND status='" + txtstatus.Text + "' AND isdeleted='0'", con);

        //SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM tblJobcardHdr WHERE InwardDate BETWEEN '" + txt_form_podate_search.Text + "' AND '" + txt_to_podate_search.Text + "' AND status='" + txtstatus.Text + "' AND isdeleted='0''", con);
        sad.Fill(dt);
        foreach (DataRow row in dt.Rows)
        {
            if (row["Reparingdate"] != DBNull.Value && Convert.ToDateTime(row["Reparingdate"]).ToString("yyyy-MM-dd") == "1900-01-01")
            {
                row["Reparingdate"] = DBNull.Value;
            }
        }
        sortedgv_JOBCARD.DataSource = dt;
        sortedgv_JOBCARD.DataBind();
    }

    public void Getdatwiserepetedno()
    {
        ViewState["Record"] = "Datewiserepeted";
        gv_JOBCARD.Visible = false;
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM tblJobcardHdr WHERE InwardDate BETWEEN '" + txt_form_podate_search.Text + "' AND '" + txt_to_podate_search.Text + "' AND RepeatedNo='" + txtrepetedno.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        foreach (DataRow row in dt.Rows)
        {
            if (row["Reparingdate"] != DBNull.Value && Convert.ToDateTime(row["Reparingdate"]).ToString("yyyy-MM-dd") == "1900-01-01")
            {
                row["Reparingdate"] = DBNull.Value;
            }
        }
        sortedgv_JOBCARD.DataSource = dt;
        sortedgv_JOBCARD.DataBind();
    }

    public void Getdatwiserepetednogrid()
    {
        gv_JOBCARD.Visible = false;
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM tblJobcardHdr WHERE InwardDate BETWEEN '" + txt_form_podate_search.Text + "' AND '" + txt_to_podate_search.Text + "' AND RepeatedNo='" + txtrepetedno.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        foreach (DataRow row in dt.Rows)
        {
            if (row["Reparingdate"] != DBNull.Value && Convert.ToDateTime(row["Reparingdate"]).ToString("yyyy-MM-dd") == "1900-01-01")
            {
                row["Reparingdate"] = DBNull.Value;
            }
        }
        sortedgv_JOBCARD.DataSource = dt;
        sortedgv_JOBCARD.DataBind();
    }

    protected void sortedgv_JOBCARD_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        sortedgv_JOBCARD.Visible = true;
        if (ViewState["Record"].ToString() == "ER")
        {
            sortedgv_JOBCARD.PageIndex = e.NewPageIndex;
            GridRecord1();
        }
        if (ViewState["Record"].ToString() == "jobno")
        {
            sortedgv_JOBCARD.PageIndex = e.NewPageIndex;
            sortedjobGrid();
        }

        if (ViewState["Record"].ToString() == "repeted")
        {
            sortedgv_JOBCARD.PageIndex = e.NewPageIndex;
            Sortedrepetednogrid();
        }
        if (ViewState["Record"].ToString() == "status")
        {
            sortedgv_JOBCARD.PageIndex = e.NewPageIndex;
            GetSortedstatusGrid();
        }

        if (ViewState["Record"].ToString() == "Disc")
        {
            sortedgv_JOBCARD.PageIndex = e.NewPageIndex;
            GetsorteddiscriptionsGrid();
        }

        if (ViewState["Record"].ToString() == "Disc & Job")
        {
            sortedgv_JOBCARD.PageIndex = e.NewPageIndex;
            GetsortdatadeswithjobGrid();
        }
        if (ViewState["Record"].ToString() == "Date")
        {
            sortedgv_JOBCARD.PageIndex = e.NewPageIndex;
            FromdateToastdategrid();
        }

        if (ViewState["Record"].ToString() == "Datewisejob")
        {
            sortedgv_JOBCARD.PageIndex = e.NewPageIndex;
            DatwiseJobNogrid();
        }

        if (ViewState["Record"].ToString() == "datewiserepeted")
        {
            sortedgv_JOBCARD.PageIndex = e.NewPageIndex;
            GetDatwiseRepetedNOgrid();
        }

        if (ViewState["Record"].ToString() == "DatewiseER")
        {
            sortedgv_JOBCARD.PageIndex = e.NewPageIndex;
            GetDatweiseEngineergrid();
        }
        if (ViewState["Record"].ToString() == "DatwiseDisc")
        {
            sortedgv_JOBCARD.PageIndex = e.NewPageIndex;
            GetDatewisediscriptionsgrid();
        }

        if (ViewState["Record"].ToString() == "Datewisestatus")
        {
            sortedgv_JOBCARD.PageIndex = e.NewPageIndex;
            GetDatewisestatusgrid();
        }

        if (ViewState["Record"].ToString() == "Datewiserepeted")
        {
            sortedgv_JOBCARD.PageIndex = e.NewPageIndex;
            Getdatwiserepetednogrid();
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        //required to avoid the run time error "  
        //Control 'GridView1' of type 'Grid View' must be placed inside a form tag with runat=server."  
    }

    protected void btnexport_Click(object sender, EventArgs e)
    {
        ExportGridToExcel();
    }

    private void ExportGridToExcel()
    {
        if (ViewState["Excell"] != null)
        {
            string Method = ViewState["Excell"].ToString();
            if (Method == "GetsortedJobno")
            {
                GetsortedJobnoForExcell();
            }
            if (Method == "Getsorepetedno")
            {
                GetsorepetednoForExcell();
            }
            if (Method == "GetsortedData")
            {
                GetsortedDataForExcell();
            }
            if (Method == "GetSortedstatus")
            {
                GetSortedstatusForExcell();
            }
            if (Method == "Getsorteddiscriptions")
            {
                GetsorteddiscriptionsForExcell();
            }
            if (Method == "Getsortdatadeswithjob")
            {
                GetsortdatadeswithjobForExcell();
            }
            if (Method == "SortedDataFromdateToLastdate")
            {
                SortedDataFromdateToLastdateForExcell();
            }
            if (Method == "DatwiseJobNo")
            {
                DatwiseJobNoForExcell();
            }
            if (Method == "GetDatweiseEngineer")
            {
                GetDatweiseEngineerForExcell();
            }
            if (Method == "GetDatewisediscriptions")
            {
                GetDatewisediscriptionsForExcell();

            }
            if (Method == "GetDatewisestatus")
            {
                GetDatewisestatusForExcell();

            }
            if (Method == "Getdatwiserepetedno")
            {
                GetdatwiserepetednoForExcell();

            }
        }
        else
        {
            GridExport();
        }
        Response.Clear();
        Response.Buffer = true;
        Response.ClearContent();
        Response.ClearHeaders();
        Response.Charset = "";
        string FileName = "Job_Card_List_" + DateTime.Now + ".xls";
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

    public void GetsortedJobnoForExcell()
    {
        ViewState["Record"] = "jobno";

        gv_JOBCARD.Visible = false;
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblJobcardHdr where JobCardNo='" + txtjob.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        foreach (DataRow row in dt.Rows)
        {
            if (row["Reparingdate"] != DBNull.Value && Convert.ToDateTime(row["Reparingdate"]).ToString("yyyy-MM-dd") == "1900-01-01")
            {
                row["Reparingdate"] = DBNull.Value;
            }
        }
        GridExportExcel.DataSource = dt;
        GridExportExcel.DataBind();
    }
    public void GetsorepetednoForExcell()
    {
        ViewState["Record"] = "repeted";

        gv_JOBCARD.Visible = false;
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblJobcardHdr where RepeatedNo='" + txtrepetedno.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        foreach (DataRow row in dt.Rows)
        {
            if (row["Reparingdate"] != DBNull.Value && Convert.ToDateTime(row["Reparingdate"]).ToString("yyyy-MM-dd") == "1900-01-01")
            {
                row["Reparingdate"] = DBNull.Value;
            }
        }
        GridExportExcel.DataSource = dt;
        GridExportExcel.DataBind();
    }
    public void GetsortedDataForExcell()
    {
        ViewState["Record"] = "ER";
        gv_JOBCARD.Visible = false;
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblJobcardHdr where EngineerName='" + txtengineername.Text + "'", con);
        sad.Fill(dt);
        foreach (DataRow row in dt.Rows)
        {
            if (row["Reparingdate"] != DBNull.Value && Convert.ToDateTime(row["Reparingdate"]).ToString("yyyy-MM-dd") == "1900-01-01")
            {
                row["Reparingdate"] = DBNull.Value;
            }
        }
        GridExportExcel.DataSource = dt;
        GridExportExcel.DataBind();
    }
    public void GetSortedstatusForExcell()
    {
        gv_JOBCARD.Visible = false;
        ViewState["Record"] = "status";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblJobcardHdr where status='" + txtstatus.Text + "'", con);
        sad.Fill(dt);
        foreach (DataRow row in dt.Rows)
        {
            if (row["Reparingdate"] != DBNull.Value && Convert.ToDateTime(row["Reparingdate"]).ToString("yyyy-MM-dd") == "1900-01-01")
            {
                row["Reparingdate"] = DBNull.Value;
            }
        }
        GridExportExcel.DataSource = dt;
        GridExportExcel.DataBind();
    }
    public void GetsorteddiscriptionsForExcell()
    {
        try
        {
            ViewState["Record"] = "Disc";
            gv_JOBCARD.Visible = false;
            DataTable dt = new DataTable();
            SqlDataAdapter sad = new SqlDataAdapter("select * from tblJobcardHdr where ItemDesc='" + txtitemdesc.Text + "' AND isdeleted='0'", con);
            sad.Fill(dt);
            foreach (DataRow row in dt.Rows)
            {
                if (row["Reparingdate"] != DBNull.Value && Convert.ToDateTime(row["Reparingdate"]).ToString("yyyy-MM-dd") == "1900-01-01")
                {
                    row["Reparingdate"] = DBNull.Value;
                }
            }
            GridExportExcel.DataSource = dt;
            GridExportExcel.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public void GetsortdatadeswithjobForExcell()
    {
        gv_JOBCARD.Visible = false;
        ViewState["Record"] = "Disc&Job";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblJobcardHdr where ItemDesc='" + txtitemdesc.Text + "' AND JobCardNo='" + txtjob.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        foreach (DataRow row in dt.Rows)
        {
            if (row["Reparingdate"] != DBNull.Value && Convert.ToDateTime(row["Reparingdate"]).ToString("yyyy-MM-dd") == "1900-01-01")
            {
                row["Reparingdate"] = DBNull.Value;
            }
        }
        GridExportExcel.DataSource = dt;
        GridExportExcel.DataBind();
    }
    public void SortedDataFromdateToLastdateForExcell()
    {
        ViewState["Record"] = "Date";
        gv_JOBCARD.Visible = false;
        DataTable dtt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblJobcardHdr where InwardDate between '" + txt_form_podate_search.Text + "' AND '" + txt_to_podate_search.Text + "' AND isdeleted='0'", con);
        //SqlDataAdapter sad = new SqlDataAdapter("select  *  from tblJobcardHdr where  InwardDate between '" + txt_form_podate_search.Text + "'  AND '" + txt_to_podate_search.Text + "' AND isdeleted='0' '", con);
        sad.Fill(dtt);
        foreach (DataRow row in dtt.Rows)
        {
            if (row["Reparingdate"] != DBNull.Value && Convert.ToDateTime(row["Reparingdate"]).ToString("yyyy-MM-dd") == "1900-01-01")
            {
                row["Reparingdate"] = DBNull.Value;
            }
        }
        GridExportExcel.DataSource = dtt;
        GridExportExcel.DataBind();
    }
    public void DatwiseJobNoForExcell()
    {
        ViewState["Record"] = "Datewisejob";
        gv_JOBCARD.Visible = false;
        DataTable dtt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM tblJobcardHdr WHERE InwardDate BETWEEN '" + txt_form_podate_search.Text + "' AND '" + txt_to_podate_search.Text + "' AND JobCardNo='" + txtjob.Text + "' AND isdeleted='0' '", con);
        sad.Fill(dtt);
        foreach (DataRow row in dtt.Rows)
        {
            if (row["Reparingdate"] != DBNull.Value && Convert.ToDateTime(row["Reparingdate"]).ToString("yyyy-MM-dd") == "1900-01-01")
            {
                row["Reparingdate"] = DBNull.Value;
            }
        }
        GridExportExcel.DataSource = dtt;
        GridExportExcel.DataBind();

    }
    public void GetDatweiseEngineerForExcell()
    {
        gv_JOBCARD.Visible = false;
        ViewState["Record"] = "DatewiseER";

        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM tblJobcardHdr WHERE InwardDate BETWEEN '" + txt_form_podate_search.Text + "' AND '" + txt_to_podate_search.Text + "' AND EngineerName='" + txtengineername.Text + "' AND isdeleted='0'", con);

        sad.Fill(dt);
        foreach (DataRow row in dt.Rows)
        {
            if (row["Reparingdate"] != DBNull.Value && Convert.ToDateTime(row["Reparingdate"]).ToString("yyyy-MM-dd") == "1900-01-01")
            {
                row["Reparingdate"] = DBNull.Value;
            }
        }
        GridExportExcel.DataSource = dt;
        GridExportExcel.DataBind();

    }
    public void GetDatewisediscriptionsForExcell()
    {
        try
        {
            ViewState["Record"] = "DatwiseDisc";
            gv_JOBCARD.Visible = false;
            DataTable dt = new DataTable();
            SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM tblJobcardHdr WHERE InwardDate BETWEEN '" + txt_form_podate_search.Text + "' AND '" + txt_to_podate_search.Text + "' AND ItemDesc='" + txtitemdesc.Text + "' AND isdeleted='0'", con);

            sad.Fill(dt);
            foreach (DataRow row in dt.Rows)
            {
                if (row["Reparingdate"] != DBNull.Value && Convert.ToDateTime(row["Reparingdate"]).ToString("yyyy-MM-dd") == "1900-01-01")
                {
                    row["Reparingdate"] = DBNull.Value;
                }
            }
            GridExportExcel.DataSource = dt;
            GridExportExcel.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public void GetDatewisestatusForExcell()
    {
        gv_JOBCARD.Visible = false;
        ViewState["Record"] = "Datewisestatus";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM tblJobcardHdr WHERE Reparingdate BETWEEN '" + txt_form_podate_search.Text + "' AND '" + txt_to_podate_search.Text + "' AND status='" + txtstatus.Text + "' AND isdeleted='0'", con);

        //SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM tblJobcardHdr WHERE InwardDate BETWEEN '" + txt_form_podate_search.Text + "' AND '" + txt_to_podate_search.Text + "' AND status='" + txtstatus.Text + "' AND isdeleted='0' '", con);
        sad.Fill(dt);
        foreach (DataRow row in dt.Rows)
        {
            if (row["Reparingdate"] != DBNull.Value && Convert.ToDateTime(row["Reparingdate"]).ToString("yyyy-MM-dd") == "1900-01-01")
            {
                row["Reparingdate"] = DBNull.Value;
            }
        }
        GridExportExcel.DataSource = dt;
        GridExportExcel.DataBind();
    }
    public void GetdatwiserepetednoForExcell()
    {
        ViewState["Record"] = "Datewiserepeted";
        gv_JOBCARD.Visible = false;
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM tblJobcardHdr WHERE InwardDate BETWEEN '" + txt_form_podate_search.Text + "' AND '" + txt_to_podate_search.Text + "' AND RepeatedNo='" + txtrepetedno.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        foreach (DataRow row in dt.Rows)
        {
            if (row["Reparingdate"] != DBNull.Value && Convert.ToDateTime(row["Reparingdate"]).ToString("yyyy-MM-dd") == "1900-01-01")
            {
                row["Reparingdate"] = DBNull.Value;
            }
        }
        GridExportExcel.DataSource = dt;
        GridExportExcel.DataBind();
    }
}