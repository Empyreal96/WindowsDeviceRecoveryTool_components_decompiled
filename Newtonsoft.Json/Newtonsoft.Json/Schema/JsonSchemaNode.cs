using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x02000094 RID: 148
	internal class JsonSchemaNode
	{
		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x060007CC RID: 1996 RVA: 0x0001DB5E File Offset: 0x0001BD5E
		// (set) Token: 0x060007CD RID: 1997 RVA: 0x0001DB66 File Offset: 0x0001BD66
		public string Id { get; private set; }

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060007CE RID: 1998 RVA: 0x0001DB6F File Offset: 0x0001BD6F
		// (set) Token: 0x060007CF RID: 1999 RVA: 0x0001DB77 File Offset: 0x0001BD77
		public ReadOnlyCollection<JsonSchema> Schemas { get; private set; }

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x060007D0 RID: 2000 RVA: 0x0001DB80 File Offset: 0x0001BD80
		// (set) Token: 0x060007D1 RID: 2001 RVA: 0x0001DB88 File Offset: 0x0001BD88
		public Dictionary<string, JsonSchemaNode> Properties { get; private set; }

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x060007D2 RID: 2002 RVA: 0x0001DB91 File Offset: 0x0001BD91
		// (set) Token: 0x060007D3 RID: 2003 RVA: 0x0001DB99 File Offset: 0x0001BD99
		public Dictionary<string, JsonSchemaNode> PatternProperties { get; private set; }

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x060007D4 RID: 2004 RVA: 0x0001DBA2 File Offset: 0x0001BDA2
		// (set) Token: 0x060007D5 RID: 2005 RVA: 0x0001DBAA File Offset: 0x0001BDAA
		public List<JsonSchemaNode> Items { get; private set; }

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x060007D6 RID: 2006 RVA: 0x0001DBB3 File Offset: 0x0001BDB3
		// (set) Token: 0x060007D7 RID: 2007 RVA: 0x0001DBBB File Offset: 0x0001BDBB
		public JsonSchemaNode AdditionalProperties { get; set; }

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x060007D8 RID: 2008 RVA: 0x0001DBC4 File Offset: 0x0001BDC4
		// (set) Token: 0x060007D9 RID: 2009 RVA: 0x0001DBCC File Offset: 0x0001BDCC
		public JsonSchemaNode AdditionalItems { get; set; }

		// Token: 0x060007DA RID: 2010 RVA: 0x0001DBD8 File Offset: 0x0001BDD8
		public JsonSchemaNode(JsonSchema schema)
		{
			this.Schemas = new ReadOnlyCollection<JsonSchema>(new JsonSchema[]
			{
				schema
			});
			this.Properties = new Dictionary<string, JsonSchemaNode>();
			this.PatternProperties = new Dictionary<string, JsonSchemaNode>();
			this.Items = new List<JsonSchemaNode>();
			this.Id = JsonSchemaNode.GetId(this.Schemas);
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x0001DC34 File Offset: 0x0001BE34
		private JsonSchemaNode(JsonSchemaNode source, JsonSchema schema)
		{
			this.Schemas = new ReadOnlyCollection<JsonSchema>(source.Schemas.Union(new JsonSchema[]
			{
				schema
			}).ToList<JsonSchema>());
			this.Properties = new Dictionary<string, JsonSchemaNode>(source.Properties);
			this.PatternProperties = new Dictionary<string, JsonSchemaNode>(source.PatternProperties);
			this.Items = new List<JsonSchemaNode>(source.Items);
			this.AdditionalProperties = source.AdditionalProperties;
			this.AdditionalItems = source.AdditionalItems;
			this.Id = JsonSchemaNode.GetId(this.Schemas);
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x0001DCCA File Offset: 0x0001BECA
		public JsonSchemaNode Combine(JsonSchema schema)
		{
			return new JsonSchemaNode(this, schema);
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x0001DCE0 File Offset: 0x0001BEE0
		public static string GetId(IEnumerable<JsonSchema> schemata)
		{
			return string.Join("-", (from s in schemata
			select s.InternalId).OrderBy((string id) => id, StringComparer.Ordinal).ToArray<string>());
		}
	}
}
