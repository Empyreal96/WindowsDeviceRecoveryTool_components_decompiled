using System;
using System.Collections.Generic;
using System.Linq;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x02000096 RID: 150
	public class JsonSchemaResolver
	{
		// Token: 0x170001AA RID: 426
		// (get) Token: 0x060007E2 RID: 2018 RVA: 0x0001DD56 File Offset: 0x0001BF56
		// (set) Token: 0x060007E3 RID: 2019 RVA: 0x0001DD5E File Offset: 0x0001BF5E
		public IList<JsonSchema> LoadedSchemas { get; protected set; }

		// Token: 0x060007E4 RID: 2020 RVA: 0x0001DD67 File Offset: 0x0001BF67
		public JsonSchemaResolver()
		{
			this.LoadedSchemas = new List<JsonSchema>();
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x0001DDAC File Offset: 0x0001BFAC
		public virtual JsonSchema GetSchema(string reference)
		{
			JsonSchema jsonSchema = this.LoadedSchemas.SingleOrDefault((JsonSchema s) => string.Equals(s.Id, reference, StringComparison.Ordinal));
			if (jsonSchema == null)
			{
				jsonSchema = this.LoadedSchemas.SingleOrDefault((JsonSchema s) => string.Equals(s.Location, reference, StringComparison.Ordinal));
			}
			return jsonSchema;
		}
	}
}
