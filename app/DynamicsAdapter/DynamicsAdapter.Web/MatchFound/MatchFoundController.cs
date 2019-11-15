using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DynamicsAdapter.Web.MatchFound.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DynamicsAdapter.Web.MatchFound
{

    [Route("[controller]")]
    [ApiController]
    public class MatchFoundController : ControllerBase
    {
        private readonly ILogger<MatchFoundController> _logger;
        private IMatchFoundResponseService _service;
        public MatchFoundController(ILogger<MatchFoundController> logger, IMatchFoundResponseService service)
        {
            _logger = logger;
            _service = service;
        }

        //POST: MatchFound/id
        [HttpPost("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> MatchFound(string id, [FromBody]Object payload)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            SSG_Identifier identifier = new SSG_Identifier();
            identifier.SSG_Identification = "identification number 2";
            identifier.StateCode = 0;
            identifier.StatusCode = 1;
            identifier.ssg_identificationeffectivedate =DateTime.Now;
            identifier.ssg_SearchAPIRequest = Guid.Parse("7ff9afcf-f1e9-e911-b811-00505683fbf4");
            //identifier.SSG_IdentificationCategoryText = "driver license";
            //identifier.SSG_InforamtionSourceText = "icbc";
           // identifier.ssg_SearchAPIRequest = "dc51d463-5106-ea11-b812-00505683fbf4";

            _logger.LogInformation("Received MatchFound response with SearchRequestId is " + id);

            var cts = new CancellationTokenSource();
            SSG_Identifier t = await _service.UploadIdentifier(identifier, cts.Token);  
            return Ok();
        }
    }
}