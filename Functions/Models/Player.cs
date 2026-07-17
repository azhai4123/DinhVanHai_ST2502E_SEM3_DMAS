using System;
namespace Functions.Models
{
    public class Player
    {
        public Guid PlayerId { get; set; }
        public string PlayerName { get; set; } = string.Empty;
        public string? FullName { get; set; }
        public int Age { get; set; }
        public int Level { get; set; }
        public string? Email { get; set; }
    }
}
