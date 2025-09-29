using GestionComercial.Domain.Constant;
using System.ComponentModel;
using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.Domain.DTOs.Security
{
    public class PermissionGroupViewModel : INotifyPropertyChanged
    {
        public ModuleType Module { get; set; }
        public string ModuleName => EnumExtensionService.GetDisplayName(Module);

        private bool _canRead;
        public bool CanRead
        {
            get => _canRead;
            set
            {
                if (_canRead != value)
                {
                    _canRead = value;
                    OnPropertyChanged(nameof(CanRead));

                    if (!value)
                    {
                        // Si quitás lectura → quitar todos los demás
                        CanAdd = false;
                        CanEdit = false;
                        CanDelete = false;
                    }
                }
            }
        }

        public bool IsEnabledCanRead { get; set; }


        private bool _canAdd;
        public bool CanAdd
        {
            get => _canAdd;
            set
            {
                if (_canAdd != value)
                {
                    _canAdd = value;
                    OnPropertyChanged(nameof(CanAdd));

                    if (value)
                    {
                        // Si activás agregar → lectura tiene que estar activa
                        if (!CanRead) CanRead = true;
                    }
                    else
                    {
                        // Si sacás agregar → también desactivar editar y borrar
                        CanEdit = false;
                        CanDelete = false;
                    }
                }
            }
        }

        private bool _canEdit;
        public bool CanEdit
        {
            get => _canEdit;
            set
            {
                if (_canEdit != value)
                {
                    _canEdit = value;
                    OnPropertyChanged(nameof(CanEdit));

                    if (value)
                    {
                        // Activar agregar y lectura
                        if (!CanRead) CanRead = true;
                        if (!CanAdd) CanAdd = true;
                    }
                    else
                    {
                        // Si sacás editar → sacar borrar
                        CanDelete = false;
                    }
                }
            }
        }

        private bool _canDelete;
        public bool CanDelete
        {
            get => _canDelete;
            set
            {
                if (_canDelete != value)
                {
                    _canDelete = value;
                    OnPropertyChanged(nameof(CanDelete));

                    if (value)
                    {
                        // Activar todo lo anterior
                        if (!CanRead) CanRead = true;
                        if (!CanAdd) CanAdd = true;
                        if (!CanEdit) CanEdit = true;
                    }
                }
            }
        }

        public PermissionGroupViewModel(ModuleType moduleType)
        {
            Module = moduleType;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string prop) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
