using System;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Json;

namespace Microsoft.Data.OData.VerboseJson
{
	// Token: 0x020001E1 RID: 481
	internal sealed class ODataVerboseJsonParameterReader : ODataParameterReaderCore
	{
		// Token: 0x06000EE7 RID: 3815 RVA: 0x00034C4C File Offset: 0x00032E4C
		internal ODataVerboseJsonParameterReader(ODataVerboseJsonInputContext verboseJsonInputContext, IEdmFunctionImport functionImport) : base(verboseJsonInputContext, functionImport)
		{
			this.verboseJsonInputContext = verboseJsonInputContext;
			this.verboseJsonPropertyAndValueDeserializer = new ODataVerboseJsonPropertyAndValueDeserializer(verboseJsonInputContext);
		}

		// Token: 0x06000EE8 RID: 3816 RVA: 0x00034C6C File Offset: 0x00032E6C
		protected override bool ReadAtStartImplementation()
		{
			this.verboseJsonPropertyAndValueDeserializer.ReadPayloadStart(false);
			if (this.verboseJsonPropertyAndValueDeserializer.JsonReader.NodeType == JsonNodeType.EndOfInput)
			{
				base.PopScope(ODataParameterReaderState.Start);
				base.EnterScope(ODataParameterReaderState.Completed, null, null);
				return false;
			}
			this.verboseJsonPropertyAndValueDeserializer.JsonReader.ReadStartObject();
			if (this.EndOfParameters())
			{
				this.ReadParametersEnd();
				return false;
			}
			this.ReadNextParameter();
			return true;
		}

		// Token: 0x06000EE9 RID: 3817 RVA: 0x00034CD1 File Offset: 0x00032ED1
		protected override bool ReadNextParameterImplementation()
		{
			base.PopScope(this.State);
			if (this.EndOfParameters())
			{
				this.ReadParametersEnd();
				return false;
			}
			this.ReadNextParameter();
			return true;
		}

		// Token: 0x06000EEA RID: 3818 RVA: 0x00034CF6 File Offset: 0x00032EF6
		protected override ODataCollectionReader CreateCollectionReader(IEdmTypeReference expectedItemTypeReference)
		{
			return new ODataVerboseJsonCollectionReader(this.verboseJsonInputContext, expectedItemTypeReference, this);
		}

		// Token: 0x06000EEB RID: 3819 RVA: 0x00034D05 File Offset: 0x00032F05
		private bool EndOfParameters()
		{
			return this.verboseJsonPropertyAndValueDeserializer.JsonReader.NodeType == JsonNodeType.EndObject;
		}

		// Token: 0x06000EEC RID: 3820 RVA: 0x00034D1A File Offset: 0x00032F1A
		private void ReadParametersEnd()
		{
			this.verboseJsonPropertyAndValueDeserializer.JsonReader.ReadEndObject();
			this.verboseJsonPropertyAndValueDeserializer.ReadPayloadEnd(false);
			base.PopScope(ODataParameterReaderState.Start);
			base.EnterScope(ODataParameterReaderState.Completed, null, null);
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x00034D48 File Offset: 0x00032F48
		private void ReadNextParameter()
		{
			string text = this.verboseJsonPropertyAndValueDeserializer.JsonReader.ReadPropertyName();
			IEdmTypeReference parameterTypeReference = base.GetParameterTypeReference(text);
			object obj;
			ODataParameterReaderState state;
			switch (parameterTypeReference.TypeKind())
			{
			case EdmTypeKind.Primitive:
			{
				IEdmPrimitiveTypeReference edmPrimitiveTypeReference = parameterTypeReference.AsPrimitive();
				if (edmPrimitiveTypeReference.PrimitiveKind() == EdmPrimitiveTypeKind.Stream)
				{
					throw new ODataException(Strings.ODataJsonParameterReader_UnsupportedPrimitiveParameterType(text, edmPrimitiveTypeReference.PrimitiveKind()));
				}
				obj = this.verboseJsonPropertyAndValueDeserializer.ReadNonEntityValue(edmPrimitiveTypeReference, null, null, true, text);
				state = ODataParameterReaderState.Value;
				goto IL_122;
			}
			case EdmTypeKind.Complex:
				obj = this.verboseJsonPropertyAndValueDeserializer.ReadNonEntityValue(parameterTypeReference, null, null, true, text);
				state = ODataParameterReaderState.Value;
				goto IL_122;
			case EdmTypeKind.Collection:
				obj = null;
				if (this.verboseJsonPropertyAndValueDeserializer.JsonReader.NodeType == JsonNodeType.PrimitiveValue)
				{
					obj = this.verboseJsonPropertyAndValueDeserializer.JsonReader.ReadPrimitiveValue();
					if (obj != null)
					{
						throw new ODataException(Strings.ODataJsonParameterReader_NullCollectionExpected(JsonNodeType.PrimitiveValue, obj));
					}
					state = ODataParameterReaderState.Value;
					goto IL_122;
				}
				else
				{
					if (((IEdmCollectionType)parameterTypeReference.Definition).ElementType.TypeKind() == EdmTypeKind.Entity)
					{
						throw new ODataException(Strings.ODataJsonParameterReader_UnsupportedParameterTypeKind(text, "Entity Collection"));
					}
					state = ODataParameterReaderState.Collection;
					goto IL_122;
				}
				break;
			}
			throw new ODataException(Strings.ODataJsonParameterReader_UnsupportedParameterTypeKind(text, parameterTypeReference.TypeKind()));
			IL_122:
			base.EnterScope(state, text, obj);
		}

		// Token: 0x04000520 RID: 1312
		private readonly ODataVerboseJsonInputContext verboseJsonInputContext;

		// Token: 0x04000521 RID: 1313
		private readonly ODataVerboseJsonPropertyAndValueDeserializer verboseJsonPropertyAndValueDeserializer;
	}
}
