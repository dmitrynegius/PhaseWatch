using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Synthesis;
using Newtonsoft.Json;
using System.IO;

namespace PhaseWatch
{
    public partial class MainForm : Form
    {
        private bool hidden = false;
        private bool SpeakNow = false;
        Task task;
        SpeechSynthesizer syn = new SpeechSynthesizer();

        public MainForm()
        {
            InitializeComponent();
            Load();
            label1.Select();
            dateTimePickerWakeUp.Checked = true;
            task = Task.Run((System.Action)Speak);
        }

        public void Save()
        {
            try
            {
                string text = JsonConvert.SerializeObject(dateTimePickerWakeUp.Value, Formatting.Indented);
                File.Delete("config.txt");
                File.WriteAllText("config.txt", text);
            }
            catch { }
        }


        public void Load()
        {
            try
            {
                string text = File.ReadAllText("config.txt");
                dateTimePickerWakeUp.Value = JsonConvert.DeserializeObject<DateTime>(text);
            }
            catch { }
        }


        private void HideToTray()
        {
            hidden = true;
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            notifyIcon1.Visible = true;
        }

        private void UnhideFromTray()
        {
            notifyIcon1.Visible = false;
            this.ShowInTaskbar = true;
            this.WindowState = FormWindowState.Normal;
            hidden = false;
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (hidden)
            {
                UnhideFromTray();
            }
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (hidden)
                UnhideFromTray();
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            if (!hidden)
                HideToTray();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Math.Abs(DateTime.Now.Subtract(dateTimePickerWakeUp.Value).TotalSeconds)
                < 60 * 10 && dateTimePickerWakeUp.Checked)
            {
                UnhideFromTray();
                SpeakNow = true;
            }
            else SpeakNow = false;
            label1.Text = "" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
        }

        public void Speak()
        {
            Random rnd = new Random();

            while (true)
            if (SpeakNow)
            {
                int r = rnd.Next(0, 9);
                switch (r)
                {
                    case 0: syn.Speak("Wake up"); break;
                    case 1: syn.Speak("Phase time"); break;
                    case 2: syn.Speak("It's time to leave the body"); break;
                    case 3: syn.Speak("Much sleep you"); break;
                    case 4: syn.Speak("Stop sleeping"); break;
                    case 5: syn.Speak("Leave the body"); break;
                    case 6: syn.Speak("Imagine the swimming"); break;
                    case 7: syn.Speak("Imagine rotation"); break;
                    case 8: syn.Speak("Imagine the rubbing of hands before the eyes"); break;
                    case 9: syn.Speak("Imagine the images before the eyey"); break;
                }
            }
        }

        private void dateTimePickerWakeUp_ValueChanged(object sender, EventArgs e)
        {
            Save();
        }
    }
}
