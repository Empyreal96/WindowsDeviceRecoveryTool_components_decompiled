using System;
using System.IO;
using System.IO.Packaging;
using System.Security;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Navigation;

namespace MS.Internal.AppModel
{
	// Token: 0x02000771 RID: 1905
	internal class ContentFilePart : PackagePart
	{
		// Token: 0x060078D5 RID: 30933 RVA: 0x0022689F File Offset: 0x00224A9F
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal ContentFilePart(Package container, Uri uri) : base(container, uri)
		{
			Invariant.Assert(Application.ResourceAssembly != null, "If the entry assembly is null no ContentFileParts should be created");
			this._fullPath = null;
		}

		// Token: 0x060078D6 RID: 30934 RVA: 0x002268C8 File Offset: 0x00224AC8
		[SecurityCritical]
		[SecurityTreatAsSafe]
		protected override Stream GetStreamCore(FileMode mode, FileAccess access)
		{
			if (this._fullPath == null)
			{
				Uri entryAssemblyLocation = this.GetEntryAssemblyLocation();
				string relativeUri;
				string text;
				string text2;
				string text3;
				BaseUriHelper.GetAssemblyNameAndPart(base.Uri, out relativeUri, out text, out text2, out text3);
				Uri uri = new Uri(entryAssemblyLocation, relativeUri);
				this._fullPath = uri.LocalPath;
			}
			Stream stream = this.CriticalOpenFile(this._fullPath);
			if (stream == null)
			{
				throw new IOException(SR.Get("UnableToLocateResource", new object[]
				{
					base.Uri.ToString()
				}));
			}
			return stream;
		}

		// Token: 0x060078D7 RID: 30935 RVA: 0x00226945 File Offset: 0x00224B45
		protected override string GetContentTypeCore()
		{
			return MimeTypeMapper.GetMimeTypeFromUri(new Uri(base.Uri.ToString(), UriKind.RelativeOrAbsolute)).ToString();
		}

		// Token: 0x060078D8 RID: 30936 RVA: 0x00226964 File Offset: 0x00224B64
		[SecurityCritical]
		private Uri GetEntryAssemblyLocation()
		{
			Uri result = null;
			PermissionSet permissionSet = new PermissionSet(null);
			permissionSet.AddPermission(new FileIOPermission(PermissionState.Unrestricted));
			permissionSet.Assert();
			try
			{
				result = new Uri(Application.ResourceAssembly.CodeBase);
			}
			catch (Exception ex)
			{
				if (CriticalExceptions.IsCriticalException(ex))
				{
					throw;
				}
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return result;
		}

		// Token: 0x060078D9 RID: 30937 RVA: 0x002269D0 File Offset: 0x00224BD0
		[SecurityCritical]
		private Stream CriticalOpenFile(string filename)
		{
			Stream result = null;
			FileIOPermission fileIOPermission = new FileIOPermission(FileIOPermissionAccess.Read, filename);
			fileIOPermission.Assert();
			try
			{
				result = File.Open(filename, FileMode.Open, FileAccess.Read, ResourceContainer.FileShare);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return result;
		}

		// Token: 0x04003937 RID: 14647
		[SecurityCritical]
		private string _fullPath;
	}
}
