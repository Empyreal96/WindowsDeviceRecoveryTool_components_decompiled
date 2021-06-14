using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x0200069A RID: 1690
	internal sealed class BamlResourceDeserializer
	{
		// Token: 0x06006E13 RID: 28179 RVA: 0x001FAAAB File Offset: 0x001F8CAB
		internal static BamlTree LoadBaml(Stream bamlStream)
		{
			return new BamlResourceDeserializer().LoadBamlImp(bamlStream);
		}

		// Token: 0x06006E14 RID: 28180 RVA: 0x001FAAB8 File Offset: 0x001F8CB8
		private BamlResourceDeserializer()
		{
		}

		// Token: 0x06006E15 RID: 28181 RVA: 0x001FAAD8 File Offset: 0x001F8CD8
		private BamlTree LoadBamlImp(Stream bamlSteam)
		{
			this._reader = new BamlReader(bamlSteam);
			this._reader.Read();
			if (this._reader.NodeType != BamlNodeType.StartDocument)
			{
				throw new XamlParseException(SR.Get("InvalidStartOfBaml"));
			}
			this._root = new BamlStartDocumentNode();
			this.PushNodeToStack(this._root);
			Hashtable hashtable = new Hashtable(8);
			IL_5C8:
			while (this._bamlTreeStack.Count > 0 && this._reader.Read())
			{
				switch (this._reader.NodeType)
				{
				case BamlNodeType.EndDocument:
				{
					BamlTreeNode node = new BamlEndDocumentNode();
					this.AddChildToCurrentParent(node);
					this.PopStack();
					break;
				}
				case BamlNodeType.ConnectionId:
				case BamlNodeType.Property:
				case BamlNodeType.ContentProperty:
				case BamlNodeType.XmlnsProperty:
				case BamlNodeType.IncludeReference:
				case BamlNodeType.DefAttribute:
				case BamlNodeType.PresentationOptionsAttribute:
					goto IL_2DF;
				case BamlNodeType.StartElement:
				{
					BamlTreeNode node2 = new BamlStartElementNode(this._reader.AssemblyName, this._reader.Name, this._reader.IsInjected, this._reader.CreateUsingTypeConverter);
					this.PushNodeToStack(node2);
					break;
				}
				case BamlNodeType.EndElement:
				{
					BamlTreeNode node3 = new BamlEndElementNode();
					this.AddChildToCurrentParent(node3);
					this.PopStack();
					break;
				}
				case BamlNodeType.StartComplexProperty:
				{
					BamlStartComplexPropertyNode bamlStartComplexPropertyNode = new BamlStartComplexPropertyNode(this._reader.AssemblyName, this._reader.Name.Substring(0, this._reader.Name.LastIndexOf('.')), this._reader.LocalName);
					bamlStartComplexPropertyNode.LocalizabilityAncestor = this.PeekPropertyStack(bamlStartComplexPropertyNode.PropertyName);
					this.PushPropertyToStack(bamlStartComplexPropertyNode.PropertyName, bamlStartComplexPropertyNode);
					this.PushNodeToStack(bamlStartComplexPropertyNode);
					break;
				}
				case BamlNodeType.EndComplexProperty:
				{
					BamlTreeNode node4 = new BamlEndComplexPropertyNode();
					this.AddChildToCurrentParent(node4);
					this.PopStack();
					break;
				}
				case BamlNodeType.LiteralContent:
				{
					BamlTreeNode node5 = new BamlLiteralContentNode(this._reader.Value);
					this.AddChildToCurrentParent(node5);
					break;
				}
				case BamlNodeType.Text:
				{
					BamlTreeNode node6 = new BamlTextNode(this._reader.Value, this._reader.TypeConverterAssemblyName, this._reader.TypeConverterName);
					this.AddChildToCurrentParent(node6);
					break;
				}
				case BamlNodeType.RoutedEvent:
				{
					BamlTreeNode node7 = new BamlRoutedEventNode(this._reader.AssemblyName, this._reader.Name.Substring(0, this._reader.Name.LastIndexOf('.')), this._reader.LocalName, this._reader.Value);
					this.AddChildToCurrentParent(node7);
					break;
				}
				case BamlNodeType.Event:
				{
					BamlTreeNode node8 = new BamlEventNode(this._reader.Name, this._reader.Value);
					this.AddChildToCurrentParent(node8);
					break;
				}
				case BamlNodeType.PIMapping:
				{
					BamlTreeNode node9 = new BamlPIMappingNode(this._reader.XmlNamespace, this._reader.ClrNamespace, this._reader.AssemblyName);
					this.AddChildToCurrentParent(node9);
					break;
				}
				case BamlNodeType.StartConstructor:
				{
					BamlTreeNode node10 = new BamlStartConstructorNode();
					this.AddChildToCurrentParent(node10);
					break;
				}
				case BamlNodeType.EndConstructor:
				{
					BamlTreeNode node11 = new BamlEndConstructorNode();
					this.AddChildToCurrentParent(node11);
					break;
				}
				default:
					goto IL_2DF;
				}
				if (this._reader.HasProperties)
				{
					hashtable.Clear();
					this._reader.MoveToFirstProperty();
					for (;;)
					{
						BamlNodeType nodeType = this._reader.NodeType;
						switch (nodeType)
						{
						case BamlNodeType.ConnectionId:
						{
							BamlTreeNode node12 = new BamlConnectionIdNode(this._reader.ConnectionId);
							this.AddChildToCurrentParent(node12);
							break;
						}
						case BamlNodeType.StartElement:
						case BamlNodeType.EndElement:
							goto IL_58F;
						case BamlNodeType.Property:
						{
							BamlPropertyNode bamlPropertyNode = new BamlPropertyNode(this._reader.AssemblyName, this._reader.Name.Substring(0, this._reader.Name.LastIndexOf('.')), this._reader.LocalName, this._reader.Value, this._reader.AttributeUsage);
							bamlPropertyNode.LocalizabilityAncestor = this.PeekPropertyStack(bamlPropertyNode.PropertyName);
							this.PushPropertyToStack(bamlPropertyNode.PropertyName, bamlPropertyNode);
							this.AddChildToCurrentParent(bamlPropertyNode);
							if (hashtable.Contains(this._reader.Name))
							{
								object obj = hashtable[this._reader.Name];
								int num = 2;
								if (obj is BamlPropertyNode)
								{
									((BamlPropertyNode)obj).Index = 1;
								}
								else
								{
									num = (int)obj;
								}
								bamlPropertyNode.Index = num;
								hashtable[this._reader.Name] = num + 1;
							}
							else
							{
								hashtable[this._reader.Name] = bamlPropertyNode;
							}
							break;
						}
						case BamlNodeType.ContentProperty:
						{
							BamlTreeNode node13 = new BamlContentPropertyNode(this._reader.AssemblyName, this._reader.Name.Substring(0, this._reader.Name.LastIndexOf('.')), this._reader.LocalName);
							this.AddChildToCurrentParent(node13);
							break;
						}
						case BamlNodeType.XmlnsProperty:
						{
							BamlTreeNode node14 = new BamlXmlnsPropertyNode(this._reader.LocalName, this._reader.Value);
							this.AddChildToCurrentParent(node14);
							break;
						}
						default:
							if (nodeType != BamlNodeType.DefAttribute)
							{
								if (nodeType != BamlNodeType.PresentationOptionsAttribute)
								{
									goto Block_6;
								}
								BamlTreeNode node15 = new BamlPresentationOptionsAttributeNode(this._reader.Name, this._reader.Value);
								this.AddChildToCurrentParent(node15);
							}
							else
							{
								if (this._reader.Name == "Uid")
								{
									((BamlStartElementNode)this._currentParent).Uid = this._reader.Value;
								}
								BamlTreeNode node16 = new BamlDefAttributeNode(this._reader.Name, this._reader.Value);
								this.AddChildToCurrentParent(node16);
							}
							break;
						}
						if (!this._reader.MoveToNextProperty())
						{
							goto IL_5C8;
						}
					}
					Block_6:
					IL_58F:
					throw new XamlParseException(SR.Get("UnRecognizedBamlNodeType", new object[]
					{
						this._reader.NodeType
					}));
				}
				continue;
				IL_2DF:
				throw new XamlParseException(SR.Get("UnRecognizedBamlNodeType", new object[]
				{
					this._reader.NodeType
				}));
			}
			if (this._reader.Read() || this._bamlTreeStack.Count > 0)
			{
				throw new XamlParseException(SR.Get("InvalidEndOfBaml"));
			}
			return new BamlTree(this._root, this._nodeCount);
		}

		// Token: 0x06006E16 RID: 28182 RVA: 0x001FB107 File Offset: 0x001F9307
		private void PushNodeToStack(BamlTreeNode node)
		{
			if (this._currentParent != null)
			{
				this._currentParent.AddChild(node);
			}
			this._bamlTreeStack.Push(node);
			this._currentParent = node;
			this._nodeCount++;
		}

		// Token: 0x06006E17 RID: 28183 RVA: 0x001FB13E File Offset: 0x001F933E
		private void AddChildToCurrentParent(BamlTreeNode node)
		{
			if (this._currentParent == null)
			{
				throw new InvalidOperationException(SR.Get("NullParentNode"));
			}
			this._currentParent.AddChild(node);
			this._nodeCount++;
		}

		// Token: 0x06006E18 RID: 28184 RVA: 0x001FB174 File Offset: 0x001F9374
		private void PopStack()
		{
			BamlTreeNode bamlTreeNode = this._bamlTreeStack.Pop();
			if (bamlTreeNode.Children != null)
			{
				foreach (BamlTreeNode bamlTreeNode2 in bamlTreeNode.Children)
				{
					BamlStartComplexPropertyNode bamlStartComplexPropertyNode = bamlTreeNode2 as BamlStartComplexPropertyNode;
					if (bamlStartComplexPropertyNode != null)
					{
						this.PopPropertyFromStack(bamlStartComplexPropertyNode.PropertyName);
					}
				}
			}
			if (this._bamlTreeStack.Count > 0)
			{
				this._currentParent = this._bamlTreeStack.Peek();
				return;
			}
			this._currentParent = null;
		}

		// Token: 0x06006E19 RID: 28185 RVA: 0x001FB214 File Offset: 0x001F9414
		private void PushPropertyToStack(string propertyName, ILocalizabilityInheritable node)
		{
			Stack<ILocalizabilityInheritable> stack;
			if (this._propertyInheritanceTreeStack.ContainsKey(propertyName))
			{
				stack = this._propertyInheritanceTreeStack[propertyName];
			}
			else
			{
				stack = new Stack<ILocalizabilityInheritable>();
				this._propertyInheritanceTreeStack.Add(propertyName, stack);
			}
			stack.Push(node);
		}

		// Token: 0x06006E1A RID: 28186 RVA: 0x001FB258 File Offset: 0x001F9458
		private void PopPropertyFromStack(string propertyName)
		{
			Stack<ILocalizabilityInheritable> stack = this._propertyInheritanceTreeStack[propertyName];
			stack.Pop();
		}

		// Token: 0x06006E1B RID: 28187 RVA: 0x001FB27C File Offset: 0x001F947C
		private ILocalizabilityInheritable PeekPropertyStack(string propertyName)
		{
			if (this._propertyInheritanceTreeStack.ContainsKey(propertyName))
			{
				Stack<ILocalizabilityInheritable> stack = this._propertyInheritanceTreeStack[propertyName];
				if (stack.Count > 0)
				{
					return stack.Peek();
				}
			}
			return this._root;
		}

		// Token: 0x0400362B RID: 13867
		private Stack<BamlTreeNode> _bamlTreeStack = new Stack<BamlTreeNode>();

		// Token: 0x0400362C RID: 13868
		private Dictionary<string, Stack<ILocalizabilityInheritable>> _propertyInheritanceTreeStack = new Dictionary<string, Stack<ILocalizabilityInheritable>>(8);

		// Token: 0x0400362D RID: 13869
		private BamlTreeNode _currentParent;

		// Token: 0x0400362E RID: 13870
		private BamlStartDocumentNode _root;

		// Token: 0x0400362F RID: 13871
		private BamlReader _reader;

		// Token: 0x04003630 RID: 13872
		private int _nodeCount;
	}
}
