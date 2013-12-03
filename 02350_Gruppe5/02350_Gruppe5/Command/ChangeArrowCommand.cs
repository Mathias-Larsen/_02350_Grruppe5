using _02350_Gruppe5.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02350_Gruppe5.Command
{
    //
    // Class used to remove a ClassBox and connected Edges
    //

    class ChangeArrowCommand : IUndoRedoCommand
    {
        private Edge edge, edgeNew;
        private ObservableCollection<Edge> edges;
        private ClassBox start, end;

        public ChangeArrowCommand(ObservableCollection<Edge> _edges, Edge _edge)
        {
            edges = _edges;
            edge = _edge;
            start = edge.EndA;
            end = edge.EndB;
        }

        public void Execute()
        {
            edges.Remove(edge);
            edgeNew = new Edge(end, start);
            edges.Add(edgeNew);
        }

        public void UnExecute()
        {
            edges.Remove(edgeNew);
            edges.Add(edge);
        }
    }
}
