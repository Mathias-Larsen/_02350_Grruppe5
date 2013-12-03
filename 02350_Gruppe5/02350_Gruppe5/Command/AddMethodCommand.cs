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

    class AddMethodCommand : IUndoRedoCommand
    {

        private ObservableCollection<ClassBox> classBoxs;
        private ObservableCollection<ClassBox> selectedclassBox;
        private ClassBox classBox;
        List<ClassBox.attOrMethodName> oldList; //Used when undo
        List<ClassBox.attOrMethodName> newList;


        public AddMethodCommand(ObservableCollection<ClassBox> _classBoxs,
            ObservableCollection<ClassBox> _selectedClassBox)
        {
            classBoxs = _classBoxs;
            selectedclassBox = _selectedClassBox;
            classBox = selectedclassBox.ElementAt(0);
            oldList = new List<ClassBox.attOrMethodName>();
            newList = new List<ClassBox.attOrMethodName>();

            foreach (ClassBox.attOrMethodName att in classBox.MethodNamesClass)
            {
                oldList.Add(att);
                newList.Add(att);
            }
            newList.Add(new ClassBox.attOrMethodName("+ new Method( ) : ReturnType"));
        }

        public void Execute()
        {

            selectedclassBox.Clear();
            classBoxs.Remove(classBox);

            classBox.MethodNamesClass = newList;

            classBoxs.Add(classBox);
            selectedclassBox.Add(classBox);
        }

        public void UnExecute()
        {
            selectedclassBox.Clear();
            classBoxs.Remove(classBox);
            classBox.MethodNamesClass = oldList;

            classBoxs.Add(classBox);
            selectedclassBox.Add(classBox);

        }
    }
}
