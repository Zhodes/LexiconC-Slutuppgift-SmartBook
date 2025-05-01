using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LexiconC_Slutuppgift_SmartBook
{
    public class Library
    {
        private List<Book> collection = new List<Book>();
        private List<Author> authors = new List<Author>();
        private List<Category> categorys = new List<Category>();

        public List<Book> Collection
        { 
            get { return collection; }
            
            private set { collection = value; }
        
        }

        public List<Author> Authors
        {
            get { return authors; }

            private set { authors = value; }
        }

        public List<Category> Categorys
        {
            get
            {
                return categorys;
            }

            private set
            {

                categorys = value;
            }
        }
        public void TrySetISBN(Book book, string ISBN)
        {
            if (checkISBN(ISBN))
            {
                throw new ArgumentException("There already exist a book with that ISBN");
            }
            else
            {
                book.ISBN = ISBN; 
            }
        }

        private bool checkISBN(string ISBN)
        {
            return Collection.Any(b => b.ISBN == ISBN);
        }

        public void AddBookToCollection(Book book)
        {
            if (!authors.Contains(book.Author)) throw new ArgumentException("Author must exist in the library.");
            if (!categorys.Contains(book.Category)) throw new ArgumentException("Category must exist in the library.");
            if (checkISBN(book.ISBN)) throw new ArgumentException("There already exist a book with that ISBN");
            Collection.Add(book);
        }

        public void TryAddCathegory(Category cathegory)
        {
            if (categorys.Any(b => b.Equals(cathegory)))
            {
                throw new ArgumentException("That title already exists");
            }
            else categorys.Add(cathegory);
        }



        public void TryAddAuthor(Author author)
        {
            if (authors.Any(b => b.Equals(author)))
            {
                throw new ArgumentException("That title already exists");
            }
            else authors.Add(author);
        }

        public List<Book> ListAllBooks()
        {
            var orderedBooks = Collection
            .OrderBy(book => book.Title);
            List<Book> orderedCollection = orderedBooks.ToList();
            return orderedCollection;
            
        }

        public void LoadLibraryFromFile()
        {
            Collection = JsonSerializer.Deserialize<List<Book>>(File.ReadAllText("collection.json"));
            authors = JsonSerializer.Deserialize<List<Author>>(File.ReadAllText("authors.json"));
            categorys = JsonSerializer.Deserialize<List<Category>>(File.ReadAllText("categorys.json"));
        }

        public void SaveLibraryToFile()
        {
            File.WriteAllText("collection.json", JsonSerializer.Serialize(Collection));
            File.WriteAllText("authors.json", JsonSerializer.Serialize(authors));
            File.WriteAllText("categorys.json", JsonSerializer.Serialize(categorys));
        }

        public void RemoveBook(Book book)
        {
            Console.WriteLine($"{book.Title} has been removed");
            Collection.Remove(book);

        }
    }
}
