using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StudentManagementSystem.Application;
using StudentManagementSystem.Domain.DTOs;
using StudentManagementSystem.Domain.Models;
using StudentManagementSystem.DataAccess.Repositories;
using StudentManagementSystem.Presentation.Commands;
using StudentManagementSystem.Infrastructure.Api;
using Moq.Protected;
using System.Net;
using StudentManagementSystem.Domain.Validators;

namespace LR3Tests
{
    [TestClass]
    public class StudentTests
    {
        [TestMethod]
        [Timeout(5000)]
        public async Task AddStudent_ValidData_ReturnsStudentWithIdAndQuote()
        {
            // Arrange
            var mockRepo = new Mock<IStudentRepository>();
            mockRepo.Setup(repo => repo.GetNextId()).Returns(1);
            mockRepo.Setup(repo => repo.Add(It.IsAny<Student>()))
                .Returns((Student s) => s);

            var mockApi = new Mock<IQuoteApiClient>();
            mockApi.Setup(api => api.GetRandomQuoteAsync())
                .ReturnsAsync(new QuoteDTO { Content = "Test Quote", Author = "Author" });

            var service = new StudentService(mockRepo.Object);
            var addCommand = new AddCommand(service, mockApi.Object);

            var consoleInput = new StringReader("TestStudent\n85"); 
            Console.SetIn(consoleInput);

            // Act
            await addCommand.ExecuteAsync();

            // Восстанавливаем стандартный ввод
            Console.SetIn(new StreamReader(Console.OpenStandardInput()));

            // Assert
            mockRepo.Verify(repo => repo.Add(It.Is<Student>(s =>
                s.Name == "TestStudent" && s.Grade == 85)), Times.Once);

            mockApi.Verify(api => api.GetRandomQuoteAsync(), Times.Once);
        }

        [TestMethod]
        public async Task GetRandomQuoteAsync_ReturnsValidDTO()
        {
            // Arrange
            var mockHttp = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                Content = new StringContent("{ 'content': 'Be yourself; everyone else is already taken.', 'author': 'Oscar Wilde' }"),
                StatusCode = HttpStatusCode.OK
            };
            mockHttp.Protected()
                    .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(response);

            var client = new HttpClient(mockHttp.Object);
            var apiClient = new QuoteApiClient(client);

            // Act
            var quote = await apiClient.GetRandomQuoteAsync();

            // Assert
            Assert.AreEqual("Be yourself; everyone else is already taken.", quote.Content);
            Assert.AreEqual("Oscar Wilde", quote.Author);
        }

        [TestMethod]
        public void StudentValidator_InvalidGrade_ThrowsException()
        {
            // Arrange
            var invalidStudent = new StudentDTO { Name = "Test", Grade = -10 };

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => StudentValidator.Validate(invalidStudent));
        }

        [TestMethod]
        public void StudentValidator_EmptyName_ThrowsException()
        {
            // Arrange
            var invalidStudent = new StudentDTO { Name = "", Grade = 80 };

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => StudentValidator.Validate(invalidStudent));
        }
    }
}