//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CirclesManagement.ADO
{
    using System;
    using System.Collections.Generic;
    
    public partial class Circle_Pupil
    {
        public int CircleID { get; set; }
        public int PupilID { get; set; }
        public bool IsAttending { get; set; }
    
        public virtual Circle Circle { get; set; }
        public virtual Pupil Pupil { get; set; }
    }
}
