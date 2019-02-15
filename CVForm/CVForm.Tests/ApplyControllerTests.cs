using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CVForm.Controllers;
using CVForm.EntityFramework;
using CVForm.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace CVForm.Tests
{
    [TestFixture]
    public class ApplyControllerTests
    {
        [Test]
        public void Details_WhenUserIsNotAnAdminOrOwnerOfJobApplication_ReturnUnauthorizedStatus()
        {
            

            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "Details_WhenUserIsNotAnAdmin_ReturnUnauthorizedStatus")
                .Options;


            using (var context = new DataContext(options))
            {
                context.JobApplications.Add(new JobApplication()
                {
                   Id = 1,
                   UserId = "1"
                });

                context.SaveChanges();


                var aservice = new Mock<IAuthorizationService>();
                aservice.Setup(m =>  m.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>()))
                    .ReturnsAsync(AuthorizationResult.Failed);
                var sut= new ApplyController(context,null,aservice.Object);

                var cp = new Mock<ClaimsPrincipal>();
                cp.Setup(m => m.FindFirst(It.IsAny<string>())).Returns(new Claim("id","2"));
                sut.ControllerContext.HttpContext.User = cp.Object;
                // Act
                var result = sut.Details(1, 1);


                //Assert 
                Assert.That(result,Is.TypeOf<UnauthorizedResult>());
            }
        }

        [Test]
        public void Details_WhenUserIsOwnerOfJobApplication_ReturnUnauthorizedStatus()
        {


            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "Details_WhenUserIsNotAnAdmin_ReturnUnauthorizedStatus")
                .Options;


            using (var context = new DataContext(options))
            {
                context.JobApplications.Add(new JobApplication()
                {
                    Id = 1,
                    UserId = "1"
                });

                context.SaveChanges();


                var sut = new ApplyController(context, null, null);

                var cp = new Mock<ClaimsPrincipal>();
                cp.Setup(m => m.FindFirst(It.IsAny<string>())).Returns(new Claim("id", "1"));
                sut.ControllerContext.HttpContext = new DefaultHttpContext();
                sut.ControllerContext.HttpContext.User = cp.Object;
                // Act
                var result = sut.Details(1, 1);


                //Assert 
                Assert.That(result.Result, Is.TypeOf<ViewResult>());
            }
        }

    }
}
