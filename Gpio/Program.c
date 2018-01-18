#include<stdio.h>

void nextCmd(
    int numCmds, int * cmds, int * args, int * output);

void setup()
{
    int input_0[3] = {788, 0, 1};
    int output_0[2];
    output_0[0] = input_0[0]; output_0[1] = 0;
    nextCmd(1, input_0, NULL, output_0);
}

void dht11()
{
    int input_1[7] = {1234, 7, 6, 2, 3, 4, 5};
    int inputArgs_1[6] = {18, 40, 85, 1, 255, 2};
    while(1)
    {
        int output_1[100];
        output_1[0] = input_1[0]; output_1[1] = 0; 
        nextCmd(4, input_1, inputArgs_1, output_1);
        for (int i = 0; i < 87; i++)
        {
            printf("output: %d\t", output_1[i]);
        }
    }
}

int main()
{
    return 0;
}
