using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace StudyLockerService
{
    public partial class StudyLockerService : ServiceBase
    {
        private EventLog eventLog = new EventLog();
        private ManagementEventWatcher eventWatcher;

        public StudyLockerService()
        {
            InitializeComponent();

            // Setup of event logs
            eventLog = new EventLog();
            if (!EventLog.SourceExists("StudyLockerService"))
            {
                EventLog.CreateEventSource("StudyLockerService", "Study Locker");
            }
            eventLog.Source = "StudyLockerService";
            eventLog.Log = "Study Locker";

            // Setup of process monitoring
            eventWatcher = new ManagementEventWatcher(new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace"));
            eventWatcher.EventArrived += ProcessOpened;
            eventWatcher.Start();
        }

        private void ProcessOpened(object sender, EventArrivedEventArgs e)
        {
            string processName = e.NewEvent.Properties["ProcessName"].Value.ToString();
            eventLog.WriteEntry("Process started: " + processName);
        }

        protected override void OnStart(string[] args)
        {
            eventLog.WriteEntry("Oh hi there, the Study Locker service has started successfully");
        }

        protected override void OnStop()
        {
            eventLog.WriteEntry("Well see you later, the Study Locker service has stopped");
        }
    }
}
