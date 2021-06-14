using System;
using System.Data.Services.Client.Metadata;
using Microsoft.Data.Edm;
using Microsoft.Data.OData;

namespace System.Data.Services.Client.Materialization
{
	// Token: 0x02000071 RID: 113
	internal sealed class ODataLinksMaterializer : ODataMessageReaderMaterializer
	{
		// Token: 0x060003C0 RID: 960 RVA: 0x000100F0 File Offset: 0x0000E2F0
		public ODataLinksMaterializer(ODataMessageReader reader, IODataMaterializerContext materializerContext, Type expectedType, bool? singleResult) : base(reader, materializerContext, expectedType, singleResult)
		{
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060003C1 RID: 961 RVA: 0x00010100 File Offset: 0x0000E300
		internal override long CountValue
		{
			get
			{
				if (this.links == null && !this.IsDisposed)
				{
					this.ReadLinks();
				}
				if (this.links != null && this.links.Count != null)
				{
					return this.links.Count.Value;
				}
				throw new InvalidOperationException(Strings.MaterializeFromAtom_CountNotPresent);
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060003C2 RID: 962 RVA: 0x0001015E File Offset: 0x0000E35E
		internal override object CurrentValue
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060003C3 RID: 963 RVA: 0x00010161 File Offset: 0x0000E361
		internal override bool IsCountable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x00010164 File Offset: 0x0000E364
		protected override void ReadWithExpectedType(IEdmTypeReference expectedClientType, IEdmTypeReference expectedReaderType)
		{
			this.ReadLinks();
			Type type = Nullable.GetUnderlyingType(base.ExpectedType) ?? base.ExpectedType;
			ClientEdmModel model = base.MaterializerContext.Model;
			ClientTypeAnnotation clientTypeAnnotation = model.GetClientTypeAnnotation(model.GetOrCreateEdmType(type));
			if (clientTypeAnnotation.IsEntityType)
			{
				throw Error.InvalidOperation(Strings.AtomMaterializer_InvalidEntityType(clientTypeAnnotation.ElementTypeName));
			}
			throw Error.InvalidOperation(Strings.Deserialize_MixedTextWithComment);
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x000101CC File Offset: 0x0000E3CC
		private void ReadLinks()
		{
			try
			{
				if (this.links == null)
				{
					this.links = this.messageReader.ReadEntityReferenceLinks();
				}
			}
			catch (ODataErrorException ex)
			{
				throw new DataServiceClientException(Strings.Deserialize_ServerException(ex.Error.Message), ex);
			}
			catch (ODataException ex2)
			{
				throw new InvalidOperationException(ex2.Message, ex2);
			}
		}

		// Token: 0x040002B4 RID: 692
		private ODataEntityReferenceLinks links;
	}
}
