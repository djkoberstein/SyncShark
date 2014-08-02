using SyncSharkEngine.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SyncSharkEngine
{
    public class DirectorySnapshotService : IDirectorySnapshotService
    {
        private Dictionary<string, Dictionary<string, IFileInfo>> m_InMemoryStore;

        public DirectorySnapshotService(IEnumerable<IDirectoryInfo> directoryInfos)
        {
            LoadFromDisk(directoryInfos);
        }

        public Dictionary<string, IFileInfo> Create(IDirectoryInfo directoryInfo)
        {
            Dictionary<string, IFileInfo> dicionary = new Dictionary<string, IFileInfo>();
            m_InMemoryStore.Add(directoryInfo.FullName, dicionary);
            foreach (var fileInfo in directoryInfo.GetFiles("*.*", SearchOption.AllDirectories))
            {
                string relativePath = fileInfo.FullName.Replace(directoryInfo.FullName, "");
                dicionary.Add(relativePath, fileInfo);
            }
            SaveToDisk();
            return dicionary;
        }

        public bool Exists(IDirectoryInfo directoryInfo)
        {
            return m_InMemoryStore.ContainsKey(directoryInfo.FullName);
        }

        public Dictionary<string, IFileInfo> Read(IDirectoryInfo directoryInfo)
        {
            return m_InMemoryStore[directoryInfo.FullName];
        }

        public Dictionary<string, IFileInfo> Update(IDirectoryInfo directoryInfo)
        {
            if (Exists(directoryInfo))
            {
                Delete(directoryInfo);
            }
            return Create(directoryInfo);
        }

        public void Delete(IDirectoryInfo directoryInfo)
        {
            m_InMemoryStore.Remove(directoryInfo.FullName);
            SaveToDisk();
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
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                using (Stream stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    m_InMemoryStore = (Dictionary<string, Dictionary<string, IFileInfo>>)binaryFormatter.Deserialize(stream);
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
