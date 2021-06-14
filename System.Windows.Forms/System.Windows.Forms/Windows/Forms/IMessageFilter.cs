using System;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	/// <summary>Defines a message filter interface.</summary>
	// Token: 0x02000288 RID: 648
	public interface IMessageFilter
	{
		/// <summary>Filters out a message before it is dispatched.</summary>
		/// <param name="m">The message to be dispatched. You cannot modify this message. </param>
		/// <returns>
		///     <see langword="true" /> to filter the message and stop it from being dispatched; <see langword="false" /> to allow the message to continue to the next filter or control.</returns>
		// Token: 0x060026AD RID: 9901
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		bool PreFilterMessage(ref Message m);
	}
}
