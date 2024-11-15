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

public partial class Admin_Expenses : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);

    string id;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GridView();

            if (Request.QueryString["Id"] != null)
            {
                id = Decrypt(Request.QueryString["Id"].ToString());
                LoadDataOfficeExpenses(id);

                btnadd.Text = "Update";
                hidden.Value = id;


            }
        }
    }

    void GridView()
    {
        try
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM Tbl_OfficeExpensess  WHERE Isdeleted ='0'", con);
            sad.Fill(dt);
            gv_officeexpenses.EmptyDataText = "Not Records Found";
            gv_officeexpenses.DataSource = dt;
            gv_officeexpenses.DataBind();
            con.Close();
        }
        catch (Exception)
        {

            throw;
        }
    }

    protected void LoadDataOfficeExpenses (string id)
    {
        try
        {
            SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM Tbl_OfficeExpensess WHERE Id='" + id + "'", con);

            DataTable dt = new DataTable();
            sad.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                textexpensestype.Text = dt.Rows[0]["ExpensiveType"].ToString();
                textbalancesheet.Text = dt.Rows[0]["BalanceSheet"].ToString();
                textnarration.Text = dt.Rows[0]["Narration"].ToString();
                textdate.Text = dt.Rows[0]["Date"].ToString();
                textamount.Text = dt.Rows[0]["Amount"].ToString();

            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    protected void btnadd_Click(object sender, EventArgs e)
    {
        try
        {
            string createdby = Session["adminname"].ToString();


            if (btnadd.Text == "Save")

            {

                SqlCommand cmd = new SqlCommand("SP_OfficeExpensess", con);
                DateTime Date = DateTime.Now;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "Save");
                cmd.Parameters.AddWithValue("@ExpensiveType", textexpensestype.Text);
                cmd.Parameters.AddWithValue("@BalanceSheet", textbalancesheet.Text);
                cmd.Parameters.AddWithValue("@Narration", textnarration.Text);
                cmd.Parameters.AddWithValue("@Date", textdate.Text);
                cmd.Parameters.AddWithValue("@Amount", textamount.Text);
                cmd.Parameters.AddWithValue("@CreatedBy", createdby);
                cmd.Parameters.AddWithValue("@CreatedDate", Date);
                cmd.Parameters.AddWithValue("@Isdeleted", '0');
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Saved Sucessfully','0');", true);
            }
            else
            {
                SqlCommand cmd = new SqlCommand("SP_OfficeExpensess", con);
                DateTime Date = DateTime.Now;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", hidden.Value);
                cmd.Parameters.AddWithValue("@Action", "Update");
                cmd.Parameters.AddWithValue("@ExpensiveType", textexpensestype.Text);
                cmd.Parameters.AddWithValue("@BalanceSheet", textbalancesheet.Text);
                cmd.Parameters.AddWithValue("@Narration", textnarration.Text);
                cmd.Parameters.AddWithValue("@Date", textdate.Text);
                cmd.Parameters.AddWithValue("@Amount", textamount.Text);
                cmd.Parameters.AddWithValue("@UpdatedBy", createdby);
                cmd.Parameters.AddWithValue("@UpdatedDate", Date);
                cmd.Parameters.AddWithValue("@Isdeleted", '0');
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Upadted Sucessfully','0');", true);

            }

        }
        catch (Exception)
        {

            throw;
        }
    }

    protected void btncancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("Expenses.aspx");
    }

    protected void gv_officeexpenses_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_officeexpenses.PageIndex = e.NewPageIndex;
        GridView();
    }

    protected void gv_officeexpenses_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "RowEdit")
        {
            Response.Redirect("Expenses.aspx?id=" + encrypt(e.CommandArgument.ToString()) + "");
        }
        if (e.CommandName == "RowDelete")
        {
            SqlCommand cmddelete = new SqlCommand("UPDATE Tbl_OfficeExpensess SET Isdeleted='1' WHERE Id=@Id", con);
            cmddelete.Parameters.AddWithValue("@Id", Convert.ToInt32(e.CommandArgument.ToString()));
            cmddelete.Parameters.AddWithValue("@Isdeleted", '1');
            con.Open();
            cmddelete.ExecuteNonQuery();
            con.Close();
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Delete Sucessfully');", true);
            
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
}