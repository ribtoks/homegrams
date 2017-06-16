using System;

namespace MacroEngine
{
	internal static class Globals
	{
		internal static string ReturnVariableName = "@returnVariable";
		
		internal static char[] Delimiters = {' ', '='};
		
		internal static string RepeatVariableName = "@iteration";
		
		internal static string DelcareVariableKeyword = "@declare";
		
		internal static string DeclareFunctionKeyword = "@declarefunc";
		
		internal static string FunctionEndKeyword = "@fend";
		
		internal static string RepeatKeyword = "@repeat";
		
		internal static string RepeatEndKewword = "@rend";
		
		internal static string IfKeyword = "@if";
		
		internal static string IfEndKeyword = "@ifend";
		
		internal static char[] StatementDelimiter = {';'};
	}
}
