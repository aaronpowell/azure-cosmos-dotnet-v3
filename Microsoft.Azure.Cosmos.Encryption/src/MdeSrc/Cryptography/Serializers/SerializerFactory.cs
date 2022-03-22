//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

// This file isn't generated, but this comment is necessary to exclude it from StyleCop analysis.
// <auto-generated/>

using System;

namespace Microsoft.Data.Encryption.Cryptography.Serializers
{
    /// <summary>
    /// Provides methods for getting serializer implementations, such as by type and ID.
    /// </summary>
    internal abstract class SerializerFactory
    {
        /// <summary>
        /// Gets a registered serializer by its Identifier Property.
        /// </summary>
        /// <param name="identifier">The identifier uniquely identifies a particular Serializer implementation.</param>
        /// <returns>The ISerializer implementation</returns>
        public abstract ISerializer GetSerializer(string identifier);

        /// <summary>
        /// Gets a default registered serializer for the type.
        /// </summary>
        /// <typeparam name="T">The data type to be serialized.</typeparam>
        /// <returns>A default registered serializer for the type.</returns>
        public abstract Serializer<T> GetDefaultSerializer<T>();

        /// <summary>
        /// Registers a custom serializer.
        /// </summary>
        /// <param name="type">The data type on which the Serializer operates.</param>
        /// <param name="serializer">The Serializer to register.</param>
        /// <param name="overrideDefault">Determines whether to override an existing default serializer for the same type.</param>
        public abstract void RegisterSerializer(Type type, ISerializer serializer, bool overrideDefault = false);

    }
}