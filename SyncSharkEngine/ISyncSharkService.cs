﻿using SyncSharkEngine.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncSharkEngine
{
    public interface ISyncSharkService
    {
        void CompareAndSync(IDirectoryInfo leftDirectoryInfo, IDirectoryInfo rightDirectoryInfo);
        IEnumerable<ISyncWorkItem> Compare(IDirectoryInfo leftDirectoryInfo, IDirectoryInfo rightDirectoryInfo);
        void Sync(IEnumerable<ISyncWorkItem> syncWorkItems);
    }
}