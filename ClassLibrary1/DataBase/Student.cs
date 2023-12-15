namespace ClassLibrary1.DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Student")]
    public partial class Student
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Student()
        {
            Mark = new HashSet<Mark>();
        }

        public long ID { get; set; }

        public long IDGroup { get; set; }

        public long IDUser { get; set; }

        public virtual Group Group { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Mark> Mark { get; set; }

        public virtual User User { get; set; }
        public string FullName
        {
            get
            {
                return User.LastName + " " + User.Name;
            }
        }
    }
}
