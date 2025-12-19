using Izuto;
using Izuto.Controls;
using Izuto.DockManager;
using Izuto.Extensions;
using Microsoft.Win32;
using plugin_level5.N3DS.Archive;
using SevenZip.Compression.LZ;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Izuto.UI
{
    /// <summary>
    /// Interaction logic for UI_MainWindow.xaml
    /// </summary>
    public partial class UI_MainWindow : UserControl
    {
        public static dynamic? loadedTab = null;
        public int RecentItem_Clicked { get; private set; }

        public static UI_MainWindow? self;
        public static string CurrentWorkingDirectory = "";
        public static OptionsFileData OptionsFile = new OptionsFileData();
        private static string ApplicationTempPath = Path.Combine(Path.GetTempPath(), "Izuto");
        public static UI_MainWindow? Self;
        public static BitmapImage? icon_zip = appImages.getImageFromResources("file_zip.png");
        public static BitmapImage? icon_unknown = appImages.getImageFromResources("file_unk.png");
        public static BitmapImage? icon_text = appImages.getImageFromResources("file_txt.png");

        public enum iconTypes
        {
            Unknown,
            Txt,
            Zip
        }
        public static void DeleteDirWithoutWarning(string path)
        {
            try
            {
                if (Directory.Exists(path))
                    Directory.Delete(path, true);
            }
            catch { }

        }

        public static bool IsAnotherInstanceRunning()
        {
            string currentProcessName = Process.GetCurrentProcess().ProcessName;
            int count = Process.GetProcessesByName(currentProcessName).Length;
            return count > 1; // More than one means another instance is running
        }

        public static void DeleteTempDirs()
        {
            // delete this applications temp folder
            DeleteTempDir();
            if (IsAnotherInstanceRunning())
                return;
            // delete all temp folders as this is the only instance of the app running
            var dirs = Directory.GetDirectories(ApplicationTempPath);
            foreach (var dir in dirs)
                DeleteDirWithoutWarning(dir);
        }

        public static void DeleteTempDir()
        {
            DeleteDirWithoutWarning(CurrentWorkingDirectory);
            CurrentWorkingDirectory = "";
        }
        public static string CreateNewTempDirectory(bool SetAsWorkingDirectory)
        {
            string newTempDir = Path.Combine(ApplicationTempPath, "temp_" + DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            int appendInt = 2;
            string adjustedTempDir = newTempDir;
            while (Directory.Exists(adjustedTempDir))
            {
                adjustedTempDir = $"{newTempDir} ({appendInt})";
                appendInt++;
            }
            newTempDir = adjustedTempDir;
            Directory.CreateDirectory(newTempDir);
            if (SetAsWorkingDirectory)
            {
                DeleteTempDir();
                CurrentWorkingDirectory = newTempDir;
            }
            return newTempDir;
        }

        public UI_MainWindow()
        {
            Self = this;
            InitializeComponent();
            if (!Directory.Exists(MainWindow.appDataPath))
                Directory.CreateDirectory(MainWindow.appDataPath);
            string themePath = System.IO.Path.Combine(MainWindow.appDataPath, "Themes");
            if (!Directory.Exists(themePath))
                Directory.CreateDirectory(themePath);
        }

        public void resize(object sender, SizeChangedEventArgs e)
        {
            if (loadedTab != null)
                loadedTab.Content.resize(e.NewSize.Height, e.NewSize.Width);
        }

        public static bool CanLoseChanges()
        {
            return true;
        }


        /// <summary>
        /// Opens a file browser dialog with a custom filter and title.
        /// </summary>
        /// <param name="filter">File filter string (e.g. "CIA files (*.cia)|*.cia").</param>
        /// <param name="title">Dialog title (e.g. "Select a CIA file").</param>
        /// <returns>Full path of the selected file, or empty string if cancelled.</returns>
        public static string BrowseForFile(string filter = "All files (*.*)|*.*", string title = "Select a file")
        {
            string filePath = string.Empty;

            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = filter,
                Title = title,
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog(MainWindow.Self.Window) == true)
            {
                filePath = openFileDialog.FileName;
            }
            return filePath;
        }
        /// <summary>
        /// Opens a folder browser dialog with a custom description.
        /// </summary>
        /// <param name="description">Text shown in the dialog (e.g. "Select output directory").</param>
        /// <returns>Full path of the selected directory, or empty string if cancelled.</returns>
        public static string BrowseForDirectory(string description = "Select a folder")
        {
            string folderPath = string.Empty;

            OpenFolderDialog folderDialog = new OpenFolderDialog()
            {
                Title = description,
            };

            if (folderDialog.ShowDialog(MainWindow.Self.Window) == true)
            {
                folderPath = folderDialog.FolderName;
            }

            return folderPath;
        }

        private async void MenuItem_Open_Click(object sender, RoutedEventArgs e)
        {
            await uiArchiveFA.Open_Click();
        }

        private void MenuItem_Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }


        public void UpdateProgress(string text, int value, int maxValue)
        { 
            MainWindow.Self.updateProgressLabel(text); 
            MainWindow.Self.updateProgress(value, maxValue);
            MainWindow.Self.enableForm(false);
        }
        public static void CustomWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Theme.initTheme((Window)sender!);
            Theme.applyCustomTheme(SettingsManager.Settings.SelectedTheme, SettingsManager.Settings.ThemeColours);
            DockHandler.ApplyThemeColorsToOpenWindows(Theme.getThemeColorsFromWindowResources(MainWindow.Self!));
        }

        public void EndProgressUpdates()
        {
            MainWindow.Self.updateProgressLabel(""); // hide the progress bar
            MainWindow.Self.enableForm(true);
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(SettingsManager.Settings.OptionsFilePath))
            {
                if (!File.Exists(SettingsManager.Settings.OptionsFilePath))
                {
                    MessageBox.Show("Failed to load options, the file no longer exists\n\n:" + SettingsManager.Settings.OptionsFilePath, "Options File Error", MessageBoxButton.OK,  MessageBoxImage.Warning);
                    SettingsManager.Settings.OptionsFilePath = "";
                    SettingsManager.Save();
                }
                else
                {
                    if (!OptionsFile.Load(SettingsManager.Settings.OptionsFilePath))
                    {
                        MessageBox.Show("The options file appears to be corrupted\n\n:" + SettingsManager.Settings.OptionsFilePath, "Options File Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        SettingsManager.Settings.OptionsFilePath = "";
                        SettingsManager.Save();
                    }
                }
            }
        }

        private void AddNoRecentItemsMenu(System.Windows.Style? style)
        {
            // add a blank item showing no recent files
            menuRecent.Items.Add(new System.Windows.Controls.MenuItem()
            {
                Header = "No Recent Archives",
                IsEnabled = false,
                Icon = null,
                Style = style,
                FontSize = 12
            });
        }
        private async void RecentItem_Click(object sender, RoutedEventArgs e)
        {
            if (!CanLoseChanges())
                return;
            System.Windows.Controls.MenuItem item = (System.Windows.Controls.MenuItem)sender;
            RecentFilesManager.RecentFile data = (RecentFilesManager.RecentFile)item.Tag;
            if (data == null)
                return;

            if (!File.Exists(data.FilePath))
            {
                if (MessageBox.Show("The selected file no longer exists.\n\n" +
                    data.FilePath + "\n\n" +
                    "Would you like to remove it from your recent files list?", "File no longer exists", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                {
                    RecentFilesManager.RemoveRecentFile(data.FilePath);
                }
                return;
            }

            if (!File.Exists(data.FilePath))
            {
                if (MessageBox.Show("The file no longer exists, do you want to remove it from your recent files list?", "Missing File", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    RecentFilesManager.RemoveRecentFile(data.FilePath);
                return;
            }
            if (data.FilePath.ToLower().EndsWith(".pkh"))
            {
                uiArchiveFA.OpenPKHFile(data.FilePath);
                return;
            }
            UI_ArchiveFA.LoadedArchiveFilePath = data.FilePath;
            uiArchiveFA.textArchiveFaPath.Text = UI_ArchiveFA.LoadedArchiveFilePath;
            await uiArchiveFA.ListFiles();
        }
        private void upperMenu_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource.GetType() != typeof(System.Windows.Controls.MenuItem))
                return;
            System.Windows.Controls.MenuItem itemclicked = (System.Windows.Controls.MenuItem)e.OriginalSource;
            if (nameof(upperFileMenu) == itemclicked.Name)
            {
                System.Windows.Style? style = (System.Windows.Style?)MainWindow.Self?.FindResource("MenuItemStyle");
                menuRecent.Items.Clear();
                if (RecentFilesManager.RecentFiles != null)
                {
                    foreach (RecentFilesManager.RecentFile file in RecentFilesManager.RecentFiles)
                    {
                        var tooltip = new ToolTip();
                        tooltip.Background = (SolidColorBrush?)MainWindow.Self?.FindResource("WindowBackgroundBrushMedium");
                        tooltip.Foreground = (SolidColorBrush?)MainWindow.Self?.FindResource("ControlTextInactive");
                        tooltip.Content = file.FilePath;
                        var item = new System.Windows.Controls.MenuItem()
                        {
                            Header = ShortenPath(file.FilePath),
                            Tag = file,
                            Style = style,
                            FontSize = 11,
                            ToolTip = tooltip
                        };
                        item.Click += RecentItem_Click;

                        menuRecent.Items.Add(item);

                    }
                }
                if (menuRecent.Items.Count == 0)
                    AddNoRecentItemsMenu(style);

                upperFileMenu.UpdateLayout();
                menuRecent.UpdateLayout();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
        public static string ShortenPath(string path, int maxLength = 50)
        {
            if (string.IsNullOrEmpty(path) || path.Length <= maxLength)
                return path;

            // Split into directory segments
            string[] parts = path.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            // Always keep first and last segment
            string first = parts[0];
            string last = parts[^1];

            // Build middle segments until we exceed maxLength
            var middle = new List<string>();
            int totalLength = first.Length + last.Length + 5; // 5 for "...\"
            for (int i = 1; i < parts.Length - 1; i++)
            {
                int nextLen = parts[i].Length + 1; // +1 for separator
                if (totalLength + nextLen > maxLength)
                {
                    middle.Add("...");
                    break;
                }
                middle.Add(parts[i]);
                totalLength += nextLen;
            }

            return string.Join(Path.DirectorySeparatorChar.ToString(),
                new[] { first }.Concat(middle).Concat(new[] { last }));
        }

        private void MenutItem_Settings_Click(object sender, RoutedEventArgs e)
        {
            CustomWindow win = DockHandler.CreateCustomWindow(MainWindow.Self.Window, new CustomWindowOptions() { WindowType = CustomWindow.WindowTypes.Resizable });
            win.ApplyContent(new OptionsWindow());
            win.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            win.Loaded += CustomWindow_Loaded;
            win.ShowDialog();
        }

        private void MenutItem_Theme_Click(object sender, RoutedEventArgs e)
        {
            dynamic pop = new popUpOptions(MainWindow.Self, this);
            popUps.loadPopUp(MainWindow.Self, "Theme Settings", "theme.png", ref pop, true);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            if (!string.IsNullOrEmpty(SettingsManager.Settings.OptionsFilePath))
            {
                if (!File.Exists(SettingsManager.Settings.OptionsFilePath))
                {
                    MessageBox.Show("Failed to load options, the file no longer exists\n\n:" + SettingsManager.Settings.OptionsFilePath, "Options File Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    SettingsManager.Settings.OptionsFilePath = "";
                    SettingsManager.Save();
                }
                else
                {
                    if (!OptionsFile.Load(SettingsManager.Settings.OptionsFilePath))
                    {
                        MessageBox.Show("The options file appears to be corrupted\n\n:" + SettingsManager.Settings.OptionsFilePath, "Options File Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        SettingsManager.Settings.OptionsFilePath = "";
                        SettingsManager.Save();
                    }
                }
            }
        }
    }
}
