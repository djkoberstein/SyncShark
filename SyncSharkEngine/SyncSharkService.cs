using SyncShark.Engine.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyncShark.Engine.Strategies.Compare;
using SyncShark.Engine.Strategies;
using SyncShark.Interfaces;

namespace SyncShark.Engine
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

        public void Sync(IDirectoryInfo leftDirectoryInfo, IDirectoryInfo rightDirectoryInfo)
        {
            m_SyncExecutionStrategy.CompareAndExecute(leftDirectoryInfo, rightDirectoryInfo);
        }

        public void Mirror(IDirectoryInfo leftDirectoryInfo, IDirectoryInfo rightDirectoryInfo)
        {
            m_MirrorExecutionStrategy.CompareAndExecute(leftDirectoryInfo, rightDirectoryInfo);
        }
    }
}
