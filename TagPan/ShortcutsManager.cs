using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LowLevelHooks.Keyboard;
using RobUtilities;

namespace TagPan
{
    [FlagsAttribute]
    public enum ModifierKey
    {
        None = 0,
        Shift = 1 << 1,
        Ctrl = 1 << 2,
        Alt = 1 << 3,
        Console = 1 << 4
    }

    public class ShortStroke
    {
        public ModifierKey modifier;
        public List<char> key;

        public ShortStroke(ModifierKey _modifier, List<char> _key)
        {
            modifier = _modifier;
            key = _key;
        }
        public static bool operator ==(ShortStroke a, ShortStroke b)
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
            return a.modifier.Is(b.modifier) && a.key.SequenceEqual(b.key);
        }

        public static bool operator !=(ShortStroke a, ShortStroke b)
        {
            return !(a == b);
        }
    }
    public class ShortcutsManager
    {
        private readonly KeyboardHook kHook;
        private ModifierKey _activeModifier= ModifierKey.None;
        public Dictionary<ShortStroke, EventHandler> HookTable;
        public List<char> CharTable;
        public bool _consoleMode = false;
        public bool _shiftMode = false;
        public bool _ctrlMode = false;
        public bool _altMode = false;

        public ShortcutsManager()
        {
           kHook = new KeyboardHook();
           HookTable = new Dictionary<ShortStroke, EventHandler>();
           CharTable = new List<char>();
           kHook.Hook();
           kHook.KeyDown +=kHook_KeyDown;
           kHook.KeyUp += kHook_KeyUp;
        }
        public void ReHook()
        {
            kHook.Hook();
        }

        void kHook_KeyUp(object sender, KeyboardHookEventArgs e)
        {
            switch (e.KeyString)
            {
                case "[LShiftKey]":
                    _activeModifier = _activeModifier.Remove(ModifierKey.Shift);
                    break;
                case "[LControlKey]":
                    _activeModifier = _activeModifier.Remove(ModifierKey.Ctrl);
                    break;
                case "[LMenu]":
                    _activeModifier = _activeModifier.Remove(ModifierKey.Alt);
                    break;
                default:
                    break;
            }
        }

        private void kHook_KeyDown(object sender, KeyboardHookEventArgs e)
        {
            switch (e.KeyString)
            {
                case "[LShiftKey]":
                    _activeModifier = _activeModifier.Add(ModifierKey.Shift);
                    break;
                case "[LControlKey]":
                    _activeModifier=_activeModifier.Add(ModifierKey.Ctrl);
                    break;
                case "[LMenu]":
                    _activeModifier = _activeModifier.Add(ModifierKey.Alt);
                    break;
                case "\\":
                    _activeModifier = _activeModifier.Add(ModifierKey.Console);
                    break;
                default:
                    ProcessKey(e.Char);
                    break;
            }
        }

        private void ProcessKey(Char e)
        {
            if (_activeModifier.Has(ModifierKey.Shift) || _activeModifier.Has(ModifierKey.Ctrl) || _activeModifier.Has(ModifierKey.Alt))
            {
                CharTable.Add(e);
                ShortStroke _inputToCompare = new ShortStroke(_activeModifier, CharTable);
                ProcessCommand(_inputToCompare);
            }
            else if (e == '\u0009')
            {
                ShortStroke _inputToCompare = new ShortStroke(_activeModifier, CharTable);
                ProcessCommand(_inputToCompare);
                _activeModifier = _activeModifier.Remove(ModifierKey.Console);
            }
            else if (_activeModifier.Is(ModifierKey.Console))
            {
                CharTable.Add(e);
            }
        }
        private void ProcessCommand(ShortStroke _command)
        {
            foreach (KeyValuePair<ShortStroke, EventHandler> _pair in HookTable)
            {
                if (_command == _pair.Key)
                {
                    _pair.Value(null, null);
                }
            }
        }

        public void AddShortcut(ModifierKey _modifier, List<char> _chars, EventHandler _function)
        {
            HookTable.Add(new ShortStroke(_modifier, _chars), _function);
        }
    }
}
