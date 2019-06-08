using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheeseIT.BusinessLogic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CheeseIT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        [HttpPost]
        public HttpResponse PostFirebaseToken([FromBody] dynamic post)
        {
            TokenRepository.GetInstance().FirebaseToken = post.token.Value;

            Response.StatusCode = 200;
            return Response;
        }
    }
}