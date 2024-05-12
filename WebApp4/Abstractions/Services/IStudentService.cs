using Microsoft.AspNetCore.Mvc;
using WebApp4.DTOs.Student_DTOs;
using WebApp4.Entities;
using WebApp4.Models;

namespace WebApp4.Abstractions
{
    public interface IStudentService
    {
        public Task<GenericResponseModel<List<StudentGetDTO>>> GetAllStudents();
        public Task<GenericResponseModel<StudentGetDTO>> GetById(int id);
        public Task<GenericResponseModel<List<StudentGetDTO>>> GetAllStudentsBySchoolId(int id);
        public Task<GenericResponseModel<StudentCreateDTO>> AddStudent(StudentCreateDTO model);
        public Task<GenericResponseModel<bool>> UpdateStudent(StudentUpdateDTO model);
        public Task<GenericResponseModel<bool>> DeleteStudent(int id);
        public Task<GenericResponseModel<bool>> ChangeSchool(ChangeSchoolDTO model);
        public Task<GenericResponseModel<bool>> ChangeSchool(int studentId,int newSchoolId);


    }
}
