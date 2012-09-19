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
        private LadderModel updatedLadder;
        private bool loaded = false;

        private bool IsUpdate = false;
        private string LadderId = "";

        public RecordLadderLog()
        {
            InitializeComponent();

            app = (App)Application.Current;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            NavigationContext.QueryString.TryGetValue("id", out LadderId);

            if (!string.IsNullOrEmpty(LadderId))
                this.IsUpdate = true;
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

            if (this.IsUpdate)
            {
                ApplicationBarIconButton deleteButton = new ApplicationBarIconButton();
                deleteButton.IconUri = new Uri("/Images/appbar.delete.rest.png", UriKind.Relative);
                deleteButton.Text = "delete";
                ApplicationBar.Buttons.Add(deleteButton);

                deleteButton.Click += new EventHandler(deleteButton_Click);
            }

            ApplicationBarIconButton cancelButton = new ApplicationBarIconButton();
            cancelButton.IconUri = new Uri("/Images/appbar.cancel.rest.png", UriKind.Relative);
            cancelButton.Text = "cancel";
            ApplicationBar.Buttons.Add(cancelButton);

            cancelButton.Click += new EventHandler(cancelButton_Click);
        }

        void deleteButton_Click(object sender, EventArgs e)
        {
            parse.Objects.Get<LadderModel>(LadderId, r =>
                {
                    if (r.Success)
                    {
                        var ladder = r.Data;
                        parse.Objects.Delete(ladder, t =>
                        {
                            if (t.Success)
                            {
                                Deployment.Current.Dispatcher.BeginInvoke(() =>
                                {
                                    MessageBox.Show("Deleted.");

                                    NavigationService.Navigate(new Uri("/Pages/DisplayLadderLog.xaml", UriKind.Relative));
                                });
                            }
                            else
                            {
                                Deployment.Current.Dispatcher.BeginInvoke(() =>
                                {
                                    MessageBox.Show(t.Error.Message);
                                });
                            }
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

        void cancelButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        void saveButton_Click(object sender, EventArgs e)
        {
            LadderModel ladderModel;

            // update model
            if (this.IsUpdate)
            {
                ladderModel = updatedLadder;
            }
            else
            {
                ladderModel = dailyLadder;
            }

            ladderModel.Rung = ((RungModel)this.rungsListPicker.SelectedItem).Rung;
            ladderModel.DateExercised = ((DateTime)this.datePicker.Value).ToString("d");
            ladderModel.Bend = this.BendTextBox.Text;
            ladderModel.SitUp = this.SitUpTextBox.Text;
            ladderModel.LegLift = this.LegLiftTextBox.Text;
            ladderModel.PushUp = this.PushUpTextBox.Text;
            ladderModel.Step = this.StepsTextBox.Text;
            ladderModel.Count = this.CountTextBox.Text;
            ladderModel.Remainder = this.RemainderTextBox.Text;

            //insert or update
            if (this.IsUpdate)
            {
                parse.Objects.Update(ladderModel.Id, ladderModel, r =>
                {
                    if (r.Success)
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            MessageBox.Show("Updated.");

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
            else
            {
                parse.Objects.Save(ladderModel, r =>
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
            if (!loaded)
            {
                loaded = true;

                CreateApplicationBar();

                this.rungsListPicker.ItemsSource = app.LadderInfo.Rungs;

                if (this.IsUpdate)
                {
                    parse.Objects.Get<LadderModel>(LadderId, r =>
                    {
                        if (r.Success)
                        {
                            updatedLadder = r.Data;

                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                            {
                                this.rungsListPicker.SelectedIndex = Convert.ToInt32(updatedLadder.Rung);

                                this.datePicker.Value = Convert.ToDateTime(updatedLadder.DateExercised);

                                this.BendTextBox.Text = updatedLadder.Bend;
                                this.SitUpTextBox.Text = updatedLadder.SitUp;
                                this.LegLiftTextBox.Text = updatedLadder.LegLift;
                                this.PushUpTextBox.Text = updatedLadder.PushUp;
                                this.StepsTextBox.Text = updatedLadder.Step;
                                this.CountTextBox.Text = updatedLadder.Count;
                                this.RemainderTextBox.Text = updatedLadder.Remainder;

                                RemoveTextBoxHints();
                            });
                        }
                    });
                }
                else
                {
                    this.datePicker.Value = DateTime.Now;
                }
            }
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
                    if (!this.IsUpdate)
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

                        RemoveTextBoxHints();
                    }
                }
            }
        }

        private void RemoveTextBoxHints()
        {
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