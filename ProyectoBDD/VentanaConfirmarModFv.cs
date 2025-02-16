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
    public partial class VentanaConfirmarModFv : Form
    {
        static OracleConnection conn = DataAccess.getConn();
        OracleCommand comm = null;
        public VentanaConfirmarModFv()
        {
            InitializeComponent();
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                comm.ExecuteNonQuery();
                MessageBox.Show("La factura fue Modificada con éxito");
                this.btnConfirmar.Enabled = false;
            }
            catch (OracleException ex)
            {

                MessageBox.Show("OracleException con código de error: " + ex.ErrorCode + "\nDetalles: " + ex.Message);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Excepción no manejada: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void VentanaConfirmarModFv_Load(object sender, EventArgs e)
        {
            CenterToParent();
            conn.Open();
            string strComm = "sp_ModificarFactura";
            comm = new OracleCommand(strComm, conn);
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add(new OracleParameter("p_NumFactura", OracleType.Number)).Value = Convert.ToInt32(VentanaRegistroVentas.NumeroFactura);
            comm.Parameters.Add(new OracleParameter("p_Total", OracleType.Number)).Value = decimal.Parse(VentanaRegistroVentas.montototal);
            comm.Parameters.Add(new OracleParameter("p_Iva", OracleType.Number)).Value = decimal.Parse(VentanaRegistroVentas.iva);
            comm.Parameters.Add(new OracleParameter("p_IdCliente", OracleType.VarChar)).Value = VentanaRegistroVentas.cicliente;
            comm.Parameters.Add(new OracleParameter("p_ModoPago", OracleType.VarChar)).Value = VentanaRegistroVentas.ModoPago;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
