using System;

namespace System.Windows.Controls
{
	/// <summary>Represents the desired size of the control's header, in pixels and in logical units. This structure is used by the <see cref="T:System.Windows.Controls.Primitives.IHierarchicalVirtualizationAndScrollInfo" /> interface.</summary>
	// Token: 0x02000518 RID: 1304
	public struct HierarchicalVirtualizationHeaderDesiredSizes
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.HierarchicalVirtualizationHeaderDesiredSizes" /> class.</summary>
		/// <param name="logicalSize">The size of the header, in logical units.</param>
		/// <param name="pixelSize">The size of the header, in device-independent units (1/96th inch per unit).</param>
		// Token: 0x06005422 RID: 21538 RVA: 0x001750F4 File Offset: 0x001732F4
		public HierarchicalVirtualizationHeaderDesiredSizes(Size logicalSize, Size pixelSize)
		{
			this._logicalSize = logicalSize;
			this._pixelSize = pixelSize;
		}

		/// <summary>Gets the size of the header, in logical units.</summary>
		/// <returns>The size of the header, in logical units.</returns>
		// Token: 0x1700146C RID: 5228
		// (get) Token: 0x06005423 RID: 21539 RVA: 0x00175104 File Offset: 0x00173304
		public Size LogicalSize
		{
			get
			{
				return this._logicalSize;
			}
		}

		/// <summary>Gets the size of the header, in device-independent units (1/96th inch per unit).</summary>
		/// <returns>The size of the header, in device-independent units (1/96th inch per unit).</returns>
		// Token: 0x1700146D RID: 5229
		// (get) Token: 0x06005424 RID: 21540 RVA: 0x0017510C File Offset: 0x0017330C
		public Size PixelSize
		{
			get
			{
				return this._pixelSize;
			}
		}

		/// <summary>Returns a value that indicates whether the specified <see cref="T:System.Windows.Controls.HierarchicalVirtualizationHeaderDesiredSizes" /> objects are equal.</summary>
		/// <param name="headerDesiredSizes1">The first object to compare.</param>
		/// <param name="headerDesiredSizes2">The second object to compare.</param>
		/// <returns>
		///     <see langword="true" /> if the specified objects are equal; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005425 RID: 21541 RVA: 0x00175114 File Offset: 0x00173314
		public static bool operator ==(HierarchicalVirtualizationHeaderDesiredSizes headerDesiredSizes1, HierarchicalVirtualizationHeaderDesiredSizes headerDesiredSizes2)
		{
			return headerDesiredSizes1.LogicalSize == headerDesiredSizes2.LogicalSize && headerDesiredSizes1.PixelSize == headerDesiredSizes2.PixelSize;
		}

		/// <summary>Returns a value that indicates whether the specified <see cref="T:System.Windows.Controls.HierarchicalVirtualizationHeaderDesiredSizes" /> objects are unequal.</summary>
		/// <param name="headerDesiredSizes1">The first object to compare.</param>
		/// <param name="headerDesiredSizes2">The second object to compare.</param>
		/// <returns>
		///     <see langword="true" /> if the specified objects are unequal; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005426 RID: 21542 RVA: 0x00175140 File Offset: 0x00173340
		public static bool operator !=(HierarchicalVirtualizationHeaderDesiredSizes headerDesiredSizes1, HierarchicalVirtualizationHeaderDesiredSizes headerDesiredSizes2)
		{
			return headerDesiredSizes1.LogicalSize != headerDesiredSizes2.LogicalSize || headerDesiredSizes1.PixelSize != headerDesiredSizes2.PixelSize;
		}

		/// <summary>Returns a value that indicates whether the specified object is equal to this <see cref="T:System.Windows.Controls.HierarchicalVirtualizationHeaderDesiredSizes" /> object.</summary>
		/// <param name="oCompare">The object to compare.</param>
		/// <returns>
		///     <see langword="true" /> if the specified object is equal to this object; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005427 RID: 21543 RVA: 0x0017516C File Offset: 0x0017336C
		public override bool Equals(object oCompare)
		{
			if (oCompare is HierarchicalVirtualizationHeaderDesiredSizes)
			{
				HierarchicalVirtualizationHeaderDesiredSizes headerDesiredSizes = (HierarchicalVirtualizationHeaderDesiredSizes)oCompare;
				return this == headerDesiredSizes;
			}
			return false;
		}

		/// <summary>Returns a value that indicates whether the specified <see cref="T:System.Windows.Controls.HierarchicalVirtualizationHeaderDesiredSizes" /> object is equal to this <see cref="T:System.Windows.Controls.HierarchicalVirtualizationHeaderDesiredSizes" /> object.</summary>
		/// <param name="comparisonHeaderSizes">The object to compare.</param>
		/// <returns>
		///     <see langword="true" /> if the specified object is equal to this object; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005428 RID: 21544 RVA: 0x00175196 File Offset: 0x00173396
		public bool Equals(HierarchicalVirtualizationHeaderDesiredSizes comparisonHeaderSizes)
		{
			return this == comparisonHeaderSizes;
		}

		/// <summary>Gets a hash code for the <see cref="T:System.Windows.Controls.HierarchicalVirtualizationHeaderDesiredSizes" />.</summary>
		/// <returns>A hash code for the <see cref="T:System.Windows.Controls.HierarchicalVirtualizationHeaderDesiredSizes" />.</returns>
		// Token: 0x06005429 RID: 21545 RVA: 0x001751A4 File Offset: 0x001733A4
		public override int GetHashCode()
		{
			return this._logicalSize.GetHashCode() ^ this._pixelSize.GetHashCode();
		}

		// Token: 0x04002D84 RID: 11652
		private Size _logicalSize;

		// Token: 0x04002D85 RID: 11653
		private Size _pixelSize;
	}
}
