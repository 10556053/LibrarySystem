using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibrarySystem.Controllers
{
    public class BookController : Controller
    {

        Models.CodeService codeService = new Models.CodeService();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost()]
        public JsonResult GetBookData(string bookName, string bookClass, string bookKeeper, string bookStatus)
        {
            Models.BookService bookService = new Models.BookService();
            Models.BookSearchArg bookSearchArg = new Models.BookSearchArg();
            bookSearchArg.BookName = bookName;
            bookSearchArg.BookClassId = bookClass;
            bookSearchArg.BookKeeperId = bookKeeper;
            bookSearchArg.BookStatusId = bookStatus;

            var result = bookService.GetBookByCondtioin(bookSearchArg);
            var jsonResut = Json(result);
            return jsonResut;
        }

        public JsonResult GetBookDataById(string bookId)
        {
            Models.BookService bookService = new Models.BookService();
            var result = bookService.GetBookById(bookId);
            var jsonResut = Json(result);
            return jsonResut;
        }
        [HttpPost]
        public JsonResult GetAllBook()
        {
            Models.BookService bookService = new Models.BookService();
            var result = bookService.GetAllBooks();
            var jsonResut = Json(result);
            return jsonResut;
        }
        [HttpPost]
        public JsonResult GetClassData()
        {
            var result = this.codeService.GetBookTypeCodeTable();
            var jsonResut = Json(result);
            return jsonResut;
        }
        [HttpPost]
        public JsonResult GetKeeperData()
        {
            var result = this.codeService.GetUserCodeTable();
            var jsonResut = Json(result);
            return jsonResut;
        }
        [HttpPost]
        public JsonResult GetStatusData()
        {
            var result = this.codeService.GetBookStatusCodeTable("BOOK_STATUS");
            var jsonResut = Json(result);
            return jsonResut;
        }

        [HttpGet()]
        public ActionResult UpdateBook()
        {
            return View();
        }

        [HttpPost()]
        public JsonResult UpdateBook(Models.Book myBookData) //變數名稱可以改
        {
            try
            {
                Models.BookService bookService = new Models.BookService();
                if (bookService.UpdateBook(myBookData))
                {
                    var msg = CreateMessage(true, "更新成功");
                    return this.Json(msg);
                }
                else
                {
                    //這邊可以用json object打包更多信息回傳給前端
                    var msg = CreateMessage(false, "更新失敗");
                    return this.Json(msg);
                }
            }
            catch (Exception ex)
            {
                //這邊可以用json object打包更多信息回傳給前端
                Console.WriteLine(ex);
                var msg = CreateMessage(false, "發生意外錯誤"+ex.Message);
                return this.Json(msg);
            }
        }

        [HttpPost()]
        public JsonResult DeleteBook(string bookId)
        {
            try
            {
                Models.BookService bookService = new Models.BookService();
                if (bookService.BookIsOccupied(bookId))
                {
                    ///如果書被佔據，就return false，不要讓書被刪除
                    var errMsg = CreateMessage(false, "書本已被佔據");
                    return this.Json(errMsg);
                }
                else
                {
                    if (bookService.DeleteBookById(bookId))
                    {
                        bookService.DeleteLendRecordByBookId(bookId);
                        var msg = CreateMessage(true, "刪除成功");
                        return this.Json(msg);
                    }
                    else
                    {
                        var errMsg = CreateMessage(false, "錯誤，可能是此書已被借走");
                        return this.Json(errMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                var errMsg = CreateMessage(false, "發生意外錯誤" + ex.Message);
                //回傳完整ex
                return this.Json(errMsg);
            }
        }
        [HttpPost()]
        public JsonResult InsertBook(Models.Book book)
        {
            try
            {
                Models.BookService bookService = new Models.BookService();
                if (bookService.InsertBook(book))
                {
                    var msg = CreateMessage(true, "新增成功");
                    return this.Json(msg);
                }
                else
                {
                    var msg = CreateMessage(false, "新增失敗");
                    return this.Json(msg);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                var msg = CreateMessage(false, ex.ToString());
                return this.Json(msg);
            }
        }

        [HttpGet()]
        public ActionResult InspectLendRecord()
        {
            return View();
        }

        [HttpPost()]
        public JsonResult InspectLendRecord(string bookId)
        {
            try
            {
                Models.BookService bookService = new Models.BookService();
                if (bookService.GetLendRecordByBookId(bookId).Count > 0)
                {
                    var result = bookService.GetLendRecordByBookId(bookId);
                    var jsonResut = Json(result);
                    return jsonResut;
                }
                else
                {
                    return this.Json("");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                var msg = CreateMessage(false, ex.Message);
                return this.Json(msg);
            }
        }

        public object CreateMessage(bool state , string msg) {
            var data = new
            {
                status = state,
                message = msg
            };
            return data;
        }
    }
}