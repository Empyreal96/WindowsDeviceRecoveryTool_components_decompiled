using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x02000101 RID: 257
	internal static class DpiHelper
	{
		// Token: 0x0600041A RID: 1050 RVA: 0x0000CECC File Offset: 0x0000B0CC
		private static void Initialize()
		{
			if (DpiHelper.isInitialized)
			{
				return;
			}
			if (DpiHelper.IsDpiAwarenessValueSet())
			{
				DpiHelper.enableHighDpi = true;
			}
			else
			{
				try
				{
					string text = ConfigurationManager.AppSettings.Get("EnableWindowsFormsHighDpiAutoResizing");
					if (!string.IsNullOrEmpty(text) && string.Equals(text, "true", StringComparison.InvariantCultureIgnoreCase))
					{
						DpiHelper.enableHighDpi = true;
					}
				}
				catch
				{
				}
			}
			if (DpiHelper.enableHighDpi)
			{
				try
				{
					DpiHelper.SetWinformsApplicationDpiAwareness();
				}
				catch (Exception ex)
				{
				}
				IntPtr dc = UnsafeNativeMethods.GetDC(NativeMethods.NullHandleRef);
				if (dc != IntPtr.Zero)
				{
					DpiHelper.deviceDpi = (double)UnsafeNativeMethods.GetDeviceCaps(new HandleRef(null, dc), 88);
					UnsafeNativeMethods.ReleaseDC(NativeMethods.NullHandleRef, new HandleRef(null, dc));
				}
			}
			DpiHelper.isInitialized = true;
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x0000CF94 File Offset: 0x0000B194
		internal static bool IsDpiAwarenessValueSet()
		{
			bool result = false;
			try
			{
				if (string.IsNullOrEmpty(DpiHelper.dpiAwarenessValue))
				{
					DpiHelper.dpiAwarenessValue = ConfigurationOptions.GetConfigSettingValue("DpiAwareness");
				}
			}
			catch
			{
			}
			if (!string.IsNullOrEmpty(DpiHelper.dpiAwarenessValue))
			{
				string a = DpiHelper.dpiAwarenessValue.ToLowerInvariant();
				if (!(a == "true") && !(a == "system") && !(a == "true/pm") && !(a == "permonitor") && !(a == "permonitorv2"))
				{
					if (!(a == "false"))
					{
					}
				}
				else
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x0000D040 File Offset: 0x0000B240
		internal static void InitializeDpiHelperForWinforms()
		{
			DpiHelper.Initialize();
			DpiHelper.InitializeDpiHelperQuirks();
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x0000D04C File Offset: 0x0000B24C
		internal static void InitializeDpiHelperQuirks()
		{
			if (DpiHelper.isDpiHelperQuirksInitialized)
			{
				return;
			}
			try
			{
				if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.CompareTo(ConfigurationOptions.RS2Version) >= 0 && DpiHelper.IsExpectedConfigValue("DisableDpiChangedMessageHandling", false) && DpiHelper.IsDpiAwarenessValueSet() && Application.RenderWithVisualStyles)
				{
					DpiHelper.enableDpiChangedMessageHandling = true;
				}
				if ((DpiHelper.IsScalingRequired || DpiHelper.enableDpiChangedMessageHandling) && DpiHelper.IsDpiAwarenessValueSet())
				{
					if (DpiHelper.IsExpectedConfigValue("CheckedListBox.DisableHighDpiImprovements", false))
					{
						DpiHelper.enableCheckedListBoxHighDpiImprovements = true;
					}
					if (DpiHelper.IsExpectedConfigValue("ToolStrip.DisableHighDpiImprovements", false))
					{
						DpiHelper.enableToolStripHighDpiImprovements = true;
					}
					if (DpiHelper.IsExpectedConfigValue("Form.DisableSinglePassScalingOfDpiForms", false))
					{
						DpiHelper.enableSinglePassScalingOfDpiForms = true;
					}
					if (DpiHelper.IsExpectedConfigValue("DataGridView.DisableHighDpiImprovements", false))
					{
						DpiHelper.enableDataGridViewControlHighDpiImprovements = true;
					}
					if (DpiHelper.IsExpectedConfigValue("AnchorLayout.DisableHighDpiImprovements", false))
					{
						DpiHelper.enableAnchorLayoutHighDpiImprovements = true;
					}
					if (DpiHelper.IsExpectedConfigValue("MonthCalendar.DisableHighDpiImprovements", false))
					{
						DpiHelper.enableMonthCalendarHighDpiImprovements = true;
					}
					if (ConfigurationOptions.GetConfigSettingValue("DisableDpiChangedHighDpiImprovements") == null)
					{
						if (ConfigurationOptions.NetFrameworkVersion.CompareTo(DpiHelper.dpiChangedMessageHighDpiImprovementsMinimumFrameworkVersion) >= 0)
						{
							DpiHelper.enableDpiChangedHighDpiImprovements = true;
						}
					}
					else if (DpiHelper.IsExpectedConfigValue("DisableDpiChangedHighDpiImprovements", false))
					{
						DpiHelper.enableDpiChangedHighDpiImprovements = true;
					}
					DpiHelper.enableThreadExceptionDialogHighDpiImprovements = true;
				}
			}
			catch
			{
			}
			DpiHelper.isDpiHelperQuirksInitialized = true;
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x0000D19C File Offset: 0x0000B39C
		internal static bool IsExpectedConfigValue(string configurationSettingName, bool expectedValue)
		{
			string configSettingValue = ConfigurationOptions.GetConfigSettingValue(configurationSettingName);
			bool flag;
			if (!bool.TryParse(configSettingValue, out flag))
			{
				flag = false;
			}
			return flag == expectedValue;
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x0600041F RID: 1055 RVA: 0x0000D1C0 File Offset: 0x0000B3C0
		internal static bool EnableDpiChangedHighDpiImprovements
		{
			get
			{
				DpiHelper.InitializeDpiHelperForWinforms();
				return DpiHelper.enableDpiChangedHighDpiImprovements;
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000420 RID: 1056 RVA: 0x0000D1CC File Offset: 0x0000B3CC
		internal static bool EnableToolStripHighDpiImprovements
		{
			get
			{
				DpiHelper.InitializeDpiHelperForWinforms();
				return DpiHelper.enableToolStripHighDpiImprovements;
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000421 RID: 1057 RVA: 0x0000D1D8 File Offset: 0x0000B3D8
		internal static bool EnableToolStripPerMonitorV2HighDpiImprovements
		{
			get
			{
				return DpiHelper.EnableDpiChangedMessageHandling && DpiHelper.enableToolStripHighDpiImprovements && DpiHelper.enableDpiChangedHighDpiImprovements;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000422 RID: 1058 RVA: 0x0000D1F0 File Offset: 0x0000B3F0
		internal static bool EnableDpiChangedMessageHandling
		{
			get
			{
				DpiHelper.InitializeDpiHelperForWinforms();
				if (DpiHelper.enableDpiChangedMessageHandling)
				{
					DpiAwarenessContext threadDpiAwarenessContext = CommonUnsafeNativeMethods.GetThreadDpiAwarenessContext();
					return CommonUnsafeNativeMethods.TryFindDpiAwarenessContextsEqual(threadDpiAwarenessContext, DpiAwarenessContext.DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2);
				}
				return false;
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000423 RID: 1059 RVA: 0x0000D219 File Offset: 0x0000B419
		internal static bool EnableCheckedListBoxHighDpiImprovements
		{
			get
			{
				DpiHelper.InitializeDpiHelperForWinforms();
				return DpiHelper.enableCheckedListBoxHighDpiImprovements;
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000424 RID: 1060 RVA: 0x0000D225 File Offset: 0x0000B425
		internal static bool EnableSinglePassScalingOfDpiForms
		{
			get
			{
				DpiHelper.InitializeDpiHelperForWinforms();
				return DpiHelper.enableSinglePassScalingOfDpiForms;
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000425 RID: 1061 RVA: 0x0000D231 File Offset: 0x0000B431
		internal static bool EnableThreadExceptionDialogHighDpiImprovements
		{
			get
			{
				DpiHelper.InitializeDpiHelperForWinforms();
				return DpiHelper.enableThreadExceptionDialogHighDpiImprovements;
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000426 RID: 1062 RVA: 0x0000D23D File Offset: 0x0000B43D
		internal static bool EnableDataGridViewControlHighDpiImprovements
		{
			get
			{
				DpiHelper.InitializeDpiHelperForWinforms();
				return DpiHelper.enableDataGridViewControlHighDpiImprovements;
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000427 RID: 1063 RVA: 0x0000D249 File Offset: 0x0000B449
		internal static bool EnableAnchorLayoutHighDpiImprovements
		{
			get
			{
				DpiHelper.InitializeDpiHelperForWinforms();
				return DpiHelper.enableAnchorLayoutHighDpiImprovements;
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000428 RID: 1064 RVA: 0x0000D255 File Offset: 0x0000B455
		internal static bool EnableMonthCalendarHighDpiImprovements
		{
			get
			{
				DpiHelper.InitializeDpiHelperForWinforms();
				return DpiHelper.enableMonthCalendarHighDpiImprovements;
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000429 RID: 1065 RVA: 0x0000D261 File Offset: 0x0000B461
		internal static int DeviceDpi
		{
			get
			{
				DpiHelper.Initialize();
				return (int)DpiHelper.deviceDpi;
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x0600042A RID: 1066 RVA: 0x0000D26E File Offset: 0x0000B46E
		private static double LogicalToDeviceUnitsScalingFactor
		{
			get
			{
				if (DpiHelper.logicalToDeviceUnitsScalingFactor == 0.0)
				{
					DpiHelper.Initialize();
					DpiHelper.logicalToDeviceUnitsScalingFactor = DpiHelper.deviceDpi / 96.0;
				}
				return DpiHelper.logicalToDeviceUnitsScalingFactor;
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x0600042B RID: 1067 RVA: 0x0000D2A0 File Offset: 0x0000B4A0
		private static InterpolationMode InterpolationMode
		{
			get
			{
				if (DpiHelper.interpolationMode == InterpolationMode.Invalid)
				{
					int num = (int)Math.Round(DpiHelper.LogicalToDeviceUnitsScalingFactor * 100.0);
					if (num % 100 == 0)
					{
						DpiHelper.interpolationMode = InterpolationMode.NearestNeighbor;
					}
					else if (num < 100)
					{
						DpiHelper.interpolationMode = InterpolationMode.HighQualityBilinear;
					}
					else
					{
						DpiHelper.interpolationMode = InterpolationMode.HighQualityBicubic;
					}
				}
				return DpiHelper.interpolationMode;
			}
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x0000D2F4 File Offset: 0x0000B4F4
		private static Bitmap ScaleBitmapToSize(Bitmap logicalImage, Size deviceImageSize)
		{
			Bitmap bitmap = new Bitmap(deviceImageSize.Width, deviceImageSize.Height, logicalImage.PixelFormat);
			using (Graphics graphics = Graphics.FromImage(bitmap))
			{
				graphics.InterpolationMode = DpiHelper.InterpolationMode;
				RectangleF srcRect = new RectangleF(0f, 0f, (float)logicalImage.Size.Width, (float)logicalImage.Size.Height);
				RectangleF destRect = new RectangleF(0f, 0f, (float)deviceImageSize.Width, (float)deviceImageSize.Height);
				srcRect.Offset(-0.5f, -0.5f);
				graphics.DrawImage(logicalImage, destRect, srcRect, GraphicsUnit.Pixel);
			}
			return bitmap;
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x0000D3B8 File Offset: 0x0000B5B8
		private static Bitmap CreateScaledBitmap(Bitmap logicalImage, int deviceDpi = 0)
		{
			Size deviceImageSize = DpiHelper.LogicalToDeviceUnits(logicalImage.Size, deviceDpi);
			return DpiHelper.ScaleBitmapToSize(logicalImage, deviceImageSize);
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x0600042E RID: 1070 RVA: 0x0000D3D9 File Offset: 0x0000B5D9
		public static bool IsScalingRequired
		{
			get
			{
				DpiHelper.Initialize();
				return DpiHelper.deviceDpi != 96.0;
			}
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x0000D3F4 File Offset: 0x0000B5F4
		public static int LogicalToDeviceUnits(int value, int devicePixels = 0)
		{
			if (devicePixels == 0)
			{
				return (int)Math.Round(DpiHelper.LogicalToDeviceUnitsScalingFactor * (double)value);
			}
			double num = (double)devicePixels / 96.0;
			return (int)Math.Round(num * (double)value);
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x0000D42C File Offset: 0x0000B62C
		public static double LogicalToDeviceUnits(double value, int devicePixels = 0)
		{
			if (devicePixels == 0)
			{
				return DpiHelper.LogicalToDeviceUnitsScalingFactor * value;
			}
			double num = (double)devicePixels / 96.0;
			return num * value;
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x0000D454 File Offset: 0x0000B654
		public static int LogicalToDeviceUnitsX(int value)
		{
			return DpiHelper.LogicalToDeviceUnits(value, 0);
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x0000D454 File Offset: 0x0000B654
		public static int LogicalToDeviceUnitsY(int value)
		{
			return DpiHelper.LogicalToDeviceUnits(value, 0);
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x0000D45D File Offset: 0x0000B65D
		public static Size LogicalToDeviceUnits(Size logicalSize, int deviceDpi = 0)
		{
			return new Size(DpiHelper.LogicalToDeviceUnits(logicalSize.Width, deviceDpi), DpiHelper.LogicalToDeviceUnits(logicalSize.Height, deviceDpi));
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x0000D47E File Offset: 0x0000B67E
		public static Bitmap CreateResizedBitmap(Bitmap logicalImage, Size targetImageSize)
		{
			if (logicalImage == null)
			{
				return null;
			}
			return DpiHelper.ScaleBitmapToSize(logicalImage, targetImageSize);
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x0000D48C File Offset: 0x0000B68C
		public static void ScaleBitmapLogicalToDevice(ref Bitmap logicalBitmap, int deviceDpi = 0)
		{
			if (logicalBitmap == null)
			{
				return;
			}
			Bitmap bitmap = DpiHelper.CreateScaledBitmap(logicalBitmap, deviceDpi);
			if (bitmap != null)
			{
				logicalBitmap.Dispose();
				logicalBitmap = bitmap;
			}
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x0000D4B4 File Offset: 0x0000B6B4
		public static int ConvertToGivenDpiPixel(int value, double pixelFactor)
		{
			int num = (int)Math.Round((double)value * pixelFactor);
			if (num != 0)
			{
				return num;
			}
			return 1;
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x0000D4D2 File Offset: 0x0000B6D2
		public static IDisposable EnterDpiAwarenessScope(DpiAwarenessContext awareness)
		{
			return new DpiHelper.DpiAwarenessScope(awareness);
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x0000D4DC File Offset: 0x0000B6DC
		public static T CreateInstanceInSystemAwareContext<T>(Func<T> createInstance)
		{
			T result;
			using (DpiHelper.EnterDpiAwarenessScope(DpiAwarenessContext.DPI_AWARENESS_CONTEXT_SYSTEM_AWARE))
			{
				result = createInstance();
			}
			return result;
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x0000D518 File Offset: 0x0000B718
		public static bool SetWinformsApplicationDpiAwareness()
		{
			Version version = Environment.OSVersion.Version;
			if (!DpiHelper.IsDpiAwarenessValueSet() || Environment.OSVersion.Platform != PlatformID.Win32NT)
			{
				return false;
			}
			string a = (DpiHelper.dpiAwarenessValue ?? string.Empty).ToLowerInvariant();
			if (version.CompareTo(ConfigurationOptions.RS2Version) >= 0)
			{
				int processDpiAwarenessContext;
				if (!(a == "true") && !(a == "system"))
				{
					if (!(a == "true/pm") && !(a == "permonitor"))
					{
						if (!(a == "permonitorv2"))
						{
							if (!(a == "false"))
							{
							}
							processDpiAwarenessContext = -1;
						}
						else
						{
							processDpiAwarenessContext = -4;
						}
					}
					else
					{
						processDpiAwarenessContext = -3;
					}
				}
				else
				{
					processDpiAwarenessContext = -2;
				}
				if (!SafeNativeMethods.SetProcessDpiAwarenessContext(processDpiAwarenessContext))
				{
					return false;
				}
			}
			else if (version.CompareTo(new Version(6, 3, 0, 0)) >= 0 && version.CompareTo(ConfigurationOptions.RS2Version) < 0)
			{
				NativeMethods.PROCESS_DPI_AWARENESS process_DPI_AWARENESS;
				if (!(a == "false"))
				{
					if (!(a == "true") && !(a == "system"))
					{
						if (!(a == "true/pm") && !(a == "permonitor") && !(a == "permonitorv2"))
						{
							process_DPI_AWARENESS = NativeMethods.PROCESS_DPI_AWARENESS.PROCESS_DPI_UNINITIALIZED;
						}
						else
						{
							process_DPI_AWARENESS = NativeMethods.PROCESS_DPI_AWARENESS.PROCESS_PER_MONITOR_DPI_AWARE;
						}
					}
					else
					{
						process_DPI_AWARENESS = NativeMethods.PROCESS_DPI_AWARENESS.PROCESS_SYSTEM_DPI_AWARE;
					}
				}
				else
				{
					process_DPI_AWARENESS = NativeMethods.PROCESS_DPI_AWARENESS.PROCESS_DPI_UNAWARE;
				}
				if (SafeNativeMethods.SetProcessDpiAwareness(process_DPI_AWARENESS) != 0)
				{
					return false;
				}
			}
			else
			{
				if (version.CompareTo(new Version(6, 1, 0, 0)) < 0 || version.CompareTo(new Version(6, 3, 0, 0)) >= 0)
				{
					return false;
				}
				NativeMethods.PROCESS_DPI_AWARENESS process_DPI_AWARENESS;
				if (!(a == "false"))
				{
					if (!(a == "true") && !(a == "system") && !(a == "true/pm") && !(a == "permonitor") && !(a == "permonitorv2"))
					{
						process_DPI_AWARENESS = NativeMethods.PROCESS_DPI_AWARENESS.PROCESS_DPI_UNINITIALIZED;
					}
					else
					{
						process_DPI_AWARENESS = NativeMethods.PROCESS_DPI_AWARENESS.PROCESS_SYSTEM_DPI_AWARE;
					}
				}
				else
				{
					process_DPI_AWARENESS = NativeMethods.PROCESS_DPI_AWARENESS.PROCESS_DPI_UNAWARE;
				}
				if (process_DPI_AWARENESS == NativeMethods.PROCESS_DPI_AWARENESS.PROCESS_SYSTEM_DPI_AWARE && !SafeNativeMethods.SetProcessDPIAware())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x0000D6F1 File Offset: 0x0000B8F1
		public static Padding LogicalToDeviceUnits(Padding logicalPadding, int deviceDpi = 0)
		{
			return new Padding(DpiHelper.LogicalToDeviceUnits(logicalPadding.Left, deviceDpi), DpiHelper.LogicalToDeviceUnits(logicalPadding.Top, deviceDpi), DpiHelper.LogicalToDeviceUnits(logicalPadding.Right, deviceDpi), DpiHelper.LogicalToDeviceUnits(logicalPadding.Bottom, deviceDpi));
		}

		// Token: 0x04000435 RID: 1077
		internal const double LogicalDpi = 96.0;

		// Token: 0x04000436 RID: 1078
		private static bool isInitialized = false;

		// Token: 0x04000437 RID: 1079
		private static double deviceDpi = 96.0;

		// Token: 0x04000438 RID: 1080
		private static double logicalToDeviceUnitsScalingFactor = 0.0;

		// Token: 0x04000439 RID: 1081
		private static bool enableHighDpi = false;

		// Token: 0x0400043A RID: 1082
		private static string dpiAwarenessValue = null;

		// Token: 0x0400043B RID: 1083
		private static InterpolationMode interpolationMode = InterpolationMode.Invalid;

		// Token: 0x0400043C RID: 1084
		private static bool isDpiHelperQuirksInitialized = false;

		// Token: 0x0400043D RID: 1085
		private static bool enableToolStripHighDpiImprovements = false;

		// Token: 0x0400043E RID: 1086
		private static bool enableDpiChangedMessageHandling = false;

		// Token: 0x0400043F RID: 1087
		private static bool enableCheckedListBoxHighDpiImprovements = false;

		// Token: 0x04000440 RID: 1088
		private static bool enableThreadExceptionDialogHighDpiImprovements = false;

		// Token: 0x04000441 RID: 1089
		private static bool enableDataGridViewControlHighDpiImprovements = false;

		// Token: 0x04000442 RID: 1090
		private static bool enableSinglePassScalingOfDpiForms = false;

		// Token: 0x04000443 RID: 1091
		private static bool enableAnchorLayoutHighDpiImprovements = false;

		// Token: 0x04000444 RID: 1092
		private static bool enableMonthCalendarHighDpiImprovements = false;

		// Token: 0x04000445 RID: 1093
		private static bool enableDpiChangedHighDpiImprovements = false;

		// Token: 0x04000446 RID: 1094
		private static readonly Version dpiChangedMessageHighDpiImprovementsMinimumFrameworkVersion = new Version(4, 8);

		// Token: 0x02000540 RID: 1344
		private class DpiAwarenessScope : IDisposable
		{
			// Token: 0x060054E5 RID: 21733 RVA: 0x001645C4 File Offset: 0x001627C4
			public DpiAwarenessScope(DpiAwarenessContext awareness)
			{
				if (DpiHelper.EnableDpiChangedHighDpiImprovements)
				{
					try
					{
						if (!CommonUnsafeNativeMethods.TryFindDpiAwarenessContextsEqual(awareness, DpiAwarenessContext.DPI_AWARENESS_CONTEXT_UNSPECIFIED))
						{
							this.originalAwareness = CommonUnsafeNativeMethods.GetThreadDpiAwarenessContext();
							if (!CommonUnsafeNativeMethods.TryFindDpiAwarenessContextsEqual(this.originalAwareness, awareness) && !CommonUnsafeNativeMethods.TryFindDpiAwarenessContextsEqual(this.originalAwareness, DpiAwarenessContext.DPI_AWARENESS_CONTEXT_UNAWARE))
							{
								this.originalAwareness = CommonUnsafeNativeMethods.SetThreadDpiAwarenessContext(awareness);
								this.dpiAwarenessScopeIsSet = true;
							}
						}
					}
					catch (EntryPointNotFoundException)
					{
						this.dpiAwarenessScopeIsSet = false;
					}
				}
			}

			// Token: 0x060054E6 RID: 21734 RVA: 0x00164640 File Offset: 0x00162840
			public void Dispose()
			{
				this.ResetDpiAwarenessContextChanges();
			}

			// Token: 0x060054E7 RID: 21735 RVA: 0x00164648 File Offset: 0x00162848
			private void ResetDpiAwarenessContextChanges()
			{
				if (this.dpiAwarenessScopeIsSet)
				{
					CommonUnsafeNativeMethods.TrySetThreadDpiAwarenessContext(this.originalAwareness);
					this.dpiAwarenessScopeIsSet = false;
				}
			}

			// Token: 0x04003761 RID: 14177
			private bool dpiAwarenessScopeIsSet;

			// Token: 0x04003762 RID: 14178
			private DpiAwarenessContext originalAwareness;
		}
	}
}
