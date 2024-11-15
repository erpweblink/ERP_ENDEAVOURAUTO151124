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

public partial class Reception_inwardEntryList : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
    DataTable dt11 = new DataTable();
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

                GridView();
                // GridExportExcel();
                //gv_Inward.DataSource = dt11;
                //gv_Inward.DataBind();
                //this.gv_Inward.Columns[10].Visible = true;

                //string UserCompany = Session["name"].ToString();
                //if (UserCompany != "Admin")
                //{
                //    btncreate.Visible = false;
                //    gvbind_Company();
                //    this.gv_Inward.Columns[12].Visible = false;
                //}
                //else
                //{
                //    GridView();
                //    gv_Inward.DataSource = dt11;
                //    gv_Inward.DataBind();
                //    this.gv_Inward.Columns[10].Visible = true;

                //}
                //this.txtDateSearch.Text = DateTime.Now.ToString("yyyy-MM-dd");
                //this.txtDateSearch.TextMode = TextBoxMode.Date;
            }
        }
    }

    void gvbind_Company()
    {
        try
        {
            string UserCompany = Session["UserCompany"].ToString();
            DataTable dt = new DataTable();
            SqlDataAdapter sad = new SqlDataAdapter("select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[MateName],FinalStatus,[SrNo],[MateStatus],[TestBy],[ModelNo],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry] where isdeleted='0' AND CustName='" + UserCompany + "' ORDER BY [JobNo] Desc ", con);
            sad.Fill(dt);
            gv_Inward.EmptyDataText = "Not Records Found";
            gv_Inward.DataSource = dt;
            gv_Inward.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
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
                SqlDataAdapter sad = new SqlDataAdapter("select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],otherinfo,[MateName],FinalStatus,[SrNo],[MateStatus],[TestBy],[ModelNo],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,Imagepath,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry]  where CustName='Schneider Electric India Pvt.Ltd.' AND isdeleted='0' ORDER BY [JobNo] Desc ", con);
                sad.Fill(dt);

                gv_Inward.EmptyDataText = "Not Records Found";
                gv_Inward.DataSource = dt;
                gv_Inward.DataBind();
                btncreate.Visible = false;
            }
            else
            {
                DataTable dt = new DataTable();
                SqlDataAdapter sad = new SqlDataAdapter("SELECT Id,JobNo,DateIn,CustName,Subcustomer,Branch,otherinfo,MateName,FinalStatus,SrNo,MateStatus,TestBy,ModelNo,CreatedBy,CreatedDate,UpdateBy,UpdateDate,ProductFault,RepeatedNo,Imagepath,DATEDIFF(DAY, DateIn,getdate()) AS days FROM tblInwardEntry WHERE isdeleted = '0' ORDER BY JobNo Desc", con);
                // SqlDataAdapter sad = new SqlDataAdapter("select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],otherinfo,[MateName],FinalStatus,[SrNo],[MateStatus],[TestBy],[ModelNo],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, CreatedDate, getdate()) AS days FROM [tblInwardEntry] where isdeleted='0' ORDER BY [CreatedDate] Desc ", con);
                sad.Fill(dt);

                gv_Inward.EmptyDataText = "Not Records Found";
                gv_Inward.DataSource = dt;
                gv_Inward.DataBind();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    void GridExportExcel()
    {
        try
        {
            // GridEporttoexcel.Visible = false;
            string role = Session["adminname"].ToString();
            if (role == "SubCustomer")
            {
                DataTable dt = new DataTable();
                SqlDataAdapter sad = new SqlDataAdapter("select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],otherinfo,[MateName],FinalStatus,[SrNo],[MateStatus],[TestBy],[ModelNo],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,Imagepath,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry]  where CustName='Schneider Electric India Pvt.Ltd.' AND isdeleted='0' ORDER BY [JobNo] Desc ", con);
                sad.Fill(dt);

                GridEporttoexcel.EmptyDataText = "Not Records Found";
                GridEporttoexcel.DataSource = dt;
                GridEporttoexcel.DataBind();
                GridEporttoexcel.Visible = false;
                //GridEporttoexcel.Visible = false;
            }
            else
            {
                DataTable dt = new DataTable();
                SqlDataAdapter sad = new SqlDataAdapter("SELECT Id,JobNo,DateIn,CustName,Subcustomer,Branch,otherinfo,MateName,FinalStatus,SrNo,MateStatus,TestBy,ModelNo,CreatedBy,CreatedDate,UpdateBy,UpdateDate,ProductFault,RepeatedNo,Imagepath,DATEDIFF(DAY, DateIn,getdate()) AS days FROM tblInwardEntry WHERE isdeleted = '0' ORDER BY JobNo Desc", con);
                // SqlDataAdapter sad = new SqlDataAdapter("select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],otherinfo,[MateName],FinalStatus,[SrNo],[MateStatus],[TestBy],[ModelNo],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, CreatedDate, getdate()) AS days FROM [tblInwardEntry] where isdeleted='0' ORDER BY [CreatedDate] Desc ", con);
                sad.Fill(dt);

                GridEporttoexcel.EmptyDataText = "Not Records Found";
                GridEporttoexcel.DataSource = dt;
                GridEporttoexcel.DataBind();
                // GridEporttoexcel.Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetJOBNOList(string prefixText, int count)
    {
        return AutoFillJOBNOlist(prefixText);
    }

    public static List<string> AutoFillJOBNOlist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT JobNo from tblInwardEntry where " + "JobNo like @Search + '%' ";

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


    protected void btncreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("InwardEntry.aspx");
    }

    protected void gv_Inward_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "RowEdit")
        {
            Response.Redirect("InwardEntry.aspx?Id=" + encrypt(e.CommandArgument.ToString()) + "");
        }

        if (e.CommandName == "RowDelete")
        {

            con.Open();
            SqlCommand cmd1 = new SqlCommand("SELECT isCompleted from [vw_InwardTesting] where JobNo='" + e.CommandArgument.ToString() + "'", con);
            Object iscomplted = cmd1.ExecuteScalar();

            if (iscomplted.ToString() == "True")
            {
                con.Close();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Testing Already Completed. You Not Able To Delete This Record.');", true);
            }
            else
            {
                con.Close();
                SqlCommand cmddelete = new SqlCommand("update tblInwardEntry set isdeleted='1' where JobNo=@JobNo", con);
                cmddelete.Parameters.AddWithValue("@JobNo", e.CommandArgument.ToString());
                cmddelete.Parameters.AddWithValue("@isdeleted", '1');
                con.Open();
                cmddelete.ExecuteNonQuery();
                con.Close();
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Delete Sucessfully');", true);

                //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Delete sucessfully!!');window.location ='CustomerList.aspx';", true);

                GridView();

            }

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
    protected void lnkBtnsearch_Click(object sender, EventArgs e)
    {
        try
        {

            if (string.IsNullOrEmpty(txtSearch.Text) && string.IsNullOrEmpty(txtDateSearch.Text) && string.IsNullOrEmpty(txtJobNo.Text) && string.IsNullOrEmpty(txtJobNo.Text) && string.IsNullOrEmpty(txtreatedNo.Text) && string.IsNullOrEmpty(txtDateSearchfrom.Text) && string.IsNullOrEmpty(txtDateSearchto.Text) && string.IsNullOrEmpty(txtDateSearch.Text) && string.IsNullOrEmpty(txtSearchProduct.Text) && string.IsNullOrEmpty(txtSearchStatus.Text))
            {
                GridView();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Search Record');", true);
            }

            else
            {
                if (!string.IsNullOrEmpty(txtJobNo.Text) && string.IsNullOrEmpty(txtDateSearchfrom.Text))
                {
                    ViewState["Excell"] = "GetsortedJobNo";
                    GetsortedJobNo();
                    //string jobno = txtJobNo.Text;

                    //DataTable dt = new DataTable();
                    //sad = new SqlDataAdapter("select Id,JobNo,DateIn,CustName,MateName,SrNo,Subcustomer,MateStatus,[otherinfo],[Imagepath],FinalStatus,TestBy,CreatedBy,CreatedDate,RepeatedNo,Branch,ModelNo,ProductFault, DATEDIFF(DAY, DateIn, getdate()) AS days from tblInwardEntry where JobNo= '" + jobno + "'", con);
                    //sad.Fill(dt);
                    //gv_Inward.EmptyDataText = "Not Records Found";
                    //gv_Inward.DataSource = dt;
                    //gv_Inward.DataBind();
                }

                if (!string.IsNullOrEmpty(txtSearch.Text) && string.IsNullOrEmpty(txtDateSearch.Text) && string.IsNullOrEmpty(txtreatedNo.Text) && string.IsNullOrEmpty(txtDateSearchfrom.Text))
                {
                    ViewState["Excell"] = "GetsortedCustomer";
                    GetsortedCustomer();
                    //DataTable dt = new DataTable();

                    //SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],[FinalStatus],[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry] Where CustName='" + txtSearch.Text + "' AND isdeleted='0'", con);
                    //sad.Fill(dt);
                    //gv_Inward.EmptyDataText = "Not Records Found";
                    //gv_Inward.DataSource = dt;
                    //gv_Inward.DataBind();
                }

                if (!string.IsNullOrEmpty(txtSearchProduct.Text) && string.IsNullOrEmpty(txtDateSearchfrom.Text))
                {

                    ViewState["Excell"] = "Getsortedproduct";
                    Getsortedproduct();

                    //DataTable dt = new DataTable();

                    //SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],[FinalStatus],[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry] Where MateName='" + txtSearchProduct.Text + "' AND isdeleted='0'", con);
                    //sad.Fill(dt);
                    //gv_Inward.EmptyDataText = "Not Records Found";
                    //gv_Inward.DataSource = dt;
                    //gv_Inward.DataBind();
                }

                if (!string.IsNullOrEmpty(txtSearchStatus.Text) && string.IsNullOrEmpty(txtDateSearchfrom.Text))
                {
                    ViewState["Excell"] = "Sortedstatus";
                    Sortedstatus();
                    //DataTable dt = new DataTable();
                    //SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],[FinalStatus],[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry] Where MateStatus='" + txtSearchStatus.Text + "' AND isdeleted='0'", con);

                    ////SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM tblInwardEntry WHERE MateStatus ='" + txtSearchStatus.Text + "' AND isdeleted='0'", con);
                    //sad.Fill(dt);
                    //gv_Inward.EmptyDataText = "Not Records Found";
                    //gv_Inward.DataSource = dt;
                    //gv_Inward.DataBind();
                }


                else if (string.IsNullOrEmpty(txtSearch.Text) && !string.IsNullOrEmpty(txtDateSearch.Text) && string.IsNullOrEmpty(txtreatedNo.Text) && string.IsNullOrEmpty(txtDateSearchfrom.Text))
                {
                    ViewState["Excell"] = "Getsorteddatwise";
                    Getsorteddatwise();
                    //DateTime date = Convert.ToDateTime(txtDateSearch.Text.ToString());
                    //DataTable dt = new DataTable();

                    //txtDateSearch.Text = date.ToString("yyyy-MM-dd");
                    //SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],FinalStatus,[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry] Where DateIn='" + txtDateSearch.Text + "' AND isdeleted='0' ", con);
                    //sad.Fill(dt);
                    //gv_Inward.EmptyDataText = "Not Records Found";
                    //gv_Inward.DataSource = dt;
                    //gv_Inward.DataBind();
                }


                else if (string.IsNullOrEmpty(txtSearch.Text) && string.IsNullOrEmpty(txtDateSearch.Text) && !string.IsNullOrEmpty(txtreatedNo.Text) && string.IsNullOrEmpty(txtDateSearchfrom.Text))
                {
                    ViewState["Excell"] = "Getsortedreatedno";
                    Getsortedreatedno();
                    ////DateTime date = Convert.ToDateTime(txtDateSearch.Text.ToString());
                    //DataTable dt = new DataTable();

                    ////txtDateSearch.Text = date.ToString("yyyy-MM-dd");
                    //SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],FinalStatus,[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry] Where RepeatedNo='" + txtreatedNo.Text + "' AND isdeleted='0' ", con);
                    //sad.Fill(dt);
                    //gv_Inward.EmptyDataText = "Not Records Found";
                    //gv_Inward.DataSource = dt;
                    //gv_Inward.DataBind();
                }
                else if ((!string.IsNullOrEmpty(txtSearch.Text) && !string.IsNullOrEmpty(txtDateSearch.Text)))
                {
                    ViewState["Excell"] = "GetsorteddatwiseCustomer";
                    GetsorteddatwiseCustomer();
                    //DateTime date = Convert.ToDateTime(txtDateSearch.Text.ToString());
                    //DataTable dt = new DataTable();

                    //txtDateSearch.Text = date.ToString("yyyy-MM-dd");
                    //SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],FinalStatus,[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry] Where DateIn='" + txtDateSearch.Text + "' AND CustName='" + txtSearch.Text + "' AND isdeleted='0' ", con);
                    //sad.Fill(dt);
                    //gv_Inward.EmptyDataText = "Not Records Found";
                    //gv_Inward.DataSource = dt;
                    //gv_Inward.DataBind();
                }
                else if ((!string.IsNullOrEmpty(txtSearch.Text) && !string.IsNullOrEmpty(txtreatedNo.Text)))
                {
                    ViewState["Excell"] = "Getsortedcustomerwiserepeatedno";
                    Getsortedcustomerwiserepeatedno();
                    //DateTime date = Convert.ToDateTime(txtDateSearch.Text.ToString());
                    //DataTable dt = new DataTable();

                    ////txtDateSearch.Text = date.ToString("yyyy-MM-dd");
                    //SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],FinalStatus,[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry] Where RepeatedNo='" + txtreatedNo.Text + "' AND CustName='" + txtSearch.Text + "' AND isdeleted='0' ", con);
                    //sad.Fill(dt);
                    //gv_Inward.EmptyDataText = "Not Records Found";
                    //gv_Inward.DataSource = dt;
                    //gv_Inward.DataBind();
                }

                else if (!string.IsNullOrEmpty(txtDateSearchfrom.Text) && !string.IsNullOrEmpty(txtDateSearchto.Text) && string.IsNullOrEmpty(txtSearch.Text) && string.IsNullOrEmpty(txtSearchProduct.Text) && string.IsNullOrEmpty(txtSearchStatus.Text) && string.IsNullOrEmpty(txtreatedNo.Text))
                {
                    ViewState["Excell"] = "GetsortedDatafromdateToDate";
                    GetsortedDatafromdateToDate();
                    //DataTable dt = new DataTable();

                    //SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],FinalStatus,[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry] Where DateIn between'" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "' ", con);

                    ////SqlDataAdapter sad = new SqlDataAdapter("select * from tblInwardEntry where DateIn between '" + txtDateSearchfrom.Text + "' AND  '" + txtDateSearchto.Text + "' ", con);
                    //sad.Fill(dt);

                    //gv_Inward.EmptyDataText = "Not Records Found";
                    //gv_Inward.DataSource = dt;
                    //gv_Inward.DataBind();
                }

                if (!string.IsNullOrEmpty(txtJobNo.Text) && (!string.IsNullOrEmpty(txtDateSearchfrom.Text)))
                {
                    ViewState["Excell"] = "GetDatewiseJobNo";
                    GetDatewiseJobNo();

                }

                if ((!string.IsNullOrEmpty(txtDateSearchfrom.Text)) && (!string.IsNullOrEmpty(txtSearch.Text)) && (string.IsNullOrEmpty(txtJobNo.Text)) && (string.IsNullOrEmpty(txtSearchProduct.Text)))
                {
                    ViewState["Excell"] = "GetDatewiseCustomer";
                    GetDatewiseCustomer();

                }

                if ((!string.IsNullOrEmpty(txtDateSearchfrom.Text)) && (!string.IsNullOrEmpty(txtSearchProduct.Text)))
                {
                    ViewState["Excell"] = "Getdatewiseproduct";
                    Getdatewiseproduct();

                }

                if ((!string.IsNullOrEmpty(txtDateSearchfrom.Text)) && (!string.IsNullOrEmpty(txtSearchStatus.Text)))
                {
                    ViewState["Excell"] = "GetdatwiseStatus";
                    GetdatwiseStatus();

                }

                if ((!string.IsNullOrEmpty(txtDateSearchfrom.Text)) && (!string.IsNullOrEmpty(txtreatedNo.Text)))
                {
                    ViewState["Excell"] = "GetdatewiseReptedNo";
                    GetdatewiseReptedNo();

                }





                //else if (!string.IsNullOrEmpty(txtJobNo.Text))
                //{
                //    DataTable dt = new DataTable();

                //    foreach (GridViewRow g1 in gv_Inward.Rows)
                //    {
                //        Label lbldaycount = (Label)g1.FindControl("lbldaycount");
                //        Label jobnono = (Label)g1.FindControl("lblJobNo");
                //        string jobno = jobnono.Text;
                //        con.Open();
                //        SqlCommand cmdquatationpo = new SqlCommand("select  c.CreatedOn from tbl_Quotation_Hdr q inner join CustomerPO_Hdr c on q.JobNo=c.JobNo where c.JobNo='" + jobno + "'", con);
                //        SqlDataReader reader = cmdquatationpo.ExecuteReader();
                //        if (reader.Read())
                //        {
                //            DateTime ffff1 = Convert.ToDateTime(reader["CreatedOn"].ToString());
                //            string update = ffff1.ToString("yyyy-MM-dd");

                //            con.Close();
                //            sad = new SqlDataAdapter("select Id,JobNo,DateIn,CustName,MateName,SrNo,Subcustomer,MateStatus,FinalStatus,TestBy,CreatedBy,CreatedDate, DATEDIFF(DAY, CreatedDate, '" + update + "') AS days from tbl_Quotation_Hdr where JobNo='" + jobno + "' AND IsDeleted='0' AND isCompleted='1' AND  JobNo='" + txtJobNo.Text + "' ", con);
                //            sad.Fill(dt);
                //        }
                //        else
                //        {
                //            con.Close();
                //            // sad = new SqlDataAdapter("select ID,Quotation_no,Quotation_Date,ExpiryDate,JobNo,Customer_Name,SubCustomer,Address,Mobile_No,Phone_No,GST_No,State_Code,kind_Att,CGST,SGST,AllTotal_price,Total_in_word,IsDeleted,CreatedBy,CreatedOn, DATEDIFF(DAY, Quotation_Date, getdate()) AS days from tbl_Quotation_Hdr where JobNo='" + jobno + "' AND IsDeleted='0' AND isCompleted='1' AND JobNo='" + txtJobNo.Text + "'", con);
                //            sad = new SqlDataAdapter("select Id,JobNo,DateIn,CustName,MateName,SrNo,Subcustomer,MateStatus,FinalStatus,TestBy,CreatedBy,CreatedDate, DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblInwardEntry where JobNo= '" + jobno + "'", con);
                //            sad.Fill(dt);
                //            // lbldaycount.ForeColor = System.Drawing.Color.Red;
                //        }
                //    }
                //}
                //gv_Inward.EmptyDataText = "Records Not Found";
                ////gv_Inward.DataSource = dt;
                //gv_Inward.DataBind();
            }

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
                com.CommandText = "select DISTINCT CustName from tblInwardEntry where " + "CustName like @Search + '%' AND isdeleted='0'";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> CustName = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        CustName.Add(sdr["CustName"].ToString());
                    }
                }
                con.Close();
                return CustName;
            }
        }
    }

    // ----------------------
    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetProductList(string prefixText, int count)
    {
        return AutoFillProductlist(prefixText);
    }

    public static List<string> AutoFillProductlist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT MateName from tblInwardEntry where " + "MateName like @Search + '%' AND isdeleted='0'";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> MateName = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        MateName.Add(sdr["MateName"].ToString());
                    }
                }
                con.Close();
                return MateName;
            }

        }
    }

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetStatusList(string prefixText, int count)
    {
        return AutoFillStatuslist(prefixText);
    }

    public static List<string> AutoFillStatuslist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT MateStatus from tblInwardEntry where " + "MateStatus like @Search + '%' AND isdeleted='0'";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> MateStatus = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        MateStatus.Add(sdr["MateStatus"].ToString());
                    }
                }
                con.Close();
                return MateStatus;
            }

        }
    }
    //---------------------



    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetFinalStatusList(string prefixText, int count)
    {
        return AutoFillFinalStatuslist(prefixText);
    }

    public static List<string> AutoFillFinalStatuslist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT FinalStatus from tblInwardEntry where " + "FinalStatus like @Search + '%' AND isdeleted='0'";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> FinalStatus = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        FinalStatus.Add(sdr["FinalStatus"].ToString());
                    }
                }
                con.Close();
                return FinalStatus;
            }
        }
    }



    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetreapeatedList(string prefixText, int count)
    {
        return AutoFillreatedlist(prefixText);
    }
    public static List<string> AutoFillreatedlist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT RepeatedNo from tblInwardEntry where " + "RepeatedNo like @Search + '%' AND isdeleted='0'";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> RepeatedNo = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        RepeatedNo.Add(sdr["RepeatedNo"].ToString());
                    }
                }
                con.Close();
                return RepeatedNo;
            }

        }
    }
    protected void gv_Inward_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_Inward.PageIndex = e.NewPageIndex;
        GridView();
    }
    protected void lnkrefresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("inwardEntryList.aspx");
    }
    string status;
    SqlDataAdapter sad;


    protected void gv_Inward_RowDataBound1(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            con.Open();
            Label jobnono = (Label)e.Row.FindControl("lblJobno");
            string jobno = jobnono.Text;
            SqlCommand cmd1 = new SqlCommand("select O.ReturnRepair, O.CreatedDate from tblInwardEntry I inner join tblOutwardEntry O on I.JobNo=O.JobNo where I.JobNo='" + jobno + "'", con);
            SqlDataReader reader = cmd1.ExecuteReader();

            if (reader.HasRows)
            {
                if (reader.Read())
                {
                    status = reader["ReturnRepair"].ToString();
                    DateTime ffff1 = Convert.ToDateTime(reader["CreatedDate"].ToString());
                    string update = ffff1.ToString("yyyy-MM-dd");
                    if (status == "Repaired")
                    {
                        con.Close();
                        sad = new SqlDataAdapter("select JobNo,RepeatedNo,DateIn,CustName,Subcustomer,Branch,MateName,ModelNo,SrNo,ProductFault,MateStatus,FinalStatus,CreatedBy, DATEDIFF(DAY, CreatedDate, '" + update + "') AS days from tblInwardEntry where JobNo='" + jobno + "' AND isdeleted='0'", con);
                        sad.Fill(dt11);
                    }
                    else
                    {
                        con.Close();
                        sad = new SqlDataAdapter("select  JobNo,RepeatedNo,DateIn,CustName,Subcustomer,Branch,MateName,ModelNo,SrNo,ProductFault,MateStatus,FinalStatus,CreatedBy,DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblInwardEntry where JobNo='" + jobno + "' AND isdeleted='0'", con);
                        sad.Fill(dt11);
                    }
                }
            }
            else
            {
                con.Close();

                sad = new SqlDataAdapter("select JobNo,RepeatedNo,DateIn,CustName,Subcustomer,Branch,MateName,ModelNo,SrNo,ProductFault,MateStatus,FinalStatus,CreatedBy,DATEDIFF(DAY, CreatedDate, getdate()) AS days from tblInwardEntry where JobNo='" + jobno + "' AND isdeleted='0'", con);
                sad.Fill(dt11);
            }
        }

        //Authorization
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnkbtnEdit = e.Row.FindControl("lnkbtnEdit") as LinkButton;
            LinkButton lnkbtnDelete = e.Row.FindControl("lnkbtnDelete") as LinkButton;

            string Id = Session["Id"].ToString();

            DataTable Dtt = new DataTable();
            SqlDataAdapter Sdd = new SqlDataAdapter("Select * FROM tblUserRoleAuthorization where UserID = '" + Id + "' AND PageName = 'inwardEntryList.aspx' AND PagesView = '1'", con);
            Sdd.Fill(Dtt);
            if (Dtt.Rows.Count > 0)
            {
                btncreate.Visible = false;
                gv_Inward.Columns[15].Visible = false;
				 gv_Inward.Columns[4].Visible = false;
                gv_Inward.Columns[5].Visible = false;
                lnkbtnEdit.Visible = false;
                lnkbtnDelete.Visible = false;
            }
        }
    }


    //sorted section start
    protected void sortedgv_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (ViewState["Record"].ToString() == "Date")
        {
            sortedgv.PageIndex = e.NewPageIndex;
            GetsortedJobNogrid();
        }

        if (ViewState["Record"].ToString() == "Customer")
        {
            sortedgv.PageIndex = e.NewPageIndex;
            GetsortedCustomergrid();
        }
        if (ViewState["Record"].ToString() == "Product")
        {
            sortedgv.PageIndex = e.NewPageIndex;
            Getsortedproductgrid();
        }

        if (ViewState["Record"].ToString() == "status")
        {
            sortedgv.PageIndex = e.NewPageIndex;
            Sortedstatusgrid();
        }

        if (ViewState["Record"].ToString() == "Date")
        {
            sortedgv.PageIndex = e.NewPageIndex;
            Getsorteddatwisegrid();
        }

        if (ViewState["Record"].ToString() == "RatedNo")
        {
            sortedgv.PageIndex = e.NewPageIndex;
            Getsortedreatednogrid();
        }

        if (ViewState["Record"].ToString() == "Datewisecustomer")
        {
            sortedgv.PageIndex = e.NewPageIndex;
            GetsorteddatwiseCustomergrid();
        }
        if (ViewState["Record"].ToString() == "customerwiserepeatedno")
        {
            sortedgv.PageIndex = e.NewPageIndex;
            Getsortedcustomerwiserepeatednogrid();
        }

        if (ViewState["Record"].ToString() == "customerwiserepeatedno")
        {
            sortedgv.PageIndex = e.NewPageIndex;
            Getsortedcustomerwiserepeatednogrid();
        }

        if (ViewState["Record"].ToString() == "FTDate")
        {
            sortedgv.PageIndex = e.NewPageIndex;
            GetsortedDatafromdateToDategrid();
        }
        if (ViewState["Record"].ToString() == "DatewiseJob")
        {
            sortedgv.PageIndex = e.NewPageIndex;
            GetDatewiseJobNogrid();
        }
        if (ViewState["Record"].ToString() == "Datewisecustomer")
        {
            sortedgv.PageIndex = e.NewPageIndex;
            GetDatewiseCustomergrid();
        }
        if (ViewState["Record"].ToString() == "DatewiseProduct")
        {
            sortedgv.PageIndex = e.NewPageIndex;
            Getdatewiseproduct();
        }

        if (ViewState["Record"].ToString() == "Datewisestatus")
        {
            sortedgv.PageIndex = e.NewPageIndex;
            GetdatwiseStatusgrid();
        }
        if (ViewState["Record"].ToString() == "datewiseRatedNo")
        {
            sortedgv.PageIndex = e.NewPageIndex;
            GetdatewiseReptedNogrid();
        }
    }
    public void GetsortedJobNo()
    {
        gv_Inward.Visible = false;
        ViewState["Record"] = "Job";
        string jobno = txtJobNo.Text;
        DataTable dt = new DataTable();
        sad = new SqlDataAdapter("select Id,JobNo,DateIn,CustName,MateName,SrNo,Subcustomer,MateStatus,[otherinfo],[Imagepath],FinalStatus,TestBy,CreatedBy,CreatedDate,RepeatedNo,Branch,ModelNo,ProductFault, DATEDIFF(DAY, DateIn, getdate()) AS days from tblInwardEntry where JobNo= '" + jobno + "'", con);
        sad.Fill(dt);
        sortedgv.EmptyDataText = "Not Records Found";
        sortedgv.DataSource = dt;
        sortedgv.DataBind();
    }
    public void GetsortedJobNogrid()
    {
        gv_Inward.Visible = false;
        string jobno = txtJobNo.Text;
        DataTable dt = new DataTable();
        sad = new SqlDataAdapter("select Id,JobNo,DateIn,CustName,MateName,SrNo,Subcustomer,MateStatus,[otherinfo],[Imagepath],FinalStatus,TestBy,CreatedBy,CreatedDate,RepeatedNo,Branch,ModelNo,ProductFault, DATEDIFF(DAY, DateIn, getdate()) AS days from tblInwardEntry where JobNo= '" + jobno + "'", con);
        sad.Fill(dt);
        sortedgv.EmptyDataText = "Not Records Found";
        sortedgv.DataSource = dt;
        sortedgv.DataBind();
    }
    public void GetsortedCustomer()
    {
        gv_Inward.Visible = false;
        ViewState["Record"] = "Customer";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],[FinalStatus],[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry] Where CustName='" + txtSearch.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        sortedgv.EmptyDataText = "Not Records Found";
        sortedgv.DataSource = dt;
        sortedgv.DataBind();
    }
    public void GetsortedCustomergrid()
    {
        gv_Inward.Visible = false;
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],[FinalStatus],[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry] Where CustName='" + txtSearch.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        sortedgv.EmptyDataText = "Not Records Found";
        sortedgv.DataSource = dt;
        sortedgv.DataBind();
    }
    public void Getsortedproduct()
    {
        gv_Inward.Visible = false;
        ViewState["Record"] = "Product";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],[FinalStatus],[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry] Where MateName='" + txtSearchProduct.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        sortedgv.EmptyDataText = "Not Records Found";
        sortedgv.DataSource = dt;
        sortedgv.DataBind();
    }
    public void Getsortedproductgrid()
    {
        gv_Inward.Visible = false;
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],[FinalStatus],[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry] Where MateName='" + txtSearchProduct.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        sortedgv.EmptyDataText = "Not Records Found";
        sortedgv.DataSource = dt;
        sortedgv.DataBind();
    }
    public void Sortedstatus()
    {
        gv_Inward.Visible = false;
        ViewState["Record"] = "status";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],[FinalStatus],[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry] Where MateStatus='" + txtSearchStatus.Text + "' AND isdeleted='0'", con);
        //SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM tblInwardEntry WHERE MateStatus ='" + txtSearchStatus.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        sortedgv.EmptyDataText = "Not Records Found";
        sortedgv.DataSource = dt;
        sortedgv.DataBind();
    }
    public void Sortedstatusgrid()
    {
        gv_Inward.Visible = false;
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],[FinalStatus],[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry] Where MateStatus='" + txtSearchStatus.Text + "' AND isdeleted='0'", con);
        //SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM tblInwardEntry WHERE MateStatus ='" + txtSearchStatus.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        sortedgv.EmptyDataText = "Not Records Found";
        sortedgv.DataSource = dt;
        sortedgv.DataBind();
    }
    public void Getsorteddatwise()
    {
        gv_Inward.Visible = false;
        ViewState["Record"] = "Date";
        DateTime date = Convert.ToDateTime(txtDateSearch.Text.ToString());
        DataTable dt = new DataTable();
        txtDateSearch.Text = date.ToString("yyyy-MM-dd");
        SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],FinalStatus,[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry] Where DateIn='" + txtDateSearch.Text + "' AND isdeleted='0' ", con);
        sad.Fill(dt);
        sortedgv.EmptyDataText = "Not Records Found";
        sortedgv.DataSource = dt;
        sortedgv.DataBind();
    }
    public void Getsorteddatwisegrid()
    {
        gv_Inward.Visible = false;
        DateTime date = Convert.ToDateTime(txtDateSearch.Text.ToString());
        DataTable dt = new DataTable();
        txtDateSearch.Text = date.ToString("yyyy-MM-dd");
        SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],FinalStatus,[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry] Where DateIn='" + txtDateSearch.Text + "' AND isdeleted='0' ", con);
        sad.Fill(dt);
        sortedgv.EmptyDataText = "Not Records Found";
        sortedgv.DataSource = dt;
        sortedgv.DataBind();
    }
    public void Getsortedreatedno()
    {
        gv_Inward.Visible = false;
        ViewState["Record"] = "RatedNo";
        DataTable dt = new DataTable();
        //txtDateSearch.Text = date.ToString("yyyy-MM-dd");
        SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],FinalStatus,[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry] Where RepeatedNo='" + txtreatedNo.Text + "' AND isdeleted='0' ", con);
        sad.Fill(dt);
        sortedgv.EmptyDataText = "Not Records Found";
        sortedgv.DataSource = dt;
        sortedgv.DataBind();
    }
    public void Getsortedreatednogrid()
    {
        gv_Inward.Visible = false;
        DataTable dt = new DataTable();
        //txtDateSearch.Text = date.ToString("yyyy-MM-dd");
        SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],FinalStatus,[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry] Where RepeatedNo='" + txtreatedNo.Text + "' AND isdeleted='0' ", con);
        sad.Fill(dt);
        sortedgv.EmptyDataText = "Not Records Found";
        sortedgv.DataSource = dt;
        sortedgv.DataBind();
    }
    public void GetsorteddatwiseCustomer()
    {
        gv_Inward.Visible = false;
        ViewState["Record"] = "Datewisecustomer";
        DateTime date = Convert.ToDateTime(txtDateSearch.Text.ToString());
        DataTable dt = new DataTable();
        txtDateSearch.Text = date.ToString("yyyy-MM-dd");
        SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],FinalStatus,[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry] Where DateIn='" + txtDateSearch.Text + "' AND CustName='" + txtSearch.Text + "' AND isdeleted='0' ", con);
        sad.Fill(dt);
        sortedgv.EmptyDataText = "Not Records Found";
        sortedgv.DataSource = dt;
        sortedgv.DataBind();
    }
    public void GetsorteddatwiseCustomergrid()
    {
        gv_Inward.Visible = false;
        DateTime date = Convert.ToDateTime(txtDateSearch.Text.ToString());
        DataTable dt = new DataTable();
        txtDateSearch.Text = date.ToString("yyyy-MM-dd");
        SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],FinalStatus,[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry] Where DateIn='" + txtDateSearch.Text + "' AND CustName='" + txtSearch.Text + "' AND isdeleted='0' ", con);
        sad.Fill(dt);
        sortedgv.EmptyDataText = "Not Records Found";
        sortedgv.DataSource = dt;
        sortedgv.DataBind();
    }
    public void Getsortedcustomerwiserepeatedno()
    {
        gv_Inward.Visible = false;
        ViewState["Record"] = "customerwiserepeatedno";
        //DateTime date = Convert.ToDateTime(txtDateSearch.Text.ToString());
        DataTable dt = new DataTable();
        //txtDateSearch.Text = date.ToString("yyyy-MM-dd");
        SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],FinalStatus,[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry] Where RepeatedNo='" + txtreatedNo.Text + "' AND CustName='" + txtSearch.Text + "' AND isdeleted='0' ", con);
        sad.Fill(dt);
        sortedgv.EmptyDataText = "Not Records Found";
        sortedgv.DataSource = dt;
        sortedgv.DataBind();
    }
    public void Getsortedcustomerwiserepeatednogrid()
    {
        gv_Inward.Visible = false;
        //DateTime date = Convert.ToDateTime(txtDateSearch.Text.ToString());
        DataTable dt = new DataTable();
        //txtDateSearch.Text = date.ToString("yyyy-MM-dd");
        SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],FinalStatus,[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry] Where RepeatedNo='" + txtreatedNo.Text + "' AND CustName='" + txtSearch.Text + "' AND isdeleted='0' ", con);
        sad.Fill(dt);
        sortedgv.EmptyDataText = "Not Records Found";
        sortedgv.DataSource = dt;
        sortedgv.DataBind();
    }
    public void GetsortedDatafromdateToDate()
    {
        gv_Inward.Visible = false;
        ViewState["Record"] = "FTDate";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],FinalStatus,[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry] Where DateIn between'" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "' ", con);
        //SqlDataAdapter sad = new SqlDataAdapter("select * from tblInwardEntry where DateIn between '" + txtDateSearchfrom.Text + "' AND  '" + txtDateSearchto.Text + "' ", con);
        sad.Fill(dt);
        sortedgv.EmptyDataText = "Not Records Found";
        sortedgv.DataSource = dt;
        sortedgv.DataBind();
    }
    public void GetsortedDatafromdateToDategrid()
    {
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],FinalStatus,[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry] Where DateIn between'" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "' ", con);
        //SqlDataAdapter sad = new SqlDataAdapter("select * from tblInwardEntry where DateIn between '" + txtDateSearchfrom.Text + "' AND  '" + txtDateSearchto.Text + "' ", con);
        sad.Fill(dt);
        sortedgv.EmptyDataText = "Not Records Found";
        sortedgv.DataSource = dt;
        sortedgv.DataBind();
    }

    public void GetDatewiseJobNo()
    {
        gv_Inward.Visible = false;
        ViewState["Record"] = "DatewiseJob";
        string jobno = txtJobNo.Text;
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],FinalStatus,[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry] Where DateIn between'" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "'  AND JobNo='" + txtJobNo.Text + "'", con);
        sad.Fill(dt);
        sortedgv.EmptyDataText = "Not Records Found";
        sortedgv.DataSource = dt;
        sortedgv.DataBind();
    }


    public void GetDatewiseJobNogrid()
    {
        gv_Inward.Visible = false;
        string jobno = txtJobNo.Text;
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],FinalStatus,[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry] Where DateIn between'" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "'  AND JobNo='" + txtJobNo.Text + "'", con);
        sad.Fill(dt);
        sortedgv.EmptyDataText = "Not Records Found";
        sortedgv.DataSource = dt;
        sortedgv.DataBind();
    }

    public void GetDatewiseCustomer()
    {
        gv_Inward.Visible = false;
        ViewState["Record"] = "Datewisecustomer";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],FinalStatus,[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry] Where DateIn between'" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "'  AND CustName='" + txtSearch.Text + "'", con);
        sad.Fill(dt);
        sortedgv.EmptyDataText = "Not Records Found";
        sortedgv.DataSource = dt;
        sortedgv.DataBind();
    }

    public void GetDatewiseCustomergrid()
    {
        gv_Inward.Visible = false;
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],FinalStatus,[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry] Where DateIn between'" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "'  AND CustName='" + txtSearch.Text + "'", con);
        sad.Fill(dt);
        sortedgv.EmptyDataText = "Not Records Found";
        sortedgv.DataSource = dt;
        sortedgv.DataBind();
    }


    public void Getdatewiseproduct()
    {
        gv_Inward.Visible = false;
        ViewState["Record"] = "DatewiseProduct";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],FinalStatus,[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry] Where DateIn between'" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "'  AND MateName='" + txtSearchProduct.Text + "'", con);
        sad.Fill(dt);
        sortedgv.EmptyDataText = "Not Records Found";
        sortedgv.DataSource = dt;
        sortedgv.DataBind();
    }


    public void Getdatewiseproductgrid()
    {
        gv_Inward.Visible = false;
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],FinalStatus,[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry] Where DateIn between'" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "'  AND MateName='" + txtSearchProduct.Text + "'", con);
        sad.Fill(dt);
        sortedgv.EmptyDataText = "Not Records Found";
        sortedgv.DataSource = dt;
        sortedgv.DataBind();
    }



    public void GetdatwiseStatus()
    {
        gv_Inward.Visible = false;
        ViewState["Record"] = "Datewisestatus";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],FinalStatus,[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry] Where DateIn between'" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "'  AND MateStatus='" + txtSearchStatus.Text + "'", con);
        sad.Fill(dt);
        sortedgv.EmptyDataText = "Not Records Found";
        sortedgv.DataSource = dt;
        sortedgv.DataBind();
    }


    public void GetdatwiseStatusgrid()
    {
        gv_Inward.Visible = false;
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],FinalStatus,[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry] Where DateIn between'" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "'  AND MateStatus='" + txtSearchStatus.Text + "'", con);
        sad.Fill(dt);
        sortedgv.EmptyDataText = "Not Records Found";
        sortedgv.DataSource = dt;
        sortedgv.DataBind();
    }

    public void GetdatewiseReptedNo()
    {
        gv_Inward.Visible = false;
        ViewState["Record"] = "datewiseRatedNo";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],FinalStatus,[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry] Where DateIn between'" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "'  AND RepeatedNo='" + txtreatedNo.Text + "'", con);
        sad.Fill(dt);
        sortedgv.EmptyDataText = "Not Records Found";
        sortedgv.DataSource = dt;
        sortedgv.DataBind();
    }

    public void GetdatewiseReptedNogrid()
    {
        gv_Inward.Visible = false;
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],FinalStatus,[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry] Where DateIn between'" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "'  AND RepeatedNo='" + txtreatedNo.Text + "'", con);
        sad.Fill(dt);
        sortedgv.EmptyDataText = "Not Records Found";
        sortedgv.DataSource = dt;
        sortedgv.DataBind();
    }
    //sorted grid end


    public override void VerifyRenderingInServerForm(Control control)
    {
        //required to avoid the run time error "  
        //Control 'GridView1' of type 'Grid View' must be placed inside a form tag with runat=server."  
    }

    protected void btnexportexcel_Click(object sender, EventArgs e)
    {

        ExportGridToExcel();
    }

    private void ExportGridToExcel()
    {
        if (ViewState["Excell"] != null)
        {
            string Method = ViewState["Excell"].ToString();

            if (Method == "GetDatewiseCustomer")
            {
                GetDatewiseCustomerforexcell();
            }
            if (Method == "Getdatewiseproduct")
            {
                GetdatewiseproductforExcell();
            }
            if (Method == "GetdatwiseStatus")
            {
                GetdatwiseStatusForExcell();
            }
            if (Method == "GetdatewiseReptedNo")
            {
                GetdatewiseReptedNoForExcell();
            }
            if (Method == "GetDatewiseJobNo")
            {
                GetDatewiseJobNoForExcell();

            }
            if (Method == "GetsortedDatafromdateToDate")
            {
                GetsortedDatafromdateToDateForExcell();

            }
            if (Method == "Getsortedcustomerwiserepeatedno")
            {
                GetsortedcustomerwiserepeatednoForEXcell();
            }
            if (Method == "GetsorteddatwiseCustomer")
            {
                GetsorteddatwiseCustomerforexcell();
            }
            if (Method == "Getsortedreatedno")
            {
                GetsortedreatednoForExcell();
            }
            if (Method == "Getsorteddatwise")
            {
                GetsorteddatwiseForExcell();
            }
            if (Method == "Sortedstatus")
            {
                SortedstatusForExcell();
            }
            if (Method == "Getsortedproduct")
            {
                GetsortedproductForExcell();
            }
            if (Method == "GetsortedCustomer")
            {
                GetsortedCustomerForExcell();
            }
            if (Method == "GetsortedJobNo")
            {
                GetsortedJobNoForExcell();
            }
        }
        else
        {
            GridExportExcel();
        }

        Response.Clear();
        Response.Buffer = true;
        Response.ClearContent();
        Response.ClearHeaders();
        Response.Charset = "";
        string FileName = "Inward_Entry_List_" + DateTime.Now + ".xls";
        StringWriter strwritter = new StringWriter();
        HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
        GridEporttoexcel.GridLines = GridLines.Both;
        GridEporttoexcel.HeaderStyle.Font.Bold = true;
        GridEporttoexcel.RenderControl(htmltextwrtter);
        Response.Write(strwritter.ToString());
        Response.End();
    }

    public void GetDatewiseCustomerforexcell()
    {
        gv_Inward.Visible = false;
        ViewState["Record"] = "Datewisecustomer";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],FinalStatus,[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry] Where DateIn between'" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "'  AND CustName='" + txtSearch.Text + "'", con);
        sad.Fill(dt);
        GridEporttoexcel.EmptyDataText = "Not Records Found";
        GridEporttoexcel.DataSource = dt;
        GridEporttoexcel.DataBind();
    }
    public void GetdatewiseproductforExcell()
    {
        gv_Inward.Visible = false;
        ViewState["Record"] = "DatewiseProduct";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],FinalStatus,[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry] Where DateIn between'" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "'  AND MateName='" + txtSearchProduct.Text + "'", con);
        sad.Fill(dt);
        GridEporttoexcel.EmptyDataText = "Not Records Found";
        GridEporttoexcel.DataSource = dt;
        GridEporttoexcel.DataBind();
    }
    public void GetdatwiseStatusForExcell()
    {
        gv_Inward.Visible = false;
        ViewState["Record"] = "Datewisestatus";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],FinalStatus,[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry] Where DateIn between'" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "'  AND MateStatus='" + txtSearchStatus.Text + "'", con);
        sad.Fill(dt);
        GridEporttoexcel.EmptyDataText = "Not Records Found";
        GridEporttoexcel.DataSource = dt;
        GridEporttoexcel.DataBind();
    }
    public void GetdatewiseReptedNoForExcell()
    {
        gv_Inward.Visible = false;
        ViewState["Record"] = "datewiseRatedNo";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],FinalStatus,[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry] Where DateIn between'" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "'  AND RepeatedNo='" + txtreatedNo.Text + "'", con);
        sad.Fill(dt);
        GridEporttoexcel.EmptyDataText = "Not Records Found";
        GridEporttoexcel.DataSource = dt;
        GridEporttoexcel.DataBind();
    }
    public void GetDatewiseJobNoForExcell()
    {
        gv_Inward.Visible = false;
        ViewState["Record"] = "DatewiseJob";
        string jobno = txtJobNo.Text;
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],FinalStatus,[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry] Where DateIn between'" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "'  AND JobNo='" + txtJobNo.Text + "'", con);
        sad.Fill(dt);
        GridEporttoexcel.EmptyDataText = "Not Records Found";
        GridEporttoexcel.DataSource = dt;
        GridEporttoexcel.DataBind();
    }
    public void GetsortedDatafromdateToDateForExcell()
    {
        gv_Inward.Visible = false;
        ViewState["Record"] = "FTDate";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],FinalStatus,[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry] Where DateIn between'" + txtDateSearchfrom.Text + "' AND '" + txtDateSearchto.Text + "' ", con);
        //SqlDataAdapter sad = new SqlDataAdapter("select * from tblInwardEntry where DateIn between '" + txtDateSearchfrom.Text + "' AND  '" + txtDateSearchto.Text + "' ", con);
        sad.Fill(dt);
        GridEporttoexcel.EmptyDataText = "Not Records Found";
        GridEporttoexcel.DataSource = dt;
        GridEporttoexcel.DataBind();
    }
    public void GetsortedcustomerwiserepeatednoForEXcell()
    {
        gv_Inward.Visible = false;
        ViewState["Record"] = "customerwiserepeatedno";
        //DateTime date = Convert.ToDateTime(txtDateSearch.Text.ToString());
        DataTable dt = new DataTable();
        //txtDateSearch.Text = date.ToString("yyyy-MM-dd");
        SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],FinalStatus,[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry] Where RepeatedNo='" + txtreatedNo.Text + "' AND CustName='" + txtSearch.Text + "' AND isdeleted='0' ", con);
        sad.Fill(dt);
        GridEporttoexcel.EmptyDataText = "Not Records Found";
        GridEporttoexcel.DataSource = dt;
        GridEporttoexcel.DataBind();
    }
    public void GetsorteddatwiseCustomerforexcell()
    {
        gv_Inward.Visible = false;
        ViewState["Record"] = "Datewisecustomer";
        DateTime date = Convert.ToDateTime(txtDateSearch.Text.ToString());
        DataTable dt = new DataTable();
        txtDateSearch.Text = date.ToString("yyyy-MM-dd");
        SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],FinalStatus,[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry] Where DateIn='" + txtDateSearch.Text + "' AND CustName='" + txtSearch.Text + "' AND isdeleted='0' ", con);
        sad.Fill(dt);
        GridEporttoexcel.EmptyDataText = "Not Records Found";
        GridEporttoexcel.DataSource = dt;
        GridEporttoexcel.DataBind();
    }
    public void GetsortedreatednoForExcell()
    {
        gv_Inward.Visible = false;
        ViewState["Record"] = "RatedNo";
        DataTable dt = new DataTable();
        //txtDateSearch.Text = date.ToString("yyyy-MM-dd");
        SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],FinalStatus,[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry] Where RepeatedNo='" + txtreatedNo.Text + "' AND isdeleted='0' ", con);
        sad.Fill(dt);
        GridEporttoexcel.EmptyDataText = "Not Records Found";
        GridEporttoexcel.DataSource = dt;
        GridEporttoexcel.DataBind();
    }
    public void GetsorteddatwiseForExcell()
    {
        gv_Inward.Visible = false;
        ViewState["Record"] = "Date";
        DateTime date = Convert.ToDateTime(txtDateSearch.Text.ToString());
        DataTable dt = new DataTable();
        txtDateSearch.Text = date.ToString("yyyy-MM-dd");
        SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],FinalStatus,[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry] Where DateIn='" + txtDateSearch.Text + "' AND isdeleted='0' ", con);
        sad.Fill(dt);
        GridEporttoexcel.EmptyDataText = "Not Records Found";
        GridEporttoexcel.DataSource = dt;
        GridEporttoexcel.DataBind();
    }
    public void SortedstatusForExcell()
    {
        gv_Inward.Visible = false;
        ViewState["Record"] = "status";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],[FinalStatus],[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry] Where MateStatus='" + txtSearchStatus.Text + "' AND isdeleted='0'", con);
        //SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM tblInwardEntry WHERE MateStatus ='" + txtSearchStatus.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        GridEporttoexcel.EmptyDataText = "Not Records Found";
        GridEporttoexcel.DataSource = dt;
        GridEporttoexcel.DataBind();
    }
    public void GetsortedproductForExcell()
    {
        gv_Inward.Visible = false;
        ViewState["Record"] = "Product";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],[FinalStatus],[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry] Where MateName='" + txtSearchProduct.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        GridEporttoexcel.EmptyDataText = "Not Records Found";
        GridEporttoexcel.DataSource = dt;
        GridEporttoexcel.DataBind();
    }
    public void GetsortedCustomerForExcell()
    {
        gv_Inward.Visible = false;
        ViewState["Record"] = "Customer";
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],[FinalStatus],[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, DateIn, getdate()) AS days FROM [tblInwardEntry] Where CustName='" + txtSearch.Text + "' AND isdeleted='0'", con);
        sad.Fill(dt);
        GridEporttoexcel.EmptyDataText = "Not Records Found";
        GridEporttoexcel.DataSource = dt;
        GridEporttoexcel.DataBind();
    }
    public void GetsortedJobNoForExcell()
    {
        gv_Inward.Visible = false;
        ViewState["Record"] = "Job";
        string jobno = txtJobNo.Text;
        DataTable dt = new DataTable();
        sad = new SqlDataAdapter("select Id,JobNo,DateIn,CustName,MateName,SrNo,Subcustomer,MateStatus,[otherinfo],[Imagepath],FinalStatus,TestBy,CreatedBy,CreatedDate,RepeatedNo,Branch,ModelNo,ProductFault, DATEDIFF(DAY, DateIn, getdate()) AS days from tblInwardEntry where JobNo= '" + jobno + "'", con);
        sad.Fill(dt);
        GridEporttoexcel.EmptyDataText = "Not Records Found";
        GridEporttoexcel.DataSource = dt;
        GridEporttoexcel.DataBind();
    }
}

