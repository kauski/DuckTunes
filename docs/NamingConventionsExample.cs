using System; 

namespace CodingStyleExample //namespaces using Pascal case
{
    //Class names using Pascal Case, Interfaces using IPascal case
    class Program : IMyInterface
    {
        //Static fields at top, if private use camel case with s_ prefix, if public use Pascal case
        public static float MyStaticValue;
        private static float s_myStaticValue;

        //Private variables using _camelCase
        int _myNumber;

        //Public variables using Pascal case
        public int MySecondNumber;

        //Const using Pascal case
        public const MyConst;

        //if type of the variable is clear from the context, use var
        public var MyInt;

        //properties using Pascal case
        public int MyNumber
        {
            get
            {
                return _myNumber;
            }
            set
            {
                _myNumber = value;
            }
        }

        //events using Pascal case and On prefix
        public event Action OnMyAction;

        //delegates using Pascal case and Callback suffix
        public delegate void MyDelegateCallback();


        //Methods using Pascal case, parameters using camelCase
        static void Main(string[] args)
        {
            //in function scope variables can use simple names and camelCase
            int xValue;
            int yValue;
        }
    }

    //Structs using PascalCase
    struct MyStruct
    {
        public int PublicInt;
    }
}

//interfaces with IPascal case
public interface IMyInterface
{

}
