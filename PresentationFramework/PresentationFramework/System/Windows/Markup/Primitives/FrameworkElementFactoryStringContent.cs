using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace System.Windows.Markup.Primitives
{
	// Token: 0x0200028A RID: 650
	internal class FrameworkElementFactoryStringContent : ElementPropertyBase
	{
		// Token: 0x0600249A RID: 9370 RVA: 0x000B1284 File Offset: 0x000AF484
		internal FrameworkElementFactoryStringContent(FrameworkElementFactory factory, FrameworkElementFactoryMarkupObject item) : base(item.Manager)
		{
			this._item = item;
			this._factory = factory;
		}

		// Token: 0x17000909 RID: 2313
		// (get) Token: 0x0600249B RID: 9371 RVA: 0x000B1222 File Offset: 0x000AF422
		public override string Name
		{
			get
			{
				return "Content";
			}
		}

		// Token: 0x1700090A RID: 2314
		// (get) Token: 0x0600249C RID: 9372 RVA: 0x00016748 File Offset: 0x00014948
		public override bool IsContent
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700090B RID: 2315
		// (get) Token: 0x0600249D RID: 9373 RVA: 0x0000B02A File Offset: 0x0000922A
		public override bool IsComposite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700090C RID: 2316
		// (get) Token: 0x0600249E RID: 9374 RVA: 0x00016748 File Offset: 0x00014948
		public override bool IsValueAsString
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700090D RID: 2317
		// (get) Token: 0x0600249F RID: 9375 RVA: 0x000B12A0 File Offset: 0x000AF4A0
		public override IEnumerable<MarkupObject> Items
		{
			get
			{
				return new MarkupObject[0];
			}
		}

		// Token: 0x060024A0 RID: 9376 RVA: 0x000B12A8 File Offset: 0x000AF4A8
		protected override IValueSerializerContext GetItemContext()
		{
			return this._item.Context;
		}

		// Token: 0x060024A1 RID: 9377 RVA: 0x000B12B5 File Offset: 0x000AF4B5
		protected override Type GetObjectType()
		{
			return this._item.ObjectType;
		}

		// Token: 0x1700090E RID: 2318
		// (get) Token: 0x060024A2 RID: 9378 RVA: 0x000B1263 File Offset: 0x000AF463
		public override AttributeCollection Attributes
		{
			get
			{
				return new AttributeCollection(new Attribute[0]);
			}
		}

		// Token: 0x1700090F RID: 2319
		// (get) Token: 0x060024A3 RID: 9379 RVA: 0x000B087A File Offset: 0x000AEA7A
		public override Type PropertyType
		{
			get
			{
				return typeof(string);
			}
		}

		// Token: 0x17000910 RID: 2320
		// (get) Token: 0x060024A4 RID: 9380 RVA: 0x000B12C2 File Offset: 0x000AF4C2
		public override object Value
		{
			get
			{
				return this._factory.Text;
			}
		}

		// Token: 0x04001B3F RID: 6975
		private FrameworkElementFactoryMarkupObject _item;

		// Token: 0x04001B40 RID: 6976
		private FrameworkElementFactory _factory;
	}
}
