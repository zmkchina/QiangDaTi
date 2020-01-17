using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace QiangDaTi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public VMWinMain _VMWinMain = new VMWinMain();
        private string _CurTiMuStr = "";
        private string _CurAnswerStr = "";
        private char[] _CurTiMuCharArr = null;
        private int _CurTiMuCharIndex = 0;
        private bool _IsRuning = false;
        private WinSetting _WinSetting = null;
        public DispatcherTimer _TimerChar = null;
        public DispatcherTimer _TimerDaoJishi = null;

        public MainWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 题目定时显示事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTimerCharEvent(object sender, EventArgs e)
        {
            _CurTiMuCharIndex++;
            if (_CurTiMuCharIndex < _CurTiMuCharArr.Length)
            {
                this.Question_Text.Text += _CurTiMuCharArr[_CurTiMuCharIndex].ToString();
            }
            else
            {
                _TimerChar.Stop();
                _IsRuning = false;
                this.btn_pause.IsEnabled = false;
                _VMWinMain.DaoJishiShengYu = _VMWinMain.DaoJiShiCount;
                this.TB_BeginQiangDa.Visibility = Visibility.Collapsed;
                this.TB_DaoJiShi.Visibility = Visibility.Visible;
                this._TimerDaoJishi.Start();
            }
        }
        /// <summary>
        /// 到计时定时器事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTimerDaoJishiEvent(object sender, EventArgs e)
        {
            _VMWinMain.DaoJishiShengYu--;
            if (_VMWinMain.DaoJishiShengYu == 0)
            {
                _TimerDaoJishi.Stop();
                this.TB_DaoJiShi.Visibility = Visibility.Collapsed;
                this.TB_BeginQiangDa.Text = "时间到！";
                this.TB_BeginQiangDa.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// 创建试题编号
        /// </summary>
        /// <param name="p_count"></param>
        public void CreateButton(int p_count)
        {
            this.WP_Buttons.Children.Clear();
            for (int i = 1; i <= p_count; i++)
            {
                Button V_Btn = new Button();
                V_Btn.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 0, 0));
                V_Btn.Foreground = Brushes.White;
                V_Btn.Tag = i;
                V_Btn.Content = i.ToString();
                V_Btn.Margin = new Thickness(10);
                V_Btn.HorizontalAlignment = HorizontalAlignment.Center;
                V_Btn.VerticalAlignment = VerticalAlignment.Center;

                Binding V_BindingFontSize = new Binding("BianHaoFontSize");
                V_Btn.SetBinding(Button.FontSizeProperty, V_BindingFontSize);
                Binding V_BindingWidth = new Binding("BtnWidthHeight");
                V_Btn.SetBinding(Button.WidthProperty, V_BindingWidth);
                Binding V_BindingHeight = new Binding("BtnWidthHeight");
                V_Btn.SetBinding(Button.HeightProperty, V_BindingHeight);

                V_Btn.Click += V_Btn_Click;
                this.WP_Buttons.Children.Add(V_Btn);
            }
        }
        /// <summary>
        /// 题目序号被点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void V_Btn_Click(object sender, RoutedEventArgs e)
        {
            Button V_Btn = (Button)sender;
            int V_Index = int.Parse(V_Btn.Tag.ToString());
            if ((V_Index - 1) > this._VMWinMain.QuestionTable.Rows.Count)
            {
                MessageBox.Show("无此题号题目，题目数量太少！", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            _CurTiMuStr = this._VMWinMain.QuestionTable.Rows[V_Index - 1][1].ToString();
            _CurAnswerStr = "参考答案 : " + this._VMWinMain.QuestionTable.Rows[V_Index - 1][2].ToString();
            _CurTiMuCharArr = _CurTiMuStr.ToArray();
            this.WP_Buttons.Visibility = Visibility.Collapsed;
            this.Question_Text.Visibility = Visibility.Visible;
            this.btn_pause.Visibility = Visibility.Visible;
            this.btn_Show.Visibility = Visibility.Visible;
            this.btn_Reurn.Visibility = Visibility.Visible;
            this.btn_pause.IsEnabled = true;
            this.btn_Show.IsEnabled = true;
            this._TimerChar.Start();
            this._IsRuning = true;
            this._CurTiMuCharIndex = 0;
            this.Question_Text.Text = _CurTiMuCharArr[_CurTiMuCharIndex].ToString();
            this.btn_pause.Content = "暂停";
            V_Btn.Background = Brushes.RosyBrown;
            V_Btn.Click -= V_Btn_Click;
            V_Btn.Content = "已用";
            V_Btn.FontSize = V_Btn.Width / 4;
            this.TB_BeginQiangDa.Text = "可以抢答...";
            this.TB_BeginQiangDa.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 暂停或继续显示试题
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_pause_Click(object sender, RoutedEventArgs e)
        {
            if (_IsRuning)
            {
                this._TimerChar.Stop();
                this._IsRuning = false;
                this.btn_pause.Content = "继续";

                this.TB_BeginQiangDa.Visibility = Visibility.Collapsed;
                this._VMWinMain.DaoJishiShengYu = _VMWinMain.DaoJiShiCount;
                this.TB_DaoJiShi.Visibility = Visibility.Visible;
                this._TimerDaoJishi.Start();
            }
            else
            {
                this.TB_BeginQiangDa.Text = "可以抢答...";
                this.TB_BeginQiangDa.Visibility = Visibility.Visible;

                this.TB_DaoJiShi.Visibility = Visibility.Collapsed;
                _VMWinMain.DaoJishiShengYu = _VMWinMain.DaoJiShiCount;
                this._TimerChar.Start();
                this._IsRuning = true;
                this.btn_pause.Content = "暂停";
                this._TimerDaoJishi.Stop();
            }
        }
        /// <summary>
        /// 显示设置窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Set_Click(object sender, RoutedEventArgs e)
        {
            if (this._WinSetting == null) this._WinSetting = new WinSetting(this);
            this._WinSetting.ShowActivated = true;
            this._WinSetting.Show();
        }
        /// <summary>
        /// 返回列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Reurn_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this._TimerDaoJishi.Stop();
            this._TimerChar.Stop();
            this._IsRuning = false;
            this.WP_Buttons.Visibility = Visibility.Visible;
            this.Question_Text.Visibility = Visibility.Collapsed;
            this.btn_pause.Visibility = Visibility.Collapsed;
            this.btn_Show.Visibility = Visibility.Collapsed;
            this.btn_Reurn.Visibility = Visibility.Collapsed;
            this.TB_DaoJiShi.Visibility = Visibility.Collapsed;
            this.TB_BeginQiangDa.Visibility = Visibility.Collapsed;

        }
        /// <summary>
        /// 显示整题
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Show_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this._TimerDaoJishi.Stop();
            this.TB_DaoJiShi.Visibility = Visibility.Collapsed;
            this.TB_BeginQiangDa.Visibility = Visibility.Visible;
            this.TB_BeginQiangDa.Text = _CurAnswerStr;

            this._TimerChar.Stop();
            this._IsRuning = false;
            this.Question_Text.Text = _CurTiMuStr;
            this.btn_pause.IsEnabled = false;
            this.btn_Show.IsEnabled = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ImageBrush b = new ImageBrush();
            b.ImageSource = new BitmapImage(new Uri("pack://application:,,,/AppRes/bkimg.jpg"));
            b.Stretch = Stretch.Fill;
            this.Background = b;

            _TimerChar = new DispatcherTimer();
            _TimerChar.Tick += new EventHandler(OnTimerCharEvent);
            _TimerChar.Interval = new TimeSpan(0, 0, 0, 0, 500);
            _TimerDaoJishi = new DispatcherTimer();
            _TimerDaoJishi.Tick += new EventHandler(OnTimerDaoJishiEvent);
            _TimerDaoJishi.Interval = new TimeSpan(0, 0, 0, 1);

            _VMWinMain.BtnCount = this._VMWinMain.QuestionTable.Rows.Count;
            _VMWinMain.BtnCount = _VMWinMain.BtnCount > 20 ? 20 : _VMWinMain.BtnCount;
            CreateButton(_VMWinMain.BtnCount);

            this.Question_Text.Visibility = Visibility.Collapsed;
            this.btn_pause.Visibility = Visibility.Collapsed;
            this.btn_Show.Visibility = Visibility.Collapsed;
            this.btn_Reurn.Visibility = Visibility.Collapsed;
            this.TB_DaoJiShi.Visibility = Visibility.Collapsed;
            this.TB_BeginQiangDa.Visibility = Visibility.Collapsed;

            this.WP_Buttons.Visibility = Visibility.Visible;
            this.DataContext = _VMWinMain;

            this.MouseDown += (x, y) =>
            {
                if (y.LeftButton == MouseButtonState.Pressed)
                {
                    this.DragMove();
                }
            };
        }

        private void btn_Max_Click(object sender, RoutedEventArgs e)
        {
            Button V_Btn = (System.Windows.Controls.Button)sender;
            if (V_Btn.Content.Equals("恢复"))
            {
                this.WindowState = WindowState.Normal;
                V_Btn.Content = "最大";
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                V_Btn.Content = "恢复";
            }
        }
        private void btn_Min_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btn_Colse_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("确认要关闭程序吗？", "确认", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _TimerDaoJishi.Stop();
                _TimerChar.Stop();
                this.Close();
                Application.Current.Shutdown();
            }
        }
    }
}
