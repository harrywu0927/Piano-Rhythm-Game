using System;
using System.Collections.Generic;
using System.Text;
using SocketGameProtocol;
using System.Data;
using System.Data.SqlClient;
namespace SocketServer.DAO
{
    class DBUtil
    {
        public static string connString = "Data Source=HARRYWU的计算机;Initial Catalog=PianoGame;Integrated Security=TRUE";
        
        
        public static SqlConnection getConnection()
        {
            SqlConnection conn = new SqlConnection(connString);
            try
            {
                conn.Open();
            }
            catch(Exception e)
            {
                Console.WriteLine("Failed to connect to database:"+e.Message);
            }
            return conn;
        }
    }
}
