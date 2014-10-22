using SyncShark.Interfaces;
using SyncShark.Engine.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncShark.Engine.Strategies.DirectorySnapshot
{
    public class MemorySnapshotStrategy : IDirectorySnapshotStrategy
    {
        private Dictionary<string, Dictionary<string, IFileSystemInfo>> m_InMemoryStore;

        public MemorySnapshotStrategy()
        {
            m_InMemoryStore = new Dictionary<string, Dictionary<string, IFileSystemInfo>>();
        }

        public Dictionary<string, IFileSystemInfo> Create(IDirectoryInfo directoryInfo)
        {
            Dictionary<string, IFileSystemInfo> dicionary = new Dictionary<string, IFileSystemInfo>();
            m_InMemoryStore.Add(directoryInfo.FullName, dicionary);
            foreach (var fileInfo in directoryInfo.GetFileSystemInfos())
            {
                string relativePath = fileInfo.FullName.Replace(directoryInfo.FullName, "");
                dicionary.Add(relativePath, fileInfo);
            }
            return dicionary;
        }

        public Dictionary<string, IFileSystemInfo> Read(IDirectoryInfo directoryInfo)
        {
            if (Exists(directoryInfo))
            {
                return m_InMemoryStore[directoryInfo.FullName];
            }
            else
            {
                return new Dictionary<string, IFileSystemInfo>();
            }
        }

        public Dictionary<string, IFileSystemInfo> Update(IDirectoryInfo directoryInfo)
        {
            if (Exists(directoryInfo))
            {
                Delete(directoryInfo);
            }
            return Create(directoryInfo);
        }

        private void Delete(IDirectoryInfo directoryInfo)
        {
            m_InMemoryStore.Remove(directoryInfo.FullName);
        }

        private bool Exists(IDirectoryInfo directoryInfo)
        {
            return m_InMemoryStore.ContainsKey(directoryInfo.FullName);
        }
    }
}
