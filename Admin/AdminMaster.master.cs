using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_MasterPage : System.Web.UI.MasterPage
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["name"] == null)
        {
            Response.Redirect("../LoginPage.aspx");
        }
        else
        {
            PageAuthorization();
            lbluser.Text = Session["name"].ToString();
        }

        //if(Session["adminname"]!=null)
        //{
        //    string str = Session["name"].ToString();
        //    string[] arrstr = str.ToString().Split(' ');
        //    lbluser.Text = arrstr[0].ToString();
        //}
        //else
        //{
        //    Response.Redirect("../LoginPage.aspx");
        //}
    }

    protected void PageAuthorization()
    {
        string username = Session["name"].ToString();
        DataTable dt = new DataTable();
        SqlCommand cmd1 = new SqlCommand("SELECT * FROM [tblUserRoleAuthorization] where UserName='" + username + "'", con);
        SqlDataAdapter sad = new SqlDataAdapter(cmd1);
        sad.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                string MenuName = row["PageName"].ToString();               
                if (MenuName == "CustomerList.aspx")
                {
                    string page1 = row["Pages"].ToString();
                    string pageView = row["PagesView"].ToString();
                    if (page1 == "False" && pageView == "False")
                    {
                        Customerid.Visible = false;
                    }
                    else
                    {
                        Customerid.Visible = true;
                    }
                }
                if (MenuName == "ProductList.aspx")
                {
                    string page1 = row["Pages"].ToString();
                    string pageView = row["PagesView"].ToString();
                    if (page1 == "False" && pageView == "False")
                    {
                        Productid.Visible = false;
                    }
                    else
                    {
                        Productid.Visible = true;
                    }
                }
                if (MenuName == "UserMasterList.aspx")
                {
                    string page1 = row["Pages"].ToString();
                    string pageView = row["PagesView"].ToString();
                    if (page1 == "False" && pageView == "False")
                    {
                        Userid.Visible = false;
                    }
                    else
                    {
                        Userid.Visible = true;
                    }
                }
                if (MenuName == "VendorList.aspx")
                {
                    string page1 = row["Pages"].ToString();
                    string pageView = row["PagesView"].ToString();
                    if (page1 == "False" && pageView == "False")
                    {
                        vendorid.Visible = false;
                    }
                    else
                    {
                        vendorid.Visible = true;
                    }
                }
               
                if (MenuName == "ComponentList.aspx")
                {
                    string page1 = row["Pages"].ToString();
                    string pageView = row["PagesView"].ToString();
                    if (page1 == "False" && pageView == "False")
                    {
                        componantid.Visible = false;
                    }
                    else
                    {
                        componantid.Visible = true;
                    }
                }
                if (MenuName == "RoleList.aspx")
                {
                    string page1 = row["Pages"].ToString();
                    string pageView = row["PagesView"].ToString();
                    if (page1 == "False" && pageView == "False")
                    {
                        Roleid.Visible = false;
                    }
                    else
                    {
                        Roleid.Visible = true;
                    }
                }
                if (MenuName == "JOBcardList.aspx")
                {
                    string page1 = row["Pages"].ToString();
                    string pageView = row["PagesView"].ToString();
                    if(page1 == "False" && pageView == "False")
                    {
                        jobcardid.Visible = false;
                    }
                    else
                    {
                        jobcardid.Visible = true;
                    }
                }
                if (MenuName == "StaffList.aspx")
                {
                    string page1 = row["Pages"].ToString();
                    string pageView = row["PagesView"].ToString();
                    if (page1 == "False" && pageView == "False")
                    {
                        staffid.Visible = false;
                    }
                    else
                    {
                        staffid.Visible = true;
                    }
                }
                //if (MenuName == "GSTTaxList.aspx")
                //{
                //    string page1 = row["Pages"].ToString();
                //    string pageView = row["PagesView"].ToString();
                //    if (page1 == "False")
                //    {
                //        gstmasterid.Visible = false;
                //    }
                //    else
                //    {
                //        gstmasterid.Visible = true;
                //    }
                //}
                //if (MenuName == "BankList.aspx")
                //{
                //    string page1 = row["Pages"].ToString();
                //    string pageView = row["PagesView"].ToString();
                //    if (page1 == "False")
                //    {
                //        bankmasterid.Visible = false;
                //    }
                //    else
                //    {
                //        bankmasterid.Visible = true;
                //    }
                //}
                //if (MenuName == "BankList.aspx")
                //{
                //    string page1 = row["Pages"].ToString();
                //    string pageView = row["PagesView"].ToString();
                //    if (pageView == "false" || page1 == "false")
                //    {
                //        bankmasterid.Visible = false;
                //    }
                //    else
                //    {
                //        bankmasterid.Visible = true;
                //    }
                //} 

                if (MenuName == "inwardEntryList.aspx")
                {
                    string page1 = row["Pages"].ToString();
                    string pageView = row["PagesView"].ToString();
                    if (page1 == "False" && pageView == "False")
                    {
                        inwardentryid.Visible = false;
                    }
                    else
                    {
                        inwardentryid.Visible = true;
                    }
                }

                if (MenuName == "OutwardEntryList.aspx")
                {
                    string page1 = row["Pages"].ToString();
                    string pageView = row["PagesView"].ToString();
                    if (page1 == "False")
                    {
                        outwardentryid.Visible = false;
                    }
                    else
                    {
                        outwardentryid.Visible = true;
                    }
                }
                if (MenuName == "OutwardEntryList.aspx")
                {
                    string page1 = row["Pages"].ToString();
                    string pageView = row["PagesView"].ToString();
                    if (page1 == "False")
                    {
                        outwardentryid.Visible = false;
                    }
                    else
                    {
                        outwardentryid.Visible = true;
                    }
                }
                if (MenuName == "UAuthorization.aspx")
                {
                    string page1 = row["Pages"].ToString();
                    string pageView = row["PagesView"].ToString();
                    if (page1 == "False")
                    {
                        UAuthorizationid.Visible = false;
                    }
                    else
                    {
                        UAuthorizationid.Visible = true;
                    }
                }
                if (MenuName == "Evalution.aspx")
                {
                    string page1 = row["Pages"].ToString();
                    string pageView = row["PagesView"].ToString();
                    if (page1 == "False" && pageView == "False")
                    {
                        evalutionid.Visible = false;
                    }
                    else
                    {
                        evalutionid.Visible = true;
                    }
                }
                if (MenuName == "EstimationList.aspx")
                {
                    string page1 = row["Pages"].ToString();
                    string pageView = row["PagesView"].ToString();
                    if (page1 == "False" && pageView == "False")
                    {
                        estimationid.Visible = false;
                    }
                    else
                    {
                        estimationid.Visible = true;
                    }
                }
                if (MenuName == "Quotation_List.aspx")
                {
                    string page1 = row["Pages"].ToString();
                    string pageView = row["PagesView"].ToString();
                    if (page1 == "False" && pageView == "False")
                    {
                        QuatationListid.Visible = false;
                    }
                    else
                    {
                        QuatationListid.Visible = true;
                    }
                }
                if (MenuName == "PurchaseOrderList.aspx")
                {
                    string page1 = row["Pages"].ToString();
                    string pageView = row["PagesView"].ToString();
                    if (page1 == "False" && pageView == "False")
                    {
                        purchaseorderlistid.Visible = false;
                    }
                    else
                    {
                        purchaseorderlistid.Visible = true;
                    }
                }
                if (MenuName == "CustomerPO_List.aspx")
                {
                    string page1 = row["Pages"].ToString();
                    string pageView = row["PagesView"].ToString();
                    if (page1 == "False" && pageView == "False")
                    {
                        CustomerPOBoth.Visible = false;
                    }
                    else
                    {
                        CustomerPOBoth.Visible = true;
                    }
                }
                if (MenuName == "TaxInvoiceList.aspx")
                {
                    string page1 = row["Pages"].ToString();
                    string pageView = row["PagesView"].ToString();
                    if (page1 == "False")
                    {
                        Iaxinvoicelistid.Visible = false;
                    }
                    else
                    {
                        Iaxinvoicelistid.Visible = true;
                    }
                }
                if (MenuName == "DeliveryChallanList.aspx")
                {
                    string page1 = row["Pages"].ToString();
                    string pageView = row["PagesView"].ToString();
                    if (page1 == "False" && pageView == "False")
                    {
                        deliverychallanid.Visible = false;
                    }
                    else
                    {
                        deliverychallanid.Visible = true;
                    }
                }
                if (MenuName == "SalesProformaList.aspx")
                {
                    string page1 = row["Pages"].ToString();
                    string pageView = row["PagesView"].ToString();
                    if (page1 == "False" && pageView == "False")
                    {
                        proformaid.Visible = false;
                    }
                    else
                    {
                        proformaid.Visible = true;
                    }
                }
                if (MenuName == "PurchaseInvoiceList.aspx")
                {
                    string page1 = row["Pages"].ToString();
                    string pageView = row["PagesView"].ToString();
                    if (page1 == "False" && pageView == "False")
                    {
                        //invoicepoid.Visible = false;
                    }
                    else
                    {
                      //  invoicepoid.Visible = true;
                    }
                }
                if (MenuName == "CreditNotesSalesList.aspx")
                {
                    string page1 = row["Pages"].ToString();
                    string pageView = row["PagesView"].ToString();
                    if (page1 == "False" && pageView == "False")
                    {
                        //creditnotesid.Visible = false;
                    }
                    else
                    {
                       // creditnotesid.Visible = true;
                    }
                }
                if (MenuName == "DebitNoteList.aspx")
                {
                    string page1 = row["Pages"].ToString();
                    string pageView = row["PagesView"].ToString();
                    if (page1 == "False" && pageView == "False")
                    {
                      //  debitnotesid.Visible = false;
                    }
                    else
                    {
                       // debitnotesid.Visible = true;
                    }
                }
                if (MenuName == "PaymentVoucherDetails.aspx")
                {
                    string page1 = row["Pages"].ToString();
                    string pageView = row["PagesView"].ToString();
                    if (page1 == "False" && pageView == "False")
                    {
                       // Payvoucherid.Visible = false;
                    }
                    else
                    {
                       // Payvoucherid.Visible = true;
                    }
                }
                if (MenuName == "ReceiptVoucherDetails.aspx")
                {
                    string page1 = row["Pages"].ToString();
                    string pageView = row["PagesView"].ToString();
                    if (page1 == "False" && pageView == "False")
                    {
                       // Receiptvoucherid.Visible = false;
                    }
                    else
                    {
                      //  Receiptvoucherid.Visible = true;
                    }
                }
                if (MenuName == "EvalutionReportGrid.aspx")
                {
                    string page1 = row["Pages"].ToString();
                    string pageView = row["PagesView"].ToString();
                    if (page1 == "False" && pageView == "False")
                    {
                        evalereportid.Visible = false;
                    }
                    else
                    {
                        evalereportid.Visible = true;
                    }
                }
                if (MenuName == "QuatationReport.aspx")
                {
                    string page1 = row["Pages"].ToString();
                    string pageView = row["PagesView"].ToString();
                    if (page1 == "False" && pageView == "False")
                    {
                        QuatationReportid.Visible = false;
                    }
                    else
                    {
                        QuatationReportid.Visible = true;
                    }
                }
                if (MenuName == "PurchaseOrderReport.aspx")
                {
                    string page1 = row["Pages"].ToString();
                    string pageView = row["PagesView"].ToString();
                    if (page1 == "False" && pageView == "False")
                    {
                        poreportid.Visible = false;
                    }
                    else
                    {
                        poreportid.Visible = true;
                    }
                }
                if (MenuName == "CustomerPO_Report.aspx")
                {
                    string page1 = row["Pages"].ToString();
                    string pageView = row["PagesView"].ToString();
                    if (page1 == "False" && pageView == "False")
                    {
                        customerreportid.Visible = false;
                    }
                    else
                    {
                        customerreportid.Visible = true;
                    }
                }
                if (MenuName == "Taxinvoicereport.aspx")
                {
                    string page1 = row["Pages"].ToString();
                    string pageView = row["PagesView"].ToString();
                    if (page1 == "False" && pageView == "False")
                    {
                        invoicereportid.Visible = false;
                    }
                    else
                    {
                        invoicereportid.Visible = true;
                    }
                }
                if (MenuName == "InwardReport.aspx")
                {
                    string page1 = row["Pages"].ToString();
                    string pageView = row["PagesView"].ToString();
                    if (page1 == "False" && pageView == "False")
                    {
                        inwardreportid.Visible = false;
                    }
                    else
                    {
                        inwardreportid.Visible = true;
                    }
                }
                if (MenuName == "OutwardReport.aspx")
                {
                    string page1 = row["Pages"].ToString();
                    string pageView = row["PagesView"].ToString();
                    if (page1 == "False" && pageView == "False")
                    {
                        outwardreportid.Visible = false;
                    }
                    else
                    {
                        outwardreportid.Visible = true;
                    }
                }
                if (MenuName == "DeliveryChallanReport.aspx")
                {
                    string page1 = row["Pages"].ToString();
                    string pageView = row["PagesView"].ToString();
                    if (page1 == "False" && pageView == "False")
                    {
                        deliveryreportid.Visible = false;
                    }
                    else
                    {
                        deliveryreportid.Visible = true;
                    }
                }
                if (MenuName == "OutstandingReport.aspx")
                {
                    string page1 = row["Pages"].ToString();
                    string pageView = row["PagesView"].ToString();
                    if (page1 == "False" && pageView == "False")
                    {
                        //OutstandingReportid.Visible = false;
                    }
                    else
                    {
                        //OutstandingReportid.Visible = true;
                    }
                }
				if (MenuName == "MasterGridview.aspx")
                {
                    string page1 = row["Pages"].ToString();
                    string pageView = row["PagesView"].ToString();
                    if (page1 == "False" && pageView == "False")
                    {
                        masterlistgrid.Visible = false;
                    }
                    else
                    {
                        masterlistgrid.Visible = true;
                    }
                }
				
				
            }
        }

    }




}
