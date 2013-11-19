using _02350_Gruppe5.Command;
using _02350_Gruppe5.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
        private Point offsetPosition; //Bruges så klassen bliver flyttet flot rundt
        private int oldPosX;
        private int oldPosY;

        public ObservableCollection<ClassBox> ClassBoxs { get; set; }
        public ObservableCollection<Edge> Edges { get; set; }
        public ObservableCollection<ClassBox> SelectedClassBox { get; set; }
        public Edge selectedEdge;
        public ClassBox toPaste;


        // Kommandoer som UI bindes til.
        public ICommand UndoCommand { get; private set; }
        public ICommand RedoCommand { get; private set; }

        // Kommandoer som UI bindes til.
        public ICommand AddClassCommand { get; private set; }
        public ICommand RemoveClassCommand { get; private set; }
        public ICommand AddEdgeCommand { get; private set; }

        // Kommandoer som UI bindes til.
        public ICommand MouseDownClassBoxCommand { get; private set; }
        public ICommand MouseMoveClassBoxCommand { get; private set; }
        public ICommand MouseUpClassBoxCommand { get; private set; }

        public ICommand PasteClassCommand { get; private set; }
        public ICommand CopyClassCommand { get; private set; }
        public ICommand SaveProgram { get; private set; }
        public ICommand OpenProgram { get; private set; }

        public ICommand AddMethodComm { get; private set; }
        public ICommand AddAttComm { get; private set; }

        public ICommand MouseDownEdgeCommand { get; private set; }
        public ICommand MouseUpEdgeCommand { get; private set; }

        public ICommand DeleteCommand { get; private set; }

        public MainViewModel()
        {

            SelectedClassBox = new ObservableCollection<ClassBox>();
            ClassBoxs = new ObservableCollection<ClassBox>();
            Edges = new ObservableCollection<Edge>();

            // Kommandoerne som UI kan kaldes bindes til de metoder der skal kaldes. Her vidersendes metode kaldne til UndoRedoControlleren.
            UndoCommand = new RelayCommand(undoRedoController.Undo, undoRedoController.CanUndo);
            RedoCommand = new RelayCommand(undoRedoController.Redo, undoRedoController.CanRedo);

            // Kommandoerne som UI kan kaldes bindes til de metoder der skal kaldes.
            AddClassCommand = new RelayCommand(AddClassBox);
            RemoveClassCommand = new RelayCommand(RemoveClassBox, SelectedClass);
            AddEdgeCommand = new RelayCommand(AddEdge, canAddEdge);

            MouseDownEdgeCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownEdge);
            MouseUpEdgeCommand = new RelayCommand<MouseButtonEventArgs>(MouseUpEdge);

            // Kommandoerne som UI kan kaldes bindes til de metoder der skal kaldes.
            MouseDownClassBoxCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownClassBox);
            MouseMoveClassBoxCommand = new RelayCommand<MouseEventArgs>(MouseMoveClassBox);
            MouseUpClassBoxCommand = new RelayCommand<MouseButtonEventArgs>(MouseUpClassBox);

            CopyClassCommand = new RelayCommand(CopyClass, SelectedClass);
            PasteClassCommand = new RelayCommand(PasteClass, CanPaste);
            SaveProgram = new RelayCommand(saveProgram);
            OpenProgram = new RelayCommand(openProgram);

            AddMethodComm = new RelayCommand(addMethod, SelectedClass);
            AddAttComm = new RelayCommand(addAtt, SelectedClass);

            DeleteCommand = new RelayCommand(DeleteEdgeAndClass, SelectedClassOrEdge);

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
        public void openProgram()
        {
            new OpenCommand(ClassBoxs, Edges);
        }
        public void AddClassBox()
        {
            undoRedoController.AddAndExecute(new AddClassCommand(ClassBoxs));
        }
        public void PasteClass()
        {
            undoRedoController.AddAndExecute(new PasteClassCommand(ClassBoxs, toPaste));
            toPaste = null;
        }
        public void CopyClass()
        {
            toPaste = SelectedClassBox.ElementAt(0);
        }
        public bool SelectedClass()
        {
            return SelectedClassBox.Count == 1;
        }
        public bool SelectedClassOrEdge()
        {
            if (SelectedClassBox.Count == 1){return true;}
            else if (selectedEdge != null) { return true; }
            else{return false;}
        }
        public bool CanPaste()
        {
            return toPaste != null;
        }
        public bool canAddEdge()
        {
            return ClassBoxs.Count >= 2;
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
        public void DeleteEdgeAndClass()
        {
            if (selectedEdge != null)
            {
                undoRedoController.AddAndExecute(new RemoveEdgesCommand(Edges, selectedEdge));
                selectedEdge = null;
            }
            else if (SelectedClassBox.Count == 1)
            {
                undoRedoController.AddAndExecute(new RemoveClassCommand(ClassBoxs, Edges, SelectedClassBox.ElementAt(0)));
                SelectedClassBox.Clear();
            }
        }

        public void MouseDownEdge(MouseButtonEventArgs e)
        {
            if (!isAddingEdge)
            {
                if (SelectedClassBox.Count == 1)
                {
                    SelectedClassBox.ElementAt(0).IsSelected = false;
                    SelectedClassBox.Clear();
                }
                e.MouseDevice.Target.CaptureMouse();
                FrameworkElement edgeElement = (FrameworkElement)e.MouseDevice.Target;
                Edge edge = (Edge)edgeElement.DataContext;
                edge.IsSelected = true;
                if (selectedEdge != null)
                {
                    selectedEdge.IsSelected = false;
                }
                selectedEdge = edge;
            }
        }
        public void MouseUpEdge(MouseButtonEventArgs e)
        {
            e.MouseDevice.Target.ReleaseMouseCapture();
        }

        // Hvis der ikke er ved at blive tilføjet en kant så fanges musen når en musetast trykkes ned. Dette bruges til at flytte punkter.
        public void MouseDownClassBox(MouseButtonEventArgs e)
        {
            if (!isAddingEdge)
            {
                if (selectedEdge != null)
                {
                    selectedEdge.IsSelected = false;
                    selectedEdge = null;
                }

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

                if (oldPosX + mousePosition.X >= 0){moveClassBoxPoint.X = movingClassBox.X = oldPosX + (int)mousePosition.X;}
                else{ moveClassBoxPoint.X = movingClassBox.X = 0;}

                if (oldPosY + mousePosition.Y >= 0){moveClassBoxPoint.Y = movingClassBox.Y = oldPosY + (int)mousePosition.Y;}
                else{moveClassBoxPoint.Y = movingClassBox.Y = 0;}

                // Updating the edges associated with the classbox being moved
                foreach (Edge edge in Edges)
                {
                    if (movingClassBox.Equals(edge.EndA))
                    {
                        edge.Points = new Edge(movingClassBox, edge.EndB).Points;
                    }
                    if (movingClassBox.Equals(edge.EndB))
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
            else if (moveClassBoxPoint != default(Point))
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
        public double ModeOpacity{get{return isAddingEdge ? 0.4 : 1.0;}}

        /////////////////////////////////////////////Save/////////////////////////////////////////////

        private bool _saveIsRunning = false;
        public event AsyncCompletedEventHandler SaveCompleted;
        private readonly object _sync = new object();

        public bool IsBusy
        {
            get { return _saveIsRunning; }
        }
        private delegate void saveProgramDelegate();
        private void saveProgram()
        {
            new SaveCommand(ClassBoxs, Edges);
        }
        public void MyTaskAsync()
        {
            saveProgramDelegate worker = new saveProgramDelegate(saveProgram);
            AsyncCallback completedCallback = new AsyncCallback(SaveCompletedCallback);

            lock (_sync)
            {
                if (_saveIsRunning)
                    throw new InvalidOperationException("The control is currently busy.");

                AsyncOperation async = AsyncOperationManager.CreateOperation(null);
                worker.BeginInvoke(completedCallback, async);
                _saveIsRunning = true;
            }
        }
        private void SaveCompletedCallback(IAsyncResult ar)
        {
            // get the original worker delegate and the AsyncOperation instance
            saveProgramDelegate worker = (saveProgramDelegate)((AsyncResult)ar).AsyncDelegate;
            AsyncOperation async = (AsyncOperation)ar.AsyncState;

            // finish the asynchronous operation
            worker.EndInvoke(ar);

            // clear the running task flag
            lock (_sync)
            {
                _saveIsRunning = false;
            }

            // raise the completed event
            AsyncCompletedEventArgs completedArgs = new AsyncCompletedEventArgs(null, false, null);
            async.PostOperationCompleted(
              delegate(object e) { OnSaveCompleted((AsyncCompletedEventArgs)e); },
              completedArgs);
        }
        protected virtual void OnSaveCompleted(AsyncCompletedEventArgs e)
        {
            if (SaveCompleted != null)
                SaveCompleted(this, e);
        }



    }
}