# HeaterController

Code to control a heater using a Shelly thermometer and a Shelly relay.

Application written in C# reads the temperature from a Shelly temperature sensor, then turns a relay on and off based on the actual temperature and the desired temperature.

The maximum time that the heater stays on is configurable. Also the minimum time that the heater stays off between each two activation is configurable.

Some of best practices in software development are demonstrated in this code including single responsibility principle and inversion of control.

