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
    public partial class VentanaConfirmarBorrCaja : Form
    {
        OracleConnection conn = DataAccess.getConn();
        private static string strComm = null;
        OracleCommand comm = null;
        public VentanaConfirmarBorrCaja()
        {
            InitializeComponent();
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            string strCom = "SELECT codigocierrecaja FROM caja WHERE codigocierrecaja = '" + VentanaRegistroCajas.CodigoCaja + "' AND ROWNUM <= 1";
            comm = new OracleCommand(strCom, conn); // Asignar la conexión a comm
            conn.Open(); // Abrir la conexión
            object resultado = comm.ExecuteScalar();
            conn.Close(); // Cerrar la conexión después de usarla
            if (resultado == null)
            {
                MessageBox.Show(" ¡¡ERROR!!, No existe la caja");

            }
            else
            {
                string deleteCommand = "DELETE FROM caja WHERE codigocierrecaja = '" + VentanaRegistroCajas.CodigoCaja + "'";
                comm = new OracleCommand(deleteCommand, conn);
                conn.Open();
                int rowsAffected = comm.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Se a Eliminado la caja con Éxito");
            }
        }

        private void VentanaConfirmarBorrCaja_Load(object sender, EventArgs e)
        {
            CenterToParent();
        }
    }


    
}
