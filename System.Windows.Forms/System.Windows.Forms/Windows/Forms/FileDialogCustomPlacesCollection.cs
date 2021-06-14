using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	/// <summary>Represents a collection of Windows Vista custom places for the <see cref="T:System.Windows.Forms.FileDialog" /> class.</summary>
	// Token: 0x02000240 RID: 576
	public class FileDialogCustomPlacesCollection : Collection<FileDialogCustomPlace>
	{
		// Token: 0x06002253 RID: 8787 RVA: 0x000A7400 File Offset: 0x000A5600
		internal void Apply(FileDialogNative.IFileDialog dialog)
		{
			for (int i = base.Items.Count - 1; i >= 0; i--)
			{
				FileDialogCustomPlace fileDialogCustomPlace = base.Items[i];
				FileIOPermission fileIOPermission = new FileIOPermission(FileIOPermissionAccess.PathDiscovery, fileDialogCustomPlace.Path);
				fileIOPermission.Demand();
				try
				{
					FileDialogNative.IShellItem nativePath = fileDialogCustomPlace.GetNativePath();
					if (nativePath != null)
					{
						dialog.AddPlace(nativePath, 0);
					}
				}
				catch (FileNotFoundException)
				{
				}
			}
		}

		/// <summary>Adds a custom place to the <see cref="T:System.Windows.Forms.FileDialogCustomPlacesCollection" /> collection.</summary>
		/// <param name="path">A folder path to the custom place.</param>
		// Token: 0x06002254 RID: 8788 RVA: 0x000A746C File Offset: 0x000A566C
		public void Add(string path)
		{
			base.Add(new FileDialogCustomPlace(path));
		}

		/// <summary>Adds a custom place to the <see cref="T:System.Windows.Forms.FileDialogCustomPlacesCollection" /> collection.</summary>
		/// <param name="knownFolderGuid">A <see cref="T:System.Guid" /> that represents a Windows Vista Known Folder. </param>
		// Token: 0x06002255 RID: 8789 RVA: 0x000A747A File Offset: 0x000A567A
		public void Add(Guid knownFolderGuid)
		{
			base.Add(new FileDialogCustomPlace(knownFolderGuid));
		}
	}
}
