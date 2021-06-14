using System;
using System.Security;
using System.Security.Permissions;

namespace System.Drawing.Printing
{
	/// <summary>Controls access to printers. This class cannot be inherited.</summary>
	// Token: 0x02000069 RID: 105
	[Serializable]
	public sealed class PrintingPermission : CodeAccessPermission, IUnrestrictedPermission
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Printing.PrintingPermission" /> class with either fully restricted or unrestricted access, as specified.</summary>
		/// <param name="state">One of the <see cref="T:System.Security.Permissions.PermissionState" /> values. </param>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="state" /> is not a valid <see cref="T:System.Security.Permissions.PermissionState" />. </exception>
		// Token: 0x060007F7 RID: 2039 RVA: 0x000207F3 File Offset: 0x0001E9F3
		public PrintingPermission(PermissionState state)
		{
			if (state == PermissionState.Unrestricted)
			{
				this.printingLevel = PrintingPermissionLevel.AllPrinting;
				return;
			}
			if (state == PermissionState.None)
			{
				this.printingLevel = PrintingPermissionLevel.NoPrinting;
				return;
			}
			throw new ArgumentException(SR.GetString("InvalidPermissionState"));
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Printing.PrintingPermission" /> class with the level of printing access specified.</summary>
		/// <param name="printingLevel">One of the <see cref="T:System.Drawing.Printing.PrintingPermissionLevel" /> values. </param>
		// Token: 0x060007F8 RID: 2040 RVA: 0x00020821 File Offset: 0x0001EA21
		public PrintingPermission(PrintingPermissionLevel printingLevel)
		{
			PrintingPermission.VerifyPrintingLevel(printingLevel);
			this.printingLevel = printingLevel;
		}

		/// <summary>Gets or sets the code's level of printing access.</summary>
		/// <returns>One of the <see cref="T:System.Drawing.Printing.PrintingPermissionLevel" /> values.</returns>
		// Token: 0x1700030B RID: 779
		// (get) Token: 0x060007F9 RID: 2041 RVA: 0x00020836 File Offset: 0x0001EA36
		// (set) Token: 0x060007FA RID: 2042 RVA: 0x0002083E File Offset: 0x0001EA3E
		public PrintingPermissionLevel Level
		{
			get
			{
				return this.printingLevel;
			}
			set
			{
				PrintingPermission.VerifyPrintingLevel(value);
				this.printingLevel = value;
			}
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x0002084D File Offset: 0x0001EA4D
		private static void VerifyPrintingLevel(PrintingPermissionLevel level)
		{
			if (level < PrintingPermissionLevel.NoPrinting || level > PrintingPermissionLevel.AllPrinting)
			{
				throw new ArgumentException(SR.GetString("InvalidPermissionLevel"));
			}
		}

		/// <summary>Gets a value indicating whether the permission is unrestricted.</summary>
		/// <returns>
		///     <see langword="true" /> if permission is unrestricted; otherwise, <see langword="false" />.</returns>
		// Token: 0x060007FC RID: 2044 RVA: 0x00020867 File Offset: 0x0001EA67
		public bool IsUnrestricted()
		{
			return this.printingLevel == PrintingPermissionLevel.AllPrinting;
		}

		/// <summary>Determines whether the current permission object is a subset of the specified permission.</summary>
		/// <param name="target">A permission object that is to be tested for the subset relationship. This object must be of the same type as the current permission object. </param>
		/// <returns>
		///     <see langword="true" /> if the current permission object is a subset of <paramref name="target" />; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="target" /> is an object that is not of the same type as the current permission object. </exception>
		// Token: 0x060007FD RID: 2045 RVA: 0x00020874 File Offset: 0x0001EA74
		public override bool IsSubsetOf(IPermission target)
		{
			if (target == null)
			{
				return this.printingLevel == PrintingPermissionLevel.NoPrinting;
			}
			PrintingPermission printingPermission = target as PrintingPermission;
			if (printingPermission == null)
			{
				throw new ArgumentException(SR.GetString("TargetNotPrintingPermission"));
			}
			return this.printingLevel <= printingPermission.printingLevel;
		}

		/// <summary>Creates and returns a permission that is the intersection of the current permission object and a target permission object.</summary>
		/// <param name="target">A permission object of the same type as the current permission object. </param>
		/// <returns>A new permission object that represents the intersection of the current object and the specified target. This object is <see langword="null" /> if the intersection is empty.</returns>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="target" /> is an object that is not of the same type as the current permission object. </exception>
		// Token: 0x060007FE RID: 2046 RVA: 0x000208BC File Offset: 0x0001EABC
		public override IPermission Intersect(IPermission target)
		{
			if (target == null)
			{
				return null;
			}
			PrintingPermission printingPermission = target as PrintingPermission;
			if (printingPermission == null)
			{
				throw new ArgumentException(SR.GetString("TargetNotPrintingPermission"));
			}
			PrintingPermissionLevel printingPermissionLevel = (this.printingLevel < printingPermission.printingLevel) ? this.printingLevel : printingPermission.printingLevel;
			if (printingPermissionLevel == PrintingPermissionLevel.NoPrinting)
			{
				return null;
			}
			return new PrintingPermission(printingPermissionLevel);
		}

		/// <summary>Creates a permission that combines the permission object and the target permission object.</summary>
		/// <param name="target">A permission object of the same type as the current permission object. </param>
		/// <returns>A new permission object that represents the union of the current permission object and the specified permission object.</returns>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="target" /> is an object that is not of the same type as the current permission object. </exception>
		// Token: 0x060007FF RID: 2047 RVA: 0x00020910 File Offset: 0x0001EB10
		public override IPermission Union(IPermission target)
		{
			if (target == null)
			{
				return this.Copy();
			}
			PrintingPermission printingPermission = target as PrintingPermission;
			if (printingPermission == null)
			{
				throw new ArgumentException(SR.GetString("TargetNotPrintingPermission"));
			}
			PrintingPermissionLevel printingPermissionLevel = (this.printingLevel > printingPermission.printingLevel) ? this.printingLevel : printingPermission.printingLevel;
			if (printingPermissionLevel == PrintingPermissionLevel.NoPrinting)
			{
				return null;
			}
			return new PrintingPermission(printingPermissionLevel);
		}

		/// <summary>Creates and returns an identical copy of the current permission object.</summary>
		/// <returns>A copy of the current permission object.</returns>
		// Token: 0x06000800 RID: 2048 RVA: 0x00020969 File Offset: 0x0001EB69
		public override IPermission Copy()
		{
			return new PrintingPermission(this.printingLevel);
		}

		/// <summary>Creates an XML encoding of the security object and its current state.</summary>
		/// <returns>An XML encoding of the security object, including any state information.</returns>
		// Token: 0x06000801 RID: 2049 RVA: 0x00020978 File Offset: 0x0001EB78
		public override SecurityElement ToXml()
		{
			SecurityElement securityElement = new SecurityElement("IPermission");
			securityElement.AddAttribute("class", base.GetType().FullName + ", " + base.GetType().Module.Assembly.FullName.Replace('"', '\''));
			securityElement.AddAttribute("version", "1");
			if (!this.IsUnrestricted())
			{
				securityElement.AddAttribute("Level", Enum.GetName(typeof(PrintingPermissionLevel), this.printingLevel));
			}
			else
			{
				securityElement.AddAttribute("Unrestricted", "true");
			}
			return securityElement;
		}

		/// <summary>Reconstructs a security object with a specified state from an XML encoding.</summary>
		/// <param name="esd">The XML encoding to use to reconstruct the security object. </param>
		// Token: 0x06000802 RID: 2050 RVA: 0x00020A20 File Offset: 0x0001EC20
		public override void FromXml(SecurityElement esd)
		{
			if (esd == null)
			{
				throw new ArgumentNullException("esd");
			}
			string text = esd.Attribute("class");
			if (text == null || text.IndexOf(base.GetType().FullName) == -1)
			{
				throw new ArgumentException(SR.GetString("InvalidClassName"));
			}
			string text2 = esd.Attribute("Unrestricted");
			if (text2 != null && string.Equals(text2, "true", StringComparison.OrdinalIgnoreCase))
			{
				this.printingLevel = PrintingPermissionLevel.AllPrinting;
				return;
			}
			this.printingLevel = PrintingPermissionLevel.NoPrinting;
			string text3 = esd.Attribute("Level");
			if (text3 != null)
			{
				this.printingLevel = (PrintingPermissionLevel)Enum.Parse(typeof(PrintingPermissionLevel), text3);
			}
		}

		// Token: 0x040006EA RID: 1770
		private PrintingPermissionLevel printingLevel;
	}
}
