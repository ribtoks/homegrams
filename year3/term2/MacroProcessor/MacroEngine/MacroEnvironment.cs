using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace MacroEngine
{
	internal class Variables : IEnumerable
	{
		private Dictionary<string, string> variables;
		
		internal Variables ()
		{
			variables = new Dictionary<string, string> ();
		}
		
		private void UpdateVariable (MacroVariable variable)
		{
			if (!variables.ContainsKey (variable.Name))
				variables.Add (variable.Name, variable.Value);
			else
				variables[variable.Name] = variable.Value;
		}
		
		public IEnumerator GetEnumerator ()
		{
			MacroVariable tempVar = new MacroVariable ("name");
			
			foreach (KeyValuePair<string, string> varPair in variables)
			{
				tempVar.Name = varPair.Key;
				tempVar.Value = varPair.Value;
				yield return tempVar;
			}
		}
		
		internal string this[string varName]
		{
			get 
			{
				if (variables.ContainsKey (varName))
					return variables[varName];
				throw new VariableNotFoundException (String.Format ("Variable <{0}> is not found", varName));
			}
			set 
			{
				UpdateVariable (new MacroVariable (varName, value));
			}
		}
		
		internal bool ContainsVariable (string name)
		{
			return variables.ContainsKey (name);
		}
	}

	internal class Functions
	{
		private Dictionary<string, MacroFunction> functions;
		
		internal Functions ()
		{
			functions = new Dictionary<string, MacroFunction> ();
		}
		
		private void UpdateFunction (MacroFunction func)
		{
			if (functions.ContainsKey (func.Name))
				functions[func.Name] = func;
			else
				functions.Add (func.Name, func);
		}
		
		internal MacroFunction this[string funcName]
		{
			get 
			{
				if (!functions.ContainsKey (funcName))
					throw new FunctionNotFoundException (string.Format ("No such function: <{0}>", 
							funcName));
				return functions[funcName];
			}
			
			set 
			{
				UpdateFunction (value);
			}
		}
		
		internal bool ContainsFunction (string funcName)
		{
			return functions.ContainsKey (funcName);
		}
	}
	
	internal class MacroEnvironment
	{
		Functions functions;
		Variables variables;
		
		internal MacroEnvironment ()
		{
			functions = new Functions ();
			variables = new Variables ();
		}
		
		internal Functions Functions
		{
			get { return functions; }
			set { functions = value; }
		}
		
		internal Variables Variables
		{
			get { return variables; }
			set { variables = value; }
		}
		
		internal bool ContainsName (string name)
		{
			return (variables.ContainsVariable (name) || functions.ContainsFunction (name));
		}
	}
	
	internal class MacroParser
	{
		private string filepath;
		
		internal MacroParser (string filePath)
		{
			filepath = filePath;
			
			if (!File.Exists (filepath))
				throw new FileNotFoundException (string.Format ("Can not find file specified: {0}", 
						Path.GetFileName (filepath)));
		}
		
		internal string GetInnerString (string line, int a, int b)
		{
			return line.Substring (a + 1, b - a - 1);
		}
		
		internal string GetBracesInnerText (string line)
		{
			return GetInnerString (line, line.IndexOf('('), line.IndexOf(')'));
		}
		
		protected CodeConstruction ReadConstruction (int startLine, StreamReader sr, 
			string stopString)
		{
			CodeConstruction body = new CodeConstruction ();
			string currline = "";
			StatementBlock statements = new StatementBlock ();
			int lineNumber = startLine;
			
			while ((currline = sr.ReadLine ()) != null) 
			{
				++lineNumber;
				
				currline = currline.TrimStart (' ', '\t');
				
				if (currline == stopString)
					break;
				
				if (string.IsNullOrEmpty (currline))
					continue;
				
				if (currline.StartsWith ("//"))
					continue;				
				
				if (currline.Contains ("(") && currline.Contains (")")) 
				{
					// it might be @if(), @repeat(), function call
					
					if (currline.Contains (Globals.RepeatKeyword)) 
					{
						body.AddCodeBlock (new StatementBlock (statements.Statements));
						statements = new StatementBlock ();
						
						int repeatCount = 0;
						try
						{
							repeatCount = Convert.ToInt32 (GetBracesInnerText (currline));
						}
						catch
						{
							throw new SyntaxException ("Wrong repeat construction at line " + lineNumber);
						}
						
						body.AddCodeBlock (ReadRepeatBlock (repeatCount, sr, lineNumber));
						continue;
					}
					
					if (currline.Contains (Globals.IfKeyword))
					{
						body.AddCodeBlock (new StatementBlock (statements.Statements));
						statements = new StatementBlock ();
						
						body.AddCodeBlock (ReadIfBlock (GetBracesInnerText (currline), sr, lineNumber));
						continue;
					}
					
					if (!currline.Contains (";"))
						throw new SyntaxException ("Statements delimiter expected");
					
					string templine = currline.Replace (" ", string.Empty);
					
					string[] parts = templine.Split (Globals.StatementDelimiter,
						StringSplitOptions.RemoveEmptyEntries);
					
					for (int i = 0; i < parts.Length; ++i)
					{
						if (parts[i].Contains ("(") && parts[i].Contains (")"))
						{
							// function call in this line
							if (parts[i].Contains ("=")) 
							{
								// assignment with functin call
								statements.AddStatement (new AssignFunctionStatement (parts[i]));
							} 
							else 
							{
								// just function call
								statements.AddStatement (new FunctionCallStatement (parts[i]));
							}
						}
						else
						{
							if (currline.Contains ("=")) 
							{
								// just assignment
								statements.AddStatement (new AssignStatement (parts[i]));
							}
							else
								throw new SyntaxException ("Wrong construction at line " + lineNumber);
						}
					}
				}
				else 
				{
					if (!currline.Contains (";"))
						throw new SyntaxException ("Statements delimiter expected");
					
					string tempLine = currline.Replace (" ", string.Empty);
					string[] parts = tempLine.Split (Globals.StatementDelimiter,
						StringSplitOptions.RemoveEmptyEntries);
					
					for (int i = 0; i < parts.Length; ++i)
					{
						if (parts[i].Contains ("=")) 
						{
							// just assignment
							statements.AddStatement (new AssignStatement (parts[i]));
						}
						else
							throw new SyntaxException ("Wrong construction at line " + lineNumber);
					}
				}
			}
			if (statements.Statements.Count > 0)
				body.AddCodeBlock (statements);
			
			return body;
		}
		
		protected MacroFunction ReadFunction (string name, 
			List<string> funcparams, StreamReader sr, int startLine)
		{
			return new MacroFunction (
				name,
				ReadConstruction (startLine, sr, Globals.FunctionEndKeyword), 
				funcparams);
		}
		
		protected RepeatBlock ReadRepeatBlock (int count, StreamReader sr, int startLine)
		{
			return new RepeatBlock (
				count, 
				ReadConstruction (startLine, sr, Globals.RepeatEndKewword));
		}
		
		protected IfBlock ReadIfBlock (string condition, StreamReader sr, int startLine)
		{
			return new IfBlock (
				condition,
				ReadConstruction (startLine, sr, Globals.IfEndKeyword));
		}
		
		internal MacroEnvironment GetMacroEnvirontment ()
		{
			FileStream fs = new FileStream (filepath, FileMode.Open, FileAccess.Read);
			StreamReader sr = new StreamReader (fs);
			
			MacroEnvironment macroEnv = new MacroEnvironment ();
			
			string currline;
			int lineNumber = 0;
			
			try
			{
				while ((currline = sr.ReadLine ()) != null)
				{
					++lineNumber;
					
					if (currline.StartsWith (Globals.DelcareVariableKeyword + " "))
					{
						string[] parts = currline.Split (new char[] { ' ' }, 
						StringSplitOptions.RemoveEmptyEntries);
						
						if (parts.Length != 3)
							throw new SyntaxException (string.Format ("Wrong variable declaration at line {0}", 
									lineNumber));
						
						macroEnv.Variables[parts[1]] = parts[2];
						continue;
					}
					
					if (currline.StartsWith (Globals.DeclareFunctionKeyword + " "))
					{
						FunctionCallStatement funcCall = null;
						try
						{
							funcCall = new FunctionCallStatement (
								currline.Substring (Globals.DeclareFunctionKeyword.Length));
						}
						catch (SyntaxException ex)
						{
							throw new SyntaxException (ex.Message);
						}
						
						macroEnv.Functions[funcCall.FunctionName] = 
							ReadFunction (funcCall.FunctionName, funcCall.FunctionParams, sr, lineNumber);
						continue;
					}
					
					if (currline.StartsWith (Globals.DeclareFunctionKeyword))
						throw new SyntaxException ("Wrong function declaration at line " + lineNumber);
					
					if (currline.StartsWith (Globals.DelcareVariableKeyword))
						throw new SyntaxException ("Wrong variable declaration at line " + lineNumber);
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				sr.Close();
			}
			
			return macroEnv;
		}
	}
}
