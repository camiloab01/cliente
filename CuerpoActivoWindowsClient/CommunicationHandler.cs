using Microsoft.AspNet.SignalR.Client;
using SignalRBroadcastServiceSample.Domain;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsClient
{
    public class CommunicationHandler : ApplicationContext
    {
        public CommunicationHandler()
        {
            InitiateCommunication();
        }

        private static void InitiateCommunication()
        {
            var hubConnection = new HubConnection("http://localhost:8084");
            IHubProxy cuerpoActivoHubProxy = hubConnection.CreateHubProxy("CuerpoActivoServiceHub");

            // This line is necessary to subscribe for broadcasting messages
            cuerpoActivoHubProxy.On<Reminder>("NotifyReminder", HandleReminder);

            // Start the connection
            hubConnection.Start().Wait();
        }

        private static void HandleReminder(Reminder reminder)
        {
            var item = new NotifyIcon();
            item.Visible = true;
            item.Icon = SystemIcons.Information;
            item.ShowBalloonTip(3000, reminder.Reminder_title, reminder.Reminder_preview_description, ToolTipIcon.Info);
        }
    }
}
