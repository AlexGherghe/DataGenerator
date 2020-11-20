using System;
using System.Windows.Controls;

namespace DataGenerator
{
    /// <summary>
    /// Interaction logic for Object_Char.xaml
    /// </summary>
    public partial class Object_Char : UserControl
    {
        public Object_Char()
        {
            InitializeComponent();
        }
        public String getType()
        {
            return "Int";
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
