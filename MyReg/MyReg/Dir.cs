using System.Collections.Generic;
using System.Xml.Serialization;

namespace MyReg
{
    public class Dir
    {
        public string key;
        public string name;
        public List<Dir> sub_dirs;
        public List<Val> values;

        public Dir()
        {
        }

        public Dir(string key, string name)
        {
            this.key = key;
            this.name = name;
            sub_dirs = new List<Dir>();
            values = new List<Val>();
        }
    }
}
