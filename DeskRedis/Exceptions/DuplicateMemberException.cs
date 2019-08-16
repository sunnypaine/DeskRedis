using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeskRedis.Exceptions
{
    public class DuplicateMemberException : Exception
    {
        private string message;


        #region 构造方法
        public new Exception InnerException { get; set; }


        public DuplicateMemberException()
        { }

        public DuplicateMemberException(string message) : base(message)
        {
            this.message = message;
        }

        public DuplicateMemberException(string message, Exception innerException) : base(message, innerException)
        {
            this.message = message;
            this.InnerException = innerException;
        }
        #endregion


        public string GetMessage()
        {
            return message;
        }
    }
}
