using System;
using NAudio.Dsp;

namespace AudioManager
{
    /// <summary>
    /// 频谱分析
    /// </summary>
	public class SpectrumAnalyser
	{
		private readonly double xScale = 200;
        private const int binsPerPoint = 2;
        private int bins = 512; // bins is half FFT size
        private readonly AudioEngine _audioEngine;
        private double _db = 0;
		private double _fre = 0;

		public SpectrumAnalyser(AudioEngine audioEngine)
		{
            _audioEngine = audioEngine;
            audioEngine.MaximumCalculated += MaximumCalculated;
            audioEngine.FftCalculated += FftCalculated;
        }

		private void FftCalculated(object sender, FftEventArgs e)
		{
            var fftResults = e.Result;
            if (fftResults.Length / 2 != bins)
            {
                bins = fftResults.Length / 2;
            }

            for (int n = 0; n < fftResults.Length / 2; n += binsPerPoint)
            {
                for (int b = 0; b < binsPerPoint; b++)
                {
                    _db += GetDbLog(fftResults[n + b]);
                }
                _fre = CalculateFrequency(n / binsPerPoint);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void MaximumCalculated(object sender, MaxSampleEventArgs e)
		{
			throw new NotImplementedException();
		}
        private double GetDbLog(Complex c)
        {
            // 乘数 10 或者 20
            double intensityDB = 20 * Math.Log10(Math.Sqrt(c.X * c.X + c.Y * c.Y));
            double minDB = -90;
            if (intensityDB < minDB) intensityDB = minDB;
            //double percent = intensityDB / minDB;
            //返回分贝强度
            return intensityDB;
        }
        private double CalculateFrequency(int bin)
        {
            if (bin == 0) return 0;
            return bin * xScale; // Math.Log10(bin) * xScale;
        }

    }

}