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
    public partial class VentanaConfirmarVenta : Form
    {
        OracleConnection conn = null;
        OracleCommand commm = null;
        OracleCommand comm = null;
        OracleCommand com = null;
        OracleCommand co = null;
        public VentanaConfirmarVenta()
        {
            InitializeComponent();
            conn = DataAccess.getConn();
            comm = new OracleCommand();
            com = new OracleCommand();
            co = new OracleCommand();
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            conn.Close();
            this.Close();
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                decimal Totaldodecimal = decimal.Parse(txtMontoTotal.Text);
                decimal IvaDecimal = decimal.Parse(txtIva.Text);
                int canti = int.Parse(VentanaVentas.Cantidad);

                // Configura y ejecuta el procedimiento almacenado sp_Vender
                string strComm = "sp_Vender";
                comm = new OracleCommand(strComm, conn);
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.Add(new OracleParameter("p_NumeroFactura", OracleType.Number)).Value = Convert.ToInt32(VentanaVentas.NumeroFactura);
                comm.Parameters.Add(new OracleParameter("p_Total", OracleType.Number)).Value = Totaldodecimal;
                comm.Parameters.Add(new OracleParameter("p_IVA", OracleType.Number)).Value = IvaDecimal;
                comm.Parameters.Add(new OracleParameter("p_Fecha", OracleType.DateTime)).Value = VentanaVentas.Fecha;
                comm.Parameters.Add(new OracleParameter("p_CI", OracleType.VarChar)).Value =VentanaVentas.CedulaRUC;
                comm.Parameters.Add(new OracleParameter("p_ModoPago", OracleType.VarChar)).Value = VentanaVentas.ModoPago;
                conn.Open();
                comm.ExecuteNonQuery();
                conn.Close();

                // Configura y ejecuta el procedimiento almacenado sp_ItemV
                string Comm = "sp_ItemV";
                com = new OracleCommand(Comm, conn);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.Add(new OracleParameter("p_Cantidad", OracleType.Number)).Value = canti;
                com.Parameters.Add(new OracleParameter("p_NumeroFactura", OracleType.Number)).Value = Convert.ToInt32(VentanaVentas.NumeroFactura);
                com.Parameters.Add(new OracleParameter("p_CodigoBarra", OracleType.Number)).Value = Convert.ToInt32(VentanaVentas.CodigoBarra);
                conn.Open();
                com.ExecuteNonQuery();
                conn.Close();

                // Configura y ejecuta el procedimiento almacenado sp_actualizarCantProductoV
                string Com = "sp_actualizarCantProductoV";
                co = new OracleCommand(Com, conn);
                co.CommandType = CommandType.StoredProcedure;
                co.Parameters.Add(new OracleParameter("p_CodigoBarra", OracleType.Number)).Value = Convert.ToInt32(VentanaVentas.CodigoBarra);
                co.Parameters.Add(new OracleParameter("p_Cantidad", OracleType.Number)).Value = canti;
                conn.Open();
                co.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("La venta fue realizada exitosamente");
                this.btnConfirmar.Enabled = false;
            }
            catch (OracleException ex)
            {
                if (ex.Message.Contains("ORA-02291")) // ORA-02291: Integridad referencial violada - clave externa no encontrada
                {
                    MessageBox.Show("¡¡ERROR!!, El cliente no existe");
                    this.btnConfirmar.Enabled = false;
                }
                else 
                {
                    MessageBox.Show("OracleException con código de error: " + ex.ErrorCode + "\nDetalles: " + ex.Message);
                }
            }
            finally
            {
                conn.Close();
            }

        }

        private void VentanaConfirmarVenta_Load(object sender, EventArgs e)
        {
            CenterToParent();
            conn.Close();

            string strCom = "SELECT precio_porPaq FROM Productos where id_producto = '" + VentanaVentas.CodigoBarra + "' AND ROWNUM <= 1";
            commm = new OracleCommand(strCom, conn);
            conn.Open();
            object resultado = commm.ExecuteScalar();
            conn.Close();
            double VxP = Convert.ToDouble(resultado);
            double cantidad = Convert.ToDouble(VentanaVentas.Cantidad);

            double total1 = (cantidad * VxP);
            double semitotal = Math.Round(total1, 2);
            double iv = (semitotal * 12) / 100;
            double iva = Math.Round(iv, 2);
            double total2 = semitotal + iva;
            double totalfinal = Math.Round(total2, 2);

            txtIva.Text = iva.ToString();
            txtPrecioPaquete.Text = VxP.ToString();
            txtCostoPrevio.Text = semitotal.ToString();
            txtCantidad.Text = cantidad.ToString();
            txtMontoTotal.Text = totalfinal.ToString();
            CenterToParent();
        }
    }
}
