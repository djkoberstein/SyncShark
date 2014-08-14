using SyncSharkEngine.FileSystem;
using SyncSharkEngine.Strategies.Compare;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncSharkEngine.Strategies
{
    public class ExecuteStrategy : IExecuteStrategy
    {
        private ICompareStrategy m_CompareStrategy;
        private IFileSystem m_FileSystem;

        public ExecuteStrategy(IFileSystem fileSystem, ICompareStrategy compareStrategy)
        {
            m_CompareStrategy = compareStrategy;
            m_FileSystem = fileSystem;
        }


        public void CompareAndExecute(IDirectoryInfo leftDirectoryInfo, IDirectoryInfo rightDirectoryInfo)
        {
            var syncWorkItems = Compare(leftDirectoryInfo, rightDirectoryInfo);
            Execute(syncWorkItems);
        }

        public IEnumerable<ISyncWorkItem> Compare(IDirectoryInfo leftDirectoryInfo, IDirectoryInfo rightDirectoryInfo)
        {
            return m_CompareStrategy.Compare(leftDirectoryInfo, rightDirectoryInfo);
        }

        public void Execute(IEnumerable<ISyncWorkItem> syncWorkItems)
        {
            foreach (var syncWorkItem in syncWorkItems)
            {
                switch (syncWorkItem.FileAction)
                {
                    case FileActions.NONE:
                        break;
                    case FileActions.COPY:
                        if (syncWorkItem.Destination is IFileInfo)
                        {
                            IFileInfo source = syncWorkItem.Source as IFileInfo;
                            IFileInfo destination = syncWorkItem.Destination as IFileInfo;

                            m_FileSystem.Delete(destination);
                            using (Stream readStream = m_FileSystem.OpenRead(source))
                            using (Stream writeStream = m_FileSystem.OpenWrite(destination))
                            {
                                readStream.CopyTo(writeStream);
                            }
                        }
                        else if (syncWorkItem.Destination is IDirectoryInfo)
                        {
                            IDirectoryInfo destination = syncWorkItem.Destination as IDirectoryInfo;
                            m_FileSystem.Create(destination);
                        }
                        break;
                    case FileActions.DELETE:
                        m_FileSystem.Delete(syncWorkItem.Destination);
                        break;
                    case FileActions.CONFLICT:
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
