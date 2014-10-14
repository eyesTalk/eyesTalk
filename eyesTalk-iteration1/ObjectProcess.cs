using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using OpenCvSharp;

namespace eyesTalk_iteration1
{
    abstract class ObjectProcess
    {
        private readonly IplImage baseImage;
        private readonly double scale;
        private readonly double scaleFactor = 1.0850;
        private readonly int minNeighbors = 2;
        private List<CvRect> rectOfObjects = new List<CvRect>();
        //private List<IplImage> objectImages;

        public ObjectProcess(ref IplImage baseImage)
        {
            this.baseImage = baseImage.Clone();
            this.scale = get_scale();
            this.scaleFactor = get_scaleFactor();
            this.minNeighbors = get_minNeighbors();
        }

        #region "abstract methods"
        protected abstract double get_scale();
        protected abstract double get_scaleFactor();
        protected abstract int get_minNeighbors();
        protected abstract int get_objectNumber();
        protected abstract string get_cascade();
        #endregion

        public CvRect getObjRect(int i)
        {
            if (i > get_objectNumber())
                return CvRect.Empty;
            return rectOfObjects[i];
        }

        public bool findObject()
        {
            bool isObjectFinded = false;
            CheckMemoryLeak();

            using (IplImage smallImg = new IplImage(new CvSize(Cv.Round(baseImage.Width / scale), Cv.Round(baseImage.Height / scale)), BitDepth.U8, 1))
            {
                using (IplImage gray = new IplImage(baseImage.Size, BitDepth.U8, 1))
                {
                    CvRect rect = baseImage.GetROI();
                    baseImage.ResetROI();

                    Cv.CvtColor(baseImage, gray, ColorConversion.BgrToGray);
                    Cv.Resize(gray, smallImg, Interpolation.Linear);
                    Cv.EqualizeHist(smallImg, smallImg);

                    baseImage.SetROI(rect);
                }
                using (CvHaarClassifierCascade cascade = CvHaarClassifierCascade.FromFile(get_cascade()))
                using (CvMemStorage storage = new CvMemStorage())
                {
                    storage.Clear();
                    Stopwatch watch = Stopwatch.StartNew();
                    CvSeq<CvAvgComp> obj = Cv.HaarDetectObjects(smallImg, cascade, storage, scaleFactor, minNeighbors, 0, new CvSize(30, 30));
                    watch.Stop();

                    for (int i = 0; i < obj.Total; i++)
                    {
                        CvRect rect = obj[i].Value.Rect; 
                        rect.X = (int) (rect.X * scale);
                        rect.Y = (int) (rect.Y * scale);
                        rect.Width = (int) (rect.Width * scale);
                        rect.Height = (int) (rect.Height * scale);
                        rectOfObjects.Add(rect);
                    }

                    if (rectOfObjects.Count == get_objectNumber())
                        isObjectFinded = true;
                }

                return isObjectFinded;
            }
        }

        [Conditional("CHECK_MEMORY_LEAK")]
        protected void CheckMemoryLeak()
        {
            while (true)
            {
                Console.WriteLine("{0} Kbytes", GC.GetTotalMemory(false) / 1024);
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                switch (keyInfo.Key)
                {
                    case ConsoleKey.Enter:
                        for (int i = 0; i < 128; i++)
                        {
                            using (CvHaarClassifierCascade cascade = CvHaarClassifierCascade.FromFile(get_cascade()))
                            {
                            }
                            //using (CvMat mat = new CvMat(1, 1024, MatrixType.U8C1))
                            //{                                
                            //}
                        }
                        GC.Collect();
                        break;
                    case ConsoleKey.Escape:
                        goto END_LOOP;
                }

            }
        END_LOOP: ;
        }


    }
}
