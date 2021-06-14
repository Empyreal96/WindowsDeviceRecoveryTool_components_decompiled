using System;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Evaluation;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData
{
	// Token: 0x02000115 RID: 277
	internal class ODataFeedAndEntryTypeContext : IODataFeedAndEntryTypeContext
	{
		// Token: 0x06000776 RID: 1910 RVA: 0x000198A4 File Offset: 0x00017AA4
		private ODataFeedAndEntryTypeContext(bool throwIfMissingTypeInfo)
		{
			this.throwIfMissingTypeInfo = throwIfMissingTypeInfo;
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000777 RID: 1911 RVA: 0x000198B3 File Offset: 0x00017AB3
		public virtual string EntitySetName
		{
			get
			{
				return this.ValidateAndReturn<string>(null);
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000778 RID: 1912 RVA: 0x000198BC File Offset: 0x00017ABC
		public virtual string EntitySetElementTypeName
		{
			get
			{
				return this.ValidateAndReturn<string>(null);
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000779 RID: 1913 RVA: 0x000198C5 File Offset: 0x00017AC5
		public virtual string ExpectedEntityTypeName
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x0600077A RID: 1914 RVA: 0x000198C8 File Offset: 0x00017AC8
		public virtual bool IsMediaLinkEntry
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x0600077B RID: 1915 RVA: 0x000198CB File Offset: 0x00017ACB
		public virtual UrlConvention UrlConvention
		{
			get
			{
				return ODataFeedAndEntryTypeContext.DefaultUrlConvention;
			}
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x000198D2 File Offset: 0x00017AD2
		internal static ODataFeedAndEntryTypeContext Create(ODataFeedAndEntrySerializationInfo serializationInfo, IEdmEntitySet entitySet, IEdmEntityType entitySetElementType, IEdmEntityType expectedEntityType, IEdmModel model, bool throwIfMissingTypeInfo)
		{
			if (serializationInfo != null)
			{
				return new ODataFeedAndEntryTypeContext.ODataFeedAndEntryTypeContextWithoutModel(serializationInfo);
			}
			if (entitySet != null && model.IsUserModel())
			{
				return new ODataFeedAndEntryTypeContext.ODataFeedAndEntryTypeContextWithModel(entitySet, entitySetElementType, expectedEntityType, model);
			}
			return new ODataFeedAndEntryTypeContext(throwIfMissingTypeInfo);
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x000198FC File Offset: 0x00017AFC
		private T ValidateAndReturn<T>(T value) where T : class
		{
			if (this.throwIfMissingTypeInfo && value == null)
			{
				throw new ODataException(Strings.ODataFeedAndEntryTypeContext_MetadataOrSerializationInfoMissing);
			}
			return value;
		}

		// Token: 0x040002CC RID: 716
		private static readonly UrlConvention DefaultUrlConvention = UrlConvention.CreateWithExplicitValue(false);

		// Token: 0x040002CD RID: 717
		private readonly bool throwIfMissingTypeInfo;

		// Token: 0x02000116 RID: 278
		private sealed class ODataFeedAndEntryTypeContextWithoutModel : ODataFeedAndEntryTypeContext
		{
			// Token: 0x0600077F RID: 1919 RVA: 0x00019927 File Offset: 0x00017B27
			internal ODataFeedAndEntryTypeContextWithoutModel(ODataFeedAndEntrySerializationInfo serializationInfo) : base(false)
			{
				this.serializationInfo = serializationInfo;
			}

			// Token: 0x170001DB RID: 475
			// (get) Token: 0x06000780 RID: 1920 RVA: 0x00019937 File Offset: 0x00017B37
			public override string EntitySetName
			{
				get
				{
					return this.serializationInfo.EntitySetName;
				}
			}

			// Token: 0x170001DC RID: 476
			// (get) Token: 0x06000781 RID: 1921 RVA: 0x00019944 File Offset: 0x00017B44
			public override string EntitySetElementTypeName
			{
				get
				{
					return this.serializationInfo.EntitySetElementTypeName;
				}
			}

			// Token: 0x170001DD RID: 477
			// (get) Token: 0x06000782 RID: 1922 RVA: 0x00019951 File Offset: 0x00017B51
			public override string ExpectedEntityTypeName
			{
				get
				{
					return this.serializationInfo.ExpectedTypeName;
				}
			}

			// Token: 0x170001DE RID: 478
			// (get) Token: 0x06000783 RID: 1923 RVA: 0x0001995E File Offset: 0x00017B5E
			public override bool IsMediaLinkEntry
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170001DF RID: 479
			// (get) Token: 0x06000784 RID: 1924 RVA: 0x00019961 File Offset: 0x00017B61
			public override UrlConvention UrlConvention
			{
				get
				{
					return ODataFeedAndEntryTypeContext.DefaultUrlConvention;
				}
			}

			// Token: 0x040002CE RID: 718
			private readonly ODataFeedAndEntrySerializationInfo serializationInfo;
		}

		// Token: 0x02000117 RID: 279
		private sealed class ODataFeedAndEntryTypeContextWithModel : ODataFeedAndEntryTypeContext
		{
			// Token: 0x06000785 RID: 1925 RVA: 0x000199EC File Offset: 0x00017BEC
			internal ODataFeedAndEntryTypeContextWithModel(IEdmEntitySet entitySet, IEdmEntityType entitySetElementType, IEdmEntityType expectedEntityType, IEdmModel model) : base(false)
			{
				this.entitySet = entitySet;
				this.entitySetElementType = entitySetElementType;
				this.expectedEntityType = expectedEntityType;
				this.model = model;
				this.lazyEntitySetName = new SimpleLazy<string>(delegate()
				{
					if (!this.model.IsDefaultEntityContainer(this.entitySet.Container))
					{
						return this.entitySet.Container.FullName() + "." + this.entitySet.Name;
					}
					return this.entitySet.Name;
				});
				this.lazyIsMediaLinkEntry = new SimpleLazy<bool>(() => this.model.HasDefaultStream(this.expectedEntityType));
				this.lazyUrlConvention = new SimpleLazy<UrlConvention>(() => UrlConvention.ForEntityContainer(this.model, this.entitySet.Container));
			}

			// Token: 0x170001E0 RID: 480
			// (get) Token: 0x06000786 RID: 1926 RVA: 0x00019A77 File Offset: 0x00017C77
			public override string EntitySetName
			{
				get
				{
					return this.lazyEntitySetName.Value;
				}
			}

			// Token: 0x170001E1 RID: 481
			// (get) Token: 0x06000787 RID: 1927 RVA: 0x00019A84 File Offset: 0x00017C84
			public override string EntitySetElementTypeName
			{
				get
				{
					return this.entitySetElementType.FullName();
				}
			}

			// Token: 0x170001E2 RID: 482
			// (get) Token: 0x06000788 RID: 1928 RVA: 0x00019A91 File Offset: 0x00017C91
			public override string ExpectedEntityTypeName
			{
				get
				{
					return this.expectedEntityType.FullName();
				}
			}

			// Token: 0x170001E3 RID: 483
			// (get) Token: 0x06000789 RID: 1929 RVA: 0x00019A9E File Offset: 0x00017C9E
			public override bool IsMediaLinkEntry
			{
				get
				{
					return this.lazyIsMediaLinkEntry.Value;
				}
			}

			// Token: 0x170001E4 RID: 484
			// (get) Token: 0x0600078A RID: 1930 RVA: 0x00019AAB File Offset: 0x00017CAB
			public override UrlConvention UrlConvention
			{
				get
				{
					return this.lazyUrlConvention.Value;
				}
			}

			// Token: 0x040002CF RID: 719
			private readonly IEdmModel model;

			// Token: 0x040002D0 RID: 720
			private readonly IEdmEntitySet entitySet;

			// Token: 0x040002D1 RID: 721
			private readonly IEdmEntityType entitySetElementType;

			// Token: 0x040002D2 RID: 722
			private readonly IEdmEntityType expectedEntityType;

			// Token: 0x040002D3 RID: 723
			private readonly SimpleLazy<string> lazyEntitySetName;

			// Token: 0x040002D4 RID: 724
			private readonly SimpleLazy<bool> lazyIsMediaLinkEntry;

			// Token: 0x040002D5 RID: 725
			private readonly SimpleLazy<UrlConvention> lazyUrlConvention;
		}
	}
}
