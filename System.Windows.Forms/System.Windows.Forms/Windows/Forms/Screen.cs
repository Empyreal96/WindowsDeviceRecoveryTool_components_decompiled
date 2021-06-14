using System;
using System.Collections;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32;

namespace System.Windows.Forms
{
	/// <summary>Represents a display device or multiple display devices on a single system.</summary>
	// Token: 0x02000342 RID: 834
	public class Screen
	{
		// Token: 0x0600342C RID: 13356 RVA: 0x000EF30B File Offset: 0x000ED50B
		internal Screen(IntPtr monitor) : this(monitor, IntPtr.Zero)
		{
		}

		// Token: 0x0600342D RID: 13357 RVA: 0x000EF31C File Offset: 0x000ED51C
		internal Screen(IntPtr monitor, IntPtr hdc)
		{
			IntPtr intPtr = hdc;
			if (!Screen.multiMonitorSupport || monitor == (IntPtr)(-1163005939))
			{
				this.bounds = SystemInformation.VirtualScreen;
				this.primary = true;
				this.deviceName = "DISPLAY";
			}
			else
			{
				NativeMethods.MONITORINFOEX monitorinfoex = new NativeMethods.MONITORINFOEX();
				SafeNativeMethods.GetMonitorInfo(new HandleRef(null, monitor), monitorinfoex);
				this.bounds = Rectangle.FromLTRB(monitorinfoex.rcMonitor.left, monitorinfoex.rcMonitor.top, monitorinfoex.rcMonitor.right, monitorinfoex.rcMonitor.bottom);
				this.primary = ((monitorinfoex.dwFlags & 1) != 0);
				this.deviceName = new string(monitorinfoex.szDevice);
				this.deviceName = this.deviceName.TrimEnd(new char[1]);
				if (hdc == IntPtr.Zero)
				{
					intPtr = UnsafeNativeMethods.CreateDC(this.deviceName);
				}
			}
			this.hmonitor = monitor;
			this.bitDepth = UnsafeNativeMethods.GetDeviceCaps(new HandleRef(null, intPtr), 12);
			this.bitDepth *= UnsafeNativeMethods.GetDeviceCaps(new HandleRef(null, intPtr), 14);
			if (hdc != intPtr)
			{
				UnsafeNativeMethods.DeleteDC(new HandleRef(null, intPtr));
			}
		}

		/// <summary>Gets an array of all displays on the system.</summary>
		/// <returns>An array of type <see cref="T:System.Windows.Forms.Screen" />, containing all displays on the system.</returns>
		// Token: 0x17000CB4 RID: 3252
		// (get) Token: 0x0600342E RID: 13358 RVA: 0x000EF468 File Offset: 0x000ED668
		public static Screen[] AllScreens
		{
			get
			{
				if (Screen.screens == null)
				{
					if (Screen.multiMonitorSupport)
					{
						Screen.MonitorEnumCallback monitorEnumCallback = new Screen.MonitorEnumCallback();
						NativeMethods.MonitorEnumProc lpfnEnum = new NativeMethods.MonitorEnumProc(monitorEnumCallback.Callback);
						SafeNativeMethods.EnumDisplayMonitors(NativeMethods.NullHandleRef, null, lpfnEnum, IntPtr.Zero);
						if (monitorEnumCallback.screens.Count > 0)
						{
							Screen[] array = new Screen[monitorEnumCallback.screens.Count];
							monitorEnumCallback.screens.CopyTo(array, 0);
							Screen.screens = array;
						}
						else
						{
							Screen.screens = new Screen[]
							{
								new Screen((IntPtr)(-1163005939))
							};
						}
					}
					else
					{
						Screen.screens = new Screen[]
						{
							Screen.PrimaryScreen
						};
					}
					SystemEvents.DisplaySettingsChanging += Screen.OnDisplaySettingsChanging;
				}
				return Screen.screens;
			}
		}

		/// <summary>Gets the number of bits of memory, associated with one pixel of data.</summary>
		/// <returns>The number of bits of memory, associated with one pixel of data </returns>
		// Token: 0x17000CB5 RID: 3253
		// (get) Token: 0x0600342F RID: 13359 RVA: 0x000EF528 File Offset: 0x000ED728
		public int BitsPerPixel
		{
			get
			{
				return this.bitDepth;
			}
		}

		/// <summary>Gets the bounds of the display.</summary>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" />, representing the bounds of the display.</returns>
		// Token: 0x17000CB6 RID: 3254
		// (get) Token: 0x06003430 RID: 13360 RVA: 0x000EF530 File Offset: 0x000ED730
		public Rectangle Bounds
		{
			get
			{
				return this.bounds;
			}
		}

		/// <summary>Gets the device name associated with a display.</summary>
		/// <returns>The device name associated with a display.</returns>
		// Token: 0x17000CB7 RID: 3255
		// (get) Token: 0x06003431 RID: 13361 RVA: 0x000EF538 File Offset: 0x000ED738
		public string DeviceName
		{
			get
			{
				return this.deviceName;
			}
		}

		/// <summary>Gets a value indicating whether a particular display is the primary device.</summary>
		/// <returns>
		///     <see langword="true" /> if this display is primary; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000CB8 RID: 3256
		// (get) Token: 0x06003432 RID: 13362 RVA: 0x000EF540 File Offset: 0x000ED740
		public bool Primary
		{
			get
			{
				return this.primary;
			}
		}

		/// <summary>Gets the primary display.</summary>
		/// <returns>The primary display.</returns>
		// Token: 0x17000CB9 RID: 3257
		// (get) Token: 0x06003433 RID: 13363 RVA: 0x000EF548 File Offset: 0x000ED748
		public static Screen PrimaryScreen
		{
			get
			{
				if (Screen.multiMonitorSupport)
				{
					Screen[] allScreens = Screen.AllScreens;
					for (int i = 0; i < allScreens.Length; i++)
					{
						if (allScreens[i].primary)
						{
							return allScreens[i];
						}
					}
					return null;
				}
				return new Screen((IntPtr)(-1163005939), IntPtr.Zero);
			}
		}

		/// <summary>Gets the working area of the display. The working area is the desktop area of the display, excluding taskbars, docked windows, and docked tool bars.</summary>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" />, representing the working area of the display.</returns>
		// Token: 0x17000CBA RID: 3258
		// (get) Token: 0x06003434 RID: 13364 RVA: 0x000EF594 File Offset: 0x000ED794
		public Rectangle WorkingArea
		{
			get
			{
				if (this.currentDesktopChangedCount != Screen.DesktopChangedCount)
				{
					Interlocked.Exchange(ref this.currentDesktopChangedCount, Screen.DesktopChangedCount);
					if (!Screen.multiMonitorSupport || this.hmonitor == (IntPtr)(-1163005939))
					{
						this.workingArea = SystemInformation.WorkingArea;
					}
					else
					{
						NativeMethods.MONITORINFOEX monitorinfoex = new NativeMethods.MONITORINFOEX();
						SafeNativeMethods.GetMonitorInfo(new HandleRef(null, this.hmonitor), monitorinfoex);
						this.workingArea = Rectangle.FromLTRB(monitorinfoex.rcWork.left, monitorinfoex.rcWork.top, monitorinfoex.rcWork.right, monitorinfoex.rcWork.bottom);
					}
				}
				return this.workingArea;
			}
		}

		// Token: 0x17000CBB RID: 3259
		// (get) Token: 0x06003435 RID: 13365 RVA: 0x000EF644 File Offset: 0x000ED844
		private static int DesktopChangedCount
		{
			get
			{
				if (Screen.desktopChangedCount == -1)
				{
					object obj = Screen.syncLock;
					lock (obj)
					{
						if (Screen.desktopChangedCount == -1)
						{
							SystemEvents.UserPreferenceChanged += Screen.OnUserPreferenceChanged;
							Screen.desktopChangedCount = 0;
						}
					}
				}
				return Screen.desktopChangedCount;
			}
		}

		/// <summary>Gets or sets a value indicating whether the specified object is equal to this <see langword="Screen" />.</summary>
		/// <param name="obj">The object to compare to this <see cref="T:System.Windows.Forms.Screen" />. </param>
		/// <returns>
		///     <see langword="true" /> if the specified object is equal to this <see cref="T:System.Windows.Forms.Screen" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06003436 RID: 13366 RVA: 0x000EF6AC File Offset: 0x000ED8AC
		public override bool Equals(object obj)
		{
			if (obj is Screen)
			{
				Screen screen = (Screen)obj;
				if (this.hmonitor == screen.hmonitor)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>Retrieves a <see cref="T:System.Windows.Forms.Screen" /> for the display that contains the specified point.</summary>
		/// <param name="point">A <see cref="T:System.Drawing.Point" /> that specifies the location for which to retrieve a <see cref="T:System.Windows.Forms.Screen" />. </param>
		/// <returns>A <see cref="T:System.Windows.Forms.Screen" /> for the display that contains the point. In multiple display environments where no display contains the point, the display closest to the specified point is returned.</returns>
		// Token: 0x06003437 RID: 13367 RVA: 0x000EF6E0 File Offset: 0x000ED8E0
		public static Screen FromPoint(Point point)
		{
			if (Screen.multiMonitorSupport)
			{
				NativeMethods.POINTSTRUCT pt = new NativeMethods.POINTSTRUCT(point.X, point.Y);
				return new Screen(SafeNativeMethods.MonitorFromPoint(pt, 2));
			}
			return new Screen((IntPtr)(-1163005939));
		}

		/// <summary>Retrieves a <see cref="T:System.Windows.Forms.Screen" /> for the display that contains the largest portion of the rectangle.</summary>
		/// <param name="rect">A <see cref="T:System.Drawing.Rectangle" /> that specifies the area for which to retrieve the display. </param>
		/// <returns>A <see cref="T:System.Windows.Forms.Screen" /> for the display that contains the largest region of the specified rectangle. In multiple display environments where no display contains the rectangle, the display closest to the rectangle is returned.</returns>
		// Token: 0x06003438 RID: 13368 RVA: 0x000EF728 File Offset: 0x000ED928
		public static Screen FromRectangle(Rectangle rect)
		{
			if (Screen.multiMonitorSupport)
			{
				NativeMethods.RECT rect2 = NativeMethods.RECT.FromXYWH(rect.X, rect.Y, rect.Width, rect.Height);
				return new Screen(SafeNativeMethods.MonitorFromRect(ref rect2, 2));
			}
			return new Screen((IntPtr)(-1163005939), IntPtr.Zero);
		}

		/// <summary>Retrieves a <see cref="T:System.Windows.Forms.Screen" /> for the display that contains the largest portion of the specified control.</summary>
		/// <param name="control">A <see cref="T:System.Windows.Forms.Control" /> for which to retrieve a <see cref="T:System.Windows.Forms.Screen" />. </param>
		/// <returns>A <see cref="T:System.Windows.Forms.Screen" /> for the display that contains the largest region of the specified control. In multiple display environments where no display contains the control, the display closest to the specified control is returned.</returns>
		// Token: 0x06003439 RID: 13369 RVA: 0x000EF780 File Offset: 0x000ED980
		public static Screen FromControl(Control control)
		{
			return Screen.FromHandleInternal(control.Handle);
		}

		/// <summary>Retrieves a <see cref="T:System.Windows.Forms.Screen" /> for the display that contains the largest portion of the object referred to by the specified handle.</summary>
		/// <param name="hwnd">The window handle for which to retrieve the <see cref="T:System.Windows.Forms.Screen" />. </param>
		/// <returns>A <see cref="T:System.Windows.Forms.Screen" /> for the display that contains the largest region of the object. In multiple display environments where no display contains any portion of the specified window, the display closest to the object is returned.</returns>
		// Token: 0x0600343A RID: 13370 RVA: 0x000EF78D File Offset: 0x000ED98D
		public static Screen FromHandle(IntPtr hwnd)
		{
			IntSecurity.ObjectFromWin32Handle.Demand();
			return Screen.FromHandleInternal(hwnd);
		}

		// Token: 0x0600343B RID: 13371 RVA: 0x000EF79F File Offset: 0x000ED99F
		internal static Screen FromHandleInternal(IntPtr hwnd)
		{
			if (Screen.multiMonitorSupport)
			{
				return new Screen(SafeNativeMethods.MonitorFromWindow(new HandleRef(null, hwnd), 2));
			}
			return new Screen((IntPtr)(-1163005939), IntPtr.Zero);
		}

		/// <summary>Retrieves the working area closest to the specified point. The working area is the desktop area of the display, excluding taskbars, docked windows, and docked tool bars.</summary>
		/// <param name="pt">A <see cref="T:System.Drawing.Point" /> that specifies the coordinates for which to retrieve the working area. </param>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that specifies the working area. In multiple display environments where no display contains the specified point, the display closest to the point is returned.</returns>
		// Token: 0x0600343C RID: 13372 RVA: 0x000EF7CF File Offset: 0x000ED9CF
		public static Rectangle GetWorkingArea(Point pt)
		{
			return Screen.FromPoint(pt).WorkingArea;
		}

		/// <summary>Retrieves the working area for the display that contains the largest portion of the specified rectangle. The working area is the desktop area of the display, excluding taskbars, docked windows, and docked tool bars.</summary>
		/// <param name="rect">The <see cref="T:System.Drawing.Rectangle" /> that specifies the area for which to retrieve the working area. </param>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that specifies the working area. In multiple display environments where no display contains the specified rectangle, the display closest to the rectangle is returned.</returns>
		// Token: 0x0600343D RID: 13373 RVA: 0x000EF7DC File Offset: 0x000ED9DC
		public static Rectangle GetWorkingArea(Rectangle rect)
		{
			return Screen.FromRectangle(rect).WorkingArea;
		}

		/// <summary>Retrieves the working area for the display that contains the largest region of the specified control. The working area is the desktop area of the display, excluding taskbars, docked windows, and docked tool bars.</summary>
		/// <param name="ctl">The <see cref="T:System.Windows.Forms.Control" /> for which to retrieve the working area. </param>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that specifies the working area. In multiple display environments where no display contains the specified control, the display closest to the control is returned.</returns>
		// Token: 0x0600343E RID: 13374 RVA: 0x000EF7E9 File Offset: 0x000ED9E9
		public static Rectangle GetWorkingArea(Control ctl)
		{
			return Screen.FromControl(ctl).WorkingArea;
		}

		/// <summary>Retrieves the bounds of the display that contains the specified point.</summary>
		/// <param name="pt">A <see cref="T:System.Drawing.Point" /> that specifies the coordinates for which to retrieve the display bounds. </param>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the display that contains the specified point. In multiple display environments where no display contains the specified point, the display closest to the point is returned.</returns>
		// Token: 0x0600343F RID: 13375 RVA: 0x000EF7F6 File Offset: 0x000ED9F6
		public static Rectangle GetBounds(Point pt)
		{
			return Screen.FromPoint(pt).Bounds;
		}

		/// <summary>Retrieves the bounds of the display that contains the largest portion of the specified rectangle.</summary>
		/// <param name="rect">A <see cref="T:System.Drawing.Rectangle" /> that specifies the area for which to retrieve the display bounds. </param>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the display that contains the specified rectangle. In multiple display environments where no monitor contains the specified rectangle, the monitor closest to the rectangle is returned.</returns>
		// Token: 0x06003440 RID: 13376 RVA: 0x000EF803 File Offset: 0x000EDA03
		public static Rectangle GetBounds(Rectangle rect)
		{
			return Screen.FromRectangle(rect).Bounds;
		}

		/// <summary>Retrieves the bounds of the display that contains the largest portion of the specified control.</summary>
		/// <param name="ctl">The <see cref="T:System.Windows.Forms.Control" /> for which to retrieve the display bounds. </param>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the display that contains the specified control. In multiple display environments where no display contains the specified control, the display closest to the control is returned.</returns>
		// Token: 0x06003441 RID: 13377 RVA: 0x000EF810 File Offset: 0x000EDA10
		public static Rectangle GetBounds(Control ctl)
		{
			return Screen.FromControl(ctl).Bounds;
		}

		/// <summary>Computes and retrieves a hash code for an object.</summary>
		/// <returns>A hash code for an object.</returns>
		// Token: 0x06003442 RID: 13378 RVA: 0x000EF81D File Offset: 0x000EDA1D
		public override int GetHashCode()
		{
			return (int)this.hmonitor;
		}

		// Token: 0x06003443 RID: 13379 RVA: 0x000EF82A File Offset: 0x000EDA2A
		private static void OnDisplaySettingsChanging(object sender, EventArgs e)
		{
			SystemEvents.DisplaySettingsChanging -= Screen.OnDisplaySettingsChanging;
			Screen.screens = null;
		}

		// Token: 0x06003444 RID: 13380 RVA: 0x000EF843 File Offset: 0x000EDA43
		private static void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
		{
			if (e.Category == UserPreferenceCategory.Desktop)
			{
				Interlocked.Increment(ref Screen.desktopChangedCount);
			}
		}

		/// <summary>Retrieves a string representing this object.</summary>
		/// <returns>A string representation of the object.</returns>
		// Token: 0x06003445 RID: 13381 RVA: 0x000EF85C File Offset: 0x000EDA5C
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				base.GetType().Name,
				"[Bounds=",
				this.bounds.ToString(),
				" WorkingArea=",
				this.WorkingArea.ToString(),
				" Primary=",
				this.primary.ToString(),
				" DeviceName=",
				this.deviceName
			});
		}

		// Token: 0x04002039 RID: 8249
		private readonly IntPtr hmonitor;

		// Token: 0x0400203A RID: 8250
		private readonly Rectangle bounds;

		// Token: 0x0400203B RID: 8251
		private Rectangle workingArea = Rectangle.Empty;

		// Token: 0x0400203C RID: 8252
		private readonly bool primary;

		// Token: 0x0400203D RID: 8253
		private readonly string deviceName;

		// Token: 0x0400203E RID: 8254
		private readonly int bitDepth;

		// Token: 0x0400203F RID: 8255
		private static object syncLock = new object();

		// Token: 0x04002040 RID: 8256
		private static int desktopChangedCount = -1;

		// Token: 0x04002041 RID: 8257
		private int currentDesktopChangedCount = -1;

		// Token: 0x04002042 RID: 8258
		private const int PRIMARY_MONITOR = -1163005939;

		// Token: 0x04002043 RID: 8259
		private const int MONITOR_DEFAULTTONULL = 0;

		// Token: 0x04002044 RID: 8260
		private const int MONITOR_DEFAULTTOPRIMARY = 1;

		// Token: 0x04002045 RID: 8261
		private const int MONITOR_DEFAULTTONEAREST = 2;

		// Token: 0x04002046 RID: 8262
		private const int MONITORINFOF_PRIMARY = 1;

		// Token: 0x04002047 RID: 8263
		private static bool multiMonitorSupport = UnsafeNativeMethods.GetSystemMetrics(80) != 0;

		// Token: 0x04002048 RID: 8264
		private static Screen[] screens;

		// Token: 0x02000712 RID: 1810
		private class MonitorEnumCallback
		{
			// Token: 0x06005FFC RID: 24572 RVA: 0x00189C25 File Offset: 0x00187E25
			public virtual bool Callback(IntPtr monitor, IntPtr hdc, IntPtr lprcMonitor, IntPtr lparam)
			{
				this.screens.Add(new Screen(monitor, hdc));
				return true;
			}

			// Token: 0x04004134 RID: 16692
			public ArrayList screens = new ArrayList();
		}
	}
}
