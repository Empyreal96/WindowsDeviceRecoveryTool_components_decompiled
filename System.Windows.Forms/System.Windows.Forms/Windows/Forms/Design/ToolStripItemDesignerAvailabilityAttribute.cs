using System;

namespace System.Windows.Forms.Design
{
	/// <summary>Specifies which types a <see cref="T:System.Windows.Forms.ToolStripItem" /> can appear in. This class cannot be inherited.</summary>
	// Token: 0x0200049D RID: 1181
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class ToolStripItemDesignerAvailabilityAttribute : Attribute
	{
		/// <summary>Initializes a new default instance of the <see cref="T:System.Windows.Forms.Design.ToolStripItemDesignerAvailabilityAttribute" /> class. </summary>
		// Token: 0x06005025 RID: 20517 RVA: 0x0014C6B7 File Offset: 0x0014A8B7
		public ToolStripItemDesignerAvailabilityAttribute()
		{
			this.visibility = ToolStripItemDesignerAvailability.None;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.Design.ToolStripItemDesignerAvailabilityAttribute" /> class with the specified visibility. </summary>
		/// <param name="visibility">A <see cref="T:System.Windows.Forms.Design.ToolStripItemDesignerAvailability" /> value specifying the visibility.</param>
		// Token: 0x06005026 RID: 20518 RVA: 0x0014C6C6 File Offset: 0x0014A8C6
		public ToolStripItemDesignerAvailabilityAttribute(ToolStripItemDesignerAvailability visibility)
		{
			this.visibility = visibility;
		}

		/// <summary>Gets the visibility of a <see cref="T:System.Windows.Forms.ToolStripItem" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Design.ToolStripItemDesignerAvailability" /> representing the visibility.</returns>
		// Token: 0x170013CB RID: 5067
		// (get) Token: 0x06005027 RID: 20519 RVA: 0x0014C6D5 File Offset: 0x0014A8D5
		public ToolStripItemDesignerAvailability ItemAdditionVisibility
		{
			get
			{
				return this.visibility;
			}
		}

		/// <summary>Returns a value that indicates whether this instance is equal to a specified object.</summary>
		/// <param name="obj">An <see cref="T:System.Object" /> to compare with this instance or a null reference (<see langword="Nothing" /> in Visual Basic).</param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="obj" /> equals the type and value of this instance; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005028 RID: 20520 RVA: 0x0014C6E0 File Offset: 0x0014A8E0
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			ToolStripItemDesignerAvailabilityAttribute toolStripItemDesignerAvailabilityAttribute = obj as ToolStripItemDesignerAvailabilityAttribute;
			return toolStripItemDesignerAvailabilityAttribute != null && toolStripItemDesignerAvailabilityAttribute.ItemAdditionVisibility == this.visibility;
		}

		/// <summary>Returns the hash code for this instance.</summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		// Token: 0x06005029 RID: 20521 RVA: 0x0014C70D File Offset: 0x0014A90D
		public override int GetHashCode()
		{
			return this.visibility.GetHashCode();
		}

		/// <summary>When overriden in a derived class, indicates whether the value of this instance is the default value for the derived class.</summary>
		/// <returns>
		///     <see langword="true" /> if this instance is the default attribute for the class; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600502A RID: 20522 RVA: 0x0014C720 File Offset: 0x0014A920
		public override bool IsDefaultAttribute()
		{
			return this.Equals(ToolStripItemDesignerAvailabilityAttribute.Default);
		}

		// Token: 0x04003404 RID: 13316
		private ToolStripItemDesignerAvailability visibility;

		/// <summary>Specifies the default value of the <see cref="T:System.Windows.Forms.Design.ToolStripItemDesignerAvailabilityAttribute" />. This field is read-only.</summary>
		// Token: 0x04003405 RID: 13317
		public static readonly ToolStripItemDesignerAvailabilityAttribute Default = new ToolStripItemDesignerAvailabilityAttribute();
	}
}
