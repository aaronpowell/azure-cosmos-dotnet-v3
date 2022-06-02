//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

namespace Microsoft.Azure.Cosmos
{
    using System;
    using System.Collections;
    using System.Threading;
    using Microsoft.Azure.Cosmos.Diagnostics;
    using Microsoft.Azure.Cosmos.Tracing;

    /// <summary>
    /// The exception that is thrown in a thread upon cancellation of an operation that
    ///  the thread was executing. This extends the OperationCanceledException to include the
    ///  diagnostics of the operation that was canceled.
    /// </summary>
    public class CosmosOperationCanceledException : OperationCanceledException
    {
        private readonly OperationCanceledException originalException;
        private readonly Lazy<string> lazyMessage;
        private readonly Lazy<string> toStringMessage;

        /// <summary>
        /// Create an instance of CosmosOperationCanceledException
        /// </summary>
        /// <param name="originalException">The original operation canceled exception</param>
        /// <param name="diagnostics"></param>
        public CosmosOperationCanceledException(
            OperationCanceledException originalException,
            CosmosDiagnostics diagnostics)
            : base(originalException.CancellationToken)
        {
            this.originalException = originalException ?? throw new ArgumentNullException(nameof(originalException));
            this.Diagnostics = diagnostics ?? throw new ArgumentNullException(nameof(diagnostics));
            this.lazyMessage = this.CreateLazyMessage(originalException.CancellationToken);
            this.toStringMessage = this.CreateToStringMessage(originalException.CancellationToken);
        }

        internal CosmosOperationCanceledException(
            OperationCanceledException originalException,
            ITrace trace)
            : base(originalException.CancellationToken)
        {
            this.originalException = originalException ?? throw new ArgumentNullException(nameof(originalException));
            if (trace == null)
            {
                throw new ArgumentNullException(nameof(trace));
            }

            trace.AddDatum("Operation Cancelled Exception", originalException);
            this.Diagnostics = new CosmosTraceDiagnostics(trace);
            this.lazyMessage = this.CreateLazyMessage(originalException.CancellationToken);
            this.toStringMessage = this.CreateToStringMessage(originalException.CancellationToken);
        }

        /// <inheritdoc/>
        public override string Source
        {
            get => this.originalException.Source;
            set => this.originalException.Source = value;
        }

        /// <inheritdoc/>
        public override string Message => this.lazyMessage.Value;

        /// <inheritdoc/>
        public override string StackTrace => this.originalException.StackTrace;

        /// <inheritdoc/>
        public override IDictionary Data => this.originalException.Data;

        /// <summary>
        /// Gets the diagnostics for the request
        /// </summary>
        public CosmosDiagnostics Diagnostics { get; }

        /// <inheritdoc/>
        public override string HelpLink
        {
            get => this.originalException.HelpLink;
            set => this.originalException.HelpLink = value;
        }

        /// <inheritdoc/>
        public override Exception GetBaseException()
        {
            return this.originalException.GetBaseException();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return this.toStringMessage.Value;
        }

        private Lazy<string> CreateLazyMessage(CancellationToken token)
        {
            return new Lazy<string>(() => $"{this.originalException.Message}{Environment.NewLine}Cancellation Token has expired: {token.IsCancellationRequested}. Learn more at: https://aka.ms/cosmosdb-tsg-request-timeout{Environment.NewLine}CosmosDiagnostics: {this.Diagnostics}");
        }

        private Lazy<string> CreateToStringMessage(CancellationToken token)
        {
            return new Lazy<string>(() => $"{this.originalException}{Environment.NewLine}Cancellation Token has expired: {token.IsCancellationRequested}. Learn more at: https://aka.ms/cosmosdb-tsg-request-timeout{Environment.NewLine}CosmosDiagnostics: {this.Diagnostics}");
        }
    }
}
