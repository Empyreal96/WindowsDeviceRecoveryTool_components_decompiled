using System;
using System.Globalization;
using System.Text;

namespace System.Windows.Forms
{
	/// <summary>Represents an entry in a <see cref="T:System.Windows.Forms.FileDialog" /> custom place collection for Windows Vista.</summary>
	// Token: 0x0200023F RID: 575
	public class FileDialogCustomPlace
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.FileDialogCustomPlace" /> class. with a specified folder path to a custom place.</summary>
		/// <param name="path">A folder path to the custom place.</param>
		// Token: 0x0600224A RID: 8778 RVA: 0x000A72A8 File Offset: 0x000A54A8
		public FileDialogCustomPlace(string path)
		{
			this.Path = path;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.FileDialogCustomPlace" /> class with a custom place identified by a Windows Vista Known Folder GUID.</summary>
		/// <param name="knownFolderGuid">A <see cref="T:System.Guid" /> that represents a Windows Vista Known Folder. </param>
		// Token: 0x0600224B RID: 8779 RVA: 0x000A72CD File Offset: 0x000A54CD
		public FileDialogCustomPlace(Guid knownFolderGuid)
		{
			this.KnownFolderGuid = knownFolderGuid;
		}

		/// <summary>Gets or sets the folder path to the custom place.</summary>
		/// <returns>A folder path to the custom place. If the custom place was specified with a Windows Vista Known Folder GUID, then an empty string is returned.</returns>
		// Token: 0x17000836 RID: 2102
		// (get) Token: 0x0600224C RID: 8780 RVA: 0x000A72F2 File Offset: 0x000A54F2
		// (set) Token: 0x0600224D RID: 8781 RVA: 0x000A730D File Offset: 0x000A550D
		public string Path
		{
			get
			{
				if (string.IsNullOrEmpty(this._path))
				{
					return string.Empty;
				}
				return this._path;
			}
			set
			{
				this._path = (value ?? "");
				this._knownFolderGuid = Guid.Empty;
			}
		}

		/// <summary>Gets or sets the Windows Vista Known Folder GUID for the custom place.</summary>
		/// <returns>A <see cref="T:System.Guid" /> that indicates the Windows Vista Known Folder for the custom place. If the custom place was specified with a folder path, then an empty GUID is returned. For a list of the available Windows Vista Known Folders, see Known Folder GUIDs for File Dialog Custom Places or the KnownFolders.h file in the Windows SDK.</returns>
		// Token: 0x17000837 RID: 2103
		// (get) Token: 0x0600224E RID: 8782 RVA: 0x000A732A File Offset: 0x000A552A
		// (set) Token: 0x0600224F RID: 8783 RVA: 0x000A7332 File Offset: 0x000A5532
		public Guid KnownFolderGuid
		{
			get
			{
				return this._knownFolderGuid;
			}
			set
			{
				this._path = string.Empty;
				this._knownFolderGuid = value;
			}
		}

		/// <summary>Returns a string that represents this <see cref="T:System.Windows.Forms.FileDialogCustomPlace" /> instance.</summary>
		/// <returns>A string that represents this <see cref="T:System.Windows.Forms.FileDialogCustomPlace" /> instance.</returns>
		// Token: 0x06002250 RID: 8784 RVA: 0x000A7346 File Offset: 0x000A5546
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "{0} Path: {1} KnownFolderGuid: {2}", new object[]
			{
				base.ToString(),
				this.Path,
				this.KnownFolderGuid
			});
		}

		// Token: 0x06002251 RID: 8785 RVA: 0x000A7380 File Offset: 0x000A5580
		internal FileDialogNative.IShellItem GetNativePath()
		{
			string text;
			if (!string.IsNullOrEmpty(this._path))
			{
				text = this._path;
			}
			else
			{
				text = FileDialogCustomPlace.GetFolderLocation(this._knownFolderGuid);
			}
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			return FileDialog.GetShellItemForPath(text);
		}

		// Token: 0x06002252 RID: 8786 RVA: 0x000A73C8 File Offset: 0x000A55C8
		private static string GetFolderLocation(Guid folderGuid)
		{
			if (!UnsafeNativeMethods.IsVista)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (UnsafeNativeMethods.Shell32.SHGetFolderPathEx(ref folderGuid, 0U, IntPtr.Zero, stringBuilder) == 0)
			{
				return stringBuilder.ToString();
			}
			return null;
		}

		// Token: 0x04000EE7 RID: 3815
		private string _path = "";

		// Token: 0x04000EE8 RID: 3816
		private Guid _knownFolderGuid = Guid.Empty;
	}
}
