//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CirclesManagement.Components
{
    using System;
    using System.Collections.Generic;
    
    public partial class Circle
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Circle()
        {
            this.Circle_Pupil = new HashSet<Circle_Pupil>();
            this.Timetables = new HashSet<Timetable>();
        }
    
        public int ID { get; set; }
        public string Title { get; set; }
        public bool IsWorking { get; set; }
        public int MaxNumberOfPupils { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Circle_Pupil> Circle_Pupil { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Timetable> Timetables { get; set; }
    }
}
