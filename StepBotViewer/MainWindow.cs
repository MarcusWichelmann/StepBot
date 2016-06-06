using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StepBotViewer
{
    public partial class MainWindow : Form
    {
        private const double stepsPerRound = 800;

        private enum LogType
        {
            STATUS,
            SENT,
            RECEIVED,
            ERROR
        }

        private StepBotClient client = new StepBotClient();

        private Scene scene = null;

        private bool keyUpPressed, keyDownPressed, keyLeftPressed, keyRightPressed;

        private int keyControlSpeed = 2000;

        private int lastSpeedLeft = 0;
        private int lastSpeedRight = 0;

        private delegate void ComputeCommandCallback(string command);
        private delegate void UpdateConnectionStateCallback();

        public MainWindow()
        {
            InitializeComponent();

            client.CommandReceived += client_CommandReceived;
            client.ConnectionStateChanged += client_ConnectionStateChanged;
        }

        private void WriteLog(LogType logType, string message)
        {
            string prefix = "";

            switch(logType)
            {
                case LogType.STATUS:
                    prefix = "Status: ";
                    break;
                case LogType.SENT:
                    prefix = "Gesendet: ";
                    break;
                case LogType.RECEIVED:
                    prefix = "Empfangen: ";
                    break;
                case LogType.ERROR:
                    prefix = "Fehler: ";
                    break;
            }

            debugLogTextBox.AppendText(prefix + message + Environment.NewLine);
        }

        private void UpdateConnectionState()
        {
            if(InvokeRequired)
            {
                Invoke(new UpdateConnectionStateCallback(UpdateConnectionState), null);
                return;
            }

            bool isConnected = client.IsConnected;

            connectHostTextBox.Enabled = !isConnected;
            connectPortNumericUpDown.Enabled = !isConnected;
            connectButton.Text = (isConnected ? "Trennen" : "Verbinden");

            WriteLog(LogType.STATUS, isConnected ? "Verbindung hergestellt." : "Verbindung beendet.");
        }

        private void client_CommandReceived(CommandReceivedEventArgs e)
        {
            ComputeCommand(e.Command);
        }

        private void client_ConnectionStateChanged(EventArgs e)
        {
            UpdateConnectionState();
        }

        private void ComputeCommand(string command)
        {
            if(InvokeRequired && !IsDisposed)
            {
                Invoke(new ComputeCommandCallback(ComputeCommand), new object[] { command });
                return;
            }

            WriteLog(LogType.RECEIVED, command);

            try
            {
                string[] args = command.Split(',');

                switch(args[0])
                {
                    case "SPED":
                        ProcessSpeedUpdate(int.Parse(args[1]), int.Parse(args[2]));

                        break;
                }
            }
            catch(Exception ex)
            {
                WriteLog(LogType.ERROR, ex.Message);
            }
        }

        private void ProcessSpeedUpdate(int speedLeft, int speedRight)
        {
            double rpmLeft = SpsToRpm(speedLeft);
            double rpmRight = SpsToRpm(speedRight);

            speedLeftLabel.Text = string.Format("{0:0.00} rpm", rpmLeft);
            speedRightLabel.Text = string.Format("{0:0.00} rpm", rpmRight);

            speedLeftBar.Value = (int)Math.Abs(Math.Round(rpmLeft));
            speedRightBar.Value = (int)Math.Abs(Math.Round(rpmRight));

            scene?.ChangeSpeed(rpmLeft, rpmRight);
        }

        private double SpsToRpm(int stepsPerSecond)
        {
            return stepsPerSecond / stepsPerRound * 60.0;
        }

        private async Task ProcessKeyControl()
        {
            if(!client.IsConnected)
                return;

            int speedLeft = 0;
            int speedRight = 0;

            if(keyUpPressed)
                speedLeft = (speedRight = keyControlSpeed);
            else if(keyDownPressed)
                speedLeft = (speedRight = -keyControlSpeed);
            else if(keyLeftPressed)
                speedLeft = -(speedRight = keyControlSpeed);
            else if(keyRightPressed)
                speedLeft = -(speedRight = -keyControlSpeed);

            if((keyUpPressed || keyDownPressed) && keyLeftPressed)
                speedLeft = 0;

            if((keyUpPressed || keyDownPressed) && keyRightPressed)
                speedRight = 0;

            if(speedLeft != lastSpeedLeft || speedRight != lastSpeedRight)
            {
                lastSpeedLeft = speedLeft;
                lastSpeedRight = speedRight;

                string command = string.Format("SACC,{0},{1}", speedRight, speedLeft);
                await client.SendAsync(command);

                WriteLog(LogType.SENT, command);
            }
        }

        private async void connectButton_Click(object sender, EventArgs e)
        {
            try
            {
                if(client.IsConnected)
                {
                    client.Disconnect();

                    scene?.Stop();
                }
                else
                {
                    debugLogTextBox.Clear();

                    scene = new Scene();
                    mapView.ChangeScene(scene);

                    await client.ConnectAsync(connectHostTextBox.Text, (int)connectPortNumericUpDown.Value);
                }
            }
            catch(Exception ex)
            {
                WriteLog(LogType.ERROR, ex.Message);
            }
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(client.IsConnected)
                client.Disconnect();
        }

        private async void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.Up:
                    keyUpPressed = true;
                    break;
                case Keys.Down:
                    keyDownPressed = true;
                    break;
                case Keys.Left:
                    keyLeftPressed = true;
                    break;
                case Keys.Right:
                    keyRightPressed = true;
                    break;
                default:
                    return;
            }

            await ProcessKeyControl();

            e.Handled = true;
        }

        private async void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.Up:
                    keyUpPressed = false;
                    break;
                case Keys.Down:
                    keyDownPressed = false;
                    break;
                case Keys.Left:
                    keyLeftPressed = false;
                    break;
                case Keys.Right:
                    keyRightPressed = false;
                    break;
                default:
                    return;
            }

            await ProcessKeyControl();

            e.Handled = true;
        }
    }
}