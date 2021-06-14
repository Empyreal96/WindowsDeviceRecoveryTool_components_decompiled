using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace Microsoft.WindowsAzure.Storage.Shared.Protocol
{
	// Token: 0x020000C5 RID: 197
	internal abstract class AccessPolicyResponseBase<T> : ResponseParsingBase<KeyValuePair<string, T>> where T : new()
	{
		// Token: 0x0600111B RID: 4379 RVA: 0x0003F9D5 File Offset: 0x0003DBD5
		protected AccessPolicyResponseBase(Stream stream) : base(stream)
		{
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x0600111C RID: 4380 RVA: 0x0003F9DE File Offset: 0x0003DBDE
		public IEnumerable<KeyValuePair<string, T>> AccessIdentifiers
		{
			get
			{
				return base.ObjectsToParse;
			}
		}

		// Token: 0x0600111D RID: 4381
		protected abstract T ParseElement(XElement accessPolicyElement);

		// Token: 0x0600111E RID: 4382 RVA: 0x0003FC4C File Offset: 0x0003DE4C
		protected override IEnumerable<KeyValuePair<string, T>> ParseXml()
		{
			XElement root = XElement.Load(this.reader);
			IEnumerable<XElement> elements = root.Elements("SignedIdentifier");
			foreach (XElement signedIdentifierElement in elements)
			{
				string id = (string)signedIdentifierElement.Element("Id");
				XElement accessPolicyElement = signedIdentifierElement.Element("AccessPolicy");
				T accessPolicy;
				if (accessPolicyElement != null)
				{
					accessPolicy = this.ParseElement(accessPolicyElement);
				}
				else
				{
					accessPolicy = ((default(T) == null) ? Activator.CreateInstance<T>() : default(T));
				}
				yield return new KeyValuePair<string, T>(id, accessPolicy);
			}
			yield break;
		}
	}
}
