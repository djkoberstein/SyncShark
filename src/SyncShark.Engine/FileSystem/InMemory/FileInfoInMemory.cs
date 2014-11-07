using SyncShark.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncShark.Engine.FileSystem
{
    public class FileInfoInMemory : IFileInfo
    {
        private string m_FullName;
        private DateTime m_LastWriteTimeUtc;

        public FileInfoInMemory(IFileInfo fileInfo)
        {
            m_FullName = fileInfo.FullName;
            m_LastWriteTimeUtc = fileInfo.LastWriteTimeUtc;
        }

        public Stream OpenRead()
        {
            return File.OpenRead(m_FullName);
        }

        public Stream OpenWrite()
        {
            return File.OpenWrite(m_FullName);
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
            File.Delete(m_FullName);
        }
    }
}
