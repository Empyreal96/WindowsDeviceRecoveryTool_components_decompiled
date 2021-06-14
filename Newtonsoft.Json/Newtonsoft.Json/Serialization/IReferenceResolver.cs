using System;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000A3 RID: 163
	public interface IReferenceResolver
	{
		// Token: 0x06000841 RID: 2113
		object ResolveReference(object context, string reference);

		// Token: 0x06000842 RID: 2114
		string GetReference(object context, object value);

		// Token: 0x06000843 RID: 2115
		bool IsReferenced(object context, object value);

		// Token: 0x06000844 RID: 2116
		void AddReference(object context, string reference, object value);
	}
}
