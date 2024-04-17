using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lib_K_Relay.Networking.Packets;
using Lib_K_Relay.Networking.Packets.Server;

namespace Lib_K_Relay.Utilities
{
    public static class PluginUtils
    {
        /// <summary>
        ///     Executes an action and properly catches and logs any exceptions.
        /// </summary>
        /// <param name="action">Operation to execute</param>
        /// <param name="errorProvider">Name of the sender if an exception occurs</param>
        /// <returns>If the operation was successful or not</returns>
        public static bool ProtectedInvoke(Action action, string errorProvider)
        {
            return ProtectedInvoke(action, errorProvider, null);
        }

        /// <summary>
        ///     Executes an action and properly catches and logs any exceptions.
        /// </summary>
        /// <param name="action">Operation to execute</param>
        /// <param name="errorProvider">Name of the sender if an exception occurs</param>
        /// <param name="filteredException">The Type of exeception you wish to ignore/not log</param>
        /// <returns>If the operation was successful or not</returns>
        public static bool ProtectedInvoke(Action action, string errorProvider, Type filteredException)
        {
            try
            {
                action();
                return true;
            }
            catch (Exception e)
            {
                if (e.GetType() != filteredException)
                    LogPluginException(e, errorProvider);
                return false;
            }
        }

        /// <summary>
        ///     Logs a formatted exception log including target site and exception details.
        /// </summary>
        /// <param name="e">Exception to log</param>
        /// <param name="caller">Name of operation that failed</param>
        private static void LogPluginException(Exception e, string caller)
        {
            var site = e.TargetSite;
            var methodName = site == null ? "<null method reference>" : site.Name;
            if (site?.ReflectedType == null) return;
            var className = site.ReflectedType.Name;

            Log("Error", "An exception was thrown within {0} at {1} {2}",
                caller, className + "." + methodName, e);
        }

        /// <summary>
        ///     Logs a formatted string containing a sender and message.
        /// </summary>
        /// <param name="sender">Name of the module/system that is logging the message</param>
        /// <param name="message">Message to be logged</param>
        public static void Log(string sender, string message)
        {
            if (sender.Length > 13) sender = sender.Substring(0, 13);
            sender += "]";
            Console.WriteLine(@"[{0,-15} {1}", sender, message);
        }

        /// <summary>
        ///     Logs a formatted string containing a sender and message.
        /// </summary>
        /// <param name="sender">Name of the module/system that is logging the message</param>
        /// <param name="message">Message to be logged</param>
        /// <param name="list">Objects to be formatted into message</param>
        public static void Log(string sender, string message, params object[] list)
        {
            var formatted = string.Format(message, list);
            Log(sender, formatted);
        }

        /// <summary>
        ///     Starts a message loop for the specified form instance and displays the form.
        /// </summary>
        /// <param name="gui">Form to be shown</param>
        public static void ShowGui(Form gui)
        {
            gui.Shown += (s, e) =>
            {
                gui.WindowState = FormWindowState.Minimized;
                gui.Show();
                gui.WindowState = FormWindowState.Normal;
            };

            Task.Run(() => gui.ShowDialog());
        }

        /// <summary>
        ///     Displays a form containing a configuration panel based off of the provided Settings objects.
        /// </summary>
        /// <param name="settingsObject">Settings to base the form off of</param>
        /// <param name="title">Title of the form to be shown</param>
        /// <param name="proxy"></param>
        public static void ShowGenericSettingsGui(dynamic settingsObject, string title)
        {
            ShowGui(new FrmGenericSettings(settingsObject, title));
        }

        /// <summary>
        ///     Waits the specified amount of ms then invokes the callback.
        ///     (Wait time is accurate down to ~50ms measurements)
        /// </summary>
        /// <param name="ms">Amount of time to wait</param>
        /// <param name="callback">Action to be invoked</param>
        public static void Delay(int ms, Action callback)
        {
            Task.Run(() =>
            {
                Thread.Sleep(ms);
                callback();
            });
        }

        /// <summary>
        ///     Displays a notification above a specified object.
        /// </summary>
        /// <param name="message">Message of the notification</param>
        /// <param name="picture"> Image to show in log message </param>
        /// <returns></returns>
        public static NotificationPacket CreateNotification(string message, int picture = 1562)
        {
            var notif = (NotificationPacket)Packet.Create(PacketType.NOTIFICATION);
            notif.Message = message;
            notif.PictureType = picture;
            notif.TypeValue = NotificationPacket.NotificationType.PortalOpened;
            notif.TextByte = 10;
            return notif;
        }

        /// <summary>
        ///     Creates an in-game message with the Oryx format and coloring.
        /// </summary>
        /// <param name="sender">Message sender</param>
        /// <param name="message">Message text</param>
        /// <returns></returns>
        public static TextPacket CreateOryxNotification(string sender, string message)
        {
            var text = (TextPacket)Packet.Create(PacketType.TEXT);
            text.BubbleTime = 0;
            text.Name = "#" + sender;
            text.NumStars = -1;
            text.ObjectId = -1;
            text.Text = message;
            return text;
        }
    }
}