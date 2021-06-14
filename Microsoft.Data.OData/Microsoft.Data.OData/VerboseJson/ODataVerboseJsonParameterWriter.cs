using System;
using System.Threading.Tasks;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.VerboseJson
{
	// Token: 0x020001DB RID: 475
	internal sealed class ODataVerboseJsonParameterWriter : ODataParameterWriterCore
	{
		// Token: 0x06000EB7 RID: 3767 RVA: 0x00033F76 File Offset: 0x00032176
		internal ODataVerboseJsonParameterWriter(ODataVerboseJsonOutputContext verboseJsonOutputContext, IEdmFunctionImport functionImport) : base(verboseJsonOutputContext, functionImport)
		{
			this.verboseJsonOutputContext = verboseJsonOutputContext;
			this.verboseJsonPropertyAndValueSerializer = new ODataVerboseJsonPropertyAndValueSerializer(this.verboseJsonOutputContext);
		}

		// Token: 0x06000EB8 RID: 3768 RVA: 0x00033F98 File Offset: 0x00032198
		protected override void VerifyNotDisposed()
		{
			this.verboseJsonOutputContext.VerifyNotDisposed();
		}

		// Token: 0x06000EB9 RID: 3769 RVA: 0x00033FA5 File Offset: 0x000321A5
		protected override void FlushSynchronously()
		{
			this.verboseJsonOutputContext.Flush();
		}

		// Token: 0x06000EBA RID: 3770 RVA: 0x00033FB2 File Offset: 0x000321B2
		protected override Task FlushAsynchronously()
		{
			return this.verboseJsonOutputContext.FlushAsync();
		}

		// Token: 0x06000EBB RID: 3771 RVA: 0x00033FBF File Offset: 0x000321BF
		protected override void StartPayload()
		{
			this.verboseJsonPropertyAndValueSerializer.WritePayloadStart();
			this.verboseJsonOutputContext.JsonWriter.StartObjectScope();
		}

		// Token: 0x06000EBC RID: 3772 RVA: 0x00033FDC File Offset: 0x000321DC
		protected override void EndPayload()
		{
			this.verboseJsonOutputContext.JsonWriter.EndObjectScope();
			this.verboseJsonPropertyAndValueSerializer.WritePayloadEnd();
		}

		// Token: 0x06000EBD RID: 3773 RVA: 0x00033FFC File Offset: 0x000321FC
		protected override void WriteValueParameter(string parameterName, object parameterValue, IEdmTypeReference expectedTypeReference)
		{
			this.verboseJsonOutputContext.JsonWriter.WriteName(parameterName);
			if (parameterValue == null)
			{
				this.verboseJsonOutputContext.JsonWriter.WriteValue(null);
				return;
			}
			ODataComplexValue odataComplexValue = parameterValue as ODataComplexValue;
			if (odataComplexValue != null)
			{
				this.verboseJsonPropertyAndValueSerializer.WriteComplexValue(odataComplexValue, expectedTypeReference, false, base.DuplicatePropertyNamesChecker, null);
				base.DuplicatePropertyNamesChecker.Clear();
				return;
			}
			this.verboseJsonPropertyAndValueSerializer.WritePrimitiveValue(parameterValue, null, expectedTypeReference);
		}

		// Token: 0x06000EBE RID: 3774 RVA: 0x00034068 File Offset: 0x00032268
		protected override ODataCollectionWriter CreateFormatCollectionWriter(string parameterName, IEdmTypeReference expectedItemType)
		{
			this.verboseJsonOutputContext.JsonWriter.WriteName(parameterName);
			return new ODataVerboseJsonCollectionWriter(this.verboseJsonOutputContext, expectedItemType, this);
		}

		// Token: 0x04000519 RID: 1305
		private readonly ODataVerboseJsonOutputContext verboseJsonOutputContext;

		// Token: 0x0400051A RID: 1306
		private readonly ODataVerboseJsonPropertyAndValueSerializer verboseJsonPropertyAndValueSerializer;
	}
}
