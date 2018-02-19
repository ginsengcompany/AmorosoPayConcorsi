
using FormsVideoLibrary;
using Plugin.MediaManager;
using Plugin.MediaManager.Abstractions;
using Plugin.MediaManager.Abstractions.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoPlayerDemos;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QuizAmoroso
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VideoLezioni : ContentPage
    {
        public VideoLezioni(string urlVideo)
        {
            InitializeComponent();
            videoView.Source = VideoSource.FromUri(urlVideo);

        }
    }
}
            
            
