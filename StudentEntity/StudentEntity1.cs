//using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace StudentEntity
{
    public  class StudentEntity1 : IStudentEntity
    {
        
        private readonly string _connectionString;
       
        public StudentEntity1(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("defaultConnection");
        }
        public async Task<List<StudentE>> GetAll()
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetAll", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var response = new List<StudentE>();
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

        private StudentE MapToValue(SqlDataReader reader)
        {
            return new StudentE()
            {
                id = (int)reader["id"],
                name = reader["name"].ToString(),
                city = reader["city"].ToString(),
                address = reader["address"].ToString(),
                phone = reader["phone"].ToString()

            };
        }



        //public async Task<StudentE> GetById(int Id)
        //{
        //    using (SqlConnection sql = new SqlConnection(_connectionString))
        //    {
        //        using (SqlCommand cmd = new SqlCommand("sdGetById", sql))
        //        {
        //            cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //            cmd.Parameters.Add(new SqlParameter("@id", Id));
        //            StudentE response = null;
        //            await sql.OpenAsync();

        //            using (var reader = await cmd.ExecuteReaderAsync())
        //            {
        //                while (await reader.ReadAsync())
        //                {
        //                    response = MapToValue(reader);
        //                }
        //            }

        //            return response;
        //        }
        //    }
        //}


        public async Task<bool> Insert(StudentE value)
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
                    return true;
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



        public async Task<bool> Update(int id, StudentE smodel)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("spUpdateStudent", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    cmd.Parameters.Add(new SqlParameter("@name", smodel.name));
                    cmd.Parameters.Add(new SqlParameter("@address", smodel.address));
                    cmd.Parameters.Add(new SqlParameter("@city", smodel.city));
                    cmd.Parameters.Add(new SqlParameter("@phone", smodel.phone));

                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return true ;
                }


            }
        }

    }

    
}
