using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace EndevourAutomation.App_Code
{
    public class userAuth1
    {
        SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
       public bool createUser, updateUser, deleteUser, readUser;
       public List<bool> Authrization1(string pagename , int userid)
        {
            using (SqlCommand cmd = new SqlCommand("select [Create],[Update],[Delete],[View] from UserAuthorization_table where Pagename=\'" + pagename + "\' and UserId= " + userid + " "))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con1;
                con1.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    sdr.Read();
                    createUser = Convert.ToBoolean(sdr["Create"]);
                   
                    updateUser = Convert.ToBoolean(sdr["Update"]);
                   
                    deleteUser = Convert.ToBoolean(sdr["Delete"]);
                    readUser = Convert.ToBoolean(sdr["View"]);

                    
                }
                con1.Close();
            }

            List<bool> listRange = new List<bool>();
            listRange.Add(createUser);
            listRange.Add(updateUser);
            listRange.Add(deleteUser);
            listRange.Add(readUser);

            return listRange;

        }

      
    }
}