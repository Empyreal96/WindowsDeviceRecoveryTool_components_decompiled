using System;

namespace System.Windows.Controls
{
	/// <summary>Specifies the sizes of a control's viewport and cache. This structure is used by the <see cref="T:System.Windows.Controls.Primitives.IHierarchicalVirtualizationAndScrollInfo" /> interface.</summary>
	// Token: 0x02000517 RID: 1303
	public struct HierarchicalVirtualizationConstraints
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.HierarchicalVirtualizationConstraints" /> class.</summary>
		/// <param name="cacheLength">The size of the cache before and after the viewport.</param>
		/// <param name="cacheLengthUnit">The type of unit that is used by the <see cref="P:System.Windows.Controls.HierarchicalVirtualizationConstraints.CacheLength" /> property.</param>
		/// <param name="viewport">The size of the cache before and after the viewport.</param>
		// Token: 0x06005417 RID: 21527 RVA: 0x00174FC3 File Offset: 0x001731C3
		public HierarchicalVirtualizationConstraints(VirtualizationCacheLength cacheLength, VirtualizationCacheLengthUnit cacheLengthUnit, Rect viewport)
		{
			this._cacheLength = cacheLength;
			this._cacheLengthUnit = cacheLengthUnit;
			this._viewport = viewport;
			this._scrollGeneration = 0L;
		}

		/// <summary>Gets the size of the cache before and after the viewport.</summary>
		/// <returns>The size of the cache before and after the viewport.</returns>
		// Token: 0x17001468 RID: 5224
		// (get) Token: 0x06005418 RID: 21528 RVA: 0x00174FE2 File Offset: 0x001731E2
		public VirtualizationCacheLength CacheLength
		{
			get
			{
				return this._cacheLength;
			}
		}

		/// <summary>Gets the type of unit that is used by the <see cref="P:System.Windows.Controls.HierarchicalVirtualizationConstraints.CacheLength" /> property.</summary>
		/// <returns>The type of unit that is used by the <see cref="P:System.Windows.Controls.HierarchicalVirtualizationConstraints.CacheLength" /> property.</returns>
		// Token: 0x17001469 RID: 5225
		// (get) Token: 0x06005419 RID: 21529 RVA: 0x00174FEA File Offset: 0x001731EA
		public VirtualizationCacheLengthUnit CacheLengthUnit
		{
			get
			{
				return this._cacheLengthUnit;
			}
		}

		/// <summary>Gets the area that displays the items of the control. </summary>
		/// <returns>The area that displays the items of the control.</returns>
		// Token: 0x1700146A RID: 5226
		// (get) Token: 0x0600541A RID: 21530 RVA: 0x00174FF2 File Offset: 0x001731F2
		public Rect Viewport
		{
			get
			{
				return this._viewport;
			}
		}

		/// <summary>Returns a value that indicates whether the specified <see cref="T:System.Windows.Controls.HierarchicalVirtualizationConstraints" /> objects are equal.</summary>
		/// <param name="constraints1">The first <see cref="T:System.Windows.Controls.HierarchicalVirtualizationConstraints" /> to compare.</param>
		/// <param name="constraints2">The second <see cref="T:System.Windows.Controls.HierarchicalVirtualizationConstraints" /> to compare.</param>
		/// <returns>
		///     <see langword="true" /> if the specified <see cref="T:System.Windows.Controls.HierarchicalVirtualizationConstraints" /> objects are equal; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600541B RID: 21531 RVA: 0x00174FFA File Offset: 0x001731FA
		public static bool operator ==(HierarchicalVirtualizationConstraints constraints1, HierarchicalVirtualizationConstraints constraints2)
		{
			return constraints1.CacheLength == constraints2.CacheLength && constraints1.CacheLengthUnit == constraints2.CacheLengthUnit && constraints2.Viewport == constraints2.Viewport;
		}

		/// <summary>Returns a value that indicates whether the specified <see cref="T:System.Windows.Controls.HierarchicalVirtualizationConstraints" /> objects are unequal.</summary>
		/// <param name="constraints1">The first <see cref="T:System.Windows.Controls.HierarchicalVirtualizationConstraints" /> to compare.</param>
		/// <param name="constraints2">The second <see cref="T:System.Windows.Controls.HierarchicalVirtualizationConstraints" /> to compare.</param>
		/// <returns>
		///     <see langword="true" /> if the specified <see cref="T:System.Windows.Controls.HierarchicalVirtualizationConstraints" /> objects are unequal; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600541C RID: 21532 RVA: 0x00175036 File Offset: 0x00173236
		public static bool operator !=(HierarchicalVirtualizationConstraints constraints1, HierarchicalVirtualizationConstraints constraints2)
		{
			return constraints1.CacheLength != constraints2.CacheLength || constraints1.CacheLengthUnit != constraints2.CacheLengthUnit || constraints1.Viewport != constraints2.Viewport;
		}

		/// <summary>Returns a value that indicates whether the specified object is equal to this <see cref="T:System.Windows.Controls.HierarchicalVirtualizationConstraints" />.</summary>
		/// <param name="oCompare">The object to compare.</param>
		/// <returns>
		///     <see langword="true" /> if the specified object is equal to this <see cref="T:System.Windows.Controls.HierarchicalVirtualizationConstraints" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600541D RID: 21533 RVA: 0x00175074 File Offset: 0x00173274
		public override bool Equals(object oCompare)
		{
			if (oCompare is HierarchicalVirtualizationConstraints)
			{
				HierarchicalVirtualizationConstraints constraints = (HierarchicalVirtualizationConstraints)oCompare;
				return this == constraints;
			}
			return false;
		}

		/// <summary>Returns a value that indicates whether the specified <see cref="T:System.Windows.Controls.HierarchicalVirtualizationConstraints" /> is equal to this <see cref="T:System.Windows.Controls.HierarchicalVirtualizationConstraints" />.</summary>
		/// <param name="comparisonConstraints">The <see cref="T:System.Windows.Controls.HierarchicalVirtualizationConstraints" /> to compare.</param>
		/// <returns>
		///     <see langword="true" /> if the specified <see cref="T:System.Windows.Controls.HierarchicalVirtualizationConstraints" /> is equal to this <see cref="T:System.Windows.Controls.HierarchicalVirtualizationConstraints" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600541E RID: 21534 RVA: 0x0017509E File Offset: 0x0017329E
		public bool Equals(HierarchicalVirtualizationConstraints comparisonConstraints)
		{
			return this == comparisonConstraints;
		}

		/// <summary>Gets a hash code for this <see cref="T:System.Windows.Controls.HierarchicalVirtualizationConstraints" />.</summary>
		/// <returns>A hash code for this <see cref="T:System.Windows.Controls.HierarchicalVirtualizationConstraints" />.</returns>
		// Token: 0x0600541F RID: 21535 RVA: 0x001750AC File Offset: 0x001732AC
		public override int GetHashCode()
		{
			return this._cacheLength.GetHashCode() ^ this._cacheLengthUnit.GetHashCode() ^ this._viewport.GetHashCode();
		}

		// Token: 0x1700146B RID: 5227
		// (get) Token: 0x06005420 RID: 21536 RVA: 0x001750E3 File Offset: 0x001732E3
		// (set) Token: 0x06005421 RID: 21537 RVA: 0x001750EB File Offset: 0x001732EB
		internal long ScrollGeneration
		{
			get
			{
				return this._scrollGeneration;
			}
			set
			{
				this._scrollGeneration = value;
			}
		}

		// Token: 0x04002D80 RID: 11648
		private VirtualizationCacheLength _cacheLength;

		// Token: 0x04002D81 RID: 11649
		private VirtualizationCacheLengthUnit _cacheLengthUnit;

		// Token: 0x04002D82 RID: 11650
		private Rect _viewport;

		// Token: 0x04002D83 RID: 11651
		private long _scrollGeneration;
	}
}
