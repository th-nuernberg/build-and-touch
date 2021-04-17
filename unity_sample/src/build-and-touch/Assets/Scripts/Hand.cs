using System;
using Bluetooth;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public HandData.Types.HandType type;
    
    private GameObject[] landmarksGo;
    
    public Landmark[] Landmarks { get; set; } = {};
    public bool IsActive { get; set; }
    public void GenerateHand(int landmarkCount = 21)
    {
        if (IsActive)
        {
            Debug.LogWarning($"{nameof(Hand)} has already been initialized. Generation will be skipped.");
            return;
        }
        
        landmarksGo = new GameObject[landmarkCount];
        
        for (var i = 0; i < landmarkCount; i++)
        {
            var landmark = Landmarks[i];

            var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.name = landmark.Type.ToString();
            var sCol = go.GetComponent<SphereCollider>();
            Destroy(sCol);

            go.transform.SetParent(this.transform);

            if (IsTip(landmark))
            {
                var col = go.AddComponent<SphereCollider>();
                col.isTrigger = true;
                var colZone = go.AddComponent<CollisionZone>();
                    
                colZone.isLeft = type == HandData.Types.HandType.LeftHand;
                colZone.finger = ConvertToFingerType(landmark.Type);
            }
                
            go.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
            landmarksGo[i] = go;
        }

        IsActive = true;
    }
    
    private void Update()
    {
        if (!IsActive) return;
        
        Gizmos.color = Color.red;

        var i = 0;
        foreach (var landmark in Landmarks)
        {
            var z = type == HandData.Types.HandType.LeftHand ? landmark.Z : -landmark.Z;
            var pos = new Vector3(landmark.X, -landmark.Y, z);
            landmarksGo[i].transform.localPosition = pos;
            i++;
        }
    }

    private static FingerType ConvertToFingerType(Landmark.Types.LandmarkType landmarkType)
    {
        switch(landmarkType)
        {
            case Landmark.Types.LandmarkType.Wrist:
                return FingerType.Palm;
            case Landmark.Types.LandmarkType.ThumbCmc:
            case Landmark.Types.LandmarkType.ThumbMcp:
            case Landmark.Types.LandmarkType.ThumbIp:
            case Landmark.Types.LandmarkType.ThumbTip:
                return FingerType.Thumb;
            case Landmark.Types.LandmarkType.IndexFingerMcp:
            case Landmark.Types.LandmarkType.IndexFingerPip:
            case Landmark.Types.LandmarkType.IndexFingerDip:
            case Landmark.Types.LandmarkType.IndexFingerTip:
                return FingerType.Index;
            case Landmark.Types.LandmarkType.MiddleFingerMcp:
            case Landmark.Types.LandmarkType.MiddleFingerPip:
            case Landmark.Types.LandmarkType.MiddleFingerDip:
            case Landmark.Types.LandmarkType.MiddleFingerTip:
                return FingerType.Middle;
            case Landmark.Types.LandmarkType.RingFingerMcp:
            case Landmark.Types.LandmarkType.RingFingerPip:
            case Landmark.Types.LandmarkType.RingFingerDip:
            case Landmark.Types.LandmarkType.RingFingerTip:
                return FingerType.Ring;
            case Landmark.Types.LandmarkType.PinkyMcp:
            case Landmark.Types.LandmarkType.PinkyPip:
            case Landmark.Types.LandmarkType.PinkyDip:
            case Landmark.Types.LandmarkType.PinkyTip:
                return FingerType.Pinky;
            default:
                throw new ArgumentOutOfRangeException(nameof(landmarkType), landmarkType, null);
        }
    }

    private static bool IsTip(Landmark landmark)
    {
        switch (landmark.Type)
        {
            case Landmark.Types.LandmarkType.ThumbTip:
            case Landmark.Types.LandmarkType.IndexFingerTip:
            case Landmark.Types.LandmarkType.MiddleFingerTip:
            case Landmark.Types.LandmarkType.RingFingerTip:
            case Landmark.Types.LandmarkType.PinkyTip:
                return true;
            default:
                return false;
        }
    }

       
}