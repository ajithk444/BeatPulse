﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BeatPulse.DocumentDb
{
    public class DocumentDbLiveness : IHealthCheck
    {

        private readonly DocumentDbOptions _documentDbOptions = new DocumentDbOptions();

        public DocumentDbLiveness(DocumentDbOptions documentDbOptions)
        {
            _documentDbOptions.UriEndpoint = documentDbOptions.UriEndpoint ?? throw new ArgumentNullException(nameof(documentDbOptions.UriEndpoint));
            _documentDbOptions.PrimaryKey = documentDbOptions.PrimaryKey ?? throw new ArgumentNullException(nameof(documentDbOptions.PrimaryKey));

        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                using (var documentDbClient = new DocumentClient(
                    new Uri(_documentDbOptions.UriEndpoint), 
                    _documentDbOptions.PrimaryKey))
                {
                    await documentDbClient.OpenAsync();

                    return HealthCheckResult.Passed();
                }
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Failed(exception: ex);
            }
        }
    }
}
