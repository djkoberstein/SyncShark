using SyncSharkEngine.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SyncSharkEngine.Strategies.DirectorySnapshot
{
    public class FileSystemSnapshotStrategy : IDirectorySnapshotStrategy
    {
        private Dictionary<string, Dictionary<string, IFileInfo>> m_InMemoryStore;
        private InMemorySnapshotStrategy m_InMemorySnapshotStrategy;

        public FileSystemSnapshotStrategy(InMemorySnapshotStrategy inMemorySnapshotStrategy, IEnumerable<IDirectoryInfo> directoryInfos)
        {
            m_InMemorySnapshotStrategy = inMemorySnapshotStrategy;
            LoadFromDisk(directoryInfos);
        }

        public Dictionary<string, IFileInfo> Create(IDirectoryInfo directoryInfo)
        {
            var snapShot = m_InMemorySnapshotStrategy.Create(directoryInfo);
            SaveToDisk();
            return snapShot;
        }

        public Dictionary<string, IFileInfo> Read(IDirectoryInfo directoryInfo)
        {
            return m_InMemorySnapshotStrategy.Read(directoryInfo);
        }

        public Dictionary<string, IFileInfo> Update(IDirectoryInfo directoryInfo)
        {
            var snapshot = m_InMemorySnapshotStrategy.Update(directoryInfo);
            SaveToDisk();
            return snapshot;
        }

        public Dictionary<string, IFileInfo> ReadOrCreate(IDirectoryInfo directoryInfo)
        {
            var snapshot = m_InMemorySnapshotStrategy.ReadOrCreate(directoryInfo);
            SaveToDisk();
            return snapshot;
        }

        public void Delete(IDirectoryInfo directoryInfo)
        {
            m_InMemorySnapshotStrategy.Delete(directoryInfo);
            SaveToDisk();
        }

        public bool Exists(IDirectoryInfo directoryInfo)
        {
            return m_InMemorySnapshotStrategy.Exists(directoryInfo);
        }

        private void SaveToDisk()
        {
            string filePath = GetFilePath(m_InMemoryStore.Keys);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (Stream stream = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                binaryFormatter.Serialize(stream, m_InMemoryStore);
            }
        }

        private void LoadFromDisk(IEnumerable<IDirectoryInfo> directoryInfos)
        {
            m_InMemoryStore = new Dictionary<string, Dictionary<string, IFileInfo>>();
            string filePath = GetFilePath(directoryInfos.Select(d => d.FullName));
            if (File.Exists(filePath))
            {
                try
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    using (Stream stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                    {
                        m_InMemoryStore = (Dictionary<string, Dictionary<string, IFileInfo>>)binaryFormatter.Deserialize(stream);
                    }

                }
                catch (SerializationException ex)
                {
                    Console.WriteLine("Error reading saved state. Deleting file. " + ex.Message);
                    File.Delete(filePath);
                }
            }
        }

        private string GetFilePath(IEnumerable<string> directoryNames)
        {
            string uid = string.Concat(directoryNames);
            string hashedUid = CalculateMD5Hash(uid);
            string filePath = string.Format(@"{0}\SharkSync\{1}.dat", Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), hashedUid);
            return filePath;
        }

        private string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
