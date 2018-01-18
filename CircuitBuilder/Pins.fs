module Pins

open System.Collections.Generic

type PinKey = | Pin_1 | Pin_2 | SDA0 | Pin_4 | SCL0 | Pin_6 | GPIO7 | TXD | Pin_9 | RXD | GPIO0 | GPIO1 | GPIO2 | Pin_14 | GPIO3 | GPIO4 | Pin_17 | GPIO5 | MOSI | Pin_20 | MISO | GPIO6 | SCLK | CE0 | Pin_25 | CE1 | SDA_0 | SCL_0 | GPIO21 | Pin_30 | GPIO22 | GPIO26 | GPIO23 | Pin_34 | GPIO24 | GPIO27 | GPIO25 | GPIO28 | Pin_39 | GPIO29

type PinCategory = | Power3_3 | Power5 | Low_High | Ground | CLK | I2C | SPI | UART | PWM

let _pinDict = Dictionary<PinKey, int * int option * PinCategory>()
