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
using System.Collections;

public partial class Reception_Product : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
    DataTable dt11 = new DataTable();
    DataTable dtpers = new DataTable();
    UserAuthorization objUserAuthorization = new UserAuthorization();
    string create1, Delete1, Update1, view1, Report1;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //if (Session["UserCompany"] == null)
            //{
            //    Response.Redirect("../LoginPage.aspx");
            //}
            //else
            //{
            //    string UserCompany = Session["UserCompany"].ToString();
            //    if (UserCompany != "ENDEAVOUR AUTOMATION")
            //    {
            //        divform.Visible = false;
            //        gvbind_Company(); ListStatus();
            //        this.gv_Prod.Columns[6].Visible = false;
            //    }
            //    else
            //    {
            //        divform.Visible = true;
            //        GridView();
            //        gv_Prod.DataSource = dt11;
            //        gv_Prod.DataBind(); ListStatus();
            //        this.gv_Prod.Columns[6].Visible = true;

            //    }
            //    //this.txtDateSearch.Text = DateTime.Now.ToString("yyyy-MM-dd");
            //    //this.txtDateSearch.TextMode = TextBoxMode.Date;
            //}
            //GridView();
            // UserAuthorization();
    

            if (Request.QueryString["Prodid"] != null)
            {
                string id = Decrypt(Request.QueryString["Prodid"].ToString());
                loadData(id);
                btnSubmit.Text = "Update";
                hidden.Value = id;
            }
        }
    }


    void gvbind_Company()
    {
        try
        {
            string UserCompany = Session["UserCompany"].ToString();
            DataTable dt = new DataTable();
            SqlDataAdapter sad = new SqlDataAdapter("SELECT [Prodid],[ProdName],[TechSpeci],[ModelNo],SerialNo,[IsStatus],[CreateBy],[CreateDate],[updateBy],[updateDate]FROM [tblProduct] where isdeleted='0'", con);
            sad.Fill(dt);
        
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //Category Autocomplated
    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetCategoryList(string prefixText, int count)
    {
        return AutoFillGetCategoryList(prefixText);
    }
    public static List<string> AutoFillGetCategoryList(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "  select DISTINCT CategoryName from [tbl_Category] where " + "CategoryName like @Search + '%'";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> CategoryName = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        CategoryName.Add(sdr["CategoryName"].ToString());
                    }
                }
                con.Close();
                return CategoryName;
            }
        }
    }



    //Rating Autocomplated
    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetRatingList(string prefixText, int count)
    {
        return AutoGetRatingListList(prefixText);
    }
    public static List<string> AutoGetRatingListList(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "  select DISTINCT RatingName from [tbl_Rating] where " + "RatingName like @Search + '%'";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> RatingName = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        RatingName.Add(sdr["RatingName"].ToString());
                    }
                }
                con.Close();
                return RatingName;
            }
        }
    }

    //protected void UserAuthorization()
    //{
    //    string pageName = "Product.aspx";

    //    ArrayList arrlist = new ArrayList();
    //    arrlist = objUserAuthorization.GetPageAuthori(pageName);
    //    if (arrlist.Count > 0)
    //    {
    //        create1 = (string)arrlist[0];
    //        Delete1 = (string)arrlist[1];
    //        Update1 = (string)arrlist[2];
    //        view1 = (string)arrlist[3];
    //        Report1 = (string)arrlist[4];

    //        foreach (GridViewRow g1 in gv_Prod.Rows)
    //        {

    //            LinkButton lnkdelete = (g1.FindControl("lnkbtnDelete") as LinkButton);
    //            LinkButton lnkedit = (g1.FindControl("lnkbtnEdit") as LinkButton);

    //            btnSubmit.Visible = create1 == "True" ? true : false;
    //            lnkdelete.Visible = Delete1 == "True" ? true : false;
    //            lnkedit.Visible = Update1 == "True" ? true : false;

    //            divText.Visible = view1 == "True" ? true : false;
    //            //divlnkserch.Visible = view1 == "True" ? true : false;
    //            //divgrid.Visible = view1 == "True" ? true : false;

    //            //btnSubmit.Visible = Report1 == "True" ? true : false;


    //            lblmsgcre.Text = create1 == "True" ? "" : "You have not authorised to create Vendor";
    //            lblmsgdel.Text = Delete1 == "True" ? "" : "You have not authorised to Delete Vendor";
    //            lblmsgupd.Text = Update1 == "True" ? "" : "You have not authorised to Update Vendor";
    //            lblmsgview.Text = view1 == "True" ? "" : "You have not authorised to View  Vendor List";
    //            //lblmsgrepor.Text = Report1 == "True" ? "" : "You have not authorised to Delete Vendor";
    //            if (create1 == "True" && Delete1 == "True" && Update1 == "True" && view1 == "True")
    //            {
    //                popup.Visible = false;
    //            }
    //            else
    //            {
    //                popup.Visible = true;
    //            }

    //        }
    //        if (gv_Prod.Rows.Count <= 0)
    //        {
    //            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Record Not Found !!!');", true);
    //            btnSubmit.Visible = create1 == "True" ? true : false;
    //            divText.Visible = false;
    //            //divlnkserch.Visible = false;
    //            //divgrid.Visible = false;
    //            popup.Visible = false;
    //        }
    //    }
    //    else
    //    {
    //        divText.Visible = false;
    //        //divlnkserch.Visible = false;
    //        //divgrid.Visible = false;
    //        btnSubmit.Visible = false;
    //        lblmsgcre.Text = "You have not authorised to create Vendor";
    //        lblmsgdel.Text = "You have not authorised to Delete Vendor";
    //        lblmsgupd.Text = "You have not authorised to Update Vendor";
    //        lblmsgview.Text = "You have not authorised to View  Vendor List";
    //    }
    //}

    void GridView()
    {
        try
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlDataAdapter sad = new SqlDataAdapter("SELECT [Prodid],[ProdName],[TechSpeci],[ModelNo],SerialNo,[IsStatus],[CreateBy],[CreateDate],[updateBy],[updateDate]FROM [tblProduct] where isdeleted='0'", con);
            sad.Fill(dt);
            //gv_Prod.EmptyDataText = "Not Records Found";
            //gv_Prod.DataSource = dt;
            //gv_Prod.DataBind();

            con.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        //string createdby = "Admin";
        string createdby = Session["adminname"].ToString();

        try
        {
            if (btnSubmit.Text == "Submit")
            {
                con.Open();
                SqlCommand cmd1 = new SqlCommand("SELECT [Prodid],[ProdName],[TechSpeci],[ModelNo],SerialNo,[IsStatus],[CreateBy],[CreateDate],[updateBy],[updateDate] FROM [tblProduct] where Fullproductname='" + txtfullproductname.Text + "' AND ModelNo='" + txtmodel.Text + "' AND isdeleted='0'", con);
                SqlDataReader reader = cmd1.ExecuteReader();

                if (reader.Read())
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Data Already Available.');window.location ='Product.aspx';", true);
                }
                else
                {
                    con.Close();
                    DateTime Date = DateTime.Now;
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SP_Product", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProdName", txtfullproductname.Text);
                    cmd.Parameters.AddWithValue("@Category", txtcategory.Text);
                    cmd.Parameters.AddWithValue("@Rating", txtrating.Text);
                    cmd.Parameters.AddWithValue("@Fullproductname", txtfullproductname.Text);
                    cmd.Parameters.AddWithValue("@TechSpeci", txtTechSpeci.Text);
                    cmd.Parameters.AddWithValue("@ModelNo", txtmodel.Text);
                    cmd.Parameters.AddWithValue("@SerialNo", txtserialno.Text);
                    cmd.Parameters.AddWithValue("@CreateBy", createdby);
                    cmd.Parameters.AddWithValue("@CreateDate", Date);
                    cmd.Parameters.AddWithValue("@updateBy", null);
                    cmd.Parameters.AddWithValue("@updateDate", Date);
                    cmd.Parameters.AddWithValue("@isdeleted", '0');
                    bool isactive = true;
                    if (DropDownListisActive.Text == "Yes")
                    {
                        isactive = true;
                    }
                    else
                    {
                        isactive = false;
                    }

                    cmd.Parameters.AddWithValue("@IsStatus", isactive);
                    cmd.Parameters.AddWithValue("@Action", "Insert");
                    cmd.ExecuteNonQuery();
                    con.Close();
                    if (Request.QueryString["Name"] != null)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Saved Successfully','0');", true);
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Saved Successfully','1');", true);
                    }
                }
            }
            else if (btnSubmit.Text == "Update")

            {
                DateTime Date = DateTime.Now;
                con.Open();
                SqlCommand cmd = new SqlCommand("SP_Product", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProdName", txtfullproductname.Text);
                cmd.Parameters.AddWithValue("@Category", txtcategory.Text);
                cmd.Parameters.AddWithValue("@Rating", txtrating.Text);
                cmd.Parameters.AddWithValue("@Fullproductname", txtfullproductname.Text);
                cmd.Parameters.AddWithValue("@TechSpeci", txtTechSpeci.Text);
                cmd.Parameters.AddWithValue("@ModelNo", txtmodel.Text);
                cmd.Parameters.AddWithValue("@SerialNo", txtserialno.Text);
                cmd.Parameters.AddWithValue("@CreateBy", createdby);
                cmd.Parameters.AddWithValue("@CreateDate", Date);
                cmd.Parameters.AddWithValue("@updateBy", null);
                cmd.Parameters.AddWithValue("@updateDate", Date);
                cmd.Parameters.AddWithValue("@isdeleted", '0');
                cmd.Parameters.AddWithValue("@Prodid", Convert.ToInt32(hidden.Value));

                bool isactive = true;
                if (DropDownListisActive.Text == "Yes")
                {
                    isactive = true;
                }
                else
                {
                    isactive = false;
                }

                cmd.Parameters.AddWithValue("@IsStatus", isactive);
                cmd.Parameters.AddWithValue("@Action", "Update");
                cmd.ExecuteNonQuery();
                con.Close();
                if (Request.QueryString["Name"] != null)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Updated Successfully','0');", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Updated Successfully','1');", true);
                }
            }
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

    protected void loadData(string id)
    {
        try
        {
            SqlDataAdapter sad = new SqlDataAdapter("SELECT [Prodid],[ProdName],Category,Rating,Fullproductname,[TechSpeci],[ModelNo],SerialNo,[IsStatus],[CreateBy],[CreateDate],[updateBy],[updateDate]FROM [tblProduct] where Prodid='" + id + "' ", con);

            DataTable dt = new DataTable();
            sad.Fill(dt);

            if (dt.Rows.Count > 0)
            {

                txtProdName.Text = dt.Rows[0]["ProdName"].ToString();
                txtcategory.Text = dt.Rows[0]["Category"].ToString();
                txtrating.Text = dt.Rows[0]["Rating"].ToString();
                txtfullproductname.Text = dt.Rows[0]["Fullproductname"].ToString();
                txtTechSpeci.Text = dt.Rows[0]["TechSpeci"].ToString();
                txtmodel.Text = dt.Rows[0]["ModelNo"].ToString();
                txtserialno.Text = dt.Rows[0]["SerialNo"].ToString();
                string flgStatus = "";
                if (dt.Rows[0]["IsStatus"].ToString() == "False")
                {
                    flgStatus = "No";
                }
                else
                {
                    flgStatus = "Yes";
                }
                DropDownListisActive.Text = flgStatus;
            }
            else
            {
               
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void gv_Prod_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "RowEdit")
        {
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
    //protected void lnkBtnsearch_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (string.IsNullOrEmpty(txtSearch.Text))
    //        {
    //            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Search Product Name');", true);

    //        }
    //        if (string.IsNullOrEmpty(txtSearch.Text))
    //        {

    //            GridView();
    //            // UserAuthorization();
    //        }
    //        else
    //        {

    //            DataTable dt = new DataTable();

    //            SqlDataAdapter sad = new SqlDataAdapter("SELECT [Prodid],[ProdName],[TechSpeci],[ModelNo],SerialNo,[IsStatus],[CreateBy],[CreateDate],[updateBy],[updateDate]FROM [tblProduct] where ProdName='" + txtSearch.Text + "' AND isdeleted='0'", con);
    //            sad.Fill(dt);
    //            gv_Prod.EmptyDataText = "Not Records Found";
    //            gv_Prod.DataSource = dt;
    //            gv_Prod.DataBind();
    //            //UserAuthorization();
    //        }

    //        //GridView();
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

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

    //SqlDataAdapter sad;
    //protected void ListStatus()
    //{
    //    try
    //    {
    //        DataTable dt = new DataTable();

    //        if (ddlStatus.Text == "All")
    //        {
    //            sad = new SqlDataAdapter("SELECT [Prodid],[ProdName],[TechSpeci],[ModelNo],SerialNo,[IsStatus],[CreateBy],[CreateDate],[updateBy],[updateDate]FROM [tblProduct] where isdeleted='0'", con);
    //        }
    //        else
    //        {
    //            sad = new SqlDataAdapter("SELECT [Prodid],[ProdName],[TechSpeci],[ModelNo],SerialNo,[IsStatus],[CreateBy],[CreateDate],[updateBy],[updateDate]FROM [tblProduct] where IsStatus='" + ddlStatus.Text + "' AND isdeleted='0'", con);
    //        }
    //        sad.Fill(dt);
    //        gv_Prod.EmptyDataText = "Not Records Found";
    //        gv_Prod.DataSource = dt;
    //        gv_Prod.DataBind();
    //        // UserAuthorization();
    //    }
    //    catch (Exception)
    //    {

    //        throw;
    //    }
    //}
    //protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    ListStatus();
    //}

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ProductList.aspx");
    }

    protected void lnkrefresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("Product.aspx");
    }

    //protected void gv_Prod_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    gv_Prod.PageIndex = e.NewPageIndex;
    //    GridView();
    //}

 
    protected void txtProdName_TextChanged(object sender, EventArgs e)
    {
        string productName = txtProdName.Text;
        string category = txtcategory.Text;
        string rating = txtrating.Text;

        // Concatenate with desired format
        txtfullproductname.Text = $"{productName}  {category} {rating}";
    }

    protected void txtcategory_TextChanged(object sender, EventArgs e)
    {
        string productName = txtProdName.Text;
        string category = txtcategory.Text;
        string rating = txtrating.Text;

        // Concatenate with desired format
        txtfullproductname.Text = $"{productName}  {category}  {rating}";
    }

    protected void txtrating_TextChanged(object sender, EventArgs e)
    {
        string productName = txtProdName.Text;
        string category = txtcategory.Text;
        string rating = txtrating.Text;

        // Concatenate with desired format
        txtfullproductname.Text = $"{productName} {category} {rating}";
    }
}