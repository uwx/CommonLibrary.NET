using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLibrary.ModelManager
{
    public interface IModelService
    {
        BoolMessageItem<Models> Process(ModelContext ctx);
    }
}
