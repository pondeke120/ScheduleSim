using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Entities
{
    public interface IDbConnectionFactory
    {
        IDbConnection Create();
        IDbTransaction BeginTransaction();
        void EndTransaction();
    }
}
