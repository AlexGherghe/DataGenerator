using System;
using System.Windows.Controls;


namespace DataGenerator
{
    /// <summary>
    /// Interaction logic for Object_Double.xaml
    /// </summary>
    public partial class Object_Double : UserControl
    {
        public Object_Double()
        {
            InitializeComponent();
        }

        public String getType()
        {
            return "Float";
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
