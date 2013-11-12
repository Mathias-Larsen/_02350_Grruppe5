using _02350_Gruppe5.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02350_Gruppe5.Command
{
    class RemoveAttCommand : IUndoRedoCommand
    {
        private ClassBox classbox;
        private ClassBox.attOrMethodName att;

        public RemoveAttCommand(ClassBox _classbox, ClassBox.attOrMethodName _att) 
        {
            classbox = _classbox;
            att = _att;
        }

        public void Execute()
        {
            classbox.removeAtt(att);
        }

        public void UnExecute()
        {
            classbox.addAtt(att);
        }
    }
}
