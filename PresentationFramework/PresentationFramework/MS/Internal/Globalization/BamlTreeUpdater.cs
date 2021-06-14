using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows.Markup;
using System.Windows.Markup.Localizer;
using System.Xml;

namespace MS.Internal.Globalization
{
	// Token: 0x020006B4 RID: 1716
	internal static class BamlTreeUpdater
	{
		// Token: 0x06006EAF RID: 28335 RVA: 0x001FC714 File Offset: 0x001FA914
		internal static void UpdateTree(BamlTree tree, BamlTreeMap treeMap, BamlLocalizationDictionary dictionary)
		{
			if (dictionary.Count <= 0)
			{
				return;
			}
			BamlTreeUpdater.BamlTreeUpdateMap treeMap2 = new BamlTreeUpdater.BamlTreeUpdateMap(treeMap, tree);
			BamlTreeUpdater.CreateMissingBamlTreeNode(dictionary, treeMap2);
			BamlLocalizationDictionaryEnumerator enumerator = dictionary.GetEnumerator();
			ArrayList arrayList = new ArrayList();
			while (enumerator.MoveNext())
			{
				if (!BamlTreeUpdater.ApplyChangeToBamlTree(enumerator.Key, enumerator.Value, treeMap2))
				{
					arrayList.Add(enumerator.Entry);
				}
			}
			for (int i = 0; i < arrayList.Count; i++)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)arrayList[i];
				BamlTreeUpdater.ApplyChangeToBamlTree((BamlLocalizableResourceKey)dictionaryEntry.Key, (BamlLocalizableResource)dictionaryEntry.Value, treeMap2);
			}
		}

		// Token: 0x06006EB0 RID: 28336 RVA: 0x001FC7B8 File Offset: 0x001FA9B8
		private static void CreateMissingBamlTreeNode(BamlLocalizationDictionary dictionary, BamlTreeUpdater.BamlTreeUpdateMap treeMap)
		{
			BamlLocalizationDictionaryEnumerator enumerator = dictionary.GetEnumerator();
			while (enumerator.MoveNext())
			{
				BamlLocalizableResourceKey key = enumerator.Key;
				BamlLocalizableResource value = enumerator.Value;
				if (treeMap.MapKeyToBamlTreeNode(key) == null)
				{
					if (key.PropertyName == "$Content")
					{
						if (treeMap.MapUidToBamlTreeElementNode(key.Uid) == null)
						{
							BamlStartElementNode bamlStartElementNode = new BamlStartElementNode(treeMap.Resolver.ResolveAssemblyFromClass(key.ClassName), key.ClassName, false, false);
							bamlStartElementNode.AddChild(new BamlDefAttributeNode("Uid", key.Uid));
							BamlTreeUpdater.TryAddContentPropertyToNewElement(treeMap, bamlStartElementNode);
							bamlStartElementNode.AddChild(new BamlEndElementNode());
							treeMap.AddBamlTreeNode(key.Uid, key, bamlStartElementNode);
						}
					}
					else
					{
						BamlTreeNode node;
						if (key.PropertyName == "$LiteralContent")
						{
							node = new BamlLiteralContentNode(value.Content);
						}
						else
						{
							node = new BamlPropertyNode(treeMap.Resolver.ResolveAssemblyFromClass(key.ClassName), key.ClassName, key.PropertyName, value.Content, BamlAttributeUsage.Default);
						}
						treeMap.AddBamlTreeNode(null, key, node);
					}
				}
			}
		}

		// Token: 0x06006EB1 RID: 28337 RVA: 0x001FC8D0 File Offset: 0x001FAAD0
		private static bool ApplyChangeToBamlTree(BamlLocalizableResourceKey key, BamlLocalizableResource resource, BamlTreeUpdater.BamlTreeUpdateMap treeMap)
		{
			if (resource == null || resource.Content == null || !resource.Modifiable)
			{
				return true;
			}
			if (!treeMap.LocalizationDictionary.Contains(key) && !treeMap.IsNewBamlTreeNode(key))
			{
				return true;
			}
			BamlTreeNode bamlTreeNode = treeMap.MapKeyToBamlTreeNode(key);
			Invariant.Assert(bamlTreeNode != null);
			BamlNodeType nodeType = bamlTreeNode.NodeType;
			if (nodeType != BamlNodeType.StartElement)
			{
				if (nodeType != BamlNodeType.Property)
				{
					if (nodeType == BamlNodeType.LiteralContent)
					{
						BamlLiteralContentNode bamlLiteralContentNode = (BamlLiteralContentNode)bamlTreeNode;
						bamlLiteralContentNode.Content = BamlResourceContentUtil.UnescapeString(resource.Content);
						if (bamlLiteralContentNode.Parent == null)
						{
							BamlTreeNode bamlTreeNode2 = treeMap.MapUidToBamlTreeElementNode(key.Uid);
							if (bamlTreeNode2 == null)
							{
								return false;
							}
							bamlTreeNode2.AddChild(bamlLiteralContentNode);
						}
					}
				}
				else
				{
					BamlPropertyNode bamlPropertyNode = (BamlPropertyNode)bamlTreeNode;
					bamlPropertyNode.Value = BamlResourceContentUtil.UnescapeString(resource.Content);
					if (bamlPropertyNode.Parent == null)
					{
						BamlStartElementNode bamlStartElementNode = treeMap.MapUidToBamlTreeElementNode(key.Uid);
						if (bamlStartElementNode == null)
						{
							return false;
						}
						bamlStartElementNode.InsertProperty(bamlTreeNode);
					}
				}
			}
			else
			{
				string b = null;
				if (treeMap.LocalizationDictionary.Contains(key))
				{
					b = treeMap.LocalizationDictionary[key].Content;
				}
				if (resource.Content != b)
				{
					BamlTreeUpdater.ReArrangeChildren(key, bamlTreeNode, resource.Content, treeMap);
				}
			}
			return true;
		}

		// Token: 0x06006EB2 RID: 28338 RVA: 0x001FC9FC File Offset: 0x001FABFC
		private static void ReArrangeChildren(BamlLocalizableResourceKey key, BamlTreeNode node, string translation, BamlTreeUpdater.BamlTreeUpdateMap treeMap)
		{
			IList<BamlTreeNode> newChildren = BamlTreeUpdater.SplitXmlContent(key, translation, treeMap);
			BamlTreeUpdater.MergeChildrenList(key, treeMap, node, newChildren);
		}

		// Token: 0x06006EB3 RID: 28339 RVA: 0x001FCA1C File Offset: 0x001FAC1C
		private static void MergeChildrenList(BamlLocalizableResourceKey key, BamlTreeUpdater.BamlTreeUpdateMap treeMap, BamlTreeNode parent, IList<BamlTreeNode> newChildren)
		{
			if (newChildren == null)
			{
				return;
			}
			List<BamlTreeNode> children = parent.Children;
			int i = 0;
			StringBuilder stringBuilder = new StringBuilder();
			if (children != null)
			{
				Hashtable hashtable = new Hashtable(newChildren.Count);
				foreach (BamlTreeNode bamlTreeNode in newChildren)
				{
					if (bamlTreeNode.NodeType == BamlNodeType.StartElement)
					{
						BamlStartElementNode bamlStartElementNode = (BamlStartElementNode)bamlTreeNode;
						if (bamlStartElementNode.Uid != null)
						{
							if (hashtable.ContainsKey(bamlStartElementNode.Uid))
							{
								treeMap.Resolver.RaiseErrorNotifyEvent(new BamlLocalizerErrorNotifyEventArgs(key, BamlLocalizerError.DuplicateElement));
								return;
							}
							hashtable[bamlStartElementNode.Uid] = null;
						}
					}
				}
				parent.Children = null;
				for (int j = 0; j < children.Count - 1; j++)
				{
					BamlTreeNode bamlTreeNode2 = children[j];
					BamlNodeType nodeType = bamlTreeNode2.NodeType;
					if (nodeType != BamlNodeType.StartElement)
					{
						if (nodeType != BamlNodeType.Text)
						{
							parent.AddChild(bamlTreeNode2);
						}
					}
					else
					{
						BamlStartElementNode bamlStartElementNode2 = (BamlStartElementNode)bamlTreeNode2;
						if (bamlStartElementNode2.Uid != null)
						{
							if (!hashtable.ContainsKey(bamlStartElementNode2.Uid))
							{
								parent.Children = children;
								treeMap.Resolver.RaiseErrorNotifyEvent(new BamlLocalizerErrorNotifyEventArgs(key, BamlLocalizerError.MismatchedElements));
								return;
							}
							hashtable.Remove(bamlStartElementNode2.Uid);
						}
						while (i < newChildren.Count)
						{
							BamlTreeNode bamlTreeNode3 = newChildren[i++];
							Invariant.Assert(bamlTreeNode3 != null);
							if (bamlTreeNode3.NodeType == BamlNodeType.Text)
							{
								stringBuilder.Append(((BamlTextNode)bamlTreeNode3).Content);
							}
							else
							{
								BamlTreeUpdater.TryFlushTextToBamlNode(parent, stringBuilder);
								parent.AddChild(bamlTreeNode3);
								if (bamlTreeNode3.NodeType == BamlNodeType.StartElement)
								{
									break;
								}
							}
						}
					}
				}
			}
			while (i < newChildren.Count)
			{
				BamlTreeNode bamlTreeNode4 = newChildren[i];
				Invariant.Assert(bamlTreeNode4 != null);
				if (bamlTreeNode4.NodeType == BamlNodeType.Text)
				{
					stringBuilder.Append(((BamlTextNode)bamlTreeNode4).Content);
				}
				else
				{
					BamlTreeUpdater.TryFlushTextToBamlNode(parent, stringBuilder);
					parent.AddChild(bamlTreeNode4);
				}
				i++;
			}
			BamlTreeUpdater.TryFlushTextToBamlNode(parent, stringBuilder);
			parent.AddChild(new BamlEndElementNode());
		}

		// Token: 0x06006EB4 RID: 28340 RVA: 0x001FCC38 File Offset: 0x001FAE38
		private static void TryFlushTextToBamlNode(BamlTreeNode parent, StringBuilder textContent)
		{
			if (textContent.Length > 0)
			{
				BamlTreeNode child = new BamlTextNode(textContent.ToString());
				parent.AddChild(child);
				textContent.Length = 0;
			}
		}

		// Token: 0x06006EB5 RID: 28341 RVA: 0x001FCC68 File Offset: 0x001FAE68
		private static IList<BamlTreeNode> SplitXmlContent(BamlLocalizableResourceKey key, string content, BamlTreeUpdater.BamlTreeUpdateMap bamlTreeMap)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<ROOT>");
			stringBuilder.Append(content);
			stringBuilder.Append("</ROOT>");
			IList<BamlTreeNode> list = new List<BamlTreeNode>(4);
			XmlDocument xmlDocument = new XmlDocument();
			bool flag = true;
			try
			{
				xmlDocument.LoadXml(stringBuilder.ToString());
				XmlElement xmlElement = xmlDocument.FirstChild as XmlElement;
				if (xmlElement != null && xmlElement.HasChildNodes)
				{
					int num = 0;
					while (num < xmlElement.ChildNodes.Count && flag)
					{
						flag = BamlTreeUpdater.GetBamlTreeNodeFromXmlNode(key, xmlElement.ChildNodes[num], bamlTreeMap, list);
						num++;
					}
				}
			}
			catch (XmlException)
			{
				bamlTreeMap.Resolver.RaiseErrorNotifyEvent(new BamlLocalizerErrorNotifyEventArgs(key, BamlLocalizerError.SubstitutionAsPlaintext));
				flag = BamlTreeUpdater.GetBamlTreeNodeFromText(key, content, bamlTreeMap, list);
			}
			if (!flag)
			{
				return null;
			}
			return list;
		}

		// Token: 0x06006EB6 RID: 28342 RVA: 0x001FCD40 File Offset: 0x001FAF40
		private static bool GetBamlTreeNodeFromXmlNode(BamlLocalizableResourceKey key, XmlNode node, BamlTreeUpdater.BamlTreeUpdateMap bamlTreeMap, IList<BamlTreeNode> newChildrenList)
		{
			if (node.NodeType == XmlNodeType.Text)
			{
				return BamlTreeUpdater.GetBamlTreeNodeFromText(key, node.Value, bamlTreeMap, newChildrenList);
			}
			if (node.NodeType != XmlNodeType.Element)
			{
				return true;
			}
			XmlElement xmlElement = node as XmlElement;
			string text = bamlTreeMap.Resolver.ResolveFormattingTagToClass(xmlElement.Name);
			bool flag = string.IsNullOrEmpty(text);
			string text2 = null;
			if (!flag)
			{
				text2 = bamlTreeMap.Resolver.ResolveAssemblyFromClass(text);
				flag = string.IsNullOrEmpty(text2);
			}
			if (flag)
			{
				bamlTreeMap.Resolver.RaiseErrorNotifyEvent(new BamlLocalizerErrorNotifyEventArgs(key, BamlLocalizerError.UnknownFormattingTag));
				return false;
			}
			string text3 = null;
			if (xmlElement.HasAttributes)
			{
				text3 = xmlElement.GetAttribute("Uid");
				if (!string.IsNullOrEmpty(text3))
				{
					text3 = BamlResourceContentUtil.UnescapeString(text3);
				}
			}
			BamlStartElementNode bamlStartElementNode = null;
			if (text3 != null)
			{
				bamlStartElementNode = bamlTreeMap.MapUidToBamlTreeElementNode(text3);
			}
			if (bamlStartElementNode == null)
			{
				bamlStartElementNode = new BamlStartElementNode(text2, text, false, false);
				if (text3 != null)
				{
					bamlTreeMap.AddBamlTreeNode(text3, new BamlLocalizableResourceKey(text3, text, "$Content", text2), bamlStartElementNode);
					bamlStartElementNode.AddChild(new BamlDefAttributeNode("Uid", text3));
				}
				BamlTreeUpdater.TryAddContentPropertyToNewElement(bamlTreeMap, bamlStartElementNode);
				bamlStartElementNode.AddChild(new BamlEndElementNode());
			}
			else if (bamlStartElementNode.TypeFullName != text)
			{
				bamlTreeMap.Resolver.RaiseErrorNotifyEvent(new BamlLocalizerErrorNotifyEventArgs(key, BamlLocalizerError.DuplicateUid));
				return false;
			}
			newChildrenList.Add(bamlStartElementNode);
			bool flag2 = true;
			if (xmlElement.HasChildNodes)
			{
				IList<BamlTreeNode> list = new List<BamlTreeNode>();
				int num = 0;
				while (num < xmlElement.ChildNodes.Count && flag2)
				{
					flag2 = BamlTreeUpdater.GetBamlTreeNodeFromXmlNode(key, xmlElement.ChildNodes[num], bamlTreeMap, list);
					num++;
				}
				if (flag2)
				{
					BamlTreeUpdater.MergeChildrenList(key, bamlTreeMap, bamlStartElementNode, list);
				}
			}
			return flag2;
		}

		// Token: 0x06006EB7 RID: 28343 RVA: 0x001FCED8 File Offset: 0x001FB0D8
		private static bool GetBamlTreeNodeFromText(BamlLocalizableResourceKey key, string content, BamlTreeUpdater.BamlTreeUpdateMap bamlTreeMap, IList<BamlTreeNode> newChildrenList)
		{
			BamlStringToken[] array = BamlResourceContentUtil.ParseChildPlaceholder(content);
			if (array == null)
			{
				bamlTreeMap.Resolver.RaiseErrorNotifyEvent(new BamlLocalizerErrorNotifyEventArgs(key, BamlLocalizerError.IncompleteElementPlaceholder));
				return false;
			}
			bool result = true;
			for (int i = 0; i < array.Length; i++)
			{
				BamlStringToken.TokenType type = array[i].Type;
				if (type != BamlStringToken.TokenType.Text)
				{
					if (type == BamlStringToken.TokenType.ChildPlaceHolder)
					{
						BamlTreeNode bamlTreeNode = bamlTreeMap.MapUidToBamlTreeElementNode(array[i].Value);
						if (bamlTreeNode != null)
						{
							newChildrenList.Add(bamlTreeNode);
						}
						else
						{
							bamlTreeMap.Resolver.RaiseErrorNotifyEvent(new BamlLocalizerErrorNotifyEventArgs(new BamlLocalizableResourceKey(array[i].Value, string.Empty, string.Empty), BamlLocalizerError.InvalidUid));
							result = false;
						}
					}
				}
				else
				{
					BamlTreeNode item = new BamlTextNode(array[i].Value);
					newChildrenList.Add(item);
				}
			}
			return result;
		}

		// Token: 0x06006EB8 RID: 28344 RVA: 0x001FCFA0 File Offset: 0x001FB1A0
		private static void TryAddContentPropertyToNewElement(BamlTreeUpdater.BamlTreeUpdateMap bamlTreeMap, BamlStartElementNode bamlNode)
		{
			string contentProperty = bamlTreeMap.GetContentProperty(bamlNode.AssemblyName, bamlNode.TypeFullName);
			if (!string.IsNullOrEmpty(contentProperty))
			{
				bamlNode.AddChild(new BamlContentPropertyNode(bamlNode.AssemblyName, bamlNode.TypeFullName, contentProperty));
			}
		}

		// Token: 0x02000B2B RID: 2859
		private class BamlTreeUpdateMap
		{
			// Token: 0x06008D4A RID: 36170 RVA: 0x0025929A File Offset: 0x0025749A
			internal BamlTreeUpdateMap(BamlTreeMap map, BamlTree tree)
			{
				this._uidToNewBamlNodeIndexMap = new Hashtable(8);
				this._keyToNewBamlNodeIndexMap = new Hashtable(8);
				this._originalMap = map;
				this._tree = tree;
			}

			// Token: 0x06008D4B RID: 36171 RVA: 0x002592C8 File Offset: 0x002574C8
			internal BamlTreeNode MapKeyToBamlTreeNode(BamlLocalizableResourceKey key)
			{
				BamlTreeNode bamlTreeNode = this._originalMap.MapKeyToBamlTreeNode(key, this._tree);
				if (bamlTreeNode == null && this._keyToNewBamlNodeIndexMap.Contains(key))
				{
					bamlTreeNode = this._tree[(int)this._keyToNewBamlNodeIndexMap[key]];
				}
				return bamlTreeNode;
			}

			// Token: 0x06008D4C RID: 36172 RVA: 0x00259317 File Offset: 0x00257517
			internal bool IsNewBamlTreeNode(BamlLocalizableResourceKey key)
			{
				return this._keyToNewBamlNodeIndexMap.Contains(key);
			}

			// Token: 0x06008D4D RID: 36173 RVA: 0x00259328 File Offset: 0x00257528
			internal BamlStartElementNode MapUidToBamlTreeElementNode(string uid)
			{
				BamlStartElementNode bamlStartElementNode = this._originalMap.MapUidToBamlTreeElementNode(uid, this._tree);
				if (bamlStartElementNode == null && this._uidToNewBamlNodeIndexMap.Contains(uid))
				{
					bamlStartElementNode = (this._tree[(int)this._uidToNewBamlNodeIndexMap[uid]] as BamlStartElementNode);
				}
				return bamlStartElementNode;
			}

			// Token: 0x06008D4E RID: 36174 RVA: 0x0025937C File Offset: 0x0025757C
			internal void AddBamlTreeNode(string uid, BamlLocalizableResourceKey key, BamlTreeNode node)
			{
				this._tree.AddTreeNode(node);
				if (uid != null)
				{
					this._uidToNewBamlNodeIndexMap[uid] = this._tree.Size - 1;
				}
				this._keyToNewBamlNodeIndexMap[key] = this._tree.Size - 1;
			}

			// Token: 0x17001F70 RID: 8048
			// (get) Token: 0x06008D4F RID: 36175 RVA: 0x002593D4 File Offset: 0x002575D4
			internal BamlLocalizationDictionary LocalizationDictionary
			{
				get
				{
					return this._originalMap.LocalizationDictionary;
				}
			}

			// Token: 0x17001F71 RID: 8049
			// (get) Token: 0x06008D50 RID: 36176 RVA: 0x002593E1 File Offset: 0x002575E1
			internal InternalBamlLocalizabilityResolver Resolver
			{
				get
				{
					return this._originalMap.Resolver;
				}
			}

			// Token: 0x06008D51 RID: 36177 RVA: 0x002593F0 File Offset: 0x002575F0
			internal string GetContentProperty(string assemblyName, string fullTypeName)
			{
				string clrNamespace = string.Empty;
				string typeShortName = fullTypeName;
				int num = fullTypeName.LastIndexOf('.');
				if (num >= 0)
				{
					clrNamespace = fullTypeName.Substring(0, num);
					typeShortName = fullTypeName.Substring(num + 1);
				}
				short knownTypeIdFromName = BamlMapTable.GetKnownTypeIdFromName(assemblyName, clrNamespace, typeShortName);
				if (knownTypeIdFromName != 0)
				{
					KnownElements knownElement = (KnownElements)(-(KnownElements)knownTypeIdFromName);
					return KnownTypes.GetContentPropertyName(knownElement);
				}
				string text = null;
				if (this._contentPropertyTable != null && this._contentPropertyTable.TryGetValue(fullTypeName, out text))
				{
					return text;
				}
				Assembly assembly = Assembly.Load(assemblyName);
				Type type = assembly.GetType(fullTypeName);
				if (type != null)
				{
					object[] customAttributes = type.GetCustomAttributes(typeof(ContentPropertyAttribute), true);
					if (customAttributes.Length != 0)
					{
						ContentPropertyAttribute contentPropertyAttribute = customAttributes[0] as ContentPropertyAttribute;
						text = contentPropertyAttribute.Name;
						if (this._contentPropertyTable == null)
						{
							this._contentPropertyTable = new Dictionary<string, string>(8);
						}
						this._contentPropertyTable.Add(fullTypeName, text);
					}
				}
				return text;
			}

			// Token: 0x04004A7E RID: 19070
			private BamlTreeMap _originalMap;

			// Token: 0x04004A7F RID: 19071
			private BamlTree _tree;

			// Token: 0x04004A80 RID: 19072
			private Hashtable _uidToNewBamlNodeIndexMap;

			// Token: 0x04004A81 RID: 19073
			private Hashtable _keyToNewBamlNodeIndexMap;

			// Token: 0x04004A82 RID: 19074
			private Dictionary<string, string> _contentPropertyTable;
		}
	}
}
