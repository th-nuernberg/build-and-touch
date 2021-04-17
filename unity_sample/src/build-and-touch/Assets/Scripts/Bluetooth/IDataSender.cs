using System;

namespace Bluetooth
{
    interface IDataSender : IDisposable
    {
        bool Initialize();
        void SendString(FingerType fingerType, byte intensity);
        void SentStopMessage(FingerType fingerType);
        void Close();
    }
}
