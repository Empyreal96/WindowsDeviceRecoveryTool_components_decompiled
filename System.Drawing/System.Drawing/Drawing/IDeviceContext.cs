using System;
using System.Security.Permissions;

namespace System.Drawing
{
	/// <summary>Defines methods for obtaining and releasing an existing handle to a Windows device context.</summary>
	// Token: 0x0200004D RID: 77
	public interface IDeviceContext : IDisposable
	{
		/// <summary>Returns the handle to a Windows device context.</summary>
		/// <returns>An <see cref="T:System.IntPtr" /> representing the handle of a device context.</returns>
		// Token: 0x060006F0 RID: 1776
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		IntPtr GetHdc();

		/// <summary>Releases the handle of a Windows device context.</summary>
		// Token: 0x060006F1 RID: 1777
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		void ReleaseHdc();
	}
}
