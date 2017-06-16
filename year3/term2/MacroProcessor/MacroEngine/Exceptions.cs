using System;

namespace MacroEngine
{
	internal class VariableNotFoundException : ApplicationException
	{
		public VariableNotFoundException()
			: base()
		{
		}
		
		public VariableNotFoundException(string message)
			: base(message)
		{
		}
		
		public VariableNotFoundException (string message, 
			Exception innerException) : base(message, innerException)
		{
		}
	}	
	
	internal class FunctionNotFoundException : ApplicationException
	{
		public FunctionNotFoundException () : base()
		{
		}

		public FunctionNotFoundException (string message) : base(message)
		{
		}

		public FunctionNotFoundException (string message, Exception innerException) :
			base(message, innerException)
		{
		}
	}
	
	internal class SyntaxException : ApplicationException
	{
		public SyntaxException()
			: base()
		{
		}
		
		public SyntaxException(string message)
			: base(message)
		{
		}
		
		public SyntaxException (string message, 
			Exception innerException) : base(message, innerException)
		{
		}
	}
	
	internal class MustDieException : ApplicationException
	{
		public MustDieException()
			: base()
		{
		}
		
		public MustDieException(string message)
			: base(message)
		{
		}
		
		public MustDieException (string message, 
			Exception innerException) : base(message, innerException)
		{
		}
	}
	
	internal class ParametersException : ApplicationException
	{
		public ParametersException()
			: base()
		{
		}
		
		public ParametersException(string message)
			: base(message)
		{
		}
		
		public ParametersException (string message, 
			Exception innerException) : base(message, innerException)
		{
		}
	}
	
	internal class WrongReturnTypeException : ApplicationException
	{
		public WrongReturnTypeException () : base()
		{
		}

		public WrongReturnTypeException (string message) : base(message)
		{
		}

		public WrongReturnTypeException (string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
