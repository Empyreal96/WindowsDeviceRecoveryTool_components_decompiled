using System;
using System.IO;
using System.IO.Packaging;
using System.Resources;
using System.Security;
using System.Windows;
using MS.Internal.Resources;

namespace MS.Internal.AppModel
{
	// Token: 0x0200079E RID: 1950
	internal class ResourcePart : PackagePart
	{
		// Token: 0x06007A5D RID: 31325 RVA: 0x0022AD48 File Offset: 0x00228F48
		[SecurityCritical]
		public ResourcePart(Package container, Uri uri, string name, ResourceManagerWrapper rmWrapper) : base(container, uri)
		{
			if (rmWrapper == null)
			{
				throw new ArgumentNullException("rmWrapper");
			}
			this._rmWrapper.Value = rmWrapper;
			this._name = name;
		}

		// Token: 0x06007A5E RID: 31326 RVA: 0x0022AD80 File Offset: 0x00228F80
		[SecurityCritical]
		[SecurityTreatAsSafe]
		protected override Stream GetStreamCore(FileMode mode, FileAccess access)
		{
			Stream stream = this.EnsureResourceLocationSet();
			if (stream == null)
			{
				stream = this._rmWrapper.Value.GetStream(this._name);
				if (stream == null)
				{
					throw new IOException(SR.Get("UnableToLocateResource", new object[]
					{
						this._name
					}));
				}
			}
			ContentType contentType = new ContentType(base.ContentType);
			if (MimeTypeMapper.BamlMime.AreTypeAndSubTypeEqual(contentType))
			{
				BamlStream bamlStream = new BamlStream(stream, this._rmWrapper.Value.Assembly);
				stream = bamlStream;
			}
			return stream;
		}

		// Token: 0x06007A5F RID: 31327 RVA: 0x0022AE05 File Offset: 0x00229005
		protected override string GetContentTypeCore()
		{
			this.EnsureResourceLocationSet();
			return MimeTypeMapper.GetMimeTypeFromUri(new Uri(this._name, UriKind.RelativeOrAbsolute)).ToString();
		}

		// Token: 0x06007A60 RID: 31328 RVA: 0x0022AE24 File Offset: 0x00229024
		private Stream EnsureResourceLocationSet()
		{
			object globalLock = this._globalLock;
			lock (globalLock)
			{
				if (this._ensureResourceIsCalled)
				{
					return null;
				}
				this._ensureResourceIsCalled = true;
				try
				{
					if (string.Compare(Path.GetExtension(this._name), ".baml", StringComparison.OrdinalIgnoreCase) == 0)
					{
						throw new IOException(SR.Get("UnableToLocateResource", new object[]
						{
							this._name
						}));
					}
					if (string.Compare(Path.GetExtension(this._name), ".xaml", StringComparison.OrdinalIgnoreCase) == 0)
					{
						string name = Path.ChangeExtension(this._name, ".baml");
						Stream stream = this._rmWrapper.Value.GetStream(name);
						if (stream != null)
						{
							this._name = name;
							return stream;
						}
					}
				}
				catch (MissingManifestResourceException)
				{
				}
			}
			return null;
		}

		// Token: 0x040039DC RID: 14812
		private SecurityCriticalDataForSet<ResourceManagerWrapper> _rmWrapper;

		// Token: 0x040039DD RID: 14813
		private bool _ensureResourceIsCalled;

		// Token: 0x040039DE RID: 14814
		private string _name;

		// Token: 0x040039DF RID: 14815
		private object _globalLock = new object();
	}
}
