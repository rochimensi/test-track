using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTrack.Models
{
    interface IAuditable
    {
        DateTime CreatedOn { get; set; }
        DateTime? LastModified { get; set; }
    }
}
