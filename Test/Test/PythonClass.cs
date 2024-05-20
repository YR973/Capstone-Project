using Python.Runtime;

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
            var pythonScript = Py.Import("llmneuralsearch");
            var pyquery = new PyString(query);
            var r = pythonScript.InvokeMethod("search", new PyObject[] { pyquery });
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
            var pythonScript = Py.Import("additem");
            var pyname = new PyString(name);
            var pydescription = new PyString(description);
            var pyid = new PyInt(id);
            pythonScript.InvokeMethod("add", new PyObject[] {pyname, pydescription, pyid});
        }
    }
}