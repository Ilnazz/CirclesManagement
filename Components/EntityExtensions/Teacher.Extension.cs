using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CirclesManagement.Components
{
    public partial class Teacher : IComparable<Teacher>
    {
        public string FullName {
            get { return $"{LastName} {FirstName[0]}. {Patronymic[0]}."; }
        }

        public int CompareTo(Teacher other)
        {
            return FullName.CompareTo(other);
        }
    }
}
