using System.Xml.Serialization;

namespace MyReg
{
    public class Val
    {
        public string name;
        public string type;
        public string value;

        public Val()
        {
        }

        public Val(string name,string type, string value)
        {
            this.name = name;
            this.value = value;
            this.type = type;
        }
    }
}
