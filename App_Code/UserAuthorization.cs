using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.SessionState;
using System.Collections;
using System.Web.UI;



    /// <summary>
    /// Summary description for UserAuthorization
    /// </summary>
    public class UserAuthorization
    {
        public UserAuthorization()
        {

        }

        public static SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);

     
        HttpContext context = HttpContext.Current;
        public ArrayList GetPageAuthori(string pageName)
        {
            ArrayList arrlist = new ArrayList();
            if (context.Session["adminname"] != null)
            {

                string username = context.Session["adminname"].ToString(); 

              DataTable dt = new DataTable();
                SqlCommand cmd1 = new SqlCommand("SELECT [ID],[UserID],UserName,[menuId],[MenuName],[Create_R],[Update_R],[View_R],[Delete_R],[Report_R],[createdBy],[CreatedDate],[UpdatedBy],[updatedDate],[IsActive] FROM [UserAuthorization_table] where UserName='" + username + "'", con);
                SqlDataAdapter sad = new SqlDataAdapter(cmd1);
                sad.Fill(dt);

                if (dt.Rows.Count > 0)
                {

                    foreach (DataRow row in dt.Rows)
                    {


                        string MenuName = row["MenuName"].ToString();
                        if (MenuName == pageName)
                        {
                            string create1 = row["Create_R"].ToString();
                            string Delete1 = row["Delete_R"].ToString();
                            string Update1 = row["Update_R"].ToString();
                            string view1 = row["View_R"].ToString();
                            string Report1 = row["Report_R"].ToString();


                            //arrlist[0] = row["create1"].ToString();
                            //arrlist[1] = row["Delete1"].ToString();
                            //arrlist[2] = row["Update1"].ToString();
                            //arrlist[3] = row["view1"].ToString();
                            //arrlist[4] = row["Report1"].ToString();
                            arrlist.Add(create1);
                            arrlist.Add(Delete1);
                            arrlist.Add(Update1);
                            arrlist.Add(view1);
                            arrlist.Add(Report1);

                        }

                    }
                    con.Close();
                }

            }
            else
            {
                context.Response.Redirect("LoginPage.aspx");
            }
            return arrlist;
        }

    }
