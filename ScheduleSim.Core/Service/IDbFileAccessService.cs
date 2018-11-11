using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Core.Service
{
    public interface IDbFileAccessService
    {
        void CopyDbFile(string srcPath, string dstPath);
        bool Exists(string path);
    }
}
