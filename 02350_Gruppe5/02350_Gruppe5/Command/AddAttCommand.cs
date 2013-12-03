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
    // Class used to add method to a ClassBox
    //

    class AddAttCommand : IUndoRedoCommand
    {

        private ObservableCollection<ClassBox> classBoxs;
        private ObservableCollection<ClassBox> selectedclassBox;
        private ClassBox classBox;
        List<ClassBox.attOrMethodName> oldList; //Used when undo
        List<ClassBox.attOrMethodName> newList; 


        public AddAttCommand(ObservableCollection<ClassBox> _classBoxs,
            ObservableCollection<ClassBox> _selectedClassBox)
        {
            classBoxs = _classBoxs;
            selectedclassBox = _selectedClassBox;
            classBox = selectedclassBox.ElementAt(0);
            oldList = new List<ClassBox.attOrMethodName>();
            newList = new List<ClassBox.attOrMethodName>();

            foreach (ClassBox.attOrMethodName att in classBox.AttNamesClass)
            {
                oldList.Add(att);
                newList.Add(att);
            }
            newList.Add(new ClassBox.attOrMethodName("+ new Attribute : Type"));
        }

        public void Execute()
        {

            selectedclassBox.Clear();
            classBoxs.Remove(classBox);
      
            classBox.AttNamesClass = newList;

            classBoxs.Add(classBox);
            selectedclassBox.Add(classBox);
        }

        public void UnExecute()
        {
            selectedclassBox.Clear();
            classBoxs.Remove(classBox);
            classBox.AttNamesClass = oldList;

            classBoxs.Add(classBox);
            selectedclassBox.Add(classBox);

        }
    }
}
