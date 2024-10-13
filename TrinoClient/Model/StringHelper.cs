using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace TrinoClient.Model
{
    public class StringHelper
    {
        #region Private Fields

        private List<KeyValuePair<string, object>> Values;

        #endregion

        #region Public Properties

        public Type Type { get; }

        #endregion

        #region Constructors

        private StringHelper(Type type)
        {
            Type = type;
            Values = [];
        }

        #endregion

        #region Public Methods

        public static StringHelper Build(object baseObject)
        {
            return new StringHelper(baseObject.GetType());
        }

        public StringHelper Add(string parameterName, object value)
        {
            Values.Add(new KeyValuePair<string, object>(parameterName, value));
            return this;
        }


        public override string ToString()
        {
            StringBuilder SB = new();
            SB.Append($"{Type.Name} {{");

            foreach (KeyValuePair<string, object> Item in Values)
            {
                object Value = Item.Value;

                if (typeof(IEnumerable).IsAssignableFrom(Value.GetType()))
                {
                    SB.Append($"{Item.Key}=[{string.Join(",", (IList)Value)}], ");
                }
                else
                {
                    SB.Append($"{Item.Key}={Value.ToString()}, ");
                }
            }

            SB.Length -= 2; // Remove the last space and comma
            SB.Append("}");

            return SB.ToString();
        }
        #endregion
    }
}
