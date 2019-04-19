using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PneumaHRM.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<object> Get()
        {
            using (var ctx = new PrincipalContext(ContextType.Domain, "Pneumasoftware"))
            using (var userP = new UserPrincipal(ctx))
            using (var search = new PrincipalSearcher(userP))
            {
                var myDomainUsers = search
                    .FindAll()
                    .Select(x => new
                    {
                        x.UserPrincipalName,
                        x.Guid,
                        x.ContextType,
                        x.DisplayName,
                        x.Description,
                        x.DistinguishedName
                    })
                    .ToList();

                return new
                {
                    users = myDomainUsers,
                    me = User.Identity.Name
                };
            }

        }
    }
}
