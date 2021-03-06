﻿using SyncShark.Interfaces;
using SyncShark.Engine.FileSystem;
using SyncShark.Engine.Strategies.Compare;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncShark.Engine.Strategies
{
    public class ExecuteStrategy : IExecuteStrategy
    {
        private ICompareStrategy m_CompareStrategy;

        public ExecuteStrategy(ICompareStrategy compareStrategy)
        {
            m_CompareStrategy = compareStrategy;
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
                        if (syncWorkItem.Source is IFileInfo)
                        {
                            IFileInfo source = syncWorkItem.Source as IFileInfo;
                            IFileInfo destination = syncWorkItem.Destination as IFileInfo;

                            destination.Delete();
                            using (Stream readStream = source.OpenRead())
                            using (Stream writeStream = destination.OpenWrite())
                            {
                                readStream.CopyTo(writeStream);
                            }
                        }
                        else
                        {
                            IDirectoryInfo destination = syncWorkItem.Destination as IDirectoryInfo;
                            destination.Create();
                        }
                        break;
                    case FileActions.DELETE:
                        syncWorkItem.Destination.Delete();
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
