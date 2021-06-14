using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x02000092 RID: 146
	internal class JsonSchemaModel
	{
		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000792 RID: 1938 RVA: 0x0001D2A8 File Offset: 0x0001B4A8
		// (set) Token: 0x06000793 RID: 1939 RVA: 0x0001D2B0 File Offset: 0x0001B4B0
		public bool Required { get; set; }

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000794 RID: 1940 RVA: 0x0001D2B9 File Offset: 0x0001B4B9
		// (set) Token: 0x06000795 RID: 1941 RVA: 0x0001D2C1 File Offset: 0x0001B4C1
		public JsonSchemaType Type { get; set; }

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000796 RID: 1942 RVA: 0x0001D2CA File Offset: 0x0001B4CA
		// (set) Token: 0x06000797 RID: 1943 RVA: 0x0001D2D2 File Offset: 0x0001B4D2
		public int? MinimumLength { get; set; }

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06000798 RID: 1944 RVA: 0x0001D2DB File Offset: 0x0001B4DB
		// (set) Token: 0x06000799 RID: 1945 RVA: 0x0001D2E3 File Offset: 0x0001B4E3
		public int? MaximumLength { get; set; }

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x0600079A RID: 1946 RVA: 0x0001D2EC File Offset: 0x0001B4EC
		// (set) Token: 0x0600079B RID: 1947 RVA: 0x0001D2F4 File Offset: 0x0001B4F4
		public double? DivisibleBy { get; set; }

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x0600079C RID: 1948 RVA: 0x0001D2FD File Offset: 0x0001B4FD
		// (set) Token: 0x0600079D RID: 1949 RVA: 0x0001D305 File Offset: 0x0001B505
		public double? Minimum { get; set; }

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x0600079E RID: 1950 RVA: 0x0001D30E File Offset: 0x0001B50E
		// (set) Token: 0x0600079F RID: 1951 RVA: 0x0001D316 File Offset: 0x0001B516
		public double? Maximum { get; set; }

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x060007A0 RID: 1952 RVA: 0x0001D31F File Offset: 0x0001B51F
		// (set) Token: 0x060007A1 RID: 1953 RVA: 0x0001D327 File Offset: 0x0001B527
		public bool ExclusiveMinimum { get; set; }

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x060007A2 RID: 1954 RVA: 0x0001D330 File Offset: 0x0001B530
		// (set) Token: 0x060007A3 RID: 1955 RVA: 0x0001D338 File Offset: 0x0001B538
		public bool ExclusiveMaximum { get; set; }

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x060007A4 RID: 1956 RVA: 0x0001D341 File Offset: 0x0001B541
		// (set) Token: 0x060007A5 RID: 1957 RVA: 0x0001D349 File Offset: 0x0001B549
		public int? MinimumItems { get; set; }

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x060007A6 RID: 1958 RVA: 0x0001D352 File Offset: 0x0001B552
		// (set) Token: 0x060007A7 RID: 1959 RVA: 0x0001D35A File Offset: 0x0001B55A
		public int? MaximumItems { get; set; }

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x060007A8 RID: 1960 RVA: 0x0001D363 File Offset: 0x0001B563
		// (set) Token: 0x060007A9 RID: 1961 RVA: 0x0001D36B File Offset: 0x0001B56B
		public IList<string> Patterns { get; set; }

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x060007AA RID: 1962 RVA: 0x0001D374 File Offset: 0x0001B574
		// (set) Token: 0x060007AB RID: 1963 RVA: 0x0001D37C File Offset: 0x0001B57C
		public IList<JsonSchemaModel> Items { get; set; }

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x060007AC RID: 1964 RVA: 0x0001D385 File Offset: 0x0001B585
		// (set) Token: 0x060007AD RID: 1965 RVA: 0x0001D38D File Offset: 0x0001B58D
		public IDictionary<string, JsonSchemaModel> Properties { get; set; }

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x060007AE RID: 1966 RVA: 0x0001D396 File Offset: 0x0001B596
		// (set) Token: 0x060007AF RID: 1967 RVA: 0x0001D39E File Offset: 0x0001B59E
		public IDictionary<string, JsonSchemaModel> PatternProperties { get; set; }

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x060007B0 RID: 1968 RVA: 0x0001D3A7 File Offset: 0x0001B5A7
		// (set) Token: 0x060007B1 RID: 1969 RVA: 0x0001D3AF File Offset: 0x0001B5AF
		public JsonSchemaModel AdditionalProperties { get; set; }

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x060007B2 RID: 1970 RVA: 0x0001D3B8 File Offset: 0x0001B5B8
		// (set) Token: 0x060007B3 RID: 1971 RVA: 0x0001D3C0 File Offset: 0x0001B5C0
		public JsonSchemaModel AdditionalItems { get; set; }

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x060007B4 RID: 1972 RVA: 0x0001D3C9 File Offset: 0x0001B5C9
		// (set) Token: 0x060007B5 RID: 1973 RVA: 0x0001D3D1 File Offset: 0x0001B5D1
		public bool PositionalItemsValidation { get; set; }

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x060007B6 RID: 1974 RVA: 0x0001D3DA File Offset: 0x0001B5DA
		// (set) Token: 0x060007B7 RID: 1975 RVA: 0x0001D3E2 File Offset: 0x0001B5E2
		public bool AllowAdditionalProperties { get; set; }

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x060007B8 RID: 1976 RVA: 0x0001D3EB File Offset: 0x0001B5EB
		// (set) Token: 0x060007B9 RID: 1977 RVA: 0x0001D3F3 File Offset: 0x0001B5F3
		public bool AllowAdditionalItems { get; set; }

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x060007BA RID: 1978 RVA: 0x0001D3FC File Offset: 0x0001B5FC
		// (set) Token: 0x060007BB RID: 1979 RVA: 0x0001D404 File Offset: 0x0001B604
		public bool UniqueItems { get; set; }

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x060007BC RID: 1980 RVA: 0x0001D40D File Offset: 0x0001B60D
		// (set) Token: 0x060007BD RID: 1981 RVA: 0x0001D415 File Offset: 0x0001B615
		public IList<JToken> Enum { get; set; }

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x060007BE RID: 1982 RVA: 0x0001D41E File Offset: 0x0001B61E
		// (set) Token: 0x060007BF RID: 1983 RVA: 0x0001D426 File Offset: 0x0001B626
		public JsonSchemaType Disallow { get; set; }

		// Token: 0x060007C0 RID: 1984 RVA: 0x0001D42F File Offset: 0x0001B62F
		public JsonSchemaModel()
		{
			this.Type = JsonSchemaType.Any;
			this.AllowAdditionalProperties = true;
			this.AllowAdditionalItems = true;
			this.Required = false;
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x0001D454 File Offset: 0x0001B654
		public static JsonSchemaModel Create(IList<JsonSchema> schemata)
		{
			JsonSchemaModel jsonSchemaModel = new JsonSchemaModel();
			foreach (JsonSchema schema in schemata)
			{
				JsonSchemaModel.Combine(jsonSchemaModel, schema);
			}
			return jsonSchemaModel;
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x0001D4A4 File Offset: 0x0001B6A4
		private static void Combine(JsonSchemaModel model, JsonSchema schema)
		{
			model.Required = (model.Required || (schema.Required ?? false));
			model.Type &= (schema.Type ?? JsonSchemaType.Any);
			model.MinimumLength = MathUtils.Max(model.MinimumLength, schema.MinimumLength);
			model.MaximumLength = MathUtils.Min(model.MaximumLength, schema.MaximumLength);
			model.DivisibleBy = MathUtils.Max(model.DivisibleBy, schema.DivisibleBy);
			model.Minimum = MathUtils.Max(model.Minimum, schema.Minimum);
			model.Maximum = MathUtils.Max(model.Maximum, schema.Maximum);
			model.ExclusiveMinimum = (model.ExclusiveMinimum || (schema.ExclusiveMinimum ?? false));
			model.ExclusiveMaximum = (model.ExclusiveMaximum || (schema.ExclusiveMaximum ?? false));
			model.MinimumItems = MathUtils.Max(model.MinimumItems, schema.MinimumItems);
			model.MaximumItems = MathUtils.Min(model.MaximumItems, schema.MaximumItems);
			model.PositionalItemsValidation = (model.PositionalItemsValidation || schema.PositionalItemsValidation);
			model.AllowAdditionalProperties = (model.AllowAdditionalProperties && schema.AllowAdditionalProperties);
			model.AllowAdditionalItems = (model.AllowAdditionalItems && schema.AllowAdditionalItems);
			model.UniqueItems = (model.UniqueItems || schema.UniqueItems);
			if (schema.Enum != null)
			{
				if (model.Enum == null)
				{
					model.Enum = new List<JToken>();
				}
				model.Enum.AddRangeDistinct(schema.Enum, JToken.EqualityComparer);
			}
			model.Disallow |= (schema.Disallow ?? JsonSchemaType.None);
			if (schema.Pattern != null)
			{
				if (model.Patterns == null)
				{
					model.Patterns = new List<string>();
				}
				model.Patterns.AddDistinct(schema.Pattern);
			}
		}
	}
}
