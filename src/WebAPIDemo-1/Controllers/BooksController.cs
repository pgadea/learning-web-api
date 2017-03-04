using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebAPIDemo_1.Models;
using WebAPITest.Data.Models;

namespace WebAPIDemo_1.Controllers
{
    [Route("api/[controller]")]
    public class BooksController : Controller
    {
        private IBookStoreRepository repository;
        private IUrlHelper urlHelper;
        const int maxPageSize = 8;

        public BooksController(IBookStoreRepository _repository, IUrlHelper urlHelper)
        {
            this.repository = _repository;
            this.urlHelper = urlHelper;
        }

        [HttpGet(Name = "GetBooks")]
        public IEnumerable<Book> Get(string title = null, 
            string isbn =null, 
            string genre =null, 
            string sort = "Id", 
            string order = "Asc", 
            int pageNo = 1, 
            int pageSize = maxPageSize)
        {
            IList<Book> allbooks = this.repository.GetAllBooks();

            // Reset page size to max page size if requested page size is greater than max page size
            if (pageSize > maxPageSize)
            {
                pageSize = maxPageSize;
            }

            // Search on the string

            allbooks = allbooks.Where(b => (title == null || b.Title.Contains(title)))
                .Where(b => (isbn == null || b.ISBN.Replace("-", string.Empty) == isbn.Replace("-", string.Empty)))
                .Where(b => (genre == null || b.Genre == genre)).ToList();


            // Implement sorting
            IList<Book> sortedBooks = allbooks.ApplySorting(sort, order);

            // Creating metadata for paging
            int totalBooks = sortedBooks.Count;
            int totalPages = (int)Math.Ceiling((double)totalBooks / pageSize);

            var prevPageLink = pageNo == 1 ? string.Empty : this.urlHelper.Link("GetBooks",
                new
                {
                    sort = sort,
                    order = order,
                    pageNo = pageNo - 1,
                    pageSize = pageSize
                });

            var nextPageLink = pageNo == totalPages ? string.Empty : this.urlHelper.Link("GetBooks",
                new
                {
                    sort = sort,
                    order = order,
                    pageNo = pageNo + 1,
                    pageSize = pageSize
                });

            // page header info
            var pageInfoHeader = new
            {
                pageNo = pageNo,
                pageSize = pageSize,
                totalbooks = totalBooks,
                totalPages = totalPages,
                prevPageLink = prevPageLink,
                nextPageLink = nextPageLink
            };

            // adding page details in header
            Response.Headers.Add("X-PageInfo", JsonConvert.SerializeObject(pageInfoHeader));

            // 
            return sortedBooks.Skip((pageNo - 1) * pageSize).Take(pageSize);
        }

        [HttpGet("{id:int}/{includeAuthor:bool?}")]
        public Book Get(int Id, bool includeAuthor = false)
        {
            return this.repository.FindBook(Id, includeAuthor);
        }


        //[HttpGet("{id:int}/author")]
        //public Author Get(int id)
        //{
        //    return this.repository.FindBook(id, true).Author;
        //}

        [HttpPost]
        public IActionResult Post([FromBody]Book newBook)
        {
            if (newBook == null)
            {
                return HttpBadRequest("Data is not received in proper format");
            }

            var addedBook = this.repository.AddBook(newBook);

            if (addedBook == null)
            {
                return HttpBadRequest("Error while adding a book");
            }


            return CreatedAtRoute("GetBooks", new { controller = "Books", id = addedBook.Id }, addedBook);

        }

        [HttpPut("{id:int}")]
        public IActionResult Put(int id, [FromBody] Book book)
        {
            if (book == null)
            {
                return HttpBadRequest("Data is not received in proper format");
            }

            bool bookUpdated = this.repository.UpdateBook(book);

            if (!bookUpdated)
            {
                return HttpBadRequest("Error while updating the book");
            }

            return new HttpOkResult();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return HttpBadRequest("Data is not received in proper format");
            }

            var book = this.repository.FindBook(id, false);

            if (book == null)
            {
                return new HttpNotFoundResult();
            }

            bool isDeleted = this.repository.DeleteBook(id);

            if (!isDeleted)
            {
                return HttpBadRequest("Error while deleting the book");
            }

            return new HttpOkResult();
        }

        [HttpPatch("{id:int}")]
        public IActionResult Patch(int id, [FromBody] Book book)
        {
            if (book == null)
            {
                return HttpBadRequest("Data is not received in proper format");
            }

            bool bookUpdated = this.repository.UpdateBook(book);

            if (!bookUpdated)
            {
                return HttpBadRequest("Error while updating the book");
            }


            return new HttpOkResult();
        }

        //public IEnumerable<string> Get()
        //{
        //    return new List<string>() { "Brij", "John" };
        //}
    }

}
