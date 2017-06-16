// TuringInterpret.cpp : main project file.

#include "StdAfx.h"
#include "Form1.h"
//#include "Form21.h"

using namespace TuringInterpret;

[STAThreadAttribute]
int main(array<System::String ^> ^args)

{   
	// Enabling Windows XP visual effects before any controls are created
	Application::EnableVisualStyles();
	Application::SetCompatibleTextRenderingDefault(false); 

	// Create the main window and run it
	Application::Run(gcnew Form1());
	return 0;
}
