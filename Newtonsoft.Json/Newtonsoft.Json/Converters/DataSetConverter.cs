using System;
using System.Data;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x0200001B RID: 27
	public class DataSetConverter : JsonConverter
	{
		// Token: 0x0600014D RID: 333 RVA: 0x000064EC File Offset: 0x000046EC
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			DataSet dataSet = (DataSet)value;
			DefaultContractResolver defaultContractResolver = serializer.ContractResolver as DefaultContractResolver;
			DataTableConverter dataTableConverter = new DataTableConverter();
			writer.WriteStartObject();
			foreach (object obj in dataSet.Tables)
			{
				DataTable dataTable = (DataTable)obj;
				writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName(dataTable.TableName) : dataTable.TableName);
				dataTableConverter.WriteJson(writer, dataTable, serializer);
			}
			writer.WriteEndObject();
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00006590 File Offset: 0x00004790
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			DataSet dataSet = (objectType == typeof(DataSet)) ? new DataSet() : ((DataSet)Activator.CreateInstance(objectType));
			DataTableConverter dataTableConverter = new DataTableConverter();
			this.CheckedRead(reader);
			while (reader.TokenType == JsonToken.PropertyName)
			{
				DataTable dataTable = dataSet.Tables[(string)reader.Value];
				bool flag = dataTable != null;
				dataTable = (DataTable)dataTableConverter.ReadJson(reader, typeof(DataTable), dataTable, serializer);
				if (!flag)
				{
					dataSet.Tables.Add(dataTable);
				}
				this.CheckedRead(reader);
			}
			return dataSet;
		}

		// Token: 0x0600014F RID: 335 RVA: 0x0000662A File Offset: 0x0000482A
		public override bool CanConvert(Type valueType)
		{
			return typeof(DataSet).IsAssignableFrom(valueType);
		}

		// Token: 0x06000150 RID: 336 RVA: 0x0000663C File Offset: 0x0000483C
		private void CheckedRead(JsonReader reader)
		{
			if (!reader.Read())
			{
				throw JsonSerializationException.Create(reader, "Unexpected end when reading DataSet.");
			}
		}
	}
}
