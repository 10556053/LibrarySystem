using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibrarySystem.Models
{
    public class BookLendRecord
    {
        public string BookLendDate { get; set; }
        public string BookKeeperId { get; set; }
        public string UserEName { get; set; }
        public string UserCName { get; set; }
        public string UserFullName { get; set; }
    }
}