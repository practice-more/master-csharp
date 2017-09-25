using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
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

namespace Async
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string _url = "http://msdn.microsoft.com";
        public MainWindow()
        {
            InitializeComponent();
            textBlockSync.Text = textBlockAsync.Text = "";
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Task showTextUITask = Task.Factory.StartNew(
                () => { textBlockSync.Text = "You can not move the form until the function end."; }, 
                CancellationToken.None,
                TaskCreationOptions.DenyChildAttach,
                TaskScheduler.FromCurrentSynchronizationContext());
            showTextUITask.Wait();

            HttpClient client = new HttpClient();
            string result = client.GetStringAsync(_url).Result;
        }

        private async void GetStringAsync(string url)
        {
            HttpClient client = new HttpClient();
            textBlockAsync.Text = "You can move the form as the task is running in the background.";
            await client.GetStringAsync(url);
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            GetStringAsync(_url);
        }
    }
}
