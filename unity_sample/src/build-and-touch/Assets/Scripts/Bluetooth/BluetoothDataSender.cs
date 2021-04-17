using System;
using System.IO.Ports;
using UnityEngine;

namespace Bluetooth
{
    public sealed class BluetoothDataSender : IDataSender
    {
        private const int BaudRate = 9600;

        private readonly string comPort;
        private SerialPort serialPort;
        private bool isInitialized;

        public BluetoothDataSender(string comPortNumber)
        {
            comPort = comPortNumber;
        }

        public void Close()
        {
            if (isInitialized)
                serialPort.Close();
        }

        public void Dispose()
        {
            Close();
        }

        public bool Initialize()
        {
            try
            {
                serialPort = new SerialPort(comPort, BaudRate);
                serialPort.Open();
                isInitialized = true;
            }
            catch (Exception)
            {
                isInitialized = false;
                Debug.LogWarning($"Could not initialize {nameof(BluetoothDataSender)} on {comPort}");
            }

            return isInitialized;
        }

        public void SendString(FingerType fingerType, byte intensity)
        {
            if (!isInitialized)
            {
                Debug.LogWarning($"{nameof(BluetoothDataSender)} is not Initialized!");
                return;
            }


            var channel = (int)fingerType;
            var message = channel + "," + intensity + ";";

            serialPort.Write(message);
        }

        public void SentStopMessage(FingerType fingerType)
        {
            SendString(fingerType, 0);
        }
    }
}
