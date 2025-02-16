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
    public partial class VentanaConfirmarModCaja : Form
    {
        static OracleConnection conn = DataAccess.getConn();
        OracleCommand comm = null;
        public VentanaConfirmarModCaja()
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
                MessageBox.Show("La caja fue Modificada con éxito");
                
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

        private void VentanaConfirmarModCaja_Load(object sender, EventArgs e)
        {
            CenterToParent();
            conn.Open();
            string strComm = "sp_ModificarCaja";
            comm = new OracleCommand(strComm, conn);
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add(new OracleParameter("p_CodigoCierreCaja", OracleType.Number)).Value = Convert.ToInt32(VentanaRegistroCajas.CodigoCaja);
            comm.Parameters.Add(new OracleParameter("p_MontoInicial", OracleType.Number)).Value = decimal.Parse(VentanaRegistroCajas.MontoInincial);
            comm.Parameters.Add(new OracleParameter("p_MontoCierre", OracleType.Number)).Value = decimal.Parse(VentanaRegistroCajas.MontoCierre);
            comm.Parameters.Add(new OracleParameter("p_TotalTransG", OracleType.VarChar)).Value = VentanaRegistroCajas.TranferenciaG;
            comm.Parameters.Add(new OracleParameter("p_TotalEfectG", OracleType.VarChar)).Value = VentanaRegistroCajas.EfectivoG;
            comm.Parameters.Add(new OracleParameter("p_GastosTotales", OracleType.Number)).Value = decimal.Parse(VentanaRegistroCajas.GastosTotales);
            comm.Parameters.Add(new OracleParameter("p_TotalTransI", OracleType.Number)).Value = decimal.Parse(VentanaRegistroCajas.TranferenciaI);
            comm.Parameters.Add(new OracleParameter("p_TotalEfectI", OracleType.Number)).Value = decimal.Parse(VentanaRegistroCajas.EfectivoI);
            comm.Parameters.Add(new OracleParameter("p_IngresosTotales", OracleType.Number)).Value = decimal.Parse(VentanaRegistroCajas.IngresosTotales);
          

        }
    }
}
