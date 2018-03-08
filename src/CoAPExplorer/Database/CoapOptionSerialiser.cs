﻿using System;
using System.Collections.Generic;
using System.Text;
using CoAPNet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace CoAPExplorer.Database
{
    //[JsonConverter(typeof(StringFlagEnumConverter))]
    public class CoapOptionConverter : JsonConverter<CoapOption>
    {
        // TODO: Support custom option factory
        private readonly CoAPNet.Options.OptionFactory _factory = CoAPNet.Options.OptionFactory.Default;

        public override CoapOption ReadJson(JsonReader reader, Type objectType, CoapOption existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            if (reader.TokenType != JsonToken.StartObject)
                return null;

            reader.Read();

            byte[] data = new byte[] { };
            int? number = 0;

            while (reader.TokenType != JsonToken.EndObject)
            {

                var name = reader.ReadAsString();
                if (name == "n")
                    number = reader.ReadAsInt32();
                if (name == "d")
                    data = reader.ReadAsBytes();
            }

            return _factory.Create(number.Value, data);
        }

        public override void WriteJson(JsonWriter writer, CoapOption value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            writer.WriteStartObject();

            writer.WritePropertyName("n");
            writer.WriteValue(value.OptionNumber);

            writer.WritePropertyName("d");
            writer.WriteValue(value.GetBytes());

            writer.WriteEndObject();
        }
    }
}
