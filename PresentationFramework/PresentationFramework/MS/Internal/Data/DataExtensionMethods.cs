using System;
using System.Collections;

namespace MS.Internal.Data
{
	// Token: 0x02000717 RID: 1815
	internal static class DataExtensionMethods
	{
		// Token: 0x060074CE RID: 29902 RVA: 0x00216A34 File Offset: 0x00214C34
		internal static int Search(this IList list, int index, int count, object value, IComparer comparer)
		{
			ArrayList arrayList;
			if ((arrayList = (list as ArrayList)) != null)
			{
				return arrayList.BinarySearch(index, count, value, comparer);
			}
			LiveShapingList liveShapingList;
			if ((liveShapingList = (list as LiveShapingList)) != null)
			{
				return liveShapingList.Search(index, count, value);
			}
			return 0;
		}

		// Token: 0x060074CF RID: 29903 RVA: 0x00216A6C File Offset: 0x00214C6C
		internal static int Search(this IList list, object value, IComparer comparer)
		{
			return list.Search(0, list.Count, value, comparer);
		}

		// Token: 0x060074D0 RID: 29904 RVA: 0x00216A80 File Offset: 0x00214C80
		internal static void Move(this IList list, int oldIndex, int newIndex)
		{
			ArrayList arrayList;
			if ((arrayList = (list as ArrayList)) != null)
			{
				object value = arrayList[oldIndex];
				arrayList.RemoveAt(oldIndex);
				arrayList.Insert(newIndex, value);
				return;
			}
			LiveShapingList liveShapingList;
			if ((liveShapingList = (list as LiveShapingList)) != null)
			{
				liveShapingList.Move(oldIndex, newIndex);
			}
		}

		// Token: 0x060074D1 RID: 29905 RVA: 0x00216AC4 File Offset: 0x00214CC4
		internal static void Sort(this IList list, IComparer comparer)
		{
			ArrayList al;
			if ((al = (list as ArrayList)) != null)
			{
				SortFieldComparer.SortHelper(al, comparer);
				return;
			}
			LiveShapingList liveShapingList;
			if ((liveShapingList = (list as LiveShapingList)) != null)
			{
				liveShapingList.Sort();
			}
		}
	}
}
