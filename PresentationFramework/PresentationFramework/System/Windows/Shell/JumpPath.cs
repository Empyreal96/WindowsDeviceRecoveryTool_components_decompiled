using System;

namespace System.Windows.Shell
{
	/// <summary>Represents a link to a file that is displayed in a Windows 7 taskbar Jump List.</summary>
	// Token: 0x02000149 RID: 329
	public class JumpPath : JumpItem
	{
		/// <summary>Gets or sets the path to the file to be included in the Jump List.</summary>
		/// <returns>The path to the file to be included in the Jump List.</returns>
		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x06000E7F RID: 3711 RVA: 0x0003806D File Offset: 0x0003626D
		// (set) Token: 0x06000E80 RID: 3712 RVA: 0x00038075 File Offset: 0x00036275
		public string Path { get; set; }
	}
}
