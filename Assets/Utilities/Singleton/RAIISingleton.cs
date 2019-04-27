using System.Reflection;
using System;

public class RAIISingleton<T> where T : class
{
    static T instance;

    protected static T Instance
    {
        get
        {
            if (instance == null)
                instance = Construct();
            return instance;
        }
    }

    static T Construct()
    {
        Type t = typeof(T);

        ConstructorInfo ci = t.GetConstructor(
                                 BindingFlags.Instance | BindingFlags.NonPublic,
                                 null, new Type[0], null);

        return (T)ci.Invoke(new object[0]);
    }

    protected RAIISingleton()
    {
    }
}
