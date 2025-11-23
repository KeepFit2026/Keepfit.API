using AutoMapper;
using KeepFit.Backend.Application.Contracts;
using KeepFit.Backend.Application.DTOs.Responses;
using KeepFit.Backend.Application.Services;
using KeepFit.Backend.Domain;
using KeepFit.Backend.Domain.Exceptions;
using KeepFit.Backend.Domain.Models.Exercise;
using Moq;

namespace KeepFit.Backend.MSTest.Exercises;

[TestClass]
public class ExerciseServiceTest
{
    private Mock<IGenericService<Exercise>> _mockGenericService;
    private Mock<IMapper> _mockMapper;
    private ExerciseService _exerciseService;

    [TestInitialize]
    public void TestInitialize()
    {
        /*
        _mockGenericService = new Mock<IGenericService<Exercise>>();
        _mockMapper = new Mock<IMapper>();
        _exerciseService = new ExerciseService(
            _mockGenericService.Object,
            _mockMapper.Object);
        */
    }

    /// <summary>
    /// Test de la méthode GetAllAsync avec une liste non vide.
    /// </summary>
    [TestMethod]
    public async Task GetAllAsyncMethodOk()
    {
        // Arrange
        var exercises = new List<Exercise>
        {
           new Exercise {Id = Guid.NewGuid(), Name = "Pompes", Description ="Faire des Pompes"},
           new Exercise {Id = Guid.NewGuid(), Name = "Squats", Description ="Faire des Squats"},
        };
        var exercisesResponse = new List<ExerciseResponse>
        {
            new ExerciseResponse {Id = exercises[0].Id, Name = "Pompes", Description = "Faire des Pompes"},
            new ExerciseResponse {Id = exercises[1].Id, Name = "Squats", Description = "Faire des Squats"},
        };
        
        _mockGenericService
            .Setup(m => m.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(exercises);
        
        _mockMapper
            .Setup(x => x.Map<List<ExerciseResponse>>(exercises))
            .Returns(exercisesResponse);

        // Act
        var result = await _exerciseService.GetAllAsync();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(exercisesResponse.Count, result.Count);
        for (var i = 0; i < exercisesResponse.Count; i++)
        {
            Assert.AreEqual(exercisesResponse[i].Id, result[i].Id);
            Assert.AreEqual(exercisesResponse[i].Name, result[i].Name);
            Assert.AreEqual(exercisesResponse[i].Description, result[i].Description);
        }
    }

    /// <summary>
    /// Test de la méthode GetAllAsync avec une liste vide.
    /// </summary>
    [TestMethod]
    public async Task GetAllAsyncMethodKo()
    {
        var emptyExerciseList = new List<Exercise>();
        _mockGenericService
            .Setup(m => m.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(emptyExerciseList);

        await Assert.ThrowsExceptionAsync<NotFoundException>(
            () => _exerciseService.GetAllAsync());
    }

    /// <summary>
    /// Test de la méthode GetAsync pour un exercice qui existe.
    /// </summary>
    [TestMethod]
    public async Task GetAsyncMethodNotOk()
    {
        // Arrange
        var exercise = new Exercise {Id = Guid.NewGuid(), Name = "Pompes", Description = "Faire des Pompes"};
        var exerciseResponse = new ExerciseResponse { Id = exercise.Id, Name = "Pompes", Description = "Faire des Pompes" };
        
        _mockGenericService
            .Setup(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(exercise);
        
        _mockMapper
            .Setup(m => m.Map<ExerciseResponse>(exercise))
            .Returns(exerciseResponse);

        // Act
        var result = await _exerciseService.GetAsync(exercise.Id);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(exercise.Id, result.Id);
        Assert.AreEqual(exercise.Name, result.Name);
        Assert.AreEqual(exercise.Description, result.Description);
    }

    /// <summary>
    /// Test de la méthode GetAsync pour un exercice qui n'existe pas.
    /// </summary>
    [TestMethod]
    public async Task GetAsyncMethodKo()
    {
        _mockGenericService
            .Setup(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Exercise)null!);
        
        await Assert.ThrowsExceptionAsync<NotFoundException>(
            () => _exerciseService.GetAsync(Guid.NewGuid()));
    }

    /// <summary>
    /// Test de la méthode DeleteExerciseAsync pour un exercice qui existe
    /// </summary>
    /// <returns></returns>
    [TestMethod]
    public async Task DeleteExerciseAsyncMethodOk()
    {
        var exercise = new Exercise { Id = Guid.NewGuid(), Name = "Pompes", Description = "Faire des Pompes" };
        
        _mockGenericService
            .Setup(m => m.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        
        //Act
        var result = await _exerciseService.DeleteExerciseAsync(exercise.Id);

        Assert.IsTrue(result);
        _mockGenericService.Verify(
            m => m.DeleteAsync(exercise.Id, It.IsAny<CancellationToken>()),
            Times.Once);
    }
    
    /// <summary>
    /// Test de la méthode DeleteExerciseAsync pour un exercice qui n'existe pas
    /// </summary>
    [TestMethod]
    public async Task DeleteExerciseAsyncMethodKo()
    {
        var exerciseId = Guid.NewGuid();
        _mockGenericService
            .Setup(m => m.DeleteAsync(exerciseId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        await Assert.ThrowsExceptionAsync<NotFoundException>(
            () => _exerciseService.DeleteExerciseAsync(exerciseId));
        
        _mockGenericService.Verify(
            m => m.DeleteAsync(exerciseId, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
