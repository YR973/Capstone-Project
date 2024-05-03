using Microsoft.AspNetCore.Mvc;

namespace Test;

public class SuggestionFetch
{
   
        public int[] OnGet(string query)
        {
            
            var suggestions = PythonClass.Suggest(query);

            return suggestions;
        }
    
}