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
            SrtFileParser srtParser = new SrtFileParser(_szOrigSrt);
            Subtitle subNext = srtParser.GetNextSubtitle();
            while (subNext != null)
            {
                sblOrigSubtitles.Add(subNext);
                subNext = srtParser.GetNextSubtitle();
            }
            sblOutpSubtitles = sblOrigSubtitles.DeepCopy();
        }
    }
}
