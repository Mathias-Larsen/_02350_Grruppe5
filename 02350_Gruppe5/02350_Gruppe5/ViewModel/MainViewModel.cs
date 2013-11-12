using _02350_Gruppe5.Command;
using _02350_Gruppe5.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace _02350_Gruppe5.ViewModel
{
    /// <summary>
    /// Denne ViewModel er bundet til MainWindow.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        // Holder styr på undo/redo.
        private UndoRedoController undoRedoController = UndoRedoController.GetInstance();

        // Holder om en kant er ved at blive tilføjet
        private bool isAddingEdge;
        // Første endepunkt når en kant er ved at blive tilføjet
        private ClassBox addingEdgeEndA;
      
        // Gemmer det første punkt som punktet har under en flytning.
        private Point moveClassBoxPoint;
        private Point offsetPosition;
        private double oldPosX;
        private double oldPosY;



        public ObservableCollection<ClassBox> ClassBoxs { get; set; }
        public ObservableCollection<Edge> Edges { get; set; }
        public ObservableCollection<ClassBox> SelectedClassBox { get; set; }
        public ClassBox toPaste;
               

        // Kommandoer som UI bindes til.
        public ICommand UndoCommand { get; private set; }
        public ICommand RedoCommand { get; private set; }

        // Kommandoer som UI bindes til.
        public ICommand AddClassCommand { get; private set; }
        public ICommand RemoveClassCommand { get; private set; }
        public ICommand AddEdgeCommand { get; private set; }
        public ICommand RemoveEdgesCommand { get; private set; }

        // Kommandoer som UI bindes til.
        public ICommand MouseDownClassBoxCommand { get; private set; }
        public ICommand MouseMoveClassBoxCommand { get; private set; }
        public ICommand MouseUpClassBoxCommand { get; private set; }

        public ICommand PasteClassCommand { get; private set; }
        public ICommand CopyClassCommand { get; private set; }
        public ICommand SaveProgram { get; private set; }
        public ICommand OpenProgram { get; private set; }

        public ICommand AddMethodComm { get; private set; }
        public ICommand RemoveMethod { get; private set; }
        public ICommand AddAttComm { get; private set; }
        public ICommand RemoveAtt { get; private set; }

        public MainViewModel()
        {
            
            SelectedClassBox = new ObservableCollection<ClassBox>();
            ClassBoxs = new ObservableCollection<ClassBox>();
            Edges = new ObservableCollection<Edge>();

            /* ClassBoxs = new ObservableCollection<ClassBox>() { 
                new ClassBox(1) { X = 30, Y = 40, Width = 80, Height = 80 }, 
                new ClassBox(2) { X = 140, Y = 230, Width = 100, Height = 100 } };
            
            // ElementAt() er en LINQ udvidelses metode som ligesom mange andre kan benyttes på stort set alle slags kollektioner i .NET.
            Edges = new ObservableCollection<Edge>() { 
                new Edge(ClassBoxs.ElementAt(0), ClassBoxs.ElementAt(1)) };
            */
            // Kommandoerne som UI kan kaldes bindes til de metoder der skal kaldes. Her vidersendes metode kaldne til UndoRedoControlleren.
            UndoCommand = new RelayCommand(undoRedoController.Undo, undoRedoController.CanUndo);
            RedoCommand = new RelayCommand(undoRedoController.Redo, undoRedoController.CanRedo);

            
            // Kommandoerne som UI kan kaldes bindes til de metoder der skal kaldes.
            AddClassCommand = new RelayCommand(AddClassBox);
            RemoveClassCommand = new RelayCommand(RemoveClassBox, SelectedClass);
            AddEdgeCommand = new RelayCommand(AddEdge);
            RemoveEdgesCommand = new RelayCommand<IList>(RemoveEdges, CanRemoveEdges);

            // Kommandoerne som UI kan kaldes bindes til de metoder der skal kaldes.
            MouseDownClassBoxCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownClassBox);
            MouseMoveClassBoxCommand = new RelayCommand<MouseEventArgs>(MouseMoveClassBox);
            MouseUpClassBoxCommand = new RelayCommand<MouseButtonEventArgs>(MouseUpClassBox);

            CopyClassCommand = new RelayCommand(CopyClass, SelectedClass);
            PasteClassCommand = new RelayCommand(PasteClass, CanPaste);
            SaveProgram = new RelayCommand(saveProgram);
            OpenProgram = new RelayCommand(openProgram);

            AddMethodComm = new RelayCommand(addMethod, SelectedClass);
            RemoveMethod = new RelayCommand<IList>(removeMethod);
            AddAttComm = new RelayCommand(addAtt, SelectedClass);
            RemoveAtt = new RelayCommand<IList>(removeAtt);
           
        }
        //MessageBox.Show("hej");
        public void addAtt()
        {

            undoRedoController.AddAndExecute(new AddAttCommand(ClassBoxs, SelectedClassBox));
        }
        public void addMethod()
        {
            undoRedoController.AddAndExecute(new AddMethodCommand(ClassBoxs, SelectedClassBox));
        }
        public void removeMethod(IList _met)
        {
           // List<ClassBox.attOrMethodName> method = _met.Cast<ClassBox.attOrMethodName>().ToList();
           // ClassBox cb = SelectedClassBox.ElementAt(0);
            MessageBox.Show(_met.Count + "");
            /*          foreach(ClassBox.attOrMethodName meString in cb.MethodNamesClass)
                      {
                          if(meString.Equals(method.ElementAt(0)))
                          {
                              MessageBox.Show(method.ElementAt(0));
                          }
                      }*/
        }
        public bool canRemoveAtt(IList _att)
        {
            return _att.Count == 1;
        }
        public void removeAtt(IList _att)
        {
            if (_att == null) { MessageBox.Show("null"); }
            MessageBox.Show("her: " + _att.ToString());
            undoRedoController.AddAndExecute(new RemoveAttCommand(SelectedClassBox.ElementAt(0), _att.Cast<ClassBox.attOrMethodName>().First()));
        }
        public void saveProgram()
        {
            new SaveCommand(ClassBoxs, Edges);
        }
        public void openProgram()
        {
            new OpenCommand(ClassBoxs, Edges);
        }
        // Tilføjer punkt med kommando.
        public void AddClassBox()
        {
            undoRedoController.AddAndExecute(new AddClassCommand(ClassBoxs));
        }
        public void PasteClass()
        {
            undoRedoController.AddAndExecute(new PasteClassCommand(ClassBoxs,toPaste));
            toPaste = null;
        }
        public void CopyClass()
        {
           
            toPaste = SelectedClassBox.ElementAt(0);
            //MessageBox.Show("hello");
        }
        public bool SelectedClass()
        {
            return SelectedClassBox.Count == 1;
        }
        public bool CanPaste()
        {
            return toPaste != null;
        }

        // Fjerner valgte punkter med kommando.
        public void RemoveClassBox()
        {
            undoRedoController.AddAndExecute(new RemoveClassCommand(ClassBoxs, Edges, SelectedClassBox.ElementAt(0)));
            SelectedClassBox.Clear();
        }

        // Starter proceduren der tilføjer en kant.
        public void AddEdge()
        {
            if (SelectedClassBox.Count >= 1)
            {
                SelectedClassBox.ElementAt(0).IsSelected = false;
            }
            SelectedClassBox.Clear();
            isAddingEdge = true;
            RaisePropertyChanged("ModeOpacity");
        }

        // Tjekker om valgte kant/er kan fjernes. Det kan de hvis der er nogle der er valgt.
        public bool CanRemoveEdges(IList _edges)
        {
            return _edges.Count > 0;
        }

        // Fjerner valgte kanter med kommando.
        public void RemoveEdges(IList _edges)
        {
            undoRedoController.AddAndExecute(new RemoveEdgesCommand(Edges, _edges.Cast<Edge>().ToList()));
        }

        // Hvis der ikke er ved at blive tilføjet en kant så fanges musen når en musetast trykkes ned. Dette bruges til at flytte punkter.
        public void MouseDownClassBox(MouseButtonEventArgs e)
        {
            if (!isAddingEdge)
            {
                e.MouseDevice.Target.CaptureMouse();
                FrameworkElement movingClass = (FrameworkElement)e.MouseDevice.Target;
                ClassBox movingClassBox = (ClassBox)movingClass.DataContext;
                Canvas canvas = FindParentOfType<Canvas>(movingClass);
                offsetPosition = Mouse.GetPosition(canvas);
                oldPosX = movingClassBox.X;
                oldPosY = movingClassBox.Y;
                movingClassBox.IsSelected = true;
               

                if (SelectedClassBox.Count == 0)
                {
                    SelectedClassBox.Add(movingClassBox);
                }
                else if (movingClassBox != SelectedClassBox.ElementAt(0))
                {
                    SelectedClassBox.ElementAt(0).IsSelected = false;
                    SelectedClassBox.Clear();
                    SelectedClassBox.Add(movingClassBox);
                }

            }
        }

        // Bruges til at flytter punkter.
        public void MouseMoveClassBox(MouseEventArgs e)
        {
            // Tjek at musen er fanget og at der ikke er ved at blive tilføjet en kant.
            if (Mouse.Captured != null && !isAddingEdge)
            {
                FrameworkElement movingClass = (FrameworkElement)e.MouseDevice.Target;
                ClassBox movingClassBox = (ClassBox)movingClass.DataContext;

                Canvas canvas = FindParentOfType<Canvas>(movingClass);
                Point mousePosition = Mouse.GetPosition(canvas);

                mousePosition.X -= offsetPosition.X;
                mousePosition.Y -= offsetPosition.Y;

                moveClassBoxPoint.X = movingClassBox.X = (int)oldPosX + (int)mousePosition.X;
                moveClassBoxPoint.Y =movingClassBox.Y= (int)oldPosY + (int)mousePosition.Y;

                // Updating the edges associated with the classbox being moved
                foreach (Edge edge in Edges)
                {
                    if (movingClassBox.Equals(edge.EndA))
                    {
                        edge.Points = new Edge(movingClassBox, edge.EndB).Points; 
                    }
                    if(movingClassBox.Equals(edge.EndB))
                    {
                        edge.Points = new Edge(edge.EndA, movingClassBox).Points; 
                    }
                }
            }
        }

        // Benyttes til at flytte punkter og tilføje kanter.
        public void MouseUpClassBox(MouseButtonEventArgs e)
        {
            FrameworkElement movingClass = (FrameworkElement)e.MouseDevice.Target;
            ClassBox movingClassBox = (ClassBox)movingClass.DataContext;

            if (isAddingEdge)
            {
                // Hvis det er den første klasse der er blevet trykket på under tilføjelsen af kanten, så gemmes punktet bare og punktet bliver markeret som valgt.
                if (addingEdgeEndA == null)
                {
                    addingEdgeEndA = movingClassBox;
                    addingEdgeEndA.IsSelected = true;
                }
                // Ellers hvis det ikke er den første og de to noder der hører til klasserne er forskellige, så oprettes kanten med kommando.
                else if (addingEdgeEndA != movingClassBox)
                {
                    undoRedoController.AddAndExecute(new AddEdgeCommand(Edges, addingEdgeEndA, (ClassBox)movingClass.DataContext));
                    // De tilhørende værdier nulstilles.
                    isAddingEdge = false;
                    RaisePropertyChanged("ModeOpacity");
                    addingEdgeEndA.IsSelected = false;
                    addingEdgeEndA = null;
                }
            }
            else if(moveClassBoxPoint != default(Point))
            {
                    Canvas canvas = FindParentOfType<Canvas>(movingClass);
                    Point mousePosition = Mouse.GetPosition(canvas);
                    undoRedoController.AddAndExecute(new MoveClassBoxCommand(movingClassBox, Edges, movingClassBox.X, movingClassBox.Y, (int)oldPosX, (int)oldPosY));
                    // Nulstil værdier.
                    moveClassBoxPoint = new Point();
                    // Musen frigøres.
                    e.MouseDevice.Target.ReleaseMouseCapture();
            }
            else                    
                e.MouseDevice.Target.ReleaseMouseCapture();
            
        }

        // Rekursiv metode der benyttes til at finde et af et grafisk elements forfædre ved hjælp af typen, der ledes højere og højere op indtil en af typen findes.
        // Syntaksen "() ? () : ()" betyder hvis den første del bliver sand så skal værdien være den anden del, ellers skal den være den tredje del.
        private static T FindParentOfType<T>(DependencyObject o)
        {
            dynamic parent = VisualTreeHelper.GetParent(o);
            return parent.GetType().IsAssignableFrom(typeof(T)) ? parent : FindParentOfType<T>(parent);
        }
        // Bruges til at gøre punkterne gennemsigtige når en ny kant tilføjes.
        public double ModeOpacity
        {
            get
            {
                return isAddingEdge ? 0.4 : 1.0;
            }
        }
    }
}