using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Represents the windows contained within another <see cref="T:System.Windows.Forms.HtmlWindow" />.</summary>
	// Token: 0x02000276 RID: 630
	public class HtmlWindowCollection : ICollection, IEnumerable
	{
		// Token: 0x0600262E RID: 9774 RVA: 0x000B5793 File Offset: 0x000B3993
		internal HtmlWindowCollection(HtmlShimManager shimManager, UnsafeNativeMethods.IHTMLFramesCollection2 collection)
		{
			this.htmlFramesCollection2 = collection;
			this.shimManager = shimManager;
		}

		// Token: 0x17000940 RID: 2368
		// (get) Token: 0x0600262F RID: 9775 RVA: 0x000B57A9 File Offset: 0x000B39A9
		private UnsafeNativeMethods.IHTMLFramesCollection2 NativeHTMLFramesCollection2
		{
			get
			{
				return this.htmlFramesCollection2;
			}
		}

		/// <summary>Retrieves a frame window by supplying the frame's position in the collection.</summary>
		/// <param name="index">The position of the <see cref="T:System.Windows.Forms.HtmlWindow" /> within the collection.</param>
		/// <returns>The <see cref="T:System.Windows.Forms.HtmlWindow" /> corresponding to the requested frame.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is greater than the number of items in the collection.</exception>
		// Token: 0x17000941 RID: 2369
		public HtmlWindow this[int index]
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
				object obj = index;
				UnsafeNativeMethods.IHTMLWindow2 ihtmlwindow = this.NativeHTMLFramesCollection2.Item(ref obj) as UnsafeNativeMethods.IHTMLWindow2;
				if (ihtmlwindow == null)
				{
					return null;
				}
				return new HtmlWindow(this.shimManager, ihtmlwindow);
			}
		}

		/// <summary>Retrieves a frame window by supplying the frame's name.</summary>
		/// <param name="windowId">The name of the <see cref="T:System.Windows.Forms.HtmlWindow" /> to retrieve.</param>
		/// <returns>The <see cref="T:System.Windows.Forms.HtmlWindow" /> element corresponding to the supplied name. </returns>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="windowId" /> is not the name of a <see langword="Frame" /> object in the current document or in any of its children.</exception>
		// Token: 0x17000942 RID: 2370
		public HtmlWindow this[string windowId]
		{
			get
			{
				object obj = windowId;
				UnsafeNativeMethods.IHTMLWindow2 ihtmlwindow = null;
				try
				{
					ihtmlwindow = (this.htmlFramesCollection2.Item(ref obj) as UnsafeNativeMethods.IHTMLWindow2);
				}
				catch (COMException)
				{
					throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
					{
						"windowId",
						windowId
					}));
				}
				if (ihtmlwindow == null)
				{
					return null;
				}
				return new HtmlWindow(this.shimManager, ihtmlwindow);
			}
		}

		/// <summary>Gets the number of elements in the collection. </summary>
		/// <returns>The number of <see cref="T:System.Windows.Forms.HtmlWindow" /> objects in the current <see cref="T:System.Windows.Forms.HtmlWindowCollection" />.</returns>
		// Token: 0x17000943 RID: 2371
		// (get) Token: 0x06002632 RID: 9778 RVA: 0x000B58AC File Offset: 0x000B3AAC
		public int Count
		{
			get
			{
				return this.NativeHTMLFramesCollection2.GetLength();
			}
		}

		/// <summary>Gets a value indicating whether access to the collection is synchronized (thread safe).</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x17000944 RID: 2372
		// (get) Token: 0x06002633 RID: 9779 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to the collection.</summary>
		/// <returns>An object that can be used to synchronize access to the collection.</returns>
		// Token: 0x17000945 RID: 2373
		// (get) Token: 0x06002634 RID: 9780 RVA: 0x000069BD File Offset: 0x00004BBD
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		/// <summary>Copies the elements of the collection to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.</summary>
		/// <param name="dest">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from collection. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in <see langword="Array" /> at which copying begins.</param>
		// Token: 0x06002635 RID: 9781 RVA: 0x000B58BC File Offset: 0x000B3ABC
		void ICollection.CopyTo(Array dest, int index)
		{
			int count = this.Count;
			for (int i = 0; i < count; i++)
			{
				dest.SetValue(this[i], index++);
			}
		}

		/// <summary>Returns an enumerator that can iterate through all elements in the <see cref="T:System.Windows.Forms.HtmlWindowCollection" />.</summary>
		/// <returns>The <see cref="T:System.Collections.IEnumerator" /> that enables enumeration of this collection's elements.</returns>
		// Token: 0x06002636 RID: 9782 RVA: 0x000B58F0 File Offset: 0x000B3AF0
		public IEnumerator GetEnumerator()
		{
			HtmlWindow[] array = new HtmlWindow[this.Count];
			((ICollection)this).CopyTo(array, 0);
			return array.GetEnumerator();
		}

		// Token: 0x04001031 RID: 4145
		private UnsafeNativeMethods.IHTMLFramesCollection2 htmlFramesCollection2;

		// Token: 0x04001032 RID: 4146
		private HtmlShimManager shimManager;
	}
}
