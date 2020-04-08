using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarjamatSRTEditor.SubtitlingModels
{
    class SubtitlingModel
    {
        private string szOrigSrtFilePath;
        private SubtitleList sblOrigSubtitles;

        private string szOutpSrtFilePath;
        private SubtitleList sblOutpSubtitles;

        public SubtitlingModel(string _szOrigSrt, string _szOutpSrt)
        {
            szOrigSrtFilePath = _szOrigSrt;
            szOutpSrtFilePath = _szOutpSrt;
            sblOrigSubtitles = new SubtitleList();
            // todo: extract subtitles from original srt file and add them to sblOrigSubtitles
            sblOutpSubtitles = sblOrigSubtitles.DeepCopy();
        }
    }
}
