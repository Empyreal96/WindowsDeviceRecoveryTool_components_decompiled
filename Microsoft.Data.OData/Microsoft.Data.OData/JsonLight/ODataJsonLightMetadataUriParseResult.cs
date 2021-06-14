using System;
using System.Collections.Generic;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.JsonLight
{
	// Token: 0x0200016B RID: 363
	internal sealed class ODataJsonLightMetadataUriParseResult
	{
		// Token: 0x06000A2D RID: 2605 RVA: 0x0002118F File Offset: 0x0001F38F
		internal ODataJsonLightMetadataUriParseResult(Uri metadataUriFromPayload)
		{
			this.metadataUriFromPayload = metadataUriFromPayload;
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000A2E RID: 2606 RVA: 0x0002119E File Offset: 0x0001F39E
		internal Uri MetadataUri
		{
			get
			{
				return this.metadataUriFromPayload;
			}
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06000A2F RID: 2607 RVA: 0x000211A6 File Offset: 0x0001F3A6
		// (set) Token: 0x06000A30 RID: 2608 RVA: 0x000211AE File Offset: 0x0001F3AE
		internal Uri MetadataDocumentUri
		{
			get
			{
				return this.metadataDocumentUri;
			}
			set
			{
				this.metadataDocumentUri = value;
			}
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06000A31 RID: 2609 RVA: 0x000211B7 File Offset: 0x0001F3B7
		// (set) Token: 0x06000A32 RID: 2610 RVA: 0x000211BF File Offset: 0x0001F3BF
		internal string Fragment
		{
			get
			{
				return this.fragment;
			}
			set
			{
				this.fragment = value;
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06000A33 RID: 2611 RVA: 0x000211C8 File Offset: 0x0001F3C8
		// (set) Token: 0x06000A34 RID: 2612 RVA: 0x000211D0 File Offset: 0x0001F3D0
		internal string SelectQueryOption
		{
			get
			{
				return this.selectQueryOption;
			}
			set
			{
				this.selectQueryOption = value;
			}
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06000A35 RID: 2613 RVA: 0x000211D9 File Offset: 0x0001F3D9
		// (set) Token: 0x06000A36 RID: 2614 RVA: 0x000211E1 File Offset: 0x0001F3E1
		internal IEdmEntitySet EntitySet
		{
			get
			{
				return this.entitySet;
			}
			set
			{
				this.entitySet = value;
			}
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000A37 RID: 2615 RVA: 0x000211EA File Offset: 0x0001F3EA
		// (set) Token: 0x06000A38 RID: 2616 RVA: 0x000211F2 File Offset: 0x0001F3F2
		internal IEdmType EdmType
		{
			get
			{
				return this.edmType;
			}
			set
			{
				this.edmType = value;
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06000A39 RID: 2617 RVA: 0x000211FB File Offset: 0x0001F3FB
		// (set) Token: 0x06000A3A RID: 2618 RVA: 0x00021203 File Offset: 0x0001F403
		internal IEdmNavigationProperty NavigationProperty
		{
			get
			{
				return this.navigationProperty;
			}
			set
			{
				this.navigationProperty = value;
			}
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000A3B RID: 2619 RVA: 0x0002120C File Offset: 0x0001F40C
		// (set) Token: 0x06000A3C RID: 2620 RVA: 0x00021214 File Offset: 0x0001F414
		internal IEnumerable<ODataPayloadKind> DetectedPayloadKinds
		{
			get
			{
				return this.detectedPayloadKinds;
			}
			set
			{
				this.detectedPayloadKinds = value;
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06000A3D RID: 2621 RVA: 0x0002121D File Offset: 0x0001F41D
		// (set) Token: 0x06000A3E RID: 2622 RVA: 0x00021225 File Offset: 0x0001F425
		internal bool IsNullProperty
		{
			get
			{
				return this.isNullProperty;
			}
			set
			{
				this.isNullProperty = value;
			}
		}

		// Token: 0x040003BC RID: 956
		private readonly Uri metadataUriFromPayload;

		// Token: 0x040003BD RID: 957
		private Uri metadataDocumentUri;

		// Token: 0x040003BE RID: 958
		private string fragment;

		// Token: 0x040003BF RID: 959
		private string selectQueryOption;

		// Token: 0x040003C0 RID: 960
		private IEdmEntitySet entitySet;

		// Token: 0x040003C1 RID: 961
		private IEdmType edmType;

		// Token: 0x040003C2 RID: 962
		private IEdmNavigationProperty navigationProperty;

		// Token: 0x040003C3 RID: 963
		private IEnumerable<ODataPayloadKind> detectedPayloadKinds;

		// Token: 0x040003C4 RID: 964
		private bool isNullProperty;
	}
}
