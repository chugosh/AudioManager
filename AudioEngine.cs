using System;
using NAudio.Wave;

namespace AudioManager
{
    public class AudioEngine
    {
        private WaveStream fileStream;
        public event EventHandler<FftEventArgs> FftCalculated;
        public event EventHandler<MaxSampleEventArgs> MaximumCalculated;
        public AudioEngine(string fileName)
		{
            try
            {
                var inputStream = new AudioFileReader(fileName);
                fileStream = inputStream;
				var aggregator = new SampleAggregator(inputStream)
				{
					NotificationCount = inputStream.WaveFormat.SampleRate / 100,
					PerformFFT = true
				};
				aggregator.FftCalculated += (s, a) => FftCalculated?.Invoke(this, a);
                aggregator.MaximumCalculated += (s, a) => MaximumCalculated?.Invoke(this, a);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message, "Problem opening file");
                CloseFile();
            }
        }
        public void Dispose()
        {
            CloseFile();
        }
        private void CloseFile()
        {
            fileStream?.Dispose();
            fileStream = null;
        }

    }
}
