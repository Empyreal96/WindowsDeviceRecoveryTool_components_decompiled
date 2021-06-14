using System;
using System.Collections;

namespace System.Windows.Forms
{
	/// <summary>Defines a collection of <see cref="T:System.Windows.Forms.HtmlElement" /> objects.</summary>
	// Token: 0x0200026C RID: 620
	public sealed class HtmlElementCollection : ICollection, IEnumerable
	{
		// Token: 0x06002590 RID: 9616 RVA: 0x000B4383 File Offset: 0x000B2583
		internal HtmlElementCollection(HtmlShimManager shimManager)
		{
			this.htmlElementCollection = null;
			this.elementsArray = null;
			this.shimManager = shimManager;
		}

		// Token: 0x06002591 RID: 9617 RVA: 0x000B43A0 File Offset: 0x000B25A0
		internal HtmlElementCollection(HtmlShimManager shimManager, UnsafeNativeMethods.IHTMLElementCollection elements)
		{
			this.htmlElementCollection = elements;
			this.elementsArray = null;
			this.shimManager = shimManager;
		}

		// Token: 0x06002592 RID: 9618 RVA: 0x000B43BD File Offset: 0x000B25BD
		internal HtmlElementCollection(HtmlShimManager shimManager, HtmlElement[] array)
		{
			this.htmlElementCollection = null;
			this.elementsArray = array;
			this.shimManager = shimManager;
		}

		// Token: 0x17000911 RID: 2321
		// (get) Token: 0x06002593 RID: 9619 RVA: 0x000B43DA File Offset: 0x000B25DA
		private UnsafeNativeMethods.IHTMLElementCollection NativeHtmlElementCollection
		{
			get
			{
				return this.htmlElementCollection;
			}
		}

		/// <summary>Gets an item from the collection by specifying its numerical index.</summary>
		/// <param name="index">The position from which to retrieve an item from the collection.</param>
		/// <returns>An item from the collection by specifying its numerical index.</returns>
		// Token: 0x17000912 RID: 2322
		public HtmlElement this[int index]
		{
			get
			{
				if (index < 0 || index >= this.Count)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidBoundArgument", new object[]
					{
						"index",
						index,
						0,
						this.Count - 1
					}));
				}
				if (this.NativeHtmlElementCollection != null)
				{
					UnsafeNativeMethods.IHTMLElement ihtmlelement = this.NativeHtmlElementCollection.Item(index, 0) as UnsafeNativeMethods.IHTMLElement;
					if (ihtmlelement == null)
					{
						return null;
					}
					return new HtmlElement(this.shimManager, ihtmlelement);
				}
				else
				{
					if (this.elementsArray != null)
					{
						return this.elementsArray[index];
					}
					return null;
				}
			}
		}

		/// <summary>Gets an item from the collection by specifying its name.</summary>
		/// <param name="elementId">The <see cref="P:System.Windows.Forms.HtmlElement.Name" /> or <see cref="P:System.Windows.Forms.HtmlElement.Id" /> attribute of the element.</param>
		/// <returns>An <see cref="T:System.Windows.Forms.HtmlElement" />, if the named element is found. Otherwise, <see langword="null" />.</returns>
		// Token: 0x17000913 RID: 2323
		public HtmlElement this[string elementId]
		{
			get
			{
				if (this.NativeHtmlElementCollection != null)
				{
					UnsafeNativeMethods.IHTMLElement ihtmlelement = this.NativeHtmlElementCollection.Item(elementId, 0) as UnsafeNativeMethods.IHTMLElement;
					if (ihtmlelement == null)
					{
						return null;
					}
					return new HtmlElement(this.shimManager, ihtmlelement);
				}
				else
				{
					if (this.elementsArray != null)
					{
						int num = this.elementsArray.Length;
						for (int i = 0; i < num; i++)
						{
							HtmlElement htmlElement = this.elementsArray[i];
							if (htmlElement.Id == elementId)
							{
								return htmlElement;
							}
						}
						return null;
					}
					return null;
				}
			}
		}

		/// <summary>Gets a collection of elements by their name.</summary>
		/// <param name="name">The name or ID of the element. </param>
		/// <returns>An <see cref="T:System.Windows.Forms.HtmlElementCollection" /> containing the elements whose <see cref="P:System.Windows.Forms.HtmlElement.Name" /> property match <paramref name="name" />. </returns>
		// Token: 0x06002596 RID: 9622 RVA: 0x000B4504 File Offset: 0x000B2704
		public HtmlElementCollection GetElementsByName(string name)
		{
			int count = this.Count;
			HtmlElement[] array = new HtmlElement[count];
			int num = 0;
			for (int i = 0; i < count; i++)
			{
				HtmlElement htmlElement = this[i];
				if (htmlElement.GetAttribute("name") == name)
				{
					array[num] = htmlElement;
					num++;
				}
			}
			if (num == 0)
			{
				return new HtmlElementCollection(this.shimManager);
			}
			HtmlElement[] array2 = new HtmlElement[num];
			for (int j = 0; j < num; j++)
			{
				array2[j] = array[j];
			}
			return new HtmlElementCollection(this.shimManager, array2);
		}

		/// <summary>Gets the number of elements in the collection. </summary>
		/// <returns>An <see cref="T:System.Int32" /> representing the number of elements in the collection.</returns>
		// Token: 0x17000914 RID: 2324
		// (get) Token: 0x06002597 RID: 9623 RVA: 0x000B4590 File Offset: 0x000B2790
		public int Count
		{
			get
			{
				if (this.NativeHtmlElementCollection != null)
				{
					return this.NativeHtmlElementCollection.GetLength();
				}
				if (this.elementsArray != null)
				{
					return this.elementsArray.Length;
				}
				return 0;
			}
		}

		/// <summary>Gets a value indicating whether access to the <see cref="T:System.Windows.Forms.HtmlElementCollection" /> is synchronized (thread safe).</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x17000915 RID: 2325
		// (get) Token: 0x06002598 RID: 9624 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to the collection.</summary>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.</returns>
		// Token: 0x17000916 RID: 2326
		// (get) Token: 0x06002599 RID: 9625 RVA: 0x000069BD File Offset: 0x00004BBD
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		/// <summary>Copies the elements of the collection to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.</summary>
		/// <param name="dest">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from collection. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
		// Token: 0x0600259A RID: 9626 RVA: 0x000B45B8 File Offset: 0x000B27B8
		void ICollection.CopyTo(Array dest, int index)
		{
			int count = this.Count;
			for (int i = 0; i < count; i++)
			{
				dest.SetValue(this[i], index++);
			}
		}

		/// <summary>Returns an enumerator that iterates through a collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through the collection.</returns>
		// Token: 0x0600259B RID: 9627 RVA: 0x000B45EC File Offset: 0x000B27EC
		public IEnumerator GetEnumerator()
		{
			HtmlElement[] array = new HtmlElement[this.Count];
			((ICollection)this).CopyTo(array, 0);
			return array.GetEnumerator();
		}

		// Token: 0x04001012 RID: 4114
		private UnsafeNativeMethods.IHTMLElementCollection htmlElementCollection;

		// Token: 0x04001013 RID: 4115
		private HtmlElement[] elementsArray;

		// Token: 0x04001014 RID: 4116
		private HtmlShimManager shimManager;
	}
}
