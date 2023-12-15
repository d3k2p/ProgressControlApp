namespace ProgressControlApp.DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Role")]
    public partial class Role
    {
        public Role()
        {
            User = new HashSet<User>();
        }

        public long ID { get; set; }

        [StringLength(30)]
        public string Name { get; set; }

        public virtual ICollection<User> User { get; set; }
    }
}
