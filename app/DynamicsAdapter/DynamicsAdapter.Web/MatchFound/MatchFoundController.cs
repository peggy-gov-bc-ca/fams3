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
            identifier.SSG_Identification = "identification number";
            identifier.SSG_IdentificationCategoryText = "driver license";
            identifier.SSG_InforamtionSourceText = "icbc";
            identifier.SSG_SearchApiRequest = id;

            _logger.LogInformation("Received MatchFound response with SearchRequestId is " + id);

            var cts = new CancellationTokenSource();
            SSG_Identifier t = await _service.UploadIdentifier(identifier, cts.Token);  
            return Ok();
        }
    }
}