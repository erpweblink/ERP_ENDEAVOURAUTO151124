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

public partial class Reception_OutwardEntry : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
    string id = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.txtDateout.Text = DateTime.Now.ToString("yyyy-MM-dd");
            this.txtDateout.TextMode = TextBoxMode.Date;
            //this.txtReturnDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            //this.txtReturnDate.TextMode = TextBoxMode.Date;
            //this.txtrepeateddate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            //this.txtrepeateddate.TextMode = TextBoxMode.Date;
             //ddlUser();
            ddlstatusrepairing();
            //ddlCustomer();
            if (Request.QueryString["Id"] != null)
            {
                id = Decrypt(Request.QueryString["Id"].ToString());
                loadData(id);
                btnSubmit.Text = "Update";
                hidden.Value = id;
            }
            else
            {
                //GenerateCode();
                //int jobid = GenerateCode();
                //string jobcode = String.Concat("JOB", jobid);
                //txtJobNo.Text = jobcode;
            }
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        //string createdby = "Admin";
        ////createdby = Session["adminname"].ToString();
        string Path = null;
        if (btnSubmit.Text == "Submit")
        {
            //if (DropDownJobWork.SelectedItem.Text == "Select Employee")
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Employee Name');", true);
            //}
            //else
            //{
                DateTime Date = DateTime.Now;
                con.Open();
                SqlCommand cmd = new SqlCommand("SP_OutwardEntry", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@JobNo", txtJobNo.Text);
                cmd.Parameters.AddWithValue("@DateOut", txtDateout.Text);
                cmd.Parameters.AddWithValue("@CustName", txtcustomer.Text);
                cmd.Parameters.AddWithValue("@MateName", txtmaterName.Text);
                cmd.Parameters.AddWithValue("@ModelNo", txtModelNo.Text);
                cmd.Parameters.AddWithValue("@SerialNo", txtSrNo.Text);
              //  cmd.Parameters.AddWithValue("@JobWorkby", DropDownJobWork.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@Dispatchdate", txtdispatchdate.Text);
                cmd.Parameters.AddWithValue("@DateReturn", txtReturnDate.Text);
                cmd.Parameters.AddWithValue("@repeatedNo", txtrepeatedno.Text);
                cmd.Parameters.AddWithValue("@repeatedDate", txtrepeateddate.Text);
                cmd.Parameters.AddWithValue("@Courier", txtcourier.Text);
                if (FileUpload.HasFile)
                {
                    var Filenamenew = FileUpload.FileName;
                    string codenew = Guid.NewGuid().ToString();
                    Path = Server.MapPath("~/ProductImg/") + codenew + "_" + Filenamenew;
                    FileUpload.SaveAs(Path);
                    cmd.Parameters.AddWithValue("@Imagepath", "~/ProductImg/" + codenew + "_" + Filenamenew);
                }
                //cmd.Parameters.AddWithValue("@Imagepath", "~/ProductImg/" + FileUpload.FileName);
                cmd.Parameters.AddWithValue("@againstby", ddlservicetype.SelectedValue);
                cmd.Parameters.AddWithValue("@ReturnRepair", DropDownrepaid.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@CreateBy", Session["adminname"].ToString());
                cmd.Parameters.AddWithValue("@CreatedDate", Date);
                cmd.Parameters.AddWithValue("@UpdateBy", null);
                cmd.Parameters.AddWithValue("@UpdateDate", Date);
                cmd.Parameters.AddWithValue("@isdeleted", '0');
                cmd.Parameters.AddWithValue("@Action", "Insert");

                cmd.ExecuteNonQuery();
                con.Close();
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Saved Successfully');", true);
           // }
        }

        else if (btnSubmit.Text == "Update")
        {
            DateTime Date = DateTime.Now;


            con.Open();
            SqlCommand cmd = new SqlCommand("SP_OutwardEntry", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@JobNo", txtJobNo.Text);
            cmd.Parameters.AddWithValue("@DateOut", txtDateout.Text);
            cmd.Parameters.AddWithValue("@CustName", txtcustomer.Text);
            cmd.Parameters.AddWithValue("@MateName", txtmaterName.Text);
            cmd.Parameters.AddWithValue("@ModelNo", txtModelNo.Text);
            cmd.Parameters.AddWithValue("@SerialNo", txtSrNo.Text);
            //cmd.Parameters.AddWithValue("@JobWorkby", DropDownJobWork.SelectedItem.Text);
            cmd.Parameters.AddWithValue("@Dispatchdate", txtdispatchdate.Text);
            cmd.Parameters.AddWithValue("@DateReturn", txtReturnDate.Text);
            cmd.Parameters.AddWithValue("@ReturnRepair", DropDownrepaid.SelectedItem.Text);
            cmd.Parameters.AddWithValue("@CreateBy", Session["adminname"].ToString());
            cmd.Parameters.AddWithValue("@CreatedDate", Date);
            cmd.Parameters.AddWithValue("@UpdateBy", Session["adminname"].ToString());
            cmd.Parameters.AddWithValue("@UpdateDate", Date);
            cmd.Parameters.AddWithValue("@repeatedNo", txtrepeatedno.Text);
            cmd.Parameters.AddWithValue("@repeatedDate", txtrepeateddate.Text);
            cmd.Parameters.AddWithValue("@Courier", txtcourier.Text);
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
            cmd.Parameters.AddWithValue("@againstby", ddlservicetype.SelectedValue);
            cmd.Parameters.AddWithValue("@isdeleted", '0');
            cmd.Parameters.AddWithValue("@Action", "update");

            cmd.ExecuteNonQuery();
            con.Close();
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Updated Successfully');", true);
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
    //protected void GenerateCode()
    //{
    //    SqlDataAdapter ad = new SqlDataAdapter("SELECT max([id]) as maxid FROM [tblOutwardEntry]", con);
    //    DataTable dt = new DataTable();
    //    ad.Fill(dt);
    //    if (dt.Rows.Count > 0)
    //    {
    //        int maxid = dt.Rows[0]["maxid"].ToString() == "" ? 0 : Convert.ToInt32(dt.Rows[0]["maxid"].ToString());
    //        txtJobNo.Text = "JOB"+(maxid + 1).ToString();
    //    }
    //    else
    //    {
    //        txtJobNo.Text = string.Empty;
    //    }
    //}
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("OutwardEntryList.aspx");
    }
    protected void loadData(string id)
    {
        try
        {
            SqlDataAdapter sad = new SqlDataAdapter("select [Id],[JobNo],[DateOut],[CustName],[MateName],[ModelNo],[SerialNo],[JobWorkby],[Dispatchdate],[DateReturn],[ReturnRepair],[CreateBy],[CreatedDate],[UpdateBy],[UpdateDate],repeatedDate,repeatedNo,Courier,Imagepath,againstby from [tblOutwardEntry] where JobNo='" + id + "'", con);

            DataTable dt = new DataTable();
            sad.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                txtJobNo.Text = dt.Rows[0]["JobNo"].ToString();
                DateTime ffff1 = Convert.ToDateTime(dt.Rows[0]["DateOut"].ToString());
                txtDateout.Text = ffff1.ToString("yyyy-MM-dd");

                txtcustomer.Text = dt.Rows[0]["CustName"].ToString();
                txtrepeatedno.Text = dt.Rows[0]["repeatedNo"].ToString();

                txtmaterName.Text = dt.Rows[0]["MateName"].ToString();
                txtSrNo.Text = dt.Rows[0]["SerialNo"].ToString();

                txtModelNo.Text = dt.Rows[0]["ModelNo"].ToString();
              //  DropDownJobWork.SelectedItem.Text = dt.Rows[0]["JobWorkby"].ToString();

                //txtdispatchdate.Text = dt.Rows[0]["Dispatchdate"].ToString();

                DateTime ffff2 = Convert.ToDateTime(dt.Rows[0]["DateReturn"].ToString());
                txtReturnDate.Text = ffff1.ToString("yyyy-MM-dd");

                DateTime fffff3 = Convert.ToDateTime(dt.Rows[0]["Dispatchdate"].ToString());
                txtdispatchdate.Text = fffff3.ToString("yyyy-MM-dd");

                DateTime ffff3 = Convert.ToDateTime(dt.Rows[0]["repeatedDate"].ToString());
                txtrepeateddate.Text = ffff3.ToString("yyyy-MM-dd");

                DropDownrepaid.SelectedItem.Text = dt.Rows[0]["ReturnRepair"].ToString();
                txtcourier.Text = dt.Rows[0]["Courier"].ToString();
                lblPath.Text = dt.Rows[0]["Imagepath"].ToString();
                ddlservicetype.Text = dt.Rows[0]["againstby"].ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    //protected void ddlUser()
    //{
    //    try
    //    {
    //        DataTable dt = new DataTable();

    //        con.Open();
    //        SqlDataAdapter sad = new SqlDataAdapter("select * from tblStaffMaster where isdeleted='0'", con);
    //        sad.Fill(dt);
    //        DropDownJobWork.DataValueField = "Id";
    //        DropDownJobWork.DataTextField = "EmpName";

    //        DropDownJobWork.DataSource = dt;
    //        DropDownJobWork.DataBind();

    //        con.Close();
    //    }
    //    catch (Exception)
    //    {

    //        throw;
    //    }
    //}
    //protected void ddlCustomer()
    //{
    //    try
    //    {
    //        DataTable dt = new DataTable();

    //        con.Open();
    //        SqlDataAdapter sad = new SqlDataAdapter("select [Custid],[CustomerName],[GSTNo],[StateCode],[PanNo],[AddresLine1],[AddresLine2],[AddresLine3],[Area],[Email],[City],[Country],[MobNo],[PostalCode],[ContactPerName1],[ContactPerNo1],[ContactPerName2],[ContactPerNo2],[IsStatus],[CreatedBy],[Createddate],[UpdatedBy],[UpdatedDate] FROM [tblCustomer] where isdeleted='0' AND IsStatus='1'", con);
    //        sad.Fill(dt);
    //        DropDownCust.DataValueField = "Custid";
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



    protected void txtJobNo_TextChanged(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblInwardEntry where JobNo='" + txtJobNo.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            txtcustomer.Text = dt.Rows[0]["CustName"].ToString();
            txtmaterName.Text = dt.Rows[0]["MateName"].ToString();
            txtModelNo.Text = dt.Rows[0]["ModelNo"].ToString();
            txtSrNo.Text = dt.Rows[0]["SrNo"].ToString();

            DateTime ffff88 = Convert.ToDateTime(dt.Rows[0]["RepeatedDate"].ToString());
            txtReturnDate.Text = ffff88.ToString("yyyy-MM-dd");

            DateTime ffff69 = Convert.ToDateTime(dt.Rows[0]["RepeatedDate"].ToString());
            txtrepeateddate.Text = ffff69.ToString("yyyy-MM-dd");
        }
    }
    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetjobList(string prefixText, int count)
    {
        return AutoFilljoblist(prefixText);
    }

    public static List<string> AutoFilljoblist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT JobNo from tblInwardEntry where " + "JobNo like @Search + '%' AND isdeleted='0'";

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

    protected void lnkproduct_Click(object sender, EventArgs e)
    {
        modelprofile.Show();
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            con.Open();
            SqlCommand cmd1 = new SqlCommand("select [Id],[Status] from RepairingStatusOutward where Status='" + txtstatus.Text + "'  ", con);
            SqlDataReader reader = cmd1.ExecuteReader();
            if (reader.Read())
            {
                con.Close();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Data Already Exist.');", true);
            }
            else
            {
                con.Close();
                con.Open();
                SqlCommand cmd = new SqlCommand("insert into RepairingStatusOutward(Status)values('" + txtstatus.Text + "')", con);
                cmd.ExecuteNonQuery();
                con.Close();
                //txtstatus.Text = "";
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void ddlstatusrepairing()
    {
        try
        {
            DataTable dt = new DataTable();

            con.Open();
            SqlDataAdapter sad = new SqlDataAdapter("select [Id],[Status] from RepairingStatusOutward", con);
            sad.Fill(dt);
            DropDownrepaid.DataValueField = "Id";
            DropDownrepaid.DataTextField = "Status";

            DropDownrepaid.DataSource = dt;
            DropDownrepaid.DataBind();

            con.Close();
        }
        catch (Exception)
        {
            throw;
        }
    }
}