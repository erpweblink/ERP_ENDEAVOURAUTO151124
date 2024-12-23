using Org.BouncyCastle.Asn1.X9;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Enquiry : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
    DataTable dtpers = new DataTable();
    string Customername;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["adminname"] == null)
        {
            Response.Redirect("../LoginPage.aspx");
        }
        else
        {
            lblProduct1.Visible = false;
            if (!IsPostBack)
            {
                Session["OneTimeFlag"] = "";
                ViewState["RowNo"] = 0;
                dtpers.Columns.AddRange(new DataColumn[5] { new DataColumn("colid"), new DataColumn("colFirst"), new DataColumn("colsecond"), new DataColumn("colemail"), new DataColumn("coldesignation") });

                ViewState["TableContact"] = dtpers;

                if (Request.QueryString["EnquiryId"] != null)
                {
                    string id = Decrypt(Request.QueryString["EnquiryId"].ToString());
                    loadData(id);

                    btnSubmit.Text = "Update";
                    imgProduct.Visible = true;
                    hidden.Value = id;
                }
                if (Request.QueryString["CUID"] != null)
                {
                    string id = Decrypt(Request.QueryString["CUID"].ToString());
                    loadCustomer(id);

                    btnSubmit.Text = "Save";
                }

            }
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (Session["OneTimeFlag"] == null || Session["OneTimeFlag"].ToString() == "")
        {
            Session["OneTimeFlag"] = "Inserted";
            //string createdby = "Admin";
            string createdby = Session["adminname"].ToString();
            int id;
            string oldName = "", oldMobNo = "", oldEmail = "", oldProductImage = "", oldProductName = "";
            try
            {
                string Path = null;
                if (btnSubmit.Text == "Save")
                {
                    DateTime Date = DateTime.Now;
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SP_Enquiry", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CustomerName", txtCustName.Text);

                    cmd.Parameters.AddWithValue("@StateCode", DropDownListcustomer.Text);

                    cmd.Parameters.AddWithValue("@AddresLine1", txtAddresline1.Text);

                    cmd.Parameters.AddWithValue("@Area", txtarea.Text);
                    cmd.Parameters.AddWithValue("@City", txtcity.Text);
                    cmd.Parameters.AddWithValue("@Country", txtcountry.Text);
                    cmd.Parameters.AddWithValue("@Email", txttemail.Text);
                    cmd.Parameters.AddWithValue("@MobNo", txtMobileNo.Text);
                    cmd.Parameters.AddWithValue("@PostalCode", txtPostalCode.Text);
                    cmd.Parameters.AddWithValue("@createdBy", createdby);
                    cmd.Parameters.AddWithValue("@createddate", Date);
                    cmd.Parameters.AddWithValue("@isdeleted", '0');
                    // Product Information
                    cmd.Parameters.AddWithValue("@ProductName", txtproductname.Text);
                    cmd.Parameters.AddWithValue("@ServiceType", ddlservicetype.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@otherinfo", txtotherinfo.Text);

                    if (FileUpload.HasFile)
                    {
                        var Filenamenew = FileUpload.FileName;
                        string codenew = Guid.NewGuid().ToString();
                        Path = Server.MapPath("~/ProductImg/") + codenew + "_" + Filenamenew;
                        FileUpload.SaveAs(Path);
                        cmd.Parameters.AddWithValue("@Imagepath", "~/ProductImg/" + codenew + "_" + Filenamenew);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Imagepath", lblPath.Text);
                    }

                    // Customer table search values
                    cmd.Parameters.AddWithValue("@OLDCustomerName", oldName);
                    cmd.Parameters.AddWithValue("@OLDCustomerMobile", oldMobNo);
                    cmd.Parameters.AddWithValue("@OLdCustomerMail", oldEmail);
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
                    cmd.Parameters.Add("@enquiry_id", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@Action", "Insert");
                    if (txttemail.Text != "" && txtMobileNo.Text != "")
                    {
                        cmd.Parameters.AddWithValue("@Code", "NewCustomer");
                    }

                    cmd.ExecuteNonQuery();
                    con.Close();
                    id = Convert.ToInt32(cmd.Parameters["@enquiry_id"].Value);

                    if (Request.QueryString["Name"] != null)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Saved Successfully','0');", true);
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Saved Successfully','1');", true);
                    }
                }
                else if (btnSubmit.Text == "Update")
                {
                    int Eqid = Convert.ToInt32(hidden.Value);
                    con.Open();
                    SqlCommand cmd2 = new SqlCommand(
                        "SELECT [CustomerName], [MobNo], [Email], [ProductImage], [ProdName]" +
                        "FROM [tbl_EnquiryMaster] " +
                        "WHERE [EnquiryId] ='" + Eqid + "'",
                        con);

                    SqlDataReader reader = cmd2.ExecuteReader();
                    if (reader.Read())
                    {
                        oldName = reader["CustomerName"].ToString();
                        oldMobNo = reader["MobNo"].ToString();
                        oldEmail = reader["Email"].ToString();
                        oldProductImage = reader["ProductImage"].ToString();
                        oldProductName = reader["ProdName"].ToString();
                    }
                    reader.Close();
                    con.Close();

                    DateTime Date = DateTime.Now;
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SP_Enquiry", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CustomerName", txtCustName.Text);
                    cmd.Parameters.AddWithValue("@StateCode", DropDownListcustomer.Text);
                    cmd.Parameters.AddWithValue("@AddresLine1", txtAddresline1.Text);
                    cmd.Parameters.AddWithValue("@Area", txtarea.Text);
                    cmd.Parameters.AddWithValue("@City", txtcity.Text);
                    cmd.Parameters.AddWithValue("@Country", txtcountry.Text);
                    cmd.Parameters.AddWithValue("@Email", txttemail.Text);
                    cmd.Parameters.AddWithValue("@MobNo", txtMobileNo.Text);
                    cmd.Parameters.AddWithValue("@PostalCode", txtPostalCode.Text);
                    cmd.Parameters.AddWithValue("@createdBy", createdby);
                    cmd.Parameters.AddWithValue("@createddate", Date);
                    cmd.Parameters.AddWithValue("@UpdatedBy", createdby);
                    cmd.Parameters.AddWithValue("@UpdatedDate", Date);
                    cmd.Parameters.AddWithValue("@isdeleted", '0');
                    // Product Information
                    cmd.Parameters.AddWithValue("@ProductName", txtproductname.Text);
                    cmd.Parameters.AddWithValue("@ServiceType", ddlservicetype.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@otherinfo", txtotherinfo.Text);
                    if (FileUpload.HasFile)
                    {
                        var Filenamenew = FileUpload.FileName;
                        string codenew = Guid.NewGuid().ToString();
                        Path = Server.MapPath("~/ProductImg/") + codenew + "_" + Filenamenew;
                        FileUpload.SaveAs(Path);
                        cmd.Parameters.AddWithValue("@Imagepath", "~/ProductImg/" + codenew + "_" + Filenamenew);
                    }
                    else
                    {
                        if (oldProductName == txtproductname.Text)
                        {
                            cmd.Parameters.AddWithValue("@Imagepath", oldProductImage);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Imagepath", lblPath.Text);
                        }
                    }
                    // Customer table search values
                    cmd.Parameters.AddWithValue("@OLDCustomerName", oldName);
                    cmd.Parameters.AddWithValue("@OLDCustomerMobile", oldMobNo);
                    cmd.Parameters.AddWithValue("@OLdCustomerMail", oldEmail);

                    cmd.Parameters.AddWithValue("@EnquiryId", Convert.ToInt32(hidden.Value));
                    cmd.Parameters.Add("@enquiry_id", SqlDbType.Int).Direction = ParameterDirection.Output;
                    id = Convert.ToInt32(cmd.Parameters["@enquiry_id"].Value);
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
                    cmd.Parameters.AddWithValue("@Action", "update");
                    cmd.Parameters.AddWithValue("@Code", "NewCustomer");
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
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Please wait... Thanks','1');", true);
        }
    }


    protected void LoadDataCustomer(string Customername)
    {
        try
        {
            SqlDataAdapter sad = new SqlDataAdapter("SELECT [Custid],[CustomerName],[GSTNo],[StateCode],[PanNo],[AddresLine1],[AddresLine2],[AddresLine3],[Area],[Email],[City],[Country],[MobNo],[PostalCode],[ContactPerName1],[ContactPerNo1],[ContactPerName2],[ContactPerNo2],[IsStatus],[CreatedBy],[Createddate],[UpdatedBy],[UpdatedDate] FROM [tblCustomer] where CustomerName='" + Customername + "'", con);

            DataTable dt = new DataTable();
            sad.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                txtCustName.Text = dt.Rows[0]["CustomerName"].ToString();

                DropDownListcustomer.Text = dt.Rows[0]["StateCode"].ToString();

                txtAddresline1.Text = dt.Rows[0]["AddresLine1"].ToString();

                txtarea.Text = dt.Rows[0]["Area"].ToString();
                txtcity.Text = dt.Rows[0]["City"].ToString();
                txtcountry.Text = dt.Rows[0]["Country"].ToString();
                txtPostalCode.Text = dt.Rows[0]["PostalCode"].ToString();
                txtMobileNo.Text = dt.Rows[0]["MobNo"].ToString();
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
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("EnquiryList.aspx");
    }

    // Below two methods are created by Nikhil 
    protected void loadData(string id)
    {
        try
        {
            int Eid = Convert.ToInt32(id);
            SqlDataAdapter sad = new SqlDataAdapter(
    "SELECT [EnquiryId], [CustomerName], [StateCode], [AddresLine1], [Area], [City], [Country], [PostalCode], [MobNo], [IsStatus], " +
    "[Email], [CreatedBy], [Createddate], [UpdatedBy], [UpdatedDate], [isdeleted]," +
    "[ProdName], [ServiceType], [ProductImage], [OtherInformation]" +
    "FROM [tbl_EnquiryMaster] " +
    "WHERE [EnquiryId] ='" + Eid + "'",
    con);
            DataTable dt = new DataTable();
            sad.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                txtCustName.Text = dt.Rows[0]["CustomerName"].ToString();

                DropDownListcustomer.Text = dt.Rows[0]["StateCode"].ToString();

                txtAddresline1.Text = dt.Rows[0]["AddresLine1"].ToString();

                txtarea.Text = dt.Rows[0]["Area"].ToString();
                txtcity.Text = dt.Rows[0]["City"].ToString();
                txtcountry.Text = dt.Rows[0]["Country"].ToString();
                txtPostalCode.Text = dt.Rows[0]["PostalCode"].ToString();
                txtMobileNo.Text = dt.Rows[0]["MobNo"].ToString();
                txttemail.Text = dt.Rows[0]["Email"].ToString();
                txtproductname.Text = dt.Rows[0]["ProdName"].ToString();
                ddlservicetype.Text = dt.Rows[0]["ServiceType"].ToString();
                txtotherinfo.Text = dt.Rows[0]["OtherInformation"].ToString();
                //lblPath.Text = dt.Rows[0]["ProductImage"].ToString();                   

                string productImage = dt.Rows[0]["ProductImage"].ToString();
                imgProduct.ImageUrl = ResolveUrl(productImage);

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
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }
    protected void loadCustomer(string id)
    {
        try
        {
            SqlDataAdapter sad = new SqlDataAdapter(
            "SELECT [CustomerName], [StateCode], [AddresLine1], [Area], [City], [Country], [PostalCode], [MobNo]," +
            "[Email] FROM [tblCustomer] " +
            "WHERE [Custid] ='" + id + "'", con);
            DataTable dt = new DataTable();
            sad.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                string email = dt.Rows[0]["Email"].ToString();
                string StateCod = dt.Rows[0]["StateCode"].ToString();
                string MobileNO = dt.Rows[0]["MobNo"].ToString();
                if (email != "")
                {
                    txttemail.Text = dt.Rows[0]["Email"].ToString();
                    txttemail.ReadOnly = true;
                }
                else
                {
                    txttemail.ReadOnly = true;
                    RequiredFieldValidator5.Enabled = false;
                }

                DropDownListcustomer.Text = dt.Rows[0]["StateCode"].ToString();

                if (MobileNO != "")
                {
                    txtMobileNo.Text = dt.Rows[0]["MobNo"].ToString();
                    txtMobileNo.ReadOnly = true;
                }
                else
                {
                    txtMobileNo.ReadOnly = true;
                    RequiredFieldValidator1.Enabled = false;
                }
                txtCustName.Text = dt.Rows[0]["CustomerName"].ToString();
                txtAddresline1.Text = dt.Rows[0]["AddresLine1"].ToString();
                txtarea.Text = dt.Rows[0]["Area"].ToString();
                txtcity.Text = dt.Rows[0]["City"].ToString();
                txtcountry.Text = dt.Rows[0]["Country"].ToString();
                txtPostalCode.Text = dt.Rows[0]["PostalCode"].ToString();
            }
        }
        catch (Exception ex)
        {

            throw ex;
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

    //Product Autocomplate  Below all the function are made by Nikhil 
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

    protected void txtCustName_TextChanged(object sender, EventArgs e)
    {
        try
        {
            SqlDataAdapter Da = new SqlDataAdapter("select * from tblCustomer  WHERE CustomerName='" + txtCustName.Text + "'", con);
            DataTable dt = new DataTable();
            Da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                string email = dt.Rows[0]["Email"].ToString();
                string StateCod = dt.Rows[0]["StateCode"].ToString();
                string MobileNO = dt.Rows[0]["MobNo"].ToString();
                if (email != "")
                {
                    txttemail.Text = dt.Rows[0]["Email"].ToString();
                    txttemail.ReadOnly = true;
                }
                else
                {
                    txttemail.ReadOnly = true;
                    RequiredFieldValidator5.Enabled = false;
                }

                DropDownListcustomer.Text = dt.Rows[0]["StateCode"].ToString();

                if (MobileNO != "")
                {
                    txtMobileNo.Text = dt.Rows[0]["MobNo"].ToString();
                    txtMobileNo.ReadOnly = true;
                }
                else
                {
                    txtMobileNo.ReadOnly = true;
                    RequiredFieldValidator1.Enabled = false;
                }
                hidden.Value = dt.Rows[0]["Custid"].ToString();
                txtAddresline1.Text = dt.Rows[0]["AddresLine1"].ToString();
                txtarea.Text = dt.Rows[0]["Area"].ToString();
                txtcity.Text = dt.Rows[0]["City"].ToString();
                txtcountry.Text = dt.Rows[0]["Country"].ToString();
                txtPostalCode.Text = dt.Rows[0]["PostalCode"].ToString();
                lnkBtmUpdate.Visible = true;
            }
            else
            {
                txttemail.Text = "";
                DropDownListcustomer.Text = "";
                txtMobileNo.Text = "";
                txtAddresline1.Text = "";
                txtarea.Text = "";
                txtcity.Text = "";
                txtcountry.Text = "";
                txtPostalCode.Text = "";
                lnkBtmUpdate.Visible = false;
                txttemail.ReadOnly = false;
                RequiredFieldValidator5.Enabled = true;
                DropDownListcustomer.Enabled = true;
                txtMobileNo.ReadOnly = false;
                RequiredFieldValidator1.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            string errorMsg = "An error occurred : " + ex.Message;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + errorMsg + "') ", true);
        }
    }


    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetCompanyList(string prefixText, int count)
    {
        return AutoFillCompanyName(prefixText);
    }

    public static List<string> AutoFillCompanyName(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "Select DISTINCT [CustomerName] from [tblCustomer] where " + "CustomerName like @Search + '%' and IsDeleted=0";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> countryNames = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        countryNames.Add(sdr["CustomerName"].ToString());
                    }
                }
                con.Close();
                return countryNames;
            }
        }
    }

    protected void btncreate1_Click(object sender, EventArgs e)
    {
        Response.Redirect("EnquiryList.aspx");
    }

    protected void lnkBtmUpdate_Click(object sender, EventArgs e)
    {
        string id = hidden.Value;
        Response.Redirect("Customer.aspx?EditCust=" + encrypted(id) + "");
    }

    public string encrypted(string encryptString)
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

    protected void txtproductname_TextChanged(object sender, EventArgs e)
    {
        lblproduct.Text = "";
         string productName = txtproductname.Text.Trim();

       
        if (string.IsNullOrEmpty(productName))
        {

            lblProduct1.Text = "Product name cannot be empty!";
            lblProduct1.ForeColor = System.Drawing.Color.Red;
        }
        else
        {
         
            bool productExists = CheckIfProductExists(productName);

            if (!productExists)
            {
                lblProduct1.Visible = true;
                txtproductname.Text = "";
                lblProduct1.Text = "Product Not Present";
                lblProduct1.ForeColor = System.Drawing.Color.Red;
                lblProduct1.Font.Bold = true;
            }            
        }
    }

   
    private bool CheckIfProductExists(string productName)
    {
        using (SqlCommand com = new SqlCommand())
        {            
            com.CommandText = "SELECT COUNT(*) FROM [tblProduct] WHERE ProdName = @Search AND isdeleted = '0'";           
            com.Parameters.AddWithValue("@Search", productName);
            com.Connection = con;

            try
            {
                con.Open();                
                int productCount = (int)com.ExecuteScalar();               
                return productCount > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                con.Close(); 
            }
        }
    }

}