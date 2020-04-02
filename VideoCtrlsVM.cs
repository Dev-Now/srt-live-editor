using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Input;

namespace VideoControlsViewModel
{
    class VideoCtrlsVM
    {
        private MediaElement vidElt;

        public VideoCtrlsVM(MediaElement _vidElt)
        {
            vidElt = _vidElt;
        }

        //private void Init_Video_Position_Ctrls(object sender, RoutedEventArgs e)
        //{
        //    //VideoPositionLblContent;
        //    //VideoSpanLblContent;
        //    //VideoPositionSldrMax;
        //    VideoPositionSldrValue = 0.0;
        //}

        #region Video Source
        public string VideoSource
        {
            get { return (vidElt.Source!=null)? vidElt.Source.ToString() : "default-file.wav"; }
            set 
            { 
                vidElt.Source = File.Exists(VideoSource) ? new Uri(VideoSource) : null;
                if (vidElt.Source!=null)
                {
                    PlayCtrlContent = "Play";
                    vidElt.Stop();
                    // timer
                    DispatcherTimer timer = new DispatcherTimer();
                    timer.Interval = TimeSpan.FromMilliseconds(10);
                    timer.Tick += Timer_Tick;
                    timer.Start();
                }
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (vidElt.Source != null)
            {
                if (vidElt.NaturalDuration.HasTimeSpan)
                {
                    //string dummy = VideoPositionLblContent;
                    VideoPositionSldrValue = vidElt.Position.TotalMilliseconds;
                    //TODO: notify page to refresh properties
                }
            }
        }
        #endregion

        #region Play Control
        private bool IsPlaying { get; set; }

        public bool PlayCtrlEnabled
        {
            get { return vidElt.Source != null; }
        }

        public string PlayCtrlContent
        {
            get { return IsPlaying ? "Pause" : "Play"; }
            set { IsPlaying = PlayCtrlContent == "Pause"; }
        }

        private void Play_Pause_Video()
        {
            if (PlayCtrlContent == "Play")
            {
                vidElt.Play();
            }
            else
            {
                vidElt.Pause();
            }
        }
        #endregion

        #region Reset Control
        public bool ResetCtrlEnabled
        {
            get { return vidElt.Source != null; }
        }
        #endregion

        #region Slider Control
        public bool VideoPositionSldrEnabled
        {
            get { return vidElt.Source != null; }
        }

        public double VideoPositionSldrMax
        {
            get 
            {
                if (vidElt.Source != null)
                {
                    if (vidElt.NaturalDuration.HasTimeSpan)
                    {
                        return vidElt.NaturalDuration.TimeSpan.TotalMilliseconds;
                    }
                }
                return 0.0;
            }
        }

        public double VideoPositionSldrValue
        {
            get { return vidElt.Position.TotalMilliseconds; }
            set { vidElt.Position = new TimeSpan(0, 0, 0, 0, Convert.ToInt32(VideoPositionSldrValue)); }
        }
        #endregion

        #region Position Label
        private string vidPositionLbl = "00:00.000";
        
        public string VideoPositionLblContent
        {
            get
            {
                if (vidElt.Source != null)
                {
                    if (vidElt.NaturalDuration.HasTimeSpan)
                    {
                        vidPositionLbl = vidElt.Position.ToString(@"mm\:ss\.fff");
                        return vidPositionLbl;
                    }
                }
                return vidPositionLbl;
            }
        }
        #endregion

        #region Span Label
        private string vidSpanLbl = "00:00.000";

        public string VideoSpanLblContent
        {
            get
            {
                if (vidElt.Source != null)
                {
                    if (vidElt.NaturalDuration.HasTimeSpan)
                    {
                        vidSpanLbl = vidElt.NaturalDuration.TimeSpan.ToString(@"mm\:ss\.fff");
                        return vidSpanLbl;
                    }
                }
                return vidSpanLbl;
            }
        }
        #endregion
    }
}
