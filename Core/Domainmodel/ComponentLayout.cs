
using System;
using System.Collections.Generic;

namespace FIVES
{
    // Represents an attribute layout for the component. Use as following:
    //   layout = new ComponentLayout();
    //   layout["attrA"] = AttributeType.INT;
    //   layout["attrB"] = AttributeType.FLOAT;
    //   layout["attrC"] = AttributeType.STRING;
    public class ComponentLayout
    {
        public ComponentLayout() {
            this.attributes = new Dictionary<string, AttributeType> ();
        }
        public AttributeType this [string name] 
        {
            get { return attributes[name]; }
            set { attributes[name] = value; }
        }

        public static bool operator ==(ComponentLayout layout_1, ComponentLayout layout_2)
        {
            bool isEqual = true;
            foreach(KeyValuePair<string, AttributeType> entry in layout_1.attributes)
            {
                isEqual = isEqual && layout_2.attributes.ContainsKey (entry.Key) && layout_2.attributes [entry.Key] == layout_1.attributes [entry.Key];
            }
            foreach(KeyValuePair<string, AttributeType> entry in layout_2.attributes)
            {
                isEqual = isEqual && layout_1.attributes.ContainsKey (entry.Key) && layout_1.attributes [entry.Key] == layout_2.attributes [entry.Key];
            }

            return isEqual;
        }

        public static bool operator !=(ComponentLayout layout_1, ComponentLayout layout_2)
        {
            return !(layout_1 == layout_2);
        }
        // We need to access this internally to be able to iterate over the list of the attributes when constructing a 
        // new component in ComponentRegistry::createComponent.
        private Guid Id { get; set; }
        internal IDictionary<string, AttributeType> attributes { get; set; }
    }
}
