using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Microsoft.Kinect;
using System.Windows.Threading;
using Kinect.Toolbox;

namespace NUIBreak
{
    /// <summary>
    /// Interaction logic for OverlayWindow.xaml
    /// </summary>
    partial class OverlayWindow : Window
    {

        #region Kinect Sensor
        private KinectSensor kinect;

        public KinectSensor Kinect
        {
            get
            {
                return this.kinect;
            }

            set
            {
                this.kinect = value;
            }
        }
        #endregion

        //For Kinect video stream
        private byte[] pixelData = null;
        private WriteableBitmap outputBitmap;
        private static readonly int Bgr32BytesPerPixel = (PixelFormats.Bgr32.BitsPerPixel + 7) / 8;

        StretchDetector stretchDetector;
        private Skeleton[] skeletons;
        SkeletonDisplayManager skeletonDisplayManager;

        DispatcherTimer countdownTimer = new DispatcherTimer();
        int countdownSeconds = 0;

        Queue<string> stretches;

        public OverlayWindow(StretchDetector detector)
        {
            InitializeComponent();
            stretchDetector = detector;
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            countdownTimer.Interval = new TimeSpan(0, 0, 1);
            countdownTimer.Tick += new EventHandler(countdownTimer_Tick);

            Random random = new Random();
            stretches = new Queue<string>(stretchDetector.AvailableStretches.Keys.OrderBy(item => random.Next()).Take(2));

            StartKinect();
            StartNextStretch();
        } 

        void StartKinect()
        {
            foreach (KinectSensor sensor in KinectSensor.KinectSensors)
            {
                if (sensor.Status == KinectStatus.Connected)
                {
                    Kinect = sensor;
                    break;
                }
            }

            if (Kinect == null)
            {
                ColorImage.Visibility = Visibility.Collapsed;
            }
            else
            {
                Kinect.ColorFrameReady += new EventHandler<ColorImageFrameReadyEventArgs>(Kinect_ColorFrameReady);   
                Kinect.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
                outputBitmap = new WriteableBitmap(640, 480, 96, 96, PixelFormats.Bgr32, null);
                ColorImage.Source = outputBitmap;

                //Skeleton detection
                Kinect.SkeletonStream.Enable(new TransformSmoothParameters
                                                   {
                                                 Smoothing = 0.5f,
                                                 Correction = 0.5f,
                                                 Prediction = 0.5f,
                                                 JitterRadius = 0.05f,
                                                 MaxDeviationRadius = 0.04f
                                             });
                Kinect.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(Kinect_SkeletonFrameReady);
                skeletonDisplayManager = new SkeletonDisplayManager(Kinect, SkeletonCanvas);

                Kinect.Start();
            }       
        }

        void StopKinect()
        {
            if (Kinect != null)
            {
                Kinect.ColorFrameReady -= new EventHandler<ColorImageFrameReadyEventArgs>(Kinect_ColorFrameReady);
                Kinect.SkeletonFrameReady -= new EventHandler<SkeletonFrameReadyEventArgs>(Kinect_SkeletonFrameReady);
                Kinect.Stop();
                pixelData = null;
                outputBitmap = null;
            }
        }

        private void StartNextStretch()
        {
            CountdownDisplay.Text = "";

            if (stretches.Count == 0)
            {
                this.Close();
                return;
            }

            stretchDetector.CurrentStretch = stretches.Dequeue();
            Instructions.Text = stretchDetector.AvailableStretches[stretchDetector.CurrentStretch].Description.Replace("\\n", Environment.NewLine);
            stretchDetector.PostureDetected += new Action<string>(stretchDetector_PostureDetected);
        }  

        void stretchDetector_PostureDetected(string obj)
        {
            stretchDetector.PostureDetected -= new Action<string>(stretchDetector_PostureDetected);
            countdownSeconds = 5;
            countdownTimer.Start();
        }

        void countdownTimer_Tick(object sender, EventArgs e)
        {
            if (countdownSeconds >= 0)
            {
                if (stretchDetector.CurrentPosture == stretchDetector.CurrentStretch)
                {
                    CountdownDisplay.Text = countdownSeconds.ToString();
                    countdownSeconds--;
                }
                else
                {
                    CountdownDisplay.Text = "";
                    countdownTimer.Stop();
                    stretchDetector.PostureDetected += new Action<string>(stretchDetector_PostureDetected);
                }
            }
            else
            {
                countdownTimer.Stop();
                StartNextStretch();
            }
        }

        void Kinect_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            if (!Kinect.IsRunning)
                return;

            using (SkeletonFrame frame = e.OpenSkeletonFrame())
            {
                if (frame == null)
                    return;

                Tools.GetSkeletons(frame, ref skeletons);

                if (skeletons.All(s => s.TrackingState == SkeletonTrackingState.NotTracked))
                    return;

                ProcessFrame(frame);
            }
        }

        private void ProcessFrame(SkeletonFrame frame)
        {
            foreach (var skeleton in skeletons)
            {
                if (skeleton.TrackingState != SkeletonTrackingState.Tracked)
                    continue;

                stretchDetector.TrackPostures(skeleton);

                skeletonDisplayManager.Draw(frame);
            }
        }

        void Kinect_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            if (!Kinect.IsRunning)
                return;

            using (ColorImageFrame imageFrame = e.OpenColorImageFrame())
            {
                if (imageFrame != null)
                {
                    if (pixelData == null)
                    {
                        pixelData = new byte[imageFrame.PixelDataLength];
                    }

                    imageFrame.CopyPixelDataTo(this.pixelData);

                    this.outputBitmap.WritePixels(
                        new Int32Rect(0, 0, imageFrame.Width, imageFrame.Height),
                        this.pixelData,
                        imageFrame.Width * Bgr32BytesPerPixel,
                        0);
                }
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            StopKinect();
            base.OnClosing(e);
        }
    }
}
