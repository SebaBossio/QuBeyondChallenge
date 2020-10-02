using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTO
{
    public class ResponseDTO
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public object Value { get; set; }
    }
}
