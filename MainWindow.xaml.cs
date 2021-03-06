﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace EVE_Industrial_Toolbox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Properties.Settings.Default.Upgrade();
            Properties.Settings.Default.Save();

            EVE_IT_GlobalData.UnserializeSettings();
        }

        private void MenuItem_File_Exit_Click(object sender, RoutedEventArgs e)
        {
            //MessageBoxResult result = MessageBox.Show("Do you really want to close EVE Industrial Toolbox?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            //if (result == MessageBoxResult.Yes)
            //{
                Application.Current.Shutdown();
            //}
        }

        private void MenuItem_File_ManageAPI_Click(object sender, RoutedEventArgs e)
        {
            ManageAPIWindow APIWindow = new ManageAPIWindow();
            APIWindow.ShowDialog();
        }
    }
}
