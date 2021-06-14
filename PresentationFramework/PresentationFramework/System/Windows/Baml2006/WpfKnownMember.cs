using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Markup;
using System.Xaml;
using System.Xaml.Schema;

namespace System.Windows.Baml2006
{
	// Token: 0x0200016E RID: 366
	internal class WpfKnownMember : WpfXamlMember
	{
		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x0600154D RID: 5453 RVA: 0x000697BB File Offset: 0x000679BB
		// (set) Token: 0x0600154E RID: 5454 RVA: 0x000697C9 File Offset: 0x000679C9
		private bool Frozen
		{
			get
			{
				return WpfXamlType.GetFlag(ref this._bitField, 1);
			}
			set
			{
				WpfXamlType.SetFlag(ref this._bitField, 1, value);
			}
		}

		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x0600154F RID: 5455 RVA: 0x000697D8 File Offset: 0x000679D8
		// (set) Token: 0x06001550 RID: 5456 RVA: 0x000697E6 File Offset: 0x000679E6
		private bool ReadOnly
		{
			get
			{
				return WpfXamlType.GetFlag(ref this._bitField, 4);
			}
			set
			{
				this.CheckFrozen();
				WpfXamlType.SetFlag(ref this._bitField, 4, value);
			}
		}

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x06001551 RID: 5457 RVA: 0x000697FB File Offset: 0x000679FB
		// (set) Token: 0x06001552 RID: 5458 RVA: 0x00069809 File Offset: 0x00067A09
		public bool HasSpecialTypeConverter
		{
			get
			{
				return WpfXamlType.GetFlag(ref this._bitField, 2);
			}
			set
			{
				this.CheckFrozen();
				WpfXamlType.SetFlag(ref this._bitField, 2, value);
			}
		}

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x06001553 RID: 5459 RVA: 0x0006981E File Offset: 0x00067A1E
		// (set) Token: 0x06001554 RID: 5460 RVA: 0x0006982C File Offset: 0x00067A2C
		public bool Ambient
		{
			get
			{
				return WpfXamlType.GetFlag(ref this._bitField, 8);
			}
			set
			{
				this.CheckFrozen();
				WpfXamlType.SetFlag(ref this._bitField, 8, value);
			}
		}

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x06001555 RID: 5461 RVA: 0x00069841 File Offset: 0x00067A41
		// (set) Token: 0x06001556 RID: 5462 RVA: 0x00069850 File Offset: 0x00067A50
		public bool IsReadPrivate
		{
			get
			{
				return WpfXamlType.GetFlag(ref this._bitField, 16);
			}
			set
			{
				this.CheckFrozen();
				WpfXamlType.SetFlag(ref this._bitField, 16, value);
			}
		}

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x06001557 RID: 5463 RVA: 0x00069866 File Offset: 0x00067A66
		// (set) Token: 0x06001558 RID: 5464 RVA: 0x00069875 File Offset: 0x00067A75
		public bool IsWritePrivate
		{
			get
			{
				return WpfXamlType.GetFlag(ref this._bitField, 32);
			}
			set
			{
				this.CheckFrozen();
				WpfXamlType.SetFlag(ref this._bitField, 32, value);
			}
		}

		// Token: 0x06001559 RID: 5465 RVA: 0x0006988B File Offset: 0x00067A8B
		public WpfKnownMember(XamlSchemaContext schema, XamlType declaringType, string name, DependencyProperty dProperty, bool isReadOnly, bool isAttachable) : base(dProperty, isAttachable)
		{
			base.DependencyProperty = dProperty;
			this.ReadOnly = isReadOnly;
		}

		// Token: 0x0600155A RID: 5466 RVA: 0x000698A7 File Offset: 0x00067AA7
		public WpfKnownMember(XamlSchemaContext schema, XamlType declaringType, string name, Type type, bool isReadOnly, bool isAttachable) : base(name, declaringType, isAttachable)
		{
			this._type = type;
			this.ReadOnly = isReadOnly;
		}

		// Token: 0x0600155B RID: 5467 RVA: 0x0000B02A File Offset: 0x0000922A
		protected override bool LookupIsUnknown()
		{
			return false;
		}

		// Token: 0x0600155C RID: 5468 RVA: 0x000698C3 File Offset: 0x00067AC3
		public void Freeze()
		{
			this.Frozen = true;
		}

		// Token: 0x0600155D RID: 5469 RVA: 0x000698CC File Offset: 0x00067ACC
		private void CheckFrozen()
		{
			if (this.Frozen)
			{
				throw new InvalidOperationException("Can't Assign to Known Member attributes");
			}
		}

		// Token: 0x0600155E RID: 5470 RVA: 0x000698E1 File Offset: 0x00067AE1
		protected override XamlMemberInvoker LookupInvoker()
		{
			return new WpfKnownMemberInvoker(this);
		}

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x0600155F RID: 5471 RVA: 0x000698E9 File Offset: 0x00067AE9
		// (set) Token: 0x06001560 RID: 5472 RVA: 0x000698F1 File Offset: 0x00067AF1
		public Action<object, object> SetDelegate
		{
			get
			{
				return this._setDelegate;
			}
			set
			{
				this.CheckFrozen();
				this._setDelegate = value;
			}
		}

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x06001561 RID: 5473 RVA: 0x00069900 File Offset: 0x00067B00
		// (set) Token: 0x06001562 RID: 5474 RVA: 0x00069908 File Offset: 0x00067B08
		public Func<object, object> GetDelegate
		{
			get
			{
				return this._getDelegate;
			}
			set
			{
				this.CheckFrozen();
				this._getDelegate = value;
			}
		}

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x06001563 RID: 5475 RVA: 0x00069917 File Offset: 0x00067B17
		// (set) Token: 0x06001564 RID: 5476 RVA: 0x0006991F File Offset: 0x00067B1F
		public Type TypeConverterType
		{
			get
			{
				return this._typeConverterType;
			}
			set
			{
				this.CheckFrozen();
				this._typeConverterType = value;
			}
		}

		// Token: 0x06001565 RID: 5477 RVA: 0x00069930 File Offset: 0x00067B30
		protected override XamlValueConverter<TypeConverter> LookupTypeConverter()
		{
			WpfSharedBamlSchemaContext bamlSharedSchemaContext = System.Windows.Markup.XamlReader.BamlSharedSchemaContext;
			if (this.HasSpecialTypeConverter)
			{
				return bamlSharedSchemaContext.GetXamlType(this._typeConverterType).TypeConverter;
			}
			if (this._typeConverterType != null)
			{
				return bamlSharedSchemaContext.GetTypeConverter(this._typeConverterType);
			}
			return null;
		}

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x06001566 RID: 5478 RVA: 0x00069979 File Offset: 0x00067B79
		// (set) Token: 0x06001567 RID: 5479 RVA: 0x00069981 File Offset: 0x00067B81
		public Type DeferringLoaderType
		{
			get
			{
				return this._deferringLoader;
			}
			set
			{
				this.CheckFrozen();
				this._deferringLoader = value;
			}
		}

		// Token: 0x06001568 RID: 5480 RVA: 0x00069990 File Offset: 0x00067B90
		protected override XamlValueConverter<XamlDeferringLoader> LookupDeferringLoader()
		{
			if (this._deferringLoader != null)
			{
				WpfSharedBamlSchemaContext bamlSharedSchemaContext = System.Windows.Markup.XamlReader.BamlSharedSchemaContext;
				return bamlSharedSchemaContext.GetDeferringLoader(this._deferringLoader);
			}
			return null;
		}

		// Token: 0x06001569 RID: 5481 RVA: 0x000699BF File Offset: 0x00067BBF
		protected override bool LookupIsReadOnly()
		{
			return this.ReadOnly;
		}

		// Token: 0x0600156A RID: 5482 RVA: 0x000699C7 File Offset: 0x00067BC7
		protected override XamlType LookupType()
		{
			if (base.DependencyProperty != null)
			{
				return System.Windows.Markup.XamlReader.BamlSharedSchemaContext.GetXamlType(base.DependencyProperty.PropertyType);
			}
			return System.Windows.Markup.XamlReader.BamlSharedSchemaContext.GetXamlType(this._type);
		}

		// Token: 0x0600156B RID: 5483 RVA: 0x000699F7 File Offset: 0x00067BF7
		protected override MemberInfo LookupUnderlyingMember()
		{
			return base.LookupUnderlyingMember();
		}

		// Token: 0x0600156C RID: 5484 RVA: 0x000699FF File Offset: 0x00067BFF
		protected override bool LookupIsAmbient()
		{
			return this.Ambient;
		}

		// Token: 0x0600156D RID: 5485 RVA: 0x00069A07 File Offset: 0x00067C07
		protected override bool LookupIsWritePublic()
		{
			return !this.IsWritePrivate;
		}

		// Token: 0x0600156E RID: 5486 RVA: 0x00069A12 File Offset: 0x00067C12
		protected override bool LookupIsReadPublic()
		{
			return !this.IsReadPrivate;
		}

		// Token: 0x0600156F RID: 5487 RVA: 0x0001B7E3 File Offset: 0x000199E3
		protected override WpfXamlMember GetAsContentProperty()
		{
			return this;
		}

		// Token: 0x0400124C RID: 4684
		private Action<object, object> _setDelegate;

		// Token: 0x0400124D RID: 4685
		private Func<object, object> _getDelegate;

		// Token: 0x0400124E RID: 4686
		private Type _deferringLoader;

		// Token: 0x0400124F RID: 4687
		private Type _typeConverterType;

		// Token: 0x04001250 RID: 4688
		private Type _type;

		// Token: 0x04001251 RID: 4689
		private byte _bitField;

		// Token: 0x0200084F RID: 2127
		[Flags]
		private enum BoolMemberBits
		{
			// Token: 0x04004056 RID: 16470
			Frozen = 1,
			// Token: 0x04004057 RID: 16471
			HasSpecialTypeConverter = 2,
			// Token: 0x04004058 RID: 16472
			ReadOnly = 4,
			// Token: 0x04004059 RID: 16473
			Ambient = 8,
			// Token: 0x0400405A RID: 16474
			ReadPrivate = 16,
			// Token: 0x0400405B RID: 16475
			WritePrivate = 32
		}
	}
}
