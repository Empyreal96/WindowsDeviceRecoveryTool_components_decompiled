using System;
using System.Xml;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x02000266 RID: 614
	internal sealed class EpmCustomWriter : EpmWriter
	{
		// Token: 0x06001444 RID: 5188 RVA: 0x0004B49F File Offset: 0x0004969F
		private EpmCustomWriter(ODataAtomOutputContext atomOutputContext) : base(atomOutputContext)
		{
		}

		// Token: 0x06001445 RID: 5189 RVA: 0x0004B4A8 File Offset: 0x000496A8
		internal static void WriteEntryEpm(XmlWriter writer, EpmTargetTree epmTargetTree, EntryPropertiesValueCache epmValueCache, IEdmEntityTypeReference entityType, ODataAtomOutputContext atomOutputContext)
		{
			EpmCustomWriter epmCustomWriter = new EpmCustomWriter(atomOutputContext);
			epmCustomWriter.WriteEntryEpm(writer, epmTargetTree, epmValueCache, entityType);
		}

		// Token: 0x06001446 RID: 5190 RVA: 0x0004B4C7 File Offset: 0x000496C7
		private static void WriteNamespaceDeclaration(XmlWriter writer, EpmTargetPathSegment targetSegment, ref string alreadyDeclaredPrefix)
		{
			if (alreadyDeclaredPrefix == null)
			{
				writer.WriteAttributeString("xmlns", targetSegment.SegmentNamespacePrefix, "http://www.w3.org/2000/xmlns/", targetSegment.SegmentNamespaceUri);
				alreadyDeclaredPrefix = targetSegment.SegmentNamespacePrefix;
			}
		}

		// Token: 0x06001447 RID: 5191 RVA: 0x0004B4F4 File Offset: 0x000496F4
		private void WriteEntryEpm(XmlWriter writer, EpmTargetTree epmTargetTree, EntryPropertiesValueCache epmValueCache, IEdmEntityTypeReference entityType)
		{
			EpmTargetPathSegment nonSyndicationRoot = epmTargetTree.NonSyndicationRoot;
			if (nonSyndicationRoot.SubSegments.Count == 0)
			{
				return;
			}
			foreach (EpmTargetPathSegment targetSegment in nonSyndicationRoot.SubSegments)
			{
				string text = null;
				this.WriteElementEpm(writer, targetSegment, epmValueCache, entityType, ref text);
			}
		}

		// Token: 0x06001448 RID: 5192 RVA: 0x0004B564 File Offset: 0x00049764
		private void WriteElementEpm(XmlWriter writer, EpmTargetPathSegment targetSegment, EntryPropertiesValueCache epmValueCache, IEdmEntityTypeReference entityType, ref string alreadyDeclaredPrefix)
		{
			string text = targetSegment.SegmentNamespacePrefix ?? string.Empty;
			writer.WriteStartElement(text, targetSegment.SegmentName, targetSegment.SegmentNamespaceUri);
			if (text.Length > 0)
			{
				EpmCustomWriter.WriteNamespaceDeclaration(writer, targetSegment, ref alreadyDeclaredPrefix);
			}
			foreach (EpmTargetPathSegment epmTargetPathSegment in targetSegment.SubSegments)
			{
				if (epmTargetPathSegment.IsAttribute)
				{
					this.WriteAttributeEpm(writer, epmTargetPathSegment, epmValueCache, entityType, ref alreadyDeclaredPrefix);
				}
			}
			if (targetSegment.HasContent)
			{
				string entryPropertyValueAsText = this.GetEntryPropertyValueAsText(targetSegment, epmValueCache, entityType);
				ODataAtomWriterUtils.WriteString(writer, entryPropertyValueAsText);
			}
			else
			{
				foreach (EpmTargetPathSegment epmTargetPathSegment2 in targetSegment.SubSegments)
				{
					if (!epmTargetPathSegment2.IsAttribute)
					{
						this.WriteElementEpm(writer, epmTargetPathSegment2, epmValueCache, entityType, ref alreadyDeclaredPrefix);
					}
				}
			}
			writer.WriteEndElement();
		}

		// Token: 0x06001449 RID: 5193 RVA: 0x0004B670 File Offset: 0x00049870
		private void WriteAttributeEpm(XmlWriter writer, EpmTargetPathSegment targetSegment, EntryPropertiesValueCache epmValueCache, IEdmEntityTypeReference entityType, ref string alreadyDeclaredPrefix)
		{
			string entryPropertyValueAsText = this.GetEntryPropertyValueAsText(targetSegment, epmValueCache, entityType);
			string text = targetSegment.SegmentNamespacePrefix ?? string.Empty;
			writer.WriteAttributeString(text, targetSegment.AttributeName, targetSegment.SegmentNamespaceUri, entryPropertyValueAsText);
			if (text.Length > 0)
			{
				EpmCustomWriter.WriteNamespaceDeclaration(writer, targetSegment, ref alreadyDeclaredPrefix);
			}
		}

		// Token: 0x0600144A RID: 5194 RVA: 0x0004B6C0 File Offset: 0x000498C0
		private string GetEntryPropertyValueAsText(EpmTargetPathSegment targetSegment, EntryPropertiesValueCache epmValueCache, IEdmEntityTypeReference entityType)
		{
			object obj = base.ReadEntryPropertyValue(targetSegment.EpmInfo, epmValueCache, entityType);
			if (obj == null)
			{
				return string.Empty;
			}
			return EpmWriterUtils.GetPropertyValueAsText(obj);
		}
	}
}
