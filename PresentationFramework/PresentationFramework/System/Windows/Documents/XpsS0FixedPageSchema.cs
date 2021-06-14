using System;
using System.IO;
using System.IO.Packaging;
using System.Security;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x02000348 RID: 840
	internal sealed class XpsS0FixedPageSchema : XpsS0Schema
	{
		// Token: 0x06002D0A RID: 11530 RVA: 0x000CB170 File Offset: 0x000C9370
		public XpsS0FixedPageSchema()
		{
			XpsSchema.RegisterSchema(this, new ContentType[]
			{
				XpsS0Schema._fixedDocumentSequenceContentType,
				XpsS0Schema._fixedDocumentContentType,
				XpsS0Schema._fixedPageContentType
			});
			base.RegisterRequiredResourceMimeTypes(new ContentType[]
			{
				XpsS0Schema._resourceDictionaryContentType,
				XpsS0Schema._fontContentType,
				XpsS0Schema._colorContextContentType,
				XpsS0Schema._obfuscatedContentType,
				XpsS0Schema._jpgContentType,
				XpsS0Schema._pngContentType,
				XpsS0Schema._tifContentType,
				XpsS0Schema._wmpContentType
			});
		}

		// Token: 0x06002D0B RID: 11531 RVA: 0x000CB1F4 File Offset: 0x000C93F4
		[SecurityCritical]
		[SecurityTreatAsSafe]
		public override void ValidateRelationships(SecurityCriticalData<Package> package, Uri packageUri, Uri partUri, ContentType mimeType)
		{
			PackagePart part = package.Value.GetPart(partUri);
			PackageRelationshipCollection relationshipsByType = part.GetRelationshipsByType("http://schemas.microsoft.com/xps/2005/06/printticket");
			int num = 0;
			foreach (PackageRelationship packageRelationship in relationshipsByType)
			{
				num++;
				if (num > 1)
				{
					throw new FileFormatException(SR.Get("XpsValidatingLoaderMoreThanOnePrintTicketPart"));
				}
				Uri partUri2 = PackUriHelper.ResolvePartUri(partUri, packageRelationship.TargetUri);
				Uri uri = PackUriHelper.Create(packageUri, partUri2);
				PackagePart part2 = package.Value.GetPart(partUri2);
				if (!XpsS0Schema._printTicketContentType.AreTypeAndSubTypeEqual(new ContentType(part2.ContentType)))
				{
					throw new FileFormatException(SR.Get("XpsValidatingLoaderPrintTicketHasIncorrectType"));
				}
			}
			relationshipsByType = part.GetRelationshipsByType("http://schemas.openxmlformats.org/package/2006/relationships/metadata/thumbnail");
			num = 0;
			foreach (PackageRelationship packageRelationship2 in relationshipsByType)
			{
				num++;
				if (num > 1)
				{
					throw new FileFormatException(SR.Get("XpsValidatingLoaderMoreThanOneThumbnailPart"));
				}
				Uri partUri3 = PackUriHelper.ResolvePartUri(partUri, packageRelationship2.TargetUri);
				Uri uri2 = PackUriHelper.Create(packageUri, partUri3);
				PackagePart part3 = package.Value.GetPart(partUri3);
				if (!XpsS0Schema._jpgContentType.AreTypeAndSubTypeEqual(new ContentType(part3.ContentType)) && !XpsS0Schema._pngContentType.AreTypeAndSubTypeEqual(new ContentType(part3.ContentType)))
				{
					throw new FileFormatException(SR.Get("XpsValidatingLoaderThumbnailHasIncorrectType"));
				}
			}
			if (XpsS0Schema._fixedDocumentContentType.AreTypeAndSubTypeEqual(mimeType))
			{
				relationshipsByType = part.GetRelationshipsByType("http://schemas.microsoft.com/xps/2005/06/restricted-font");
				foreach (PackageRelationship packageRelationship3 in relationshipsByType)
				{
					Uri partUri4 = PackUriHelper.ResolvePartUri(partUri, packageRelationship3.TargetUri);
					Uri uri3 = PackUriHelper.Create(packageUri, partUri4);
					PackagePart part4 = package.Value.GetPart(partUri4);
					if (!XpsS0Schema._fontContentType.AreTypeAndSubTypeEqual(new ContentType(part4.ContentType)) && !XpsS0Schema._obfuscatedContentType.AreTypeAndSubTypeEqual(new ContentType(part4.ContentType)))
					{
						throw new FileFormatException(SR.Get("XpsValidatingLoaderRestrictedFontHasIncorrectType"));
					}
				}
			}
			if (XpsS0Schema._fixedDocumentSequenceContentType.AreTypeAndSubTypeEqual(mimeType))
			{
				relationshipsByType = package.Value.GetRelationshipsByType("http://schemas.microsoft.com/xps/2005/06/discard-control");
				num = 0;
				foreach (PackageRelationship packageRelationship4 in relationshipsByType)
				{
					num++;
					if (num > 1)
					{
						throw new FileFormatException(SR.Get("XpsValidatingLoaderMoreThanOneDiscardControlInPackage"));
					}
					Uri partUri5 = PackUriHelper.ResolvePartUri(partUri, packageRelationship4.TargetUri);
					Uri uri4 = PackUriHelper.Create(packageUri, partUri5);
					PackagePart part5 = package.Value.GetPart(partUri5);
					if (!XpsS0Schema._discardControlContentType.AreTypeAndSubTypeEqual(new ContentType(part5.ContentType)))
					{
						throw new FileFormatException(SR.Get("XpsValidatingLoaderDiscardControlHasIncorrectType"));
					}
				}
				relationshipsByType = package.Value.GetRelationshipsByType("http://schemas.openxmlformats.org/package/2006/relationships/metadata/thumbnail");
				num = 0;
				foreach (PackageRelationship packageRelationship5 in relationshipsByType)
				{
					num++;
					if (num > 1)
					{
						throw new FileFormatException(SR.Get("XpsValidatingLoaderMoreThanOneThumbnailInPackage"));
					}
					Uri partUri6 = PackUriHelper.ResolvePartUri(partUri, packageRelationship5.TargetUri);
					Uri uri5 = PackUriHelper.Create(packageUri, partUri6);
					PackagePart part6 = package.Value.GetPart(partUri6);
					if (!XpsS0Schema._jpgContentType.AreTypeAndSubTypeEqual(new ContentType(part6.ContentType)) && !XpsS0Schema._pngContentType.AreTypeAndSubTypeEqual(new ContentType(part6.ContentType)))
					{
						throw new FileFormatException(SR.Get("XpsValidatingLoaderThumbnailHasIncorrectType"));
					}
				}
			}
		}

		// Token: 0x04001D60 RID: 7520
		private const string _printTicketRel = "http://schemas.microsoft.com/xps/2005/06/printticket";

		// Token: 0x04001D61 RID: 7521
		private const string _discardControlRel = "http://schemas.microsoft.com/xps/2005/06/discard-control";

		// Token: 0x04001D62 RID: 7522
		private const string _restrictedFontRel = "http://schemas.microsoft.com/xps/2005/06/restricted-font";

		// Token: 0x04001D63 RID: 7523
		private const string _thumbnailRel = "http://schemas.openxmlformats.org/package/2006/relationships/metadata/thumbnail";
	}
}
