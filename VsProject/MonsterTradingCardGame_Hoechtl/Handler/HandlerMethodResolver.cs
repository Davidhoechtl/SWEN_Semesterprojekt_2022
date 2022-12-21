using MonsterTradingCardGame_Hoechtl.Handler.HttpAttributes;
using MonsterTradingCardGame_Hoechtl.Infrastructure;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Reflection;

namespace MonsterTradingCardGame_Hoechtl.Handler
{
    internal class HandlerMethodResolver
    {
        public HandlerMethodResolver(IEnumerable<IHandler> handlers)
        {
            this.handlers = handlers;
        }

        public HttpResponse InvokeHandlerMethod(Infrastructure.HttpMethod httpMethod, string methodPath, string jsonContent)
        {
            foreach (IHandler handler in handlers)
            {
                Type implentation = handler.GetType();

                foreach (MethodInfo methodInfo in implentation.GetMethods())
                {
                    HttpAttribute attr = methodInfo.GetCustomAttributes(typeof(HttpAttribute), true).FirstOrDefault() as HttpAttribute;
                    if (attr == null)
                    {
                        // Method without a HttpAttribute
                        continue;
                    }

                    if (attr.Method == httpMethod &&
                       methodPath.Equals(methodInfo.Name, StringComparison.Ordinal))
                    {
                        object[] parameters = methodInfo.GetParameters();
                        object parameterObject = ParseParameter(methodInfo.GetParameters().FirstOrDefault(), jsonContent);

                        try
                        {
                            object returnValue = methodInfo.Invoke(handler, new object[] { parameterObject });

                            if( returnValue is not HttpResponse)
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
            }
            return null;
        }

        private object ParseParameter(ParameterInfo firstParameter, string jsonContent)
        {
            Type parameterType = firstParameter.ParameterType;
            return JsonConvert.DeserializeObject(jsonContent, parameterType);
        }

        private readonly IEnumerable<IHandler> handlers;
    }
}
