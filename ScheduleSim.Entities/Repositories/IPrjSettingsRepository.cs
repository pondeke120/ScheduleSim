using ScheduleSim.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Entities.Repositories
{
    public interface IPrjSettingsRepository
    {
        PrjSettings FindRecently();
        void UpdateRecently(PrjSettings settings);
        void Insert(PrjSettings settings);
        void RemoveAll();
    }
}
