using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.Profiling;
using Debug = UnityEngine.Debug;
using System.Globalization;

namespace d2
{
    public class DatasheetStream
    {
        private static readonly char Separator = '\t';
        
        private string _data;
        private int _index;
        private int _end;

        public void SetSource(StringSpan span)
        {
            _data = span.str;
            _index = span.index;
            _end = span.index + span.length;
        }

        public string NextString()
        {
            int endIndex = _index;
            while (endIndex < _end && _data[endIndex] != Separator)
                endIndex++;
            int length = endIndex - _index;
            string result = null;
            if (length != 0)
                result = _data.Substring(_index, length);
            _index = endIndex + 1;
            return result;
        }

        public void Read(ref int result)
        {
            if (_index >= _end)
            {
                return;
            }
            if (_data[_index] == Separator)
            {
                _index++;
                return;
            }
            int sign;
            char c = _data[_index++];
            if (c == '-')
            {
                sign = -1;
                result = 0;
            }
            else
            {
                sign = 1;
                result = c - '0';
            }

            while (_index < _end && _data[_index] != Separator)
            {
                result = result * 10 + (_data[_index] - '0');
                _index++;
            }

            result *= sign;
            _index++; // skip tab
        }

        public void Read(ref uint result)
        {
            if (_index >= _end)
            {
                return;
            }
            if (_data[_index] == Separator)
            {
                _index++;
                return;
            }

            if (_index < _end && _data[_index] != Separator)
                result = 0;
            while (_index < _end && _data[_index] != Separator)
            {
                result = result * 10 + (uint)(_data[_index] - '0');
                _index++;
            }

            _index++; // skip tab
        }
        
        public void Read(ref string result)
        {
            var value = NextString();
            if (value == null || value == "xxx")
                return;
            result = value;
        }

        public void Read(ref bool result)
        {
            if (_index >= _end)
            {
                return;
            }
            if (_data[_index] == Separator)
            {
                _index++;
                return;
            }
            
            result = _data[_index] != '0';
            _index += 2; // skip tab
        }

        public void Read(ref float result)
        {
            var value = NextString();
            if (value == null || value == "xxx")
                return;
            result = (float) Convert.ToDouble(value, CultureInfo.InvariantCulture);
        }
    }

    public struct StringSpan
    {
        public readonly string str;
        public readonly int index;
        public readonly int length;

        public StringSpan(string str, int index, int length)
        {
            this.str = str;
            this.index = index;
            this.length = length;
        }

        public override string ToString()
        {
            return str.Substring(index, length);
        }
    }

    public class DatasheetLineSplitter
    {
        private readonly string _str;
        private int _index;

        public DatasheetLineSplitter(string str)
        {
            _str = str;
            _index = 0;
        }

        public bool ReadLine(ref StringSpan result)
        {
            if (_index >= _str.Length)
                return false;
            int startIndex = _index;
            int length = ReadLine();
            result = new StringSpan(_str, startIndex, length);
            return true;
        }

        private int ReadLine()
        {
            int length = 0;
            while (_index < _str.Length && !IsSeparator(_str[_index]))
            {
                length++;
                _index++;
            }
            while (_index < _str.Length && IsSeparator(_str[_index]))
            {
                _index++;
            }
            return length;
        }

        public void Skip(int count)
        {
            for (int i = 0; i < count; ++i)
                ReadLine();
        }

        private bool IsSeparator(char c)
        {
            return c == '\n' || c == '\r';
        }
    }

    public class DataMgr
    {
        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
        public class Sequence : Attribute
        {
            public int length;
        }

        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
        public class Record : Attribute
        {
        }

        public static List<T> Load<T>(string filename, int headerLines, Action<T, DatasheetStream> loader) where T : new()
        {
            string csv = File.ReadAllText(filename);

            var splitter = new DatasheetLineSplitter(csv);
            splitter.Skip(headerLines);
            var records = new List<T>(1024);
            int lineIndex = headerLines;
            StringSpan line = new StringSpan();
            var stream = new DatasheetStream();
            while(splitter.ReadLine(ref line))
            {
                if (line.length == 0)
                    continue;

                stream.SetSource(line);
                T obj = new T();
                try
                {
                    loader.Invoke(obj, stream);
                }
                catch (Exception e)
                {
                    throw new Exception("Datasheet parsing error at line " + lineIndex + ": " + line.str.Substring(0, 32), e);
                }
                records.Add(obj);
            }
            Debug.Log("Load " + filename + " (" + records.Count + " records");
            return records;
        }
    }
}
