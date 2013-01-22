using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using SharedClasses;

namespace AutoUploadChangesToFtp
{
	public partial class MainForm : Form
	{
		private readonly TimeSpan DefaultInterval = TimeSpan.FromMinutes(1);

		private Icon originalNoChangesTrayIcon = null;
		private Icon hasChangesTrayIcon = null;
		private Icon busyCheckingChangesTrayIcon = null;

		List<LinkedFolderToFtp> linkedFolders = new List<LinkedFolderToFtp>();
		TextFeedbackEventHandler textFeedbackHandler;
		ProgressChangedEventHandler progressChangedHandler;
		private string filePathForIntervalMinutes = SettingsInterop.GetFullFilePathInLocalAppdata("intervaltocheck.fjset", "AutoUploadChangesToFtp");

		public MainForm()
		{
			InitializeComponent();
			notifyIconTrayIcon.Icon = this.Icon;

			LoadInterval();

			numericUpDownIntervalForChecking.ValueChanged += delegate { SaveInterval(); };

			originalNoChangesTrayIcon = notifyIconTrayIcon.Icon;
			Font fontX = new System.Drawing.Font(this.Font.FontFamily, 20, FontStyle.Bold);
			Font fontAt = new System.Drawing.Font(this.Font.FontFamily, 14, FontStyle.Bold);
			hasChangesTrayIcon = IconsInterop.GetIcon("*", Color.Transparent, Brushes.Maroon, fontX, notifyIconTrayIcon.Icon);//, new PointF(0, 0));
			busyCheckingChangesTrayIcon = IconsInterop.GetIcon("#", Color.Transparent, Brushes.White, fontAt, notifyIconTrayIcon.Icon);

			foreach (Control con in this.Controls)
				con.KeyPress += new KeyPressEventHandler(con_KeyPress);

			labelStatusMessage.Text = "";

			InitializeLinkedFolders();

			timerRoutineChecking.Start();
			AppendTextbox("Auto checking started every " + TimeSpan.FromMilliseconds(timerRoutineChecking.Interval).TotalMinutes + " minutes", false);

			textFeedbackHandler += (s, e) =>
			{
				AppendTextbox(e.FeedbackText, e.FeedbackType != TextFeedbackType.Subtle);
			};
			progressChangedHandler += (s, e) =>
			{
				UpdateProgess((int)Math.Truncate((double)100 * (double)e.CurrentValue / (double)e.MaximumValue));
			};
		}

		private void SaveInterval()
		{
			try
			{
				File.WriteAllText(filePathForIntervalMinutes, numericUpDownIntervalForChecking.Value.ToString());
			}
			catch (Exception exc)
			{
				UserMessages.ShowErrorMessage("Unable to save new interval: " + exc.Message);
			}
		}

		private void LoadInterval()
		{
			if (!File.Exists(filePathForIntervalMinutes))
			{
				numericUpDownIntervalForChecking.Value = (int)DefaultInterval.TotalMinutes;
				SaveInterval();
				return;
			}

			int tmpIntervalMinutes;
			try
			{
				if (int.TryParse(File.ReadAllText(filePathForIntervalMinutes), out tmpIntervalMinutes))
				{
					timerRoutineChecking.Interval = (int)TimeSpan.FromMinutes(tmpIntervalMinutes).TotalMilliseconds;
					numericUpDownIntervalForChecking.Value = tmpIntervalMinutes;
				}
			}
			catch (Exception exc)
			{
				UserMessages.ShowErrorMessage("Unable to read interval from file: " + exc.Message);
			}
		}

		void con_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == 27)//Escape key
				this.Close();
		}

		private void InitializeLinkedFolders()
		{
			string dir = LinkedFolderToFtp.GetJsonFolderPath();

			var jsonfiles = Directory.GetFiles(dir, "*.json", SearchOption.TopDirectoryOnly);
			if (jsonfiles.Length == 0)
				ChangeLinkedFolders();

			RefreshLinkedFolderList(dir);
		}

		private void ChangeLinkedFolders()
		{
			NewLinkedFolder tmpform = new NewLinkedFolder();
			tmpform.RemoveAllUsercontrols();
			foreach (var lf in linkedFolders)
			{
				var uc = tmpform.AddAnotherLinkedFolder(lf.LocalRootDirectory, lf.FtpRootUrl, lf.FtpUsername, lf.FtpPassword, lf.ExcludedRelativeFolders);
				uc.Tag = lf;
			}
			if (tmpform.ShowDialog(this) == DialogResult.OK)
			{
				foreach (LinkedFolderUserControl uc in tmpform.GetUsercontrols())
				{
					var linkedFolderTag = uc.Tag as LinkedFolderToFtp;
					if (linkedFolderTag != null)
					{
						if (linkedFolderTag.LocalRootDirectory == uc.textBoxLocalRootDirectory.Text
							&& linkedFolderTag.FtpRootUrl == uc.textBoxFtpRootUrl.Text
							&& linkedFolderTag.FtpUsername == uc.textBoxFtpUsername.Text
							&& linkedFolderTag.FtpPassword == uc.textBoxFtpPassword.Text
							&& LinkedFolderToFtp.AreStringListsEqual(linkedFolderTag.ExcludedRelativeFolders, uc.textBoxExcludedFolders.Text.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim('\\')).ToList())
							)
							continue;
					}

					bool isNew = false;
					if (linkedFolderTag == null)
					{
						isNew = true;
						linkedFolderTag = new LinkedFolderToFtp();
					}
					linkedFolderTag.LocalRootDirectory = uc.textBoxLocalRootDirectory.Text;
					linkedFolderTag.FtpRootUrl = uc.textBoxFtpRootUrl.Text;
					linkedFolderTag.FtpUsername = uc.textBoxFtpUsername.Text;
					linkedFolderTag.FtpPassword = uc.textBoxFtpPassword.Text;
					linkedFolderTag.ExcludedRelativeFolders = uc.textBoxExcludedFolders.Text.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim('\\')).ToList();
					if (isNew)
					{
						linkedFolderTag.RegenerateFilesList();
						TextFeedbackEventArgs.RaiseTextFeedbackEvent_Ifnotnull(null, textFeedbackHandler,
							"New linked folder added, changes will be monitored from now on in local directory: " + linkedFolderTag.LocalRootDirectory);
					}
					linkedFolderTag.SaveDetails();
				}
				foreach (object removedFolders in tmpform.RemovedUsercontrolNonNullTags)
				{
					LinkedFolderToFtp linkedFol = removedFolders as LinkedFolderToFtp;
					if (linkedFol == null) continue;
					linkedFol.RemoveDetails();
				}
			}
		}

		private void RefreshLinkedFolderList(string dir)
		{
			linkedFolders.Clear();
			var jsonfiles = Directory.GetFiles(dir, "*.json", SearchOption.TopDirectoryOnly);
			foreach (var jf in jsonfiles)
				linkedFolders.Add(LinkedFolderToFtp.CreateFromJsonFile(jf));
		}

		private void timerRoutineChecking_Tick(object sender, EventArgs e)
		{
			CheckForChanges();
		}

		private void AppendTextbox(string text, bool ShowNotificationIfFormHidden)
		{
			ThreadingInterop.UpdateGuiFromThread(this, delegate
			{
				textBox1.AppendText(
					(textBox1.Text.Length > 0 ? Environment.NewLine : "") +
					string.Format("[{0}]: {1}", DateTime.Now.ToString("HH:mm:ss"), text));
				if (ShowNotificationIfFormHidden && !this.Visible)
					notifyIconTrayIcon.ShowBalloonTip(3000, "Notification", text, ToolTipIcon.Info);
			});
		}

		private void UpdateProgess(int percentage)
		{
			ThreadingInterop.UpdateGuiFromThread(this, delegate
			{
				if (progressBar1.Value != percentage)
				{
					progressBar1.Value = percentage;
					Application.DoEvents();
				}
				if (percentage == 100)
				{
					progressBar1.Value = 0;
					labelStatusMessage.Text = "Completed";
				}
			});
		}

		private void buttonCheckForChanges_Click(object sender, EventArgs e)
		{
			CheckForChanges(true);
		}

		private void buttonAutoUploadChanged_Click(object sender, EventArgs e)
		{
			CheckForChanges(false);
		}

		private bool IsBusyChecking = false;
		private void CheckForChanges(bool confirmIfChangesBeforeUpload = true)
		{
			ThreadingInterop.PerformVoidFunctionSeperateThread(() =>
			{
				if (IsBusyChecking)
				{
					AppendTextbox("Cannot check for changes, check is already in progress.", true);
					return;
				}

				IsBusyChecking = true;
				AppendTextbox("Checking for changes in all linked folders...", false);

				string oldNotifyText = notifyIconTrayIcon.Text;
				try
				{
					notifyIconTrayIcon.Icon = busyCheckingChangesTrayIcon;
					notifyIconTrayIcon.Text = "Busy checking for local changes";
					bool anyFolderHasChanges = false;
					foreach (var linkedFol in linkedFolders)
					{
						var currentDetails = new LinkedFolderToFtp(linkedFol.LocalRootDirectory, linkedFol.FtpRootUrl, linkedFol.FtpUsername, linkedFol.FtpPassword, linkedFol.ExcludedRelativeFolders);
						if (currentDetails.CompareToCachedAndUploadChanges(confirmIfChangesBeforeUpload, textFeedbackHandler, progressChangedHandler, this))
							anyFolderHasChanges = true;
					}
					notifyIconTrayIcon.Icon = anyFolderHasChanges ? hasChangesTrayIcon : originalNoChangesTrayIcon;
				}
				finally
				{
					IsBusyChecking = false;
					notifyIconTrayIcon.Text = oldNotifyText;
				}
			},
			false);
		}

		private void checkBoxTopmost_CheckedChanged(object sender, EventArgs e)
		{
			this.TopMost = checkBoxTopmost.Checked;
		}

		private bool ForceClose = false;
		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.UserClosing && !ForceClose)
			{
				e.Cancel = true;
				this.HideThisForm();
			}
		}

		private void showToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.ShowThisForm();
			this.BringToFront();

			this.TopMost = !this.TopMost;
			this.TopMost = !this.TopMost;
		}

		private void checkForchangesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CheckForChanges(true);
		}

		private void autouploadChangesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CheckForChanges(false);
		}

		private void addMoreLinkedDirectoriesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ChangeLinkedFolders();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.ForceClose = true;
			this.Close();
		}

		private void buttonChangeLinkedFolders_Click(object sender, EventArgs e)
		{
			ChangeLinkedFolders();
		}

		private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				ToggleShowHide();
			}
		}

		private void ToggleShowHide()
		{
			if (this.Visible)
				this.HideThisForm();
			else
				this.ShowThisForm();
		}

		private void ShowThisForm()
		{
			this.Show();
			this.BringToFront();
			this.TopMost = !this.TopMost;
			this.TopMost = !this.TopMost;
		}

		private void HideThisForm()
		{
			this.Hide();
		}

		private void Form1_Shown(object sender, EventArgs e)
		{
			this.HideThisForm();
			if (!Win32Api.RegisterHotKey(this.Handle, Win32Api.Hotkey1, Win32Api.MOD_WIN, (uint)Keys.C))
				UserMessages.ShowWarningMessage("AutoUploadChangesToFtp could not register hotkey Win + C");
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == Win32Api.WM_HOTKEY)
				if (m.WParam == new IntPtr(Win32Api.Hotkey1))
					ToggleShowHide();
			base.WndProc(ref m);
		}

		private void linkLabel1_Click(object sender, EventArgs e)
		{
			textBox1.Clear();
			AppendTextbox("Cleared all messages.", false);
		}

		private void buttonRabaseMetadata_Click(object sender, EventArgs e)
		{
			if (linkedFolders.Count == 0)
			{
				UserMessages.ShowWarningMessage("No linked folders yet");
				return;
			}
			LinkedFolderToFtp pickedItem =
				linkedFolders.Count > 1
				? PickItemForm.PickItem<LinkedFolderToFtp>(linkedFolders, "Please select a linked folder to rebase metadata.", null)
				: (UserMessages.Confirm("Only one linked folder, confirm to rebase it?") ? linkedFolders[0] : null);
			if (pickedItem != null)
			{
				pickedItem.RegenerateFilesList();
				pickedItem.SaveDetails();
			}
		}

		private void labelAbout_Click(object sender, EventArgs e)
		{
			AboutWindow2.ShowAboutWindow(new System.Collections.ObjectModel.ObservableCollection<DisplayItem>()
			{
				new DisplayItem("Author", "Francois Hill"),
				new DisplayItem("Icon(s) obtained from", null)//"http://www.icons-land.com", "http://www.icons-land.com/vista-base-software-icons.php")
			});
		}
	}

	public class LinkedFolderToFtp
	{
		private const int cMaxFilesToShowInUsermessages = 20;

		public string LocalRootDirectory;
		public string FtpRootUrl;
		public string FtpUsername;
		public string FtpPassword;
		public FileDetails[] Files;

		public List<string> ExcludedRelativeFolders;
		//TODO: Add support for excluded files
		//public List<string> ExcludedFiles;

		public LinkedFolderToFtp() { }
		public LinkedFolderToFtp(string LocalRootDirectory, string FtpRootUrl, string FtpUsername, string FtpPassword, List<string> ExcludedRelativeFolders)
		{
			LocalRootDirectory = LocalRootDirectory.TrimEnd('\\');
			this.LocalRootDirectory = LocalRootDirectory;
			this.FtpRootUrl = FtpRootUrl.TrimEnd('/', '\\');
			this.FtpUsername = FtpUsername;
			this.FtpPassword = FtpPassword;
			this.ExcludedRelativeFolders = ExcludedRelativeFolders;
			RegenerateFilesList();
		}

		public override string ToString()
		{
			return this.LocalRootDirectory + "(" + this.FtpRootUrl + ")";
		}

		public void RegenerateFilesList()
		{
			var files = new List<FileDetails>();
			string pathNoSlash = LocalRootDirectory;
			foreach (var f in Directory.GetFiles(pathNoSlash, "*", SearchOption.AllDirectories))
			{
				//TODO: Must maybe give user option to ignore .svn or not
				if (f.IndexOf(".svn", StringComparison.InvariantCultureIgnoreCase) == -1
					&& !MustPathBeExcluded(FileDetails.GetRelativePath(pathNoSlash, f)))
					files.Add(new FileDetails(pathNoSlash, f));
			}
			this.Files = files.ToArray();
		}

		public void SaveDetails()
		{
			JSON.SetDefaultJsonInstanceSettings();
			var json = JSON.Instance.ToJSON(this, false);
			File.WriteAllText(
				GetCachedJsonFilepath(),
				json);
		}
		public void RemoveDetails()
		{
			if (File.Exists(GetCachedJsonFilepath()))
				File.Delete(GetCachedJsonFilepath());
		}

		public static LinkedFolderToFtp CreateFromJsonFile(string filePath)
		{
			JSON.SetDefaultJsonInstanceSettings();
			LinkedFolderToFtp tmpObj = new LinkedFolderToFtp();
			if (JSON.Instance.FillObject(tmpObj, File.ReadAllText(filePath)) != null)
				return tmpObj;
			return null;
		}

		public string GetAbsolutePath(FileDetails fd)
		{
			return LocalRootDirectory.TrimEnd('\\') + "\\" + fd.RelativePath.TrimStart('\\');
		}

		//Null returned if something besides the filedetails failed
		public bool? CompareToCached(out Dictionary<string, FileDetails> newFiles, out Dictionary<string, FileDetails> deletedFiles, out Dictionary<string, FileDetails> changedFiles, out LinkedFolderToFtp cachedDetails)
		{
			newFiles = null;
			deletedFiles = null;
			changedFiles = null;
			cachedDetails = null;

			if (!File.Exists(GetCachedJsonFilepath()))
				return false;

			JSON.SetDefaultJsonInstanceSettings();
			cachedDetails = new LinkedFolderToFtp();
			if (JSON.Instance.FillObject(cachedDetails, File.ReadAllText(GetCachedJsonFilepath())) != null)
				return cachedDetails.CompareDetails(this, out newFiles, out deletedFiles, out changedFiles);
			return false;
		}

		public static string GetJsonFolderPath()
		{
			string dir = SettingsInterop.LocalAppdataPath("AutoUploadChangesToFtp").TrimEnd('\\');
			if (!Directory.Exists(dir))
				Directory.CreateDirectory(dir);
			return dir;
		}
		private string GetCachedJsonFilepath()
		{
			return GetJsonFolderPath() + "\\" + FileSystemInterop.FilenameEncodeToValid(LocalRootDirectory, (err) => UserMessages.ShowErrorMessage(err)) + ".json";
		}

		private bool MustPathBeExcluded(string relativePath)
		{
			if (ExcludedRelativeFolders == null)
				return false;
			foreach (var exclPath in ExcludedRelativeFolders)
				if (relativePath.StartsWith(exclPath.TrimEnd('\\'), StringComparison.InvariantCultureIgnoreCase))
					return true;
			return false;
		}

		//Null returned if something besides the filedetails failed
		public bool? CompareDetails(LinkedFolderToFtp otherFolderDet, out Dictionary<string, FileDetails> newIn2, out Dictionary<string, FileDetails> missingIn2, out Dictionary<string, FileDetails> changedItems)
		{
			newIn2 = null;
			missingIn2 = null;
			changedItems = null;

			//Does not compare paths
			if (this.FtpRootUrl != otherFolderDet.FtpRootUrl)
				return null;
			if (!AreStringListsEqual(this.ExcludedRelativeFolders, otherFolderDet.ExcludedRelativeFolders))
				return null;

			if (this.Files == null)
			{
				newIn2 = new Dictionary<string, FileDetails>();
				foreach (var f in otherFolderDet.Files)
					newIn2.Add(f.RelativePath, f);

				changedItems = new Dictionary<string, FileDetails>();
				missingIn2 = new Dictionary<string, FileDetails>();
				return false;
			}

			var tmpDict1= new Dictionary<string, FileDetails>();
			foreach (var f in this.Files)
				tmpDict1.Add(f.RelativePath, f);

			var tmpDict2 = new Dictionary<string, FileDetails>();
			foreach (var f in otherFolderDet.Files)
				tmpDict2.Add(f.RelativePath, f);

			newIn2 = new Dictionary<string, FileDetails>();
			foreach (var fpath in tmpDict2.Keys)
				if (!tmpDict1.ContainsKey(fpath))
					if (!MustPathBeExcluded(fpath))
						newIn2.Add(fpath, tmpDict2[fpath]);

			missingIn2 = new Dictionary<string, FileDetails>();
			foreach (var fpath in tmpDict1.Keys)
				if (!tmpDict2.ContainsKey(fpath))
					if (!MustPathBeExcluded(fpath))
						missingIn2.Add(fpath, tmpDict1[fpath]);

			changedItems = new Dictionary<string, FileDetails>();
			foreach (var fpath in tmpDict1.Keys)
				if (tmpDict2.ContainsKey(fpath))
				{
					var file1 = tmpDict1[fpath];
					var file2 = tmpDict2[fpath];

					if (!file1.Equals(file2))
						if (!MustPathBeExcluded(fpath))
							changedItems.Add(fpath, tmpDict1[fpath]);
				}

			return
				newIn2.Count == 0
				&& missingIn2.Count == 0
				&& changedItems.Count == 0;
		}

		public override bool Equals(object obj)
		{
			LinkedFolderToFtp otherFolderDet = obj as LinkedFolderToFtp;
			if (otherFolderDet == null)
				return false;

			//Does not compare paths
			Dictionary<string, FileDetails> newIn2;
			Dictionary<string, FileDetails> missingIn2;
			Dictionary<string, FileDetails> changedItems;
			return CompareDetails(otherFolderDet, out newIn2, out missingIn2, out changedItems) == true;
		}
		public override int GetHashCode() { return 0; }

		public bool CompareToCachedAndUploadChanges(bool confirmIfChangesBeforeUpload, TextFeedbackEventHandler textFeedbackHandler = null, ProgressChangedEventHandler progressChangedHandler = null, Form formToCheckIfVisible = null)
		{
			bool hasChanges = false;

			LinkedFolderToFtp cachedDetails;// = new FolderDetails();

			Dictionary<string, FileDetails> newFiles;
			Dictionary<string, FileDetails> deletedFiles;
			Dictionary<string, FileDetails> changedFiles;

			if (this.ExcludedRelativeFolders != null && this.ExcludedRelativeFolders.Count > 0)
				TextFeedbackEventArgs.RaiseTextFeedbackEvent_Ifnotnull(null, textFeedbackHandler, "There are excluded folders.", TextFeedbackType.Subtle);
			bool? compareResult = this.CompareToCached(out newFiles, out deletedFiles, out changedFiles, out cachedDetails);
			if (compareResult != true)
			{
				if (compareResult == null)
				{
					TextFeedbackEventArgs.RaiseTextFeedbackEvent_Ifnotnull(null, textFeedbackHandler, "Cannot compare current and cached files, different FtpUrls.", TextFeedbackType.Error);
					//UserMessages.ShowWarningMessage("Cannot compare current and cached files, different FtpUrls.");
					return true;//Saying comparison true, because cannot really compare if FTP URLs differ
				}
				if (newFiles == null)//&& deletedFiles == null && changedFiles == null)
				{
					this.SaveDetails();
					TextFeedbackEventArgs.RaiseTextFeedbackEvent_Ifnotnull(null, textFeedbackHandler, "Cannot compare current and cached files, cached file was missing and got regenerated.", TextFeedbackType.Error);
					//UserMessages.ShowWarningMessage("Cannot compare current and cached files, cached file was missing and got regenerated.");
					return true;//Cached files was missing and got regenerated, therefore no changes at this point
				}


				hasChanges = changedFiles.Count > 0 || newFiles.Count > 0 || deletedFiles.Count > 0;

				string confirmMessage = 
					string.Format("There are {0} changed files, continue to upload them to the ftp site?{1}{2}", changedFiles.Count, Environment.NewLine,
						string.Join(Environment.NewLine, changedFiles.Keys.Count <= cMaxFilesToShowInUsermessages ? changedFiles.Keys : changedFiles.Keys.Take(cMaxFilesToShowInUsermessages))
						+ (changedFiles.Keys.Count <= cMaxFilesToShowInUsermessages ? "" : Environment.NewLine + "..." + Environment.NewLine + "and another " + (changedFiles.Keys.Count - cMaxFilesToShowInUsermessages) + " files"));

				if (changedFiles.Count > 0 && (!confirmIfChangesBeforeUpload ||
					((formToCheckIfVisible == null || formToCheckIfVisible.Visible) && UserMessages.Confirm(confirmMessage))))
				{
					foreach (var cf in changedFiles.Keys)
					{
						if (!NetworkInterop.FtpUploadFiles(
							null,
							changedFiles[cf].GetUrl(this.FtpRootUrl, true),
							this.FtpUsername,
							this.FtpPassword,
							new string[] { this.GetAbsolutePath(changedFiles[cf]) },
							(err) => UserMessages.ShowErrorMessage(err),
							null,
							textFeedbackHandler,
							progressChangedHandler))
						{
							TextFeedbackEventArgs.RaiseTextFeedbackEvent_Ifnotnull(null, textFeedbackHandler, "Uploading other changed files cancelled, unable to upload changed file: " + changedFiles[cf].GetUrl(this.FtpRootUrl, false), TextFeedbackType.Error);
							return false;
						}
					}
				}
				else if (changedFiles.Count > 0)
				{
					TextFeedbackEventArgs.RaiseTextFeedbackEvent_Ifnotnull(null, textFeedbackHandler, "There were changed files which were not uploaded now", TextFeedbackType.Subtle);
					return true;
				}

				confirmMessage = 
					string.Format("There are {0} added files, continue to upload them to the ftp site?{1}{2}", newFiles.Count, Environment.NewLine,
						string.Join(Environment.NewLine, newFiles.Keys.Count <= cMaxFilesToShowInUsermessages ? newFiles.Keys : newFiles.Keys.Take(cMaxFilesToShowInUsermessages))
						+ (newFiles.Keys.Count <= cMaxFilesToShowInUsermessages ? "" : Environment.NewLine + "..." + Environment.NewLine + "and another " + (newFiles.Keys.Count - cMaxFilesToShowInUsermessages) + " files"));
				if (newFiles.Count > 0 && (!confirmIfChangesBeforeUpload
					|| ((formToCheckIfVisible == null || formToCheckIfVisible.Visible) && UserMessages.Confirm(confirmMessage))))
				{
					foreach (var nf in newFiles.Keys)
					{
						if (!NetworkInterop.FtpUploadFiles(
							null,
							newFiles[nf].GetUrl(this.FtpRootUrl, true),
							this.FtpUsername,
							this.FtpPassword,
							new string[] { this.GetAbsolutePath(newFiles[nf]) },
							(err) => UserMessages.ShowErrorMessage(err),
							null,
							textFeedbackHandler,
							progressChangedHandler))
						{
							TextFeedbackEventArgs.RaiseTextFeedbackEvent_Ifnotnull(null, textFeedbackHandler, "Uploading other new files cancelled, unable to upload new file: " + newFiles[nf].GetUrl(this.FtpRootUrl, false), TextFeedbackType.Error);
							return false;
						}
					}
				}
				else if (newFiles.Count > 0)
				{
					TextFeedbackEventArgs.RaiseTextFeedbackEvent_Ifnotnull(null, textFeedbackHandler, "There were new files which were not uploaded now", TextFeedbackType.Subtle);
					return true;
				}

				confirmMessage =
					string.Format("There are {0} deleted files, continue to delete them from the ftp site?{1}{2}", deletedFiles.Count, Environment.NewLine,
						string.Join(Environment.NewLine, deletedFiles.Keys.Count <= cMaxFilesToShowInUsermessages ? deletedFiles.Keys : deletedFiles.Keys.Take(cMaxFilesToShowInUsermessages))
						+ (deletedFiles.Keys.Count <= cMaxFilesToShowInUsermessages ? "" : Environment.NewLine + "..." + Environment.NewLine + "and another " + (deletedFiles.Keys.Count - cMaxFilesToShowInUsermessages) + " files"));
				if (deletedFiles.Count > 0 && (!confirmIfChangesBeforeUpload
					|| ((formToCheckIfVisible == null || formToCheckIfVisible.Visible) && UserMessages.Confirm(confirmMessage))))
				{
					foreach (var df in deletedFiles.Keys)
					{
						if (!NetworkInterop.DeleteFTPfile(
							null,
							deletedFiles[df].GetUrl(cachedDetails.FtpRootUrl, false),
							cachedDetails.FtpUsername,
							cachedDetails.FtpPassword,
							textFeedbackHandler,
							progressChangedHandler))
						{
							TextFeedbackEventArgs.RaiseTextFeedbackEvent_Ifnotnull(null, textFeedbackHandler, "Deleting other files cancelled, unable to delete file online: " + deletedFiles[df].GetUrl(cachedDetails.FtpRootUrl, false), TextFeedbackType.Error);
							return false;
						}
					}
				}
				else if (deletedFiles.Count > 0)
				{
					TextFeedbackEventArgs.RaiseTextFeedbackEvent_Ifnotnull(null, textFeedbackHandler, "There were deleted files which were not uploaded now", TextFeedbackType.Subtle);
					return true;
				}

				this.SaveDetails();
				return false;//We already uploaded all changes
			}
			else
				TextFeedbackEventArgs.RaiseTextFeedbackEvent_Ifnotnull(null, textFeedbackHandler, "No changes for " + this.LocalRootDirectory + ".", TextFeedbackType.Subtle);

			return hasChanges;
		}

		public static bool AreStringListsEqual(List<string> list1, List<string> list2)
		{
			if (list1 == null && list2 == null)
				return true;
			if (list1 == null || list2 == null)
				return false;

			if (list1.Count != list2.Count)
				return false;

			foreach (string i1 in list1)
				if (!list2.Contains(i1, StringComparer.InvariantCultureIgnoreCase))
					return false;
			return true;
		}
	}

	public class FileDetails
	{
		public string RelativePath;
		public long FileSize;
		public DateTime LastWriteLocal;
		public FileDetails() { }
		public FileDetails(string basePathNoSlashAtEnd, string fullFilePath)
		{
			this.RelativePath = GetRelativePath(basePathNoSlashAtEnd, fullFilePath);//fullFilePath.Substring(basePathNoSlashAtEnd.Length + 1);
			var tmpFI = new FileInfo(fullFilePath);
			this.FileSize = tmpFI.Length;
			this.LastWriteLocal = tmpFI.LastWriteTime;
			this.LastWriteLocal.AddMilliseconds(-this.LastWriteLocal.Millisecond);
		}

		public static string GetRelativePath(string basePath, string fullFilePath)
		{
			return fullFilePath.Substring(basePath.TrimEnd('\\').Length + 1).TrimEnd('\\');
		}

		public string GetUrl(string baseUrl, bool returnOnlyDirAndExcludeFilename = false)
		{
			string tmpstr = baseUrl.TrimEnd('\\', '/') + '/' + this.RelativePath.Replace("\\", "/");
			if (returnOnlyDirAndExcludeFilename)
				return tmpstr.Substring(0, tmpstr.LastIndexOf('/'));
			else
				return tmpstr;
		}
		public override bool Equals(object obj)
		{
			FileDetails otherFileDet = obj as FileDetails;
			if (otherFileDet == null)
				return false;

			return
				this.RelativePath == otherFileDet.RelativePath &&
				this.FileSize == otherFileDet.FileSize &&
				this.LastWriteLocal.Year == otherFileDet.LastWriteLocal.Year &&
				this.LastWriteLocal.Month == otherFileDet.LastWriteLocal.Month &&
				this.LastWriteLocal.Day == otherFileDet.LastWriteLocal.Day &&
				this.LastWriteLocal.Hour == otherFileDet.LastWriteLocal.Hour &&
				this.LastWriteLocal.Minute == otherFileDet.LastWriteLocal.Minute &&
				this.LastWriteLocal.Second == otherFileDet.LastWriteLocal.Second;
		}
		public override int GetHashCode() { return 0; }
	}
}
