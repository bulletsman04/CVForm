using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CommunityCertForT;
using CommunityCertForT.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace CVForm.Models
{
    public class InAdminsGroupRequirement : IAuthorizationRequirement
    {
       
        public InAdminsGroupRequirement()
        {
           
        }
    }
    public class InAdminsGroupHandler : AuthorizationHandler<InAdminsGroupRequirement>
        {
            private IConfiguration _configuration;
            public InAdminsGroupHandler(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                InAdminsGroupRequirement requirement)
            {
                AppSettings appSettings = _configuration.GetSection("AppSettings").Get<AppSettings>(); ;
                AADGraph graph = new AADGraph(appSettings);
                string groupName = "Admins";
                string groupId = appSettings.AADGroups.FirstOrDefault(g =>
                    String.Compare(g.Name, groupName) == 0).Id;
                Task<bool> isIngroup =  graph.IsUserInGroup(context.User.Claims, groupId);

                if (isIngroup.Result)
                {
                    context.Succeed(requirement);
                }

                
                return Task.CompletedTask;
            }
        }
    
}
