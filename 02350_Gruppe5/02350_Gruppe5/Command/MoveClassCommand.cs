using _02350_Gruppe5.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02350_Gruppe5.Command
{
    //
    // Class used to move the x- and y-coordinates of a ClassBox
    //

    public class MoveClassBoxCommand : IUndoRedoCommand
    {
        private ClassBox classBox;
        private ObservableCollection<Edge> edges;
        private int x;
        private int y;
        private int newX;
        private int newY;

        public MoveClassBoxCommand(ClassBox _classBox, ObservableCollection<Edge> _edges, int _newX, int _newY, int _x, int _y) { 
            classBox = _classBox;
            edges = _edges;
            newX = _newX; 
            newY = _newY; 
            x = _x; 
            y = _y; 
        }

        public void Execute()
        {
            classBox.X = newX;
            classBox.Y = newY;
            
            //Move the edges.
            foreach (Edge edge in edges)
            {
                if (classBox.Equals(edge.EndA))
                {
                    edge.Points = new Edge(classBox, edge.EndB).Points;
                }
                if (classBox.Equals(edge.EndB))
                {
                    edge.Points = new Edge(edge.EndA, classBox).Points;
                }
            }
             
        }

        public void UnExecute()
        {
            classBox.X = x;
            classBox.Y = y;
            
            //Move the edges. 
            foreach (Edge edge in edges)
            {
                if (classBox.Equals(edge.EndA))
                {
                    edge.Points = new Edge(classBox, edge.EndB).Points;
                }
                if (classBox.Equals(edge.EndB))
                {
                    edge.Points = new Edge(edge.EndA, classBox).Points;
                }
            }
            
        }
    }
}
