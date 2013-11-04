using _02350_Gruppe5.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02350_Gruppe5.Command
{
    // Metode til at flytte x,y coor til en classBox
    public class MoveClassBoxCommand : IUndoRedoCommand
    {
        private ClassBox classBox;
        private int x;
        private int y;
        private int newX;
        private int newY;

        public MoveClassBoxCommand(ClassBox _classBox, int _newX, int _newY, int _x, int _y) { 
            classBox = _classBox; 
            newX = _newX; 
            newY = _newY; 
            x = _x; 
            y = _y; 
        }

        public void Execute()
        {
            classBox.X = newX;
            classBox.Y = newY;
        }

        public void UnExecute()
        {
            classBox.X = x;
            classBox.Y = y;
        }
    }
}
