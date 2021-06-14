using System;
using System.ComponentModel.Design;
using System.Security.Permissions;

namespace System.Windows.Forms.PropertyGridInternal
{
	/// <summary>Contains a set of menu commands used by the designer in Visual Studio.</summary>
	// Token: 0x02000493 RID: 1171
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public class PropertyGridCommands
	{
		/// <summary>Represents the GUID the internal property browser uses to create a shortcut menu.</summary>
		// Token: 0x0400336F RID: 13167
		protected static readonly Guid wfcMenuGroup = new Guid("{a72bd644-1979-4cbc-a620-ea4112198a66}");

		/// <summary>Represents the GUID for the internal property browser’s command set.</summary>
		// Token: 0x04003370 RID: 13168
		protected static readonly Guid wfcMenuCommand = new Guid("{5a51cf82-7619-4a5d-b054-47f438425aa7}");

		/// <summary>Represents the command identifier for the Reset menu item.</summary>
		// Token: 0x04003371 RID: 13169
		public static readonly CommandID Reset = new CommandID(PropertyGridCommands.wfcMenuCommand, 12288);

		/// <summary>Represents the command identifier for the Description menu item.</summary>
		// Token: 0x04003372 RID: 13170
		public static readonly CommandID Description = new CommandID(PropertyGridCommands.wfcMenuCommand, 12289);

		/// <summary>Represents the command identifier for the Hide menu item.</summary>
		// Token: 0x04003373 RID: 13171
		public static readonly CommandID Hide = new CommandID(PropertyGridCommands.wfcMenuCommand, 12290);

		/// <summary>Represents the command identifier for the Commands menu item.</summary>
		// Token: 0x04003374 RID: 13172
		public static readonly CommandID Commands = new CommandID(PropertyGridCommands.wfcMenuCommand, 12304);
	}
}
