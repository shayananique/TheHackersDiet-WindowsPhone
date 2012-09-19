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
using HackersDiet.Phone.Models;
using Microsoft.Phone.Shell;
using Parse;

namespace HackersDiet.Phone.Pages
{
    public partial class RecordLadderLog : PhoneApplicationPage
    {
        private App app;
        private Driver parse = new Driver();
        private LadderModel dailyLadder;

        public RecordLadderLog()
        {
            InitializeComponent();

            CreateApplicationBar();

            app = (App)Application.Current;

            app.LadderInfo.Rungs.Insert(0, new RungModel() { Rung = "Select a Rung" });
            this.rungsListPicker.ItemsSource = app.LadderInfo.Rungs;

            this.datePicker.Value = DateTime.Now;
        }

        private void CreateApplicationBar()
        {
            ApplicationBar = new ApplicationBar();

            ApplicationBar.Mode = ApplicationBarMode.Default;
            ApplicationBar.Opacity = 1.0;
            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = true;

            ApplicationBarIconButton saveButton = new ApplicationBarIconButton();
            saveButton.IconUri = new Uri("/Images/appbar.save.rest.png", UriKind.Relative);
            saveButton.Text = "save";
            ApplicationBar.Buttons.Add(saveButton);

            saveButton.Click += new EventHandler(saveButton_Click);

            ApplicationBarIconButton cancelButton = new ApplicationBarIconButton();
            cancelButton.IconUri = new Uri("/Images/appbar.cancel.rest.png", UriKind.Relative);
            cancelButton.Text = "cancel";
            ApplicationBar.Buttons.Add(cancelButton);

            cancelButton.Click += new EventHandler(cancelButton_Click);
        }

        void cancelButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        void saveButton_Click(object sender, EventArgs e)
        {
            if (dailyLadder != null)
            {
                parse.Objects.Save(dailyLadder, r =>
                {
                    if (r.Success)
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            MessageBox.Show("Saved.");

                            NavigationService.Navigate(new Uri("/Pages/DisplayLadderLog.xaml", UriKind.Relative));
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
        }

        private void PhoneApplicationPage_OrientationChanged(object sender, OrientationChangedEventArgs e)
        {

        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void rungsListPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.rungsListPicker.SelectedIndex > -1)
            {
                string rung = ((RungModel)this.rungsListPicker.SelectedItem).Rung;
                int rungNumber;

                bool isNum = Int32.TryParse(rung, out rungNumber);

                if (isNum)
                {
                    dailyLadder = (from r in app.LadderInfo.Ladders
                                   where r.Rung == rung
                                   select r).First();

                    dailyLadder.DateExercised = ((DateTime)datePicker.Value).ToString("d");

                    this.BendTextBox.Text = dailyLadder.Bend.ToString();
                    this.SitUpTextBox.Text = dailyLadder.SitUp.ToString();
                    this.LegLiftTextBox.Text = dailyLadder.LegLift.ToString();
                    this.PushUpTextBox.Text = dailyLadder.PushUp.ToString();
                    this.StepsTextBox.Text = dailyLadder.Step.ToString();
                    this.CountTextBox.Text = dailyLadder.Count.ToString();
                    this.RemainderTextBox.Text = dailyLadder.Remainder.ToString();

                    this.BendTextBox.Hint = "";
                    this.SitUpTextBox.Hint = "";
                    this.LegLiftTextBox.Hint = "";
                    this.PushUpTextBox.Hint = "";
                    this.StepsTextBox.Hint = "";
                    this.CountTextBox.Hint = "";
                    this.RemainderTextBox.Hint = "";
                }
            }
        }
    }
}