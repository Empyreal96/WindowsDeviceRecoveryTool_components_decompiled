using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;
using Microsoft.JScript;
using Microsoft.VisualBasic;

namespace System.Management
{
	// Token: 0x02000051 RID: 81
	internal class ManagementClassGenerator
	{
		// Token: 0x060002EB RID: 747 RVA: 0x000106F8 File Offset: 0x0000E8F8
		public ManagementClassGenerator()
		{
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0001080C File Offset: 0x0000EA0C
		public ManagementClassGenerator(ManagementClass cls)
		{
			this.classobj = cls;
		}

		// Token: 0x060002ED RID: 749 RVA: 0x00010924 File Offset: 0x0000EB24
		public CodeTypeDeclaration GenerateCode(bool includeSystemProperties, bool systemPropertyClass)
		{
			CodeTypeDeclaration result;
			if (systemPropertyClass)
			{
				this.InitilializePublicPrivateMembers();
				result = this.GenerateSystemPropertiesClass();
			}
			else
			{
				this.CheckIfClassIsProperlyInitialized();
				this.InitializeCodeGeneration();
				result = this.GetCodeTypeDeclarationForClass(includeSystemProperties);
			}
			return result;
		}

		// Token: 0x060002EE RID: 750 RVA: 0x00010958 File Offset: 0x0000EB58
		public bool GenerateCode(CodeLanguage lang, string filePath, string netNamespace)
		{
			if (filePath == null)
			{
				throw new ArgumentOutOfRangeException(ManagementClassGenerator.GetString("NULLFILEPATH_EXCEPT"));
			}
			if (filePath.Length == 0)
			{
				throw new ArgumentOutOfRangeException(ManagementClassGenerator.GetString("EMPTY_FILEPATH_EXCEPT"));
			}
			this.NETNamespace = netNamespace;
			this.CheckIfClassIsProperlyInitialized();
			this.InitializeCodeGeneration();
			this.tw = new StreamWriter(new FileStream(filePath, FileMode.Create), Encoding.UTF8);
			return this.GenerateAndWriteCode(lang);
		}

		// Token: 0x060002EF RID: 751 RVA: 0x000109C4 File Offset: 0x0000EBC4
		private void CheckIfClassIsProperlyInitialized()
		{
			if (this.classobj == null)
			{
				if (this.OriginalNamespace == null || (this.OriginalNamespace != null && this.OriginalNamespace.Length == 0))
				{
					throw new ArgumentOutOfRangeException(ManagementClassGenerator.GetString("NAMESPACE_NOTINIT_EXCEPT"));
				}
				if (this.OriginalClassName == null || (this.OriginalClassName != null && this.OriginalClassName.Length == 0))
				{
					throw new ArgumentOutOfRangeException(ManagementClassGenerator.GetString("CLASSNAME_NOTINIT_EXCEPT"));
				}
			}
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x00010A33 File Offset: 0x0000EC33
		private void InitializeCodeGeneration()
		{
			this.InitializeClassObject();
			this.InitilializePublicPrivateMembers();
			this.ProcessNamespaceAndClassName();
			this.ProcessNamingCollisions();
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x00010A50 File Offset: 0x0000EC50
		private CodeTypeDeclaration GetCodeTypeDeclarationForClass(bool bIncludeSystemClassinClassDef)
		{
			this.cc = new CodeTypeDeclaration(this.PrivateNamesUsed["GeneratedClassName"].ToString());
			this.cc.BaseTypes.Add(new CodeTypeReference(this.PrivateNamesUsed["ComponentClass"].ToString()));
			this.AddClassComments(this.cc);
			this.GeneratePublicReadOnlyProperty(this.PublicNamesUsed["NamespaceProperty"].ToString(), "System.String", this.OriginalNamespace, false, true, ManagementClassGenerator.GetString("COMMENT_ORIGNAMESPACE"));
			this.GeneratePrivateMember(this.PrivateNamesUsed["CreationWmiNamespace"].ToString(), "System.String", new CodePrimitiveExpression(this.OriginalNamespace), true, ManagementClassGenerator.GetString("COMMENT_CREATEDWMINAMESPACE"));
			this.GenerateClassNameProperty();
			this.GeneratePrivateMember(this.PrivateNamesUsed["CreationClassName"].ToString(), "System.String", new CodePrimitiveExpression(this.OriginalClassName), true, ManagementClassGenerator.GetString("COMMENT_CREATEDCLASS"));
			this.GeneratePublicReadOnlyProperty(this.PublicNamesUsed["SystemPropertiesProperty"].ToString(), this.PublicNamesUsed["SystemPropertiesClass"].ToString(), this.PrivateNamesUsed["SystemPropertiesObject"].ToString(), true, true, ManagementClassGenerator.GetString("COMMENT_SYSOBJECT"));
			this.GeneratePublicReadOnlyProperty(this.PublicNamesUsed["LateBoundObjectProperty"].ToString(), this.PublicNamesUsed["BaseObjClass"].ToString(), this.PrivateNamesUsed["CurrentObject"].ToString(), true, false, ManagementClassGenerator.GetString("COMMENT_LATEBOUNDPROP"));
			this.GenerateScopeProperty();
			this.GeneratePublicProperty(this.PublicNamesUsed["AutoCommitProperty"].ToString(), "System.Boolean", new CodeSnippetExpression(this.PrivateNamesUsed["AutoCommitProperty"].ToString()), false, ManagementClassGenerator.GetString("COMMENT_AUTOCOMMITPROP"), false);
			this.GeneratePathProperty();
			this.GeneratePrivateMember(this.PrivateNamesUsed["statMgmtScope"].ToString(), this.PublicNamesUsed["ScopeClass"].ToString(), new CodePrimitiveExpression(null), true, ManagementClassGenerator.GetString("COMMENT_STATICMANAGEMENTSCOPE"));
			this.GeneratePublicProperty(this.PrivateNamesUsed["staticScope"].ToString(), this.PublicNamesUsed["ScopeClass"].ToString(), new CodeVariableReferenceExpression(this.PrivateNamesUsed["statMgmtScope"].ToString()), true, ManagementClassGenerator.GetString("COMMENT_STATICSCOPEPROPERTY"), true);
			this.GenerateIfClassvalidFunction();
			this.GenerateProperties();
			this.GenerateMethodToInitializeVariables();
			this.GenerateConstructPath();
			this.GenerateDefaultConstructor();
			this.GenerateInitializeObject();
			if (this.bSingletonClass)
			{
				this.GenerateConstructorWithScope();
				this.GenerateConstructorWithOptions();
				this.GenerateConstructorWithScopeOptions();
			}
			else
			{
				this.GenerateConstructorWithKeys();
				this.GenerateConstructorWithScopeKeys();
				this.GenerateConstructorWithPathOptions();
				this.GenerateConstructorWithScopePath();
				this.GenerateGetInstancesWithNoParameters();
				this.GenerateGetInstancesWithCondition();
				this.GenerateGetInstancesWithProperties();
				this.GenerateGetInstancesWithWhereProperties();
				this.GenerateGetInstancesWithScope();
				this.GenerateGetInstancesWithScopeCondition();
				this.GenerateGetInstancesWithScopeProperties();
				this.GenerateGetInstancesWithScopeWhereProperties();
				this.GenerateCollectionClass();
			}
			this.GenerateConstructorWithPath();
			this.GenerateConstructorWithScopePathOptions();
			this.GenarateConstructorWithLateBound();
			this.GenarateConstructorWithLateBoundForEmbedded();
			this.GenerateCreateInstance();
			this.GenerateDeleteInstance();
			this.GenerateMethods();
			this.GeneratePrivateMember(this.PrivateNamesUsed["SystemPropertiesObject"].ToString(), this.PublicNamesUsed["SystemPropertiesClass"].ToString(), null);
			this.GeneratePrivateMember(this.PrivateNamesUsed["LateBoundObject"].ToString(), this.PublicNamesUsed["LateBoundClass"].ToString(), ManagementClassGenerator.GetString("COMMENT_LATEBOUNDOBJ"));
			this.GeneratePrivateMember(this.PrivateNamesUsed["AutoCommitProperty"].ToString(), "System.Boolean", new CodePrimitiveExpression(true), false, ManagementClassGenerator.GetString("COMMENT_PRIVAUTOCOMMIT"));
			this.GeneratePrivateMember(this.PrivateNamesUsed["EmbeddedObject"].ToString(), this.PublicNamesUsed["BaseObjClass"].ToString(), ManagementClassGenerator.GetString("COMMENT_EMBEDDEDOBJ"));
			this.GeneratePrivateMember(this.PrivateNamesUsed["CurrentObject"].ToString(), this.PublicNamesUsed["BaseObjClass"].ToString(), ManagementClassGenerator.GetString("COMMENT_CURRENTOBJ"));
			this.GeneratePrivateMember(this.PrivateNamesUsed["IsEmbedded"].ToString(), "System.Boolean", new CodePrimitiveExpression(false), false, ManagementClassGenerator.GetString("COMMENT_FLAGFOREMBEDDED"));
			this.cc.Members.Add(this.GenerateTypeConverterClass());
			if (bIncludeSystemClassinClassDef)
			{
				this.cc.Members.Add(this.GenerateSystemPropertiesClass());
			}
			if (this.bHasEmbeddedProperties)
			{
				this.AddCommentsForEmbeddedProperties();
			}
			this.cc.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("COMMENT_CLASSBEGIN") + this.OriginalClassName));
			return this.cc;
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x00010F58 File Offset: 0x0000F158
		private bool GenerateAndWriteCode(CodeLanguage lang)
		{
			if (!this.InitializeCodeGenerator(lang))
			{
				return false;
			}
			this.InitializeCodeTypeDeclaration(lang);
			this.GetCodeTypeDeclarationForClass(true);
			this.cc.Name = this.cp.CreateValidIdentifier(this.cc.Name);
			this.cn.Types.Add(this.cc);
			try
			{
				this.cp.GenerateCodeFromNamespace(this.cn, this.tw, new CodeGeneratorOptions());
			}
			finally
			{
				this.tw.Close();
			}
			return true;
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x00010FF4 File Offset: 0x0000F1F4
		private void InitializeClassObject()
		{
			if (this.classobj == null)
			{
				ManagementPath managementPath;
				if (this.OriginalPath.Length != 0)
				{
					managementPath = new ManagementPath(this.OriginalPath);
				}
				else
				{
					managementPath = new ManagementPath();
					if (this.OriginalServer.Length != 0)
					{
						managementPath.Server = this.OriginalServer;
					}
					managementPath.ClassName = this.OriginalClassName;
					managementPath.NamespacePath = this.OriginalNamespace;
				}
				this.classobj = new ManagementClass(managementPath);
			}
			else
			{
				ManagementPath path = this.classobj.Path;
				this.OriginalServer = path.Server;
				this.OriginalClassName = path.ClassName;
				this.OriginalNamespace = path.NamespacePath;
				char[] array = this.OriginalNamespace.ToCharArray();
				if (array.Length >= 2 && array[0] == '\\' && array[1] == '\\')
				{
					bool flag = false;
					int length = this.OriginalNamespace.Length;
					this.OriginalNamespace = string.Empty;
					for (int i = 2; i < length; i++)
					{
						if (flag)
						{
							this.OriginalNamespace += array[i].ToString();
						}
						else if (array[i] == '\\')
						{
							flag = true;
						}
					}
				}
			}
			try
			{
				this.classobj.Get();
			}
			catch (ManagementException)
			{
				throw;
			}
			this.bSingletonClass = false;
			foreach (QualifierData qualifierData in this.classobj.Qualifiers)
			{
				if (string.Compare(qualifierData.Name, "singleton", StringComparison.OrdinalIgnoreCase) == 0)
				{
					this.bSingletonClass = true;
					break;
				}
			}
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x000111A4 File Offset: 0x0000F3A4
		private void InitilializePublicPrivateMembers()
		{
			this.PublicNamesUsed.Add("SystemPropertiesProperty", "SystemProperties");
			this.PublicNamesUsed.Add("LateBoundObjectProperty", "LateBoundObject");
			this.PublicNamesUsed.Add("NamespaceProperty", "OriginatingNamespace");
			this.PublicNamesUsed.Add("ClassNameProperty", "ManagementClassName");
			this.PublicNamesUsed.Add("ScopeProperty", "Scope");
			this.PublicNamesUsed.Add("PathProperty", "Path");
			this.PublicNamesUsed.Add("SystemPropertiesClass", "ManagementSystemProperties");
			this.PublicNamesUsed.Add("LateBoundClass", "System.Management.ManagementObject");
			this.PublicNamesUsed.Add("PathClass", "System.Management.ManagementPath");
			this.PublicNamesUsed.Add("ScopeClass", "System.Management.ManagementScope");
			this.PublicNamesUsed.Add("QueryOptionsClass", "System.Management.EnumerationOptions");
			this.PublicNamesUsed.Add("GetOptionsClass", "System.Management.ObjectGetOptions");
			this.PublicNamesUsed.Add("ArgumentExceptionClass", "System.ArgumentException");
			this.PublicNamesUsed.Add("QueryClass", "SelectQuery");
			this.PublicNamesUsed.Add("ObjectSearcherClass", "System.Management.ManagementObjectSearcher");
			this.PublicNamesUsed.Add("FilterFunction", "GetInstances");
			this.PublicNamesUsed.Add("ConstructPathFunction", "ConstructPath");
			this.PublicNamesUsed.Add("TypeConverter", "TypeConverter");
			this.PublicNamesUsed.Add("AutoCommitProperty", "AutoCommit");
			this.PublicNamesUsed.Add("CommitMethod", "CommitObject");
			this.PublicNamesUsed.Add("ManagementClass", "System.Management.ManagementClass");
			this.PublicNamesUsed.Add("NotSupportedExceptClass", "System.NotSupportedException");
			this.PublicNamesUsed.Add("BaseObjClass", "System.Management.ManagementBaseObject");
			this.PublicNamesUsed.Add("OptionsProp", "Options");
			this.PublicNamesUsed.Add("ClassPathProperty", "ClassPath");
			this.PublicNamesUsed.Add("CreateInst", "CreateInstance");
			this.PublicNamesUsed.Add("DeleteInst", "Delete");
			this.PublicNamesUsed.Add("SystemNameSpace", "System");
			this.PublicNamesUsed.Add("ArgumentOutOfRangeException", "System.ArgumentOutOfRangeException");
			this.PublicNamesUsed.Add("System", "System");
			this.PublicNamesUsed.Add("Other", "Other");
			this.PublicNamesUsed.Add("Unknown", "Unknown");
			this.PublicNamesUsed.Add("PutOptions", "System.Management.PutOptions");
			this.PublicNamesUsed.Add("Type", "System.Type");
			this.PublicNamesUsed.Add("Boolean", "System.Boolean");
			this.PublicNamesUsed.Add("ValueType", "System.ValueType");
			this.PublicNamesUsed.Add("Events1", "Events");
			this.PublicNamesUsed.Add("Component1", "Component");
			this.PrivateNamesUsed.Add("SystemPropertiesObject", "PrivateSystemProperties");
			this.PrivateNamesUsed.Add("LateBoundObject", "PrivateLateBoundObject");
			this.PrivateNamesUsed.Add("AutoCommitProperty", "AutoCommitProp");
			this.PrivateNamesUsed.Add("Privileges", "EnablePrivileges");
			this.PrivateNamesUsed.Add("ComponentClass", "System.ComponentModel.Component");
			this.PrivateNamesUsed.Add("ScopeParam", "mgmtScope");
			this.PrivateNamesUsed.Add("NullRefExcep", "System.NullReferenceException");
			this.PrivateNamesUsed.Add("ConverterClass", "WMIValueTypeConverter");
			this.PrivateNamesUsed.Add("EnumParam", "enumOptions");
			this.PrivateNamesUsed.Add("CreationClassName", "CreatedClassName");
			this.PrivateNamesUsed.Add("CreationWmiNamespace", "CreatedWmiNamespace");
			this.PrivateNamesUsed.Add("ClassNameCheckFunc", "CheckIfProperClass");
			this.PrivateNamesUsed.Add("EmbeddedObject", "embeddedObj");
			this.PrivateNamesUsed.Add("CurrentObject", "curObj");
			this.PrivateNamesUsed.Add("IsEmbedded", "isEmbedded");
			this.PrivateNamesUsed.Add("ToDateTimeMethod", "ToDateTime");
			this.PrivateNamesUsed.Add("ToDMTFDateTimeMethod", "ToDmtfDateTime");
			this.PrivateNamesUsed.Add("ToDMTFTimeIntervalMethod", "ToDmtfTimeInterval");
			this.PrivateNamesUsed.Add("ToTimeSpanMethod", "ToTimeSpan");
			this.PrivateNamesUsed.Add("SetMgmtScope", "SetStaticManagementScope");
			this.PrivateNamesUsed.Add("statMgmtScope", "statMgmtScope");
			this.PrivateNamesUsed.Add("staticScope", "StaticScope");
			this.PrivateNamesUsed.Add("initVariable", "Initialize");
			this.PrivateNamesUsed.Add("putOptions", "putOptions");
			this.PrivateNamesUsed.Add("InitialObjectFunc", "InitializeObject");
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x000116DC File Offset: 0x0000F8DC
		private void ProcessNamingCollisions()
		{
			if (this.classobj.Properties != null)
			{
				foreach (PropertyData propertyData in this.classobj.Properties)
				{
					this.PublicProperties.Add(propertyData.Name, propertyData.Name);
				}
			}
			if (this.classobj.Methods != null)
			{
				foreach (MethodData methodData in this.classobj.Methods)
				{
					this.PublicMethods.Add(methodData.Name, methodData.Name);
				}
			}
			foreach (object obj in this.PublicNamesUsed.Values)
			{
				string text = (string)obj;
				int num = this.IsContainedIn(text, ref this.PublicProperties);
				if (num != -1)
				{
					this.PublicProperties.SetByIndex(num, this.ResolveCollision(text, false));
				}
				else
				{
					num = this.IsContainedIn(text, ref this.PublicMethods);
					if (num != -1)
					{
						this.PublicMethods.SetByIndex(num, this.ResolveCollision(text, false));
					}
				}
			}
			foreach (object obj2 in this.PublicProperties.Values)
			{
				string text2 = (string)obj2;
				int num = this.IsContainedIn(text2, ref this.PrivateNamesUsed);
				if (num != -1)
				{
					this.PrivateNamesUsed.SetByIndex(num, this.ResolveCollision(text2, false));
				}
			}
			foreach (object obj3 in this.PublicMethods.Values)
			{
				string text3 = (string)obj3;
				int num = this.IsContainedIn(text3, ref this.PrivateNamesUsed);
				if (num != -1)
				{
					this.PrivateNamesUsed.SetByIndex(num, this.ResolveCollision(text3, false));
				}
			}
			foreach (object obj4 in this.PublicProperties.Values)
			{
				string text4 = (string)obj4;
				int num = this.IsContainedIn(text4, ref this.PublicMethods);
				if (num != -1)
				{
					this.PublicMethods.SetByIndex(num, this.ResolveCollision(text4, false));
				}
			}
			string inString = this.PrivateNamesUsed["GeneratedClassName"].ToString() + "Collection";
			this.PrivateNamesUsed.Add("CollectionClass", this.ResolveCollision(inString, true));
			inString = this.PrivateNamesUsed["GeneratedClassName"].ToString() + "Enumerator";
			this.PrivateNamesUsed.Add("EnumeratorClass", this.ResolveCollision(inString, true));
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x00011A38 File Offset: 0x0000FC38
		private string ResolveCollision(string inString, bool bCheckthisFirst)
		{
			string text = inString;
			bool flag = true;
			int num = -1;
			string text2 = "";
			if (!bCheckthisFirst)
			{
				num++;
				text = text + text2 + num.ToString((IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(int)));
			}
			while (flag)
			{
				if (this.IsContainedIn(text, ref this.PublicProperties) == -1 && this.IsContainedIn(text, ref this.PublicMethods) == -1 && this.IsContainedIn(text, ref this.PublicNamesUsed) == -1 && this.IsContainedIn(text, ref this.PrivateNamesUsed) == -1)
				{
					flag = false;
					break;
				}
				try
				{
					num++;
				}
				catch (OverflowException)
				{
					text2 += "_";
					num = 0;
				}
				text = inString + text2 + num.ToString((IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(int)));
			}
			if (text.Length > 0)
			{
				string str = text.Substring(0, 1).ToUpper(CultureInfo.InvariantCulture);
				text = str + text.Substring(1, text.Length - 1);
			}
			return text;
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x00011B58 File Offset: 0x0000FD58
		private void ProcessNamespaceAndClassName()
		{
			string text = string.Empty;
			string text2 = string.Empty;
			if (this.NETNamespace.Length == 0)
			{
				text2 = this.OriginalNamespace;
				text2 = text2.Replace('\\', '.');
				text2 = text2.ToUpper(CultureInfo.InvariantCulture);
			}
			else
			{
				text2 = this.NETNamespace;
			}
			if (this.OriginalClassName.IndexOf('_') > 0)
			{
				text = this.OriginalClassName.Substring(0, this.OriginalClassName.IndexOf('_'));
				if (this.NETNamespace.Length == 0)
				{
					text2 += ".";
					text2 += text;
				}
				text = this.OriginalClassName.Substring(this.OriginalClassName.IndexOf('_') + 1);
			}
			else
			{
				text = this.OriginalClassName;
			}
			if (!char.IsLetter(text[0]))
			{
				text = "C" + text;
			}
			text = this.ResolveCollision(text, true);
			if (Type.GetType("System." + text) != null || Type.GetType("System.ComponentModel." + text) != null || Type.GetType("System.Management." + text) != null || Type.GetType("System.Collections." + text) != null || Type.GetType("System.Globalization." + text) != null)
			{
				this.PublicNamesUsed.Add(text, text);
				text = this.ResolveCollision(text, true);
			}
			this.PrivateNamesUsed.Add("GeneratedClassName", text);
			this.PrivateNamesUsed.Add("GeneratedNamespace", text2);
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x00011CE8 File Offset: 0x0000FEE8
		private void InitializeCodeTypeDeclaration(CodeLanguage lang)
		{
			this.cn = new CodeNamespace(this.PrivateNamesUsed["GeneratedNamespace"].ToString());
			this.cn.Imports.Add(new CodeNamespaceImport("System"));
			this.cn.Imports.Add(new CodeNamespaceImport("System.ComponentModel"));
			this.cn.Imports.Add(new CodeNamespaceImport("System.Management"));
			this.cn.Imports.Add(new CodeNamespaceImport("System.Collections"));
			this.cn.Imports.Add(new CodeNamespaceImport("System.Globalization"));
			if (lang == CodeLanguage.VB)
			{
				this.cn.Imports.Add(new CodeNamespaceImport("Microsoft.VisualBasic"));
			}
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x00011DB8 File Offset: 0x0000FFB8
		private void GeneratePublicReadOnlyProperty(string propName, string propType, object propValue, bool isLiteral, bool isBrowsable, string Comment)
		{
			this.cmp = new CodeMemberProperty();
			this.cmp.Name = propName;
			this.cmp.Attributes = (MemberAttributes)24578;
			this.cmp.Type = new CodeTypeReference(propType);
			this.caa = new CodeAttributeArgument();
			this.caa.Value = new CodePrimitiveExpression(isBrowsable);
			this.cad = new CodeAttributeDeclaration();
			this.cad.Name = "Browsable";
			this.cad.Arguments.Add(this.caa);
			this.cmp.CustomAttributes = new CodeAttributeDeclarationCollection();
			this.cmp.CustomAttributes.Add(this.cad);
			this.caa = new CodeAttributeArgument();
			this.caa.Value = new CodeFieldReferenceExpression(new CodeTypeReferenceExpression("DesignerSerializationVisibility"), "Hidden");
			this.cad = new CodeAttributeDeclaration();
			this.cad.Name = "DesignerSerializationVisibility";
			this.cad.Arguments.Add(this.caa);
			this.cmp.CustomAttributes.Add(this.cad);
			if (isLiteral)
			{
				this.cmp.GetStatements.Add(new CodeMethodReturnStatement(new CodeSnippetExpression(propValue.ToString())));
			}
			else
			{
				this.cmp.GetStatements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(propValue)));
			}
			this.cc.Members.Add(this.cmp);
			if (Comment != null && Comment.Length != 0)
			{
				this.cmp.Comments.Add(new CodeCommentStatement(Comment));
			}
		}

		// Token: 0x060002FA RID: 762 RVA: 0x00011F6C File Offset: 0x0001016C
		private void GeneratePublicProperty(string propName, string propType, CodeExpression Value, bool isBrowsable, string Comment, bool isStatic)
		{
			this.cmp = new CodeMemberProperty();
			this.cmp.Name = propName;
			this.cmp.Attributes = (MemberAttributes)24578;
			this.cmp.Type = new CodeTypeReference(propType);
			if (isStatic)
			{
				this.cmp.Attributes = (this.cmp.Attributes | MemberAttributes.Static);
			}
			this.caa = new CodeAttributeArgument();
			this.caa.Value = new CodePrimitiveExpression(isBrowsable);
			this.cad = new CodeAttributeDeclaration();
			this.cad.Name = "Browsable";
			this.cad.Arguments.Add(this.caa);
			this.cmp.CustomAttributes = new CodeAttributeDeclarationCollection();
			this.cmp.CustomAttributes.Add(this.cad);
			if (ManagementClassGenerator.IsDesignerSerializationVisibilityToBeSet(propName))
			{
				this.caa = new CodeAttributeArgument();
				this.caa.Value = new CodeFieldReferenceExpression(new CodeTypeReferenceExpression("DesignerSerializationVisibility"), "Hidden");
				this.cad = new CodeAttributeDeclaration();
				this.cad.Name = "DesignerSerializationVisibility";
				this.cad.Arguments.Add(this.caa);
				this.cmp.CustomAttributes.Add(this.cad);
			}
			this.cmp.GetStatements.Add(new CodeMethodReturnStatement(Value));
			this.cmp.SetStatements.Add(new CodeAssignStatement(Value, new CodeSnippetExpression("value")));
			this.cc.Members.Add(this.cmp);
			if (Comment != null && Comment.Length != 0)
			{
				this.cmp.Comments.Add(new CodeCommentStatement(Comment));
			}
		}

		// Token: 0x060002FB RID: 763 RVA: 0x00012138 File Offset: 0x00010338
		private void GeneratePathProperty()
		{
			this.cmp = new CodeMemberProperty();
			this.cmp.Name = this.PublicNamesUsed["PathProperty"].ToString();
			this.cmp.Attributes = (MemberAttributes)24578;
			this.cmp.Type = new CodeTypeReference(this.PublicNamesUsed["PathClass"].ToString());
			this.caa = new CodeAttributeArgument();
			this.caa.Value = new CodePrimitiveExpression(true);
			this.cad = new CodeAttributeDeclaration();
			this.cad.Name = "Browsable";
			this.cad.Arguments.Add(this.caa);
			this.cmp.CustomAttributes = new CodeAttributeDeclarationCollection();
			this.cmp.CustomAttributes.Add(this.cad);
			this.cpre = new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(this.PrivateNamesUsed["LateBoundObject"].ToString()), "Path");
			this.cis = new CodeConditionStatement();
			this.cboe = new CodeBinaryOperatorExpression();
			this.cboe.Left = new CodeVariableReferenceExpression(this.PrivateNamesUsed["IsEmbedded"].ToString());
			this.cboe.Right = new CodePrimitiveExpression(false);
			this.cboe.Operator = CodeBinaryOperatorType.ValueEquality;
			this.cis.Condition = this.cboe;
			this.cis.TrueStatements.Add(new CodeMethodReturnStatement(this.cpre));
			this.cis.FalseStatements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(null)));
			this.cmp.GetStatements.Add(this.cis);
			this.cis = new CodeConditionStatement();
			this.cboe = new CodeBinaryOperatorExpression();
			this.cboe.Left = new CodeVariableReferenceExpression(this.PrivateNamesUsed["IsEmbedded"].ToString());
			this.cboe.Right = new CodePrimitiveExpression(false);
			this.cboe.Operator = CodeBinaryOperatorType.ValueEquality;
			this.cis.Condition = this.cboe;
			CodeConditionStatement codeConditionStatement = new CodeConditionStatement();
			this.cmie = new CodeMethodInvokeExpression();
			this.cmie.Method.MethodName = this.PrivateNamesUsed["ClassNameCheckFunc"].ToString();
			this.cmie.Parameters.Add(new CodePrimitiveExpression(null));
			this.cmie.Parameters.Add(new CodeVariableReferenceExpression("value"));
			this.cmie.Parameters.Add(new CodePrimitiveExpression(null));
			codeConditionStatement.Condition = new CodeBinaryOperatorExpression
			{
				Left = this.cmie,
				Right = new CodePrimitiveExpression(true),
				Operator = CodeBinaryOperatorType.IdentityInequality
			};
			this.coce = new CodeObjectCreateExpression();
			this.coce.CreateType = new CodeTypeReference(this.PublicNamesUsed["ArgumentExceptionClass"].ToString());
			this.coce.Parameters.Add(new CodePrimitiveExpression(ManagementClassGenerator.GetString("CLASSNOT_FOUND_EXCEPT")));
			codeConditionStatement.TrueStatements.Add(new CodeThrowExceptionStatement(this.coce));
			this.cis.TrueStatements.Add(codeConditionStatement);
			this.cis.TrueStatements.Add(new CodeAssignStatement(this.cpre, new CodeSnippetExpression("value")));
			this.cmp.SetStatements.Add(this.cis);
			this.cc.Members.Add(this.cmp);
			this.cmp.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("COMMENT_MGMTPATH")));
		}

		// Token: 0x060002FC RID: 764 RVA: 0x00012514 File Offset: 0x00010714
		private CodeTypeDeclaration GenerateSystemPropertiesClass()
		{
			CodeTypeDeclaration codeTypeDeclaration = new CodeTypeDeclaration(this.PublicNamesUsed["SystemPropertiesClass"].ToString());
			codeTypeDeclaration.TypeAttributes = TypeAttributes.NestedPublic;
			this.cctor = new CodeConstructor();
			this.cctor.Attributes = MemberAttributes.Public;
			this.cpde = new CodeParameterDeclarationExpression();
			this.cpde.Type = new CodeTypeReference(this.PublicNamesUsed["BaseObjClass"].ToString());
			this.cpde.Name = "ManagedObject";
			this.cctor.Parameters.Add(this.cpde);
			this.cctor.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(this.PrivateNamesUsed["LateBoundObject"].ToString()), new CodeVariableReferenceExpression("ManagedObject")));
			codeTypeDeclaration.Members.Add(this.cctor);
			this.caa = new CodeAttributeArgument();
			this.caa.Value = new CodeTypeOfExpression(typeof(ExpandableObjectConverter));
			this.cad = new CodeAttributeDeclaration();
			this.cad.Name = this.PublicNamesUsed["TypeConverter"].ToString();
			this.cad.Arguments.Add(this.caa);
			codeTypeDeclaration.CustomAttributes.Add(this.cad);
			foreach (PropertyData propertyData in this.classobj.SystemProperties)
			{
				this.cmp = new CodeMemberProperty();
				this.caa = new CodeAttributeArgument();
				this.caa.Value = new CodePrimitiveExpression(true);
				this.cad = new CodeAttributeDeclaration();
				this.cad.Name = "Browsable";
				this.cad.Arguments.Add(this.caa);
				this.cmp.CustomAttributes = new CodeAttributeDeclarationCollection();
				this.cmp.CustomAttributes.Add(this.cad);
				char[] array = propertyData.Name.ToCharArray();
				int num = 0;
				while (num < array.Length && !char.IsLetterOrDigit(array[num]))
				{
					num++;
				}
				if (num == array.Length)
				{
					num = 0;
				}
				char[] array2 = new char[array.Length - num];
				for (int i = num; i < array.Length; i++)
				{
					array2[i - num] = array[i];
				}
				this.cmp.Name = new string(array2).ToUpper(CultureInfo.InvariantCulture);
				this.cmp.Attributes = (MemberAttributes)24578;
				this.cmp.Type = this.ConvertCIMType(propertyData.Type, propertyData.IsArray);
				this.cie = new CodeIndexerExpression(new CodeVariableReferenceExpression(this.PrivateNamesUsed["LateBoundObject"].ToString()), new CodeExpression[]
				{
					new CodePrimitiveExpression(propertyData.Name)
				});
				this.cmp.GetStatements.Add(new CodeMethodReturnStatement(new CodeCastExpression(this.cmp.Type, this.cie)));
				codeTypeDeclaration.Members.Add(this.cmp);
			}
			this.cf = new CodeMemberField();
			this.cf.Name = this.PrivateNamesUsed["LateBoundObject"].ToString();
			this.cf.Attributes = (MemberAttributes)20482;
			this.cf.Type = new CodeTypeReference(this.PublicNamesUsed["BaseObjClass"].ToString());
			codeTypeDeclaration.Members.Add(this.cf);
			codeTypeDeclaration.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("COMMENT_SYSPROPCLASS")));
			return codeTypeDeclaration;
		}

		// Token: 0x060002FD RID: 765 RVA: 0x00012904 File Offset: 0x00010B04
		private void GenerateProperties()
		{
			bool bDynamicClass = this.IsDynamicClass();
			CodeMemberMethod codeMemberMethod = null;
			string text = string.Empty;
			for (int i = 0; i < this.PublicProperties.Count; i++)
			{
				bool flag = false;
				PropertyData propertyData = this.classobj.Properties[this.PublicProperties.GetKey(i).ToString()];
				bool flag2 = true;
				bool flag3 = true;
				bool flag4 = false;
				this.cmp = new CodeMemberProperty();
				this.cmp.Name = this.PublicProperties[propertyData.Name].ToString();
				this.cmp.Attributes = (MemberAttributes)24578;
				this.cmp.Type = this.ConvertCIMType(propertyData.Type, propertyData.IsArray);
				if (propertyData.Type == CimType.DateTime)
				{
					CodeTypeReference type = this.cmp.Type;
					flag = this.GetDateTimeType(propertyData, ref type);
					this.cmp.Type = type;
				}
				if ((this.cmp.Type.ArrayRank == 0 && this.cmp.Type.BaseType == new CodeTypeReference(this.PublicNamesUsed["BaseObjClass"].ToString()).BaseType) || (this.cmp.Type.ArrayRank > 0 && this.cmp.Type.ArrayElementType.BaseType == new CodeTypeReference(this.PublicNamesUsed["BaseObjClass"].ToString()).BaseType))
				{
					this.bHasEmbeddedProperties = true;
				}
				text = "Is" + this.PublicProperties[propertyData.Name].ToString() + "Null";
				CodeMemberProperty codeMemberProperty = new CodeMemberProperty();
				codeMemberProperty.Name = text;
				codeMemberProperty.Attributes = (MemberAttributes)24578;
				codeMemberProperty.Type = new CodeTypeReference("System.Boolean");
				this.caa = new CodeAttributeArgument();
				this.caa.Value = new CodePrimitiveExpression(true);
				this.cad = new CodeAttributeDeclaration();
				this.cad.Name = "Browsable";
				this.cad.Arguments.Add(this.caa);
				this.cmp.CustomAttributes = new CodeAttributeDeclarationCollection();
				this.cmp.CustomAttributes.Add(this.cad);
				this.caa = new CodeAttributeArgument();
				this.caa.Value = new CodePrimitiveExpression(false);
				this.cad = new CodeAttributeDeclaration();
				this.cad.Name = "Browsable";
				this.cad.Arguments.Add(this.caa);
				codeMemberProperty.CustomAttributes = new CodeAttributeDeclarationCollection();
				codeMemberProperty.CustomAttributes.Add(this.cad);
				this.caa = new CodeAttributeArgument();
				this.caa.Value = new CodeFieldReferenceExpression(new CodeTypeReferenceExpression("DesignerSerializationVisibility"), "Hidden");
				this.cad = new CodeAttributeDeclaration();
				this.cad.Name = "DesignerSerializationVisibility";
				this.cad.Arguments.Add(this.caa);
				this.cmp.CustomAttributes.Add(this.cad);
				codeMemberProperty.CustomAttributes.Add(this.cad);
				this.cie = new CodeIndexerExpression(new CodeVariableReferenceExpression(this.PrivateNamesUsed["CurrentObject"].ToString()), new CodeExpression[]
				{
					new CodePrimitiveExpression(propertyData.Name)
				});
				bool flag5 = false;
				string text2 = this.ProcessPropertyQualifiers(propertyData, ref flag2, ref flag3, ref flag4, bDynamicClass, out flag5);
				if (flag2 || flag3)
				{
					if (text2.Length != 0)
					{
						this.caa = new CodeAttributeArgument();
						this.caa.Value = new CodePrimitiveExpression(text2);
						this.cad = new CodeAttributeDeclaration();
						this.cad.Name = "Description";
						this.cad.Arguments.Add(this.caa);
						this.cmp.CustomAttributes.Add(this.cad);
					}
					bool flag6 = this.GeneratePropertyHelperEnums(propertyData, this.PublicProperties[propertyData.Name].ToString(), flag5);
					if (flag2)
					{
						if (ManagementClassGenerator.IsPropertyValueType(propertyData.Type) && !propertyData.IsArray)
						{
							this.cis = new CodeConditionStatement();
							this.cis.Condition = new CodeBinaryOperatorExpression(this.cie, CodeBinaryOperatorType.IdentityEquality, new CodePrimitiveExpression(null));
							this.cis.TrueStatements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(true)));
							this.cis.FalseStatements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(false)));
							codeMemberProperty.GetStatements.Add(this.cis);
							this.cc.Members.Add(codeMemberProperty);
							this.caa = new CodeAttributeArgument();
							this.caa.Value = new CodeTypeOfExpression(this.PrivateNamesUsed["ConverterClass"].ToString());
							this.cad = new CodeAttributeDeclaration();
							this.cad.Name = this.PublicNamesUsed["TypeConverter"].ToString();
							this.cad.Arguments.Add(this.caa);
							this.cmp.CustomAttributes.Add(this.cad);
							if (propertyData.Type != CimType.DateTime)
							{
								this.cis = new CodeConditionStatement();
								this.cis.Condition = new CodeBinaryOperatorExpression(this.cie, CodeBinaryOperatorType.IdentityEquality, new CodePrimitiveExpression(null));
								if (flag6)
								{
									if (propertyData.IsArray)
									{
										this.cis.TrueStatements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(null)));
									}
									else
									{
										this.cmie = new CodeMethodInvokeExpression();
										this.cmie.Method.TargetObject = new CodeTypeReferenceExpression("System.Convert");
										this.cmie.Parameters.Add(new CodePrimitiveExpression(propertyData.NullEnumValue));
										this.cmie.Method.MethodName = this.arrConvFuncName;
										this.cis.TrueStatements.Add(new CodeMethodReturnStatement(new CodeCastExpression(this.cmp.Type, this.cmie)));
									}
								}
								else
								{
									this.cmie = new CodeMethodInvokeExpression();
									this.cmie.Parameters.Add(new CodePrimitiveExpression(propertyData.NullEnumValue));
									this.cmie.Method.MethodName = this.GetConversionFunction(propertyData.Type);
									this.cmie.Method.TargetObject = new CodeTypeReferenceExpression("System.Convert");
									if (propertyData.IsArray)
									{
										CodeExpression[] initializers = new CodeExpression[]
										{
											this.cmie
										};
										this.cis.TrueStatements.Add(new CodeMethodReturnStatement(new CodeArrayCreateExpression(this.cmp.Type, initializers)));
									}
									else
									{
										this.cis.TrueStatements.Add(new CodeMethodReturnStatement(this.cmie));
									}
								}
								this.cmp.GetStatements.Add(this.cis);
							}
							this.cmm = new CodeMemberMethod();
							this.cmm.Name = "ShouldSerialize" + this.PublicProperties[propertyData.Name].ToString();
							this.cmm.Attributes = (MemberAttributes)20482;
							this.cmm.ReturnType = new CodeTypeReference("System.Boolean");
							CodeConditionStatement codeConditionStatement = new CodeConditionStatement();
							codeConditionStatement.Condition = new CodeBinaryOperatorExpression(new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), text), CodeBinaryOperatorType.ValueEquality, new CodePrimitiveExpression(false));
							codeConditionStatement.TrueStatements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(true)));
							this.cmm.Statements.Add(codeConditionStatement);
							this.cmm.Statements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(false)));
							this.cc.Members.Add(this.cmm);
						}
						if (propertyData.Type == CimType.Reference)
						{
							this.GenerateCodeForRefAndDateTimeTypes(this.cie, propertyData.IsArray, this.cmp.GetStatements, this.PublicNamesUsed["PathClass"].ToString(), null, false);
						}
						else if (propertyData.Type == CimType.DateTime)
						{
							if (flag)
							{
								this.GenerateCodeForRefAndDateTimeTypes(this.cie, propertyData.IsArray, this.cmp.GetStatements, "System.TimeSpan", null, false);
							}
							else
							{
								this.GenerateCodeForRefAndDateTimeTypes(this.cie, propertyData.IsArray, this.cmp.GetStatements, "System.DateTime", null, false);
							}
						}
						else if (flag6)
						{
							if (propertyData.IsArray)
							{
								this.AddGetStatementsForEnumArray(this.cie, this.cmp);
							}
							else
							{
								this.cmie = new CodeMethodInvokeExpression();
								this.cmie.Method.TargetObject = new CodeTypeReferenceExpression("System.Convert");
								this.cmie.Parameters.Add(this.cie);
								this.cmie.Method.MethodName = this.arrConvFuncName;
								this.cmp.GetStatements.Add(new CodeMethodReturnStatement(new CodeCastExpression(this.cmp.Type, this.cmie)));
							}
						}
						else
						{
							this.cmp.GetStatements.Add(new CodeMethodReturnStatement(new CodeCastExpression(this.cmp.Type, this.cie)));
						}
					}
					if (flag3)
					{
						if (flag5)
						{
							codeMemberMethod = new CodeMemberMethod();
							codeMemberMethod.Name = "Reset" + this.PublicProperties[propertyData.Name].ToString();
							codeMemberMethod.Attributes = (MemberAttributes)20482;
							codeMemberMethod.Statements.Add(new CodeAssignStatement(this.cie, new CodePrimitiveExpression(null)));
						}
						if (propertyData.Type == CimType.Reference)
						{
							this.AddPropertySet(this.cie, propertyData.IsArray, this.cmp.SetStatements, this.PublicNamesUsed["PathClass"].ToString(), null);
						}
						else if (propertyData.Type == CimType.DateTime)
						{
							if (flag)
							{
								this.AddPropertySet(this.cie, propertyData.IsArray, this.cmp.SetStatements, "System.TimeSpan", null);
							}
							else
							{
								this.AddPropertySet(this.cie, propertyData.IsArray, this.cmp.SetStatements, "System.DateTime", null);
							}
						}
						else if (flag6 && flag5)
						{
							CodeConditionStatement codeConditionStatement2 = new CodeConditionStatement();
							if (propertyData.IsArray)
							{
								codeConditionStatement2.Condition = new CodeBinaryOperatorExpression(new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(new CodeTypeReference(this.PublicProperties[propertyData.Name].ToString() + "Values")), "NULL_ENUM_VALUE"), CodeBinaryOperatorType.ValueEquality, new CodeArrayIndexerExpression(new CodeVariableReferenceExpression("value"), new CodeExpression[]
								{
									new CodePrimitiveExpression(0)
								}));
							}
							else
							{
								codeConditionStatement2.Condition = new CodeBinaryOperatorExpression(new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(new CodeTypeReference(this.PublicProperties[propertyData.Name].ToString() + "Values")), "NULL_ENUM_VALUE"), CodeBinaryOperatorType.ValueEquality, new CodeSnippetExpression("value"));
							}
							codeConditionStatement2.TrueStatements.Add(new CodeAssignStatement(this.cie, new CodePrimitiveExpression(null)));
							codeConditionStatement2.FalseStatements.Add(new CodeAssignStatement(this.cie, new CodeSnippetExpression("value")));
							this.cmp.SetStatements.Add(codeConditionStatement2);
						}
						else
						{
							this.cmp.SetStatements.Add(new CodeAssignStatement(this.cie, new CodeSnippetExpression("value")));
						}
						this.cmie = new CodeMethodInvokeExpression();
						this.cmie.Method.TargetObject = new CodeVariableReferenceExpression(this.PrivateNamesUsed["LateBoundObject"].ToString());
						this.cmie.Method.MethodName = "Put";
						this.cboe = new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression(this.PrivateNamesUsed["AutoCommitProperty"].ToString()), CodeBinaryOperatorType.ValueEquality, new CodePrimitiveExpression(true));
						CodeBinaryOperatorExpression left = new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression(this.PrivateNamesUsed["IsEmbedded"].ToString()), CodeBinaryOperatorType.ValueEquality, new CodePrimitiveExpression(false));
						CodeBinaryOperatorExpression codeBinaryOperatorExpression = new CodeBinaryOperatorExpression();
						codeBinaryOperatorExpression.Right = this.cboe;
						codeBinaryOperatorExpression.Left = left;
						codeBinaryOperatorExpression.Operator = CodeBinaryOperatorType.BooleanAnd;
						this.cis = new CodeConditionStatement();
						this.cis.Condition = codeBinaryOperatorExpression;
						this.cis.TrueStatements.Add(new CodeExpressionStatement(this.cmie));
						this.cmp.SetStatements.Add(this.cis);
						if (flag5)
						{
							codeMemberMethod.Statements.Add(this.cis);
						}
					}
					this.cc.Members.Add(this.cmp);
					if (flag5 && flag3)
					{
						this.cc.Members.Add(codeMemberMethod);
					}
				}
			}
			this.GenerateCommitMethod();
		}

		// Token: 0x060002FE RID: 766 RVA: 0x00013680 File Offset: 0x00011880
		private string ProcessPropertyQualifiers(PropertyData prop, ref bool bRead, ref bool bWrite, ref bool bStatic, bool bDynamicClass, out bool nullable)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			nullable = true;
			bRead = true;
			bWrite = false;
			this.arrConvFuncName = "ToInt32";
			this.enumType = "System.Int32";
			string result = string.Empty;
			foreach (QualifierData qualifierData in prop.Qualifiers)
			{
				if (string.Compare(qualifierData.Name, "description", StringComparison.OrdinalIgnoreCase) == 0)
				{
					result = qualifierData.Value.ToString();
				}
				else if (string.Compare(qualifierData.Name, "Not_Null", StringComparison.OrdinalIgnoreCase) == 0)
				{
					nullable = false;
				}
				else
				{
					if (string.Compare(qualifierData.Name, "key", StringComparison.OrdinalIgnoreCase) == 0)
					{
						this.arrKeyType.Add(this.cmp.Type);
						this.arrKeys.Add(prop.Name);
						nullable = false;
						break;
					}
					if (string.Compare(qualifierData.Name, "static", StringComparison.OrdinalIgnoreCase) == 0)
					{
						bStatic = true;
						this.cmp.Attributes |= MemberAttributes.Static;
					}
					else if (string.Compare(qualifierData.Name, "read", StringComparison.OrdinalIgnoreCase) == 0)
					{
						if (!(bool)qualifierData.Value)
						{
							bRead = false;
						}
						else
						{
							bRead = true;
						}
					}
					else if (string.Compare(qualifierData.Name, "write", StringComparison.OrdinalIgnoreCase) == 0)
					{
						flag = true;
						flag2 = (bool)qualifierData.Value;
					}
					else
					{
						if (string.Compare(qualifierData.Name, "ValueMap", StringComparison.OrdinalIgnoreCase) == 0 && !flag3)
						{
							try
							{
								this.ValueMap.Clear();
								if (ManagementClassGenerator.isTypeInt(prop.Type) && qualifierData.Value != null)
								{
									string[] array = (string[])qualifierData.Value;
									for (int i = 0; i < array.Length; i++)
									{
										try
										{
											this.arrConvFuncName = ManagementClassGenerator.ConvertToNumericValueAndAddToArray(prop.Type, array[i], this.ValueMap, out this.enumType);
										}
										catch (OverflowException)
										{
										}
									}
								}
								continue;
							}
							catch (FormatException)
							{
								flag3 = true;
								this.ValueMap.Clear();
								continue;
							}
							catch (InvalidCastException)
							{
								this.ValueMap.Clear();
								continue;
							}
						}
						if (string.Compare(qualifierData.Name, "Values", StringComparison.OrdinalIgnoreCase) == 0 && !flag3)
						{
							try
							{
								this.Values.Clear();
								if (ManagementClassGenerator.isTypeInt(prop.Type) && qualifierData.Value != null)
								{
									ArrayList arrayList = new ArrayList(5);
									string[] array2 = (string[])qualifierData.Value;
									for (int j = 0; j < array2.Length; j++)
									{
										if (array2[j].Length == 0)
										{
											this.Values.Clear();
											flag3 = true;
											break;
										}
										string value = ManagementClassGenerator.ConvertValuesToName(array2[j]);
										arrayList.Add(value);
									}
									this.ResolveEnumNameValues(arrayList, ref this.Values);
								}
								continue;
							}
							catch (InvalidCastException)
							{
								this.Values.Clear();
								continue;
							}
						}
						if (string.Compare(qualifierData.Name, "BitMap", StringComparison.OrdinalIgnoreCase) == 0 && !flag3)
						{
							try
							{
								this.BitMap.Clear();
								if (ManagementClassGenerator.isTypeInt(prop.Type) && qualifierData.Value != null)
								{
									string[] array3 = (string[])qualifierData.Value;
									for (int k = 0; k < array3.Length; k++)
									{
										this.BitMap.Add(ManagementClassGenerator.ConvertBitMapValueToInt32(array3[k]));
									}
								}
								continue;
							}
							catch (FormatException)
							{
								this.BitMap.Clear();
								flag3 = true;
								continue;
							}
							catch (InvalidCastException)
							{
								this.BitMap.Clear();
								continue;
							}
						}
						if (string.Compare(qualifierData.Name, "BitValues", StringComparison.OrdinalIgnoreCase) == 0 && !flag3)
						{
							try
							{
								this.BitValues.Clear();
								if (ManagementClassGenerator.isTypeInt(prop.Type) && qualifierData.Value != null)
								{
									ArrayList arrayList2 = new ArrayList(5);
									string[] array4 = (string[])qualifierData.Value;
									for (int l = 0; l < array4.Length; l++)
									{
										if (array4[l].Length == 0)
										{
											this.BitValues.Clear();
											flag3 = true;
											break;
										}
										string value2 = ManagementClassGenerator.ConvertValuesToName(array4[l]);
										arrayList2.Add(value2);
									}
									this.ResolveEnumNameValues(arrayList2, ref this.BitValues);
								}
							}
							catch (InvalidCastException)
							{
								this.BitValues.Clear();
							}
						}
					}
				}
			}
			if ((!bDynamicClass && !flag) || (!bDynamicClass && flag && flag2) || (bDynamicClass && flag && flag2))
			{
				bWrite = true;
			}
			return result;
		}

		// Token: 0x060002FF RID: 767 RVA: 0x00013BBC File Offset: 0x00011DBC
		private bool GeneratePropertyHelperEnums(PropertyData prop, string strPropertyName, bool bNullable)
		{
			bool result = false;
			bool flag = false;
			string text = this.ResolveCollision(strPropertyName + "Values", true);
			if (this.Values.Count > 0 && (this.ValueMap.Count == 0 || this.ValueMap.Count == this.Values.Count))
			{
				if (this.ValueMap.Count == 0)
				{
					flag = true;
				}
				this.EnumObj = new CodeTypeDeclaration(text);
				if (prop.IsArray)
				{
					this.cmp.Type = new CodeTypeReference(text, 1);
				}
				else
				{
					this.cmp.Type = new CodeTypeReference(text);
				}
				this.EnumObj.IsEnum = true;
				this.EnumObj.TypeAttributes = TypeAttributes.Public;
				long num = 0L;
				for (int i = 0; i < this.Values.Count; i++)
				{
					this.cmf = new CodeMemberField();
					this.cmf.Name = this.Values[i].ToString();
					if (this.ValueMap.Count > 0)
					{
						this.cmf.InitExpression = new CodePrimitiveExpression(this.ValueMap[i]);
						long num2 = System.Convert.ToInt64(this.ValueMap[i], (IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(ulong)));
						if (num2 > num)
						{
							num = num2;
						}
						if (!flag && System.Convert.ToInt64(this.ValueMap[i], (IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(ulong))) == 0L)
						{
							flag = true;
						}
					}
					else
					{
						this.cmf.InitExpression = new CodePrimitiveExpression(i);
						if ((long)i > num)
						{
							num = (long)i;
						}
					}
					this.EnumObj.Members.Add(this.cmf);
				}
				if (bNullable && !flag)
				{
					this.cmf = new CodeMemberField();
					this.cmf.Name = "NULL_ENUM_VALUE";
					this.cmf.InitExpression = new CodePrimitiveExpression(0);
					this.EnumObj.Members.Add(this.cmf);
					prop.NullEnumValue = 0L;
				}
				else if (bNullable && flag)
				{
					this.cmf = new CodeMemberField();
					this.cmf.Name = "NULL_ENUM_VALUE";
					this.cmf.InitExpression = new CodePrimitiveExpression((int)(num + 1L));
					this.EnumObj.Members.Add(this.cmf);
					prop.NullEnumValue = (long)((int)(num + 1L));
				}
				else if (!bNullable && !flag)
				{
					this.cmf = new CodeMemberField();
					this.cmf.Name = "INVALID_ENUM_VALUE";
					this.cmf.InitExpression = new CodePrimitiveExpression(0);
					this.EnumObj.Members.Add(this.cmf);
					prop.NullEnumValue = 0L;
				}
				this.cc.Members.Add(this.EnumObj);
				result = true;
			}
			this.Values.Clear();
			this.ValueMap.Clear();
			flag = false;
			if (this.BitValues.Count > 0 && (this.BitMap.Count == 0 || this.BitMap.Count == this.BitValues.Count))
			{
				if (this.BitMap.Count == 0)
				{
					flag = true;
				}
				this.EnumObj = new CodeTypeDeclaration(text);
				if (prop.IsArray)
				{
					this.cmp.Type = new CodeTypeReference(text, 1);
				}
				else
				{
					this.cmp.Type = new CodeTypeReference(text);
				}
				this.EnumObj.IsEnum = true;
				this.EnumObj.TypeAttributes = TypeAttributes.Public;
				int num3 = 1;
				long num4 = 0L;
				for (int j = 0; j < this.BitValues.Count; j++)
				{
					this.cmf = new CodeMemberField();
					this.cmf.Name = this.BitValues[j].ToString();
					if (this.BitMap.Count > 0)
					{
						this.cmf.InitExpression = new CodePrimitiveExpression(this.BitMap[j]);
						long num5 = System.Convert.ToInt64(this.BitMap[j], (IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(ulong)));
						if (num5 > num4)
						{
							num4 = num5;
						}
					}
					else
					{
						this.cmf.InitExpression = new CodePrimitiveExpression(num3);
						if ((long)num3 > num4)
						{
							num4 = (long)num3;
						}
						num3 <<= 1;
					}
					if (!flag && System.Convert.ToInt64(this.BitMap[j], (IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(ulong))) == 0L)
					{
						flag = true;
					}
					this.EnumObj.Members.Add(this.cmf);
				}
				if (bNullable && !flag)
				{
					this.cmf = new CodeMemberField();
					this.cmf.Name = "NULL_ENUM_VALUE";
					this.cmf.InitExpression = new CodePrimitiveExpression(0);
					this.EnumObj.Members.Add(this.cmf);
					prop.NullEnumValue = 0L;
				}
				else if (bNullable && flag)
				{
					this.cmf = new CodeMemberField();
					this.cmf.Name = "NULL_ENUM_VALUE";
					if (this.BitValues.Count > 30)
					{
						num4 += 1L;
					}
					else
					{
						num4 <<= 1;
					}
					this.cmf.InitExpression = new CodePrimitiveExpression((int)num4);
					this.EnumObj.Members.Add(this.cmf);
					prop.NullEnumValue = (long)((int)num4);
				}
				else if (!bNullable && !flag)
				{
					this.cmf = new CodeMemberField();
					this.cmf.Name = "INVALID_ENUM_VALUE";
					this.cmf.InitExpression = new CodePrimitiveExpression(0);
					this.EnumObj.Members.Add(this.cmf);
					prop.NullEnumValue = 0L;
				}
				this.cc.Members.Add(this.EnumObj);
				result = true;
			}
			this.BitValues.Clear();
			this.BitMap.Clear();
			return result;
		}

		// Token: 0x06000300 RID: 768 RVA: 0x000141EC File Offset: 0x000123EC
		private void GenerateConstructPath()
		{
			this.cmm = new CodeMemberMethod();
			this.cmm.Name = this.PublicNamesUsed["ConstructPathFunction"].ToString();
			this.cmm.Attributes = (MemberAttributes)20483;
			this.cmm.ReturnType = new CodeTypeReference("System.String");
			for (int i = 0; i < this.arrKeys.Count; i++)
			{
				string baseType = ((CodeTypeReference)this.arrKeyType[i]).BaseType;
				this.cmm.Parameters.Add(new CodeParameterDeclarationExpression(baseType, "key" + this.arrKeys[i].ToString()));
			}
			string text = this.OriginalNamespace + ":" + this.OriginalClassName;
			if (this.bSingletonClass)
			{
				text += "=@";
				this.cmm.Statements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(text)));
			}
			else
			{
				string text2 = "strPath";
				this.cmm.Statements.Add(new CodeVariableDeclarationStatement("System.String", text2, new CodePrimitiveExpression(text)));
				for (int j = 0; j < this.arrKeys.Count; j++)
				{
					CodeMethodInvokeExpression right;
					if (((CodeTypeReference)this.arrKeyType[j]).BaseType == "System.String")
					{
						CodeMethodInvokeExpression ce = ManagementClassGenerator.GenerateConcatStrings(new CodeVariableReferenceExpression("key" + this.arrKeys[j]), new CodePrimitiveExpression("\""));
						CodeMethodInvokeExpression ce2 = ManagementClassGenerator.GenerateConcatStrings(new CodePrimitiveExpression("\""), ce);
						CodeMethodInvokeExpression ce3 = ManagementClassGenerator.GenerateConcatStrings(new CodePrimitiveExpression((j == 0) ? ("." + this.arrKeys[j] + "=") : ("," + this.arrKeys[j] + "=")), ce2);
						right = ManagementClassGenerator.GenerateConcatStrings(new CodeVariableReferenceExpression(text2), ce3);
					}
					else
					{
						this.cmie = new CodeMethodInvokeExpression();
						this.cmie.Method.TargetObject = new CodeCastExpression(new CodeTypeReference(((CodeTypeReference)this.arrKeyType[j]).BaseType + " "), new CodeVariableReferenceExpression("key" + this.arrKeys[j]));
						this.cmie.Method.MethodName = "ToString";
						CodeMethodInvokeExpression ce4 = ManagementClassGenerator.GenerateConcatStrings(new CodePrimitiveExpression((j == 0) ? ("." + this.arrKeys[j] + "=") : ("," + this.arrKeys[j] + "=")), this.cmie);
						right = ManagementClassGenerator.GenerateConcatStrings(new CodeVariableReferenceExpression(text2), ce4);
					}
					this.cmm.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(text2), right));
				}
				this.cmm.Statements.Add(new CodeMethodReturnStatement(new CodeVariableReferenceExpression(text2)));
			}
			this.cc.Members.Add(this.cmm);
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0001452C File Offset: 0x0001272C
		private void GenerateDefaultConstructor()
		{
			this.cctor = new CodeConstructor();
			this.cctor.Attributes = MemberAttributes.Public;
			CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method.MethodName = this.PrivateNamesUsed["InitialObjectFunc"].ToString();
			codeMethodInvokeExpression.Method.TargetObject = new CodeThisReferenceExpression();
			codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(null));
			if (this.bSingletonClass)
			{
				this.cmie = new CodeMethodInvokeExpression();
				this.cmie.Method.TargetObject = new CodeTypeReferenceExpression(this.PrivateNamesUsed["GeneratedClassName"].ToString());
				this.cmie.Method.MethodName = this.PublicNamesUsed["ConstructPathFunction"].ToString();
				this.coce = new CodeObjectCreateExpression();
				this.coce.CreateType = new CodeTypeReference(this.PublicNamesUsed["PathClass"].ToString());
				this.coce.Parameters.Add(this.cmie);
				codeMethodInvokeExpression.Parameters.Add(this.coce);
			}
			else
			{
				codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(null));
			}
			codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(null));
			this.cctor.Statements.Add(new CodeExpressionStatement(codeMethodInvokeExpression));
			this.cc.Members.Add(this.cctor);
			this.cctor.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("COMMENT_CONSTRUCTORS")));
		}

		// Token: 0x06000302 RID: 770 RVA: 0x000146D0 File Offset: 0x000128D0
		private void GenerateConstructorWithKeys()
		{
			if (this.arrKeyType.Count > 0)
			{
				this.cctor = new CodeConstructor();
				this.cctor.Attributes = MemberAttributes.Public;
				CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression();
				codeMethodInvokeExpression.Method.MethodName = this.PrivateNamesUsed["InitialObjectFunc"].ToString();
				codeMethodInvokeExpression.Method.TargetObject = new CodeThisReferenceExpression();
				for (int i = 0; i < this.arrKeys.Count; i++)
				{
					this.cpde = new CodeParameterDeclarationExpression();
					this.cpde.Type = new CodeTypeReference(((CodeTypeReference)this.arrKeyType[i]).BaseType);
					this.cpde.Name = "key" + this.arrKeys[i].ToString();
					this.cctor.Parameters.Add(this.cpde);
				}
				if (this.cctor.Parameters.Count == 1 && this.cctor.Parameters[0].Type.BaseType == new CodeTypeReference(this.PublicNamesUsed["PathClass"].ToString()).BaseType)
				{
					this.cpde = new CodeParameterDeclarationExpression();
					this.cpde.Type = new CodeTypeReference("System.Object");
					this.cpde.Name = "dummyParam";
					this.cctor.Parameters.Add(this.cpde);
					this.cctor.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression("dummyParam"), new CodePrimitiveExpression(null)));
				}
				codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(null));
				this.cmie = new CodeMethodInvokeExpression();
				this.cmie.Method.TargetObject = new CodeTypeReferenceExpression(this.PrivateNamesUsed["GeneratedClassName"].ToString());
				this.cmie.Method.MethodName = this.PublicNamesUsed["ConstructPathFunction"].ToString();
				for (int j = 0; j < this.arrKeys.Count; j++)
				{
					this.cmie.Parameters.Add(new CodeVariableReferenceExpression("key" + this.arrKeys[j]));
				}
				this.coce = new CodeObjectCreateExpression();
				this.coce.CreateType = new CodeTypeReference(this.PublicNamesUsed["PathClass"].ToString());
				this.coce.Parameters.Add(this.cmie);
				codeMethodInvokeExpression.Parameters.Add(this.coce);
				codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(null));
				this.cctor.Statements.Add(new CodeExpressionStatement(codeMethodInvokeExpression));
				this.cc.Members.Add(this.cctor);
			}
		}

		// Token: 0x06000303 RID: 771 RVA: 0x000149D0 File Offset: 0x00012BD0
		private void GenerateConstructorWithScopeKeys()
		{
			this.cctor = new CodeConstructor();
			this.cctor.Attributes = MemberAttributes.Public;
			this.cctor.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(this.PublicNamesUsed["ScopeClass"].ToString()), this.PrivateNamesUsed["ScopeParam"].ToString()));
			CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method.MethodName = this.PrivateNamesUsed["InitialObjectFunc"].ToString();
			codeMethodInvokeExpression.Method.TargetObject = new CodeThisReferenceExpression();
			if (this.arrKeyType.Count > 0)
			{
				for (int i = 0; i < this.arrKeys.Count; i++)
				{
					this.cpde = new CodeParameterDeclarationExpression();
					this.cpde.Type = new CodeTypeReference(((CodeTypeReference)this.arrKeyType[i]).BaseType);
					this.cpde.Name = "key" + this.arrKeys[i].ToString();
					this.cctor.Parameters.Add(this.cpde);
				}
				if (this.cctor.Parameters.Count == 2 && this.cctor.Parameters[1].Type.BaseType == new CodeTypeReference(this.PublicNamesUsed["PathClass"].ToString()).BaseType)
				{
					this.cpde = new CodeParameterDeclarationExpression();
					this.cpde.Type = new CodeTypeReference("System.Object");
					this.cpde.Name = "dummyParam";
					this.cctor.Parameters.Add(this.cpde);
					this.cctor.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression("dummyParam"), new CodePrimitiveExpression(null)));
				}
				codeMethodInvokeExpression.Parameters.Add(new CodeCastExpression(new CodeTypeReference(this.PublicNamesUsed["ScopeClass"].ToString()), new CodeVariableReferenceExpression(this.PrivateNamesUsed["ScopeParam"].ToString())));
				this.cmie = new CodeMethodInvokeExpression();
				this.cmie.Method.TargetObject = new CodeTypeReferenceExpression(this.PrivateNamesUsed["GeneratedClassName"].ToString());
				this.cmie.Method.MethodName = this.PublicNamesUsed["ConstructPathFunction"].ToString();
				for (int j = 0; j < this.arrKeys.Count; j++)
				{
					this.cmie.Parameters.Add(new CodeVariableReferenceExpression("key" + this.arrKeys[j]));
				}
				this.coce = new CodeObjectCreateExpression();
				this.coce.CreateType = new CodeTypeReference(this.PublicNamesUsed["PathClass"].ToString());
				this.coce.Parameters.Add(this.cmie);
				codeMethodInvokeExpression.Parameters.Add(this.coce);
				codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(null));
				this.cctor.Statements.Add(new CodeExpressionStatement(codeMethodInvokeExpression));
				this.cc.Members.Add(this.cctor);
			}
		}

		// Token: 0x06000304 RID: 772 RVA: 0x00014D48 File Offset: 0x00012F48
		private void GenerateConstructorWithPath()
		{
			string text = "path";
			this.cctor = new CodeConstructor();
			this.cctor.Attributes = MemberAttributes.Public;
			this.cpde = new CodeParameterDeclarationExpression();
			this.cpde.Type = new CodeTypeReference(this.PublicNamesUsed["PathClass"].ToString());
			this.cpde.Name = text;
			this.cctor.Parameters.Add(this.cpde);
			CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method.MethodName = this.PrivateNamesUsed["InitialObjectFunc"].ToString();
			codeMethodInvokeExpression.Method.TargetObject = new CodeThisReferenceExpression();
			codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(null));
			codeMethodInvokeExpression.Parameters.Add(new CodeVariableReferenceExpression(text));
			codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(null));
			this.cctor.Statements.Add(new CodeExpressionStatement(codeMethodInvokeExpression));
			this.cc.Members.Add(this.cctor);
		}

		// Token: 0x06000305 RID: 773 RVA: 0x00014E64 File Offset: 0x00013064
		private void GenerateConstructorWithPathOptions()
		{
			string text = "path";
			string text2 = "getOptions";
			this.cctor = new CodeConstructor();
			this.cctor.Attributes = MemberAttributes.Public;
			this.cctor.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(this.PublicNamesUsed["PathClass"].ToString()), text));
			this.cctor.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(this.PublicNamesUsed["GetOptionsClass"].ToString()), text2));
			CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method.MethodName = this.PrivateNamesUsed["InitialObjectFunc"].ToString();
			codeMethodInvokeExpression.Method.TargetObject = new CodeThisReferenceExpression();
			codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(null));
			codeMethodInvokeExpression.Parameters.Add(new CodeVariableReferenceExpression(text));
			codeMethodInvokeExpression.Parameters.Add(new CodeVariableReferenceExpression(text2));
			this.cctor.Statements.Add(new CodeExpressionStatement(codeMethodInvokeExpression));
			this.cc.Members.Add(this.cctor);
		}

		// Token: 0x06000306 RID: 774 RVA: 0x00014F94 File Offset: 0x00013194
		private void GenerateConstructorWithScopePath()
		{
			string text = "path";
			this.cctor = new CodeConstructor();
			this.cctor.Attributes = MemberAttributes.Public;
			this.cctor.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(this.PublicNamesUsed["ScopeClass"].ToString()), this.PrivateNamesUsed["ScopeParam"].ToString()));
			this.cctor.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(this.PublicNamesUsed["PathClass"].ToString()), text));
			CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method.MethodName = this.PrivateNamesUsed["InitialObjectFunc"].ToString();
			codeMethodInvokeExpression.Method.TargetObject = new CodeThisReferenceExpression();
			codeMethodInvokeExpression.Parameters.Add(new CodeVariableReferenceExpression(this.PrivateNamesUsed["ScopeParam"].ToString()));
			codeMethodInvokeExpression.Parameters.Add(new CodeVariableReferenceExpression(text));
			codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(null));
			this.cctor.Statements.Add(new CodeExpressionStatement(codeMethodInvokeExpression));
			this.cc.Members.Add(this.cctor);
		}

		// Token: 0x06000307 RID: 775 RVA: 0x000150E8 File Offset: 0x000132E8
		private void GenerateConstructorWithScope()
		{
			this.cctor = new CodeConstructor();
			this.cctor.Attributes = MemberAttributes.Public;
			this.cctor.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(this.PublicNamesUsed["ScopeClass"].ToString()), this.PrivateNamesUsed["ScopeParam"].ToString()));
			CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method.MethodName = this.PrivateNamesUsed["InitialObjectFunc"].ToString();
			codeMethodInvokeExpression.Method.TargetObject = new CodeThisReferenceExpression();
			codeMethodInvokeExpression.Parameters.Add(new CodeVariableReferenceExpression(this.PrivateNamesUsed["ScopeParam"].ToString()));
			this.cmie = new CodeMethodInvokeExpression();
			this.cmie.Method.TargetObject = new CodeTypeReferenceExpression(this.PrivateNamesUsed["GeneratedClassName"].ToString());
			this.cmie.Method.MethodName = this.PublicNamesUsed["ConstructPathFunction"].ToString();
			this.coce = new CodeObjectCreateExpression();
			this.coce.CreateType = new CodeTypeReference(this.PublicNamesUsed["PathClass"].ToString());
			this.coce.Parameters.Add(this.cmie);
			codeMethodInvokeExpression.Parameters.Add(this.coce);
			codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(null));
			this.cctor.Statements.Add(new CodeExpressionStatement(codeMethodInvokeExpression));
			this.cc.Members.Add(this.cctor);
		}

		// Token: 0x06000308 RID: 776 RVA: 0x000152A4 File Offset: 0x000134A4
		private void GenerateConstructorWithOptions()
		{
			string text = "getOptions";
			this.cctor = new CodeConstructor();
			this.cctor.Attributes = MemberAttributes.Public;
			this.cctor.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(this.PublicNamesUsed["GetOptionsClass"].ToString()), text));
			CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method.MethodName = this.PrivateNamesUsed["InitialObjectFunc"].ToString();
			codeMethodInvokeExpression.Method.TargetObject = new CodeThisReferenceExpression();
			codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(null));
			this.cmie = new CodeMethodInvokeExpression();
			this.cmie.Method.TargetObject = new CodeTypeReferenceExpression(this.PrivateNamesUsed["GeneratedClassName"].ToString());
			this.cmie.Method.MethodName = this.PublicNamesUsed["ConstructPathFunction"].ToString();
			this.coce = new CodeObjectCreateExpression();
			this.coce.CreateType = new CodeTypeReference(this.PublicNamesUsed["PathClass"].ToString());
			this.coce.Parameters.Add(this.cmie);
			codeMethodInvokeExpression.Parameters.Add(this.coce);
			codeMethodInvokeExpression.Parameters.Add(new CodeVariableReferenceExpression(text));
			this.cctor.Statements.Add(new CodeExpressionStatement(codeMethodInvokeExpression));
			this.cc.Members.Add(this.cctor);
		}

		// Token: 0x06000309 RID: 777 RVA: 0x00015440 File Offset: 0x00013640
		private void GenerateConstructorWithScopeOptions()
		{
			string text = "getOptions";
			this.cctor = new CodeConstructor();
			this.cctor.Attributes = MemberAttributes.Public;
			this.cctor.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(this.PublicNamesUsed["ScopeClass"].ToString()), this.PrivateNamesUsed["ScopeParam"].ToString()));
			this.cctor.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(this.PublicNamesUsed["GetOptionsClass"].ToString()), text));
			CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method.MethodName = this.PrivateNamesUsed["InitialObjectFunc"].ToString();
			codeMethodInvokeExpression.Method.TargetObject = new CodeThisReferenceExpression();
			codeMethodInvokeExpression.Parameters.Add(new CodeVariableReferenceExpression(this.PrivateNamesUsed["ScopeParam"].ToString()));
			this.cmie = new CodeMethodInvokeExpression();
			this.cmie.Method.TargetObject = new CodeTypeReferenceExpression(this.PrivateNamesUsed["GeneratedClassName"].ToString());
			this.cmie.Method.MethodName = this.PublicNamesUsed["ConstructPathFunction"].ToString();
			this.coce = new CodeObjectCreateExpression();
			this.coce.CreateType = new CodeTypeReference(this.PublicNamesUsed["PathClass"].ToString());
			this.coce.Parameters.Add(this.cmie);
			codeMethodInvokeExpression.Parameters.Add(this.coce);
			codeMethodInvokeExpression.Parameters.Add(new CodeVariableReferenceExpression(text));
			this.cctor.Statements.Add(new CodeExpressionStatement(codeMethodInvokeExpression));
			this.cc.Members.Add(this.cctor);
		}

		// Token: 0x0600030A RID: 778 RVA: 0x00015634 File Offset: 0x00013834
		private void GenerateConstructorWithScopePathOptions()
		{
			string text = "path";
			string text2 = "getOptions";
			this.cctor = new CodeConstructor();
			this.cctor.Attributes = MemberAttributes.Public;
			this.cctor.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(this.PublicNamesUsed["ScopeClass"].ToString()), this.PrivateNamesUsed["ScopeParam"].ToString()));
			this.cctor.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(this.PublicNamesUsed["PathClass"].ToString()), text));
			this.cctor.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(this.PublicNamesUsed["GetOptionsClass"].ToString()), text2));
			CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method.MethodName = this.PrivateNamesUsed["InitialObjectFunc"].ToString();
			codeMethodInvokeExpression.Method.TargetObject = new CodeThisReferenceExpression();
			codeMethodInvokeExpression.Parameters.Add(new CodeVariableReferenceExpression(this.PrivateNamesUsed["ScopeParam"].ToString()));
			codeMethodInvokeExpression.Parameters.Add(new CodeVariableReferenceExpression(text));
			codeMethodInvokeExpression.Parameters.Add(new CodeVariableReferenceExpression(text2));
			this.cctor.Statements.Add(new CodeExpressionStatement(codeMethodInvokeExpression));
			this.cc.Members.Add(this.cctor);
		}

		// Token: 0x0600030B RID: 779 RVA: 0x000157C0 File Offset: 0x000139C0
		private void GenarateConstructorWithLateBound()
		{
			string text = "theObject";
			string propertyName = "SystemProperties";
			this.cctor = new CodeConstructor();
			this.cctor.Attributes = MemberAttributes.Public;
			this.cpde = new CodeParameterDeclarationExpression();
			this.cpde.Type = new CodeTypeReference(this.PublicNamesUsed["LateBoundClass"].ToString());
			this.cpde.Name = text;
			this.cctor.Parameters.Add(this.cpde);
			this.InitPrivateMemberVariables(this.cctor);
			this.cis = new CodeConditionStatement();
			this.cpre = new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text), propertyName);
			this.cie = new CodeIndexerExpression(this.cpre, new CodeExpression[]
			{
				new CodePrimitiveExpression("__CLASS")
			});
			this.cpre = new CodePropertyReferenceExpression(this.cie, "Value");
			this.cmie = new CodeMethodInvokeExpression();
			this.cmie.Method.MethodName = this.PrivateNamesUsed["ClassNameCheckFunc"].ToString();
			this.cmie.Parameters.Add(new CodeVariableReferenceExpression(text));
			this.cboe = new CodeBinaryOperatorExpression();
			this.cboe.Left = this.cmie;
			this.cboe.Right = new CodePrimitiveExpression(true);
			this.cboe.Operator = CodeBinaryOperatorType.ValueEquality;
			this.cis.Condition = this.cboe;
			this.cis.TrueStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(this.PrivateNamesUsed["LateBoundObject"].ToString()), new CodeVariableReferenceExpression(text)));
			this.coce = new CodeObjectCreateExpression();
			this.coce.CreateType = new CodeTypeReference(this.PublicNamesUsed["SystemPropertiesClass"].ToString());
			this.coce.Parameters.Add(new CodeVariableReferenceExpression(this.PrivateNamesUsed["LateBoundObject"].ToString()));
			this.cis.TrueStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(this.PrivateNamesUsed["SystemPropertiesObject"].ToString()), this.coce));
			this.cis.TrueStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(this.PrivateNamesUsed["CurrentObject"].ToString()), new CodeVariableReferenceExpression(this.PrivateNamesUsed["LateBoundObject"].ToString())));
			this.coce = new CodeObjectCreateExpression();
			this.coce.CreateType = new CodeTypeReference(this.PublicNamesUsed["ArgumentExceptionClass"].ToString());
			this.coce.Parameters.Add(new CodePrimitiveExpression(ManagementClassGenerator.GetString("CLASSNOT_FOUND_EXCEPT")));
			this.cis.FalseStatements.Add(new CodeThrowExceptionStatement(this.coce));
			this.cctor.Statements.Add(this.cis);
			this.cc.Members.Add(this.cctor);
		}

		// Token: 0x0600030C RID: 780 RVA: 0x00015AEC File Offset: 0x00013CEC
		private void GenarateConstructorWithLateBoundForEmbedded()
		{
			string text = "theObject";
			this.cctor = new CodeConstructor();
			this.cctor.Attributes = MemberAttributes.Public;
			this.cpde = new CodeParameterDeclarationExpression();
			this.cpde.Type = new CodeTypeReference(this.PublicNamesUsed["BaseObjClass"].ToString());
			this.cpde.Name = text;
			this.cctor.Parameters.Add(this.cpde);
			this.InitPrivateMemberVariables(this.cctor);
			this.cmie = new CodeMethodInvokeExpression();
			this.cmie.Method.MethodName = this.PrivateNamesUsed["ClassNameCheckFunc"].ToString();
			this.cmie.Parameters.Add(new CodeVariableReferenceExpression(text));
			this.cboe = new CodeBinaryOperatorExpression();
			this.cboe.Left = this.cmie;
			this.cboe.Right = new CodePrimitiveExpression(true);
			this.cboe.Operator = CodeBinaryOperatorType.ValueEquality;
			this.cis = new CodeConditionStatement();
			this.cis.Condition = this.cboe;
			this.cis.TrueStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(this.PrivateNamesUsed["EmbeddedObject"].ToString()), new CodeVariableReferenceExpression(text)));
			this.coce = new CodeObjectCreateExpression();
			this.coce.CreateType = new CodeTypeReference(this.PublicNamesUsed["SystemPropertiesClass"].ToString());
			this.coce.Parameters.Add(new CodeVariableReferenceExpression(text));
			this.cis.TrueStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(this.PrivateNamesUsed["SystemPropertiesObject"].ToString()), this.coce));
			this.cis.TrueStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(this.PrivateNamesUsed["CurrentObject"].ToString()), new CodeVariableReferenceExpression(this.PrivateNamesUsed["EmbeddedObject"].ToString())));
			this.cis.TrueStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(this.PrivateNamesUsed["IsEmbedded"].ToString()), new CodePrimitiveExpression(true)));
			this.coce = new CodeObjectCreateExpression();
			this.coce.CreateType = new CodeTypeReference(this.PublicNamesUsed["ArgumentExceptionClass"].ToString());
			this.coce.Parameters.Add(new CodePrimitiveExpression(ManagementClassGenerator.GetString("CLASSNOT_FOUND_EXCEPT")));
			this.cis.FalseStatements.Add(new CodeThrowExceptionStatement(this.coce));
			this.cctor.Statements.Add(this.cis);
			this.cc.Members.Add(this.cctor);
		}

		// Token: 0x0600030D RID: 781 RVA: 0x00015DEC File Offset: 0x00013FEC
		private void GenerateInitializeObject()
		{
			string text = "path";
			string text2 = "getOptions";
			bool flag = true;
			try
			{
				this.classobj.Qualifiers["priveleges"].ToString();
			}
			catch (ManagementException ex)
			{
				if (ex.ErrorCode != ManagementStatus.NotFound)
				{
					throw;
				}
				flag = false;
			}
			CodeMemberMethod codeMemberMethod = new CodeMemberMethod();
			codeMemberMethod.Name = this.PrivateNamesUsed["InitialObjectFunc"].ToString();
			codeMemberMethod.Attributes = (MemberAttributes)20482;
			codeMemberMethod.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(this.PublicNamesUsed["ScopeClass"].ToString()), this.PrivateNamesUsed["ScopeParam"].ToString()));
			codeMemberMethod.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(this.PublicNamesUsed["PathClass"].ToString()), text));
			codeMemberMethod.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(this.PublicNamesUsed["GetOptionsClass"].ToString()), text2));
			this.InitPrivateMemberVariables(codeMemberMethod);
			this.cis = new CodeConditionStatement();
			this.cis.Condition = new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression(text), CodeBinaryOperatorType.IdentityInequality, new CodePrimitiveExpression(null));
			CodeConditionStatement codeConditionStatement = new CodeConditionStatement();
			this.cmie = new CodeMethodInvokeExpression();
			this.cmie.Method.MethodName = this.PrivateNamesUsed["ClassNameCheckFunc"].ToString();
			this.cmie.Parameters.Add(new CodeVariableReferenceExpression(this.PrivateNamesUsed["ScopeParam"].ToString()));
			this.cmie.Parameters.Add(new CodeVariableReferenceExpression(text));
			this.cmie.Parameters.Add(new CodeVariableReferenceExpression(text2));
			this.cboe = new CodeBinaryOperatorExpression();
			this.cboe.Left = this.cmie;
			this.cboe.Right = new CodePrimitiveExpression(true);
			this.cboe.Operator = CodeBinaryOperatorType.IdentityInequality;
			codeConditionStatement.Condition = this.cboe;
			this.coce = new CodeObjectCreateExpression();
			this.coce.CreateType = new CodeTypeReference(this.PublicNamesUsed["ArgumentExceptionClass"].ToString());
			this.coce.Parameters.Add(new CodePrimitiveExpression(ManagementClassGenerator.GetString("CLASSNOT_FOUND_EXCEPT")));
			codeConditionStatement.TrueStatements.Add(new CodeThrowExceptionStatement(this.coce));
			this.cis.TrueStatements.Add(codeConditionStatement);
			codeMemberMethod.Statements.Add(this.cis);
			this.coce = new CodeObjectCreateExpression();
			this.coce.CreateType = new CodeTypeReference(this.PublicNamesUsed["LateBoundClass"].ToString());
			this.coce.Parameters.Add(new CodeVariableReferenceExpression(this.PrivateNamesUsed["ScopeParam"].ToString()));
			this.coce.Parameters.Add(new CodeVariableReferenceExpression(text));
			this.coce.Parameters.Add(new CodeVariableReferenceExpression(text2));
			codeMemberMethod.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(this.PrivateNamesUsed["LateBoundObject"].ToString()), this.coce));
			this.coce = new CodeObjectCreateExpression();
			this.coce.CreateType = new CodeTypeReference(this.PublicNamesUsed["SystemPropertiesClass"].ToString());
			this.coce.Parameters.Add(new CodeVariableReferenceExpression(this.PrivateNamesUsed["LateBoundObject"].ToString()));
			codeMemberMethod.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(this.PrivateNamesUsed["SystemPropertiesObject"].ToString()), this.coce));
			codeMemberMethod.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(this.PrivateNamesUsed["CurrentObject"].ToString()), new CodeVariableReferenceExpression(this.PrivateNamesUsed["LateBoundObject"].ToString())));
			this.cc.Members.Add(codeMemberMethod);
			if (flag)
			{
				this.cpre = new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(this.PrivateNamesUsed["LateBoundObject"].ToString()), this.PublicNamesUsed["ScopeProperty"].ToString()), "Options"), "EnablePrivileges");
				this.cctor.Statements.Add(new CodeAssignStatement(this.cpre, new CodePrimitiveExpression(true)));
			}
		}

		// Token: 0x0600030E RID: 782 RVA: 0x000162BC File Offset: 0x000144BC
		private void GenerateMethods()
		{
			string text = "inParams";
			string text2 = "outParams";
			string text3 = "classObj";
			bool flag = false;
			bool flag2 = false;
			CodePropertyReferenceExpression codePropertyReferenceExpression = null;
			CimType cimType = CimType.SInt8;
			CodeTypeReference codeTypeReference = null;
			bool bArray = false;
			ArrayList arrayList = new ArrayList(5);
			ArrayList arrayList2 = new ArrayList(5);
			ArrayList arrayList3 = new ArrayList(5);
			for (int i = 0; i < this.PublicMethods.Count; i++)
			{
				flag = false;
				MethodData methodData = this.classobj.Methods[this.PublicMethods.GetKey(i).ToString()];
				string variableName = this.PrivateNamesUsed["LateBoundObject"].ToString();
				if (methodData.OutParameters != null && methodData.OutParameters.Properties != null)
				{
					foreach (PropertyData propertyData in methodData.OutParameters.Properties)
					{
						arrayList.Add(propertyData.Name);
					}
				}
				this.cmm = new CodeMemberMethod();
				this.cmm.Attributes = (MemberAttributes)24578;
				this.cmm.Name = this.PublicMethods[methodData.Name].ToString();
				foreach (QualifierData qualifierData in methodData.Qualifiers)
				{
					if (string.Compare(qualifierData.Name, "static", StringComparison.OrdinalIgnoreCase) == 0)
					{
						this.cmm.Attributes |= MemberAttributes.Static;
						flag = true;
						break;
					}
					if (string.Compare(qualifierData.Name, "privileges", StringComparison.OrdinalIgnoreCase) == 0)
					{
						flag2 = true;
					}
				}
				this.cis = new CodeConditionStatement();
				this.cboe = new CodeBinaryOperatorExpression();
				if (flag)
				{
					this.cmm.Statements.Add(new CodeVariableDeclarationStatement("System.Boolean", "IsMethodStatic", new CodePrimitiveExpression(flag)));
					this.cboe.Left = new CodeVariableReferenceExpression("IsMethodStatic");
					this.cboe.Right = new CodePrimitiveExpression(true);
				}
				else
				{
					this.cboe.Left = new CodeVariableReferenceExpression(this.PrivateNamesUsed["IsEmbedded"].ToString());
					this.cboe.Right = new CodePrimitiveExpression(false);
				}
				this.cboe.Operator = CodeBinaryOperatorType.ValueEquality;
				this.cis.Condition = this.cboe;
				bool flag3 = true;
				this.cis.TrueStatements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference(this.PublicNamesUsed["BaseObjClass"].ToString()), text, new CodePrimitiveExpression(null)));
				if (flag)
				{
					string text4 = "mgmtPath";
					CodeObjectCreateExpression codeObjectCreateExpression = new CodeObjectCreateExpression();
					codeObjectCreateExpression.CreateType = new CodeTypeReference(this.PublicNamesUsed["PathClass"].ToString());
					codeObjectCreateExpression.Parameters.Add(new CodeVariableReferenceExpression(this.PrivateNamesUsed["CreationClassName"].ToString()));
					this.cis.TrueStatements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference(this.PublicNamesUsed["PathClass"].ToString()), text4, codeObjectCreateExpression));
					CodeObjectCreateExpression codeObjectCreateExpression2 = new CodeObjectCreateExpression();
					codeObjectCreateExpression2.CreateType = new CodeTypeReference(this.PublicNamesUsed["ManagementClass"].ToString());
					codeObjectCreateExpression2.Parameters.Add(new CodeVariableReferenceExpression(this.PrivateNamesUsed["statMgmtScope"].ToString()));
					codeObjectCreateExpression2.Parameters.Add(new CodeVariableReferenceExpression(text4));
					codeObjectCreateExpression2.Parameters.Add(new CodePrimitiveExpression(null));
					this.coce = new CodeObjectCreateExpression();
					this.coce.CreateType = new CodeTypeReference(this.PublicNamesUsed["ManagementClass"].ToString());
					this.coce.Parameters.Add(codeObjectCreateExpression2);
					this.cis.TrueStatements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference(this.PublicNamesUsed["ManagementClass"].ToString()), text3, codeObjectCreateExpression2));
					variableName = text3;
				}
				if (flag2)
				{
					codePropertyReferenceExpression = new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(flag ? text3 : this.PrivateNamesUsed["LateBoundObject"].ToString()), this.PublicNamesUsed["ScopeProperty"].ToString()), "Options"), "EnablePrivileges");
					this.cis.TrueStatements.Add(new CodeVariableDeclarationStatement("System.Boolean", this.PrivateNamesUsed["Privileges"].ToString(), codePropertyReferenceExpression));
					this.cis.TrueStatements.Add(new CodeAssignStatement(codePropertyReferenceExpression, new CodePrimitiveExpression(true)));
				}
				if (methodData.InParameters != null && methodData.InParameters.Properties != null)
				{
					foreach (PropertyData propertyData2 in methodData.InParameters.Properties)
					{
						bool flag4 = false;
						if (flag3)
						{
							this.cmie = new CodeMethodInvokeExpression(new CodeVariableReferenceExpression(variableName), "GetMethodParameters", new CodeExpression[]
							{
								new CodePrimitiveExpression(methodData.Name)
							});
							this.cis.TrueStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(text), this.cmie));
							flag3 = false;
						}
						this.cpde = new CodeParameterDeclarationExpression();
						this.cpde.Name = propertyData2.Name;
						this.cpde.Type = this.ConvertCIMType(propertyData2.Type, propertyData2.IsArray);
						this.cpde.Direction = FieldDirection.In;
						if (propertyData2.Type == CimType.DateTime)
						{
							CodeTypeReference type = this.cpde.Type;
							flag4 = this.GetDateTimeType(propertyData2, ref type);
							this.cpde.Type = type;
						}
						for (int j = 0; j < arrayList.Count; j++)
						{
							if (string.Compare(propertyData2.Name, arrayList[j].ToString(), StringComparison.OrdinalIgnoreCase) == 0)
							{
								this.cpde.Direction = FieldDirection.Ref;
								arrayList2.Add(propertyData2.Name);
								arrayList3.Add(this.cpde.Type);
							}
						}
						this.cmm.Parameters.Add(this.cpde);
						this.cie = new CodeIndexerExpression(new CodeVariableReferenceExpression(text), new CodeExpression[]
						{
							new CodePrimitiveExpression(propertyData2.Name)
						});
						if (propertyData2.Type == CimType.Reference)
						{
							this.AddPropertySet(this.cie, propertyData2.IsArray, this.cis.TrueStatements, this.PublicNamesUsed["PathClass"].ToString(), new CodeVariableReferenceExpression(this.cpde.Name));
						}
						else if (propertyData2.Type == CimType.DateTime)
						{
							if (flag4)
							{
								this.AddPropertySet(this.cie, propertyData2.IsArray, this.cis.TrueStatements, "System.TimeSpan", new CodeVariableReferenceExpression(this.cpde.Name));
							}
							else
							{
								this.AddPropertySet(this.cie, propertyData2.IsArray, this.cis.TrueStatements, "System.DateTime", new CodeVariableReferenceExpression(this.cpde.Name));
							}
						}
						else if (this.cpde.Type.ArrayRank == 0)
						{
							this.cis.TrueStatements.Add(new CodeAssignStatement(this.cie, new CodeCastExpression(new CodeTypeReference(this.cpde.Type.BaseType + " "), new CodeVariableReferenceExpression(this.cpde.Name))));
						}
						else
						{
							this.cis.TrueStatements.Add(new CodeAssignStatement(this.cie, new CodeCastExpression(this.cpde.Type, new CodeVariableReferenceExpression(this.cpde.Name))));
						}
					}
				}
				arrayList.Clear();
				bool flag5 = false;
				flag3 = true;
				bool flag6 = false;
				if (methodData.OutParameters != null && methodData.OutParameters.Properties != null)
				{
					foreach (PropertyData propertyData3 in methodData.OutParameters.Properties)
					{
						bool flag4 = false;
						if (flag3)
						{
							this.cmie = new CodeMethodInvokeExpression(new CodeVariableReferenceExpression(variableName), "InvokeMethod", new CodeExpression[0]);
							this.cmie.Parameters.Add(new CodePrimitiveExpression(methodData.Name));
							this.cmie.Parameters.Add(new CodeVariableReferenceExpression(text));
							this.cmie.Parameters.Add(new CodePrimitiveExpression(null));
							this.cis.TrueStatements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference(this.PublicNamesUsed["BaseObjClass"].ToString()), text2, this.cmie));
							flag3 = false;
							flag6 = true;
						}
						bool flag7 = false;
						for (int k = 0; k < arrayList2.Count; k++)
						{
							if (string.Compare(propertyData3.Name, arrayList2[k].ToString(), StringComparison.OrdinalIgnoreCase) == 0)
							{
								flag7 = true;
							}
						}
						if (!flag7)
						{
							if (string.Compare(propertyData3.Name, "ReturnValue", StringComparison.OrdinalIgnoreCase) == 0)
							{
								this.cmm.ReturnType = this.ConvertCIMType(propertyData3.Type, propertyData3.IsArray);
								flag5 = true;
								cimType = propertyData3.Type;
								if (propertyData3.Type == CimType.DateTime)
								{
									CodeTypeReference returnType = this.cmm.ReturnType;
									bool dateTimeType = this.GetDateTimeType(propertyData3, ref returnType);
									this.cmm.ReturnType = returnType;
								}
								codeTypeReference = this.cmm.ReturnType;
								bArray = propertyData3.IsArray;
							}
							else
							{
								this.cpde = new CodeParameterDeclarationExpression();
								this.cpde.Name = propertyData3.Name;
								this.cpde.Type = this.ConvertCIMType(propertyData3.Type, propertyData3.IsArray);
								this.cpde.Direction = FieldDirection.Out;
								this.cmm.Parameters.Add(this.cpde);
								if (propertyData3.Type == CimType.DateTime)
								{
									CodeTypeReference type2 = this.cpde.Type;
									flag4 = this.GetDateTimeType(propertyData3, ref type2);
									this.cpde.Type = type2;
								}
								this.cpre = new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text2), "Properties");
								this.cie = new CodeIndexerExpression(this.cpre, new CodeExpression[]
								{
									new CodePrimitiveExpression(propertyData3.Name)
								});
								if (propertyData3.Type == CimType.Reference)
								{
									this.GenerateCodeForRefAndDateTimeTypes(this.cie, propertyData3.IsArray, this.cis.TrueStatements, this.PublicNamesUsed["PathClass"].ToString(), new CodeVariableReferenceExpression(propertyData3.Name), true);
								}
								else if (propertyData3.Type == CimType.DateTime)
								{
									if (flag4)
									{
										this.GenerateCodeForRefAndDateTimeTypes(this.cie, propertyData3.IsArray, this.cis.TrueStatements, "System.TimeSpan", new CodeVariableReferenceExpression(propertyData3.Name), true);
									}
									else
									{
										this.GenerateCodeForRefAndDateTimeTypes(this.cie, propertyData3.IsArray, this.cis.TrueStatements, "System.DateTime", new CodeVariableReferenceExpression(propertyData3.Name), true);
									}
								}
								else if (propertyData3.IsArray || propertyData3.Type == CimType.Object)
								{
									this.cis.TrueStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(propertyData3.Name), new CodeCastExpression(this.ConvertCIMType(propertyData3.Type, propertyData3.IsArray), new CodePropertyReferenceExpression(this.cie, "Value"))));
								}
								else
								{
									CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression();
									codeMethodInvokeExpression.Parameters.Add(new CodePropertyReferenceExpression(this.cie, "Value"));
									codeMethodInvokeExpression.Method.MethodName = this.GetConversionFunction(propertyData3.Type);
									codeMethodInvokeExpression.Method.TargetObject = new CodeTypeReferenceExpression("System.Convert");
									this.cis.TrueStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(propertyData3.Name), codeMethodInvokeExpression));
								}
								if (propertyData3.Type == CimType.DateTime && !propertyData3.IsArray)
								{
									if (flag4)
									{
										this.coce = new CodeObjectCreateExpression();
										this.coce.CreateType = new CodeTypeReference("System.TimeSpan");
										this.coce.Parameters.Add(new CodePrimitiveExpression(0));
										this.coce.Parameters.Add(new CodePrimitiveExpression(0));
										this.coce.Parameters.Add(new CodePrimitiveExpression(0));
										this.coce.Parameters.Add(new CodePrimitiveExpression(0));
										this.coce.Parameters.Add(new CodePrimitiveExpression(0));
										this.cis.FalseStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(propertyData3.Name), this.coce));
									}
									else
									{
										this.cis.FalseStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(propertyData3.Name), new CodeFieldReferenceExpression(new CodeTypeReferenceExpression("System.DateTime"), "MinValue")));
									}
								}
								else if (ManagementClassGenerator.IsPropertyValueType(propertyData3.Type) && !propertyData3.IsArray)
								{
									CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression();
									codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(0));
									codeMethodInvokeExpression.Method.MethodName = this.GetConversionFunction(propertyData3.Type);
									codeMethodInvokeExpression.Method.TargetObject = new CodeTypeReferenceExpression("System.Convert");
									this.cis.FalseStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(propertyData3.Name), codeMethodInvokeExpression));
								}
								else
								{
									this.cis.FalseStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(propertyData3.Name), new CodePrimitiveExpression(null)));
								}
							}
						}
					}
				}
				if (!flag6)
				{
					this.cmie = new CodeMethodInvokeExpression(new CodeVariableReferenceExpression(variableName), "InvokeMethod", new CodeExpression[0]);
					this.cmie.Parameters.Add(new CodePrimitiveExpression(methodData.Name));
					this.cmie.Parameters.Add(new CodeVariableReferenceExpression(text));
					this.cmie.Parameters.Add(new CodePrimitiveExpression(null));
					this.cmis = new CodeExpressionStatement(this.cmie);
					this.cis.TrueStatements.Add(this.cmis);
				}
				for (int l = 0; l < arrayList2.Count; l++)
				{
					this.cpre = new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text2), "Properties");
					this.cie = new CodeIndexerExpression(this.cpre, new CodeExpression[]
					{
						new CodePrimitiveExpression(arrayList2[l].ToString())
					});
					this.cis.TrueStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(arrayList2[l].ToString()), new CodeCastExpression((CodeTypeReference)arrayList3[l], new CodePropertyReferenceExpression(this.cie, "Value"))));
				}
				arrayList2.Clear();
				if (flag2)
				{
					this.cis.TrueStatements.Add(new CodeAssignStatement(codePropertyReferenceExpression, new CodeVariableReferenceExpression(this.PrivateNamesUsed["Privileges"].ToString())));
				}
				if (flag5)
				{
					CodeVariableDeclarationStatement value = new CodeVariableDeclarationStatement(codeTypeReference, "retVar");
					this.cpre = new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text2), "Properties");
					this.cie = new CodeIndexerExpression(this.cpre, new CodeExpression[]
					{
						new CodePrimitiveExpression("ReturnValue")
					});
					if (codeTypeReference.BaseType == new CodeTypeReference(this.PublicNamesUsed["PathClass"].ToString()).BaseType)
					{
						this.cmm.Statements.Add(value);
						this.cmm.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression("retVar"), new CodePrimitiveExpression(null)));
						this.GenerateCodeForRefAndDateTimeTypes(this.cie, bArray, this.cis.TrueStatements, this.PublicNamesUsed["PathClass"].ToString(), new CodeVariableReferenceExpression("retVar"), true);
						this.cis.TrueStatements.Add(new CodeMethodReturnStatement(new CodeVariableReferenceExpression("retVar")));
						this.cis.FalseStatements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(null)));
					}
					else if (codeTypeReference.BaseType == "System.DateTime")
					{
						this.cmm.Statements.Add(value);
						this.cmm.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression("retVar"), new CodeFieldReferenceExpression(new CodeTypeReferenceExpression("System.DateTime"), "MinValue")));
						this.GenerateCodeForRefAndDateTimeTypes(this.cie, bArray, this.cis.TrueStatements, "System.DateTime", new CodeVariableReferenceExpression("retVar"), true);
						this.cis.TrueStatements.Add(new CodeMethodReturnStatement(new CodeVariableReferenceExpression("retVar")));
						this.cis.FalseStatements.Add(new CodeMethodReturnStatement(new CodeVariableReferenceExpression("retVar")));
					}
					else if (codeTypeReference.BaseType == "System.TimeSpan")
					{
						this.cmm.Statements.Add(value);
						this.coce = new CodeObjectCreateExpression();
						this.coce.CreateType = new CodeTypeReference("System.TimeSpan");
						this.coce.Parameters.Add(new CodePrimitiveExpression(0));
						this.coce.Parameters.Add(new CodePrimitiveExpression(0));
						this.coce.Parameters.Add(new CodePrimitiveExpression(0));
						this.coce.Parameters.Add(new CodePrimitiveExpression(0));
						this.coce.Parameters.Add(new CodePrimitiveExpression(0));
						this.cmm.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression("retVar"), this.coce));
						this.GenerateCodeForRefAndDateTimeTypes(this.cie, bArray, this.cis.TrueStatements, "System.TimeSpan", new CodeVariableReferenceExpression("retVar"), true);
						this.cis.TrueStatements.Add(new CodeMethodReturnStatement(new CodeVariableReferenceExpression("retVar")));
						this.cis.FalseStatements.Add(new CodeMethodReturnStatement(new CodeVariableReferenceExpression("retVar")));
					}
					else if (codeTypeReference.ArrayRank == 0 && codeTypeReference.BaseType != new CodeTypeReference(this.PublicNamesUsed["BaseObjClass"].ToString()).BaseType)
					{
						this.cmie = new CodeMethodInvokeExpression();
						this.cmie.Parameters.Add(new CodePropertyReferenceExpression(this.cie, "Value"));
						this.cmie.Method.MethodName = this.GetConversionFunction(cimType);
						this.cmie.Method.TargetObject = new CodeTypeReferenceExpression("System.Convert");
						this.cis.TrueStatements.Add(new CodeMethodReturnStatement(this.cmie));
						this.cmie = new CodeMethodInvokeExpression();
						this.cmie.Parameters.Add(new CodePrimitiveExpression(0));
						this.cmie.Method.MethodName = this.GetConversionFunction(cimType);
						this.cmie.Method.TargetObject = new CodeTypeReferenceExpression("System.Convert");
						this.cis.FalseStatements.Add(new CodeMethodReturnStatement(this.cmie));
					}
					else
					{
						this.cis.TrueStatements.Add(new CodeMethodReturnStatement(new CodeCastExpression(codeTypeReference, new CodePropertyReferenceExpression(this.cie, "Value"))));
						this.cis.FalseStatements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(null)));
					}
				}
				this.cmm.Statements.Add(this.cis);
				this.cc.Members.Add(this.cmm);
			}
		}

		// Token: 0x0600030F RID: 783 RVA: 0x00017824 File Offset: 0x00015A24
		private void GenerateGetInstancesWithNoParameters()
		{
			this.cmm = new CodeMemberMethod();
			this.cmm.Attributes = (MemberAttributes)24579;
			this.cmm.Name = this.PublicNamesUsed["FilterFunction"].ToString();
			this.cmm.ReturnType = new CodeTypeReference(this.PrivateNamesUsed["CollectionClass"].ToString());
			this.cmie = new CodeMethodInvokeExpression();
			this.cmie.Method.MethodName = this.PublicNamesUsed["FilterFunction"].ToString();
			this.cmie.Parameters.Add(new CodePrimitiveExpression(null));
			this.cmie.Parameters.Add(new CodePrimitiveExpression(null));
			this.cmie.Parameters.Add(new CodePrimitiveExpression(null));
			this.cmm.Statements.Add(new CodeMethodReturnStatement(this.cmie));
			this.cc.Members.Add(this.cmm);
			this.cmm.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("COMMENT_GETINSTANCES")));
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0001795C File Offset: 0x00015B5C
		private void GenerateGetInstancesWithCondition()
		{
			string text = "condition";
			this.cmm = new CodeMemberMethod();
			this.cmm.Attributes = (MemberAttributes)24579;
			this.cmm.Name = this.PublicNamesUsed["FilterFunction"].ToString();
			this.cmm.ReturnType = new CodeTypeReference(this.PrivateNamesUsed["CollectionClass"].ToString());
			this.cmm.Parameters.Add(new CodeParameterDeclarationExpression("System.String", text));
			this.cmie = new CodeMethodInvokeExpression(null, this.PublicNamesUsed["FilterFunction"].ToString(), new CodeExpression[0]);
			this.cmie.Parameters.Add(new CodePrimitiveExpression(null));
			this.cmie.Parameters.Add(new CodeVariableReferenceExpression(text));
			this.cmie.Parameters.Add(new CodePrimitiveExpression(null));
			this.cmm.Statements.Add(new CodeMethodReturnStatement(this.cmie));
			this.cc.Members.Add(this.cmm);
		}

		// Token: 0x06000311 RID: 785 RVA: 0x00017A8C File Offset: 0x00015C8C
		private void GenerateGetInstancesWithProperties()
		{
			string text = "selectedProperties";
			this.cmm = new CodeMemberMethod();
			this.cmm.Attributes = (MemberAttributes)24579;
			this.cmm.Name = this.PublicNamesUsed["FilterFunction"].ToString();
			this.cmm.ReturnType = new CodeTypeReference(this.PrivateNamesUsed["CollectionClass"].ToString());
			this.cmm.Parameters.Add(new CodeParameterDeclarationExpression("System.String []", text));
			this.cmie = new CodeMethodInvokeExpression(null, this.PublicNamesUsed["FilterFunction"].ToString(), new CodeExpression[0]);
			this.cmie.Parameters.Add(new CodePrimitiveExpression(null));
			this.cmie.Parameters.Add(new CodePrimitiveExpression(null));
			this.cmie.Parameters.Add(new CodeVariableReferenceExpression(text));
			this.cmm.Statements.Add(new CodeMethodReturnStatement(this.cmie));
			this.cc.Members.Add(this.cmm);
		}

		// Token: 0x06000312 RID: 786 RVA: 0x00017BBC File Offset: 0x00015DBC
		private void GenerateGetInstancesWithWhereProperties()
		{
			string text = "selectedProperties";
			string text2 = "condition";
			this.cmm = new CodeMemberMethod();
			this.cmm.Attributes = (MemberAttributes)24579;
			this.cmm.Name = this.PublicNamesUsed["FilterFunction"].ToString();
			this.cmm.ReturnType = new CodeTypeReference(this.PrivateNamesUsed["CollectionClass"].ToString());
			this.cmm.Parameters.Add(new CodeParameterDeclarationExpression("System.String", text2));
			this.cmm.Parameters.Add(new CodeParameterDeclarationExpression("System.String []", text));
			this.cmie = new CodeMethodInvokeExpression(null, this.PublicNamesUsed["FilterFunction"].ToString(), new CodeExpression[0]);
			this.cmie.Parameters.Add(new CodePrimitiveExpression(null));
			this.cmie.Parameters.Add(new CodeVariableReferenceExpression(text2));
			this.cmie.Parameters.Add(new CodeVariableReferenceExpression(text));
			this.cmm.Statements.Add(new CodeMethodReturnStatement(this.cmie));
			this.cc.Members.Add(this.cmm);
		}

		// Token: 0x06000313 RID: 787 RVA: 0x00017D0C File Offset: 0x00015F0C
		private void GenerateGetInstancesWithScope()
		{
			this.cmm = new CodeMemberMethod();
			this.cmm.Attributes = (MemberAttributes)24579;
			this.cmm.Name = this.PublicNamesUsed["FilterFunction"].ToString();
			this.cmm.ReturnType = new CodeTypeReference(this.PrivateNamesUsed["CollectionClass"].ToString());
			this.cmm.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(this.PublicNamesUsed["ScopeClass"].ToString()), this.PrivateNamesUsed["ScopeParam"].ToString()));
			this.cmm.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(this.PublicNamesUsed["QueryOptionsClass"].ToString()), this.PrivateNamesUsed["EnumParam"].ToString()));
			string text = "clsObject";
			string text2 = "pathObj";
			this.cis = new CodeConditionStatement();
			this.cboe = new CodeBinaryOperatorExpression();
			this.cboe.Left = new CodeVariableReferenceExpression(this.PrivateNamesUsed["ScopeParam"].ToString());
			this.cboe.Right = new CodePrimitiveExpression(null);
			this.cboe.Operator = CodeBinaryOperatorType.IdentityEquality;
			this.cis.Condition = this.cboe;
			CodeConditionStatement codeConditionStatement = new CodeConditionStatement();
			codeConditionStatement.Condition = new CodeBinaryOperatorExpression
			{
				Left = new CodeVariableReferenceExpression(this.PrivateNamesUsed["statMgmtScope"].ToString()),
				Right = new CodePrimitiveExpression(null),
				Operator = CodeBinaryOperatorType.IdentityEquality
			};
			this.coce = new CodeObjectCreateExpression();
			this.coce.CreateType = new CodeTypeReference(this.PublicNamesUsed["ScopeClass"].ToString());
			codeConditionStatement.TrueStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(this.PrivateNamesUsed["ScopeParam"].ToString()), this.coce));
			codeConditionStatement.TrueStatements.Add(new CodeAssignStatement(new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(this.PrivateNamesUsed["ScopeParam"].ToString()), "Path"), "NamespacePath"), new CodePrimitiveExpression(this.classobj.Scope.Path.NamespacePath)));
			codeConditionStatement.FalseStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(this.PrivateNamesUsed["ScopeParam"].ToString()), new CodeVariableReferenceExpression(this.PrivateNamesUsed["statMgmtScope"].ToString())));
			this.cis.TrueStatements.Add(codeConditionStatement);
			this.cmm.Statements.Add(this.cis);
			this.coce = new CodeObjectCreateExpression();
			this.coce.CreateType = new CodeTypeReference(this.PublicNamesUsed["PathClass"].ToString());
			this.cmm.Statements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference(this.PublicNamesUsed["PathClass"].ToString()), text2, this.coce));
			this.cmm.Statements.Add(new CodeAssignStatement(new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text2), "ClassName"), new CodePrimitiveExpression(this.OriginalClassName)));
			this.cmm.Statements.Add(new CodeAssignStatement(new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text2), "NamespacePath"), new CodePrimitiveExpression(this.classobj.Scope.Path.NamespacePath)));
			this.coce = new CodeObjectCreateExpression();
			this.coce.CreateType = new CodeTypeReference(this.PublicNamesUsed["ManagementClass"].ToString());
			this.coce.Parameters.Add(new CodeVariableReferenceExpression(this.PrivateNamesUsed["ScopeParam"].ToString()));
			this.coce.Parameters.Add(new CodeVariableReferenceExpression(text2));
			this.coce.Parameters.Add(new CodePrimitiveExpression(null));
			this.cmm.Statements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference(this.PublicNamesUsed["ManagementClass"].ToString()), text, this.coce));
			this.cis = new CodeConditionStatement();
			this.cboe = new CodeBinaryOperatorExpression();
			this.cboe.Left = new CodeVariableReferenceExpression(this.PrivateNamesUsed["EnumParam"].ToString());
			this.cboe.Right = new CodePrimitiveExpression(null);
			this.cboe.Operator = CodeBinaryOperatorType.IdentityEquality;
			this.cis.Condition = this.cboe;
			this.coce = new CodeObjectCreateExpression();
			this.coce.CreateType = new CodeTypeReference(this.PublicNamesUsed["QueryOptionsClass"].ToString());
			this.cis.TrueStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(this.PrivateNamesUsed["EnumParam"].ToString()), this.coce));
			this.cis.TrueStatements.Add(new CodeAssignStatement(new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(this.PrivateNamesUsed["EnumParam"].ToString()), "EnsureLocatable"), new CodePrimitiveExpression(true)));
			this.cmm.Statements.Add(this.cis);
			this.coce = new CodeObjectCreateExpression();
			this.coce.CreateType = new CodeTypeReference(this.PrivateNamesUsed["CollectionClass"].ToString());
			this.cmie = new CodeMethodInvokeExpression();
			this.cmie.Method = new CodeMethodReferenceExpression(new CodeVariableReferenceExpression(text), "GetInstances");
			this.cmie.Parameters.Add(new CodeVariableReferenceExpression(this.PrivateNamesUsed["EnumParam"].ToString()));
			this.coce.Parameters.Add(this.cmie);
			this.cmm.Statements.Add(new CodeMethodReturnStatement(this.coce));
			this.cc.Members.Add(this.cmm);
		}

		// Token: 0x06000314 RID: 788 RVA: 0x00018374 File Offset: 0x00016574
		private void GenerateGetInstancesWithScopeCondition()
		{
			string text = "condition";
			this.cmm = new CodeMemberMethod();
			this.cmm.Attributes = (MemberAttributes)24579;
			this.cmm.Name = this.PublicNamesUsed["FilterFunction"].ToString();
			this.cmm.ReturnType = new CodeTypeReference(this.PrivateNamesUsed["CollectionClass"].ToString());
			this.cmm.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(this.PublicNamesUsed["ScopeClass"].ToString()), this.PrivateNamesUsed["ScopeParam"].ToString()));
			this.cmm.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference("System.String"), text));
			this.cmie = new CodeMethodInvokeExpression(null, this.PublicNamesUsed["FilterFunction"].ToString(), new CodeExpression[0]);
			this.cmie.Parameters.Add(new CodeVariableReferenceExpression(this.PrivateNamesUsed["ScopeParam"].ToString()));
			this.cmie.Parameters.Add(new CodeVariableReferenceExpression(text));
			this.cmie.Parameters.Add(new CodePrimitiveExpression(null));
			this.cmm.Statements.Add(new CodeMethodReturnStatement(this.cmie));
			this.cc.Members.Add(this.cmm);
		}

		// Token: 0x06000315 RID: 789 RVA: 0x00018500 File Offset: 0x00016700
		private void GenerateGetInstancesWithScopeProperties()
		{
			string text = "selectedProperties";
			this.cmm = new CodeMemberMethod();
			this.cmm.Attributes = (MemberAttributes)24579;
			this.cmm.Name = this.PublicNamesUsed["FilterFunction"].ToString();
			this.cmm.ReturnType = new CodeTypeReference(this.PrivateNamesUsed["CollectionClass"].ToString());
			this.cmm.Parameters.Add(new CodeParameterDeclarationExpression(this.PublicNamesUsed["ScopeClass"].ToString(), this.PrivateNamesUsed["ScopeParam"].ToString()));
			this.cmm.Parameters.Add(new CodeParameterDeclarationExpression("System.String []", text));
			this.cmie = new CodeMethodInvokeExpression(null, this.PublicNamesUsed["FilterFunction"].ToString(), new CodeExpression[0]);
			this.cmie.Parameters.Add(new CodeVariableReferenceExpression(this.PrivateNamesUsed["ScopeParam"].ToString()));
			this.cmie.Parameters.Add(new CodePrimitiveExpression(null));
			this.cmie.Parameters.Add(new CodeVariableReferenceExpression(text));
			this.cmm.Statements.Add(new CodeMethodReturnStatement(this.cmie));
			this.cc.Members.Add(this.cmm);
		}

		// Token: 0x06000316 RID: 790 RVA: 0x00018684 File Offset: 0x00016884
		private void GenerateGetInstancesWithScopeWhereProperties()
		{
			string text = "condition";
			string text2 = "selectedProperties";
			string text3 = "ObjectSearcher";
			this.cmm = new CodeMemberMethod();
			this.cmm.Attributes = (MemberAttributes)24579;
			this.cmm.Name = this.PublicNamesUsed["FilterFunction"].ToString();
			this.cmm.ReturnType = new CodeTypeReference(this.PrivateNamesUsed["CollectionClass"].ToString());
			this.cmm.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(this.PublicNamesUsed["ScopeClass"].ToString()), this.PrivateNamesUsed["ScopeParam"].ToString()));
			this.cmm.Parameters.Add(new CodeParameterDeclarationExpression("System.String", text));
			this.cmm.Parameters.Add(new CodeParameterDeclarationExpression("System.String []", text2));
			this.cis = new CodeConditionStatement();
			this.cboe = new CodeBinaryOperatorExpression();
			this.cboe.Left = new CodeVariableReferenceExpression(this.PrivateNamesUsed["ScopeParam"].ToString());
			this.cboe.Right = new CodePrimitiveExpression(null);
			this.cboe.Operator = CodeBinaryOperatorType.IdentityEquality;
			this.cis.Condition = this.cboe;
			CodeConditionStatement codeConditionStatement = new CodeConditionStatement();
			codeConditionStatement.Condition = new CodeBinaryOperatorExpression
			{
				Left = new CodeVariableReferenceExpression(this.PrivateNamesUsed["statMgmtScope"].ToString()),
				Right = new CodePrimitiveExpression(null),
				Operator = CodeBinaryOperatorType.IdentityEquality
			};
			this.coce = new CodeObjectCreateExpression();
			this.coce.CreateType = new CodeTypeReference(this.PublicNamesUsed["ScopeClass"].ToString());
			codeConditionStatement.TrueStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(this.PrivateNamesUsed["ScopeParam"].ToString()), this.coce));
			codeConditionStatement.TrueStatements.Add(new CodeAssignStatement(new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(this.PrivateNamesUsed["ScopeParam"].ToString()), "Path"), "NamespacePath"), new CodePrimitiveExpression(this.classobj.Scope.Path.NamespacePath)));
			codeConditionStatement.FalseStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(this.PrivateNamesUsed["ScopeParam"].ToString()), new CodeVariableReferenceExpression(this.PrivateNamesUsed["statMgmtScope"].ToString())));
			this.cis.TrueStatements.Add(codeConditionStatement);
			this.cmm.Statements.Add(this.cis);
			CodeObjectCreateExpression codeObjectCreateExpression = new CodeObjectCreateExpression();
			codeObjectCreateExpression.CreateType = new CodeTypeReference(this.PublicNamesUsed["QueryClass"].ToString());
			codeObjectCreateExpression.Parameters.Add(new CodePrimitiveExpression(this.OriginalClassName));
			codeObjectCreateExpression.Parameters.Add(new CodeVariableReferenceExpression(text));
			codeObjectCreateExpression.Parameters.Add(new CodeVariableReferenceExpression(text2));
			this.coce = new CodeObjectCreateExpression();
			this.coce.CreateType = new CodeTypeReference(this.PublicNamesUsed["ObjectSearcherClass"].ToString());
			this.coce.Parameters.Add(new CodeVariableReferenceExpression(this.PrivateNamesUsed["ScopeParam"].ToString()));
			this.coce.Parameters.Add(codeObjectCreateExpression);
			this.cmm.Statements.Add(new CodeVariableDeclarationStatement(this.PublicNamesUsed["ObjectSearcherClass"].ToString(), text3, this.coce));
			this.coce = new CodeObjectCreateExpression();
			this.coce.CreateType = new CodeTypeReference(this.PublicNamesUsed["QueryOptionsClass"].ToString());
			this.cmm.Statements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference(this.PublicNamesUsed["QueryOptionsClass"].ToString()), this.PrivateNamesUsed["EnumParam"].ToString(), this.coce));
			this.cmm.Statements.Add(new CodeAssignStatement(new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(this.PrivateNamesUsed["EnumParam"].ToString()), "EnsureLocatable"), new CodePrimitiveExpression(true)));
			this.cmm.Statements.Add(new CodeAssignStatement(new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text3), "Options"), new CodeVariableReferenceExpression(this.PrivateNamesUsed["EnumParam"].ToString())));
			this.coce = new CodeObjectCreateExpression();
			this.coce.CreateType = new CodeTypeReference(this.PrivateNamesUsed["CollectionClass"].ToString());
			this.coce.Parameters.Add(new CodeMethodInvokeExpression(new CodeVariableReferenceExpression(text3), "Get", new CodeExpression[0]));
			this.cmm.Statements.Add(new CodeMethodReturnStatement(this.coce));
			this.cc.Members.Add(this.cmm);
		}

		// Token: 0x06000317 RID: 791 RVA: 0x00018BEC File Offset: 0x00016DEC
		private void GeneratePrivateMember(string memberName, string MemberType, string Comment)
		{
			this.GeneratePrivateMember(memberName, MemberType, null, false, Comment);
		}

		// Token: 0x06000318 RID: 792 RVA: 0x00018BFC File Offset: 0x00016DFC
		private void GeneratePrivateMember(string memberName, string MemberType, CodeExpression initExpression, bool isStatic, string Comment)
		{
			this.cf = new CodeMemberField();
			this.cf.Name = memberName;
			this.cf.Attributes = (MemberAttributes)20482;
			if (isStatic)
			{
				this.cf.Attributes = (this.cf.Attributes | MemberAttributes.Static);
			}
			this.cf.Type = new CodeTypeReference(MemberType);
			if (initExpression != null && isStatic)
			{
				this.cf.InitExpression = initExpression;
			}
			this.cc.Members.Add(this.cf);
			if (Comment != null && Comment.Length != 0)
			{
				this.cf.Comments.Add(new CodeCommentStatement(Comment));
			}
		}

		// Token: 0x06000319 RID: 793 RVA: 0x00018CB0 File Offset: 0x00016EB0
		private CodeTypeDeclaration GenerateTypeConverterClass()
		{
			string type = "System.ComponentModel.ITypeDescriptorContext";
			string text = "context";
			string text2 = "destinationType";
			string text3 = "value";
			string type2 = "System.Globalization.CultureInfo";
			string text4 = "culture";
			string type3 = "System.Collections.IDictionary";
			string text5 = "dictionary";
			string typeName = "PropertyDescriptorCollection";
			string text6 = "attributeVar";
			string text7 = "inBaseType";
			string text8 = "baseConverter";
			string text9 = "baseType";
			string type4 = "TypeDescriptor";
			string text10 = "srcType";
			CodeTypeDeclaration codeTypeDeclaration = new CodeTypeDeclaration(this.PrivateNamesUsed["ConverterClass"].ToString());
			codeTypeDeclaration.BaseTypes.Add(this.PublicNamesUsed["TypeConverter"].ToString());
			this.cf = new CodeMemberField();
			this.cf.Name = text8;
			this.cf.Attributes = (MemberAttributes)20482;
			this.cf.Type = new CodeTypeReference(this.PublicNamesUsed["TypeConverter"].ToString());
			codeTypeDeclaration.Members.Add(this.cf);
			this.cf = new CodeMemberField();
			this.cf.Name = text9;
			this.cf.Attributes = (MemberAttributes)20482;
			this.cf.Type = new CodeTypeReference(this.PublicNamesUsed["Type"].ToString());
			codeTypeDeclaration.Members.Add(this.cf);
			this.cctor = new CodeConstructor();
			this.cctor.Attributes = MemberAttributes.Public;
			this.cpde = new CodeParameterDeclarationExpression();
			this.cpde.Name = text7;
			this.cpde.Type = new CodeTypeReference("System.Type");
			this.cctor.Parameters.Add(this.cpde);
			this.cmie = new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(type4), "GetConverter", new CodeExpression[0]);
			this.cmie.Parameters.Add(new CodeVariableReferenceExpression(text7));
			this.cctor.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(text8), this.cmie));
			this.cctor.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(text9), new CodeVariableReferenceExpression(text7)));
			codeTypeDeclaration.Members.Add(this.cctor);
			this.cmm = new CodeMemberMethod();
			this.cmm.Attributes = (MemberAttributes)24836;
			this.cmm.Name = "CanConvertFrom";
			this.cmm.ReturnType = new CodeTypeReference("System.Boolean");
			this.cmm.Parameters.Add(new CodeParameterDeclarationExpression(type, text));
			this.cmm.Parameters.Add(new CodeParameterDeclarationExpression("System.Type", text10));
			this.cmie = new CodeMethodInvokeExpression(new CodeVariableReferenceExpression(text8), "CanConvertFrom", new CodeExpression[0]);
			this.cmie.Parameters.Add(new CodeVariableReferenceExpression(text));
			this.cmie.Parameters.Add(new CodeVariableReferenceExpression(text10));
			this.cmm.Statements.Add(new CodeMethodReturnStatement(this.cmie));
			codeTypeDeclaration.Members.Add(this.cmm);
			this.cmm = new CodeMemberMethod();
			this.cmm.Attributes = (MemberAttributes)24836;
			this.cmm.Name = "CanConvertTo";
			this.cmm.ReturnType = new CodeTypeReference("System.Boolean");
			this.cmm.Parameters.Add(new CodeParameterDeclarationExpression(type, text));
			this.cmm.Parameters.Add(new CodeParameterDeclarationExpression("System.Type", text2));
			this.cmie = new CodeMethodInvokeExpression(new CodeVariableReferenceExpression(text8), "CanConvertTo", new CodeExpression[0]);
			this.cmie.Parameters.Add(new CodeVariableReferenceExpression(text));
			this.cmie.Parameters.Add(new CodeVariableReferenceExpression(text2));
			this.cmm.Statements.Add(new CodeMethodReturnStatement(this.cmie));
			codeTypeDeclaration.Members.Add(this.cmm);
			this.cmm = new CodeMemberMethod();
			this.cmm.Attributes = (MemberAttributes)24836;
			this.cmm.Name = "ConvertFrom";
			this.cmm.ReturnType = new CodeTypeReference("System.Object");
			this.cmm.Parameters.Add(new CodeParameterDeclarationExpression(type, text));
			this.cmm.Parameters.Add(new CodeParameterDeclarationExpression(type2, text4));
			this.cmm.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference("System.Object"), text3));
			this.cmie = new CodeMethodInvokeExpression(new CodeVariableReferenceExpression(text8), "ConvertFrom", new CodeExpression[0]);
			this.cmie.Parameters.Add(new CodeVariableReferenceExpression(text));
			this.cmie.Parameters.Add(new CodeVariableReferenceExpression(text4));
			this.cmie.Parameters.Add(new CodeVariableReferenceExpression(text3));
			this.cmm.Statements.Add(new CodeMethodReturnStatement(this.cmie));
			codeTypeDeclaration.Members.Add(this.cmm);
			this.cmm = new CodeMemberMethod();
			this.cmm.Attributes = (MemberAttributes)24836;
			this.cmm.ReturnType = new CodeTypeReference("System.Object");
			this.cmm.Name = "CreateInstance";
			this.cmm.Parameters.Add(new CodeParameterDeclarationExpression(type, text));
			this.cmm.Parameters.Add(new CodeParameterDeclarationExpression(type3, text5));
			this.cmie = new CodeMethodInvokeExpression(new CodeVariableReferenceExpression(text8), "CreateInstance", new CodeExpression[0]);
			this.cmie.Parameters.Add(new CodeVariableReferenceExpression(text));
			this.cmie.Parameters.Add(new CodeVariableReferenceExpression(text5));
			this.cmm.Statements.Add(new CodeMethodReturnStatement(this.cmie));
			codeTypeDeclaration.Members.Add(this.cmm);
			this.cmm = new CodeMemberMethod();
			this.cmm.Attributes = (MemberAttributes)24836;
			this.cmm.Name = "GetCreateInstanceSupported";
			this.cmm.ReturnType = new CodeTypeReference("System.Boolean");
			this.cmm.Parameters.Add(new CodeParameterDeclarationExpression(type, text));
			this.cmie = new CodeMethodInvokeExpression(new CodeVariableReferenceExpression(text8), "GetCreateInstanceSupported", new CodeExpression[0]);
			this.cmie.Parameters.Add(new CodeVariableReferenceExpression(text));
			this.cmm.Statements.Add(new CodeMethodReturnStatement(this.cmie));
			codeTypeDeclaration.Members.Add(this.cmm);
			this.cmm = new CodeMemberMethod();
			this.cmm.Attributes = (MemberAttributes)24836;
			this.cmm.Name = "GetProperties";
			this.cmm.Parameters.Add(new CodeParameterDeclarationExpression(type, text));
			this.cmm.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference("System.Object"), text3));
			CodeTypeReference type5 = new CodeTypeReference(new CodeTypeReference("System.Attribute"), 1);
			this.cmm.Parameters.Add(new CodeParameterDeclarationExpression(type5, text6));
			this.cmm.ReturnType = new CodeTypeReference(typeName);
			this.cmie = new CodeMethodInvokeExpression(new CodeVariableReferenceExpression(text8), "GetProperties", new CodeExpression[0]);
			this.cmie.Parameters.Add(new CodeVariableReferenceExpression(text));
			this.cmie.Parameters.Add(new CodeVariableReferenceExpression(text3));
			this.cmie.Parameters.Add(new CodeVariableReferenceExpression(text6));
			this.cmm.Statements.Add(new CodeMethodReturnStatement(this.cmie));
			codeTypeDeclaration.Members.Add(this.cmm);
			this.cmm = new CodeMemberMethod();
			this.cmm.Attributes = (MemberAttributes)24836;
			this.cmm.Name = "GetPropertiesSupported";
			this.cmm.Parameters.Add(new CodeParameterDeclarationExpression(type, text));
			this.cmm.ReturnType = new CodeTypeReference("System.Boolean");
			this.cmie = new CodeMethodInvokeExpression(new CodeVariableReferenceExpression(text8), "GetPropertiesSupported", new CodeExpression[0]);
			this.cmie.Parameters.Add(new CodeVariableReferenceExpression(text));
			this.cmm.Statements.Add(new CodeMethodReturnStatement(this.cmie));
			codeTypeDeclaration.Members.Add(this.cmm);
			this.cmm = new CodeMemberMethod();
			this.cmm.Attributes = (MemberAttributes)24836;
			this.cmm.Name = "GetStandardValues";
			this.cmm.Parameters.Add(new CodeParameterDeclarationExpression(type, text));
			this.cmm.ReturnType = new CodeTypeReference("System.ComponentModel.TypeConverter.StandardValuesCollection");
			this.cmie = new CodeMethodInvokeExpression(new CodeVariableReferenceExpression(text8), "GetStandardValues", new CodeExpression[0]);
			this.cmie.Parameters.Add(new CodeVariableReferenceExpression(text));
			this.cmm.Statements.Add(new CodeMethodReturnStatement(this.cmie));
			codeTypeDeclaration.Members.Add(this.cmm);
			this.cmm = new CodeMemberMethod();
			this.cmm.Attributes = (MemberAttributes)24836;
			this.cmm.Name = "GetStandardValuesExclusive";
			this.cmm.Parameters.Add(new CodeParameterDeclarationExpression(type, text));
			this.cmm.ReturnType = new CodeTypeReference("System.Boolean");
			this.cmie = new CodeMethodInvokeExpression(new CodeVariableReferenceExpression(text8), "GetStandardValuesExclusive", new CodeExpression[0]);
			this.cmie.Parameters.Add(new CodeVariableReferenceExpression(text));
			this.cmm.Statements.Add(new CodeMethodReturnStatement(this.cmie));
			codeTypeDeclaration.Members.Add(this.cmm);
			this.cmm = new CodeMemberMethod();
			this.cmm.Attributes = (MemberAttributes)24836;
			this.cmm.Name = "GetStandardValuesSupported";
			this.cmm.Parameters.Add(new CodeParameterDeclarationExpression(type, text));
			this.cmm.ReturnType = new CodeTypeReference("System.Boolean");
			this.cmie = new CodeMethodInvokeExpression(new CodeVariableReferenceExpression(text8), "GetStandardValuesSupported", new CodeExpression[0]);
			this.cmie.Parameters.Add(new CodeVariableReferenceExpression(text));
			this.cmm.Statements.Add(new CodeMethodReturnStatement(this.cmie));
			codeTypeDeclaration.Members.Add(this.cmm);
			this.cmm = new CodeMemberMethod();
			this.cmm.Attributes = (MemberAttributes)24836;
			this.cmm.Name = "ConvertTo";
			this.cmm.Parameters.Add(new CodeParameterDeclarationExpression(type, text));
			this.cmm.Parameters.Add(new CodeParameterDeclarationExpression(type2, text4));
			this.cmm.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference("System.Object"), text3));
			this.cmm.Parameters.Add(new CodeParameterDeclarationExpression("System.Type", text2));
			this.cmm.ReturnType = new CodeTypeReference("System.Object");
			this.cmie = new CodeMethodInvokeExpression(new CodeVariableReferenceExpression(text8), "ConvertTo", new CodeExpression[0]);
			this.cmie.Parameters.Add(new CodeVariableReferenceExpression(text));
			this.cmie.Parameters.Add(new CodeVariableReferenceExpression(text4));
			this.cmie.Parameters.Add(new CodeVariableReferenceExpression(text3));
			this.cmie.Parameters.Add(new CodeVariableReferenceExpression(text2));
			CodeMethodReturnStatement value = new CodeMethodReturnStatement(this.cmie);
			this.cis = new CodeConditionStatement();
			CodeBinaryOperatorExpression codeBinaryOperatorExpression = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression.Left = new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text9), "BaseType");
			codeBinaryOperatorExpression.Right = new CodeTypeOfExpression(typeof(Enum));
			codeBinaryOperatorExpression.Operator = CodeBinaryOperatorType.IdentityEquality;
			this.cis.Condition = codeBinaryOperatorExpression;
			CodeBinaryOperatorExpression codeBinaryOperatorExpression2 = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression2.Left = new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("value"), "GetType", new CodeExpression[0]);
			codeBinaryOperatorExpression2.Right = new CodeVariableReferenceExpression("destinationType");
			codeBinaryOperatorExpression2.Operator = CodeBinaryOperatorType.IdentityEquality;
			this.cis.TrueStatements.Add(new CodeConditionStatement(codeBinaryOperatorExpression2, new CodeStatement[]
			{
				new CodeMethodReturnStatement(new CodeVariableReferenceExpression("value"))
			}));
			CodeBinaryOperatorExpression codeBinaryOperatorExpression3 = new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression("value"), CodeBinaryOperatorType.IdentityEquality, new CodePrimitiveExpression(null));
			CodeBinaryOperatorExpression right = new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression(text), CodeBinaryOperatorType.IdentityInequality, new CodePrimitiveExpression(null));
			CodeBinaryOperatorExpression codeBinaryOperatorExpression4 = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression4.Left = codeBinaryOperatorExpression3;
			codeBinaryOperatorExpression4.Right = right;
			codeBinaryOperatorExpression4.Operator = CodeBinaryOperatorType.BooleanAnd;
			this.cmie = new CodeMethodInvokeExpression(new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text), "PropertyDescriptor"), "ShouldSerializeValue", new CodeExpression[0]);
			this.cmie.Parameters.Add(new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text), "Instance"));
			CodeBinaryOperatorExpression right2 = new CodeBinaryOperatorExpression(this.cmie, CodeBinaryOperatorType.ValueEquality, new CodePrimitiveExpression(false));
			CodeBinaryOperatorExpression codeBinaryOperatorExpression5 = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression5.Left = codeBinaryOperatorExpression4;
			codeBinaryOperatorExpression5.Right = right2;
			codeBinaryOperatorExpression5.Operator = CodeBinaryOperatorType.BooleanAnd;
			this.cis.TrueStatements.Add(new CodeConditionStatement(codeBinaryOperatorExpression5, new CodeStatement[]
			{
				new CodeMethodReturnStatement(new CodeSnippetExpression(" \"NULL_ENUM_VALUE\" "))
			}));
			this.cis.TrueStatements.Add(value);
			this.cmm.Statements.Add(this.cis);
			this.cis = new CodeConditionStatement();
			codeBinaryOperatorExpression = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression.Left = new CodeVariableReferenceExpression(text9);
			codeBinaryOperatorExpression.Right = new CodeTypeOfExpression(this.PublicNamesUsed["Boolean"].ToString());
			codeBinaryOperatorExpression.Operator = CodeBinaryOperatorType.IdentityEquality;
			codeBinaryOperatorExpression2 = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression2.Left = new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text9), "BaseType");
			codeBinaryOperatorExpression2.Right = new CodeTypeOfExpression(this.PublicNamesUsed["ValueType"].ToString());
			codeBinaryOperatorExpression2.Operator = CodeBinaryOperatorType.IdentityEquality;
			codeBinaryOperatorExpression3 = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression3.Left = codeBinaryOperatorExpression;
			codeBinaryOperatorExpression3.Right = codeBinaryOperatorExpression2;
			codeBinaryOperatorExpression3.Operator = CodeBinaryOperatorType.BooleanAnd;
			this.cis.Condition = codeBinaryOperatorExpression3;
			codeBinaryOperatorExpression3 = new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression("value"), CodeBinaryOperatorType.IdentityEquality, new CodePrimitiveExpression(null));
			right = new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression(text), CodeBinaryOperatorType.IdentityInequality, new CodePrimitiveExpression(null));
			codeBinaryOperatorExpression4 = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression4.Left = codeBinaryOperatorExpression3;
			codeBinaryOperatorExpression4.Right = right;
			codeBinaryOperatorExpression4.Operator = CodeBinaryOperatorType.BooleanAnd;
			this.cmie = new CodeMethodInvokeExpression(new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text), "PropertyDescriptor"), "ShouldSerializeValue", new CodeExpression[0]);
			this.cmie.Parameters.Add(new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text), "Instance"));
			right2 = new CodeBinaryOperatorExpression(this.cmie, CodeBinaryOperatorType.ValueEquality, new CodePrimitiveExpression(false));
			codeBinaryOperatorExpression5 = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression5.Left = codeBinaryOperatorExpression4;
			codeBinaryOperatorExpression5.Right = right2;
			codeBinaryOperatorExpression5.Operator = CodeBinaryOperatorType.BooleanAnd;
			this.cis.TrueStatements.Add(new CodeConditionStatement(codeBinaryOperatorExpression5, new CodeStatement[]
			{
				new CodeMethodReturnStatement(new CodePrimitiveExpression(""))
			}));
			this.cis.TrueStatements.Add(value);
			this.cmm.Statements.Add(this.cis);
			this.cis = new CodeConditionStatement();
			codeBinaryOperatorExpression = new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression(text), CodeBinaryOperatorType.IdentityInequality, new CodePrimitiveExpression(null));
			this.cmie = new CodeMethodInvokeExpression(new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text), "PropertyDescriptor"), "ShouldSerializeValue", new CodeExpression[0]);
			this.cmie.Parameters.Add(new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text), "Instance"));
			codeBinaryOperatorExpression2 = new CodeBinaryOperatorExpression(this.cmie, CodeBinaryOperatorType.ValueEquality, new CodePrimitiveExpression(false));
			codeBinaryOperatorExpression3 = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression3.Left = codeBinaryOperatorExpression;
			codeBinaryOperatorExpression3.Right = codeBinaryOperatorExpression2;
			codeBinaryOperatorExpression3.Operator = CodeBinaryOperatorType.BooleanAnd;
			this.cis.Condition = codeBinaryOperatorExpression3;
			this.cis.TrueStatements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression("")));
			this.cmm.Statements.Add(this.cis);
			this.cmm.Statements.Add(value);
			codeTypeDeclaration.Members.Add(this.cmm);
			codeTypeDeclaration.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("COMMENT_PROPTYPECONVERTER")));
			return codeTypeDeclaration;
		}

		// Token: 0x0600031A RID: 794 RVA: 0x00019E14 File Offset: 0x00018014
		private void GenerateCollectionClass()
		{
			string typeName = "ManagementObjectCollection";
			string text = "privColObj";
			string text2 = "objCollection";
			this.ccc = new CodeTypeDeclaration(this.PrivateNamesUsed["CollectionClass"].ToString());
			this.ccc.BaseTypes.Add("System.Object");
			this.ccc.BaseTypes.Add("ICollection");
			this.ccc.TypeAttributes = TypeAttributes.NestedPublic;
			this.cf = new CodeMemberField();
			this.cf.Name = text;
			this.cf.Attributes = (MemberAttributes)20482;
			this.cf.Type = new CodeTypeReference(typeName);
			this.ccc.Members.Add(this.cf);
			this.cctor = new CodeConstructor();
			this.cctor.Attributes = MemberAttributes.Public;
			this.cpde = new CodeParameterDeclarationExpression();
			this.cpde.Name = text2;
			this.cpde.Type = new CodeTypeReference(typeName);
			this.cctor.Parameters.Add(this.cpde);
			this.cctor.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(text), new CodeVariableReferenceExpression(text2)));
			this.ccc.Members.Add(this.cctor);
			this.cmp = new CodeMemberProperty();
			this.cmp.Type = new CodeTypeReference("System.Int32");
			this.cmp.Attributes = (MemberAttributes)24582;
			this.cmp.Name = "Count";
			this.cmp.ImplementationTypes.Add("System.Collections.ICollection");
			this.cmp.GetStatements.Add(new CodeMethodReturnStatement(new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text), "Count")));
			this.ccc.Members.Add(this.cmp);
			this.cmp = new CodeMemberProperty();
			this.cmp.Type = new CodeTypeReference("System.Boolean");
			this.cmp.Attributes = (MemberAttributes)24582;
			this.cmp.Name = "IsSynchronized";
			this.cmp.ImplementationTypes.Add("System.Collections.ICollection");
			this.cmp.GetStatements.Add(new CodeMethodReturnStatement(new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text), "IsSynchronized")));
			this.ccc.Members.Add(this.cmp);
			this.cmp = new CodeMemberProperty();
			this.cmp.Type = new CodeTypeReference("System.Object");
			this.cmp.Attributes = (MemberAttributes)24582;
			this.cmp.Name = "SyncRoot";
			this.cmp.ImplementationTypes.Add("System.Collections.ICollection");
			this.cmp.GetStatements.Add(new CodeMethodReturnStatement(new CodeThisReferenceExpression()));
			this.ccc.Members.Add(this.cmp);
			string text3 = "array";
			string text4 = "index";
			string text5 = "nCtr";
			this.cmm = new CodeMemberMethod();
			this.cmm.Attributes = (MemberAttributes)24582;
			this.cmm.Name = "CopyTo";
			this.cmm.ImplementationTypes.Add("System.Collections.ICollection");
			this.cpde = new CodeParameterDeclarationExpression();
			this.cpde.Name = text3;
			this.cpde.Type = new CodeTypeReference("System.Array");
			this.cmm.Parameters.Add(this.cpde);
			this.cpde = new CodeParameterDeclarationExpression();
			this.cpde.Name = text4;
			this.cpde.Type = new CodeTypeReference("System.Int32");
			this.cmm.Parameters.Add(this.cpde);
			this.cmie = new CodeMethodInvokeExpression(new CodeVariableReferenceExpression(text), "CopyTo", new CodeExpression[0]);
			this.cmie.Parameters.Add(new CodeVariableReferenceExpression(text3));
			this.cmie.Parameters.Add(new CodeVariableReferenceExpression(text4));
			this.cmm.Statements.Add(new CodeExpressionStatement(this.cmie));
			this.cmm.Statements.Add(new CodeVariableDeclarationStatement("System.Int32", text5));
			this.cfls = new CodeIterationStatement();
			this.cfls.InitStatement = new CodeAssignStatement(new CodeVariableReferenceExpression(text5), new CodePrimitiveExpression(0));
			this.cboe = new CodeBinaryOperatorExpression();
			this.cboe.Left = new CodeVariableReferenceExpression(text5);
			this.cboe.Operator = CodeBinaryOperatorType.LessThan;
			this.cboe.Right = new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text3), "Length");
			this.cfls.TestExpression = this.cboe;
			this.cfls.IncrementStatement = new CodeAssignStatement(new CodeVariableReferenceExpression(text5), new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression(text5), CodeBinaryOperatorType.Add, new CodePrimitiveExpression(1)));
			this.cmie = new CodeMethodInvokeExpression(new CodeVariableReferenceExpression(text3), "SetValue", new CodeExpression[0]);
			CodeMethodInvokeExpression expression = new CodeMethodInvokeExpression(new CodeVariableReferenceExpression(text3), "GetValue", new CodeExpression[]
			{
				new CodeVariableReferenceExpression(text5)
			});
			this.coce = new CodeObjectCreateExpression();
			this.coce.CreateType = new CodeTypeReference(this.PrivateNamesUsed["GeneratedClassName"].ToString());
			this.coce.Parameters.Add(new CodeCastExpression(new CodeTypeReference(this.PublicNamesUsed["LateBoundClass"].ToString()), expression));
			this.cmie.Parameters.Add(this.coce);
			this.cmie.Parameters.Add(new CodeVariableReferenceExpression(text5));
			this.cfls.Statements.Add(new CodeExpressionStatement(this.cmie));
			this.cmm.Statements.Add(this.cfls);
			this.ccc.Members.Add(this.cmm);
			this.cmm = new CodeMemberMethod();
			this.cmm.Attributes = (MemberAttributes)24582;
			this.cmm.Name = "GetEnumerator";
			this.cmm.ImplementationTypes.Add("System.Collections.IEnumerable");
			this.cmm.ReturnType = new CodeTypeReference("System.Collections.IEnumerator");
			this.coce = new CodeObjectCreateExpression();
			this.coce.CreateType = new CodeTypeReference(this.PrivateNamesUsed["EnumeratorClass"].ToString());
			this.coce.Parameters.Add(new CodeMethodInvokeExpression(new CodeVariableReferenceExpression(text), "GetEnumerator", new CodeExpression[0]));
			this.cmm.Statements.Add(new CodeMethodReturnStatement(this.coce));
			this.ccc.Members.Add(this.cmm);
			this.GenerateEnumeratorClass();
			this.ccc.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("COMMENT_ENUMIMPL")));
			this.cc.Members.Add(this.ccc);
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0001A55C File Offset: 0x0001875C
		private void GenerateEnumeratorClass()
		{
			string text = "privObjEnum";
			string str = "ManagementObjectEnumerator";
			string str2 = "ManagementObjectCollection";
			string text2 = "objEnum";
			this.ecc = new CodeTypeDeclaration(this.PrivateNamesUsed["EnumeratorClass"].ToString());
			this.ecc.TypeAttributes = TypeAttributes.NestedPublic;
			this.ecc.BaseTypes.Add("System.Object");
			this.ecc.BaseTypes.Add("System.Collections.IEnumerator");
			this.cf = new CodeMemberField();
			this.cf.Name = text;
			this.cf.Attributes = (MemberAttributes)20482;
			this.cf.Type = new CodeTypeReference(str2 + "." + str);
			this.ecc.Members.Add(this.cf);
			this.cctor = new CodeConstructor();
			this.cctor.Attributes = MemberAttributes.Public;
			this.cpde = new CodeParameterDeclarationExpression();
			this.cpde.Name = text2;
			this.cpde.Type = new CodeTypeReference(str2 + "." + str);
			this.cctor.Parameters.Add(this.cpde);
			this.cctor.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(text), new CodeVariableReferenceExpression(text2)));
			this.ecc.Members.Add(this.cctor);
			this.cmp = new CodeMemberProperty();
			this.cmp.Type = new CodeTypeReference("System.Object");
			this.cmp.Attributes = (MemberAttributes)24582;
			this.cmp.Name = "Current";
			this.cmp.ImplementationTypes.Add("System.Collections.IEnumerator");
			this.coce = new CodeObjectCreateExpression();
			this.coce.CreateType = new CodeTypeReference(this.PrivateNamesUsed["GeneratedClassName"].ToString());
			this.coce.Parameters.Add(new CodeCastExpression(new CodeTypeReference(this.PublicNamesUsed["LateBoundClass"].ToString()), new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text), "Current")));
			this.cmp.GetStatements.Add(new CodeMethodReturnStatement(this.coce));
			this.ecc.Members.Add(this.cmp);
			this.cmm = new CodeMemberMethod();
			this.cmm.Attributes = (MemberAttributes)24582;
			this.cmm.Name = "MoveNext";
			this.cmm.ImplementationTypes.Add("System.Collections.IEnumerator");
			this.cmm.ReturnType = new CodeTypeReference("System.Boolean");
			this.cmie = new CodeMethodInvokeExpression(new CodeVariableReferenceExpression(text), "MoveNext", new CodeExpression[0]);
			this.cmm.Statements.Add(new CodeMethodReturnStatement(this.cmie));
			this.ecc.Members.Add(this.cmm);
			this.cmm = new CodeMemberMethod();
			this.cmm.Attributes = (MemberAttributes)24582;
			this.cmm.Name = "Reset";
			this.cmm.ImplementationTypes.Add("System.Collections.IEnumerator");
			this.cmie = new CodeMethodInvokeExpression(new CodeVariableReferenceExpression(text), "Reset", new CodeExpression[0]);
			this.cmm.Statements.Add(new CodeExpressionStatement(this.cmie));
			this.ecc.Members.Add(this.cmm);
			this.ccc.Members.Add(this.ecc);
		}

		// Token: 0x0600031C RID: 796 RVA: 0x0001A914 File Offset: 0x00018B14
		private int IsContainedIn(string strToFind, ref SortedList sortedList)
		{
			int result = -1;
			for (int i = 0; i < sortedList.Count; i++)
			{
				if (string.Compare(sortedList.GetByIndex(i).ToString(), strToFind, StringComparison.OrdinalIgnoreCase) == 0)
				{
					result = i;
					break;
				}
			}
			return result;
		}

		// Token: 0x0600031D RID: 797 RVA: 0x0001A950 File Offset: 0x00018B50
		private CodeTypeReference ConvertCIMType(CimType cType, bool isArray)
		{
			string text;
			switch (cType)
			{
			case CimType.SInt16:
				text = "System.Int16";
				goto IL_155;
			case CimType.SInt32:
				text = "System.Int32";
				goto IL_155;
			case CimType.Real32:
				text = "System.Single";
				goto IL_155;
			case CimType.Real64:
				text = "System.Double";
				goto IL_155;
			case (CimType)6:
			case (CimType)7:
			case (CimType)9:
			case (CimType)10:
			case (CimType)12:
			case CimType.Object:
			case (CimType)14:
			case (CimType)15:
				break;
			case CimType.String:
				text = "System.String";
				goto IL_155;
			case CimType.Boolean:
				text = "System.Boolean";
				goto IL_155;
			case CimType.SInt8:
				text = "System.SByte";
				goto IL_155;
			case CimType.UInt8:
				text = "System.Byte";
				goto IL_155;
			case CimType.UInt16:
				if (!this.bUnsignedSupported)
				{
					text = "System.Int16";
					goto IL_155;
				}
				text = "System.UInt16";
				goto IL_155;
			case CimType.UInt32:
				if (!this.bUnsignedSupported)
				{
					text = "System.Int32";
					goto IL_155;
				}
				text = "System.UInt32";
				goto IL_155;
			case CimType.SInt64:
				text = "System.Int64";
				goto IL_155;
			case CimType.UInt64:
				if (!this.bUnsignedSupported)
				{
					text = "System.Int64";
					goto IL_155;
				}
				text = "System.UInt64";
				goto IL_155;
			default:
				switch (cType)
				{
				case CimType.DateTime:
					text = "System.DateTime";
					goto IL_155;
				case CimType.Reference:
					text = this.PublicNamesUsed["PathClass"].ToString();
					goto IL_155;
				case CimType.Char16:
					text = "System.Char";
					goto IL_155;
				}
				break;
			}
			text = this.PublicNamesUsed["BaseObjClass"].ToString();
			IL_155:
			if (isArray)
			{
				return new CodeTypeReference(text, 1);
			}
			return new CodeTypeReference(text);
		}

		// Token: 0x0600031E RID: 798 RVA: 0x0001AAC4 File Offset: 0x00018CC4
		private static bool isTypeInt(CimType cType)
		{
			switch (cType)
			{
			case CimType.SInt16:
			case CimType.SInt32:
			case CimType.SInt8:
			case CimType.UInt8:
			case CimType.UInt16:
			case CimType.UInt32:
				return true;
			case CimType.Real32:
			case CimType.Real64:
			case (CimType)6:
			case (CimType)7:
			case CimType.String:
			case (CimType)9:
			case (CimType)10:
			case CimType.Boolean:
			case (CimType)12:
			case CimType.Object:
			case (CimType)14:
			case (CimType)15:
			case CimType.SInt64:
			case CimType.UInt64:
				break;
			default:
				if (cType - CimType.DateTime > 2)
				{
				}
				break;
			}
			return false;
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x0600031F RID: 799 RVA: 0x0001AB39 File Offset: 0x00018D39
		public string GeneratedFileName
		{
			get
			{
				return this.genFileName;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000320 RID: 800 RVA: 0x0001AB41 File Offset: 0x00018D41
		public string GeneratedTypeName
		{
			get
			{
				return this.PrivateNamesUsed["GeneratedNamespace"].ToString() + "." + this.PrivateNamesUsed["GeneratedClassName"].ToString();
			}
		}

		// Token: 0x06000321 RID: 801 RVA: 0x0001AB78 File Offset: 0x00018D78
		private static string ConvertValuesToName(string str)
		{
			string text = string.Empty;
			string text2 = "_";
			string text3 = string.Empty;
			if (str.Length == 0)
			{
				return string.Copy("");
			}
			char[] array = str.ToCharArray();
			if (!char.IsLetter(array[0]))
			{
				text = "Val_";
				text3 = "l";
			}
			for (int i = 0; i < str.Length; i++)
			{
				bool flag = true;
				if (!char.IsLetterOrDigit(array[i]))
				{
					if (text3 == text2)
					{
						flag = false;
					}
					else
					{
						text3 = text2;
					}
				}
				else
				{
					text3 = new string(array[i], 1);
				}
				if (flag)
				{
					text += text3;
				}
			}
			return text;
		}

		// Token: 0x06000322 RID: 802 RVA: 0x0001AC18 File Offset: 0x00018E18
		private void ResolveEnumNameValues(ArrayList arrIn, ref ArrayList arrayOut)
		{
			arrayOut.Clear();
			string text = string.Empty;
			IFormatProvider provider = (IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(int));
			for (int i = 0; i < arrIn.Count; i++)
			{
				text = arrIn[i].ToString();
				text = this.ResolveCollision(text, true);
				if (ManagementClassGenerator.IsContainedInArray(text, arrayOut))
				{
					int num = 0;
					text = arrIn[i].ToString() + num.ToString(provider);
					while (ManagementClassGenerator.IsContainedInArray(text, arrayOut))
					{
						num++;
						text = arrIn[i].ToString() + num.ToString(provider);
					}
				}
				arrayOut.Add(text);
			}
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0001ACD0 File Offset: 0x00018ED0
		private static bool IsContainedInArray(string strToFind, ArrayList arrToSearch)
		{
			for (int i = 0; i < arrToSearch.Count; i++)
			{
				if (string.Compare(arrToSearch[i].ToString(), strToFind, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0001AD08 File Offset: 0x00018F08
		private bool InitializeCodeGenerator(CodeLanguage lang)
		{
			string arg = "";
			bool flag = true;
			try
			{
				switch (lang)
				{
				case CodeLanguage.CSharp:
					arg = "C#.";
					this.cp = new CSharpCodeProvider();
					break;
				case CodeLanguage.JScript:
					arg = "JScript.NET.";
					this.cp = new JScriptCodeProvider();
					break;
				case CodeLanguage.VB:
					arg = "Visual Basic.";
					this.cp = new VBCodeProvider();
					break;
				case CodeLanguage.VJSharp:
				{
					arg = "Visual J#.";
					flag = false;
					AssemblyName name = Assembly.GetExecutingAssembly().GetName();
					AssemblyName assemblyName = new AssemblyName();
					assemblyName.CultureInfo = new CultureInfo("");
					assemblyName.Name = "VJSharpCodeProvider";
					assemblyName.SetPublicKey(name.GetPublicKey());
					assemblyName.Version = name.Version;
					Assembly assembly = Assembly.Load(assemblyName);
					if (assembly != null)
					{
						Type type = assembly.GetType("Microsoft.VJSharp.VJSharpCodeProvider");
						if (type != null)
						{
							this.cp = (CodeDomProvider)Activator.CreateInstance(type);
							flag = true;
						}
					}
					break;
				}
				case CodeLanguage.Mcpp:
				{
					arg = "Managed C++.";
					flag = false;
					AssemblyName name = Assembly.GetExecutingAssembly().GetName();
					AssemblyName assemblyName = new AssemblyName();
					assemblyName.CultureInfo = new CultureInfo("");
					assemblyName.SetPublicKey(name.GetPublicKey());
					assemblyName.Name = "CppCodeProvider";
					assemblyName.Version = new Version(this.VSVERSION);
					Assembly assembly = Assembly.Load(assemblyName);
					if (assembly != null)
					{
						Type type = assembly.GetType("Microsoft.VisualC.CppCodeProvider");
						if (type != null)
						{
							this.cp = (CodeDomProvider)Activator.CreateInstance(type);
							flag = true;
						}
					}
					break;
				}
				}
			}
			catch
			{
				throw new ArgumentOutOfRangeException(string.Format(ManagementClassGenerator.GetString("UNABLE_TOCREATE_GEN_EXCEPT"), arg));
			}
			if (flag)
			{
				this.GetUnsignedSupport(lang);
				return true;
			}
			throw new ArgumentOutOfRangeException(string.Format(ManagementClassGenerator.GetString("UNABLE_TOCREATE_GEN_EXCEPT"), arg));
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0001AF0C File Offset: 0x0001910C
		private void GetUnsignedSupport(CodeLanguage Language)
		{
			if (Language != CodeLanguage.CSharp)
			{
				int num = Language - CodeLanguage.JScript;
				return;
			}
			this.bUnsignedSupported = true;
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0001AF20 File Offset: 0x00019120
		private void GenerateCommitMethod()
		{
			this.cmm = new CodeMemberMethod();
			this.cmm.Name = this.PublicNamesUsed["CommitMethod"].ToString();
			this.cmm.Attributes = (MemberAttributes)24578;
			this.caa = new CodeAttributeArgument();
			this.caa.Value = new CodePrimitiveExpression(true);
			this.cad = new CodeAttributeDeclaration();
			this.cad.Name = "Browsable";
			this.cad.Arguments.Add(this.caa);
			this.cmm.CustomAttributes = new CodeAttributeDeclarationCollection();
			this.cmm.CustomAttributes.Add(this.cad);
			this.cis = new CodeConditionStatement();
			this.cboe = new CodeBinaryOperatorExpression();
			this.cboe.Left = new CodeVariableReferenceExpression(this.PrivateNamesUsed["IsEmbedded"].ToString());
			this.cboe.Right = new CodePrimitiveExpression(false);
			this.cboe.Operator = CodeBinaryOperatorType.ValueEquality;
			this.cis.Condition = this.cboe;
			this.cmie = new CodeMethodInvokeExpression();
			this.cmie.Method.TargetObject = new CodeVariableReferenceExpression(this.PrivateNamesUsed["LateBoundObject"].ToString());
			this.cmie.Method.MethodName = "Put";
			this.cis.TrueStatements.Add(new CodeExpressionStatement(this.cmie));
			this.cmm.Statements.Add(this.cis);
			this.cc.Members.Add(this.cmm);
			this.cmm = new CodeMemberMethod();
			this.cmm.Name = this.PublicNamesUsed["CommitMethod"].ToString();
			this.cmm.Attributes = (MemberAttributes)24578;
			CodeParameterDeclarationExpression codeParameterDeclarationExpression = new CodeParameterDeclarationExpression();
			codeParameterDeclarationExpression.Type = new CodeTypeReference(this.PublicNamesUsed["PutOptions"].ToString());
			codeParameterDeclarationExpression.Name = this.PrivateNamesUsed["putOptions"].ToString();
			this.cmm.Parameters.Add(codeParameterDeclarationExpression);
			this.caa = new CodeAttributeArgument();
			this.caa.Value = new CodePrimitiveExpression(true);
			this.cad = new CodeAttributeDeclaration();
			this.cad.Name = "Browsable";
			this.cad.Arguments.Add(this.caa);
			this.cmm.CustomAttributes = new CodeAttributeDeclarationCollection();
			this.cmm.CustomAttributes.Add(this.cad);
			this.cis = new CodeConditionStatement();
			this.cboe = new CodeBinaryOperatorExpression();
			this.cboe.Left = new CodeVariableReferenceExpression(this.PrivateNamesUsed["IsEmbedded"].ToString());
			this.cboe.Right = new CodePrimitiveExpression(false);
			this.cboe.Operator = CodeBinaryOperatorType.ValueEquality;
			this.cis.Condition = this.cboe;
			this.cmie = new CodeMethodInvokeExpression();
			this.cmie.Method.TargetObject = new CodeVariableReferenceExpression(this.PrivateNamesUsed["LateBoundObject"].ToString());
			this.cmie.Method.MethodName = "Put";
			this.cmie.Parameters.Add(new CodeVariableReferenceExpression(this.PrivateNamesUsed["putOptions"].ToString()));
			this.cis.TrueStatements.Add(new CodeExpressionStatement(this.cmie));
			this.cmm.Statements.Add(this.cis);
			this.cc.Members.Add(this.cmm);
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0001B31C File Offset: 0x0001951C
		private static int ConvertBitMapValueToInt32(string bitMap)
		{
			string text = "0x";
			int result;
			if (bitMap.StartsWith(text, StringComparison.Ordinal) || bitMap.StartsWith(text.ToUpper(CultureInfo.InvariantCulture), StringComparison.Ordinal))
			{
				text = string.Empty;
				char[] array = bitMap.ToCharArray();
				int length = bitMap.Length;
				for (int i = 2; i < length; i++)
				{
					text += array[i].ToString();
				}
				result = System.Convert.ToInt32(text, (IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(int)));
			}
			else
			{
				result = System.Convert.ToInt32(bitMap, (IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(int)));
			}
			return result;
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0001B3CC File Offset: 0x000195CC
		private string GetConversionFunction(CimType cimType)
		{
			string result = string.Empty;
			switch (cimType)
			{
			case CimType.SInt16:
				result = "ToInt16";
				break;
			case CimType.SInt32:
				result = "ToInt32";
				break;
			case CimType.Real32:
				result = "ToSingle";
				break;
			case CimType.Real64:
				result = "ToDouble";
				break;
			case (CimType)6:
			case (CimType)7:
			case (CimType)9:
			case (CimType)10:
			case (CimType)12:
			case CimType.Object:
			case (CimType)14:
			case (CimType)15:
				break;
			case CimType.String:
				result = "ToString";
				break;
			case CimType.Boolean:
				result = "ToBoolean";
				break;
			case CimType.SInt8:
				result = "ToSByte";
				break;
			case CimType.UInt8:
				result = "ToByte";
				break;
			case CimType.UInt16:
				if (!this.bUnsignedSupported)
				{
					result = "ToInt16";
				}
				else
				{
					result = "ToUInt16";
				}
				break;
			case CimType.UInt32:
				if (!this.bUnsignedSupported)
				{
					result = "ToInt32";
				}
				else
				{
					result = "ToUInt32";
				}
				break;
			case CimType.SInt64:
				result = "ToInt64";
				break;
			case CimType.UInt64:
				if (!this.bUnsignedSupported)
				{
					result = "ToInt64";
				}
				else
				{
					result = "ToUInt64";
				}
				break;
			default:
				if (cimType == CimType.Char16)
				{
					result = "ToChar";
				}
				break;
			}
			return result;
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0001B4E1 File Offset: 0x000196E1
		private static bool IsDesignerSerializationVisibilityToBeSet(string propName)
		{
			return string.Compare(propName, "Path", StringComparison.OrdinalIgnoreCase) != 0;
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0001B4F4 File Offset: 0x000196F4
		private static bool IsPropertyValueType(CimType cType)
		{
			bool result = true;
			if (cType == CimType.String || cType == CimType.Object || cType == CimType.Reference)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0001B514 File Offset: 0x00019714
		private bool IsDynamicClass()
		{
			bool result = false;
			try
			{
				result = System.Convert.ToBoolean(this.classobj.Qualifiers["dynamic"].Value, (IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(bool)));
			}
			catch (ManagementException)
			{
			}
			return result;
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0001B574 File Offset: 0x00019774
		private static string ConvertToNumericValueAndAddToArray(CimType cimType, string numericValue, ArrayList arrayToAdd, out string enumType)
		{
			string result = string.Empty;
			enumType = string.Empty;
			if (cimType - CimType.SInt16 > 1 && cimType - CimType.SInt8 > 2)
			{
				if (cimType == CimType.UInt32)
				{
					arrayToAdd.Add(System.Convert.ToInt32(numericValue, (IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(int))));
					result = "ToInt32";
					enumType = "System.Int32";
				}
			}
			else
			{
				arrayToAdd.Add(System.Convert.ToInt32(numericValue, (IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(int))));
				result = "ToInt32";
				enumType = "System.Int32";
			}
			return result;
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0001B618 File Offset: 0x00019818
		private void AddClassComments(CodeTypeDeclaration cc)
		{
			cc.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("COMMENT_SHOULDSERIALIZE")));
			cc.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("COMMENT_ISPROPNULL")));
			cc.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("COMMENT_RESETPROP")));
			cc.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("COMMENT_ATTRIBPROP")));
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0001B691 File Offset: 0x00019891
		private static string GetString(string strToGet)
		{
			return RC.GetString(strToGet);
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0001B69C File Offset: 0x0001989C
		private void GenerateClassNameProperty()
		{
			string text = "strRet";
			this.cmp = new CodeMemberProperty();
			this.cmp.Name = this.PublicNamesUsed["ClassNameProperty"].ToString();
			this.cmp.Attributes = (MemberAttributes)24578;
			this.cmp.Type = new CodeTypeReference("System.String");
			this.caa = new CodeAttributeArgument();
			this.caa.Value = new CodePrimitiveExpression(true);
			this.cad = new CodeAttributeDeclaration();
			this.cad.Name = "Browsable";
			this.cad.Arguments.Add(this.caa);
			this.cmp.CustomAttributes = new CodeAttributeDeclarationCollection();
			this.cmp.CustomAttributes.Add(this.cad);
			this.caa = new CodeAttributeArgument();
			this.caa.Value = new CodeFieldReferenceExpression(new CodeTypeReferenceExpression("DesignerSerializationVisibility"), "Hidden");
			this.cad = new CodeAttributeDeclaration();
			this.cad.Name = "DesignerSerializationVisibility";
			this.cad.Arguments.Add(this.caa);
			this.cmp.CustomAttributes.Add(this.cad);
			this.cmp.GetStatements.Add(new CodeVariableDeclarationStatement("System.String", text, new CodeVariableReferenceExpression(this.PrivateNamesUsed["CreationClassName"].ToString())));
			this.cis = new CodeConditionStatement();
			this.cboe = new CodeBinaryOperatorExpression();
			this.cboe.Left = new CodeVariableReferenceExpression(this.PrivateNamesUsed["CurrentObject"].ToString());
			this.cboe.Right = new CodePrimitiveExpression(null);
			this.cboe.Operator = CodeBinaryOperatorType.IdentityInequality;
			this.cis.Condition = this.cboe;
			CodeConditionStatement codeConditionStatement = new CodeConditionStatement();
			codeConditionStatement.Condition = new CodeBinaryOperatorExpression
			{
				Left = new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(this.PrivateNamesUsed["CurrentObject"].ToString()), this.PublicNamesUsed["ClassPathProperty"].ToString()),
				Right = new CodePrimitiveExpression(null),
				Operator = CodeBinaryOperatorType.IdentityInequality
			};
			this.cis.TrueStatements.Add(codeConditionStatement);
			codeConditionStatement.TrueStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(text), new CodeCastExpression(new CodeTypeReference("System.String"), new CodeIndexerExpression(new CodeVariableReferenceExpression(this.PrivateNamesUsed["CurrentObject"].ToString()), new CodeExpression[]
			{
				new CodePrimitiveExpression("__CLASS")
			}))));
			CodeConditionStatement codeConditionStatement2 = new CodeConditionStatement();
			CodeBinaryOperatorExpression codeBinaryOperatorExpression = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression.Left = new CodeVariableReferenceExpression(text);
			codeBinaryOperatorExpression.Right = new CodePrimitiveExpression(null);
			codeBinaryOperatorExpression.Operator = CodeBinaryOperatorType.IdentityEquality;
			CodeBinaryOperatorExpression codeBinaryOperatorExpression2 = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression2.Left = new CodeVariableReferenceExpression(text);
			codeBinaryOperatorExpression2.Right = new CodeFieldReferenceExpression(new CodeTypeReferenceExpression("System.String"), "Empty");
			codeBinaryOperatorExpression2.Operator = CodeBinaryOperatorType.IdentityEquality;
			codeConditionStatement2.Condition = new CodeBinaryOperatorExpression
			{
				Left = codeBinaryOperatorExpression,
				Right = codeBinaryOperatorExpression2,
				Operator = CodeBinaryOperatorType.BooleanOr
			};
			codeConditionStatement2.TrueStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(text), new CodeVariableReferenceExpression(this.PrivateNamesUsed["CreationClassName"].ToString())));
			codeConditionStatement.TrueStatements.Add(codeConditionStatement2);
			this.cmp.GetStatements.Add(this.cis);
			this.cmp.GetStatements.Add(new CodeMethodReturnStatement(new CodeVariableReferenceExpression(text)));
			this.cc.Members.Add(this.cmp);
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0001BA70 File Offset: 0x00019C70
		private void GenerateIfClassvalidFuncWithAllParams()
		{
			string text = "path";
			string text2 = "OptionsParam";
			this.cmm = new CodeMemberMethod();
			this.cmm.Name = this.PrivateNamesUsed["ClassNameCheckFunc"].ToString();
			this.cmm.Attributes = (MemberAttributes)20482;
			this.cmm.ReturnType = new CodeTypeReference("System.Boolean");
			this.cmm.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(this.PublicNamesUsed["ScopeClass"].ToString()), this.PrivateNamesUsed["ScopeParam"].ToString()));
			this.cmm.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(this.PublicNamesUsed["PathClass"].ToString()), text));
			this.cmm.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(this.PublicNamesUsed["GetOptionsClass"].ToString()), text2));
			CodeExpression[] parameters = new CodeExpression[]
			{
				new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text), "ClassName"),
				new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), this.PublicNamesUsed["ClassNameProperty"].ToString()),
				new CodePrimitiveExpression(true),
				new CodePropertyReferenceExpression(new CodeTypeReferenceExpression("System.Globalization.CultureInfo"), "InvariantCulture")
			};
			this.cmie = new CodeMethodInvokeExpression(new CodeTypeReferenceExpression("System.String"), "Compare", parameters);
			this.cboe = new CodeBinaryOperatorExpression();
			this.cboe.Left = this.cmie;
			this.cboe.Right = new CodePrimitiveExpression(0);
			this.cboe.Operator = CodeBinaryOperatorType.ValueEquality;
			CodeBinaryOperatorExpression codeBinaryOperatorExpression = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression.Left = new CodeVariableReferenceExpression(text);
			codeBinaryOperatorExpression.Right = new CodePrimitiveExpression(null);
			codeBinaryOperatorExpression.Operator = CodeBinaryOperatorType.IdentityInequality;
			CodeBinaryOperatorExpression codeBinaryOperatorExpression2 = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression2.Left = codeBinaryOperatorExpression;
			codeBinaryOperatorExpression2.Right = this.cboe;
			codeBinaryOperatorExpression2.Operator = CodeBinaryOperatorType.BooleanAnd;
			this.cis = new CodeConditionStatement();
			this.cis.Condition = codeBinaryOperatorExpression2;
			this.cis.TrueStatements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(true)));
			this.coce = new CodeObjectCreateExpression();
			this.coce.CreateType = new CodeTypeReference(this.PublicNamesUsed["LateBoundClass"].ToString());
			this.coce.Parameters.Add(new CodeVariableReferenceExpression(this.PrivateNamesUsed["ScopeParam"].ToString()));
			this.coce.Parameters.Add(new CodeVariableReferenceExpression(text));
			this.coce.Parameters.Add(new CodeVariableReferenceExpression(text2));
			CodeMethodReferenceExpression codeMethodReferenceExpression = new CodeMethodReferenceExpression();
			codeMethodReferenceExpression.MethodName = this.PrivateNamesUsed["ClassNameCheckFunc"].ToString();
			this.cis.FalseStatements.Add(new CodeMethodReturnStatement(new CodeMethodInvokeExpression(codeMethodReferenceExpression, new CodeExpression[]
			{
				this.coce
			})));
			this.cmm.Statements.Add(this.cis);
			this.cc.Members.Add(this.cmm);
		}

		// Token: 0x06000331 RID: 817 RVA: 0x0001BDC4 File Offset: 0x00019FC4
		private void GenerateIfClassvalidFunction()
		{
			this.GenerateIfClassvalidFuncWithAllParams();
			string text = "theObj";
			string text2 = "count";
			string text3 = "parentClasses";
			this.cmm = new CodeMemberMethod();
			this.cmm.Name = this.PrivateNamesUsed["ClassNameCheckFunc"].ToString();
			this.cmm.Attributes = (MemberAttributes)20482;
			this.cmm.ReturnType = new CodeTypeReference("System.Boolean");
			this.cmm.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(this.PublicNamesUsed["BaseObjClass"].ToString()), text));
			CodeExpression[] parameters = new CodeExpression[]
			{
				new CodeCastExpression(new CodeTypeReference("System.String"), new CodeIndexerExpression(new CodeVariableReferenceExpression(text), new CodeExpression[]
				{
					new CodePrimitiveExpression("__CLASS")
				})),
				new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), this.PublicNamesUsed["ClassNameProperty"].ToString()),
				new CodePrimitiveExpression(true),
				new CodePropertyReferenceExpression(new CodeTypeReferenceExpression("System.Globalization.CultureInfo"), "InvariantCulture")
			};
			this.cmie = new CodeMethodInvokeExpression(new CodeTypeReferenceExpression("System.String"), "Compare", parameters);
			this.cboe = new CodeBinaryOperatorExpression();
			this.cboe.Left = this.cmie;
			this.cboe.Right = new CodePrimitiveExpression(0);
			this.cboe.Operator = CodeBinaryOperatorType.ValueEquality;
			CodeBinaryOperatorExpression codeBinaryOperatorExpression = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression.Left = new CodeVariableReferenceExpression(text);
			codeBinaryOperatorExpression.Right = new CodePrimitiveExpression(null);
			codeBinaryOperatorExpression.Operator = CodeBinaryOperatorType.IdentityInequality;
			CodeBinaryOperatorExpression codeBinaryOperatorExpression2 = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression2.Left = codeBinaryOperatorExpression;
			codeBinaryOperatorExpression2.Right = this.cboe;
			codeBinaryOperatorExpression2.Operator = CodeBinaryOperatorType.BooleanAnd;
			this.cis = new CodeConditionStatement();
			this.cis.Condition = codeBinaryOperatorExpression2;
			this.cis.TrueStatements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(true)));
			CodeExpression initExpression = new CodeCastExpression(new CodeTypeReference("System.Array"), new CodeIndexerExpression(new CodeVariableReferenceExpression(text), new CodeExpression[]
			{
				new CodePrimitiveExpression("__DERIVATION")
			}));
			this.cis.FalseStatements.Add(new CodeVariableDeclarationStatement("System.Array", text3, initExpression));
			CodeConditionStatement codeConditionStatement = new CodeConditionStatement();
			this.cboe = new CodeBinaryOperatorExpression();
			this.cboe.Left = new CodeVariableReferenceExpression(text3);
			this.cboe.Right = new CodePrimitiveExpression(null);
			this.cboe.Operator = CodeBinaryOperatorType.IdentityInequality;
			codeConditionStatement.Condition = this.cboe;
			this.cfls = new CodeIterationStatement();
			codeConditionStatement.TrueStatements.Add(new CodeVariableDeclarationStatement("System.Int32", text2, new CodePrimitiveExpression(0)));
			this.cfls.InitStatement = new CodeAssignStatement(new CodeVariableReferenceExpression(text2), new CodePrimitiveExpression(0));
			this.cboe = new CodeBinaryOperatorExpression();
			this.cboe.Left = new CodeVariableReferenceExpression(text2);
			this.cboe.Operator = CodeBinaryOperatorType.LessThan;
			this.cboe.Right = new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text3), "Length");
			this.cfls.TestExpression = this.cboe;
			this.cfls.IncrementStatement = new CodeAssignStatement(new CodeVariableReferenceExpression(text2), new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression(text2), CodeBinaryOperatorType.Add, new CodePrimitiveExpression(1)));
			CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method.MethodName = "GetValue";
			codeMethodInvokeExpression.Method.TargetObject = new CodeVariableReferenceExpression(text3);
			codeMethodInvokeExpression.Parameters.Add(new CodeVariableReferenceExpression(text2));
			CodeExpression[] parameters2 = new CodeExpression[]
			{
				new CodeCastExpression(new CodeTypeReference("System.String"), codeMethodInvokeExpression),
				new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), this.PublicNamesUsed["ClassNameProperty"].ToString()),
				new CodePrimitiveExpression(true),
				new CodePropertyReferenceExpression(new CodeTypeReferenceExpression("System.Globalization.CultureInfo"), "InvariantCulture")
			};
			CodeMethodInvokeExpression left = new CodeMethodInvokeExpression(new CodeTypeReferenceExpression("System.String"), "Compare", parameters2);
			CodeConditionStatement codeConditionStatement2 = new CodeConditionStatement();
			this.cboe = new CodeBinaryOperatorExpression();
			this.cboe.Left = left;
			this.cboe.Right = new CodePrimitiveExpression(0);
			this.cboe.Operator = CodeBinaryOperatorType.ValueEquality;
			codeConditionStatement2.Condition = this.cboe;
			codeConditionStatement2.TrueStatements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(true)));
			codeConditionStatement.TrueStatements.Add(this.cfls);
			this.cfls.Statements.Add(codeConditionStatement2);
			this.cis.FalseStatements.Add(codeConditionStatement);
			this.cmm.Statements.Add(this.cis);
			this.cmm.Statements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(false)));
			this.cc.Members.Add(this.cmm);
		}

		// Token: 0x06000332 RID: 818 RVA: 0x0001C2E0 File Offset: 0x0001A4E0
		private void GenerateCodeForRefAndDateTimeTypes(CodeIndexerExpression prop, bool bArray, CodeStatementCollection statColl, string strType, CodeVariableReferenceExpression varToAssign, bool bIsValueProprequired)
		{
			if (!bArray)
			{
				CodeConditionStatement codeConditionStatement = new CodeConditionStatement();
				codeConditionStatement.Condition = new CodeBinaryOperatorExpression
				{
					Left = prop,
					Operator = CodeBinaryOperatorType.IdentityInequality,
					Right = new CodePrimitiveExpression(null)
				};
				if (string.Compare(strType, this.PublicNamesUsed["PathClass"].ToString(), StringComparison.OrdinalIgnoreCase) == 0)
				{
					CodeMethodReferenceExpression codeMethodReferenceExpression = new CodeMethodReferenceExpression();
					codeMethodReferenceExpression.MethodName = "ToString";
					codeMethodReferenceExpression.TargetObject = prop;
					this.cmie = new CodeMethodInvokeExpression();
					this.cmie.Method = codeMethodReferenceExpression;
					if (varToAssign == null)
					{
						codeConditionStatement.TrueStatements.Add(new CodeMethodReturnStatement(this.CreateObjectForProperty(strType, this.cmie)));
						statColl.Add(codeConditionStatement);
						statColl.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(null)));
						return;
					}
					statColl.Add(new CodeAssignStatement(varToAssign, new CodePrimitiveExpression(null)));
					codeConditionStatement.TrueStatements.Add(new CodeAssignStatement(varToAssign, this.CreateObjectForProperty(strType, this.cmie)));
					statColl.Add(codeConditionStatement);
					return;
				}
				else
				{
					statColl.Add(codeConditionStatement);
					CodeExpression param;
					if (bIsValueProprequired)
					{
						param = new CodeCastExpression(new CodeTypeReference("System.String"), new CodePropertyReferenceExpression(prop, "Value"));
					}
					else
					{
						param = new CodeCastExpression(new CodeTypeReference("System.String"), prop);
					}
					if (varToAssign == null)
					{
						codeConditionStatement.TrueStatements.Add(new CodeMethodReturnStatement(this.CreateObjectForProperty(strType, param)));
						codeConditionStatement.FalseStatements.Add(new CodeMethodReturnStatement(this.CreateObjectForProperty(strType, null)));
						return;
					}
					codeConditionStatement.TrueStatements.Add(new CodeAssignStatement(varToAssign, this.CreateObjectForProperty(strType, param)));
					codeConditionStatement.FalseStatements.Add(new CodeAssignStatement(varToAssign, this.CreateObjectForProperty(strType, null)));
					return;
				}
			}
			else
			{
				string text = "len";
				string text2 = "iCounter";
				string text3 = "arrToRet";
				CodeConditionStatement codeConditionStatement2 = new CodeConditionStatement();
				codeConditionStatement2.Condition = new CodeBinaryOperatorExpression
				{
					Left = prop,
					Operator = CodeBinaryOperatorType.IdentityInequality,
					Right = new CodePrimitiveExpression(null)
				};
				CodePropertyReferenceExpression initExpression;
				if (bIsValueProprequired)
				{
					initExpression = new CodePropertyReferenceExpression(new CodeCastExpression(new CodeTypeReference("System.Array"), new CodePropertyReferenceExpression(prop, "Value")), "Length");
				}
				else
				{
					initExpression = new CodePropertyReferenceExpression(new CodeCastExpression(new CodeTypeReference("System.Array"), prop), "Length");
				}
				codeConditionStatement2.TrueStatements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference("System.Int32"), text, initExpression));
				CodeTypeReference type = new CodeTypeReference(new CodeTypeReference(strType), 1);
				codeConditionStatement2.TrueStatements.Add(new CodeVariableDeclarationStatement(type, text3, new CodeArrayCreateExpression(new CodeTypeReference(strType), new CodeVariableReferenceExpression(text))));
				this.cfls = new CodeIterationStatement();
				this.cfls.InitStatement = new CodeVariableDeclarationStatement(new CodeTypeReference("System.Int32"), text2, new CodePrimitiveExpression(0));
				CodeBinaryOperatorExpression codeBinaryOperatorExpression = new CodeBinaryOperatorExpression();
				codeBinaryOperatorExpression.Left = new CodeVariableReferenceExpression(text2);
				codeBinaryOperatorExpression.Operator = CodeBinaryOperatorType.LessThan;
				codeBinaryOperatorExpression.Right = new CodeVariableReferenceExpression(text);
				this.cfls.TestExpression = codeBinaryOperatorExpression;
				this.cfls.IncrementStatement = new CodeAssignStatement(new CodeVariableReferenceExpression(text2), new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression(text2), CodeBinaryOperatorType.Add, new CodePrimitiveExpression(1)));
				CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression();
				codeMethodInvokeExpression.Method.MethodName = "GetValue";
				if (bIsValueProprequired)
				{
					codeMethodInvokeExpression.Method.TargetObject = new CodeCastExpression(new CodeTypeReference("System.Array"), new CodePropertyReferenceExpression(prop, "Value"));
				}
				else
				{
					codeMethodInvokeExpression.Method.TargetObject = new CodeCastExpression(new CodeTypeReference("System.Array"), prop);
				}
				codeMethodInvokeExpression.Parameters.Add(new CodeVariableReferenceExpression(text2));
				CodeMethodInvokeExpression codeMethodInvokeExpression2 = new CodeMethodInvokeExpression();
				codeMethodInvokeExpression2.Method.MethodName = "ToString";
				codeMethodInvokeExpression2.Method.TargetObject = codeMethodInvokeExpression;
				this.cfls.Statements.Add(new CodeAssignStatement(new CodeIndexerExpression(new CodeVariableReferenceExpression(text3), new CodeExpression[]
				{
					new CodeVariableReferenceExpression(text2)
				}), this.CreateObjectForProperty(strType, codeMethodInvokeExpression2)));
				codeConditionStatement2.TrueStatements.Add(this.cfls);
				if (varToAssign == null)
				{
					codeConditionStatement2.TrueStatements.Add(new CodeMethodReturnStatement(new CodeVariableReferenceExpression(text3)));
					statColl.Add(codeConditionStatement2);
					statColl.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(null)));
					return;
				}
				statColl.Add(new CodeAssignStatement(varToAssign, new CodePrimitiveExpression(null)));
				codeConditionStatement2.TrueStatements.Add(new CodeAssignStatement(varToAssign, new CodeVariableReferenceExpression(text3)));
				statColl.Add(codeConditionStatement2);
				return;
			}
		}

		// Token: 0x06000333 RID: 819 RVA: 0x0001C77C File Offset: 0x0001A97C
		private void AddPropertySet(CodeIndexerExpression prop, bool bArray, CodeStatementCollection statColl, string strType, CodeVariableReferenceExpression varValue)
		{
			if (varValue == null)
			{
				varValue = new CodeVariableReferenceExpression("value");
			}
			if (!bArray)
			{
				statColl.Add(new CodeAssignStatement(prop, this.ConvertPropertyToString(strType, varValue)));
				return;
			}
			string text = "len";
			string text2 = "iCounter";
			string text3 = "arrProp";
			CodeConditionStatement codeConditionStatement = new CodeConditionStatement();
			codeConditionStatement.Condition = new CodeBinaryOperatorExpression
			{
				Left = varValue,
				Operator = CodeBinaryOperatorType.IdentityInequality,
				Right = new CodePrimitiveExpression(null)
			};
			CodePropertyReferenceExpression initExpression = new CodePropertyReferenceExpression(new CodeCastExpression(new CodeTypeReference("System.Array"), varValue), "Length");
			codeConditionStatement.TrueStatements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference("System.Int32"), text, initExpression));
			CodeTypeReference type = new CodeTypeReference(new CodeTypeReference("System.String"), 1);
			codeConditionStatement.TrueStatements.Add(new CodeVariableDeclarationStatement(type, text3, new CodeArrayCreateExpression(new CodeTypeReference("System.String"), new CodeVariableReferenceExpression(text))));
			this.cfls = new CodeIterationStatement();
			this.cfls.InitStatement = new CodeVariableDeclarationStatement(new CodeTypeReference("System.Int32"), text2, new CodePrimitiveExpression(0));
			CodeBinaryOperatorExpression codeBinaryOperatorExpression = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression.Left = new CodeVariableReferenceExpression(text2);
			codeBinaryOperatorExpression.Operator = CodeBinaryOperatorType.LessThan;
			codeBinaryOperatorExpression.Right = new CodeVariableReferenceExpression(text);
			this.cfls.TestExpression = codeBinaryOperatorExpression;
			this.cfls.IncrementStatement = new CodeAssignStatement(new CodeVariableReferenceExpression(text2), new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression(text2), CodeBinaryOperatorType.Add, new CodePrimitiveExpression(1)));
			CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method.MethodName = "GetValue";
			codeMethodInvokeExpression.Method.TargetObject = new CodeCastExpression(new CodeTypeReference("System.Array"), varValue);
			codeMethodInvokeExpression.Parameters.Add(new CodeVariableReferenceExpression(text2));
			this.cfls.Statements.Add(new CodeAssignStatement(new CodeIndexerExpression(new CodeVariableReferenceExpression(text3), new CodeExpression[]
			{
				new CodeVariableReferenceExpression(text2)
			}), this.ConvertPropertyToString(strType, codeMethodInvokeExpression)));
			codeConditionStatement.TrueStatements.Add(this.cfls);
			codeConditionStatement.TrueStatements.Add(new CodeAssignStatement(prop, new CodeVariableReferenceExpression(text3)));
			codeConditionStatement.FalseStatements.Add(new CodeAssignStatement(prop, new CodePrimitiveExpression(null)));
			statColl.Add(codeConditionStatement);
		}

		// Token: 0x06000334 RID: 820 RVA: 0x0001C9D0 File Offset: 0x0001ABD0
		private CodeExpression CreateObjectForProperty(string strType, CodeExpression param)
		{
			if (!(strType == "System.DateTime"))
			{
				if (!(strType == "System.TimeSpan"))
				{
					if (!(strType == "System.Management.ManagementPath"))
					{
						return null;
					}
					this.coce = new CodeObjectCreateExpression();
					this.coce.CreateType = new CodeTypeReference(this.PublicNamesUsed["PathClass"].ToString());
					this.coce.Parameters.Add(param);
					return this.coce;
				}
				else
				{
					if (param == null)
					{
						this.coce = new CodeObjectCreateExpression();
						this.coce.CreateType = new CodeTypeReference("System.TimeSpan");
						this.coce.Parameters.Add(new CodePrimitiveExpression(0));
						this.coce.Parameters.Add(new CodePrimitiveExpression(0));
						this.coce.Parameters.Add(new CodePrimitiveExpression(0));
						this.coce.Parameters.Add(new CodePrimitiveExpression(0));
						this.coce.Parameters.Add(new CodePrimitiveExpression(0));
						return this.coce;
					}
					this.cmie = new CodeMethodInvokeExpression();
					this.cmie.Parameters.Add(param);
					this.cmie.Method.MethodName = this.PrivateNamesUsed["ToTimeSpanMethod"].ToString();
					return this.cmie;
				}
			}
			else
			{
				if (param == null)
				{
					return new CodeFieldReferenceExpression(new CodeTypeReferenceExpression("System.DateTime"), "MinValue");
				}
				this.cmie = new CodeMethodInvokeExpression();
				this.cmie.Parameters.Add(param);
				this.cmie.Method.MethodName = this.PrivateNamesUsed["ToDateTimeMethod"].ToString();
				return this.cmie;
			}
		}

		// Token: 0x06000335 RID: 821 RVA: 0x0001CBBC File Offset: 0x0001ADBC
		private CodeExpression ConvertPropertyToString(string strType, CodeExpression beginingExpression)
		{
			if (strType == "System.DateTime")
			{
				return new CodeMethodInvokeExpression
				{
					Parameters = 
					{
						new CodeCastExpression(new CodeTypeReference("System.DateTime"), beginingExpression)
					},
					Method = 
					{
						MethodName = this.PrivateNamesUsed["ToDMTFDateTimeMethod"].ToString()
					}
				};
			}
			if (strType == "System.TimeSpan")
			{
				return new CodeMethodInvokeExpression
				{
					Parameters = 
					{
						new CodeCastExpression(new CodeTypeReference("System.TimeSpan"), beginingExpression)
					},
					Method = 
					{
						MethodName = this.PrivateNamesUsed["ToDMTFTimeIntervalMethod"].ToString()
					}
				};
			}
			if (!(strType == "System.Management.ManagementPath"))
			{
				return null;
			}
			return new CodePropertyReferenceExpression(new CodeCastExpression(new CodeTypeReference(this.PublicNamesUsed["PathClass"].ToString()), beginingExpression), this.PublicNamesUsed["PathProperty"].ToString());
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0001CCBC File Offset: 0x0001AEBC
		private void GenerateScopeProperty()
		{
			this.cmp = new CodeMemberProperty();
			this.cmp.Name = this.PublicNamesUsed["ScopeProperty"].ToString();
			this.cmp.Attributes = (MemberAttributes)24578;
			this.cmp.Type = new CodeTypeReference(this.PublicNamesUsed["ScopeClass"].ToString());
			this.caa = new CodeAttributeArgument();
			this.caa.Value = new CodePrimitiveExpression(true);
			this.cad = new CodeAttributeDeclaration();
			this.cad.Name = "Browsable";
			this.cad.Arguments.Add(this.caa);
			this.cmp.CustomAttributes = new CodeAttributeDeclarationCollection();
			this.cmp.CustomAttributes.Add(this.cad);
			if (ManagementClassGenerator.IsDesignerSerializationVisibilityToBeSet(this.PublicNamesUsed["ScopeProperty"].ToString()))
			{
				this.caa = new CodeAttributeArgument();
				this.caa.Value = new CodeFieldReferenceExpression(new CodeTypeReferenceExpression("DesignerSerializationVisibility"), "Hidden");
				this.cad = new CodeAttributeDeclaration();
				this.cad.Name = "DesignerSerializationVisibility";
				this.cad.Arguments.Add(this.caa);
				this.cmp.CustomAttributes.Add(this.cad);
			}
			this.cis = new CodeConditionStatement();
			this.cboe = new CodeBinaryOperatorExpression();
			this.cboe.Left = new CodeVariableReferenceExpression(this.PrivateNamesUsed["IsEmbedded"].ToString());
			this.cboe.Right = new CodePrimitiveExpression(false);
			this.cboe.Operator = CodeBinaryOperatorType.ValueEquality;
			this.cis.Condition = this.cboe;
			CodeExpression codeExpression = new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(this.PrivateNamesUsed["LateBoundObject"].ToString()), "Scope");
			this.cis.TrueStatements.Add(new CodeMethodReturnStatement(codeExpression));
			this.cis.FalseStatements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(null)));
			this.cmp.GetStatements.Add(this.cis);
			this.cis = new CodeConditionStatement();
			this.cboe = new CodeBinaryOperatorExpression();
			this.cboe.Left = new CodeVariableReferenceExpression(this.PrivateNamesUsed["IsEmbedded"].ToString());
			this.cboe.Right = new CodePrimitiveExpression(false);
			this.cboe.Operator = CodeBinaryOperatorType.ValueEquality;
			this.cis.Condition = this.cboe;
			this.cis.TrueStatements.Add(new CodeAssignStatement(codeExpression, new CodeSnippetExpression("value")));
			this.cmp.SetStatements.Add(this.cis);
			this.cc.Members.Add(this.cmp);
			this.cmp.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("COMMENT_MGMTSCOPE")));
		}

		// Token: 0x06000337 RID: 823 RVA: 0x0001CFEC File Offset: 0x0001B1EC
		private void AddGetStatementsForEnumArray(CodeIndexerExpression ciProp, CodeMemberProperty cmProp)
		{
			string text = "arrEnumVals";
			string text2 = "enumToRet";
			string text3 = "counter";
			string baseType = cmProp.Type.BaseType;
			cmProp.GetStatements.Add(new CodeVariableDeclarationStatement("System.Array", text, new CodeCastExpression(new CodeTypeReference("System.Array"), ciProp)));
			cmProp.GetStatements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference(baseType, 1), text2, new CodeArrayCreateExpression(new CodeTypeReference(baseType), new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text), "Length"))));
			this.cfls = new CodeIterationStatement();
			cmProp.GetStatements.Add(new CodeVariableDeclarationStatement("System.Int32", text3, new CodePrimitiveExpression(0)));
			this.cfls.InitStatement = new CodeAssignStatement(new CodeVariableReferenceExpression(text3), new CodePrimitiveExpression(0));
			CodeBinaryOperatorExpression codeBinaryOperatorExpression = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression.Left = new CodeVariableReferenceExpression(text3);
			codeBinaryOperatorExpression.Operator = CodeBinaryOperatorType.LessThan;
			codeBinaryOperatorExpression.Right = new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text), "Length");
			this.cfls.TestExpression = codeBinaryOperatorExpression;
			this.cfls.IncrementStatement = new CodeAssignStatement(new CodeVariableReferenceExpression(text3), new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression(text3), CodeBinaryOperatorType.Add, new CodePrimitiveExpression(1)));
			CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method.MethodName = "GetValue";
			codeMethodInvokeExpression.Method.TargetObject = new CodeVariableReferenceExpression(text);
			codeMethodInvokeExpression.Parameters.Add(new CodeVariableReferenceExpression(text3));
			CodeMethodInvokeExpression codeMethodInvokeExpression2 = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression2.Method.TargetObject = new CodeTypeReferenceExpression("System.Convert");
			codeMethodInvokeExpression2.Parameters.Add(codeMethodInvokeExpression);
			codeMethodInvokeExpression2.Method.MethodName = this.arrConvFuncName;
			this.cfls.Statements.Add(new CodeAssignStatement(new CodeIndexerExpression(new CodeVariableReferenceExpression(text2), new CodeExpression[]
			{
				new CodeVariableReferenceExpression(text3)
			}), new CodeCastExpression(new CodeTypeReference(baseType), codeMethodInvokeExpression2)));
			cmProp.GetStatements.Add(this.cfls);
			cmProp.GetStatements.Add(new CodeMethodReturnStatement(new CodeVariableReferenceExpression(text2)));
		}

		// Token: 0x06000338 RID: 824 RVA: 0x0001D210 File Offset: 0x0001B410
		private void AddCommentsForEmbeddedProperties()
		{
			this.cc.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("")));
			this.cc.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("")));
			this.cc.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("")));
			this.cc.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("")));
			this.cc.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("EMBEDDED_COMMENT1")));
			this.cc.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("EMBEDDED_COMMENT2")));
			this.cc.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("EMBEDDED_COMMENT3")));
			this.cc.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("EMBEDDED_COMMENT4")));
			this.cc.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("EMBEDDED_COMMENT5")));
			this.cc.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("EMBEDDED_COMMENT6")));
			this.cc.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("")));
			this.cc.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("EMBEDDED_COMMENT7")));
			this.cc.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("EMBEDED_VB_CODESAMP1")));
			this.cc.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("EMBEDED_VB_CODESAMP2")));
			this.cc.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("EMBEDED_VB_CODESAMP3")));
			this.cc.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("EMBEDED_VB_CODESAMP4")));
			this.cc.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("EMBEDED_VB_CODESAMP5")));
			this.cc.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("EMBEDED_VB_CODESAMP6")));
			this.cc.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("EMBEDED_VB_CODESAMP7")));
			this.cc.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("EMBEDED_VB_CODESAMP8")));
			this.cc.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("EMBEDED_VB_CODESAMP9")));
			this.cc.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("EMBEDED_VB_CODESAMP10")));
			this.cc.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("")));
			this.cc.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("EMBEDDED_COMMENT8")));
			this.cc.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("EMBEDED_CS_CODESAMP1")));
			this.cc.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("EMBEDED_CS_CODESAMP2")));
			this.cc.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("EMBEDED_CS_CODESAMP3")));
			this.cc.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("EMBEDED_CS_CODESAMP4")));
			this.cc.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("EMBEDED_CS_CODESAMP5")));
			this.cc.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("EMBEDED_CS_CODESAMP6")));
			this.cc.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("EMBEDED_CS_CODESAMP7")));
			this.cc.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("EMBEDED_CS_CODESAMP8")));
			this.cc.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("EMBEDED_CS_CODESAMP9")));
			this.cc.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("EMBEDED_CS_CODESAMP10")));
			this.cc.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("EMBEDED_CS_CODESAMP11")));
			this.cc.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("EMBEDED_CS_CODESAMP12")));
			this.cc.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("EMBEDED_CS_CODESAMP13")));
			this.cc.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("EMBEDED_CS_CODESAMP14")));
			this.cc.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("EMBEDED_CS_CODESAMP15")));
		}

		// Token: 0x06000339 RID: 825 RVA: 0x0001D700 File Offset: 0x0001B900
		private bool GetDateTimeType(PropertyData prop, ref CodeTypeReference codeType)
		{
			bool flag = false;
			codeType = null;
			if (prop.IsArray)
			{
				codeType = new CodeTypeReference("System.DateTime", 1);
			}
			else
			{
				codeType = new CodeTypeReference("System.DateTime");
			}
			try
			{
				if (string.Compare(prop.Qualifiers["SubType"].Value.ToString(), "interval", StringComparison.OrdinalIgnoreCase) == 0)
				{
					flag = true;
					if (prop.IsArray)
					{
						codeType = new CodeTypeReference("System.TimeSpan", 1);
					}
					else
					{
						codeType = new CodeTypeReference("System.TimeSpan");
					}
				}
			}
			catch (ManagementException)
			{
			}
			if (flag)
			{
				if (!this.bTimeSpanConversionFunctionsAdded)
				{
					this.cc.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("COMMENT_TIMESPANCONVFUNC")));
					this.bTimeSpanConversionFunctionsAdded = true;
					this.GenerateTimeSpanConversionFunction();
				}
			}
			else if (!this.bDateConversionFunctionsAdded)
			{
				this.cc.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("COMMENT_DATECONVFUNC")));
				this.bDateConversionFunctionsAdded = true;
				this.GenerateDateTimeConversionFunction();
			}
			return flag;
		}

		// Token: 0x0600033A RID: 826 RVA: 0x0001D808 File Offset: 0x0001BA08
		private void GenerateCreateInstance()
		{
			string text = "tmpMgmtClass";
			this.cmm = new CodeMemberMethod();
			string text2 = "mgmtScope";
			string text3 = "mgmtPath";
			this.cmm.Attributes = (MemberAttributes)24579;
			this.cmm.Name = this.PublicNamesUsed["CreateInst"].ToString();
			this.cmm.ReturnType = new CodeTypeReference(this.PrivateNamesUsed["GeneratedClassName"].ToString());
			this.caa = new CodeAttributeArgument();
			this.caa.Value = new CodePrimitiveExpression(true);
			this.cad = new CodeAttributeDeclaration();
			this.cad.Name = "Browsable";
			this.cad.Arguments.Add(this.caa);
			this.cmm.CustomAttributes = new CodeAttributeDeclarationCollection();
			this.cmm.CustomAttributes.Add(this.cad);
			this.cmm.Statements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference(this.PublicNamesUsed["ScopeClass"].ToString()), text2, new CodePrimitiveExpression(null)));
			CodeConditionStatement codeConditionStatement = new CodeConditionStatement();
			codeConditionStatement.Condition = new CodeBinaryOperatorExpression
			{
				Left = new CodeVariableReferenceExpression(this.PrivateNamesUsed["statMgmtScope"].ToString()),
				Right = new CodePrimitiveExpression(null),
				Operator = CodeBinaryOperatorType.IdentityEquality
			};
			this.coce = new CodeObjectCreateExpression();
			this.coce.CreateType = new CodeTypeReference(this.PublicNamesUsed["ScopeClass"].ToString());
			codeConditionStatement.TrueStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(text2), this.coce));
			codeConditionStatement.TrueStatements.Add(new CodeAssignStatement(new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text2), "Path"), "NamespacePath"), new CodeVariableReferenceExpression(this.PrivateNamesUsed["CreationWmiNamespace"].ToString())));
			codeConditionStatement.FalseStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(text2), new CodeVariableReferenceExpression(this.PrivateNamesUsed["statMgmtScope"].ToString())));
			this.cmm.Statements.Add(codeConditionStatement);
			CodeObjectCreateExpression codeObjectCreateExpression = new CodeObjectCreateExpression();
			codeObjectCreateExpression.CreateType = new CodeTypeReference(this.PublicNamesUsed["PathClass"].ToString());
			codeObjectCreateExpression.Parameters.Add(new CodeVariableReferenceExpression(this.PrivateNamesUsed["CreationClassName"].ToString()));
			this.cmm.Statements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference(this.PublicNamesUsed["PathClass"].ToString()), text3, codeObjectCreateExpression));
			CodeObjectCreateExpression codeObjectCreateExpression2 = new CodeObjectCreateExpression();
			codeObjectCreateExpression2.CreateType = new CodeTypeReference(this.PublicNamesUsed["ManagementClass"].ToString());
			codeObjectCreateExpression2.Parameters.Add(new CodeVariableReferenceExpression(text2));
			codeObjectCreateExpression2.Parameters.Add(new CodeVariableReferenceExpression(text3));
			codeObjectCreateExpression2.Parameters.Add(new CodePrimitiveExpression(null));
			this.cmm.Statements.Add(new CodeVariableDeclarationStatement(this.PublicNamesUsed["ManagementClass"].ToString(), text, codeObjectCreateExpression2));
			CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method.MethodName = "CreateInstance";
			codeMethodInvokeExpression.Method.TargetObject = new CodeVariableReferenceExpression(text);
			this.coce = new CodeObjectCreateExpression();
			this.coce.CreateType = new CodeTypeReference(this.PrivateNamesUsed["GeneratedClassName"].ToString());
			this.coce.Parameters.Add(codeMethodInvokeExpression);
			this.cmm.Statements.Add(new CodeMethodReturnStatement(this.coce));
			this.cc.Members.Add(this.cmm);
		}

		// Token: 0x0600033B RID: 827 RVA: 0x0001DC0C File Offset: 0x0001BE0C
		private void GenerateDeleteInstance()
		{
			this.cmm = new CodeMemberMethod();
			this.cmm.Attributes = (MemberAttributes)24578;
			this.cmm.Name = this.PublicNamesUsed["DeleteInst"].ToString();
			this.caa = new CodeAttributeArgument();
			this.caa.Value = new CodePrimitiveExpression(true);
			this.cad = new CodeAttributeDeclaration();
			this.cad.Name = "Browsable";
			this.cad.Arguments.Add(this.caa);
			this.cmm.CustomAttributes = new CodeAttributeDeclarationCollection();
			this.cmm.CustomAttributes.Add(this.cad);
			CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method.MethodName = "Delete";
			codeMethodInvokeExpression.Method.TargetObject = new CodeVariableReferenceExpression(this.PrivateNamesUsed["LateBoundObject"].ToString());
			this.cmm.Statements.Add(codeMethodInvokeExpression);
			this.cc.Members.Add(this.cmm);
		}

		// Token: 0x0600033C RID: 828 RVA: 0x0001DD32 File Offset: 0x0001BF32
		private void GenerateDateTimeConversionFunction()
		{
			this.AddToDateTimeFunction();
			this.AddToDMTFDateTimeFunction();
		}

		// Token: 0x0600033D RID: 829 RVA: 0x0001DD40 File Offset: 0x0001BF40
		private void GenerateTimeSpanConversionFunction()
		{
			this.AddToTimeSpanFunction();
			this.AddToDMTFTimeIntervalFunction();
		}

		// Token: 0x0600033E RID: 830 RVA: 0x0001DD50 File Offset: 0x0001BF50
		private void AddToDateTimeFunction()
		{
			string text = "dmtfDate";
			string text2 = "year";
			string text3 = "month";
			string text4 = "day";
			string text5 = "hour";
			string text6 = "minute";
			string text7 = "second";
			string text8 = "ticks";
			string text9 = "dmtf";
			string text10 = "tempString";
			string text11 = "datetime";
			CodeMemberMethod codeMemberMethod = new CodeMemberMethod();
			codeMemberMethod.Name = this.PrivateNamesUsed["ToDateTimeMethod"].ToString();
			codeMemberMethod.Attributes = MemberAttributes.Static;
			codeMemberMethod.ReturnType = new CodeTypeReference("System.DateTime");
			codeMemberMethod.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference("System.String"), text));
			codeMemberMethod.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("COMMENT_TODATETIME")));
			codeMemberMethod.Statements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference("System.DateTime"), "initializer", new CodeFieldReferenceExpression(new CodeTypeReferenceExpression("System.DateTime"), "MinValue")));
			CodeVariableReferenceExpression targetObject = new CodeVariableReferenceExpression("initializer");
			codeMemberMethod.Statements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference("System.Int32"), text2, new CodePropertyReferenceExpression(targetObject, "Year")));
			codeMemberMethod.Statements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference("System.Int32"), text3, new CodePropertyReferenceExpression(targetObject, "Month")));
			codeMemberMethod.Statements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference("System.Int32"), text4, new CodePropertyReferenceExpression(targetObject, "Day")));
			codeMemberMethod.Statements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference("System.Int32"), text5, new CodePropertyReferenceExpression(targetObject, "Hour")));
			codeMemberMethod.Statements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference("System.Int32"), text6, new CodePropertyReferenceExpression(targetObject, "Minute")));
			codeMemberMethod.Statements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference("System.Int32"), text7, new CodePropertyReferenceExpression(targetObject, "Second")));
			codeMemberMethod.Statements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference("System.Int64"), text8, new CodePrimitiveExpression(0)));
			codeMemberMethod.Statements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference("System.String"), text9, new CodeVariableReferenceExpression(text)));
			CodeFieldReferenceExpression initExpression = new CodeFieldReferenceExpression(new CodeTypeReferenceExpression("System.DateTime"), "MinValue");
			codeMemberMethod.Statements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference("System.DateTime"), text11, initExpression));
			codeMemberMethod.Statements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference("System.String"), text10, new CodeFieldReferenceExpression(new CodeTypeReferenceExpression("System.String"), "Empty")));
			CodeBinaryOperatorExpression codeBinaryOperatorExpression = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression.Left = new CodeVariableReferenceExpression(text9);
			codeBinaryOperatorExpression.Right = new CodePrimitiveExpression(null);
			codeBinaryOperatorExpression.Operator = CodeBinaryOperatorType.IdentityEquality;
			CodeConditionStatement codeConditionStatement = new CodeConditionStatement();
			codeConditionStatement.Condition = codeBinaryOperatorExpression;
			CodeObjectCreateExpression codeObjectCreateExpression = new CodeObjectCreateExpression();
			codeObjectCreateExpression.CreateType = new CodeTypeReference(this.PublicNamesUsed["ArgumentOutOfRangeException"].ToString());
			codeConditionStatement.TrueStatements.Add(new CodeThrowExceptionStatement(codeObjectCreateExpression));
			codeMemberMethod.Statements.Add(codeConditionStatement);
			codeBinaryOperatorExpression = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression.Left = new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text9), "Length");
			codeBinaryOperatorExpression.Right = new CodePrimitiveExpression(0);
			codeBinaryOperatorExpression.Operator = CodeBinaryOperatorType.ValueEquality;
			codeConditionStatement = new CodeConditionStatement();
			codeConditionStatement.Condition = codeBinaryOperatorExpression;
			codeConditionStatement.TrueStatements.Add(new CodeThrowExceptionStatement(codeObjectCreateExpression));
			codeMemberMethod.Statements.Add(codeConditionStatement);
			codeBinaryOperatorExpression = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression.Left = new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text9), "Length");
			codeBinaryOperatorExpression.Right = new CodePrimitiveExpression(25);
			codeBinaryOperatorExpression.Operator = CodeBinaryOperatorType.IdentityInequality;
			codeConditionStatement = new CodeConditionStatement();
			codeConditionStatement.Condition = codeBinaryOperatorExpression;
			codeConditionStatement.TrueStatements.Add(new CodeThrowExceptionStatement(codeObjectCreateExpression));
			codeMemberMethod.Statements.Add(codeConditionStatement);
			CodeTryCatchFinallyStatement codeTryCatchFinallyStatement = new CodeTryCatchFinallyStatement();
			ManagementClassGenerator.DateTimeConversionFunctionHelper(codeTryCatchFinallyStatement.TryStatements, "****", text10, text9, text2, 0, 4);
			ManagementClassGenerator.DateTimeConversionFunctionHelper(codeTryCatchFinallyStatement.TryStatements, "**", text10, text9, text3, 4, 2);
			ManagementClassGenerator.DateTimeConversionFunctionHelper(codeTryCatchFinallyStatement.TryStatements, "**", text10, text9, text4, 6, 2);
			ManagementClassGenerator.DateTimeConversionFunctionHelper(codeTryCatchFinallyStatement.TryStatements, "**", text10, text9, text5, 8, 2);
			ManagementClassGenerator.DateTimeConversionFunctionHelper(codeTryCatchFinallyStatement.TryStatements, "**", text10, text9, text6, 10, 2);
			ManagementClassGenerator.DateTimeConversionFunctionHelper(codeTryCatchFinallyStatement.TryStatements, "**", text10, text9, text7, 12, 2);
			CodeMethodReferenceExpression method = new CodeMethodReferenceExpression(new CodeVariableReferenceExpression(text9), "Substring");
			CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method = method;
			codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(15));
			codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(6));
			codeTryCatchFinallyStatement.TryStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(text10), codeMethodInvokeExpression));
			codeBinaryOperatorExpression = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression.Left = new CodePrimitiveExpression("******");
			codeBinaryOperatorExpression.Right = new CodeVariableReferenceExpression(text10);
			codeBinaryOperatorExpression.Operator = CodeBinaryOperatorType.IdentityInequality;
			codeConditionStatement = new CodeConditionStatement();
			codeConditionStatement.Condition = codeBinaryOperatorExpression;
			CodeMethodReferenceExpression method2 = new CodeMethodReferenceExpression(new CodeTypeReferenceExpression("System.Int64"), "Parse");
			CodeMethodInvokeExpression codeMethodInvokeExpression2 = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression2.Method = method2;
			codeMethodInvokeExpression2.Parameters.Add(new CodeVariableReferenceExpression(text10));
			CodeCastExpression codeCastExpression = new CodeCastExpression("System.Int64", new CodeBinaryOperatorExpression
			{
				Left = new CodeFieldReferenceExpression(new CodeTypeReferenceExpression("System.TimeSpan"), "TicksPerMillisecond"),
				Right = new CodePrimitiveExpression(1000),
				Operator = CodeBinaryOperatorType.Divide
			});
			CodeBinaryOperatorExpression codeBinaryOperatorExpression2 = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression2.Left = codeMethodInvokeExpression2;
			codeBinaryOperatorExpression2.Right = codeCastExpression;
			codeBinaryOperatorExpression2.Operator = CodeBinaryOperatorType.Multiply;
			codeConditionStatement.TrueStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(text8), codeBinaryOperatorExpression2));
			codeTryCatchFinallyStatement.TryStatements.Add(codeConditionStatement);
			CodeBinaryOperatorExpression codeBinaryOperatorExpression3 = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression3.Left = new CodeVariableReferenceExpression(text2);
			codeBinaryOperatorExpression3.Right = new CodePrimitiveExpression(0);
			codeBinaryOperatorExpression3.Operator = CodeBinaryOperatorType.LessThan;
			CodeBinaryOperatorExpression codeBinaryOperatorExpression4 = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression4.Left = new CodeVariableReferenceExpression(text3);
			codeBinaryOperatorExpression4.Right = new CodePrimitiveExpression(0);
			codeBinaryOperatorExpression4.Operator = CodeBinaryOperatorType.LessThan;
			CodeBinaryOperatorExpression codeBinaryOperatorExpression5 = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression5.Left = new CodeVariableReferenceExpression(text4);
			codeBinaryOperatorExpression5.Right = new CodePrimitiveExpression(0);
			codeBinaryOperatorExpression5.Operator = CodeBinaryOperatorType.LessThan;
			CodeBinaryOperatorExpression codeBinaryOperatorExpression6 = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression6.Left = new CodeVariableReferenceExpression(text5);
			codeBinaryOperatorExpression6.Right = new CodePrimitiveExpression(0);
			codeBinaryOperatorExpression6.Operator = CodeBinaryOperatorType.LessThan;
			CodeBinaryOperatorExpression codeBinaryOperatorExpression7 = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression7.Left = new CodeVariableReferenceExpression(text6);
			codeBinaryOperatorExpression7.Right = new CodePrimitiveExpression(0);
			codeBinaryOperatorExpression7.Operator = CodeBinaryOperatorType.LessThan;
			CodeBinaryOperatorExpression codeBinaryOperatorExpression8 = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression8.Left = new CodeVariableReferenceExpression(text7);
			codeBinaryOperatorExpression8.Right = new CodePrimitiveExpression(0);
			codeBinaryOperatorExpression8.Operator = CodeBinaryOperatorType.LessThan;
			CodeBinaryOperatorExpression codeBinaryOperatorExpression9 = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression9.Left = new CodeVariableReferenceExpression(text8);
			codeBinaryOperatorExpression9.Right = new CodePrimitiveExpression(0);
			codeBinaryOperatorExpression9.Operator = CodeBinaryOperatorType.LessThan;
			CodeBinaryOperatorExpression codeBinaryOperatorExpression10 = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression10.Left = codeBinaryOperatorExpression3;
			codeBinaryOperatorExpression10.Right = codeBinaryOperatorExpression4;
			codeBinaryOperatorExpression10.Operator = CodeBinaryOperatorType.BooleanOr;
			CodeBinaryOperatorExpression codeBinaryOperatorExpression11 = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression11.Left = codeBinaryOperatorExpression10;
			codeBinaryOperatorExpression11.Right = codeBinaryOperatorExpression5;
			codeBinaryOperatorExpression11.Operator = CodeBinaryOperatorType.BooleanOr;
			CodeBinaryOperatorExpression codeBinaryOperatorExpression12 = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression12.Left = codeBinaryOperatorExpression11;
			codeBinaryOperatorExpression12.Right = codeBinaryOperatorExpression6;
			codeBinaryOperatorExpression12.Operator = CodeBinaryOperatorType.BooleanOr;
			CodeBinaryOperatorExpression codeBinaryOperatorExpression13 = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression13.Left = codeBinaryOperatorExpression12;
			codeBinaryOperatorExpression13.Right = codeBinaryOperatorExpression7;
			codeBinaryOperatorExpression13.Operator = CodeBinaryOperatorType.BooleanOr;
			CodeBinaryOperatorExpression codeBinaryOperatorExpression14 = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression14.Left = codeBinaryOperatorExpression13;
			codeBinaryOperatorExpression14.Right = codeBinaryOperatorExpression7;
			codeBinaryOperatorExpression14.Operator = CodeBinaryOperatorType.BooleanOr;
			CodeBinaryOperatorExpression codeBinaryOperatorExpression15 = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression15.Left = codeBinaryOperatorExpression14;
			codeBinaryOperatorExpression15.Right = codeBinaryOperatorExpression8;
			codeBinaryOperatorExpression15.Operator = CodeBinaryOperatorType.BooleanOr;
			CodeBinaryOperatorExpression codeBinaryOperatorExpression16 = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression16.Left = codeBinaryOperatorExpression15;
			codeBinaryOperatorExpression16.Right = codeBinaryOperatorExpression9;
			codeBinaryOperatorExpression16.Operator = CodeBinaryOperatorType.BooleanOr;
			codeConditionStatement = new CodeConditionStatement();
			codeConditionStatement.Condition = codeBinaryOperatorExpression16;
			codeConditionStatement.TrueStatements.Add(new CodeThrowExceptionStatement(codeObjectCreateExpression));
			codeTryCatchFinallyStatement.TryStatements.Add(codeConditionStatement);
			string text12 = "e";
			CodeCatchClause codeCatchClause = new CodeCatchClause(text12);
			CodeObjectCreateExpression codeObjectCreateExpression2 = new CodeObjectCreateExpression();
			codeObjectCreateExpression2.CreateType = new CodeTypeReference(this.PublicNamesUsed["ArgumentOutOfRangeException"].ToString());
			codeObjectCreateExpression2.Parameters.Add(new CodePrimitiveExpression(null));
			codeObjectCreateExpression2.Parameters.Add(new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text12), "Message"));
			codeCatchClause.Statements.Add(new CodeThrowExceptionStatement(codeObjectCreateExpression2));
			codeTryCatchFinallyStatement.CatchClauses.Add(codeCatchClause);
			codeMemberMethod.Statements.Add(codeTryCatchFinallyStatement);
			this.coce = new CodeObjectCreateExpression();
			this.coce.CreateType = new CodeTypeReference("System.DateTime");
			this.coce.Parameters.Add(new CodeVariableReferenceExpression(text2));
			this.coce.Parameters.Add(new CodeVariableReferenceExpression(text3));
			this.coce.Parameters.Add(new CodeVariableReferenceExpression(text4));
			this.coce.Parameters.Add(new CodeVariableReferenceExpression(text5));
			this.coce.Parameters.Add(new CodeVariableReferenceExpression(text6));
			this.coce.Parameters.Add(new CodeVariableReferenceExpression(text7));
			this.coce.Parameters.Add(new CodePrimitiveExpression(0));
			codeMemberMethod.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(text11), this.coce));
			CodeMethodReferenceExpression method3 = new CodeMethodReferenceExpression(new CodeVariableReferenceExpression(text11), "AddTicks");
			CodeMethodInvokeExpression codeMethodInvokeExpression3 = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression3.Method = method3;
			codeMethodInvokeExpression3.Parameters.Add(new CodeVariableReferenceExpression(text8));
			codeMemberMethod.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(text11), codeMethodInvokeExpression3));
			method2 = new CodeMethodReferenceExpression(new CodePropertyReferenceExpression(new CodeTypeReferenceExpression("System.TimeZone"), "CurrentTimeZone"), "GetUtcOffset");
			codeMethodInvokeExpression2 = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression2.Method = method2;
			codeMethodInvokeExpression2.Parameters.Add(new CodeVariableReferenceExpression(text11));
			string text13 = "tickOffset";
			codeMemberMethod.Statements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference("System.TimeSpan"), text13, codeMethodInvokeExpression2));
			string text14 = "UTCOffset";
			codeMemberMethod.Statements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference("System.Int32"), text14, new CodePrimitiveExpression(0)));
			string text15 = "OffsetToBeAdjusted";
			codeMemberMethod.Statements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference("System.Int32"), text15, new CodePrimitiveExpression(0)));
			string text16 = "OffsetMins";
			codeCastExpression = new CodeCastExpression("System.Int64", new CodeBinaryOperatorExpression
			{
				Left = new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text13), "Ticks"),
				Right = new CodeFieldReferenceExpression(new CodeTypeReferenceExpression("System.TimeSpan"), "TicksPerMinute"),
				Operator = CodeBinaryOperatorType.Divide
			});
			codeMemberMethod.Statements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference("System.Int64"), text16, codeCastExpression));
			method = new CodeMethodReferenceExpression(new CodeVariableReferenceExpression(text9), "Substring");
			codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method = method;
			codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(22));
			codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(3));
			codeMemberMethod.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(text10), codeMethodInvokeExpression));
			codeBinaryOperatorExpression = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression.Left = new CodeVariableReferenceExpression(text10);
			codeBinaryOperatorExpression.Right = new CodePrimitiveExpression("******");
			codeBinaryOperatorExpression.Operator = CodeBinaryOperatorType.IdentityInequality;
			codeConditionStatement = new CodeConditionStatement();
			codeConditionStatement.Condition = codeBinaryOperatorExpression;
			method = new CodeMethodReferenceExpression(new CodeVariableReferenceExpression(text9), "Substring");
			codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method = method;
			codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(21));
			codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(4));
			codeConditionStatement.TrueStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(text10), codeMethodInvokeExpression));
			CodeTryCatchFinallyStatement codeTryCatchFinallyStatement2 = new CodeTryCatchFinallyStatement();
			method = new CodeMethodReferenceExpression(new CodeTypeReferenceExpression("System.Int32"), "Parse");
			codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method = method;
			codeMethodInvokeExpression.Parameters.Add(new CodeVariableReferenceExpression(text10));
			codeTryCatchFinallyStatement2.TryStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(text14), codeMethodInvokeExpression));
			codeTryCatchFinallyStatement2.CatchClauses.Add(codeCatchClause);
			codeConditionStatement.TrueStatements.Add(codeTryCatchFinallyStatement2);
			codeBinaryOperatorExpression = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression.Left = new CodeVariableReferenceExpression(text16);
			codeBinaryOperatorExpression.Right = new CodeVariableReferenceExpression(text14);
			codeBinaryOperatorExpression.Operator = CodeBinaryOperatorType.Subtract;
			codeConditionStatement.TrueStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(text15), new CodeCastExpression(new CodeTypeReference("System.Int32"), codeBinaryOperatorExpression)));
			method = new CodeMethodReferenceExpression(new CodeVariableReferenceExpression(text11), "AddMinutes");
			codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method = method;
			codeMethodInvokeExpression.Parameters.Add(new CodeCastExpression("System.Double", new CodeVariableReferenceExpression(text15)));
			codeConditionStatement.TrueStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(text11), codeMethodInvokeExpression));
			codeMemberMethod.Statements.Add(codeConditionStatement);
			codeMemberMethod.Statements.Add(new CodeMethodReturnStatement(new CodeVariableReferenceExpression(text11)));
			this.cc.Members.Add(codeMemberMethod);
		}

		// Token: 0x0600033F RID: 831 RVA: 0x0001EB90 File Offset: 0x0001CD90
		private static void DateTimeConversionFunctionHelper(CodeStatementCollection cmmdt, string toCompare, string tempVarName, string dmtfVarName, string toAssign, int SubStringParam1, int SubStringParam2)
		{
			CodeMethodReferenceExpression method = new CodeMethodReferenceExpression(new CodeVariableReferenceExpression(dmtfVarName), "Substring");
			CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method = method;
			codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(SubStringParam1));
			codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(SubStringParam2));
			cmmdt.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(tempVarName), codeMethodInvokeExpression));
			CodeBinaryOperatorExpression codeBinaryOperatorExpression = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression.Left = new CodePrimitiveExpression(toCompare);
			codeBinaryOperatorExpression.Right = new CodeVariableReferenceExpression(tempVarName);
			codeBinaryOperatorExpression.Operator = CodeBinaryOperatorType.IdentityInequality;
			CodeConditionStatement codeConditionStatement = new CodeConditionStatement();
			codeConditionStatement.Condition = codeBinaryOperatorExpression;
			method = new CodeMethodReferenceExpression(new CodeTypeReferenceExpression("System.Int32"), "Parse");
			codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method = method;
			codeMethodInvokeExpression.Parameters.Add(new CodeVariableReferenceExpression(tempVarName));
			codeConditionStatement.TrueStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(toAssign), codeMethodInvokeExpression));
			cmmdt.Add(codeConditionStatement);
		}

		// Token: 0x06000340 RID: 832 RVA: 0x0001EC88 File Offset: 0x0001CE88
		private void AddToDMTFTimeIntervalFunction()
		{
			string text = "dmtftimespan";
			string text2 = "timespan";
			string text3 = "tsTemp";
			string text4 = "microsec";
			string text5 = "strMicroSec";
			CodeMemberMethod codeMemberMethod = new CodeMemberMethod();
			codeMemberMethod.Name = this.PrivateNamesUsed["ToDMTFTimeIntervalMethod"].ToString();
			codeMemberMethod.Attributes = MemberAttributes.Static;
			codeMemberMethod.ReturnType = new CodeTypeReference("System.String");
			codeMemberMethod.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference("System.TimeSpan"), text2));
			codeMemberMethod.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("COMMENT_TODMTFTIMEINTERVAL")));
			CodePropertyReferenceExpression expression = new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text2), "Days");
			this.cmie = new CodeMethodInvokeExpression();
			this.cmie.Method = new CodeMethodReferenceExpression(new CodeCastExpression(new CodeTypeReference("System.Int32 "), expression), "ToString");
			CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method = new CodeMethodReferenceExpression(this.cmie, "PadLeft");
			codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(8));
			codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression('0'));
			codeMemberMethod.Statements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference("System.String"), text, codeMethodInvokeExpression));
			CodeObjectCreateExpression codeObjectCreateExpression = new CodeObjectCreateExpression();
			codeObjectCreateExpression.CreateType = new CodeTypeReference(this.PublicNamesUsed["ArgumentOutOfRangeException"].ToString());
			CodeFieldReferenceExpression initExpression = new CodeFieldReferenceExpression(new CodeTypeReferenceExpression("System.TimeSpan"), "MaxValue");
			codeMemberMethod.Statements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference("System.TimeSpan"), "maxTimeSpan", initExpression));
			CodeBinaryOperatorExpression codeBinaryOperatorExpression = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression.Left = new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text2), "Days");
			codeBinaryOperatorExpression.Operator = CodeBinaryOperatorType.GreaterThan;
			codeBinaryOperatorExpression.Right = new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("maxTimeSpan"), "Days");
			CodeConditionStatement codeConditionStatement = new CodeConditionStatement();
			codeConditionStatement.Condition = codeBinaryOperatorExpression;
			codeConditionStatement.TrueStatements.Add(new CodeThrowExceptionStatement(codeObjectCreateExpression));
			codeMemberMethod.Statements.Add(codeConditionStatement);
			CodeFieldReferenceExpression initExpression2 = new CodeFieldReferenceExpression(new CodeTypeReferenceExpression("System.TimeSpan"), "MinValue");
			codeMemberMethod.Statements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference("System.TimeSpan"), "minTimeSpan", initExpression2));
			CodeBinaryOperatorExpression codeBinaryOperatorExpression2 = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression2.Left = new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text2), "Days");
			codeBinaryOperatorExpression2.Operator = CodeBinaryOperatorType.LessThan;
			codeBinaryOperatorExpression2.Right = new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("minTimeSpan"), "Days");
			CodeConditionStatement codeConditionStatement2 = new CodeConditionStatement();
			codeConditionStatement2.Condition = codeBinaryOperatorExpression2;
			codeConditionStatement2.TrueStatements.Add(new CodeThrowExceptionStatement(codeObjectCreateExpression));
			codeMemberMethod.Statements.Add(codeConditionStatement2);
			expression = new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text2), "Hours");
			this.cmie = new CodeMethodInvokeExpression();
			this.cmie.Method = new CodeMethodReferenceExpression(new CodeCastExpression(new CodeTypeReference("System.Int32 "), expression), "ToString");
			codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method = new CodeMethodReferenceExpression(this.cmie, "PadLeft");
			codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(2));
			codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression('0'));
			CodeMethodInvokeExpression right = ManagementClassGenerator.GenerateConcatStrings(new CodeVariableReferenceExpression(text), codeMethodInvokeExpression);
			codeMemberMethod.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(text), right));
			expression = new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text2), "Minutes");
			this.cmie = new CodeMethodInvokeExpression();
			this.cmie.Method = new CodeMethodReferenceExpression(new CodeCastExpression(new CodeTypeReference("System.Int32 "), expression), "ToString");
			codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method = new CodeMethodReferenceExpression(this.cmie, "PadLeft");
			codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(2));
			codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression('0'));
			right = ManagementClassGenerator.GenerateConcatStrings(new CodeVariableReferenceExpression(text), codeMethodInvokeExpression);
			codeMemberMethod.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(text), right));
			expression = new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text2), "Seconds");
			this.cmie = new CodeMethodInvokeExpression();
			this.cmie.Method = new CodeMethodReferenceExpression(new CodeCastExpression(new CodeTypeReference("System.Int32 "), expression), "ToString");
			codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method = new CodeMethodReferenceExpression(this.cmie, "PadLeft");
			codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(2));
			codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression('0'));
			right = ManagementClassGenerator.GenerateConcatStrings(new CodeVariableReferenceExpression(text), codeMethodInvokeExpression);
			codeMemberMethod.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(text), right));
			right = ManagementClassGenerator.GenerateConcatStrings(new CodeVariableReferenceExpression(text), new CodePrimitiveExpression("."));
			codeMemberMethod.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(text), right));
			this.coce = new CodeObjectCreateExpression();
			this.coce.CreateType = new CodeTypeReference("System.TimeSpan");
			this.coce.Parameters.Add(new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text2), "Days"));
			this.coce.Parameters.Add(new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text2), "Hours"));
			this.coce.Parameters.Add(new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text2), "Minutes"));
			this.coce.Parameters.Add(new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text2), "Seconds"));
			this.coce.Parameters.Add(new CodePrimitiveExpression(0));
			codeMemberMethod.Statements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference("System.TimeSpan"), text3, this.coce));
			codeBinaryOperatorExpression = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression.Left = new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text2), "Ticks");
			codeBinaryOperatorExpression.Right = new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text3), "Ticks");
			codeBinaryOperatorExpression.Operator = CodeBinaryOperatorType.Subtract;
			CodeBinaryOperatorExpression codeBinaryOperatorExpression3 = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression3.Left = codeBinaryOperatorExpression;
			codeBinaryOperatorExpression3.Right = new CodePrimitiveExpression(1000);
			codeBinaryOperatorExpression3.Operator = CodeBinaryOperatorType.Multiply;
			CodeBinaryOperatorExpression codeBinaryOperatorExpression4 = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression4.Left = codeBinaryOperatorExpression3;
			codeBinaryOperatorExpression4.Right = new CodeFieldReferenceExpression(new CodeTypeReferenceExpression("System.TimeSpan"), "TicksPerMillisecond");
			codeBinaryOperatorExpression4.Operator = CodeBinaryOperatorType.Divide;
			codeMemberMethod.Statements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference("System.Int64"), text4, new CodeCastExpression("System.Int64", codeBinaryOperatorExpression4)));
			this.cmie = new CodeMethodInvokeExpression();
			this.cmie.Method = new CodeMethodReferenceExpression(new CodeCastExpression(new CodeTypeReference("System.Int64 "), new CodeVariableReferenceExpression(text4)), "ToString");
			codeMemberMethod.Statements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference("System.String"), text5, this.cmie));
			codeBinaryOperatorExpression = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression.Left = new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text5), "Length");
			codeBinaryOperatorExpression.Right = new CodePrimitiveExpression(6);
			codeBinaryOperatorExpression.Operator = CodeBinaryOperatorType.GreaterThan;
			codeConditionStatement = new CodeConditionStatement();
			codeConditionStatement.Condition = codeBinaryOperatorExpression;
			this.cmie = new CodeMethodInvokeExpression();
			this.cmie.Method = new CodeMethodReferenceExpression(new CodeVariableReferenceExpression(text5), "Substring");
			this.cmie.Parameters.Add(new CodePrimitiveExpression(0));
			this.cmie.Parameters.Add(new CodePrimitiveExpression(6));
			codeConditionStatement.TrueStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(text5), this.cmie));
			codeMemberMethod.Statements.Add(codeConditionStatement);
			this.cmie = new CodeMethodInvokeExpression();
			this.cmie.Method = new CodeMethodReferenceExpression(new CodeVariableReferenceExpression(text5), "PadLeft");
			this.cmie.Parameters.Add(new CodePrimitiveExpression(6));
			this.cmie.Parameters.Add(new CodePrimitiveExpression('0'));
			right = ManagementClassGenerator.GenerateConcatStrings(new CodeVariableReferenceExpression(text), this.cmie);
			codeMemberMethod.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(text), right));
			right = ManagementClassGenerator.GenerateConcatStrings(new CodeVariableReferenceExpression(text), new CodePrimitiveExpression(":000"));
			codeMemberMethod.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(text), right));
			codeMemberMethod.Statements.Add(new CodeMethodReturnStatement(new CodeVariableReferenceExpression(text)));
			this.cc.Members.Add(codeMemberMethod);
		}

		// Token: 0x06000341 RID: 833 RVA: 0x0001F584 File Offset: 0x0001D784
		private void AddToDMTFDateTimeFunction()
		{
			string text = "utcString";
			string text2 = "date";
			CodeMemberMethod codeMemberMethod = new CodeMemberMethod();
			codeMemberMethod.Name = this.PrivateNamesUsed["ToDMTFDateTimeMethod"].ToString();
			codeMemberMethod.Attributes = MemberAttributes.Static;
			codeMemberMethod.ReturnType = new CodeTypeReference("System.String");
			codeMemberMethod.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference("System.DateTime"), text2));
			codeMemberMethod.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("COMMENT_TODMTFDATETIME")));
			codeMemberMethod.Statements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference("System.String"), text, new CodeFieldReferenceExpression(new CodeTypeReferenceExpression("System.String"), "Empty")));
			CodeMethodReferenceExpression method = new CodeMethodReferenceExpression(new CodePropertyReferenceExpression(new CodeTypeReferenceExpression("System.TimeZone"), "CurrentTimeZone"), "GetUtcOffset");
			CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method = method;
			codeMethodInvokeExpression.Parameters.Add(new CodeVariableReferenceExpression(text2));
			string text3 = "tickOffset";
			codeMemberMethod.Statements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference("System.TimeSpan"), text3, codeMethodInvokeExpression));
			string text4 = "OffsetMins";
			this.cboe = new CodeBinaryOperatorExpression();
			this.cboe.Left = new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text3), "Ticks");
			this.cboe.Right = new CodeFieldReferenceExpression(new CodeTypeReferenceExpression("System.TimeSpan"), "TicksPerMinute");
			this.cboe.Operator = CodeBinaryOperatorType.Divide;
			CodeCastExpression initExpression = new CodeCastExpression("System.Int64", this.cboe);
			codeMemberMethod.Statements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference("System.Int64"), text4, initExpression));
			method = new CodeMethodReferenceExpression(new CodeTypeReferenceExpression("System.Math"), "Abs");
			codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method = method;
			codeMethodInvokeExpression.Parameters.Add(new CodeVariableReferenceExpression(text4));
			this.cboe = new CodeBinaryOperatorExpression();
			this.cboe.Left = codeMethodInvokeExpression;
			this.cboe.Right = new CodePrimitiveExpression(999);
			this.cboe.Operator = CodeBinaryOperatorType.GreaterThan;
			CodeConditionStatement codeConditionStatement = new CodeConditionStatement();
			codeConditionStatement.Condition = this.cboe;
			method = new CodeMethodReferenceExpression(new CodeVariableReferenceExpression(text2), "ToUniversalTime");
			codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method = method;
			codeConditionStatement.TrueStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(text2), codeMethodInvokeExpression));
			codeConditionStatement.TrueStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(text), new CodePrimitiveExpression("+000")));
			CodeBinaryOperatorExpression codeBinaryOperatorExpression = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression.Left = new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text3), "Ticks");
			codeBinaryOperatorExpression.Right = new CodePrimitiveExpression(0);
			codeBinaryOperatorExpression.Operator = CodeBinaryOperatorType.GreaterThanOrEqual;
			CodeConditionStatement codeConditionStatement2 = new CodeConditionStatement();
			codeConditionStatement2.Condition = codeBinaryOperatorExpression;
			CodeBinaryOperatorExpression codeBinaryOperatorExpression2 = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression2.Left = new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text3), "Ticks");
			codeBinaryOperatorExpression2.Right = new CodeFieldReferenceExpression(new CodeTypeReferenceExpression("System.TimeSpan"), "TicksPerMinute");
			codeBinaryOperatorExpression2.Operator = CodeBinaryOperatorType.Divide;
			codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method = new CodeMethodReferenceExpression(new CodeCastExpression(new CodeTypeReference("System.Int64 "), codeBinaryOperatorExpression2), "ToString");
			CodeMethodInvokeExpression codeMethodInvokeExpression2 = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression2.Method = new CodeMethodReferenceExpression(codeMethodInvokeExpression, "PadLeft");
			codeMethodInvokeExpression2.Parameters.Add(new CodePrimitiveExpression(3));
			codeMethodInvokeExpression2.Parameters.Add(new CodePrimitiveExpression('0'));
			codeConditionStatement2.TrueStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(text), ManagementClassGenerator.GenerateConcatStrings(new CodePrimitiveExpression("+"), codeMethodInvokeExpression2)));
			codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method = new CodeMethodReferenceExpression(new CodeCastExpression(new CodeTypeReference("System.Int64 "), new CodeVariableReferenceExpression(text4)), "ToString");
			codeConditionStatement2.FalseStatements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference("System.String"), "strTemp", codeMethodInvokeExpression));
			codeMethodInvokeExpression2 = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression2.Method = new CodeMethodReferenceExpression(new CodeVariableReferenceExpression("strTemp"), "Substring");
			codeMethodInvokeExpression2.Parameters.Add(new CodePrimitiveExpression(1));
			codeMethodInvokeExpression2.Parameters.Add(new CodeBinaryOperatorExpression(new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("strTemp"), "Length"), CodeBinaryOperatorType.Subtract, new CodePrimitiveExpression(1)));
			CodeMethodInvokeExpression codeMethodInvokeExpression3 = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression3.Method = new CodeMethodReferenceExpression(codeMethodInvokeExpression2, "PadLeft");
			codeMethodInvokeExpression3.Parameters.Add(new CodePrimitiveExpression(3));
			codeMethodInvokeExpression3.Parameters.Add(new CodePrimitiveExpression('0'));
			codeConditionStatement2.FalseStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(text), ManagementClassGenerator.GenerateConcatStrings(new CodePrimitiveExpression("-"), codeMethodInvokeExpression3)));
			codeConditionStatement.FalseStatements.Add(codeConditionStatement2);
			codeMemberMethod.Statements.Add(codeConditionStatement);
			string text5 = "dmtfDateTime";
			codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method = new CodeMethodReferenceExpression(new CodeCastExpression(new CodeTypeReference("System.Int32 "), new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text2), "Year")), "ToString");
			codeMethodInvokeExpression2 = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression2.Method = new CodeMethodReferenceExpression(codeMethodInvokeExpression, "PadLeft");
			codeMethodInvokeExpression2.Parameters.Add(new CodePrimitiveExpression(4));
			codeMethodInvokeExpression2.Parameters.Add(new CodePrimitiveExpression('0'));
			codeMemberMethod.Statements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference("System.String"), text5, codeMethodInvokeExpression2));
			this.ToDMTFDateHelper("Month", codeMemberMethod, "System.Int32 ");
			this.ToDMTFDateHelper("Day", codeMemberMethod, "System.Int32 ");
			this.ToDMTFDateHelper("Hour", codeMemberMethod, "System.Int32 ");
			this.ToDMTFDateHelper("Minute", codeMemberMethod, "System.Int32 ");
			this.ToDMTFDateHelper("Second", codeMemberMethod, "System.Int32 ");
			codeMemberMethod.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(text5), ManagementClassGenerator.GenerateConcatStrings(new CodeVariableReferenceExpression(text5), new CodePrimitiveExpression("."))));
			string text6 = "dtTemp";
			this.coce = new CodeObjectCreateExpression();
			this.coce.CreateType = new CodeTypeReference("System.DateTime");
			this.coce.Parameters.Add(new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text2), "Year"));
			this.coce.Parameters.Add(new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text2), "Month"));
			this.coce.Parameters.Add(new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text2), "Day"));
			this.coce.Parameters.Add(new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text2), "Hour"));
			this.coce.Parameters.Add(new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text2), "Minute"));
			this.coce.Parameters.Add(new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text2), "Second"));
			this.coce.Parameters.Add(new CodePrimitiveExpression(0));
			codeMemberMethod.Statements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference("System.DateTime"), text6, this.coce));
			string text7 = "microsec";
			this.cboe = new CodeBinaryOperatorExpression();
			this.cboe.Left = new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text2), "Ticks");
			this.cboe.Right = new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text6), "Ticks");
			this.cboe.Operator = CodeBinaryOperatorType.Subtract;
			codeBinaryOperatorExpression = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression.Left = this.cboe;
			codeBinaryOperatorExpression.Right = new CodePrimitiveExpression(1000);
			codeBinaryOperatorExpression.Operator = CodeBinaryOperatorType.Multiply;
			initExpression = new CodeCastExpression("System.Int64", new CodeBinaryOperatorExpression
			{
				Left = codeBinaryOperatorExpression,
				Right = new CodeFieldReferenceExpression(new CodeTypeReferenceExpression("System.TimeSpan"), "TicksPerMillisecond"),
				Operator = CodeBinaryOperatorType.Divide
			});
			codeMemberMethod.Statements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference("System.Int64"), text7, initExpression));
			string text8 = "strMicrosec";
			codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method = new CodeMethodReferenceExpression(new CodeCastExpression(new CodeTypeReference("System.Int64 "), new CodeVariableReferenceExpression(text7)), "ToString");
			codeMemberMethod.Statements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference("System.String"), text8, codeMethodInvokeExpression));
			this.cboe = new CodeBinaryOperatorExpression();
			this.cboe.Left = new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text8), "Length");
			this.cboe.Right = new CodePrimitiveExpression(6);
			this.cboe.Operator = CodeBinaryOperatorType.GreaterThan;
			codeConditionStatement = new CodeConditionStatement();
			codeConditionStatement.Condition = this.cboe;
			codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method = new CodeMethodReferenceExpression(new CodeVariableReferenceExpression(text8), "Substring");
			codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(0));
			codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(6));
			codeConditionStatement.TrueStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(text8), codeMethodInvokeExpression));
			codeMemberMethod.Statements.Add(codeConditionStatement);
			codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method = new CodeMethodReferenceExpression(new CodeVariableReferenceExpression(text8), "PadLeft");
			codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(6));
			codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression('0'));
			codeMethodInvokeExpression2 = ManagementClassGenerator.GenerateConcatStrings(new CodeVariableReferenceExpression(text5), codeMethodInvokeExpression);
			codeMemberMethod.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(text5), codeMethodInvokeExpression2));
			codeMemberMethod.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(text5), ManagementClassGenerator.GenerateConcatStrings(new CodeVariableReferenceExpression(text5), new CodeVariableReferenceExpression(text))));
			codeMemberMethod.Statements.Add(new CodeMethodReturnStatement(new CodeVariableReferenceExpression(text5)));
			this.cc.Members.Add(codeMemberMethod);
		}

		// Token: 0x06000342 RID: 834 RVA: 0x0001FFC8 File Offset: 0x0001E1C8
		private void ToDMTFDateHelper(string dateTimeMember, CodeMemberMethod cmmdt, string strType)
		{
			string variableName = "dmtfDateTime";
			string variableName2 = "date";
			CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method = new CodeMethodReferenceExpression(new CodeCastExpression(new CodeTypeReference(strType), new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(variableName2), dateTimeMember)), "ToString");
			CodeMethodInvokeExpression codeMethodInvokeExpression2 = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression2.Method = new CodeMethodReferenceExpression(codeMethodInvokeExpression, "PadLeft");
			codeMethodInvokeExpression2.Parameters.Add(new CodePrimitiveExpression(2));
			codeMethodInvokeExpression2.Parameters.Add(new CodePrimitiveExpression('0'));
			CodeMethodInvokeExpression codeMethodInvokeExpression3 = ManagementClassGenerator.GenerateConcatStrings(codeMethodInvokeExpression, codeMethodInvokeExpression2);
			cmmdt.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(variableName), ManagementClassGenerator.GenerateConcatStrings(new CodeVariableReferenceExpression(variableName), codeMethodInvokeExpression2)));
		}

		// Token: 0x06000343 RID: 835 RVA: 0x00020080 File Offset: 0x0001E280
		private void AddToTimeSpanFunction()
		{
			string text = "dmtfTimespan";
			string text2 = "days";
			string text3 = "hours";
			string text4 = "minutes";
			string text5 = "seconds";
			string text6 = "ticks";
			CodeMemberMethod codeMemberMethod = new CodeMemberMethod();
			codeMemberMethod.Name = this.PrivateNamesUsed["ToTimeSpanMethod"].ToString();
			codeMemberMethod.Attributes = MemberAttributes.Static;
			codeMemberMethod.ReturnType = new CodeTypeReference("System.TimeSpan");
			codeMemberMethod.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference("System.String"), text));
			codeMemberMethod.Comments.Add(new CodeCommentStatement(ManagementClassGenerator.GetString("COMMENT_TOTIMESPAN")));
			codeMemberMethod.Statements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference("System.Int32"), text2, new CodePrimitiveExpression(0)));
			codeMemberMethod.Statements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference("System.Int32"), text3, new CodePrimitiveExpression(0)));
			codeMemberMethod.Statements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference("System.Int32"), text4, new CodePrimitiveExpression(0)));
			codeMemberMethod.Statements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference("System.Int32"), text5, new CodePrimitiveExpression(0)));
			codeMemberMethod.Statements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference("System.Int64"), text6, new CodePrimitiveExpression(0)));
			CodeBinaryOperatorExpression codeBinaryOperatorExpression = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression.Left = new CodeVariableReferenceExpression(text);
			codeBinaryOperatorExpression.Right = new CodePrimitiveExpression(null);
			codeBinaryOperatorExpression.Operator = CodeBinaryOperatorType.IdentityEquality;
			CodeConditionStatement codeConditionStatement = new CodeConditionStatement();
			codeConditionStatement.Condition = codeBinaryOperatorExpression;
			CodeObjectCreateExpression codeObjectCreateExpression = new CodeObjectCreateExpression();
			codeObjectCreateExpression.CreateType = new CodeTypeReference(this.PublicNamesUsed["ArgumentOutOfRangeException"].ToString());
			codeConditionStatement.TrueStatements.Add(new CodeThrowExceptionStatement(codeObjectCreateExpression));
			codeMemberMethod.Statements.Add(codeConditionStatement);
			codeBinaryOperatorExpression = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression.Left = new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text), "Length");
			codeBinaryOperatorExpression.Right = new CodePrimitiveExpression(0);
			codeBinaryOperatorExpression.Operator = CodeBinaryOperatorType.ValueEquality;
			codeConditionStatement = new CodeConditionStatement();
			codeConditionStatement.Condition = codeBinaryOperatorExpression;
			codeConditionStatement.TrueStatements.Add(new CodeThrowExceptionStatement(codeObjectCreateExpression));
			codeMemberMethod.Statements.Add(codeConditionStatement);
			codeBinaryOperatorExpression = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression.Left = new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text), "Length");
			codeBinaryOperatorExpression.Right = new CodePrimitiveExpression(25);
			codeBinaryOperatorExpression.Operator = CodeBinaryOperatorType.IdentityInequality;
			codeConditionStatement = new CodeConditionStatement();
			codeConditionStatement.Condition = codeBinaryOperatorExpression;
			codeConditionStatement.TrueStatements.Add(new CodeThrowExceptionStatement(codeObjectCreateExpression));
			codeMemberMethod.Statements.Add(codeConditionStatement);
			CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method = new CodeMethodReferenceExpression(new CodeVariableReferenceExpression(text), "Substring");
			codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(21));
			codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(4));
			codeBinaryOperatorExpression = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression.Left = codeMethodInvokeExpression;
			codeBinaryOperatorExpression.Right = new CodePrimitiveExpression(":000");
			codeBinaryOperatorExpression.Operator = CodeBinaryOperatorType.IdentityInequality;
			codeConditionStatement = new CodeConditionStatement();
			codeConditionStatement.Condition = codeBinaryOperatorExpression;
			codeConditionStatement.TrueStatements.Add(new CodeThrowExceptionStatement(codeObjectCreateExpression));
			codeMemberMethod.Statements.Add(codeConditionStatement);
			CodeTryCatchFinallyStatement codeTryCatchFinallyStatement = new CodeTryCatchFinallyStatement();
			string text7 = "tempString";
			codeTryCatchFinallyStatement.TryStatements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference("System.String"), text7, new CodeFieldReferenceExpression(new CodeTypeReferenceExpression("System.String"), "Empty")));
			ManagementClassGenerator.ToTimeSpanHelper(0, 8, text2, codeTryCatchFinallyStatement.TryStatements);
			ManagementClassGenerator.ToTimeSpanHelper(8, 2, text3, codeTryCatchFinallyStatement.TryStatements);
			ManagementClassGenerator.ToTimeSpanHelper(10, 2, text4, codeTryCatchFinallyStatement.TryStatements);
			ManagementClassGenerator.ToTimeSpanHelper(12, 2, text5, codeTryCatchFinallyStatement.TryStatements);
			codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method = new CodeMethodReferenceExpression(new CodeVariableReferenceExpression(text), "Substring");
			codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(15));
			codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(6));
			codeTryCatchFinallyStatement.TryStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(text7), codeMethodInvokeExpression));
			codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method = new CodeMethodReferenceExpression(new CodeTypeReferenceExpression("System.Int64"), "Parse");
			codeMethodInvokeExpression.Parameters.Add(new CodeVariableReferenceExpression(text7));
			codeTryCatchFinallyStatement.TryStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(text6), new CodeBinaryOperatorExpression(codeMethodInvokeExpression, CodeBinaryOperatorType.Multiply, new CodeCastExpression("System.Int64", new CodeBinaryOperatorExpression(new CodeFieldReferenceExpression(new CodeTypeReferenceExpression("System.TimeSpan"), "TicksPerMillisecond"), CodeBinaryOperatorType.Divide, new CodePrimitiveExpression(1000))))));
			CodeBinaryOperatorExpression codeBinaryOperatorExpression2 = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression2.Left = new CodeVariableReferenceExpression(text2);
			codeBinaryOperatorExpression2.Right = new CodePrimitiveExpression(0);
			codeBinaryOperatorExpression2.Operator = CodeBinaryOperatorType.LessThan;
			CodeBinaryOperatorExpression codeBinaryOperatorExpression3 = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression3.Left = new CodeVariableReferenceExpression(text3);
			codeBinaryOperatorExpression3.Right = new CodePrimitiveExpression(0);
			codeBinaryOperatorExpression3.Operator = CodeBinaryOperatorType.LessThan;
			CodeBinaryOperatorExpression codeBinaryOperatorExpression4 = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression4.Left = new CodeVariableReferenceExpression(text4);
			codeBinaryOperatorExpression4.Right = new CodePrimitiveExpression(0);
			codeBinaryOperatorExpression4.Operator = CodeBinaryOperatorType.LessThan;
			CodeBinaryOperatorExpression codeBinaryOperatorExpression5 = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression5.Left = new CodeVariableReferenceExpression(text5);
			codeBinaryOperatorExpression5.Right = new CodePrimitiveExpression(0);
			codeBinaryOperatorExpression5.Operator = CodeBinaryOperatorType.LessThan;
			CodeBinaryOperatorExpression codeBinaryOperatorExpression6 = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression6.Left = new CodeVariableReferenceExpression(text6);
			codeBinaryOperatorExpression6.Right = new CodePrimitiveExpression(0);
			codeBinaryOperatorExpression6.Operator = CodeBinaryOperatorType.LessThan;
			CodeBinaryOperatorExpression codeBinaryOperatorExpression7 = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression7.Left = codeBinaryOperatorExpression2;
			codeBinaryOperatorExpression7.Right = codeBinaryOperatorExpression3;
			codeBinaryOperatorExpression7.Operator = CodeBinaryOperatorType.BooleanOr;
			CodeBinaryOperatorExpression codeBinaryOperatorExpression8 = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression8.Left = codeBinaryOperatorExpression7;
			codeBinaryOperatorExpression8.Right = codeBinaryOperatorExpression4;
			codeBinaryOperatorExpression8.Operator = CodeBinaryOperatorType.BooleanOr;
			CodeBinaryOperatorExpression codeBinaryOperatorExpression9 = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression9.Left = codeBinaryOperatorExpression8;
			codeBinaryOperatorExpression9.Right = codeBinaryOperatorExpression5;
			codeBinaryOperatorExpression9.Operator = CodeBinaryOperatorType.BooleanOr;
			CodeBinaryOperatorExpression codeBinaryOperatorExpression10 = new CodeBinaryOperatorExpression();
			codeBinaryOperatorExpression10.Left = codeBinaryOperatorExpression9;
			codeBinaryOperatorExpression10.Right = codeBinaryOperatorExpression6;
			codeBinaryOperatorExpression10.Operator = CodeBinaryOperatorType.BooleanOr;
			codeConditionStatement = new CodeConditionStatement();
			codeConditionStatement.Condition = codeBinaryOperatorExpression10;
			codeConditionStatement.TrueStatements.Add(new CodeThrowExceptionStatement(codeObjectCreateExpression));
			string text8 = "e";
			CodeCatchClause codeCatchClause = new CodeCatchClause(text8);
			CodeObjectCreateExpression codeObjectCreateExpression2 = new CodeObjectCreateExpression();
			codeObjectCreateExpression2.CreateType = new CodeTypeReference(this.PublicNamesUsed["ArgumentOutOfRangeException"].ToString());
			codeObjectCreateExpression2.Parameters.Add(new CodePrimitiveExpression(null));
			codeObjectCreateExpression2.Parameters.Add(new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text8), "Message"));
			codeCatchClause.Statements.Add(new CodeThrowExceptionStatement(codeObjectCreateExpression2));
			codeTryCatchFinallyStatement.CatchClauses.Add(codeCatchClause);
			codeMemberMethod.Statements.Add(codeTryCatchFinallyStatement);
			string text9 = "timespan";
			this.coce = new CodeObjectCreateExpression();
			this.coce.CreateType = new CodeTypeReference("System.TimeSpan");
			this.coce.Parameters.Add(new CodeVariableReferenceExpression(text2));
			this.coce.Parameters.Add(new CodeVariableReferenceExpression(text3));
			this.coce.Parameters.Add(new CodeVariableReferenceExpression(text4));
			this.coce.Parameters.Add(new CodeVariableReferenceExpression(text5));
			this.coce.Parameters.Add(new CodePrimitiveExpression(0));
			codeMemberMethod.Statements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference("System.TimeSpan"), text9, this.coce));
			string text10 = "tsTemp";
			codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method = new CodeMethodReferenceExpression(new CodeTypeReferenceExpression("System.TimeSpan"), "FromTicks");
			codeMethodInvokeExpression.Parameters.Add(new CodeVariableReferenceExpression(text6));
			codeMemberMethod.Statements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference("System.TimeSpan"), text10, codeMethodInvokeExpression));
			codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method = new CodeMethodReferenceExpression(new CodeVariableReferenceExpression(text9), "Add");
			codeMethodInvokeExpression.Parameters.Add(new CodeVariableReferenceExpression(text10));
			codeMemberMethod.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(text9), codeMethodInvokeExpression));
			codeMemberMethod.Statements.Add(new CodeMethodReturnStatement(new CodeVariableReferenceExpression(text9)));
			this.cc.Members.Add(codeMemberMethod);
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0002093C File Offset: 0x0001EB3C
		private static void ToTimeSpanHelper(int start, int numOfCharacters, string strVarToAssign, CodeStatementCollection statCol)
		{
			string variableName = "tempString";
			string variableName2 = "dmtfTimespan";
			CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method = new CodeMethodReferenceExpression(new CodeVariableReferenceExpression(variableName2), "Substring");
			codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(start));
			codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(numOfCharacters));
			statCol.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(variableName), codeMethodInvokeExpression));
			codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method = new CodeMethodReferenceExpression(new CodeTypeReferenceExpression("System.Int32"), "Parse");
			codeMethodInvokeExpression.Parameters.Add(new CodeVariableReferenceExpression(variableName));
			statCol.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(strVarToAssign), codeMethodInvokeExpression));
		}

		// Token: 0x06000345 RID: 837 RVA: 0x000209F8 File Offset: 0x0001EBF8
		private void InitPrivateMemberVariables(CodeMemberMethod cmMethod)
		{
			CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method.MethodName = this.PrivateNamesUsed["initVariable"].ToString();
			cmMethod.Statements.Add(codeMethodInvokeExpression);
		}

		// Token: 0x06000346 RID: 838 RVA: 0x00020A38 File Offset: 0x0001EC38
		private void GenerateMethodToInitializeVariables()
		{
			CodeMemberMethod codeMemberMethod = new CodeMemberMethod();
			codeMemberMethod.Name = this.PrivateNamesUsed["initVariable"].ToString();
			codeMemberMethod.Attributes = (MemberAttributes)20482;
			codeMemberMethod.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(this.PrivateNamesUsed["AutoCommitProperty"].ToString()), new CodePrimitiveExpression(true)));
			codeMemberMethod.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(this.PrivateNamesUsed["IsEmbedded"].ToString()), new CodePrimitiveExpression(false)));
			this.cc.Members.Add(codeMemberMethod);
		}

		// Token: 0x06000347 RID: 839 RVA: 0x00020AF0 File Offset: 0x0001ECF0
		private static CodeMethodInvokeExpression GenerateConcatStrings(CodeExpression ce1, CodeExpression ce2)
		{
			CodeExpression[] parameters = new CodeExpression[]
			{
				ce1,
				ce2
			};
			return new CodeMethodInvokeExpression(new CodeTypeReferenceExpression("System.String"), "Concat", parameters);
		}

		// Token: 0x040001FA RID: 506
		private string VSVERSION = "8.0.0.0";

		// Token: 0x040001FB RID: 507
		private string OriginalServer = string.Empty;

		// Token: 0x040001FC RID: 508
		private string OriginalNamespace = string.Empty;

		// Token: 0x040001FD RID: 509
		private string OriginalClassName = string.Empty;

		// Token: 0x040001FE RID: 510
		private string OriginalPath = string.Empty;

		// Token: 0x040001FF RID: 511
		private bool bSingletonClass;

		// Token: 0x04000200 RID: 512
		private bool bUnsignedSupported = true;

		// Token: 0x04000201 RID: 513
		private string NETNamespace = string.Empty;

		// Token: 0x04000202 RID: 514
		private string arrConvFuncName = string.Empty;

		// Token: 0x04000203 RID: 515
		private string enumType = string.Empty;

		// Token: 0x04000204 RID: 516
		private const int DMTF_DATETIME_STR_LENGTH = 25;

		// Token: 0x04000205 RID: 517
		private bool bDateConversionFunctionsAdded;

		// Token: 0x04000206 RID: 518
		private bool bTimeSpanConversionFunctionsAdded;

		// Token: 0x04000207 RID: 519
		private ManagementClass classobj;

		// Token: 0x04000208 RID: 520
		private CodeDomProvider cp;

		// Token: 0x04000209 RID: 521
		private TextWriter tw;

		// Token: 0x0400020A RID: 522
		private string genFileName = string.Empty;

		// Token: 0x0400020B RID: 523
		private CodeTypeDeclaration cc;

		// Token: 0x0400020C RID: 524
		private CodeTypeDeclaration ccc;

		// Token: 0x0400020D RID: 525
		private CodeTypeDeclaration ecc;

		// Token: 0x0400020E RID: 526
		private CodeTypeDeclaration EnumObj;

		// Token: 0x0400020F RID: 527
		private CodeNamespace cn;

		// Token: 0x04000210 RID: 528
		private CodeMemberProperty cmp;

		// Token: 0x04000211 RID: 529
		private CodeConstructor cctor;

		// Token: 0x04000212 RID: 530
		private CodeMemberField cf;

		// Token: 0x04000213 RID: 531
		private CodeObjectCreateExpression coce;

		// Token: 0x04000214 RID: 532
		private CodeParameterDeclarationExpression cpde;

		// Token: 0x04000215 RID: 533
		private CodeIndexerExpression cie;

		// Token: 0x04000216 RID: 534
		private CodeMemberField cmf;

		// Token: 0x04000217 RID: 535
		private CodeMemberMethod cmm;

		// Token: 0x04000218 RID: 536
		private CodePropertyReferenceExpression cpre;

		// Token: 0x04000219 RID: 537
		private CodeMethodInvokeExpression cmie;

		// Token: 0x0400021A RID: 538
		private CodeExpressionStatement cmis;

		// Token: 0x0400021B RID: 539
		private CodeConditionStatement cis;

		// Token: 0x0400021C RID: 540
		private CodeBinaryOperatorExpression cboe;

		// Token: 0x0400021D RID: 541
		private CodeIterationStatement cfls;

		// Token: 0x0400021E RID: 542
		private CodeAttributeArgument caa;

		// Token: 0x0400021F RID: 543
		private CodeAttributeDeclaration cad;

		// Token: 0x04000220 RID: 544
		private ArrayList arrKeyType = new ArrayList(5);

		// Token: 0x04000221 RID: 545
		private ArrayList arrKeys = new ArrayList(5);

		// Token: 0x04000222 RID: 546
		private ArrayList BitMap = new ArrayList(5);

		// Token: 0x04000223 RID: 547
		private ArrayList BitValues = new ArrayList(5);

		// Token: 0x04000224 RID: 548
		private ArrayList ValueMap = new ArrayList(5);

		// Token: 0x04000225 RID: 549
		private ArrayList Values = new ArrayList(5);

		// Token: 0x04000226 RID: 550
		private SortedList PublicProperties = new SortedList(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04000227 RID: 551
		private SortedList PublicMethods = new SortedList(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04000228 RID: 552
		private SortedList PublicNamesUsed = new SortedList(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04000229 RID: 553
		private SortedList PrivateNamesUsed = new SortedList(StringComparer.OrdinalIgnoreCase);

		// Token: 0x0400022A RID: 554
		private ArrayList CommentsString = new ArrayList(5);

		// Token: 0x0400022B RID: 555
		private bool bHasEmbeddedProperties;

		// Token: 0x0400022C RID: 556
		private const int IDS_COMMENT_SHOULDSERIALIZE = 0;

		// Token: 0x0400022D RID: 557
		private const int IDS_COMMENT_ISPROPNULL = 1;

		// Token: 0x0400022E RID: 558
		private const int IDS_COMMENT_RESETPROP = 2;

		// Token: 0x0400022F RID: 559
		private const int IDS_COMMENT_ATTRIBPROP = 3;

		// Token: 0x04000230 RID: 560
		private const int IDS_COMMENT_DATECONVFUNC = 4;

		// Token: 0x04000231 RID: 561
		private const int IDS_COMMENT_GETINSTANCES = 5;

		// Token: 0x04000232 RID: 562
		private const int IDS_COMMENT_CLASSBEGIN = 6;

		// Token: 0x04000233 RID: 563
		private const int IDS_COMMENT_PRIV_AUTOCOMMIT = 7;

		// Token: 0x04000234 RID: 564
		private const int IDS_COMMENT_CONSTRUCTORS = 8;

		// Token: 0x04000235 RID: 565
		private const int IDS_COMMENT_ORIG_NAMESPACE = 9;

		// Token: 0x04000236 RID: 566
		private const int IDS_COMMENT_CLASSNAME = 10;

		// Token: 0x04000237 RID: 567
		private const int IDS_COMMENT_SYSOBJECT = 11;

		// Token: 0x04000238 RID: 568
		private const int IDS_COMMENT_LATEBOUNDOBJ = 12;

		// Token: 0x04000239 RID: 569
		private const int IDS_COMMENT_MGMTSCOPE = 13;

		// Token: 0x0400023A RID: 570
		private const int IDS_COMMENT_AUTOCOMMITPROP = 14;

		// Token: 0x0400023B RID: 571
		private const int IDS_COMMENT_MGMTPATH = 15;

		// Token: 0x0400023C RID: 572
		private const int IDS_COMMENT_PROP_TYPECONVERTER = 16;

		// Token: 0x0400023D RID: 573
		private const int IDS_COMMENT_SYSPROPCLASS = 17;

		// Token: 0x0400023E RID: 574
		private const int IDS_COMMENT_ENUMIMPL = 18;

		// Token: 0x0400023F RID: 575
		private const int IDS_COMMENT_LATEBOUNDPROP = 19;

		// Token: 0x04000240 RID: 576
		private const int IDS_COMMENTS_CREATEDCLASS = 20;

		// Token: 0x04000241 RID: 577
		private const int IDS_COMMENT_EMBEDDEDOBJ = 21;

		// Token: 0x04000242 RID: 578
		private const int IDS_COMMENT_CURRENTOBJ = 22;

		// Token: 0x04000243 RID: 579
		private const int IDS_COMMENT_FLAGFOREMBEDDED = 23;
	}
}
