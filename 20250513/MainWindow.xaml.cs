using System.Text;
using System.Transactions;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

//[WPF]アニメーションの再生完了を待って別の処理を開始する方法
//https://teratail.com/questions/299699

namespace _20250513
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double MyGridSize = 40.0;//グリッドサイズ
        private bool isAnimetion;//アニメーション中の判定
        private Storyboard MyStoryboard = new();
        private DoubleAnimation MyLeftAnime;
        private DoubleAnimation MyTopAnime;
        public MainWindow()
        {
            InitializeComponent();
            
            MyStoryboard.Completed += Storyboard_Completed;

            //横移動用アニメ
            //数値はアニメ終了までの時間、秒
            MyLeftAnime = new DoubleAnimation() { Duration = TimeSpan.FromSeconds(0.1) };
            //MyLeftAnime = new DoubleAnimation() { Duration = TimeSpan.FromSeconds(0.03) };
            Storyboard.SetTargetProperty(MyLeftAnime, new PropertyPath(Canvas.LeftProperty));
            MyStoryboard.Children.Add(MyLeftAnime);

            //縦移動用アニメ
            MyTopAnime = new DoubleAnimation() { Duration = TimeSpan.FromSeconds(0.1) };
            //MyTopAnime = new DoubleAnimation() { Duration = TimeSpan.FromSeconds(0.03) };
            Storyboard.SetTargetProperty(MyTopAnime, new PropertyPath(Canvas.TopProperty));
            MyStoryboard.Children.Add(MyTopAnime);
        }

        private void Storyboard_Completed(object? sender, EventArgs e)
        {
            isAnimetion = false;
        }

        //アニメなしのグリッドスナップ移動
        private void MyThumb_DragDelta_Normal(object sender, DragDeltaEventArgs e)
        {
            if (sender is Thumb t)
            {
                var yoko = e.HorizontalChange;
                yoko = (int)(yoko / MyGridSize) * MyGridSize;
                var tate = (int)(e.VerticalChange / MyGridSize) * MyGridSize;
                Canvas.SetLeft(t, Canvas.GetLeft(t) + yoko);
                Canvas.SetTop(t, (int)(Canvas.GetTop(t) +tate));
            }
        }

        //アニメありでのグリッドナップ移動
        private void MyThumb_DragDelta_Animation(object sender, DragDeltaEventArgs e)
        {
            if (sender is Thumb t)
            {
                var imayoko = Canvas.GetLeft(t);
                var imatate = Canvas.GetTop(t);
                double atoyoko = imayoko;
                var yoko = e.HorizontalChange;
                var tate = e.VerticalChange;

                if (yoko > 0)
                {
                    atoyoko = imayoko + (int)(yoko / MyGridSize + 0.5) * MyGridSize;
                }
                else if (yoko < 0)
                {
                    atoyoko = imayoko + (int)(yoko / MyGridSize - 0.5) * MyGridSize;
                }

                double atotate = imatate + (int)(tate / MyGridSize + 0.5) * MyGridSize;

                if (imayoko != atoyoko || imatate != atotate)
                {
                    if (isAnimetion == false)
                    {
                        isAnimetion = true;
                        IdouAnime3(t, imayoko, atoyoko, imatate, atotate);
                    }
                }
                e.Handled = true;
            }
        }

        private void IdouAnime3(FrameworkElement element, double begin, double end, double imatate, double atotate)
        {
            MyLeftAnime.From = begin;
            MyLeftAnime.To = end;
            MyTopAnime.From = imatate;
            MyTopAnime.To = atotate;
            element.BeginStoryboard(MyStoryboard);
        }

        //private void IdouAnime2(FrameworkElement element, double begin, double end, double imatate, double atotate)
        //{
        //    //Storyboard storyboard = new();
        //    DoubleAnimation yokoAnime = new()
        //    {
        //        From = begin,
        //        To = end,
        //        Duration = TimeSpan.FromSeconds(0.05),
        //    };
        //    DoubleAnimation tateAnime = new()
        //    {
        //        From = imatate,
        //        To = atotate,
        //        Duration = TimeSpan.FromSeconds(0.05),
        //    };
        //    MyStoryboard.Children.Clear();
        //    //MyStoryboard.Completed += Storyboard_Completed;
        //    Storyboard.SetTargetProperty(yokoAnime, new PropertyPath(Canvas.LeftProperty));
        //    MyStoryboard.Children.Add(yokoAnime);
        //    Storyboard.SetTargetProperty(tateAnime, new PropertyPath(Canvas.TopProperty));
        //    MyStoryboard.Children.Add(tateAnime);
        //    element.BeginStoryboard(MyStoryboard);
        //}

        //private void IdouAnime(FrameworkElement element, double begin, double end, double imatate, double atotate)
        //{
        //    Storyboard storyboard = new();
        //    DoubleAnimation yokoAnime = new()
        //    {
        //        From = begin,
        //        To = end,
        //        Duration = TimeSpan.FromSeconds(0.05),
        //    };
        //    DoubleAnimation tateAnime = new()
        //    {
        //        From = imatate,
        //        To = atotate,
        //        Duration = TimeSpan.FromSeconds(0.05),
        //    };

        //    storyboard.Completed += Storyboard_Completed;
        //    Storyboard.SetTargetProperty(yokoAnime, new PropertyPath(Canvas.LeftProperty));
        //    storyboard.Children.Add(yokoAnime);
        //    Storyboard.SetTargetProperty(tateAnime, new PropertyPath(Canvas.TopProperty));
        //    storyboard.Children.Add(tateAnime);
        //    element.BeginStoryboard(storyboard);
        //}


        //public double MyGridSize
        //{
        //    get { return (double)GetValue(MyGridSizeProperty); }
        //    set { SetValue(MyGridSizeProperty, value); }
        //}
        //public static readonly DependencyProperty MyGridSizeProperty =
        //    DependencyProperty.Register(nameof(MyGridSize), typeof(double), typeof(MainWindow), new PropertyMetadata(1.0));

    }
}