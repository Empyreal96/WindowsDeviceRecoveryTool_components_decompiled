using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Markup;
using System.Xml;

namespace System.Windows.Data
{
	/// <summary>Represents a collection of <see cref="T:System.Windows.Data.XmlNamespaceMapping" /> objects.</summary>
	// Token: 0x020001BE RID: 446
	[Localizability(LocalizationCategory.NeverLocalize)]
	public class XmlNamespaceMappingCollection : XmlNamespaceManager, ICollection<XmlNamespaceMapping>, IEnumerable<XmlNamespaceMapping>, IEnumerable, IAddChildInternal, IAddChild
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Data.XmlNamespaceMappingCollection" /> class.</summary>
		// Token: 0x06001CB9 RID: 7353 RVA: 0x00086833 File Offset: 0x00084A33
		public XmlNamespaceMappingCollection() : base(new NameTable())
		{
		}

		/// <summary>For a description of this member, see <see cref="M:System.Windows.Markup.IAddChild.AddChild(System.Object)" />.</summary>
		/// <param name="value">The child <see cref="T:System.Object" /> to add.</param>
		// Token: 0x06001CBA RID: 7354 RVA: 0x00086840 File Offset: 0x00084A40
		void IAddChild.AddChild(object value)
		{
			this.AddChild(value);
		}

		/// <summary>Adds a <see cref="T:System.Windows.Data.XmlNamespaceMapping" /> object to this collection.</summary>
		/// <param name="value">The <see cref="T:System.Windows.Data.XmlNamespaceMapping" /> object to add. This cannot be <see langword="null" />.</param>
		/// <exception cref="T:System.ArgumentException">If <paramref name="mapping" /> is <see langword="null" />.</exception>
		// Token: 0x06001CBB RID: 7355 RVA: 0x0008684C File Offset: 0x00084A4C
		protected virtual void AddChild(object value)
		{
			XmlNamespaceMapping xmlNamespaceMapping = value as XmlNamespaceMapping;
			if (xmlNamespaceMapping == null)
			{
				throw new ArgumentException(SR.Get("RequiresXmlNamespaceMapping", new object[]
				{
					value.GetType().FullName
				}), "value");
			}
			this.Add(xmlNamespaceMapping);
		}

		/// <summary>For a description of this member, see <see cref="M:System.Windows.Markup.IAddChild.AddText(System.String)" />.</summary>
		/// <param name="text">The text to add to the <see cref="T:System.Object" />.</param>
		// Token: 0x06001CBC RID: 7356 RVA: 0x00086899 File Offset: 0x00084A99
		void IAddChild.AddText(string text)
		{
			this.AddText(text);
		}

		/// <summary>Adds a text string as a child of this <see cref="T:System.Windows.Data.XmlNamespaceMappingCollection" /> object.</summary>
		/// <param name="text">The text string to add as a child.</param>
		/// <exception cref="T:System.ArgumentNullException">If <paramref name="text" /> is <see langword="null" />.</exception>
		// Token: 0x06001CBD RID: 7357 RVA: 0x000868A2 File Offset: 0x00084AA2
		protected virtual void AddText(string text)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		/// <summary>Adds a <see cref="T:System.Windows.Data.XmlNamespaceMapping" /> object to this collection.</summary>
		/// <param name="mapping">The <see cref="T:System.Windows.Data.XmlNamespaceMapping" /> object to add. This cannot be <see langword="null" />.</param>
		/// <exception cref="T:System.ArgumentNullException">If <paramref name="mapping" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">If the <see cref="P:System.Windows.Data.XmlNamespaceMapping.Uri" /> of the <see cref="T:System.Windows.Data.XmlNamespaceMapping" /> object is <see langword="null" />. </exception>
		// Token: 0x06001CBE RID: 7358 RVA: 0x000868BC File Offset: 0x00084ABC
		public void Add(XmlNamespaceMapping mapping)
		{
			if (mapping == null)
			{
				throw new ArgumentNullException("mapping");
			}
			if (mapping.Uri == null)
			{
				throw new ArgumentException(SR.Get("RequiresXmlNamespaceMappingUri"), "mapping");
			}
			this.AddNamespace(mapping.Prefix, mapping.Uri.OriginalString);
		}

		/// <summary>Removes all <see cref="T:System.Windows.Data.XmlNamespaceMapping" /> objects in this collection.</summary>
		// Token: 0x06001CBF RID: 7359 RVA: 0x00086918 File Offset: 0x00084B18
		public void Clear()
		{
			int count = this.Count;
			XmlNamespaceMapping[] array = new XmlNamespaceMapping[count];
			this.CopyTo(array, 0);
			for (int i = 0; i < count; i++)
			{
				this.Remove(array[i]);
			}
		}

		/// <summary>Returns a value that indicates whether this collection contains the specified <see cref="T:System.Windows.Data.XmlNamespaceMapping" /> object.</summary>
		/// <param name="mapping">The <see cref="T:System.Windows.Data.XmlNamespaceMapping" /> object of interest. This cannot be <see langword="null" />.</param>
		/// <returns>
		///     <see langword="true" /> if this collection contains the specified <see cref="T:System.Windows.Data.XmlNamespaceMapping" /> object; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">If <paramref name="mapping" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">If the <see cref="P:System.Windows.Data.XmlNamespaceMapping.Uri" /> of the <see cref="T:System.Windows.Data.XmlNamespaceMapping" /> object is <see langword="null" />. </exception>
		// Token: 0x06001CC0 RID: 7360 RVA: 0x00086954 File Offset: 0x00084B54
		public bool Contains(XmlNamespaceMapping mapping)
		{
			if (mapping == null)
			{
				throw new ArgumentNullException("mapping");
			}
			if (mapping.Uri == null)
			{
				throw new ArgumentException(SR.Get("RequiresXmlNamespaceMappingUri"), "mapping");
			}
			return this.LookupNamespace(mapping.Prefix) == mapping.Uri.OriginalString;
		}

		/// <summary>Copies the items of the collection to the specified array, starting at the specified index.</summary>
		/// <param name="array">The array that is the destination of the items copied from the collection.</param>
		/// <param name="arrayIndex">The zero-based index in array at which copying starts.</param>
		/// <exception cref="T:System.ArgumentNullException">If <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">If the number of items exceed the length of the array. </exception>
		// Token: 0x06001CC1 RID: 7361 RVA: 0x000869B4 File Offset: 0x00084BB4
		public void CopyTo(XmlNamespaceMapping[] array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			int num = arrayIndex;
			int num2 = array.Length;
			foreach (object obj in this)
			{
				XmlNamespaceMapping xmlNamespaceMapping = (XmlNamespaceMapping)obj;
				if (num >= num2)
				{
					throw new ArgumentException(SR.Get("Collection_CopyTo_NumberOfElementsExceedsArrayLength", new object[]
					{
						"arrayIndex",
						"array"
					}));
				}
				array[num] = xmlNamespaceMapping;
				num++;
			}
		}

		/// <summary>Removes the specified <see cref="T:System.Windows.Data.XmlNamespaceMapping" /> object from this collection.</summary>
		/// <param name="mapping">The <see cref="T:System.Windows.Data.XmlNamespaceMapping" /> object to remove. This cannot be <see langword="null" />.</param>
		/// <returns>
		///     <see langword="true" /> if the specified <see cref="T:System.Windows.Data.XmlNamespaceMapping" /> object has been successfully removed; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">If <paramref name="mapping" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">If the <see cref="P:System.Windows.Data.XmlNamespaceMapping.Uri" /> of the <see cref="T:System.Windows.Data.XmlNamespaceMapping" /> object is <see langword="null" />. </exception>
		// Token: 0x06001CC2 RID: 7362 RVA: 0x00086A4C File Offset: 0x00084C4C
		public bool Remove(XmlNamespaceMapping mapping)
		{
			if (mapping == null)
			{
				throw new ArgumentNullException("mapping");
			}
			if (mapping.Uri == null)
			{
				throw new ArgumentException(SR.Get("RequiresXmlNamespaceMappingUri"), "mapping");
			}
			if (this.Contains(mapping))
			{
				this.RemoveNamespace(mapping.Prefix, mapping.Uri.OriginalString);
				return true;
			}
			return false;
		}

		/// <summary>Gets the number of <see cref="T:System.Windows.Data.XmlNamespaceMapping" /> objects in the collection.</summary>
		/// <returns>The number of <see cref="T:System.Windows.Data.XmlNamespaceMapping" /> objects in the collection.</returns>
		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x06001CC3 RID: 7363 RVA: 0x00086AB4 File Offset: 0x00084CB4
		public int Count
		{
			get
			{
				int num = 0;
				foreach (object obj in this)
				{
					XmlNamespaceMapping xmlNamespaceMapping = (XmlNamespaceMapping)obj;
					num++;
				}
				return num;
			}
		}

		/// <summary>Gets a value that indicates whether this collection is read-only.</summary>
		/// <returns>This always returns <see langword="false" />.</returns>
		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x06001CC4 RID: 7364 RVA: 0x0000B02A File Offset: 0x0000922A
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		/// <summary>Returns an <see cref="T:System.Collections.IEnumerator" /> object that you can use to enumerate the items in this collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that you can use to enumerate the items in this collection.</returns>
		// Token: 0x06001CC5 RID: 7365 RVA: 0x00086B08 File Offset: 0x00084D08
		public override IEnumerator GetEnumerator()
		{
			return this.ProtectedGetEnumerator();
		}

		// Token: 0x06001CC6 RID: 7366 RVA: 0x00086B08 File Offset: 0x00084D08
		IEnumerator<XmlNamespaceMapping> IEnumerable<XmlNamespaceMapping>.GetEnumerator()
		{
			return this.ProtectedGetEnumerator();
		}

		/// <summary>Returns a generic <see cref="T:System.Collections.Generic.IEnumerator`1" /> object.</summary>
		/// <returns>A generic <see cref="T:System.Collections.Generic.IEnumerator`1" /> object.</returns>
		// Token: 0x06001CC7 RID: 7367 RVA: 0x00086B10 File Offset: 0x00084D10
		protected IEnumerator<XmlNamespaceMapping> ProtectedGetEnumerator()
		{
			IEnumerator enumerator = this.BaseEnumerator;
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				string text = (string)obj;
				if (!(text == "xmlns") && !(text == "xml"))
				{
					string text2 = this.LookupNamespace(text);
					if (!(text == string.Empty) || !(text2 == string.Empty))
					{
						Uri uri = new Uri(text2, UriKind.RelativeOrAbsolute);
						XmlNamespaceMapping xmlNamespaceMapping = new XmlNamespaceMapping(text, uri);
						yield return xmlNamespaceMapping;
					}
				}
			}
			yield break;
		}

		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x06001CC8 RID: 7368 RVA: 0x00086B1F File Offset: 0x00084D1F
		private IEnumerator BaseEnumerator
		{
			get
			{
				return base.GetEnumerator();
			}
		}
	}
}
