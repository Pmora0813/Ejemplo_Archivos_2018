using Archivos.Clases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Archivos
{
    public partial class frmPrincipal : Form
    {
        IArchivo tipoArchivo;

        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            cmbTipoArchivo.Items.Add(new ArchivoTexto());
            cmbTipoArchivo.Items.Add(new ArchivoXml());
            cmbTipoArchivo.Items.Add(new ArchivoJson());

            cmbTipoArchivo.SelectedIndex = -1;
        }

        private void cmbTipoArchivo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Refrescar();
            mtxCodigo.Text = "";
            txtAutor.Text = "";
            txtTitulo.Text = "";
            mskPrecio.Text = "";
        }

        private void Refrescar()
        {
            tipoArchivo = (IArchivo)cmbTipoArchivo.SelectedItem;
            if (tipoArchivo != null)
            {
                mtxCodigo.Clear();
                txtAutor.Clear();
                txtTitulo.Clear();
                mskPrecio.Clear();
                dgvLibros.DataSource = tipoArchivo.ObtenerLibros();
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            tipoArchivo = (IArchivo)cmbTipoArchivo.SelectedItem;
            if (tipoArchivo != null)
            {
                int codigo = 0;
                if (ValidarDatos(out codigo))
                {
                    Libro libro = new Libro();

                    libro.Codigo = codigo;
                    libro.Titulo = txtTitulo.Text;
                    libro.Autor = txtAutor.Text;
                    libro.Precio = Convert.ToDouble(mskPrecio.Text.Trim());

                    tipoArchivo.GuardarLibro(libro);

                    Refrescar();
                }
            }
            else
            {
                MessageBox.Show("Seleccione un tipo de archivo");
            }
        }

        private bool ValidarDatos(out int codigo)
        {
            if (!Int32.TryParse(mtxCodigo.Text, out codigo))
            {
                MessageBox.Show("El código debe ser numérico");
                return false;
            }
            if (txtAutor.Text.Length == 0)
            {
                MessageBox.Show("Ingrese el nombre del autor");
                return false;
            }
            if (txtAutor.Text.Length == 0)
            {
                MessageBox.Show("Ingrese el nombre del autor");
                return false;
            }

            if (mskPrecio.Text.Length == 0)
            {
                MessageBox.Show("Ingrese el precio");
                return false;
            }

            return true;
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            if (dgvLibros.SelectedRows.Count > 0)
            {
                Libro libro = dgvLibros.SelectedRows[0].DataBoundItem as Libro;

                if (libro != null)
                {
                    if (MessageBox.Show("Esta seguro?", "Confirme", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
                    {
                        if (tipoArchivo != null)
                        {
                            tipoArchivo.BorrarLibro(libro);
                            Refrescar();
                        }
                        else
                        {
                            MessageBox.Show("Seleccione un tipo de archivo");
                        }
                    }
                }
            }
        }

        private void dgvLibros_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLibros.SelectedRows.Count > 0)
            {
                Libro libro = dgvLibros.SelectedRows[0].DataBoundItem as Libro;

                if (libro != null)
                {
                    mtxCodigo.Text = libro.Codigo.ToString();
                    txtAutor.Text = libro.Autor;
                    txtTitulo.Text = libro.Titulo;
                    mskPrecio.Text = libro.Precio + "";
                }
            }
        }
    }
}
