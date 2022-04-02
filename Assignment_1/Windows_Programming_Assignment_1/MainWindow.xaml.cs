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
using System.Windows.Threading;

namespace Windows_Programming_Assignment_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MediaProgress.Minimum = 0;
            MediaProgress.Maximum = 0;
        }
        // When program is started, a new Media object is instantiated.
        Media newMedia = new Media();
        bool isDragging;
        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            // The user is prompted to select an mp3 media source
            if (newMedia.openFileDialog.ShowDialog() == true)
            {
                // The media source is set
                newMedia.mediaPlayer.Source = new Uri(newMedia.openFileDialog.FileName);
                Media_MenuItem.IsEnabled = true;

                // A new TagLib File object is created
                var mediaData = TagLib.File.Create(newMedia.openFileDialog.FileName);

                // The maximum value for the slider is set
                MediaProgress.Maximum = mediaData.Properties.Duration.TotalSeconds;

                // The mp3 starts to play
                newMedia.mediaPlayer.Play();

                // A timer is instantiated and is subscribed to the Tick event
                DispatcherTimer timer = new DispatcherTimer();
                timer.Tick += Timer_Tick;
                timer.Start();

                // The total duration of the mp3 is shown as a label
                lblTotalDuration.Text = TimeSpan.FromSeconds(mediaData.Properties.Duration.TotalSeconds).ToString(@"hh\:mm\:ss");
            }
        }

        private void MediaSelected_Check(object sender, CanExecuteRoutedEventArgs e)
        {
            // If there is a valid mp3 selected, the user can Play, Pause, and Stop
            if(newMedia.mediaPlayer.Source != null)
            {
                e.CanExecute = true;
            }
        }

        private void Pause_Click(object sender, ExecutedRoutedEventArgs e)
        {
            newMedia.mediaPlayer.Pause();
        }

        private void Play_Click(object sender, ExecutedRoutedEventArgs e)
        {
            newMedia.mediaPlayer.Play();
        }

        private void Stop_Click(object sender, ExecutedRoutedEventArgs e)
        {
            newMedia.mediaPlayer.Stop();
        }

        private void MediaProgressStart_Drag(object sender, RoutedEventArgs e)
        {
            // If the user moves the slider, the isDragging flag is set to true,
            // and the timer stops updating the slider
            isDragging = true;
        }

        private void MediaProgressEnd_Drag(object sender, RoutedEventArgs e)
        {
            // When the user lets go of the slider, update the media to jump to the selected timestamp
            isDragging = false;
            newMedia.mediaPlayer.Position = TimeSpan.FromSeconds(MediaProgress.Value);
        }

        private void MediaProgress_Value(object sender, RoutedEventArgs e)
        {
            // When the slider value is changed, update the elapsed seconds label.
            lblProgressStatus.Text = TimeSpan.FromSeconds(MediaProgress.Value).ToString(@"hh\:mm\:ss");
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // If there is a media source and the user is not dragging the slider,
            // set the slider value equal to the elapsed seconds of the mp3
            if (newMedia.mediaPlayer.Source != null && isDragging == false)
            {
                MediaProgress.Value = newMedia.mediaPlayer.Position.TotalSeconds;
            }
        }
    }
}
