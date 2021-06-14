using System;
using System.ComponentModel.Design;
using System.Security.Permissions;

namespace System.Drawing.Design
{
	/// <summary>Provides data for the <see cref="E:System.Drawing.Design.ToolboxItem.ComponentsCreating" /> event that occurs when components are added to the toolbox.</summary>
	// Token: 0x0200007D RID: 125
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public class ToolboxComponentsCreatingEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Design.ToolboxComponentsCreatingEventArgs" /> class.</summary>
		/// <param name="host">The designer host that is making the request. </param>
		// Token: 0x0600086F RID: 2159 RVA: 0x00020E16 File Offset: 0x0001F016
		public ToolboxComponentsCreatingEventArgs(IDesignerHost host)
		{
			this.host = host;
		}

		/// <summary>Gets or sets an instance of the <see cref="T:System.ComponentModel.Design.IDesignerHost" /> that made the request to create toolbox components.</summary>
		/// <returns>The <see cref="T:System.ComponentModel.Design.IDesignerHost" /> that made the request to create toolbox components, or <see langword="null" /> if no designer host was provided to the toolbox item.</returns>
		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06000870 RID: 2160 RVA: 0x00020E25 File Offset: 0x0001F025
		public IDesignerHost DesignerHost
		{
			get
			{
				return this.host;
			}
		}

		// Token: 0x0400070B RID: 1803
		private readonly IDesignerHost host;
	}
}
