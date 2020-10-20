using System;
using System.Collections.Generic;
using System.Text;

namespace CommanderLibr.Commands
{
    class getpixel : Command
    {
        public string commString { get; set; }
        public getpixel()
        {
            commString = this.GetType().Name;
        }

        public void Called(string[] args)
        {

        }





    }
}
