using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class ListType
{
    private readonly TipoDato inner_type;

    public ListType(TipoDato inner_type)
    {
        this.inner_type = inner_type;
    }
}