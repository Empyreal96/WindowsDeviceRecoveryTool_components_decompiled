using System;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000A9 RID: 169
	public interface IValueProvider
	{
		// Token: 0x0600085B RID: 2139
		void SetValue(object target, object value);

		// Token: 0x0600085C RID: 2140
		object GetValue(object target);
	}
}
