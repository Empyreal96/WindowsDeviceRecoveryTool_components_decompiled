using System;

namespace System.Windows.Input
{
	/// <summary>Specifies the possible values for changes in focus when logical and directional navigation occurs.</summary>
	// Token: 0x0200017C RID: 380
	public enum KeyboardNavigationMode
	{
		/// <summary>Each element receives keyboard focus, as long as it is a navigation stop.  Navigation leaves the containing element when an edge is reached.</summary>
		// Token: 0x04001282 RID: 4738
		Continue,
		/// <summary>The container and all of its child elements as a whole receive focus only once. Either the first tree child or the or the last focused element in the group receives focus</summary>
		// Token: 0x04001283 RID: 4739
		Once,
		/// <summary>Depending on the direction of the navigation, the focus returns to the first or the last item when the end or the beginning of the container is reached.  Focus cannot leave the container using logical navigation.</summary>
		// Token: 0x04001284 RID: 4740
		Cycle,
		/// <summary>No keyboard navigation is allowed inside this container.</summary>
		// Token: 0x04001285 RID: 4741
		None,
		/// <summary>Depending on the direction of the navigation, focus returns to the first or the last item when the end or the beginning of the container is reached, but does not move past the beginning or end of the container.</summary>
		// Token: 0x04001286 RID: 4742
		Contained,
		/// <summary>Tab Indexes are considered on local subtree only inside this container and behave like <see cref="F:System.Windows.Input.KeyboardNavigationMode.Continue" /> after that.</summary>
		// Token: 0x04001287 RID: 4743
		Local
	}
}
