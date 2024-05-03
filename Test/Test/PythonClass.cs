using Python.Runtime;
using Test;
using System;
using System.IO;
using Test.Pages;
using System.Linq;
namespace Test
{

    public class PythonClass
    {
        public static int[] RunScript(string query)
        {
            Runtime.PythonDLL = @"C:\Users\yfroo\AppData\Local\Programs\Python\Python311\python311.dll";
            PythonEngine.Initialize();
            using (Py.GIL())
            {
                string scriptPath = @"C:\Users\yfroo\Documents\GitHub\hello\Capstone-Project\Test\Test\Pages";
                string modulePath = @"C:\Users\yfroo\.virtualenvs\LangTest1-ywe_PRZU\Lib\site-packages";
                string scriptName = "llmneuralsearch";
                dynamic sys = Py.Import("sys");
                sys.path.extend("C:\\Users\\yfroo");
                sys.path.append(modulePath);
                sys.path.append(scriptPath);
                var pythonScript = Py.Import(scriptName);
                var pyquery = new PyString(query);
                var result = pythonScript.InvokeMethod("search", new PyObject[] {pyquery});
                //Console.WriteLine(result);
                string resultString = result.ToString().Trim('[', ']');
                //Console.WriteLine(resultString);
                string[] resultArray = resultString.Split(',');
                int[] idListInt = new int[20];
                for (int i = 0; i < resultArray.Length; i++)
                {
                    idListInt[i] = int.Parse(resultArray[i]);
                }
                return idListInt;
            }
        }

        public static int[] Suggest(string query = "noinput")
        {
            if (query=="noinput" || query.Length>=50)
            {
                int[] i = {-1};
                return i;
            }
            if (!PythonEngine.IsInitialized)
            {
                Runtime.PythonDLL = @"C:\Users\yfroo\AppData\Local\Programs\Python\Python311\python311.dll";
                PythonEngine.Initialize();
            }
            using (Py.GIL())
            {
                string scriptPath = @"C:\Users\yfroo\Documents\GitHub\hello\Capstone-Project\Test\Test\Pages";
                string modulePath = @"C:\Users\yfroo\.virtualenvs\LangTest1-ywe_PRZU\Lib\site-packages";
                string scriptName = "llmneuralsearch";
                dynamic sys = Py.Import("sys");
                sys.path.extend("C:\\Users\\yfroo");
                sys.path.append(modulePath);
                sys.path.append(scriptPath);
                var pythonScript = Py.Import(scriptName);
                var pyquery = new PyString(query);
                var result = pythonScript.InvokeMethod("search", new PyObject[] {pyquery});
                string resultString = result.ToString().Trim('[', ']');
                string[] resultArray = resultString.Split(',');
                int[] idListInt = new int[3];
                for (int i = 0; i < resultArray.Length; i++)
                {
                    idListInt[i] = int.Parse(resultArray[i]);
                }
                
                return idListInt;
            }

            
        }
        
        
    }
    

}