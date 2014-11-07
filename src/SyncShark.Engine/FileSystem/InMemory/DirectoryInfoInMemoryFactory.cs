using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncShark.Engine.FileSystem.InMemory
{
    public class DirectoryInfoInMemoryFactory
    {
        public DirectoryInfoInMemory GetInstance(string fullName)
        {
            if (!Directory.Exists(fullName))
            {
                throw new ArgumentException("fullName must represent a directory that exists!");
            }

            var directoryInfoFacade = new DirectoryInfoFacade(new DirectoryInfo(fullName));
            var directoryInfo = new DirectoryInfoInMemory(directoryInfoFacade);
            Build(directoryInfo);
            return directoryInfo;
        }

        private void Build(DirectoryInfoInMemory directoryInfo)
        {
            foreach (var subDirectoryPath in Directory.EnumerateDirectories(directoryInfo.FullName, "*.*", SearchOption.TopDirectoryOnly))
            {
                var subDirectoryInfoFacade = new DirectoryInfoFacade(new DirectoryInfo(subDirectoryPath));
                var subDirectoryInfo = new DirectoryInfoInMemory(subDirectoryInfoFacade);
                Build(subDirectoryInfo);
                directoryInfo.DirectoryInfos.Add(subDirectoryInfo);
            }

            foreach (var filePath in Directory.EnumerateFiles(directoryInfo.FullName, "*.*", SearchOption.TopDirectoryOnly))
            {
                var fileInfoFacade = new FileInfoFacade(new FileInfo(filePath));
                var fileInfo = new FileInfoInMemory(fileInfoFacade);
            }
        }
    }
}
