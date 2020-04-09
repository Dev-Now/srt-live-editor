using System;
using System.Globalization;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarjamatSRTEditor.SubtitlingModels
{
    class SrtFileParser
    {
        private string[] szSrtFileLines;
        private uint uNextLineToParse;
        private uint nSubtitleNdx = 0u;

        public SrtFileParser(string szSrtFilePath)
        {
            if (File.Exists(szSrtFilePath)) 
            {
                uNextLineToParse = 0;
                szSrtFileLines = File.ReadAllLines(szSrtFilePath);
            }
        }

        public Subtitle GetNextSubtitle()
        {
            try
            {
                string szNextValidLine = GetNextValidLine();
                if (szNextValidLine == "") return null; // no more subtitles

                // index parsing
                Convert.ToUInt32(szNextValidLine); // just to make sure the srt file content is valid
                uint nIndex = ++nSubtitleNdx;
                // start and end parsing
                szNextValidLine = GetNextValidLine();
                TimeSpan tsStart, tsEnd;
                ParseStartAndEnd(in szNextValidLine, out tsStart, out tsEnd);
                // text parsing
                szNextValidLine = GetNextValidLine();
                if (szNextValidLine.Length == 0) throw new Exception("Empty subtitle text.");
                string szText = String.Copy(szNextValidLine);
                // take care of compound subtitles...
                bool bThereIsMore = EndsWith(szText, "###");
                string szNextLine = GetNextLine();
                StringBuilder sbText = new StringBuilder(RemoveEnding(szText, "###"));
                while (bThereIsMore || (szNextLine.Length != 0))
                {
                    if (bThereIsMore)
                    {
                        szNextValidLine = GetNextValidLine();
                        Convert.ToUInt32(szNextValidLine); // just to make sure the srt file content is valid
                        szNextValidLine = GetNextValidLine();
                        TimeSpan tsUnused;
                        ParseStartAndEnd(in szNextValidLine, out tsUnused, out tsEnd); // update the end time
                        szNextValidLine = GetNextValidLine();
                        if (szNextValidLine.Length == 0) throw new Exception("Empty subtitle text line.");
                    }
                    else
                    {
                        szNextValidLine = szNextLine;
                        szNextLine = GetNextLine();
                    }
                    sbText.Append("\n").Append(RemoveEnding(szNextValidLine, "###"));
                    // check if there is more
                    bThereIsMore = EndsWith(szNextValidLine, "###");
                }
                szText = sbText.ToString();
                return new Subtitle(nIndex, szText, tsStart, tsEnd);
            }
            catch (Exception e)
            {
                throw new Exception("Failed to parse SRT file", e);
            }
        }

        #region navigation utils
        private string GetNextValidLine()
        {
            if (uNextLineToParse >= szSrtFileLines.Length) return "";
            string szTrimmedLine = szSrtFileLines[uNextLineToParse].Trim();
            while (szTrimmedLine.Length == 0) // ignore empty lines
            {
                if (++uNextLineToParse >= szSrtFileLines.Length) return "";
                szTrimmedLine = szSrtFileLines[uNextLineToParse].Trim();
            }
            ++uNextLineToParse;
            return szTrimmedLine;
        }

        private string GetNextLine()
        {
            uint nLineNdx = uNextLineToParse++;
            return (nLineNdx < szSrtFileLines.Length) ? szSrtFileLines[nLineNdx].Trim() : "";
        }
        #endregion

        #region parsing utils
        private void ParseStartAndEnd(in string szLine, out TimeSpan tsStart, out TimeSpan tsEnd)
        {
            string[] stringSeparators = new string[] { "-->"};
            string[] parts = szLine.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries).Select(szPart => szPart.Trim()).ToArray();
            if (parts.Length != 2) throw new Exception("Failed to parse subtitle start and end.");
            tsStart = TimeSpan.Parse(parts[0], new CultureInfo("ru-RU"));
            tsEnd = TimeSpan.Parse(parts[1], new CultureInfo("ru-RU"));
        }

        private bool EndsWith(string szTxt, string szEnding)
        {
            return szTxt.Substring(szTxt.Length - szEnding.Length) == szEnding;
        }

        private string RemoveEnding(string szTxt, string szEnding)
        {
            if (EndsWith(szTxt, szEnding))
            {
                return szTxt.Substring(0, szTxt.Length - szEnding.Length);
            }
            else { return szTxt; } 
        }
        #endregion
    }
}
