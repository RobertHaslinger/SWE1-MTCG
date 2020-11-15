using System;
using System.IO;
using Moq;
using NUnit.Framework;
using SWE1_MTCG.Controller;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Server;
using SWE1_MTCG.Services;

namespace SWE1_MTCG.Test
{
    [TestFixture]
    public class MessageControllerTest
    {
        private Mock<IFileService> _fileServiceMock;
        private MessageController _messageController;

        [SetUp]
        public void SetUp()
        {
            _fileServiceMock= new Mock<IFileService>(MockBehavior.Strict);
            _messageController= new MessageController(_fileServiceMock.Object);
        }

        [Test]
        public void Test_ControllerReturnsNotFoundResponseIfNoMessageWasFound()
        {
            _fileServiceMock.Setup(s => s.GetFileContent(5)).Throws(new FileNotFoundException());

            StatusCode response = _messageController.ReadExistingFile(5).Key;

            Assert.AreEqual(response, StatusCode.NotFound);
        }

        [Test]
        public void Test_ControllerReturnsUnauthorizedResponseIfMessageCouldNotBeAccessed()
        {
            _fileServiceMock.Setup(s => s.DeleteFile(5)).Throws(new UnauthorizedAccessException());

            StatusCode response = _messageController.DeleteFile(5).Key;

            Assert.AreEqual(response, StatusCode.Unauthorized);
        }

        [Test]
        public void Test_ControllerReturnsBadRequestResponseIfMessageIdIsBad()
        {
            _fileServiceMock.Setup(s => s.GetFileContent(5)).Throws(new ArgumentException());

            StatusCode response = _messageController.ReadExistingFile(5).Key;

            Assert.AreEqual(response, StatusCode.BadRequest);
        }

        [Test]
        public void Test_ControllerReturnsOkResponseIfMessageWasFound()
        {
            _fileServiceMock.Setup(s => s.GetFileContent(5)).Returns("Test content");

            StatusCode response = _messageController.ReadExistingFile(5).Key;

            Assert.AreEqual(response, StatusCode.OK);
        }
    }
}