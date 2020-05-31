using System.ComponentModel.DataAnnotations;

namespace ProjectData.Models
{
    public class City
    {
        [Key]
        public int city_id { get; set; }

        public int country_id { get; set; }

        public int region_id { get; set; }

        public string name { get; set; }
    }
}
