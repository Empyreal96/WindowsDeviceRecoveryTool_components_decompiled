using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies whether a column type is visible in the <see cref="T:System.Windows.Forms.DataGridView" /> designer. This class cannot be inherited. </summary>
	// Token: 0x020001B1 RID: 433
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class DataGridViewColumnDesignTimeVisibleAttribute : Attribute
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewColumnDesignTimeVisibleAttribute" /> class using the specified value to initialize the <see cref="P:System.Windows.Forms.DataGridViewColumnDesignTimeVisibleAttribute.Visible" /> property. </summary>
		/// <param name="visible">The value of the <see cref="P:System.Windows.Forms.DataGridViewColumnDesignTimeVisibleAttribute.Visible" /> property.</param>
		// Token: 0x06001C52 RID: 7250 RVA: 0x0008D704 File Offset: 0x0008B904
		public DataGridViewColumnDesignTimeVisibleAttribute(bool visible)
		{
			this.visible = visible;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewColumnDesignTimeVisibleAttribute" /> class using the default <see cref="P:System.Windows.Forms.DataGridViewColumnDesignTimeVisibleAttribute.Visible" /> property value of <see langword="true" />. </summary>
		// Token: 0x06001C53 RID: 7251 RVA: 0x0008D713 File Offset: 0x0008B913
		public DataGridViewColumnDesignTimeVisibleAttribute()
		{
		}

		/// <summary>Gets a value indicating whether the column type is visible in the <see cref="T:System.Windows.Forms.DataGridView" /> designer.</summary>
		/// <returns>
		///     <see langword="true" /> to indicate that the column type is visible in the <see cref="T:System.Windows.Forms.DataGridView" /> designer; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x06001C54 RID: 7252 RVA: 0x0008D71B File Offset: 0x0008B91B
		public bool Visible
		{
			get
			{
				return this.visible;
			}
		}

		/// <summary>Gets a value indicating whether this object is equivalent to the specified object.</summary>
		/// <param name="obj">The <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Object" />.</param>
		/// <returns>
		///     <see langword="true" /> to indicate that the specified object is a <see cref="T:System.Windows.Forms.DataGridViewColumnDesignTimeVisibleAttribute" /> instance with the same <see cref="P:System.Windows.Forms.DataGridViewColumnDesignTimeVisibleAttribute.Visible" /> property value as this instance; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001C55 RID: 7253 RVA: 0x0008D724 File Offset: 0x0008B924
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DataGridViewColumnDesignTimeVisibleAttribute dataGridViewColumnDesignTimeVisibleAttribute = obj as DataGridViewColumnDesignTimeVisibleAttribute;
			return dataGridViewColumnDesignTimeVisibleAttribute != null && dataGridViewColumnDesignTimeVisibleAttribute.Visible == this.visible;
		}

		/// <summary>Returns the hash code for this instance.</summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		// Token: 0x06001C56 RID: 7254 RVA: 0x0008D751 File Offset: 0x0008B951
		public override int GetHashCode()
		{
			return typeof(DataGridViewColumnDesignTimeVisibleAttribute).GetHashCode() ^ (this.visible ? -1 : 0);
		}

		/// <summary>Gets a value indicating whether this attribute instance is equal to the <see cref="F:System.Windows.Forms.DataGridViewColumnDesignTimeVisibleAttribute.Default" /> attribute value.</summary>
		/// <returns>
		///     <see langword="true" /> to indicate that this instance is equal to the <see cref="F:System.Windows.Forms.DataGridViewColumnDesignTimeVisibleAttribute.Default" /> instance; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001C57 RID: 7255 RVA: 0x0008D76F File Offset: 0x0008B96F
		public override bool IsDefaultAttribute()
		{
			return this.Visible == DataGridViewColumnDesignTimeVisibleAttribute.Default.Visible;
		}

		// Token: 0x04000C92 RID: 3218
		private bool visible;

		/// <summary>A <see cref="T:System.Windows.Forms.DataGridViewColumnDesignTimeVisibleAttribute" /> value indicating that the column is visible in the <see cref="T:System.Windows.Forms.DataGridView" /> designer. </summary>
		// Token: 0x04000C93 RID: 3219
		public static readonly DataGridViewColumnDesignTimeVisibleAttribute Yes = new DataGridViewColumnDesignTimeVisibleAttribute(true);

		/// <summary>A <see cref="T:System.Windows.Forms.DataGridViewColumnDesignTimeVisibleAttribute" /> value indicating that the column is not visible in the <see cref="T:System.Windows.Forms.DataGridView" /> designer. </summary>
		// Token: 0x04000C94 RID: 3220
		public static readonly DataGridViewColumnDesignTimeVisibleAttribute No = new DataGridViewColumnDesignTimeVisibleAttribute(false);

		/// <summary>The default <see cref="T:System.Windows.Forms.DataGridViewColumnDesignTimeVisibleAttribute" /> value, which is <see cref="F:System.Windows.Forms.DataGridViewColumnDesignTimeVisibleAttribute.Yes" />, indicating that the column is visible in the <see cref="T:System.Windows.Forms.DataGridView" /> designer. </summary>
		// Token: 0x04000C95 RID: 3221
		public static readonly DataGridViewColumnDesignTimeVisibleAttribute Default = DataGridViewColumnDesignTimeVisibleAttribute.Yes;
	}
}
