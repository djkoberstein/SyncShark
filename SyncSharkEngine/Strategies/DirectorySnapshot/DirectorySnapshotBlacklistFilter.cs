using SyncShark.Interfaces;
using SyncShark.Engine.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SyncShark.Engine.Strategies.DirectorySnapshot
{
    public class DirectorySnapshotBlacklistFilter : IDirectorySnapshotStrategy
    {
        private IDirectorySnapshotStrategy m_DirectorySnapshotStrategy;
        private List<Regex> m_BlackList;

        public DirectorySnapshotBlacklistFilter(IDirectorySnapshotStrategy directorySnapshotStrategy)
            : this(directorySnapshotStrategy, new List<Regex>())
        {
        }

        public DirectorySnapshotBlacklistFilter(IDirectorySnapshotStrategy directorySnapshotStrategy, List<Regex> blackList)
        {
            m_DirectorySnapshotStrategy = directorySnapshotStrategy;
            m_BlackList = new List<Regex>();
            m_BlackList.Add(new Regex(@"^" + FileSystemSnapshotStrategy.STORE_FILE_NAME));
            m_BlackList.AddRange(blackList);
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
                if (m_BlackList.Any(regex => regex.IsMatch(relativePath)))
                {
                    filteredDictionary.Remove(relativePath);
                }
            }
            return filteredDictionary;
        }
    }
}
