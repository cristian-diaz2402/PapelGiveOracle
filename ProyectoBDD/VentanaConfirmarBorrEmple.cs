using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoBDD
{
    public partial class VentanaConfirmarBorrEmple : Form
    {
        public VentanaConfirmarBorrEmple()
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
            string strCom = "SELECT id_empleado FROM empleados_uio WHERE id_empleado = '" + VentanaEmpleados.Cedula + "' AND ROWNUM <= 1";
            comm = new OracleCommand(strCom, conn); // Asignar la conexión a comm
            conn.Open(); // Abrir la conexión
            object resultado = comm.ExecuteScalar();
            conn.Close(); // Cerrar la conexión después de usarla
            if (resultado == null)
            {
                MessageBox.Show(" ¡¡ERROR!!, No existe el Empleados");

            }
            else
            {
                string deleteCommand = "DELETE FROM empleados_uio WHERE id_empleado = '" + VentanaEmpleados.Cedula + "'";
                comm = new OracleCommand(deleteCommand, conn);
                conn.Open();
                int rowsAffected = comm.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Se a Eliminado el Empleado con Éxito");
            }
        }

        private void VentanaConfirmarBorrEmple_Load(object sender, EventArgs e)
        {
            CenterToParent();
        }
    }
}
