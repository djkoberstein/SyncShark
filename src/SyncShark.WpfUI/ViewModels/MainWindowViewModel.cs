using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using SyncShark.Engine.FileSystem;
using SyncShark.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SyncShark.WpfUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ISyncSharkService m_SyncSharkService;

        private string m_SourcePath;
        public string SourcePath
        {
            get
            {
                return m_SourcePath;
            }
            set
            {
                m_SourcePath = value;
                RaisePropertyChanged();
            }
        }

        private string m_DestinationPath;
        public string DestinationPath
        {
            get
            {
                return m_DestinationPath;
            }
            set
            {
                m_DestinationPath = value;
                RaisePropertyChanged();
            }
        }

        private bool m_IsSynchronizeChecked;
        public bool IsSynchronizeChecked
        {
            get
            {
                return m_IsSynchronizeChecked;
            }
            set
            {
                m_IsSynchronizeChecked = value;
                RaisePropertyChanged();
            }
        }

        private bool m_IsMirrorChecked;
        public bool IsMirrorChecked
        {
            get
            {
                return m_IsMirrorChecked;
            }
            set
            {
                m_IsMirrorChecked = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<ISyncWorkItem> m_WorkItems;
        public ObservableCollection<ISyncWorkItem> WorkItems
        {
            get
            {
                return m_WorkItems;
            }
            set
            {
                m_WorkItems = value;
                RaisePropertyChanged();
            }
        }

        #region Synchronize Command
        private RelayCommand m_ExecuteCommand;
        public RelayCommand ExecuteCommand
        {
            get
            {
                return m_ExecuteCommand;
            }
            set
            {
                m_ExecuteCommand = value;
                RaisePropertyChanged();
            }
        }
        private bool ExecuteCommandCanExecute()
        {
            return (IsSynchronizeChecked ^ IsMirrorChecked)
                && Directory.Exists(SourcePath)
                && Directory.Exists(DestinationPath);
        }
        private void ExecuteCommandExecute()
        {
            var left = new DirectoryInfoFacade(new DirectoryInfo(m_SourcePath));
            var right = new DirectoryInfoFacade(new DirectoryInfo(m_DestinationPath));

            if (IsSynchronizeChecked)
            {
                m_SyncSharkService.Sync(left, right);
            }
            else if (IsMirrorChecked)
            {
                m_SyncSharkService.Mirror(left, right);
            }
        }
        #endregion

        public MainWindowViewModel()
        {
            ExecuteCommand = new RelayCommand(ExecuteCommandExecute, ExecuteCommandCanExecute);
        }

        public MainWindowViewModel(ISyncSharkService syncSharkService)
            : this ()
        {
            m_SyncSharkService = syncSharkService;
        }


    }
}
