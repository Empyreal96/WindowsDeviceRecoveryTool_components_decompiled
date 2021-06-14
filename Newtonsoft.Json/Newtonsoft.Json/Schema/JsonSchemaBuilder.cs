using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x0200008D RID: 141
	internal class JsonSchemaBuilder
	{
		// Token: 0x06000759 RID: 1881 RVA: 0x0001B84B File Offset: 0x00019A4B
		public JsonSchemaBuilder(JsonSchemaResolver resolver)
		{
			this._stack = new List<JsonSchema>();
			this._documentSchemas = new Dictionary<string, JsonSchema>();
			this._resolver = resolver;
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x0001B870 File Offset: 0x00019A70
		private void Push(JsonSchema value)
		{
			this._currentSchema = value;
			this._stack.Add(value);
			this._resolver.LoadedSchemas.Add(value);
			this._documentSchemas.Add(value.Location, value);
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x0001B8A8 File Offset: 0x00019AA8
		private JsonSchema Pop()
		{
			JsonSchema currentSchema = this._currentSchema;
			this._stack.RemoveAt(this._stack.Count - 1);
			this._currentSchema = this._stack.LastOrDefault<JsonSchema>();
			return currentSchema;
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x0600075C RID: 1884 RVA: 0x0001B8E6 File Offset: 0x00019AE6
		private JsonSchema CurrentSchema
		{
			get
			{
				return this._currentSchema;
			}
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x0001B8F0 File Offset: 0x00019AF0
		internal JsonSchema Read(JsonReader reader)
		{
			JToken jtoken = JToken.ReadFrom(reader);
			this._rootSchema = (jtoken as JObject);
			JsonSchema jsonSchema = this.BuildSchema(jtoken);
			this.ResolveReferences(jsonSchema);
			return jsonSchema;
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x0001B921 File Offset: 0x00019B21
		private string UnescapeReference(string reference)
		{
			return Uri.UnescapeDataString(reference).Replace("~1", "/").Replace("~0", "~");
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x0001B948 File Offset: 0x00019B48
		private JsonSchema ResolveReferences(JsonSchema schema)
		{
			if (schema.DeferredReference != null)
			{
				string text = schema.DeferredReference;
				bool flag = text.StartsWith("#", StringComparison.Ordinal);
				if (flag)
				{
					text = this.UnescapeReference(text);
				}
				JsonSchema jsonSchema = this._resolver.GetSchema(text);
				if (jsonSchema == null)
				{
					if (flag)
					{
						string[] array = schema.DeferredReference.TrimStart(new char[]
						{
							'#'
						}).Split(new char[]
						{
							'/'
						}, StringSplitOptions.RemoveEmptyEntries);
						JToken jtoken = this._rootSchema;
						foreach (string reference in array)
						{
							string text2 = this.UnescapeReference(reference);
							if (jtoken.Type == JTokenType.Object)
							{
								jtoken = jtoken[text2];
							}
							else if (jtoken.Type == JTokenType.Array || jtoken.Type == JTokenType.Constructor)
							{
								int num;
								if (int.TryParse(text2, out num) && num >= 0 && num < jtoken.Count<JToken>())
								{
									jtoken = jtoken[num];
								}
								else
								{
									jtoken = null;
								}
							}
							if (jtoken == null)
							{
								break;
							}
						}
						if (jtoken != null)
						{
							jsonSchema = this.BuildSchema(jtoken);
						}
					}
					if (jsonSchema == null)
					{
						throw new JsonException("Could not resolve schema reference '{0}'.".FormatWith(CultureInfo.InvariantCulture, schema.DeferredReference));
					}
				}
				schema = jsonSchema;
			}
			if (schema.ReferencesResolved)
			{
				return schema;
			}
			schema.ReferencesResolved = true;
			if (schema.Extends != null)
			{
				for (int j = 0; j < schema.Extends.Count; j++)
				{
					schema.Extends[j] = this.ResolveReferences(schema.Extends[j]);
				}
			}
			if (schema.Items != null)
			{
				for (int k = 0; k < schema.Items.Count; k++)
				{
					schema.Items[k] = this.ResolveReferences(schema.Items[k]);
				}
			}
			if (schema.AdditionalItems != null)
			{
				schema.AdditionalItems = this.ResolveReferences(schema.AdditionalItems);
			}
			if (schema.PatternProperties != null)
			{
				foreach (KeyValuePair<string, JsonSchema> keyValuePair in schema.PatternProperties.ToList<KeyValuePair<string, JsonSchema>>())
				{
					schema.PatternProperties[keyValuePair.Key] = this.ResolveReferences(keyValuePair.Value);
				}
			}
			if (schema.Properties != null)
			{
				foreach (KeyValuePair<string, JsonSchema> keyValuePair2 in schema.Properties.ToList<KeyValuePair<string, JsonSchema>>())
				{
					schema.Properties[keyValuePair2.Key] = this.ResolveReferences(keyValuePair2.Value);
				}
			}
			if (schema.AdditionalProperties != null)
			{
				schema.AdditionalProperties = this.ResolveReferences(schema.AdditionalProperties);
			}
			return schema;
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x0001BC28 File Offset: 0x00019E28
		private JsonSchema BuildSchema(JToken token)
		{
			JObject jobject = token as JObject;
			if (jobject == null)
			{
				throw JsonException.Create(token, token.Path, "Expected object while parsing schema object, got {0}.".FormatWith(CultureInfo.InvariantCulture, token.Type));
			}
			JToken value;
			if (jobject.TryGetValue("$ref", out value))
			{
				return new JsonSchema
				{
					DeferredReference = (string)value
				};
			}
			string text = token.Path.Replace(".", "/").Replace("[", "/").Replace("]", string.Empty);
			if (!string.IsNullOrEmpty(text))
			{
				text = "/" + text;
			}
			text = "#" + text;
			JsonSchema result;
			if (this._documentSchemas.TryGetValue(text, out result))
			{
				return result;
			}
			this.Push(new JsonSchema
			{
				Location = text
			});
			this.ProcessSchemaProperties(jobject);
			return this.Pop();
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x0001BD14 File Offset: 0x00019F14
		private void ProcessSchemaProperties(JObject schemaObject)
		{
			foreach (KeyValuePair<string, JToken> keyValuePair in schemaObject)
			{
				string key;
				switch (key = keyValuePair.Key)
				{
				case "type":
					this.CurrentSchema.Type = this.ProcessType(keyValuePair.Value);
					break;
				case "id":
					this.CurrentSchema.Id = (string)keyValuePair.Value;
					break;
				case "title":
					this.CurrentSchema.Title = (string)keyValuePair.Value;
					break;
				case "description":
					this.CurrentSchema.Description = (string)keyValuePair.Value;
					break;
				case "properties":
					this.CurrentSchema.Properties = this.ProcessProperties(keyValuePair.Value);
					break;
				case "items":
					this.ProcessItems(keyValuePair.Value);
					break;
				case "additionalProperties":
					this.ProcessAdditionalProperties(keyValuePair.Value);
					break;
				case "additionalItems":
					this.ProcessAdditionalItems(keyValuePair.Value);
					break;
				case "patternProperties":
					this.CurrentSchema.PatternProperties = this.ProcessProperties(keyValuePair.Value);
					break;
				case "required":
					this.CurrentSchema.Required = new bool?((bool)keyValuePair.Value);
					break;
				case "requires":
					this.CurrentSchema.Requires = (string)keyValuePair.Value;
					break;
				case "minimum":
					this.CurrentSchema.Minimum = new double?((double)keyValuePair.Value);
					break;
				case "maximum":
					this.CurrentSchema.Maximum = new double?((double)keyValuePair.Value);
					break;
				case "exclusiveMinimum":
					this.CurrentSchema.ExclusiveMinimum = new bool?((bool)keyValuePair.Value);
					break;
				case "exclusiveMaximum":
					this.CurrentSchema.ExclusiveMaximum = new bool?((bool)keyValuePair.Value);
					break;
				case "maxLength":
					this.CurrentSchema.MaximumLength = new int?((int)keyValuePair.Value);
					break;
				case "minLength":
					this.CurrentSchema.MinimumLength = new int?((int)keyValuePair.Value);
					break;
				case "maxItems":
					this.CurrentSchema.MaximumItems = new int?((int)keyValuePair.Value);
					break;
				case "minItems":
					this.CurrentSchema.MinimumItems = new int?((int)keyValuePair.Value);
					break;
				case "divisibleBy":
					this.CurrentSchema.DivisibleBy = new double?((double)keyValuePair.Value);
					break;
				case "disallow":
					this.CurrentSchema.Disallow = this.ProcessType(keyValuePair.Value);
					break;
				case "default":
					this.CurrentSchema.Default = keyValuePair.Value.DeepClone();
					break;
				case "hidden":
					this.CurrentSchema.Hidden = new bool?((bool)keyValuePair.Value);
					break;
				case "readonly":
					this.CurrentSchema.ReadOnly = new bool?((bool)keyValuePair.Value);
					break;
				case "format":
					this.CurrentSchema.Format = (string)keyValuePair.Value;
					break;
				case "pattern":
					this.CurrentSchema.Pattern = (string)keyValuePair.Value;
					break;
				case "enum":
					this.ProcessEnum(keyValuePair.Value);
					break;
				case "extends":
					this.ProcessExtends(keyValuePair.Value);
					break;
				case "uniqueItems":
					this.CurrentSchema.UniqueItems = (bool)keyValuePair.Value;
					break;
				}
			}
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x0001C2BC File Offset: 0x0001A4BC
		private void ProcessExtends(JToken token)
		{
			IList<JsonSchema> list = new List<JsonSchema>();
			if (token.Type == JTokenType.Array)
			{
				using (IEnumerator<JToken> enumerator = ((IEnumerable<JToken>)token).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						JToken token2 = enumerator.Current;
						list.Add(this.BuildSchema(token2));
					}
					goto IL_52;
				}
			}
			JsonSchema jsonSchema = this.BuildSchema(token);
			if (jsonSchema != null)
			{
				list.Add(jsonSchema);
			}
			IL_52:
			if (list.Count > 0)
			{
				this.CurrentSchema.Extends = list;
			}
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x0001C340 File Offset: 0x0001A540
		private void ProcessEnum(JToken token)
		{
			if (token.Type != JTokenType.Array)
			{
				throw JsonException.Create(token, token.Path, "Expected Array token while parsing enum values, got {0}.".FormatWith(CultureInfo.InvariantCulture, token.Type));
			}
			this.CurrentSchema.Enum = new List<JToken>();
			foreach (JToken jtoken in ((IEnumerable<JToken>)token))
			{
				this.CurrentSchema.Enum.Add(jtoken.DeepClone());
			}
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x0001C3D8 File Offset: 0x0001A5D8
		private void ProcessAdditionalProperties(JToken token)
		{
			if (token.Type == JTokenType.Boolean)
			{
				this.CurrentSchema.AllowAdditionalProperties = (bool)token;
				return;
			}
			this.CurrentSchema.AdditionalProperties = this.BuildSchema(token);
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x0001C408 File Offset: 0x0001A608
		private void ProcessAdditionalItems(JToken token)
		{
			if (token.Type == JTokenType.Boolean)
			{
				this.CurrentSchema.AllowAdditionalItems = (bool)token;
				return;
			}
			this.CurrentSchema.AdditionalItems = this.BuildSchema(token);
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x0001C438 File Offset: 0x0001A638
		private IDictionary<string, JsonSchema> ProcessProperties(JToken token)
		{
			IDictionary<string, JsonSchema> dictionary = new Dictionary<string, JsonSchema>();
			if (token.Type != JTokenType.Object)
			{
				throw JsonException.Create(token, token.Path, "Expected Object token while parsing schema properties, got {0}.".FormatWith(CultureInfo.InvariantCulture, token.Type));
			}
			foreach (JToken jtoken in ((IEnumerable<JToken>)token))
			{
				JProperty jproperty = (JProperty)jtoken;
				if (dictionary.ContainsKey(jproperty.Name))
				{
					throw new JsonException("Property {0} has already been defined in schema.".FormatWith(CultureInfo.InvariantCulture, jproperty.Name));
				}
				dictionary.Add(jproperty.Name, this.BuildSchema(jproperty.Value));
			}
			return dictionary;
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x0001C4F8 File Offset: 0x0001A6F8
		private void ProcessItems(JToken token)
		{
			this.CurrentSchema.Items = new List<JsonSchema>();
			switch (token.Type)
			{
			case JTokenType.Object:
				this.CurrentSchema.Items.Add(this.BuildSchema(token));
				this.CurrentSchema.PositionalItemsValidation = false;
				return;
			case JTokenType.Array:
				this.CurrentSchema.PositionalItemsValidation = true;
				using (IEnumerator<JToken> enumerator = ((IEnumerable<JToken>)token).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						JToken token2 = enumerator.Current;
						this.CurrentSchema.Items.Add(this.BuildSchema(token2));
					}
					return;
				}
				break;
			}
			throw JsonException.Create(token, token.Path, "Expected array or JSON schema object, got {0}.".FormatWith(CultureInfo.InvariantCulture, token.Type));
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x0001C5D0 File Offset: 0x0001A7D0
		private JsonSchemaType? ProcessType(JToken token)
		{
			JTokenType type = token.Type;
			if (type == JTokenType.Array)
			{
				JsonSchemaType? jsonSchemaType = new JsonSchemaType?(JsonSchemaType.None);
				foreach (JToken jtoken in ((IEnumerable<JToken>)token))
				{
					if (jtoken.Type != JTokenType.String)
					{
						throw JsonException.Create(jtoken, jtoken.Path, "Exception JSON schema type string token, got {0}.".FormatWith(CultureInfo.InvariantCulture, token.Type));
					}
					jsonSchemaType |= JsonSchemaBuilder.MapType((string)jtoken);
				}
				return jsonSchemaType;
			}
			if (type != JTokenType.String)
			{
				throw JsonException.Create(token, token.Path, "Expected array or JSON schema type string token, got {0}.".FormatWith(CultureInfo.InvariantCulture, token.Type));
			}
			return new JsonSchemaType?(JsonSchemaBuilder.MapType((string)token));
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x0001C6D0 File Offset: 0x0001A8D0
		internal static JsonSchemaType MapType(string type)
		{
			JsonSchemaType result;
			if (!JsonSchemaConstants.JsonSchemaTypeMapping.TryGetValue(type, out result))
			{
				throw new JsonException("Invalid JSON schema type: {0}".FormatWith(CultureInfo.InvariantCulture, type));
			}
			return result;
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x0001C71C File Offset: 0x0001A91C
		internal static string MapType(JsonSchemaType type)
		{
			return JsonSchemaConstants.JsonSchemaTypeMapping.Single((KeyValuePair<string, JsonSchemaType> kv) => kv.Value == type).Key;
		}

		// Token: 0x04000249 RID: 585
		private readonly IList<JsonSchema> _stack;

		// Token: 0x0400024A RID: 586
		private readonly JsonSchemaResolver _resolver;

		// Token: 0x0400024B RID: 587
		private readonly IDictionary<string, JsonSchema> _documentSchemas;

		// Token: 0x0400024C RID: 588
		private JsonSchema _currentSchema;

		// Token: 0x0400024D RID: 589
		private JObject _rootSchema;
	}
}
