using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApp4.Abstractions;
using WebApp4.Abstractions.IRepositories;
using WebApp4.Abstractions.IUnitOfWorks;
using WebApp4.Contexts;
using WebApp4.DTOs.School_DTOs;
using WebApp4.DTOs.Student_DTOs;
using WebApp4.Entities;
using WebApp4.Models;

namespace WebApp4.Implementations.Services;

public class SchoolService : ISchoolService
{
    private readonly IUnitOfWork _unitOfWork; //yuxarida olmalidi!!!! yoxsa repo gelmir
    private readonly IMapper _mapper;
    private IGenericRepository<School> _schoolRepo;
    public SchoolService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _schoolRepo = _unitOfWork.GetRepository<School>();
    }
    public async Task<GenericResponseModel<SchoolCreateDTO>> AddSchool(SchoolCreateDTO model)
    {
        GenericResponseModel<SchoolCreateDTO> responseModel = new GenericResponseModel<SchoolCreateDTO>()
        {
            Data = null,
            StatusCode = 404
        };
        if (model == null)
        {
            return responseModel;
        }

        School data = new();
        data.Number = model.Number;
        data.Name = model.Name;
        await _schoolRepo.Add(data);
        var affectedRows = await _unitOfWork.SaveAsync();
        if (affectedRows > 0)
        {
            responseModel.Data = model;
            responseModel.StatusCode = 200;
        }
        else
        {
            return responseModel;
        }
        return responseModel;
    }

    public async Task<GenericResponseModel<bool>> DeleteSchool(int id)
    {
        GenericResponseModel<bool> response = new GenericResponseModel<bool>()
        {
            Data = false,
            StatusCode = 404
        };
        var data = _schoolRepo.GetById(id);
        if (data == null)
        {
            return response;
        }
        _schoolRepo.DeleteById(id);
        var affectedRows = await _unitOfWork.SaveAsync();
        if (affectedRows <= 0)
        {
            return response;
        }
        response.Data = true;
        response.StatusCode = 200;
        return response;
    }

    public async Task<GenericResponseModel<List<SchoolGetDTO>>> GetAllSchools()
    {
        GenericResponseModel<List<SchoolGetDTO>> responseModel = new GenericResponseModel<List<SchoolGetDTO>>()
        {
            Data = null,
            StatusCode = 500
        };
        List<School> data = await _schoolRepo.GetAll().ToListAsync();
        if (data.Count > 0)
        {
            List<SchoolGetDTO> school = _mapper.Map<List<SchoolGetDTO>>(data);
            responseModel.Data = school;
            responseModel.StatusCode = 200;
        }
        else
        {
            responseModel.Data = null;
            responseModel.StatusCode = 404;
        }
        return responseModel;
    }

    public async Task<GenericResponseModel<SchoolGetDTO>> GetById(int id)
    {
        GenericResponseModel<SchoolGetDTO> response = new GenericResponseModel<SchoolGetDTO>()
        {
            Data = null,
            StatusCode = 404
        };
        School data = await _schoolRepo.GetById(id);
        if (data == null)
        {
            return response;
        }
        var school = _mapper.Map<SchoolGetDTO>(data);
        response.Data = school;
        response.StatusCode = 200;
        return response;
    }

    public async Task<GenericResponseModel<bool>> UpdateSchool(SchoolUpdateDTO model)
    {
        GenericResponseModel<bool> response = new GenericResponseModel<bool>()
        {
            Data = false,
            StatusCode = 404
        };
        var data = await _schoolRepo.GetById(model.Id);
        if (data == null)
        {
            return response;
        }
        data.Name = model.Name;
        _schoolRepo.Update(data);
        var affectedRows = await _unitOfWork.SaveAsync();
        if (affectedRows <= 0)
        {
            return response;
        }
        response.Data = true;
        response.StatusCode = 200;
        return response;
    }
}
