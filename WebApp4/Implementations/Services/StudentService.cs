using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApp4.Abstractions;
using WebApp4.Contexts;
using WebApp4.DTOs.Student_DTOs;
using WebApp4.Entities;
using WebApp4.Models;

namespace WebApp4.Implementations.Services
{
    public class StudentService : IStudentService
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _appDbContext;
        public StudentService(IMapper mapper, AppDbContext context)
        {
            _appDbContext = context;
            _mapper = mapper;
        }
        public async Task<GenericResponseModel<StudentCreateDTO>> AddStudent(StudentCreateDTO model)
        {
            GenericResponseModel<StudentCreateDTO> responseModel = new GenericResponseModel<StudentCreateDTO>()
            {
                Data = null,
                StatusCode = 404
            };
            try
            {
                if (model == null)
                {
                    return responseModel;
                }
                else
                {
                    Student data = new Student();
                    data.Name = model.Name;
                    data.School_Id = model.School_Id;
                    _appDbContext.Students.Add(data);
                    var affectedRows = await _appDbContext.SaveChangesAsync();
                    if (affectedRows > 0)
                    {
                        responseModel.Data = model;
                        responseModel.StatusCode = 200;
                    }
                    else
                    {
                        return responseModel;
                    }
                }
            }
            catch (Exception ex)
            {
                return responseModel;
            }

            return responseModel;
        }

        public async Task<GenericResponseModel<bool>> ChangeSchool(ChangeSchoolDTO model)
        {
            GenericResponseModel<bool> response = new GenericResponseModel<bool>
            {
                Data = false,
                StatusCode = 404
            };
            var school = await _appDbContext.Schools.FirstOrDefaultAsync(x => x.Id == model.NewSchoolId);
            if (school != null)
            {
                var data = await _appDbContext.Students.FirstOrDefaultAsync(x => x.Id == model.StudentId);
                if (data != null)
                {
                    data.School_Id = model.NewSchoolId;
                    _appDbContext.Update(data);
                    var affectedRows = await _appDbContext.SaveChangesAsync();
                    if (affectedRows > 0)
                    {
                        response.Data = true;
                        response.StatusCode = 200;
                    }
                    else
                    {
                        return response;
                    }
                }
                else
                {
                    return response;
                }
            }
            else
            {
                return response;
            }

            return response;
        }

        public async Task<GenericResponseModel<bool>> ChangeSchool(int studentId, int newSchoolId)
        {
            GenericResponseModel<bool> response = new GenericResponseModel<bool>
            {
                Data = false,
                StatusCode = 404
            };
            if (studentId < 0 || newSchoolId < 0)
            {
                return response;
            }
            var school = await _appDbContext.Schools.FirstOrDefaultAsync(x => x.Id == newSchoolId);
            if (school != null)
            {
                var data = _appDbContext.Students.Include(x => x.School).Where(x => x.Id == studentId).FirstOrDefault();
                if (data != null)
                {
                    data.School_Id = newSchoolId;
                    _appDbContext.Update(data);
                    var affectedRows = await _appDbContext.SaveChangesAsync();
                    if (affectedRows > 0)
                    {
                        response.Data = true;
                        response.StatusCode = 200;
                    }
                    else
                    {
                        return response;
                    }
                }
                else
                {
                    return response;
                }
            }
            else
            {
                return response;
            }
            return response;

        }

        public async Task<GenericResponseModel<bool>> DeleteStudent(int id)
        {
            GenericResponseModel<bool> response = new GenericResponseModel<bool>()
            {
                Data = false,
                StatusCode = 404
            };
            var data = _appDbContext.Students.Where(predicate => predicate.Id == id).FirstOrDefault();
            if (data != null)
            {
                _appDbContext.Students.Remove(data);
                var affectedRows = await _appDbContext.SaveChangesAsync();
                if (affectedRows > 0)
                {
                    response.Data = true;
                    response.StatusCode = 200;
                }
                else
                {
                    return response;
                }
            }
            else
            {
                return response;
            }
            return response;
        }

        public async Task<GenericResponseModel<List<StudentGetDTO>>> GetAllStudents()
        {
            GenericResponseModel<List<StudentGetDTO>> responseModel = new GenericResponseModel<List<StudentGetDTO>>()
            {
                Data = null,
                StatusCode = 500
            };
            List<Student> data = await _appDbContext.Students.Include(x => x.School).ToListAsync();
            if (data.Count > 0)
            {
                var student = _mapper.Map<List<StudentGetDTO>>(data);
                responseModel.Data = student;
                responseModel.StatusCode = 200;
            }
            else
            {
                responseModel.Data = null;
                responseModel.StatusCode = 404;
            }
            return responseModel;
        }

        public async Task<GenericResponseModel<List<StudentGetDTO>>> GetAllStudentsBySchoolId(int id)
        {
            GenericResponseModel<List<StudentGetDTO>> response = new GenericResponseModel<List<StudentGetDTO>>()
            {
                Data = null,
                StatusCode = 404
            };
            List<Student> data = await _appDbContext.Students.Include(x => x.School).Where(x => x.School_Id == id).ToListAsync();
            if (data.Count > 0)
            {
                var student = _mapper.Map<List<StudentGetDTO>>(data);
                response.Data = student;
                response.StatusCode = 200;
            }
            else
            {
                return response;
            }
            return response;
        }

        public async Task<GenericResponseModel<StudentGetDTO>> GetById(int id)
        {
            GenericResponseModel<StudentGetDTO> response = new GenericResponseModel<StudentGetDTO>()
            {
                Data = null,
                StatusCode = 500
            };
            Student data = await _appDbContext.Students.Include(x => x.School).Where(x => x.Id == id).FirstOrDefaultAsync();
            if (data != null)
            {
                var student = _mapper.Map<StudentGetDTO>(data);
                response.Data = student;
                response.StatusCode = 200;
            }
            else
            {
                response.Data = null;
                response.StatusCode = 404;
            }
            return response;
        }

        public async Task<GenericResponseModel<bool>> UpdateStudent(StudentUpdateDTO model)
        {
            GenericResponseModel<bool> response = new GenericResponseModel<bool>()
            {
                Data = false,
                StatusCode = 404
            };
            var data = _appDbContext.Students.Include(x => x.School).Where(x => x.Id == model.Id).FirstOrDefault();
            if (data != null)
            {
                data.Name = model.Name;
                _appDbContext.Students.Update(data);
                var affectedRows = await _appDbContext.SaveChangesAsync();
                if (affectedRows > 0)
                {
                    response.Data = true;
                    response.StatusCode = 200;
                }
                else
                {
                    return response;
                }
            }
            else
            {
                return response;
            }
            return response;
        }
    }
}
