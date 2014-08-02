using SyncSharkEngine.FileSystem;
using SyncSharkEngine.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyncSharkEngine.Strategies.Compare;

namespace SyncSharkEngine.Factories
{
    public class SyncSharkServiceFactory : ISyncSharkServiceFactory
    {
        private ICompareStrategy m_CompareStrategy;

        public SyncSharkServiceFactory(ICompareStrategy compareStrategy)
	    {
            m_CompareStrategy = compareStrategy;
	    }

        public ISyncSharkService GetSyncSharkService(IDirectoryInfo leftDirectoryInfo, IDirectoryInfo rightDirectoryInfo)
        {
            return new SyncSharkService(m_CompareStrategy, leftDirectoryInfo, rightDirectoryInfo);
        }
    }
}
