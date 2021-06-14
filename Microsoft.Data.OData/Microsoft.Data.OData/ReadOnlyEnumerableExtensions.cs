using System;
using System.Collections.Generic;

namespace Microsoft.Data.OData
{
	// Token: 0x02000135 RID: 309
	internal static class ReadOnlyEnumerableExtensions
	{
		// Token: 0x06000828 RID: 2088 RVA: 0x0001AC57 File Offset: 0x00018E57
		internal static bool IsEmptyReadOnlyEnumerable<T>(this IEnumerable<T> source)
		{
			return object.ReferenceEquals(source, ReadOnlyEnumerable<T>.Empty());
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x0001AC64 File Offset: 0x00018E64
		internal static ReadOnlyEnumerable<T> ToReadOnlyEnumerable<T>(this IEnumerable<T> source, string collectionName)
		{
			ReadOnlyEnumerable<T> readOnlyEnumerable = source as ReadOnlyEnumerable<T>;
			if (readOnlyEnumerable == null)
			{
				throw new ODataException(Strings.ReaderUtils_EnumerableModified(collectionName));
			}
			return readOnlyEnumerable;
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x0001AC88 File Offset: 0x00018E88
		internal static ReadOnlyEnumerable<T> GetOrCreateReadOnlyEnumerable<T>(this IEnumerable<T> source, string collectionName)
		{
			if (source.IsEmptyReadOnlyEnumerable<T>())
			{
				return new ReadOnlyEnumerable<T>();
			}
			return source.ToReadOnlyEnumerable(collectionName);
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x0001ACA0 File Offset: 0x00018EA0
		internal static ReadOnlyEnumerable<T> ConcatToReadOnlyEnumerable<T>(this IEnumerable<T> source, string collectionName, T item)
		{
			ReadOnlyEnumerable<T> orCreateReadOnlyEnumerable = source.GetOrCreateReadOnlyEnumerable(collectionName);
			orCreateReadOnlyEnumerable.AddToSourceList(item);
			return orCreateReadOnlyEnumerable;
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x0001ACBD File Offset: 0x00018EBD
		internal static void AddAction(this ODataEntry entry, ODataAction action)
		{
			entry.Actions = entry.Actions.ConcatToReadOnlyEnumerable("Actions", action);
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x0001ACD6 File Offset: 0x00018ED6
		internal static void AddFunction(this ODataEntry entry, ODataFunction function)
		{
			entry.Functions = entry.Functions.ConcatToReadOnlyEnumerable("Functions", function);
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x0001ACEF File Offset: 0x00018EEF
		internal static void AddAssociationLink(this ODataEntry entry, ODataAssociationLink associationLink)
		{
			entry.AssociationLinks = entry.AssociationLinks.ConcatToReadOnlyEnumerable("AssociationLinks", associationLink);
		}
	}
}
