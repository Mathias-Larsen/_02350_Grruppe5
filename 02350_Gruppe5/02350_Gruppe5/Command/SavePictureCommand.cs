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
    class SavePictureCommand
    {
        private Grid screen;
        public SavePictureCommand(StackPanel input)
        {
            screen = (Grid)input.Children[1];


            // Create a render bitmap and push the surface to it
            RenderTargetBitmap renderBitmap = new RenderTargetBitmap((int)input.ActualWidth, (int)input.ActualHeight, 96d, 96d, PixelFormats.Pbgra32);
            renderBitmap.Render(screen);

            // Configure save file dialog box
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "SceenPrint"; // Default file name
            dlg.DefaultExt = ".png"; // Default file extension
            dlg.Filter = "XML documents (.png)|*.png"; // Filter files by extension 

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();
            string filename = null;
            // Process save file dialog box results 

            if (result == true)
            {
                // Save document 
                filename = dlg.FileName;

                using (FileStream outStream = new FileStream(filename, FileMode.Create))
                {
                    // Use png encoder for our data
                    PngBitmapEncoder encoder = new PngBitmapEncoder();
                    // push the rendered bitmap to it
                    encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                    // save the data to the stream
                    encoder.Save(outStream);
                }
            }
        }

    }
}