using System;
using System.Diagnostics;
using System.Xml;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x020000EB RID: 235
	internal abstract class ODataAtomDeserializer : ODataDeserializer
	{
		// Token: 0x060005D7 RID: 1495 RVA: 0x0001449C File Offset: 0x0001269C
		protected ODataAtomDeserializer(ODataAtomInputContext atomInputContext) : base(atomInputContext)
		{
			this.atomInputContext = atomInputContext;
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x060005D8 RID: 1496 RVA: 0x000144AC File Offset: 0x000126AC
		internal BufferingXmlReader XmlReader
		{
			get
			{
				return this.atomInputContext.XmlReader;
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x060005D9 RID: 1497 RVA: 0x000144B9 File Offset: 0x000126B9
		protected ODataAtomInputContext AtomInputContext
		{
			get
			{
				return this.atomInputContext;
			}
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x000144C1 File Offset: 0x000126C1
		internal void ReadPayloadStart()
		{
			this.XmlReader.ReadPayloadStart();
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x000144CE File Offset: 0x000126CE
		internal void ReadPayloadEnd()
		{
			this.XmlReader.ReadPayloadEnd();
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x000144DB File Offset: 0x000126DB
		internal Uri ProcessUriFromPayload(string uriFromPayload, Uri xmlBaseUri)
		{
			return this.ProcessUriFromPayload(uriFromPayload, xmlBaseUri, true);
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x000144E8 File Offset: 0x000126E8
		internal Uri ProcessUriFromPayload(string uriFromPayload, Uri xmlBaseUri, bool makeAbsolute)
		{
			Uri uri = xmlBaseUri;
			if (!(uri != null))
			{
				uri = base.MessageReaderSettings.BaseUri;
				uri != null;
			}
			Uri uri2 = new Uri(uriFromPayload, UriKind.RelativeOrAbsolute);
			Uri uri3 = this.AtomInputContext.ResolveUri(uri, uri2);
			if (uri3 != null)
			{
				return uri3;
			}
			if (!uri2.IsAbsoluteUri && makeAbsolute)
			{
				if (!(uri != null))
				{
					throw new ODataException(Strings.ODataAtomDeserializer_RelativeUriUsedWithoutBaseUriSpecified(uriFromPayload));
				}
				uri2 = UriUtils.UriToAbsoluteUri(uri, uri2);
			}
			return uri2;
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x00014560 File Offset: 0x00012760
		[Conditional("DEBUG")]
		internal void AssertXmlCondition(params XmlNodeType[] allowedNodeTypes)
		{
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x00014562 File Offset: 0x00012762
		[Conditional("DEBUG")]
		internal void AssertXmlCondition(bool allowEmptyElement, params XmlNodeType[] allowedNodeTypes)
		{
		}

		// Token: 0x04000269 RID: 617
		private readonly ODataAtomInputContext atomInputContext;
	}
}
