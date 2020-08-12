using Windows.Devices.Pwm;

namespace ESP32Toolset.IO
{
    /// <summary>
    /// Controls an RGB LED attached to 3 GPIO pins
    /// </summary>
    public class RgbLed
    {
        private readonly PwmPin _red;
        private readonly PwmPin _green;
        private readonly PwmPin _blue;

        public RgbLed(PwmPin red, PwmPin green, PwmPin blue)
        {
            _red = red;
            _green = green;
            _blue = blue;

            InitialisePins();
        }

        public RgbLed(int redPin, int greenPing, int bluePin)
        {
            _red = PwmController.GetDefault().OpenPin(redPin);
            _green = PwmController.GetDefault().OpenPin(greenPing);
            _blue = PwmController.GetDefault().OpenPin(bluePin);

            InitialisePins();
        }

        private void InitialisePins()
        {
            _red.Stop();
            _green.Stop();
            _blue.Stop();

            _red.SetActiveDutyCyclePercentage(0);
            _green.SetActiveDutyCyclePercentage(0);
            _blue.SetActiveDutyCyclePercentage(0);
        }

        public void SetRed(double brightness)
        {
            _red.Start();
            _red.SetActiveDutyCyclePercentage(brightness);
        }
    }
}
