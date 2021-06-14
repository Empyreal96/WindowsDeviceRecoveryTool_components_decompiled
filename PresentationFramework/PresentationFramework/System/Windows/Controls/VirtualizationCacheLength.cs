using System;
using System.ComponentModel;
using System.Globalization;
using MS.Internal;

namespace System.Windows.Controls
{
	/// <summary>Represents the measurements for the <see cref="P:System.Windows.Controls.VirtualizingPanel.CacheLength" /> attached property.</summary>
	// Token: 0x02000514 RID: 1300
	[TypeConverter(typeof(VirtualizationCacheLengthConverter))]
	public struct VirtualizationCacheLength : IEquatable<VirtualizationCacheLength>
	{
		/// <summary>Creates a new instance of the <see cref="T:System.Windows.Controls.VirtualizationCacheLength" /> class with a uniform cache length for each side of the viewport.</summary>
		/// <param name="cacheBeforeAndAfterViewport">The size of the cache before and after the viewport.</param>
		// Token: 0x06005405 RID: 21509 RVA: 0x00174C5B File Offset: 0x00172E5B
		public VirtualizationCacheLength(double cacheBeforeAndAfterViewport)
		{
			this = new VirtualizationCacheLength(cacheBeforeAndAfterViewport, cacheBeforeAndAfterViewport);
		}

		/// <summary>Creates a new instance of the <see cref="T:System.Windows.Controls.VirtualizationCacheLength" /> class with the specified cache lengths for each side of the viewport.</summary>
		/// <param name="cacheBeforeViewport">The size of the cache before the viewport.</param>
		/// <param name="cacheAfterViewport">The size of the cache after the viewport.</param>
		// Token: 0x06005406 RID: 21510 RVA: 0x00174C68 File Offset: 0x00172E68
		public VirtualizationCacheLength(double cacheBeforeViewport, double cacheAfterViewport)
		{
			if (DoubleUtil.IsNaN(cacheBeforeViewport))
			{
				throw new ArgumentException(SR.Get("InvalidCtorParameterNoNaN", new object[]
				{
					"cacheBeforeViewport"
				}));
			}
			if (DoubleUtil.IsNaN(cacheAfterViewport))
			{
				throw new ArgumentException(SR.Get("InvalidCtorParameterNoNaN", new object[]
				{
					"cacheAfterViewport"
				}));
			}
			this._cacheBeforeViewport = cacheBeforeViewport;
			this._cacheAfterViewport = cacheAfterViewport;
		}

		/// <summary>Determines whether the two specified <see cref="T:System.Windows.Controls.VirtualizationCacheLength" /> objects are equal.</summary>
		/// <param name="cl1">The first object to compare.</param>
		/// <param name="cl2">The second object to compare.</param>
		/// <returns>
		///     <see langword="true" /> if the specified <see cref="T:System.Windows.Controls.VirtualizationCacheLength" /> are equal; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005407 RID: 21511 RVA: 0x00174CCF File Offset: 0x00172ECF
		public static bool operator ==(VirtualizationCacheLength cl1, VirtualizationCacheLength cl2)
		{
			return cl1.CacheBeforeViewport == cl2.CacheBeforeViewport && cl1.CacheAfterViewport == cl2.CacheAfterViewport;
		}

		/// <summary>Determines whether the two specified <see cref="T:System.Windows.Controls.VirtualizationCacheLength" /> objects are equal.</summary>
		/// <param name="cl1">The first object to compare.</param>
		/// <param name="cl2">The second object to compare.</param>
		/// <returns>
		///     <see langword="true" /> if the specified <see cref="T:System.Windows.Controls.VirtualizationCacheLength" /> are equal; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005408 RID: 21512 RVA: 0x00174CF3 File Offset: 0x00172EF3
		public static bool operator !=(VirtualizationCacheLength cl1, VirtualizationCacheLength cl2)
		{
			return cl1.CacheBeforeViewport != cl2.CacheBeforeViewport || cl1.CacheAfterViewport != cl2.CacheAfterViewport;
		}

		/// <summary>Determines whether the specified object is equal to the current <see cref="T:System.Windows.Controls.VirtualizationCacheLength" />.</summary>
		/// <param name="oCompare">The object to compare with the current <see cref="T:System.Windows.Controls.VirtualizationCacheLength" />.</param>
		/// <returns>
		///     <see langword="true" /> if the specified object is equal to the current <see cref="T:System.Windows.Controls.VirtualizationCacheLength" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005409 RID: 21513 RVA: 0x00174D1C File Offset: 0x00172F1C
		public override bool Equals(object oCompare)
		{
			if (oCompare is VirtualizationCacheLength)
			{
				VirtualizationCacheLength cl = (VirtualizationCacheLength)oCompare;
				return this == cl;
			}
			return false;
		}

		/// <summary>Determines whether the specified <see cref="T:System.Windows.Controls.VirtualizationCacheLength" /> is equal to the current <see cref="T:System.Windows.Controls.VirtualizationCacheLength" />.</summary>
		/// <param name="cacheLength">The object to compare with the current <see cref="T:System.Windows.Controls.VirtualizationCacheLength" />. </param>
		/// <returns>
		///     <see langword="true" /> if the specified object is equal to the current <see cref="T:System.Windows.Controls.VirtualizationCacheLength" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600540A RID: 21514 RVA: 0x00174D46 File Offset: 0x00172F46
		public bool Equals(VirtualizationCacheLength cacheLength)
		{
			return this == cacheLength;
		}

		/// <summary>Gets a hash code for the current <see cref="T:System.Windows.Controls.VirtualizationCacheLength" />.</summary>
		/// <returns>A hash code for the current <see cref="T:System.Windows.Controls.VirtualizationCacheLength" />.</returns>
		// Token: 0x0600540B RID: 21515 RVA: 0x00174D54 File Offset: 0x00172F54
		public override int GetHashCode()
		{
			return (int)this._cacheBeforeViewport + (int)this._cacheAfterViewport;
		}

		/// <summary>Gets the size of the cache after the viewport when the <see cref="T:System.Windows.Controls.VirtualizingPanel" /> is virtualizing.</summary>
		/// <returns>The size of the cache after the viewport when the <see cref="T:System.Windows.Controls.VirtualizingPanel" /> is virtualizing.</returns>
		// Token: 0x17001466 RID: 5222
		// (get) Token: 0x0600540C RID: 21516 RVA: 0x00174D65 File Offset: 0x00172F65
		public double CacheBeforeViewport
		{
			get
			{
				return this._cacheBeforeViewport;
			}
		}

		/// <summary>Gets the size of the cache before the viewport when the <see cref="T:System.Windows.Controls.VirtualizingPanel" /> is virtualizing.</summary>
		/// <returns>The size of the cache after the viewport when the <see cref="T:System.Windows.Controls.VirtualizingPanel" /> is virtualizing.</returns>
		// Token: 0x17001467 RID: 5223
		// (get) Token: 0x0600540D RID: 21517 RVA: 0x00174D6D File Offset: 0x00172F6D
		public double CacheAfterViewport
		{
			get
			{
				return this._cacheAfterViewport;
			}
		}

		/// <summary>Returns a string that represents the current <see cref="T:System.Windows.Controls.VirtualizationCacheLength" />.</summary>
		/// <returns>A string that represents the current <see cref="T:System.Windows.Controls.VirtualizationCacheLength" />.</returns>
		// Token: 0x0600540E RID: 21518 RVA: 0x00174D75 File Offset: 0x00172F75
		public override string ToString()
		{
			return VirtualizationCacheLengthConverter.ToString(this, CultureInfo.InvariantCulture);
		}

		// Token: 0x04002D21 RID: 11553
		private double _cacheBeforeViewport;

		// Token: 0x04002D22 RID: 11554
		private double _cacheAfterViewport;
	}
}
