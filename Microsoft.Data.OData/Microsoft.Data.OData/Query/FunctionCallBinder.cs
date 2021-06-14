using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Library;
using Microsoft.Data.OData.Metadata;
using Microsoft.Data.OData.Query.Metadata;
using Microsoft.Data.OData.Query.SemanticAst;
using Microsoft.Data.OData.Query.SyntacticAst;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x0200002B RID: 43
	internal sealed class FunctionCallBinder
	{
		// Token: 0x06000112 RID: 274 RVA: 0x00005484 File Offset: 0x00003684
		internal FunctionCallBinder(MetadataBinder.QueryTokenVisitor bindMethod)
		{
			this.bindMethod = bindMethod;
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00005494 File Offset: 0x00003694
		internal static void TypePromoteArguments(FunctionSignature signature, List<QueryNode> argumentNodes)
		{
			for (int i = 0; i < argumentNodes.Count; i++)
			{
				SingleValueNode source = (SingleValueNode)argumentNodes[i];
				IEdmTypeReference targetTypeReference = signature.ArgumentTypes[i];
				argumentNodes[i] = MetadataBindingUtils.ConvertToTypeIfNeeded(source, targetTypeReference);
			}
		}

		// Token: 0x06000114 RID: 276 RVA: 0x000054D8 File Offset: 0x000036D8
		internal static IEdmTypeReference[] EnsureArgumentsAreSingleValue(string functionName, List<QueryNode> argumentNodes)
		{
			ExceptionUtils.CheckArgumentNotNull<string>(functionName, "functionCallToken");
			ExceptionUtils.CheckArgumentNotNull<List<QueryNode>>(argumentNodes, "argumentNodes");
			IEdmTypeReference[] array = new IEdmTypeReference[argumentNodes.Count];
			for (int i = 0; i < argumentNodes.Count; i++)
			{
				SingleValueNode singleValueNode = argumentNodes[i] as SingleValueNode;
				if (singleValueNode == null)
				{
					throw new ODataException(Strings.MetadataBinder_FunctionArgumentNotSingleValue(functionName));
				}
				array[i] = singleValueNode.TypeReference;
			}
			return array;
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00005564 File Offset: 0x00003764
		internal static FunctionSignatureWithReturnType MatchSignatureToBuiltInFunction(string functionName, IEdmTypeReference[] argumentTypes, FunctionSignatureWithReturnType[] signatures)
		{
			int argumentCount = argumentTypes.Length;
			FunctionSignatureWithReturnType functionSignatureWithReturnType;
			if (argumentTypes.All((IEdmTypeReference a) => a == null) && argumentCount > 0)
			{
				functionSignatureWithReturnType = signatures.FirstOrDefault((FunctionSignatureWithReturnType candidateFunction) => candidateFunction.ArgumentTypes.Count<IEdmTypeReference>() == argumentCount);
				if (functionSignatureWithReturnType == null)
				{
					throw new ODataException(Strings.FunctionCallBinder_CannotFindASuitableOverload(functionName, argumentTypes.Count<IEdmTypeReference>()));
				}
				functionSignatureWithReturnType = new FunctionSignatureWithReturnType(null, functionSignatureWithReturnType.ArgumentTypes);
			}
			else
			{
				functionSignatureWithReturnType = TypePromotionUtils.FindBestFunctionSignature(signatures, argumentTypes);
				if (functionSignatureWithReturnType == null)
				{
					throw new ODataException(Strings.MetadataBinder_NoApplicableFunctionFound(functionName, BuiltInFunctions.BuildFunctionSignatureListDescription(functionName, signatures)));
				}
			}
			return functionSignatureWithReturnType;
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00005610 File Offset: 0x00003810
		internal static FunctionSignatureWithReturnType[] GetBuiltInFunctionSignatures(string functionName)
		{
			FunctionSignatureWithReturnType[] result;
			if (!BuiltInFunctions.TryGetBuiltInFunction(functionName, out result))
			{
				throw new ODataException(Strings.MetadataBinder_UnknownFunction(functionName));
			}
			return result;
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00005644 File Offset: 0x00003844
		internal QueryNode BindFunctionCall(FunctionCallToken functionCallToken, BindingState state)
		{
			ExceptionUtils.CheckArgumentNotNull<FunctionCallToken>(functionCallToken, "functionCallToken");
			ExceptionUtils.CheckArgumentNotNull<string>(functionCallToken.Name, "functionCallToken.Name");
			QueryNode parent;
			if (functionCallToken.Source != null)
			{
				parent = this.bindMethod(functionCallToken.Source);
			}
			else
			{
				parent = NodeFactory.CreateRangeVariableReferenceNode(state.ImplicitRangeVariable);
			}
			QueryNode result;
			if (this.TryBindIdentifier(functionCallToken.Name, functionCallToken.Arguments, parent, state, out result))
			{
				return result;
			}
			if (this.TryBindIdentifier(functionCallToken.Name, functionCallToken.Arguments, null, state, out result))
			{
				return result;
			}
			List<QueryNode> argumentNodes = new List<QueryNode>(from ar in functionCallToken.Arguments
			select this.bindMethod(ar));
			return FunctionCallBinder.BindAsBuiltInFunction(functionCallToken, state, argumentNodes);
		}

		// Token: 0x06000118 RID: 280 RVA: 0x000056EF File Offset: 0x000038EF
		internal bool TryBindEndPathAsFunctionCall(EndPathToken endPathToken, QueryNode parent, BindingState state, out QueryNode boundFunction)
		{
			return this.TryBindIdentifier(endPathToken.Identifier, null, parent, state, out boundFunction);
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00005702 File Offset: 0x00003902
		internal bool TryBindInnerPathAsFunctionCall(InnerPathToken innerPathToken, QueryNode parent, BindingState state, out QueryNode boundFunction)
		{
			return this.TryBindIdentifier(innerPathToken.Identifier, null, parent, state, out boundFunction);
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00005715 File Offset: 0x00003915
		internal bool TryBindDottedIdentifierAsFunctionCall(DottedIdentifierToken dottedIdentifierToken, SingleValueNode parent, BindingState state, out QueryNode boundFunction)
		{
			return this.TryBindIdentifier(dottedIdentifierToken.Identifier, null, parent, state, out boundFunction);
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00005728 File Offset: 0x00003928
		private static QueryNode BindAsBuiltInFunction(FunctionCallToken functionCallToken, BindingState state, List<QueryNode> argumentNodes)
		{
			if (functionCallToken.Source != null)
			{
				throw new ODataException(Strings.FunctionCallBinder_BuiltInFunctionMustHaveHaveNullParent(functionCallToken.Name));
			}
			if (FunctionCallBinder.IsUnboundFunction(functionCallToken.Name))
			{
				return FunctionCallBinder.CreateUnboundFunctionNode(functionCallToken, argumentNodes, state);
			}
			FunctionSignatureWithReturnType[] builtInFunctionSignatures = FunctionCallBinder.GetBuiltInFunctionSignatures(functionCallToken.Name);
			IEdmTypeReference[] argumentTypes = FunctionCallBinder.EnsureArgumentsAreSingleValue(functionCallToken.Name, argumentNodes);
			FunctionSignatureWithReturnType functionSignatureWithReturnType = FunctionCallBinder.MatchSignatureToBuiltInFunction(functionCallToken.Name, argumentTypes, builtInFunctionSignatures);
			if (functionSignatureWithReturnType.ReturnType != null)
			{
				FunctionCallBinder.TypePromoteArguments(functionSignatureWithReturnType, argumentNodes);
			}
			IEdmTypeReference returnType = functionSignatureWithReturnType.ReturnType;
			return new SingleValueFunctionCallNode(functionCallToken.Name, new ReadOnlyCollection<QueryNode>(argumentNodes), returnType);
		}

		// Token: 0x0600011C RID: 284 RVA: 0x000057CC File Offset: 0x000039CC
		private bool TryBindIdentifier(string identifier, IEnumerable<FunctionParameterToken> arguments, QueryNode parent, BindingState state, out QueryNode boundFunction)
		{
			boundFunction = null;
			IEdmType bindingType = null;
			SingleValueNode singleValueNode = parent as SingleValueNode;
			if (singleValueNode != null)
			{
				if (singleValueNode.TypeReference != null)
				{
					bindingType = singleValueNode.TypeReference.Definition;
				}
			}
			else
			{
				CollectionNode collectionNode = parent as CollectionNode;
				if (collectionNode != null)
				{
					bindingType = collectionNode.CollectionType.Definition;
				}
			}
			if (!UriEdmHelpers.IsBindingTypeValid(bindingType))
			{
				return false;
			}
			List<FunctionParameterToken> list = (arguments == null) ? new List<FunctionParameterToken>() : arguments.ToList<FunctionParameterToken>();
			IEdmFunctionImport edmFunctionImport;
			if (!FunctionOverloadResolver.ResolveFunctionsFromList(identifier, (from ar in list
			select ar.ParameterName).ToList<string>(), bindingType, state.Model, out edmFunctionImport))
			{
				return false;
			}
			if (singleValueNode != null && singleValueNode.TypeReference == null)
			{
				throw new ODataException(Strings.FunctionCallBinder_CallingFunctionOnOpenProperty(identifier));
			}
			if (edmFunctionImport.IsSideEffecting)
			{
				return false;
			}
			ICollection<FunctionParameterToken> source;
			if (!FunctionParameterParser.TryParseFunctionParameters(list, state.Configuration, edmFunctionImport, out source))
			{
				return false;
			}
			IEnumerable<QueryNode> enumerable = from p in source
			select this.bindMethod(p);
			IEdmTypeReference returnType = edmFunctionImport.ReturnType;
			IEdmEntitySet entitySet = null;
			SingleEntityNode singleEntityNode = parent as SingleEntityNode;
			if (singleEntityNode != null)
			{
				entitySet = edmFunctionImport.GetTargetEntitySet(singleEntityNode.EntitySet, state.Model);
			}
			if (returnType.IsEntity())
			{
				boundFunction = new SingleEntityFunctionCallNode(identifier, new IEdmFunctionImport[]
				{
					edmFunctionImport
				}, enumerable, (IEdmEntityTypeReference)returnType.Definition.ToTypeReference(), entitySet, parent);
			}
			else if (returnType.IsEntityCollection())
			{
				IEdmCollectionTypeReference returnedCollectionTypeReference = (IEdmCollectionTypeReference)returnType;
				boundFunction = new EntityCollectionFunctionCallNode(identifier, new IEdmFunctionImport[]
				{
					edmFunctionImport
				}, enumerable, returnedCollectionTypeReference, entitySet, parent);
			}
			else if (returnType.IsCollection())
			{
				IEdmCollectionTypeReference returnedCollectionType = (IEdmCollectionTypeReference)returnType;
				boundFunction = new CollectionFunctionCallNode(identifier, new IEdmFunctionImport[]
				{
					edmFunctionImport
				}, enumerable, returnedCollectionType, parent);
			}
			else
			{
				boundFunction = new SingleValueFunctionCallNode(identifier, new IEdmFunctionImport[]
				{
					edmFunctionImport
				}, enumerable, returnType, parent);
			}
			return true;
		}

		// Token: 0x0600011D RID: 285 RVA: 0x000059A4 File Offset: 0x00003BA4
		private static bool IsUnboundFunction(string functionName)
		{
			return FunctionCallBinder.UnboundFunctionNames.Contains(functionName);
		}

		// Token: 0x0600011E RID: 286 RVA: 0x000059B4 File Offset: 0x00003BB4
		private static SingleValueNode CreateUnboundFunctionNode(FunctionCallToken functionCallToken, List<QueryNode> args, BindingState state)
		{
			IEdmTypeReference edmTypeReference = null;
			string name;
			if ((name = functionCallToken.Name) != null)
			{
				if (!(name == "isof"))
				{
					if (name == "cast")
					{
						edmTypeReference = FunctionCallBinder.ValidateAndBuildCastArgs(state, ref args);
						if (edmTypeReference.IsEntity())
						{
							IEdmEntityTypeReference entityTypeReference = edmTypeReference.AsEntity();
							SingleEntityNode singleEntityNode = args.ElementAt(0) as SingleEntityNode;
							if (singleEntityNode != null)
							{
								return new SingleEntityFunctionCallNode(functionCallToken.Name, args, entityTypeReference, singleEntityNode.EntitySet);
							}
						}
					}
				}
				else
				{
					edmTypeReference = FunctionCallBinder.ValidateAndBuildIsOfArgs(state, ref args);
				}
			}
			return new SingleValueFunctionCallNode(functionCallToken.Name, args, edmTypeReference);
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00005A3D File Offset: 0x00003C3D
		private static IEdmTypeReference ValidateAndBuildCastArgs(BindingState state, ref List<QueryNode> args)
		{
			return FunctionCallBinder.ValidateIsOfOrCast(state, true, ref args);
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00005A47 File Offset: 0x00003C47
		private static IEdmTypeReference ValidateAndBuildIsOfArgs(BindingState state, ref List<QueryNode> args)
		{
			return FunctionCallBinder.ValidateIsOfOrCast(state, false, ref args);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00005A54 File Offset: 0x00003C54
		private static IEdmTypeReference ValidateIsOfOrCast(BindingState state, bool isCast, ref List<QueryNode> args)
		{
			if (args.Count != 1 && args.Count != 2)
			{
				throw new ODataErrorException(Strings.MetadataBinder_CastOrIsOfExpressionWithWrongNumberOfOperands(args.Count));
			}
			ConstantNode constantNode = args.Last<QueryNode>() as ConstantNode;
			IEdmTypeReference edmTypeReference = null;
			if (constantNode != null)
			{
				edmTypeReference = FunctionCallBinder.TryGetTypeReference(state.Model, constantNode.Value as string);
			}
			if (edmTypeReference == null)
			{
				throw new ODataException(Strings.MetadataBinder_CastOrIsOfFunctionWithoutATypeArgument);
			}
			if (edmTypeReference.IsCollection())
			{
				throw new ODataException(Strings.MetadataBinder_CastOrIsOfCollectionsNotSupported);
			}
			if (args.Count == 1)
			{
				args = new List<QueryNode>
				{
					new EntityRangeVariableReferenceNode(state.ImplicitRangeVariable.Name, state.ImplicitRangeVariable as EntityRangeVariable),
					args[0]
				};
			}
			else if (!(args[0] is SingleValueNode))
			{
				throw new ODataException(Strings.MetadataBinder_CastOrIsOfCollectionsNotSupported);
			}
			if (isCast)
			{
				return edmTypeReference;
			}
			return EdmCoreModel.Instance.GetBoolean(true);
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00005B44 File Offset: 0x00003D44
		private static IEdmTypeReference TryGetTypeReference(IEdmModel model, string fullTypeName)
		{
			IEdmTypeReference edmTypeReference = UriEdmHelpers.FindTypeFromModel(model, fullTypeName).ToTypeReference();
			if (edmTypeReference == null)
			{
				return UriEdmHelpers.FindCollectionTypeFromModel(model, fullTypeName);
			}
			return edmTypeReference;
		}

		// Token: 0x04000059 RID: 89
		private readonly MetadataBinder.QueryTokenVisitor bindMethod;

		// Token: 0x0400005A RID: 90
		private static readonly string[] UnboundFunctionNames = new string[]
		{
			"cast",
			"isof"
		};
	}
}
