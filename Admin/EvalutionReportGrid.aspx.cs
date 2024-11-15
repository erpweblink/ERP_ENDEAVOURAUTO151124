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

public partial class Admin_EvalutionReportGrid : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
    string id;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Evalution();

        }
    }

    protected void Evalution()
    {
        try
        {
            DataTable dt = new DataTable();

            con.Open();
            //SqlDataAdapter sad = new SqlDataAdapter("select * FROM tblTestingProduct where isCompleted='1' AND isdeleted='0' ", con);
            SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM tblTestingProduct WHERE isCompleted = '1' AND isdeleted = '0' ORDER BY EntryDate Desc", con);
            sad.Fill(dt);

            gv_Evalution.EmptyDataText = "Not Records Found";
            gv_Evalution.DataSource = dt;
            gv_Evalution.DataBind();

            con.Close();

        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void ShowComponent()
    {
        try
        {
            DataTable dt = new DataTable();

            con.Open();
            SqlDataAdapter sad = new SqlDataAdapter("select * FROM tblRepairingEvalution where JobNo='" + lbljobshow.Text + "' ", con);
            sad.Fill(dt);

            GrdComponent.EmptyDataText = "Component Records not Found";
            GrdComponent.DataSource = dt;
            GrdComponent.DataBind();

            con.Close();

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
                com.CommandText = "select DISTINCT CustomerName from tblTestingProduct where " + "CustomerName like @Search + '%' AND isdeleted='0' AND isCompleted='1'";

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
                com.CommandText = "select DISTINCT EngiName from tblTestingProduct where " + "EngiName like @Search + '%' AND isdeleted='0' AND isCompleted='1'";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> EngiName = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        EngiName.Add(sdr["EngiName"].ToString());
                    }
                }
                con.Close();
                return EngiName;
            }

        }
    }

    protected void lnkBtnsearch_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtjob.Text) && string.IsNullOrEmpty(txtSearchCust.Text) && string.IsNullOrEmpty(txtsearchEngi.Text) && string.IsNullOrEmpty(txtDateSearchfrom.Text) && string.IsNullOrEmpty(txtDateSearchto.Text))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Search Customer Name');", true);

            Evalution();
        }
        else if (!string.IsNullOrEmpty(txtjob.Text) && !string.IsNullOrEmpty(txtSearchCust.Text))
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlDataAdapter sad = new SqlDataAdapter("select [id],[JobNo],[CustomerName],[ModelNo],[SerialNo]," +
                "[EngiName],[TestingDate],[Status],[Remark],[EntryDate],[isCompleted],[isdeleted],[isCancel]," +
                "[CreatedBy],[CreatedDate],[UpdateBy],[UpdatedDate] FROM [tblTestingProduct] where JobNo='" + txtjob.Text + "' AND CustomerName='" + txtSearchCust.Text + "'  AND isCompleted='1' AND isdeleted='0' ", con);
            sad.Fill(dt);

            gv_Evalution.EmptyDataText = "Not Records Found";
            gv_Evalution.DataSource = dt;
            gv_Evalution.DataBind();
            con.Close();
        }

        else if (!string.IsNullOrEmpty(txtjob.Text) && !string.IsNullOrEmpty(txtsearchEngi.Text))
        {
            DataTable dt = new DataTable();

            con.Open();
            SqlDataAdapter sad = new SqlDataAdapter("select [id],[JobNo],[CustomerName],[ModelNo],[SerialNo]," +
                "[EngiName],[TestingDate],[Status],[Remark],[EntryDate],[isCompleted],[isdeleted],[isCancel]," +
                "[CreatedBy],[CreatedDate],[UpdateBy],[UpdatedDate] FROM [tblTestingProduct] where JobNo='" + txtjob.Text + "' AND EngiName='" + txtsearchEngi.Text + "'  AND isCompleted='1' AND isdeleted='0' ", con);
            sad.Fill(dt);

            gv_Evalution.EmptyDataText = "Not Records Found";
            gv_Evalution.DataSource = dt;
            gv_Evalution.DataBind();

            con.Close();
        }

        //Job No filter
        else if (!string.IsNullOrEmpty(txtjob.Text))
        {
            ViewState["Excell"] = "JobNo";
            GetJobno();

            //DataTable dt = new DataTable();

            //con.Open();
            //SqlDataAdapter sad = new SqlDataAdapter("select [id],[JobNo],[CustomerName],[ProductName],[ModelNo],[SerialNo]," +
            //    "[EngiName],[TestingDate],[Status],[Remark],[EntryDate],[isCompleted],[isdeleted],[isCancel]," +
            //    "[CreatedBy],[CreatedDate],[UpdateBy],[UpdatedDate] FROM tblTestingProduct where JobNo='" + txtjob.Text + "'  AND isCompleted='1' AND isdeleted='0' ", con);
            //sad.Fill(dt);

            //gv_Evalution.EmptyDataText = "Not Records Found";
            //gv_Evalution.DataSource = dt;
            //gv_Evalution.DataBind();

            //con.Close();
        }
        //filter customer name
        else if (!string.IsNullOrEmpty(txtSearchCust.Text) && (string.IsNullOrEmpty(txtsearchEngi.Text) && string.IsNullOrEmpty(txtDateSearchto.Text)))
        {
            ViewState["Excell"] = "CustName";
            GetCustomer();

            //DataTable dt = new DataTable();

            //con.Open();
            //SqlDataAdapter sad = new SqlDataAdapter("select [id],[JobNo],[CustomerName],[ProductName],[ModelNo],[SerialNo]," +
            //    "[EngiName],[TestingDate],[Status],[Remark],[EntryDate],[isCompleted],[isdeleted],[isCancel]," +
            //    "[CreatedBy],[CreatedDate],[UpdateBy],[UpdatedDate] FROM [tblTestingProduct] where CustomerName='" + txtSearchCust.Text + "'  AND isCompleted='1' AND isdeleted='0' ", con);
            //sad.Fill(dt);
            //ViewState["Record"] = "Customer";
            //gv_Evalution.EmptyDataText = "Not Records Found";
            //gv_Evalution.DataSource = dt;
            //gv_Evalution.DataBind();

            //con.Close();
        }

        //Engineear Filter
        else if (!string.IsNullOrEmpty(txtsearchEngi.Text) && string.IsNullOrEmpty(txtDateSearchto.Text))
        {
            ViewState["Excell"] = "EngiName";
            GetEngineer();
            //GetEngineerexcel();

            //DataTable dt = new DataTable();

            //con.Open();
            //SqlDataAdapter sad = new SqlDataAdapter("select [id],[JobNo],[CustomerName],[ModelNo],[SerialNo]," +
            //    "[EngiName],[TestingDate],[Status],[Remark],[EntryDate],[isCompleted],[isdeleted],[isCancel]," +
            //    "[CreatedBy],[CreatedDate],[UpdateBy],[UpdatedDate],ProductName FROM [tblTestingProduct] where EngiName='" + txtsearchEngi.Text + "'  AND isCompleted='1' AND isdeleted='0' ", con);
            //sad.Fill(dt);
            //ViewState["Record"] = "EngiName";
            //gv_Evalution.EmptyDataText = "Not Records Found";
            //gv_Evalution.DataSource = dt;
            //gv_Evalution.DataBind();

            //con.Close();
        }
        else if (!string.IsNullOrEmpty(txtDateSearchfrom.Text) && string.IsNullOrEmpty(txtDateSearchto.Text))
        {
            DataTable dt = new DataTable();

            con.Open();
            SqlDataAdapter sad = new SqlDataAdapter("select * from tblTestingProduct where EntryDate='" + txtDateSearchfrom.Text + "' AND isdeleted = '0' AND isCompleted = '1' ", con);
            sad.Fill(dt);
            ViewState["Record"] = "FromDate";
            gv_Evalution.EmptyDataText = "Not Records Found";
            gv_Evalution.DataSource = dt;
            gv_Evalution.DataBind();

            con.Close();
            //Evalution();
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Enter To Date!!');", true);

        }
        else if (string.IsNullOrEmpty(txtDateSearchfrom.Text) && !string.IsNullOrEmpty(txtDateSearchto.Text))
        {
            DataTable dt = new DataTable();

            con.Open();
            SqlDataAdapter sad = new SqlDataAdapter("select * from tblTestingProduct where EntryDate ='" + txtDateSearchto.Text + "' AND isdeleted = '0' AND isCompleted = '1' ", con);
            sad.Fill(dt);

            gv_Evalution.EmptyDataText = "Not Records Found";
            gv_Evalution.DataSource = dt;
            gv_Evalution.DataBind();

            con.Close();
            //Evalution();
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Enter From DateFrom Date!!');", true);

        }

        //FROM DATE TO DATE 
        else if (!string.IsNullOrEmpty(txtDateSearchfrom.Text) && !string.IsNullOrEmpty(txtDateSearchto.Text) && (string.IsNullOrEmpty(txtsearchEngi.Text) && (string.IsNullOrEmpty(txtSearchCust.Text))))
        {
            ViewState["Excell"] = "DateWise";
            GetDatewise();

            //DataTable dt = new DataTable();

            //con.Open();
            //SqlDataAdapter sad = new SqlDataAdapter("select * from tblTestingProduct where EntryDate between '" + txtDateSearchfrom.Text + "' AND  '" + txtDateSearchto.Text + "' AND isdeleted = '0' AND isCompleted = '1' ", con);
            //sad.Fill(dt);
            //ViewState["Record"] = "Datewise";
            //gv_Evalution.EmptyDataText = "Not Records Found";
            //gv_Evalution.DataSource = dt;
            //gv_Evalution.DataBind();

            //con.Close();

        }

        else if (!string.IsNullOrEmpty(txtDateSearchfrom.Text) && !string.IsNullOrEmpty(txtDateSearchto.Text) && (!string.IsNullOrEmpty(txtsearchEngi.Text)))
        {
            ViewState["Excell"] = "EngDateWise";
            GetEngDatewise();


            //DataTable dt = new DataTable();

            //con.Open();
            //SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM tblTestingProduct WHERE EntryDate BETWEEN '" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "' AND EngiName='" + txtsearchEngi.Text + "' AND isdeleted = 0 AND isCompleted = 1", con);

            //sad.Fill(dt);
            //ViewState["Record"] = "EngDateWise";
            //gv_Evalution.EmptyDataText = "Not Records Found";
            //gv_Evalution.DataSource = dt;
            //gv_Evalution.DataBind();

            //con.Close();
        }

        else if (!string.IsNullOrEmpty(txtDateSearchfrom.Text) && !string.IsNullOrEmpty(txtDateSearchto.Text) && (!string.IsNullOrEmpty(txtSearchCust.Text)))
        {
            ViewState["Excell"] = "CustDateWise";
            GetDatewiseCustomer();

            //DataTable dt = new DataTable();
            //con.Open();
            //SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM tblTestingProduct WHERE EntryDate BETWEEN '" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "' AND CustomerName='" + txtSearchCust.Text + "' AND isdeleted = 0 AND isCompleted = 1", con);

            //sad.Fill(dt);
            //ViewState["Record"] = "CustomerDateWise";
            //gv_Evalution.EmptyDataText = "Not Records Found";
            //gv_Evalution.DataSource = dt;
            //gv_Evalution.DataBind();

            //con.Close();
        }

    }

    protected void lnkrefresh_Click(object sender, EventArgs e)
    {
        txtjob.Text = ""; txtSearchCust.Text = ""; txtsearchEngi.Text = ""; txtDateSearchfrom.Text = ""; txtDateSearchto.Text = "";

        if (string.IsNullOrEmpty(txtjob.Text) && string.IsNullOrEmpty(txtSearchCust.Text) && string.IsNullOrEmpty(txtsearchEngi.Text) && string.IsNullOrEmpty(txtDateSearchfrom.Text) && string.IsNullOrEmpty(txtDateSearchto.Text))
        {
            Evalution();
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
                com.CommandText = "select DISTINCT JobNo from tblTestingProduct where " + "JobNo like @Search + '%' AND isdeleted='0' AND isCompleted='1'";

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

    protected void lnkshow_Click(object sender, EventArgs e)
    {
        int rowIndex = Convert.ToInt32(((sender as LinkButton).NamingContainer as GridViewRow).RowIndex);
        GridViewRow row = gv_Evalution.Rows[rowIndex];

        lbljobshow.Text = (row.FindControl("lblJobno") as Label).Text;
        ShowComponent();
        //lblCompNameShow.Text= (row.FindControl("lblCompName") as Label).Text;
        lblmodelNoshow.Text = (row.FindControl("lblModelNo") as Label).Text;
        lblSerialNoshow.Text = (row.FindControl("lblSriNo") as Label).Text;
        lblTestingDateshow.Text = (row.FindControl("lblTestingDate") as Label).Text;
        lblcustomershow.Text = (row.FindControl("lblCustname") as Label).Text;
        lblEntryDateshow.Text = (row.FindControl("lblEntryDate") as Label).Text;
        lblEngiNameshow.Text = (row.FindControl("lblEngineer") as Label).Text;
        lblstatusshow.Text = (row.FindControl("lblsttaus") as Label).Text;
        lblRemarkshow.Text = (row.FindControl("lblRemark") as Label).Text;
        lblproductshow.Text = (row.FindControl("lblproduct") as Label).Text;
        modelprofile.Show();
    }

    protected void gv_Evalution_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (ViewState["Record"] != null)
        {
            if (ViewState["Record"].ToString() == "JobNo")
            {
                gv_Evalution.PageIndex = e.NewPageIndex;
                GetJobno();
            }
            if (ViewState["Record"].ToString() == "EngiName")
            {
                gv_Evalution.PageIndex = e.NewPageIndex;
                GetEngineer();
            }
            if (ViewState["Record"].ToString() == "Customer")
            {
                gv_Evalution.PageIndex = e.NewPageIndex;
                GetCustomer();
            }

            if (ViewState["Record"].ToString() == "FromDate")
            {
                gv_Evalution.PageIndex = e.NewPageIndex;
                GetParticularDate();
            }
            if (ViewState["Record"].ToString() == "FromDate")
            {
                gv_Evalution.PageIndex = e.NewPageIndex;
                GetParticularDate();
            }
            if (ViewState["Record"].ToString() == "DateWise")
            {
                gv_Evalution.PageIndex = e.NewPageIndex;
                GetDatewise();
            }
            if (ViewState["Record"].ToString() == "EngDateWise")
            {
                gv_Evalution.PageIndex = e.NewPageIndex;
                GetEngDatewise();
            }
            if (ViewState["Record"].ToString() == "CustDateWise")
            {
                gv_Evalution.PageIndex = e.NewPageIndex;
                GetDatewiseCustomer();
            }
          

        }
        else

        {
            gv_Evalution.PageIndex = e.NewPageIndex;
            Evalution();

        }
    }

    protected void gv_Evalution_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (Session["name"].ToString() == "Admin")
        {
            gv_Evalution.Columns[4].Visible = true;
            cust.Visible = true;
            cust1.Visible = true;
        }
        else
        {
            cust.Visible = false;
            cust1.Visible = false;
            gv_Evalution.Columns[4].Visible = false;
        }
    }

    private void GetJobno()
    {
        ViewState["Record"] = "JobNo";
        ViewState["Excell"] = "JobNo";
        DataTable dt = new DataTable();

        con.Open();
        SqlDataAdapter sad = new SqlDataAdapter("select [id],[JobNo],[CustomerName],[ProductName],[ModelNo],[SerialNo]," +
            "[EngiName],[TestingDate],[Status],[Remark],[EntryDate],[isCompleted],[isdeleted],[isCancel]," +
            "[CreatedBy],[CreatedDate],[UpdateBy],[UpdatedDate] FROM tblTestingProduct where JobNo='" + txtjob.Text + "'  AND isCompleted='1' AND isdeleted='0' ", con);
        sad.Fill(dt);

        gv_Evalution.EmptyDataText = "Not Records Found";
        gv_Evalution.DataSource = dt;
        gv_Evalution.DataBind();

        con.Close();
    }

    public void GetEngineer()
    {

        ViewState["Record"] = "EngiName";
        ViewState["Excell"] = "EngiName";
        DataTable dt = new DataTable();
        con.Open();
        SqlDataAdapter sad = new SqlDataAdapter("select [id],[JobNo],[CustomerName],[ModelNo],[SerialNo]," +
           "[EngiName],[TestingDate],[Status],[Remark],[EntryDate],[isCompleted],[isdeleted],[isCancel]," +
           "[CreatedBy],[CreatedDate],[UpdateBy],[UpdatedDate],ProductName FROM [tblTestingProduct] where EngiName='" + txtsearchEngi.Text + "'  AND isCompleted='1' AND isdeleted='0' ", con);
        sad.Fill(dt);

        gv_Evalution.EmptyDataText = "Not Records Found";
        gv_Evalution.DataSource = dt;
        gv_Evalution.DataBind();

        con.Close();
    }

    public void GetCustomer()
    {

        ViewState["Record"] = "CustName";
        ViewState["Excell"] = "CustName";
        DataTable dt = new DataTable();

        con.Open();
        SqlDataAdapter sad = new SqlDataAdapter("select [id],[JobNo],[CustomerName],[ProductName],[ModelNo],[SerialNo]," +
            "[EngiName],[TestingDate],[Status],[Remark],[EntryDate],[isCompleted],[isdeleted],[isCancel]," +
            "[CreatedBy],[CreatedDate],[UpdateBy],[UpdatedDate] FROM [tblTestingProduct] where CustomerName='" + txtSearchCust.Text + "'  AND isCompleted='1' AND isdeleted='0' ", con);
        sad.Fill(dt);
        ViewState["Record"] = "Customer";
        gv_Evalution.EmptyDataText = "Not Records Found";
        gv_Evalution.DataSource = dt;
        gv_Evalution.DataBind();

        con.Close();
    }

    public void GetParticularDate()
    {
        DataTable dt = new DataTable();

        con.Open();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblTestingProduct where EntryDate='" + txtDateSearchfrom.Text + "' AND isdeleted = '0' AND isCompleted = '1' ", con);
        sad.Fill(dt);
        gv_Evalution.EmptyDataText = "Not Records Found";
        gv_Evalution.DataSource = dt;
        gv_Evalution.DataBind();
        con.Close();
    }

    public void GetDatewise()
    {
        ViewState["Record"] = "DateWise";
        ViewState["Excell"] = "DateWise";

        DataTable dt = new DataTable();

        con.Open();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblTestingProduct where EntryDate between '" + txtDateSearchfrom.Text + "' AND  '" + txtDateSearchto.Text + "' AND isdeleted = '0' AND isCompleted = '1' ", con);
        sad.Fill(dt);

        gv_Evalution.EmptyDataText = "Not Records Found";
        gv_Evalution.DataSource = dt;
        gv_Evalution.DataBind();

        con.Close();
    }

    public void GetEngDatewise()
    {

        ViewState["Record"] = "EngDateWise";
        ViewState["Excell"] = "EngDateWise";

        DataTable dt = new DataTable();
        con.Open();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM tblTestingProduct WHERE EntryDate BETWEEN '" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "' AND EngiName='" + txtsearchEngi.Text + "' AND isdeleted = 0 AND isCompleted = 1", con);

        sad.Fill(dt);
        gv_Evalution.EmptyDataText = "Not Records Found";
        gv_Evalution.DataSource = dt;
        gv_Evalution.DataBind();
        con.Close();
    }

    public void GetDatewiseCustomer()
    {
        ViewState["Record"] = "CustDateWise";
        ViewState["Excell"] = "CustDateWise";

        DataTable dt = new DataTable();
        con.Open();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM tblTestingProduct WHERE EntryDate BETWEEN '" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "' AND CustomerName='" + txtSearchCust.Text + "' AND isdeleted = 0 AND isCompleted = 1", con);
        sad.Fill(dt);
        gv_Evalution.EmptyDataText = "Not Records Found";
        gv_Evalution.DataSource = dt;
        gv_Evalution.DataBind();

        con.Close();
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        //required to avoid the run time error "
        //Control 'GridView1' of type 'Grid View' must be placed inside a form tag with runat=server."
    }

    private void GetJobnoExcel()
    {
        ViewState["Record"] = "JobNo";
        ViewState["Excell"] = "JobNo";
        DataTable dt = new DataTable();

        con.Open();
        SqlDataAdapter sad = new SqlDataAdapter("select [id],[JobNo],[CustomerName],[ProductName],[ModelNo],[SerialNo]," +
            "[EngiName],[TestingDate],[Status],[Remark],[EntryDate],[isCompleted],[isdeleted],[isCancel]," +
            "[CreatedBy],[CreatedDate],[UpdateBy],[UpdatedDate] FROM tblTestingProduct where JobNo='" + txtjob.Text + "'  AND isCompleted='1' AND isdeleted='0' ", con);
        sad.Fill(dt);

        sortedgv.EmptyDataText = "Not Records Found";
        sortedgv.DataSource = dt;
        sortedgv.DataBind();

        con.Close();
    }

    public void GetEngineerexcel()
    {
        gv_Evalution.Visible = false;
        ViewState["Excell"] = "EngiName";
        DataTable dt = new DataTable();
        con.Open();
        SqlDataAdapter sad = new SqlDataAdapter("select [id],[JobNo],[CustomerName],[ModelNo],[SerialNo]," +
           "[EngiName],[TestingDate],[Status],[Remark],[EntryDate],[isCompleted],[isdeleted],[isCancel]," +
           "[CreatedBy],[CreatedDate],[UpdateBy],[UpdatedDate],ProductName FROM [tblTestingProduct] where EngiName='" + txtsearchEngi.Text + "'  AND isCompleted='1' AND isdeleted='0' ", con);
        sad.Fill(dt);

        sortedgv.EmptyDataText = "Not Records Found";
        sortedgv.DataSource = dt;
        sortedgv.DataBind();
        con.Close();
    }

    public void Getjobnoxcel()
    {
        ViewState["Excell"] = "JobNo";
        DataTable dt = new DataTable();

        con.Open();
        SqlDataAdapter sad = new SqlDataAdapter("select [id],[JobNo],[CustomerName],[ProductName],[ModelNo],[SerialNo]," +
            "[EngiName],[TestingDate],[Status],[Remark],[EntryDate],[isCompleted],[isdeleted],[isCancel]," +
            "[CreatedBy],[CreatedDate],[UpdateBy],[UpdatedDate] FROM tblTestingProduct where JobNo='" + txtjob.Text + "'  AND isCompleted='1' AND isdeleted='0' ", con);
        sad.Fill(dt);

        sortedgv.EmptyDataText = "Not Records Found";
        sortedgv.DataSource = dt;
        sortedgv.DataBind();

        con.Close();
    }

    public void GetCustomerExcel()
    {
        ViewState["Excell"] = "CustName";
        DataTable dt = new DataTable();

        con.Open();
        SqlDataAdapter sad = new SqlDataAdapter("select [id],[JobNo],[CustomerName],[ProductName],[ModelNo],[SerialNo]," +
            "[EngiName],[TestingDate],[Status],[Remark],[EntryDate],[isCompleted],[isdeleted],[isCancel]," +
            "[CreatedBy],[CreatedDate],[UpdateBy],[UpdatedDate] FROM [tblTestingProduct] where CustomerName='" + txtSearchCust.Text + "'  AND isCompleted='1' AND isdeleted='0' ", con);
        sad.Fill(dt);
        ViewState["Record"] = "Customer";
        sortedgv.EmptyDataText = "Not Records Found";
        sortedgv.DataSource = dt;
        sortedgv.DataBind();

        con.Close();
    }

    public void GetDatewiseExcel()
    {
        ViewState["Record"] = "DateWise";
        ViewState["Excell"] = "DateWise";

        DataTable dt = new DataTable();

        con.Open();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblTestingProduct where EntryDate between '" + txtDateSearchfrom.Text + "' AND  '" + txtDateSearchto.Text + "' AND isdeleted = '0' AND isCompleted = '1' ", con);
        sad.Fill(dt);

        sortedgv.EmptyDataText = "Not Records Found";
        sortedgv.DataSource = dt;
        sortedgv.DataBind();

        con.Close();
    }


    public void GetEngDatewiseExcel()
    {
        ViewState["Record"] = "EngDateWise";
        ViewState["Excell"] = "EngDateWise";

        DataTable dt = new DataTable();
        con.Open();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM tblTestingProduct WHERE EntryDate BETWEEN '" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "' AND EngiName='" + txtsearchEngi.Text + "' AND isdeleted = 0 AND isCompleted = 1", con);

        sad.Fill(dt);
        sortedgv.EmptyDataText = "Not Records Found";
        sortedgv.DataSource = dt;
        sortedgv.DataBind();
        con.Close();
    }

    public void GetDatewiseCustomerExcel()
    {
        ViewState["Record"] = "CustDateWise";
        ViewState["Excell"] = "CustDateWise";

        DataTable dt = new DataTable();
        con.Open();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM tblTestingProduct WHERE EntryDate BETWEEN '" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "' AND CustomerName='" + txtSearchCust.Text + "' AND isdeleted = 0 AND isCompleted = 1", con);
        sad.Fill(dt);
        sortedgv.EmptyDataText = "Not Records Found";
        sortedgv.DataSource = dt;
        sortedgv.DataBind();

        con.Close();
    }

    protected void btnExportExcel_Click(object sender, EventArgs e)
    {
        ExportGridToExcel();
    }

    private void ExportGridToExcel()
    {

        if (ViewState["Excell"] != null)
        {
            gv_Evalution.Visible = false;
            string Method = ViewState["Excell"].ToString();

            if (Method == "EngiName")
            {
                GetEngineerexcel();
            }
            if (Method == "JobNo")
            {
                GetJobnoExcel();
            }
            if (Method == "CustName")
            {
                GetCustomerExcel();
            }
            if (Method == "DateWise")
            {
                GetDatewiseExcel();
            }
            if (Method == "EngDateWise")
            {
                GetEngDatewiseExcel();
            }
            if (Method == "CustDateWise")
            {
                GetDatewiseCustomerExcel();
            }

            

            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";
            string FileName = "Inward_Entry_List_" + DateTime.Now + ".xls";
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
    }
}