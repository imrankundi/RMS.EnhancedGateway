namespace RMS.Component.Wmi
{
    public class WmiClassInfo
    {
        public string Namespace { get; private set; }
        public string Name { get; private set; }
        public WmiClassInfo(string @namespace, string name)
        {
            Namespace = @namespace;
            Name = name;
        }
    }
}
