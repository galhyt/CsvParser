using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;

namespace CsvParser
{
    public class CsvRow: DynamicDictionary<string>
    {
        public static dynamic GetCsvRow(string[] fields,string row)
        {
            CsvRow ret = new CsvRow();
            string[] vals = row.Split(',');
            for (int i = 0; i < fields.Count(); i++)
            {
                string f = fields[i];
                ret[f] = vals[i];
            }

            return ret;
        }
    }

    public class DynamicDictionary<TValue> : DynamicObject
    {
        private Dictionary<string, TValue> _dictionary;

        public DynamicDictionary()
        {
            _dictionary = new Dictionary<string, TValue>();
        }

        public TValue this[string field]
        {
            get
            {
                return _dictionary[field];
            }

            set
            {
                _dictionary[field] = value;
            }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            TValue data;
            if (!_dictionary.TryGetValue(binder.Name, out data))
            {
                throw new KeyNotFoundException("There's no key by that name");
            }

            result = (TValue)data;

            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (_dictionary.ContainsKey(binder.Name))
            {
                _dictionary[binder.Name] = (TValue)value;
            }
            else
            {
                _dictionary.Add(binder.Name, (TValue)value);
            }

            return true;
        }
    }

}