using _02350_Gruppe5.Model;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02350_Gruppe5.Command
{
    // Bruges til at tilføje punkt til punkt samlingen.
    public class AddClassCommand : IUndoRedoCommand
    {
        private ObservableCollection<ClassBox> classBoxs;
        private ClassBox classBox;

        public AddClassCommand(ObservableCollection<ClassBox> _classBox) { classBoxs = _classBox; }

        public void Execute()
        {
            classBoxs.Add(classBox = new ClassBox());
        }

        public void UnExecute()
        {
            classBoxs.Remove(classBox);
        }
    }
}
