using System;
using System.Threading.Tasks;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.VerboseJson
{
	// Token: 0x02000271 RID: 625
	internal sealed class ODataVerboseJsonCollectionWriter : ODataCollectionWriterCore
	{
		// Token: 0x060014BC RID: 5308 RVA: 0x0004D2E8 File Offset: 0x0004B4E8
		internal ODataVerboseJsonCollectionWriter(ODataVerboseJsonOutputContext verboseJsonOutputContext, IEdmTypeReference itemTypeReference) : base(verboseJsonOutputContext, itemTypeReference)
		{
			this.verboseJsonOutputContext = verboseJsonOutputContext;
			this.verboseJsonCollectionSerializer = new ODataVerboseJsonCollectionSerializer(this.verboseJsonOutputContext);
		}

		// Token: 0x060014BD RID: 5309 RVA: 0x0004D30A File Offset: 0x0004B50A
		internal ODataVerboseJsonCollectionWriter(ODataVerboseJsonOutputContext verboseJsonOutputContext, IEdmTypeReference expectedItemType, IODataReaderWriterListener listener) : base(verboseJsonOutputContext, expectedItemType, listener)
		{
			this.verboseJsonOutputContext = verboseJsonOutputContext;
			this.verboseJsonCollectionSerializer = new ODataVerboseJsonCollectionSerializer(this.verboseJsonOutputContext);
		}

		// Token: 0x060014BE RID: 5310 RVA: 0x0004D32D File Offset: 0x0004B52D
		protected override void VerifyNotDisposed()
		{
			this.verboseJsonOutputContext.VerifyNotDisposed();
		}

		// Token: 0x060014BF RID: 5311 RVA: 0x0004D33A File Offset: 0x0004B53A
		protected override void FlushSynchronously()
		{
			this.verboseJsonOutputContext.Flush();
		}

		// Token: 0x060014C0 RID: 5312 RVA: 0x0004D347 File Offset: 0x0004B547
		protected override Task FlushAsynchronously()
		{
			return this.verboseJsonOutputContext.FlushAsync();
		}

		// Token: 0x060014C1 RID: 5313 RVA: 0x0004D354 File Offset: 0x0004B554
		protected override void StartPayload()
		{
			this.verboseJsonCollectionSerializer.WritePayloadStart();
		}

		// Token: 0x060014C2 RID: 5314 RVA: 0x0004D361 File Offset: 0x0004B561
		protected override void EndPayload()
		{
			this.verboseJsonCollectionSerializer.WritePayloadEnd();
		}

		// Token: 0x060014C3 RID: 5315 RVA: 0x0004D36E File Offset: 0x0004B56E
		protected override void StartCollection(ODataCollectionStart collectionStart)
		{
			this.verboseJsonCollectionSerializer.WriteCollectionStart();
		}

		// Token: 0x060014C4 RID: 5316 RVA: 0x0004D37B File Offset: 0x0004B57B
		protected override void EndCollection()
		{
			this.verboseJsonCollectionSerializer.WriteCollectionEnd();
		}

		// Token: 0x060014C5 RID: 5317 RVA: 0x0004D388 File Offset: 0x0004B588
		protected override void WriteCollectionItem(object item, IEdmTypeReference expectedItemType)
		{
			if (item == null)
			{
				ValidationUtils.ValidateNullCollectionItem(expectedItemType, this.verboseJsonOutputContext.MessageWriterSettings.WriterBehavior);
				this.verboseJsonOutputContext.JsonWriter.WriteValue(null);
				return;
			}
			ODataComplexValue odataComplexValue = item as ODataComplexValue;
			if (odataComplexValue != null)
			{
				this.verboseJsonCollectionSerializer.WriteComplexValue(odataComplexValue, expectedItemType, false, base.DuplicatePropertyNamesChecker, base.CollectionValidator);
				base.DuplicatePropertyNamesChecker.Clear();
				return;
			}
			this.verboseJsonCollectionSerializer.WritePrimitiveValue(item, base.CollectionValidator, expectedItemType);
		}

		// Token: 0x04000753 RID: 1875
		private readonly ODataVerboseJsonOutputContext verboseJsonOutputContext;

		// Token: 0x04000754 RID: 1876
		private readonly ODataVerboseJsonCollectionSerializer verboseJsonCollectionSerializer;
	}
}
