using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_IssuedComponent : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
    DataTable dtcomponant = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["RowNo"] = 0;

        dtcomponant.Columns.AddRange(new DataColumn[5] { new DataColumn("No"),new DataColumn("Jobno"), new DataColumn("CompId"),
                new DataColumn("CompName"),new DataColumn("Quantity")
            });
        ViewState["CompData"] = dtcomponant;
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
                com.CommandText = "select DISTINCT CustomerName,Custid from tblCustomer where " + "CustomerName like @Search + '%' " + "AND isdeleted = '0' AND IsStatus = '1'";

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


    protected void txtjobno_TextChanged(object sender, EventArgs e)
    {
        DataTable Dt = new DataTable();
        SqlDataAdapter Daa = new SqlDataAdapter("SELECT ProductName,EngiName FROM [tblTestingProduct] WHERE CustomerName='" + txtcustomername.Text + "' AND JobNo = '" + txtjobno.Text + "'", con);
        Daa.Fill(Dt);
        if (Dt.Rows.Count > 0)
        {
            txtproductname.Text = Dt.Rows[0]["ProductName"].ToString();
            txtengeername.Text = Dt.Rows[0]["EngiName"].ToString();       
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
                    dt.Rows.Add(ViewState["RowNo"], txtjobno.Text, txtcomponent.Text, txtcomponent.Text, txtQuantityComp.Text);
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
}