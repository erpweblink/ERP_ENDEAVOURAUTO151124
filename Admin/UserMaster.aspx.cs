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

public partial class Admin_UserMaster : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ddlrolee();
           // GridView();
            if (Request.QueryString["Id"] != null)
            {

                string id = Decrypt(Request.QueryString["Id"].ToString());
                loadData(id);
                btnSubmit.Text = "Update";
                hidden.Value = id;

            }
            else
            {

            }
        }
    }

    //protected void Companyname()
    //{
    //    try
    //    {
    //        DataTable dt = new DataTable();

    //        con.Open();
    //        SqlDataAdapter sad = new SqlDataAdapter("select [CustomerName] FROM [tblCustomer] where isdeleted='0' ", con);
    //        sad.Fill(dt);
    //        ddlcompany.DataValueField = "CustomerName";
    //        ddlcompany.DataSource = dt;
    //        ddlcompany.DataBind();

    //        con.Close();
    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }
    //}

    protected void ddlrolee()
    {
        try
        {
            DataTable dt = new DataTable();

            con.Open();
            SqlDataAdapter sad = new SqlDataAdapter("select [Id],[Role] FROM [tblRole] where isdeleted='0' AND IsActive='1'", con);
            sad.Fill(dt);
            ddlRole.DataValueField = "Id";
            ddlRole.DataTextField = "Role";
            ddlRole.DataSource = dt;
            ddlRole.DataBind();

            con.Close();
        }
        catch (Exception)
        {

            throw;
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        // string createdby = "Admin";
        string createdby = Session["adminname"].ToString();

        try
        {
            if (btnSubmit.Text == "Submit")

            {

                if (ddlRole.SelectedItem.Text == "Select Role")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Role');", true);
                }
                else
                {
                    con.Open();
                    SqlCommand cmd1 = new SqlCommand("SELECT [Id],[name],[pass],[ConfamPass],[MobileNumber],[role],roleId,[IsActive],[Email],[CreatedBy],[CreatedDate],[updatedBy],[updatedDate] from LogIn where [Email]='" + txtemail.Text + "' AND [MobileNumber]='" + txtmobileNo.Text + "' AND isdeleted='0' ", con);
                    SqlDataReader reader = cmd1.ExecuteReader();

                    if (reader.Read())
                    {

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert(' User already exist..!');", true);
                    }
                    else
                    {
                        con.Close();

                        DateTime Date = DateTime.Now;
                        con.Open();
                        SqlCommand cmd = new SqlCommand("SP_User", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@name", txtName.Text);
                        cmd.Parameters.AddWithValue("@pass", Password.Text);
                        cmd.Parameters.AddWithValue("@ConfamPass", txtconpassword.Text);
                        cmd.Parameters.AddWithValue("@MobileNumber", txtmobileNo.Text);
                        cmd.Parameters.AddWithValue("@role", ddlRole.SelectedItem.Text);
                        cmd.Parameters.AddWithValue("@roleId", ddlRole.SelectedItem.Value);
                        //cmd.Parameters.AddWithValue("@UserCompany", ddlcompany.SelectedItem.Text);
                        // cmd.Parameters.AddWithValue("@IsActive", DropDownListisActive.Text);
                        cmd.Parameters.AddWithValue("@Email", txtemail.Text);
                        cmd.Parameters.AddWithValue("@CreatedBy", createdby);
                        cmd.Parameters.AddWithValue("@CreatedDate", Date);
                        cmd.Parameters.AddWithValue("@updatedBy", null);
                        cmd.Parameters.AddWithValue("@updatedDate", Date);
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

                        cmd.Parameters.AddWithValue("@IsActive", isactive);
                        cmd.Parameters.AddWithValue("@Action", "Insert");
                        cmd.ExecuteNonQuery();
                        con.Close();
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Saved Successfully');", true);
                    }
                }
            }
            else if (btnSubmit.Text == "Update")

            {
                if (ddlRole.SelectedItem.Text == "Select Role")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Role');", true);
                }
                else
                {
                    con.Close();

                    DateTime Date = DateTime.Now;
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SP_User", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@name", txtName.Text);
                    cmd.Parameters.AddWithValue("@pass", Password.Text);
                    cmd.Parameters.AddWithValue("@ConfamPass", txtconpassword.Text);
                    cmd.Parameters.AddWithValue("@MobileNumber", txtmobileNo.Text);
                    cmd.Parameters.AddWithValue("@role", ddlRole.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@roleId", ddlRole.SelectedItem.Value);
                    //cmd.Parameters.AddWithValue("@UserCompany", ddlcompany.SelectedItem.Text);
                    // cmd.Parameters.AddWithValue("@IsActive", DropDownListisActive.Text);
                    cmd.Parameters.AddWithValue("@Email", txtemail.Text);
                    cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(hidden.Value));

                    cmd.Parameters.AddWithValue("@CreatedBy", createdby);
                    cmd.Parameters.AddWithValue("@CreatedDate", Date);
                    cmd.Parameters.AddWithValue("@updatedBy", null);
                    cmd.Parameters.AddWithValue("@updatedDate", Date);
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

                    cmd.Parameters.AddWithValue("@IsActive", isactive);
                    cmd.Parameters.AddWithValue("@Action", "Update");
                    cmd.ExecuteNonQuery();
                    con.Close();
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Updated Successfully');", true);
                    ddlRole.Items.Clear();
                }
            }

        }
        catch (Exception ex)
        {
            throw;
        }
    }
    //void GridView()
    //{
    //    try
    //    {
    //        DataTable dt = new DataTable();
    //        con.Open();
    //        SqlDataAdapter sad = new SqlDataAdapter("SELECT [Id],[name],[pass],[ConfamPass],[MobileNumber],[role],roleId,[IsActive],[Email],[CreatedBy],[CreatedDate],[updatedBy],[updatedDate] from LogIn where isdeleted='0' ", con);
    //        sad.Fill(dt);
    //        gv_user.EmptyDataText = "Not Records Found";
    //        gv_user.DataSource = dt;
    //        gv_user.DataBind();

    //        con.Close();
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    protected void gv_user_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "RowEdit")
        {
            Response.Redirect("UserMaster.aspx?Id=" + encrypt(e.CommandArgument.ToString()) + "");
        }
        if (e.CommandName == "RowDelete")
        {
            SqlCommand cmddelete = new SqlCommand("update LogIn set isdeleted='1' where Id=@Id", con);
            cmddelete.Parameters.AddWithValue("@Id", Convert.ToInt32(e.CommandArgument.ToString()));
            cmddelete.Parameters.AddWithValue("@isdeleted", '1');
            con.Open();
            cmddelete.ExecuteNonQuery();
            con.Close();
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Delete Sucessfully');", true);

            //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Delete sucessfully!!');window.location ='CustomerList.aspx';", true);

            //GridView();

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

    protected void loadData(string id)
    {
        try
        {
            SqlDataAdapter sad = new SqlDataAdapter("SELECT [Id],[name],[pass],[ConfamPass],[MobileNumber],[role],roleId,[IsActive],[Email],[CreatedBy],[CreatedDate],[updatedBy],[updatedDate] from LogIn  where [Id]='" + id + "'", con);

            DataTable dt = new DataTable();
            sad.Fill(dt);

            if (dt.Rows.Count > 0)
            {

                txtName.Text = dt.Rows[0]["name"].ToString();
                txtemail.Text = dt.Rows[0]["Email"].ToString();
                Password.Text = dt.Rows[0]["pass"].ToString();
                txtconpassword.Text = dt.Rows[0]["ConfamPass"].ToString();
                txtmobileNo.Text = dt.Rows[0]["MobileNumber"].ToString();
               // ddlcompany.SelectedValue = dt.Rows[0][""].ToString();
               // ddlRole.SelectedItem.Text = dt.Rows[0]["role"].ToString();
                ddlRole.SelectedValue = dt.Rows[0]["roleId"].ToString();
                string flgStatus = "";
                if (dt.Rows[0]["IsActive"].ToString() == "False")
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
    protected void CheckBoxconpass_CheckedChanged(object sender, EventArgs e)
    {
        //txtconpassword.TextMode.
    }

    protected void gv_user_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblisstatu = (Label)e.Row.FindControl("lblIsActive") as Label;
            if (lblisstatu.Text == "True")

            {
                lblisstatu.Text = "Active";
            }

            else

            {

                lblisstatu.Text = "DeActive";

            }
        }
    }
    SqlDataAdapter sad;
    //protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    //{

    //    try
    //    {
    //        DataTable dt = new DataTable();

    //        if (ddlStatus.Text == "All")
    //        {
    //            sad = new SqlDataAdapter("SELECT [Id],[name],[pass],[ConfamPass],[MobileNumber],[role],roleId,[IsActive],[Email],[CreatedBy],[CreatedDate],[updatedBy],[updatedDate] from LogIn where isdeleted='0' ", con);
    //        }
    //        else
    //        {
    //            sad = new SqlDataAdapter("SELECT [Id],[name],[pass],[ConfamPass],[MobileNumber],[role],roleId,[IsActive],[Email],[CreatedBy],[CreatedDate],[updatedBy],[updatedDate] from LogIn where [IsActive]='" + ddlStatus.SelectedValue + "' AND isdeleted='0' ", con);
    //        }
    //        sad.Fill(dt);
    //        //gv_user.EmptyDataText = "Not Records Found";
    //        //gv_user.DataSource = dt;
    //        //gv_user.DataBind();
    //    }
    //    catch (Exception)
    //    {

    //        throw;
    //    }

    //}

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetUserList(string prefixText, int count)
    {
        return AutoFillUserlist(prefixText);
    }

    public static List<string> AutoFillUserlist(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT name from LogIn where " + "name like @Search + '%' AND isdeleted='0'";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> name = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        name.Add(sdr["name"].ToString());
                    }
                }
                con.Close();
                return name;
            }

        }
    }
    //protected void lnkBtnsearch_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (string.IsNullOrEmpty(txtSearch.Text))
    //        {
    //            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Search User Name');", true);

    //        }
    //        if (string.IsNullOrEmpty(txtSearch.Text))
    //        {

    //           // GridView();

    //        }
    //        else
    //        {

    //            DataTable dt = new DataTable();

    //            SqlDataAdapter sad = new SqlDataAdapter("SELECT [Id],[name],[pass],[ConfamPass],[MobileNumber],[role],[IsActive],[Email],[CreatedBy],[CreatedDate],[updatedBy],[updatedDate] from LogIn  where [name]='" + txtSearch.Text + "' AND isdeleted='0'", con);
    //            sad.Fill(dt);
    //            //gv_user.EmptyDataText = "Not Records Found";
    //            //gv_user.DataSource = dt;
    //            //gv_user.DataBind();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("UserMasterList.aspx");
    }
    //protected void gv_user_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    gv_user.PageIndex = e.NewPageIndex;
    //    GridView();
    //}

    protected void lnkrefresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("UserMaster.aspx");
    }
}