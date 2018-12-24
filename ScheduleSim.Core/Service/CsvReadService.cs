using ScheduleSim.Core.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Core.Service
{
    public class CsvReadService<T> : ICsvReadService<T>
    {
        private ICsvToModelConverter<T> converter;

        public CsvReadService(
            ICsvToModelConverter<T> converter)
        {
            this.converter = converter;
        }

        public IEnumerable<T> Read(string filePath)
        {
            // ファイルオープン
            using (var sr = new StreamReader(File.Open(filePath, FileMode.Open), Encoding.GetEncoding("shift-jis")))
            {
                // ヘッダ行を読み込み
                var header = sr.ReadLine();
                this.converter.SettingHeader(header);

                // 末尾まで1行ずつ変換しながら読み込み
                while (sr.Peek() > 0)
                {
                    var text = sr.ReadLine();
                    if (string.IsNullOrWhiteSpace(text)) break;
                    yield return this.converter.Convert(text);
                }
            }
        }
    }
}
