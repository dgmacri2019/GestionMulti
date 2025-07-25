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

namespace GestionComercial.Desktop.Controls.Users
{
    /// <summary>
    /// Lógica de interacción para EditUserControlView.xaml
    /// </summary>
    public partial class EditUserControlView : UserControl
    {
        public event Action UsuarioActualizado;
        public EditUserControlView(string id)
        {
            InitializeComponent();
        }
    }
}
