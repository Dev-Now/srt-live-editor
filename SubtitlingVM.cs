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
        public string SubtitleLblContent
        {
            get
            {
                TimeSpan tsVidCurrPos = _vidvm.GetCurrentVideoPosition();
                return _subm.GetSubtitleAt(tsVidCurrPos);
            }
        }

        public string SubtitleLblVisible
        {
            get
            {
                TimeSpan tsVidCurrPos = _vidvm.GetCurrentVideoPosition();
                return _subm.IsThereSubtitleAt(tsVidCurrPos)? "Visible" : "Hidden";
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
        }
    }
}
