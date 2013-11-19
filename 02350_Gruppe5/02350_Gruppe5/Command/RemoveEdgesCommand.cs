using _02350_Gruppe5.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02350_Gruppe5.Command
{
    // Bruges til at fjerne en kant fra canvaset
    public class RemoveEdgesCommand : IUndoRedoCommand
    {
        private ObservableCollection<Edge> edges;
        private Edge removeEdge;

        public RemoveEdgesCommand(ObservableCollection<Edge> _edges, Edge _removeEdge)
        {
            edges = _edges;
            removeEdge = _removeEdge;
        }

        public void Execute()
        {
            edges.Remove(removeEdge);
        }

        public void UnExecute()
        {
            edges.Add(removeEdge);
        }

    }
}
