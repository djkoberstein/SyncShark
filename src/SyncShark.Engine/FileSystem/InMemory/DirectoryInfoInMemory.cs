using SyncShark.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncShark.Engine.FileSystem
{
    public class DirectoryInfoInMemory : IDirectoryInfo
    {
        public List<DirectoryInfoInMemory> DirectoryInfos { get; set; }
        public List<FileInfoInMemory> FileInfos { get; set; }
        private DateTime m_LastWriteTimeUtc;
        private string m_FullName;

        public DirectoryInfoInMemory(IDirectoryInfo directoryInfo)
        {
            DirectoryInfos = new List<DirectoryInfoInMemory>();
            FileInfos = new List<FileInfoInMemory>();
            m_LastWriteTimeUtc = directoryInfo.LastWriteTimeUtc;
            m_FullName = directoryInfo.FullName;
        }

        public IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos()
        {
            foreach (var directoryInfo in DirectoryInfos)
            {
                yield return directoryInfo;
                foreach (var subDirectoryInfo in directoryInfo.EnumerateFileSystemInfos())
                {
                    yield return subDirectoryInfo;
                }
            }
            foreach (var fileInfo in FileInfos)
            {
                yield return fileInfo;
            }
            yield break;
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
            Directory.Delete(m_FullName);
        }
    }
}
