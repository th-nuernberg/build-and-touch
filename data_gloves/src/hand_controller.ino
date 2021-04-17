/**
* Map String value to analog ping
* esp32 has no analogWrite, use ledcWrite
*/
#include "BluetoothSerial.h"
#include "Arduino.h"

uint8_t THUMB = 0;
uint8_t INDEX = 1;
uint8_t MIDDLE = 2;
uint8_t RING = 3;
uint8_t PINKY = 4;
uint8_t PALM = 5;

int fingerPins[] = { 12, 13, 14, 25, 26, 27 };

BluetoothSerial SerialBT;
String line;

void setup() 
{
    SerialBT.begin("controller-right");
    
    // binding fingers (channel) to GPIO pins
    mapChannelToGPIO(THUMB, fingerPins[THUMB]);
    mapChannelToGPIO(INDEX, fingerPins[INDEX]);
    mapChannelToGPIO(MIDDLE, fingerPins[MIDDLE]);
    mapChannelToGPIO(RING, fingerPins[RING]);
    mapChannelToGPIO(PINKY, fingerPins[PINKY]);
    mapChannelToGPIO(PALM, fingerPins[PALM]);
    
    // set pinmode
    for (int i=0; i<sizeof(fingerPins); i++)
    {
        pinMode(fingerPins[i], OUTPUT);
    }
}

/**
* channel = channel for ledcWrite esp32 - pin = GPIO with vibrator
*/
void mapChannelToGPIO(int channel, int pin)
{
    ledcSetup(channel, 5000, 8);
    ledcAttachPin(pin, channel);
}

void loop() 
{
    if(SerialBT.available())
    {
        char input = SerialBT.read();
        if(input != ';' && input != '\n')
        {
            line += input;
        }
        else 
        { 
            controlVib(line); line = ""; 
        }
    }
}

void btPrint(String text)
{
    for(int i=0; i<text.length(); i++){
        SerialBT.write(text[i]);
    }
}

void btPrintln(String text)
{
    btPrint(text);
    SerialBT.write('\n');
}

/**
* valuePair = CHANNEL, INTENSITY - e.g. 0,255 - only one at a time!
*/
void controlVib(String valuePair)
{
    String s_channel = getValue(valuePair, ',', 0);
    String s_intensity = getValue(valuePair, ',', 1);

    int channel = s_channel.toInt();
    int intensity = s_intensity.toInt();

    btPrintln(s_channel + "," + s_intensity);

    if (channel >= 0
        && channel < sizeof(fingerPins)
        && intensity >= 0
        && intensity <= 255)
    {
        ledcWrite(channel, intensity); 
    }
}

// https://stackoverflow.com/questions/9072320/split-string-into-string-array/14824108#14824108
String getValue(String data, char separator, int index)
{
    int found = 0;
    int strIndex[] = {0, -1};
    int maxIndex = data.length()-1;
    for (int i=0; i<=maxIndex && found<=index; i++)
    {
        if (data.charAt(i)==separator || i==maxIndex)
        {
            found++;
            strIndex[0] = strIndex[1]+1;
            strIndex[1] = (i == maxIndex) ? i+1 : i;
        }
    }
    return found>index ? data.substring(strIndex[0], strIndex[1]) : "";
}