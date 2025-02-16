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
    public partial class VentanaConfirmarAddEmple : Form
    {
        public VentanaConfirmarAddEmple()
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
                MessageBox.Show("El empleado fue ingresado con éxito");
                this.btnConfirmar.Enabled = false;
            }
            catch (OracleException ex)
            {
                if (ex.Message.Contains("ORA-00001"))
                {
                    MessageBox.Show("¡¡ERROR!!, El empleado ya existe");
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

        private void VentanaConfirmarAddEmple_Load(object sender, EventArgs e)
        {
            CenterToParent();
            conn.Open();
            string strComm = "sp_InsertarEmpleado";
            comm = new OracleCommand(strComm, conn);
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add(new OracleParameter("p_Cedula", OracleType.VarChar)).Value = VentanaEmpleados.Cedula;
            comm.Parameters.Add(new OracleParameter("p_PrimerNombre", OracleType.VarChar)).Value = VentanaEmpleados.PrimerNombre;
            comm.Parameters.Add(new OracleParameter("p_SegundoNombre", OracleType.VarChar)).Value = VentanaEmpleados.SegundoNombre;
            comm.Parameters.Add(new OracleParameter("p_PrimerApellido", OracleType.VarChar)).Value = VentanaEmpleados.PrimerApellido;
            comm.Parameters.Add(new OracleParameter("p_SegundoApellido", OracleType.VarChar)).Value = VentanaEmpleados.SegundoApellido;
            comm.Parameters.Add(new OracleParameter("p_Direccion", OracleType.VarChar)).Value = VentanaEmpleados.Direccion;
            comm.Parameters.Add(new OracleParameter("p_Telefono", OracleType.VarChar)).Value = VentanaEmpleados.Telefono;
            comm.Parameters.Add(new OracleParameter("p_Correo", OracleType.VarChar)).Value = VentanaEmpleados.Correo;
            comm.Parameters.Add(new OracleParameter("p_Rol", OracleType.VarChar)).Value = VentanaEmpleados.Rol;
            comm.Parameters.Add(new OracleParameter("p_Ocupacion", OracleType.VarChar)).Value = VentanaEmpleados.Ocupacion;
            comm.Parameters.Add(new OracleParameter("p_Clave", OracleType.VarChar)).Value = VentanaEmpleados.Password;
            comm.Parameters.Add(new OracleParameter("p_NombreUsuario", OracleType.VarChar)).Value = VentanaEmpleados.NombreUsuario;
            comm.Parameters.Add(new OracleParameter("p_NombreSede", OracleType.VarChar)).Value = VentanaEmpleados.Sede;
            comm.Parameters.Add(new OracleParameter("p_Sueldo", OracleType.VarChar)).Value = VentanaEmpleados.Sueldo;
        }
    }
}
