using System;
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
using iTextSharp.tool.xml.html.table;
using System.Activities.Expressions;

public partial class Reception_Customer : System.Web.UI.Page
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

            if (Request.QueryString["Custid"] != null)
            {
                string id = Decrypt(Request.QueryString["Custid"].ToString());
                loadData(id);

                btnSubmit.Text = "Update";
                hidden.Value = id;
            }
            // Code by Nikhil to edit Invert Entry page customer
            if (Request.QueryString["Edit"] != null)
            {
                string id = Decrypt(Request.QueryString["Edit"].ToString());
                loadData(id);

                btnSubmit.Text = "Save & Go Back";
                btnCancel.Visible = false;
                hidden.Value = id;
            }
              if (Request.QueryString["EditCust"] != null)
            {
                string id = Decrypt(Request.QueryString["EditCust"].ToString());
                loadData(id);

                btnSubmit.Text = "Edit";
                btnCancel.Visible = false;
                hidden.Value = id;
            }

        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        //string createdby = "Admin";
        string createdby = Session["adminname"].ToString();
        int id;
        try
        {
            if (btnSubmit.Text == "Submit")
            {
                con.Open();
                SqlCommand cmd1 = new SqlCommand("SELECT [Custid],[CustomerName],[GSTNo],[StateCode],[PanNo],[AddresLine1],[AddresLine2],[AddresLine3],[Area],[Email],[City],[Country],[MobNo],[PostalCode],[ContactPerName1],[ContactPerNo1],[ContactPerName2],[ContactPerNo2],[IsStatus],[CreatedBy],[Createddate],[UpdatedBy],[UpdatedDate] FROM [tblCustomer] where [CustomerName]='" + txtCustName.Text + "'  AND [MobNo]='" + txtMobileNo.Text + "' AND isdeleted='0' ", con);
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
                    SqlCommand cmd = new SqlCommand("SP_CustomerMaster", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CustomerName", txtCustName.Text);
                    cmd.Parameters.AddWithValue("@GSTNo", txtgstno.Text);
                    cmd.Parameters.AddWithValue("@StateCode", DropDownListcustomer.Text);
                    cmd.Parameters.AddWithValue("@PanNo", txtpanno.Text);
                    cmd.Parameters.AddWithValue("@AddresLine1", txtAddresline1.Text);
                    cmd.Parameters.AddWithValue("@AddresLine2", txtadreLine2.Text);
                    cmd.Parameters.AddWithValue("@AddresLine3", txtadreLine3.Text);
                    cmd.Parameters.AddWithValue("@Area", txtarea.Text);
                    cmd.Parameters.AddWithValue("@City", txtcity.Text);
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@Country", txtcountry.Text);
                    cmd.Parameters.AddWithValue("@MobNo", txtMobileNo.Text);
                    cmd.Parameters.AddWithValue("@PostalCode", txtPostalCode.Text);
                    cmd.Parameters.AddWithValue("@createdBy", createdby);
                    cmd.Parameters.AddWithValue("@createddate", Date);
                    cmd.Parameters.AddWithValue("@isdeleted", '0');

                    //19 - 11 - 2024 Added by Nikhil to update the Enquiry master table details for inward entry purpose here we are not updating enquiry table 
                    cmd.Parameters.AddWithValue("@OldEnqId", null);

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
                    cmd.Parameters.Add("@cust_id", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@Action", "Insert");

                    cmd.ExecuteNonQuery();
                    con.Close();
                    id = Convert.ToInt32(cmd.Parameters["@cust_id"].Value);
                    foreach (GridViewRow g1 in gv_Customercontact.Rows)
                    {
                        string personname = (g1.FindControl("lblCustomername") as Label).Text;
                        string personno = (g1.FindControl("lblCustomernameno") as Label).Text;
                        string email = (g1.FindControl("lblemailperson") as Label).Text;
                        string designation = (g1.FindControl("lbldesignation") as Label).Text;

                        con.Open();
                        SqlCommand cmdtable = new SqlCommand("insert into tblCustomerContactPerson(CustName,ContactPerName,ContactPerNo,cust_id,CreatedBy,Email,designation)values('" + txtCustName.Text + "','" + personname + "','" + personno + "','" + id + "','" + createdby + "','" + email + "','" + designation + "')", con);
                        cmdtable.ExecuteNonQuery();
                        con.Close();
                    }
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
                SqlCommand cmd = new SqlCommand("SP_CustomerMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerName", txtCustName.Text);
                cmd.Parameters.AddWithValue("@GSTNo", txtgstno.Text);
                cmd.Parameters.AddWithValue("@StateCode", DropDownListcustomer.Text);
                cmd.Parameters.AddWithValue("@PanNo", txtpanno.Text);
                cmd.Parameters.AddWithValue("@AddresLine1", txtAddresline1.Text);
                cmd.Parameters.AddWithValue("@AddresLine2", txtadreLine2.Text);
                cmd.Parameters.AddWithValue("@AddresLine3", txtadreLine3.Text);
                cmd.Parameters.AddWithValue("@Area", txtarea.Text);
                cmd.Parameters.AddWithValue("@City", txtcity.Text);
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@Country", txtcountry.Text);
                cmd.Parameters.AddWithValue("@MobNo", txtMobileNo.Text);
                cmd.Parameters.AddWithValue("@PostalCode", txtPostalCode.Text);
                cmd.Parameters.AddWithValue("@createdBy", createdby);
                cmd.Parameters.AddWithValue("@createddate", Date);
                cmd.Parameters.AddWithValue("@UpdatedBy", createdby);
                cmd.Parameters.AddWithValue("@UpdatedDate", Date);
                cmd.Parameters.AddWithValue("@isdeleted", '0');

                //19 - 11 - 2024 Added by Nikhil to update the Enquiry master table details for inward entry purpose here we are not updating enquiry table 
                cmd.Parameters.AddWithValue("@OldEnqId", null);

                cmd.Parameters.AddWithValue("@Custid", Convert.ToInt32(hidden.Value));
                cmd.Parameters.Add("@cust_id", SqlDbType.Int).Direction = ParameterDirection.Output;
                id = Convert.ToInt32(cmd.Parameters["@cust_id"].Value);
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
                ///delete First
                SqlCommand cmddelete = new SqlCommand("DELETE FROM tblCustomerContactPerson WHERE cust_id=@cust_id", con);
                cmddelete.Parameters.AddWithValue("@cust_id", Convert.ToInt32(hidden.Value));
                con.Open();
                cmddelete.ExecuteNonQuery();
                con.Close();
                ///Insert Then
                foreach (GridViewRow g1 in gv_Customercontact.Rows)
                {
                    string personname = (g1.FindControl("lblCustomername") as Label).Text;
                    string personno = (g1.FindControl("lblCustomernameno") as Label).Text;
                    string email = (g1.FindControl("lblemailperson") as Label).Text;
                    string designation = (g1.FindControl("lbldesignation") as Label).Text;

                    con.Open();
                    SqlCommand cmdtable = new SqlCommand("insert into tblCustomerContactPerson(CustName,ContactPerName,ContactPerNo,cust_id,CreatedBy,Email,designation)values('" + txtCustName.Text + "','" + personname + "','" + personno + "','" + Convert.ToInt32(hidden.Value) + "','" + createdby + "','" + email + "','" + designation + "')", con);
                    cmdtable.ExecuteNonQuery();
                    con.Close();
                }
                ///redirect pages
                if (Request.QueryString["Name"] != null)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Updated Successfully','0');", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Updated Successfully','1');", true);
                }
            }            
            // Code by Nikhil to edit Invert Entry page customer to edit Invert Entry page customer
            else if (btnSubmit.Text == "Save & Go Back")
            {

                string value = hidden.Value;

                string[] idParts = value.Split(',');
                string primaryId = idParts[0].Trim();
                string secondaryId = idParts.Length > 1 ? idParts[1].Trim() : string.Empty;

                DateTime Date = DateTime.Now;
                con.Open();
                SqlCommand cmd = new SqlCommand("SP_CustomerMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerName", txtCustName.Text);
                cmd.Parameters.AddWithValue("@GSTNo", txtgstno.Text);
                cmd.Parameters.AddWithValue("@StateCode", DropDownListcustomer.Text);
                cmd.Parameters.AddWithValue("@PanNo", txtpanno.Text);
                cmd.Parameters.AddWithValue("@AddresLine1", txtAddresline1.Text);
                cmd.Parameters.AddWithValue("@AddresLine2", txtadreLine2.Text);
                cmd.Parameters.AddWithValue("@AddresLine3", txtadreLine3.Text);
                cmd.Parameters.AddWithValue("@Area", txtarea.Text);
                cmd.Parameters.AddWithValue("@City", txtcity.Text);
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@Country", txtcountry.Text);
                cmd.Parameters.AddWithValue("@MobNo", txtMobileNo.Text);
                cmd.Parameters.AddWithValue("@PostalCode", txtPostalCode.Text);
                cmd.Parameters.AddWithValue("@createdBy", createdby);
                cmd.Parameters.AddWithValue("@createddate", Date);
                cmd.Parameters.AddWithValue("@UpdatedBy", createdby);
                cmd.Parameters.AddWithValue("@UpdatedDate", Date);
                cmd.Parameters.AddWithValue("@isdeleted", '0');
                
                //19 - 11 - 2024 Added by Nikhil to update the Enquiry master table details for inward entry purpose
                cmd.Parameters.AddWithValue("@OldEnqId", secondaryId);
                
                cmd.Parameters.AddWithValue("@Custid", Convert.ToInt32(primaryId));
                cmd.Parameters.Add("@cust_id", SqlDbType.Int).Direction = ParameterDirection.Output;
                id = Convert.ToInt32(cmd.Parameters["@cust_id"].Value);
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
                ///delete First
                SqlCommand cmddelete = new SqlCommand("DELETE FROM tblCustomerContactPerson WHERE cust_id=@cust_id", con);
                cmddelete.Parameters.AddWithValue("@cust_id", Convert.ToInt32(primaryId));
                con.Open();
                cmddelete.ExecuteNonQuery();
                con.Close();
                ///Insert Then
                foreach (GridViewRow g1 in gv_Customercontact.Rows)
                {
                    string personname = (g1.FindControl("lblCustomername") as Label).Text;
                    string personno = (g1.FindControl("lblCustomernameno") as Label).Text;
                    string email = (g1.FindControl("lblemailperson") as Label).Text;
                    string designation = (g1.FindControl("lbldesignation") as Label).Text;

                    con.Open();
                   // SqlCommand cmdtable = new SqlCommand("insert into tblCustomerContactPerson(CustName,ContactPerName,ContactPerNo,cust_id,CreatedBy,Email,designation)values('" + txtCustName.Text + "','" + personname + "','" + personno + "','" + Convert.ToInt32(hidden.Value) + "','" + createdby + "','" + email + "','" + designation + "')", con);
                    SqlCommand cmdtable = new SqlCommand("insert into tblCustomerContactPerson(CustName,ContactPerName,ContactPerNo,cust_id,CreatedBy,Email,designation)values('" + txtCustName.Text + "','" + personname + "','" + personno + "','" + primaryId + "','" + createdby + "','" + email + "','" + designation + "')", con);
                    cmdtable.ExecuteNonQuery();
                    con.Close();
                }

                string formattedValue = $"{primaryId} , {secondaryId}";
                string encryptedValue = encrypts(formattedValue);
                string redirectUrl = "InwardEntry.aspx?CUID=" + encryptedValue;


                if (Request.QueryString["Name"] != null)
                {
                    ClientScript.RegisterStartupScript(
                        this.GetType(),
                        "alertRedirect",
                        $"HideLabel('Data Updated Successfully', '0', '{redirectUrl}');",
                        true
                    );
                }
                else
                {
                    ClientScript.RegisterStartupScript(
                        this.GetType(),
                        "alertRedirect",
                        $"HideLabel('Data Updated Successfully', '0', '{redirectUrl}');",
                        true
                    );
                }


            }
            //Code by Nikhil to update the existing customer from enquiry page 
            else if (btnSubmit.Text == "Edit")
            {
                DateTime Date = DateTime.Now;
                con.Open();
                SqlCommand cmd = new SqlCommand("SP_CustomerMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerName", txtCustName.Text);
                cmd.Parameters.AddWithValue("@GSTNo", txtgstno.Text);
                cmd.Parameters.AddWithValue("@StateCode", DropDownListcustomer.Text);
                cmd.Parameters.AddWithValue("@PanNo", txtpanno.Text);
                cmd.Parameters.AddWithValue("@AddresLine1", txtAddresline1.Text);
                cmd.Parameters.AddWithValue("@AddresLine2", txtadreLine2.Text);
                cmd.Parameters.AddWithValue("@AddresLine3", txtadreLine3.Text);
                cmd.Parameters.AddWithValue("@Area", txtarea.Text);
                cmd.Parameters.AddWithValue("@City", txtcity.Text);
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@Country", txtcountry.Text);
                cmd.Parameters.AddWithValue("@MobNo", txtMobileNo.Text);
                cmd.Parameters.AddWithValue("@PostalCode", txtPostalCode.Text);
                cmd.Parameters.AddWithValue("@createdBy", createdby);
                cmd.Parameters.AddWithValue("@createddate", Date);
                cmd.Parameters.AddWithValue("@UpdatedBy", createdby);
                cmd.Parameters.AddWithValue("@UpdatedDate", Date);
                cmd.Parameters.AddWithValue("@isdeleted", '0');

                //19 - 11 - 2024 Added by Nikhil to update the Enquiry master table details for inward entry purpose here we are not updating enquiry table 
                cmd.Parameters.AddWithValue("@OldEnqId", null);

                cmd.Parameters.AddWithValue("@Custid", Convert.ToInt32(hidden.Value));
                cmd.Parameters.Add("@cust_id", SqlDbType.Int).Direction = ParameterDirection.Output;
                id = Convert.ToInt32(cmd.Parameters["@cust_id"].Value);
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
                ///delete First
                SqlCommand cmddelete = new SqlCommand("DELETE FROM tblCustomerContactPerson WHERE cust_id=@cust_id", con);
                cmddelete.Parameters.AddWithValue("@cust_id", Convert.ToInt32(hidden.Value));
                con.Open();
                cmddelete.ExecuteNonQuery();
                con.Close();
                ///Insert Then
                foreach (GridViewRow g1 in gv_Customercontact.Rows)
                {
                    string personname = (g1.FindControl("lblCustomername") as Label).Text;
                    string personno = (g1.FindControl("lblCustomernameno") as Label).Text;
                    string email = (g1.FindControl("lblemailperson") as Label).Text;
                    string designation = (g1.FindControl("lbldesignation") as Label).Text;

                    con.Open();
                    SqlCommand cmdtable = new SqlCommand("insert into tblCustomerContactPerson(CustName,ContactPerName,ContactPerNo,cust_id,CreatedBy,Email,designation)values('" + txtCustName.Text + "','" + personname + "','" + personno + "','" + Convert.ToInt32(hidden.Value) + "','" + createdby + "','" + email + "','" + designation + "')", con);
                    cmdtable.ExecuteNonQuery();
                    con.Close();
                }
                string formattedValue = hidden.Value;
                string encryptedValue = encrypts(formattedValue);
                string redirectUrl = "Enquiry.aspx?CUID=" + encryptedValue;


                if (Request.QueryString["Name"] != null)
                {
                    ClientScript.RegisterStartupScript(
                        this.GetType(),
                        "alertRedirect",
                        $"HideLabel('Data Updated Successfully', '0', '{redirectUrl}');",
                        true
                    );
                }
                else
                {
                    ClientScript.RegisterStartupScript(
                        this.GetType(),
                        "alertRedirect",
                        $"HideLabel('Data Updated Successfully', '0', '{redirectUrl}');",
                        true
                    );
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
                txtgstno.Text = dt.Rows[0]["GSTNo"].ToString();
                DropDownListcustomer.Text = dt.Rows[0]["StateCode"].ToString();
                txtpanno.Text = dt.Rows[0]["PanNo"].ToString();
                txtEmail.Text = dt.Rows[0]["Email"].ToString();
                txtAddresline1.Text = dt.Rows[0]["AddresLine1"].ToString();
                txtadreLine2.Text = dt.Rows[0]["AddresLine2"].ToString();
                txtadreLine3.Text = dt.Rows[0]["AddresLine3"].ToString();
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
                DataTable Dtproduct = new DataTable();
                SqlDataAdapter daa = new SqlDataAdapter("SELECT  [Id],[CustName],[ContactPerName],[ContactPerNo],[CreatedBy],[Createddate],[UpdatedBy],[UpdatedDate],[isdeleted],[cust_id],Email,designation FROM tblCustomerContactPerson WHERE CustName='" + Customername + "'  ", con);
                daa.Fill(Dtproduct);
                int count = 1;
                if (Dtproduct.Rows.Count > 0)
                {
                    if (dtpers.Columns.Count < 1)
                    {
                        ShowRecord();
                    }
                    for (int i = 0; i < Dtproduct.Rows.Count; i++)
                    {
                        dtpers.Rows.Add(count, Dtproduct.Rows[i]["ContactPerName"].ToString(), Dtproduct.Rows[i]["ContactPerNo"].ToString(), Dtproduct.Rows[i]["Email"].ToString(), Dtproduct.Rows[i]["designation"].ToString());
                        count = count + 1;
                    }
                }
                gv_Customercontact.DataSource = dtpers;
                gv_Customercontact.DataBind();
            }
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //txtCustName.Text = ""; txtgstno.Text = ""; DropDownListcustomer.Text = ""; txtpanno.Text = ""; txtAddresline1.Text = ""; txtadreLine2.Text = "";
        //txtadreLine3.Text = ""; txtarea.Text = ""; txtEmail.Text = ""; txtcity.Text = ""; txtcountry.Text = ""; txtMobileNo.Text = ""; txtPostalCode.Text = "";
        //txtContactPerson1.Text = ""; txtContactPerson2.Text = ""; txtContactPersonNo1.Text = ""; txtContactPersonNo2.Text = "";
        Response.Redirect("CustomerList.aspx");
    }

    protected void loadData(string id)
    {
        try
        {
          
            string[] idParts = id.Split(',');
            string primaryId = idParts[0].Trim();
            string secondaryId = idParts.Length > 1 ? idParts[1].Trim() : string.Empty;


            SqlDataAdapter sad = new SqlDataAdapter("SELECT [Custid],[CustomerName],[GSTNo],[StateCode],[PanNo],[AddresLine1],[AddresLine2],[AddresLine3],[Area],[Email],[City],[Country],[MobNo],[PostalCode],[ContactPerName1],[ContactPerNo1],[ContactPerName2],[ContactPerNo2],[IsStatus],[CreatedBy],[Createddate],[UpdatedBy],[UpdatedDate] FROM [tblCustomer] where Custid='" + primaryId + "'", con);
            DataTable dt = new DataTable();
            sad.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                txtCustName.Text = dt.Rows[0]["CustomerName"].ToString();
                txtgstno.Text = dt.Rows[0]["GSTNo"].ToString();
                DropDownListcustomer.Text = dt.Rows[0]["StateCode"].ToString();
                txtpanno.Text = dt.Rows[0]["PanNo"].ToString();
                txtAddresline1.Text = dt.Rows[0]["AddresLine1"].ToString();
                txtadreLine2.Text = dt.Rows[0]["AddresLine2"].ToString();
                txtadreLine3.Text = dt.Rows[0]["AddresLine3"].ToString();
                txtarea.Text = dt.Rows[0]["Area"].ToString();
                txtcity.Text = dt.Rows[0]["City"].ToString();
                txtcountry.Text = dt.Rows[0]["Country"].ToString();
                txtPostalCode.Text = dt.Rows[0]["PostalCode"].ToString();
                txtMobileNo.Text = dt.Rows[0]["MobNo"].ToString();
                txtEmail.Text = dt.Rows[0]["Email"].ToString();                
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
                DataTable Dtproduct = new DataTable();
                SqlDataAdapter daa = new SqlDataAdapter("SELECT  [Id],[CustName],[ContactPerName],[ContactPerNo],[CreatedBy],[Createddate],[UpdatedBy],[UpdatedDate],[isdeleted],[cust_id],Email,designation FROM tblCustomerContactPerson WHERE cust_id='" + primaryId + "'  ", con);
                daa.Fill(Dtproduct);
                int count = 1;
                if (Dtproduct.Rows.Count > 0)
                {
                    if (dtpers.Columns.Count < 1)
                    {
                        ShowRecord();
                    }
                    for (int i = 0; i < Dtproduct.Rows.Count; i++)
                    {
                        dtpers.Rows.Add(count, Dtproduct.Rows[i]["ContactPerName"].ToString(), Dtproduct.Rows[i]["ContactPerNo"].ToString(), Dtproduct.Rows[i]["Email"].ToString(), Dtproduct.Rows[i]["designation"].ToString());
                        count = count + 1;
                    }
                }
                gv_Customercontact.DataSource = dtpers;
                gv_Customercontact.DataBind();

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

    protected void btnadd_Click(object sender, EventArgs e)
    {
        if (txtcustomername.Text == "" && txtcustomernameno.Text == "")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Please fill Contact Details  !!!');", true);
        }
        else
        {
            ShowRecord();
        }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Success", "scrollToElement();", true);
    }

    protected void lnkbtnDelete_Click(object sender, EventArgs e)
    {
        GridViewRow row = (sender as LinkButton).NamingContainer as GridViewRow;
        DataTable dt = ViewState["TableContact"] as DataTable;
        dt.Rows.Remove(dt.Rows[row.RowIndex]);
        ViewState["TableContact"] = dt;
        gv_Customercontact.DataSource = (DataTable)ViewState["TableContact"];
        gv_Customercontact.DataBind();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Contact Delete Succesfully !!!');", true);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Success", "scrollToElement();", true);
    }

    protected void ShowRecord()
    {
        ViewState["RowNo"] = (int)ViewState["RowNo"] + 1;
        DataTable Dt = (DataTable)ViewState["TableContact"];
        Dt.Rows.Add(ViewState["RowNo"], txtcustomername.Text, txtcustomernameno.Text, txttemail.Text, txtdesignation.Text);
        ViewState["TableContact"] = Dt;
        txtcustomername.Text = string.Empty;
        txtcustomernameno.Text = string.Empty;
        txttemail.Text = string.Empty;
        txtdesignation.Text = string.Empty;
        gv_Customercontact.DataSource = (DataTable)ViewState["TableContact"];
        gv_Customercontact.DataBind();
    }


    public string encrypts(string encryptString)
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
}