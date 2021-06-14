using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Xaml;
using System.Xaml.Schema;

namespace System.Windows.Baml2006
{
	// Token: 0x02000176 RID: 374
	internal class WpfXamlType : XamlType
	{
		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x060015D0 RID: 5584 RVA: 0x0006A9DA File Offset: 0x00068BDA
		// (set) Token: 0x060015D1 RID: 5585 RVA: 0x0006A9E8 File Offset: 0x00068BE8
		private bool IsBamlScenario
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

		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x060015D2 RID: 5586 RVA: 0x0006A9F7 File Offset: 0x00068BF7
		// (set) Token: 0x060015D3 RID: 5587 RVA: 0x0006AA05 File Offset: 0x00068C05
		private bool UseV3Rules
		{
			get
			{
				return WpfXamlType.GetFlag(ref this._bitField, 2);
			}
			set
			{
				WpfXamlType.SetFlag(ref this._bitField, 2, value);
			}
		}

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x060015D4 RID: 5588 RVA: 0x0006AA14 File Offset: 0x00068C14
		protected ConcurrentDictionary<string, XamlMember> Members
		{
			get
			{
				if (this._members == null)
				{
					this._members = new ConcurrentDictionary<string, XamlMember>(1, 11);
				}
				return this._members;
			}
		}

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x060015D5 RID: 5589 RVA: 0x0006AA32 File Offset: 0x00068C32
		protected ConcurrentDictionary<string, XamlMember> AttachableMembers
		{
			get
			{
				if (this._attachableMembers == null)
				{
					this._attachableMembers = new ConcurrentDictionary<string, XamlMember>(1, 11);
				}
				return this._attachableMembers;
			}
		}

		// Token: 0x060015D6 RID: 5590 RVA: 0x0006AA50 File Offset: 0x00068C50
		public WpfXamlType(Type type, XamlSchemaContext schema, bool isBamlScenario, bool useV3Rules) : base(type, schema)
		{
			this.IsBamlScenario = isBamlScenario;
			this.UseV3Rules = useV3Rules;
		}

		// Token: 0x060015D7 RID: 5591 RVA: 0x0006AA6C File Offset: 0x00068C6C
		protected override XamlMember LookupContentProperty()
		{
			XamlMember xamlMember = base.LookupContentProperty();
			WpfXamlMember wpfXamlMember = xamlMember as WpfXamlMember;
			if (wpfXamlMember != null)
			{
				xamlMember = wpfXamlMember.AsContentProperty;
			}
			return xamlMember;
		}

		// Token: 0x060015D8 RID: 5592 RVA: 0x0006AA98 File Offset: 0x00068C98
		protected override bool LookupIsNameScope()
		{
			if (base.UnderlyingType == typeof(ResourceDictionary))
			{
				return false;
			}
			if (typeof(ResourceDictionary).IsAssignableFrom(base.UnderlyingType))
			{
				InterfaceMapping interfaceMap = base.UnderlyingType.GetInterfaceMap(typeof(INameScope));
				foreach (MethodInfo methodInfo in interfaceMap.TargetMethods)
				{
					if (methodInfo.Name.Contains("RegisterName"))
					{
						return methodInfo.DeclaringType != typeof(ResourceDictionary);
					}
				}
				return false;
			}
			return base.LookupIsNameScope();
		}

		// Token: 0x060015D9 RID: 5593 RVA: 0x0006AB34 File Offset: 0x00068D34
		private XamlMember FindMember(string name, bool isAttached, bool skipReadOnlyCheck)
		{
			XamlMember xamlMember = this.FindKnownMember(name, isAttached);
			if (xamlMember != null)
			{
				return xamlMember;
			}
			xamlMember = this.FindDependencyPropertyBackedProperty(name, isAttached, skipReadOnlyCheck);
			if (xamlMember != null)
			{
				return xamlMember;
			}
			xamlMember = this.FindRoutedEventBackedProperty(name, isAttached, skipReadOnlyCheck);
			if (xamlMember != null)
			{
				return xamlMember;
			}
			if (isAttached)
			{
				xamlMember = base.LookupAttachableMember(name);
			}
			else
			{
				xamlMember = base.LookupMember(name, skipReadOnlyCheck);
			}
			WpfKnownType wpfXamlType;
			if (xamlMember != null && (wpfXamlType = (xamlMember.DeclaringType as WpfKnownType)) != null)
			{
				XamlMember xamlMember2 = WpfXamlType.FindKnownMember(wpfXamlType, name, isAttached);
				if (xamlMember2 != null)
				{
					return xamlMember2;
				}
			}
			return xamlMember;
		}

		// Token: 0x060015DA RID: 5594 RVA: 0x0006ABC8 File Offset: 0x00068DC8
		protected override XamlMember LookupMember(string name, bool skipReadOnlyCheck)
		{
			return this.FindMember(name, false, skipReadOnlyCheck);
		}

		// Token: 0x060015DB RID: 5595 RVA: 0x0006ABD3 File Offset: 0x00068DD3
		protected override XamlMember LookupAttachableMember(string name)
		{
			return this.FindMember(name, true, false);
		}

		// Token: 0x060015DC RID: 5596 RVA: 0x0006ABE0 File Offset: 0x00068DE0
		protected override IEnumerable<XamlMember> LookupAllMembers()
		{
			List<XamlMember> list = new List<XamlMember>();
			IEnumerable<XamlMember> enumerable = base.LookupAllMembers();
			foreach (XamlMember xamlMember in enumerable)
			{
				XamlMember xamlMember2 = xamlMember;
				if (!(xamlMember2 is WpfXamlMember))
				{
					xamlMember2 = base.GetMember(xamlMember2.Name);
				}
				list.Add(xamlMember2);
			}
			return list;
		}

		// Token: 0x060015DD RID: 5597 RVA: 0x0006AC54 File Offset: 0x00068E54
		private XamlMember FindKnownMember(string name, bool isAttachable)
		{
			XamlType xamlType = this;
			if (this is WpfKnownType)
			{
				XamlMember xamlMember;
				for (;;)
				{
					WpfXamlType wpfXamlType = xamlType as WpfXamlType;
					xamlMember = WpfXamlType.FindKnownMember(wpfXamlType, name, isAttachable);
					if (xamlMember != null)
					{
						break;
					}
					xamlType = xamlType.BaseType;
					if (!(xamlType != null))
					{
						goto IL_35;
					}
				}
				return xamlMember;
			}
			IL_35:
			return null;
		}

		// Token: 0x060015DE RID: 5598 RVA: 0x0006AC98 File Offset: 0x00068E98
		private XamlMember FindRoutedEventBackedProperty(string name, bool isAttachable, bool skipReadOnlyCheck)
		{
			RoutedEvent routedEventFromName = EventManager.GetRoutedEventFromName(name, base.UnderlyingType);
			XamlMember xamlMember = null;
			if (routedEventFromName == null)
			{
				return xamlMember;
			}
			WpfXamlType wpfXamlType;
			if (this.IsBamlScenario)
			{
				wpfXamlType = (System.Windows.Markup.XamlReader.BamlSharedSchemaContext.GetXamlType(routedEventFromName.OwnerType) as WpfXamlType);
			}
			else
			{
				wpfXamlType = (System.Windows.Markup.XamlReader.GetWpfSchemaContext().GetXamlType(routedEventFromName.OwnerType) as WpfXamlType);
			}
			if (wpfXamlType != null)
			{
				xamlMember = WpfXamlType.FindKnownMember(wpfXamlType, name, isAttachable);
			}
			if (this.IsBamlScenario)
			{
				xamlMember = new WpfXamlMember(routedEventFromName, isAttachable);
			}
			else if (isAttachable)
			{
				xamlMember = this.GetAttachedRoutedEvent(name, routedEventFromName);
				if (xamlMember == null)
				{
					xamlMember = this.GetRoutedEvent(name, routedEventFromName, skipReadOnlyCheck);
				}
				if (xamlMember == null)
				{
					xamlMember = new WpfXamlMember(routedEventFromName, true);
				}
			}
			else
			{
				xamlMember = this.GetRoutedEvent(name, routedEventFromName, skipReadOnlyCheck);
				if (xamlMember == null)
				{
					xamlMember = this.GetAttachedRoutedEvent(name, routedEventFromName);
				}
				if (xamlMember == null)
				{
					xamlMember = new WpfXamlMember(routedEventFromName, false);
				}
			}
			if (this.Members.TryAdd(name, xamlMember))
			{
				return xamlMember;
			}
			return this.Members[name];
		}

		// Token: 0x060015DF RID: 5599 RVA: 0x0006AD94 File Offset: 0x00068F94
		private XamlMember FindDependencyPropertyBackedProperty(string name, bool isAttachable, bool skipReadOnlyCheck)
		{
			XamlMember xamlMember = null;
			DependencyProperty dependencyProperty;
			if ((dependencyProperty = DependencyProperty.FromName(name, base.UnderlyingType)) != null)
			{
				WpfXamlType wpfXamlType;
				if (this.IsBamlScenario)
				{
					wpfXamlType = (System.Windows.Markup.XamlReader.BamlSharedSchemaContext.GetXamlType(dependencyProperty.OwnerType) as WpfXamlType);
				}
				else
				{
					wpfXamlType = (System.Windows.Markup.XamlReader.GetWpfSchemaContext().GetXamlType(dependencyProperty.OwnerType) as WpfXamlType);
				}
				if (wpfXamlType != null)
				{
					xamlMember = WpfXamlType.FindKnownMember(wpfXamlType, name, isAttachable);
				}
				if (xamlMember == null)
				{
					if (this.IsBamlScenario)
					{
						xamlMember = new WpfXamlMember(dependencyProperty, isAttachable);
					}
					else if (isAttachable)
					{
						xamlMember = this.GetAttachedDependencyProperty(name, dependencyProperty);
						if (xamlMember == null)
						{
							return null;
						}
					}
					else
					{
						xamlMember = this.GetRegularDependencyProperty(name, dependencyProperty, skipReadOnlyCheck);
						if (xamlMember == null)
						{
							xamlMember = this.GetAttachedDependencyProperty(name, dependencyProperty);
						}
						if (xamlMember == null)
						{
							xamlMember = new WpfXamlMember(dependencyProperty, false);
						}
					}
					return this.CacheAndReturnXamlMember(xamlMember);
				}
			}
			return xamlMember;
		}

		// Token: 0x060015E0 RID: 5600 RVA: 0x0006AE68 File Offset: 0x00069068
		private XamlMember CacheAndReturnXamlMember(XamlMember xamlMember)
		{
			if (!xamlMember.IsAttachable || this.IsBamlScenario)
			{
				if (this.Members.TryAdd(xamlMember.Name, xamlMember))
				{
					return xamlMember;
				}
				return this.Members[xamlMember.Name];
			}
			else
			{
				if (this.AttachableMembers.TryAdd(xamlMember.Name, xamlMember))
				{
					return xamlMember;
				}
				return this.AttachableMembers[xamlMember.Name];
			}
		}

		// Token: 0x060015E1 RID: 5601 RVA: 0x0006AED4 File Offset: 0x000690D4
		private XamlMember GetAttachedRoutedEvent(string name, RoutedEvent re)
		{
			XamlMember xamlMember = base.LookupAttachableMember(name);
			if (xamlMember != null)
			{
				return new WpfXamlMember(re, (MethodInfo)xamlMember.UnderlyingMember, base.SchemaContext, this.UseV3Rules);
			}
			return null;
		}

		// Token: 0x060015E2 RID: 5602 RVA: 0x0006AF14 File Offset: 0x00069114
		private XamlMember GetRoutedEvent(string name, RoutedEvent re, bool skipReadOnlyCheck)
		{
			XamlMember xamlMember = base.LookupMember(name, skipReadOnlyCheck);
			if (xamlMember != null)
			{
				return new WpfXamlMember(re, (EventInfo)xamlMember.UnderlyingMember, base.SchemaContext, this.UseV3Rules);
			}
			return null;
		}

		// Token: 0x060015E3 RID: 5603 RVA: 0x0006AF54 File Offset: 0x00069154
		private XamlMember GetAttachedDependencyProperty(string name, DependencyProperty property)
		{
			XamlMember xamlMember = base.LookupAttachableMember(name);
			if (xamlMember != null)
			{
				return new WpfXamlMember(property, xamlMember.Invoker.UnderlyingGetter, xamlMember.Invoker.UnderlyingSetter, base.SchemaContext, this.UseV3Rules);
			}
			return null;
		}

		// Token: 0x060015E4 RID: 5604 RVA: 0x0006AF9C File Offset: 0x0006919C
		private XamlMember GetRegularDependencyProperty(string name, DependencyProperty property, bool skipReadOnlyCheck)
		{
			XamlMember xamlMember = base.LookupMember(name, skipReadOnlyCheck);
			if (!(xamlMember != null))
			{
				return null;
			}
			PropertyInfo propertyInfo = xamlMember.UnderlyingMember as PropertyInfo;
			if (propertyInfo != null)
			{
				return new WpfXamlMember(property, propertyInfo, base.SchemaContext, this.UseV3Rules);
			}
			throw new NotImplementedException();
		}

		// Token: 0x060015E5 RID: 5605 RVA: 0x0006AFEC File Offset: 0x000691EC
		private static XamlMember FindKnownMember(WpfXamlType wpfXamlType, string name, bool isAttachable)
		{
			XamlMember xamlMember = null;
			if (!isAttachable || wpfXamlType.IsBamlScenario)
			{
				if (wpfXamlType._members != null && wpfXamlType.Members.TryGetValue(name, out xamlMember))
				{
					return xamlMember;
				}
			}
			else if (wpfXamlType._attachableMembers != null && wpfXamlType.AttachableMembers.TryGetValue(name, out xamlMember))
			{
				return xamlMember;
			}
			WpfKnownType wpfKnownType = wpfXamlType as WpfKnownType;
			if (wpfKnownType != null)
			{
				if (!isAttachable || wpfXamlType.IsBamlScenario)
				{
					xamlMember = System.Windows.Markup.XamlReader.BamlSharedSchemaContext.CreateKnownMember(wpfXamlType.Name, name);
				}
				if (isAttachable || (xamlMember == null && wpfXamlType.IsBamlScenario))
				{
					xamlMember = System.Windows.Markup.XamlReader.BamlSharedSchemaContext.CreateKnownAttachableMember(wpfXamlType.Name, name);
				}
				if (xamlMember != null)
				{
					return wpfKnownType.CacheAndReturnXamlMember(xamlMember);
				}
			}
			return null;
		}

		// Token: 0x060015E6 RID: 5606 RVA: 0x0006B0A0 File Offset: 0x000692A0
		protected override XamlCollectionKind LookupCollectionKind()
		{
			if (!this.UseV3Rules)
			{
				return base.LookupCollectionKind();
			}
			if (base.UnderlyingType.IsArray)
			{
				return XamlCollectionKind.Array;
			}
			if (typeof(IDictionary).IsAssignableFrom(base.UnderlyingType))
			{
				return XamlCollectionKind.Dictionary;
			}
			if (typeof(IList).IsAssignableFrom(base.UnderlyingType))
			{
				return XamlCollectionKind.Collection;
			}
			if (typeof(DocumentReferenceCollection).IsAssignableFrom(base.UnderlyingType) || typeof(PageContentCollection).IsAssignableFrom(base.UnderlyingType))
			{
				return XamlCollectionKind.Collection;
			}
			if (typeof(ICollection<XmlNamespaceMapping>).IsAssignableFrom(base.UnderlyingType) && this.IsXmlNamespaceMappingCollection)
			{
				return XamlCollectionKind.Collection;
			}
			return XamlCollectionKind.None;
		}

		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x060015E7 RID: 5607 RVA: 0x0006B152 File Offset: 0x00069352
		private bool IsXmlNamespaceMappingCollection
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				return typeof(XmlNamespaceMappingCollection).IsAssignableFrom(base.UnderlyingType);
			}
		}

		// Token: 0x060015E8 RID: 5608 RVA: 0x0006B169 File Offset: 0x00069369
		internal XamlMember FindBaseXamlMember(string name, bool isAttachable)
		{
			if (isAttachable)
			{
				return base.LookupAttachableMember(name);
			}
			return base.LookupMember(name, true);
		}

		// Token: 0x060015E9 RID: 5609 RVA: 0x0006B17E File Offset: 0x0006937E
		internal static bool GetFlag(ref byte bitField, byte typeBit)
		{
			return (bitField & typeBit) > 0;
		}

		// Token: 0x060015EA RID: 5610 RVA: 0x0006B187 File Offset: 0x00069387
		internal static void SetFlag(ref byte bitField, byte typeBit, bool value)
		{
			if (value)
			{
				bitField |= typeBit;
				return;
			}
			bitField &= ~typeBit;
		}

		// Token: 0x04001278 RID: 4728
		private const int ConcurrencyLevel = 1;

		// Token: 0x04001279 RID: 4729
		private const int Capacity = 11;

		// Token: 0x0400127A RID: 4730
		private ConcurrentDictionary<string, XamlMember> _attachableMembers;

		// Token: 0x0400127B RID: 4731
		private ConcurrentDictionary<string, XamlMember> _members;

		// Token: 0x0400127C RID: 4732
		protected byte _bitField;

		// Token: 0x02000852 RID: 2130
		[Flags]
		private enum BoolTypeBits
		{
			// Token: 0x04004067 RID: 16487
			BamlScenerio = 1,
			// Token: 0x04004068 RID: 16488
			V3Rules = 2
		}
	}
}
