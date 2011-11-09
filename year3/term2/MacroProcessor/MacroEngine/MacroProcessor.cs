using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MacroEngine
{
	public class MacroProcessor
	{
		MacroEnvironment env;
		MacroParser parser;
		string textFile;
		string processedTextFile;
		
		public MacroProcessor (string macroFilePath, string textFilePath)
		{
			parser = new MacroParser (macroFilePath);
			textFile = textFilePath;
			env = null;
		}
		
		public string ProcessedTextFile
		{
			get { return processedTextFile; }
		}
		
		public void ParseMacroses ()
		{
			env = parser.GetMacroEnvirontment ();
			
			List<SystemFunction> sysfunctions = new List<SystemFunction> ();
			
			sysfunctions.Add (new PrintFunction ());
			sysfunctions.Add (new PrintLineFunction ());
			sysfunctions.Add (new ConcatFunction ());
			sysfunctions.Add (new ReduceFunction ());
			sysfunctions.Add (new LengthFunction ());
			
			foreach (SystemFunction sf in sysfunctions)
			{
				env.Functions[sf.Name] = sf;
			}
		}
		
		public void ApplyMacroses ()
		{
			if (!File.Exists (textFile))
				throw new FileNotFoundException ("No such file: " + textFile);
			
			FileStream fs = new FileStream (textFile, FileMode.Open, FileAccess.Read);
			StreamReader sr = new StreamReader (fs);
			
			string newfilename = Path.GetFileNameWithoutExtension (textFile) + 
				"_processed" + Path.GetExtension (textFile);
			
			processedTextFile = textFile.Replace(
				Path.GetFileName(textFile), string.Empty) + newfilename;
			
			FileStream resFS = new FileStream (processedTextFile, 
				FileMode.Create, FileAccess.ReadWrite);
			StreamWriter sw = new StreamWriter (resFS);
			
			string currline = "";
			
			StringBuilder result = new StringBuilder ();
			StringBuilder curr = new StringBuilder ();
			
			try
			{
				while ((currline = sr.ReadLine ()) != null)
				{
					for (int i = 0; i < currline.Length; ++i)
					{
						// if we are reading letter or digit
						if (char.IsLetterOrDigit (currline[i]))
						{
							curr.Append (currline[i]);
						
							if (env.ContainsName (curr.ToString ()))
							{
								string currBuffer = curr.ToString ();
								curr.Length = 0;
							
								if (env.Variables.ContainsVariable (currBuffer))
								{
									result.Append (env.Variables[currBuffer]);
								}
								else
								{
									if (env.Functions.ContainsFunction (currBuffer))
									{
										string[] paramsLine;
										try
										{
											// read parameters
											paramsLine = parser.GetInnerString (currline,
											currline.IndexOf ('(', i - 1), 
											currline.IndexOf (')', i)).Replace (" ", string.Empty).Split (new char[] { ',' });
										}
										catch
										{
											continue;
										}
									
										List<string> paramsValues = new List<string> ();
									
										foreach (string s in paramsLine)
										{
											if (env.Variables.ContainsVariable (s))
												paramsValues.Add (env.Variables[s]);
											else
												paramsValues.Add (s);
										}
									
										int originalCount = env.Functions[currBuffer].ParametersNames.Count;
										int currCount = paramsValues.Count;
										if (currCount < originalCount)
										{
											for (int j = currCount; j < originalCount; ++j)
												paramsValues.Add (string.Empty);
										}
									
										ExecutionResult res = env.Functions[currBuffer].Invoke (env.Variables, 
											env.Functions, paramsValues);
										
										curr.Length = 0;
										result.Append (res.PrintOutput);
									
										// on next iteration i would be incremented
										i = currline.IndexOf (')', i);
									}
								}
								// if is function
							}
							// if not variable
						}
						else
						{
							result.Append (curr.ToString ());
							result.Append (currline[i]);
							curr.Length = 0;
						}
					}
					// general loop
					
					
					
					sw.WriteLine (result.ToString ());
					result.Length = 0;
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				sw.Close ();
				sr.Close ();
			}
		}
	}
}
