using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace TagPan
{
    public enum concat
    {
        [Description("+")]
        addition,
        [Description("-")]
        substraction,
        [Description("*")]
        intersection
    }

    public abstract class ConsoleElement :IConsoleSelElement
    {

        public ConsoleElement()
        {
        }



        public abstract List<int> getCorrespondingSel();
        public abstract string getCorrespondingStr();

        public static List<int> operator +(ConsoleElement c1, ConsoleElement c2)
        {
            List<int> result = new List<int>();
            result.AddRange(c1.getCorrespondingSel());
            result.AddRange(c2.getCorrespondingSel());
            result=result.Distinct().ToList();
            return result;
        }
        public static List<int> operator +(List<int> c1, ConsoleElement c2)
        {
            List<int> result = new List<int>();
            result.AddRange(c1);
            result.AddRange(c2.getCorrespondingSel());
            result = result.Distinct().ToList();
            return result;
        }
        public static List<int> operator -(ConsoleElement c1, ConsoleElement c2)
        {
            List<int> result = new List<int>();
            result.AddRange(c1.getCorrespondingSel().Where(x=> !c2.getCorrespondingSel().Any(y=> x==y)) );
            result = result.Distinct().ToList();
            return result;
        }
        public static List<int> operator -(List<int> c1, ConsoleElement c2)
        {
            List<int> result = new List<int>();
            result.AddRange(c1.Where(x => !c2.getCorrespondingSel().Any(y => x == y)));
            result = result.Distinct().ToList();
            return result;
        }
        public static List<int> operator *(ConsoleElement c1, ConsoleElement c2)
        {
            List<int> result = new List<int>();
            result.AddRange(c1.getCorrespondingSel().Intersect(c2.getCorrespondingSel()));
            return result;
        }
        public static List<int> operator *(List<int> c1, ConsoleElement c2)
        {
            List<int> result = new List<int>();
            result.AddRange(c1.Intersect(c2.getCorrespondingSel()));
            return result;
        }
    }
}
