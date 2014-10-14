using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenCvSharp;
using System.Threading;

namespace eyesTalk_iteration1
{
    public partial class Form1 : Form
    {
        private Thread cameraThread;
        private CvColor[] colors = new CvColor[]{
                new CvColor(0,0,255),
                new CvColor(0,128,255),
                new CvColor(0,255,255),
                new CvColor(0,255,0),
                new CvColor(255,128,0),
                new CvColor(255,255,0),
                new CvColor(255,0,0),
                new CvColor(255,0,255),
            };

        public Form1()
        {
            InitializeComponent();
            captureCamera();
        }

        private void captureCamera()
        {
            cameraThread = new Thread(new ThreadStart(CaptureCameraCallback));
            cameraThread.Start();
        }

        private void CaptureCameraCallback()
        {
            using (CvCapture cap = CvCapture.FromCamera(CaptureDevice.Any, 1))
            {
                while (CvWindow.WaitKey(20) < 0)
                {
                    IplImage image = cap.QueryFrame();

                    Face face = new Face(ref image);
                    if (face.findObject())
                    {
                        CvRect rect = face.getObjRect(0);
                        image.DrawRect(rect, colors[0]);
                        //image.SetROI(rect);
                        Eyes eyes = new Eyes(ref image);
                        if (eyes.findObject())
                        {
                            CvRect rectR = eyes.getObjRect(0);
                            CvRect rectL = eyes.getObjRect(1);
                            image.DrawRect(rectR, colors[1]);
                            image.DrawRect(rectL, colors[2]);
                        }

                        image.ResetROI();
                    }
                    Bitmap bm = BitmapConverter.ToBitmap(image);
                    bm.SetResolution(pictureBoxCamera.Width, pictureBoxCamera.Height);
                    pictureBoxCamera.Image = bm;
                }

                cameraThread.Abort();

            }
        }
    }
}
