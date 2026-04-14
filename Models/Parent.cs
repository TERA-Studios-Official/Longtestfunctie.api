using System.ComponentModel.DataAnnotations;

namespace Longfunctie.api.Models
{
    public class Parent
    {
        [Key]
        [MaxLength(20)]
        public string? Name { get; set; }

        public string? PinHash { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<Child>? Children { get; set; }
    }
}
