
import cv2
import connection
import mediapipe as mp
import landmark_helper as lh
import handdata_pb2 as hd

# Whether to show the captured image in a window.
# Good for visualization and debugging.
SHOW_WINDOW = False

mp_drawing = mp.solutions.drawing_utils
mp_hands = mp.solutions.hands

hands = mp_hands.Hands(
    min_detection_confidence=0.5, 
    min_tracking_confidence=0.5)

connection.connect()
cap = cv2.VideoCapture(0)

try:
    while cap.isOpened():
        success, image = cap.read()
        if not success:
            print("Ignoring empty camera frame.")
            continue

        # For a selfie-view display, replace the following line. This will flip the image.
        #image = cv2.cvtColor(cv2.flip(image, 1), cv2.COLOR_BGR2RGB)        
        image = cv2.cvtColor(image, cv2.COLOR_BGR2RGB)

        image.flags.writeable = False
        results = hands.process(image)

        image_height, image_width, _ = image.shape

        if results.multi_hand_landmarks:        

            message = hd.Message()

            for hand_idx, hand_landmarks in enumerate(results.multi_hand_landmarks):
                handdata = message.Hands.add()
                
                # Right/left hand detection
                # For a simple case, where it's ensured, that the hands will not cross each other
                # we can just use one finger and check which one is to the right/left side of the other.
                index_x = hand_landmarks.landmark[lh.landmark2id["INDEX_FINGER_TIP"]].x
                pinky_x = hand_landmarks.landmark[lh.landmark2id["PINKY_TIP"]].x
                isRight = index_x > pinky_x
                
                # mediapipe comes with an build in handiness detection, that can be used instead of the custom check
                #hand_label = results.multi_handedness[hand_idx].classification[0].label
                #isRight = True if hand_label == "Right" else False

                handdata.Hand = hd.HandData.HandType.RightHand if isRight else hd.HandData.HandType.LeftHand
            
                for hand_idx in range(0, 21):
                    landmark = handdata.Landmarks.add()
                    landmark.X = hand_landmarks.landmark[hand_idx].x
                    landmark.Y = hand_landmarks.landmark[hand_idx].y
                    landmark.Z  = hand_landmarks.landmark[hand_idx].z
                    landmark.Type = hand_idx
                    
                mp_drawing.draw_landmarks(image, hand_landmarks, mp_hands.HAND_CONNECTIONS)

            connection.send(message.SerializeToString())

        if SHOW_WINDOW:
            cv2.imshow('MediaPipe Hands', image)
            if cv2.waitKey(5) & 0xFF == 27:
                break

finally:
    hands.close()
    cap.release()
    connection.close()
