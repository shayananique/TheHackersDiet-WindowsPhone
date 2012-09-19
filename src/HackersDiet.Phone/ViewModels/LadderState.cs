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
using System.Collections.Generic;
using HackersDiet.Phone.Models;

namespace HackersDiet.Phone.ViewModels
{
    static public class LadderState
    {

       
        public static LadderContext LoadLadderContext()
        {
            LadderContext ladderContext = new LadderContext();

            ladderContext.Ladders = LoadLadders();
            ladderContext.Rungs = LoadRungs(ladderContext.Ladders);

            return ladderContext;
        }

        private static List<RungModel> LoadRungs(List<LadderModel> ladders)
        {
            List<RungModel> rungs = new List<RungModel>();
            foreach (var ladder in ladders)
            {
                rungs.Add(new RungModel() { Rung = ladder.Rung.ToString() });
            }

            return rungs;
        }

        private static List<LadderModel> LoadLadders()
        {
            List<LadderModel> ladders = new List<LadderModel>();
            
            for (int i = 0; i < 48; i++)
            {
                LadderModel ladder = new LadderModel();
                ladder.Rung = Constants.Rungs[i].ToString();
                ladder.Bend = Constants.Bends[i].ToString();
                ladder.SitUp = Constants.SitUps[i].ToString();
                ladder.LegLift = Constants.LegLifts[i].ToString();
                ladder.PushUp = Constants.PushUps[i].ToString();
                ladder.Step = Constants.Steps[i].ToString();
                ladder.Count = Constants.Counts[i].ToString();
                ladder.Remainder = Constants.Remainders[i].ToString();

                ladders.Add(ladder);
            }

            return ladders;
        }
    }
}
