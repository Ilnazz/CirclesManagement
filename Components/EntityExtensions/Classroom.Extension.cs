using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CirclesManagement.Components
{
    public partial class Classroom : IComparable<Classroom>
    {
        public int CompareTo(Classroom other)
        {
            return Number.CompareTo(other.Number);
        }
    }
}
