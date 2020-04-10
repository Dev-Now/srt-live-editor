using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TarjamatSRTEditor.SubtitlingModels;
using System.Windows.Threading;
using VideoControlsViewModel;

namespace SubtitlingViewModel
{
    public class SubtitlingVM: INotifyPropertyChanged
    {
        SubtitlingModel _subm;
        VideoCtrlsVM _vidvm;

        public SubtitlingVM(VideoCtrlsVM _vcvm, string _szOrigSrtPath, string _szOutpSrtPath)
        {
            _vidvm = _vcvm;
            _subm = new SubtitlingModel(_szOrigSrtPath, _szOutpSrtPath);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string szPropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(szPropertyName));
        }

        #region Subtitle Label
        private string _szCurrentSubtitle = "";
        private float _fLineHeight = 41f;
        private float _fLineWidth = 7f;
        public string SubtitleLblContent
        {
            get
            {
                TimeSpan tsVidCurrPos = _vidvm.GetCurrentVideoPosition();
                _szCurrentSubtitle = _subm.GetSubtitleAt(tsVidCurrPos);
                return _szCurrentSubtitle;
            }
        }

        public string SubtitleLblVisible
        {
            get
            {
                return _szCurrentSubtitle != ""? "Visible" : "Hidden";
            }
        }

        public uint SubtitleLblHeight
        {
            get
            {
                return Convert.ToUInt32(_szCurrentSubtitle.Split('\n').Count() * _fLineHeight);
            }
        }

        public uint SubtitleLblWidth
        {
            get
            {
                return Math.Max(Convert.ToUInt32(_szCurrentSubtitle.Split('\n').Max(line => line.Length) * _fLineWidth), 50);
            }
        }
        #endregion

        public void InitSubtitling()
        {
            // timer
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            NotifyPropertyChanged("SubtitleLblContent");
            NotifyPropertyChanged("SubtitleLblVisible");
            NotifyPropertyChanged("SubtitleLblHeight");
            NotifyPropertyChanged("SubtitleLblWidth");
        }
    }
}
