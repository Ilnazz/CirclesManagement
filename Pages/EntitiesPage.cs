using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CirclesManagement.Pages
{
    public abstract class EntitiesPage : Page
    {
        public bool ShowDeletedEntities;

        public abstract void AddEntity();

        public abstract void SaveChanges();
    }
}
