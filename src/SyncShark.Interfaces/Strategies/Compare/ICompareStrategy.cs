using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SyncShark.Interfaces
{
    public interface ICompareStrategy
    {
        IEnumerable<ISyncWorkItem> Compare(IDirectoryInfo leftDirectoryInfo, IDirectoryInfo rightDirectoryInfo);
    }
}
