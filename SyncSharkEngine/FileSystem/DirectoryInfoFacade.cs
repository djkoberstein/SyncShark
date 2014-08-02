using SyncSharkEngine.Factories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncSharkEngine.FileSystem
{
    public class DirectoryInfoFacade : IDirectoryInfo
    {
        private DirectoryInfo m_DirectoryInfo;

        public DirectoryInfoFacade(DirectoryInfo directoryInfo)
        {
            m_DirectoryInfo = directoryInfo;
        }

        public string FullName { get { return m_DirectoryInfo.FullName; } }

        public IEnumerable<IFileInfo> GetFiles(string searchPattern, SearchOption searchOption)
        {
            foreach (FileInfo fileInfo in m_DirectoryInfo.GetFiles(searchPattern, searchOption))
            {
                yield return new FileInfoFacade(new FileInfo(fileInfo.FullName));
            }
            yield break;
        }

        public DateTime LastWriteTimeUtc
        {
            get { return m_DirectoryInfo.LastWriteTimeUtc; }
        }

        public void Delete()
        {
            m_DirectoryInfo.Delete();
        }
    }
}
