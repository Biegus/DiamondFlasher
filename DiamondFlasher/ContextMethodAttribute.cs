using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondFlasher
{
   
    [AttributeUsage(AttributeTargets.Method,AllowMultiple =true)]
    public class ContextMethodAttribute : Attribute
    {
        public Type Type { get; }
        public string Name { get; }
        public string[] ArgumentsDescription { get; set; } = new string[0];
        public string Description { get; set; }
        public byte MinimumArguments { get; set; } = 0;
        public ContextMethodAttribute(Type type,string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            if (typeof(Context).IsAssignableFrom(type) == false)
                throw new ArgumentException(nameof(type), "type should delivered from Context");
            this.Type = type;
        }
    }
}
