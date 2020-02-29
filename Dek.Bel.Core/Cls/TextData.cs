using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dek.Cls
{
    public class TextData
    {
        public string Original { get; }
        public string Filtered { get => FilterOriginal(Deselections); }
        public List<string> DeselectedTexts { get => GetDeslectedTexts(Deselections); }
        public List<DekRange> Deselections { get; private set; }
        public List<DekRange> InverseDeselections { get => GetInverseDeselection(); }
        public string Elipsis { get; set; } = "…";

        public void AddDeselection(DekRange textRange)
        {
            foreach(var range in Deselections)
            {
                if (textRange.Intersects(range))
                    throw new ArgumentException($"New range {textRange} intersects existing range {range}.");
            }

            Deselections.Add(textRange);

            Deselections = Deselections.OrderBy(x => x).ToList();
        }

        private List<string> GetDeslectedTexts(List<DekRange> deselectionRanges)
        {
            throw new NotImplementedException();
        }
        

        private string FilterOriginal(List<DekRange> deselections)
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
        private List<DekRange> GetInverseDeselection()
        {
            if (string.IsNullOrEmpty(Original))
                return new List<DekRange>();

            DekRange origRange = new DekRange(0, Original.Length - 1);
            var invertedSelections = new List<DekRange>();
            int pos = 0;
            foreach(var range in Deselections)
            {
                if (pos < range.Start)
                    invertedSelections.Add(new DekRange(pos, range.Start - 1));

                pos = range.Stop + 1;
            }

            return invertedSelections;
        }
    }




}
