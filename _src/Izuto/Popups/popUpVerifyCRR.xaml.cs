using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Izuto
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class popUpVerifyCRR : UserControl
    {
        public popUps.popUpType? popUpObj;
        private MainWindow? mainWindow;
        ActionResult MainResult;
        public bool changesMade = false;

        public enum refresh_objects
        {
            none
        }

        public popUpVerifyCRR(MainWindow? mainWin, ActionResult mainResult)
        {
            MainResult = mainResult;
            // This call is required by the designer.
            InitializeComponent();
            Name = "popUpVerifyCRR";
            mainWindow = mainWin;

            // Add any initialization after the InitializeComponent() call.
            mainWindow = mainWin;

        }
        public void resize(double newsize)
        {
        }
        public void resize(double newheight, double newwidth)
        {
        }
        public void refresh(refresh_objects obj)
        {
           App.CustomMessageBox.Show("Refresh not implemented", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        public void load()
        {
            if (popUpObj == null)
                return;
            popUpObj.cancelFunctionDelegate = cancel;

            StringBuilder sb = new StringBuilder();
            foreach(var result in CROTools.CROTools.CROResults)
            {
                sb.AppendLine($"[{(result.Success ? "Success" : "Failed")}] {result.Message}");
            }
            txtResults.Text = sb.ToString();

            popUpObj.appear();
        }
        private void fieldWasChanged(object o)
        {
        }
        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            if (popUpObj == null)
                return;
            // close the popup
            popUpObj.closePopUp(fadeCompleted, null);
        }


        private void cancel()
        {
            if (mainWindow?.canLoseChanges(this) == false)
                return;
            popUpObj?.closePopUp(fadeCompletedCancel, null);
        }


        public void fadeCompletedCancel()
        {
        }

        public void fadeCompleted()
        {
        }

        public delegate void completedFunctionDelegate(string result);
    }
}
