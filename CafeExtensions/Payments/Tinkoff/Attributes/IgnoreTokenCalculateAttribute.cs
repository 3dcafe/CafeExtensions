using System;

namespace MorePayments.Payment.Tinkoff.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreTokenCalculateAttribute : Attribute
    {
        public IgnoreTokenCalculateAttribute() { }
    }
}
