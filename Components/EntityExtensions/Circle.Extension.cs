using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CirclesManagement.Components
{
    public partial class Circle : IComparable<Circle>
    {
        public int CompareTo(Circle other)
        {
            return Title.CompareTo(other.Title);
        }
    }
}
