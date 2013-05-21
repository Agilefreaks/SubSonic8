namespace Client.Common.Models.Subsonic
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Text;

    [DataContract]
    public abstract class SerializableModelBase<T> : MediaModelBase
        where T : MediaModelBase
    {
        #region Public Methods and Operators

        public string Serialize()
        {
            var dataContractSerializer = new DataContractSerializer(typeof(T));
            string data;
            using (var memoryStream = new MemoryStream())
            {
                dataContractSerializer.WriteObject(memoryStream, this);
                memoryStream.Flush();
                memoryStream.Position = 0;
                var streamReader = new StreamReader(memoryStream, Encoding.UTF8);
                data = streamReader.ReadToEnd();
            }

            return data;
        }

        #endregion

        #region Methods

        protected static T Deserialize(string data)
        {
            return Deserialize(data, new Type[0]);
        }

        protected static T Deserialize(string data, Type[] typesOfChildren)
        {
            T result = null;
            var dataContractSerializer = new DataContractSerializer(typeof(T), typesOfChildren);
            if (data != null)
            {
                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(data)))
                {
                    try
                    {
                        result = dataContractSerializer.ReadObject(ms) as T;
                    }
                    catch (Exception)
                    {
                        result = null;
                    }
                }
            }

            return result;
        }

        #endregion
    }
}