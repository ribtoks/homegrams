using System;
using Gtk;
using System.IO;
using MacroProcesor;
using MacroEngine;

public partial class MainWindow : Gtk.Window
{
	public MainWindow () : base(Gtk.WindowType.Toplevel)
	{
		Build ();
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}
	
	protected virtual void OnButton3Clicked (object sender, System.EventArgs e)
	{
		this.textview3.Buffer.Text = 
			File.ReadAllText (this.filechooserbutton1.Filename);
	}	
	
	protected virtual void OnButton6Clicked (object sender, System.EventArgs e)
	{
		File.WriteAllText (this.filechooserbutton1.Filename, 
			this.textview3.Buffer.Text);
	}
	
	protected virtual void OnButton5Clicked (object sender, System.EventArgs e)
	{
		this.textview4.Buffer.Text = 
			File.ReadAllText (this.filechooserbutton2.Filename);
	}
	
	protected virtual void OnButton7Clicked (object sender, System.EventArgs e)
	{
		File.WriteAllText (this.filechooserbutton2.Filename, 
			this.textview4.Buffer.Text);
	}
	
	protected virtual void OnButton8Clicked (object sender, System.EventArgs e)
	{
		string macroFileName = this.filechooserbutton1.Filename;
		string textFileName = this.filechooserbutton2.Filename;
		
		MacroProcessor mp = new MacroProcessor (macroFileName, textFileName);
		try 
		{
			mp.ParseMacroses ();
		} 
		catch (Exception ex) 
		{
			MessageDialog md = new MessageDialog (null, DialogFlags.Modal,
				MessageType.Error, ButtonsType.Ok, ex.Message);
			md.Run ();
			md.Destroy ();
			return;
		}

		try 
		{
			mp.ApplyMacroses ();
		} 
		catch (Exception ex) 
		{
			MessageDialog md = new MessageDialog (null, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, ex.Message);
			md.Run ();
			md.Destroy ();
			return;
		}
		
		ResultsWindow window = new ResultsWindow ();
		window.SetField (File.ReadAllText (mp.ProcessedTextFile));
		window.Show ();
	}
	
	
	
	
	
}
