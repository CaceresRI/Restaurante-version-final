using System;
using System.Collections.Generic;
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
using System.Data.EntityClient;
using System.Data.Objects;
using System.Data;
using System.Data.OracleClient;


namespace Restaurante.Vista.paginas_administrador
{
    /// <summary>
    /// Lógica de interacción para Adm_mesas.xaml
    /// </summary>
    public partial class Adm_mesas : Page
    {
        String Conexion = "Data Source=localhost:1521/xe; password=123456; User id=RESTAURANT";
        OracleConnection cone = new OracleConnection();
        public Adm_mesas()
        {
            InitializeComponent();
        }
    }
}
