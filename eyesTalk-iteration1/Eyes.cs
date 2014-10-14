using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenCvSharp;

namespace eyesTalk_iteration1
{
    class Eyes : ObjectProcess
    {
        private const double scale = 2;
        private const double scaleFactor = 2.5;
        private const int minNeighbors = 2;
        private const int objectNumber = 2;
        private CvRect rigtEye;
        private CvRect leftEye;
        public Eyes(ref IplImage frame) //frame must be colored image.
            : base(ref frame)
        { }

        #region "override"
        protected override double get_scale() { return scale; }
        protected override double get_scaleFactor() { return scaleFactor; }
        protected override int get_minNeighbors() { return minNeighbors; }
        protected override int get_objectNumber() { return objectNumber; }
        protected override string get_cascade() { return Const.eye; }
        #endregion

        //public void takeTemp()
        public bool detectBlink()
        {
            return false;
        }
        private void detectStateOfEyes()
        { }
    }
}
