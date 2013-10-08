using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace _02350_Gruppe5.Command
{
    // Dette er interfacet der benyttes til at implementere kommandoer, som kan bruges til undo/redo.
    public interface IUndoRedoCommand
    {
        void Execute();
        void UnExecute();
    }
}
