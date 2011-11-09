using System;
using System.Collections.Generic;

namespace MacroEngine
{
	internal abstract class CodeBlock
	{
		internal abstract ExecutionResult ExecuteBlock(params object[] args);
		
		internal static Variables CheckVariables (object obj)
		{
			Variables vars = null;
			try 
			{
				vars = (Variables)obj;
			} 
			catch (InvalidCastException) 
			{
				throw new MustDieException ("Argument is not <Variables> objects");
			}
			
			return vars;
		}
		
		internal static Functions CheckFunctions (object obj)
		{
			Functions funcs = null;
			try 
			{
				funcs = (Functions)obj;
			} 
			catch (InvalidCastException) 
			{
				throw new MustDieException ("Argument is not <Functions> objects");
			}
			
			return funcs;
		}
	}
	
	internal class CodeConstruction
	{
		protected List<CodeBlock> construction;
		
		internal CodeConstruction ()
		{
			construction = new List<CodeBlock> ();
		}
		
		internal CodeConstruction (List<CodeBlock> codeBlocks)
		{
			construction = new List<CodeBlock> (codeBlocks);
		}
		
		internal List<CodeBlock> Construction
		{
			get { return construction; }
		}
		
		internal void AddCodeBlock (CodeBlock block)
		{
			construction.Add (block);
		}
		
		internal ExecutionResult ExecuteConstruction (params object[] args)
		{
			Variables globals = CodeBlock.CheckVariables (args[0]);
			Variables localVariables = CodeBlock.CheckVariables (args[1]);
			Functions functions = CodeBlock.CheckFunctions(args[2]);
			
			string output = "";
			
			for (int i = 0; i < construction.Count; ++i)
			{
				ExecutionResult tempRes = construction[i].ExecuteBlock (globals, 
					localVariables, functions);
				
				// if must return from code block
				if (localVariables.ContainsVariable (Globals.ReturnVariableName))
				{
					return new ExecutionResult (output + tempRes.PrintOutput);
				}
				
				output += tempRes.PrintOutput;
			}
			
			return new ExecutionResult (output);
		}
	}	
	
	internal class StatementBlock : CodeBlock
	{
		protected List<Statement> statements;
		
		internal StatementBlock ()
		{
			statements = new List<Statement> ();
		}
		
		internal StatementBlock (List<Statement> statementBlocks)
		{
			statements = new List<Statement> (statementBlocks);
		}
		
		internal void AddStatement (Statement st)
		{
			statements.Add (st);
		}
		
		internal override ExecutionResult ExecuteBlock (params object[] args)
		{
			Variables globals = CodeBlock.CheckVariables (args[0]);
			Variables localVariables = CodeBlock.CheckVariables (args[1]);
			Functions functions = CodeBlock.CheckFunctions (args[2]);
			
			string output = "";
			
			for (int i = 0; i < statements.Count; ++i) 
			{
				ExecutionResult tempRes = statements[i].Execute (globals, localVariables, functions);
				
				// if must return from code block
				if (localVariables.ContainsVariable (Globals.ReturnVariableName)) {
					return new ExecutionResult (output + tempRes.PrintOutput);
				}
				
				output += tempRes.PrintOutput;
			}
			
			return new ExecutionResult (output);
		}
		
		internal List<Statement> Statements
		{
			get { return statements; }
		}
	}	
	
	internal class RepeatBlock : CodeBlock
	{
		protected int repeatCount;
		protected CodeConstruction repeatWhat;
		
		internal RepeatBlock (int count, CodeConstruction construction)
		{
			repeatCount = count;
			repeatWhat = new CodeConstruction (construction.Construction);
		}
		
		internal RepeatBlock (RepeatBlock what)
			: this(what.repeatCount, what.repeatWhat)
		{
		}
		
		internal override ExecutionResult ExecuteBlock (params object[] args)
		{
			Variables globalVariables = CodeBlock.CheckVariables (args[0]);
			Variables localVariables = CodeBlock.CheckVariables (args[1]);
			Functions functions = CodeBlock.CheckFunctions (args[2]);
			
			string output = "";
			
			for (int i = 0; i < repeatCount; ++i)
			{
				localVariables[Globals.RepeatVariableName] = i.ToString ();
				
				ExecutionResult tempRes = repeatWhat.ExecuteConstruction (globalVariables, 
					localVariables, functions);
				
				if (localVariables.ContainsVariable (Globals.ReturnVariableName))
					return new ExecutionResult (output + tempRes.PrintOutput);
				
				output += tempRes.PrintOutput;
			}
			
			return new ExecutionResult (output);
		}

	}
	
	internal enum BoolOperation { Equal, NotEqual, Less, More }
	
	internal class IfBlock : CodeBlock
	{
		string condition;
		CodeConstruction ifBody;
		BoolOperation boolOp;
		
		internal IfBlock (string ifLine, CodeConstruction code)
		{
			condition = ifLine;
			ifBody = new CodeConstruction (code.Construction);
		}
		
		protected bool ProvideVariblesOperation (Variables var1Variables, Variables var2Variables, 
			string var1, string var2)
		{
			switch (boolOp)
			{
			case BoolOperation.Equal:
				return (var1Variables[var1].CompareTo (var2Variables[var2]) == 0);
			case BoolOperation.NotEqual:
				return (var1Variables[var1].CompareTo (var2Variables[var2]) != 0);
			case BoolOperation.Less:
				return (var1Variables[var1].CompareTo (var2Variables[var2]) < 0);
			case BoolOperation.More:
				return (var1Variables[var1].CompareTo (var2Variables[var2]) > 0);
			default:
				throw new MustDieException ();
			}
		}
		
		protected bool ProvideConstantOperation (Variables vars, string var1, string constant)
		{
			switch (boolOp)
			{
			case BoolOperation.Equal:
				return (vars[var1].CompareTo (constant) == 0);
			case BoolOperation.NotEqual:
				return (vars[var1].CompareTo (constant) != 0);
			case BoolOperation.Less:
				return (vars[var1].CompareTo (constant) < 0);
			case BoolOperation.More:
				return (vars[var1].CompareTo(constant) > 0);
			default:
				throw new MustDieException ();
			}
		}
		
		protected void CheckCondition ()
		{
			List<BoolOperation> operations = new List<BoolOperation> ();
			
			if (condition.IndexOf ("==") != -1)
				operations.Add (BoolOperation.Equal);
			
			if (condition.IndexOf ("!=") != -1)
				operations.Add (BoolOperation.NotEqual);
			
			if (condition.IndexOf ("<") != -1)
				operations.Add (BoolOperation.Less);
			
			if (condition.IndexOf (">") != -1)
				operations.Add (BoolOperation.More);
			
			if (operations.Count == 0)
				throw new SyntaxException ("No bool operation in <If> statement");
			
			if (operations.Count > 1)
				throw new SyntaxException ("Wrong format of <If> statement: to many bool operations");
			
			boolOp = operations[0];
		}
		
		internal override ExecutionResult ExecuteBlock (params object[] args)
		{
			CheckCondition ();
			
			// parse if block
			string[] parts = condition.Replace (" ", string.Empty).Split (
				new string[] { "==", "<", ">", "!=" }, StringSplitOptions.RemoveEmptyEntries);
			
			Variables globalVariables = CodeBlock.CheckVariables (args[0]);
			Variables localVariables = CodeBlock.CheckVariables (args[1]);
			Functions functions = CodeBlock.CheckFunctions (args[2]);
			
			string varName = parts[0];
			
			if (!localVariables.ContainsVariable (varName) && 
				!globalVariables.ContainsVariable (varName))
				throw new VariableNotFoundException ("Unknown variable in <If> statement");
			
			bool state = false;
			
			Variables vars1 = null;
			if (globalVariables.ContainsVariable (varName))
				vars1 = globalVariables;
			else
				if (localVariables.ContainsVariable (varName))
				vars1 = localVariables;
			
			Variables vars2 = null;
			if (globalVariables.ContainsVariable (parts[1]))
				vars2 = globalVariables;
			else
				if (localVariables.ContainsVariable (parts[1]))
				vars2 = localVariables;
			
			if (vars2 == null)
				state = ProvideConstantOperation (vars1, varName, parts[1]);
			else
				state = ProvideVariblesOperation (vars1, vars2, varName, parts[1]);
			
			if (state)
				return ifBody.ExecuteConstruction (globalVariables, localVariables, functions);
			
			return new ExecutionResult ("");
		}
	}
}
