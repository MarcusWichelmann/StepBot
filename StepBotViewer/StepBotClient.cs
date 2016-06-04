using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StepBotViewer
{
	public class StepBotClient
	{
		public bool IsConnected
		{
			get
			{
				return tcpClient != null && tcpClient.Connected;
			}
		}

		public event CommandReceivedEventHandler CommandReceived;
		public event ConnectionStateChangedEventHandler ConnectionStateChanged;

		public delegate void CommandReceivedEventHandler(CommandReceivedEventArgs e);
		public delegate void ConnectionStateChangedEventHandler(EventArgs e);

		private TcpClient tcpClient;
		private NetworkStream stream;

		private Thread receiveThread;

		private string pendingCommand;

		private bool closeConnection = false;

		public async Task ConnectAsync(string host, int port)
		{
			tcpClient = new TcpClient();

			await tcpClient.ConnectAsync(host, port);

			if(!tcpClient.Connected)
				return;

			closeConnection = false;

			stream = tcpClient.GetStream();

			receiveThread = new Thread(Receive);
			receiveThread.Start();

			ConnectionStateChanged?.Invoke(new EventArgs());
		}

		public void Disconnect()
		{
			closeConnection = true;
			tcpClient.Close();

			ConnectionStateChanged?.Invoke(new EventArgs());
		}

		private void Receive()
		{
			while(!closeConnection)
			{
				try
				{
					int b;
					while((b = stream.ReadByte()) != -1 && !closeConnection)
					{
						char c = (char)b;

						if(c == '\n')
						{
							CommandReceived?.Invoke(new CommandReceivedEventArgs(pendingCommand.TrimEnd('\r')));
							pendingCommand = string.Empty;
						}
						else
						{
							pendingCommand += c;
						}
					}
				}
				catch
				{
					if(!closeConnection)
						throw;
				}
			}
		}
	}

	public class CommandReceivedEventArgs
	{
		public string Command { get; private set; }

		public CommandReceivedEventArgs(string command)
		{
			Command = command;
		}
	}
}
