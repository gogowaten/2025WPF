using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pixtack4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RootThumb MyRoot { get; set; } = null!;
        private ManageExCanvas MyManageExCanvas { get; set; } = null!;


        //アプリ名
        private const string APP_NAME = "Pixtack4";
        //アプリのバージョン
        private string MyAppVersion = null!;
        //アプリのフォルダパス
        private string MyAppDirectory = null!;

        public MainWindow()
        {
            InitializeComponent();
            MyInitialize();
            MyInitialize2();

        }

        #region 初期処理
        
        private void MyInitialize()
        {
            MyAppDirectory = Environment.CurrentDirectory;
            MyAppVersion = GetAppVersion();
        }
        private void MyInitialize2()
        {
            this.Title = APP_NAME + "_" + MyAppVersion;
        }

        /// <summary>
        /// アプリのバージョン取得
        /// </summary>
        /// <returns></returns>
        private static string GetAppVersion()
        {
            //実行ファイルのバージョン取得
            string[] cl = Environment.GetCommandLineArgs();

            //System.Diagnostics.FileVersionInfo
            if (FileVersionInfo.GetVersionInfo(cl[0]).FileVersion is string ver)
            {
                return ver;
            }
            else { return string.Empty; }
        }

        ///// <summary>
        ///// ウィンドウ位置設定が画面外だった場合は0にする
        ///// </summary>
        ///// <param name="config"></param>
        ///// <returns></returns>
        //private void FixWindowLocate()
        //{
        //    if (MyAppData.AppLeft < -10 ||
        //        MyAppData.AppLeft > SystemParameters.VirtualScreenWidth - 100)
        //    {
        //        MyAppData.AppLeft = 0;
        //    }
        //    if (MyAppData.AppTop < -10 ||
        //        MyAppData.AppTop > SystemParameters.VirtualScreenHeight - 100)
        //    {
        //        MyAppData.AppTop = 0;
        //    }
        //}

        #endregion 初期処理
    }
}