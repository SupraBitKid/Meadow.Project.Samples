﻿using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Temperature;
using Meadow.Hardware;
using System;
using System.Threading.Tasks;
using WifiWeather.Services;
using WifiWeather.ViewModels;
using WifiWeather.Views;

namespace WifiWeather
{
    // public class MeadowApp : App<F7FeatherV1> <- If you have a Meadow F7v1.*
    public class MeadowApp : App<F7FeatherV2>
    {
        RgbPwmLed onboardLed;
        WeatherView displayController;
        AnalogTemperature analogTemperature;

        public override async Task Initialize()
        {
            onboardLed = new RgbPwmLed(
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue);
            onboardLed.SetColor(Color.Red);

            var wifi = Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();

            await wifi.Connect(Secrets.WIFI_NAME, Secrets.WIFI_PASSWORD, TimeSpan.FromSeconds(45));

            analogTemperature = new AnalogTemperature(
                analogPin: Device.Pins.A00,
                sensorType: AnalogTemperature.KnownSensorType.LM35
            );

            displayController = new WeatherView();

            onboardLed.StartPulse(Color.Green);
        }

        async Task GetTemperature()
        {
            onboardLed.StartPulse(Color.Magenta);

            // Get indoor conditions
            var roomTemperature = await analogTemperature.Read();

            // Get outdoor conditions
            var outdoorConditions = await WeatherService.GetWeatherForecast();

            onboardLed.StartPulse(Color.Orange);

            // Format indoor/outdoor conditions data
            var model = new WeatherViewModel(outdoorConditions, roomTemperature);

            // Send formatted data to display to render
            displayController.UpdateDisplay(model);

            onboardLed.StartPulse(Color.Green);
        }

        public override async Task Run()
        {
            await GetTemperature();

            while (true)
            {
                if (DateTime.Now.Minute == 0 && DateTime.Now.Second == 0)
                {
                    await GetTemperature();
                }

                displayController.UpdateDateTime();
                await Task.Delay(TimeSpan.FromMinutes(1));
            }
        }
    }
}