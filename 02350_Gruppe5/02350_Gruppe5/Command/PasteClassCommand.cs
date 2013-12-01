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
    //
    // Class used to add a copy of a ClassBox, assuming that one has been copied
    //

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
            newClassBox = new ClassBox(classBoxs.Count + 1);
            List<ClassBox.attOrMethodName> att = new List<ClassBox.attOrMethodName>();
            List<ClassBox.attOrMethodName> met = new List<ClassBox.attOrMethodName>();
            foreach (ClassBox.attOrMethodName attribut in  classBoxToAdd.AttNamesClass)
            {
                ClassBox.attOrMethodName newAtt = new ClassBox.attOrMethodName(attribut.Name);
                att.Add(newAtt);
            }
            foreach (ClassBox.attOrMethodName method in classBoxToAdd.MethodNamesClass)
            {
                ClassBox.attOrMethodName newMethod = new ClassBox.attOrMethodName(method.Name);
                met.Add(newMethod);
            }

            newClassBox.AttNamesClass = att;
            newClassBox.MethodNamesClass = met;
            newClassBox.ClassName = classBoxToAdd.ClassName;
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