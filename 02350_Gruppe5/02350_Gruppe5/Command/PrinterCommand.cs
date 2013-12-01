using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace _02350_Gruppe5.Command
{
    //
    // Class used to send the Grid to a local printer
    //

    class PrinterCommand
    {
        private Grid screen;
        public PrinterCommand(StackPanel input)
        {
            screen = (Grid)input.Children[1];

            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(screen, "Print Job");
            }
        }
    }
}
