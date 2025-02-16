using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoBDD
{
    public partial class VentanaConfirmarCompra : Form
    {
        public VentanaConfirmarCompra()
        {
            InitializeComponent();
        }
        static OracleConnection conn = DataAccess.getConn();
        OracleCommand comm = null;
        OracleCommand com = null;
        OracleCommand co = null;
        private void btnRegresar_Click(object sender, EventArgs e)
        {
            conn.Close();
            this.Close();
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                comm.ExecuteNonQuery();
                com.ExecuteNonQuery();
                co.ExecuteNonQuery();
                MessageBox.Show("La compra fue realizada exitosamente");
                this.btnConfirmar.Enabled = false;
            }
            catch (OracleException ex)
            {
                if (ex.Message.Contains("ORA-00001"))
                {
                    MessageBox.Show(" ¡¡ERROR!!, La compra ya existe");
                    this.btnConfirmar.Enabled = false;
                }
                else
                {
                    MessageBox.Show("OracleException con código de error: " + ex.ErrorCode + "\nDetalles: " + ex.Message);
                }
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

        private void VentanaConfirmarCompra_Load(object sender, EventArgs e)
        {
            CenterToParent();
            conn.Close();
            Random random = new Random();
            double numeroAleatorio = random.NextDouble() * 99 + 1;
            double VxP = Math.Round(numeroAleatorio, 2);

            double cant = int.Parse(VentanaCompras.Cantidad);
            double cantidad = Math.Round(cant, 2);

            double total1 = (cantidad * VxP);
            double semitotal = Math.Round(total1, 2);

            double iv = (semitotal * 12) / 100;
            double iva = Math.Round(iv, 2);

            double total2 = (semitotal + iva);
            double totalfinal = Math.Round(total2, 2);

            txtIva.Text = iva.ToString();
            txtPrecioPaquete.Text = VxP.ToString();
            txtCostoPrevio.Text = semitotal.ToString();
            txtCantidad.Text = cantidad.ToString();
            txtMontoTotal.Text = totalfinal.ToString();
            CenterToParent();
            conn.Open();
            decimal Totaldodecimal = decimal.Parse(txtMontoTotal.Text);
            decimal IvaDecimal = decimal.Parse(txtIva.Text);
            string strComm = "sp_Comprar";
            comm = new OracleCommand(strComm, conn);
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add(new OracleParameter("p_NumeroOrden", OracleType.Number)).Value = Convert.ToInt32(VentanaCompras.NumeroOrden);
            comm.Parameters.Add(new OracleParameter("p_Total", OracleType.Number)).Value = Totaldodecimal;
            comm.Parameters.Add(new OracleParameter("p_IVA", OracleType.Number)).Value = IvaDecimal;
            comm.Parameters.Add(new OracleParameter("p_Fecha", OracleType.DateTime)).Value = VentanaCompras.Fecha;
            comm.Parameters.Add(new OracleParameter("p_ModoPago", OracleType.VarChar)).Value = VentanaCompras.ModoPago;

            string Comm = "sp_ItemC";
            com = new OracleCommand(Comm, conn);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.Add(new OracleParameter("p_Cantidad", OracleType.Number)).Value = Convert.ToInt32(VentanaCompras.Cantidad);
            com.Parameters.Add(new OracleParameter("p_NumeroOrden", OracleType.Number)).Value = Convert.ToInt32(VentanaCompras.NumeroOrden);
            com.Parameters.Add(new OracleParameter("p_CodigoBarras", OracleType.Number)).Value = Convert.ToInt32(VentanaCompras.CodigoBarra);
            com.Parameters.Add(new OracleParameter("p_RUC", OracleType.VarChar)).Value = VentanaCompras.RUC;

            int canti = int.Parse(VentanaCompras.Cantidad);
            string Com = "sp_actualizarCantProducto";
            co = new OracleCommand(Com, conn);
            co.CommandType = CommandType.StoredProcedure;
            co.Parameters.Add(new OracleParameter("p_CodigoBarra", OracleType.Number)).Value = Convert.ToInt32(VentanaCompras.CodigoBarra);
            co.Parameters.Add(new OracleParameter("p_Cantidad", OracleType.Number)).Value = canti;
        }
    }
}
