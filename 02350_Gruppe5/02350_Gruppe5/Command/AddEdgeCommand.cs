using _02350_Gruppe5.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02350_Gruppe5.Command
{
    // Bruges til at tilføje en kant til kant samlingen.
    public class AddEdgeCommand : IUndoRedoCommand
    {
        private ObservableCollection<Edge> edges;
        private Edge edge;

        public AddEdgeCommand(ObservableCollection<Edge> _edges, ClassBox _endA, ClassBox _endB) { edges = _edges; edge = new Edge() { EndA = _endA, EndB = _endB }; }

        public void Execute()
        {
            edges.Add(edge);
        }

        public void UnExecute()
        {
            edges.Remove(edge);
        }
    }
}
