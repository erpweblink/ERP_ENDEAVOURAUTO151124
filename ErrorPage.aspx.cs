using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Org.BouncyCastle.Utilities;

public partial class ErrorPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        string errorMessage = Request.QueryString["msg"];
        string currentPageUrl = Request.QueryString["url"];

        if (!string.IsNullOrEmpty(errorMessage))
        {
            lblErrorMessage.Text ="Error: "+ errorMessage + currentPageUrl;
        }
    }

}