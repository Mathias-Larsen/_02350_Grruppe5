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
    // Class used to add attributes to a ClassBox
    //

    class AddAttCommand : IUndoRedoCommand
    {

        private ObservableCollection<ClassBox> classBoxs;
        private ObservableCollection<ClassBox> selectedclassBox;
        private ClassBox oldClass;
        private ClassBox newClass;
        List<ClassBox.attOrMethodName> oldList;


        public AddAttCommand(ObservableCollection<ClassBox> _classBoxs,
            ObservableCollection<ClassBox> _selectedClassBox)
        {
            classBoxs = _classBoxs;
            selectedclassBox = _selectedClassBox;
            oldClass = selectedclassBox.ElementAt(0);
        }

        public void Execute()
        {
            newClass = selectedclassBox.ElementAt(0);

            selectedclassBox.Clear();
            classBoxs.Remove(oldClass);

            List<ClassBox.attOrMethodName> list = newClass.AttNamesClass;
            oldList = list;
            list.Add(new ClassBox.attOrMethodName("New attribute"));
            newClass.AttNamesClass = list;

            classBoxs.Add(newClass);
            selectedclassBox.Add(newClass);
        }

        // Unused as it is run through the DataGrid containing the list of attributes
        public void UnExecute()
        {

        }
    }
}
