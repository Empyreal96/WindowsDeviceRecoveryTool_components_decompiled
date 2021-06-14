using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x0200001C RID: 28
	public class DataTableConverter : JsonConverter
	{
		// Token: 0x06000152 RID: 338 RVA: 0x0000665C File Offset: 0x0000485C
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			DataTable dataTable = (DataTable)value;
			DefaultContractResolver defaultContractResolver = serializer.ContractResolver as DefaultContractResolver;
			writer.WriteStartArray();
			foreach (object obj in dataTable.Rows)
			{
				DataRow dataRow = (DataRow)obj;
				writer.WriteStartObject();
				foreach (object obj2 in dataRow.Table.Columns)
				{
					DataColumn dataColumn = (DataColumn)obj2;
					if (serializer.NullValueHandling != NullValueHandling.Ignore || (dataRow[dataColumn] != null && dataRow[dataColumn] != DBNull.Value))
					{
						writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName(dataColumn.ColumnName) : dataColumn.ColumnName);
						serializer.Serialize(writer, dataRow[dataColumn]);
					}
				}
				writer.WriteEndObject();
			}
			writer.WriteEndArray();
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00006780 File Offset: 0x00004980
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			DataTable dataTable = existingValue as DataTable;
			if (dataTable == null)
			{
				dataTable = ((objectType == typeof(DataTable)) ? new DataTable() : ((DataTable)Activator.CreateInstance(objectType)));
			}
			if (reader.TokenType == JsonToken.PropertyName)
			{
				dataTable.TableName = (string)reader.Value;
				DataTableConverter.CheckedRead(reader);
			}
			if (reader.TokenType != JsonToken.StartArray)
			{
				throw JsonSerializationException.Create(reader, "Unexpected JSON token when reading DataTable. Expected StartArray, got {0}.".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			DataTableConverter.CheckedRead(reader);
			while (reader.TokenType != JsonToken.EndArray)
			{
				DataTableConverter.CreateRow(reader, dataTable);
				DataTableConverter.CheckedRead(reader);
			}
			return dataTable;
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00006828 File Offset: 0x00004A28
		private static void CreateRow(JsonReader reader, DataTable dt)
		{
			DataRow dataRow = dt.NewRow();
			DataTableConverter.CheckedRead(reader);
			while (reader.TokenType == JsonToken.PropertyName)
			{
				string text = (string)reader.Value;
				DataTableConverter.CheckedRead(reader);
				DataColumn dataColumn = dt.Columns[text];
				if (dataColumn == null)
				{
					Type columnDataType = DataTableConverter.GetColumnDataType(reader);
					dataColumn = new DataColumn(text, columnDataType);
					dt.Columns.Add(dataColumn);
				}
				if (dataColumn.DataType == typeof(DataTable))
				{
					if (reader.TokenType == JsonToken.StartArray)
					{
						DataTableConverter.CheckedRead(reader);
					}
					DataTable dataTable = new DataTable();
					while (reader.TokenType != JsonToken.EndArray)
					{
						DataTableConverter.CreateRow(reader, dataTable);
						DataTableConverter.CheckedRead(reader);
					}
					dataRow[text] = dataTable;
				}
				else if (dataColumn.DataType.IsArray && dataColumn.DataType != typeof(byte[]))
				{
					if (reader.TokenType == JsonToken.StartArray)
					{
						DataTableConverter.CheckedRead(reader);
					}
					List<object> list = new List<object>();
					while (reader.TokenType != JsonToken.EndArray)
					{
						list.Add(reader.Value);
						DataTableConverter.CheckedRead(reader);
					}
					Array array = Array.CreateInstance(dataColumn.DataType.GetElementType(), list.Count);
					Array.Copy(list.ToArray(), array, list.Count);
					dataRow[text] = array;
				}
				else
				{
					dataRow[text] = (reader.Value ?? DBNull.Value);
				}
				DataTableConverter.CheckedRead(reader);
			}
			dataRow.EndEdit();
			dt.Rows.Add(dataRow);
		}

		// Token: 0x06000155 RID: 341 RVA: 0x000069A8 File Offset: 0x00004BA8
		private static Type GetColumnDataType(JsonReader reader)
		{
			JsonToken tokenType = reader.TokenType;
			switch (tokenType)
			{
			case JsonToken.StartArray:
			{
				DataTableConverter.CheckedRead(reader);
				if (reader.TokenType == JsonToken.StartObject)
				{
					return typeof(DataTable);
				}
				Type columnDataType = DataTableConverter.GetColumnDataType(reader);
				return columnDataType.MakeArrayType();
			}
			case JsonToken.Integer:
			case JsonToken.Float:
			case JsonToken.String:
			case JsonToken.Boolean:
			case JsonToken.Date:
			case JsonToken.Bytes:
				return reader.ValueType;
			case JsonToken.Null:
			case JsonToken.Undefined:
				return typeof(string);
			}
			throw JsonSerializationException.Create(reader, "Unexpected JSON token when reading DataTable: {0}".FormatWith(CultureInfo.InvariantCulture, tokenType));
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00006A5D File Offset: 0x00004C5D
		private static void CheckedRead(JsonReader reader)
		{
			if (!reader.Read())
			{
				throw JsonSerializationException.Create(reader, "Unexpected end when reading DataTable.");
			}
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00006A73 File Offset: 0x00004C73
		public override bool CanConvert(Type valueType)
		{
			return typeof(DataTable).IsAssignableFrom(valueType);
		}
	}
}
