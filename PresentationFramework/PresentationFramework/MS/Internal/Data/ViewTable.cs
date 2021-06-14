using System;
using System.Collections;
using System.Collections.Specialized;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x02000745 RID: 1861
	internal class ViewTable : HybridDictionary
	{
		// Token: 0x17001C42 RID: 7234
		internal ViewRecord this[CollectionViewSource cvs]
		{
			get
			{
				return (ViewRecord)base[new WeakRefKey(cvs)];
			}
			set
			{
				base[new WeakRefKey(cvs)] = value;
			}
		}

		// Token: 0x060076EA RID: 30442 RVA: 0x0021FBAC File Offset: 0x0021DDAC
		internal bool Purge()
		{
			ArrayList arrayList = new ArrayList();
			foreach (object obj in this)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				WeakRefKey weakRefKey = (WeakRefKey)dictionaryEntry.Key;
				if (weakRefKey.Target == null)
				{
					ViewRecord viewRecord = (ViewRecord)dictionaryEntry.Value;
					CollectionView collectionView = viewRecord.View as CollectionView;
					if (collectionView != null)
					{
						if (!collectionView.IsInUse)
						{
							collectionView.DetachFromSourceCollection();
							arrayList.Add(weakRefKey);
						}
					}
					else
					{
						arrayList.Add(weakRefKey);
					}
				}
			}
			for (int i = 0; i < arrayList.Count; i++)
			{
				base.Remove(arrayList[i]);
			}
			return arrayList.Count > 0 || base.Count == 0;
		}
	}
}
