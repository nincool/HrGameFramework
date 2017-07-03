using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Hr.Utility
{
    public class HrByteBuffer
    {

        public string FilePath
        {
            get;
            set;
        }

        protected FileMode fileMode
        {
            get;
            set;
        }

        protected FileStream m_fileStream = null;


        public HrByteBuffer(string strFilePath)
        {
            FilePath = strFilePath;    
        }

        public bool OpenFile()
        {
            Destory();
            
            if (!string.IsNullOrEmpty(FilePath))
            {
                m_fileStream = new FileStream(FilePath, fileMode);

                return true;
            }

            return false;
        }

        public void Destory()
        {
            if(m_fileStream != null)
            {
                m_fileStream.Close();
                m_fileStream = null;
            }
        }

        public void SaveFile()
        {
            if (m_fileStream != null)
            {
                m_fileStream.Flush();
            }
        }
    }

    public class HrByteBufferWriter : HrByteBuffer
    {
        public HrByteBufferWriter(string strFilePath) : base(strFilePath)
        {
            fileMode = FileMode.Create;

            OpenFile();
        }

        ~HrByteBufferWriter()
        {
            SaveFile();
            Destory();
        }

        public void WriteShort(Int16 nValue)
        {
            byte[] bytes = new byte[sizeof(Int16)];
            bytes = BitConverter.GetBytes(nValue);

            if (m_fileStream != null)
            {
                m_fileStream.Write(bytes, 0, sizeof(Int16));
            }
        }

        public void WriteUShort(UInt16 nValue)
        {
            byte[] bytes = new byte[sizeof(UInt16)];
            bytes = BitConverter.GetBytes(nValue);

            if (m_fileStream != null)
            {
                m_fileStream.Write(bytes, 0, sizeof(UInt16));
            }
        }

        public void WriteInt(Int32 nValue)
        {
            byte[] bytes = new byte[sizeof(Int32)];
            bytes = BitConverter.GetBytes(nValue);

            if (m_fileStream != null)
            {
                m_fileStream.Write(bytes, 0, sizeof(Int32));
            }
        }

        public void WriteUInt(UInt32 nValue)
        {
            byte[] bytes = new byte[sizeof(UInt32)];
            bytes = BitConverter.GetBytes(nValue);

            if (m_fileStream != null)
            {
                m_fileStream.Write(bytes, 0, sizeof(UInt32));
            }
        }

        public void WriteInt64(Int64 nValue)
        {
            byte[] bytes = new byte[sizeof(Int64)];
            bytes = BitConverter.GetBytes(nValue);

            if (m_fileStream != null)
            {
                m_fileStream.Write(bytes, 0, sizeof(Int64));
            }
        }

        public void WriteUint64(UInt64 nValue)
        {
            byte[] bytes = new byte[sizeof(UInt64)];
            bytes = BitConverter.GetBytes(nValue);

            if (m_fileStream != null)
            {
                m_fileStream.Write(bytes, 0, sizeof(UInt64));
            }
        }

        public void WriteFloat(float fValue)
        {
            byte[] bytes = BitConverter.GetBytes(fValue);
            if (m_fileStream != null)
            {
                m_fileStream.Write(bytes, 0, sizeof(float));
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

            if (m_fileStream != null)
            {
                m_fileStream.Write(bytes, 0, sizeof(byte));
            }
        }

        public void WriteString(string strValue)
        {
            if (strValue == null)
            {
                WriteUShort(0);
                return;
            }

            byte[] bytes = Encoding.UTF8.GetBytes(strValue);

            int nLength = bytes.Length;
            if (nLength > 0)
            {
                WriteUShort((UInt16)nLength);
                m_fileStream.Write(bytes, 0, nLength);
            }
            else
            {
                WriteUShort(0);
            }
        }
    }
    
    public class HrByteBufferReader : HrByteBuffer
    {
        public HrByteBufferReader(string strFilePath) : base(strFilePath)
        {
            fileMode = FileMode.Open;
            OpenFile();
        }

        ~HrByteBufferReader()
        {
            Destory();
        }

        public Int16 ReadShort()
        {
            Int16 nValue = 0;
            byte[] bytes = new byte[sizeof(Int64)];
            if (m_fileStream != null && (m_fileStream.Read(bytes, 0, sizeof(Int16)) == sizeof(Int16)))
            {
                nValue = BitConverter.ToInt16(bytes, 0);
            }
            return nValue;
        }

        public UInt16 ReadUnshort()
        {
            UInt16 nValue = 0;
            byte[] bytes = new byte[sizeof(UInt16)];
            if (m_fileStream != null && (m_fileStream.Read(bytes, 0, sizeof(UInt16)) == sizeof(UInt16)))
            {
                nValue = BitConverter.ToUInt16(bytes, 0);
            }
            return nValue;
        }

        public Int32 ReadInt()
        {
            Int32 nValue = 0;
            byte[] bytes = new byte[sizeof(Int32)];
            if (m_fileStream != null && (m_fileStream.Read(bytes, 0, sizeof(Int32)) == sizeof(Int32)))
            {
                nValue = BitConverter.ToInt32(bytes, 0);
            }
            return nValue;
        }

        public UInt32 ReadUInt()
        {
            UInt32 nValue = 0;
            byte[] bytes = new byte[sizeof(UInt32)];
            if (m_fileStream != null && (m_fileStream.Read(bytes, 0, sizeof(UInt32)) == sizeof(UInt32)))
            {
                nValue = BitConverter.ToUInt32(bytes, 0);
            }
            return nValue;
        }

        public Int64 ReadInt64()
        {
            Int64 nValue = 0;
            byte[] bytes = new byte[sizeof(Int64)];
            if (m_fileStream != null && (m_fileStream.Read(bytes, 0, sizeof(Int64)) == sizeof(Int64)))
            {
                nValue = BitConverter.ToUInt32(bytes, 0);
            }
            return nValue;
        }

        public UInt64 ReadUInt64()
        {
            UInt64 nValue = 0;
            byte[] bytes = new byte[sizeof(UInt64)];
            if (m_fileStream != null && (m_fileStream.Read(bytes, 0, sizeof(UInt64)) == sizeof(UInt64)))
            {
                nValue = BitConverter.ToUInt64(bytes, 0);
            }
            return nValue;
        }

        public bool ReadBool()
        {
            return ReadByte() == 1;
        }

        public byte ReadByte()
        {
            byte nValue = 0;
            byte[] bytes = new byte[sizeof(byte)];
            if (m_fileStream != null && (m_fileStream.Read(bytes, 0, sizeof(byte)) == sizeof(byte)))
                nValue = bytes[0];

            return nValue;
        }

        public string ReadString()
        {
            short nLength = ReadShort();
            byte[] buffer = new byte[nLength];
            m_fileStream.Read(buffer, 0, nLength);

            return Encoding.UTF8.GetString(buffer);
        }

        public float ReadFloat()
        {
            float fValue = 0f;
            byte[] bytes = new byte[sizeof(float)];
            if (m_fileStream != null && (m_fileStream.Read(bytes, 0, sizeof(float)) == sizeof(float)))
                fValue = BitConverter.ToSingle(bytes, 0);

            return fValue;
        }
    }
}
