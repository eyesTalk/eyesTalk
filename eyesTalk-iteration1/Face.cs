using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenCvSharp;

namespace eyesTalk_iteration1
{
    class Face : ObjectProcess
    {
        private const double scale = 5;
        private const double scaleFactor = 1.0850;
        private const int minNeighbors = 2;
        private const int objectNumber = 1;
        public Face(ref IplImage frame) //frame must be colored image.
            : base(ref frame)
        { }


        #region "abstract override func"
        protected override double get_scale() { return scale; }
        protected override double get_scaleFactor() { return scaleFactor; }
        protected override int get_minNeighbors() { return minNeighbors; }
        protected override int get_objectNumber() { return objectNumber; }
        protected override string get_cascade() { return Const.frontalface_default; }
        #endregion



    }
}
