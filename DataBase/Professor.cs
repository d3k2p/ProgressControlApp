namespace ProgressControlApp.DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Professor")]
    public partial class Professor
    {
        public long ID { get; set; }

        [StringLength(30)]
        public string Name { get; set; }

        [StringLength(30)]
        public string LastName { get; set; }

        public long IDSubject { get; set; }

        public virtual Subject Subject { get; set; }
    }
}
