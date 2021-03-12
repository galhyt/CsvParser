using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CsvParser
{
    public class CsvParser : IEnumerable<CsvRow>
    {
        private string fpath;
        private StreamReader file;
        private bool _IsEmpty = false;
        private int _Count = 0;
        private string[] fields;

        public CsvParser(string _fpath)
        {
            fpath = _fpath;
            try
            {
                file = new StreamReader(fpath);
                if (file.Peek() == -1) _IsEmpty = true;

                // read titles
                string titlesRow = file.ReadLine();
                fields = titlesRow.Split(',');

                // calculates Count
                while (file.ReadLine() != null)
                {
                    _Count++;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        ~CsvParser()
        {
            file.Close();
            file.Dispose();
        }

        public bool IsEmpty
        {
            get
            {
                return _IsEmpty;
            }
        }

        public int Count
        {
            get
            {
                return _Count;
            }
        }

        public dynamic this[int n]
        {
            get
            {
                if (n >= _Count) return null;

                var enumarator = GetEnumerator();
                string row = null;
                for (int i = 0; i <= n; i++)
                {
                    enumarator.MoveNext();
                }

                return enumarator.Current;
            }
        }

        public List<CsvRow> WhereEquals(string field, string value)
        {
            return WhereClause(row => row[field] == value);
        }

        public List<CsvRow> WhereEquals(string field, int value)
        {
            return WhereEquals(field, value.ToString());
        }
        public List<CsvRow> WhereEquals(string field, double value)
        {
            return WhereEquals(field, value.ToString());
        }

        public List<CsvRow> WhereGreaterThan(string field, int value)
        {
            return WhereClause(row => Convert.ToInt32(row[field]) > value);
        }

        public List<CsvRow> WhereGreaterThan(string field, double value)
        {
            return WhereClause(row => Convert.ToDouble(row[field]) > value);
        }

        public List<CsvRow> WhereLessThan(string field, int value)
        {
            return WhereClause(row => Convert.ToInt32(row[field]) < value);
        }

        public List<CsvRow> WhereLessThan(string field, double value)
        {
            return WhereClause(row => Convert.ToDouble(row[field]) < value);
        }

        private List<CsvRow> WhereClause(Func<CsvRow, bool> lambda)
        {
            var ret = (from CsvRow row in this
                       where lambda(row)
                       select row);
            if (ret == null) return null;

            return ret.ToList();
        }

        public IEnumerator<CsvRow> GetEnumerator()
        {
            return new CsvParserEnumarator(file, fields);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    class CsvParserEnumarator : IEnumerator<CsvRow>
    {
        StreamReader file;
        CsvRow row = null;
        string[] fields;
        public CsvParserEnumarator(StreamReader _file, string[] _fields)
        {
            file = _file;
            fields = _fields;
            Reset();
        }

        public CsvRow Current => row;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public bool MoveNext()
        {
            if (file.Peek() == -1) return false;
            row = CsvRow.GetCsvRow(fields, file.ReadLine());

            return true;
        }

        public void Reset()
        {
            file.DiscardBufferedData();
            file.BaseStream.Seek(0, SeekOrigin.Begin);
            file.ReadLine(); // reads first titles line
            row = null;
        }
    }
}
