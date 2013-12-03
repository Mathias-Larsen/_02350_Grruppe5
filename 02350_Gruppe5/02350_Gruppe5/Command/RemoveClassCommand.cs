using _02350_Gruppe5.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02350_Gruppe5.Command
{
    //
    // Class used to remove a ClassBox and connected Edges
    //

    public class RemoveClassCommand : IUndoRedoCommand
    {
        private ObservableCollection<ClassBox> classBoxs;
        private ObservableCollection<Edge> edges;
        private ClassBox removeClassBox;
        private List<Edge> removeEdges;

        public RemoveClassCommand(ObservableCollection<ClassBox> _classBoxs, ObservableCollection<Edge> _edges, ClassBox _removeClassBox) 
        { 
            classBoxs = _classBoxs; 
            edges = _edges; 
            removeClassBox = _removeClassBox;
            removeEdges = new List<Edge>();
            foreach(Edge e in edges)
            {
                if (e.EndA.Equals(removeClassBox) || e.EndB.Equals(removeClassBox))
                {
                    removeEdges.Add(e);
                }
            }
        }

        public void Execute()
        {
            foreach (Edge e in removeEdges)
            {
                edges.Remove(e);
            }
            classBoxs.Remove(removeClassBox);
        }

        public void UnExecute()
        {
            classBoxs.Add(removeClassBox);
            // Fjerner kanter
            foreach (Edge e in removeEdges) {
                if (e.EndA.Equals(removeClassBox)) e.EndA = removeClassBox;
                if (e.EndB.Equals(removeClassBox)) e.EndB = removeClassBox;
                edges.Add(e);
            }
        }
    }
}
