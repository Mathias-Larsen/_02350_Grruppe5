using _02350_Gruppe5.Model;
using _02350_Gruppe5.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace _02350_Gruppe5.Command
{
    //
    // Class used to save an image of the Grid
    //

    class SavePictureCommand
    {
        private Grid screen;
        public SavePictureCommand(StackPanel input)
        {
            // get Grid
            screen = (Grid)input.Children[1];

            // Create a render bitmap and push the surface to it
            RenderTargetBitmap renderBitmap = new RenderTargetBitmap((int)input.ActualWidth, (int)input.ActualHeight, 96d, 96d, PixelFormats.Pbgra32);
            renderBitmap.Render(screen);

            // Configure save file dialog box
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog(); // initialize dialoge
            dlg.FileName = "SceenPrint"; // Default file name
            dlg.DefaultExt = ".png"; // Default file extension
            dlg.Filter = "PNG (.png)|*.png|GIF (.gif)|*.gif|TIFF (.tiff)|*.tiff|All Graphics Types|*.png;*.gif;*.tiff|All Files (*.*)|(.*)"; // Filter files by extension
            dlg.FilterIndex = 1; // Index of starting extension (.png)

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();
            string filename = null;

            // Process save file dialog box results 
            if (result == true)
            {
                // get path and filename
                filename = dlg.FileName;
                string ext = Path.GetExtension(filename);

                // saving the image
                using (FileStream outStream = new FileStream(filename, FileMode.Create))
                {
                    switch (ext.ToLower())
                    {
                        case ".png":
                            // Use png encoder for our data
                            PngBitmapEncoder encoderPng = new PngBitmapEncoder();
                            // push the rendered bitmap to it
                            encoderPng.Frames.Add(BitmapFrame.Create(renderBitmap));
                            // save the data to the stream
                            encoderPng.Save(outStream);
                            break;
                        case ".gif":
                            // Use gif encoder for our data
                            GifBitmapEncoder encoderGif = new GifBitmapEncoder();
                            // push the rendered bitmap to it
                            encoderGif.Frames.Add(BitmapFrame.Create(renderBitmap));
                            // save the data to the stream
                            encoderGif.Save(outStream);
                            break;
                        case ".tiff":
                            // Use tiff encoder for our data
                            TiffBitmapEncoder encoderTiff = new TiffBitmapEncoder();
                            // push the rendered bitmap to it
                            encoderTiff.Frames.Add(BitmapFrame.Create(renderBitmap));
                            // save the data to the stream
                            encoderTiff.Save(outStream);
                            break;
                        default:
                            MessageBox.Show("Not a supported file extension.");
                            break;
                    }
                }
            }
        }
    }
}