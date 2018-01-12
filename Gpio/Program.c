#include<stdio.h>
#include<wiringPi.h>

int pinIdx = 1;

int wiringPiSetupCmd = 1;
int pullDownCmd = 2;
int pullUpCmd = 3;
int readModeCmd = 4;
int readDataCmd = 5;

//#########################################
int errorIdx = 1;

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

void pullUp(int pin, int microseconds)
{
    digitalWrite(pin, HIGH);
    delayMicroseconds(microseconds);
}

void readCounts(
    int pin, 
    int numTransitions, 
    int microseconds, 
    int badDataCounts,
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
            if(counts == badDataCounts)
            {
                return;
            }
        }
        lastState = digitalRead(pin);
        output[i+outputOffset] = counts;
    }
}
    
void nextCmd(int numCmds, int * cmds, int * args, int * output)
{ 
    int nextArgIdx = 0;
    int pin = cmds[pinIdx];
    for (int i = 2; i < 2 + numCmds; i++)
    {
        if(cmds[i] == wiringPiSetupCmd)
        {
            piSetup();
        }
        else if(cmds[i] == pullDownCmd)
        {
            pullDown(pin, args[nextArgIdx++]);
        }
        else if(cmds[i] == pullUpCmd)
        {
            pullUp(pin, args[nextArgIdx++]);
        }
        else if(cmds[i] == readModeCmd)
        {
            pinMode(pin, INPUT);
        }       
        else if(cmds[i] == readDataCmd)
        {
            readCounts(
                pin, 
                args[nextArgIdx++],
                args[nextArgIdx++],
                args[nextArgIdx++],
                args[nextArgIdx++],
                output); 
        }
        else
        {
            output[errorIdx] = notImplementedCmdError;
        }
    }
}

int main()
{
    int input_0[3] = {788, 0, 1};
    int output_0[2];
    output_0[0] = input_0[0]; output_0[errorIdx] = 0;
    nextCmd(1, input_0, NULL, output_0);

    while(true)
    {
        int input_1[6] = {1234, 7, 2, 3, 4, 5};
        int inputArgs_1[6] = {18, 40, 85, 1, 255, 2};
        int output_1[100];
        output_1[0] = input_1[0]; output_1[errorIdx] = 0; 
        nextCmd(4, input_1, inputArgs_1, output_1);
        for (int i = 0; i < 87; i++)
        {
            printf("output: %d\t", output_1[i]);
        }
    }

    return 0;
}
