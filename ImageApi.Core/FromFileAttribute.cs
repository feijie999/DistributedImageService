using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ImageApi.Core
{
    public class FromFileAttribute : Attribute, IBindingSourceMetadata
    {
        public BindingSource BindingSource => BindingSource.FormFile;
    }
}
