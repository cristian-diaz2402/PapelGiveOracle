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
    public partial class VentanaConfirmarBorrFV : Form
    {
        public VentanaConfirmarBorrFV()
        {
            InitializeComponent();
        }
        OracleConnection conn = DataAccess.getConn();
        private static string strComm = null;
        OracleCommand comm = null;

        private void VentanaConfirmarBorrFV_Load(object sender, EventArgs e)
        {
            CenterToParent();
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
    
                string strCom = "SELECT num_factura FROM facturasv WHERE num_factura = '" + VentanaRegistroVentas.NumeroFactura + "' AND ROWNUM <= 1";
                comm = new OracleCommand(strCom, conn);
                conn.Open();
                object resultado = comm.ExecuteScalar();
                conn.Close();

                if (resultado == null)
                {
                    MessageBox.Show(" ¡¡ERROR!!, No existe la factura");
                }
                else
                {
                string strComm = "BEGIN sp_EliminarFactura(:p_NumFactura); END;";

                OracleCommand cmd = new OracleCommand(strComm, conn);
                cmd.CommandType = CommandType.Text;

                // Crear parámetro de entrada
                OracleParameter paramBuscar = new OracleParameter(":p_NumFactura", OracleType.NVarChar);
                paramBuscar.Value = VentanaRegistroVentas.NumeroFactura;
                cmd.Parameters.Add(paramBuscar);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    // La transacción se maneja automáticamente en el procedimiento almacenado
                    MessageBox.Show("Se a Eliminado la factura con Éxito");
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

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
