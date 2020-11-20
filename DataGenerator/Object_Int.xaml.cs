using System;
using System.Windows.Controls;


namespace DataGenerator
{
    /// <summary>
    /// Interaction logic for Object_Int.xaml
    /// </summary>
    public partial class Object_Int : UserControl
    {
        public Object_Int()
        {
            InitializeComponent();
        }
        public int getMin()
        {
            int result = Int32.MinValue, aux;
            if (Int32.TryParse(min_range_TB.Text, out aux) == true)
                result = aux;
            return result;
        }
        public int getMax()
        {
            int result = Int32.MaxValue, aux;
            if (Int32.TryParse(max_range_TB.Text, out aux) == true)
                result = aux;
            return result;
        }
        public int getSize()
        {
            int result = 1, aux;
            if (Int32.TryParse(size_TB.Text, out aux) == true)
                result = aux;
            return result;
        }
    }
}
