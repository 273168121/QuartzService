using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XJob.Business
{

    [Serializable]
    public class ResultInfo
    { 
        public bool IsSuccess { get; set; } = true;
        
        public string Message { get; set; }
    }
}
