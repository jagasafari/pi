#include<stdio.h>
#include<wiringPi.h>
#include "Commands.h"

void readCounts(
    int pin, 
    int numTransitions, 
    int microseconds, 
    int maxCounts,
    int outputOffset,
    int * output)
{
    int lastState = HIGH;
    for(int i = 0; i < numTransitions; i++)
    {
        int counts = 0;
        while(digitalRead(pin) == lastState)
        {       
            counts++;
            if(counts == maxCounts)
            {
                return;
            }
        }
        lastState = digitalRead(pin);
        output[i+outputOffset] = counts;
    }
}

void nextCmd(
    int numCmds, int * cmds, int * args, int * output)
{ 
    int nextArgIdx = 0;
    int pin = cmds[pinIdx];
    for (int i = 2; i < 2 + numCmds; i++)
    {
        if(cmds[i] == wiringPiSetupCmd)
        {
            wiringPiSetup();
        }
        else if(cmds[i] == writeModeCmd)
        {
            pinMode(pin, OUTPUT);
        }
        else if(cmds[i] == pullDownCmd)
        {
            digitalWrite(pin, LOW);
            delay(args[nextArgIdx]);
            nextArgIdx = nextArgIdx + 1;
        }
        else if(cmds[i] == pullUpCmd)
        {
            digitalWrite(pin, HIGH);
            delayMicroseconds(args[nextArgIdx]);
            nextArgIdx = nextArgIdx + 1;
        }
        else if(cmds[i] == readModeCmd)
        {
            pinMode(pin, INPUT);
        }       
        else if(cmds[i] == readDataCmd)
        {

            int numTransitions = args[nextArgIdx];
            nextArgIdx = nextArgIdx + 1;
            int microseconds = args[nextArgIdx];
            nextArgIdx = nextArgIdx + 1;
            int badDataCounts = args[nextArgIdx];
            nextArgIdx = nextArgIdx + 1;
            int outputOffset = args[nextArgIdx];
            nextArgIdx = nextArgIdx + 1;
            readCounts(
                pin, 
                numTransitions,
                microseconds,
                badDataCounts,
                outputOffset,
                output); 
        }
        else
        {
            output[errorIdx] = notImplementedCmdError;
        }
    }
}
