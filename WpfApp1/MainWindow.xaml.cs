using iText.Kernel.Pdf;
using iText.Layout;
using System;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
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
using System.IO;

namespace WpfApp1
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


        public const String DEST = "results/chapter01/hello_world.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Run()
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public static void CreatePdf(String dest)
        {
            //Initialize PDF writer
            PdfWriter writer = new PdfWriter(dest);
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(writer);
            // Initialize document
            Document document = new Document(pdf);
            //Add paragraph to the document
            document.Add(new iText.Layout.Element.Paragraph("Hello World!"));
            //Close document
            document.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Run();
        }
    }
}
