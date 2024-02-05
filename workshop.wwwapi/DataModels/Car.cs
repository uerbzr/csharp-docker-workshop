using System.ComponentModel.DataAnnotations.Schema;

namespace workshop.wwwapi.DataModels
{
    [Table("cars")]
    public class Car
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("make")]
        public string Make { get; set; }
        [Column("model")]
        public string Model { get; set; }
    }
}
