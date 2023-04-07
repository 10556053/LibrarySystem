using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace LibrarySystem.Models
{
    public class CodeService
    {
        private string GetDBConnectionString()
        {
            return
                System.Configuration.ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString.ToString();
        }
        public List<SelectListItem> GetBookTypeCodeTable()
        {
            DataTable dt = new DataTable();
            string sql = @"Select bc.BOOK_CLASS_NAME As CodeName, bc.BOOK_CLASS_ID As CodeID 
                           From BOOK_CLASS bc";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapCodeData(dt);
        }

        public List<SelectListItem> GetUserCodeTable()
        {
            DataTable dt = new DataTable();
            string sql = @"Select mm.USER_CNAME+'-'+mm.USER_ENAME As CodeName, mm.[USER_ID] As CodeID 
                           From MEMBER_M mm";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapCodeData(dt);
        }

        public List<SelectListItem> GetBookStatusCodeTable( String  codeType)
        {
            DataTable dt = new DataTable();
            string sql = @"Select bc.CODE_NAME As CodeName, bc.CODE_ID As CodeID 
                           From BOOK_CODE bc
						   WHERE bc.CODE_TYPE = @codeType";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@codeType", codeType));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapCodeData(dt);
        }

        private List<SelectListItem> MapCodeData(DataTable dt)
        {
            List<SelectListItem> result = new List<SelectListItem>();
            foreach (DataRow row in dt.Rows)
            {
                result.Add(new SelectListItem()
                {
                    Text = row["CodeName"].ToString(),
                    Value = row["CodeId"].ToString()
                });
            }
            return result;
        }
    }
}