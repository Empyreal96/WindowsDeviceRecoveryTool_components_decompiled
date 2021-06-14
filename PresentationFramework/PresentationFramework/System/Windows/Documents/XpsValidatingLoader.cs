using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Security;
using System.Windows.Markup;
using System.Xml;
using MS.Internal;
using MS.Internal.IO.Packaging;

namespace System.Windows.Documents
{
	// Token: 0x02000439 RID: 1081
	internal class XpsValidatingLoader
	{
		// Token: 0x06003F75 RID: 16245 RVA: 0x0000326D File Offset: 0x0000146D
		internal XpsValidatingLoader()
		{
		}

		// Token: 0x06003F76 RID: 16246 RVA: 0x001247A2 File Offset: 0x001229A2
		internal object Load(Stream stream, Uri parentUri, ParserContext pc, ContentType mimeType)
		{
			return this.Load(stream, parentUri, pc, mimeType, null);
		}

		// Token: 0x06003F77 RID: 16247 RVA: 0x001247B0 File Offset: 0x001229B0
		internal void Validate(Stream stream, Uri parentUri, ParserContext pc, ContentType mimeType, string rootElement)
		{
			this.Load(stream, parentUri, pc, mimeType, rootElement);
		}

		// Token: 0x06003F78 RID: 16248 RVA: 0x001247C0 File Offset: 0x001229C0
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private object Load(Stream stream, Uri parentUri, ParserContext pc, ContentType mimeType, string rootElement)
		{
			object result = null;
			List<Type> safeTypes = new List<Type>
			{
				typeof(ResourceDictionary)
			};
			if (!XpsValidatingLoader.DocumentMode)
			{
				if (rootElement == null)
				{
					XmlReader reader = XmlReader.Create(stream, null, pc);
					result = XamlReader.Load(reader, pc, XamlParseMode.Synchronous, FrameworkCompatibilityPreferences.DisableLegacyDangerousXamlDeserializationMode, safeTypes);
					stream.Close();
				}
			}
			else
			{
				XpsSchema schema = XpsSchema.GetSchema(mimeType);
				Uri baseUri = pc.BaseUri;
				Uri uri;
				Uri uri2;
				PackUriHelper.ValidateAndGetPackUriComponents(baseUri, out uri, out uri2);
				Package package = PreloadedPackages.GetPackage(uri);
				if (parentUri != null)
				{
					Uri packageUri = PackUriHelper.GetPackageUri(parentUri);
					if (!packageUri.Equals(uri))
					{
						throw new FileFormatException(SR.Get("XpsValidatingLoaderUriNotInSamePackage"));
					}
				}
				schema.ValidateRelationships(new SecurityCriticalData<Package>(package), uri, uri2, mimeType);
				if (schema.AllowsMultipleReferencesToSameUri(mimeType))
				{
					this._uniqueUriRef = null;
				}
				else
				{
					this._uniqueUriRef = new Hashtable(11);
				}
				Hashtable hashtable = (XpsValidatingLoader._validResources.Count > 0) ? XpsValidatingLoader._validResources.Peek() : null;
				if (schema.HasRequiredResources(mimeType))
				{
					hashtable = new Hashtable(11);
					PackagePart part = package.GetPart(uri2);
					PackageRelationshipCollection relationshipsByType = part.GetRelationshipsByType(XpsValidatingLoader._requiredResourceRel);
					foreach (PackageRelationship packageRelationship in relationshipsByType)
					{
						Uri partUri = PackUriHelper.ResolvePartUri(uri2, packageRelationship.TargetUri);
						Uri key = PackUriHelper.Create(uri, partUri);
						PackagePart part2 = package.GetPart(partUri);
						if (schema.IsValidRequiredResourceMimeType(part2.ValidatedContentType))
						{
							if (!hashtable.ContainsKey(key))
							{
								hashtable.Add(key, true);
							}
						}
						else if (!hashtable.ContainsKey(key))
						{
							hashtable.Add(key, false);
						}
					}
				}
				XpsSchemaValidator xpsSchemaValidator = new XpsSchemaValidator(this, schema, mimeType, stream, uri, uri2);
				XpsValidatingLoader._validResources.Push(hashtable);
				if (rootElement != null)
				{
					xpsSchemaValidator.XmlReader.MoveToContent();
					if (!rootElement.Equals(xpsSchemaValidator.XmlReader.Name))
					{
						throw new FileFormatException(SR.Get("XpsValidatingLoaderUnsupportedMimeType"));
					}
					while (xpsSchemaValidator.XmlReader.Read())
					{
					}
				}
				else
				{
					result = XamlReader.Load(xpsSchemaValidator.XmlReader, pc, XamlParseMode.Synchronous, FrameworkCompatibilityPreferences.DisableLegacyDangerousXamlDeserializationMode, safeTypes);
				}
				XpsValidatingLoader._validResources.Pop();
			}
			return result;
		}

		// Token: 0x17000FC0 RID: 4032
		// (get) Token: 0x06003F79 RID: 16249 RVA: 0x00124A20 File Offset: 0x00122C20
		internal static bool DocumentMode
		{
			get
			{
				return XpsValidatingLoader._documentMode;
			}
		}

		// Token: 0x06003F7A RID: 16250 RVA: 0x00124A27 File Offset: 0x00122C27
		internal static void AssertDocumentMode()
		{
			XpsValidatingLoader._documentMode = true;
		}

		// Token: 0x06003F7B RID: 16251 RVA: 0x00124A30 File Offset: 0x00122C30
		internal void UriHitHandler(int node, Uri uri)
		{
			if (this._uniqueUriRef != null)
			{
				if (this._uniqueUriRef.Contains(uri))
				{
					if ((int)this._uniqueUriRef[uri] != node)
					{
						throw new FileFormatException(SR.Get("XpsValidatingLoaderDuplicateReference"));
					}
				}
				else
				{
					this._uniqueUriRef.Add(uri, node);
				}
			}
			Hashtable hashtable = XpsValidatingLoader._validResources.Peek();
			if (hashtable != null)
			{
				if (!hashtable.ContainsKey(uri))
				{
					bool flag = false;
					foreach (object obj in hashtable.Keys)
					{
						Uri uri2 = (Uri)obj;
						if (PackUriHelper.ComparePackUri(uri2, uri) == 0)
						{
							hashtable.Add(uri, hashtable[uri2]);
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						throw new FileFormatException(SR.Get("XpsValidatingLoaderUnlistedResource"));
					}
				}
				if (!(bool)hashtable[uri])
				{
					throw new FileFormatException(SR.Get("XpsValidatingLoaderUnsupportedMimeType"));
				}
			}
		}

		// Token: 0x04002731 RID: 10033
		private static Stack<Hashtable> _validResources = new Stack<Hashtable>();

		// Token: 0x04002732 RID: 10034
		private Hashtable _uniqueUriRef;

		// Token: 0x04002733 RID: 10035
		private static bool _documentMode = false;

		// Token: 0x04002734 RID: 10036
		private static string _requiredResourceRel = "http://schemas.microsoft.com/xps/2005/06/required-resource";

		// Token: 0x04002735 RID: 10037
		private static XpsS0FixedPageSchema xpsS0FixedPageSchema = new XpsS0FixedPageSchema();

		// Token: 0x04002736 RID: 10038
		private static XpsS0ResourceDictionarySchema xpsS0ResourceDictionarySchema = new XpsS0ResourceDictionarySchema();

		// Token: 0x04002737 RID: 10039
		private static XpsDocStructSchema xpsDocStructSchema = new XpsDocStructSchema();
	}
}
