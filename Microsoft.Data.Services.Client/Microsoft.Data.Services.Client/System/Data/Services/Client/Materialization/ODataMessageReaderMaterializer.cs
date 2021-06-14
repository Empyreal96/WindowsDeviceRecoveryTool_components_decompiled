using System;
using System.Collections.Generic;
using System.Data.Services.Client.Metadata;
using Microsoft.Data.Edm;
using Microsoft.Data.OData;

namespace System.Data.Services.Client.Materialization
{
	// Token: 0x0200006A RID: 106
	internal abstract class ODataMessageReaderMaterializer : ODataMaterializer
	{
		// Token: 0x0600038C RID: 908 RVA: 0x0000FB78 File Offset: 0x0000DD78
		public ODataMessageReaderMaterializer(ODataMessageReader reader, IODataMaterializerContext context, Type expectedType, bool? singleResult) : base(context, expectedType)
		{
			this.messageReader = reader;
			this.SingleResult = singleResult;
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x0600038D RID: 909 RVA: 0x0000FB91 File Offset: 0x0000DD91
		internal sealed override ODataFeed CurrentFeed
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x0600038E RID: 910 RVA: 0x0000FB94 File Offset: 0x0000DD94
		internal sealed override ODataEntry CurrentEntry
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x0600038F RID: 911 RVA: 0x0000FB97 File Offset: 0x0000DD97
		internal sealed override bool IsEndOfStream
		{
			get
			{
				return this.hasReadValue;
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000390 RID: 912 RVA: 0x0000FB9F File Offset: 0x0000DD9F
		internal override long CountValue
		{
			get
			{
				throw new InvalidOperationException(Strings.MaterializeFromAtom_CountNotPresent);
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000391 RID: 913 RVA: 0x0000FBAB File Offset: 0x0000DDAB
		internal sealed override ProjectionPlan MaterializeEntryPlan
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000392 RID: 914 RVA: 0x0000FBB2 File Offset: 0x0000DDB2
		protected sealed override bool IsDisposed
		{
			get
			{
				return this.messageReader == null;
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000393 RID: 915 RVA: 0x0000FBBD File Offset: 0x0000DDBD
		protected override ODataFormat Format
		{
			get
			{
				return ODataUtils.GetReadFormat(this.messageReader);
			}
		}

		// Token: 0x06000394 RID: 916 RVA: 0x0000FBCA File Offset: 0x0000DDCA
		internal sealed override void ClearLog()
		{
		}

		// Token: 0x06000395 RID: 917 RVA: 0x0000FBCC File Offset: 0x0000DDCC
		internal sealed override void ApplyLogToContext()
		{
		}

		// Token: 0x06000396 RID: 918 RVA: 0x0000FBD0 File Offset: 0x0000DDD0
		protected sealed override bool ReadImplementation()
		{
			if (!this.hasReadValue)
			{
				try
				{
					ClientEdmModel model = base.MaterializerContext.Model;
					Type type = base.ExpectedType;
					IEdmTypeReference edmTypeReference = model.GetOrCreateEdmType(type).ToEdmTypeReference(ClientTypeUtil.CanAssignNull(type));
					if (this.SingleResult != null && !this.SingleResult.Value && edmTypeReference.Definition.TypeKind != EdmTypeKind.Collection)
					{
						type = typeof(ICollection<>).MakeGenericType(new Type[]
						{
							type
						});
						edmTypeReference = model.GetOrCreateEdmType(type).ToEdmTypeReference(false);
					}
					IEdmTypeReference expectedReaderType = base.MaterializerContext.ResolveExpectedTypeForReading(type).ToEdmTypeReference(edmTypeReference.IsNullable);
					this.ReadWithExpectedType(edmTypeReference, expectedReaderType);
				}
				catch (ODataErrorException ex)
				{
					throw new DataServiceClientException(Strings.Deserialize_ServerException(ex.Error.Message), ex);
				}
				catch (ODataException ex2)
				{
					throw new InvalidOperationException(ex2.Message, ex2);
				}
				catch (ArgumentException ex3)
				{
					throw new InvalidOperationException(ex3.Message, ex3);
				}
				finally
				{
					this.hasReadValue = true;
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000397 RID: 919 RVA: 0x0000FD0C File Offset: 0x0000DF0C
		protected sealed override void OnDispose()
		{
			if (this.messageReader != null)
			{
				this.messageReader.Dispose();
				this.messageReader = null;
			}
		}

		// Token: 0x06000398 RID: 920
		protected abstract void ReadWithExpectedType(IEdmTypeReference expectedClientType, IEdmTypeReference expectedReaderType);

		// Token: 0x040002AB RID: 683
		protected readonly bool? SingleResult;

		// Token: 0x040002AC RID: 684
		protected ODataMessageReader messageReader;

		// Token: 0x040002AD RID: 685
		private bool hasReadValue;
	}
}
