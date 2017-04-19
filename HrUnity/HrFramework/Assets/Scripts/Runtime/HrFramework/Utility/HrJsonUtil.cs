using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.Text;

namespace Hr
{
    public class HrJsonUtil
    {

        public static void SaveJsonFile(string strFilePath, Dictionary<string, string> dicJsonData)
        {
            StringBuilder sb = new StringBuilder();
            JsonWriter writer = new JsonWriter(sb);

            writer.WriteObjectStart();

            foreach (var iteJsonData in dicJsonData)
            {
                writer.WritePropertyName(iteJsonData.Key);
                writer.Write(iteJsonData.Value);
            }

            writer.WriteObjectEnd();
        }
    }
}

