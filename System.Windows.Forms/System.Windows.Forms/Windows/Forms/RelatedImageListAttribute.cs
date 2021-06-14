using System;

namespace System.Windows.Forms
{
	/// <summary>Indicates which <see cref="T:System.Windows.Forms.ImageList" /> a property is related to.</summary>
	// Token: 0x0200032F RID: 815
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public sealed class RelatedImageListAttribute : Attribute
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.RelatedImageListAttribute" /> class. </summary>
		/// <param name="relatedImageList">The name of the <see cref="T:System.Windows.Forms.ImageList" /> the property relates to.</param>
		// Token: 0x0600326B RID: 12907 RVA: 0x000EADAC File Offset: 0x000E8FAC
		public RelatedImageListAttribute(string relatedImageList)
		{
			this.relatedImageList = relatedImageList;
		}

		/// <summary>Gets the name of the related <see cref="T:System.Windows.Forms.ImageList" /></summary>
		/// <returns>The name of the related <see cref="T:System.Windows.Forms.ImageList" /></returns>
		// Token: 0x17000C79 RID: 3193
		// (get) Token: 0x0600326C RID: 12908 RVA: 0x000EADBB File Offset: 0x000E8FBB
		public string RelatedImageList
		{
			get
			{
				return this.relatedImageList;
			}
		}

		// Token: 0x04001E49 RID: 7753
		private string relatedImageList;
	}
}
