using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CirclesManagement.Components
{
    public partial class Grade : IComparable<Grade>
    {
        public int CompareTo(Grade other)
        {
            return Title.CompareTo(other.Title);
        }
    }
}
