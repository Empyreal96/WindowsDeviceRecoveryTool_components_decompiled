using System;

namespace System.Data.Services.Client
{
	// Token: 0x02000126 RID: 294
	internal class UriResolver
	{
		// Token: 0x060009BF RID: 2495 RVA: 0x00027D84 File Offset: 0x00025F84
		private UriResolver(Uri baseUri, Func<string, Uri> resolveEntitySet)
		{
			this.baseUri = baseUri;
			this.resolveEntitySet = resolveEntitySet;
			if (this.baseUri != null)
			{
				this.baseUriWithSlash = UriResolver.ForceSlashTerminatedUri(this.baseUri);
			}
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x060009C0 RID: 2496 RVA: 0x00027DB9 File Offset: 0x00025FB9
		internal Func<string, Uri> ResolveEntitySet
		{
			get
			{
				return this.resolveEntitySet;
			}
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x060009C1 RID: 2497 RVA: 0x00027DC1 File Offset: 0x00025FC1
		internal Uri RawBaseUriValue
		{
			get
			{
				return this.baseUri;
			}
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x060009C2 RID: 2498 RVA: 0x00027DC9 File Offset: 0x00025FC9
		internal Uri BaseUriOrNull
		{
			get
			{
				return this.baseUriWithSlash;
			}
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x00027DD1 File Offset: 0x00025FD1
		internal static UriResolver CreateFromBaseUri(Uri baseUri, string parameterName)
		{
			UriResolver.ConvertToAbsoluteAndValidateBaseUri(ref baseUri, parameterName);
			return new UriResolver(baseUri, null);
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x00027DE2 File Offset: 0x00025FE2
		internal UriResolver CloneWithOverrideValue(Uri overrideBaseUriValue, string parameterName)
		{
			UriResolver.ConvertToAbsoluteAndValidateBaseUri(ref overrideBaseUriValue, parameterName);
			return new UriResolver(overrideBaseUriValue, this.resolveEntitySet);
		}

		// Token: 0x060009C5 RID: 2501 RVA: 0x00027DF8 File Offset: 0x00025FF8
		internal UriResolver CloneWithOverrideValue(Func<string, Uri> overrideResolveEntitySetValue)
		{
			return new UriResolver(this.baseUri, overrideResolveEntitySetValue);
		}

		// Token: 0x060009C6 RID: 2502 RVA: 0x00027E08 File Offset: 0x00026008
		internal Uri GetEntitySetUri(string entitySetName)
		{
			Uri entitySetUriFromResolver = this.GetEntitySetUriFromResolver(entitySetName);
			if (entitySetUriFromResolver != null)
			{
				return UriResolver.ForceNonSlashTerminatedUri(entitySetUriFromResolver);
			}
			if (this.baseUriWithSlash != null)
			{
				return UriUtil.CreateUri(this.baseUriWithSlash, UriUtil.CreateUri(entitySetName, UriKind.Relative));
			}
			throw Error.InvalidOperation(Strings.Context_ResolveEntitySetOrBaseUriRequired(entitySetName));
		}

		// Token: 0x060009C7 RID: 2503 RVA: 0x00027E60 File Offset: 0x00026060
		internal Uri GetBaseUriWithSlash()
		{
			return this.GetBaseUriWithSlash(() => Strings.Context_BaseUriRequired);
		}

		// Token: 0x060009C8 RID: 2504 RVA: 0x00027E8C File Offset: 0x0002608C
		internal Uri GetOrCreateAbsoluteUri(Uri requestUri)
		{
			Util.CheckArgumentNull<Uri>(requestUri, "requestUri");
			if (!requestUri.IsAbsoluteUri)
			{
				return UriUtil.CreateUri(this.GetBaseUriWithSlash(() => Strings.Context_RequestUriIsRelativeBaseUriRequired), requestUri);
			}
			return requestUri;
		}

		// Token: 0x060009C9 RID: 2505 RVA: 0x00027ED8 File Offset: 0x000260D8
		private static void ConvertToAbsoluteAndValidateBaseUri(ref Uri baseUri, string parameterName)
		{
			baseUri = UriResolver.ConvertToAbsoluteUri(baseUri);
			if (UriResolver.IsValidBaseUri(baseUri))
			{
				return;
			}
			if (parameterName != null)
			{
				throw Error.Argument(Strings.Context_BaseUri, parameterName);
			}
			throw Error.InvalidOperation(Strings.Context_BaseUri);
		}

		// Token: 0x060009CA RID: 2506 RVA: 0x00027F08 File Offset: 0x00026108
		private static bool IsValidBaseUri(Uri baseUri)
		{
			return baseUri == null || (baseUri.IsAbsoluteUri && Uri.IsWellFormedUriString(UriUtil.UriToString(baseUri), UriKind.Absolute) && string.IsNullOrEmpty(baseUri.Query) && string.IsNullOrEmpty(baseUri.Fragment) && (!(baseUri.Scheme != "http") || !(baseUri.Scheme != "https")));
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x00027F77 File Offset: 0x00026177
		private static Uri ConvertToAbsoluteUri(Uri baseUri)
		{
			if (baseUri == null)
			{
				return null;
			}
			return baseUri;
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x00027F88 File Offset: 0x00026188
		private static Uri ForceNonSlashTerminatedUri(Uri uri)
		{
			string text = UriUtil.UriToString(uri);
			if (text[text.Length - 1] == '/')
			{
				return UriUtil.CreateUri(text.Substring(0, text.Length - 1), UriKind.Absolute);
			}
			return uri;
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x00027FC8 File Offset: 0x000261C8
		private static Uri ForceSlashTerminatedUri(Uri uri)
		{
			string text = UriUtil.UriToString(uri);
			if (text[text.Length - 1] != '/')
			{
				return UriUtil.CreateUri(text + "/", UriKind.Absolute);
			}
			return uri;
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x00028001 File Offset: 0x00026201
		private Uri GetBaseUriWithSlash(Func<string> getErrorMessage)
		{
			if (this.baseUriWithSlash == null)
			{
				throw Error.InvalidOperation(getErrorMessage());
			}
			return this.baseUriWithSlash;
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x00028024 File Offset: 0x00026224
		private Uri GetEntitySetUriFromResolver(string entitySetName)
		{
			if (this.resolveEntitySet != null)
			{
				Uri uri = this.resolveEntitySet(entitySetName);
				if (uri != null)
				{
					if (UriResolver.IsValidBaseUri(uri))
					{
						return uri;
					}
					throw Error.InvalidOperation(Strings.Context_ResolveReturnedInvalidUri);
				}
			}
			return null;
		}

		// Token: 0x040005A1 RID: 1441
		private readonly Uri baseUri;

		// Token: 0x040005A2 RID: 1442
		private readonly Func<string, Uri> resolveEntitySet;

		// Token: 0x040005A3 RID: 1443
		private readonly Uri baseUriWithSlash;
	}
}
