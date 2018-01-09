#include<stdio.h>
#include<wiringPi.h>

int main()
{
    printf("Setting up wiringPi\n");
    wiringPiSetup();
    printf("Set up\n");
    return 0;
}
