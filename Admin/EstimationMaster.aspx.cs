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

public partial class Admin_EstimationMaster : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
    string txt;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["adminname"] == null)
            {
                Response.Redirect("../LoginPage.aspx");
            }
            string role = Session["adminname"].ToString();

            if (role == "Purchase")
            {
                divCustName.Visible = false;
                divtxtestimatedquo.Visible = false;
                // This hides the div containing Customer Name
            }

            //this.txtcomprecdate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            //this.txtcomprecdate.TextMode = TextBoxMode.Date;

            if (Request.QueryString["JobNo"] != null)
            {
                string id = Decrypt(Request.QueryString["JobNo"].ToString());
                loadData(id);
                btnSubmit.Text = "Update";
                gv_Updateestimat.Visible = true;
                txtJobNo.ReadOnly = true;
                hidden.Value = id;
            }
        }
    }

    private DataTable GetDataTable(string query)
    {
        string conString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(conString))
        {
            using (SqlCommand cmd = new SqlCommand(query))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    cmd.Connection = con;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        return dt;
                    }
                }
            }
        }
    }

    protected void BindVendorList1(DropDownList txtvendor1header)
    {
        try
        {
            txtvendor1header.DataSource = GetDataTable("SELECT * FROM tblVendor");
            txtvendor1header.DataTextField = "VendorName";
            txtvendor1header.DataBind();
            txtvendor1header.Items.Insert(0, new ListItem("--Select Vendor--", ""));
        }

        catch (Exception)
        {
            throw;
        }
    }

    protected void BindVendorList2(DropDownList txtvendor2header)
    {
        try
        {
            txtvendor2header.DataSource = GetDataTable("SELECT * FROM tblVendor");
            txtvendor2header.DataTextField = "VendorName";
            txtvendor2header.DataBind();
            txtvendor2header.Items.Insert(0, new ListItem("--Select Vendor--", ""));
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void BindVendorList3(DropDownList txtvendor3header)
    {
        try
        {
            txtvendor3header.DataSource = GetDataTable("SELECT * FROM tblVendor");
            txtvendor3header.DataTextField = "VendorName";
            txtvendor3header.DataBind();
            txtvendor3header.Items.Insert(0, new ListItem("--Select Vendor--", ""));
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void BindVendorList4(DropDownList txtvendor4header)
    {
        try
        {
            txtvendor4header.DataSource = GetDataTable("SELECT * FROM tblVendor");
            txtvendor4header.DataTextField = "VendorName";
            txtvendor4header.DataBind();
            txtvendor4header.Items.Insert(0, new ListItem("--Select Vendor--", ""));
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void BindVendorList5(DropDownList txtvendor5header)
    {
        try
        {
            txtvendor5header.DataSource = GetDataTable("SELECT * FROM tblVendor");
            txtvendor5header.DataTextField = "VendorName";
            txtvendor5header.DataBind();
            txtvendor5header.Items.Insert(0, new ListItem("--Select Vendor--", ""));
        }
        catch (Exception)
        {
            throw;
        }
    }



    decimal total1vendor1upd, total1vendor2upd, total1vendor3upd, total1vendor4upd, total1vendor5upd, Qtyupdate, vender1update, vender2update, vender3update, vender4update, vender5update = 0;
    protected void loadData(string id)
    {
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("Select * from tblEstimationHdr where JobNo='" + id + "' ", con);

        sad.Fill(dt);


        if (dt.Rows.Count > 0)
        {

            txtJobNo.Text = dt.Rows[0]["JobNo"].ToString();
            txtcustname.Text = dt.Rows[0]["CustName"].ToString();
            txtproduct.Text = dt.Rows[0]["productname"].ToString();
            txtmodelno.Text = dt.Rows[0]["modelno"].ToString();
            txtFinalStatus.Text = dt.Rows[0]["FinalStatus"].ToString();
            txtfinalcost.Text = dt.Rows[0]["FinalCost"].ToString();

            txtcomponentcost.Text = dt.Rows[0]["componetcost"].ToString();
            txtsitevisitcharges.Text = dt.Rows[0]["SiteVisitCharges"].ToString();
            txtothercharges.Text = dt.Rows[0]["OtherCharges"].ToString();
            txtfinalcost.Text = dt.Rows[0]["FinalCost"].ToString();
            txtestimatedquo.Text = dt.Rows[0]["EstimatedQuotation"].ToString();
            txtcompstatus.Text = dt.Rows[0]["Componetstatus"].ToString();

            DateTime ffff4 = Convert.ToDateTime(dt.Rows[0]["CompRecDate"].ToString());
            DateTime defaultDate = new DateTime(1900, 1, 1);

            if (ffff4 != defaultDate)
            {
                txtcomprecdate.Text = ffff4.ToString("yyyy-MM-dd");
            }

            lblTotal.Text = "Total Cost of Component : " + txtcomponentcost.Text;


            // cmd1.Parameters.AddWithValue("@componetcost", txtcomponentcost.Text);   //Component cost
            // cmd1.Parameters.AddWithValue("@SiteVisitCharges", txtsitevisitcharges.Text);   //Site visit charges 
            // cmd1.Parameters.AddWithValue("@OtherCharges", txtothercharges.Text);           //Other charges
            // cmd1.Parameters.AddWithValue("@FinalCost", txtfinalcost.Text);              //Final Cost
            // cmd1.Parameters.AddWithValue("@EstimatedQuotation", txtestimatedquo.Text);   //Quotation Amount

        }

        DataTable dt1 = new DataTable();
        SqlDataAdapter sad1 = new SqlDataAdapter("Select * from vw_EstimationLoaddata where JobNo='" + id + "' ", con);

        sad1.Fill(dt1);
        header1.Value = dt1.Rows[0]["VendorName1"].ToString();
        header2.Value = dt1.Rows[0]["VendorName2"].ToString();
        header3.Value = dt1.Rows[0]["VendorName3"].ToString();
        header4.Value = dt1.Rows[0]["VendorName4"].ToString();
        header5.Value = dt1.Rows[0]["VendorName5"].ToString();
        gv_Updateestimat.DataSource = dt1;
        gv_Updateestimat.DataBind();

        foreach (GridViewRow gv in gv_Updateestimat.Rows)
        {
            Label Qty = (Label)gv.FindControl("lblqty");
            TextBox vender1 = (TextBox)gv.FindControl("txtvendor1");
            TextBox vender2 = (TextBox)gv.FindControl("txtvendor2");
            TextBox vender3 = (TextBox)gv.FindControl("txtvendor3");
            TextBox vender4 = (TextBox)gv.FindControl("txtvendor4");
            TextBox vender5 = (TextBox)gv.FindControl("txtvendor5");

            Qtyupdate = Convert.ToDecimal(Qty.Text);
            vender1update = Convert.ToDecimal(vender1.Text);
            vender2update = Convert.ToDecimal(vender2.Text);
            vender3update = Convert.ToDecimal(vender3.Text);
            vender4update = Convert.ToDecimal(vender4.Text);
            vender5update = Convert.ToDecimal(vender5.Text);

            total1vendor1upd += Qtyupdate * vender1update;
            total1vendor2upd += Qtyupdate * vender2update;
            total1vendor3upd += Qtyupdate * vender3update;
            total1vendor4upd += Qtyupdate * vender4update;
            total1vendor5upd += Qtyupdate * vender5update;

        }
        Label footervender1upd = ((Label)gv_Updateestimat.FooterRow.FindControl("lblvendortotal1"));
        footervender1upd.Text = "" + total1vendor1upd;
        Label footervender2upd = ((Label)gv_Updateestimat.FooterRow.FindControl("lblvendortotal2"));
        footervender2upd.Text = "" + total1vendor2upd;
        Label footervender3upd = ((Label)gv_Updateestimat.FooterRow.FindControl("lblvendortotal3"));
        footervender3upd.Text = "" + total1vendor3upd;
        Label footervender4upd = ((Label)gv_Updateestimat.FooterRow.FindControl("lblvendortotal4"));
        footervender4upd.Text = "" + total1vendor4upd;
        Label footervender5upd = ((Label)gv_Updateestimat.FooterRow.FindControl("lblvendortotal5"));
        footervender5upd.Text = "" + total1vendor5upd;
    }

    protected void txtJobNo_TextChanged(object sender, EventArgs e)
    {
        try
        {
            // Check if JobNo exists in tblEstimationHdr table
            SqlCommand cmd = new SqlCommand("SELECT COUNT(1) FROM tblEstimationHdr WHERE JobNo = @JobNo", con);
            cmd.Parameters.AddWithValue("@JobNo", txtJobNo.Text);
            con.Open();
            int count = (int)cmd.ExecuteScalar();
            con.Close();

            if (count > 0)
            {
                // Display message if JobNo already exists
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('This Job No already exists in Estimation.');", true);
                return; // Exit the method early if job number already exists
            }

            DataTable dt = new DataTable();
            SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM vw_Estimation WHERE JobNo = @JobNo", con);
            sad.SelectCommand.Parameters.AddWithValue("@JobNo", txtJobNo.Text);
            sad.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                txtcustname.Text = dt.Rows[0]["CustomerName"].ToString();
                gv_estimation.DataSource = dt;
                gv_estimation.EmptyDataText = "Record Not Found";
                gv_estimation.DataBind();
            }

            DataTable dtt = new DataTable();
            SqlDataAdapter sadd = new SqlDataAdapter("SELECT * FROM [tblInwardEntry] WHERE JobNo = @JobNo", con);
            sadd.SelectCommand.Parameters.AddWithValue("@JobNo", txtJobNo.Text);
            sadd.Fill(dtt);
            if (dtt.Rows.Count > 0)
            {
                txtmodelno.Text = dtt.Rows[0]["ModelNo"].ToString();
                txtproduct.Text = dtt.Rows[0]["MateName"].ToString();
            }
        }
        catch (Exception ex)
        {
            // Handle exception and display error message
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error: " + ex.Message + "');", true);
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }



        //try
        //{
        //    DataTable dt = new DataTable();
        //    SqlDataAdapter sad = new SqlDataAdapter(" select * from vw_Estimation where JobNo='" + txtJobNo.Text + "'", con);
        //    sad.Fill(dt);
        //    if (dt.Rows.Count > 0)
        //    {
        //        txtcustname.Text = dt.Rows[0]["CustomerName"].ToString();
        //        gv_estimation.DataSource = dt;
        //        gv_estimation.EmptyDataText = "Record Not Found";
        //        gv_estimation.DataBind();
        //    }

        //    DataTable dtt = new DataTable();
        //    SqlDataAdapter sadd = new SqlDataAdapter("select * from [tblInwardEntry] where JobNo='" + txtJobNo.Text + "'", con);
        //    sadd.Fill(dtt);
        //    if (dt.Rows.Count > 0)
        //    {             
        //        txtmodelno.Text = dtt.Rows[0]["ModelNo"].ToString();
        //        txtproduct.Text = dtt.Rows[0]["MateName"].ToString();
        //    }
        //}
        //catch (Exception)
        //{
        //    throw;
        //}
    }

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetjobList(string prefixText, int count)
    {
        return AutoFilljoblist(prefixText);
    }

    public static List<string> AutoFilljoblist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT JobNo from tblTestingProduct where " + "JobNo like @Search + '%' AND isdeleted='0' AND isCompleted='1'";

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

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string createdby = Session["adminname"].ToString();
        DateTime date = DateTime.Today;
        if (btnSubmit.Text == "Submit")
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from tblEstimationHdr where JobNo='" + txtJobNo.Text + "' AND isdeleted != '1' ", con);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Data Already Exist.');window.location ='EstimationMaster.aspx';", true);
            }
            else
            {

                string vendor1header = (gv_estimation.HeaderRow.FindControl("txtvendor1header") as DropDownList).Text;




                string vendor2header = (gv_estimation.HeaderRow.FindControl("txtvendor2header") as DropDownList).Text;
                string vendor3header = (gv_estimation.HeaderRow.FindControl("txtvendor3header") as DropDownList).Text;
                string vendor4header = (gv_estimation.HeaderRow.FindControl("txtvendor4header") as DropDownList).Text;
                string vendor5header = (gv_estimation.HeaderRow.FindControl("txtvendor5header") as DropDownList).Text;

                if (vendor1header != "" && vendor2header != "" && vendor3header != "" && vendor4header != "" && vendor5header != "")
                {
                    con.Close();
                    SqlCommand cmd1 = new SqlCommand("SP_EstimastionHdr", con);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@JobNo", txtJobNo.Text);
                    cmd1.Parameters.AddWithValue("@CustName", txtcustname.Text);
                    cmd1.Parameters.AddWithValue("@productname", txtproduct.Text);
                    cmd1.Parameters.AddWithValue("@modelno", txtmodelno.Text);
                    cmd1.Parameters.AddWithValue("@FinalStatus", txtFinalStatus.Text);


                    cmd1.Parameters.AddWithValue("@componetcost", txtcomponentcost.Text);   //Component cost
                    cmd1.Parameters.AddWithValue("@SiteVisitCharges", txtsitevisitcharges.Text);   //Site visit charges 
                    cmd1.Parameters.AddWithValue("@OtherCharges", txtothercharges.Text);           //Other charges
                    cmd1.Parameters.AddWithValue("@FinalCost", txtfinalcost.Text);              //Final Cost
                    cmd1.Parameters.AddWithValue("@EstimatedQuotation", txtestimatedquo.Text);   //Quotation Amount

                    cmd1.Parameters.AddWithValue("@Componetstatus", txtcompstatus.Text);
                    cmd1.Parameters.AddWithValue("@CompRecDate", txtcomprecdate.Text);
                    cmd1.Parameters.AddWithValue("@CreatedBy", createdby);
                    cmd1.Parameters.AddWithValue("@CreatedDate", date);
                    cmd1.Parameters.AddWithValue("@isdeleted", '0');


                    cmd1.Parameters.AddWithValue("@VendorName1", vendor1header);
                    cmd1.Parameters.AddWithValue("@VendorName2", vendor2header);
                    cmd1.Parameters.AddWithValue("@VendorName3", vendor3header);
                    cmd1.Parameters.AddWithValue("@VendorName4", vendor4header);
                    cmd1.Parameters.AddWithValue("@VendorName5", vendor5header);

                    cmd1.Parameters.AddWithValue("@Action", "Insert");
                    con.Open();
                   cmd1.ExecuteNonQuery();
                    con.Close();

                    foreach (GridViewRow g1 in gv_estimation.Rows)
                    {

                        string Compname = (g1.FindControl("lblCompName") as Label).Text;
                        string qty = (g1.FindControl("lblqty") as Label).Text;
                        string vendor1 = (g1.FindControl("txtvendor1") as TextBox).Text;
                        string vendor2 = (g1.FindControl("txtvendor2") as TextBox).Text;
                        string vendor3 = (g1.FindControl("txtvendor3") as TextBox).Text;
                        string vendor4 = (g1.FindControl("txtvendor4") as TextBox).Text;
                        string vendor5 = (g1.FindControl("txtvendor5") as TextBox).Text;
                        string status = (g1.FindControl("txtstatus") as TextBox).Text;


                        SqlCommand cmd2 = new SqlCommand("INSERT INTO tblEstimationDtls(JobNo,CompName,Quantity,vendor1Rate,vendor2Rate,vendor3Rate,vendor4Rate,vendor5Rate,status)values('" + txtJobNo.Text + "','" + Compname + "','" + qty + "','" + vendor1 + "','" + vendor2 + "','" + vendor3 + "','" + vendor4 + "','" + vendor5 + "','" + status + "')", con);
                        con.Open();
                        cmd2.ExecuteNonQuery();
                        con.Close();


                    }
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Saved Successfully');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Vendor');", true);
                }


            }
        }
        else if (btnSubmit.Text == "Update")
        {
            string Id="";
            decimal FinalCost= 0;
            string QuotationStatus = "";
            string JobDaysCount="";

            con.Open();
            SqlCommand cmdupdate1 = new SqlCommand("select [id],[FinalCost],[QuotationStatus],[JobDaysCount] from tblEstimationHdr WHERE JobNo='" + txtJobNo.Text + "'", con);
            SqlDataReader reader = cmdupdate1.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                 Id = reader["id"].ToString();
                 FinalCost = Convert.ToDecimal(reader["FinalCost"].ToString());
                 QuotationStatus = reader["QuotationStatus"].ToString();
                 JobDaysCount = reader["JobDaysCount"].ToString();
                reader.Close();
            }

                con.Close();
            SqlCommand cmd1 = new SqlCommand("SP_EstimastionHdr", con);
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@JobNo", txtJobNo.Text);
            cmd1.Parameters.AddWithValue("@CustName", txtcustname.Text);
            cmd1.Parameters.AddWithValue("@productname", txtproduct.Text);
            cmd1.Parameters.AddWithValue("@modelno", txtmodelno.Text);
            cmd1.Parameters.AddWithValue("@FinalStatus", txtFinalStatus.Text);
            cmd1.Parameters.AddWithValue("@componetcost", txtcomponentcost.Text);   //Component cost
            cmd1.Parameters.AddWithValue("@SiteVisitCharges", txtsitevisitcharges.Text);   //Site visit charges 
            cmd1.Parameters.AddWithValue("@OtherCharges", txtothercharges.Text);           //Other charges
            cmd1.Parameters.AddWithValue("@FinalCost", txtfinalcost.Text);              //Final Cost
            cmd1.Parameters.AddWithValue("@EstimatedQuotation", txtestimatedquo.Text);   //Quotation Amount
            cmd1.Parameters.AddWithValue("@Componetstatus", txtcompstatus.Text);
            cmd1.Parameters.AddWithValue("@CompRecDate", txtcomprecdate.Text);
            cmd1.Parameters.AddWithValue("@CreatedBy", null);
            cmd1.Parameters.AddWithValue("@CreatedDate", null);
            cmd1.Parameters.AddWithValue("@UpdateBy", createdby);
            cmd1.Parameters.AddWithValue("@UpdatedDate", date);
            cmd1.Parameters.AddWithValue("@isdeleted", '0');

            string vendor1header = (gv_Updateestimat.HeaderRow.FindControl("txtvendor1header") as DropDownList).Text;
            string vendor2header = (gv_Updateestimat.HeaderRow.FindControl("txtvendor2header") as DropDownList).Text;
            string vendor3header = (gv_Updateestimat.HeaderRow.FindControl("txtvendor3header") as DropDownList).Text;
            string vendor4header = (gv_Updateestimat.HeaderRow.FindControl("txtvendor4header") as DropDownList).Text;
            string vendor5header = (gv_Updateestimat.HeaderRow.FindControl("txtvendor5header") as DropDownList).Text;
            cmd1.Parameters.AddWithValue("@VendorName1", vendor1header);
            cmd1.Parameters.AddWithValue("@VendorName2", vendor2header);
            cmd1.Parameters.AddWithValue("@VendorName3", vendor3header);
            cmd1.Parameters.AddWithValue("@VendorName4", vendor4header);
            cmd1.Parameters.AddWithValue("@VendorName5", vendor5header);

            cmd1.Parameters.AddWithValue("@Action", "Update");
            con.Open();
            cmd1.ExecuteNonQuery();
            con.Close();

            foreach (GridViewRow g1 in gv_Updateestimat.Rows)
            {

                string Compname = (g1.FindControl("lblCompName") as Label).Text;
                string qty = (g1.FindControl("lblqty") as Label).Text;
                string vendor1 = (g1.FindControl("txtvendor1") as TextBox).Text;
                string vendor2 = (g1.FindControl("txtvendor2") as TextBox).Text;
                string vendor3 = (g1.FindControl("txtvendor3") as TextBox).Text;
                string vendor4 = (g1.FindControl("txtvendor4") as TextBox).Text;
                string vendor5 = (g1.FindControl("txtvendor5") as TextBox).Text;
                string status = (g1.FindControl("txtstatus") as TextBox).Text;

                SqlCommand cmd2 = new SqlCommand("Update tblEstimationDtls set status='" + status + "',vendor1Rate='" + vendor1 + "',vendor2Rate='" + vendor2 + "',vendor3Rate='" + vendor3 + "',vendor4Rate='" + vendor4 + "',vendor5Rate='" + vendor5 + "' where JobNo='" + txtJobNo.Text + "' AND CompName='" + Compname + "'", con);
                con.Open();
                cmd2.ExecuteNonQuery();
                con.Close();
            }

            if(FinalCost != Convert.ToDecimal(txtfinalcost.Text))
            {
                con.Close();
                SqlCommand cmdds = new SqlCommand("UPDATE tblEstimationHdr SET QuotationStatus = 'Pending', JobDaysCount = 0  WHERE id = '" + Id + "'", con);

                con.Open();
                cmdds.ExecuteNonQuery();
                con.Close();
            }
            else
            {
                con.Close();
                SqlCommand cmdds = new SqlCommand("UPDATE tblEstimationHdr SET QuotationStatus = '"+QuotationStatus+"', JobDaysCount = '" + JobDaysCount + "' WHERE id = '" + Id + "'", con);

                con.Open();
                cmdds.ExecuteNonQuery();
                con.Close();
            }

            ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Updated Successfully');", true);

        }
    }

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetstatusList(string prefixText, int count)
    {
        return AutoFillstatuslist(prefixText);
    }

    public static List<string> AutoFillstatuslist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT status from tblEstimationDtls where " + "status like @Search + '%'";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> status = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        status.Add(sdr["status"].ToString());
                    }
                }
                con.Close();
                return status;
            }

        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("EstimationList.aspx");
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

    decimal total, vendor1total, vendor2total, vendor3total, vendor4total, Qty, lblvendorrate1, vendor5total = 0;

    protected void gv_Updateestimat_RowDataBound1(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType != DataControlRowType.Header)
        {


            DropDownList TXB1 = (DropDownList)gv_Updateestimat.HeaderRow.FindControl("txtvendor1header");
            TXB1.Text = header1.Value;
            DropDownList TXB2 = (DropDownList)gv_Updateestimat.HeaderRow.FindControl("txtvendor2header");
            TXB2.Text = header2.Value;
            DropDownList TXB3 = (DropDownList)gv_Updateestimat.HeaderRow.FindControl("txtvendor3header");
            TXB3.Text = header3.Value;
            DropDownList TXB4 = (DropDownList)gv_Updateestimat.HeaderRow.FindControl("txtvendor4header");
            TXB4.Text = header4.Value;
            DropDownList TXB5 = (DropDownList)gv_Updateestimat.HeaderRow.FindControl("txtvendor5header");
            TXB5.Text = header5.Value;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //DropDownList ddlvendorlistUpdate1 = (DropDownList)gv_estimation.HeaderRow.FindControl("txtvendor1header");
            //BindVendorList1update(ddlvendorlistUpdate1);
            //DropDownList ddlvendorlistUpdate2 = (DropDownList)gv_estimation.HeaderRow.FindControl("txtvendor2header");
            //BindVendorList2update(ddlvendorlistUpdate2);
            //DropDownList ddlvendorlistUpdate3 = (DropDownList)gv_estimation.HeaderRow.FindControl("txtvendor3header");
            //BindVendorList3update(ddlvendorlistUpdate3);
            //DropDownList ddlvendorlistUpdate4 = (DropDownList)gv_estimation.HeaderRow.FindControl("txtvendor4header");
            //BindVendorList4update(ddlvendorlistUpdate4);
            //DropDownList ddlvendorlistUpdate5 = (DropDownList)gv_estimation.HeaderRow.FindControl("txtvendor5header");
            //BindVendorList5update(ddlvendorlistUpdate5); 


            DropDownList ddlvendorlist1 = (DropDownList)gv_Updateestimat.HeaderRow.FindControl("txtvendor1header");
            BindVendorList1(ddlvendorlist1);
            DropDownList ddlvendorlist2 = (DropDownList)gv_Updateestimat.HeaderRow.FindControl("txtvendor2header");
            BindVendorList2(ddlvendorlist2);
            DropDownList ddlvendorlist3 = (DropDownList)gv_Updateestimat.HeaderRow.FindControl("txtvendor3header");
            BindVendorList3(ddlvendorlist3);
            DropDownList ddlvendorlist4 = (DropDownList)gv_Updateestimat.HeaderRow.FindControl("txtvendor4header");
            BindVendorList4(ddlvendorlist4);
            DropDownList ddlvendorlist5 = (DropDownList)gv_Updateestimat.HeaderRow.FindControl("txtvendor5header");
            BindVendorList5(ddlvendorlist5);

        }
    }

    protected void gv_estimation_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblqtye = (e.Row.FindControl("lblqty") as Label);
            total += Convert.ToDecimal(lblqtye.Text);
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblTotal = (e.Row.FindControl("lblqtytotal") as Label);
            lblTotal.Text = " " + total.ToString();
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList ddlvendorlist1 = (DropDownList)gv_estimation.HeaderRow.FindControl("txtvendor1header");
            BindVendorList1(ddlvendorlist1);
            DropDownList ddlvendorlist2 = (DropDownList)gv_estimation.HeaderRow.FindControl("txtvendor2header");
            BindVendorList2(ddlvendorlist2);
            DropDownList ddlvendorlist3 = (DropDownList)gv_estimation.HeaderRow.FindControl("txtvendor3header");
            BindVendorList3(ddlvendorlist3);
            DropDownList ddlvendorlist4 = (DropDownList)gv_estimation.HeaderRow.FindControl("txtvendor4header");
            BindVendorList4(ddlvendorlist4);
            DropDownList ddlvendorlist5 = (DropDownList)gv_estimation.HeaderRow.FindControl("txtvendor5header");
            BindVendorList5(ddlvendorlist5);
        }
    }

    decimal total1vendor1, total1vendor2, total1vendor3, total1vendor4, total1vendor5, Qty1, vender11, vender22, vender33, vender44, vender55, footervender1 = 0;

    protected void btntotal_Click(object sender, EventArgs e)
    {

        foreach (GridViewRow gv in gv_estimation.Rows)
        {
            Label Qty = (Label)gv.FindControl("lblqty");
            TextBox vender1 = (TextBox)gv.FindControl("txtvendor1");
            TextBox vender2 = (TextBox)gv.FindControl("txtvendor2");
            TextBox vender3 = (TextBox)gv.FindControl("txtvendor3");
            TextBox vender4 = (TextBox)gv.FindControl("txtvendor4");
            TextBox vender5 = (TextBox)gv.FindControl("txtvendor5");

            Qty1 = Convert.ToDecimal(Qty.Text);
            vender11 = Convert.ToDecimal(vender1.Text);
            vender22 = Convert.ToDecimal(vender2.Text);
            vender33 = Convert.ToDecimal(vender3.Text);
            vender44 = Convert.ToDecimal(vender4.Text);
            vender55 = Convert.ToDecimal(vender5.Text);

            total1vendor1 += Qty1 * vender11;
            total1vendor2 += Qty1 * vender22;
            total1vendor3 += Qty1 * vender33;
            total1vendor4 += Qty1 * vender44;
            total1vendor5 += Qty1 * vender55;

        }

        Label footervender1 = ((Label)gv_estimation.FooterRow.FindControl("lblvendortotal1"));
        footervender1.Text = "" + total1vendor1;
        Label footervender2 = ((Label)gv_estimation.FooterRow.FindControl("lblvendortotal2"));
        footervender2.Text = "" + total1vendor2;
        Label footervender3 = ((Label)gv_estimation.FooterRow.FindControl("lblvendortotal3"));
        footervender3.Text = "" + total1vendor3;
        Label footervender4 = ((Label)gv_estimation.FooterRow.FindControl("lblvendortotal4"));
        footervender4.Text = "" + total1vendor4;
        Label footervender5 = ((Label)gv_estimation.FooterRow.FindControl("lblvendortotal5"));
        footervender5.Text = "" + total1vendor5;

        var TotalCost = total1vendor1 + total1vendor2 + total1vendor3 + total1vendor4 + total1vendor5;
        txtcomponentcost.Text = TotalCost.ToString();

        lblTotal.Text = "Total Cost of Component : " + txtcomponentcost.Text;
    }

    protected void txtvendor1_TextChanged(object sender, EventArgs e)
    {

    }

    protected void btntotal_Click1(object sender, EventArgs e)
    {
        foreach (GridViewRow gv in gv_Updateestimat.Rows)
        {
            Label Qty = (Label)gv.FindControl("lblqty");
            TextBox vender1 = (TextBox)gv.FindControl("txtvendor1");
            TextBox vender2 = (TextBox)gv.FindControl("txtvendor2");
            TextBox vender3 = (TextBox)gv.FindControl("txtvendor3");
            TextBox vender4 = (TextBox)gv.FindControl("txtvendor4");
            TextBox vender5 = (TextBox)gv.FindControl("txtvendor5");

            Qtyupdate = Convert.ToDecimal(Qty.Text);
            vender1update = Convert.ToDecimal(vender1.Text);
            vender2update = Convert.ToDecimal(vender2.Text);
            vender3update = Convert.ToDecimal(vender3.Text);
            vender4update = Convert.ToDecimal(vender4.Text);
            vender5update = Convert.ToDecimal(vender5.Text);

            total1vendor1upd += Qtyupdate * vender1update;
            total1vendor2upd += Qtyupdate * vender2update;
            total1vendor3upd += Qtyupdate * vender3update;
            total1vendor4upd += Qtyupdate * vender4update;
            total1vendor5upd += Qtyupdate * vender5update;

        }
        Label footervender1upd = ((Label)gv_Updateestimat.FooterRow.FindControl("lblvendortotal1"));
        footervender1upd.Text = "" + total1vendor1upd;
        Label footervender2upd = ((Label)gv_Updateestimat.FooterRow.FindControl("lblvendortotal2"));
        footervender2upd.Text = "" + total1vendor2upd;
        Label footervender3upd = ((Label)gv_Updateestimat.FooterRow.FindControl("lblvendortotal3"));
        footervender3upd.Text = "" + total1vendor3upd;
        Label footervender4upd = ((Label)gv_Updateestimat.FooterRow.FindControl("lblvendortotal4"));
        footervender4upd.Text = "" + total1vendor4upd;
        Label footervender5upd = ((Label)gv_Updateestimat.FooterRow.FindControl("lblvendortotal5"));
        footervender5upd.Text = "" + total1vendor5upd;

        var TotalCost = total1vendor1upd + total1vendor2upd + total1vendor3upd + total1vendor4upd + total1vendor5upd;
        txtcomponentcost.Text = TotalCost.ToString();

        lblTotal.Text = "Total Cost of Component : " + txtcomponentcost.Text;
    }



    protected void txtothercharges_TextChanged(object sender, EventArgs e)
    {
        //  var Total = txtcomponentcost.Text + txtsitevisitcharges.Text + txtothercharges.Text;

        //  txtfinalcost.Text = Total;

        // Initialize variables to store numerical values
        decimal componentCost = 0;
        decimal siteVisitCharges = 0;
        decimal otherCharges = 0;

        // Try parsing the text values to decimal
        decimal.TryParse(txtcomponentcost.Text, out componentCost);
        decimal.TryParse(txtsitevisitcharges.Text, out siteVisitCharges);
        decimal.TryParse(txtothercharges.Text, out otherCharges);

        // Calculate the total
        var Total = componentCost + siteVisitCharges + otherCharges;

        // Assign the total to the final cost text box
        txtfinalcost.Text = Total.ToString("F2"); // Format to 2 decimal places
        txtestimatedquo.Text = Total.ToString("F2"); // Format to 2 decimal places
    }

    protected void txtsitevisitcharges_TextChanged(object sender, EventArgs e)
    {
        //  var Total = txtcomponentcost.Text + txtsitevisitcharges.Text + txtothercharges.Text;

        //  txtfinalcost.Text = Total;

        // Initialize variables to store numerical values
        decimal componentCost = 0;
        decimal siteVisitCharges = 0;
        decimal otherCharges = 0;

        // Try parsing the text values to decimal
        decimal.TryParse(txtcomponentcost.Text, out componentCost);
        decimal.TryParse(txtsitevisitcharges.Text, out siteVisitCharges);
        decimal.TryParse(txtothercharges.Text, out otherCharges);

        // Calculate the total
        var Total = componentCost + siteVisitCharges + otherCharges;

        // Assign the total to the final cost text box
        txtfinalcost.Text = Total.ToString("F2"); // Format to 2 decimal places
        txtestimatedquo.Text = Total.ToString("F2"); // Format to 2 decimal places
    }

    protected void txtcomponentcost_TextChanged(object sender, EventArgs e)
    {
        //  var Total = txtcomponentcost.Text + txtsitevisitcharges.Text + txtothercharges.Text;

        //  txtfinalcost.Text = Total;

        // Initialize variables to store numerical values
        decimal componentCost = 0;
        decimal siteVisitCharges = 0;
        decimal otherCharges = 0;

        // Try parsing the text values to decimal
        decimal.TryParse(txtcomponentcost.Text, out componentCost);
        decimal.TryParse(txtsitevisitcharges.Text, out siteVisitCharges);
        decimal.TryParse(txtothercharges.Text, out otherCharges);

        // Calculate the total
        var Total = componentCost + siteVisitCharges + otherCharges;

        // Assign the total to the final cost text box
        txtfinalcost.Text = Total.ToString("F2"); // Format to 2 decimal places
        txtestimatedquo.Text = Total.ToString("F2"); // Format to 2 decimal places
    }
}