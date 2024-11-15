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
using System.ComponentModel;

public partial class Reception_PoPupTesting : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
    string id = "";
    string Id = "";
    DataTable dtcomponant = new DataTable();
    DataTable Dt_Itemsdetails = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //  this.txttestingdate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            // this.txttestingdate.TextMode = TextBoxMode.Date;
            if (Request.QueryString["JobNo"] != null)
            {
                id = Decrypt(Request.QueryString["JobNo"].ToString());
                loadData(id);
            }

            // DropDownComp();
            // CompGridview();
            //if (Session["adminname"] != null)
            // {
            //     txtEngiName.Text = Session["adminname"].ToString();
            // }
            ViewState["RowNo"] = 0;

            dtcomponant.Columns.AddRange(new DataColumn[5] { new DataColumn("No"),new DataColumn("Jobno"), new DataColumn("CompId"),
                new DataColumn("CompName"),new DataColumn("Quantity")
               
            });
           
            ViewState["CompData"] = dtcomponant;

            if (Request.QueryString["id"] != null)
            {
                Id = Decrypt(Request.QueryString["id"].ToString());
                loaddataEdit();
            }


        }
    }

    //Component list Autocomplate 
    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetComponetList(string prefixText, int count)
    {
        return AutoFillComponentlist(prefixText);
    }
    public static List<string> AutoFillComponentlist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT CompName FROM tblComponent WHERE " + " CompName like @Search + '%' AND isdeleted='0'";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> CompName = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        CompName.Add(sdr["CompName"].ToString());
                    }
                }
                con.Close();
                return CompName;
            }
        }
    }

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]

    public static List<string> GeCustomerList(string prefixText, int count)
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
                //com.CommandText = "select DISTINCT CustomerName,Custid from tblCustomer where " + "CustomerName like @Search + '%' " + "AND isdeleted = '0' AND IsStatus = '1'";


                com.CommandText = "SELECT  DISTINCT CustomerName  FROM [tblTestingProduct]  As TP Inner join  tblRepairingEvalution As TS on  TP.JobNo=  TS.JobNo  where " + "CustomerName like @Search + '%' " + "AND TP.isdeleted = '0'";

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

    //Component Job No list Autocomplate 
    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GeJobnoList(string prefixText, int count)
    {
        return AutoFillJobNolist(prefixText);
    }
    public static List<string> AutoFillJobNolist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT JobNo FROM [tblTestingProduct] WHERE " + " JobNo like @Search + '%' AND isdeleted='0'";

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

    protected void loaddataEdit()
    {
        try
        {
            btnSubmit.Text = "Update";
            SqlDataAdapter sad = new SqlDataAdapter("select * FROM [tblTestingProduct] where id='" + Id + "'", con);

            DataTable dt = new DataTable();
            sad.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                txttagno.Text = dt.Rows[0]["JobNo"].ToString();
                txtproductname.Text = dt.Rows[0]["ProductName"].ToString();
                txtEngiName.Text = dt.Rows[0]["EngiName"].ToString();

                // txtStatus.Text = dt.Rows[0]["Status"].ToString();
                // txtRemark.Text = dt.Rows[0]["Remark"].ToString();
                // txtreportedto.Text = dt.Rows[0]["ReportedTO"].ToString();
                //DateTime f1 = Convert.ToDateTime(dt.Rows[0]["TestingDate"].ToString());
                //txttestingdate.Text = f1.ToString("yyyy-MM-dd");
                DateTime ffff4 = Convert.ToDateTime(dt.Rows[0]["TestingDate"].ToString());
                //  txttestingdate.Text = ffff4.ToString("yyyy-MM-dd");
            }
            /////bind gridview componants
            SqlDataAdapter sadcom = new SqlDataAdapter("select * FROM tblRepairingEvalution where JobNo='" + txttagno.Text + "'", con);

            DataTable dtcom = new DataTable();
            sadcom.Fill(dtcom);
            int count = 0;
            for (int i = 0; i < dtcom.Rows.Count; i++)
            {
                dtcomponant.Rows.Add(count, dtcom.Rows[i]["JobNo"].ToString(), dtcom.Rows[i]["CompId"].ToString(), dtcom.Rows[i]["CompName"].ToString(), dtcom.Rows[i]["Quantity"].ToString());
                count = count + 1;
            }
            //gv_Comp.DataSource = dtcomponant;
            //gv_Comp.DataBind();
            //gv_Comp.EmptyDataText = "Records Not Found";
            Gv_CompNew.DataSource = dtcomponant;
            Gv_CompNew.DataBind();
            Gv_CompNew.EmptyDataText = "Records Not Found";
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void loadData(string id)
    {
        try
        {

            SqlDataAdapter sad = new SqlDataAdapter("select [id],[JobNo],[CustomerName],[ModelNo],[SerialNo],[EngiName],[ReportedTO],[TestingDate],[Status],[Remark],[EntryDate],[isCompleted],[isdeleted],[isCancel],[CreatedBy],[CreatedDate],[UpdateBy],[UpdatedDate],ProductName FROM [tblTestingProduct] where JobNo='" + id + "'", con);

            DataTable dt = new DataTable();
            sad.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                txtproductname.Text = dt.Rows[0]["ProductName"].ToString();
                txtcustomername.Text = dt.Rows[0]["CustomerName"].ToString();
                txtEngiName.Text = dt.Rows[0]["EngiName"].ToString();
                ViewState["ID"] = dt.Rows[0]["id"].ToString();
                txttagno.DataSource = dt;
                txttagno.DataTextField = "JobNo";
                txttagno.DataBind();
                //txttagno.Items.Insert(0, new ListItem("--Select--", "0"));
                string jobno = id;
                GetissuedcomponetforUpdate(jobno);
                btnUpdate.Visible = true;
                btnSubmit.Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    protected void lnkbtnAdd_Click(object sender, EventArgs e)
    {
        {
            if (txtcomponent.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Please Select Componant !!!');", true);
            }
            else
            {
                ViewState["RowNo"] = (int)ViewState["RowNo"] + 1;
                DataTable dt = (DataTable)ViewState["CompData"];
                DataRow dr2 = dt.NewRow();
                bool ifExist = false;

                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["CompId"].ToString() == txtcomponent.Text.Trim())
                    {
                        ifExist = true;
                    }
                }
                if (ifExist == false)
                {
                    dt.Rows.Add(ViewState["RowNo"], txttagno.Text, txtcomponent.Text, txtcomponent.Text, txtQuantityComp.Text);
                    ViewState["CompData"] = dt;
                    txtcomponent.Text = string.Empty;
                    // txtQuantity1.Text = string.Empty;

                    gv_Comp.DataSource = (DataTable)ViewState["CompData"];
                    gv_Comp.DataBind();
                    txtQuantityComp.Text = "";
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Selected Componant Already Exit !!!');", true);
                }

            }

        }
    }

    //void DropDownComp()
    //{
    //    try
    //    {
    //        DataTable dt = new DataTable();

    //        con.Open();
    //        SqlDataAdapter sad = new SqlDataAdapter("select [Compid],[CompName],[IsStatus],[CreateBy],[CreateDate],[UpdateBy],[UpdateDate] FROM [tblComponent] where isdeleted='0' AND IsStatus='1'", con);
    //        sad.Fill(dt);
    //        DropDownListComp.DataValueField = "Compid";
    //        DropDownListComp.DataTextField = "CompName";

    //        DropDownListComp.DataSource = dt;
    //        DropDownListComp.DataBind();

    //        con.Close();
    //    }
    //    catch (Exception)
    //    {

    //        throw;
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
    void CompGridview()
    {
        try
        {
            DataTable dt = new DataTable();

            con.Open();
            SqlDataAdapter sad = new SqlDataAdapter("select [Id], [JobNo],[CompId],[CompName] from [tblRepairingEvalution]", con);
            sad.Fill(dt);

            gv_Comp.EmptyDataText = "Not Records Found";
            gv_Comp.DataSource = dt;
            gv_Comp.DataBind();

            con.Close();

        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            string CretedBy = Session["adminname"].ToString();
            if (btnSubmit.Text == "Submit")
            {
                con.Open();
                SqlCommand Cmd = new SqlCommand("SP_IssuedComponet", con);
                Cmd.CommandType = CommandType.StoredProcedure;
                //Cmd.Parameters.AddWithValue("@Action", "Insert");
                Cmd.Parameters.AddWithValue("@Action", "Add");
                Cmd.Parameters.AddWithValue("@JobNo", txttagno.Text.Trim());
                Cmd.Parameters.AddWithValue("@CustomerName", txtcustomername.Text.Trim());
                Cmd.Parameters.AddWithValue("@ProductName", txtproductname.Text.Trim());
                Cmd.Parameters.AddWithValue("@EngineerName", txtEngiName.Text.Trim());
                Cmd.Parameters.AddWithValue("@CreatedBy", CretedBy);
                Cmd.Parameters.AddWithValue("@Createddate", DateTime.Now);
                Cmd.Parameters.AddWithValue("@IssuedDate", txtissudedate.Text.Trim());

                Cmd.ExecuteNonQuery();
                con.Close();

                //Save Tax Invoice Tempary Details 
                foreach (GridViewRow grd1 in gv_Comp.Rows)
                {
                    con.Open();
                    string lblcompid = (grd1.FindControl("lblcompid") as Label).Text;
                    string lblCompnamee = (grd1.FindControl("lblCompnamee") as Label).Text;
                    string lblQty = (grd1.FindControl("lblQty") as Label).Text;
                    string lblJobno = (grd1.FindControl("lblJobno") as Label).Text;

                    SqlCommand cmdd = new SqlCommand("INSERT INTO [Tbl_IssuedComponetDtls] (CompId,CompName,Quantity,JobNo) VALUES(@CompId,@CompName,@Quantity,@JobNo)", con);

                    cmdd.Parameters.AddWithValue("@CompId", lblcompid);
                    cmdd.Parameters.AddWithValue("@CompName", lblCompnamee);
                    cmdd.Parameters.AddWithValue("@Quantity", lblQty);
                    cmdd.Parameters.AddWithValue("@JobNo", lblJobno);
                    cmdd.ExecuteNonQuery();
                    con.Close();
                }

                //Save Tax Invoice From databse Details 
                foreach (GridViewRow grd1 in Gv_CompNew.Rows)
                {
                    con.Open();
                    string lblcompid = (grd1.FindControl("lblcompid") as Label).Text;
                    string lblCompnamee = (grd1.FindControl("lblCompnamee") as Label).Text;
                    string lblQty = (grd1.FindControl("lblQty") as Label).Text;
                    string lblJobno = (grd1.FindControl("lblJobno") as Label).Text;

                    SqlCommand cmdd = new SqlCommand("INSERT INTO [Tbl_IssuedComponetDtls] (CompId,CompName,Quantity,JobNo) VALUES(@CompId,@CompName,@Quantity,@JobNo)", con);

                    cmdd.Parameters.AddWithValue("@CompId", lblcompid);
                    cmdd.Parameters.AddWithValue("@CompName", lblCompnamee);
                    cmdd.Parameters.AddWithValue("@Quantity", lblQty);
                    cmdd.Parameters.AddWithValue("@JobNo", lblJobno);
                    cmdd.ExecuteNonQuery();
                    con.Close();
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Save Successfully..!!');window.location='IssuedComponentList.aspx'; ", true);

                //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Save Successfully..!!') ", true);
                //Save Contact Details End
            }
            else if (btnSubmit.Text == "Update")
            {
                SqlCommand Cmd = new SqlCommand("SPIssuedComp", con);
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.AddWithValue("@Action", "Update");
                Cmd.Parameters.AddWithValue("@JobNo", txttagno.Text.Trim());
                Cmd.Parameters.AddWithValue("@CustomerName", txtcustomername.Text.Trim());
                Cmd.Parameters.AddWithValue("@ProductName", txtproductname.Text.Trim());
                Cmd.Parameters.AddWithValue("@EngineerName", txtEngiName.Text.Trim());
                Cmd.Parameters.AddWithValue("@UpdatedOn", DateTime.Now);
                Cmd.Parameters.AddWithValue("@IssuedDate", txtissudedate.Text.Trim());
                Cmd.Parameters.AddWithValue("@IsDeleted", '0');
                Cmd.Parameters.AddWithValue("@UpdatedBy", Session["Username"].ToString());
                Cmd.ExecuteNonQuery();

                SqlCommand cmdd = new SqlCommand("INSERT INTO [Tbl_IssuedComponetDtls] (CompId,CompName,Quantity,JobNo) VALUES(@CompId,@CompName,@Quantity,@JobNo)", con);

                SqlCommand cmddelete = new SqlCommand("DELETE FROM Tbl_IssuedComponetDtls WHERE JobNo=@JobNo", con);
                cmddelete.Parameters.AddWithValue("@JobNo", txttagno.Text);
                cmddelete.ExecuteNonQuery();


                //Update Tax Invoice Tempary Details 
                foreach (GridViewRow grd1 in gv_Comp.Rows)
                {
                    con.Open();
                    string lblcompid = (grd1.FindControl("lblcompid") as Label).Text;
                    string lblCompnamee = (grd1.FindControl("lblCompnamee") as Label).Text;
                    string lblQty = (grd1.FindControl("lblQty") as Label).Text;
                    string lblJobno = (grd1.FindControl("lblJobno") as Label).Text;
                    SqlCommand cmd = new SqlCommand("INSERT INTO [Tbl_IssuedComponetDtls] (CompId,CompName,Quantity,JobNo) VALUES(@CompId,@CompName,@Quantity,@JobNo)", con);
                    cmd.Parameters.AddWithValue("@CompId", lblcompid);
                    cmd.Parameters.AddWithValue("@CompName", lblCompnamee);
                    cmd.Parameters.AddWithValue("@Quantity", lblQty);
                    cmd.Parameters.AddWithValue("@JobNo", lblJobno);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                //Update Tax Invoice  databse Details End

                foreach (GridViewRow grd1 in Gv_CompNew.Rows)
                {
                    con.Open();
                    string lblcompid = (grd1.FindControl("lblcompid") as Label).Text;
                    string lblCompnamee = (grd1.FindControl("lblCompnamee") as Label).Text;
                    string lblQty = (grd1.FindControl("lblQty") as Label).Text;
                    string lblJobno = (grd1.FindControl("lblJobno") as Label).Text;
                    SqlCommand cmd = new SqlCommand("INSERT INTO [Tbl_IssuedComponetDtls] (CompId,CompName,Quantity,JobNo) VALUES(@CompId,@CompName,@Quantity,@JobNo)", con);
                    cmd.Parameters.AddWithValue("@CompId", lblcompid);
                    cmd.Parameters.AddWithValue("@CompName", lblCompnamee);
                    cmd.Parameters.AddWithValue("@Quantity", lblQty);
                    cmd.Parameters.AddWithValue("@JobNo", lblJobno);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Updated Successfully..!!') ", true);
                Response.Redirect("IssuedComponentList.aspx");
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void gv_Comp_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    protected void lnkbtnDelete_Click(object sender, EventArgs e)
    {
        GridViewRow row = (sender as LinkButton).NamingContainer as GridViewRow;
        DataTable dt = ViewState["CompData"] as DataTable;
        dt.Rows.Remove(dt.Rows[row.RowIndex]);
        ViewState["CompData"] = dt;
        gv_Comp.DataSource = (DataTable)ViewState["CompData"];
        gv_Comp.DataBind();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Data Delete Successfully !!!');", true);
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("IssuedComponentList.aspx");
    }
    //Changes for databinding
    protected void txtjobno_TextChanged(object sender, EventArgs e)
    {
        DataTable Dt = new DataTable();
        // SqlDataAdapter Daa = new SqlDataAdapter("SELECT ProductName,EngiName FROM [tblTestingProduct] WHERE CustomerName='" + txtcustomername.Text + "' AND JobNo = '" + txttagno.Text + "'", con);
        SqlDataAdapter Daa = new SqlDataAdapter("SELECT ProductName,EngiName FROM [tblTestingProduct] WHERE CustomerName='" + txtcustomername.Text + "' AND JobNo = '" + txttagno.Text + "'", con);
        Daa.Fill(Dt);
        if (Dt.Rows.Count > 0)
        {
            //txtcustomername.Text = Dt.Rows[0]["CustomerName"].ToString();
            txtproductname.Text = Dt.Rows[0]["ProductName"].ToString();
            txtEngiName.Text = Dt.Rows[0]["EngiName"].ToString();
        }

        try
        {
            ///bind grid in automatic in customer po table.
            con.Open();
            DataTable SDt = new DataTable();
            //SqlDataAdapter SDA = new SqlDataAdapter("SELECT JobNo,[Description],[Hsn_Sac],[Rate],[Unit],[Quantity],[TaxPercenteage],[DiscountPercentage],[Total] FROM vw_Tax_Invoice WHERE Pono='" + txt_PoNo.Text + "'", con);
            SqlDataAdapter SDA = new SqlDataAdapter("SELECT id,Jobno,CompId,CompName,Quantity FROM [tblRepairingEvalution] WHERE JobNo='" + txttagno.Text + "'", con);
            SDA.Fill(SDt);
            int count = 1;
            if (SDt.Rows.Count > 0)
            {
                ViewState["RowNo"] = 0;
                Dt_Itemsdetails.Columns.AddRange(new DataColumn[5] { new DataColumn("id"), new DataColumn("Jobno"), new DataColumn("CompId"), new DataColumn("CompName"), new DataColumn("Quantity") });

                ViewState["Invoicedetails"] = Dt_Itemsdetails;
                for (int i = 0; i < SDt.Rows.Count; i++)
                {
                    Dt_Itemsdetails.Rows.Add(count, SDt.Rows[i]["Jobno"].ToString(), SDt.Rows[i]["CompId"].ToString(), SDt.Rows[i]["CompName"].ToString(), SDt.Rows[i]["Quantity"].ToString());

                    count = count + 1;
                }
            }

            gv_Comp.DataSource = Dt_Itemsdetails;
            gv_Comp.DataBind();
            gv_Comp.EmptyDataText = "Not Records Found";
            con.Close();

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public void GetJonNo()
    {
        DataTable Dt = new DataTable();
        SqlDataAdapter Daa = new SqlDataAdapter("SELECT JobNo  FROM [tblTestingProduct] where CustomerName = '" + txtcustomername.Text + "'", con);
        Daa.Fill(Dt);
        if (Dt.Rows.Count > 0)
        {
            txttagno.DataSource = Dt;
            txttagno.DataTextField = "JobNo";
            txttagno.DataBind();
            txttagno.Items.Insert(0, new ListItem("--Select--", "0"));

        }
    }
    protected void txtcustomername_TextChanged(object sender, EventArgs e)
    {
        GetJonNo();
    }
    protected void txttagno_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable Dt = new DataTable();
        SqlDataAdapter Daa = new SqlDataAdapter("SELECT ProductName,EngiName  FROM [tblTestingProduct] where JobNo = '" + txttagno.Text + "'", con);
        Daa.Fill(Dt);
        if (Dt.Rows.Count > 0)
        {
            if (Dt.Rows[0]["ProductName"].ToString() != "")
            {
                txtproductname.Text = Dt.Rows[0]["ProductName"].ToString();
            }

            if (Dt.Rows[0]["EngiName"].ToString() != "")
            {
                txtEngiName.Text = Dt.Rows[0]["EngiName"].ToString();
            }

            string jobno = txttagno.Text;
            ViewState["jobno"] = jobno;
            Getcomponent(jobno);

        }


    }
    public void Getcomponent(string jobno)
    {

        SqlCommand command = new SqlCommand();
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataSet ds = new DataSet();
        command.Connection = con;
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = "SP_IssuedComponetDtls";
        command.Parameters.AddWithValue("@Action", "GetIssuedcomponent");
        command.Parameters.AddWithValue("@JobNo", jobno);
        adapter = new SqlDataAdapter();
        adapter.SelectCommand = command;
        adapter.Fill(ds);
        Gv_CompNew.DataSource = ds.Tables[0];
        Gv_CompNew.DataBind();
    }



    protected void lnkbtnDeletenew_Click(object sender, EventArgs e)
    {
        loadGriddata();
        //DataTable dtcomponant = new DataTable();
        //ViewState["RowNo"] = 0;
        //dtcomponant.Columns.AddRange(new DataColumn[5] { new DataColumn("No"),new DataColumn("Jobno"), new DataColumn("CompId"),
        //new DataColumn("CompName"),new DataColumn("Quantity")});
        //ViewState["CompData"] = dtcomponant;
        GridViewRow row = (sender as LinkButton).NamingContainer as GridViewRow;
        //ViewState["RowNo"] = (int)ViewState["RowNo"] + 1;
        //DataTable dt = (DataTable)ViewState["CompData"];
        //DataRow dr2 = dt.NewRow();
        DataTable dt = ViewState["Loadadta"] as DataTable;
        if (dt.Rows.Count > 0)
        {
            dt.Rows.Remove(dt.Rows[row.RowIndex]);
            ViewState["Loadadta"] = dt;
            Gv_CompNew.DataSource = (DataTable)ViewState["Loadadta"];
            Gv_CompNew.DataBind();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Data Delete Successfully !!!');", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Technical Problem Occured !!!');", true);
        }
    }

    public void loadGriddata()
    {
        string jobno = ViewState["jobno"].ToString();
        SqlDataAdapter Da = new SqlDataAdapter("select JobNo,CompId,CompName ,Quantity FROM tblRepairingEvalution WHERE [JobNo]='" + jobno + "'", con);
        DataTable Dt = new DataTable();
        Da.Fill(Dt);
        if (Dt.Rows.Count > 0)
        {
            gv_Comp.DataSource = Dt;
            ViewState["Loadadta"] = Dt;
        }

    }

    public void GetissuedcomponetforUpdate(string jobno)
    {
        DvUpdatecomp.Visible = true;
        SqlCommand command = new SqlCommand();
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataSet ds = new DataSet();
        command.Connection = con;
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = "SP_IssuedComponetDtls";
        command.Parameters.AddWithValue("@Action", "GetIsuucomdtlsforUpdate");
        command.Parameters.AddWithValue("@JobNo", jobno);
        adapter = new SqlDataAdapter();
        adapter.SelectCommand = command;
        adapter.Fill(ds);
        GVUpdatecomp.DataSource = ds.Tables[0];
        GVUpdatecomp.DataBind();
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (ViewState["ID"].ToString() != null)
        {
            con.Open();
            SqlCommand Cmd = new SqlCommand("SP_IssuedComponetDtls", con);
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.Parameters.AddWithValue("@Action", "UpdateIssuedHrd");
            Cmd.Parameters.AddWithValue("@JobNo", txttagno.Text.Trim());
            Cmd.Parameters.AddWithValue("@CustomerName", txtcustomername.Text.Trim());
            Cmd.Parameters.AddWithValue("@ProductName", txtproductname.Text.Trim());
            Cmd.Parameters.AddWithValue("@EngineerName", txtEngiName.Text.Trim());
            // Cmd.Parameters.AddWithValue("@UpdatedBy", Session["Username"].ToString());
            Cmd.ExecuteNonQuery();
            if (Gv_CompNew.Rows.Count > 0)
            {
                foreach (GridViewRow grd1 in Gv_CompNew.Rows)
                {
                    string lblcompid = (grd1.FindControl("lblcompid") as Label).Text;
                    string lblCompnamee = (grd1.FindControl("lblCompnamee") as Label).Text;
                    string lblQty = (grd1.FindControl("lblQty") as Label).Text;
                    string lblJobno = (grd1.FindControl("lblJobno") as Label).Text;
                    SqlCommand cmd = new SqlCommand("SP_IssuedComponetDtls", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CompId", lblcompid);
                    cmd.Parameters.AddWithValue("@CompName", lblCompnamee);
                    cmd.Parameters.AddWithValue("@Quantity", lblQty);
                    cmd.Parameters.AddWithValue("@JobNo", lblJobno);
                    cmd.Parameters.AddWithValue("@Action", "UpdateIssuedDtls");
                    cmd.ExecuteNonQuery();
                }
            }

            if (gv_Comp.Rows.Count > 0)
            {
                foreach (GridViewRow grd1 in gv_Comp.Rows)
                {
                    string lblcompid = (grd1.FindControl("lblcompid") as Label).Text;
                    string lblCompnamee = (grd1.FindControl("lblCompnamee") as Label).Text;
                    string lblQty = (grd1.FindControl("lblQty") as Label).Text;
                    string lblJobno = (grd1.FindControl("lblJobno") as Label).Text;
                    SqlCommand cmd = new SqlCommand("SP_IssuedComponetDtls", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CompId", lblcompid);
                    cmd.Parameters.AddWithValue("@CompName", lblCompnamee);
                    cmd.Parameters.AddWithValue("@Quantity", lblQty);
                    cmd.Parameters.AddWithValue("@JobNo", lblJobno);
                    cmd.Parameters.AddWithValue("@Action", "UpdateIssuedDtls");
                    cmd.ExecuteNonQuery();
                }
            }

            con.Close();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Updated Successfully !!!');", true);
            Response.Redirect("IssuedComponentList.aspx");
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Technical Problem Occured !!!');", true);
            Response.Redirect("IssuedComponentList.aspx");
        }
    }


    protected void GVUpdatecomp_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Deletecomp")
        {
            con.Open();
            int index = Convert.ToInt32(e.CommandArgument);
            string jobno = (string)this.GVUpdatecomp.DataKeys[index]["JobNo"];
            string compNme = (string)this.GVUpdatecomp.DataKeys[index]["CompName"];
            string CompID = (string)this.GVUpdatecomp.DataKeys[index]["CompId"];
            string Quantity = (string)this.GVUpdatecomp.DataKeys[index]["Quantity"];
            SqlCommand cmd = new SqlCommand("SP_IssuedComponetDtls", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CompId", CompID);
            cmd.Parameters.AddWithValue("@CompName", compNme);
            cmd.Parameters.AddWithValue("@Quantity", Quantity);
            cmd.Parameters.AddWithValue("@JobNo", jobno);
            cmd.Parameters.AddWithValue("@Action", "Deleteissuedcompnentforlist");
            cmd.ExecuteNonQuery();
            con.Close();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Deleted Successfully !!!');", true);
            GetissuedcomponetforUpdate(jobno);
        }
    }
}


