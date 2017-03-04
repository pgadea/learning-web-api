using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPITest.Data.Models
{
    public class Book
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Genre { get; set; }

        public DateTime PublishedOn { get; set; }

        public int NoofPages { get; set; }

        public string ISBN { get; set; }

        public Author Author { get; set; }
    }
}
