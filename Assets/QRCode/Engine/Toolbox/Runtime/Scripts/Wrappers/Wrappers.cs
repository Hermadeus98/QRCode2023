namespace QRCode.Engine.Toolbox.Wrappers
{
    using System;

    public class WrapperArgs<T> : EventArgs, IWrapper<T>
    {
        //--<Argument>
        public T Arg { get; }
        
        //--<Constructors>
        public WrapperArgs(T arg) => this.Arg = arg;
    }

    public class WrapperArgs<T,U> : EventArgs, IWrapper<T, U>
    {
        //--<Arguments>
        public T Arg1 { get; }
        public U Arg2 { get; }
        
        //--<Constructors>
        public WrapperArgs(T arg1, U arg2)
        {
            this.Arg1 = arg1;
            this.Arg2 = arg2;
        }
    }
    
    public class WrapperArgs<T,U,V> : EventArgs, IWrapper<T, U, V>
    {
        //--<Argument>
        public T Arg1 { get; }
        public U Arg2 { get; }
        public V Arg3 { get; }

        //--<Constructors>
        public WrapperArgs(T arg1, U arg2, V arg3)
        {
            this.Arg1 = arg1;
            this.Arg2 = arg2;
            this.Arg3 = arg3;
        }
    }
}
