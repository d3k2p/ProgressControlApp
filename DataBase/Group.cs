namespace ProgressControlApp.DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Group")]
    public partial class Group
    {
        public Group()
        {
            Student = new HashSet<Student>();
        }

        public long ID { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public virtual ICollection<Student> Student { get; set; }
    }
}
