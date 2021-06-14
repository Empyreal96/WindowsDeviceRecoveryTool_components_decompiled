using System;
using System.Collections;
using System.Reflection;

namespace System.Management.Instrumentation
{
	// Token: 0x020000C0 RID: 192
	internal class SchemaMapping
	{
		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000517 RID: 1303 RVA: 0x0002433E File Offset: 0x0002253E
		public Type ClassType
		{
			get
			{
				return this.classType;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000518 RID: 1304 RVA: 0x00024346 File Offset: 0x00022546
		public ManagementClass NewClass
		{
			get
			{
				return this.newClass;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000519 RID: 1305 RVA: 0x0002434E File Offset: 0x0002254E
		public string ClassName
		{
			get
			{
				return this.className;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x0600051A RID: 1306 RVA: 0x00024356 File Offset: 0x00022556
		public string ClassPath
		{
			get
			{
				return this.classPath;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x0600051B RID: 1307 RVA: 0x0002435E File Offset: 0x0002255E
		public CodeWriter Code
		{
			get
			{
				return this.code;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x0600051C RID: 1308 RVA: 0x00024366 File Offset: 0x00022566
		public string CodeClassName
		{
			get
			{
				return this.codeClassName;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x0600051D RID: 1309 RVA: 0x0002436E File Offset: 0x0002256E
		public InstrumentationType InstrumentationType
		{
			get
			{
				return this.instrumentationType;
			}
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x00024376 File Offset: 0x00022576
		public static void ThrowUnsupportedMember(MemberInfo mi)
		{
			SchemaMapping.ThrowUnsupportedMember(mi, null);
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x0002437F File Offset: 0x0002257F
		public static void ThrowUnsupportedMember(MemberInfo mi, Exception innerException)
		{
			throw new ArgumentException(string.Format(RC.GetString("UNSUPPORTEDMEMBER_EXCEPT"), mi.Name), mi.Name, innerException);
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x000243A4 File Offset: 0x000225A4
		public SchemaMapping(Type type, SchemaNaming naming, Hashtable mapTypeToConverterClassName)
		{
			this.codeClassName = (string)mapTypeToConverterClassName[type];
			this.classType = type;
			bool flag = false;
			string baseClassName = ManagedNameAttribute.GetBaseClassName(type);
			this.className = ManagedNameAttribute.GetMemberName(type);
			this.instrumentationType = InstrumentationClassAttribute.GetAttribute(type).InstrumentationType;
			this.classPath = naming.NamespaceName + ":" + this.className;
			if (baseClassName == null)
			{
				this.newClass = new ManagementClass(naming.NamespaceName, "", null);
				this.newClass.SystemProperties["__CLASS"].Value = this.className;
			}
			else
			{
				ManagementClass managementClass = new ManagementClass(naming.NamespaceName + ":" + baseClassName);
				if (this.instrumentationType == InstrumentationType.Instance)
				{
					bool flag2 = false;
					try
					{
						QualifierData qualifierData = managementClass.Qualifiers["abstract"];
						if (qualifierData.Value is bool)
						{
							flag2 = (bool)qualifierData.Value;
						}
					}
					catch (ManagementException ex)
					{
						if (ex.ErrorCode != ManagementStatus.NotFound)
						{
							throw;
						}
					}
					if (!flag2)
					{
						throw new Exception(RC.GetString("CLASSINST_EXCEPT"));
					}
				}
				this.newClass = managementClass.Derive(this.className);
			}
			CodeWriter codeWriter = this.code.AddChild("public class " + this.codeClassName + " : IWmiConverter");
			CodeWriter codeWriter2 = codeWriter.AddChild(new CodeWriter());
			codeWriter2.Line("static ManagementClass managementClass = new ManagementClass(@\"" + this.classPath + "\");");
			codeWriter2.Line("static IntPtr classWbemObjectIP;");
			codeWriter2.Line("static Guid iidIWbemObjectAccess = new Guid(\"49353C9A-516B-11D1-AEA6-00C04FB68820\");");
			codeWriter2.Line("internal ManagementObject instance = managementClass.CreateInstance();");
			codeWriter2.Line("object reflectionInfoTempObj = null ; ");
			codeWriter2.Line("FieldInfo reflectionIWbemClassObjectField = null ; ");
			codeWriter2.Line("IntPtr emptyWbemObject = IntPtr.Zero ; ");
			codeWriter2.Line("IntPtr originalObject = IntPtr.Zero ; ");
			codeWriter2.Line("bool toWmiCalled = false ; ");
			codeWriter2.Line("IntPtr theClone = IntPtr.Zero;");
			codeWriter2.Line("public static ManagementObject emptyInstance = managementClass.CreateInstance();");
			codeWriter2.Line("public IntPtr instWbemObjectAccessIP;");
			CodeWriter codeWriter3 = codeWriter.AddChild("static " + this.codeClassName + "()");
			codeWriter3.Line("classWbemObjectIP = (IntPtr)managementClass;");
			codeWriter3.Line("IntPtr wbemObjectAccessIP;");
			codeWriter3.Line("Marshal.QueryInterface(classWbemObjectIP, ref iidIWbemObjectAccess, out wbemObjectAccessIP);");
			codeWriter3.Line("int cimType;");
			CodeWriter codeWriter4 = codeWriter.AddChild("public " + this.codeClassName + "()");
			codeWriter4.Line("IntPtr wbemObjectIP = (IntPtr)instance;");
			codeWriter4.Line("originalObject = (IntPtr)instance;");
			codeWriter4.Line("Marshal.QueryInterface(wbemObjectIP, ref iidIWbemObjectAccess, out instWbemObjectAccessIP);");
			codeWriter4.Line("FieldInfo tempField = instance.GetType().GetField ( \"_wbemObject\", BindingFlags.Instance | BindingFlags.NonPublic );");
			codeWriter4.Line("if ( tempField == null )");
			codeWriter4.Line("{");
			codeWriter4.Line("   tempField = instance.GetType().GetField ( \"wbemObject\", BindingFlags.Instance | BindingFlags.NonPublic ) ;");
			codeWriter4.Line("}");
			codeWriter4.Line("reflectionInfoTempObj = tempField.GetValue (instance) ;");
			codeWriter4.Line("reflectionIWbemClassObjectField = reflectionInfoTempObj.GetType().GetField (\"pWbemClassObject\", BindingFlags.Instance | BindingFlags.NonPublic );");
			codeWriter4.Line("emptyWbemObject = (IntPtr) emptyInstance;");
			CodeWriter codeWriter5 = codeWriter.AddChild("~" + this.codeClassName + "()");
			codeWriter5.AddChild("if(instWbemObjectAccessIP != IntPtr.Zero)").Line("Marshal.Release(instWbemObjectAccessIP);");
			codeWriter5.Line("if ( toWmiCalled == true )");
			codeWriter5.Line("{");
			codeWriter5.Line("\tMarshal.Release (originalObject);");
			codeWriter5.Line("}");
			CodeWriter codeWriter6 = codeWriter.AddChild("public void ToWMI(object obj)");
			codeWriter6.Line("toWmiCalled = true ;");
			codeWriter6.Line("if(instWbemObjectAccessIP != IntPtr.Zero)");
			codeWriter6.Line("{");
			codeWriter6.Line("    Marshal.Release(instWbemObjectAccessIP);");
			codeWriter6.Line("    instWbemObjectAccessIP = IntPtr.Zero;");
			codeWriter6.Line("}");
			codeWriter6.Line("if(theClone != IntPtr.Zero)");
			codeWriter6.Line("{");
			codeWriter6.Line("    Marshal.Release(theClone);");
			codeWriter6.Line("    theClone = IntPtr.Zero;");
			codeWriter6.Line("}");
			codeWriter6.Line("IWOA.Clone_f(12, emptyWbemObject, out theClone) ;");
			codeWriter6.Line("Marshal.QueryInterface(theClone, ref iidIWbemObjectAccess, out instWbemObjectAccessIP) ;");
			codeWriter6.Line("reflectionIWbemClassObjectField.SetValue ( reflectionInfoTempObj, theClone ) ;");
			codeWriter6.Line(string.Format("{0} instNET = ({0})obj;", type.FullName.Replace('+', '.')));
			CodeWriter codeWriter7 = codeWriter.AddChild("public static explicit operator IntPtr(" + this.codeClassName + " obj)");
			codeWriter7.Line("return obj.instWbemObjectAccessIP;");
			codeWriter2.Line("public ManagementObject GetInstance() {return instance;}");
			PropertyDataCollection properties = this.newClass.Properties;
			switch (this.instrumentationType)
			{
			case InstrumentationType.Instance:
				properties.Add("ProcessId", CimType.String, false);
				properties.Add("InstanceId", CimType.String, false);
				properties["ProcessId"].Qualifiers.Add("key", true);
				properties["InstanceId"].Qualifiers.Add("key", true);
				this.newClass.Qualifiers.Add("dynamic", true, false, false, false, true);
				this.newClass.Qualifiers.Add("provider", naming.DecoupledProviderInstanceName, false, false, false, true);
				break;
			case InstrumentationType.Abstract:
				this.newClass.Qualifiers.Add("abstract", true, false, false, false, true);
				break;
			}
			int num = 0;
			bool flag3 = false;
			foreach (MemberInfo memberInfo in type.GetMembers())
			{
				if ((memberInfo is FieldInfo || memberInfo is PropertyInfo) && memberInfo.GetCustomAttributes(typeof(IgnoreMemberAttribute), false).Length == 0)
				{
					if (memberInfo is FieldInfo)
					{
						FieldInfo fieldInfo = memberInfo as FieldInfo;
						if (fieldInfo.IsStatic)
						{
							SchemaMapping.ThrowUnsupportedMember(memberInfo);
						}
					}
					else if (memberInfo is PropertyInfo)
					{
						PropertyInfo propertyInfo = memberInfo as PropertyInfo;
						if (!propertyInfo.CanRead)
						{
							SchemaMapping.ThrowUnsupportedMember(memberInfo);
						}
						MethodInfo getMethod = propertyInfo.GetGetMethod();
						if (null == getMethod || getMethod.IsStatic)
						{
							SchemaMapping.ThrowUnsupportedMember(memberInfo);
						}
						if (getMethod.GetParameters().Length != 0)
						{
							SchemaMapping.ThrowUnsupportedMember(memberInfo);
						}
					}
					string memberName = ManagedNameAttribute.GetMemberName(memberInfo);
					Type type2;
					if (memberInfo is FieldInfo)
					{
						type2 = (memberInfo as FieldInfo).FieldType;
					}
					else
					{
						type2 = (memberInfo as PropertyInfo).PropertyType;
					}
					bool flag4 = false;
					if (type2.IsArray)
					{
						if (type2.GetArrayRank() != 1)
						{
							SchemaMapping.ThrowUnsupportedMember(memberInfo);
						}
						flag4 = true;
						type2 = type2.GetElementType();
					}
					string text = null;
					string text2 = null;
					if (mapTypeToConverterClassName.Contains(type2))
					{
						text2 = (string)mapTypeToConverterClassName[type2];
						text = ManagedNameAttribute.GetMemberName(type2);
					}
					bool flag5 = false;
					if (type2 == typeof(object))
					{
						flag5 = true;
						if (!flag)
						{
							flag = true;
							codeWriter2.Line("static Hashtable mapTypeToConverter = new Hashtable();");
							foreach (object obj in mapTypeToConverterClassName)
							{
								DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
								codeWriter3.Line(string.Format("mapTypeToConverter[typeof({0})] = typeof({1});", ((Type)dictionaryEntry.Key).FullName.Replace('+', '.'), (string)dictionaryEntry.Value));
							}
						}
					}
					string text3 = "prop_" + num;
					string text4 = "handle_" + num++;
					codeWriter2.Line("static int " + text4 + ";");
					codeWriter3.Line(string.Format("IWOA.GetPropertyHandle_f27(27, wbemObjectAccessIP, \"{0}\", out cimType, out {1});", memberName, text4));
					codeWriter2.Line("PropertyData " + text3 + ";");
					codeWriter4.Line(string.Format("{0} = instance.Properties[\"{1}\"];", text3, memberName));
					if (flag5)
					{
						CodeWriter codeWriter8 = codeWriter6.AddChild(string.Format("if(instNET.{0} != null)", memberInfo.Name));
						CodeWriter codeWriter9 = codeWriter6.AddChild("else");
						codeWriter9.Line(string.Format("{0}.Value = null;", text3));
						if (flag4)
						{
							codeWriter8.Line(string.Format("int len = instNET.{0}.Length;", memberInfo.Name));
							codeWriter8.Line("ManagementObject[] embeddedObjects = new ManagementObject[len];");
							codeWriter8.Line("IWmiConverter[] embeddedConverters = new IWmiConverter[len];");
							CodeWriter codeWriter10 = codeWriter8.AddChild("for(int i=0;i<len;i++)");
							CodeWriter codeWriter11 = codeWriter10.AddChild(string.Format("if((instNET.{0}[i] != null) && mapTypeToConverter.Contains(instNET.{0}[i].GetType()))", memberInfo.Name));
							codeWriter11.Line(string.Format("Type type = (Type)mapTypeToConverter[instNET.{0}[i].GetType()];", memberInfo.Name));
							codeWriter11.Line("embeddedConverters[i] = (IWmiConverter)Activator.CreateInstance(type);");
							codeWriter11.Line(string.Format("embeddedConverters[i].ToWMI(instNET.{0}[i]);", memberInfo.Name));
							codeWriter11.Line("embeddedObjects[i] = embeddedConverters[i].GetInstance();");
							codeWriter10.AddChild("else").Line(string.Format("embeddedObjects[i] = SafeAssign.GetManagementObject(instNET.{0}[i]);", memberInfo.Name));
							codeWriter8.Line(string.Format("{0}.Value = embeddedObjects;", text3));
						}
						else
						{
							CodeWriter codeWriter12 = codeWriter8.AddChild(string.Format("if(mapTypeToConverter.Contains(instNET.{0}.GetType()))", memberInfo.Name));
							codeWriter12.Line(string.Format("Type type = (Type)mapTypeToConverter[instNET.{0}.GetType()];", memberInfo.Name));
							codeWriter12.Line("IWmiConverter converter = (IWmiConverter)Activator.CreateInstance(type);");
							codeWriter12.Line(string.Format("converter.ToWMI(instNET.{0});", memberInfo.Name));
							codeWriter12.Line(string.Format("{0}.Value = converter.GetInstance();", text3));
							codeWriter8.AddChild("else").Line(string.Format("{0}.Value = SafeAssign.GetInstance(instNET.{1});", text3, memberInfo.Name));
						}
					}
					else if (text != null)
					{
						CodeWriter codeWriter13;
						if (type2.IsValueType)
						{
							codeWriter13 = codeWriter6;
						}
						else
						{
							codeWriter13 = codeWriter6.AddChild(string.Format("if(instNET.{0} != null)", memberInfo.Name));
							CodeWriter codeWriter14 = codeWriter6.AddChild("else");
							codeWriter14.Line(string.Format("{0}.Value = null;", text3));
						}
						if (flag4)
						{
							codeWriter13.Line(string.Format("int len = instNET.{0}.Length;", memberInfo.Name));
							codeWriter13.Line("ManagementObject[] embeddedObjects = new ManagementObject[len];");
							codeWriter13.Line(string.Format("{0}[] embeddedConverters = new {0}[len];", text2));
							CodeWriter codeWriter15 = codeWriter13.AddChild("for(int i=0;i<len;i++)");
							codeWriter15.Line(string.Format("embeddedConverters[i] = new {0}();", text2));
							if (type2.IsValueType)
							{
								codeWriter15.Line(string.Format("embeddedConverters[i].ToWMI(instNET.{0}[i]);", memberInfo.Name));
							}
							else
							{
								CodeWriter codeWriter16 = codeWriter15.AddChild(string.Format("if(instNET.{0}[i] != null)", memberInfo.Name));
								codeWriter16.Line(string.Format("embeddedConverters[i].ToWMI(instNET.{0}[i]);", memberInfo.Name));
							}
							codeWriter15.Line("embeddedObjects[i] = embeddedConverters[i].instance;");
							codeWriter13.Line(string.Format("{0}.Value = embeddedObjects;", text3));
						}
						else
						{
							codeWriter2.Line(string.Format("{0} lazy_embeddedConverter_{1} = null;", text2, text3));
							CodeWriter codeWriter17 = codeWriter.AddChild(string.Format("{0} embeddedConverter_{1}", text2, text3));
							CodeWriter codeWriter18 = codeWriter17.AddChild("get");
							CodeWriter codeWriter19 = codeWriter18.AddChild(string.Format("if(null == lazy_embeddedConverter_{0})", text3));
							codeWriter19.Line(string.Format("lazy_embeddedConverter_{0} = new {1}();", text3, text2));
							codeWriter18.Line(string.Format("return lazy_embeddedConverter_{0};", text3));
							codeWriter13.Line(string.Format("embeddedConverter_{0}.ToWMI(instNET.{1});", text3, memberInfo.Name));
							codeWriter13.Line(string.Format("{0}.Value = embeddedConverter_{0}.instance;", text3));
						}
					}
					else if (!flag4)
					{
						if (type2 == typeof(byte) || type2 == typeof(sbyte))
						{
							codeWriter6.Line(string.Format("{0} instNET_{1} = instNET.{1} ;", type2, memberInfo.Name));
							codeWriter6.Line(string.Format("IWOA.WritePropertyValue_f28(28, instWbemObjectAccessIP, {0}, 1, ref instNET_{1});", text4, memberInfo.Name));
						}
						else if (type2 == typeof(short) || type2 == typeof(ushort) || type2 == typeof(char))
						{
							codeWriter6.Line(string.Format("{0} instNET_{1} = instNET.{1} ;", type2, memberInfo.Name));
							codeWriter6.Line(string.Format("IWOA.WritePropertyValue_f28(28, instWbemObjectAccessIP, {0}, 2, ref instNET_{1});", text4, memberInfo.Name));
						}
						else if (type2 == typeof(uint) || type2 == typeof(int) || type2 == typeof(float))
						{
							codeWriter6.Line(string.Format("IWOA.WriteDWORD_f31(31, instWbemObjectAccessIP, {0}, instNET.{1});", text4, memberInfo.Name));
						}
						else if (type2 == typeof(ulong) || type2 == typeof(long) || type2 == typeof(double))
						{
							codeWriter6.Line(string.Format("IWOA.WriteQWORD_f33(33, instWbemObjectAccessIP, {0}, instNET.{1});", text4, memberInfo.Name));
						}
						else if (type2 == typeof(bool))
						{
							codeWriter6.Line(string.Format("if(instNET.{0})", memberInfo.Name));
							codeWriter6.Line(string.Format("    IWOA.WritePropertyValue_f28(28, instWbemObjectAccessIP, {0}, 2, ref SafeAssign.boolTrue);", text4));
							codeWriter6.Line("else");
							codeWriter6.Line(string.Format("    IWOA.WritePropertyValue_f28(28, instWbemObjectAccessIP, {0}, 2, ref SafeAssign.boolFalse);", text4));
						}
						else if (type2 == typeof(string))
						{
							CodeWriter codeWriter20 = codeWriter6.AddChild(string.Format("if(null != instNET.{0})", memberInfo.Name));
							codeWriter20.Line(string.Format("IWOA.WritePropertyValue_f28(28, instWbemObjectAccessIP, {0}, (instNET.{1}.Length+1)*2, instNET.{1});", text4, memberInfo.Name));
							codeWriter6.AddChild("else").Line(string.Format("IWOA.Put_f5(5, instWbemObjectAccessIP, \"{0}\", 0, ref nullObj, 8);", memberName));
							if (!flag3)
							{
								flag3 = true;
								codeWriter2.Line("object nullObj = DBNull.Value;");
							}
						}
						else if (type2 == typeof(DateTime) || type2 == typeof(TimeSpan))
						{
							codeWriter6.Line(string.Format("IWOA.WritePropertyValue_f28(28, instWbemObjectAccessIP, {0}, 52, SafeAssign.WMITimeToString(instNET.{1}));", text4, memberInfo.Name));
						}
						else
						{
							codeWriter6.Line(string.Format("{0}.Value = instNET.{1};", text3, memberInfo.Name));
						}
					}
					else if (type2 == typeof(DateTime) || type2 == typeof(TimeSpan))
					{
						codeWriter6.AddChild(string.Format("if(null == instNET.{0})", memberInfo.Name)).Line(string.Format("{0}.Value = null;", text3));
						codeWriter6.AddChild("else").Line(string.Format("{0}.Value = SafeAssign.WMITimeArrayToStringArray(instNET.{1});", text3, memberInfo.Name));
					}
					else
					{
						codeWriter6.Line(string.Format("{0}.Value = instNET.{1};", text3, memberInfo.Name));
					}
					CimType propertyType = CimType.String;
					if (!(memberInfo.DeclaringType != type))
					{
						bool flag6 = true;
						try
						{
							PropertyData propertyData = this.newClass.Properties[memberName];
							CimType type3 = propertyData.Type;
							if (propertyData.IsLocal)
							{
								throw new ArgumentException(string.Format(RC.GetString("MEMBERCONFLILCT_EXCEPT"), memberInfo.Name), memberInfo.Name);
							}
						}
						catch (ManagementException ex2)
						{
							if (ex2.ErrorCode != ManagementStatus.NotFound)
							{
								throw;
							}
							flag6 = false;
						}
						if (!flag6)
						{
							if (text != null)
							{
								propertyType = CimType.Object;
							}
							else if (flag5)
							{
								propertyType = CimType.Object;
							}
							else if (type2 == typeof(ManagementObject))
							{
								propertyType = CimType.Object;
							}
							else if (type2 == typeof(sbyte))
							{
								propertyType = CimType.SInt8;
							}
							else if (type2 == typeof(byte))
							{
								propertyType = CimType.UInt8;
							}
							else if (type2 == typeof(short))
							{
								propertyType = CimType.SInt16;
							}
							else if (type2 == typeof(ushort))
							{
								propertyType = CimType.UInt16;
							}
							else if (type2 == typeof(int))
							{
								propertyType = CimType.SInt32;
							}
							else if (type2 == typeof(uint))
							{
								propertyType = CimType.UInt32;
							}
							else if (type2 == typeof(long))
							{
								propertyType = CimType.SInt64;
							}
							else if (type2 == typeof(ulong))
							{
								propertyType = CimType.UInt64;
							}
							else if (type2 == typeof(float))
							{
								propertyType = CimType.Real32;
							}
							else if (type2 == typeof(double))
							{
								propertyType = CimType.Real64;
							}
							else if (type2 == typeof(bool))
							{
								propertyType = CimType.Boolean;
							}
							else if (type2 == typeof(string))
							{
								propertyType = CimType.String;
							}
							else if (type2 == typeof(char))
							{
								propertyType = CimType.Char16;
							}
							else if (type2 == typeof(DateTime))
							{
								propertyType = CimType.DateTime;
							}
							else if (type2 == typeof(TimeSpan))
							{
								propertyType = CimType.DateTime;
							}
							else
							{
								SchemaMapping.ThrowUnsupportedMember(memberInfo);
							}
							try
							{
								properties.Add(memberName, propertyType, flag4);
							}
							catch (ManagementException innerException)
							{
								SchemaMapping.ThrowUnsupportedMember(memberInfo, innerException);
							}
							if (type2 == typeof(TimeSpan))
							{
								PropertyData propertyData2 = properties[memberName];
								propertyData2.Qualifiers.Add("SubType", "interval", false, true, true, true);
							}
							if (text != null)
							{
								PropertyData propertyData3 = properties[memberName];
								propertyData3.Qualifiers["CIMTYPE"].Value = "object:" + text;
							}
						}
					}
				}
			}
			codeWriter3.Line("Marshal.Release(wbemObjectAccessIP);");
		}

		// Token: 0x04000515 RID: 1301
		private Type classType;

		// Token: 0x04000516 RID: 1302
		private ManagementClass newClass;

		// Token: 0x04000517 RID: 1303
		private string className;

		// Token: 0x04000518 RID: 1304
		private string classPath;

		// Token: 0x04000519 RID: 1305
		private string codeClassName;

		// Token: 0x0400051A RID: 1306
		private CodeWriter code = new CodeWriter();

		// Token: 0x0400051B RID: 1307
		private InstrumentationType instrumentationType;
	}
}
