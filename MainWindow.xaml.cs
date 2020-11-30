using System;
using System.Collections.Generic;
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
using System.Threading;

namespace MatchGame
{
    using System.Windows.Threading;
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        int tenthOfSecondElapsed;
        int matchesFound;

        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;
            SetUpGame();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            tenthOfSecondElapsed++;
            timeTextBlock.Text = (tenthOfSecondElapsed / 10F).ToString("0.0s");
            if (matchesFound == 8)
            {
                timer.Stop();
                timeTextBlock.Text = timeTextBlock.Text + " Play again?";
            }
        }

        List<string> animalRandomEmoji = new List<string>();
            
        private void SetUpGame()
        {
            List<string> animalEmoji = new List<string>()
            {
                "🐙","🐙",
                "🐘","🐘",
                "🐪","🐪",
                "🐧","🐧",
                "🦁","🦁",
                "🦊","🦊",
                "🐹","🐹",
                "🐷","🐷",
            };
            animalRandomEmoji.Clear();
            Random random = new Random();
            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                if (animalEmoji.Count > 0) { //if (textBlock.Name != "timeTextBlock") 
                    int index = random.Next(animalEmoji.Count);
                    string nextEmoji = animalEmoji[index];
                    animalRandomEmoji.Add(nextEmoji);
                    textBlock.Text = "?";
                    animalEmoji.RemoveAt(index);
                }
            }
            Dispatcher.Invoke(new Action(() => { }), DispatcherPriority.ContextIdle);
            timer.Start();
            tenthOfSecondElapsed = 0;
            matchesFound = 0;
        }
        //global variables
        TextBlock lastTextBlockClicked;
        bool findingMatch = false;
        string lastEmjoiClicked;
       
            
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {

            TextBlock textBlock = sender as TextBlock;            
            
            int textBlockNumber = 0;
            if (textBlock.Name.Length == 2 )
            {
                textBlockNumber = Convert.ToInt32(textBlock.Name[1].ToString());
            }
            else { textBlockNumber = Convert.ToInt32(textBlock.Name.Substring(1)); }

            if (findingMatch == false)
            {                
                //textBlock.Visibility = Visibility.Hidden;
                textBlock.Text = animalRandomEmoji[textBlockNumber];
                lastTextBlockClicked = textBlock;
                findingMatch = true;
                lastEmjoiClicked = animalRandomEmoji[textBlockNumber];
            }
            else if (animalRandomEmoji[textBlockNumber] == lastEmjoiClicked && textBlock != lastTextBlockClicked)
            {
                textBlock.Text = animalRandomEmoji[textBlockNumber];
                Dispatcher.Invoke(new Action(() => { }), DispatcherPriority.ContextIdle);
                IsHitTestVisible = false;
                Thread.Sleep(1000);
                IsHitTestVisible = true;
                lastTextBlockClicked.Visibility = Visibility.Hidden;
                textBlock.Visibility = Visibility.Hidden;
                findingMatch = false;
                matchesFound++;
            }
            else if (textBlock != lastTextBlockClicked)
            {
                textBlock.Text = animalRandomEmoji[textBlockNumber];
                //Updates UI while rest of the code is in pause
                Dispatcher.Invoke(new Action(() => { }), DispatcherPriority.ContextIdle);
                IsHitTestVisible = false;
                Thread.Sleep(1000);
                IsHitTestVisible = true;
                textBlock.Visibility = Visibility.Visible;
                textBlock.Text = "?";
                lastTextBlockClicked.Visibility = Visibility.Visible;
                lastTextBlockClicked.Text = "?";
                findingMatch = false;
            }
        }

        private void timeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (matchesFound == 8)
            {
                SetUpGame();
            }
        }
    }
}
