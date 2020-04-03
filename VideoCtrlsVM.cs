using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Input;
using System.ComponentModel;

namespace VideoControlsViewModel
{
    public class DataReqEventArgs : EventArgs
    {
        public Uri Source;
        public bool HasDuration { get; set; }
        public TimeSpan Duration { get; set; }
        public TimeSpan Position { get; set; }
    }

    public class PlayPauseCommand : ICommand
    {
        private VideoCtrlsVM obj;
        public PlayPauseCommand(VideoCtrlsVM _obj)
        {
            obj = _obj;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            obj.Play_Pause_Video();
        }
    }

    public class ResetCommand : ICommand
    {
        private VideoCtrlsVM obj;
        public ResetCommand(VideoCtrlsVM _obj)
        {
            obj = _obj;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            obj.Reset_Video();
        }
    }

    // This is the View Model Class
    public class VideoCtrlsVM: INotifyPropertyChanged
    {
        public VideoCtrlsVM(string _szVidPath)
        {
            VideoSource = File.Exists(_szVidPath)? _szVidPath: null;
            objPlayPauseCmd = new PlayPauseCommand(this);
            objResetCmd = new ResetCommand(this);
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string szPropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(szPropertyName));
        }

        #region Commands
        private PlayPauseCommand objPlayPauseCmd;
        private ResetCommand objResetCmd;
        public ICommand PlayPauseBtnClick
        {
            get { return objPlayPauseCmd; }
        }
        public ICommand ResetBtnClick
        {
            get { return objResetCmd; }
        }
        #endregion

        #region Control Requests
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
        public string VideoSource { get; }
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
            set 
            { 
                isPlaying = value == "Pause";
                NotifyPropertyChanged("PlayCtrlContent");
            }
        }

        public void Play_Pause_Video()
        {
            DataReqEventArgs eData = new DataReqEventArgs();
            if (PlayCtrlContent == "Play")
            {
                PlayRequest?.Invoke(this, eData);
                PlayCtrlContent = "Pause";
            }
            else
            {
                PauseRequest?.Invoke(this, eData);
                PlayCtrlContent = "Play";
            }
        }
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

        public void Reset_Video()
        {
            StopRequest?.Invoke(this, new DataReqEventArgs());
            PlayCtrlContent = "Play";
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
                if (!bUpdateVidPos) { return dSaveVidPos; }
                DataReqEventArgs eData = new DataReqEventArgs();
                GetPosition?.Invoke(this, eData);
                return eData.Position.TotalMilliseconds; 
            }
            set {
                DataReqEventArgs eData = new DataReqEventArgs();
                eData.Position = new TimeSpan(0, 0, 0, 0, Convert.ToInt32(value));
                if (!bUpdateVidPos) { dSaveVidPos = eData.Position.TotalMilliseconds; }
                else { SetPosition?.Invoke(this, eData); }
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

        public void Load_Video()
        {
            // a request to play is used to make the video load, (couldn't find better...)
            // then upon MediaOpened event, request a Stop!
            PlayRequest?.Invoke(this, new DataReqEventArgs());
        }

        public void InitVideoCtrls()
        {
            DataReqEventArgs eData = new DataReqEventArgs();
            GetSource?.Invoke(this, eData);
            if (eData.Source != null)
            {
                // timer
                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(10);
                timer.Tick += Timer_Tick;
                timer.Start();
            }           
            NotifyPropertyChanged("VideoSpanLblContent");
            NotifyPropertyChanged("VideoPositionSldrMax");
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (bUpdateVidPos)
            {
                NotifyPropertyChanged("VideoPositionLblContent");
                NotifyPropertyChanged("VideoPositionSldrValue");
            }
        }

        private bool bUpdateVidPos = true;
        private double dSaveVidPos = 0.0;
        public void Toggle_Video_Position_Update()
        {
            bUpdateVidPos = !bUpdateVidPos;
            if (bUpdateVidPos)
            {
                DataReqEventArgs eData = new DataReqEventArgs();
                eData.Position = new TimeSpan(0, 0, 0, 0, Convert.ToInt32(dSaveVidPos));
                SetPosition?.Invoke(this, eData);
            }
        }
    }
}
