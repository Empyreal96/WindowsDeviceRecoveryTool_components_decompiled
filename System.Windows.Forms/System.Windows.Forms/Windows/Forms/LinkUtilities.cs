using System;
using System.Drawing;
using System.Globalization;
using System.Security;
using System.Security.Permissions;
using Microsoft.Win32;

namespace System.Windows.Forms
{
	// Token: 0x020002B9 RID: 697
	internal class LinkUtilities
	{
		// Token: 0x0600285C RID: 10332 RVA: 0x000BC504 File Offset: 0x000BA704
		private static Color GetIEColor(string name)
		{
			new RegistryPermission(PermissionState.Unrestricted).Assert();
			Color result;
			try
			{
				RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Internet Explorer\\Settings");
				if (registryKey != null)
				{
					string text = (string)registryKey.GetValue(name);
					registryKey.Close();
					if (text != null)
					{
						string[] array = text.Split(new char[]
						{
							','
						});
						int[] array2 = new int[3];
						int num = Math.Min(array2.Length, array.Length);
						for (int i = 0; i < num; i++)
						{
							int.TryParse(array[i], out array2[i]);
						}
						return Color.FromArgb(array2[0], array2[1], array2[2]);
					}
				}
				if (string.Equals(name, "Anchor Color", StringComparison.OrdinalIgnoreCase))
				{
					result = Color.Blue;
				}
				else if (string.Equals(name, "Anchor Color Visited", StringComparison.OrdinalIgnoreCase))
				{
					result = Color.Purple;
				}
				else
				{
					string.Equals(name, "Anchor Color Hover", StringComparison.OrdinalIgnoreCase);
					result = Color.Red;
				}
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return result;
		}

		// Token: 0x170009CA RID: 2506
		// (get) Token: 0x0600285D RID: 10333 RVA: 0x000BC5FC File Offset: 0x000BA7FC
		public static Color IELinkColor
		{
			get
			{
				if (LinkUtilities.ielinkColor.IsEmpty)
				{
					LinkUtilities.ielinkColor = LinkUtilities.GetIEColor("Anchor Color");
				}
				return LinkUtilities.ielinkColor;
			}
		}

		// Token: 0x170009CB RID: 2507
		// (get) Token: 0x0600285E RID: 10334 RVA: 0x000BC61E File Offset: 0x000BA81E
		public static Color IEActiveLinkColor
		{
			get
			{
				if (LinkUtilities.ieactiveLinkColor.IsEmpty)
				{
					LinkUtilities.ieactiveLinkColor = LinkUtilities.GetIEColor("Anchor Color Hover");
				}
				return LinkUtilities.ieactiveLinkColor;
			}
		}

		// Token: 0x170009CC RID: 2508
		// (get) Token: 0x0600285F RID: 10335 RVA: 0x000BC640 File Offset: 0x000BA840
		public static Color IEVisitedLinkColor
		{
			get
			{
				if (LinkUtilities.ievisitedLinkColor.IsEmpty)
				{
					LinkUtilities.ievisitedLinkColor = LinkUtilities.GetIEColor("Anchor Color Visited");
				}
				return LinkUtilities.ievisitedLinkColor;
			}
		}

		// Token: 0x06002860 RID: 10336 RVA: 0x000BC664 File Offset: 0x000BA864
		public static Color GetVisitedLinkColor()
		{
			int red = (int)((SystemColors.Window.R + SystemColors.WindowText.R + 1) / 2);
			int g = (int)SystemColors.WindowText.G;
			int blue = (int)((SystemColors.Window.B + SystemColors.WindowText.B + 1) / 2);
			return Color.FromArgb(red, g, blue);
		}

		// Token: 0x06002861 RID: 10337 RVA: 0x000BC6C8 File Offset: 0x000BA8C8
		public static LinkBehavior GetIELinkBehavior()
		{
			new RegistryPermission(PermissionState.Unrestricted).Assert();
			try
			{
				RegistryKey registryKey = null;
				try
				{
					registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Internet Explorer\\Main");
				}
				catch (SecurityException)
				{
				}
				if (registryKey != null)
				{
					string text = (string)registryKey.GetValue("Anchor Underline");
					registryKey.Close();
					if (text != null && string.Compare(text, "no", true, CultureInfo.InvariantCulture) == 0)
					{
						return LinkBehavior.NeverUnderline;
					}
					if (text != null && string.Compare(text, "hover", true, CultureInfo.InvariantCulture) == 0)
					{
						return LinkBehavior.HoverUnderline;
					}
					return LinkBehavior.AlwaysUnderline;
				}
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return LinkBehavior.AlwaysUnderline;
		}

		// Token: 0x06002862 RID: 10338 RVA: 0x000BC770 File Offset: 0x000BA970
		public static void EnsureLinkFonts(Font baseFont, LinkBehavior link, ref Font linkFont, ref Font hoverLinkFont)
		{
			if (linkFont != null && hoverLinkFont != null)
			{
				return;
			}
			bool flag = true;
			bool flag2 = true;
			if (link == LinkBehavior.SystemDefault)
			{
				link = LinkUtilities.GetIELinkBehavior();
			}
			switch (link)
			{
			case LinkBehavior.AlwaysUnderline:
				flag = true;
				flag2 = true;
				break;
			case LinkBehavior.HoverUnderline:
				flag = false;
				flag2 = true;
				break;
			case LinkBehavior.NeverUnderline:
				flag = false;
				flag2 = false;
				break;
			}
			if (flag2 == flag)
			{
				FontStyle fontStyle = baseFont.Style;
				if (flag2)
				{
					fontStyle |= FontStyle.Underline;
				}
				else
				{
					fontStyle &= ~FontStyle.Underline;
				}
				hoverLinkFont = new Font(baseFont, fontStyle);
				linkFont = hoverLinkFont;
				return;
			}
			FontStyle fontStyle2 = baseFont.Style;
			if (flag2)
			{
				fontStyle2 |= FontStyle.Underline;
			}
			else
			{
				fontStyle2 &= ~FontStyle.Underline;
			}
			hoverLinkFont = new Font(baseFont, fontStyle2);
			FontStyle fontStyle3 = baseFont.Style;
			if (flag)
			{
				fontStyle3 |= FontStyle.Underline;
			}
			else
			{
				fontStyle3 &= ~FontStyle.Underline;
			}
			linkFont = new Font(baseFont, fontStyle3);
		}

		// Token: 0x04001195 RID: 4501
		private static Color ielinkColor = Color.Empty;

		// Token: 0x04001196 RID: 4502
		private static Color ieactiveLinkColor = Color.Empty;

		// Token: 0x04001197 RID: 4503
		private static Color ievisitedLinkColor = Color.Empty;

		// Token: 0x04001198 RID: 4504
		private const string IESettingsRegPath = "Software\\Microsoft\\Internet Explorer\\Settings";

		// Token: 0x04001199 RID: 4505
		public const string IEMainRegPath = "Software\\Microsoft\\Internet Explorer\\Main";

		// Token: 0x0400119A RID: 4506
		private const string IEAnchorColor = "Anchor Color";

		// Token: 0x0400119B RID: 4507
		private const string IEAnchorColorVisited = "Anchor Color Visited";

		// Token: 0x0400119C RID: 4508
		private const string IEAnchorColorHover = "Anchor Color Hover";
	}
}
