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

namespace Dek.Bel.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Label_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Style style = FindResource("CategoryLabelStyle") as Style;
            var addLabel = CategoryWrapPanel.Children[CategoryWrapPanel.Children.Count - 1];
            CategoryWrapPanel.Children.Remove(addLabel);
            CategoryWrapPanel.Children.Add(new Label { Style = style });
            CategoryWrapPanel.Children.Add(addLabel);

        }
    }
}
