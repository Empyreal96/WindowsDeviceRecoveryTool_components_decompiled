using System;
using Microsoft.Data.OData;
using Microsoft.Data.OData.Metadata;

namespace System.Data.Services.Common
{
	// Token: 0x020001ED RID: 493
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
	public sealed class EntityPropertyMappingAttribute : Attribute
	{
		// Token: 0x06000F1E RID: 3870 RVA: 0x000360D0 File Offset: 0x000342D0
		public EntityPropertyMappingAttribute(string sourcePath, SyndicationItemProperty targetSyndicationItem, SyndicationTextContentKind targetTextContentKind, bool keepInContent)
		{
			if (string.IsNullOrEmpty(sourcePath))
			{
				throw new ArgumentException(Strings.EntityPropertyMapping_EpmAttribute("sourcePath"));
			}
			this.sourcePath = sourcePath;
			this.targetPath = targetSyndicationItem.ToTargetPath();
			this.targetSyndicationItem = targetSyndicationItem;
			this.targetTextContentKind = targetTextContentKind;
			this.targetNamespacePrefix = "atom";
			this.targetNamespaceUri = "http://www.w3.org/2005/Atom";
			this.keepInContent = keepInContent;
		}

		// Token: 0x06000F1F RID: 3871 RVA: 0x0003613C File Offset: 0x0003433C
		public EntityPropertyMappingAttribute(string sourcePath, string targetPath, string targetNamespacePrefix, string targetNamespaceUri, bool keepInContent)
		{
			if (string.IsNullOrEmpty(sourcePath))
			{
				throw new ArgumentException(Strings.EntityPropertyMapping_EpmAttribute("sourcePath"));
			}
			this.sourcePath = sourcePath;
			if (string.IsNullOrEmpty(targetPath))
			{
				throw new ArgumentException(Strings.EntityPropertyMapping_EpmAttribute("targetPath"));
			}
			if (targetPath[0] == '@')
			{
				throw new ArgumentException(Strings.EntityPropertyMapping_InvalidTargetPath(targetPath));
			}
			this.targetPath = targetPath;
			this.targetSyndicationItem = SyndicationItemProperty.CustomProperty;
			this.targetTextContentKind = SyndicationTextContentKind.Plaintext;
			this.targetNamespacePrefix = targetNamespacePrefix;
			if (string.IsNullOrEmpty(targetNamespaceUri))
			{
				throw new ArgumentException(Strings.EntityPropertyMapping_EpmAttribute("targetNamespaceUri"));
			}
			this.targetNamespaceUri = targetNamespaceUri;
			Uri uri;
			if (!Uri.TryCreate(targetNamespaceUri, UriKind.Absolute, out uri))
			{
				throw new ArgumentException(Strings.EntityPropertyMapping_TargetNamespaceUriNotValid(targetNamespaceUri));
			}
			this.keepInContent = keepInContent;
		}

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06000F20 RID: 3872 RVA: 0x000361FB File Offset: 0x000343FB
		public string SourcePath
		{
			get
			{
				return this.sourcePath;
			}
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06000F21 RID: 3873 RVA: 0x00036203 File Offset: 0x00034403
		public string TargetPath
		{
			get
			{
				return this.targetPath;
			}
		}

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06000F22 RID: 3874 RVA: 0x0003620B File Offset: 0x0003440B
		public SyndicationItemProperty TargetSyndicationItem
		{
			get
			{
				return this.targetSyndicationItem;
			}
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06000F23 RID: 3875 RVA: 0x00036213 File Offset: 0x00034413
		public string TargetNamespacePrefix
		{
			get
			{
				return this.targetNamespacePrefix;
			}
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06000F24 RID: 3876 RVA: 0x0003621B File Offset: 0x0003441B
		public string TargetNamespaceUri
		{
			get
			{
				return this.targetNamespaceUri;
			}
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06000F25 RID: 3877 RVA: 0x00036223 File Offset: 0x00034423
		public SyndicationTextContentKind TargetTextContentKind
		{
			get
			{
				return this.targetTextContentKind;
			}
		}

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06000F26 RID: 3878 RVA: 0x0003622B File Offset: 0x0003442B
		public bool KeepInContent
		{
			get
			{
				return this.keepInContent;
			}
		}

		// Token: 0x04000545 RID: 1349
		private readonly string sourcePath;

		// Token: 0x04000546 RID: 1350
		private readonly string targetPath;

		// Token: 0x04000547 RID: 1351
		private readonly SyndicationItemProperty targetSyndicationItem;

		// Token: 0x04000548 RID: 1352
		private readonly SyndicationTextContentKind targetTextContentKind;

		// Token: 0x04000549 RID: 1353
		private readonly string targetNamespacePrefix;

		// Token: 0x0400054A RID: 1354
		private readonly string targetNamespaceUri;

		// Token: 0x0400054B RID: 1355
		private readonly bool keepInContent;
	}
}
