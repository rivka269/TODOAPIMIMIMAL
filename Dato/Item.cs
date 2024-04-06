 using System.ComponentModel.DataAnnotations;

namespace TodoApi.Dato
{
    public class Item
    {
        [Key]
        public int Id { get; set; }
        
        [MaxLength(100)]
        public string Name { get; set; } 
        
        public string IsComplete { get; set; } 
    }
}