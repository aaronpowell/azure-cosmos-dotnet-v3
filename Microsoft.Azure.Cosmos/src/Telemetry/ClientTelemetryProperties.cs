﻿//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

namespace Microsoft.Azure.Cosmos.Telemetry
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    [Serializable]
    internal sealed class ClientTelemetryProperties
    {
        [JsonProperty(PropertyName = "timeStamp")]
        internal string DateTimeUtc { get; set; }

        [JsonProperty(PropertyName = "clientId")]
        internal string ClientId { get; }

        [JsonProperty(PropertyName = "processId")]
        internal string ProcessId { get; }

        [JsonProperty(PropertyName = "userAgent")]
        internal string UserAgent { get; }

        [JsonProperty(PropertyName = "connectionMode")]
        internal string ConnectionMode { get; }

        [JsonProperty(PropertyName = "globalDatabaseAccountName")]
        internal string GlobalDatabaseAccountName { get; set; }

        [JsonProperty(PropertyName = "applicationRegion")]
        internal string ApplicationRegion { get; set; }

        [JsonProperty(PropertyName = "hostEnvInfo")]
        internal string HostEnvInfo { get; set; }

        [JsonProperty(PropertyName = "acceleratedNetworking")]
        internal bool? AcceleratedNetworking { get; set; }

        /// <summary>
        /// Preferred Region set by the client
        /// </summary>
        [JsonProperty(PropertyName = "preferredRegions")]
        internal IReadOnlyList<string> PreferredRegions { get; set; }

        [JsonProperty(PropertyName = "aggregationIntervalInSec")]
        internal int AggregationIntervalInSec { get; set; }

        [JsonProperty(PropertyName = "systemInfo")]
        internal List<SystemInfo> SystemInfo { get; set; }

        [JsonProperty(PropertyName = "cacheRefreshInfo")]
        private List<OperationInfo> CacheRefreshInfo { get; set; }

        [JsonProperty(PropertyName = "operationInfo")]
        internal List<OperationInfo> OperationInfo { get; set; }

        [JsonProperty(PropertyName = "miscellaneous")]
        internal string Miscellaneous { get; set; }

        [JsonIgnore]
        private readonly ConnectionMode ConnectionModeEnum;

        internal ClientTelemetryProperties(string clientId,
                                   string processId,
                                   string userAgent,
                                   ConnectionMode connectionMode,
                                   IReadOnlyList<string> preferredRegions,
                                   int aggregationIntervalInSec)
        {
            this.ClientId = clientId;
            this.ProcessId = processId;
            this.UserAgent = userAgent;
            this.ConnectionModeEnum = connectionMode;
            this.ConnectionMode = ClientTelemetryProperties.GetConnectionModeString(connectionMode);
            this.SystemInfo = new List<SystemInfo>();
            this.PreferredRegions = preferredRegions;
            this.AggregationIntervalInSec = aggregationIntervalInSec;
        }

        /// <summary>
        /// Needed by Serializer to deserialize the json
        /// </summary>
        public ClientTelemetryProperties(string dateTimeUtc,
            string clientId,
            string processId,
            string userAgent,
            string connectionMode,
            string globalDatabaseAccountName,
            string applicationRegion,
            string hostEnvInfo,
            bool? acceleratedNetworking,
            IReadOnlyList<string> preferredRegions,
            List<SystemInfo> systemInfo,
            List<OperationInfo> cacheRefreshInfo,
            List<OperationInfo> operationInfo)
        {
            this.DateTimeUtc = dateTimeUtc;
            this.ClientId = clientId;
            this.ProcessId = processId;
            this.UserAgent = userAgent;
            this.ConnectionMode = connectionMode;
            this.GlobalDatabaseAccountName = globalDatabaseAccountName;
            this.ApplicationRegion = applicationRegion;
            this.HostEnvInfo = hostEnvInfo;
            this.AcceleratedNetworking = acceleratedNetworking;
            this.SystemInfo = systemInfo;
            this.CacheRefreshInfo = cacheRefreshInfo;
            this.OperationInfo = operationInfo;
            this.PreferredRegions = preferredRegions;
        }

        private static string GetConnectionModeString(ConnectionMode connectionMode)
        {
            return connectionMode switch
            {
                Cosmos.ConnectionMode.Direct => "DIRECT",
                Cosmos.ConnectionMode.Gateway => "GATEWAY",
                _ => connectionMode.ToString().ToUpper(),
            };
        }
    }
}
