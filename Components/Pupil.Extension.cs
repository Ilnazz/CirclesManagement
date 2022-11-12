using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CirclesManagement.Components
{
    public partial class Pupil
    {
        public string FullName
        {
            get { return $"{LastName} {FirstName[0]}. {Patronymic[0]}."; }
        }
    }
}
