using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CirclesManagement.Components
{
    public partial class Group
    {
        public string Title
        {
            get { return $"{Teacher.LastNameAndInitials} - {Circle.Title}"; }
        }
    }
}
