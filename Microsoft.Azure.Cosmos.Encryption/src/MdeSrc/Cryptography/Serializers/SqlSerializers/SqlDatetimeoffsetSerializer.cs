//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

// This file isn't generated, but this comment is necessary to exclude it from StyleCop analysis.
// <auto-generated/>

using System;
using System.Linq;

using static System.BitConverter;
using static Microsoft.Data.Encryption.Resources.Strings;

namespace Microsoft.Data.Encryption.Cryptography.Serializers
{
    /// <summary>
    /// Contains the methods for serializing and deserializing <see cref="DateTimeOffset"/> type data objects
	/// that is compatible with the Always Encrypted feature in SQL Server and Azure SQL.
    /// </summary>
    internal sealed class SqlDateTimeOffsetSerializer : Serializer<DateTimeOffset>
    {
        private const int MaxScale = 7;
        private const int MinScale = 0;
        private const int DefaultScale = 7;
        private readonly SqlDateTime2Serializer sqlDateTime2Serializer;

        /// <summary>
        /// The <see cref="Identifier"/> uniquely identifies a particular Serializer implementation.
        /// </summary>
        public override string Identifier => "SQL_DateTimeOffset";

        private int scale;

        /// <summary>
        /// Gets or sets the number of decimal places to which Value is resolved. The default is 7.
        /// </summary>
        /// <exception cref="MicrosoftDataEncryptionException">
        /// Thrown when set to a value that is out of the valid range [0 - 7] for this setting.
        /// </exception>
        public int Scale
        {
            get => scale;
            set
            {
                if (value < MinScale || value > MaxScale)
                {
                    throw new MicrosoftDataEncryptionException(ValueOutOfRange.Format(value));
                }
                scale = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDateTimeOffsetSerializer"/> class.
        /// </summary>
        /// <param name="scale">The number of decimal places to which Value is resolved.</param>
        /// <exception cref="MicrosoftDataEncryptionException">
        /// Thrown when set to a value that is out of the valid range [0 - 7] for this setting.
        /// </exception>
        public SqlDateTimeOffsetSerializer(int scale = DefaultScale)
        {
            Scale = scale;
            sqlDateTime2Serializer = new SqlDateTime2Serializer(scale);
        }

        /// <summary>
        /// Deserializes the provided <paramref name="bytes"/>
        /// </summary>
        /// <param name="bytes">The data to be deserialized</param>
        /// <returns>The serialized data</returns>
        /// <exception cref="MicrosoftDataEncryptionException">
        /// <paramref name="bytes"/> is null.
        /// -or-
        /// The length of <paramref name="bytes"/> is less than 10.
        /// </exception>
        public override DateTimeOffset Deserialize(byte[] bytes)
        {
            const int DateTimeIndex = 0;
            const int TimeSpanIndex = sizeof(long);
            const int DataSize = sizeof(long) + sizeof(short);

            bytes.ValidateNotNull(nameof(bytes));
            bytes.ValidateSize(DataSize, nameof(bytes));

            byte[] dateTimePart = bytes.Skip(DateTimeIndex).Take(sizeof(long)).ToArray();
            byte[] offsetPart = bytes.Skip(TimeSpanIndex).Take(sizeof(short)).ToArray();

            short minutes = ToInt16(offsetPart, 0);
            DateTime dateTime = sqlDateTime2Serializer.Deserialize(dateTimePart).AddMinutes(minutes);
            TimeSpan offset = new TimeSpan(0, minutes, 0);
            return new DateTimeOffset(dateTime, offset);
        }

        /// <summary>
        /// Serializes the provided <paramref name="value"/>
        /// </summary>
        /// <param name="value">The value to be serialized</param>
        /// <returns>
        /// An array of bytes with length 10.
        /// </returns>
        public override byte[] Serialize(DateTimeOffset value)
        {
            byte[] datetimePart = sqlDateTime2Serializer.Serialize(value.UtcDateTime);
            short offsetMinutes = (short)value.Offset.TotalMinutes;
            byte[] offsetPart = GetBytes(offsetMinutes);
            return datetimePart.Concat(offsetPart).ToArray();
        }
    }
}