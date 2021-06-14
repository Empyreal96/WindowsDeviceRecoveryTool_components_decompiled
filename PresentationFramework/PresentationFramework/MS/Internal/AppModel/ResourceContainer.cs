using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Packaging;
using System.Reflection;
using System.Security;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Navigation;
using MS.Internal.Resources;

namespace MS.Internal.AppModel
{
	// Token: 0x0200079D RID: 1949
	internal class ResourceContainer : Package
	{
		// Token: 0x17001CC9 RID: 7369
		// (get) Token: 0x06007A50 RID: 31312 RVA: 0x0022AAB8 File Offset: 0x00228CB8
		internal static ResourceManagerWrapper ApplicationResourceManagerWrapper
		{
			get
			{
				if (ResourceContainer._applicationResourceManagerWrapper == null)
				{
					Assembly resourceAssembly = Application.ResourceAssembly;
					if (resourceAssembly != null)
					{
						ResourceContainer._applicationResourceManagerWrapper = new ResourceManagerWrapper(resourceAssembly);
					}
				}
				return ResourceContainer._applicationResourceManagerWrapper;
			}
		}

		// Token: 0x17001CCA RID: 7370
		// (get) Token: 0x06007A51 RID: 31313 RVA: 0x0022AAEB File Offset: 0x00228CEB
		internal static FileShare FileShare
		{
			get
			{
				return ResourceContainer._fileShare;
			}
		}

		// Token: 0x06007A52 RID: 31314 RVA: 0x0022AAF2 File Offset: 0x00228CF2
		internal ResourceContainer() : base(FileAccess.Read)
		{
		}

		// Token: 0x06007A53 RID: 31315 RVA: 0x00016748 File Offset: 0x00014948
		public override bool PartExists(Uri uri)
		{
			return true;
		}

		// Token: 0x06007A54 RID: 31316 RVA: 0x0022AAFC File Offset: 0x00228CFC
		[SecurityCritical]
		[SecurityTreatAsSafe]
		protected override PackagePart GetPartCore(Uri uri)
		{
			if (!ResourceContainer.assemblyLoadhandlerAttached && !BrowserInteropHelper.IsBrowserHosted)
			{
				AppDomain.CurrentDomain.AssemblyLoad += this.OnAssemblyLoadEventHandler;
				ResourceContainer.assemblyLoadhandlerAttached = true;
			}
			string resourceIDFromRelativePath;
			bool flag;
			ResourceManagerWrapper resourceManagerWrapper = this.GetResourceManagerWrapper(uri, out resourceIDFromRelativePath, out flag);
			if (flag)
			{
				return new ContentFilePart(this, uri);
			}
			resourceIDFromRelativePath = ResourceIDHelper.GetResourceIDFromRelativePath(resourceIDFromRelativePath);
			return new ResourcePart(this, uri, resourceIDFromRelativePath, resourceManagerWrapper);
		}

		// Token: 0x06007A55 RID: 31317 RVA: 0x0022AB5C File Offset: 0x00228D5C
		private void OnAssemblyLoadEventHandler(object sender, AssemblyLoadEventArgs args)
		{
			Assembly loadedAssembly = args.LoadedAssembly;
			if (!loadedAssembly.ReflectionOnly && !loadedAssembly.GlobalAssemblyCache)
			{
				AssemblyName assemblyName = new AssemblyName(loadedAssembly.FullName);
				string text = assemblyName.Name.ToLowerInvariant();
				string text2 = string.Empty;
				string text3 = text;
				this.UpdateCachedRMW(text3, args.LoadedAssembly);
				string text4 = assemblyName.Version.ToString();
				if (!string.IsNullOrEmpty(text4))
				{
					text3 += text4;
					this.UpdateCachedRMW(text3, args.LoadedAssembly);
				}
				byte[] publicKeyToken = assemblyName.GetPublicKeyToken();
				for (int i = 0; i < publicKeyToken.Length; i++)
				{
					text2 += publicKeyToken[i].ToString("x", NumberFormatInfo.InvariantInfo);
				}
				if (!string.IsNullOrEmpty(text2))
				{
					text3 += text2;
					this.UpdateCachedRMW(text3, args.LoadedAssembly);
					text3 = text + text2;
					this.UpdateCachedRMW(text3, args.LoadedAssembly);
				}
			}
		}

		// Token: 0x06007A56 RID: 31318 RVA: 0x0022AC55 File Offset: 0x00228E55
		private void UpdateCachedRMW(string key, Assembly assembly)
		{
			if (ResourceContainer._registeredResourceManagers.ContainsKey(key))
			{
				ResourceContainer._registeredResourceManagers[key].Assembly = assembly;
			}
		}

		// Token: 0x06007A57 RID: 31319 RVA: 0x0022AC78 File Offset: 0x00228E78
		private ResourceManagerWrapper GetResourceManagerWrapper(Uri uri, out string partName, out bool isContentFile)
		{
			ResourceManagerWrapper resourceManagerWrapper = ResourceContainer.ApplicationResourceManagerWrapper;
			isContentFile = false;
			string text;
			string text2;
			string text3;
			BaseUriHelper.GetAssemblyNameAndPart(uri, out partName, out text, out text2, out text3);
			if (!string.IsNullOrEmpty(text))
			{
				string text4 = text + text2 + text3;
				ResourceContainer._registeredResourceManagers.TryGetValue(text4.ToLowerInvariant(), out resourceManagerWrapper);
				if (resourceManagerWrapper == null)
				{
					Assembly loadedAssembly = BaseUriHelper.GetLoadedAssembly(text, text2, text3);
					if (loadedAssembly.Equals(Application.ResourceAssembly))
					{
						resourceManagerWrapper = ResourceContainer.ApplicationResourceManagerWrapper;
					}
					else
					{
						resourceManagerWrapper = new ResourceManagerWrapper(loadedAssembly);
					}
					ResourceContainer._registeredResourceManagers[text4.ToLowerInvariant()] = resourceManagerWrapper;
				}
			}
			if (resourceManagerWrapper == ResourceContainer.ApplicationResourceManagerWrapper)
			{
				if (resourceManagerWrapper == null)
				{
					throw new IOException(SR.Get("EntryAssemblyIsNull"));
				}
				if (ContentFileHelper.IsContentFile(partName))
				{
					isContentFile = true;
					resourceManagerWrapper = null;
				}
			}
			return resourceManagerWrapper;
		}

		// Token: 0x06007A58 RID: 31320 RVA: 0x0000C238 File Offset: 0x0000A438
		protected override PackagePart CreatePartCore(Uri uri, string contentType, CompressionOption compressionOption)
		{
			return null;
		}

		// Token: 0x06007A59 RID: 31321 RVA: 0x00041D30 File Offset: 0x0003FF30
		protected override void DeletePartCore(Uri uri)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06007A5A RID: 31322 RVA: 0x00041D30 File Offset: 0x0003FF30
		protected override PackagePart[] GetPartsCore()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06007A5B RID: 31323 RVA: 0x00041D30 File Offset: 0x0003FF30
		protected override void FlushCore()
		{
			throw new NotSupportedException();
		}

		// Token: 0x040039D6 RID: 14806
		internal const string XamlExt = ".xaml";

		// Token: 0x040039D7 RID: 14807
		internal const string BamlExt = ".baml";

		// Token: 0x040039D8 RID: 14808
		private static Dictionary<string, ResourceManagerWrapper> _registeredResourceManagers = new Dictionary<string, ResourceManagerWrapper>();

		// Token: 0x040039D9 RID: 14809
		private static ResourceManagerWrapper _applicationResourceManagerWrapper = null;

		// Token: 0x040039DA RID: 14810
		private static FileShare _fileShare = FileShare.Read;

		// Token: 0x040039DB RID: 14811
		private static bool assemblyLoadhandlerAttached = false;
	}
}
