using DynamicsAdapter.Web.MatchFound.Models;
using Microsoft.Extensions.Logging;
using Simple.OData.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DynamicsAdapter.Web.MatchFound
{
    public interface IMatchFoundResponseService
    {
        Task<SSG_Identifier> UploadIdentifier(SSG_Identifier identifier, CancellationToken cancellationToken);
    }

    public class MatchFoundResponseService : IMatchFoundResponseService
    {
        private readonly IODataClient _oDataClient;
        private readonly ILogger<MatchFoundResponseService> _logger;
        public MatchFoundResponseService(IODataClient oDataClient, ILogger<MatchFoundResponseService> logger)
        {
            this._oDataClient = oDataClient;
            this._logger = logger;
        }

        public async Task<SSG_Identifier> UploadIdentifier(SSG_Identifier identifier, CancellationToken cancellationToken)
        {
            try
            {
                return await this._oDataClient.For<SSG_Identifier>().Set(identifier).InsertEntryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

    }
}
