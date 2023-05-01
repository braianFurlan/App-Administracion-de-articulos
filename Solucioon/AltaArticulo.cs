using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using ArticuloNegocio;
using System.IO;
using System.Configuration;

namespace Solucioon
{
    public partial class AltaArticulo : Form
    {
        private Articulo articulo = null;
        private OpenFileDialog archivo = null;
        public AltaArticulo()
        {
            InitializeComponent();
            Text = "Agregar articulo";
        }
        public AltaArticulo(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            Text = "Modificar Articulo";
        }
        private void levantarImagen()
        {
            OpenFileDialog archivo = new OpenFileDialog();
            archivo.Filter = "jpg|*.jpg;|png|*.png";
            if (archivo.ShowDialog() == DialogResult.OK)
            {
                txtUrlImagen.Text = archivo.FileName;
                CargarImagen(archivo.FileName);


            }
        }



        private void btnAceptar_Click(object sender, EventArgs e)
        {
            
            Negocio negocio = new Negocio();
            try
            {
                if (articulo == null)
                {
                    articulo = new Articulo();

                }
                articulo.codigoArticulo = txtCodigoArticulo.Text;
                articulo.nombre = txtNombre.Text;
                articulo.descripcion = txtDescripcion.Text;
                articulo.url_imagen = txtUrlImagen.Text;
                articulo.precio = decimal.Parse(txtPrecio.Text);
                articulo.marca = (MARCA)cboMarca.SelectedItem;
                articulo.categoria = (CATEGORIA)cboCategoria.SelectedItem;

                if (articulo.Id != 0)
                {
                    negocio.modificar(articulo);
                    MessageBox.Show("Modificado exitosamente");
                }
                else
                {
                    negocio.agregar(articulo);
                    MessageBox.Show("Agregado exitosamente");
                }
                //guardo imagen si la levanto localmente 
                if (archivo != null && !(txtUrlImagen.Text.ToUpper().Contains("HTTP")))
                {
                    File.Copy(archivo.FileName, ConfigurationManager.AppSettings["images-folder"] + archivo.SafeFileName);
                }
                    
                


                Close();
                
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AltaArticulo_Load(object sender, EventArgs e)
        {
            btnAceptar.Enabled = false;
            CategoriaNegocio categoria = new CategoriaNegocio();
            MarcaNegocio marca = new MarcaNegocio();
            try
            {
                cboCategoria.DataSource = categoria.listar();
                cboCategoria.ValueMember = "Id";
                cboCategoria.DisplayMember = "Descripcion";
                cboMarca.DataSource = marca.listar();
                cboMarca.ValueMember = "Id";
                cboMarca.DisplayMember = "Descripcion";

                if (articulo != null)

                {
                    txtCodigoArticulo.Text = articulo.codigoArticulo;
                    txtNombre.Text = articulo.nombre;
                    txtDescripcion.Text = articulo.descripcion;
                    txtPrecio.Text = articulo.precio.ToString();
                    txtUrlImagen.Text = articulo.url_imagen;
                    CargarImagen(articulo.url_imagen);
                    cboCategoria.SelectedValue = articulo.categoria.id;
                    cboCategoria.SelectedValue = articulo.marca.id;

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void cboCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtUrlImagen_Leave(object sender, EventArgs e)
        {
            CargarImagen(txtUrlImagen.Text);
        }
        private void CargarImagen(string imagen)
        {
            try
            {
                pbxArticuloAgregar.Load(imagen);
            }
            catch (Exception )
            {

                pbxArticuloAgregar.Load("https://i.pinimg.com/736x/3a/ab/e0/3aabe0e9a520b9ad90407a82f85adb42.jpg");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            levantarImagen();

        }

        
        private void validar()
        {
            var va = !string.IsNullOrEmpty(txtCodigoArticulo.Text) &&
                !string.IsNullOrEmpty(txtNombre.Text) &&
                !string.IsNullOrEmpty(txtPrecio.Text);
            btnAceptar.Enabled = va;
        }
        

        private void txtPrecio_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 32 && e.KeyChar <= 45) || ((e.KeyChar >= 58 && e.KeyChar <= 255)))
            {
                MessageBox.Show("Porfavor solo numeros", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private void txtCodigoArticulo_TextChanged(object sender, EventArgs e)
        {
            validar(); 
        }

        private void txtNombre_TextChanged(object sender, EventArgs e)
        {
            validar();
        }

        private void txtPrecio_TextChanged(object sender, EventArgs e)
        {
            validar();
        }
    }
}
