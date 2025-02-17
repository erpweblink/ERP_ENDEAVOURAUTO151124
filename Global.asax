<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e)
    {
        //Class1 obj=new Class1();

        //System.Timers.Timer myTimer = new System.Timers.Timer();
        //// Set the Interval to 5 seconds (5000 milliseconds).
        //myTimer.Interval = 300000; // 5mins
        //myTimer.AutoReset = true;
        //myTimer.Elapsed += new ElapsedEventHandler(myTimer_Elapsed);
        //myTimer.Enabled = true; 
    }

    public void myTimer_Elapsed(object source, System.Timers.ElapsedEventArgs e)
    {
        //clsScheduleMail objScheduleMail = new clsScheduleMail();
        //objScheduleMail.SendScheduleMail();
    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown

    }

    void Application_Error(object sender, EventArgs e)
    {
        Exception exception = Server.GetLastError();
        Server.ClearError();
        string errorMessage = exception.InnerException.Message;
        string encodedErrorMessage = Server.UrlEncode(errorMessage);
        string currentPageUrl = Request.Url.AbsoluteUri;
        string encodedCurrentPageUrl = Server.UrlEncode(currentPageUrl);

        Response.Redirect("~/ErrorPage.aspx?msg=" + encodedErrorMessage + "&url=" + encodedCurrentPageUrl);

    }

    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started
        //if (Session["ProductionName"] == null)
        //{
        //    //Redirect to Welcome Page if Session is not null    
        //  Response.Redirect("Login.aspx");
        //}


    }

    void Session_End(object sender, EventArgs e)
    {

        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.
    }


</script>
