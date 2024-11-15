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

public partial class Reception_CustomerList : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GridView();
        }
    }

    void GridView()
    {
        try
        {
            string role = Session["adminname"].ToString();

            if (role == "SubCustomer")
            {
                DataTable dt = new DataTable();
                con.Open();
                SqlDataAdapter sad = new SqlDataAdapter("SELECT [Custid],[CustomerName],[GSTNo],[StateCode],[PanNo],[AddresLine1],[AddresLine2],[AddresLine3],[Area],[Email],[City],[Country],[MobNo],[PostalCode],[ContactPerName1],[ContactPerNo1],[ContactPerName2],[ContactPerNo2],[IsStatus],[CreatedBy],[Createddate],[UpdatedBy],[UpdatedDate] FROM [tblCustomer] where CustomerName='Schneider Electric India Pvt.Ltd.' AND isdeleted='0' ORDER BY Createddate Desc", con);
                sad.Fill(dt);
                gv_Customer.EmptyDataText = "Not Records Found";
                gv_Customer.DataSource = dt;
                gv_Customer.DataBind();
                con.Close();
            }
            else
            {
                DataTable dt = new DataTable();
                con.Open();
                SqlDataAdapter sad = new SqlDataAdapter("SELECT [Custid],[CustomerName],[GSTNo],[StateCode],[PanNo],[AddresLine1],[AddresLine2],[AddresLine3],[Area],[Email],[City],[Country],[MobNo],[PostalCode],[ContactPerName1],[ContactPerNo1],[ContactPerName2],[ContactPerNo2],[IsStatus],[CreatedBy],[Createddate],[UpdatedBy],[UpdatedDate] FROM [tblCustomer] where  isdeleted='0' ORDER BY Createddate Desc", con);
                sad.Fill(dt);
                gv_Customer.EmptyDataText = "Not Records Found";
                gv_Customer.DataSource = dt;
                gv_Customer.DataBind();
                con.Close();
            }
         
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void gv_Customer_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "RowEdit")
        {
            Response.Redirect("Customer.aspx?Custid=" + encrypt(e.CommandArgument.ToString()) + "");
        }

        if (e.CommandName == "RowDelete")
        {
            SqlCommand cmddelete = new SqlCommand("update tblCustomer set isdeleted='1' where Custid=@Custid", con);
            cmddelete.Parameters.AddWithValue("@Custid", Convert.ToInt32(e.CommandArgument.ToString()));
            cmddelete.Parameters.AddWithValue("@isdeleted", '1');
            con.Open();
            cmddelete.ExecuteNonQuery();
            con.Close();
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Delete Sucessfully');", true);
            GridView();
        }
    }
    protected void gv_Customer_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_Customer.PageIndex = e.NewPageIndex;
        GridView();
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

    protected void btncreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("Customer.aspx");

    }

    protected void lnkBtnsearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(txtSearch.Text))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Search Customer Name');", true);
            }
            if (string.IsNullOrEmpty(txtSearch.Text))
            {
                GridView();
            }
            else
            {

                DataTable dt = new DataTable();

                SqlDataAdapter sad = new SqlDataAdapter("SELECT [Custid],[CustomerName],[GSTNo],[StateCode],[PanNo],[AddresLine1],[AddresLine2],[AddresLine3],[Area],[Email],[City],[Country],[MobNo],[PostalCode],[ContactPerName1],[ContactPerNo1],[ContactPerName2],[ContactPerNo2],[IsStatus],[CreatedBy],[Createddate],[UpdatedBy],[UpdatedDate] FROM [tblCustomer] where [CustomerName]='" + txtSearch.Text + "' AND isdeleted='0' ", con);
                sad.Fill(dt);
                gv_Customer.EmptyDataText = "Not Records Found";
                gv_Customer.DataSource = dt;
                gv_Customer.DataBind();
            }


            //GridView();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetCustomerList(string prefixText, int count)
    {
        return AutoFillCustomerlist(prefixText);
    }

    public static List<string> AutoFillCustomerlist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT CustomerName from tblCustomer where " + "CustomerName like @Search + '%' AND isdeleted='0'";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> CustomerName = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        CustomerName.Add(sdr["CustomerName"].ToString());
                    }
                }
                con.Close();
                return CustomerName;
            }

        }
    }

    protected void gv_Customer_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            LinkButton lnkbtnEdit = e.Row.FindControl("lnkbtnEdit") as LinkButton;
            LinkButton lnkbtnDelete = e.Row.FindControl("lnkbtnDelete") as LinkButton;


            string Id = Session["Id"].ToString();


            DataTable Dtt = new DataTable();
            SqlDataAdapter Sdd = new SqlDataAdapter("Select * FROM tblUserRoleAuthorization where UserID = '" + Id + "' AND PageName = 'CustomerList.aspx' AND PagesView = '1'", con);
            Sdd.Fill(Dtt);
            if (Dtt.Rows.Count > 0)
            {
                //btnAddDelChallan.Visible = false;
                gv_Customer.Columns[8].Visible = false;
                lnkbtnEdit.Visible = false;
                lnkbtnDelete.Visible = false;
                btncreate.Visible = true;
            }
        }



        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    Label lblisstatus = (Label)e.Row.FindControl("lblisstatus") as Label;

        //    if (lblisstatus.Text == "True")

        //    {
        //        lblisstatus.Text = "Active";
        //    }

        //    else

        //    {

        //        lblisstatus.Text = "DeActive";

        //    }

        //}
        //if (Session["name"].ToString() == "Admin")
        //{
        //    gv_Customer.Columns[6].Visible = true;
        //    gv_Customer.Columns[7].Visible = true;
        //    // cust.Visible = true;
        //    // cust1.Visible = true;
        //}
        //else
        //{
        //    // cust.Visible = false;
        //    //cust1.Visible = false;
        //    gv_Customer.Columns[6].Visible = false;
        //    gv_Customer.Columns[7].Visible = true;
        //}
    }
    SqlDataAdapter sad;
    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();

            if (ddlStatus.Text == "All")
            {
                sad = new SqlDataAdapter("SELECT [Custid],[CustomerName],[GSTNo],[StateCode],[PanNo],[AddresLine1],[AddresLine2],[AddresLine3],[Area],[Email],[City],[Country],[MobNo],[PostalCode],[ContactPerName1],[ContactPerNo1],[ContactPerName2],[ContactPerNo2],[IsStatus],[CreatedBy],[Createddate],[UpdatedBy],[UpdatedDate] FROM [tblCustomer] where isdeleted='0' ", con);
            }
            else
            {
                sad = new SqlDataAdapter("SELECT [Custid],[CustomerName],[GSTNo],[StateCode],[PanNo],[AddresLine1],[AddresLine2],[AddresLine3],[Area],[Email],[City],[Country],[MobNo],[PostalCode],[ContactPerName1],[ContactPerNo1],[ContactPerName2],[ContactPerNo2],[IsStatus],[CreatedBy],[Createddate],[UpdatedBy],[UpdatedDate] FROM [tblCustomer] where [IsStatus]='" + ddlStatus.Text + "' AND isdeleted='0' ", con);
            }
            sad.Fill(dt);
            gv_Customer.EmptyDataText = "Not Records Found";
            gv_Customer.DataSource = dt;
            gv_Customer.DataBind();
        }
        catch (Exception)
        {

            throw;
        }
    }

    protected void lnkrefresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("CustomerList.aspx");
    }
}