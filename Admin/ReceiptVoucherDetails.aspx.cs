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

public partial class Admin_ReceiptVoucherDetails : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Load_Record();
        }
    }

    protected void btncreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("ReceiptVoucher.aspx");
    }

    protected void gv_Customer_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (Session["Role"].ToString() == "Admin")
        //{
        //    gv_Customer.Columns[6].Visible = true;
        //    gv_Customer.Columns[7].Visible = true;
        //}
        //else
        //{
        //    gv_Customer.Columns[6].Visible = false;
        //    gv_Customer.Columns[7].Visible = true;
        //}
    }

    private void Load_Record()
    {
        DataTable Dt = new DataTable();
        SqlDataAdapter Da = new SqlDataAdapter("SELECT * FROM TblReceiptVoucherHdr", con);
        Da.Fill(Dt);

        gv_Customer.DataSource = Dt;
        gv_Customer.EmptyDataText = "Record Not Found";
        gv_Customer.DataBind();
    }

    protected void gv_Customer_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName== "RowPrint")
        {
            Response.Write("<script>window.open ('../reportPdf/ReceiptVoucherPdf.aspx?Id=" + encrypt(e.CommandArgument.ToString()) + "','_blank');</script>");

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

}