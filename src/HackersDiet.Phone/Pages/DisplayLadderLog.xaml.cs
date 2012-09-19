using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Parse;
using HackersDiet.Phone.Models;

namespace HackersDiet.Phone.Pages
{
    public partial class DisplayLadderLog : PhoneApplicationPage
    {
        private Driver parse = new Driver();

        public DisplayLadderLog()
        {
            InitializeComponent();

            CreateApplicationBar();
        }

        private void CreateApplicationBar()
        {
            ApplicationBar = new ApplicationBar();

            ApplicationBar.Mode = ApplicationBarMode.Default;
            ApplicationBar.Opacity = 1.0;
            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = true;

            ApplicationBarIconButton homeButton = new ApplicationBarIconButton();
            homeButton.IconUri = new Uri("/Images/appbar.back.rest.png", UriKind.Relative);
            homeButton.Text = "home";
            ApplicationBar.Buttons.Add(homeButton);

            homeButton.Click += new EventHandler(backButton_Click);
        }

        void backButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            parse.Objects.Query<LadderModel>().Execute(r =>
            {
                if (r.Success)
                {
                    var ladders = r.Data.Results;

                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        this.ListLadders.ItemsSource = ladders;
                    });
                }
                else
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show(r.Error.Message);
                    });
                }
            });
        }

        private void ListLadders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = (sender as ListBox).SelectedIndex;

            LadderModel selectedLadder = ((sender as ListBox).SelectedItem as LadderModel);

            if (index >= 0)
            {
                NavigationService.Navigate(new Uri("/Pages/RecordLadderLog.xaml?id=" + selectedLadder.Id, UriKind.Relative));
            }
        }
    }
}