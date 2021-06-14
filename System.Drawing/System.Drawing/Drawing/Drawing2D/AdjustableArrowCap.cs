using System;
using System.Runtime.InteropServices;

namespace System.Drawing.Drawing2D
{
	/// <summary>Represents an adjustable arrow-shaped line cap. This class cannot be inherited.</summary>
	// Token: 0x020000B2 RID: 178
	public sealed class AdjustableArrowCap : CustomLineCap
	{
		// Token: 0x06000A27 RID: 2599 RVA: 0x000257FB File Offset: 0x000239FB
		internal AdjustableArrowCap(IntPtr nativeCap) : base(nativeCap)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Drawing2D.AdjustableArrowCap" /> class with the specified width and height. The arrow end caps created with this constructor are always filled.</summary>
		/// <param name="width">The width of the arrow. </param>
		/// <param name="height">The height of the arrow. </param>
		// Token: 0x06000A28 RID: 2600 RVA: 0x00025804 File Offset: 0x00023A04
		public AdjustableArrowCap(float width, float height) : this(width, height, true)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Drawing2D.AdjustableArrowCap" /> class with the specified width, height, and fill property. Whether an arrow end cap is filled depends on the argument passed to the <paramref name="isFilled" /> parameter.</summary>
		/// <param name="width">The width of the arrow. </param>
		/// <param name="height">The height of the arrow. </param>
		/// <param name="isFilled">
		///       <see langword="true" /> to fill the arrow cap; otherwise, <see langword="false" />. </param>
		// Token: 0x06000A29 RID: 2601 RVA: 0x00025810 File Offset: 0x00023A10
		public AdjustableArrowCap(float width, float height, bool isFilled)
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateAdjustableArrowCap(height, width, isFilled, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeLineCap(zero);
		}

		// Token: 0x06000A2A RID: 2602 RVA: 0x00025848 File Offset: 0x00023A48
		private void _SetHeight(float height)
		{
			int num = SafeNativeMethods.Gdip.GdipSetAdjustableArrowCapHeight(new HandleRef(this, this.nativeCap), height);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000A2B RID: 2603 RVA: 0x00025878 File Offset: 0x00023A78
		private float _GetHeight()
		{
			float result;
			int num = SafeNativeMethods.Gdip.GdipGetAdjustableArrowCapHeight(new HandleRef(this, this.nativeCap), out result);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return result;
		}

		/// <summary>Gets or sets the height of the arrow cap.</summary>
		/// <returns>The height of the arrow cap.</returns>
		// Token: 0x1700039B RID: 923
		// (get) Token: 0x06000A2C RID: 2604 RVA: 0x000258A9 File Offset: 0x00023AA9
		// (set) Token: 0x06000A2D RID: 2605 RVA: 0x000258B1 File Offset: 0x00023AB1
		public float Height
		{
			get
			{
				return this._GetHeight();
			}
			set
			{
				this._SetHeight(value);
			}
		}

		// Token: 0x06000A2E RID: 2606 RVA: 0x000258BC File Offset: 0x00023ABC
		private void _SetWidth(float width)
		{
			int num = SafeNativeMethods.Gdip.GdipSetAdjustableArrowCapWidth(new HandleRef(this, this.nativeCap), width);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000A2F RID: 2607 RVA: 0x000258EC File Offset: 0x00023AEC
		private float _GetWidth()
		{
			float result;
			int num = SafeNativeMethods.Gdip.GdipGetAdjustableArrowCapWidth(new HandleRef(this, this.nativeCap), out result);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return result;
		}

		/// <summary>Gets or sets the width of the arrow cap.</summary>
		/// <returns>The width, in units, of the arrow cap.</returns>
		// Token: 0x1700039C RID: 924
		// (get) Token: 0x06000A30 RID: 2608 RVA: 0x0002591D File Offset: 0x00023B1D
		// (set) Token: 0x06000A31 RID: 2609 RVA: 0x00025925 File Offset: 0x00023B25
		public float Width
		{
			get
			{
				return this._GetWidth();
			}
			set
			{
				this._SetWidth(value);
			}
		}

		// Token: 0x06000A32 RID: 2610 RVA: 0x00025930 File Offset: 0x00023B30
		private void _SetMiddleInset(float middleInset)
		{
			int num = SafeNativeMethods.Gdip.GdipSetAdjustableArrowCapMiddleInset(new HandleRef(this, this.nativeCap), middleInset);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000A33 RID: 2611 RVA: 0x00025960 File Offset: 0x00023B60
		private float _GetMiddleInset()
		{
			float result;
			int num = SafeNativeMethods.Gdip.GdipGetAdjustableArrowCapMiddleInset(new HandleRef(this, this.nativeCap), out result);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return result;
		}

		/// <summary>Gets or sets the number of units between the outline of the arrow cap and the fill.</summary>
		/// <returns>The number of units between the outline of the arrow cap and the fill of the arrow cap.</returns>
		// Token: 0x1700039D RID: 925
		// (get) Token: 0x06000A34 RID: 2612 RVA: 0x00025991 File Offset: 0x00023B91
		// (set) Token: 0x06000A35 RID: 2613 RVA: 0x00025999 File Offset: 0x00023B99
		public float MiddleInset
		{
			get
			{
				return this._GetMiddleInset();
			}
			set
			{
				this._SetMiddleInset(value);
			}
		}

		// Token: 0x06000A36 RID: 2614 RVA: 0x000259A4 File Offset: 0x00023BA4
		private void _SetFillState(bool isFilled)
		{
			int num = SafeNativeMethods.Gdip.GdipSetAdjustableArrowCapFillState(new HandleRef(this, this.nativeCap), isFilled);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000A37 RID: 2615 RVA: 0x000259D4 File Offset: 0x00023BD4
		private bool _IsFilled()
		{
			bool result = false;
			int num = SafeNativeMethods.Gdip.GdipGetAdjustableArrowCapFillState(new HandleRef(this, this.nativeCap), out result);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return result;
		}

		/// <summary>Gets or sets whether the arrow cap is filled.</summary>
		/// <returns>This property is <see langword="true" /> if the arrow cap is filled; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700039E RID: 926
		// (get) Token: 0x06000A38 RID: 2616 RVA: 0x00025A07 File Offset: 0x00023C07
		// (set) Token: 0x06000A39 RID: 2617 RVA: 0x00025A0F File Offset: 0x00023C0F
		public bool Filled
		{
			get
			{
				return this._IsFilled();
			}
			set
			{
				this._SetFillState(value);
			}
		}
	}
}
