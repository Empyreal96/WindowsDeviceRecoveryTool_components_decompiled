using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Markup;
using System.Xaml;
using System.Xaml.Schema;

namespace System.Windows.Baml2006
{
	// Token: 0x02000170 RID: 368
	internal class WpfKnownType : WpfXamlType, ICustomAttributeProvider
	{
		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x06001574 RID: 5492 RVA: 0x00069BC9 File Offset: 0x00067DC9
		// (set) Token: 0x06001575 RID: 5493 RVA: 0x00069BD7 File Offset: 0x00067DD7
		private bool Frozen
		{
			get
			{
				return WpfXamlType.GetFlag(ref this._bitField, 4);
			}
			set
			{
				WpfXamlType.SetFlag(ref this._bitField, 4, value);
			}
		}

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x06001576 RID: 5494 RVA: 0x00069BE6 File Offset: 0x00067DE6
		// (set) Token: 0x06001577 RID: 5495 RVA: 0x00069BF4 File Offset: 0x00067DF4
		public bool WhitespaceSignificantCollection
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

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x06001578 RID: 5496 RVA: 0x00069C09 File Offset: 0x00067E09
		// (set) Token: 0x06001579 RID: 5497 RVA: 0x00069C18 File Offset: 0x00067E18
		public bool IsUsableDuringInit
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

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x0600157A RID: 5498 RVA: 0x00069C2E File Offset: 0x00067E2E
		// (set) Token: 0x0600157B RID: 5499 RVA: 0x00069C3D File Offset: 0x00067E3D
		public bool HasSpecialValueConverter
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

		// Token: 0x0600157C RID: 5500 RVA: 0x00069C53 File Offset: 0x00067E53
		public WpfKnownType(XamlSchemaContext schema, int bamlNumber, string name, Type underlyingType) : this(schema, bamlNumber, name, underlyingType, true, true)
		{
		}

		// Token: 0x0600157D RID: 5501 RVA: 0x00069C62 File Offset: 0x00067E62
		public WpfKnownType(XamlSchemaContext schema, int bamlNumber, string name, Type underlyingType, bool isBamlType, bool useV3Rules) : base(underlyingType, schema, isBamlType, useV3Rules)
		{
			this._bamlNumber = (short)bamlNumber;
			this._name = name;
			this._underlyingType = underlyingType;
		}

		// Token: 0x0600157E RID: 5502 RVA: 0x00069C88 File Offset: 0x00067E88
		public void Freeze()
		{
			this.Frozen = true;
		}

		// Token: 0x0600157F RID: 5503 RVA: 0x00069C91 File Offset: 0x00067E91
		private void CheckFrozen()
		{
			if (this.Frozen)
			{
				throw new InvalidOperationException("Can't Assign to Known Type attributes");
			}
		}

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x06001580 RID: 5504 RVA: 0x00069CA6 File Offset: 0x00067EA6
		public short BamlNumber
		{
			get
			{
				return this._bamlNumber;
			}
		}

		// Token: 0x06001581 RID: 5505 RVA: 0x00069CAE File Offset: 0x00067EAE
		protected override XamlMember LookupContentProperty()
		{
			return this.CallGetMember(this._contentPropertyName);
		}

		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x06001582 RID: 5506 RVA: 0x00069CBC File Offset: 0x00067EBC
		// (set) Token: 0x06001583 RID: 5507 RVA: 0x00069CC4 File Offset: 0x00067EC4
		public string ContentPropertyName
		{
			get
			{
				return this._contentPropertyName;
			}
			set
			{
				this.CheckFrozen();
				this._contentPropertyName = value;
			}
		}

		// Token: 0x06001584 RID: 5508 RVA: 0x00069CD4 File Offset: 0x00067ED4
		protected override XamlMember LookupAliasedProperty(XamlDirective directive)
		{
			if (directive == XamlLanguage.Name)
			{
				return this.CallGetMember(this._runtimeNamePropertyName);
			}
			if (directive == XamlLanguage.Key && this._dictionaryKeyPropertyName != null)
			{
				return this.LookupMember(this._dictionaryKeyPropertyName, true);
			}
			if (directive == XamlLanguage.Lang)
			{
				return this.CallGetMember(this._xmlLangPropertyName);
			}
			if (directive == XamlLanguage.Uid)
			{
				return this.CallGetMember(this._uidPropertyName);
			}
			return null;
		}

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x06001585 RID: 5509 RVA: 0x00069D53 File Offset: 0x00067F53
		// (set) Token: 0x06001586 RID: 5510 RVA: 0x00069D5B File Offset: 0x00067F5B
		public string RuntimeNamePropertyName
		{
			get
			{
				return this._runtimeNamePropertyName;
			}
			set
			{
				this.CheckFrozen();
				this._runtimeNamePropertyName = value;
			}
		}

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x06001587 RID: 5511 RVA: 0x00069D6A File Offset: 0x00067F6A
		// (set) Token: 0x06001588 RID: 5512 RVA: 0x00069D72 File Offset: 0x00067F72
		public string XmlLangPropertyName
		{
			get
			{
				return this._xmlLangPropertyName;
			}
			set
			{
				this.CheckFrozen();
				this._xmlLangPropertyName = value;
			}
		}

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x06001589 RID: 5513 RVA: 0x00069D81 File Offset: 0x00067F81
		// (set) Token: 0x0600158A RID: 5514 RVA: 0x00069D89 File Offset: 0x00067F89
		public string UidPropertyName
		{
			get
			{
				return this._uidPropertyName;
			}
			set
			{
				this.CheckFrozen();
				this._uidPropertyName = value;
			}
		}

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x0600158B RID: 5515 RVA: 0x00069D98 File Offset: 0x00067F98
		// (set) Token: 0x0600158C RID: 5516 RVA: 0x00069DA0 File Offset: 0x00067FA0
		public string DictionaryKeyPropertyName
		{
			get
			{
				return this._dictionaryKeyPropertyName;
			}
			set
			{
				this.CheckFrozen();
				this._dictionaryKeyPropertyName = value;
			}
		}

		// Token: 0x0600158D RID: 5517 RVA: 0x00069DAF File Offset: 0x00067FAF
		protected override XamlCollectionKind LookupCollectionKind()
		{
			return this._collectionKind;
		}

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x0600158E RID: 5518 RVA: 0x00069DAF File Offset: 0x00067FAF
		// (set) Token: 0x0600158F RID: 5519 RVA: 0x00069DB7 File Offset: 0x00067FB7
		public XamlCollectionKind CollectionKind
		{
			get
			{
				return this._collectionKind;
			}
			set
			{
				this.CheckFrozen();
				this._collectionKind = value;
			}
		}

		// Token: 0x06001590 RID: 5520 RVA: 0x00069DC6 File Offset: 0x00067FC6
		protected override bool LookupIsWhitespaceSignificantCollection()
		{
			return this.WhitespaceSignificantCollection;
		}

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x06001591 RID: 5521 RVA: 0x00069DCE File Offset: 0x00067FCE
		// (set) Token: 0x06001592 RID: 5522 RVA: 0x00069DD6 File Offset: 0x00067FD6
		public Func<object> DefaultConstructor
		{
			get
			{
				return this._defaultConstructor;
			}
			set
			{
				this.CheckFrozen();
				this._defaultConstructor = value;
			}
		}

		// Token: 0x06001593 RID: 5523 RVA: 0x00069DE8 File Offset: 0x00067FE8
		protected override XamlValueConverter<TypeConverter> LookupTypeConverter()
		{
			WpfSharedBamlSchemaContext bamlSharedSchemaContext = System.Windows.Markup.XamlReader.BamlSharedSchemaContext;
			if (this._typeConverterType != null)
			{
				return bamlSharedSchemaContext.GetTypeConverter(this._typeConverterType);
			}
			if (this.HasSpecialValueConverter)
			{
				return base.LookupTypeConverter();
			}
			return null;
		}

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x06001594 RID: 5524 RVA: 0x00069E26 File Offset: 0x00068026
		// (set) Token: 0x06001595 RID: 5525 RVA: 0x00069E2E File Offset: 0x0006802E
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

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x06001596 RID: 5526 RVA: 0x00069E3D File Offset: 0x0006803D
		// (set) Token: 0x06001597 RID: 5527 RVA: 0x00069E45 File Offset: 0x00068045
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

		// Token: 0x06001598 RID: 5528 RVA: 0x00069E54 File Offset: 0x00068054
		protected override XamlValueConverter<XamlDeferringLoader> LookupDeferringLoader()
		{
			if (this._deferringLoader != null)
			{
				WpfSharedBamlSchemaContext bamlSharedSchemaContext = System.Windows.Markup.XamlReader.BamlSharedSchemaContext;
				return bamlSharedSchemaContext.GetDeferringLoader(this._deferringLoader);
			}
			return null;
		}

		// Token: 0x06001599 RID: 5529 RVA: 0x00069E84 File Offset: 0x00068084
		protected override EventHandler<XamlSetMarkupExtensionEventArgs> LookupSetMarkupExtensionHandler()
		{
			if (typeof(Setter).IsAssignableFrom(this._underlyingType))
			{
				return new EventHandler<XamlSetMarkupExtensionEventArgs>(Setter.ReceiveMarkupExtension);
			}
			if (typeof(DataTrigger).IsAssignableFrom(this._underlyingType))
			{
				return new EventHandler<XamlSetMarkupExtensionEventArgs>(DataTrigger.ReceiveMarkupExtension);
			}
			if (typeof(Condition).IsAssignableFrom(this._underlyingType))
			{
				return new EventHandler<XamlSetMarkupExtensionEventArgs>(Condition.ReceiveMarkupExtension);
			}
			return null;
		}

		// Token: 0x0600159A RID: 5530 RVA: 0x00069F00 File Offset: 0x00068100
		protected override EventHandler<XamlSetTypeConverterEventArgs> LookupSetTypeConverterHandler()
		{
			if (typeof(Setter).IsAssignableFrom(this._underlyingType))
			{
				return new EventHandler<XamlSetTypeConverterEventArgs>(Setter.ReceiveTypeConverter);
			}
			if (typeof(Trigger).IsAssignableFrom(this._underlyingType))
			{
				return new EventHandler<XamlSetTypeConverterEventArgs>(Trigger.ReceiveTypeConverter);
			}
			if (typeof(Condition).IsAssignableFrom(this._underlyingType))
			{
				return new EventHandler<XamlSetTypeConverterEventArgs>(Condition.ReceiveTypeConverter);
			}
			return null;
		}

		// Token: 0x0600159B RID: 5531 RVA: 0x00069F7A File Offset: 0x0006817A
		protected override bool LookupUsableDuringInitialization()
		{
			return this.IsUsableDuringInit;
		}

		// Token: 0x0600159C RID: 5532 RVA: 0x00069F82 File Offset: 0x00068182
		protected override XamlTypeInvoker LookupInvoker()
		{
			return new WpfKnownTypeInvoker(this);
		}

		// Token: 0x0600159D RID: 5533 RVA: 0x00069F8A File Offset: 0x0006818A
		private XamlMember CallGetMember(string name)
		{
			if (name != null)
			{
				return base.GetMember(name);
			}
			return null;
		}

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x0600159E RID: 5534 RVA: 0x00069F98 File Offset: 0x00068198
		public Dictionary<int, Baml6ConstructorInfo> Constructors
		{
			get
			{
				if (this._constructors == null)
				{
					this._constructors = new Dictionary<int, Baml6ConstructorInfo>();
				}
				return this._constructors;
			}
		}

		// Token: 0x0600159F RID: 5535 RVA: 0x00069FB4 File Offset: 0x000681B4
		protected override IList<XamlType> LookupPositionalParameters(int paramCount)
		{
			if (base.IsMarkupExtension)
			{
				List<XamlType> list = null;
				Baml6ConstructorInfo baml6ConstructorInfo = this.Constructors[paramCount];
				if (this.Constructors.TryGetValue(paramCount, out baml6ConstructorInfo))
				{
					list = new List<XamlType>();
					foreach (Type type in baml6ConstructorInfo.Types)
					{
						list.Add(base.SchemaContext.GetXamlType(type));
					}
				}
				return list;
			}
			return base.LookupPositionalParameters(paramCount);
		}

		// Token: 0x060015A0 RID: 5536 RVA: 0x0001B7E3 File Offset: 0x000199E3
		protected override ICustomAttributeProvider LookupCustomAttributeProvider()
		{
			return this;
		}

		// Token: 0x060015A1 RID: 5537 RVA: 0x0006A04C File Offset: 0x0006824C
		object[] ICustomAttributeProvider.GetCustomAttributes(bool inherit)
		{
			return base.UnderlyingType.GetCustomAttributes(inherit);
		}

		// Token: 0x060015A2 RID: 5538 RVA: 0x0006A05C File Offset: 0x0006825C
		object[] ICustomAttributeProvider.GetCustomAttributes(Type attributeType, bool inherit)
		{
			Attribute attribute;
			if (!this.TryGetCustomAttribute(attributeType, out attribute))
			{
				return base.UnderlyingType.GetCustomAttributes(attributeType, inherit);
			}
			if (attribute != null)
			{
				return new Attribute[]
				{
					attribute
				};
			}
			if (WpfKnownType.s_EmptyAttributes == null)
			{
				WpfKnownType.s_EmptyAttributes = new Attribute[0];
			}
			return WpfKnownType.s_EmptyAttributes;
		}

		// Token: 0x060015A3 RID: 5539 RVA: 0x0006A0A8 File Offset: 0x000682A8
		private bool TryGetCustomAttribute(Type attributeType, out Attribute result)
		{
			bool result2 = true;
			if (attributeType == typeof(ContentPropertyAttribute))
			{
				result = ((this._contentPropertyName == null) ? null : new ContentPropertyAttribute(this._contentPropertyName));
			}
			else if (attributeType == typeof(RuntimeNamePropertyAttribute))
			{
				result = ((this._runtimeNamePropertyName == null) ? null : new RuntimeNamePropertyAttribute(this._runtimeNamePropertyName));
			}
			else if (attributeType == typeof(DictionaryKeyPropertyAttribute))
			{
				result = ((this._dictionaryKeyPropertyName == null) ? null : new DictionaryKeyPropertyAttribute(this._dictionaryKeyPropertyName));
			}
			else if (attributeType == typeof(XmlLangPropertyAttribute))
			{
				result = ((this._xmlLangPropertyName == null) ? null : new XmlLangPropertyAttribute(this._xmlLangPropertyName));
			}
			else if (attributeType == typeof(UidPropertyAttribute))
			{
				result = ((this._uidPropertyName == null) ? null : new UidPropertyAttribute(this._uidPropertyName));
			}
			else
			{
				result = null;
				result2 = false;
			}
			return result2;
		}

		// Token: 0x060015A4 RID: 5540 RVA: 0x0006A19F File Offset: 0x0006839F
		bool ICustomAttributeProvider.IsDefined(Type attributeType, bool inherit)
		{
			return base.UnderlyingType.IsDefined(attributeType, inherit);
		}

		// Token: 0x04001255 RID: 4693
		private static Attribute[] s_EmptyAttributes;

		// Token: 0x04001256 RID: 4694
		private short _bamlNumber;

		// Token: 0x04001257 RID: 4695
		private string _name;

		// Token: 0x04001258 RID: 4696
		private Type _underlyingType;

		// Token: 0x04001259 RID: 4697
		private string _contentPropertyName;

		// Token: 0x0400125A RID: 4698
		private string _runtimeNamePropertyName;

		// Token: 0x0400125B RID: 4699
		private string _dictionaryKeyPropertyName;

		// Token: 0x0400125C RID: 4700
		private string _xmlLangPropertyName;

		// Token: 0x0400125D RID: 4701
		private string _uidPropertyName;

		// Token: 0x0400125E RID: 4702
		private Func<object> _defaultConstructor;

		// Token: 0x0400125F RID: 4703
		private Type _deferringLoader;

		// Token: 0x04001260 RID: 4704
		private Type _typeConverterType;

		// Token: 0x04001261 RID: 4705
		private XamlCollectionKind _collectionKind;

		// Token: 0x04001262 RID: 4706
		private Dictionary<int, Baml6ConstructorInfo> _constructors;

		// Token: 0x02000850 RID: 2128
		[Flags]
		private enum BoolTypeBits
		{
			// Token: 0x0400405D RID: 16477
			Frozen = 4,
			// Token: 0x0400405E RID: 16478
			WhitespaceSignificantCollection = 8,
			// Token: 0x0400405F RID: 16479
			UsableDurintInit = 16,
			// Token: 0x04004060 RID: 16480
			HasSpecialValueConverter = 32
		}
	}
}
