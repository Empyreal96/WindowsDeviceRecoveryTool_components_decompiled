using System;
using System.Runtime.InteropServices;

namespace System.Drawing.Drawing2D
{
	/// <summary>Encapsulates a custom user-defined line cap.</summary>
	// Token: 0x020000BA RID: 186
	public class CustomLineCap : MarshalByRefObject, ICloneable, IDisposable
	{
		// Token: 0x06000A46 RID: 2630 RVA: 0x000037F8 File Offset: 0x000019F8
		internal CustomLineCap()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Drawing2D.CustomLineCap" /> class with the specified outline and fill.</summary>
		/// <param name="fillPath">A <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> object that defines the fill for the custom cap. </param>
		/// <param name="strokePath">A <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> object that defines the outline of the custom cap. </param>
		// Token: 0x06000A47 RID: 2631 RVA: 0x00025ADC File Offset: 0x00023CDC
		public CustomLineCap(GraphicsPath fillPath, GraphicsPath strokePath) : this(fillPath, strokePath, LineCap.Flat)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Drawing2D.CustomLineCap" /> class from the specified existing <see cref="T:System.Drawing.Drawing2D.LineCap" /> enumeration with the specified outline and fill.</summary>
		/// <param name="fillPath">A <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> object that defines the fill for the custom cap. </param>
		/// <param name="strokePath">A <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> object that defines the outline of the custom cap. </param>
		/// <param name="baseCap">The line cap from which to create the custom cap. </param>
		// Token: 0x06000A48 RID: 2632 RVA: 0x00025AE7 File Offset: 0x00023CE7
		public CustomLineCap(GraphicsPath fillPath, GraphicsPath strokePath, LineCap baseCap) : this(fillPath, strokePath, baseCap, 0f)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Drawing2D.CustomLineCap" /> class from the specified existing <see cref="T:System.Drawing.Drawing2D.LineCap" /> enumeration with the specified outline, fill, and inset.</summary>
		/// <param name="fillPath">A <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> object that defines the fill for the custom cap. </param>
		/// <param name="strokePath">A <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> object that defines the outline of the custom cap. </param>
		/// <param name="baseCap">The line cap from which to create the custom cap. </param>
		/// <param name="baseInset">The distance between the cap and the line. </param>
		// Token: 0x06000A49 RID: 2633 RVA: 0x00025AF8 File Offset: 0x00023CF8
		public CustomLineCap(GraphicsPath fillPath, GraphicsPath strokePath, LineCap baseCap, float baseInset)
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateCustomLineCap(new HandleRef(fillPath, (fillPath == null) ? IntPtr.Zero : fillPath.nativePath), new HandleRef(strokePath, (strokePath == null) ? IntPtr.Zero : strokePath.nativePath), baseCap, baseInset, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			this.SetNativeLineCap(zero);
		}

		// Token: 0x06000A4A RID: 2634 RVA: 0x00025B59 File Offset: 0x00023D59
		internal CustomLineCap(IntPtr nativeLineCap)
		{
			this.SetNativeLineCap(nativeLineCap);
		}

		// Token: 0x06000A4B RID: 2635 RVA: 0x00025B68 File Offset: 0x00023D68
		internal void SetNativeLineCap(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException("handle");
			}
			this.nativeCap = new SafeCustomLineCapHandle(handle);
		}

		/// <summary>Releases all resources used by this <see cref="T:System.Drawing.Drawing2D.CustomLineCap" /> object.</summary>
		// Token: 0x06000A4C RID: 2636 RVA: 0x00025B8E File Offset: 0x00023D8E
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Drawing.Drawing2D.CustomLineCap" /> and optionally releases the managed resources. </summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources. </param>
		// Token: 0x06000A4D RID: 2637 RVA: 0x00025B9D File Offset: 0x00023D9D
		protected virtual void Dispose(bool disposing)
		{
			if (this.disposed)
			{
				return;
			}
			if (disposing && this.nativeCap != null)
			{
				this.nativeCap.Dispose();
			}
			this.disposed = true;
		}

		/// <summary>Allows an <see cref="T:System.Drawing.Drawing2D.CustomLineCap" /> to attempt to free resources and perform other cleanup operations before the <see cref="T:System.Drawing.Drawing2D.CustomLineCap" /> is reclaimed by garbage collection.</summary>
		// Token: 0x06000A4E RID: 2638 RVA: 0x00025BC8 File Offset: 0x00023DC8
		~CustomLineCap()
		{
			this.Dispose(false);
		}

		/// <summary>Creates an exact copy of this <see cref="T:System.Drawing.Drawing2D.CustomLineCap" />.</summary>
		/// <returns>The <see cref="T:System.Drawing.Drawing2D.CustomLineCap" /> this method creates, cast as an object.</returns>
		// Token: 0x06000A4F RID: 2639 RVA: 0x00025BF8 File Offset: 0x00023DF8
		public object Clone()
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCloneCustomLineCap(new HandleRef(this, this.nativeCap), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return CustomLineCap.CreateCustomLineCapObject(zero);
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x00025C34 File Offset: 0x00023E34
		internal static CustomLineCap CreateCustomLineCapObject(IntPtr cap)
		{
			CustomLineCapType customLineCapType = CustomLineCapType.Default;
			int num = SafeNativeMethods.Gdip.GdipGetCustomLineCapType(new HandleRef(null, cap), out customLineCapType);
			if (num != 0)
			{
				SafeNativeMethods.Gdip.GdipDeleteCustomLineCap(new HandleRef(null, cap));
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			if (customLineCapType == CustomLineCapType.Default)
			{
				return new CustomLineCap(cap);
			}
			if (customLineCapType != CustomLineCapType.AdjustableArrowCap)
			{
				SafeNativeMethods.Gdip.GdipDeleteCustomLineCap(new HandleRef(null, cap));
				throw SafeNativeMethods.Gdip.StatusException(6);
			}
			return new AdjustableArrowCap(cap);
		}

		/// <summary>Sets the caps used to start and end lines that make up this custom cap.</summary>
		/// <param name="startCap">The <see cref="T:System.Drawing.Drawing2D.LineCap" /> enumeration used at the beginning of a line within this cap. </param>
		/// <param name="endCap">The <see cref="T:System.Drawing.Drawing2D.LineCap" /> enumeration used at the end of a line within this cap. </param>
		// Token: 0x06000A51 RID: 2641 RVA: 0x00025C94 File Offset: 0x00023E94
		public void SetStrokeCaps(LineCap startCap, LineCap endCap)
		{
			int num = SafeNativeMethods.Gdip.GdipSetCustomLineCapStrokeCaps(new HandleRef(this, this.nativeCap), startCap, endCap);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Gets the caps used to start and end lines that make up this custom cap.</summary>
		/// <param name="startCap">The <see cref="T:System.Drawing.Drawing2D.LineCap" /> enumeration used at the beginning of a line within this cap. </param>
		/// <param name="endCap">The <see cref="T:System.Drawing.Drawing2D.LineCap" /> enumeration used at the end of a line within this cap. </param>
		// Token: 0x06000A52 RID: 2642 RVA: 0x00025CC4 File Offset: 0x00023EC4
		public void GetStrokeCaps(out LineCap startCap, out LineCap endCap)
		{
			int num = SafeNativeMethods.Gdip.GdipGetCustomLineCapStrokeCaps(new HandleRef(this, this.nativeCap), out startCap, out endCap);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000A53 RID: 2643 RVA: 0x00025CF4 File Offset: 0x00023EF4
		private void _SetStrokeJoin(LineJoin lineJoin)
		{
			int num = SafeNativeMethods.Gdip.GdipSetCustomLineCapStrokeJoin(new HandleRef(this, this.nativeCap), lineJoin);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x00025D24 File Offset: 0x00023F24
		private LineJoin _GetStrokeJoin()
		{
			LineJoin result;
			int num = SafeNativeMethods.Gdip.GdipGetCustomLineCapStrokeJoin(new HandleRef(this, this.nativeCap), out result);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return result;
		}

		/// <summary>Gets or sets the <see cref="T:System.Drawing.Drawing2D.LineJoin" /> enumeration that determines how lines that compose this <see cref="T:System.Drawing.Drawing2D.CustomLineCap" /> object are joined.</summary>
		/// <returns>The <see cref="T:System.Drawing.Drawing2D.LineJoin" /> enumeration this <see cref="T:System.Drawing.Drawing2D.CustomLineCap" /> object uses to join lines.</returns>
		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x06000A55 RID: 2645 RVA: 0x00025D55 File Offset: 0x00023F55
		// (set) Token: 0x06000A56 RID: 2646 RVA: 0x00025D5D File Offset: 0x00023F5D
		public LineJoin StrokeJoin
		{
			get
			{
				return this._GetStrokeJoin();
			}
			set
			{
				this._SetStrokeJoin(value);
			}
		}

		// Token: 0x06000A57 RID: 2647 RVA: 0x00025D68 File Offset: 0x00023F68
		private void _SetBaseCap(LineCap baseCap)
		{
			int num = SafeNativeMethods.Gdip.GdipSetCustomLineCapBaseCap(new HandleRef(this, this.nativeCap), baseCap);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000A58 RID: 2648 RVA: 0x00025D98 File Offset: 0x00023F98
		private LineCap _GetBaseCap()
		{
			LineCap result;
			int num = SafeNativeMethods.Gdip.GdipGetCustomLineCapBaseCap(new HandleRef(this, this.nativeCap), out result);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return result;
		}

		/// <summary>Gets or sets the <see cref="T:System.Drawing.Drawing2D.LineCap" /> enumeration on which this <see cref="T:System.Drawing.Drawing2D.CustomLineCap" /> is based.</summary>
		/// <returns>The <see cref="T:System.Drawing.Drawing2D.LineCap" /> enumeration on which this <see cref="T:System.Drawing.Drawing2D.CustomLineCap" /> is based.</returns>
		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x06000A59 RID: 2649 RVA: 0x00025DC9 File Offset: 0x00023FC9
		// (set) Token: 0x06000A5A RID: 2650 RVA: 0x00025DD1 File Offset: 0x00023FD1
		public LineCap BaseCap
		{
			get
			{
				return this._GetBaseCap();
			}
			set
			{
				this._SetBaseCap(value);
			}
		}

		// Token: 0x06000A5B RID: 2651 RVA: 0x00025DDC File Offset: 0x00023FDC
		private void _SetBaseInset(float inset)
		{
			int num = SafeNativeMethods.Gdip.GdipSetCustomLineCapBaseInset(new HandleRef(this, this.nativeCap), inset);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000A5C RID: 2652 RVA: 0x00025E0C File Offset: 0x0002400C
		private float _GetBaseInset()
		{
			float result;
			int num = SafeNativeMethods.Gdip.GdipGetCustomLineCapBaseInset(new HandleRef(this, this.nativeCap), out result);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return result;
		}

		/// <summary>Gets or sets the distance between the cap and the line.</summary>
		/// <returns>The distance between the beginning of the cap and the end of the line.</returns>
		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x06000A5D RID: 2653 RVA: 0x00025E3D File Offset: 0x0002403D
		// (set) Token: 0x06000A5E RID: 2654 RVA: 0x00025E45 File Offset: 0x00024045
		public float BaseInset
		{
			get
			{
				return this._GetBaseInset();
			}
			set
			{
				this._SetBaseInset(value);
			}
		}

		// Token: 0x06000A5F RID: 2655 RVA: 0x00025E50 File Offset: 0x00024050
		private void _SetWidthScale(float widthScale)
		{
			int num = SafeNativeMethods.Gdip.GdipSetCustomLineCapWidthScale(new HandleRef(this, this.nativeCap), widthScale);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000A60 RID: 2656 RVA: 0x00025E80 File Offset: 0x00024080
		private float _GetWidthScale()
		{
			float result;
			int num = SafeNativeMethods.Gdip.GdipGetCustomLineCapWidthScale(new HandleRef(this, this.nativeCap), out result);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return result;
		}

		/// <summary>Gets or sets the amount by which to scale this <see cref="T:System.Drawing.Drawing2D.CustomLineCap" /> Class object with respect to the width of the <see cref="T:System.Drawing.Pen" /> object.</summary>
		/// <returns>The amount by which to scale the cap.</returns>
		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x06000A61 RID: 2657 RVA: 0x00025EB1 File Offset: 0x000240B1
		// (set) Token: 0x06000A62 RID: 2658 RVA: 0x00025EB9 File Offset: 0x000240B9
		public float WidthScale
		{
			get
			{
				return this._GetWidthScale();
			}
			set
			{
				this._SetWidthScale(value);
			}
		}

		// Token: 0x0400097C RID: 2428
		internal SafeCustomLineCapHandle nativeCap;

		// Token: 0x0400097D RID: 2429
		private bool disposed;
	}
}
