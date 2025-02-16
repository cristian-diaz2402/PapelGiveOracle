using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoBDD
{
    public partial class VentanaAuditorias : Form
    {
        OracleConnection conn = DataAccess.getConn();
        private static string strComm = null;
        OracleCommand comm = null;
        OracleCommand com = null;

        public VentanaAuditorias()
        {
            InitializeComponent();
        }



        private void VentanaAuditorias_Load(object sender, EventArgs e) { 
       
            CenterToParent();
            string strComm = "select * from auditorias";
            OracleCommand cmd = new OracleCommand(strComm, conn);
            // Change the CommandType to Text for a regular SQL statement
            cmd.CommandType = CommandType.Text;
            OracleDataAdapter adaptador = new OracleDataAdapter(cmd);
            DataTable tabla = new DataTable();
            adaptador.Fill(tabla);
            dataGridViewAuditorias.DataSource = tabla;
            dataGridViewAuditorias.ReadOnly = true;
        }




        private void btnRegresar_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
