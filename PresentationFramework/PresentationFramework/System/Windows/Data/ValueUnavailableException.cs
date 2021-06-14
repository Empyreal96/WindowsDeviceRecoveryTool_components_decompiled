using System;
using System.Runtime.Serialization;

namespace System.Windows.Data
{
	/// <summary>The exception that is thrown by the <see cref="M:System.Windows.Data.BindingGroup.GetValue(System.Object,System.String)" /> method when the value is not available.</summary>
	// Token: 0x020001BD RID: 445
	[Serializable]
	public class ValueUnavailableException : SystemException
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Data.ValueUnavailableException" /> class. </summary>
		// Token: 0x06001CB5 RID: 7349 RVA: 0x0008680E File Offset: 0x00084A0E
		public ValueUnavailableException()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Data.ValueUnavailableException" /> class with the specified message. </summary>
		/// <param name="message">The message that describes the error. </param>
		// Token: 0x06001CB6 RID: 7350 RVA: 0x00086816 File Offset: 0x00084A16
		public ValueUnavailableException(string message) : base(message)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Data.ValueUnavailableException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
		/// <param name="message">The error message that explains the reason for the exception. </param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException" /> parameter is not a null reference (<see langword="Nothing" /> in Visual Basic), the current exception is raised in a <see langword="catch" /> block that handles the inner exception. </param>
		// Token: 0x06001CB7 RID: 7351 RVA: 0x0008681F File Offset: 0x00084A1F
		public ValueUnavailableException(string message, Exception innerException) : base(message, innerException)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Data.ValueUnavailableException" /> class with serialized data.</summary>
		/// <param name="info">The object that holds the serialized object data. </param>
		/// <param name="context">The contextual information about the source or destination. </param>
		// Token: 0x06001CB8 RID: 7352 RVA: 0x00086829 File Offset: 0x00084A29
		protected ValueUnavailableException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
