using SyncShark.Engine.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using SyncShark.Interfaces;

namespace SyncShark.Engine.Strategies.DirectorySnapshot
{
    public class FileSystemSnapshotStrategy : IDirectorySnapshotStrategy
    {
        public const string STORE_FILE_NAME = @"sync.ss_db";

        private IDirectorySnapshotStrategy m_DirectorySnapshotStrategy;

        public FileSystemSnapshotStrategy(MemorySnapshotStrategy inMemorySnapshotStrategy)
        {
            m_DirectorySnapshotStrategy = inMemorySnapshotStrategy;
        }

        public Dictionary<string, IFileSystemInfo> Create(IDirectoryInfo directoryInfo)
        {
            var snapShot = m_DirectorySnapshotStrategy.Create(directoryInfo);
            SaveToDisk(directoryInfo, snapShot);
            return snapShot;
        }

        public Dictionary<string, IFileSystemInfo> Read(IDirectoryInfo directoryInfo)
        {
            return LoadFromDisk(directoryInfo);
        }

        public Dictionary<string, IFileSystemInfo> Update(IDirectoryInfo directoryInfo)
        {
            var snapshot = m_DirectorySnapshotStrategy.Update(directoryInfo);
            SaveToDisk(directoryInfo, snapshot);
            return snapshot;
        }

        private void SaveToDisk(IDirectoryInfo directoryInfo, Dictionary<string, IFileSystemInfo> dictionary)
        {
            string filePath = Path.Combine(directoryInfo.FullName, STORE_FILE_NAME);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (Stream stream = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                binaryFormatter.Serialize(stream, dictionary);
            }
        }

        private Dictionary<string, IFileSystemInfo> LoadFromDisk(IDirectoryInfo directoryInfo)
        {
            var directoryStore = new Dictionary<string, IFileSystemInfo>();
            string filePath = Path.Combine(directoryInfo.FullName, STORE_FILE_NAME);
            if (File.Exists(filePath))
            {
                try
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    using (Stream stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                    {
                        directoryStore = (Dictionary<string, IFileSystemInfo>)binaryFormatter.Deserialize(stream);
                    }
                }
                catch (SerializationException ex)
                {
                    Console.WriteLine("Error reading saved state. Deleting file. " + ex.Message);
                    File.Delete(filePath);
                }
                catch (InvalidCastException ex)
                {
                    Console.WriteLine("Error reading saved state. Deleting file. " + ex.Message);
                    File.Delete(filePath);
                }
            }
            return directoryStore;
        }
    }
}
