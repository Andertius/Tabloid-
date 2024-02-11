﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Tabloid.Core.Extensions;

public static class TypeLoaderExtensions
{
    public static IEnumerable<Type?> GetLoadableTypes(this Assembly assembly)
    {
        if (assembly is null)
        {
            ArgumentNullException.ThrowIfNull(assembly, nameof(assembly));
        }

        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException e)
        {
            return e.Types.Where(t => t is not null);
        }
    }
}