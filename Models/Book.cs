using System.ComponentModel.DataAnnotations;

namespace OnlineBookStore.Models
{
    public class Book : BaseEntity
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public int CategoryId { get; set; }

        public Category? Category { get; set; }

      //  public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
