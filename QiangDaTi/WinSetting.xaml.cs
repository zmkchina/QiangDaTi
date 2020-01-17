using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QiangDaTi
{
    /// <summary>
    /// WinSetting.xaml 的交互逻辑
    /// </summary>
    public partial class WinSetting : Window
    {
        private MainWindow _MainWindow = null;
        private bool _CanClose = false;
        public WinSetting(MainWindow P_MainWindow)
        {
            InitializeComponent();
            _MainWindow = P_MainWindow;
            this.DataContext = _MainWindow._VMWinMain;
        }


        private void Btn_Count_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(this.Tb_Count.Text, out int V_BtnCount))
            {
                if (V_BtnCount > _MainWindow._VMWinMain.QuestionTable.Rows.Count)
                {
                    MessageBox.Show("试题数量仅有" + _MainWindow._VMWinMain.QuestionTable.Rows.Count + "条", "数量不足", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (V_BtnCount > 0 && V_BtnCount < 200)
                {
                    _MainWindow.CreateButton(_MainWindow._VMWinMain.BtnCount);
                }
            }
        }

       
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (!_CanClose)
            {
                e.Cancel = true;  // cancels the window close    
                this.Hide();      // Programmatically hides the window
            }
        }
        
    }
}
