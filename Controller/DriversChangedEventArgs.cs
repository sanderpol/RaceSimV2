using System;
using System.Collections.Generic;
using System.Text;
using Model;

namespace Controller
{
    public class DriversChangedEventArgs : EventArgs
    {
        public Track Track { get; set; }
    }
}
