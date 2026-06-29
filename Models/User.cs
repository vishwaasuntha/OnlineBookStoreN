using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineBookStore.Models
{
    public  class User
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        // ROLE: Customer or Admin
        public string Role { get; set; }

        [NotMapped]
        public DateTime CreatedAt { get; internal set; } = DateTime.Now;
    }
}
  