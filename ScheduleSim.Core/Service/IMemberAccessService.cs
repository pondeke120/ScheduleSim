using ScheduleSim.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Core.Service
{
    public interface IMemberAccessService
    {
        void Update(IEnumerable<Member> members);
        IEnumerable<Member> GetAllMembers();
    }
}
