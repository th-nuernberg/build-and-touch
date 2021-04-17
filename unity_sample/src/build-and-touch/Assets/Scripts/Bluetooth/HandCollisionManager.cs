using System;
using UnityEngine;

namespace Bluetooth
{
    public class HandCollisionManager : Singleton<HandCollisionManager>
    {
        private const int _fingerCount = 6; // 5 fingers + palm

        private static readonly TimeSpan _timeoutDelay = TimeSpan.FromMilliseconds(200);

        private readonly DateTime[] _timeoutsLeft = new DateTime[_fingerCount];
        private readonly DateTime[] _timeoutsRight = new DateTime[_fingerCount];

        private readonly bool[] _sentStopMessageLeft = new bool[_fingerCount];
        private readonly bool[] _sentStopMessageRight = new bool[_fingerCount];

        public byte Intensity = 128;
        public string LeftHandComPort;  // TODO: Make readonly at runtime;
        public string RightHandComPort;

        private IDataSender _leftHand;
        private IDataSender _rightHand;


        private bool _leftHandInitialized;
        private bool _rightHandInitialized;

        // Use this for initialization
        void Start()
        {
            _leftHand = new BluetoothDataSender(LeftHandComPort);
            _rightHand = new BluetoothDataSender(RightHandComPort);

            _leftHandInitialized =  _leftHand.Initialize();
            _rightHandInitialized = _rightHand.Initialize();
        }

        // Update is called once per frame
        void Update()
        {
            

            for (int i = 0; i < _fingerCount; i++)
            {
                if (_timeoutsLeft[i] < DateTime.Now && !_sentStopMessageLeft[i])
                {
                    _leftHand.SentStopMessage((FingerType) i);
                    _sentStopMessageLeft[i] = true;
                }
            
                if (_timeoutsRight[i] < DateTime.Now && !_sentStopMessageRight[i])
                {
                    _rightHand.SentStopMessage((FingerType)i);
                    _sentStopMessageRight[i] = true;
                }
            }
        }

        public void NotifyCollisionEnter(Collider collider, CollisionZone collisionZone)
        {

        }

        public void NotifyCollisionStay(Collider collider, CollisionZone collisionZone)
        {
           Debug.Log("Collision Stay: " + collider + " from Left? " + collisionZone.isLeft + " Finger:" + collisionZone.finger);

            IDataSender sender;
            if(TryGetSender(collisionZone, out sender))
            {
                DateTime[] timeouts;
                bool[] stopMessageSent;
                if (collisionZone.isLeft)
                {
                    timeouts = _timeoutsLeft;
                    stopMessageSent = _sentStopMessageLeft;
                }
                else
                {
                    timeouts = _timeoutsRight;
                    stopMessageSent = _sentStopMessageRight;
                }

                var finger = (int)collisionZone.finger;
                if (timeouts[finger] <= DateTime.Now || stopMessageSent[finger])
                {
                    sender.SendString(collisionZone.finger, Intensity);
                    timeouts[finger] = DateTime.Now + _timeoutDelay;
                    stopMessageSent[finger] = false;
                }
            }
        }

        public void NotifyCollisionExit(Collider collider, CollisionZone collisionZone)
        {
        }

        private bool TryGetSender(CollisionZone collisionZone, out IDataSender sender)
        {
            if (collisionZone.isLeft)
            {
                sender = _leftHand;
                return _leftHandInitialized;
            }

            sender = _rightHand;
            return _rightHandInitialized;                  
        }

        private void OnDestroy()
        {
            if(_leftHand != null)
            {
                _leftHand.Close();
            }
            if(_rightHand != null)
            {
                _rightHand.Close();
            }
        }
    }
}