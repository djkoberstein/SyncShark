using SyncShark.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SyncShark.WpfUI.ViewModels
{
    public class WorkItemViewModel : BaseViewModel
    {
        private ObservableCollection<ISyncWorkItem> m_WorkItems = new ObservableCollection<ISyncWorkItem>();
        public ObservableCollection<ISyncWorkItem> WorkItems 
        { 
            get
            {
                return m_WorkItems;
            }
            set
            {
                m_WorkItems = value;
                FirePropertyChanged();
            }
        }
    }
}
