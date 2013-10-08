using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02350_Gruppe5.Model
{
    // Kant klassen har en reference til de to noder som den er imellem.
    public class Edge : NotifyBase
    {
        // Properties.
        private ClassBox endA;
        private ClassBox endB;
        private int connection_xA, connection_yA, connection_xB, connection_yB;
        public ClassBox EndA { 
            get { return endA; } 
            set { endA = value; NotifyPropertyChanged("EndA"); } }
        public ClassBox EndB { 
            get { return endB; } 
            set { endB = value; NotifyPropertyChanged("EndB"); } }
        public int Connection_xA
        {
            get {
                int centrum_xA, centrum_yA,centrum_xB, centrum_yB;
 
                return 0;
            }
        }
    }
}
