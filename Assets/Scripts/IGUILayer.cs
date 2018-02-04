using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    interface IGUILayer
    {
        void Draw();
        void resetData();
        bool getFlag();
    }
}
