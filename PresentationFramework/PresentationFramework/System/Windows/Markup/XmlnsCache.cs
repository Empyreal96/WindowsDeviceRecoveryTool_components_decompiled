using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;

namespace System.Windows.Markup
{
	// Token: 0x02000274 RID: 628
	internal class XmlnsCache
	{
		// Token: 0x060023CC RID: 9164 RVA: 0x000AEC3A File Offset: 0x000ACE3A
		internal XmlnsCache()
		{
			this._compatTable = new Dictionary<string, string>();
			this._compatTableReverse = new Dictionary<string, string>();
			this._cacheTable = new HybridDictionary();
			this._uriToAssemblyNameTable = new HybridDictionary();
		}

		// Token: 0x060023CD RID: 9165 RVA: 0x000AEC70 File Offset: 0x000ACE70
		internal List<ClrNamespaceAssemblyPair> GetMappingArray(string xmlns)
		{
			List<ClrNamespaceAssemblyPair> list = null;
			lock (this)
			{
				list = (this._cacheTable[xmlns] as List<ClrNamespaceAssemblyPair>);
				if (list == null)
				{
					if (this._uriToAssemblyNameTable[xmlns] != null)
					{
						string[] array = this._uriToAssemblyNameTable[xmlns] as string[];
						Assembly[] array2 = new Assembly[array.Length];
						for (int i = 0; i < array.Length; i++)
						{
							array2[i] = ReflectionHelper.LoadAssembly(array[i], null);
						}
						this._cacheTable[xmlns] = this.GetClrnsToAssemblyNameMappingList(array2, xmlns);
					}
					else
					{
						Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
						this._cacheTable[xmlns] = this.GetClrnsToAssemblyNameMappingList(assemblies, xmlns);
						this.ProcessXmlnsCompatibleWithAttributes(assemblies);
					}
					list = (this._cacheTable[xmlns] as List<ClrNamespaceAssemblyPair>);
				}
			}
			return list;
		}

		// Token: 0x060023CE RID: 9166 RVA: 0x000AED60 File Offset: 0x000ACF60
		internal void SetUriToAssemblyNameMapping(string namespaceUri, string[] asmNameList)
		{
			this._uriToAssemblyNameTable[namespaceUri] = asmNameList;
		}

		// Token: 0x060023CF RID: 9167 RVA: 0x000AED70 File Offset: 0x000ACF70
		internal string GetNewXmlnamespace(string oldXmlnamespace)
		{
			string result;
			if (this._compatTable.TryGetValue(oldXmlnamespace, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x060023D0 RID: 9168 RVA: 0x000AED90 File Offset: 0x000ACF90
		private Attribute[] GetAttributes(Assembly asm, Type attrType)
		{
			return Attribute.GetCustomAttributes(asm, attrType);
		}

		// Token: 0x060023D1 RID: 9169 RVA: 0x000AED9C File Offset: 0x000ACF9C
		private void GetNamespacesFromDefinitionAttr(Attribute attr, out string xmlns, out string clrns)
		{
			XmlnsDefinitionAttribute xmlnsDefinitionAttribute = (XmlnsDefinitionAttribute)attr;
			xmlns = xmlnsDefinitionAttribute.XmlNamespace;
			clrns = xmlnsDefinitionAttribute.ClrNamespace;
		}

		// Token: 0x060023D2 RID: 9170 RVA: 0x000AEDC0 File Offset: 0x000ACFC0
		private void GetNamespacesFromCompatAttr(Attribute attr, out string oldXmlns, out string newXmlns)
		{
			XmlnsCompatibleWithAttribute xmlnsCompatibleWithAttribute = (XmlnsCompatibleWithAttribute)attr;
			oldXmlns = xmlnsCompatibleWithAttribute.OldNamespace;
			newXmlns = xmlnsCompatibleWithAttribute.NewNamespace;
		}

		// Token: 0x060023D3 RID: 9171 RVA: 0x000AEDE4 File Offset: 0x000ACFE4
		private List<ClrNamespaceAssemblyPair> GetClrnsToAssemblyNameMappingList(Assembly[] asmList, string xmlnsRequested)
		{
			List<ClrNamespaceAssemblyPair> list = new List<ClrNamespaceAssemblyPair>();
			for (int i = 0; i < asmList.Length; i++)
			{
				string fullName = asmList[i].FullName;
				Attribute[] attributes = this.GetAttributes(asmList[i], typeof(XmlnsDefinitionAttribute));
				for (int j = 0; j < attributes.Length; j++)
				{
					string text = null;
					string text2 = null;
					this.GetNamespacesFromDefinitionAttr(attributes[j], out text, out text2);
					if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text2))
					{
						throw new ArgumentException(SR.Get("ParserAttributeArgsLow", new object[]
						{
							"XmlnsDefinitionAttribute"
						}));
					}
					if (string.CompareOrdinal(xmlnsRequested, text) == 0)
					{
						list.Add(new ClrNamespaceAssemblyPair(text2, fullName));
					}
				}
			}
			return list;
		}

		// Token: 0x060023D4 RID: 9172 RVA: 0x000AEE98 File Offset: 0x000AD098
		private void ProcessXmlnsCompatibleWithAttributes(Assembly[] asmList)
		{
			for (int i = 0; i < asmList.Length; i++)
			{
				Attribute[] attributes = this.GetAttributes(asmList[i], typeof(XmlnsCompatibleWithAttribute));
				for (int j = 0; j < attributes.Length; j++)
				{
					string text = null;
					string text2 = null;
					this.GetNamespacesFromCompatAttr(attributes[j], out text, out text2);
					if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text2))
					{
						throw new ArgumentException(SR.Get("ParserAttributeArgsLow", new object[]
						{
							"XmlnsCompatibleWithAttribute"
						}));
					}
					if (this._compatTable.ContainsKey(text) && this._compatTable[text] != text2)
					{
						throw new InvalidOperationException(SR.Get("ParserCompatDuplicate", new object[]
						{
							text,
							this._compatTable[text]
						}));
					}
					this._compatTable[text] = text2;
					this._compatTableReverse[text2] = text;
				}
			}
		}

		// Token: 0x04001B0B RID: 6923
		private HybridDictionary _cacheTable;

		// Token: 0x04001B0C RID: 6924
		private Dictionary<string, string> _compatTable;

		// Token: 0x04001B0D RID: 6925
		private Dictionary<string, string> _compatTableReverse;

		// Token: 0x04001B0E RID: 6926
		private HybridDictionary _uriToAssemblyNameTable;
	}
}
