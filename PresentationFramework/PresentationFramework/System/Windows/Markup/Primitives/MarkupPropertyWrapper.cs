using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace System.Windows.Markup.Primitives
{
	// Token: 0x02000284 RID: 644
	internal class MarkupPropertyWrapper : MarkupProperty
	{
		// Token: 0x06002462 RID: 9314 RVA: 0x000B0B03 File Offset: 0x000AED03
		public MarkupPropertyWrapper(MarkupProperty baseProperty)
		{
			this._baseProperty = baseProperty;
		}

		// Token: 0x170008E4 RID: 2276
		// (get) Token: 0x06002463 RID: 9315 RVA: 0x000B0B12 File Offset: 0x000AED12
		public override AttributeCollection Attributes
		{
			get
			{
				return this._baseProperty.Attributes;
			}
		}

		// Token: 0x170008E5 RID: 2277
		// (get) Token: 0x06002464 RID: 9316 RVA: 0x000B0B1F File Offset: 0x000AED1F
		public override IEnumerable<MarkupObject> Items
		{
			get
			{
				return this._baseProperty.Items;
			}
		}

		// Token: 0x170008E6 RID: 2278
		// (get) Token: 0x06002465 RID: 9317 RVA: 0x000B0B2C File Offset: 0x000AED2C
		public override string Name
		{
			get
			{
				return this._baseProperty.Name;
			}
		}

		// Token: 0x170008E7 RID: 2279
		// (get) Token: 0x06002466 RID: 9318 RVA: 0x000B0B39 File Offset: 0x000AED39
		public override Type PropertyType
		{
			get
			{
				return this._baseProperty.PropertyType;
			}
		}

		// Token: 0x170008E8 RID: 2280
		// (get) Token: 0x06002467 RID: 9319 RVA: 0x000B0B46 File Offset: 0x000AED46
		public override string StringValue
		{
			get
			{
				return this._baseProperty.StringValue;
			}
		}

		// Token: 0x170008E9 RID: 2281
		// (get) Token: 0x06002468 RID: 9320 RVA: 0x000B0B53 File Offset: 0x000AED53
		public override IEnumerable<Type> TypeReferences
		{
			get
			{
				return this._baseProperty.TypeReferences;
			}
		}

		// Token: 0x170008EA RID: 2282
		// (get) Token: 0x06002469 RID: 9321 RVA: 0x000B0B60 File Offset: 0x000AED60
		public override object Value
		{
			get
			{
				return this._baseProperty.Value;
			}
		}

		// Token: 0x170008EB RID: 2283
		// (get) Token: 0x0600246A RID: 9322 RVA: 0x000B0B6D File Offset: 0x000AED6D
		public override DependencyProperty DependencyProperty
		{
			get
			{
				return this._baseProperty.DependencyProperty;
			}
		}

		// Token: 0x170008EC RID: 2284
		// (get) Token: 0x0600246B RID: 9323 RVA: 0x000B0B7A File Offset: 0x000AED7A
		public override bool IsAttached
		{
			get
			{
				return this._baseProperty.IsAttached;
			}
		}

		// Token: 0x170008ED RID: 2285
		// (get) Token: 0x0600246C RID: 9324 RVA: 0x000B0B87 File Offset: 0x000AED87
		public override bool IsComposite
		{
			get
			{
				return this._baseProperty.IsComposite;
			}
		}

		// Token: 0x170008EE RID: 2286
		// (get) Token: 0x0600246D RID: 9325 RVA: 0x000B0B94 File Offset: 0x000AED94
		public override bool IsConstructorArgument
		{
			get
			{
				return this._baseProperty.IsConstructorArgument;
			}
		}

		// Token: 0x170008EF RID: 2287
		// (get) Token: 0x0600246E RID: 9326 RVA: 0x000B0BA1 File Offset: 0x000AEDA1
		public override bool IsKey
		{
			get
			{
				return this._baseProperty.IsKey;
			}
		}

		// Token: 0x170008F0 RID: 2288
		// (get) Token: 0x0600246F RID: 9327 RVA: 0x000B0BAE File Offset: 0x000AEDAE
		public override bool IsValueAsString
		{
			get
			{
				return this._baseProperty.IsValueAsString;
			}
		}

		// Token: 0x170008F1 RID: 2289
		// (get) Token: 0x06002470 RID: 9328 RVA: 0x000B0BBB File Offset: 0x000AEDBB
		public override bool IsContent
		{
			get
			{
				return this._baseProperty.IsContent;
			}
		}

		// Token: 0x170008F2 RID: 2290
		// (get) Token: 0x06002471 RID: 9329 RVA: 0x000B0BC8 File Offset: 0x000AEDC8
		public override PropertyDescriptor PropertyDescriptor
		{
			get
			{
				return this._baseProperty.PropertyDescriptor;
			}
		}

		// Token: 0x06002472 RID: 9330 RVA: 0x000B0BD5 File Offset: 0x000AEDD5
		internal override void VerifyOnlySerializableTypes()
		{
			this._baseProperty.VerifyOnlySerializableTypes();
		}

		// Token: 0x04001B32 RID: 6962
		private MarkupProperty _baseProperty;
	}
}
