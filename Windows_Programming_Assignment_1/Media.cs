using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Controls;
using Microsoft.Win32;
using System.Windows.Threading;

namespace Windows_Programming_Assignment_1
{
    class Media
    {
        public MediaElement mediaPlayer { get; set; }
        public OpenFileDialog openFileDialog { get; set; }
        // Media constructor instantiates and sets the properties for MediaElement and OpenFileDialog.
        public Media()
        {
            this.mediaPlayer = new MediaElement();
            this.mediaPlayer.LoadedBehavior = MediaState.Manual;
            this.mediaPlayer.UnloadedBehavior = MediaState.Manual;

            this.openFileDialog = new OpenFileDialog();
            this.openFileDialog.Filter = "MP3 files (*.mp3)|*.mp3";
        }
    }
}
