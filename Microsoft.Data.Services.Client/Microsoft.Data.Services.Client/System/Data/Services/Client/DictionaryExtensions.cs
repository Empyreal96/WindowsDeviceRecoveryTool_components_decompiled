using System;
using System.Collections.Generic;

namespace System.Data.Services.Client
{
	// Token: 0x02000058 RID: 88
	internal static class DictionaryExtensions
	{
		// Token: 0x060002E5 RID: 741 RVA: 0x0000DA74 File Offset: 0x0000BC74
		internal static TValue FindOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> createValue)
		{
			TValue result;
			if (!dictionary.TryGetValue(key, out result))
			{
				result = (dictionary[key] = createValue());
			}
			return result;
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000DA9C File Offset: 0x0000BC9C
		internal static void SetRange<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IEnumerable<KeyValuePair<TKey, TValue>> valuesToCopy)
		{
			foreach (KeyValuePair<TKey, TValue> keyValuePair in valuesToCopy)
			{
				dictionary[keyValuePair.Key] = keyValuePair.Value;
			}
		}
	}
}
