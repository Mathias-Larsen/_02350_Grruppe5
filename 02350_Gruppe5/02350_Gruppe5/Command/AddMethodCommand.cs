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
    // Class used to add methods to a ClassBox
    //

    class AddMethodCommand : IUndoRedoCommand
    {

        private ObservableCollection<ClassBox> classBoxs;
        private ObservableCollection<ClassBox> selectedclassBox;


        public AddMethodCommand(ObservableCollection<ClassBox> _classBoxs,
            ObservableCollection<ClassBox> _selectedClassBox)
        {
            classBoxs = _classBoxs;

            selectedclassBox = _selectedClassBox;
        }

        public void Execute()
        {
            ClassBox classBox = selectedclassBox.ElementAt(0);

            selectedclassBox.Clear();
            classBoxs.Remove(classBox);

            List<ClassBox.attOrMethodName> list = classBox.MethodNamesClass;
            list.Add(new ClassBox.attOrMethodName("New method"));
            classBox.MethodNamesClass = list;

            classBoxs.Add(classBox);
            selectedclassBox.Add(classBox);
        }

        // Unused as it is run through the DataGrid containing the list of attributes
        public void UnExecute()
        {

        }
    }
}
