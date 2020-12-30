using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;

namespace DiamondFlasher
{
    public abstract class Context
    {

        private static readonly Dictionary<Type, Dictionary<string, (Delegate deleg, ContextMethodAttribute attribute)>> commands;
        private static readonly Stack<Type> contexts = new Stack<Type>();
      
        static Context()
        {
            commands = (from type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(item => item.GetTypes())
                        from method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                        from attribute in  method.GetCustomAttributes<ContextMethodAttribute>()
                        where attribute!=null
                        let deleg = method.CreateDelegate(typeof(Action<,>).MakeGenericType(typeof(string[]), method.GetParameters()[1].ParameterType))        
                        group (deleg,attribute) by attribute.Type)
                        
              .ToDictionary(item => item.Key, item =>item.ToDictionary(element=>element.attribute.Name,element=>element) );
        }
      
        public static Dictionary<string,(Delegate delg,ContextMethodAttribute attribute)> GetCommands(Type type)
        {
            return (from cmd in commands
                    where cmd.Key.IsAssignableFrom(type)
                    from element in cmd.Value
                    select element)
              .ToDictionary(item => item.Key, item => item.Value);

        }
        public static Type GetUpperContexType()
        {
            return contexts.Peek();
        }
        public static void ExitContext<T>()
        {
            var first= contexts.Peek();
            if (first == typeof(T))
                contexts.Pop();
            else
            {
                throw new ArgumentException("Wrong context type");
            }
 
        }
        public static void LoadContext<T>(T context,string input=null)
            where T:Context
        {
            if(contexts.Contains(typeof(T)))
            {
                throw new InvalidOperationException("Recursioned contexts");
            }
            contexts.Push(typeof(T));
            ConsoleHelper.WriteLine($"Context: <{Styles.Answer}>{typeof(T).Name}</color>", Styles.Information);
            do
            {

            
                string line;
                var curContext = GetCommands(typeof(T));
                line =input?? ConsoleHelper.ReadLine();
                string[] split = new Regex("(\".*?\")|(^.*?(?= |$))").Matches(line).Select(item => item.Value).ToArray();
             
                if (curContext.TryGetValue(split[0], out var value))
                {
                    if(split.Length-1<value.attribute.MinimumArguments)
                    {
                        ConsoleHelper.WriteLine($"This command reguires at least {value.attribute.MinimumArguments} arguments", Styles.BadInformation);
                        continue;
                    }
                    Regex regex = new Regex("(?<=\").*?(?=\")");
                    bool failed = false;
                    string[] arguments = split.Skip(1)
                        .Select(item =>
                        {
                            var res = regex.Match(item);
                            if (res.Success == false)
                            {
                                failed = true;
                                return null;
                            }
                            else return res.Value;

                        }).ToArray();
                    if(failed)
                    {
                        ConsoleHelper.WriteLine("Wrong argument style",Styles.BadInformation);
                    }
                    else
                    {
                        try
                        {
                            value.delg.DynamicInvoke(arguments, context);
                        }
                        catch(TargetInvocationException e)
                        {
                            ConsoleHelper.Write($"Error occured when trying to do ",Styles.BadInformation);
                            ConsoleHelper.WriteLine($"{e.InnerException.Message}", Styles.Error);
                        }
                       
                    }
                  
                }
                else
                {
                    ConsoleHelper.WriteLine($"Uknown command, type <{Styles.Command}>help</color> to see available commands",Styles.BadInformation);
                }

            } while (input==null&&contexts.Contains(typeof(T)));
           

        }
    }
}
