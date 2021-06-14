using System;

namespace System.Windows.Diagnostics
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Diagnostics.ResourceDictionaryDiagnostics.ThemedResourceDictionaryUnloaded" /> event.</summary>
	// Token: 0x02000195 RID: 405
	public class ResourceDictionaryUnloadedEventArgs : EventArgs
	{
		// Token: 0x060017BC RID: 6076 RVA: 0x00073DD7 File Offset: 0x00071FD7
		internal ResourceDictionaryUnloadedEventArgs(ResourceDictionaryInfo resourceDictionaryInfo)
		{
			this.ResourceDictionaryInfo = resourceDictionaryInfo;
		}

		/// <summary>Gets data for the <see cref="E:System.Windows.Diagnostics.ResourceDictionaryDiagnostics.ThemedResourceDictionaryUnloaded" /> event.</summary>
		/// <returns>Data for the <see cref="E:System.Windows.Diagnostics.ResourceDictionaryDiagnostics.ThemedResourceDictionaryUnloaded" /> event.</returns>
		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x060017BD RID: 6077 RVA: 0x00073DE6 File Offset: 0x00071FE6
		// (set) Token: 0x060017BE RID: 6078 RVA: 0x00073DEE File Offset: 0x00071FEE
		public ResourceDictionaryInfo ResourceDictionaryInfo { get; private set; }
	}
}
