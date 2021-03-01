using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hobbie.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public long id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public bool isDeleted { get; set; }

        public virtual IList<Hobbie> hobbies { get; set; }
    }
}
