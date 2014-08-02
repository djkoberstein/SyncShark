using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncSharkEngine.DataModel
{
    public interface IDirectoryInfo
    {
        string FullName { get; }
        IEnumerable<IFileInfo> GetFiles(string searchPattern, SearchOption searchOption);
    }
}
