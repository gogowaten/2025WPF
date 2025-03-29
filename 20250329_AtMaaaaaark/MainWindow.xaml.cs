using System.IO;
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

namespace _20250329_AtMaaaaaark
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string aaa = "D:\\ブログ用\\gazo-.png";
            string bbb = @"D:\ブログ用\gazo-.png";
            var AAA = System.IO.Path.GetDirectoryName(aaa);
            var BBB = System.IO.Path.GetDirectoryName(bbb);
            var ccc = @"E:\\20250329ItemData.zip";
            var CCC = System.IO.Path.GetDirectoryName(ccc);
            var ddd = "E:\\20250329ItemData.zip";
            var DDD = System.IO.Path.GetDirectoryName(ddd);
            var inu = Directory.Exists(AAA);
            var neko = Directory.Exists(BBB);
            var tako = Directory.Exists(CCC);
            var ika = Directory.Exists(DDD);
        }
    }
}