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
using System.Globalization;

public partial class Admin_staffMaster : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
    string id;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DateTime date1;

            if (Request.QueryString["Id"] != null)
            {

                id = Decrypt(Request.QueryString["Id"].ToString());
                loadData(id);
                btnSubmit.Text = "Update";
                hidden.Value = id;
            }
        }
    }

    protected void loadData(string id)
    {
        try
        {
            DataTable dt = new DataTable();
            SqlDataAdapter sad = new SqlDataAdapter("select * from tblStaffMaster where Id='" + id + "'", con);
            sad.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                txtName.Text = dt.Rows[0]["EmpName"].ToString();
                txtRefBy.Text = dt.Rows[0]["RefBy"].ToString();
                txtmob.Text = dt.Rows[0]["MobNo"].ToString();
                txtsalary.Text = dt.Rows[0]["salary"].ToString();
                txtemail.Text = dt.Rows[0]["Email"].ToString();
                txtsalaryAC.Text = dt.Rows[0]["SalaryAC"].ToString();
                txtTimeout.Text = dt.Rows[0]["Timeout"].ToString();
                DateTime ffff1 = Convert.ToDateTime(dt.Rows[0]["LeavingDate"].ToString());
                txtLeavingdate.Text = ffff1.ToString("yyyy-MM-dd");
                DateTime ffff2 = Convert.ToDateTime(dt.Rows[0]["DOJ"].ToString());
                txtdateJoining.Text = ffff2.ToString("yyyy-MM-dd");
                txtAddress.Text = dt.Rows[0]["PresentAddress"].ToString();
                txtpermantaddres.Text = dt.Rows[0]["permanantAddress"].ToString();
                txtExtraInfo.Text = dt.Rows[0]["ExtraInformation"].ToString();

                string flgStatus = "";
                if (dt.Rows[0]["IsActive"].ToString() == "False")
                {
                    flgStatus = "No";
                }
                else
                {
                    flgStatus = "Yes";
                }
                ddlisActive.Text = flgStatus;

                txtDesignation.Text = dt.Rows[0]["Designation"].ToString();
                string str = dt.Rows[0]["TimeIn"].ToString();
                string str1 = dt.Rows[0]["Timeout"].ToString();

                string[] arrstr = str.ToString().Split('-');
                string[] arrstr1 = str1.ToString().Split('-');


                if (arrstr.Length > 0)
                {
                    txtTimeIn.Text = arrstr[0].ToString();
                    ddltimein.Text = arrstr[1].ToString();
                }

                if (arrstr1.Length > 0)
                {
                    txtTimeout.Text = arrstr1[0].ToString();
                    ddltimeout.Text = arrstr1[1].ToString();
                }
                txtCommisionAC.Text = dt.Rows[0]["Bloodgroup"].ToString();
            }
        }
        catch (Exception)
        {

            throw;
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

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string createdby = Session["adminname"].ToString();
        try
        {
            if (btnSubmit.Text == "Submit")
            {
                con.Open();
                SqlCommand cmd1 = new SqlCommand("select * from tblStaffMaster where Email='" + txtemail.Text + "' AND MobNo='" + txtmob.Text + "'", con);
                SqlDataReader reader = cmd1.ExecuteReader();

                if (reader.Read())
                {

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert(' Employee already exist..!');", true);
                }
                else
                {
                    con.Close();

                    DateTime Date = DateTime.Now;
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SP_StaffMaster", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EmpName", txtName.Text);
                    cmd.Parameters.AddWithValue("@PresentAddress", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@MobNo", txtmob.Text);
                    cmd.Parameters.AddWithValue("@Email", txtemail.Text);
                    cmd.Parameters.AddWithValue("@salary", txtsalary.Text);
                    cmd.Parameters.AddWithValue("@DOJ", txtdateJoining.Text);
                    cmd.Parameters.AddWithValue("@LeavingDate", txtLeavingdate.Text);
                    cmd.Parameters.AddWithValue("@Bloodgroup", txtCommisionAC.Text);
                    cmd.Parameters.AddWithValue("@SalaryAC", txtsalaryAC.Text);
                    cmd.Parameters.AddWithValue("@Designation", txtDesignation.Text);
                    cmd.Parameters.AddWithValue("@RefBy", txtRefBy.Text);
                    cmd.Parameters.AddWithValue("@permanantAddress", txtpermantaddres.Text);
                    cmd.Parameters.AddWithValue("@ExtraInformation", txtExtraInfo.Text);
                    cmd.Parameters.AddWithValue("@CreatedBy", createdby);
                    cmd.Parameters.AddWithValue("@Createddate", Date);
                    cmd.Parameters.AddWithValue("@isdeleted", '0');

                    cmd.Parameters.AddWithValue("@TimeIn", txtTimeIn.Text + "-" + ddltimein.Text);
                    cmd.Parameters.AddWithValue("@Timeout", txtTimeout.Text + "-" + ddltimeout.Text);

                    bool isactive = true;
                    if (ddlisActive.Text == "Yes")
                    {
                        isactive = true;
                    }
                    else
                    {
                        isactive = false;
                    }

                    cmd.Parameters.AddWithValue("@IsActive", isactive);
                    cmd.Parameters.AddWithValue("@Action", "Insert");
                    cmd.ExecuteNonQuery();
                    con.Close();
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabe('Data Saved Successfully');", true);
                    //Response.Redirect("StaffList.aspx");
                }
            }
            else if (btnSubmit.Text == "Update")
            {
                con.Close();

                DateTime Date = DateTime.Now;
                con.Open();
                SqlCommand cmd = new SqlCommand("SP_StaffMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmpName", txtName.Text);
                cmd.Parameters.AddWithValue("@PresentAddress", txtAddress.Text);
                cmd.Parameters.AddWithValue("@MobNo", txtmob.Text);
                cmd.Parameters.AddWithValue("@Email", txtemail.Text);
                cmd.Parameters.AddWithValue("@salary", txtsalary.Text);
                cmd.Parameters.AddWithValue("@DOJ", txtdateJoining.Text);
                cmd.Parameters.AddWithValue("@LeavingDate", txtLeavingdate.Text);
                cmd.Parameters.AddWithValue("@Bloodgroup", txtCommisionAC.Text);
                cmd.Parameters.AddWithValue("@SalaryAC", txtsalaryAC.Text);
                cmd.Parameters.AddWithValue("@Designation", txtDesignation.Text);
                cmd.Parameters.AddWithValue("@RefBy", txtRefBy.Text);
                cmd.Parameters.AddWithValue("@permanantAddress", txtpermantaddres.Text);
                cmd.Parameters.AddWithValue("@ExtraInformation", txtExtraInfo.Text);
                cmd.Parameters.AddWithValue("@UpdatedBY", createdby);
                cmd.Parameters.AddWithValue("@UpdatedDate", Date);
                cmd.Parameters.AddWithValue("@isdeleted", '0');
                cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(hidden.Value));
                bool isactive = true;
                if (ddlisActive.Text == "Yes")
                {
                    isactive = true;
                }
                else
                {
                    isactive = false;
                }
                cmd.Parameters.AddWithValue("@TimeIn", txtTimeIn.Text + "-" + ddltimein.Text);
                cmd.Parameters.AddWithValue("@Timeout", txtTimeout.Text + "-" + ddltimeout.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@IsActive", isactive);
                cmd.Parameters.AddWithValue("@Action", "Update");
                cmd.ExecuteNonQuery();
                con.Close();

                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabe('Data Updated Successfully');", true);
                
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("StaffList.aspx");
    }
}