using System;

namespace System.Windows.Forms
{
	/// <summary>Provides operating-system specific feature queries.</summary>
	// Token: 0x02000303 RID: 771
	public class OSFeature : FeatureSupport
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.OSFeature" /> class. </summary>
		// Token: 0x06002EB8 RID: 11960 RVA: 0x000D8FC6 File Offset: 0x000D71C6
		protected OSFeature()
		{
		}

		/// <summary>Gets a <see langword="static" /> instance of the <see cref="T:System.Windows.Forms.OSFeature" /> class to use for feature queries. This property is read-only. </summary>
		/// <returns>An instance of the <see cref="T:System.Windows.Forms.OSFeature" /> class.</returns>
		// Token: 0x17000B44 RID: 2884
		// (get) Token: 0x06002EB9 RID: 11961 RVA: 0x000D8FCE File Offset: 0x000D71CE
		public static OSFeature Feature
		{
			get
			{
				if (OSFeature.feature == null)
				{
					OSFeature.feature = new OSFeature();
				}
				return OSFeature.feature;
			}
		}

		/// <summary>Retrieves the version of the specified feature currently available on the system. </summary>
		/// <param name="feature">The feature whose version is requested, either <see cref="F:System.Windows.Forms.OSFeature.LayeredWindows" /> or <see cref="F:System.Windows.Forms.OSFeature.Themes" />.</param>
		/// <returns>A <see cref="T:System.Version" /> representing the version of the specified operating system feature currently available on the system; or <see langword="null" /> if the feature cannot be found.</returns>
		// Token: 0x06002EBA RID: 11962 RVA: 0x000D8FE8 File Offset: 0x000D71E8
		public override Version GetVersionPresent(object feature)
		{
			Version result = null;
			if (feature == OSFeature.LayeredWindows)
			{
				if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.CompareTo(new Version(5, 0, 0, 0)) >= 0)
				{
					result = new Version(0, 0, 0, 0);
				}
			}
			else if (feature == OSFeature.Themes)
			{
				if (!OSFeature.themeSupportTested)
				{
					try
					{
						SafeNativeMethods.IsAppThemed();
						OSFeature.themeSupport = true;
					}
					catch
					{
						OSFeature.themeSupport = false;
					}
					OSFeature.themeSupportTested = true;
				}
				if (OSFeature.themeSupport)
				{
					result = new Version(0, 0, 0, 0);
				}
			}
			return result;
		}

		// Token: 0x17000B45 RID: 2885
		// (get) Token: 0x06002EBB RID: 11963 RVA: 0x000D9084 File Offset: 0x000D7284
		internal bool OnXp
		{
			get
			{
				bool result = false;
				if (Environment.OSVersion.Platform == PlatformID.Win32NT)
				{
					result = (Environment.OSVersion.Version.CompareTo(new Version(5, 1, 0, 0)) >= 0);
				}
				return result;
			}
		}

		// Token: 0x17000B46 RID: 2886
		// (get) Token: 0x06002EBC RID: 11964 RVA: 0x000D90C0 File Offset: 0x000D72C0
		internal bool OnWin2k
		{
			get
			{
				bool result = false;
				if (Environment.OSVersion.Platform == PlatformID.Win32NT)
				{
					result = (Environment.OSVersion.Version.CompareTo(new Version(5, 0, 0, 0)) >= 0);
				}
				return result;
			}
		}

		/// <summary>Retrieves a value indicating whether the operating system supports the specified feature or metric. </summary>
		/// <param name="enumVal">A <see cref="T:System.Windows.Forms.SystemParameter" /> representing the feature to search for.</param>
		/// <returns>
		///     <see langword="true" /> if the feature is available on the system; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002EBD RID: 11965 RVA: 0x000D90FC File Offset: 0x000D72FC
		public static bool IsPresent(SystemParameter enumVal)
		{
			switch (enumVal)
			{
			case SystemParameter.DropShadow:
				return OSFeature.Feature.OnXp;
			case SystemParameter.FlatMenu:
				return OSFeature.Feature.OnXp;
			case SystemParameter.FontSmoothingContrastMetric:
				return OSFeature.Feature.OnXp;
			case SystemParameter.FontSmoothingTypeMetric:
				return OSFeature.Feature.OnXp;
			case SystemParameter.MenuFadeEnabled:
				return OSFeature.Feature.OnWin2k;
			case SystemParameter.SelectionFade:
				return OSFeature.Feature.OnWin2k;
			case SystemParameter.ToolTipAnimationMetric:
				return OSFeature.Feature.OnWin2k;
			case SystemParameter.UIEffects:
				return OSFeature.Feature.OnWin2k;
			case SystemParameter.CaretWidthMetric:
				return OSFeature.Feature.OnWin2k;
			case SystemParameter.VerticalFocusThicknessMetric:
				return OSFeature.Feature.OnXp;
			case SystemParameter.HorizontalFocusThicknessMetric:
				return OSFeature.Feature.OnXp;
			default:
				return false;
			}
		}

		/// <summary>Represents the layered, top-level windows feature. This field is read-only. </summary>
		// Token: 0x04001D40 RID: 7488
		public static readonly object LayeredWindows = new object();

		/// <summary>Represents the operating system themes feature. This field is read-only.</summary>
		// Token: 0x04001D41 RID: 7489
		public static readonly object Themes = new object();

		// Token: 0x04001D42 RID: 7490
		private static OSFeature feature = null;

		// Token: 0x04001D43 RID: 7491
		private static bool themeSupportTested = false;

		// Token: 0x04001D44 RID: 7492
		private static bool themeSupport = false;
	}
}
