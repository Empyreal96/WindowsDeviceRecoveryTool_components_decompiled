using System;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;

namespace System.Windows
{
	/// <summary>The exception that is thrown when a resource reference key cannot be found during parsing or serialization of markup extension resources.</summary>
	// Token: 0x020000ED RID: 237
	[Serializable]
	public class ResourceReferenceKeyNotFoundException : InvalidOperationException
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.ResourceReferenceKeyNotFoundException" /> class.</summary>
		// Token: 0x0600086C RID: 2156 RVA: 0x0001B7E6 File Offset: 0x000199E6
		public ResourceReferenceKeyNotFoundException()
		{
			this._resourceKey = null;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.ResourceReferenceKeyNotFoundException" /> class with the specified error message and resource key.</summary>
		/// <param name="message">A possible descriptive message.</param>
		/// <param name="resourceKey">The key that was not found.</param>
		// Token: 0x0600086D RID: 2157 RVA: 0x0001B7F5 File Offset: 0x000199F5
		public ResourceReferenceKeyNotFoundException(string message, object resourceKey) : base(message)
		{
			this._resourceKey = resourceKey;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.ResourceReferenceKeyNotFoundException" /> class with the specified serialization information and streaming context.</summary>
		/// <param name="info">Specific information from the serialization process.</param>
		/// <param name="context">The context at the time the exception was thrown.</param>
		// Token: 0x0600086E RID: 2158 RVA: 0x0001B805 File Offset: 0x00019A05
		protected ResourceReferenceKeyNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._resourceKey = info.GetValue("Key", typeof(object));
		}

		/// <summary>Gets the key that was not found and caused the exception to be thrown.</summary>
		/// <returns>The resource key.</returns>
		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x0600086F RID: 2159 RVA: 0x0001B82A File Offset: 0x00019A2A
		public object Key
		{
			get
			{
				return this._resourceKey;
			}
		}

		/// <summary>Reports specifics of the exception to debuggers or dialogs.</summary>
		/// <param name="info">Specific information from the serialization process.</param>
		/// <param name="context">The context at the time the exception was thrown.</param>
		// Token: 0x06000870 RID: 2160 RVA: 0x0001B832 File Offset: 0x00019A32
		[SecurityCritical]
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("Key", this._resourceKey);
		}

		// Token: 0x0400079E RID: 1950
		private object _resourceKey;
	}
}
