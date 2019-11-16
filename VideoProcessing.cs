using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using AForge.Video.FFMPEG;

namespace Speed_Painting
{
    public static class VideoProcessing {

        public static bool StartVideoMakingProcess(Bitmap bitmap, Bitmap colorMap, int[,] colorValues, int webMin, int webMax, int FPS, int frameCount, int skipFrames)
        {
            // PROGRESS BAR SETUP
            var progressBarForm = new Form();
            progressBarForm.Show(); 
            progressBarForm.Size = new Size(500, 150);
            progressBarForm.StartPosition = FormStartPosition.Manual;
            progressBarForm.Location = new Point(500, 400);
            progressBarForm.FormBorderStyle = FormBorderStyle.FixedSingle;
            progressBarForm.MaximizeBox = false;
            progressBarForm.MinimizeBox = false;
            progressBarForm.BackColor = Color.Black;

            var progressBar = new ProgressBar();
            progressBarForm.Controls.Add(progressBar);
            progressBar.Size = new Size(474, 25);
            progressBar.Location = new Point(5, 25);
            progressBar.Show();
            progressBar.Minimum = 0;
            progressBar.Step = 1;
            progressBar.Value = 1;

            var progressBar2 = new ProgressBar();
            progressBarForm.Controls.Add(progressBar2);
            progressBar2.Size = new Size(474, 25);
            progressBar2.Location = new Point(5, 5);
            progressBar2.Show();
            progressBar2.Minimum = 0;
            progressBar2.Step = 1;
            progressBar2.Value = 1;

            var progressBarLabel = new Label
            {
                Location = new Point(5, 55),
                Size = new Size(474, 50),
                Text = "wait for it",
                BackColor = Color.Red
            };
            progressBarForm.Controls.Add(progressBarLabel);
            progressBarLabel.Show();

            int skipNow = -1; // for checking if a frame should be skipped
            int skipAt = skipFrames; // skip every x frames to shorten the videos supplied from UI
            var fps = FPS; // supplied from UI
            var videoWriter = new VideoFileWriter();
            
            string path = "Videos\\" + DateTime.Now.Ticks + ".avi";
            videoWriter.Open(path, 1920, 1080, fps, VideoCodec.MSMPEG4v3, 50000000);

            // check if image is 1920 x 1080
            if (bitmap.Height != 1080 && bitmap.Width != 1920)
            {
                // resize it if needed
                bitmap = new Bitmap(bitmap, new Size(1920, 1080));
            }
            
            // create a black and white image 
            Bitmap blackAndWhiteImage = bitmap;
            for (int x=0; x < blackAndWhiteImage.Width; x++)
            {
                for (int y = 0; y < blackAndWhiteImage.Height; y++)
                {
                    // get average color
                    Color oldColor = bitmap.GetPixel(x, y);
                    int averageColor = (oldColor.B + oldColor.G + oldColor.R) / 3;
                    
                    // apply average color
                    Color newColor = Color.FromArgb(255, averageColor, averageColor, averageColor);
                    blackAndWhiteImage.SetPixel(x, y, newColor);
                }
            }

            
            // create cover that will be cleared out slowly to reveal image underneath it 
            Bitmap tempImage = new Bitmap(@"D:\VS19\Speed Painting\Images\2.bmp");

            //Console.WriteLine(tempImage.Size);
            Bitmap whiteCoverImage = new Bitmap(tempImage, blackAndWhiteImage.Width, blackAndWhiteImage.Height);
            Bitmap checkForWhiteCoverImage = new Bitmap(whiteCoverImage);
            using (Graphics gWh = Graphics.FromImage(whiteCoverImage))
            {
                //***************  ADD PAPER TEXTURE LATER ON IF NEEDS BE

                Rectangle ImageSize = new Rectangle(0, 0, 1920, 1080);
                //gWh.FillRectangle(Brushes.White, ImageSize);
            }
            progressBarForm.Refresh();
            // Get hand images
            DirectoryInfo di = new DirectoryInfo(@"D:\VS19\Speed Painting\Images\hands");
            Dictionary<string, Bitmap> handImages = new Dictionary<string, Bitmap>();
            List<string> handImagesNames = new List<string>();
            foreach (var fi in di.GetFiles())
            {
                Console.WriteLine(fi.Name);
                handImages.Add(fi.Name, new Bitmap("D:\\VS19\\Speed Painting\\Images\\hands\\" + fi.Name));
                handImagesNames.Add(fi.Name);
            }
            Point lastPoint = new Point();

            progressBar.Maximum = colorValues.Length/3;

            progressBar2.Maximum = 1080;

            // create an image for output and output it self
            Bitmap outputImage = new Bitmap(1920, 1080);
            using (Graphics gr = Graphics.FromImage(outputImage))
            {
                // Initial setup
                // choose drawingStyle:  left to right    OR     top to bottom
                Random r = new Random();
                int style = 1; // LOCKED FOR NOW 
                int rowsAlreadyChecked = 0; // does not allow checking rows that have already been searched though (speeds up whole process by 10x at least)

                // choose a color value from the list (use first one )
                int currentMapColorIndex = 0;
                Color currentMapColor = Color.FromArgb(colorValues[currentMapColorIndex, 0], colorValues[currentMapColorIndex, 1], colorValues[currentMapColorIndex, 2]);
                
                // start drawing loop
                // frame count supplied from the UI around 30k us 10min video
                for (int i = 0; i < frameCount; i++)
                {
                    // draw b&w image this will always be bottom layer
                    gr.DrawImage(blackAndWhiteImage, 0, 0, 1920, 1080);

                    // make pixels from overlay transparent
                    // follow color map to change pixels in certain areas
                    Point startPosition = new Point(0, 0);
                    
                    NewColor:
                    // Chose drawing style and starting point
                    if (style == 0) // draw left to right
                    {
                        // Go through all the pixels 
                        for (int x = 0; x < 1920; x++)
                        {
                            for (int y = 0; y < 1080; y++)
                            {
                                //Console.WriteLine(currentMapColor.R + " " + currentMapColor.G + " " + currentMapColor.B);
                                // look for pixel that is not fully transparent and is at correct color in color map
                                if (whiteCoverImage.GetPixel(x, y).A != 0
                                    && colorMap.GetPixel(x, y).R == currentMapColor.R
                                    && colorMap.GetPixel(x, y).G == currentMapColor.G
                                    && colorMap.GetPixel(x, y).B == currentMapColor.B)
                                {
                                    // start drawing from this point
                                    //Console.WriteLine("THIS SHOULD BE 54 is it ?:" + colorMap.GetPixel(x, y).R);
                                    startPosition.X = x;
                                    startPosition.Y = y;
                                    goto PointChoosen;
                                }
                            }
                        }
                    }
                    else // draw top to bottom
                    {
                        for (int y = rowsAlreadyChecked; y < 1080; y++)
                        {
                            for (int x = 0; x < 1920; x++)
                            {
                               if (whiteCoverImage.GetPixel(x, y).A != 0
                                    && colorMap.GetPixel(x, y).R == currentMapColor.R
                                    && colorMap.GetPixel(x, y).G == currentMapColor.G
                                    && colorMap.GetPixel(x, y).B == currentMapColor.B)
                                {
                                    // start drawing from this point
                                    startPosition.X = x;
                                    startPosition.Y = y;
                                    rowsAlreadyChecked = y;
                                    progressBar2.Value = y;
                                    goto PointChoosen;
                                }
                            }
                        }
                    }

                    if (currentMapColorIndex + 1 != colorValues.GetLength(0))
                    {
                        currentMapColorIndex++;
                        rowsAlreadyChecked = 0;
                        progressBar.Value = currentMapColorIndex;
                        progressBarLabel.Text = currentMapColorIndex + " / " + progressBar.Maximum;
                        progressBarForm.Refresh();
                    }
                    else
                    {
                        videoWriter.Close();
                        return true;
                    }
                    currentMapColor = Color.FromArgb(colorValues[currentMapColorIndex, 0], colorValues[currentMapColorIndex, 1], colorValues[currentMapColorIndex, 2]);

                    // try next color
                    goto NewColor;



                    PointChoosen: // now draw pixels 
                                  // pixels to be drawn ranging from:
                    int reachMin = webMin; // supplied from UI
                    int reachMax = webMax; // supplied from UI
                    int xReach = r.Next(reachMin, reachMax);
                    int yReach = r.Next(reachMin, reachMax);
                    for (int l = 0; l < xReach; l++)
                    {
                        for (int k = 0; k < yReach; k++)
                        {
                            if (startPosition.X + l >= 0 && startPosition.X + l < 1920) // check for outofbound 
                            {
                                if (startPosition.Y + k >= 0 && startPosition.Y + k < 1080) // check for outofbound 
                                {
                                    // changing alpha of the pixel
                                    Color ctc = Color.FromArgb(0, whiteCoverImage.GetPixel(startPosition.X + l, startPosition.Y + k));
                                    whiteCoverImage.SetPixel(startPosition.X + l, startPosition.Y + k, ctc);

                                    // make pixels surrounding it a bit transparent as well 
                                    for (int c = -1; c < 2; c++)
                                    {
                                        for (int v = -1; v < 2; v++)
                                        {
                                            int xx = startPosition.X + c + l;
                                            int yy = startPosition.Y + v + k;
                                            if (xx >= 0 && xx < 1920) // check for outofbound 
                                            {
                                                if (yy >= 0 && yy < 1080) // check for outofbound 
                                                {
                                                    if (whiteCoverImage.GetPixel(xx, yy).A == 255) // check if pixels alpha is needed to change
                                                    {
                                                        Color ctc2 = Color.FromArgb(r.Next(0, 150), whiteCoverImage.GetPixel(xx, yy));
                                                        whiteCoverImage.SetPixel(xx, yy, ctc2);
                                                        lastPoint.X = xx;
                                                        lastPoint.Y = yy;
                                                    }

                                                }
                                            }
                                        }
                                    }
                                }
                            }


                            yReach = r.Next(reachMin, reachMax);
                        }
                        xReach = r.Next(reachMin, reachMax);
                    }



                    // assign finished white cover for next check
                    checkForWhiteCoverImage = whiteCoverImage;

                    
                    if(skipNow == -1)
                    {
                        // draw the cover image
                        gr.DrawImage(whiteCoverImage, new Point(0, 0));


                        // Add hand pic **************************************
                        int randomHand = r.Next(0, handImages.Count);
                        Bitmap handPic = handImages[handImagesNames[randomHand]];
                        char[] delimiterChars = { '_', '.' };
                        string[] positions = handImagesNames[randomHand].Split(delimiterChars);
                        int handX = lastPoint.X - Int32.Parse(positions[0]);
                        int handY = lastPoint.Y - Int32.Parse(positions[1]);

                        gr.DrawImage(handPic, handX, handY, handPic.Width, handPic.Height);

                        // add the picture into video as a frame *************************
                        videoWriter.WriteVideoFrame(outputImage);
                        skipNow++;
                        
                    }
                    else
                    {
                        skipNow++;
                        if(skipNow >= skipAt)
                        {
                            skipNow = -1; // next frame will be written
                        }

                    }
                }
                videoWriter.Close();
                progressBar.Dispose();
                progressBarForm.Dispose();
            }
            
            return false; // should not get here 
        }
        
    }
}
