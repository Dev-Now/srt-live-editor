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
using SubtitlingViewModel;

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

            // video controls view model init
            VideoCtrlsVM vcvm = new VideoCtrlsVM(szVidPath);
            vcvm.StopRequest        += (sender, e) => { VideoEl.Stop(); };
            vcvm.PlayRequest        += (sender, e) => { VideoEl.Play(); };
            vcvm.PauseRequest       += (sender, e) => { VideoEl.Pause(); };
            vcvm.SetPosition        += (sender, e) => { VideoEl.Position = e.Position; };
            vcvm.GetSource          += (sender, e) => { e.Source = VideoEl.Source; };
            vcvm.HasNaturalDuration += (sender, e) => { e.HasDuration = VideoEl.NaturalDuration.HasTimeSpan; };
            vcvm.GetDuration        += (sender, e) => { e.Duration = VideoEl.NaturalDuration.TimeSpan; };
            vcvm.GetPosition        += (sender, e) => { e.Position = VideoEl.Position; };
            vcvm.Load_Video();

            // subtitling view model init
            SubtitlingVM subvm = new SubtitlingVM(vcvm, szSrtPath, szOutpPath);

            // data context
            SrtEditPageVM sepvm = new SrtEditPageVM();
            sepvm.VideoCtrlsVM = vcvm;
            sepvm.SubtitlingVM = subvm;
            DataContext = sepvm;
        }

        private void Init_Video_Position_Ctrls(object sender, RoutedEventArgs e)
        {
            SrtEditPageVM sepvm = DataContext as SrtEditPageVM;
            VideoCtrlsVM vcvm = sepvm.VideoCtrlsVM as VideoCtrlsVM;
            vcvm.InitVideoCtrls();
            vcvm.Reset_Video();
            (sepvm.SubtitlingVM as SubtitlingVM).InitSubtitling();
        }

        private void Reset_Video(object sender, RoutedEventArgs e)
        {
            VideoCtrlsVM vcvm = ((SrtEditPageVM)DataContext).VideoCtrlsVM as VideoCtrlsVM;
            vcvm.Reset_Video();
        }

        private void VideoEl_TogglePositionUpdate(object sender, RoutedEventArgs e)
        {
            VideoCtrlsVM vcvm = ((SrtEditPageVM)DataContext).VideoCtrlsVM as VideoCtrlsVM;
            vcvm.Toggle_Video_Position_Update();
        }
    }
}
