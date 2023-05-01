using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using ArticuloNegocio;





namespace Solucioon
{
    public partial class AdministracionArticulos : Form
    {
        
        private List<Articulo> listaArticulos;
        public AdministracionArticulos()
        {
            InitializeComponent();
            Text = "Administrador de articulos";
        }
        private void ocultarColumnas()
        {
            dataGridView1.Columns["url_imagen"].Visible = false;
            dataGridView1.Columns["MyProperty"].Visible = false;
            dataGridView1.Columns["Id"].Visible = false;
        }
        private bool validarFiltro()
        {
            if (cboCampo.SelectedIndex < 0)
            {
                MessageBox.Show("Porfavor seleccione el campo");
                return true;

           
            }
            if (cboCriterio.SelectedIndex <0)
            {
                MessageBox.Show("Porfavor seleccione el criterio");
                return true;
            }
            return false;
        }
        private bool soloNumeros(string cadena)
        {
            return true;
        }
        private void cargar()
        {
            Negocio negocio = new Negocio();
            try
            {

                listaArticulos = negocio.listar();
                dataGridView1.DataSource = listaArticulos;
                ocultarColumnas();
                CargarImagen(listaArticulos[0].url_imagen);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cargar();
            cboCampo.Items.Add("Codigo");
            cboCampo.Items.Add("Nombre");
            

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                Articulo ArticuloSeleccionado = (Articulo)dataGridView1.CurrentRow.DataBoundItem;
                CargarImagen(ArticuloSeleccionado.url_imagen);
            }

           
        }
        private void CargarImagen(string imagen)
        {
            try
            {
                pbxImagen.Load(imagen);
            }
            catch (Exception )
            {

                pbxImagen.Load("https://i.pinimg.com/736x/3a/ab/e0/3aabe0e9a520b9ad90407a82f85adb42.jpg");
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            AltaArticulo alta = new AltaArticulo();
            alta.ShowDialog();
            cargar();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Articulo seleccionado;
            seleccionado = (Articulo)dataGridView1.CurrentRow.DataBoundItem;
            AltaArticulo modificar = new AltaArticulo(seleccionado);
            modificar.ShowDialog();
            cargar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            Negocio negocio = new Negocio();
            Articulo seleccionado;

            try
            {
                DialogResult  respuesta = MessageBox.Show("¿Seguro que quiere eliminarlo?","Eliminando",MessageBoxButtons.YesNo,MessageBoxIcon.Warning);
                if (respuesta == DialogResult.Yes)
                {
                    seleccionado = (Articulo)dataGridView1.CurrentRow.DataBoundItem;
                    negocio.eliminar(seleccionado.Id);
                    cargar();
                }
                else
                {
                    DialogResult.Cancel.Equals(respuesta);
                }
                

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            List<Articulo> listaFiltrada;
            string filtro = txtFiltroRapido.Text;
            if (filtro != "")
            {
                listaFiltrada = listaArticulos.FindAll(x => x.nombre.ToUpper().Contains(filtro.ToUpper()) || x.descripcion.ToUpper().Contains(filtro.ToUpper()) || x.marca.descripcion.ToUpper().Contains(filtro.ToUpper()));
            }
            else
            {
                listaFiltrada = listaArticulos;
            }
            
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = listaFiltrada;
            ocultarColumnas();
        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Articulo> listaFiltrada;
            string filtro = txtFiltroRapido.Text;
            if (filtro != "")
            {
                listaFiltrada = listaArticulos.FindAll(x => x.nombre.ToUpper().Contains(filtro.ToUpper()) || x.descripcion.ToUpper().Contains(filtro.ToUpper()) || x.marca.descripcion.ToUpper().Contains(filtro.ToUpper()));
            }
            else
            {
                listaFiltrada = listaArticulos;
            }

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = listaFiltrada;
            ocultarColumnas();
        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboCampo.SelectedItem.ToString();

            if (opcion == "Nombre")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Empieza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");
            }
            else if(opcion == "Codigo")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Empieza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");
            }



        }

        

       

        private void btnFiltroAvanzado_Click(object sender, EventArgs e)
        {
            Negocio negocio = new Negocio();
            try
            {
                if (validarFiltro())
                {
                    return;
                }
                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = txtFiltroAvanzado.Text;
                dataGridView1.DataSource = negocio.filtrar(campo, criterio, filtro);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void btnFiltroAvanzado_Click_1(object sender, EventArgs e)
        {
            Negocio negocio = new Negocio();
            try
            {
                if (validarFiltro())
                {
                    return;
                }
                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = txtFiltroAvanzado.Text;
                dataGridView1.DataSource = negocio.filtrar(campo, criterio, filtro);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void txtFiltroAvanzado_TextChanged(object sender, EventArgs e)
        {
            List<Articulo> listaFiltrada;
            string filtro = txtFiltroRapido.Text;
            if (filtro != "")
            {
                listaFiltrada = listaArticulos.FindAll(x => x.nombre.ToUpper().Contains(filtro.ToUpper()) || x.descripcion.ToUpper().Contains(filtro.ToUpper()) || x.marca.descripcion.ToUpper().Contains(filtro.ToUpper()));
            }
            else
            {
                listaFiltrada = listaArticulos;
            }

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = listaFiltrada;
            ocultarColumnas();
        }
    }
    
}
