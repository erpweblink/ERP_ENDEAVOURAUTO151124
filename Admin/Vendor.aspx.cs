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

public partial class Reception_Vendor : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString); 

    DataTable dtpers = new DataTable(); 
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["RowNo"] = 0;
            dtpers.Columns.AddRange(new DataColumn[5] { new DataColumn("colid"), new DataColumn("colFirst"), new DataColumn("colsecond"), new DataColumn("colemail"), new DataColumn("coldesignation") });

            ViewState["TableContact"] = dtpers;

            if (Request.QueryString["VendorId"] != null)
            {
                string id = Decrypt(Request.QueryString["VendorId"].ToString());
                loadData(id);
                vendorlbl.InnerText = "Update Vendor";
                btnSubmit.Text = "Update";
                hidden.Value = id;
            }
            else
            {

            }
        }
    }

    protected void loadData(string id)
    {
        try
        {
            SqlDataAdapter sad = new SqlDataAdapter("SELECT [VendorId],[VendorName],[GSTNo],[StateCode],[PanNo],[AddreLine1],[AddreLine2],[AddreLine3],[Area],[Email],[City],[Country],[MobNo],[PostalCode],[Contactper1],[ContactNumb1],[Contactper2],[ContactNumb2],[IsStatus],[CreatedBy],[CreatedDate],[UpdatedBy],[UpdatedDate] FROM [tblVendor] where Vendorid='" + id + "'", con);

            DataTable dt = new DataTable();
            sad.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                txtvendorName.Text = dt.Rows[0]["VendorName"].ToString();
                txtgstno.Text = dt.Rows[0]["GSTNo"].ToString();
                DropDownListvendor.Text = dt.Rows[0]["StateCode"].ToString();
                txtpanno.Text = dt.Rows[0]["PanNo"].ToString();
                txtAddresline1.Text = dt.Rows[0]["AddreLine1"].ToString();
                txtadreLine2.Text = dt.Rows[0]["AddreLine2"].ToString();
                txtadreLine3.Text = dt.Rows[0]["AddreLine3"].ToString();
                txtarea.Text = dt.Rows[0]["Area"].ToString();
                txtcity.Text = dt.Rows[0]["City"].ToString();
                txtcountry.Text = dt.Rows[0]["Country"].ToString();
                txtPostalCode.Text = dt.Rows[0]["PostalCode"].ToString();
                txtMobileNo.Text = dt.Rows[0]["MobNo"].ToString();
                //txtEmail.Text = dt.Rows[0]["Email"].ToString();

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
                SqlDataAdapter daa = new SqlDataAdapter("SELECT  [id],[vend_id],[vendorName],[ContactPerName],[ContactPerNo],[CreatedBy],[Createddate],[UpdatedBy],[UpdatedDate],[isdeleted],designation,Email FROM [tblVendorContact] WHERE vend_id='" + id + "'  ", con);
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
                gv_vendorcontact.DataSource = dtpers;
                gv_vendorcontact.DataBind();
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

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        //  string createdby = "Admin";
        string createdby = Session["adminname"].ToString();
        int id;
        try
        {
            if (btnSubmit.Text == "Submit")
            {
                con.Open();
                SqlCommand cmd1 = new SqlCommand("SELECT [VendorId],[VendorName],[GSTNo],[StateCode],[PanNo],[AddreLine1],[AddreLine2],[AddreLine3],[Area],[Email],[City],[Country],[MobNo],[PostalCode],[Contactper1],[ContactNumb1],[Contactper2],[ContactNumb2],[IsStatus],[CreatedBy],[CreatedDate],[UpdatedBy],[UpdatedDate] FROM [tblVendor] where isdeleted='0' AND [VendorName]='" + txtvendorName.Text + "'  AND [MobNo]='" + txtMobileNo.Text + "' ", con);
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
                    SqlCommand cmd = new SqlCommand("SP_VendorMaster", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@VendorName", txtvendorName.Text);
                    cmd.Parameters.AddWithValue("@GSTNo", txtgstno.Text);
                    cmd.Parameters.AddWithValue("@StateCode", DropDownListvendor.Text);
                    cmd.Parameters.AddWithValue("@PanNo", txtpanno.Text);
                    cmd.Parameters.AddWithValue("@AddresLine1", txtAddresline1.Text);
                    cmd.Parameters.AddWithValue("@AddresLine2", txtadreLine2.Text);
                    cmd.Parameters.AddWithValue("@AddresLine3", txtadreLine3.Text);
                    cmd.Parameters.AddWithValue("@Area", txtarea.Text);
                    cmd.Parameters.AddWithValue("@City", txtcity.Text);
                    cmd.Parameters.AddWithValue("@Country", txtcountry.Text);
                    cmd.Parameters.AddWithValue("@MobNo", txtMobileNo.Text);
                    cmd.Parameters.AddWithValue("@PostalCode", txtPostalCode.Text);
                    cmd.Parameters.AddWithValue("@createdBy", createdby);
                    cmd.Parameters.AddWithValue("@createddate", Date);
                    cmd.Parameters.AddWithValue("@isdeleted", '0');
                    cmd.Parameters.Add("@vend_id", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@Action", "Insert");

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
                    cmd.ExecuteNonQuery();
                    con.Close();
                    id = Convert.ToInt32(cmd.Parameters["@vend_id"].Value);
                    foreach (GridViewRow g1 in gv_vendorcontact.Rows)
                    {
                        string personname = (g1.FindControl("lblCustomername") as Label).Text;
                        string personno = (g1.FindControl("lblCustomernameno") as Label).Text;
                        string email = (g1.FindControl("lblemailperson") as Label).Text;
                        string designation = (g1.FindControl("lbldesignation") as Label).Text;
                        con.Open();
                        SqlCommand cmdtable = new SqlCommand("insert into tblVendorContact(vend_id,vendorName,ContactPerName,ContactPerNo,CreatedBy,Email,designation)values('" + id + "','" + txtvendorName.Text + "','" + personname + "','" + personno + "','" + createdby + "','" + email + "','" + designation + "')", con);
                        cmdtable.ExecuteNonQuery();
                        con.Close();
                    }

                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Saved Successfully');", true);
                }
            }
            else if (btnSubmit.Text == "Update")
            {
                DateTime Date = DateTime.Now;
                con.Open();
                SqlCommand cmd = new SqlCommand("SP_VendorMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@VendorName", txtvendorName.Text);
                cmd.Parameters.AddWithValue("@GSTNo", txtgstno.Text);
                cmd.Parameters.AddWithValue("@StateCode", DropDownListvendor.Text);
                cmd.Parameters.AddWithValue("@PanNo", txtpanno.Text);
                cmd.Parameters.AddWithValue("@AddresLine1", txtAddresline1.Text);
                cmd.Parameters.AddWithValue("@AddresLine2", txtadreLine2.Text);
                cmd.Parameters.AddWithValue("@AddresLine3", txtadreLine3.Text);
                cmd.Parameters.AddWithValue("@Area", txtarea.Text);
                cmd.Parameters.AddWithValue("@City", txtcity.Text);
                cmd.Parameters.AddWithValue("@Country", txtcountry.Text);
                cmd.Parameters.AddWithValue("@MobNo", txtMobileNo.Text);
                cmd.Parameters.AddWithValue("@PostalCode", txtPostalCode.Text);
                cmd.Parameters.AddWithValue("@createdBy", createdby);
                cmd.Parameters.AddWithValue("@createddate", Date);
                cmd.Parameters.AddWithValue("@UpdatedBy", createdby);
                cmd.Parameters.AddWithValue("@UpdatedDate", Date);
                cmd.Parameters.AddWithValue("@isdeleted", '0');

                cmd.Parameters.AddWithValue("@vendorId", Convert.ToInt32(hidden.Value));
                cmd.Parameters.Add("@vend_id", SqlDbType.Int).Direction = ParameterDirection.Output;

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

                DataTable Dt = new DataTable();
                SqlDataAdapter daa = new SqlDataAdapter("SELECT  [id],[vend_id],[vendorName],[ContactPerName],[ContactPerNo],[CreatedBy],[Createddate],[UpdatedBy],[UpdatedDate],[isdeleted] FROM [tblVendorContact] WHERE vend_id='" + Convert.ToInt32(hidden.Value) + "'  ", con);
                daa.Fill(Dt);
                //if (Dt.Rows.Count == gv_vendorcontact.Rows.Count)
                //{
                //    foreach (GridViewRow g2 in gv_vendorcontact.Rows)
                //    {
                //        string personname = (g2.FindControl("lblCustomername") as Label).Text;
                //        string personno = (g2.FindControl("lblCustomernameno") as Label).Text;
                //        string email = (g2.FindControl("lblemailperson") as Label).Text;
                //        string designation = (g2.FindControl("lbldesignation") as Label).Text;
                //        SqlCommand cmdupdate = new SqlCommand("Update tblVendorContact SET vendorName = '" + txtvendorName.Text + "',ContactPerName = '" + personname + "',ContactPerNo = '" + personno + "', UpdatedBy='" + createdby + "',Email='" + email + "',designation='" + designation + "' WHERE vend_id='" + Convert.ToInt32(hidden.Value) + "' AND ContactPerNo = '" + personno + "'", con);
                //        con.Open();
                //        cmdupdate.ExecuteNonQuery();
                //        con.Close();
                //    }
                //}
                //else
                //{
                SqlCommand cmddelete = new SqlCommand("DELETE FROM tblVendorContact WHERE vend_id=@vend_id", con);
                cmddelete.Parameters.AddWithValue("@vend_id", Convert.ToInt32(hidden.Value));
                con.Open();
                cmddelete.ExecuteNonQuery();
                con.Close();
                foreach (GridViewRow g1 in gv_vendorcontact.Rows)
                {
                    string personname = (g1.FindControl("lblCustomername") as Label).Text;
                    string personno = (g1.FindControl("lblCustomernameno") as Label).Text;
                    string email = (g1.FindControl("lblemailperson") as Label).Text;
                    string designation = (g1.FindControl("lbldesignation") as Label).Text;
                    con.Open();
                    SqlCommand cmdtable = new SqlCommand("insert into tblVendorContact(vend_id,vendorName,ContactPerName,ContactPerNo,CreatedBy,Email,designation)values('" + hidden.Value + "','" + txtvendorName.Text + "','" + personname + "','" + personno + "','" + createdby + "','" + email + "','" + designation + "')", con);

                    cmdtable.ExecuteNonQuery();
                    con.Close();
                }
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Updated Successfully');", true);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //txtvendorName.Text = ""; txtgstno.Text = ""; DropDownListvendor.Text = ""; txtpanno.Text = ""; txtAddresline1.Text = ""; txtadreLine2.Text = "";
        //txtadreLine3.Text = ""; txtarea.Text = ""; txtEmail.Text = ""; txtcity.Text = ""; txtcountry.Text = ""; txtMobileNo.Text = ""; txtPostalCode.Text = "";
        //txtContactPerson1.Text = ""; txtContactPerson2.Text = ""; txtContactPersonNo1.Text = ""; txtContactPersonNo2.Text = "";

        Response.Redirect("VendorList.aspx");
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
        gv_vendorcontact.DataSource = (DataTable)ViewState["TableContact"];
        gv_vendorcontact.DataBind();
    }

    protected void lnkbtnDelete_Click(object sender, EventArgs e)
    {
        GridViewRow row = (sender as LinkButton).NamingContainer as GridViewRow;

        DataTable dt = ViewState["TableContact"] as DataTable;
        dt.Rows.Remove(dt.Rows[row.RowIndex]);
        ViewState["TableContact"] = dt;
        gv_vendorcontact.DataSource = (DataTable)ViewState["TableContact"];
        gv_vendorcontact.DataBind();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Contact Delete Successfully !!!');", true);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Success", "scrollToElement();", true);
    }
}