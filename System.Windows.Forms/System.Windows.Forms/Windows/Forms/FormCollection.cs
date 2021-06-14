using System;
using System.Collections;

namespace System.Windows.Forms
{
	/// <summary>Represents a collection of <see cref="T:System.Windows.Forms.Form" /> objects.</summary>
	// Token: 0x02000253 RID: 595
	public class FormCollection : ReadOnlyCollectionBase
	{
		/// <summary>Gets or sets an element in the collection by the name of the associated <see cref="T:System.Windows.Forms.Form" /> object.</summary>
		/// <param name="name">The name of the <see cref="T:System.Windows.Forms.Form" />.</param>
		/// <returns>The form with the specified name.</returns>
		// Token: 0x170008AC RID: 2220
		public virtual Form this[string name]
		{
			get
			{
				if (name != null)
				{
					object collectionSyncRoot = FormCollection.CollectionSyncRoot;
					lock (collectionSyncRoot)
					{
						foreach (object obj in base.InnerList)
						{
							Form form = (Form)obj;
							if (string.Equals(form.Name, name, StringComparison.OrdinalIgnoreCase))
							{
								return form;
							}
						}
					}
				}
				return null;
			}
		}

		/// <summary>Gets or sets an element in the collection by its numeric index.</summary>
		/// <param name="index">The location of the <see cref="T:System.Windows.Forms.Form" /> within the collection.</param>
		/// <returns>The form at the specified index.</returns>
		// Token: 0x170008AD RID: 2221
		public virtual Form this[int index]
		{
			get
			{
				Form result = null;
				object collectionSyncRoot = FormCollection.CollectionSyncRoot;
				lock (collectionSyncRoot)
				{
					result = (Form)base.InnerList[index];
				}
				return result;
			}
		}

		// Token: 0x06002437 RID: 9271 RVA: 0x000B0978 File Offset: 0x000AEB78
		internal void Add(Form form)
		{
			object collectionSyncRoot = FormCollection.CollectionSyncRoot;
			lock (collectionSyncRoot)
			{
				base.InnerList.Add(form);
			}
		}

		// Token: 0x06002438 RID: 9272 RVA: 0x000B09C0 File Offset: 0x000AEBC0
		internal bool Contains(Form form)
		{
			bool result = false;
			object collectionSyncRoot = FormCollection.CollectionSyncRoot;
			lock (collectionSyncRoot)
			{
				result = base.InnerList.Contains(form);
			}
			return result;
		}

		// Token: 0x06002439 RID: 9273 RVA: 0x000B0A0C File Offset: 0x000AEC0C
		internal void Remove(Form form)
		{
			object collectionSyncRoot = FormCollection.CollectionSyncRoot;
			lock (collectionSyncRoot)
			{
				base.InnerList.Remove(form);
			}
		}

		// Token: 0x04000F8C RID: 3980
		internal static object CollectionSyncRoot = new object();
	}
}
