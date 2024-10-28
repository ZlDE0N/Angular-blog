using BackendBlogServicesApi.Data;
using NSubstitute;
using Microsoft.EntityFrameworkCore;
using BackendBlogServicesApi.Entity;
using BackendBlogServicesApi.Repositories;
using Moq;


namespace BackendBlogServices.Test;

public class Tests
{

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task TestValidateExisitingUserById()
    {
        // Arrange
        int id = 7;
        var user = new User
        {
            Id = id,
            Username = "haki-D-rey",
            FirstName = "cesar cuadra",
            LastName = "irias",
            email = "you@gmail.com",
            Password = "haki1234" // Asegúrate de que Password es un campo requerido
        };
        var users = new List<User> { user };

        // Crea un contexto de base de datos en memoria
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "BlogDBV1") // Nombre de la base de datos en memoria
            .EnableSensitiveDataLogging() // Habilitar registro de datos sensibles
            .Options;

        // Usar el contexto en memoria
        using (var context = new AppDbContext(options))
        {
            context.Users.AddRange(users);
            context.SaveChanges(); // Guarda los cambios en la base de datos en memoria
        }

        // Crea una nueva instancia del contexto para el repositorio
        using (var context = new AppDbContext(options))
        {
            var subject = new UserRepository(context);

            // Act
            var result = await subject.GetUserById(id);

            // Assert
            Assert.NotNull(result); // Verifica que el resultado no es nulo

            // Verifica que todos los datos coinciden utilizando el modelo de restricciones
            Assert.That(result.Id, Is.EqualTo(user.Id)); // Verifica que el Id coincide
            Assert.That(result.Username, Is.EqualTo(user.Username)); // Verifica que el username coincide
            Assert.That(result.FirstName, Is.EqualTo(user.FirstName)); // Verifica que el primer nombre coincide
            Assert.That(result.LastName, Is.EqualTo(user.LastName)); // Verifica que el apellido coincide
            Assert.That(result.email, Is.EqualTo(user.email)); // Verifica que el email coincide
            Assert.That(result.Password, Is.EqualTo(user.Password)); // Verifica que la contraseña coincide
        }
    }




    private DbSet<T> FakeDbSet<T>(List<T> data) where T : class
    {
        var queryableData = data.AsQueryable();
        var mockSet = new Mock<DbSet<T>>();

        mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryableData.Provider);
        mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryableData.Expression);
        mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
        mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryableData.GetEnumerator());

        return mockSet.Object;
    }
}