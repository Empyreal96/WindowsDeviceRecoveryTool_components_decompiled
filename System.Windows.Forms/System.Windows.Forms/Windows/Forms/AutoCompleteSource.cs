using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies the source for <see cref="T:System.Windows.Forms.ComboBox" /> and <see cref="T:System.Windows.Forms.TextBox" /> automatic completion functionality.</summary>
	// Token: 0x02000118 RID: 280
	public enum AutoCompleteSource
	{
		/// <summary>Specifies the file system as the source.</summary>
		// Token: 0x04000559 RID: 1369
		FileSystem = 1,
		/// <summary>Includes the Uniform Resource Locators (URLs) in the history list.</summary>
		// Token: 0x0400055A RID: 1370
		HistoryList,
		/// <summary>Includes the Uniform Resource Locators (URLs) in the list of those URLs most recently used.</summary>
		// Token: 0x0400055B RID: 1371
		RecentlyUsedList = 4,
		/// <summary>Specifies the equivalent of <see cref="F:System.Windows.Forms.AutoCompleteSource.HistoryList" /> and <see cref="F:System.Windows.Forms.AutoCompleteSource.RecentlyUsedList" /> as the source.</summary>
		// Token: 0x0400055C RID: 1372
		AllUrl = 6,
		/// <summary>Specifies the equivalent of <see cref="F:System.Windows.Forms.AutoCompleteSource.FileSystem" /> and <see cref="F:System.Windows.Forms.AutoCompleteSource.AllUrl" /> as the source. This is the default value when <see cref="T:System.Windows.Forms.AutoCompleteMode" /> has been set to a value other than the default.</summary>
		// Token: 0x0400055D RID: 1373
		AllSystemSources,
		/// <summary>Specifies that only directory names and not file names will be automatically completed.</summary>
		// Token: 0x0400055E RID: 1374
		FileSystemDirectories = 32,
		/// <summary>Specifies strings from a built-in <see cref="T:System.Windows.Forms.AutoCompleteStringCollection" /> as the source.</summary>
		// Token: 0x0400055F RID: 1375
		CustomSource = 64,
		/// <summary>Specifies that no <see cref="T:System.Windows.Forms.AutoCompleteSource" /> is currently in use. This is the default value of <see cref="T:System.Windows.Forms.AutoCompleteSource" />.</summary>
		// Token: 0x04000560 RID: 1376
		None = 128,
		/// <summary>Specifies that the items of the <see cref="T:System.Windows.Forms.ComboBox" /> represent the source.</summary>
		// Token: 0x04000561 RID: 1377
		ListItems = 256
	}
}
