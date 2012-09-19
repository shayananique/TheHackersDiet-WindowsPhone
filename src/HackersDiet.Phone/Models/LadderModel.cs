using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using Parse;

namespace HackersDiet.Phone.Models
{
    public class LadderModel : ParseObject
    {
        public string DateExercised { get; set; }
        public string Rung { get; set; }
        public string Bend { get; set; }
        public string SitUp { get; set; }
        public string LegLift { get; set; }
        public string PushUp { get; set; }
        public string Step { get; set; }
        public string Count { get; set; }
        public string Remainder { get; set; }
    }
}
