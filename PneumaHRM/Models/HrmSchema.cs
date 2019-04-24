using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PneumaHRM.Models
{
    public class HrmSchema : Schema
    {
        public HrmSchema()
        {
            Query = new HrmQuery();
            Mutation = new HrmMutation();
        }
    }
}
