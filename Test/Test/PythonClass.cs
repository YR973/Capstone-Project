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
                string modulePath = @"..\Test\modulespy";
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
        
        public static string[] Suggest(string query)
        {
            if (!PythonEngine.IsInitialized)
            {
                string scriptPath = @"..\Test\Pages";
                string modulePath = @"..\Test\modulespy";
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
            //string scriptName = "llmsuggestions";
            var pythonScript = Py.Import("llmsuggestions");
            var pyquery = new PyString(query);
            var r = pythonScript.InvokeMethod("search", new PyObject[] { pyquery });
            //dynamic pyStr = Py.Import("builtins").GetAttr("str");
            var resultArray = 
                Py.Import("builtins")
                    .GetAttr("str")
                    .Invoke(r)
                    .As<string>()
                    .Trim('[', ']')
                    .Split(',');
            return resultArray;
                
        }
    }
}