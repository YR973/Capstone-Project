using Python.Runtime;
using Test;
using System;
using System.Diagnostics;
using System.IO;
using Test.Pages;
using System.Linq;
using Microsoft.VisualBasic.CompilerServices;
using Org.BouncyCastle.Utilities;

namespace Test
{

    public class PythonClass
    {
        public static string[] RunScript(string query)
        {
            if (!PythonEngine.IsInitialized)
            {
                string scriptPath = @"..\Test\Pages";
                string modulePath = @"C:\inetpub\wwwroot\Test2";
                
                PythonEngine.Initialize();
                dynamic sys = Py.Import("sys");
                sys
                    .path
                    .extend(".");
                sys
                    .path
                    .append(modulePath);
                sys
                    .path
                    .append(scriptPath);
            }
            //string scriptName = "llmneuralsearch";
            var pythonScript = Py.Import("llmneuralsearch");
            var pyquery = new PyString(query);
            var r = pythonScript.InvokeMethod("search", new PyObject[] { pyquery });
            //dynamic pyStr = Py.Import("builtins").GetAttr("str");
            var resultArray = 
                Py
                    .Import("builtins")
                    .GetAttr("str")
                    .Invoke(r)
                    .As<string>()
                    .Trim('[', ']')
                    .Split(',');
            return resultArray;
        }
        
        public static void AddItem(string name,string description,int id)
        {
            if (!PythonEngine.IsInitialized)
            {
                string scriptPath = @"..\Test\Pages";
                string modulePath = @"C:\Users\yfroo\Documents\GitHub\hello\Capstone-Project\Test\Test\Pages\modulespy";
                
                PythonEngine.Initialize();
                dynamic sys = Py.Import("sys");
                sys
                    .path
                    .extend(".");
                sys
                    .path
                    .append(modulePath);
                sys
                    .path
                    .append(scriptPath);
            }
            var pythonScript = Py.Import("additem");
            var pyname = new PyString(name);
            var pydescription = new PyString(description);
            var pyid = new PyInt(id);
            pythonScript.InvokeMethod("add", new PyObject[] {pyname, pydescription, pyid});
        }
        
        // public static string[] Suggest(string query)
        // {
        //     if (!PythonEngine.IsInitialized)
        //     {
        //         string scriptPath = @"..\Test\Pages";
        //         string modulePath = @"..\Test\modulespy";
        //         PythonEngine.Initialize();
        //         dynamic sys = Py.Import("sys");
        //         sys
        //             .path
        //             .extend(".");
        //         sys
        //             .path
        //             .append(modulePath);
        //         sys
        //             .path
        //             .append(scriptPath);
        //     }
        //     //string scriptName = "llmsuggestions";
        //     var pythonScript = Py.Import("llmsuggestions");
        //     var pyquery = new PyString(query);
        //     var r = pythonScript.InvokeMethod("search", new PyObject[] { pyquery });
        //     //dynamic pyStr = Py.Import("builtins").GetAttr("str");
        //     var resultArray = 
        //         Py.Import("builtins")
        //             .GetAttr("str")
        //             .Invoke(r)
        //             .As<string>()
        //             .Trim('[', ']')
        //             .Split(',');
        //     return resultArray;
        //         
        // }
    }
}