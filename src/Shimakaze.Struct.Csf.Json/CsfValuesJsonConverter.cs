﻿using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shimakaze.Struct.Csf.Json
{
    public class CsfValuesJsonConverter : JsonConverter<List<CsfValue>>
    {
        public override List<CsfValue> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
                throw new JsonException();

            var converter = options.GetConverter<CsfValue>();
            var result = new List<CsfValue>();
            while (reader.Read())
            {
                if (reader.TokenType is JsonTokenType.EndArray)
                    break;

                result.Add(converter.Read(ref reader, options));
            }
            return result;
        }
        public override void Write(Utf8JsonWriter writer, List<CsfValue> value, JsonSerializerOptions options)
        {
            var converter = options.GetConverter<CsfValue>();
            if (value.Count > 1)
            {
                writer.WritePropertyName(nameof(CsfLabel.Values).ToLower());
                writer.WriteStartArray();
                value.ForEach(i => converter.Write(writer, i, options));
                writer.WriteEndArray();
            }
            else
            {
                writer.WritePropertyName("value");
                //if (string.IsNullOrEmpty(value[0].Extra))
                //{
                //    writer.WriteStartObject();
                //    writer.WritePropertyName(nameof(value[0].Value).ToLower());
                //    converter.Write(writer, value.Value, options);

                //    writer.WriteString(nameof(value.Extra).ToLower(), value.Extra);
                //    writer.WriteEndObject();
                //}
                converter.Write(writer, value[0], options);
            }
        }
    }
}
