using AutoMapper;
using KeepFit.Backend.Application.Contracts;
using KeepFit.Backend.Application.DTOs.Classroom;
using KeepFit.Backend.Application.DTOs.Responses;
using KeepFit.Backend.Domain.Models;

namespace KeepFit.Backend.Application.Services;

public class ClassroomService(
    IGenericService<Classroom> genericService,
    IMapper mapper
    ) : BaseService<Classroom, ClassroomResponse, ClassroomDto>(genericService, mapper), IClassroomService
{
    
}