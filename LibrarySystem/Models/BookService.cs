using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace LibrarySystem.Models
{
    public class BookService
    {
        /// <summary>
        /// 取得DB連線字串
        /// </summary>
        /// <returns></returns>
        private string GetDBConnectionString()
        {
            return
                System.Configuration.ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString.ToString();
        }
        /// <summary>
        /// 依照條件取得客戶資料
        /// </summary>
        /// <returns></returns>
        public List<Models.Book> GetBookByCondtioin(Models.BookSearchArg arg)
        {

            DataTable dt = new DataTable();
            string sql = @"SELECT
	                        bd.BOOK_ID
                           ,bd.BOOK_NAME
                           ,bd.BOOK_CLASS_ID
                           ,bc.BOOK_CLASS_NAME
                           ,bd.BOOK_AUTHOR
                           ,bd.BOOK_BOUGHT_DATE
                           ,bd.BOOK_PUBLISHER
                           ,bd.BOOK_NOTE
                           ,bd.BOOK_STATUS
                           ,bcd.CODE_NAME
                           ,bd.BOOK_KEEPER
                           ,mm.USER_ENAME
                           ,mm.USER_ENAME+'-'+mm.USER_CNAME AS USER_FULLNAME
                        FROM BOOK_DATA AS bd
                        LEFT JOIN BOOK_CLASS AS bc
	                        ON (bd.BOOK_CLASS_ID = bc.BOOK_CLASS_ID)
                        LEFT JOIN BOOK_CODE AS bcd
	                        ON (bd.BOOK_STATUS = bcd.CODE_ID
			                        AND bcd.CODE_TYPE = 'BOOK_STATUS')
                        LEFT JOIN MEMBER_M AS mm
	                        ON (bd.BOOK_KEEPER = mm.USER_ID)
                        WHERE (UPPER(bd.BOOK_NAME) LIKE UPPER('%' + @BookName + '%')
                        OR @BookName = '')
                        AND (bd.BOOK_CLASS_ID = @BookClassId
                        OR @BookClassId = '')
                        AND (bd.BOOK_KEEPER = @BookKeeperId
                        OR @BookKeeperId = '')
                        AND (bd.BOOK_STATUS = @BookStatusId
                        OR @BookStatusId = '')
                        ORDER BY bd.BOOK_BOUGHT_DATE DESC";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@BookName", arg.BookName == null ? string.Empty : arg.BookName));
                cmd.Parameters.Add(new SqlParameter("@BookClassId", arg.BookClassId == null ? string.Empty : arg.BookClassId));
                cmd.Parameters.Add(new SqlParameter("@BookKeeperId", arg.BookKeeperId == null ? string.Empty : arg.BookKeeperId));
                cmd.Parameters.Add(new SqlParameter("@BookStatusId", arg.BookStatusId == null ? string.Empty : arg.BookStatusId));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapBookDataToList(dt);
        }
        public List<Models.Book> GetAllBooks()
        {

            DataTable dt = new DataTable();
            string sql = @"SELECT * FROM BOOK_DATA bd";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapAutoCompleteBookData(dt);
        }
        //改成直接回傳Book
        public List<Models.Book> GetBookById(String id)
        {

            DataTable dt = new DataTable();
            string sql = @"SELECT
	                        bd.BOOK_ID
                           ,bd.BOOK_NAME
                           ,bd.BOOK_CLASS_ID
                           ,bc.BOOK_CLASS_NAME
                           ,bd.BOOK_AUTHOR
                           ,bd.BOOK_BOUGHT_DATE
                           ,bd.BOOK_PUBLISHER
                           ,bd.BOOK_NOTE
                           ,bd.BOOK_STATUS
                           ,bcd.CODE_NAME
                           ,bd.BOOK_KEEPER
                           ,mm.USER_ENAME
                           ,mm.USER_ENAME+'-'+mm.USER_CNAME AS USER_FULLNAME
                        FROM BOOK_DATA AS bd
                        LEFT JOIN BOOK_CLASS AS bc
	                        ON (bd.BOOK_CLASS_ID = bc.BOOK_CLASS_ID)
                        LEFT JOIN BOOK_CODE AS bcd
	                        ON (bd.BOOK_STATUS = bcd.CODE_ID
			                        AND bcd.CODE_TYPE = 'BOOK_STATUS')
                        LEFT JOIN MEMBER_M AS mm
	                        ON (bd.BOOK_KEEPER = mm.USER_ID)
                        WHERE bd.BOOK_ID = @BookId";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@BookId", string.IsNullOrEmpty(id) ? string.Empty : id));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                try
                {
                    sqlAdapter.Fill(dt);
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                }
                
                conn.Close();
            }
            return this.MapBookDataToList(dt);
        }
        public Boolean UpdateBook(Models.Book book)
        {
            string sql = @"UPDATE BOOK_DATA
			                    SET BOOK_NAME = @book_name
			                       ,BOOK_CLASS_ID = @book_class_id
			                       ,BOOK_AUTHOR = @book_author
			                       ,BOOK_BOUGHT_DATE = @book_bought_date
			                       ,BOOK_PUBLISHER = @publisher
			                       ,BOOK_NOTE = @book_note
			                       ,BOOK_STATUS = @book_status
			                       ,BOOK_KEEPER = @book_keeper_id
			                       ,MODIFY_DATE = GETDATE()
			                       ,MODIFY_USER = @modify_user
			                    WHERE BOOK_ID = @book_id";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@book_name", book.BookName));
                cmd.Parameters.Add(new SqlParameter("@book_class_id", book.BookClassId));
                cmd.Parameters.Add(new SqlParameter("@book_author", book.Author));
                cmd.Parameters.Add(new SqlParameter("@book_bought_date", book.BookBoughtDate));
                cmd.Parameters.Add(new SqlParameter("@publisher", book.BookPublisher));
                cmd.Parameters.Add(new SqlParameter("@book_note", HttpUtility.HtmlEncode(book.BookNote)));
                cmd.Parameters.Add(new SqlParameter("@book_status", book.BookStatusId));

                cmd.Parameters.Add(new SqlParameter("@book_keeper_id", book.BookKeeperId == null ? (Object)DBNull.Value : book.BookKeeperId));
                cmd.Parameters.Add(new SqlParameter("@modify_user", book.BookKeeperId == null ? (Object)DBNull.Value : book.BookKeeperId));
                cmd.Parameters.Add(new SqlParameter("@book_id", book.BookId));
                var result = cmd.ExecuteNonQuery();
                if (book.BookStatusId == "B" && book.BookKeeperId != null)
                {
                    InsertLendRecord(book);
                }
                conn.Close();
                if (result != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        public void InsertLendRecord(Models.Book book)
        {
            string sql = @"
                    INSERT INTO BOOK_LEND_RECORD (BOOK_ID, KEEPER_ID, LEND_DATE, CRE_DATE, CRE_USR, MOD_DATE, MOD_USR)
			                    SELECT
				                    @book_id AS BOOK_ID
			                       ,@book_keeper_id AS KEEPER_ID
			                       ,GETDATE() AS LEND_DATE
			                       ,GETDATE() AS CRE_DATE
			                       ,@book_keeper_id AS CRE_USR
			                       ,GETDATE() AS MOD_DATE
			                       ,@book_keeper_id AS MOD_USR";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@book_id", book.BookId));
                cmd.Parameters.Add(new SqlParameter("@book_keeper_id", book.BookKeeperId));
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public Boolean InsertBook(Models.Book book)
        {
            string sql = @"INSERT INTO BOOK_DATA (BOOK_NAME, BOOK_CLASS_ID, BOOK_AUTHOR, BOOK_BOUGHT_DATE, BOOK_PUBLISHER, BOOK_NOTE, BOOK_STATUS, BOOK_KEEPER, BOOK_AMOUNT, CREATE_DATE, CREATE_USER, MODIFY_DATE, MODIFY_USER)
	                            SELECT
		                            @book_name AS BOOK_NAME
	                               ,@book_class_id AS BOOK_CLASS_ID
	                               ,@book_author AS BOOK_AUTHOR
	                               ,@book_bought_date AS BOOK_BOUGHT_DATE
	                               ,@publisher AS BOOK_PUBLISHER
	                               ,@book_note AS BOOK_NOTE
	                               ,'A' AS BOOK_STATUS
	                               ,'' AS BOOK_KEEPER
	                               ,1 AS BOOK_AMOUNT
	                               ,GETDATE() AS CREATE_DATE
	                               ,'0001' AS CREATE_USER
	                               ,GETDATE() AS MODIFY_DATE
	                               ,'0001' AS MODIFY_USER";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@book_name", book.BookName));
                cmd.Parameters.Add(new SqlParameter("@book_class_id", book.BookClassId));
                cmd.Parameters.Add(new SqlParameter("@book_author", book.Author));
                cmd.Parameters.Add(new SqlParameter("@book_bought_date", book.BookBoughtDate));
                cmd.Parameters.Add(new SqlParameter("@publisher", book.BookPublisher));
                cmd.Parameters.Add(new SqlParameter("@book_note", HttpUtility.HtmlEncode(book.BookNote)));

                var result = cmd.ExecuteNonQuery();
                conn.Close();
                if (result != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        public Boolean DeleteBookById(string bookId)
        {
            try
            {
                string sql = "Delete FROM BOOK_DATA Where BOOK_ID=@bookId";
                using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add(new SqlParameter("@bookId", bookId));
                    var result = cmd.ExecuteNonQuery();
                    conn.Close();
                    if (result > 0 )
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
                return false;
            }
        }
        public Boolean DeleteLendRecordByBookId(string bookId)
        {
            try
            {
                string sql = "Delete FROM BOOK_LEND_RECORD Where BOOK_ID=@bookId";
                using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add(new SqlParameter("@bookId", bookId));
                    var result = cmd.ExecuteNonQuery();
                    conn.Close();
                    if (result != 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Boolean BookIsOccupied(string bookId)
        {
            string sql = @"SELECT * FROM BOOK_DATA bd  WHERE bd.BOOK_ID = @book_Id AND (bd.BOOK_STATUS = 'Ｂ' OR bd.BOOK_STATUS='C')";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@book_Id", bookId));

                var result = cmd.ExecuteScalar();

                if (result != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                conn.Close();
            }
        }
        public List<Models.BookLendRecord> GetLendRecordByBookId(String id)
        {

            DataTable dt = new DataTable();
            string sql = @"SELECT
	                        blr.LEND_DATE
                           ,blr.KEEPER_ID
                           ,mm.USER_ENAME
                           ,mm.USER_CNAME
                        FROM BOOK_LEND_RECORD blr
                        INNER JOIN MEMBER_M mm
	                        ON blr.KEEPER_ID = mm.USER_ID
                        WHERE blr.BOOK_ID = @BookId";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@BookId", string.IsNullOrEmpty(id) ? string.Empty : id));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapLendRecordDataToList(dt);
        }

        private List<Models.Book> MapBookDataToList(DataTable booksData)
        {
            List<Models.Book> result = new List<Book>();
            foreach (DataRow row in booksData.Rows)
            {
                result.Add(new Book()
                {
                    BookId = (int)row["BOOK_ID"],
                    BookName = row["BOOK_NAME"].ToString(),
                    BookClassId = row["BOOK_CLASS_ID"].ToString(),
                    BookClass = row["BOOK_CLASS_NAME"].ToString(),
                    Author = row["BOOK_AUTHOR"].ToString(),
                    BookBoughtDate = ((DateTime)row["BOOK_BOUGHT_DATE"]).ToString("yyyy/MM/dd"),
                    BookPublisher = row["BOOK_PUBLISHER"].ToString(),
                    BookNote = HttpUtility.HtmlDecode(row["BOOK_NOTE"].ToString()),
                    BookStatusId = (string)row["BOOK_STATUS"],
                    BookStatus = row["CODE_NAME"].ToString(),
                    BookKeeper = row["USER_ENAME"].ToString(),
                    BookKeeperFullName = row["USER_FULLNAME"].ToString(),
                    BookKeeperId = row["BOOK_KEEPER"].ToString(),
                });
            }
            return result;
        }
        private List<Models.Book> MapAutoCompleteBookData(DataTable booksData)
        {
            List<Models.Book> result = new List<Book>();
            foreach (DataRow row in booksData.Rows)
            {
                result.Add(new Book()
                {
                    BookId = (int)row["BOOK_ID"],
                    BookName = row["BOOK_NAME"].ToString(),
                    BookClassId = row["BOOK_CLASS_ID"].ToString(),
                    Author = row["BOOK_AUTHOR"].ToString(),
                    BookBoughtDate = ((DateTime)row["BOOK_BOUGHT_DATE"]).ToString("yyyy/MM/dd"),
                    BookPublisher = row["BOOK_PUBLISHER"].ToString(),
                    BookNote = HttpUtility.HtmlDecode(row["BOOK_NOTE"].ToString()),
                    BookStatusId = (string)row["BOOK_STATUS"],
                    BookKeeperId = row["BOOK_KEEPER"].ToString(),
                });
            }
            return result;
        }

        private List<Models.BookLendRecord> MapLendRecordDataToList(DataTable booksData)
        {
            List<Models.BookLendRecord> result = new List<BookLendRecord>();
            foreach (DataRow row in booksData.Rows)
            {
                result.Add(new BookLendRecord()
                {
                    BookLendDate = ((DateTime)row["LEND_DATE"]).ToString("yyyy/MM/dd"),
                    BookKeeperId = row["KEEPER_ID"].ToString(),
                    UserEName = row["USER_ENAME"].ToString(),
                    UserCName = row["USER_CNAME"].ToString(),
                });
            }
            return result;
        }
    }
}