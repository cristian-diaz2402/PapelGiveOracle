using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OracleClient;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ProyectoBDD
{
    public partial class VentanaConfirmarBorrProv : Form
    {
        public VentanaConfirmarBorrProv()
        {
            InitializeComponent();
        }
        OracleConnection conn = DataAccess.getConn();
        private static string strComm = null;
        OracleCommand comm = null;
        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            string strCom = "SELECT id_proveedor FROM proveedores_uio WHERE id_proveedor = '" + VentanaProveedores.RUC + "' AND ROWNUM <= 1";
            comm = new OracleCommand(strCom, conn); // Asignar la conexión a comm
            conn.Open(); // Abrir la conexión
            object resultado = comm.ExecuteScalar();
            conn.Close(); // Cerrar la conexión después de usarla
            if (resultado == null)
            {
                MessageBox.Show(" ¡¡ERROR!!, No existe el Proveedor");

            }
            else
            {
                string deleteCommand = "DELETE FROM Proveedores_uio WHERE id_proveedor = '" + VentanaProveedores.RUC + "'";
                comm = new OracleCommand(deleteCommand, conn);
                conn.Open();
                int rowsAffected = comm.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Se a Eliminado el Proveedor con Éxito");
            }
        }

            private void VentanaConfirmarBorrProv_Load(object sender, EventArgs e)
            {
                    CenterToParent();
        }
        }
    } 

