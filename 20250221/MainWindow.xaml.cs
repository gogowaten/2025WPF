using System.ComponentModel;
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

namespace _20250221;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        TBT.MyItemData.MyForegroundR = 0;
        //TBT.MyForeground = Brushes.Blue;
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
        //myTB2.MyItemData.MyZIndex--;
        Panel.SetZIndex(myTB2, 10);
    }

    private void Button_Click_2(object sender, RoutedEventArgs e)
    {
        myTB2.MyItemData.MyForeground = Brushes.Green;
    }
}

public class AAA : TextBlock, INotifyPropertyChanged
{
    private byte _myFill;
    [DataMember] public byte MyFill { get => _myFill; set => SetProperty(ref _myFill, value); }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void SetProperty<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string? name = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return;
        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}