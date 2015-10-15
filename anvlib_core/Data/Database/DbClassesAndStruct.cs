using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace anvlib.Data.Database
{
    internal struct DbColumnInformation
    {
        public bool IsNullable { get; set; }
        public int MaxLength { get; set; }
        public int Precision { get; set; }
        public int Sacale { get; set; }
        public object DefaultValue { get; set; }
        public object AdditionalInfo { get; set; }
    }

    public enum DataInsertMethod
    { 
        /// <summary>
        /// Обычная вставка, не очень быстро, зато надежно.
        /// </summary>
        Normal,
        /// <summary>
        /// Если у СУБД есть методы быстрой встаки данных, то быстро,
        /// но если будут ошибки вставки, то строки просто пропустятся и 
        /// даже не будет сообщений об ошибках
        /// </summary>
        FastIfPossible
    }
}
