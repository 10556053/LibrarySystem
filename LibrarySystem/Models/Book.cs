using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace LibrarySystem.Models
{
    public class Book
    {
        /// <summary>
        /// 書本編號
        /// </summary>
        ///[MaxLength(5)]
        [DisplayName("書本編號")]
        public int BookId { get; set; }

        /// <summary>
        /// 書本名稱
        /// </summary>
        [DisplayName("書本名稱")]
        [Required(ErrorMessage = "此欄位必填")]
        public string BookName { get; set; }

        /// <summary>
        /// 書本類別
        /// </summary>
        [DisplayName("圖書類別")]
        public string BookClass { get; set; }

        /// <summary>
        /// 書本類別id
        /// </summary>
        [DisplayName("圖書類別-Id")]
        [Required(ErrorMessage = "此欄位必填")]
        public string BookClassId { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        [DisplayName("作者")]
        [Required(ErrorMessage = "此欄位必填")]
        public string Author { get; set; }

        /// <summary>
        /// 購買日期
        /// </summary>
        [DisplayName("購書日期")]
        [Required(ErrorMessage = "此欄位必填")]
        public string BookBoughtDate { get; set; }

        /// <summary>
        /// 出版商
        /// </summary>
        [DisplayName("出版商")]
        [Required(ErrorMessage = "此欄位必填")]
        public string BookPublisher { get; set; }

        /// <summary>
        /// 書本備註
        /// </summary>
        [DisplayName("內容簡介")]
        [AllowHtml]
        [Required(ErrorMessage = "此欄位必填")]
        public string BookNote { get; set; }

        /// <summary>
        /// 書本狀態
        /// </summary>
        [DisplayName("借閱狀態")]
        public string BookStatus { get; set; }

        /// <summary>
        /// 書本狀態-Id
        /// </summary>
        [DisplayName("借閱狀態-Id")]
        [Required(ErrorMessage = "此欄位必填")]
        public string BookStatusId { get; set; }


        /// <summary>
        /// 借閱人
        /// </summary>
        [DisplayName("借閱人")]
        public string BookKeeper { get; set; }

        public string BookKeeperFullName { get; set; }
        /// <summary>
        /// 借閱人
        /// </summary>
        [DisplayName("借閱人id")]
        [Required(ErrorMessage = "此欄位必填")]
        public string BookKeeperId { get; set; }

        ///// <summary>
        ///// 書籍數量
        ///// </summary>
        [DisplayName("書籍數量")]
        public string BookAmount { get; set; }

        /// <summary>
        /// 新增日期
        /// </summary>
        [DisplayName("新增日期")]
        public string CreateDate { get; set; }
        /// <summary>
        /// 新增人
        /// </summary>
        [DisplayName("新增人")]
        public string CreateUser { get; set; }
        /// <summary>
        /// 新增人id
        /// </summary>
        [DisplayName("新增人id")]
        public string CreateUserid { get; set; }
        /// <summary>
        /// 修改日期
        /// </summary>
        [DisplayName("修改日期")]
        public string ModDate { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        [DisplayName("修改人")]
        public string ModUser { get; set; }

        /// <summary>
        /// 修改人id
        /// </summary>
        [DisplayName("修改人id")]
        public string ModUserid { get; set; }
    }
}