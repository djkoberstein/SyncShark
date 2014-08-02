using SyncSharkEngine.FileSystem;
using SyncSharkEngine.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncSharkEngine.Factories
{
    public class SyncSharkServiceFactory : ISyncSharkServiceFactory
    {
        private ICompareService m_CompareService;

        public SyncSharkServiceFactory(ICompareService compareService)
	    {
            m_CompareService = compareService;
	    }

        public ISyncSharkService GetSyncSharkService()
        {
            return new SyncSharkService(m_CompareService);
        }
    }
}
