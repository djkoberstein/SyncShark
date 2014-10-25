using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SyncShark.WpfUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Window m_MainWindow;

        public App()
        {
            throw new InvalidOperationException("Don't use the default constructor!");
        }

        public App(Window window)
            : base()
        {
            m_MainWindow = window;
        }

        public void Start()
        {
            Run(m_MainWindow);
        }
    }
}
