using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPITest.Data.Models;

namespace WebAPIDemo_1.Controllers
{
    [Route("api")]

    public class AuthorsController : Controller
    {
        private IBookStoreRepository repository;
        public AuthorsController(IBookStoreRepository _repository)
        {
            this.repository = _repository;
        }


        [HttpGet("authors")]
        public IEnumerable<Author> Get()
        {
            return this.repository.GetAllAuthors();
        }

        [HttpGet("authors/{id:int}")]
        [HttpGet("books/{bookid:int}/Author")]
        public IActionResult Get(int bookid, int? id)
        {
            if (id == null)
            {
                Book book = this.repository.FindBook(bookid, true);

                if(book != null)
                {
                    return Ok(book.Author);
                }

                return HttpNotFound();
            }
            else
            {
                return Ok(this.repository.FindAutor(id.Value, true));
            }    
        }

    }
}
