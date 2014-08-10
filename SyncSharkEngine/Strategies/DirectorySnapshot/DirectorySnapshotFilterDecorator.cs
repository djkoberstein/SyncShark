using SyncSharkEngine.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncSharkEngine.Strategies.DirectorySnapshot
{
    public class DirectorySnapshotFilterDecorator : IDirectorySnapshotStrategy
    {
        private IDirectorySnapshotStrategy m_DirectorySnapshotStrategy;
        private List<string> m_ExcludedFileNames;
        
        public DirectorySnapshotFilterDecorator(IDirectorySnapshotStrategy directorySnapshotStrategy)
        {
            m_DirectorySnapshotStrategy = directorySnapshotStrategy;
            m_ExcludedFileNames = new List<string>();
            m_ExcludedFileNames.Add(FileSystemSnapshotStrategy.STORE_FILE_NAME);
            m_ExcludedFileNames.Add(@"\SharkSync\");
        }

        public Dictionary<string, IFileSystemInfo> Create(IDirectoryInfo directoryInfo)
        {
            var dictionary = m_DirectorySnapshotStrategy.Create(directoryInfo);
            var filteredDictionary = FilterDictionary(dictionary);
            return filteredDictionary;
        }

        public Dictionary<string, IFileSystemInfo> Read(IDirectoryInfo directoryInfo)
        {
            var dictionary = m_DirectorySnapshotStrategy.Read(directoryInfo);
            var filteredDictionary = FilterDictionary(dictionary);
            return filteredDictionary;
        }

        public Dictionary<string, IFileSystemInfo> Update(IDirectoryInfo directoryInfo)
        {
            var dictionary = m_DirectorySnapshotStrategy.Update(directoryInfo);
            var filteredDictionary = FilterDictionary(dictionary);
            return filteredDictionary;
        }

        private Dictionary<string, IFileSystemInfo> FilterDictionary(Dictionary<string, IFileSystemInfo> dictionary)
        {
            Dictionary<string, IFileSystemInfo> filteredDictionary = new Dictionary<string, IFileSystemInfo>(dictionary);
            string[] keys = filteredDictionary.Keys.ToArray();
            foreach (var relativePath in keys)
            {
                if (m_ExcludedFileNames.Any(f => relativePath.StartsWith(f)))
                {
                    filteredDictionary.Remove(relativePath);
                }
            }
            return filteredDictionary;
        }
    }
}
