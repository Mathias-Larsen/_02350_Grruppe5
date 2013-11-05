using _02350_Gruppe5.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace _02350_Gruppe5.Command
{
    class SaveCommand 
    {

        public SaveCommand(ObservableCollection<ClassBox> classBoxs, ObservableCollection<Edge> edges)
        {
            // Configure save file dialog box
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".xml"; // Default file extension
            dlg.Filter = "XML documents (.xml)|*.xml"; // Filter files by extension 

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();
            string filename = null;
            // Process save file dialog box results 
            if (result == true)
            {
                // Save document 
                filename = dlg.FileName;

                XmlSerializer serializer = new XmlSerializer(typeof(ToSave));
                TextWriter writer = new StreamWriter(filename);
                ToSave toSave = new ToSave();
                int i = 0;
                ClassBoxSave[] classes = new ClassBoxSave[classBoxs.Count];
                foreach (ClassBox classIn in classBoxs)
                {
                    ClassBoxSave cs = new ClassBoxSave();

                    cs.x = classIn.X;
                    cs.y = classIn.Y;
                    cs.width = classIn.Width;
                    cs.height = classIn.Height;
                    cs.name = classIn.ClassName;
                    cs.number = classIn.Number;
                    String[] methods = new String[classIn.MethodNames.Count];
                    String[] attributs = new String[classIn.AttNames.Count];


                    int j = 0;
                    foreach (String met in classIn.MethodNames)
                    {
                        methods[j] = met;
                        j++;
                    }

                    j = 0;
                    if (classIn.AttNames != null)
                    {
                        foreach (String att in classIn.AttNames)
                        {
                            attributs[j] = att;
                            j++;
                        }
                    }
                    cs.att = attributs;
                    cs.method = methods;
                    classes[i] = cs;
                    i++;
                }
                toSave.classes = classes;

                EdgeSave[] edge = new EdgeSave[edges.Count];

                i = 0;
                foreach (Edge edgeIn in edges)
                {
                    EdgeSave edgeToAdd = new EdgeSave();
                    edgeToAdd.a = edgeIn.EndA.Number;
                    edgeToAdd.b = edgeIn.EndB.Number;
                    edge[i] = edgeToAdd;
                    i++;
                }
                toSave.edges = edge;

                serializer.Serialize(writer, toSave);
                writer.Close();
            }
        }

    }
    [XmlRootAttribute("ToSave", Namespace = "http://dtu.programming",IsNullable = false)]
    public class ToSave
    {
        public ClassBoxSave[] classes;
        public EdgeSave[] edges;
    }
    public class ClassBoxSave
    {
        public int number;
        public int x;
        public int y;
        public int width;
        public int height;
        public string name;
        public string[] att;
        public string[] method;
    }
    public class EdgeSave
    {
        public int a;
        public int b;
    }

}

