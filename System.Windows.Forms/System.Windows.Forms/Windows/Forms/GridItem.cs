using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	/// <summary>Implements one row in a <see cref="T:System.Windows.Forms.PropertyGrid" />.</summary>
	// Token: 0x02000259 RID: 601
	public abstract class GridItem
	{
		/// <summary>Gets or sets user-defined data about the <see cref="T:System.Windows.Forms.GridItem" />.</summary>
		/// <returns>An <see cref="T:System.Object" /> that contains data about the <see cref="T:System.Windows.Forms.GridItem" />.</returns>
		// Token: 0x170008B0 RID: 2224
		// (get) Token: 0x06002444 RID: 9284 RVA: 0x000B0A97 File Offset: 0x000AEC97
		// (set) Token: 0x06002445 RID: 9285 RVA: 0x000B0A9F File Offset: 0x000AEC9F
		[SRCategory("CatData")]
		[Localizable(false)]
		[Bindable(true)]
		[SRDescription("ControlTagDescr")]
		[DefaultValue(null)]
		[TypeConverter(typeof(StringConverter))]
		public object Tag
		{
			get
			{
				return this.userData;
			}
			set
			{
				this.userData = value;
			}
		}

		/// <summary>When overridden in a derived class, gets the collection of <see cref="T:System.Windows.Forms.GridItem" /> objects, if any, associated as a child of this <see cref="T:System.Windows.Forms.GridItem" />.</summary>
		/// <returns>The collection of <see cref="T:System.Windows.Forms.GridItem" /> objects.</returns>
		// Token: 0x170008B1 RID: 2225
		// (get) Token: 0x06002446 RID: 9286
		public abstract GridItemCollection GridItems { get; }

		/// <summary>When overridden in a derived class, gets the type of this <see cref="T:System.Windows.Forms.GridItem" />.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.GridItemType" /> values.</returns>
		// Token: 0x170008B2 RID: 2226
		// (get) Token: 0x06002447 RID: 9287
		public abstract GridItemType GridItemType { get; }

		/// <summary>When overridden in a derived class, gets the text of this <see cref="T:System.Windows.Forms.GridItem" />.</summary>
		/// <returns>A <see cref="T:System.String" /> representing the text associated with this <see cref="T:System.Windows.Forms.GridItem" />.</returns>
		// Token: 0x170008B3 RID: 2227
		// (get) Token: 0x06002448 RID: 9288
		public abstract string Label { get; }

		/// <summary>When overridden in a derived class, gets the parent <see cref="T:System.Windows.Forms.GridItem" /> of this <see cref="T:System.Windows.Forms.GridItem" />, if any.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.GridItem" /> representing the parent of the <see cref="T:System.Windows.Forms.GridItem" />.</returns>
		// Token: 0x170008B4 RID: 2228
		// (get) Token: 0x06002449 RID: 9289
		public abstract GridItem Parent { get; }

		/// <summary>When overridden in a derived class, gets the <see cref="T:System.ComponentModel.PropertyDescriptor" /> that is associated with this <see cref="T:System.Windows.Forms.GridItem" />.</summary>
		/// <returns>The <see cref="T:System.ComponentModel.PropertyDescriptor" /> associated with this <see cref="T:System.Windows.Forms.GridItem" />.</returns>
		// Token: 0x170008B5 RID: 2229
		// (get) Token: 0x0600244A RID: 9290
		public abstract PropertyDescriptor PropertyDescriptor { get; }

		/// <summary>When overridden in a derived class, gets the current value of this <see cref="T:System.Windows.Forms.GridItem" />.</summary>
		/// <returns>The current value of this <see cref="T:System.Windows.Forms.GridItem" />. This can be <see langword="null" />.</returns>
		// Token: 0x170008B6 RID: 2230
		// (get) Token: 0x0600244B RID: 9291
		public abstract object Value { get; }

		/// <summary>When overridden in a derived class, gets a value indicating whether the specified property is expandable to show nested properties.</summary>
		/// <returns>
		///     <see langword="true" /> if the specified property can be expanded; otherwise, <see langword="false" />. The default is false.</returns>
		// Token: 0x170008B7 RID: 2231
		// (get) Token: 0x0600244C RID: 9292 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		public virtual bool Expandable
		{
			get
			{
				return false;
			}
		}

		/// <summary>When overridden in a derived class, gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.GridItem" /> is in an expanded state.</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		/// <exception cref="T:System.NotSupportedException">The <see cref="P:System.Windows.Forms.GridItem.Expanded" /> property was set to <see langword="true" />, but a <see cref="T:System.Windows.Forms.GridItem" /> is not expandable.</exception>
		// Token: 0x170008B8 RID: 2232
		// (get) Token: 0x0600244D RID: 9293 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		// (set) Token: 0x0600244E RID: 9294 RVA: 0x000B0AA8 File Offset: 0x000AECA8
		public virtual bool Expanded
		{
			get
			{
				return false;
			}
			set
			{
				throw new NotSupportedException(SR.GetString("GridItemNotExpandable"));
			}
		}

		/// <summary>When overridden in a derived class, selects this <see cref="T:System.Windows.Forms.GridItem" /> in the <see cref="T:System.Windows.Forms.PropertyGrid" />.</summary>
		/// <returns>
		///     <see langword="true" /> if the selection is successful; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600244F RID: 9295
		public abstract bool Select();

		// Token: 0x04000F9C RID: 3996
		private object userData;
	}
}
