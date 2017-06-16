using System;
using MacroEngine;

namespace MacroProcConsole
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			MacroProcessor mp = new MacroProcessor (args[0], args[1]);
			try
			{
				mp.ParseMacroses ();
			}
			catch (Exception ex)
			{
				Console.WriteLine (ex.Message);
				return;
			}
#if RELEASE			
			try
			{
#endif
				mp.ApplyMacroses ();
#if RELEASE
			}
			catch (Exception ex)
			{
				Console.WriteLine (ex.Message);
			}
#endif
		}
	}
}
