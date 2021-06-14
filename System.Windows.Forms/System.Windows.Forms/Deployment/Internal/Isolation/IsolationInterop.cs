using System;
using System.Deployment.Internal.Isolation.Manifest;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000063 RID: 99
	internal static class IsolationInterop
	{
		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060001D8 RID: 472 RVA: 0x00007ED8 File Offset: 0x000060D8
		public static Store UserStore
		{
			get
			{
				if (IsolationInterop._userStore == null)
				{
					object synchObject = IsolationInterop._synchObject;
					lock (synchObject)
					{
						if (IsolationInterop._userStore == null)
						{
							IsolationInterop._userStore = new Store(IsolationInterop.GetUserStore(0U, IntPtr.Zero, ref IsolationInterop.IID_IStore) as IStore);
						}
					}
				}
				return IsolationInterop._userStore;
			}
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00007F44 File Offset: 0x00006144
		[SecuritySafeCritical]
		public static Store GetUserStore()
		{
			return new Store(IsolationInterop.GetUserStore(0U, IntPtr.Zero, ref IsolationInterop.IID_IStore) as IStore);
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060001DA RID: 474 RVA: 0x00007F60 File Offset: 0x00006160
		public static Store SystemStore
		{
			get
			{
				if (IsolationInterop._systemStore == null)
				{
					object synchObject = IsolationInterop._synchObject;
					lock (synchObject)
					{
						if (IsolationInterop._systemStore == null)
						{
							IsolationInterop._systemStore = new Store(IsolationInterop.GetSystemStore(0U, ref IsolationInterop.IID_IStore) as IStore);
						}
					}
				}
				return IsolationInterop._systemStore;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060001DB RID: 475 RVA: 0x00007FC8 File Offset: 0x000061C8
		public static IIdentityAuthority IdentityAuthority
		{
			[SecuritySafeCritical]
			get
			{
				if (IsolationInterop._idAuth == null)
				{
					object synchObject = IsolationInterop._synchObject;
					lock (synchObject)
					{
						if (IsolationInterop._idAuth == null)
						{
							IsolationInterop._idAuth = IsolationInterop.GetIdentityAuthority();
						}
					}
				}
				return IsolationInterop._idAuth;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060001DC RID: 476 RVA: 0x00008020 File Offset: 0x00006220
		public static IAppIdAuthority AppIdAuthority
		{
			[SecuritySafeCritical]
			get
			{
				if (IsolationInterop._appIdAuth == null)
				{
					object synchObject = IsolationInterop._synchObject;
					lock (synchObject)
					{
						if (IsolationInterop._appIdAuth == null)
						{
							IsolationInterop._appIdAuth = IsolationInterop.GetAppIdAuthority();
						}
					}
				}
				return IsolationInterop._appIdAuth;
			}
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00008078 File Offset: 0x00006278
		[SecuritySafeCritical]
		internal static IActContext CreateActContext(IDefinitionAppId AppId)
		{
			IsolationInterop.CreateActContextParameters createActContextParameters;
			createActContextParameters.Size = (uint)Marshal.SizeOf(typeof(IsolationInterop.CreateActContextParameters));
			createActContextParameters.Flags = 16U;
			createActContextParameters.CustomStoreList = IntPtr.Zero;
			createActContextParameters.CultureFallbackList = IntPtr.Zero;
			createActContextParameters.ProcessorArchitectureList = IntPtr.Zero;
			createActContextParameters.Source = IntPtr.Zero;
			createActContextParameters.ProcArch = 0;
			IsolationInterop.CreateActContextParametersSource createActContextParametersSource;
			createActContextParametersSource.Size = (uint)Marshal.SizeOf(typeof(IsolationInterop.CreateActContextParametersSource));
			createActContextParametersSource.Flags = 0U;
			createActContextParametersSource.SourceType = 1U;
			createActContextParametersSource.Data = IntPtr.Zero;
			IsolationInterop.CreateActContextParametersSourceDefinitionAppid createActContextParametersSourceDefinitionAppid;
			createActContextParametersSourceDefinitionAppid.Size = (uint)Marshal.SizeOf(typeof(IsolationInterop.CreateActContextParametersSourceDefinitionAppid));
			createActContextParametersSourceDefinitionAppid.Flags = 0U;
			createActContextParametersSourceDefinitionAppid.AppId = AppId;
			IActContext result;
			try
			{
				createActContextParametersSource.Data = createActContextParametersSourceDefinitionAppid.ToIntPtr();
				createActContextParameters.Source = createActContextParametersSource.ToIntPtr();
				result = (IsolationInterop.CreateActContext(ref createActContextParameters) as IActContext);
			}
			finally
			{
				if (createActContextParametersSource.Data != IntPtr.Zero)
				{
					IsolationInterop.CreateActContextParametersSourceDefinitionAppid.Destroy(createActContextParametersSource.Data);
					createActContextParametersSource.Data = IntPtr.Zero;
				}
				if (createActContextParameters.Source != IntPtr.Zero)
				{
					IsolationInterop.CreateActContextParametersSource.Destroy(createActContextParameters.Source);
					createActContextParameters.Source = IntPtr.Zero;
				}
			}
			return result;
		}

		// Token: 0x060001DE RID: 478 RVA: 0x000081C4 File Offset: 0x000063C4
		internal static IActContext CreateActContext(IReferenceAppId AppId)
		{
			IsolationInterop.CreateActContextParameters createActContextParameters;
			createActContextParameters.Size = (uint)Marshal.SizeOf(typeof(IsolationInterop.CreateActContextParameters));
			createActContextParameters.Flags = 16U;
			createActContextParameters.CustomStoreList = IntPtr.Zero;
			createActContextParameters.CultureFallbackList = IntPtr.Zero;
			createActContextParameters.ProcessorArchitectureList = IntPtr.Zero;
			createActContextParameters.Source = IntPtr.Zero;
			createActContextParameters.ProcArch = 0;
			IsolationInterop.CreateActContextParametersSource createActContextParametersSource;
			createActContextParametersSource.Size = (uint)Marshal.SizeOf(typeof(IsolationInterop.CreateActContextParametersSource));
			createActContextParametersSource.Flags = 0U;
			createActContextParametersSource.SourceType = 2U;
			createActContextParametersSource.Data = IntPtr.Zero;
			IsolationInterop.CreateActContextParametersSourceReferenceAppid createActContextParametersSourceReferenceAppid;
			createActContextParametersSourceReferenceAppid.Size = (uint)Marshal.SizeOf(typeof(IsolationInterop.CreateActContextParametersSourceReferenceAppid));
			createActContextParametersSourceReferenceAppid.Flags = 0U;
			createActContextParametersSourceReferenceAppid.AppId = AppId;
			IActContext result;
			try
			{
				createActContextParametersSource.Data = createActContextParametersSourceReferenceAppid.ToIntPtr();
				createActContextParameters.Source = createActContextParametersSource.ToIntPtr();
				result = (IsolationInterop.CreateActContext(ref createActContextParameters) as IActContext);
			}
			finally
			{
				if (createActContextParametersSource.Data != IntPtr.Zero)
				{
					IsolationInterop.CreateActContextParametersSourceDefinitionAppid.Destroy(createActContextParametersSource.Data);
					createActContextParametersSource.Data = IntPtr.Zero;
				}
				if (createActContextParameters.Source != IntPtr.Zero)
				{
					IsolationInterop.CreateActContextParametersSource.Destroy(createActContextParameters.Source);
					createActContextParameters.Source = IntPtr.Zero;
				}
			}
			return result;
		}

		// Token: 0x060001DF RID: 479
		[DllImport("clr.dll", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		internal static extern object CreateActContext(ref IsolationInterop.CreateActContextParameters Params);

		// Token: 0x060001E0 RID: 480
		[SecurityCritical]
		[DllImport("clr.dll", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		internal static extern object CreateCMSFromXml([In] byte[] buffer, [In] uint bufferSize, [In] IManifestParseErrorCallback Callback, [In] ref Guid riid);

		// Token: 0x060001E1 RID: 481
		[SecurityCritical]
		[DllImport("clr.dll", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		internal static extern object ParseManifest([MarshalAs(UnmanagedType.LPWStr)] [In] string pszManifestPath, [In] IManifestParseErrorCallback pIManifestParseErrorCallback, [In] ref Guid riid);

		// Token: 0x060001E2 RID: 482
		[SecurityCritical]
		[DllImport("clr.dll", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		private static extern object GetUserStore([In] uint Flags, [In] IntPtr hToken, [In] ref Guid riid);

		// Token: 0x060001E3 RID: 483
		[SecurityCritical]
		[DllImport("clr.dll", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		private static extern object GetSystemStore([In] uint Flags, [In] ref Guid riid);

		// Token: 0x060001E4 RID: 484
		[SecurityCritical]
		[DllImport("clr.dll", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.Interface)]
		private static extern IIdentityAuthority GetIdentityAuthority();

		// Token: 0x060001E5 RID: 485
		[SecurityCritical]
		[DllImport("clr.dll", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.Interface)]
		private static extern IAppIdAuthority GetAppIdAuthority();

		// Token: 0x060001E6 RID: 486
		[DllImport("clr.dll", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		internal static extern object GetUserStateManager([In] uint Flags, [In] IntPtr hToken, [In] ref Guid riid);

		// Token: 0x060001E7 RID: 487 RVA: 0x00008310 File Offset: 0x00006510
		internal static Guid GetGuidOfType(Type type)
		{
			GuidAttribute guidAttribute = (GuidAttribute)Attribute.GetCustomAttribute(type, typeof(GuidAttribute), false);
			return new Guid(guidAttribute.Value);
		}

		// Token: 0x04000198 RID: 408
		private static object _synchObject = new object();

		// Token: 0x04000199 RID: 409
		private static Store _userStore = null;

		// Token: 0x0400019A RID: 410
		private static Store _systemStore = null;

		// Token: 0x0400019B RID: 411
		private static IIdentityAuthority _idAuth = null;

		// Token: 0x0400019C RID: 412
		private static IAppIdAuthority _appIdAuth = null;

		// Token: 0x0400019D RID: 413
		public const string IsolationDllName = "clr.dll";

		// Token: 0x0400019E RID: 414
		public static Guid IID_ICMS = IsolationInterop.GetGuidOfType(typeof(ICMS));

		// Token: 0x0400019F RID: 415
		public static Guid IID_IDefinitionIdentity = IsolationInterop.GetGuidOfType(typeof(IDefinitionIdentity));

		// Token: 0x040001A0 RID: 416
		public static Guid IID_IManifestInformation = IsolationInterop.GetGuidOfType(typeof(IManifestInformation));

		// Token: 0x040001A1 RID: 417
		public static Guid IID_IEnumSTORE_ASSEMBLY = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_ASSEMBLY));

		// Token: 0x040001A2 RID: 418
		public static Guid IID_IEnumSTORE_ASSEMBLY_FILE = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_ASSEMBLY_FILE));

		// Token: 0x040001A3 RID: 419
		public static Guid IID_IEnumSTORE_CATEGORY = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_CATEGORY));

		// Token: 0x040001A4 RID: 420
		public static Guid IID_IEnumSTORE_CATEGORY_INSTANCE = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_CATEGORY_INSTANCE));

		// Token: 0x040001A5 RID: 421
		public static Guid IID_IEnumSTORE_DEPLOYMENT_METADATA = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_DEPLOYMENT_METADATA));

		// Token: 0x040001A6 RID: 422
		public static Guid IID_IEnumSTORE_DEPLOYMENT_METADATA_PROPERTY = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_DEPLOYMENT_METADATA_PROPERTY));

		// Token: 0x040001A7 RID: 423
		public static Guid IID_IStore = IsolationInterop.GetGuidOfType(typeof(IStore));

		// Token: 0x040001A8 RID: 424
		public static Guid GUID_SXS_INSTALL_REFERENCE_SCHEME_OPAQUESTRING = new Guid("2ec93463-b0c3-45e1-8364-327e96aea856");

		// Token: 0x040001A9 RID: 425
		public static Guid SXS_INSTALL_REFERENCE_SCHEME_SXS_STRONGNAME_SIGNED_PRIVATE_ASSEMBLY = new Guid("3ab20ac0-67e8-4512-8385-a487e35df3da");

		// Token: 0x02000533 RID: 1331
		internal struct CreateActContextParameters
		{
			// Token: 0x04003739 RID: 14137
			[MarshalAs(UnmanagedType.U4)]
			public uint Size;

			// Token: 0x0400373A RID: 14138
			[MarshalAs(UnmanagedType.U4)]
			public uint Flags;

			// Token: 0x0400373B RID: 14139
			[MarshalAs(UnmanagedType.SysInt)]
			public IntPtr CustomStoreList;

			// Token: 0x0400373C RID: 14140
			[MarshalAs(UnmanagedType.SysInt)]
			public IntPtr CultureFallbackList;

			// Token: 0x0400373D RID: 14141
			[MarshalAs(UnmanagedType.SysInt)]
			public IntPtr ProcessorArchitectureList;

			// Token: 0x0400373E RID: 14142
			[MarshalAs(UnmanagedType.SysInt)]
			public IntPtr Source;

			// Token: 0x0400373F RID: 14143
			[MarshalAs(UnmanagedType.U2)]
			public ushort ProcArch;

			// Token: 0x02000884 RID: 2180
			[Flags]
			public enum CreateFlags
			{
				// Token: 0x040043CE RID: 17358
				Nothing = 0,
				// Token: 0x040043CF RID: 17359
				StoreListValid = 1,
				// Token: 0x040043D0 RID: 17360
				CultureListValid = 2,
				// Token: 0x040043D1 RID: 17361
				ProcessorFallbackListValid = 4,
				// Token: 0x040043D2 RID: 17362
				ProcessorValid = 8,
				// Token: 0x040043D3 RID: 17363
				SourceValid = 16,
				// Token: 0x040043D4 RID: 17364
				IgnoreVisibility = 32
			}
		}

		// Token: 0x02000534 RID: 1332
		internal struct CreateActContextParametersSource
		{
			// Token: 0x060054AA RID: 21674 RVA: 0x00163904 File Offset: 0x00161B04
			[SecurityCritical]
			public IntPtr ToIntPtr()
			{
				IntPtr intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(this));
				Marshal.StructureToPtr(this, intPtr, false);
				return intPtr;
			}

			// Token: 0x060054AB RID: 21675 RVA: 0x0016393A File Offset: 0x00161B3A
			[SecurityCritical]
			public static void Destroy(IntPtr p)
			{
				Marshal.DestroyStructure(p, typeof(IsolationInterop.CreateActContextParametersSource));
				Marshal.FreeCoTaskMem(p);
			}

			// Token: 0x04003740 RID: 14144
			[MarshalAs(UnmanagedType.U4)]
			public uint Size;

			// Token: 0x04003741 RID: 14145
			[MarshalAs(UnmanagedType.U4)]
			public uint Flags;

			// Token: 0x04003742 RID: 14146
			[MarshalAs(UnmanagedType.U4)]
			public uint SourceType;

			// Token: 0x04003743 RID: 14147
			[MarshalAs(UnmanagedType.SysInt)]
			public IntPtr Data;

			// Token: 0x02000885 RID: 2181
			[Flags]
			public enum SourceFlags
			{
				// Token: 0x040043D6 RID: 17366
				Definition = 1,
				// Token: 0x040043D7 RID: 17367
				Reference = 2
			}
		}

		// Token: 0x02000535 RID: 1333
		internal struct CreateActContextParametersSourceReferenceAppid
		{
			// Token: 0x060054AC RID: 21676 RVA: 0x00163954 File Offset: 0x00161B54
			public IntPtr ToIntPtr()
			{
				IntPtr intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(this));
				Marshal.StructureToPtr(this, intPtr, false);
				return intPtr;
			}

			// Token: 0x060054AD RID: 21677 RVA: 0x0016398A File Offset: 0x00161B8A
			public static void Destroy(IntPtr p)
			{
				Marshal.DestroyStructure(p, typeof(IsolationInterop.CreateActContextParametersSourceReferenceAppid));
				Marshal.FreeCoTaskMem(p);
			}

			// Token: 0x04003744 RID: 14148
			[MarshalAs(UnmanagedType.U4)]
			public uint Size;

			// Token: 0x04003745 RID: 14149
			[MarshalAs(UnmanagedType.U4)]
			public uint Flags;

			// Token: 0x04003746 RID: 14150
			public IReferenceAppId AppId;
		}

		// Token: 0x02000536 RID: 1334
		internal struct CreateActContextParametersSourceDefinitionAppid
		{
			// Token: 0x060054AE RID: 21678 RVA: 0x001639A4 File Offset: 0x00161BA4
			[SecurityCritical]
			public IntPtr ToIntPtr()
			{
				IntPtr intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(this));
				Marshal.StructureToPtr(this, intPtr, false);
				return intPtr;
			}

			// Token: 0x060054AF RID: 21679 RVA: 0x001639DA File Offset: 0x00161BDA
			[SecurityCritical]
			public static void Destroy(IntPtr p)
			{
				Marshal.DestroyStructure(p, typeof(IsolationInterop.CreateActContextParametersSourceDefinitionAppid));
				Marshal.FreeCoTaskMem(p);
			}

			// Token: 0x04003747 RID: 14151
			[MarshalAs(UnmanagedType.U4)]
			public uint Size;

			// Token: 0x04003748 RID: 14152
			[MarshalAs(UnmanagedType.U4)]
			public uint Flags;

			// Token: 0x04003749 RID: 14153
			public IDefinitionAppId AppId;
		}
	}
}
