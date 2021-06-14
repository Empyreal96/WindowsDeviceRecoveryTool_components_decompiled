using System;
using System.Threading.Tasks;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x02000272 RID: 626
	internal sealed class ODataAtomCollectionWriter : ODataCollectionWriterCore
	{
		// Token: 0x060014C6 RID: 5318 RVA: 0x0004D403 File Offset: 0x0004B603
		internal ODataAtomCollectionWriter(ODataAtomOutputContext atomOutputContext, IEdmTypeReference itemTypeReference) : base(atomOutputContext, itemTypeReference)
		{
			this.atomOutputContext = atomOutputContext;
			this.atomCollectionSerializer = new ODataAtomCollectionSerializer(atomOutputContext);
		}

		// Token: 0x060014C7 RID: 5319 RVA: 0x0004D420 File Offset: 0x0004B620
		protected override void VerifyNotDisposed()
		{
			this.atomOutputContext.VerifyNotDisposed();
		}

		// Token: 0x060014C8 RID: 5320 RVA: 0x0004D42D File Offset: 0x0004B62D
		protected override void FlushSynchronously()
		{
			this.atomOutputContext.Flush();
		}

		// Token: 0x060014C9 RID: 5321 RVA: 0x0004D43A File Offset: 0x0004B63A
		protected override Task FlushAsynchronously()
		{
			return this.atomOutputContext.FlushAsync();
		}

		// Token: 0x060014CA RID: 5322 RVA: 0x0004D447 File Offset: 0x0004B647
		protected override void StartPayload()
		{
			this.atomCollectionSerializer.WritePayloadStart();
		}

		// Token: 0x060014CB RID: 5323 RVA: 0x0004D454 File Offset: 0x0004B654
		protected override void EndPayload()
		{
			this.atomCollectionSerializer.WritePayloadEnd();
		}

		// Token: 0x060014CC RID: 5324 RVA: 0x0004D464 File Offset: 0x0004B664
		protected override void StartCollection(ODataCollectionStart collectionStart)
		{
			string name = collectionStart.Name;
			if (name == null)
			{
				throw new ODataException(Strings.ODataAtomCollectionWriter_CollectionNameMustNotBeNull);
			}
			this.atomOutputContext.XmlWriter.WriteStartElement(name, this.atomCollectionSerializer.MessageWriterSettings.WriterBehavior.ODataNamespace);
			this.atomOutputContext.XmlWriter.WriteAttributeString("xmlns", "http://www.w3.org/2000/xmlns/", this.atomCollectionSerializer.MessageWriterSettings.WriterBehavior.ODataNamespace);
			this.atomCollectionSerializer.WriteDefaultNamespaceAttributes(ODataAtomSerializer.DefaultNamespaceFlags.ODataMetadata | ODataAtomSerializer.DefaultNamespaceFlags.GeoRss | ODataAtomSerializer.DefaultNamespaceFlags.Gml);
		}

		// Token: 0x060014CD RID: 5325 RVA: 0x0004D4E8 File Offset: 0x0004B6E8
		protected override void EndCollection()
		{
			this.atomOutputContext.XmlWriter.WriteEndElement();
		}

		// Token: 0x060014CE RID: 5326 RVA: 0x0004D4FC File Offset: 0x0004B6FC
		protected override void WriteCollectionItem(object item, IEdmTypeReference expectedItemType)
		{
			this.atomOutputContext.XmlWriter.WriteStartElement("element", this.atomCollectionSerializer.MessageWriterSettings.WriterBehavior.ODataNamespace);
			if (item == null)
			{
				ValidationUtils.ValidateNullCollectionItem(expectedItemType, this.atomOutputContext.MessageWriterSettings.WriterBehavior);
				this.atomOutputContext.XmlWriter.WriteAttributeString("null", "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata", "true");
			}
			else
			{
				ODataComplexValue odataComplexValue = item as ODataComplexValue;
				if (odataComplexValue != null)
				{
					this.atomCollectionSerializer.WriteComplexValue(odataComplexValue, expectedItemType, false, true, null, null, base.DuplicatePropertyNamesChecker, base.CollectionValidator, null, null, null);
					base.DuplicatePropertyNamesChecker.Clear();
				}
				else
				{
					this.atomCollectionSerializer.WritePrimitiveValue(item, base.CollectionValidator, expectedItemType, null);
				}
			}
			this.atomOutputContext.XmlWriter.WriteEndElement();
		}

		// Token: 0x04000755 RID: 1877
		private readonly ODataAtomOutputContext atomOutputContext;

		// Token: 0x04000756 RID: 1878
		private readonly ODataAtomCollectionSerializer atomCollectionSerializer;
	}
}
