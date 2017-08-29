using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using System.IO;

namespace Wenli.Data.RabbitMQ.Common
{
    public class SerializeUtil
    {
        /// <summary>
        ///     ProtolBuf序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static byte[] ProtolBufSerialize<T>(T instance)
        {
            using (var ms = new MemoryStream())
            {
                Serializer.Serialize(ms, instance);
                ms.Flush();
                return ms.ToArray();
            }
        }

        /// <summary>
        ///     ProtolBuf反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static T ProtolBufDeserialize<T>(byte[] buffer)
        {
            using (var ms = new MemoryStream(buffer))
            {
                return Serializer.Deserialize<T>(ms);
            }
        }
    }
}
