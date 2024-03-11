using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBContex.Models
{
    [Table("Images")]
    public class Image
    {
        [Key]
        public int ImageId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public byte[] Data { get; set; }
    }
}
