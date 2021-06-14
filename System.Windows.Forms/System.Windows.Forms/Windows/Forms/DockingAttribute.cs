using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies the default docking behavior for a control.</summary>
	// Token: 0x0200021F RID: 543
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class DockingAttribute : Attribute
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DockingAttribute" /> class. </summary>
		// Token: 0x0600211B RID: 8475 RVA: 0x000A3EDC File Offset: 0x000A20DC
		public DockingAttribute()
		{
			this.dockingBehavior = DockingBehavior.Never;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DockingAttribute" /> class with the given docking behavior. </summary>
		/// <param name="dockingBehavior">A <see cref="T:System.Windows.Forms.DockingBehavior" /> value specifying the default behavior.</param>
		// Token: 0x0600211C RID: 8476 RVA: 0x000A3EEB File Offset: 0x000A20EB
		public DockingAttribute(DockingBehavior dockingBehavior)
		{
			this.dockingBehavior = dockingBehavior;
		}

		/// <summary>Gets the docking behavior supplied to this attribute.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.DockingBehavior" /> value.</returns>
		// Token: 0x170007D8 RID: 2008
		// (get) Token: 0x0600211D RID: 8477 RVA: 0x000A3EFA File Offset: 0x000A20FA
		public DockingBehavior DockingBehavior
		{
			get
			{
				return this.dockingBehavior;
			}
		}

		/// <summary>Compares an arbitrary object with the <see cref="T:System.Windows.Forms.DockingAttribute" /> object for equality.</summary>
		/// <param name="obj">The <see cref="T:System.Object" /> against which to compare this <see cref="T:System.Windows.Forms.DockingAttribute" />.</param>
		/// <returns>
		///     <see langword="true" /> is <paramref name="obj" /> is equal to this <see cref="T:System.Windows.Forms.DockingAttribute" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600211E RID: 8478 RVA: 0x000A3F04 File Offset: 0x000A2104
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DockingAttribute dockingAttribute = obj as DockingAttribute;
			return dockingAttribute != null && dockingAttribute.DockingBehavior == this.dockingBehavior;
		}

		/// <summary>The hash code for this object.</summary>
		/// <returns>An <see cref="T:System.Int32" /> representing an in-memory hash of this object.</returns>
		// Token: 0x0600211F RID: 8479 RVA: 0x000A3F31 File Offset: 0x000A2131
		public override int GetHashCode()
		{
			return this.dockingBehavior.GetHashCode();
		}

		/// <summary>Specifies whether this <see cref="T:System.Windows.Forms.DockingAttribute" /> is the default docking attribute.</summary>
		/// <returns>
		///     <see langword="true" /> is the current <see cref="T:System.Windows.Forms.DockingAttribute" /> is the default; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002120 RID: 8480 RVA: 0x000A3F44 File Offset: 0x000A2144
		public override bool IsDefaultAttribute()
		{
			return this.Equals(DockingAttribute.Default);
		}

		// Token: 0x04000E42 RID: 3650
		private DockingBehavior dockingBehavior;

		/// <summary>The default <see cref="T:System.Windows.Forms.DockingAttribute" /> for this control.</summary>
		// Token: 0x04000E43 RID: 3651
		public static readonly DockingAttribute Default = new DockingAttribute();
	}
}
