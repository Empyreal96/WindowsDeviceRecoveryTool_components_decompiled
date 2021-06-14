using System;

namespace System.Windows.Shell
{
	/// <summary>Represents a shortcut to an application in the Windows 7 taskbar Jump List.</summary>
	// Token: 0x0200014A RID: 330
	public class JumpTask : JumpItem
	{
		/// <summary>Gets or sets the text displayed for the task in the Jump List.</summary>
		/// <returns>The text displayed for the task in the Jump List. The default is <see langword="null" />.</returns>
		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x06000E82 RID: 3714 RVA: 0x0003807E File Offset: 0x0003627E
		// (set) Token: 0x06000E83 RID: 3715 RVA: 0x00038086 File Offset: 0x00036286
		public string Title { get; set; }

		/// <summary>Gets or sets the text displayed in the tooltip for the task in the Jump List.</summary>
		/// <returns>The text displayed in the tooltip for the task. The default is <see langword="null" />.</returns>
		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x06000E84 RID: 3716 RVA: 0x0003808F File Offset: 0x0003628F
		// (set) Token: 0x06000E85 RID: 3717 RVA: 0x00038097 File Offset: 0x00036297
		public string Description { get; set; }

		/// <summary>Gets or sets the path to the application.</summary>
		/// <returns>The path to the application. The default is <see langword="null" />.</returns>
		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x06000E86 RID: 3718 RVA: 0x000380A0 File Offset: 0x000362A0
		// (set) Token: 0x06000E87 RID: 3719 RVA: 0x000380A8 File Offset: 0x000362A8
		public string ApplicationPath { get; set; }

		/// <summary>Gets or sets the arguments passed to the application on startup.</summary>
		/// <returns>The arguments passed to the application on startup. The default is <see langword="null" />.</returns>
		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x06000E88 RID: 3720 RVA: 0x000380B1 File Offset: 0x000362B1
		// (set) Token: 0x06000E89 RID: 3721 RVA: 0x000380B9 File Offset: 0x000362B9
		public string Arguments { get; set; }

		/// <summary>Gets or sets the working directory of the application on startup.</summary>
		/// <returns>The working directory of the application on startup. The default is <see langword="null" />.</returns>
		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x06000E8A RID: 3722 RVA: 0x000380C2 File Offset: 0x000362C2
		// (set) Token: 0x06000E8B RID: 3723 RVA: 0x000380CA File Offset: 0x000362CA
		public string WorkingDirectory { get; set; }

		/// <summary>Gets or sets the path to a resource that contains the icon to display in the Jump List.</summary>
		/// <returns>The path to a resource that contains the icon. The default is <see langword="null" />.</returns>
		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x06000E8C RID: 3724 RVA: 0x000380D3 File Offset: 0x000362D3
		// (set) Token: 0x06000E8D RID: 3725 RVA: 0x000380DB File Offset: 0x000362DB
		public string IconResourcePath { get; set; }

		/// <summary>Gets or sets the zero-based index of an icon embedded in a resource.</summary>
		/// <returns>The zero-based index of the icon, or -1 if no icon is used. The default is 0.</returns>
		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x06000E8E RID: 3726 RVA: 0x000380E4 File Offset: 0x000362E4
		// (set) Token: 0x06000E8F RID: 3727 RVA: 0x000380EC File Offset: 0x000362EC
		public int IconResourceIndex { get; set; }
	}
}
