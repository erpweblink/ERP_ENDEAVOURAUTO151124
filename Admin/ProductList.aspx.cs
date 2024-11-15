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

public partial class Admin_ProductList : System.Web.UI.Page
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

        //if (!IsPostBack)
        //{

        //    GridView();
        //    if (Request.QueryString["Prodid"] != null)
        //    {
        //        string id = Decrypt(Request.QueryString["Prodid"].ToString());

        //        hidden.Value = id;
        //    }
        //    else
        //    {

        //    }
        //}

        //GridView();
    }

    void GridView()
    {
        try
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlDataAdapter sad = new SqlDataAdapter("SELECT [Prodid],[ProdName],[TechSpeci],[ModelNo],SerialNo,[IsStatus],[CreateBy],[CreateDate],[updateBy],[updateDate]FROM [tblProduct] where isdeleted='0' ORDER BY CreateDate", con);
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

    protected void lnkBtnsearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(txtSearch.Text))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Search Product Name');", true);

            }
            if (string.IsNullOrEmpty(txtSearch.Text))
            {

                GridView();
                // UserAuthorization();
            }
            else
            {

                DataTable dt = new DataTable();

                SqlDataAdapter sad = new SqlDataAdapter("SELECT [Prodid],[ProdName],[TechSpeci],[ModelNo],SerialNo,[IsStatus],[CreateBy],[CreateDate],[updateBy],[updateDate]FROM [tblProduct] where ProdName='" + txtSearch.Text + "' AND isdeleted='0'", con);
                sad.Fill(dt);
                gv_Prod.EmptyDataText = "Not Records Found";
                gv_Prod.DataSource = dt;
                gv_Prod.DataBind();
                //UserAuthorization();
            }
            //GridView();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void lnkrefresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("ProductList.aspx");
    }

    SqlDataAdapter sadd;
    protected void ListStatus()
    {
        try
        {
            DataTable dt = new DataTable();

            if (ddlStatus.Text == "All")
            {
                sadd = new SqlDataAdapter("SELECT [Prodid],[ProdName],[TechSpeci],[ModelNo],SerialNo,[IsStatus],[CreateBy],[CreateDate],[updateBy],[updateDate]FROM [tblProduct] where isdeleted='0'", con);
            }
            else
            {
                sadd = new SqlDataAdapter("SELECT [Prodid],[ProdName],[TechSpeci],[ModelNo],SerialNo,[IsStatus],[CreateBy],[CreateDate],[updateBy],[updateDate]FROM [tblProduct] where IsStatus='" + ddlStatus.Text + "' AND isdeleted='0'", con);
            }
            sadd.Fill(dt);
            gv_Prod.EmptyDataText = "Not Records Found";
            gv_Prod.DataSource = dt;
            gv_Prod.DataBind();
            // UserAuthorization();
        }
        catch (Exception)
        {

            throw;
        }
    }

    

    protected void ddlStatus_TextChanged(object sender, EventArgs e)
    {
        ListStatus();
    }

    protected void gv_Prod_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "RowEdit")
        {
            string ash  = e.CommandArgument.ToString();
            Response.Redirect("Product.aspx?Prodid=" + encrypt(e.CommandArgument.ToString()) + "");
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

    protected void gv_Prod_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblisstatus = (Label)e.Row.FindControl("lblIsStatus") as Label;

            if (lblisstatus.Text == "True")

            {
                lblisstatus.Text = "Active";
            }

            else

            {

                lblisstatus.Text = "DeActive";

            }

        }
    }

    protected void lnkBtnsearch_Click1(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(txtSearch.Text))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Search Product Name');", true);

            }
            if (string.IsNullOrEmpty(txtSearch.Text))
            {

                GridView();
                // UserAuthorization();
            }
            else
            {
                ViewState["Excell"] = "Getproduct";            
                Getsortedproduct();

                //DataTable dt = new DataTable();

                //SqlDataAdapter sad = new SqlDataAdapter("SELECT [Prodid],[ProdName],[TechSpeci],[ModelNo],SerialNo,[IsStatus],[CreateBy],[CreateDate],[updateBy],[updateDate]FROM [tblProduct] where ProdName='" + txtSearch.Text + "' AND isdeleted='0'", con);
                //sad.Fill(dt);
                //gv_Prod.EmptyDataText = "Not Records Found";
                //gv_Prod.DataSource = dt;
                //gv_Prod.DataBind();

            }


            //GridView();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetProdList(string prefixText, int count)
    {
        return AutoFillProdlist(prefixText);
    }

    public static List<string> AutoFillProdlist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT ProdName from tblProduct where " + "ProdName like @Search + '%' AND isdeleted='0'";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();                                                                                                 
                List<string> ProdName = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        ProdName.Add(sdr["ProdName"].ToString());
                    }
                }
                con.Close();
                return ProdName;
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

    protected void gv_Prod_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_Prod.PageIndex = e.NewPageIndex;
        GridView();
    }

    protected void btnceate_Click(object sender, EventArgs e)
    {
        Response.Redirect("Product.aspx");
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        //required to avoid the run time error "  
        //Control 'GridView1' of type 'Grid View' must be placed inside a form tag with runat=server."  
    }

    protected void btnexportexcel_Click(object sender, EventArgs e)
    {
        ExportGridToExcel();
    }

    private void ExportGridToExcel()
    {
        if (ViewState["Excell"] != null)
        {
            string Method = ViewState["Excell"].ToString();

            if (Method == "Getproduct")
            {
                Getsortedproduct();
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
        string FileName = "Product_List_" + DateTime.Now + ".xls";
        StringWriter strwritter = new StringWriter();
        HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
        sortrdgridproduct.GridLines = GridLines.Both;
        sortrdgridproduct.HeaderStyle.Font.Bold = true;
        sortrdgridproduct.RenderControl(htmltextwrtter);
        Response.Write(strwritter.ToString());
        Response.End();
    }

    void GridExportExcel()
    {
        try
        {
            DataTable dt = new DataTable();

            //SqlDataAdapter sad = new SqlDataAdapter("SELECT [Prodid],[ProdName],[TechSpeci],[ModelNo],SerialNo,[IsStatus],[CreateBy],[CreateDate],[updateBy],[updateDate]FROM [tblProduct] where ProdName='" + txtSearch.Text + "' AND isdeleted='0'", con);
            SqlDataAdapter sad = new SqlDataAdapter("SELECT [Prodid],[ProdName],[TechSpeci],[ModelNo],SerialNo,[IsStatus],[CreateBy],[CreateDate],[updateBy],[updateDate]FROM [tblProduct] where isdeleted='0'", con);
            sad.Fill(dt);
            sortrdgridproduct.EmptyDataText = "Not Records Found";
            sortrdgridproduct.DataSource = dt;
            sortrdgridproduct.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }



    

    public void Getsortedproduct()
    {
        //gv_Inward.Visible = false;
        //ViewState["Record"] = "Job";
        //string jobno = txtJobNo.Text;
        //DataTable dt = new DataTable();
        //sad = new SqlDataAdapter("select Id,JobNo,DateIn,CustName,MateName,SrNo,Subcustomer,MateStatus,[otherinfo],[Imagepath],FinalStatus,TestBy,CreatedBy,CreatedDate,RepeatedNo,Branch,ModelNo,ProductFault, DATEDIFF(DAY, DateIn, getdate()) AS days from tblInwardEntry where JobNo= '" + jobno + "'", con);
        //sad.Fill(dt);
        //sortedgv.EmptyDataText = "Not Records Found";
        //sortedgv.DataSource = dt;
        //sortedgv.DataBind();


        gv_Prod.Visible = false;
        ViewState["Record"] = "Job";
        DataTable dt = new DataTable();

        SqlDataAdapter sad = new SqlDataAdapter("SELECT [Prodid],[ProdName],[TechSpeci],[ModelNo],SerialNo,[IsStatus],[CreateBy],[CreateDate],[updateBy],[updateDate]FROM [tblProduct] where ProdName='" + txtSearch.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        sortrdgridproduct.EmptyDataText = "Not Records Found";
        sortrdgridproduct.DataSource = dt;
        sortrdgridproduct.DataBind();
    }
}