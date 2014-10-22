using SyncShark.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace SyncShark.Engine.FileSystem
{
    [Serializable]
    public class DirectoryInfoFacade : IDirectoryInfo
    {
        private DirectoryInfo m_DirectoryInfo;

        public DirectoryInfoFacade(DirectoryInfo directoryInfo)
        {
            m_DirectoryInfo = directoryInfo;
            FullName = m_DirectoryInfo.FullName;
            LastWriteTimeUtc = m_DirectoryInfo.LastWriteTimeUtc;
        }

        public string FullName { get; set; }
        public DateTime LastWriteTimeUtc { get; set; }

        IEnumerable<IFileSystemInfo> IDirectoryInfo.GetFileSystemInfos()
        {
            foreach (var directoryInfo in m_DirectoryInfo.EnumerateDirectories())
            {
                yield return new DirectoryInfoFacade(directoryInfo);
            }
            foreach (FileInfo fileInfo in m_DirectoryInfo.GetFiles("*.*", SearchOption.AllDirectories))
            {
                yield return new FileInfoFacade(new FileInfo(fileInfo.FullName));
            }
            yield break;
        }

        public void Create()
        {
            m_DirectoryInfo.Create();
        }

        public void Delete()
        {
            m_DirectoryInfo.Delete(true);
        }
    }
}
