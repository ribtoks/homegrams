using System;
using System.Collections.Generic;

namespace MacroEngine
{
	internal sealed class MacroVariable
	{
		private string name;
		private string varValue = "";
		
		internal MacroVariable (string varName, string initValue)
		{
			name = varName;
			varValue = initValue;
		}
		
		internal MacroVariable (string varName)
			: this(varName, "")
		{
		}
		
		internal string Name
		{
			get { return name; }
			set { name = value; }
		}
		
		internal string Value
		{
			get { return varValue; }
			set { varValue = value; }
		}
	}
	
	internal enum FuncReturnType {Void, Value}
	
	internal class FunctionResult : ExecutionResult
	{
		protected string result;
		protected FuncReturnType type;
		
		internal FunctionResult (string output, string fResult, FuncReturnType returnType)
			:base(output)
		{
			result = fResult;
			type = returnType;
		}
		
		internal string Result
		{
			get { return result; }
		}
		
		internal FuncReturnType ReturnType
		{
			get { return type; }
		}
	}
	
	
	internal class MacroFunction
	{
		protected string name;
		protected CodeConstruction body;
		protected List<string> parameters;
		
		internal MacroFunction (string funcName, CodeConstruction funcBody, List<string> funcParams)
		{
			name = funcName;
			body = funcBody;
			
			parameters = new List<string> (funcParams);
			
			//foreach (string param in funcParams)
			//	parameters[param] = string.Empty;
		}
		
		internal virtual FunctionResult Invoke (Variables globals, Functions functions, List<string> paramsValues)
		{
			if (paramsValues.Count != parameters.Count)
				throw new ParametersException ("Wrong parameters count");
		
			Variables locals = new Variables ();
			
			//foreach (MacroVariable mv in parameters)
			// set all parameters as local variables
			for (int i = 0; i < parameters.Count; ++i)
				locals[ parameters[i] ] = paramsValues[i];
			
			ExecutionResult execResult = body.ExecuteConstruction (globals, locals, functions);
			
			string returnValue = "";
			FuncReturnType retType = FuncReturnType.Void;
			if (locals.ContainsVariable (Globals.ReturnVariableName))
			{
				returnValue = locals[Globals.ReturnVariableName];
				retType = FuncReturnType.Value;
			}
			
			return new FunctionResult (execResult.PrintOutput, returnValue, retType);
		}
		
		internal string Name
		{
			get { return name; }
		}
		
		internal List<string> ParametersNames
		{
			get { return parameters; }
		}
	}
	
	internal class SystemFunction : MacroFunction
	{
		internal SystemFunction (string funcName, CodeConstruction funcBody, List<string> funcParams)
			: base(funcName, funcBody, funcParams)
		{
		}
	}
	
	internal class PrintFunction : SystemFunction
	{
		internal PrintFunction ()
			: base("@print", new CodeConstruction (), new List<string> ())
		{
		}
		
		internal override FunctionResult Invoke (Variables globals, Functions functions, List<string> paramsValues)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder ();
			
			for (int i = 0; i < paramsValues.Count; ++i)
				sb.Append (paramsValues[i]);
			
			return new FunctionResult (sb.ToString (), "", FuncReturnType.Void);
		}
	}
	
	internal class PrintLineFunction : SystemFunction
	{
		internal PrintLineFunction ()
			: base("@println", new CodeConstruction (), new List<string> ())
		{
		}
		
		internal override FunctionResult Invoke (Variables globals, Functions functions, List<string> paramsValues)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder ();
			
			for (int i = 0; i < paramsValues.Count; ++i)
				sb.AppendLine (paramsValues[i]);
			
			if (paramsValues.Count == 0)
				sb.AppendLine ();
			
			return new FunctionResult (sb.ToString (), "", FuncReturnType.Void);
		}
	}
	
	internal class ConcatFunction : SystemFunction
	{
		internal ConcatFunction () : base("@concat", new CodeConstruction (), new List<string> ())
		{
		}

		internal override FunctionResult Invoke (Variables globals, Functions functions, List<string> paramsValues)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder ();
			
			for (int i = 0; i < paramsValues.Count; ++i)
				sb.Append (paramsValues[i]);
			
			return new FunctionResult ("", sb.ToString (), FuncReturnType.Value);
		}
	}
	
	internal class ReduceFunction : SystemFunction
	{
		internal ReduceFunction () : base("@reduce", new CodeConstruction (), new List<string> ())
		{
		}

		internal override FunctionResult Invoke (Variables globals, Functions functions, List<string> paramsValues)
		{
			if (paramsValues.Count != 2)
				throw new ParametersException ("Wrong parameters count");
			
			int a = Convert.ToInt32 (paramsValues[1]);
			int index = paramsValues[0].Length;
			
			return new FunctionResult ("", paramsValues[0].Substring(0, index - a), FuncReturnType.Value);
		}
	}
	
	internal class LengthFunction : SystemFunction
	{
		internal LengthFunction () : base("@length", new CodeConstruction (), new List<string> ())
		{
		}

		internal override FunctionResult Invoke (Variables globals, Functions functions, List<string> paramsValues)
		{
			if (paramsValues.Count != 1)
				throw new ParametersException ("Wrong parameters count");
			
			return new FunctionResult ("", paramsValues[0].Length.ToString(), FuncReturnType.Value);
		}
	}
}
