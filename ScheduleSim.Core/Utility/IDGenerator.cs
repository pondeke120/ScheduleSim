using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Core.Utility
{
    public class IDGenerator : IIDGenerator
    {
        private int _currnetIndex;

        public IDGenerator()
        {
            this._currnetIndex = 1;
        }

        public int CurrentIndex
        {
            get
            {
                return _currnetIndex;
            }
        }

        public int CreateNewId()
        {
            return _currnetIndex++;
        }

        public void Reset()
        {
            this._currnetIndex = 1;
        }

        public void SetCurrentIndex(int index)
        {
            this._currnetIndex = index;
        }
    }
}
