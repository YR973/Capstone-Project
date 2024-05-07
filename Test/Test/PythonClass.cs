using Python.Runtime;
using Test;
using System;
using System.Diagnostics;
using System.IO;
using Test.Pages;
using System.Linq;
namespace Test
{

    public class PythonClass
    {
        // public static string[] RunScript(string query)
        // {
        //     Console.WriteLine("before engine called");
        //     if (!PythonEngine.IsInitialized)
        //     {
        //         Runtime.PythonDLL = @"..\Python311\python311.dll";
        //         
        //     }
        //     PythonEngine.Initialize();
        //     Console.WriteLine("after engine called");
        //     
        //     using (Py.GIL()) 
        //     {
        //         Console.WriteLine("before script called2");
        //         string scriptPath = @"..\Test\Pages";
        //         string modulePath = @"..\Test\modulespy";
        //         string scriptName = "llmneuralsearch";
        //         dynamic sys = Py.Import("sys");
        //         sys.path.extend(".");
        //         sys.path.append(modulePath);
        //         sys.path.append(scriptPath);
        //         var pythonScript = Py.Import(scriptName);
        //         Console.WriteLine("after script called2");
        //         var pyquery = new PyString(query);
        //         Console.WriteLine("before search called");
        //         var result = pythonScript.InvokeMethod("search", new PyObject[] {pyquery}).ToString().Trim('[', ']').Split(',');
        //         Console.WriteLine("after search called");
        //         //Py.GIL().Dispose();
        //         //PythonEngine.Initialize(false);
        //         return result;
        //     }
        //     
        // }

        // public static string[] RunScript(string query)
        // {
        //     if (!(PythonEngine.IsInitialized))
        //     {
        //         Runtime.PythonDLL = @"..\Python311\python311.dll";
        //         PythonEngine.Initialize();
        //         string[] result;
        //         string scriptPath = @"..\Test\Pages";
        //         string modulePath = @"..\Test\modulespy";
        //         string scriptName = "llmneuralsearch";
        //         dynamic sys = Py.Import("sys");
        //         sys.path.extend(".");
        //         sys.path.append(modulePath);
        //         sys.path.append(scriptPath);
        //         var pythonScript = Py.Import(scriptName);
        //         var pyquery = new PyString(query);
        //         result = pythonScript.InvokeMethod("search", new PyObject[] { pyquery })
        //             .ToString().Trim('[', ']')
        //             .Split(',');
        //         Console.WriteLine("after search called");
        //         return result;
        //     }
        //     else
        //     {
        //         string[] result;
        //         string scriptName = "llmneuralsearch";
        //         var pythonScript = Py.Import(scriptName);
        //         var pyquery = new PyString(query);
        //         result = pythonScript.InvokeMethod("search", new PyObject[] { pyquery })
        //             .ToString().Trim('[', ']')
        //             .Split(',');
        //         
        //         return result;
        //     }
        // }
        
        
        public static string[] RunScript(string query)
        {
            if (!PythonEngine.IsInitialized)
            {
                Runtime.PythonDLL = @"..\Python311\python311.dll";
            }
            Console.WriteLine("before engine called");
            
            PythonEngine.Initialize();
            Console.WriteLine("after engine called");
            using (Py.GIL()) 
            {
                Console.WriteLine("before script called2");
                string scriptPath = @"..\Test\Pages";
                string modulePath = @"..\Test\modulespy";
                string scriptName = "llmneuralsearch";
                dynamic sys = Py.Import("sys");
                sys.path.extend(".");
                sys.path.append(modulePath);
                sys.path.append(scriptPath);
                var pythonScript = Py.Import(scriptName);
                Console.WriteLine("after script called2");
                var pyquery = new PyString(query);
                Console.WriteLine("before search called");
                var result = pythonScript.InvokeMethod("search", new PyObject[] {pyquery}).ToString().Trim('[', ']').Split(',');
                Console.WriteLine("after search called");
                
                return result;
            }
            
        }
        
        public static string[] Suggest(string query)
        {
            if (!PythonEngine.IsInitialized)
            {
                Runtime.PythonDLL = @"..\Python311\python311.dll";
                PythonEngine.Initialize();
            }
            using (Py.GIL())
            {
                string scriptPath = @"Test\Pages";
                string modulePath = @"Test\modulespy";
                string scriptName = "llmsuggestions";
                dynamic sys = Py.Import("sys");
                sys.path.extend(".");
                sys.path.append(modulePath);
                sys.path.append(scriptPath);
                var pythonScript = Py.Import(scriptName);
                var pyquery = new PyString(query);
                var result = pythonScript.InvokeMethod("search", new PyObject[] {pyquery}).ToString().Trim('[', ']').Split(',');
                return result;
                
            }

            
        }
        
        
    }
    

}