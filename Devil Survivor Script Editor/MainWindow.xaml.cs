using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using System.ComponentModel;
using System.Media;

namespace Devil_Survivor_Script_Editor
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static BackgroundWorker generateOutputBGW = new BackgroundWorker();

        public MainWindow()
        {

            //CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
            //CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");

            //CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("es-ES");
            //CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("es-ES");

            InitializeComponent();
            if (!Directory.Exists("./DSSE-files")) Directory.CreateDirectory("./DSSE-files");
            if (!Directory.Exists("./DSSE-files/translation")) Directory.CreateDirectory("./DSSE-files/translation");
            if (!Directory.Exists("./DSSE-files/output")) Directory.CreateDirectory("./DSSE-files/output");
            originalTextBox.Text = Properties.Resources.openAny;
        
            generateOutputBGW.DoWork += new DoWorkEventHandler(generateOutputBGW_DoWork);
            generateOutputBGW.ProgressChanged += new ProgressChangedEventHandler(generateOutputBGW_ProgressChanged);
            generateOutputBGW.RunWorkerCompleted += new RunWorkerCompletedEventHandler(generateOutputBGW_RunWorkerCompleted);
            generateOutputBGW.WorkerReportsProgress = true;

        }

        private void OpenCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void NextCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void PrevCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void GotoCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void GotoCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            currentNumberBox.SelectAll();
            currentNumberBox.Focus();
        }

        private void CopyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CopyAllCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        List<string> archivosRuta = new List<string>();
        List<string> archivosNombres = new List<string>();
        public string folderResult;
        private void openButton_Click(object sender, RoutedEventArgs e)
        {
            var openFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            openFolderDialog.Description = Properties.Resources.fileOpenDesc;
            if (openFolderDialog.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
            {
                folderResult = openFolderDialog.SelectedPath + "/";
                if (File.Exists(folderResult + "b_0000.cmp"))
                {
                    archivosRuta.Clear();
                    archivosRuta.AddRange(Directory.GetFiles(folderResult, "*.cmp"));

                    archivosNombres.Clear();
                    for (int i = 0; i < archivosRuta.Count; i++)
                    {
                        archivosNombres.Add(Path.GetFileNameWithoutExtension(archivosRuta[i]));
                        //Console.WriteLine(archivosRuta[i]);
                    }
                    archivosListBox.ItemsSource = archivosNombres;
                }
                else MessageBox.Show(Properties.Resources.fileOpenErrText, Properties.Resources.fileOpenErrTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        List<string> lineasTexto = new List<string>();
        public string archivoPrevio = "";
        public int maxLineasTexto = 0;
        public int currentText = 0;

        private void archivosListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (archivoPrevio != "")
            {
                saving.saveSegment(archivoPrevio, currentText, maxLineasTexto, lineasTexto, traduccionTextBox.Text);
                memory.saveToMemory(originalTextBox.Text, traduccionTextBox.Text);
            }

            archivoPrevio = archivosNombres[archivosListBox.SelectedIndex];
            string textoConvertido = leerArchivo.leer(archivosRuta[archivosListBox.SelectedIndex]);

            string[] result = Regex.Split(textoConvertido, @"\[00\]\n|(?=\[new\])");
            lineasTexto.Clear();
            foreach (string s in result)
            {
                lineasTexto.Add(s);
            }

            lineasTexto.RemoveAll(item => item == "");

            originalTextBox.Text = lineasTexto[0];

            maxLineasTexto = lineasTexto.Count()-1;
            currentNumberBox.Text = "0";
            textNumber.Content = " / " + maxLineasTexto;

            buttonSiguienteTexto.IsEnabled = true;
            buttonCopiarTexto.IsEnabled = true;
            buttonCopyAll.IsEnabled = true;
            buttonOpeninNote.IsEnabled = true;
            currentText = 0;

            currentNumberBox.IsEnabled = true;

            loadTranslatedText();
        }


        private void buttonSiguienteTexto_Click(object sender, RoutedEventArgs e)
        {

            if (buttonSiguienteTexto.IsEnabled)
            {
                saving.saveSegment(archivoPrevio, currentText, maxLineasTexto, lineasTexto, traduccionTextBox.Text);
                memory.saveToMemory(originalTextBox.Text, traduccionTextBox.Text);
            }
            if (currentText != maxLineasTexto)
            {
                currentText++;
                currentNumberBox.Text = currentText.ToString();
                originalTextBox.Text = lineasTexto[currentText];
                textNumber.Content = " / " + maxLineasTexto;
            }

            if (currentText == maxLineasTexto) buttonSiguienteTexto.IsEnabled = false;

            if (currentText != 0) buttonAnteriorTexto.IsEnabled = true;

            loadTranslatedText();

        }

        private void buttonAnteriorTexto_Click(object sender, RoutedEventArgs e)
        {
            if (buttonAnteriorTexto.IsEnabled)
            {
                saving.saveSegment(archivoPrevio, currentText, maxLineasTexto, lineasTexto, traduccionTextBox.Text);
                memory.saveToMemory(originalTextBox.Text, traduccionTextBox.Text);
            }

            if (currentText != 0)
            {
                currentText--;
                originalTextBox.Text = lineasTexto[currentText];
                currentNumberBox.Text = currentText.ToString();
                textNumber.Content = " / " + maxLineasTexto;
            }
            else buttonAnteriorTexto.IsEnabled = false;

            if (currentText != maxLineasTexto) buttonSiguienteTexto.IsEnabled = true;
            if (currentText == 0) buttonAnteriorTexto.IsEnabled = false;

            loadTranslatedText();

        }

        private void currentNumberBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if ((currentNumberBox.Text.All(char.IsDigit) == true) && currentNumberBox.Text != "")
            {
                saving.saveSegment(archivoPrevio, currentText, maxLineasTexto, lineasTexto, traduccionTextBox.Text);
                memory.saveToMemory(originalTextBox.Text, traduccionTextBox.Text);

                if (e.Key == Key.Enter)
                {
                    if ((Int32.Parse(currentNumberBox.Text) <= maxLineasTexto) && ((Int32.Parse(currentNumberBox.Text) >= 0)))
                    {
                        currentText = Int32.Parse(currentNumberBox.Text);
                        originalTextBox.Text = lineasTexto[currentText];
                    }
                    if ((currentNumberBox.Text == "0") || (((Int32.Parse(currentNumberBox.Text) > 0)) && (Int32.Parse(currentNumberBox.Text) != maxLineasTexto)))
                    {
                        buttonAnteriorTexto.IsEnabled = false;
                        buttonSiguienteTexto.IsEnabled = true;
                    }
                    else if (Int32.Parse(currentNumberBox.Text) == maxLineasTexto)
                    {
                        buttonSiguienteTexto.IsEnabled = false;
                        buttonAnteriorTexto.IsEnabled = true;
                    }

                    else if ((Int32.Parse(currentNumberBox.Text) >= 0) && (Int32.Parse(currentNumberBox.Text) != maxLineasTexto))
                    {
                        buttonAnteriorTexto.IsEnabled = true;
                    }

                    loadTranslatedText();
                }

            }
            else
            {
                currentNumberBox.Text = string.Empty;
            }
        }

        private void buttonCopiarTexto_Click(object sender, RoutedEventArgs e)
        {
            if ((originalTextBox.Text != "") && (archivoPrevio != "")) traduccionTextBox.Text = originalTextBox.Text;
        }

        private void buttonCopyAll_Click(object sender, RoutedEventArgs e)
        {
            while (currentText < maxLineasTexto)
            {
                
                traduccionTextBox.Text = originalTextBox.Text;
                saving.saveSegment(archivoPrevio, currentText, maxLineasTexto, lineasTexto, traduccionTextBox.Text);
                currentText++;
                originalTextBox.Text = lineasTexto[currentText];
                //Console.WriteLine(currentText + " / " + maxLineasTexto + " " + lineasTexto[currentText]);
            }

            traduccionTextBox.Text = originalTextBox.Text;
            textNumber.Content = " / " + maxLineasTexto;
            currentNumberBox.Text = currentText.ToString();
            if (currentText == maxLineasTexto) buttonSiguienteTexto.IsEnabled = false;
            if (currentText != 0) buttonAnteriorTexto.IsEnabled = true;
        }


        private void traduccionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (statusText.Text != "") { statusText.Text = ""; iconStatus.Source = null; }

            try {
                tagParser.tagParse(traduccionTextBox, OutPutTranstextBlock);
            }
            catch {
                Image myImage = new Image();
                BitmapImage myImageSource = new BitmapImage();
                myImageSource.BeginInit();
                myImageSource.UriSource = new Uri("./images/icons/exclamation-red.png",UriKind.Relative);
                myImageSource.EndInit();
                iconStatus.Source = myImageSource;
                string myText = Properties.Resources.syntaxErr;
                Run myTextR = new Run(myText);
                myTextR.Foreground = new SolidColorBrush(Color.FromRgb(145,65,65));
                statusText.Inlines.Add(myTextR);
            };

            string text = traduccionTextBox.Text.Trim();
            int wordCount = 0, index = 0;
            if (traduccionTextBox.Text.Contains("[new]")) wordCount--;
            while (index < text.Length)
            {
                // check if current char is part of a word
                while (index < text.Length && Char.IsWhiteSpace(text[index]) == false)
                    index++;

                wordCount++;

                // skip whitespace until next word
                while (index < text.Length && Char.IsWhiteSpace(text[index]) == true)
                    index++;

                
            }

            wordsTextBlock.Text = Properties.Resources.words + wordCount.ToString() +" | "+ Properties.Resources.lines + traduccionTextBox.LineCount.ToString();
        }

        private void originalTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            tagParser.tagParse(originalTextBox, OutPutOrigintextBlock);

        }

        private void addYellowButton_Click(object sender, RoutedEventArgs e)
        {
            traduccionTextBox.Text = addTag(true) + "[color1]" + traduccionTextBox.SelectedText + "[/color]" + addTag(false);
        }

        private void addYellow2Button_Click(object sender, RoutedEventArgs e)
        {
            traduccionTextBox.Text = addTag(true) + "[color2]" + traduccionTextBox.SelectedText + "[/color]" + addTag(false);
        }

        private string addTag(bool mode)
        {
            int tagStart = traduccionTextBox.SelectionStart;
            int tagEnd = traduccionTextBox.SelectionLength;
            string beforeText = traduccionTextBox.Text.Substring(0, tagStart);
            string afterText = traduccionTextBox.Text.Substring(beforeText.Length + tagEnd);
            return (mode == true) ? beforeText : afterText;
        }

        private void addNewButton_Click(object sender, RoutedEventArgs e)
        {
            traduccionTextBox.Text = traduccionTextBox.Text.Insert(traduccionTextBox.CaretIndex,"[new]");
        }

        private void addInvertedButton_Click(object sender, RoutedEventArgs e)
        {
            traduccionTextBox.Text = addTag(true) + "“" + traduccionTextBox.SelectedText + "“" + addTag(false);
        }

        private void buttonOpeninNote_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists((Environment.CurrentDirectory + "/DSSE-files/translation/" + archivoPrevio + ".xml")))
            System.Diagnostics.Process.Start(Environment.CurrentDirectory + "/DSSE-files/translation/" + archivoPrevio + ".xml");
        }

        private void loadTranslatedText()
        {
            if (stateATFT == true)  showAT(false);
            traduccionTextBox.Text = "";

            if (File.Exists("./DSSE-files/translation/" + archivoPrevio + ".xml"))
            {
                XDocument document = XDocument.Load("./DSSE-files/translation/" + archivoPrevio + ".xml");
                XElement root = document.Root;

                traduccionTextBox.Text = root.Elements("Segment").Where(f =>
                f.Attribute("ID").Value.Equals(currentText.ToString())).Select(f =>
                f.Element("Translated")).Single().Value;

                if (originalTextBox.Text.Contains("[new]") && (traduccionTextBox.Text == "")) traduccionTextBox.AppendText("[new]");

                if ((File.Exists("./DSSE-files/translation-memory.tmx")) && ((traduccionTextBox.Text == "") || (traduccionTextBox.Text == "[new]")))
                {
                    XDocument document2 = XDocument.Load("./DSSE-files/translation-memory.tmx");
                    XElement root2 = document2.Root;

                    var memTranslate = from e in root2.Descendants("tuv")
                                       where (string)e.Attribute("lang") == "en"
                                       where (string)e.Element("seg") == originalTextBox.Text
                                       select e.NextNode;

                    //if (memTranslate != null)
                    if (memTranslate.Any())
                    {
                        //Console.WriteLine("NOT NULL");
                        foreach (XElement e in memTranslate)
                        {
                            if ((e.Element("seg").Value != "") && (e.Element("seg").Value != "[new]"))
                            {
                                //Console.WriteLine("a" + e.Element("seg").Value + "b");
                                traduccionTextBox.Text = e.Element("seg").Value;
                                //Console.WriteLine("AUTO TRANSLATED FROM MEMORY!");
                                showAT(true);
                            }

                            else if (e.Element("seg").Value == "")
                            {
                                int i = 0;
                                List<string> stringList = new List<string>();
                                stringList = (from d in root2.Descendants("tuv")
                                              where (string)d.Attribute("lang") == "en"
                                              select d)
                                             .Elements("seg")
                                             .Select(d => d.Value)
                                             .ToList();

                                while (i != stringList.Count())
                                {
                                    //Console.WriteLine(fuzzy.match(originalTextBox.Text, stringList[i]) + "%");
                                    string fmatch = fuzzy.match(originalTextBox.Text, stringList[i]);
                                    //Console.WriteLine(fmatch);

                                    var fuzzymatch = from d in root2.Descendants("tuv")
                                             where (string)d.Attribute("lang") == "en"
                                             where (string)d.Element("seg") == fmatch
                                             select d.NextNode;
                                    
                                    if (fuzzymatch.Any())
                                    {
                                        foreach(XElement d in fuzzymatch)
                                        {
                                            if ((d.Element("seg").Value != "") && (d.Element("seg").Value != "[new]"))
                                            {
                                                traduccionTextBox.Text = d.Element("seg").Value;
                                                //Console.WriteLine("Fuzzy Match!");
                                                textBlockAT.Text = fuzzy.matchPercent + "%";
                                                showFT(true);
                                            }
                                        }
                                    }
                                    i++;
                                }
                            }
                            
                        }

                    }
                    memTranslate = null;
                }

            }
        }

        private bool stateATFT;

        private void showAT (bool mode) {
            if (mode == true)
            {
                traduccionTextBox.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 102, 255, 34));
                traduccionTextBox.Background = new SolidColorBrush(Color.FromArgb(40, 102, 255, 34));
                textBlockAT.Text = "AT";
                textBlockAT.Background = new SolidColorBrush(Color.FromArgb(255, 102, 255, 34));
                textBlockAT.Visibility = Visibility.Visible;
                stateATFT = true;
            }
            else
            {
                traduccionTextBox.ClearValue(BorderBrushProperty);
                traduccionTextBox.ClearValue(BackgroundProperty);
                textBlockAT.Visibility = Visibility.Hidden;
                stateATFT = false;
            }
        }

        private void showFT(bool mode)
        {
            if (mode == true)
            {
                traduccionTextBox.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 255, 255, 34));
                traduccionTextBox.Background = new SolidColorBrush(Color.FromArgb(40, 255, 255, 34));
                textBlockAT.Background = new SolidColorBrush(Color.FromArgb(255, 203, 203, 37));
                textBlockAT.Visibility = Visibility.Visible;
                stateATFT = true;
            }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (archivosListBox.Items.Count == 0)
                MessageBox.Show(Properties.Resources.fileSaveErrText, Properties.Resources.fileSaveErrTitle, MessageBoxButton.OK, MessageBoxImage.Exclamation);
            else if (!generateOutputBGW.IsBusy)
            {
                saveButton.IsEnabled = false;
                outputProgress.Visibility = Visibility.Visible;
                progressText.Visibility = Visibility.Visible;
                generateOutputBGW.RunWorkerAsync();
            }
        }

        private void generateOutputBGW_DoWork(object sender, DoWorkEventArgs e)
        {
            generateOutput.generate(folderResult);

        }

        private void generateOutputBGW_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            outputProgress.Maximum = generateOutput.fileNumber;
            outputProgress.Value = e.ProgressPercentage;
            //Console.WriteLine("Progreso2:" + (e.ProgressPercentage));

            progressText.Content = String.Format(Properties.Resources.progressText, e.ProgressPercentage, generateOutput.fileNumber);
            //Console.WriteLine(String.Format(Properties.Resources.progressText, e.ProgressPercentage, generateOutput.fileNumber));
        }

        private void generateOutputBGW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressText.Content = Properties.Resources.progressTextFinished;
            SystemSounds.Beep.Play();
            MessageBox.Show(Properties.Resources.outputFinishText, Properties.Resources.outputFinishTitle, MessageBoxButton.OK, MessageBoxImage.Asterisk);
            saveButton.IsEnabled = true;
            outputProgress.Visibility = Visibility.Hidden;
            outputProgress.Value = 0;
            progressText.Visibility = Visibility.Hidden;
            progressText.Content = "";
        }

        public static bool aboutOpen = false;

        private void aboutButton_Click(object sender, RoutedEventArgs e)
        {
            if (!aboutOpen)
            {
                about about = new about();
                about.Owner = this;
                about.Show();
                aboutOpen = true;
            }
        }

    }



    public static class CustomCommands
    {
        public static readonly RoutedUICommand Next = new RoutedUICommand
            (
                        "Next",
                        "Next",
                        typeof(CustomCommands),
                        new InputGestureCollection()
                        {
                                new KeyGesture(Key.F5)
                        }
            );

        public static readonly RoutedUICommand Prev = new RoutedUICommand
            (

                        "Prev",
                        "Prev",
                        typeof(CustomCommands),
                        new InputGestureCollection()
                        {
                             new KeyGesture(Key.F4)
                        }
            );

        public static readonly RoutedUICommand Goto = new RoutedUICommand
            (

                        "Goto",
                        "Goto",
                        typeof(CustomCommands),
                        new InputGestureCollection()
                        {
                                new KeyGesture(Key.G, ModifierKeys.Control)
                        }
            );

        public static readonly RoutedUICommand Copy = new RoutedUICommand
            (

                        "Copy",
                        "Copy",
                        typeof(CustomCommands),
                        new InputGestureCollection()
                        {
                                new KeyGesture(Key.F1)
                        }
            );
        public static readonly RoutedUICommand CopyAll = new RoutedUICommand
            (

                        "CopyAll",
                        "CopyAll",
                        typeof(CustomCommands),
                        new InputGestureCollection()
                        {
                                new KeyGesture(Key.F9)
                        }
            );

        //Define more commands here, just like the one above
    }
}