using SyncSharkEngine.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncSharkEngine.Strategies.DirectorySnapshot
{
    public class InMemorySnapshotStrategy : IDirectorySnapshotStrategy
    {
        private Dictionary<string, Dictionary<string, IFileInfo>> m_InMemoryStore;

        public InMemorySnapshotStrategy()
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
            return m_InMemoryStore[directoryInfo.FullName];
        }

        public Dictionary<string, IFileInfo> Update(IDirectoryInfo directoryInfo)
        {
            if (Exists(directoryInfo))
            {
                Delete(directoryInfo);
            }
            return Create(directoryInfo);
        }

        public Dictionary<string, IFileInfo> ReadOrCreate(IDirectoryInfo directoryInfo)
        {
            if (Exists(directoryInfo))
            {
                return Read(directoryInfo);
            }
            else
            {
                return Create(directoryInfo);
            }
        }

        public void Delete(IDirectoryInfo directoryInfo)
        {
            m_InMemoryStore.Remove(directoryInfo.FullName);
        }

        public bool Exists(IDirectoryInfo directoryInfo)
        {
            return m_InMemoryStore.ContainsKey(directoryInfo.FullName);
        }
    }
}
