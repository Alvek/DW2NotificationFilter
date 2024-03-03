using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationFilter
{
    class SubPropertyDescriptor : PropertyDescriptor
    {
        private PropertyDescriptor _parent;
        private PropertyDescriptor _child;

        public SubPropertyDescriptor(PropertyDescriptor parent, PropertyDescriptor child, string propertyDescriptorName)
            : base(propertyDescriptorName, null)
        {
            _child = child;
            _parent = parent;
        }
        //in this example I have made this read-only, but you can set this to false to allow two-way data-binding
        public override bool IsReadOnly { get { return true; } }
        public override void ResetValue(object component) { }
        public override bool CanResetValue(object component) { return false; }
        public override bool ShouldSerializeValue(object component) { return true; }
        public override Type ComponentType { get { return _parent.ComponentType; } }
        public override Type PropertyType { get { return _child.PropertyType; } }
        //this is how the value for the property 'described' is accessed
        public override object GetValue(object component)
        {
            return _child.GetValue(_parent.GetValue(component));
        }
        /*My example has the read-only value set to true, so a full implementation of the SetValue() function is not necessary.  
        However, for two-day binding this must be fully implemented similar to the above method. */
        public override void SetValue(object component, object value)
        {
            //READ ONLY
            /*Example:  _child.SetValue(_parent.GetValue(component), value);
              Add any event fires or other additional functions here to handle a data update*/
        }
    }
    class MyClassTypeDescriptors : CustomTypeDescriptor
    {
        Type typeProp;

        public MyClassTypeDescriptors(ICustomTypeDescriptor parent, Type type)
            : base(parent)
        {
            typeProp = type;
        }
        //This method will add the additional properties to the object.  
        //It helps to think of the various PropertyDescriptors are columns in a database table
        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            PropertyDescriptorCollection cols = base.GetProperties(attributes);
            string propName = ""; //empty string to be populated later
                                  //find the matching property in the type being called.
            foreach (PropertyDescriptor col in cols)
            {
                if (col.PropertyType.Name == typeProp.Name)
                    propName = col.Name;
            }
            PropertyDescriptor pd = cols[propName];
            PropertyDescriptorCollection children = pd.GetChildProperties(); //expand the child object

            PropertyDescriptor[] propDescripts = new PropertyDescriptor[cols.Count + children.Count];
            int count = cols.Count; //start adding at the last index of the array
            cols.CopyTo(propDescripts, 0);
            //creation of the 'descriptor strings'
            foreach (PropertyDescriptor cpd in children)
            {
                propDescripts[count] = new SubPropertyDescriptor(pd, cpd, pd.Name + "_" + cpd.Name);
                count++;
            }

            PropertyDescriptorCollection newCols = new PropertyDescriptorCollection(propDescripts);
            return newCols;
        }
    }
    class MyClassTypeDescProvider<T> : TypeDescriptionProvider
    {
        private ICustomTypeDescriptor td;

        public MyClassTypeDescProvider()
            : this(TypeDescriptor.GetProvider(typeof(T)))
        { }

        public MyClassTypeDescProvider(TypeDescriptionProvider parent)
            : base(parent)
        { }

        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
        {
            if (td == null)
            {
                td = base.GetTypeDescriptor(objectType, instance);
                td = new MyClassTypeDescriptors(td, typeof(T));
            }
            return td;
        }
    }
}
