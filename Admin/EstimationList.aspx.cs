using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Security.Cryptography;
using System.IO;
using System.Text;

public partial class Admin_EstimationList : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Gridrecord();
        }
    }
    protected void Gridrecord()
    {
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT Id,JobNo,CustName,FinalStatus,FinalCost,SiteVisitCharges,OtherCharges,EstimatedQuotation,Componetstatus,CreatedBy,CompRecDate,Convert(varchar,A.CreatedDate,103) " +
            "as CreatedDate from tblEstimationHdr as A where isdeleted='0' order by A.CreatedDate DESC", con);
        sad.Fill(dt);
        
        foreach (DataRow row in dt.Rows)
        {          
            DateTime compRecDate = (DateTime)row["CompRecDate"];
            DateTime defaultDate = new DateTime(1900, 1, 1);
            if (compRecDate != defaultDate)
            {
                row["CompRecDate"] = compRecDate.ToString("yyyy-MM-dd");
            }
            else
            {
                row["CompRecDate"] = DBNull.Value;
            }
        }
        gv_EstimationList.DataSource = dt;
        gv_EstimationList.EmptyDataText = "Record Not Found";
        gv_EstimationList.DataBind();
    }

    protected void ExportExcelGrid()
    {
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT Id,JobNo,CustName,FinalStatus,FinalCost,SiteVisitCharges,OtherCharges,EstimatedQuotation,Componetstatus,CreatedBy,CompRecDate,Convert(varchar,A.CreatedDate,103) " +
            "as CreatedDate from tblEstimationHdr as A where isdeleted='0'order by A.CreatedDate DESC", con);
        sad.Fill(dt);
        Exporttoexcelgrid.DataSource = dt;
        Exporttoexcelgrid.EmptyDataText = "Record Not Found";
        Exporttoexcelgrid.DataBind();
    }

    protected void btncreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("EstimationMaster.aspx");
    }

    protected void gv_EstimationList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "RowEdit")
        {
            Response.Redirect("EstimationMaster.aspx?JobNo=" + encrypt(e.CommandArgument.ToString()) + "");
        }

        if (e.CommandName == "RowDelete")
        {

            SqlCommand cmddelete = new SqlCommand("update tblEstimationHdr set isdeleted='1' where Id=@Id", con);
            cmddelete.Parameters.AddWithValue("@Id", Convert.ToInt32(e.CommandArgument.ToString()));
            cmddelete.Parameters.AddWithValue("@isdeleted", '1');
            con.Open();
            cmddelete.ExecuteNonQuery();
            con.Close();
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Deleted Successfully');", true);

            //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Delete sucessfully!!');window.location ='CustomerList.aspx';", true);

            Gridrecord();
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

    protected void lnkBtnsearch_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtjob.Text) && (string.IsNullOrEmpty(txtcustSearch.Text)))
        {
            Gridrecord();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Please Search Record !!!');", true);
        }
        else if (!string.IsNullOrEmpty(txtjob.Text) && (string.IsNullOrEmpty(txtcustSearch.Text)))
        {
            try
            {
                ViewState["Excell"] = "Getsortedjobno";
                Getsortedjobno();
                //DataTable dt = new DataTable();
                //SqlDataAdapter sad = new SqlDataAdapter("SELECT Id,JobNo,CustName,FinalStatus,FinalCost,SiteVisitCharges,OtherCharges,EstimatedQuotation,Componetstatus,CompRecDate,CreatedBy, Convert(varchar,A.CreatedDate,103) as CreatedDate from tblEstimationHdr as A where JobNo='" + txtjob.Text + "' AND isdeleted='0'order by A.CreatedDate", con);
                //sad.Fill(dt);
                //gv_EstimationList.DataSource = dt;
                //gv_EstimationList.EmptyDataText = "Record Not Found";
                //gv_EstimationList.DataBind();

            }
            catch (Exception)
            {

                throw;
            }
        }
        else if (string.IsNullOrEmpty(txtjob.Text) && (!string.IsNullOrEmpty(txtcustSearch.Text)))
        {
            try
            {
                ViewState["Excell"] = "GetSortedcutsomer";
                GetSortedcutsomer();
                //DataTable dt = new DataTable();
                //SqlDataAdapter sad = new SqlDataAdapter("SELECT Id,JobNo,CustName,FinalStatus,SiteVisitCharges,OtherCharges,EstimatedQuotation,FinalCost,Componetstatus,CompRecDate,CreatedBy, Convert(varchar,A.CreatedDate,103) as CreatedDate from tblEstimationHdr as A where CustName='" + txtcustSearch.Text + "' AND  isdeleted='0' order by A.CreatedDate", con);
                //sad.Fill(dt);
                //gv_EstimationList.DataSource = dt;
                //gv_EstimationList.EmptyDataText = "Record Not Found";
                //gv_EstimationList.DataBind();

            }
            catch (Exception)
            {

                throw;
            }
        }
        else if (!string.IsNullOrEmpty(txtjob.Text) && (!string.IsNullOrEmpty(txtcustSearch.Text)))
        {
            try
            {
                ViewState["Excell"] = "Getsortedcustomerwithjob";
                Getsortedcustomerwithjob();
                //DataTable dt = new DataTable();
                //SqlDataAdapter sad = new SqlDataAdapter("SELECT Id,JobNo,CustName,FinalStatus,Componetstatus,CompRecDate,CreatedBy, Convert(varchar,A.CreatedDate,103) as CreatedDate from tblEstimationHdr as A where CustName='" + txtcustSearch.Text + "' AND JobNo='" + txtjob.Text + "'  AND  isdeleted='0' order by A.CreatedDate", con);
                //sad.Fill(dt);
                //gv_EstimationList.DataSource = dt;
                //gv_EstimationList.EmptyDataText = "Record Not Found";
                //gv_EstimationList.DataBind();

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
    protected void lnkrefresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("EstimationList.aspx");
    }

    protected void gv_EstimationList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_EstimationList.PageIndex = e.NewPageIndex;
        Gridrecord();
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
                com.CommandText = "select DISTINCT JobNo from tblEstimationHdr where " + "JobNo like @Search + '%' AND isdeleted='0'";

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
        return AutoFillcustomerlist(prefixText);
    }

    public static List<string> AutoFillcustomerlist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT CustName from tblEstimationHdr where " + "CustName like @Search + '%' AND isdeleted='0'";

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


    //sorted Grid start

    protected void sortedgvEstimations_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_EstimationList.PageIndex = e.NewPageIndex;
        Gridrecord();

        if (ViewState["Record"].ToString() == "Job")
        {
            sortedgvEstimations.PageIndex = e.NewPageIndex;
            GetsortedjobnoGrid();
        }
        if (ViewState["Record"].ToString() == "Customer")
        {
            sortedgvEstimations.PageIndex = e.NewPageIndex;
            GetSortedcutsomergrid();
        }
        if (ViewState["Record"].ToString() == "Customer&Job")
        {
            sortedgvEstimations.PageIndex = e.NewPageIndex;
            GetsortedcustomerwithjobGrid();
        }
    }

    public void Getsortedjobno()
    {

        gv_EstimationList.Visible = false;
        ViewState["Report"] = "Job";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT Id,JobNo,CustName,FinalStatus,FinalCost,SiteVisitCharges,OtherCharges,EstimatedQuotation,Componetstatus,CompRecDate,CreatedBy, Convert(varchar,A.CreatedDate,103) as CreatedDate from tblEstimationHdr as A where JobNo='" + txtjob.Text + "' AND isdeleted='0'order by A.CreatedDate", con);
        sad.Fill(dt);
        sortedgvEstimations.DataSource = dt;
        sortedgvEstimations.EmptyDataText = "Record Not Found";
        sortedgvEstimations.DataBind();
    }

    public void GetsortedjobnoGrid()
    {

        gv_EstimationList.Visible = false;
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT Id,JobNo,CustName,FinalStatus,FinalCost,SiteVisitCharges,OtherCharges,EstimatedQuotation,Componetstatus,CompRecDate,CreatedBy, Convert(varchar,A.CreatedDate,103) as CreatedDate from tblEstimationHdr as A where JobNo='" + txtjob.Text + "' AND isdeleted='0'order by A.CreatedDate", con);
        sad.Fill(dt);
        sortedgvEstimations.DataSource = dt;
        sortedgvEstimations.EmptyDataText = "Record Not Found";
        sortedgvEstimations.DataBind();
    }

    public void GetSortedcutsomer()
    {
        gv_EstimationList.Visible = false;
        ViewState["Report"] = "Customer";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT Id,JobNo,CustName,FinalStatus,SiteVisitCharges,OtherCharges,EstimatedQuotation,FinalCost,Componetstatus,CompRecDate,CreatedBy, Convert(varchar,A.CreatedDate,103) as CreatedDate from tblEstimationHdr as A where CustName='" + txtcustSearch.Text + "' AND  isdeleted='0' order by A.CreatedDate", con);
        sad.Fill(dt);
        sortedgvEstimations.DataSource = dt;
        sortedgvEstimations.EmptyDataText = "Record Not Found";
        sortedgvEstimations.DataBind();
    }

    public void GetSortedcutsomergrid()
    {
        gv_EstimationList.Visible = false;
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT Id,JobNo,CustName,FinalStatus,SiteVisitCharges,OtherCharges,EstimatedQuotation,FinalCost,Componetstatus,CompRecDate,CreatedBy, Convert(varchar,A.CreatedDate,103) as CreatedDate from tblEstimationHdr as A where CustName='" + txtcustSearch.Text + "' AND  isdeleted='0' order by A.CreatedDate", con);
        sad.Fill(dt);
        sortedgvEstimations.DataSource = dt;
        sortedgvEstimations.EmptyDataText = "Record Not Found";
        sortedgvEstimations.DataBind();
    }

    public void Getsortedcustomerwithjob()
    {
        gv_EstimationList.Visible = false;
        ViewState["Report"] = "Customer&Job";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT Id,JobNo,CustName,FinalStatus,Componetstatus,CompRecDate,CreatedBy, Convert(varchar,A.CreatedDate,103) as CreatedDate from tblEstimationHdr as A where CustName='" + txtcustSearch.Text + "' AND JobNo='" + txtjob.Text + "'  AND  isdeleted='0' order by A.CreatedDate", con);
        sad.Fill(dt);
        sortedgvEstimations.DataSource = dt;
        sortedgvEstimations.EmptyDataText = "Record Not Found";
        sortedgvEstimations.DataBind();
    }

    public void GetsortedcustomerwithjobGrid()
    {
        gv_EstimationList.Visible = false;
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT Id,JobNo,CustName,FinalStatus,Componetstatus,CompRecDate,CreatedBy, Convert(varchar,A.CreatedDate,103) as CreatedDate from tblEstimationHdr as A where CustName='" + txtcustSearch.Text + "' AND JobNo='" + txtjob.Text + "'  AND  isdeleted='0' order by A.CreatedDate", con);
        sad.Fill(dt);
        sortedgvEstimations.DataSource = dt;
        sortedgvEstimations.EmptyDataText = "Record Not Found";
        sortedgvEstimations.DataBind();
    }

    //sorted Grid End

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
            string Method = ViewState["Excell"].ToString();
            if (Method == "Getsortedjobno")
            {
                GetsortedjobnoForExcell();
            }
            if (Method == "GetSortedcutsomer")
            {
                GetSortedcutsomerForExcell();
            }
            if (Method == "Getsortedcustomerwithjob")
            {
                GetsortedcustomerwithjobForExcel();
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
        string FileName = "Estimation_List_" + DateTime.Now + ".xls";
        StringWriter strwritter = new StringWriter();
        HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
        Exporttoexcelgrid.GridLines = GridLines.Both;
        Exporttoexcelgrid.HeaderStyle.Font.Bold = true;
        Exporttoexcelgrid.RenderControl(htmltextwrtter);
        Response.Write(strwritter.ToString());
        Response.End();
    }

    public void GetsortedjobnoForExcell()
    {

        gv_EstimationList.Visible = false;
        ViewState["Report"] = "Job";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT Id,JobNo,CustName,FinalStatus,FinalCost,SiteVisitCharges,OtherCharges,EstimatedQuotation,Componetstatus,CompRecDate,CreatedBy, Convert(varchar,A.CreatedDate,103) as CreatedDate from tblEstimationHdr as A where JobNo='" + txtjob.Text + "' AND isdeleted='0'order by A.CreatedDate", con);
        sad.Fill(dt);
        Exporttoexcelgrid.DataSource = dt;
        Exporttoexcelgrid.EmptyDataText = "Record Not Found";
        Exporttoexcelgrid.DataBind();
    }

    public void GetSortedcutsomerForExcell()
    {
        gv_EstimationList.Visible = false;
        ViewState["Report"] = "Customer";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT Id,JobNo,CustName,FinalStatus,SiteVisitCharges,OtherCharges,EstimatedQuotation,FinalCost,Componetstatus,CompRecDate,CreatedBy, Convert(varchar,A.CreatedDate,103) as CreatedDate from tblEstimationHdr as A where CustName='" + txtcustSearch.Text + "' AND  isdeleted='0' order by A.CreatedDate", con);
        sad.Fill(dt);
        Exporttoexcelgrid.DataSource = dt;
        Exporttoexcelgrid.EmptyDataText = "Record Not Found";
        Exporttoexcelgrid.DataBind();
    }
    public void GetsortedcustomerwithjobForExcel()
    {
        gv_EstimationList.Visible = false;
        ViewState["Report"] = "Customer&Job";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT Id,JobNo,CustName,FinalStatus,Componetstatus,CompRecDate,CreatedBy, Convert(varchar,A.CreatedDate,103) as CreatedDate from tblEstimationHdr as A where CustName='" + txtcustSearch.Text + "' AND JobNo='" + txtjob.Text + "'  AND  isdeleted='0' order by A.CreatedDate", con);
        sad.Fill(dt);
        Exporttoexcelgrid.DataSource = dt;
        Exporttoexcelgrid.EmptyDataText = "Record Not Found";
        Exporttoexcelgrid.DataBind();
    }
}
