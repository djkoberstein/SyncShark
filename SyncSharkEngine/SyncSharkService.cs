using SyncSharkEngine.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyncSharkEngine.Strategies.Compare;
using SyncSharkEngine.Strategies;

namespace SyncSharkEngine
{
    public class SyncSharkService : ISyncSharkService
    {
        private IExecuteStrategy m_SyncExecutionStrategy;
        private IExecuteStrategy m_MirrorExecutionStrategy;

        public SyncSharkService(IExecuteStrategy syncExecutionStrategy, IExecuteStrategy mirrorExecutionStrategy)
        {
            m_SyncExecutionStrategy = syncExecutionStrategy;
            m_MirrorExecutionStrategy = mirrorExecutionStrategy;
        }

        public void Sync()
        {
            m_SyncExecutionStrategy.CompareAndExecute();
        }

        public void Mirror()
        {
            m_MirrorExecutionStrategy.CompareAndExecute();
        }
    }
}
