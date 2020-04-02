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
    class DataReqEventArgs : EventArgs
    {
        public Uri Source;
        public bool HasDuration { get; set; }
        public TimeSpan Duration { get; set; }
        public TimeSpan Position { get; set; }
    }

    // This is the View Model Class
    class VideoCtrlsVM
    {
        #region Control Requests
        public event EventHandler<DataReqEventArgs> SetSource;
        public event EventHandler<DataReqEventArgs> StopRequest;
        public event EventHandler<DataReqEventArgs> PlayRequest;
        public event EventHandler<DataReqEventArgs> PauseRequest;
        public event EventHandler<DataReqEventArgs> SetPosition;
        #endregion
        
        #region Data Requests
        public event EventHandler<DataReqEventArgs> GetSource;
        public event EventHandler<DataReqEventArgs> HasNaturalDuration;
        public event EventHandler<DataReqEventArgs> GetDuration;
        public event EventHandler<DataReqEventArgs> GetPosition;
        #endregion
        
        #region Video Source
        public string VideoSource
        {
            get 
            {
                DataReqEventArgs eData = new DataReqEventArgs();
                GetSource?.Invoke(this, eData);
                return (eData.Source!=null)? eData.Source.ToString() : "invalid-file.wav"; 
            }
            set 
            {
                DataReqEventArgs eData = new DataReqEventArgs();
                eData.Source = File.Exists(VideoSource) ? new Uri(VideoSource) : null;
                SetSource?.Invoke(this, eData);
                if (eData.Source!=null)
                {
                    PlayCtrlContent = "Play";
                    StopRequest?.Invoke(this, eData); //vidElt.Stop();
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
            DataReqEventArgs eData = new DataReqEventArgs();
            GetSource?.Invoke(this, eData);
            if (eData.Source != null)
            {
                HasNaturalDuration?.Invoke(this, eData);
                if (eData.HasDuration)
                {
                    GetPosition?.Invoke(this, eData);
                    //string dummy = VideoPositionLblContent;
                    VideoPositionSldrValue = eData.Position.TotalMilliseconds;
                    //TODO: notify page to refresh properties
                }
            }
        }
        #endregion

        #region Play Control
        private bool isPlaying = false;
        public bool PlayCtrlEnabled
        {
            get 
            {
                DataReqEventArgs eData = new DataReqEventArgs();
                GetSource?.Invoke(this, eData);
                return eData.Source != null;
            }
        }

        public string PlayCtrlContent
        {
            get { return isPlaying ? "Pause" : "Play"; }
            set { isPlaying = PlayCtrlContent == "Pause"; }
        }

        //private void Play_Pause_Video()
        //{
        //    if (PlayCtrlContent == "Play")
        //    {
        //        PlayRequest?.Invoke(this, EventArgs.Empty); //vidElt.Play();
        //    }
        //    else
        //    {
        //        PauseRequest?.Invoke(this, EventArgs.Empty); //vidElt.Pause();
        //    }
        //}
        #endregion

        #region Reset Control
        public bool ResetCtrlEnabled
        {
            get 
            {
                DataReqEventArgs eData = new DataReqEventArgs();
                GetSource?.Invoke(this, eData);
                return eData.Source != null; 
            }
        }
        #endregion

        #region Slider Control
        public bool VideoPositionSldrEnabled
        {
            get 
            {
                DataReqEventArgs eData = new DataReqEventArgs();
                GetSource?.Invoke(this, eData);
                return eData.Source != null; 
            }
        }

        public double VideoPositionSldrMax
        {
            get 
            {
                DataReqEventArgs eData = new DataReqEventArgs();
                GetSource?.Invoke(this, eData);
                if (eData.Source != null)
                {
                    HasNaturalDuration?.Invoke(this, eData);
                    if (eData.HasDuration)
                    {
                        GetDuration?.Invoke(this, eData);
                        return eData.Duration.TotalMilliseconds;
                    }
                }
                return 0.0;
            }
        }

        public double VideoPositionSldrValue
        {
            get 
            {
                DataReqEventArgs eData = new DataReqEventArgs();
                GetPosition?.Invoke(this, eData);
                return eData.Position.TotalMilliseconds; 
            }
            set {
                DataReqEventArgs eData = new DataReqEventArgs();
                eData.Position = new TimeSpan(0, 0, 0, 0, Convert.ToInt32(VideoPositionSldrValue)); 
                SetPosition?.Invoke(this, eData);
            }
        }
        #endregion

        #region Position Label
        private string vidPositionLbl = "00:00.000";
        
        public string VideoPositionLblContent
        {
            get
            {
                DataReqEventArgs eData = new DataReqEventArgs();
                GetSource?.Invoke(this, eData);
                if (eData.Source != null)
                {
                    HasNaturalDuration?.Invoke(this, eData);
                    if (eData.HasDuration)
                    {
                        GetPosition?.Invoke(this, eData);
                        vidPositionLbl = eData.Position.ToString(@"mm\:ss\.fff");
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
                DataReqEventArgs eData = new DataReqEventArgs();
                GetSource?.Invoke(this, eData);
                if (eData.Source != null)
                {
                    HasNaturalDuration?.Invoke(this, eData);
                    if (eData.HasDuration)
                    {
                        GetDuration?.Invoke(this, eData);
                        vidSpanLbl = eData.Duration.ToString(@"mm\:ss\.fff");
                        return vidSpanLbl;
                    }
                }
                return vidSpanLbl;
            }
        }
        #endregion
    }
}
