using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoBDD
{
    public partial class VentanaConfirmarCierreCaja : Form
    {
        OracleConnection conn = null;
        OracleCommand comm = null;
        OracleCommand com = null;
        OracleCommand co = null;
        public VentanaConfirmarCierreCaja()
        {
            InitializeComponent();
            conn = DataAccess.getConn();
            comm = new OracleCommand();
            com = new OracleCommand();
            co = new OracleCommand();
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {


                // Configura y ejecuta el procedimiento almacenado sp_Vender
                string strComm = "sp_InsertarRegistroCaja";
                string formatoFecha = "yyyy-MM-dd"; // El formato que esperas en el string
                string fechaTexto = this.txtFech.Text; // El string que deseas convertir
                DateTime fechaValidada = DateTime.ParseExact(fechaTexto, formatoFecha, CultureInfo.InvariantCulture);

                decimal MC = Convert.ToDecimal(txtMontoFinal.Text);
                decimal TTG = Convert.ToDecimal(txtTotalTransG.Text);
                decimal TEG = Convert.ToDecimal(txtTotalEfecG.Text);
                decimal GT = Convert.ToDecimal(txtTotalGast.Text);
                decimal TTI = Convert.ToDecimal(txtTotalTransIn.Text);
                decimal TEI = Convert.ToDecimal(txtTotalEfecIn.Text);
                decimal IT = Convert.ToDecimal(txtTotalIngresos.Text);
                decimal MI = Convert.ToDecimal(VentanaCierreCaja.MontoInicial);
                comm = new OracleCommand(strComm, conn);
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.Add(new OracleParameter("p_CodigoCaja", OracleType.Number)).Value = Convert.ToInt32(VentanaCierreCaja.CodigoCaja);
                comm.Parameters.Add(new OracleParameter("p_Fecha", OracleType.DateTime)).Value = fechaValidada;
                comm.Parameters.Add(new OracleParameter("p_NombreUsuario", OracleType.VarChar)).Value = VentanaCierreCaja.NombreUsuario;
                comm.Parameters.Add(new OracleParameter("p_MontoInicial", OracleType.Number)).Value = MI;
                comm.Parameters.Add(new OracleParameter("p_MontoCierre", OracleType.Number)).Value = MC;
                comm.Parameters.Add(new OracleParameter("p_TotalTransG", OracleType.Number)).Value = TTG;
                comm.Parameters.Add(new OracleParameter("p_TotalEfectG", OracleType.Number)).Value = TEG;
                comm.Parameters.Add(new OracleParameter("p_GastosTotales", OracleType.Number)).Value = GT;
                comm.Parameters.Add(new OracleParameter("p_TotalTransI", OracleType.Number)).Value = TTI;
                comm.Parameters.Add(new OracleParameter("p_TotalEfectI", OracleType.Number)).Value = TEI;
                comm.Parameters.Add(new OracleParameter("p_IngresosTotales", OracleType.Number)).Value = IT;
                conn.Open();
                comm.ExecuteNonQuery();
                conn.Close();
                double CierreCaja = Convert.ToDouble(txtMontoFinal.Text);

                if (CierreCaja < 0)
                {
                    double CCM = CierreCaja * (-1);
                    MessageBox.Show("Se ha cerrado la caja con perdidas. Se ha tenido que utilizar " + CCM.ToString() + "$ dinero de otro lado");
                }
                else
                {
                    MessageBox.Show("Se ha cerrado la caja con exito");
                }
                this.btnConfirmar.Enabled = false;
            }
            catch (SqlException)
            {

                MessageBox.Show("¡¡ha surgido algun error");
                this.btnConfirmar.Enabled = false;

            }
        }

        private void VentanaConfirmarCierreCaja_Load(object sender, EventArgs e)
        {
            CenterToParent();
            txtFech.Text = VentanaCierreCaja.Fecha;
            string formatoFecha = "yyyy-MM-dd";

            if (DateTime.TryParseExact(txtFech.Text, formatoFecha, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fechaValidada))
            {
                string fecha = fechaValidada.ToString("yyyy-MM-dd");
                string strCo = "SELECT SUM(total) FROM NumeroOrdenCompra WHERE modoPag = 'Transferencia' and fecha_adq = TO_DATE('" + fecha + "', 'YYYY-MM-DD')";
                comm = new OracleCommand(strCo, conn);
                conn.Open();
                object tfg = comm.ExecuteScalar();
                conn.Close();
                double TFG = Convert.IsDBNull(tfg) ? 0 : Convert.ToDouble(tfg);
                string strC = "SELECT SUM(total) FROM NumeroOrdenCompra WHERE modoPag = 'Efectivo' and fecha_adq = TO_DATE('" + fecha + "', 'YYYY-MM-DD')";
                comm = new OracleCommand(strC, conn);
                conn.Open();
                object teg = comm.ExecuteScalar();
                conn.Close();
                double TEG = Convert.IsDBNull(teg) ? 0 : Convert.ToDouble(teg);

                double TotalTransG = Convert.ToDouble(TFG);
                double TotalEfectG = Convert.ToDouble(TEG);
                double GT = TotalTransG + TotalEfectG;
                double GastosTotales = Math.Round(GT, 2);

                string str = "SELECT SUM(total) FROM FacturasV WHERE modoPago = 'Transferencia' and fecha_fact = TO_DATE('" + fecha + "', 'YYYY-MM-DD')";
                comm = new OracleCommand(str, conn);
                conn.Open();
                object tfi = comm.ExecuteScalar();
                conn.Close();
                double TFI = Convert.IsDBNull(tfi) ? 0 : Convert.ToDouble(tfi);
                string a = "SELECT SUM(total) FROM FacturasV WHERE modoPago = 'Efectivo' and fecha_fact = TO_DATE('" + fecha + "', 'YYYY-MM-DD')";
                comm = new OracleCommand(a, conn);
                conn.Open();
                object tei = comm.ExecuteScalar();
                conn.Close();
                double TEI = Convert.IsDBNull(tei) ? 0 : Convert.ToDouble(tei);

                double TotalTransI = Convert.ToDouble(TFI);
                double TotalEfectI = Convert.ToDouble(TEI);
                double IT = TotalTransI + TotalEfectI;
                double IngresosTotales = Math.Round(IT, 2);
                double MontoInicial = Convert.ToDouble(VentanaCierreCaja.MontoInicial);
                double CC = (MontoInicial + IngresosTotales) - GastosTotales;
                double CierreCaja = Math.Round(CC, 2);


                txtTotalTransG.Text = TFG.ToString();
                txtTotalEfecG.Text = TEG.ToString();
                txtTotalGast.Text = GastosTotales.ToString();
                txtTotalTransIn.Text = TFI.ToString();
                txtTotalEfecIn.Text = TEI.ToString();
                txtTotalIngresos.Text = IngresosTotales.ToString();
                txtMontoFinal.Text = CierreCaja.ToString();
                CenterToParent();
            }
        }
    }
}
