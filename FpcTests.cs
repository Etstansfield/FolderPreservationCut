using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using NUnit;
using NUnit.Framework;
using Moq;
using FolderPreservationCut.Interfaces;

namespace FolderPreservationCut
{
    [TestFixture]
    class FpcTests
    {
        private Fpc _fpc;
        private Mock<IFileProvider> _fileProviderMock;
        
        
        [SetUp]
        public void Setup()
        {
            _fileProviderMock = new Mock<IFileProvider>();
            _fileProviderMock.Setup(file => file.FindFolderLocation(It.Is<string>(s => s.Contains("C:\\StartLocation")))).Returns(false);
            _fileProviderMock.Setup(file => file.FindFolderLocation(It.Is<string>(s => s.Contains("C:\\StartLocationExists")))).Returns(true);
            _fileProviderMock.Setup(file => file.FindFolderLocation(It.Is<string>(s => s.Contains("C:\\EndLocation")))).Returns(false);
            _fileProviderMock.Setup(file => file.FindFolderLocation(It.Is<string>(s => s.Contains("C:\\EndLocationExists")))).Returns(true);

            _fpc = new Fpc(_fileProviderMock.Object);
            
        }
        
        [TestCase("", "test")]  // we don't care about the second param here
        [TestCase(null, "test")]
        public void FpcMissingStartLocationExceptionTest(string startLocation, string endLocation)
        {
            var ex = Assert.Throws<ArgumentException>(() => _fpc.Cut(startLocation, endLocation));
            Assert.That(ex.Message, Is.EqualTo("Start Location is Required!"));
        }

        [TestCase("test", null)]  // we don't care about the second param here
        [TestCase("test", "")]
        public void FpcMissingEndLocationExceptionTest(string startLocation, string endLocation)
        {
            var ex = Assert.Throws<ArgumentException>(() => _fpc.Cut(startLocation, endLocation));
            Assert.That(ex.Message, Is.EqualTo("End Location is Required!"));
        }

        [Test]
        public void FpcNonExistantStartLocationTest()
        {
            // first of all - we need to make a mock of IFileProvider
            // handled in the setup function

            var ex = Assert.Throws<FileNotFoundException>(() =>
            {
                _fpc.Cut("C:\\StartLocation", "C:\\EndLocationExists");
            });
            Assert.That(ex.Message, Is.EqualTo("Error accessing start location!"));
        }

        [Test]
        public void FpcNonExistantEndLocationTest()
        {
            var ex = Assert.Throws<FileNotFoundException>(() =>
            {
                _fpc.Cut("C:\\StartLocationExists", "C:\\EndLocation");
            });
            Assert.That(ex.Message, Is.EqualTo("Error accessing end location!"));
        }
    }
}
