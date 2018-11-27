using ScheduleSim.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Entities.Repositories
{
    public interface IMemberRepository
    {
        void Insert(IEnumerable<Member> members);
        IEnumerable<Member> Find();
        void RemoveAll();
        int GetCurrentIndex();
    }
}
