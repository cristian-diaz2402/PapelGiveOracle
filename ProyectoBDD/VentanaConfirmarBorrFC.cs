using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.OracleClient;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoBDD
{
    public partial class VentanaConfirmarBorrFC : Form
    {
        static OracleConnection conn = DataAccess.getConn();
        OracleCommand comm = null;
        public VentanaConfirmarBorrFC()
        {
            InitializeComponent();
        }


        private void btnRegresar_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            string strCom = "SELECT num_orden FROM numeroordencompra WHERE num_orden = '" + VentanaRegistroCompras.NumeroFactura + "' AND ROWNUM <= 1";
            comm = new OracleCommand(strCom, conn);
            conn.Open();
            object resultado = comm.ExecuteScalar();
            conn.Close();

            if (resultado == null)
            {
                MessageBox.Show(" ¡¡ERROR!!, No existe la compra");
            }
            else
            {
                string strComm = "BEGIN sp_EliminarCompra(:p_NumFactura); END;";

                OracleCommand cmd = new OracleCommand(strComm, conn);
                cmd.CommandType = CommandType.Text;

                // Crear parámetro de entrada
                OracleParameter paramBuscar = new OracleParameter(":p_NumFactura", OracleType.NVarChar);
                paramBuscar.Value = VentanaRegistroCompras.NumeroFactura;
                cmd.Parameters.Add(paramBuscar);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    // La transacción se maneja automáticamente en el procedimiento almacenado
                    MessageBox.Show("Se a Eliminado la compra con Éxito");
                }
                catch (Exception ex)
                {
                    // Manejar la excepción según sea necesario
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void VentanaConfirmarBorrFC_Load(object sender, EventArgs e)
        {
            CenterToParent();
        }
    }
}
