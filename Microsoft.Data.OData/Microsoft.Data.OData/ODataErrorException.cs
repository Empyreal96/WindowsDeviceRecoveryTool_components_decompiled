using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Microsoft.Data.OData
{
	// Token: 0x0200023B RID: 571
	[DebuggerDisplay("{Message}")]
	[Serializable]
	public sealed class ODataErrorException : ODataException
	{
		// Token: 0x0600123D RID: 4669 RVA: 0x0004465F File Offset: 0x0004285F
		public ODataErrorException() : this(Strings.ODataErrorException_GeneralError)
		{
		}

		// Token: 0x0600123E RID: 4670 RVA: 0x0004466C File Offset: 0x0004286C
		public ODataErrorException(string message) : this(message, null)
		{
		}

		// Token: 0x0600123F RID: 4671 RVA: 0x00044676 File Offset: 0x00042876
		public ODataErrorException(string message, Exception innerException) : this(message, innerException, new ODataError())
		{
		}

		// Token: 0x06001240 RID: 4672 RVA: 0x00044685 File Offset: 0x00042885
		public ODataErrorException(ODataError error) : this(Strings.ODataErrorException_GeneralError, null, error)
		{
		}

		// Token: 0x06001241 RID: 4673 RVA: 0x00044694 File Offset: 0x00042894
		public ODataErrorException(string message, ODataError error) : this(message, null, error)
		{
		}

		// Token: 0x06001242 RID: 4674 RVA: 0x000446B4 File Offset: 0x000428B4
		public ODataErrorException(string message, Exception innerException, ODataError error) : base(message, innerException)
		{
			this.state.ODataError = error;
			base.SerializeObjectState += delegate(object exception, SafeSerializationEventArgs eventArgs)
			{
				eventArgs.AddSerializedState(this.state);
			};
		}

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x06001243 RID: 4675 RVA: 0x000446EE File Offset: 0x000428EE
		public ODataError Error
		{
			get
			{
				return this.state.ODataError;
			}
		}

		// Token: 0x0400069B RID: 1691
		[NonSerialized]
		private ODataErrorException.ODataErrorExceptionSafeSerializationState state;

		// Token: 0x0200023C RID: 572
		[Serializable]
		private struct ODataErrorExceptionSafeSerializationState : ISafeSerializationData
		{
			// Token: 0x170003E0 RID: 992
			// (get) Token: 0x06001245 RID: 4677 RVA: 0x000446FB File Offset: 0x000428FB
			// (set) Token: 0x06001246 RID: 4678 RVA: 0x00044703 File Offset: 0x00042903
			public ODataError ODataError { get; set; }

			// Token: 0x06001247 RID: 4679 RVA: 0x0004470C File Offset: 0x0004290C
			void ISafeSerializationData.CompleteDeserialization(object obj)
			{
				ODataErrorException ex = obj as ODataErrorException;
				ex.state = this;
			}
		}
	}
}
