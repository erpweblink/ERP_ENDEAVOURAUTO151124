using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_UAuthorization : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
    string Pages = "";
    string PagesView = "";
    string PageName = "";

    //string Create_R, Update_R, View_R, Delete_R, Report_R, Pages = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindRole();

            GridDiv.Visible = false;
        }
    }

    protected void BindUser()
    {
        try
        {
            ddluser.Items.Clear();
            ddluser.Items.Add(new ListItem("--Select User--", ""));
            DataTable Dt = new DataTable();

            SqlDataAdapter Da = new SqlDataAdapter("Select Id,name From LogIn Where role='" + ddlrole.SelectedItem.Text + "' AND isdeleted='0' AND IsActive='1' ", con);
            Da.Fill(Dt);
            ddluser.DataTextField = "name";
            ddluser.DataValueField = "id";
            ddluser.DataSource = Dt;
            ddluser.DataBind();
            //ddluser.Items.Insert(0, "-- Select User --");
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void BindRole()
    {
        try
        {
            ddlrole.Items.Clear();
            ddlrole.Items.Add(new ListItem("--Select Role--", ""));
            DataTable Dt = new DataTable();

            
            SqlDataAdapter Da = new SqlDataAdapter("Select Id,Role From tblRole where Role !='Admin' AND isdeleted = '0'", con);
           // SqlDataAdapter Da = new SqlDataAdapter("Select Id,Role From tblRole where isdeleted = '0'", con);
            Da.Fill(Dt);
            ddlrole.DataTextField = "Role";
            ddlrole.DataValueField = "Id";
            ddlrole.DataSource = Dt;
            ddlrole.DataBind();
            //ddluser.Items.Insert(0, "-- Select User --");
            BindUser();
        }
        catch (Exception)
        {

            throw;
        }
    }

    protected void ddlrole_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindUser();
    }

    protected void ddluser_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //con.Open();
            GridDiv.Visible = true;
            DataTable Dt = new DataTable();
            SqlDataAdapter Da = new SqlDataAdapter("SELECT * FROM [EndeavourAuto].[tblUserRoleAuthorization] where [UserID]='" + ddluser.SelectedItem.Value + "'", con);
            Da.Fill(Dt);
            if (Dt.Rows.Count > 0)
            {

                btnSubmit.Text = "Update";
                DataTable Dtt = new DataTable();
                SqlDataAdapter Daa = new SqlDataAdapter("SELECT * FROM [EndeavourAuto].[tblUserRoleAuthorization] where [UserID]='" + ddluser.SelectedItem.Value + "'", con);
                Daa.Fill(Dtt);
                gvUserAuthorization.EmptyDataText = "No Records Found";
                gvUserAuthorization.DataSource = Dtt;
                gvUserAuthorization.DataBind();
                //con.Close();
            }
            else
            {
                //con.Close();

                btnSubmit.Text = "Save";
                DataTable Dttt = new DataTable();
                SqlDataAdapter Daaa = new SqlDataAdapter("SELECT * FROM [tblAuthPages]", con);
                Daaa.Fill(Dttt);
                gvUserAuthorization.EmptyDataText = "No Records Found";
                gvUserAuthorization.DataSource = Dttt;
                //con.Open();
                gvUserAuthorization.DataBind();
                //con.Close();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void gvUserAuthorization_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox chkpages = (CheckBox)e.Row.FindControl("chkPages");
            CheckBox chkpagesview = (CheckBox)e.Row.FindControl("chkPagesView");


            con.Open();
            int id = Convert.ToInt32(gvUserAuthorization.DataKeys[e.Row.RowIndex].Values[0]);

            SqlCommand cmd = new SqlCommand("select PageName,Pages,PagesView from tblUserRoleAuthorization where ID='" + id + "' AND [UserID]='" + ddluser.SelectedItem.Value + "'", con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                Pages = dr["Pages"].ToString();
                PagesView = dr["PagesView"].ToString();
                PageName = dr["PageName"].ToString();

                dr.Close();
            }
            chkpages.Checked = Pages == "True" ? true : false;
            chkpagesview.Checked = PagesView == "True" ? true : false;
            if (PageName == "OutstandingReport.aspx" || PageName == "PartyLedgerReport.aspx" || PageName == "PurchaseReport.aspx" || PageName == "RegisterReport.aspx" || PageName == "DepartmentWiseReport.aspx" || PageName == "CommercialReport.aspx" || PageName == "CompletedOADepartmentWiseRpt.aspx" || PageName == "TestCertificate.aspx" || PageName == "DepartmentWiseRpt.aspx")
            {
                chkpages.Enabled = false;
            }
            if (PageName == "ManualOrderAcceptance.aspx" || PageName == "AuditLogDashboard.aspx" || PageName == "UserAuthorization.aspx" || PageName == "RoleMaster.aspx" || PageName == "Addusers.aspx")
            {
                chkpagesview.Enabled = false;
            }
            con.Close();

        }
    }

    protected void gvUserAuthorization_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridViewRow grv = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
        int RowIndex = grv.RowIndex;
        CheckBox chkpages = (CheckBox)gvUserAuthorization.Rows[RowIndex].FindControl("chkPages");
        CheckBox chkpagesview = (CheckBox)gvUserAuthorization.Rows[RowIndex].FindControl("chkPagesView");
        chkpages.Checked = chkpages.Checked == true ? false : true;
        chkpagesview.Checked = chkpagesview.Checked == true ? false : true;

    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnSubmit.Text == "Save")
            {
                foreach (GridViewRow g1 in gvUserAuthorization.Rows)
                {
                    string menuname = (g1.FindControl("lblMenuName") as Label).Text;
                    string pagename = (g1.FindControl("lblPageName") as Label).Text;
                    string menu = (g1.FindControl("lblMenuId") as Label).Text;
                    int userId = Convert.ToInt32(ddluser.SelectedItem.Value);
                    bool pageChk = (g1.FindControl("chkPages") as CheckBox).Checked;
                    bool pageviewChk = (g1.FindControl("chkPagesView") as CheckBox).Checked;
                    DateTime Date = DateTime.Now;
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SP_UAuthorization", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", ddluser.SelectedItem.Value);
                    cmd.Parameters.AddWithValue("@UserName", ddluser.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@menuId", menu);
                    cmd.Parameters.AddWithValue("@MenuName", menuname);
                    cmd.Parameters.AddWithValue("@PageName", pagename);
                    //cmd.Parameters.AddWithValue("@createdBy", Session["name"].ToString());
                    cmd.Parameters.AddWithValue("@CreatedDate", Date);
                    cmd.Parameters.AddWithValue("@Pages", pageChk);
                    cmd.Parameters.AddWithValue("@PagesView", pageviewChk);
                    cmd.Parameters.AddWithValue("@Action", "Insert");
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Pages Authorized Save Successfully..!!'); window.location='UAuthorization.aspx';", true);
            }
            else if (btnSubmit.Text == "Update")
            {
                foreach (GridViewRow g1 in gvUserAuthorization.Rows)
                {
                    string menuname = (g1.FindControl("lblMenuName") as Label).Text;
                    string pagename = (g1.FindControl("lblPageName") as Label).Text;
                    string menu = (g1.FindControl("lblMenuId") as Label).Text;
                    bool pageChk = (g1.FindControl("chkPages") as CheckBox).Checked;
                    bool pageviewChk = (g1.FindControl("chkPagesView") as CheckBox).Checked;
                    int userId = Convert.ToInt32(ddluser.SelectedItem.Value);
                    DateTime Date = DateTime.Now;
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SP_UAuthorization", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.Parameters.AddWithValue("@UserName", ddluser.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@menuId", menu);
                    cmd.Parameters.AddWithValue("@MenuName", menuname);
                    cmd.Parameters.AddWithValue("@PageName", pagename);
                    //cmd.Parameters.AddWithValue("@UpdatedBy", Session["name"].ToString());
                    cmd.Parameters.AddWithValue("@updatedDate", Date);
                    cmd.Parameters.AddWithValue("@Pages", pageChk);
                    cmd.Parameters.AddWithValue("@PagesView", pageviewChk);
                    cmd.Parameters.AddWithValue("@Action", "Update");
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Pages Authorized Update Successfully..!!'); window.location='UAuthorization.aspx';", true);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("UAuthorization.aspx");
    }
}

   
   

