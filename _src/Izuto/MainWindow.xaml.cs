using Izuto.Extensions;
using Izuto.UI;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Izuto.App;

namespace Izuto
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow? Self;
        public const string appname = "Izuto";
        public static string appDataPath = "";
        public const string appver = "2.0.0.0";
        public const string appdeveloper = "retrohead";
        public const string appfirstyear = "2025";
        public static objectAnimations? objectAnim;
        public class runFunctionsType
        {
            public hexAndMathFunctions hexAndMathFunction = new hexAndMathFunctions();
        }

        #region form fields

        /// <summary>
        /// Class to hold form fields and their properties for data binding in the UI.
        /// </summary>
        public class formFieldsType : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler? PropertyChanged;

            private double _progress_val = 0;
            public double ProgressValue
            {
                get
                {
                    return _progress_val;
                }
                set
                {
                    _progress_val = value;

                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(ProgressValue)));
                }
            }

            private double _progress_queue_val = 0;
            public double ProgressQueueValue
            {
                get
                {
                    return _progress_queue_val;
                }
                set
                {
                    _progress_queue_val = value;

                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(ProgressQueueValue)));
                }
            }

            private string? _copyright;
            public string? Copyright
            {
                get
                {
                    return _copyright;
                }
                set
                {
                    _copyright = value;

                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(Copyright)));
                }
            }

            private string? _status;
            public string? Status
            {
                get
                {
                    return _status;
                }
                set
                {
                    _status = value;

                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(Status)));
                }
            }

            public void OnPropertyChanged(PropertyChangedEventArgs e)
            {
                PropertyChanged?.Invoke(this, e);
            }
        }
        public formFieldsType formFields = new formFieldsType();


        /// <summary>
        /// Update the progress label with a string value.
        /// </summary>
        /// <param name="txt"></param>
        public void updateProgressLabel(string txt)
        {
            txt = txt.Replace(" And ", " and ");
            txt = txt.Replace(" Of ", " of ");
            txt = txt.Replace(" For ", " for ");
            formFields.Status = txt;
            if ((txt != "") & (txt.Trim().ToLower() != "completed"))
            {
                Dispatcher.BeginInvoke(() =>
                {
                    panelSmallProgress.Visibility = Visibility.Visible;
                });
            }
            else if ((txt == "") | (txt.Trim().ToLower() == "completed"))
            {
                Dispatcher.BeginInvoke(() =>
                {
                    panelSmallProgress.Visibility = Visibility.Hidden;
                });
            }
        }

        /// <summary>
        /// Update the main progress bar with a value and maximum value.
        /// </summary>
        /// <param name="val"></param>
        /// <param name="max"></param>
        public void updateProgress(double val, double max)
        {
            double percent = max == 0 ? 0 : (val / max);
            formFields.ProgressValue = percent * 100;
            if ((formFields.ProgressQueueValue == 0) || (formFields.ProgressQueueValue == 100) || (formFields.ProgressQueueValue is double.NaN))
            {
                // hide the queue progress bar
                Dispatcher.BeginInvoke(() =>
                {
                    progressGrid.RowDefinitions[2].Height = new GridLength(0);
                    progressGrid.RowDefinitions[3].Height = new GridLength(56);
                    queueProgress.Visibility = Visibility.Collapsed;
                });
            }
        }

        /// <summary>
        /// Update the queue progress bar with a value and maximum value.
        /// </summary>
        /// <param name="val"></param>
        /// <param name="max"></param>
        public void updateProgressQueue(double val, double max)
        {
            double percent = (val / max);
            formFields.ProgressQueueValue = percent * 100;
            // show the queue progress bar
            Dispatcher.BeginInvoke(() =>
            {
                progressGrid.RowDefinitions[2].Height = new GridLength(28);
                progressGrid.RowDefinitions[3].Height = new GridLength(28);
                queueProgress.Visibility = Visibility.Visible;
            });
        }


        private void panelSmallProgress_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }


        /// <summary>
        /// Check if changes have been made in a pop-up window and confirm if the user wants to lose those changes.
        /// </summary>
        /// <param name="popUp"></param>
        /// <returns></returns>
        public bool canLoseChanges(dynamic popUp)
        {
            if ((popUp.changesMade == true))
            {
                if ((App.CustomMessageBox.Show("Changes made are about to be lost. Are you sure you want to quit without saving?", "Confirm Data Loss", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel))
                {
                    return false;
                }
            }
            popUp.changesMade = false;
            return true;
        }
        #endregion

        #region form
        public MainWindow()
        {
            Self = this;
            appDataPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), appname);
            if (!Directory.Exists(appDataPath))
                Directory.CreateDirectory(appDataPath);

            // initialize form and load
            InitializeComponent();
            Title = appname;

            lblCopy.DataContext = formFields;
            formFields.Copyright = "Application version " + appver + " ©️ " + appdeveloper + " " + appfirstyear + " - " + DateTime.Now.Year;

            lblProgress.DataContext = formFields;
            formFields.Status = "Initialising";

            mainProgress.DataContext = formFields;
            queueProgress.DataContext = formFields;

            if (Properties.Settings.Default.FullSize)
                WindowState = WindowState.Maximized;

            updateProgressQueue(0, 0);
            updateProgress(0, 0);

            // Load progress bar
            lblProgress.DataContext = formFields;
            formFields.Status = "Initialising";

            mainProgress.DataContext = formFields;
            queueProgress.DataContext = formFields;

            Theme.initTheme(this);
            Theme.applyCustomTheme(Properties.Settings.Default.SelectedTheme, Properties.Settings.Default.ThemeColours);
            Theme.applyTheme(this);
        }

        /// <summary>
        /// Event handler for when the main window is loaded. It registers syntax highlighting definitions and initializes the progress panel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            panelSmallProgress.Visibility = Visibility.Hidden;
            DarkTitleBar.Apply(this);
        }

        /// <summary>
        /// Event handler for when the main window is closing. It checks if there are any active pop-up windows and prompts the user to close them before closing the application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (popUps.IsOpen())
            {
                App.CustomMessageBox.Show("Please close the active pop-up windows before closing the application", "Active Pop-Ups", MessageBoxButton.OK, MessageBoxImage.Information);
                e.Cancel = true;
                return;
            }

            if (!UI_MainWindow.CanLoseChanges())
            {
                e.Cancel = true;
                return;
            }
        }

        /// <summary>
        /// Event handler for when the main window state changes (e.g., maximized or minimized). It saves the full size state to application settings.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                Properties.Settings.Default.FullSize = true;
                Properties.Settings.Default.Save();
            }
            else if (WindowState == WindowState.Normal)
            {
                Properties.Settings.Default.FullSize = false;
                Properties.Settings.Default.Save();
            }
        }

        /// <summary>
        /// Get the value of a resource from the specified object and resource name.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string getResourceVal(dynamic obj, string name)
        {
            ResourceDictionary res = obj.Resources;
            return res[name].ToString() ?? "";
        }


        /// <summary>
        /// Overwrite a resource value in the specified object with a new value.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void overwriteResource(dynamic obj, string name, string value)
        {
            ResourceDictionary res = obj.Resources;
            res[name] = ColorConverter.ConvertFromString(value);
        }

        /// <summary>
        /// Load a theme from a specified file and merge it into the application's resources.
        /// </summary>
        /// <param name="themeFile"></param>
        public static void loadTheme(string themeFile)
        {
            Uri uri = new Uri(@"/Izuto;component/Resources/Themes/" + themeFile, UriKind.Relative);
            ResourceDictionary rs = (ResourceDictionary)System.Windows.Application.LoadComponent(uri);
            System.Windows.Application.Current.Resources.MergedDictionaries.Add(rs);
        }

        /// <summary>
        /// Disable the form by setting the visibility of the enabled panel to visible and disabling the main form.
        /// </summary>
        public void disableForm()
        {
            enableForm(false);
        }

        /// <summary>
        /// Enable the pop-up panel by setting its visibility to hidden or visible based on the enabled parameter.
        /// </summary>
        /// <param name="enabled"></param>
        public void enablePopUp(bool enabled)
        {
            if ((enabled))
            {
                Dispatcher.BeginInvoke(() =>
                {
                    panelPopUpEnabled.Visibility = Visibility.Hidden;
                });
            }
            else
                Dispatcher.BeginInvoke(() =>
                {
                    panelPopUpEnabled.Visibility = Visibility.Visible;
                });
        }

        /// <summary>
        /// Enable or disable the main form based on the enabled parameter. If the pop-up is not open, it enables the pop-up panel.
        /// </summary>
        /// <param name="enabled"></param>
        public void enableForm(bool enabled)
        {
            Dispatcher.BeginInvoke(() =>
            {
                if ((popUps.IsOpen() == false))
                    enablePopUp(true);
                if ((enabled))
                {
                    panelEnabled.Visibility = Visibility.Hidden;
                    enableMainForm();
                }
                else
                {
                    panelEnabled.Visibility = Visibility.Visible;
                    disableMainForm();
                }
            });
        }


        public void disableMainForm()
        {
            if (popUps.Exist())
                return;
            mainWindow.IsEnabled = false;
        }
        public void enableMainForm()
        {
            if (popUps.Exist())
                return;
            mainWindow.IsEnabled = true;
        }

        public delegate void setObjectVisibilityDelegate(dynamic o, Visibility vis);
        public delegate void setObjectOpacityDelegate(dynamic o, double opacity);
        public static void setObjectOpacity(dynamic o, double opacity)
        {

            MainWindow.Self?.Dispatcher.Invoke(() =>
            {
                o.Opacity = opacity;
            });
        }
        public static void setObjectVisibility(dynamic o, Visibility vis)
        {
            MainWindow.Self?.Dispatcher.Invoke(() =>
            {
                o.Visibility = vis;
            });
        }
        private void lst_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            //Do nothing for the moment, event Is added during listviewdata load
        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            popUps.resize(e.NewSize.Height);
            mainWindow.resize(sender, e);
        }
        public static T? FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child is T tChild)
                {
                    return tChild;
                }

                T? childOfChild = FindVisualChild<T>(child);
                if (childOfChild != null)
                {
                    return childOfChild;
                }
            }
            return null;
        }

        public static void UpdateTitle(string extratitle)
        {
            if (Self == null)
                return;
            if (extratitle == "")
                Self.Title = appname;
            else
                Self.Title = $"{appname} - {extratitle}";
        }
        #endregion


        public static string BytesToHexString(byte[]? bytes, string spacing = "", int? bytesPerRow = null)
        {
            if (bytes == null || bytes.Count() == 0)
                return "";
            int len = bytes.Count();

            // Estimate capacity: 2 hex chars per byte + spacing + possible newline
            int estimated = len * (2 + spacing.Length + 1);
            var sb = new StringBuilder(estimated);

            int rowCount = 0;
            for (int i = 0; i < len; i++)
            {
                // Add space between bytes (except at start of line)
                if (rowCount > 0)
                    sb.Append(spacing);

                sb.Append(bytes[i].ToString("X2"));

                if (!string.IsNullOrEmpty(spacing))
                    sb.Append(spacing);

                rowCount++;

                if (bytesPerRow.HasValue && rowCount == bytesPerRow.Value)
                {
                    sb.AppendLine();
                    rowCount = 0;
                }
            }

            return sb.ToString();
        }

    }
}