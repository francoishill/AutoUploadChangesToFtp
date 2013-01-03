using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;
using SharedClasses;

namespace AutoUploadChangesToFtp
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			typeof(Form).GetField("defaultIcon", BindingFlags.NonPublic | BindingFlags.Static)
				.SetValue(null, new Icon(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("AutoUploadChangesToFtp.app.ico")));

			Form mainForm = new MainForm();
			AutoUpdating.CheckForUpdates_ExceptionHandler(
				delegate
				{
					ThreadingInterop.UpdateGuiFromThread(mainForm, () => mainForm.Text += " (up to date, version " + AutoUpdating.GetThisAppVersionString() + ")");
				});
			/*AutoUpdating.CheckForUpdates(
				//AutoUpdatingForm.CheckForUpdates(
				//exitApplicationAction: () => Application.Exit(),
				ActionIfUptoDate_Versionstring: (installedversion) => ThreadingInterop.UpdateGuiFromThread(mainForm, () => mainForm.Text += " (up to date, version " + installedversion + ")"));//,
				//ActionIfUnableToCheckForUpdates: (errmsg) => ThreadingInterop.UpdateGuiFromThread(mainForm, () => mainForm.Text += " (" + errmsg + ")"),
				//ShowModally: true);*/
			Application.Run(mainForm);
		}
	}
}
