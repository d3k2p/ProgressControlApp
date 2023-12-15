namespace ClassLibrary1.DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Mark")]
    public partial class Mark
    {
        public long ID { get; set; }

        [Required]
        [StringLength(2147483647)]
        public string Value { get; set; }

        public long? IDSubject { get; set; }

        public long? IDStudent { get; set; }

        public virtual Student Student { get; set; }

        public virtual Subject Subject { get; set; }
    }
}
