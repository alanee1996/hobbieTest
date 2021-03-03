using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hobbie.Entities
{
    public class Hobbie
    {
        [Key]
        [Required]
        public long id { get; set; }
        [Required]
        [MaxLength(100)]
        public string hobbie { get; set; }
        [Required]
        [ForeignKey("users")]
        public long userId { get; set; }
    }
}
