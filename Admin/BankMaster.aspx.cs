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

public partial class Admin_BankMaster : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
    DataTable dt11 = new DataTable();
    UserAuthorization objUserAuthorization = new UserAuthorization();
    string create1, Delete1, Update1, view1, Report1;
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnSubmit.Text == "Submit")
            {
                con.Open();
                SqlCommand cmd1 = new SqlCommand("SELECT * FROM tbl_Bank WHERE BankName = '" + txtbankname + "' AND AccounNo ='" + txtaccountno + "'", con);
                SqlDataReader reader = cmd1.ExecuteReader();


                if (reader.Read())
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Bank Already Exist.');window.location ='AddBnak.aspx';", true);
                }
                else
                {
                    con.Close();
                    DateTime Date = DateTime.Now;
                    con.Open();
                    SqlCommand cmd = new SqlCommand("[EndeavourAuto].[SP_Banknew]", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "Insert");
                    cmd.Parameters.AddWithValue("@BankName", txtbankname.Text);
                    cmd.Parameters.AddWithValue("@AccounNo", txtaccountno.Text);
                    cmd.Parameters.AddWithValue("@IFSCCode", txtifsccode.Text);
                    cmd.Parameters.AddWithValue("@BranchName", txtbranchname.Text);
                    cmd.Parameters.AddWithValue("@Remark", txtremark.Text);
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
                SqlCommand cmd = new SqlCommand("[EndeavourAuto].[SP_Banknew]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BankName", txtbankname.Text);
                cmd.Parameters.AddWithValue("@AccounNo", txtaccountno.Text);
                cmd.Parameters.AddWithValue("@IFSCCode", txtaccountno.Text);
                cmd.Parameters.AddWithValue("@BranchName", txtbranchname.Text);
                cmd.Parameters.AddWithValue("@Remark", txtremark.Text);
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(hidden.Value));
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    //protected void LoadDataCustomer(string Customername)
    //{
    //    try
    //    {
    //        SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM tbl_Bank WHERE BankName = '" + txtbankname + "'", con);

    //        DataTable dt = new DataTable();
    //        sad.Fill(dt);
    //        if (dt.Rows.Count > 0)
    //        {
    //            txtbankname.Text = dt.Rows[0]["BankName"].ToString();
    //            txtaccountno.Text = dt.Rows[0]["AccounNo"].ToString();
    //            txtifsccode.Text = dt.Rows[0]["IFSCCode"].ToString();
    //            txtbranchname.Text = dt.Rows[0]["BranchName"].ToString();
    //            txtremark.Text = dt.Rows[0]["Remark"].ToString();
    //            gv_Customercontact.DataSource = dt;
    //            gv_Customercontact.DataBind();

    //        }
    //    catch (Exception ex)
    //    {

    //        throw ex;
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




    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("BankMaster.aspx");
    }
}