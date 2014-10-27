using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncShark.WpfUI.ViewModels
{
    public class ConfigurationViewModel : BaseViewModel
    {
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
                FirePropertyChanged();
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
                FirePropertyChanged();
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
                FirePropertyChanged();
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
                FirePropertyChanged();
            }
        }

        public ConfigurationViewModel()
        {
            SourcePath = "";
            DestinationPath = "";
            IsSynchronizeChecked = true;
            IsMirrorChecked = false;
        }
    }
}
