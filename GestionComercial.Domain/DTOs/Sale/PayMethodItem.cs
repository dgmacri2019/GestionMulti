using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GestionComercial.Domain.DTOs.Sale
{
    public class PayMethodItem : INotifyPropertyChanged
    {
        private string _metodoCodigo;
        private string _metodoDescripcion;
        private decimal _monto;
        private int _metodoId;


        public int MetodoId
        {
            get => _metodoId;
            set { _metodoId = value; OnPropertyChanged(); }
        }

        // acá guardás EF, MP, TJ, etc.
        public string MetodoCodigo
        {
            get => _metodoCodigo;
            set { _metodoCodigo = value; OnPropertyChanged(); }
        }

        public decimal Monto
        {
            get => _monto;
            set { _monto = value; OnPropertyChanged(); }
        }

        // Para mostrar en grilla el nombre completo
        public string MetodoDescripcion
        {
            get => _metodoDescripcion;
            set { _metodoDescripcion = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}