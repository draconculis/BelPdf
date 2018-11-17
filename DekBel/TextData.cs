using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Dek.Cls;

namespace Dek.Bel
{
    public class TextData
    {
        public string Original { get; }
        public string Filtered { get => FilterOriginal(Deselections); }
        public List<string> DeselectedTexts { get => GetDeslectedTexts(Deselections); }
        public List<TextRange> Deselections { get; private set; }
        public List<TextRange> InverseDeselections { get => GetInverseDeselection(); }
        public string Elipsis { get; set; } = "…";

        public void AddDeselection(TextRange textRange)
        {
            foreach(var range in Deselections)
            {
                if (textRange.Intersects(range))
                    throw new ArgumentException($"New range {textRange} intersects existing range {range}.");
            }

            Deselections.Add(textRange);

            Deselections = Deselections.OrderBy(x => x).ToList();
        }

        private List<string> GetDeslectedTexts(List<TextRange> deselectionRanges)
        {
            throw new NotImplementedException();
        }

        

        

        private string FilterOriginal(List<TextRange> deselections)
        {
            StringBuilder result = new StringBuilder();

            bool isFirst = true;
            foreach(var range in InverseDeselections)
            {
                if (!isFirst)
                    result.Append($" {Elipsis} ");
                else
                    isFirst = false;

                result.Append(Original.Range(range));
            }

            return result.ToString();
        }


        /*
         0123456789
         desel (1,2), (5,6)
         result = 0|34|789
         ranges: (0,0)(3,4)(5,6)
        */
        private List<TextRange> GetInverseDeselection()
        {
            if (string.IsNullOrEmpty(Original))
                return new List<TextRange>();

            TextRange origRange = new TextRange(0, Original.Length - 1);
            var invertedSelections = new List<TextRange>();
            int pos = 0;
            foreach(var range in Deselections)
            {
                if (pos < range.Start)
                    invertedSelections.Add(new TextRange(pos, range.Start - 1));

                pos = range.Stop + 1;
            }

            return invertedSelections;
        }
    }




}
