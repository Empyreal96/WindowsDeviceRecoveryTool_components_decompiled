using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x02000114 RID: 276
	internal abstract class ArrangedElement : Component, IArrangedElement, IComponent, IDisposable
	{
		// Token: 0x06000610 RID: 1552 RVA: 0x00011954 File Offset: 0x0000FB54
		internal ArrangedElement()
		{
			this.Padding = this.DefaultPadding;
			this.Margin = this.DefaultMargin;
			this.state[ArrangedElement.stateVisible] = true;
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000611 RID: 1553 RVA: 0x000119B1 File Offset: 0x0000FBB1
		public Rectangle Bounds
		{
			get
			{
				return this.bounds;
			}
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000612 RID: 1554 RVA: 0x000119B9 File Offset: 0x0000FBB9
		ArrangedElementCollection IArrangedElement.Children
		{
			get
			{
				return this.GetChildren();
			}
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000613 RID: 1555 RVA: 0x000119C1 File Offset: 0x0000FBC1
		IArrangedElement IArrangedElement.Container
		{
			get
			{
				return this.GetContainer();
			}
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06000614 RID: 1556 RVA: 0x000119C9 File Offset: 0x0000FBC9
		protected virtual Padding DefaultMargin
		{
			get
			{
				return Padding.Empty;
			}
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000615 RID: 1557 RVA: 0x000119C9 File Offset: 0x0000FBC9
		protected virtual Padding DefaultPadding
		{
			get
			{
				return Padding.Empty;
			}
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000616 RID: 1558 RVA: 0x000119D0 File Offset: 0x0000FBD0
		public virtual Rectangle DisplayRectangle
		{
			get
			{
				return this.Bounds;
			}
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000617 RID: 1559
		public abstract LayoutEngine LayoutEngine { get; }

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000618 RID: 1560 RVA: 0x000119E5 File Offset: 0x0000FBE5
		// (set) Token: 0x06000619 RID: 1561 RVA: 0x000119ED File Offset: 0x0000FBED
		public Padding Margin
		{
			get
			{
				return CommonProperties.GetMargin(this);
			}
			set
			{
				value = LayoutUtils.ClampNegativePaddingToZero(value);
				if (this.Margin != value)
				{
					CommonProperties.SetMargin(this, value);
				}
			}
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x0600061A RID: 1562 RVA: 0x00011A0C File Offset: 0x0000FC0C
		// (set) Token: 0x0600061B RID: 1563 RVA: 0x00011A1A File Offset: 0x0000FC1A
		public virtual Padding Padding
		{
			get
			{
				return CommonProperties.GetPadding(this, this.DefaultPadding);
			}
			set
			{
				value = LayoutUtils.ClampNegativePaddingToZero(value);
				if (this.Padding != value)
				{
					CommonProperties.SetPadding(this, value);
				}
			}
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x0600061C RID: 1564 RVA: 0x00011A39 File Offset: 0x0000FC39
		// (set) Token: 0x0600061D RID: 1565 RVA: 0x00011A41 File Offset: 0x0000FC41
		public virtual IArrangedElement Parent
		{
			get
			{
				return this.parent;
			}
			set
			{
				this.parent = value;
			}
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x0600061E RID: 1566 RVA: 0x00011A4A File Offset: 0x0000FC4A
		public virtual bool ParticipatesInLayout
		{
			get
			{
				return this.Visible;
			}
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x0600061F RID: 1567 RVA: 0x00011A52 File Offset: 0x0000FC52
		PropertyStore IArrangedElement.Properties
		{
			get
			{
				return this.Properties;
			}
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000620 RID: 1568 RVA: 0x00011A5A File Offset: 0x0000FC5A
		private PropertyStore Properties
		{
			get
			{
				return this.propertyStore;
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000621 RID: 1569 RVA: 0x00011A62 File Offset: 0x0000FC62
		// (set) Token: 0x06000622 RID: 1570 RVA: 0x00011A74 File Offset: 0x0000FC74
		public virtual bool Visible
		{
			get
			{
				return this.state[ArrangedElement.stateVisible];
			}
			set
			{
				if (this.state[ArrangedElement.stateVisible] != value)
				{
					this.state[ArrangedElement.stateVisible] = value;
					if (this.Parent != null)
					{
						LayoutTransaction.DoLayout(this.Parent, this, PropertyNames.Visible);
					}
				}
			}
		}

		// Token: 0x06000623 RID: 1571
		protected abstract IArrangedElement GetContainer();

		// Token: 0x06000624 RID: 1572
		protected abstract ArrangedElementCollection GetChildren();

		// Token: 0x06000625 RID: 1573 RVA: 0x00011AB4 File Offset: 0x0000FCB4
		public virtual Size GetPreferredSize(Size constrainingSize)
		{
			return this.LayoutEngine.GetPreferredSize(this, constrainingSize - this.Padding.Size) + this.Padding.Size;
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x00011AF6 File Offset: 0x0000FCF6
		public virtual void PerformLayout(IArrangedElement container, string propertyName)
		{
			if (this.suspendCount <= 0)
			{
				this.OnLayout(new LayoutEventArgs(container, propertyName));
			}
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x00011B10 File Offset: 0x0000FD10
		protected virtual void OnLayout(LayoutEventArgs e)
		{
			bool flag = this.LayoutEngine.Layout(this, e);
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x00011B2B File Offset: 0x0000FD2B
		protected virtual void OnBoundsChanged(Rectangle oldBounds, Rectangle newBounds)
		{
			((IArrangedElement)this).PerformLayout(this, PropertyNames.Size);
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x00011B39 File Offset: 0x0000FD39
		public void SetBounds(Rectangle bounds, BoundsSpecified specified)
		{
			this.SetBoundsCore(bounds, specified);
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x00011B44 File Offset: 0x0000FD44
		protected virtual void SetBoundsCore(Rectangle bounds, BoundsSpecified specified)
		{
			if (bounds != this.bounds)
			{
				Rectangle oldBounds = this.bounds;
				this.bounds = bounds;
				this.OnBoundsChanged(oldBounds, bounds);
			}
		}

		// Token: 0x0400053E RID: 1342
		private Rectangle bounds = Rectangle.Empty;

		// Token: 0x0400053F RID: 1343
		private IArrangedElement parent;

		// Token: 0x04000540 RID: 1344
		private BitVector32 state;

		// Token: 0x04000541 RID: 1345
		private PropertyStore propertyStore = new PropertyStore();

		// Token: 0x04000542 RID: 1346
		private int suspendCount;

		// Token: 0x04000543 RID: 1347
		private static readonly int stateVisible = BitVector32.CreateMask();

		// Token: 0x04000544 RID: 1348
		private static readonly int stateDisposing = BitVector32.CreateMask(ArrangedElement.stateVisible);

		// Token: 0x04000545 RID: 1349
		private static readonly int stateLocked = BitVector32.CreateMask(ArrangedElement.stateDisposing);

		// Token: 0x04000546 RID: 1350
		private static readonly int PropControlsCollection = PropertyStore.CreateKey();

		// Token: 0x04000547 RID: 1351
		private Control spacer = new Control();
	}
}
