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
    // Class used to change the text in an attribute of method, of a ClassBox, to Italic, currently unused TODO
    //

    class ItalicTextCommand : IUndoRedoCommand
    {
        private ObservableCollection<ClassBox> classBoxs;
        private ObservableCollection<ClassBox> selectedClassBox;

        public ItalicTextCommand(ObservableCollection<ClassBox> _classBoxs, ObservableCollection<ClassBox> _selectedClassBox)
        {
            classBoxs = _classBoxs;
            selectedClassBox = _selectedClassBox;
        }

        public void Execute()
        {
            ClassBox classBox = selectedClassBox.ElementAt(0);

            selectedClassBox.Clear();
            classBoxs.Remove(classBox);


        }

        public void UnExecute()
        {

        }
    }
}
