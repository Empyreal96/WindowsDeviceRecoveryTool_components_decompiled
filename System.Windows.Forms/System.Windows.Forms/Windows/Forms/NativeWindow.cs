using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Internal;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security;
using System.Security.Permissions;
using System.Text;
using Microsoft.Win32;

namespace System.Windows.Forms
{
	/// <summary>Provides a low-level encapsulation of a window handle and a window procedure.</summary>
	// Token: 0x020002F5 RID: 757
	[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class NativeWindow : MarshalByRefObject, IWin32Window
	{
		// Token: 0x06002DDA RID: 11738 RVA: 0x000D58B8 File Offset: 0x000D3AB8
		static NativeWindow()
		{
			EventHandler value = new EventHandler(NativeWindow.OnShutdown);
			AppDomain.CurrentDomain.ProcessExit += value;
			AppDomain.CurrentDomain.DomainUnload += value;
			int num = NativeWindow.primes[4];
			NativeWindow.hashBuckets = new NativeWindow.HandleBucket[num];
			NativeWindow.hashLoadSize = (int)(0.72f * (float)num);
			if (NativeWindow.hashLoadSize >= num)
			{
				NativeWindow.hashLoadSize = num - 1;
			}
			NativeWindow.hashForIdHandle = new Dictionary<short, IntPtr>();
			NativeWindow.hashForHandleId = new Dictionary<IntPtr, short>();
		}

		/// <summary>Initializes an instance of the <see cref="T:System.Windows.Forms.NativeWindow" /> class.</summary>
		// Token: 0x06002DDB RID: 11739 RVA: 0x000D596A File Offset: 0x000D3B6A
		public NativeWindow()
		{
			this.weakThisPtr = new WeakReference(this);
		}

		// Token: 0x17000B18 RID: 2840
		// (get) Token: 0x06002DDC RID: 11740 RVA: 0x000D5993 File Offset: 0x000D3B93
		internal DpiAwarenessContext DpiAwarenessContext
		{
			get
			{
				return this.windowDpiAwarenessContext;
			}
		}

		/// <summary>Releases the resources associated with this window. </summary>
		// Token: 0x06002DDD RID: 11741 RVA: 0x000D599C File Offset: 0x000D3B9C
		~NativeWindow()
		{
			this.ForceExitMessageLoop();
		}

		// Token: 0x06002DDE RID: 11742 RVA: 0x000D59C8 File Offset: 0x000D3BC8
		internal void ForceExitMessageLoop()
		{
			IntPtr value;
			bool flag2;
			lock (this)
			{
				value = this.handle;
				flag2 = this.ownHandle;
			}
			if (this.handle != IntPtr.Zero)
			{
				if (UnsafeNativeMethods.IsWindow(new HandleRef(null, this.handle)))
				{
					int num;
					int windowThreadProcessId = SafeNativeMethods.GetWindowThreadProcessId(new HandleRef(null, this.handle), out num);
					Application.ThreadContext threadContext = Application.ThreadContext.FromId(windowThreadProcessId);
					IntPtr value2 = (threadContext == null) ? IntPtr.Zero : threadContext.GetHandle();
					if (value2 != IntPtr.Zero)
					{
						int num2 = 0;
						SafeNativeMethods.GetExitCodeThread(new HandleRef(null, value2), out num2);
						if (!AppDomain.CurrentDomain.IsFinalizingForUnload() && num2 == 259)
						{
							IntPtr intPtr;
							UnsafeNativeMethods.SendMessageTimeout(new HandleRef(null, this.handle), NativeMethods.WM_UIUNSUBCLASS, IntPtr.Zero, IntPtr.Zero, 2, 100, out intPtr) == IntPtr.Zero;
						}
					}
				}
				if (this.handle != IntPtr.Zero)
				{
					this.ReleaseHandle(true);
				}
			}
			if (value != IntPtr.Zero && flag2)
			{
				UnsafeNativeMethods.PostMessage(new HandleRef(this, value), 16, 0, 0);
			}
		}

		// Token: 0x17000B19 RID: 2841
		// (get) Token: 0x06002DDF RID: 11743 RVA: 0x000D5B08 File Offset: 0x000D3D08
		internal static bool AnyHandleCreated
		{
			get
			{
				return NativeWindow.anyHandleCreated;
			}
		}

		/// <summary>Gets the handle for this window. </summary>
		/// <returns>If successful, an <see cref="T:System.IntPtr" /> representing the handle to the associated native Win32 window; otherwise, 0 if no handle is associated with the window.</returns>
		// Token: 0x17000B1A RID: 2842
		// (get) Token: 0x06002DE0 RID: 11744 RVA: 0x000D5B0F File Offset: 0x000D3D0F
		public IntPtr Handle
		{
			get
			{
				return this.handle;
			}
		}

		// Token: 0x17000B1B RID: 2843
		// (get) Token: 0x06002DE1 RID: 11745 RVA: 0x000D5B17 File Offset: 0x000D3D17
		internal NativeWindow PreviousWindow
		{
			get
			{
				return this.previousWindow;
			}
		}

		// Token: 0x17000B1C RID: 2844
		// (get) Token: 0x06002DE2 RID: 11746 RVA: 0x000D5B1F File Offset: 0x000D3D1F
		internal static IntPtr UserDefindowProc
		{
			get
			{
				return NativeWindow.userDefWindowProc;
			}
		}

		// Token: 0x17000B1D RID: 2845
		// (get) Token: 0x06002DE3 RID: 11747 RVA: 0x000D5B28 File Offset: 0x000D3D28
		private static int WndProcFlags
		{
			get
			{
				int num = (int)NativeWindow.wndProcFlags;
				if (num == 0)
				{
					if (NativeWindow.userSetProcFlags != 0)
					{
						num = (int)NativeWindow.userSetProcFlags;
					}
					else if (NativeWindow.userSetProcFlagsForApp != 0)
					{
						num = (int)NativeWindow.userSetProcFlagsForApp;
					}
					else if (!Application.CustomThreadExceptionHandlerAttached)
					{
						if (Debugger.IsAttached)
						{
							num |= 4;
						}
						else
						{
							num = NativeWindow.AdjustWndProcFlagsFromRegistry(num);
							if ((num & 2) != 0)
							{
								num = NativeWindow.AdjustWndProcFlagsFromMetadata(num);
								if ((num & 16) != 0)
								{
									if ((num & 8) != 0)
									{
										num = NativeWindow.AdjustWndProcFlagsFromConfig(num);
									}
									else
									{
										num |= 4;
									}
								}
							}
						}
					}
					num |= 1;
					NativeWindow.wndProcFlags = (byte)num;
				}
				return num;
			}
		}

		// Token: 0x17000B1E RID: 2846
		// (get) Token: 0x06002DE4 RID: 11748 RVA: 0x000D5BA7 File Offset: 0x000D3DA7
		internal static bool WndProcShouldBeDebuggable
		{
			get
			{
				return (NativeWindow.WndProcFlags & 4) != 0;
			}
		}

		// Token: 0x06002DE5 RID: 11749 RVA: 0x000D5BB4 File Offset: 0x000D3DB4
		private static void AddWindowToTable(IntPtr handle, NativeWindow window)
		{
			object obj = NativeWindow.internalSyncObject;
			lock (obj)
			{
				if (NativeWindow.handleCount >= NativeWindow.hashLoadSize)
				{
					NativeWindow.ExpandTable();
				}
				NativeWindow.anyHandleCreated = true;
				NativeWindow.anyHandleCreatedInApp = true;
				uint num2;
				uint num3;
				uint num = NativeWindow.InitHash(handle, NativeWindow.hashBuckets.Length, out num2, out num3);
				int num4 = 0;
				int num5 = -1;
				GCHandle window2 = GCHandle.Alloc(window, GCHandleType.Weak);
				int num6;
				for (;;)
				{
					num6 = (int)(num2 % (uint)NativeWindow.hashBuckets.Length);
					if (num5 == -1 && NativeWindow.hashBuckets[num6].handle == new IntPtr(-1) && NativeWindow.hashBuckets[num6].hash_coll < 0)
					{
						num5 = num6;
					}
					if (NativeWindow.hashBuckets[num6].handle == IntPtr.Zero || (NativeWindow.hashBuckets[num6].handle == new IntPtr(-1) && ((long)NativeWindow.hashBuckets[num6].hash_coll & (long)((ulong)-2147483648)) == 0L))
					{
						break;
					}
					if ((long)(NativeWindow.hashBuckets[num6].hash_coll & 2147483647) == (long)((ulong)num) && handle == NativeWindow.hashBuckets[num6].handle)
					{
						goto Block_11;
					}
					if (num5 == -1)
					{
						NativeWindow.HandleBucket[] array = NativeWindow.hashBuckets;
						int num7 = num6;
						array[num7].hash_coll = (array[num7].hash_coll | int.MinValue);
					}
					num2 += num3;
					if (++num4 >= NativeWindow.hashBuckets.Length)
					{
						goto Block_15;
					}
				}
				if (num5 != -1)
				{
					num6 = num5;
				}
				NativeWindow.hashBuckets[num6].window = window2;
				NativeWindow.hashBuckets[num6].handle = handle;
				NativeWindow.HandleBucket[] array2 = NativeWindow.hashBuckets;
				int num8 = num6;
				array2[num8].hash_coll = (array2[num8].hash_coll | (int)num);
				NativeWindow.handleCount++;
				return;
				Block_11:
				GCHandle window3 = NativeWindow.hashBuckets[num6].window;
				if (window3.IsAllocated)
				{
					if (window3.Target != null)
					{
						window.previousWindow = (NativeWindow)window3.Target;
						window.previousWindow.nextWindow = window;
					}
					window3.Free();
				}
				NativeWindow.hashBuckets[num6].window = window2;
				return;
				Block_15:
				if (num5 != -1)
				{
					NativeWindow.hashBuckets[num5].window = window2;
					NativeWindow.hashBuckets[num5].handle = handle;
					NativeWindow.HandleBucket[] array3 = NativeWindow.hashBuckets;
					int num9 = num5;
					array3[num9].hash_coll = (array3[num9].hash_coll | (int)num);
					NativeWindow.handleCount++;
				}
			}
		}

		// Token: 0x06002DE6 RID: 11750 RVA: 0x000D5E48 File Offset: 0x000D4048
		internal static void AddWindowToIDTable(object wrapper, IntPtr handle)
		{
			NativeWindow.hashForIdHandle[NativeWindow.globalID] = handle;
			NativeWindow.hashForHandleId[handle] = NativeWindow.globalID;
			UnsafeNativeMethods.SetWindowLong(new HandleRef(wrapper, handle), -12, new HandleRef(wrapper, (IntPtr)((int)NativeWindow.globalID)));
			NativeWindow.globalID += 1;
		}

		// Token: 0x06002DE7 RID: 11751 RVA: 0x000D5EA1 File Offset: 0x000D40A1
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static int AdjustWndProcFlagsFromConfig(int wndProcFlags)
		{
			if (WindowsFormsSection.GetSection().JitDebugging)
			{
				wndProcFlags |= 4;
			}
			return wndProcFlags;
		}

		// Token: 0x06002DE8 RID: 11752 RVA: 0x000D5EB8 File Offset: 0x000D40B8
		private static int AdjustWndProcFlagsFromRegistry(int wndProcFlags)
		{
			new RegistryPermission(PermissionState.Unrestricted).Assert();
			try
			{
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\.NETFramework");
				if (registryKey == null)
				{
					return wndProcFlags;
				}
				try
				{
					object value = registryKey.GetValue("DbgJITDebugLaunchSetting");
					if (value != null)
					{
						int num = 0;
						try
						{
							num = (int)value;
						}
						catch (InvalidCastException)
						{
							num = 1;
						}
						if (num != 1)
						{
							wndProcFlags |= 2;
							wndProcFlags |= 8;
						}
					}
					else if (registryKey.GetValue("DbgManagedDebugger") != null)
					{
						wndProcFlags |= 2;
						wndProcFlags |= 8;
					}
				}
				finally
				{
					registryKey.Close();
				}
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return wndProcFlags;
		}

		// Token: 0x06002DE9 RID: 11753 RVA: 0x000D5F68 File Offset: 0x000D4168
		private static int AdjustWndProcFlagsFromMetadata(int wndProcFlags)
		{
			if ((wndProcFlags & 2) != 0)
			{
				Assembly entryAssembly = Assembly.GetEntryAssembly();
				if (entryAssembly != null && Attribute.IsDefined(entryAssembly, typeof(DebuggableAttribute)))
				{
					Attribute[] customAttributes = Attribute.GetCustomAttributes(entryAssembly, typeof(DebuggableAttribute));
					if (customAttributes.Length != 0)
					{
						DebuggableAttribute debuggableAttribute = (DebuggableAttribute)customAttributes[0];
						if (debuggableAttribute.IsJITTrackingEnabled)
						{
							wndProcFlags |= 16;
						}
					}
				}
			}
			return wndProcFlags;
		}

		/// <summary>Assigns a handle to this window. </summary>
		/// <param name="handle">The handle to assign to this window. </param>
		/// <exception cref="T:System.Exception">This window already has a handle. </exception>
		/// <exception cref="T:System.ComponentModel.Win32Exception">The windows procedure for the associated native window could not be retrieved.</exception>
		// Token: 0x06002DEA RID: 11754 RVA: 0x000D5FC8 File Offset: 0x000D41C8
		public void AssignHandle(IntPtr handle)
		{
			this.AssignHandle(handle, true);
		}

		// Token: 0x06002DEB RID: 11755 RVA: 0x000D5FD4 File Offset: 0x000D41D4
		internal void AssignHandle(IntPtr handle, bool assignUniqueID)
		{
			lock (this)
			{
				this.CheckReleased();
				this.handle = handle;
				if (DpiHelper.EnableDpiChangedHighDpiImprovements && this.windowDpiAwarenessContext != DpiAwarenessContext.DPI_AWARENESS_CONTEXT_UNSPECIFIED)
				{
					DpiAwarenessContext dpiAwarenessContext = CommonUnsafeNativeMethods.TryGetDpiAwarenessContextForWindow(this.handle);
					if (dpiAwarenessContext != DpiAwarenessContext.DPI_AWARENESS_CONTEXT_UNSPECIFIED && !CommonUnsafeNativeMethods.TryFindDpiAwarenessContextsEqual(this.windowDpiAwarenessContext, dpiAwarenessContext))
					{
						this.windowDpiAwarenessContext = dpiAwarenessContext;
					}
				}
				if (NativeWindow.userDefWindowProc == IntPtr.Zero)
				{
					string lpProcName = (Marshal.SystemDefaultCharSize == 1) ? "DefWindowProcA" : "DefWindowProcW";
					NativeWindow.userDefWindowProc = UnsafeNativeMethods.GetProcAddress(new HandleRef(null, UnsafeNativeMethods.GetModuleHandle("user32.dll")), lpProcName);
					if (NativeWindow.userDefWindowProc == IntPtr.Zero)
					{
						throw new Win32Exception();
					}
				}
				this.defWindowProc = UnsafeNativeMethods.GetWindowLong(new HandleRef(this, handle), -4);
				if (NativeWindow.WndProcShouldBeDebuggable)
				{
					this.windowProc = new NativeMethods.WndProc(this.DebuggableCallback);
				}
				else
				{
					this.windowProc = new NativeMethods.WndProc(this.Callback);
				}
				NativeWindow.AddWindowToTable(handle, this);
				UnsafeNativeMethods.SetWindowLong(new HandleRef(this, handle), -4, this.windowProc);
				this.windowProcPtr = UnsafeNativeMethods.GetWindowLong(new HandleRef(this, handle), -4);
				if (assignUniqueID && ((int)((long)UnsafeNativeMethods.GetWindowLong(new HandleRef(this, handle), -16)) & 1073741824) != 0 && (int)((long)UnsafeNativeMethods.GetWindowLong(new HandleRef(this, handle), -12)) == 0)
				{
					UnsafeNativeMethods.SetWindowLong(new HandleRef(this, handle), -12, new HandleRef(this, handle));
				}
				if (this.suppressedGC)
				{
					new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
					try
					{
						GC.ReRegisterForFinalize(this);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					this.suppressedGC = false;
				}
				this.OnHandleChange();
			}
		}

		// Token: 0x06002DEC RID: 11756 RVA: 0x000D61B0 File Offset: 0x000D43B0
		private IntPtr Callback(IntPtr hWnd, int msg, IntPtr wparam, IntPtr lparam)
		{
			Message message = Message.Create(hWnd, msg, wparam, lparam);
			try
			{
				if (this.weakThisPtr.IsAlive && this.weakThisPtr.Target != null)
				{
					this.WndProc(ref message);
				}
				else
				{
					this.DefWndProc(ref message);
				}
			}
			catch (Exception e)
			{
				this.OnThreadException(e);
			}
			finally
			{
				if (msg == 130)
				{
					this.ReleaseHandle(false);
				}
				if (msg == NativeMethods.WM_UIUNSUBCLASS)
				{
					this.ReleaseHandle(true);
				}
			}
			return message.Result;
		}

		// Token: 0x06002DED RID: 11757 RVA: 0x000D6244 File Offset: 0x000D4444
		private void CheckReleased()
		{
			if (this.handle != IntPtr.Zero)
			{
				throw new InvalidOperationException(SR.GetString("HandleAlreadyExists"));
			}
		}

		/// <summary>Creates a window and its handle with the specified creation parameters. </summary>
		/// <param name="cp">A <see cref="T:System.Windows.Forms.CreateParams" /> that specifies the creation parameters for this window. </param>
		/// <exception cref="T:System.OutOfMemoryException">The operating system ran out of resources when trying to create the native window.</exception>
		/// <exception cref="T:System.ComponentModel.Win32Exception">The native Win32 API could not create the specified window. </exception>
		/// <exception cref="T:System.InvalidOperationException">The handle of the current native window is already assigned; in explanation, the <see cref="P:System.Windows.Forms.NativeWindow.Handle" /> property is not equal to <see cref="F:System.IntPtr.Zero" />.</exception>
		// Token: 0x06002DEE RID: 11758 RVA: 0x000D6268 File Offset: 0x000D4468
		public virtual void CreateHandle(CreateParams cp)
		{
			IntSecurity.CreateAnyWindow.Demand();
			if ((cp.Style & 1073741824) != 1073741824 || cp.Parent == IntPtr.Zero)
			{
				IntSecurity.TopLevelWindow.Demand();
			}
			lock (this)
			{
				this.CheckReleased();
				NativeWindow.WindowClass windowClass = NativeWindow.WindowClass.Create(cp.ClassName, cp.ClassStyle);
				object obj = NativeWindow.createWindowSyncObject;
				lock (obj)
				{
					if (!(this.handle != IntPtr.Zero))
					{
						windowClass.targetWindow = this;
						IntPtr value = IntPtr.Zero;
						int error = 0;
						using (DpiHelper.EnterDpiAwarenessScope(this.windowDpiAwarenessContext))
						{
							IntPtr moduleHandle = UnsafeNativeMethods.GetModuleHandle(null);
							try
							{
								if (cp.Caption != null && cp.Caption.Length > 32767)
								{
									cp.Caption = cp.Caption.Substring(0, 32767);
								}
								value = UnsafeNativeMethods.CreateWindowEx(cp.ExStyle, windowClass.windowClassName, cp.Caption, cp.Style, cp.X, cp.Y, cp.Width, cp.Height, new HandleRef(cp, cp.Parent), NativeMethods.NullHandleRef, new HandleRef(null, moduleHandle), cp.Param);
								error = Marshal.GetLastWin32Error();
							}
							catch (NullReferenceException innerException)
							{
								throw new OutOfMemoryException(SR.GetString("ErrorCreatingHandle"), innerException);
							}
						}
						windowClass.targetWindow = null;
						if (value == IntPtr.Zero)
						{
							throw new Win32Exception(error, SR.GetString("ErrorCreatingHandle"));
						}
						this.ownHandle = true;
						System.Internal.HandleCollector.Add(value, NativeMethods.CommonHandles.Window);
					}
				}
			}
		}

		// Token: 0x06002DEF RID: 11759 RVA: 0x000D6488 File Offset: 0x000D4688
		private IntPtr DebuggableCallback(IntPtr hWnd, int msg, IntPtr wparam, IntPtr lparam)
		{
			Message message = Message.Create(hWnd, msg, wparam, lparam);
			try
			{
				if (this.weakThisPtr.IsAlive && this.weakThisPtr.Target != null)
				{
					this.WndProc(ref message);
				}
				else
				{
					this.DefWndProc(ref message);
				}
			}
			finally
			{
				if (msg == 130)
				{
					this.ReleaseHandle(false);
				}
				if (msg == NativeMethods.WM_UIUNSUBCLASS)
				{
					this.ReleaseHandle(true);
				}
			}
			return message.Result;
		}

		/// <summary>Invokes the default window procedure associated with this window. </summary>
		/// <param name="m">The message that is currently being processed. </param>
		// Token: 0x06002DF0 RID: 11760 RVA: 0x000D6504 File Offset: 0x000D4704
		public void DefWndProc(ref Message m)
		{
			if (this.previousWindow != null)
			{
				m.Result = this.previousWindow.Callback(m.HWnd, m.Msg, m.WParam, m.LParam);
				return;
			}
			if (this.defWindowProc == IntPtr.Zero)
			{
				m.Result = UnsafeNativeMethods.DefWindowProc(m.HWnd, m.Msg, m.WParam, m.LParam);
				return;
			}
			m.Result = UnsafeNativeMethods.CallWindowProc(this.defWindowProc, m.HWnd, m.Msg, m.WParam, m.LParam);
		}

		/// <summary>Destroys the window and its handle. </summary>
		// Token: 0x06002DF1 RID: 11761 RVA: 0x000D65A4 File Offset: 0x000D47A4
		public virtual void DestroyHandle()
		{
			lock (this)
			{
				if (this.handle != IntPtr.Zero)
				{
					if (!UnsafeNativeMethods.DestroyWindow(new HandleRef(this, this.handle)))
					{
						this.UnSubclass();
						UnsafeNativeMethods.PostMessage(new HandleRef(this, this.handle), 16, 0, 0);
					}
					this.handle = IntPtr.Zero;
					this.ownHandle = false;
				}
				new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
				try
				{
					GC.SuppressFinalize(this);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				this.suppressedGC = true;
			}
		}

		// Token: 0x06002DF2 RID: 11762 RVA: 0x000D6658 File Offset: 0x000D4858
		private static void ExpandTable()
		{
			int num = NativeWindow.hashBuckets.Length;
			int prime = NativeWindow.GetPrime(1 + num * 2);
			NativeWindow.HandleBucket[] array = new NativeWindow.HandleBucket[prime];
			for (int i = 0; i < num; i++)
			{
				NativeWindow.HandleBucket handleBucket = NativeWindow.hashBuckets[i];
				if (handleBucket.handle != IntPtr.Zero && handleBucket.handle != new IntPtr(-1))
				{
					uint num2 = (uint)(handleBucket.hash_coll & int.MaxValue);
					uint num3 = 1U + ((num2 >> 5) + 1U) % (uint)(array.Length - 1);
					int num4;
					for (;;)
					{
						num4 = (int)(num2 % (uint)array.Length);
						if (array[num4].handle == IntPtr.Zero || array[num4].handle == new IntPtr(-1))
						{
							break;
						}
						NativeWindow.HandleBucket[] array2 = array;
						int num5 = num4;
						array2[num5].hash_coll = (array2[num5].hash_coll | int.MinValue);
						num2 += num3;
					}
					array[num4].window = handleBucket.window;
					array[num4].handle = handleBucket.handle;
					NativeWindow.HandleBucket[] array3 = array;
					int num6 = num4;
					array3[num6].hash_coll = (array3[num6].hash_coll | (handleBucket.hash_coll & int.MaxValue));
				}
			}
			NativeWindow.hashBuckets = array;
			NativeWindow.hashLoadSize = (int)(0.72f * (float)prime);
			if (NativeWindow.hashLoadSize >= prime)
			{
				NativeWindow.hashLoadSize = prime - 1;
			}
		}

		/// <summary>Retrieves the window associated with the specified handle. </summary>
		/// <param name="handle">A handle to a window. </param>
		/// <returns>The <see cref="T:System.Windows.Forms.NativeWindow" /> associated with the specified handle. This method returns <see langword="null" /> when the handle does not have an associated window.</returns>
		// Token: 0x06002DF3 RID: 11763 RVA: 0x000D67B3 File Offset: 0x000D49B3
		public static NativeWindow FromHandle(IntPtr handle)
		{
			if (handle != IntPtr.Zero && NativeWindow.handleCount > 0)
			{
				return NativeWindow.GetWindowFromTable(handle);
			}
			return null;
		}

		// Token: 0x06002DF4 RID: 11764 RVA: 0x000D67D4 File Offset: 0x000D49D4
		private static int GetPrime(int minSize)
		{
			if (minSize < 0)
			{
				throw new OutOfMemoryException();
			}
			for (int i = 0; i < NativeWindow.primes.Length; i++)
			{
				int num = NativeWindow.primes[i];
				if (num >= minSize)
				{
					return num;
				}
			}
			for (int j = minSize - 2 | 1; j < 2147483647; j += 2)
			{
				bool flag = true;
				if ((j & 1) != 0)
				{
					int num2 = (int)Math.Sqrt((double)j);
					for (int k = 3; k < num2; k += 2)
					{
						if (j % k == 0)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						return j;
					}
				}
				else if (j == 2)
				{
					return j;
				}
			}
			return minSize;
		}

		// Token: 0x06002DF5 RID: 11765 RVA: 0x000D6858 File Offset: 0x000D4A58
		private static NativeWindow GetWindowFromTable(IntPtr handle)
		{
			NativeWindow.HandleBucket[] array = NativeWindow.hashBuckets;
			int num = 0;
			uint num3;
			uint num4;
			uint num2 = NativeWindow.InitHash(handle, array.Length, out num3, out num4);
			NativeWindow.HandleBucket handleBucket;
			for (;;)
			{
				int num5 = (int)(num3 % (uint)array.Length);
				handleBucket = array[num5];
				if (handleBucket.handle == IntPtr.Zero)
				{
					break;
				}
				if ((long)(handleBucket.hash_coll & 2147483647) == (long)((ulong)num2) && handle == handleBucket.handle && handleBucket.window.IsAllocated)
				{
					goto Block_4;
				}
				num3 += num4;
				if (handleBucket.hash_coll >= 0 || ++num >= array.Length)
				{
					goto IL_97;
				}
			}
			return null;
			Block_4:
			return (NativeWindow)handleBucket.window.Target;
			IL_97:
			return null;
		}

		// Token: 0x06002DF6 RID: 11766 RVA: 0x000D6900 File Offset: 0x000D4B00
		internal IntPtr GetHandleFromID(short id)
		{
			IntPtr zero;
			if (NativeWindow.hashForIdHandle == null || !NativeWindow.hashForIdHandle.TryGetValue(id, out zero))
			{
				zero = IntPtr.Zero;
			}
			return zero;
		}

		// Token: 0x06002DF7 RID: 11767 RVA: 0x000D692C File Offset: 0x000D4B2C
		private static uint InitHash(IntPtr handle, int hashsize, out uint seed, out uint incr)
		{
			uint num = (uint)(handle.GetHashCode() & int.MaxValue);
			seed = num;
			incr = 1U + ((seed >> 5) + 1U) % (uint)(hashsize - 1);
			return num;
		}

		/// <summary>Specifies a notification method that is called when the handle for a window is changed. </summary>
		// Token: 0x06002DF8 RID: 11768 RVA: 0x0000701A File Offset: 0x0000521A
		protected virtual void OnHandleChange()
		{
		}

		// Token: 0x06002DF9 RID: 11769 RVA: 0x000D695C File Offset: 0x000D4B5C
		[PrePrepareMethod]
		private static void OnShutdown(object sender, EventArgs e)
		{
			if (NativeWindow.handleCount > 0)
			{
				object obj = NativeWindow.internalSyncObject;
				lock (obj)
				{
					for (int i = 0; i < NativeWindow.hashBuckets.Length; i++)
					{
						NativeWindow.HandleBucket handleBucket = NativeWindow.hashBuckets[i];
						if (handleBucket.handle != IntPtr.Zero && handleBucket.handle != new IntPtr(-1))
						{
							HandleRef handleRef = new HandleRef(handleBucket, handleBucket.handle);
							UnsafeNativeMethods.SetWindowLong(handleRef, -4, new HandleRef(null, NativeWindow.userDefWindowProc));
							UnsafeNativeMethods.SetClassLong(handleRef, -24, NativeWindow.userDefWindowProc);
							UnsafeNativeMethods.PostMessage(handleRef, 16, 0, 0);
							if (handleBucket.window.IsAllocated)
							{
								NativeWindow nativeWindow = (NativeWindow)handleBucket.window.Target;
								if (nativeWindow != null)
								{
									nativeWindow.handle = IntPtr.Zero;
								}
							}
							handleBucket.window.Free();
						}
						NativeWindow.hashBuckets[i].handle = IntPtr.Zero;
						NativeWindow.hashBuckets[i].hash_coll = 0;
					}
					NativeWindow.handleCount = 0;
				}
			}
			NativeWindow.WindowClass.DisposeCache();
		}

		/// <summary>When overridden in a derived class, manages an unhandled thread exception. </summary>
		/// <param name="e">An <see cref="T:System.Exception" /> that specifies the unhandled thread exception. </param>
		// Token: 0x06002DFA RID: 11770 RVA: 0x0000701A File Offset: 0x0000521A
		protected virtual void OnThreadException(Exception e)
		{
		}

		/// <summary>Releases the handle associated with this window. </summary>
		// Token: 0x06002DFB RID: 11771 RVA: 0x000D6AAC File Offset: 0x000D4CAC
		public virtual void ReleaseHandle()
		{
			this.ReleaseHandle(true);
		}

		// Token: 0x06002DFC RID: 11772 RVA: 0x000D6AB8 File Offset: 0x000D4CB8
		private void ReleaseHandle(bool handleValid)
		{
			if (this.handle != IntPtr.Zero)
			{
				lock (this)
				{
					if (this.handle != IntPtr.Zero)
					{
						if (handleValid)
						{
							this.UnSubclass();
						}
						NativeWindow.RemoveWindowFromTable(this.handle, this);
						if (this.ownHandle)
						{
							System.Internal.HandleCollector.Remove(this.handle, NativeMethods.CommonHandles.Window);
							this.ownHandle = false;
						}
						this.handle = IntPtr.Zero;
						if (this.weakThisPtr.IsAlive && this.weakThisPtr.Target != null)
						{
							this.OnHandleChange();
							new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
							try
							{
								GC.SuppressFinalize(this);
							}
							finally
							{
								CodeAccessPermission.RevertAssert();
							}
							this.suppressedGC = true;
						}
					}
				}
			}
		}

		// Token: 0x06002DFD RID: 11773 RVA: 0x000D6BA4 File Offset: 0x000D4DA4
		private static void RemoveWindowFromTable(IntPtr handle, NativeWindow window)
		{
			object obj = NativeWindow.internalSyncObject;
			lock (obj)
			{
				uint num2;
				uint num3;
				uint num = NativeWindow.InitHash(handle, NativeWindow.hashBuckets.Length, out num2, out num3);
				int num4 = 0;
				NativeWindow value = window.PreviousWindow;
				int num5;
				for (;;)
				{
					num5 = (int)(num2 % (uint)NativeWindow.hashBuckets.Length);
					NativeWindow.HandleBucket handleBucket = NativeWindow.hashBuckets[num5];
					if ((long)(handleBucket.hash_coll & 2147483647) == (long)((ulong)num) && handle == handleBucket.handle)
					{
						break;
					}
					num2 += num3;
					if (NativeWindow.hashBuckets[num5].hash_coll >= 0 || ++num4 >= NativeWindow.hashBuckets.Length)
					{
						goto IL_1ED;
					}
				}
				bool flag2 = window.nextWindow == null;
				bool flag3 = NativeWindow.IsRootWindowInListWithChildren(window);
				if (window.previousWindow != null)
				{
					window.previousWindow.nextWindow = window.nextWindow;
				}
				if (window.nextWindow != null)
				{
					window.nextWindow.defWindowProc = window.defWindowProc;
					window.nextWindow.previousWindow = window.previousWindow;
				}
				window.nextWindow = null;
				window.previousWindow = null;
				if (flag3)
				{
					if (NativeWindow.hashBuckets[num5].window.IsAllocated)
					{
						NativeWindow.hashBuckets[num5].window.Free();
					}
					NativeWindow.hashBuckets[num5].window = GCHandle.Alloc(value, GCHandleType.Weak);
				}
				else if (flag2)
				{
					NativeWindow.HandleBucket[] array = NativeWindow.hashBuckets;
					int num6 = num5;
					array[num6].hash_coll = (array[num6].hash_coll & int.MinValue);
					if (NativeWindow.hashBuckets[num5].hash_coll != 0)
					{
						NativeWindow.hashBuckets[num5].handle = new IntPtr(-1);
					}
					else
					{
						NativeWindow.hashBuckets[num5].handle = IntPtr.Zero;
					}
					if (NativeWindow.hashBuckets[num5].window.IsAllocated)
					{
						NativeWindow.hashBuckets[num5].window.Free();
					}
					NativeWindow.handleCount--;
				}
				IL_1ED:;
			}
		}

		// Token: 0x06002DFE RID: 11774 RVA: 0x000D6DC8 File Offset: 0x000D4FC8
		private static bool IsRootWindowInListWithChildren(NativeWindow window)
		{
			return window.PreviousWindow != null && window.nextWindow == null;
		}

		// Token: 0x06002DFF RID: 11775 RVA: 0x000D6DE0 File Offset: 0x000D4FE0
		internal static void RemoveWindowFromIDTable(IntPtr handle)
		{
			short key = NativeWindow.hashForHandleId[handle];
			NativeWindow.hashForHandleId.Remove(handle);
			NativeWindow.hashForIdHandle.Remove(key);
		}

		// Token: 0x06002E00 RID: 11776 RVA: 0x000D6E14 File Offset: 0x000D5014
		internal static void SetUnhandledExceptionModeInternal(UnhandledExceptionMode mode, bool threadScope)
		{
			if (!threadScope && NativeWindow.anyHandleCreatedInApp)
			{
				throw new InvalidOperationException(SR.GetString("ApplicationCannotChangeApplicationExceptionMode"));
			}
			if (threadScope && NativeWindow.anyHandleCreated)
			{
				throw new InvalidOperationException(SR.GetString("ApplicationCannotChangeThreadExceptionMode"));
			}
			switch (mode)
			{
			case UnhandledExceptionMode.Automatic:
				if (threadScope)
				{
					NativeWindow.userSetProcFlags = 0;
					return;
				}
				NativeWindow.userSetProcFlagsForApp = 0;
				return;
			case UnhandledExceptionMode.ThrowException:
				if (threadScope)
				{
					NativeWindow.userSetProcFlags = 5;
					return;
				}
				NativeWindow.userSetProcFlagsForApp = 5;
				return;
			case UnhandledExceptionMode.CatchException:
				if (threadScope)
				{
					NativeWindow.userSetProcFlags = 1;
					return;
				}
				NativeWindow.userSetProcFlagsForApp = 1;
				return;
			default:
				throw new InvalidEnumArgumentException("mode", (int)mode, typeof(UnhandledExceptionMode));
			}
		}

		// Token: 0x06002E01 RID: 11777 RVA: 0x000D6EB4 File Offset: 0x000D50B4
		private void UnSubclass()
		{
			bool flag = !this.weakThisPtr.IsAlive || this.weakThisPtr.Target == null;
			HandleRef hWnd = new HandleRef(this, this.handle);
			IntPtr windowLong = UnsafeNativeMethods.GetWindowLong(new HandleRef(this, this.handle), -4);
			if (!(this.windowProcPtr == windowLong))
			{
				if (this.nextWindow == null || this.nextWindow.defWindowProc != this.windowProcPtr)
				{
					UnsafeNativeMethods.SetWindowLong(hWnd, -4, new HandleRef(this, NativeWindow.userDefWindowProc));
				}
				return;
			}
			if (this.previousWindow == null)
			{
				UnsafeNativeMethods.SetWindowLong(hWnd, -4, new HandleRef(this, this.defWindowProc));
				return;
			}
			if (flag)
			{
				UnsafeNativeMethods.SetWindowLong(hWnd, -4, new HandleRef(this, NativeWindow.userDefWindowProc));
				return;
			}
			UnsafeNativeMethods.SetWindowLong(hWnd, -4, this.previousWindow.windowProc);
		}

		/// <summary>Invokes the default window procedure associated with this window. </summary>
		/// <param name="m">A <see cref="T:System.Windows.Forms.Message" /> that is associated with the current Windows message. </param>
		// Token: 0x06002E02 RID: 11778 RVA: 0x000D6F8F File Offset: 0x000D518F
		protected virtual void WndProc(ref Message m)
		{
			this.DefWndProc(ref m);
		}

		// Token: 0x04001CE2 RID: 7394
		private static readonly TraceSwitch WndProcChoice;

		// Token: 0x04001CE3 RID: 7395
		private static readonly int[] primes = new int[]
		{
			11,
			17,
			23,
			29,
			37,
			47,
			59,
			71,
			89,
			107,
			131,
			163,
			197,
			239,
			293,
			353,
			431,
			521,
			631,
			761,
			919,
			1103,
			1327,
			1597,
			1931,
			2333,
			2801,
			3371,
			4049,
			4861,
			5839,
			7013,
			8419,
			10103,
			12143,
			14591,
			17519,
			21023,
			25229,
			30293,
			36353,
			43627,
			52361,
			62851,
			75431,
			90523,
			108631,
			130363,
			156437,
			187751,
			225307,
			270371,
			324449,
			389357,
			467237,
			560689,
			672827,
			807403,
			968897,
			1162687,
			1395263,
			1674319,
			2009191,
			2411033,
			2893249,
			3471899,
			4166287,
			4999559,
			5999471,
			7199369
		};

		// Token: 0x04001CE4 RID: 7396
		private const int InitializedFlags = 1;

		// Token: 0x04001CE5 RID: 7397
		private const int DebuggerPresent = 2;

		// Token: 0x04001CE6 RID: 7398
		private const int UseDebuggableWndProc = 4;

		// Token: 0x04001CE7 RID: 7399
		private const int LoadConfigSettings = 8;

		// Token: 0x04001CE8 RID: 7400
		private const int AssemblyIsDebuggable = 16;

		// Token: 0x04001CE9 RID: 7401
		[ThreadStatic]
		private static bool anyHandleCreated;

		// Token: 0x04001CEA RID: 7402
		private static bool anyHandleCreatedInApp;

		// Token: 0x04001CEB RID: 7403
		private const float hashLoadFactor = 0.72f;

		// Token: 0x04001CEC RID: 7404
		private static int handleCount;

		// Token: 0x04001CED RID: 7405
		private static int hashLoadSize;

		// Token: 0x04001CEE RID: 7406
		private static NativeWindow.HandleBucket[] hashBuckets;

		// Token: 0x04001CEF RID: 7407
		private static IntPtr userDefWindowProc;

		// Token: 0x04001CF0 RID: 7408
		[ThreadStatic]
		private static byte wndProcFlags = 0;

		// Token: 0x04001CF1 RID: 7409
		[ThreadStatic]
		private static byte userSetProcFlags = 0;

		// Token: 0x04001CF2 RID: 7410
		private static byte userSetProcFlagsForApp;

		// Token: 0x04001CF3 RID: 7411
		private static short globalID = 1;

		// Token: 0x04001CF4 RID: 7412
		private static Dictionary<short, IntPtr> hashForIdHandle;

		// Token: 0x04001CF5 RID: 7413
		private static Dictionary<IntPtr, short> hashForHandleId;

		// Token: 0x04001CF6 RID: 7414
		private static object internalSyncObject = new object();

		// Token: 0x04001CF7 RID: 7415
		private static object createWindowSyncObject = new object();

		// Token: 0x04001CF8 RID: 7416
		private IntPtr handle;

		// Token: 0x04001CF9 RID: 7417
		private NativeMethods.WndProc windowProc;

		// Token: 0x04001CFA RID: 7418
		private IntPtr windowProcPtr;

		// Token: 0x04001CFB RID: 7419
		private IntPtr defWindowProc;

		// Token: 0x04001CFC RID: 7420
		private bool suppressedGC;

		// Token: 0x04001CFD RID: 7421
		private bool ownHandle;

		// Token: 0x04001CFE RID: 7422
		private NativeWindow previousWindow;

		// Token: 0x04001CFF RID: 7423
		private NativeWindow nextWindow;

		// Token: 0x04001D00 RID: 7424
		private WeakReference weakThisPtr;

		// Token: 0x04001D01 RID: 7425
		private DpiAwarenessContext windowDpiAwarenessContext = DpiHelper.EnableDpiChangedHighDpiImprovements ? CommonUnsafeNativeMethods.TryGetThreadDpiAwarenessContext() : DpiAwarenessContext.DPI_AWARENESS_CONTEXT_UNSPECIFIED;

		// Token: 0x020006FB RID: 1787
		private struct HandleBucket
		{
			// Token: 0x0400402F RID: 16431
			public IntPtr handle;

			// Token: 0x04004030 RID: 16432
			public GCHandle window;

			// Token: 0x04004031 RID: 16433
			public int hash_coll;
		}

		// Token: 0x020006FC RID: 1788
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		private class WindowClass
		{
			// Token: 0x06005F98 RID: 24472 RVA: 0x00188C85 File Offset: 0x00186E85
			internal WindowClass(string className, int classStyle)
			{
				this.className = className;
				this.classStyle = classStyle;
				this.RegisterClass();
			}

			// Token: 0x06005F99 RID: 24473 RVA: 0x00188CA1 File Offset: 0x00186EA1
			public IntPtr Callback(IntPtr hWnd, int msg, IntPtr wparam, IntPtr lparam)
			{
				UnsafeNativeMethods.SetWindowLong(new HandleRef(null, hWnd), -4, new HandleRef(this, this.defWindowProc));
				this.targetWindow.AssignHandle(hWnd);
				return this.targetWindow.Callback(hWnd, msg, wparam, lparam);
			}

			// Token: 0x06005F9A RID: 24474 RVA: 0x00188CDC File Offset: 0x00186EDC
			internal static NativeWindow.WindowClass Create(string className, int classStyle)
			{
				object obj = NativeWindow.WindowClass.wcInternalSyncObject;
				NativeWindow.WindowClass result;
				lock (obj)
				{
					NativeWindow.WindowClass windowClass = NativeWindow.WindowClass.cache;
					if (className == null)
					{
						while (windowClass != null)
						{
							if (windowClass.className == null && windowClass.classStyle == classStyle)
							{
								break;
							}
							windowClass = windowClass.next;
						}
					}
					else
					{
						while (windowClass != null && !className.Equals(windowClass.className))
						{
							windowClass = windowClass.next;
						}
					}
					if (windowClass == null)
					{
						windowClass = new NativeWindow.WindowClass(className, classStyle);
						windowClass.next = NativeWindow.WindowClass.cache;
						NativeWindow.WindowClass.cache = windowClass;
					}
					else if (!windowClass.registered)
					{
						windowClass.RegisterClass();
					}
					result = windowClass;
				}
				return result;
			}

			// Token: 0x06005F9B RID: 24475 RVA: 0x00188D84 File Offset: 0x00186F84
			internal static void DisposeCache()
			{
				object obj = NativeWindow.WindowClass.wcInternalSyncObject;
				lock (obj)
				{
					for (NativeWindow.WindowClass windowClass = NativeWindow.WindowClass.cache; windowClass != null; windowClass = windowClass.next)
					{
						windowClass.UnregisterClass();
					}
				}
			}

			// Token: 0x06005F9C RID: 24476 RVA: 0x00188DD8 File Offset: 0x00186FD8
			private string GetFullClassName(string className)
			{
				StringBuilder stringBuilder = new StringBuilder(50);
				stringBuilder.Append(Application.WindowsFormsVersion);
				stringBuilder.Append('.');
				stringBuilder.Append(className);
				stringBuilder.Append(".app.");
				stringBuilder.Append(NativeWindow.WindowClass.domainQualifier);
				stringBuilder.Append('.');
				string name = Convert.ToString(AppDomain.CurrentDomain.GetHashCode(), 16);
				stringBuilder.Append(VersioningHelper.MakeVersionSafeName(name, ResourceScope.Process, ResourceScope.AppDomain));
				return stringBuilder.ToString();
			}

			// Token: 0x06005F9D RID: 24477 RVA: 0x00188E54 File Offset: 0x00187054
			private void RegisterClass()
			{
				NativeMethods.WNDCLASS_D wndclass_D = new NativeMethods.WNDCLASS_D();
				if (NativeWindow.userDefWindowProc == IntPtr.Zero)
				{
					string lpProcName = (Marshal.SystemDefaultCharSize == 1) ? "DefWindowProcA" : "DefWindowProcW";
					NativeWindow.userDefWindowProc = UnsafeNativeMethods.GetProcAddress(new HandleRef(null, UnsafeNativeMethods.GetModuleHandle("user32.dll")), lpProcName);
					if (NativeWindow.userDefWindowProc == IntPtr.Zero)
					{
						throw new Win32Exception();
					}
				}
				string text;
				if (this.className == null)
				{
					wndclass_D.hbrBackground = UnsafeNativeMethods.GetStockObject(5);
					wndclass_D.style = this.classStyle;
					this.defWindowProc = NativeWindow.userDefWindowProc;
					text = "Window." + Convert.ToString(this.classStyle, 16);
					this.hashCode = 0;
				}
				else
				{
					NativeMethods.WNDCLASS_I wndclass_I = new NativeMethods.WNDCLASS_I();
					bool classInfo = UnsafeNativeMethods.GetClassInfo(NativeMethods.NullHandleRef, this.className, wndclass_I);
					int lastWin32Error = Marshal.GetLastWin32Error();
					if (!classInfo)
					{
						throw new Win32Exception(lastWin32Error, SR.GetString("InvalidWndClsName"));
					}
					wndclass_D.style = wndclass_I.style;
					wndclass_D.cbClsExtra = wndclass_I.cbClsExtra;
					wndclass_D.cbWndExtra = wndclass_I.cbWndExtra;
					wndclass_D.hIcon = wndclass_I.hIcon;
					wndclass_D.hCursor = wndclass_I.hCursor;
					wndclass_D.hbrBackground = wndclass_I.hbrBackground;
					wndclass_D.lpszMenuName = Marshal.PtrToStringAuto(wndclass_I.lpszMenuName);
					text = this.className;
					this.defWindowProc = wndclass_I.lpfnWndProc;
					this.hashCode = this.className.GetHashCode();
				}
				this.windowClassName = this.GetFullClassName(text);
				this.windowProc = new NativeMethods.WndProc(this.Callback);
				wndclass_D.lpfnWndProc = this.windowProc;
				wndclass_D.hInstance = UnsafeNativeMethods.GetModuleHandle(null);
				wndclass_D.lpszClassName = this.windowClassName;
				short num = UnsafeNativeMethods.RegisterClass(wndclass_D);
				if (num == 0)
				{
					int lastWin32Error2 = Marshal.GetLastWin32Error();
					if (lastWin32Error2 == 1410)
					{
						NativeMethods.WNDCLASS_I wndclass_I2 = new NativeMethods.WNDCLASS_I();
						bool classInfo2 = UnsafeNativeMethods.GetClassInfo(new HandleRef(null, UnsafeNativeMethods.GetModuleHandle(null)), this.windowClassName, wndclass_I2);
						if (classInfo2 && wndclass_I2.lpfnWndProc == NativeWindow.UserDefindowProc)
						{
							if (UnsafeNativeMethods.UnregisterClass(this.windowClassName, new HandleRef(null, UnsafeNativeMethods.GetModuleHandle(null))))
							{
								num = UnsafeNativeMethods.RegisterClass(wndclass_D);
							}
							else
							{
								do
								{
									NativeWindow.WindowClass.domainQualifier++;
									this.windowClassName = this.GetFullClassName(text);
									wndclass_D.lpszClassName = this.windowClassName;
									num = UnsafeNativeMethods.RegisterClass(wndclass_D);
								}
								while (num == 0 && Marshal.GetLastWin32Error() == 1410);
							}
						}
					}
					if (num == 0)
					{
						this.windowProc = null;
						throw new Win32Exception(lastWin32Error2);
					}
				}
				this.registered = true;
			}

			// Token: 0x06005F9E RID: 24478 RVA: 0x001890DF File Offset: 0x001872DF
			private void UnregisterClass()
			{
				if (this.registered && UnsafeNativeMethods.UnregisterClass(this.windowClassName, new HandleRef(null, UnsafeNativeMethods.GetModuleHandle(null))))
				{
					this.windowProc = null;
					this.registered = false;
				}
			}

			// Token: 0x04004032 RID: 16434
			internal static NativeWindow.WindowClass cache;

			// Token: 0x04004033 RID: 16435
			internal NativeWindow.WindowClass next;

			// Token: 0x04004034 RID: 16436
			internal string className;

			// Token: 0x04004035 RID: 16437
			internal int classStyle;

			// Token: 0x04004036 RID: 16438
			internal string windowClassName;

			// Token: 0x04004037 RID: 16439
			internal int hashCode;

			// Token: 0x04004038 RID: 16440
			internal IntPtr defWindowProc;

			// Token: 0x04004039 RID: 16441
			internal NativeMethods.WndProc windowProc;

			// Token: 0x0400403A RID: 16442
			internal bool registered;

			// Token: 0x0400403B RID: 16443
			internal NativeWindow targetWindow;

			// Token: 0x0400403C RID: 16444
			private static object wcInternalSyncObject = new object();

			// Token: 0x0400403D RID: 16445
			private static int domainQualifier = 0;
		}
	}
}
