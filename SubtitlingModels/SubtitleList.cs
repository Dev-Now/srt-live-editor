using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarjamatSRTEditor.SubtitlingModels
{
    class SubtitleList : List<Subtitle>
    {
        private int _nLastSubShowingNdx = 0; // means no subtitle was showing at initial state

        public SubtitleList DeepCopy()
        {
            SubtitleList sblCopy = new SubtitleList();
            foreach (Subtitle sub in this) { sblCopy.Add(sub.DeepCopy()); }
            return sblCopy;
        }

        public string GetSubtitleAt(TimeSpan ts)
        {
            if ( (_nLastSubShowingNdx > 0) && (this[_nLastSubShowingNdx - 1].StartTime <= ts) )
            {
                for (int nSubNdx = _nLastSubShowingNdx - 1; nSubNdx < this.Count; ++nSubNdx)
                {
                    if (this[nSubNdx].IsSubtitleShowingAt(ts))
                    {
                        _nLastSubShowingNdx = nSubNdx+1;
                        return this[nSubNdx].Text;
                    }
                }
            }
            foreach (Subtitle sub in this)
            {
                if (sub.IsSubtitleShowingAt(ts))
                {
                    _nLastSubShowingNdx = Convert.ToInt32(sub.Index);
                    return sub.Text;
                }
            }
            return "";
        }
    }
}
