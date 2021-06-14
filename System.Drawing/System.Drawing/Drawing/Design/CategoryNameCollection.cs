using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Drawing.Design
{
	/// <summary>Represents a collection of category name strings.</summary>
	// Token: 0x02000072 RID: 114
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class CategoryNameCollection : ReadOnlyCollectionBase
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Design.CategoryNameCollection" /> class using the specified collection.</summary>
		/// <param name="value">A <see cref="T:System.Drawing.Design.CategoryNameCollection" /> that contains the names to initialize the collection values to. </param>
		// Token: 0x0600082A RID: 2090 RVA: 0x00020CFA File Offset: 0x0001EEFA
		public CategoryNameCollection(CategoryNameCollection value)
		{
			base.InnerList.AddRange(value);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Design.CategoryNameCollection" /> class using the specified array of names.</summary>
		/// <param name="value">An array of strings that contains the names of the categories to initialize the collection values to. </param>
		// Token: 0x0600082B RID: 2091 RVA: 0x00020CFA File Offset: 0x0001EEFA
		public CategoryNameCollection(string[] value)
		{
			base.InnerList.AddRange(value);
		}

		/// <summary>Gets the category name at the specified index.</summary>
		/// <param name="index">The index of the collection element to access. </param>
		/// <returns>The category name at the specified index.</returns>
		// Token: 0x17000318 RID: 792
		public string this[int index]
		{
			get
			{
				return (string)base.InnerList[index];
			}
		}

		/// <summary>Indicates whether the specified category is contained in the collection.</summary>
		/// <param name="value">The string to check for in the collection. </param>
		/// <returns>
		///     <see langword="true" /> if the specified category is contained in the collection; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600082D RID: 2093 RVA: 0x00020D21 File Offset: 0x0001EF21
		public bool Contains(string value)
		{
			return base.InnerList.Contains(value);
		}

		/// <summary>Copies the collection elements to the specified array at the specified index.</summary>
		/// <param name="array">The array to copy to. </param>
		/// <param name="index">The index of the destination array at which to begin copying. </param>
		// Token: 0x0600082E RID: 2094 RVA: 0x00020D2F File Offset: 0x0001EF2F
		public void CopyTo(string[] array, int index)
		{
			base.InnerList.CopyTo(array, index);
		}

		/// <summary>Gets the index of the specified value.</summary>
		/// <param name="value">The category name to retrieve the index of in the collection. </param>
		/// <returns>The index in the collection, or <see langword="null" /> if the string does not exist in the collection.</returns>
		// Token: 0x0600082F RID: 2095 RVA: 0x00020D3E File Offset: 0x0001EF3E
		public int IndexOf(string value)
		{
			return base.InnerList.IndexOf(value);
		}
	}
}
