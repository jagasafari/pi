#include<stdio.h>
#include<wiringPi.h>

void read()
{
    int pin = 4;
    int inMode = 0;
    int outMode = 1;
    int maxRealTime = 99;
    int defaultPriority = 0;
    int lowState = 0;
    int highState = 1;

    pinMode(pin, outMode);
    if (piHiPri(maxRealTime) != 0)
    {
        printf("Error: Max real time priority setup failure\n");
    }
    digitalWrite(pin, lowState);
    delay(2);
    digitalWrite(pin, highState);
    delayMicroseconds(140);
    pinMode(pin, inMode);
    piHiPri(defaultPriority);
    read();
}

int timeStampIdx = 0;
int pinIdx = 1;
int wiringPiSetupCmd = 1;
int pullDownCmd = 2;

int result[6] = {0, 0, 0, 0, 0, 0};
int errorIdx = 1;
int noError = 0;
int notImplementedCmdError = 1;

void piSetup()
{
    printf("Setting up wiringPi\n");
    wiringPiSetup();
    printf("Pi set up completed\n");
}

void pullDown(int pin, int milliseconds)
{
    pinMode(pin, OUTPUT);
    digitalWrite(pin, LOW);
    delay(milliseconds);
}

void executeNextCmd(int size, int * cmds, int * cmdArgs)
{ 
    int nextArgIdx = 0;
    for (int i = 2; i < size; i++)
    {
        if(cmds[i] == wiringPiSetupCmd)
        {
            piSetup();
        }
        else if(cmds[i] == pullDownCmd)
        {
            pullDown(cmds[pinIdx], cmdArgs[nextArgIdx++]);
        }
        else
        {
            result[errorIdx] = notImplementedCmdError;
        }
    }
}

int main()
{
    // 0 -> ticks, 1 -> pin, 2 .. -> cmd
    int cmdInput[5] = {1234, 7, 1, 2, 3};
    int cmdArgs[5] = {0, 0, 0, 0, 0};
    // 0 -> ticks, 1 -> error, 2 .. -> result counts
    result[timeStampIdx] = cmdInput[timeStampIdx]; 
    result[errorIdx] = noError; 
    result[2] = 0; result[3] = 0; result[4] = 0; result[5] = 0;
    executeNextCmd(5, cmdInput, cmdArgs);
    for (int i = 0; i < 5; i++)
    {
        printf("result: %d\t", result[i]);
    }
    return 0;
}
