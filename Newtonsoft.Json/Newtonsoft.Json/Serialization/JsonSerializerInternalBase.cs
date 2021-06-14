using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000C0 RID: 192
	internal abstract class JsonSerializerInternalBase
	{
		// Token: 0x06000954 RID: 2388 RVA: 0x00022434 File Offset: 0x00020634
		protected JsonSerializerInternalBase(JsonSerializer serializer)
		{
			ValidationUtils.ArgumentNotNull(serializer, "serializer");
			this.Serializer = serializer;
			this.TraceWriter = serializer.TraceWriter;
			this._serializing = (base.GetType() == typeof(JsonSerializerInternalWriter));
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000955 RID: 2389 RVA: 0x00022480 File Offset: 0x00020680
		internal BidirectionalDictionary<string, object> DefaultReferenceMappings
		{
			get
			{
				if (this._mappings == null)
				{
					this._mappings = new BidirectionalDictionary<string, object>(EqualityComparer<string>.Default, new JsonSerializerInternalBase.ReferenceEqualsEqualityComparer(), "A different value already has the Id '{0}'.", "A different Id has already been assigned for value '{0}'.");
				}
				return this._mappings;
			}
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x000224AF File Offset: 0x000206AF
		private ErrorContext GetErrorContext(object currentObject, object member, string path, Exception error)
		{
			if (this._currentErrorContext == null)
			{
				this._currentErrorContext = new ErrorContext(currentObject, member, path, error);
			}
			if (this._currentErrorContext.Error != error)
			{
				throw new InvalidOperationException("Current error context error is different to requested error.");
			}
			return this._currentErrorContext;
		}

		// Token: 0x06000957 RID: 2391 RVA: 0x000224E9 File Offset: 0x000206E9
		protected void ClearErrorContext()
		{
			if (this._currentErrorContext == null)
			{
				throw new InvalidOperationException("Could not clear error context. Error context is already null.");
			}
			this._currentErrorContext = null;
		}

		// Token: 0x06000958 RID: 2392 RVA: 0x00022508 File Offset: 0x00020708
		protected bool IsErrorHandled(object currentObject, JsonContract contract, object keyValue, IJsonLineInfo lineInfo, string path, Exception ex)
		{
			ErrorContext errorContext = this.GetErrorContext(currentObject, keyValue, path, ex);
			if (this.TraceWriter != null && this.TraceWriter.LevelFilter >= TraceLevel.Error && !errorContext.Traced)
			{
				errorContext.Traced = true;
				string text = this._serializing ? "Error serializing" : "Error deserializing";
				if (contract != null)
				{
					text = text + " " + contract.UnderlyingType;
				}
				text = text + ". " + ex.Message;
				if (!(ex is JsonException))
				{
					text = JsonPosition.FormatMessage(lineInfo, path, text);
				}
				this.TraceWriter.Trace(TraceLevel.Error, text, ex);
			}
			if (contract != null && currentObject != null)
			{
				contract.InvokeOnError(currentObject, this.Serializer.Context, errorContext);
			}
			if (!errorContext.Handled)
			{
				this.Serializer.OnError(new ErrorEventArgs(currentObject, errorContext));
			}
			return errorContext.Handled;
		}

		// Token: 0x04000349 RID: 841
		private ErrorContext _currentErrorContext;

		// Token: 0x0400034A RID: 842
		private BidirectionalDictionary<string, object> _mappings;

		// Token: 0x0400034B RID: 843
		private bool _serializing;

		// Token: 0x0400034C RID: 844
		internal readonly JsonSerializer Serializer;

		// Token: 0x0400034D RID: 845
		internal readonly ITraceWriter TraceWriter;

		// Token: 0x020000C1 RID: 193
		private class ReferenceEqualsEqualityComparer : IEqualityComparer<object>
		{
			// Token: 0x06000959 RID: 2393 RVA: 0x000225E0 File Offset: 0x000207E0
			bool IEqualityComparer<object>.Equals(object x, object y)
			{
				return object.ReferenceEquals(x, y);
			}

			// Token: 0x0600095A RID: 2394 RVA: 0x000225E9 File Offset: 0x000207E9
			int IEqualityComparer<object>.GetHashCode(object obj)
			{
				return RuntimeHelpers.GetHashCode(obj);
			}
		}
	}
}
