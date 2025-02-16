using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoBDD
{
    public partial class VentanaCaja : Form
    {
        public VentanaCaja()
        {
            InitializeComponent();
        }

        private void btnsalir_Click(object sender, EventArgs e)
        {
            this.Close();
            
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void VentanaCaja_Load(object sender, EventArgs e)
        {
            CenterToParent();
        }

        private void btnCierreCaja_Click(object sender, EventArgs e)
        {
            VentanaCierreCaja VCcaja = new VentanaCierreCaja();
            VCcaja.ShowDialog();
        }

        private void btnSolicitudes_Click(object sender, EventArgs e)
        {
            VentanaRegistroCajas VRcaja = new VentanaRegistroCajas();
            VRcaja.ShowDialog();
        }
    }
}
