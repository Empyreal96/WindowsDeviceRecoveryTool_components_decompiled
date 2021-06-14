using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.Control.Layout" /> event. This class cannot be inherited.</summary>
	// Token: 0x020002AD RID: 685
	public sealed class LayoutEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.LayoutEventArgs" /> class with the specified component and property affected.</summary>
		/// <param name="affectedComponent">The <see cref="T:System.ComponentModel.Component" /> affected by the layout change. </param>
		/// <param name="affectedProperty">The property affected by the layout change. </param>
		// Token: 0x060027D0 RID: 10192 RVA: 0x000B9E0C File Offset: 0x000B800C
		public LayoutEventArgs(IComponent affectedComponent, string affectedProperty)
		{
			this.affectedComponent = affectedComponent;
			this.affectedProperty = affectedProperty;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.LayoutEventArgs" /> class with the specified control and property affected.</summary>
		/// <param name="affectedControl">The <see cref="T:System.Windows.Forms.Control" /> affected by the layout change.</param>
		/// <param name="affectedProperty">The property affected by the layout change.</param>
		// Token: 0x060027D1 RID: 10193 RVA: 0x000B9E22 File Offset: 0x000B8022
		public LayoutEventArgs(Control affectedControl, string affectedProperty) : this(affectedControl, affectedProperty)
		{
		}

		/// <summary>Gets the <see cref="T:System.ComponentModel.Component" /> affected by the layout change.</summary>
		/// <returns>An <see cref="T:System.ComponentModel.IComponent" /> representing the <see cref="T:System.ComponentModel.Component" /> affected by the layout change.</returns>
		// Token: 0x170009A9 RID: 2473
		// (get) Token: 0x060027D2 RID: 10194 RVA: 0x000B9E2C File Offset: 0x000B802C
		public IComponent AffectedComponent
		{
			get
			{
				return this.affectedComponent;
			}
		}

		/// <summary>Gets the child control affected by the change.</summary>
		/// <returns>The child <see cref="T:System.Windows.Forms.Control" /> affected by the change.</returns>
		// Token: 0x170009AA RID: 2474
		// (get) Token: 0x060027D3 RID: 10195 RVA: 0x000B9E34 File Offset: 0x000B8034
		public Control AffectedControl
		{
			get
			{
				return this.affectedComponent as Control;
			}
		}

		/// <summary>Gets the property affected by the change.</summary>
		/// <returns>The property affected by the change.</returns>
		// Token: 0x170009AB RID: 2475
		// (get) Token: 0x060027D4 RID: 10196 RVA: 0x000B9E41 File Offset: 0x000B8041
		public string AffectedProperty
		{
			get
			{
				return this.affectedProperty;
			}
		}

		// Token: 0x0400116E RID: 4462
		private readonly IComponent affectedComponent;

		// Token: 0x0400116F RID: 4463
		private readonly string affectedProperty;
	}
}
