using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Deployment.Application;
using System.Deployment.Internal.Isolation;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Windows.Forms.Layout;
using System.Windows.Forms.VisualStyles;
using Microsoft.Win32;

namespace System.Windows.Forms
{
	/// <summary>Provides <see langword="static" /> methods and properties to manage an application, such as methods to start and stop an application, to process Windows messages, and properties to get information about an application. This class cannot be inherited.</summary>
	// Token: 0x02000111 RID: 273
	public sealed class Application
	{
		// Token: 0x060005A5 RID: 1445 RVA: 0x000027DB File Offset: 0x000009DB
		private Application()
		{
		}

		/// <summary>Gets a value indicating whether the caller can quit this application.</summary>
		/// <returns>
		///     <see langword="true" /> if the caller can quit this application; otherwise, <see langword="false" />.</returns>
		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x060005A6 RID: 1446 RVA: 0x00010173 File Offset: 0x0000E373
		public static bool AllowQuit
		{
			get
			{
				return Application.ThreadContext.FromCurrent().GetAllowQuit();
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x060005A7 RID: 1447 RVA: 0x0001017F File Offset: 0x0000E37F
		internal static bool CanContinueIdle
		{
			get
			{
				return Application.ThreadContext.FromCurrent().ComponentManager.FContinueIdle();
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x060005A8 RID: 1448 RVA: 0x00010190 File Offset: 0x0000E390
		internal static bool ComCtlSupportsVisualStyles
		{
			get
			{
				if (!Application.comCtlSupportsVisualStylesInitialized)
				{
					Application.comCtlSupportsVisualStyles = Application.InitializeComCtlSupportsVisualStyles();
					Application.comCtlSupportsVisualStylesInitialized = true;
				}
				return Application.comCtlSupportsVisualStyles;
			}
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x000101B0 File Offset: 0x0000E3B0
		private static bool InitializeComCtlSupportsVisualStyles()
		{
			if (Application.useVisualStyles && OSFeature.Feature.IsPresent(OSFeature.Themes))
			{
				return true;
			}
			IntPtr intPtr = UnsafeNativeMethods.GetModuleHandle("comctl32.dll");
			if (intPtr != IntPtr.Zero)
			{
				try
				{
					IntPtr procAddress = UnsafeNativeMethods.GetProcAddress(new HandleRef(null, intPtr), "ImageList_WriteEx");
					return procAddress != IntPtr.Zero;
				}
				catch
				{
					return false;
				}
			}
			intPtr = UnsafeNativeMethods.LoadLibraryFromSystemPathIfAvailable("comctl32.dll");
			if (intPtr != IntPtr.Zero)
			{
				IntPtr procAddress2 = UnsafeNativeMethods.GetProcAddress(new HandleRef(null, intPtr), "ImageList_WriteEx");
				return procAddress2 != IntPtr.Zero;
			}
			return false;
		}

		/// <summary>Gets the registry key for the application data that is shared among all users.</summary>
		/// <returns>A <see cref="T:Microsoft.Win32.RegistryKey" /> representing the registry key of the application data that is shared among all users.</returns>
		// Token: 0x170001EC RID: 492
		// (get) Token: 0x060005AA RID: 1450 RVA: 0x0001025C File Offset: 0x0000E45C
		public static RegistryKey CommonAppDataRegistry
		{
			get
			{
				return Registry.LocalMachine.CreateSubKey(Application.CommonAppDataRegistryKeyName);
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x060005AB RID: 1451 RVA: 0x00010270 File Offset: 0x0000E470
		internal static string CommonAppDataRegistryKeyName
		{
			get
			{
				string format = "Software\\{0}\\{1}\\{2}";
				return string.Format(CultureInfo.CurrentCulture, format, new object[]
				{
					Application.CompanyName,
					Application.ProductName,
					Application.ProductVersion
				});
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x060005AC RID: 1452 RVA: 0x000102AC File Offset: 0x0000E4AC
		internal static bool UseEverettThreadAffinity
		{
			get
			{
				if (!Application.checkedThreadAffinity)
				{
					Application.checkedThreadAffinity = true;
					try
					{
						new RegistryPermission(PermissionState.Unrestricted).Assert();
						RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(Application.CommonAppDataRegistryKeyName);
						if (registryKey != null)
						{
							object value = registryKey.GetValue("EnableSystemEventsThreadAffinityCompatibility");
							registryKey.Close();
							if (value != null && (int)value != 0)
							{
								Application.useEverettThreadAffinity = true;
							}
						}
					}
					catch (SecurityException)
					{
					}
					catch (InvalidCastException)
					{
					}
				}
				return Application.useEverettThreadAffinity;
			}
		}

		/// <summary>Gets the path for the application data that is shared among all users.</summary>
		/// <returns>The path for the application data that is shared among all users.</returns>
		// Token: 0x170001EF RID: 495
		// (get) Token: 0x060005AD RID: 1453 RVA: 0x00010330 File Offset: 0x0000E530
		public static string CommonAppDataPath
		{
			get
			{
				try
				{
					if (ApplicationDeployment.IsNetworkDeployed)
					{
						string text = AppDomain.CurrentDomain.GetData("DataDirectory") as string;
						if (text != null)
						{
							return text;
						}
					}
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsSecurityOrCriticalException(ex))
					{
						throw;
					}
				}
				return Application.GetDataPath(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));
			}
		}

		/// <summary>Gets the company name associated with the application.</summary>
		/// <returns>The company name.</returns>
		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x060005AE RID: 1454 RVA: 0x00010390 File Offset: 0x0000E590
		public static string CompanyName
		{
			get
			{
				object obj = Application.internalSyncObject;
				lock (obj)
				{
					if (Application.companyName == null)
					{
						Assembly entryAssembly = Assembly.GetEntryAssembly();
						if (entryAssembly != null)
						{
							object[] customAttributes = entryAssembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
							if (customAttributes != null && customAttributes.Length != 0)
							{
								Application.companyName = ((AssemblyCompanyAttribute)customAttributes[0]).Company;
							}
						}
						if (Application.companyName == null || Application.companyName.Length == 0)
						{
							Application.companyName = Application.GetAppFileVersionInfo().CompanyName;
							if (Application.companyName != null)
							{
								Application.companyName = Application.companyName.Trim();
							}
						}
						if (Application.companyName == null || Application.companyName.Length == 0)
						{
							Type appMainType = Application.GetAppMainType();
							if (appMainType != null)
							{
								string @namespace = appMainType.Namespace;
								if (!string.IsNullOrEmpty(@namespace))
								{
									int num = @namespace.IndexOf(".");
									if (num != -1)
									{
										Application.companyName = @namespace.Substring(0, num);
									}
									else
									{
										Application.companyName = @namespace;
									}
								}
								else
								{
									Application.companyName = Application.ProductName;
								}
							}
						}
					}
				}
				return Application.companyName;
			}
		}

		/// <summary>Gets or sets the culture information for the current thread.</summary>
		/// <returns>A <see cref="T:System.Globalization.CultureInfo" /> representing the culture information for the current thread.</returns>
		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x060005AF RID: 1455 RVA: 0x000104B8 File Offset: 0x0000E6B8
		// (set) Token: 0x060005B0 RID: 1456 RVA: 0x000104C4 File Offset: 0x0000E6C4
		public static CultureInfo CurrentCulture
		{
			get
			{
				return Thread.CurrentThread.CurrentCulture;
			}
			set
			{
				Thread.CurrentThread.CurrentCulture = value;
			}
		}

		/// <summary>Gets or sets the current input language for the current thread.</summary>
		/// <returns>An <see cref="T:System.Windows.Forms.InputLanguage" /> representing the current input language for the current thread.</returns>
		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x060005B1 RID: 1457 RVA: 0x000104D1 File Offset: 0x0000E6D1
		// (set) Token: 0x060005B2 RID: 1458 RVA: 0x000104D8 File Offset: 0x0000E6D8
		public static InputLanguage CurrentInputLanguage
		{
			get
			{
				return InputLanguage.CurrentInputLanguage;
			}
			set
			{
				IntSecurity.AffectThreadBehavior.Demand();
				InputLanguage.CurrentInputLanguage = value;
			}
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x060005B3 RID: 1459 RVA: 0x000104EA File Offset: 0x0000E6EA
		internal static bool CustomThreadExceptionHandlerAttached
		{
			get
			{
				return Application.ThreadContext.FromCurrent().CustomThreadExceptionHandlerAttached;
			}
		}

		/// <summary>Gets the path for the executable file that started the application, including the executable name.</summary>
		/// <returns>The path and executable name for the executable file that started the application.This path will be different depending on whether the Windows Forms application is deployed using ClickOnce. ClickOnce applications are stored in a per-user application cache in the C:\Documents and Settings\username directory. For more information, see Accessing Local and Remote Data in ClickOnce Applications.</returns>
		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x060005B4 RID: 1460 RVA: 0x000104F8 File Offset: 0x0000E6F8
		public static string ExecutablePath
		{
			get
			{
				if (Application.executablePath == null)
				{
					Assembly entryAssembly = Assembly.GetEntryAssembly();
					if (entryAssembly == null)
					{
						StringBuilder moduleFileNameLongPath = UnsafeNativeMethods.GetModuleFileNameLongPath(NativeMethods.NullHandleRef);
						Application.executablePath = IntSecurity.UnsafeGetFullPath(moduleFileNameLongPath.ToString());
					}
					else
					{
						string codeBase = entryAssembly.CodeBase;
						Uri uri = new Uri(codeBase);
						if (uri.IsFile)
						{
							Application.executablePath = uri.LocalPath + Uri.UnescapeDataString(uri.Fragment);
						}
						else
						{
							Application.executablePath = uri.ToString();
						}
					}
				}
				Uri uri2 = new Uri(Application.executablePath);
				if (uri2.Scheme == "file")
				{
					new FileIOPermission(FileIOPermissionAccess.PathDiscovery, Application.executablePath).Demand();
				}
				return Application.executablePath;
			}
		}

		/// <summary>Gets the path for the application data of a local, non-roaming user.</summary>
		/// <returns>The path for the application data of a local, non-roaming user.</returns>
		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x060005B5 RID: 1461 RVA: 0x000105B0 File Offset: 0x0000E7B0
		public static string LocalUserAppDataPath
		{
			get
			{
				try
				{
					if (ApplicationDeployment.IsNetworkDeployed)
					{
						string text = AppDomain.CurrentDomain.GetData("DataDirectory") as string;
						if (text != null)
						{
							return text;
						}
					}
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsSecurityOrCriticalException(ex))
					{
						throw;
					}
				}
				return Application.GetDataPath(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
			}
		}

		/// <summary>Gets a value indicating whether a message loop exists on this thread.</summary>
		/// <returns>
		///     <see langword="true" /> if a message loop exists; otherwise, <see langword="false" />.</returns>
		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x060005B6 RID: 1462 RVA: 0x00010610 File Offset: 0x0000E810
		public static bool MessageLoop
		{
			get
			{
				return Application.ThreadContext.FromCurrent().GetMessageLoop();
			}
		}

		/// <summary>Gets a collection of open forms owned by the application.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.FormCollection" /> containing all the currently open forms owned by this application.</returns>
		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x060005B7 RID: 1463 RVA: 0x0001061C File Offset: 0x0000E81C
		public static FormCollection OpenForms
		{
			[UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
			get
			{
				return Application.OpenFormsInternal;
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x060005B8 RID: 1464 RVA: 0x00010623 File Offset: 0x0000E823
		internal static FormCollection OpenFormsInternal
		{
			get
			{
				if (Application.forms == null)
				{
					Application.forms = new FormCollection();
				}
				return Application.forms;
			}
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x0001063B File Offset: 0x0000E83B
		internal static void OpenFormsInternalAdd(Form form)
		{
			Application.OpenFormsInternal.Add(form);
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x00010648 File Offset: 0x0000E848
		internal static void OpenFormsInternalRemove(Form form)
		{
			Application.OpenFormsInternal.Remove(form);
		}

		/// <summary>Gets the product name associated with this application.</summary>
		/// <returns>The product name.</returns>
		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x060005BB RID: 1467 RVA: 0x00010658 File Offset: 0x0000E858
		public static string ProductName
		{
			get
			{
				object obj = Application.internalSyncObject;
				lock (obj)
				{
					if (Application.productName == null)
					{
						Assembly entryAssembly = Assembly.GetEntryAssembly();
						if (entryAssembly != null)
						{
							object[] customAttributes = entryAssembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
							if (customAttributes != null && customAttributes.Length != 0)
							{
								Application.productName = ((AssemblyProductAttribute)customAttributes[0]).Product;
							}
						}
						if (Application.productName == null || Application.productName.Length == 0)
						{
							Application.productName = Application.GetAppFileVersionInfo().ProductName;
							if (Application.productName != null)
							{
								Application.productName = Application.productName.Trim();
							}
						}
						if (Application.productName == null || Application.productName.Length == 0)
						{
							Type appMainType = Application.GetAppMainType();
							if (appMainType != null)
							{
								string @namespace = appMainType.Namespace;
								if (!string.IsNullOrEmpty(@namespace))
								{
									int num = @namespace.LastIndexOf(".");
									if (num != -1 && num < @namespace.Length - 1)
									{
										Application.productName = @namespace.Substring(num + 1);
									}
									else
									{
										Application.productName = @namespace;
									}
								}
								else
								{
									Application.productName = appMainType.Name;
								}
							}
						}
					}
				}
				return Application.productName;
			}
		}

		/// <summary>Gets the product version associated with this application.</summary>
		/// <returns>The product version.</returns>
		// Token: 0x170001FA RID: 506
		// (get) Token: 0x060005BC RID: 1468 RVA: 0x0001079C File Offset: 0x0000E99C
		public static string ProductVersion
		{
			get
			{
				object obj = Application.internalSyncObject;
				lock (obj)
				{
					if (Application.productVersion == null)
					{
						Assembly entryAssembly = Assembly.GetEntryAssembly();
						if (entryAssembly != null)
						{
							object[] customAttributes = entryAssembly.GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false);
							if (customAttributes != null && customAttributes.Length != 0)
							{
								Application.productVersion = ((AssemblyInformationalVersionAttribute)customAttributes[0]).InformationalVersion;
							}
						}
						if (Application.productVersion == null || Application.productVersion.Length == 0)
						{
							Application.productVersion = Application.GetAppFileVersionInfo().ProductVersion;
							if (Application.productVersion != null)
							{
								Application.productVersion = Application.productVersion.Trim();
							}
						}
						if (Application.productVersion == null || Application.productVersion.Length == 0)
						{
							Application.productVersion = "1.0.0.0";
						}
					}
				}
				return Application.productVersion;
			}
		}

		/// <summary>Registers a callback for checking whether the message loop is running in hosted environments.</summary>
		/// <param name="callback">The method to call when Windows Forms needs to check if the hosting environment is still sending messages.</param>
		// Token: 0x060005BD RID: 1469 RVA: 0x00010874 File Offset: 0x0000EA74
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void RegisterMessageLoop(Application.MessageLoopCallback callback)
		{
			Application.ThreadContext.FromCurrent().RegisterMessageLoop(callback);
		}

		/// <summary>Gets a value specifying whether the current application is drawing controls with visual styles.</summary>
		/// <returns>
		///     <see langword="true" /> if visual styles are enabled for controls in the client area of application windows; otherwise, <see langword="false" />.</returns>
		// Token: 0x170001FB RID: 507
		// (get) Token: 0x060005BE RID: 1470 RVA: 0x00010881 File Offset: 0x0000EA81
		public static bool RenderWithVisualStyles
		{
			get
			{
				return Application.ComCtlSupportsVisualStyles && VisualStyleRenderer.IsSupported;
			}
		}

		/// <summary>Gets or sets the format string to apply to top-level window captions when they are displayed with a warning banner.</summary>
		/// <returns>The format string to apply to top-level window captions.</returns>
		// Token: 0x170001FC RID: 508
		// (get) Token: 0x060005BF RID: 1471 RVA: 0x00010891 File Offset: 0x0000EA91
		// (set) Token: 0x060005C0 RID: 1472 RVA: 0x000108AE File Offset: 0x0000EAAE
		public static string SafeTopLevelCaptionFormat
		{
			get
			{
				if (Application.safeTopLevelCaptionSuffix == null)
				{
					Application.safeTopLevelCaptionSuffix = SR.GetString("SafeTopLevelCaptionFormat");
				}
				return Application.safeTopLevelCaptionSuffix;
			}
			set
			{
				IntSecurity.WindowAdornmentModification.Demand();
				if (value == null)
				{
					value = string.Empty;
				}
				Application.safeTopLevelCaptionSuffix = value;
			}
		}

		/// <summary>Gets the path for the executable file that started the application, not including the executable name.</summary>
		/// <returns>The path for the executable file that started the application.This path will be different depending on whether the Windows Forms application is deployed using ClickOnce. ClickOnce applications are stored in a per-user application cache in the C:\Documents and Settings\username directory. For more information, see Accessing Local and Remote Data in ClickOnce Applications.</returns>
		// Token: 0x170001FD RID: 509
		// (get) Token: 0x060005C1 RID: 1473 RVA: 0x000108CC File Offset: 0x0000EACC
		public static string StartupPath
		{
			get
			{
				if (Application.startupPath == null)
				{
					StringBuilder moduleFileNameLongPath = UnsafeNativeMethods.GetModuleFileNameLongPath(NativeMethods.NullHandleRef);
					Application.startupPath = Path.GetDirectoryName(moduleFileNameLongPath.ToString());
				}
				new FileIOPermission(FileIOPermissionAccess.PathDiscovery, Application.startupPath).Demand();
				return Application.startupPath;
			}
		}

		/// <summary>Unregisters the message loop callback made with <see cref="M:System.Windows.Forms.Application.RegisterMessageLoop(System.Windows.Forms.Application.MessageLoopCallback)" />.</summary>
		// Token: 0x060005C2 RID: 1474 RVA: 0x00010910 File Offset: 0x0000EB10
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void UnregisterMessageLoop()
		{
			Application.ThreadContext.FromCurrent().RegisterMessageLoop(null);
		}

		/// <summary>Gets or sets whether the wait cursor is used for all open forms of the application.</summary>
		/// <returns>
		///     <see langword="true" /> is the wait cursor is used for all open forms; otherwise, <see langword="false" />.</returns>
		// Token: 0x170001FE RID: 510
		// (get) Token: 0x060005C3 RID: 1475 RVA: 0x0001091D File Offset: 0x0000EB1D
		// (set) Token: 0x060005C4 RID: 1476 RVA: 0x00010924 File Offset: 0x0000EB24
		public static bool UseWaitCursor
		{
			get
			{
				return Application.useWaitCursor;
			}
			set
			{
				object collectionSyncRoot = FormCollection.CollectionSyncRoot;
				lock (collectionSyncRoot)
				{
					Application.useWaitCursor = value;
					foreach (object obj in Application.OpenFormsInternal)
					{
						Form form = (Form)obj;
						form.UseWaitCursor = Application.useWaitCursor;
					}
				}
			}
		}

		/// <summary>Gets the path for the application data of a user.</summary>
		/// <returns>The path for the application data of a user.</returns>
		// Token: 0x170001FF RID: 511
		// (get) Token: 0x060005C5 RID: 1477 RVA: 0x000109B0 File Offset: 0x0000EBB0
		public static string UserAppDataPath
		{
			get
			{
				try
				{
					if (ApplicationDeployment.IsNetworkDeployed)
					{
						string text = AppDomain.CurrentDomain.GetData("DataDirectory") as string;
						if (text != null)
						{
							return text;
						}
					}
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsSecurityOrCriticalException(ex))
					{
						throw;
					}
				}
				return Application.GetDataPath(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
			}
		}

		/// <summary>Gets the registry key for the application data of a user.</summary>
		/// <returns>A <see cref="T:Microsoft.Win32.RegistryKey" /> representing the registry key for the application data specific to the user.</returns>
		// Token: 0x17000200 RID: 512
		// (get) Token: 0x060005C6 RID: 1478 RVA: 0x00010A10 File Offset: 0x0000EC10
		public static RegistryKey UserAppDataRegistry
		{
			get
			{
				string format = "Software\\{0}\\{1}\\{2}";
				return Registry.CurrentUser.CreateSubKey(string.Format(CultureInfo.CurrentCulture, format, new object[]
				{
					Application.CompanyName,
					Application.ProductName,
					Application.ProductVersion
				}));
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x060005C7 RID: 1479 RVA: 0x00010A56 File Offset: 0x0000EC56
		internal static bool UseVisualStyles
		{
			get
			{
				return Application.useVisualStyles;
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x060005C8 RID: 1480 RVA: 0x00010A5D File Offset: 0x0000EC5D
		internal static string WindowsFormsVersion
		{
			get
			{
				return "WindowsForms10";
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x060005C9 RID: 1481 RVA: 0x00010A64 File Offset: 0x0000EC64
		internal static string WindowMessagesVersion
		{
			get
			{
				return "WindowsForms12";
			}
		}

		/// <summary>Gets a value that specifies how visual styles are applied to application windows.</summary>
		/// <returns>A bitwise combination of the <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleState" /> values.</returns>
		// Token: 0x17000204 RID: 516
		// (get) Token: 0x060005CA RID: 1482 RVA: 0x00010A6C File Offset: 0x0000EC6C
		// (set) Token: 0x060005CB RID: 1483 RVA: 0x00010A8C File Offset: 0x0000EC8C
		public static VisualStyleState VisualStyleState
		{
			get
			{
				if (!VisualStyleInformation.IsSupportedByOS)
				{
					return VisualStyleState.NoneEnabled;
				}
				return (VisualStyleState)SafeNativeMethods.GetThemeAppProperties();
			}
			set
			{
				if (VisualStyleInformation.IsSupportedByOS)
				{
					if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3) && LocalAppContextSwitches.EnableVisualStyleValidation)
					{
						throw new InvalidEnumArgumentException("value", (int)value, typeof(VisualStyleState));
					}
					SafeNativeMethods.SetThemeAppProperties((int)value);
					SafeNativeMethods.EnumThreadWindowsCallback enumThreadWindowsCallback = new SafeNativeMethods.EnumThreadWindowsCallback(Application.SendThemeChanged);
					SafeNativeMethods.EnumWindows(enumThreadWindowsCallback, IntPtr.Zero);
					GC.KeepAlive(enumThreadWindowsCallback);
				}
			}
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x00010AF4 File Offset: 0x0000ECF4
		private static bool SendThemeChanged(IntPtr handle, IntPtr extraParameter)
		{
			int currentProcessId = SafeNativeMethods.GetCurrentProcessId();
			int num;
			SafeNativeMethods.GetWindowThreadProcessId(new HandleRef(null, handle), out num);
			if (num == currentProcessId && SafeNativeMethods.IsWindowVisible(new HandleRef(null, handle)))
			{
				Application.SendThemeChangedRecursive(handle, IntPtr.Zero);
				SafeNativeMethods.RedrawWindow(new HandleRef(null, handle), null, NativeMethods.NullHandleRef, 1157);
			}
			return true;
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x00010B4D File Offset: 0x0000ED4D
		private static bool SendThemeChangedRecursive(IntPtr handle, IntPtr lparam)
		{
			UnsafeNativeMethods.EnumChildWindows(new HandleRef(null, handle), new NativeMethods.EnumChildrenCallback(Application.SendThemeChangedRecursive), NativeMethods.NullHandleRef);
			UnsafeNativeMethods.SendMessage(new HandleRef(null, handle), 794, 0, 0);
			return true;
		}

		/// <summary>Occurs when the application is about to shut down.</summary>
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060005CE RID: 1486 RVA: 0x00010B82 File Offset: 0x0000ED82
		// (remove) Token: 0x060005CF RID: 1487 RVA: 0x00010B8F File Offset: 0x0000ED8F
		public static event EventHandler ApplicationExit
		{
			add
			{
				Application.AddEventHandler(Application.EVENT_APPLICATIONEXIT, value);
			}
			remove
			{
				Application.RemoveEventHandler(Application.EVENT_APPLICATIONEXIT, value);
			}
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x00010B9C File Offset: 0x0000ED9C
		private static void AddEventHandler(object key, Delegate value)
		{
			object obj = Application.internalSyncObject;
			lock (obj)
			{
				if (Application.eventHandlers == null)
				{
					Application.eventHandlers = new EventHandlerList();
				}
				Application.eventHandlers.AddHandler(key, value);
			}
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x00010BF4 File Offset: 0x0000EDF4
		private static void RemoveEventHandler(object key, Delegate value)
		{
			object obj = Application.internalSyncObject;
			lock (obj)
			{
				if (Application.eventHandlers != null)
				{
					Application.eventHandlers.RemoveHandler(key, value);
				}
			}
		}

		/// <summary>Adds a message filter to monitor Windows messages as they are routed to their destinations.</summary>
		/// <param name="value">The implementation of the <see cref="T:System.Windows.Forms.IMessageFilter" /> interface you want to install. </param>
		// Token: 0x060005D2 RID: 1490 RVA: 0x00010C44 File Offset: 0x0000EE44
		public static void AddMessageFilter(IMessageFilter value)
		{
			IntSecurity.UnmanagedCode.Demand();
			Application.ThreadContext.FromCurrent().AddMessageFilter(value);
		}

		/// <summary>Runs any filters against a window message, and returns a copy of the modified message.</summary>
		/// <param name="message">The Windows event message to filter. </param>
		/// <returns>
		///     <see langword="True" /> if the filters were processed; otherwise, <see langword="false" />.</returns>
		// Token: 0x060005D3 RID: 1491 RVA: 0x00010C5C File Offset: 0x0000EE5C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static bool FilterMessage(ref Message message)
		{
			NativeMethods.MSG msg = default(NativeMethods.MSG);
			msg.hwnd = message.HWnd;
			msg.message = message.Msg;
			msg.wParam = message.WParam;
			msg.lParam = message.LParam;
			bool flag;
			bool result = Application.ThreadContext.FromCurrent().ProcessFilters(ref msg, out flag);
			if (flag)
			{
				message.HWnd = msg.hwnd;
				message.Msg = msg.message;
				message.WParam = msg.wParam;
				message.LParam = msg.lParam;
			}
			return result;
		}

		/// <summary>Occurs when the application finishes processing and is about to enter the idle state.</summary>
		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060005D4 RID: 1492 RVA: 0x00010CE8 File Offset: 0x0000EEE8
		// (remove) Token: 0x060005D5 RID: 1493 RVA: 0x00010D44 File Offset: 0x0000EF44
		public static event EventHandler Idle
		{
			add
			{
				Application.ThreadContext threadContext = Application.ThreadContext.FromCurrent();
				Application.ThreadContext obj = threadContext;
				lock (obj)
				{
					Application.ThreadContext threadContext2 = threadContext;
					threadContext2.idleHandler = (EventHandler)Delegate.Combine(threadContext2.idleHandler, value);
					object componentManager = threadContext.ComponentManager;
				}
			}
			remove
			{
				Application.ThreadContext threadContext = Application.ThreadContext.FromCurrent();
				Application.ThreadContext obj = threadContext;
				lock (obj)
				{
					Application.ThreadContext threadContext2 = threadContext;
					threadContext2.idleHandler = (EventHandler)Delegate.Remove(threadContext2.idleHandler, value);
				}
			}
		}

		/// <summary>Occurs when the application is about to enter a modal state. </summary>
		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060005D6 RID: 1494 RVA: 0x00010D98 File Offset: 0x0000EF98
		// (remove) Token: 0x060005D7 RID: 1495 RVA: 0x00010DEC File Offset: 0x0000EFEC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static event EventHandler EnterThreadModal
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			add
			{
				Application.ThreadContext threadContext = Application.ThreadContext.FromCurrent();
				Application.ThreadContext obj = threadContext;
				lock (obj)
				{
					Application.ThreadContext threadContext2 = threadContext;
					threadContext2.enterModalHandler = (EventHandler)Delegate.Combine(threadContext2.enterModalHandler, value);
				}
			}
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			remove
			{
				Application.ThreadContext threadContext = Application.ThreadContext.FromCurrent();
				Application.ThreadContext obj = threadContext;
				lock (obj)
				{
					Application.ThreadContext threadContext2 = threadContext;
					threadContext2.enterModalHandler = (EventHandler)Delegate.Remove(threadContext2.enterModalHandler, value);
				}
			}
		}

		/// <summary>Occurs when the application is about to leave a modal state. </summary>
		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060005D8 RID: 1496 RVA: 0x00010E40 File Offset: 0x0000F040
		// (remove) Token: 0x060005D9 RID: 1497 RVA: 0x00010E94 File Offset: 0x0000F094
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static event EventHandler LeaveThreadModal
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			add
			{
				Application.ThreadContext threadContext = Application.ThreadContext.FromCurrent();
				Application.ThreadContext obj = threadContext;
				lock (obj)
				{
					Application.ThreadContext threadContext2 = threadContext;
					threadContext2.leaveModalHandler = (EventHandler)Delegate.Combine(threadContext2.leaveModalHandler, value);
				}
			}
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			remove
			{
				Application.ThreadContext threadContext = Application.ThreadContext.FromCurrent();
				Application.ThreadContext obj = threadContext;
				lock (obj)
				{
					Application.ThreadContext threadContext2 = threadContext;
					threadContext2.leaveModalHandler = (EventHandler)Delegate.Remove(threadContext2.leaveModalHandler, value);
				}
			}
		}

		/// <summary>Occurs when an untrapped thread exception is thrown.</summary>
		// Token: 0x14000007 RID: 7
		// (add) Token: 0x060005DA RID: 1498 RVA: 0x00010EE8 File Offset: 0x0000F0E8
		// (remove) Token: 0x060005DB RID: 1499 RVA: 0x00010F34 File Offset: 0x0000F134
		public static event ThreadExceptionEventHandler ThreadException
		{
			add
			{
				IntSecurity.AffectThreadBehavior.Demand();
				Application.ThreadContext threadContext = Application.ThreadContext.FromCurrent();
				Application.ThreadContext obj = threadContext;
				lock (obj)
				{
					threadContext.threadExceptionHandler = value;
				}
			}
			remove
			{
				Application.ThreadContext threadContext = Application.ThreadContext.FromCurrent();
				Application.ThreadContext obj = threadContext;
				lock (obj)
				{
					Application.ThreadContext threadContext2 = threadContext;
					threadContext2.threadExceptionHandler = (ThreadExceptionEventHandler)Delegate.Remove(threadContext2.threadExceptionHandler, value);
				}
			}
		}

		/// <summary>Occurs when a thread is about to shut down. When the main thread for an application is about to be shut down, this event is raised first, followed by an <see cref="E:System.Windows.Forms.Application.ApplicationExit" /> event.</summary>
		// Token: 0x14000008 RID: 8
		// (add) Token: 0x060005DC RID: 1500 RVA: 0x00010F88 File Offset: 0x0000F188
		// (remove) Token: 0x060005DD RID: 1501 RVA: 0x00010F95 File Offset: 0x0000F195
		public static event EventHandler ThreadExit
		{
			add
			{
				Application.AddEventHandler(Application.EVENT_THREADEXIT, value);
			}
			remove
			{
				Application.RemoveEventHandler(Application.EVENT_THREADEXIT, value);
			}
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x00010FA2 File Offset: 0x0000F1A2
		internal static void BeginModalMessageLoop()
		{
			Application.ThreadContext.FromCurrent().BeginModalMessageLoop(null);
		}

		/// <summary>Processes all Windows messages currently in the message queue.</summary>
		// Token: 0x060005DF RID: 1503 RVA: 0x00010FAF File Offset: 0x0000F1AF
		public static void DoEvents()
		{
			Application.ThreadContext.FromCurrent().RunMessageLoop(2, null);
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x00010FBD File Offset: 0x0000F1BD
		internal static void DoEventsModal()
		{
			Application.ThreadContext.FromCurrent().RunMessageLoop(-2, null);
		}

		/// <summary>Enables visual styles for the application.</summary>
		// Token: 0x060005E1 RID: 1505 RVA: 0x00010FCC File Offset: 0x0000F1CC
		public static void EnableVisualStyles()
		{
			string text = null;
			new FileIOPermission(PermissionState.None)
			{
				AllFiles = FileIOPermissionAccess.PathDiscovery
			}.Assert();
			try
			{
				text = typeof(Application).Assembly.Location;
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			if (text != null)
			{
				Application.EnableVisualStylesInternal(text, 101);
			}
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x00011028 File Offset: 0x0000F228
		private static void EnableVisualStylesInternal(string assemblyFileName, int nativeResourceID)
		{
			Application.useVisualStyles = UnsafeNativeMethods.ThemingScope.CreateActivationContext(assemblyFileName, nativeResourceID);
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x00011036 File Offset: 0x0000F236
		internal static void EndModalMessageLoop()
		{
			Application.ThreadContext.FromCurrent().EndModalMessageLoop(null);
		}

		/// <summary>Informs all message pumps that they must terminate, and then closes all application windows after the messages have been processed.</summary>
		// Token: 0x060005E4 RID: 1508 RVA: 0x00011043 File Offset: 0x0000F243
		public static void Exit()
		{
			Application.Exit(null);
		}

		/// <summary>Informs all message pumps that they must terminate, and then closes all application windows after the messages have been processed.</summary>
		/// <param name="e">Returns whether any <see cref="T:System.Windows.Forms.Form" /> within the application cancelled the exit.</param>
		// Token: 0x060005E5 RID: 1509 RVA: 0x0001104C File Offset: 0x0000F24C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static void Exit(CancelEventArgs e)
		{
			Assembly entryAssembly = Assembly.GetEntryAssembly();
			Assembly callingAssembly = Assembly.GetCallingAssembly();
			if (entryAssembly == null || callingAssembly == null || !entryAssembly.Equals(callingAssembly))
			{
				IntSecurity.AffectThreadBehavior.Demand();
			}
			bool cancel = Application.ExitInternal();
			if (e != null)
			{
				e.Cancel = cancel;
			}
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x0001109C File Offset: 0x0000F29C
		private static bool ExitInternal()
		{
			bool flag = false;
			object obj = Application.internalSyncObject;
			lock (obj)
			{
				if (Application.exiting)
				{
					return false;
				}
				Application.exiting = true;
				try
				{
					if (Application.forms != null)
					{
						foreach (object obj2 in Application.OpenFormsInternal)
						{
							Form form = (Form)obj2;
							if (form.RaiseFormClosingOnAppExit())
							{
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						if (Application.forms != null)
						{
							while (Application.OpenFormsInternal.Count > 0)
							{
								Application.OpenFormsInternal[0].RaiseFormClosedOnAppExit();
							}
						}
						Application.ThreadContext.ExitApplication();
					}
				}
				finally
				{
					Application.exiting = false;
				}
			}
			return flag;
		}

		/// <summary>Exits the message loop on the current thread and closes all windows on the thread.</summary>
		// Token: 0x060005E7 RID: 1511 RVA: 0x0001118C File Offset: 0x0000F38C
		public static void ExitThread()
		{
			IntSecurity.AffectThreadBehavior.Demand();
			Application.ExitThreadInternal();
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x000111A0 File Offset: 0x0000F3A0
		private static void ExitThreadInternal()
		{
			Application.ThreadContext threadContext = Application.ThreadContext.FromCurrent();
			if (threadContext.ApplicationContext != null)
			{
				threadContext.ApplicationContext.ExitThread();
				return;
			}
			threadContext.Dispose(true);
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x000111CE File Offset: 0x0000F3CE
		internal static void FormActivated(bool modal, bool activated)
		{
			if (modal)
			{
				return;
			}
			Application.ThreadContext.FromCurrent().FormActivated(activated);
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x000111E0 File Offset: 0x0000F3E0
		private static FileVersionInfo GetAppFileVersionInfo()
		{
			object obj = Application.internalSyncObject;
			lock (obj)
			{
				if (Application.appFileVersion == null)
				{
					Type appMainType = Application.GetAppMainType();
					if (appMainType != null)
					{
						new FileIOPermission(PermissionState.None)
						{
							AllFiles = (FileIOPermissionAccess.Read | FileIOPermissionAccess.PathDiscovery)
						}.Assert();
						try
						{
							Application.appFileVersion = FileVersionInfo.GetVersionInfo(appMainType.Module.FullyQualifiedName);
							goto IL_73;
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
						}
					}
					Application.appFileVersion = FileVersionInfo.GetVersionInfo(Application.ExecutablePath);
				}
			}
			IL_73:
			return (FileVersionInfo)Application.appFileVersion;
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x00011288 File Offset: 0x0000F488
		private static Type GetAppMainType()
		{
			object obj = Application.internalSyncObject;
			lock (obj)
			{
				if (Application.mainType == null)
				{
					Assembly entryAssembly = Assembly.GetEntryAssembly();
					if (entryAssembly != null)
					{
						Application.mainType = entryAssembly.EntryPoint.ReflectedType;
					}
				}
			}
			return Application.mainType;
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x000112F4 File Offset: 0x0000F4F4
		private static Application.ThreadContext GetContextForHandle(HandleRef handle)
		{
			int num;
			int windowThreadProcessId = SafeNativeMethods.GetWindowThreadProcessId(handle, out num);
			return Application.ThreadContext.FromId(windowThreadProcessId);
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x00011314 File Offset: 0x0000F514
		private static string GetDataPath(string basePath)
		{
			string format = "{0}\\{1}\\{2}\\{3}";
			string text = Application.CompanyName;
			string text2 = Application.ProductName;
			string text3 = Application.ProductVersion;
			string text4 = string.Format(CultureInfo.CurrentCulture, format, new object[]
			{
				basePath,
				text,
				text2,
				text3
			});
			object obj = Application.internalSyncObject;
			lock (obj)
			{
				if (!Directory.Exists(text4))
				{
					Directory.CreateDirectory(text4);
				}
			}
			return text4;
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x000113A0 File Offset: 0x0000F5A0
		private static void RaiseExit()
		{
			if (Application.eventHandlers != null)
			{
				Delegate @delegate = Application.eventHandlers[Application.EVENT_APPLICATIONEXIT];
				if (@delegate != null)
				{
					((EventHandler)@delegate)(null, EventArgs.Empty);
				}
			}
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x000113D8 File Offset: 0x0000F5D8
		private static void RaiseThreadExit()
		{
			if (Application.eventHandlers != null)
			{
				Delegate @delegate = Application.eventHandlers[Application.EVENT_THREADEXIT];
				if (@delegate != null)
				{
					((EventHandler)@delegate)(null, EventArgs.Empty);
				}
			}
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x00011410 File Offset: 0x0000F610
		internal static void ParkHandle(HandleRef handle, DpiAwarenessContext dpiAwarenessContext = DpiAwarenessContext.DPI_AWARENESS_CONTEXT_UNSPECIFIED)
		{
			Application.ThreadContext contextForHandle = Application.GetContextForHandle(handle);
			if (contextForHandle != null)
			{
				contextForHandle.GetParkingWindow(dpiAwarenessContext).ParkHandle(handle);
			}
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x00011434 File Offset: 0x0000F634
		internal static void ParkHandle(CreateParams cp, DpiAwarenessContext dpiAwarenessContext = DpiAwarenessContext.DPI_AWARENESS_CONTEXT_UNSPECIFIED)
		{
			Application.ThreadContext threadContext = Application.ThreadContext.FromCurrent();
			if (threadContext != null)
			{
				cp.Parent = threadContext.GetParkingWindow(dpiAwarenessContext).Handle;
			}
		}

		/// <summary>Initializes OLE on the current thread.</summary>
		/// <returns>One of the <see cref="T:System.Threading.ApartmentState" /> values.</returns>
		// Token: 0x060005F2 RID: 1522 RVA: 0x0001145C File Offset: 0x0000F65C
		public static ApartmentState OleRequired()
		{
			return Application.ThreadContext.FromCurrent().OleRequired();
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Application.ThreadException" /> event. </summary>
		/// <param name="t">An <see cref="T:System.Exception" /> that represents the exception that was thrown. </param>
		// Token: 0x060005F3 RID: 1523 RVA: 0x00011468 File Offset: 0x0000F668
		public static void OnThreadException(Exception t)
		{
			Application.ThreadContext.FromCurrent().OnThreadException(t);
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x00011478 File Offset: 0x0000F678
		internal static void UnparkHandle(HandleRef handle, DpiAwarenessContext context)
		{
			Application.ThreadContext contextForHandle = Application.GetContextForHandle(handle);
			if (contextForHandle != null)
			{
				contextForHandle.GetParkingWindow(context).UnparkHandle(handle);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Application.Idle" /> event in hosted scenarios.</summary>
		/// <param name="e">The <see cref="T:System.EventArgs" /> objects to pass to the <see cref="E:System.Windows.Forms.Application.Idle" /> event.</param>
		// Token: 0x060005F5 RID: 1525 RVA: 0x0001149C File Offset: 0x0000F69C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void RaiseIdle(EventArgs e)
		{
			Application.ThreadContext threadContext = Application.ThreadContext.FromCurrent();
			if (threadContext.idleHandler != null)
			{
				threadContext.idleHandler(Thread.CurrentThread, e);
			}
		}

		/// <summary>Removes a message filter from the message pump of the application.</summary>
		/// <param name="value">The implementation of the <see cref="T:System.Windows.Forms.IMessageFilter" /> to remove from the application. </param>
		// Token: 0x060005F6 RID: 1526 RVA: 0x000114C8 File Offset: 0x0000F6C8
		public static void RemoveMessageFilter(IMessageFilter value)
		{
			Application.ThreadContext.FromCurrent().RemoveMessageFilter(value);
		}

		/// <summary>Shuts down the application and starts a new instance immediately.</summary>
		/// <exception cref="T:System.NotSupportedException">Your code is not a Windows Forms application. You cannot call this method in this context.</exception>
		// Token: 0x060005F7 RID: 1527 RVA: 0x000114D8 File Offset: 0x0000F6D8
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void Restart()
		{
			if (Assembly.GetEntryAssembly() == null)
			{
				throw new NotSupportedException(SR.GetString("RestartNotSupported"));
			}
			bool flag = false;
			Process currentProcess = Process.GetCurrentProcess();
			if (string.Equals(currentProcess.MainModule.ModuleName, "ieexec.exe", StringComparison.OrdinalIgnoreCase))
			{
				string str = string.Empty;
				new FileIOPermission(PermissionState.Unrestricted).Assert();
				try
				{
					str = Path.GetDirectoryName(typeof(object).Module.FullyQualifiedName);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				if (string.Equals(str + "\\ieexec.exe", currentProcess.MainModule.FileName, StringComparison.OrdinalIgnoreCase))
				{
					flag = true;
					Application.ExitInternal();
					string text = AppDomain.CurrentDomain.GetData("APP_LAUNCH_URL") as string;
					if (text != null)
					{
						Process.Start(currentProcess.MainModule.FileName, text);
					}
				}
			}
			if (!flag)
			{
				if (ApplicationDeployment.IsNetworkDeployed)
				{
					string updatedApplicationFullName = ApplicationDeployment.CurrentDeployment.UpdatedApplicationFullName;
					uint hostTypeFromMetaData = (uint)Application.ClickOnceUtility.GetHostTypeFromMetaData(updatedApplicationFullName);
					Application.ExitInternal();
					UnsafeNativeMethods.CorLaunchApplication(hostTypeFromMetaData, updatedApplicationFullName, 0, null, 0, null, new UnsafeNativeMethods.PROCESS_INFORMATION());
					return;
				}
				string[] commandLineArgs = Environment.GetCommandLineArgs();
				StringBuilder stringBuilder = new StringBuilder((commandLineArgs.Length - 1) * 16);
				for (int i = 1; i < commandLineArgs.Length - 1; i++)
				{
					stringBuilder.Append('"');
					stringBuilder.Append(commandLineArgs[i]);
					stringBuilder.Append("\" ");
				}
				if (commandLineArgs.Length > 1)
				{
					stringBuilder.Append('"');
					stringBuilder.Append(commandLineArgs[commandLineArgs.Length - 1]);
					stringBuilder.Append('"');
				}
				ProcessStartInfo startInfo = Process.GetCurrentProcess().StartInfo;
				startInfo.FileName = Application.ExecutablePath;
				if (stringBuilder.Length > 0)
				{
					startInfo.Arguments = stringBuilder.ToString();
				}
				Application.ExitInternal();
				Process.Start(startInfo);
			}
		}

		/// <summary>Begins running a standard application message loop on the current thread, without a form.</summary>
		/// <exception cref="T:System.InvalidOperationException">A main message loop is already running on this thread. </exception>
		// Token: 0x060005F8 RID: 1528 RVA: 0x000116B0 File Offset: 0x0000F8B0
		public static void Run()
		{
			Application.ThreadContext.FromCurrent().RunMessageLoop(-1, new ApplicationContext());
		}

		/// <summary>Begins running a standard application message loop on the current thread, and makes the specified form visible.</summary>
		/// <param name="mainForm">A <see cref="T:System.Windows.Forms.Form" /> that represents the form to make visible. </param>
		/// <exception cref="T:System.InvalidOperationException">A main message loop is already running on the current thread. </exception>
		// Token: 0x060005F9 RID: 1529 RVA: 0x000116C2 File Offset: 0x0000F8C2
		public static void Run(Form mainForm)
		{
			Application.ThreadContext.FromCurrent().RunMessageLoop(-1, new ApplicationContext(mainForm));
		}

		/// <summary>Begins running a standard application message loop on the current thread, with an <see cref="T:System.Windows.Forms.ApplicationContext" />.</summary>
		/// <param name="context">An <see cref="T:System.Windows.Forms.ApplicationContext" /> in which the application is run. </param>
		/// <exception cref="T:System.InvalidOperationException">A main message loop is already running on this thread. </exception>
		// Token: 0x060005FA RID: 1530 RVA: 0x000116D5 File Offset: 0x0000F8D5
		public static void Run(ApplicationContext context)
		{
			Application.ThreadContext.FromCurrent().RunMessageLoop(-1, context);
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x000116E3 File Offset: 0x0000F8E3
		internal static void RunDialog(Form form)
		{
			Application.ThreadContext.FromCurrent().RunMessageLoop(4, new Application.ModalApplicationContext(form));
		}

		/// <summary>Sets the application-wide default for the UseCompatibleTextRendering property defined on certain controls.</summary>
		/// <param name="defaultValue">The default value to use for new controls. If <see langword="true" />, new controls that support UseCompatibleTextRendering use the GDI+ based <see cref="T:System.Drawing.Graphics" /> class for text rendering; if <see langword="false" />, new controls use the GDI based <see cref="T:System.Windows.Forms.TextRenderer" /> class.</param>
		/// <exception cref="T:System.InvalidOperationException">You can only call this method before the first window is created by your Windows Forms application. </exception>
		// Token: 0x060005FC RID: 1532 RVA: 0x000116F6 File Offset: 0x0000F8F6
		public static void SetCompatibleTextRenderingDefault(bool defaultValue)
		{
			if (NativeWindow.AnyHandleCreated)
			{
				throw new InvalidOperationException(SR.GetString("Win32WindowAlreadyCreated"));
			}
			Control.UseCompatibleTextRenderingDefault = defaultValue;
		}

		/// <summary>Suspends or hibernates the system, or requests that the system be suspended or hibernated.</summary>
		/// <param name="state">A <see cref="T:System.Windows.Forms.PowerState" /> indicating the power activity mode to which to transition. </param>
		/// <param name="force">
		///       <see langword="true" /> to force the suspended mode immediately; <see langword="false" /> to cause Windows to send a suspend request to every application. </param>
		/// <param name="disableWakeEvent">
		///       <see langword="true" /> to disable restoring the system's power status to active on a wake event, <see langword="false" /> to enable restoring the system's power status to active on a wake event. </param>
		/// <returns>
		///     <see langword="true" /> if the system is being suspended, otherwise, <see langword="false" />.</returns>
		// Token: 0x060005FD RID: 1533 RVA: 0x00011715 File Offset: 0x0000F915
		public static bool SetSuspendState(PowerState state, bool force, bool disableWakeEvent)
		{
			IntSecurity.AffectMachineState.Demand();
			return UnsafeNativeMethods.SetSuspendState(state == PowerState.Hibernate, force, disableWakeEvent);
		}

		/// <summary>Instructs the application how to respond to unhandled exceptions.</summary>
		/// <param name="mode">An <see cref="T:System.Windows.Forms.UnhandledExceptionMode" /> value describing how the application should behave if an exception is thrown without being caught.</param>
		/// <exception cref="T:System.InvalidOperationException">You cannot set the exception mode after the application has created its first window.</exception>
		// Token: 0x060005FE RID: 1534 RVA: 0x0001172C File Offset: 0x0000F92C
		public static void SetUnhandledExceptionMode(UnhandledExceptionMode mode)
		{
			Application.SetUnhandledExceptionMode(mode, true);
		}

		/// <summary>Instructs the application how to respond to unhandled exceptions, optionally applying thread-specific behavior.</summary>
		/// <param name="mode">An <see cref="T:System.Windows.Forms.UnhandledExceptionMode" /> value describing how the application should behave if an exception is thrown without being caught.</param>
		/// <param name="threadScope">
		///       <see langword="true" /> to set the thread exception mode; otherwise, <see langword="false" />.</param>
		/// <exception cref="T:System.InvalidOperationException">You cannot set the exception mode after the application has created its first window.</exception>
		// Token: 0x060005FF RID: 1535 RVA: 0x00011735 File Offset: 0x0000F935
		public static void SetUnhandledExceptionMode(UnhandledExceptionMode mode, bool threadScope)
		{
			IntSecurity.AffectThreadBehavior.Demand();
			NativeWindow.SetUnhandledExceptionModeInternal(mode, threadScope);
		}

		// Token: 0x0400051E RID: 1310
		private static EventHandlerList eventHandlers;

		// Token: 0x0400051F RID: 1311
		private static string startupPath;

		// Token: 0x04000520 RID: 1312
		private static string executablePath;

		// Token: 0x04000521 RID: 1313
		private static object appFileVersion;

		// Token: 0x04000522 RID: 1314
		private static Type mainType;

		// Token: 0x04000523 RID: 1315
		private static string companyName;

		// Token: 0x04000524 RID: 1316
		private static string productName;

		// Token: 0x04000525 RID: 1317
		private static string productVersion;

		// Token: 0x04000526 RID: 1318
		private static string safeTopLevelCaptionSuffix;

		// Token: 0x04000527 RID: 1319
		private static bool useVisualStyles = false;

		// Token: 0x04000528 RID: 1320
		private static bool comCtlSupportsVisualStylesInitialized = false;

		// Token: 0x04000529 RID: 1321
		private static bool comCtlSupportsVisualStyles = false;

		// Token: 0x0400052A RID: 1322
		private static FormCollection forms = null;

		// Token: 0x0400052B RID: 1323
		private static object internalSyncObject = new object();

		// Token: 0x0400052C RID: 1324
		private static bool useWaitCursor = false;

		// Token: 0x0400052D RID: 1325
		private static bool useEverettThreadAffinity = false;

		// Token: 0x0400052E RID: 1326
		private static bool checkedThreadAffinity = false;

		// Token: 0x0400052F RID: 1327
		private const string everettThreadAffinityValue = "EnableSystemEventsThreadAffinityCompatibility";

		// Token: 0x04000530 RID: 1328
		private static bool exiting;

		// Token: 0x04000531 RID: 1329
		private static readonly object EVENT_APPLICATIONEXIT = new object();

		// Token: 0x04000532 RID: 1330
		private static readonly object EVENT_THREADEXIT = new object();

		// Token: 0x04000533 RID: 1331
		private const string IEEXEC = "ieexec.exe";

		// Token: 0x04000534 RID: 1332
		private const string CLICKONCE_APPS_DATADIRECTORY = "DataDirectory";

		// Token: 0x04000535 RID: 1333
		private static bool parkingWindowSupportsPMAv2 = true;

		/// <summary>Represents a method that will check whether the hosting environment is still sending messages. </summary>
		/// <returns>
		///     <see langword="true" /> if the hosting environment is still sending messages; otherwise, <see langword="false" />.</returns>
		// Token: 0x02000543 RID: 1347
		// (Invoke) Token: 0x060054F5 RID: 21749
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public delegate bool MessageLoopCallback();

		// Token: 0x02000544 RID: 1348
		private class ClickOnceUtility
		{
			// Token: 0x060054F8 RID: 21752 RVA: 0x000027DB File Offset: 0x000009DB
			private ClickOnceUtility()
			{
			}

			// Token: 0x060054F9 RID: 21753 RVA: 0x00164958 File Offset: 0x00162B58
			public static Application.ClickOnceUtility.HostType GetHostTypeFromMetaData(string appFullName)
			{
				Application.ClickOnceUtility.HostType result = Application.ClickOnceUtility.HostType.Default;
				try
				{
					IDefinitionAppId appId = IsolationInterop.AppIdAuthority.TextToDefinition(0U, appFullName);
					result = (Application.ClickOnceUtility.GetPropertyBoolean(appId, "IsFullTrust") ? Application.ClickOnceUtility.HostType.CorFlag : Application.ClickOnceUtility.HostType.AppLaunch);
				}
				catch
				{
				}
				return result;
			}

			// Token: 0x060054FA RID: 21754 RVA: 0x001649A0 File Offset: 0x00162BA0
			private static bool GetPropertyBoolean(IDefinitionAppId appId, string propName)
			{
				string propertyString = Application.ClickOnceUtility.GetPropertyString(appId, propName);
				if (string.IsNullOrEmpty(propertyString))
				{
					return false;
				}
				bool result;
				try
				{
					result = Convert.ToBoolean(propertyString, CultureInfo.InvariantCulture);
				}
				catch
				{
					result = false;
				}
				return result;
			}

			// Token: 0x060054FB RID: 21755 RVA: 0x001649E4 File Offset: 0x00162BE4
			private static string GetPropertyString(IDefinitionAppId appId, string propName)
			{
				byte[] deploymentProperty = IsolationInterop.UserStore.GetDeploymentProperty(Store.GetPackagePropertyFlags.Nothing, appId, Application.ClickOnceUtility.InstallReference, new Guid("2ad613da-6fdb-4671-af9e-18ab2e4df4d8"), propName);
				int num = deploymentProperty.Length;
				if (num == 0 || deploymentProperty.Length % 2 != 0 || deploymentProperty[num - 2] != 0 || deploymentProperty[num - 1] != 0)
				{
					return null;
				}
				return Encoding.Unicode.GetString(deploymentProperty, 0, num - 2);
			}

			// Token: 0x17001461 RID: 5217
			// (get) Token: 0x060054FC RID: 21756 RVA: 0x00164A3B File Offset: 0x00162C3B
			private static StoreApplicationReference InstallReference
			{
				get
				{
					return new StoreApplicationReference(IsolationInterop.GUID_SXS_INSTALL_REFERENCE_SCHEME_OPAQUESTRING, "{3f471841-eef2-47d6-89c0-d028f03a4ad5}", null);
				}
			}

			// Token: 0x02000888 RID: 2184
			public enum HostType
			{
				// Token: 0x040043DF RID: 17375
				Default,
				// Token: 0x040043E0 RID: 17376
				AppLaunch,
				// Token: 0x040043E1 RID: 17377
				CorFlag
			}
		}

		// Token: 0x02000545 RID: 1349
		private class ComponentManager : UnsafeNativeMethods.IMsoComponentManager
		{
			// Token: 0x17001462 RID: 5218
			// (get) Token: 0x060054FD RID: 21757 RVA: 0x00164A4D File Offset: 0x00162C4D
			private Hashtable OleComponents
			{
				get
				{
					if (this.oleComponents == null)
					{
						this.oleComponents = new Hashtable();
						this.cookieCounter = 0;
					}
					return this.oleComponents;
				}
			}

			// Token: 0x060054FE RID: 21758 RVA: 0x00164A6F File Offset: 0x00162C6F
			int UnsafeNativeMethods.IMsoComponentManager.QueryService(ref Guid guidService, ref Guid iid, out object ppvObj)
			{
				ppvObj = null;
				return -2147467262;
			}

			// Token: 0x060054FF RID: 21759 RVA: 0x0000E214 File Offset: 0x0000C414
			bool UnsafeNativeMethods.IMsoComponentManager.FDebugMessage(IntPtr hInst, int msg, IntPtr wparam, IntPtr lparam)
			{
				return true;
			}

			// Token: 0x06005500 RID: 21760 RVA: 0x00164A7C File Offset: 0x00162C7C
			bool UnsafeNativeMethods.IMsoComponentManager.FRegisterComponent(UnsafeNativeMethods.IMsoComponent component, NativeMethods.MSOCRINFOSTRUCT pcrinfo, out IntPtr dwComponentID)
			{
				Application.ComponentManager.ComponentHashtableEntry componentHashtableEntry = new Application.ComponentManager.ComponentHashtableEntry();
				componentHashtableEntry.component = component;
				componentHashtableEntry.componentInfo = pcrinfo;
				Hashtable hashtable = this.OleComponents;
				int num = this.cookieCounter + 1;
				this.cookieCounter = num;
				hashtable.Add(num, componentHashtableEntry);
				dwComponentID = (IntPtr)this.cookieCounter;
				return true;
			}

			// Token: 0x06005501 RID: 21761 RVA: 0x00164AD0 File Offset: 0x00162CD0
			bool UnsafeNativeMethods.IMsoComponentManager.FRevokeComponent(IntPtr dwComponentID)
			{
				int num = (int)((long)dwComponentID);
				Application.ComponentManager.ComponentHashtableEntry componentHashtableEntry = (Application.ComponentManager.ComponentHashtableEntry)this.OleComponents[num];
				if (componentHashtableEntry == null)
				{
					return false;
				}
				if (componentHashtableEntry.component == this.activeComponent)
				{
					this.activeComponent = null;
				}
				if (componentHashtableEntry.component == this.trackingComponent)
				{
					this.trackingComponent = null;
				}
				this.OleComponents.Remove(num);
				return true;
			}

			// Token: 0x06005502 RID: 21762 RVA: 0x00164B40 File Offset: 0x00162D40
			bool UnsafeNativeMethods.IMsoComponentManager.FUpdateComponentRegistration(IntPtr dwComponentID, NativeMethods.MSOCRINFOSTRUCT info)
			{
				int num = (int)((long)dwComponentID);
				Application.ComponentManager.ComponentHashtableEntry componentHashtableEntry = (Application.ComponentManager.ComponentHashtableEntry)this.OleComponents[num];
				if (componentHashtableEntry == null)
				{
					return false;
				}
				componentHashtableEntry.componentInfo = info;
				return true;
			}

			// Token: 0x06005503 RID: 21763 RVA: 0x00164B7C File Offset: 0x00162D7C
			bool UnsafeNativeMethods.IMsoComponentManager.FOnComponentActivate(IntPtr dwComponentID)
			{
				int num = (int)((long)dwComponentID);
				Application.ComponentManager.ComponentHashtableEntry componentHashtableEntry = (Application.ComponentManager.ComponentHashtableEntry)this.OleComponents[num];
				if (componentHashtableEntry == null)
				{
					return false;
				}
				this.activeComponent = componentHashtableEntry.component;
				return true;
			}

			// Token: 0x06005504 RID: 21764 RVA: 0x00164BBC File Offset: 0x00162DBC
			bool UnsafeNativeMethods.IMsoComponentManager.FSetTrackingComponent(IntPtr dwComponentID, bool fTrack)
			{
				int num = (int)((long)dwComponentID);
				Application.ComponentManager.ComponentHashtableEntry componentHashtableEntry = (Application.ComponentManager.ComponentHashtableEntry)this.OleComponents[num];
				if (componentHashtableEntry == null)
				{
					return false;
				}
				if (componentHashtableEntry.component == this.trackingComponent ^ fTrack)
				{
					return false;
				}
				if (fTrack)
				{
					this.trackingComponent = componentHashtableEntry.component;
				}
				else
				{
					this.trackingComponent = null;
				}
				return true;
			}

			// Token: 0x06005505 RID: 21765 RVA: 0x00164C1C File Offset: 0x00162E1C
			void UnsafeNativeMethods.IMsoComponentManager.OnComponentEnterState(IntPtr dwComponentID, int uStateID, int uContext, int cpicmExclude, int rgpicmExclude, int dwReserved)
			{
				int num = (int)((long)dwComponentID);
				this.currentState |= uStateID;
				if (uContext == 0 || uContext == 1)
				{
					foreach (object obj in this.OleComponents.Values)
					{
						Application.ComponentManager.ComponentHashtableEntry componentHashtableEntry = (Application.ComponentManager.ComponentHashtableEntry)obj;
						componentHashtableEntry.component.OnEnterState(uStateID, true);
					}
				}
			}

			// Token: 0x06005506 RID: 21766 RVA: 0x00164CA0 File Offset: 0x00162EA0
			bool UnsafeNativeMethods.IMsoComponentManager.FOnComponentExitState(IntPtr dwComponentID, int uStateID, int uContext, int cpicmExclude, int rgpicmExclude)
			{
				int num = (int)((long)dwComponentID);
				this.currentState &= ~uStateID;
				if (uContext == 0 || uContext == 1)
				{
					foreach (object obj in this.OleComponents.Values)
					{
						Application.ComponentManager.ComponentHashtableEntry componentHashtableEntry = (Application.ComponentManager.ComponentHashtableEntry)obj;
						componentHashtableEntry.component.OnEnterState(uStateID, false);
					}
				}
				return false;
			}

			// Token: 0x06005507 RID: 21767 RVA: 0x00164D24 File Offset: 0x00162F24
			bool UnsafeNativeMethods.IMsoComponentManager.FInState(int uStateID, IntPtr pvoid)
			{
				return (this.currentState & uStateID) != 0;
			}

			// Token: 0x06005508 RID: 21768 RVA: 0x00164D34 File Offset: 0x00162F34
			bool UnsafeNativeMethods.IMsoComponentManager.FContinueIdle()
			{
				NativeMethods.MSG msg = default(NativeMethods.MSG);
				return !UnsafeNativeMethods.PeekMessage(ref msg, NativeMethods.NullHandleRef, 0, 0, 0);
			}

			// Token: 0x06005509 RID: 21769 RVA: 0x00164D5C File Offset: 0x00162F5C
			bool UnsafeNativeMethods.IMsoComponentManager.FPushMessageLoop(IntPtr dwComponentID, int reason, int pvLoopData)
			{
				int num = (int)((long)dwComponentID);
				int num2 = this.currentState;
				bool flag = true;
				if (!this.OleComponents.ContainsKey(num))
				{
					return false;
				}
				UnsafeNativeMethods.IMsoComponent msoComponent = this.activeComponent;
				try
				{
					NativeMethods.MSG msg = default(NativeMethods.MSG);
					NativeMethods.MSG[] array = new NativeMethods.MSG[]
					{
						msg
					};
					Application.ComponentManager.ComponentHashtableEntry componentHashtableEntry = (Application.ComponentManager.ComponentHashtableEntry)this.OleComponents[num];
					if (componentHashtableEntry == null)
					{
						return false;
					}
					UnsafeNativeMethods.IMsoComponent component = componentHashtableEntry.component;
					this.activeComponent = component;
					while (flag)
					{
						UnsafeNativeMethods.IMsoComponent msoComponent2;
						if (this.trackingComponent != null)
						{
							msoComponent2 = this.trackingComponent;
						}
						else if (this.activeComponent != null)
						{
							msoComponent2 = this.activeComponent;
						}
						else
						{
							msoComponent2 = component;
						}
						bool flag2 = UnsafeNativeMethods.PeekMessage(ref msg, NativeMethods.NullHandleRef, 0, 0, 0);
						if (flag2)
						{
							array[0] = msg;
							flag = msoComponent2.FContinueMessageLoop(reason, pvLoopData, array);
							if (flag)
							{
								bool flag3;
								if (msg.hwnd != IntPtr.Zero && SafeNativeMethods.IsWindowUnicode(new HandleRef(null, msg.hwnd)))
								{
									flag3 = true;
									UnsafeNativeMethods.GetMessageW(ref msg, NativeMethods.NullHandleRef, 0, 0);
								}
								else
								{
									flag3 = false;
									UnsafeNativeMethods.GetMessageA(ref msg, NativeMethods.NullHandleRef, 0, 0);
								}
								if (msg.message == 18)
								{
									Application.ThreadContext.FromCurrent().DisposeThreadWindows();
									if (reason != -1)
									{
										UnsafeNativeMethods.PostQuitMessage((int)msg.wParam);
									}
									flag = false;
									break;
								}
								if (!msoComponent2.FPreTranslateMessage(ref msg))
								{
									UnsafeNativeMethods.TranslateMessage(ref msg);
									if (flag3)
									{
										UnsafeNativeMethods.DispatchMessageW(ref msg);
									}
									else
									{
										UnsafeNativeMethods.DispatchMessageA(ref msg);
									}
								}
							}
						}
						else
						{
							if (reason == 2)
							{
								break;
							}
							if (reason == -2)
							{
								break;
							}
							bool flag4 = false;
							if (this.OleComponents != null)
							{
								foreach (object obj in this.OleComponents.Values)
								{
									Application.ComponentManager.ComponentHashtableEntry componentHashtableEntry2 = (Application.ComponentManager.ComponentHashtableEntry)obj;
									flag4 |= componentHashtableEntry2.component.FDoIdle(-1);
								}
							}
							flag = msoComponent2.FContinueMessageLoop(reason, pvLoopData, null);
							if (flag)
							{
								if (flag4)
								{
									UnsafeNativeMethods.MsgWaitForMultipleObjectsEx(0, IntPtr.Zero, 100, 255, 4);
								}
								else if (!UnsafeNativeMethods.PeekMessage(ref msg, NativeMethods.NullHandleRef, 0, 0, 0))
								{
									UnsafeNativeMethods.WaitMessage();
								}
							}
						}
					}
				}
				finally
				{
					this.currentState = num2;
					this.activeComponent = msoComponent;
				}
				return !flag;
			}

			// Token: 0x0600550A RID: 21770 RVA: 0x00164FCC File Offset: 0x001631CC
			bool UnsafeNativeMethods.IMsoComponentManager.FCreateSubComponentManager(object punkOuter, object punkServProv, ref Guid riid, out IntPtr ppvObj)
			{
				ppvObj = IntPtr.Zero;
				return false;
			}

			// Token: 0x0600550B RID: 21771 RVA: 0x00164FD7 File Offset: 0x001631D7
			bool UnsafeNativeMethods.IMsoComponentManager.FGetParentComponentManager(out UnsafeNativeMethods.IMsoComponentManager ppicm)
			{
				ppicm = null;
				return false;
			}

			// Token: 0x0600550C RID: 21772 RVA: 0x00164FE0 File Offset: 0x001631E0
			bool UnsafeNativeMethods.IMsoComponentManager.FGetActiveComponent(int dwgac, UnsafeNativeMethods.IMsoComponent[] ppic, NativeMethods.MSOCRINFOSTRUCT info, int dwReserved)
			{
				UnsafeNativeMethods.IMsoComponent msoComponent = null;
				if (dwgac == 0)
				{
					msoComponent = this.activeComponent;
				}
				else if (dwgac == 1)
				{
					msoComponent = this.trackingComponent;
				}
				else if (dwgac == 2)
				{
					if (this.trackingComponent != null)
					{
						msoComponent = this.trackingComponent;
					}
					else
					{
						msoComponent = this.activeComponent;
					}
				}
				if (ppic != null)
				{
					ppic[0] = msoComponent;
				}
				if (info != null && msoComponent != null)
				{
					foreach (object obj in this.OleComponents.Values)
					{
						Application.ComponentManager.ComponentHashtableEntry componentHashtableEntry = (Application.ComponentManager.ComponentHashtableEntry)obj;
						if (componentHashtableEntry.component == msoComponent)
						{
							info = componentHashtableEntry.componentInfo;
							break;
						}
					}
				}
				return msoComponent != null;
			}

			// Token: 0x0400376A RID: 14186
			private Hashtable oleComponents;

			// Token: 0x0400376B RID: 14187
			private int cookieCounter;

			// Token: 0x0400376C RID: 14188
			private UnsafeNativeMethods.IMsoComponent activeComponent;

			// Token: 0x0400376D RID: 14189
			private UnsafeNativeMethods.IMsoComponent trackingComponent;

			// Token: 0x0400376E RID: 14190
			private int currentState;

			// Token: 0x02000889 RID: 2185
			private class ComponentHashtableEntry
			{
				// Token: 0x040043E2 RID: 17378
				public UnsafeNativeMethods.IMsoComponent component;

				// Token: 0x040043E3 RID: 17379
				public NativeMethods.MSOCRINFOSTRUCT componentInfo;
			}
		}

		// Token: 0x02000546 RID: 1350
		internal sealed class ThreadContext : MarshalByRefObject, UnsafeNativeMethods.IMsoComponent
		{
			// Token: 0x0600550E RID: 21774 RVA: 0x00165094 File Offset: 0x00163294
			public ThreadContext()
			{
				IntPtr zero = IntPtr.Zero;
				UnsafeNativeMethods.DuplicateHandle(new HandleRef(null, SafeNativeMethods.GetCurrentProcess()), new HandleRef(null, SafeNativeMethods.GetCurrentThread()), new HandleRef(null, SafeNativeMethods.GetCurrentProcess()), ref zero, 0, false, 2);
				this.handle = zero;
				this.id = SafeNativeMethods.GetCurrentThreadId();
				this.messageLoopCount = 0;
				Application.ThreadContext.currentThreadContext = this;
				Application.ThreadContext.contextHash[this.id] = this;
			}

			// Token: 0x17001463 RID: 5219
			// (get) Token: 0x0600550F RID: 21775 RVA: 0x00165120 File Offset: 0x00163320
			public ApplicationContext ApplicationContext
			{
				get
				{
					return this.applicationContext;
				}
			}

			// Token: 0x17001464 RID: 5220
			// (get) Token: 0x06005510 RID: 21776 RVA: 0x00165128 File Offset: 0x00163328
			internal UnsafeNativeMethods.IMsoComponentManager ComponentManager
			{
				get
				{
					if (this.componentManager == null)
					{
						if (this.fetchingComponentManager)
						{
							return null;
						}
						this.fetchingComponentManager = true;
						try
						{
							UnsafeNativeMethods.IMsoComponentManager msoComponentManager = null;
							Application.OleRequired();
							IntPtr intPtr = (IntPtr)0;
							if (NativeMethods.Succeeded(UnsafeNativeMethods.CoRegisterMessageFilter(NativeMethods.NullHandleRef, ref intPtr)) && intPtr != (IntPtr)0)
							{
								IntPtr intPtr2 = (IntPtr)0;
								UnsafeNativeMethods.CoRegisterMessageFilter(new HandleRef(null, intPtr), ref intPtr2);
								object obj = Marshal.GetObjectForIUnknown(intPtr);
								Marshal.Release(intPtr);
								UnsafeNativeMethods.IOleServiceProvider oleServiceProvider = obj as UnsafeNativeMethods.IOleServiceProvider;
								if (oleServiceProvider != null)
								{
									try
									{
										IntPtr zero = IntPtr.Zero;
										Guid guid = new Guid("000C060B-0000-0000-C000-000000000046");
										Guid guid2 = new Guid("{000C0601-0000-0000-C000-000000000046}");
										int hr = oleServiceProvider.QueryService(ref guid, ref guid2, out zero);
										if (NativeMethods.Succeeded(hr) && zero != IntPtr.Zero)
										{
											IntPtr intPtr3;
											try
											{
												Guid guid3 = typeof(UnsafeNativeMethods.IMsoComponentManager).GUID;
												hr = Marshal.QueryInterface(zero, ref guid3, out intPtr3);
											}
											finally
											{
												Marshal.Release(zero);
											}
											if (NativeMethods.Succeeded(hr) && intPtr3 != IntPtr.Zero)
											{
												try
												{
													msoComponentManager = ComponentManagerBroker.GetComponentManager(intPtr3);
												}
												finally
												{
													Marshal.Release(intPtr3);
												}
											}
											if (msoComponentManager != null)
											{
												if (intPtr == zero)
												{
													obj = null;
												}
												this.externalComponentManager = true;
												AppDomain.CurrentDomain.DomainUnload += this.OnDomainUnload;
												AppDomain.CurrentDomain.ProcessExit += this.OnDomainUnload;
											}
										}
									}
									catch
									{
									}
								}
								if (obj != null && Marshal.IsComObject(obj))
								{
									Marshal.ReleaseComObject(obj);
								}
							}
							if (msoComponentManager == null)
							{
								msoComponentManager = new Application.ComponentManager();
								this.externalComponentManager = false;
							}
							if (msoComponentManager != null && this.componentID == -1)
							{
								IntPtr value;
								bool flag = msoComponentManager.FRegisterComponent(this, new NativeMethods.MSOCRINFOSTRUCT
								{
									cbSize = Marshal.SizeOf(typeof(NativeMethods.MSOCRINFOSTRUCT)),
									uIdleTimeInterval = 0,
									grfcrf = 9,
									grfcadvf = 1
								}, out value);
								this.componentID = (int)((long)value);
								if (flag && !(msoComponentManager is Application.ComponentManager))
								{
									this.messageLoopCount++;
								}
								this.componentManager = msoComponentManager;
							}
						}
						finally
						{
							this.fetchingComponentManager = false;
						}
					}
					return this.componentManager;
				}
			}

			// Token: 0x17001465 RID: 5221
			// (get) Token: 0x06005511 RID: 21777 RVA: 0x001653B4 File Offset: 0x001635B4
			internal bool CustomThreadExceptionHandlerAttached
			{
				get
				{
					return this.threadExceptionHandler != null;
				}
			}

			// Token: 0x06005512 RID: 21778 RVA: 0x001653C0 File Offset: 0x001635C0
			internal Application.ParkingWindow GetParkingWindow(DpiAwarenessContext context)
			{
				Application.ParkingWindow result;
				lock (this)
				{
					Application.ParkingWindow parkingWindow = this.GetParkingWindowForContext(context);
					if (parkingWindow == null)
					{
						IntSecurity.ManipulateWndProcAndHandles.Assert();
						try
						{
							using (DpiHelper.EnterDpiAwarenessScope(context))
							{
								parkingWindow = new Application.ParkingWindow();
							}
							this.parkingWindows.Add(parkingWindow);
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
						}
					}
					result = parkingWindow;
				}
				return result;
			}

			// Token: 0x06005513 RID: 21779 RVA: 0x00165454 File Offset: 0x00163654
			internal Application.ParkingWindow GetParkingWindowForContext(DpiAwarenessContext context)
			{
				if (this.parkingWindows.Count == 0)
				{
					return null;
				}
				if (!DpiHelper.EnableDpiChangedHighDpiImprovements || CommonUnsafeNativeMethods.TryFindDpiAwarenessContextsEqual(context, DpiAwarenessContext.DPI_AWARENESS_CONTEXT_UNSPECIFIED))
				{
					return this.parkingWindows[0];
				}
				foreach (Application.ParkingWindow parkingWindow in this.parkingWindows)
				{
					if (CommonUnsafeNativeMethods.TryFindDpiAwarenessContextsEqual(parkingWindow.DpiAwarenessContext, context))
					{
						return parkingWindow;
					}
				}
				return null;
			}

			// Token: 0x17001466 RID: 5222
			// (get) Token: 0x06005514 RID: 21780 RVA: 0x001654E4 File Offset: 0x001636E4
			// (set) Token: 0x06005515 RID: 21781 RVA: 0x0016550D File Offset: 0x0016370D
			internal Control ActivatingControl
			{
				get
				{
					if (this.activatingControlRef != null && this.activatingControlRef.IsAlive)
					{
						return this.activatingControlRef.Target as Control;
					}
					return null;
				}
				set
				{
					if (value != null)
					{
						this.activatingControlRef = new WeakReference(value);
						return;
					}
					this.activatingControlRef = null;
				}
			}

			// Token: 0x17001467 RID: 5223
			// (get) Token: 0x06005516 RID: 21782 RVA: 0x00165528 File Offset: 0x00163728
			internal Control MarshalingControl
			{
				get
				{
					Control result;
					lock (this)
					{
						if (this.marshalingControl == null)
						{
							this.marshalingControl = new Application.MarshalingControl();
						}
						result = this.marshalingControl;
					}
					return result;
				}
			}

			// Token: 0x06005517 RID: 21783 RVA: 0x00165578 File Offset: 0x00163778
			internal void AddMessageFilter(IMessageFilter f)
			{
				if (this.messageFilters == null)
				{
					this.messageFilters = new ArrayList();
				}
				if (f != null)
				{
					this.SetState(16, false);
					if (this.messageFilters.Count > 0 && f is IMessageModifyAndFilter)
					{
						this.messageFilters.Insert(0, f);
						return;
					}
					this.messageFilters.Add(f);
				}
			}

			// Token: 0x06005518 RID: 21784 RVA: 0x001655D8 File Offset: 0x001637D8
			internal void BeginModalMessageLoop(ApplicationContext context)
			{
				bool flag = this.ourModalLoop;
				this.ourModalLoop = true;
				try
				{
					UnsafeNativeMethods.IMsoComponentManager msoComponentManager = this.ComponentManager;
					if (msoComponentManager != null)
					{
						msoComponentManager.OnComponentEnterState((IntPtr)this.componentID, 1, 0, 0, 0, 0);
					}
				}
				finally
				{
					this.ourModalLoop = flag;
				}
				this.DisableWindowsForModalLoop(false, context);
				this.modalCount++;
				if (this.enterModalHandler != null && this.modalCount == 1)
				{
					this.enterModalHandler(Thread.CurrentThread, EventArgs.Empty);
				}
			}

			// Token: 0x06005519 RID: 21785 RVA: 0x0016566C File Offset: 0x0016386C
			internal void DisableWindowsForModalLoop(bool onlyWinForms, ApplicationContext context)
			{
				Application.ThreadWindows previousThreadWindows = this.threadWindows;
				this.threadWindows = new Application.ThreadWindows(onlyWinForms);
				this.threadWindows.Enable(false);
				this.threadWindows.previousThreadWindows = previousThreadWindows;
				Application.ModalApplicationContext modalApplicationContext = context as Application.ModalApplicationContext;
				if (modalApplicationContext != null)
				{
					modalApplicationContext.DisableThreadWindows(true, onlyWinForms);
				}
			}

			// Token: 0x0600551A RID: 21786 RVA: 0x001656B8 File Offset: 0x001638B8
			internal void Dispose(bool postQuit)
			{
				lock (this)
				{
					try
					{
						int num = this.disposeCount;
						this.disposeCount = num + 1;
						if (num == 0)
						{
							if (this.messageLoopCount > 0 && postQuit)
							{
								this.PostQuit();
							}
							else
							{
								bool flag2 = SafeNativeMethods.GetCurrentThreadId() == this.id;
								try
								{
									if (flag2)
									{
										if (this.componentManager != null)
										{
											this.RevokeComponent();
										}
										this.DisposeThreadWindows();
										try
										{
											Application.RaiseThreadExit();
										}
										finally
										{
											if (this.GetState(1) && !this.GetState(2))
											{
												this.SetState(1, false);
												UnsafeNativeMethods.OleUninitialize();
											}
										}
									}
								}
								finally
								{
									if (this.handle != IntPtr.Zero)
									{
										UnsafeNativeMethods.CloseHandle(new HandleRef(this, this.handle));
										this.handle = IntPtr.Zero;
									}
									try
									{
										if (Application.ThreadContext.totalMessageLoopCount == 0)
										{
											Application.RaiseExit();
										}
									}
									finally
									{
										object obj = Application.ThreadContext.tcInternalSyncObject;
										lock (obj)
										{
											Application.ThreadContext.contextHash.Remove(this.id);
										}
										if (Application.ThreadContext.currentThreadContext == this)
										{
											Application.ThreadContext.currentThreadContext = null;
										}
									}
								}
							}
							GC.SuppressFinalize(this);
						}
					}
					finally
					{
						this.disposeCount--;
					}
				}
			}

			// Token: 0x0600551B RID: 21787 RVA: 0x00165890 File Offset: 0x00163A90
			private void DisposeParkingWindow()
			{
				if (this.parkingWindows.Count != 0)
				{
					int num;
					int windowThreadProcessId = SafeNativeMethods.GetWindowThreadProcessId(new HandleRef(this.parkingWindows[0], this.parkingWindows[0].Handle), out num);
					int currentThreadId = SafeNativeMethods.GetCurrentThreadId();
					for (int i = 0; i < this.parkingWindows.Count; i++)
					{
						if (windowThreadProcessId == currentThreadId)
						{
							this.parkingWindows[i].Destroy();
						}
						else
						{
							this.parkingWindows[i] = null;
						}
					}
					this.parkingWindows.Clear();
				}
			}

			// Token: 0x0600551C RID: 21788 RVA: 0x00165920 File Offset: 0x00163B20
			internal void DisposeThreadWindows()
			{
				try
				{
					if (this.applicationContext != null)
					{
						this.applicationContext.Dispose();
						this.applicationContext = null;
					}
					Application.ThreadWindows threadWindows = new Application.ThreadWindows(true);
					threadWindows.Dispose();
					this.DisposeParkingWindow();
				}
				catch
				{
				}
			}

			// Token: 0x0600551D RID: 21789 RVA: 0x00165970 File Offset: 0x00163B70
			internal void EnableWindowsForModalLoop(bool onlyWinForms, ApplicationContext context)
			{
				if (this.threadWindows != null)
				{
					this.threadWindows.Enable(true);
					this.threadWindows = this.threadWindows.previousThreadWindows;
				}
				Application.ModalApplicationContext modalApplicationContext = context as Application.ModalApplicationContext;
				if (modalApplicationContext != null)
				{
					modalApplicationContext.DisableThreadWindows(false, onlyWinForms);
				}
			}

			// Token: 0x0600551E RID: 21790 RVA: 0x001659B4 File Offset: 0x00163BB4
			internal void EndModalMessageLoop(ApplicationContext context)
			{
				this.EnableWindowsForModalLoop(false, context);
				bool flag = this.ourModalLoop;
				this.ourModalLoop = true;
				try
				{
					UnsafeNativeMethods.IMsoComponentManager msoComponentManager = this.ComponentManager;
					if (msoComponentManager != null)
					{
						msoComponentManager.FOnComponentExitState((IntPtr)this.componentID, 1, 0, 0, 0);
					}
				}
				finally
				{
					this.ourModalLoop = flag;
				}
				this.modalCount--;
				if (this.leaveModalHandler != null && this.modalCount == 0)
				{
					this.leaveModalHandler(Thread.CurrentThread, EventArgs.Empty);
				}
			}

			// Token: 0x0600551F RID: 21791 RVA: 0x00165A44 File Offset: 0x00163C44
			internal static void ExitApplication()
			{
				Application.ThreadContext.ExitCommon(true);
			}

			// Token: 0x06005520 RID: 21792 RVA: 0x00165A4C File Offset: 0x00163C4C
			private static void ExitCommon(bool disposing)
			{
				object obj = Application.ThreadContext.tcInternalSyncObject;
				lock (obj)
				{
					if (Application.ThreadContext.contextHash != null)
					{
						Application.ThreadContext[] array = new Application.ThreadContext[Application.ThreadContext.contextHash.Values.Count];
						Application.ThreadContext.contextHash.Values.CopyTo(array, 0);
						for (int i = 0; i < array.Length; i++)
						{
							if (array[i].ApplicationContext != null)
							{
								array[i].ApplicationContext.ExitThread();
							}
							else
							{
								array[i].Dispose(disposing);
							}
						}
					}
				}
			}

			// Token: 0x06005521 RID: 21793 RVA: 0x00165AE4 File Offset: 0x00163CE4
			internal static void ExitDomain()
			{
				Application.ThreadContext.ExitCommon(false);
			}

			// Token: 0x06005522 RID: 21794 RVA: 0x00165AEC File Offset: 0x00163CEC
			~ThreadContext()
			{
				if (this.handle != IntPtr.Zero)
				{
					UnsafeNativeMethods.CloseHandle(new HandleRef(this, this.handle));
					this.handle = IntPtr.Zero;
				}
			}

			// Token: 0x06005523 RID: 21795 RVA: 0x00165B44 File Offset: 0x00163D44
			internal void FormActivated(bool activate)
			{
				if (activate)
				{
					UnsafeNativeMethods.IMsoComponentManager msoComponentManager = this.ComponentManager;
					if (msoComponentManager != null && !(msoComponentManager is Application.ComponentManager))
					{
						msoComponentManager.FOnComponentActivate((IntPtr)this.componentID);
					}
				}
			}

			// Token: 0x06005524 RID: 21796 RVA: 0x00165B78 File Offset: 0x00163D78
			internal void TrackInput(bool track)
			{
				if (track != this.GetState(32))
				{
					UnsafeNativeMethods.IMsoComponentManager msoComponentManager = this.ComponentManager;
					if (msoComponentManager != null && !(msoComponentManager is Application.ComponentManager))
					{
						msoComponentManager.FSetTrackingComponent((IntPtr)this.componentID, track);
						this.SetState(32, track);
					}
				}
			}

			// Token: 0x06005525 RID: 21797 RVA: 0x00165BC0 File Offset: 0x00163DC0
			internal static Application.ThreadContext FromCurrent()
			{
				Application.ThreadContext threadContext = Application.ThreadContext.currentThreadContext;
				if (threadContext == null)
				{
					threadContext = new Application.ThreadContext();
				}
				return threadContext;
			}

			// Token: 0x06005526 RID: 21798 RVA: 0x00165BE0 File Offset: 0x00163DE0
			internal static Application.ThreadContext FromId(int id)
			{
				Application.ThreadContext threadContext = (Application.ThreadContext)Application.ThreadContext.contextHash[id];
				if (threadContext == null && id == SafeNativeMethods.GetCurrentThreadId())
				{
					threadContext = new Application.ThreadContext();
				}
				return threadContext;
			}

			// Token: 0x06005527 RID: 21799 RVA: 0x00165C15 File Offset: 0x00163E15
			internal bool GetAllowQuit()
			{
				return Application.ThreadContext.totalMessageLoopCount > 0 && Application.ThreadContext.baseLoopReason == -1;
			}

			// Token: 0x06005528 RID: 21800 RVA: 0x00165C29 File Offset: 0x00163E29
			internal IntPtr GetHandle()
			{
				return this.handle;
			}

			// Token: 0x06005529 RID: 21801 RVA: 0x00165C31 File Offset: 0x00163E31
			internal int GetId()
			{
				return this.id;
			}

			// Token: 0x0600552A RID: 21802 RVA: 0x00165C39 File Offset: 0x00163E39
			internal CultureInfo GetCulture()
			{
				if (this.culture == null || this.culture.LCID != SafeNativeMethods.GetThreadLocale())
				{
					this.culture = new CultureInfo(SafeNativeMethods.GetThreadLocale());
				}
				return this.culture;
			}

			// Token: 0x0600552B RID: 21803 RVA: 0x00165C6B File Offset: 0x00163E6B
			internal bool GetMessageLoop()
			{
				return this.GetMessageLoop(false);
			}

			// Token: 0x0600552C RID: 21804 RVA: 0x00165C74 File Offset: 0x00163E74
			internal bool GetMessageLoop(bool mustBeActive)
			{
				if (this.messageLoopCount > ((mustBeActive && this.externalComponentManager) ? 1 : 0))
				{
					return true;
				}
				if (this.ComponentManager != null && this.externalComponentManager)
				{
					if (!mustBeActive)
					{
						return true;
					}
					UnsafeNativeMethods.IMsoComponent[] array = new UnsafeNativeMethods.IMsoComponent[1];
					if (this.ComponentManager.FGetActiveComponent(0, array, null, 0) && array[0] == this)
					{
						return true;
					}
				}
				Application.MessageLoopCallback messageLoopCallback = this.messageLoopCallback;
				return messageLoopCallback != null && messageLoopCallback();
			}

			// Token: 0x0600552D RID: 21805 RVA: 0x00165CE1 File Offset: 0x00163EE1
			private bool GetState(int bit)
			{
				return (this.threadState & bit) != 0;
			}

			// Token: 0x0600552E RID: 21806 RVA: 0x0000DE5C File Offset: 0x0000C05C
			public override object InitializeLifetimeService()
			{
				return null;
			}

			// Token: 0x0600552F RID: 21807 RVA: 0x00165CEE File Offset: 0x00163EEE
			internal bool IsValidComponentId()
			{
				return this.componentID != -1;
			}

			// Token: 0x06005530 RID: 21808 RVA: 0x00165CFC File Offset: 0x00163EFC
			internal ApartmentState OleRequired()
			{
				Thread currentThread = Thread.CurrentThread;
				if (!this.GetState(1))
				{
					int num = UnsafeNativeMethods.OleInitialize();
					this.SetState(1, true);
					if (num == -2147417850)
					{
						this.SetState(2, true);
					}
				}
				if (this.GetState(2))
				{
					return ApartmentState.MTA;
				}
				return ApartmentState.STA;
			}

			// Token: 0x06005531 RID: 21809 RVA: 0x00165D42 File Offset: 0x00163F42
			private void OnAppThreadExit(object sender, EventArgs e)
			{
				this.Dispose(true);
			}

			// Token: 0x06005532 RID: 21810 RVA: 0x00165D4B File Offset: 0x00163F4B
			[PrePrepareMethod]
			private void OnDomainUnload(object sender, EventArgs e)
			{
				this.RevokeComponent();
				Application.ThreadContext.ExitDomain();
			}

			// Token: 0x06005533 RID: 21811 RVA: 0x00165D58 File Offset: 0x00163F58
			internal void OnThreadException(Exception t)
			{
				if (this.GetState(4))
				{
					return;
				}
				this.SetState(4, true);
				try
				{
					if (this.threadExceptionHandler != null)
					{
						this.threadExceptionHandler(Thread.CurrentThread, new ThreadExceptionEventArgs(t));
					}
					else if (SystemInformation.UserInteractive)
					{
						ThreadExceptionDialog threadExceptionDialog = new ThreadExceptionDialog(t);
						DialogResult dialogResult = DialogResult.OK;
						IntSecurity.ModifyFocus.Assert();
						try
						{
							dialogResult = threadExceptionDialog.ShowDialog();
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
							threadExceptionDialog.Dispose();
						}
						if (dialogResult != DialogResult.Abort)
						{
							if (dialogResult == DialogResult.Yes)
							{
								WarningException ex = t as WarningException;
								if (ex != null)
								{
									Help.ShowHelp(null, ex.HelpUrl, ex.HelpTopic);
								}
							}
						}
						else
						{
							Application.ExitInternal();
							new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
							Environment.Exit(0);
						}
					}
				}
				finally
				{
					this.SetState(4, false);
				}
			}

			// Token: 0x06005534 RID: 21812 RVA: 0x00165E2C File Offset: 0x0016402C
			internal void PostQuit()
			{
				UnsafeNativeMethods.PostThreadMessage(this.id, 18, IntPtr.Zero, IntPtr.Zero);
				this.SetState(8, true);
			}

			// Token: 0x06005535 RID: 21813 RVA: 0x00165E4E File Offset: 0x0016404E
			internal void RegisterMessageLoop(Application.MessageLoopCallback callback)
			{
				this.messageLoopCallback = callback;
			}

			// Token: 0x06005536 RID: 21814 RVA: 0x00165E57 File Offset: 0x00164057
			internal void RemoveMessageFilter(IMessageFilter f)
			{
				if (this.messageFilters != null)
				{
					this.SetState(16, false);
					this.messageFilters.Remove(f);
				}
			}

			// Token: 0x06005537 RID: 21815 RVA: 0x00165E78 File Offset: 0x00164078
			internal void RunMessageLoop(int reason, ApplicationContext context)
			{
				IntPtr userCookie = IntPtr.Zero;
				if (Application.useVisualStyles)
				{
					userCookie = UnsafeNativeMethods.ThemingScope.Activate();
				}
				try
				{
					this.RunMessageLoopInner(reason, context);
				}
				finally
				{
					UnsafeNativeMethods.ThemingScope.Deactivate(userCookie);
				}
			}

			// Token: 0x06005538 RID: 21816 RVA: 0x00165EBC File Offset: 0x001640BC
			private void RunMessageLoopInner(int reason, ApplicationContext context)
			{
				if (reason == 4 && !SystemInformation.UserInteractive)
				{
					throw new InvalidOperationException(SR.GetString("CantShowModalOnNonInteractive"));
				}
				if (reason == -1)
				{
					this.SetState(8, false);
				}
				if (Application.ThreadContext.totalMessageLoopCount++ == 0)
				{
					Application.ThreadContext.baseLoopReason = reason;
				}
				this.messageLoopCount++;
				if (reason == -1)
				{
					if (this.messageLoopCount != 1)
					{
						throw new InvalidOperationException(SR.GetString("CantNestMessageLoops"));
					}
					this.applicationContext = context;
					this.applicationContext.ThreadExit += this.OnAppThreadExit;
					if (this.applicationContext.MainForm != null)
					{
						this.applicationContext.MainForm.Visible = true;
					}
					DpiHelper.InitializeDpiHelperForWinforms();
					AccessibilityImprovements.ValidateLevels();
				}
				Form form = this.currentForm;
				if (context != null)
				{
					this.currentForm = context.MainForm;
				}
				bool flag = false;
				bool flag2 = false;
				HandleRef hWnd = new HandleRef(null, IntPtr.Zero);
				if (reason == -2)
				{
					flag2 = true;
				}
				if (reason == 4 || reason == 5)
				{
					flag = true;
					bool flag3 = this.currentForm != null && this.currentForm.Enabled;
					this.BeginModalMessageLoop(context);
					hWnd = new HandleRef(null, UnsafeNativeMethods.GetWindowLong(new HandleRef(this.currentForm, this.currentForm.Handle), -8));
					if (hWnd.Handle != IntPtr.Zero)
					{
						if (SafeNativeMethods.IsWindowEnabled(hWnd))
						{
							SafeNativeMethods.EnableWindow(hWnd, false);
						}
						else
						{
							hWnd = new HandleRef(null, IntPtr.Zero);
						}
					}
					if (this.currentForm != null && this.currentForm.IsHandleCreated && SafeNativeMethods.IsWindowEnabled(new HandleRef(this.currentForm, this.currentForm.Handle)) != flag3)
					{
						SafeNativeMethods.EnableWindow(new HandleRef(this.currentForm, this.currentForm.Handle), flag3);
					}
				}
				try
				{
					if (this.messageLoopCount == 1)
					{
						WindowsFormsSynchronizationContext.InstallIfNeeded();
					}
					if (flag && this.currentForm != null)
					{
						this.currentForm.Visible = true;
					}
					if ((!flag && !flag2) || this.ComponentManager is Application.ComponentManager)
					{
						bool flag4 = this.ComponentManager.FPushMessageLoop((IntPtr)this.componentID, reason, 0);
					}
					else if (reason == 2 || reason == -2)
					{
						bool flag4 = this.LocalModalMessageLoop(null);
					}
					else
					{
						bool flag4 = this.LocalModalMessageLoop(this.currentForm);
					}
				}
				finally
				{
					if (flag)
					{
						this.EndModalMessageLoop(context);
						if (hWnd.Handle != IntPtr.Zero)
						{
							SafeNativeMethods.EnableWindow(hWnd, true);
						}
					}
					this.currentForm = form;
					Application.ThreadContext.totalMessageLoopCount--;
					this.messageLoopCount--;
					if (this.messageLoopCount == 0)
					{
						WindowsFormsSynchronizationContext.Uninstall(false);
					}
					if (reason == -1)
					{
						this.Dispose(true);
					}
					else if (this.messageLoopCount == 0 && this.componentManager != null)
					{
						this.RevokeComponent();
					}
				}
			}

			// Token: 0x06005539 RID: 21817 RVA: 0x00166180 File Offset: 0x00164380
			private bool LocalModalMessageLoop(Form form)
			{
				bool result;
				try
				{
					NativeMethods.MSG msg = default(NativeMethods.MSG);
					bool flag = true;
					while (flag)
					{
						bool flag2 = UnsafeNativeMethods.PeekMessage(ref msg, NativeMethods.NullHandleRef, 0, 0, 0);
						if (flag2)
						{
							bool flag3;
							if (msg.hwnd != IntPtr.Zero && SafeNativeMethods.IsWindowUnicode(new HandleRef(null, msg.hwnd)))
							{
								flag3 = true;
								if (!UnsafeNativeMethods.GetMessageW(ref msg, NativeMethods.NullHandleRef, 0, 0))
								{
									continue;
								}
							}
							else
							{
								flag3 = false;
								if (!UnsafeNativeMethods.GetMessageA(ref msg, NativeMethods.NullHandleRef, 0, 0))
								{
									continue;
								}
							}
							if (!this.PreTranslateMessage(ref msg))
							{
								UnsafeNativeMethods.TranslateMessage(ref msg);
								if (flag3)
								{
									UnsafeNativeMethods.DispatchMessageW(ref msg);
								}
								else
								{
									UnsafeNativeMethods.DispatchMessageA(ref msg);
								}
							}
							if (form != null)
							{
								flag = !form.CheckCloseDialog(false);
							}
						}
						else
						{
							if (form == null)
							{
								break;
							}
							if (!UnsafeNativeMethods.PeekMessage(ref msg, NativeMethods.NullHandleRef, 0, 0, 0))
							{
								UnsafeNativeMethods.WaitMessage();
							}
						}
					}
					result = flag;
				}
				catch
				{
					result = false;
				}
				return result;
			}

			// Token: 0x0600553A RID: 21818 RVA: 0x00166274 File Offset: 0x00164474
			internal bool ProcessFilters(ref NativeMethods.MSG msg, out bool modified)
			{
				bool result = false;
				modified = false;
				if (this.messageFilters != null && !this.GetState(16) && (LocalAppContextSwitches.DontSupportReentrantFilterMessage || this.inProcessFilters == 0))
				{
					if (this.messageFilters.Count > 0)
					{
						this.messageFilterSnapshot = new IMessageFilter[this.messageFilters.Count];
						this.messageFilters.CopyTo(this.messageFilterSnapshot);
					}
					else
					{
						this.messageFilterSnapshot = null;
					}
					this.SetState(16, true);
				}
				this.inProcessFilters++;
				try
				{
					if (this.messageFilterSnapshot != null)
					{
						int num = this.messageFilterSnapshot.Length;
						Message message = Message.Create(msg.hwnd, msg.message, msg.wParam, msg.lParam);
						for (int i = 0; i < num; i++)
						{
							IMessageFilter messageFilter = this.messageFilterSnapshot[i];
							bool flag = messageFilter.PreFilterMessage(ref message);
							if (messageFilter is IMessageModifyAndFilter)
							{
								msg.hwnd = message.HWnd;
								msg.message = message.Msg;
								msg.wParam = message.WParam;
								msg.lParam = message.LParam;
								modified = true;
							}
							if (flag)
							{
								result = true;
								break;
							}
						}
					}
				}
				finally
				{
					this.inProcessFilters--;
				}
				return result;
			}

			// Token: 0x0600553B RID: 21819 RVA: 0x001663BC File Offset: 0x001645BC
			internal bool PreTranslateMessage(ref NativeMethods.MSG msg)
			{
				bool flag = false;
				if (this.ProcessFilters(ref msg, out flag))
				{
					return true;
				}
				if (msg.message >= 256 && msg.message <= 264)
				{
					if (msg.message == 258)
					{
						int num = 21364736;
						if ((int)((long)msg.wParam) == 3 && ((int)((long)msg.lParam) & num) == num && Debugger.IsAttached)
						{
							Debugger.Break();
						}
					}
					Control control = Control.FromChildHandleInternal(msg.hwnd);
					bool flag2 = false;
					Message message = Message.Create(msg.hwnd, msg.message, msg.wParam, msg.lParam);
					if (control != null)
					{
						if (NativeWindow.WndProcShouldBeDebuggable)
						{
							if (Control.PreProcessControlMessageInternal(control, ref message) == PreProcessControlState.MessageProcessed)
							{
								flag2 = true;
								goto IL_104;
							}
							goto IL_104;
						}
						else
						{
							try
							{
								if (Control.PreProcessControlMessageInternal(control, ref message) == PreProcessControlState.MessageProcessed)
								{
									flag2 = true;
								}
								goto IL_104;
							}
							catch (Exception t)
							{
								this.OnThreadException(t);
								goto IL_104;
							}
						}
					}
					IntPtr ancestor = UnsafeNativeMethods.GetAncestor(new HandleRef(null, msg.hwnd), 2);
					if (ancestor != IntPtr.Zero && UnsafeNativeMethods.IsDialogMessage(new HandleRef(null, ancestor), ref msg))
					{
						return true;
					}
					IL_104:
					msg.wParam = message.WParam;
					msg.lParam = message.LParam;
					if (flag2)
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x0600553C RID: 21820 RVA: 0x00166500 File Offset: 0x00164700
			private void RevokeComponent()
			{
				if (this.componentManager != null && this.componentID != -1)
				{
					int value = this.componentID;
					UnsafeNativeMethods.IMsoComponentManager msoComponentManager = this.componentManager;
					try
					{
						msoComponentManager.FRevokeComponent((IntPtr)value);
						if (Marshal.IsComObject(msoComponentManager))
						{
							Marshal.ReleaseComObject(msoComponentManager);
						}
					}
					finally
					{
						this.componentManager = null;
						this.componentID = -1;
					}
				}
			}

			// Token: 0x0600553D RID: 21821 RVA: 0x0016656C File Offset: 0x0016476C
			internal void SetCulture(CultureInfo culture)
			{
				if (culture != null && culture.LCID != SafeNativeMethods.GetThreadLocale())
				{
					SafeNativeMethods.SetThreadLocale(culture.LCID);
				}
			}

			// Token: 0x0600553E RID: 21822 RVA: 0x0016658A File Offset: 0x0016478A
			private void SetState(int bit, bool value)
			{
				if (value)
				{
					this.threadState |= bit;
					return;
				}
				this.threadState &= ~bit;
			}

			// Token: 0x0600553F RID: 21823 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			bool UnsafeNativeMethods.IMsoComponent.FDebugMessage(IntPtr hInst, int msg, IntPtr wparam, IntPtr lparam)
			{
				return false;
			}

			// Token: 0x06005540 RID: 21824 RVA: 0x001665AD File Offset: 0x001647AD
			bool UnsafeNativeMethods.IMsoComponent.FPreTranslateMessage(ref NativeMethods.MSG msg)
			{
				return this.PreTranslateMessage(ref msg);
			}

			// Token: 0x06005541 RID: 21825 RVA: 0x001665B6 File Offset: 0x001647B6
			void UnsafeNativeMethods.IMsoComponent.OnEnterState(int uStateID, bool fEnter)
			{
				if (this.ourModalLoop)
				{
					return;
				}
				if (uStateID == 1)
				{
					if (fEnter)
					{
						this.DisableWindowsForModalLoop(true, null);
						return;
					}
					this.EnableWindowsForModalLoop(true, null);
				}
			}

			// Token: 0x06005542 RID: 21826 RVA: 0x0000701A File Offset: 0x0000521A
			void UnsafeNativeMethods.IMsoComponent.OnAppActivate(bool fActive, int dwOtherThreadID)
			{
			}

			// Token: 0x06005543 RID: 21827 RVA: 0x0000701A File Offset: 0x0000521A
			void UnsafeNativeMethods.IMsoComponent.OnLoseActivation()
			{
			}

			// Token: 0x06005544 RID: 21828 RVA: 0x0000701A File Offset: 0x0000521A
			void UnsafeNativeMethods.IMsoComponent.OnActivationChange(UnsafeNativeMethods.IMsoComponent component, bool fSameComponent, int pcrinfo, bool fHostIsActivating, int pchostinfo, int dwReserved)
			{
			}

			// Token: 0x06005545 RID: 21829 RVA: 0x001665D9 File Offset: 0x001647D9
			bool UnsafeNativeMethods.IMsoComponent.FDoIdle(int grfidlef)
			{
				if (this.idleHandler != null)
				{
					this.idleHandler(Thread.CurrentThread, EventArgs.Empty);
				}
				return false;
			}

			// Token: 0x06005546 RID: 21830 RVA: 0x001665FC File Offset: 0x001647FC
			bool UnsafeNativeMethods.IMsoComponent.FContinueMessageLoop(int reason, int pvLoopData, NativeMethods.MSG[] msgPeeked)
			{
				bool result = true;
				if (msgPeeked == null && this.GetState(8))
				{
					result = false;
				}
				else
				{
					switch (reason)
					{
					case -2:
					case 2:
						if (!UnsafeNativeMethods.PeekMessage(ref this.tempMsg, NativeMethods.NullHandleRef, 0, 0, 0))
						{
							result = false;
						}
						break;
					case 1:
					{
						int num;
						SafeNativeMethods.GetWindowThreadProcessId(new HandleRef(null, UnsafeNativeMethods.GetActiveWindow()), out num);
						if (num == SafeNativeMethods.GetCurrentProcessId())
						{
							result = false;
						}
						break;
					}
					case 4:
					case 5:
						if (this.currentForm == null || this.currentForm.CheckCloseDialog(false))
						{
							result = false;
						}
						break;
					}
				}
				return result;
			}

			// Token: 0x06005547 RID: 21831 RVA: 0x0000E214 File Offset: 0x0000C414
			bool UnsafeNativeMethods.IMsoComponent.FQueryTerminate(bool fPromptUser)
			{
				return true;
			}

			// Token: 0x06005548 RID: 21832 RVA: 0x00166697 File Offset: 0x00164897
			void UnsafeNativeMethods.IMsoComponent.Terminate()
			{
				if (this.messageLoopCount > 0 && !(this.ComponentManager is Application.ComponentManager))
				{
					this.messageLoopCount--;
				}
				this.Dispose(false);
			}

			// Token: 0x06005549 RID: 21833 RVA: 0x000F1545 File Offset: 0x000EF745
			IntPtr UnsafeNativeMethods.IMsoComponent.HwndGetWindow(int dwWhich, int dwReserved)
			{
				return IntPtr.Zero;
			}

			// Token: 0x0400376F RID: 14191
			private const int STATE_OLEINITIALIZED = 1;

			// Token: 0x04003770 RID: 14192
			private const int STATE_EXTERNALOLEINIT = 2;

			// Token: 0x04003771 RID: 14193
			private const int STATE_INTHREADEXCEPTION = 4;

			// Token: 0x04003772 RID: 14194
			private const int STATE_POSTEDQUIT = 8;

			// Token: 0x04003773 RID: 14195
			private const int STATE_FILTERSNAPSHOTVALID = 16;

			// Token: 0x04003774 RID: 14196
			private const int STATE_TRACKINGCOMPONENT = 32;

			// Token: 0x04003775 RID: 14197
			private const int INVALID_ID = -1;

			// Token: 0x04003776 RID: 14198
			private static Hashtable contextHash = new Hashtable();

			// Token: 0x04003777 RID: 14199
			private static object tcInternalSyncObject = new object();

			// Token: 0x04003778 RID: 14200
			private static int totalMessageLoopCount;

			// Token: 0x04003779 RID: 14201
			private static int baseLoopReason;

			// Token: 0x0400377A RID: 14202
			[ThreadStatic]
			private static Application.ThreadContext currentThreadContext;

			// Token: 0x0400377B RID: 14203
			internal ThreadExceptionEventHandler threadExceptionHandler;

			// Token: 0x0400377C RID: 14204
			internal EventHandler idleHandler;

			// Token: 0x0400377D RID: 14205
			internal EventHandler enterModalHandler;

			// Token: 0x0400377E RID: 14206
			internal EventHandler leaveModalHandler;

			// Token: 0x0400377F RID: 14207
			private ApplicationContext applicationContext;

			// Token: 0x04003780 RID: 14208
			private List<Application.ParkingWindow> parkingWindows = new List<Application.ParkingWindow>();

			// Token: 0x04003781 RID: 14209
			private Control marshalingControl;

			// Token: 0x04003782 RID: 14210
			private CultureInfo culture;

			// Token: 0x04003783 RID: 14211
			private ArrayList messageFilters;

			// Token: 0x04003784 RID: 14212
			private IMessageFilter[] messageFilterSnapshot;

			// Token: 0x04003785 RID: 14213
			private int inProcessFilters;

			// Token: 0x04003786 RID: 14214
			private IntPtr handle;

			// Token: 0x04003787 RID: 14215
			private int id;

			// Token: 0x04003788 RID: 14216
			private int messageLoopCount;

			// Token: 0x04003789 RID: 14217
			private int threadState;

			// Token: 0x0400378A RID: 14218
			private int modalCount;

			// Token: 0x0400378B RID: 14219
			private WeakReference activatingControlRef;

			// Token: 0x0400378C RID: 14220
			private UnsafeNativeMethods.IMsoComponentManager componentManager;

			// Token: 0x0400378D RID: 14221
			private bool externalComponentManager;

			// Token: 0x0400378E RID: 14222
			private bool fetchingComponentManager;

			// Token: 0x0400378F RID: 14223
			private int componentID = -1;

			// Token: 0x04003790 RID: 14224
			private Form currentForm;

			// Token: 0x04003791 RID: 14225
			private Application.ThreadWindows threadWindows;

			// Token: 0x04003792 RID: 14226
			private NativeMethods.MSG tempMsg;

			// Token: 0x04003793 RID: 14227
			private int disposeCount;

			// Token: 0x04003794 RID: 14228
			private bool ourModalLoop;

			// Token: 0x04003795 RID: 14229
			private Application.MessageLoopCallback messageLoopCallback;
		}

		// Token: 0x02000547 RID: 1351
		internal sealed class MarshalingControl : Control
		{
			// Token: 0x0600554B RID: 21835 RVA: 0x001666DA File Offset: 0x001648DA
			internal MarshalingControl() : base(false)
			{
				base.Visible = false;
				base.SetState2(8, false);
				base.SetTopLevel(true);
				base.CreateControl();
				this.CreateHandle();
			}

			// Token: 0x17001468 RID: 5224
			// (get) Token: 0x0600554C RID: 21836 RVA: 0x00166708 File Offset: 0x00164908
			protected override CreateParams CreateParams
			{
				get
				{
					CreateParams createParams = base.CreateParams;
					if (Environment.OSVersion.Platform == PlatformID.Win32NT)
					{
						createParams.Parent = (IntPtr)NativeMethods.HWND_MESSAGE;
					}
					return createParams;
				}
			}

			// Token: 0x0600554D RID: 21837 RVA: 0x0000701A File Offset: 0x0000521A
			protected override void OnLayout(LayoutEventArgs levent)
			{
			}

			// Token: 0x0600554E RID: 21838 RVA: 0x0000701A File Offset: 0x0000521A
			protected override void OnSizeChanged(EventArgs e)
			{
			}
		}

		// Token: 0x02000548 RID: 1352
		internal sealed class ParkingWindow : ContainerControl, IArrangedElement, IComponent, IDisposable
		{
			// Token: 0x0600554F RID: 21839 RVA: 0x0016673A File Offset: 0x0016493A
			public ParkingWindow()
			{
				base.SetState2(8, false);
				base.SetState(524288, true);
				this.Text = "WindowsFormsParkingWindow";
				base.Visible = false;
			}

			// Token: 0x17001469 RID: 5225
			// (get) Token: 0x06005550 RID: 21840 RVA: 0x00166768 File Offset: 0x00164968
			protected override CreateParams CreateParams
			{
				get
				{
					CreateParams createParams = base.CreateParams;
					if (Environment.OSVersion.Platform == PlatformID.Win32NT)
					{
						createParams.Parent = (IntPtr)NativeMethods.HWND_MESSAGE;
					}
					return createParams;
				}
			}

			// Token: 0x06005551 RID: 21841 RVA: 0x0016679A File Offset: 0x0016499A
			internal override void AddReflectChild()
			{
				if (this.childCount < 0)
				{
					this.childCount = 0;
				}
				this.childCount++;
			}

			// Token: 0x06005552 RID: 21842 RVA: 0x001667BC File Offset: 0x001649BC
			internal override void RemoveReflectChild()
			{
				this.childCount--;
				if (this.childCount < 0)
				{
					this.childCount = 0;
				}
				if (this.childCount == 0 && base.IsHandleCreated)
				{
					int num;
					int windowThreadProcessId = SafeNativeMethods.GetWindowThreadProcessId(new HandleRef(this, base.HandleInternal), out num);
					Application.ThreadContext threadContext = Application.ThreadContext.FromId(windowThreadProcessId);
					if (threadContext == null || threadContext != Application.ThreadContext.FromCurrent())
					{
						UnsafeNativeMethods.PostMessage(new HandleRef(this, base.HandleInternal), 1025, IntPtr.Zero, IntPtr.Zero);
						return;
					}
					this.CheckDestroy();
				}
			}

			// Token: 0x06005553 RID: 21843 RVA: 0x00166848 File Offset: 0x00164A48
			private void CheckDestroy()
			{
				if (this.childCount == 0)
				{
					IntPtr window = UnsafeNativeMethods.GetWindow(new HandleRef(this, base.Handle), 5);
					if (window == IntPtr.Zero)
					{
						this.DestroyHandle();
					}
				}
			}

			// Token: 0x06005554 RID: 21844 RVA: 0x00166883 File Offset: 0x00164A83
			public void Destroy()
			{
				this.DestroyHandle();
			}

			// Token: 0x06005555 RID: 21845 RVA: 0x0016688B File Offset: 0x00164A8B
			internal void ParkHandle(HandleRef handle)
			{
				if (!base.IsHandleCreated)
				{
					this.CreateHandle();
				}
				UnsafeNativeMethods.SetParent(handle, new HandleRef(this, base.Handle));
			}

			// Token: 0x06005556 RID: 21846 RVA: 0x001668AE File Offset: 0x00164AAE
			internal void UnparkHandle(HandleRef handle)
			{
				if (base.IsHandleCreated)
				{
					this.CheckDestroy();
				}
			}

			// Token: 0x06005557 RID: 21847 RVA: 0x0000701A File Offset: 0x0000521A
			protected override void OnLayout(LayoutEventArgs levent)
			{
			}

			// Token: 0x06005558 RID: 21848 RVA: 0x0000701A File Offset: 0x0000521A
			void IArrangedElement.PerformLayout(IArrangedElement affectedElement, string affectedProperty)
			{
			}

			// Token: 0x06005559 RID: 21849 RVA: 0x001668C0 File Offset: 0x00164AC0
			protected override void WndProc(ref Message m)
			{
				if (m.Msg != 24)
				{
					base.WndProc(ref m);
					if (m.Msg == 528)
					{
						if (NativeMethods.Util.LOWORD((int)((long)m.WParam)) == 2)
						{
							UnsafeNativeMethods.PostMessage(new HandleRef(this, base.Handle), 1025, IntPtr.Zero, IntPtr.Zero);
							return;
						}
					}
					else if (m.Msg == 1025)
					{
						this.CheckDestroy();
					}
				}
			}

			// Token: 0x04003796 RID: 14230
			private const int WM_CHECKDESTROY = 1025;

			// Token: 0x04003797 RID: 14231
			private int childCount;
		}

		// Token: 0x02000549 RID: 1353
		private sealed class ThreadWindows
		{
			// Token: 0x0600555A RID: 21850 RVA: 0x00166934 File Offset: 0x00164B34
			internal ThreadWindows(bool onlyWinForms)
			{
				this.windows = new IntPtr[16];
				this.onlyWinForms = onlyWinForms;
				UnsafeNativeMethods.EnumThreadWindows(SafeNativeMethods.GetCurrentThreadId(), new NativeMethods.EnumThreadWindowsCallback(this.Callback), NativeMethods.NullHandleRef);
			}

			// Token: 0x0600555B RID: 21851 RVA: 0x00166974 File Offset: 0x00164B74
			private bool Callback(IntPtr hWnd, IntPtr lparam)
			{
				if (SafeNativeMethods.IsWindowVisible(new HandleRef(null, hWnd)) && SafeNativeMethods.IsWindowEnabled(new HandleRef(null, hWnd)))
				{
					bool flag = true;
					if (this.onlyWinForms && Control.FromHandleInternal(hWnd) == null)
					{
						flag = false;
					}
					if (flag)
					{
						if (this.windowCount == this.windows.Length)
						{
							IntPtr[] destinationArray = new IntPtr[this.windowCount * 2];
							Array.Copy(this.windows, 0, destinationArray, 0, this.windowCount);
							this.windows = destinationArray;
						}
						IntPtr[] array = this.windows;
						int num = this.windowCount;
						this.windowCount = num + 1;
						array[num] = hWnd;
					}
				}
				return true;
			}

			// Token: 0x0600555C RID: 21852 RVA: 0x00166A0C File Offset: 0x00164C0C
			internal void Dispose()
			{
				for (int i = 0; i < this.windowCount; i++)
				{
					IntPtr handle = this.windows[i];
					if (UnsafeNativeMethods.IsWindow(new HandleRef(null, handle)))
					{
						Control control = Control.FromHandleInternal(handle);
						if (control != null)
						{
							control.Dispose();
						}
					}
				}
			}

			// Token: 0x0600555D RID: 21853 RVA: 0x00166A54 File Offset: 0x00164C54
			internal void Enable(bool state)
			{
				if (!this.onlyWinForms && !state)
				{
					this.activeHwnd = UnsafeNativeMethods.GetActiveWindow();
					Control activatingControl = Application.ThreadContext.FromCurrent().ActivatingControl;
					if (activatingControl != null)
					{
						this.focusedHwnd = activatingControl.Handle;
					}
					else
					{
						this.focusedHwnd = UnsafeNativeMethods.GetFocus();
					}
				}
				for (int i = 0; i < this.windowCount; i++)
				{
					IntPtr handle = this.windows[i];
					if (UnsafeNativeMethods.IsWindow(new HandleRef(null, handle)))
					{
						SafeNativeMethods.EnableWindow(new HandleRef(null, handle), state);
					}
				}
				if (!this.onlyWinForms && state)
				{
					if (this.activeHwnd != IntPtr.Zero && UnsafeNativeMethods.IsWindow(new HandleRef(null, this.activeHwnd)))
					{
						UnsafeNativeMethods.SetActiveWindow(new HandleRef(null, this.activeHwnd));
					}
					if (this.focusedHwnd != IntPtr.Zero && UnsafeNativeMethods.IsWindow(new HandleRef(null, this.focusedHwnd)))
					{
						UnsafeNativeMethods.SetFocus(new HandleRef(null, this.focusedHwnd));
					}
				}
			}

			// Token: 0x04003798 RID: 14232
			private IntPtr[] windows;

			// Token: 0x04003799 RID: 14233
			private int windowCount;

			// Token: 0x0400379A RID: 14234
			private IntPtr activeHwnd;

			// Token: 0x0400379B RID: 14235
			private IntPtr focusedHwnd;

			// Token: 0x0400379C RID: 14236
			internal Application.ThreadWindows previousThreadWindows;

			// Token: 0x0400379D RID: 14237
			private bool onlyWinForms = true;
		}

		// Token: 0x0200054A RID: 1354
		private class ModalApplicationContext : ApplicationContext
		{
			// Token: 0x0600555E RID: 21854 RVA: 0x00166B4F File Offset: 0x00164D4F
			public ModalApplicationContext(Form modalForm) : base(modalForm)
			{
			}

			// Token: 0x0600555F RID: 21855 RVA: 0x00166B58 File Offset: 0x00164D58
			public void DisableThreadWindows(bool disable, bool onlyWinForms)
			{
				Control control = null;
				if (base.MainForm != null && base.MainForm.IsHandleCreated)
				{
					IntPtr windowLong = UnsafeNativeMethods.GetWindowLong(new HandleRef(this, base.MainForm.Handle), -8);
					control = Control.FromHandleInternal(windowLong);
					if (control != null && control.InvokeRequired)
					{
						this.parentWindowContext = Application.GetContextForHandle(new HandleRef(this, windowLong));
					}
					else
					{
						this.parentWindowContext = null;
					}
				}
				if (this.parentWindowContext != null)
				{
					if (control == null)
					{
						control = this.parentWindowContext.ApplicationContext.MainForm;
					}
					if (disable)
					{
						control.Invoke(new Application.ModalApplicationContext.ThreadWindowCallback(this.DisableThreadWindowsCallback), new object[]
						{
							this.parentWindowContext,
							onlyWinForms
						});
						return;
					}
					control.Invoke(new Application.ModalApplicationContext.ThreadWindowCallback(this.EnableThreadWindowsCallback), new object[]
					{
						this.parentWindowContext,
						onlyWinForms
					});
				}
			}

			// Token: 0x06005560 RID: 21856 RVA: 0x00166C38 File Offset: 0x00164E38
			private void DisableThreadWindowsCallback(Application.ThreadContext context, bool onlyWinForms)
			{
				context.DisableWindowsForModalLoop(onlyWinForms, this);
			}

			// Token: 0x06005561 RID: 21857 RVA: 0x00166C42 File Offset: 0x00164E42
			private void EnableThreadWindowsCallback(Application.ThreadContext context, bool onlyWinForms)
			{
				context.EnableWindowsForModalLoop(onlyWinForms, this);
			}

			// Token: 0x06005562 RID: 21858 RVA: 0x0000701A File Offset: 0x0000521A
			protected override void ExitThreadCore()
			{
			}

			// Token: 0x0400379E RID: 14238
			private Application.ThreadContext parentWindowContext;

			// Token: 0x0200088A RID: 2186
			// (Invoke) Token: 0x06007068 RID: 28776
			private delegate void ThreadWindowCallback(Application.ThreadContext context, bool onlyWinForms);
		}
	}
}
