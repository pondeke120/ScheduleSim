using ScheduleSim.Core.Contexts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Core.Service
{
    public class DbFileAccessService : IDbFileAccessService
    {
        private AppContext appContext;

        public DbFileAccessService(AppContext appContext)
        {
            this.appContext = appContext;
        }

        public void CopyDbFile(string srcPath, string dstPath)
        {
            File.Copy(srcPath, dstPath, true);
            this.appContext.ProjectDbFile = dstPath;
        }

        public bool Exists(string path)
        {
            return
                File.Exists(path);
        }
    }
}
