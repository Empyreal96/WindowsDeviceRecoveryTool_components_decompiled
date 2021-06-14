using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Windows.Markup.Primitives
{
	// Token: 0x02000281 RID: 641
	internal class ElementDictionaryItemsPseudoProperty : ElementPseudoPropertyBase
	{
		// Token: 0x0600244E RID: 9294 RVA: 0x000B09B1 File Offset: 0x000AEBB1
		internal ElementDictionaryItemsPseudoProperty(IDictionary value, Type type, ElementMarkupObject obj) : base(value, type, obj)
		{
			this._value = value;
		}

		// Token: 0x170008DA RID: 2266
		// (get) Token: 0x0600244F RID: 9295 RVA: 0x000B09C3 File Offset: 0x000AEBC3
		public override string Name
		{
			get
			{
				return "Entries";
			}
		}

		// Token: 0x170008DB RID: 2267
		// (get) Token: 0x06002450 RID: 9296 RVA: 0x00016748 File Offset: 0x00014948
		public override bool IsContent
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170008DC RID: 2268
		// (get) Token: 0x06002451 RID: 9297 RVA: 0x00016748 File Offset: 0x00014948
		public override bool IsComposite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170008DD RID: 2269
		// (get) Token: 0x06002452 RID: 9298 RVA: 0x000B09CC File Offset: 0x000AEBCC
		public override IEnumerable<MarkupObject> Items
		{
			get
			{
				foreach (object obj in this._value)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					ElementMarkupObject elementMarkupObject = new ElementMarkupObject(dictionaryEntry.Value, base.Manager);
					elementMarkupObject.SetKey(new ElementKey(dictionaryEntry.Key, typeof(object), elementMarkupObject));
					yield return elementMarkupObject;
				}
				IDictionaryEnumerator dictionaryEnumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x04001B2F RID: 6959
		private IDictionary _value;
	}
}
