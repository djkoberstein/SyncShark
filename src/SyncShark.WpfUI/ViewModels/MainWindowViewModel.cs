using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncShark.WpfUI.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private WorkItemViewModel m_WorkItemViewModel;
        public WorkItemViewModel WorkItemViewModel 
        { 
            get
            {
                return m_WorkItemViewModel;
            }
            set
            {
                m_WorkItemViewModel = value;
                FirePropertyChanged();
            }
        }

        private ConfigurationViewModel m_ConfigureationViewModel;
        public ConfigurationViewModel ConfigureationViewModel
        {
            get
            {
                return m_ConfigureationViewModel;
            }
            set
            {
                m_ConfigureationViewModel = value;
                FirePropertyChanged();
            }
        }

        public MainWindowViewModel()
        {

        }

        public MainWindowViewModel(WorkItemViewModel workItemViewModel, ConfigurationViewModel configureationViewModel)
        {
            WorkItemViewModel = workItemViewModel;
            ConfigureationViewModel = configureationViewModel;
        }
    }
}
