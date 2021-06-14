using System;
using System.Collections;
using System.Configuration.Install;
using System.IO;
using System.Reflection;
using System.Security.Permissions;
using System.Text;

namespace System.Management.Instrumentation
{
	/// <summary>Installs instrumented assemblies. Include an instance of this installer class in the project installer for an assembly that includes instrumentation.          Note: the WMI .NET libraries are now considered in final state, and no further development, enhancements, or updates will be available for non-security related issues affecting these libraries. The MI APIs should be used for all new development.</summary>
	// Token: 0x020000BE RID: 190
	public class ManagementInstaller : Installer
	{
		/// <summary>Gets or sets installer options for this class.          </summary>
		/// <returns>Returns a <see cref="T:System.String" /> value containing the installer options for this class.</returns>
		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x0600050F RID: 1295 RVA: 0x00024028 File Offset: 0x00022228
		public override string HelpText
		{
			get
			{
				if (ManagementInstaller.helpPrinted)
				{
					return base.HelpText;
				}
				ManagementInstaller.helpPrinted = true;
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("/MOF=[filename]\r\n");
				stringBuilder.Append(" " + RC.GetString("FILETOWRITE_MOF") + "\r\n\r\n");
				stringBuilder.Append("/Force or /F\r\n");
				stringBuilder.Append(" " + RC.GetString("FORCE_UPDATE"));
				return stringBuilder.ToString() + base.HelpText;
			}
		}

		/// <summary>Installs the assembly.          </summary>
		/// <param name="savedState">The state of the assembly.</param>
		// Token: 0x06000510 RID: 1296 RVA: 0x000240B4 File Offset: 0x000222B4
		public override void Install(IDictionary savedState)
		{
			FileIOPermission fileIOPermission = new FileIOPermission(FileIOPermissionAccess.Read, base.Context.Parameters["assemblypath"]);
			fileIOPermission.Demand();
			base.Install(savedState);
			base.Context.LogMessage(RC.GetString("WMISCHEMA_INSTALLATIONSTART"));
			string assemblyFile = base.Context.Parameters["assemblypath"];
			Assembly assembly = Assembly.LoadFrom(assemblyFile);
			SchemaNaming schemaNaming = SchemaNaming.GetSchemaNaming(assembly);
			schemaNaming.DecoupledProviderInstanceName = AssemblyNameUtility.UniqueToAssemblyFullVersion(assembly);
			if (schemaNaming == null)
			{
				return;
			}
			if (!schemaNaming.IsAssemblyRegistered() || base.Context.Parameters.ContainsKey("force") || base.Context.Parameters.ContainsKey("f"))
			{
				base.Context.LogMessage(RC.GetString("REGESTRING_ASSEMBLY") + " " + schemaNaming.DecoupledProviderInstanceName);
				schemaNaming.RegisterNonAssemblySpecificSchema(base.Context);
				schemaNaming.RegisterAssemblySpecificSchema();
			}
			this.mof = schemaNaming.Mof;
			base.Context.LogMessage(RC.GetString("WMISCHEMA_INSTALLATIONEND"));
		}

		/// <summary>Commits the assembly to the operation.          </summary>
		/// <param name="savedState">The state of the assembly.</param>
		// Token: 0x06000511 RID: 1297 RVA: 0x000241C4 File Offset: 0x000223C4
		public override void Commit(IDictionary savedState)
		{
			base.Commit(savedState);
			if (base.Context.Parameters.ContainsKey("mof"))
			{
				string text = base.Context.Parameters["mof"];
				if (text == null || text.Length == 0)
				{
					text = base.Context.Parameters["assemblypath"];
					if (text == null || text.Length == 0)
					{
						text = "defaultmoffile";
					}
					else
					{
						text = Path.GetFileName(text);
					}
				}
				if (text.Length < 4)
				{
					text += ".mof";
				}
				else if (string.Compare(text.Substring(text.Length - 4, 4), ".mof", StringComparison.OrdinalIgnoreCase) != 0)
				{
					text += ".mof";
				}
				base.Context.LogMessage(RC.GetString("MOFFILE_GENERATING") + " " + text);
				using (StreamWriter streamWriter = new StreamWriter(text, false, Encoding.Unicode))
				{
					streamWriter.WriteLine("//**************************************************************************");
					streamWriter.WriteLine("//* {0}", text);
					streamWriter.WriteLine("//**************************************************************************");
					streamWriter.WriteLine(this.mof);
				}
			}
		}

		/// <summary>Rolls back the state of the assembly.          </summary>
		/// <param name="savedState">The state of the assembly.</param>
		// Token: 0x06000512 RID: 1298 RVA: 0x000242FC File Offset: 0x000224FC
		public override void Rollback(IDictionary savedState)
		{
			base.Rollback(savedState);
		}

		/// <summary>Uninstalls the assembly.          </summary>
		/// <param name="savedState">The state of the assembly.</param>
		// Token: 0x06000513 RID: 1299 RVA: 0x00024305 File Offset: 0x00022505
		public override void Uninstall(IDictionary savedState)
		{
			base.Uninstall(savedState);
		}

		// Token: 0x04000513 RID: 1299
		private static bool helpPrinted;

		// Token: 0x04000514 RID: 1300
		private string mof;
	}
}
