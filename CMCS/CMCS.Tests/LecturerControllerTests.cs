using Xunit;
using Moq;
using CMCS.Controllers;
using CMCS.Models;
using CMCS.Models.ViewModels;
using CMCS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CMCS.Tests
{
    public class LecturerControllerTests
    {
        [Fact]
        public void TotalAmount_ShouldBeCorrect()
        {
           
            var claim = new Claim
            {
                HoursWorked = 50,
                HourlyRate = 100
            };

         
            var total = claim.HoursWorked * claim.HourlyRate;

       
            Assert.Equal(5000, total);
        }

        
        [Fact]
        public void Notes_ShouldBeSavedCorrectly()
        {
           
            var claim = new Claim
            {
                Notes = "Lecturer added detailed notes about extra sessions."
            };

            Assert.Equal("Lecturer added detailed notes about extra sessions.", claim.Notes);
        }

     
        [Fact]
        public async Task Submit_ShouldAddClaim_AndRedirectToTrack()
        {
            
            var mockService = new Mock<IClaimService>();

            var vm = new SubmitClaimVm
            {
                Month = new DateTime(2025, 10, 1),
                HoursWorked = 50,
                HourlyRate = 100,
                Notes = "Test claim with document",
                LecturerId = "L1001",
                LecturerName = "Hazel"
            };

            var fileMock = new Mock<IFormFile>();
            var content = "Fake PDF content";
            var fileName = "test.pdf";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);

            var files = new List<IFormFile> { fileMock.Object };

            
            mockService
     .Setup(s => s.SubmitClaimAsync(It.IsAny<Claim>(), It.IsAny<List<IFormFile>>(), It.IsAny<CancellationToken>()))
     .ReturnsAsync(new Claim());

            var controller = new LecturerController(mockService.Object);

            controller.ModelState.Clear();

           
            var result = await controller.Submit(vm);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Track", redirectResult.ActionName);

            mockService.Verify(
        s => s.SubmitClaimAsync(It.IsAny<Claim>(), It.IsAny<List<IFormFile>>(), It.IsAny<CancellationToken>()),
        Times.Once
    );
        }
    }
}

