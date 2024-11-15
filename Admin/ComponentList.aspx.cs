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

public partial class Admin_ComponentList : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
    DataTable dt11 = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["adminname"] == null)
            {
                Response.Redirect("../LoginPage.aspx");
            }
            else
            {
                GridView();

                gv_Comp.DataSource = dt11;
                gv_Comp.DataBind();
                this.gv_Comp.Columns[7].Visible = true;

                //string UserCompany = Session["adminname"].ToString();
                //if (UserCompany != "Admin")
                //{
                //    //divform.Visible = false; ComponentStatus();
                //    gvbind_Company();

                //    this.gv_Comp.Columns[7].Visible = false;
                //}
                //else
                //{
                //    //divform.Visible = true; ComponentStatus();
                //    GridView();

                //    gv_Comp.DataSource = dt11;
                //    gv_Comp.DataBind();
                //    this.gv_Comp.Columns[7].Visible = true;

                //}
                //this.txtDateSearch.Text = DateTime.Now.ToString("yyyy-MM-dd");
                //this.txtDateSearch.TextMode = TextBoxMode.Date;
            }

            //DropDownListUnits.Items.Clear();
            //DropDownListUnits.Items.Add("Nos.");
            //DropDownListUnits.Items.Add("Unit.");
            //DropDownListUnits.Items.Add("Pieces.");
            GridView();

            //if (Request.QueryString["Compid"] != null)
            //{
            //    string id = Decrypt(Request.QueryString["Compid"].ToString());
            //    loadData(id);
            //    btnSubmit.Text = "Update";
            //    hidden.Value = id;

            //}
        }
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

    void gvbind_Company()
    {
        try
        {
            string UserCompany = Session["UserCompany"].ToString();
            DataTable dt = new DataTable();
            SqlDataAdapter sad = new SqlDataAdapter("SELECT [Compid],[CompName],[IsStatus],[HSN],[Tax],[Units],[Rate],[CreateBy],[CreateDate],[UpdateBy],[UpdateDate] FROM [tblComponent] where isdeleted='0'", con);
            sad.Fill(dt);
            gv_Comp.EmptyDataText = "Not Records Found";
            gv_Comp.DataSource = dt;
            gv_Comp.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    void GridView()
    {
        try
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlDataAdapter sad = new SqlDataAdapter("SELECT [Compid],[CompName],[IsStatus],[HSN],[Tax],[Units],[Rate],[CreateBy],[CreateDate],[UpdateBy],[UpdateDate] FROM [tblComponent] where isdeleted='0' ORDER BY [CreateDate] Desc", con);
            sad.Fill(dt);
            gv_Comp.EmptyDataText = "Not Records Found";
            gv_Comp.DataSource = dt;
            gv_Comp.DataBind();

            con.Close();
        }
        catch (Exception ex)
        {
            throw ex;
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

    protected void gv_Comp_RowDataBound(object sender, GridViewRowEventArgs e)
    {
		 if (e.Row.RowType == DataControlRowType.DataRow)
        {

           // LinkButton lnkbtnEdit = e.Row.FindControl("lnkbtnEdit") as LinkButton;
           // LinkButton lnkbtnDelete = e.Row.FindControl("lnkbtnDelete") as LinkButton;


            string adminname = Session["adminname"].ToString();

            if (adminname == "Technical")
            {
                DataTable dt = new DataTable();
                SqlDataAdapter sad = new SqlDataAdapter("SELECT [Compid],[CompName],[IsStatus],[HSN],[Tax],[Units],[Rate],[CreateBy],[CreateDate],[UpdateBy],[UpdateDate] FROM [tblComponent] where isdeleted='0' ORDER BY [CreateDate] Desc", con);
                sad.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                   // btncreate.Visible = false;
                    gv_Comp.Columns[2].Visible = false;
                    gv_Comp.Columns[3].Visible = false;
                    gv_Comp.Columns[4].Visible = false;
                    gv_Comp.Columns[5].Visible = false;
                    gv_Comp.Columns[6].Visible = false;
					gv_Comp.Columns[7].Visible = false;


                }
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblisstatus = (Label)e.Row.FindControl("lblisstatus") as Label;

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



    protected void lnkBtnsearch_Click(object sender, EventArgs e)
    {
        try
        {

            if (string.IsNullOrEmpty(txtSearch.Text))
            {

                GridView();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Search Componant');", true);
            }
            else
            {
                ViewState["Excell"] = "Getproduct";
                Getsortedproduct();

                //DataTable dt = new DataTable();

                //SqlDataAdapter sad = new SqlDataAdapter("SELECT [Compid],[CompName],[IsStatus],[HSN],[Tax],[Units],[Rate],[CreateBy],[CreateDate],[UpdateBy],[UpdateDate] FROM [tblComponent] where CompName='" + txtSearch.Text + "' AND [isdeleted] = '0'  ", con);
                //sad.Fill(dt);
                //gv_Comp.EmptyDataText = "Not Records Found";
                //gv_Comp.DataSource = dt;
                //gv_Comp.DataBind();
            }
            //GridView();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    SqlDataAdapter sad;
    protected void ComponentStatus()
    {
        ViewState["Excell"] = "GetsortedproductActice";
        GetsortedproductActice();


        //gv_Comp.Visible = false;
        //try
        //{
        //    DataTable dt = new DataTable();

        //    if (ddlStatus.Text == "All")
        //    {
        //        sad = new SqlDataAdapter("SELECT [Compid],[CompName],[IsStatus],[HSN],[Tax],[Units],[Rate],[CreateBy],[CreateDate],[UpdateBy],[UpdateDate] FROM tblComponent where isdeleted='0'", con);
        //    }
        //    else
        //    {
        //        sad = new SqlDataAdapter("SELECT [Compid],[CompName],[IsStatus],[HSN],[Tax],[Units],[Rate],[CreateBy],[CreateDate],[UpdateBy],[UpdateDate] FROM [tblComponent] where IsStatus='" + ddlStatus.SelectedValue + "' AND isdeleted='0' ", con);
        //    }
        //    sad.Fill(dt);
        //    Sortedcomponetgrid.EmptyDataText = "Not Records Found";
        //    Sortedcomponetgrid.DataSource = dt;
        //    Sortedcomponetgrid.DataBind();

        //    //gv_Comp.EmptyDataText = "Not Records Found";
        //    //gv_Comp.DataSource = dt;
        //    //gv_Comp.DataBind();
        //}
        //catch (Exception)
        //{

        //    throw;
        //}
    }

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetCompList(string prefixText, int count)
    {
        return AutoFillComplist(prefixText);
    }

    public static List<string> AutoFillComplist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT CompName from tblComponent where " + "CompName like @Search + '%' AND isdeleted='0'";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> CompName = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        CompName.Add(sdr["CompName"].ToString());
                    }
                }
                con.Close();
                return CompName;
            }
        }
    }

    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        ComponentStatus();
    }

    protected void gv_Comp_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_Comp.PageIndex = e.NewPageIndex;
        GridView();
    }

    protected void lnkrefresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("ComponentList.aspx");
    }

    protected void btncreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("Component.aspx");
    }

   

    protected void gv_Comp_RowCommand2(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "RowEdit")
        {
            Response.Redirect("Component.aspx?Compid=" + encrypt(e.CommandArgument.ToString()) + "");
        }

        if (e.CommandName == "RowDelete")
        {

            SqlCommand cmddelete = new SqlCommand("update tblComponent set isdeleted='1' where Compid=@Compid", con);
            cmddelete.Parameters.AddWithValue("@Compid", Convert.ToInt32(e.CommandArgument.ToString()));
            cmddelete.Parameters.AddWithValue("@isdeleted", '1');
            con.Open();
            cmddelete.ExecuteNonQuery();
            con.Close();
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Delete Sucessfully');", true);

            //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Delete sucessfully!!');window.location ='CustomerList.aspx';", true);

            GridView();
        }
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
            if (Method == "GetsortedproductActice")
            {
                GetsortedproductActice();
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
        Sortedcomponetgrid.GridLines = GridLines.Both;
        Sortedcomponetgrid.HeaderStyle.Font.Bold = true;
        Sortedcomponetgrid.RenderControl(htmltextwrtter);
        Response.Write(strwritter.ToString());
        Response.End();
    }

    void GridExportExcel()
    {
        try
        {
            DataTable dt = new DataTable();

            SqlDataAdapter sad = new SqlDataAdapter("SELECT [Compid],[CompName],[IsStatus],[HSN],[Tax],[Units],[Rate],[CreateBy],[CreateDate],[UpdateBy],[UpdateDate] FROM [tblComponent] where isdeleted = '0'  ", con);
            sad.Fill(dt);
            Sortedcomponetgrid.EmptyDataText = "Not Records Found";
            Sortedcomponetgrid.DataSource = dt;
            Sortedcomponetgrid.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public void Getsortedproduct()
    {
        gv_Comp.Visible = false;
        ViewState["Record"] = "Job";
        DataTable dt = new DataTable();

        SqlDataAdapter sad = new SqlDataAdapter("SELECT [Compid],[CompName],[IsStatus],[HSN],[Tax],[Units],[Rate],[CreateBy],[CreateDate],[UpdateBy],[UpdateDate] FROM [tblComponent] where CompName='" + txtSearch.Text + "' AND [isdeleted] = '0'  ", con);
        sad.Fill(dt);
        Sortedcomponetgrid.EmptyDataText = "Not Records Found";
        Sortedcomponetgrid.DataSource = dt;
        Sortedcomponetgrid.DataBind();
    }

    public void GetsortedproductActice()
    {


        gv_Comp.Visible = false;
        try
        {
            DataTable dt = new DataTable();
            ViewState["Record"] = "Product";
            if (ddlStatus.Text == "All")
            {
                sad = new SqlDataAdapter("SELECT [Compid],[CompName],[IsStatus],[HSN],[Tax],[Units],[Rate],[CreateBy],[CreateDate],[UpdateBy],[UpdateDate] FROM tblComponent where isdeleted='0'", con);
            }
            else
            {
                sad = new SqlDataAdapter("SELECT [Compid],[CompName],[IsStatus],[HSN],[Tax],[Units],[Rate],[CreateBy],[CreateDate],[UpdateBy],[UpdateDate] FROM [tblComponent] where IsStatus='" + ddlStatus.SelectedValue + "' AND isdeleted='0' ", con);
            }
            sad.Fill(dt);
            Sortedcomponetgrid.EmptyDataText = "Not Records Found";
            Sortedcomponetgrid.DataSource = dt;
            Sortedcomponetgrid.DataBind();
        }
        catch (Exception)
        {

            throw;
        }
    }
}