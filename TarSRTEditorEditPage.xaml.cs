using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VideoControlsViewModel;

namespace TarjamatSRTEditor
{
    /// <summary>
    /// Interaction logic for TarSRTEditorEditPage.xaml
    /// </summary>
    public partial class TarSRTEditorEditPage : Page
    {
        public TarSRTEditorEditPage(string szVidPath, string szSrtPath, string szOutpPath)
        {
            InitializeComponent();

            VideoCtrlsVM vcvm = new VideoCtrlsVM(szVidPath);
            DataContext = vcvm;
            vcvm.StopRequest        += (sender, e) => { VideoEl.Stop(); };
            vcvm.PlayRequest        += (sender, e) => { VideoEl.Play(); };
            vcvm.PauseRequest       += (sender, e) => { VideoEl.Pause(); };
            vcvm.SetPosition        += (sender, e) => { VideoEl.Position = e.Position; };
            vcvm.GetSource          += (sender, e) => { e.Source = VideoEl.Source; };
            vcvm.HasNaturalDuration += (sender, e) => { e.HasDuration = VideoEl.NaturalDuration.HasTimeSpan; };
            vcvm.GetDuration        += (sender, e) => { e.Duration = VideoEl.NaturalDuration.TimeSpan; };
            vcvm.GetPosition        += (sender, e) => { e.Position = VideoEl.Position; };
            vcvm.Reset_Video();

            // configure VideoEl and Subtitles text box
            //if (File.Exists(szVidPath))
            //{
            //    VideoEl.Source = new Uri(szVidPath);
            //    PlayCtrl.Content = "Play";
            //    VideoEl.Stop();
            //    // timer
            //    DispatcherTimer timer = new DispatcherTimer();
            //    timer.Interval = TimeSpan.FromMilliseconds(10);
            //    timer.Tick += Timer_Tick;
            //    timer.Start();
            //}
            //else
            //{
            //    PlayCtrl.IsEnabled = false;
            //    ResetCtrl.IsEnabled = false;
            //    VideoPositionLbl.Content = "00:00.000";
            //    VideoPositionSlider.IsEnabled = false;
            //    VideoSpanLbl.Content = "00:00.000";
            //}
        }

        private void Init_Video_Position_Ctrls(object sender, RoutedEventArgs e)
        {
            VideoCtrlsVM vcvm = (VideoCtrlsVM)DataContext;
            vcvm.InitVideoCtrls();
        }

        //private void Timer_Tick(object sender, EventArgs e)
        //{
        //    if (VideoEl.Source != null)
        //    {
        //        if (VideoEl.NaturalDuration.HasTimeSpan)
        //        {
        //            VideoPositionLbl.Content = VideoEl.Position.ToString(@"mm\:ss\.fff");
        //            VideoPositionSlider.Value = (int)VideoEl.Position.TotalMilliseconds;
        //        }
        //    }
        //}

        //private void Play_Pause_Video(object sender, RoutedEventArgs e)
        //{
        //    if (PlayCtrl.Content.ToString() == "Play")
        //    {
        //        VideoEl.Play();
        //        PlayCtrl.Content = "Pause";
        //    }
        //    else
        //    {
        //        VideoEl.Pause();
        //        PlayCtrl.Content = "Play";
        //    }
        //}

        //private void Reset_Video(object sender, RoutedEventArgs e)
        //{
        //    VideoEl.Stop();
        //    PlayCtrl.Content = "Play";
        //}

        //private void Set_Video_Position(object sender, RoutedEventArgs e)
        //{
        //    Slider sldr = (Slider)sender;
        //    if (sldr.IsFocused || sldr.IsMouseOver)
        //    {
        //        TimeSpan tsNewPos = new TimeSpan(0,0,0,0,(int)((Slider)sender).Value);
        //        VideoEl.Position = tsNewPos;
        //    }
        //}
    }
}
