﻿using System;
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
using Microsoft.Win32;

namespace TarjamatSRTEditor
{
    /// <summary>
    /// Interaction logic for TarSRTEditorHome.xaml
    /// </summary>
    public partial class TarSRTEditorHome : Page
    {
        public TarSRTEditorHome()
        {
            InitializeComponent();
        }

        private string szVidPath;
        private string szSrtPath;
        private string szOutpPath;

        private void Browse_Dialog(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            string senderName = ((Button)sender).Name;
            ofd.Filter = (senderName == "VideoFileBrowseBtn")?
                "Video files (*.mp4)|*.mp4|Video files (*.wmv)|*.wmv|Video files (*.wav)|*.wav|Video files (*.ogg)|*.ogg":
                "Subtitles files (*.srt)|*.srt";
            if (ofd.ShowDialog() == true)
            {
                if(senderName == "VideoFileBrowseBtn")
                {
                    VideoFileTxBx.Text = ofd.FileName;
                }
                else
                {
                    SrtFileTxBx.Text = ofd.FileName;
                    OutputFileTxBx.Text = ofd.FileName.Replace(".srt", "-modified.srt");
                }
            }
        }

        private void Open_Editor_Page(object sender, RoutedEventArgs e)
        {
            // save the new files paths
            szVidPath = VideoFileTxBx.Text;
            szSrtPath = SrtFileTxBx.Text;
            szOutpPath = OutputFileTxBx.Text;
            // navigate to Editor Page
            TarSRTEditorEditPage EditPage = new TarSRTEditorEditPage(szVidPath, szSrtPath, szOutpPath);
            this.NavigationService.Navigate(EditPage);
        }
    }
}
