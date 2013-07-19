using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Diagnostics;
using System.Runtime.Serialization;
using System;

namespace eStoreViewModels
{
    public class eStoreViewModels
    {
        public static void ErrorRoutine(Exception e, string obj, string method)
        {
            if (e.InnerException != null)
            {
                Debug.WriteLine("Error in eStoreViewModels, objects = " + obj +
                    ", method = " + method +
                    ", inner exception message = " +
                    e.InnerException.Message, EventLogEntryType.Error);
                throw e.InnerException;
            }
            else
            {
                Debug.WriteLine("Error in eStoreViewModels, objects = " + obj +
                    ", method = " + method + ", message = " +
                    e.Message, EventLogEntryType.Error);
                throw e;
            }
        }

        public static byte[] Serializer(Object inObject)
        {
            BinaryFormatter frm = new BinaryFormatter();
            MemoryStream strm = new MemoryStream();
            frm.Serialize(strm, inObject);
            byte[] ByteArrayObject = strm.ToArray();
            return ByteArrayObject;
        }

        public static Object Deserializer(byte[] ByteArrayIn)
        {
            BinaryFormatter frm = new BinaryFormatter();
            MemoryStream strm = new MemoryStream(ByteArrayIn);
            Object returnObject = frm.Deserialize(strm);
            return returnObject;
        }
    }
}
