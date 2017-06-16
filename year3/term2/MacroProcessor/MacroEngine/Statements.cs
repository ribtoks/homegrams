using System;
using System.Collections.Generic;

namespace MacroEngine
{
	// ---------------------------------------------
	// ------------ Statement Object ---------------
	// ---------------------------------------------

	internal class ExecutionResult
	{
		protected string printOutput;
		
		internal ExecutionResult (string output)
		{
			printOutput = output;
		}
		
		internal string PrintOutput
		{
			get { return printOutput; }
		}
	}

	internal abstract class Statement
	{
		protected string line = "";
		
		internal Statement()
		{
		}
		
		internal Statement (string expression)
		{
			line = expression.TrimStart(' ', '\t');
		}
				
		protected abstract void Parse();
		internal abstract ExecutionResult Execute(params object[] args);
		
		internal string Line
		{
			get { return line; }
		}
	}
	
	internal class ReturnStatement : Statement
	{
		protected string returnWhat;
		
		internal ReturnStatement (string expression)
			: base(expression)
		{
			if (!expression.StartsWith ("@return"))
				throw new SyntaxException ("No <@return> in return statement.");
			Parse ();
		}
		
		internal ReturnStatement (ReturnStatement what)
			: this(what.line)
		{
			returnWhat = what.returnWhat;
		}
		
		protected override void Parse ()
		{
			string[] parts = line.Split (new string[] { "@return", " " }, 
			StringSplitOptions.RemoveEmptyEntries);
			
			if (parts.Length != 1)
				throw new SyntaxException ("Wrong return statement.");
			
			returnWhat = parts[0];		
		}
		
		internal override ExecutionResult Execute (params object[] args)
		{			
			Variables localVariables = CodeBlock.CheckVariables (args[1]);
			
			localVariables[Globals.ReturnVariableName] = returnWhat;
			
			return new ExecutionResult ("");
		}
	}
	
	internal class ReturnFuntionStatement : ReturnStatement
	{
		protected FunctionCallStatement functionCall;
		
		internal ReturnFuntionStatement (string expression)
			: base(expression)
		{
			if (!expression.StartsWith ("@return"))
				throw new SyntaxException ("No <@return> in return statement.");
			line = expression;
			
			Parse ();
		}
		
		internal ReturnFuntionStatement (ReturnFuntionStatement what)
			: this(what.line)
		{
			functionCall = new FunctionCallStatement (what.functionCall);
		}
		
		protected override void Parse ()
		{
			string[] parts = line.Replace (" ", string.Empty).Split (new string[] { "@return" }, 
			StringSplitOptions.RemoveEmptyEntries);
			
			if (parts.Length != 1)
				throw new SyntaxException ("Wrong return statememnt");
			
			string funcPart = parts[1];
			
			functionCall = new FunctionCallStatement (funcPart);
		}
		
		internal override ExecutionResult Execute (params object[] args)
		{
			Variables globalVariables = CodeBlock.CheckVariables (args[0]);
			Variables localVariables = CodeBlock.CheckVariables (args[1]);
			Functions functions = CodeBlock.CheckFunctions (args[2]);
			
			FunctionResult res = (FunctionResult)functionCall.Execute (globalVariables, functions);
			
			if (res.ReturnType == FuncReturnType.Void)
				throw new WrongReturnTypeException (string.Format ("Function <{0}> doesn't return a value", 
						functionCall.FunctionName));
			
			returnWhat = res.Result;
			
			localVariables[Globals.ReturnVariableName] = returnWhat;
			
			return new ExecutionResult (res.PrintOutput);
		}
	}
	
	// ---------------------------------------------
	// ----------- Assignment Object ---------------
	// ---------------------------------------------
	
	internal class AssignStatement : Statement
	{
		protected string varName = "";
		protected string newValue = "";
		
		internal AssignStatement()
		{
		}
		
		internal AssignStatement (string expression)
			: base(expression)
		{
			Parse ();
		}
		
		internal AssignStatement (AssignStatement what)
			: this(what.line)
		{
		}
		
		protected override void Parse ()
		{
			if (line.EndsWith (";"))
				throw new SyntaxException (string.Format ("Unexpected symbol: <{0}>", ","));
			
			string[] parts = line.Split (Globals.Delimiters, StringSplitOptions.RemoveEmptyEntries);
			
			if (parts.Length != 2)
				throw new SyntaxException ("Unexpected symbols.");
			
			varName = parts[0];
			newValue = parts[1];
		}
		
		protected void Assign (Variables globalVariables, Variables localVariables)
		{
			if (globalVariables.ContainsVariable (varName)) {
				if (globalVariables.ContainsVariable (newValue))
					globalVariables[varName] = globalVariables[newValue];
				else {
					if (localVariables.ContainsVariable (newValue))
						globalVariables[varName] = localVariables[newValue];
					else
						globalVariables[varName] = newValue;
				}
			} else {
				if (globalVariables.ContainsVariable (newValue))
					localVariables[varName] = globalVariables[newValue];
				else {
					if (localVariables.ContainsVariable (newValue))
						localVariables[varName] = localVariables[newValue];
					else
						localVariables[varName] = newValue;
				}
			}
		}
		
		internal override ExecutionResult Execute (params object[] args)
		{			
			Variables globalVariables = CodeBlock.CheckVariables (args[0]);
			Variables localVariables = CodeBlock.CheckVariables (args[1]);
			
			Assign (globalVariables, localVariables);
			
			return new ExecutionResult ("");
		}
	}
	
	internal class AssignFunctionStatement : AssignStatement
	{
		protected FunctionCallStatement functionCall;
		
		internal AssignFunctionStatement (string expression)
			:base()
		{
			line = expression;
			Parse ();
		}
		
		internal AssignFunctionStatement (AssignFunctionStatement what)
			: this(what.line)
		{
			functionCall = new FunctionCallStatement (what.functionCall);
		}
		
		internal void CheckFunctionCallLine (string expr)
		{
			if (expr.IndexOf ('(') == -1)
				throw new SyntaxException ("Left brace is missing");
			
			if (expr.IndexOf (')') == -1)
				throw new SyntaxException ("Right brace is missing");
			
			if (expr.IndexOf ('(') > expr.IndexOf (')'))
				throw new SyntaxException ("Wrong order of braces");
			
			if (expr[expr.Length - 1] != ')')
				throw new SyntaxException ("Wrong function statement");
		}
		
		protected override void Parse ()
		{
			string[] parts = line.Replace (" ", string.Empty).Split (Globals.Delimiters, 
				StringSplitOptions.RemoveEmptyEntries);
			if (parts.Length != 2)
				throw new SyntaxException ("Wrong expression");
			
			varName = parts[0];
			
			functionCall = new FunctionCallStatement (parts[1]);
		}
		
		internal override ExecutionResult Execute (params object[] args)
		{
			Variables globalVariables = CodeBlock.CheckVariables (args[0]);
			Variables localVariables = CodeBlock.CheckVariables (args[1]);
			Functions functions = CodeBlock.CheckFunctions (args[2]);
			
			FunctionResult res = (FunctionResult)functionCall.Execute (globalVariables, localVariables, functions);
			
			newValue = res.Result;
			
			if (res.ReturnType == FuncReturnType.Void)
				throw new WrongReturnTypeException (string.Format ("Function <{0}> doesn't return a value", 
						functionCall.FunctionName));
			
			if (globalVariables.ContainsVariable (varName))
				globalVariables[varName] = newValue;
			else
				// assign or create local variable
				localVariables[varName] = newValue;
			
			return new ExecutionResult (res.PrintOutput);
		}
	}
	
	internal class FunctionCallStatement : Statement
	{
		protected string functionName;
		protected List<string> parameters;
		protected List<string> parametersValues;
		
		internal FunctionCallStatement (string expression)
			: base(expression)
		{
			Parse ();
		}
		
		internal FunctionCallStatement (FunctionCallStatement what)
			: base(what.line)
		{
			functionName = what.functionName;
			parameters = new List<string> (what.parameters);
			parametersValues = new List<string> (what.parametersValues);
		}
		
		protected void CheckFunctionCallLine (string expr)
		{
			if (!expr.Contains ("("))
				throw new SyntaxException ("Left brace is missing");
			
			if (expr.IndexOf ('(') == 0)
				throw new SyntaxException ("No function name");
			
			if (!expr.Contains (")"))
				throw new SyntaxException ("Right brace is missing");
			
			if (expr.IndexOf ('(') > expr.IndexOf (')'))
				throw new SyntaxException ("Wrong order of braces");
			
			if (expr[expr.Length - 1] != ')')
				throw new SyntaxException ("Wrong function statement");
			
			if (Math.Abs (expr.Replace ("(", string.Empty).Length - expr.Length) > 1)
				throw new SyntaxException ("There're more than one left brace");
			
			if (Math.Abs (expr.Replace (")", string.Empty).Length - expr.Length) > 1)
				throw new SyntaxException ("There're more than one right brace");
		}
		
		protected override void Parse ()
		{
			string expr = line.Replace (" ", string.Empty);
			
			CheckFunctionCallLine (expr);
			functionName = expr.Substring (0, expr.IndexOf ('('));
			
			string args = expr.Substring (expr.IndexOf ('('));
			args = args.Substring (1, args.Length - 2);
			
			parameters = new List<string> (args.Split (new char[] { ',' }, 
			StringSplitOptions.RemoveEmptyEntries));
			parametersValues = new List<string> ();
			for (int i = 0; i < parameters.Count; ++i)
				parametersValues.Add ("");
		}

		internal override ExecutionResult Execute (params object[] args)
		{
			Variables globalVariables = CodeBlock.CheckVariables (args[0]);
			Variables locals = CodeBlock.CheckVariables (args[1]);
			Functions functions = CodeBlock.CheckFunctions (args[2]);
			
			for (int i = 0; i < parameters.Count; ++i)
			{
				if (locals.ContainsVariable (parameters[i]))
					parametersValues[i] = locals[parameters[i]];
				else
				{
					if (globalVariables.ContainsVariable (parameters[i]))
						parametersValues[i] = globalVariables[parameters[i]];
				}
			}
			
			return functions[functionName].Invoke (globalVariables, functions, parametersValues);
		}
		
		internal string FunctionName
		{
			get { return functionName; }
		}
		
		internal List<string> FunctionParams
		{
			get { return parameters; }
		}
	}
}