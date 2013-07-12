using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GlobalHotkeys;
using System.Diagnostics;

namespace TagPan
{
    public partial class TagForm : Form
    {
        public Dictionary<ShortKey, EventHandler> hotkeyTable = new Dictionary<ShortKey, EventHandler>();
        private GlobalHotkey ghk_apply;
        private GlobalHotkey ghk_select;
        private GlobalHotkey ghk_select_common;
        private GlobalHotkey ghk_untag;
        private GlobalHotkey ghk_addtag;
        private GlobalHotkey ghk_renametag;
        private GlobalHotkey ghk_deletetag;
        private GlobalHotkey ghk_addToSelection_toggle;
        private GlobalHotkey ghk_ChildAuto_toggle;

        public TagForm()
        {
            InitializeComponent();
            FormClosing += TagForm_FormClosing;
            Init();
        }

        private void Init()
        {
            ghk_apply = new GlobalHotkey(Modifiers.Shift, Keys.D1, this, true);
            hotkeyTable.Add(new ShortKey(Modifiers.Shift, Keys.D1), tagPanel.ApplyTag);
            ghk_select = new GlobalHotkey(Modifiers.Shift, Keys.D2, this, true);
            hotkeyTable.Add(new ShortKey(Modifiers.Shift, Keys.D2), tagPanel.SelectTaggedItem);
            ghk_select_common = new GlobalHotkey(Modifiers.Shift, Keys.D3, this, true);
            hotkeyTable.Add(new ShortKey(Modifiers.Shift, Keys.D3), tagPanel.SelectCommonItems);
            ghk_untag = new GlobalHotkey(Modifiers.Shift, Keys.D4, this, true);
            hotkeyTable.Add(new ShortKey(Modifiers.Shift, Keys.D4), tagPanel.RemoveTag);
            ghk_addtag = new GlobalHotkey(Modifiers.Ctrl, Keys.D1, this, true);
            hotkeyTable.Add(new ShortKey(Modifiers.Ctrl, Keys.D1), tagPanel.AddTag);
            ghk_renametag  = new GlobalHotkey(Modifiers.Ctrl, Keys.D2, this, true);
            hotkeyTable.Add(new ShortKey(Modifiers.Ctrl, Keys.D2), tagPanel.RenameTag);
            ghk_deletetag = new GlobalHotkey(Modifiers.Ctrl, Keys.D3, this, true);
            hotkeyTable.Add(new ShortKey(Modifiers.Ctrl, Keys.D3), tagPanel.DeleteTag);
            ghk_addToSelection_toggle = new GlobalHotkey(Modifiers.Ctrl, Keys.D4, this, true);
            hotkeyTable.Add(new ShortKey(Modifiers.Ctrl, Keys.D4), tagPanel.AddToSelection);
            ghk_ChildAuto_toggle = new GlobalHotkey(Modifiers.Ctrl, Keys.D5, this, true);
            hotkeyTable.Add(new ShortKey(Modifiers.Ctrl, Keys.D5), tagPanel.ChildAutoSelectToggle);
        }

        void TagForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ghk_apply.Dispose();
            ghk_select.Dispose();
            ghk_select_common.Dispose();
            ghk_untag.Dispose();
            ghk_addtag.Dispose();
            ghk_renametag.Dispose();
            ghk_deletetag.Dispose();
            ghk_addToSelection_toggle.Dispose();
            ghk_ChildAuto_toggle.Dispose();
        }

        protected override void WndProc(ref Message m)
        {

            var hotkeyInfo = HotkeyInfo.GetFromMessage(m);
            Debug.WriteLine((WindowsUtilities.WindowsMessages)m.Msg);
            Debug.WriteLine(hotkeyInfo);
            if (hotkeyInfo != null)
            {
                var keypair = hotkeyTable.Where(x => x.Key == hotkeyInfo).FirstOrDefault();
                keypair.Value(null, null);
            }
            base.WndProc(ref m);
        }
    }
}
