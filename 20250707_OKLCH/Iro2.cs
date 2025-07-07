using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace _20250707_OKLCH
{
    public class Iro2 : DependencyObject
    {
        public Iro2()
        {
            MyBind();
        }

        private void MyBind()
        {
            MultiBinding mb = new() { Converter = new MyConvOklch2() };
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(RProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(GProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(BProperty) });
            BindingOperations.SetBinding(this, MyOklchProperty, mb);

            BindingOperations.SetBinding(this, LProperty, new Binding() { Converter = new MyConvL(), Source = this, Path = new PropertyPath(MyOklchProperty) });
            BindingOperations.SetBinding(this, CProperty, new Binding() { Converter = new MyConvC(), Source = this, Path = new PropertyPath(MyOklchProperty) });
            BindingOperations.SetBinding(this, HProperty, new Binding() { Converter = new MyConvH(), Source = this, Path = new PropertyPath(MyOklchProperty) });

        }


        #region 依存関係プロパティ


        public byte R
        {
            get { return (byte)GetValue(RProperty); }
            set { SetValue(RProperty, value); }
        }
        public static readonly DependencyProperty RProperty =
            DependencyProperty.Register(nameof(R), typeof(byte), typeof(Iro2), new PropertyMetadata((byte)0));

        public byte G
        {
            get { return (byte)GetValue(GProperty); }
            set { SetValue(GProperty, value); }
        }
        public static readonly DependencyProperty GProperty =
            DependencyProperty.Register(nameof(G), typeof(byte), typeof(Iro2), new PropertyMetadata((byte)0));

        public byte B
        {
            get { return (byte)GetValue(BProperty); }
            set { SetValue(BProperty, value); }
        }
        public static readonly DependencyProperty BProperty =
            DependencyProperty.Register(nameof(B), typeof(byte), typeof(Iro2), new PropertyMetadata((byte)0));


        public (double l, double c, double h) MyOklch
        {
            get { return ((double, double, double))GetValue(MyOklchProperty); }
            set { SetValue(MyOklchProperty, value); }
        }
        public static readonly DependencyProperty MyOklchProperty =
            DependencyProperty.Register(nameof(MyOklch), typeof((double, double, double)), typeof(Iro2), new PropertyMetadata((0.0, 0.0, 0.0)));


        public double L
        {
            get { return (double)GetValue(LProperty); }
            set { SetValue(LProperty, value); }
        }
        public static readonly DependencyProperty LProperty =
            DependencyProperty.Register(nameof(L), typeof(double), typeof(Iro2), new PropertyMetadata(0.0));

        public double C
        {
            get { return (double)GetValue(CProperty); }
            set { SetValue(CProperty, value); }
        }
        public static readonly DependencyProperty CProperty =
            DependencyProperty.Register(nameof(C), typeof(double), typeof(Iro2), new PropertyMetadata(0.0));

        public double H
        {
            get { return (double)GetValue(HProperty); }
            set { SetValue(HProperty, value); }
        }
        public static readonly DependencyProperty HProperty =
            DependencyProperty.Register(nameof(H), typeof(double), typeof(Iro2), new PropertyMetadata(0.0));



        #endregion 依存関係プロパティ

    }

}
