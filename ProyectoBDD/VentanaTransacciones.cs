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
    public partial class VentanaTransacciones : Form
    {
        public VentanaTransacciones()
        {
            InitializeComponent();
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSoliCompra_Click(object sender, EventArgs e)
        {
            VentanaCompras Vcompras = new VentanaCompras();
            Vcompras.ShowDialog();
        }

        private void btnSoliVenta_Click(object sender, EventArgs e)
        {
            VentanaVentas Vventas = new VentanaVentas();
            Vventas.ShowDialog();
        }

        private void VentanaTransacciones_Load(object sender, EventArgs e)
        {
            CenterToParent();
        }

        private void btnRegistrosVentas_Click(object sender, EventArgs e)
        {
            VentanaRegistroVentas MCC = new VentanaRegistroVentas();
            MCC.Show();
        }

        private void btnRcompras_Click(object sender, EventArgs e)
        {
            VentanaRegistroCompras VRC = new VentanaRegistroCompras();
            VRC.Show();
        }
    }
}
