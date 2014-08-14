using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncSharkEngine.FileSystem
{
    public class LocalFileSystem : IFileSystem
    {

        public void Create(IDirectoryInfo directoryInfo)
        {
            Directory.CreateDirectory(directoryInfo.FullName);
        }

        public void Delete(IFileSystemInfo fileSystemInfo)
        {
            if (fileSystemInfo is IFileInfo)
                Delete(fileSystemInfo as IFileInfo);
            else if (fileSystemInfo is IDirectoryInfo)
                Delete(fileSystemInfo as IDirectoryInfo);
            else
                throw new ArgumentException("Unrecognized fileSystemInfo");
        }

        public Stream OpenRead(IFileInfo fileInfo)
        {
            return File.Open(fileInfo.FullName, FileMode.Open, FileAccess.Read);
        }

        public Stream OpenWrite(IFileInfo fileInfo)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fileInfo.FullName));
            return File.Open(fileInfo.FullName, FileMode.Create, FileAccess.Write);
        }

        private void Delete(IDirectoryInfo directoryInfo)
        {
            if (Directory.Exists(directoryInfo.FullName))
                Directory.Delete(directoryInfo.FullName);
        }

        private void Delete(IFileInfo fileInfo)
        {
            if (File.Exists(fileInfo.FullName))
                File.Delete(fileInfo.FullName);
        }
    }
}
