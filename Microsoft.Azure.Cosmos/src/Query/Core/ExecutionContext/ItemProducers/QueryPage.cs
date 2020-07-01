﻿// ------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
// ------------------------------------------------------------

namespace Microsoft.Azure.Cosmos.Query.Core.ExecutionContext.ItemProducers
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Azure.Cosmos.CosmosElements;
    using Microsoft.Azure.Cosmos.Pagination;
    using Microsoft.Azure.Cosmos.Query.Core.QueryClient;

    internal sealed class QueryPage : Page<QueryState>
    {
        public QueryPage(
            IReadOnlyList<CosmosElement> documents,
            double requestCharge,
            string activityId,
            long responseLengthInBytes,
            CosmosQueryExecutionInfo cosmosQueryExecutionInfo,
            QueryState state)
            : base(state)
        {
            this.Documents = documents ?? throw new ArgumentNullException(nameof(documents));
            this.RequestCharge = requestCharge < 0 ? throw new ArgumentOutOfRangeException(nameof(requestCharge)) : requestCharge;
            this.ActivityId = activityId;
            this.ResponseLengthInBytes = responseLengthInBytes < 0 ? throw new ArgumentOutOfRangeException(nameof(responseLengthInBytes)) : responseLengthInBytes;
            this.CosmosQueryExecutionInfo = cosmosQueryExecutionInfo;
        }

        public IReadOnlyList<CosmosElement> Documents { get; }

        public double RequestCharge { get; }

        public string ActivityId { get; }

        public long ResponseLengthInBytes { get; }

        public CosmosQueryExecutionInfo CosmosQueryExecutionInfo { get; }
    }
}