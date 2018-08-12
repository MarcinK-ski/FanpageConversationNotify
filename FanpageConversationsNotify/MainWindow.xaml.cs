using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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

namespace FanpageConversationsNotify
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
         NameValueCollection config = ConfigurationManager.AppSettings;

        Fanpage fanpage;
        Conversations conversations;
        bool wasNewMessageConfirmed = true;

        public MainWindow()
        {
            fanpage = new Fanpage(config["fanpageid"], config["token"]);
            conversations = new Conversations();

            InitializeComponent();
            LastRefreshTSLabel.Content = "None";
            FanpageIDLabel.Content = config["fanpageid"];
        }

        /// <summary>
        /// Run method periodily 
        /// </summary>
        /// <param name="fanpage"></param>
        /// <param name="firstWaiting"></param>
        /// <param name="period"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async Task RunCheckingTaskAsync(Fanpage fanpage, TimeSpan firstWaiting, TimeSpan period, CancellationToken token)
        {
            if (firstWaiting > TimeSpan.Zero)
                await Task.Delay(firstWaiting, token);

            while (!token.IsCancellationRequested)
            {
                await CheckNewMessageComes(fanpage);

                if (period > TimeSpan.Zero)
                    await Task.Delay(period, token);
            }
        }

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            int period = 10;
            string periodFromConfig = config["period"];
            if(periodFromConfig != null)
                if(Int32.TryParse(periodFromConfig, out int temp))
                {
                    period = temp;
                }

            await RunCheckingTaskAsync(fanpage, TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(period), CancellationToken.None);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fanpage"></param>
        /// <returns></returns>
        private async Task CheckNewMessageComes(Fanpage fanpage)
        {
            if (wasNewMessageConfirmed)
            {
                bool isNewMessage = await conversations.IsNewMessage(fanpage);

                if (isNewMessage)
                    wasNewMessageConfirmed = false;

                await ConversationsInfo(isNewMessage);

                TSLabel.Content = conversations.lastConversationDateTime.ToString();
                LastRefreshTSLabel.Content = DateTime.Now.ToString();
            }
            else
            {
                InfosErrorsLabel.Content = "Last message wasn't confirmed. REFRESH ISN'T POSSIBLE!";
                await Task.Delay(3000);
            }

            InfosErrorsLabel.Content = "No infos";

        }

        /// <summary>
        /// Refresh messages info
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CheckNewMessagesAsync(object sender, RoutedEventArgs e)
        {
            await CheckNewMessageComes(fanpage);
        }

        /// <summary>
        /// Confirm read new message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            wasNewMessageConfirmed = true;
            await ConversationsInfo(false);
        }

        /// <summary>
        /// Show info, is new message comes (in InfoLabel), if theres no new messages, hide window
        /// </summary>
        /// <param name="isNewMessage"></param>
        private async Task ConversationsInfo(bool isNewMessage)
        {
            if (isNewMessage)
            {
                InfoLabel.Content = "NOWA WIADOMOSC";
                ShowWindow();
            }
            else
            {
                InfoLabel.Content = "Brak nowych wiadomosci";
                await HideWindow();
            }
        }

        private async Task HideWindow()
        {
            await Task.Delay(2000);
            this.Hide();
        }

        private void ShowWindow()
        {
            this.Show();
        }
        

        /// <summary>
        /// Test async code
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void testBtn_Click(object sender, RoutedEventArgs e)
        {
            testLbl.Content += "...";
        }

    }
}
