using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;

namespace _02350_Gruppe5.Model
{
    public class ClassBox : NotifyBase
    {
        private int x, y, number;
        public int Number { get { return number; } private set { number = value; } } // skal bruges til at finde nummeret på en enkelt klasse

        public int X { get { return x; } set { x = value; NotifyPropertyChanged("X"); } } // der skal også være noget notify property changed
        public int Y { get { return y; } set { y = value; NotifyPropertyChanged("Y"); } }
        private int width, height;
        public int Width { get { return width; } set { width = value; NotifyPropertyChanged("Width"); } }
        public int Height { get { return height; } set { height = value; NotifyPropertyChanged("Height"); } }
        public int CenterX { get { return X + Width / 2; } set { X = value - Width / 2; NotifyPropertyChanged("X"); } }
        public int CenterY { get { return Y + Height / 2; } set { Y = value - Height / 2; NotifyPropertyChanged("Y"); } }

        private string className;
        public string ClassName { get { return className; } set { className = value; NotifyPropertyChanged("ClassName"); } }

        //private List<string> attNames, methodNames;
        //public List<string> AttNames { get { return namesAsString(attNamesClass); } set { attNames = value; NotifyPropertyChanged("AttNames"); NotifyPropertyChanged("AttNamesBoxes"); } }
        //public List<string> MethodNames { get { return namesAsString(methodNamesClass); } set { methodNames = value; NotifyPropertyChanged("MethodNames"); } }

        private List<attOrMethodName> attNamesClass, methodNamesClass;
        public List<attOrMethodName> AttNamesClass { get { return attNamesClass; } set { attNamesClass = value; NotifyPropertyChanged("Attribut names"); } }
        public List<attOrMethodName> MethodNamesClass { get { return methodNamesClass; } set { methodNamesClass = value; NotifyPropertyChanged("MethodNames"); } }

        public ClassBox(int num)
        {
            Number = num;
            X = Y = 100; //hvor skal de dukke op?
            Width = Height = 200; //og hvor store skal de være?
            className = "Class name";
            attNamesClass = new List<attOrMethodName>();
            methodNamesClass = new List<attOrMethodName>();
            attNamesClass.Add(new attOrMethodName("att1"));
            attNamesClass.Add(new attOrMethodName("att2"));
            methodNamesClass.Add(new attOrMethodName("met1"));
            methodNamesClass.Add(new attOrMethodName("met2"));

            //attNames = new List<string>();
            //methodNames = new List<string>();
        }
        // ViewModel properties. Den burde være i sin egen ViewModel klasse som Node klasserne wrappes af, men i dette tilfælde er det her meget lettere.
        private bool isSelected;
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                NotifyPropertyChanged("IsSelected");
                NotifyPropertyChanged("SelectedColor");
            }

        }
        public Brush SelectedColor { get { return IsSelected ? Brushes.Blue : Brushes.Black; } }

        /*       private List<attOrMethodName> ListToTextBox(List<string> input){
                   List<attOrMethodName> output = new List<attOrMethodName>();
                   foreach (string att in input) {
                       output.Add(new attOrMethodName(att));
                   }
                   return output;
               }
               */
        public class attOrMethodName
        {
            public attOrMethodName(string _name)
            {
                name = _name;
            }
            private string name;
            public string Name { get { return name; } set { name = value; } }
        }

        public void addAtt(attOrMethodName input)
        {
            attNamesClass.Add(input);
        }

        public void removeAtt(attOrMethodName input)
        {
            attNamesClass.Remove(input);
        }
    }
}
