﻿using System;
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

public partial class Reception_InwardEntry : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
    string id = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["adminname"] == null)
        {
            Response.Redirect("../Login.aspx");
        }
        else
        {
            if (!IsPostBack)
            {
                this.txtDateIn.Text = DateTime.Now.ToString("yyyy-MM-dd");
                this.txtDateIn.TextMode = TextBoxMode.Date;
               // this.txtrepeateddate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                //this.txtrepeateddate.TextMode = TextBoxMode.Date;
                //ddlCustomer();
               // Product();

                if (Request.QueryString["Id"] != null)
                {
                    id = Decrypt(Request.QueryString["Id"].ToString());
                    loadData(id);
                    btnSubmit.Text = "Update";
                    hidden.Value = id;
                }
                else
                {
                    GenerateCode();
                    //int jobid = AutoGen();
                    //string jobcode = String.Concat("JOB", jobid);
                    //txtJobNo.Text = jobcode;
                }
            }
        }

        //if (!IsPostBack)
        //{
        //    this.txtDateIn.Text = DateTime.Now.ToString("yyyy-MM-dd");
        //    this.txtDateIn.TextMode = TextBoxMode.Date;
        //    this.txtrepeateddate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        //    this.txtrepeateddate.TextMode = TextBoxMode.Date;
        //    ddlCustomer();
        //    Product();

        //    if (Request.QueryString["Id"] != null)
        //    {
        //        id = Decrypt(Request.QueryString["Id"].ToString());
        //        loadData(id);
        //        btnSubmit.Text = "Update";
        //        hidden.Value = id;

        //    }
        //    else
        //    {
        //        GenerateCode();
        //        //int jobid = AutoGen();
        //        //string jobcode = String.Concat("JOB", jobid);
        //        //txtJobNo.Text = jobcode;
        //    }
        //}
    }
    protected void lnkBtmNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("Customer.aspx?Name=InwardEntry");
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("InwardEntryList.aspx");
    }


    //customer Autocomplate 
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
                com.CommandText = "  select DISTINCT CustomerName from [tblCustomer] where " + "CustomerName like @Search + '%' AND isdeleted='0'";

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

    //Product Autocomplate 
    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetProductList(string prefixText, int count)
    {
        return AutoFillGetProductList(prefixText);
    }
    public static List<string> AutoFillGetProductList(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT ProdName from [tblProduct] where " + "ProdName like @Search + '%' AND isdeleted='0'";

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

    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        // string createdby = "Admin";
        // CreatedBy = Session["adminname"].ToString();
        string Path = null;
        if (btnSubmit.Text == "Submit")
        {

            if (txtcustomername.Text == "Select Customer")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Customer Name');", true);
            }
            else if (txtproductname.Text == "Select Product")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Product Name');", true);
            }
            else
            {
                string QuatationNo;
                SqlDataAdapter ad = new SqlDataAdapter("SELECT max([id]) as maxid FROM [tblInwardEntry]", con);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    int maxid = dt.Rows[0]["maxid"].ToString() == "" ? 0 : Convert.ToInt32(dt.Rows[0]["maxid"].ToString());
                    QuatationNo = "QN" + (maxid + 1).ToString();

                    DateTime Date = DateTime.Now;

                    con.Open();
                    SqlCommand cmd = new SqlCommand("SP_InwardEntry", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@JobNo", txtJobNo.Text);
                    cmd.Parameters.AddWithValue("@DateIn", txtDateIn.Text);
                    cmd.Parameters.AddWithValue("@CustName", txtcustomername.Text);
                    cmd.Parameters.AddWithValue("@MateName", txtproductname.Text);
                    cmd.Parameters.AddWithValue("@SrNo", txtSrNo.Text);
                    cmd.Parameters.AddWithValue("@Subcustomer", txtsubcust.Text);
                    cmd.Parameters.AddWithValue("@MateStatus", txtMateriStatus.Text);
                    cmd.Parameters.AddWithValue("@FinalStatus", txtfinalstatus.Text);
                    //cmd.Parameters.AddWithValue("@TestBy", DropDownListtest.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@ModelNo", txtModelNo.Text);
                    cmd.Parameters.AddWithValue("@CreatedBy", Session["adminname"].ToString());
                    cmd.Parameters.AddWithValue("@CreatedDate", Date);
                    cmd.Parameters.AddWithValue("@UpdateBy", Session["adminname"].ToString());
                    cmd.Parameters.AddWithValue("@UpdateDate", Date);
                    cmd.Parameters.AddWithValue("@Quotation_no", QuatationNo);
                    cmd.Parameters.AddWithValue("@isdeleted", '0');
                    cmd.Parameters.AddWithValue("@ProductFault", txtproductfaulty.Text);
                    cmd.Parameters.AddWithValue("@RepeatedNo", txtrepeatedno.Text);
                    cmd.Parameters.AddWithValue("@RepeatedDate", txtrepeateddate.Text);
                    cmd.Parameters.AddWithValue("@RepeatedJobNo", txtrepetedjob.Text);
                    cmd.Parameters.AddWithValue("@Date", txtdate.Text);
                    cmd.Parameters.AddWithValue("@Branch", txtbranch.Text);
                    cmd.Parameters.AddWithValue("@otherinfo", txtotherinfo.Text);
                    cmd.Parameters.AddWithValue("@ServiceType", ddlservicetype.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@Services", txtservices.Text);
                    cmd.Parameters.AddWithValue("@CustChallnno", txtcustomerno.Text);
                    if (FileUpload.HasFile)
                    {
                        var Filenamenew = FileUpload.FileName;
                        string codenew = Guid.NewGuid().ToString();
                        Path = Server.MapPath("~/ProductImg/") + codenew + "_" + Filenamenew;
                        FileUpload.SaveAs(Path);
                        cmd.Parameters.AddWithValue("@Imagepath", "~/ProductImg/" + codenew + "_" + Filenamenew);
                    }
                   // cmd.Parameters.AddWithValue("@Imagepath", "~/ProductImg/" + FileUpload.FileName);
                    cmd.Parameters.AddWithValue("@Action", "Insert");
                    cmd.ExecuteNonQuery();
                    con.Close();
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Saved Successfully');", true);
                }
                else
                {
                    QuatationNo = string.Empty;
                }
            }
        }
        else if (btnSubmit.Text == "Update")
        {
            DateTime Date = DateTime.Now;
            DateTime dat = Convert.ToDateTime(txtDateIn.Text);
            con.Open();
            SqlCommand cmd1 = new SqlCommand("SP_InwardEntry", con);
            cmd1.CommandType = CommandType.StoredProcedure;

            cmd1.Parameters.AddWithValue("@JobNo", txtJobNo.Text);
            cmd1.Parameters.AddWithValue("@DateIn", dat);
            cmd1.Parameters.AddWithValue("@CustName", txtcustomername.Text);
            cmd1.Parameters.AddWithValue("@MateName", txtproductname.Text);
            cmd1.Parameters.AddWithValue("@SrNo", txtSrNo.Text);
            cmd1.Parameters.AddWithValue("@Subcustomer", txtsubcust.Text);
            cmd1.Parameters.AddWithValue("@MateStatus", txtMateriStatus.Text);
            cmd1.Parameters.AddWithValue("@FinalStatus", txtfinalstatus.Text);
            //cmd1.Parameters.AddWithValue("@TestBy", DropDownListtest.SelectedItem.Text);
            cmd1.Parameters.AddWithValue("@ModelNo", txtModelNo.Text);

            cmd1.Parameters.AddWithValue("@CreatedBy", Session["adminname"].ToString());
            cmd1.Parameters.AddWithValue("@CreatedDate", Date);
            cmd1.Parameters.AddWithValue("@UpdateBy", Session["adminname"].ToString());
            cmd1.Parameters.AddWithValue("@UpdateDate", Date);
            cmd1.Parameters.AddWithValue("@isdeleted", '0');
            cmd1.Parameters.AddWithValue("@ProductFault", txtproductfaulty.Text);
            cmd1.Parameters.AddWithValue("@RepeatedNo", txtrepeatedno.Text);
            cmd1.Parameters.AddWithValue("@RepeatedDate", txtrepeateddate.Text);
            cmd1.Parameters.AddWithValue("@RepeatedJobNo", txtrepetedjob.Text);
            cmd1.Parameters.AddWithValue("@Date", txtdate.Text);
            cmd1.Parameters.AddWithValue("@Branch", txtbranch.Text);
            cmd1.Parameters.AddWithValue("@otherinfo", txtotherinfo.Text);
            cmd1.Parameters.AddWithValue("@ServiceType", ddlservicetype.SelectedItem.Text);
            cmd1.Parameters.AddWithValue("@Services", txtservices.Text);
            cmd1.Parameters.AddWithValue("@CustChallnno", txtcustomerno.Text);
            if (FileUpload.HasFile)
            {
                var Filenamenew = FileUpload.FileName;
                string codenew = Guid.NewGuid().ToString();
                Path = Server.MapPath("~/ProductImg/") + codenew + "_" + Filenamenew;
                FileUpload.SaveAs(Path);
                cmd1.Parameters.AddWithValue("@Imagepath", "~/ProductImg/" + codenew + "_" + Filenamenew);
            }
            else
            {
                cmd1.Parameters.AddWithValue("@Imagepath", lblPath.Text);
            }
            cmd1.Parameters.AddWithValue("@Action", "Update");
            cmd1.ExecuteNonQuery();
            con.Close();
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Updated Successfully');", true);
        }
    }
    //protected void ddlCustomer()
    //{
    //    try
    //    {
    //        DataTable dt = new DataTable();

    //        con.Open();
    //        SqlDataAdapter sad = new SqlDataAdapter("select [Custid],[CustomerName],[GSTNo],[StateCode],[PanNo],[AddresLine1],[AddresLine2],[AddresLine3],[Area],[Email],[City],[Country],[MobNo],[PostalCode],[ContactPerName1],[ContactPerNo1],[ContactPerName2],[ContactPerNo2],[IsStatus],[CreatedBy],[Createddate],[UpdatedBy],[UpdatedDate] FROM [tblCustomer] where isdeleted='0' AND IsStatus='1'", con);
    //        sad.Fill(dt);
    //        //  DropDownCust.DataValueField = "Custid";
    //        DropDownCust.DataTextField = "CustomerName";

    //        DropDownCust.DataSource = dt;
    //        DropDownCust.DataBind();

    //        con.Close();
    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }
    //}
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
            SqlDataAdapter sad = new SqlDataAdapter("select * FROM tblInwardEntry where JobNo='" + id + "' ", con);

            DataTable dt = new DataTable();
            sad.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                txtJobNo.Text = dt.Rows[0]["JobNo"].ToString();
                DateTime ffff1 = Convert.ToDateTime(dt.Rows[0]["DateIn"].ToString());
                txtDateIn.Text = ffff1.ToString("yyyy-MM-dd");
                txtcustomername.Text = dt.Rows[0]["CustName"].ToString();
                txtproductname.Text = dt.Rows[0]["MateName"].ToString();
                txtSrNo.Text = dt.Rows[0]["SrNo"].ToString();
                txtsubcust.Text = dt.Rows[0]["Subcustomer"].ToString();
                txtMateriStatus.Text = dt.Rows[0]["MateStatus"].ToString();
                txtfinalstatus.Text = dt.Rows[0]["FinalStatus"].ToString();
                // DropDownListtest.SelectedItem.Text = dt.Rows[0]["TestBy"].ToString();
                txtModelNo.Text = dt.Rows[0]["ModelNo"].ToString();
                txtproductfaulty.Text = dt.Rows[0]["ProductFault"].ToString();
                txtrepeatedno.Text = dt.Rows[0]["RepeatedNo"].ToString();
                //DateTime ffff2 = Convert.ToDateTime(dt.Rows[0]["RepeatedDate"].ToString());

                // DateTime ffff3 = Convert.ToDateTime(dt.Rows[0]["Date"].ToString());
                //txtdate.Text = ffff3.ToString("yyyy-MM-dd");

                DateTime ffff4 = Convert.ToDateTime(dt.Rows[0]["RepeatedDate"].ToString());
                txtrepeateddate.Text = ffff4.ToString("yyyy-MM-dd");
                txtbranch.Text = dt.Rows[0]["Branch"].ToString();
                txtotherinfo.Text = dt.Rows[0]["otherinfo"].ToString();
                ddlservicetype.Text = dt.Rows[0]["ServiceType"].ToString();
                txtservices.Text = dt.Rows[0]["Services"].ToString();
                txtcustomerno.Text = dt.Rows[0]["CustChallnno"].ToString();
                lblPath.Text = dt.Rows[0]["Imagepath"].ToString();
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
    protected void txtDateIn_TextChanged(object sender, EventArgs e)
    {

    }
    protected void GenerateCode()
    {
        SqlDataAdapter ad = new SqlDataAdapter("SELECT max([id]) as maxid FROM [tblInwardEntry] WHERE isdeleted ='0'", con);
        DataTable dt = new DataTable();
        ad.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            int maxid = dt.Rows[0]["maxid"].ToString() == "" ? 6211 : Convert.ToInt32(dt.Rows[0]["maxid"].ToString());
            if (maxid == 6211) //new change 
            {
                txtJobNo.Text = (maxid).ToString();
            }
            else
            {
                txtJobNo.Text = (maxid + 6211).ToString();
            }

            //int maxid = dt.Rows[0]["maxid"].ToString() == "" ? 5014 : Convert.ToInt32(dt.Rows[0]["maxid"].ToString());
            //if (maxid == 5014)
            //{
            //    txtJobNo.Text = "JOBNO." + (maxid).ToString();
            //}
            //else
            //{
            //    txtJobNo.Text = "JOBNO." + (maxid + 5014).ToString();
            //}
            //          
        }
        else
        {
            txtJobNo.Text = string.Empty;
        }
    }

    //protected void Product()
    //{
    //    DataTable dt = new DataTable();
    //    con.Open();
    //    SqlDataAdapter sad = new SqlDataAdapter("select * from tblProduct where isdeleted='0' AND IsStatus='1'", con);
    //    sad.Fill(dt);
    //    ddlproduct.DataTextField = "ProdName";
    //    ddlproduct.DataValueField = "Prodid";
    //    ddlproduct.DataSource = dt;
    //    ddlproduct.DataBind();
    //    con.Close();
    //}

    protected void lnkproduct_Click(object sender, EventArgs e)
    {
        Response.Redirect("Product.aspx?Name=InwardEntry");
    }

    //protected void ddlproduct_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    DataTable dt = new DataTable();
    //    con.Open();
    //    SqlDataAdapter sad = new SqlDataAdapter("select * from tblProduct where isdeleted='0' AND IsStatus='1' AND Prodid='" + txtproductname.Text + "'", con);
    //    sad.Fill(dt);
    //    if (dt.Rows.Count > 0)
    //    {
    //        txtModelNo.Text = dt.Rows[0]["ModelNo"].ToString();
    //        txtSrNo.Text = dt.Rows[0]["SerialNo"].ToString();
    //    }
    //}

    protected void txtproductname_TextChanged(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        con.Open();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblProduct where isdeleted='0' AND IsStatus='1' AND ProdName='" + txtproductname.Text + "'", con);
        sad.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            txtModelNo.Text = dt.Rows[0]["ModelNo"].ToString();
            txtSrNo.Text = dt.Rows[0]["SerialNo"].ToString();
        }
    }
}
