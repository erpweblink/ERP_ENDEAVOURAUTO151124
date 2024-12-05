using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Security.Cryptography;
using System.IO;
using System.Text;

public partial class Admin_JOBCard : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);

    DataTable dt = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillEngName1();
            FillEngName2();
            FillEngName3();
            FillEngName4();

            GridRecordparticular1();
            GridRecordparticular2();
            //BindParticulars1();
            //BindParticulars2();
            if (Request.QueryString["JobCardNo"] != null)
            {
                string id = Decrypt(Request.QueryString["JobCardNo"].ToString());
                loadData(id);
                btnSubmit.Text = "Update";
                txtjobno.ReadOnly = true;
                hidden.Value = id;
            }
        }
    }

    private void FillEngName1()
    {
        SqlDataAdapter ad = new SqlDataAdapter("SELECT * FROM [tbl_Engineer]", con);
        DataTable dt = new DataTable();
        ad.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            txtengineername.DataSource = dt;
            // ddlcompnay.DataValueField = "ID";
            txtengineername.DataTextField = "EngineerName";
            txtengineername.DataBind();
            txtengineername.Items.Insert(0, " --  Select Engineer Name -- ");
        }
    }

    private void FillEngName2()
    {
        SqlDataAdapter ad = new SqlDataAdapter("SELECT * FROM [tbl_Engineer]", con);
        DataTable dt = new DataTable();
        ad.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            //txtengineername2.DataSource = dt;
            //// ddlcompnay.DataValueField = "ID";
            //txtengineername2.DataTextField = "EngineerName";
            //txtengineername2.DataBind();
            //txtengineername2.Items.Insert(0, " --  Select Engineer Name -- ");
        }
    }

    private void FillEngName3()
    {
        SqlDataAdapter ad = new SqlDataAdapter("SELECT * FROM [tbl_Engineer]", con);
        DataTable dt = new DataTable();
        ad.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            //txtengineername3.DataSource = dt;
            //// ddlcompnay.D3ataValueField = "ID";
            //txtengineername3.DataTextField = "EngineerName";
            //txtengineername3.DataBind();
            //txtengineername3.Items.Insert(0, " --  Select Engineer Name -- ");
        }
    }

    private void FillEngName4()
    {
        SqlDataAdapter ad = new SqlDataAdapter("SELECT * FROM [tbl_Engineer]", con);
        DataTable dt = new DataTable();
        ad.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            //txtengineername4.DataSource = dt;
            //// ddlcompnay.D4ataValueField = "ID";
            //txtengineername4.DataTextField = "EngineerName";
            //txtengineername4.DataBind();
            //txtengineername4.Items.Insert(0, " --  Select Engineer Name -- ");
        }
    }

    protected void GridRecordparticular1()
    {
        try
        {
            DataTable dt = new DataTable();
            SqlDataAdapter sad = new SqlDataAdapter("select * from tbljobcardparticulars", con);
            sad.Fill(dt);
            gvparticular.DataSource = dt;
            gvparticular.DataBind();
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void GridRecordparticular2()
    {
        try
        {
            DataTable dt = new DataTable();
            SqlDataAdapter sad = new SqlDataAdapter("select * from tbljobcardparticula2", con);
            sad.Fill(dt);
            gv_jobcardgrid2.DataSource = dt;
            gv_jobcardgrid2.DataBind();
        }
        catch (Exception)
        {
            throw;
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


    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string createdby = Session["adminname"].ToString();

        try
        {
            if (btnSubmit.Text == "Submit")
            {

                con.Open();
                SqlCommand cmd2 = new SqlCommand("SELECT  * FROM [tblInwardEntry] where JobNo='" + txtjobno.Text + "' AND isdeleted='0'", con);
                SqlDataReader reader1 = cmd2.ExecuteReader();

                if (reader1.Read())
                {
                    con.Close();
                    con.Open();
                    SqlCommand cmd1 = new SqlCommand("SELECT  [Id],[JobCardNo],[RepeatedNo],[ItemDesc],[ModelNo],[InwardDate],[outwardDate],[CreatedBy],[CreatedDate],[updatedby],[Updateddate],isdeleted FROM [tblJobcardHdr] where JobCardNo='" + txtjobno.Text + "' AND isdeleted='0'", con);
                    SqlDataReader reader = cmd1.ExecuteReader();

                    if (reader.Read())
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Data Already Exist.');window.location ='JOBCard.aspx';", true);
                    }
                    else
                    {
                        con.Close();
                        DateTime Date = DateTime.Now;
                        string selectedEngineers = hiddenSelectedEngineers.Value;

                        SqlCommand cmd = new SqlCommand("SP_JobcardHdr", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@JobCardNo", txtjobno.Text);
                        cmd.Parameters.AddWithValue("@RepeatedNo", txtrepeatedNo.Text);
                        cmd.Parameters.AddWithValue("@ItemDesc", txtitemdescription.Text);
                        cmd.Parameters.AddWithValue("@ModelNo", txtmodelno.Text);
                        cmd.Parameters.AddWithValue("@InwardDate", txtinwarddate.Text);
                        cmd.Parameters.AddWithValue("@outwardDate", txtoutwardate.Text);
                        cmd.Parameters.AddWithValue("@CreatedBy", createdby);
                        cmd.Parameters.AddWithValue("@status", txtstatus.SelectedItem.Text);
                       // cmd.Parameters.AddWithValue("@EngineerName", txtengineername.Text);
                        cmd.Parameters.AddWithValue("@EngineerName", selectedEngineers);
                        //cmd.Parameters.AddWithValue("@EngineerName2", txtengineername2.Text);
                        //cmd.Parameters.AddWithValue("@EngineerName3", txtengineername3.Text);
                        //cmd.Parameters.AddWithValue("@EngineerName4", txtengineername4.Text);
                        cmd.Parameters.AddWithValue("@Reparingdate", txtreparingdate.Text);

                        cmd.Parameters.AddWithValue("@MotorTrial", txtmotortrial.Text);
                        cmd.Parameters.AddWithValue("@MotorRating", txtmotorrating.Text);
                        cmd.Parameters.AddWithValue("@MotorCurrent", txtmotorcurrent.Text);
                        cmd.Parameters.AddWithValue("@TrialTime", txttrialtimedate.Text);
                        cmd.Parameters.AddWithValue("@KeypadTrail", DropDownListkeypadtrail.SelectedValue);
                        cmd.Parameters.AddWithValue("@AnologTrail", DropDownListanologtrail.SelectedValue);
                        cmd.Parameters.AddWithValue("@FanCleaning", DropDownListfancleaning.SelectedValue);
                        cmd.Parameters.AddWithValue("@ParameterOrignal", DropDownListparameterorignal.SelectedValue);
                        cmd.Parameters.AddWithValue("@PackingSOP", DropDownListpackingsop.SelectedValue);

                        cmd.Parameters.AddWithValue("@CreatedDate", Date);
                        cmd.Parameters.AddWithValue("@isdeleted", '0');
                        cmd.Parameters.AddWithValue("@Action", "Insert");
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();

                        foreach (GridViewRow g1 in gvparticular.Rows)
                        {

                            string Particular = (g1.FindControl("lblparticular1") as Label).Text;
                            string status = (g1.FindControl("txtstatus1") as TextBox).Text;
                            SqlCommand cmd4 = new SqlCommand("INSERT INTO tbljobCardDtls(JobCardNo,Particular,Statusjob)values('" + txtjobno.Text + "','" + Particular + "','" + status + "')", con);
                            con.Open();
                            cmd4.ExecuteNonQuery();
                            con.Close();

                        }
                        foreach (GridViewRow g2 in gv_jobcardgrid2.Rows)
                        {
                            string status = (g2.FindControl("txtstatus2") as TextBox).Text;
                            string Particular = (g2.FindControl("lblparticular2") as Label).Text;

                            SqlCommand cmd3 = new SqlCommand("INSERT INTO tblJobcardDtl2(JobCardNo,Particular,Status)values('" + txtjobno.Text + "','" + Particular + "','" + status + "')", con);
                            con.Open();
                            cmd3.ExecuteNonQuery();
                            con.Close();
                        }

                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Saved Successfully');", true);
                    }
                }
                else
                {
                    con.Close();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Job No. Not Exist !!');window.location ='JOBCard.aspx';", true);
                }
            }
            else if (btnSubmit.Text == "Update")
            {

                con.Close();
                DateTime Date = DateTime.Now;

                SqlCommand cmd = new SqlCommand("SP_JobcardHdr", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@JobCardNo", txtjobno.Text);
                cmd.Parameters.AddWithValue("@RepeatedNo", txtrepeatedNo.Text);
                cmd.Parameters.AddWithValue("@ItemDesc", txtitemdescription.Text);
                cmd.Parameters.AddWithValue("@ModelNo", txtmodelno.Text);
                cmd.Parameters.AddWithValue("@InwardDate", txtinwarddate.Text);
                cmd.Parameters.AddWithValue("@outwardDate", txtoutwardate.Text);
                cmd.Parameters.AddWithValue("@updatedby", createdby);
                cmd.Parameters.AddWithValue("@status", txtstatus.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@EngineerName", hiddenSelectedEngineers.Value);
                //cmd.Parameters.AddWithValue("@EngineerName2", txtengineername2.Text);
                //cmd.Parameters.AddWithValue("@EngineerName3", txtengineername3.Text);
                //cmd.Parameters.AddWithValue("@EngineerName4", txtengineername4.Text);
                cmd.Parameters.AddWithValue("@Reparingdate", txtreparingdate.Text);

                cmd.Parameters.AddWithValue("@MotorTrial", txtmotortrial.Text);
                cmd.Parameters.AddWithValue("@MotorRating", txtmotorrating.Text);
                cmd.Parameters.AddWithValue("@MotorCurrent", txtmotorcurrent.Text);
                cmd.Parameters.AddWithValue("@TrialTime", txttrialtimedate.Text);
                cmd.Parameters.AddWithValue("@KeypadTrail", DropDownListkeypadtrail.SelectedValue);
                cmd.Parameters.AddWithValue("@AnologTrail", DropDownListanologtrail.SelectedValue);
                cmd.Parameters.AddWithValue("@FanCleaning", DropDownListfancleaning.SelectedValue);
                cmd.Parameters.AddWithValue("@ParameterOrignal", DropDownListparameterorignal.SelectedValue);
                cmd.Parameters.AddWithValue("@PackingSOP", DropDownListpackingsop.SelectedValue);

                cmd.Parameters.AddWithValue("@Updateddate", Date);
                cmd.Parameters.AddWithValue("@isdeleted", '0');
                cmd.Parameters.AddWithValue("@Action", "Update");
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                foreach (GridViewRow g1 in gvparticular.Rows)
                {

                    string Particular = (g1.FindControl("lblparticular1") as Label).Text;
                    string status = (g1.FindControl("txtstatus1") as TextBox).Text;
                    SqlCommand cmd4 = new SqlCommand("update tbljobCardDtls set Particular='" + Particular + "',Statusjob='" + status + "' where JobCardNo='" + txtjobno.Text + "' AND Particular='" + Particular + "'", con);
                    con.Open();
                    cmd4.ExecuteNonQuery();
                    con.Close();
                }

                foreach (GridViewRow g2 in gv_jobcardgrid2.Rows)
                {
                    string status = (g2.FindControl("txtstatus2") as TextBox).Text;
                    string Particular = (g2.FindControl("lblparticular2") as Label).Text;
                    SqlCommand cmd3 = new SqlCommand("update tblJobcardDtl2 set Particular='" + Particular + "',Status='" + status + "' where JobCardNo='" + txtjobno.Text + "' AND Particular='" + Particular + "'", con);
                    con.Open();
                    cmd3.ExecuteNonQuery();
                    con.Close();
                }
            }
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Updated Successfully');", true);

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
            SqlDataAdapter sad = new SqlDataAdapter("SELECT  [Id],[JobCardNo],[RepeatedNo],[ItemDesc],[ModelNo],[InwardDate],[outwardDate],[Reparingdate],[CreatedBy],[CreatedDate],[updatedby],[Updateddate],[isdeleted],[MotorTrial],[MotorRating],[MotorCurrent],[TrialTime],[KeypadTrail], [AnologTrail],[FanCleaning],[ParameterOrignal],[PackingSOP],status,EngineerName,EngineerName2,EngineerName3,EngineerName4 from tblJobcardHdr where JobCardNo='" + id + "'", con);

            DataTable dt = new DataTable();
            sad.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                txtjobno.Text = dt.Rows[0]["JobCardNo"].ToString();
                txtrepeatedNo.Text = dt.Rows[0]["RepeatedNo"].ToString();
                txtitemdescription.Text = dt.Rows[0]["ItemDesc"].ToString();
                txtmodelno.Text = dt.Rows[0]["ModelNo"].ToString();
                hiddenSelectedEngineers.Value = dt.Rows[0]["EngineerName"].ToString();
                //txtengineername2.Text = dt.Rows[0]["EngineerName2"].ToString();
                //txtengineername3.Text = dt.Rows[0]["EngineerName3"].ToString();
                //txtengineername4.Text = dt.Rows[0]["EngineerName4"].ToString();
                txtstatus.Text = dt.Rows[0]["status"].ToString();
                DateTime ffff2 = Convert.ToDateTime(dt.Rows[0]["InwardDate"].ToString());
                txtinwarddate.Text = ffff2.ToString("yyyy-MM-dd");
                DateTime ffff1 = Convert.ToDateTime(dt.Rows[0]["outwardDate"].ToString());
                txtoutwardate.Text = ffff1.ToString("yyyy-MM-dd");
                //DateTime ffff3 = Convert.ToDateTime(dt.Rows[0]["Reparingdate"].ToString());
                //txtreparingdate.Text = ffff3.ToString("yyyy-MM-dd");

                DateTime ffff3;
                if (dt.Rows[0]["Reparingdate"] == DBNull.Value || string.IsNullOrEmpty(dt.Rows[0]["Reparingdate"].ToString()))
                {
                    ffff3 = new DateTime(1900, 1, 1);
                }
                else
                {
                    ffff3 = Convert.ToDateTime(dt.Rows[0]["Reparingdate"].ToString());
                }
                if (ffff3 == new DateTime(1900, 1, 1))
                {
                    txtreparingdate.Text = "";
                }
                else
                {
                    txtreparingdate.Text = ffff3.ToString("yyyy-MM-dd");
                }

                //DateTime ffff4 = Convert.ToDateTime(dt.Rows[0]["RepeatedDate"].ToString());
                //DateTime defaultDate = new DateTime(1900, 1, 1);

                //if (ffff4 != defaultDate)
                //{
                //    txtrepeateddate.Text = ffff4.ToString("yyyy-MM-dd");
                //}

                txtmotortrial.Text = dt.Rows[0]["MotorTrial"].ToString();
                txtmotorrating.Text = dt.Rows[0]["MotorRating"].ToString();
                txtmotorcurrent.Text = dt.Rows[0]["MotorCurrent"].ToString();
                DropDownListkeypadtrail.SelectedValue = dt.Rows[0]["KeypadTrail"].ToString();
                DropDownListanologtrail.SelectedValue = dt.Rows[0]["AnologTrail"].ToString();
                DropDownListfancleaning.SelectedValue = dt.Rows[0]["FanCleaning"].ToString();
                DropDownListparameterorignal.SelectedValue = dt.Rows[0]["ParameterOrignal"].ToString();
                DropDownListpackingsop.SelectedValue = dt.Rows[0]["PackingSOP"].ToString();


                DateTime ffff4;
                if (dt.Rows[0]["TrialTime"] == DBNull.Value || string.IsNullOrWhiteSpace(dt.Rows[0]["TrialTime"].ToString()))
                {
                    ffff4 = DateTime.MinValue;
                }
                else
                {
                    if (!DateTime.TryParse(dt.Rows[0]["TrialTime"].ToString(), out ffff4))
                    {
                        ffff4 = DateTime.MinValue;
                    }
                }
                txttrialtimedate.Text = ffff4.ToString("yyyy-MM-dd");
            }

            /////1st particulas gridview
            DataTable dt1 = new DataTable();
             
            SqlDataAdapter sad1 = new SqlDataAdapter("SELECT [Id],[JobCardNo],[Particular],[Statusjob] from tbljobCardDtls where JobCardNo='" + id + "'", con);
            sad1.Fill(dt1);

            gvparticular.DataSource = dt1;
            gvparticular.DataBind();
            ///2nd particulas gridview
            DataTable dt2 = new DataTable();
            SqlDataAdapter sad2 = new SqlDataAdapter("SELECT * from tblJobcardDtl2 where JobCardNo='" + id + "'", con);
            sad2.Fill(dt2);
            gv_jobcardgrid2.DataSource = dt2;
            gv_jobcardgrid2.DataBind();
            gv_jobcardgrid2.Visible = true;
        }
        catch (Exception ex)
        {
            throw;
        }

    }


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("JOBcardList.aspx");
    }

    protected void txtjobno_TextChanged(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        SqlDataAdapter sad = new SqlDataAdapter("select * from tblInwardEntry where JobNo='" + txtjobno.Text + "'", con);
        sad.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            txtitemdescription.Text = dt.Rows[0]["MateName"].ToString();
            txtmodelno.Text = dt.Rows[0]["ModelNo"].ToString();
            DateTime ffff2 = Convert.ToDateTime(dt.Rows[0]["DateIn"].ToString());
            txtinwarddate.Text = ffff2.ToString("yyyy-MM-dd");
        }
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
                com.CommandText = "select DISTINCT JobNo from tblInwardEntry where " + "JobNo like @Search + '%' AND isdeleted='0'";

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
}
