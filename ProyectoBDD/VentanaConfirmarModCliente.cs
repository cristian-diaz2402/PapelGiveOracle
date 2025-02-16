using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OracleClient;

namespace ProyectoBDD
{
    public partial class VentanaConfirmarModCliente : Form
    {
        static OracleConnection conn = DataAccess.getConn();
        OracleCommand comm = null;
        OracleCommand Com = null;
        public VentanaConfirmarModCliente()
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
                MessageBox.Show("El cliente fue Modificado con éxito");
                this.btnConfirmar.Enabled = false;
            }
            catch (OracleException ex)
            {
                if (ex.Message.Contains("ORA-00001"))
                {
                    MessageBox.Show("¡¡ERROR!!, El cliente ya existe");
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

        private void VentanaConfirmarModCliente_Load(object sender, EventArgs e)
        {
            CenterToParent();
            conn.Open();
            string strComm = "sp_ModificarCliente";
            comm = new OracleCommand(strComm, conn);
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add(new OracleParameter("p_Cedula", OracleType.VarChar)).Value = VentanaClientes.Cedula;
            comm.Parameters.Add(new OracleParameter("p_PrimerNombre", OracleType.VarChar)).Value = VentanaClientes.PrimerNombre;
            comm.Parameters.Add(new OracleParameter("p_SegundoNombre", OracleType.VarChar)).Value = VentanaClientes.SegundoNombre;
            comm.Parameters.Add(new OracleParameter("p_PrimerApellido", OracleType.VarChar)).Value = VentanaClientes.PrimerApellido;
            comm.Parameters.Add(new OracleParameter("p_SegundoApellido", OracleType.VarChar)).Value = VentanaClientes.SegundoApellido;
            comm.Parameters.Add(new OracleParameter("p_Direccion", OracleType.VarChar)).Value = VentanaClientes.Direccion;
            comm.Parameters.Add(new OracleParameter("p_Telefono", OracleType.VarChar)).Value = VentanaClientes.Telefono;
            comm.Parameters.Add(new OracleParameter("p_Correo", OracleType.VarChar)).Value = VentanaClientes.Correo;
            comm.Parameters.Add(new OracleParameter("p_NombreEmpresa", OracleType.VarChar)).Value = VentanaClientes.NombreEmpresa;
            comm.Parameters.Add(new OracleParameter("p_Ruc", OracleType.VarChar)).Value = VentanaClientes.RUC;
            comm.Parameters.Add(new OracleParameter("p_Sede", OracleType.VarChar)).Value = VentanaClientes.Sede;
        }
    }
}
