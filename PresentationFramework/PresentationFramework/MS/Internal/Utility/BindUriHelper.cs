using System;
using System.Security;
using System.Text;
using System.Windows;
using System.Windows.Navigation;
using MS.Internal.AppModel;

namespace MS.Internal.Utility
{
	// Token: 0x020007EC RID: 2028
	internal static class BindUriHelper
	{
		// Token: 0x06007D2B RID: 32043 RVA: 0x00232FE9 File Offset: 0x002311E9
		internal static string UriToString(Uri uri)
		{
			if (uri == null)
			{
				throw new ArgumentNullException("uri");
			}
			return new StringBuilder(uri.GetComponents(uri.IsAbsoluteUri ? UriComponents.AbsoluteUri : UriComponents.SerializationInfoString, UriFormat.SafeUnescaped), 2083).ToString();
		}

		// Token: 0x17001D17 RID: 7447
		// (get) Token: 0x06007D2C RID: 32044 RVA: 0x00233026 File Offset: 0x00231226
		// (set) Token: 0x06007D2D RID: 32045 RVA: 0x0023302D File Offset: 0x0023122D
		internal static Uri BaseUri
		{
			get
			{
				return BaseUriHelper.BaseUri;
			}
			[SecurityCritical]
			set
			{
				BaseUriHelper.BaseUri = BaseUriHelper.FixFileUri(value);
			}
		}

		// Token: 0x06007D2E RID: 32046 RVA: 0x0023303A File Offset: 0x0023123A
		internal static bool DoSchemeAndHostMatch(Uri first, Uri second)
		{
			return SecurityHelper.AreStringTypesEqual(first.Scheme, second.Scheme) && first.Host.Equals(second.Host);
		}

		// Token: 0x06007D2F RID: 32047 RVA: 0x00233064 File Offset: 0x00231264
		internal static Uri GetResolvedUri(Uri baseUri, Uri orgUri)
		{
			Uri result;
			if (orgUri == null)
			{
				result = null;
			}
			else if (!orgUri.IsAbsoluteUri)
			{
				Uri baseUri2 = (baseUri == null) ? BindUriHelper.BaseUri : baseUri;
				result = new Uri(baseUri2, orgUri);
			}
			else
			{
				result = BaseUriHelper.FixFileUri(orgUri);
			}
			return result;
		}

		// Token: 0x06007D30 RID: 32048 RVA: 0x002330AC File Offset: 0x002312AC
		internal static string GetReferer(Uri destinationUri)
		{
			string result = null;
			Uri browserSource = SiteOfOriginContainer.BrowserSource;
			if (browserSource != null)
			{
				SecurityZone securityZone = CustomCredentialPolicy.MapUrlToZone(browserSource);
				SecurityZone securityZone2 = CustomCredentialPolicy.MapUrlToZone(destinationUri);
				if (securityZone == securityZone2 && SecurityHelper.AreStringTypesEqual(browserSource.Scheme, destinationUri.Scheme))
				{
					result = browserSource.GetComponents(UriComponents.AbsoluteUri, UriFormat.UriEscaped);
				}
			}
			return result;
		}

		// Token: 0x06007D31 RID: 32049 RVA: 0x002330FA File Offset: 0x002312FA
		internal static Uri GetResolvedUri(Uri originalUri)
		{
			return BindUriHelper.GetResolvedUri(null, originalUri);
		}

		// Token: 0x06007D32 RID: 32050 RVA: 0x00233104 File Offset: 0x00231304
		internal static Uri GetUriToNavigate(DependencyObject element, Uri baseUri, Uri inputUri)
		{
			if (inputUri == null || inputUri.IsAbsoluteUri)
			{
				return inputUri;
			}
			if (BindUriHelper.StartWithFragment(inputUri))
			{
				baseUri = null;
			}
			Uri resolvedUri;
			if (baseUri != null)
			{
				if (!baseUri.IsAbsoluteUri)
				{
					resolvedUri = BindUriHelper.GetResolvedUri(BindUriHelper.GetResolvedUri(null, baseUri), inputUri);
				}
				else
				{
					resolvedUri = BindUriHelper.GetResolvedUri(baseUri, inputUri);
				}
			}
			else
			{
				Uri uri = null;
				if (element != null)
				{
					INavigator navigator = element as INavigator;
					if (navigator != null)
					{
						uri = navigator.CurrentSource;
					}
					else
					{
						NavigationService navigationService = element.GetValue(NavigationService.NavigationServiceProperty) as NavigationService;
						uri = ((navigationService == null) ? null : navigationService.CurrentSource);
					}
				}
				if (uri != null)
				{
					if (uri.IsAbsoluteUri)
					{
						resolvedUri = BindUriHelper.GetResolvedUri(uri, inputUri);
					}
					else
					{
						resolvedUri = BindUriHelper.GetResolvedUri(BindUriHelper.GetResolvedUri(null, uri), inputUri);
					}
				}
				else
				{
					resolvedUri = BindUriHelper.GetResolvedUri(null, inputUri);
				}
			}
			return resolvedUri;
		}

		// Token: 0x06007D33 RID: 32051 RVA: 0x002331C8 File Offset: 0x002313C8
		internal static bool StartWithFragment(Uri uri)
		{
			return uri.OriginalString.StartsWith("#", StringComparison.Ordinal);
		}

		// Token: 0x06007D34 RID: 32052 RVA: 0x002331DC File Offset: 0x002313DC
		internal static string GetFragment(Uri uri)
		{
			Uri uri2 = uri;
			string result = string.Empty;
			if (!uri.IsAbsoluteUri)
			{
				uri2 = new Uri(BindUriHelper.placeboBase, uri);
			}
			string fragment = uri2.Fragment;
			if (fragment != null && fragment.Length > 0)
			{
				result = fragment.Substring(1);
			}
			return result;
		}

		// Token: 0x06007D35 RID: 32053 RVA: 0x00233224 File Offset: 0x00231424
		internal static Uri GetUriRelativeToPackAppBase(Uri original)
		{
			if (original == null)
			{
				return null;
			}
			Uri resolvedUri = BindUriHelper.GetResolvedUri(original);
			Uri packAppBaseUri = BaseUriHelper.PackAppBaseUri;
			return packAppBaseUri.MakeRelativeUri(resolvedUri);
		}

		// Token: 0x06007D36 RID: 32054 RVA: 0x00233252 File Offset: 0x00231452
		internal static bool IsXamlMimeType(ContentType mimeType)
		{
			return MimeTypeMapper.XamlMime.AreTypeAndSubTypeEqual(mimeType) || MimeTypeMapper.FixedDocumentSequenceMime.AreTypeAndSubTypeEqual(mimeType) || MimeTypeMapper.FixedDocumentMime.AreTypeAndSubTypeEqual(mimeType) || MimeTypeMapper.FixedPageMime.AreTypeAndSubTypeEqual(mimeType);
		}

		// Token: 0x04003AE4 RID: 15076
		private const int MAX_PATH_LENGTH = 2048;

		// Token: 0x04003AE5 RID: 15077
		private const int MAX_SCHEME_LENGTH = 32;

		// Token: 0x04003AE6 RID: 15078
		public const int MAX_URL_LENGTH = 2083;

		// Token: 0x04003AE7 RID: 15079
		private const string PLACEBOURI = "http://microsoft.com/";

		// Token: 0x04003AE8 RID: 15080
		private static Uri placeboBase = new Uri("http://microsoft.com/");

		// Token: 0x04003AE9 RID: 15081
		private const string FRAGMENTMARKER = "#";
	}
}
