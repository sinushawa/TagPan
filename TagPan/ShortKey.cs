using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GlobalHotkeys;
using System.Windows.Forms;

namespace TagPan
{
    public class ShortKey
    {
        public Modifiers modifier;
        public Keys key;

        public ShortKey(Modifiers _modifier, Keys _key)
        {
            modifier = _modifier;
            key = _key;
        }
        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            HotkeyInfo p = obj as HotkeyInfo;
            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (modifier == p.Modifiers) && (key == p.Key);
        }
        public static bool operator ==(ShortKey a, HotkeyInfo b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.modifier == b.Modifiers && a.key == b.Key;
        }

        public static bool operator !=(ShortKey a, HotkeyInfo b)
        {
            return !(a == b);
        }
        public override int GetHashCode()
        {
            return (int)modifier ^ (int)key;
        }
    }
    
}
