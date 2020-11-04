using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksApi.Models
{
    public class BookRabbitMessage
    {
        public string pattern { get; set; }
        public Book data { get; set; }
    }
}
