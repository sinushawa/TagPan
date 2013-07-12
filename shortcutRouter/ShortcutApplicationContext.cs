using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GlobalHotkeys;

namespace shortcutRouter
{
    class ShortcutApplicationContext : ApplicationContext
    {
        private GlobalHotkey ghk_apply;
        private GlobalHotkey ghk_select;
        private GlobalHotkey ghk_select_common;
        private GlobalHotkey ghk_untag;
        private GlobalHotkey ghk_addtag;
        private GlobalHotkey ghk_renametag;
        private GlobalHotkey ghk_deletetag;
        private GlobalHotkey ghk_addToSelection_toggle;
        private GlobalHotkey ghk_ChildAuto_toggle;

        NotifyIcon notifyIcon = new NotifyIcon();

        public ShortcutApplicationContext()
        {
            MenuItem exitMenuItem = new MenuItem("Exit", new EventHandler(Exit));

            notifyIcon.Icon = shortcutRouter.Properties.Resources.AppIcon;
            notifyIcon.ContextMenu = new ContextMenu(new MenuItem[] { exitMenuItem });
            notifyIcon.Visible = true;


        }


        void Exit(object sender, EventArgs e)
        {
            // We must manually tidy up and remove the icon before we exit.
            // Otherwise it will be left behind until the user mouses over.
            notifyIcon.Visible = false;

            Application.Exit();
        }
    }
}
