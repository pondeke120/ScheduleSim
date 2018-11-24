using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScheduleSim.Entities.Models;
using ScheduleSim.Entities.Repositories;

namespace ScheduleSim.Core.Service
{
    public class MemberAccessService : IMemberAccessService
    {
        private IMemberRepository memberRepository;

        public MemberAccessService(
            IMemberRepository memberRepository)
        {
            this.memberRepository = memberRepository;
        }

        public IEnumerable<Member> GetAllMembers()
        {
            return
                this.memberRepository.Find();
        }

        public void Update(IEnumerable<Member> members)
        {
            this.memberRepository.RemoveAll();
            this.memberRepository.Insert(members);
        }
    }
}
