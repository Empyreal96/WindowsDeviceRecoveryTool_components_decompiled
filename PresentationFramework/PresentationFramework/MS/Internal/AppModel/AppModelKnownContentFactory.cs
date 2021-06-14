using System;
using System.ComponentModel;
using System.IO;
using System.IO.Packaging;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Navigation;
using MS.Internal.Resources;

namespace MS.Internal.AppModel
{
	// Token: 0x020007A1 RID: 1953
	internal static class AppModelKnownContentFactory
	{
		// Token: 0x06007A68 RID: 31336 RVA: 0x0022AF64 File Offset: 0x00229164
		internal static object BamlConverter(Stream stream, Uri baseUri, bool canUseTopLevelBrowser, bool sandboxExternalContent, bool allowAsync, bool isJournalNavigation, out XamlReader asyncObjectConverter)
		{
			asyncObjectConverter = null;
			if (!BaseUriHelper.IsPackApplicationUri(baseUri))
			{
				throw new InvalidOperationException(SR.Get("BamlIsNotSupportedOutsideOfApplicationResources"));
			}
			Uri partUri = PackUriHelper.GetPartUri(baseUri);
			string partName;
			string text;
			string text2;
			string text3;
			BaseUriHelper.GetAssemblyNameAndPart(partUri, out partName, out text, out text2, out text3);
			if (ContentFileHelper.IsContentFile(partName))
			{
				throw new InvalidOperationException(SR.Get("BamlIsNotSupportedOutsideOfApplicationResources"));
			}
			return Application.LoadBamlStreamWithSyncInfo(stream, new ParserContext
			{
				BaseUri = baseUri,
				SkipJournaledProperties = isJournalNavigation
			});
		}

		// Token: 0x06007A69 RID: 31337 RVA: 0x0022AFDC File Offset: 0x002291DC
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal static object XamlConverter(Stream stream, Uri baseUri, bool canUseTopLevelBrowser, bool sandboxExternalContent, bool allowAsync, bool isJournalNavigation, out XamlReader asyncObjectConverter)
		{
			asyncObjectConverter = null;
			if (sandboxExternalContent)
			{
				if (SecurityHelper.AreStringTypesEqual(baseUri.Scheme, BaseUriHelper.PackAppBaseUri.Scheme))
				{
					baseUri = BaseUriHelper.ConvertPackUriToAbsoluteExternallyVisibleUri(baseUri);
				}
				stream.Close();
				return new WebBrowser
				{
					Source = baseUri
				};
			}
			ParserContext parserContext = new ParserContext();
			parserContext.BaseUri = baseUri;
			parserContext.SkipJournaledProperties = isJournalNavigation;
			if (allowAsync)
			{
				XamlReader xamlReader = new XamlReader();
				asyncObjectConverter = xamlReader;
				xamlReader.LoadCompleted += AppModelKnownContentFactory.OnParserComplete;
				return xamlReader.LoadAsync(stream, parserContext);
			}
			return XamlReader.Load(stream, parserContext);
		}

		// Token: 0x06007A6A RID: 31338 RVA: 0x0022B069 File Offset: 0x00229269
		private static void OnParserComplete(object sender, AsyncCompletedEventArgs args)
		{
			if (!args.Cancelled && args.Error != null)
			{
				throw args.Error;
			}
		}

		// Token: 0x06007A6B RID: 31339 RVA: 0x0022B084 File Offset: 0x00229284
		internal static object HtmlXappConverter(Stream stream, Uri baseUri, bool canUseTopLevelBrowser, bool sandboxExternalContent, bool allowAsync, bool isJournalNavigation, out XamlReader asyncObjectConverter)
		{
			asyncObjectConverter = null;
			if (canUseTopLevelBrowser)
			{
				return null;
			}
			if (SecurityHelper.AreStringTypesEqual(baseUri.Scheme, BaseUriHelper.PackAppBaseUri.Scheme))
			{
				baseUri = BaseUriHelper.ConvertPackUriToAbsoluteExternallyVisibleUri(baseUri);
			}
			stream.Close();
			return new WebBrowser
			{
				Source = baseUri
			};
		}
	}
}
