using System;
using System.Threading.Tasks;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.JsonLight
{
	// Token: 0x02000198 RID: 408
	internal sealed class ODataJsonLightParameterWriter : ODataParameterWriterCore
	{
		// Token: 0x06000C53 RID: 3155 RVA: 0x0002A45C File Offset: 0x0002865C
		internal ODataJsonLightParameterWriter(ODataJsonLightOutputContext jsonLightOutputContext, IEdmFunctionImport functionImport) : base(jsonLightOutputContext, functionImport)
		{
			this.jsonLightOutputContext = jsonLightOutputContext;
			this.jsonLightValueSerializer = new ODataJsonLightValueSerializer(this.jsonLightOutputContext);
		}

		// Token: 0x06000C54 RID: 3156 RVA: 0x0002A47E File Offset: 0x0002867E
		protected override void VerifyNotDisposed()
		{
			this.jsonLightOutputContext.VerifyNotDisposed();
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x0002A48B File Offset: 0x0002868B
		protected override void FlushSynchronously()
		{
			this.jsonLightOutputContext.Flush();
		}

		// Token: 0x06000C56 RID: 3158 RVA: 0x0002A498 File Offset: 0x00028698
		protected override Task FlushAsynchronously()
		{
			return this.jsonLightOutputContext.FlushAsync();
		}

		// Token: 0x06000C57 RID: 3159 RVA: 0x0002A4A5 File Offset: 0x000286A5
		protected override void StartPayload()
		{
			this.jsonLightValueSerializer.WritePayloadStart();
			this.jsonLightOutputContext.JsonWriter.StartObjectScope();
		}

		// Token: 0x06000C58 RID: 3160 RVA: 0x0002A4C2 File Offset: 0x000286C2
		protected override void EndPayload()
		{
			this.jsonLightOutputContext.JsonWriter.EndObjectScope();
			this.jsonLightValueSerializer.WritePayloadEnd();
		}

		// Token: 0x06000C59 RID: 3161 RVA: 0x0002A4E0 File Offset: 0x000286E0
		protected override void WriteValueParameter(string parameterName, object parameterValue, IEdmTypeReference expectedTypeReference)
		{
			this.jsonLightOutputContext.JsonWriter.WriteName(parameterName);
			if (parameterValue == null)
			{
				this.jsonLightOutputContext.JsonWriter.WriteValue(null);
				return;
			}
			ODataComplexValue odataComplexValue = parameterValue as ODataComplexValue;
			if (odataComplexValue != null)
			{
				this.jsonLightValueSerializer.WriteComplexValue(odataComplexValue, expectedTypeReference, false, false, base.DuplicatePropertyNamesChecker);
				base.DuplicatePropertyNamesChecker.Clear();
				return;
			}
			this.jsonLightValueSerializer.WritePrimitiveValue(parameterValue, expectedTypeReference);
		}

		// Token: 0x06000C5A RID: 3162 RVA: 0x0002A54B File Offset: 0x0002874B
		protected override ODataCollectionWriter CreateFormatCollectionWriter(string parameterName, IEdmTypeReference expectedItemType)
		{
			this.jsonLightOutputContext.JsonWriter.WriteName(parameterName);
			return new ODataJsonLightCollectionWriter(this.jsonLightOutputContext, expectedItemType, this);
		}

		// Token: 0x0400043A RID: 1082
		private readonly ODataJsonLightOutputContext jsonLightOutputContext;

		// Token: 0x0400043B RID: 1083
		private readonly ODataJsonLightValueSerializer jsonLightValueSerializer;
	}
}
