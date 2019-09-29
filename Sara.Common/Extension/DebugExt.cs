namespace Sara.Common.Extension
{
    public static class DebugExt
    {
        public static void D(this object @object)
        {
            System.Diagnostics.Debug.Print(@object.ToString());
        }
        public static void D(this object obj, string description)
        {
            System.Diagnostics.Debug.Print($"{description}: [{obj}]");
        }
        public static string GetMessageName(this object obj)
        {
            if (obj is System.Type)
                return ((System.Type)obj).FullName;
            return obj.GetType().FullName;
        }
    }
}
