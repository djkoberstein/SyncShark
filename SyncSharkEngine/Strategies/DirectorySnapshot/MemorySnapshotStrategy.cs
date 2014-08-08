using SyncSharkEngine.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncSharkEngine.Strategies.DirectorySnapshot
{
    public class MemorySnapshotStrategy : IDirectorySnapshotStrategy
    {
        private Dictionary<string, Dictionary<string, IFileInfo>> m_InMemoryStore;

        public MemorySnapshotStrategy()
        {
            m_InMemoryStore = new Dictionary<string, Dictionary<string, IFileInfo>>();
        }

        public Dictionary<string, IFileInfo> Create(IDirectoryInfo directoryInfo)
        {
            Dictionary<string, IFileInfo> dicionary = new Dictionary<string, IFileInfo>();
            m_InMemoryStore.Add(directoryInfo.FullName, dicionary);
            foreach (var fileInfo in directoryInfo.GetFiles("*.*", SearchOption.AllDirectories))
            {
                string relativePath = fileInfo.FullName.Replace(directoryInfo.FullName, "");
                dicionary.Add(relativePath, fileInfo);
            }
            return dicionary;
        }

        public Dictionary<string, IFileInfo> Read(IDirectoryInfo directoryInfo)
        {
            if (Exists(directoryInfo))
            {
                return m_InMemoryStore[directoryInfo.FullName];
            }
            else
            {
                return new Dictionary<string, IFileInfo>();
            }
        }

        public Dictionary<string, IFileInfo> Update(IDirectoryInfo directoryInfo)
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
