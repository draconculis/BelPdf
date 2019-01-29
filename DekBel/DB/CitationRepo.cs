using BelManagedLib;
using Dek.Bel.Cls;
using Dek.Bel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.DB
{
    [Export]
    public class CitationRepo
    {
        [Import] IDBService dBService { get; set; }
        string SqlSelectFields = $"SELECT `Id`, `Fragment`, `PageStart`, `PageStop`, `GlyphStart`, `GlyphStop`, `Rectangles`, `Date`";

        public CitationRepo()
        {
            Mef.Initialize(this);
        }

        public RawCitation AddRawCitations(EventData messsage)
        {
            RawCitation raw = new RawCitation
            {
                Id = Id.NewId(),
                Fragment = messsage.Text,
                GlyphStart = messsage.StartGlyph,
                GlyphStop = messsage.StopGlyph,
                PageStop = messsage.StopPage,
                PageStart = messsage.StartPage,
                Date = DateTime.Now,
            };

            dBService.InsertOrUpdate(raw);

            return raw;
        }

        private static string ComposeCitation(List<RawCitation> rawCitations, string citation, string adjoiner = null)
        {
            // TODO: The rectangles, jada jada

            string text = "";
            if (adjoiner == null) adjoiner = "";
            foreach(var raw in rawCitations)
            {
                text += adjoiner + raw.Fragment;
            }

            text += adjoiner + citation;
            return text;
        }

        internal static Citation CreateNewCitation(List<RawCitation> rawCitations, EventData message, string adjoiner)
        {
            int[] rects = ExtractArrayFromEventData(message.SelectionRects, message.Len * 4);
            var citation = new Citation
            {
                Id = Id.NewId(),
                Citation1 = ComposeCitation(rawCitations, message.Text, adjoiner), // Original, untouched
                Citation2 = ComposeCitation(rawCitations, message.Text, adjoiner), // More of the same, this will be editied in texb 1
                CreatedDate = DateTime.Now,
                GlyphStart = message.StartGlyph,
                PageStop = message.StopGlyph,
                PhysicalPageStart = message.StartPage,
                PhysicalPageStop = message.StopPage,
                SelectionRects = ConvertArrayToString(rects),
            };

            return citation;
        }

        internal static int[] ExtractArrayFromEventData(IntPtr ptr, int len)
        {
            IntPtr start = ptr;
            int[] result = new int[len];
            Marshal.Copy(start, result, 0, len);
            return result;
        }

        public static string ConvertArrayToString(int[] rects)
        {
            string res = "";
            int counter = 1;
            foreach(int i in rects)
                res += i.ToString() + $"{(counter++ % 4 == 0 ? ";" : ",")}";

            return res;
        }

    }
}
