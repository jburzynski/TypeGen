using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
using Microsoft.Win32;
using TypeGen.Core;

namespace TypeGen.Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Generator _generator;

        public MainWindow()
        {
            InitializeComponent();

            _generator = new Generator
            {
                Options = new GeneratorOptions
                {
                    BaseOutputDirectory = "../../generated files"
                }
            };
        }

        private void generateButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog { Filter = "All files (*.*)|*.*|DLL files (*.dll)|*.dll|EXE files (*.exe)|*.exe" };
            if (openFileDialog.ShowDialog() == true)
            {
                string fileName = openFileDialog.FileName;
                Assembly assembly = Assembly.LoadFrom(fileName);
                _generator.Generate(assembly);
            }
        }
    }
}
