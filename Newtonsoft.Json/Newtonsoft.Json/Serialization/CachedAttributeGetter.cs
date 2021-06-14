using System;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200009C RID: 156
	internal static class CachedAttributeGetter<T> where T : Attribute
	{
		// Token: 0x060007F6 RID: 2038 RVA: 0x0001E5B7 File Offset: 0x0001C7B7
		public static T GetAttribute(object type)
		{
			return CachedAttributeGetter<T>.TypeAttributeCache.Get(type);
		}

		// Token: 0x040002AF RID: 687
		private static readonly ThreadSafeStore<object, T> TypeAttributeCache = new ThreadSafeStore<object, T>(new Func<object, T>(JsonTypeReflector.GetAttribute<T>));
	}
}
