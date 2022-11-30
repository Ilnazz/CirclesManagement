using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CirclesManagement.Components
{
    public partial class Teacher : IComparable<Teacher>
    {
        public string LastNameAndInitials
        {
            get { return $"{LastName} {(FirstName.Length > 0 ? $"{FirstName[0]}" : "")}. {(Patronymic.Length > 0 ? $"{Patronymic[0]}" : "")}."; }
        }

        public string FullName
        {
            get { return $"{LastName} {FirstName} {Patronymic}"; }
        }

        public string Login
        {
            get { return Users.First().Login; }
            set { Users.First().Login = value; }
        }

        public string Password
        {
            get { return Users.First().Password; }
            set { Users.First().Password = value; }
        }

        public int CompareTo(Teacher other)
        {
            return FullName.CompareTo(other);
        }
    }
}
