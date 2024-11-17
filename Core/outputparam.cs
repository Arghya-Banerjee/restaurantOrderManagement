using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    [AttributeUsage(AttributeTargets.Property)]
    public class outputparam : Attribute
    {
        public ParamType ParamType { get; set; }
    }

    public enum ParamType
    {
        Input = 0,
        Output = 1,
        InputOutput = 2
    }
}
