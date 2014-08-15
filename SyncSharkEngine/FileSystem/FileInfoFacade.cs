using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncSharkEngine.FileSystem
{
    [Serializable]
    public class FileInfoFacade : IFileInfo
    {
        private FileInfo m_FileInfo;

        public FileInfoFacade(FileInfo fileInfo)
        {
            m_FileInfo = fileInfo;
        }

        public string FullName
        {
            get { return m_FileInfo.FullName; }
        }

        public DateTime LastWriteTimeUtc
        {
            get
            {
                return m_FileInfo.LastWriteTimeUtc;
            }
            set
            {
                m_FileInfo.LastWriteTimeUtc = value;
            }
        }

        public Stream OpenRead()
        {
            return m_FileInfo.OpenRead();
        }

        public Stream OpenWrite()
        {
            return m_FileInfo.OpenWrite();
        }

        public void Delete()
        {
            m_FileInfo.Delete();
        }
    }
}
