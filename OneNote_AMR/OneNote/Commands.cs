using MahApps.Metro.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LikePaint
{

    public partial class MainWindow : MetroWindow
    {
        private void cmd_Save(object sender, ExecutedRoutedEventArgs e)
        {

            SaveFileDialog save = new SaveFileDialog();
            save.CheckPathExists = true;
            save.ValidateNames = true;
            save.AddExtension = true;
            save.OverwritePrompt = true;
            save.DefaultExt = ".jpg";
            save.Filter = "JPeg Image|*.jpg";

            if (save.ShowDialog().Value)
            {
                this.Save(save.FileName);
            }
        }

        private void cmd_Delete(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to delete everything?", "Delete Confirmation", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                Delete();
            }
            else
                return;
        }
    }

    public class Commands
    {
        public static RoutedCommand Save { get; set; }
        public static RoutedCommand Delete { get; set; }

        static Commands()
        {
            Commands.Save = new RoutedCommand("Save", typeof(Commands));
            Commands.Delete = new RoutedCommand("Delete", typeof(Commands));
        }
    }
}
