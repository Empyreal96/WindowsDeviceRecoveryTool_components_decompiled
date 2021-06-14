using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Library;

namespace Microsoft.Data.OData.Query.SemanticAst
{
	// Token: 0x02000084 RID: 132
	public sealed class OperationSegment : ODataPathSegment
	{
		// Token: 0x06000316 RID: 790 RVA: 0x0000B0A4 File Offset: 0x000092A4
		public OperationSegment(IEdmFunctionImport operation, IEdmEntitySet entitySet) : this()
		{
			ExceptionUtils.CheckArgumentNotNull<IEdmFunctionImport>(operation, "operation");
			this.operations = new ReadOnlyCollection<IEdmFunctionImport>(new IEdmFunctionImport[]
			{
				operation
			});
			this.entitySet = entitySet;
			this.computedReturnEdmType = ((operation.ReturnType != null) ? operation.ReturnType.Definition : null);
			this.EnsureTypeAndSetAreCompatable();
		}

		// Token: 0x06000317 RID: 791 RVA: 0x0000B134 File Offset: 0x00009334
		public OperationSegment(IEnumerable<IEdmFunctionImport> operationsIn, IEdmEntitySet entitySet) : this()
		{
			ExceptionUtils.CheckArgumentNotNull<IEnumerable<IEdmFunctionImport>>(operationsIn, "operationsIn");
			this.operations = new ReadOnlyCollection<IEdmFunctionImport>(operationsIn.ToList<IEdmFunctionImport>());
			ExceptionUtils.CheckArgumentCollectionNotNullOrEmpty<IEdmFunctionImport>(this.operations, "operations");
			IEdmType typeSoFar = (this.operations.First<IEdmFunctionImport>().ReturnType != null) ? this.operations.First<IEdmFunctionImport>().ReturnType.Definition : null;
			if (typeSoFar == null)
			{
				if (this.operations.Any((IEdmFunctionImport operation) => operation.ReturnType != null))
				{
					typeSoFar = OperationSegment.UnknownSentinel;
				}
			}
			else if (this.operations.Any((IEdmFunctionImport operation) => !typeSoFar.IsEquivalentTo(operation.ReturnType.Definition)))
			{
				typeSoFar = OperationSegment.UnknownSentinel;
			}
			this.computedReturnEdmType = typeSoFar;
			this.entitySet = entitySet;
			this.EnsureTypeAndSetAreCompatable();
		}

		// Token: 0x06000318 RID: 792 RVA: 0x0000B225 File Offset: 0x00009425
		public OperationSegment(IEnumerable<IEdmFunctionImport> operationsIn, IEnumerable<OperationSegmentParameter> parameters, IEdmEntitySet entitySet) : this(operationsIn, entitySet)
		{
			this.parameters = new ReadOnlyCollection<OperationSegmentParameter>((parameters == null) ? new List<OperationSegmentParameter>() : parameters.ToList<OperationSegmentParameter>());
		}

		// Token: 0x06000319 RID: 793 RVA: 0x0000B24A File Offset: 0x0000944A
		private OperationSegment()
		{
			this.parameters = new ReadOnlyCollection<OperationSegmentParameter>(new List<OperationSegmentParameter>());
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x0600031A RID: 794 RVA: 0x0000B262 File Offset: 0x00009462
		public IEnumerable<IEdmFunctionImport> Operations
		{
			get
			{
				return this.operations.AsEnumerable<IEdmFunctionImport>();
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600031B RID: 795 RVA: 0x0000B26F File Offset: 0x0000946F
		public IEnumerable<OperationSegmentParameter> Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600031C RID: 796 RVA: 0x0000B277 File Offset: 0x00009477
		public override IEdmType EdmType
		{
			get
			{
				if (object.ReferenceEquals(this.computedReturnEdmType, OperationSegment.UnknownSentinel))
				{
					throw new ODataException(Strings.OperationSegment_ReturnTypeForMultipleOverloads);
				}
				return this.computedReturnEdmType;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x0600031D RID: 797 RVA: 0x0000B29C File Offset: 0x0000949C
		public IEdmEntitySet EntitySet
		{
			get
			{
				return this.entitySet;
			}
		}

		// Token: 0x0600031E RID: 798 RVA: 0x0000B2A4 File Offset: 0x000094A4
		public override T Translate<T>(PathSegmentTranslator<T> translator)
		{
			ExceptionUtils.CheckArgumentNotNull<PathSegmentTranslator<T>>(translator, "translator");
			return translator.Translate(this);
		}

		// Token: 0x0600031F RID: 799 RVA: 0x0000B2B8 File Offset: 0x000094B8
		public override void Handle(PathSegmentHandler handler)
		{
			ExceptionUtils.CheckArgumentNotNull<PathSegmentHandler>(handler, "translator");
			handler.Handle(this);
		}

		// Token: 0x06000320 RID: 800 RVA: 0x0000B2CC File Offset: 0x000094CC
		internal override bool Equals(ODataPathSegment other)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataPathSegment>(other, "other");
			OperationSegment operationSegment = other as OperationSegment;
			return operationSegment != null && operationSegment.Operations.SequenceEqual(this.Operations) && operationSegment.EntitySet == this.entitySet;
		}

		// Token: 0x06000321 RID: 801 RVA: 0x0000B314 File Offset: 0x00009514
		private void EnsureTypeAndSetAreCompatable()
		{
			if (this.entitySet == null || this.computedReturnEdmType == OperationSegment.UnknownSentinel)
			{
				return;
			}
			if (this.computedReturnEdmType == null)
			{
				throw new ODataException(Strings.OperationSegment_CannotReturnNull);
			}
			IEdmType definition = this.computedReturnEdmType;
			IEdmCollectionType edmCollectionType = this.computedReturnEdmType as IEdmCollectionType;
			if (edmCollectionType != null)
			{
				definition = edmCollectionType.ElementType.Definition;
			}
			if (!this.entitySet.ElementType.IsOrInheritsFrom(definition) && !definition.IsOrInheritsFrom(this.entitySet.ElementType))
			{
				throw new ODataException(Strings.OperationSegment_CannotReturnNull);
			}
		}

		// Token: 0x040000E6 RID: 230
		private static readonly IEdmType UnknownSentinel = new EdmEnumType("Sentinel", "UndeterminableTypeMarker");

		// Token: 0x040000E7 RID: 231
		private readonly ReadOnlyCollection<IEdmFunctionImport> operations;

		// Token: 0x040000E8 RID: 232
		private readonly ReadOnlyCollection<OperationSegmentParameter> parameters;

		// Token: 0x040000E9 RID: 233
		private readonly IEdmEntitySet entitySet;

		// Token: 0x040000EA RID: 234
		private readonly IEdmType computedReturnEdmType;
	}
}
