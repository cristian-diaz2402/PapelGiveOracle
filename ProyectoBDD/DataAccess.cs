using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoBDD
{
    internal class DataAccess
    {
        static string strConn = "DATA SOURCE = orcl; PASSWORD = oracle; USER ID = quito;";
        public static OracleConnection conn = new OracleConnection(strConn);


        public static OracleConnection getConn()
        {
            return conn;

        }
        public static OracleCommand getComm(string strComm)
        {
            OracleCommand comm = new OracleCommand(strConn);
            return comm;
        }
    }
}
