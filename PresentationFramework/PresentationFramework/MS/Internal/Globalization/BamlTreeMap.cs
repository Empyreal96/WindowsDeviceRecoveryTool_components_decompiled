using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Markup;
using System.Windows.Markup.Localizer;

namespace MS.Internal.Globalization
{
	// Token: 0x0200069C RID: 1692
	internal class BamlTreeMap
	{
		// Token: 0x06006E20 RID: 28192 RVA: 0x001FB39A File Offset: 0x001F959A
		internal BamlTreeMap(BamlLocalizer localizer, BamlTree tree, BamlLocalizabilityResolver resolver, TextReader comments)
		{
			this._tree = tree;
			this._resolver = new InternalBamlLocalizabilityResolver(localizer, resolver, comments);
			this._localizableResourceBuilder = new LocalizableResourceBuilder(this._resolver);
		}

		// Token: 0x17001A24 RID: 6692
		// (get) Token: 0x06006E21 RID: 28193 RVA: 0x001FB3C9 File Offset: 0x001F95C9
		internal BamlLocalizationDictionary LocalizationDictionary
		{
			get
			{
				this.EnsureMap();
				return this._localizableResources;
			}
		}

		// Token: 0x17001A25 RID: 6693
		// (get) Token: 0x06006E22 RID: 28194 RVA: 0x001FB3D7 File Offset: 0x001F95D7
		internal InternalBamlLocalizabilityResolver Resolver
		{
			get
			{
				return this._resolver;
			}
		}

		// Token: 0x06006E23 RID: 28195 RVA: 0x001FB3DF File Offset: 0x001F95DF
		internal BamlTreeNode MapKeyToBamlTreeNode(BamlLocalizableResourceKey key, BamlTree tree)
		{
			if (this._keyToBamlNodeIndexMap.Contains(key))
			{
				return tree[(int)this._keyToBamlNodeIndexMap[key]];
			}
			return null;
		}

		// Token: 0x06006E24 RID: 28196 RVA: 0x001FB408 File Offset: 0x001F9608
		internal BamlStartElementNode MapUidToBamlTreeElementNode(string uid, BamlTree tree)
		{
			if (this._uidToBamlNodeIndexMap.Contains(uid))
			{
				return tree[(int)this._uidToBamlNodeIndexMap[uid]] as BamlStartElementNode;
			}
			return null;
		}

		// Token: 0x06006E25 RID: 28197 RVA: 0x001FB438 File Offset: 0x001F9638
		internal void EnsureMap()
		{
			if (this._localizableResources != null)
			{
				return;
			}
			this._resolver.InitLocalizabilityCache();
			this._keyToBamlNodeIndexMap = new Hashtable(this._tree.Size);
			this._uidToBamlNodeIndexMap = new Hashtable(this._tree.Size / 2);
			this._localizableResources = new BamlLocalizationDictionary();
			for (int i = 0; i < this._tree.Size; i++)
			{
				BamlTreeNode bamlTreeNode = this._tree[i];
				if (!bamlTreeNode.Unidentifiable)
				{
					if (bamlTreeNode.NodeType == BamlNodeType.StartElement)
					{
						BamlStartElementNode bamlStartElementNode = (BamlStartElementNode)bamlTreeNode;
						this._resolver.AddClassAndAssembly(bamlStartElementNode.TypeFullName, bamlStartElementNode.AssemblyName);
					}
					BamlLocalizableResourceKey key = BamlTreeMap.GetKey(bamlTreeNode);
					if (key != null)
					{
						if (bamlTreeNode.NodeType == BamlNodeType.StartElement)
						{
							if (this._uidToBamlNodeIndexMap.ContainsKey(key.Uid))
							{
								this._resolver.RaiseErrorNotifyEvent(new BamlLocalizerErrorNotifyEventArgs(key, BamlLocalizerError.DuplicateUid));
								bamlTreeNode.Unidentifiable = true;
								if (bamlTreeNode.Children == null)
								{
									goto IL_1AB;
								}
								using (List<BamlTreeNode>.Enumerator enumerator = bamlTreeNode.Children.GetEnumerator())
								{
									while (enumerator.MoveNext())
									{
										BamlTreeNode bamlTreeNode2 = enumerator.Current;
										if (bamlTreeNode2.NodeType != BamlNodeType.StartElement)
										{
											bamlTreeNode2.Unidentifiable = true;
										}
									}
									goto IL_1AB;
								}
							}
							this._uidToBamlNodeIndexMap.Add(key.Uid, i);
						}
						this._keyToBamlNodeIndexMap.Add(key, i);
						if (this._localizableResources.RootElementKey == null && bamlTreeNode.NodeType == BamlNodeType.StartElement && bamlTreeNode.Parent != null && bamlTreeNode.Parent.NodeType == BamlNodeType.StartDocument)
						{
							this._localizableResources.SetRootElementKey(key);
						}
						BamlLocalizableResource bamlLocalizableResource = this._localizableResourceBuilder.BuildFromNode(key, bamlTreeNode);
						if (bamlLocalizableResource != null)
						{
							this._localizableResources.Add(key, bamlLocalizableResource);
						}
					}
				}
				IL_1AB:;
			}
			this._resolver.ReleaseLocalizabilityCache();
		}

		// Token: 0x06006E26 RID: 28198 RVA: 0x001FB620 File Offset: 0x001F9820
		internal static BamlLocalizableResourceKey GetKey(BamlTreeNode node)
		{
			BamlLocalizableResourceKey result = null;
			BamlNodeType nodeType = node.NodeType;
			if (nodeType != BamlNodeType.StartElement)
			{
				if (nodeType != BamlNodeType.Property)
				{
					if (nodeType == BamlNodeType.LiteralContent)
					{
						BamlLiteralContentNode bamlLiteralContentNode = (BamlLiteralContentNode)node;
						BamlStartElementNode bamlStartElementNode = (BamlStartElementNode)node.Parent;
						if (bamlStartElementNode.Uid != null)
						{
							result = new BamlLocalizableResourceKey(bamlStartElementNode.Uid, bamlStartElementNode.TypeFullName, "$LiteralContent", bamlStartElementNode.AssemblyName);
						}
					}
				}
				else
				{
					BamlPropertyNode bamlPropertyNode = (BamlPropertyNode)node;
					BamlStartElementNode bamlStartElementNode2 = (BamlStartElementNode)bamlPropertyNode.Parent;
					if (bamlStartElementNode2.Uid != null)
					{
						string uid;
						if (bamlPropertyNode.Index <= 0)
						{
							uid = bamlStartElementNode2.Uid;
						}
						else
						{
							uid = string.Format(TypeConverterHelper.InvariantEnglishUS, "{0}.{1}_{2}", new object[]
							{
								bamlStartElementNode2.Uid,
								bamlPropertyNode.PropertyName,
								bamlPropertyNode.Index
							});
						}
						result = new BamlLocalizableResourceKey(uid, bamlPropertyNode.OwnerTypeFullName, bamlPropertyNode.PropertyName, bamlPropertyNode.AssemblyName);
					}
				}
			}
			else
			{
				BamlStartElementNode bamlStartElementNode3 = (BamlStartElementNode)node;
				if (bamlStartElementNode3.Uid != null)
				{
					result = new BamlLocalizableResourceKey(bamlStartElementNode3.Uid, bamlStartElementNode3.TypeFullName, "$Content", bamlStartElementNode3.AssemblyName);
				}
			}
			return result;
		}

		// Token: 0x04003633 RID: 13875
		private Hashtable _keyToBamlNodeIndexMap;

		// Token: 0x04003634 RID: 13876
		private Hashtable _uidToBamlNodeIndexMap;

		// Token: 0x04003635 RID: 13877
		private LocalizableResourceBuilder _localizableResourceBuilder;

		// Token: 0x04003636 RID: 13878
		private BamlLocalizationDictionary _localizableResources;

		// Token: 0x04003637 RID: 13879
		private BamlTree _tree;

		// Token: 0x04003638 RID: 13880
		private InternalBamlLocalizabilityResolver _resolver;
	}
}
