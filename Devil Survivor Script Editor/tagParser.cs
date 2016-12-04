using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Devil_Survivor_Script_Editor
{
    class tagParser
    {
        public static void tagParse (TextBox Box1, TextBlock Box2)
        {

            if (((Box1.Text.Contains("[color9]")) || (Box1.Text.Contains("[color2]")) || (Box1.Text.Contains("[color3]"))  || (Box1.Text.Contains("[color5]"))) && (Box1.Text.Contains("[/color]")))
            {

                List<int> tagStart = new List<int>();
                List<int> tagEnd = new List<int>();
                List<int> tagLength = new List<int>();
                List<string> yellowText = new List<string>();
                List<Run> myYellowRun = new List<Run>();
                List<string> beforeText = new List<string>();
                List<string> afterText = new List<string>();
                int distanceText;

                Box2.Inlines.Clear();
                int tagsCount = Regex.Matches(Box1.Text, @"\[\/color\]").Count;
                MatchCollection tagsFound1 = Regex.Matches(Box1.Text, @"\[color.\]");
                MatchCollection tagsFound2 = Regex.Matches(Box1.Text, @"\[\/color\]");

                for (int i = 0; i < tagsCount; i++)
                {
                    for (int j = 0; j < tagsCount; j++)
                    {
                        tagStart.Add(tagsFound1[j].Groups[0].Index + 8);
                        tagEnd.Add(tagsFound2[j].Groups[0].Index);
                        tagLength.Add(tagEnd[j] - tagStart[j]);
                    }

                    //Console.WriteLine("Nº: " + i + "\nIndexStart: " + tagStart[i] + "\nIndexEnd: " + tagEnd[i] + "\nLength: " + tagLength[i]);

                    yellowText.Add(Box1.Text.Substring(tagStart[i], tagLength[i]));
                    myYellowRun.Add(new Run(yellowText[i].ToString()));

                    if (tagsFound1[i].Groups[0].Value == "[color9]") myYellowRun[i].Foreground = new SolidColorBrush(Color.FromRgb(219, 196, 104));
                    else if (tagsFound1[i].Groups[0].Value == "[color2]") myYellowRun[i].Foreground = new SolidColorBrush(Color.FromRgb(140, 140, 189));
                    else if (tagsFound1[i].Groups[0].Value == "[color3]") myYellowRun[i].Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                    else if (tagsFound1[i].Groups[0].Value == "[color5]") myYellowRun[i].Foreground = new SolidColorBrush(Color.FromRgb(255, 153, 255));

                    if ((i == 0) && (tagsCount == 1)) // Solo hay un tag
                    {
                        beforeText.Add(Box1.Text.Substring(0, tagStart[i]));
                        afterText.Add(Box1.Text.Substring(tagEnd[i]));
                        //Console.WriteLine("this0");
                    }
                    else if ((i == 0) && (tagsCount > 1) && (i != tagsCount)) // Primera tag
                    {
                        distanceText = tagStart[i + 1] - tagEnd[i];
                        beforeText.Add(Box1.Text.Substring(0, tagStart[i]));
                        afterText.Add(Box1.Text.Substring(tagEnd[i], distanceText));
                        //Console.WriteLine("this1");
                    }
                    else if ((i != 0) && (i + 1 < tagsCount)) //Tags restantes
                    {
                        distanceText = tagStart[i + 1] - tagEnd[i];
                        afterText.Add(Box1.Text.Substring(tagEnd[i], distanceText));
                        //Console.WriteLine("this2");
                    }
                    else if (i + 1 == tagsCount) //Última tag
                    {
                        afterText.Add(Box1.Text.Substring(tagEnd[i]));
                        //Console.WriteLine("this3");
                    }

                    if (beforeText.ElementAtOrDefault(i) != null) Box2.Inlines.Add(beforeText[i].Replace("[color9]", "").Replace("[/color]", "").Replace("[color2]", "").Replace("[color3]","").Replace("[color5]","").Replace("[new]", ""));
                    Box2.Inlines.Add(myYellowRun[i]);
                    Box2.Inlines.Add(afterText[i].Replace("[color9]", "").Replace("[/color]", "").Replace("[color2]", "").Replace("[color3]","").Replace("[color5]","").Replace("[new]",""));
                }

                beforeText.Clear();
                afterText.Clear();
                yellowText.Clear();
                tagStart.Clear();
                tagEnd.Clear();
                tagLength.Clear();
                myYellowRun.Clear();
            }


            else
            {
                Box2.Inlines.Clear();
                Box2.Inlines.Add(Box1.Text);
                Box2.Text = Box2.Text.Replace("[new]", "");
            }

        }
    }
}
