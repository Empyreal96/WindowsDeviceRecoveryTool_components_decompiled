using System;
using System.Xml;
using System.Xml.XPath;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x020000E8 RID: 232
	internal sealed class DefaultNamespaceCompensatingXmlWriter : XmlWriter
	{
		// Token: 0x060005AA RID: 1450 RVA: 0x000140B5 File Offset: 0x000122B5
		internal DefaultNamespaceCompensatingXmlWriter(XmlWriter writer)
		{
			this.writer = writer;
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x060005AB RID: 1451 RVA: 0x000140C4 File Offset: 0x000122C4
		public override string XmlLang
		{
			get
			{
				return this.writer.XmlLang;
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x060005AC RID: 1452 RVA: 0x000140D1 File Offset: 0x000122D1
		public override WriteState WriteState
		{
			get
			{
				return this.writer.WriteState;
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x060005AD RID: 1453 RVA: 0x000140DE File Offset: 0x000122DE
		public override XmlSpace XmlSpace
		{
			get
			{
				return this.writer.XmlSpace;
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x060005AE RID: 1454 RVA: 0x000140EB File Offset: 0x000122EB
		public override XmlWriterSettings Settings
		{
			get
			{
				return this.writer.Settings;
			}
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x000140F8 File Offset: 0x000122F8
		public override void WriteNode(XPathNavigator navigator, bool defattr)
		{
			this.writer.WriteNode(navigator, defattr);
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x00014107 File Offset: 0x00012307
		public override void WriteNode(XmlReader reader, bool defattr)
		{
			this.writer.WriteNode(reader, defattr);
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x00014116 File Offset: 0x00012316
		public override void WriteAttributes(XmlReader reader, bool defattr)
		{
			this.writer.WriteAttributes(reader, defattr);
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x00014125 File Offset: 0x00012325
		public override string LookupPrefix(string ns)
		{
			return this.writer.LookupPrefix(ns);
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x00014133 File Offset: 0x00012333
		public override void Flush()
		{
			this.writer.Flush();
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x00014140 File Offset: 0x00012340
		public override void WriteNmToken(string name)
		{
			this.writer.WriteNmToken(name);
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x0001414E File Offset: 0x0001234E
		public override void Close()
		{
			this.writer.Close();
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x0001415B File Offset: 0x0001235B
		public override void WriteBinHex(byte[] buffer, int index, int count)
		{
			this.writer.WriteBinHex(buffer, index, count);
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x0001416B File Offset: 0x0001236B
		public override void WriteRaw(string data)
		{
			this.writer.WriteRaw(data);
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x00014179 File Offset: 0x00012379
		public override void WriteBase64(byte[] buffer, int index, int count)
		{
			this.writer.WriteBase64(buffer, index, count);
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x00014189 File Offset: 0x00012389
		public override void WriteRaw(char[] buffer, int index, int count)
		{
			this.writer.WriteRaw(buffer, index, count);
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x00014199 File Offset: 0x00012399
		public override void WriteChars(char[] buffer, int index, int count)
		{
			this.writer.WriteChars(buffer, index, count);
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x000141A9 File Offset: 0x000123A9
		public override void WriteSurrogateCharEntity(char lowChar, char highChar)
		{
			this.writer.WriteSurrogateCharEntity(lowChar, highChar);
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x000141B8 File Offset: 0x000123B8
		public override void WriteString(string text)
		{
			this.writer.WriteString(text);
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x000141C6 File Offset: 0x000123C6
		public override void WriteStartAttribute(string prefix, string localName, string ns)
		{
			this.writer.WriteStartAttribute(prefix, localName, ns);
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x000141D6 File Offset: 0x000123D6
		public override void WriteEndAttribute()
		{
			this.writer.WriteEndAttribute();
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x000141E3 File Offset: 0x000123E3
		public override void WriteCData(string text)
		{
			this.writer.WriteCData(text);
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x000141F1 File Offset: 0x000123F1
		public override void WriteComment(string text)
		{
			this.writer.WriteComment(text);
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x000141FF File Offset: 0x000123FF
		public override void WriteProcessingInstruction(string name, string text)
		{
			this.writer.WriteProcessingInstruction(name, text);
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x0001420E File Offset: 0x0001240E
		public override void WriteEntityRef(string name)
		{
			this.writer.WriteEntityRef(name);
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x0001421C File Offset: 0x0001241C
		public override void WriteCharEntity(char ch)
		{
			this.writer.WriteCharEntity(ch);
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x0001422A File Offset: 0x0001242A
		public override void WriteWhitespace(string ws)
		{
			this.writer.WriteWhitespace(ws);
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x00014238 File Offset: 0x00012438
		public override void WriteStartDocument()
		{
			this.writer.WriteStartDocument();
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x00014245 File Offset: 0x00012445
		public override void WriteStartDocument(bool standalone)
		{
			this.writer.WriteStartDocument(standalone);
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x00014253 File Offset: 0x00012453
		public override void WriteEndDocument()
		{
			this.writer.WriteEndDocument();
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x00014260 File Offset: 0x00012460
		public override void WriteDocType(string name, string pubid, string sysid, string subset)
		{
			this.writer.WriteDocType(name, pubid, sysid, subset);
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x00014272 File Offset: 0x00012472
		public override void WriteStartElement(string prefix, string localName, string ns)
		{
			if (this.rootPrefix == null)
			{
				this.rootPrefix = prefix;
				prefix = string.Empty;
			}
			else if (this.rootPrefix == prefix)
			{
				prefix = string.Empty;
			}
			this.writer.WriteStartElement(prefix, localName, ns);
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x000142AF File Offset: 0x000124AF
		public override void WriteEndElement()
		{
			this.writer.WriteEndElement();
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x000142BC File Offset: 0x000124BC
		public override void WriteFullEndElement()
		{
			this.writer.WriteFullEndElement();
		}

		// Token: 0x04000262 RID: 610
		private readonly XmlWriter writer;

		// Token: 0x04000263 RID: 611
		private string rootPrefix;
	}
}
