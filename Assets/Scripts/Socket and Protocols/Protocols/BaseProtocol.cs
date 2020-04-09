using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Protocol
{
    public abstract class BaseProtocol
    {

        public abstract char Identity { get; }

        public string from_client_name = "";         // this is the display from name :)

        private int chached_jsonLength = 0;

        public virtual T AsType<T>() where T : BaseProtocol
        {
            if ( this is T )
            {
                return this as T;
            }
            else
            {
                Debug.LogErrorFormat( "Unable to get {0}, as type T ({1})", GetType(), typeof( T ) );
                return null;
            }
                        
        }

        /// <summary>
        /// Gets this class in json format :)
        /// </summary>
        /// <param name="jsonLength">OUT the length of the message</param>
        /// <returns>this class as json</returns>
        public virtual string GetJson ( out int jsonLength )
        {
            string jsonStr = JsonUtility.ToJson( this );
            chached_jsonLength = jsonLength = jsonStr.Length;

            return jsonStr;
        }

        /// <summary>
        /// Gets the chached message length from the last time GetMessage was called
        /// </summary>
        /// <returns>chached message length</returns>
        public virtual int GetJsonLength ()
        {
            return chached_jsonLength;
        }

        public virtual void Send()
        {
            ClientSocket.ActiveSocket.SendMsg( this );
        }
    }
}