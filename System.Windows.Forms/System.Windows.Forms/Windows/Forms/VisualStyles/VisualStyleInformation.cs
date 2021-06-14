using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Windows.Forms.VisualStyles
{
	/// <summary>Provides information about the current visual style of the operating system.</summary>
	// Token: 0x02000447 RID: 1095
	public static class VisualStyleInformation
	{
		/// <summary>Gets a value indicating whether the operating system supports visual styles.</summary>
		/// <returns>
		///     <see langword="true" /> if the operating system supports visual styles; otherwise, <see langword="false" />.</returns>
		// Token: 0x170012DA RID: 4826
		// (get) Token: 0x06004CB6 RID: 19638 RVA: 0x0013AB91 File Offset: 0x00138D91
		public static bool IsSupportedByOS
		{
			get
			{
				return OSFeature.Feature.IsPresent(OSFeature.Themes);
			}
		}

		/// <summary>Gets a value indicating whether the user has enabled visual styles in the operating system.</summary>
		/// <returns>
		///     <see langword="true" /> if the user has enabled visual styles in an operating system that supports them; otherwise, <see langword="false" />.</returns>
		// Token: 0x170012DB RID: 4827
		// (get) Token: 0x06004CB7 RID: 19639 RVA: 0x0013ABA2 File Offset: 0x00138DA2
		public static bool IsEnabledByUser
		{
			get
			{
				return VisualStyleInformation.IsSupportedByOS && SafeNativeMethods.IsAppThemed();
			}
		}

		// Token: 0x170012DC RID: 4828
		// (get) Token: 0x06004CB8 RID: 19640 RVA: 0x0013ABB4 File Offset: 0x00138DB4
		internal static string ThemeFilename
		{
			get
			{
				if (VisualStyleInformation.IsEnabledByUser)
				{
					StringBuilder stringBuilder = new StringBuilder(512);
					SafeNativeMethods.GetCurrentThemeName(stringBuilder, stringBuilder.Capacity, null, 0, null, 0);
					return stringBuilder.ToString();
				}
				return string.Empty;
			}
		}

		/// <summary>Gets the color scheme of the current visual style.</summary>
		/// <returns>A string that specifies the color scheme of the current visual style if visual styles are enabled; otherwise, an empty string ("").</returns>
		// Token: 0x170012DD RID: 4829
		// (get) Token: 0x06004CB9 RID: 19641 RVA: 0x0013ABF0 File Offset: 0x00138DF0
		public static string ColorScheme
		{
			get
			{
				if (VisualStyleInformation.IsEnabledByUser)
				{
					StringBuilder stringBuilder = new StringBuilder(512);
					SafeNativeMethods.GetCurrentThemeName(null, 0, stringBuilder, stringBuilder.Capacity, null, 0);
					return stringBuilder.ToString();
				}
				return string.Empty;
			}
		}

		/// <summary>Gets a string that describes the size of the current visual style.</summary>
		/// <returns>A string that describes the size of the current visual style if visual styles are enabled; otherwise, an empty string ("").</returns>
		// Token: 0x170012DE RID: 4830
		// (get) Token: 0x06004CBA RID: 19642 RVA: 0x0013AC2C File Offset: 0x00138E2C
		public static string Size
		{
			get
			{
				if (VisualStyleInformation.IsEnabledByUser)
				{
					StringBuilder stringBuilder = new StringBuilder(512);
					SafeNativeMethods.GetCurrentThemeName(null, 0, null, 0, stringBuilder, stringBuilder.Capacity);
					return stringBuilder.ToString();
				}
				return string.Empty;
			}
		}

		/// <summary>Gets the display name of the current visual style.</summary>
		/// <returns>A string that specifies the display name of the current visual style if visual styles are enabled; otherwise, an empty string ("").</returns>
		// Token: 0x170012DF RID: 4831
		// (get) Token: 0x06004CBB RID: 19643 RVA: 0x0013AC68 File Offset: 0x00138E68
		public static string DisplayName
		{
			get
			{
				if (VisualStyleInformation.IsEnabledByUser)
				{
					StringBuilder stringBuilder = new StringBuilder(512);
					SafeNativeMethods.GetThemeDocumentationProperty(VisualStyleInformation.ThemeFilename, VisualStyleDocProperty.DisplayName, stringBuilder, stringBuilder.Capacity);
					return stringBuilder.ToString();
				}
				return string.Empty;
			}
		}

		/// <summary>Gets the company that created the current visual style.</summary>
		/// <returns>A string that specifies the company that created the current visual style if visual styles are enabled; otherwise, an empty string ("").</returns>
		// Token: 0x170012E0 RID: 4832
		// (get) Token: 0x06004CBC RID: 19644 RVA: 0x0013ACAC File Offset: 0x00138EAC
		public static string Company
		{
			get
			{
				if (VisualStyleInformation.IsEnabledByUser)
				{
					StringBuilder stringBuilder = new StringBuilder(512);
					SafeNativeMethods.GetThemeDocumentationProperty(VisualStyleInformation.ThemeFilename, VisualStyleDocProperty.Company, stringBuilder, stringBuilder.Capacity);
					return stringBuilder.ToString();
				}
				return string.Empty;
			}
		}

		/// <summary>Gets the author of the current visual style.</summary>
		/// <returns>A string that specifies the author of the current visual style if visual styles are enabled; otherwise, an empty string ("").</returns>
		// Token: 0x170012E1 RID: 4833
		// (get) Token: 0x06004CBD RID: 19645 RVA: 0x0013ACF0 File Offset: 0x00138EF0
		public static string Author
		{
			get
			{
				if (VisualStyleInformation.IsEnabledByUser)
				{
					StringBuilder stringBuilder = new StringBuilder(512);
					SafeNativeMethods.GetThemeDocumentationProperty(VisualStyleInformation.ThemeFilename, VisualStyleDocProperty.Author, stringBuilder, stringBuilder.Capacity);
					return stringBuilder.ToString();
				}
				return string.Empty;
			}
		}

		/// <summary>Gets the copyright of the current visual style.</summary>
		/// <returns>A string that specifies the copyright of the current visual style if visual styles are enabled; otherwise, an empty string ("").</returns>
		// Token: 0x170012E2 RID: 4834
		// (get) Token: 0x06004CBE RID: 19646 RVA: 0x0013AD34 File Offset: 0x00138F34
		public static string Copyright
		{
			get
			{
				if (VisualStyleInformation.IsEnabledByUser)
				{
					StringBuilder stringBuilder = new StringBuilder(512);
					SafeNativeMethods.GetThemeDocumentationProperty(VisualStyleInformation.ThemeFilename, VisualStyleDocProperty.Copyright, stringBuilder, stringBuilder.Capacity);
					return stringBuilder.ToString();
				}
				return string.Empty;
			}
		}

		/// <summary>Gets a URL provided by the author of the current visual style.</summary>
		/// <returns>A string that specifies a URL provided by the author of the current visual style if visual styles are enabled; otherwise, an empty string ("").</returns>
		// Token: 0x170012E3 RID: 4835
		// (get) Token: 0x06004CBF RID: 19647 RVA: 0x0013AD78 File Offset: 0x00138F78
		public static string Url
		{
			get
			{
				if (VisualStyleInformation.IsEnabledByUser)
				{
					StringBuilder stringBuilder = new StringBuilder(512);
					SafeNativeMethods.GetThemeDocumentationProperty(VisualStyleInformation.ThemeFilename, VisualStyleDocProperty.Url, stringBuilder, stringBuilder.Capacity);
					return stringBuilder.ToString();
				}
				return string.Empty;
			}
		}

		/// <summary>Gets the version of the current visual style.</summary>
		/// <returns>A string that indicates the version of the current visual style if visual styles are enabled; otherwise, an empty string ("").</returns>
		// Token: 0x170012E4 RID: 4836
		// (get) Token: 0x06004CC0 RID: 19648 RVA: 0x0013ADBC File Offset: 0x00138FBC
		public static string Version
		{
			get
			{
				if (VisualStyleInformation.IsEnabledByUser)
				{
					StringBuilder stringBuilder = new StringBuilder(512);
					SafeNativeMethods.GetThemeDocumentationProperty(VisualStyleInformation.ThemeFilename, VisualStyleDocProperty.Version, stringBuilder, stringBuilder.Capacity);
					return stringBuilder.ToString();
				}
				return string.Empty;
			}
		}

		/// <summary>Gets a description of the current visual style.</summary>
		/// <returns>A string that describes the current visual style if visual styles are enabled; otherwise, an empty string ("").</returns>
		// Token: 0x170012E5 RID: 4837
		// (get) Token: 0x06004CC1 RID: 19649 RVA: 0x0013AE00 File Offset: 0x00139000
		public static string Description
		{
			get
			{
				if (VisualStyleInformation.IsEnabledByUser)
				{
					StringBuilder stringBuilder = new StringBuilder(512);
					SafeNativeMethods.GetThemeDocumentationProperty(VisualStyleInformation.ThemeFilename, VisualStyleDocProperty.Description, stringBuilder, stringBuilder.Capacity);
					return stringBuilder.ToString();
				}
				return string.Empty;
			}
		}

		/// <summary>Gets a value indicating whether the current visual style supports flat menus.</summary>
		/// <returns>
		///     <see langword="true" /> if visual styles are enabled and the current visual style supports flat menus; otherwise, <see langword="false" />.</returns>
		// Token: 0x170012E6 RID: 4838
		// (get) Token: 0x06004CC2 RID: 19650 RVA: 0x0013AE44 File Offset: 0x00139044
		public static bool SupportsFlatMenus
		{
			get
			{
				if (Application.RenderWithVisualStyles)
				{
					if (VisualStyleInformation.visualStyleRenderer == null)
					{
						VisualStyleInformation.visualStyleRenderer = new VisualStyleRenderer(VisualStyleElement.Window.Caption.Active);
					}
					else
					{
						VisualStyleInformation.visualStyleRenderer.SetParameters(VisualStyleElement.Window.Caption.Active);
					}
					return SafeNativeMethods.GetThemeSysBool(new HandleRef(null, VisualStyleInformation.visualStyleRenderer.Handle), VisualStyleSystemProperty.SupportsFlatMenus);
				}
				return false;
			}
		}

		/// <summary>Gets the minimum color depth for the current visual style.</summary>
		/// <returns>The minimum color depth for the current visual style if visual styles are enabled; otherwise, 0.</returns>
		// Token: 0x170012E7 RID: 4839
		// (get) Token: 0x06004CC3 RID: 19651 RVA: 0x0013AE9C File Offset: 0x0013909C
		public static int MinimumColorDepth
		{
			get
			{
				if (Application.RenderWithVisualStyles)
				{
					if (VisualStyleInformation.visualStyleRenderer == null)
					{
						VisualStyleInformation.visualStyleRenderer = new VisualStyleRenderer(VisualStyleElement.Window.Caption.Active);
					}
					else
					{
						VisualStyleInformation.visualStyleRenderer.SetParameters(VisualStyleElement.Window.Caption.Active);
					}
					int result = 0;
					SafeNativeMethods.GetThemeSysInt(new HandleRef(null, VisualStyleInformation.visualStyleRenderer.Handle), VisualStyleSystemProperty.MinimumColorDepth, ref result);
					return result;
				}
				return 0;
			}
		}

		/// <summary>Gets the color that the current visual style uses to paint the borders of controls that contain text.</summary>
		/// <returns>If visual styles are enabled, the <see cref="T:System.Drawing.Color" /> that the current visual style uses to paint the borders of controls that contain text; otherwise, <see cref="P:System.Drawing.SystemColors.ControlDarkDark" />.</returns>
		// Token: 0x170012E8 RID: 4840
		// (get) Token: 0x06004CC4 RID: 19652 RVA: 0x0013AEFC File Offset: 0x001390FC
		public static Color TextControlBorder
		{
			get
			{
				if (Application.RenderWithVisualStyles)
				{
					if (VisualStyleInformation.visualStyleRenderer == null)
					{
						VisualStyleInformation.visualStyleRenderer = new VisualStyleRenderer(VisualStyleElement.TextBox.TextEdit.Normal);
					}
					else
					{
						VisualStyleInformation.visualStyleRenderer.SetParameters(VisualStyleElement.TextBox.TextEdit.Normal);
					}
					return VisualStyleInformation.visualStyleRenderer.GetColor(ColorProperty.BorderColor);
				}
				return SystemColors.WindowFrame;
			}
		}

		/// <summary>Gets the color that the current visual style uses to indicate the hot state of a control.</summary>
		/// <returns>If visual styles are enabled, the <see cref="T:System.Drawing.Color" /> used to paint a highlight on a control in the hot state; otherwise, <see cref="P:System.Drawing.SystemColors.ButtonHighlight" />.</returns>
		// Token: 0x170012E9 RID: 4841
		// (get) Token: 0x06004CC5 RID: 19653 RVA: 0x0013AF50 File Offset: 0x00139150
		public static Color ControlHighlightHot
		{
			get
			{
				if (Application.RenderWithVisualStyles)
				{
					if (VisualStyleInformation.visualStyleRenderer == null)
					{
						VisualStyleInformation.visualStyleRenderer = new VisualStyleRenderer(VisualStyleElement.Button.PushButton.Normal);
					}
					else
					{
						VisualStyleInformation.visualStyleRenderer.SetParameters(VisualStyleElement.Button.PushButton.Normal);
					}
					return VisualStyleInformation.visualStyleRenderer.GetColor(ColorProperty.AccentColorHint);
				}
				return SystemColors.ButtonHighlight;
			}
		}

		// Token: 0x04003166 RID: 12646
		[ThreadStatic]
		private static VisualStyleRenderer visualStyleRenderer;
	}
}
