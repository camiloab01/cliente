using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.AspNet.SignalR.Client;
using SignalRBroadcastServiceSample.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace CuerpoActivoWPFClient
{
    public class CommunicationHandler
    {
        private String baseUrl = "http://dev-cuerpoactivo.lumenup.net/";
        private String attachment;

        public CommunicationHandler()
        {
            InitiateCommunication();
        }

        private void InitiateCommunication()
        {
            string ip = String.Format("http://{0}:8084", ConfigurationManager.AppSettings.Get("IPAddress")); 
            var hubConnection = new HubConnection(ip);
            IHubProxy cuerpoActivoHubProxy = hubConnection.CreateHubProxy("CuerpoActivoServiceHub");

            // This line is necessary to subscribe for broadcasting messages
            cuerpoActivoHubProxy.On<Reminder>("NotifyReminder", HandleReminder);

            // Start the connection
            hubConnection.Start().Wait();
        }

        private void HandleReminder(Reminder _reminder)
        {
            Application.Current.Dispatcher.Invoke(new System.Action(() =>
            {
                TaskbarIcon notification = new TaskbarIcon();
                String notificationMessage = string.Empty;
                Icon icon = Properties.Resources.icon_colores;

                if(!string.IsNullOrEmpty(_reminder.Reminder_attachment))
                {
                    attachment = baseUrl + _reminder.Reminder_attachment;
                    notificationMessage = _reminder.Reminder_preview_description + Environment.NewLine + baseUrl + _reminder.Reminder_attachment;
                    notification.TrayBalloonTipClicked += new RoutedEventHandler(notification_Click);
                }
                else
                {
                    notificationMessage = _reminder.Reminder_preview_description;
                }
                
                notification.ShowBalloonTip(_reminder.Reminder_title, notificationMessage, icon, false);
            }));
        }

        private void notification_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(attachment);
        }
    }
}
