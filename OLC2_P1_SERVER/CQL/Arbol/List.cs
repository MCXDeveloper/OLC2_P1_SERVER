using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class List
{
    private readonly object inner_type;

    public List(object inner_type)
    {
        this.inner_type = inner_type;
    }

}