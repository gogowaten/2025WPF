using Microsoft.Win32;
using System.Diagnostics;
using System.Runtime.Serialization;
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
using System.Xml;

namespace Pixtack4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public RootThumb MyRoot { get;private set; } = null!;// 確認用でパブリックにしている
        private ManageExCanvas MyManageExCanvas { get; set; } = null!;

        //今開いているファイルパスを保持
        //private string CurrentOpenFilePath = string.Empty;

        //
        //private string ROOT_DATA_FILE_NAME = "RootData.px4";
        //RootのDataの拡張子はpx4
        //それ以外のDataの拡張子はpx4item

        //アプリ名
        private const string APP_NAME = "Pixtack4";
        //アプリのバージョン
        private string MyAppVersion = null!;
        //アプリのフォルダパス
        private string MyAppDirectory = null!;

        //アプリのウィンドウData
        private AppWindowData MyAppWindowData { get; set; } = null!;
        //アプリのウィンドウDataファイル名
        private const string APP_WINDOW_DATA_FILE_NAME = "AppWindowData.xml";

        //アプリの設定Data
        public AppData MyAppData { get;private set; } = null!;// 確認用でパブリックにしている
        //アプリのDataファイル名
        private const string APP_DATA_FILE_NAME = "AppData.xml";


        //datetime.tostringの書式、これを既定値にする
        private const string DATE_TIME_STRING_FORMAT = "yyyMMdd'_'HHmmss'.'fff";

        public MainWindow()
        {
            InitializeComponent();
            MyInitialize();
            MyInitialize2();
            Closing += MainWindow_Closing;
            DataContext = this;
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {

            AppClosing(e);// アプリ終了直前の処理
        }


        #region アプリ終了時の処理
        /// <summary>
        /// アプリ終了直前の処理
        /// </summary>
        private void AppClosing(System.ComponentModel.CancelEventArgs e)
        {
            //ダイアログ表示、保存するかを聞く
            if (MyRoot.MyThumbs.Count > 0)
            {
                MessageBoxResult result = MessageBox.Show(
                    "ファイルに保存してから終了する？\n\n\n" +
                    "はい：保存して終了\n\n" +
                    "いいえ：保存しないで終了\n\n" +
                    "キャンセル：終了をキャンセル",
                    "アプリ終了前の確認",
                    MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.Cancel);

                if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;// 終了をキャンセル
                }
                else if (result == MessageBoxResult.Yes)
                {
                    OverwriteSaveData();// 上書き保存
                }
            }


            string filePath = System.IO.Path.Combine(MyAppDirectory, APP_WINDOW_DATA_FILE_NAME);
            if (!MyAppWindowData.Serialize(filePath))
            {
                MessageBox.Show("アプリのWindow設定を保存できなかった");
            }

            filePath = MyAppData.CurrentOpenFilePath;
            if (filePath == string.Empty)
            {
            }
            filePath = System.IO.Path.Combine(MyAppDirectory, APP_DATA_FILE_NAME);
            if (!MyAppData.Serialize(filePath, MyAppData))
            {
                MessageBox.Show("アプリの設定を保存できなかった");
            }
        }
        #endregion アプリ終了前の処理

        #region 初期処理

        private void MyInitialize()
        {
            //アプリのパスとバージョン取得
            MyAppDirectory = Environment.CurrentDirectory;
            MyAppVersion = GetAppVersion();

            //アプリの設定の読み込みと設定
            if (LoadAppData() is AppData appData) { MyAppData = appData; }
            else { MyAppData = new AppData(); }






        }


        private void MyInitialize2()
        {
            this.Title = APP_NAME + "_" + MyAppVersion;
            LoadAppWindowData();// ウィンドウ設定ファイルの読み込み
            MyBindWindowData();// ウィンドウのバインド設定

            // RootThumbとManageExCanvasの初期化
            MyInitializeRootThumb();

            //前回に開いていたファイルを開く
            OpenPx4FileRootThumb(MyAppData.CurrentOpenFilePath);

            MyBind();

            //初回起動時はRootThumbを新規作成

        }


        private void MyBind()
        {
            //今開いているファイルのフルパスからファイル名だけを表示
            MyStatusCurrentFileName.SetBinding(TextBlock.TextProperty, new Binding(nameof(MyAppData.CurrentOpenFilePath)) { Source = MyAppData, Converter = new MyConvPathFileName() });
        }


        #region アプリのウィンドウ設定

        /// <summary>
        /// ウィンドウ設定ファイルの読み込み
        /// </summary>
        private void LoadAppWindowData()
        {
            //アプリのフォルダから設定ファイルを読み込む、ファイルがなかったら新規作成
            string filePath = System.IO.Path.Combine(MyAppDirectory, APP_WINDOW_DATA_FILE_NAME);
            if (AppWindowData.Deserialize(filePath) is AppWindowData data)
            {
                MyAppWindowData = data;
            }
            else
            {
                MyAppWindowData = new AppWindowData();
            }
        }

        /// <summary>
        /// ウィンドウのバインド設定
        /// </summary>
        private void MyBindWindowData()
        {
            //バインド設定、ウィンドウの最大化も？
            SetBinding(LeftProperty, new Binding(nameof(AppWindowData.Left)) { Source = MyAppWindowData, Mode = BindingMode.TwoWay });
            SetBinding(TopProperty, new Binding(nameof(AppWindowData.Top)) { Source = MyAppWindowData, Mode = BindingMode.TwoWay });
            SetBinding(WidthProperty, new Binding(nameof(AppWindowData.Width)) { Source = MyAppWindowData, Mode = BindingMode.TwoWay });
            SetBinding(HeightProperty, new Binding(nameof(AppWindowData.Height)) { Source = MyAppWindowData, Mode = BindingMode.TwoWay });
            SetBinding(WindowStateProperty, new Binding(nameof(AppWindowData.WindowState)) { Source = MyAppWindowData, Mode = BindingMode.TwoWay });

            FixWindowLocate();// ウィンドウ位置設定が画面外だった場合は0(左上)にする
            //最小化されていた場合はNormalに戻す
            if (WindowState == WindowState.Minimized) { WindowState = WindowState.Normal; }
        }

        /// <summary>
        /// ウィンドウ位置設定が画面外だった場合は0(左上)にする
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private void FixWindowLocate()
        {
            if (MyAppWindowData.Left < -10 ||
                MyAppWindowData.Left > SystemParameters.VirtualScreenWidth - 100)
            {
                MyAppWindowData.Left = 0;
            }
            if (MyAppWindowData.Top < -10 ||
                MyAppWindowData.Top > SystemParameters.VirtualScreenHeight - 100)
            {
                MyAppWindowData.Top = 0;
            }
        }

        #endregion アプリのウィンドウ設定

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

        #endregion 初期処理








        #region ボタンクリック        
        //確認テスト用
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var neko = MyAppWindowData;
        }


        private void Button_Click_OpenPx4File(object sender, RoutedEventArgs e)
        {
            MyStatusMessage.Text = OpenPx4File();
        }

        /// <summary>
        /// px4ファイルを開く
        /// </summary>
        private string OpenPx4File()
        {
            if (MyRoot.MyThumbs.Count > 0)
            {
                MessageBoxResult result = MessageBox.Show(
                   "今の状態を保存してから開く？\n\n\n" +
                   "はい＿：保存してから開く\n\n" +
                   "いいえ：保存しないで開く\n\n" +
                   "キャンセル：開くのををキャンセル",
                   "px4ファイルを開く前の確認",
                   MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.Cancel);

                if (result == MessageBoxResult.Cancel)
                {
                    return MakeStatusMessage("ファイルを開くのをキャンセルした");
                }
                else if (result == MessageBoxResult.Yes)
                {
                    OverwriteSaveData();// 上書き保存
                }
            }

            return OpenPx4File2();
        }

        /// <summary>
        /// ダイアログからpx4ファイルを開く
        /// </summary>
        /// <returns></returns>
        private string OpenPx4File2()
        {
            OpenFileDialog dialog = new()
            {
                InitialDirectory = MyAppData.InitialDirectory,
                Multiselect = false,
                Filter = "対応ファイル | *.px4;"
            };
            if (dialog.ShowDialog() == true)
            {
                if (OpenPx4FileRootThumb(dialog.FileName))
                {
                    return MakeStatusMessage("px4ファイルを開いた");
                }
                return MakeStatusMessage("ファイルを開けなかった");
            }
            return MakeStatusMessage("ファイルを開くのをキャンセルした");
        }

        private void Button_Click_OpenFile(object sender, RoutedEventArgs e)
        {
            OpenFile();
        }

        /// <summary>
        /// ファイルを開く
        /// pc4の単一ファイルなら
        /// </summary>
        private void OpenFile()
        {
            var dialog = new OpenFileDialog();
            dialog.InitialDirectory = MyAppData.InitialDirectory;
            dialog.Multiselect = true;
            dialog.Filter = "対応ファイル | *.bmp; *.jpg; *.png; *.gif; *.tiff; *.px4; | すべて | *.* ";
            if (dialog.ShowDialog() == true)
            {
                string[] paths = dialog.FileNames;
                Array.Sort(paths);//ファイル名でソート
                if (MyAppData.IsFileNameDescendingOrder) { Array.Reverse(paths); }

            }

        }

        private void Button_Click_ResetRoot(object sender, RoutedEventArgs e)
        {
            MyStatusMessage.Text = ResetRootThumb();// RootThumbを新規作成してリセット
        }

        private void Button_Click_SaveData(object sender, RoutedEventArgs e)
        {
            MyStatusMessage.Text = SaveData();// 名前をつけて保存
        }

        private void Button_Click_OverwriteSave(object sender, RoutedEventArgs e)
        {
            MyStatusMessage.Text = OverwriteSaveData();// 上書き保存
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ResetWindowState();// ウィンドウの位置とサイズをリセット
        }

        #endregion ボタンクリック


        //ウィンドウにファイルドロップ時
        private void Window_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] paths = (string[])e.Data.GetData(DataFormats.FileDrop);
                MyRoot.OpenFiles(paths);
            }
        }

        #region メソッド

        #region 初期化、リセット系

        /// <summary>
        /// RootThumbを新規作成してリセット
        /// </summary>
        private string ResetRootThumb()
        {
            string message = "Item数が0だったのでリセットされなかった";
            if (MyRoot.MyThumbs.Count > 0)
            {
                MessageBoxResult result = MessageBox.Show(
                    "今の状態を保存してからリセットする？\n\n\n" +
                    "はい＿：保存してからリセット\n\n" +
                    "いいえ：保存しないでリセット\n\n" +
                    "キャンセル：リセットをキャンセル",
                    "リセット前の確認", MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.Cancel);
                if (result == MessageBoxResult.Yes)
                {
                    MyStatusMessage.Text = SaveData(MyAppData.CurrentOpenFilePath);
                    MyInitializeRootThumb();
                    message = "リセット完了";
                    MyAppData.CurrentOpenFilePath = string.Empty;//今開いているファイルパスもリセット
                }
                else if (result == MessageBoxResult.No)
                {
                    MyInitializeRootThumb();
                    message = "リセット完了";
                    MyAppData.CurrentOpenFilePath = string.Empty;//今開いているファイルパスもリセット
                }
                else
                {
                    message = "リセットは中止された";
                }
            }
            return MakeStatusMessage(message);
        }

        /// <summary>
        /// RootThumbとManageExCanvasの初期化
        /// </summary>
        private void MyInitializeRootThumb()
        {
            var data = new ItemData(ThumbType.Root);
            MyRoot = new RootThumb(data);
            var manager = new ManageExCanvas(MyRoot, new ManageData());
            MyManageExCanvas = manager;
            MyScrollViewer.Content = MyManageExCanvas;
        }

        /// <summary>
        /// ウィンドウの位置とサイズをリセット
        /// </summary>
        private void ResetWindowState()
        {
            MyAppWindowData = new AppWindowData();
            MyBindWindowData();
        }

        #endregion 初期化、リセット系

        #region Save

        /// <summary>
        /// 上書き保存
        /// </summary>
        /// <returns></returns>
        private string OverwriteSaveData()
        {
            return SaveData(MyAppData.CurrentOpenFilePath);
        }

        /// <summary>
        /// Saveダイアログを開いて名前を付けてData保存
        /// </summary>
        private string SaveData()
        {
            Microsoft.Win32.SaveFileDialog dialog = new()
            {
                Filter = "*.px4|*.px4",
                AddExtension = true,
            };

            if (dialog.ShowDialog() == true)
            {
                return SaveData(dialog.FileName);
            }
            else { return MakeStatusMessage("保存はキャンセルされた"); }
        }

        /// <summary>
        /// 指定パスにData保存、パスがない場合はSaveFileDialog
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private string SaveData(string filePath)
        {
            if (filePath == string.Empty) { return SaveData(); }
            if (MyRoot.SaveItemData(MyRoot.MyItemData, filePath))
            {
                MyAppData.CurrentOpenFilePath = filePath;// 今開いているファイルのパス更新
                return MakeStatusMessage("保存完了");
            }
            else { return MakeStatusMessage("保存に失敗"); }
        }

        #endregion Save

        /// <summary>
        /// メッセージに現在時刻を付けて返す
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private string MakeStatusMessage(string message)
        {
            return MakeStringNowTime() + "_" + message;
        }

        //今の日時をStringで作成
        private string MakeStringNowTime()
        {
            DateTime dt = DateTime.Now;
            //string str = dt.ToString("yyyyMMdd");            
            //string str = dt.ToString("yyyyMMdd" + "_" + "HHmmssfff");
            string str = dt.ToString(DATE_TIME_STRING_FORMAT);
            //string str = dt.ToString("yyyyMMdd" + "_" + "HH" + "_" + "mm" + "_" + "ss" + "_" + "fff");
            return str;
        }

        ///// <summary>
        ///// RootThumbのDataをpx4ファイルに保存
        ///// </summary>
        ///// <param name="filePath"></param>
        ///// <returns></returns>
        //private bool SaveRootData(string filePath)
        //{
        //    return MyRoot.SaveItemData(MyRoot.MyItemData, filePath);
        //}

        ///// <summary>
        ///// Saveダイアログを開いて保存
        ///// </summary>
        //private void SaveRootData()
        //{
        //    Microsoft.Win32.SaveFileDialog dialog = new()
        //    {
        //        Filter = "*.px4|*.px4",
        //        AddExtension = true,
        //    };
        //    if (dialog.ShowDialog() == true)
        //    {
        //        MyRoot.SaveItemData(MyRoot.MyItemData, dialog.FileName);
        //    }
        //}

        #region Load、Open、読み込み系

        /// <summary>
        /// px4ファイルを開いて、今のRootと入れ替える
        /// </summary>
        private bool OpenPx4FileRootThumb(string filePath)
        {
            if (filePath != string.Empty)
            {
                if (MyRoot.LoadItemData(filePath) is ItemData data)
                {
                    if (new RootThumb(data) is RootThumb root)
                    {
                        MyManageExCanvas.ChangeRootThumb(root);
                        MyRoot = root;
                        MyAppData.CurrentOpenFilePath = filePath;
                        return true;
                    }
                }
            }
            return false;
        }



        /// <summary>
        /// アプリ設定ファイルの読み込み
        /// </summary>
        private AppData? LoadAppData(string filePath)
        {
            //アプリのフォルダから設定ファイルを読み込む、ファイルがなかったら新規作成
            if (ItemDataKiso.Deserialize<AppData>(filePath) is AppData data)
            {
                return data;
            }
            return null;
        }
        private AppData? LoadAppData()
        {
            //アプリのフォルダから設定ファイルを読み込む、ファイルがなかったら新規作成
            string filePath = System.IO.Path.Combine(MyAppDirectory, APP_DATA_FILE_NAME);
            return LoadAppData(filePath);
        }

        #endregion Load、Open、読み込み系

        #endregion メソッド

    }
}