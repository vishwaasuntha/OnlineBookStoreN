using Xunit;
using OnlineBookStore.Models;
namespace OnlineBookSystem.Tests
{
    public class BookTests
    {
        [Fact]
        public void Book_Properties_Work_Correctly()
        {
            // Arrange
            Book book = new Book();

            // Act
            book.Id = 2;
            book.Title = "Tiny Dogs";
            book.Author = "Rose Lihou";
            book.Price = 5;
            book.CreatedAt = DateTime.Now;

            // Assert
            Assert.Equal("Tiny Dogs", book.Title);
            Assert.Equal("Rose Lihou", book.Author);
            Assert.Equal(5, book.Price);
            Assert.Equal(2, book.Id);
        }
    }
}