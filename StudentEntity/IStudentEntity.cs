using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StudentEntity
{
    public interface IStudentEntity
    {
        Task<List<StudentE>> GetAll();
        
        Task<bool> Insert(StudentE newuser);
        Task<bool> Update(int id, StudentE newuser);

        
        Task DeleteById(int Id);
        //Task GetById(int id);

    }
    }
    