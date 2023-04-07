using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace LibrarySystem.Models
{
    public class BookSearchArg
    {
        [DisplayName("書本名稱")]
        public string BookName { get; set; }

        /// <summary>
        /// 書本類別
        /// </summary>
        [DisplayName("書本類別")]
        public string BookClass { get; set; }

        [DisplayName("書本類別-Id")]
        public string BookClassId { get; set; }

        /// <summary>
        /// 借閱人
        /// </summary>
        [DisplayName("借閱人")]
        public string BookKeeper { get; set; }

        [DisplayName("借閱人id")]
        public string BookKeeperId { get; set; }

        /// <summary>
        /// 書本狀態
        /// </summary>
        [DisplayName("書本狀態")]
        public string BookStatus { get; set; }

        [DisplayName("書本狀態-Id")]
        public string BookStatusId { get; set; }
    }
}