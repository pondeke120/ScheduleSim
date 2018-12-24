using ClosedXML.Excel;
using ScheduleSim.Core.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Core.Service
{
    public class XlsxReadService<T> : IXlsxReadService<T>
    {
        private ICsvToModelConverter<T> converter;

        public XlsxReadService(
            ICsvToModelConverter<T> converter)
        {
            this.converter = converter;
        }

        public IEnumerable<T> Read(string filePath)
        {
            // ブックを読み込む
            using (var workbook = new XLWorkbook(filePath))
            {
                // 1シート目を取得
                var worksheet = workbook.Worksheets.Worksheet(1);

                // ヘッダ行を読み込み
                var lastColumnNumber = worksheet.LastColumnUsed().ColumnNumber();
                var header = string.Join(",", worksheet.Row(1).Cells($"1:{lastColumnNumber}").Select(x => x.GetValue<string>()));
                this.converter.SettingHeader(header);

                // 末尾まで1行ずつ変換しながら読み込み
                var lastRow = worksheet.LastRowUsed().RowNumber();
                for (int i = 2; i <= lastRow; i++)
                {
                    var splitTexts = worksheet.Row(i).Cells($"1:{lastColumnNumber}").Select(x => x.GetValue<string>()).ToArray();
                    if (splitTexts.All(string.IsNullOrWhiteSpace)) break;
                    var text = string.Join(",", splitTexts);
                    yield return this.converter.Convert(text);
                }
            }
        }
    }
}
