using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelToBinary
{
    class HrConfData
    {
        public void Load(string strFileName)
        {
            ByteBufferRead btBufferRead = new ByteBufferRead(strFileName, Encoding.UTF8);
            int nFlag = btBufferRead.ReadInt();
            if (nFlag == 123456)
            {
                int nSheetCount = btBufferRead.ReadInt();
                {
                    for (int i = 0; i < nSheetCount; ++i)
                    {
                        ByteSheetData sheet = new ByteSheetData();
                        sheet.Load(btBufferRead);
                    }
                }
            }
        }
    }
}
