﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Activities.Expressions;

public partial class Admin_EnquirPage_EnquiryPage : System.Web.UI.Page
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
                SqlDataAdapter sad = new SqlDataAdapter(
     "SELECT [EnquiryId], [CustomerName], [StateCode], [AddresLine1], [Area], [City], [Country], [PostalCode], [MobNo],[Email],[IsStatus] " +
     "FROM [tbl_EnquiryMaster] where  isdeleted='0' ORDER BY Createddate Desc ", con);
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
                SqlDataAdapter sad = new SqlDataAdapter(
     "SELECT [EnquiryId], [CustomerName], [StateCode], [AddresLine1], [Area], [City], [Country], [PostalCode], [MobNo], [Email], [IsStatus] " +
     "FROM [tbl_EnquiryMaster] where  isdeleted='0' ORDER BY Createddate Desc ", con);
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
            Response.Redirect("Enquiry.aspx?EnquiryId=" + encrypt(e.CommandArgument.ToString()) + "");
        }

        if (e.CommandName == "RowDelete")
        {
            SqlCommand cmddelete = new SqlCommand("update tbl_EnquiryMaster set isdeleted='1' where EnquiryId=@EnquiryId", con);
            cmddelete.Parameters.AddWithValue("@EnquiryId", Convert.ToInt32(e.CommandArgument.ToString()));
            cmddelete.Parameters.AddWithValue("@isdeleted", '1');
            con.Open();
            cmddelete.ExecuteNonQuery();
            con.Close();
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Delete Sucessfully');", true);
            GridView();
        }
        if(e.CommandName == "CreateJobCard")
        {
            int index = Convert.ToInt32(e.CommandArgument);   
            ViewState["EnquiryId"] = e.CommandArgument.ToString();
           Response.Redirect("InwardEntry.aspx?CODE=" + encrypt(e.CommandArgument.ToString()));

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
        Response.Redirect("Enquiry.aspx");

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
                SqlDataAdapter sad = new SqlDataAdapter(
    "SELECT [EnquiryId], [CustomerName], [StateCode], [AddresLine1], [Area], [City], [Country], [PostalCode], [MobNo],[Email],[IsStatus] " +
                "FROM [tbl_EnquiryMaster] " +
    "WHERE [CustomerName]='" + txtSearch.Text + "'AND isdeleted = '0'", con); 
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
                com.CommandText = "select DISTINCT CustomerName from tbl_EnquiryMaster where " + "CustomerName like @Search + '%'AND isdeleted='0'";

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




    }
    SqlDataAdapter sad;
    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();

            if (ddlStatus.Text == "All")
            {
                sad = new SqlDataAdapter(
    "SELECT [EnquiryId], [CustomerName], [StateCode], [AddresLine1], [Area], [City], [Country], [PostalCode], [MobNo], [IsStatus], " +
    "[Email], [CreatedBy], [Createddate], [UpdatedBy], [UpdatedDate] " +
    "FROM [tbl_EnquiryMaster] " +
    "WHERE [isdeleted] = 0",
    con);
            }
            else
            {
                sad = new SqlDataAdapter(
    "SELECT [EnquiryId], [CustomerName], [StateCode], [AddresLine1], [Area], [City], [Country], [PostalCode], [MobNo], [IsStatus], " +
    "[Email], [CreatedBy], [Createddate], [UpdatedBy], [UpdatedDate] " +
    "FROM [tbl_EnquiryMaster] " +
    " where [IsStatus]='" + ddlStatus.Text + "' AND isdeleted='0'", 
    con);
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
        Response.Redirect("EnquiryList.aspx");
    }
}