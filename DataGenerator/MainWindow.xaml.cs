
using System;
using System.Collections.Generic;
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
using System.Xaml.Schema;

namespace DataGenerator
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    //TODO : make TextBoxes unfocusable unless corresponding CheckBox is checked
    //TODO : restrict the input of TextBoxes to numbers

    public partial class MainWindow : Window
    {
        private Control[] arr_number_config, arr_lorem_config;

        public MainWindow()
        {
            InitializeComponent();
            string[] formats = { "Array", "Matrix", "Objects" };
            string[] types = { "Int", "Float", "Lorem Ipsum" };
            arr_number_config = new Control[] {Size_TB, MinVal_TB, MaxVal_TB, Separator_TB, Separator_CB, Range_CB, Size_Lbl};
            arr_lorem_config = new Control[] { Size_TB, Size_Lbl, No_Separator_RB, Space_RB, Custom_Separator_RB, Lorem_separator_TB };

            Format_CB.ItemsSource = formats;
            Type_CB.ItemsSource = types;
            Type_CB.SelectedIndex = 0;  //auto selects Int
            Type_CB.Visibility = Visibility.Collapsed;
            foreach (Control obj in arr_number_config)
                obj.Visibility = Visibility.Collapsed;
            foreach (Control obj in arr_lorem_config)
                obj.Visibility = Visibility.Collapsed;
        }

        private void Format_CB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If "Objects" is selected, everything below disappears
            if (Format_CB.SelectedItem.Equals("Array") || Format_CB.SelectedItem.Equals("Matrix"))
                Type_CB.Visibility = Visibility.Visible;
            else
                Type_CB.Visibility = Visibility.Collapsed;
        }

        private void Type_CB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Type_CB.SelectedItem.Equals("Int") || Type_CB.SelectedItem.Equals("Float"))
            {
                foreach (Control obj in arr_lorem_config)
                    obj.Visibility = Visibility.Collapsed;
                foreach (Control obj in arr_number_config)
                    obj.Visibility = Visibility.Visible;
            }
            else if (Type_CB.SelectedItem.Equals("Lorem Ipsum"))
            {
                foreach (Control obj in arr_number_config)
                    obj.Visibility = Visibility.Collapsed;
                foreach (Control obj in arr_lorem_config)
                    obj.Visibility = Visibility.Visible;
            }

        }
        private void Type_CB_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Type_CB.Visibility == Visibility.Collapsed)     //if Type_CB is collapsed, everything below it is
            {
                foreach (Control obj in arr_number_config)
                    obj.Visibility = Visibility.Collapsed;
                foreach (Control obj in arr_lorem_config)
                    obj.Visibility = Visibility.Collapsed;
            }
            else if (Type_CB.SelectedItem.Equals("Int") || Type_CB.SelectedItem.Equals("Float"))    //summons back the required fields
                foreach (Control obj in arr_number_config)
                    obj.Visibility = Visibility.Visible;
            else
                foreach (Control obj in arr_lorem_config)
                    obj.Visibility = Visibility.Visible;

        }

        private void Generate_Btn_Click(object sender, RoutedEventArgs e)
        {
            Generator gen = new Generator();
            //TODO : do validity checkups before calling generator
            if (Type_CB.Text.Equals("Int") || Type_CB.Text.Equals("Float"))
            {
                int min = Int32.MinValue, max = Int32.MaxValue, len;
                bool is_float = false;
                String separator = " ";

                if (Type_CB.Text.Equals("Float"))   //if "Float" is selected
                    is_float = true;
                if (Range_CB.IsChecked == true)     //if user wants a custom value range
                {
                    min = Int32.Parse(MinVal_TB.Text);
                    max = Int32.Parse(MaxVal_TB.Text);
                }
                if (Separator_CB.IsChecked == true)     //if user wants a custom separator
                    separator = Separator_TB.Text;
                len = Int32.Parse(Size_TB.Text);
                String result = gen.num_arr(len, min, max, separator, is_float);
                Result_TB.Text = result;
            }
            if (Type_CB.Text.Equals("Lorem Ipsum"))
            {
                String separator = " ";
                int len;

                len = Int32.Parse(Size_TB.Text);
                if (No_Separator_RB.IsChecked == true)
                    separator = "";
                else if (Space_RB.IsChecked == true)
                    separator = " ";
                else if (Custom_Separator_RB.IsChecked == true)
                    separator = Lorem_separator_TB.Text;
                String result = gen.lorem_arr(len, separator);
                Result_TB.Text = result;
            }
        }
    }

    public class Generator
    {
        public String num_arr(int len, int min_val, int max_val, String separator, bool is_float)
        {
            var rand = new Random();
            String array = new String("");
            int x;

                for (int i = 0; i < len; i++)
                {
                x = rand.Next(min_val, max_val);
                if (is_float == true)
                    array += rand.NextDouble() * x;
                else
                    array += x;
                array += separator;
                }
            return array;
        }
        public String lorem_arr(int len, String separator)
        {
            const short lorem_size = 20;
            String array = new String("");
            String[] words = new[]{"lorem", "ipsum", "dolor", "sit", "amet", "consectetuer",
        "adipiscing", "elit", "sed", "diam", "nonummy", "nibh", "euismod",
        "tincidunt", "ut", "laoreet", "dolore", "magna", "aliquam", "erat"};
            short[] lenghts = new short[] {5, 5, 5, 3, 4, 12, 10, 4, 3, 4, 7, 4, 7, 9, 2, 7, 6, 5, 7, 4 };
            int curr_len = 0, pos, sep_len;
            Random rand = new Random();

            sep_len = separator.Length;
            while (curr_len < len)
            {
                pos = rand.Next(0, lorem_size);
                array += words[pos];
                array += separator;
                curr_len += lenghts[pos] + sep_len;
            }
            if (curr_len >= len)     //cuts excess characters
                array = array.Remove(len);
            return array;
        }
    }
}
