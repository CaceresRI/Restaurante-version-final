using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Drawing;
using System.IO;

namespace Restaurante.Vista.paginas_administrador
{
    /// <summary>
    /// Lógica de interacción para Adm_Recetas.xaml
    /// </summary>
    public partial class Adm_Recetas : Page
    {

        public Adm_Recetas()
        {
            InitializeComponent();
            cargarInsumos();
        }

        OracleConnection cone = new OracleConnection("Data Source = localhost:1521 / xe; password=123456; User id = RESTAURANT;");
        string filename = "";
        //Esta funcion sirve para mostrar los controles necesarios para crear un nuevo insumo siendo administrador, solo cuando se desee crearlo
        private void mostrarCrear()
        {
            lblDescripcion.Visibility = Visibility.Visible;
            lblprecio.Visibility = Visibility.Visible;
            lblreceta.Visibility = Visibility.Visible;
            txtPrecio.Visibility = Visibility.Visible;
            txtReceta.Visibility = Visibility.Visible;
            txtDescripcion.Visibility = Visibility.Visible;
            Crear_confirmacion.Visibility = Visibility.Visible;
            abririmagen.Visibility = Visibility.Visible;
            lblImagen.Visibility = Visibility.Visible;
            lblinsumo.Visibility = Visibility.Visible;
            cbinsumo.Visibility = Visibility.Visible;
        }
        private void cargarInsumos()
        {
            OracleCommand comando = new OracleCommand("select nom_insumo from insumos", cone);
            cone.Open();
            string nombre = "";
            OracleDataReader registro = comando.ExecuteReader();
            while (registro.Read())
            {
                cbinsumo.Items.Add(registro["nom_insumo"].ToString());
            }
            registro.Close();
            cone.Close();
        }
        //Ocultamos los controles para crear un nuevo insumos en caso de no ser necesario su uso(al querer eliminar, listar o actualizar)
        private void ocultarCrear()
        {

            lblDescripcion.Visibility = Visibility.Hidden;
            lblprecio.Visibility = Visibility.Hidden;
            lblreceta.Visibility = Visibility.Hidden;
            txtPrecio.Visibility = Visibility.Hidden;
            txtReceta.Visibility = Visibility.Hidden;
            txtDescripcion.Visibility = Visibility.Hidden;
            abririmagen.Visibility = Visibility.Hidden;
            Crear_confirmacion.Visibility = Visibility.Hidden;
            lblImagen.Visibility = Visibility.Hidden;
            lblinsumo.Visibility = Visibility.Hidden;
            cbinsumo.Visibility = Visibility.Hidden;
        }
        //Lo mismo de arriba, en este caso mostrando los controles al querer eliminar un insumo
        private void mostrarEliminar()
        {
            eliminartxt.Visibility = Visibility.Visible;
            confirmar.Visibility = Visibility.Visible;
        }
        //Otra vez ocultamos los controles cuando seran utilizados 
        private void ocultarEliminar()
        {
            eliminartxt.Visibility = Visibility.Hidden;
            confirmar.Visibility = Visibility.Hidden;
        }

        public  void Listar()
        {
            cone.Open();

            OracleCommand cmd = new OracleCommand("SP_LISTAR_PLATO", cone);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("registros", OracleType.Cursor).Direction = ParameterDirection.Output;
       
            OracleDataAdapter adaptador = new OracleDataAdapter();
            adaptador.SelectCommand = cmd;

            DataTable dt = new DataTable();
            adaptador.Fill(dt);
            dgRecetas.ItemsSource = dt.AsDataView();
            dgRecetas.Items.Refresh();

            cone.Close();
        }
        private void btnCrear_Click(object sender, RoutedEventArgs e)
        {
            mostrarCrear();
            ocultarEliminar();
        }

        private void btnListar_Click(object sender, RoutedEventArgs e)
        {
            ocultarCrear();
            ocultarEliminar();
            Listar();
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            ocultarCrear();
            mostrarEliminar();
        }
        private void confEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (eliminartxt.Text != "")
            {
                //Se abre la conexion
                cone.Open();
                //Se envia el nombre del insumo a eliminar

                OracleCommand cmd = new OracleCommand("SP_ELIMINAR_RECETA", cone);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("p_nombre", OracleType.Char).Value = eliminartxt.Text;
                cmd.ExecuteNonQuery();

                cone.Close();
                MessageBox.Show("receta eliminada");
                Listar();
            }
            else
            {
                MessageBox.Show("ingrese insumo a eliminar");
            }
        }


        private void abririmagen_click(object sender, RoutedEventArgs e)
        {

            //se crea eñ objeto OpenFileDialog, el cual permite buscar la imagen en nuestro ordenador
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Se muestra al usuario esperando una acción
            bool? respuesta = openFileDialog.ShowDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);


            //tipo de extenciones de archivos habilitados para esta acción
            openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg";


            if (respuesta == true)
            {
               
                Uri fileUri = new Uri(openFileDialog.FileName);
                imagen.Source = new BitmapImage(fileUri);

            }

        }

        private void Crear_confirmacion_Click(object sender, RoutedEventArgs e)
        {
            if (txtReceta.Text != "" && txtPrecio.Text != ""
                && txtDescripcion.Text != "" && int.TryParse(txtPrecio.Text, out int result) == true && cbinsumo.SelectedIndex > -1)
            {


                int precio = int.Parse(txtPrecio.Text);
                byte[] arr;
                using (MemoryStream ms = new MemoryStream())
                {
                    var bmp = imagen.Source as BitmapImage;
                    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bmp));
                    encoder.Save(ms);
                    arr = ms.ToArray();
                }


                OracleCommand cmd = new OracleCommand("SP_CREAR_RECETA", cone);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("p_nombre", OracleType.Char).Value = txtReceta.Text;
                //******************IMAGEN*******************
                cmd.Parameters.Add("p_imagen", OracleType.Blob).Value = arr;
                //******************IMAGEN*******************
                cmd.Parameters.Add("p_precio", OracleType.Number).Value = precio;
                cmd.Parameters.Add("p_descripcion", OracleType.Char).Value = txtDescripcion.Text;
                    cmd.Parameters.Add("p_insumo", OracleType.Char).Value = cbinsumo.Text;
                
                cone.Open();
                cmd.ExecuteNonQuery();
                cone.Close();
                MessageBox.Show("plato agregado correctamente");
            }
            else
            {
                if (int.TryParse(txtPrecio.Text, out int result2) == false)
                    MessageBox.Show("Precio invalido, solo debe contener numeros");
                else
                    MessageBox.Show("Faltan campos por rellenar");
            }
        }

       
    }
}
