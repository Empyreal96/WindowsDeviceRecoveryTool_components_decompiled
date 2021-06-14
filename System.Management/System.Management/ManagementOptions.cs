using System;
using System.ComponentModel;

namespace System.Management
{
	/// <summary>Provides an abstract base class for all options objects.</summary>
	// Token: 0x0200002B RID: 43
	[TypeConverter(typeof(ExpandableObjectConverter))]
	public abstract class ManagementOptions : ICloneable
	{
		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000137 RID: 311 RVA: 0x00007F58 File Offset: 0x00006158
		// (remove) Token: 0x06000138 RID: 312 RVA: 0x00007F90 File Offset: 0x00006190
		internal event IdentifierChangedEventHandler IdentifierChanged;

		// Token: 0x06000139 RID: 313 RVA: 0x00007FC5 File Offset: 0x000061C5
		internal void FireIdentifierChanged()
		{
			if (this.IdentifierChanged != null)
			{
				this.IdentifierChanged(this, null);
			}
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00007FDC File Offset: 0x000061DC
		internal void HandleIdentifierChange(object sender, IdentifierChangedEventArgs args)
		{
			this.FireIdentifierChanged();
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600013B RID: 315 RVA: 0x00007FE4 File Offset: 0x000061E4
		// (set) Token: 0x0600013C RID: 316 RVA: 0x00007FEC File Offset: 0x000061EC
		internal int Flags
		{
			get
			{
				return this.flags;
			}
			set
			{
				this.flags = value;
			}
		}

		/// <summary>Gets or sets a WMI context object. This is a name-value pairs list to be passed through to a WMI provider that supports context information for customized operation.          </summary>
		/// <returns>Returns a <see cref="T:System.Management.ManagementNamedValueCollection" /> that contains WMI context information. </returns>
		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600013D RID: 317 RVA: 0x00007FF8 File Offset: 0x000061F8
		// (set) Token: 0x0600013E RID: 318 RVA: 0x00008024 File Offset: 0x00006224
		public ManagementNamedValueCollection Context
		{
			get
			{
				if (this.context == null)
				{
					return this.context = new ManagementNamedValueCollection();
				}
				return this.context;
			}
			set
			{
				ManagementNamedValueCollection managementNamedValueCollection = this.context;
				if (value != null)
				{
					this.context = value.Clone();
				}
				else
				{
					this.context = new ManagementNamedValueCollection();
				}
				if (managementNamedValueCollection != null)
				{
					managementNamedValueCollection.IdentifierChanged -= this.HandleIdentifierChange;
				}
				this.context.IdentifierChanged += this.HandleIdentifierChange;
				this.HandleIdentifierChange(this, null);
			}
		}

		/// <summary>Gets or sets the time-out to apply to the operation. Note that for operations that return collections, this time-out applies to the enumeration through the resulting collection, not the operation itself (the <see cref="P:System.Management.EnumerationOptions.ReturnImmediately" />                   property is used for the latter). This property is used to indicate that the operation should be performed semi-synchronously.                       </summary>
		/// <returns>Returns a <see cref="T:System.TimeSpan" /> that defines the time-out time to apply to the operation.</returns>
		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600013F RID: 319 RVA: 0x00008088 File Offset: 0x00006288
		// (set) Token: 0x06000140 RID: 320 RVA: 0x00008090 File Offset: 0x00006290
		public TimeSpan Timeout
		{
			get
			{
				return this.timeout;
			}
			set
			{
				if (value.Ticks < 0L)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.timeout = value;
				this.FireIdentifierChanged();
			}
		}

		// Token: 0x06000141 RID: 321 RVA: 0x000080B5 File Offset: 0x000062B5
		internal ManagementOptions() : this(null, ManagementOptions.InfiniteTimeout)
		{
		}

		// Token: 0x06000142 RID: 322 RVA: 0x000080C3 File Offset: 0x000062C3
		internal ManagementOptions(ManagementNamedValueCollection context, TimeSpan timeout) : this(context, timeout, 0)
		{
		}

		// Token: 0x06000143 RID: 323 RVA: 0x000080CE File Offset: 0x000062CE
		internal ManagementOptions(ManagementNamedValueCollection context, TimeSpan timeout, int flags)
		{
			this.flags = flags;
			if (context != null)
			{
				this.Context = context;
			}
			else
			{
				this.context = null;
			}
			this.Timeout = timeout;
		}

		// Token: 0x06000144 RID: 324 RVA: 0x000080F7 File Offset: 0x000062F7
		internal IWbemContext GetContext()
		{
			if (this.context != null)
			{
				return this.context.GetContext();
			}
			return null;
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000145 RID: 325 RVA: 0x0000810E File Offset: 0x0000630E
		// (set) Token: 0x06000146 RID: 326 RVA: 0x00008121 File Offset: 0x00006321
		internal bool SendStatus
		{
			get
			{
				return (this.Flags & 128) != 0;
			}
			set
			{
				this.Flags = ((!value) ? (this.Flags & -129) : (this.Flags | 128));
			}
		}

		/// <summary>Returns a copy of the object.          </summary>
		/// <returns>The cloned object.</returns>
		// Token: 0x06000147 RID: 327
		public abstract object Clone();

		/// <summary>Indicates that no timeout should occur.</summary>
		// Token: 0x04000135 RID: 309
		public static readonly TimeSpan InfiniteTimeout = TimeSpan.MaxValue;

		// Token: 0x04000136 RID: 310
		internal int flags;

		// Token: 0x04000137 RID: 311
		internal ManagementNamedValueCollection context;

		// Token: 0x04000138 RID: 312
		internal TimeSpan timeout;
	}
}
