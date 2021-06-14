using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Converters;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Xaml;
using System.Xaml.Schema;
using MS.Internal.PresentationFramework;
using MS.Internal.WindowsBase;

namespace System.Windows.Baml2006
{
	// Token: 0x0200015F RID: 351
	internal class Baml2006SchemaContext : XamlSchemaContext
	{
		// Token: 0x06000FA6 RID: 4006 RVA: 0x0003CA75 File Offset: 0x0003AC75
		public Baml2006SchemaContext(Assembly localAssembly) : this(localAssembly, System.Windows.Markup.XamlReader.BamlSharedSchemaContext)
		{
		}

		// Token: 0x06000FA7 RID: 4007 RVA: 0x0003CA84 File Offset: 0x0003AC84
		internal Baml2006SchemaContext(Assembly localAssembly, XamlSchemaContext parentSchemaContext) : base(new Assembly[0])
		{
			this._localAssembly = localAssembly;
			this._parentSchemaContext = parentSchemaContext;
		}

		// Token: 0x06000FA8 RID: 4008 RVA: 0x0003CAED File Offset: 0x0003ACED
		public override bool TryGetCompatibleXamlNamespace(string xamlNamespace, out string compatibleNamespace)
		{
			return this._parentSchemaContext.TryGetCompatibleXamlNamespace(xamlNamespace, out compatibleNamespace);
		}

		// Token: 0x06000FA9 RID: 4009 RVA: 0x0003CAFC File Offset: 0x0003ACFC
		public override XamlDirective GetXamlDirective(string xamlNamespace, string name)
		{
			return this._parentSchemaContext.GetXamlDirective(xamlNamespace, name);
		}

		// Token: 0x06000FAA RID: 4010 RVA: 0x0003CB0B File Offset: 0x0003AD0B
		public override IEnumerable<string> GetAllXamlNamespaces()
		{
			return this._parentSchemaContext.GetAllXamlNamespaces();
		}

		// Token: 0x06000FAB RID: 4011 RVA: 0x0003CB18 File Offset: 0x0003AD18
		public override ICollection<XamlType> GetAllXamlTypes(string xamlNamespace)
		{
			return this._parentSchemaContext.GetAllXamlTypes(xamlNamespace);
		}

		// Token: 0x06000FAC RID: 4012 RVA: 0x0003CB26 File Offset: 0x0003AD26
		public override string GetPreferredPrefix(string xmlns)
		{
			return this._parentSchemaContext.GetPreferredPrefix(xmlns);
		}

		// Token: 0x06000FAD RID: 4013 RVA: 0x0003CB34 File Offset: 0x0003AD34
		public override XamlType GetXamlType(Type type)
		{
			return this._parentSchemaContext.GetXamlType(type);
		}

		// Token: 0x06000FAE RID: 4014 RVA: 0x0003CB44 File Offset: 0x0003AD44
		protected override XamlType GetXamlType(string xamlNamespace, string name, params XamlType[] typeArguments)
		{
			this.EnsureXmlnsAssembliesLoaded(xamlNamespace);
			XamlTypeName xamlTypeName = new XamlTypeName
			{
				Namespace = xamlNamespace,
				Name = name
			};
			if (typeArguments != null)
			{
				foreach (XamlType xamlType in typeArguments)
				{
					xamlTypeName.TypeArguments.Add(new XamlTypeName(xamlType));
				}
			}
			return this._parentSchemaContext.GetXamlType(xamlTypeName);
		}

		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x06000FAF RID: 4015 RVA: 0x0003CBA0 File Offset: 0x0003ADA0
		internal XamlMember StaticExtensionMemberTypeProperty
		{
			get
			{
				return Baml2006SchemaContext._xStaticMemberProperty.Value;
			}
		}

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x06000FB0 RID: 4016 RVA: 0x0003CBAC File Offset: 0x0003ADAC
		internal XamlMember TypeExtensionTypeProperty
		{
			get
			{
				return Baml2006SchemaContext._xTypeTypeProperty.Value;
			}
		}

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x06000FB1 RID: 4017 RVA: 0x0003CBB8 File Offset: 0x0003ADB8
		internal XamlMember ResourceDictionaryDeferredContentProperty
		{
			get
			{
				return Baml2006SchemaContext._resourceDictionaryDefContentProperty.Value;
			}
		}

		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x06000FB2 RID: 4018 RVA: 0x0003CBC4 File Offset: 0x0003ADC4
		internal XamlType ResourceDictionaryType
		{
			get
			{
				return Baml2006SchemaContext._resourceDictionaryType.Value;
			}
		}

		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x06000FB3 RID: 4019 RVA: 0x0003CBD0 File Offset: 0x0003ADD0
		internal XamlType EventSetterType
		{
			get
			{
				return Baml2006SchemaContext._eventSetterType.Value;
			}
		}

		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x06000FB4 RID: 4020 RVA: 0x0003CBDC File Offset: 0x0003ADDC
		internal XamlMember EventSetterEventProperty
		{
			get
			{
				return Baml2006SchemaContext._eventSetterEventProperty.Value;
			}
		}

		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x06000FB5 RID: 4021 RVA: 0x0003CBE8 File Offset: 0x0003ADE8
		internal XamlMember EventSetterHandlerProperty
		{
			get
			{
				return Baml2006SchemaContext._eventSetterHandlerProperty.Value;
			}
		}

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x06000FB6 RID: 4022 RVA: 0x0003CBF4 File Offset: 0x0003ADF4
		internal XamlMember FrameworkTemplateTemplateProperty
		{
			get
			{
				return Baml2006SchemaContext._frameworkTemplateTemplateProperty.Value;
			}
		}

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x06000FB7 RID: 4023 RVA: 0x0003CC00 File Offset: 0x0003AE00
		internal XamlType StaticResourceExtensionType
		{
			get
			{
				return Baml2006SchemaContext._staticResourceExtensionType.Value;
			}
		}

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x06000FB8 RID: 4024 RVA: 0x0003CC0C File Offset: 0x0003AE0C
		internal Assembly LocalAssembly
		{
			get
			{
				return this._localAssembly;
			}
		}

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x06000FB9 RID: 4025 RVA: 0x0003CC14 File Offset: 0x0003AE14
		// (set) Token: 0x06000FBA RID: 4026 RVA: 0x0003CC1C File Offset: 0x0003AE1C
		internal Baml2006ReaderSettings Settings { get; set; }

		// Token: 0x06000FBB RID: 4027 RVA: 0x0003CC28 File Offset: 0x0003AE28
		internal void Reset()
		{
			object syncObject = this._syncObject;
			lock (syncObject)
			{
				this._bamlAssembly.Clear();
				this._bamlType.Clear();
				this._bamlProperty.Clear();
				this._bamlString.Clear();
				this._bamlXmlnsMappings.Clear();
			}
		}

		// Token: 0x06000FBC RID: 4028 RVA: 0x0003CC9C File Offset: 0x0003AE9C
		internal Assembly GetAssembly(short assemblyId)
		{
			Baml2006SchemaContext.BamlAssembly bamlAssembly;
			if (this.TryGetBamlAssembly(assemblyId, out bamlAssembly))
			{
				return this.ResolveAssembly(bamlAssembly);
			}
			throw new KeyNotFoundException();
		}

		// Token: 0x06000FBD RID: 4029 RVA: 0x0003CCC4 File Offset: 0x0003AEC4
		internal string GetAssemblyName(short assemblyId)
		{
			Baml2006SchemaContext.BamlAssembly bamlAssembly;
			if (this.TryGetBamlAssembly(assemblyId, out bamlAssembly))
			{
				return bamlAssembly.Name;
			}
			throw new KeyNotFoundException(SR.Get("BamlAssemblyIdNotFound", new object[]
			{
				assemblyId.ToString(TypeConverterHelper.InvariantEnglishUS)
			}));
		}

		// Token: 0x06000FBE RID: 4030 RVA: 0x0003CD08 File Offset: 0x0003AF08
		internal Type GetClrType(short typeId)
		{
			Baml2006SchemaContext.BamlType bamlType;
			XamlType xamlType;
			if (!this.TryGetBamlType(typeId, out bamlType, out xamlType))
			{
				throw new KeyNotFoundException(SR.Get("BamlTypeIdNotFound", new object[]
				{
					typeId.ToString(TypeConverterHelper.InvariantEnglishUS)
				}));
			}
			if (xamlType != null)
			{
				return xamlType.UnderlyingType;
			}
			return this.ResolveBamlTypeToType(bamlType);
		}

		// Token: 0x06000FBF RID: 4031 RVA: 0x0003CD60 File Offset: 0x0003AF60
		internal XamlType GetXamlType(short typeId)
		{
			Baml2006SchemaContext.BamlType bamlType;
			XamlType xamlType;
			if (!this.TryGetBamlType(typeId, out bamlType, out xamlType))
			{
				throw new KeyNotFoundException(SR.Get("BamlTypeIdNotFound", new object[]
				{
					typeId.ToString(TypeConverterHelper.InvariantEnglishUS)
				}));
			}
			if (xamlType != null)
			{
				return xamlType;
			}
			return this.ResolveBamlType(bamlType, typeId);
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x0003CDB4 File Offset: 0x0003AFB4
		internal DependencyProperty GetDependencyProperty(short propertyId)
		{
			XamlMember property = this.GetProperty(propertyId, false);
			WpfXamlMember wpfXamlMember = property as WpfXamlMember;
			if (wpfXamlMember != null)
			{
				return wpfXamlMember.DependencyProperty;
			}
			throw new KeyNotFoundException();
		}

		// Token: 0x06000FC1 RID: 4033 RVA: 0x0003CDE8 File Offset: 0x0003AFE8
		internal XamlMember GetProperty(short propertyId, XamlType parentType)
		{
			Baml2006SchemaContext.BamlProperty bamlProperty;
			XamlMember xamlMember;
			if (!this.TryGetBamlProperty(propertyId, out bamlProperty, out xamlMember))
			{
				throw new KeyNotFoundException();
			}
			if (xamlMember != null)
			{
				return xamlMember;
			}
			XamlType xamlType = this.GetXamlType(bamlProperty.DeclaringTypeId);
			if (parentType.CanAssignTo(xamlType))
			{
				xamlMember = xamlType.GetMember(bamlProperty.Name);
				if (xamlMember == null)
				{
					xamlMember = xamlType.GetAttachableMember(bamlProperty.Name);
				}
			}
			else
			{
				xamlMember = xamlType.GetAttachableMember(bamlProperty.Name);
			}
			object syncObject = this._syncObject;
			lock (syncObject)
			{
				this._bamlProperty[(int)propertyId] = xamlMember;
			}
			return xamlMember;
		}

		// Token: 0x06000FC2 RID: 4034 RVA: 0x0003CE9C File Offset: 0x0003B09C
		internal XamlMember GetProperty(short propertyId, bool isAttached)
		{
			Baml2006SchemaContext.BamlProperty bamlProperty;
			XamlMember xamlMember;
			if (!this.TryGetBamlProperty(propertyId, out bamlProperty, out xamlMember))
			{
				throw new KeyNotFoundException();
			}
			XamlType xamlType;
			if (xamlMember != null)
			{
				if (xamlMember.IsAttachable != isAttached)
				{
					xamlType = xamlMember.DeclaringType;
					if (isAttached)
					{
						xamlMember = xamlType.GetAttachableMember(xamlMember.Name);
					}
					else
					{
						xamlMember = xamlType.GetMember(xamlMember.Name);
					}
				}
				return xamlMember;
			}
			xamlType = this.GetXamlType(bamlProperty.DeclaringTypeId);
			if (isAttached)
			{
				xamlMember = xamlType.GetAttachableMember(bamlProperty.Name);
			}
			else
			{
				xamlMember = xamlType.GetMember(bamlProperty.Name);
			}
			object syncObject = this._syncObject;
			lock (syncObject)
			{
				this._bamlProperty[(int)propertyId] = xamlMember;
			}
			return xamlMember;
		}

		// Token: 0x06000FC3 RID: 4035 RVA: 0x0003CF64 File Offset: 0x0003B164
		internal XamlType GetPropertyDeclaringType(short propertyId)
		{
			Baml2006SchemaContext.BamlProperty bamlProperty;
			XamlMember xamlMember;
			if (!this.TryGetBamlProperty(propertyId, out bamlProperty, out xamlMember))
			{
				throw new KeyNotFoundException();
			}
			if (xamlMember != null)
			{
				return xamlMember.DeclaringType;
			}
			return this.GetXamlType(bamlProperty.DeclaringTypeId);
		}

		// Token: 0x06000FC4 RID: 4036 RVA: 0x0003CFA0 File Offset: 0x0003B1A0
		internal string GetPropertyName(short propertyId, bool fullName)
		{
			Baml2006SchemaContext.BamlProperty bamlProperty = null;
			XamlMember xamlMember;
			if (!this.TryGetBamlProperty(propertyId, out bamlProperty, out xamlMember))
			{
				throw new KeyNotFoundException();
			}
			if (xamlMember != null)
			{
				return xamlMember.Name;
			}
			return bamlProperty.Name;
		}

		// Token: 0x06000FC5 RID: 4037 RVA: 0x0003CFD8 File Offset: 0x0003B1D8
		internal string GetString(short stringId)
		{
			object syncObject = this._syncObject;
			string text;
			lock (syncObject)
			{
				if (stringId >= 0 && (int)stringId < this._bamlString.Count)
				{
					text = this._bamlString[(int)stringId];
				}
				else
				{
					text = Baml2006SchemaContext.KnownTypes.GetKnownString(stringId);
				}
			}
			if (text == null)
			{
				throw new KeyNotFoundException();
			}
			return text;
		}

		// Token: 0x06000FC6 RID: 4038 RVA: 0x0003D044 File Offset: 0x0003B244
		internal void AddAssembly(short assemblyId, string assemblyName)
		{
			if (assemblyId < 0)
			{
				throw new ArgumentOutOfRangeException("assemblyId");
			}
			if (assemblyName == null)
			{
				throw new ArgumentNullException("assemblyName");
			}
			List<Baml2006SchemaContext.BamlAssembly> bamlAssembly = this._bamlAssembly;
			lock (bamlAssembly)
			{
				if ((int)assemblyId == this._bamlAssembly.Count)
				{
					Baml2006SchemaContext.BamlAssembly item = new Baml2006SchemaContext.BamlAssembly(assemblyName);
					this._bamlAssembly.Add(item);
				}
				else if ((int)assemblyId > this._bamlAssembly.Count)
				{
					throw new ArgumentOutOfRangeException("assemblyId", SR.Get("AssemblyIdOutOfSequence", new object[]
					{
						assemblyId
					}));
				}
			}
		}

		// Token: 0x06000FC7 RID: 4039 RVA: 0x0003D0F0 File Offset: 0x0003B2F0
		internal void AddXamlType(short typeId, short assemblyId, string typeName, Baml2006SchemaContext.TypeInfoFlags flags)
		{
			if (typeId < 0)
			{
				throw new ArgumentOutOfRangeException("typeId");
			}
			if (typeName == null)
			{
				throw new ArgumentNullException("typeName");
			}
			object syncObject = this._syncObject;
			lock (syncObject)
			{
				if ((int)typeId == this._bamlType.Count)
				{
					Baml2006SchemaContext.BamlType bamlType = new Baml2006SchemaContext.BamlType(assemblyId, typeName);
					bamlType.Flags = flags;
					this._bamlType.Add(bamlType);
				}
				else if ((int)typeId > this._bamlType.Count)
				{
					throw new ArgumentOutOfRangeException("typeId", SR.Get("TypeIdOutOfSequence", new object[]
					{
						typeId
					}));
				}
			}
		}

		// Token: 0x06000FC8 RID: 4040 RVA: 0x0003D1A8 File Offset: 0x0003B3A8
		internal void AddProperty(short propertyId, short declaringTypeId, string propertyName)
		{
			if (propertyId < 0)
			{
				throw new ArgumentOutOfRangeException("propertyId");
			}
			if (propertyName == null)
			{
				throw new ArgumentNullException("propertyName");
			}
			object syncObject = this._syncObject;
			lock (syncObject)
			{
				if ((int)propertyId == this._bamlProperty.Count)
				{
					Baml2006SchemaContext.BamlProperty item = new Baml2006SchemaContext.BamlProperty(declaringTypeId, propertyName);
					this._bamlProperty.Add(item);
				}
				else if ((int)propertyId > this._bamlProperty.Count)
				{
					throw new ArgumentOutOfRangeException("propertyId", SR.Get("PropertyIdOutOfSequence", new object[]
					{
						propertyId
					}));
				}
			}
		}

		// Token: 0x06000FC9 RID: 4041 RVA: 0x0003D258 File Offset: 0x0003B458
		internal void AddString(short stringId, string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			object syncObject = this._syncObject;
			lock (syncObject)
			{
				if ((int)stringId == this._bamlString.Count)
				{
					this._bamlString.Add(value);
				}
				else if ((int)stringId > this._bamlString.Count)
				{
					throw new ArgumentOutOfRangeException("stringId", SR.Get("StringIdOutOfSequence", new object[]
					{
						stringId
					}));
				}
			}
		}

		// Token: 0x06000FCA RID: 4042 RVA: 0x0003D2F0 File Offset: 0x0003B4F0
		internal void AddXmlnsMapping(string xmlns, short[] assemblies)
		{
			object syncObject = this._syncObject;
			lock (syncObject)
			{
				this._bamlXmlnsMappings[xmlns] = assemblies;
			}
		}

		// Token: 0x06000FCB RID: 4043 RVA: 0x0003D338 File Offset: 0x0003B538
		private void EnsureXmlnsAssembliesLoaded(string xamlNamespace)
		{
			object syncObject = this._syncObject;
			short[] array;
			lock (syncObject)
			{
				this._bamlXmlnsMappings.TryGetValue(xamlNamespace, out array);
			}
			if (array != null)
			{
				foreach (short assemblyId in array)
				{
					this.GetAssembly(assemblyId);
				}
			}
		}

		// Token: 0x06000FCC RID: 4044 RVA: 0x0003D3A8 File Offset: 0x0003B5A8
		private Assembly ResolveAssembly(Baml2006SchemaContext.BamlAssembly bamlAssembly)
		{
			if (bamlAssembly.Assembly != null)
			{
				return bamlAssembly.Assembly;
			}
			AssemblyName assemblyName = new AssemblyName(bamlAssembly.Name);
			bamlAssembly.Assembly = SafeSecurityHelper.GetLoadedAssembly(assemblyName);
			if (bamlAssembly.Assembly == null)
			{
				byte[] publicKeyToken = assemblyName.GetPublicKeyToken();
				if (assemblyName.Version != null || assemblyName.CultureInfo != null || publicKeyToken != null)
				{
					try
					{
						bamlAssembly.Assembly = Assembly.Load(assemblyName.FullName);
						goto IL_D3;
					}
					catch
					{
						if (bamlAssembly.Assembly == null)
						{
							if (this.MatchesLocalAssembly(assemblyName.Name, publicKeyToken))
							{
								bamlAssembly.Assembly = this._localAssembly;
							}
							else
							{
								AssemblyName assemblyName2 = new AssemblyName(assemblyName.Name);
								if (publicKeyToken != null)
								{
									assemblyName2.SetPublicKeyToken(publicKeyToken);
								}
								bamlAssembly.Assembly = Assembly.Load(assemblyName2);
							}
						}
						goto IL_D3;
					}
				}
				bamlAssembly.Assembly = Assembly.LoadWithPartialName(assemblyName.Name);
			}
			IL_D3:
			return bamlAssembly.Assembly;
		}

		// Token: 0x06000FCD RID: 4045 RVA: 0x0003D4A0 File Offset: 0x0003B6A0
		private bool MatchesLocalAssembly(string shortName, byte[] publicKeyToken)
		{
			if (this._localAssembly == null)
			{
				return false;
			}
			AssemblyName assemblyName = new AssemblyName(this._localAssembly.FullName);
			return !(shortName != assemblyName.Name) && (publicKeyToken == null || SafeSecurityHelper.IsSameKeyToken(publicKeyToken, assemblyName.GetPublicKeyToken()));
		}

		// Token: 0x06000FCE RID: 4046 RVA: 0x0003D4F0 File Offset: 0x0003B6F0
		private Type ResolveBamlTypeToType(Baml2006SchemaContext.BamlType bamlType)
		{
			Baml2006SchemaContext.BamlAssembly bamlAssembly;
			if (this.TryGetBamlAssembly(bamlType.AssemblyId, out bamlAssembly))
			{
				Assembly assembly = this.ResolveAssembly(bamlAssembly);
				if (assembly != null)
				{
					return assembly.GetType(bamlType.Name, false);
				}
			}
			return null;
		}

		// Token: 0x06000FCF RID: 4047 RVA: 0x0003D530 File Offset: 0x0003B730
		private XamlType ResolveBamlType(Baml2006SchemaContext.BamlType bamlType, short typeId)
		{
			Type type = this.ResolveBamlTypeToType(bamlType);
			if (type != null)
			{
				bamlType.ClrNamespace = type.Namespace;
				XamlType xamlType = this._parentSchemaContext.GetXamlType(type);
				object syncObject = this._syncObject;
				lock (syncObject)
				{
					this._bamlType[(int)typeId] = xamlType;
				}
				return xamlType;
			}
			throw new NotImplementedException();
		}

		// Token: 0x06000FD0 RID: 4048 RVA: 0x0003D5A8 File Offset: 0x0003B7A8
		private bool TryGetBamlAssembly(short assemblyId, out Baml2006SchemaContext.BamlAssembly bamlAssembly)
		{
			object syncObject = this._syncObject;
			lock (syncObject)
			{
				if (assemblyId >= 0 && (int)assemblyId < this._bamlAssembly.Count)
				{
					bamlAssembly = this._bamlAssembly[(int)assemblyId];
					return true;
				}
			}
			Assembly knownAssembly = Baml2006SchemaContext.KnownTypes.GetKnownAssembly(assemblyId);
			if (knownAssembly != null)
			{
				bamlAssembly = new Baml2006SchemaContext.BamlAssembly(knownAssembly);
				return true;
			}
			bamlAssembly = null;
			return false;
		}

		// Token: 0x06000FD1 RID: 4049 RVA: 0x0003D628 File Offset: 0x0003B828
		private bool TryGetBamlType(short typeId, out Baml2006SchemaContext.BamlType bamlType, out XamlType xamlType)
		{
			bamlType = null;
			xamlType = null;
			object syncObject = this._syncObject;
			lock (syncObject)
			{
				if (typeId >= 0 && (int)typeId < this._bamlType.Count)
				{
					object obj = this._bamlType[(int)typeId];
					bamlType = (obj as Baml2006SchemaContext.BamlType);
					xamlType = (obj as XamlType);
					return obj != null;
				}
			}
			if (typeId < 0)
			{
				if (this._parentSchemaContext == System.Windows.Markup.XamlReader.BamlSharedSchemaContext)
				{
					xamlType = System.Windows.Markup.XamlReader.BamlSharedSchemaContext.GetKnownBamlType(typeId);
				}
				else
				{
					xamlType = this._parentSchemaContext.GetXamlType(Baml2006SchemaContext.KnownTypes.GetKnownType(typeId));
				}
				return true;
			}
			return bamlType != null;
		}

		// Token: 0x06000FD2 RID: 4050 RVA: 0x0003D6DC File Offset: 0x0003B8DC
		private bool TryGetBamlProperty(short propertyId, out Baml2006SchemaContext.BamlProperty bamlProperty, out XamlMember xamlMember)
		{
			object syncObject = this._syncObject;
			lock (syncObject)
			{
				if (propertyId >= 0 && (int)propertyId < this._bamlProperty.Count)
				{
					object obj = this._bamlProperty[(int)propertyId];
					xamlMember = (obj as XamlMember);
					bamlProperty = (obj as Baml2006SchemaContext.BamlProperty);
					return true;
				}
			}
			if (propertyId < 0)
			{
				if (this._parentSchemaContext == System.Windows.Markup.XamlReader.BamlSharedSchemaContext)
				{
					xamlMember = System.Windows.Markup.XamlReader.BamlSharedSchemaContext.GetKnownBamlMember(propertyId);
				}
				else
				{
					short typeId;
					string name;
					Baml2006SchemaContext.KnownTypes.GetKnownProperty(propertyId, out typeId, out name);
					xamlMember = this.GetXamlType(typeId).GetMember(name);
				}
				bamlProperty = null;
				return true;
			}
			xamlMember = null;
			bamlProperty = null;
			return false;
		}

		// Token: 0x040011AB RID: 4523
		internal const short StaticExtensionTypeId = 602;

		// Token: 0x040011AC RID: 4524
		internal const short StaticResourceTypeId = 603;

		// Token: 0x040011AD RID: 4525
		internal const short DynamicResourceTypeId = 189;

		// Token: 0x040011AE RID: 4526
		internal const short TemplateBindingTypeId = 634;

		// Token: 0x040011AF RID: 4527
		internal const short TypeExtensionTypeId = 691;

		// Token: 0x040011B0 RID: 4528
		internal const string WpfNamespace = "http://schemas.microsoft.com/winfx/2006/xaml/presentation";

		// Token: 0x040011B1 RID: 4529
		private readonly List<Baml2006SchemaContext.BamlAssembly> _bamlAssembly = new List<Baml2006SchemaContext.BamlAssembly>();

		// Token: 0x040011B2 RID: 4530
		private readonly List<object> _bamlType = new List<object>();

		// Token: 0x040011B3 RID: 4531
		private readonly List<object> _bamlProperty = new List<object>();

		// Token: 0x040011B4 RID: 4532
		private readonly List<string> _bamlString = new List<string>();

		// Token: 0x040011B5 RID: 4533
		private readonly Dictionary<string, short[]> _bamlXmlnsMappings = new Dictionary<string, short[]>();

		// Token: 0x040011B6 RID: 4534
		private static readonly Lazy<XamlMember> _xStaticMemberProperty = new Lazy<XamlMember>(() => XamlLanguage.Static.GetMember("MemberType"));

		// Token: 0x040011B7 RID: 4535
		private static readonly Lazy<XamlMember> _xTypeTypeProperty = new Lazy<XamlMember>(() => XamlLanguage.Static.GetMember("Type"));

		// Token: 0x040011B8 RID: 4536
		private static readonly Lazy<XamlMember> _resourceDictionaryDefContentProperty = new Lazy<XamlMember>(() => Baml2006SchemaContext._resourceDictionaryType.Value.GetMember("DeferrableContent"));

		// Token: 0x040011B9 RID: 4537
		private static readonly Lazy<XamlType> _resourceDictionaryType = new Lazy<XamlType>(() => System.Windows.Markup.XamlReader.BamlSharedSchemaContext.GetXamlType(typeof(ResourceDictionary)));

		// Token: 0x040011BA RID: 4538
		private static readonly Lazy<XamlType> _eventSetterType = new Lazy<XamlType>(() => System.Windows.Markup.XamlReader.BamlSharedSchemaContext.GetXamlType(typeof(EventSetter)));

		// Token: 0x040011BB RID: 4539
		private static readonly Lazy<XamlMember> _eventSetterEventProperty = new Lazy<XamlMember>(() => Baml2006SchemaContext._eventSetterType.Value.GetMember("Event"));

		// Token: 0x040011BC RID: 4540
		private static readonly Lazy<XamlMember> _eventSetterHandlerProperty = new Lazy<XamlMember>(() => Baml2006SchemaContext._eventSetterType.Value.GetMember("Handler"));

		// Token: 0x040011BD RID: 4541
		private static readonly Lazy<XamlMember> _frameworkTemplateTemplateProperty = new Lazy<XamlMember>(() => System.Windows.Markup.XamlReader.BamlSharedSchemaContext.GetXamlType(typeof(FrameworkTemplate)).GetMember("Template"));

		// Token: 0x040011BE RID: 4542
		private static readonly Lazy<XamlType> _staticResourceExtensionType = new Lazy<XamlType>(() => System.Windows.Markup.XamlReader.BamlSharedSchemaContext.GetXamlType(typeof(StaticResourceExtension)));

		// Token: 0x040011BF RID: 4543
		private object _syncObject = new object();

		// Token: 0x040011C0 RID: 4544
		private Assembly _localAssembly;

		// Token: 0x040011C1 RID: 4545
		private XamlSchemaContext _parentSchemaContext;

		// Token: 0x02000843 RID: 2115
		// (Invoke) Token: 0x06007F17 RID: 32535
		private delegate Type LazyTypeOf();

		// Token: 0x02000844 RID: 2116
		internal static class KnownTypes
		{
			// Token: 0x06007F1A RID: 32538 RVA: 0x002374C4 File Offset: 0x002356C4
			public static Type GetAttachableTargetType(short propertyId)
			{
				switch (propertyId)
				{
				case -64:
					return typeof(UIElement);
				case -63:
					return typeof(UIElement);
				case -62:
					return typeof(UIElement);
				case -61:
					return typeof(UIElement);
				default:
					if (propertyId == -39)
					{
						return typeof(UIElement);
					}
					return typeof(DependencyObject);
				}
			}

			// Token: 0x06007F1B RID: 32539 RVA: 0x00237530 File Offset: 0x00235730
			public static Assembly GetKnownAssembly(short assemblyId)
			{
				Assembly result;
				switch (-assemblyId)
				{
				case 0:
					result = typeof(double).Assembly;
					break;
				case 1:
					result = typeof(Uri).Assembly;
					break;
				case 2:
					result = typeof(DependencyObject).Assembly;
					break;
				case 3:
					result = typeof(UIElement).Assembly;
					break;
				case 4:
					result = typeof(FrameworkElement).Assembly;
					break;
				default:
					result = null;
					break;
				}
				return result;
			}

			// Token: 0x06007F1C RID: 32540 RVA: 0x002375BC File Offset: 0x002357BC
			public static Type GetKnownType(short typeId)
			{
				typeId = -typeId;
				Baml2006SchemaContext.LazyTypeOf lazyTypeOf;
				switch (typeId)
				{
				case 1:
					lazyTypeOf = (() => typeof(AccessText));
					break;
				case 2:
					lazyTypeOf = (() => typeof(AdornedElementPlaceholder));
					break;
				case 3:
					lazyTypeOf = (() => typeof(Adorner));
					break;
				case 4:
					lazyTypeOf = (() => typeof(AdornerDecorator));
					break;
				case 5:
					lazyTypeOf = (() => typeof(AdornerLayer));
					break;
				case 6:
					lazyTypeOf = (() => typeof(AffineTransform3D));
					break;
				case 7:
					lazyTypeOf = (() => typeof(AmbientLight));
					break;
				case 8:
					lazyTypeOf = (() => typeof(AnchoredBlock));
					break;
				case 9:
					lazyTypeOf = (() => typeof(Animatable));
					break;
				case 10:
					lazyTypeOf = (() => typeof(AnimationClock));
					break;
				case 11:
					lazyTypeOf = (() => typeof(AnimationTimeline));
					break;
				case 12:
					lazyTypeOf = (() => typeof(Application));
					break;
				case 13:
					lazyTypeOf = (() => typeof(ArcSegment));
					break;
				case 14:
					lazyTypeOf = (() => typeof(ArrayExtension));
					break;
				case 15:
					lazyTypeOf = (() => typeof(AxisAngleRotation3D));
					break;
				case 16:
					lazyTypeOf = (() => typeof(BaseIListConverter));
					break;
				case 17:
					lazyTypeOf = (() => typeof(BeginStoryboard));
					break;
				case 18:
					lazyTypeOf = (() => typeof(BevelBitmapEffect));
					break;
				case 19:
					lazyTypeOf = (() => typeof(BezierSegment));
					break;
				case 20:
					lazyTypeOf = (() => typeof(Binding));
					break;
				case 21:
					lazyTypeOf = (() => typeof(BindingBase));
					break;
				case 22:
					lazyTypeOf = (() => typeof(BindingExpression));
					break;
				case 23:
					lazyTypeOf = (() => typeof(BindingExpressionBase));
					break;
				case 24:
					lazyTypeOf = (() => typeof(BindingListCollectionView));
					break;
				case 25:
					lazyTypeOf = (() => typeof(BitmapDecoder));
					break;
				case 26:
					lazyTypeOf = (() => typeof(BitmapEffect));
					break;
				case 27:
					lazyTypeOf = (() => typeof(BitmapEffectCollection));
					break;
				case 28:
					lazyTypeOf = (() => typeof(BitmapEffectGroup));
					break;
				case 29:
					lazyTypeOf = (() => typeof(BitmapEffectInput));
					break;
				case 30:
					lazyTypeOf = (() => typeof(BitmapEncoder));
					break;
				case 31:
					lazyTypeOf = (() => typeof(BitmapFrame));
					break;
				case 32:
					lazyTypeOf = (() => typeof(BitmapImage));
					break;
				case 33:
					lazyTypeOf = (() => typeof(BitmapMetadata));
					break;
				case 34:
					lazyTypeOf = (() => typeof(BitmapPalette));
					break;
				case 35:
					lazyTypeOf = (() => typeof(BitmapSource));
					break;
				case 36:
					lazyTypeOf = (() => typeof(Block));
					break;
				case 37:
					lazyTypeOf = (() => typeof(BlockUIContainer));
					break;
				case 38:
					lazyTypeOf = (() => typeof(BlurBitmapEffect));
					break;
				case 39:
					lazyTypeOf = (() => typeof(BmpBitmapDecoder));
					break;
				case 40:
					lazyTypeOf = (() => typeof(BmpBitmapEncoder));
					break;
				case 41:
					lazyTypeOf = (() => typeof(Bold));
					break;
				case 42:
					lazyTypeOf = (() => typeof(BoolIListConverter));
					break;
				case 43:
					lazyTypeOf = (() => typeof(bool));
					break;
				case 44:
					lazyTypeOf = (() => typeof(BooleanAnimationBase));
					break;
				case 45:
					lazyTypeOf = (() => typeof(BooleanAnimationUsingKeyFrames));
					break;
				case 46:
					lazyTypeOf = (() => typeof(BooleanConverter));
					break;
				case 47:
					lazyTypeOf = (() => typeof(BooleanKeyFrame));
					break;
				case 48:
					lazyTypeOf = (() => typeof(BooleanKeyFrameCollection));
					break;
				case 49:
					lazyTypeOf = (() => typeof(BooleanToVisibilityConverter));
					break;
				case 50:
					lazyTypeOf = (() => typeof(Border));
					break;
				case 51:
					lazyTypeOf = (() => typeof(BorderGapMaskConverter));
					break;
				case 52:
					lazyTypeOf = (() => typeof(Brush));
					break;
				case 53:
					lazyTypeOf = (() => typeof(BrushConverter));
					break;
				case 54:
					lazyTypeOf = (() => typeof(BulletDecorator));
					break;
				case 55:
					lazyTypeOf = (() => typeof(Button));
					break;
				case 56:
					lazyTypeOf = (() => typeof(ButtonBase));
					break;
				case 57:
					lazyTypeOf = (() => typeof(byte));
					break;
				case 58:
					lazyTypeOf = (() => typeof(ByteAnimation));
					break;
				case 59:
					lazyTypeOf = (() => typeof(ByteAnimationBase));
					break;
				case 60:
					lazyTypeOf = (() => typeof(ByteAnimationUsingKeyFrames));
					break;
				case 61:
					lazyTypeOf = (() => typeof(ByteConverter));
					break;
				case 62:
					lazyTypeOf = (() => typeof(ByteKeyFrame));
					break;
				case 63:
					lazyTypeOf = (() => typeof(ByteKeyFrameCollection));
					break;
				case 64:
					lazyTypeOf = (() => typeof(CachedBitmap));
					break;
				case 65:
					lazyTypeOf = (() => typeof(Camera));
					break;
				case 66:
					lazyTypeOf = (() => typeof(Canvas));
					break;
				case 67:
					lazyTypeOf = (() => typeof(char));
					break;
				case 68:
					lazyTypeOf = (() => typeof(CharAnimationBase));
					break;
				case 69:
					lazyTypeOf = (() => typeof(CharAnimationUsingKeyFrames));
					break;
				case 70:
					lazyTypeOf = (() => typeof(CharConverter));
					break;
				case 71:
					lazyTypeOf = (() => typeof(CharIListConverter));
					break;
				case 72:
					lazyTypeOf = (() => typeof(CharKeyFrame));
					break;
				case 73:
					lazyTypeOf = (() => typeof(CharKeyFrameCollection));
					break;
				case 74:
					lazyTypeOf = (() => typeof(CheckBox));
					break;
				case 75:
					lazyTypeOf = (() => typeof(Clock));
					break;
				case 76:
					lazyTypeOf = (() => typeof(ClockController));
					break;
				case 77:
					lazyTypeOf = (() => typeof(ClockGroup));
					break;
				case 78:
					lazyTypeOf = (() => typeof(CollectionContainer));
					break;
				case 79:
					lazyTypeOf = (() => typeof(CollectionView));
					break;
				case 80:
					lazyTypeOf = (() => typeof(CollectionViewSource));
					break;
				case 81:
					lazyTypeOf = (() => typeof(Color));
					break;
				case 82:
					lazyTypeOf = (() => typeof(ColorAnimation));
					break;
				case 83:
					lazyTypeOf = (() => typeof(ColorAnimationBase));
					break;
				case 84:
					lazyTypeOf = (() => typeof(ColorAnimationUsingKeyFrames));
					break;
				case 85:
					lazyTypeOf = (() => typeof(ColorConvertedBitmap));
					break;
				case 86:
					lazyTypeOf = (() => typeof(ColorConvertedBitmapExtension));
					break;
				case 87:
					lazyTypeOf = (() => typeof(ColorConverter));
					break;
				case 88:
					lazyTypeOf = (() => typeof(ColorKeyFrame));
					break;
				case 89:
					lazyTypeOf = (() => typeof(ColorKeyFrameCollection));
					break;
				case 90:
					lazyTypeOf = (() => typeof(ColumnDefinition));
					break;
				case 91:
					lazyTypeOf = (() => typeof(CombinedGeometry));
					break;
				case 92:
					lazyTypeOf = (() => typeof(ComboBox));
					break;
				case 93:
					lazyTypeOf = (() => typeof(ComboBoxItem));
					break;
				case 94:
					lazyTypeOf = (() => typeof(CommandConverter));
					break;
				case 95:
					lazyTypeOf = (() => typeof(ComponentResourceKey));
					break;
				case 96:
					lazyTypeOf = (() => typeof(ComponentResourceKeyConverter));
					break;
				case 97:
					lazyTypeOf = (() => typeof(CompositionTarget));
					break;
				case 98:
					lazyTypeOf = (() => typeof(Condition));
					break;
				case 99:
					lazyTypeOf = (() => typeof(ContainerVisual));
					break;
				case 100:
					lazyTypeOf = (() => typeof(ContentControl));
					break;
				case 101:
					lazyTypeOf = (() => typeof(ContentElement));
					break;
				case 102:
					lazyTypeOf = (() => typeof(ContentPresenter));
					break;
				case 103:
					lazyTypeOf = (() => typeof(ContentPropertyAttribute));
					break;
				case 104:
					lazyTypeOf = (() => typeof(ContentWrapperAttribute));
					break;
				case 105:
					lazyTypeOf = (() => typeof(ContextMenu));
					break;
				case 106:
					lazyTypeOf = (() => typeof(ContextMenuService));
					break;
				case 107:
					lazyTypeOf = (() => typeof(Control));
					break;
				case 108:
					lazyTypeOf = (() => typeof(ControlTemplate));
					break;
				case 109:
					lazyTypeOf = (() => typeof(ControllableStoryboardAction));
					break;
				case 110:
					lazyTypeOf = (() => typeof(CornerRadius));
					break;
				case 111:
					lazyTypeOf = (() => typeof(CornerRadiusConverter));
					break;
				case 112:
					lazyTypeOf = (() => typeof(CroppedBitmap));
					break;
				case 113:
					lazyTypeOf = (() => typeof(CultureInfo));
					break;
				case 114:
					lazyTypeOf = (() => typeof(CultureInfoConverter));
					break;
				case 115:
					lazyTypeOf = (() => typeof(CultureInfoIetfLanguageTagConverter));
					break;
				case 116:
					lazyTypeOf = (() => typeof(Cursor));
					break;
				case 117:
					lazyTypeOf = (() => typeof(CursorConverter));
					break;
				case 118:
					lazyTypeOf = (() => typeof(DashStyle));
					break;
				case 119:
					lazyTypeOf = (() => typeof(DataChangedEventManager));
					break;
				case 120:
					lazyTypeOf = (() => typeof(DataTemplate));
					break;
				case 121:
					lazyTypeOf = (() => typeof(DataTemplateKey));
					break;
				case 122:
					lazyTypeOf = (() => typeof(DataTrigger));
					break;
				case 123:
					lazyTypeOf = (() => typeof(DateTime));
					break;
				case 124:
					lazyTypeOf = (() => typeof(DateTimeConverter));
					break;
				case 125:
					lazyTypeOf = (() => typeof(DateTimeConverter2));
					break;
				case 126:
					lazyTypeOf = (() => typeof(decimal));
					break;
				case 127:
					lazyTypeOf = (() => typeof(DecimalAnimation));
					break;
				case 128:
					lazyTypeOf = (() => typeof(DecimalAnimationBase));
					break;
				case 129:
					lazyTypeOf = (() => typeof(DecimalAnimationUsingKeyFrames));
					break;
				case 130:
					lazyTypeOf = (() => typeof(DecimalConverter));
					break;
				case 131:
					lazyTypeOf = (() => typeof(DecimalKeyFrame));
					break;
				case 132:
					lazyTypeOf = (() => typeof(DecimalKeyFrameCollection));
					break;
				case 133:
					lazyTypeOf = (() => typeof(Decorator));
					break;
				case 134:
					lazyTypeOf = (() => typeof(DefinitionBase));
					break;
				case 135:
					lazyTypeOf = (() => typeof(DependencyObject));
					break;
				case 136:
					lazyTypeOf = (() => typeof(DependencyProperty));
					break;
				case 137:
					lazyTypeOf = (() => typeof(DependencyPropertyConverter));
					break;
				case 138:
					lazyTypeOf = (() => typeof(DialogResultConverter));
					break;
				case 139:
					lazyTypeOf = (() => typeof(DiffuseMaterial));
					break;
				case 140:
					lazyTypeOf = (() => typeof(DirectionalLight));
					break;
				case 141:
					lazyTypeOf = (() => typeof(DiscreteBooleanKeyFrame));
					break;
				case 142:
					lazyTypeOf = (() => typeof(DiscreteByteKeyFrame));
					break;
				case 143:
					lazyTypeOf = (() => typeof(DiscreteCharKeyFrame));
					break;
				case 144:
					lazyTypeOf = (() => typeof(DiscreteColorKeyFrame));
					break;
				case 145:
					lazyTypeOf = (() => typeof(DiscreteDecimalKeyFrame));
					break;
				case 146:
					lazyTypeOf = (() => typeof(DiscreteDoubleKeyFrame));
					break;
				case 147:
					lazyTypeOf = (() => typeof(DiscreteInt16KeyFrame));
					break;
				case 148:
					lazyTypeOf = (() => typeof(DiscreteInt32KeyFrame));
					break;
				case 149:
					lazyTypeOf = (() => typeof(DiscreteInt64KeyFrame));
					break;
				case 150:
					lazyTypeOf = (() => typeof(DiscreteMatrixKeyFrame));
					break;
				case 151:
					lazyTypeOf = (() => typeof(DiscreteObjectKeyFrame));
					break;
				case 152:
					lazyTypeOf = (() => typeof(DiscretePoint3DKeyFrame));
					break;
				case 153:
					lazyTypeOf = (() => typeof(DiscretePointKeyFrame));
					break;
				case 154:
					lazyTypeOf = (() => typeof(DiscreteQuaternionKeyFrame));
					break;
				case 155:
					lazyTypeOf = (() => typeof(DiscreteRectKeyFrame));
					break;
				case 156:
					lazyTypeOf = (() => typeof(DiscreteRotation3DKeyFrame));
					break;
				case 157:
					lazyTypeOf = (() => typeof(DiscreteSingleKeyFrame));
					break;
				case 158:
					lazyTypeOf = (() => typeof(DiscreteSizeKeyFrame));
					break;
				case 159:
					lazyTypeOf = (() => typeof(DiscreteStringKeyFrame));
					break;
				case 160:
					lazyTypeOf = (() => typeof(DiscreteThicknessKeyFrame));
					break;
				case 161:
					lazyTypeOf = (() => typeof(DiscreteVector3DKeyFrame));
					break;
				case 162:
					lazyTypeOf = (() => typeof(DiscreteVectorKeyFrame));
					break;
				case 163:
					lazyTypeOf = (() => typeof(DockPanel));
					break;
				case 164:
					lazyTypeOf = (() => typeof(DocumentPageView));
					break;
				case 165:
					lazyTypeOf = (() => typeof(DocumentReference));
					break;
				case 166:
					lazyTypeOf = (() => typeof(DocumentViewer));
					break;
				case 167:
					lazyTypeOf = (() => typeof(DocumentViewerBase));
					break;
				case 168:
					lazyTypeOf = (() => typeof(double));
					break;
				case 169:
					lazyTypeOf = (() => typeof(DoubleAnimation));
					break;
				case 170:
					lazyTypeOf = (() => typeof(DoubleAnimationBase));
					break;
				case 171:
					lazyTypeOf = (() => typeof(DoubleAnimationUsingKeyFrames));
					break;
				case 172:
					lazyTypeOf = (() => typeof(DoubleAnimationUsingPath));
					break;
				case 173:
					lazyTypeOf = (() => typeof(DoubleCollection));
					break;
				case 174:
					lazyTypeOf = (() => typeof(DoubleCollectionConverter));
					break;
				case 175:
					lazyTypeOf = (() => typeof(DoubleConverter));
					break;
				case 176:
					lazyTypeOf = (() => typeof(DoubleIListConverter));
					break;
				case 177:
					lazyTypeOf = (() => typeof(DoubleKeyFrame));
					break;
				case 178:
					lazyTypeOf = (() => typeof(DoubleKeyFrameCollection));
					break;
				case 179:
					lazyTypeOf = (() => typeof(Drawing));
					break;
				case 180:
					lazyTypeOf = (() => typeof(DrawingBrush));
					break;
				case 181:
					lazyTypeOf = (() => typeof(DrawingCollection));
					break;
				case 182:
					lazyTypeOf = (() => typeof(DrawingContext));
					break;
				case 183:
					lazyTypeOf = (() => typeof(DrawingGroup));
					break;
				case 184:
					lazyTypeOf = (() => typeof(DrawingImage));
					break;
				case 185:
					lazyTypeOf = (() => typeof(DrawingVisual));
					break;
				case 186:
					lazyTypeOf = (() => typeof(DropShadowBitmapEffect));
					break;
				case 187:
					lazyTypeOf = (() => typeof(Duration));
					break;
				case 188:
					lazyTypeOf = (() => typeof(DurationConverter));
					break;
				case 189:
					lazyTypeOf = (() => typeof(DynamicResourceExtension));
					break;
				case 190:
					lazyTypeOf = (() => typeof(DynamicResourceExtensionConverter));
					break;
				case 191:
					lazyTypeOf = (() => typeof(Ellipse));
					break;
				case 192:
					lazyTypeOf = (() => typeof(EllipseGeometry));
					break;
				case 193:
					lazyTypeOf = (() => typeof(EmbossBitmapEffect));
					break;
				case 194:
					lazyTypeOf = (() => typeof(EmissiveMaterial));
					break;
				case 195:
					lazyTypeOf = (() => typeof(EnumConverter));
					break;
				case 196:
					lazyTypeOf = (() => typeof(EventManager));
					break;
				case 197:
					lazyTypeOf = (() => typeof(EventSetter));
					break;
				case 198:
					lazyTypeOf = (() => typeof(EventTrigger));
					break;
				case 199:
					lazyTypeOf = (() => typeof(Expander));
					break;
				case 200:
					lazyTypeOf = (() => typeof(Expression));
					break;
				case 201:
					lazyTypeOf = (() => typeof(ExpressionConverter));
					break;
				case 202:
					lazyTypeOf = (() => typeof(Figure));
					break;
				case 203:
					lazyTypeOf = (() => typeof(FigureLength));
					break;
				case 204:
					lazyTypeOf = (() => typeof(FigureLengthConverter));
					break;
				case 205:
					lazyTypeOf = (() => typeof(FixedDocument));
					break;
				case 206:
					lazyTypeOf = (() => typeof(FixedDocumentSequence));
					break;
				case 207:
					lazyTypeOf = (() => typeof(FixedPage));
					break;
				case 208:
					lazyTypeOf = (() => typeof(Floater));
					break;
				case 209:
					lazyTypeOf = (() => typeof(FlowDocument));
					break;
				case 210:
					lazyTypeOf = (() => typeof(FlowDocumentPageViewer));
					break;
				case 211:
					lazyTypeOf = (() => typeof(FlowDocumentReader));
					break;
				case 212:
					lazyTypeOf = (() => typeof(FlowDocumentScrollViewer));
					break;
				case 213:
					lazyTypeOf = (() => typeof(FocusManager));
					break;
				case 214:
					lazyTypeOf = (() => typeof(FontFamily));
					break;
				case 215:
					lazyTypeOf = (() => typeof(FontFamilyConverter));
					break;
				case 216:
					lazyTypeOf = (() => typeof(FontSizeConverter));
					break;
				case 217:
					lazyTypeOf = (() => typeof(FontStretch));
					break;
				case 218:
					lazyTypeOf = (() => typeof(FontStretchConverter));
					break;
				case 219:
					lazyTypeOf = (() => typeof(FontStyle));
					break;
				case 220:
					lazyTypeOf = (() => typeof(FontStyleConverter));
					break;
				case 221:
					lazyTypeOf = (() => typeof(FontWeight));
					break;
				case 222:
					lazyTypeOf = (() => typeof(FontWeightConverter));
					break;
				case 223:
					lazyTypeOf = (() => typeof(FormatConvertedBitmap));
					break;
				case 224:
					lazyTypeOf = (() => typeof(Frame));
					break;
				case 225:
					lazyTypeOf = (() => typeof(FrameworkContentElement));
					break;
				case 226:
					lazyTypeOf = (() => typeof(FrameworkElement));
					break;
				case 227:
					lazyTypeOf = (() => typeof(FrameworkElementFactory));
					break;
				case 228:
					lazyTypeOf = (() => typeof(FrameworkPropertyMetadata));
					break;
				case 229:
					lazyTypeOf = (() => typeof(FrameworkPropertyMetadataOptions));
					break;
				case 230:
					lazyTypeOf = (() => typeof(FrameworkRichTextComposition));
					break;
				case 231:
					lazyTypeOf = (() => typeof(FrameworkTemplate));
					break;
				case 232:
					lazyTypeOf = (() => typeof(FrameworkTextComposition));
					break;
				case 233:
					lazyTypeOf = (() => typeof(Freezable));
					break;
				case 234:
					lazyTypeOf = (() => typeof(GeneralTransform));
					break;
				case 235:
					lazyTypeOf = (() => typeof(GeneralTransformCollection));
					break;
				case 236:
					lazyTypeOf = (() => typeof(GeneralTransformGroup));
					break;
				case 237:
					lazyTypeOf = (() => typeof(Geometry));
					break;
				case 238:
					lazyTypeOf = (() => typeof(Geometry3D));
					break;
				case 239:
					lazyTypeOf = (() => typeof(GeometryCollection));
					break;
				case 240:
					lazyTypeOf = (() => typeof(GeometryConverter));
					break;
				case 241:
					lazyTypeOf = (() => typeof(GeometryDrawing));
					break;
				case 242:
					lazyTypeOf = (() => typeof(GeometryGroup));
					break;
				case 243:
					lazyTypeOf = (() => typeof(GeometryModel3D));
					break;
				case 244:
					lazyTypeOf = (() => typeof(GestureRecognizer));
					break;
				case 245:
					lazyTypeOf = (() => typeof(GifBitmapDecoder));
					break;
				case 246:
					lazyTypeOf = (() => typeof(GifBitmapEncoder));
					break;
				case 247:
					lazyTypeOf = (() => typeof(GlyphRun));
					break;
				case 248:
					lazyTypeOf = (() => typeof(GlyphRunDrawing));
					break;
				case 249:
					lazyTypeOf = (() => typeof(GlyphTypeface));
					break;
				case 250:
					lazyTypeOf = (() => typeof(Glyphs));
					break;
				case 251:
					lazyTypeOf = (() => typeof(GradientBrush));
					break;
				case 252:
					lazyTypeOf = (() => typeof(GradientStop));
					break;
				case 253:
					lazyTypeOf = (() => typeof(GradientStopCollection));
					break;
				case 254:
					lazyTypeOf = (() => typeof(Grid));
					break;
				case 255:
					lazyTypeOf = (() => typeof(GridLength));
					break;
				case 256:
					lazyTypeOf = (() => typeof(GridLengthConverter));
					break;
				case 257:
					lazyTypeOf = (() => typeof(GridSplitter));
					break;
				case 258:
					lazyTypeOf = (() => typeof(GridView));
					break;
				case 259:
					lazyTypeOf = (() => typeof(GridViewColumn));
					break;
				case 260:
					lazyTypeOf = (() => typeof(GridViewColumnHeader));
					break;
				case 261:
					lazyTypeOf = (() => typeof(GridViewHeaderRowPresenter));
					break;
				case 262:
					lazyTypeOf = (() => typeof(GridViewRowPresenter));
					break;
				case 263:
					lazyTypeOf = (() => typeof(GridViewRowPresenterBase));
					break;
				case 264:
					lazyTypeOf = (() => typeof(GroupBox));
					break;
				case 265:
					lazyTypeOf = (() => typeof(GroupItem));
					break;
				case 266:
					lazyTypeOf = (() => typeof(Guid));
					break;
				case 267:
					lazyTypeOf = (() => typeof(GuidConverter));
					break;
				case 268:
					lazyTypeOf = (() => typeof(GuidelineSet));
					break;
				case 269:
					lazyTypeOf = (() => typeof(HeaderedContentControl));
					break;
				case 270:
					lazyTypeOf = (() => typeof(HeaderedItemsControl));
					break;
				case 271:
					lazyTypeOf = (() => typeof(HierarchicalDataTemplate));
					break;
				case 272:
					lazyTypeOf = (() => typeof(HostVisual));
					break;
				case 273:
					lazyTypeOf = (() => typeof(Hyperlink));
					break;
				case 274:
					lazyTypeOf = (() => typeof(IAddChild));
					break;
				case 275:
					lazyTypeOf = (() => typeof(IAddChildInternal));
					break;
				case 276:
					lazyTypeOf = (() => typeof(ICommand));
					break;
				case 277:
					lazyTypeOf = (() => typeof(IComponentConnector));
					break;
				case 278:
					lazyTypeOf = (() => typeof(INameScope));
					break;
				case 279:
					lazyTypeOf = (() => typeof(IStyleConnector));
					break;
				case 280:
					lazyTypeOf = (() => typeof(IconBitmapDecoder));
					break;
				case 281:
					lazyTypeOf = (() => typeof(Image));
					break;
				case 282:
					lazyTypeOf = (() => typeof(ImageBrush));
					break;
				case 283:
					lazyTypeOf = (() => typeof(ImageDrawing));
					break;
				case 284:
					lazyTypeOf = (() => typeof(ImageMetadata));
					break;
				case 285:
					lazyTypeOf = (() => typeof(ImageSource));
					break;
				case 286:
					lazyTypeOf = (() => typeof(ImageSourceConverter));
					break;
				case 287:
					lazyTypeOf = (() => typeof(InPlaceBitmapMetadataWriter));
					break;
				case 288:
					lazyTypeOf = (() => typeof(InkCanvas));
					break;
				case 289:
					lazyTypeOf = (() => typeof(InkPresenter));
					break;
				case 290:
					lazyTypeOf = (() => typeof(Inline));
					break;
				case 291:
					lazyTypeOf = (() => typeof(InlineCollection));
					break;
				case 292:
					lazyTypeOf = (() => typeof(InlineUIContainer));
					break;
				case 293:
					lazyTypeOf = (() => typeof(InputBinding));
					break;
				case 294:
					lazyTypeOf = (() => typeof(InputDevice));
					break;
				case 295:
					lazyTypeOf = (() => typeof(InputLanguageManager));
					break;
				case 296:
					lazyTypeOf = (() => typeof(InputManager));
					break;
				case 297:
					lazyTypeOf = (() => typeof(InputMethod));
					break;
				case 298:
					lazyTypeOf = (() => typeof(InputScope));
					break;
				case 299:
					lazyTypeOf = (() => typeof(InputScopeConverter));
					break;
				case 300:
					lazyTypeOf = (() => typeof(InputScopeName));
					break;
				case 301:
					lazyTypeOf = (() => typeof(InputScopeNameConverter));
					break;
				case 302:
					lazyTypeOf = (() => typeof(short));
					break;
				case 303:
					lazyTypeOf = (() => typeof(Int16Animation));
					break;
				case 304:
					lazyTypeOf = (() => typeof(Int16AnimationBase));
					break;
				case 305:
					lazyTypeOf = (() => typeof(Int16AnimationUsingKeyFrames));
					break;
				case 306:
					lazyTypeOf = (() => typeof(Int16Converter));
					break;
				case 307:
					lazyTypeOf = (() => typeof(Int16KeyFrame));
					break;
				case 308:
					lazyTypeOf = (() => typeof(Int16KeyFrameCollection));
					break;
				case 309:
					lazyTypeOf = (() => typeof(int));
					break;
				case 310:
					lazyTypeOf = (() => typeof(Int32Animation));
					break;
				case 311:
					lazyTypeOf = (() => typeof(Int32AnimationBase));
					break;
				case 312:
					lazyTypeOf = (() => typeof(Int32AnimationUsingKeyFrames));
					break;
				case 313:
					lazyTypeOf = (() => typeof(Int32Collection));
					break;
				case 314:
					lazyTypeOf = (() => typeof(Int32CollectionConverter));
					break;
				case 315:
					lazyTypeOf = (() => typeof(Int32Converter));
					break;
				case 316:
					lazyTypeOf = (() => typeof(Int32KeyFrame));
					break;
				case 317:
					lazyTypeOf = (() => typeof(Int32KeyFrameCollection));
					break;
				case 318:
					lazyTypeOf = (() => typeof(Int32Rect));
					break;
				case 319:
					lazyTypeOf = (() => typeof(Int32RectConverter));
					break;
				case 320:
					lazyTypeOf = (() => typeof(long));
					break;
				case 321:
					lazyTypeOf = (() => typeof(Int64Animation));
					break;
				case 322:
					lazyTypeOf = (() => typeof(Int64AnimationBase));
					break;
				case 323:
					lazyTypeOf = (() => typeof(Int64AnimationUsingKeyFrames));
					break;
				case 324:
					lazyTypeOf = (() => typeof(Int64Converter));
					break;
				case 325:
					lazyTypeOf = (() => typeof(Int64KeyFrame));
					break;
				case 326:
					lazyTypeOf = (() => typeof(Int64KeyFrameCollection));
					break;
				case 327:
					lazyTypeOf = (() => typeof(Italic));
					break;
				case 328:
					lazyTypeOf = (() => typeof(ItemCollection));
					break;
				case 329:
					lazyTypeOf = (() => typeof(ItemsControl));
					break;
				case 330:
					lazyTypeOf = (() => typeof(ItemsPanelTemplate));
					break;
				case 331:
					lazyTypeOf = (() => typeof(ItemsPresenter));
					break;
				case 332:
					lazyTypeOf = (() => typeof(JournalEntry));
					break;
				case 333:
					lazyTypeOf = (() => typeof(JournalEntryListConverter));
					break;
				case 334:
					lazyTypeOf = (() => typeof(JournalEntryUnifiedViewConverter));
					break;
				case 335:
					lazyTypeOf = (() => typeof(JpegBitmapDecoder));
					break;
				case 336:
					lazyTypeOf = (() => typeof(JpegBitmapEncoder));
					break;
				case 337:
					lazyTypeOf = (() => typeof(KeyBinding));
					break;
				case 338:
					lazyTypeOf = (() => typeof(KeyConverter));
					break;
				case 339:
					lazyTypeOf = (() => typeof(KeyGesture));
					break;
				case 340:
					lazyTypeOf = (() => typeof(KeyGestureConverter));
					break;
				case 341:
					lazyTypeOf = (() => typeof(KeySpline));
					break;
				case 342:
					lazyTypeOf = (() => typeof(KeySplineConverter));
					break;
				case 343:
					lazyTypeOf = (() => typeof(KeyTime));
					break;
				case 344:
					lazyTypeOf = (() => typeof(KeyTimeConverter));
					break;
				case 345:
					lazyTypeOf = (() => typeof(KeyboardDevice));
					break;
				case 346:
					lazyTypeOf = (() => typeof(Label));
					break;
				case 347:
					lazyTypeOf = (() => typeof(LateBoundBitmapDecoder));
					break;
				case 348:
					lazyTypeOf = (() => typeof(LengthConverter));
					break;
				case 349:
					lazyTypeOf = (() => typeof(Light));
					break;
				case 350:
					lazyTypeOf = (() => typeof(Line));
					break;
				case 351:
					lazyTypeOf = (() => typeof(LineBreak));
					break;
				case 352:
					lazyTypeOf = (() => typeof(LineGeometry));
					break;
				case 353:
					lazyTypeOf = (() => typeof(LineSegment));
					break;
				case 354:
					lazyTypeOf = (() => typeof(LinearByteKeyFrame));
					break;
				case 355:
					lazyTypeOf = (() => typeof(LinearColorKeyFrame));
					break;
				case 356:
					lazyTypeOf = (() => typeof(LinearDecimalKeyFrame));
					break;
				case 357:
					lazyTypeOf = (() => typeof(LinearDoubleKeyFrame));
					break;
				case 358:
					lazyTypeOf = (() => typeof(LinearGradientBrush));
					break;
				case 359:
					lazyTypeOf = (() => typeof(LinearInt16KeyFrame));
					break;
				case 360:
					lazyTypeOf = (() => typeof(LinearInt32KeyFrame));
					break;
				case 361:
					lazyTypeOf = (() => typeof(LinearInt64KeyFrame));
					break;
				case 362:
					lazyTypeOf = (() => typeof(LinearPoint3DKeyFrame));
					break;
				case 363:
					lazyTypeOf = (() => typeof(LinearPointKeyFrame));
					break;
				case 364:
					lazyTypeOf = (() => typeof(LinearQuaternionKeyFrame));
					break;
				case 365:
					lazyTypeOf = (() => typeof(LinearRectKeyFrame));
					break;
				case 366:
					lazyTypeOf = (() => typeof(LinearRotation3DKeyFrame));
					break;
				case 367:
					lazyTypeOf = (() => typeof(LinearSingleKeyFrame));
					break;
				case 368:
					lazyTypeOf = (() => typeof(LinearSizeKeyFrame));
					break;
				case 369:
					lazyTypeOf = (() => typeof(LinearThicknessKeyFrame));
					break;
				case 370:
					lazyTypeOf = (() => typeof(LinearVector3DKeyFrame));
					break;
				case 371:
					lazyTypeOf = (() => typeof(LinearVectorKeyFrame));
					break;
				case 372:
					lazyTypeOf = (() => typeof(List));
					break;
				case 373:
					lazyTypeOf = (() => typeof(ListBox));
					break;
				case 374:
					lazyTypeOf = (() => typeof(ListBoxItem));
					break;
				case 375:
					lazyTypeOf = (() => typeof(ListCollectionView));
					break;
				case 376:
					lazyTypeOf = (() => typeof(ListItem));
					break;
				case 377:
					lazyTypeOf = (() => typeof(ListView));
					break;
				case 378:
					lazyTypeOf = (() => typeof(ListViewItem));
					break;
				case 379:
					lazyTypeOf = (() => typeof(Localization));
					break;
				case 380:
					lazyTypeOf = (() => typeof(LostFocusEventManager));
					break;
				case 381:
					lazyTypeOf = (() => typeof(MarkupExtension));
					break;
				case 382:
					lazyTypeOf = (() => typeof(Material));
					break;
				case 383:
					lazyTypeOf = (() => typeof(MaterialCollection));
					break;
				case 384:
					lazyTypeOf = (() => typeof(MaterialGroup));
					break;
				case 385:
					lazyTypeOf = (() => typeof(Matrix));
					break;
				case 386:
					lazyTypeOf = (() => typeof(Matrix3D));
					break;
				case 387:
					lazyTypeOf = (() => typeof(Matrix3DConverter));
					break;
				case 388:
					lazyTypeOf = (() => typeof(MatrixAnimationBase));
					break;
				case 389:
					lazyTypeOf = (() => typeof(MatrixAnimationUsingKeyFrames));
					break;
				case 390:
					lazyTypeOf = (() => typeof(MatrixAnimationUsingPath));
					break;
				case 391:
					lazyTypeOf = (() => typeof(MatrixCamera));
					break;
				case 392:
					lazyTypeOf = (() => typeof(MatrixConverter));
					break;
				case 393:
					lazyTypeOf = (() => typeof(MatrixKeyFrame));
					break;
				case 394:
					lazyTypeOf = (() => typeof(MatrixKeyFrameCollection));
					break;
				case 395:
					lazyTypeOf = (() => typeof(MatrixTransform));
					break;
				case 396:
					lazyTypeOf = (() => typeof(MatrixTransform3D));
					break;
				case 397:
					lazyTypeOf = (() => typeof(MediaClock));
					break;
				case 398:
					lazyTypeOf = (() => typeof(MediaElement));
					break;
				case 399:
					lazyTypeOf = (() => typeof(MediaPlayer));
					break;
				case 400:
					lazyTypeOf = (() => typeof(MediaTimeline));
					break;
				case 401:
					lazyTypeOf = (() => typeof(Menu));
					break;
				case 402:
					lazyTypeOf = (() => typeof(MenuBase));
					break;
				case 403:
					lazyTypeOf = (() => typeof(MenuItem));
					break;
				case 404:
					lazyTypeOf = (() => typeof(MenuScrollingVisibilityConverter));
					break;
				case 405:
					lazyTypeOf = (() => typeof(MeshGeometry3D));
					break;
				case 406:
					lazyTypeOf = (() => typeof(Model3D));
					break;
				case 407:
					lazyTypeOf = (() => typeof(Model3DCollection));
					break;
				case 408:
					lazyTypeOf = (() => typeof(Model3DGroup));
					break;
				case 409:
					lazyTypeOf = (() => typeof(ModelVisual3D));
					break;
				case 410:
					lazyTypeOf = (() => typeof(ModifierKeysConverter));
					break;
				case 411:
					lazyTypeOf = (() => typeof(MouseActionConverter));
					break;
				case 412:
					lazyTypeOf = (() => typeof(MouseBinding));
					break;
				case 413:
					lazyTypeOf = (() => typeof(MouseDevice));
					break;
				case 414:
					lazyTypeOf = (() => typeof(MouseGesture));
					break;
				case 415:
					lazyTypeOf = (() => typeof(MouseGestureConverter));
					break;
				case 416:
					lazyTypeOf = (() => typeof(MultiBinding));
					break;
				case 417:
					lazyTypeOf = (() => typeof(MultiBindingExpression));
					break;
				case 418:
					lazyTypeOf = (() => typeof(MultiDataTrigger));
					break;
				case 419:
					lazyTypeOf = (() => typeof(MultiTrigger));
					break;
				case 420:
					lazyTypeOf = (() => typeof(NameScope));
					break;
				case 421:
					lazyTypeOf = (() => typeof(NavigationWindow));
					break;
				case 422:
					lazyTypeOf = (() => typeof(NullExtension));
					break;
				case 423:
					lazyTypeOf = (() => typeof(NullableBoolConverter));
					break;
				case 424:
					lazyTypeOf = (() => typeof(NullableConverter));
					break;
				case 425:
					lazyTypeOf = (() => typeof(NumberSubstitution));
					break;
				case 426:
					lazyTypeOf = (() => typeof(object));
					break;
				case 427:
					lazyTypeOf = (() => typeof(ObjectAnimationBase));
					break;
				case 428:
					lazyTypeOf = (() => typeof(ObjectAnimationUsingKeyFrames));
					break;
				case 429:
					lazyTypeOf = (() => typeof(ObjectDataProvider));
					break;
				case 430:
					lazyTypeOf = (() => typeof(ObjectKeyFrame));
					break;
				case 431:
					lazyTypeOf = (() => typeof(ObjectKeyFrameCollection));
					break;
				case 432:
					lazyTypeOf = (() => typeof(OrthographicCamera));
					break;
				case 433:
					lazyTypeOf = (() => typeof(OuterGlowBitmapEffect));
					break;
				case 434:
					lazyTypeOf = (() => typeof(Page));
					break;
				case 435:
					lazyTypeOf = (() => typeof(PageContent));
					break;
				case 436:
					lazyTypeOf = (() => typeof(PageFunctionBase));
					break;
				case 437:
					lazyTypeOf = (() => typeof(Panel));
					break;
				case 438:
					lazyTypeOf = (() => typeof(Paragraph));
					break;
				case 439:
					lazyTypeOf = (() => typeof(ParallelTimeline));
					break;
				case 440:
					lazyTypeOf = (() => typeof(ParserContext));
					break;
				case 441:
					lazyTypeOf = (() => typeof(PasswordBox));
					break;
				case 442:
					lazyTypeOf = (() => typeof(Path));
					break;
				case 443:
					lazyTypeOf = (() => typeof(PathFigure));
					break;
				case 444:
					lazyTypeOf = (() => typeof(PathFigureCollection));
					break;
				case 445:
					lazyTypeOf = (() => typeof(PathFigureCollectionConverter));
					break;
				case 446:
					lazyTypeOf = (() => typeof(PathGeometry));
					break;
				case 447:
					lazyTypeOf = (() => typeof(PathSegment));
					break;
				case 448:
					lazyTypeOf = (() => typeof(PathSegmentCollection));
					break;
				case 449:
					lazyTypeOf = (() => typeof(PauseStoryboard));
					break;
				case 450:
					lazyTypeOf = (() => typeof(Pen));
					break;
				case 451:
					lazyTypeOf = (() => typeof(PerspectiveCamera));
					break;
				case 452:
					lazyTypeOf = (() => typeof(PixelFormat));
					break;
				case 453:
					lazyTypeOf = (() => typeof(PixelFormatConverter));
					break;
				case 454:
					lazyTypeOf = (() => typeof(PngBitmapDecoder));
					break;
				case 455:
					lazyTypeOf = (() => typeof(PngBitmapEncoder));
					break;
				case 456:
					lazyTypeOf = (() => typeof(Point));
					break;
				case 457:
					lazyTypeOf = (() => typeof(Point3D));
					break;
				case 458:
					lazyTypeOf = (() => typeof(Point3DAnimation));
					break;
				case 459:
					lazyTypeOf = (() => typeof(Point3DAnimationBase));
					break;
				case 460:
					lazyTypeOf = (() => typeof(Point3DAnimationUsingKeyFrames));
					break;
				case 461:
					lazyTypeOf = (() => typeof(Point3DCollection));
					break;
				case 462:
					lazyTypeOf = (() => typeof(Point3DCollectionConverter));
					break;
				case 463:
					lazyTypeOf = (() => typeof(Point3DConverter));
					break;
				case 464:
					lazyTypeOf = (() => typeof(Point3DKeyFrame));
					break;
				case 465:
					lazyTypeOf = (() => typeof(Point3DKeyFrameCollection));
					break;
				case 466:
					lazyTypeOf = (() => typeof(Point4D));
					break;
				case 467:
					lazyTypeOf = (() => typeof(Point4DConverter));
					break;
				case 468:
					lazyTypeOf = (() => typeof(PointAnimation));
					break;
				case 469:
					lazyTypeOf = (() => typeof(PointAnimationBase));
					break;
				case 470:
					lazyTypeOf = (() => typeof(PointAnimationUsingKeyFrames));
					break;
				case 471:
					lazyTypeOf = (() => typeof(PointAnimationUsingPath));
					break;
				case 472:
					lazyTypeOf = (() => typeof(PointCollection));
					break;
				case 473:
					lazyTypeOf = (() => typeof(PointCollectionConverter));
					break;
				case 474:
					lazyTypeOf = (() => typeof(PointConverter));
					break;
				case 475:
					lazyTypeOf = (() => typeof(PointIListConverter));
					break;
				case 476:
					lazyTypeOf = (() => typeof(PointKeyFrame));
					break;
				case 477:
					lazyTypeOf = (() => typeof(PointKeyFrameCollection));
					break;
				case 478:
					lazyTypeOf = (() => typeof(PointLight));
					break;
				case 479:
					lazyTypeOf = (() => typeof(PointLightBase));
					break;
				case 480:
					lazyTypeOf = (() => typeof(PolyBezierSegment));
					break;
				case 481:
					lazyTypeOf = (() => typeof(PolyLineSegment));
					break;
				case 482:
					lazyTypeOf = (() => typeof(PolyQuadraticBezierSegment));
					break;
				case 483:
					lazyTypeOf = (() => typeof(Polygon));
					break;
				case 484:
					lazyTypeOf = (() => typeof(Polyline));
					break;
				case 485:
					lazyTypeOf = (() => typeof(Popup));
					break;
				case 486:
					lazyTypeOf = (() => typeof(PresentationSource));
					break;
				case 487:
					lazyTypeOf = (() => typeof(PriorityBinding));
					break;
				case 488:
					lazyTypeOf = (() => typeof(PriorityBindingExpression));
					break;
				case 489:
					lazyTypeOf = (() => typeof(ProgressBar));
					break;
				case 490:
					lazyTypeOf = (() => typeof(ProjectionCamera));
					break;
				case 491:
					lazyTypeOf = (() => typeof(PropertyPath));
					break;
				case 492:
					lazyTypeOf = (() => typeof(PropertyPathConverter));
					break;
				case 493:
					lazyTypeOf = (() => typeof(QuadraticBezierSegment));
					break;
				case 494:
					lazyTypeOf = (() => typeof(Quaternion));
					break;
				case 495:
					lazyTypeOf = (() => typeof(QuaternionAnimation));
					break;
				case 496:
					lazyTypeOf = (() => typeof(QuaternionAnimationBase));
					break;
				case 497:
					lazyTypeOf = (() => typeof(QuaternionAnimationUsingKeyFrames));
					break;
				case 498:
					lazyTypeOf = (() => typeof(QuaternionConverter));
					break;
				case 499:
					lazyTypeOf = (() => typeof(QuaternionKeyFrame));
					break;
				case 500:
					lazyTypeOf = (() => typeof(QuaternionKeyFrameCollection));
					break;
				case 501:
					lazyTypeOf = (() => typeof(QuaternionRotation3D));
					break;
				case 502:
					lazyTypeOf = (() => typeof(RadialGradientBrush));
					break;
				case 503:
					lazyTypeOf = (() => typeof(RadioButton));
					break;
				case 504:
					lazyTypeOf = (() => typeof(RangeBase));
					break;
				case 505:
					lazyTypeOf = (() => typeof(Rect));
					break;
				case 506:
					lazyTypeOf = (() => typeof(Rect3D));
					break;
				case 507:
					lazyTypeOf = (() => typeof(Rect3DConverter));
					break;
				case 508:
					lazyTypeOf = (() => typeof(RectAnimation));
					break;
				case 509:
					lazyTypeOf = (() => typeof(RectAnimationBase));
					break;
				case 510:
					lazyTypeOf = (() => typeof(RectAnimationUsingKeyFrames));
					break;
				case 511:
					lazyTypeOf = (() => typeof(RectConverter));
					break;
				case 512:
					lazyTypeOf = (() => typeof(RectKeyFrame));
					break;
				case 513:
					lazyTypeOf = (() => typeof(RectKeyFrameCollection));
					break;
				case 514:
					lazyTypeOf = (() => typeof(Rectangle));
					break;
				case 515:
					lazyTypeOf = (() => typeof(RectangleGeometry));
					break;
				case 516:
					lazyTypeOf = (() => typeof(RelativeSource));
					break;
				case 517:
					lazyTypeOf = (() => typeof(RemoveStoryboard));
					break;
				case 518:
					lazyTypeOf = (() => typeof(RenderOptions));
					break;
				case 519:
					lazyTypeOf = (() => typeof(RenderTargetBitmap));
					break;
				case 520:
					lazyTypeOf = (() => typeof(RepeatBehavior));
					break;
				case 521:
					lazyTypeOf = (() => typeof(RepeatBehaviorConverter));
					break;
				case 522:
					lazyTypeOf = (() => typeof(RepeatButton));
					break;
				case 523:
					lazyTypeOf = (() => typeof(ResizeGrip));
					break;
				case 524:
					lazyTypeOf = (() => typeof(ResourceDictionary));
					break;
				case 525:
					lazyTypeOf = (() => typeof(ResourceKey));
					break;
				case 526:
					lazyTypeOf = (() => typeof(ResumeStoryboard));
					break;
				case 527:
					lazyTypeOf = (() => typeof(RichTextBox));
					break;
				case 528:
					lazyTypeOf = (() => typeof(RotateTransform));
					break;
				case 529:
					lazyTypeOf = (() => typeof(RotateTransform3D));
					break;
				case 530:
					lazyTypeOf = (() => typeof(Rotation3D));
					break;
				case 531:
					lazyTypeOf = (() => typeof(Rotation3DAnimation));
					break;
				case 532:
					lazyTypeOf = (() => typeof(Rotation3DAnimationBase));
					break;
				case 533:
					lazyTypeOf = (() => typeof(Rotation3DAnimationUsingKeyFrames));
					break;
				case 534:
					lazyTypeOf = (() => typeof(Rotation3DKeyFrame));
					break;
				case 535:
					lazyTypeOf = (() => typeof(Rotation3DKeyFrameCollection));
					break;
				case 536:
					lazyTypeOf = (() => typeof(RoutedCommand));
					break;
				case 537:
					lazyTypeOf = (() => typeof(RoutedEvent));
					break;
				case 538:
					lazyTypeOf = (() => typeof(RoutedEventConverter));
					break;
				case 539:
					lazyTypeOf = (() => typeof(RoutedUICommand));
					break;
				case 540:
					lazyTypeOf = (() => typeof(RoutingStrategy));
					break;
				case 541:
					lazyTypeOf = (() => typeof(RowDefinition));
					break;
				case 542:
					lazyTypeOf = (() => typeof(Run));
					break;
				case 543:
					lazyTypeOf = (() => typeof(RuntimeNamePropertyAttribute));
					break;
				case 544:
					lazyTypeOf = (() => typeof(sbyte));
					break;
				case 545:
					lazyTypeOf = (() => typeof(SByteConverter));
					break;
				case 546:
					lazyTypeOf = (() => typeof(ScaleTransform));
					break;
				case 547:
					lazyTypeOf = (() => typeof(ScaleTransform3D));
					break;
				case 548:
					lazyTypeOf = (() => typeof(ScrollBar));
					break;
				case 549:
					lazyTypeOf = (() => typeof(ScrollContentPresenter));
					break;
				case 550:
					lazyTypeOf = (() => typeof(ScrollViewer));
					break;
				case 551:
					lazyTypeOf = (() => typeof(Section));
					break;
				case 552:
					lazyTypeOf = (() => typeof(SeekStoryboard));
					break;
				case 553:
					lazyTypeOf = (() => typeof(Selector));
					break;
				case 554:
					lazyTypeOf = (() => typeof(Separator));
					break;
				case 555:
					lazyTypeOf = (() => typeof(SetStoryboardSpeedRatio));
					break;
				case 556:
					lazyTypeOf = (() => typeof(Setter));
					break;
				case 557:
					lazyTypeOf = (() => typeof(SetterBase));
					break;
				case 558:
					lazyTypeOf = (() => typeof(Shape));
					break;
				case 559:
					lazyTypeOf = (() => typeof(float));
					break;
				case 560:
					lazyTypeOf = (() => typeof(SingleAnimation));
					break;
				case 561:
					lazyTypeOf = (() => typeof(SingleAnimationBase));
					break;
				case 562:
					lazyTypeOf = (() => typeof(SingleAnimationUsingKeyFrames));
					break;
				case 563:
					lazyTypeOf = (() => typeof(SingleConverter));
					break;
				case 564:
					lazyTypeOf = (() => typeof(SingleKeyFrame));
					break;
				case 565:
					lazyTypeOf = (() => typeof(SingleKeyFrameCollection));
					break;
				case 566:
					lazyTypeOf = (() => typeof(Size));
					break;
				case 567:
					lazyTypeOf = (() => typeof(Size3D));
					break;
				case 568:
					lazyTypeOf = (() => typeof(Size3DConverter));
					break;
				case 569:
					lazyTypeOf = (() => typeof(SizeAnimation));
					break;
				case 570:
					lazyTypeOf = (() => typeof(SizeAnimationBase));
					break;
				case 571:
					lazyTypeOf = (() => typeof(SizeAnimationUsingKeyFrames));
					break;
				case 572:
					lazyTypeOf = (() => typeof(SizeConverter));
					break;
				case 573:
					lazyTypeOf = (() => typeof(SizeKeyFrame));
					break;
				case 574:
					lazyTypeOf = (() => typeof(SizeKeyFrameCollection));
					break;
				case 575:
					lazyTypeOf = (() => typeof(SkewTransform));
					break;
				case 576:
					lazyTypeOf = (() => typeof(SkipStoryboardToFill));
					break;
				case 577:
					lazyTypeOf = (() => typeof(Slider));
					break;
				case 578:
					lazyTypeOf = (() => typeof(SolidColorBrush));
					break;
				case 579:
					lazyTypeOf = (() => typeof(SoundPlayerAction));
					break;
				case 580:
					lazyTypeOf = (() => typeof(Span));
					break;
				case 581:
					lazyTypeOf = (() => typeof(SpecularMaterial));
					break;
				case 582:
					lazyTypeOf = (() => typeof(SpellCheck));
					break;
				case 583:
					lazyTypeOf = (() => typeof(SplineByteKeyFrame));
					break;
				case 584:
					lazyTypeOf = (() => typeof(SplineColorKeyFrame));
					break;
				case 585:
					lazyTypeOf = (() => typeof(SplineDecimalKeyFrame));
					break;
				case 586:
					lazyTypeOf = (() => typeof(SplineDoubleKeyFrame));
					break;
				case 587:
					lazyTypeOf = (() => typeof(SplineInt16KeyFrame));
					break;
				case 588:
					lazyTypeOf = (() => typeof(SplineInt32KeyFrame));
					break;
				case 589:
					lazyTypeOf = (() => typeof(SplineInt64KeyFrame));
					break;
				case 590:
					lazyTypeOf = (() => typeof(SplinePoint3DKeyFrame));
					break;
				case 591:
					lazyTypeOf = (() => typeof(SplinePointKeyFrame));
					break;
				case 592:
					lazyTypeOf = (() => typeof(SplineQuaternionKeyFrame));
					break;
				case 593:
					lazyTypeOf = (() => typeof(SplineRectKeyFrame));
					break;
				case 594:
					lazyTypeOf = (() => typeof(SplineRotation3DKeyFrame));
					break;
				case 595:
					lazyTypeOf = (() => typeof(SplineSingleKeyFrame));
					break;
				case 596:
					lazyTypeOf = (() => typeof(SplineSizeKeyFrame));
					break;
				case 597:
					lazyTypeOf = (() => typeof(SplineThicknessKeyFrame));
					break;
				case 598:
					lazyTypeOf = (() => typeof(SplineVector3DKeyFrame));
					break;
				case 599:
					lazyTypeOf = (() => typeof(SplineVectorKeyFrame));
					break;
				case 600:
					lazyTypeOf = (() => typeof(SpotLight));
					break;
				case 601:
					lazyTypeOf = (() => typeof(StackPanel));
					break;
				case 602:
					lazyTypeOf = (() => typeof(StaticExtension));
					break;
				case 603:
					lazyTypeOf = (() => typeof(StaticResourceExtension));
					break;
				case 604:
					lazyTypeOf = (() => typeof(StatusBar));
					break;
				case 605:
					lazyTypeOf = (() => typeof(StatusBarItem));
					break;
				case 606:
					lazyTypeOf = (() => typeof(StickyNoteControl));
					break;
				case 607:
					lazyTypeOf = (() => typeof(StopStoryboard));
					break;
				case 608:
					lazyTypeOf = (() => typeof(Storyboard));
					break;
				case 609:
					lazyTypeOf = (() => typeof(StreamGeometry));
					break;
				case 610:
					lazyTypeOf = (() => typeof(StreamGeometryContext));
					break;
				case 611:
					lazyTypeOf = (() => typeof(StreamResourceInfo));
					break;
				case 612:
					lazyTypeOf = (() => typeof(string));
					break;
				case 613:
					lazyTypeOf = (() => typeof(StringAnimationBase));
					break;
				case 614:
					lazyTypeOf = (() => typeof(StringAnimationUsingKeyFrames));
					break;
				case 615:
					lazyTypeOf = (() => typeof(StringConverter));
					break;
				case 616:
					lazyTypeOf = (() => typeof(StringKeyFrame));
					break;
				case 617:
					lazyTypeOf = (() => typeof(StringKeyFrameCollection));
					break;
				case 618:
					lazyTypeOf = (() => typeof(StrokeCollection));
					break;
				case 619:
					lazyTypeOf = (() => typeof(StrokeCollectionConverter));
					break;
				case 620:
					lazyTypeOf = (() => typeof(Style));
					break;
				case 621:
					lazyTypeOf = (() => typeof(Stylus));
					break;
				case 622:
					lazyTypeOf = (() => typeof(StylusDevice));
					break;
				case 623:
					lazyTypeOf = (() => typeof(TabControl));
					break;
				case 624:
					lazyTypeOf = (() => typeof(TabItem));
					break;
				case 625:
					lazyTypeOf = (() => typeof(TabPanel));
					break;
				case 626:
					lazyTypeOf = (() => typeof(Table));
					break;
				case 627:
					lazyTypeOf = (() => typeof(TableCell));
					break;
				case 628:
					lazyTypeOf = (() => typeof(TableColumn));
					break;
				case 629:
					lazyTypeOf = (() => typeof(TableRow));
					break;
				case 630:
					lazyTypeOf = (() => typeof(TableRowGroup));
					break;
				case 631:
					lazyTypeOf = (() => typeof(TabletDevice));
					break;
				case 632:
					lazyTypeOf = (() => typeof(TemplateBindingExpression));
					break;
				case 633:
					lazyTypeOf = (() => typeof(TemplateBindingExpressionConverter));
					break;
				case 634:
					lazyTypeOf = (() => typeof(TemplateBindingExtension));
					break;
				case 635:
					lazyTypeOf = (() => typeof(TemplateBindingExtensionConverter));
					break;
				case 636:
					lazyTypeOf = (() => typeof(TemplateKey));
					break;
				case 637:
					lazyTypeOf = (() => typeof(TemplateKeyConverter));
					break;
				case 638:
					lazyTypeOf = (() => typeof(TextBlock));
					break;
				case 639:
					lazyTypeOf = (() => typeof(TextBox));
					break;
				case 640:
					lazyTypeOf = (() => typeof(TextBoxBase));
					break;
				case 641:
					lazyTypeOf = (() => typeof(TextComposition));
					break;
				case 642:
					lazyTypeOf = (() => typeof(TextCompositionManager));
					break;
				case 643:
					lazyTypeOf = (() => typeof(TextDecoration));
					break;
				case 644:
					lazyTypeOf = (() => typeof(TextDecorationCollection));
					break;
				case 645:
					lazyTypeOf = (() => typeof(TextDecorationCollectionConverter));
					break;
				case 646:
					lazyTypeOf = (() => typeof(TextEffect));
					break;
				case 647:
					lazyTypeOf = (() => typeof(TextEffectCollection));
					break;
				case 648:
					lazyTypeOf = (() => typeof(TextElement));
					break;
				case 649:
					lazyTypeOf = (() => typeof(TextSearch));
					break;
				case 650:
					lazyTypeOf = (() => typeof(ThemeDictionaryExtension));
					break;
				case 651:
					lazyTypeOf = (() => typeof(Thickness));
					break;
				case 652:
					lazyTypeOf = (() => typeof(ThicknessAnimation));
					break;
				case 653:
					lazyTypeOf = (() => typeof(ThicknessAnimationBase));
					break;
				case 654:
					lazyTypeOf = (() => typeof(ThicknessAnimationUsingKeyFrames));
					break;
				case 655:
					lazyTypeOf = (() => typeof(ThicknessConverter));
					break;
				case 656:
					lazyTypeOf = (() => typeof(ThicknessKeyFrame));
					break;
				case 657:
					lazyTypeOf = (() => typeof(ThicknessKeyFrameCollection));
					break;
				case 658:
					lazyTypeOf = (() => typeof(Thumb));
					break;
				case 659:
					lazyTypeOf = (() => typeof(TickBar));
					break;
				case 660:
					lazyTypeOf = (() => typeof(TiffBitmapDecoder));
					break;
				case 661:
					lazyTypeOf = (() => typeof(TiffBitmapEncoder));
					break;
				case 662:
					lazyTypeOf = (() => typeof(TileBrush));
					break;
				case 663:
					lazyTypeOf = (() => typeof(TimeSpan));
					break;
				case 664:
					lazyTypeOf = (() => typeof(TimeSpanConverter));
					break;
				case 665:
					lazyTypeOf = (() => typeof(Timeline));
					break;
				case 666:
					lazyTypeOf = (() => typeof(TimelineCollection));
					break;
				case 667:
					lazyTypeOf = (() => typeof(TimelineGroup));
					break;
				case 668:
					lazyTypeOf = (() => typeof(ToggleButton));
					break;
				case 669:
					lazyTypeOf = (() => typeof(ToolBar));
					break;
				case 670:
					lazyTypeOf = (() => typeof(ToolBarOverflowPanel));
					break;
				case 671:
					lazyTypeOf = (() => typeof(ToolBarPanel));
					break;
				case 672:
					lazyTypeOf = (() => typeof(ToolBarTray));
					break;
				case 673:
					lazyTypeOf = (() => typeof(ToolTip));
					break;
				case 674:
					lazyTypeOf = (() => typeof(ToolTipService));
					break;
				case 675:
					lazyTypeOf = (() => typeof(Track));
					break;
				case 676:
					lazyTypeOf = (() => typeof(Transform));
					break;
				case 677:
					lazyTypeOf = (() => typeof(Transform3D));
					break;
				case 678:
					lazyTypeOf = (() => typeof(Transform3DCollection));
					break;
				case 679:
					lazyTypeOf = (() => typeof(Transform3DGroup));
					break;
				case 680:
					lazyTypeOf = (() => typeof(TransformCollection));
					break;
				case 681:
					lazyTypeOf = (() => typeof(TransformConverter));
					break;
				case 682:
					lazyTypeOf = (() => typeof(TransformGroup));
					break;
				case 683:
					lazyTypeOf = (() => typeof(TransformedBitmap));
					break;
				case 684:
					lazyTypeOf = (() => typeof(TranslateTransform));
					break;
				case 685:
					lazyTypeOf = (() => typeof(TranslateTransform3D));
					break;
				case 686:
					lazyTypeOf = (() => typeof(TreeView));
					break;
				case 687:
					lazyTypeOf = (() => typeof(TreeViewItem));
					break;
				case 688:
					lazyTypeOf = (() => typeof(Trigger));
					break;
				case 689:
					lazyTypeOf = (() => typeof(TriggerAction));
					break;
				case 690:
					lazyTypeOf = (() => typeof(TriggerBase));
					break;
				case 691:
					lazyTypeOf = (() => typeof(TypeExtension));
					break;
				case 692:
					lazyTypeOf = (() => typeof(TypeTypeConverter));
					break;
				case 693:
					lazyTypeOf = (() => typeof(Typography));
					break;
				case 694:
					lazyTypeOf = (() => typeof(UIElement));
					break;
				case 695:
					lazyTypeOf = (() => typeof(ushort));
					break;
				case 696:
					lazyTypeOf = (() => typeof(UInt16Converter));
					break;
				case 697:
					lazyTypeOf = (() => typeof(uint));
					break;
				case 698:
					lazyTypeOf = (() => typeof(UInt32Converter));
					break;
				case 699:
					lazyTypeOf = (() => typeof(ulong));
					break;
				case 700:
					lazyTypeOf = (() => typeof(UInt64Converter));
					break;
				case 701:
					lazyTypeOf = (() => typeof(UShortIListConverter));
					break;
				case 702:
					lazyTypeOf = (() => typeof(Underline));
					break;
				case 703:
					lazyTypeOf = (() => typeof(UniformGrid));
					break;
				case 704:
					lazyTypeOf = (() => typeof(Uri));
					break;
				case 705:
					lazyTypeOf = (() => typeof(UriTypeConverter));
					break;
				case 706:
					lazyTypeOf = (() => typeof(UserControl));
					break;
				case 707:
					lazyTypeOf = (() => typeof(Validation));
					break;
				case 708:
					lazyTypeOf = (() => typeof(Vector));
					break;
				case 709:
					lazyTypeOf = (() => typeof(Vector3D));
					break;
				case 710:
					lazyTypeOf = (() => typeof(Vector3DAnimation));
					break;
				case 711:
					lazyTypeOf = (() => typeof(Vector3DAnimationBase));
					break;
				case 712:
					lazyTypeOf = (() => typeof(Vector3DAnimationUsingKeyFrames));
					break;
				case 713:
					lazyTypeOf = (() => typeof(Vector3DCollection));
					break;
				case 714:
					lazyTypeOf = (() => typeof(Vector3DCollectionConverter));
					break;
				case 715:
					lazyTypeOf = (() => typeof(Vector3DConverter));
					break;
				case 716:
					lazyTypeOf = (() => typeof(Vector3DKeyFrame));
					break;
				case 717:
					lazyTypeOf = (() => typeof(Vector3DKeyFrameCollection));
					break;
				case 718:
					lazyTypeOf = (() => typeof(VectorAnimation));
					break;
				case 719:
					lazyTypeOf = (() => typeof(VectorAnimationBase));
					break;
				case 720:
					lazyTypeOf = (() => typeof(VectorAnimationUsingKeyFrames));
					break;
				case 721:
					lazyTypeOf = (() => typeof(VectorCollection));
					break;
				case 722:
					lazyTypeOf = (() => typeof(VectorCollectionConverter));
					break;
				case 723:
					lazyTypeOf = (() => typeof(VectorConverter));
					break;
				case 724:
					lazyTypeOf = (() => typeof(VectorKeyFrame));
					break;
				case 725:
					lazyTypeOf = (() => typeof(VectorKeyFrameCollection));
					break;
				case 726:
					lazyTypeOf = (() => typeof(VideoDrawing));
					break;
				case 727:
					lazyTypeOf = (() => typeof(ViewBase));
					break;
				case 728:
					lazyTypeOf = (() => typeof(Viewbox));
					break;
				case 729:
					lazyTypeOf = (() => typeof(Viewport3D));
					break;
				case 730:
					lazyTypeOf = (() => typeof(Viewport3DVisual));
					break;
				case 731:
					lazyTypeOf = (() => typeof(VirtualizingPanel));
					break;
				case 732:
					lazyTypeOf = (() => typeof(VirtualizingStackPanel));
					break;
				case 733:
					lazyTypeOf = (() => typeof(Visual));
					break;
				case 734:
					lazyTypeOf = (() => typeof(Visual3D));
					break;
				case 735:
					lazyTypeOf = (() => typeof(VisualBrush));
					break;
				case 736:
					lazyTypeOf = (() => typeof(VisualTarget));
					break;
				case 737:
					lazyTypeOf = (() => typeof(WeakEventManager));
					break;
				case 738:
					lazyTypeOf = (() => typeof(WhitespaceSignificantCollectionAttribute));
					break;
				case 739:
					lazyTypeOf = (() => typeof(Window));
					break;
				case 740:
					lazyTypeOf = (() => typeof(WmpBitmapDecoder));
					break;
				case 741:
					lazyTypeOf = (() => typeof(WmpBitmapEncoder));
					break;
				case 742:
					lazyTypeOf = (() => typeof(WrapPanel));
					break;
				case 743:
					lazyTypeOf = (() => typeof(WriteableBitmap));
					break;
				case 744:
					lazyTypeOf = (() => typeof(XamlBrushSerializer));
					break;
				case 745:
					lazyTypeOf = (() => typeof(XamlInt32CollectionSerializer));
					break;
				case 746:
					lazyTypeOf = (() => typeof(XamlPathDataSerializer));
					break;
				case 747:
					lazyTypeOf = (() => typeof(XamlPoint3DCollectionSerializer));
					break;
				case 748:
					lazyTypeOf = (() => typeof(XamlPointCollectionSerializer));
					break;
				case 749:
					lazyTypeOf = (() => typeof(System.Windows.Markup.XamlReader));
					break;
				case 750:
					lazyTypeOf = (() => typeof(XamlStyleSerializer));
					break;
				case 751:
					lazyTypeOf = (() => typeof(XamlTemplateSerializer));
					break;
				case 752:
					lazyTypeOf = (() => typeof(XamlVector3DCollectionSerializer));
					break;
				case 753:
					lazyTypeOf = (() => typeof(System.Windows.Markup.XamlWriter));
					break;
				case 754:
					lazyTypeOf = (() => typeof(XmlDataProvider));
					break;
				case 755:
					lazyTypeOf = (() => typeof(XmlLangPropertyAttribute));
					break;
				case 756:
					lazyTypeOf = (() => typeof(XmlLanguage));
					break;
				case 757:
					lazyTypeOf = (() => typeof(XmlLanguageConverter));
					break;
				case 758:
					lazyTypeOf = (() => typeof(XmlNamespaceMapping));
					break;
				case 759:
					lazyTypeOf = (() => typeof(ZoomPercentageConverter));
					break;
				default:
					lazyTypeOf = (() => null);
					break;
				}
				return lazyTypeOf();
			}

			// Token: 0x06007F1D RID: 32541 RVA: 0x0023EF88 File Offset: 0x0023D188
			internal static TypeConverter CreateKnownTypeConverter(short converterId)
			{
				TypeConverter result = null;
				if (converterId <= -410)
				{
					if (converterId <= -568)
					{
						if (converterId <= -692)
						{
							if (converterId <= -715)
							{
								if (converterId <= -723)
								{
									if (converterId != -757)
									{
										if (converterId == -723)
										{
											result = new VectorConverter();
										}
									}
									else
									{
										result = new XmlLanguageConverter();
									}
								}
								else if (converterId != -722)
								{
									if (converterId == -715)
									{
										result = new Vector3DConverter();
									}
								}
								else
								{
									result = new VectorCollectionConverter();
								}
							}
							else if (converterId <= -705)
							{
								if (converterId != -714)
								{
									if (converterId == -705)
									{
										result = new UriTypeConverter();
									}
								}
								else
								{
									result = new Vector3DCollectionConverter();
								}
							}
							else
							{
								switch (converterId)
								{
								case -701:
									result = new UShortIListConverter();
									break;
								case -700:
									result = new UInt64Converter();
									break;
								case -699:
								case -697:
									break;
								case -698:
									result = new UInt32Converter();
									break;
								case -696:
									result = new UInt16Converter();
									break;
								default:
									if (converterId == -692)
									{
										result = new TypeTypeConverter();
									}
									break;
								}
							}
						}
						else if (converterId <= -645)
						{
							if (converterId <= -664)
							{
								if (converterId != -681)
								{
									if (converterId == -664)
									{
										result = new TimeSpanConverter();
									}
								}
								else
								{
									result = new TransformConverter();
								}
							}
							else if (converterId != -655)
							{
								if (converterId == -645)
								{
									result = new TextDecorationCollectionConverter();
								}
							}
							else
							{
								result = new ThicknessConverter();
							}
						}
						else if (converterId <= -619)
						{
							switch (converterId)
							{
							case -637:
								result = new TemplateKeyConverter();
								break;
							case -636:
							case -634:
								break;
							case -635:
								result = new TemplateBindingExtensionConverter();
								break;
							case -633:
								result = new TemplateBindingExpressionConverter();
								break;
							default:
								if (converterId == -619)
								{
									result = new StrokeCollectionConverter();
								}
								break;
							}
						}
						else if (converterId != -615)
						{
							if (converterId != -572)
							{
								if (converterId == -568)
								{
									result = new Size3DConverter();
								}
							}
							else
							{
								result = new SizeConverter();
							}
						}
						else
						{
							result = new StringConverter();
						}
					}
					else if (converterId <= -473)
					{
						if (converterId <= -521)
						{
							if (converterId <= -545)
							{
								if (converterId != -563)
								{
									if (converterId == -545)
									{
										result = new SByteConverter();
									}
								}
								else
								{
									result = new SingleConverter();
								}
							}
							else if (converterId != -538)
							{
								if (converterId == -521)
								{
									result = new RepeatBehaviorConverter();
								}
							}
							else
							{
								result = new RoutedEventConverter();
							}
						}
						else if (converterId <= -507)
						{
							if (converterId != -511)
							{
								if (converterId == -507)
								{
									result = new Rect3DConverter();
								}
							}
							else
							{
								result = new RectConverter();
							}
						}
						else if (converterId != -498)
						{
							if (converterId != -492)
							{
								switch (converterId)
								{
								case -475:
									result = new PointIListConverter();
									break;
								case -474:
									result = new PointConverter();
									break;
								case -473:
									result = new PointCollectionConverter();
									break;
								}
							}
							else
							{
								result = new PropertyPathConverter();
							}
						}
						else
						{
							result = new QuaternionConverter();
						}
					}
					else if (converterId <= -453)
					{
						if (converterId <= -463)
						{
							if (converterId != -467)
							{
								if (converterId == -463)
								{
									result = new Point3DConverter();
								}
							}
							else
							{
								result = new Point4DConverter();
							}
						}
						else if (converterId != -462)
						{
							if (converterId == -453)
							{
								result = new PixelFormatConverter();
							}
						}
						else
						{
							result = new Point3DCollectionConverter();
						}
					}
					else if (converterId <= -423)
					{
						if (converterId != -445)
						{
							if (converterId == -423)
							{
								result = new NullableBoolConverter();
							}
						}
						else
						{
							result = new PathFigureCollectionConverter();
						}
					}
					else if (converterId != -415)
					{
						if (converterId != -411)
						{
							if (converterId == -410)
							{
								result = new ModifierKeysConverter();
							}
						}
						else
						{
							result = new MouseActionConverter();
						}
					}
					else
					{
						result = new MouseGestureConverter();
					}
				}
				else if (converterId <= -201)
				{
					if (converterId <= -306)
					{
						if (converterId <= -338)
						{
							if (converterId <= -387)
							{
								if (converterId != -392)
								{
									if (converterId == -387)
									{
										result = new Matrix3DConverter();
									}
								}
								else
								{
									result = new MatrixConverter();
								}
							}
							else if (converterId != -348)
							{
								switch (converterId)
								{
								case -344:
									result = new KeyTimeConverter();
									break;
								case -342:
									result = new KeySplineConverter();
									break;
								case -340:
									result = new KeyGestureConverter();
									break;
								case -338:
									result = new KeyConverter();
									break;
								}
							}
							else
							{
								result = new LengthConverter();
							}
						}
						else if (converterId <= -319)
						{
							if (converterId != -324)
							{
								if (converterId == -319)
								{
									result = new Int32RectConverter();
								}
							}
							else
							{
								result = new Int64Converter();
							}
						}
						else if (converterId != -315)
						{
							if (converterId != -314)
							{
								if (converterId == -306)
								{
									result = new Int16Converter();
								}
							}
							else
							{
								result = new Int32CollectionConverter();
							}
						}
						else
						{
							result = new Int32Converter();
						}
					}
					else if (converterId <= -267)
					{
						if (converterId <= -299)
						{
							if (converterId != -301)
							{
								if (converterId == -299)
								{
									result = new InputScopeConverter();
								}
							}
							else
							{
								result = new InputScopeNameConverter();
							}
						}
						else if (converterId != -286)
						{
							if (converterId == -267)
							{
								result = new GuidConverter();
							}
						}
						else
						{
							result = new ImageSourceConverter();
						}
					}
					else if (converterId <= -240)
					{
						if (converterId != -256)
						{
							if (converterId == -240)
							{
								result = new GeometryConverter();
							}
						}
						else
						{
							result = new GridLengthConverter();
						}
					}
					else
					{
						switch (converterId)
						{
						case -222:
							result = new FontWeightConverter();
							break;
						case -221:
						case -219:
						case -217:
							break;
						case -220:
							result = new FontStyleConverter();
							break;
						case -218:
							result = new FontStretchConverter();
							break;
						case -216:
							result = new FontSizeConverter();
							break;
						case -215:
							result = new FontFamilyConverter();
							break;
						default:
							if (converterId != -204)
							{
								if (converterId == -201)
								{
									result = new ExpressionConverter();
								}
							}
							else
							{
								result = new FigureLengthConverter();
							}
							break;
						}
					}
				}
				else if (converterId <= -111)
				{
					if (converterId <= -138)
					{
						if (converterId <= -188)
						{
							if (converterId != -190)
							{
								if (converterId == -188)
								{
									result = new DurationConverter();
								}
							}
							else
							{
								result = new DynamicResourceExtensionConverter();
							}
						}
						else
						{
							switch (converterId)
							{
							case -176:
								result = new DoubleIListConverter();
								break;
							case -175:
								result = new DoubleConverter();
								break;
							case -174:
								result = new DoubleCollectionConverter();
								break;
							default:
								if (converterId == -138)
								{
									result = new DialogResultConverter();
								}
								break;
							}
						}
					}
					else if (converterId <= -130)
					{
						if (converterId != -137)
						{
							if (converterId == -130)
							{
								result = new DecimalConverter();
							}
						}
						else
						{
							result = new DependencyPropertyConverter();
						}
					}
					else if (converterId != -125)
					{
						if (converterId != -124)
						{
							switch (converterId)
							{
							case -117:
								result = new CursorConverter();
								break;
							case -115:
								result = new CultureInfoIetfLanguageTagConverter();
								break;
							case -114:
								result = new CultureInfoConverter();
								break;
							case -111:
								result = new CornerRadiusConverter();
								break;
							}
						}
						else
						{
							result = new DateTimeConverter();
						}
					}
					else
					{
						result = new DateTimeConverter2();
					}
				}
				else if (converterId <= -71)
				{
					if (converterId <= -94)
					{
						if (converterId != -96)
						{
							if (converterId == -94)
							{
								result = new CommandConverter();
							}
						}
						else
						{
							result = new ComponentResourceKeyConverter();
						}
					}
					else if (converterId != -87)
					{
						if (converterId == -71)
						{
							result = new CharIListConverter();
						}
					}
					else
					{
						result = new ColorConverter();
					}
				}
				else if (converterId <= -61)
				{
					if (converterId != -70)
					{
						if (converterId == -61)
						{
							result = new ByteConverter();
						}
					}
					else
					{
						result = new CharConverter();
					}
				}
				else if (converterId != -53)
				{
					if (converterId != -46)
					{
						if (converterId == -42)
						{
							result = new BoolIListConverter();
						}
					}
					else
					{
						result = new BooleanConverter();
					}
				}
				else
				{
					result = new BrushConverter();
				}
				return result;
			}

			// Token: 0x06007F1E RID: 32542 RVA: 0x0023F858 File Offset: 0x0023DA58
			public static bool GetKnownProperty(short propertyId, out short typeId, out string propertyName)
			{
				switch (propertyId)
				{
				case -268:
					typeId = -754;
					propertyName = "XmlSerializer";
					goto IL_174F;
				case -267:
					typeId = -742;
					propertyName = "Children";
					goto IL_174F;
				case -266:
					typeId = -739;
					propertyName = "Content";
					goto IL_174F;
				case -265:
					typeId = -732;
					propertyName = "Children";
					goto IL_174F;
				case -264:
					typeId = -731;
					propertyName = "Children";
					goto IL_174F;
				case -263:
					typeId = -730;
					propertyName = "Children";
					goto IL_174F;
				case -262:
					typeId = -728;
					propertyName = "Child";
					goto IL_174F;
				case -261:
					typeId = -720;
					propertyName = "KeyFrames";
					goto IL_174F;
				case -260:
					typeId = -712;
					propertyName = "KeyFrames";
					goto IL_174F;
				case -259:
					typeId = -706;
					propertyName = "Content";
					goto IL_174F;
				case -258:
					typeId = -703;
					propertyName = "Children";
					goto IL_174F;
				case -257:
					typeId = -702;
					propertyName = "Inlines";
					goto IL_174F;
				case -256:
					typeId = -688;
					propertyName = "Setters";
					goto IL_174F;
				case -255:
					typeId = -687;
					propertyName = "Items";
					goto IL_174F;
				case -254:
					typeId = -686;
					propertyName = "Items";
					goto IL_174F;
				case -253:
					typeId = -673;
					propertyName = "Content";
					goto IL_174F;
				case -252:
					typeId = -672;
					propertyName = "ToolBars";
					goto IL_174F;
				case -251:
					typeId = -671;
					propertyName = "Children";
					goto IL_174F;
				case -250:
					typeId = -670;
					propertyName = "Children";
					goto IL_174F;
				case -249:
					typeId = -669;
					propertyName = "Items";
					goto IL_174F;
				case -248:
					typeId = -668;
					propertyName = "Content";
					goto IL_174F;
				case -247:
					typeId = -654;
					propertyName = "KeyFrames";
					goto IL_174F;
				case -246:
					typeId = -638;
					propertyName = "Inlines";
					goto IL_174F;
				case -245:
					typeId = -630;
					propertyName = "Rows";
					goto IL_174F;
				case -244:
					typeId = -629;
					propertyName = "Cells";
					goto IL_174F;
				case -243:
					typeId = -627;
					propertyName = "Blocks";
					goto IL_174F;
				case -242:
					typeId = -626;
					propertyName = "RowGroups";
					goto IL_174F;
				case -241:
					typeId = -625;
					propertyName = "Children";
					goto IL_174F;
				case -240:
					typeId = -624;
					propertyName = "Content";
					goto IL_174F;
				case -239:
					typeId = -623;
					propertyName = "Items";
					goto IL_174F;
				case -238:
					typeId = -620;
					propertyName = "Setters";
					goto IL_174F;
				case -237:
					typeId = -614;
					propertyName = "KeyFrames";
					goto IL_174F;
				case -236:
					typeId = -608;
					propertyName = "Children";
					goto IL_174F;
				case -235:
					typeId = -605;
					propertyName = "Content";
					goto IL_174F;
				case -234:
					typeId = -604;
					propertyName = "Items";
					goto IL_174F;
				case -233:
					typeId = -601;
					propertyName = "Children";
					goto IL_174F;
				case -232:
					typeId = -580;
					propertyName = "Inlines";
					goto IL_174F;
				case -231:
					typeId = -571;
					propertyName = "KeyFrames";
					goto IL_174F;
				case -230:
					typeId = -562;
					propertyName = "KeyFrames";
					goto IL_174F;
				case -229:
					typeId = -553;
					propertyName = "Items";
					goto IL_174F;
				case -228:
					typeId = -551;
					propertyName = "Blocks";
					goto IL_174F;
				case -227:
					typeId = -550;
					propertyName = "Content";
					goto IL_174F;
				case -226:
					typeId = -542;
					propertyName = "Text";
					goto IL_174F;
				case -225:
					typeId = -533;
					propertyName = "KeyFrames";
					goto IL_174F;
				case -224:
					typeId = -527;
					propertyName = "Document";
					goto IL_174F;
				case -223:
					typeId = -522;
					propertyName = "Content";
					goto IL_174F;
				case -222:
					typeId = -510;
					propertyName = "KeyFrames";
					goto IL_174F;
				case -221:
					typeId = -503;
					propertyName = "Content";
					goto IL_174F;
				case -220:
					typeId = -502;
					propertyName = "GradientStops";
					goto IL_174F;
				case -219:
					typeId = -497;
					propertyName = "KeyFrames";
					goto IL_174F;
				case -218:
					typeId = -487;
					propertyName = "Bindings";
					goto IL_174F;
				case -217:
					typeId = -470;
					propertyName = "KeyFrames";
					goto IL_174F;
				case -216:
					typeId = -460;
					propertyName = "KeyFrames";
					goto IL_174F;
				case -215:
					typeId = -439;
					propertyName = "Children";
					goto IL_174F;
				case -214:
					typeId = -438;
					propertyName = "Inlines";
					goto IL_174F;
				case -213:
					typeId = -437;
					propertyName = "Children";
					goto IL_174F;
				case -212:
					typeId = -436;
					propertyName = "Content";
					goto IL_174F;
				case -211:
					typeId = -435;
					propertyName = "Child";
					goto IL_174F;
				case -210:
					typeId = -428;
					propertyName = "KeyFrames";
					goto IL_174F;
				case -209:
					typeId = -419;
					propertyName = "Setters";
					goto IL_174F;
				case -208:
					typeId = -418;
					propertyName = "Setters";
					goto IL_174F;
				case -207:
					typeId = -416;
					propertyName = "Bindings";
					goto IL_174F;
				case -206:
					typeId = -409;
					propertyName = "Children";
					goto IL_174F;
				case -205:
					typeId = -403;
					propertyName = "Items";
					goto IL_174F;
				case -204:
					typeId = -402;
					propertyName = "Items";
					goto IL_174F;
				case -203:
					typeId = -401;
					propertyName = "Items";
					goto IL_174F;
				case -202:
					typeId = -389;
					propertyName = "KeyFrames";
					goto IL_174F;
				case -201:
					typeId = -378;
					propertyName = "Content";
					goto IL_174F;
				case -200:
					typeId = -377;
					propertyName = "Items";
					goto IL_174F;
				case -199:
					typeId = -376;
					propertyName = "Blocks";
					goto IL_174F;
				case -198:
					typeId = -374;
					propertyName = "Content";
					goto IL_174F;
				case -197:
					typeId = -373;
					propertyName = "Items";
					goto IL_174F;
				case -196:
					typeId = -372;
					propertyName = "ListItems";
					goto IL_174F;
				case -195:
					typeId = -358;
					propertyName = "GradientStops";
					goto IL_174F;
				case -194:
					typeId = -346;
					propertyName = "Content";
					goto IL_174F;
				case -193:
					typeId = -330;
					propertyName = "VisualTree";
					goto IL_174F;
				case -192:
					typeId = -329;
					propertyName = "Items";
					goto IL_174F;
				case -191:
					typeId = -327;
					propertyName = "Inlines";
					goto IL_174F;
				case -190:
					typeId = -323;
					propertyName = "KeyFrames";
					goto IL_174F;
				case -189:
					typeId = -312;
					propertyName = "KeyFrames";
					goto IL_174F;
				case -188:
					typeId = -305;
					propertyName = "KeyFrames";
					goto IL_174F;
				case -187:
					typeId = -300;
					propertyName = "NameValue";
					goto IL_174F;
				case -186:
					typeId = -292;
					propertyName = "Child";
					goto IL_174F;
				case -185:
					typeId = -289;
					propertyName = "Child";
					goto IL_174F;
				case -184:
					typeId = -288;
					propertyName = "Children";
					goto IL_174F;
				case -183:
					typeId = -273;
					propertyName = "Inlines";
					goto IL_174F;
				case -182:
					typeId = -271;
					propertyName = "VisualTree";
					goto IL_174F;
				case -181:
					typeId = -270;
					propertyName = "Items";
					goto IL_174F;
				case -180:
					typeId = -269;
					propertyName = "Content";
					goto IL_174F;
				case -179:
					typeId = -265;
					propertyName = "Content";
					goto IL_174F;
				case -178:
					typeId = -264;
					propertyName = "Content";
					goto IL_174F;
				case -177:
					typeId = -260;
					propertyName = "Content";
					goto IL_174F;
				case -176:
					typeId = -258;
					propertyName = "Columns";
					goto IL_174F;
				case -175:
					typeId = -254;
					propertyName = "Children";
					goto IL_174F;
				case -174:
					typeId = -231;
					propertyName = "VisualTree";
					goto IL_174F;
				case -173:
					typeId = -210;
					propertyName = "Document";
					goto IL_174F;
				case -172:
					typeId = -209;
					propertyName = "Blocks";
					goto IL_174F;
				case -171:
					typeId = -208;
					propertyName = "Blocks";
					goto IL_174F;
				case -170:
					typeId = -207;
					propertyName = "Children";
					goto IL_174F;
				case -169:
					typeId = -206;
					propertyName = "References";
					goto IL_174F;
				case -168:
					typeId = -205;
					propertyName = "Pages";
					goto IL_174F;
				case -167:
					typeId = -202;
					propertyName = "Blocks";
					goto IL_174F;
				case -166:
					typeId = -199;
					propertyName = "Content";
					goto IL_174F;
				case -165:
					typeId = -198;
					propertyName = "Actions";
					goto IL_174F;
				case -164:
					typeId = -171;
					propertyName = "KeyFrames";
					goto IL_174F;
				case -163:
					typeId = -166;
					propertyName = "Document";
					goto IL_174F;
				case -162:
					typeId = -163;
					propertyName = "Children";
					goto IL_174F;
				case -161:
					typeId = -133;
					propertyName = "Child";
					goto IL_174F;
				case -160:
					typeId = -129;
					propertyName = "KeyFrames";
					goto IL_174F;
				case -159:
					typeId = -122;
					propertyName = "Setters";
					goto IL_174F;
				case -158:
					typeId = -120;
					propertyName = "VisualTree";
					goto IL_174F;
				case -157:
					typeId = -108;
					propertyName = "VisualTree";
					goto IL_174F;
				case -156:
					typeId = -105;
					propertyName = "Items";
					goto IL_174F;
				case -155:
					typeId = -93;
					propertyName = "Content";
					goto IL_174F;
				case -154:
					typeId = -92;
					propertyName = "Items";
					goto IL_174F;
				case -153:
					typeId = -84;
					propertyName = "KeyFrames";
					goto IL_174F;
				case -152:
					typeId = -74;
					propertyName = "Content";
					goto IL_174F;
				case -151:
					typeId = -69;
					propertyName = "KeyFrames";
					goto IL_174F;
				case -150:
					typeId = -66;
					propertyName = "Children";
					goto IL_174F;
				case -149:
					typeId = -60;
					propertyName = "KeyFrames";
					goto IL_174F;
				case -148:
					typeId = -56;
					propertyName = "Content";
					goto IL_174F;
				case -147:
					typeId = -55;
					propertyName = "Content";
					goto IL_174F;
				case -146:
					typeId = -54;
					propertyName = "Child";
					goto IL_174F;
				case -145:
					typeId = -50;
					propertyName = "Child";
					goto IL_174F;
				case -144:
					typeId = -45;
					propertyName = "KeyFrames";
					goto IL_174F;
				case -143:
					typeId = -41;
					propertyName = "Inlines";
					goto IL_174F;
				case -142:
					typeId = -37;
					propertyName = "Child";
					goto IL_174F;
				case -141:
					typeId = -14;
					propertyName = "Items";
					goto IL_174F;
				case -140:
					typeId = -8;
					propertyName = "Blocks";
					goto IL_174F;
				case -139:
					typeId = -4;
					propertyName = "Child";
					goto IL_174F;
				case -138:
					typeId = -2;
					propertyName = "Child";
					goto IL_174F;
				case -136:
					typeId = -729;
					propertyName = "Children";
					goto IL_174F;
				case -135:
					typeId = -694;
					propertyName = "Visibility";
					goto IL_174F;
				case -134:
					typeId = -694;
					propertyName = "RenderTransform";
					goto IL_174F;
				case -133:
					typeId = -694;
					propertyName = "IsEnabled";
					goto IL_174F;
				case -132:
					typeId = -694;
					propertyName = "Focusable";
					goto IL_174F;
				case -131:
					typeId = -694;
					propertyName = "ClipToBounds";
					goto IL_174F;
				case -130:
					typeId = -682;
					propertyName = "Children";
					goto IL_174F;
				case -129:
					typeId = -679;
					propertyName = "Children";
					goto IL_174F;
				case -128:
					typeId = -675;
					propertyName = "ViewportSize";
					goto IL_174F;
				case -127:
					typeId = -675;
					propertyName = "Value";
					goto IL_174F;
				case -126:
					typeId = -675;
					propertyName = "Orientation";
					goto IL_174F;
				case -125:
					typeId = -675;
					propertyName = "Minimum";
					goto IL_174F;
				case -124:
					typeId = -675;
					propertyName = "Maximum";
					goto IL_174F;
				case -123:
					typeId = -675;
					propertyName = "IsDirectionReversed";
					goto IL_174F;
				case -122:
					typeId = -667;
					propertyName = "Children";
					goto IL_174F;
				case -121:
					typeId = -648;
					propertyName = "Foreground";
					goto IL_174F;
				case -120:
					typeId = -648;
					propertyName = "FontWeight";
					goto IL_174F;
				case -119:
					typeId = -648;
					propertyName = "FontStyle";
					goto IL_174F;
				case -118:
					typeId = -648;
					propertyName = "FontStretch";
					goto IL_174F;
				case -117:
					typeId = -648;
					propertyName = "FontSize";
					goto IL_174F;
				case -116:
					typeId = -648;
					propertyName = "FontFamily";
					goto IL_174F;
				case -115:
					typeId = -648;
					propertyName = "Background";
					goto IL_174F;
				case -114:
					typeId = -639;
					propertyName = "Text";
					goto IL_174F;
				case -113:
					typeId = -638;
					propertyName = "TextWrapping";
					goto IL_174F;
				case -112:
					typeId = -638;
					propertyName = "TextTrimming";
					goto IL_174F;
				case -111:
					typeId = -638;
					propertyName = "TextDecorations";
					goto IL_174F;
				case -110:
					typeId = -638;
					propertyName = "Text";
					goto IL_174F;
				case -109:
					typeId = -638;
					propertyName = "Foreground";
					goto IL_174F;
				case -108:
					typeId = -638;
					propertyName = "FontWeight";
					goto IL_174F;
				case -107:
					typeId = -638;
					propertyName = "FontStyle";
					goto IL_174F;
				case -106:
					typeId = -638;
					propertyName = "FontStretch";
					goto IL_174F;
				case -105:
					typeId = -638;
					propertyName = "FontSize";
					goto IL_174F;
				case -104:
					typeId = -638;
					propertyName = "FontFamily";
					goto IL_174F;
				case -103:
					typeId = -638;
					propertyName = "Background";
					goto IL_174F;
				case -102:
					typeId = -558;
					propertyName = "StrokeThickness";
					goto IL_174F;
				case -101:
					typeId = -558;
					propertyName = "Stroke";
					goto IL_174F;
				case -100:
					typeId = -558;
					propertyName = "Fill";
					goto IL_174F;
				case -99:
					typeId = -550;
					propertyName = "VerticalScrollBarVisibility";
					goto IL_174F;
				case -98:
					typeId = -550;
					propertyName = "HorizontalScrollBarVisibility";
					goto IL_174F;
				case -97:
					typeId = -550;
					propertyName = "CanContentScroll";
					goto IL_174F;
				case -96:
					typeId = -541;
					propertyName = "MinHeight";
					goto IL_174F;
				case -95:
					typeId = -541;
					propertyName = "MaxHeight";
					goto IL_174F;
				case -94:
					typeId = -541;
					propertyName = "Height";
					goto IL_174F;
				case -93:
					typeId = -485;
					propertyName = "PopupAnimation";
					goto IL_174F;
				case -92:
					typeId = -485;
					propertyName = "Placement";
					goto IL_174F;
				case -91:
					typeId = -485;
					propertyName = "IsOpen";
					goto IL_174F;
				case -90:
					typeId = -485;
					propertyName = "Child";
					goto IL_174F;
				case -89:
					typeId = -446;
					propertyName = "Figures";
					goto IL_174F;
				case -88:
					typeId = -443;
					propertyName = "Segments";
					goto IL_174F;
				case -87:
					typeId = -442;
					propertyName = "Data";
					goto IL_174F;
				case -86:
					typeId = -437;
					propertyName = "Background";
					goto IL_174F;
				case -85:
					typeId = -434;
					propertyName = "Content";
					goto IL_174F;
				case -84:
					typeId = -408;
					propertyName = "Children";
					goto IL_174F;
				case -83:
					typeId = -384;
					propertyName = "Children";
					goto IL_174F;
				case -82:
					typeId = -329;
					propertyName = "ItemsSource";
					goto IL_174F;
				case -81:
					typeId = -329;
					propertyName = "ItemsPanel";
					goto IL_174F;
				case -80:
					typeId = -329;
					propertyName = "ItemTemplateSelector";
					goto IL_174F;
				case -79:
					typeId = -329;
					propertyName = "ItemTemplate";
					goto IL_174F;
				case -78:
					typeId = -329;
					propertyName = "ItemContainerStyleSelector";
					goto IL_174F;
				case -77:
					typeId = -329;
					propertyName = "ItemContainerStyle";
					goto IL_174F;
				case -76:
					typeId = -281;
					propertyName = "Stretch";
					goto IL_174F;
				case -75:
					typeId = -281;
					propertyName = "Source";
					goto IL_174F;
				case -74:
					typeId = -273;
					propertyName = "NavigateUri";
					goto IL_174F;
				case -73:
					typeId = -270;
					propertyName = "HeaderTemplateSelector";
					goto IL_174F;
				case -72:
					typeId = -270;
					propertyName = "HeaderTemplate";
					goto IL_174F;
				case -71:
					typeId = -270;
					propertyName = "Header";
					goto IL_174F;
				case -70:
					typeId = -270;
					propertyName = "HasHeader";
					goto IL_174F;
				case -69:
					typeId = -269;
					propertyName = "HeaderTemplateSelector";
					goto IL_174F;
				case -68:
					typeId = -269;
					propertyName = "HeaderTemplate";
					goto IL_174F;
				case -67:
					typeId = -269;
					propertyName = "Header";
					goto IL_174F;
				case -66:
					typeId = -269;
					propertyName = "HasHeader";
					goto IL_174F;
				case -65:
					typeId = -259;
					propertyName = "Header";
					goto IL_174F;
				case -64:
					typeId = -254;
					propertyName = "RowSpan";
					goto IL_174F;
				case -63:
					typeId = -254;
					propertyName = "Row";
					goto IL_174F;
				case -62:
					typeId = -254;
					propertyName = "ColumnSpan";
					goto IL_174F;
				case -61:
					typeId = -254;
					propertyName = "Column";
					goto IL_174F;
				case -60:
					typeId = -251;
					propertyName = "GradientStops";
					goto IL_174F;
				case -59:
					typeId = -242;
					propertyName = "Children";
					goto IL_174F;
				case -58:
					typeId = -236;
					propertyName = "Children";
					goto IL_174F;
				case -57:
					typeId = -226;
					propertyName = "Width";
					goto IL_174F;
				case -56:
					typeId = -226;
					propertyName = "VerticalAlignment";
					goto IL_174F;
				case -55:
					typeId = -226;
					propertyName = "Style";
					goto IL_174F;
				case -54:
					typeId = -226;
					propertyName = "Name";
					goto IL_174F;
				case -53:
					typeId = -226;
					propertyName = "MinWidth";
					goto IL_174F;
				case -52:
					typeId = -226;
					propertyName = "MinHeight";
					goto IL_174F;
				case -51:
					typeId = -226;
					propertyName = "MaxWidth";
					goto IL_174F;
				case -50:
					typeId = -226;
					propertyName = "MaxHeight";
					goto IL_174F;
				case -49:
					typeId = -226;
					propertyName = "Margin";
					goto IL_174F;
				case -48:
					typeId = -226;
					propertyName = "HorizontalAlignment";
					goto IL_174F;
				case -47:
					typeId = -226;
					propertyName = "Height";
					goto IL_174F;
				case -46:
					typeId = -226;
					propertyName = "FlowDirection";
					goto IL_174F;
				case -45:
					typeId = -225;
					propertyName = "Style";
					goto IL_174F;
				case -44:
					typeId = -212;
					propertyName = "Document";
					goto IL_174F;
				case -43:
					typeId = -211;
					propertyName = "Document";
					goto IL_174F;
				case -42:
					typeId = -183;
					propertyName = "Children";
					goto IL_174F;
				case -41:
					typeId = -167;
					propertyName = "Document";
					goto IL_174F;
				case -40:
					typeId = -163;
					propertyName = "LastChildFill";
					goto IL_174F;
				case -39:
					typeId = -163;
					propertyName = "Dock";
					goto IL_174F;
				case -38:
					typeId = -107;
					propertyName = "VerticalContentAlignment";
					goto IL_174F;
				case -37:
					typeId = -107;
					propertyName = "Template";
					goto IL_174F;
				case -36:
					typeId = -107;
					propertyName = "TabIndex";
					goto IL_174F;
				case -35:
					typeId = -107;
					propertyName = "Padding";
					goto IL_174F;
				case -34:
					typeId = -107;
					propertyName = "IsTabStop";
					goto IL_174F;
				case -33:
					typeId = -107;
					propertyName = "HorizontalContentAlignment";
					goto IL_174F;
				case -32:
					typeId = -107;
					propertyName = "Foreground";
					goto IL_174F;
				case -31:
					typeId = -107;
					propertyName = "FontWeight";
					goto IL_174F;
				case -30:
					typeId = -107;
					propertyName = "FontStyle";
					goto IL_174F;
				case -29:
					typeId = -107;
					propertyName = "FontStretch";
					goto IL_174F;
				case -28:
					typeId = -107;
					propertyName = "FontSize";
					goto IL_174F;
				case -27:
					typeId = -107;
					propertyName = "FontFamily";
					goto IL_174F;
				case -26:
					typeId = -107;
					propertyName = "BorderThickness";
					goto IL_174F;
				case -25:
					typeId = -107;
					propertyName = "BorderBrush";
					goto IL_174F;
				case -24:
					typeId = -107;
					propertyName = "Background";
					goto IL_174F;
				case -23:
					typeId = -102;
					propertyName = "RecognizesAccessKey";
					goto IL_174F;
				case -22:
					typeId = -102;
					propertyName = "ContentTemplateSelector";
					goto IL_174F;
				case -21:
					typeId = -102;
					propertyName = "ContentTemplate";
					goto IL_174F;
				case -20:
					typeId = -102;
					propertyName = "ContentSource";
					goto IL_174F;
				case -19:
					typeId = -102;
					propertyName = "Content";
					goto IL_174F;
				case -18:
					typeId = -101;
					propertyName = "Focusable";
					goto IL_174F;
				case -17:
					typeId = -100;
					propertyName = "HasContent";
					goto IL_174F;
				case -16:
					typeId = -100;
					propertyName = "ContentTemplateSelector";
					goto IL_174F;
				case -15:
					typeId = -100;
					propertyName = "ContentTemplate";
					goto IL_174F;
				case -14:
					typeId = -100;
					propertyName = "Content";
					goto IL_174F;
				case -13:
					typeId = -90;
					propertyName = "Width";
					goto IL_174F;
				case -12:
					typeId = -90;
					propertyName = "MinWidth";
					goto IL_174F;
				case -11:
					typeId = -90;
					propertyName = "MaxWidth";
					goto IL_174F;
				case -10:
					typeId = -56;
					propertyName = "IsPressed";
					goto IL_174F;
				case -9:
					typeId = -56;
					propertyName = "CommandTarget";
					goto IL_174F;
				case -8:
					typeId = -56;
					propertyName = "CommandParameter";
					goto IL_174F;
				case -7:
					typeId = -56;
					propertyName = "Command";
					goto IL_174F;
				case -6:
					typeId = -50;
					propertyName = "BorderThickness";
					goto IL_174F;
				case -5:
					typeId = -50;
					propertyName = "BorderBrush";
					goto IL_174F;
				case -4:
					typeId = -50;
					propertyName = "Background";
					goto IL_174F;
				case -3:
					typeId = -28;
					propertyName = "Children";
					goto IL_174F;
				case -2:
					typeId = -17;
					propertyName = "Storyboard";
					goto IL_174F;
				case -1:
					typeId = -1;
					propertyName = "Text";
					goto IL_174F;
				}
				typeId = short.MinValue;
				propertyName = null;
				IL_174F:
				return propertyName != null;
			}

			// Token: 0x06007F1F RID: 32543 RVA: 0x00240FBC File Offset: 0x0023F1BC
			public static string GetKnownString(short stringId)
			{
				string result;
				if (stringId != -2)
				{
					if (stringId == -1)
					{
						result = "Name";
					}
					else
					{
						result = null;
					}
				}
				else
				{
					result = "Uid";
				}
				return result;
			}

			// Token: 0x06007F20 RID: 32544 RVA: 0x00240FE8 File Offset: 0x0023F1E8
			public static Type GetTypeConverterForKnownProperty(short propertyId)
			{
				short num;
				string propName;
				if (!Baml2006SchemaContext.KnownTypes.GetKnownProperty(propertyId, out num, out propName))
				{
					return null;
				}
				KnownElements knownTypeConverterIdForProperty = System.Windows.Markup.KnownTypes.GetKnownTypeConverterIdForProperty((KnownElements)(-1 * num), propName);
				if (knownTypeConverterIdForProperty == KnownElements.UnknownElement)
				{
					return null;
				}
				return Baml2006SchemaContext.KnownTypes.GetKnownType((short)((KnownElements)(-1) * knownTypeConverterIdForProperty));
			}

			// Token: 0x06007F21 RID: 32545 RVA: 0x0024101C File Offset: 0x0023F21C
			public static bool? IsKnownPropertyAttachable(short propertyId)
			{
				if (propertyId >= 0 || propertyId < -268)
				{
					return null;
				}
				if (propertyId <= -61)
				{
					switch (propertyId)
					{
					case -121:
						return null;
					case -120:
						return null;
					case -119:
						return null;
					case -118:
						return null;
					case -117:
						return null;
					case -116:
						return null;
					case -115:
					case -114:
					case -113:
					case -112:
					case -111:
					case -110:
					case -103:
					case -102:
					case -101:
					case -100:
						break;
					case -109:
						return null;
					case -108:
						return null;
					case -107:
						return null;
					case -106:
						return null;
					case -105:
						return null;
					case -104:
						return null;
					case -99:
						return null;
					case -98:
						return null;
					case -97:
						return null;
					default:
						switch (propertyId)
						{
						case -64:
							return new bool?(true);
						case -63:
							return new bool?(true);
						case -62:
							return new bool?(true);
						case -61:
							return new bool?(true);
						}
						break;
					}
				}
				else
				{
					if (propertyId == -46)
					{
						return null;
					}
					if (propertyId == -39)
					{
						return new bool?(true);
					}
				}
				return new bool?(false);
			}

			// Token: 0x04003CDF RID: 15583
			public const short BooleanConverter = 46;

			// Token: 0x04003CE0 RID: 15584
			public const short DependencyPropertyConverter = 137;

			// Token: 0x04003CE1 RID: 15585
			public const short EnumConverter = 195;

			// Token: 0x04003CE2 RID: 15586
			public const short StringConverter = 615;

			// Token: 0x04003CE3 RID: 15587
			public const short XamlBrushSerializer = 744;

			// Token: 0x04003CE4 RID: 15588
			public const short XamlInt32CollectionSerializer = 745;

			// Token: 0x04003CE5 RID: 15589
			public const short XamlPathDataSerializer = 746;

			// Token: 0x04003CE6 RID: 15590
			public const short XamlPoint3DCollectionSerializer = 747;

			// Token: 0x04003CE7 RID: 15591
			public const short XamlPointCollectionSerializer = 748;

			// Token: 0x04003CE8 RID: 15592
			public const short XamlVector3DCollectionSerializer = 752;

			// Token: 0x04003CE9 RID: 15593
			public const short MaxKnownType = 759;

			// Token: 0x04003CEA RID: 15594
			public const short MaxKnownProperty = 268;

			// Token: 0x04003CEB RID: 15595
			public const short MinKnownProperty = -268;

			// Token: 0x04003CEC RID: 15596
			public const short VisualTreeKnownPropertyId = -174;
		}

		// Token: 0x02000845 RID: 2117
		private sealed class BamlAssembly
		{
			// Token: 0x06007F22 RID: 32546 RVA: 0x002411A8 File Offset: 0x0023F3A8
			public BamlAssembly(string name)
			{
				if (name == null)
				{
					throw new ArgumentNullException("name");
				}
				this.Name = name;
				this.Assembly = null;
			}

			// Token: 0x06007F23 RID: 32547 RVA: 0x002411CC File Offset: 0x0023F3CC
			public BamlAssembly(Assembly assembly)
			{
				if (assembly == null)
				{
					throw new ArgumentNullException("assembly");
				}
				this.Name = null;
				this.Assembly = assembly;
			}

			// Token: 0x04003CED RID: 15597
			public readonly string Name;

			// Token: 0x04003CEE RID: 15598
			internal Assembly Assembly;
		}

		// Token: 0x02000846 RID: 2118
		private sealed class BamlType
		{
			// Token: 0x06007F24 RID: 32548 RVA: 0x002411F6 File Offset: 0x0023F3F6
			public BamlType(short assemblyId, string name)
			{
				if (name == null)
				{
					throw new ArgumentNullException("name");
				}
				this.AssemblyId = assemblyId;
				this.Name = name;
			}

			// Token: 0x04003CEF RID: 15599
			public short AssemblyId;

			// Token: 0x04003CF0 RID: 15600
			public string Name;

			// Token: 0x04003CF1 RID: 15601
			public Baml2006SchemaContext.TypeInfoFlags Flags;

			// Token: 0x04003CF2 RID: 15602
			public string ClrNamespace;
		}

		// Token: 0x02000847 RID: 2119
		private sealed class BamlProperty
		{
			// Token: 0x06007F25 RID: 32549 RVA: 0x0024121A File Offset: 0x0023F41A
			public BamlProperty(short declaringTypeId, string name)
			{
				if (name == null)
				{
					throw new ArgumentNullException("name");
				}
				this.DeclaringTypeId = declaringTypeId;
				this.Name = name;
			}

			// Token: 0x04003CF3 RID: 15603
			public readonly short DeclaringTypeId;

			// Token: 0x04003CF4 RID: 15604
			public readonly string Name;
		}

		// Token: 0x02000848 RID: 2120
		[Flags]
		internal enum TypeInfoFlags : byte
		{
			// Token: 0x04003CF6 RID: 15606
			Internal = 1,
			// Token: 0x04003CF7 RID: 15607
			UnusedTwo = 2,
			// Token: 0x04003CF8 RID: 15608
			UnusedThree = 4
		}
	}
}
