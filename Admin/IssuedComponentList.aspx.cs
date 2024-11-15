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

public partial class Admin_IssuedComponentList : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["adminname"] == null)
            {
                Response.Redirect("../LoginPage.aspx");
            }
            else
            {
                ViewData();
                //gvbind_Company();
            }
        }
    }

    protected void ViewData()
    {
        try
        {
            string role = Session["adminname"].ToString();
            if (role == "SubCustomer")
            {
                // DataTable Dt = new DataTable();
                //// SqlDataAdapter Da = new SqlDataAdapter("SELECT ID,Quotation_no,Quotation_Date,ExpiryDate,CreatedOn,JobNo,Customer_Name,SubCustomer,Address,Mobile_No,Phone_No,GST_No,State_Code,kind_Att,CGST,SGST,AllTotal_price,Total_in_word,IsDeleted,CreatedBy,CreatedOn,DATEDIFF(DAY, Quotation_Date, getdate()) AS days FROM tbl_Quotation_Hdr WHERE Customer_Name='Schneider Electric India Pvt.Ltd.' AND IsDeleted='0'  ORDER BY Quotation_Date DESC ", con);

                // Da.Fill(Dt);
                // GV_IssuedComponent.DataSource = Dt;
                // GV_IssuedComponent.DataBind();
            }
			 if (role == "Technical")
            {
                DataTable Dt = new DataTable();
                SqlDataAdapter Da = new SqlDataAdapter("SELECT * FROM [Tbl_IssuedComponetHdr] ", con);

                Da.Fill(Dt);
                GV_IssuedComponent.DataSource = Dt;
                GV_IssuedComponent.DataBind();
                GV_IssuedComponent.Columns[4].Visible = false;

            }
            else
            {
                DataTable Dt = new DataTable();
                SqlDataAdapter Da = new SqlDataAdapter("SELECT * FROM [Tbl_IssuedComponetHdr] where isdeleted IS NULL ", con);

                Da.Fill(Dt);
                GV_IssuedComponent.DataSource = Dt;
                GV_IssuedComponent.DataBind();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void GV_IssuedComponent_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            //SqlDataAdapter Da = new SqlDataAdapter("SELECT Quotation_no From tbl_Quotation_Hdr WHERE Quotation_no='" + lbl_Quo_no + "'", con);
            //DataTable Dt = new DataTable();
            //Da.Fill(Dt);

            string Id = GV_IssuedComponent.DataKeys[e.Row.RowIndex].Value.ToString();
            GridView gvDetails = e.Row.FindControl("gvDetails") as GridView;

            SqlDataAdapter Daaa = new SqlDataAdapter("SELECT * FROM [Tbl_IssuedComponetDtls] WHERE [JobNo]='" + Id + "'", con);
            DataTable Dttt = new DataTable();
            Daaa.Fill(Dttt);
            gvDetails.DataSource = Dttt;
            gvDetails.DataBind();
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("NEWISSUCOMP.aspx");
    }

    protected void GV_IssuedComponent_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GV_IssuedComponent.PageIndex = e.NewPageIndex;
        ViewData();
    }

    protected void GV_IssuedComponent_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "RowEdit")
        {
            Response.Redirect("NEWISSUCOMP.aspx?JobNo=" + encrypt(e.CommandArgument.ToString()) + "");
        }
        if (e.CommandName == "RowDelete")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            string jobNo = (string)this.GV_IssuedComponent.DataKeys[index]["JobNo"];
            string CustomerName = (string)this.GV_IssuedComponent.DataKeys[index]["CustomerName"];
            Deleteissudecomponent(jobNo, CustomerName);
            ViewData();
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

    public void Deleteissudecomponent(string jobNo, string CustomerName)
    {
        con.Open();
        SqlCommand command = new SqlCommand();
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataSet ds = new DataSet();
        command.Connection = con;
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = "SP_IssuedComponetDtls";
        command.Parameters.AddWithValue("@Action", "DeleteIssuedcomponent");
        command.Parameters.AddWithValue("@JobNo", jobNo);
        command.Parameters.AddWithValue("@CompName", CustomerName);
        command.ExecuteNonQuery();
        con.Close();
    }

    protected void lnkBtnsearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(txtDateSearchfrom.Text) && string.IsNullOrEmpty(txtDateSearchto.Text))
            {
                ViewData();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Search Record');", true);
            }

            else if (!string.IsNullOrEmpty(txtDateSearchfrom.Text) && !string.IsNullOrEmpty(txtDateSearchto.Text))
            {
                DataTable dt = new DataTable();

                SqlDataAdapter sad = new SqlDataAdapter("select [Id],[JobNo],[CustomerName],[ProductName],[EngineerName],[CreatedBy],[Createddate],[UpdatedBy],[UpdatedDate] ,isdeleted,IssuedDate FROM [Tbl_IssuedComponetHdr] Where [IssuedDate] between'" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "' ", con);

               // SqlDataAdapter sad = new SqlDataAdapter("select * from [Tbl_IssuedComponetHdr] where IssuedDate'" + txtDateSearchfrom.Text + "' AND  '" + txtDateSearchto.Text + "' ", con);
                sad.Fill(dt);

                GV_IssuedComponent.EmptyDataText = "Not Records Found";
                GV_IssuedComponent.DataSource = dt;
                GV_IssuedComponent.DataBind();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void lnkBtn_rfresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("IssuedComponentList.aspx");
    }
}