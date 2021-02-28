namespace Haro.AdminPanel.Utilities.Object
{
    public static class PropertyCopy
    {
        public static void Copy<TSource, TTarget>(TSource source, TTarget target)
            where TSource : class
            where TTarget : class
        {
            PropertyCopier<TSource, TTarget>.Copy(source, target);
        }
    }
}