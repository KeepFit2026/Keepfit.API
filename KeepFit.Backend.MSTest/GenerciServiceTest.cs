using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using KeepFit.Backend.Application.Services;
using KeepFit.Backend.Infrastructure;

namespace KeepFit.Tests.Services;

public class TestEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}

[TestClass]
public class GenericServiceTests
{
    private Mock<AppDbContext> _mockContext;
    private Mock<DbSet<TestEntity>> _mockDbSet;
    private Mock<IMemoryCache> _mockCache;
    private Mock<ILogger<GenericService<TestEntity>>> _mockLogger;
    private GenericService<TestEntity> _service;

    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>().Options;
        _mockContext = new Mock<AppDbContext>(options);
        _mockDbSet = new Mock<DbSet<TestEntity>>();

        _mockContext.Setup(c => c.Set<TestEntity>()).Returns(_mockDbSet.Object);

        _mockCache = new Mock<IMemoryCache>();
        _mockLogger = new Mock<ILogger<GenericService<TestEntity>>>();

        _service = new GenericService<TestEntity>(
            _mockContext.Object, 
            _mockCache.Object, 
            _mockLogger.Object
        );
    }

    // Test pour savoir si je récupère bien le cache si déjà en cache
    [TestMethod]
    public async Task ExistsAsync_ShouldReturnFromCache_WhenKeyAlreadyExists()
    {
        var id = Guid.NewGuid();
        var cacheKey = $"{typeof(TestEntity).Name}_{id}";
        object cacheValue = true;
        
        _mockCache.Setup(m => m.TryGetValue(cacheKey, out cacheValue)).Returns(true);

        // ACT
        var result = await _service.ExistsAsync(id);

        // ASSERT
        Assert.IsTrue(result); // Le résultat vient du cache
        
        //Vérifie que la méthode FindAsync n'a pas été utilisé.
        _mockDbSet.Verify(m => m.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()), Times.Never); 
    }

    // Test pour vérifier s'il trouve bien l'item
    [TestMethod]
    public async Task ExistsAsync_ShouldReturnTrue_WhenEntityFoundInDb()
    {
        // ARRANGE
        var id = Guid.NewGuid();
        var cacheKey = $"{typeof(TestEntity).Name}_{id}";
        var entity = new TestEntity { Id = id };
        object outValue;

        // Cache vide
        _mockCache.Setup(m => m.TryGetValue(cacheKey, out outValue)).Returns(false);
        
        // Setup obligatoire pour le Set du cache 
        var mockCacheEntry = new Mock<ICacheEntry>();
        _mockCache.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(mockCacheEntry.Object);

        // La DB trouve l'item
        _mockDbSet.Setup(m => m.FindAsync(new object[] { id }, It.IsAny<CancellationToken>()))
                  .ReturnsAsync(entity);

        // ACT
        var result = await _service.ExistsAsync(id);

        // ASSERT
        Assert.IsTrue(result); // On vérifie juste qu'il a trouvé l'item (Resultat True)
    }

    // Test pour savoir si ça va bien en cache
    [TestMethod]
    public async Task ExistsAsync_ShouldSetCache_WhenEntityFoundInDb()
    {
        // ARRANGE
        var id = Guid.NewGuid();
        var cacheKey = $"{typeof(TestEntity).Name}_{id}";
        var entity = new TestEntity { Id = id };
        object outValue;

        // Cache vide
        _mockCache.Setup(m => m.TryGetValue(cacheKey, out outValue)).Returns(false);

        // On mock l'entrée du cache pour vérifier qu'elle est créée
        var mockCacheEntry = new Mock<ICacheEntry>();
        _mockCache.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(mockCacheEntry.Object);

        // trouve l'item
        _mockDbSet.Setup(m => m.FindAsync(new object[] { id }, It.IsAny<CancellationToken>()))
                  .ReturnsAsync(entity);

        // ACT
        await _service.ExistsAsync(id);

        // ASSERT
        // On vérifie spécifiquement que la méthode CreateEntry a été appelée avec la bonne clé
        _mockCache.Verify(m => m.CreateEntry(cacheKey), Times.Once);
    }
}