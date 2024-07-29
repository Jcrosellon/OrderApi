using System.ComponentModel.DataAnnotations;

namespace OrderApi.Models
{
    public class MessageRequest
    {
        [Key]
        public int Id { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Message { get; set; }
        public string? Name { get; internal set; }
    }
}
