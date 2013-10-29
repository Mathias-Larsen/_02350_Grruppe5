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
    class PasteClassCommand : IUndoRedoCommand
    {

        private ObservableCollection<ClassBox> classBoxs;
        private ClassBox classBoxToAdd;
        private ClassBox newClassBox;

        public PasteClassCommand(ObservableCollection<ClassBox> _classBoxs, ClassBox _classBox)
        {
            classBoxs = _classBoxs;
            classBoxToAdd = _classBox;
        }

        public void Execute()
        {
            newClassBox = new ClassBox();
            newClassBox.AttNames = classBoxToAdd.AttNames;
            newClassBox.ClassName = classBoxToAdd.ClassName;
            newClassBox.MethodNames = classBoxToAdd.MethodNames;
            newClassBox.Height = classBoxToAdd.Height;
            newClassBox.Width = classBoxToAdd.Width;
            classBoxs.Add(newClassBox);
        }

        public void UnExecute()
        {
            classBoxs.Remove(newClassBox);
        }

    }
}