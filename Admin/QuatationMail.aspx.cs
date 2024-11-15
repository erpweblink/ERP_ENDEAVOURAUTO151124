using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;


public partial class Admin_QuatationMail : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);

    string QuatationNo ="QN9";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Gridview();
        }
    }
    protected void Gridview()
    {
        try
        {
            DataTable dt = new DataTable();
            SqlDataAdapter sad = new SqlDataAdapter("select * from tbl_Quotation_Dtl where Quotation_no='" + QuatationNo + "'", con);
            sad.Fill(dt);
            grd.DataSource = dt;
            grd.DataBind();
        }
        catch (Exception)
        {

            throw;
        }
    }
}