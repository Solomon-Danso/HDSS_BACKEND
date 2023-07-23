using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HDSS_BACKEND.Data;
using Microsoft.AspNetCore.Mvc;

namespace HDSS_BACKEND.Controllers
{
    [ApiController]
    [Route("api/Director")]
    public class DirectorController : ControllerBase
    {
        private readonly DataContext context;
        public DirectorController(DataContext ctx){
            context = ctx;
        }

        

    }
}