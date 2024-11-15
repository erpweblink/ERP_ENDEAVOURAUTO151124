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

public partial class Reception_Component : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
    DataTable dt11 = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
          

            DropDownListUnits.Items.Clear();
            DropDownListUnits.Items.Add("Nos.");
            DropDownListUnits.Items.Add("Unit.");
            DropDownListUnits.Items.Add("Pieces.");
            //GridView();

            if (Request.QueryString["Compid"] != null)
            {
                string id = Decrypt(Request.QueryString["Compid"].ToString());
                loadData(id);
                btnSubmit.Text = "Update";
                hidden.Value = id;

            }

        }

    }


    //void gvbind_Company()
    //{
    //    try
    //    {
    //        string UserCompany = Session["UserCompany"].ToString();
    //        DataTable dt = new DataTable();
    //        SqlDataAdapter sad = new SqlDataAdapter("SELECT [Compid],[CompName],[IsStatus],[HSN],[Tax],[Units],[Rate],[CreateBy],[CreateDate],[UpdateBy],[UpdateDate] FROM [tblComponent] where isdeleted='0'", con);
    //        sad.Fill(dt);
    //        gv_Comp.EmptyDataText = "Not Records Found";
    //        gv_Comp.DataSource = dt;
    //        gv_Comp.DataBind();
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }

    //}

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
     string createdby = Session["adminname"].ToString();

        try
        {
            if (btnSubmit.Text == "Submit")
            {
                int id;

                con.Open();
                SqlCommand cmd1 = new SqlCommand("SELECT [Compid],[CompName],[IsStatus],[HSN],[Tax],[Units],[Rate],[CreateBy],[CreateDate],[UpdateBy],[UpdateDate] FROM [tblComponent] where CompName='" + txtCompName.Text + "' AND isdeleted='0'", con);
                SqlDataReader reader = cmd1.ExecuteReader();

                if (reader.Read())
                {

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Data Already Exist.');window.location ='Component.aspx';", true);
                }
                else
                {
                    con.Close();
                    DateTime Date = DateTime.Now;
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SP_Component", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CompName", txtCompName.Text);
                    cmd.Parameters.AddWithValue("@HSN", txthsc.Text);
                    cmd.Parameters.AddWithValue("@Tax", txttax.Text);
                    cmd.Parameters.AddWithValue("@Units", DropDownListUnits.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@Rate", txtrate.Text);
                    cmd.Parameters.AddWithValue("@CreateBy", createdby);
                    cmd.Parameters.AddWithValue("@CreateDate", Date);
                    cmd.Parameters.AddWithValue("@UpdateBy", null);
                    cmd.Parameters.AddWithValue("@UpdateDate", Date);
                    cmd.Parameters.AddWithValue("@isdeleted", '0');
                    bool isactive = true;
                    if (DropDownListisActive.Text == "Yes")
                    {
                        isactive = true;
                    }
                    else
                    {
                        isactive = false;
                    }

                    cmd.Parameters.AddWithValue("@IsStatus", isactive);
                    cmd.Parameters.AddWithValue("@Action", "Insert");
                    cmd.ExecuteNonQuery();
                    con.Close();

                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Saved Successfully');", true);
                }
            }
            else if (btnSubmit.Text == "Update")
            {
                
                    DateTime Date = DateTime.Now;
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SP_Component", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CompName", txtCompName.Text);
                    cmd.Parameters.AddWithValue("@HSN", txthsc.Text);
                    cmd.Parameters.AddWithValue("@Tax", txttax.Text);
                    cmd.Parameters.AddWithValue("@Units", DropDownListUnits.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@Rate", txtrate.Text);

                    cmd.Parameters.AddWithValue("@CreateBy", createdby);
                    cmd.Parameters.AddWithValue("@CreateDate", Date);
                    cmd.Parameters.AddWithValue("@UpdateBy", null);
                    cmd.Parameters.AddWithValue("@UpdateDate", Date);
                    cmd.Parameters.AddWithValue("@Compid", Convert.ToInt32(hidden.Value));
                    cmd.Parameters.AddWithValue("@isdeleted", '0');
                    bool isactive = true;
                    if (DropDownListisActive.Text == "Yes")
                    {
                        isactive = true;
                    }
                    else
                    {
                        isactive = false;
                    }

                    cmd.Parameters.AddWithValue("@IsStatus", isactive);
                    cmd.Parameters.AddWithValue("@Action", "Update");
                    cmd.ExecuteNonQuery();
                    con.Close();
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Updated Successfully');", true);
                }
             }
        catch (Exception ex)
        {
            throw;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        txtCompName.Text = "";
        Response.Redirect("ComponentList.aspx");
    }

    //void GridView()
    //{
    //    try
    //    {
    //        DataTable dt = new DataTable();
    //        con.Open();
    //        SqlDataAdapter sad = new SqlDataAdapter("SELECT [Compid],[CompName],[IsStatus],[HSN],[Tax],[Units],[Rate],[CreateBy],[CreateDate],[UpdateBy],[UpdateDate] FROM [tblComponent] where isdeleted='0'", con);
    //        sad.Fill(dt);
    //        gv_Comp.EmptyDataText = "Not Records Found";
    //        gv_Comp.DataSource = dt;
    //        gv_Comp.DataBind();

    //        con.Close();
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}


    //protected void gv_Comp_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    if (e.CommandName == "RowEdit")
    //    {
    //        Response.Redirect("Component.aspx?Compid=" + encrypt(e.CommandArgument.ToString()) + "");
    //    }

    //    if (e.CommandName == "RowDelete")
    //    {

    //        SqlCommand cmddelete = new SqlCommand("update tblComponent set isdeleted='1' where Compid=@Compid", con);
    //        cmddelete.Parameters.AddWithValue("@Compid", Convert.ToInt32(e.CommandArgument.ToString()));
    //        cmddelete.Parameters.AddWithValue("@isdeleted", '1');
    //        con.Open();
    //        cmddelete.ExecuteNonQuery();
    //        con.Close();
    //        ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Delete Sucessfully');", true);

    //        //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Delete sucessfully!!');window.location ='CustomerList.aspx';", true);

    //        GridView();
    //    }
    //}
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

    protected void loadData(string id)
    {
        try
        {
            SqlDataAdapter sad = new SqlDataAdapter("SELECT [Compid],[CompName],[IsStatus],[HSN],[Tax],[Units],[Rate],[CreateBy],[CreateDate],[UpdateBy],[UpdateDate] FROM [tblComponent] where Compid='" + id + "' ", con);

            DataTable dt = new DataTable();
            sad.Fill(dt);

            if (dt.Rows.Count > 0)
            {

                txtCompName.Text = dt.Rows[0]["CompName"].ToString();
                txthsc.Text = dt.Rows[0]["HSN"].ToString();
                txttax.Text = dt.Rows[0]["Tax"].ToString();
                DropDownListUnits.SelectedItem.Text = dt.Rows[0]["Units"].ToString();
                txtrate.Text = dt.Rows[0]["Rate"].ToString();
               
                string flgStatus = "";
                if (dt.Rows[0]["IsStatus"].ToString() == "False")
                {
                    flgStatus = "No";
                }
                else
                {
                    flgStatus = "Yes";
                }
                DropDownListisActive.Text = flgStatus;
            }
            else
            {

            }
        }
        catch (Exception ex)
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

    protected void gv_Comp_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblisstatus = (Label)e.Row.FindControl("lblisstatus") as Label;

            if (lblisstatus.Text == "True")

            {
                lblisstatus.Text = "Active";
            }

            else

            {

                lblisstatus.Text = "DeActive";

            }

        }

    }

    //protected void lnkBtnsearch_Click(object sender, EventArgs e)
    //{
    //    try
    //    {

    //        if (string.IsNullOrEmpty(txtSearch.Text))
    //        {

    //            GridView();
    //            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Search Componant');", true);
    //        }
    //        else
    //        {

    //            DataTable dt = new DataTable();

    //            SqlDataAdapter sad = new SqlDataAdapter("SELECT [Compid],[CompName],[IsStatus],[HSN],[Tax],[Units],[Rate],[CreateBy],[CreateDate],[UpdateBy],[UpdateDate] FROM [tblComponent] where CompName='" + txtSearch.Text + "' ", con);
    //            sad.Fill(dt);
    //            gv_Comp.EmptyDataText = "Not Records Found";
    //            gv_Comp.DataSource = dt;
    //            gv_Comp.DataBind();
    //        }


    //        //GridView();
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }

    //}

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetCompList(string prefixText, int count)
    {
        return AutoFillComplist(prefixText);
    }

    public static List<string> AutoFillComplist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT CompName from tblComponent where " + "CompName like @Search + '%' AND isdeleted='0'";

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
    //SqlDataAdapter sad;
    //protected void ComponentStatus()
    //{
    //    try
    //    {
    //        DataTable dt = new DataTable();

    //        if (ddlStatus.Text == "All")
    //        {
    //            sad = new SqlDataAdapter("SELECT [Compid],[CompName],[IsStatus],[HSN],[Tax],[Units],[Rate],[CreateBy],[CreateDate],[UpdateBy],[UpdateDate] FROM tblComponent where isdeleted='0'", con);
    //        }
    //        else
    //        {
    //            sad = new SqlDataAdapter("SELECT [Compid],[CompName],[IsStatus],[HSN],[Tax],[Units],[Rate],[CreateBy],[CreateDate],[UpdateBy],[UpdateDate] FROM [tblComponent] where IsStatus='" + ddlStatus.SelectedValue + "' AND isdeleted='0' ", con);
    //        }
    //        sad.Fill(dt);
    //        gv_Comp.EmptyDataText = "Not Records Found";
    //        gv_Comp.DataSource = dt;
    //        gv_Comp.DataBind();
    //    }
    //    catch (Exception)
    //    {

    //        throw;
    //    }
    //}

    //protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    ComponentStatus();
    //}

    //protected void gv_Comp_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    gv_Comp.PageIndex = e.NewPageIndex;
    //    GridView();
    //}

    protected void lnkrefresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("Component.aspx");
    }
}