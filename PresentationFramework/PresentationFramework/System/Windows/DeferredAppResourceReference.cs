using System;
using System.Collections;

namespace System.Windows
{
	// Token: 0x02000117 RID: 279
	internal class DeferredAppResourceReference : DeferredResourceReference
	{
		// Token: 0x06000BB1 RID: 2993 RVA: 0x0002AF6C File Offset: 0x0002916C
		internal DeferredAppResourceReference(ResourceDictionary dictionary, object resourceKey) : base(dictionary, resourceKey)
		{
		}

		// Token: 0x06000BB2 RID: 2994 RVA: 0x0002AF78 File Offset: 0x00029178
		internal override object GetValue(BaseValueSourceInternal valueSource)
		{
			object syncRoot = ((ICollection)Application.Current.Resources).SyncRoot;
			object value;
			lock (syncRoot)
			{
				value = base.GetValue(valueSource);
			}
			return value;
		}

		// Token: 0x06000BB3 RID: 2995 RVA: 0x0002AFC4 File Offset: 0x000291C4
		internal override Type GetValueType()
		{
			object syncRoot = ((ICollection)Application.Current.Resources).SyncRoot;
			Type valueType;
			lock (syncRoot)
			{
				valueType = base.GetValueType();
			}
			return valueType;
		}
	}
}
