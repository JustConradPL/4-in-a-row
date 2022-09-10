using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4_in_a_row
{

    public enum FieldType
    {
        empty,yello,red,random
    }
    //####################### FIELDTYPE ##################################################
//####################################################################
    class FieldInfo
    {
        public FieldType fieldColor;
        public int COLUMN { get; private set; }
        public int ROW { get; private set; }
        //$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
        public FieldInfo(int x, int y)
        {
            fieldColor = FieldType.empty;
            COLUMN = x;
            ROW = y;
        }// ---------------------------------------

    }//##################### FIELDINFO ##################################################
//#######################################################################
 
}
