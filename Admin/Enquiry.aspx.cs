using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;

public partial class Admin_Enquiry : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
    DataTable dtpers = new DataTable();
    string Customername;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            ViewState["RowNo"] = 0;
            dtpers.Columns.AddRange(new DataColumn[5] { new DataColumn("colid"), new DataColumn("colFirst"), new DataColumn("colsecond"), new DataColumn("colemail"), new DataColumn("coldesignation") });

            ViewState["TableContact"] = dtpers;

            if (Request.QueryString["EnquiryId"] != null)
            {
                string id = Decrypt(Request.QueryString["EnquiryId"].ToString());               
                loadData(id);

                btnSubmit.Text = "Update";
                hidden.Value = id;
            }
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        //string createdby = "Admin";
        string createdby = Session["adminname"].ToString();
        int id;
        string oldName = "", oldMobNo = "", oldEmail = "";
        try
        {
            if (btnSubmit.Text == "Save")
            {
                con.Open();                
                SqlCommand cmd1 = new SqlCommand(
                    "SELECT [EnquiryId], [CustomerName], [StateCode], [AddresLine1], [Area], [City], [Country], [PostalCode], [MobNo], [Email], [IsStatus] " +
                    "FROM [tbl_EnquiryMaster] " +
                    "WHERE [CustomerName] ='" + txtCustName.Text + "'  AND [MobNo]='" + txtMobileNo.Text + "'",
                    con);

                SqlDataReader reader = cmd1.ExecuteReader();
                if (reader.Read())
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Data Already Exist.');", true);
                }
                else
                {
                    con.Close();
                    DateTime Date = DateTime.Now;
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SP_EnquiryMaster", con);
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
            }
            else if (btnSubmit.Text == "Update")
            {
                int Eqid = Convert.ToInt32(hidden.Value);
                con.Open();
                SqlCommand cmd2 = new SqlCommand(
                    "SELECT [CustomerName], [MobNo], [Email]" +
                    "FROM [tbl_EnquiryMaster] " +
                    "WHERE [EnquiryId] ='" + Eqid + "'",
                    con);

                SqlDataReader reader = cmd2.ExecuteReader();
                if (reader.Read())
                {
                     oldName = reader["CustomerName"].ToString();
                     oldMobNo = reader["MobNo"].ToString();
                     oldEmail = reader["Email"].ToString();                   
                }
                reader.Close();
                con.Close();
                
                DateTime Date = DateTime.Now;
                con.Open();
                SqlCommand cmd = new SqlCommand("SP_EnquiryMaster", con);
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

    protected void loadData(string id)
    {
        try
        {
            int Eid = Convert.ToInt32(id);           
            SqlDataAdapter sad = new SqlDataAdapter(
    "SELECT [EnquiryId], [CustomerName], [StateCode], [AddresLine1], [Area], [City], [Country], [PostalCode], [MobNo], [IsStatus], " +
    "[Email], [CreatedBy], [Createddate], [UpdatedBy], [UpdatedDate], [isdeleted] " +
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
}