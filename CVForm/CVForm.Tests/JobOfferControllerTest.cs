using CVForm.Controllers;
using CVForm.EntityFramework;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using CVForm.Models;
using Microsoft.AspNetCore.Mvc;

namespace Tests
{
    public class JobOfferControllerTests
    {
        [Test]
        public  void Details_WhenThereIsNoJobOfferWithGivenId_ReturnNotFoundStatus()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "Details_WhenThereIsNoJobOfferWithGivenId_ReturnNotFoundStatus")
                .Options;

            using (var context = new DataContext(options))
            {
                var controller = new JobOfferController(context);

                // Act
                var result =  controller.Details(1);


                //Assert 
                Assert.IsInstanceOf<NotFoundResult>(result);
            }
        }

        [Test]
        public void Details_WhenThereIsJobOfferWithGivenId_ReturnsViewWithJobOffer()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "Details_HasJobOfferDetails_WhenThereIsJobOfferWithGivenId_ReturnsViewWithJobOffer")
                .Options;


            using (var context = new DataContext(options))
            {
                context.JobOfers.Add(new JobOffer()
                {
                    ID = 1,
                    CompanyId = 1
                });
                context.Companies.Add(new Company() {ID = 1});
                context.SaveChanges();
                var controller = new JobOfferController(context);

                // Act
                var result = controller.Details(1) as ViewResult;


                //Assert 
                Assert.IsInstanceOf<ViewResult>(result);
                Assert.IsNotNull(result.Model);
                Assert.IsInstanceOf<JobOffer>(result.Model);
            }
        }

        [Test]
        public void Create_WhenModelIsNull_ThrowNullReferenceException()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "Create_WhenModelIsNull_ThrowNullReferenceException")
                .Options;


            using (var context = new DataContext(options))
            {
                var controller = new JobOfferController(context);

                //Act and Assert 
                Assert.ThrowsAsync<NullReferenceException> ( () => controller.Create(null));
            }
        }
    }
}