using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Markup;

namespace System.Windows.Documents.DocumentStructures
{
	/// <summary>Represents a set of one or more <see cref="T:System.Windows.Documents.DocumentStructures.StoryFragment" /> elements.</summary>
	// Token: 0x02000457 RID: 1111
	[ContentProperty("StoryFragmentList")]
	public class StoryFragments : IAddChild, IEnumerable<StoryFragment>, IEnumerable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.DocumentStructures.StoryFragments" /> class. </summary>
		// Token: 0x06004037 RID: 16439 RVA: 0x00126070 File Offset: 0x00124270
		public StoryFragments()
		{
			this._elementList = new List<StoryFragment>();
		}

		/// <summary>Adds a <see cref="T:System.Windows.Documents.DocumentStructures.StoryFragment" /> to the <see cref="T:System.Windows.Documents.DocumentStructures.StoryFragments" /> collection.</summary>
		/// <param name="storyFragment">The <see cref="T:System.Windows.Documents.DocumentStructures.StoryFragment" /> to add.</param>
		/// <exception cref="T:System.ArgumentNullException">The <see cref="T:System.Windows.Documents.DocumentStructures.StoryFragment" /> is <see langword="null" />.</exception>
		// Token: 0x06004038 RID: 16440 RVA: 0x00126083 File Offset: 0x00124283
		public void Add(StoryFragment storyFragment)
		{
			if (storyFragment == null)
			{
				throw new ArgumentNullException("storyFragment");
			}
			((IAddChild)this).AddChild(storyFragment);
		}

		/// <summary>Adds a child object to the <see cref="T:System.Windows.Documents.DocumentStructures.StoryFragments" />.</summary>
		/// <param name="value">The child object to add.</param>
		// Token: 0x06004039 RID: 16441 RVA: 0x0012609C File Offset: 0x0012429C
		void IAddChild.AddChild(object value)
		{
			if (value is StoryFragment)
			{
				this._elementList.Add((StoryFragment)value);
				return;
			}
			throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
			{
				value.GetType(),
				typeof(StoryFragment)
			}), "value");
		}

		/// <summary>Adds the text content of a node to the object.</summary>
		/// <param name="text">The text to add to the object.</param>
		// Token: 0x0600403A RID: 16442 RVA: 0x00002137 File Offset: 0x00000337
		void IAddChild.AddText(string text)
		{
		}

		// Token: 0x0600403B RID: 16443 RVA: 0x00041D30 File Offset: 0x0003FF30
		IEnumerator<StoryFragment> IEnumerable<StoryFragment>.GetEnumerator()
		{
			throw new NotSupportedException();
		}

		/// <summary>This API is not implemented.</summary>
		/// <returns>This API is not implemented.</returns>
		// Token: 0x0600403C RID: 16444 RVA: 0x001260F3 File Offset: 0x001242F3
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StoryFragment>)this).GetEnumerator();
		}

		// Token: 0x17000FDC RID: 4060
		// (get) Token: 0x0600403D RID: 16445 RVA: 0x001260FB File Offset: 0x001242FB
		internal List<StoryFragment> StoryFragmentList
		{
			get
			{
				return this._elementList;
			}
		}

		// Token: 0x0400275E RID: 10078
		private List<StoryFragment> _elementList;
	}
}
