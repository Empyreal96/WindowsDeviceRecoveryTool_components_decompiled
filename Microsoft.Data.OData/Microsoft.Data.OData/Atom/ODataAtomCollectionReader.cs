using System;
using System.Xml;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x02000225 RID: 549
	internal sealed class ODataAtomCollectionReader : ODataCollectionReaderCore
	{
		// Token: 0x06001130 RID: 4400 RVA: 0x0004028D File Offset: 0x0003E48D
		internal ODataAtomCollectionReader(ODataAtomInputContext atomInputContext, IEdmTypeReference expectedItemTypeReference) : base(atomInputContext, expectedItemTypeReference, null)
		{
			this.atomInputContext = atomInputContext;
			this.atomCollectionDeserializer = new ODataAtomCollectionDeserializer(atomInputContext);
		}

		// Token: 0x06001131 RID: 4401 RVA: 0x000402AC File Offset: 0x0003E4AC
		protected override bool ReadAtStartImplementation()
		{
			this.atomCollectionDeserializer.ReadPayloadStart();
			bool isCollectionElementEmpty;
			ODataCollectionStart item = this.atomCollectionDeserializer.ReadCollectionStart(out isCollectionElementEmpty);
			base.EnterScope(ODataCollectionReaderState.CollectionStart, item, isCollectionElementEmpty);
			return true;
		}

		// Token: 0x06001132 RID: 4402 RVA: 0x000402DC File Offset: 0x0003E4DC
		protected override bool ReadAtCollectionStartImplementation()
		{
			this.atomCollectionDeserializer.SkipToElementInODataNamespace();
			if (this.atomCollectionDeserializer.XmlReader.NodeType == XmlNodeType.EndElement || base.IsCollectionElementEmpty)
			{
				this.atomCollectionDeserializer.ReadCollectionEnd();
				base.ReplaceScope(ODataCollectionReaderState.CollectionEnd, this.Item);
			}
			else
			{
				object item = this.atomCollectionDeserializer.ReadCollectionItem(base.ExpectedItemTypeReference, base.CollectionValidator);
				base.EnterScope(ODataCollectionReaderState.Value, item);
			}
			return true;
		}

		// Token: 0x06001133 RID: 4403 RVA: 0x0004034C File Offset: 0x0003E54C
		protected override bool ReadAtValueImplementation()
		{
			this.atomCollectionDeserializer.SkipToElementInODataNamespace();
			if (this.atomInputContext.XmlReader.NodeType == XmlNodeType.EndElement)
			{
				this.atomCollectionDeserializer.ReadCollectionEnd();
				base.PopScope(ODataCollectionReaderState.Value);
				base.ReplaceScope(ODataCollectionReaderState.CollectionEnd, this.Item);
			}
			else
			{
				object item = this.atomCollectionDeserializer.ReadCollectionItem(base.ExpectedItemTypeReference, base.CollectionValidator);
				base.ReplaceScope(ODataCollectionReaderState.Value, item);
			}
			return true;
		}

		// Token: 0x06001134 RID: 4404 RVA: 0x000403BA File Offset: 0x0003E5BA
		protected override bool ReadAtCollectionEndImplementation()
		{
			this.atomCollectionDeserializer.ReadPayloadEnd();
			base.PopScope(ODataCollectionReaderState.CollectionEnd);
			base.ReplaceScope(ODataCollectionReaderState.Completed, null);
			return false;
		}

		// Token: 0x04000657 RID: 1623
		private readonly ODataAtomInputContext atomInputContext;

		// Token: 0x04000658 RID: 1624
		private readonly ODataAtomCollectionDeserializer atomCollectionDeserializer;
	}
}
