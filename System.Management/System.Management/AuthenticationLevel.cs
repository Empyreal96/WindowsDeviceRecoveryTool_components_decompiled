using System;

namespace System.Management
{
	/// <summary>Describes the authentication level to be used to connect to WMI. This is used for the COM connection to WMI.          </summary>
	// Token: 0x02000028 RID: 40
	public enum AuthenticationLevel
	{
		/// <summary>The default COM authentication level. WMI uses the default Windows Authentication setting.</summary>
		// Token: 0x04000122 RID: 290
		Default,
		/// <summary>No COM authentication.</summary>
		// Token: 0x04000123 RID: 291
		None,
		/// <summary>Connect-level COM authentication.</summary>
		// Token: 0x04000124 RID: 292
		Connect,
		/// <summary>Call-level COM authentication.</summary>
		// Token: 0x04000125 RID: 293
		Call,
		/// <summary>Packet-level COM authentication.</summary>
		// Token: 0x04000126 RID: 294
		Packet,
		/// <summary>Packet Integrity-level COM authentication.</summary>
		// Token: 0x04000127 RID: 295
		PacketIntegrity,
		/// <summary>Packet Privacy-level COM authentication.</summary>
		// Token: 0x04000128 RID: 296
		PacketPrivacy,
		/// <summary>Authentication level should remain as it was before.</summary>
		// Token: 0x04000129 RID: 297
		Unchanged = -1
	}
}
