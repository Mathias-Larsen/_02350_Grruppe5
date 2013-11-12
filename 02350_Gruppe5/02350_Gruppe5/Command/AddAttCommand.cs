using _02350_Gruppe5.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02350_Gruppe5.Command
{
    class AddAttCommand : IUndoRedoCommand
    {

        private ObservableCollection<ClassBox> classBoxs;
        private ObservableCollection<ClassBox> selectedclassBox;


        public AddAttCommand(ObservableCollection<ClassBox> _classBoxs,
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

            List<ClassBox.attOrMethodName> list = classBox.AttNamesClass;
            list.Add(new ClassBox.attOrMethodName("New attribute"));
            classBox.AttNamesClass = list;

            classBoxs.Add(classBox);
            selectedclassBox.Add(classBox);
        }

        public void UnExecute()
        {

        }
    }
}
