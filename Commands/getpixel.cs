﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CommanderLibr.Commands
{
    class commandTemplate : Command
    {
        public string commString { get; set; }
        public commandTemplate()
        {
            commString = this.GetType().Name;
        }

        public void Called(string[] args)
        {

        }





    }
}
