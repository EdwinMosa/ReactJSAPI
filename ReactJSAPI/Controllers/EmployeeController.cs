using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ReactJSAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace ReactJSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select EmployeeId, EmployeeName, Department, 
                             convert(varchar(10), DateOfJoining,120) as DateOfJoining, PhotoFileName from dbo.Employee";

            DataTable table = new();
            string sqlDataSource = _configuration.GetConnectionString("DBConn");
            SqlDataReader sqlDataReader;

            using (SqlConnection sqlConnection = new(sqlDataSource))
            {
                sqlConnection.Open();
                using SqlCommand sqlCommand = new(query, sqlConnection);
                sqlDataReader = sqlCommand.ExecuteReader();
                table.Load(sqlDataReader);
                sqlDataReader.Close();
                sqlConnection.Close();
            }
            return new JsonResult(table);
        }


        [HttpPost]
        public JsonResult Post(Employee employee)
        {
            string query = @"Insert into dbo.Employee Values (@EmployeeName, @Department, @DateOfJoining, @PhotoFileName)";

            DataTable table = new();
            string sqlDataSource = _configuration.GetConnectionString("DBConn");
            SqlDataReader sqlDataReader;

            using (SqlConnection sqlConnection = new(sqlDataSource))
            {
                sqlConnection.Open();
                using SqlCommand sqlCommand = new(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName);
                sqlCommand.Parameters.AddWithValue("@Department", employee.Department);
                sqlCommand.Parameters.AddWithValue("@DateOfJoining", employee.DateOfJoining);
                sqlCommand.Parameters.AddWithValue("@PhotoFileName", employee.PhotoFileName);
                sqlDataReader = sqlCommand.ExecuteReader();
                table.Load(sqlDataReader);
                sqlDataReader.Close();
                sqlConnection.Close();
            }
            return new JsonResult("Added Successfully");
        }


        [HttpPut]
        public JsonResult Put(Employee employee)
        {
            string query = @"Update dbo.Employee Set EmployeeName = @EmployeeName, Department = @Department, DateOfJoining = @DateOfJoining, 
                             PhotoFileName = @PhotoFileName 
                             Where EmployeeId = @EmployeeId";

            DataTable table = new();
            string sqlDataSource = _configuration.GetConnectionString("DBConn");
            SqlDataReader sqlDataReader;

            using (SqlConnection sqlConnection = new(sqlDataSource))
            {
                sqlConnection.Open();
                using SqlCommand sqlCommand = new(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);
                sqlCommand.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName);
                sqlCommand.Parameters.AddWithValue("@Department", employee.Department);
                sqlCommand.Parameters.AddWithValue("@DateOfJoining", employee.DateOfJoining);
                sqlCommand.Parameters.AddWithValue("@PhotoFileName", employee.PhotoFileName);
                sqlDataReader = sqlCommand.ExecuteReader();
                table.Load(sqlDataReader);
                sqlDataReader.Close();
                sqlConnection.Close();
            }
            return new JsonResult("Updated Successfully");
        }


        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"Delete from dbo.Employee Where EmployeeId = @EmployeeId";

            DataTable table = new();
            string sqlDataSource = _configuration.GetConnectionString("DBConn");
            SqlDataReader sqlDataReader;

            using (SqlConnection sqlConnection = new(sqlDataSource))
            {
                sqlConnection.Open();
                using SqlCommand sqlCommand = new(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@EmployeeId", id);
                sqlDataReader = sqlCommand.ExecuteReader();
                table.Load(sqlDataReader);
                sqlDataReader.Close();
                sqlConnection.Close();
            }
            return new JsonResult("Deleted Successfully");
        }


        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + filename;

                using(var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
                return new JsonResult(filename);
            }
            catch (Exception)
            {
                return new JsonResult("anonymous.jpg");
            }
        }
    }
}
