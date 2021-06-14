using System;

namespace System.Windows.Shell
{
	/// <summary>Represents the base class for the <see cref="T:System.Windows.Shell.JumpPath" /> and <see cref="T:System.Windows.Shell.JumpTask" /> classes.</summary>
	// Token: 0x02000144 RID: 324
	public abstract class JumpItem
	{
		// Token: 0x06000E4F RID: 3663 RVA: 0x0000326D File Offset: 0x0000146D
		internal JumpItem()
		{
		}

		/// <summary>Gets or sets the name of the category the <see cref="T:System.Windows.Shell.JumpItem" /> is grouped with in the Windows 7 taskbar Jump List.</summary>
		/// <returns>The name of the category the <see cref="T:System.Windows.Shell.JumpItem" /> is grouped with. The default is <see langword="null" />.</returns>
		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x06000E50 RID: 3664 RVA: 0x00036DBA File Offset: 0x00034FBA
		// (set) Token: 0x06000E51 RID: 3665 RVA: 0x00036DC2 File Offset: 0x00034FC2
		public string CustomCategory { get; set; }
	}
}
