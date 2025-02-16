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
    public partial class VentanaAuditoriaCajas : Form
    {
        OracleConnection conn = DataAccess.getConn();
        private static string strComm = null;
        OracleCommand comm = null;
        OracleCommand com = null;
        public VentanaAuditoriaCajas()
        {
            InitializeComponent();
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void VentanaAuditoriaCajas_Load(object sender, EventArgs e)
        {
            CenterToParent();
            string strComm = "select * from Auditoria_Caja";
            OracleCommand cmd = new OracleCommand(strComm, conn);
            // Change the CommandType to Text for a regular SQL statement
            cmd.CommandType = CommandType.Text;
            OracleDataAdapter adaptador = new OracleDataAdapter(cmd);
            DataTable tabla = new DataTable();
            adaptador.Fill(tabla);
            dataGridViewAudiCajas.DataSource = tabla;
            dataGridViewAudiCajas.ReadOnly = true;
        }
    }
}
