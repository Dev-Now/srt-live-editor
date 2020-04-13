using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarjamatSRTEditor.SubtitlingModels
{
    public class Subtitle
    {
        public uint Index { get; set; }
        public string Text { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        private Subtitle() { }

        public Subtitle(uint _nIndex, string _szText, TimeSpan _tsStart, TimeSpan _tsEnd)
        {
            Index = _nIndex;
            Text = String.Copy(_szText);
            StartTime = new TimeSpan(_tsStart.Ticks);
            EndTime = new TimeSpan(_tsEnd.Ticks);
        }

        public Subtitle DeepCopy()
        {
            Subtitle subCopy = new Subtitle();
            subCopy.Index = this.Index;
            subCopy.Text = String.Copy(this.Text);
            subCopy.StartTime = new TimeSpan(this.StartTime.Ticks);
            subCopy.EndTime = new TimeSpan(this.EndTime.Ticks);
            return subCopy;
        }

        public bool IsSubtitleShowingAt(TimeSpan ts)
        {
            if ((ts >= StartTime) && (ts <= EndTime)) return true;
            else return false;
        }
    }
}
