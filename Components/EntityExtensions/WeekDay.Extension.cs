using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CirclesManagement.Components
{
    public partial class WeekDay : IComparable<WeekDay>
    {
        public int CompareTo(WeekDay other)
        {
            return Title.CompareTo(other.Title);
        }
    }
}
