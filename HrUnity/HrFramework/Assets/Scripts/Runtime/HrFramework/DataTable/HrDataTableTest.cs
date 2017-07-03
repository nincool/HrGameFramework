using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.DataTable
{
    public class HrDataTableTestInfo
    {
        [DataTableMember("id")]
        public int ID
        {
            get;
            set;
        }

        [DataTableMember("sex")]
        public int Sex
        {
            get;
            set;
        }

        [DataTableMember("type")]
        public int Type
        {
            get;
            set;
        }

        [DataTableMember("action")]
        public int Action
        {
            get;
            set;
        }
    }


}
