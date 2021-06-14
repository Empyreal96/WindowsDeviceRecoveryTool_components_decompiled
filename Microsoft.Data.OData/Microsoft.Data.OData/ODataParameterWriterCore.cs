using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData
{
	// Token: 0x02000196 RID: 406
	internal abstract class ODataParameterWriterCore : ODataParameterWriter, IODataReaderWriterListener, IODataOutputInStreamErrorListener
	{
		// Token: 0x06000C1B RID: 3099 RVA: 0x00029A98 File Offset: 0x00027C98
		protected ODataParameterWriterCore(ODataOutputContext outputContext, IEdmFunctionImport functionImport)
		{
			this.outputContext = outputContext;
			this.functionImport = functionImport;
			this.scopes.Push(ODataParameterWriterCore.ParameterWriterState.Start);
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000C1C RID: 3100 RVA: 0x00029AD8 File Offset: 0x00027CD8
		protected DuplicatePropertyNamesChecker DuplicatePropertyNamesChecker
		{
			get
			{
				DuplicatePropertyNamesChecker result;
				if ((result = this.duplicatePropertyNamesChecker) == null)
				{
					result = (this.duplicatePropertyNamesChecker = new DuplicatePropertyNamesChecker(false, false));
				}
				return result;
			}
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06000C1D RID: 3101 RVA: 0x00029AFF File Offset: 0x00027CFF
		private ODataParameterWriterCore.ParameterWriterState State
		{
			get
			{
				return this.scopes.Peek();
			}
		}

		// Token: 0x06000C1E RID: 3102 RVA: 0x00029B0C File Offset: 0x00027D0C
		public sealed override void Flush()
		{
			this.VerifyCanFlush(true);
			this.InterceptException(new Action(this.FlushSynchronously));
		}

		// Token: 0x06000C1F RID: 3103 RVA: 0x00029B30 File Offset: 0x00027D30
		public sealed override Task FlushAsync()
		{
			this.VerifyCanFlush(false);
			return this.FlushAsynchronously().FollowOnFaultWith(delegate(Task t)
			{
				this.EnterErrorScope();
			});
		}

		// Token: 0x06000C20 RID: 3104 RVA: 0x00029B58 File Offset: 0x00027D58
		public sealed override void WriteStart()
		{
			this.VerifyCanWriteStart(true);
			this.InterceptException(delegate()
			{
				this.WriteStartImplementation();
			});
		}

		// Token: 0x06000C21 RID: 3105 RVA: 0x00029B8F File Offset: 0x00027D8F
		public sealed override Task WriteStartAsync()
		{
			this.VerifyCanWriteStart(false);
			return TaskUtils.GetTaskForSynchronousOperation(delegate()
			{
				this.InterceptException(delegate()
				{
					this.WriteStartImplementation();
				});
			});
		}

		// Token: 0x06000C22 RID: 3106 RVA: 0x00029BD0 File Offset: 0x00027DD0
		public sealed override void WriteValue(string parameterName, object parameterValue)
		{
			ExceptionUtils.CheckArgumentStringNotNullOrEmpty(parameterName, "parameterName");
			IEdmTypeReference expectedTypeReference = this.VerifyCanWriteValueParameter(true, parameterName, parameterValue);
			this.InterceptException(delegate()
			{
				this.WriteValueImplementation(parameterName, parameterValue, expectedTypeReference);
			});
		}

		// Token: 0x06000C23 RID: 3107 RVA: 0x00029C74 File Offset: 0x00027E74
		public sealed override Task WriteValueAsync(string parameterName, object parameterValue)
		{
			ExceptionUtils.CheckArgumentStringNotNullOrEmpty(parameterName, "parameterName");
			IEdmTypeReference expectedTypeReference = this.VerifyCanWriteValueParameter(false, parameterName, parameterValue);
			return TaskUtils.GetTaskForSynchronousOperation(delegate()
			{
				this.InterceptException(delegate()
				{
					this.WriteValueImplementation(parameterName, parameterValue, expectedTypeReference);
				});
			});
		}

		// Token: 0x06000C24 RID: 3108 RVA: 0x00029CF8 File Offset: 0x00027EF8
		public sealed override ODataCollectionWriter CreateCollectionWriter(string parameterName)
		{
			ExceptionUtils.CheckArgumentStringNotNullOrEmpty(parameterName, "parameterName");
			IEdmTypeReference itemTypeReference = this.VerifyCanCreateCollectionWriter(true, parameterName);
			return this.InterceptException<ODataCollectionWriter>(() => this.CreateCollectionWriterImplementation(parameterName, itemTypeReference));
		}

		// Token: 0x06000C25 RID: 3109 RVA: 0x00029D88 File Offset: 0x00027F88
		public sealed override Task<ODataCollectionWriter> CreateCollectionWriterAsync(string parameterName)
		{
			ExceptionUtils.CheckArgumentStringNotNullOrEmpty(parameterName, "parameterName");
			IEdmTypeReference itemTypeReference = this.VerifyCanCreateCollectionWriter(false, parameterName);
			return TaskUtils.GetTaskForSynchronousOperation<ODataCollectionWriter>(() => this.InterceptException<ODataCollectionWriter>(() => this.CreateCollectionWriterImplementation(parameterName, itemTypeReference)));
		}

		// Token: 0x06000C26 RID: 3110 RVA: 0x00029DE5 File Offset: 0x00027FE5
		public sealed override void WriteEnd()
		{
			this.VerifyCanWriteEnd(true);
			this.InterceptException(delegate()
			{
				this.WriteEndImplementation();
			});
			if (this.State == ODataParameterWriterCore.ParameterWriterState.Completed)
			{
				this.Flush();
			}
		}

		// Token: 0x06000C27 RID: 3111 RVA: 0x00029E42 File Offset: 0x00028042
		public sealed override Task WriteEndAsync()
		{
			this.VerifyCanWriteEnd(false);
			return TaskUtils.GetTaskForSynchronousOperation(delegate()
			{
				this.InterceptException(delegate()
				{
					this.WriteEndImplementation();
				});
			}).FollowOnSuccessWithTask(delegate(Task task)
			{
				if (this.State == ODataParameterWriterCore.ParameterWriterState.Completed)
				{
					return this.FlushAsync();
				}
				return TaskUtils.CompletedTask;
			});
		}

		// Token: 0x06000C28 RID: 3112 RVA: 0x00029E6D File Offset: 0x0002806D
		void IODataReaderWriterListener.OnException()
		{
			this.ReplaceScope(ODataParameterWriterCore.ParameterWriterState.Error);
		}

		// Token: 0x06000C29 RID: 3113 RVA: 0x00029E76 File Offset: 0x00028076
		void IODataReaderWriterListener.OnCompleted()
		{
			this.ReplaceScope(ODataParameterWriterCore.ParameterWriterState.CanWriteParameter);
		}

		// Token: 0x06000C2A RID: 3114 RVA: 0x00029E7F File Offset: 0x0002807F
		void IODataOutputInStreamErrorListener.OnInStreamError()
		{
			throw new ODataException(Strings.ODataParameterWriter_InStreamErrorNotSupported);
		}

		// Token: 0x06000C2B RID: 3115
		protected abstract void VerifyNotDisposed();

		// Token: 0x06000C2C RID: 3116
		protected abstract void FlushSynchronously();

		// Token: 0x06000C2D RID: 3117
		protected abstract Task FlushAsynchronously();

		// Token: 0x06000C2E RID: 3118
		protected abstract void StartPayload();

		// Token: 0x06000C2F RID: 3119
		protected abstract void WriteValueParameter(string parameterName, object parameterValue, IEdmTypeReference expectedTypeReference);

		// Token: 0x06000C30 RID: 3120
		protected abstract ODataCollectionWriter CreateFormatCollectionWriter(string parameterName, IEdmTypeReference expectedItemType);

		// Token: 0x06000C31 RID: 3121
		protected abstract void EndPayload();

		// Token: 0x06000C32 RID: 3122 RVA: 0x00029E8B File Offset: 0x0002808B
		private void VerifyCanWriteStart(bool synchronousCall)
		{
			this.VerifyNotDisposed();
			this.VerifyCallAllowed(synchronousCall);
			if (this.State != ODataParameterWriterCore.ParameterWriterState.Start)
			{
				throw new ODataException(Strings.ODataParameterWriterCore_CannotWriteStart);
			}
		}

		// Token: 0x06000C33 RID: 3123 RVA: 0x00029EAD File Offset: 0x000280AD
		private void WriteStartImplementation()
		{
			this.InterceptException(new Action(this.StartPayload));
			this.EnterScope(ODataParameterWriterCore.ParameterWriterState.CanWriteParameter);
		}

		// Token: 0x06000C34 RID: 3124 RVA: 0x00029ECC File Offset: 0x000280CC
		private IEdmTypeReference VerifyCanWriteParameterAndGetTypeReference(bool synchronousCall, string parameterName)
		{
			this.VerifyNotDisposed();
			this.VerifyCallAllowed(synchronousCall);
			this.VerifyNotInErrorOrCompletedState();
			if (this.State != ODataParameterWriterCore.ParameterWriterState.CanWriteParameter)
			{
				throw new ODataException(Strings.ODataParameterWriterCore_CannotWriteParameter);
			}
			if (this.parameterNamesWritten.Contains(parameterName))
			{
				throw new ODataException(Strings.ODataParameterWriterCore_DuplicatedParameterNameNotAllowed(parameterName));
			}
			this.parameterNamesWritten.Add(parameterName);
			return this.GetParameterTypeReference(parameterName);
		}

		// Token: 0x06000C35 RID: 3125 RVA: 0x00029F30 File Offset: 0x00028130
		private IEdmTypeReference VerifyCanWriteValueParameter(bool synchronousCall, string parameterName, object parameterValue)
		{
			IEdmTypeReference edmTypeReference = this.VerifyCanWriteParameterAndGetTypeReference(synchronousCall, parameterName);
			if (edmTypeReference != null && !edmTypeReference.IsODataPrimitiveTypeKind() && !edmTypeReference.IsODataComplexTypeKind())
			{
				throw new ODataException(Strings.ODataParameterWriterCore_CannotWriteValueOnNonValueTypeKind(parameterName, edmTypeReference.TypeKind()));
			}
			if (parameterValue != null && (!EdmLibraryExtensions.IsPrimitiveType(parameterValue.GetType()) || parameterValue is Stream) && !(parameterValue is ODataComplexValue))
			{
				throw new ODataException(Strings.ODataParameterWriterCore_CannotWriteValueOnNonSupportedValueType(parameterName, parameterValue.GetType()));
			}
			return edmTypeReference;
		}

		// Token: 0x06000C36 RID: 3126 RVA: 0x00029FA4 File Offset: 0x000281A4
		private IEdmTypeReference VerifyCanCreateCollectionWriter(bool synchronousCall, string parameterName)
		{
			IEdmTypeReference edmTypeReference = this.VerifyCanWriteParameterAndGetTypeReference(synchronousCall, parameterName);
			if (edmTypeReference != null && !edmTypeReference.IsNonEntityCollectionType())
			{
				throw new ODataException(Strings.ODataParameterWriterCore_CannotCreateCollectionWriterOnNonCollectionTypeKind(parameterName, edmTypeReference.TypeKind()));
			}
			if (edmTypeReference != null)
			{
				return edmTypeReference.GetCollectionItemType();
			}
			return null;
		}

		// Token: 0x06000C37 RID: 3127 RVA: 0x00029FE8 File Offset: 0x000281E8
		private IEdmTypeReference GetParameterTypeReference(string parameterName)
		{
			if (this.functionImport == null)
			{
				return null;
			}
			IEdmFunctionParameter edmFunctionParameter = this.functionImport.FindParameter(parameterName);
			if (edmFunctionParameter == null)
			{
				throw new ODataException(Strings.ODataParameterWriterCore_ParameterNameNotFoundInFunctionImport(parameterName, this.functionImport.Name));
			}
			return this.outputContext.EdmTypeResolver.GetParameterType(edmFunctionParameter);
		}

		// Token: 0x06000C38 RID: 3128 RVA: 0x0002A060 File Offset: 0x00028260
		private void WriteValueImplementation(string parameterName, object parameterValue, IEdmTypeReference expectedTypeReference)
		{
			this.InterceptException(delegate()
			{
				this.WriteValueParameter(parameterName, parameterValue, expectedTypeReference);
			});
		}

		// Token: 0x06000C39 RID: 3129 RVA: 0x0002A0A4 File Offset: 0x000282A4
		private ODataCollectionWriter CreateCollectionWriterImplementation(string parameterName, IEdmTypeReference expectedItemType)
		{
			ODataCollectionWriter result = this.CreateFormatCollectionWriter(parameterName, expectedItemType);
			this.ReplaceScope(ODataParameterWriterCore.ParameterWriterState.ActiveSubWriter);
			return result;
		}

		// Token: 0x06000C3A RID: 3130 RVA: 0x0002A0C2 File Offset: 0x000282C2
		private void VerifyCanWriteEnd(bool synchronousCall)
		{
			this.VerifyNotDisposed();
			this.VerifyCallAllowed(synchronousCall);
			this.VerifyNotInErrorOrCompletedState();
			if (this.State != ODataParameterWriterCore.ParameterWriterState.CanWriteParameter)
			{
				throw new ODataException(Strings.ODataParameterWriterCore_CannotWriteEnd);
			}
			this.VerifyAllParametersWritten();
		}

		// Token: 0x06000C3B RID: 3131 RVA: 0x0002A154 File Offset: 0x00028354
		private void VerifyAllParametersWritten()
		{
			if (this.functionImport != null && this.functionImport.Parameters != null)
			{
				IEnumerable<IEdmFunctionParameter> source;
				if (this.functionImport.IsBindable)
				{
					source = this.functionImport.Parameters.Skip(1);
				}
				else
				{
					source = this.functionImport.Parameters;
				}
				IEnumerable<string> source2 = from p in source
				where !this.parameterNamesWritten.Contains(p.Name) && !this.outputContext.EdmTypeResolver.GetParameterType(p).IsNullable
				select p.Name;
				if (source2.Any<string>())
				{
					source2 = from name in source2
					select string.Format(CultureInfo.InvariantCulture, "'{0}'", new object[]
					{
						name
					});
					throw new ODataException(Strings.ODataParameterWriterCore_MissingParameterInParameterPayload(string.Join(", ", source2.ToArray<string>()), this.functionImport.Name));
				}
			}
		}

		// Token: 0x06000C3C RID: 3132 RVA: 0x0002A241 File Offset: 0x00028441
		private void WriteEndImplementation()
		{
			this.InterceptException(delegate()
			{
				this.EndPayload();
			});
			this.LeaveScope();
		}

		// Token: 0x06000C3D RID: 3133 RVA: 0x0002A25B File Offset: 0x0002845B
		private void VerifyNotInErrorOrCompletedState()
		{
			if (this.State == ODataParameterWriterCore.ParameterWriterState.Error || this.State == ODataParameterWriterCore.ParameterWriterState.Completed)
			{
				throw new ODataException(Strings.ODataParameterWriterCore_CannotWriteInErrorOrCompletedState);
			}
		}

		// Token: 0x06000C3E RID: 3134 RVA: 0x0002A27A File Offset: 0x0002847A
		private void VerifyCanFlush(bool synchronousCall)
		{
			this.VerifyNotDisposed();
			this.VerifyCallAllowed(synchronousCall);
		}

		// Token: 0x06000C3F RID: 3135 RVA: 0x0002A289 File Offset: 0x00028489
		private void VerifyCallAllowed(bool synchronousCall)
		{
			if (synchronousCall)
			{
				if (!this.outputContext.Synchronous)
				{
					throw new ODataException(Strings.ODataParameterWriterCore_SyncCallOnAsyncWriter);
				}
			}
			else if (this.outputContext.Synchronous)
			{
				throw new ODataException(Strings.ODataParameterWriterCore_AsyncCallOnSyncWriter);
			}
		}

		// Token: 0x06000C40 RID: 3136 RVA: 0x0002A2C0 File Offset: 0x000284C0
		private void InterceptException(Action action)
		{
			try
			{
				action();
			}
			catch
			{
				this.EnterErrorScope();
				throw;
			}
		}

		// Token: 0x06000C41 RID: 3137 RVA: 0x0002A2F0 File Offset: 0x000284F0
		private T InterceptException<T>(Func<T> function)
		{
			T result;
			try
			{
				result = function();
			}
			catch
			{
				this.EnterErrorScope();
				throw;
			}
			return result;
		}

		// Token: 0x06000C42 RID: 3138 RVA: 0x0002A320 File Offset: 0x00028520
		private void EnterErrorScope()
		{
			if (this.State != ODataParameterWriterCore.ParameterWriterState.Error)
			{
				this.EnterScope(ODataParameterWriterCore.ParameterWriterState.Error);
			}
		}

		// Token: 0x06000C43 RID: 3139 RVA: 0x0002A332 File Offset: 0x00028532
		private void EnterScope(ODataParameterWriterCore.ParameterWriterState newState)
		{
			this.ValidateTransition(newState);
			this.scopes.Push(newState);
		}

		// Token: 0x06000C44 RID: 3140 RVA: 0x0002A347 File Offset: 0x00028547
		private void LeaveScope()
		{
			this.ValidateTransition(ODataParameterWriterCore.ParameterWriterState.Completed);
			if (this.State == ODataParameterWriterCore.ParameterWriterState.CanWriteParameter)
			{
				this.scopes.Pop();
			}
			this.ReplaceScope(ODataParameterWriterCore.ParameterWriterState.Completed);
		}

		// Token: 0x06000C45 RID: 3141 RVA: 0x0002A36C File Offset: 0x0002856C
		private void ReplaceScope(ODataParameterWriterCore.ParameterWriterState newState)
		{
			this.ValidateTransition(newState);
			this.scopes.Pop();
			this.scopes.Push(newState);
		}

		// Token: 0x06000C46 RID: 3142 RVA: 0x0002A390 File Offset: 0x00028590
		private void ValidateTransition(ODataParameterWriterCore.ParameterWriterState newState)
		{
			if (this.State != ODataParameterWriterCore.ParameterWriterState.Error && newState == ODataParameterWriterCore.ParameterWriterState.Error)
			{
				return;
			}
			switch (this.State)
			{
			case ODataParameterWriterCore.ParameterWriterState.Start:
				if (newState != ODataParameterWriterCore.ParameterWriterState.CanWriteParameter && newState != ODataParameterWriterCore.ParameterWriterState.Completed)
				{
					throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataParameterWriterCore_ValidateTransition_InvalidTransitionFromStart));
				}
				break;
			case ODataParameterWriterCore.ParameterWriterState.CanWriteParameter:
				if (newState != ODataParameterWriterCore.ParameterWriterState.CanWriteParameter && newState != ODataParameterWriterCore.ParameterWriterState.ActiveSubWriter && newState != ODataParameterWriterCore.ParameterWriterState.Completed)
				{
					throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataParameterWriterCore_ValidateTransition_InvalidTransitionFromCanWriteParameter));
				}
				break;
			case ODataParameterWriterCore.ParameterWriterState.ActiveSubWriter:
				if (newState != ODataParameterWriterCore.ParameterWriterState.CanWriteParameter)
				{
					throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataParameterWriterCore_ValidateTransition_InvalidTransitionFromActiveSubWriter));
				}
				break;
			case ODataParameterWriterCore.ParameterWriterState.Completed:
				throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataParameterWriterCore_ValidateTransition_InvalidTransitionFromCompleted));
			case ODataParameterWriterCore.ParameterWriterState.Error:
				if (newState != ODataParameterWriterCore.ParameterWriterState.Error)
				{
					throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataParameterWriterCore_ValidateTransition_InvalidTransitionFromError));
				}
				break;
			default:
				throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataParameterWriterCore_ValidateTransition_UnreachableCodePath));
			}
		}

		// Token: 0x0400042D RID: 1069
		private readonly ODataOutputContext outputContext;

		// Token: 0x0400042E RID: 1070
		private readonly IEdmFunctionImport functionImport;

		// Token: 0x0400042F RID: 1071
		private Stack<ODataParameterWriterCore.ParameterWriterState> scopes = new Stack<ODataParameterWriterCore.ParameterWriterState>();

		// Token: 0x04000430 RID: 1072
		private HashSet<string> parameterNamesWritten = new HashSet<string>(StringComparer.Ordinal);

		// Token: 0x04000431 RID: 1073
		private DuplicatePropertyNamesChecker duplicatePropertyNamesChecker;

		// Token: 0x02000197 RID: 407
		private enum ParameterWriterState
		{
			// Token: 0x04000435 RID: 1077
			Start,
			// Token: 0x04000436 RID: 1078
			CanWriteParameter,
			// Token: 0x04000437 RID: 1079
			ActiveSubWriter,
			// Token: 0x04000438 RID: 1080
			Completed,
			// Token: 0x04000439 RID: 1081
			Error
		}
	}
}
