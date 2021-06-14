using System;

namespace Microsoft.Win32
{
	/// <summary>Represents an entry in a <see cref="T:Microsoft.Win32.FileDialog" /> custom place list.</summary>
	// Token: 0x02000090 RID: 144
	public sealed class FileDialogCustomPlace
	{
		/// <summary>Initializes a new instance of the <see cref="T:Microsoft.Win32.FileDialogCustomPlace" /> class with the specified known folder GUID. </summary>
		/// <param name="knownFolder">The GUID of a known folder.</param>
		// Token: 0x06000222 RID: 546 RVA: 0x00005849 File Offset: 0x00003A49
		public FileDialogCustomPlace(Guid knownFolder)
		{
			this.KnownFolder = knownFolder;
		}

		/// <summary>Initializes a new instance of the <see cref="T:Microsoft.Win32.FileDialogCustomPlace" /> class with the specified path. </summary>
		/// <param name="path">The path for the folder.</param>
		// Token: 0x06000223 RID: 547 RVA: 0x00005858 File Offset: 0x00003A58
		public FileDialogCustomPlace(string path)
		{
			this.Path = (path ?? "");
		}

		/// <summary>Gets the GUID of the known folder for the custom place.</summary>
		/// <returns>The GUID of a known folder.</returns>
		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000224 RID: 548 RVA: 0x00005870 File Offset: 0x00003A70
		// (set) Token: 0x06000225 RID: 549 RVA: 0x00005878 File Offset: 0x00003A78
		public Guid KnownFolder { get; private set; }

		/// <summary>Gets the file path for the custom place.</summary>
		/// <returns>The path for a custom place.</returns>
		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000226 RID: 550 RVA: 0x00005881 File Offset: 0x00003A81
		// (set) Token: 0x06000227 RID: 551 RVA: 0x00005889 File Offset: 0x00003A89
		public string Path { get; private set; }
	}
}
