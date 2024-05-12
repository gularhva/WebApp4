using WebApp4.DTOs.School_DTOs;
using WebApp4.Models;

namespace WebApp4.Abstractions
{
    public interface ISchoolService
    {
        public Task<GenericResponseModel<List<SchoolGetDTO>>> GetAllSchools();
        public Task<GenericResponseModel<SchoolGetDTO>> GetById(int id);
        public Task<GenericResponseModel<SchoolCreateDTO>> AddSchool(SchoolCreateDTO model);
        public Task<GenericResponseModel<bool>> UpdateSchool(SchoolUpdateDTO model);
        public Task<GenericResponseModel<bool>> DeleteSchool(int id);
    }
}
