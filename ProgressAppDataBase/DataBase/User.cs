namespace ProgressAppDataBase.DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        public User()
        {
            Student = new HashSet<Student>();
        }

        public long ID { get; set; }

        [StringLength(2147483647)]
        public string Name { get; set; }

        [StringLength(2147483647)]
        public string LastName { get; set; }

        [StringLength(2147483647)]
        public string Login { get; set; }

        [StringLength(2147483647)]
        public string Password { get; set; }

        public long IDRole { get; set; }

        public virtual Role Role { get; set; }

        public virtual ICollection<Student> Student { get; set; }
    }
}
