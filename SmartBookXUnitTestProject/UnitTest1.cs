using LexiconC_Slutuppgift_SmartBook;

namespace SmartBookXUnitTestProject
{
    public class SmartBookTests
    {
        [Fact]
        public void AddBookToCollection_ShouldAddBookToLibraryCollection()
        {
            //Arrange
            var library = new Library();
            var author = new Author("John", "Smith");
            var cathegory = new Category("Fiction");
            var book = new Book("Test Book", author, cathegory, "1234567890");

            library.TryAddCategory(cathegory);
            library.TryAddAuthor(author);

            //Act
            library.AddBookToCollection(book);

            //Assert
            Assert.Single(library.Collection);
            Assert.Equal("Test Book", library.Collection[0].Title);
            Assert.Equal("Smith, John", library.Collection[0].Author.ToString());
            Assert.Equal("Fiction", library.Collection[0].Category.Name);
            Assert.Equal("1234567890", library.Collection[0].ISBN);
            
        }

        [Fact]
        public void TryAddAuthor_ShouldAddAuthorToAuthor()
        {
            //Arrange
            var library = new Library();
            var author = new Author("John", "Smith");

            //Act
            library.TryAddAuthor(author);

            //Assert
            Assert.Single(library.Authors);
            Assert.Equal("John", library.Authors[0].FirstName);
            Assert.Equal("Smith", library.Authors[0].LastName);
            Assert.Equal("Smith, John", library.Authors[0].ToString());
            

        }

        [Fact]
        public void TryAddCathegory_ShouldAddCathegoryToCathegorys()
        {
            //Arrange
            var library = new Library();
            var cathegory = new Category("Fiction");

            //Act
            library.TryAddCategory(cathegory);

            //Assert
            Assert.Single(library.Categorys);
            Assert.Equal("Fiction", library.Categorys[0].Name);
        }

        [Fact]
        public void RemoveBook_ShouldRemoveBookFromCollection()
        {
            //Arrange
            var library = new Library();
            var author = new Author("N", "N");
            var cathegory = new Category("Fiction");
            var book = new Book("Test Book", author, cathegory, "1234567890");

            library.TryAddCategory(cathegory);
            library.TryAddAuthor(author);
            library.AddBookToCollection(book);
            Assert.Single(library.Collection);
            //Act
            library.RemoveBook(book);

            //Assert
            Assert.Empty(library.Collection);
        }
    }
}
