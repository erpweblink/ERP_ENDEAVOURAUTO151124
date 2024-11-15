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

public partial class Admin_UserAuthoration : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
    string Create_R, Update_R, View_R, Delete_R, Report_R, Pages = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ddlroledrop();
            // GridViewMenu();
            // ddlUserdrop();
            GridDiv.Visible = false;
            Role();
            // ddlUser.Items.Add(new ListItem("select user", "0"));
        }
    }
    protected void Role()
    {
        try
        {

            DataTable dt = new DataTable();
            con.Open();

            SqlDataAdapter sad = new SqlDataAdapter("select [Id],[name],[pass],[ConfamPass],[MobileNumber],[role],[roleId],[IsActive],[Email],[CreatedBy],[CreatedDate],[updatedBy],[updatedDate] FROM [LogIn] where [roleId]='" + ddlRole.SelectedItem.Value + "' AND isdeleted='0' AND IsActive='1'", con);
            sad.Fill(dt);
            ddlUser.DataValueField = "Id";
            ddlUser.DataTextField = "name";
            ddlUser.DataSource = dt;
            ddlUser.DataBind();
            //ddlUser.Items.Add(new ListItem("select user", ""));
            con.Close();
            GridDiv.Visible = false;
        }
        catch (Exception)
        {

            throw;
        }
    }
    protected void GridViewMenu()
    {
        try
        {

            DataTable dt = new DataTable();
            con.Open();
            SqlDataAdapter sad = new SqlDataAdapter("SELECT [ID],[MenuID],[MenuName],[Submenu],[role],[CreatedDate],[ModifiedDate],[CreatedBy],[UdatedBy],[IsActive] FROM [Tbl_MenuMaster]", con);
            sad.Fill(dt);
            gvUserAuthorization.EmptyDataText = "Not Records Found";
            gvUserAuthorization.DataSource = dt;
            gvUserAuthorization.DataBind();

            con.Close();
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + gvUserAuthorization.ClientID + "', 400, 1020 , 40 ,true); </script>", false);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    //protected void ddlUserdrop()
    // {
    //     try
    //     {
    //         DataTable dt = new DataTable();

    //         con.Open();
    //         SqlDataAdapter sad = new SqlDataAdapter("select [Id],[name],[pass],[ConfamPass],[MobileNumber],[role],[IsActive],[Email],[CreatedBy],[CreatedDate],[updatedBy],[updatedDate] FROM [LogIn]", con);
    //         sad.Fill(dt);
    //         ddlUser.DataValueField = "Id";
    //         ddlUser.DataTextField = "name";

    //         ddlUser.DataSource = dt;
    //         ddlUser.DataBind();

    //         con.Close();
    //     }
    //     catch (Exception)
    //     {

    //         throw;
    //     }
    // }

    protected void ddlroledrop()
    {
        try
        {
            DataTable dt = new DataTable();

            con.Open();
            SqlDataAdapter sad = new SqlDataAdapter("select [Id],[Role],[IsActive],[CreatedBy],[CreatedDate],[UpdateBy],[UpdatedDate],isdeleted,IsActive FROM [tblRole] where Role != 'Admin' AND isdeleted='0' AND IsActive='1'", con);
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

    protected void ddlRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ddlUser.Items.Add(new ListItem("select user", ""));
        Role();
    }

    protected void ddlUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GridDiv.Visible = true;

            con.Open();
            SqlCommand cmd1 = new SqlCommand("SELECT [ID],[UserID],UserName,[menuId],[MenuName],[Create_R],[Update_R],[View_R],[Delete_R],[Report_R],[createdBy],[CreatedDate],[UpdatedBy],[updatedDate],[IsActive] FROM [UserAuthorization_table] where [UserID]='" + ddlUser.SelectedItem.Value + "'", con);
            SqlDataReader reader = cmd1.ExecuteReader();
            if (reader.Read())
            {

                reader.Close();
                btnSubmit.Text = "Update";
                DataTable dt = new DataTable();
                SqlDataAdapter sad = new SqlDataAdapter("SELECT [ID],[UserID],UserName,[menuId],[MenuName],[Create_R],[Update_R],[View_R],[Delete_R],[Report_R],[createdBy],[CreatedDate],[UpdatedBy],[updatedDate],[IsActive],Pages FROM [UserAuthorization_table] where [UserID]='" + ddlUser.SelectedItem.Value + "'", con);
                sad.Fill(dt);
                gvUserAuthorization.EmptyDataText = "Not Records Found";
                gvUserAuthorization.DataSource = dt;
                gvUserAuthorization.DataBind();
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + gvUserAuthorization.ClientID + "', 400, 1020 , 40 ,true); </script>", false);

            }
            else
            {

                reader.Close();
                btnSubmit.Text = "Submit";
                DataTable dt = new DataTable();

                SqlDataAdapter sad = new SqlDataAdapter("SELECT [ID],[MenuID],[MenuName],[Submenu],[role],[CreatedDate],[ModifiedDate],[CreatedBy],[UdatedBy],[IsActive] FROM [Tbl_MenuMaster]   ", con);
                sad.Fill(dt);

                gvUserAuthorization.EmptyDataText = "Not Records Found";
                gvUserAuthorization.DataSource = dt;
                gvUserAuthorization.DataBind();
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + gvUserAuthorization.ClientID + "', 400, 1020 , 40 ,true); </script>", false);


            }

            con.Close();

        }


        catch (Exception)
        {

            throw;
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        if (btnSubmit.Text == "Submit")

        {
            foreach (GridViewRow g1 in gvUserAuthorization.Rows)
            {

                bool createChk = (g1.FindControl("chkCreate") as CheckBox).Checked;
                bool updateChk = (g1.FindControl("chkUpdate") as CheckBox).Checked;
                bool ViewChk = (g1.FindControl("chkView") as CheckBox).Checked;
                bool deleteChk = (g1.FindControl("chkDelete") as CheckBox).Checked;
                bool reportChk = (g1.FindControl("chkReport") as CheckBox).Checked;
                string menuname = (g1.FindControl("lblMenuName") as Label).Text;
                string menu = (g1.FindControl("lblMenuId") as Label).Text;
                int userId = Convert.ToInt32(ddlUser.SelectedItem.Value);
                bool pageChk = (g1.FindControl("chkPages") as CheckBox).Checked;


                string createdby = "Admin";
                ////createdby = Session["adminname"].ToString();
                DateTime Date = DateTime.Now;
                con.Open();
                SqlCommand cmd = new SqlCommand("SP_Authorazation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", ddlUser.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@UserName", ddlUser.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@menuId", menu);
                cmd.Parameters.AddWithValue("@MenuName", menuname);
                cmd.Parameters.AddWithValue("@Create", createChk);
                cmd.Parameters.AddWithValue("@Update", updateChk);
                cmd.Parameters.AddWithValue("@View", ViewChk);
                cmd.Parameters.AddWithValue("@Delete", deleteChk);
                cmd.Parameters.AddWithValue("@Report", reportChk);
                cmd.Parameters.AddWithValue("@createdBy", createdby);
                cmd.Parameters.AddWithValue("@CreatedDate", Date);
                cmd.Parameters.AddWithValue("@UpdatedBy", createdby);
                cmd.Parameters.AddWithValue("@updatedDate", null);
                cmd.Parameters.AddWithValue("@Pages", pageChk);
                //cmd.Parameters.AddWithValue("@IsActive", updateChk);
                cmd.Parameters.AddWithValue("@Action", "Insert");
                cmd.ExecuteNonQuery();
                con.Close();
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Saved Sucessfully');", true);
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + gvUserAuthorization.ClientID + "', 400, 1020 , 40 ,true); </script>", false);
            }
        }

        else if (btnSubmit.Text == "Update")
        {
            foreach (GridViewRow g1 in gvUserAuthorization.Rows)
            {
                string menuname = (g1.FindControl("lblMenuName") as Label).Text;
                string menu = (g1.FindControl("lblMenuId") as Label).Text;
                bool createChk = (g1.FindControl("chkCreate") as CheckBox).Checked;
                bool updateChk = (g1.FindControl("chkUpdate") as CheckBox).Checked;
                bool ViewChk = (g1.FindControl("chkView") as CheckBox).Checked;
                bool deleteChk = (g1.FindControl("chkDelete") as CheckBox).Checked;
                bool reportChk = (g1.FindControl("chkReport") as CheckBox).Checked;
                bool pageChk = (g1.FindControl("chkPages") as CheckBox).Checked;
                int userId = Convert.ToInt32(ddlUser.SelectedItem.Value);

                //string createdby = "Admin";
                string createdby = Session["adminname"].ToString();
                DateTime Date = DateTime.Now;
                con.Open();
                SqlCommand cmd = new SqlCommand("SP_Authorazation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.Parameters.AddWithValue("@UserName", ddlUser.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@menuId", menu);
                cmd.Parameters.AddWithValue("@MenuName", menuname);
                cmd.Parameters.AddWithValue("@Create", createChk);
                cmd.Parameters.AddWithValue("@Update", updateChk);
                cmd.Parameters.AddWithValue("@View", ViewChk);
                cmd.Parameters.AddWithValue("@Delete", deleteChk);
                cmd.Parameters.AddWithValue("@Report", reportChk);
                cmd.Parameters.AddWithValue("@createdBy", createdby);
                cmd.Parameters.AddWithValue("@CreatedDate", Date);
                cmd.Parameters.AddWithValue("@UpdatedBy", createdby);
                cmd.Parameters.AddWithValue("@updatedDate", Date);
                cmd.Parameters.AddWithValue("@Pages", pageChk);
                //cmd.Parameters.AddWithValue("@ID", );
                //cmd.Parameters.AddWithValue("@IsActive", updateChk);
                cmd.Parameters.AddWithValue("@Action", "Update");
                cmd.ExecuteNonQuery();
                con.Close();
            }
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Updated Sucessfully');", true);
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + gvUserAuthorization.ClientID + "', 400, 1020 , 40 ,true); </script>", false);
        }
    }

    protected void gvUserAuthorization_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (btnSubmit.Text == "Update")
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int id = Convert.ToInt32(gvUserAuthorization.DataKeys[e.Row.RowIndex].Values[0]);
                CheckBox chkCreate = (CheckBox)e.Row.FindControl("chkCreate");
                CheckBox chkUpdate = (CheckBox)e.Row.FindControl("chkUpdate");
                CheckBox chkView = (CheckBox)e.Row.FindControl("chkView");
                CheckBox chkDelete = (CheckBox)e.Row.FindControl("chkDelete");
                CheckBox chkReport = (CheckBox)e.Row.FindControl("chkReport");
                CheckBox chkpages = (CheckBox)e.Row.FindControl("chkPages");
                SqlCommand cmd = new SqlCommand("select Create_R,Update_R,View_R,Delete_R,Report_R,Pages from UserAuthorization_table where ID='" + id + "'", con);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {

                    Create_R = dr["Create_R"].ToString();
                    Update_R = dr["Update_R"].ToString();
                    View_R = dr["View_R"].ToString();
                    Delete_R = dr["Delete_R"].ToString();
                    Report_R = dr["Report_R"].ToString();
                    Pages = dr["Pages"].ToString();
                    dr.Close();
                }

                chkCreate.Checked = Create_R == "True" ? true : false;
                chkUpdate.Checked = Update_R == "True" ? true : false;
                chkView.Checked = View_R == "True" ? true : false;
                chkDelete.Checked = Delete_R == "True" ? true : false;
                chkReport.Checked = Report_R == "True" ? true : false;
                chkpages.Checked = Pages == "True" ? true : false;
            }
            // ScriptManager.RegisterStartupScript(this, this.GetType(), "Success", "scrollToElement();", true);
            //ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Data Update Sucessfully');", true);
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + gvUserAuthorization.ClientID + "', 400, 1020 , 40 ,true); </script>", false);

        }



    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("UserAuthoration.aspx");
    }
    protected void gvUserAuthorization_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridViewRow grv = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
        int RowIndex = grv.RowIndex;
        LinkButton selectlnkbtn = (LinkButton)gvUserAuthorization.Rows[RowIndex].FindControl("chkselectall");

        CheckBox chkCreate = (CheckBox)gvUserAuthorization.Rows[RowIndex].FindControl("chkCreate");
        chkCreate.Checked = chkCreate.Checked == true ? false : true;

        CheckBox chkUpdate = (CheckBox)gvUserAuthorization.Rows[RowIndex].FindControl("chkUpdate");
        chkUpdate.Checked = chkUpdate.Checked == true ? false : true;


        CheckBox chkDelete = (CheckBox)gvUserAuthorization.Rows[RowIndex].FindControl("chkDelete");
        chkDelete.Checked = chkDelete.Checked == true ? false : true;
        //chkUpdate.Checked = true;

        CheckBox chkreport = (CheckBox)gvUserAuthorization.Rows[RowIndex].FindControl("chkReport");
        chkreport.Checked = chkreport.Checked == true ? false : true;
        CheckBox chkview = (CheckBox)gvUserAuthorization.Rows[RowIndex].FindControl("chkView");
        chkview.Checked = chkview.Checked == true ? false : true;
        CheckBox chkpages = (CheckBox)gvUserAuthorization.Rows[RowIndex].FindControl("chkPages");
        chkpages.Checked = chkpages.Checked == true ? false : true;
        // ScriptManager.RegisterStartupScript(this, this.GetType(), "Success", "scrollToElement();", true);
        ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + gvUserAuthorization.ClientID + "', 400, 1020 , 40 ,true); </script>", false);

    }

}


