using SyncSharkEngine.DataModel;
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
        private IDirectorySnapshotServiceFactory m_DirectorySnapshotServiceFactory;
        
        public SyncSharkServiceFactory(IDirectorySnapshotServiceFactory directorySnapshotServiceFactory)
	    {
            m_DirectorySnapshotServiceFactory = directorySnapshotServiceFactory;
	    }

        public ISyncSharkService GetSyncSharkService()
        {
            return new SyncSharkService(m_DirectorySnapshotServiceFactory);
        }
    }
}
