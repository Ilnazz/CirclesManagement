using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CirclesManagement.Components
{
    public partial class Pupil : IComparable<Pupil>
    {
        public string LastNameAndInitials
        {
            get { return $"{LastName} {(FirstName.Length > 0 ? $"{FirstName[0]}" : "")}. {(Patronymic.Length > 0 ? $"{Patronymic[0]}" : "")}."; }
        }

        public string FullName
        {
            get { return $"{LastName} {FirstName} {Patronymic}"; }
        }

        public int CompareTo(Pupil other)
        {
            return FullName.CompareTo(other.FullName);
        }
    }
}
