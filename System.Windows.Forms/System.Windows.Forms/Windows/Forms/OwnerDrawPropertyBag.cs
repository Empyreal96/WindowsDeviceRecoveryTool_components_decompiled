using System;
using System.Drawing;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	/// <summary>Contains values of properties that a component might need only occasionally.</summary>
	// Token: 0x02000304 RID: 772
	[Serializable]
	public class OwnerDrawPropertyBag : MarshalByRefObject, ISerializable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.OwnerDrawPropertyBag" /> class. </summary>
		/// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> value.</param>
		/// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> value.</param>
		// Token: 0x06002EBF RID: 11967 RVA: 0x000D91E0 File Offset: 0x000D73E0
		protected OwnerDrawPropertyBag(SerializationInfo info, StreamingContext context)
		{
			foreach (SerializationEntry serializationEntry in info)
			{
				if (serializationEntry.Name == "Font")
				{
					this.font = (Font)serializationEntry.Value;
				}
				else if (serializationEntry.Name == "ForeColor")
				{
					this.foreColor = (Color)serializationEntry.Value;
				}
				else if (serializationEntry.Name == "BackColor")
				{
					this.backColor = (Color)serializationEntry.Value;
				}
			}
		}

		// Token: 0x06002EC0 RID: 11968 RVA: 0x000D9297 File Offset: 0x000D7497
		internal OwnerDrawPropertyBag()
		{
		}

		/// <summary>Gets or sets the font of the text displayed by the component.</summary>
		/// <returns>The <see cref="T:System.Drawing.Font" /> to apply to the text displayed by the component. The default is <see langword="null" />.</returns>
		// Token: 0x17000B47 RID: 2887
		// (get) Token: 0x06002EC1 RID: 11969 RVA: 0x000D92B5 File Offset: 0x000D74B5
		// (set) Token: 0x06002EC2 RID: 11970 RVA: 0x000D92BD File Offset: 0x000D74BD
		public Font Font
		{
			get
			{
				return this.font;
			}
			set
			{
				this.font = value;
			}
		}

		/// <summary>Gets or sets the foreground color of the component.</summary>
		/// <returns>The foreground color of the component. The default is <see cref="F:System.Drawing.Color.Empty" />.</returns>
		// Token: 0x17000B48 RID: 2888
		// (get) Token: 0x06002EC3 RID: 11971 RVA: 0x000D92C6 File Offset: 0x000D74C6
		// (set) Token: 0x06002EC4 RID: 11972 RVA: 0x000D92CE File Offset: 0x000D74CE
		public Color ForeColor
		{
			get
			{
				return this.foreColor;
			}
			set
			{
				this.foreColor = value;
			}
		}

		/// <summary>Gets or sets the background color for the component.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the background color of the component. The default is <see cref="F:System.Drawing.Color.Empty" />.</returns>
		// Token: 0x17000B49 RID: 2889
		// (get) Token: 0x06002EC5 RID: 11973 RVA: 0x000D92D7 File Offset: 0x000D74D7
		// (set) Token: 0x06002EC6 RID: 11974 RVA: 0x000D92DF File Offset: 0x000D74DF
		public Color BackColor
		{
			get
			{
				return this.backColor;
			}
			set
			{
				this.backColor = value;
			}
		}

		// Token: 0x17000B4A RID: 2890
		// (get) Token: 0x06002EC7 RID: 11975 RVA: 0x000D92E8 File Offset: 0x000D74E8
		internal IntPtr FontHandle
		{
			get
			{
				if (this.fontWrapper == null)
				{
					this.fontWrapper = new Control.FontHandleWrapper(this.Font);
				}
				return this.fontWrapper.Handle;
			}
		}

		/// <summary>Returns whether the <see cref="T:System.Windows.Forms.OwnerDrawPropertyBag" /> contains all default values.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.OwnerDrawPropertyBag" /> contains all default values; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002EC8 RID: 11976 RVA: 0x000D930E File Offset: 0x000D750E
		public virtual bool IsEmpty()
		{
			return this.Font == null && this.foreColor.IsEmpty && this.backColor.IsEmpty;
		}

		/// <summary>Copies an <see cref="T:System.Windows.Forms.OwnerDrawPropertyBag" />.</summary>
		/// <param name="value">The <see cref="T:System.Windows.Forms.OwnerDrawPropertyBag" /> to be copied.</param>
		/// <returns>A new copy of the <see cref="T:System.Windows.Forms.OwnerDrawPropertyBag" /> control.</returns>
		// Token: 0x06002EC9 RID: 11977 RVA: 0x000D9334 File Offset: 0x000D7534
		public static OwnerDrawPropertyBag Copy(OwnerDrawPropertyBag value)
		{
			object obj = OwnerDrawPropertyBag.internalSyncObject;
			OwnerDrawPropertyBag result;
			lock (obj)
			{
				OwnerDrawPropertyBag ownerDrawPropertyBag = new OwnerDrawPropertyBag();
				if (value == null)
				{
					result = ownerDrawPropertyBag;
				}
				else
				{
					ownerDrawPropertyBag.backColor = value.backColor;
					ownerDrawPropertyBag.foreColor = value.foreColor;
					ownerDrawPropertyBag.Font = value.font;
					result = ownerDrawPropertyBag;
				}
			}
			return result;
		}

		/// <summary>Populates the specified <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with the data needed to serialize the target object.</summary>
		/// <param name="si">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to populate with data.</param>
		/// <param name="context">The destination for this serialization.</param>
		// Token: 0x06002ECA RID: 11978 RVA: 0x000D93A4 File Offset: 0x000D75A4
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo si, StreamingContext context)
		{
			si.AddValue("BackColor", this.BackColor);
			si.AddValue("ForeColor", this.ForeColor);
			si.AddValue("Font", this.Font);
		}

		// Token: 0x04001D45 RID: 7493
		private Font font;

		// Token: 0x04001D46 RID: 7494
		private Color foreColor = Color.Empty;

		// Token: 0x04001D47 RID: 7495
		private Color backColor = Color.Empty;

		// Token: 0x04001D48 RID: 7496
		private Control.FontHandleWrapper fontWrapper;

		// Token: 0x04001D49 RID: 7497
		private static object internalSyncObject = new object();
	}
}
