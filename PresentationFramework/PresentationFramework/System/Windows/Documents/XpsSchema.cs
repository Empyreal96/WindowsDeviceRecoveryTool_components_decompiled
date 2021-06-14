using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Xml;
using System.Xml.Schema;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x02000346 RID: 838
	internal class XpsSchema
	{
		// Token: 0x06002CF1 RID: 11505 RVA: 0x000CACF0 File Offset: 0x000C8EF0
		protected XpsSchema()
		{
		}

		// Token: 0x06002CF2 RID: 11506 RVA: 0x000CAD08 File Offset: 0x000C8F08
		protected static void RegisterSchema(XpsSchema schema, ContentType[] handledMimeTypes)
		{
			foreach (ContentType key in handledMimeTypes)
			{
				XpsSchema._schemas.Add(key, schema);
			}
		}

		// Token: 0x06002CF3 RID: 11507 RVA: 0x000CAD38 File Offset: 0x000C8F38
		protected void RegisterRequiredResourceMimeTypes(ContentType[] requiredResourceMimeTypes)
		{
			if (requiredResourceMimeTypes != null)
			{
				foreach (ContentType key in requiredResourceMimeTypes)
				{
					this._requiredResourceMimeTypes.Add(key, true);
				}
			}
		}

		// Token: 0x06002CF4 RID: 11508 RVA: 0x000CAD70 File Offset: 0x000C8F70
		public virtual XmlReaderSettings GetXmlReaderSettings()
		{
			return new XmlReaderSettings
			{
				ValidationFlags = (XmlSchemaValidationFlags.ReportValidationWarnings | XmlSchemaValidationFlags.ProcessIdentityConstraints)
			};
		}

		// Token: 0x06002CF5 RID: 11509 RVA: 0x00002137 File Offset: 0x00000337
		public virtual void ValidateRelationships(SecurityCriticalData<Package> package, Uri packageUri, Uri partUri, ContentType mimeType)
		{
		}

		// Token: 0x06002CF6 RID: 11510 RVA: 0x0000B02A File Offset: 0x0000922A
		public virtual bool HasRequiredResources(ContentType mimeType)
		{
			return false;
		}

		// Token: 0x06002CF7 RID: 11511 RVA: 0x0000B02A File Offset: 0x0000922A
		public virtual bool HasUriAttributes(ContentType mimeType)
		{
			return false;
		}

		// Token: 0x06002CF8 RID: 11512 RVA: 0x00016748 File Offset: 0x00014948
		public virtual bool AllowsMultipleReferencesToSameUri(ContentType mimeType)
		{
			return true;
		}

		// Token: 0x06002CF9 RID: 11513 RVA: 0x0000B02A File Offset: 0x0000922A
		public virtual bool IsValidRootNamespaceUri(string namespaceUri)
		{
			return false;
		}

		// Token: 0x17000B2F RID: 2863
		// (get) Token: 0x06002CFA RID: 11514 RVA: 0x000CAD8C File Offset: 0x000C8F8C
		public virtual string RootNamespaceUri
		{
			get
			{
				return "";
			}
		}

		// Token: 0x06002CFB RID: 11515 RVA: 0x000CAD94 File Offset: 0x000C8F94
		public bool IsValidRequiredResourceMimeType(ContentType mimeType)
		{
			foreach (object obj in this._requiredResourceMimeTypes.Keys)
			{
				ContentType contentType = (ContentType)obj;
				if (contentType.AreTypeAndSubTypeEqual(mimeType))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002CFC RID: 11516 RVA: 0x0000C238 File Offset: 0x0000A438
		public virtual string[] ExtractUriFromAttr(string attrName, string attrValue)
		{
			return null;
		}

		// Token: 0x06002CFD RID: 11517 RVA: 0x000CADFC File Offset: 0x000C8FFC
		public static XpsSchema GetSchema(ContentType mimeType)
		{
			XpsSchema result = null;
			if (!XpsSchema._schemas.TryGetValue(mimeType, out result))
			{
				throw new FileFormatException(SR.Get("XpsValidatingLoaderUnsupportedMimeType"));
			}
			return result;
		}

		// Token: 0x04001D4D RID: 7501
		private static readonly Dictionary<ContentType, XpsSchema> _schemas = new Dictionary<ContentType, XpsSchema>(new ContentType.StrongComparer());

		// Token: 0x04001D4E RID: 7502
		private Hashtable _requiredResourceMimeTypes = new Hashtable(11);
	}
}
