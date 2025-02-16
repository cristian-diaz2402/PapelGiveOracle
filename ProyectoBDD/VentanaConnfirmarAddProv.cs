using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OracleClient;
using System.Windows.Forms;

namespace ProyectoBDD
{
    public partial class VentanaConnfirmarAddProv : Form
    {
        public VentanaConnfirmarAddProv()
        {
            InitializeComponent();
        }
        static OracleConnection conn = DataAccess.getConn();
        OracleCommand comm = null;
        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                comm.ExecuteNonQuery();
                MessageBox.Show("El proveedor fue ingresado con éxito");
                this.btnConfirmar.Enabled = false;
            }
            catch (OracleException ex)
            {
                if (ex.Message.Contains("ORA-00001"))
                {
                    MessageBox.Show("¡¡ERROR!!, El proveedor ya existe");
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

        private void VentanaConnfirmarAddProv_Load(object sender, EventArgs e)
        {
            CenterToParent();
            conn.Open();
            string strComm = "sp_InsertarProveedor";
            comm = new OracleCommand(strComm, conn);
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add(new OracleParameter("p_Ruc", OracleType.VarChar)).Value = VentanaProveedores.RUC;
            comm.Parameters.Add(new OracleParameter("p_NombreEmpresarial", OracleType.VarChar)).Value = VentanaProveedores.NombreEmpresa;
            comm.Parameters.Add(new OracleParameter("p_NombreSede", OracleType.VarChar)).Value = "Quito";
            comm.Parameters.Add(new OracleParameter("p_Direccion", OracleType.VarChar)).Value = VentanaProveedores.Direccion;
            comm.Parameters.Add(new OracleParameter("p_Telefono", OracleType.VarChar)).Value = VentanaProveedores.Telefono;
            comm.Parameters.Add(new OracleParameter("p_Correo", OracleType.VarChar)).Value = VentanaProveedores.Correo;
        }
    }
}
