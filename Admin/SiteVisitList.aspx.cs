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

public partial class Admin_SiteVisitList : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
    DataTable dt11 = new DataTable();
    UserAuthorization objUserAuthorization = new UserAuthorization();
    string create1, Delete1, Update1, view1, Report1;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GridView();
        }
    }

    void GridView()
    {
        try
        {
            DataTable dt = new DataTable();
            con.Open();
            // SqlDataAdapter sad = new SqlDataAdapter("SELECT [Prodid],[ProdName],[TechSpeci],[ModelNo],SerialNo,[IsStatus],[CreateBy],[CreateDate],[updateBy],[updateDate]FROM [tblProduct] where isdeleted='0' ORDER BY CreateDate", con);
            //   SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM tbl_sitevisit where isdeleted='0' AND ORDER BY Desc", con);
            SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM tbl_sitevisit WHERE isdeleted='0' ORDER BY [CreateDate] DESC;", con);
            sad.Fill(dt);
            gv_Prod.EmptyDataText = "Not Records Found";
            gv_Prod.DataSource = dt;
            gv_Prod.DataBind();
            con.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> Getsite(string prefixText, int count)
    {
        return AutoFillGetsitelist(prefixText);
    }

    public static List<string> AutoFillGetsitelist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT Custname from tbl_sitevisit where " + "Custname like @Search + '%' AND isdeleted='0'";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> CustomerName = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        CustomerName.Add(sdr["Custname"].ToString());
                    }
                }
                con.Close();
                return CustomerName;
            }

        }
    }



    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> Getengineer(string prefixText, int count)
    {
        return AutoFillGetengi(prefixText);
    }

    public static List<string> AutoFillGetengi(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT EngineerName from [tbl_Engineer] where " + "EngineerName like @Search + '%'";

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


    protected void lnkrefresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("SiteVisitList.aspx");
    }

    SqlDataAdapter sadd;


    protected void gv_Prod_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_Prod.PageIndex = e.NewPageIndex;
        GridView();
    }

    protected void btnceate_Click(object sender, EventArgs e)
    {
        Response.Redirect("Sitevisitnew.aspx");
    }

    protected void gv_Prod_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "RowEdit")
        {
            string ash = e.CommandArgument.ToString();
            Response.Redirect("Sitevisitnew.aspx?ID=" + encrypt(e.CommandArgument.ToString()) + "");
        }

        if (e.CommandName == "RowDelete")
        {
            SqlCommand cmddelete = new SqlCommand("UPDATE tblProduct set isdeleted='1' where Prodid=@Prodid", con);
            cmddelete.Parameters.AddWithValue("@Prodid", Convert.ToInt32(e.CommandArgument.ToString()));
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

    protected void txtSearch_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(txtSearch.Text))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Search Site Name');", true);
            }
            if (string.IsNullOrEmpty(txtSearch.Text))
            {
                GridView();
            }
            else
            {
                DataTable dt = new DataTable();

                // SqlDataAdapter sad = new SqlDataAdapter("SELECT [Custid],[CustomerName],[GSTNo],[StateCode],[PanNo],[AddresLine1],[AddresLine2],[AddresLine3],[Area],[Email],[City],[Country],[MobNo],[PostalCode],[ContactPerName1],[ContactPerNo1],[ContactPerName2],[ContactPerNo2],[IsStatus],[CreatedBy],[Createddate],[UpdatedBy],[UpdatedDate] FROM [tblCustomer] where [CustomerName]='" + txtSearch.Text + "' AND isdeleted='0' ", con);
                SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM [tbl_sitevisit] WHERE [Custname]='" + txtSearch.Text + "' AND isdeleted ='0' ", con);
                sad.Fill(dt);
                gv_Prod.EmptyDataText = "Not Records Found";
                gv_Prod.DataSource = dt;
                gv_Prod.DataBind();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        //required to avoid the run time error "  
        //Control 'GridView1' of type 'Grid View' must be placed inside a form tag with runat=server."  
    }

    //protected void btnexportexcel_Click(object sender, EventArgs e)
    //{
    //    // Re-run the filter logic to get the filtered data in DataTable
    //    DataTable dt = new DataTable();
    //    string engineerName = txtengineername.Text;
    //    string query = @"SELECT [ID]
    //      ,[Custname]
    //      ,[location]
    //      ,[Engineername]
    //      ,[ExpensesAmt]
    //      ,[QuotationAmt]
    //      ,[Product]
    //      ,[Date]
    //      ,[Servicetype]
    //      ,[CreateBy]
    //      ,[CreateDate]
    //      ,[UpdateBy]
    //      ,[UpdateDate] FROM [tbl_sitevisit] WHERE isdeleted = '0'";

    //    // Apply filters only if the user has entered search criteria
    //    bool isFiltered = false;

    //    // Check if 'engineerName' is provided
    //    if (!string.IsNullOrEmpty(engineerName))
    //    {
    //        query += @" AND (Engineername = @EngineerName)";
    //        isFiltered = true;
    //    }

    //    // Check if 'from' date is provided
    //    if (!string.IsNullOrEmpty(txtDateSearchfrom.Text))
    //    {
    //        query += " AND Date >= @FromDate"; // Assuming 'VisitDate' is your date column
    //        isFiltered = true;
    //    }

    //    // Check if 'to' date is provided
    //    if (!string.IsNullOrEmpty(txtDateSearchto.Text))
    //    {
    //        query += " AND Date <= @ToDate"; // Assuming 'VisitDate' is your date column
    //        isFiltered = true;
    //    }

    //    SqlDataAdapter sad = new SqlDataAdapter(query, con);

    //    // Use parameters to avoid SQL injection if filters are applied
    //    if (!string.IsNullOrEmpty(engineerName))
    //    {
    //        sad.SelectCommand.Parameters.AddWithValue("@EngineerName", engineerName);
    //    }

    //    // Add date parameters if available
    //    if (!string.IsNullOrEmpty(txtDateSearchfrom.Text))
    //    {
    //        DateTime fromDate;
    //        if (DateTime.TryParse(txtDateSearchfrom.Text, out fromDate))
    //        {
    //            sad.SelectCommand.Parameters.AddWithValue("@FromDate", fromDate);
    //        }
    //    }

    //    if (!string.IsNullOrEmpty(txtDateSearchto.Text))
    //    {
    //        DateTime toDate;
    //        if (DateTime.TryParse(txtDateSearchto.Text, out toDate))
    //        {
    //            sad.SelectCommand.Parameters.AddWithValue("@ToDate", toDate);
    //        }
    //    }

    //    // Fill the DataTable with either filtered or all records based on the search criteria
    //    sad.Fill(dt);

    //    // Now Export this filtered DataTable to Excel
    //    ExportToExcel(dt);
    //}

    protected void btnexportexcel_Click(object sender, EventArgs e)
    {
        // Re-run the filter logic to get the filtered data in DataTable
        DataTable dt = new DataTable();
        string filterValue = txtengineername.Text.Trim();
        string query = @"SELECT [ID]
          ,[Custname]
          ,[location]
          ,[Engineername]
          ,[ExpensesAmt]
          ,[QuotationAmt]
          ,[Product]
          ,[Date]
          ,[Servicetype]
          FROM [tbl_sitevisit] 
          WHERE isdeleted = '0'";

        // Apply filters only if the user has entered search criteria
        bool isFiltered = false;

        // Check if 'engineerName' is provided
        if (!string.IsNullOrEmpty(filterValue))
        {
            query += " AND EngineerName LIKE @EngineerName";
            isFiltered = true;
        }

        // Check if 'from' date is provided
        if (!string.IsNullOrEmpty(txtDateSearchfrom.Text))
        {
            query += " AND Date >= @FromDate"; // Assuming 'VisitDate' is your date column
            isFiltered = true;
        }

        // Check if 'to' date is provided
        if (!string.IsNullOrEmpty(txtDateSearchto.Text))
        {
            query += " AND Date <= @ToDate"; // Assuming 'VisitDate' is your date column
            isFiltered = true;
        }

        SqlDataAdapter sad = new SqlDataAdapter(query, con);

        // Use parameters to avoid SQL injection if filters are applied
        if (!string.IsNullOrEmpty(filterValue))
        {
            sad.SelectCommand.Parameters.AddWithValue("@EngineerName", "%" + filterValue + "%");
        }

        // Add date parameters if available
        if (!string.IsNullOrEmpty(txtDateSearchfrom.Text))
        {
            DateTime fromDate;
            if (DateTime.TryParse(txtDateSearchfrom.Text, out fromDate))
            {
                sad.SelectCommand.Parameters.AddWithValue("@FromDate", fromDate);
            }
        }

        if (!string.IsNullOrEmpty(txtDateSearchto.Text))
        {
            DateTime toDate;
            if (DateTime.TryParse(txtDateSearchto.Text, out toDate))
            {
                sad.SelectCommand.Parameters.AddWithValue("@ToDate", toDate);
            }
        }

        // Fill the DataTable with either filtered or all records based on the search criteria
        sad.Fill(dt);

        // Post-process data to filter comma-separated names
        if (!string.IsNullOrEmpty(filterValue))
        {
            var filteredRows = dt.AsEnumerable().Where(row =>
                row.Field<string>("EngineerName") != null &&
                row.Field<string>("EngineerName")
                    .Split(',')
                    .Select(name => name.Trim().ToLower())
                    .Contains(filterValue.ToLower()));

            // If filter is applied, create a filtered DataTable
            if (filteredRows.Any())
            {
                dt = filteredRows.CopyToDataTable();
            }
            else
            {
                dt.Clear(); // No match, return an empty table
            }
        }

        // Now Export this filtered DataTable to Excel
        ExportToExcel(dt);
    }


    // Function to Export DataTable to Excel
    private void ExportToExcel(DataTable dt)
    {
        string fileName = "Site_Visit_List_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
        Response.ClearContent();
        Response.ContentType = "application/excel";
        Response.AddHeader("content-disposition", "attachment; filename=" + fileName);

        System.IO.StringWriter sw = new System.IO.StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);

        // Create a GridView to export to Excel
        GridView gv = new GridView();
        gv.DataSource = dt;
        gv.DataBind();

        gv.RenderControl(htw);
        Response.Write(sw.ToString());
        Response.End();
    }


    private void ExportGridToExcel()
    {
        //if (ViewState["Excell"] != null)
        //{
        //    string Method = ViewState["Excell"].ToString();

        //    if (Method == "GetDatewiseCustomer")
        //    {
        //        GetDatewiseCustomerforexcell();
        //    }
        //}
        //else
        //{
        //    gv_Prod();
        //}

        Response.Clear();
        Response.Buffer = true;
        Response.ClearContent();
        Response.ClearHeaders();
        Response.Charset = "";
        string FileName = "SiteVisit_List_" + DateTime.Now + ".xls";
        StringWriter strwritter = new StringWriter();
        HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
        gv_Prod.GridLines = GridLines.Both;
        gv_Prod.HeaderStyle.Font.Bold = true;
        gv_Prod.RenderControl(htmltextwrtter);
        Response.Write(strwritter.ToString());
        Response.End();
    }


    //protected void lnkBtnsearch_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        //if (string.IsNullOrEmpty(txtengineername.Text) || )
    //        //if (!string.IsNullOrEmpty(txtengineername.Text) && string.IsNullOrEmpty(txtDateSearchfrom.Text) && string.IsNullOrEmpty(txtDateSearchto.Text))
    //        //{
    //        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Engineer Name');", true);
    //        //}
    //        //if (!string.IsNullOrEmpty(txtengineername.Text) && string.IsNullOrEmpty(txtDateSearchfrom.Text) && string.IsNullOrEmpty(txtDateSearchto.Text))
    //        //{
    //        //    GridView();
    //        //}
    //        //else

    //            DataTable dt = new DataTable();
    //            string engineerName = txtengineername.Text;
    //            string query = @"SELECT * FROM [tbl_sitevisit] 
    //            WHERE Engineername = @EngineerName AND isdeleted = '0'";

    //            // Check if 'from' date is provided
    //            if (!string.IsNullOrEmpty(txtDateSearchfrom.Text))
    //            {
    //                query += " AND Date >= @FromDate"; // Assuming 'VisitDate' is your date column
    //            }

    //            // Check if 'to' date is provided
    //            if (!string.IsNullOrEmpty(txtDateSearchto.Text))
    //            {
    //                query += " AND Date <= @ToDate"; // Assuming 'VisitDate' is your date column
    //            }

    //            SqlDataAdapter sad = new SqlDataAdapter(query, con);

    //            // Use parameters to avoid SQL injection
    //            sad.SelectCommand.Parameters.AddWithValue("@EngineerName", engineerName);

    //            // Add date parameters if available
    //            if (!string.IsNullOrEmpty(txtDateSearchfrom.Text))
    //            {
    //                sad.SelectCommand.Parameters.AddWithValue("@FromDate", DateTime.Parse(txtDateSearchfrom.Text));
    //            }

    //            if (!string.IsNullOrEmpty(txtDateSearchto.Text))
    //            {
    //                sad.SelectCommand.Parameters.AddWithValue("@ToDate", DateTime.Parse(txtDateSearchto.Text));
    //            }

    //            sad.Fill(dt);

    //            gv_Prod.EmptyDataText = "No Records Found";
    //            gv_Prod.DataSource = dt;
    //            gv_Prod.DataBind();

    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    protected void lnkBtnsearch_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            string filterValue = txtengineername.Text.Trim();
            string query = @"SELECT * FROM [tbl_sitevisit] 
                         WHERE isdeleted = '0'";

            // If EngineerName is provided, include it in the query
            if (!string.IsNullOrEmpty(filterValue))
            {
                query += " AND EngineerName LIKE @EngineerName";
            }

            // Check if 'from' date is provided
            if (!string.IsNullOrEmpty(txtDateSearchfrom.Text))
            {
                query += " AND Date >= @FromDate"; // Assuming 'VisitDate' is your date column
            }

            // Check if 'to' date is provided
            if (!string.IsNullOrEmpty(txtDateSearchto.Text))
            {
                query += " AND Date <= @ToDate"; // Assuming 'VisitDate' is your date column
            }

            SqlDataAdapter sad = new SqlDataAdapter(query, con);

            // Use parameters to avoid SQL injection
            if (!string.IsNullOrEmpty(filterValue))
            {
                sad.SelectCommand.Parameters.AddWithValue("@EngineerName", "%" + filterValue + "%");
            }

            // Add date parameters if available
            if (!string.IsNullOrEmpty(txtDateSearchfrom.Text))
            {
                sad.SelectCommand.Parameters.AddWithValue("@FromDate", DateTime.Parse(txtDateSearchfrom.Text));
            }

            if (!string.IsNullOrEmpty(txtDateSearchto.Text))
            {
                sad.SelectCommand.Parameters.AddWithValue("@ToDate", DateTime.Parse(txtDateSearchto.Text));
            }

            sad.Fill(dt);

            // Post-process data to filter comma-separated names
            var filteredRows = dt.AsEnumerable().Where(row =>
                row.Field<string>("EngineerName") != null &&
                row.Field<string>("EngineerName")
                    .Split(',')
                    .Select(name => name.Trim().ToLower())
                    .Contains(filterValue.ToLower()));

            // If filter is applied, create a filtered DataTable
            if (filteredRows.Any())
            {
                dt = filteredRows.CopyToDataTable();
            }
            else
            {
                dt.Clear(); // No match, return an empty table
            }

            gv_Prod.EmptyDataText = "No Records Found";
            gv_Prod.DataSource = dt;
            gv_Prod.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

}