using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace _20250204
{
    public class AAA : Shape
    {
        static AAA()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AAA),new FrameworkPropertyMetadata(typeof(AAA)));
        }
        public AAA()
        {

        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
    
}
