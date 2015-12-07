using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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

namespace DirectorySizeUtility
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public long ROOT_SIZE;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            FileExplorer.Items.Clear();

            DirectoryInfo item = new DirectoryInfo(SearchBox.Text);
            TreeViewItem root = new TreeViewItem();

            this.ROOT_SIZE = DirSize(item);

            root.Header = String.Format("{0} -- {1}Kb : {2}", item.Name, ROOT_SIZE / 1000, PercentageOfParent(ROOT_SIZE));
            FileExplorer.Items.Add(root);

            Dig(item, root);

        }

        public String PercentageOfParent(long Size)
        {
            string outstring = "";
            int percentage = (int) Math.Floor(100 * ((double)Size / ROOT_SIZE));

            outstring = percentage.ToString() + "% : " + new String('▓', percentage) + new string('░', 100 - percentage);
            //outstring = percentage.ToString() + "% : " + new string('░', 100 - percentage) + new String('▓', percentage);
            return outstring;

            //return percentage.ToString();
        }

        public String PercentageOfParent(long ParentSize, long Size)
        {
            string outstring = "";
            int percentage = (int)Math.Floor(100 * ((double)Size / ParentSize));

            outstring = percentage.ToString() + "% : " + new String('█', percentage);
            return outstring;

            //return percentage.ToString();
        }

        private long DirSize(DirectoryInfo item)
        {
            long Size = 0;

            try
            {
                FileInfo[] fis = item.GetFiles();
                foreach (FileInfo fi in fis)
                {
                    Size += fi.Length;
                }
                DirectoryInfo[] dis = item.GetDirectories();
                foreach (DirectoryInfo di in dis)
                {
                    Size += DirSize(di);
                }
            }
            catch ( Exception e )
            {
                //do nothing
            }

            return (Size);
        }

        private void Dig(DirectoryInfo dir, TreeViewItem t)
        {
            try
            {
                foreach (DirectoryInfo item in dir.EnumerateDirectories())
                {
                    var subitem = new TreeViewItem();

                    //subitem.Header = item.Name;

                    long itemsize = DirSize(item);

                    subitem.Header = String.Format("{0} -- {1}Kb : {2}", item.Name, (itemsize / 1000), PercentageOfParent(itemsize));
                    //subitem.Header = String.Format("{0} -- {1}Kb", (itemsize / 1000), item.Name);

                    t.Items.Add(subitem);

                    Dig(item, subitem);
                }
                foreach (FileInfo item in dir.EnumerateFiles()) {
                    var subitem = new TreeViewItem();
                    subitem.Header = String.Format("{0} -- {1}Kb : {2}", item.Name, (item.Length / 1000), PercentageOfParent(item.Length));

                    t.Items.Add(subitem);
                }
            }
            catch (Exception e)
            {
                var subitem = new TreeViewItem() { Header = "YOU GET NOTHING" };

                t.Items.Add(subitem);

                Console.WriteLine("YOU GET NOTHING");
            }
        }


    }

    public class HeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int hold;
            bool test = Int32.TryParse(value.ToString(), out hold);
            if (test)
            {
                return hold - 20;
            }

            return Int32.MaxValue;
            //throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class HeightConverter2 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int hold;
            bool test = Int32.TryParse(value.ToString(), out hold);
            if (test)
            {
                return hold - 40;
            }

            return Int32.MaxValue;
            //throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
