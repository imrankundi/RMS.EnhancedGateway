namespace RMS.Component.Wmi
{
    public abstract class WmiBaseType
    {
        public WmiClassInfo ClassInfo { get; }
        public WmiBaseType(WmiClassInfo classInfo)
        {
            ClassInfo = classInfo;
        }
    }
}
