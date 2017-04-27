using System.IO;
using System.Collections.Generic;
using System;
using System.Text;
using System.Reflection;

public abstract class ISaveable
{
    public virtual void Save(ByteBuffer buffer)
    {
        SaveAuto(buffer);
    }

    public virtual bool CheckValid() { return true; }

    public void SaveAuto(ByteBuffer buffer)
    {
        var properties = GetType().GetProperties();

        foreach(var v in properties)
        {
            SavePropertyValue(v, buffer);
        }
    }

    private  void SavePropertyValue(PropertyInfo property,ByteBuffer buffer)
    {
        var value = property.GetValue(this, new object[] { });
        if (property.PropertyType == typeof(int))
        {
            buffer.WriteInt((int)value);
        }
        else if (property.PropertyType == typeof(string))
        {
            buffer.WriteString2((string)value);
        }
        else if (property.PropertyType == typeof(uint))
        {
            buffer.WriteUInt((uint)value);
        }
        else if (property.PropertyType == typeof(short))
        {
            buffer.WriteShort((short)value);
        }
        else if (property.PropertyType == typeof(ushort))
        {
            buffer.WriteUShort((ushort)value);
        }
        else if (property.PropertyType == typeof(byte))
        {
            buffer.WriteByte((byte)value);
        }
        else if (property.PropertyType == typeof(float))
        {
            buffer.WriteFloat((float)value);
        }
        else if (property.PropertyType == typeof(bool))
        {
            buffer.WriteBool((bool)value);
        }
        else
        {
            throw new Exception("Invalid property type : " + property.PropertyType.ToString());
        }
    }
}

public class ByteBuffer
{

    public ByteBuffer()
    {

    }

    private Encoding m_CurrentEncoding = Encoding.UTF8;
    private FileStream m_fsSource = null;

    public ByteBuffer(byte[] bytes)
    {
        m_fsSource = new MemoryStream(bytes);
    }

    public ByteBuffer(string strPath, bool bOpen, Encoding encoding)
    {
        m_CurrentEncoding = encoding;
        bool bRes = false;
        if (bOpen)
        {
            bRes = Open(strPath);
        }
        else
        {
            bRes = OpenOverWrite(strPath);
        }
        if (!bRes)
        {
            throw new Exception(String.Format("打开文件【{0}】失败！", strPath));
        }
    }

    public ByteBuffer(string strPath, bool bOpen)
    {
        m_CurrentEncoding = StaticData.EncodingSetting.Instance.Current;
        bool bRes = false;
        if (bOpen)
        {
            bRes = Open(strPath);
        }
        else
        {
            bRes = OpenOverWrite(strPath);
        }
        if (!bRes)
        {
            throw new Exception(String.Format("打开文件【{0}】失败！", strPath));
        }
    }


    public bool IsOpen()
    {
        return m_fsSource != null;
    }

    // Closes a file. 
    public void Close()
    {
        if (m_fsSource != null)
        {
            m_fsSource.Close();
            m_fsSource = null;
        }
    }

    public bool Open(string strPath)
    {
        Close();
        if (strPath != null && strPath.Length > 0)
        {
            m_fsSource = new FileStream(strPath, FileMode.Open);
        }
        return m_fsSource != null;
    }

    public bool OpenOverWrite(string strPath)
    {
        Close();
        if (strPath != null && strPath.Length > 0)
        {
            m_fsSource = new FileStream(strPath, FileMode.Create);
        }
        return m_fsSource != null;
    }

    public void Save()
    {
        m_fsSource.Flush();
    }

    public void ReadShort(ref Int16 nValue)
    {
        byte[] bytes = new byte[sizeof(Int16)];
        if (m_fsSource != null && (m_fsSource.Read(bytes, 0, sizeof(Int16)) == sizeof(Int16)))
            nValue = BitConverter.ToInt16(bytes, 0);
        else
            nValue = 0;
    }

    public Int16 ReadShort()
    {
        Int16 v = 0;
        ReadShort(ref v);
        return v;
    }

    public void ReadUShort(ref UInt16 nValue)
    {
        byte[] bytes = new byte[sizeof(UInt16)];
        if (m_fsSource != null && (m_fsSource.Read(bytes, 0, sizeof(UInt16)) == sizeof(UInt16)))
            nValue = BitConverter.ToUInt16(bytes, 0);
        else
            nValue = 0;
    }

    public UInt16 ReadUShort()
    {
        UInt16 v = 0;
        ReadUShort(ref v);
        return v;
    }

    public void ReadInt(ref Int32 nValue)
    {
        byte[] bytes = new byte[sizeof(Int32)];
        if (m_fsSource != null && (m_fsSource.Read(bytes, 0, sizeof(Int32)) == sizeof(Int32)))
            nValue = BitConverter.ToInt32(bytes, 0);
        else
            nValue = 0;
    }

    public int ReadInt()
    {
        int v = 0;
        ReadInt(ref v);
        return v;
    }

    public void ReadUInt(ref UInt32 nValue)
    {
        byte[] bytes = new byte[sizeof(UInt32)];
        if (m_fsSource != null && (m_fsSource.Read(bytes, 0, sizeof(UInt32)) == sizeof(UInt32)))
            nValue = BitConverter.ToUInt32(bytes, 0);
        else
            nValue = 0;
    }

    public uint ReadUInt()
    {
        uint nValue = 0;
        ReadUInt(ref nValue);
        return nValue;
    }

    public void ReadInt64(ref Int64 nValue)
    {
        byte[] bytes = new byte[sizeof(Int64)];
        if (m_fsSource != null && (m_fsSource.Read(bytes, 0, sizeof(Int64)) == sizeof(Int64)))
            nValue = BitConverter.ToInt64(bytes, 0);
        else
            nValue = 0;
    }

    public void ReadBool(ref bool bValue)
    {
        byte nByte = 0;
        ReadByte(ref nByte);
        bValue = (nByte != 0);
    }

    public void ReadByte(ref byte nValue)
    {
        byte[] bytes = new byte[sizeof(byte)];
        if (m_fsSource != null && (m_fsSource.Read(bytes, 0, sizeof(byte)) == sizeof(byte)))
            nValue = bytes[0];
        else
            nValue = 0;
    }

    public byte ReadByte()
    {
        byte bValue = 0;
        ReadByte(ref bValue);
        return bValue;
    }

    public string ReadString()
    {
        short nSize = 0;
        ReadShort(ref nSize);
        byte[] buff = new byte[nSize];
        m_fsSource.Read(buff, 0, nSize);
        return m_CurrentEncoding.GetString(buff);
    }

    public void ReadString(ref string strOut, UInt16 nSize)
    {
        strOut = "";
        if (nSize > 0)
        {
            byte[] bytes = new byte[nSize];
            if (m_fsSource != null && (m_fsSource.Read(bytes, 0, nSize) == nSize))
            {
                strOut = m_CurrentEncoding.GetString(bytes);
            }
        }
    }

    public void ReadString2(ref string strOut, UInt16 nSize)
    {
        strOut = "";
        if (nSize > 0)
        {
            byte[] bytes = new byte[nSize];
            if (m_fsSource != null && (m_fsSource.Read(bytes, 0, nSize) == nSize))
            {
                strOut = m_CurrentEncoding.GetString(bytes);
            }
        }
    }


    public void WriteShort(Int16 nValue)
    {
        byte[] bytes = new byte[sizeof(Int16)];
        bytes = BitConverter.GetBytes(nValue);

        if (m_fsSource != null)
        {
            m_fsSource.Write(bytes, 0, sizeof(Int16));
        }
    }

    public void WriteUShort(UInt16 nValue)
    {
        byte[] bytes = new byte[sizeof(UInt16)];
        bytes = BitConverter.GetBytes(nValue);

        if (m_fsSource != null)
        {
            m_fsSource.Write(bytes, 0, sizeof(UInt16));
        }
    }

    public void WriteInt(Int32 nValue)
    {
        byte[] bytes = new byte[sizeof(Int32)];
        bytes = BitConverter.GetBytes(nValue);

        if (m_fsSource != null)
        {
            m_fsSource.Write(bytes, 0, sizeof(Int32));
        }
    }

    public void WriteFloat(float fValue)
    {
        byte[] bytes = BitConverter.GetBytes(fValue);
        if (m_fsSource != null)
        {
            m_fsSource.Write(bytes, 0, sizeof(float));
        }
    }

    public void WriteUInt(UInt32 nValue)
    {
        byte[] bytes = new byte[sizeof(UInt32)];
        bytes = BitConverter.GetBytes(nValue);

        if (m_fsSource != null)
        {
            m_fsSource.Write(bytes, 0, sizeof(UInt32));
        }
    }

    public void WriteInt64(Int64 nValue)
    {
        byte[] bytes = new byte[sizeof(Int64)];
        bytes = BitConverter.GetBytes(nValue);

        if (m_fsSource != null)
        {
            m_fsSource.Write(bytes, 0, sizeof(Int64));
        }
    }

    public void WriteBool(bool bValue)
    {
        byte nByte = 0;
        if (bValue)
        {
            nByte = 1;
        }
        WriteByte(nByte);
    }

    public void WriteByte(byte nValue)
    {
        byte[] bytes = new byte[sizeof(byte)];
        bytes[0] = nValue;

        if (m_fsSource != null)
        {
            m_fsSource.Write(bytes, 0, sizeof(byte));
        }
    }

    public void WriteString2(string strIn)
    {
        if (strIn == null)
        {
            WriteUShort(0);
            return;
        }

        byte[] bytes = m_CurrentEncoding.GetBytes(strIn);

        int nLength = bytes.Length;
        if (nLength > 0)
        {
            WriteUShort((UInt16)nLength);
            m_fsSource.Write(bytes, 0, nLength);
        }
        else
            WriteUShort(0);
    }

    public void WriteStringServer(string strIn)
    {
        if (strIn == null)
        {
            WriteUShort(0);
            WriteByte(0);
            return;
        }

        byte[] bytes = m_CurrentEncoding.GetBytes(strIn);

        int nLength = bytes.Length;
        if (nLength > 0)
        {
            WriteUShort((UInt16)nLength);
            m_fsSource.Write(bytes, 0, nLength);
            WriteByte(0);
        }
        else
        {
            WriteUShort(0);
            WriteByte(0);
        }
    }

    public void WriteList(List<string> list)
    {
        if (list == null)
        {
            WriteUShort(0);
            return;
        }

        WriteUShort((ushort)list.Count);
        list.ForEach((x) =>
        {
            WriteString2(x);
        });
    }

    public void WriteList<T>(List<T> list) where T : struct
    {
        if (list == null)
        {
            WriteUShort(0);
            return;
        }

        Type type = typeof(T);
        Type byteType = typeof(byte);
        Type boolType = typeof(bool);
        Type shortType = typeof(short);
        Type ushortType = typeof(ushort);
        Type intType = typeof(int);
        Type uintType = typeof(uint);

        WriteUShort((ushort)list.Count);
        list.ForEach((x) =>
        {
            if (type == byteType)
                WriteByte((byte)Convert.ChangeType(x, byteType));
            else if (type == boolType)
                WriteBool((bool)Convert.ChangeType(x, boolType));
            else if (type == shortType)
                WriteShort((short)Convert.ChangeType(x, shortType));
            else if (type == ushortType)
                WriteUShort((ushort)Convert.ChangeType(x, ushortType));
            else if (type == intType)
                WriteInt((int)Convert.ChangeType(x, intType));
            else if (type == uintType)
                WriteUInt((uint)Convert.ChangeType(x, uintType));
        });
    }

    public void WriteObjectList<T>(List<T> objList) where T : ISaveable
    {
        if (objList == null)
        {
            WriteUShort(0);
            return;
        }

        WriteUShort((ushort)objList.Count);
        foreach (T obj in objList)
        {
            obj.Save(this);
        }
    }

    public void WriteObjectListWithNoSize<T>(List<T> objList) where T : ISaveable
    {
        if (objList != null)
        {
            foreach (T obj in objList)
            {
                obj.Save(this);
            }
        }
    }

    private byte[] ConverString2Byte(string strValue)
    {
        int nLen = strValue.Length;
        if (nLen > 0)
        {
            byte[] bResult = new byte[(nLen + 1) * 2];
            for (int i = 0; i < nLen; i++)
            {
                byte[] bytevalue = BitConverter.GetBytes(strValue[i]);
                bResult[i * 2] = bytevalue[0];
                bResult[i * 2 + 1] = bytevalue[1];
            }
            bResult[nLen * 2] = 0;
            bResult[nLen * 2 + 1] = 0;

            return bResult;
        }
        return null;
    }

}

