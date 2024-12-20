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
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillEngName();

            this.txttestingdate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            this.txttestingdate.TextMode = TextBoxMode.Date;
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

    private void FillEngName()
    {
        SqlDataAdapter ad = new SqlDataAdapter("SELECT * FROM [tbl_Engineer]", con);
        DataTable dt = new DataTable();
        ad.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            txtEngiName.DataSource = dt;
            // ddlcompnay.DataValueField = "ID";
            txtEngiName.DataTextField = "EngineerName";
            txtEngiName.DataBind();
            txtEngiName.Items.Insert(0, " --  Select Engineer Name -- ");
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
                //txtEngiName.Text = dt.Rows[0]["EngiName"].ToString();
                hiddenSelectedEngineers.Value = dt.Rows[0]["EngiName"].ToString();
                txtStatus.Text = dt.Rows[0]["Status"].ToString();
                txtRemark.Text = dt.Rows[0]["Remark"].ToString();
                txtreportedto.Text = dt.Rows[0]["ReportedTO"].ToString();
                //DateTime f1 = Convert.ToDateTime(dt.Rows[0]["TestingDate"].ToString());
                //txttestingdate.Text = f1.ToString("yyyy-MM-dd");
                DateTime ffff4 = Convert.ToDateTime(dt.Rows[0]["TestingDate"].ToString());
                txttestingdate.Text = ffff4.ToString("yyyy-MM-dd");
            }
            /////bind gridview componants
            SqlDataAdapter sadcom = new SqlDataAdapter("select * FROM tblRepairingEvalution where JobNo='" +txttagno.Text + "'", con);

            DataTable dtcom = new DataTable();
            sadcom.Fill(dtcom);
            int count=0;
            for (int i = 0; i < dtcom.Rows.Count; i++)
            {
                dtcomponant.Rows.Add(count, dtcom.Rows[i]["JobNo"].ToString(), dtcom.Rows[i]["CompId"].ToString(), dtcom.Rows[i]["CompName"].ToString(), dtcom.Rows[i]["Quantity"].ToString());
                count = count + 1;
            }
            gv_Comp.DataSource = dtcomponant;
            gv_Comp.DataBind();
            gv_Comp.EmptyDataText = "Records Not Found";
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
                txttagno.Text = dt.Rows[0]["JobNo"].ToString();
                txtproductname.Text = dt.Rows[0]["ProductName"].ToString();
                txtEngiName.Text = dt.Rows[0]["EngiName"].ToString();
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
                    
                    dt.Rows.Add(ViewState["RowNo"], txttagno.Text, txtcomponent.Text, txtcomponent.Text,txtQuantityComp.Text);
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

            if (txtStatus.SelectedItem.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Please Fill Status !!!');", true);
            }
            else if (txtRemark.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Please Fill Remarks !!!');", true);
            }
            else if (gv_Comp.Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Add Componant And Quantity !!!');", true);

            }
            else
            {

                // string createdby = "Admin";
                string createdby = Session["adminname"].ToString();

                DateTime Date = DateTime.Now;
                if (btnSubmit.Text == "Submit")
                {


                    foreach (GridViewRow g1 in gv_Comp.Rows)
                    {

                        Label lblCompId = (Label)gv_Comp.Rows[g1.RowIndex].FindControl("lblcompid");
                        Label lblCompname = (Label)gv_Comp.Rows[g1.RowIndex].FindControl("lblCompnamee");
                        Label lblQuantity = (Label)gv_Comp.Rows[g1.RowIndex].FindControl("lblQty");
                        Label lbljobno = (Label)gv_Comp.Rows[g1.RowIndex].FindControl("lblJobno");

                        string CompId = lblCompId.Text;
                        string CompName = lblCompname.Text;
                        string CompQty = lblQuantity.Text;
                        string job = lbljobno.Text;

                        SqlCommand cmd = new SqlCommand(@"INSERT INTO [tblRepairingEvalution]([JobNo],[CompId]
                ,[CompName],[Quantity],[CreatedBy],[CreatedDate]) 
                 VALUES ('" + job + "','" + CompId + "','" + CompName + "','" + CompQty + "','" + createdby + "',getdate())", con);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                else
                {
                    try
                    {
                        DataTable Dt = new DataTable();
                        SqlDataAdapter sadd = new SqlDataAdapter("select * from tblRepairingEvalution where JobNo='" + txttagno.Text + "'", con);
                        sadd.Fill(Dt);
                        if (Dt.Rows.Count == gv_Comp.Rows.Count)
                        {
                            foreach (GridViewRow g1 in gv_Comp.Rows)
                            {

                                Label lblCompId = (Label)gv_Comp.Rows[g1.RowIndex].FindControl("lblcompid");
                                Label lblCompname = (Label)gv_Comp.Rows[g1.RowIndex].FindControl("lblCompnamee");
                                Label lblQuantity = (Label)gv_Comp.Rows[g1.RowIndex].FindControl("lblQty");
                                Label lbljobno = (Label)gv_Comp.Rows[g1.RowIndex].FindControl("lblJobno");

                                string CompId = lblCompId.Text;
                                string CompName = lblCompname.Text;
                                string CompQty = lblQuantity.Text;
                                string job = lbljobno.Text;

                                SqlCommand cmd = new SqlCommand("update tblRepairingEvalution set CompName='" + CompName + "',Quantity='" + CompQty + "',UpdateBy='" + createdby + "' where CompId='" + CompId + "'", con);
                                con.Open();
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }
                        }
                        else
                        {
                            SqlCommand cmddelete = new SqlCommand("DELETE FROM tblRepairingEvalution WHERE JobNo=@JobNo", con);
                            cmddelete.Parameters.AddWithValue("@JobNo", txttagno.Text);
                            con.Open();
                            cmddelete.ExecuteNonQuery();
                            con.Close();
                            foreach (GridViewRow g1 in gv_Comp.Rows)
                            {

                                Label lblCompId = (Label)gv_Comp.Rows[g1.RowIndex].FindControl("lblcompid");
                                Label lblCompname = (Label)gv_Comp.Rows[g1.RowIndex].FindControl("lblCompnamee");
                                Label lblQuantity = (Label)gv_Comp.Rows[g1.RowIndex].FindControl("lblQty");
                                Label lbljobno = (Label)gv_Comp.Rows[g1.RowIndex].FindControl("lblJobno");

                                string CompId = lblCompId.Text;
                                string CompName = lblCompname.Text;
                                string CompQty = lblQuantity.Text;
                                string job = lbljobno.Text;

                                SqlCommand cmd = new SqlCommand(@"INSERT INTO [tblRepairingEvalution]([JobNo],[CompId]
                ,[CompName],[Quantity],[CreatedBy],[CreatedDate]) 
                 VALUES ('" + job + "','" + CompId + "','" + CompName + "','" + CompQty + "','" + createdby + "',getdate())", con);
                                con.Open();
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }

                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
                con.Open();
                string complete = "1";

                string selectedEngineers = hiddenSelectedEngineers.Value;

                SqlCommand cmd1 = new SqlCommand("update tblTestingProduct set ProductName='" + txtproductname.Text + "', EngiName='" + selectedEngineers + "', ReportedTO='"+ txtreportedto.Text +"', TestingDate='" + txttestingdate.Text + "',Status='" + txtStatus.SelectedItem.Text + "',Remark='" + txtRemark.Text + "' ,isCompleted='" + complete + "',UpdateBy='" + createdby + "',UpdatedDate=getdate() where JobNo='" + txttagno.Text + "'", con);
                cmd1.ExecuteNonQuery();
                con.Close();
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Updated Successfully');", true);
                //Response.Redirect("Evalution.aspx");
            }
               
                
            
        }
        catch (Exception)
        {

            throw;
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
        Response.Redirect("Evalution.aspx");
    }
}