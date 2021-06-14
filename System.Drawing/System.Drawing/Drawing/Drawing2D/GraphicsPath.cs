using System;
using System.ComponentModel;
using System.Drawing.Internal;
using System.Runtime.InteropServices;

namespace System.Drawing.Drawing2D
{
	/// <summary>Represents a series of connected lines and curves. This class cannot be inherited.</summary>
	// Token: 0x020000C0 RID: 192
	public sealed class GraphicsPath : MarshalByRefObject, ICloneable, IDisposable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> class with a <see cref="P:System.Drawing.Drawing2D.GraphicsPath.FillMode" /> value of <see cref="F:System.Drawing.Drawing2D.FillMode.Alternate" />.</summary>
		// Token: 0x06000A64 RID: 2660 RVA: 0x00025ED1 File Offset: 0x000240D1
		public GraphicsPath() : this(FillMode.Alternate)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> class with the specified <see cref="T:System.Drawing.Drawing2D.FillMode" /> enumeration.</summary>
		/// <param name="fillMode">The <see cref="T:System.Drawing.Drawing2D.FillMode" /> enumeration that determines how the interior of this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> is filled. </param>
		// Token: 0x06000A65 RID: 2661 RVA: 0x00025EDC File Offset: 0x000240DC
		public GraphicsPath(FillMode fillMode)
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreatePath((int)fillMode, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			this.nativePath = zero;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> array with the specified <see cref="T:System.Drawing.Drawing2D.PathPointType" /> and <see cref="T:System.Drawing.PointF" /> arrays.</summary>
		/// <param name="pts">An array of <see cref="T:System.Drawing.PointF" /> structures that defines the coordinates of the points that make up this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />. </param>
		/// <param name="types">An array of <see cref="T:System.Drawing.Drawing2D.PathPointType" /> enumeration elements that specifies the type of each corresponding point in the <paramref name="pts" /> array. </param>
		// Token: 0x06000A66 RID: 2662 RVA: 0x00025F0F File Offset: 0x0002410F
		public GraphicsPath(PointF[] pts, byte[] types) : this(pts, types, FillMode.Alternate)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> array with the specified <see cref="T:System.Drawing.Drawing2D.PathPointType" /> and <see cref="T:System.Drawing.PointF" /> arrays and with the specified <see cref="T:System.Drawing.Drawing2D.FillMode" /> enumeration element.</summary>
		/// <param name="pts">An array of <see cref="T:System.Drawing.PointF" /> structures that defines the coordinates of the points that make up this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />. </param>
		/// <param name="types">An array of <see cref="T:System.Drawing.Drawing2D.PathPointType" /> enumeration elements that specifies the type of each corresponding point in the <paramref name="pts" /> array. </param>
		/// <param name="fillMode">A <see cref="T:System.Drawing.Drawing2D.FillMode" /> enumeration that specifies how the interiors of shapes in this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> are filled. </param>
		// Token: 0x06000A67 RID: 2663 RVA: 0x00025F1C File Offset: 0x0002411C
		public GraphicsPath(PointF[] pts, byte[] types, FillMode fillMode)
		{
			if (pts == null)
			{
				throw new ArgumentNullException("pts");
			}
			IntPtr zero = IntPtr.Zero;
			if (pts.Length != types.Length)
			{
				throw SafeNativeMethods.Gdip.StatusException(2);
			}
			int num = types.Length;
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(pts);
			IntPtr intPtr2 = Marshal.AllocHGlobal(num);
			try
			{
				Marshal.Copy(types, 0, intPtr2, num);
				int num2 = SafeNativeMethods.Gdip.GdipCreatePath2(new HandleRef(null, intPtr), new HandleRef(null, intPtr2), num, (int)fillMode, out zero);
				if (num2 != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num2);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
				Marshal.FreeHGlobal(intPtr2);
			}
			this.nativePath = zero;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> class with the specified <see cref="T:System.Drawing.Drawing2D.PathPointType" /> and <see cref="T:System.Drawing.Point" /> arrays.</summary>
		/// <param name="pts">An array of <see cref="T:System.Drawing.Point" /> structures that defines the coordinates of the points that make up this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />. </param>
		/// <param name="types">An array of <see cref="T:System.Drawing.Drawing2D.PathPointType" /> enumeration elements that specifies the type of each corresponding point in the <paramref name="pts" /> array. </param>
		// Token: 0x06000A68 RID: 2664 RVA: 0x00025FB8 File Offset: 0x000241B8
		public GraphicsPath(Point[] pts, byte[] types) : this(pts, types, FillMode.Alternate)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> class with the specified <see cref="T:System.Drawing.Drawing2D.PathPointType" /> and <see cref="T:System.Drawing.Point" /> arrays and with the specified <see cref="T:System.Drawing.Drawing2D.FillMode" /> enumeration element.</summary>
		/// <param name="pts">An array of <see cref="T:System.Drawing.Point" /> structures that defines the coordinates of the points that make up this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />. </param>
		/// <param name="types">An array of <see cref="T:System.Drawing.Drawing2D.PathPointType" /> enumeration elements that specifies the type of each corresponding point in the <paramref name="pts" /> array. </param>
		/// <param name="fillMode">A <see cref="T:System.Drawing.Drawing2D.FillMode" /> enumeration that specifies how the interiors of shapes in this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> are filled. </param>
		// Token: 0x06000A69 RID: 2665 RVA: 0x00025FC4 File Offset: 0x000241C4
		public GraphicsPath(Point[] pts, byte[] types, FillMode fillMode)
		{
			if (pts == null)
			{
				throw new ArgumentNullException("pts");
			}
			IntPtr zero = IntPtr.Zero;
			if (pts.Length != types.Length)
			{
				throw SafeNativeMethods.Gdip.StatusException(2);
			}
			int num = types.Length;
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(pts);
			IntPtr intPtr2 = Marshal.AllocHGlobal(num);
			try
			{
				Marshal.Copy(types, 0, intPtr2, num);
				int num2 = SafeNativeMethods.Gdip.GdipCreatePath2I(new HandleRef(null, intPtr), new HandleRef(null, intPtr2), num, (int)fillMode, out zero);
				if (num2 != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num2);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
				Marshal.FreeHGlobal(intPtr2);
			}
			this.nativePath = zero;
		}

		/// <summary>Creates an exact copy of this path.</summary>
		/// <returns>The <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> this method creates, cast as an object.</returns>
		// Token: 0x06000A6A RID: 2666 RVA: 0x00026060 File Offset: 0x00024260
		public object Clone()
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipClonePath(new HandleRef(this, this.nativePath), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return new GraphicsPath(zero, 0);
		}

		// Token: 0x06000A6B RID: 2667 RVA: 0x00026098 File Offset: 0x00024298
		private GraphicsPath(IntPtr nativePath, int extra)
		{
			if (nativePath == IntPtr.Zero)
			{
				throw new ArgumentNullException("nativePath");
			}
			this.nativePath = nativePath;
		}

		/// <summary>Releases all resources used by this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
		// Token: 0x06000A6C RID: 2668 RVA: 0x000260BF File Offset: 0x000242BF
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000A6D RID: 2669 RVA: 0x000260D0 File Offset: 0x000242D0
		private void Dispose(bool disposing)
		{
			if (this.nativePath != IntPtr.Zero)
			{
				try
				{
					SafeNativeMethods.Gdip.GdipDeletePath(new HandleRef(this, this.nativePath));
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsSecurityOrCriticalException(ex))
					{
						throw;
					}
				}
				finally
				{
					this.nativePath = IntPtr.Zero;
				}
			}
		}

		/// <summary>Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.</summary>
		// Token: 0x06000A6E RID: 2670 RVA: 0x00026138 File Offset: 0x00024338
		~GraphicsPath()
		{
			this.Dispose(false);
		}

		/// <summary>Empties the <see cref="P:System.Drawing.Drawing2D.GraphicsPath.PathPoints" /> and <see cref="P:System.Drawing.Drawing2D.GraphicsPath.PathTypes" /> arrays and sets the <see cref="T:System.Drawing.Drawing2D.FillMode" /> to <see cref="F:System.Drawing.Drawing2D.FillMode.Alternate" />.</summary>
		// Token: 0x06000A6F RID: 2671 RVA: 0x00026168 File Offset: 0x00024368
		public void Reset()
		{
			int num = SafeNativeMethods.Gdip.GdipResetPath(new HandleRef(this, this.nativePath));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Drawing.Drawing2D.FillMode" /> enumeration that determines how the interiors of shapes in this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> are filled.</summary>
		/// <returns>A <see cref="T:System.Drawing.Drawing2D.FillMode" /> enumeration that specifies how the interiors of shapes in this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> are filled.</returns>
		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x06000A70 RID: 2672 RVA: 0x00026194 File Offset: 0x00024394
		// (set) Token: 0x06000A71 RID: 2673 RVA: 0x000261C4 File Offset: 0x000243C4
		public FillMode FillMode
		{
			get
			{
				int result = 0;
				int num = SafeNativeMethods.Gdip.GdipGetPathFillMode(new HandleRef(this, this.nativePath), out result);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return (FillMode)result;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 1))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(FillMode));
				}
				int num = SafeNativeMethods.Gdip.GdipSetPathFillMode(new HandleRef(this, this.nativePath), (int)value);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		// Token: 0x06000A72 RID: 2674 RVA: 0x00026214 File Offset: 0x00024414
		private PathData _GetPathData()
		{
			int num = Marshal.SizeOf(typeof(GPPOINTF));
			int pointCount = this.PointCount;
			PathData pathData = new PathData();
			pathData.Types = new byte[pointCount];
			IntPtr intPtr = Marshal.AllocHGlobal(3 * IntPtr.Size);
			IntPtr intPtr2 = Marshal.AllocHGlobal(checked(num * pointCount));
			try
			{
				GCHandle gchandle = GCHandle.Alloc(pathData.Types, GCHandleType.Pinned);
				try
				{
					IntPtr intPtr3 = gchandle.AddrOfPinnedObject();
					Marshal.StructureToPtr(pointCount, intPtr, false);
					Marshal.StructureToPtr(intPtr2, (IntPtr)((long)intPtr + (long)IntPtr.Size), false);
					Marshal.StructureToPtr(intPtr3, (IntPtr)((long)intPtr + (long)(2 * IntPtr.Size)), false);
					int num2 = SafeNativeMethods.Gdip.GdipGetPathData(new HandleRef(this, this.nativePath), intPtr);
					if (num2 != 0)
					{
						throw SafeNativeMethods.Gdip.StatusException(num2);
					}
					pathData.Points = SafeNativeMethods.Gdip.ConvertGPPOINTFArrayF(intPtr2, pointCount);
				}
				finally
				{
					gchandle.Free();
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
				Marshal.FreeHGlobal(intPtr2);
			}
			return pathData;
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Drawing2D.PathData" /> that encapsulates arrays of points (<paramref name="points" />) and types (<paramref name="types" />) for this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Drawing2D.PathData" /> that encapsulates arrays for both the points and types for this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</returns>
		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x06000A73 RID: 2675 RVA: 0x00026328 File Offset: 0x00024528
		public PathData PathData
		{
			get
			{
				return this._GetPathData();
			}
		}

		/// <summary>Starts a new figure without closing the current figure. All subsequent points added to the path are added to this new figure.</summary>
		// Token: 0x06000A74 RID: 2676 RVA: 0x00026330 File Offset: 0x00024530
		public void StartFigure()
		{
			int num = SafeNativeMethods.Gdip.GdipStartPathFigure(new HandleRef(this, this.nativePath));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Closes the current figure and starts a new figure. If the current figure contains a sequence of connected lines and curves, the method closes the loop by connecting a line from the endpoint to the starting point.</summary>
		// Token: 0x06000A75 RID: 2677 RVA: 0x0002635C File Offset: 0x0002455C
		public void CloseFigure()
		{
			int num = SafeNativeMethods.Gdip.GdipClosePathFigure(new HandleRef(this, this.nativePath));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Closes all open figures in this path and starts a new figure. It closes each open figure by connecting a line from its endpoint to its starting point.</summary>
		// Token: 0x06000A76 RID: 2678 RVA: 0x00026388 File Offset: 0x00024588
		public void CloseAllFigures()
		{
			int num = SafeNativeMethods.Gdip.GdipClosePathFigures(new HandleRef(this, this.nativePath));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Sets a marker on this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
		// Token: 0x06000A77 RID: 2679 RVA: 0x000263B4 File Offset: 0x000245B4
		public void SetMarkers()
		{
			int num = SafeNativeMethods.Gdip.GdipSetPathMarker(new HandleRef(this, this.nativePath));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Clears all markers from this path.</summary>
		// Token: 0x06000A78 RID: 2680 RVA: 0x000263E0 File Offset: 0x000245E0
		public void ClearMarkers()
		{
			int num = SafeNativeMethods.Gdip.GdipClearPathMarkers(new HandleRef(this, this.nativePath));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Reverses the order of points in the <see cref="P:System.Drawing.Drawing2D.GraphicsPath.PathPoints" /> array of this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
		// Token: 0x06000A79 RID: 2681 RVA: 0x0002640C File Offset: 0x0002460C
		public void Reverse()
		{
			int num = SafeNativeMethods.Gdip.GdipReversePath(new HandleRef(this, this.nativePath));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Gets the last point in the <see cref="P:System.Drawing.Drawing2D.GraphicsPath.PathPoints" /> array of this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.PointF" /> that represents the last point in this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</returns>
		// Token: 0x06000A7A RID: 2682 RVA: 0x00026438 File Offset: 0x00024638
		public PointF GetLastPoint()
		{
			GPPOINTF gppointf = new GPPOINTF();
			int num = SafeNativeMethods.Gdip.GdipGetPathLastPoint(new HandleRef(this, this.nativePath), gppointf);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return gppointf.ToPoint();
		}

		/// <summary>Indicates whether the specified point is contained within this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
		/// <param name="x">The x-coordinate of the point to test. </param>
		/// <param name="y">The y-coordinate of the point to test. </param>
		/// <returns>This method returns <see langword="true" /> if the specified point is contained within this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000A7B RID: 2683 RVA: 0x0002646E File Offset: 0x0002466E
		public bool IsVisible(float x, float y)
		{
			return this.IsVisible(new PointF(x, y), null);
		}

		/// <summary>Indicates whether the specified point is contained within this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
		/// <param name="point">A <see cref="T:System.Drawing.PointF" /> that represents the point to test. </param>
		/// <returns>This method returns <see langword="true" /> if the specified point is contained within this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000A7C RID: 2684 RVA: 0x0002647E File Offset: 0x0002467E
		public bool IsVisible(PointF point)
		{
			return this.IsVisible(point, null);
		}

		/// <summary>Indicates whether the specified point is contained within this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> in the visible clip region of the specified <see cref="T:System.Drawing.Graphics" />.</summary>
		/// <param name="x">The x-coordinate of the point to test. </param>
		/// <param name="y">The y-coordinate of the point to test. </param>
		/// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> for which to test visibility. </param>
		/// <returns>This method returns <see langword="true" /> if the specified point is contained within this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000A7D RID: 2685 RVA: 0x00026488 File Offset: 0x00024688
		public bool IsVisible(float x, float y, Graphics graphics)
		{
			return this.IsVisible(new PointF(x, y), graphics);
		}

		/// <summary>Indicates whether the specified point is contained within this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
		/// <param name="pt">A <see cref="T:System.Drawing.PointF" /> that represents the point to test. </param>
		/// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> for which to test visibility. </param>
		/// <returns>This method returns <see langword="true" /> if the specified point is contained within this; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000A7E RID: 2686 RVA: 0x00026498 File Offset: 0x00024698
		public bool IsVisible(PointF pt, Graphics graphics)
		{
			int num2;
			int num = SafeNativeMethods.Gdip.GdipIsVisiblePathPoint(new HandleRef(this, this.nativePath), pt.X, pt.Y, new HandleRef(graphics, (graphics != null) ? graphics.NativeGraphics : IntPtr.Zero), out num2);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return num2 != 0;
		}

		/// <summary>Indicates whether the specified point is contained within this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
		/// <param name="x">The x-coordinate of the point to test. </param>
		/// <param name="y">The y-coordinate of the point to test. </param>
		/// <returns>This method returns <see langword="true" /> if the specified point is contained within this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000A7F RID: 2687 RVA: 0x000264EB File Offset: 0x000246EB
		public bool IsVisible(int x, int y)
		{
			return this.IsVisible(new Point(x, y), null);
		}

		/// <summary>Indicates whether the specified point is contained within this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
		/// <param name="point">A <see cref="T:System.Drawing.Point" /> that represents the point to test. </param>
		/// <returns>This method returns <see langword="true" /> if the specified point is contained within this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000A80 RID: 2688 RVA: 0x000264FB File Offset: 0x000246FB
		public bool IsVisible(Point point)
		{
			return this.IsVisible(point, null);
		}

		/// <summary>Indicates whether the specified point is contained within this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />, using the specified <see cref="T:System.Drawing.Graphics" />.</summary>
		/// <param name="x">The x-coordinate of the point to test. </param>
		/// <param name="y">The y-coordinate of the point to test. </param>
		/// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> for which to test visibility. </param>
		/// <returns>This method returns <see langword="true" /> if the specified point is contained within this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000A81 RID: 2689 RVA: 0x00026505 File Offset: 0x00024705
		public bool IsVisible(int x, int y, Graphics graphics)
		{
			return this.IsVisible(new Point(x, y), graphics);
		}

		/// <summary>Indicates whether the specified point is contained within this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
		/// <param name="pt">A <see cref="T:System.Drawing.Point" /> that represents the point to test. </param>
		/// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> for which to test visibility. </param>
		/// <returns>This method returns <see langword="true" /> if the specified point is contained within this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000A82 RID: 2690 RVA: 0x00026518 File Offset: 0x00024718
		public bool IsVisible(Point pt, Graphics graphics)
		{
			int num2;
			int num = SafeNativeMethods.Gdip.GdipIsVisiblePathPointI(new HandleRef(this, this.nativePath), pt.X, pt.Y, new HandleRef(graphics, (graphics != null) ? graphics.NativeGraphics : IntPtr.Zero), out num2);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return num2 != 0;
		}

		/// <summary>Indicates whether the specified point is contained within (under) the outline of this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> when drawn with the specified <see cref="T:System.Drawing.Pen" />.</summary>
		/// <param name="x">The x-coordinate of the point to test. </param>
		/// <param name="y">The y-coordinate of the point to test. </param>
		/// <param name="pen">The <see cref="T:System.Drawing.Pen" /> to test. </param>
		/// <returns>This method returns <see langword="true" /> if the specified point is contained within the outline of this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> when drawn with the specified <see cref="T:System.Drawing.Pen" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000A83 RID: 2691 RVA: 0x0002656B File Offset: 0x0002476B
		public bool IsOutlineVisible(float x, float y, Pen pen)
		{
			return this.IsOutlineVisible(new PointF(x, y), pen, null);
		}

		/// <summary>Indicates whether the specified point is contained within (under) the outline of this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> when drawn with the specified <see cref="T:System.Drawing.Pen" />.</summary>
		/// <param name="point">A <see cref="T:System.Drawing.PointF" /> that specifies the location to test. </param>
		/// <param name="pen">The <see cref="T:System.Drawing.Pen" /> to test. </param>
		/// <returns>This method returns <see langword="true" /> if the specified point is contained within the outline of this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> when drawn with the specified <see cref="T:System.Drawing.Pen" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000A84 RID: 2692 RVA: 0x0002657C File Offset: 0x0002477C
		public bool IsOutlineVisible(PointF point, Pen pen)
		{
			return this.IsOutlineVisible(point, pen, null);
		}

		/// <summary>Indicates whether the specified point is contained within (under) the outline of this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> when drawn with the specified <see cref="T:System.Drawing.Pen" /> and using the specified <see cref="T:System.Drawing.Graphics" />.</summary>
		/// <param name="x">The x-coordinate of the point to test. </param>
		/// <param name="y">The y-coordinate of the point to test. </param>
		/// <param name="pen">The <see cref="T:System.Drawing.Pen" /> to test. </param>
		/// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> for which to test visibility. </param>
		/// <returns>This method returns <see langword="true" /> if the specified point is contained within (under) the outline of this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> as drawn with the specified <see cref="T:System.Drawing.Pen" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000A85 RID: 2693 RVA: 0x00026587 File Offset: 0x00024787
		public bool IsOutlineVisible(float x, float y, Pen pen, Graphics graphics)
		{
			return this.IsOutlineVisible(new PointF(x, y), pen, graphics);
		}

		/// <summary>Indicates whether the specified point is contained within (under) the outline of this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> when drawn with the specified <see cref="T:System.Drawing.Pen" /> and using the specified <see cref="T:System.Drawing.Graphics" />.</summary>
		/// <param name="pt">A <see cref="T:System.Drawing.PointF" /> that specifies the location to test. </param>
		/// <param name="pen">The <see cref="T:System.Drawing.Pen" /> to test. </param>
		/// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> for which to test visibility. </param>
		/// <returns>This method returns <see langword="true" /> if the specified point is contained within (under) the outline of this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> as drawn with the specified <see cref="T:System.Drawing.Pen" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000A86 RID: 2694 RVA: 0x0002659C File Offset: 0x0002479C
		public bool IsOutlineVisible(PointF pt, Pen pen, Graphics graphics)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			int num2;
			int num = SafeNativeMethods.Gdip.GdipIsOutlineVisiblePathPoint(new HandleRef(this, this.nativePath), pt.X, pt.Y, new HandleRef(pen, pen.NativePen), new HandleRef(graphics, (graphics != null) ? graphics.NativeGraphics : IntPtr.Zero), out num2);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return num2 != 0;
		}

		/// <summary>Indicates whether the specified point is contained within (under) the outline of this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> when drawn with the specified <see cref="T:System.Drawing.Pen" />.</summary>
		/// <param name="x">The x-coordinate of the point to test. </param>
		/// <param name="y">The y-coordinate of the point to test. </param>
		/// <param name="pen">The <see cref="T:System.Drawing.Pen" /> to test. </param>
		/// <returns>This method returns <see langword="true" /> if the specified point is contained within the outline of this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> when drawn with the specified <see cref="T:System.Drawing.Pen" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000A87 RID: 2695 RVA: 0x00026609 File Offset: 0x00024809
		public bool IsOutlineVisible(int x, int y, Pen pen)
		{
			return this.IsOutlineVisible(new Point(x, y), pen, null);
		}

		/// <summary>Indicates whether the specified point is contained within (under) the outline of this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> when drawn with the specified <see cref="T:System.Drawing.Pen" />.</summary>
		/// <param name="point">A <see cref="T:System.Drawing.Point" /> that specifies the location to test. </param>
		/// <param name="pen">The <see cref="T:System.Drawing.Pen" /> to test. </param>
		/// <returns>This method returns <see langword="true" /> if the specified point is contained within the outline of this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> when drawn with the specified <see cref="T:System.Drawing.Pen" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000A88 RID: 2696 RVA: 0x0002661A File Offset: 0x0002481A
		public bool IsOutlineVisible(Point point, Pen pen)
		{
			return this.IsOutlineVisible(point, pen, null);
		}

		/// <summary>Indicates whether the specified point is contained within (under) the outline of this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> when drawn with the specified <see cref="T:System.Drawing.Pen" /> and using the specified <see cref="T:System.Drawing.Graphics" />.</summary>
		/// <param name="x">The x-coordinate of the point to test. </param>
		/// <param name="y">The y-coordinate of the point to test. </param>
		/// <param name="pen">The <see cref="T:System.Drawing.Pen" /> to test. </param>
		/// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> for which to test visibility. </param>
		/// <returns>This method returns <see langword="true" /> if the specified point is contained within the outline of this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> as drawn with the specified <see cref="T:System.Drawing.Pen" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000A89 RID: 2697 RVA: 0x00026625 File Offset: 0x00024825
		public bool IsOutlineVisible(int x, int y, Pen pen, Graphics graphics)
		{
			return this.IsOutlineVisible(new Point(x, y), pen, graphics);
		}

		/// <summary>Indicates whether the specified point is contained within (under) the outline of this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> when drawn with the specified <see cref="T:System.Drawing.Pen" /> and using the specified <see cref="T:System.Drawing.Graphics" />.</summary>
		/// <param name="pt">A <see cref="T:System.Drawing.Point" /> that specifies the location to test. </param>
		/// <param name="pen">The <see cref="T:System.Drawing.Pen" /> to test. </param>
		/// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> for which to test visibility. </param>
		/// <returns>This method returns <see langword="true" /> if the specified point is contained within the outline of this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> as drawn with the specified <see cref="T:System.Drawing.Pen" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000A8A RID: 2698 RVA: 0x00026638 File Offset: 0x00024838
		public bool IsOutlineVisible(Point pt, Pen pen, Graphics graphics)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			int num2;
			int num = SafeNativeMethods.Gdip.GdipIsOutlineVisiblePathPointI(new HandleRef(this, this.nativePath), pt.X, pt.Y, new HandleRef(pen, pen.NativePen), new HandleRef(graphics, (graphics != null) ? graphics.NativeGraphics : IntPtr.Zero), out num2);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return num2 != 0;
		}

		/// <summary>Appends a line segment to this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
		/// <param name="pt1">A <see cref="T:System.Drawing.PointF" /> that represents the starting point of the line. </param>
		/// <param name="pt2">A <see cref="T:System.Drawing.PointF" /> that represents the endpoint of the line. </param>
		// Token: 0x06000A8B RID: 2699 RVA: 0x000266A5 File Offset: 0x000248A5
		public void AddLine(PointF pt1, PointF pt2)
		{
			this.AddLine(pt1.X, pt1.Y, pt2.X, pt2.Y);
		}

		/// <summary>Appends a line segment to this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
		/// <param name="x1">The x-coordinate of the starting point of the line. </param>
		/// <param name="y1">The y-coordinate of the starting point of the line. </param>
		/// <param name="x2">The x-coordinate of the endpoint of the line. </param>
		/// <param name="y2">The y-coordinate of the endpoint of the line. </param>
		// Token: 0x06000A8C RID: 2700 RVA: 0x000266CC File Offset: 0x000248CC
		public void AddLine(float x1, float y1, float x2, float y2)
		{
			int num = SafeNativeMethods.Gdip.GdipAddPathLine(new HandleRef(this, this.nativePath), x1, y1, x2, y2);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Appends a series of connected line segments to the end of this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
		/// <param name="points">An array of <see cref="T:System.Drawing.PointF" /> structures that represents the points that define the line segments to add. </param>
		// Token: 0x06000A8D RID: 2701 RVA: 0x000266FC File Offset: 0x000248FC
		public void AddLines(PointF[] points)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int num = SafeNativeMethods.Gdip.GdipAddPathLine2(new HandleRef(this, this.nativePath), new HandleRef(null, intPtr), points.Length);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Appends a line segment to this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
		/// <param name="pt1">A <see cref="T:System.Drawing.Point" /> that represents the starting point of the line. </param>
		/// <param name="pt2">A <see cref="T:System.Drawing.Point" /> that represents the endpoint of the line. </param>
		// Token: 0x06000A8E RID: 2702 RVA: 0x00026760 File Offset: 0x00024960
		public void AddLine(Point pt1, Point pt2)
		{
			this.AddLine(pt1.X, pt1.Y, pt2.X, pt2.Y);
		}

		/// <summary>Appends a line segment to the current figure.</summary>
		/// <param name="x1">The x-coordinate of the starting point of the line. </param>
		/// <param name="y1">The y-coordinate of the starting point of the line. </param>
		/// <param name="x2">The x-coordinate of the endpoint of the line. </param>
		/// <param name="y2">The y-coordinate of the endpoint of the line. </param>
		// Token: 0x06000A8F RID: 2703 RVA: 0x00026784 File Offset: 0x00024984
		public void AddLine(int x1, int y1, int x2, int y2)
		{
			int num = SafeNativeMethods.Gdip.GdipAddPathLineI(new HandleRef(this, this.nativePath), x1, y1, x2, y2);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Appends a series of connected line segments to the end of this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
		/// <param name="points">An array of <see cref="T:System.Drawing.Point" /> structures that represents the points that define the line segments to add. </param>
		// Token: 0x06000A90 RID: 2704 RVA: 0x000267B4 File Offset: 0x000249B4
		public void AddLines(Point[] points)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int num = SafeNativeMethods.Gdip.GdipAddPathLine2I(new HandleRef(this, this.nativePath), new HandleRef(null, intPtr), points.Length);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Appends an elliptical arc to the current figure.</summary>
		/// <param name="rect">A <see cref="T:System.Drawing.RectangleF" /> that represents the rectangular bounds of the ellipse from which the arc is taken. </param>
		/// <param name="startAngle">The starting angle of the arc, measured in degrees clockwise from the x-axis. </param>
		/// <param name="sweepAngle">The angle between <paramref name="startAngle" /> and the end of the arc. </param>
		// Token: 0x06000A91 RID: 2705 RVA: 0x00026818 File Offset: 0x00024A18
		public void AddArc(RectangleF rect, float startAngle, float sweepAngle)
		{
			this.AddArc(rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
		}

		/// <summary>Appends an elliptical arc to the current figure.</summary>
		/// <param name="x">The x-coordinate of the upper-left corner of the rectangular region that defines the ellipse from which the arc is drawn. </param>
		/// <param name="y">The y-coordinate of the upper-left corner of the rectangular region that defines the ellipse from which the arc is drawn. </param>
		/// <param name="width">The width of the rectangular region that defines the ellipse from which the arc is drawn. </param>
		/// <param name="height">The height of the rectangular region that defines the ellipse from which the arc is drawn. </param>
		/// <param name="startAngle">The starting angle of the arc, measured in degrees clockwise from the x-axis. </param>
		/// <param name="sweepAngle">The angle between <paramref name="startAngle" /> and the end of the arc. </param>
		// Token: 0x06000A92 RID: 2706 RVA: 0x00026840 File Offset: 0x00024A40
		public void AddArc(float x, float y, float width, float height, float startAngle, float sweepAngle)
		{
			int num = SafeNativeMethods.Gdip.GdipAddPathArc(new HandleRef(this, this.nativePath), x, y, width, height, startAngle, sweepAngle);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Appends an elliptical arc to the current figure.</summary>
		/// <param name="rect">A <see cref="T:System.Drawing.Rectangle" /> that represents the rectangular bounds of the ellipse from which the arc is taken. </param>
		/// <param name="startAngle">The starting angle of the arc, measured in degrees clockwise from the x-axis. </param>
		/// <param name="sweepAngle">The angle between <paramref name="startAngle" /> and the end of the arc. </param>
		// Token: 0x06000A93 RID: 2707 RVA: 0x00026872 File Offset: 0x00024A72
		public void AddArc(Rectangle rect, float startAngle, float sweepAngle)
		{
			this.AddArc(rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
		}

		/// <summary>Appends an elliptical arc to the current figure.</summary>
		/// <param name="x">The x-coordinate of the upper-left corner of the rectangular region that defines the ellipse from which the arc is drawn. </param>
		/// <param name="y">The y-coordinate of the upper-left corner of the rectangular region that defines the ellipse from which the arc is drawn. </param>
		/// <param name="width">The width of the rectangular region that defines the ellipse from which the arc is drawn. </param>
		/// <param name="height">The height of the rectangular region that defines the ellipse from which the arc is drawn. </param>
		/// <param name="startAngle">The starting angle of the arc, measured in degrees clockwise from the x-axis. </param>
		/// <param name="sweepAngle">The angle between <paramref name="startAngle" /> and the end of the arc. </param>
		// Token: 0x06000A94 RID: 2708 RVA: 0x00026898 File Offset: 0x00024A98
		public void AddArc(int x, int y, int width, int height, float startAngle, float sweepAngle)
		{
			int num = SafeNativeMethods.Gdip.GdipAddPathArcI(new HandleRef(this, this.nativePath), x, y, width, height, startAngle, sweepAngle);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Adds a cubic Bézier curve to the current figure.</summary>
		/// <param name="pt1">A <see cref="T:System.Drawing.PointF" /> that represents the starting point of the curve. </param>
		/// <param name="pt2">A <see cref="T:System.Drawing.PointF" /> that represents the first control point for the curve. </param>
		/// <param name="pt3">A <see cref="T:System.Drawing.PointF" /> that represents the second control point for the curve. </param>
		/// <param name="pt4">A <see cref="T:System.Drawing.PointF" /> that represents the endpoint of the curve. </param>
		// Token: 0x06000A95 RID: 2709 RVA: 0x000268CC File Offset: 0x00024ACC
		public void AddBezier(PointF pt1, PointF pt2, PointF pt3, PointF pt4)
		{
			this.AddBezier(pt1.X, pt1.Y, pt2.X, pt2.Y, pt3.X, pt3.Y, pt4.X, pt4.Y);
		}

		/// <summary>Adds a cubic Bézier curve to the current figure.</summary>
		/// <param name="x1">The x-coordinate of the starting point of the curve. </param>
		/// <param name="y1">The y-coordinate of the starting point of the curve. </param>
		/// <param name="x2">The x-coordinate of the first control point for the curve. </param>
		/// <param name="y2">The y-coordinate of the first control point for the curve. </param>
		/// <param name="x3">The x-coordinate of the second control point for the curve. </param>
		/// <param name="y3">The y-coordinate of the second control point for the curve. </param>
		/// <param name="x4">The x-coordinate of the endpoint of the curve. </param>
		/// <param name="y4">The y-coordinate of the endpoint of the curve. </param>
		// Token: 0x06000A96 RID: 2710 RVA: 0x00026918 File Offset: 0x00024B18
		public void AddBezier(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
		{
			int num = SafeNativeMethods.Gdip.GdipAddPathBezier(new HandleRef(this, this.nativePath), x1, y1, x2, y2, x3, y3, x4, y4);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Adds a sequence of connected cubic Bézier curves to the current figure.</summary>
		/// <param name="points">An array of <see cref="T:System.Drawing.PointF" /> structures that represents the points that define the curves. </param>
		// Token: 0x06000A97 RID: 2711 RVA: 0x00026950 File Offset: 0x00024B50
		public void AddBeziers(PointF[] points)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int num = SafeNativeMethods.Gdip.GdipAddPathBeziers(new HandleRef(this, this.nativePath), new HandleRef(null, intPtr), points.Length);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Adds a cubic Bézier curve to the current figure.</summary>
		/// <param name="pt1">A <see cref="T:System.Drawing.Point" /> that represents the starting point of the curve. </param>
		/// <param name="pt2">A <see cref="T:System.Drawing.Point" /> that represents the first control point for the curve. </param>
		/// <param name="pt3">A <see cref="T:System.Drawing.Point" /> that represents the second control point for the curve. </param>
		/// <param name="pt4">A <see cref="T:System.Drawing.Point" /> that represents the endpoint of the curve. </param>
		// Token: 0x06000A98 RID: 2712 RVA: 0x000269B4 File Offset: 0x00024BB4
		public void AddBezier(Point pt1, Point pt2, Point pt3, Point pt4)
		{
			this.AddBezier(pt1.X, pt1.Y, pt2.X, pt2.Y, pt3.X, pt3.Y, pt4.X, pt4.Y);
		}

		/// <summary>Adds a cubic Bézier curve to the current figure.</summary>
		/// <param name="x1">The x-coordinate of the starting point of the curve. </param>
		/// <param name="y1">The y-coordinate of the starting point of the curve. </param>
		/// <param name="x2">The x-coordinate of the first control point for the curve. </param>
		/// <param name="y2">The y-coordinate of the first control point for the curve. </param>
		/// <param name="x3">The x-coordinate of the second control point for the curve. </param>
		/// <param name="y3">The y-coordinate of the second control point for the curve. </param>
		/// <param name="x4">The x-coordinate of the endpoint of the curve. </param>
		/// <param name="y4">The y-coordinate of the endpoint of the curve. </param>
		// Token: 0x06000A99 RID: 2713 RVA: 0x00026A00 File Offset: 0x00024C00
		public void AddBezier(int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4)
		{
			int num = SafeNativeMethods.Gdip.GdipAddPathBezierI(new HandleRef(this, this.nativePath), x1, y1, x2, y2, x3, y3, x4, y4);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Adds a sequence of connected cubic Bézier curves to the current figure.</summary>
		/// <param name="points">An array of <see cref="T:System.Drawing.Point" /> structures that represents the points that define the curves. </param>
		// Token: 0x06000A9A RID: 2714 RVA: 0x00026A38 File Offset: 0x00024C38
		public void AddBeziers(params Point[] points)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int num = SafeNativeMethods.Gdip.GdipAddPathBeziersI(new HandleRef(this, this.nativePath), new HandleRef(null, intPtr), points.Length);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Adds a spline curve to the current figure. A cardinal spline curve is used because the curve travels through each of the points in the array.</summary>
		/// <param name="points">An array of <see cref="T:System.Drawing.PointF" /> structures that represents the points that define the curve. </param>
		// Token: 0x06000A9B RID: 2715 RVA: 0x00026A9C File Offset: 0x00024C9C
		public void AddCurve(PointF[] points)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int num = SafeNativeMethods.Gdip.GdipAddPathCurve(new HandleRef(this, this.nativePath), new HandleRef(null, intPtr), points.Length);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Adds a spline curve to the current figure.</summary>
		/// <param name="points">An array of <see cref="T:System.Drawing.PointF" /> structures that represents the points that define the curve. </param>
		/// <param name="tension">A value that specifies the amount that the curve bends between control points. Values greater than 1 produce unpredictable results. </param>
		// Token: 0x06000A9C RID: 2716 RVA: 0x00026B00 File Offset: 0x00024D00
		public void AddCurve(PointF[] points, float tension)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int num = SafeNativeMethods.Gdip.GdipAddPathCurve2(new HandleRef(this, this.nativePath), new HandleRef(null, intPtr), points.Length, tension);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Adds a spline curve to the current figure.</summary>
		/// <param name="points">An array of <see cref="T:System.Drawing.PointF" /> structures that represents the points that define the curve. </param>
		/// <param name="offset">The index of the element in the <paramref name="points" /> array that is used as the first point in the curve. </param>
		/// <param name="numberOfSegments">The number of segments used to draw the curve. A segment can be thought of as a line connecting two points. </param>
		/// <param name="tension">A value that specifies the amount that the curve bends between control points. Values greater than 1 produce unpredictable results. </param>
		// Token: 0x06000A9D RID: 2717 RVA: 0x00026B64 File Offset: 0x00024D64
		public void AddCurve(PointF[] points, int offset, int numberOfSegments, float tension)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int num = SafeNativeMethods.Gdip.GdipAddPathCurve3(new HandleRef(this, this.nativePath), new HandleRef(null, intPtr), points.Length, offset, numberOfSegments, tension);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Adds a spline curve to the current figure. A cardinal spline curve is used because the curve travels through each of the points in the array.</summary>
		/// <param name="points">An array of <see cref="T:System.Drawing.Point" /> structures that represents the points that define the curve. </param>
		// Token: 0x06000A9E RID: 2718 RVA: 0x00026BCC File Offset: 0x00024DCC
		public void AddCurve(Point[] points)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int num = SafeNativeMethods.Gdip.GdipAddPathCurveI(new HandleRef(this, this.nativePath), new HandleRef(null, intPtr), points.Length);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Adds a spline curve to the current figure.</summary>
		/// <param name="points">An array of <see cref="T:System.Drawing.Point" /> structures that represents the points that define the curve. </param>
		/// <param name="tension">A value that specifies the amount that the curve bends between control points. Values greater than 1 produce unpredictable results. </param>
		// Token: 0x06000A9F RID: 2719 RVA: 0x00026C30 File Offset: 0x00024E30
		public void AddCurve(Point[] points, float tension)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int num = SafeNativeMethods.Gdip.GdipAddPathCurve2I(new HandleRef(this, this.nativePath), new HandleRef(null, intPtr), points.Length, tension);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Adds a spline curve to the current figure.</summary>
		/// <param name="points">An array of <see cref="T:System.Drawing.Point" /> structures that represents the points that define the curve. </param>
		/// <param name="offset">The index of the element in the <paramref name="points" /> array that is used as the first point in the curve. </param>
		/// <param name="numberOfSegments">A value that specifies the amount that the curve bends between control points. Values greater than 1 produce unpredictable results. </param>
		/// <param name="tension">A value that specifies the amount that the curve bends between control points. Values greater than 1 produce unpredictable results. </param>
		// Token: 0x06000AA0 RID: 2720 RVA: 0x00026C94 File Offset: 0x00024E94
		public void AddCurve(Point[] points, int offset, int numberOfSegments, float tension)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int num = SafeNativeMethods.Gdip.GdipAddPathCurve3I(new HandleRef(this, this.nativePath), new HandleRef(null, intPtr), points.Length, offset, numberOfSegments, tension);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Adds a closed curve to this path. A cardinal spline curve is used because the curve travels through each of the points in the array.</summary>
		/// <param name="points">An array of <see cref="T:System.Drawing.PointF" /> structures that represents the points that define the curve. </param>
		// Token: 0x06000AA1 RID: 2721 RVA: 0x00026CFC File Offset: 0x00024EFC
		public void AddClosedCurve(PointF[] points)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int num = SafeNativeMethods.Gdip.GdipAddPathClosedCurve(new HandleRef(this, this.nativePath), new HandleRef(null, intPtr), points.Length);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Adds a closed curve to this path. A cardinal spline curve is used because the curve travels through each of the points in the array.</summary>
		/// <param name="points">An array of <see cref="T:System.Drawing.PointF" /> structures that represents the points that define the curve. </param>
		/// <param name="tension">A value between from 0 through 1 that specifies the amount that the curve bends between points, with 0 being the smallest curve (sharpest corner) and 1 being the smoothest curve. </param>
		// Token: 0x06000AA2 RID: 2722 RVA: 0x00026D60 File Offset: 0x00024F60
		public void AddClosedCurve(PointF[] points, float tension)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int num = SafeNativeMethods.Gdip.GdipAddPathClosedCurve2(new HandleRef(this, this.nativePath), new HandleRef(null, intPtr), points.Length, tension);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Adds a closed curve to this path. A cardinal spline curve is used because the curve travels through each of the points in the array.</summary>
		/// <param name="points">An array of <see cref="T:System.Drawing.Point" /> structures that represents the points that define the curve. </param>
		// Token: 0x06000AA3 RID: 2723 RVA: 0x00026DC4 File Offset: 0x00024FC4
		public void AddClosedCurve(Point[] points)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int num = SafeNativeMethods.Gdip.GdipAddPathClosedCurveI(new HandleRef(this, this.nativePath), new HandleRef(null, intPtr), points.Length);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Adds a closed curve to this path. A cardinal spline curve is used because the curve travels through each of the points in the array.</summary>
		/// <param name="points">An array of <see cref="T:System.Drawing.Point" /> structures that represents the points that define the curve. </param>
		/// <param name="tension">A value between from 0 through 1 that specifies the amount that the curve bends between points, with 0 being the smallest curve (sharpest corner) and 1 being the smoothest curve. </param>
		// Token: 0x06000AA4 RID: 2724 RVA: 0x00026E28 File Offset: 0x00025028
		public void AddClosedCurve(Point[] points, float tension)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int num = SafeNativeMethods.Gdip.GdipAddPathClosedCurve2I(new HandleRef(this, this.nativePath), new HandleRef(null, intPtr), points.Length, tension);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Adds a rectangle to this path.</summary>
		/// <param name="rect">A <see cref="T:System.Drawing.RectangleF" /> that represents the rectangle to add. </param>
		// Token: 0x06000AA5 RID: 2725 RVA: 0x00026E8C File Offset: 0x0002508C
		public void AddRectangle(RectangleF rect)
		{
			int num = SafeNativeMethods.Gdip.GdipAddPathRectangle(new HandleRef(this, this.nativePath), rect.X, rect.Y, rect.Width, rect.Height);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Adds a series of rectangles to this path.</summary>
		/// <param name="rects">An array of <see cref="T:System.Drawing.RectangleF" /> structures that represents the rectangles to add. </param>
		// Token: 0x06000AA6 RID: 2726 RVA: 0x00026ED4 File Offset: 0x000250D4
		public void AddRectangles(RectangleF[] rects)
		{
			if (rects == null)
			{
				throw new ArgumentNullException("rects");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertRectangleToMemory(rects);
			try
			{
				int num = SafeNativeMethods.Gdip.GdipAddPathRectangles(new HandleRef(this, this.nativePath), new HandleRef(null, intPtr), rects.Length);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Adds a rectangle to this path.</summary>
		/// <param name="rect">A <see cref="T:System.Drawing.Rectangle" /> that represents the rectangle to add. </param>
		// Token: 0x06000AA7 RID: 2727 RVA: 0x00026F38 File Offset: 0x00025138
		public void AddRectangle(Rectangle rect)
		{
			int num = SafeNativeMethods.Gdip.GdipAddPathRectangleI(new HandleRef(this, this.nativePath), rect.X, rect.Y, rect.Width, rect.Height);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Adds a series of rectangles to this path.</summary>
		/// <param name="rects">An array of <see cref="T:System.Drawing.Rectangle" /> structures that represents the rectangles to add. </param>
		// Token: 0x06000AA8 RID: 2728 RVA: 0x00026F80 File Offset: 0x00025180
		public void AddRectangles(Rectangle[] rects)
		{
			if (rects == null)
			{
				throw new ArgumentNullException("rects");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertRectangleToMemory(rects);
			try
			{
				int num = SafeNativeMethods.Gdip.GdipAddPathRectanglesI(new HandleRef(this, this.nativePath), new HandleRef(null, intPtr), rects.Length);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Adds an ellipse to the current path.</summary>
		/// <param name="rect">A <see cref="T:System.Drawing.RectangleF" /> that represents the bounding rectangle that defines the ellipse. </param>
		// Token: 0x06000AA9 RID: 2729 RVA: 0x00026FE4 File Offset: 0x000251E4
		public void AddEllipse(RectangleF rect)
		{
			this.AddEllipse(rect.X, rect.Y, rect.Width, rect.Height);
		}

		/// <summary>Adds an ellipse to the current path.</summary>
		/// <param name="x">The x-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse. </param>
		/// <param name="y">The y-coordinate of the upper left corner of the bounding rectangle that defines the ellipse. </param>
		/// <param name="width">The width of the bounding rectangle that defines the ellipse. </param>
		/// <param name="height">The height of the bounding rectangle that defines the ellipse. </param>
		// Token: 0x06000AAA RID: 2730 RVA: 0x00027008 File Offset: 0x00025208
		public void AddEllipse(float x, float y, float width, float height)
		{
			int num = SafeNativeMethods.Gdip.GdipAddPathEllipse(new HandleRef(this, this.nativePath), x, y, width, height);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Adds an ellipse to the current path.</summary>
		/// <param name="rect">A <see cref="T:System.Drawing.Rectangle" /> that represents the bounding rectangle that defines the ellipse. </param>
		// Token: 0x06000AAB RID: 2731 RVA: 0x00027036 File Offset: 0x00025236
		public void AddEllipse(Rectangle rect)
		{
			this.AddEllipse(rect.X, rect.Y, rect.Width, rect.Height);
		}

		/// <summary>Adds an ellipse to the current path.</summary>
		/// <param name="x">The x-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse. </param>
		/// <param name="y">The y-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse. </param>
		/// <param name="width">The width of the bounding rectangle that defines the ellipse. </param>
		/// <param name="height">The height of the bounding rectangle that defines the ellipse. </param>
		// Token: 0x06000AAC RID: 2732 RVA: 0x0002705C File Offset: 0x0002525C
		public void AddEllipse(int x, int y, int width, int height)
		{
			int num = SafeNativeMethods.Gdip.GdipAddPathEllipseI(new HandleRef(this, this.nativePath), x, y, width, height);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Adds the outline of a pie shape to this path.</summary>
		/// <param name="rect">A <see cref="T:System.Drawing.Rectangle" /> that represents the bounding rectangle that defines the ellipse from which the pie is drawn. </param>
		/// <param name="startAngle">The starting angle for the pie section, measured in degrees clockwise from the x-axis. </param>
		/// <param name="sweepAngle">The angle between <paramref name="startAngle" /> and the end of the pie section, measured in degrees clockwise from <paramref name="startAngle" />. </param>
		// Token: 0x06000AAD RID: 2733 RVA: 0x0002708A File Offset: 0x0002528A
		public void AddPie(Rectangle rect, float startAngle, float sweepAngle)
		{
			this.AddPie(rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
		}

		/// <summary>Adds the outline of a pie shape to this path.</summary>
		/// <param name="x">The x-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse from which the pie is drawn. </param>
		/// <param name="y">The y-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse from which the pie is drawn. </param>
		/// <param name="width">The width of the bounding rectangle that defines the ellipse from which the pie is drawn. </param>
		/// <param name="height">The height of the bounding rectangle that defines the ellipse from which the pie is drawn. </param>
		/// <param name="startAngle">The starting angle for the pie section, measured in degrees clockwise from the x-axis. </param>
		/// <param name="sweepAngle">The angle between <paramref name="startAngle" /> and the end of the pie section, measured in degrees clockwise from <paramref name="startAngle" />. </param>
		// Token: 0x06000AAE RID: 2734 RVA: 0x000270B0 File Offset: 0x000252B0
		public void AddPie(float x, float y, float width, float height, float startAngle, float sweepAngle)
		{
			int num = SafeNativeMethods.Gdip.GdipAddPathPie(new HandleRef(this, this.nativePath), x, y, width, height, startAngle, sweepAngle);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Adds the outline of a pie shape to this path.</summary>
		/// <param name="x">The x-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse from which the pie is drawn. </param>
		/// <param name="y">The y-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse from which the pie is drawn. </param>
		/// <param name="width">The width of the bounding rectangle that defines the ellipse from which the pie is drawn. </param>
		/// <param name="height">The height of the bounding rectangle that defines the ellipse from which the pie is drawn. </param>
		/// <param name="startAngle">The starting angle for the pie section, measured in degrees clockwise from the x-axis. </param>
		/// <param name="sweepAngle">The angle between <paramref name="startAngle" /> and the end of the pie section, measured in degrees clockwise from <paramref name="startAngle" />. </param>
		// Token: 0x06000AAF RID: 2735 RVA: 0x000270E4 File Offset: 0x000252E4
		public void AddPie(int x, int y, int width, int height, float startAngle, float sweepAngle)
		{
			int num = SafeNativeMethods.Gdip.GdipAddPathPieI(new HandleRef(this, this.nativePath), x, y, width, height, startAngle, sweepAngle);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Adds a polygon to this path.</summary>
		/// <param name="points">An array of <see cref="T:System.Drawing.PointF" /> structures that defines the polygon to add. </param>
		// Token: 0x06000AB0 RID: 2736 RVA: 0x00027118 File Offset: 0x00025318
		public void AddPolygon(PointF[] points)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int num = SafeNativeMethods.Gdip.GdipAddPathPolygon(new HandleRef(this, this.nativePath), new HandleRef(null, intPtr), points.Length);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Adds a polygon to this path.</summary>
		/// <param name="points">An array of <see cref="T:System.Drawing.Point" /> structures that defines the polygon to add. </param>
		// Token: 0x06000AB1 RID: 2737 RVA: 0x0002717C File Offset: 0x0002537C
		public void AddPolygon(Point[] points)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int num = SafeNativeMethods.Gdip.GdipAddPathPolygonI(new HandleRef(this, this.nativePath), new HandleRef(null, intPtr), points.Length);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Appends the specified <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> to this path.</summary>
		/// <param name="addingPath">The <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> to add. </param>
		/// <param name="connect">A Boolean value that specifies whether the first figure in the added path is part of the last figure in this path. A value of <see langword="true" /> specifies that (if possible) the first figure in the added path is part of the last figure in this path. A value of <see langword="false" /> specifies that the first figure in the added path is separate from the last figure in this path. </param>
		// Token: 0x06000AB2 RID: 2738 RVA: 0x000271E0 File Offset: 0x000253E0
		public void AddPath(GraphicsPath addingPath, bool connect)
		{
			if (addingPath == null)
			{
				throw new ArgumentNullException("addingPath");
			}
			int num = SafeNativeMethods.Gdip.GdipAddPathPath(new HandleRef(this, this.nativePath), new HandleRef(addingPath, addingPath.nativePath), connect);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Adds a text string to this path.</summary>
		/// <param name="s">The <see cref="T:System.String" /> to add. </param>
		/// <param name="family">A <see cref="T:System.Drawing.FontFamily" /> that represents the name of the font with which the test is drawn. </param>
		/// <param name="style">A <see cref="T:System.Drawing.FontStyle" /> enumeration that represents style information about the text (bold, italic, and so on). This must be cast as an integer (see the example code later in this section). </param>
		/// <param name="emSize">The height of the em square box that bounds the character. </param>
		/// <param name="origin">A <see cref="T:System.Drawing.PointF" /> that represents the point where the text starts. </param>
		/// <param name="format">A <see cref="T:System.Drawing.StringFormat" /> that specifies text formatting information, such as line spacing and alignment. </param>
		// Token: 0x06000AB3 RID: 2739 RVA: 0x00027224 File Offset: 0x00025424
		public void AddString(string s, FontFamily family, int style, float emSize, PointF origin, StringFormat format)
		{
			GPRECTF gprectf = new GPRECTF(origin.X, origin.Y, 0f, 0f);
			int num = SafeNativeMethods.Gdip.GdipAddPathString(new HandleRef(this, this.nativePath), s, s.Length, new HandleRef(family, (family != null) ? family.NativeFamily : IntPtr.Zero), style, emSize, ref gprectf, new HandleRef(format, (format != null) ? format.nativeFormat : IntPtr.Zero));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Adds a text string to this path.</summary>
		/// <param name="s">The <see cref="T:System.String" /> to add. </param>
		/// <param name="family">A <see cref="T:System.Drawing.FontFamily" /> that represents the name of the font with which the test is drawn. </param>
		/// <param name="style">A <see cref="T:System.Drawing.FontStyle" /> enumeration that represents style information about the text (bold, italic, and so on). This must be cast as an integer (see the example code later in this section). </param>
		/// <param name="emSize">The height of the em square box that bounds the character. </param>
		/// <param name="origin">A <see cref="T:System.Drawing.Point" /> that represents the point where the text starts. </param>
		/// <param name="format">A <see cref="T:System.Drawing.StringFormat" /> that specifies text formatting information, such as line spacing and alignment. </param>
		// Token: 0x06000AB4 RID: 2740 RVA: 0x000272A8 File Offset: 0x000254A8
		public void AddString(string s, FontFamily family, int style, float emSize, Point origin, StringFormat format)
		{
			GPRECT gprect = new GPRECT(origin.X, origin.Y, 0, 0);
			int num = SafeNativeMethods.Gdip.GdipAddPathStringI(new HandleRef(this, this.nativePath), s, s.Length, new HandleRef(family, (family != null) ? family.NativeFamily : IntPtr.Zero), style, emSize, ref gprect, new HandleRef(format, (format != null) ? format.nativeFormat : IntPtr.Zero));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Adds a text string to this path.</summary>
		/// <param name="s">The <see cref="T:System.String" /> to add. </param>
		/// <param name="family">A <see cref="T:System.Drawing.FontFamily" /> that represents the name of the font with which the test is drawn. </param>
		/// <param name="style">A <see cref="T:System.Drawing.FontStyle" /> enumeration that represents style information about the text (bold, italic, and so on). This must be cast as an integer (see the example code later in this section). </param>
		/// <param name="emSize">The height of the em square box that bounds the character. </param>
		/// <param name="layoutRect">A <see cref="T:System.Drawing.RectangleF" /> that represents the rectangle that bounds the text. </param>
		/// <param name="format">A <see cref="T:System.Drawing.StringFormat" /> that specifies text formatting information, such as line spacing and alignment. </param>
		// Token: 0x06000AB5 RID: 2741 RVA: 0x00027324 File Offset: 0x00025524
		public void AddString(string s, FontFamily family, int style, float emSize, RectangleF layoutRect, StringFormat format)
		{
			GPRECTF gprectf = new GPRECTF(layoutRect);
			int num = SafeNativeMethods.Gdip.GdipAddPathString(new HandleRef(this, this.nativePath), s, s.Length, new HandleRef(family, (family != null) ? family.NativeFamily : IntPtr.Zero), style, emSize, ref gprectf, new HandleRef(format, (format != null) ? format.nativeFormat : IntPtr.Zero));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Adds a text string to this path.</summary>
		/// <param name="s">The <see cref="T:System.String" /> to add. </param>
		/// <param name="family">A <see cref="T:System.Drawing.FontFamily" /> that represents the name of the font with which the test is drawn. </param>
		/// <param name="style">A <see cref="T:System.Drawing.FontStyle" /> enumeration that represents style information about the text (bold, italic, and so on). This must be cast as an integer (see the example code later in this section). </param>
		/// <param name="emSize">The height of the em square box that bounds the character. </param>
		/// <param name="layoutRect">A <see cref="T:System.Drawing.Rectangle" /> that represents the rectangle that bounds the text. </param>
		/// <param name="format">A <see cref="T:System.Drawing.StringFormat" /> that specifies text formatting information, such as line spacing and alignment. </param>
		// Token: 0x06000AB6 RID: 2742 RVA: 0x00027394 File Offset: 0x00025594
		public void AddString(string s, FontFamily family, int style, float emSize, Rectangle layoutRect, StringFormat format)
		{
			GPRECT gprect = new GPRECT(layoutRect);
			int num = SafeNativeMethods.Gdip.GdipAddPathStringI(new HandleRef(this, this.nativePath), s, s.Length, new HandleRef(family, (family != null) ? family.NativeFamily : IntPtr.Zero), style, emSize, ref gprect, new HandleRef(format, (format != null) ? format.nativeFormat : IntPtr.Zero));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Applies a transform matrix to this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
		/// <param name="matrix">A <see cref="T:System.Drawing.Drawing2D.Matrix" /> that represents the transformation to apply. </param>
		// Token: 0x06000AB7 RID: 2743 RVA: 0x00027404 File Offset: 0x00025604
		public void Transform(Matrix matrix)
		{
			if (matrix == null)
			{
				throw new ArgumentNullException("matrix");
			}
			if (matrix.nativeMatrix == IntPtr.Zero)
			{
				return;
			}
			int num = SafeNativeMethods.Gdip.GdipTransformPath(new HandleRef(this, this.nativePath), new HandleRef(matrix, matrix.nativeMatrix));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Returns a rectangle that bounds this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.RectangleF" /> that represents a rectangle that bounds this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</returns>
		// Token: 0x06000AB8 RID: 2744 RVA: 0x0002745A File Offset: 0x0002565A
		public RectangleF GetBounds()
		{
			return this.GetBounds(null);
		}

		/// <summary>Returns a rectangle that bounds this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> when this path is transformed by the specified <see cref="T:System.Drawing.Drawing2D.Matrix" />.</summary>
		/// <param name="matrix">The <see cref="T:System.Drawing.Drawing2D.Matrix" /> that specifies a transformation to be applied to this path before the bounding rectangle is calculated. This path is not permanently transformed; the transformation is used only during the process of calculating the bounding rectangle. </param>
		/// <returns>A <see cref="T:System.Drawing.RectangleF" /> that represents a rectangle that bounds this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</returns>
		// Token: 0x06000AB9 RID: 2745 RVA: 0x00027463 File Offset: 0x00025663
		public RectangleF GetBounds(Matrix matrix)
		{
			return this.GetBounds(matrix, null);
		}

		/// <summary>Returns a rectangle that bounds this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> when the current path is transformed by the specified <see cref="T:System.Drawing.Drawing2D.Matrix" /> and drawn with the specified <see cref="T:System.Drawing.Pen" />.</summary>
		/// <param name="matrix">The <see cref="T:System.Drawing.Drawing2D.Matrix" /> that specifies a transformation to be applied to this path before the bounding rectangle is calculated. This path is not permanently transformed; the transformation is used only during the process of calculating the bounding rectangle. </param>
		/// <param name="pen">The <see cref="T:System.Drawing.Pen" /> with which to draw the <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />. </param>
		/// <returns>A <see cref="T:System.Drawing.RectangleF" /> that represents a rectangle that bounds this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</returns>
		// Token: 0x06000ABA RID: 2746 RVA: 0x00027470 File Offset: 0x00025670
		public RectangleF GetBounds(Matrix matrix, Pen pen)
		{
			GPRECTF gprectf = default(GPRECTF);
			IntPtr handle = IntPtr.Zero;
			IntPtr handle2 = IntPtr.Zero;
			if (matrix != null)
			{
				handle = matrix.nativeMatrix;
			}
			if (pen != null)
			{
				handle2 = pen.NativePen;
			}
			int num = SafeNativeMethods.Gdip.GdipGetPathWorldBounds(new HandleRef(this, this.nativePath), ref gprectf, new HandleRef(matrix, handle), new HandleRef(pen, handle2));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return gprectf.ToRectangleF();
		}

		/// <summary>Converts each curve in this path into a sequence of connected line segments.</summary>
		// Token: 0x06000ABB RID: 2747 RVA: 0x000274D8 File Offset: 0x000256D8
		public void Flatten()
		{
			this.Flatten(null);
		}

		/// <summary>Applies the specified transform and then converts each curve in this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> into a sequence of connected line segments.</summary>
		/// <param name="matrix">A <see cref="T:System.Drawing.Drawing2D.Matrix" /> by which to transform this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> before flattening. </param>
		// Token: 0x06000ABC RID: 2748 RVA: 0x000274E1 File Offset: 0x000256E1
		public void Flatten(Matrix matrix)
		{
			this.Flatten(matrix, 0.25f);
		}

		/// <summary>Converts each curve in this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> into a sequence of connected line segments.</summary>
		/// <param name="matrix">A <see cref="T:System.Drawing.Drawing2D.Matrix" /> by which to transform this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> before flattening. </param>
		/// <param name="flatness">Specifies the maximum permitted error between the curve and its flattened approximation. A value of 0.25 is the default. Reducing the flatness value will increase the number of line segments in the approximation. </param>
		// Token: 0x06000ABD RID: 2749 RVA: 0x000274F0 File Offset: 0x000256F0
		public void Flatten(Matrix matrix, float flatness)
		{
			int num = SafeNativeMethods.Gdip.GdipFlattenPath(new HandleRef(this, this.nativePath), new HandleRef(matrix, (matrix == null) ? IntPtr.Zero : matrix.nativeMatrix), flatness);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Adds an additional outline to the path.</summary>
		/// <param name="pen">A <see cref="T:System.Drawing.Pen" /> that specifies the width between the original outline of the path and the new outline this method creates. </param>
		// Token: 0x06000ABE RID: 2750 RVA: 0x00027530 File Offset: 0x00025730
		public void Widen(Pen pen)
		{
			float flatness = 0.6666667f;
			this.Widen(pen, null, flatness);
		}

		/// <summary>Adds an additional outline to the <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
		/// <param name="pen">A <see cref="T:System.Drawing.Pen" /> that specifies the width between the original outline of the path and the new outline this method creates. </param>
		/// <param name="matrix">A <see cref="T:System.Drawing.Drawing2D.Matrix" /> that specifies a transform to apply to the path before widening. </param>
		// Token: 0x06000ABF RID: 2751 RVA: 0x0002754C File Offset: 0x0002574C
		public void Widen(Pen pen, Matrix matrix)
		{
			float flatness = 0.6666667f;
			this.Widen(pen, matrix, flatness);
		}

		/// <summary>Replaces this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> with curves that enclose the area that is filled when this path is drawn by the specified pen.</summary>
		/// <param name="pen">A <see cref="T:System.Drawing.Pen" /> that specifies the width between the original outline of the path and the new outline this method creates. </param>
		/// <param name="matrix">A <see cref="T:System.Drawing.Drawing2D.Matrix" /> that specifies a transform to apply to the path before widening. </param>
		/// <param name="flatness">A value that specifies the flatness for curves. </param>
		// Token: 0x06000AC0 RID: 2752 RVA: 0x00027568 File Offset: 0x00025768
		public void Widen(Pen pen, Matrix matrix, float flatness)
		{
			IntPtr handle;
			if (matrix == null)
			{
				handle = IntPtr.Zero;
			}
			else
			{
				handle = matrix.nativeMatrix;
			}
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			int num;
			SafeNativeMethods.Gdip.GdipGetPointCount(new HandleRef(this, this.nativePath), out num);
			if (num == 0)
			{
				return;
			}
			int num2 = SafeNativeMethods.Gdip.GdipWidenPath(new HandleRef(this, this.nativePath), new HandleRef(pen, pen.NativePen), new HandleRef(matrix, handle), flatness);
			if (num2 != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num2);
			}
		}

		/// <summary>Applies a warp transform, defined by a rectangle and a parallelogram, to this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
		/// <param name="destPoints">An array of <see cref="T:System.Drawing.PointF" /> structures that define a parallelogram to which the rectangle defined by <paramref name="srcRect" /> is transformed. The array can contain either three or four elements. If the array contains three elements, the lower-right corner of the parallelogram is implied by the first three points. </param>
		/// <param name="srcRect">A <see cref="T:System.Drawing.RectangleF" /> that represents the rectangle that is transformed to the parallelogram defined by <paramref name="destPoints" />. </param>
		// Token: 0x06000AC1 RID: 2753 RVA: 0x000275DD File Offset: 0x000257DD
		public void Warp(PointF[] destPoints, RectangleF srcRect)
		{
			this.Warp(destPoints, srcRect, null);
		}

		/// <summary>Applies a warp transform, defined by a rectangle and a parallelogram, to this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
		/// <param name="destPoints">An array of <see cref="T:System.Drawing.PointF" /> structures that define a parallelogram to which the rectangle defined by <paramref name="srcRect" /> is transformed. The array can contain either three or four elements. If the array contains three elements, the lower-right corner of the parallelogram is implied by the first three points. </param>
		/// <param name="srcRect">A <see cref="T:System.Drawing.RectangleF" /> that represents the rectangle that is transformed to the parallelogram defined by <paramref name="destPoints" />. </param>
		/// <param name="matrix">A <see cref="T:System.Drawing.Drawing2D.Matrix" /> that specifies a geometric transform to apply to the path. </param>
		// Token: 0x06000AC2 RID: 2754 RVA: 0x000275E8 File Offset: 0x000257E8
		public void Warp(PointF[] destPoints, RectangleF srcRect, Matrix matrix)
		{
			this.Warp(destPoints, srcRect, matrix, WarpMode.Perspective);
		}

		/// <summary>Applies a warp transform, defined by a rectangle and a parallelogram, to this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
		/// <param name="destPoints">An array of <see cref="T:System.Drawing.PointF" /> structures that defines a parallelogram to which the rectangle defined by <paramref name="srcRect" /> is transformed. The array can contain either three or four elements. If the array contains three elements, the lower-right corner of the parallelogram is implied by the first three points. </param>
		/// <param name="srcRect">A <see cref="T:System.Drawing.RectangleF" /> that represents the rectangle that is transformed to the parallelogram defined by <paramref name="destPoints" />. </param>
		/// <param name="matrix">A <see cref="T:System.Drawing.Drawing2D.Matrix" /> that specifies a geometric transform to apply to the path. </param>
		/// <param name="warpMode">A <see cref="T:System.Drawing.Drawing2D.WarpMode" /> enumeration that specifies whether this warp operation uses perspective or bilinear mode. </param>
		// Token: 0x06000AC3 RID: 2755 RVA: 0x000275F4 File Offset: 0x000257F4
		public void Warp(PointF[] destPoints, RectangleF srcRect, Matrix matrix, WarpMode warpMode)
		{
			this.Warp(destPoints, srcRect, matrix, warpMode, 0.25f);
		}

		/// <summary>Applies a warp transform, defined by a rectangle and a parallelogram, to this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
		/// <param name="destPoints">An array of <see cref="T:System.Drawing.PointF" /> structures that define a parallelogram to which the rectangle defined by <paramref name="srcRect" /> is transformed. The array can contain either three or four elements. If the array contains three elements, the lower-right corner of the parallelogram is implied by the first three points. </param>
		/// <param name="srcRect">A <see cref="T:System.Drawing.RectangleF" /> that represents the rectangle that is transformed to the parallelogram defined by <paramref name="destPoints" />. </param>
		/// <param name="matrix">A <see cref="T:System.Drawing.Drawing2D.Matrix" /> that specifies a geometric transform to apply to the path. </param>
		/// <param name="warpMode">A <see cref="T:System.Drawing.Drawing2D.WarpMode" /> enumeration that specifies whether this warp operation uses perspective or bilinear mode. </param>
		/// <param name="flatness">A value from 0 through 1 that specifies how flat the resulting path is. For more information, see the <see cref="M:System.Drawing.Drawing2D.GraphicsPath.Flatten" /> methods. </param>
		// Token: 0x06000AC4 RID: 2756 RVA: 0x00027608 File Offset: 0x00025808
		public void Warp(PointF[] destPoints, RectangleF srcRect, Matrix matrix, WarpMode warpMode, float flatness)
		{
			if (destPoints == null)
			{
				throw new ArgumentNullException("destPoints");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(destPoints);
			try
			{
				int num = SafeNativeMethods.Gdip.GdipWarpPath(new HandleRef(this, this.nativePath), new HandleRef(matrix, (matrix == null) ? IntPtr.Zero : matrix.nativeMatrix), new HandleRef(null, intPtr), destPoints.Length, srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, warpMode, flatness);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Gets the number of elements in the <see cref="P:System.Drawing.Drawing2D.GraphicsPath.PathPoints" /> or the <see cref="P:System.Drawing.Drawing2D.GraphicsPath.PathTypes" /> array.</summary>
		/// <returns>An integer that specifies the number of elements in the <see cref="P:System.Drawing.Drawing2D.GraphicsPath.PathPoints" /> or the <see cref="P:System.Drawing.Drawing2D.GraphicsPath.PathTypes" /> array.</returns>
		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x06000AC5 RID: 2757 RVA: 0x000276A0 File Offset: 0x000258A0
		public int PointCount
		{
			get
			{
				int result = 0;
				int num = SafeNativeMethods.Gdip.GdipGetPointCount(new HandleRef(this, this.nativePath), out result);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return result;
			}
		}

		/// <summary>Gets the types of the corresponding points in the <see cref="P:System.Drawing.Drawing2D.GraphicsPath.PathPoints" /> array.</summary>
		/// <returns>An array of bytes that specifies the types of the corresponding points in the path.</returns>
		// Token: 0x170003AA RID: 938
		// (get) Token: 0x06000AC6 RID: 2758 RVA: 0x000276D0 File Offset: 0x000258D0
		public byte[] PathTypes
		{
			get
			{
				int pointCount = this.PointCount;
				byte[] array = new byte[pointCount];
				int num = SafeNativeMethods.Gdip.GdipGetPathTypes(new HandleRef(this, this.nativePath), array, pointCount);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return array;
			}
		}

		/// <summary>Gets the points in the path.</summary>
		/// <returns>An array of <see cref="T:System.Drawing.PointF" /> objects that represent the path.</returns>
		// Token: 0x170003AB RID: 939
		// (get) Token: 0x06000AC7 RID: 2759 RVA: 0x0002770C File Offset: 0x0002590C
		public PointF[] PathPoints
		{
			get
			{
				int pointCount = this.PointCount;
				int num = Marshal.SizeOf(typeof(GPPOINTF));
				IntPtr intPtr = Marshal.AllocHGlobal(checked(pointCount * num));
				PointF[] result;
				try
				{
					int num2 = SafeNativeMethods.Gdip.GdipGetPathPoints(new HandleRef(this, this.nativePath), new HandleRef(null, intPtr), pointCount);
					if (num2 != 0)
					{
						throw SafeNativeMethods.Gdip.StatusException(num2);
					}
					PointF[] array = SafeNativeMethods.Gdip.ConvertGPPOINTFArrayF(intPtr, pointCount);
					result = array;
				}
				finally
				{
					Marshal.FreeHGlobal(intPtr);
				}
				return result;
			}
		}

		// Token: 0x0400098F RID: 2447
		internal IntPtr nativePath;
	}
}
