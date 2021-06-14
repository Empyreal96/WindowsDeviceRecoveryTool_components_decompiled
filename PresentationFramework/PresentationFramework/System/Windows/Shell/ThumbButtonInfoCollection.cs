using System;

namespace System.Windows.Shell
{
	/// <summary>Represents a collection of <see cref="T:System.Windows.Shell.ThumbButtonInfo" /> objects that are associated with a <see cref="T:System.Windows.Window" />.</summary>
	// Token: 0x0200014E RID: 334
	public class ThumbButtonInfoCollection : FreezableCollection<ThumbButtonInfo>
	{
		/// <summary>Creates a new instance of the collection.</summary>
		/// <returns>The new instance of the collection.</returns>
		// Token: 0x06000EC9 RID: 3785 RVA: 0x00038A6C File Offset: 0x00036C6C
		protected override Freezable CreateInstanceCore()
		{
			return new ThumbButtonInfoCollection();
		}

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x06000ECA RID: 3786 RVA: 0x00038A74 File Offset: 0x00036C74
		internal static ThumbButtonInfoCollection Empty
		{
			get
			{
				if (ThumbButtonInfoCollection.s_empty == null)
				{
					ThumbButtonInfoCollection thumbButtonInfoCollection = new ThumbButtonInfoCollection();
					thumbButtonInfoCollection.Freeze();
					ThumbButtonInfoCollection.s_empty = thumbButtonInfoCollection;
				}
				return ThumbButtonInfoCollection.s_empty;
			}
		}

		// Token: 0x04001150 RID: 4432
		private static ThumbButtonInfoCollection s_empty;
	}
}
