using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies a standard interface for retrieving feature information from the current system.</summary>
	// Token: 0x0200027E RID: 638
	public interface IFeatureSupport
	{
		/// <summary>Determines whether any version of the specified feature is currently available on the system.</summary>
		/// <param name="feature">The feature to look for. </param>
		/// <returns>
		///     <see langword="true" /> if the feature is present; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002652 RID: 9810
		bool IsPresent(object feature);

		/// <summary>Determines whether the specified or newer version of the specified feature is currently available on the system.</summary>
		/// <param name="feature">The feature to look for. </param>
		/// <param name="minimumVersion">A <see cref="T:System.Version" /> representing the minimum version number of the feature to look for. </param>
		/// <returns>
		///     <see langword="true" /> if the requested version of the feature is present; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002653 RID: 9811
		bool IsPresent(object feature, Version minimumVersion);

		/// <summary>Retrieves the version of the specified feature.</summary>
		/// <param name="feature">The feature whose version is requested. </param>
		/// <returns>A <see cref="T:System.Version" /> representing the version number of the specified feature; or <see langword="null" /> if the feature is not installed.</returns>
		// Token: 0x06002654 RID: 9812
		Version GetVersionPresent(object feature);
	}
}
