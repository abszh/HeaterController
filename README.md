# Heater Controller

This is an application to control a heater using a Shelly temperature sensor and a Shelly relay. See https://www.shelly.com/en.

The code is written in C#. The application reads the temperature from a Shelly temperature sensor, then turns a relay on or off based on the actual temperature and the desired temperature.

The maximum time that the heater stays on is configurable. Also the minimum time that the heater stays off between each two activation is configurable.

Some of best practices in software development are demonstrated in this code including single responsibility principle and inversion of control.

## Motivation

In my home, the control unit of the gas heater is in living room. At night there is no one in the living room. I can save a lot of energy if I close the heater vents in the living room. However by doing so, the heater would never turn off.

To solve this issue, I have put a Shelly temperature sensor in the kids bedroom. I have also made some changes to the heater so that i‌t can be turned on and off using a Shelly relay. At night I run this application so that the heater is turned on and off based on the temperature of the kids' bedroom and not the living room

## How to use

After building the application from the source code, run it in command prompt with the following syntax.

```ps1
HeaterController <relayIpAddress> <sensorIpAddress>
```

### Example

```ps1
HeaterController 10.0.0.101 10.0.0.102
```

