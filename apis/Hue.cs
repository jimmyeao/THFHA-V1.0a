﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using THFHA_V1._0.Model;

namespace THFHA_V1._0.apis
{
    public class HueModule : IModule
    {
        private string name = "Hue";
        private bool isEnabled = false;
        private string state = "Disconnected";

        public string Name
        {
            get { return name; }
        }

        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }

        public string State
        {
            get { return state; }
            set { state = value; }
        }

        public HueModule()
        {
            // Initialize your module here
        }
    }

}
