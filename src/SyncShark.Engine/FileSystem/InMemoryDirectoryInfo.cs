using SyncShark.Engine.Utilities;
using SyncShark.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncShark.Engine.FileSystem
{
    public class InMemoryDirectoryInfo : IDirectoryInfo
    {
        List<IDirectoryInfo> m_DirectoryInfos;
        List<IFileInfo> m_FileInfos;
        private string m_FullName;
        private DateTime m_LastWriteTimeUtc;

        public InMemoryDirectoryInfo(IDirectoryInfo directoryInfo)
        {
            m_FullName = FullName;
            m_LastWriteTimeUtc = LastWriteTimeUtc;
            m_DirectoryInfos = directoryInfo.EnumerateDirectories("*.*", SearchOption.TopDirectoryOnly).ToList();
            m_FileInfos = directoryInfo.GetFiles("*.*", SearchOption.TopDirectoryOnly).ToList();
        }

        public IEnumerable<IFileSystemInfo> GetFileSystemInfos()
        {
            foreach (var directoryInfo in EnumerateDirectories("*.*", SearchOption.AllDirectories))
            {
                yield return directoryInfo;
            }
            foreach (var fileInfo in GetFiles("*.*", SearchOption.AllDirectories))
            {
                yield return fileInfo;
            }
        }

        public void Create()
        {
            Directory.CreateDirectory(m_FullName);
        }

        public string FullName
        {
            get { return m_FullName; }
        }

        public DateTime LastWriteTimeUtc
        {
            get { return m_LastWriteTimeUtc; }
        }

        public void Delete()
        {
            m_DirectoryInfos.Clear();
            m_FileInfos.Clear();
            Directory.Delete(m_FullName);
        }

        public IEnumerable<IFileInfo> GetFiles(string searchPattern, SearchOption searchOption)
        {
            foreach (var fileInfo in m_FileInfos)
            {
                if (StringUtils.Like(fileInfo.FullName, searchPattern))
                {
                    yield return fileInfo;
                }
            }

            if (searchOption.Equals(SearchOption.AllDirectories))
            {
                foreach (var directoryInfo in m_DirectoryInfos)
                {
                    foreach (var fileInfo in directoryInfo.GetFiles(searchPattern, searchOption))
                    {
                        yield return fileInfo;
                    }
                }
            }
        }

        public IEnumerable<IDirectoryInfo> EnumerateDirectories(string searchPattern, SearchOption searchOption)
        {
            foreach (var directoryInfo in m_DirectoryInfos)
            {
                if (StringUtils.Like(directoryInfo.FullName, searchPattern))
                {
                    yield return directoryInfo;
                    foreach (var subDirectoryInfo in directoryInfo.EnumerateDirectories(searchPattern, searchOption))
                    {
                        if (StringUtils.Like(subDirectoryInfo.FullName, searchPattern))
                        {
                            yield return subDirectoryInfo;
                        }
                    }

                }
            }
        }
    }
}
