using System;

namespace MacroProcesor
{
	public partial class ResultsWindow : Gtk.Window
	{
		public ResultsWindow () : base(Gtk.WindowType.Toplevel)
		{
			this.Build ();
		}
		
		public void SetField (string text)
		{
			this.textview5.Buffer.Text = text;
		}
	}
}
