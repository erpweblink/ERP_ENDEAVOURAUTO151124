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
using System.Web.Security;

public partial class LoginPage : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            if (Request.Cookies["stsuserid"] != null)
                txtusername.Text = Request.Cookies["stsuserid"].Value;

            if (Request.Cookies["stspwd"] != null)
                txtpassword.Attributes.Add("value", Request.Cookies["stspwd"].Value);
        }
    }

    protected void submit_Click(object sender, EventArgs e)
    {
        SqlCommand cmd = new SqlCommand("SELECT * FROM LogIn WHERE name='" + txtusername.Text + "' AND pass='" + txtpassword.Text + "'", con);
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddWithValue("@name", txtusername.Text.Trim());
        cmd.Parameters.AddWithValue("@pass", txtpassword.Text.Trim()); cmd.Connection.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            while (dr.Read())
            {
                string Username = dr["name"].ToString();
                string Role = dr["Role"].ToString();
                string status = dr["IsActive"].ToString();
                if (status == "True")
                {
                    if (!string.IsNullOrEmpty(Username))
                    {
                      
                        Response.Cookies["Excuserid"].Expires = DateTime.Now.AddDays(-1);
                        Response.Cookies["Excpwd"].Expires = DateTime.Now.AddDays(-1);

                        Session["Id"] = dr["Id"].ToString();
                        string roleName = dr["role"].ToString();
                        Session["adminname"] = roleName;
                        Session["name"] = dr["name"].ToString();

                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabe('Login Successfully..!!')", true);
                    }
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Login Failed, Activate Your Account First..!!');", true);               
                    txtusername.Text = ""; txtpassword.Text = "";
                }
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel('Please Enter Correct Username And Password');", true);
            txtusername.Text = ""; txtpassword.Text = ""; 
        }
        cmd.Connection.Close();
    }
}