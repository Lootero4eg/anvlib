using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Reflection;

namespace anvlib.zzzSamples_Reminders
{
    /*
     * Для нормально сериализации коллекции объектов(классов) в дизайнере, 
     * необходимо чтобы у объекта был открытый пустой конструктор
     * иначе дизайнер не запишет изменямые в объекте данные.
     * Если по каким то причинам нужно будет использовать другой параметризированный
     * конструктор, то неоходимо воспользоваться конвертером типов(TypeConverter)
     * маленький не рабочий пример см. ниже
     * 
     * и не забывай, что при описании "свойства" твоей коллекции в классе
     * необходимо указать аттрибут
     * [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
     * 
     * Рабочий реализованный пример это TypedTreeViewDisplayMember
     */

    internal class TypedTreeViewMemberTypeConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            //return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
            /*if (destinationType == typeof(TypedTreeViewDisplayMemberNew))
                return true;*/

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            /*if (destinationType == typeof(InstanceDescriptor) && value is TypedTreeViewDisplayMemberNew)
            {
                ConstructorInfo constructor = typeof(TypedTreeViewDisplayMemberNew).GetConstructor(System.Type.EmptyTypes);//new[] { typeof(string), typeof(string)});
                var tvdml = value as TypedTreeViewDisplayMemberNew;
                var descriptor = new InstanceDescriptor(constructor, System.Type.EmptyTypes, true);

                return descriptor;
            }*/

            /*if (destinationType == typeof(TypedTreeViewDisplayMemberNew)) 
            {
                if (value == null)
                    return new TypedTreeViewDisplayMemberNew("", "");


            }*/

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
