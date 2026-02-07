using AutoMapper;
using KeepFit.Backend.Application.Contracts;
using KeepFit.Backend.Application.DTOs.Programs;
using KeepFit.Backend.Application.DTOs.Requests;
using KeepFit.Backend.Application.DTOs.Responses;
using KeepFit.Backend.Domain.Exceptions;
using KeepFit.Backend.Domain.Models.Training;
using KeepFit.Backend.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace KeepFit.Backend.Application.Services;

public class ProgramService(
    IGenericService<FitnessProgram> genericService,
    AppDbContext context,
    IMapper mapper
) : BaseService<FitnessProgram, ProgramResponse, ProgramDto>(genericService, mapper), IProgramService
{
    
}