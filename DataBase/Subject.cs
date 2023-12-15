namespace ProgressControlApp.DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Subject")]
    public partial class Subject
    {
        public Subject()
        {
            Mark = new HashSet<Mark>();
            Professor = new HashSet<Professor>();
        }

        public long ID { get; set; }

        [StringLength(30)]
        public string Name { get; set; }

        public virtual ICollection<Mark> Mark { get; set; }

        public virtual ICollection<Professor> Professor { get; set; }
    }
}
