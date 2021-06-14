using System;
using System.Deployment.Internal.Isolation.Manifest;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200005D RID: 93
	internal class Store
	{
		// Token: 0x17000047 RID: 71
		// (get) Token: 0x0600019C RID: 412 RVA: 0x000075A6 File Offset: 0x000057A6
		public IStore InternalStore
		{
			get
			{
				return this._pStore;
			}
		}

		// Token: 0x0600019D RID: 413 RVA: 0x000075AE File Offset: 0x000057AE
		public Store(IStore pStore)
		{
			if (pStore == null)
			{
				throw new ArgumentNullException("pStore");
			}
			this._pStore = pStore;
		}

		// Token: 0x0600019E RID: 414 RVA: 0x000075CC File Offset: 0x000057CC
		[SecuritySafeCritical]
		public uint[] Transact(StoreTransactionOperation[] operations)
		{
			if (operations == null || operations.Length == 0)
			{
				throw new ArgumentException("operations");
			}
			uint[] array = new uint[operations.Length];
			int[] rgResults = new int[operations.Length];
			this._pStore.Transact(new IntPtr(operations.Length), operations, array, rgResults);
			return array;
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00007614 File Offset: 0x00005814
		public void Transact(StoreTransactionOperation[] operations, uint[] rgDispositions, int[] rgResults)
		{
			if (operations == null || operations.Length == 0)
			{
				throw new ArgumentException("operations");
			}
			this._pStore.Transact(new IntPtr(operations.Length), operations, rgDispositions, rgResults);
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00007640 File Offset: 0x00005840
		[SecuritySafeCritical]
		public IDefinitionIdentity BindReferenceToAssemblyIdentity(uint Flags, IReferenceIdentity ReferenceIdentity, uint cDeploymentsToIgnore, IDefinitionIdentity[] DefinitionIdentity_DeploymentsToIgnore)
		{
			Guid iid_IDefinitionIdentity = IsolationInterop.IID_IDefinitionIdentity;
			object obj = this._pStore.BindReferenceToAssembly(Flags, ReferenceIdentity, cDeploymentsToIgnore, DefinitionIdentity_DeploymentsToIgnore, ref iid_IDefinitionIdentity);
			return (IDefinitionIdentity)obj;
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x0000766C File Offset: 0x0000586C
		[SecuritySafeCritical]
		public void CalculateDelimiterOfDeploymentsBasedOnQuota(uint dwFlags, uint cDeployments, IDefinitionAppId[] rgpIDefinitionAppId_Deployments, ref StoreApplicationReference InstallerReference, ulong ulonglongQuota, ref uint Delimiter, ref ulong SizeSharedWithExternalDeployment, ref ulong SizeConsumedByInputDeploymentArray)
		{
			IntPtr zero = IntPtr.Zero;
			this._pStore.CalculateDelimiterOfDeploymentsBasedOnQuota(dwFlags, new IntPtr((long)((ulong)cDeployments)), rgpIDefinitionAppId_Deployments, ref InstallerReference, ulonglongQuota, ref zero, ref SizeSharedWithExternalDeployment, ref SizeConsumedByInputDeploymentArray);
			Delimiter = (uint)zero.ToInt64();
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x000076A8 File Offset: 0x000058A8
		[SecuritySafeCritical]
		public ICMS BindReferenceToAssemblyManifest(uint Flags, IReferenceIdentity ReferenceIdentity, uint cDeploymentsToIgnore, IDefinitionIdentity[] DefinitionIdentity_DeploymentsToIgnore)
		{
			Guid iid_ICMS = IsolationInterop.IID_ICMS;
			object obj = this._pStore.BindReferenceToAssembly(Flags, ReferenceIdentity, cDeploymentsToIgnore, DefinitionIdentity_DeploymentsToIgnore, ref iid_ICMS);
			return (ICMS)obj;
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x000076D4 File Offset: 0x000058D4
		[SecuritySafeCritical]
		public ICMS GetAssemblyManifest(uint Flags, IDefinitionIdentity DefinitionIdentity)
		{
			Guid iid_ICMS = IsolationInterop.IID_ICMS;
			object assemblyInformation = this._pStore.GetAssemblyInformation(Flags, DefinitionIdentity, ref iid_ICMS);
			return (ICMS)assemblyInformation;
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00007700 File Offset: 0x00005900
		[SecuritySafeCritical]
		public IDefinitionIdentity GetAssemblyIdentity(uint Flags, IDefinitionIdentity DefinitionIdentity)
		{
			Guid iid_IDefinitionIdentity = IsolationInterop.IID_IDefinitionIdentity;
			object assemblyInformation = this._pStore.GetAssemblyInformation(Flags, DefinitionIdentity, ref iid_IDefinitionIdentity);
			return (IDefinitionIdentity)assemblyInformation;
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00007729 File Offset: 0x00005929
		public StoreAssemblyEnumeration EnumAssemblies(Store.EnumAssembliesFlags Flags)
		{
			return this.EnumAssemblies(Flags, null);
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00007734 File Offset: 0x00005934
		[SecuritySafeCritical]
		public StoreAssemblyEnumeration EnumAssemblies(Store.EnumAssembliesFlags Flags, IReferenceIdentity refToMatch)
		{
			Guid guidOfType = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_ASSEMBLY));
			object obj = this._pStore.EnumAssemblies((uint)Flags, refToMatch, ref guidOfType);
			return new StoreAssemblyEnumeration((IEnumSTORE_ASSEMBLY)obj);
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x0000776C File Offset: 0x0000596C
		[SecuritySafeCritical]
		public StoreAssemblyFileEnumeration EnumFiles(Store.EnumAssemblyFilesFlags Flags, IDefinitionIdentity Assembly)
		{
			Guid guidOfType = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_ASSEMBLY_FILE));
			object obj = this._pStore.EnumFiles((uint)Flags, Assembly, ref guidOfType);
			return new StoreAssemblyFileEnumeration((IEnumSTORE_ASSEMBLY_FILE)obj);
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x000077A4 File Offset: 0x000059A4
		[SecuritySafeCritical]
		public StoreAssemblyFileEnumeration EnumPrivateFiles(Store.EnumApplicationPrivateFiles Flags, IDefinitionAppId Application, IDefinitionIdentity Assembly)
		{
			Guid guidOfType = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_ASSEMBLY_FILE));
			object obj = this._pStore.EnumPrivateFiles((uint)Flags, Application, Assembly, ref guidOfType);
			return new StoreAssemblyFileEnumeration((IEnumSTORE_ASSEMBLY_FILE)obj);
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x000077E0 File Offset: 0x000059E0
		[SecuritySafeCritical]
		public IEnumSTORE_ASSEMBLY_INSTALLATION_REFERENCE EnumInstallationReferences(Store.EnumAssemblyInstallReferenceFlags Flags, IDefinitionIdentity Assembly)
		{
			Guid guidOfType = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_ASSEMBLY_INSTALLATION_REFERENCE));
			object obj = this._pStore.EnumInstallationReferences((uint)Flags, Assembly, ref guidOfType);
			return (IEnumSTORE_ASSEMBLY_INSTALLATION_REFERENCE)obj;
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00007814 File Offset: 0x00005A14
		[SecuritySafeCritical]
		public Store.IPathLock LockAssemblyPath(IDefinitionIdentity asm)
		{
			IntPtr c;
			string path = this._pStore.LockAssemblyPath(0U, asm, out c);
			return new Store.AssemblyPathLock(this._pStore, c, path);
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00007840 File Offset: 0x00005A40
		[SecuritySafeCritical]
		public Store.IPathLock LockApplicationPath(IDefinitionAppId app)
		{
			IntPtr c;
			string path = this._pStore.LockApplicationPath(0U, app, out c);
			return new Store.ApplicationPathLock(this._pStore, c, path);
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0000786C File Offset: 0x00005A6C
		[SecuritySafeCritical]
		public ulong QueryChangeID(IDefinitionIdentity asm)
		{
			return this._pStore.QueryChangeID(asm);
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00007888 File Offset: 0x00005A88
		[SecuritySafeCritical]
		public StoreCategoryEnumeration EnumCategories(Store.EnumCategoriesFlags Flags, IReferenceIdentity CategoryMatch)
		{
			Guid guidOfType = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_CATEGORY));
			object obj = this._pStore.EnumCategories((uint)Flags, CategoryMatch, ref guidOfType);
			return new StoreCategoryEnumeration((IEnumSTORE_CATEGORY)obj);
		}

		// Token: 0x060001AE RID: 430 RVA: 0x000078C0 File Offset: 0x00005AC0
		public StoreSubcategoryEnumeration EnumSubcategories(Store.EnumSubcategoriesFlags Flags, IDefinitionIdentity CategoryMatch)
		{
			return this.EnumSubcategories(Flags, CategoryMatch, null);
		}

		// Token: 0x060001AF RID: 431 RVA: 0x000078CC File Offset: 0x00005ACC
		[SecuritySafeCritical]
		public StoreSubcategoryEnumeration EnumSubcategories(Store.EnumSubcategoriesFlags Flags, IDefinitionIdentity Category, string SearchPattern)
		{
			Guid guidOfType = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_CATEGORY_SUBCATEGORY));
			object obj = this._pStore.EnumSubcategories((uint)Flags, Category, SearchPattern, ref guidOfType);
			return new StoreSubcategoryEnumeration((IEnumSTORE_CATEGORY_SUBCATEGORY)obj);
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00007908 File Offset: 0x00005B08
		[SecuritySafeCritical]
		public StoreCategoryInstanceEnumeration EnumCategoryInstances(Store.EnumCategoryInstancesFlags Flags, IDefinitionIdentity Category, string SubCat)
		{
			Guid guidOfType = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_CATEGORY_INSTANCE));
			object obj = this._pStore.EnumCategoryInstances((uint)Flags, Category, SubCat, ref guidOfType);
			return new StoreCategoryInstanceEnumeration((IEnumSTORE_CATEGORY_INSTANCE)obj);
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00007944 File Offset: 0x00005B44
		[SecurityCritical]
		public byte[] GetDeploymentProperty(Store.GetPackagePropertyFlags Flags, IDefinitionAppId Deployment, StoreApplicationReference Reference, Guid PropertySet, string PropertyName)
		{
			BLOB blob = default(BLOB);
			byte[] array = null;
			try
			{
				this._pStore.GetDeploymentProperty((uint)Flags, Deployment, ref Reference, ref PropertySet, PropertyName, out blob);
				array = new byte[blob.Size];
				Marshal.Copy(blob.BlobData, array, 0, (int)blob.Size);
			}
			finally
			{
				blob.Dispose();
			}
			return array;
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x000079AC File Offset: 0x00005BAC
		[SecuritySafeCritical]
		public StoreDeploymentMetadataEnumeration EnumInstallerDeployments(Guid InstallerId, string InstallerName, string InstallerMetadata, IReferenceAppId DeploymentFilter)
		{
			StoreApplicationReference storeApplicationReference = new StoreApplicationReference(InstallerId, InstallerName, InstallerMetadata);
			object obj = this._pStore.EnumInstallerDeploymentMetadata(0U, ref storeApplicationReference, DeploymentFilter, ref IsolationInterop.IID_IEnumSTORE_DEPLOYMENT_METADATA);
			return new StoreDeploymentMetadataEnumeration((IEnumSTORE_DEPLOYMENT_METADATA)obj);
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x000079E8 File Offset: 0x00005BE8
		[SecuritySafeCritical]
		public StoreDeploymentMetadataPropertyEnumeration EnumInstallerDeploymentProperties(Guid InstallerId, string InstallerName, string InstallerMetadata, IDefinitionAppId Deployment)
		{
			StoreApplicationReference storeApplicationReference = new StoreApplicationReference(InstallerId, InstallerName, InstallerMetadata);
			object obj = this._pStore.EnumInstallerDeploymentMetadataProperties(0U, ref storeApplicationReference, Deployment, ref IsolationInterop.IID_IEnumSTORE_DEPLOYMENT_METADATA_PROPERTY);
			return new StoreDeploymentMetadataPropertyEnumeration((IEnumSTORE_DEPLOYMENT_METADATA_PROPERTY)obj);
		}

		// Token: 0x0400018C RID: 396
		private IStore _pStore;

		// Token: 0x02000528 RID: 1320
		[Flags]
		public enum EnumAssembliesFlags
		{
			// Token: 0x0400371D RID: 14109
			Nothing = 0,
			// Token: 0x0400371E RID: 14110
			VisibleOnly = 1,
			// Token: 0x0400371F RID: 14111
			MatchServicing = 2,
			// Token: 0x04003720 RID: 14112
			ForceLibrarySemantics = 4
		}

		// Token: 0x02000529 RID: 1321
		[Flags]
		public enum EnumAssemblyFilesFlags
		{
			// Token: 0x04003722 RID: 14114
			Nothing = 0,
			// Token: 0x04003723 RID: 14115
			IncludeInstalled = 1,
			// Token: 0x04003724 RID: 14116
			IncludeMissing = 2
		}

		// Token: 0x0200052A RID: 1322
		[Flags]
		public enum EnumApplicationPrivateFiles
		{
			// Token: 0x04003726 RID: 14118
			Nothing = 0,
			// Token: 0x04003727 RID: 14119
			IncludeInstalled = 1,
			// Token: 0x04003728 RID: 14120
			IncludeMissing = 2
		}

		// Token: 0x0200052B RID: 1323
		[Flags]
		public enum EnumAssemblyInstallReferenceFlags
		{
			// Token: 0x0400372A RID: 14122
			Nothing = 0
		}

		// Token: 0x0200052C RID: 1324
		public interface IPathLock : IDisposable
		{
			// Token: 0x17001456 RID: 5206
			// (get) Token: 0x0600549F RID: 21663
			string Path { get; }
		}

		// Token: 0x0200052D RID: 1325
		private class AssemblyPathLock : Store.IPathLock, IDisposable
		{
			// Token: 0x060054A0 RID: 21664 RVA: 0x001637B8 File Offset: 0x001619B8
			public AssemblyPathLock(IStore s, IntPtr c, string path)
			{
				this._pSourceStore = s;
				this._pLockCookie = c;
				this._path = path;
			}

			// Token: 0x060054A1 RID: 21665 RVA: 0x001637E0 File Offset: 0x001619E0
			[SecuritySafeCritical]
			private void Dispose(bool fDisposing)
			{
				if (fDisposing)
				{
					GC.SuppressFinalize(this);
				}
				if (this._pLockCookie != IntPtr.Zero)
				{
					this._pSourceStore.ReleaseAssemblyPath(this._pLockCookie);
					this._pLockCookie = IntPtr.Zero;
				}
			}

			// Token: 0x060054A2 RID: 21666 RVA: 0x0016381C File Offset: 0x00161A1C
			~AssemblyPathLock()
			{
				this.Dispose(false);
			}

			// Token: 0x060054A3 RID: 21667 RVA: 0x0016384C File Offset: 0x00161A4C
			void IDisposable.Dispose()
			{
				this.Dispose(true);
			}

			// Token: 0x17001457 RID: 5207
			// (get) Token: 0x060054A4 RID: 21668 RVA: 0x00163855 File Offset: 0x00161A55
			public string Path
			{
				get
				{
					return this._path;
				}
			}

			// Token: 0x0400372B RID: 14123
			private IStore _pSourceStore;

			// Token: 0x0400372C RID: 14124
			private IntPtr _pLockCookie = IntPtr.Zero;

			// Token: 0x0400372D RID: 14125
			private string _path;
		}

		// Token: 0x0200052E RID: 1326
		private class ApplicationPathLock : Store.IPathLock, IDisposable
		{
			// Token: 0x060054A5 RID: 21669 RVA: 0x0016385D File Offset: 0x00161A5D
			public ApplicationPathLock(IStore s, IntPtr c, string path)
			{
				this._pSourceStore = s;
				this._pLockCookie = c;
				this._path = path;
			}

			// Token: 0x060054A6 RID: 21670 RVA: 0x00163885 File Offset: 0x00161A85
			[SecuritySafeCritical]
			private void Dispose(bool fDisposing)
			{
				if (fDisposing)
				{
					GC.SuppressFinalize(this);
				}
				if (this._pLockCookie != IntPtr.Zero)
				{
					this._pSourceStore.ReleaseApplicationPath(this._pLockCookie);
					this._pLockCookie = IntPtr.Zero;
				}
			}

			// Token: 0x060054A7 RID: 21671 RVA: 0x001638C0 File Offset: 0x00161AC0
			~ApplicationPathLock()
			{
				this.Dispose(false);
			}

			// Token: 0x060054A8 RID: 21672 RVA: 0x001638F0 File Offset: 0x00161AF0
			void IDisposable.Dispose()
			{
				this.Dispose(true);
			}

			// Token: 0x17001458 RID: 5208
			// (get) Token: 0x060054A9 RID: 21673 RVA: 0x001638F9 File Offset: 0x00161AF9
			public string Path
			{
				get
				{
					return this._path;
				}
			}

			// Token: 0x0400372E RID: 14126
			private IStore _pSourceStore;

			// Token: 0x0400372F RID: 14127
			private IntPtr _pLockCookie = IntPtr.Zero;

			// Token: 0x04003730 RID: 14128
			private string _path;
		}

		// Token: 0x0200052F RID: 1327
		[Flags]
		public enum EnumCategoriesFlags
		{
			// Token: 0x04003732 RID: 14130
			Nothing = 0
		}

		// Token: 0x02000530 RID: 1328
		[Flags]
		public enum EnumSubcategoriesFlags
		{
			// Token: 0x04003734 RID: 14132
			Nothing = 0
		}

		// Token: 0x02000531 RID: 1329
		[Flags]
		public enum EnumCategoryInstancesFlags
		{
			// Token: 0x04003736 RID: 14134
			Nothing = 0
		}

		// Token: 0x02000532 RID: 1330
		[Flags]
		public enum GetPackagePropertyFlags
		{
			// Token: 0x04003738 RID: 14136
			Nothing = 0
		}
	}
}
