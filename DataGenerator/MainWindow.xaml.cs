﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace DataGenerator
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    //TODO : make TextBoxes unfocusable unless corresponding CheckBox is checked
    //TODO : restrict the input of TextBoxes to numbers
    //TODO : check size for negative number

    public partial class MainWindow : Window
    {
        private Control[] arr_number_config, arr_lorem_config, matrix_config, arr_config;
        String[] types_matrix, types_arr, types_object;
        List<Type> obj_types;
        Generator gen;

        public MainWindow()
        {

            InitializeComponent();
            
            string[] formats = { "Array", "Matrix", "Objects" };
            types_arr = new string[] { "Int", "Float", "Lorem Ipsum" };
            types_matrix = new string[] { "Int", "Float" };
            types_object = new string[] { "Int", "Float", "Name", "Char" };
            
            arr_config = new Control[] { Size_TB, MinVal_TB, MaxVal_TB, Separator_TB, Separator_CB, Range_CB, Size_Lbl, No_Separator_RB, Space_RB, Custom_Separator_RB, Lorem_separator_TB };
            arr_number_config = new Control[] { Size_TB, MinVal_TB, MaxVal_TB, Separator_TB, Separator_CB, Range_CB, Size_Lbl };
            arr_lorem_config = new Control[] { Size_TB, Size_Lbl, No_Separator_RB, Space_RB, Custom_Separator_RB, Lorem_separator_TB };
            matrix_config = new Control[] { Matrix_X, Matrix_Y, Diagonal_LB, Matrix_Size_Lbl, Separator_TB, Separator_CB, MinVal_TB, MaxVal_TB, Range_CB };

            long init = GC.GetTotalMemory(true);
            Trace.WriteLine("Array urile consuma " + (GC.GetTotalMemory(true) - init));
            gen = new Generator();
            
            obj_types = new List<Type> { };
            Format_CB.ItemsSource = formats;
            Type_CB.ItemsSource = types_arr;
            Format_CB.SelectedIndex = 0;
            Type_CB.SelectedIndex = 0;  //auto selects Int  
            Obj_CB.ItemsSource = types_object;
            Obj_CB.SelectedIndex = -1;
           

        }

        private void Vanish_Obj()
        {
            Clear_Btn.Visibility = Visibility.Collapsed;
            Obj_CB.Visibility = Visibility.Collapsed;
            Obj_SP.Visibility = Visibility.Collapsed;
            Size_TB.Visibility = Visibility.Collapsed;
            Size_Lbl.Visibility = Visibility.Collapsed;
            Obj_SP.Children.Clear();
            obj_types.Clear();
        }

        private void Show_Obj()
        {
            Clear_Btn.Visibility = Visibility.Visible;
            Obj_CB.Visibility = Visibility.Visible;
            Obj_SP.Visibility = Visibility.Visible;
            Size_TB.Visibility = Visibility.Visible;
            Size_Lbl.Visibility = Visibility.Visible;
        }

        private void Generate_Array()
        {
            if (Type_CB.Text.Equals("Int") || Type_CB.Text.Equals("Float"))
            {
                int min = Int32.MinValue, max = Int32.MaxValue, len;
                bool is_float = false;
                String separator = " ";
                if (Int32.TryParse(Size_TB.Text, out len) == false)
                {
                    MessageBox.Show("Please select size", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (len < 1)    //If the size is negative or 0
                {
                    MessageBox.Show("Size cannot be <1", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (Type_CB.Text.Equals("Float"))   //if "Float" is selected
                    is_float = true;
                if (Range_CB.IsChecked == true)     //Parsing the custom range
                {
                    int aux;
                    if (Int32.TryParse(MinVal_TB.Text, out aux) == true)
                        min = aux;
                    if (Int32.TryParse(MaxVal_TB.Text, out aux) == true)
                        max = aux;
                }
                if (Separator_CB.IsChecked == true)     //if user wants a custom separator
                    separator = Separator_TB.Text;
                Result_TB.Text = gen.num_arr(len, min, max, separator, is_float);   //generating data

            }
            if (Type_CB.Text.Equals("Lorem Ipsum"))
            {
                String separator = " ";
                int len;

                if (Int32.TryParse(Size_TB.Text, out len) == false) //if size is not a number
                {
                    MessageBox.Show("Please select size", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                if (len < 1)    //if size <1
                {
                    MessageBox.Show("Size cannot be <1", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                if (No_Separator_RB.IsChecked == true)
                    separator = "";
                else if (Space_RB.IsChecked == true)
                    separator = " ";
                else if (Custom_Separator_RB.IsChecked == true)
                    separator = Lorem_separator_TB.Text;
                Result_TB.Text = gen.lorem_arr(len, separator); //generate and display data
            }
        }

        private void Generate_Matrix()
        {
            int rows, columns, special_props, min = Int32.MinValue, max = Int32.MaxValue;
            bool isFloat = false;
            String separator = " ";
            if (Int32.TryParse(Matrix_X.Text, out rows) == false || Int32.TryParse(Matrix_Y.Text, out columns) == false)
            {
                MessageBox.Show("Please select size", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (rows < 1 || columns < 1)    //if either size is <1
            {
                MessageBox.Show("Size cannot be <1", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (Range_CB.IsChecked == true)     //Parsing custom range
            {
                int aux;
                if (Int32.TryParse(MinVal_TB.Text, out aux) == true)
                    min = aux;
                if (Int32.TryParse(MaxVal_TB.Text, out aux) == true)
                    max = aux;
            }

            if (Type_CB.SelectedItem.Equals("Float"))
                isFloat = true;
            if (Separator_CB.IsChecked == true)
                separator = Separator_TB.Text;
            special_props = Diagonal_LB.SelectedIndex;

            Result_TB.Text = gen.matrix(rows, columns, min, max, isFloat, separator, special_props);
            Diagonal_LB.SelectedIndex = -1;
        }

        private void Generate_Objects()
        {
            int size = 1, aux;
            Label label = new Label();
            Object_Int obj_int = new Object_Int();
            Object_Double obj_double = new Object_Double();
            Object_Char obj_char = new Object_Char();
            String result = new string("");
            if (Int32.TryParse(Size_TB.Text, out aux) == true)  //parsing size
                size = aux;
            if (size < 1)
            {
                MessageBox.Show("Size cannot be <1", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            for (int i = 0; i < size; i++)
                for (int j = 0; j < Obj_SP.Children.Count; j++)
                {
                    if (obj_types[j] == typeof(Label))
                    {
                        result += gen.name();
                        result += "\r\n";
                    }
                    else if (obj_types[j] == typeof(Object_Int))
                    {
                        obj_int = (Object_Int)Obj_SP.Children[j];
                        result += gen.num_arr(obj_int.getSize(), obj_int.getMin(), obj_int.getMax(), " ", false);
                        result += "\r\n";
                    }
                    else if (obj_types[j] == typeof(Object_Double))
                    {
                        obj_double = (Object_Double)Obj_SP.Children[j];
                        result += gen.num_arr(obj_double.getSize(), obj_double.getMin(), obj_double.getMax(), " ", true);
                        result += "\r\n";
                    }
                    else if (obj_types[j] == typeof(Object_Char))
                    {
                        obj_char = (Object_Char)Obj_SP.Children[j];
                        result += gen.lorem_arr(obj_char.getSize(), "");
                        result += "\r\n";
                    }
                }
            Result_TB.Text = result;
        }

        private void Format_CB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // TODO : Change what happens when Objects is selected
            if (Format_CB.SelectedItem.Equals("Array"))
            {
                Vanish_Obj();
                foreach (Control obj in matrix_config)
                    obj.Visibility = Visibility.Collapsed;
                Type_CB.ItemsSource = types_arr;
                Type_CB.Visibility = Visibility.Visible;
                Type_CB.SelectedIndex = -1;        //forces load of fields in case visibility was not changed


            }
            else if (Format_CB.SelectedItem.Equals("Matrix"))
            {
                Vanish_Obj();
                foreach (Control obj in arr_config)
                    obj.Visibility = Visibility.Collapsed;
                Type_CB.ItemsSource = types_matrix;
                Type_CB.Visibility = Visibility.Visible;
                Type_CB.SelectedIndex = -1;    //forces load of fields in case visibility was not changed
            }
            else
            {
                Type_CB.Visibility = Visibility.Collapsed;
                Show_Obj();
            }
        }

        private void Obj_CB_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            e.Handled = true;
        }

        private void Type_CB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Type_CB.SelectedIndex == -1)    //if Visibility was se tot visible
            {
                Type_CB.SelectedIndex = 0;
                return;
            }
            if (Format_CB.SelectedItem.Equals("Array"))
            {
                foreach (Control obj in matrix_config)
                    obj.Visibility = Visibility.Collapsed;
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
            else if (Format_CB.SelectedItem.Equals("Matrix"))
            {
                foreach (Control obj in arr_config)
                    obj.Visibility = Visibility.Collapsed;
                foreach (Control obj in matrix_config)
                    obj.Visibility = Visibility.Visible;
            }

        }

        private void Type_CB_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Type_CB.Visibility == Visibility.Collapsed)     //if Type_CB is collapsed, everything below it is
            {
                foreach (Control obj in arr_config)
                    obj.Visibility = Visibility.Collapsed;
                foreach (Control obj in matrix_config)
                    obj.Visibility = Visibility.Collapsed;
            }
            else
                Type_CB.SelectedIndex = -1;

        }

        private void Obj_CB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Obj_CB.SelectedIndex == -1)
                return;
            if (Obj_CB.SelectedItem.Equals("Int"))
            {
                obj_types.Add(typeof(Object_Int));
                Obj_SP.Children.Add(new Object_Int());
                Obj_CB.SelectedIndex = -1;
                return;
            }
            if (Obj_CB.SelectedItem.Equals("Float"))
            {

                obj_types.Add(typeof(Object_Double));
                Obj_SP.Children.Add(new Object_Double());
                Obj_CB.SelectedIndex = -1;
                return;
            }
            if (Obj_CB.SelectedItem.Equals("Char"))
            {

                obj_types.Add(typeof(Object_Char));
                Obj_SP.Children.Add(new Object_Char());
                Obj_CB.SelectedIndex = -1;
                return;
            }
            if (Obj_CB.SelectedItem.Equals("Name"))
            {
                Label aux = new Label();
                aux.Content = "Name";
                aux.VerticalAlignment = VerticalAlignment.Center;
                aux.Margin = new Thickness(0, 10, 0, 10);
                obj_types.Add(typeof(Label));
                Obj_SP.Children.Add(aux);
                Obj_CB.SelectedIndex = -1;
                return;
            }
        }

        private void Clear_Btn_Click(object sender, RoutedEventArgs e)
        {
            Obj_SP.Children.Clear();
            obj_types.Clear();
        }

        private void Generate_Btn_Click(object sender, RoutedEventArgs e)
        {

            if (Format_CB.SelectedItem.Equals("Array"))
                Generate_Array();
            else if (Format_CB.SelectedItem.Equals("Matrix"))
                Generate_Matrix();
            else if (Format_CB.SelectedItem.Equals("Objects"))
                Generate_Objects();
        }
    }


    public class Generator
    {
        static Random rand;
        static String[] names;
        static Generator()
        {
            rand = new Random();
            using (StreamReader sr = new StreamReader("Names.txt"))
            {
                names = sr.ReadToEnd().Split(",");
            }
        }
        public String num_arr(int len, int min_val, int max_val, String separator, bool is_float)
        {

            String result = new String("");
            int x;

            for (int i = 0; i < len; i++)
            {
                x = rand.Next(Math.Min(min_val, max_val), Math.Max(min_val, max_val));
                if (is_float == true)
                    result += Math.Round(rand.NextDouble() * x, 2);
                else
                    result += x;
                result += separator;
            }
            result = result.Remove(result.Length - separator.Length);
            return result;
        }
        public String lorem_arr(int len, String separator)
        {
            const short lorem_size = 20;
            String array = new String("");
            String[] words = new[]{"lorem", "ipsum", "dolor", "sit", "amet", "consectetuer",
        "adipiscing", "elit", "sed", "diam", "nonummy", "nibh", "euismod",
        "tincidunt", "ut", "laoreet", "dolore", "magna", "aliquam", "erat"};
            short[] lenghts = new short[] { 5, 5, 5, 3, 4, 12, 10, 4, 3, 4, 7, 4, 7, 9, 2, 7, 6, 5, 7, 4 };
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
            if (curr_len > len)     //cuts excess characters
                array = array.Remove(len);
            return array;
        }
        public String matrix(int rows, int cols, int min, int max, bool isFloat, String separator, int special_props)
        {
            /*special props: 
             * 0 - upper diagonal
             * 1 - lower diagonal*/
            String result = new string("");
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                {
                    if (special_props == 0 && i > j)    //if upper diagonal
                        result += "0";
                    else if (special_props == 1 && i < j)   //if lower diagonal
                        result += "0";
                    else
                    {

                        if (isFloat == true)
                            result += Math.Round(rand.NextDouble() * (Math.Max(min, max) - Math.Min(min, max)) + Math.Min(min, max), 2);
                        else
                            result += rand.Next(Math.Min(min, max), Math.Max(min, max));
                    }
                    if (j == cols - 1)  //last item of line
                        result += "\r\n";
                    else result += separator;
                }
            result = result.Remove(result.Length - separator.Length);
            return result;
        }
        public String name()
        {
            return names[rand.Next(0, names.Length)];
        }

    }
}
