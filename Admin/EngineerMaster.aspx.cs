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

public partial class Admin_EngineerMaster : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);

    string id;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //RoleGrid();
            if (Request.QueryString["Id"] != null)
            {
                string id = Decrypt(Request.QueryString["Id"].ToString());
                loadData(id);
                btnSubmit.Text = "Update";
                hidden.Value = id;
            }
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        try
        {
            DataTable Dt = new DataTable();
            SqlDataAdapter Da = new SqlDataAdapter("SELECT * FROM [tbl_Engineer] WHERE EngineerName ='" + txtengineername.Text + "'", con);
            Da.Fill(Dt);

            if (btnSubmit.Text == "Update")
            {
                SqlCommand Cmd = new SqlCommand("UPDATE tbl_Engineer  SET  EngineerName=@EngineerName WHERE Id='" + hidden.Value + "'", con);

                Cmd.Parameters.AddWithValue("@EngineerName", txtengineername.Text);
                Cmd.Parameters.AddWithValue("@UpdatedBy", txtengineername.Text);  // Session["name"].ToString());
                //Cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);

                con.Open();
                Cmd.ExecuteNonQuery();
                con.Close();

                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Updated Successfully');", true);


            }
            else
            {
                if (Dt.Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Record Alredy Save !!');", true);
                }
                else
                {
                    SqlCommand Cmd = new SqlCommand("INSERT INTO tbl_Engineer (EngineerName) VALUES (@EngineerName)", con);

                    Cmd.Parameters.AddWithValue("@EngineerName", txtengineername.Text);
                    Cmd.Parameters.AddWithValue("@CreatedBy", txtengineername.Text);  //Session["name"].ToString());
                  //  Cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                  //  Cmd.Parameters.AddWithValue("@isdeleted", '0');

                    con.Open();
                    Cmd.ExecuteNonQuery();
                    con.Close();

                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Saved Successfully');", true);


                }

            }


        }
        catch (Exception)
        {

            throw;
        }

    }

    protected void loadData(string id)
    {

        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM [tbl_Engineer] where  [Id]='" + id + "' ", con);
        sad.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            txtengineername.Text = dt.Rows[0]["EngineerName"].ToString();
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
    protected void gv_Role_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblisstatu = (Label)e.Row.FindControl("lblIsActive") as Label;
            if (lblisstatu.Text == "True")

            {
                lblisstatu.Text = "Active";
            }

            else

            {

                lblisstatu.Text = "DeActive";

            }
        }
    }


    protected void lnkrefresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("EngineerMaster.aspx");
    }
    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetRoleList(string prefixText, int count)
    {
        return AutoFillRolelist(prefixText);
    }

    public static List<string> AutoFillRolelist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT EngineerName from [tbl_Engineer] where " + "EngineerName like @Search + '%'";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> EngineerName = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        EngineerName.Add(sdr["EngineerName"].ToString());
                    }
                }
                con.Close();
                return EngineerName;
            }
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("RoleList.aspx");
    }
}