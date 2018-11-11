using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Entities.Repositories
{
    public interface IDbVersionRepository
    {
        void Regist(string version, string explain);
    }
}
