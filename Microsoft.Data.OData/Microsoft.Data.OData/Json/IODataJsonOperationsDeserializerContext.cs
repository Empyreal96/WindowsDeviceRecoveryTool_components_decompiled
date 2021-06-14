using System;

namespace Microsoft.Data.OData.Json
{
	// Token: 0x02000174 RID: 372
	internal interface IODataJsonOperationsDeserializerContext
	{
		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000A90 RID: 2704
		JsonReader JsonReader { get; }

		// Token: 0x06000A91 RID: 2705
		Uri ProcessUriFromPayload(string uriFromPayload);

		// Token: 0x06000A92 RID: 2706
		void AddActionToEntry(ODataAction action);

		// Token: 0x06000A93 RID: 2707
		void AddFunctionToEntry(ODataFunction function);
	}
}
