using System;

namespace System.Management
{
	/// <summary>Holds event data for the <see cref="E:System.Management.ManagementOperationObserver.Progress" /> event.          </summary>
	// Token: 0x02000012 RID: 18
	public class ProgressEventArgs : ManagementEventArgs
	{
		// Token: 0x06000059 RID: 89 RVA: 0x00003E95 File Offset: 0x00002095
		internal ProgressEventArgs(object context, int upperBound, int current, string message) : base(context)
		{
			this.upperBound = upperBound;
			this.current = current;
			this.message = message;
		}

		/// <summary>Gets the total amount of work required to be done by the operation.          </summary>
		/// <returns>Returns an <see cref="T:System.Int32" /> value representing the total amount of work to be done by the operation.</returns>
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00003EB4 File Offset: 0x000020B4
		public int UpperBound
		{
			get
			{
				return this.upperBound;
			}
		}

		/// <summary>Gets the current amount of work done by the operation. This is always less than or equal to <see cref="P:System.Management.ProgressEventArgs.UpperBound" />.          </summary>
		/// <returns>Returns an <see cref="T:System.Int32" /> value representing the current amount of work already completed by the operation.</returns>
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600005B RID: 91 RVA: 0x00003EBC File Offset: 0x000020BC
		public int Current
		{
			get
			{
				return this.current;
			}
		}

		/// <summary>Gets or sets optional additional information regarding the operation's progress.          </summary>
		/// <returns>Returns a <see cref="T:System.String" /> value containing information regarding the operation's progress.</returns>
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600005C RID: 92 RVA: 0x00003EC4 File Offset: 0x000020C4
		public string Message
		{
			get
			{
				if (this.message == null)
				{
					return string.Empty;
				}
				return this.message;
			}
		}

		// Token: 0x04000081 RID: 129
		private int upperBound;

		// Token: 0x04000082 RID: 130
		private int current;

		// Token: 0x04000083 RID: 131
		private string message;
	}
}
