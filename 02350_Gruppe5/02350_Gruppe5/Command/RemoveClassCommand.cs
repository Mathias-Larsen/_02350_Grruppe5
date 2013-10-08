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
    // Bruges til at fjerne punkt fra punkt samlingen, fjerne også tilhørende kanter.
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
            removeEdges = _edges.Where(x => x.EndA.Number == removeClassBox.Number || x.EndB.Number == removeClassBox.Number).ToList(); 
        }

        public void Execute()
        {
            foreach (Edge e in removeEdges) edges.Remove(e);
            classBoxs.Remove(removeClassBox);
        }

        public void UnExecute()
        {
            classBoxs.Add(removeClassBox);
            foreach (Edge e in removeEdges) {
                if (e.EndA == null) e.EndA = removeClassBox;
                if (e.EndB == null) e.EndB = removeClassBox;
                edges.Add(e);
            }
        }
    }
}
