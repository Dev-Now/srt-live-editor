using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarjamatSRTEditor.SubtitlingModels
{
    class SubtitleList : List<Subtitle>
    {
        public SubtitleList DeepCopy()
        {
            SubtitleList sblCopy = new SubtitleList();
            foreach (Subtitle sub in this) { sblCopy.Add(sub.DeepCopy()); }
            return sblCopy;
        }
    }
}
