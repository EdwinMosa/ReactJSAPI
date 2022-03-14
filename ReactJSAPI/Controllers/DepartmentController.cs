using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ReactJSAPI.Models;
using System.Data;
using System.Data.SqlClient;

namespace ReactJSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select DepartmentId, DepartmentName from dbo.Department";

            DataTable table = new ();
            string sqlDataSource = _configuration.GetConnectionString("DBConn");
            SqlDataReader sqlDataReader;

            using (SqlConnection sqlConnection = new (sqlDataSource))
            {
                sqlConnection.Open();
                using(SqlCommand sqlCommand = new (query, sqlConnection))
                {
                    sqlDataReader = sqlCommand.ExecuteReader();
                    table.Load(sqlDataReader);
                    sqlDataReader.Close();
                    sqlConnection.Close();
                }
            }
            return new JsonResult(table);
        }


        [HttpPost]
        public JsonResult Post(Department department)
        {
            string query = @"Insert into dbo.Department Values (@DepartmentName)";

            DataTable table = new();
            string sqlDataSource = _configuration.GetConnectionString("DBConn");
            SqlDataReader sqlDataReader;

            using (SqlConnection sqlConnection = new(sqlDataSource))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@DepartmentName", department.DepartmentName);
                    sqlDataReader = sqlCommand.ExecuteReader();
                    table.Load(sqlDataReader);
                    sqlDataReader.Close();
                    sqlConnection.Close();
                }
            }
            return new JsonResult("Added Successfully");
        }


        [HttpPut]
        public JsonResult Put(Department department)
        {
            string query = @"Update dbo.Department Set DepartmentName = @DepartmentName Where DepartmentId = @DepartmentId";

            DataTable table = new();
            string sqlDataSource = _configuration.GetConnectionString("DBConn");
            SqlDataReader sqlDataReader;

            using (SqlConnection sqlConnection = new(sqlDataSource))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@DepartmentId", department.DepartmentId);
                    sqlCommand.Parameters.AddWithValue("@DepartmentName", department.DepartmentName);
                    sqlDataReader = sqlCommand.ExecuteReader();
                    table.Load(sqlDataReader);
                    sqlDataReader.Close();
                    sqlConnection.Close();
                }
            }
            return new JsonResult("Updated Successfully");
        }


        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"Delete from dbo.Department Where DepartmentId = @DepartmentId";

            DataTable table = new();
            string sqlDataSource = _configuration.GetConnectionString("DBConn");
            SqlDataReader sqlDataReader;

            using (SqlConnection sqlConnection = new(sqlDataSource))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@DepartmentId", id);
                    sqlDataReader = sqlCommand.ExecuteReader();
                    table.Load(sqlDataReader);
                    sqlDataReader.Close();
                    sqlConnection.Close();
                }
            }
            return new JsonResult("Deleted Successfully");
        }
    }
}
