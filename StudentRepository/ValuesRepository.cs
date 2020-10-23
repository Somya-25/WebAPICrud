using Microsoft.Extensions.Configuration;
using StudentDTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace StudentRepository
{
    public class ValuesRepository
    {

        private readonly string _connectionString;
        public ValuesRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("defaultConnection");
        }
        public async Task<List<Student>> GetAll()
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetAll", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var response = new List<Student>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToValue(reader));
                        }
                    }

                    return response;
                }
            }
        }

        private Student MapToValue(SqlDataReader reader)
        {
            return new Student()
            {
                id = (int)reader["id"],
                name = reader["name"].ToString(),
                city = reader["city"].ToString(),
                address = reader["address"].ToString(),
                phone = reader["phone"].ToString()

            };
        }

        

        public async Task<Student> GetById(int Id)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sdGetById", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id", Id));
                    Student response = null;
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response = MapToValue(reader);
                        }
                    }

                    return response;
                }
            }
        }


        public async Task Insert(Student value)
        {

            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("spAddStudent", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@name", value.name));
                    cmd.Parameters.Add(new SqlParameter("@address", value.address));
                    cmd.Parameters.Add(new SqlParameter("@city", value.city));
                    cmd.Parameters.Add(new SqlParameter("@phone", value.phone));

                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }

        }


        public async Task DeleteById(int Id)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("spDeleteStudent", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id", Id));
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        
        public  async Task UpdateDetails(int id ,Student smodel )
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("spUpdateStudent", sql))
                {
                    cmd.Parameters.AddWithValue("@id",id);
                    cmd.Parameters.AddWithValue("@name", smodel.name);
                    cmd.Parameters.AddWithValue("@address", smodel.address);
                    cmd.Parameters.AddWithValue("@city", smodel.city);
                    cmd.Parameters.AddWithValue("@phone", smodel.phone);
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return ;
                }


            }
        }
    }
}
