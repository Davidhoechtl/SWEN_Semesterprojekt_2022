
namespace MonsterTradingCardGame_Hoechtl.Handler
{

    using MonsterTradingCardGame_Hoechtl.Handler.HttpAttributes;
    using MonsterTradingCardGame_Hoechtl.Infrastructure;
    using Newtonsoft.Json;
    using System.Reflection;

    internal class HandlerMethodResolver
    {
        public HttpResponse InvokeHandlerMethod(IHandler handler, HttpMethod httpMethod, string[] pathData, string jsonContent)
        {
            string methodName = GetMethodNameFromMethodPath(pathData);
            Type implentation = handler.GetType();

            foreach (MethodInfo methodInfo in implentation.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
            {
                HttpAttribute attr = methodInfo.GetCustomAttributes(typeof(HttpAttribute), true).FirstOrDefault() as HttpAttribute;
                if (attr == null)
                {
                    // Method without a HttpAttribute
                    continue;
                }

                if (attr.Method == httpMethod &&
                   methodName.Equals(methodInfo.Name, StringComparison.Ordinal))
                {
                    ParameterInfo[] parameters = methodInfo.GetParameters();
                    object[] parsedParameter = ParseParameter(parameters.FirstOrDefault(), jsonContent);

                    try
                    {
                        object returnValue = methodInfo.Invoke(handler, parsedParameter);

                        if (returnValue is not HttpResponse)
                        {
                            throw new Exception("Method was excecuted succsessfully but the return vlaue is not a HttpResponse.");
                        }

                        return returnValue as HttpResponse;
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
                }
            }

            return null;
        }

        private object[] ParseParameter(ParameterInfo firstParameter, string jsonContent)
        {
            if(firstParameter == null)
            {
                return Array.Empty<object>();
            }
            else
            {
                Type parameterType = firstParameter.ParameterType;
                object parsedParameter = JsonConvert.DeserializeObject(jsonContent, parameterType);
                return new object[] { parsedParameter };
            }
        }

        private string GetMethodNameFromMethodPath(string[] pathData)
        {
            return pathData.Skip(1).First();
        }
    }
}
