using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace _20250616
{
    
    public class CustomControl1 : Control
    {
        static CustomControl1()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomControl1), new FrameworkPropertyMetadata(typeof(CustomControl1)));
        }
    }


    public class FreehandGrid : Control
    {
        
        static FreehandGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FreehandGrid), new FrameworkPropertyMetadata(typeof(FreehandGrid)));
        }
        public FreehandGrid()
        {

        }
    }


}
